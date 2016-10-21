using System;
using System.Xml.Serialization;

namespace Wcss
{
    public partial class SaleRuleCollection : Utils._Collection.IOrderable<SaleRule>
    {
        /// <summary>
        /// Adds a SaleRule to the collection
        /// </summary>
        /// <param name="context"></param>
        /// <param name="name"></param>
        /// <param name="displayText"></param>
        /// <returns></returns>
        public SaleRule AddToCollection(string context, string name, string displayText)
        {
            System.Collections.Generic.List<System.Web.UI.Pair> args = new System.Collections.Generic.List<System.Web.UI.Pair>();
            args.Add(new System.Web.UI.Pair("DtStamp", DateTime.Now));
            args.Add(new System.Web.UI.Pair("ApplicationId", _Config.APPLICATION_ID));
            args.Add(new System.Web.UI.Pair("Context", (_Enums.ProductContext)Enum.Parse(typeof(_Enums.ProductContext), context, true)));
            args.Add(new System.Web.UI.Pair("Name", (name.Trim().Length > 0) ? name.Trim() : null));
            args.Add(new System.Web.UI.Pair("DisplayText", displayText));

            return AddToCollection(args);
        }

        public SaleRule AddToCollection(System.Collections.Generic.List<System.Web.UI.Pair> args)
        {
            return Utils._Collection.AddItemToOrderedCollection(this.GetList(), args);
        }

        /// <summary>
        /// Delete a SaleRule from the collection by ID
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        public bool DeleteFromCollection(int idx)
        {
            return Utils._Collection.DeleteFromOrderedCollection(this.GetList(), idx);
        }

        /// <summary>
        /// Reorder a SaleRule by ID and direction
        /// </summary>
        /// <param name="idx"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public SaleRule ReorderItem(int idx, string direction)
        {
            return Utils._Collection.ReorderOrderedCollection(this.GetList(), idx, direction);
        }
    }

    public partial class SaleRule
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
        [XmlAttribute("Context")]
        public _Enums.ProductContext Context
        {
            get { return (_Enums.ProductContext)Enum.Parse(typeof(_Enums.ProductContext), this.VcContext, true); }
            set { this.VcContext = value.ToString(); }
        }
    }
}
