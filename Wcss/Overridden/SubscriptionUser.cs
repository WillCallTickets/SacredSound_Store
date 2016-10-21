using System;
using System.Xml.Serialization;

namespace Wcss
{
    public partial class SubscriptionUser
    {
        [XmlAttribute("IsSubscribed")]
        public bool IsSubscribed
        {
            get { return this.BSubscribed; }
            set { this.BSubscribed = value; }
        }
        [XmlAttribute("LastActionDate")]
        public DateTime LastActionDate
        {
            get { return (this.DtLastActionDate.HasValue) ? this.DtLastActionDate.Value : DateTime.MaxValue; }
            set { this.DtLastActionDate = value; }
        }
        [XmlAttribute("IsHtmlFormat")]
        public bool IsHtmlFormat
        {
            get { return this.BHtmlFormat; }
            set { this.BHtmlFormat = value; }
        }
        [XmlAttribute("EmailAddress")]
        public string EmailAddress
        {
            get 
            { 
                return this.AspnetUserRecord.LoweredUserName;
            }
        }
    }
}
