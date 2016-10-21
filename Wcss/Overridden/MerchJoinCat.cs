using System;
using System.Xml.Serialization;
using SubSonic;

namespace Wcss
{
    public partial class MerchJoinCat
    {
        [XmlAttribute("MerchCategorieName")]
        public string MerchCategorieName
        {
            get { return this.MerchCategorieRecord.Name; }
        }
        [XmlAttribute("MerchRecordName")]
        public string MerchRecordName
        {
            get { return this.MerchRecord.Name; }
        }
        [XmlAttribute("DisplayOrder")]
        public int DisplayOrder
        {
            get { return this.IDisplayOrder; }
            set { this.IDisplayOrder = value; }
        }
    }

    public partial class MerchJoinCatCollection : Utils._Collectionizer.IOrderable<MerchJoinCat>
    {
        /// <summary>
        /// Adds a JShowActItem to the collection
        /// </summary>
        /// <param name="showDateId"></param>
        /// <param name="actId"></param>
        /// <returns></returns>
        public MerchJoinCat AddMerchToCollection(int merchCategorieId, int merchId)
        {
            System.Collections.Generic.List<System.Web.UI.Pair> args = new System.Collections.Generic.List<System.Web.UI.Pair>();
            args.Add(new System.Web.UI.Pair("DtStamp", DateTime.Now));
            args.Add(new System.Web.UI.Pair("TMerchId", merchId));
            args.Add(new System.Web.UI.Pair("TMerchCategorieId", merchCategorieId));
            
            MerchJoinCat added = AddToCollection(args);

            return added;
        }

        public MerchJoinCat AddToCollection(System.Collections.Generic.List<System.Web.UI.Pair> args)
        {
            return Utils._Collectionizer.AddItemToOrderedCollection(this.GetList(), args, true);
        }

        /// <summary>
        /// Delete a MerchJoinCat from the collection by ID
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        public bool DeleteFromCollection(int idx)
        {
            return Utils._Collectionizer.DeleteFromOrderedCollection(this.GetList(), idx);
        }

        /// <summary>
        /// Reorder a MerchJoinCat by ID and direction
        /// </summary>
        /// <param name="idx"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public MerchJoinCat ReorderItem(int idx, string direction)
        {
            return Utils._Collectionizer.ReorderOrderedCollection(this.GetList(), idx, direction);
        }

        public MerchJoinCat InsertAtOrdinalInExistingCollection(int idx, int newOrdinal)
        {
            return Utils._Collectionizer.InsertAtOrdinalInExistingCollection(this.GetList(), idx, newOrdinal);
        }
    }
}
