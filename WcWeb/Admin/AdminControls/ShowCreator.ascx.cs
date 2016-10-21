using System;
using System.Web.UI.WebControls;
using Wcss;

namespace WillCallWeb.Admin
{
    public partial class ShowCreator : BaseControl
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            clockFirstDate.SelectedDateChanged += new WillCallWeb.Components.Util.CalendarClock.SelectedDateChangedEventHandler(clockFirstDate_SelectedDateChanged);
            Editor_Act1.SelectedActChanged += new WillCallWeb.Admin.AdminControls.Editor_Act.SelectedEditActChangedEventHandler(Editor_SelectedChanged);
            Editor_Venue1.SelectedVenueChanged += new WillCallWeb.Admin.AdminControls.Editor_Venue.SelectedEditVenueChangedEventHandler(Editor_SelectedChanged);
        }
        public override void Dispose()
        {
            clockFirstDate.SelectedDateChanged -= new WillCallWeb.Components.Util.CalendarClock.SelectedDateChangedEventHandler(clockFirstDate_SelectedDateChanged);
            Editor_Act1.SelectedActChanged -= new WillCallWeb.Admin.AdminControls.Editor_Act.SelectedEditActChangedEventHandler(Editor_SelectedChanged);
            Editor_Venue1.SelectedVenueChanged -= new WillCallWeb.Admin.AdminControls.Editor_Venue.SelectedEditVenueChangedEventHandler(Editor_SelectedChanged);
            base.Dispose();
        }
        protected void clockFirstDate_SelectedDateChanged(object sender, WillCallWeb.Components.Util.CalendarClock.CalendarClockChangedEventArgs e)
        {
            btnAdd.DataBind();
        }
        protected void EditorAct_Init(EventArgs e)
        {   
            base.OnInit(e);
            Editor_Act1.SelectedActChanged += new WillCallWeb.Admin.AdminControls.Editor_Act.SelectedEditActChangedEventHandler(Editor_SelectedChanged);
        }
        protected void EditorAct_Dispose()
        {   
            Editor_Act1.SelectedActChanged -= new WillCallWeb.Admin.AdminControls.Editor_Act.SelectedEditActChangedEventHandler(Editor_SelectedChanged);
            base.Dispose();
        }
        protected void Editor_SelectedChanged(object sender, WillCallWeb.Admin.AdminEvent.EditorEntityChangedEventArgs e)
        {
            btnAdd.DataBind();
        }
        protected void btnAdd_DataBinding(object sender, EventArgs e)
        {
            btnAdd.Enabled = (Editor_Act1.SelectedIdx > 0) && (Editor_Venue1.SelectedIdx > 0) && 
                (clockFirstDate.SelectedDate != clockFirstDate.DefaultValue);
        }        
        protected void Page_Load(object sender, EventArgs e)
        {
            SetInitialAMPMs();

            if (!IsPostBack)
            {
                Atx.CurrentActId = 0;
                Atx.CurrentVenueId = 0;
            }

            if (!IsPostBack && _Config._Site_Entity_Mode == _Enums.SiteEntityMode.Venue && _Config._Site_Entity_Name != null)
            {
                Venue v = new Venue();
                v.LoadAndCloseReader(Venue.FetchByParameter("Name", _Config._Default_VenueName));
                if (v != null && v.Id > 0)
                    Editor_Venue1.SelectedIdx = v.Id;
            }

            ddlAges.DataBind();
            btnAdd.DataBind();
        }

        private void SetInitialAMPMs()
        {
            if (!IsPostBack)
            {
                clockAnnounce.AMPM.SelectedIndex = -1;
                ListItem lia = clockAnnounce.AMPM.Items.FindByText("AM");
                if (lia != null)
                    lia.Selected = true;

                clockOnsale.AMPM.SelectedIndex = -1;
                ListItem lis = clockOnsale.AMPM.Items.FindByText("AM");
                if (lis != null)
                    lis.Selected = true;
            }
        }
    

        #region Add Show

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    #region Validate date, time, act and venue input

                    DateTime dos = clockFirstDate.SelectedDate;

                    if(dos.Year < 1990)
                        throw new Exception("Please select a valid year.");

                    //DateTime timeDate = DateTime.Parse(string.Format("{0}", dos.Date.ToString("MM/dd/yyyy HH:mm tt")));

                    string timeString = clockShowTime.SelectedDate.ToString("hh:mm tt"); 
                    string showtime = (chkTba.Checked) ? "TBA" : timeString;

                    DateTime announce = clockAnnounce.SelectedDate;
                    DateTime onsale = clockOnsale.SelectedDate;

                    if (ddlAges.SelectedValue == string.Empty)
                        throw new Exception("Please select an age classification.");

                    if (Editor_Act1.SelectedIdx == 0)
                        throw new Exception("Please select an act.");

                    if (Editor_Venue1.SelectedIdx == 0)
                        throw new Exception("Please select a venue.");

                    #endregion

                    #region Create the show

                    Show show = new Show();
                    show.ApplicationId = _Config.APPLICATION_ID;
                    show.DtStamp = DateTime.Now;

                    Venue venue = new Venue(Editor_Venue1.SelectedIdx);
                    show.VenueRecord = venue;

                    Act act = new Act(Editor_Act1.SelectedIdx);
                    ActCollection acts = new ActCollection();
                    acts.Add(act);

                    show.Name = Show.CalculatedShowName(dos, venue, acts);
                    //show.ResetCalculatedActName();

                    if (announce > Utils.Constants._MinDate)
                        show.AnnounceDate = announce;
                    if (onsale > Utils.Constants._MinDate)
                        show.DateOnSale = onsale;

                    show.IsActive = true;
                    show.Save();

                    #endregion

                    #region Create the First Show Date

                    //SHOWDATE
                    ShowDate sd = new ShowDate();
                    sd.DtStamp = DateTime.Now;

                    Age age = _Lookits.Ages.GetList().Find(delegate(Age match) { return (match.Name.ToLower() == ddlAges.SelectedValue.ToLower()); });
                    if (age == null)
                        age = _Config._Default_Age;

                    sd.AgeRecord = age;

                    sd.ShowRecord = show;
                    sd.DateOfShow = DateTime.Parse(dos.ToString("yyyy-MM-dd hh:mmtt"));
                    sd.ShowTime = showtime;
                    sd.IsAutoBilling = true;
                    sd.Billing = null;
                    sd.IsActive = true;

                    ShowStatus newStat = _Lookits.ShowStatii.GetList().Find(delegate(ShowStatus match) { return (match.Name == "OnSale"); });
                    if(newStat != null)
                        sd.ShowStatusRecord = newStat;

                    sd.Save();

                    #endregion

                    #region Create the headline act

                    //ADD ACT
                    JShowAct showAct = new JShowAct();
                    showAct.DtStamp = DateTime.Now;

                    showAct.TShowDateId = sd.Id;
                    showAct.TActId = act.Id;
                    showAct.DisplayOrder = sd.JShowActRecords().Count;//should be zero
                    showAct.TopBilling = true;

                    sd.JShowActRecords().Add(showAct);
                    sd.JShowActRecords().SaveAll();

                    #endregion

                    //let parent handle redirection
                    AdminEvent.OnShowChosen(this, show.Id);

                }
                catch (System.Threading.ThreadAbortException) { }
                catch (Exception ex)
                {
                    _Error.LogException(ex);
                    CustomValidation.IsValid = false;
                    CustomValidation.ErrorMessage = ex.Message;
                }
            }
        }

        #endregion

        #region Ages

        protected void ddlAges_DataBinding(object sender, EventArgs e)
        {
            if (ddlAges.Items.Count == 0)
                ddlAges.DataSource = _Lookits.Ages;
        }
        protected void ddlAges_DataBound(object sender, EventArgs e)
        {
            if (!IsPostBack && ddlAges.Items.Count > 0)
            {
                int sel = _Lookits.Ages.GetList().FindIndex(delegate(Age match) { return (match.Name.ToLower() == _Config._Default_Age.Name.ToLower()); });
                ddlAges.SelectedIndex = sel;
            }
        }

        #endregion
    }
}
