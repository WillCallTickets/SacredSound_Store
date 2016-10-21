using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Wcss.QueryRow
{
    public class SalesReportMain
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
                    int maxCells = 4;
                    bool useHrAsSeparator = false;
                    Table t = new Table();
                    t.CellPadding = 0;
                    t.CellSpacing = 1;

                    if (ReportTableClass != null)
                        t.CssClass = ReportTableClass;

                    t.Rows.Add(ConstructHeaderRow(string.Format("<h2>{0} - Sales Report For Period <span>{1} thru {2}</span></h2>", 
                        _Config._Site_Entity_Name, 
                        this.StartDate.ToString("MM/dd/yyyy hh:mmtt"), this.EndDate.ToString("MM/dd/yyyy hh:mmtt")), maxCells));                    

                    //SALES ACTIVITY
                    t.Rows.Add(ConstructHeaderRow("<h4 class=\"rounded\">Sales Activity</h4>", maxCells));

                    t.Rows.Add(ConstructTableRow("Sales &amp; Item quantity", 
                        this.MainRow.NumInvoices.ToString(),
                        this.MainRow.NumItems.ToString(),
                        "Number of sales and quantity of items. Refunds not included."));
                    
                    
                    t.Rows.Add(ConstructSpacerRow(useHrAsSeparator, maxCells));

                    t.Rows.Add(ConstructTableRow("Tickets", this.MainRow.NumTickets.ToString(), this.MainRow.SalesTicket.ToString("n2")));
                    t.Rows.Add(ConstructTableRow("Add&#39;l Service Charges", this.MainRow.NumAdditionalServiceCharges.ToString(), 
                        this.MainRow.SalesAdditionalServiceCharge.ToString("n2")));
                    t.Rows.Add(ConstructTableRow("Service Charges", null, this.MainRow.SalesServiceCharge.ToString("n2")));
                    t.Rows.Add(ConstructTableRow("(Total Tickets)", null, this.MainRow.TotalTicket.ToString("n2")));
                    
                    t.Rows.Add(ConstructSpacerRow(useHrAsSeparator, maxCells));
                    
                    t.Rows.Add(ConstructTableRow("Merchandise", this.MainRow.NumMerch.ToString(), this.MainRow.SalesMerch.ToString("n2")));

                    t.Rows.Add(ConstructSpacerRow(useHrAsSeparator, maxCells));

                    t.Rows.Add(ConstructTableRow("Bundles", this.MainRow.NumBundles.ToString(), this.MainRow.SalesBundle.ToString("n2")));

                    t.Rows.Add(ConstructSpacerRow(useHrAsSeparator, maxCells));
                    
                    t.Rows.Add(ConstructTableRow("Ticket Shipping", this.MainRow.NumTicketShipping.ToString(), this.MainRow.SalesTicketShipping.ToString("n2")));
                    t.Rows.Add(ConstructTableRow("Merch Shipping", this.MainRow.NumMerchShipping.ToString(), this.MainRow.SalesMerchShipping.ToString("n2")));
                    t.Rows.Add(ConstructTableRow("(Total Shipping)", this.MainRow.NumTotalShipping.ToString(), this.MainRow.TotalShipping.ToString("n2")));
                    
                    t.Rows.Add(ConstructSpacerRow(useHrAsSeparator, maxCells));

                    t.Rows.Add(ConstructTableRow("Processing Fees", null, this.MainRow.SalesProcessingFee.ToString("n2"))); 
                    t.Rows.Add(ConstructTableRow("Donations", this.MainRow.NumDonations.ToString(), this.MainRow.SalesDonation.ToString("n2")));
                    t.Rows.Add(ConstructTableRow("Discounts", this.MainRow.NumDiscounts.ToString(), this.MainRow.SalesDiscount.ToString("n2")));
                    t.Rows.Add(ConstructTableRow("Other Items", this.MainRow.NumOther.ToString(), this.MainRow.SalesOther.ToString("n2")));
                    t.Rows.Add(ConstructTableRow("Adjustments", this.MainRow.NumAdjustments.ToString(), this.MainRow.AggAdjustment.ToString("n2")));

                    t.Rows.Add(ConstructSpacerRow(useHrAsSeparator, maxCells));

                    t.Rows.Add(ConstructTableRow("Gross Sales", this.MainRow.NumInvoices.ToString(), this.MainRow.AggTotalPaidOnInvoices.ToString("n2")));
                    t.Rows.Add(ConstructTableRow("Refunded", null, this.MainRow.AggRefunds.ToString("n2"), "Refunds applied to invoices within this period."));
                    t.Rows.Add(ConstructTableRow("Net Sales", null, this.MainRow.AggNetPaidOnInvoices.ToString("n2")));
                    t.Rows.Add(ConstructSpacerRow(useHrAsSeparator, maxCells));


                    //SHIPPING BREAKDOWN
                    t.Rows.Add(ConstructHeaderRow("<h4 class=\"rounded\">Shipping</h4>", maxCells));
                    t.Rows.Add(ConstructTableRow("Actual Shipping", null, this.MainRow.AggShipActual.ToString("n2"),
                        "Shipping charged in shipments."));
                    t.Rows.Add(ConstructTableRow("Calculated Handling", null, this.MainRow.AggShipHandlingCalculation.ToString("n2"),
                        "Handling charges added to shipping - by formula."));
                    t.Rows.Add(ConstructTableRow("Differential", null, this.MainRow.AggShipDifferential.ToString("n2"),
                        "Difference between actual and total."));
                    t.Rows.Add(ConstructSpacerRow(useHrAsSeparator, maxCells));


                    //STORE CREDIT AND GIFT CERTIFICATES
                    t.Rows.Add(ConstructHeaderRow("<h4 class=\"rounded\">Store Credit And Gift Certificates</h4>", maxCells));
                    t.Rows.Add(ConstructTableRow("Store Credit Income", null, this.GiftRow.StoreCreditSpent.ToString("n2"),
                        "Sales total for store credit."));                    
                    t.Rows.Add(ConstructTableRow("Gift Certificate Sales", this.GiftRow.NumGiftSold.ToString(), this.GiftRow.GiftMoneySold.ToString("n2")));
                    t.Rows.Add(ConstructTableRow("Amount Redeemed", this.GiftRow.NumGiftRedeemed.ToString(), this.GiftRow.GiftMoneyRedeemed.ToString("n2"),
                        "Gift certificate amount redeemed within the period."));
                    t.Rows.Add(ConstructTableRow("Period GCs Outstanding", null, this.GiftRow.OutstandingRedemptionMoney.ToString("n2"),
                        "Amount to be redeemed from gift certificates bought within the time period."));
                    t.Rows.Add(ConstructTableRow("Credit Outstanding", null, this.GiftRow.StoreCreditInHolding.ToString("n2"),
                        "Store credit in holding in user accounts - yet to be spent."));
                    t.Rows.Add(ConstructSpacerRow(useHrAsSeparator, maxCells));


                    //REFUNDS
                    t.Rows.Add(ConstructHeaderRow("<h4 class=\"rounded\">Refunds in this period</h4>", maxCells));

                    t.Rows.Add(ConstructTableRow("Total Refunds", 
                        string.Format("{0} / {1}", this.RefundRow.NumInvoices.ToString(), this.RefundRow.NumItems.ToString()), 
                        this.RefundRow.RefundedTotal.ToString("n2"), "Invoices / Items affected."));

                    t.Rows.Add(ConstructSpacerRow(useHrAsSeparator, maxCells));

                    t.Rows.Add(ConstructTableRow("Processing Fees", this.RefundRow.NumProcessingFees.ToString(), this.RefundRow.RefundedProcessingFees.ToString("n2")));
                    t.Rows.Add(ConstructTableRow("Tickets", this.RefundRow.NumTickets.ToString(), this.RefundRow.RefundedTickets.ToString("n2")));
                    t.Rows.Add(ConstructTableRow("Add&#39;l Service Charges", this.RefundRow.NumAdditionalServiceCharges.ToString(), ""));
                    t.Rows.Add(ConstructTableRow("Service Charges", null, this.RefundRow.RefundedServiceCharges.ToString("n2")));
                    t.Rows.Add(ConstructTableRow("Merchandise", this.RefundRow.NumMerch.ToString(), this.RefundRow.RefundedMerch.ToString("n2")));
                    t.Rows.Add(ConstructTableRow("Bundles", this.RefundRow.NumBundles.ToString(), this.RefundRow.RefundedBundles.ToString("n2")));
                    t.Rows.Add(ConstructTableRow("Donations", this.RefundRow.NumDonations.ToString(), this.RefundRow.RefundedDonations.ToString("n2")));
                    t.Rows.Add(ConstructTableRow("Damaged", this.RefundRow.NumDamaged.ToString(), this.RefundRow.RefundedDamaged.ToString("n2")));
                    t.Rows.Add(ConstructTableRow("Other", this.RefundRow.NumOther.ToString(), this.RefundRow.RefundedOther.ToString("n2"))); 
                    t.Rows.Add(ConstructTableRow("Ticket Shipping", this.RefundRow.NumTicketShipping.ToString(), this.RefundRow.RefundedTicketShipping.ToString("n2")));
                    t.Rows.Add(ConstructTableRow("Merch Shipping", this.RefundRow.NumMerchShipping.ToString(), this.RefundRow.RefundedMerchShipping.ToString("n2")));
                    
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
        private TableRow ConstructTableRow(string header, string numberOf, string reportedValue)
        {
            return ConstructTableRow(header, numberOf, reportedValue, null);
        }
        private TableRow ConstructTableRow(string header, string numberOf, string reportedValue, string description)
        {
            TableRow tr = new TableRow();

            TableHeaderCell th = new TableHeaderCell();            
            th.Text = header;

            TableCell tn = new TableCell();
            tn.Text = numberOf ?? "&nbsp;";

            TableCell td = new TableCell();
            td.Text = reportedValue;

            TableCell desc = new TableCell();
            desc.Text = description ?? "&nbsp;";

            tr.Cells.Add(th);
            tr.Cells.Add(tn);
            tr.Cells.Add(td);
            tr.Cells.Add(desc);

            return tr;
        }

        #endregion

        private Report_MainRow _mainRow = new Report_MainRow();
        private Report_RefundRow _refundRow = new Report_RefundRow();
        private Report_GiftRow _giftRow = new Report_GiftRow();
        public Report_MainRow MainRow { get { return _mainRow; } set { _mainRow = value;  } }
        public Report_RefundRow RefundRow { get { return _refundRow; } set { _refundRow = value; } }
        public Report_GiftRow GiftRow { get { return _giftRow; } set { _giftRow = value; } }

        public SalesReportMain(DateTime startDate, DateTime endDate, string reportTableClass) 
        {
            this.ReportTableClass = reportTableClass;
            StartDate = startDate.Date;
            EndDate = endDate.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

            using (IDataReader dr = SPs.TxReportSales(_Config.APPLICATION_ID,
                StartDate.ToString("MM/dd/yyyy hh:mmtt"), EndDate.ToString("MM/dd/yyyy hh:mmtt")
                ).GetReader())
            {
                while (dr.Read())
                    MainRow = new Report_MainRow(dr);
                dr.NextResult();

                while (dr.Read())
                    RefundRow = new Report_RefundRow(dr);
                dr.Close();
            }

           using (IDataReader dr = SPs.TxReportSalesGiftsInRange(_Config.APPLICATION_ID,
               startDate.ToString("MM/dd/yyyy hh:mmtt"), endDate.ToString("MM/dd/yyyy hh:mmtt"),
               0, 1000000).GetReader())
           {
               while (dr.Read())
                   GiftRow = new Report_GiftRow(dr);
               dr.Close();
           }
        }
    }
	
    public class Report_RefundRow
    {
        private int _numInvoices = 0;
        private int _numItems = 0;
        private int _numProcessingFees = 0;
        private int _numTickets = 0;
        private int _numAdditionalServiceCharges = 0;
        private int _numMerch = 0;
        private int _numBundles = 0;
        private int _numDonations = 0;
        private int _numTicketShipping = 0;
        private int _numMerchShipping = 0;
        private int _numOther = 0;
        private int _numDamaged = 0;

        private decimal _refundedProcessingFees = 0;
        private decimal _refundedTickets = 0;
        private decimal _refundedServiceCharges = 0;
        private decimal _refundedMerch = 0;
        private decimal _refundedBundles = 0;
        private decimal _refundedDonations = 0; 
        private decimal _refundedTicketShipping = 0;
        private decimal _refundedMerchShipping = 0;
        private decimal _refundedOther = 0;
        private decimal _refundedDamaged = 0;

        public int NumInvoices { get { return _numInvoices; } set { _numInvoices = value; } }
        public int NumItems { get { return _numItems; } set { _numItems = value; } }
        public int NumProcessingFees { get { return _numProcessingFees; } set { _numProcessingFees = value; } }
        public int NumTickets { get { return _numTickets; } set { _numTickets = value; } }
        public int NumAdditionalServiceCharges { get { return _numAdditionalServiceCharges; } set { _numAdditionalServiceCharges = value; } }
        public int NumMerch { get { return _numMerch; } set { _numMerch = value; } }
        public int NumBundles { get { return _numBundles; } set { _numBundles = value; } }
        public int NumDonations { get { return _numDonations; } set { _numDonations = value; } }
        public int NumTicketShipping { get { return _numTicketShipping; } set { _numTicketShipping = value; } }
        public int NumMerchShipping { get { return _numMerchShipping; } set { _numMerchShipping = value; } }
        public int NumOther { get { return _numOther; } set { _numOther = value; } }
        public int NumDamaged { get { return _numDamaged; } set { _numDamaged = value; } }

        //public decimal TotalRefunded { get { return _merchRefunded + TicketsRefunded + ServiceRefunded + ProcessingRefunded + 
        //    MerchShippingRefunded + TicketShippingRefunded + DamageRefunded + OtherRefunded; } }
        public decimal RefundedProcessingFees { get { return _refundedProcessingFees; } set { _refundedProcessingFees = value; } }
        public decimal RefundedTickets { get { return _refundedTickets; } set { _refundedTickets = value; } }
        public decimal RefundedServiceCharges { get { return _refundedServiceCharges; } set { _refundedServiceCharges = value; } }
        public decimal RefundedBundles { get { return _refundedBundles; } set { _refundedBundles = value; } }
        public decimal RefundedMerch { get { return _refundedMerch; } set { _refundedMerch = value; } }
        public decimal RefundedDonations { get { return _refundedDonations; } set { _refundedDonations = value; } }
        public decimal RefundedTicketShipping { get { return _refundedTicketShipping; } set { _refundedTicketShipping = value; } }
        public decimal RefundedMerchShipping { get { return _refundedMerchShipping; } set { _refundedMerchShipping = value; } }
        public decimal RefundedOther { get { return _refundedOther; } set { _refundedOther = value; } }
        public decimal RefundedDamaged { get { return _refundedDamaged; } set { _refundedDamaged = value; } }
        public decimal RefundedTotal { get { return RefundedProcessingFees + RefundedTickets + RefundedServiceCharges + 
            RefundedMerch + RefundedBundles + RefundedDonations + RefundedTicketShipping + RefundedMerchShipping + RefundedOther + RefundedDamaged; } }

        public Report_RefundRow() {}

        public Report_RefundRow(IDataReader dr)
        {
            try
            {
                NumInvoices = (int)dr.GetValue(dr.GetOrdinal("NumInvoices"));
                NumItems = (int)dr.GetValue(dr.GetOrdinal("NumItems"));
                NumProcessingFees = (int)dr.GetValue(dr.GetOrdinal("NumProcessingFees"));
                NumTickets = (int)dr.GetValue(dr.GetOrdinal("NumTickets"));
                NumAdditionalServiceCharges = (int)dr.GetValue(dr.GetOrdinal("NumAdditionalServiceCharges"));
                NumMerch = (int)dr.GetValue(dr.GetOrdinal("NumMerch"));
                NumBundles = (int)dr.GetValue(dr.GetOrdinal("NumBundles"));
                NumDonations = (int)dr.GetValue(dr.GetOrdinal("NumDonations"));
                NumTicketShipping = (int)dr.GetValue(dr.GetOrdinal("NumTicketShipping"));
                NumMerchShipping = (int)dr.GetValue(dr.GetOrdinal("NumMerchShipping"));
                NumOther = (int)dr.GetValue(dr.GetOrdinal("NumOther"));
                NumDamaged = (int)dr.GetValue(dr.GetOrdinal("NumDamaged"));

                RefundedProcessingFees = (decimal)dr.GetValue(dr.GetOrdinal("RefundedProcessingFees"));
                RefundedTickets = (decimal)dr.GetValue(dr.GetOrdinal("RefundedTickets"));
                RefundedServiceCharges = (decimal)dr.GetValue(dr.GetOrdinal("RefundedServiceCharges"));
                RefundedMerch = (decimal)dr.GetValue(dr.GetOrdinal("RefundedMerch"));
                RefundedBundles = (decimal)dr.GetValue(dr.GetOrdinal("RefundedBundles"));
                RefundedDonations = (decimal)dr.GetValue(dr.GetOrdinal("RefundedDonations"));
                RefundedTicketShipping = (decimal)dr.GetValue(dr.GetOrdinal("RefundedTicketShipping"));
                RefundedMerchShipping = (decimal)dr.GetValue(dr.GetOrdinal("RefundedMerchShipping"));
                RefundedOther = (decimal)dr.GetValue(dr.GetOrdinal("RefundedOther"));
                RefundedDamaged = (decimal)dr.GetValue(dr.GetOrdinal("RefundedDamaged"));
            }
            catch (Exception ex)
            {
                _Error.LogException(ex);
            }
        }
    }

    public class Report_GiftRow
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

        public Report_GiftRow() { }

        public Report_GiftRow(IDataReader dr) 
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

    public class Report_MainRow
    {   
        private int _numInvoices = 0;
        private int _numTickets = 0;
        private int _numAdditionalServiceCharges = 0;
        private int _numMerch = 0;
        private int _numBundles = 0;
        private int _numDonations = 0;
        private int _numTicketShipping = 0;
        private int _numMerchShipping = 0;
        private int _numDiscounts = 0;
        private int _numOther = 0;
        
        private decimal _salesProcessingFee = 0;
        private decimal _salesTicket = 0;
        private decimal _salesServiceCharge = 0;
        private decimal _salesAdditionalServiceCharge = 0;
        private decimal _salesMerch = 0;
        private decimal _salesBundle = 0;
        private decimal _salesDonation = 0;
        private decimal _salesTicketShipping = 0;
        private decimal _salesMerchShipping = 0;
        private decimal _salesDiscount = 0;
        private decimal _salesOther = 0;

        private decimal _aggTotalPaidOnInvoices = 0;
        private decimal _aggNetPaidOnInvoices = 0;
        private decimal _aggAdjustment = 0;
        private decimal _aggShipHandlingCalculation = 0;
        private decimal _aggShipActual = 0;
        private int _numShipments = 0;
        private int _numAdjustments = 0;
        private decimal _aggShipDifferential = 0;


        public int NumInvoices { get { return _numInvoices; } set { _numInvoices = value; } }
        public int NumItems { get { return NumTickets + NumAdditionalServiceCharges + NumMerch + NumBundles + NumDonations + NumTicketShipping + NumMerchShipping + NumDiscounts + NumOther; } }

        public int NumTickets { get { return _numTickets; } set { _numTickets = value; } }
        public int NumAdditionalServiceCharges { get { return _numAdditionalServiceCharges; } set { _numAdditionalServiceCharges = value; } }
        public int NumMerch { get { return _numMerch; } set { _numMerch = value; } }
        public int NumBundles { get { return _numBundles; } set { _numBundles = value; } }
        public int NumDonations { get { return _numDonations; } set { _numDonations = value; } }
        public int NumTicketShipping { get { return _numTicketShipping; } set { _numTicketShipping = value; } }
        public int NumMerchShipping { get { return _numMerchShipping; } set { _numMerchShipping = value; } }
        public int NumTotalShipping { get { return NumTicketShipping + NumMerchShipping; } }
        public int NumDiscounts { get { return _numDiscounts; } set { _numDiscounts = value; } }
        public int NumOther { get { return _numOther; } set { _numOther = value; } }

        public decimal SalesProcessingFee { get { return _salesProcessingFee; } set { _salesProcessingFee = value; } }

        public decimal SalesTicket { get { return _salesTicket; } set { _salesTicket = value; } }
        public decimal SalesServiceCharge { get { return _salesServiceCharge; } set { _salesServiceCharge = value; } }
        public decimal SalesAdditionalServiceCharge { get { return _salesAdditionalServiceCharge; } set { _salesAdditionalServiceCharge = value; } }
        public decimal TotalTicket { get { return SalesTicket + SalesServiceCharge + SalesAdditionalServiceCharge; } }
        
        public decimal SalesMerch { get { return _salesMerch; } set { _salesMerch = value; } }
        public decimal SalesBundle { get { return _salesBundle; } set { _salesBundle = value; } }
        public decimal SalesDonation { get { return _salesDonation; } set { _salesDonation = value; } }
        
        public decimal TotalShipping { get { return SalesTicketShipping + SalesMerchShipping; } }
        public decimal SalesTicketShipping { get { return _salesTicketShipping; } set { _salesTicketShipping = value; } }
        public decimal SalesMerchShipping { get { return _salesMerchShipping; } set { _salesMerchShipping = value; } }
        public decimal SalesDiscount { get { return _salesDiscount; } set { _salesDiscount = value; } }
        public decimal SalesOther { get { return _salesOther; } set { _salesOther = value; } }

        public decimal AggTotalPaidOnInvoices { get { return _aggTotalPaidOnInvoices; } set { _aggTotalPaidOnInvoices = value; } }
        public decimal AggNetPaidOnInvoices { get { return _aggNetPaidOnInvoices; } set { _aggNetPaidOnInvoices = value; } }
        public decimal AggRefunds { get { return AggTotalPaidOnInvoices - AggNetPaidOnInvoices; } }
        public decimal AggAdjustment { get { return _aggAdjustment; } set { _aggAdjustment = value; } }
        public decimal AggShipHandlingCalculation { get { return _aggShipHandlingCalculation; } set { _aggShipHandlingCalculation = value; } }
        public decimal AggShipActual { get { return _aggShipActual; } set { _aggShipActual = value; } }
        public int NumShipments { get { return _numShipments; } set { _numShipments = value; } }
        public int NumAdjustments { get { return _numAdjustments; } set { _numAdjustments = value; } }
        public decimal AggShipDifferential { get { return _aggShipDifferential; } set { _aggShipDifferential = value; } }


        public Report_MainRow() {}

        public Report_MainRow(IDataReader dr) 
        {
            try
            {
                NumInvoices = (int)dr.GetValue(dr.GetOrdinal("NumInvoices"));
                NumTickets = (int)dr.GetValue(dr.GetOrdinal("NumTickets"));                
                NumAdditionalServiceCharges = (int)dr.GetValue(dr.GetOrdinal("NumAdditionalServiceCharges"));
                NumMerch = (int)dr.GetValue(dr.GetOrdinal("NumMerch"));
                NumBundles = (int)dr.GetValue(dr.GetOrdinal("NumBundles"));
                NumDonations = (int)dr.GetValue(dr.GetOrdinal("NumDonations"));
                NumTicketShipping = (int)dr.GetValue(dr.GetOrdinal("NumTicketShipping"));
                NumMerchShipping = (int)dr.GetValue(dr.GetOrdinal("NumMerchShipping"));
                NumOther = (int)dr.GetValue(dr.GetOrdinal("NumOther"));
                NumDiscounts = (int)dr.GetValue(dr.GetOrdinal("NumDiscounts"));

                SalesProcessingFee = (decimal)dr.GetValue(dr.GetOrdinal("SalesProcessingFee"));
                SalesTicket = (decimal)dr.GetValue(dr.GetOrdinal("SalesTicket"));
                SalesServiceCharge = (decimal)dr.GetValue(dr.GetOrdinal("SalesServiceCharge"));
                SalesAdditionalServiceCharge = (decimal)dr.GetValue(dr.GetOrdinal("SalesAdditionalServiceCharge"));
                SalesMerch = (decimal)dr.GetValue(dr.GetOrdinal("SalesMerch"));
                SalesBundle = (decimal)dr.GetValue(dr.GetOrdinal("SalesBundle"));
                SalesDonation = (decimal)dr.GetValue(dr.GetOrdinal("SalesDonation"));
                SalesTicketShipping = (decimal)dr.GetValue(dr.GetOrdinal("SalesTicketShipping"));
                SalesMerchShipping = (decimal)dr.GetValue(dr.GetOrdinal("SalesMerchShipping"));
                SalesDiscount = (decimal)dr.GetValue(dr.GetOrdinal("SalesDiscount"));
                SalesOther = (decimal)dr.GetValue(dr.GetOrdinal("SalesOther"));

                AggTotalPaidOnInvoices = (decimal)dr.GetValue(dr.GetOrdinal("AggTotalPaidOnInvoices"));
                AggNetPaidOnInvoices = (decimal)dr.GetValue(dr.GetOrdinal("AggNetPaidOnInvoices"));
                AggAdjustment = (decimal)dr.GetValue(dr.GetOrdinal("AggAdjustment"));
                AggShipHandlingCalculation = (decimal)dr.GetValue(dr.GetOrdinal("AggShipHandlingCalculation"));
                NumShipments = (int)dr.GetValue(dr.GetOrdinal("NumShipments"));
                NumAdjustments = (int)dr.GetValue(dr.GetOrdinal("NumAdjustments")); 
                AggShipDifferential = (decimal)dr.GetValue(dr.GetOrdinal("AggShipDifferential"));
                
            }
            catch (Exception ex)
            {
                _Error.LogException(ex);
            }
        }
    }

}