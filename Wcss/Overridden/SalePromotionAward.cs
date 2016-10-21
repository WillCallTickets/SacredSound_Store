using System;
using System.Xml.Serialization;

namespace Wcss
{
    public partial class SalePromotionAward
    {
        #region Table Properties

        [XmlAttribute("IsActive")]
        public bool IsActive
        {
            get { return this.BActive; }
            set { this.BActive = value; }
        }

        #endregion
    }
}
