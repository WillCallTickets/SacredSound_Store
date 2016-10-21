using System;
using System.Text;
using System.Xml.Serialization;

namespace Wcss
{
    public partial class ShowDate 
    {
        private string _friendlyUrl = null;
        /// <summary>
        /// A link to the show page for this showdate. TODO: this will eventually change to a more MVC looking url. 
        /// in the form yyyy/mm/dd/hhmmtt/showeventname
        /// </summary>
        public string FriendlyUrl
        {
            get
            {
                if (_friendlyUrl == null)
                {
                    //TODO: this will change when we switch to an MVC pattern

                    _friendlyUrl = string.Format("/Store/ChooseTicket.aspx?sid={0}", this.TShowId.ToString());

                    //More MVC like
                    //string showName = Utils.ParseHelper.FriendlyFormat(this.ShowRecord.ShowEventPart);

                    //showName = string.Format("{0}{1}", showName.Substring(0, 1).ToUpper(), showName.Remove(0, 1));

                    //_friendlyUrl = string.Format("{0}{1}/{2}",
                    //    string.Empty, //this.TShowId.ToString(), 
                    //    this.DateOfShow.ToString("yyyy/MM/dd/hhmmtt"), 
                    //    showName).TrimEnd('/');
                }

                return _friendlyUrl;
            }
        }

        string _seoName = null;
        public string SeoName
        {
            get
            {
                if (_seoName == null)
                {
                    //date venue main act
                    //date y2010 m11 d11 h16
                    DateTime sd = this.DateOfShow;
                    string datePart = string.Format("y{0} m{1} d{2} h{3}-{4}", 
                        sd.Year.ToString(), 
                        (sd.Month > 9) ? sd.Month.ToString() : string.Format("0{0}", sd.Month.ToString()),
                        (sd.Day > 9) ? sd.Day.ToString() : string.Format("0{0}", sd.Day.ToString()),
                        (sd.Hour > 9) ? sd.Hour.ToString() : string.Format("0{0}", sd.Hour.ToString()),
                        (sd.Minute > 9) ? sd.Minute.ToString() : string.Format("0{0}", sd.Minute.ToString())
                        );
                    
                    string venuePart = Utils.ParseHelper.SeoFormat(this.ShowRecord.VenueRecord.Name);
                    if (venuePart.Trim().Length > 0)
                        _seoName += venuePart;

                    string actPart = string.Empty;
                    string[] actparts = this.ActString.Split('~');
                    foreach (string s in actparts)
                    {
                        string parsed = Utils.ParseHelper.SeoFormat(s);
                        if (parsed.Trim().Length > 0)
                            actPart += string.Format("{0} ", parsed);
                    }

                    _seoName = string.Format("{0}{1}{2}", datePart, 
                        (venuePart.Trim().Length > 0) ? string.Format(" {0}", venuePart.Trim()) : string.Empty, 
                        (actPart.Trim().Length > 0) ? string.Format(" {0}", actPart.Trim()) : string.Empty);
                }

                return _seoName;
            }
        }

        public string ListTitle
        {
            get
            {
                return string.Format("{0} - {1} {2}", this.Id.ToString(), this.Display.Date_NoMarkup_StatusNotFirstNoMarkup_NoTime,
                        this.ShowRecord.ShowNamePart);
            }
        }

        private string _showingsCsv = null;
        /// <summary>
        /// generally you will only go after this if it has been determined that this object is multiple. This does not worry about matching acts. The 
        /// fact that they are the same show is enough to qualify the relationship
        /// </summary>
        public string Showings_csv
        {
            get
            {
                if (_showingsCsv == null)
                {
                    if (this.ShowRecord.ShowDateRecords().Count > 0)
                    {
                        ShowDateCollection coll = new ShowDateCollection();
                        coll.AddRange(this.ShowRecord.ShowDateRecords().GetList().FindAll(delegate(ShowDate match)
                        {
                            return (match.IsActive && match.DateOfShow.AddHours(-_Config.DayTurnoverTime).Date == this.DateOfShow.AddHours(-_Config.DayTurnoverTime).Date);
                        }));

                        if (coll.Count > 1)
                        {
                            coll.Sort("DtDateOfShow", true);

                            foreach (ShowDate sd in coll)
                                _showingsCsv += string.Format("{0},", sd.ShowTime);

                            _showingsCsv = _showingsCsv.TrimEnd(',');
                        }
                    }
                } 

                return _showingsCsv;
            }
            set { _showingsCsv = null; }
        }

        private string _actString = null;
        private string _actString_Headliner = null;
        /// <summary>
        /// somewhat simplified - just checks the act's name - no pre, post, etc are evaluated
        /// </summary>
        public string ActString
        {
            get
            {
                if (_actString == null && this.JShowActRecords().Count > 0)
                {
                    _actString_Headliner = null;

                    System.Collections.Generic.List<JShowAct> list = new System.Collections.Generic.List<JShowAct>();
                    list.AddRange(this.JShowActRecords());
                    list.Sort(new Utils.Reflector.CompareEntities<JShowAct>(Utils.Reflector.Direction.Ascending, "IDisplayOrder"));

                    foreach (JShowAct jsa in list)
                    {
                        if(_actString_Headliner == null)
                            _actString_Headliner = jsa.ActRecord.Name;

                        _actString += string.Format("{0}~", jsa.ActRecord.Name);
                    }

                    _actString = _actString.TrimEnd('~');
                }
                
                return _actString;
            }
            set { _actString = null; }
        }

        /// <summary>
        /// This is the date that the show was created or announced
        /// </summary>
        public DateTime PseudoPublishDate { get { return (this.DtStamp > this.ShowRecord.AnnounceDate) ? this.DtStamp : this.ShowRecord.AnnounceDate; } }

        public ShowDateDisplay Display = null; //new Display(this);

        public override void Initialize()
        {
            base.Initialize();
            if(this.Display == null)
                this.Display = new ShowDateDisplay(this);
        }

        #region Properties

        /// <summary>
        /// Indicates how this show is displayed - there is no code for a show - so setting as private just removes from side menu and main (homepage) listing
        /// </summary>
        [XmlAttribute("IsPrivateShow")]
        public bool IsPrivateShow
        {
            get { return this.BPrivateShow; }
            set { this.BPrivateShow = value; }
        }
        [XmlAttribute("IsAutoBilling")]
        public bool IsAutoBilling
        {
            get { return this.BAutoBilling; }
            set { this.BAutoBilling = value; }
        }
        [XmlAttribute("IsActive")]
        public bool IsActive
        {
            get { return this.BActive; }
            set { this.BActive = value; }
        } 
        public bool CoHeadline
        {
            get 
            {
                JShowActCollection acts = new JShowActCollection();
                acts.AddRange(this.JShowActRecords().GetList().FindAll(delegate(JShowAct match) { return (match.TopBilling_Effective); }));

                int count = acts.Count;

                return (count > 1);
            }
        }
        /// <summary>
        /// Includes the door time - so MM/dd/yyyy hh:mmtt
        /// </summary>
        [XmlAttribute("DateOfShow")]
        public DateTime DateOfShow
        {
            get { return DtDateOfShow; }
            set { this.DtDateOfShow = value; }
        }
        [XmlAttribute("IsLateNightShow")]
        public bool IsLateNightShow
        {
            get { return this.BLateNightShow; }
            set { this.BLateNightShow = value; }
        }
        [XmlAttribute("UseFbRsvp")]
        public bool UseFbRsvp
        {
            get { return this.BUseFbRsvp; }
            set { this.BUseFbRsvp = value; }
        }
        /// <summary>
        /// Returns an adjustment for late night shows
        /// </summary>
        public DateTime DateOfShow_ToSortBy
        {
            get
            {
                if (this.IsLateNightShow)
                    return this.DtDateOfShow.AddHours(24);

                return this.DateOfShow;
            }
        }

        
        public bool IsOn
        {
            get
            {
                if (this.ShowStatusRecord.Name == _Enums.ShowDateStatus.OnSale.ToString() || this.ShowStatusRecord.Name == _Enums.ShowDateStatus.SoldOut.ToString())
                    return true;

                return false;
            }
        }
        public string StatusName
        {
            get
            {
                if (this.ShowStatusRecord != null)
                    return ShowStatusRecord.Name;

                return _Enums.ShowDateStatus.OnSale.ToString();
            }
        }
        #endregion

        #region Children Total Inventory
        private ShowTicketCollection ShowTicketRecords_Active
        {
            get
            {
                ShowTicketCollection coll = new ShowTicketCollection();
                coll.AddRange(this.ShowTicketRecords().GetList().FindAll(delegate(ShowTicket match) { return (match.IsActive); }));
                return coll;
            }
        }
        public int AllotedChildren
        {
            get
            {
                int total = 0;
                foreach (ShowTicket child in this.ShowTicketRecords_Active)
                    total += child.Allotment;
                return total;
            }
        }
        //#w
        //public int PendingChildren
        //{
        //    get
        //    {
        //        int total = 0;
        //        foreach (ShowTicket child in this.ShowTicketRecords_Active)
        //            total += child.Pending;
        //        return total;
        //    }
        //}
        public int SoldChildren
        {
            get
            {
                int total = 0;
                foreach (ShowTicket child in this.ShowTicketRecords_Active)
                    total += child.Sold;
                return total;
            }
        }
        public int AvailableChildren
        {
            get
            {
                int total = 0;
                foreach (ShowTicket child in this.ShowTicketRecords_Active)
                    total += child.Available;
                return total;
            }
        }
        public int RefundedChildren
        {
            get
            {
                int total = 0;
                foreach (ShowTicket child in this.ShowTicketRecords_Active)
                    total += child.Refunded;
                return total;
            }
        }
        #endregion

        #region DisplayProperties

        public string cartPromoter { get { return this.ShowRecord.cartPromoter; } }

        private string _listMainAct = null;
        public string listMainAct
        {
            get
            {
                if (_listMainAct == null)
                {
                    StringBuilder sb = new StringBuilder();

                    JShowActCollection coll = new JShowActCollection();
                    coll.AddRange(this.JShowActRecords().GetList().FindAll(
                        delegate(JShowAct entity) { return (entity.TopBilling_Effective); }));
                    if (coll.Count > 1)
                        coll.Sort("IDisplayOrder", true);

                    foreach (JShowAct js in coll)
                    {
                        sb.AppendFormat("{0} {1} {2} {3} {4}~",
                            (js.PreText != null) ? js.PreText : string.Empty,
                            js.ActRecord.Name_Displayable,
                            (js.ActText != null) ? js.ActText : string.Empty,
                            (js.Featuring != null) ? js.Featuring : string.Empty,
                            (js.PostText != null) ? js.PostText : string.Empty).ToString().Trim();
                    }

                    Utils.ParseHelper.ConvertTildesToCommasAndAmpersands(sb);

                    _listMainAct = sb.ToString();
                }

                return _listMainAct;
            }
        }

        private string _wc_CartHeadliner = null;
        public string wc_CartHeadliner
        {
            get
            {
                if (_wc_CartHeadliner == null)
                {
                    StringBuilder sb = new StringBuilder();

                    JShowActCollection coll = new JShowActCollection();
                    coll.AddRange(this.JShowActRecords().GetList().FindAll(
                        delegate(JShowAct entity) { return (entity.TopBilling_Effective); }));
                    if (coll.Count > 1)
                        coll.Sort("IDisplayOrder", true);

                    //always display co headlines
                    if (coll.Count > 0)
                    {
                        foreach (JShowAct ent in coll)
                        {
                            if (ent.PreText != null && ent.PreText.Trim().Length > 0)
                                sb.AppendFormat("<span class=\"pretext\">{0}</span> ", ent.PreText.Trim());

                            sb.AppendFormat("<span class=\"name\">{0}</span> ", ent.ActRecord.Name_Displayable);

                            if (ent.ActText != null && ent.ActText.Trim().Length > 0)
                                sb.AppendFormat("<span class=\"extra\">{0}</span> ", ent.ActText.Trim());

                            if (ent.Featuring != null && ent.Featuring.Trim().Length > 0)
                                sb.AppendFormat("<span class=\"featuring\">{0}</span> ", ent.Featuring.Trim());

                            if (ent.PostText != null && ent.PostText.Trim().Length > 0)
                                sb.AppendFormat("<span class=\"posttext\">{0}</span>", ent.PostText.Trim());

                            sb.Append("~");
                        }

                        Utils.ParseHelper.ConvertTildesToCommasAndAmpersands(sb);
                    }

                    _wc_CartHeadliner = sb.ToString();
                }

                return _wc_CartHeadliner;
            }
        }

        private string _altCartOpeners = null;
        public string altCartOpeners
        {
            get
            {
                if (_altCartOpeners == null)
                {
                    StringBuilder sb = new StringBuilder();

                    JShowActCollection coll = new JShowActCollection();
                    coll.AddRange(this.JShowActRecords().GetList().FindAll(
                        delegate(JShowAct entity) { return (!entity.TopBilling_Effective); }));
                    if (coll.Count > 1)
                        coll.Sort("IDisplayOrder", true);

                    if (coll.Count > 0)
                    {
                        sb.AppendFormat("<span class=\"with\"> with </span>");
                        foreach (JShowAct ent in coll)
                        {
                            if (ent.PreText != null && ent.PreText.Trim().Length > 0)
                                sb.AppendFormat("<span class=\"pretext\">{0}</span> ", ent.PreText.Trim());

                            sb.AppendFormat("<span class=\"name\">{0}</span> ", ent.ActRecord.Name_Displayable);

                            if (ent.ActText != null && ent.ActText.Trim().Length > 0)
                                sb.AppendFormat("<span class=\"extra\">{0}</span> ", ent.ActText.Trim());

                            if (ent.Featuring != null && ent.Featuring.Trim().Length > 0)
                                sb.AppendFormat("<span class=\"featuring\">{0}</span> ", ent.Featuring.Trim());

                            if (ent.PostText != null && ent.PostText.Trim().Length > 0)
                                sb.AppendFormat("<span class=\"posttext\">{0}</span>", ent.PostText.Trim());

                            sb.Append("~");
                        }

                        Utils.ParseHelper.ConvertTildesToCommasAndAmpersands(sb);
                    }

                    _altCartOpeners = sb.ToString();
                }

                return _altCartOpeners;
            }
        }

        private string _lst_EventName = null;
        public string lst_EventName
        {
            get
            {
                if (_lst_EventName == null)
                {
                    StringBuilder sb = new StringBuilder();

                    //add the act if not in act mode
                    if (_Config._Site_Entity_Mode == _Enums.SiteEntityMode.Venue ||
                        _Config._Site_Entity_Mode == _Enums.SiteEntityMode.Promoter)
                    {
                        JShowActCollection coll = new JShowActCollection();
                        coll.AddRange(this.JShowActRecords().GetList().FindAll(
                            delegate(JShowAct entity) { return (entity.TopBilling_Effective); }));
                        if (coll.Count > 1)
                            coll.Sort("IDisplayOrder", true);

                        //always display co headlines
                        if (coll.Count > 0)
                        {
                            foreach (JShowAct ent in coll)
                                sb.AppendFormat("{0}~", ent.ActRecord.Name_Displayable);

                            Utils.ParseHelper.ConvertTildesToCommasAndAmpersands(sb);
                        }
                    }

                    //add the venue info if not in venue mode
                    if (_Config._Site_Entity_Mode == _Enums.SiteEntityMode.Act ||
                        _Config._Site_Entity_Mode == _Enums.SiteEntityMode.Promoter)
                    {
                        string state = (this.ShowRecord.VenueRecord.State == null) ? string.Empty : this.ShowRecord.VenueRecord.State.Trim();
                        string city = (this.ShowRecord.VenueRecord.City == null) ? string.Empty : this.ShowRecord.VenueRecord.City.Trim();
                        string divider = (state.Length > 0 && city.Length > 0) ? ", " : string.Empty;
                        string littleAddress = string.Empty;

                        if (state.Trim().Length > 0 || city.Trim().Length > 0)
                            littleAddress = string.Format(" - {0}{1}{2}", city, divider, state);

                        sb.Insert(0, string.Format("{0}{1}", this.ShowRecord.VenueRecord.Name_Displayable, littleAddress));
                    }

                    sb.Insert(0, string.Format("{0} - ", this.DateOfShow.ToString("MM/dd/yy hh:mmtt")));

                    _lst_EventName = sb.ToString();
                }

                return _lst_EventName;
            }
        }

        private string _wc_CartOpeners = null;
        public string wc_CartOpeners
        {
            get
            {
                if (_wc_CartOpeners == null)
                {
                    StringBuilder sb = new StringBuilder();

                    JShowActCollection coll = new JShowActCollection();
                    coll.AddRange(this.JShowActRecords().GetList().FindAll(
                        delegate(JShowAct entity) { return (!entity.TopBilling_Effective); }));
                    if (coll.Count > 1)
                        coll.Sort("IDisplayOrder", true);

                    if (coll.Count > 0)
                    {
                        sb.AppendFormat("<span class=\"with\"> with </span>");
                        foreach (JShowAct ent in coll)
                        {
                            if (ent.PreText != null && ent.PreText.Trim().Length > 0)
                                sb.AppendFormat("<span class=\"pretext\">{0}</span> ", ent.PreText.Trim());

                            sb.AppendFormat("<span class=\"name\">{0}</span> ", ent.ActRecord.Name_Displayable);

                            if (ent.ActText != null && ent.ActText.Trim().Length > 0)
                                sb.AppendFormat("<span class=\"extra\">{0}</span> ", ent.ActText.Trim());

                            if (ent.Featuring != null && ent.Featuring.Trim().Length > 0)
                                sb.AppendFormat("<span class=\"featuring\">{0}</span> ", ent.Featuring.Trim());

                            if (ent.PostText != null && ent.PostText.Trim().Length > 0)
                                sb.AppendFormat("<span class=\"posttext\">{0}</span>", ent.PostText.Trim());

                            sb.Append("~");
                        }

                        Utils.ParseHelper.ConvertTildesToCommasAndAmpersands(sb);
                    }

                    _wc_CartOpeners = sb.ToString();
                }

                return _wc_CartOpeners;
            }
        }
        #endregion

        public bool IsSoldOut 
        { 
            get 
            { 
                return this.ShowRecord.IsSoldOut || (this.ShowStatusRecord.Name.ToLower() == _Enums.ShowDateStatus.SoldOut.ToString().ToLower()); 
            } 
        }

        public bool IsVirtuallySoldOut
        {
            get
            {
                ShowTicketCollection coll = new ShowTicketCollection();
                coll.AddRange(this.ShowTicketRecords().GetList().FindAll(delegate(ShowTicket match) { return (match.IsActive == true); }));

                foreach (ShowTicket st in coll)
                {
                    if (!st.IsSoldOut)
                        return false;
                }

                return (coll.Count > 0);//return false if there are no active shows
            }
        }

        /// <summary>
        /// makes no assumption of the status of the ticket - ticket handles its own status - just the active ones though
        /// </summary>
        public ShowTicketCollection GetDisplayableTickets(_Enums.VendorTypes vendorType, string unlockCode, bool includeLotterySignups, bool includeLotteryFulfillments, ShowTicketCollection existingTix)
        {
            ShowTicketCollection tix = new ShowTicketCollection();

            //must be active -- and announced
            //dont allow moved or cancelled tix
            if (this.ShowRecord.IsActive && this.ShowRecord.AnnounceDate < DateTime.Now && (!this.ShowRecord.IsSoldOut))//worry about sale date when we get down to tickets
            {
                //active             dont show past tickets
                if (this.IsActive && (this.DateOfShow.Date >= DateTime.Now.Date) && (this.ShowStatusRecord.Name.ToLower() == _Enums.ShowDateStatus.OnSale.ToString().ToLower()))
                {
                    tix.AddRange(this.ShowTicketRecords().GetList().FindAll(
                        delegate(ShowTicket match) { return (match.IsAvailableForListing(vendorType, unlockCode, includeLotterySignups, includeLotteryFulfillments, existingTix) == true); }));//can have zero availability or be sold out too
                }
            }

            return tix;
        }
        
        public DateTime GetMinTixKeyStartDate(string mp)
        {
            DateTime start = DateTime.MaxValue;

            foreach (ShowTicket st in this.ShowTicketRecords())
            {
                if (HasAppropriateKey(mp) && start > st.UnlockDate)
                    start = st.UnlockDate;
            }

            return start;
        }

        //we can have the appropriate key on the day of announce or onsale
        //the key must be VALID (see above) to activate sales
        //this is so we can post a different on sale time to the key holders
        public bool HasAppropriateKey(string mp)
        {
            if (mp != null && mp.Length > 0)
            {
                foreach (ShowTicket st in this.ShowTicketRecords())
                {
                    if (st.IsActive && st.IsUnlocked(mp, DateTime.Now))
                        return true;
                }
            }

            return false;
        }

        public string DoorTime { get { return DateOfShow.ToShortTimeString().Replace(" PM", "").Replace(" AM", ""); } }

        //TODO: create a default age
        public string AgesString { get { if (this.AgeRecord != null) return this.AgeRecord.Name; return _Config._Default_Age.Name; } }

        /// <summary>
        /// shows the date of the show as well as showname part from its parent show
        /// </summary>
        public string ListingString { get { return string.Format("[{0}] {1} - {2}", 
            (this.IsActive && this.ShowRecord.IsActive) ? "Active" : "NotActive",
            this.DateOfShow.ToString("ddd MM/dd/yyyy hh:mmtt"), 
            this.ShowRecord.ShowNamePartCondense); } }
    }

    public partial class ShowDateCollection
    {
        public void SortBy_DateToOrderBy()
        {
            if(this != null && this.Count > 1)
                this.GetList().Sort(delegate(ShowDate x, ShowDate y) { return (x.DateOfShow_ToSortBy.CompareTo(y.DateOfShow_ToSortBy)); });        
        }
    }
}
