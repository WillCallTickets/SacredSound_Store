using System;
using System.Xml.Serialization;

namespace Wcss
{
    /// <summary>
    /// Show Link - handles 2 types of links.
    /// 1) Links to remote urls. Display text is shown as text for anchor to an absolute url.
    /// 2) Links to other Shows on this site. Indicated by the linkUrl being an integer
    /// ShowLink and RemoteLink collections will be sorted in order in context
    /// </summary>
    public partial class ShowLink
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
        public string ShowLink_Header
        {
            get
            {
                return _Config._ShowLinks_Header;
            }
        }
        public bool IsShowLink
        {
            get
            {
                return Utils.Validation.IsInteger(this.LinkUrl);
            }
        }
        public bool IsRemoteLink
        {
            get
            {
                return (!IsShowLink);
            }
        }
        public string LinkUrl_BaseLink
        {
            get
            {
                return (IsShowLink) ? string.Format("/Store/ChooseTicket.aspx?sid={0}", this.LinkUrl) :
                    Utils.ParseHelper.FormatUrlFromString(this.LinkUrl);
            }
        }
        public string LinkUrl_Formatted(bool useBlankTarget)
        {   
            string target = (useBlankTarget) ? string.Format(" target=\"_blank\"") : string.Empty;//add a space in front

            return string.Format("<a href=\"{0}\"{1}>{2}</a>", this.LinkUrl_BaseLink, target, DisplayText);
        }
    }

    public partial class ShowLinkCollection : Utils._Collection.IOrderable<ShowLink>
    {
        public ShowLinkCollection RemoteLinks_Active
        {
            get
            {
                ShowLinkCollection coll = new ShowLinkCollection();
                coll.AddRange(this.GetList().FindAll(delegate(ShowLink match) { return (match.IsRemoteLink && match.IsActive); }));
                if (coll.Count > 1)
                    coll.Sort("IDisplayOrder", true);

                return coll;
            }
        }
        public ShowLinkCollection ShowLinks_Active
        {
            get
            {
                ShowLinkCollection coll = new ShowLinkCollection();
                coll.AddRange(this.GetList().FindAll(delegate(ShowLink match) { return (match.IsShowLink && match.IsActive); }));
                if (coll.Count > 1)
                    coll.Sort("IDisplayOrder", true);

                return coll;
            }
        }

        
        /// <summary>
        /// Adds a ShowLink to the collection
        /// </summary>
        /// <param name="showId"></param>
        /// <param name="linkUrl"></param>
        /// <param name="displayText"></param>
        /// <returns></returns>
        public ShowLink AddToCollection(int showId, string linkUrl, string displayText)
        {
            ShowLink existing = this.GetList().Find(delegate(ShowLink match) { return (match.LinkUrl == linkUrl); });
            if (existing != null)
                throw new Exception("This link already exists in the collection.");

            System.Collections.Generic.List<System.Web.UI.Pair> args = new System.Collections.Generic.List<System.Web.UI.Pair>();
            args.Add(new System.Web.UI.Pair("DtStamp", DateTime.Now));
            args.Add(new System.Web.UI.Pair("IsActive", true));
            args.Add(new System.Web.UI.Pair("TShowId", showId));
            args.Add(new System.Web.UI.Pair("LinkUrl", linkUrl));
            args.Add(new System.Web.UI.Pair("DisplayText", displayText));

            ShowLink newItem = AddToCollection(args);

            if (newItem != null && newItem.IsShowLink)
            {
                ShowLink linkItem = null;
                Show linkedShow = Show.FetchByID(int.Parse(linkUrl));

                if (linkedShow != null)
                {
                    System.Collections.Generic.List<System.Web.UI.Pair> linkArgs = new System.Collections.Generic.List<System.Web.UI.Pair>();
                    linkArgs.Add(new System.Web.UI.Pair("DtStamp", DateTime.Now));
                    linkArgs.Add(new System.Web.UI.Pair("IsActive", true));
                    linkArgs.Add(new System.Web.UI.Pair("TShowId", linkedShow.Id));
                    linkArgs.Add(new System.Web.UI.Pair("LinkUrl", showId.ToString()));
                    linkArgs.Add(new System.Web.UI.Pair("DisplayText", newItem.ShowRecord.Name_WithLocation));

                    linkItem = linkedShow.ShowLinkRecords().AddToCollection(linkArgs);
                }
            }

            return newItem;
        }

        public ShowLink AddToCollection(System.Collections.Generic.List<System.Web.UI.Pair> args)
        {
            return Utils._Collection.AddItemToOrderedCollection(this.GetList(), args);
        }

        /// <summary>
        /// Delete a ShowLink from the collection by ID
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        public bool DeleteFromCollection(int idx)
        {
            ShowLink entity = (ShowLink)this.Find(idx);

            //try to remove the corresponding show's entry as well
            if (entity != null && entity.IsShowLink)
            {
                Show linkedShow = Show.FetchByID(int.Parse(entity.LinkUrl));
                if (linkedShow != null)
                {
                    //remove the entry where the linkUrl matches this entity's TShowId
                    ShowLink link = linkedShow.ShowLinkRecords().GetList().Find(delegate(ShowLink match) { return (match.LinkUrl == entity.TShowId.ToString()); });
                    if (link != null)
                    {
                        int linkOrder = link.DisplayOrder;

                        ShowLink.Delete(link.Id);

                        foreach (ShowLink ent in linkedShow.ShowLinkRecords())
                        {
                            if (ent.DisplayOrder > linkOrder)
                                ent.DisplayOrder -= 1;
                        }

                        linkedShow.ShowLinkRecords().Remove(link);
                        linkedShow.ShowLinkRecords().SaveAll();
                    }
                }
            }

            return Utils._Collection.DeleteFromOrderedCollection(this.GetList(), idx);
        }

        /// <summary>
        /// Reorder a ShowLink by ID and direction
        /// </summary>
        /// <param name="idx"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public ShowLink ReorderItem(int idx, string direction)
        {
            return Utils._Collection.ReorderOrderedCollection(this.GetList(), idx, direction);
        }
    }
}
