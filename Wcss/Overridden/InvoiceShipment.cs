using System;
using System.Text;
using System.Xml.Serialization;
using SubSonic;

namespace Wcss
{
    /// <summary>
    /// This class contains items that are created NOT VIA the ORDER FLOW, but through
    /// the admin - when shipments are being set up and fulfilled
    /// </summary>
    public partial class InvoiceShipment
    {
        #region Props

        [XmlAttribute("DateCreated")]
        public DateTime DateCreated { get { return this.DtCreated; } set { this.DtCreated = value; } }

        [XmlAttribute("Context")]
        public _Enums.ProductContext Context
        {
            get { return (_Enums.ProductContext)Enum.Parse(typeof(_Enums.ProductContext), this.VcContext, true); }
            set { this.VcContext = value.ToString().ToLower(); } }

        [XmlAttribute("IsLabelPrinted")]
        public bool IsLabelPrinted
        {
            get { return this.BLabelPrinted; }
            set { this.BLabelPrinted = value; }
        }

        [XmlAttribute("Carrier")]
        public _Enums.ShippingCarrier Carrier
        {
            get { return (_Enums.ShippingCarrier)Enum.Parse(typeof(_Enums.ShippingCarrier), this.VcCarrier, true); }
            set { this.VcCarrier = value.ToString(); }
        }
        
        [XmlAttribute("ReturnedToSender")]
        public bool ReturnedToSender
        {
            get { return (this.BRTS.HasValue) ? BRTS.Value : false; }
            set { this.BRTS = value; }
        }

        [XmlAttribute("DateShipped")]
        public DateTime DateShipped { get { return (this.DtShipped.HasValue) ? this.DtShipped.Value : DateTime.MaxValue; } 
            set { this.DtShipped = value; } }

        [XmlAttribute("WeightCalculated")]
        public decimal WeightCalculated { get { return decimal.Round(this.MWeightCalculated,2); } set { this.MWeightCalculated = decimal.Round(value,2); } }

        [XmlAttribute("WeightActual")]
        public decimal WeightActual { get { return decimal.Round(this.MWeightActual, 2); } set { this.MWeightActual = decimal.Round(value, 2); } }

        [XmlAttribute("HandlingCalculated")]
        public decimal HandlingCalculated { get { return decimal.Round(this.MHandlingCalculated, 2); } set { this.MHandlingCalculated = decimal.Round(value, 2); } }

        [XmlAttribute("ShippingCharged")]
        public decimal ShippingCharged { get { return decimal.Round(this.MShippingCharged, 2); } set { this.MShippingCharged = decimal.Round(value, 2); } }

        [XmlAttribute("ShippingActual")]
        public decimal ShippingActual { get { return decimal.Round(this.MShippingActual, 2); } set { this.MShippingActual = decimal.Round(value, 2); } }

        private string _addressee = null;
        [XmlAttribute("Addressee")]
        public string Addressee
        {
            get
            {
                if (_addressee == null)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<div style=\"border-bottom: solid #666 1px;\">");

                    if(this.CompanyName != null && this.CompanyName.Trim().Length > 0)
                        sb.AppendFormat("<div>{0}</div>", this.CompanyName.Trim());

                    sb.AppendFormat("<div>{0} {1}</div>", FirstName.Trim(), LastName.Trim());

                    sb.AppendFormat("<div>{0}</div>", Address1.Trim());

                    if (this.Address2 != null && this.Address2.Trim().Length > 0)
                        sb.AppendFormat("<div>{0}</div>", this.Address2.Trim());

                    sb.AppendFormat("<div>{0}, {1}</div>", this.City.Trim(), this.StateProvince.Trim());
                    sb.AppendFormat("<div>{0} {1}</div>", this.PostalCode.Trim(), this.Country.Trim());
                    sb.AppendFormat("<div>{0}</div>", this.Phone.Trim());

                    sb.Append("</div>");

                    _addressee = sb.ToString();
                }

                return _addressee;
            }
        }

        #endregion

        public static InvoiceShipmentCollection GetInvoiceShipmentsInRange(_Enums.ProductContext context,
             DateTime startDate, DateTime endDate, int startRowIndex, int maximumRows)
        {
            startDate = startDate.Date;
            endDate = endDate.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

            //Note that invoice can be listed in both categories - ndepends on items in invoice
            InvoiceShipmentCollection coll = new InvoiceShipmentCollection();

            coll.LoadAndCloseReader(SPs.TxGetShipmentsInRange(_Config.APPLICATION_ID, context.ToString(), 
                startDate.ToString("MM/dd/yyyy hh:mmtt"), endDate.ToString("MM/dd/yyyy hh:mmtt"), 
                startRowIndex, maximumRows).GetReader());

            return coll;
        }
        public static int GetInvoiceShipmentsInRangeCount(_Enums.ProductContext context, DateTime startDate, DateTime endDate)
        {
            //Note that invoice can be listed in both categories - ndepends on items in invoice
            int count = 0;
            endDate = endDate.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

            using (System.Data.IDataReader dr = SPs.TxGetShipmentsInRangeCount(_Config.APPLICATION_ID, context.ToString(), 
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
    }
}

