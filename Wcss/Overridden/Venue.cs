using System;
using System.Xml.Serialization;

namespace Wcss
{
    public partial class Venue
    {
        [XmlAttribute("Capacity")]
        public int Capacity
        {
            get { return (!this.ICapacity.HasValue) ? 0 : this.ICapacity.Value; }
            set { this.ICapacity = value; }
        }

        [XmlAttribute("listInfo")]
        public string listInfo
        {
            get
            {
                return System.Text.RegularExpressions.Regex.Replace(string.Format("{0} {1} {2} {3} {4}", 
                    this.Name, 
                    ShortAddress ?? string.Empty,
                    (BoxOfficePhone != null) ? BoxOfficePhone : string.Empty,
                    (BoxOfficePhoneExt != null && BoxOfficePhoneExt.Trim().Length > 0) ?
                        string.Format("ext: {0}", BoxOfficePhoneExt) : string.Empty,
                    (BoxOfficeNotes != null && BoxOfficeNotes.Trim().Length > 0) ?
                    BoxOfficeNotes.Trim() : string.Empty), @"\s+", " ").Trim();
            }           
        }

        [XmlAttribute("Name_Displayable")]
        public string Name_Displayable
        {
            get
            {
                return (this.DisplayName == null || this.DisplayName.Trim().Length == 0) ? this.Name.ToUpper() : this.DisplayName;
            }
        }

        public string Website_Configured { get { return Utils.ParseHelper.FormatUrlFromString(this.WebsiteUrl); } }

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
                    string.Format("{0}{1}", _ImageManager._VenueImageStorage_Local, this.PictureUrl) : string.Empty;
            }
        }
        public string Url_Original { get { return this.ImageManager.OriginalUrl; } }
        public string Thumbnail_Small { get { return this.ImageManager.Thumbnail_Small; } }
        public string Thumbnail_Large { get { return this.ImageManager.Thumbnail_Large; } }
        public string Thumbnail_Max { get { return this.ImageManager.Thumbnail_Max; } }

        [XmlAttribute("PicWidth")]
        public int PicWidth
        {
            get
            {
                if (this.IPicWidth == 0 && this.PictureUrl != null && this.PictureUrl.Trim().Length > 0)
                {
                    try
                    {
                        System.Web.UI.Pair p = Utils.ImageTools.GetDimensions(System.Web.HttpContext.Current.Server.MapPath(this.ImageManager.OriginalUrl));
                        this.IPicWidth = (int)p.First;
                        this.IPicHeight = (int)p.Second;

                        _ImageManager.UpdatePictureDimensions(this.Id, Wcss.Venue.table.ToString(), this.IPicWidth, this.IPicHeight);
                    }
                    catch (Exception ex)
                    {
                        _Error.LogException(ex);
                    }
                }

                return this.IPicWidth;
            }
            set
            {
                this.IPicWidth = value;
            }
        }
        [XmlAttribute("PicHeight")]
        public int PicHeight
        {
            get
            {
                if (this.IPicHeight == 0 && this.PictureUrl != null && this.PictureUrl.Trim().Length > 0)
                {
                    try
                    {
                        System.Web.UI.Pair p = Utils.ImageTools.GetDimensions(System.Web.HttpContext.Current.Server.MapPath(this.ImageManager.OriginalUrl));
                        this.IPicWidth = (int)p.First;
                        this.IPicHeight = (int)p.Second;

                        _ImageManager.UpdatePictureDimensions(this.Id, Wcss.Venue.table.ToString(), this.IPicWidth, this.IPicHeight);
                    }
                    catch (Exception ex)
                    {
                        _Error.LogException(ex);
                    }
                }

                return this.IPicHeight;
            }
            set
            {
                this.IPicHeight = value;
            }
        }

        #endregion
    }
}
