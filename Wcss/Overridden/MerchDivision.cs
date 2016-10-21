using System;
using System.Xml.Serialization;

namespace Wcss
{
    public partial class MerchDivision
    {
        [XmlAttribute("DisplayOrder")]
        public int DisplayOrder
        {
            get { return this.IDisplayOrder; }
            set { this.IDisplayOrder = value; }
        }
        /// <summary>
        /// tells us if this should be publicly displayed
        /// </summary>
        [XmlAttribute("IsInternalOnly")]
        public bool IsInternalOnly
        {
            get { return this.BInternalOnly; }
            set { this.BInternalOnly = value; }
        }
        public bool HasDisplayableItems(MerchCollection coll)
        {
            foreach (Merch merch in coll)
            {
                if (!merch.IsInternalOnly)
                {
                    foreach (MerchJoinCat cat in merch.MerchJoinCatRecords())
                        if ((!cat.MerchCategorieRecord.IsInternalOnly) && cat.MerchCategorieRecord.TMerchDivisionId == this.Id)
                            return true;
                }
            }

            return false;
        }
        public bool HasAvailableItems(MerchCollection coll)
        {
            foreach (Merch merch in coll)
            {
                foreach (MerchJoinCat cat in merch.MerchJoinCatRecords())
                    if (cat.MerchCategorieRecord.TMerchDivisionId == this.Id)
                        return true;
            }

            return false;
        }
    }

    public partial class MerchDivisionCollection : Utils._Collectionizer.IOrderable<MerchDivision>
    {
        /// <summary>
        /// Adds a MerchDivision to the collection
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public MerchDivision AddToCollection(string name, string description)
        {
            System.Collections.Generic.List<System.Web.UI.Pair> args = new System.Collections.Generic.List<System.Web.UI.Pair>();
            args.Add(new System.Web.UI.Pair("DtStamp", DateTime.Now));
            args.Add(new System.Web.UI.Pair("ApplicationId", _Config.APPLICATION_ID));
            args.Add(new System.Web.UI.Pair("Name", name));
            args.Add(new System.Web.UI.Pair("IsInternalOnly", false));
            args.Add(new System.Web.UI.Pair("Description", description));

            return AddToCollection(args);
        }

        public MerchDivision AddToCollection(System.Collections.Generic.List<System.Web.UI.Pair> args)
        {
            return Utils._Collectionizer.AddItemToOrderedCollection(this.GetList(), args);
        }

        /// <summary>
        /// Delete a MerchDivision from the collection by ID
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        public bool DeleteFromCollection(int idx)
        {
            return Utils._Collectionizer.DeleteFromOrderedCollection(this.GetList(), idx);
        }

        /// <summary>
        /// Reorder a MerchDivision by ID and direction
        /// </summary>
        /// <param name="idx"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public MerchDivision ReorderItem(int idx, string direction)
        {
            return Utils._Collectionizer.ReorderOrderedCollection(this.GetList(), idx, direction);
        }

        public MerchDivision InsertAtOrdinalInExistingCollection(int idx, int newOrdinal)
        {
            return Utils._Collectionizer.InsertAtOrdinalInExistingCollection(this.GetList(), idx, newOrdinal);
        }
    }
}
