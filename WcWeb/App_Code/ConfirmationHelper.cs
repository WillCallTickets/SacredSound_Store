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
    /// <summary>
    /// Summary description for CheckoutHelper
    /// </summary>
    public partial class ConfirmationHelper
    {
        public ConfirmationHelper() { }

        public static string AddParam(QueryCommand cmd, string paramName, object paramValue, DbType dbtype)
        {
            cmd.AddParameter(paramName, paramValue, dbtype);
            return paramName;
        }

        public static void CreateInvoiceItemMerch(QueryCommand cmd, StringBuilder sb, StringBuilder products, SaleItem_Merchandise item, int quantity, ref int loopCounter)
        {
            string currentCounter = loopCounter.ToString();

            //if it is a gift certificate purchase - we need to create a 
            string idx = AddParam(cmd, string.Format("@idx_{0}", currentCounter), item.tMerchId, DbType.Int32);
            string name = AddParam(cmd, string.Format("@name_{0}", currentCounter), item.MerchItem.DisplayNameWithAttribs, DbType.String);
            string price = AddParam(cmd, string.Format("@price_{0}", currentCounter), item.Price, DbType.Decimal);
            //note that we use the given quantity so that we can override in cases of mult items that need extra entries
            string qty = AddParam(cmd, string.Format("@qty_{0}", currentCounter), quantity, DbType.Int32);

            //insert sql - check for items to split
            //allow multiple entries for the same product
            //in this case we want to get different codes for each individual item purchased
            if ((item.MerchItem.IsDownloadDelivery || item.MerchItem.IsActivationCodeDelivery || item.MerchItem.IsGiftCertificateDelivery) && item.Quantity > 1)
                for (int i = 0; i < item.Quantity; i++)
                    sb.Append(Checkout_Services.InsertInvoiceItem_Merch_Sql(name, price, "@singleQty", idx, null, null, null));
            else
                sb.Append(Checkout_Services.InsertInvoiceItem_Merch_Sql(name, price, qty, idx, null, null, null));

            ConfirmationHelper.AddBundledItems(item, ref loopCounter, cmd, sb, products);

            loopCounter++;
        }

        /// <summary>
        /// The base ticket will indicate false for isPackageTicket. isPackageTicket indicates that is part of the package for listing purposes. All 
        /// pertinent info - price and charges - is recorded on the base ticket
        /// </summary>
        public static void CreateInvoiceItemTicket(QueryCommand cmd, StringBuilder sb, StringBuilder products, ShowTicket st, SaleItem_Ticket sit,
            bool isPackageTicket, ref int loopCounter)
        {
            string counter = loopCounter.ToString();

            //act, price, qty, idx, shoId, age, svc, pkp, shoDate, crit, desc));
            string mainactname = (st.IsCampingPass()) ? string.Format("CAMPING - {0}", st.ShowDateRecord.ShowRecord.ShowEventPart.ToUpper()) : 
                st.ShowDateRecord.ShowRecord.ShowNamePart.ToUpper();

            sb.Append(Checkout_Services.InsertInvoiceItem_Ticket_Sql(
                AddParam(cmd, string.Format("@act_{0}", counter), mainactname, DbType.String),
                AddParam(cmd, string.Format("@price_{0}", counter), (isPackageTicket) ? 0 : sit.Price, DbType.Decimal),
                AddParam(cmd, string.Format("@qty_{0}", counter), sit.Quantity, DbType.Int32),
                AddParam(cmd, string.Format("@idx_{0}", counter), st.Id, DbType.Int32),
                AddParam(cmd, string.Format("@shoId_{0}", counter), st.TShowId, DbType.Int32),
                AddParam(cmd, string.Format("@age_{0}", counter), st.AgeRecord.Name, DbType.String),
                AddParam(cmd, string.Format("@svc_{0}", counter), (isPackageTicket) ? 0 : sit.ServiceFee, DbType.Decimal),
                AddParam(cmd, string.Format("@pkp_{0}", counter), (sit.PickupName.Trim().Length > 0) ? sit.PickupName.Trim().ToUpper() : null, DbType.String),
                AddParam(cmd, string.Format("@shoDate_{0}", counter), st.DateOfShow, DbType.DateTime),
                AddParam(cmd, string.Format("@crit_{0}", counter), st.CriteriaText, DbType.String),
                AddParam(cmd, string.Format("@desc_{0}", counter), st.SalesDescription, DbType.String)));

            //only create bundeld items for the base of the package
            if (!isPackageTicket)
                ConfirmationHelper.AddBundledItems(sit, ref loopCounter, cmd, sb, products);

            loopCounter++;
        }

        /// <summary>
        /// This will enter in the bundle and any associated items
        /// Note that a procedure will need to be run later to tie together the invoiceitemParents>bundles>bundleitems as we do not yet have invoiceitem ids
        /// </summary>
        private static void AddBundledItems(SaleItem_Base saleItemBase, ref int loopCounter, SubSonic.QueryCommand cmd, StringBuilder sb, StringBuilder products)
        {
            bool isMerch = (saleItemBase is SaleItem_Merchandise);
            bool isTicket = (saleItemBase is SaleItem_Ticket);

            //determine if we should continue the processing of bundles
            if ((!isMerch && !isTicket) || saleItemBase.MerchBundleSelections.Count == 0)
                return;

            int parentIdx = 0;

            //get a list of merchbundles - we need to do this to configure pricing
            System.Collections.Generic.List<MerchBundle> bundles = new System.Collections.Generic.List<MerchBundle>();
            if (isMerch)
            {
                parentIdx = ((SaleItem_Merchandise)saleItemBase).tMerchId;
                bundles.AddRange(((SaleItem_Merchandise)saleItemBase).MerchItem.ParentMerchRecord.MerchBundleRecords());
            }
            else if (isTicket)
            {
                parentIdx = ((SaleItem_Ticket)saleItemBase).tShowTicketId;
                bundles.AddRange(((SaleItem_Ticket)saleItemBase).Ticket.MerchBundleRecords());
            }

            bundles.RemoveAll(delegate(MerchBundle match) { return ((!match.IsActive) || (match.ActiveInventory.Count == 0)); });

            if (bundles.Count > 1)
                bundles.Sort(delegate(MerchBundle x, MerchBundle y) { return (x.DisplayOrder.CompareTo(y.DisplayOrder)); });

            //set up list one time only!
            System.Collections.Generic.List<MerchBundle_Listing> selections = new System.Collections.Generic.List<MerchBundle_Listing>();

            //init loop counter
            loopCounter++;

            foreach (MerchBundle bundle in bundles)
            {
                string currentCounter = loopCounter.ToString();

                //clear for new loop
                selections.Clear();
                //do not add any opt outs
                selections.AddRange(saleItemBase.MerchBundleSelections.FindAll(delegate(MerchBundle_Listing match) { return (match.BundleId == bundle.Id && (!match.IsOptOut)); }));
                int selectionCount = selections.Count;

                string bnTitle = AddParam(cmd, string.Format("@bntitle_{0}", currentCounter), bundle.Title, DbType.String);

                //this will be used to match the items to this bundle
                //also serves as a place to store the corresponding merchBundleId for the bundle itself and its selections
                //bundleId is stored in the criteria
                string bundleCriteria = string.Format("{0}{1}", InvoiceItem.MerchBundleIdConstant, bundle.Id.ToString());
                string bncrit = AddParam(cmd, string.Format("@bncrtitc_{0}", currentCounter), bundleCriteria, DbType.String);

                //dont bother recording if we do not have valid selections
                if (selectionCount == 0)
                {
                    sb.Append(Checkout_Services.InsertInvoiceItem_MerchBundleEntry_Sql(false, bnTitle, "@zero", "@singleQty", bncrit, null));
                    loopCounter++;
                }
                else if (selectionCount > 0)
                {
                    int chargeInstances = 0;
                    decimal bundlePrice = saleItemBase.GetIndividualBundlePrice(bundle, out chargeInstances);

                    products.Append(ShoppingCart.FormatItemProductListing(_Enums.InvoiceItemContext.bundle, bundle.Id, chargeInstances));

                    //note that bundles that are priced per selection will not record a price here
                    sb.Append(Checkout_Services.InsertInvoiceItem_MerchBundleEntry_Sql(true, bnTitle, 
                        AddParam(cmd, string.Format("@bnprice_{0}", currentCounter), bundle.Price, DbType.Decimal),
                        AddParam(cmd, string.Format("@bnqty_{0}", currentCounter), chargeInstances, DbType.Int32),
                        bncrit,
                        AddParam(cmd, string.Format("@bndscrp_{0}", currentCounter), (bundle.Comment != null) ? bundle.Comment : string.Empty, DbType.String)));

                    //keep this here in case you may ever want to implement a zero qty for merch bundles - for above
                    //AddParam(cmd, string.Format("@bnprice_{0}", currentCounter), (bundle.PricedPerSelection) ? 0 : bundle.Price, DbType.Decimal),
                    //AddParam(cmd, string.Format("@bnqty_{0}", currentCounter), (bundle.PricedPerSelection) ? 0 : chargeInstances, DbType.Int32),


                    //update bundle item to point to its parent and its merch bundle
                    //get scope id
                    string insertIdx = string.Format("@bnidx_{0}", currentCounter);
                    sb.AppendFormat("DECLARE {0} int; ", insertIdx);
                    sb.AppendFormat("SELECT {0} = SCOPE_IDENTITY(); ", insertIdx);

                    //get max id of parent - this will be the one we just entered
                    string parentItemIdx = string.Format("@bnprt_{0}", currentCounter);
                    sb.AppendFormat("DECLARE {0} int; ", parentItemIdx);

                    //tickets match tickets - merch matches merch
                    sb.AppendFormat("SELECT {0} = MAX(ii.[Id]) FROM [InvoiceItem] ii WHERE ii.[tInvoiceId] = @invoiceId AND ii.[{1}] = {2}; ",
                        parentItemIdx,
                        (isMerch) ? "tMerchId" : "tShowTicketId",
                        parentIdx.ToString());

                    //criteria already holds the merchBundleId and we are adding the parent item id
                    sb.AppendFormat("UPDATE [InvoiceItem] SET [Criteria] = [Criteria] + '&{0}' + CAST({1} as varchar) WHERE [Id] = {2}; ",
                        InvoiceItem.ParentItemIdConstant, parentItemIdx, insertIdx);

                    loopCounter++;//increment counter before adding selections



                    //add selections as merch items - do not specify prices - match them to the inserted bundle item
                    //collection does not contain opt outs                
                    foreach (MerchBundle_Listing listing in selections)
                    {
                        string nxtCounter = loopCounter.ToString();

                        products.Append(ShoppingCart.FormatItemProductListing(_Enums.InvoiceItemContext.merch, listing.SelectedInventoryId, listing.Quantity));

                        string biname = AddParam(cmd, string.Format("@biname_{0}", nxtCounter), listing.SelectedInventory.DisplayNameWithAttribs, DbType.String);
                        string biidx = AddParam(cmd, string.Format("@biidx_{0}", nxtCounter), listing.SelectedInventoryId, DbType.Int32);
                        //Criteria -> bundleCriteria already holds the merchbundleid - now we are adding the parent id of the bundle that this selction belongs to
                        string bicriteria = string.Format("'{0}&{1}' + CAST({2} as varchar)", bundleCriteria, InvoiceItem.ParentItemIdConstant, insertIdx);

                        //string biprice = null;
                        //if(bundle.PricedPerSelection)
                        //    biprice = AddParam(cmd, string.Format("@biprice_{0}", nxtCounter), bundle.Price, DbType.Decimal);
                        //(biprice != null) ? biprice : "@zero", 


                        //insert invoiceitem sql
                        //split out those items which need to be listed individually
                        if (listing.SelectedInventory.IsRequiresSeparateListing)
                        {
                            for (int i = 0; i < listing.Quantity; i++)
                                sb.Append(Checkout_Services.InsertInvoiceItem_Merch_Sql(biname, 
                                    "@zero", 
                                    "@singleQty", biidx, bicriteria, null, null));
                        }
                        else
                        {
                            string biqty = string.Format("@biqty_{0}", nxtCounter);
                            cmd.AddParameter(biqty, listing.Quantity, DbType.Int32);

                            sb.Append(Checkout_Services.InsertInvoiceItem_Merch_Sql(biname,
                                "@zero", 
                                biqty, biidx, bicriteria, null, null));
                        }

                        loopCounter++;//increment counter after adding selections
                    }
                }
            }
        }

        #region PostPurchaseText

        public static void ApplyPostText(StringBuilder postText, QueryCommand postCmd)
        {
            if (postText.Length > 0)
            {
                postCmd.CommandSql = postText.ToString();

                try
                {
                    SubSonic.DataService.ExecuteQuery(postCmd);
                }
                catch (Exception ex)
                {
                    _Error.LogException(ex, true);//sends an admin email as well
                }
            }

            postCmd = null;
        }

        public static void ConstructPostText(string userName, StringBuilder postText, QueryCommand postCmd, WebContext ctx)
        {
            postText.Length = 0;
            postCmd.Parameters.Clear();

            PostPurchaseTextCollection ppColl = new PostPurchaseTextCollection();
            postCmd.Parameters.Add(string.Format("@UniqueId"), ctx.SessionInvoice.UniqueId, DbType.String);
            int counter = 1645;//an arbitrary number so that params will not be intermingled with previous params

            foreach (SaleItem_Ticket sit in ctx.Cart.TicketItems)
            {
                sit.Ticket.PostPurchaseTextRecords().CopyTo(ppColl);

                SqlInsertPP(_Enums.InvoiceItemContext.ticket, ppColl, postText, postCmd, ctx,
                    userName, ref counter, sit.tShowTicketId, sit.tShowTicketId);
            }
            foreach (SaleItem_Merchandise mit in ctx.Cart.MerchandiseItems)
            {
                ppColl.Clear();

                Merch parent = (mit.MerchItem.IsParent) ? mit.MerchItem : mit.MerchItem.ParentMerchRecord;
                parent.PostPurchaseTextRecords().CopyTo(ppColl);

                SqlInsertPP(_Enums.InvoiceItemContext.merch, ppColl, postText, postCmd, ctx,
                    userName, ref counter, parent.Id, mit.tMerchId);
            }
            foreach (SaleItem_Promotion pit in ctx.Cart.PromotionItems)
            {
                foreach (Merch mrc in pit.SelectedAwardsMerchCollection)
                {
                    ppColl.Clear();

                    Merch parent = (mrc.IsParent) ? mrc : mrc.ParentMerchRecord;
                    foreach (PostPurchaseText ppt in parent.PostPurchaseTextRecords())
                    {
                        if (!ppColl.Contains(ppt))
                            ppColl.Add(ppt);
                    }

                    SqlInsertPP(_Enums.InvoiceItemContext.merch, ppColl, postText, postCmd, ctx,
                        userName, ref counter, parent.Id, mrc.Id);
                }
            }
        }

        private static void SqlInsertPP(_Enums.InvoiceItemContext context, PostPurchaseTextCollection ppColl, StringBuilder postText, QueryCommand postCmd, WebContext ctx,
            string userName, ref int idx, int parentIdx, int itemIdx)
        {
            string IDX = idx.ToString();

            if (ppColl.Count > 0)
                ppColl.Sort("IDisplayOrder", true);

            foreach (PostPurchaseText pp in ppColl)
            {
                if (pp.IsActive)
                {
                    postText.Append("INSERT	InvoiceItemPostPurchaseText([dtStamp], [TInvoiceItemId], [TPostPurchaseTextId], [PostText], [iDisplayOrder]) ");
                    postText.AppendFormat("SELECT	getDate(), ii.[Id] as 'TInvoiceItemId', @ppId_{0}, @processedText_{0} as 'PostText', @displayOrder_{0} ",
                        IDX);
                    postText.Append("FROM	[Invoice] i, [InvoiceItem] ii ");
                    postText.Append("WHERE	i.[UniqueId] = @uniqueId AND i.[Id] = ii.[tInvoiceId] AND ");

                    if (context == _Enums.InvoiceItemContext.ticket)
                        postText.AppendFormat("ii.[tShowTicketId] IS NOT NULL AND ii.[tShowTicketId] = @itemIdx_{0} ", IDX);
                    else if (context == _Enums.InvoiceItemContext.merch)
                        postText.AppendFormat("ii.[tMerchId] IS NOT NULL AND ii.[tMerchId] = @itemIdx_{0} ", IDX);

                    postCmd.Parameters.Add(string.Format("@ppId_{0}", IDX), pp.Id, DbType.Int32);
                    postCmd.Parameters.Add(string.Format("@processedText_{0}", IDX),
                        pp.PostTextProcessed(ctx.SessionInvoice, userName, parentIdx), DbType.String);
                    postCmd.Parameters.Add(string.Format("@displayOrder_{0}", IDX), pp.DisplayOrder, DbType.Int32);
                    postCmd.Parameters.Add(string.Format("@itemIdx_{0}", IDX), itemIdx, DbType.Int32);

                    idx++;
                }
            }
        }

        #endregion

        #region DeliveryCodes

        public static void RecordDeliveryCodes(WebContext ctx)
        {
            Invoice i = ctx.SessionAuthorizeNet.InvoiceRecord;

            foreach (InvoiceItem ii in i.InvoiceItemRecords())
            {
                if (ii.IsMerchandiseItem)
                {
                    //match the merch item to current saleitems
                    Merch m = (Merch)ctx.SaleMerch.Find(ii.TMerchId);
                    //if not found do it the hard way
                    if (m == null)
                        m = ii.MerchRecord;

                    //NOTE: We are not comparing to the invoiceItem here - it has not been properly initialized with delivery codes yet!
                    //if the merch item is download or unique code delivery
                    //than record the unique code to the db
                    if (m != null && (m.IsDownloadDelivery || m.IsActivationCodeDelivery || m.IsGiftCertificateDelivery))
                    {
                        //gives us code=xxxx
                        string code = Inventory.CreateDeliveryCodeForInvoiceItem(ii, m, true);
                    }
                }
            }
        }
        #endregion
    }
}