using System;
using System.Collections.Generic;
using System.Text;
using System.Data;


namespace Wcss.QueryRow
{
    [Serializable]
    public partial class MerchSalesDetailRow
    {
        public string DivName { get; set; }
        public string CatName { get; set; }

        public int NumItemsSold { get; set; }
        public decimal TotalSales { get; set; }

        public MerchSalesDetailRow(IDataReader dr)
        {
            DivName = dr.GetValue(dr.GetOrdinal("DivName")).ToString();
            CatName = dr.GetValue(dr.GetOrdinal("CatName")).ToString();
            NumItemsSold = (int)dr.GetValue(dr.GetOrdinal("NumItemsSold"));
            TotalSales = (decimal)dr.GetValue(dr.GetOrdinal("TotalSales"));
        }

        public static List<MerchSalesDetailRow> GetMerchSalesDetailInPeriod(DateTime startDate, DateTime endDate)
        {
            startDate = startDate.Date;
            endDate = endDate.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

            List<MerchSalesDetailRow> list = new List<MerchSalesDetailRow>();

            using (IDataReader dr = SPs.TxReportMerchSalesDetailInPeriod(_Config.APPLICATION_ID,
                    startDate.ToString("MM/dd/yyyy hh:mmtt"), endDate.ToString("MM/dd/yyyy hh:mmtt")).GetReader())
            {
                while (dr.Read())
                {
                    MerchSalesDetailRow tsr = new MerchSalesDetailRow(dr);

                    list.Add(tsr);
                }

                dr.Close();
            }

            return list;
        }
    }
}
