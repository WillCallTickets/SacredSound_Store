using System;
using System.Xml.Serialization;

namespace Wcss
{
    public partial class Promoter
    {
        [XmlAttribute("ListInDirectory")]
        public bool ListInDirectory
        {
            get { return this.BListInDirectory; }
            set { this.BListInDirectory = value; }
        }
        [XmlAttribute("Name_Derived")]
        public string Name_Derived
        {
            get { return this.Name.ToUpper(); }
            set
            {
                this.Name = value.Trim().ToUpper();
                if (value.Trim().ToLower().StartsWith("the "))
                    this.NameRoot = value.Substring(4).ToUpper();
                else
                    this.NameRoot = value.Trim().ToUpper();
            }
        }
        [XmlAttribute("NameRoot_Derived")]
        public string NameRoot_Derived
        {
            get { return (this.Name.StartsWith("THE ")) ? this.Name.Substring(4) : this.Name; }
        }

        public string Website_Configured { get { return Utils.ParseHelper.FormatUrlFromString(this.Website); } }

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
                    string.Format("{0}{1}", _ImageManager._PromoterImageStorage_Local, this.PictureUrl) : string.Empty;
            }
        }
        public string Url_Original { get { return this.ImageManager.OriginalUrl; } }
        public string Thumbnail_Small { get { return this.ImageManager.Thumbnail_Small; } }
        public string Thumbnail_Large { get { return this.ImageManager.Thumbnail_Large; } }
        public string Thumbnail_Max { get { return this.ImageManager.Thumbnail_Max; } }

        #endregion
    }
}
