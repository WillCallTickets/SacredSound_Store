using System;
using System.Configuration;
using System.Xml.Serialization;

namespace Wcss
{
    public partial class ChargeStatement
    {
        #region Rates

        //From Web.Config file
        public static decimal _Rate_PerSale { get { return decimal.Parse(ConfigurationManager.AppSettings["Rate_PerSale"]); } }
        public static decimal _Rate_PerRefund { get { return decimal.Parse(ConfigurationManager.AppSettings["Rate_PerRefund"]); } }
        public static decimal _Rate_PctGrossSalesThreshhold { get { return decimal.Parse(ConfigurationManager.AppSettings["Rate_PctGrossSalesThreshhold"]); } }
        public static decimal _Rate_PctGrossSales { get { return decimal.Parse(ConfigurationManager.AppSettings["Rate_PctGrossSales"]); } }
        public static decimal _Rate_PerTicketInvoice { get { return decimal.Parse(ConfigurationManager.AppSettings["Rate_PerTicketInvoice"]); } }
        public static decimal _Rate_PerTicketUnit { get { return decimal.Parse(ConfigurationManager.AppSettings["Rate_PerTicketUnit"]); } }
        public static decimal _Rate_PctTicketSales { get { return decimal.Parse(ConfigurationManager.AppSettings["Rate_PctTicketSales"]); } }
        public static decimal _Rate_PerMerchInvoice { get { return decimal.Parse(ConfigurationManager.AppSettings["Rate_PerMerchInvoice"]); } }
        public static decimal _Rate_PerMerchUnit { get { return decimal.Parse(ConfigurationManager.AppSettings["Rate_PerMerchUnit"]); } }
        public static decimal _Rate_PctMerchSales { get { return decimal.Parse(ConfigurationManager.AppSettings["Rate_PctMerchSales"]); } }
        public static decimal _Rate_PerTktShip { get { return decimal.Parse(ConfigurationManager.AppSettings["Rate_PerTktShip"]); } }
        public static decimal _Rate_PctTktShipSales { get { return decimal.Parse(ConfigurationManager.AppSettings["Rate_PctTktShipSales"]); } }
        public static decimal _Rate_PerSubscription { get { return decimal.Parse(ConfigurationManager.AppSettings["Rate_PerSubscription"]); } }
        public static decimal _Rate_PerMailSent { get { return decimal.Parse(ConfigurationManager.AppSettings["Rate_PerMailSent"]); } }
        public static decimal _Rate_Hourly { get { return decimal.Parse(ConfigurationManager.AppSettings["Rate_Hourly"]); } }

        #endregion

        [XmlAttribute("Month")]
        public int Month
        {
            get { return this.IMonth; }
            set { this.IMonth = value;  }
        }
        [XmlAttribute("Year")]
        public int Year
        {
            get { return this.IYear; }
            set { this.IYear = value; }
        }
        [XmlAttribute("DatePaid")]
        public DateTime DatePaid
        {
            get { return (this.DtPaid.HasValue) ? this.DtPaid.Value : DateTime.MaxValue; }
            set { this.DtPaid = value; }
        }

        public DateTime StartDate
        {
            get
            {
                return DateTime.Parse(string.Format("{0}/1/{1}", this.Month, this.Year));
            }
        }
        public DateTime EndDate
        {
            get
            {
                return this.StartDate.AddMonths(1).AddSeconds(-1);
            }
        }
        public decimal SubTotal
        {
            get
            {
                decimal grs = (this.Gross >= this.GrossThreshhold) ? this.Gross * this.GrossPct : 0;

                return this.SalesQty * this.SalesQtyPct +
                    this.RefundQty * this.RefundQtyPct +
                    grs +
                    this.TicketInvoiceQty * this.TicketInvoicePct +
                    this.TicketUnitQty * this.TicketUnitPct +
                    this.TicketSales * this.TicketSalesPct +
                    this.MerchInvoiceQty * this.MerchInvoicePct +
                    this.MerchUnitQty * this.MerchUnitPct +
                    this.MerchSales * this.MerchSalesPct +
                    this.ShipUnitQty * ShipUnitPct +
                    this.ShipSales * this.ShipSalesPct +
                    ((this.MailerPortion.HasValue) ? 0 : this.MailerPortion.Value) + 
                    this.HourlyPortion;
            }
        }
        public decimal Total
        {
            get
            {
                return this.SubTotal - this.Discount;
            }
        }
    }
}

