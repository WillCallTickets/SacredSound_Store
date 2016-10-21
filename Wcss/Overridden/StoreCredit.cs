using System;
using System.Xml.Serialization;
using System.Web.Security;
using System.Web.Profile;

namespace Wcss
{
    public partial class StoreCredit
    {
        #region Properties

        [XmlAttribute("DateStamp")]
        public DateTime DateStamp
        {
            get { return this.DtStamp; }
            set { this.DtStamp = value; }
        }
        [XmlAttribute("Amount")]
        public decimal Amount
        {
            get { return decimal.Round(this.MAmount, 2); }
            set { this.MAmount = decimal.Round(value, 2); }
        }
        //no need to have a status as we will just inject a credit line to adjust for any returns/exchanges
        //[XmlAttribute("PurchaseAction")]

        /// <summary>
        /// Because invoice item is linked by a column that is not a primary key, we have to create a method to retrieve the matching invoiceItem
        /// </summary>
        /// <returns></returns>
        private InvoiceItem _invoiceItemRecord;
        public InvoiceItem InvoiceItemRecord
        {
            get
            {
                if (_invoiceItemRecord == null)
                {
                    if (this.RedemptionId != null && this.RedemptionId != Guid.Empty)
                    {
                        _invoiceItemRecord = new InvoiceItem();
                        _invoiceItemRecord.LoadAndCloseReader(FetchByParameter("Guid", this.RedemptionId.Value));
                    }
                }

                return _invoiceItemRecord;
            }
        }

        #endregion

        //for methods see GiftCertificates in AppCode dir
    }
}
