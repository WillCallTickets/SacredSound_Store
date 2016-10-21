using System;
using System.Xml.Serialization;

namespace Wcss
{
    public partial class MerchCategorie
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
    }

    public partial class MerchCategorieCollection : Utils._Collectionizer.IOrderable<MerchCategorie>
    {
        /// <summary>
        /// Adds a MerchCategorie to the collection
        /// </summary>
        /// <param name="name"></param>
        /// <param name="merchDivisionId"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public MerchCategorie AddToCollection(string name, int merchDivisionId, string description)
        {
            System.Collections.Generic.List<System.Web.UI.Pair> args = new System.Collections.Generic.List<System.Web.UI.Pair>();
            args.Add(new System.Web.UI.Pair("DtStamp", DateTime.Now));
            args.Add(new System.Web.UI.Pair("Name", name));
            args.Add(new System.Web.UI.Pair("IsInternalOnly", false));
            args.Add(new System.Web.UI.Pair("TMerchDivisionId", merchDivisionId));
            args.Add(new System.Web.UI.Pair("Description", description));

            return AddToCollection(args);
        }

        public MerchCategorie AddToCollection(System.Collections.Generic.List<System.Web.UI.Pair> args)
        {
            return Utils._Collectionizer.AddItemToOrderedCollection(this.GetList(), args);
        }

        /// <summary>
        /// Delete a MerchCategorie from the collection by ID
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        public bool DeleteFromCollection(int idx)
        {
            return Utils._Collectionizer.DeleteFromOrderedCollection(this.GetList(), idx);
        }

        /// <summary>
        /// Reorder a MerchCategorie by ID and direction
        /// </summary>
        /// <param name="idx"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public MerchCategorie ReorderItem(int idx, string direction)
        {
            return Utils._Collectionizer.ReorderOrderedCollection(this.GetList(), idx, direction);
        }

        public MerchCategorie InsertAtOrdinalInExistingCollection(int idx, int newOrdinal)
        {
            return Utils._Collectionizer.InsertAtOrdinalInExistingCollection(this.GetList(), idx, newOrdinal);
        }
    }
}
