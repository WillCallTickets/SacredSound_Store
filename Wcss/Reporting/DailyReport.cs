using System;
using System.Collections.Generic;
using System.Data;

namespace Wcss.Reporting
{
    public class SummaryReport
    {
        /// <summary>
        /// This will create a weekly report
        /// </summary>
        public SummaryReport() {}
        public SummaryReport(DateTime startDate, DateTime endDate)
        {
        }
    }

    public class DailyReport
    {
        /// <summary>
        /// This will create a daily report
        /// </summary>
        public DailyReport() {}
        public DailyReport(DateTime startDate)
        {
        }
    }
}