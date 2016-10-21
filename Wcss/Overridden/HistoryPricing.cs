using System;
using System.Xml.Serialization;

namespace Wcss
{
    public partial class HistoryPricing
    {
        [XmlAttribute("DateAdjusted")]
        public DateTime DateAdjusted
        {
            get { return this.DtAdjusted; }
            set { this.DtAdjusted = value; }
        }

        [XmlAttribute("OldPrice")]
        public decimal OldPrice
        {
            get { return decimal.Round(this.MOldPrice, 2); }
            set { this.MOldPrice = decimal.Round(value, 2); }
        }

        [XmlAttribute("NewPrice")]
        public decimal NewPrice
        {
            get { return decimal.Round(this.MNewPrice, 2); }
            set { this.MNewPrice = decimal.Round(value, 2); }
        }

        [XmlAttribute("Context")]
        public _Enums.HistoryInventoryContext Context
        {
            get { return (_Enums.HistoryInventoryContext)Enum.Parse(typeof(_Enums.HistoryInventoryContext), this.VcContext, true); }
            set { this.VcContext = value.ToString(); }
        }
    }
}
