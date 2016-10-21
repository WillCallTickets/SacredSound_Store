using System;
using System.Xml.Serialization;

using Utils.ExtensionMethods;

namespace Wcss
{
    /// <summary>
    /// note the cascade update in db
    /// </summary>
    public partial class FaqCategorie
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
        [XmlAttribute("Display_Preferred")]
        public string Display_Preferred
        {
            get
            {
                if (this.DisplayText.HasValueLength())
                    return this.DisplayText.Trim();

                return this.Name;
            }
        }
    }

    public partial class FaqCategorieCollection : Utils._Collection.IOrderable<FaqCategorie>
    {
        /// <summary>
        /// Adds an FaqCategorie to the collection
        /// </summary>
        /// <param name="newCategorieName"></param>
        /// <param name="displayText"></param>
        /// <returns></returns>
        public FaqCategorie AddToCollection(string newCategorieName, string displayText)
        {
            System.Collections.Generic.List<System.Web.UI.Pair> args = new System.Collections.Generic.List<System.Web.UI.Pair>();
            args.Add(new System.Web.UI.Pair("DtStamp", DateTime.Now));
            args.Add(new System.Web.UI.Pair("ApplicationId", _Config.APPLICATION_ID));
            args.Add(new System.Web.UI.Pair("IsActive", false));
            args.Add(new System.Web.UI.Pair("Name", newCategorieName));//newItem.FirstName = firstName;
            args.Add(new System.Web.UI.Pair("DisplayText", displayText));//newItem.LastName = lastName;

            return AddToCollection(args);
        }

        public FaqCategorie AddToCollection(System.Collections.Generic.List<System.Web.UI.Pair> args)
        {
            return Utils._Collection.AddItemToOrderedCollection(this.GetList(), args);
        }

        /// <summary>
        /// Delete an FaqCategorie from the collection by ID
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        public bool DeleteFromCollection(int idx)
        {
            return Utils._Collection.DeleteFromOrderedCollection(this.GetList(), idx);
        }

        /// <summary>
        /// Reorder a FaqCategorie by ID and direction
        /// </summary>
        /// <param name="idx"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public FaqCategorie ReorderItem(int idx, string direction)
        {
            return Utils._Collection.ReorderOrderedCollection(this.GetList(), idx, direction);
        }
    }
}
