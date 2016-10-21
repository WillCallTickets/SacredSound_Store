using System;
using System.Collections.Generic;
using System.Data;

namespace Wcss.QueryRow
{
    [Serializable]
    public partial class TicketCountRow
    {
        private int _showDateId;
        private DateTime _showDate;
        private string _showName;
        private int _5day = 0;
        private int _4day = 0;
        private int _3day = 0;
        private int _2day = 0;
        private int _1day = 0;
        private int _today = 0;
        private int _allotment = 0;
        private int _pending = 0;
        private int _sold = 0;
        private int _available = 0;
        private int _refunded = 0;

        public int ShowDateId { get { return _showDateId; } set { _showDateId = value; } }
        public DateTime ShowDate { get { return _showDate; } set { _showDate = value; } }
        public string ShowName { get { return _showName; } set { _showName = value; } }
        public int FiveDay { get { return _5day; } set { _5day = value; } }
        public int FourDay { get { return _4day; } set { _4day = value; } }
        public int ThreeDay { get { return _3day; } set { _3day = value; } }
        public int TwoDay { get { return _2day; } set { _2day = value; } }
        public int OneDay { get { return _1day; } set { _1day = value; } }
        public int ToDay { get { return _today; } set { _today = value; } }
        public int Allotment { get { return _allotment; } set { _allotment = value; } }
        public int Pending { get { return _pending; } set { _pending = value; } }
        public int Sold { get { return _sold; } set { _sold = value; } }
        public int Available { get { return _available; } set { _available = value; } }
        public int Refunded { get { return _refunded; } set { _refunded = value; } }

        public TicketCountRow(IDataReader dr)
        {
            ShowDateId = (int)dr.GetValue(dr.GetOrdinal("ShowDateId"));
            ShowDate = (DateTime)dr.GetValue(dr.GetOrdinal("ShowDate"));
            ShowName = dr.GetValue(dr.GetOrdinal("ShowName")).ToString();
            FiveDay = (int)dr.GetValue(dr.GetOrdinal("_5"));
            FourDay = (int)dr.GetValue(dr.GetOrdinal("_4"));
            ThreeDay = (int)dr.GetValue(dr.GetOrdinal("_3"));
            TwoDay = (int)dr.GetValue(dr.GetOrdinal("_2"));
            OneDay = (int)dr.GetValue(dr.GetOrdinal("_1"));
            ToDay = (int)dr.GetValue(dr.GetOrdinal("today"));
            Allotment = (int)dr.GetValue(dr.GetOrdinal("allotment"));
            Pending = (int)dr.GetValue(dr.GetOrdinal("pending"));
            Sold = (int)dr.GetValue(dr.GetOrdinal("sold"));
            Available = (int)dr.GetValue(dr.GetOrdinal("available"));
            Refunded = (int)dr.GetValue(dr.GetOrdinal("refunded"));
        }

        public static List<TicketCountRow> GetTicketCounts(DateTime startDate, DateTime endDate, int startRowIndex, int maximumRows)
        {
            startDate = startDate.Date;
            endDate = endDate.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

            List<TicketCountRow> list = new List<TicketCountRow>();

            using (IDataReader dr = SPs.TxGetTicketCounts(_Config.APPLICATION_ID, startDate.ToString("MM/dd/yyyy hh:mmtt"), endDate.ToString("MM/dd/yyyy hh:mmtt"), 
                startRowIndex, maximumRows).GetReader())
            {
                while (dr.Read())
                {
                    TicketCountRow tsr = new TicketCountRow(dr);

                    list.Add(tsr);
                }

                dr.Close();
            }

            return list;
        }

        public static int GetTicketCountsCount(DateTime startDate, DateTime endDate)
        {
            startDate = startDate.Date;
            endDate = endDate.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

            int count = 0;

            using (IDataReader dr = SPs.TxGetTicketCountsCount(_Config.APPLICATION_ID, startDate.ToString("MM/dd/yyyy hh:mmtt"), endDate.ToString("MM/dd/yyyy hh:mmtt")).GetReader())
            {
                while (dr.Read())
                {
                    count = (int)dr.GetValue(0);
                }

                dr.Close();
            }

            return count;
        }
    }
}
