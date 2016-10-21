using System;
using System.Data;
using System.Text;
using System.Web.Services;
using System.Collections.Generic;

using Wcss;
using WillCallWeb;
using WillCallWeb.StoreObjects;
using SubSonic;

public partial class Store_Confirmation : WillCallWeb.BasePage
{
    

    protected AuthorizeNet _auth;
    private string _invNum = null;
    private string InvNum { get { return _invNum; } set { _invNum = value; } }

    protected override void OnPreInit(EventArgs e)
    {
        QualifySsl(true);
        base.OnPreInit(e);
    }

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);//must have this to get theme template - link to master
        InvNum = Request["inv"];
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        //load appropriate panel
        string detailToLoad = @"\PurchaseDetails";
        panelDetails.Controls.Add(LoadControl(string.Format(@"..\Controls{0}.ascx", detailToLoad)));

        string cartToLoad = @"\Cart_Purchase";
        panelCart.Controls.Add(LoadControl(string.Format(@"..\Controls{0}.ascx", cartToLoad)));

        StringBuilder google = new StringBuilder();
        StringBuilder sb = new StringBuilder();
        StringBuilder products = new StringBuilder();
        StringBuilder postText = new StringBuilder();
        QueryCommand postCmd = new QueryCommand(string.Empty, DataService.Provider.Name);

        ProfileCommon purchaseProfile = this.Profile;
        string userName = purchaseProfile.UserName;


        #region New Orders

        //Ctx.MonkeyWrench();        

        if (Ctx.OrderProcessingVariables != null)//this means we haven't processed the auth attempt yet
        {
            if(! ConfirmationChecker.CheckCouponCodeValidity(Ctx, userName))
                base.Redirect("/Store/Checkout.aspx");

            DateTime now = DateTime.Now;
            string[] processVars = Ctx.OrderProcessingVariables.Split('~');//cardName,cardNumber, expMonth,expYear,securityCode
            string lastFour = (processVars[1].Trim().Length == 0) ? "" : processVars[1].Substring((processVars[1].Trim().Length - 4), 4);
            string itemList = Utils.ParseHelper.StripHtmlTags(Ctx.Cart.ItemList_Delimited);
            

            //string itemList = Utils.ParseHelper.StripHtmlTags( Ctx.Cart.ItemList_Delimited;
            string unqId = Ctx.SessionInvoice.UniqueId;

            try
            {
                #region Begin Process Order
                
                //IMMEDIATE
                //are shipping discounts reflected in auth??
                //session invoice should be created/instantiated from id in checkout
                Ctx.SessionAuthorizeNet.SetAuthData(Ctx.SessionInvoice, purchaseProfile,
                    processVars[0], processVars[1], processVars[2], processVars[3], processVars[4], Ctx.Cart.ChargeTotal, Request.UserHostAddress);

                Ctx.OrderProcessingVariables = null;

                ConfirmationChecker.CheckStoreCreditLimit(Ctx, purchaseProfile);
                

                //begin follow flow
                Ctx.SessionAuthorizeNet.SendPayment(itemList);


                //if the use is frauding the order on the store credit - if it is a neg amount


                //Reinstate
                if (Ctx.SessionAuthorizeNet != null)
                {
                    if (Ctx.SessionAuthorizeNet.BillingAddress != null && Ctx.SessionAuthorizeNet.BillingAddress.Length > 60)
                        Ctx.SessionAuthorizeNet.BillingAddress = Ctx.SessionAuthorizeNet.BillingAddress.Substring(0, 58);
                    if (Ctx.SessionAuthorizeNet.ShipToAddress != null && Ctx.SessionAuthorizeNet.ShipToAddress.Length > 60)
                        Ctx.SessionAuthorizeNet.ShipToAddress = Ctx.SessionAuthorizeNet.ShipToAddress.Substring(0, 58);
                }

                Ctx.SessionAuthorizeNet.Save();


                ////TESTING ONLY!!!!!!!!
                //Ctx.SessionAuthorizeNet.IsAuthorized = false;
                //Ctx.SessionAuthorizeNet.ResponseReasonText = "testing errors - bogus payment";

                //Ctx.SessionAuthorizeNet.Save();

                //base.Redirect("/Store/Checkout.aspx");
                ////END TESTING ONLY


                //IF NOT AUTHORIZED!!!!!
                if (!Ctx.SessionAuthorizeNet.IsAuthorized)
                {
                    try
                    {
                        //log the failure as a user event
                        UserEvent.NewUserEvent(userName, now, now, _Enums.EventQStatus.Failed, userName,
                            _Enums.EventQContext.User, _Enums.EventQVerb.AuthDecline, null, Ctx.SessionInvoice.BalanceDue.ToString("c"),
                            Ctx.SessionAuthorizeNet.ResponseReasonText, true);

                        //reset inventory on promotions
                        //090515 - NO NEED TO DO THIS - promotions are not placed into pending stock
                    }
                    catch (Exception ex)
                    {
                        _Error.LogException(ex);
                    }

                    base.Redirect("/Store/Checkout.aspx");
                }

                //Ctx.MonkeyWrench();

                #endregion

                //Begin Google tracking
                //ConfirmationTracker.Google_InitTracking(google, unqId, Ctx);
                ConfirmationTracker.Analytics_InitTrans(google, unqId, Ctx);

                int counter = 0;//i is not numbered by item id - it might overlap an id in merch and tix
                QueryCommand cmd = new QueryCommand(string.Empty, DataService.Provider.Name);
                

                ConfirmationQuery.InitConfirmationQuery(cmd, Ctx, lastFour);

                ConfirmationQuery.ConstructCartSql(sb, Ctx, cmd, google, unqId, products, ref counter, now);

                ConfirmationQuery.ExecuteCartSql(cmd);

                ConfirmationQuery.RecordCoupons(Ctx, userName);

                ConfirmationChecker.CleanupStoreCreditOperations(Ctx, purchaseProfile);


                //POST PURCHASE TEXT
                ConfirmationHelper.ConstructPostText(userName, postText, postCmd, Ctx);

                //Send out any notifications for items that were sold that may be tracked
                ConfirmationChecker.CheckNotificationTracking(Ctx);

                //submit any info necessary for age submissions/verifications
                try
                {
                    bool logVerification = false;
                    foreach (SaleItem_Merchandise item in Ctx.Cart.MerchandiseItems)
                    {
                        Merch parentItem = (item.MerchItem.IsParent) ? item.MerchItem : item.MerchItem.ParentMerchRecord;
                        if (Ctx.Merch_Requires_18Over_Acknowledge_List.Contains(parentItem.Id))
                        {
                            logVerification = true;
                            break;
                        }
                    }

                    if (logVerification)
                    {
                        DateTime dob = DateTime.MaxValue;
                        string profileDob = DateTime.TryParse(purchaseProfile.DateOfBirth, out dob)
                            ? dob.ToString("MM/dd/yyyy") : string.Empty;

                        if (userName != null && userName.Trim().Length > 0 && userName.ToLower() != "anonymous")
                            InvoiceEvent.NewInvoiceEvent(Ctx.SessionInvoice.Id, DateTime.Now, DateTime.Now, _Enums.EventQStatus.Success,
                                userName, Ctx.SessionAuthorizeNet.UserId, userName, _Enums.EventQContext.Invoice, _Enums.EventQVerb.AgeVerify18Submission,
                                null,
                                Ctx.UserAgeComplianceDate.ToString("MM/dd/yyyy"),
                                (Ctx.UserAgeComplianceDate.ToString("MM/dd/yyyy") != profileDob) ? "Submitted with order - DOES NOT MATCH PROFILE" : "Submitted with order", true);
                    }
                }
                catch (Exception ex)
                {
                    _Error.LogException(ex);
                }



                //clear cart - true indicates that it handles inventory chores
                string retClear = Ctx.Cart.ClearCart(true);


                #region Complete Google Analytics code - overwrite default and record transaction and items

                base.Analytics_ResetCode();
                if (base.Analytics_OpenCode())
                {
                    ConfirmationTracker.Analytics_CloseAndTrackTrans(google);
                    base.Analytics_AppendCode(google);
                    base.Analytics_CloseCode();
                }
                
                #endregion


                //record the unique codes for an invoice
                ConfirmationHelper.RecordDeliveryCodes(Ctx);

                int idHold = Ctx.AuthId;
                Ctx.SessionAuthorizeNet = null;//resets the auth - forces a recall from db
                Ctx.AuthId = idHold;               
            }
            catch (System.Threading.ThreadAbortException) { }
            catch (Exception ex)
            {
                //todo - display some sort of error
                _Error.LogException(ex, true);

                Ctx.CurrentCartException = "There was an error processing your request. Please try again later.";
                base.Redirect("/Store/Checkout.aspx");
            }

            //Ctx.MonkeyWrench();

            //POST PURCHASE TEXT
            ConfirmationHelper.ApplyPostText(postText, postCmd);

            //reset session invoice here - all further invoice information requests are handled from the authnet's InvoiceRecord
            Ctx.SessionInvoice = null;
            
            //if we fail on a mailing - it will not kill the order process
            Ctx.SessionAuthorizeNet.SendConfirmationEmail(userWebInfo, Ctx.SessionAuthorizeNet.Email);

            //Ctx.MonkeyWrench();
        }

        #endregion

        //page displays according to auth

        #region Display The Order

        //*******************************************
        // NOTE* we reconstruct the session objects here
        //*******************************************

        //invNum indicates we are coming in from the customer section - so reset the auth if necessary
       if (InvNum != null && InvNum.Trim() != "0" && Utils.Validation.IsInteger(InvNum))
        {
            Ctx.SessionAuthorizeNet = null;
            _auth = null;

            Ctx.InvoiceId = int.Parse(InvNum);

            //Ctx.MonkeyWrench();

            //avoid spoofed querystring
            if (Ctx.SessionInvoice == null || Ctx.SessionInvoice.Id != Ctx.InvoiceId)
                base.Redirect("~/Default.aspx");
            
            //find the relevant auth - look thru the collection and match to transactiontype = auth_capture and authorized
            _auth = Ctx.SessionInvoice.AuthorizeNetRecords().GetList().Find(
                    delegate(AuthorizeNet match) { return (match.IsAuthorized && match.TransactionType.ToLower() == "auth_capture"); }
                    );

            //avoid spoofed querystring
            if (_auth == null ||
                (((!this.User.IsInRole("Administrator")) && (!this.User.IsInRole("OrderFiller"))) && 
                _auth.AspnetUserRecord.UserName.Trim().ToLower() != this.User.Identity.Name.Trim().ToLower()))
            {
                Ctx.SessionInvoice = null;
                _auth = null;
                base.Redirect("~/Default.aspx");
            }

            Ctx.SessionAuthorizeNet = _auth;

            Ctx.SessionInvoice = null;

            //Ctx.MonkeyWrench();
        }

        #endregion

    }

    #region Web Methods

    [WebMethod]
    public static string SendGcEml(string to, string from, string toEmail, string fromEmail, string code)
    {
        if (toEmail.Length > 0)
        {
            try
            {
                if (!Utils.Validation.IsValidEmail(toEmail))
                    throw new Exception("Email is invalid");

                InvoiceItem inv = new InvoiceItem();
                inv.LoadAndCloseReader(InvoiceItem.FetchByParameter("Criteria", Comparison.Like,
                    string.Format("{0}{1}", InvoiceItem.GiftCertificateDeliveryConstant, code)
                    ));

                if (inv == null || (!inv.IsGiftCertificateDelivery))
                    throw new Exception("Invalid code submitted");

                //place into mailqueue - keep a record of it
                MailQueue.SendGiftCertificate(toEmail, fromEmail, to, from, inv.LineItemTotal.ToString("n2"), code);

                return toEmail;
            }
            catch (Exception)
            {
                throw;
            }
        }

        return string.Empty;
    }
    #endregion
}
//1271 - 110518