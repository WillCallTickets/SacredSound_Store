using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using SubSonic;

namespace Wcss
{
    public partial class HeaderImage
    {
        #region Table Properties

        public static List<_Enums.HeaderImageContext> TransformContextFromString(string contexts)
        {
            List<_Enums.HeaderImageContext> list = new List<_Enums.HeaderImageContext>();

            if (contexts != null && contexts.Trim().Length > 0)
            {
                string[] ctxs = contexts.Split(',');
                foreach (string s in ctxs)
                    list.Add((_Enums.HeaderImageContext)Enum.Parse(typeof(_Enums.HeaderImageContext), s, true));
            }

            return list;
        }

        public bool IsContext_ALL { get { return this.HeaderImageContext.Contains(_Enums.HeaderImageContext.All); } }
        public bool IsContext_Merch { get { return this.HeaderImageContext.Contains(_Enums.HeaderImageContext.Merch); } }
        public bool IsContext_Show { get { return this.HeaderImageContext.Contains(_Enums.HeaderImageContext.Show); } }
        public bool IsContext_Confirm { get { return this.HeaderImageContext.Contains(_Enums.HeaderImageContext.Confirm); } }
        public bool IsContext_Checkout { get { return this.HeaderImageContext.Contains(_Enums.HeaderImageContext.Checkout); } }
        public bool IsContext_Cart { get { return this.HeaderImageContext.Contains(_Enums.HeaderImageContext.Cart); } }
        public bool IsContext_About { get { return this.HeaderImageContext.Contains(_Enums.HeaderImageContext.About); } }
        public bool IsContext_Account { get { return this.HeaderImageContext.Contains(_Enums.HeaderImageContext.Account); } }
        public bool IsContext_Aux { get { return this.HeaderImageContext.Contains(_Enums.HeaderImageContext.Aux); } }

        [XmlAttribute("HeaderImageContext")]
        public List<_Enums.HeaderImageContext> HeaderImageContext
        {
            get { return TransformContextFromString(this.VcContext); }
            set
            {
                if (value.Count == 0)
                    this.VcContext = null;
                else
                {
                    string list = string.Empty;
                    foreach (_Enums.DiscountContext app in value)
                        list += string.Format("{0},", app.ToString());

                    list.TrimEnd(',');

                    this.VcContext = list;
                }
            }
        }
        public bool HasHeaderImageContext(_Enums.HeaderImageContext ctx)
        {
            return HeaderImageContext.Contains(ctx);
        }
        public bool IsMerchBanner { get { return this.TMerchId.HasValue; } }
        public bool IsShowBanner { get { return this.TShowId.HasValue; } }
        

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
        [XmlAttribute("IsDisplayPriority")]
        public bool IsDisplayPriority
        {
            get { return this.BDisplayPriority; }
            set { this.BDisplayPriority = value; }
        }
        [XmlAttribute("IsExclusive")]
        public bool IsExclusive
        {
            get { return this.BExclusive; }
            set { this.BExclusive = value; }
        }
        /// <summary>
        /// If the promotion is a banner....then this will hold the timeout - the length of time to display the particular banner
        /// </summary>
        [XmlAttribute("TimeoutMsec")]
        public int TimeoutMsec
        {
            get { return this.ITimeoutMsec; }
            set { this.ITimeoutMsec = value; }
        }
        [XmlAttribute("DateStart")]
        public DateTime DateStart
        {
            get { return (!this.DtStart.HasValue) ? DateTime.MinValue : this.DtStart.Value; }
            set { this.DtStart = value; }
        }
        //dtEndDate
        [XmlAttribute("DateEnd")]
        public DateTime DateEnd
        {
            get { return (!this.DtEnd.HasValue) ? DateTime.MaxValue : this.DtEnd.Value; }
            set { this.DtEnd = value; }
        }

        #endregion

        #region Derived Properties

        /// <summary>
        /// Indicates no unlock code is necessary
        /// </summary>
        [XmlAttribute("IsPublicOffer")]
        public bool IsPublicOffer
        {
            get { return this.UnlockCode == null || this.UnlockCode.Trim().Length == 0; }
        }
        /// <summary>
        /// Indicates that an unlock code is necessary to purchase the item
        /// </summary>
        [XmlAttribute("IsPrivateOffer")]
        public bool IsPrivateOffer
        {
            get { return (!this.IsPublicOffer); }
        }

        public bool IsUnlocked(string unlockCode)
        {
            if (this.IsPublicOffer)
                return true;

            if (unlockCode != null && unlockCode.Trim().Length > 0 && unlockCode == this.UnlockCode)
                return true;

            return false;
        }

        private bool _hasStarted
        {
            get
            {
                return this.DateStart < DateTime.Now;
            }
        }
        private bool _hasEnded
        {
            get
            {
                return this.DateEnd < DateTime.Now;
            }
        }
        public bool IsCurrentlyRunning(string unlockCode)
        {
            return this.IsActive && this.IsUnlocked(unlockCode) && this._hasStarted && (!this._hasEnded);
        }

        //AWARDS

        //triggers
        private bool IsUnlockActive { get { return this.UnlockCode != null && this.UnlockCode.Trim().Length > 0; } }

        #endregion

        public static string VirtualDirectory
        {
            get
            {
                //ensure directories exist
                string path = string.Format("/{0}/Images/Banners/", _Config._VirtualResourceDir);
                string mappedPath = System.Web.HttpContext.Current.Server.MapPath(path);

                if (!System.IO.Directory.Exists(mappedPath))
                    System.IO.Directory.CreateDirectory(mappedPath);

                return path;
            }
        }
        public string VirtualFilePath
        {
            get
            {
                if (this.FileName != null && this.FileName.Trim().Length > 0)
                {
                    return string.Format("{0}{1}", VirtualDirectory, this.FileName);

                    //ensure the file exists?
                    //string mappedFile = System.Web.HttpContext.Current.Server.MapPath(filePath);

                    //if (System.IO.File.Exists(mappedFile))
                    //    return filePath;
                }

                return string.Empty;
            }
        }



        #region DataSource methods

        public static void DeleteMethod(int Id, string FileName)
        {
            //a shunt to enable this to be used with an object datasource
        }
        public static void DeleteMethod()
        {
            //a shunt to enable this to be used with an object datasource
        }

        private static string GetActivityContext(string activeStatus)
        {
            if (activeStatus.ToLower() == "active")
                return "bActive = 1";
            else if (activeStatus.ToLower() == "notactive")
                    return "bActive = 0";

            return "bActive = 0 OR bActive = 1";
        }

        public static HeaderImageCollection GetHeaderImages(string activeStatus, int startRowIndex, int maximumRows)
        {
            HeaderImageCollection coll = new HeaderImageCollection();

            coll.LoadAndCloseReader(
                SubSonic.DataService.GetReader(
                    new SubSonic.QueryCommand(
                        string.Format("SELECT * FROM HeaderImage WHERE {0} ORDER BY [iDisplayOrder] ASC ", GetActivityContext(activeStatus)),
                        SubSonic.DataService.Provider.Name)));

            return coll;
        }

        public static int GetHeaderImagesCount(string activeStatus)
        {
            int count = 0;

            SubSonic.QueryCommand cmd = 
                new SubSonic.QueryCommand(
                    string.Format("SELECT COUNT(*) FROM HeaderImage WHERE {0} ", GetActivityContext(activeStatus)),
                    SubSonic.DataService.Provider.Name);

            try
            {
                using (System.Data.IDataReader dr = SubSonic.DataService.GetReader(cmd))
                {
                    while (dr.Read())
                        count = (int)dr.GetValue(0);
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                _Error.LogException(ex);
            }

            return count;
        }

        #endregion

    }

    public partial class HeaderImageCollection
    {
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
        public HeaderImage AddToCollection(string fileName, string navigateUrl, string displayText, Show show, Merch merch,
            int timeoutMsecs, string contextList, string unlockCode, DateTime startDate, DateTime endDate)
        {
            System.Collections.Generic.List<System.Web.UI.Pair> args = new System.Collections.Generic.List<System.Web.UI.Pair>();
            args.Add(new System.Web.UI.Pair("FileName", fileName));
            args.Add(new System.Web.UI.Pair("NavigateUrl", navigateUrl));
            args.Add(new System.Web.UI.Pair("DisplayText", displayText));
            if(show != null)
                args.Add(new System.Web.UI.Pair("TShowId", show.Id));
            if(merch != null)
                args.Add(new System.Web.UI.Pair("TMerchId", merch.Id));
            args.Add(new System.Web.UI.Pair("vcContext", contextList));
            args.Add(new System.Web.UI.Pair("UnlockCode", unlockCode));
            args.Add(new System.Web.UI.Pair("StartDate", startDate));
            args.Add(new System.Web.UI.Pair("EndDate", endDate));
            args.Add(new System.Web.UI.Pair("ITimeoutMsecs", timeoutMsecs));

            HeaderImage newItem = AddToCollection(args);

            return newItem;
        }

        public HeaderImage AddToCollection(System.Collections.Generic.List<System.Web.UI.Pair> args)
        {
            return Utils._Collection.AddItemToOrderedCollection(this.GetList(), args);
        }

        /// <summary>
        /// Delete a Banner from the collection by ID
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        public bool DeleteFromCollection(int idx)
        {
            return Utils._Collection.DeleteFromOrderedCollection(this.GetList(), idx);
        }

        /// <summary>
        /// Reorder a Banner by ID and direction
        /// </summary>
        /// <param name="idx"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public HeaderImage ReorderItem(int idx, string direction)
        {
            //override this to handle active and non-active
            return Utils._Collection.ReorderOrderedCollection(this.GetList(), idx, direction);
        }
    }
}
