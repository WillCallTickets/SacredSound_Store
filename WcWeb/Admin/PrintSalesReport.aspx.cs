using System;
using System.Data.SqlTypes;

using Wcss;
using Wcss.QueryRow;

namespace WillCallWeb.Admin
{
    public partial class PrintSalesReport : WillCallWeb.BasePage
    {
        protected override void OnPreInit(EventArgs e)
        {
            QualifySsl(false);
            base.OnPreInit(e);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetInputs();
                SetupReport();
            }
        }
        protected void SetupReport()
        {
            Wcss.QueryRow.SalesReportMain _report = new Wcss.QueryRow.SalesReportMain(StartDate, EndDate, "rpttable");

            pnlSales.Controls.Clear();
            pnlSales.Controls.Add(_report.ReportTable);
        }

        #region Set Inputs

        protected DateTime StartDate = (DateTime)SqlDateTime.MinValue;
        protected DateTime EndDate = (DateTime)SqlDateTime.MaxValue;

        private void SetInputs()
        {
            //dates have precedence
            string start = Request.QueryString["start"];
            string end = Request.QueryString["end"];

            if (start != null)
                StartDate = new DateTime(long.Parse(start));
            if (end != null)
                EndDate = new DateTime(long.Parse(end));
        }

        #endregion

}
}