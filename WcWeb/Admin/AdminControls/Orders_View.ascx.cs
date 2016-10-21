using System;
using System.Data;
using System.Web.Security;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;

using Wcss;

//dbo.fn_InterleaveStrings(cast(ii.guid as varchar(50)), cast(ii.id as varchar(50)), 16) as 'DeliveryCode',

namespace WillCallWeb.Admin.AdminControls
{
    public partial class Orders_View : BaseControl, IPostBackEventHandler
    {
        void IPostBackEventHandler.RaisePostBackEvent(string eventArgument)
        {
            string[] args = eventArgument.Split('~');
            string command = args[0];
            int idx = (args.Length > 1 && Utils.Validation.IsInteger((string)args[1])) ? int.Parse(args[1]) : 0;
            string result = string.Empty;

            switch (command.ToLower())
            {
                case "reissuecode":
                    string code = Inventory.ReissueItemCode(idx);

                    if (code != null)
                    {
                        InvoiceItem item = InvoiceItem.FetchByID(idx);

                        string oldValue = item.DeliveryCode;

                        //mail the code to the customer
                        AspnetUser user = item.InvoiceRecord.AspnetUserRecord;
                        ProfileCommon p = new ProfileCommon();

                        MailQueue.SendReissuedCode(user.UserName, _Config._CustomerService_Email, user.UserName, _Config._CustomerService_FromName, code, item);

                        //record the event for the invoice
                        Invoice invoice = item.InvoiceRecord;
                        string creatorName = this.Profile.UserName;

                        InvoiceEvent.NewInvoiceEvent(invoice.Id, DateTime.Now, DateTime.Now, _Enums.EventQStatus.Success,
                           creatorName, invoice.UserId, invoice.AspnetUserRecord.UserName,
                           _Enums.EventQContext.Invoice, _Enums.EventQVerb.DeliveryCodeReissued,
                           oldValue, code, string.Format("Code reissued for {0}.", item.MainActName), true);

                        //update inventory
                        //treat as damage
                        Inventory.AdjustInventoryHistory(
                            item.MerchRecord,
                            item.MerchRecord.Allotment,
                            1,
                            creatorName,
                            _Enums.HistoryInventoryContext.Damage);

                        item.MerchRecord.Damaged += 1;

                        string sql = "UPDATE [Merch] SET [iDamaged] = [iDamaged] + 1 WHERE [Id] = @productId; SELECT 0";
                        _DatabaseCommandHelper cmd = new _DatabaseCommandHelper(sql);
                        cmd.AddCmdParameter("productId", item.TMerchId, DbType.Int32);

                        cmd.PerformQuery("GetNumberOfUsedCodes");

                    }
                    else
                    {
                        //a null indicates no more codes
                        this.CustomValidation.IsValid = false;
                        this.CustomValidation.ErrorMessage = "No codes are available. Add more codes to the item inventory";
                    }

                    BindGridsAndForms();

                    break;
            }
        }

        protected void btnPrintTickets_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
        }

        #region PageOverhead

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            //set opacity for nav events
            if (this.HasControls() && this.UpdatePanel1.Visible)
                Atx.RegisterJQueryScript_BlockUI_AjaxMethod(this.UpdatePanel1, "#orderview", true);
        }

        protected int _invoiceId = 0;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            string req = Request.QueryString["Inv"];
            if (req != null && Utils.Validation.IsInteger(req))
                _invoiceId = int.Parse(req);

        }
        protected override void OnLoad(EventArgs e)
        {   
            if (!IsPostBack)
            {
                BindGridsAndForms();
            }
        }
        protected void BindGridsAndForms()
        {
            FormInvoice.DataBind();
            GridShipments.DataBind();
            lstProduct.DataBind();
        }
        protected void btnLink_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            //ensure we have the latest (refreshed) data
            Ctx.SessionInvoice = null;
            Ctx.InvoiceId = _invoiceId;

            switch (btn.CommandName.ToLower())
            {
                case "custsales":
                    base.Redirect(string.Format("/Admin/CustomerEditor.aspx?p=sales&UserName={0}", Ctx.SessionInvoice.PurchaseEmail));
                    break;
                case "shipping":
                    base.Redirect(string.Format("/Admin/Orders.aspx?p=shipping&Inv={0}", Ctx.SessionInvoice.Id.ToString()));
                    break;
                case "confirmation":
                    //emit javascript to popup
                    string script = string.Empty;
                    script = string.Format("doPageTab('/Store/Confirmation.aspx?inv={0}');", Ctx.SessionInvoice.Id.ToString());
                    System.Web.UI.ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.UpdatePanel1.GetType(),
                        Guid.NewGuid().ToString(), " ;" + script, true);
                    //base.Redirect(string.Format("/Store/Confirmation.aspx?inv={0}", Ctx.SessionInvoice.Id.ToString()));
                    BindGridsAndForms();
                    break;
                case "refund":
                    base.Redirect(string.Format("/Admin/Orders.aspx?p=refund&Inv={0}", Ctx.SessionInvoice.Id.ToString()));
                    break;
                case "exchange":
                    base.Redirect(string.Format("/Admin/Orders.aspx?p=exch&Inv={0}", Ctx.SessionInvoice.Id.ToString()));
                    break;
            }            
        }

        #endregion  

        #region Invoice Form

        protected bool refundable_shippable = true;

        protected void FormInvoice_DataBound(object sender, EventArgs e)
        {
            FormView form = (FormView)sender;
            DataRowView drv = (DataRowView)form.DataItem;

            if (drv != null)
            {
                DataRow row = drv.Row;

                Literal invStatus = (Literal)form.FindControl("litStatus");
                string status = Utils.DataHelper.GetColumnValue(row, "InvoiceStatus", DbType.String).ToString();

                if(invStatus != null)
                    invStatus.Text = (status.ToLower() == _Enums.InvoiceStatii.Paid.ToString().ToLower()) ? status :
                        string.Format("<span style=\"color: red; font-weight: bold;\">{0}</span>", status);
                
                Literal litLastFour = (Literal)form.FindControl("litLastFour");
                if (litLastFour != null)
                    litLastFour.Text = row.ItemArray.GetValue(row.Table.Columns.IndexOf("LastFour")).ToString();

                string email = row.ItemArray.GetValue(row.Table.Columns.IndexOf("Email")).ToString();

                litUserEditor.Text = (email != null) ? string.Format("<a href=\"/Admin/EditUser.aspx?username={0}\" >{0}</a>", 
                    email) : "&nbsp;";
            }
        }

        #endregion

        #region Shipping Form

        protected void FormInvoiceBillShip_ItemUpdating(object sender, FormViewUpdateEventArgs e)
        {
            if (Page.IsValid)
            {
                FormView form = (FormView)sender;

                string userName = (string)e.Keys["UserName"];
                
                bool oldSame = bool.Parse(e.OldValues["bSameAsBilling"].ToString());
                string oldCompany = (string)e.OldValues["CompanyName"];
                string oldFirst = (string)e.OldValues["FirstName"];
                string oldLast = (string)e.OldValues["LastName"];
                string oldAddress1 = (string)e.OldValues["Address1"];
                string oldAddress2 = (string)e.OldValues["Address2"];
                string oldCity = (string)e.OldValues["City"];
                string oldState = (string)e.OldValues["StateProvince"];
                string oldZip = (string)e.OldValues["PostalCode"];
                string oldCountry = (string)e.OldValues["Country"];
                string oldPhone = (string)e.OldValues["Phone"];

                bool newSame = bool.Parse(e.NewValues["bSameAsBilling"].ToString());
                string newCompany = (string)e.NewValues["CompanyName"];
                string newFirst = (string)e.NewValues["FirstName"];
                string newLast = (string)e.NewValues["LastName"];
                string newAddress1 = (string)e.NewValues["Address1"];
                string newAddress2 = (string)e.NewValues["Address2"];
                string newCity = (string)e.NewValues["City"];
                string newState = (string)e.NewValues["StateProvince"];
                string newZip = (string)e.NewValues["PostalCode"];
                string newCountry = (string)e.NewValues["Country"];
                string newPhone = (string)e.NewValues["Phone"];

                #region Validate required input

                bool inputError = false;

                //validate required input
                if (!newSame)
                {
                    if (newFirst.Trim().Length == 0)
                    {
                        ((CustomValidator)form.FindControl("CustomShipFirst")).IsValid = false;
                        inputError = true;
                    }
                    if (newLast.Trim().Length == 0)
                    {
                        ((CustomValidator)form.FindControl("CustomShipLast")).IsValid = false;
                        inputError = true;
                    }
                    if (newAddress1.Trim().Length == 0)
                    {
                        ((CustomValidator)form.FindControl("CustomShipAddress")).IsValid = false;
                        inputError = true;
                    }
                    if (newCity.Trim().Length == 0)
                    {
                        ((CustomValidator)form.FindControl("CustomShipCity")).IsValid = false;
                        inputError = true;
                    }
                    if (newState.Trim().Length == 0)
                    {
                        ((CustomValidator)form.FindControl("CustomShipState")).IsValid = false;
                        inputError = true;
                    }
                    if (newZip.Trim().Length == 0)
                    {
                        ((CustomValidator)form.FindControl("CustomShipZip")).IsValid = false;
                        inputError = true;
                    }
                    if (newCountry.Trim().Length == 0)
                    {
                        ((CustomValidator)form.FindControl("CustomShipCountry")).IsValid = false;
                        inputError = true;
                    }
                    if (newPhone.Trim().Length == 0)
                    {
                        ((CustomValidator)form.FindControl("CustomShipPhone")).IsValid = false;
                        inputError = true;
                    }
                }

                if (inputError)
                {
                    e.Cancel = true;
                    return;
                }

                bool trackingChanged = false;
                if (e.OldValues["TrackingInformation"] != e.NewValues["TrackingInformation"])
                    trackingChanged = true;

                #endregion

                string oldAddress = string.Format("{0}:{1}:{2}: {3}:{4}: {5}:{6}: {7}:{8}:{9}", oldCompany ?? string.Empty,
                    oldFirst ?? string.Empty, oldLast ?? string.Empty, oldAddress1 ?? string.Empty, oldAddress2 ?? string.Empty,
                    oldCity ?? string.Empty, oldState ?? string.Empty, oldZip ?? string.Empty, oldCountry ?? string.Empty, oldPhone ?? string.Empty);

                string newAddress = string.Format("{0}:{1}:{2}: {3}:{4}: {5}:{6}: {7}:{8}:{9}", newCompany ?? string.Empty,
                    newFirst ?? string.Empty, newLast ?? string.Empty, newAddress1 ?? string.Empty, newAddress2 ?? string.Empty,
                    newCity ?? string.Empty, newState ?? string.Empty, newZip ?? string.Empty, newCountry ?? string.Empty, newPhone ?? string.Empty);

                AspnetUser usr = AspnetUser.GetUserByUserName(userName);
                
                if (usr == null)
                    throw new Exception("{0} does not exist in the application.");

                Guid userId = usr.UserId;

                if (oldAddress != newAddress || trackingChanged || newSame != oldSame)
                {
                    DateTime now = DateTime.Now;

                    if (trackingChanged)
                        InvoiceEvent.NewInvoiceEvent(_invoiceId, now, now, _Enums.EventQStatus.Success, Profile.UserName, userId, userName,
                            _Enums.EventQContext.Invoice, _Enums.EventQVerb.TrackingChange, e.OldValues["TrackingInformation"].ToString(),
                            e.NewValues["TrackingInformation"].ToString(), null, true);

                    if (newSame != oldSame)
                    {
                        e.NewValues["bSameAsBilling"] = newSame;

                        if(newSame == true)
                            InvoiceEvent.NewInvoiceEvent(_invoiceId, now, now, _Enums.EventQStatus.Success, Profile.UserName, userId, userName,
                                _Enums.EventQContext.Invoice, _Enums.EventQVerb.ChangeShipAddress, oldAddress, "Use billing address", null, true);
                        else
                            InvoiceEvent.NewInvoiceEvent(_invoiceId, now, now, _Enums.EventQStatus.Success, Profile.UserName, userId, userName,
                                _Enums.EventQContext.Invoice, _Enums.EventQVerb.ChangeShipAddress, "billing address was used", newAddress, null, true);
                    }
                    else if (oldAddress != newAddress)
                        InvoiceEvent.NewInvoiceEvent(_invoiceId, now, now, _Enums.EventQStatus.Success, Profile.UserName, userId, userName,
                            _Enums.EventQContext.Invoice, _Enums.EventQVerb.ChangeShipAddress, oldAddress, newAddress, null, true);

                    e.Cancel = false;
                }
                else
                {
                    e.Cancel = true;
                    form.ChangeMode(FormViewMode.ReadOnly);
                }
            }
        }
        protected void FormInvoiceBillShip_ItemUpdated(object sender, FormViewUpdatedEventArgs e)
        {
            if (e.Exception != null)
            {
                FormView form = (FormView)sender;
                CustomValidator val = (CustomValidator)form.FindControl("CustomValidation");

                val.IsValid = false;
                val.ErrorMessage = e.Exception.Message;
                e.ExceptionHandled = true;
            }
            else
            {
                GridEvents.DataBind();
            }
        }
        protected void FormInvoiceBillShip_ItemCommand(object sender, FormViewCommandEventArgs e)
        {
            
        }
        protected void FormInvoiceBillShip_DataBound(object sender, EventArgs e)
        {
            FormView form = (FormView)sender;

            //add a text changed JS event to uncheck the same as billing when the first name is changed
            //ensure this only happens once in js
            TextBox txtFirst = (TextBox)form.FindControl("FirstNameTextBox");
            if (txtFirst != null)
                txtFirst.Attributes.Add("onChange", "EnsureShipCheck(this, 'bSameAsBillingCheckBox');");
        }

        #endregion

        #region Item Listing

        StringBuilder sb = new StringBuilder();
        private string BuildProductName(InvoiceItem ii)
        {
            string productName = string.Empty;

            if (ii.Context == _Enums.InvoiceItemContext.charity)
                productName = string.Format("Donation to {0}", ii.MainActName);
            else
            {
                if (ii.Context == _Enums.InvoiceItemContext.shippingmerch || ii.Context == _Enums.InvoiceItemContext.shippingticket)
                    productName = string.Format("{0} ", ii.Context);

                string desc = string.Format("{0} {1}", ii.Description ?? string.Empty, ii.Criteria ?? string.Empty).Trim();

                productName += System.Text.RegularExpressions.Regex
                    .Replace(string.Format("{0} @ {1} {2} {3}",
                        ii.Quantity.ToString(),
                        ii.AgeDescription ?? string.Empty,
                        ShowTicket.IsCampingPass(desc) ? string.Empty : 
                            (ii.DateOfShow != DateTime.MaxValue) ? ii.DateOfShow.ToString("MM/dd/yyyy hh:mmtt") : string.Empty,
                        ii.MainActName), @"\s+", " ").Trim();
            }

            if ((!ii.IsBundle) && (!ii.IsBundleSelection))
            {
                if (ii.Description != null && ii.Description.Trim().Length > 0)
                    productName += string.Format(" - {0}", ii.Description.Trim());

                if (ii.Criteria != null && ii.Criteria.Trim().Length > 0 && (!ii.IsActivationCodeDelivery))
                    productName += string.Format(" - {0}", ii.Criteria.Trim());
            }

            return productName;
        }
        private void DrawProductTable(ListViewItem viewItem, Literal lit, InvoiceItem ii)
        {
            sb.Length = 0;

            sb.AppendLine("<tr>");

            string indentClass = string.Empty;
            if (ii.IsBundleSelection)
                indentClass = "indent2";
            else if (ii.IsBundle)
                indentClass = "indent1";

            sb.AppendFormat("<th class=\"contxt{0}\">{1}{2}</th>", 
                (indentClass.Trim().Length > 0) ? string.Format(" {0}", indentClass) : string.Empty,
                (indentClass.Trim().Length > 0) ? "<span class=\"child-of\">&nbsp;</span>" : string.Empty,
                ii.VcContext);
            

            //hyper link to product
            string productHref = string.Empty;
            string dateHref = string.Empty;
            
            if (ii.IsTicketItem)
            {
                if(this.Page.User.IsInRole("Manifester"))
                {
                    productHref = string.Format("/Admin/Listings.aspx?p=tickets&tixid={0}", ii.TShowTicketId.ToString());
                    dateHref = string.Format("/Admin/Listings.aspx?p=tickets&shodateid={0}&tixid=0", ii.ShowTicketRecord.TShowDateId.ToString());
                }
            }
            else if (ii.IsMerchandiseItem)
                productHref = string.Format("/Admin/MerchEditor.aspx?p=ItemEdit&merchitem={0}", ii.TMerchId.ToString());


            sb.Append("<td colspan=\"3\" style=\"\">");

            if (productHref.Trim().Length > 0)
                sb.AppendFormat("<a href=\"{0}\" >{1}</a>", productHref, BuildProductName(ii));
            else
                sb.AppendFormat("{0}", BuildProductName(ii));

            if (dateHref.Trim().Length > 0)
                sb.AppendFormat("<div><a href=\"{0}\" >&nbsp;* See all tickets for this date</a></div>", dateHref);

            sb.Append("</td>");

            sb.AppendLine("</tr>");
            sb.AppendLine();


            //line 2 - pricing
            sb.AppendLine("<tr>");
            sb.AppendFormat("<th>{0}</th>",
                (ii.PurchaseAction.ToLower() != _Enums.PurchaseActions.Purchased.ToString().ToLower()) ?
                    string.Format("<span style=\"color:red;\">{0}</span>", ii.PurchaseAction) :
                    ii.PurchaseAction);
            sb.AppendFormat("<td style=\"white-space:nowrap;\">{0} @ ({1}{2}{3}) = {4}</td>", 
                ii.Quantity.ToString(), 
                ii.Price.ToString("n2"),
                (ii.ServiceCharge > 0) ? string.Format(" + {0}svc", ii.ServiceCharge.ToString("n2")) : string.Empty,
                (ii.Adjustment > 0) ? string.Format(" + {0}adj", ii.Adjustment.ToString("n2")) : string.Empty,
                ii.LineItemTotal.ToString("c"));
            sb.AppendLine();
            sb.AppendLine("<th style=\"white-space:nowrap;\">ID / Guid</th>");
            sb.AppendFormat("<td style=\"white-space:nowrap;\">{0} / {1}</td>", ii.Id.ToString(), ii.Guid.ToString()); 
            sb.AppendLine();
            sb.AppendLine("</tr>");

            //line 3 - naming
            if (ii.Context == _Enums.InvoiceItemContext.ticket)
            {
                sb.AppendLine("<tr>");
                sb.AppendLine("<th>Purchase Name</th>");
                sb.AppendFormat("<td>{0}&nbsp;</td>", ii.PurchaseName);
                sb.AppendLine();
                sb.AppendLine("<th>Pickup Name</th>");
                sb.AppendFormat("<td>{0}&nbsp;</td>", ii.PickupName);
                sb.AppendLine();
                sb.AppendLine("</tr>");
            }

            //line 4 - shipping
            if ((ii.Context == _Enums.InvoiceItemContext.merch && (!ii.IsDownloadDelivery) && (!ii.IsGiftCertificateDelivery) && (!ii.IsActivationCodeDelivery)) ||
                ii.Context == _Enums.InvoiceItemContext.ticket ||
                ii.Context == _Enums.InvoiceItemContext.damaged ||
                ii.Context == _Enums.InvoiceItemContext.shippingmerch ||
                ii.Context == _Enums.InvoiceItemContext.shippingticket)
            {
                sb.AppendLine("<tr>");
                sb.AppendLine("<th>Ship Method</th>");
                sb.AppendFormat("<td>{0}&nbsp;</td>", ii.ShippingMethod ?? string.Empty);
                sb.AppendLine();
                sb.AppendLine("<th>Ship Date</th>");
                sb.AppendFormat("<td>{0}&nbsp;</td>", (ii.DateShipped != DateTime.MaxValue) ? 
                    ii.DateShipped.ToString("MM/dd/yyyy hh:mmtt") : string.Empty);
                sb.AppendLine();
                sb.AppendLine("</tr>");
            }

            //line 5 - delivery code
            if (ii.Context == _Enums.InvoiceItemContext.merch && ((ii.IsDownloadDelivery) || (ii.IsGiftCertificateDelivery) || (ii.IsActivationCodeDelivery)))
            {
                sb.AppendLine("<tr>");
                sb.AppendFormat("<th>{0}</th>", ii.GetDeliveryCodeLabel());
                sb.AppendLine();
                sb.AppendFormat("<td colspan=\"2\">{0}&nbsp;</td>", ii.DeliveryCode ?? string.Empty);
                sb.AppendLine();

                if (!ii.IsGiftCertificateDelivery)
                {
                    //get a postback value for this command
                    string cmdLink = Page.ClientScript.GetPostBackEventReference(this,
                            string.Format("reissuecode~{0}", ii.Id.ToString())).Replace("'", "&#39;");

                    sb.AppendLine();
                    sb.AppendFormat("<td><input type=\"button\" id=\"reissue{0}\" onclick=\"{1}\" value=\"Reissue Code\" class=\"btntny\" /></td>", ii.Id.ToString(), cmdLink);
                    sb.AppendLine();
                }
                else
                    sb.AppendLine("<td>&nbsp;</td>");


                sb.AppendLine("</tr>");

                
            }

            //line 6 - notes
            if (ii.Notes != null && ii.Notes.Trim().Length > 0)
            {
                sb.AppendLine("<tr>");
                sb.AppendLine("<th>Notes</th>");
                sb.AppendFormat("<td colspan=\"3\">{0}&nbsp;</td>", ii.Notes);
                sb.AppendLine();
                sb.AppendLine("</tr>");
            }

            //line 7 - btnedit and link ship
            Button btnEdit = (Button)viewItem.FindControl("btnEdit");
            if (btnEdit != null)
                btnEdit.Visible = (ii.Context == _Enums.InvoiceItemContext.ticket);

            //only links to shipment items
            Button linkShip = (Button)viewItem.FindControl("btnLinkShip");
            if (linkShip != null)
            {
                if (ii.ChildInvoiceItemRecords().Count > 0)
                {
                    linkShip.Visible = true;
                    linkShip.CommandArgument = ii.ChildInvoiceItemRecords()[0].TInvoiceId.ToString();
                }
                else
                    linkShip.Visible = false;
            }

            HtmlTableRow command = (HtmlTableRow)viewItem.FindControl("trCommand");
            if(command != null)
                command.Visible = ((btnEdit != null && btnEdit.Visible) || (linkShip != null && linkShip.Visible));

            
            lit.Text = sb.ToString();
        }
        protected void lstProduct_DataBinding(object sender, EventArgs e)
        {
            ListView listview = (ListView)sender;

            /*
             * we really dont want to miss anything here - so keep it simple
             * get all items - but dont include bundles or bundle selections
             * then go thru that list and add in bundles and selections
             */

            SubSonic.QueryCommand cmd = new SubSonic.QueryCommand(
                string.Format("SELECT ii.* FROM [InvoiceItem] ii WHERE ii.[TInvoiceId] = {0}; ", _invoiceId.ToString()), 
                SubSonic.DataService.Provider.Name);

            InvoiceItemCollection original = new InvoiceItemCollection();
            original.LoadAndCloseReader(SubSonic.DataService.GetReader(cmd));

            List<InvoiceItem> mainItems = new List<InvoiceItem>();
            mainItems.AddRange(original.GetList().FindAll(delegate(InvoiceItem match) { return ((!match.IsBundle) && (!match.IsBundleSelection)); }));

            if(mainItems.Count > 1)
                mainItems.Sort(delegate(InvoiceItem x, InvoiceItem y) { return (x.Id.CompareTo(y.Id)); });

            //now add in a any bundles
            List<InvoiceItem> bindList = new List<InvoiceItem>();

            if (original.Count != mainItems.Count)
            {
                foreach (InvoiceItem ii in mainItems)
                {
                    bindList.Add(ii);

                    //only merch and ticket have potential bundles
                    if (ii.IsTicketItem || ii.IsMerchandiseItem)
                    {
                        List<InvoiceItem> bundles = new List<InvoiceItem>();
                        bundles.AddRange(original.GetList().FindAll(delegate(InvoiceItem match)
                        {
                            return match.IsBundle && match.TParentInvoiceItemId == ii.Id;
                        }));

                        foreach (InvoiceItem bundle in bundles)
                        {
                            bindList.Add(bundle);

                            bindList.AddRange(original.GetList().FindAll(delegate(InvoiceItem match)
                            {
                                return match.IsBundleSelection && match.TParentInvoiceItemId == bundle.Id;
                            }));
                        }
                    }   
                }
            }
            else
                bindList.AddRange(mainItems);


            listview.DataKeyNames = new string[] { "Id" };
            listview.DataSource = bindList;

        }
        protected void lstProduct_DataBound(object sender, EventArgs e)
        {
            ListView list = (ListView)sender;

            if (list.SelectedIndex == -1 && list.Items.Count > 0)
                list.SelectedIndex = 0;
        }
        protected void lstProduct_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            ListView list = (ListView)sender;
            ListViewDataItem viewItem = (ListViewDataItem)e.Item;
            ListViewItemType type = viewItem.ItemType;

            if (type == ListViewItemType.DataItem)
            {
                InvoiceItem ii = (InvoiceItem)viewItem.DataItem;
                Literal litTable = (Literal)viewItem.FindControl("litTable");

                if (litTable != null)
                    DrawProductTable(viewItem, litTable, ii);

                Literal litProductName = (Literal)viewItem.FindControl("litProductName");
                if (litProductName != null)
                    litProductName.Text = BuildProductName(ii);
            }
        }
        protected void lstProduct_ItemEditing(object sender, ListViewEditEventArgs e)
        {
            ListView listView = (ListView)sender;
            listView.EditIndex = e.NewEditIndex;
            listView.DataBind();
        }
        protected void lstProduct_Updating(object sender, ListViewUpdateEventArgs e)
        {
            //DO NOT UPDATE AS A PACKAGE - DO EACH SEPARATELY!!
            if (Page.IsValid)
            {
                Invoice i = new Invoice(_invoiceId);
                if (i != null)
                {
                    ListView listView = (ListView)sender;
                    InvoiceItem ii = (InvoiceItem)i.InvoiceItemRecords().Find(listView.DataKeys[e.ItemIndex].Value);

                    if (ii != null)
                    {
                        ListViewDataItem viewItem = listView.Items[e.ItemIndex];
                        string userName = i.AspnetUserRecord.UserName;
                        AspnetUser usr = AspnetUser.GetUserByUserName(userName);
                        Guid userId = (usr != null) ? usr.UserId : Guid.Empty;

                        DateTime now = DateTime.Now;
                        bool isChanged = false;                        

                        //determine what is being changed and log it
                        string oldNotes = ii.Notes ?? string.Empty;
                        TextBox txtNotes = (TextBox)viewItem.FindControl("txtNotes");
                        string newNotes = (txtNotes != null) ? (txtNotes.Text.Trim().Length > 0) ? txtNotes.Text.Trim() : string.Empty : null;

                        if (oldNotes != newNotes)
                        {
                            ii.Notes = newNotes;

                            InvoiceEvent.NewInvoiceEvent(_invoiceId, now, now, _Enums.EventQStatus.Success, Profile.UserName, userId,
                                userName, _Enums.EventQContext.Invoice, _Enums.EventQVerb.ChangePickupName, oldNotes, newNotes, null, true);

                            isChanged = true;
                        }

                        string oldPickup = ii.PickupName;
                        TextBox txtLast = (TextBox)listView.Items[e.ItemIndex].FindControl("txtPickupLast");
                        TextBox txtFirst = (TextBox)listView.Items[e.ItemIndex].FindControl("txtPickupFirst");

                        if (txtLast != null && txtFirst != null)
                        {
                            //dont allow commas in the entry
                            string last = txtLast.Text.Replace(',', '_').Trim().ToUpper();
                            string first = txtFirst.Text.Replace(',', '_').Trim().ToUpper();

                            string newPickup = string.Format("{0}{1}{2}", last, (last.Length > 0) ? ", " : string.Empty, first);//comma adds a space

                            if (oldPickup == null)
                                oldPickup = string.Empty;

                            if (oldPickup != newPickup)
                            {
                                ii.PickupName = newPickup;                                

                                InvoiceEvent.NewInvoiceEvent(_invoiceId, now, now, _Enums.EventQStatus.Success, Profile.UserName, userId,
                                    userName, _Enums.EventQContext.Invoice, _Enums.EventQVerb.ChangePickupName, oldPickup, newPickup, null, true);

                                isChanged = true;
                            }
                        }


                        if (isChanged)
                        {
                            ii.Save();
                            GridEvents.DataBind();                            
                        }

                        listView.EditIndex = -1;
                        lstProduct.DataBind();
                    }
                }
            }
        }
        protected void lstProduct_ItemCanceling(object sender, ListViewCancelEventArgs e)
        {
            ListView list = (ListView)sender;
            list.EditIndex = -1;
            list.DataBind();
        }
        protected void lstProduct_Command(object sender, ListViewCommandEventArgs e)
        {
            ListView list = (ListView)sender;

            switch (e.CommandName.ToLower())
            {
                case "linkship":
                    base.Redirect(string.Format("/Admin/Orders.aspx?p=shipping&Inv={0}", e.CommandArgument.ToString()));
                    break;
                case "willcall":
                    //make sure we have a first and last name for pickup
                    int iiIdx = int.Parse(e.CommandArgument.ToString());
                    
                    try
                    {
                        Invoice _inv = new Invoice(_invoiceId);
                        InvoiceItem _item = (InvoiceItem)_inv.InvoiceItemRecords().Find(iiIdx);
                        string oldMethod = (_item != null && _item.ShippingMethod != null && _item.ShippingMethod.Trim().Length > 0) ? _item.ShippingMethod.Trim() : string.Empty;

                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        sb.Append("UPDATE [InvoiceItem] SET [ShippingMethod] = @method, [dtShipped] = null FROM [InvoiceItem] ii WHERE ii.[Id] = @id ");
                        SubSonic.QueryCommand cmd = new SubSonic.QueryCommand(sb.ToString(), SubSonic.DataService.Provider.Name);
                        cmd.Parameters.Add("@method", ShipMethod.WillCall);

                        cmd.Parameters.Add("@id", iiIdx, DbType.Int32);

                        SubSonic.DataService.ExecuteQuery(cmd);

                        //add an event to record change
                        InvoiceEvent.NewInvoiceEvent(_inv.Id, DateTime.Now, DateTime.Now, _Enums.EventQStatus.Success, this.Page.User.Identity.Name,
                            _inv.UserId, _inv.AspnetUserRecord.UserName, _Enums.EventQContext.Invoice,
                            _Enums.EventQVerb.ChangeShipMethod, oldMethod, ShipMethod.WillCall, null, true);

                        lstProduct.DataBind();
                        list.EditIndex = -1;
                        list.DataBind();
                    }
                    catch (Exception ex)
                    {
                        ListViewDataItem viewItem = (ListViewDataItem)e.Item;
                        CustomValidator custom = (CustomValidator)list.Items[viewItem.DataItemIndex].FindControl("customWillCall");

                        if (custom != null)
                        {
                            custom.IsValid = false;
                            custom.ErrorMessage = ex.Message;
                        }
                    }


                    break;
            }
        }
        
        #endregion

        #region Grid Shipments

        protected void GridShipments_DataBinding(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;

            Invoice _invoice = Invoice.FetchByID(this._invoiceId);
            if (_invoice != null && _invoice.ApplicationId != _Config.APPLICATION_ID)
                _invoice = null;

            InvoiceShipmentCollection coll = _invoice.InvoiceShipmentRecords();
            if (coll.Count > 1)
                coll.Sort("Id", true);

            grid.DataSource = coll;

            //_rowCounter = 0;
        }
        protected void GridShipments_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridView grid = (GridView)sender;
            InvoiceShipment _shipment = (InvoiceShipment)e.Row.DataItem;

            if (_shipment != null)
            {
                Literal ship = (Literal)e.Row.FindControl("litShipped");

                if (ship != null)
                    ship.Text = string.Format("{0}{1}{2}",
                        (_shipment.Status != null && _shipment.Status.Trim().Length > 0) ? string.Format("<div>{0}</div>", _shipment.Status) : string.Empty,
                        (_shipment.DateShipped < DateTime.MaxValue) ?
                        string.Format("{0}<br/>", _shipment.DateShipped.ToString("MM/dd/yyyy hh:mmtt")) : string.Empty,
                        _shipment.ReferenceNumber.ToString());

                Literal address = (Literal)e.Row.FindControl("litAddress");

                if (address != null)
                    address.Text = string.Format("{0}<br/>{1} {2}<br/>{3} {4} {5} {6} {7} {8}<br/>{9}",
                        _shipment.InvoiceRecord.PurchaseEmail,
                        _shipment.FirstName, _shipment.LastName,
                        _shipment.Address1, _shipment.Address2, _shipment.City, _shipment.StateProvince, _shipment.PostalCode,
                        _shipment.Country, _shipment.Phone);

                Literal pack = (Literal)e.Row.FindControl("litPacking");

                if (pack != null)
                    pack.Text = string.Format("<div>{0}</div>{1}", _shipment.PackingList.Replace("~", "</div><div>"),
                        (_shipment.PackingAdditional != null && _shipment.PackingAdditional.Trim().Length > 0) ?
                        string.Format("<div>{0}</div>", _shipment.PackingAdditional.Trim()) : string.Empty);
            }
        }
        protected void GridShipments_DataBound(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;

            if (grid.Rows.Count > 0 && grid.SelectedIndex == -1)
                grid.SelectedIndex = grid.Rows.Count - 1;

            //FormDetails.DataBind();
        }
        #endregion

        #region Auths

        protected void GridAuths_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Button resend = (Button)e.Row.FindControl("btnResend");
                if (resend != null)
                    resend.CommandArgument = e.Row.RowIndex.ToString();
            }
        }
        protected void GridAuths_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridView grid = (GridView)sender;
            string cmd = e.CommandName.ToLower();

            switch (cmd)
            {
                case "resend":
                    int rowIdx = int.Parse(e.CommandArgument.ToString());
                    int idx = (int)grid.DataKeys[rowIdx]["Id"];
                    AuthorizeNet auth = AuthorizeNet.FetchByID(idx);

                    if (auth != null && auth.ApplicationId == _Config.APPLICATION_ID)
                    {
                        TextBox txtAddress = (TextBox)grid.Rows[rowIdx].FindControl("txtResendToAddress");

                        string toAddress = txtAddress.Text.Trim();

                        if (!Utils.Validation.IsValidEmail(toAddress))
                        {
                            CustomValidation.IsValid = false;
                            CustomValidation.ErrorMessage = "The email to send to is invalid.";
                            return;
                        }

                        auth.SendConfirmationEmail(string.Format("**sent from admin: {0}**", DateTime.Now.ToString("MM/dd/yyyy hh:mmtt")), toAddress, true);

                        DateTime now = DateTime.Now;
                        InvoiceEvent.NewInvoiceEvent(_invoiceId, now, now, _Enums.EventQStatus.Success, Profile.UserName, auth.UserId,
                            auth.Email, _Enums.EventQContext.Invoice, _Enums.EventQVerb.ResendConfirmationEmail, "AuthId", auth.Id.ToString(), 
                            string.Format("sent to: {0}", toAddress), true);

                        GridShipments.DataBind();
                        lstProduct.DataBind();
                        GridEvents.DataBind();
                    }

                    break;
            }
        }
        protected void GridAuths_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                GridView grid = (GridView)sender;
                DataRowView view = (DataRowView)e.Row.DataItem;

                Button resend = (Button)e.Row.FindControl("btnResend");
                string type = view.Row.ItemArray.GetValue(view.Row.Table.Columns.IndexOf("TransactionType")).ToString();

                if (resend != null)
                    resend.Visible = (type.ToLower() == "auth_capture");

                int idx = int.Parse(view.Row.ItemArray.GetValue(view.Row.Table.Columns.IndexOf("Id")).ToString());
                AuthorizeNet auth = AuthorizeNet.FetchByID(idx);

                TextBox txtAddress = (TextBox)e.Row.FindControl("txtResendToAddress");

                if (auth != null && txtAddress != null)
                {
                    txtAddress.Visible = (type.ToLower() == "auth_capture");
                    txtAddress.Text = auth.AspnetUserRecord.UserName;
                }

                string method = view.Row.ItemArray.GetValue(view.Row.Table.Columns.IndexOf("Method")).ToString();
                if (method != null && method.ToLower() == "storecredit")
                {
                    Literal lit = (Literal)e.Row.FindControl("litCreditTitle");
                    if (lit != null)
                        lit.Visible = true;
                }
            }
        }
        #endregion

        //no methods for event grid

        protected void Querystring_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters["@appId"].Value = _Config.APPLICATION_ID;

            if(e.Command.Parameters.Contains("@willcall"))
                e.Command.Parameters["@willcall"].Value = ShipMethod.WillCall;
        }


        
}
}
