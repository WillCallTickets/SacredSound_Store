using System;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using Wcss;

//OnClientClick="return confirm('Are you sure you want to refund this invoice\r\n--and--\r\nthat have checked the correct items and/or processes to refund?')" />

namespace WillCallWeb.Admin.AdminControls
{
    /// <summary>
    /// Changing row order in the items grid could have serious consequences as to 
    /// </summary>
    public partial class Orders_Exchange : BaseControl
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
                Atx.SetCurrentInvoiceRecord(0);//reset existing
                Atx.SetCurrentInvoiceRecord(_invoiceId);
            }
        }
        protected override void OnLoad(EventArgs e)
        {
            GridInvoice.DataBind();
            GridExchanges.DataBind();

            if (!IsPostBack)
            {   
                GridView1.DataBind();
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

        #region Exchange Processes

        protected void btnDoExchange_Click(object sender, EventArgs e)
        {
            try
            {
                //START HERE!!!!!
                //Go thru inventory and fix misnamed items - eg. stickers

                //ensure we have items selected for exchange
                int exchangeCount = 0;
                int selectionCount = 0;

                foreach (GridViewRow gvr in GridView1.Rows)
                {
                    if (gvr.RowType == DataControlRowType.DataRow)
                    {
                        //if the item is selected for exchange - increment count and verify an exchange selection
                        CheckBox chkSelect = (CheckBox)gvr.FindControl("chkSelect");
                        if (chkSelect != null)
                        {
                            if (chkSelect.Checked)
                            {
                                exchangeCount++;
                                DropDownList ddlExchange = (DropDownList)gvr.FindControl("ddlExchange");
                                if (ddlExchange != null)
                                {
                                    if (ddlExchange.SelectedIndex > 0)
                                        selectionCount++;
                                }

                                //verify  other text if other is chosen
                                RadioButtonList rdoReason = (RadioButtonList)gvr.FindControl("rdoReason");
                                TextBox txtOther = (TextBox)gvr.FindControl("txtOther");
                                if(rdoReason != null && txtOther != null && 
                                    rdoReason.SelectedValue.ToLower() == "other" && txtOther.Text.Trim().Length == 0)
                                    throw new Exception("Please provide a reason for the exchange (you must specify when choosing other).");
                            }
                        }
                    }
                }

                if (exchangeCount == 0)
                    throw new Exception("You have no items marked for exchange.");
                else if(exchangeCount != selectionCount)
                    throw new Exception("You are missing some items to exchange in your selections.");

                // Exchange
                string creatorName = this.Profile.UserName;
                string result = OrderExchange.DoExchange(Profile.GetProfile(Atx.CurrentInvoiceRecord.AspnetUserRecord.UserName), Atx.CurrentInvoiceRecord, GridView1, 
                    creatorName, this.Request.UserHostAddress, chkIssueCredit.Checked);


                if (result.IndexOf("SUCCESS") != -1)
                {
                    if (result.IndexOf("SYNCUSER") != -1)
                        WillCallWeb.StoreObjects.SaleItem_StoreCredit.Profile_StoreCredit_Sync(Atx.CurrentInvoiceRecord.AspnetUserRecord.UserName, creatorName, true);

                    int idx = Atx.CurrentInvoiceRecord.Id;

                    Atx.Clear_CurrentMerchListing();

                    Atx.SetTransactionVariables(idx, 0, DateTime.Now);
                    base.Redirect("/Admin/ProcessingTransaction.aspx?redir=exch");
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

        #region GridView

        protected void GridView1_DataBinding(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;

            Invoice inv = Atx.CurrentInvoiceRecord;

            InvoiceItemCollection exchangeable = new InvoiceItemCollection();
            List<ExchangeListItem> items = new List<ExchangeListItem>();

            //TODO: shipping methods
            
            //tickets and merch - tickets where it is not a package or is the base of the package
            exchangeable.AddRange(inv.InvoiceItemRecords().GetList().FindAll(delegate(InvoiceItem match)
            {
                return (
                    (
                    //match.Context == _Enums.InvoiceItemContext.bundle || 
                    (match.Context == _Enums.InvoiceItemContext.merch && match.MerchRecord != null && 
                        match.MerchRecord.DeliveryType != _Enums.DeliveryType.giftcertificate)
                    ||
                    (match.Context == _Enums.InvoiceItemContext.ticket && 
                        ((!match.ShowTicketRecord.IsPackage) || (match.ShowTicketRecord.IsPackage && match.ShowTicketRecord.IsBaseOfPackage)))
                    ) 
                    && (match.PurchaseAction == _Enums.PurchaseActions.Purchased.ToString())
                    ); }));

            //convert items to exchangeable
            foreach (InvoiceItem ii in exchangeable)
                items.Add(new ExchangeListItem(ii));

            grid.DataSource = items;
        }
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridView grid = (GridView)sender;

            ExchangeListItem rli = (ExchangeListItem)e.Row.DataItem;

            if (rli != null)
            {
                DropDownList qty = (DropDownList)e.Row.FindControl("ddlQty");

                int max = rli.Quantity;

                for (int i = 1; i <= max; i++)
                    qty.Items.Add(new ListItem(i.ToString()));

                qty.SelectedIndex = qty.Items.Count - 1;

                DropDownList exchange = (DropDownList)e.Row.FindControl("ddlExchange");
                RadioButtonList reason = (RadioButtonList)e.Row.FindControl("rdoReason");

                List<ListItem> items = new List<ListItem>();
                items.Add(new ListItem("-- Select an item for exchange --", "0"));

                InvoiceItem ii = (InvoiceItem)Atx.CurrentInvoiceRecord.InvoiceItemRecords().Find(rli.ItemId);
                if (ii == null)
                    throw new Exception("Invoice item could not be found");

                if (rli.Context == _Enums.InvoiceItemContext.ticket)
                {
                    //if it is a package - make it so we can't select
                    CheckBox select = (CheckBox)e.Row.FindControl("chkSelect");
                    if (select != null)
                        select.Enabled = (!rli.IsPackageTicket);

                    if (reason != null && reason.Items.Count == 0)
                    {
                        reason.Items.Add(new ListItem("Order Error"));
                        reason.Items.Add(new ListItem("Wrong Item"));
                        reason.Items.Add(new ListItem("Other"));

                        reason.SelectedIndex = 0;
                    }

                    if (exchange != null)
                    {
                        foreach (ShowTicket st in Atx.SaleTickets)
                        {
                            if (st.Id != ii.TShowTicketId)
                            {
                                string desc = string.Format("({5}) {0} {1} {2} {3} - {4} ", st.PerItemPrice.ToString("c"),
                                    st.DateOfShow.ToString("MM/dd/yyyy hh:mmtt"), st.AgeDescription, st.ShowDateRecord.ShowRecord.ShowNamePart,
                                    st.CriteriaText, st.Available);

                                ListItem li = new ListItem(desc, st.Id.ToString());
                                if (st.Available <= 0)
                                {
                                    //li.Attributes.CssStyle.Add("color", "Red");
                                    //li.Attributes.Add("cssclass", "noinventory");
                                    li.Text = string.Format("***NO INVENTORY*** {0}", desc);
                                }

                                items.Add(li);
                            }
                        }
                    }
                }
                else if (rli.Context == _Enums.InvoiceItemContext.bundle)
                {
                }
                else
                {
                    if (exchange != null)
                    {

                        if (reason != null && reason.Items.Count == 0)
                        {
                            reason.Items.Add(new ListItem("Size"));
                            reason.Items.Add(new ListItem("Color"));
                            reason.Items.Add(new ListItem("Style"));
                            reason.Items.Add(new ListItem("Damaged"));
                            reason.Items.Add(new ListItem("Order Error"));
                            reason.Items.Add(new ListItem("Wrong Item"));
                            reason.Items.Add(new ListItem("Other"));

                            reason.SelectedIndex = 0;
                        }

                        Merch child = (Merch)Atx.SaleMerch.Find(ii.TMerchId);
                        if (child == null)
                            child = Merch.FetchByID(ii.TMerchId);
                        if (child == null)
                            throw new Exception("Merchandise item could not be found");
                        Merch parent = child.ParentMerchRecord;

                        MerchCollection coll = new MerchCollection();
                        List<Merch> lst = Atx.CurrentMerchListing.GetList().FindAll(delegate(Merch match) { return (match.IsChild); });
                        if (lst.Count > 0)
                        {
                            coll.AddRange(lst);
                            if (coll.Count > 1)
                                coll.Sort("TParentListing", true);
                        }

                        //TODO: limit possible items to exchange with?
                        foreach (Merch m in coll)
                        {
                            string desc = string.Format("({0}) {1} {2}", m.Available,
                                m.Price_Effective.ToString("c"), m.DisplayNameWithAttribs);

                            ListItem li = new ListItem(desc, m.Id.ToString());
                            if (m.Available <= 0)
                                li.Text = string.Format("*** {0}", desc);

                            items.Add(li);
                        }
                    }
                }

                if (exchange != null)
                {
                    exchange.DataSource = items;
                    exchange.DataTextField = "Text";
                    exchange.DataValueField = "Value";
                    exchange.DataBind();
                }

                Literal litDescription = (Literal)e.Row.FindControl("litDescription");
                if (litDescription != null)
                    litDescription.Text = rli.Description;
            }
        }
        protected void ddlExchange_DataBound(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            if (ddl.Items.Count > 0 && ddl.SelectedIndex == -1)
                ddl.SelectedIndex = 0;
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
        protected void GridInvoice_DataBound(object sender, EventArgs e)
        {
            GridItems.DataBind();
        }

        #endregion

        protected void SqlInvoiceEvents_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters["@appId"].Value = _Config.APPLICATION_ID;
            e.Command.Parameters["@InvoiceId"].Value = Atx.CurrentInvoiceRecord.Id;
        }
}
}
