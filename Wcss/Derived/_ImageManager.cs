using System;
using System.IO;

namespace Wcss
{
    public class _ImageManager
    {
        public static readonly string _MerchImageStorage_Local = string.Format("/{0}/Images/Merchandise/", _Config._VirtualResourceDir);
        public static readonly string _ActImageStorage_Local = string.Format("/{0}/Images/Acts/", _Config._VirtualResourceDir);
        public static readonly string _VenueImageStorage_Local = string.Format("/{0}/Images/Venues/", _Config._VirtualResourceDir);
        public static readonly string _PromoterImageStorage_Local = string.Format("/{0}/Images/Promoters/", _Config._VirtualResourceDir);
        public static readonly string _CharityImageStorage_Local = string.Format("/{0}/Images/Charity/", _Config._VirtualResourceDir);
        public static readonly string _ShowImageStorage_Local = string.Format("/{0}/Images/Shows/", _Config._VirtualResourceDir);

        private string _originalUrl = null;
        public string _OriginalUrl
        {
            get{
                return _originalUrl;
            }
            set{
                _originalUrl = value;
            }
        }
        /// <summary>
        /// Path function creates backslashes - must replace
        /// </summary>
        private string _originalDirectory_Virtual { get { return Path.GetDirectoryName(_OriginalUrl).Replace("\\","/"); } }
        private string _originalFileName { get { return Path.GetFileName(_OriginalUrl); } }

        private _ImageManager() { }
        public _ImageManager(string originalUrl)
        {
            _OriginalUrl = originalUrl;
        }

        public static void UpdatePictureDimensions(int idx, string tableName, int width, int height)
        {
            try
            {
                SPs.TxPictureUpdate(idx, tableName, width, height).Execute();
            }
            catch (Exception ex)
            {
                _Error.LogException(ex);
            }
        }

        public static void EnsureThumbDirectories(string context)
        {
            string ctx = string.Empty;
            if (context.ToLower() == "act")
                ctx = _ImageManager._ActImageStorage_Local;
            else if (context.ToLower() == "venue")
                ctx = _ImageManager._VenueImageStorage_Local;
            else if (context.ToLower() == "show")
                ctx = _ImageManager._ShowImageStorage_Local;
            else if (context.ToLower() == "merch")
                ctx = _ImageManager._MerchImageStorage_Local;
            else if (context.ToLower() == "promoter")
                ctx = _ImageManager._PromoterImageStorage_Local;
            else if (context.ToLower() == "charity")
                ctx = _ImageManager._CharityImageStorage_Local;

            string smallPath = System.Web.HttpContext.Current.Server.MapPath(string.Format("{0}{1}", ctx, _ImageManager.smallThumbPath));
            string largePath = System.Web.HttpContext.Current.Server.MapPath(string.Format("{0}{1}", ctx, _ImageManager.largeThumbPath));
            string maxPath = System.Web.HttpContext.Current.Server.MapPath(string.Format("{0}{1}", ctx, _ImageManager.maxThumbPath));

            if (!Directory.Exists(smallPath))
                Directory.CreateDirectory(smallPath);
            if (!Directory.Exists(largePath))
                Directory.CreateDirectory(largePath);
            if (!Directory.Exists(maxPath))
                Directory.CreateDirectory(maxPath);
        }


        public static string smallThumbPath = "/thumbsm/";
        public static string largeThumbPath = "/thumblg/";
        public static string maxThumbPath = "/thumbmx/";

        /// <summary>
        /// be sure to reset pictureUrl or DiplayUrl as well as pic height and width on parent
        /// </summary>
        public void Delete()
        {
            if (_OriginalUrl != null && _OriginalUrl.Trim().Length > 0)
            {
                //if we can find the local thumbnail - delete it
                string mapped = System.Web.HttpContext.Current.Server.MapPath(_OriginalUrl);

                if (File.Exists(mapped))
                    File.Delete(mapped);

                DeleteThumbnails();

                _OriginalUrl = null;
            }
        }
        public void DeleteThumbnails()
        {
            DeleteThumbnails_Small();
            DeleteThumbnails_Large();
            DeleteThumbnails_Max();

            DeleteRemoteThumbnails();
        }
        public void DeleteThumbnails_Small()
        {
            if (_OriginalUrl != null && _OriginalUrl.Trim().Length > 0)
            {
                string small = System.Web.HttpContext.Current.Server.MapPath(string.Format("{0}{1}{2}", _originalDirectory_Virtual, smallThumbPath, _originalFileName));
                if (File.Exists(small)) File.Delete(small);
            }
        }
        public void DeleteThumbnails_Large()
        {
            if (_OriginalUrl != null && _OriginalUrl.Trim().Length > 0)
            {
                string large = System.Web.HttpContext.Current.Server.MapPath(string.Format("{0}{1}{2}", _originalDirectory_Virtual, largeThumbPath, _originalFileName));
                if (File.Exists(large)) File.Delete(large);
            }
        }
        public void DeleteThumbnails_Max()
        {
            if (_OriginalUrl != null && _OriginalUrl.Trim().Length > 0)
            {
                string max = System.Web.HttpContext.Current.Server.MapPath(string.Format("{0}{1}{2}", _originalDirectory_Virtual, maxThumbPath, _originalFileName));
                if (File.Exists(max)) File.Delete(max);
            }
        }

        /// <summary>
        /// TODO
        /// </summary>
        internal void DeleteRemoteThumbnails()
        {
            return;
        }


        //small thumbnail - local,remote and effective

        /// <summary>
        /// determines if file exists before handing back a url
        /// </summary>
        public string OriginalUrl
        {
            get
            {   
                //added in http to deal with old images
                if (_OriginalUrl == null || (_OriginalUrl.ToLower().IndexOf("://") != -1) || (!File.Exists(System.Web.HttpContext.Current.Server.MapPath(_OriginalUrl))))
                    return string.Empty;

                string path = Path.GetDirectoryName(_OriginalUrl).Replace("\\", "/");
                //string file = System.Web.HttpUtility.HtmlEncode(Path.GetFileName(_OriginalUrl));
                string file = Path.GetFileName(_OriginalUrl);
                return string.Format("{0}/{1}", path, file);
            }
        }
        public string Thumbnail_Small
        {
            get
            {
                return (_Config._UseRemoteImages) ? Thumbnail_CalculateRemote(_ImageManager.smallThumbPath) :
                    Thumbnail_CalculateLocal(_ImageManager.smallThumbPath);
            }
        }
        public string Thumbnail_Large
        {
            get
            {
                return (_Config._UseRemoteImages) ? Thumbnail_CalculateRemote(_ImageManager.largeThumbPath) :
                    Thumbnail_CalculateLocal(_ImageManager.largeThumbPath);
            }
        }
        public string Thumbnail_Max
        {
            get
            {
                return (_Config._UseRemoteImages) ? Thumbnail_CalculateRemote(_ImageManager.maxThumbPath) :
                    Thumbnail_CalculateLocal(_ImageManager.maxThumbPath);
            }
        }
        public void CreateAllThumbs()
        {
            Thumbnail_CalculateLocal(smallThumbPath);
            Thumbnail_CalculateLocal(largeThumbPath);
            Thumbnail_CalculateLocal(maxThumbPath);
        }
        private string Thumbnail_CalculateLocal(string thumbPath)
        {
            if (_OriginalUrl == null || _OriginalUrl.Trim().Length == 0 || (!File.Exists(System.Web.HttpContext.Current.Server.MapPath(_OriginalUrl))))
                return string.Empty;

            string destPathAndFile = string.Format("{0}{1}{2}", _originalDirectory_Virtual, thumbPath, _originalFileName);
            string mappedDestFilePath = System.Web.HttpContext.Current.Server.MapPath(destPathAndFile);


            //get the context from the path - presuppose merch
            int sm = _Config._MerchThumbSizeSm;
            int lg = _Config._MerchThumbSizeLg;
            int mx = _Config._MerchThumbSizeMax;

            //test for merch
            if (_OriginalUrl.ToLower().IndexOf(_ImageManager._MerchImageStorage_Local.ToLower()) != -1)
            {
                //sm = _Config._ActThumbSizeSm;
                //lg = _Config._ActThumbSizeLg;
                //mx = _Config._ActThumbSizeMax;
            }
            //test for act
            else if (_OriginalUrl.ToLower().IndexOf(_ImageManager._ActImageStorage_Local.ToLower()) != -1)
            {
                sm = _Config._ActThumbSizeSm;
                lg = _Config._ActThumbSizeLg;
                mx = _Config._ActThumbSizeMax;
            }
            //test for show
            else if (_OriginalUrl.ToLower().IndexOf(_ImageManager._ShowImageStorage_Local.ToLower()) != -1)
            {
                sm = _Config._ShowThumbSizeSm;
                lg = _Config._ShowThumbSizeLg;
                mx = _Config._ShowThumbSizeMax;
            }
            //test for venue
            else if (_OriginalUrl.ToLower().IndexOf(_ImageManager._VenueImageStorage_Local.ToLower()) != -1)
            {
                sm = _Config._VenueThumbSizeSm;
                lg = _Config._VenueThumbSizeLg;
                mx = _Config._VenueThumbSizeMax;
            }
            else if (_OriginalUrl.ToLower().IndexOf(_ImageManager._CharityImageStorage_Local.ToLower()) != -1)
            {
                sm = _Config._CharityThumbSizeSm;
                lg = _Config._CharityThumbSizeLg;
                mx = _Config._CharityThumbSizeMax;
            }
            else if (_OriginalUrl.ToLower().IndexOf(_ImageManager._PromoterImageStorage_Local.ToLower()) != -1)
            {
                sm = _Config._PromoterThumbSizeSm;
                lg = _Config._PromoterThumbSizeLg;
                mx = _Config._PromoterThumbSizeMax;
            }

            //ensure file + path exists
            //create thumbnail and save it 
            int size = sm;

            if (thumbPath == _ImageManager.largeThumbPath)
                size = lg;
            else if (thumbPath == _ImageManager.maxThumbPath)
                size = mx;

            if (!File.Exists(mappedDestFilePath))
                Utils.ImageTools.SetThumbnailImage(_OriginalUrl, mappedDestFilePath, size);

            //return string.Format("{0}{1}{2}", _originalDirectory_Virtual, thumbPath, System.Web.HttpUtility.HtmlEncode(_originalFileName));
            return string.Format("{0}{1}{2}", _originalDirectory_Virtual, thumbPath, _originalFileName);
        }
        private string Thumbnail_CalculateRemote(string thumbPath)
        {
            return string.Empty;

            //TODO 
            //FUTURE
            //ensure file + path exists
            //return string.Format("{0}{1}{2}{3}", _Config._MerchImageStorage_Remote, Path, ThumbPath, ImageName);


            //int g = _Config._ActThumbSizeLg;
            //int h = _Config._VenueThumbSizeLg;
        }

    }
}

