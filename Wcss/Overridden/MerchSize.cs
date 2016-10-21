using System;
using System.Xml.Serialization;

namespace Wcss
{
    public partial class MerchSize
    {
        [XmlAttribute("DisplayOrder")]
        public int DisplayOrder
        {
            get { return this.IDisplayOrder; }
            set { this.IDisplayOrder = value; }
        }
    }

    public partial class MerchSizeCollection : Utils._Collection.IOrderable<MerchSize>
    {
        /// <summary>
        /// Adds a MerchSize to the collection
        /// </summary>
        /// <param name="name"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public MerchSize AddToCollection(string name, string code)
        {
            System.Collections.Generic.List<System.Web.UI.Pair> args = new System.Collections.Generic.List<System.Web.UI.Pair>();
            args.Add(new System.Web.UI.Pair("DtStamp", DateTime.Now));
            args.Add(new System.Web.UI.Pair("ApplicationId", _Config.APPLICATION_ID));
            args.Add(new System.Web.UI.Pair("Name", name));
            args.Add(new System.Web.UI.Pair("Code", code));

            return AddToCollection(args);
        }

        public MerchSize AddToCollection(System.Collections.Generic.List<System.Web.UI.Pair> args)
        {
            return Utils._Collection.AddItemToOrderedCollection(this.GetList(), args);
        }

        /// <summary>
        /// Delete a MerchSize from the collection by ID
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        public bool DeleteFromCollection(int idx)
        {
            return Utils._Collection.DeleteFromOrderedCollection(this.GetList(), idx);
        }

        /// <summary>
        /// Reorder a MerchSize by ID and direction
        /// </summary>
        /// <param name="idx"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public MerchSize ReorderItem(int idx, string direction)
        {
            return Utils._Collection.ReorderOrderedCollection(this.GetList(), idx, direction);
        }
    }
}
