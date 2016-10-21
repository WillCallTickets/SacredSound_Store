using System;
using System.Xml.Serialization;

namespace Wcss
{
    /// <summary>
    ///
    /// </summary>
    public partial class MerchBundleItem
    {
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

        /// <summary>
        /// Checks the corresponding merch offering for availability
        /// </summary>
        /// <returns></returns>
        public MerchCollection AvailableInventory
        {
            get
            {
                return this.MerchRecord.AvailableInventory;
            }
        }
    }

    public partial class MerchBundleItemCollection : Utils._Collection.IOrderable<MerchBundleItem>
    {   
        /// <summary>
        /// Adds a MerchBundleItem to the collection
        /// </summary>
        /// <param name="bundle"></param>
        /// <param name="merchId">The item to  be awarded</param>
        /// <param name="displayText"></param>
        /// <returns></returns>
        public MerchBundleItem AddToCollection(MerchBundle bundle, int merchId)
        {
            System.Collections.Generic.List<System.Web.UI.Pair> args = new System.Collections.Generic.List<System.Web.UI.Pair>();
            args.Add(new System.Web.UI.Pair("DtStamp", DateTime.Now));
            args.Add(new System.Web.UI.Pair("IsActive", true));
            args.Add(new System.Web.UI.Pair("TMerchBundleId", bundle.Id));
            args.Add(new System.Web.UI.Pair("TMerchId", merchId));

            MerchBundleItem newItem = AddToCollection(args);

            return newItem;
        }

        public MerchBundleItem AddToCollection(System.Collections.Generic.List<System.Web.UI.Pair> args)
        {
            return Utils._Collection.AddItemToOrderedCollection(this.GetList(), args);
        }

        /// <summary>
        /// Delete a MerchBundleItem from the collection by ID
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        public bool DeleteFromCollection(int idx)
        {
            return Utils._Collection.DeleteFromOrderedCollection(this.GetList(), idx);
        }

        /// <summary>
        /// Reorder a MerchBundleItem by ID and direction
        /// </summary>
        /// <param name="idx"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public MerchBundleItem ReorderItem(int idx, string direction)
        {
            return Utils._Collection.ReorderOrderedCollection(this.GetList(), idx, direction);
        }
    }
}
