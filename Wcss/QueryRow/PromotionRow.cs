using System;
using System.Collections.Generic;
using System.Data;

namespace Wcss.QueryRow
{
    public class PromotionRow
    {
        private int _id = 0;
        private bool _isActive = false;
        private string _name = null;
        private string _displayText = null;
        private string _additionalText = null;
        private string _bannerUrl = null;
        private DateTime _startDate = DateTime.MinValue;
        private DateTime _endDate = DateTime.MaxValue;

        public int Id { get { return _id; } set { _id = value; } }
        public bool IsActive { get { return _isActive; } set { _isActive = value; } }
        public string Name { get { return _name; } set { _name = value; } }
        public string DisplayText { get { return _displayText; } set { _displayText = value; } }
        public string AdditionalText { get { return _additionalText; } set { _additionalText = value; } }
        public string BannerUrl { get { return _bannerUrl; } set { _bannerUrl = value; } }
        public DateTime StartDate { get { return _startDate; } set { _startDate = value; } }
        public DateTime EndDate { get { return _endDate; } set { _endDate = value; } }

        //public PromotionRow() {}

        public PromotionRow(IDataReader dr)
        {
            Id = (int)dr.GetValue(dr.GetOrdinal("Id"));
            IsActive = (bool)dr.GetValue(dr.GetOrdinal("IsActive"));
            Name = dr.GetValue(dr.GetOrdinal("Name")).ToString();
            DisplayText = dr.GetValue(dr.GetOrdinal("DisplayText")).ToString();
            AdditionalText = dr.GetValue(dr.GetOrdinal("AdditionalText")).ToString();
            BannerUrl = dr.GetValue(dr.GetOrdinal("BannerUrl")).ToString();
            string start = dr.GetValue(dr.GetOrdinal("StartDate")).ToString();
            if(start.Trim().Length > 0)
                StartDate = (DateTime)dr.GetValue(dr.GetOrdinal("StartDate"));
            string end = dr.GetValue(dr.GetOrdinal("EndDate")).ToString();
            if (end.Trim().Length > 0)
                EndDate = (DateTime)dr.GetValue(dr.GetOrdinal("EndDate"));
        }

        public static List<PromotionRow> GetPromotionRows(bool bannersOnly, int startRowIndex, int maximumRows)
        {
            List<PromotionRow> list = new List<PromotionRow>();

            using (IDataReader dr = SPs.TxGetPromotionRows(_Config.APPLICATION_ID, bannersOnly, startRowIndex, maximumRows).GetReader())
            {
                while (dr.Read())
                {
                    PromotionRow tsr = new PromotionRow(dr);

                    list.Add(tsr);
                }

                dr.Close();
            }

            return list;
        }

        public static int GetPromotionRowsCount(bool bannersOnly)
        {
            int count = 0;

            using (IDataReader dr = SPs.TxGetPromotionRowsCount(_Config.APPLICATION_ID, bannersOnly).GetReader())
            {
                while (dr.Read())
                {
                    count = (int)dr.GetValue(0);
                }

                dr.Close();
            }

            return count;
        }


        /*
        SELECT banner.Id, banner.bActive, banner.Name, ISNULL(banner.DisplayText,'') as 'DisplayText', ISNULL(banner.AdditionalText,'') as 'AdditionalText', 
    ISNULL(banner.BannerUrl,'') as 'BannerUrl', banner.dtStartDate, banner.dtEndDate 
    FROM [SalePromotion] banner WHERE (banner.tRequiredParentMerchId IS NULL OR banner.tRequiredParentMerchId = 0) AND 
    (banner.tRequiredParentShowTicketId IS NULL OR banner.tRequiredParentShowTicketId = 0) AND banner.mDiscountAmount <= 0 AND banner.iDiscountPercent <= 0 ORDER BY banner.Id DESC"
    */

    }
}