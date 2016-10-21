using System;
using System.Collections.Generic;
using System.Text;
using System.Data;


namespace Wcss.QueryRow
{
    [Serializable]
    public partial class NumTixInPeriodRow
    {
        private int _numItems;
        public int NumItems { get { return _numItems; } set { _numItems = value; } }

        public NumTixInPeriodRow(IDataReader dr)
        {
            NumItems = (int)dr.GetValue(dr.GetOrdinal("NumItems"));
        }
        /// <summary>
        /// Returns the number of tickets sold within a given period for shows with that same period
        /// </summary>
        /// <returns></returns>
        public static int GetNumberOfTicketsInPeriod(DateTime startDate, DateTime endDate)
        {
            startDate = startDate.Date;
            endDate = endDate.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

            List<NumTixInPeriodRow> list = new List<NumTixInPeriodRow>();

            using (IDataReader dr = SPs.TxReportNumberOfTicketsInPeriodForShowsInPeriod(_Config.APPLICATION_ID, 
                    startDate.ToString("MM/dd/yyyy hh:mmtt"), endDate.ToString("MM/dd/yyyy hh:mmtt")).GetReader())
            {
                while (dr.Read())
                {
                    NumTixInPeriodRow tsr = new NumTixInPeriodRow(dr);

                    list.Add(tsr);
                }

                dr.Close();
            }

            if (list.Count > 0)
                return list[0].NumItems;
            else return -1;
        }
    }
}
