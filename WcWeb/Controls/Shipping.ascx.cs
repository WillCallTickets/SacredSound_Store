using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using Wcss;
using WillCallWeb.StoreObjects;

namespace WillCallWeb.Controls
{
    public partial class Shipping : WillCallWeb.BaseControl, System.Web.SessionState.IRequiresSessionState
    {
        #region Page Overhead

        protected bool _displayShippingSection 
        { 
            get
            {
                //if the cart has item
                return ((Ctx.Cart.HasTicketItems && _Config._Shipping_Tickets_Active) ||
                    (Ctx.Cart.HasMerchandiseItems_Shippable && _Config._Shipping_Merch_Active));
            } 
        }
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
        
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            BindImageSourceUrl();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            //this has to be checked here to allow master page to remove expired items
            try
            {
                if (Ctx == null || Ctx.Cart == null)
                {
                    //log the ip and the referrer
                    string info = string.Format("REDIRECTED chooseticket: {0} - no context or no cart - IP: {1} Referrer: {2}", DateTime.Now.ToString("MM/dd/yyyy hh:mmtt"), Request.UserHostAddress,
                        (Request.UrlReferrer == null) ? "null" : Request.UrlReferrer.ToString());

                    _Error.LogException(new Exception(info));

                    base.Redirect("/Store/ChooseTicket.aspx");
                }

                if (!Ctx.Cart.HasItems)
                    base.Redirect("/Store/Cart_Edit.aspx");
            }
            catch (System.Threading.ThreadAbortException) { }
            catch (Exception ex)
            {
                _Error.LogException(ex);
            }

            //Ctx.MonkeyWrench();

            //we should only arrive here from the checkout page
            if (Request.UrlReferrer != null)
            {
                if (!(Request.UrlReferrer.AbsolutePath.ToLower() == "/store/checkout.aspx" || Request.UrlReferrer.AbsolutePath.ToLower() == "/store/shipping.aspx"))
                {
                    _Error.LogException(new Exception(string.Format("REDIRECTED edit: Shipping.ascx - referrer not valid - referrer: {0}", Request.UrlReferrer.ToString())));

                    base.Redirect("/Store/Cart_Edit.aspx");
                }
            }

            if ((Ctx.SessionAuthorizeNet == null) || (Ctx.SessionAuthorizeNet != null && (!Ctx.SessionAuthorizeNet.IsAuthorized) &&
                (Ctx.SessionAuthorizeNet.ResponseReasonText != null && Ctx.SessionAuthorizeNet.ResponseReasonText.Trim().Length > 0)))
            {
                string error = (Ctx.SessionAuthorizeNet == null) ? "is null" : "in error";

                _Error.LogException(new Exception(string.Format("REDIRECTED checkout: Authorize net {0} - referrer: {1}", error, 
                    (Request.UrlReferrer != null) ? Request.UrlReferrer.ToString() : "is null")));

                base.Redirect("/Store/Checkout.aspx");
            }

            //allow for paid, partiallyrefunded, etc
            if (Ctx.SessionInvoice == null || (Ctx.SessionInvoice != null && Ctx.SessionInvoice.InvoiceStatus.ToLower() != "notpaid"))
            {
                if (_Config._LogRenderError && Ctx.SessionInvoice == null)
                    _Error.LogException(new Exception("REDIRECTED checkout: Ctx.SessionInvoice is null in shipping - redirected"));
                
                Ctx.SessionInvoice = null;
                base.Redirect("/Store/Checkout.aspx");
            }

            //set current user
            if (this.Profile == null)
            {
                _Error.LogException(new Exception("REDIRECTED register with return url here: Profile is null on controls/ship page"));
                base.Redirect(string.Format("/Register.aspx?ReturnUrl={0}", this.Request.Path));
            }

            profile = this.Profile;
            
            if (!IsPostBack)
            {
                Ctx.Cart.Shipments_Merch.Clear();
            }

            //turn off inputs if no items
            ButtonAuth.Enabled = Ctx.Cart.HasItems && (!Ctx.Cart.IsOverMaxTransactionAllowed);
        }

        #endregion

        protected bool VerifyShippingSelections()
        {
            System.Collections.Generic.List<string> errors = new System.Collections.Generic.List<string>();

            //verify ticket shipping
            if (Ctx.Cart.RequiresTicketShippingMethod)
            {
                //if any tickets must ship...be sure a mthod is chosen
                if (Ctx.Cart.Shipments_Tickets.Count == 0)
                {
                    _Error.WriteDebugger("There are tickets without shipping methods.");
                    errors.Add("Shipping is required for some of your ticket items.");
                }
            }
            //verify merch shipping
            if (Ctx.Cart.HasMerchandiseItems_Shippable)
            {
                //all merch items must have an associated saleitem_shipping in the cart
                foreach (SaleItem_Merchandise sim in Ctx.Cart.MerchandiseItems)
                {
                    //search saleItemShipping items to find the ship method that contains
                    if (sim.HasParcelDelivery)
                    {
                        SaleItem_Shipping sip = Ctx.Cart.Shipments_Merch.Find(delegate(SaleItem_Shipping match) { return (match.Items_Merch.Contains(sim)); });

                        if (sip == null)
                        {
                            _Error.WriteDebugger("There are merchandise items without shipping methods.");
                            errors.Add(string.Format("Please select a shipping method for {0}", sim.MerchItem.DisplayNameWithAttribs));
                        }
                        else if (sim.MerchItem.IsFlatShip && sip.ShipMethod.IndexOf("ups ") != -1 && Utils.Shipping.IsPoBoxAddress(Ctx.SessionInvoice.WorkingShippingAddress))
                            errors.Add("UPS will not ship to PO Boxes.");
                    }

                }
            }

            if (errors.Count == 0)
                return true;

            CustomValidation.IsValid = false;
            CustomValidation.ErrorMessage = "<ul>";

            foreach (string s in errors)
                CustomValidation.ErrorMessage += string.Format("<li>{0}</li>", s);

            CustomValidation.ErrorMessage += "</ul>";

            return false;
        }

        private bool SetupPromotions()
        {
            //examine the totals cart promos - and if there are any visible ddls - then assign awards
            WillCallWeb.Components.Cart.Cart_Totals tot = (WillCallWeb.Components.Cart.Cart_Totals)this.Cart1.FindControl("Cart_Totals1");
            
            if (tot != null)
            {
                Repeater rpt = (Repeater)tot.FindControl("rptPromotion");
                if (rpt != null)
                {
                    foreach (RepeaterItem itm in rpt.Items)
                    {
                        RadioButtonList ddl = (RadioButtonList)itm.FindControl("ddlAwards");
                        if (ddl != null && ddl.Visible)
                        {
                            //estb objects - itm.DataItem is null
                            //we need to go thru all the collections - because the repeater may order them differently than the cart
                            List<SaleItem_Promotion> promoCollection = (List<SaleItem_Promotion>)rpt.DataSource;
                            SaleItem_Promotion promotionItem = promoCollection[itm.ItemIndex];

                            //if we do not have a promotional sale item - create one
                            if (promotionItem != null)
                            {
                                //WHAT WAS SELECTED AS AN AWARD?
                                //deal with no selection
                                if (ddl.SelectedValue == "0")
                                {
                                    CustomAuth.IsValid = false;
                                    CustomAuth.ErrorMessage = "You have not selected your free gift.";
                                    return true;
                                }
                                //deal with customer choosing no option
                                else if (ddl.SelectedValue == _Config._NoSelectionIdValue.ToString())
                                {
                                    //note promo with this choice
                                    if (promotionItem != null)
                                    {
                                        promotionItem.CustomerOptIn = false;
                                    }
                                }
                                else//assign a selection to the promotion
                                {
                                    if (promotionItem != null)
                                    {
                                        int chosenIdx = int.Parse(ddl.SelectedValue);

                                        //check for inventory via sql
                                        //if we get a positive result - then add the item
                                        //else show an error and reset selected(or dont set)
                                        //string sql = "SELECT * FROM Merch m WHERE Id = @idx AND m.[iAvailable] > 0 ";

                                        //we can get away with doing this here without storing in another table because we will deal before any 
                                        //possible custom input

                                        //PROMOTIONS ARE NOT PLACED INTO PENDING TABLE!
                                        int affected = (int)SPs.TxInventoryRealTimeAvailability(chosenIdx, 
                                            _Enums.InvoiceItemContext.merch.ToString()).ExecuteScalar();

                                        //#w
                                        ////string sql = "UPDATE [Merch] SET [--ending] = [iPending] + 1 FROM [Merch] m WHERE m.[Id] = @idx AND m.[iAvailable] > 0 ";
                                        ////SubSonic.QueryCommand cmd = new SubSonic.QueryCommand(sql, SubSonic.DataService.Provider.Name);
                                        ////cmd.Parameters.Add("@idx", chosenIdx, DbType.Int32);

                                        //////if we get a row back from this query than go forward - else error
                                        ////int affected = SubSonic.DataService.ExecuteQuery(cmd);

                                        if (affected == 0)
                                        {
                                            CustomAuth.IsValid = false;
                                            CustomAuth.ErrorMessage = "We're sorry, the promotional item you selected is no longer in stock.";

                                            return true;
                                        }

                                        promotionItem.CustomerOptIn = true;
                                        promotionItem.AddSelectedAward(chosenIdx);
                                        promotionItem.Quantity = 1;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return false;
        }

        protected void ButtonAuth_Click(object sender, System.EventArgs e)
        {
            //todo message to tell user to login again if not authd
            if (Ctx.Cart.IsOverMaxTransactionAllowed)
                base.Redirect("/Store/Cart_Edit.aspx");

            if (Ctx.Cart.CartHasTicketItemsThatShouldBeConsideredExpired(this.Profile.UserName, this.Page.ToString()))
                base.Redirect("/Store/Cart_Edit.aspx");

            if (Page.IsValid && this.Page.User.Identity.IsAuthenticated && (!Ctx.Cart.IsOverMaxTransactionAllowed))
            {
                Ctx.UpdateCaptcha();//set it to null

                bool extraProtection = false;

                if (Wcss._Config._SubscriptionsActive)
                    UserSubscriptions1.SaveSubscriptions();

                //don't allow to continue when removed tickets are shown

                if (Ctx.CurrentCartRemovals.Length > 0)
                {
                    extraProtection = true;
                }

                if (!Ctx.Cart.HasItems)
                {
                    CustomAuth.IsValid = false;
                    CustomAuth.ErrorMessage = "Your cart is empty!";
                    extraProtection = true;
                }

                if (extraProtection)
                {
                    ButtonAuth.Enabled = false;
                    return;
                }

                //only if input is good
               if (VerifyShippingSelections())
                {
                   //if all is good - checkout promotions
                    bool extProtection = SetupPromotions();
                    if (extProtection)
                    {
                        return;
                    }

                    //Do the checkout
                    try
                    {
                        //SAVE SHIPPING INFO
                        //UPDATE THE INVOICE WITH SHIPPING INFO
                        //find the input values
                        string message = txtMessage.Text.Trim();

                        UpdateTotalForAuthorization(Ctx.SessionAuthorizeNet, message);

                        //Ctx.MonkeyWrench();

                        //process the order
                        base.Redirect("/Store/ProcessingOrder.aspx");

                    }
                    catch (System.Threading.ThreadAbortException) { }
                    catch (Exception ex)
                    {
                        _Error.LogException(ex);

                        //display error message
                        CustomAuth.IsValid = false;
                        CustomAuth.ErrorMessage = string.Format("There was an error processing your order. Please contact technical support via our <a href=\"/Contact.aspx\">contact</a> page");

                        //clear out code box - rebind?
                        TextCaptcha.Text = string.Empty;
                    }
                }
            }
        }

        private void UpdateTotalForAuthorization(AuthorizeNet _auth, string message)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            SubSonic.QueryCommand cmd = new SubSonic.QueryCommand(string.Empty, SubSonic.DataService.Provider.Name);

            _auth.Amount = Ctx.Cart.ChargeTotal;            

            cmd.AddParameter("@invId", _auth.TInvoiceId, DbType.Int32);
            cmd.AddParameter("@invTotal", _auth.Amount, DbType.Decimal);
            cmd.AddParameter("@authId", _auth.Id, DbType.Int32);

            decimal merchTotal = 0;
            foreach (SaleItem_Shipping sis in Ctx.Cart.Shipments_All)
            {
                if (sis.Items_Merch.Count > 0)
                    foreach (SaleItem_Merchandise sim in sis.Items_Merch)
                        merchTotal += sim.LineTotal;
            }

            _auth.FreightAmount = Ctx.Cart.ShippingAndHandling;
            cmd.AddParameter("@totalShipCost", _auth.FreightAmount, DbType.Decimal);

            
            cmd.AddParameter("@handlingCharge", ecommercemax_shipping.ComputeHandlingFee(Ctx.Cart, merchTotal), DbType.Decimal);
            cmd.AddParameter("@message", message.Trim(), DbType.String);

            sb.Append("BEGIN TRANSACTION ");

            // Ensure that invoice has latest total!!
            sb.Append("UPDATE [Invoice] SET [mBalanceDue] = @invTotal WHERE [Id] = @invId; ");

            sb.Append("UPDATE InvoiceBillShip ");
            sb.Append("SET [ShipMessage] = @message, [mHandlingComputed] = @handlingCharge ");
            sb.Append("FROM InvoiceBillShip WHERE [tInvoiceId] = @invId; ");

            //Auth net
            sb.Append("UPDATE [AuthorizeNet] SET [mFreightAmount] = @totalShipCost, [mAmount] = @invTotal WHERE [Id] = @authId; ");
            
            sb.Append("COMMIT TRANSACTION ");

            cmd.CommandSql = sb.ToString();

            try
            {
                SubSonic.DataService.ExecuteScalar(cmd);
            }
            catch (System.Data.SqlClient.SqlException sex)
            {
                _Error.LogException(sex);
                throw new Exception(string.Format("Update Invoice with ship method Sql Error.\r\n{0}\r\n{1}", sex.Message, sex.StackTrace));
            }
            catch (Exception ex)
            {
                _Error.LogException(ex);
                throw new Exception(string.Format("Update Invoice with ship method Error.\r\n{0}\r\n{1}", ex.Message, ex.StackTrace));
            }
        }

        private void BindPage()
        {
            TextCaptcha.Text = string.Empty;
        }
}
}
