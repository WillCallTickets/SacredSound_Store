using System;
using System.Linq;

using Wcss;

namespace WillCallWeb.StoreObjects
{
    /// <summary>
    /// Summary description for GiftCertificate
    /// </summary>
    [Serializable]
    public class SaleItem_StoreCredit
    {
        private decimal _price = 0;
        public decimal Price
        {
            get
            {
                return decimal.Round(_price, 2);
            }
            set
            {
                _price = decimal.Round(value, 2);
            }
        }

        /// <summary>
        /// keep user name as a safety check
        /// if we were to do this from the admin - what would happen?
        /// </summary>
        /// <param name="code"></param>
        /// <param name="userName"></param>
        public static decimal RedeemGiftCertificate(string userName, string code)
        {
            decimal amount = 0;

            //don't do this unless the user is authenticated (logged in)
            if (System.Web.HttpContext.Current != null)
            {
                ProfileCommon profile = System.Web.HttpContext.Current.Profile as ProfileCommon;
                ProfileCommon p = profile.GetProfile(userName);
                
                if (p != null && (!p.IsAnonymous) && p.UserName.ToLower() == userName)
                {
                    //AspnetUser usr = AspnetUser.GetUserByUserName(userName);

                    ////pending store credit should be removed in two places
                    //// 1) if we fail the auth (back on checkout - where it is redirected)
                    //// 2) after credit is recorded into db/after profile is updated with new storecredit total
                    //if(PendingOperation.PendingExists_StoreCredit(profile.UserName, (usr != null) ? usr.CustomerId : 100))
                    //{
                    //    throw new Exception(string.Format("Your store credit has changed ({0}).", profile.UserName));
                    //}

                    amount = ValidateGiftCertificateRedemption(code);

                    RecordRedemption(profile, amount, "Credit redeemed", code, null);
                }
                else
                    throw new Exception("User must be logged in to redeem a gift certificate or store credit.");
            }

            return amount;
        }

        public static void ApplyCreditToInvoice(string userName, Invoice invoice, decimal amountToApply)
        {
            //don't do this unless the user is authenticated (logged in)
            if (System.Web.HttpContext.Current != null)
            {
                ProfileCommon profile = System.Web.HttpContext.Current.Profile as ProfileCommon;
                if (profile != null && (!profile.IsAnonymous) && profile.UserName.ToLower() == userName)
                {
                    //and tie the trans id to the credit record
                    RecordRedemption(profile, amountToApply, "Applied to invoice", null, null);
                }
            }
        }


        /// <summary>
        /// This will create a redemption row if the redemption key exists (guid from invoiceitem) and has not been used
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private static decimal ValidateGiftCertificateRedemption(string code)
        {
            SubSonic.StoredProcedure sp = SPs.TxStoreCreditValidateGiftCertificateRedemption(_Config.APPLICATION_ID, new Guid(code));

            object o = sp.ExecuteScalar();

            if (!Utils.Validation.IsDecimal(o.ToString()))
                throw new Exception(o.ToString());

            decimal amount = (decimal)o;

            if (amount == 0)
                throw new Exception("Code is not valid. Please check your entry.");

            return amount;
        }


        /// <summary>
        /// this will create a row in the store credit table and will also adjust the profile parameter
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="amount"></param>
        /// <param name="comment"></param>
        /// <param name="redemptionId"></param>
        /// <param name="transactionId"></param>
        /// <returns></returns>
        private static StoreCredit RecordRedemption(ProfileCommon profile, decimal amount, string comment, string redemptionId, int? transactionId)
        {
            StoreCredit credit = new StoreCredit();
            
            credit.DateStamp = DateTime.Now;
            credit.ApplicationId = _Config.APPLICATION_ID;
            credit.Amount = amount;
            if (transactionId.HasValue && transactionId > 0)
                credit.TInvoiceTransactionId = transactionId;
            credit.Comment = comment;
            System.Web.Security.MembershipUser member = System.Web.Security.Membership.GetUser(profile.UserName);
            credit.UserId = (Guid)member.ProviderUserKey;

            if (redemptionId != null)
            {
                Guid redGuid = new Guid(redemptionId);
                credit.RedemptionId = redGuid;

                InvoiceItem ii = new InvoiceItem();
                ii.LoadAndCloseReader(InvoiceItem.FetchByParameter("Guid", redGuid));
                if (ii != null && ii.Id > 0)
                {
                    ii.DateShipped = DateTime.Now;
                    ii.Description = string.Format("Redeemed {0} by {1}", DateTime.Now.ToString("MM/dd/yyyy hh:mmtt"), profile.UserName);
                    ii.Save();
                }
            }

            credit.Save();

            //now update the profile
            profile.StoreCredit = profile.StoreCredit + (float)amount;

            //TODO?????
            //profile.Save();

            return credit;
        }

        /// <summary>
        /// Adds or subtracts an amount to the storecredit in user's PROFILE
        /// </summary>
        /// <param name="creatorProfile">A profile object used to retrieve a profile</param>
        /// <param name="userName">The username of the account to add/subtract the credit</param>
        /// <param name="amount"></param>
        public static void Profile_StoreCredit_Adjust(ProfileCommon creatorProfile, string userName, decimal amount)
        {
            ProfileCommon p = creatorProfile.GetProfile(userName);

            //add a row to store credit for the users account
            RecordRedemption(p, amount, string.Format("{0} account by admin", (amount > 0) ? "added to" : "subtracted from"), null, null);

            //log event
            System.Web.Security.MembershipUser mem = System.Web.Security.Membership.GetUser(userName);//use to get user id
            UserEvent.RecordStoreCreditEvent(creatorProfile.UserName, (Guid)mem.ProviderUserKey, userName, amount, decimal.Parse(p.StoreCredit.ToString()),
                (amount > 0) ? "added" : "removed", null);
            
            p.Save();
        }

        /// <summary>
        /// Gets the real time profile store credit value. It sums the credits and debits yielding a balance. Updates the users' profile
        /// </summary>
        /// <param name="profile"></param>
        /// <returns></returns>
        public static decimal Profile_StoreCredit_Sync(string userName, string creatorName, bool recordEvent)
        {
            System.Web.Security.MembershipUser member = System.Web.Security.Membership.GetUser(userName);

            decimal result = new
                SubSonic.Select(SubSonic.Aggregate.Sum("mAmount", "Amount"))
                .From(StoreCredit.Schema)
                .Where("ApplicationId").IsEqualTo(_Config.APPLICATION_ID)
                .And("UserId").IsEqualTo(member.ProviderUserKey.ToString())
                .ExecuteScalar<decimal>();

            result = Math.Round(result, 2);
            
            //adjust profile if necessary
            ProfileCommon userProfile = new ProfileCommon();
            userProfile = userProfile.GetProfile(userName);

            if(userProfile.StoreCredit != (float)result)
            {
                userProfile.StoreCredit = (float)result;
                userProfile.Save();
            }

            if(recordEvent)
                UserEvent.RecordStoreCreditEvent(creatorName, (Guid)member.ProviderUserKey, userName, 
                    result, decimal.Parse(userProfile.StoreCredit.ToString()),"synced", null);

            return result;
        }
    }
}
