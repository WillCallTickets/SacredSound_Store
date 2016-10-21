using System;
using System.Xml.Serialization;
using System.Web.Profile;

using Wcss;

namespace WillCallWeb
{
    public class _Profile : ProfileCommon
    {
        #region Properties
        
        [XmlAttribute("ContactInfo")]
        public ProfileGroupBase ContactInfo { get { return base.GetProfileGroup("ContactInfo"); } }
        [XmlAttribute("Preferences")]
        public ProfileGroupBase Preferences { get { return base.GetProfileGroup("Preferences"); } }
        [XmlAttribute("LegacyData")]
        public ProfileGroupBase LegacyData { get { return base.GetProfileGroup("LegacyData"); } }


        [XmlAttribute("FirstName")]
        public string FirstName
        {
            get { return base.GetPropertyValue("FirstName").ToString(); }
            set { if(value == null || value.Trim().Length == 0 )
                throw new Exception("The FirstName property is required."); 
                base.SetPropertyValue("FirstName",value); }
        }
        [XmlAttribute("LastName")]
        public string LastName
        {
            get { return base.GetPropertyValue("LastName").ToString(); }
            set
            {
                if (value == null || value.Trim().Length == 0)
                    throw new Exception("The LastName property is required.");
                base.SetPropertyValue("LastName", value);
            }
        }

        public string FullName 
        {
            get
            {
                return string.Format("{0} {1}", this.FirstName, this.LastName);
            }
        }
        public string FullName_LastFirst
        {
            get
            {
                return string.Format("{1}, {0}", this.FirstName, this.LastName);
            }
        }
        

        [XmlAttribute("EmailAddress")]
        public string EmailAddress
        {
            get { return this.UserName; }
        }
        [XmlAttribute("Gender")]
        public _Enums.GenderTypes Gender
        {
            get 
            { 
                string gender = base.GetPropertyValue("Gender").ToString();
                if (gender == null || gender.Trim().Length == 0)
                    return _Enums.GenderTypes.noneSpecified;
                return (_Enums.GenderTypes)Enum.Parse(typeof(_Enums.GenderTypes), gender, true);
            }
            set { base.SetPropertyValue("Gender", (value == _Enums.GenderTypes.noneSpecified) ? string.Empty : value.ToString()); }
        }
        [XmlAttribute("DateOfBirth")]
        public string DateOfBirth
        {
            get 
            { 
                string dob = base.GetPropertyValue("DateOfBirth").ToString();
                if (dob == null || dob.Trim().Length == 0)
                    return string.Empty;
                return dob;
            }
            set
            {
                if (value == null || value.Trim().Length == 0 || (!Utils.Validation.IsDate(value)))
                    value = string.Empty;
                base.SetPropertyValue("DateOfBirth", value);
            }
        }
        [XmlAttribute("Comments")]
        public string Comments
        {
            get { return base.GetPropertyValue("Comments").ToString(); }
            set
            {
                if (value == null || value.Trim().Length == 0)
                    value = string.Empty;
                base.SetPropertyValue("Comments", value);
            }
        }

        [XmlAttribute("Address1")]
        public string Address1
        {
            get { return ContactInfo.GetPropertyValue("Address1").ToString(); }
            set
            {
                if (value == null || value.Trim().Length == 0)
                    value = string.Empty;
                ContactInfo.SetPropertyValue("Address1", value);
            }
        }
        [XmlAttribute("Address2")]
        public string Address2
        {
            get { return ContactInfo.GetPropertyValue("Address2").ToString(); }
            set
            {
                if (value == null || value.Trim().Length == 0)
                    value = string.Empty;
                ContactInfo.SetPropertyValue("Address2", value);
            }
        }
        [XmlAttribute("City")]
        public string City
        {
            get { return ContactInfo.GetPropertyValue("City").ToString(); }
            set
            {
                if (value == null || value.Trim().Length == 0)
                    value = string.Empty;
                ContactInfo.SetPropertyValue("City", value);
            }
        }
        [XmlAttribute("State")]
        public string State
        {
            get { return ContactInfo.GetPropertyValue("State").ToString(); }
            set
            {
                if (value == null || value.Trim().Length == 0)
                    value = string.Empty;
                ContactInfo.SetPropertyValue("State", value);
            }
        }
        [XmlAttribute("PostalCode")]
        public string PostalCode
        {
            get { return ContactInfo.GetPropertyValue("PostalCode").ToString(); }
            set
            {
                if (value == null || value.Trim().Length == 0)
                    value = string.Empty;
                ContactInfo.SetPropertyValue("PostalCode", value);
            }
        }
        [XmlAttribute("Country")]
        public string Country
        {
            get { return ContactInfo.GetPropertyValue("Country").ToString(); }
            set
            {
                if (value == null || value.Trim().Length == 0)
                    value = string.Empty;
                ContactInfo.SetPropertyValue("Country", value);
            }
        }
        [XmlAttribute("Phone")]
        public string Phone
        {
            get { return ContactInfo.GetPropertyValue("Phone").ToString(); }
            set
            {
                if (value == null || value.Trim().Length == 0)
                    value = string.Empty;
                ContactInfo.SetPropertyValue("Phone", value);
            }
        }

        [XmlAttribute("Theme")]
        public string Theme
        {
            get { return Preferences.GetPropertyValue("Theme").ToString(); }
            set
            {
                if (value == null || value.Trim().Length == 0)
                    value = string.Empty;
                Preferences.SetPropertyValue("Theme", value);
            }
        }
        [XmlAttribute("Culture")]
        public string Culture
        {
            get { return Preferences.GetPropertyValue("Culture").ToString(); }
            set
            {
                if (value == null || value.Trim().Length == 0)
                    value = string.Empty;
                Preferences.SetPropertyValue("Culture", value);
            }
        }
        [XmlAttribute("EmailFormat")]
        public _Enums.EmailFormats EmailFormat
        {
            get 
            { 
                string format = Preferences.GetPropertyValue("EmailFormat").ToString();
                if (format == null || format.Trim().Length == 0)
                    return _Enums.EmailFormats.html;
                return (_Enums.EmailFormats)Enum.Parse(typeof(_Enums.EmailFormats), format, true);
            }
            set { Preferences.SetPropertyValue("EmailFormat", value.ToString()); }
        }

        [XmlAttribute("MemberSince")]
        public DateTime MemberSince
        {
            get { return (DateTime)LegacyData.GetPropertyValue("MemberSince"); }
            set { LegacyData.SetPropertyValue("MemberSince", value); }
        }
        [XmlAttribute("OldCustomerId")]
        public int OldCustomerId
        {
            get { return (int)LegacyData.GetPropertyValue("OldCustomerId"); }
            set { LegacyData.SetPropertyValue("OldCustomerId", value); }
        }
        
        #endregion
    }
}
