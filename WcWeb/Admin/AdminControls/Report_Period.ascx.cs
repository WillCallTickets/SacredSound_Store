using System;
using System.Web.UI.WebControls;

using Wcss.QueryRow;

namespace WillCallWeb.Admin.AdminControls
{
    public partial class Report_Period : BaseControl
    {
        //list all tix - paging for more than 20 tix?
        //need to show ticket date/name
        //iAllotment, --ending, iSold, iAvailable
        //past 5 days sales?

        protected int _numItems = 0;
        protected decimal _basePriceTotal = 0;
        protected decimal _serviceChargeTotal = 0;
        protected decimal _lineItemTotal = 0;

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
                cal.SelectedDate = DateTime.Parse(DateTime.Now.ToString("MM/1/yyyy"));
            else
                cal.SelectedDate = DateTime.Parse(DateTime.Now.ToString("MM/1/yyyy")).AddMonths(1).AddMinutes(-1);
            
            //DateTime.Now.Date.AddMonths(1).AddDays(1).AddMinutes(-1);

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

        protected void litTixInPeriod_DataBinding(object sender, EventArgs e)
        {
            Literal lit = (Literal)sender;
            lit.Text = Wcss.QueryRow.NumTixInPeriodRow.GetNumberOfTicketsInPeriod(clockStart.SelectedDate, clockEnd.SelectedDate).ToString();
        }
        protected void GridView1_DataBound(object sender, EventArgs e)
        {
            //get from sp
            litTixInPeriod.DataBind();
        }
        protected void GridView1_DataBinding(object sender, EventArgs e)
        {
            litTixInPeriod.Text = string.Empty;

            _numItems = 0;
            _basePriceTotal = 0;
            _serviceChargeTotal = 0;
            _lineItemTotal = 0;
        }
        protected void GridView1_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            GridView grid = (GridView)sender;

            if (e.Row.DataItem != null)
            {
                ServiceFeeBreakdownRow ent = (ServiceFeeBreakdownRow)e.Row.DataItem;

                if (ent != null)
                {
                    _numItems += ent.NumItems;
                    _basePriceTotal += ent.BasePriceTotal;
                    _serviceChargeTotal += ent.ServiceChargeTotal;
                    _lineItemTotal += ent.LineItemTotal;
                }
            }
        }

        #endregion

    }
}
