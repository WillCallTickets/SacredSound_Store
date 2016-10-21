using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wcss
{
    /// <summary>
    ///
    /// </summary>
    public partial class MerchBundle
    {
        /// <summary>
        /// we only attach to merch parents
        /// </summary>
        [XmlAttribute("ParentDescription")]
        public string ParentDescription
        {
            get
            {
                return this.MerchRecord.DisplayNameWithAttribs;
            }
        }

        #region Properties

        [XmlAttribute("IsActive")]
        public bool IsActive
        {
            get { return this.BActive; }
            set { this.BActive = value; }
        }
        [XmlAttribute("DisplayOrder")]
        public int DisplayOrder
        {
            get { return this.IDisplayOrder; }
            set { this.IDisplayOrder = value; }
        }
        [XmlAttribute("IncludeWeight")]
        public bool IncludeWeight
        {
            get { return this.BIncludeWeight; }
            set { this.BIncludeWeight = value; }
        }
        [XmlAttribute("RequiredParentQty")]
        public int RequiredParentQty
        {
            get { return this.IRequiredParentQty; }
            set { this.IRequiredParentQty = value; }
        }
        [XmlAttribute("MaxSelections")]
        public int MaxSelections
        {
            get { return this.IMaxSelections; }
            set { this.IMaxSelections = value; }
        }
        [XmlAttribute("Price")]
        public decimal Price
        {
            get { return this.MPrice; }
            set { this.MPrice = value; }
        }
        [XmlAttribute("PricedPerSelection")]
        public bool PricedPerSelection
        {
            get { return (this.BPricedPerSelection.HasValue) ? this.BPricedPerSelection.Value : false; }
            set { this.BPricedPerSelection = value; }
        }
        [XmlAttribute("TitleEncoded")]
        public string TitleEncoded
        {
            get
            {
                return System.Web.HttpUtility.HtmlEncode(this.Title);
            }
        }

        #endregion

        #region Derived Properties and Methods

        /// <summary>
        /// retrieve a child item by its id
        /// </summary>
        /// <param name="bundleItemId"></param>
        /// <returns></returns>
        //public MerchBundleItem GetMerchBundleItem(int bundleItemId)
        //{
        //    return (MerchBundleItem)this.MerchBundleItemRecords().Find(bundleItemId);
        //}

        /// <summary>
        /// from a bundle and its items, gets a list of merch that are running and have inventory
        /// we do not bother checking for the bundle to be active or running here. First iteration 
        /// checks the Current Item for unlocked and running, etc
        private MerchCollection _activeInventory = null;
        public MerchCollection ActiveInventory
        {
            get
            {
                if (_activeInventory == null)
                {
                    _activeInventory = new MerchCollection();

                    MerchBundleItemCollection bunItems = new MerchBundleItemCollection();
                    bunItems.AddRange(this.MerchBundleItemRecords().GetList().FindAll(delegate(MerchBundleItem match) { return match.IsActive; }));

                    if (bunItems.Count > 1)
                        bunItems.Sort("DisplayOrder", true);

                    foreach (MerchBundleItem bun in bunItems)
                        _activeInventory.AddRange(Get_ActiveInventory(bun));
                }

                return _activeInventory;
            }
        }
        private MerchCollection Get_ActiveInventory(MerchBundleItem bun)
        {
            return bun.AvailableInventory.OrderByAsc("DisplayNameWithAttribs");
        }

        public bool IsMultSelection
        {
            get { return this.MaxSelections > 1; }
        }
        /// <summary>
        /// Indicates the bundle is priced and is always optional
        /// </summary>
        public bool OffersOptout
        {
            get { return this.Price > 0; }
        }
        public bool HasOnlyOneAvailableSelection
        {
            get
            {
                //if we have an opt out
                if (OffersOptout)
                    return false;

                return (this.ActiveInventory.Count == 1);
            }
        }
        public int GetOnlyAvailableSelectiondId()
        {
            if(this.ActiveInventory.Count == 1)
                return this.ActiveInventory[0].Id;

            return 0;
        }

        #endregion

        #region Static Methods

        public static string DisplayBundle_Listing(ShowTicket ticket,
            bool displayComment, bool includeInstructions,
            bool showImages, string pageName)
        {
            //get list of bundle that are running and are unlocked
            //further refine the list to include only those items with inventory
            MerchBundleCollection bundleColl = ticket.MerchBundleRecords().Get_MerchBundleRecords_RunningAndAvailable();

            return DisplayBundle_Listing(bundleColl, displayComment, includeInstructions, false, pageName);
        }
        public static string DisplayBundle_Listing(Merch merch,
            bool displayComment, bool includeInstructions,
            bool showImages, string pageName)
        {
            //get list of bundle that are running and are unlocked
            //further refine the list to include only those items with inventory
            MerchBundleCollection bundleColl = merch.MerchBundleRecords().Get_MerchBundleRecords_RunningAndAvailable();

            return DisplayBundle_Listing(bundleColl, displayComment, includeInstructions, showImages, pageName);
        }
        private static string DisplayBundle_Listing(MerchBundleCollection bundleColl, 
            bool displayComment, bool includeInstructions,
            bool showImages, string pageName)
        {
            //if we have bundles for this item and those bundles have available inventory 
            // - than we display the fact that there is an available bundle to the user
            if (bundleColl.Count > 0)
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.AppendLine("<div class=\"bundle-offer\">");

                foreach (MerchBundle oven in bundleColl)
                {
                    sb.AppendLine("<div class=\"bundle-container\">");

                    //send -1 as num selections - as we are not displaying the num selections anyway 
                    sb.AppendLine(DisplayBundle_Item(-1, oven, displayComment, false, null, pageName));

                    //display bundle images
                    if (showImages)
                        sb.Append(DisplayBundle_Images(oven));
                    else
                        sb.Append(DisplayBundle_Choices(oven));

                    sb.AppendLine("</div>");
                }

                if (includeInstructions)
                {
                    sb.AppendFormat("<div class=\"bundle-instructions\">{0}</div>", _Config._Message_MerchBundleInstruction);
                    sb.AppendLine();
                }

                sb.AppendLine("</div>");

                return sb.ToString();
            }

            return string.Empty;
        }
        public static string DisplayBundle_Choices(MerchBundle bundle)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            MerchBundleItemCollection mcoll = new MerchBundleItemCollection();
            mcoll.AddRange(bundle.MerchBundleItemRecords().GetList().FindAll(delegate(MerchBundleItem match) { return match.IsActive; }));

            if (mcoll.Count > 0)
            {   
                foreach (MerchBundleItem mbi in mcoll)
                {
                    sb.AppendLine("<div class=\"bundle-itm-container\">");
                    sb.AppendLine(mbi.MerchRecord.DisplayNameWithAttribs);
                    sb.AppendLine("</div>");
                }
            }

            return sb.ToString();
        }
        public static string DisplayBundle_Images(MerchBundle bundle)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            MerchBundleItemCollection mcoll = new MerchBundleItemCollection();
            mcoll.AddRange(bundle.MerchBundleItemRecords().GetList().FindAll(delegate(MerchBundleItem match) { return match.IsActive; }));

            if (mcoll.Count > 0)
            {
                sb.AppendLine("<div class=\"bundle-img-wrapper\">");
                sb.AppendLine("<div class=\"bundle-img-container\">");

                foreach (MerchBundleItem mbi in mcoll)
                {
                    Merch merch = mbi.MerchRecord;
                    string displayName = merch.DisplayNameWithAttribs;
                    if (merch.IsChild)
                        merch = merch.ParentMerchRecord;
                    
                    ItemImageCollection icoll = new ItemImageCollection();
                    icoll.AddRange(merch.ItemImageRecords().GetList().FindAll(delegate(ItemImage match) { return match.IsItemImage; }));
                    if (icoll.Count > 1)
                        icoll.Sort("IDisplayOrder", true);

                    if (icoll.Count > 0)
                    {
                        ItemImage img = icoll[0];
                        sb.AppendLine("<div class=\"bundle-img\">");
                        
                        sb.AppendFormat("<img src=\"{0}\" border=\"0\" />", img.Thumbnail_Small);
                        sb.AppendLine();
                        sb.AppendLine("<div class=\"bundle-img-cluetip\">");
                        sb.AppendLine("<div class=\"cluetainer\">");
                        sb.AppendFormat("<div class=\"cluetext\" >{0}</div>", displayName);
                        sb.AppendLine();
                        sb.AppendFormat("<img src=\"{0}\" border=\"0\" alt=\"{1}\" />", img.Thumbnail_Small, displayName);
                        sb.AppendLine();
                        sb.AppendLine("</div>");//cluetainer
                        sb.AppendLine("</div>");//bundle-img-cluetip
                        sb.AppendLine("</div>");//bundle-img
                    }
                }

                sb.AppendLine("</div>");
                sb.AppendLine("</div>");
            }

            return sb.ToString();
        }

        /// <summary>
        /// This version should be used in the shopping cart
        /// </summary>
        public static string DisplayBundle_Item(int selectedItems, MerchBundle oven, bool displayComment, bool displayNumSelections, string hasNotMetRequirement,
            string pageName)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine("<div class=\"bundle-header\">");
            sb.AppendFormat("<div class=\"bundle-title\">{0}</div>", oven.Title);
            sb.AppendLine();

            if (displayComment && oven.Comment != null && oven.Comment.Trim().Length > 0)
            {
                sb.AppendFormat("<div class=\"bundle-comment\">{0}</div>", oven.Comment);
                sb.AppendLine();
            }

            if (hasNotMetRequirement != null && hasNotMetRequirement.Trim().Length > 0)
            {
                sb.AppendFormat("<div class=\"bundle-requires\">{0}", hasNotMetRequirement.Trim());
                if (pageName.ToLower().IndexOf("store_cart_edit_aspx") == -1)
                    sb.Append(" <a class=\"btntribe\" href=\"/Store/Cart_Edit.aspx\">edit cart</a>");
                sb.Append("</div>");
                sb.AppendLine();
            }
            else if ((! oven.HasOnlyOneAvailableSelection) && displayNumSelections)
            {
                sb.AppendFormat("<div class=\"bundle-numchoices\"{0}>You have selected {1} of {2} offers from this bundle.{3}</div>",
                    (selectedItems < oven.MaxSelections) ? " style=\"color:red;\"" : string.Empty,
                    selectedItems.ToString(), oven.MaxSelections.ToString(),
                    ((pageName.ToLower().IndexOf("store_cart_edit_aspx") == -1) && (selectedItems < oven.MaxSelections)) ? 
                        string.Format(" <a class=\"btntribe\" href=\"/Store/Cart_Edit.aspx\">edit cart</a>") : string.Empty
                    
                    );
                sb.AppendLine();
            }

            sb.AppendLine("</div>");

            return sb.ToString();
        }


        #endregion
    }

    public partial class MerchBundleCollection : Utils._Collection.IOrderable<MerchBundle>
    {
        /// <summary>
        /// Checks to see that the bundle is up and running - sees if it matches any codes necessary - and verifies that there is inventory available
        /// Then sorts by display order
        /// </summary>
        /// <param name="unlockCode"></param>
        /// <param name="couponCodes"></param>
        /// <returns></returns>
        public Wcss.MerchBundleCollection Get_MerchBundleRecords_RunningAndAvailable()
        {
            MerchBundleCollection coll = new MerchBundleCollection();
            coll.AddRange(this.GetList().FindAll(delegate(MerchBundle match)
            {
                return (match.IsActive && match.ActiveInventory.Count > 0 );
            }));

            if (coll.Count > 1)
                coll.Sort("IDisplayOrder", true);

            return coll;            
        }

        public MerchBundle AddToCollection(ShowTicket parent, int requiredParentQty, string title, string comment, decimal price, bool pricedPerSelection, int maxSelection)
        {
            System.Collections.Generic.List<System.Web.UI.Pair> args = new System.Collections.Generic.List<System.Web.UI.Pair>();
            args.Add(new System.Web.UI.Pair("DtStamp", DateTime.Now));
            args.Add(new System.Web.UI.Pair("IsActive", true));
            //displayorder, startdate, enddate, unlockcode            
            args.Add(new System.Web.UI.Pair("TShowTicketId", parent.Id));
            args.Add(new System.Web.UI.Pair("Title", title));
            args.Add(new System.Web.UI.Pair("Comment", comment));
            args.Add(new System.Web.UI.Pair("RequiredParentQty", requiredParentQty));
            args.Add(new System.Web.UI.Pair("MaxSelections", maxSelection));
            args.Add(new System.Web.UI.Pair("Price", price));
            args.Add(new System.Web.UI.Pair("PricedPerSelection", pricedPerSelection));
            args.Add(new System.Web.UI.Pair("IncludeWeight", false));

            MerchBundle newItem = AddToCollection(args);

            return newItem;
        }
        public MerchBundle AddToCollection(Merch parent, int requiredParentQty, string title, string comment, decimal price, bool pricedPerSelection, int maxSelection)
        {
            System.Collections.Generic.List<System.Web.UI.Pair> args = new System.Collections.Generic.List<System.Web.UI.Pair>();
            args.Add(new System.Web.UI.Pair("DtStamp", DateTime.Now));
            args.Add(new System.Web.UI.Pair("IsActive", true));
            //displayorder, startdate, enddate, unlockcode            
            args.Add(new System.Web.UI.Pair("TMerchId", parent.Id));            
            args.Add(new System.Web.UI.Pair("Title", title));
            args.Add(new System.Web.UI.Pair("Comment", comment));
            args.Add(new System.Web.UI.Pair("RequiredParentQty", requiredParentQty));
            args.Add(new System.Web.UI.Pair("MaxSelections", maxSelection));
            args.Add(new System.Web.UI.Pair("Price", price));
            args.Add(new System.Web.UI.Pair("PricedPerSelection", pricedPerSelection)); 
            args.Add(new System.Web.UI.Pair("IncludeWeight", false));

            MerchBundle newItem = AddToCollection(args);

            return newItem;
        }

        public MerchBundle AddToCollection(System.Collections.Generic.List<System.Web.UI.Pair> args)
        {
            return Utils._Collection.AddItemToOrderedCollection(this.GetList(), args);
        }

        /// <summary>
        /// Delete a MerchBundle from the collection by ID
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        public bool DeleteFromCollection(int idx)
        {
            //we must manually remove child items - MerchBundleItems - from collection
            MerchBundle currentPackage = (MerchBundle)this.Find(idx);
            if (currentPackage != null)
            {
                while (currentPackage.MerchBundleItemRecords().Count > 0)
                    currentPackage.MerchBundleItemRecords().DeleteFromCollection(currentPackage.MerchBundleItemRecords()[currentPackage.MerchBundleItemRecords().Count - 1].Id);
            }

            return Utils._Collection.DeleteFromOrderedCollection(this.GetList(), idx);
        }

        /// <summary>
        /// Reorder a MerchBundle by ID and direction
        /// </summary>
        /// <param name="idx"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public MerchBundle ReorderItem(int idx, string direction)
        {
            return Utils._Collection.ReorderOrderedCollection(this.GetList(), idx, direction);
        }
    }
}
