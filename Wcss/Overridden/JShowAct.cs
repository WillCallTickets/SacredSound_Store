using System;
using System.Xml.Serialization;

namespace Wcss
{
    public partial class JShowAct
    {
        [XmlAttribute("DisplayOrder")]
        public int DisplayOrder
        {
            get { return this.IDisplayOrder; }
            set { this.IDisplayOrder = value; }
        }
        [XmlAttribute("TopBilling")]
        public bool TopBilling
        {
            get 
            {
                return (!this.BTopBilling.HasValue) ? (false) : this.BTopBilling.Value; 
            }
            set { this.BTopBilling = value; }
        }
        [XmlAttribute("TopBilling_Effective")]
        public bool TopBilling_Effective
        {
            get
            {
                return (DisplayOrder == 0) ? true : this.TopBilling;
            }
        }
        [XmlAttribute("DisplayNameWithAttributes")]
        public string DisplayNameWithAttributes
        {
            get
            {
                return string.Format("{0} {1} {2} {3}", this.PreText ?? string.Empty, this.ActRecord.Name, this.ActText ?? string.Empty, this.PostText ?? string.Empty).Trim();
            }
        }
    }

    public partial class JShowActCollection : Utils._Collection.IOrderable<JShowAct>
    {
        /// <summary>
        /// Adds a JShowActItem to the collection
        /// </summary>
        /// <param name="showDateId"></param>
        /// <param name="actId"></param>
        /// <returns></returns>
        public JShowAct AddActToCollection(ShowDate showDate, int actId, string actName, string userName, Guid userId)
        {
            System.Collections.Generic.List<System.Web.UI.Pair> args = new System.Collections.Generic.List<System.Web.UI.Pair>();
            args.Add(new System.Web.UI.Pair("DtStamp", DateTime.Now));
            args.Add(new System.Web.UI.Pair("TShowDateId", showDate.Id));
            args.Add(new System.Web.UI.Pair("TActId", actId));
            args.Add(new System.Web.UI.Pair("TopBilling", false));

            JShowAct added = AddToCollection(args);

            if(actName.Length > 1500)
                actName = actName.Substring(0,1499);

            EventQ.LogEvent(DateTime.Now, DateTime.Now, _Enums.EventQStatus.Success, userName, userId, null, _Enums.EventQContext.ShowDate, _Enums.EventQVerb._ActAdded, 
                    actName, string.Format("ShowDate: {0} - {1}", showDate.DateOfShow.ToString(), showDate.Id.ToString()), showDate.ShowRecord.Name);

            return added;
        }

        public JShowAct AddToCollection(System.Collections.Generic.List<System.Web.UI.Pair> args)
        {
            return Utils._Collection.AddItemToOrderedCollection(this.GetList(), args);
        }

        /// <summary>
        /// Delete a JShowAct from the collection by ID
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        public bool DeleteFromCollection(int idx)
        {
            return Utils._Collection.DeleteFromOrderedCollection(this.GetList(), idx);
        }

        /// <summary>
        /// Reorder a JShowAct by ID and direction
        /// </summary>
        /// <param name="idx"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public JShowAct ReorderItem(int idx, string direction)
        {
            return Utils._Collection.ReorderOrderedCollection(this.GetList(), idx, direction);
        }
    }
}
