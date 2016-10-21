using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Web.UI;
using System.Text;
using SubSonic;

namespace Wcss
{
    public partial class RequiredShowTicketPastPurchase
    {
        #region Table Properties

        /// <summary>
        /// This will set how many can be added to cart. If true, allowable will be the lesser of the past or allowed per order.
        /// If false, then the req is simply an unlock to being able to buy however many they would like.
        /// Limits will apply to all reqs in the collection
        /// </summary>
        [XmlAttribute("LimitQtyToPastQty")]
        public bool LimitQtyToPastQty
        {
            get { return this.BLimitQtyToPastQty; }
            set { this.BLimitQtyToPastQty = value; }
        }

        #endregion

        #region Static Methods to aid in searching past purchases

        public static int SetMaxAllowedBasedOnRequired(string userName, ShowTicket currentTicket)
        {
            int max = currentTicket.MaxQuantityPerOrder;

            //if we are allowing purchases based on past purchases - we need to check how many they are allowed to purchase
            if (userName != null && userName.Trim().Length > 0)
            {
                Utils.Helper.Quartet<bool, int, int, string> returnVal = RequiredShowTicketPastPurchase.ScanPurchaseRequirements(userName, currentTicket);
                bool passedReqs = (bool)returnVal.First;
                int pastQty = (int)returnVal.Third;

                //passedReqs could mean that there arent any reqs to pass - so check qty!
                if (passedReqs && pastQty > -1)
                {
                    //now check to see if we have limited qties to past purchase and if we have the override max allowed flag
                    int overrideable = currentTicket.RequiredShowTicketPastPurchaseRecords().GetList()
                        .FindIndex(delegate(RequiredShowTicketPastPurchase match) { return match.LimitQtyToPastQty; });

                    if (overrideable != -1)
                    {
                        int currentPurchases = (int)returnVal.Second;
                        //we set the max based on how many more tickets they can buy
                        int allowedpurchases = pastQty - currentPurchases;

                        max = allowedpurchases;
                    }
                }
            }

            return max;
        }

        /*
        public static int SetMaxAllowedBasedOnRequired(string userName, Merch parentMerch)
        {
            int max = parentMerch.MaxQuantityPerOrder;

            //if we are allowing purchases based on past purchases - we need to check how many they are allowed to purchase
            if (userName != null && userName.Trim().Length > 0)
            {
                Utils.Helper.Quartet<bool, int, int, string> returnVal = RequiredShowTicketPastPurchase.ScanPurchaseRequirements(userName, parentMerch);
                bool passedReqs = (bool)returnVal.First;
                int pastQty = (int)returnVal.Third;

                //passedReqs could mean that there arent any reqs to pass - so check qty!
                if (passedReqs && pastQty > -1)
                {
                    //now check to see if we have limited qties to past purchase and if we have the override max allowed flag
                    int overrideable = parentMerch.RequiredShowTicketPastPurchaseRecords().GetList()
                        .FindIndex(delegate(RequiredShowTicketPastPurchase match) { return match.LimitQtyToPastQty; });

                    if (overrideable != -1)
                    {
                        int currentPurchases = (int)returnVal.Second;
                        //we set the max based on how many more tickets they can buy
                        int allowedpurchases = pastQty - currentPurchases;

                        max = allowedpurchases;
                    }
                }
            }

            return max;
        }
        */

        /// <summary>
        /// Determines if a PastPurchase is required for the currentTicket. 
        /// Past purchases are searched within the date range of the requirement
        /// get a qty of items purchased
        /// get a qty of purchases that match current product purchases to limit total purchases
        /// Client must use return values to determine if requirement is met and is still valid
        /// If we return false in the first of the triplet - then we do not need to proceed any further.
        /// If true then check inventory threshholds
        /// add current qty to cart qty to get update total
        /// return -1 for values if we don't need to compare
        /// </summary>
        /// <param name="userName">Profile name</param>
        /// <param name="currentTicket">The ShowTicket that may need requirements</param>
        /// <returns>Utils.Helper.Quartet(bool, int, int, string) first bool indicates if we should check further. Second is currentTickets, third is pastQty, fourth is reasons why it did not pass</returns>
        /// 
        public static Utils.Helper.Quartet<bool, int, int, string> ScanPurchaseRequirements(string userName, ShowTicket currentTicket)
        {
            bool hasRequirements = (currentTicket.RequiredShowTicketPastPurchaseRecords().Count > 0);
            int currentTicketQty = 0; 
            int pastPurchaseQty = 0;
            
            //only do any real work if we have to
            //here we return that we have passed the requirement - there is no inventory to check - no need to pursue 
            if (!hasRequirements)
                return new Utils.Helper.Quartet<bool, int, int, string>(true, -1, -1, string.Empty);

            //TODO: pass down marketing code? for currently running unlock requirement - right now just use string.Empty
            //put together a list of running requirements
            string result = string.Empty;
            RequiredShowTicketPastPurchaseCollection coll = new RequiredShowTicketPastPurchaseCollection();
            coll.AddRange(currentTicket.RequiredShowTicketPastPurchaseRecords().GetList().FindAll(delegate(RequiredShowTicketPastPurchase match)
            { return (match.RequiredRecord.IsCurrentlyRunning(string.Empty)); }));

            //we need viable reqs to continue
            if(coll.Count == 0)
                return new Utils.Helper.Quartet<bool, int, int, string>(true, -1, -1, string.Empty);

            //if we do have reqs - we need a username to continue
            if(userName == null || userName.TrimEnd().Length == 0)
                return new Utils.Helper.Quartet<bool, int, int, string>(false, -1, -1, "No username - user must be logged in");
            
            //now we look to get info about past and current purchases
            StringBuilder sb = new StringBuilder();
            SubSonic.QueryCommand cmd = new QueryCommand(string.Empty, SubSonic.DataService.Provider.Name);
            cmd.Parameters.Add("@userName", userName, System.Data.DbType.String);
            cmd.Parameters.Add("@currentShowTicketIdx", currentTicket.Id, System.Data.DbType.Int32);

            sb.Append("DECLARE @retVal varchar(500); SET @retVal = ''; ");
            sb.Append("DECLARE @qtyCurrentPurchases int; SET @qtyCurrentPurchases = 0; ");
            sb.Append("DECLARE @qtyPast int; SET @qtyPast = 0; ");
            sb.Append("DECLARE @qtyLocalLoop int; ");


            //Determine how many they have bought of the current ticket
            //SQL: get matches on current ticket - not time sensitive
            sb.Append("SELECT @qtyCurrentPurchases = ISNULL(SUM(ii.[iQuantity]), 0) ");
            sb.Append("FROM [InvoiceItem] ii, [Invoice] i, [Aspnet_Users] u ");
            sb.Append("WHERE u.[UserName] = @userName AND u.[UserId] = i.[UserId] AND ");
            sb.Append("i.[Id] = ii.[TInvoiceId] AND ");
            sb.Append("ii.[tShowTicketId] IS NOT NULL AND ii.[TShowTicketId] = @currentShowTicketIdx AND ");        
            sb.Append("ii.[PurchaseAction] = 'Purchased' ");
            sb.Append("GROUP BY ii.[Id] ");


            //foreach requirement - construct sql
            int idx = 0;
            foreach (RequiredShowTicketPastPurchase past in coll)
            {
                //ensure we have valid ids to match
                string ids = past.RequiredRecord.VcIdx.Trim().TrimEnd(',');

                if (ids.Length == 0)
                {
                    _Error.LogException(new Exception(string.Format("ids provided in past purchase are empty. ShowTicket - {0}. reqPastPurchaseId - {1}.", 
                        currentTicket.Id.ToString(), past.Id)));
                    return new Utils.Helper.Quartet<bool, int, int, string>(false, -1, -1, "no id matches provided");
                }

                //we will try to construct an id list here - if it fails the error will be caught in the outer try/catch
                string[] idInput = ids.Split(',');

                try
                {
                    foreach (string s in idInput)
                        int.Parse(s);
                }
                catch (Exception ex)
                {
                    _Error.LogException(ex);
                    _Error.LogException(new Exception(string.Format("ids provided in past purchase are invalid. ShowTicket - {0}. reqPastPurchaseId - {1}.",
                        currentTicket.Id.ToString(), past.Id)));
                    return new Utils.Helper.Quartet<bool, int, int, string>(false, -1, -1, "invalid ids provided");
                }

                //establish the ids to match - SQL - IN (@matchIdList_)
                cmd.Parameters.Add(string.Format("@dateStart_{0}", idx.ToString()), past.RequiredRecord.DateStart, System.Data.DbType.DateTime);
                cmd.Parameters.Add(string.Format("@dateEnd_{0}", idx.ToString()), past.RequiredRecord.DateEnd, System.Data.DbType.DateTime);

                sb.Append("SET @qtyLocalLoop = 0; ");

                sb.Append("SELECT @qtyLocalLoop = ISNULL(SUM(ii.[iQuantity]), 0) ");
                sb.Append("FROM [InvoiceItem] ii, [Invoice] i, [Aspnet_Users] u  ");
                sb.Append("WHERE u.[UserName] = @userName AND u.[UserId] = i.[UserId] AND ");
                sb.AppendFormat("i.[dtInvoiceDate] BETWEEN @dateStart_{0} AND @dateEnd_{0} AND i.[Id] = ii.[TInvoiceId] AND ", idx.ToString());
                //pkg tickets that are just linked have a 0 lineitemtotal
                sb.Append("ii.[mLineItemTotal] > 0 AND ");


                //TODO: allow for merchandise to be a past requirement
                if (past.RequiredRecord.RequiredContext == _Enums.RequirementContext.merch)
                    sb.AppendFormat("ii.[tMerchId] IS NOT NULL AND ii.[tMerchId] = m.[Id] AND m.[tParentListing] IS NOT NULL AND m.[tParentListing] IN ( {0} ) AND ", ids);
                else if (past.RequiredRecord.RequiredContext == _Enums.RequirementContext.ticket)
                    sb.AppendFormat("ii.[tShowTicketId] IS NOT NULL AND ii.[TShowTicketId] IN ( {0} ) AND ", ids);



                //sb.AppendFormat("ii.[tShowTicketId] IS NOT NULL AND ii.[TShowTicketId] IN ( {0} ) AND ", ids);
                //DO bother with date - we want to see how many they have purchased in the past period!!!!
                sb.Append("ii.[PurchaseAction] = 'Purchased' ");
                //sb.Append("GROUP BY ii.[Id] ");dont group

                sb.Append("IF (@qtyLocalLoop > 0) BEGIN ");
                sb.Append("SET @qtyPast = @qtyPast + @qtyLocalLoop END ");
                sb.AppendFormat("ELSE BEGIN SELECT @retVal = @retVal + 'Match not met for {0} ~' END ", ids);

                idx++;
            }

            //if retval is empty - at least one condition was not met
            sb.Append("IF (LEN(LTRIM(RTRIM(@retVal))) = 0) BEGIN SET @retVal = 'SUCCESS' END ");
            sb.Append("SELECT @retVal, @qtyCurrentPurchases, @qtyPast ");

                
            //do proc to find the nums we are looking for
            //do with dataset to get error messages
            cmd.CommandSql = sb.ToString();

            bool passed = false;

            try
            {
                System.Data.DataSet ds = SubSonic.DataService.GetDataSet(cmd);
                System.Data.DataTable dt = ds.Tables[0];
                result = dt.Rows[0].ItemArray[0].ToString();
                currentTicketQty = int.Parse(dt.Rows[0].ItemArray[1].ToString());
                pastPurchaseQty = int.Parse(dt.Rows[0].ItemArray[2].ToString());
            
                //return SUCCESS - or list of reasons why it does not match
                if (result == "SUCCESS")
                {
                    result = string.Empty;
                    passed = true;
                }
                else
                {
                    //result will have a delimiter
                    result = result.TrimEnd('~');
                }
            }
            catch (Exception ex)
            {
                _Error.LogException(ex);
                return new Utils.Helper.Quartet<bool, int, int, string>(false, -1, -1, "Error occurred");
            }

            return new Utils.Helper.Quartet<bool, int, int, string>(passed, currentTicketQty, pastPurchaseQty, result);
        }

        #endregion
    }
}


///

///// <summary>
///// If true, then the conditions must be met to display the ticket
///// </summary>
//[XmlAttribute("RequiredForVisibility")]
//public bool RequiredForVisibility
//{
//    get { return this.BRequiredForVisibility; }
//    set { this.BRequiredForVisibility = value; }
//}

///// <summary>
///// Indicates if we should show the ticket or not
///// </summary>
///// <param name="currentTicket"></param>
///// <param name="userName"></param>
///// <returns></returns>
//public static bool AllowTicketDisplay(ShowTicket currentTicket, string userName)
//{
//    bool hasReqs = currentTicket.RequirementPastPurchaseRecords().Count > 0;

//    //if there aren't any reqs - then this should not affect how the ticket is displayed
//    if (!hasReqs)
//        return true;

//    RequiredShowTicketPastPurchaseCollection coll = new RequiredShowTicketPastPurchaseCollection();
//    coll.AddRange(currentTicket.RequiredShowTicketPastPurchaseRecords().GetList().FindAll(delegate(RequiredShowTicketPastPurchase match)
//    { return (match.RequiredRecord.IsCurrentlyRunning(string.Empty)); }));

//    //we need viable reqs to continue - same as above - reqs dont affect
//    if(coll.Count == 0)
//        return true;

//    //we have now established that we have running reqs

//    //if we do have reqs - we need a username to continue - must be logged in to see option
//    if (userName == null || userName.TrimEnd().Length == 0)
//        return false;


//    //if any of the reqs has the RequiredForVisibility flag set





//    //if there are no requirements - then yes go ahead and display
//    if (!allowDisplay)
//    {
//        //if there are requirements - we must be logged in
//        if (userName == null || userName.TrimEnd().Length == 0)
//            return display;

//        //see if the flag for displaying the ticket is on

//        string result = string.Empty;
//        RequirementPastPurchaseCollection coll = new RequirementPastPurchaseCollection();
//        coll.AddRange(currentTicket.RequirementPastPurchaseRecords());

//        if (coll.GetList().FindIndex(delegate(RequirementPastPurchase match) { return match.RequiredForVisibility; } ) != -1)
//        {
//            //if the user is logged in - just check to make sure they have past purchases to enable display
//            StringBuilder sb = new StringBuilder();
//            SubSonic.QueryCommand cmd = new QueryCommand(string.Empty, SubSonic.DataService.Provider.Name);
//            cmd.Parameters.Add("@userName", userName, System.Data.DbType.String);

//            int idx = 0;
//            foreach (RequirementPastPurchase past in coll)
//            {
//                cmd.Parameters.Add(string.Format("@matchIdx_{0}", idx.ToString()), past.IdxListing, System.Data.DbType.String);

//                sb.Append("IF NOT EXISTS (SELECT * FROM [InvoiceItem] ii, [Invoice] i, [Aspnet_Users] u ");
//                sb.Append("WHERE u.[UserName] = @userName AND u.[UserId] = i.[UserId] AND ");
//                sb.Append("i.[Id] = ii.[TInvoiceId] AND ii.[TShowTicketId] IS NOT NULL AND ");
//                sb.AppendFormat("ii.[TShowTicketId] IN (@matchIdx_{0}) AND ii.[PurchaseAction] = 'Purchased') BEGIN ", idx.ToString());
//                sb.Append("SELECT 'NOMATCH'; RETURN END ");

//                idx++;
//            }

//            sb.Append("SELECT 'SUCCESS'; RETURN ");
//            cmd.CommandSql = sb.ToString();

//            try
//            {
//                System.Data.DataSet ds = SubSonic.DataService.GetDataSet(cmd);
//                System.Data.DataTable dt = ds.Tables[0];
//                result = dt.Rows[0].ItemArray[0].ToString();

//                //return SUCCESS - or list of reasons why it does not match
//                display = (result == "SUCCESS");
//            }
//            catch (Exception ex)
//            {
//                _Error.LogException(ex);
//                display = false;
//            }
//        }
//    }

//    return display;
//}