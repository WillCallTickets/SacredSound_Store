using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using Wcss;

namespace WillCallWeb.Admin.AdminControls
{
    /// <summary>
    /// PanelLocal only shows up when the invoice is very old or card is expired and we will need to write a check
    /// </summary>
    public partial class Orders_Shipping : BaseControl
    {
        #region Page Overhead

        int _invoiceId = 0;
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            //set opacity for nav events
            if (this.HasControls() && this.UpdatePanel1.Visible)
                Atx.RegisterJQueryScript_BlockUI_AjaxMethod(this.UpdatePanel1, "#orderview", true);
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            string req = Request.QueryString["Inv"];
            if (req != null && Utils.Validation.IsInteger(req))
            {
                _invoiceId = int.Parse(req);
                Atx.SetCurrentInvoiceRecord(_invoiceId);
            }
        }
        protected void btnLink_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            switch (btn.CommandName.ToLower())
            {
                case "invoice":
                    base.Redirect(string.Format("/Admin/Orders.aspx?p=view&Inv={0}", _invoiceId.ToString()));
                    break;
                case "custsales":
                    base.Redirect(string.Format("/Admin/CustomerEditor.aspx?p=sales&UserName={0}", Ctx.SessionInvoice.PurchaseEmail));
                    break;
                case "shipping":
                    base.Redirect(string.Format("/Admin/Orders.aspx?p=shipping&Inv={0}", _invoiceId.ToString()));
                    break;
                case "refund":
                    base.Redirect(string.Format("/Admin/Orders.aspx?p=refund&Inv={0}", _invoiceId.ToString()));
                    break;
                case "exchange":
                    base.Redirect(string.Format("/Admin/Orders.aspx?p=exch&Inv={0}", _invoiceId.ToString()));
                    break;
            }
        }
        protected override void OnLoad(EventArgs e)
        {
            if (!IsPostBack)
            {
                GridInvoice.DataBind();
                GridItems.DataBind();
                GridShip.DataBind();
                GridShipments.DataBind();
            }

            litUserEditor.Text = (Atx.CurrentInvoiceRecord != null) ? string.Format("<a href=\"/Admin/EditUser.aspx?username={0}\" >{0}</a>",
                Atx.CurrentInvoiceRecord.AspnetUserRecord.UserName) : "&nbsp;";
        }

        #endregion

        #region GridInvoice & GridItems & GridShip

        protected void GridInvoice_DataBinding(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;

            if (grid.Rows.Count == 0)
            {
                InvoiceCollection coll = new InvoiceCollection();
                coll.Add(Atx.CurrentInvoiceRecord);
                grid.DataSource = coll;
            }
        }

        protected void GridItems_DataBinding(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;

            List<InvoiceItem> list = new List<InvoiceItem>();
            list.AddRange(Atx.CurrentInvoiceRecord.InvoiceItemRecords().GetList().FindAll(delegate(InvoiceItem match) { 
                return (match.Context != _Enums.InvoiceItemContext.damaged &&
                    match.Context != _Enums.InvoiceItemContext.processing && 
                    match.Context != _Enums.InvoiceItemContext.servicecharge && 
                    match.Context != _Enums.InvoiceItemContext.charity && 
                    match.Context != _Enums.InvoiceItemContext.shippingmerch && 
                    match.Context != _Enums.InvoiceItemContext.shippingticket);
            }));

            grid.DataSource = list;
        }
        protected void GridItems_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridView grid = (GridView)sender;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                InvoiceItem ii = (InvoiceItem)e.Row.DataItem;
                if (ii.PurchaseAction != _Enums.PurchaseActions.Purchased.ToString())
                    e.Row.Attributes.Add("style","background-color: #e1e1e1;");

                Literal description = (Literal)e.Row.FindControl("litDescription");
                if (description != null)
                    description.Text = GetItemShipText(ii, false, true);

                Literal shipped = (Literal)e.Row.FindControl("litShipped");
                if (shipped != null && ii.DateShipped < DateTime.MaxValue)
                    shipped.Text = ii.DateShipped.ToString("MM/dd/yyyy");

                Button resetDate = (Button)e.Row.FindControl("btnClearDate");
                Button resetMethod = (Button)e.Row.FindControl("btnClearMethod");
                if (resetDate != null)
                    resetDate.Enabled = ii.DateShipped < DateTime.MaxValue;
                if (resetMethod != null)
                    resetMethod.Enabled = (ii.Context == _Enums.InvoiceItemContext.ticket) && 
                        (ii.ShippingMethod != null && ii.ShippingMethod.ToLower() != ShipMethod.WillCall.ToLower());

                
                //check box only necessary for shipments and items
                if (
                    ii.Context != _Enums.InvoiceItemContext.merch && ii.Context != _Enums.InvoiceItemContext.ticket &&
                    ii.Context != _Enums.InvoiceItemContext.shippingmerch && ii.Context != _Enums.InvoiceItemContext.shippingticket)
                    e.Row.Cells[7].Controls.Clear();

                Button create = (Button)e.Row.FindControl("btnCreateShipment");
                if (create != null)
                {
                    //ship methods should allow create shipment
                    create.Visible = (ii.Context == _Enums.InvoiceItemContext.shippingmerch || ii.Context == _Enums.InvoiceItemContext.shippingticket);
                    if (create.Visible && ii.DateShipped < DateTime.MaxValue)
                        create.OnClientClick = "return confirm('This shipment has already been shipped. Would you still like to proceed?');";                    
                }
            }
        }
        protected void GridItems_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridView grid = (GridView)sender;
            string command = e.CommandName.ToLower();
            int idx = int.Parse(e.CommandArgument.ToString());
            System.Text.StringBuilder sql = new System.Text.StringBuilder();
            SubSonic.QueryCommand cmd = new SubSonic.QueryCommand(string.Empty, SubSonic.DataService.Provider.Name);

            cmd.AddParameter("@idx", idx, DbType.Int32);
            cmd.AddParameter("@willcall", ShipMethod.WillCall, DbType.String);

            string oldDate = string.Empty;
            string oldMethod = string.Empty;

            InvoiceItem ii = InvoiceItem.FetchByID(idx);

            //we are clearing info from the item - the shipment may have already been done and gone - so this does not update shipments
            switch (command)
            {
                case "cleardate"://dates can be cleared for all items

                    oldDate = (ii.DateShipped == null) ? string.Empty : ii.DateShipped.ToString();

                    sql.Append("UPDATE [InvoiceItem] SET [dtShipped] = null WHERE [Id] = @idx; ");

                    break;
                //set to will call for tix - linked and merch should ignore                    
                //except, when doing a linked item - set underlying items to will call
                case "clearmethod":

                    oldMethod = (ii.ShippingMethod) ?? string.Empty;

                    sql.AppendFormat("UPDATE [InvoiceItem] SET [ShippingMethod] = @willcall, [dtShipped] = null WHERE [Id] = @idx AND [vcContext] = '{0}'; ",
                         _Enums.InvoiceItemContext.ticket.ToString());
                    
                    break;
            }

            //rebind the info
            //reset the invoice object
            try
            {
                cmd.CommandSql = sql.ToString();

                SubSonic.DataService.ExecuteScalar(cmd);

                //log an event
                switch (command)
                {
                    case "cleardate":
                        InvoiceEvent.NewInvoiceEvent(_invoiceId, DateTime.Now, DateTime.Now, _Enums.EventQStatus.Success, this.Page.User.Identity.Name,
                            Atx.CurrentInvoiceRecord.UserId, Atx.CurrentInvoiceRecord.PurchaseEmail, _Enums.EventQContext.Invoice,
                            _Enums.EventQVerb.ChangeShipDate, oldDate, string.Empty, "ship date reset", true);
                        break;
                    case "clearmethod":
                        InvoiceEvent.NewInvoiceEvent(_invoiceId, DateTime.Now, DateTime.Now, _Enums.EventQStatus.Success, this.Page.User.Identity.Name,
                            Atx.CurrentInvoiceRecord.UserId, Atx.CurrentInvoiceRecord.PurchaseEmail, _Enums.EventQContext.Invoice,
                            _Enums.EventQVerb.ChangeShipMethod, oldMethod, ShipMethod.WillCall, "changed to will call", true);
                        break;
                }

                //refresh the current invoice
                int id = _invoiceId;
                Atx.SetCurrentInvoiceRecord(0);
                Atx.SetCurrentInvoiceRecord(id);

                //form.ChangeMode(FormViewMode.Edit);
                GridItems.DataBind();
            }
            catch (Exception ex)
            {
                _Error.LogException(ex);
            }
        }

        protected void GridShip_DataBinding(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;

            List<InvoiceItem> list = new List<InvoiceItem>();
            list.AddRange(Atx.CurrentInvoiceRecord.InvoiceItemRecords().GetList().FindAll(delegate(InvoiceItem match)
            {
                return (match.Context == _Enums.InvoiceItemContext.shippingmerch || 
                        match.Context == _Enums.InvoiceItemContext.shippingticket);
            }));

            grid.DataSource = list;
        }
        protected void GridShip_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Retrieve the LinkButton control from the first column.
                Button fulfill = (Button)e.Row.FindControl("btnFulfill");

                if (fulfill != null)
                    fulfill.CommandArgument = e.Row.RowIndex.ToString();
            }
        }
        protected void GridShip_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridView grid = (GridView)sender;

            string command = e.CommandName.ToLower();

            switch (command)
            {
                case "cleardate":
                    int idx = int.Parse(e.CommandArgument.ToString());
                    System.Text.StringBuilder buil = new System.Text.StringBuilder();

                    buil.Append("UPDATE [InvoiceItem] SET [dtShipped] = null WHERE [Id] = @idx OR [TShipItemId] = @idx ");

                    SubSonic.QueryCommand clear = new SubSonic.QueryCommand(buil.ToString(), SubSonic.DataService.Provider.Name);
                    clear.AddParameter("@idx", idx, DbType.Int32);

                    try
                    {
                        SubSonic.DataService.ExecuteScalar(clear);

                        //refresh the current invoice
                        int id = _invoiceId;
                        Atx.SetCurrentInvoiceRecord(0);
                        Atx.SetCurrentInvoiceRecord(id);

                        //form.ChangeMode(FormViewMode.Edit);
                        GridItems.DataBind();
                        GridShip.DataBind();
                        GridShipments.DataBind();//this will cascade
                        GridShipments.SelectedIndex = GridShipments.Rows.Count - 1;//select the latest entry
                    }
                    catch (Exception ex)
                    {
                        CustomValidator custom = null;

                        foreach(GridViewRow gvr in grid.Rows)
                        {
                            if(grid.DataKeys[gvr.DataItemIndex]["Id"].ToString() == idx.ToString())
                            {
                                custom = (CustomValidator)gvr.FindControl("CustomValidation");
                                break;
                            }
                        }

                        if (custom != null)
                        {
                            custom.IsValid = false;
                            custom.ErrorMessage = ex.Message;
                        }
                    }
                    break;
                case "generic":
                    FormDetails.ChangeMode(FormViewMode.Insert);
                    break;
                case "fulfill":
                    //get info from selected row
                    int rowIdx = int.Parse(e.CommandArgument.ToString());
                    int itmIdx = int.Parse(grid.DataKeys[rowIdx]["Id"].ToString());

                    //reference objects
                    Invoice _invoice = Atx.CurrentInvoiceRecord;
                    InvoiceBillShip ship = Atx.CurrentInvoiceRecord.InvoiceBillShip;
                    InvoiceItem shipRequest = (InvoiceItem)_invoice.InvoiceItemRecords().Find(itmIdx);

                    try
                    {
                        if (shipRequest == null)
                            throw new Exception("Ship request could not be found");

                        //init vars
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        System.Text.StringBuilder pack = new System.Text.StringBuilder();
                        SubSonic.QueryCommand cmd = new SubSonic.QueryCommand(string.Empty, SubSonic.DataService.Provider.Name);

                        string context = (shipRequest.Context == _Enums.InvoiceItemContext.shippingmerch) ? _Enums.ProductContext.merch.ToString() : _Enums.ProductContext.ticket.ToString();
                        string method = shipRequest.MainActName;

                        string carrier = ShipmentBatch.GetCarrierName(method);                   

                        decimal charged = shipRequest.LineItemTotal;
                        decimal weightCalc = 0;
                        decimal handlingCalc = 0;

                        //sql
                        sb.Append("DECLARE @shipId int; ");
                        sb.Append("INSERT INTO InvoiceShipment ([dtStamp],[tInvoiceId],[UserId],[dtCreated],[ReferenceNumber],[vcContext],[TShipItemId],");
                        sb.Append("[CompanyName],[FirstName],[LastName],[Address1],[Address2],[City],[StateProvince],[PostalCode],[Country],[Phone],");
                        sb.Append("[ShipMessage],[vcCarrier],[ShipMethod],[dtShipped],[TrackingInformation],[PackingList],[PackingAdditional],");
                        sb.Append("[mWeightCalculated],[mHandlingCalculated],[mShippingCharged]) ");

                        sb.Append("VALUES ((getDate()),@invoiceId,@userId,@dateCreated,@reference,@context,@shipItemId,");
                        sb.Append("@company,@first,@last,@address1,@address2,@city,@state,@zip,@country,@phone,");
                        sb.Append("@message,@carrier,@method,@dateCreated,@tracking,@packing,@additional,");
                        sb.Append("@weightCalc,@handlingCalc,@shippingCharged) ");

                        sb.Append("SET @shipId = SCOPE_IDENTITY(); ");

                        sb.AppendFormat("UPDATE InvoiceBillShip SET dtShipped = @dateCreated WHERE Id = {0}; ", ship.Id);

                        //update the shiprequest item - get rid of returned to sender
                        sb.AppendFormat("UPDATE InvoiceItem SET [ShippingMethod] = @method, [bRTS] = 0, [dtShipped] = @dateCreated WHERE [Id] = {0}; ", shipRequest.Id);


                        //loop thru items that should have this shipment and record 
                        int i = 0;
                        foreach (InvoiceItem itm in shipRequest.ChildInvoiceItemRecords())
                        {
                            if (itm.PurchaseAction.ToLower() == _Enums.PurchaseActions.Purchased.ToString().ToLower())
                            {
                                string itemId = string.Format("@itemId_{0}", i);
                                string qty = string.Format("@qty_{0}", i);
                                cmd.AddParameter(itemId, itm.Id, DbType.Int32);
                                cmd.AddParameter(qty, itm.Quantity, DbType.Int32);

                                sb.Append("INSERT INTO InvoiceShipmentItem ([dtStamp],[tInvoiceShipmentId],[tInvoiceItemId],[iQuantity]) ");
                                sb.AppendFormat("VALUES ((getDate()),@shipId,{0},{1}) ", itemId, qty);

                                sb.AppendFormat("UPDATE InvoiceItem SET [ShippingMethod] = @method, [bRTS] = 0, [dtShipped] = @dateCreated WHERE [Id] = {0}; ", itm.Id);

                                pack.AppendFormat("{0}~", GetItemShipText(itm, false, true));

                                if (itm.IsMerchandiseItem)
                                {
                                    if (itm.IsPromotionItem)
                                        weightCalc += itm.SalePromotionRecord.Weight;
                                    else
                                        weightCalc += itm.MerchRecord.Weight;

                                    handlingCalc += itm.LineItemTotal;
                                }
                            }

                            i++;
                        }

                        //assign params
                        cmd.AddParameter("@invoiceId", _invoice.Id, DbType.Int32);
                        cmd.AddParameter("@shipItemId", itmIdx, DbType.Int32);
                        cmd.AddParameter("@userId", _invoice.UserId.ToString(), DbType.String);
                        cmd.AddParameter("@dateCreated", DateTime.Now.ToString(), DbType.String);
                        cmd.AddParameter("@reference", Guid.NewGuid().ToString(), DbType.String);
                        cmd.AddParameter("@company", ship.Company_Working, DbType.String);
                        cmd.AddParameter("@first", ship.FirstName_Working, DbType.String);
                        cmd.AddParameter("@last", ship.LastName_Working, DbType.String);
                        cmd.AddParameter("@address1", ship.Address1_Working, DbType.String);
                        cmd.AddParameter("@address2", ship.Address2_Working, DbType.String);
                        cmd.AddParameter("@city", ship.City_Working, DbType.String);
                        cmd.AddParameter("@state", ship.State_Working, DbType.String);
                        cmd.AddParameter("@zip", ship.Zip_Working, DbType.String);
                        cmd.AddParameter("@country", ship.Country_Working, DbType.String);
                        cmd.AddParameter("@phone", ship.Phone_Working, DbType.String);
                        cmd.AddParameter("@message", ship.ShipMessage_Working, DbType.String);
                        cmd.AddParameter("@tracking", string.Empty, DbType.String);//TODO: to be done in update
                        cmd.AddParameter("@additional", string.Empty, DbType.String);//TODO: to be done in update
                        cmd.AddParameter("@context", context, DbType.String);
                        cmd.AddParameter("@method", method, DbType.String);
                        cmd.AddParameter("@carrier", carrier, DbType.String);
                        cmd.AddParameter("@shippingCharged", charged, DbType.String);
                        cmd.AddParameter("@packing", pack.ToString().TrimEnd('~'), DbType.String);
                        cmd.AddParameter("@weightCalc", weightCalc, DbType.Decimal);
                        cmd.AddParameter("@handlingCalc", ecommercemax_shipping.ComputeHandlingFee(method, handlingCalc), DbType.Decimal);

                        cmd.CommandSql = sb.ToString();

                    
                        SubSonic.DataService.ExecuteScalar(cmd);

                        //refresh the current invoice
                        int id = _invoiceId;
                        Atx.SetCurrentInvoiceRecord(0);
                        Atx.SetCurrentInvoiceRecord(id);

                        //form.ChangeMode(FormViewMode.Edit);
                        GridItems.DataBind();
                        GridShip.DataBind();
                        
                        GridShipments.SelectedIndex = Atx.CurrentInvoiceRecord.InvoiceShipmentRecords().Count - 1;// GridShipments.Rows.Count - 1;//select the latest entry
                        GridShipments.DataBind();//this will cascade
                    }
                    catch (Exception ex)
                    {
                        CustomValidator custom = (CustomValidator)grid.Rows[rowIdx].FindControl("CustomValidation");
                        if (custom != null)
                        {
                            custom.IsValid = false;
                            custom.ErrorMessage = ex.Message;
                        }
                    }

                    break;
            }
        }
        protected void GridShip_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridView grid = (GridView)sender;
            
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                InvoiceItem ii = (InvoiceItem)e.Row.DataItem;
                if (ii.PurchaseAction != _Enums.PurchaseActions.Purchased.ToString() || ii.ReturnedToSender)
                    e.Row.Attributes.Add("style", "background-color: Plum;");

                Literal litShipping = (Literal)e.Row.FindControl("litShipping");
                if (litShipping != null)
                {
                    litShipping.Text = ii.AdminDisplayShipMethod;
                }


                Literal returned = (Literal)e.Row.FindControl("litReturned");
                if (returned != null && ii.ReturnedToSender)
                    returned.Text = string.Format("<div style=\"font-size:12px;line-height:18px;\">***{0}</div>", ii.Notes);

                //list items slated for this shipment
                Literal itmList = (Literal)e.Row.FindControl("litItemList");
                if (itmList != null)
                {
                    foreach (InvoiceItem ent in ii.ChildInvoiceItemRecords())
                    {
                        if(ent.PurchaseAction.ToLower() == _Enums.PurchaseActions.Purchased.ToString().ToLower())
                        {
                            itmList.Text += GetItemShipText(ent, true, false);
                        }
                    }
                }

                Literal shipped = (Literal)e.Row.FindControl("litShipped");
                if (shipped != null && ii.DateShipped < DateTime.MaxValue)
                    shipped.Text = ii.DateShipped.ToString("MM/dd/yyyy");

                Button resetDate = (Button)e.Row.FindControl("btnClearDate");
                if (resetDate != null)
                    resetDate.Enabled = ii.DateShipped < DateTime.MaxValue;

                Button fulfill = (Button)e.Row.FindControl("btnFulfill");
                if (fulfill != null)
                {
                    //ship methods should allow create shipment
                    fulfill.Visible = (ii.PurchaseAction == _Enums.PurchaseActions.Purchased.ToString());
                    if (fulfill.Visible && ii.DateShipped < DateTime.MaxValue)
                        fulfill.OnClientClick = "return confirm('This shipment has already been shipped. Would you still like to proceed?');";                    
                }
            }
        }

        #endregion

        #region Actual Shipment Listing - GridShipments

        int _rowCounter;
        protected void GridShipments_DataBinding(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;

            InvoiceShipmentCollection coll = Atx.CurrentInvoiceRecord.InvoiceShipmentRecords();
            if (coll.Count > 1)
                coll.Sort("Id", true);

            grid.DataSource = coll;

            _rowCounter = 0;
        }
        protected void GridShipments_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridView grid = (GridView)sender;
            InvoiceShipment _shipment = (InvoiceShipment)e.Row.DataItem;

            if (_shipment != null)
            {
                Literal litBatch = (Literal)e.Row.FindControl("litBatch");

                if (litBatch != null && _shipment.ShipmentBatchInvoiceShipmentRecords().Count > 0)
                {
                    ShipmentBatchInvoiceShipment join = _shipment.ShipmentBatchInvoiceShipmentRecords()[0];
                    ShipmentBatch batch = join.ShipmentBatchRecord;
                    litBatch.Text = string.Format("Batch Id: {0}", batch.BatchId);
                }

                Literal ship = (Literal)e.Row.FindControl("litShipped");

                if (ship != null)
                    ship.Text = string.Format("{0}{1}{2}",
                        (_shipment.Status != null && _shipment.Status.Trim().Length > 0) ? string.Format("<div>{0}</div>", _shipment.Status) : string.Empty, 
                        (_shipment.DateShipped < DateTime.MaxValue) ? 
                        string.Format("{0}<br/>", _shipment.DateShipped.ToString("MM/dd/yyyy hh:mmtt")) : string.Empty, 
                        _shipment.ReferenceNumber.ToString());

                Literal address = (Literal)e.Row.FindControl("litAddress");

                if (address != null)
                    address.Text = string.Format("{9}<br/>{0} {1}<br/>{2}{3}<br/>{4}<br/>{5}<br/>{6}<br/>{7}<br/>{8}", 
                        _shipment.FirstName, _shipment.LastName, //1 and 2
                        _shipment.Address1, //3
                        (_shipment.Address2 != null && _shipment.Address2.Trim().Length > 0) ? string.Format("<br/>{0}", _shipment.Address2.Trim()) : string.Empty, //4
                        _shipment.City, //5
                        _shipment.StateProvince, //6
                        _shipment.PostalCode, //7
                        _shipment.Country, //8
                        _shipment.Phone, 
                        _shipment.InvoiceRecord.PurchaseEmail );//9

                Literal pack = (Literal)e.Row.FindControl("litPacking");

                if (pack != null)
                    pack.Text = string.Format("<div>{0}</div>{1}", _shipment.PackingList.Replace("~", "</div><div>"), 
                        (_shipment.PackingAdditional != null && _shipment.PackingAdditional.Trim().Length > 0) ?
                        string.Format("<div>{0}</div>", _shipment.PackingAdditional.Trim()) : string.Empty);

                LinkButton delete = (LinkButton)e.Row.FindControl("btnDelete");

                if (delete != null)
                    delete.OnClientClick = string.Format("return confirm('Are you sure you want to delete the shipment: {0}? Note that this will cause the items in the shipment to marked as NOT shipped.')",
                        Utils.ParseHelper.ParseJsAlert(_shipment.ReferenceNumber.ToString()));

                //set visibility for returned to sender
                Button rts = (Button)e.Row.FindControl("btnReturn");

                if (rts != null)
                {
                    rts.Visible = ((!_shipment.ReturnedToSender) && (_shipment.Context == _Enums.ProductContext.ticket || _shipment.Context == _Enums.ProductContext.all) &&
                        _shipment.ShipMethod != null && _shipment.ShipMethod.Trim().Length > 0 && _shipment.ShipMethod != ShipMethod.WillCall &&
                        _shipment.DateShipped < DateTime.MaxValue);

                    if (rts.Visible)
                    {
                        rts.CommandArgument = string.Format("{0}~{1}~{2}", _rowCounter, _shipment.Id, _shipment.DateShipped);
                        rts.OnClientClick = string.Format("return confirm('Are you sure you want to return the shipment: {0}? All tickets in this shipment will be marked for will call.')",
                            Utils.ParseHelper.ParseJsAlert(_shipment.ReferenceNumber.ToString()));
                    }
                }

                _rowCounter += 1;
            }
        }
        protected void GridShipments_DataBound(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;

            //select last row
            if (grid.Rows.Count > 0 && grid.SelectedIndex == -1)
                grid.SelectedIndex = grid.Rows.Count - 1;

            FormDetails.DataBind();
        }
        protected void GridShipments_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(FormDetails.CurrentMode != FormViewMode.Edit)
                FormDetails.ChangeMode(FormViewMode.Edit);

            FormDetails.DataBind();
        }
        protected void GridShipments_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            //try to discern if shipping should be reset
            GridView grid = (GridView)sender;
            object key = grid.DataKeys[e.RowIndex].Value;
            
            if (key != null)
            {
                int idx = int.Parse(key.ToString());

                InvoiceShipment _shipment = (InvoiceShipment)Atx.CurrentInvoiceRecord.InvoiceShipmentRecords().Find((int)idx);

                if (_shipment != null)
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();

                    sb.Append("DECLARE @shipItemId int; ");

                    //update the invoiceitem that was the shipment item
                    sb.AppendFormat("SELECT @shipItemId = [TShipItemId] FROM [InvoiceShipment] WHERE [Id] = {0} ", idx);
                    sb.Append("IF @shipItemId IS NOT NULL BEGIN ");
                    sb.Append("UPDATE [InvoiceItem] SET [dtShipped] = null WHERE [Id] = @shipItemId ");
                    sb.Append("END ");

                    //update the other invoice items - match to invoiceshipment items
                    sb.Append("UPDATE [InvoiceItem] SET [dtShipped] = null WHERE [Id] IN ");
                    sb.AppendFormat("(SELECT [tInvoiceItemId] FROM [InvoiceShipmentItem] WHERE [tInvoiceShipmentId] = {0}); ", idx);

                    sb.AppendFormat("DELETE FROM [InvoiceShipment] WHERE [Id] = {0} ", _shipment.Id);

                    Utils.DataHelper.ExecuteNonQuery(sb, _Config.DSN);

                    //rebind all grids to show updated info
                    int invId = Atx.CurrentInvoiceRecord.Id;
                    Atx.SetCurrentInvoiceRecord(0);
                    Atx.SetCurrentInvoiceRecord(invId);

                    GridItems.DataBind();
                    GridShip.DataBind();
                    GridShipments.SelectedIndex = -1;
                    GridShipments.DataBind();
                }
            }
        }
        protected void GridShipments_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //try to discern if shipping should be reset
            GridView grid = (GridView)sender;
            string[] parts = e.CommandArgument.ToString().Split('~');

            if (parts.Length == 3)
            {
                int rowIdx = int.Parse(parts[0]);

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                SubSonic.QueryCommand cmd = new SubSonic.QueryCommand(string.Empty, SubSonic.DataService.Provider.Name);

                switch (e.CommandName.ToLower())
                {
                    case "rts":
                        //update the invoice item to reflect that the ticket had been shipped - but was returned to the sender - 
                        //for whatever reason
                        //update notes
                        //record the old ship date to notes as well
                        //clear the ship date
                        //change the shipmethod to will call
                        int idx = int.Parse(parts[1]);
                        string shipDate = parts[2];

                        cmd.AddParameter("@shipId", idx, DbType.Int32);
                        cmd.AddParameter("@oldShipDate", shipDate, DbType.String);
                        cmd.AddParameter("@nowDate", DateTime.Now.ToString("MM/dd/yyyy hh:mmtt"), DbType.String);
                        cmd.AddParameter("@newMethod", ShipMethod.WillCall, DbType.String);
                        cmd.AddParameter("@context", _Enums.InvoiceItemContext.ticket.ToString(), DbType.String);

                        sb.Append("UPDATE InvoiceShipment SET [Status] = 'Returned to sender: ' + @nowDate + ' ' + ISNULL([Status],''), ");
                        sb.Append("[bRTS] = 1 ");
                        sb.Append("WHERE [Id] = @shipId; ");

                        //update the shiprequest if it exists
                        sb.Append("UPDATE [InvoiceItem] SET [Notes] = 'Returned to sender: ' + @nowDate + ' ' + ISNULL([Notes],''), [bRTS] = 1 ");
                        sb.Append("FROM [InvoiceItem] ii WHERE ii.[Id] IN (SELECT [TShipItemId] FROM [InvoiceShipment] WHERE [Id] = @shipId) ");

                        //update ticket items
                        sb.Append("UPDATE InvoiceItem SET [Notes] = 'Returned to sender: ' + @nowDate + ' ' + ISNULL([Notes],''), ");
                        sb.Append("[ShippingMethod] = @newMethod, [bRTS] = 1 ");
                        sb.AppendFormat("WHERE [PurchaseAction] = '{0}' AND [Id] IN (SELECT ii.[Id] FROM [InvoiceShipmentItem] item, [InvoiceItem] ii ", 
                            _Enums.PurchaseActions.Purchased.ToString());
                        sb.Append("WHERE item.[tInvoiceShipmentId] = @shipId AND ii.[Id] = item.[tInvoiceItemId] AND ");
                        sb.Append("ii.[vcContext] = @context); ");

                        break;
                }

                if (sb.Length == 0)
                    return;

                try
                {
                    cmd.CommandSql = sb.ToString();
                    SubSonic.DataService.ExecuteScalar(cmd);

                    int idx = Atx.CurrentInvoiceRecord.Id;
                    Atx.SetCurrentInvoiceRecord(0);
                    Atx.SetCurrentInvoiceRecord(idx);

                    GridItems.DataBind();
                    GridShipments.DataBind();
                }
                catch (Exception ex)
                {
                    _Error.LogException(ex);

                    CustomValidator val = (CustomValidator)grid.Rows[rowIdx].FindControl("RowValidation");
                    if (val != null)
                    {
                        val.IsValid = false;
                        val.ErrorMessage = ex.Message;
                    }
                }
            }
        }

        #endregion

        #region Form Details

        protected void FormDetails_DataBinding(object sender, EventArgs e)
        {
            FormView form = (FormView)sender;

            InvoiceShipmentCollection coll = new InvoiceShipmentCollection();

            if (GridShipments.SelectedValue != null)
            {
                InvoiceShipment addShip = (InvoiceShipment)Atx.CurrentInvoiceRecord.InvoiceShipmentRecords().Find((int)GridShipments.SelectedValue);
                if (addShip != null)
                    coll.Add(addShip);

                form.DataSource = coll;
            }
        }
        protected void FormDetails_DataBound(object sender, EventArgs e)
        {
            FormView form = (FormView)sender;

            InvoiceShipment _shipment = (InvoiceShipment)form.DataItem;

            TextBox company = (TextBox)form.FindControl("txtCompany");
            TextBox first = (TextBox)form.FindControl("txtFirst");
            TextBox last = (TextBox)form.FindControl("txtLast");
            Literal displayEmail = (Literal)form.FindControl("litDisplayEmail");
            Literal full = (Literal)form.FindControl("litFullName");
            TextBox address1 = (TextBox)form.FindControl("txtAddress1");
            TextBox address2 = (TextBox)form.FindControl("txtAddress2");
            TextBox city = (TextBox)form.FindControl("txtCity");
            TextBox state = (TextBox)form.FindControl("txtState");
            TextBox zip = (TextBox)form.FindControl("txtZip");
            TextBox country = (TextBox)form.FindControl("txtCountry");
            TextBox phone = (TextBox)form.FindControl("txtPhone");
            TextBox message = (TextBox)form.FindControl("txtMessage");
            TextBox weightActual = (TextBox)form.FindControl("txtActualWeight");
            TextBox shipActual = (TextBox)form.FindControl("txtShip");
            TextBox txtCharged = (TextBox)form.FindControl("txtCharged");
            TextBox txtDateShipped = (TextBox)form.FindControl("txtDateShipped");
            
            if (form.CurrentMode == FormViewMode.Insert)
            {
                RadioButtonList shipContext = (RadioButtonList)form.FindControl("rdoShipContext");

                txtCharged.Text = (shipContext.SelectedValue.ToLower() == _Enums.ProductContext.merch.ToString().ToLower()) ? Atx.CurrentInvoiceRecord.MerchandiseShipping.ToString() :
                    (shipContext.SelectedValue.ToLower() == _Enums.ProductContext.ticket.ToString().ToLower()) ? Atx.CurrentInvoiceRecord.TicketShipping.ToString() : "0.0";                   
                    
                InvoiceBillShip ship = Atx.CurrentInvoiceRecord.InvoiceBillShip;

                if (company != null)
                    company.Text = ship.Company_Working;
                if (first != null)
                    first.Text = ship.FirstName_Working;
                if (last != null)
                    last.Text = ship.LastName_Working;
                if (displayEmail != null)
                    displayEmail.Text = ship.Email_Working;
                if (full != null)
                    full.Text = ship.FullName_Working;
                if (address1 != null)
                    address1.Text = ship.Address1_Working;
                if (address2 != null)
                    address2.Text = ship.Address2_Working;
                if (city != null)
                    city.Text = ship.City_Working;
                if (state != null)
                    state.Text = ship.State_Working;
                if (zip != null)
                    zip.Text = ship.Zip_Working;
                if (country != null)
                    country.Text = ship.Country_Working;
                if (phone != null)
                    phone.Text = ship.Phone_Working;
                if (message != null)
                    message.Text = ship.ShipMessage_Working;

                CheckBoxList itemlist = (CheckBoxList)form.FindControl("chkItems");
                if (itemlist != null)
                    itemlist.DataBind();
            }

            //display any batch info
            if (_shipment != null && form.CurrentMode == FormViewMode.Edit && _shipment.VcContext.ToLower() == "ticket")
            {
                Literal litBatchId = (Literal)form.FindControl("litBatchId");
                Literal litBatchName = (Literal)form.FindControl("litBatchName");

                if (_shipment.ShipmentBatchInvoiceShipmentRecords().Count > 0)
                {
                    foreach (ShipmentBatchInvoiceShipment sbis in _shipment.ShipmentBatchInvoiceShipmentRecords())
                    {
                        ShipmentBatch batch = sbis.ShipmentBatchRecord;
                        litBatchId.Text = string.Format("<div>{0}</div>", batch.BatchId);
                        litBatchName.Text = string.Format("<div>{0}</div>", batch.Name);                    
                    }
                }
            }


            //edit mode
            Literal packList = (Literal)form.FindControl("litPackList");
            if (packList != null && _shipment != null && _shipment.InvoiceShipmentItemRecords().Count > 0)
                foreach (InvoiceShipmentItem isi in _shipment.InvoiceShipmentItemRecords())
                {
                    InvoiceItem item = isi.InvoiceItemRecord;
                    packList.Text += GetItemShipText(item, true, false);
                }

            Button print = (Button)form.FindControl("btnPrint");
            if (print != null && _shipment != null)
            {
                //print.Enabled = _shipment.DateShipped < DateTime.MaxValue;
                print.OnClientClick = string.Format("javascript:doPagePopup('/Admin/PrintPackList.aspx?ship={0}','false')", _shipment.Id);
            }

            ListBox lstItems = (ListBox)form.FindControl("lstItems");
            if (lstItems != null && _shipment != null)
                lstItems.DataBind();

            if (txtDateShipped != null && _shipment != null && _shipment.DateShipped != DateTime.MaxValue)
                txtDateShipped.Text = _shipment.DateShipped.ToString("MM/dd/yyyy hh:mm tt");

            //if the shipment has been returned - don't allow editing
            if (_shipment != null && _shipment.ReturnedToSender)
            {
                Button save = (Button)form.FindControl("btnSave");
                if (save != null)
                    save.Enabled = false;
                Button cancel = (Button)form.FindControl("btnCancel");
                if (cancel != null)
                    cancel.Enabled = false;
                Button ship = (Button)form.FindControl("btnShip");
                if (ship != null)
                    ship.Enabled = false;
                Button clear = (Button)form.FindControl("btnClearDate");
                if (clear != null)
                    clear.Enabled = false;

                if (print != null)
                    print.Enabled = false;
            }

        }
        protected void FormDetails_ModeChanging(object sender, FormViewModeEventArgs e)
        {
            FormView form = (FormView)sender;

            form.ChangeMode(e.NewMode);
            if (e.CancelingEdit)
                form.DataBind();
        }
        protected void FormDetails_ModeChanged(object sender, EventArgs e)
        {
            FormView form = (FormView)sender;
        }
        protected void FormDetails_ItemCommand(object sender, FormViewCommandEventArgs e)
        {
            FormView form = (FormView)sender;
            string command = e.CommandName.ToLower();

            switch (command)
            {
                case "cleardate":
                    InvoiceShipment _shipment = InvoiceShipment.FetchByID((int)form.SelectedValue);

                    if (_shipment != null)
                    {
                        System.Text.StringBuilder str = new System.Text.StringBuilder();

                        //update the actual shipment
                        str.AppendFormat("UPDATE [InvoiceShipment] SET [dtShipped] = null WHERE [Id] = {0}; ", _shipment.Id);

                        //update the shipitem if exists
                        if (_shipment.TShipItemId.HasValue)
                            str.AppendFormat("UPDATE [InvoiceItem] SET [dtShipped] = null WHERE [Id] = {0}; ", _shipment.TShipItemId.Value);

                        str.AppendFormat("UPDATE InvoiceItem SET dtShipped = null WHERE [PurchaseAction] = '{0}' AND [Id] IN ",
                            _Enums.PurchaseActions.Purchased.ToString());
                        str.AppendFormat("(SELECT [tInvoiceItemId] FROM InvoiceShipmentItem WHERE [tInvoiceShipmentId] = {0}); ", _shipment.Id);

                        Utils.DataHelper.ExecuteNonQuery(str, _Config.DSN);

                        //rebind all grids to show updated info
                        int idmx = Atx.CurrentInvoiceRecord.Id;
                        Atx.SetCurrentInvoiceRecord(0);
                        Atx.SetCurrentInvoiceRecord(idmx);

                        GridItems.DataBind();//this will cascade
                        GridShip.DataBind();
                        GridShipments.DataBind();
                        GridShipments.SelectedIndex = GridShipments.Rows.Count - 1;
                        form.DataBind();
                    }
                    break;
                case "checkrates":
                    //get current address info
                    ((RadioButtonList)form.FindControl("rdoMethods")).DataBind();
                    //update rate drop down
                    break;
                case "cancel":
                    form.ChangeMode(FormViewMode.Edit);
                    form.DataBind();
                    break;
                case "ship"://finalizes an new generic entry
                    //mark appropriate items as shipped - record method and date
                    InvoiceShipment ship = InvoiceShipment.FetchByID((int)form.SelectedValue);

                    if (ship != null)
                    {
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        DateTime now = DateTime.Now;

                        ship.DateShipped = now;

                        sb.AppendFormat("UPDATE InvoiceBillShip SET dtShipped = '{0}' WHERE tInvoiceId = {1}; ", now.ToString("MM/dd/yyyy hh:mmtt"), _invoiceId);
                        sb.AppendFormat("UPDATE InvoiceShipment SET dtShipped = '{0}' WHERE [Id] = {1} ", now.ToString("MM/dd/yyyy hh:mmtt"), ship.Id);
                        
                        //update the shipping request item if exists
                        if (ship.TShipItemId.HasValue)
                            sb.AppendFormat("UPDATE [InvoiceItem] SET [dtShipped] = '{0}' WHERE [Id] = {1}; ", now.ToString("MM/dd/yyyy hh:mmtt"), ship.TShipItemId.Value);

                        //update tickets, merch 
                        sb.AppendFormat("UPDATE InvoiceItem SET dtShipped = '{0}', [ShippingMethod] = '{1}' WHERE [Id] IN ", 
                            now.ToString("MM/dd/yyyy hh:mmtt"), ship.ShipMethod);
                        sb.AppendFormat("(SELECT [tInvoiceItemId] FROM InvoiceShipmentItem WHERE [tInvoiceShipmentId] = {0}) ", ship.Id);
                       
                        Utils.DataHelper.ExecuteNonQuery(sb, _Config.DSN);

                        //rebind all grids to show updated info
                        int idex = Atx.CurrentInvoiceRecord.Id;
                        Atx.SetCurrentInvoiceRecord(0);
                        Atx.SetCurrentInvoiceRecord(idex);

                        GridItems.DataBind();//this will cascade
                        GridShip.DataBind();
                        GridShipments.DataBind();
                        GridShipments.SelectedIndex = GridShipments.Rows.Count - 1;
                        form.DataBind();
                    }
                    break;
            }
        }
        protected void FormDetails_ItemInserting(object sender, FormViewInsertEventArgs e)
        {
            FormView form = (FormView)sender;
            _errors.Clear();
            Invoice _invoice = Atx.CurrentInvoiceRecord;

            TextBox txtGenMethod = (TextBox)form.FindControl("txtGenericMethod");
            string method = txtGenMethod.Text.Trim();

            if (method.Length == 0 || (method.ToLower() == "willcall" || method.ToLower() == ShipMethod.WillCall.ToLower()))
                _errors.Add("You must specify a ship method. You cannot specify WillCall.");

            CheckBoxList list = (CheckBoxList)FormDetails.FindControl("chkItems");

            string first = ((TextBox)form.FindControl("txtFirst")).Text.Trim();
            string last = ((TextBox)form.FindControl("txtLast")).Text.Trim();
            Utils.Validation.ValidateRequiredField(_errors, "First Name", first);
            Utils.Validation.ValidateRequiredField(_errors, "Last Name", last);

            string address1 = ((TextBox)form.FindControl("txtAddress1")).Text.Trim();
            string address2 = ((TextBox)form.FindControl("txtAddress2")).Text.Trim();
            string city = ((TextBox)form.FindControl("txtCity")).Text.Trim();
            string state = ((TextBox)form.FindControl("txtState")).Text.Trim();
            string zip = ((TextBox)form.FindControl("txtZip")).Text.Trim();
            string country = ((TextBox)form.FindControl("txtCountry")).Text.Trim();
            string phone = ((TextBox)form.FindControl("txtPhone")).Text.Trim();
            Utils.Validation.ValidateRequiredField(_errors, "Address1", address1);
            Utils.Validation.ValidateRequiredField(_errors, "City", city);
            Utils.Validation.ValidateRequiredField(_errors, "State", state);
            Utils.Validation.ValidateRequiredField(_errors, "Postal Code / Zip", zip);
            Utils.Validation.ValidateRequiredField(_errors, "Country", country);
            Utils.Validation.ValidateRequiredField(_errors, "Phone", phone);

            string charged = ((TextBox)form.FindControl("txtCharged")).Text.Trim();
            Utils.Validation.ValidateRequiredField(_errors, "Amount Charged", charged);


            CustomValidator validation = (CustomValidator)form.FindControl("CustomValidation");

            if (Utils.Validation.IncurredErrors(_errors, validation))
            {
                e.Cancel = true;
                return;
            }


            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            SubSonic.QueryCommand cmd = new SubSonic.QueryCommand(string.Empty, SubSonic.DataService.Provider.Name);

            RadioButtonList contextList = (RadioButtonList)form.FindControl("rdoShipContext");
            string context = contextList.SelectedValue.ToLower();
            
            cmd.AddParameter("@invoiceId", _invoice.Id, DbType.Int32);
            cmd.AddParameter("@userId", _invoice.UserId.ToString(), DbType.String);
            cmd.AddParameter("@dateCreated", DateTime.Now.ToString(), DbType.String);
            cmd.AddParameter("@reference", Guid.NewGuid().ToString(), DbType.String);
            cmd.AddParameter("@context", context, DbType.String);
            cmd.AddParameter("@company", ((TextBox)form.FindControl("txtCompany")).Text.Trim(), DbType.String);
            cmd.AddParameter("@first", first, DbType.String);
            cmd.AddParameter("@last", last, DbType.String);
            cmd.AddParameter("@address1", address1, DbType.String);
            cmd.AddParameter("@address2", address2, DbType.String);
            cmd.AddParameter("@city", city, DbType.String);
            cmd.AddParameter("@state", state, DbType.String);
            cmd.AddParameter("@zip", zip, DbType.String);
            cmd.AddParameter("@country", country, DbType.String);
            cmd.AddParameter("@phone", phone, DbType.String);
            cmd.AddParameter("@message", ((TextBox)form.FindControl("txtMessage")).Text.Trim(), DbType.String);

            //TODO: enter carrier info
            
            cmd.AddParameter("@carrier", string.Empty, DbType.String);
            cmd.AddParameter("@method", method, DbType.String);
            cmd.AddParameter("@tracking", ((TextBox)form.FindControl("txtTracking")).Text.Trim(), DbType.String);
            cmd.AddParameter("@additional", ((TextBox)form.FindControl("txtAdditional")).Text.Trim(), DbType.String);
            cmd.AddParameter("@shippingCharged", decimal.Parse(charged), DbType.Decimal);

            decimal actual = 0;
            string actualInput = ((TextBox)form.FindControl("txtShip")).Text.Trim();
            if (actualInput.Length > 0 && Utils.Validation.IsDecimal(actualInput))
                actual = decimal.Parse(actualInput);

            cmd.AddParameter("@actualShipping", actual, DbType.Decimal);
            

            sb.Append("DECLARE @shipId int; ");

            sb.Append("INSERT INTO InvoiceShipment ([dtStamp],[tInvoiceId],[UserId],[dtCreated],[ReferenceNumber],[vcContext],[CompanyName],");
            sb.Append("[FirstName],[LastName],[Address1],[Address2],[City],[StateProvince],[PostalCode],[Country],[Phone],");
            sb.Append("[ShipMessage],[dtShipped],[vcCarrier],[ShipMethod],[TrackingInformation],[PackingList],[PackingAdditional],");
            sb.Append("[mWeightCalculated],[mHandlingCalculated],[mShippingCharged],[mShippingActual]) ");

            sb.Append("VALUES ((getDate()),@invoiceId,@userId,@dateCreated,@reference,@context,@company,");
            sb.Append("@first,@last,@address1,@address2,@city,@state,@zip,@country,@phone,");
            sb.Append("@message,@dateCreated,@carrier,@method,@tracking,@packing,@additional,");
            sb.Append("@weightCalc,@handlingCalc,@shippingCharged,@actualShipping) ");

            sb.Append("SET @shipId = SCOPE_IDENTITY(); ");

            System.Text.StringBuilder pack = new System.Text.StringBuilder();
            decimal weightCalc = 0;
            decimal handlingCalc = 0;
            

            int i = 0;
            foreach (ListItem li in list.Items)
            {
                if (li.Selected)
                {
                    InvoiceItem itm = (InvoiceItem)_invoice.InvoiceItemRecords().Find(int.Parse(li.Value));

                    if(itm == null)
                        itm = InvoiceItem.FetchByID(int.Parse(li.Value));

                    if(itm == null)
                        throw new Exception("Listed item cannot be found");

                    string itemId = string.Format("@itemId_{0}", i);
                    string qty = string.Format("@qty_{0}", i);
                    cmd.AddParameter(itemId, itm.Id, DbType.Int32);
                    cmd.AddParameter(qty, itm.Quantity, DbType.Int32);

                    sb.Append("INSERT INTO InvoiceShipmentItem ([dtStamp],[tInvoiceShipmentId],[tInvoiceItemId],[iQuantity]) ");
                    sb.AppendFormat("VALUES ((getDate()),@shipId,{0},{1}) ", itemId, qty);

                    sb.AppendFormat("UPDATE InvoiceItem SET [ShippingMethod] = @method, [bRTS] = 0, [dtShipped] = (getDate()) WHERE [Id] = {0}; ", itm.Id);

                    pack.AppendFormat("{0}~", GetItemShipText(itm, false, true));

                    if (itm.IsMerchandiseItem)
                    {
                        if(itm.IsPromotionItem)
                            weightCalc += itm.SalePromotionRecord.Weight;
                        else
                            weightCalc += itm.MerchRecord.Weight;

                        handlingCalc += itm.LineItemTotal;
                    }

                    i++;
                }
            }

            cmd.AddParameter("@packing", pack.ToString().TrimEnd('~'), DbType.String);
            cmd.AddParameter("@weightCalc", weightCalc, DbType.Decimal);
            cmd.AddParameter("@handlingCalc", ecommercemax_shipping.ComputeHandlingFee(method, handlingCalc), DbType.Decimal);

            cmd.CommandSql = sb.ToString();

            try
            {
                SubSonic.DataService.ExecuteScalar(cmd);

                int idx = _invoiceId;
                Atx.SetCurrentInvoiceRecord(0);
                Atx.SetCurrentInvoiceRecord(idx);

                form.ChangeMode(FormViewMode.Edit);
                GridItems.DataBind();
                GridShipments.DataBind();//this will cascade
            }
            catch (Exception ex)
            {
                CustomValidator custom = (CustomValidator)form.FindControl("CustomValidation");
                if (custom != null)
                {
                    custom.IsValid = false;
                    custom.ErrorMessage = ex.Message;
                }
            }
        }

        List<string> _errors = new List<string>();

        protected void FormDetails_ItemUpdating(object sender, FormViewUpdateEventArgs e)
        {
            FormView form = (FormView)sender;
            _errors.Clear();
            Invoice _invoice = Atx.CurrentInvoiceRecord;

            if (form.DataKey != null)
            {
                InvoiceShipment _shipment = (InvoiceShipment)_invoice.InvoiceShipmentRecords().Find((int)form.DataKey.Value);

                if (_invoice != null && _shipment != null)
                {
                    //validate inputs
                    string first = ((TextBox)form.FindControl("txtFirst")).Text.Trim();
                    string last = ((TextBox)form.FindControl("txtLast")).Text.Trim();
                    Utils.Validation.ValidateRequiredField(_errors, "First Name", first);
                    Utils.Validation.ValidateRequiredField(_errors, "Last Name", last);

                    string address1 = ((TextBox)form.FindControl("txtAddress1")).Text.Trim();
                    string address2 = ((TextBox)form.FindControl("txtAddress2")).Text.Trim();
                    string city = ((TextBox)form.FindControl("txtCity")).Text.Trim();
                    string state = ((TextBox)form.FindControl("txtState")).Text.Trim();
                    string zip = ((TextBox)form.FindControl("txtZip")).Text.Trim();
                    string country = ((TextBox)form.FindControl("txtCountry")).Text.Trim();
                    string phone = ((TextBox)form.FindControl("txtPhone")).Text.Trim();
                    Utils.Validation.ValidateRequiredField(_errors, "Address1", address1);
                    Utils.Validation.ValidateRequiredField(_errors, "City", city);
                    Utils.Validation.ValidateRequiredField(_errors, "State", state);
                    Utils.Validation.ValidateRequiredField(_errors, "Postal Code / Zip", zip);
                    Utils.Validation.ValidateRequiredField(_errors, "Country", country);
                    Utils.Validation.ValidateRequiredField(_errors, "Phone", phone);

                    string charged = ((TextBox)form.FindControl("txtCharged")).Text.Trim();
                    Utils.Validation.ValidateRequiredField(_errors, "Amount Charged", charged);

                    CustomValidator validation = (CustomValidator)form.FindControl("CustomValidation");
                    
                    if (Utils.Validation.IncurredErrors(_errors, validation))
                    {
                        e.Cancel = true;
                        return;
                    }
                    
                    
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    SubSonic.QueryCommand cmd = new SubSonic.QueryCommand(string.Empty, SubSonic.DataService.Provider.Name);

                    //todo: ability to change context and shipmethod
                    //if changing shipmethod - charge the customer - refund, etc

                    //determine if packing list matches - 
                    //diff items need to update invoiceitems

                    cmd.AddParameter("@shipId", _shipment.Id, DbType.Int32);
                    cmd.AddParameter("@printed", ((CheckBox)form.FindControl("chkPrinted")).Checked, DbType.Boolean);
                    cmd.AddParameter("@additional", ((TextBox)form.FindControl("txtAdditional")).Text.Trim(), DbType.String);
                    cmd.AddParameter("@company", ((TextBox)form.FindControl("txtCompany")).Text.Trim(), DbType.String);
                    cmd.AddParameter("@first", first, DbType.String);
                    cmd.AddParameter("@last", last, DbType.String);
                    cmd.AddParameter("@address1", address1, DbType.String);
                    cmd.AddParameter("@address2", address2, DbType.String);
                    cmd.AddParameter("@city", city, DbType.String);
                    cmd.AddParameter("@state", state, DbType.String);
                    cmd.AddParameter("@zip", zip, DbType.String);
                    cmd.AddParameter("@country", country, DbType.String);
                    cmd.AddParameter("@phone", phone, DbType.String);
                    cmd.AddParameter("@message", ((TextBox)form.FindControl("txtMessage")).Text.Trim(), DbType.String);
                    cmd.AddParameter("@tracking", ((TextBox)form.FindControl("txtTracking")).Text.Trim(), DbType.String);
                    cmd.AddParameter("@shippingCharged", decimal.Parse(charged), DbType.Decimal);
                    cmd.AddParameter("@actualWeight", Decimal.Parse(((TextBox)form.FindControl("txtActualWeight")).Text.Trim()), DbType.Decimal);
                    cmd.AddParameter("@actualShipping", Decimal.Parse(((TextBox)form.FindControl("txtShip")).Text.Trim()), DbType.Decimal);


                    WillCallWeb.Components.Util.CalendarClock clockShip =
                        (WillCallWeb.Components.Util.CalendarClock)form.FindControl("clockShip");
                    DateTime selected = clockShip.SelectedDate;
                    cmd.AddParameter("@shipDate", (selected < DateTime.MaxValue) ? selected : System.Data.SqlTypes.SqlDateTime.Null, DbType.DateTime);

                    //update items if date does not match
                    if (selected != _shipment.DateShipped)
                    {
                        //update the shipitem if exists
                        if (_shipment.TShipItemId.HasValue)
                            sb.AppendFormat("UPDATE [InvoiceItem] SET [dtShipped] = @shipDate WHERE [Id] = {0}; ", _shipment.TShipItemId.Value);

                        sb.AppendFormat("UPDATE InvoiceItem SET dtShipped = @shipDate WHERE [PurchaseAction] = '{0}' AND [Id] IN ", 
                            _Enums.PurchaseActions.Purchased.ToString());
                        sb.AppendFormat("(SELECT [tInvoiceItemId] FROM InvoiceShipmentItem WHERE [tInvoiceShipmentId] = {0}); ", _shipment.Id);
                    }

                    sb.Append("UPDATE InvoiceShipment SET [bLabelPrinted] = @printed, [PackingAdditional] = @additional, [CompanyName] = @company, ");
                    sb.Append("[FirstName] = @first, [LastName] = @last, [Address1] = @address1, [Address2] = @address2, ");
                    sb.Append("[City] = @city, [StateProvince] = @state, [PostalCode] = @zip, [Country] = @country, ");
                    sb.Append("[Phone] = @phone, [ShipMessage] = @message, [dtShipped] = @shipDate, [TrackingInformation] = @tracking, ");
                    sb.Append("[mWeightActual] = @actualWeight, [mShippingActual] = @actualShipping, [mShippingCharged] = @shippingCharged ");
                    sb.Append("WHERE [Id] = @shipId ");

                    cmd.CommandSql = sb.ToString();

                    try
                    {
                        SubSonic.DataService.ExecuteScalar(cmd);

                        int idx = _invoiceId;
                        Atx.SetCurrentInvoiceRecord(0);
                        Atx.SetCurrentInvoiceRecord(idx);

                        GridItems.DataBind();
                        GridShip.DataBind();
                        GridShipments.DataBind();
                    }
                    catch (Exception ex)
                    {
                        CustomValidator custom = (CustomValidator)form.FindControl("CustomValidation");
                        if (custom != null)
                        {
                            custom.IsValid = false;
                            custom.ErrorMessage = ex.Message;
                        }
                    }
                }
            }
        }

        #endregion

        private string GetItemShipText(InvoiceItem ii, bool includeDivsAndRaquo, bool includeEstimatedShipDate)
        {
            string txt = string.Empty;

            if (includeDivsAndRaquo)
                txt = "<div><b>&raquo;</b> ";

            if (ii.IsBundle)
                txt += string.Format("BUNDLE: {0}", ii.MainActName);
            else if (ii.IsBundleSelection)
                txt += string.Format("{0} @ BUNDLE SELECTION: {1}", ii.Quantity.ToString(), ii.MainActName);
            else if (ii.IsActivationCodeDelivery)
                txt += string.Format("{0} @ {1} - ACTTIVATION CODE: {2}", ii.Quantity.ToString(), ii.MainActName, ii.ActivationDeliveryCode);
            else
                txt += ii.LineItemDescription_CriteriaAndDescription(includeEstimatedShipDate);

            if (includeDivsAndRaquo)
                txt += "</div>";

            return txt;
        }


        #region Select ShipContext Selection
        
        protected void rdoMethods_DataBinding(object sender, EventArgs e)
        {
            if (FormDetails.CurrentMode == FormViewMode.Insert)
            {
                RadioButtonList rdoMethods = (RadioButtonList)sender;
                CheckBoxList itemlist = (CheckBoxList)FormDetails.FindControl("chkItems");
                TextBox address1 = (TextBox)FormDetails.FindControl("txtAddress1");
                TextBox address2 = (TextBox)FormDetails.FindControl("txtAddress2");
                TextBox state = (TextBox)FormDetails.FindControl("txtState");
                TextBox zip = (TextBox)FormDetails.FindControl("txtZip");
                TextBox country = (TextBox)FormDetails.FindControl("txtCountry");

                TextBox weightActual = (TextBox)FormDetails.FindControl("txtActualWeight");
                decimal shipWeight = (weightActual.Text.Trim().Length > 0 && Utils.Validation.IsDecimal(weightActual.Text)) ? decimal.Parse(weightActual.Text) : 0;

                string shipAddress = string.Format("{0} {1}", address1.Text.Trim(), address2.Text.Trim());
                string shipState = state.Text.Trim();
                string shipZip = zip.Text.Trim();
                string shipCountry = country.Text.Trim();

                if (shipAddress.Length == 0)
                    shipAddress = Atx.CurrentInvoiceRecord.WorkingShippingAddress;
                if (shipState.Length == 0)
                    shipState = Atx.CurrentInvoiceRecord.WorkingState;
                if (shipZip.Length == 0)
                    shipZip = Atx.CurrentInvoiceRecord.WorkingZip;
                if (shipCountry.Length == 0)
                    shipCountry = Atx.CurrentInvoiceRecord.WorkingCountry.Trim();

                if (shipCountry.ToLower() == "usa")
                    shipCountry = "us";

                //bool isMediaRateQualified = false;

                if (itemlist != null)
                {
                    Decimal weightToShip = shipWeight;
                    List<InvoiceItem> merchItemsForMediaCheck = new List<InvoiceItem>();

                    if (shipWeight == 0)
                    {
                        foreach (ListItem li in itemlist.Items)
                        {
                            if (li.Selected)
                            {
                                InvoiceItem itm = (InvoiceItem)Atx.CurrentInvoiceRecord.InvoiceItemRecords().Find(int.Parse(li.Value));

                                if (itm == null)
                                    itm = InvoiceItem.FetchByID(int.Parse(li.Value));

                                if (itm == null)
                                    throw new Exception("Listed item cannot be found");

                                //decide weight - include flat ships as we are just trying to show costs involved
                                if (itm.IsMerchandiseItem)
                                {
                                    weightToShip = itm.MerchRecord.Weight;                                   
                                    merchItemsForMediaCheck.Add(itm);
                                }
                                else if (itm.IsPromotionItem && itm.SalePromotionRecord.IsMerchPromotion)
                                {
                                    weightToShip = itm.SalePromotionRecord.Weight;
                                    merchItemsForMediaCheck.Add(itm);
                                }
                            }
                        }
                    }

                    if (weightToShip == 0)
                        weightToShip = 1;

                    //this is done with 0 for handling
                    List<ListItem> shipMethods = new List<ListItem>();
                    shipMethods.Add(new ListItem("No charge for this shipment","0"));
                    shipMethods.Add(new ListItem("Item is to be combined in another shipment","0"));

                    string chosenMethod = string.Empty;

                    RadioButtonList shipContext = (RadioButtonList)FormDetails.FindControl("rdoShipContext");

                    //this will enforce, in ALL context, that default is set to willcall
                    chosenMethod = string.Empty;

                    if (shipContext.SelectedValue.ToLower() == "tickets" ||
                        (shipContext.SelectedValue.ToLower() == "all" && (Atx.CurrentInvoiceRecord.HasTicketItems || Atx.CurrentInvoiceRecord.HasTicketShipmentItemsOtherThanWillCall)))
                        shipMethods.Add(new ListItem(ShipMethod.WillCall, string.Format("{0}~0", ShipMethod.WillCall)));

                    try
                    {
                        shipMethods.AddRange(ecommercemax_shipping.GetShipRates(0, shipAddress, shipCountry, shipZip, shipState, weightToShip, merchItemsForMediaCheck));

                        rdoMethods.Enabled = true;
                    }
                    catch (Exception ex)
                    {
                        shipMethods.Add(new ListItem(ex.Message));
                        rdoMethods.Enabled = false;
                    }

                    rdoMethods.DataSource = shipMethods;
                    rdoMethods.DataTextField = "Text";
                    rdoMethods.DataValueField = "Value";
                }
            }
        }

        protected void rdoMethods_DataBound(object sender, EventArgs e)
        {
            RadioButtonList rdoMethods = (RadioButtonList)sender;
            RadioButtonList shipContext = (RadioButtonList)FormDetails.FindControl("rdoShipContext");

            if (shipContext != null)
            {
                //this will enforce, in ALL context, that default is set to willcall
                string chosenMethod = string.Empty;// (shipContext.SelectedValue.ToLower() == "merch") ? Atx.CurrentInvoiceRecord.MerchandiseShipmentItems[0].MainActName : Atx.CurrentInvoiceRecord.TicketShipItem.MainActName;

                if (rdoMethods.SelectedIndex == -1)
                {
                    foreach (ListItem li in rdoMethods.Items)
                    {
                        if (li.Value.ToLower().IndexOf(chosenMethod.ToLower()) != -1)
                        {
                            li.Selected = true;
                            break;
                        }
                    }

                    rdoMethods.SelectedIndex = 0;
                }
            }
        }
        
        protected void rdoShipContext_DataBinding(object sender, EventArgs e)
        {
            RadioButtonList list = (RadioButtonList)sender;

            list.Items.Clear();

            if (Atx.CurrentInvoiceRecord != null && Atx.CurrentInvoiceRecord.ShippableMerchItems_PostSale_AllowActivationCodeDelivery.Count > 0)
                list.Items.Add(new ListItem(_Enums.ProductContext.merch.ToString(), _Enums.ProductContext.merch.ToString().ToLower()));

            if (Atx.CurrentInvoiceRecord != null && Atx.CurrentInvoiceRecord.HasTicketItems_NotYetShippedPostSale)
                list.Items.Add(new ListItem(_Enums.ProductContext.ticket.ToString(), _Enums.ProductContext.ticket.ToString().ToLower()));

            list.Items.Add(new ListItem(_Enums.ProductContext.all.ToString()));
        }
        protected void rdoShipContext_DataBound(object sender, EventArgs e)
        {
            RadioButtonList list = (RadioButtonList)sender;

            if (list.Items.Count == 0)
                list.Items.Add(new ListItem("Sorry, there are no items available for shipment", ""));

            if (list.SelectedIndex == -1)
                list.SelectedIndex = 0;

            CheckBoxList items = (CheckBoxList)FormDetails.FindControl("chkItems");
            if (items != null)
                items.DataBind();
        }
        protected void rdoShipContext_SelectedIndexChanged(object sender, EventArgs e)
        {
            RadioButtonList shipContext = (RadioButtonList)sender;

            CheckBoxList items = (CheckBoxList)FormDetails.FindControl("chkItems");
            if (items != null)
                items.DataBind();

            RadioButtonList list = (RadioButtonList)FormDetails.FindControl("rdoMethods");
            if (list != null)
            {
                list.SelectedIndex = -1;
                list.DataBind();
            }

            TextBox txtCharged = (TextBox)FormDetails.FindControl("txtCharged");

            if (txtCharged != null && FormDetails.CurrentMode == FormViewMode.Insert)
            {
                txtCharged.Text = (shipContext.SelectedValue.ToLower() == _Enums.ProductContext.merch.ToString().ToLower()) ? Atx.CurrentInvoiceRecord.MerchandiseShipping.ToString() :
                    (shipContext.SelectedValue.ToLower() == _Enums.ProductContext.ticket.ToString().ToLower()) ? Atx.CurrentInvoiceRecord.TicketShipping.ToString() : "0.0";
            }
            
        }
        protected void chkItems_DataBinding(object sender, EventArgs e)
        {
            CheckBoxList list = (CheckBoxList)sender;
            list.Items.Clear();

            string context = null;
            //get the value of the context
            RadioButtonList contextControl = (RadioButtonList)FormDetails.FindControl("rdoShipContext");
            InvoiceShipment _shipment = (InvoiceShipment)FormDetails.DataItem;

            if (contextControl != null)
                context = contextControl.SelectedValue.ToLower();
            else if (_shipment != null)
                context = _shipment.Context.ToString().ToLower();

            InvoiceItemCollection coll = new InvoiceItemCollection();
            if (context.ToLower() == _Enums.ProductContext.merch.ToString().ToLower())
                if (Atx.CurrentInvoiceRecord.MerchandiseShipmentItems != null)
                    coll.AddRange(Atx.CurrentInvoiceRecord.MerchandiseShipmentItems);
            else if(Atx.CurrentInvoiceRecord.TicketShipmentItems != null)
                coll.AddRange(Atx.CurrentInvoiceRecord.TicketShipmentItems);

            string shipMethod = (coll.Count > 0) ? coll[0].MainActName : string.Empty;


            TextBox genMethod = (TextBox)FormDetails.FindControl("txtGenericMethod");
            if (genMethod != null)
                genMethod.Text = shipMethod;

            if (context != null && context.ToLower() == _Enums.ProductContext.merch.ToString().ToLower())
                foreach (InvoiceItem ii in Atx.CurrentInvoiceRecord.ShippableMerchItems_PostSale_AllowActivationCodeDelivery)
                {
                    if (ii.PurchaseAction == _Enums.PurchaseActions.Purchased.ToString())
                    {
                        string descr = GetItemShipText(ii, false, false);

                        ListItem li = new ListItem(descr, ii.Id.ToString());

                        if (FormDetails.CurrentMode != FormViewMode.Insert && ii.DateShipped != DateTime.MaxValue)
                            li.Selected = true;
                        else if (FormDetails.CurrentMode == FormViewMode.Insert && ii.DateShipped == DateTime.MaxValue)
                            li.Selected = true;

                        if (ii.DateShipped != DateTime.MaxValue)
                            li.Attributes.Add("style", "color: red;");

                        list.Items.Add(li);
                    }
                }
            else if (context == _Enums.ProductContext.ticket.ToString().ToLower())
                foreach (InvoiceItem ii in Atx.CurrentInvoiceRecord.TicketItems_NotYetShippedPostSale)
                {
                    if (ii.PurchaseAction == _Enums.PurchaseActions.Purchased.ToString())
                    {
                        string desc = GetItemShipText(ii, false, true);
                        ListItem li = new ListItem(desc, ii.Id.ToString());

                        if (FormDetails.CurrentMode != FormViewMode.Insert && ii.DateShipped != DateTime.MaxValue)
                            li.Selected = true;
                        else if (FormDetails.CurrentMode == FormViewMode.Insert && (ii.DateShipped == DateTime.MaxValue || (ii.ShippingMethod != null && ii.ShippingMethod.ToLower() != ShipMethod.WillCall.ToLower())))
                            li.Selected = true;

                        if (ii.DateShipped != DateTime.MaxValue)
                            li.Attributes.Add("style", "color: red;");
                        list.Items.Add(li);
                    }
                }
            else if(context == _Enums.ProductContext.all.ToString().ToLower())
                foreach (InvoiceItem ii in Atx.CurrentInvoiceRecord.InvoiceItemRecords())
                {
                    if (ii.PurchaseAction == _Enums.PurchaseActions.Purchased.ToString() && (ii.IsMerchandiseItem || ii.IsTicketItem || ii.IsPromotionItem))
                    {
                        ListItem li = new ListItem(GetItemShipText(ii, false, true), ii.Id.ToString());

                        if (ii.DateShipped != DateTime.MaxValue || (ii.ShippingMethod != null && ii.ShippingMethod.ToLower() != ShipMethod.WillCall.ToLower()))
                            li.Attributes.Add("style", "color: red;");

                        list.Items.Add(li);
                    }
                }
        }
        protected void lstItems_DataBinding(object sender, EventArgs e)
        {
            ListBox list = (ListBox)sender;
            list.Items.Clear();

            InvoiceShipment _shipment = (InvoiceShipment)FormDetails.DataItem;

            if (_shipment != null)
            {
                foreach (InvoiceShipmentItem isi in _shipment.InvoiceShipmentItemRecords())
                {
                    string def = string.Format("{0} @ {1} ", isi.Quantity, GetItemShipText(isi.InvoiceItemRecord, false, false));
                    list.Items.Add(new ListItem(def, isi.Id.ToString()));
                }
            }
        }

        #endregion

        #region Misc

        protected void CustomPacking_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (IsPostBack)
            {
                CustomValidator custom = (CustomValidator)source;
                CheckBoxList list = (CheckBoxList)FormDetails.FindControl("chkItems");

                args.IsValid = (list != null && list.SelectedIndex != -1);
            }
        }

        #endregion
        
}
}

    
