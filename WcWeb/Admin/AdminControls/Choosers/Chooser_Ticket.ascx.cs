using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using Wcss;

namespace WillCallWeb.Admin.AdminControls.Choosers
{
    public partial class Chooser_Ticket : BaseControl
    {
        List<string> _errors = new List<string>();

        public int SelectedValue
        {
            get
            {
                return int.Parse(ddlTickets.SelectedValue);
            }
        }

        #region Page Overhead

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
            }
        }

        #endregion

        #region Calendar for baseline

        protected void clock_Init(object sender, EventArgs e)
        {
            WillCallWeb.Components.Util.CalendarClock cal = (WillCallWeb.Components.Util.CalendarClock)sender;

            cal.SelectedDate = DateTime.Now.AddDays(-7).Date.AddHours(_Config.DayTurnoverTime);
        }

        #endregion

        protected void SqlDates_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            WillCallWeb.Components.Util.CalendarClock cal = clockBaseline;

            //e.Command.Parameters["@DateBaseline"].Value = cal.SelectedDate;
        }
}
}
