using System;
using System.Xml.Serialization;

namespace Wcss
{
    public partial class HistorySubscriptionEmail
    {
        [XmlAttribute("DateSent")]
        public DateTime DateSent
        {
            get { return this.DtSent; }
            set { this.DtSent = value; }
        }

        [XmlAttribute("Recipients")]
        public int Recipients
        {
            get { return this.IRecipients; }
            set { this.IRecipients = value; }
        }
    }
}
