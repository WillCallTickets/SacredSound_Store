using System;
using System.Web.UI.WebControls;

using Wcss.QueryRow;

namespace WillCallWeb.Admin.AdminControls
{
    public partial class Report_TixCounts : BaseControl
    {
        //list all tix - paging for more than 20 tix?
        //need to show ticket date/name
        //iAllotment, --ending, iSold, iAvailable
        //past 5 days sales?

        protected int _tAllot = 0;
        protected int _tPend = 0;
        protected int _tSold = 0;
        protected int _tAvail = 0;
        protected int _tRefund = 0;
        protected int _5day = 0;
        protected int _4day = 0;
        protected int _3day = 0;
        protected int _2day = 0;
        protected int _1day = 0;
        protected int _today = 0;

        #region New paging

        bool isSelectCount;
        protected void objData_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            isSelectCount = e.ExecutingSelectCount;

            if (!isSelectCount)
            {
                e.Arguments.StartRowIndex = (GridView1.PageIndex * GridView1.PageSize) + 1;
                e.Arguments.MaximumRows = GridView1.PageSize;
            }
        }
        protected void objData_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (isSelectCount && e.ReturnValue != null && e.ReturnValue.GetType().Name == "Int32")
            {
                GooglePager1.DataSetSize = (int)e.ReturnValue;
            }
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            GooglePager1.GooglePagerChanged += new WillCallWeb.Components.Navigation.gglPager.GooglePagerChangedEvent(GooglePager1_GooglePagerChanged);
        }
        public override void Dispose()
        {
            GooglePager1.GooglePagerChanged += new WillCallWeb.Components.Navigation.gglPager.GooglePagerChangedEvent(GooglePager1_GooglePagerChanged);
            base.Dispose();
        }
        protected void GooglePager1_GooglePagerChanged(object sender, WillCallWeb.Components.Navigation.gglPager.GooglePagerEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            Atx.adminPageSize = e.NewPageSize;
            GridView1.PageSize = Atx.adminPageSize;
        }
        protected void GridView1_Init(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;

            GooglePager1.PageSize = Atx.adminPageSize;
            grid.PageSize = GooglePager1.PageSize;
            grid.PageIndex = GooglePager1.PageIndex;
        }
        protected void GridView1_DataBound(object sender, EventArgs e)
        {
            GooglePager1.DataBind();
        }

        #endregion

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            //set opacity for nav events
            if (this.HasControls() && this.UpdatePanel1.Visible)
                Atx.RegisterJQueryScript_BlockUI_AjaxMethod(this.UpdatePanel1, "#report", true);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //GooglePager1.PageSize = 50;
            }
        }
        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            GridView1.DataBind();
        }

        #region Calendars

        protected void clock_Init(object sender, EventArgs e)
        {
            WillCallWeb.Components.Util.CalendarClock cal = (WillCallWeb.Components.Util.CalendarClock)sender;

            if (cal.ID.ToLower().IndexOf("start") != -1)
                cal.SelectedDate = DateTime.Now.AddDays(-DateTime.Now.Day + 1 - 3).Date;//TODO remove the plus 3
            else
                cal.SelectedDate = DateTime.Now.Date.AddMonths(6).AddDays(1).AddMinutes(-1);

            cal.UseTime = false;
        }
        protected void clock_SelectedDateChanged(object sender, WillCallWeb.Components.Util.CalendarClock.CalendarClockChangedEventArgs e)
        {
            WillCallWeb.Components.Util.CalendarClock cal = (WillCallWeb.Components.Util.CalendarClock)sender;

            if (cal.ID.ToLower().IndexOf("start") != -1)
                clockStart.SelectedDate = e.ChosenDate;
            else
                clockEnd.SelectedDate = e.ChosenDate;

            EnsureValidCalendarSelection();

            //GridView1.DataBind();
        }
        private void EnsureValidCalendarSelection()
        {
            DateTime startClock = clockStart.SelectedDate;
            DateTime endClock = clockEnd.SelectedDate;

            if (endClock < startClock)
                clockEnd.SelectedDate = startClock.AddDays(1);
        }

        #endregion

        #region GridView

        protected void GridView1_DataBinding(object sender, EventArgs e)
        {
            _tAllot = 0;
            _tPend = 0;
            _tSold = 0;
            _tAvail = 0;
            _tRefund = 0;
            _5day = 0;
            _4day = 0;
            _3day = 0;
            _2day = 0;
            _1day = 0;
            _today = 0;
        }
        protected void GridView1_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            GridView grid = (GridView)sender;

            if (e.Row.DataItem != null)
            {
                TicketCountRow ent = (TicketCountRow)e.Row.DataItem;
                if (ent != null)
                {
                    Literal name = (Literal)e.Row.FindControl("litName");
                    if (name != null)
                        name.Text = (Wcss._Config._Site_Entity_Mode == Wcss._Enums.SiteEntityMode.Venue) ?
                            (ent.ShowName.ToLower().StartsWith(Wcss._Config._Default_VenueName.ToLower())) ?
                            ent.ShowName.Substring(Wcss._Config._Default_VenueName.Length + 3) : ent.ShowName//allows for <space><dash><space> at end of venue name
                            : ent.ShowName;

                    _tAllot += ent.Allotment;
                    _tPend += 0;//#w ent.Pending;
                    _tSold += ent.Sold;
                    _tAvail += 0;//#w ent.Available;
                    _tRefund += ent.Refunded;
                    _5day += ent.FiveDay;
                    _4day += ent.FourDay;
                    _3day += ent.ThreeDay;
                    _2day += ent.TwoDay;
                    _1day += ent.OneDay;
                    _today += ent.ToDay;
                }
            }
        }

        #endregion

    }
}
