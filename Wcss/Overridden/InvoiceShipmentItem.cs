using System;
using System.Xml.Serialization;

namespace Wcss
{
    /// <summary>
    /// This class contains items that are created NOT VIA the ORDER FLOW, but through
    /// the admin - when shipments are being set up and fulfilled
    /// </summary>
    public partial class InvoiceShipmentItem
    {
        #region Properties

        [XmlAttribute("Quantity")]
        public int Quantity
        {
            get { return this.IQuantity; }
            set { this.IQuantity = value; }
        }

        #endregion
    }
}
