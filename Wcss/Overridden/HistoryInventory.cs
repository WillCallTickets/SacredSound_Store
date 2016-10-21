using System;
using System.Xml.Serialization;

namespace Wcss
{
    public partial class HistoryInventory
    {
        [XmlAttribute("DateAdjusted")]
        public DateTime DateAdjusted
        {
            get { return this.DtAdjusted; }
            set { this.DtAdjusted = value; }
        }

        [XmlAttribute("CurrentlyAllotted")]
        public int CurrentlyAllotted
        {
            get { return this.ICurrentlyAllotted; }
            set { this.ICurrentlyAllotted = value; }
        }

        [XmlAttribute("Adjustment")]
        public int Adjustment
        {
            get { return this.IAdjustment; }
            set { this.IAdjustment = value; }
        }

        [XmlAttribute("Context")]
        public _Enums.HistoryInventoryContext Context
        {
            get { return (_Enums.HistoryInventoryContext)Enum.Parse(typeof(_Enums.HistoryInventoryContext), this.VcContext, true); }
            set { this.VcContext = value.ToString(); }
        }
    }
}
