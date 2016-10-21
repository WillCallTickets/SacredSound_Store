using System;
using System.Xml.Serialization;

namespace Wcss
{
    public partial class CharitableListing
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
            get { return this.BTopBilling; }
            set { this.BTopBilling = value; }
        }
        [XmlAttribute("TopBilling_Effective")]
        public bool TopBilling_Effective
        {
            get { return (DisplayOrder == 0) ? true : this.TopBilling; }
        }
        [XmlAttribute("IsAvailableForContribution")]
        public bool IsAvailableForContribution
        {
            get { return this.BAvailableForContribution; }
            set { this.BAvailableForContribution = value; }
        }
        [XmlAttribute("OrgName_Displayable")]
        public string OrgName_Displayable
        {
            get
            {
                return this.CharitableOrgRecord.Name_Displayable;
            }
        }
    }
    public partial class CharitableContribution
    {
        [XmlAttribute("Amount")]
        public decimal Amount
        {
            get
            {
                return this.InvoiceItemRecord.LineItemTotal;
            }
        }
        [XmlAttribute("OrgName")]
        public string OrgName
        {
            get
            {
                return this.InvoiceItemRecord.MainActName;
            }
        }
    }
    public partial class CharitableOrg
    {
        [XmlAttribute("IsActive")]
        public bool IsActive
        {
            get { return this.BActive; }
            set { this.BActive = value; }
        }        
        [XmlAttribute("Name_Displayable")]
        public string Name_Displayable
        {
            get
            {
                return (this.DisplayName == null || this.DisplayName.Trim().Length == 0) ? this.Name.ToUpper() : this.DisplayName;
            }
        }
        [XmlAttribute("DisplayNameWithAttributes")]
        public string DisplayNameWithAttributes
        {
            get
            {
                return Name_Displayable;
            }
        }

        public void Delete()
        {
            this.ImageManager.Delete();

            CharitableOrg.Delete(this.Id);
        }

        #region Image Mgmt

        private _ImageManager _imageManager = null;
        public _ImageManager ImageManager
        {
            get
            {
                if (_imageManager == null || ((this.PictureUrl != null && this.PictureUrl.Trim().Length > 0) &&
                    (_imageManager != null && _imageManager.OriginalUrl.Trim().Length == 0)))
                    _imageManager = new _ImageManager(this.path_original);

                return _imageManager;
            }
        }
        private string path_original
        {
            get
            {
                return (this.PictureUrl != null && this.PictureUrl.Trim().Length > 0) ?
                    string.Format("{0}{1}", _ImageManager._CharityImageStorage_Local, this.PictureUrl) : string.Empty;
            }
        }
        public string Url_Original { get { return this.ImageManager.OriginalUrl; } }
        public string Thumbnail_Small { get { return this.ImageManager.Thumbnail_Small; } }
        public string Thumbnail_Large { get { return this.ImageManager.Thumbnail_Large; } }
        public string Thumbnail_Max { get { return this.ImageManager.Thumbnail_Max; } }

        #endregion
    }
    

    public partial class CharitableListingCollection : Utils._Collection.IOrderable<CharitableListing>
    {
        /// <summary>
        /// Adds a CharitableListing to the collection
        /// </summary>
        /// <param name="charitableOrgId"></param>
        /// <param name="availableForContribution"></param>
        /// <param name="TopBilling"></param>
        /// <returns></returns>
        public CharitableListing AddToCollection(int charitableOrgId, bool availableForContribution, bool TopBilling)
        {
            //do not allow adding an exiting item!!!
            if (this.GetList().FindIndex(delegate(CharitableListing match) { return (match.TCharitableOrgId == charitableOrgId); }) != -1)
                throw new Exception("The item you are trying to add is already in the collection. Please edit that entry.");

            System.Collections.Generic.List<System.Web.UI.Pair> args = new System.Collections.Generic.List<System.Web.UI.Pair>();
            args.Add(new System.Web.UI.Pair("DtStamp", DateTime.Now));
            args.Add(new System.Web.UI.Pair("ApplicationId", _Config.APPLICATION_ID));//newItem.ApplicationId = _Config.APPLICATION_ID;
            args.Add(new System.Web.UI.Pair("TCharitableOrgId", charitableOrgId));//newItem.TCharitableOrgId = charitableOrgId;
            args.Add(new System.Web.UI.Pair("IsAvailableForContribution", availableForContribution));//newItem.IsAvailableForContribution = IsAvailableForContribution;
            args.Add(new System.Web.UI.Pair("TopBilling", TopBilling));//newItem.TopBilling = TopBilling;

            return AddToCollection(args);
        }

        public CharitableListing AddToCollection(System.Collections.Generic.List<System.Web.UI.Pair> args)
        {
            return Utils._Collection.AddItemToOrderedCollection(this.GetList(), args);
        }

        /// <summary>
        /// Delete a CharitableListing from the collection by ID
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        public bool DeleteFromCollection(int idx)
        {
            return Utils._Collection.DeleteFromOrderedCollection(this.GetList(), idx);
        }

        /// <summary>
        /// Reorder a CharitableListing by ID and direction
        /// </summary>
        /// <param name="idx"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public CharitableListing ReorderItem(int idx, string direction)
        {
            return Utils._Collection.ReorderOrderedCollection(this.GetList(), idx, direction);
        }
    }
}
