using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Wcss.QueryRow
{
    public class SalesReport
    {
        #region ReportTable

        public DateTime StartDate { get; protected set; }
        public DateTime EndDate { get; protected set; }

        public Table BuildReportTable(string tableClass)
        {
            ReportTableClass = tableClass;
            return ReportTable;
        }

        protected string ReportTableClass { get; set; }
        private Table _reportTable = null;
        public Table ReportTable
        {
            get
            {
                if (_reportTable == null)
                {
                    int maxCells = 3;
                    bool useHrAsSeparator = false;
                    Table t = new Table();
                    t.CellPadding = 0;
                    t.CellSpacing = 1;

                    if (ReportTableClass != null)
                        t.CssClass = ReportTableClass;

                    t.Rows.Add(ConstructHeaderRow(string.Format("<h2>Sales Report For Period {0} thru {1}</h2>", 
                        this.StartDate.ToString("MM/dd/yyyy hh:mmtt"), this.EndDate.ToString("MM/dd/yyyy hh:mmtt")), maxCells));
                    t.Rows.Add(ConstructSpacerRow(useHrAsSeparator, maxCells));

                    t.Rows.Add(ConstructHeaderRow("<h4 class=\"rounded\">Sales Activity</h4>", maxCells));

                    t.Rows.Add(ConstructTableRow("Number of Sales", this.AllSales_Row.NumInvoices.ToString(), 
                        "Number of invoices/sales/purchase transactions. No refunds are included."));
                    t.Rows.Add(ConstructTableRow("Total Line Items", this.AllSales_Row.LinePurchases.ToString(),
                        "Number of separate line items within period. Includes all items."));

                    t.Rows.Add(ConstructSpacerRow(true, maxCells));

                    t.Rows.Add(ConstructTableRow("Item Quantity", this.AllSales_Row.ItemsPurchased.ToString(),
                        "Tickets, merchandise and charity/donations."));
                    t.Rows.Add(ConstructTableRow("Merch Quantity", this.AllSales_Row.MerchPurchased.ToString(),
                        "&nbsp;"));
                    t.Rows.Add(ConstructTableRow("Ticket Quantity", this.AllSales_Row.TicketsPurchased.ToString(),
                        "&nbsp;"));
                    t.Rows.Add(ConstructTableRow("Donation Quantity", this.AllSales_Row.DonationsPurchased.ToString(),
                        "&nbsp;"));
                    t.Rows.Add(ConstructTableRow("Other Quantity", this.AllSales_Row.OtherPurchased.ToString(),
                        "Includes processing fees, shipping eg. items that are not tickets, merch or donations."));
                    t.Rows.Add(ConstructSpacerRow(useHrAsSeparator, maxCells));


                    t.Rows.Add(ConstructHeaderRow("<h4 class=\"rounded\">Sales Totals</h4>", maxCells));
                    t.Rows.Add(ConstructTableRow("Base Sales", this.AllSales_Row.BaseSales.ToString("n2"),
                        "Amount of goods sold before service fees, shipping and processing."));
                    t.Rows.Add(ConstructTableRow("Base Merch Sales", this.AllSales_Row.MerchPortion.ToString("n2"),
                        "Does not include shipping."));
                    t.Rows.Add(ConstructTableRow("Base Ticket Sales", this.AllSales_Row.TicketPortion.ToString("n2"),
                        "Does not include shipping or service fees"));
                    t.Rows.Add(ConstructTableRow("Service Charges", this.AllSales_Row.ServiceCharge.ToString("n2"),
                        "&nbsp;")); 
                    t.Rows.Add(ConstructTableRow("Donation Sales", this.AllSales_Row.DonationPortion.ToString("n2"),
                        "&nbsp;"));
                    t.Rows.Add(ConstructTableRow("Other Sales", this.AllSales_Row.OtherPortion.ToString("n2"),
                        "Includes processing fees, shipping eg. items that are not tickets, service fees, merch or donations."));
                    t.Rows.Add(ConstructTableRow("Line Item Total", this.AllSales_Row.LineItemTotal.ToString("n2"),
                        "Total before processing fees. This is the total paid for line items."));
                    t.Rows.Add(ConstructTableRow("Processing Fees", this.AllSales_Row.ProcessingFee.ToString("n2"),
                        "Amount of processing fees.")); 
                    t.Rows.Add(ConstructTableRow("Damaged", this.AllSales_Row.Damaged.ToString("n2"),
                        "Value of goods reported as damaged."));
                    
                    
                    
                    
                    t.Rows.Add(ConstructSpacerRow(useHrAsSeparator, maxCells));                    

                    
                    t.Rows.Add(ConstructTableRow("Shipments", this.AllSales_Row.Shipments.ToString(),
                        "Number of shipments."));
                    t.Rows.Add(ConstructTableRow("Shipping Sales", this.AllSales_Row.ShipCharged.ToString("n2"),
                        "Amount of charged to customers for shipping."));
                    t.Rows.Add(ConstructTableRow("Handling Charges", this.AllSales_Row.ShipHandlingCalc.ToString("n2"),
                        "Handling calculated and added to shipping charges."));
                    t.Rows.Add(ConstructTableRow("Shipping Merchandise", this.AllSales_Row.ShipMerch.ToString("n2"),
                        "Amount paid for shipping merchandise items."));
                    t.Rows.Add(ConstructTableRow("Shipping Tickets", this.AllSales_Row.ShipTicket.ToString("n2"),
                        "Amount paid for shipping ticket items."));
                    t.Rows.Add(ConstructTableRow("Actual Shipping Cost", this.AllSales_Row.ShipActual.ToString("n2"),
                        "Amount of actual (recorded) shipping cost."));
                    t.Rows.Add(ConstructTableRow("Shipping Differential", this.AllSales_Row.ShipDifferential.ToString("n2"),
                        "Difference between amount paid by customers and actual shipping costs."));






                    t.Rows.Add(ConstructTableRow("Gross", this.AllSales_Row.TotalPaid.ToString("n2"),
                        "Gross amount of item sales."));
                    t.Rows.Add(ConstructTableRow("Total Refunds", this.AllSales_RowRefund.TotalRefunded.ToString("n2"),
                        "Amount of refunds in the period."));
                    t.Rows.Add(ConstructTableRow("Adjustments", this.AllSales_Row.Adjustment.ToString("n2"),
                        "Adjustments to line item totals for exchanges."));
                    t.Rows.Add(ConstructTableRow("Net", this.AllSales_Row.NetPaid.ToString("n2"),
                        "Net amount of item sales. This is derived after any adjustments and refunds."));
                    t.Rows.Add(ConstructSpacerRow(useHrAsSeparator, maxCells));


                    t.Rows.Add(ConstructHeaderRow("<h4 class=\"rounded\">Store Credit And Gift Certificates</h4>", maxCells));
                    t.Rows.Add(ConstructTableRow("Store Credit Income", this.AllSales_RowGift.StoreCreditSpent.ToString("n2"),
                        "Sales total for store credit."));
                    t.Rows.Add(ConstructTableRow("Gift Certificates Sold", this.AllSales_RowGift.NumGiftSold.ToString(),
                        "&nbsp;"));
                    t.Rows.Add(ConstructTableRow("Gift Certificate Sales", this.AllSales_RowGift.GiftMoneySold.ToString("n2"),
                        "&nbsp;"));
                    t.Rows.Add(ConstructTableRow("Gift Certificates Redeemed", this.AllSales_RowGift.NumGiftRedeemed.ToString(),
                        "Number of gift certificates redeemed within the period."));
                    t.Rows.Add(ConstructTableRow("Amount Redeemed", this.AllSales_RowGift.GiftMoneyRedeemed.ToString("n2"),
                        "Gift certificate amount redeemed within the period."));
                    t.Rows.Add(ConstructTableRow("Period GCs Outstanding", this.AllSales_RowGift.OutstandingRedemptionMoney.ToString("n2"),
                        "Amount of money yet to be redeemed from gift certificates bought within the time period."));
                    t.Rows.Add(ConstructTableRow("Credit Outstanding", this.AllSales_RowGift.StoreCreditInHolding.ToString("n2"),
                        "Store credit in holding in user accounts - yet to be spent."));
                    t.Rows.Add(ConstructSpacerRow(useHrAsSeparator, maxCells));


                    t.Rows.Add(ConstructHeaderRow("<h4 class=\"rounded\">Refunds</h4>", maxCells));                    
                    t.Rows.Add(ConstructTableRow("Total Refunds", this.AllSales_RowRefund.TotalRefunded.ToString("n2"),
                        "Amount of refunds in the period."));
                    t.Rows.Add(ConstructSpacerRow(false, maxCells));
                    t.Rows.Add(ConstructTableRow("Number of refunds", this.AllSales_RowRefund.LineRefunds.ToString(),
                        "Number of refunds performed. Not necessarily quantity of items refunded."));
                    t.Rows.Add(ConstructTableRow("Merch refunds", this.AllSales_RowRefund.MerchRefunds.ToString(),
                        "&nbsp;"));
                    t.Rows.Add(ConstructTableRow("Merch amount", this.AllSales_RowRefund.MerchRefunded.ToString("n2"),
                        "&nbsp;"));
                    t.Rows.Add(ConstructTableRow("Ticket refunds", this.AllSales_RowRefund.TicketsRefunds.ToString(),
                        "&nbsp;"));
                    t.Rows.Add(ConstructTableRow("Ticket amount", this.AllSales_RowRefund.TicketsRefunded.ToString("n2"),
                        "&nbsp;"));
                    t.Rows.Add(ConstructTableRow("Donation refunds", this.AllSales_RowRefund.DonationsRefunds.ToString(),
                        "&nbsp;"));
                    t.Rows.Add(ConstructTableRow("Donation amount", this.AllSales_RowRefund.DonationsRefunded.ToString("n2"),
                        "&nbsp;"));
                    t.Rows.Add(ConstructTableRow("Service fee refunds", this.AllSales_RowRefund.ServiceRefunds.ToString(),
                        "&nbsp;"));
                    t.Rows.Add(ConstructTableRow("Service fee amount", this.AllSales_RowRefund.ServiceRefunded.ToString("n2"),
                        "&nbsp;"));
                    t.Rows.Add(ConstructTableRow("Processing fee refunds", this.AllSales_RowRefund.ProcessingRefunds.ToString(),
                        "&nbsp;"));
                    t.Rows.Add(ConstructTableRow("Processing fee amount", this.AllSales_RowRefund.ProcessingRefunded.ToString("n2"),
                        "&nbsp;"));
                    t.Rows.Add(ConstructTableRow("Merch shipping refunds", this.AllSales_RowRefund.MerchShippingRefunds.ToString(),
                        "&nbsp;"));
                    t.Rows.Add(ConstructTableRow("Merch shipping amount", this.AllSales_RowRefund.MerchShippingRefunded.ToString("n2"),
                        "&nbsp;"));
                    t.Rows.Add(ConstructTableRow("Ticket shipping refunds", this.AllSales_RowRefund.TicketShippingRefunds.ToString(),
                        "&nbsp;"));
                    t.Rows.Add(ConstructTableRow("Ticket shipping amount", this.AllSales_RowRefund.TicketShippingRefunded.ToString("n2"),
                        "&nbsp;"));
                    t.Rows.Add(ConstructTableRow("Damage refunds", this.AllSales_RowRefund.DamageRefunds.ToString(),
                        "&nbsp;"));
                    t.Rows.Add(ConstructTableRow("Damage amount", this.AllSales_RowRefund.DamageRefunded.ToString("n2"),
                        "&nbsp;"));
                    t.Rows.Add(ConstructTableRow("Other refunds", this.AllSales_RowRefund.OtherRefunds.ToString(),
                        "&nbsp;"));
                    t.Rows.Add(ConstructTableRow("Other amount", this.AllSales_RowRefund.OtherRefunded.ToString("n2"),
                        "&nbsp;"));

                    
                    _reportTable = t;
                }

                return _reportTable;
            }
        }
        private TableRow ConstructHeaderRow(string caption, int colspan)
        {
            TableRow tr = new TableRow();

            TableCell th = new TableCell();
            th.ColumnSpan = colspan;
            th.Attributes.Add("class", "rptheader");
            th.Text = caption;

            tr.Cells.Add(th);

            return tr;
        }
        private TableRow ConstructSpacerRow(bool useHR, int colspan)
        {
            TableRow tr = new TableRow();

            TableCell td = new TableCell();
            td.ColumnSpan = colspan;
            td.Attributes.Add("class", "rptseparator");
            td.Text = (useHR) ? "<hr/>" : "&nbsp;";

            tr.Cells.Add(td);

            return tr;
        }
        private TableRow ConstructTableRow(string header, string reportedValue, string description)
        {
            TableRow tr = new TableRow();

            TableHeaderCell th = new TableHeaderCell();            
            th.Text = header;

            TableCell td = new TableCell();
            td.Text = reportedValue;

            TableCell desc = new TableCell();
            desc.Text = description ?? "&nbsp;";

            tr.Cells.Add(th);
            tr.Cells.Add(td);
            tr.Cells.Add(desc);

            return tr;
        }

        #endregion

        protected List<SalesReport_All_DataRow> _allSalesDataRow = new List<SalesReport_All_DataRow>();
        protected List<SalesReport_All_AggregateRow> _allSalesAggregateRow = new List<SalesReport_All_AggregateRow>();
        protected List<SalesReport_RefundRow> _allSalesRefundRow = new List<SalesReport_RefundRow>();
        protected List<SalesReport_GiftRow> _allSalesGiftRow = new List<SalesReport_GiftRow>();
        protected List<Merch> _giftItems = new List<Merch>();
        protected int _allSalesCount = 0;

        public List<SalesReport_All_DataRow> AllSales_DataRow { get { return _allSalesDataRow; } set { _allSalesDataRow = value; } }
        public List<SalesReport_All_AggregateRow> AllSales_AggregateRow { get { return _allSalesAggregateRow; } set { _allSalesAggregateRow = value; } }
        public List<SalesReport_RefundRow> AllSales_RefundRow { get { return _allSalesRefundRow; } set { _allSalesRefundRow = value; } }
        public List<SalesReport_GiftRow> AllSales_GiftRow { get { return _allSalesGiftRow; } set { _allSalesGiftRow = value; } }
        public List<Merch> AllSales_GiftItems { get { return _giftItems; } set { _giftItems = value; } }
        /// <summary>
        /// returns the count of all rows that meet the range criteria. Us for the VirtualItemCount property of the DataGrid control
        /// </summary>
        public int AllSales_Count { get { return AllSales_AggregateRow[0].NumInvoices; } }
        public SalesReport_All_AggregateRow AllSales_Row { get { return this.AllSales_AggregateRow[0]; } }
        public SalesReport_GiftRow AllSales_RowGift { get { return this.AllSales_GiftRow[0]; } }
        public SalesReport_RefundRow AllSales_RowRefund { get { return this.AllSales_RefundRow[0]; } }

        #region TODO tix and merch
        protected SalesReport_Tickets_DataRow _ticketSalesDataRow = new SalesReport_Tickets_DataRow();
        protected SalesReport_Tickets_AggregateRow _ticketSalesAggregateRow = new SalesReport_Tickets_AggregateRow();
        protected int _ticketSalesCount = 0;

        public SalesReport_Tickets_DataRow TicketSales_DataRow { get { return _ticketSalesDataRow; } set { _ticketSalesDataRow = value; } }
        public SalesReport_Tickets_AggregateRow TicketSales_AggregateRow { get { return _ticketSalesAggregateRow; } set { _ticketSalesAggregateRow = value; } }
        public int TicketSales_Count { get { return _ticketSalesCount; } set { _ticketSalesCount = value; } }

        protected SalesReport_Merch_DataRow _merchSalesDataRow = new SalesReport_Merch_DataRow();
        protected SalesReport_Merch_AggregateRow _merchSalesAggregateRow = new SalesReport_Merch_AggregateRow();
        protected int _merchSalesCount = 0;

        public SalesReport_Merch_DataRow MerchSales_DataRow { get { return _merchSalesDataRow; } set { _merchSalesDataRow = value; } }
        public SalesReport_Merch_AggregateRow MerchSales_AggregateRow { get { return _merchSalesAggregateRow; } set { _merchSalesAggregateRow = value; } }
        public int MerchSales_Count { get { return _merchSalesCount; } set { _merchSalesCount = value; } }
        #endregion

        //public void DownloadSalesReport(DateTime startDate, DateTime endDate, int startRowIndex, int maximumRows, string reportTableClass)
        //{
        //    SalesReport report = new SalesReport(startDate, endDate, startRowIndex, maximumRows, reportTableClass);

        //    //format and save sales report
        //    //download process
        //    //when complete - delete the file

        //}

   
        public SalesReport(DateTime startDate, DateTime endDate, int startRowIndex, int maximumRows, string reportTableClass) 
        {
            this.ReportTableClass = reportTableClass;
            StartDate = startDate.Date;
            EndDate = endDate.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

            //Note that invoice can be listed in both categories - ndepends on items in invoice
            List<SalesReport_All_DataRow> data = new List<SalesReport_All_DataRow>();
            List<SalesReport_All_AggregateRow> aggs = new List<SalesReport_All_AggregateRow>();
            List<SalesReport_RefundRow> refs = new List<SalesReport_RefundRow>();
            List<SalesReport_GiftRow> gifts = new List<SalesReport_GiftRow>();
            List<Merch> giftItems = new List<Merch>();

           using (IDataReader dr = SPs.TxReportSalesAllInRangeDataAggs(_Config.APPLICATION_ID, 
               StartDate.ToString("MM/dd/yyyy hh:mmtt"), EndDate.ToString("MM/dd/yyyy hh:mmtt"), 
               startRowIndex, maximumRows).GetReader())
            {
                while (dr.Read())
                {
                    SalesReport_All_DataRow row = new SalesReport_All_DataRow(dr);
                    data.Add(row);
                }

                dr.NextResult();

                while (dr.Read())
                {
                    SalesReport_All_AggregateRow row = new SalesReport_All_AggregateRow(dr);
                    aggs.Add(row);
                }

                dr.NextResult();

                while (dr.Read())
                {
                    SalesReport_RefundRow row = new SalesReport_RefundRow(dr);
                    refs.Add(row);
                }

                dr.Close();
            }

            using (IDataReader dr = SPs.TxReportSalesGiftsInRange(_Config.APPLICATION_ID, 
                startDate.ToString("MM/dd/yyyy hh:mmtt"), endDate.ToString("MM/dd/yyyy hh:mmtt"), 
                startRowIndex, maximumRows).GetReader())
            {
               while (dr.Read())
                {
                    SalesReport_GiftRow row = new SalesReport_GiftRow(dr);
                    gifts.Add(row);
                }

                dr.Close();
            }

            this.AllSales_DataRow = data;
            this.AllSales_AggregateRow = aggs;
            this.AllSales_RefundRow = refs;
            this.AllSales_GiftRow = gifts;
        }
    }

    public class SalesReport_RefundRow
    {
        private int _lineRefunds = 0;
        private int _merchRefunds = 0;
        private int _ticketsRefunds = 0;
        private int _donationsRefunds = 0;
        private int _serviceRefunds = 0;
        private int _processingRefunds = 0;
        private int _merchShippingRefunds = 0;
        private int _ticketShippingRefunds = 0;
        private int _damageRefunds = 0;
        private int _otherRefunds = 0;

        private decimal _merchRefunded = 0;
        private decimal _ticketsRefunded = 0;
        private decimal _serviceRefunded = 0;
        private decimal _donationsRefunded = 0;
        private decimal _processingRefunded = 0; 
        private decimal _merchShippingRefunded = 0;
        private decimal _ticketShippingRefunded = 0;
        private decimal _damageRefunded = 0;
        private decimal _otherRefunded = 0;

        public int LineRefunds { get { return _lineRefunds; } set { _lineRefunds = value; } }
        public int MerchRefunds { get { return _merchRefunds; } set { _merchRefunds = value; } }
        public int TicketsRefunds { get { return _ticketsRefunds; } set { _ticketsRefunds = value; } }
        public int DonationsRefunds { get { return _donationsRefunds; } set { _donationsRefunds = value; } }
        public int ServiceRefunds { get { return _serviceRefunds; } set { _serviceRefunds = value; } }
        public int ProcessingRefunds { get { return _processingRefunds; } set { _processingRefunds = value; } }
        public int MerchShippingRefunds { get { return _merchShippingRefunds; } set { _merchShippingRefunds = value; } }
        public int TicketShippingRefunds { get { return _ticketShippingRefunds; } set { _ticketShippingRefunds = value; } }
        public int DamageRefunds { get { return _damageRefunds; } set { _damageRefunds = value; } }
        public int OtherRefunds { get { return _otherRefunds; } set { _otherRefunds = value; } }

        public decimal TotalRefunded { get { return _merchRefunded + TicketsRefunded + ServiceRefunded + ProcessingRefunded + 
            MerchShippingRefunded + TicketShippingRefunded + DamageRefunded + OtherRefunded; } }
        public decimal MerchRefunded { get { return _merchRefunded; } set { _merchRefunded = value; } }
        public decimal TicketsRefunded { get { return _ticketsRefunded; } set { _ticketsRefunded = value; } }
        public decimal DonationsRefunded { get { return _donationsRefunded; } set { _donationsRefunded = value; } }
        public decimal ServiceRefunded { get { return _serviceRefunded; } set { _serviceRefunded = value; } }
        public decimal ProcessingRefunded { get { return _processingRefunded; } set { _processingRefunded = value; } }
        public decimal MerchShippingRefunded { get { return _merchShippingRefunded; } set { _merchShippingRefunded = value; } }
        public decimal TicketShippingRefunded { get { return _ticketShippingRefunded; } set { _ticketShippingRefunded = value; } }
        public decimal DamageRefunded { get { return _damageRefunded; } set { _damageRefunded = value; } }
        public decimal OtherRefunded { get { return _otherRefunded; } set { _otherRefunded = value; } }

        public SalesReport_RefundRow(IDataReader dr)
        {
            try
            {
                LineRefunds = (int)dr.GetValue(dr.GetOrdinal("LineRefunds"));
                MerchRefunds = (int)dr.GetValue(dr.GetOrdinal("MerchRefunds"));
                TicketsRefunds = (int)dr.GetValue(dr.GetOrdinal("TicketsRefunds"));
                DonationsRefunds = (int)dr.GetValue(dr.GetOrdinal("DonationsRefunds"));
                ServiceRefunds = (int)dr.GetValue(dr.GetOrdinal("ServiceRefunds"));
                ProcessingRefunds = (int)dr.GetValue(dr.GetOrdinal("ProcessingRefunds"));
                MerchShippingRefunds = (int)dr.GetValue(dr.GetOrdinal("MerchShippingRefunds"));
                TicketShippingRefunds = (int)dr.GetValue(dr.GetOrdinal("TicketShippingRefunds"));
                DamageRefunds = (int)dr.GetValue(dr.GetOrdinal("DamageRefunds"));
                OtherRefunds = (int)dr.GetValue(dr.GetOrdinal("OtherRefunds"));

                MerchRefunded = (decimal)dr.GetValue(dr.GetOrdinal("MerchRefunded"));
                TicketsRefunded = (decimal)dr.GetValue(dr.GetOrdinal("TicketsRefunded"));
                DonationsRefunded = (decimal)dr.GetValue(dr.GetOrdinal("DonationsRefunded"));
                ServiceRefunded = (decimal)dr.GetValue(dr.GetOrdinal("ServiceRefunded"));
                ProcessingRefunded = (decimal)dr.GetValue(dr.GetOrdinal("ProcessingRefunded"));
                MerchShippingRefunded = (decimal)dr.GetValue(dr.GetOrdinal("MerchShippingRefunded"));
                TicketShippingRefunded = (decimal)dr.GetValue(dr.GetOrdinal("TicketShippingRefunded"));
                DamageRefunded = (decimal)dr.GetValue(dr.GetOrdinal("DamageRefunded"));
                OtherRefunded = (decimal)dr.GetValue(dr.GetOrdinal("OtherRefunded"));
            }
            catch (Exception ex)
            {
                _Error.LogException(ex);
            }
        }
    }

    public class SalesReport_GiftRow
    {   
        private int     _numGiftSold = 0;
        private decimal _giftMoneySold = 0;
        private decimal _storeCreditSpent = 0;
        private string  _giftSalesBreakdown = "";
        private int     _numGiftRedeemed = 0;
        private decimal _giftMoneyRedeemed = 0;
        private decimal _outstandingRedemptionMoney = 0;
        private int     _numStoreCreditHolders = 0;//
        private decimal _storeCreditInHolding = 0;

        public int      NumGiftSold { get { return _numGiftSold; } set { _numGiftSold = value; } }
        public decimal  GiftMoneySold { get { return _giftMoneySold; } set { _giftMoneySold = value; } }
        public decimal  StoreCreditSpent { get { return _storeCreditSpent; } set { _storeCreditSpent = value; } }
        public string   GiftSalesBreakdown { get { return _giftSalesBreakdown; } set { _giftSalesBreakdown = value; } }
        public int      NumGiftRedeemed { get { return _numGiftRedeemed; } set { _numGiftRedeemed = value; } }
        public decimal  GiftMoneyRedeemed { get { return _giftMoneyRedeemed; } set { _giftMoneyRedeemed = value; } }
        public decimal  OutstandingRedemptionMoney { get { return _outstandingRedemptionMoney; } set { _outstandingRedemptionMoney = value; } }
        public int      NumStoreCreditHolders { get { return _numStoreCreditHolders; } set { _numStoreCreditHolders = value; } }
        public decimal  StoreCreditInHolding { get { return _storeCreditInHolding; } set { _storeCreditInHolding = value; } }

        public SalesReport_GiftRow(IDataReader dr) 
        {
            try
            {
                NumGiftSold = (int)dr.GetValue(dr.GetOrdinal("NumGiftSold"));
                GiftMoneySold = (decimal)dr.GetValue(dr.GetOrdinal("GiftMoneySold"));
                StoreCreditSpent = (decimal)dr.GetValue(dr.GetOrdinal("StoreCreditSpent"));
                NumGiftRedeemed = (int)dr.GetValue(dr.GetOrdinal("NumGiftRedeemed"));
                GiftMoneyRedeemed = (decimal)dr.GetValue(dr.GetOrdinal("GiftMoneyRedeemed"));
                OutstandingRedemptionMoney = (decimal)dr.GetValue(dr.GetOrdinal("OutstandingRedemptionMoney"));
                NumStoreCreditHolders = (int)dr.GetValue(dr.GetOrdinal("NumStoreCreditHolders"));
                StoreCreditInHolding = (decimal)dr.GetValue(dr.GetOrdinal("StoreCreditInHolding"));                
            }
            catch(Exception ex)
            {
                _Error.LogException(ex);
            }
        }
    }

    public class SalesReport_All_BaseRow
    {   
        private int _linePurchases = 0;
        private int _itemsPurchased = 0;
        private int _merchPurchased = 0;
        private int _ticketsPurchased = 0;
        private int _donationsPurchased = 0;//
        private int _otherPurchased = 0;
        
        private decimal _baseSales = 0;
        private decimal _ticketPortion = 0;
        private decimal _merchPortion = 0;
        private decimal _donationPortion = 0;
        private decimal _otherPortion = 0; 
        private decimal _serviceCharge = 0;
        private decimal _adjustment = 0;
        private decimal _lineItemTotal = 0;

        private decimal _processingFee = 0;
        private decimal _totalPaid = 0;
        private decimal _netPaid = 0;
        private decimal _damaged = 0;

        private decimal _shipCharged = 0;
        private decimal _shipHandlingCalc = 0;
        private decimal _shipActual = 0;
        private int _shipments = 0;
        private decimal _shipDifferential = 0;
        private decimal _shipMerch = 0;
        private decimal _shipTicket = 0;

        public int LinePurchases { get { return _linePurchases; } set { _linePurchases = value; } }
        public int ItemsPurchased { get { return _itemsPurchased; } set { _itemsPurchased = value; } }
        public int MerchPurchased { get { return _merchPurchased; } set { _merchPurchased = value; } }
        public int TicketsPurchased { get { return _ticketsPurchased; } set { _ticketsPurchased = value; } }
        public int DonationsPurchased { get { return _donationsPurchased; } set { _donationsPurchased = value; } }
        public int OtherPurchased { get { return _otherPurchased; } set { _otherPurchased = value; } }
        
        public decimal BaseSales { get { return _baseSales; } set { _baseSales = value; } }
        public decimal TicketPortion { get { return _ticketPortion; } set { _ticketPortion = value; } }
        public decimal MerchPortion { get { return _merchPortion; } set { _merchPortion = value; } }
        public decimal DonationPortion { get { return _donationPortion; } set { _donationPortion = value; } }
        public decimal OtherPortion { get { return _otherPortion; } set { _otherPortion = value; } }
        public decimal ServiceCharge { get { return _serviceCharge; } set { _serviceCharge = value; } }
        public decimal Adjustment { get { return _adjustment; } set { _adjustment = value; } }
        public decimal LineItemTotal { get { return _lineItemTotal; } set { _lineItemTotal = value; } }
        
        public decimal ProcessingFee { get { return _processingFee; } set { _processingFee = value; } }
        public decimal TotalPaid { get { return _totalPaid; } set { _totalPaid = value; } }
        public decimal NetPaid { get { return _netPaid; } set { _netPaid = value; } }
        public decimal Damaged { get { return _damaged; } set { _damaged = value; } }

        public decimal ShipCharged { get { return _shipCharged; } set { _shipCharged = value; } }
        public decimal ShipHandlingCalc { get { return _shipHandlingCalc; } set { _shipHandlingCalc = value; } }
        public decimal ShipActual { get { return _shipActual; } set { _shipActual = value; } }
        public int Shipments { get { return _shipments; } set { _shipments = value; } }
        public decimal ShipDifferential { get { return _shipDifferential; } set { _shipDifferential = value; } }
        public decimal ShipMerch { get { return _shipMerch; } set { _shipMerch = value; } }
        public decimal ShipTicket { get { return _shipTicket; } set { _shipTicket = value; } }

        public SalesReport_All_BaseRow(IDataReader dr) 
        {
            try
            {
                LinePurchases = (int)dr.GetValue(dr.GetOrdinal("LinePurchases"));
                ItemsPurchased = (int)dr.GetValue(dr.GetOrdinal("ItemsPurchased"));
                MerchPurchased = (int)dr.GetValue(dr.GetOrdinal("MerchPurchased"));
                DonationsPurchased = (int)dr.GetValue(dr.GetOrdinal("DonationsPurchased"));
                TicketsPurchased = (int)dr.GetValue(dr.GetOrdinal("TicketsPurchased"));
                OtherPurchased = (int)dr.GetValue(dr.GetOrdinal("OtherPurchased"));
                
                BaseSales = (decimal)dr.GetValue(dr.GetOrdinal("BaseSales"));
                TicketPortion = (decimal)dr.GetValue(dr.GetOrdinal("TicketPortion"));
                MerchPortion = (decimal)dr.GetValue(dr.GetOrdinal("MerchPortion"));
                DonationPortion = (decimal)dr.GetValue(dr.GetOrdinal("DonationPortion"));
                OtherPortion = (decimal)dr.GetValue(dr.GetOrdinal("OtherPortion"));
                ServiceCharge = (decimal)dr.GetValue(dr.GetOrdinal("ServiceCharge"));
                Adjustment = (decimal)dr.GetValue(dr.GetOrdinal("Adjustment"));
                LineItemTotal = (decimal)dr.GetValue(dr.GetOrdinal("LineItemTotal"));

                ProcessingFee = (decimal)dr.GetValue(dr.GetOrdinal("ProcessingFee"));
                TotalPaid = (decimal)dr.GetValue(dr.GetOrdinal("TotalPaid"));
                NetPaid = (decimal)dr.GetValue(dr.GetOrdinal("NetPaid"));
                Damaged = (decimal)dr.GetValue(dr.GetOrdinal("Damaged"));

                ShipCharged = (decimal)dr.GetValue(dr.GetOrdinal("ShipCharged"));
                ShipHandlingCalc = (decimal)dr.GetValue(dr.GetOrdinal("ShipHandlingCalc"));
                ShipActual = (decimal)dr.GetValue(dr.GetOrdinal("ShipActual"));
                Shipments = (int)dr.GetValue(dr.GetOrdinal("Shipments"));
                ShipDifferential = (decimal)dr.GetValue(dr.GetOrdinal("ShipDifferential"));
                ShipMerch = (decimal)dr.GetValue(dr.GetOrdinal("ShipMerch"));
                ShipTicket = (decimal)dr.GetValue(dr.GetOrdinal("ShipTicket"));
            }
            catch (Exception ex)
            {
                _Error.LogException(ex);
            }
        }
    }

    public class SalesReport_All_DataRow : SalesReport_All_BaseRow
    {
        private int _invoiceId;

        public int InvoiceId { get { return _invoiceId; } set { _invoiceId = value; } }

        public SalesReport_All_DataRow(IDataReader dr) : base(dr)
        {
            InvoiceId = (int)dr.GetValue(dr.GetOrdinal("InvoiceId"));
        }
    }
    public class SalesReport_All_AggregateRow : SalesReport_All_BaseRow
    {
        private int _numInvoices;

        public int NumInvoices { get { return _numInvoices; } set { _numInvoices = value; } }

        public SalesReport_All_AggregateRow(IDataReader dr) : base(dr)
        {
            NumInvoices = (int)dr.GetValue(dr.GetOrdinal("NumInvoices"));
        }
    }

    #region SaveForLater
    public class SalesReport_Tickets_DataRow
    {
        public SalesReport_Tickets_DataRow() { }
    }
    public class SalesReport_Tickets_AggregateRow
    {
        public SalesReport_Tickets_AggregateRow() { }
    }

    public class SalesReport_Merch_DataRow
    {
        public SalesReport_Merch_DataRow() { }
    }
    public class SalesReport_Merch_AggregateRow
    {
        public SalesReport_Merch_AggregateRow() { }
    }
    #endregion

}