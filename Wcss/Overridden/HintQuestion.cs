using System;
using System.Xml.Serialization;

namespace Wcss
{
    public partial class HintQuestion
    {
        [XmlAttribute("DisplayOrder")]
        public int DisplayOrder
        {
            get { return this.IDisplayOrder; }
            set { this.IDisplayOrder = value; }
        }
    }

    public partial class HintQuestionCollection : Utils._Collection.IOrderable<HintQuestion>
    {
        /// <summary>
        /// Adds a HintQuestion to the collection
        /// </summary>
        /// <param name="questionText"></param>
        /// <returns></returns>
        public HintQuestion AddToCollection(string questionText)
        {
            System.Collections.Generic.List<System.Web.UI.Pair> args = new System.Collections.Generic.List<System.Web.UI.Pair>();
            args.Add(new System.Web.UI.Pair("DtStamp", DateTime.Now));
            args.Add(new System.Web.UI.Pair("ApplicationId", _Config.APPLICATION_ID));
            args.Add(new System.Web.UI.Pair("Text", questionText));

            return AddToCollection(args);
        }

        public HintQuestion AddToCollection(System.Collections.Generic.List<System.Web.UI.Pair> args)
        {
            return Utils._Collection.AddItemToOrderedCollection(this.GetList(), args);
        }

        /// <summary>
        /// Delete an HintQuestion from the collection by ID
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        public bool DeleteFromCollection(int idx)
        {
            return Utils._Collection.DeleteFromOrderedCollection(this.GetList(), idx);
        }

        /// <summary>
        /// Reorder a HintQuestion by ID and direction
        /// </summary>
        /// <param name="idx"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public HintQuestion ReorderItem(int idx, string direction)
        {
            return Utils._Collection.ReorderOrderedCollection(this.GetList(), idx, direction);
        }
    }
}
