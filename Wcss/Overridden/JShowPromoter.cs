using System;
using System.Xml.Serialization;

namespace Wcss
{
    public partial class JShowPromoterCollection : Utils._Collection.IOrderable<JShowPromoter>
    {
        /// <summary>
        /// Add a JShowPromoter to the collection
        /// </summary>
        /// <param name="showId"></param>
        /// <param name="promoterId"></param>
        /// <returns></returns>
        public JShowPromoter AddPromoterToCollection(int showId, int promoterId)
        {
            System.Collections.Generic.List<System.Web.UI.Pair> args = new System.Collections.Generic.List<System.Web.UI.Pair>();
            args.Add(new System.Web.UI.Pair("DtStamp", DateTime.Now));
            args.Add(new System.Web.UI.Pair("TShowId", showId));
            args.Add(new System.Web.UI.Pair("TPromoterId", promoterId));

            return AddToCollection(args);
        }

        public JShowPromoter AddToCollection(System.Collections.Generic.List<System.Web.UI.Pair> args)
        {
            return Utils._Collection.AddItemToOrderedCollection(this.GetList(), args);
        }

        /// <summary>
        /// Delete a JShowPromoter from the collection by ID
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        public bool DeleteFromCollection(int idx)
        {
            return Utils._Collection.DeleteFromOrderedCollection(this.GetList(), idx);
        }

        /// <summary>
        /// Reorder a JShowPromoter by ID and direction
        /// </summary>
        /// <param name="idx"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public JShowPromoter ReorderItem(int idx, string direction)
        {
            return Utils._Collection.ReorderOrderedCollection(this.GetList(), idx, direction);
        }
    }

    public partial class JShowPromoter
    {
        [XmlAttribute("DisplayOrder")]
        public int DisplayOrder
        {
            get { return this.IDisplayOrder; }
            set { this.IDisplayOrder = value; }
        }

        [XmlAttribute("PromoterName")]
        public string PromoterName
        {
            get 
            {
                if (this.PromoterRecord != null)
                    return PromoterRecord.Name;

                return string.Empty;
            }
        }
    }
}
