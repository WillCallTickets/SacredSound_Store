using System;
using System.Xml.Serialization;

namespace Wcss
{
    public partial class InvoiceTransaction
    {
        [XmlAttribute("Amount")]
        public decimal Amount
        {
            get { return decimal.Round(this.MAmount, 2); }
            set { this.MAmount = value; }
        }
    }
}
