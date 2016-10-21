using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

using Wcss;
using SubSonic;
using WillCallWeb.StoreObjects;

namespace WillCallWeb
{
    public partial class ConfirmationTracker
    {
        //builds the string for analytic ecommerce tracking - adds invoice info
        public static void Analytics_InitTrans(StringBuilder google, string uniqueId, WebContext ctx)
        {
            /*
             * _gaq.push(['_addTrans',
                '1234',           // order ID - required
                'Acme Clothing',  // affiliation or store name
                '11.99',          // total - required
                '1.29',           // tax
                '5',              // shipping
                'San Jose',       // city
                'California',     // state or province
                'USA'             // country
              ]);
             */

            google.AppendLine();
            google.AppendLine();

            ConfirmationTracker.AppendTransVar(google, "_gaq.push(['_addTrans',", false, false, 1, false);
            ConfirmationTracker.AppendTransVar(google, uniqueId, true, true, 0, false);
            ConfirmationTracker.AppendTransVar(google, Utils.ParseHelper.HtmlEncode_Extended(_Config._Site_Entity_Name), true, true, 0, true);
            ConfirmationTracker.AppendTransVar(google, ctx.Cart.Total_NonDiscounted.ToString("n2"), true, true, 2, false);
            ConfirmationTracker.AppendTransVar(google, (ctx.SessionAuthorizeNet.TaxAmount > 0) ? ctx.SessionAuthorizeNet.TaxAmount.ToString("n2") : "0.00", true, true, 0, false);
            ConfirmationTracker.AppendTransVar(google, ctx.Cart.ShippingAndHandling.ToString("n2"), true, true, 0, true);
            ConfirmationTracker.AppendTransVar(google, Utils.ParseHelper.HtmlEncode_Extended(ctx.SessionAuthorizeNet.City), true, true, 2, false);
            ConfirmationTracker.AppendTransVar(google, Utils.ParseHelper.HtmlEncode_Extended(ctx.SessionAuthorizeNet.State), true, true, 0, false);
            ConfirmationTracker.AppendTransVar(google, Utils.ParseHelper.HtmlEncode_Extended(ctx.SessionAuthorizeNet.Country), true, false, 0, true);
            ConfirmationTracker.AppendTransVar(google, "]);", false, false, 1);
        }
        public static void Analytics_AddTransItem(StringBuilder google, string uniqueId, string contextAndId, string description, string invoiceItemContext, decimal price, int qty)
        {
            /*
             * _gaq.push(['_addItem',
                '1234',           // order ID - required
                'DD44',           // SKU/code - required
                'T-Shirt',        // product name
                'Green Medium',   // category or variation
                '11.99',          // unit price - required
                '1'               // quantity - required
              ]);
            */
            google.AppendLine();

            ConfirmationTracker.AppendTransVar(google, "_gaq.push(['_addItem',", false, false, 1, false);
            ConfirmationTracker.AppendTransVar(google, uniqueId, true, true, 0, true);
            ConfirmationTracker.AppendTransVar(google, contextAndId, true, true, 2, false);
            ConfirmationTracker.AppendTransVar(google, Utils.ParseHelper.HtmlEncode_Extended(description), true, true, 0, true);
            ConfirmationTracker.AppendTransVar(google, invoiceItemContext, true, true, 2, false);
            ConfirmationTracker.AppendTransVar(google, price.ToString("n2"), true, true, 0, false);
            ConfirmationTracker.AppendTransVar(google, qty.ToString(), true, true, 0, true);
            ConfirmationTracker.AppendTransVar(google, "]);", false, false, 1, true);
        }
        public static void Analytics_CloseAndTrackTrans(StringBuilder google)
        {
            //submits transaction to the Analytics servers
            google.AppendLine();
            ConfirmationTracker.AppendTransVar(google, "_gaq.push(['_trackTrans']); ", false, false, 1, true);
            google.AppendLine();
        }
        private static void AppendTransVar(StringBuilder google, string var, bool varAsValue, bool useComma, int tabStop)
        {
            AppendTransVar(google, var, varAsValue, useComma, tabStop, true);
        }
        private static void AppendTransVar(StringBuilder google, string var, bool varAsValue, bool useComma, int tabStop, bool addNewLine)
        {
            if (tabStop > 0)
                google.Append(Utils.Constants.Tabs(tabStop));

            google.AppendFormat("{0}{1}{0}{2} ", (varAsValue) ? "'" : string.Empty, var, (useComma) ? "," : string.Empty);//orderid
            
            if(addNewLine)
                google.AppendLine();
        }
    }

    /// <summary>
    /// Summary description for CheckoutHelper
    /// </summary>
    public partial class ConfirmationChecker
    {
        public static void CheckStoreCreditLimit(WebContext ctx, ProfileCommon profile)
        {
            //analyze the user's gift redemption - if they dont have the store credit they say they do - return an error
            if (ctx.Cart.StoreCreditToApply > 0)
            {
                //if there is store credit pending for this dude then screw him
                //there should be nothing in pending for a user, within the time frame for a valid purchase
                if (profile.StoreCredit < (float)ctx.Cart.StoreCreditToApply)
                {
                    ctx.Cart.StoreCredit.Price = 0;
                    throw new Exception(string.Format("You do not have enough store credit for this order ({0}).", profile.UserName));
                }

                //check to make sure that we dont have another operation pending
                //in this case we dont want to allow overlap of redemption

                //pending store credit should be removed in two places
                // 1) if we fail the auth (back on checkout - where it is redirected)
                // 2) after credit is recorded into db/after profile is updated with new storecredit total
                if (PendingOperation.PendingExists_StoreCredit(profile.UserName, ctx.SessionInvoice.Id))
                {
                    ctx.Cart.StoreCredit.Price = 0;
                    throw new Exception(string.Format("Your store credit has changed ({0}).", profile.UserName));
                }
            }
        }

        //if we had a store credit applied....
        public static void CleanupStoreCreditOperations(WebContext ctx, ProfileCommon purchaseProfile)
        {
            if (ctx.Cart.StoreCreditToApply > 0)
            {
                decimal creditsToIssue = ctx.Cart.StoreCreditToApply;

                WillCallWeb.StoreObjects.SaleItem_StoreCredit.Profile_StoreCredit_Sync(purchaseProfile.UserName, purchaseProfile.UserName, true);

                //delete any pending rows
                PendingOperation.DeleteOperation(ctx.SessionInvoice.Id);
            }
        }

        /// <summary>
        /// ensure user has not overused coupon codes - this is checked when adding - but check for spoof value here
        /// if too many uses - return with error
        /// </summary>
        public static bool CheckCouponCodeValidity(WebContext ctx, string userName)
        {
            if (ctx.Cart.PromotionItems.Count > 0)
            {
                //if the cart had street teamer program promotions...
                System.Collections.Generic.List<SaleItem_Promotion> proms = new System.Collections.Generic.List<SaleItem_Promotion>();
                proms.AddRange(ctx.Cart.PromotionItems.FindAll(delegate(SaleItem_Promotion match) { return match.SalePromotion.Requires_PromotionCode; }));

                System.Collections.Generic.List<string> msgs = new System.Collections.Generic.List<string>();

                foreach (SaleItem_Promotion promo in proms)
                {
                    //get the matching coupon code
                    string matchingCode = promo.SalePromotion.CouponMatch(ctx.SalePromotion_CouponCodes, _Config._Coupon_IgnoreCase);

                    if (matchingCode != null && matchingCode.Length > 0)
                    {
                        bool OK = UserCouponRedemption.IsAllowedRedemption(userName, matchingCode, promo.SalePromotion.MaxUsesPerUser);
                        if (!OK)
                        {
                            //remove the coupon and give a reason for decline
                            ctx.SalePromotion_CouponCodes.Remove(matchingCode);
                            msgs.Add(matchingCode);
                        }
                    }
                }

                if (msgs.Count > 0)
                {
                    //log the failure as a user event
                    string[] codes = msgs.ToArray();

                    UserEvent.NewUserEvent(userName, DateTime.Now, DateTime.Now, _Enums.EventQStatus.Failed, userName,
                        _Enums.EventQContext.User, _Enums.EventQVerb.InvalidCoupon, null, "user over coupon quota", string.Join("~", codes), true);

                    foreach (string s in msgs)
                        ctx.SessionAuthorizeNet.ResponseReasonText += string.Format("<li>The coupon ({0}) has been exceeded its maximum usage.</li>");

                    return false;                    
                }
            }

            return true;
        }

        public static void CheckNotificationTracking(WebContext ctx)
        {
            try
            {
                //get list of active subscriptions where track_ is start of name
                SubscriptionCollection subColl = new SubscriptionCollection();
                subColl.AddRange(_Lookits.Subscriptions.GetList()
                    .FindAll(delegate(Subscription match) { return (match.IsActive && (match.Name.IndexOf("track_", StringComparison.OrdinalIgnoreCase) == 0)); }));

                if (subColl.Count > 0)
                {
                    foreach (Subscription sub in subColl)
                    {
                        //if we have active subscribers....
                        if (sub.SubscriptionUserRecords().Count > 0)
                        {
                            //parse out ticket|merch and id
                            _Enums.InvoiceItemContext context = _Enums.InvoiceItemContext.notassigned;
                            int idxs = 0;
                            if (sub.Name.IndexOf("track_merch_", StringComparison.OrdinalIgnoreCase) == 0)
                            {
                                context = _Enums.InvoiceItemContext.merch;
                                idxs = int.Parse(sub.Name.Substring(((string)"track_merch_").Length));
                            }
                            else if (sub.Name.IndexOf("track_ticket_", StringComparison.OrdinalIgnoreCase) == 0)
                            {
                                context = _Enums.InvoiceItemContext.ticket;
                                idxs = int.Parse(sub.Name.Substring(((string)"track_ticket_").Length));
                            }

                            if (idxs > 0)
                            {
                                if (context == _Enums.InvoiceItemContext.merch)
                                {
                                    foreach (SaleItem_Merchandise itm in ctx.Cart.MerchandiseItems)
                                    {
                                        if (itm.tMerchId == idxs || itm.MerchItem.TParentListing == idxs)
                                        {
                                            int avail = itm.MerchItem.Available - itm.Quantity;

                                            //send notifications
                                            foreach (SubscriptionUser subUsr in sub.SubscriptionUserRecords())
                                            {
                                                if (subUsr.IsSubscribed)
                                                {
                                                    MailQueue.SendEmail(_Config._CustomerService_Email, _Config._CustomerService_FromName,
                                                        subUsr.EmailAddress, null, null,
                                                        string.Format("{0} Notification: {1}", _Config._Site_Entity_Name, sub.Name), //subject
                                                        null,
                                                        string.Format("{0}(s) {1} were sold at {2}. Remaining inventory: {3}.",
                                                            itm.Quantity.ToString(), itm.MerchItem.DisplayNameWithAttribs, DateTime.Now.ToString("MM/dd/yyyy hh:mmtt"),
                                                            avail.ToString()),
                                                        null, false, null);
                                                }
                                            }
                                        }
                                    }
                                }
                                else if (context == _Enums.InvoiceItemContext.ticket)
                                {
                                    foreach (SaleItem_Ticket itm in ctx.Cart.TicketItems)
                                    {
                                        if (itm.tShowTicketId == idxs)
                                        {
                                            int avail = itm.Ticket.Available - itm.Quantity;
                                            //send notifications
                                            foreach (SubscriptionUser subUsr in sub.SubscriptionUserRecords())
                                            {
                                                if (subUsr.IsSubscribed)
                                                {
                                                    MailQueue.SendEmail(_Config._CustomerService_Email, _Config._CustomerService_FromName,
                                                        subUsr.EmailAddress, null, null,
                                                        string.Format("{0} Notification: {1}", _Config._Site_Entity_Name, sub.Name), //subject
                                                        null,
                                                        string.Format("{0}(s) {1} were sold at {2}. Remaining inventory: {3}.",
                                                            itm.Quantity.ToString(), 
                                                            //tags are not necessary for this notification
                                                            Utils.ParseHelper.StripHtmlTags(itm.Ticket.DisplayNameWithAttribsAndDescription), 
                                                            DateTime.Now.ToString("MM/dd/yyyy hh:mmtt"),
                                                            avail.ToString()),
                                                        null, false, null);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _Error.LogException(ex);
            }
        }

	}
}