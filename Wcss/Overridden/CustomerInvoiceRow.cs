using System;
using System.Collections.Generic;
using System.Data;


namespace Wcss
{

    [Serializable]
    public class MerchSalesAggsAndRows
    {
        private CustomerInvoiceAggregateRow _aggregates;
        public CustomerInvoiceAggregateRow AggregateRow
        {
            get
            {
                return _aggregates;
            }
            set
            {
                _aggregates = value;
            }
        }
        private List<CustomerInvoiceRow> _invoices;
        public List<CustomerInvoiceRow> Invoices
        {
            get
            {
                return _invoices;
            }
            set
            {
                _invoices = value;
            }
        }
        public MerchSalesAggsAndRows(CustomerInvoiceAggregateRow agg, List<CustomerInvoiceRow> list)
        {
            this.AggregateRow = agg;
            this.Invoices = list;
        }
    }

    [Serializable]
    public partial class CustomerInvoiceAggregateRow
    {
        private string _indicator;
        private decimal _totalPaid;
        private decimal _totalRefunds;
        private decimal _totalTax;
        private decimal _totalFreight;
        private decimal _totalNetPaid;

        public string Indicator { get { return _indicator; } set { _indicator = value; } }
        public decimal TotalPaid { get { return _totalPaid; } set { _totalPaid = value; } }
        public decimal TotalRefunds { get { return _totalRefunds; } set { _totalRefunds = value; } }
        public decimal TotalTax { get { return _totalTax; } set { _totalTax = value; } }
        public decimal TotalFreight { get { return _totalFreight; } set { _totalFreight = value; } }
        public decimal TotalNetPaid { get { return _totalNetPaid; } set { _totalNetPaid = value; } }

        public CustomerInvoiceAggregateRow(IDataReader dr)
        {
            Indicator = dr.GetValue(dr.GetOrdinal("Indicator")).ToString();
            TotalPaid = (dr.GetValue(dr.GetOrdinal("TotalPaid")).ToString().Length > 0) ? (decimal)dr.GetValue(dr.GetOrdinal("TotalPaid")) : 0;
            TotalRefunds = (dr.GetValue(dr.GetOrdinal("TotalRefunds")).ToString().Length > 0) ? (decimal)dr.GetValue(dr.GetOrdinal("TotalRefunds")) : 0;
            TotalTax = (dr.GetValue(dr.GetOrdinal("TotalTax")).ToString().Length > 0) ? (decimal)dr.GetValue(dr.GetOrdinal("TotalTax")) : 0;
            TotalFreight = (dr.GetValue(dr.GetOrdinal("TotalFreight")).ToString().Length > 0) ? (decimal)dr.GetValue(dr.GetOrdinal("TotalFreight")) : 0;
            TotalNetPaid = (dr.GetValue(dr.GetOrdinal("TotalNetPaid")).ToString().Length > 0) ? (decimal)dr.GetValue(dr.GetOrdinal("TotalNetPaid")) : 0;
        }
    }

    [Serializable]
    public partial class CustomerInvoiceRow
    {
        private int _invoiceId;
        private string _uniqueId = "";
        private int _authNetId;
        private DateTime _invoiceDate;
        private string _invoiceStatus;
        private string _description;
        private string _productList;
        private string _purchaserName;
        private string _purchaserEmail;
        private decimal _totalPaid;
        private decimal _totalRefunds;
        private decimal _taxAmount;
        private decimal _freightAmount;
        private decimal _netPaid;
        private string _transactionType;

        public int InvoiceId { get { return _invoiceId; } set { _invoiceId = value; } }
        public string UniqueId { get { return (_uniqueId == null) ? string.Empty : _uniqueId; } set { _uniqueId = value; } }
        public int AuthNetId { get { return _authNetId; } set { _authNetId = value; } }
        public DateTime InvoiceDate { get { return _invoiceDate; } set { _invoiceDate = value; } }
        public string InvoiceStatus { get { return _invoiceStatus; } set { _invoiceStatus = value; } }
        public string Description { get { return _description; } set { _description = value; } }
        protected string productList { get { return _productList; } set { _productList = value; } }
        public List<Invoice.InvoiceProductListing> ProductListing { get { return Invoice.InvoiceProductListing.InvoiceProducts(productList); } }
        public string PurchaserName { get { return _purchaserName; } set { _purchaserName = value; } }
        public string PurchaserEmail { get { return _purchaserEmail; } set { _purchaserEmail = value; } }
        public decimal TotalPaid { get { return _totalPaid; } set { _totalPaid = value; } }
        public decimal TotalRefunds { get { return _totalRefunds; } set { _totalRefunds = value; } }
        public decimal TaxAmount { get { return _taxAmount; } set { _taxAmount = value; } }
        public decimal FreightAmount { get { return _freightAmount; } set { _freightAmount = value; } }
        public decimal NetPaid { get { return _netPaid; } set { _netPaid = value; } }
        public string TransactionType { get { return _transactionType; } set { _transactionType = value; } }

        public CustomerInvoiceRow(DataRow rowFromGetCustomerPurchases)
        {
            DataRow dr = rowFromGetCustomerPurchases;

            InvoiceId = (int)dr.ItemArray.GetValue(dr.Table.Columns.IndexOf("InvoiceId"));
            UniqueId = dr.ItemArray.GetValue(dr.Table.Columns.IndexOf("UniqueId")).ToString();
            AuthNetId = (int)dr.ItemArray.GetValue(dr.Table.Columns.IndexOf("AuthNetId"));
            InvoiceDate = (DateTime)dr.ItemArray.GetValue(dr.Table.Columns.IndexOf("dtInvoiceDate"));
            InvoiceStatus = dr.ItemArray.GetValue(dr.Table.Columns.IndexOf("InvoiceStatus")).ToString();
            Description = dr.ItemArray.GetValue(dr.Table.Columns.IndexOf("Description")).ToString();
            productList = dr.ItemArray.GetValue(dr.Table.Columns.IndexOf("ProductList")).ToString();
            PurchaserName = dr.ItemArray.GetValue(dr.Table.Columns.IndexOf("PurchaserName")).ToString();
            PurchaserEmail = dr.ItemArray.GetValue(dr.Table.Columns.IndexOf("PurchaserEmail")).ToString();
            TotalPaid = (decimal)dr.ItemArray.GetValue(dr.Table.Columns.IndexOf("mTotalPaid"));
            TotalRefunds = (decimal)dr.ItemArray.GetValue(dr.Table.Columns.IndexOf("mTotalRefunds"));
            TaxAmount = (decimal)dr.ItemArray.GetValue(dr.Table.Columns.IndexOf("TaxAmount"));
            FreightAmount = (decimal)dr.ItemArray.GetValue(dr.Table.Columns.IndexOf("FreightAmount"));
            NetPaid = (decimal)dr.ItemArray.GetValue(dr.Table.Columns.IndexOf("mNetPaid"));
            TransactionType = dr.ItemArray.GetValue(dr.Table.Columns.IndexOf("TransactionType")).ToString();
        }

        public CustomerInvoiceRow(IDataReader dr)
        {
            InvoiceId = (int)dr.GetValue(dr.GetOrdinal("InvoiceId"));
            UniqueId = dr.GetValue(dr.GetOrdinal("UniqueId")).ToString();
            AuthNetId = (int)dr.GetValue(dr.GetOrdinal("AuthNetId"));
            InvoiceDate = (DateTime)dr.GetValue(dr.GetOrdinal("dtInvoiceDate"));
            InvoiceStatus = dr.GetValue(dr.GetOrdinal("InvoiceStatus")).ToString();
            Description = dr.GetValue(dr.GetOrdinal("Description")).ToString();
            productList = dr.GetValue(dr.GetOrdinal("ProductList")).ToString();
            PurchaserName = dr.GetValue(dr.GetOrdinal("PurchaserName")).ToString();
            PurchaserEmail = dr.GetValue(dr.GetOrdinal("PurchaserEmail")).ToString();
            TotalPaid = (decimal)dr.GetValue(dr.GetOrdinal("mTotalPaid"));
            TotalRefunds = (decimal)dr.GetValue(dr.GetOrdinal("mTotalRefunds"));
            TaxAmount = (decimal)dr.GetValue(dr.GetOrdinal("TaxAmount"));
            FreightAmount = (decimal)dr.GetValue(dr.GetOrdinal("FreightAmount"));
            NetPaid = (decimal)dr.GetValue(dr.GetOrdinal("mNetPaid"));
            TransactionType = dr.GetValue(dr.GetOrdinal("TransactionType")).ToString();
        }

        public CustomerInvoiceRow(int invoiceId, string uniqueId, int authNetId, DateTime invoiceDate, string shipStatus, 
            string invoiceStatus, string description,
            string ProductList, string purchaserName, string purchaserEmail, decimal totalPaid, decimal totalRefunds,
            decimal taxAmount, decimal freightAmount, decimal netPaid, string transactionType)
        {
            InvoiceId = invoiceId;
            UniqueId = uniqueId;
            AuthNetId = authNetId;
            InvoiceDate = invoiceDate;
            InvoiceStatus = invoiceStatus;
            Description = description;
            productList = ProductList;
            PurchaserName = purchaserName;
            PurchaserEmail = purchaserEmail;
            TotalPaid = totalPaid;
            TotalRefunds = totalRefunds;
            TaxAmount = taxAmount;
            FreightAmount = freightAmount;
            NetPaid = netPaid;
            TransactionType = transactionType;
        }
        

        /// <summary>
        /// returns orders for a given context - ALL, Tickets, Merch
        /// </summary>
        /// <param name="context"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>        
        public static List<CustomerInvoiceRow> GetOrdersInRange(_Enums.ProductContext context, DateTime startDate, DateTime endDate,
            int startRowIndex, int maximumRows)
        {
            startDate = startDate.Date;
            endDate = endDate.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

            //Note that invoice can be listed in both categories - ndepends on items in invoice
            List<CustomerInvoiceRow> list = new List<CustomerInvoiceRow>();

            using (IDataReader dr = SPs.TxGetOrdersInRange(_Config.APPLICATION_ID, context.ToString(), startDate.ToString("MM/dd/yyyy hh:mmtt"),
                endDate.ToString("MM/dd/yyyy hh:mmtt"), startRowIndex, maximumRows).GetReader())
            {
                while (dr.Read())
                {
                    CustomerInvoiceRow cpr = new CustomerInvoiceRow(dr);

                    list.Add(cpr);
                }

                dr.Close();
            }

            return list;
        }

        public static int GetOrdersInRangeCount(_Enums.ProductContext context, DateTime startDate, DateTime endDate)
        {
            startDate = startDate.Date;
            endDate = endDate.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

            //Note that invoice can be listed in both categories - ndepends on items in invoice
            int count = 0;
            
            using (IDataReader dr = SPs.TxGetOrdersInRangeCount(_Config.APPLICATION_ID, context.ToString(), 
                startDate.ToString("MM/dd/yyyy hh:mmtt"), endDate.ToString("MM/dd/yyyy hh:mmtt")).GetReader())
            {
                while (dr.Read())
                {
                    count = (int)dr.GetValue(0);
                }

                dr.Close();
            }

            return count;
        }


        /// <summary>
        /// Given a username, gets all the purchases for that user
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public static List<CustomerInvoiceRow> GetCustomerSalesHistory(string userName, int startRowIndex, int maximumRows)
        {
            //compensate for grid control startIdx - only for old method
            if(startRowIndex.ToString().EndsWith("0"))
                startRowIndex += 1;

            //Note that invoice can be listed in both categories - ndepends on items in invoice
            List<CustomerInvoiceRow> list = new List<CustomerInvoiceRow>();

            using (IDataReader dr = SPs.TxGetCustomerSalesHistory(_Config.APPLICATION_NAME, userName, startRowIndex, maximumRows).GetReader())
            {
                while (dr.Read())
                {
                    CustomerInvoiceRow cpr = new CustomerInvoiceRow(dr);

                    list.Add(cpr);
                }

                dr.Close();
            }

            return list;
        }

        public static int GetCustomerSalesHistoryCount(string userName)
        {
            //Note that invoice can be listed in both categories - ndepends on items in invoice
            int count = 0;

            using (IDataReader dr = SPs.TxGetCustomerSalesHistoryCount(_Config.APPLICATION_NAME, userName).GetReader())
            {
                while (dr.Read())
                {
                    count = (int)dr.GetValue(0);
                }

                dr.Close();
            }

            return count;
        }

        public static List<string> GetEmailOfMerchSalesInRange(int parentId, int gridId, string style, string color, 
            string size, string activeStatus, DateTime startDate, DateTime endDate)
        {
            return GetEmailOfMerchSalesInRange(parentId, gridId, style, color, size, activeStatus, startDate, endDate, false);
        }
        public static List<string> GetEmailOfMerchSalesInRange(int parentId, int gridId, string style, string color,
            string size, string activeStatus, DateTime startDate, DateTime endDate, bool includeInvoiceIdWithEmail)
        {
            if (style != null && style.ToLower().IndexOf("all") != -1)
                style = string.Empty;
            if (color != null && color.ToLower().IndexOf("all") != -1)
                color = string.Empty;
            if (size != null && size.ToLower().IndexOf("all") != -1)
                size = string.Empty;

            startDate = startDate.Date;
            endDate = endDate.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
            
            if (gridId > 0)
                parentId = gridId;

            bool emailOnly = true;

            //Note that invoice can be listed in both categories - ndepends on items in invoice
            List<string> list = new List<string>();

            using (IDataReader dr = SPs.TxGetMerchSalesInRange(_Config.APPLICATION_ID, parentId, 
                style, color, size, activeStatus, emailOnly, includeInvoiceIdWithEmail, startDate.ToString("MM/dd/yyyy hh:mmtt"), 
                endDate.ToString("MM/dd/yyyy hh:mmtt"), 0, System.Data.SqlTypes.SqlInt32.MaxValue.Value).GetReader())
            {
                while (dr.Read())
                {
                    if (includeInvoiceIdWithEmail)
                        list.Add(string.Format("{0}={1}", dr.GetValue(dr.GetOrdinal("UniqueId")).ToString(), dr.GetValue(dr.GetOrdinal("PurchaserEmail")).ToString() ));
                    else
                        list.Add(dr.GetValue(dr.GetOrdinal("PurchaserEmail")).ToString());
                }

                dr.Close();
            }

            return list;
        }
        public static List<CustomerInvoiceRow> GetMerchSalesInRange(int parentId, int gridId, string style, string color,
            string size, string activeStatus, DateTime startDate, DateTime endDate, int startRowIndex, int maximumRows)
        {
            if (style != null && style.ToLower().IndexOf("all") != -1)
                style = string.Empty;
            if (color != null && color.ToLower().IndexOf("all") != -1)
                color = string.Empty;
            if (size != null && size.ToLower().IndexOf("all") != -1)
                size = string.Empty;

            startDate = startDate.Date;
            endDate = endDate.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

            if (gridId > 0)
                parentId = gridId;

            bool emailOnly = false;
            bool includeInvoiceIdWithEmail = false;

            //Note that invoice can be listed in both categories - ndepends on items in invoice
            List<CustomerInvoiceRow> list = new List<CustomerInvoiceRow>();            

            using (IDataReader dr = SPs.TxGetMerchSalesInRange(_Config.APPLICATION_ID, parentId,
                style, color, size, activeStatus, emailOnly, includeInvoiceIdWithEmail, startDate.ToString("MM/dd/yyyy hh:mmtt"),
                endDate.ToString("MM/dd/yyyy hh:mmtt"), startRowIndex, maximumRows).GetReader())
            {
                while (dr.Read())
                {
                    CustomerInvoiceRow cpr = new CustomerInvoiceRow(dr);

                    list.Add(cpr);
                }

                dr.Close();
            }

            return list;
        }
        public static List<CustomerInvoiceAggregateRow> GetMerchSalesInRangeAggregates(int parentId, int gridId, string style, 
            string color, string size, string activeStatus, DateTime startDate, DateTime endDate)
        {
            if (style != null && style.ToLower().IndexOf("all") != -1)
                style = string.Empty;
            if (color != null && color.ToLower().IndexOf("all") != -1)
                color = string.Empty;
            if (size != null && size.ToLower().IndexOf("all") != -1)
                size = string.Empty;

            startDate = startDate.Date;
            endDate = endDate.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

            //Note this used to pass in a grid id (selected id) to further narrow the search/range
            if (gridId > 0)
                parentId = gridId;

            List<CustomerInvoiceAggregateRow> list = new List<CustomerInvoiceAggregateRow>();

            using (IDataReader dr = SPs.TxGetMerchSalesInRangeAggregates(_Config.APPLICATION_ID, parentId, style, color, size, activeStatus,
                startDate.ToString("MM/dd/yyyy hh:mmtt"), endDate.ToString("MM/dd/yyyy hh:mmtt")).GetReader())
            {
                while (dr.Read())
                {
                    CustomerInvoiceAggregateRow agg = new CustomerInvoiceAggregateRow(dr);
                    list.Add(agg);
                }

                dr.Close();
            }

            return list;
        }

        public static int GetMerchSalesInRangeCount(int parentId, int gridId, string style, string color, 
            string size, string activeStatus, DateTime startDate, DateTime endDate)
        {
            if (style != null && style.ToLower().IndexOf("all") != -1)
                style = string.Empty;
            if (color != null && color.ToLower().IndexOf("all") != -1)
                color = string.Empty;
            if (size != null && size.ToLower().IndexOf("all") != -1)
                size = string.Empty;

            startDate = startDate.Date;
            endDate = endDate.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

            if (gridId > 0)
                parentId = gridId;

            //Note that invoice can be listed in both categories - ndepends on items in invoice
            int count = 0;

            using (IDataReader dr = SPs.TxGetMerchSalesInRangeCount(_Config.APPLICATION_ID, parentId, style, color, 
                size, activeStatus, startDate.ToString("MM/dd/yyyy hh:mmtt"),
                endDate.ToString("MM/dd/yyyy hh:mmtt")).GetReader())
            {
                while (dr.Read())
                {
                    count = (int)dr.GetValue(0);
                }

                dr.Close();
            }

            return count;
        }

        public static List<string> GetMerchCodesInRange(int parentId, int gridId, string style, string color,
           string size, string activeStatus, DateTime startDate, DateTime endDate)
        {
            if (style != null && style.ToLower().IndexOf("all") != -1)
                style = string.Empty;
            if (color != null && color.ToLower().IndexOf("all") != -1)
                color = string.Empty;
            if (size != null && size.ToLower().IndexOf("all") != -1)
                size = string.Empty;

            startDate = startDate.Date;
            endDate = endDate.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

            if (gridId > 0)
                parentId = gridId;

            //Note that invoice can be listed in both categories - ndepends on items in invoice
            List<string> list = new List<string>();

            using (IDataReader dr = SPs.TxGetMerchCodesInRange(_Config.APPLICATION_ID, parentId,
                style, color, size, activeStatus, startDate.ToString("MM/dd/yyyy hh:mmtt"),
                endDate.ToString("MM/dd/yyyy hh:mmtt")).GetReader())
            {
                while (dr.Read())
                {
                    list.Add(dr.GetValue(dr.GetOrdinal("CodeLine")).ToString());
                }

                dr.Close();
            }

            return list;
        }
    }
}
