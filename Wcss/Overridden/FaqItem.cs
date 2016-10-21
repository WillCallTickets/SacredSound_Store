using System;
using System.Xml.Serialization;

namespace Wcss
{
    public partial class FaqItem
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

    }

    public partial class FaqItemCollection : Utils._Collection.IOrderable<FaqItem>
    {
        /// <summary>
        /// Adds an FaqItem to the collection
        /// </summary>
        /// <param name="faqCategorieId"></param>
        /// <param name="question"></param>
        /// <param name="answer"></param>
        /// <returns></returns>
        public FaqItem AddToCollection(int faqCategorieId, string question, string answer)
        {
            System.Collections.Generic.List<System.Web.UI.Pair> args = new System.Collections.Generic.List<System.Web.UI.Pair>();
            args.Add(new System.Web.UI.Pair("DtStamp", DateTime.Now));
            args.Add(new System.Web.UI.Pair("TFaqCategorieId", faqCategorieId)); //args.Add(new System.Web.UI.Pair("EmailAddress", email)); //newItem.TFaqCategorieId = faqCategorieId;
            args.Add(new System.Web.UI.Pair("IsActive", false)); //newItem.IsActive = false;
            args.Add(new System.Web.UI.Pair("Question", question)); //newItem.Question = question;
            args.Add(new System.Web.UI.Pair("Answer", answer)); //newItem.Answer = answer;

            return AddToCollection(args);
        }

        public FaqItem AddToCollection(System.Collections.Generic.List<System.Web.UI.Pair> args)
        {
            return Utils._Collection.AddItemToOrderedCollection(this.GetList(), args);
        }

        /// <summary>
        /// Delete an FaqItem from the collection by ID
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        public bool DeleteFromCollection(int idx)
        {
            return Utils._Collection.DeleteFromOrderedCollection(this.GetList(), idx);
        }

        /// <summary>
        /// Reorder a FaqItem by ID and direction
        /// </summary>
        /// <param name="idx"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public FaqItem ReorderItem(int idx, string direction)
        {
            return Utils._Collection.ReorderOrderedCollection(this.GetList(), idx, direction);
        }
    }
}
