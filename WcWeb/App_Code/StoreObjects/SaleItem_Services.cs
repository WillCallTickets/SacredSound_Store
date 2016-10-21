using System;
using System.Collections.Generic;
using System.Text;
using Wcss;

namespace WillCallWeb.StoreObjects
{
    public class Checkout_Services
    {
        /// <summary>
        /// procId is assigned to criteria
        /// </summary>
        public static string InsertInvoiceItem_ProcessingFee_Sql(string mainActNameVar, string priceVar, string qtyVar, string procId)
        {
            return Checkout_Services.InsertInvoiceItem(true, _Enums.InvoiceItemContext.processing, null, mainActNameVar, priceVar, qtyVar, false, 
                null, null, null, null, null, null, 
                null, 
                null, 
                procId, null, null);
        }
        public static string InsertInvoiceItem_Ticket_Sql(string mainActNameVar, string priceVar, string qtyVar,
            string showTicketIdVar, string showIdVar, string ageVar, string serviceChargeVar, string pickupNameVar,
            string showDateVar, 
            string criteriaVar, string descriptionVar)
        {
            return Checkout_Services.InsertInvoiceItem(true, _Enums.InvoiceItemContext.ticket, null, mainActNameVar, priceVar, qtyVar, true, 
                showTicketIdVar, showIdVar, ageVar, serviceChargeVar, pickupNameVar, 
                showDateVar, 
                null, 
                null, 
                criteriaVar, descriptionVar, null);
        }
        /// <summary>
        /// Items must be checked for split items - downloads/passes - when looped
        /// </summary>
        public static string InsertInvoiceItem_Merch_Sql(string mainActNameVar, string priceVar, string qtyVar,
            string merchIdVar,
            string criteriaVar, string descriptionVar, string notesVar)
        {
            return Checkout_Services.InsertInvoiceItem(true, _Enums.InvoiceItemContext.merch, null, mainActNameVar, priceVar, qtyVar, true,
                null, null, null, null, null,
                null,
                merchIdVar,
                null,
                criteriaVar, descriptionVar, notesVar);
        }
        /// <summary>
        /// Criteria will be formed later in process as we do not yet have the invoiceitem id
        /// </summary>
        public static string InsertInvoiceItem_MerchBundleEntry_Sql(bool isPurchased, string mainActNameVar, string priceVar, string qtyVar, 
            string criteriaVar, string descriptionVar)
        {
            return Checkout_Services.InsertInvoiceItem(isPurchased, _Enums.InvoiceItemContext.bundle, null, mainActNameVar, priceVar, qtyVar, true,
                null, null, null, null, null,
                null,
                null,
                null,
                criteriaVar, descriptionVar, null);
        }
        /// <summary>
        /// Use when context is irregular
        /// </summary>
        public static string InsertInvoiceItem_PromotionItem_Sql(bool isPurchased, string contextVar,
            string mainActNameVar, string priceVar, string qtyVar,
            string merchIdVar,
            string promoIdxVar,
            string criteriaVar, string descriptionVar, string notesVar)
        {
            return Checkout_Services.InsertInvoiceItem(isPurchased, _Enums.InvoiceItemContext.notassigned, contextVar, mainActNameVar, priceVar, qtyVar, true,
                null, null, null, null, null,
                null,
                merchIdVar,
                promoIdxVar,
                criteriaVar, descriptionVar, notesVar);
        }
        /// <summary>
        /// Use for typed context
        /// </summary>
        public static string InsertInvoiceItem_PromotionItem_Sql(bool isPurchased, _Enums.InvoiceItemContext context,
            string mainActNameVar, string priceVar, string qtyVar,
            string merchIdVar,
            string promoIdxVar,
            string criteriaVar, string descriptionVar, string notesVar)
        {
            return Checkout_Services.InsertInvoiceItem(isPurchased, context, null, mainActNameVar, priceVar, qtyVar, true,
                null, null, null, null, null,
                null,
                merchIdVar,
                promoIdxVar,
                criteriaVar, descriptionVar, notesVar);
        }
        public static string InsertInvoiceItem_ShippingItem_Sql(string contextVar,
            string mainActNameVar, string priceVar, string qtyVar, string showDateVar)
        {
            return Checkout_Services.InsertInvoiceItem(true, _Enums.InvoiceItemContext.notassigned, contextVar, mainActNameVar, priceVar, qtyVar, false,
                null, null, null, null, null,
                showDateVar,
                null,
                null,
                null, null, null);
        }
        public static string InsertInvoiceItem_DonationItem_Sql(string contextVar,
            string mainActNameVar, string priceVar, string qtyVar,
            string criteriaVar)
        {
            return Checkout_Services.InsertInvoiceItem(true, _Enums.InvoiceItemContext.notassigned, contextVar, mainActNameVar, priceVar, qtyVar, false,
                null, null, null, null, null,
                null,
                null,
                null,
                criteriaVar, null, null);
        }
        public static string InsertInvoiceItem_NoteItem_Sql(string note)
        {
            return Checkout_Services.InsertInvoiceItem(true, _Enums.InvoiceItemContext.noteitem, null, note, "@zero", "@singleQty", false,
                null, null, null, null, null,
                null,
                null,
                null,
                null, null, null);
        }
        /// <summary>
        /// this one is for bundles and....
        /// </summary>
        /// <param name="sb"></param>
        private static string InsertInvoiceItem(
            bool isPurchased, _Enums.InvoiceItemContext context, string contextVar, string mainActNameVar, string priceVar, string qtyVar, bool includePurchaseName,
            string showTicketIdVar, string showIdVar, string ageVar, string serviceChargeVar, string pickupNameVar,
            string showDateVar,//also used for shipping
            string merchIdVar,
            string promoIdVar,
            string criteriaVar, string descriptionVar, string notesVar
            )
        {
            StringBuilder insert = new StringBuilder();
            StringBuilder values = new StringBuilder();

            insert.AppendFormat("INSERT [InvoiceItem] ([TInvoiceId], [PurchaseAction], [vcContext], [MainActName], [mPrice], [iQuantity], {0}",
                (includePurchaseName) ? "[PurchaseName], " : string.Empty);
            values.AppendFormat("VALUES (@invoiceId, {0}, {1}, {2}, {3}, {4}, {5}",
                (isPurchased) ? "@purchased" : "'NotYetPurchased'", 
                (contextVar != null) ? contextVar : string.Format("'{0}'", context.ToString()),
                mainActNameVar, priceVar, qtyVar,
                (includePurchaseName) ? "@purchaseName, " : string.Empty);

            if (showTicketIdVar != null)//assume all others are in there too
            {
                insert.Append("[TShowTicketId], [TShowId], [AgeDescription], [mServiceCharge], [PickupName], ");
                values.AppendFormat("{0}, {1}, {2}, {3}, {4}, ", showTicketIdVar, showIdVar, ageVar, serviceChargeVar, pickupNameVar);
            }
            if (showDateVar != null)//assume all others are in there too
            {
                insert.Append("[dtDateOfShow], ");
                values.AppendFormat("{0}, ", showDateVar);
            }
            if (merchIdVar != null)
            {
                insert.Append("[TMerchId], ");
                values.AppendFormat("{0}, ", merchIdVar);
            }
            if (promoIdVar != null)
            {
                insert.Append("[TSalePromotionId], ");
                values.AppendFormat("{0}, ", promoIdVar);
            }
            if (criteriaVar != null)
            {
                insert.Append("[Criteria], ");
                values.AppendFormat("{0}, ", criteriaVar);
            }
            if (descriptionVar != null)
            {
                insert.Append("[Description], ");
                values.AppendFormat("{0}, ", descriptionVar);
            }
            if (notesVar != null)
            {
                insert.Append("[Notes], ");
                values.AppendFormat("{0}, ", notesVar);
            }

            return string.Format("{0}) {1})", insert.ToString().TrimEnd(new char[] { ',', ' ' }), values.ToString().TrimEnd(new char[] { ',', ' ' }));
        }
    }

    public class SaleItem_Services
    {
        private static System.Text.StringBuilder sb = new System.Text.StringBuilder();
        private static System.Text.StringBuilder ss = new System.Text.StringBuilder();

        #region WebMethods Called from client

        public static object InitBundleEditor(WebContext _ctx, string context, int saleItem_ItemId, int bundleId)
        {
            SaleItem_Base saleItemBase = GetSaleItemBase(context, _ctx, saleItem_ItemId);
            
            if (saleItemBase != null)
            {
                MerchBundle bundle = saleItemBase.GetMerchBundle(bundleId);
                List<MerchBundle_Listing> selections = saleItemBase.GetValidMerchBundleListings_Selected(bundleId);

                int maxSelectionsAllowed = _ctx.Cart.GetMaxPossibleSelectionsAllowedForBundle(saleItemBase, bundleId);
                int qtySelected = GetQtySelected(selections);            

                //return a json object with our info
                return ReturnJsonBundleInfo(selections, saleItemBase, bundle, qtySelected, maxSelectionsAllowed, _ctx);
            }

            return "false";
        }

        public static object ClearAll(WebContext _ctx, string context, int saleItem_ItemId, int bundleId)
        {
            SaleItem_Base saleItemBase = GetSaleItemBase(context, _ctx, saleItem_ItemId);

            if (saleItemBase != null)
            {
                MerchBundle bundle = saleItemBase.GetMerchBundle(bundleId);

                int maxSelectionsAllowed = _ctx.Cart.GetMaxPossibleSelectionsAllowedForBundle(saleItemBase, bundleId);

                saleItemBase.MerchBundleSelections.RemoveAll(delegate(MerchBundle_Listing match) { return match.BundleId == bundleId; });

                //return a json object with our info
                return ReturnJsonBundleInfo(null, saleItemBase, bundle, 0, maxSelectionsAllowed, _ctx);
            }

            return "false";
        }

        private static int GetRealTimeRemaining(int selectedItemId)
        {

            return 0;
        }        

        /// <summary>
        /// Return a JSON object to the caller. Item 1 will have "you have selected 0 out of 3 selections. 
        /// Item 2 will be the selection list to populate the control
        /// Only one item is added at a time - qty = 1
        /// </summary>
        /// <param name="merchItemId"></param>
        /// <param name="bundleId"></param>
        /// <param name="selectedItemId"></param>
        /// <returns></returns>
        public static object AddChoice(WebContext _ctx, string context, int saleItem_ItemId, int bundleId, int selectedItemId)
        {
            SaleItem_Base saleItemBase = GetSaleItemBase(context, _ctx, saleItem_ItemId);

            if (saleItemBase != null)
            {
                MerchBundle bundle = saleItemBase.GetMerchBundle(bundleId);
                List<MerchBundle_Listing> selections = saleItemBase.GetValidMerchBundleListings_Selected(bundleId);

                int maxSelectionsAllowed = _ctx.Cart.GetMaxPossibleSelectionsAllowedForBundle(saleItemBase, bundleId);
                int qtySelected = GetQtySelected(selections);
                int qtyToAdd = 1;
                string msg = null;

                //if we have selected the opt out then clear any other selections
                if (selectedItemId == 0)
                    saleItemBase.MerchBundleSelections.RemoveAll(delegate(MerchBundle_Listing match) { return match.BundleId == bundleId; });
                else
                {
                    if ((qtySelected + qtyToAdd) <= maxSelectionsAllowed)
                    {
                        MerchBundle_Listing existing = GetExistingListing(selections, saleItemBase, bundleId, selectedItemId);

                        //make sure that real-time inventory is valid
                        //if it is not - remove it from the available choices
                        //indicate to user that no more are available
                        Merch available = (Merch)bundle.ActiveInventory.Find(selectedItemId);
                        int availableQty = available.Available;

                        //todo handle this rare case? wait to see if it becomes an issue
                        //if (existing != null && existing.Quantity > availableQty)//if the qty has been changed
                        //{
                        //    //alter the selections
                        //}
                        //else 
                         
                        //if we cannot add
                        if ((existing != null && (existing.Quantity + qtyToAdd) > availableQty) ||
                            (existing == null && qtyToAdd > availableQty))
                        {
                            msg = "Sorry, we are currently out of available inventory for that item.";
                        }
                        else
                        {
                            if (existing != null)
                            {
                                existing.Quantity += 1;
                            }
                            //else create a new selection
                            else
                            {
                                if (bundle == null)
                                    return false.ToString();

                                MerchBundle_Listing newListing = new MerchBundle_Listing(0, bundle, selectedItemId, 1);
                                saleItemBase.MerchBundleSelections.Add(newListing);
                            }

                            qtySelected += qtyToAdd;
                        }
                    }
                    //if we don't have available slots, then ignore
                }
                
                //return a json object with our info
                return ReturnJsonBundleInfo(selections, saleItemBase, bundle, qtySelected, maxSelectionsAllowed, _ctx, msg);
            }

            return "false";
        }

        public static object RemoveOne(WebContext _ctx, string context, int saleItem_ItemId, int bundleId, int selectedItemId)
        {
            SaleItem_Base saleItemBase = GetSaleItemBase(context, _ctx, saleItem_ItemId);

            if (saleItemBase != null)
            {
                MerchBundle bundle = saleItemBase.GetMerchBundle(bundleId);
                List<MerchBundle_Listing> selections = saleItemBase.GetValidMerchBundleListings_Selected(bundleId);

                int maxSelectionsAllowed = _ctx.Cart.GetMaxPossibleSelectionsAllowedForBundle(saleItemBase, bundleId);
                int qtySelected = GetQtySelected(selections);                

                //if we have selected the opt out then clear any other selections
                if (selectedItemId == 0)
                    saleItemBase.MerchBundleSelections.RemoveAll(delegate(MerchBundle_Listing match) { return match.BundleId == bundleId; });
                else
                {
                    MerchBundle_Listing existing = GetExistingListing(selections, saleItemBase, bundleId, selectedItemId);
                    //if there is an existing one - we are going to edit its quantity if > 0 - else remove it
                    saleItemBase.MerchBundleSelections.RemoveAll(delegate(MerchBundle_Listing match) { return match.BundleId == bundleId && (match.IsOptOut); });

                    if (existing != null)
                    {
                        if (existing.Quantity > 1)
                            existing.Quantity -= 1;
                        else
                            saleItemBase.MerchBundleSelections.Remove(existing);

                        qtySelected -= 1;
                    }

                    //if we don't have available slots, then ignore
                }

                //selections.Clear();
                //selections.AddRange(saleItemBase.MerchBundleSelections.FindAll(delegate(MerchBundle_Listing match) { return (match.BundleId == bundleId); }));

                //return a json object with our info
                return ReturnJsonBundleInfo(selections, saleItemBase, bundle, qtySelected, maxSelectionsAllowed, _ctx);
            }

            return "false";
        }

        private static MerchBundle_Listing GetExistingListing(List<MerchBundle_Listing> selections, SaleItem_Base saleItemBase, int bundleId, int selectedItemId)
        {
            MerchBundle_Listing existing = null;

            if (selections.Count > 0)
            {
                //if there is an existing one - we are going to edit its quantity if > 0 - else remove it
                saleItemBase.MerchBundleSelections.RemoveAll(delegate(MerchBundle_Listing match) { return match.BundleId == bundleId && (match.IsOptOut); });

                //see if there is already an existing selection
                existing = saleItemBase.MerchBundleSelections
                    .Find(delegate(MerchBundle_Listing match) { return (match.BundleId == bundleId && match.SelectedInventoryId == selectedItemId); });
            }

            return existing;
        }

        public static int GetQtySelected(List<MerchBundle_Listing> selections)
        {
            int qtySelected = 0;

            foreach (MerchBundle_Listing listing in selections)
                qtySelected += listing.Quantity;

            return qtySelected;
        }

        private static SaleItem_Base GetSaleItemBase(string context, WebContext _ctx, int saleItem_ItemId)
        {
            bool isMerch = (context.ToLower() == "m");
            bool isTicket = (context.ToLower() == "t");

            if ((isMerch && _ctx.Cart.HasMerchandiseItems) || (isTicket && _ctx.Cart.HasTicketItems))
            {
                return (isMerch) ?
                    (SaleItem_Base)_ctx.Cart.MerchandiseItems.Find(delegate(SaleItem_Merchandise match) { return (match.tMerchId == saleItem_ItemId); }) :
                    (SaleItem_Base)_ctx.Cart.TicketItems.Find(delegate(SaleItem_Ticket match) { return (match.tShowTicketId == saleItem_ItemId); });
            }

            return null;
        }

        private static object ReturnJsonBundleInfo(List<MerchBundle_Listing> selections, SaleItem_Base saleItemBase, 
            MerchBundle bundle, int qtySelected, int maxSelectionsAllowed, WebContext _ctx)
        {
            return ReturnJsonBundleInfo(selections, saleItemBase, bundle, qtySelected, maxSelectionsAllowed, _ctx, null);
        }
        private static object ReturnJsonBundleInfo(List<MerchBundle_Listing> selections, SaleItem_Base saleItemBase, 
            MerchBundle bundle, int qtySelected, int maxSelectionsAllowed, WebContext _ctx, string msg)
        {
            if (selections != null)
            {
                selections.Clear();
                selections.AddRange(saleItemBase.MerchBundleSelections.FindAll(delegate(MerchBundle_Listing match) { return (match.BundleId == bundle.Id); }));
            }

            //return a json object with our info
            return new
            {
                SelectStatus = string.Format("You have selected {0} of {1} selections for this bundle.", qtySelected.ToString(), maxSelectionsAllowed.ToString()),
                ListContent = GetListInnerHtml(selections),
                Total = GetPriceLineInnerHtml(saleItemBase, bundle),
                Message = (msg != null) ? msg : string.Empty
            };
        }

        public static object SetComplianceDate18(WebContext _ctx, string userName, string profileDob, string month, string day, string year)
        {   
            DateTime complianceDate = DateTime.MaxValue;

            if (DateTime.TryParse(string.Format("{0}/{1}/{2}", month, day, year), out complianceDate))
            {
                _ctx.UserAgeComplianceDate = complianceDate;

                //record an event for the current user
                if (userName != null && userName.Trim().Length > 0 && userName.ToLower() != "anonymous")
                    UserEvent.NewUserEvent(userName, DateTime.Now, DateTime.Now, _Enums.EventQStatus.Success,
                        userName, _Enums.EventQContext.User, _Enums.EventQVerb.AgeVerify18Submission, string.Format("account profile dob: {0}", profileDob),
                        _ctx.UserAgeComplianceDate.ToString("MM/dd/yyyy"), 
                        (_ctx.UserAgeComplianceDate.ToString("MM/dd/yyyy") != profileDob) ? "DOES NOT MATCH PROFILE" : string.Empty, true);
            }
            
            //return a json object with our info
            return new
            {
                Message = 
                (complianceDate == DateTime.MaxValue) ? "You have entered an invalid date." :
                (_ctx.UserIs18OrOlder) ? "SUCCESS" :
                "You must be 18 years of age to view this product."
            };
        }


        #endregion

        #region Static ListConstructors

        private static string GetChoices(MerchBundle bundle)
        {
            sb.Length = 0;//reset

            //display a distinct list of items to choose from
            MerchCollection coll = new MerchCollection();
            coll.CopyFrom(bundle.ActiveInventory);

            if (coll.Count > 0)
                foreach (Merch inventory in coll)
                    sb.Append(SaleItem_Services.ConstructListElement(inventory));

            return sb.ToString();
        }

        public static string GetListInnerHtml(List<MerchBundle_Listing> selections)
        {
            sb.Length = 0;

            sb.AppendLine();

            if (selections == null || selections.Count == 0)
            {
                //sb.Append(ConstructListElement(null, false));
            }
            else
            {
                foreach (MerchBundle_Listing listing in selections)
                    for (int i = 0; i < listing.Quantity; i++)
                        sb.Append(ConstructListElement(listing));
            }

            return sb.ToString();
        }

        /// <summary>
        /// Selections should be pre-screened for matching the bundle
        /// </summary>
        public static string GetPriceLineInnerHtml(SaleItem_Base saleItem, MerchBundle bundle)//, List<MerchBundle_Listing> selections, WebContext ctx)
        {
            ss.Length = 0;

            //only show the innards if there is an opt out - but keep the structure
            if (bundle.OffersOptout)
            {
                ss.AppendLine("<div class=\"priceline-wrapper\">");
                ss.AppendFormat("<span>Bundle Total:</span> <span class=\"amount\">{0}</span>", saleItem.GetIndividualBundlePrice(bundle).ToString("c"));
                ss.AppendLine("</div>");
            }

            return ss.ToString();
        }

        private static string ConstructListElement(MerchBundle_Listing listing)
        {
            return ConstructListElement(listing.SelectedInventory, true);
        }
        public static string ConstructListElement(Merch merch)
        {
            return ConstructListElement(merch, false);
        }
        private static string ConstructListElement(Merch inventory, bool isChoiceElement)
        {
            ss.Length = 0;

            ItemImage img = (inventory != null) ?
                inventory.ParentMerchRecord.ItemImageRecords().GetList().Find(delegate(ItemImage match) { return (match.IsItemImage); }) : null;

            //only show msg for choices
            if (isChoiceElement && inventory == null)
                ss.Append("You have no selections for this bundle.");
            else if(inventory != null)
            {
               if (isChoiceElement)
                {
                    //include structure from jquery
                    ss.AppendLine("<div class=\"item icart\">");
                    ss.AppendFormat("<div class=\"divrm\"><a title=\"remove selection\" onclick=\"remove(this)\" class=\"remove {0}\"></a></div>", inventory.Id.ToString());
                    ss.AppendLine();
                }

                //reconstruct list item
                ss.AppendFormat("<div class=\"choice\" id=\"{0}\">", inventory.Id.ToString());

                if (img != null)
                    ss.AppendFormat("<img src=\"{0}\" alt=\"\" /> ", img.Thumbnail_Small);

                ss.AppendFormat("{0}</div>", inventory.DisplayNameWithAttribs);

                //include structure from jquery - close it up
                if (isChoiceElement)
                    ss.AppendLine("</div>");
            }

            ss.AppendLine();

            return ss.ToString();
        }

        #endregion
    }
}