using System;
using System.Xml.Serialization;
using System.IO;//delete the thumbnail

//create a new thumbnail
//get dimensions
//save the new data

namespace Wcss
{
    public partial class ItemImageCollection : Utils._Collection.IOrderable<ItemImage>
    {   
        /// <summary>
        /// this supposes that we have already determined that the values have changed
        /// </summary>
        /// <param name="idx"></param>
        /// <param name="newSize"></param>
        /// <param name="className"></param>
        public void ResizeItemImage(int idx, int newSize)
        {
            ItemImage entity = (ItemImage)this.Find(idx);

            if (entity != null)
            {
                entity.ImageManager.DeleteThumbnails();
                entity.ImageManager.CreateAllThumbs();
            }
        }

        /// <summary>
        /// Add an ItemImage to the collection. Also constructs thumbnails for the image
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="divisionDirectory"></param>
        /// <param name="className"></param>
        /// <param name="originalName"></param>
        /// <param name="imageHeight"></param>
        /// <param name="imageWidth"></param>
        /// <returns></returns>
        public ItemImage AddToCollection(Merch parent, string divisionDirectory, string className,
            string originalName, int imageHeight, int imageWidth)
        {
            System.Collections.Generic.List<System.Web.UI.Pair> args = new System.Collections.Generic.List<System.Web.UI.Pair>();
            args.Add(new System.Web.UI.Pair("DtStamp", DateTime.Now));
            args.Add(new System.Web.UI.Pair("TMerchId", parent.Id));
            args.Add(new System.Web.UI.Pair("OverrideThumbnail", false));
            args.Add(new System.Web.UI.Pair("Path", divisionDirectory));
            args.Add(new System.Web.UI.Pair("ImageName", originalName));
            args.Add(new System.Web.UI.Pair("ImageHeight", imageHeight));
            args.Add(new System.Web.UI.Pair("ImageWidth", imageWidth));
            args.Add(new System.Web.UI.Pair("ThumbClass", className ?? string.Empty));

            ItemImage newItem = AddToCollection(args);

            newItem.ImageManager.CreateAllThumbs();

            return newItem;
        }

        public ItemImage AddToCollection(System.Collections.Generic.List<System.Web.UI.Pair> args)
        {
            return Utils._Collection.AddItemToOrderedCollection(this.GetList(), args);
        }

        /// <summary>
        /// Delete an ItemImage from the collection by ID. Properly disposes child objects (thumbnails and image files)
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        public bool DeleteFromCollection(int idx)
        {
            ItemImage entity = (ItemImage)this.Find(idx);
            entity.ImageManager.Delete();//destroys its children

            return Utils._Collection.DeleteFromOrderedCollection(this.GetList(), idx);
        }

        /// <summary>
        /// Reorder a ItemImage by ID and direction
        /// </summary>
        /// <param name="idx"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public ItemImage ReorderItem(int idx, string direction)
        {
            return Utils._Collection.ReorderOrderedCollection(this.GetList(), idx, direction);
        }
    }


    public partial class ItemImage
    {
        [XmlAttribute("DisplayOrder")]
        public int DisplayOrder
        {
            get { return this.IDisplayOrder; }
            set { this.IDisplayOrder = value; }
        }

        #region Image Mgmt

        public _ImageManager _imageManager = null;
        public _ImageManager ImageManager
        {
            get
            {
                if (_imageManager == null)
                    _imageManager = new _ImageManager(this.path_original);

                return _imageManager;
            }
        }
        private string path_original
        {
            get
            {
                return (this.ImageName != null && this.ImageName.Trim().Length > 0) ?
                    string.Format("{0}{1}/{2}", _ImageManager._MerchImageStorage_Local, this.Path, this.ImageName) : string.Empty;
            }
        }
        public string Url_Original { get { return this.ImageManager.OriginalUrl; } }
        public string Thumbnail_Small { get { return (this.OverrideThumbnail) ? this.Url_Original : this.ImageManager.Thumbnail_Small; } }
        public string Thumbnail_Large { get { return (this.OverrideThumbnail) ? this.Url_Original : this.ImageManager.Thumbnail_Large; } }
        public string Thumbnail_Max { get { return (this.OverrideThumbnail) ? this.Url_Original : this.ImageManager.Thumbnail_Max; } }

        #endregion

        public bool IsPortrait { get { return this.ImageWidth <= this.ImageHeight;  } }
        public bool IsLandscape { get { return this.ImageWidth > this.ImageHeight; } }

        public bool IsDetailImage
        {
            get
            {
                if(! this.BDetailImage.HasValue)
                    return false;

                return this.BDetailImage.Value;
            }
            set
            {
                this.BDetailImage = value;
            }
        }
        public bool IsItemImage
        {
            get
            {
                if (!this.BItemImage.HasValue)
                    return true;

                return this.BItemImage.Value;
            }
            set
            {
                this.BItemImage = value;
            }
        }
        /// <summary>
        /// Overrides the default sizing for images and uses the original image without creating a thumbnail. 
        /// Not recommended unless a hi-res image is absolutely necessary for normal display (pictures that 
        /// do not look correct as a thumbnail - smudged/moire).
        /// </summary>
        public bool OverrideThumbnail
        {
            get
            {
                if (!this.BOverrideThumbnail.HasValue)
                    return false;

                return this.BOverrideThumbnail.Value;
            }
            set
            {
                this.BOverrideThumbnail = value;
            }
        }

        public void ChangeImageName(string newName)
        {
            string extOld = System.IO.Path.GetExtension(this.ImageName).ToLower();
            string extNew = System.IO.Path.GetExtension(newName).ToLower();

            if (extOld != extNew)
                throw new Exception("The extension of the new item must match the existing extension.");

            if (this.ImageName != null && this.ImageName.Trim().Length > 0)
                ImageManager.DeleteThumbnails();

            //get the original image and rename - move the file
            string mappedSourceImage = System.Web.HttpContext.Current.Server.MapPath(path_original);
            string newImage = System.Web.HttpContext.Current.Server.MapPath(string.Format("{0}{1}/{2}", 
                _ImageManager._MerchImageStorage_Local, this.Path, newName.Replace(" ", string.Empty)));
            File.Move(mappedSourceImage, newImage);
            
            //change the name - thumbs will rebuild auto
            this.ImageName = newName;
            this._imageManager = null;

            this.ImageManager.CreateAllThumbs();

            this.Save();
        }
    }
}

