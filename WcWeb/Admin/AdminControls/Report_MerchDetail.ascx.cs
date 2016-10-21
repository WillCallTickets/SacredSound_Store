using System;
using System.Web.UI.WebControls;

using Wcss.QueryRow;

namespace WillCallWeb.Admin.AdminControls
{
    public partial class Report_MerchDetail : BaseControl
    {
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

        protected string _divHolder;

        protected void GridView1_DataBinding(object sender, EventArgs e)
        {
            _divHolder = string.Empty;
        }
        protected void GridView1_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            GridView grid = (GridView)sender;

            if (e.Row.DataItem != null)
            {
                MerchSalesDetailRow ent = (MerchSalesDetailRow)e.Row.DataItem;
                Literal lit = (Literal)e.Row.FindControl("litDivision");

                if (ent != null && lit != null && _divHolder != ent.DivName)
                {
                    _divHolder = ent.DivName;
                    lit.Text = _divHolder;
                }
            }
        }

        #endregion

    }
}
