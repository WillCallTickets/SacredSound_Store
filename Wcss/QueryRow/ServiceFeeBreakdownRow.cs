using System;
using System.Collections.Generic;
using System.Text;
using System.Data;


namespace Wcss.QueryRow
{
    [Serializable]
    public partial class ServiceFeeBreakdownRow
    {
        private decimal _serviceCharge;
        private int _numItems;
        private decimal _basePriceTotal;
        private decimal _serviceChargeTotal;
        private decimal _lineItemTotal;

        public decimal ServiceCharge { get { return _serviceCharge; } set { _serviceCharge = value; } }
        public int NumItems { get { return _numItems; } set { _numItems = value; } }
        public decimal BasePriceTotal { get { return _basePriceTotal; } set { _basePriceTotal = value; } }
        public decimal ServiceChargeTotal { get { return _serviceChargeTotal; } set { _serviceChargeTotal = value; } }
        public decimal LineItemTotal { get { return _lineItemTotal; } set { _lineItemTotal = value; } }

        public ServiceFeeBreakdownRow(IDataReader dr)
        {
            ServiceCharge = (decimal)dr.GetValue(dr.GetOrdinal("ServiceCharge"));
            NumItems = (int)dr.GetValue(dr.GetOrdinal("NumItems"));
            BasePriceTotal = (decimal)dr.GetValue(dr.GetOrdinal("BasePriceTotal"));
            ServiceChargeTotal = (decimal)dr.GetValue(dr.GetOrdinal("ServiceChargeTotal"));
            LineItemTotal = (decimal)dr.GetValue(dr.GetOrdinal("LineItemTotal"));
        }

        public static List<ServiceFeeBreakdownRow> GetServiceFeeBreakdownInPeriod(DateTime startDate, DateTime endDate)
        {
            startDate = startDate.Date;
            endDate = endDate.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

            List<ServiceFeeBreakdownRow> list = new List<ServiceFeeBreakdownRow>();

            using (IDataReader dr = SPs.TxReportServiceFeeBreakdownInPeriod(_Config.APPLICATION_ID, 
                    startDate.ToString("MM/dd/yyyy hh:mmtt"), endDate.ToString("MM/dd/yyyy hh:mmtt")).GetReader())
            {
                while (dr.Read())
                {
                    ServiceFeeBreakdownRow tsr = new ServiceFeeBreakdownRow(dr);

                    list.Add(tsr);
                }

                dr.Close();
            }

            return list;
        }
    }
}
