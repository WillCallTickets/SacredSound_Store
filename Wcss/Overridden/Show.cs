using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

using System.Web.UI;

namespace Wcss
{
    public partial class Show 
    {
        public string FriendlyUrl
        {
            get
            {
                return this.CurrentNextShowDate.FriendlyUrl;
            }
        }

        /// <summary>
        /// returns a list of showdates that are ordered by DateOfShow_ToSortBy
        /// specify a bool to get dates in time context - false to retrieve all dates
        /// </summary>
        public ShowDateCollection GetDisplayableOrderedShowDates
        {
            get
            {
                DateTime offset = _Config.SHOWOFFSETDATE;
                ShowDateCollection coll = new ShowDateCollection();

                if (this.IsDisplayable)
                {
                    coll.AddRange(this.ShowDateRecords().GetList().FindAll(delegate(ShowDate match) { return (match.IsActive == true && match.DateOfShow_ToSortBy >= offset); }));

                    coll.SortBy_DateToOrderBy();
                }

                return coll;
            }
        }

        #region Show Image & DisplayUrl

         #region Image Mgmt

        /// <summary>
        /// note this method is only available for show
        /// It does not save record!
        /// </summary>
        public void ImageManager_Delete()
        {
            //TODO: make sure we are not deleting any default pics - venues!!
            if (this.DisplayUrl != null && this.DisplayUrl.Trim().Length > 0)
            {
                int matchingUrls = new SubSonic.Select().From<Show>().Where("DisplayUrl").IsEqualTo(this.DisplayUrl).GetRecordCount();

                if(matchingUrls == 1)
                    this.ImageManager.Delete();
            }

            //have this out of the loop so that non-synced urls can be cleaned up
            this._imageManager = null;
            this.PicWidth = 0;
            this.PicHeight = 0;
            this.DisplayUrl = null;
            this.Save();
        }

        private void AssignImage()
        {
            if (this.DisplayUrl != null && this.DisplayUrl.Length > 0)
            {
                _imageManager = new _ImageManager(string.Format("{0}{1}", _ImageManager._ShowImageStorage_Local, this.DisplayUrl.ToLower()));
                if (this.ImageHeight <= 0 || this.ImageWidth <= 0)
                {
                    this.IPicHeight = PicHeight;//this sets both dims

                    _imgWidth = this.IPicWidth;
                    _imgHeight = this.IPicHeight;
                }
            }
            else if (this.ShowDateRecords().Count > 0)
            {
                JShowActCollection coll = new JShowActCollection();
                coll.AddRange(this.ShowDateRecords()[0].JShowActRecords().GetList().FindAll(
                    delegate(JShowAct match) { return (match.TopBilling_Effective); }));

                if (coll.Count > 1)
                    coll.Sort("IDisplayOrder", true);

                if (coll.Count > 0)
                {
                    JShowAct act = coll[0];
                    if (act != null)
                    {
                        Act ac = act.ActRecord;

                        if (ac != null && ac.Url_Original != null && ac.Url_Original.Trim().Length > 0)
                        {
                            _imageManager = ac.ImageManager;

                            if (this.ImageHeight <= 0 || this.ImageWidth <= 0)
                            {
                                this.IPicHeight = PicHeight;//this sets both dims
                                _imgWidth = ac.PicWidth;
                                _imgHeight = ac.PicHeight;
                            }
                        }
                    }
                }
            }

            //ASSIGN A DEFAULT
            //if the file does not exist - and we are in venue mode - assign the default venue image
            if (_imageManager == null && this.VenueRecord.Url_Original != null && this.VenueRecord.Url_Original.Trim().Length > 0)
            {
                _imageManager = this.VenueRecord.ImageManager;

                if (this.ImageHeight <= 0 || this.ImageWidth <= 0)
                {
                    this.IPicHeight = PicHeight;//this sets both dims
                    _imgWidth = this.VenueRecord.IPicWidth;
                    _imgHeight = this.VenueRecord.IPicHeight;
                }
            }
        }
        private _ImageManager _imageManager = null;
        public _ImageManager ImageManager
        {
            get
            {
                //090727 - get rid of the extra file exists calls
                if (_imageManager == null)
                {
                    AssignImage();
                 
                    //if no image to assign - keep the mgr null
                }

                //record display url if show is day of - or in past
                //and the image is not a venue image
                //RecordShowImage();

                return _imageManager;
            }
            set { _imageManager = null; }
        }
        private void RecordShowImage()
        {
            //only do this if we have not recorded a displayurl for this show
            if (_imageManager != null && (this.DisplayUrl == null || this.DisplayUrl.Trim().Length == 0) && 
                this.LastDate <= DateTime.Now.Date && System.Web.HttpContext.Current != null)
            {
                string mappedOrig = System.Web.HttpContext.Current.Server.MapPath(_imageManager.OriginalUrl);

                //ensure given image
                if (System.IO.File.Exists(mappedOrig))
                {
                    //get the filename
                    string filename = System.IO.Path.GetFileName(mappedOrig);
                    string unmappedDest = string.Format("/{0}/Images/Shows/{1}", _Config._VirtualResourceDir, filename);
                    string mappedDest = System.Web.HttpContext.Current.Server.MapPath(unmappedDest);

                    if (!System.IO.File.Exists(mappedDest))
                    {
                        //copy the image to the show directory
                        System.IO.File.Copy(mappedOrig, mappedDest);
                    }

                    try
                    {
                        this.DisplayUrl = filename;
                        _imageManager = new _ImageManager(string.Format("{0}{1}", _ImageManager._ShowImageStorage_Local, this.DisplayUrl.ToLower()));
                        _imageManager.CreateAllThumbs();
                        System.Web.UI.Pair p = Utils.ImageTools.GetDimensions(System.Web.HttpContext.Current.Server.MapPath(this.ImageManager.OriginalUrl));

                        this.IPicWidth = (int)p.First;
                        this.IPicHeight = (int)p.Second;
                        _imgWidth = this.IPicWidth;
                        _imgHeight = this.IPicHeight;

                        string sql = "UPDATE [Show] SET [DisplayUrl] = @displayUrl, [iPicWidth] = @width, [iPicHeight] = @height WHERE [Id] = @idx; ";
                        SubSonic.QueryCommand cmd = new SubSonic.QueryCommand(sql, SubSonic.DataService.Provider.Name);
                        cmd.Parameters.Add("@idx", this.Id, System.Data.DbType.Int32);
                        cmd.Parameters.Add("@displayUrl", this.DisplayUrl, System.Data.DbType.String);
                        cmd.Parameters.Add("@width", this.IPicWidth, System.Data.DbType.Int32);
                        cmd.Parameters.Add("@height", this.IPicHeight, System.Data.DbType.Int32);

                        SubSonic.DataService.ExecuteQuery(cmd);
                    }
                    catch (Exception ex)
                    {
                        _Error.LogException(ex);
                        throw;
                    }
                }
            }
        }



        private string path_original
        {
            get
            {
                return (this.DisplayUrl != null && this.DisplayUrl.Trim().Length > 0) ?
                    string.Format("{0}{1}", _ImageManager._ShowImageStorage_Local, this.DisplayUrl) : string.Empty;
            }
        }
        public string Url_Original { get { return this.ImageManager.OriginalUrl; } }
        public string Thumbnail_Small { get { return this.ImageManager.Thumbnail_Small; } }
        public string Thumbnail_Large { get { return this.ImageManager.Thumbnail_Large; } }
        public string Thumbnail_Max { get { return this.ImageManager.Thumbnail_Max; } }

        public int PicWidth
        {
            get
            {
                if (this.IPicWidth == 0 && DisplayUrl != null)
                {
                    try
                    {
                        System.Web.UI.Pair p = Utils.ImageTools.GetDimensions(System.Web.HttpContext.Current.Server.MapPath(this.ImageManager.OriginalUrl));
                        this.IPicWidth = (int)p.First;
                        this.IPicHeight = (int)p.Second;

                        _ImageManager.UpdatePictureDimensions(this.Id, Wcss.Show.table.ToString(), this.IPicWidth, this.IPicHeight);
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
        public int PicHeight
        {
            get
            {
                if (this.IPicHeight == 0 && DisplayUrl != null)
                {
                    try
                    {
                        System.Web.UI.Pair p = Utils.ImageTools.GetDimensions(System.Web.HttpContext.Current.Server.MapPath(this.ImageManager.OriginalUrl));
                        this.IPicWidth = (int)p.First;
                        this.IPicHeight = (int)p.Second;

                        _ImageManager.UpdatePictureDimensions(this.Id, Wcss.Show.table.ToString(), this.IPicWidth, this.IPicHeight);
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
        //we use the image dimensions to figure out which where the imageis coming from
        private int _imgWidth = -1;
        private int _imgHeight = -1;
        public int ImageHeight
        {
            get
            {
                return _imgHeight;
            }
        }
        public int ImageWidth
        {
            get
            {
                return _imgWidth;
            }
        }
        public bool IsSquare { get { return ImageHeight == ImageWidth; } }
        public bool IsLandscape { get { return ImageHeight < ImageWidth; } }
        public bool IsPortrait { get { return ImageHeight > ImageWidth; } }

        #endregion

        /// <summary>
        /// returns a single image for the show. will return the headliner image - otherwise, an empty string. this is not null
        /// </summary>
        private string _showUrl = null;
        public string ShowUrl
        {
            get
            {
                if (_showUrl == null)
                    _showUrl = string.Format("http://{0}/Store/ChooseTicket.aspx?sid={1}", _Config._DomainName, this.Id.ToString());

                return _showUrl;
            }
        }
        
        /// <summary>
        /// returns a single image for the show. will return the headliner image - otherwise, an empty string. this is not null
        /// </summary>
        private string _showImageUrl = null;
        public string ShowImageUrl
        {
            get
            {
                if (_showImageUrl == null && this.ImageManager != null)
                {
                    _showImageUrl = this.ImageManager.Thumbnail_Large;
                }

                return _showImageUrl;
            }
        }
        public static string GetShowDisplayImage(ShowDate sd, bool useLargeVersion, bool utilizeDefaultPicture, bool recordImage)
        {
            string image = sd.ShowRecord.ShowImageUrl;

            if (image != null)
            {
                if(!useLargeVersion)
                    image = image.Replace("thumblg", "thumbsm");

                //default pics
                if (!utilizeDefaultPicture)
                {
                    //lit out default pics here
                    if(image.ToLower().IndexOf("newbordergray.png") != -1)
                        image = null;
                }

                if (image != null && recordImage)
                {
                    sd.ShowRecord.RecordShowImage();
                }
            }

            //return a default if none found
            if ((image == null || (image != null && image.Trim().Length == 0)) && utilizeDefaultPicture)
            {
                if (_Config._Site_Entity_Mode == _Enums.SiteEntityMode.Venue && sd.ShowRecord.VenueRecord.Url_Original != null && sd.ShowRecord.VenueRecord.Url_Original.Trim().Length > 0)
                    return (useLargeVersion) ? sd.ShowRecord.VenueRecord.ImageManager.Thumbnail_Large : sd.ShowRecord.VenueRecord.ImageManager.Thumbnail_Large;
                else
                    return string.Format("/{0}/Images/UI/blankimage_75.png", _Config._VirtualResourceDir);
            }

            return image;
        }

        #endregion

        [XmlAttribute("IsDisplayRichText")]
        public bool IsDisplayRichText
        {
            get { return this.BDisplayRichText; }
            set { this.BDisplayRichText = value; }
        }
        [XmlAttribute("IsAllowFacebookLike")]
        public bool IsAllowFacebookLike
        {
            get { return this.BAllowFacebookLike; }
            set { this.BAllowFacebookLike = value; }
        }
        [XmlAttribute("IsHideAutoGenerated")]
        public bool IsHideAutoGenerated
        {
            get { return this.BHideAutoGenerated; }
            set { this.BHideAutoGenerated = value; }
        }

        public ShowDisplay Display = null; //new Display(this);

        public override void Initialize()
        {
            base.Initialize();
            if(this.Display == null)
                this.Display = new ShowDisplay(this);
        }
        
        public bool AllShowDatesSoldOut
        {
            get
            {
                foreach (ShowDate sd in this.ShowDateRecords())
                    if (!sd.IsSoldOut)
                        return false;

                return true;
            }
        }
        public bool IsSoldOut
        {
            get
            {
                return this.BSoldOut;
            }
            set
            {
                this.BSoldOut = value;
            }
        }
        public bool IsVirtuallySoldOut
        {
            get
            {
                ShowDateCollection coll = new ShowDateCollection();
                coll.AddRange(this.ShowDateRecords().GetList().FindAll(delegate(ShowDate match) { return (match.IsActive == true); }));

                foreach (ShowDate sd in coll)
                {
                    if (!sd.IsSoldOut)
                        return false;
                }

                return (coll.Count > 0);//return false if there are no active shows
            }
        }

        #region Properties
        public bool OverrideActBilling
        {
            get { return this.BOverrideActBilling; }
            set { this.BOverrideActBilling = value; }
        }
        public DateTime AnnounceDate
        {
            get { return (!this.DtAnnounceDate.HasValue) ? Utils.Constants._MinDate : this.DtAnnounceDate.Value; }
            set { this.DtAnnounceDate = (value > Utils.Constants._MinDate) ? value : (DateTime?)null; }
        }
        public DateTime DateOnSale
        {
            get { return (!this.DtDateOnSale.HasValue) ? Utils.Constants._MinDate : this.DtDateOnSale.Value; }
            set { this.DtDateOnSale = (value > Utils.Constants._MinDate) ? value : (DateTime?)null; }
        }
        public bool IsActive
        {
            get { return this.BActive; }
            set { this.BActive = value; }
        }
        /// <summary>
        /// ensures show is announced and active - see other IsDisplayable properties (web and ticketing)
        /// </summary>
        public bool IsDisplayable
        {
            get { return this.IsActive && this.AnnounceDate < DateTime.Now; }
        }
        public string TopText_Derived { get { return (this.TopText == null) ? string.Empty : this.TopText; } set { this.TopText = value.Trim(); } }
        
        #endregion

        public bool HasShowDateKey(string mp)
        {
            if (this.AnnounceDate < DateTime.Now)
            {
                foreach (ShowDate sd in this.ShowDateRecords())
                {
                    if (sd.HasAppropriateKey(mp))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public DateTime GetShowDateTixKeyStart(string mp)
        {
            DateTime start = DateTime.MaxValue;

            foreach (ShowDate sd in this.ShowDateRecords())
            {
                //get the earliest date
                DateTime retVal = sd.GetMinTixKeyStartDate(mp);
                if (retVal < start)
                    start = retVal;
            }

            return start;
        }

        public bool AllShowsAreDayOfShowOrPast
        {
            get
            {
                foreach (ShowDate sd in this.ShowDateRecords().GetList()
                    .FindAll(delegate(ShowDate match) { return (match.IsActive == true); }))
                {
                    if (sd.DateOfShow.Date > DateTime.Now.Date)
                        return false;
                }

                return true;
            }
        }

        public bool IsCompletelyOfStatus(_Enums.ShowDateStatus st)
        {
            foreach (ShowDate sd in this.ShowDateRecords())
            {
                if (sd.IsActive && sd.ShowStatusRecord.Name != st.ToString())
                    return false;
            }

            return true;
        }
        public ActCollection HeadlinerCollection
        {
            get
            {
                ActCollection coll = new ActCollection();

                foreach (ShowDate sd in this.ShowDateRecords())
                {
                    JShowActCollection coll2 = new JShowActCollection();
                    coll2.AddRange(sd.JShowActRecords().GetList().FindAll(delegate(JShowAct match) { return (match.TopBilling_Effective); }));
                    if (coll2.Count > 1)
                        coll2.Sort("IDisplayOrder", true);

                    foreach (JShowAct js in coll2)// sd.JShowActRecords().OrderByAsc("DisplayOrder"))
                    {
                        //TODO: does this work
                        if (!coll.Contains(js.ActRecord))
                            coll.Add(js.ActRecord);
                    }
                }

                return coll;
            }
        }
        //private string _seoName = null;
        public string SeoName
        {
            get
            {
                return this.CurrentNextShowDate.SeoName;
            }
        }
        public bool ShowNameMatches(bool performChangeWhenItDoesntMatch)
        {
            //get first show date
            ShowDateCollection dateColl = new ShowDateCollection();
            dateColl.AddRange(this.ShowDateRecords().GetList().FindAll(
                delegate(ShowDate match) { return (match.IsActive == true); } ));
            if (dateColl.Count > 1)
                dateColl.Sort("DtDateOfShow", true);

            if (dateColl.Count == 0)
                return false;
                //throw new Exception("There are no active show dates to compare with.");

            DateTime firstDate = dateColl[0].DateOfShow;

            //get headliners from first date
            JShowActCollection actColl = new JShowActCollection();
            actColl.AddRange(dateColl[0].JShowActRecords().GetList().FindAll(
                delegate(JShowAct match) { return (match.TopBilling_Effective == true); }));
            if (actColl.Count > 1)
                actColl.Sort("IDisplayOrder", true);

            ActCollection coll = new ActCollection();
            foreach (JShowAct join in actColl)
                coll.Add(join.ActRecord);

            if (coll.Count == 0)
            {
                string error = string.Format("No headliner: ShowId: {0} Name: {1}", this.Id, this.Name);
                
                _Error.LogException(new Exception(error));

                return false;//dont allow to make a name without a headline act
            }

            string calcdName = Show.CalculatedShowName(firstDate, this.VenueRecord, coll);
            this.ResetCalculatedActName();

            if (performChangeWhenItDoesntMatch && (this.Name != calcdName))
            {
                this.Name = calcdName;

                this.Save();

                return true;
            }

            return (this.Name == calcdName);
        }
        private string _displayName = null;
        public string DisplayShowName
        {
            get
            {
                if (_displayName == null)
                    _displayName = string.Format("{0} {1}{2} {3}", this.ShowNamePart, this.VenueRecord.City,
                        (this.VenueRecord.City != null && this.VenueRecord.City.Trim().Length > 0 && this.VenueRecord.State != null &&
                        this.VenueRecord.State.Trim().Length > 0) ? ", " : string.Empty, this.VenueRecord.State).Trim();

                return _displayName;
            }
        }
        public static string CalculatedShowName(DateTime sDate, Venue venue, ActCollection headlineActs)
        {
            //300 total length - but lets keep to 255 for authnet
            //yyyy-mm-dd hh:mmtt - (22)
            //venue - 70
            //name - 155

            //format the proper date time
            StringBuilder sb = new StringBuilder();

            string _venue = (venue.Name_Displayable.Length > 55) ? venue.Name_Displayable.Substring(0, 55) + "..." : venue.Name_Displayable.Trim();

            StringBuilder _head = new StringBuilder();

            foreach (Act act in headlineActs)
                _head.AppendFormat("{0}~", act.Name_Displayable);

            Utils.ParseHelper.ConvertTildesToCommasAndAmpersands(_head);

            if (_head.Length > 150)
            {
                _head.Length = 150;
                _head.Append("...");
            }

            sb.AppendFormat("{0} {1} - {2} - {3}", sDate.ToString("yyyy/MM/dd"), sDate.ToString("hh:mm tt"), _venue, _head.ToString());

            return sb.ToString().ToUpper();
        }
        public void ResetCalculatedActName()
        {
            _calculatedActName = null;
        }
        private string _calculatedActName = null;
        public string CalculatedActName
        {
            get
            {
                if (_calculatedActName == null)
                {
                    _calculatedActName = string.Empty;

                    ActCollection acts = new ActCollection();

                    acts.AddRange(this.GetAllActs());

                    if (acts.Count > 0)
                    {
                        foreach (Act jsq in acts)
                            _calculatedActName += string.Format("{0}~", jsq.Name_Displayable);

                        if (_calculatedActName.Length > 0)
                            _calculatedActName = Utils.ParseHelper.ConvertTildesToCommasAndAmpersands(_calculatedActName);
                    }
                }

                return _calculatedActName;
            }

        }

        public bool HasPromoter(Promoter p)
        {
            foreach (JShowPromoter js in this.JShowPromoterRecords())
                if (js.PromoterRecord.Id == p.Id) return true;

            return false;
        }

        public bool HasShowDate(int showDateId)
        {
            foreach (ShowDate sd in this.ShowDateRecords())
            {
                if (sd.Id == showDateId) return true;
            }

            return false;
        }

        [XmlAttribute("CurrentNextShowDate")]
        public ShowDate CurrentNextShowDate
        {
            get
            {
                //if we are prior to the first date or past the last date - return the first date
                if (_Config.SHOWOFFSETDATE <= this.FirstDate || _Config.SHOWOFFSETDATE > this.LastDate)
                    return this.FirstShowDate;
                else
                {
                    //find the closest date
                    ShowDateCollection coll = new ShowDateCollection();
                    coll.AddRange(this.ShowDateRecords().GetList().FindAll(
                        delegate(ShowDate match) { return (match.IsActive); }));
                    if (coll.Count > 1)
                        coll.Sort("DtDateOfShow", true);

                    foreach (ShowDate sd in coll)
                    {
                        //this works when showsdates are in order
                        if (sd.DateOfShow >= _Config.SHOWOFFSETDATE)
                            return sd;
                    }
                }

                //default
                return FirstShowDate;
            }
        }
        private ShowDate _firstShowDate = null;
        [XmlAttribute("FirstShowDate")]
        public ShowDate FirstShowDate
        {
            get
            {
                if (_firstShowDate == null)
                {
                    if (this.ShowDateRecords().Count > 0)
                    {
                        ShowDateCollection coll = new ShowDateCollection();
                        coll.AddRange(this.ShowDateRecords().GetList().FindAll(
                            delegate(ShowDate match) { return (match.IsActive); }));
                        if (coll.Count > 1)
                            coll.Sort("DtDateOfShow", true);

                        if (coll.Count > 0)
                            _firstShowDate = coll[0];
                    }
                }

                return _firstShowDate;
            }
        }
        private ShowDate _lastShowDate = null;
        [XmlAttribute("LastShowDate")]
        public ShowDate LastShowDate
        {
            get
            {
                if (_lastShowDate == null)
                {
                    if (this.ShowDateRecords().Count > 0)
                    {
                        ShowDateCollection coll = new ShowDateCollection();
                        coll.AddRange(this.ShowDateRecords().GetList().FindAll(
                            delegate(ShowDate match) { return (match.IsActive); }));
                        
                        //order by reverse - use false
                        if (coll.Count > 1)
                            coll.Sort("DtDateOfShow", false);

                        if (coll.Count > 0)
                            _lastShowDate = coll[0];
                    }
                }

                return _lastShowDate;
            }
        }

        [XmlAttribute("FirstDate")]
        public DateTime FirstDate
        {
            get
            {
                if (FirstShowDate == null)
                    return Utils.Constants._MinDate;
                else
                    return FirstShowDate.DateOfShow;
            }
        }
        [XmlAttribute("LastDate")]
        public DateTime LastDate
        {
            get
            {
                if (LastShowDate == null)
                    return Utils.Constants._MinDate;
                else
                    return LastShowDate.DateOfShow;
            }
        }
        
        private DateTime _showDatePart = DateTime.MinValue;
        [XmlAttribute("ShowDatePart")]
        public DateTime ShowDatePart
        {
            get {
                if (_showDatePart == DateTime.MinValue)
                {
                    string dte = this.Name.Substring(0, 19).Trim(); return DateTime.Parse(dte);
                }

                return _showDatePart;
            }
        }
        private string _showNamePart = null;
        [XmlAttribute("ShowNamePart")]
        public string ShowNamePart
        {
            get
            {
                if (_showNamePart == null)
                {
                    _showNamePart = ParseShowNamePart(this.Name);
                }

                return _showNamePart;
            }
        }
        private string _showEventPart = null;
        [XmlAttribute("ShowEventPart")]
        public string ShowEventPart
        {
            get
            {
                if (_showEventPart == null)
                {
                    _showEventPart = ParseShowNamePart(this.ShowNamePart);
                }

                return _showEventPart;
            }
        }
        public static string ParseShowNamePart(string name)
        {
            return name.Substring(name.IndexOf("-", 0) + 1, name.Length - (name.IndexOf("-", 0) + 1)).Trim();
        }

        /// <summary>
        /// This is an uncached collection here!
        /// </summary>
        /// <returns></returns>
        public ShowTicketCollection GetDisplayableTickets(_Enums.VendorTypes vendorType, string unlockCode, bool includeLotterySignups, bool includeLotteryFulfillments)
        {
            //unlockCode
            ShowTicketCollection tix = new ShowTicketCollection();

            //show has to be announced
            //show must be on sale
            //if (this.IsActive && this.AnnounceDate < DateTime.Now && this.DateOnSale < DateTime.Now && (!this.IsSoldOut)
            if (this.IsActive && this.AnnounceDate < DateTime.Now && (!this.IsSoldOut))//worry about sale date when we get down to tickets
            {
                ShowDateCollection coll = new ShowDateCollection();
                coll.AddRange(this.ShowDateRecords().GetList().FindAll(
                    delegate(ShowDate match) { return (match.IsActive); }));
                if (coll.Count > 1)
                    coll.Sort("DtDateOfShow", true);

                foreach (ShowDate sd in coll)
                {
                    tix.AddRange(sd.GetDisplayableTickets(vendorType, unlockCode, includeLotterySignups, includeLotteryFulfillments, tix));
                }
            }

            return tix;
        }

        private ShowTicketCollection _overrideSelloutTickets = null;
        public ShowTicketCollection OverrideSelloutTickets
        {
            get
            {
                if (_overrideSelloutTickets == null)
                {
                    //get a list of tickets that will show if the show is sold out
                    _overrideSelloutTickets = new ShowTicketCollection();

                    //show just needs to be active - not worried about sold out here - that is determined by showticket field
                    if (this.IsActive)
                    {   
                        //once again ignore sellout
                        ShowDateCollection coll = new ShowDateCollection();
                        coll.AddRange(this.ShowDateRecords().GetList().FindAll(
                            delegate(ShowDate match) { return (match.IsActive); }));
                        if (coll.Count > 1)
                            coll.Sort("DtDateOfShow", true);

                        foreach (ShowDate sd in coll)
                        {
                            ShowTicketCollection coll2 = new ShowTicketCollection();
                            coll2.AddRange(sd.ShowTicketRecords().GetList().FindAll(
                                delegate(ShowTicket match) { return (match.IsActive && match.IsOverrideSellout && match.Available > 0); }));
                            if (coll2.Count > 1)
                                coll2.Sort("IDisplayOrder", true);

                            foreach (ShowTicket st in coll2)
                                _overrideSelloutTickets.Add(st);
                        }
                    }
                }

                return _overrideSelloutTickets;
            }
        }

        public bool IsLateShowOf(ShowDate sd)
        {
            DateTime LateNightShowTime = sd.DateOfShow.AddDays(1).Date.AddHours(3);//until 3AM

            //if this show contains an active showdate that is a late night show of sd then true
            foreach (ShowDate sDate in this.ShowDateRecords())
            {
                //if active and the status is good and if the date is past the show to compare with and less than 3 in the morning of the next day
                if (sDate.IsActive && sDate.IsOn && sDate.DateOfShow > sd.DateOfShow && sDate.DateOfShow < LateNightShowTime)
                {
                    return true;
                }
            }

            return false;
        }

        public bool HasWebSales()
        {
            foreach (ShowDate sd in this.ShowDateRecords())
            {
                foreach (ShowTicket st in sd.ShowTicketRecords())
                {
                    if (st.Sold > 0) return true;
                }
            }

            return false;
        }

        public void Deactivate()
        {
            this.IsActive = false;

            foreach (ShowDate sd in this.ShowDateRecords())
            {
                sd.ShowStatusRecord = Wcss._Lookits.ShowStatii.Where("Name",_Enums.ShowDateStatus.NotActive.ToString())[0];

                foreach (ShowTicket st in sd.ShowTicketRecords())
                {
                    st.IsActive = false;
                }
            }
        }

        public ActCollection GetAllActs()
        {
            ActCollection coll = new ActCollection();

            foreach (ShowDate sd in this.ShowDateRecords())
            {
                JShowActCollection coll2 = new JShowActCollection();
                coll2.AddRange(sd.JShowActRecords());
                if (coll2.Count > 1)
                    coll2.Sort("IDisplayOrder", true);

                foreach (JShowAct js in coll2)// sd.JShowActRecords().OrderByAsc("DisplayOrder"))
                {
                    if (coll.GetList().FindIndex(delegate (Act match) { return (match.Name_Displayable == js.ActRecord.Name_Displayable); } ) == -1)
                        coll.Add(js.ActRecord);
                }
            }

            return coll;
        }

        #region Child Collections

        #region ShowDates
        /// <summary>
        /// please note that dateToCopy can be null
        /// </summary>
        /// <param name="dateToAdd"></param>
        /// <param name="showTime"></param>
        /// <param name="dateToCopy"></param>
        /// <returns></returns>
        public ShowDate AddShowDateFromShowDate(DateTime dateToAdd, string showTime, ShowDate dateToCopy, string userName, Guid providerUserKey)
        {
            ShowDate newItem = new ShowDate();
            newItem.DtStamp = DateTime.Now;
            newItem.TShowId = this.Id;
            newItem.DateOfShow = DateTime.Parse(dateToAdd.ToString("yyyy-MM-dd hh:mmtt"));
            newItem.IsAutoBilling = dateToCopy.IsAutoBilling;
            newItem.Billing = dateToCopy.Billing;
            newItem.IsActive = true;//always make new additions active - allow change in edit mode
            newItem.ShowTime = (dateToCopy == null) ? showTime : dateToCopy.ShowTime;
            Age age = (dateToCopy == null) ? (Age)_Lookits.Ages.GetList().Find(delegate(Age match) { return (match.Name == _Config._Default_Age.ToString()); }) :
                dateToCopy.AgeRecord;
            newItem.TAgeId = (age != null) ? age.Id : 0;
            newItem.PricingText = (dateToCopy == null) ? null : dateToCopy.PricingText;
            newItem.IsPrivateShow = dateToCopy.IsPrivateShow;

            ShowStatus newStatus = (dateToCopy == null) ?
                (ShowStatus)_Lookits.ShowStatii.GetList().Find(delegate(ShowStatus match) { return (match.Name.ToLower() == "onsale"); }) :
                dateToCopy.ShowStatusRecord;
            newItem.TStatusId = (newStatus != null) ? newStatus.Id : 0;
            newItem.DisplayNotes = (dateToCopy == null) ? null : dateToCopy.DisplayNotes;

            try
            {
                newItem.Save();
            }
            catch (Exception e)
            {
                _Error.LogException(e);
                throw e;
            }

            //save children
            if (dateToCopy != null)
            {
                //verify that date to add is valid - cannot exist anywhere else
                ShowDate exists = this.ShowDateRecords().GetList().Find(
                    delegate(ShowDate match) { return (match.DateOfShow == dateToAdd); });

                if (exists != null)
                    throw new Exception(string.Format("A date already exists for the date and time specified. Please edit that date."));

                //copy all acts
                foreach (JShowAct join in dateToCopy.JShowActRecords())
                {
                    JShowAct js = new JShowAct();
                    js.TShowDateId = newItem.Id;//this has changed from join !!!
                    js.TActId = join.TActId;
                    js.PreText = join.PreText;
                    js.ActText = join.ActText;
                    js.Featuring = join.Featuring;
                    js.PostText = join.PostText;
                    js.DisplayOrder = join.DisplayOrder;
                    js.TopBilling = join.TopBilling;

                    newItem.JShowActRecords().Add(js);
                }

                newItem.JShowActRecords().SaveAll();



                //copy all tickets - except for packaged tickets
                foreach (ShowTicket join in dateToCopy.ShowTicketRecords())
                {
                    if ((!join.IsPackage) && (!join.IsDosTicket))
                    {
                        ShowTicket st = join.CopyShowTicketComplete(newItem, userName, providerUserKey);
                                                
                    }
                }
            }

            //do we need this to refresh collection?
            //newItem.ShowTicketRecords().SaveAll();//ok to call save all here - as the insert is the only dirty row

            ////post purchase
            //foreach (ShowTicket join in dateToCopy.ShowTicketRecords())
            //{
            //    bool hasPost = false;
            //    ShowTicket st2 = null;

            //    foreach (PostPurchaseText pp in join.PostPurchaseTextRecords())
            //    {
            //        hasPost = true;

            //        //find the corresponding show ticket in the new collection
            //        st2 = newItem.ShowTicketRecords().GetList().Find(delegate(ShowTicket match) 
            //        { 
            //            return match.IDisplayOrder == join.IDisplayOrder; });

            //        PostPurchaseText newpp = new PostPurchaseText();
            //        newpp.BActive = pp.BActive;
            //        newpp.DtStamp = DateTime.Now;
            //        newpp.IDisplayOrder = pp.IDisplayOrder;
            //        newpp.InProcessDescription = pp.InProcessDescription;
            //        newpp.PostText = pp.PostText;
            //        newpp.TShowTicketId = st2.Id;
            //        //newpp.Save();
            //        st2.PostPurchaseTextRecords().Add(newpp);
            //    }

            //    if(hasPost && st2 != null)
            //        st2.PostPurchaseTextRecords().SaveAll();
            //}

            this.ShowDateRecords().Add(newItem);

            return newItem;
        }

        public bool DeleteShowDate(int idx)
        {
            ShowDate entity = (ShowDate)this.ShowDateRecords().Find(idx);

            if (entity != null)
            {
                ShowTicketCollection coll = new ShowTicketCollection();
                coll.AddRange(entity.ShowTicketRecords());

                try
                {
                    while (coll.Count > 0)
                    {
                        coll.DeleteFromCollection(coll[0].Id);
                    }

                    this.ShowDateRecords().Remove(entity);

                    ShowDate.Delete(idx);

                    this.ShowDateRecords().SaveAll();

                    return true;
                }
                catch (Exception e)
                {
                    _Error.LogException(e);
                    throw e;
                }
            }

            return false;
        }
        #endregion

        #endregion

    }
}

