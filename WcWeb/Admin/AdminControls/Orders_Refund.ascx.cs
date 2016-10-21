using System;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using Wcss;

//OnClientClick="return confirm('Are you sure you want to refund this invoice\r\n--and--\r\nthat have checked the correct items and/or processes to refund?')" />

//show date of invoice
//show expiry of card
//show customer address

//allow authnet to deny based on 120 days
//allow authnet to deny based on card expiry

//show auth net return vals properly

namespace WillCallWeb.Admin.AdminControls
{
    /// <summary>
    /// Changing row order in the items grid could have serious consequences as to 
    /// </summary>
    public partial class Orders_Refund : BaseControl
    {
        protected void rdoProcessor_DataBinding(object sender, EventArgs e)
        {
            RadioButtonList radio = (RadioButtonList)sender;

            if (radio.Items.Count == 0)
            {
                List<ListItem> items = new List<ListItem>();

                if (Atx.CurrentInvoiceRecord.StoreCreditTotal > 0 && Atx.CurrentInvoiceRecord.CreditCardPaymentsTotal > 0)
                    items.Add(new ListItem("Store Credit Then Auth&nbsp;&nbsp;", "CreditAndAuthNet"));
                if (Atx.CurrentInvoiceRecord.CreditCardPaymentsTotal > 0)
                    items.Add(new ListItem("AuthNet Only&nbsp;&nbsp;","AuthNet"));

                //always allow store credit and company check
                items.Add(new ListItem("Store Credit Only&nbsp;&nbsp;", "StoreCredit"));

                items.Add(new ListItem("Company Check", "Check"));

                radio.DataSource = items;
                radio.DataTextField = "Text";
                radio.DataValueField = "Value";
            }
        }
        protected void rdoProcessor_DataBound(object sender, EventArgs e)
        {
            RadioButtonList radio = (RadioButtonList)sender;
            if (radio.Items.Count > 0 && radio.SelectedIndex == -1)
                radio.SelectedIndex = 0;
        }

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
                Atx.SetCurrentInvoiceRecord(0);//reset existing
                Atx.SetCurrentInvoiceRecord(_invoiceId);
            }
        }
        protected override void OnLoad(EventArgs e)
        {
            rdoProcessor.DataBind();

            if (!IsPostBack)
            {   
                GridInvoice.DataBind();
                GridView1.DataBind();
                GridRefunds.DataBind();
            }

            litUserEditor.Text = (Atx.CurrentInvoiceRecord != null) ? string.Format("<a href=\"/Admin/EditUser.aspx?username={0}\" >{0}</a>",
                Atx.CurrentInvoiceRecord.AspnetUserRecord.UserName) : "&nbsp;";
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
                    base.Redirect(string.Format("/Admin/CustomerEditor.aspx?p=sales&UserName={0}", Atx.CurrentInvoiceRecord.PurchaseEmail));
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

        #endregion

        #region Grid Invoice

        protected void GridInvoice_DataBinding(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;

            InvoiceCollection coll = new InvoiceCollection();
            coll.Add(Atx.CurrentInvoiceRecord);
            grid.DataSource = coll;
        }
        protected void GridInvoice_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Invoice i = (Invoice)e.Row.DataItem;
            Literal ships = (Literal)e.Row.FindControl("litShipments");
            if (ships != null && i != null)
            {
                foreach (InvoiceShipment s in i.InvoiceShipmentRecords())
                {
                    ships.Text += s.Addressee;
                }
            }

            Literal exp = (Literal)e.Row.FindControl("litExpiry");
            if (exp != null && i != null)
            {
                Cashew cash = i.CashewRecord;

                exp.Text = " - ";

                if (cash == null)
                    exp.Text += "<span style=\"color: Red; font-weight: bold;\">Invalid/outdated credit card.</span>";
                else
                {
                    if (cash.ExpiryDate_Effective < DateTime.Now)
                        exp.Text += "<span style=\"color: Red; font-weight: bold;\">";

                    exp.Text += string.Format("{0}", cash.ExpiryDate_Effective.ToString("MM/dd/yyyy"));

                    if (cash.ExpiryDate_Effective < DateTime.Now)
                        exp.Text += "</span>";
                }
            }
        }
        protected void GridInvoice_DataBound(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;
            
            //we wont ever have more than one row
            if (grid.Rows.Count > 0)
                grid.SelectedIndex = 0;

            GridItems.DataBind();
        }

        #endregion

        #region Items

        protected void GridItems_DataBinding(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;

            grid.DataSource = Atx.CurrentInvoiceRecord.InvoiceItemRecords();
        }
        protected void GridItems_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridView grid = (GridView)sender;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                InvoiceItem ii = (InvoiceItem)e.Row.DataItem;
                if (ii.PurchaseAction != _Enums.PurchaseActions.Purchased.ToString())
                    e.Row.Attributes.Add("style", "background-color: #e1e1e1;");

                Literal description = (Literal)e.Row.FindControl("litDescription");
                if (description != null)
                {
                    description.Text = ii.LineItemDescription_CriteriaAndDescription(true);

                    if (ii.Notes != null && ii.Notes.Trim().Length > 0)
                        description.Text += string.Format("<div>{0}</div>", System.Web.HttpUtility.HtmlEncode(ii.Notes));
                }

                //check box only necessary for shipments and items
                if (ii.Context != _Enums.InvoiceItemContext.merch && ii.Context != _Enums.InvoiceItemContext.ticket &&
                    ii.Context != _Enums.InvoiceItemContext.shippingmerch && ii.Context != _Enums.InvoiceItemContext.shippingticket)
                    e.Row.Cells[7].Controls.Clear();
            }
        }

        #endregion

        #region GridView - Listing of Items for refunding

        protected void btnSelect_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow gvr in GridView1.Rows)
            {
                CheckBox select = (CheckBox)gvr.FindControl("chkSelect");
                select.Checked = true;
                GridInvoice.DataBind();
                GridRefunds.DataBind();
            }
        }
        protected void btnDeselect_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow gvr in GridView1.Rows)
            {
                CheckBox select = (CheckBox)gvr.FindControl("chkSelect");
                select.Checked = false;
                GridInvoice.DataBind();
                GridRefunds.DataBind();
            }
        }
        protected void GridView1_DataBinding(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;

            Invoice inv = Atx.CurrentInvoiceRecord;

            List<RefundListItem> items = new List<RefundListItem>();

            //go thru merch and ticket items - ensure we dont have previously refunded items
            InvoiceItemCollection refundable = new InvoiceItemCollection();
            //service fees
            //tickets and merch
            //and promotions
            refundable.AddRange(inv.InvoiceItemRecords().GetList().FindAll(delegate(InvoiceItem match)
            {
                return ((

                    (   match.Context == _Enums.InvoiceItemContext.processing || 
                        match.Context == _Enums.InvoiceItemContext.bundle || 
                        match.Context == _Enums.InvoiceItemContext.charity || 
                        match.Context == _Enums.InvoiceItemContext.servicecharge || 
                        match.Context == _Enums.InvoiceItemContext.shippingmerch || 
                        match.Context == _Enums.InvoiceItemContext.shippingticket) || 
                    (   match.Context == _Enums.InvoiceItemContext.merch || 
                        (match.Context == _Enums.InvoiceItemContext.ticket && 
                        ((!match.ShowTicketRecord.IsPackage) || (match.ShowTicketRecord.IsPackage && match.ShowTicketRecord.IsBaseOfPackage))))) &&
                        (match.PurchaseAction == _Enums.PurchaseActions.Purchased.ToString()));
            }));

            //if we have any gift certificates - we need to ensure they have not been redeemed
            InvoiceItemCollection GcsRedeemed = new InvoiceItemCollection();
            GcsRedeemed.AddRange(refundable.GetList()
                .FindAll(delegate(InvoiceItem match) { return (match.IsGiftCertificateDelivery && match.DateShipped < DateTime.Now); }));

            if (GcsRedeemed.Count > 0)
            {
                foreach (InvoiceItem ii in GcsRedeemed)
                {
                    int idx = refundable.GetList().FindIndex(delegate(InvoiceItem match) { return (match.Guid == ii.Guid); } );
                    if(idx != -1 && idx < refundable.Count)
                        refundable.RemoveAt(idx);
                }
            }

            //convert items to exchangeable
            foreach (InvoiceItem ii in refundable)
                items.Add(new RefundListItem(ii));

            grid.DataSource = items;
        }
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridView grid = (GridView)sender;

            RefundListItem rli = (RefundListItem)e.Row.DataItem;

            if (rli != null)
            {
                DropDownList qty = (DropDownList)e.Row.FindControl("ddlQty");
                int max = rli.Quantity;
                for (int i = 1; i <= max; i++)
                    qty.Items.Add(new ListItem(i.ToString()));
                qty.SelectedIndex = qty.Items.Count - 1;
                qty.Enabled = (qty.Items.Count > 1);

                //tickets have service charges
                CheckBox chkService = (CheckBox)e.Row.FindControl("chkService");
                Literal litService = (Literal)e.Row.FindControl("litService");
                if (rli.Context == _Enums.InvoiceItemContext.ticket)
                {
                    chkService.Visible = true;
                    chkService.Checked = true;
                    litService.Text = string.Format("{0}", rli.Service.ToString("n"));
                }
                else
                {
                    chkService.Visible = false;
                    litService.Text = string.Empty;
                }

                Literal description = (Literal)e.Row.FindControl("litDescription");
                if (description != null)
                {
                    InvoiceItem itm = InvoiceItem.FetchByID(rli.ItemId);

                    string itemListing = itm.LineItemDescription_CriteriaAndDescription(true);

                    //if an item has already gone thru its process or poses a risk if it were to be refunded - highlight in red
                    //ex: items that have shipped
                    if (itm != null && itm.DateShipped < DateTime.Now)
                    {
                        description.Text = string.Format("<div style=\"color: Red;\">{0}</div>", itemListing);
                        if (itm.IsGiftCertificateDelivery)
                        {
                            CheckBox chkSelect = (CheckBox)e.Row.FindControl("chkSelect");
                            if(chkSelect != null)
                                chkSelect.Enabled = false;
                        }
                    }
                    else
                        description.Text = itemListing;
                }
            }
        }
        protected void GridView1_DataBound(object sender, EventArgs e) { }

        #endregion
        
        #region GridRefunds

        protected void GridRefunds_DataBinding(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;

            InvoiceItemCollection coll = new InvoiceItemCollection();
            coll.AddRange(Atx.CurrentInvoiceRecord.InvoiceItemRecords().GetList().FindAll(
                delegate(InvoiceItem match)
                {
                    return (match.PurchaseAction.ToLower() == _Enums.PurchaseActions.PurchasedThenRemoved.ToString().ToLower());
                }));

            if(coll.Count > 1)
                coll.Sort("Id", false);

            grid.DataSource = coll;
        }
        protected void GridRefunds_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridView grid = (GridView)sender;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal description = (Literal)e.Row.FindControl("litDescription");
                InvoiceItem ii = (InvoiceItem)e.Row.DataItem;

                if (description != null && ii != null)
                {
                    description.Text = ii.LineItemDescription_CriteriaAndDescription(true);
                }
            }
        }

        #endregion

        #region Refund Processes

        protected void btnDoRefund_Click(object sender, EventArgs e)
        {    
            string proc = rdoProcessor.SelectedValue;
            string checkNumber = txtCheckNum.Text.Trim();

            try
            {
                if (Atx.TransactionProcessingVariables != null)
                    throw new Exception("Processing Variables are not null. You may be duplicating a transaction. Please contact the administrator. To continue, you may have to logout and log back in.");

                decimal discount = (txtDiscount.Text.Trim().Length > 0 && Utils.Validation.IsDecimal(txtDiscount.Text.Trim())) ? 
                    decimal.Parse(txtDiscount.Text.Trim()) : 0;

                //TODO: change to discounter - add a role
                if(discount > 0 && (!this.Page.User.IsInRole("Administrator")))
                    throw new Exception("You must have administrative priveleges to discount an invoice.");

                string creatorName = this.Profile.UserName;

                string result = OrderRefund.DoRefund(proc, Atx.CurrentInvoiceRecord, GridView1, checkNumber,
                    discount, txtDescription.Text.Trim(), creatorName, this.Request.UserHostAddress, string.Empty);

                if (result.IndexOf("SUCCESS") != -1)
                {   
                    int idx = Atx.CurrentInvoiceRecord.Id;

                    Atx.SetTransactionVariables(idx, 0, DateTime.Now);

                    //if we have issued any store credit....
                    //update user credit
                    if (result.IndexOf("SYNCUSER") != -1)
                        WillCallWeb.StoreObjects.SaleItem_StoreCredit.Profile_StoreCredit_Sync(Atx.CurrentInvoiceRecord.AspnetUserRecord.UserName, creatorName, true);

                    base.Redirect("/Admin/ProcessingTransaction.aspx?redir=refund");
                }
            }
            catch (System.Threading.ThreadAbortException) { }
            catch (Exception ex)
            {
                _Error.LogException(ex);

                CustomValidation.IsValid = false;
                CustomValidation.ErrorMessage = ex.Message;
            }
        }

        #endregion

        #region Sql Selecting
        protected void Sql_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters["@appId"].Value = _Config.APPLICATION_ID;
        }
        #endregion
    }
}
