using System;
using System.Xml.Serialization;

namespace Wcss
{
    public partial class PostPurchaseTextCollection : Utils._Collection.IOrderable<PostPurchaseText>
    {
        /// <summary>
        /// Add a PostPurchaseText to the collection
        /// </summary>
        /// <param name="showId"></param>
        /// <param name="promoterId"></param>
        /// <returns></returns>
        public PostPurchaseText AddPostPurchaseTextToCollection(ShowTicket st, string inProcess, string postText)
        {
            System.Collections.Generic.List<System.Web.UI.Pair> args = new System.Collections.Generic.List<System.Web.UI.Pair>();
            args.Add(new System.Web.UI.Pair("DtStamp", DateTime.Now));
            args.Add(new System.Web.UI.Pair("TShowTicketId", st.Id));
            args.Add(new System.Web.UI.Pair("BActive", false));
            args.Add(new System.Web.UI.Pair("InProcessDescription", inProcess));
            args.Add(new System.Web.UI.Pair("PostText", postText));

            return AddToCollection(args);
        }
        public PostPurchaseText AddPostPurchaseTextToCollection(Merch m, string inProcess, string postText)
        {
            System.Collections.Generic.List<System.Web.UI.Pair> args = new System.Collections.Generic.List<System.Web.UI.Pair>();
            args.Add(new System.Web.UI.Pair("DtStamp", DateTime.Now));
            args.Add(new System.Web.UI.Pair("TMerchId", m.Id));
            args.Add(new System.Web.UI.Pair("BActive", false));
            args.Add(new System.Web.UI.Pair("InProcessDescription", inProcess));
            args.Add(new System.Web.UI.Pair("PostText", postText));

            return AddToCollection(args);
        }
        public PostPurchaseText AddToCollection(System.Collections.Generic.List<System.Web.UI.Pair> args)
        {
            return Utils._Collection.AddItemToOrderedCollection(this.GetList(), args);
        }

        /// <summary>
        /// Delete a PostPurchaseText from the collection by ID
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        public bool DeleteFromCollection(int idx)
        {
            return Utils._Collection.DeleteFromOrderedCollection(this.GetList(), idx);
        }

        /// <summary>
        /// Reorder a PostPurchaseText by ID and direction
        /// </summary>
        /// <param name="idx"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public PostPurchaseText ReorderItem(int idx, string direction)
        {
            return Utils._Collection.ReorderOrderedCollection(this.GetList(), idx, direction);
        }
    }

    public partial class PostPurchaseText
    {
        [XmlAttribute("DisplayOrder")]
        public int DisplayOrder
        {
            get { return this.IDisplayOrder; }
            set { this.IDisplayOrder = value; }
        }

        [XmlAttribute("IsActive")]
        public bool IsActive
        {
            get { return this.BActive; }
            set { this.BActive = value; }
        }

        public string PostTextProcessed(Invoice inv, string userName, int itemIdx)
        {
            return this.PostText.Trim().Replace("INVOICEID", inv.UniqueId).Replace("USERNAME", userName).Replace("ITEMID", itemIdx.ToString());
        }
        public string PostTextProcessedForWeb(Invoice inv, string userName, int itemIdx)
        {
            return System.Web.HttpUtility.HtmlEncode(this.PostTextProcessed(inv, userName, itemIdx));
        }
    }
}
