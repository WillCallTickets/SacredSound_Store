using System;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using Wcss;
using WillCallWeb.StoreObjects;

namespace WillCallWeb.Controls
{
    public partial class OrderFinalize : WillCallWeb.BaseControl
    {
        //respond to cart changes
        private void EventHandler_CartChanged(object sender, EventArgs e)
        {
            if (Ctx.Cart.IsOverMaxTransactionAllowed)
            {
                Ctx.CurrentCartException = Ctx.Cart.FormatMaxTransactionError();

                Response.Redirect("/Store/Cart_Edit.aspx");
            }

            BindCheckout();
        }

        protected bool _displayShippingSection 
        { 
            get
            {
                //if the cart has item
                if (Ctx.Cart.HasTicketItems_CurrentlyShippable || (Ctx.Cart.HasMerchandiseItems_Shippable && _Config._Shipping_Merch_Active))
                    return true;

                return false;
            } 
        }

        private bool _sameAsBilling = true;
        private string _company = string.Empty;
        private string _firstName = string.Empty;
        private string _lastName = string.Empty;
        private string _address1 = string.Empty;
        private string _address2 = string.Empty;
        private string _city = string.Empty;
        private string _state = string.Empty;
        private string _zip = string.Empty;
        private string _country = string.Empty;
        private string _phone = string.Empty;

        public void ValidateTerms(object source, ServerValidateEventArgs e)
        {
            e.IsValid = (CheckTerms.Checked);
        }

        public void ValidateCaptcha(object source, ServerValidateEventArgs e)
        {
            if (Page.IsValid)
            {
                e.IsValid = TextCaptcha.Text.ToLower() == Ctx.CurrentCaptcha.ToLower();
                Ctx.UpdateCaptcha();
                TextCaptcha.Text = string.Empty;

                BindImageSourceUrl();
            }
        }

        private void BindImageSourceUrl()
        {
            //the qs is to ensure that the page realizes that this is a different request
            ImgCaptcha.Src = string.Format("/Controls/JpegImage.aspx?{0}", DateTime.Now.Ticks.ToString());
        }

        protected void ButtonRefreshCaptcha_Click(object sender, System.EventArgs e)
        {
            Ctx.UpdateCaptcha();

            BindImageSourceUrl();
        }

        protected bool _rebindImage = false;
        ProfileCommon profile;

        public void FillLists()
        {
            if (ddlMonth.Items.Count == 0)//Month
            {
                ddlMonth.Items.Add(new ListItem("--", string.Empty));
                for (int i = 1; i < 13; i++)
                    ddlMonth.Items.Add(new ListItem(i.ToString()));
            }
            if (ddlYear.Items.Count == 0)//Year
            {
                ddlYear.Items.Add(new ListItem("--", string.Empty));
                int start = DateTime.Now.Year;
                for (int i = start; i < (start + _Config._MaxYearsCardExpiry); i++)
                    ddlYear.Items.Add(new ListItem(i.ToString()));
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            //System.Web.UI.ScriptManager.RegisterStartupScript(this.wizEdit, this.wizEdit.GetType(),
            //   Guid.NewGuid().ToString(), " ;" + regFunc.ToString(), true);
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            //add a text changed JS event to uncheck the same as billing when the first name is changed
            //ensure this only happens once in js
            TextShipFirst.Attributes.Add("onChange", "EnsureShipCheck(this, 'CheckUseBilling');");
            Ctx.Cart.CartChanged += new WillCallWeb.StoreObjects.ShoppingCart.CartChangedEvent(EventHandler_CartChanged);

            regexCC.ValidationExpression = Utils.Validation.regexCC.ToString();

            BindImageSourceUrl();
        }

        public override void Dispose()
        {
            base.Dispose();

            Ctx.Cart.CartChanged -= new WillCallWeb.StoreObjects.ShoppingCart.CartChangedEvent(EventHandler_CartChanged);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if((Ctx.CurrentCartException != null) || 
                (Ctx.SessionAuthorizeNet != null && (!Ctx.SessionAuthorizeNet.IsAuthorized) &&
                (Ctx.SessionAuthorizeNet.ResponseReasonText != null && Ctx.SessionAuthorizeNet.ResponseReasonText.Trim().Length > 0)))
                PendingOperation.DeleteOperation(Ctx.SessionInvoice.Id);

            //this has to be checked here to allow master page to remove expired items
            if (!Ctx.Cart.HasItems)
                base.Redirect("/Store/Cart_Edit.aspx");

            //if we have an error
            if (Ctx.SessionAuthorizeNet != null && (!Ctx.SessionAuthorizeNet.IsAuthorized) &&
                (Ctx.SessionAuthorizeNet.ResponseReasonText != null && Ctx.SessionAuthorizeNet.ResponseReasonText.Trim().Length > 0))
            {
                //display the error
                ValidationSummaryCheckout.HeaderText = "There was an error processing your payment:";
                CustomValidation.IsValid = false;
                CustomValidation.ErrorMessage = Ctx.SessionAuthorizeNet.ResponseReasonText.Trim();
            }

            //set current user
            profile = this.Profile;

            if (!IsPostBack)
            {
                FillCountries();
                FillLists();
                FillUserData();
            }

            //allow for paid, partiallyrefunded, etc
            if (Ctx.SessionInvoice != null && Ctx.SessionInvoice.InvoiceStatus.ToLower() != "notpaid")
                Ctx.SessionInvoice = null;

            //avoid double clicks! always reset
            Ctx.OrderProcessingVariables = null;

            //always reset so as to create a new auth on every auth order request
            Ctx.SessionAuthorizeNet = null;

            BindCheckout();
        }

        private void BindCheckout()
        {
            shipinfo.Visible = _displayShippingSection;

            //turn off inputs if no items
            authorization.Visible = (!_displayShippingSection && (!Ctx.Cart.IsOverMaxTransactionAllowed));

            ButtonAuth.Visible = authorization.Visible && Ctx.Cart.HasItems && (!Ctx.Cart.IsOverMaxTransactionAllowed);

            continueshipping.Visible = (!authorization.Visible) && (!Ctx.Cart.IsOverMaxTransactionAllowed);

        }

        protected bool _CreditCardRequired
        {
            get { return ((Ctx.Cart.ChargeTotal > 0) || _displayShippingSection); }
        }

        protected bool ValidateInputControls()
        {
            bool useBillingAddress = CheckUseBilling.Checked;

            List<string> sb = new List<string>();
            
            //first, last add1, add2(not req), city, state, zip, country, phone, cardNumber, expiry, nameOnCard, securityCode
            if (TextFirst.Text.Trim().Length == 0)
                sb.Add("First name is required.");
            if (TextLast.Text.Trim().Length == 0)
                sb.Add("Last name is required.");

            string address1;
            address1 = TextAddress.Text.Trim();
            if (address1.Length == 0)
                sb.Add("Address is required.");

            if (TextCity.Text.Trim().Length == 0)
                sb.Add("City is required.");
            if (TextState.Text.Trim().Length == 0)
                sb.Add("State/province is required.");
            if (TextZip.Text.Trim().Length == 0)
                sb.Add("Zip/postal Code is required.");

            //country is automatically filled - cannot be empty
            if (ddlCountry.SelectedValue.Trim() == string.Empty)
                sb.Add("Country is required.");

            if (TextPhone.Text.Trim().Length == 0)
                sb.Add("Phone is required.");

            if(_CreditCardRequired)
            {
                string cardNumber = TextNumber.Text.Trim();
                System.Text.RegularExpressions.Match mCC = Utils.Validation.regexCC.Match(cardNumber);
                if (cardNumber.Length == 0 || mCC.Length < cardNumber.Length) 
                    sb.Add("Please enter a valid credit card number.");
                
                string selMonth = ddlMonth.SelectedValue;
                string selYear = ddlYear.SelectedValue;

                if (selMonth.Trim().Length == 0 || selYear.Trim().Length == 0)
                    sb.Add("Credit card expiry is required.");

                if (TextCardName.Text.Trim().Length == 0)
                    sb.Add("The name on the credit card is required.");
                if (TextCode.Text.Trim().Length == 0)
                    sb.Add("The credit card security code is required.");
            }

            //validate shipping only if check box not checked
            if (!useBillingAddress)
            {
                if (TextShipFirst.Text.Trim().Length == 0)
                    sb.Add("Shipping first name is required.");
                if (TextShipLast.Text.Trim().Length == 0)
                    sb.Add("Shipping last name is required.");

                string add1;
                add1 = TextShipAddress.Text.Trim();
                if (add1.Length == 0)
                    sb.Add("Shipping address is required.");

                if (TextShipCity.Text.Trim().Length == 0)
                    sb.Add("Shipping city is required.");
                if (TextShipState.Text.Trim().Length == 0)
                    sb.Add("Shipping state/province is required.");
                if (TextShipZip.Text.Trim().Length == 0)
                    sb.Add("Shipping zip/postal Code is required.");

                if (ddlShipCountry.SelectedValue.Trim() == string.Empty)
                    sb.Add("Shipping country is required.");

                if (TextShipPhone.Text.Trim().Length == 0)
                    sb.Add("Shipping phone is required.");
            }

            if (sb.Count > 0)
            {
                CustomValidation.ErrorMessage = "<ul>";
                foreach (string s in sb)
                {
                    CustomValidation.ErrorMessage += string.Format("<li>{0}</ li>", s);
                }
                CustomValidation.ErrorMessage += "</ ul>";

                CustomValidation.IsValid = false;

                TextCaptcha.Text = string.Empty;
                
            }

            return CustomValidation.IsValid;
        }

        protected bool VerifyShippingAddresses()
        {
            //verify ticket shipping address if necessary - only verify merch if we do not have usps enabled
            //usps will ship to POs
            if (Ctx.Cart.RequiresTicketShippingMethod || (Ctx.Cart.HasMerchandiseItems_Shippable && (!_Config._USPS_Enabled)))
            {
                //verify against po box
                string shippingAddress = (CheckUseBilling.Checked) ? string.Format("{0} {1}", TextAddress.Text.Trim(), TextAddress2.Text.Trim()) :
                    string.Format("{0} {1}", TextShipAddress.Text.Trim(), TextShipAddress2.Text.Trim());

                if (Utils.Shipping.IsPoBoxAddress(shippingAddress))
                {
                    CustomValidation.IsValid = false;
                    CustomValidation.ErrorMessage = "Sorry, your order contains items that cannot be shipped to PO Boxes.";

                    return false;
                }
            }

            return true;
        }
        
        protected void ButtonAuth_Click(object sender, System.EventArgs e)
        {
            if (Ctx.Cart.IsOverMaxTransactionAllowed)
                base.Redirect("/Store/Cart_Edit.aspx");

            if(Ctx.Cart.CartHasTicketItemsThatShouldBeConsideredExpired(this.Profile.UserName, this.Page.ToString()))
                base.Redirect("/Store/Cart_Edit.aspx");


            //todo message to tell user to login again if not authd
            if (Page.IsValid && this.Page.User.Identity.IsAuthenticated)
            {
                Ctx.UpdateCaptcha();//set it to null

                bool extraProtection = false;
                //don't allow to continue when removed tickets are shown
                //Cart_Main1.BindCart();

                if (Ctx.CurrentCartRemovals.Length > 0)
                {
                    extraProtection = true;
                }

                if (!Ctx.Cart.HasItems)
                {
                    CustomValidation.IsValid = false;
                    CustomValidation.ErrorMessage = "Your cart is empty!";
                    extraProtection = true;
                }

                if (extraProtection)
                {
                    ButtonAuth.Enabled = false;
                    return;
                }

                System.Text.StringBuilder query = new System.Text.StringBuilder();
                List<ListItem> queryParams = new List<ListItem>();

                //only if input is good
                if (SaveCustomerData(queryParams))
                {
                    if (authorization.Visible && Wcss._Config._SubscriptionsActive)
                        UserSubscriptions1.SaveSubscriptions();

                    string nameOnCard = TextCardName.Text.Trim();
                    Ctx.Cart.PurchaseName = string.Format("{0}, {1}", TextLast.Text.Trim(), TextFirst.Text.Trim());
                    decimal amountDue = Ctx.Cart.ChargeTotal;

                    string aspnetUserId = null;
                    int customerId = 0;
                    int cashewId = 0;

                    //Do the checkout
                    try
                    {  
                        //Always create the cassius even if the CC is not required
                        Cashew.CheckoutCashew(this.Profile.UserName, nameOnCard, TextNumber.Text.Trim(),
                            ddlMonth.SelectedValue, ddlYear.SelectedValue, ref aspnetUserId, ref customerId, ref cashewId);

                        string invoiceKey = (Ctx.SessionInvoice != null) ? Ctx.SessionInvoice.UniqueId :
                            AuthorizeNet.CreateInvoiceKey();

                        //FUTURE: allow choice of vendor
                        Vendor vendor = _Lookits.Vendors.GetVendor_Online();

                        //TODO: record sessionId and invoiceId
                        Ctx.InvoiceId = Ctx.Cart.CreateInvoiceForOrderFlow(invoiceKey, aspnetUserId, customerId, cashewId, 
                            "CustomerSite", this.Profile.UserName);
                        
                        //add bill ship
                        this.CheckoutBillShip(Ctx.InvoiceId, invoiceKey, aspnetUserId, customerId);

                        //at this point - verify shipping selections
                        if (VerifyShippingAddresses())
                        {
                            //add auth net
                            Ctx.AuthId = this.CheckoutAuthNet(Ctx.SessionInvoice, aspnetUserId, customerId, nameOnCard);

                            //these vars help us determine if order has been processed or not
                            Ctx.SetProcessingVariables(TextCardName.Text.Trim(), TextNumber.Text.Trim(), ddlMonth.SelectedValue,
                                ddlYear.SelectedValue.Trim(), TextCode.Text.Trim());

                            //Ctx.MonkeyWrench();

                            //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                            //at this point, the invoice, invoicebillship, invoiceitems and authnet have been created in the db

                            //process the order
                            if (_displayShippingSection)
                                base.Redirect("/Store/Shipping.aspx");
                            else
                                base.Redirect("/Store/ProcessingOrder.aspx");
                        }

                    }
                    catch (System.Threading.ThreadAbortException) { }
                    catch (Exception ex)
                    {
                        _Error.LogException(ex);

                        //display error message
                        CustomValidation.IsValid = false;
                        CustomValidation.ErrorMessage = string.Format("There was an error processing your order. Please contact technical support via our <a href=\"/Contact.aspx\">contact</a> page");

                        //clear out code box - rebind?
                        TextCaptcha.Text = string.Empty;
                    }
                }
            }
        }

        #region Checkouts

        /// <summary>
        /// Invoice products are set here
        /// </summary>
        private int CheckoutAuthNet(Invoice _invoice, string aspnetUserId, int customerId, string nameOnCard)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            SubSonic.QueryCommand cmd = new SubSonic.QueryCommand(string.Empty, SubSonic.DataService.Provider.Name);

            //formulate billing and shipping address
            string billingAddie = string.Format("{0} {1}", profile.ContactInfo.Address1, profile.ContactInfo.Address2).Trim();
            billingAddie = (billingAddie.Length > 60) ? billingAddie.Substring(0, 50) : billingAddie;

            string shippingAddie = string.Format("{0} {1}", _address1, _address2).Trim();
            shippingAddie = (shippingAddie.Length > 60) ? shippingAddie.Substring(0, 50) : shippingAddie;




            cmd.AddParameter("@appId", _invoice.ApplicationId, DbType.Guid);
            cmd.AddParameter("@invoiceId", _invoice.Id, DbType.Int32);
            cmd.AddParameter("@invoiceKey", _invoice.UniqueId, DbType.String);
            cmd.AddParameter("@userId", aspnetUserId, DbType.String);
            cmd.AddParameter("@customerId", customerId, DbType.Int32);
            cmd.AddParameter("@nameOnCard", nameOnCard, DbType.String);
            cmd.AddParameter("@ip", HttpContext.Current.Request.UserHostAddress, DbType.String);
            cmd.AddParameter("@authd", 0, DbType.Boolean);
            cmd.AddParameter("@taxDue", 0, DbType.Decimal);
            cmd.AddParameter("@method", "CC", DbType.String);
            cmd.AddParameter("@type", "auth_capture", DbType.String);
            cmd.AddParameter("@amountDue", Ctx.Cart.ChargeTotal, DbType.Decimal);

            //MATCH DESCRIPTION WITH INVOICE PRODUCTS
            //255 length for description
            System.Text.StringBuilder authdesc = new System.Text.StringBuilder();
            System.Text.StringBuilder invoiceProducts = new System.Text.StringBuilder();


            //WE DO THIS IN PARTS TO GET DESIRED ORDERING

            //TICKETS
            List<SaleItem_Ticket> collTix = new List<SaleItem_Ticket>();
            collTix.AddRange(Ctx.Cart.TicketItems);
            if(collTix.Count > 1)
                collTix.Sort(delegate(SaleItem_Ticket x, SaleItem_Ticket y) { return (x.ItemShowDate.CompareTo(y.ItemShowDate)); });

            foreach (SaleItem_Ticket item in collTix)
            {
                if (item.Ticket.IsPackage)
                    authdesc.Append("PKG ");

                //describe the root item
                string desc = string.Format("{0} {1}", item.Ticket.SalesDescription_Derived, item.Ticket.CriteriaText_Derived);
                if (desc.Trim().Length > 0)
                    desc = Utils.ParseHelper.StripHtmlTags(desc).Trim();

                authdesc.AppendFormat("{0}@ {1} {2} {3}{4}~", item.Quantity, item.ItemShowDate.ToString("MM/dd/yy hh:mmtt"),
                    (item.Ticket.AgeDescription.Trim().Length > 7) ? item.Ticket.AgeDescription.Trim().Substring(0, 7).Trim() : item.Ticket.AgeDescription.Trim(),
                    item.Ticket.ShowDateRecord.ShowRecord.ShowNamePart.ToUpper(), 
                    string.Format(" {0}", desc.Trim().Length > 0 ? desc.Trim() : string.Empty));

                //the other pkg tix need explanation too
                foreach(ShowTicket pkg in item.Ticket.LinkedShowTickets)
                    authdesc.AppendFormat("PKG {0}@ {1} {2} {3}~", item.Quantity, pkg.DateOfShow.ToString("MM/dd/yy hh:mmtt"),
                        (pkg.AgeDescription.Trim().Length > 7) ? pkg.AgeDescription.Trim().Substring(0, 7).Trim() : pkg.AgeDescription.Trim(),
                        pkg.ShowDateRecord.ShowRecord.ShowNamePart.ToUpper());//description is only for invoiceitem                

                invoiceProducts.Append(ShoppingCart.FormatItemProductListing(item));
            }

            ////LINKED TICKETS FOR SHIPPING - note packages are shown as the individual shows

            //MERCHANDISE ITEMS
            List<SaleItem_Merchandise> collMerch = new List<SaleItem_Merchandise>();
            collMerch.AddRange(Ctx.Cart.MerchandiseItems);
            if (collMerch.Count > 1)
                collMerch.Sort(delegate(SaleItem_Merchandise x, SaleItem_Merchandise y) { return (x.MerchItem.DisplayNameWithAttribs.CompareTo(y.MerchItem.DisplayNameWithAttribs)); });

            foreach (SaleItem_Merchandise item in collMerch)
            {
                authdesc.AppendFormat("{0}@ {1}~", item.Quantity, item.MerchItem.DisplayNameWithAttribs.Trim());
                invoiceProducts.Append(ShoppingCart.FormatItemProductListing(item));
            }

            //PROMOTIONS
            List<SaleItem_Promotion> collPromo = new List<SaleItem_Promotion>();
            collPromo.AddRange(Ctx.Cart.PromotionItems.FindAll(delegate(SaleItem_Promotion match) { return (match.SalePromotion != null); }));
            if (collPromo.Count > 1)
                collMerch.Sort(delegate(SaleItem_Merchandise x, SaleItem_Merchandise y) { return (x.MerchItem.DisplayNameWithAttribs.CompareTo(y.MerchItem.DisplayNameWithAttribs)); });

            foreach (SaleItem_Promotion item in collPromo)
            {
                string atts = string.Format("{0}@(promo:{1}) ", item.Quantity, item.tSalePromotionId);

                if (item.SalePromotion.IsTicketPromotion)
                {
                    ShowTicket st = item.SalePromotion.ShowTicketRecord;
                    atts += string.Format("{0}{1}~", item.SalePromotion.DisplayNameWithAttribs,
                        string.Format(" {0}", st.SalesDescription_Derived).Trim());
                    authdesc.Append((atts.Length > 45) ? atts.Substring(0, 44).Trim() : atts);
                    invoiceProducts.Append(ShoppingCart.FormatItemProductListing(item));
                }
                else if (item.SalePromotion.IsMerchPromotion)
                {
                    if (item.HasProductSelections)
                    {
                        atts += item.SalePromotion.DisplayNameWithAttribs;
                        authdesc.Append((atts.Length > 45) ? atts.Substring(0, 44).Trim() : atts);
                        invoiceProducts.Append(ShoppingCart.FormatItemProductListing(item));
                    }
                }
                else
                {
                    atts += item.SalePromotion.DisplayNameWithAttribs;
                    authdesc.Append((atts.Length > 45) ? atts.Substring(0, 44).Trim() : atts);
                    invoiceProducts.Append(ShoppingCart.FormatItemProductListing(item));
                }
            }


            //cleanup
            authdesc = authdesc.Replace(@"\", "-").Replace("/", "-").Replace("'", "");

            if (authdesc.Length > 255)
            {
                authdesc.Length = 255;
                if (authdesc[254] != '~')
                    authdesc[254] = '~';
            }

            cmd.AddParameter("@description", System.Text.RegularExpressions.Regex.Replace(authdesc.ToString(), @"\s+", " ").Trim(), DbType.String);

            cmd.AddParameter("@dupes", _Config._AuthorizeNetDuplicateSeconds, DbType.Int32);
            cmd.AddParameter("email", profile.UserName, DbType.String);

            cmd.AddParameter("@blCompany", profile.ContactInfo.CompanyName, DbType.String);
            cmd.AddParameter("@blFirst", profile.FirstName, DbType.String);
            cmd.AddParameter("@blLast", profile.LastName, DbType.String);
            cmd.AddParameter("@blAddress", billingAddie, DbType.String);
            cmd.AddParameter("@blCity", profile.ContactInfo.City, DbType.String);
            cmd.AddParameter("@blState", profile.ContactInfo.State, DbType.String);
            cmd.AddParameter("@blZip", profile.ContactInfo.PostalCode, DbType.String);
            cmd.AddParameter("@blCountry", profile.ContactInfo.Country, DbType.String);
            cmd.AddParameter("@blPhone", profile.ContactInfo.Phone, DbType.String);

            cmd.AddParameter("@company", _company, DbType.String);
            cmd.AddParameter("@first", _firstName, DbType.String);
            cmd.AddParameter("@last", _lastName, DbType.String);
            cmd.AddParameter("@address", shippingAddie, DbType.String);
            cmd.AddParameter("@city", _city, DbType.String);
            cmd.AddParameter("@state", _state, DbType.String);
            cmd.AddParameter("@zip", _zip, DbType.String);
            cmd.AddParameter("@country", _country, DbType.String);
            cmd.AddParameter("@phone", _phone, DbType.String);


            sb.Append("DECLARE @authNetId int SET @authNetId = 0 ");

            sb.Append("  BEGIN TRANSACTION ");

            //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            //SAVE SAME ORDER LIST AS AUTH DESCRIPTION 
            sb.AppendFormat("\r\nUPDATE [Invoice] SET [vcProducts] = '{0}' WHERE [Id] = @invoiceId \r\n ", invoiceProducts.ToString());

            sb.Append("INSERT INTO AuthorizeNet ([ApplicationId],[InvoiceNumber],[bAuthorized],[TInvoiceId],[UserId],[CustomerId], ");
            sb.Append("[Method],[TransactionType],[mAmount],[mTaxAmount],[Description],[iDupeSeconds], ");
            sb.Append("[Email],[FirstName],[LastName],[NameOnCard],[Company],[BillingAddress],[City],[State],[Zip], ");
            sb.Append("[Country],[Phone],[IpAddress],[ShipToFirstName],[ShipToLastName],[ShipToCompany], ");
            sb.Append("[ShipToAddress],[ShipToCity],[ShipToState],[ShipToZip],[ShipToCountry]) ");
            sb.Append("VALUES (@appId,@invoiceKey,@authd,@invoiceId,@userId,@customerId,@method,@type,@amountDue,@taxDue, ");
            sb.Append("@description,@dupes,@email,@blFirst,@blLast,@nameOnCard,@blCompany,@blAddress, ");
            sb.Append("@blCity,@blState,@blZip,@blCountry,@blPhone,@ip,@first,@last,@company,@address, ");
            sb.Append("@city,@state,@zip,@country) ");

            sb.Append("  COMMIT TRANSACTION ");

            sb.Append(" SET @authNetId = SCOPE_IDENTITY() ");

            sb.Append(" SELECT @authNetId ");

            cmd.CommandSql = sb.ToString();

            try
            {
                int authNetId = (int)SubSonic.DataService.ExecuteScalar(cmd);

                if (authNetId < 10000)
                {
                    string error = string.Format("{2}{0}CheckoutAuth could not be completed {0}returnValue: {1}{0}invoiceId: {3}{0}nameOnCard: {4}{0}ip: {5}{0}",
                        Utils.Constants.NewLine, authNetId, DateTime.Now, _invoice.Id, nameOnCard, HttpContext.Current.Request.UserHostAddress);

                    _Error.LogException(new Exception(error));

                    throw new Exception("Error ocurred during CheckoutAuthNet");
                }

                return authNetId;
            }
            catch (System.Data.SqlClient.SqlException sex)
            {
                _Error.LogException(sex);
                throw new Exception(string.Format("CheckoutAuthNet Sql Error.\r\n{0}\r\n{1}", sex.Message, sex.StackTrace));
            }
            catch (Exception ex)
            {
                _Error.LogException(ex);
                throw new Exception(string.Format("CheckoutAuthNet Error.\r\n{0}\r\n{1}", ex.Message, ex.StackTrace));
            }
        }
        private void CheckoutBillShip(int invoiceId, string invoiceKey, string aspnetUserId, int customerId)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            SubSonic.QueryCommand cmd = new SubSonic.QueryCommand(string.Empty, SubSonic.DataService.Provider.Name);

            cmd.AddParameter("@invoiceId", invoiceId, DbType.Int32);
            cmd.AddParameter("@invoiceKey", invoiceKey, DbType.String);
            cmd.AddParameter("@userId", aspnetUserId, DbType.String); 
            cmd.AddParameter("@customerId", customerId, DbType.Int32);

            cmd.AddParameter("@blCompany", profile.ContactInfo.CompanyName, DbType.String);
            cmd.AddParameter("@blFirst", profile.FirstName, DbType.String);
            cmd.AddParameter("@blLast", profile.LastName, DbType.String);
            cmd.AddParameter("@blAddress1", profile.ContactInfo.Address1, DbType.String);
            cmd.AddParameter("@blAddress2", profile.ContactInfo.Address2, DbType.String);
            cmd.AddParameter("@blCity", profile.ContactInfo.City, DbType.String);
            cmd.AddParameter("@blState", profile.ContactInfo.State, DbType.String);
            cmd.AddParameter("@blZip", profile.ContactInfo.PostalCode, DbType.String);
            cmd.AddParameter("@blCountry", profile.ContactInfo.Country, DbType.String);
            cmd.AddParameter("@blPhone", profile.ContactInfo.Phone, DbType.String);

            cmd.AddParameter("@sameAsBilling", (_sameAsBilling) ? 1 : 0, DbType.Boolean);
            cmd.AddParameter("@company", _company ?? string.Empty, DbType.String);
            cmd.AddParameter("@first", _firstName, DbType.String);
            cmd.AddParameter("@last", _lastName, DbType.String);
            cmd.AddParameter("@address1", _address1, DbType.String);
            cmd.AddParameter("@address2", _address2, DbType.String);
            cmd.AddParameter("@city", _city, DbType.String);
            cmd.AddParameter("@state", _state.ToUpper(), DbType.String);
            cmd.AddParameter("@zip", _zip, DbType.String);
            cmd.AddParameter("@country", _country.ToUpper(), DbType.String);
            cmd.AddParameter("@phone", _phone, DbType.String);
            
            sb.Append("IF EXISTS (SELECT * FROM InvoiceBillShip WHERE [TInvoiceId] = @invoiceId) BEGIN ");
            sb.Append("UPDATE InvoiceBillShip ");
            sb.Append("SET [blCompany] = @blCompany,[blFirstName] = @blFirst,[blLastName] = @blLast,[blAddress1] = @blAddress1, ");
            sb.Append("[blAddress2] = @blAddress2,[blCity] = @blCity,[blStateProvince] = @blState,[blPostalCode] = @blZip, ");
            sb.Append("[blCountry] = @blCountry,[blPhone] = @blPhone,[bSameAsBilling] = @sameAsBilling, ");
            sb.Append("[CompanyName] = @company,[FirstName] = @first,[LastName] = @last,[Address1] = @address1,[Address2] = @address2, ");
            sb.Append("[City] = @city,[StateProvince] = @state,[PostalCode] = @zip,[Country] = @country,[Phone] = @phone ");
            sb.Append("WHERE [TInvoiceId] = @invoiceId ");
            sb.Append("END ");

            sb.Append("ELSE BEGIN ");
            sb.Append("INSERT INTO InvoiceBillShip([tInvoiceId],[UserId],[CustomerId],[blCompany],[blFirstName],[blLastName],[blAddress1],[blAddress2], ");
            sb.Append("[blCity],[blStateProvince],[blPostalCode],[blCountry],[blPhone],[bSameAsBilling], ");
            sb.Append("[CompanyName],[FirstName],[LastName],[Address1],[Address2],[City],[StateProvince],[PostalCode],[Country],[Phone]) ");
            sb.Append("VALUES (@invoiceId,@userId,@customerId,@blCompany,@blFirst,@blLast,@blAddress1,@blAddress2, ");
            sb.Append("@blCity,@blState,@blZip,@blCountry,@blPhone,@sameAsBilling, ");
            sb.Append("@company,@first,@last,@address1,@address2,@city,@state,@zip,@country,@phone) ");
            sb.Append("END ");

            cmd.CommandSql = sb.ToString();

            try
            {
                SubSonic.DataService.ExecuteScalar(cmd);

                if (Ctx.SessionInvoice != null && Ctx.SessionInvoice.InvoiceBillShip != null)
                {
                    int invId = Ctx.SessionInvoice.Id;
                    Ctx.SessionInvoice = null;
                    Ctx.InvoiceId = invId;
                }
            
            }
            catch (System.Data.SqlClient.SqlException sex)
            {
                _Error.LogException(sex);
                throw new Exception(string.Format("CheckoutBillShip Sql Error.\r\n{0}\r\n{1}", sex.Message, sex.StackTrace));
            }
            catch (Exception ex)
            {
                _Error.LogException(ex);
                throw new Exception(string.Format("CheckoutBillShip Error.\r\n{0}\r\n{1}", ex.Message, ex.StackTrace));
            }
        }
        #endregion

        public void FillUserData()
        {
            if (profile != null && (! profile.IsAnonymous))
            {
                TextFirst.Text      = (profile.FirstName != null)               ? profile.FirstName : string.Empty;
                TextLast.Text       = (profile.LastName != null)                ? profile.LastName : string.Empty;
                TextAddress.Text    = (profile.ContactInfo.Address1 != null)    ? profile.ContactInfo.Address1 : string.Empty;
                TextAddress2.Text   = (profile.ContactInfo.Address2 != null)    ? profile.ContactInfo.Address2 : string.Empty;
                TextCity.Text       = (profile.ContactInfo.City != null)        ? profile.ContactInfo.City : string.Empty;
                TextState.Text      = (profile.ContactInfo.State != null)       ? Utils.ParseHelper.GetStateCode(profile.ContactInfo.State) : string.Empty;
                TextZip.Text        = (profile.ContactInfo.PostalCode != null)  ? profile.ContactInfo.PostalCode : string.Empty;
                TextPhone.Text      = (profile.ContactInfo.Phone != null)       ? profile.ContactInfo.Phone : string.Empty;

                //existing vars - card, month, year, nameoncard, securitycode

                if (Ctx.OrderProcessingVariables == null || Ctx.OrderProcessingVariables.Trim().Length == 0)
                {
                    if (_Config._TicketTestMode)
                    {
                        //cardtype,cardnumber,month,year,nameoncard,securitycode
                        //ddlCardType.SelectedIndex = 1;
                        TextNumber.Text = "4111111111111111";
                        ddlMonth.SelectedIndex = DateTime.Now.Month;
                        ddlYear.SelectedIndex = 3;
                        TextCardName.Text = "Jimmy test Bob";
                        TextCode.Text = "111";

                        CheckTerms.Checked = true;
                    }
                }
                else
                {
                    string[] parts = Ctx.OrderProcessingVariables.Split('~');//cardName,cardNumber, expMonth,expYear,securityCode
                    string cardName = parts[0];
                    string cardNumber = parts[1];
                    string expMonth = parts[2];
                    string expYear = parts[3];
                    string security = parts[4];

                    TextNumber.Text = cardNumber;
                    ddlMonth.SelectedIndex = -1;
                    ddlMonth.Items.FindByText(expMonth).Selected = true;
                    ddlYear.SelectedIndex = -1;
                    ddlYear.Items.FindByText(expYear).Selected = true;
                    TextCardName.Text = cardName;
                    TextCode.Text = security;

                    //do shipping if it exists
                    if (Ctx.SessionInvoice != null && Ctx.SessionInvoice.InvoiceBillShip != null && (!Ctx.SessionInvoice.InvoiceBillShip.SameAsBilling))
                    {
                        InvoiceBillShip ship = Ctx.SessionInvoice.InvoiceBillShip;
                        CheckUseBilling.Checked = false;
                        TextShipFirst.Text = ship.FirstName;
                        TextShipLast.Text = ship.LastName;
                        TextShipAddress.Text = ship.Address1;
                        TextShipAddress2.Text = ship.Address2;
                        TextShipCity.Text = ship.City;
                        TextShipState.Text = ship.StateProvince;
                        TextShipZip.Text = ship.PostalCode;
                        //txtShipCountry.Text = ship.Country;
                        TextShipPhone.Text = ship.Phone;
                    }
                }
            }
        }

        private bool SaveCustomerData(List<ListItem> items)
        {
            if (ValidateInputControls())
            {
                //if we dont have a billship than make a new one - otherwise update
                //_displayShippingSection
                //this is checked to true if section ins not visible
                _sameAsBilling = (CheckUseBilling.Checked);

                if ((!_sameAsBilling))
                {
                    //all shipping input is required except for address2 and companyname and ddlCOuntry is auto
                    _firstName = TextShipFirst.Text.Trim();
                    _lastName = TextShipLast.Text.Trim();
                    _address1 = TextShipAddress.Text.Trim();
                    _address2 = TextShipAddress2.Text.Trim();
                    _city = TextShipCity.Text.Trim();
                    _state = TextShipState.Text.Trim();
                    _zip = TextShipZip.Text.Trim();
                    _country = ddlShipCountry.SelectedValue;
                    _phone = TextShipPhone.Text.Trim();
                }

                //update customer profile
                if (profile.FirstName != TextFirst.Text.Trim())
                    profile.FirstName = TextFirst.Text.Trim();

                if (profile.LastName != TextLast.Text.Trim())
                    profile.LastName = TextLast.Text.Trim();

                if (profile.ContactInfo.Address1 != TextAddress.Text.Trim())
                    profile.ContactInfo.Address1 = TextAddress.Text.Trim();

                if (profile.ContactInfo.Address2 != TextAddress2.Text.Trim())
                    profile.ContactInfo.Address2 = TextAddress2.Text.Trim();

                if (profile.ContactInfo.City != TextCity.Text.Trim())
                    profile.ContactInfo.City = TextCity.Text.Trim();

                if (profile.ContactInfo.State != TextState.Text.Trim().ToUpper())
                    profile.ContactInfo.State = TextState.Text.Trim().ToUpper();

                if (profile.ContactInfo.PostalCode != TextZip.Text.Trim())
                    profile.ContactInfo.PostalCode = TextZip.Text.Trim();

                if (profile.ContactInfo.Country != ddlCountry.SelectedValue.Trim().ToUpper())
                    profile.ContactInfo.Country = ddlCountry.SelectedValue.Trim().ToUpper();

                if (profile.ContactInfo.Phone != TextPhone.Text.Trim())
                    profile.ContactInfo.Phone = TextPhone.Text.Trim();

                profile.Save();

                return true;
            }

            return false;
        }

        private void BindPage()
        {
            TextCaptcha.Text = string.Empty;
        }
        private void FillCountries()
        {
            ddlCountry.DataBind();
            ddlShipCountry.DataBind();
        }

        protected void ddlCountry_DataBinding(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;

            if (ddl.Items.Count == 0)
            {
                ddl.DataSource = Utils.ListFiller.CountryListing;
                ddl.DataTextField = "Text";
                ddl.DataValueField = "Value";
            }
        }
        protected void ddlCountry_DataBound(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            bool isShipCountry = ddl.ID == "ddlShipCountry";

            ddl.SelectedIndex = -1;

            string country = (isShipCountry) ? 
                ((Ctx.SessionInvoice != null && Ctx.SessionInvoice.InvoiceBillShip != null &&
                Ctx.SessionInvoice.InvoiceBillShip.Country != null && Ctx.SessionInvoice.InvoiceBillShip.Country.Trim().Length > 0) ?
                Ctx.SessionInvoice.InvoiceBillShip.Country.Trim() : _Config._Default_CountryCode) :                 
                ((profile.ContactInfo.Country != null && profile.ContactInfo.Country.Trim().Length > 0) ?
                profile.ContactInfo.Country.Trim() : _Config._Default_CountryCode);

            ListItem li = ddl.Items.FindByValue(country);

            if (li != null)
                li.Selected = true;
            else
                ddl.SelectedIndex = 0;
        }
    }
}
