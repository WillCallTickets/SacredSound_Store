using System;
using System.Xml.Serialization;

namespace Wcss
{
    public partial class MerchStock
    {
        [XmlAttribute("Quantity")]
        public int Quantity
        {
            get { return this.IQty; }
            set { this.IQty = value; }
        }
        [XmlAttribute("TimeToLive")]
        public DateTime TimeToLive
        {
            get { return this.DtTTL; }
            set { this.DtTTL = value; }
        }
    }
}
