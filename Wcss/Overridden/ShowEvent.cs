using System;
using System.Xml.Serialization;

namespace Wcss
{
    /// <summary>
    /// This class provides a "bare bones" listing
    /// </summary>
    public partial class ShowEvent
    {
        public enum OwnerTypes
        {
            MailerContent,
            NA
        }
        public enum ParentTypes
        {
            Show = 0,
            Venue,
            NA
        }

        /// <summary>
        /// The relation to the collections owner
        /// </summary>
        [XmlAttribute("OwnerId")]
        public int OwnerId
        {
            get { return this.TOwnerId; }
            set { this.TOwnerId = value; }
        }
        /// <summary>
        /// The relation that corresponds to the type of object that created/or is linked to this object
        /// </summary>
        [XmlAttribute("OwnerType")]
        public OwnerTypes OwnerType
        {
            get { if(this.VcOwnerType == null) return OwnerTypes.NA; return (OwnerTypes)Enum.Parse(typeof(OwnerTypes), this.VcOwnerType, true); }
            set { if(value == OwnerTypes.NA) this.VcOwnerType = null; else this.VcOwnerType = value.ToString(); }
        }
        /// <summary>
        /// The relation that corresponds to the type of object that created/or is linked to this object
        /// </summary>
        [XmlAttribute("ParentId")]
        public int ParentId
        {
            get { return this.TParentId; }
            set { this.TParentId = value; }
        }
        /// <summary>
        /// The relation that corresponds to the type of object that created/or is linked to this object
        /// </summary>
        [XmlAttribute("ParentType")]
        public ParentTypes ParentType
        {
            get { if(this.VcParentType == null) return ParentTypes.NA; return (ParentTypes)Enum.Parse(typeof(ParentTypes), this.VcParentType, true); }
            set { if(value == ParentTypes.NA) this.VcParentType = null; else this.VcParentType = value.ToString(); }
        }
        [XmlAttribute("IsActive")]
        public bool IsActive
        {
            get { return this.BActive; }
            set { this.BActive = value; }
        }  
        [XmlAttribute("Ordinal")]
        public int Ordinal
        {
            get { return this.IOrdinal; }
            set { this.IOrdinal = value; }
        }
    }

    public partial class ShowEventCollection : Utils._Collection.IOrderable<ShowEvent>
    {
        /// <summary>
        /// Adds a ShowEvent to the collection
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="originalFileName"></param>
        /// <param name="fileNameOnly"></param>
        /// <param name="displayText"></param>
        /// <returns></returns>
        public ShowEvent AddToCollection(int ownerId, ShowEvent.OwnerTypes ownertype, int parentId, ShowEvent.ParentTypes parenttype, bool active, 
            string dateString, string status, string showTitle, string promoter, string header, string headliner, string opener, string venue, 
            string times, string ages, string pricing, string url, string imageUrl)
        {
            System.Collections.Generic.List<System.Web.UI.Pair> args = new System.Collections.Generic.List<System.Web.UI.Pair>();
            args.Add(new System.Web.UI.Pair("DtStamp", DateTime.Now));
            args.Add(new System.Web.UI.Pair("TOwnerId", ownerId));
            args.Add(new System.Web.UI.Pair("VcOwnerType", (ownertype == ShowEvent.OwnerTypes.NA) ? null : ownertype.ToString()));
            args.Add(new System.Web.UI.Pair("TParentId", parentId));
            args.Add(new System.Web.UI.Pair("VcParentType", (parenttype == ShowEvent.ParentTypes.NA) ? null : parenttype.ToString()));
            args.Add(new System.Web.UI.Pair("BActive", active));
            args.Add(new System.Web.UI.Pair("DateString", dateString));
            args.Add(new System.Web.UI.Pair("Status", status));
            args.Add(new System.Web.UI.Pair("ShowTitle", showTitle));
            args.Add(new System.Web.UI.Pair("Promoter", promoter));
            args.Add(new System.Web.UI.Pair("Header", header));
            args.Add(new System.Web.UI.Pair("Headliner", headliner));
            args.Add(new System.Web.UI.Pair("Opener", opener));
            args.Add(new System.Web.UI.Pair("Venue", venue));
            args.Add(new System.Web.UI.Pair("Times", times));
            args.Add(new System.Web.UI.Pair("Ages", ages));
            args.Add(new System.Web.UI.Pair("Pricing", pricing));
            args.Add(new System.Web.UI.Pair("Url", url));
            args.Add(new System.Web.UI.Pair("ImageUrl", imageUrl));

            return AddToCollection(args);
        }

        public ShowEvent AddToCollection(System.Collections.Generic.List<System.Web.UI.Pair> args)
        {
            return Utils._Collection.AddItemToOrderedCollection(this.GetList(), args, "IOrdinal", false);
        }

        /// <summary>
        /// Delete an item from the collection by ID
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        public bool DeleteFromCollection(int idx)
        {
            return Utils._Collection.DeleteFromOrderedCollection(this.GetList(), idx, "IOrdinal");
        }

        /// <summary>
        /// Reorder a Event by ID and direction
        /// </summary>
        /// <param name="idx"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public ShowEvent ReorderItem(int idx, string direction)
        {
            return Utils._Collection.ReorderOrderedCollection(this.GetList(), idx, direction, "IOrdinal");
        }
    }
}
