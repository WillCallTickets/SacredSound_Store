using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Wcss.QueryRow
{
    public class SalesReportBundleRow
    {
        public static List<SalesReportBundleRow> GetBundle_CSVReport(string category, bool activeStatus, DateTime startDate, DateTime endDate)
        {
            return GetBundleReportRows(category, activeStatus, startDate, endDate, 0, 100000);
        }
        public static List<SalesReportBundleRow> GetBundleReportRows(string category, bool activeStatus, DateTime startDate, DateTime endDate, int startRowIndex, int maximumRows)
        {
            startDate = startDate.Date;
            endDate = endDate.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

            List<SalesReportBundleRow> list = new List<SalesReportBundleRow>();

            using (IDataReader dr = SPs.TxReportMerchBundleDetailInPeriod(_Config.APPLICATION_ID, category, activeStatus.ToString(),
                startDate.ToString("MM/dd/yyyy hh:mmtt"), endDate.ToString("MM/dd/yyyy hh:mmtt"),
                startRowIndex, maximumRows, InvoiceItem.MerchBundleIdConstant).GetReader())
            {
                while (dr.Read())
                {
                    SalesReportBundleRow _row = new SalesReportBundleRow(dr);
                    list.Add(_row);
                }
                dr.Close();
            }

            return list;
        }
        public static int GetBundleReportRows_Count(string category, bool activeStatus, DateTime startDate, DateTime endDate)
        {
            //Note that invoice can be listed in both categories - depends on items in invoice
            int count = 0;

            using (IDataReader dr = SPs.TxReportMerchBundleDetailInPeriodCount(_Config.APPLICATION_ID, category, activeStatus.ToString()).GetReader())
            {
                while (dr.Read())
                {
                    count = (int)dr.GetValue(0);
                }

                dr.Close();
            }

            return count;
        }

    
        private int     _bundleId = 0;
        private bool    _active = false;
        private int?    _tMerchId = null;
        private int?    _tShowTicketId = null;
        private string  _parentDescription = null;
        private string  _title = null;
        private string  _comment = null;
        private int     _requiredParentQty = 0;
        private int     _maxSelections = 0;
        private decimal _price = 0;
        private bool    _includeWeight = false;
        private int     _numBundlesSold = 0;
        private decimal _bundleSales = 0; 
        private int     _numBundlesRefunded = 0;
        private decimal _bundleRefunds = 0; 
        private int     _numItemsSold = 0;
        private int     _numItemsRefunded = 0;

        public int BundleId             { get { return _bundleId; } set { _bundleId = value; } }
        public bool IsActive            { get { return _active; } set { _active = value; } }
        public int? TMerchId            { get { return _tMerchId; } set { _tMerchId = value; } }
        public int? TShowTicketId       { get { return _tShowTicketId; } set { _tShowTicketId = value; } }
        public string ParentDescription { get { return _parentDescription; } set { _parentDescription = value; } }
        public string Title             { get { return _title; } set { _title = value; } }
        public string Comment           { get { return _comment; } set { _comment = value; } }
        public int RequiredParentQty    { get { return _requiredParentQty; } set { _requiredParentQty = value; } }
        public int MaxSelections        { get { return _maxSelections; } set { _maxSelections = value; } }
        public decimal Price            { get { return _price; } set { _price = value; } }
        public bool IsIncludeWeight     { get { return _includeWeight; } set { _includeWeight = value; } }
        public int NumBundlesSold       { get { return _numBundlesSold; } set { _numBundlesSold = value; } }
        public decimal BundleSales      { get { return _bundleSales; } set { _bundleSales = value; } }
        public int NumBundlesRefunded   { get { return _numBundlesRefunded; } set { _numBundlesRefunded = value; } }
        public decimal BundleRefunds    { get { return _bundleRefunds; } set { _bundleRefunds = value; } }
        public int NumItemsSold         { get { return _numItemsSold; } set { _numItemsSold = value; } }
        public int NumItemsRefunded     { get { return _numItemsRefunded; } set { _numItemsRefunded = value; } }

        public SalesReportBundleRow() {}

        public SalesReportBundleRow(IDataReader dr)
        {
            try
            {
                BundleId            = (int)dr.GetValue(dr.GetOrdinal("Id"));
                IsActive            = (bool)dr.GetValue(dr.GetOrdinal("bActive"));
                if (dr.GetValue(dr.GetOrdinal("TMerchId")).ToString().Trim().Length > 0) 
                    TMerchId        = (int)dr.GetValue(dr.GetOrdinal("TMerchId"));
                if (dr.GetValue(dr.GetOrdinal("TShowTicketId")).ToString().Trim().Length > 0)
                    TShowTicketId   = (int)dr.GetValue(dr.GetOrdinal("TShowTicketId"));
                ParentDescription   = (string)dr.GetValue(dr.GetOrdinal("ParentDescription"));
                Title               = (string)dr.GetValue(dr.GetOrdinal("Title"));
                Comment             = (string)dr.GetValue(dr.GetOrdinal("Comment"));
                RequiredParentQty   = (int)dr.GetValue(dr.GetOrdinal("iRequiredParentQty"));
                MaxSelections       = (int)dr.GetValue(dr.GetOrdinal("iMaxSelections"));
                Price               = (decimal)dr.GetValue(dr.GetOrdinal("mPrice"));
                IsIncludeWeight     = (bool)dr.GetValue(dr.GetOrdinal("bIncludeWeight"));
                NumBundlesSold      = (int)dr.GetValue(dr.GetOrdinal("NumBundlesSold"));
                BundleSales         = (decimal)dr.GetValue(dr.GetOrdinal("BundleSales"));
                NumBundlesRefunded  = (int)dr.GetValue(dr.GetOrdinal("NumBundlesRefunded"));
                BundleRefunds       = (decimal)dr.GetValue(dr.GetOrdinal("BundleRefunds"));
                NumItemsSold        = (int)dr.GetValue(dr.GetOrdinal("NumItemsSold"));
                NumItemsRefunded    = (int)dr.GetValue(dr.GetOrdinal("NumItemsRefunded"));
            }
            catch (Exception ex)
            {
                _Error.LogException(ex);
            }
        }

        //CSV//
        /// <summary>
        /// Converts a list of merch rows for csv export. Intended for entire batches of shipments.
        /// </summary>
        /// <param name="invoiceShipments"></param>
        /// <param name="invoiceItems"></param>
        /// <param name="fileAttachmentName"></param>
        public static void CSV_ProvideDownload(List<SalesReportBundleRow> list, string fileAttachmentName, string pageToAccommodateDownload)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            //write header
            sb.AppendFormat("BundleId,IsActive,TMerchId,TShowTicketId,ParentDescription,Title,Comment,RequiredParentQty,MaxSelections,Price,IsIncludeWeight,NumBundlesSold,BundleSales,NumBundlesRefunded,BundleRefunds,NumItemsSold,NumItemsRefunded{0}", Environment.NewLine);
            
            foreach (SalesReportBundleRow row in list)
                ProcessRowPerFormat(sb, row);

            CSV_WriteToContextForDownload(sb, fileAttachmentName, pageToAccommodateDownload);
        }

        private static void ProcessRowPerFormat(System.Text.StringBuilder sb, SalesReportBundleRow row)
        {
            sb.AppendFormat("\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\",\"{6}\",\"{7}\",\"{8}\",\"{9}\",\"{10}\",\"{11}\",\"{12}\",\"{13}\",\"{14}\",\"{15}\",\"{16}\",\"{17}\",\"{18}\"{19}",

            row.BundleId.ToString(),
            row.IsActive.ToString(),
            (row.TMerchId.HasValue) ? row.TMerchId.Value.ToString() : string.Empty,
            (row.TShowTicketId.HasValue) ? row.TShowTicketId.Value.ToString() : string.Empty,
            row.ParentDescription.Replace(',', ' '),
            row.Title.Replace(',', ' '),
            row.Comment.Replace(',', ' '),
            row.RequiredParentQty.ToString(),
            row.MaxSelections.ToString(),
            row.Price.ToString("n2"),
            row.IsIncludeWeight.ToString(),
            row.NumBundlesSold.ToString(),
            row.BundleSales.ToString("n2"),
            row.NumBundlesRefunded.ToString(),
            row.BundleRefunds.ToString("n2"),
            row.NumItemsSold.ToString(),
            row.NumItemsRefunded.ToString(),
            Environment.NewLine);
        }

        public static void CSV_WriteToContextForDownload(System.Text.StringBuilder sb, string attachment, string pageToAccommodateDownload)
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            context.Response.Clear();
            context.Response.ClearContent();
            context.Response.ClearHeaders();
            context.Response.ContentType = "application/x-download";//"text/csv";
            context.Response.AddHeader("Content-Disposition", attachment);
            
            try
            {
                context.Response.Write(sb.ToString());
                context.Response.End();//this may thread abort

                return;
            }
            catch (System.Threading.ThreadAbortException)
            {
                //we can safely ignore this error 
                return;
            }
            catch (Exception ex)
            {
                _Error.LogException(ex);
            }
        }
    }
}