using System;
using System.Web.UI.WebControls;

namespace WillCallWeb.Admin.AdminControls
{
    public partial class Report_SalesAll : BaseControl
    {
        //get report for current month
        protected Wcss.QueryRow.SalesReportMain _report = null;

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
                BindReport();
            }
        }

        private void BindReport()
        {
            _report = new Wcss.QueryRow.SalesReportMain(clockStart.SelectedDate, DateTime.Parse(clockEnd.SelectedDate.ToString("MM/dd/yyyy 23:59")), "rpttable");

            pnlReport.DataBind();
        }
        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            BindReport();
        }

        #region Calendars

        protected void clock_Init(object sender, EventArgs e)
        {
            WillCallWeb.Components.Util.CalendarClock cal = (WillCallWeb.Components.Util.CalendarClock)sender;

            if (cal.ID.ToLower().IndexOf("start") != -1)//set to first of month
                cal.SelectedDate = DateTime.Parse(string.Format("{0}/1/{1} 12AM", DateTime.Now.Month.ToString(), DateTime.Now.Year.ToString()));
            else
                cal.SelectedDate = DateTime.Now.Date.AddDays(1).AddMinutes(-1);

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

            BindReport();
        }
        private void EnsureValidCalendarSelection()
        {
            DateTime startClock = clockStart.SelectedDate;
            DateTime endClock = clockEnd.SelectedDate;

            if (endClock < startClock)
                clockEnd.SelectedDate = startClock.AddDays(1);
        }

        #endregion

        protected void pnlReport_DataBinding(object sender, EventArgs e)
        {
            Panel p = (Panel)sender;

            p.Controls.Clear();
            p.Controls.Add(_report.ReportTable);
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            //TODO: send the report table to a print page

        }
        protected void btnCsv_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            string fileAttachmentName = string.Empty;

            if(_report == null)
                _report = new Wcss.QueryRow.SalesReportMain(
                    clockStart.SelectedDate, DateTime.Parse(clockEnd.SelectedDate.ToString("MM/dd/yyyy 23:59")), 
                    "rpttable");

            if (_report != null)
            {
                string filename = string.Format("Sales_Report_{0}_{1}", 
                    _report.StartDate.ToString("yyyyMMddhhmmtt"), _report.EndDate.ToString("yyyyMMddhhmmtt"));

                fileAttachmentName = string.Format("attachment; filename={0}.csv",
                    Utils.ParseHelper.StripInvalidChars_Filename(filename));

                System.Text.StringBuilder sb = new System.Text.StringBuilder();

                Table t = _report.ReportTable;
                foreach (TableRow tr in t.Rows)
                {
                    int cellNum = 1;

                    foreach (TableCell td in tr.Cells)
                    {
                        sb.AppendFormat(Utils.ParseHelper.StripHtmlTags(td.Text).Replace(",", " ").Replace("&nbsp;", string.Empty));

                        if (cellNum < tr.Cells.Count)
                            sb.Append(",");

                        cellNum++;
                    }

                    sb.AppendLine();
                }


                Utils.FileLoader.CSV_WriteToContextForDownload(sb, fileAttachmentName, null);
            }
        }
    }
}
