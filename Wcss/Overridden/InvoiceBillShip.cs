using System;
using System.Xml.Serialization;

namespace Wcss
{
    public partial class InvoiceBillShip
    {
        #region Properties

        [XmlAttribute("SameAsBilling")]
        public bool SameAsBilling
        {
            get { return this.BSameAsBilling; }
            set { this.BSameAsBilling = value; }
        }

        [XmlAttribute("DateShipped")]
        public DateTime DateShipped
        {
            get { return (!this.DtShipped.HasValue) ? DateTime.MaxValue : this.DtShipped.Value; }
            set { this.DtShipped = value; }
        }

        [XmlAttribute("ActualShipping")]
        public decimal ActualShipping
        {
            get { return (this.MActualShipping.HasValue) ? decimal.Round(this.MActualShipping.Value, 2) : 0; }
            set { this.MActualShipping = value; }
        }

        [XmlAttribute("HandlingComputed")]
        public decimal HandlingComputed
        {
            get { return (this.MHandlingComputed.HasValue) ? decimal.Round(this.MHandlingComputed.Value, 2) : 0; }
            set { this.MHandlingComputed = value; }
        }

        #endregion

        [XmlAttribute("Company_Working")]
        public string Company_Working
        {
            get { return (this.SameAsBilling) ? ((this.BlCompany != null) ? this.BlCompany.Trim() : string.Empty) :
                    ((this.CompanyName != null) ? this.CompanyName.Trim() : string.Empty);
            }
        }
        [XmlAttribute("FirstName_Working")]
        public string FirstName_Working { get { return (this.SameAsBilling) ? this.BlFirstName.Trim() : this.FirstName.Trim(); } }
        [XmlAttribute("LastName_Working")]
        public string LastName_Working { get { return (this.SameAsBilling) ? this.BlLastName.Trim() : this.LastName.Trim(); } }
        [XmlAttribute("FullName_Working")]
        public string FullName_Working { get { return string.Format("{0} {1}", this.FirstName_Working, this.LastName_Working).Trim(); } }
        [XmlAttribute("Email_Working")]
        public string Email_Working { get { return this.InvoiceRecord.PurchaseEmail.Trim(); } }
        [XmlAttribute("Address1_Working")]
        public string Address1_Working { get { return (this.SameAsBilling) ? this.BlAddress1.Trim() : this.Address1.Trim(); } }
        [XmlAttribute("Address2_Working")]
        public string Address2_Working
        {
            get
            {
                return (this.SameAsBilling) ? ((this.BlAddress2 != null) ? this.BlAddress2.Trim() : string.Empty) :
                    ((this.Address2 != null) ? this.Address2.Trim() : string.Empty);
            }
        }
        [XmlAttribute("City_Working")]
        public string City_Working { get { return (this.SameAsBilling) ? this.BlCity.Trim() : this.City.Trim(); } }
        [XmlAttribute("State_Working")]
        public string State_Working { get { return (this.SameAsBilling) ? this.BlStateProvince.Trim() : this.StateProvince.Trim(); } }
        [XmlAttribute("Zip_Working")]
        public string Zip_Working { get { return (this.SameAsBilling) ? this.BlPostalCode.Trim() : this.PostalCode.Trim(); } }
        [XmlAttribute("Country_Working")]
        public string Country_Working { get { return (this.SameAsBilling) ? this.BlCountry.Trim() : this.Country.Trim(); } }
        [XmlAttribute("Phone_Working")]
        public string Phone_Working { get { return (this.SameAsBilling) ? this.BlPhone.Trim() : this.Phone.Trim(); } }
        [XmlAttribute("ShipMessage_Working")]
        public string ShipMessage_Working { get { return (this.ShipMessage != null) ? this.ShipMessage.Trim() : string.Empty; } }


        private string _addresseeBilling = null;
        [XmlAttribute("Addressee_Billing")]
        public string Addressee_Billing
        {
            get
            {
                if (_addresseeBilling == null)
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append("<div style=\"border-bottom: solid #666 1px;\">");

                    if (this.BlCompany != null && this.BlCompany.Trim().Length > 0)
                        sb.AppendFormat("<div>{0}</div>", this.BlCompany.Trim());

                    sb.AppendFormat("<div>{0} {1}</div>", this.BlFirstName.Trim(), this.BlLastName.Trim());

                    sb.AppendFormat("<div>{0}</div>", this.BlAddress1.Trim());

                    if (this.BlAddress2 != null && this.BlAddress2.Trim().Length > 0)
                        sb.AppendFormat("<div>{0}</div>", this.BlAddress2.Trim());

                    sb.AppendFormat("<div>{0}, {1}</div>", this.BlCity.Trim(), this.BlStateProvince.Trim());
                    sb.AppendFormat("<div>{0} {1}</div>", this.BlPostalCode.Trim(), this.BlCountry.Trim());
                    sb.AppendFormat("<div>{0}</div>", this.BlPhone.Trim());

                    sb.Append("</div>");

                    _addresseeBilling = sb.ToString();
                }

                return _addresseeBilling;
            }
        }
    }
}
