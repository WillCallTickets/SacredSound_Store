using System;
using System.Web.UI.WebControls;

using Wcss;

namespace WillCallWeb.Controls
{
    public partial class UserProfile : WillCallWeb.BaseControl
   {
       protected ProfileCommon profile;

      private string _userName = "";
      public string UserName
      {
         get { return _userName; }
         set { _userName = value; }
      }

      protected void Page_Init(object sender, EventArgs e)
      {
         this.Page.RegisterRequiresControlState(this);

         profile = this.Profile;
      }

      protected override void LoadControlState(object savedState)
      {
         object[] ctlState = (object[])savedState;
         base.LoadControlState(ctlState[0]);
         this.UserName = (string)ctlState[1];
      }

      protected override object SaveControlState()
      {
         object[] ctlState = new object[2];
         ctlState[0] = base.SaveControlState();
         ctlState[1] = this.UserName;
         return ctlState;
      }
      protected bool DisplayUserSubscriptions { get { return (this.Page.ToString().ToLower() != "asp.register_aspx" && this.Page.ToString().ToLower() != "asp.admin_edituser_aspx"); } }

      public void BindPage(string userName)
      {
          this.UserName = userName;
          
          if (this.UserName.Length > 0)
          {
              profile = this.Profile.GetProfile(this.UserName);
              UserSubscriptions1.UserName = userName;
          }

          UserSubscriptions1.AllowAdminSubscriptions = true;
          UserSubscriptions1.DataBind();
      }
      protected void Page_Load(object sender, EventArgs e)
      {
          if (!this.IsPostBack)
          {
              // if the UserName property contains an emtpy string, retrieve the profile
              // for the current user, otherwise for the specified user
              if (this.UserName.Length > 0)
              {
                  profile = this.Profile.GetProfile(this.UserName);
              }

              if ((!profile.IsAnonymous) && (profile.LegacyData.MemberSince == DateTime.MinValue || profile.LegacyData.MemberSince == System.Data.SqlTypes.SqlDateTime.MinValue))
                  profile.LegacyData.MemberSince = DateTime.Now;

              txtFirstName.Text = profile.FirstName;
              txtLastName.Text = profile.LastName;

              ddlGenders.SelectedValue = profile.Gender.ToString();
              if (profile.DateOfBirth != string.Empty)
                  txtBirthDate.Text = profile.DateOfBirth;

              txtAddress1.Text = profile.ContactInfo.Address1;
              txtAddress2.Text = profile.ContactInfo.Address2;
              txtCity.Text = profile.ContactInfo.City;
              txtPostalCode.Text = profile.ContactInfo.PostalCode;
              txtState.Text = Utils.ParseHelper.GetStateCode(profile.ContactInfo.State);
              ddlCountry.DataBind();
              txtPhone.Text = profile.ContactInfo.Phone;
          }

          DateTime since = (profile.LegacyData.MemberSince == DateTime.MinValue || profile.LegacyData.MemberSince == System.Data.SqlTypes.SqlDateTime.MinValue) ?
              DateTime.Now : profile.LegacyData.MemberSince;

          LiteralSince.Text = string.Format("{0}", since.ToString("MM/dd/yyyy"));
      }

       public void SaveProfile()
       {
           // if the UserName property contains an emtpy string, save the current user's profile,
           // othwerwise save the profile for the specified user
           if (this.UserName.Length > 0)
              profile = this.Profile.GetProfile(this.UserName);

          if ((!profile.IsAnonymous) && (profile.LegacyData.MemberSince == DateTime.MinValue || profile.LegacyData.MemberSince == System.Data.SqlTypes.SqlDateTime.MinValue))
              profile.LegacyData.MemberSince = DateTime.Now;

           System.Text.StringBuilder oldValue = new System.Text.StringBuilder();
           System.Text.StringBuilder newValue = new System.Text.StringBuilder();

           string emlFormat = "Html";
           if (profile.Preferences.EmailFormat != emlFormat)
           {
               if (profile.Preferences.EmailFormat != null)
               {
                   oldValue.AppendFormat("EmailFmt:{0}~", profile.Preferences.EmailFormat);
                   newValue.AppendFormat("EmailFmt:{0}~", emlFormat);
               }
               profile.Preferences.EmailFormat = emlFormat;
           }

           string first = txtFirstName.Text.Trim();
           if (profile.FirstName != first)
           {
               if(profile.FirstName != null)
               {
                   oldValue.AppendFormat("First:{0}~", profile.FirstName);
                   newValue.AppendFormat("First:{0}~", first);
               }
               profile.FirstName = first;
           }
           string last = txtLastName.Text.Trim();
           if (profile.LastName != last)
           {
               if(profile.LastName != null)
               {
                   oldValue.AppendFormat("Last:{0}~", profile.LastName);
                   newValue.AppendFormat("Last:{0}~", last);
               }
               profile.LastName = last;
           }

           string gender = ddlGenders.SelectedValue;
           if (profile.Gender != gender)
           {
               if(profile.Gender != null)
               {
                   oldValue.AppendFormat("Gender:{0}~", profile.Gender);
                   newValue.AppendFormat("Gender:{0}~", gender);
               }
               profile.Gender = gender;
           }


           if (txtBirthDate.Text.Trim().Length > 0 && Utils.Validation.IsDate(txtBirthDate.Text.Trim()) &&
               profile.DateOfBirth != DateTime.Parse(txtBirthDate.Text.Trim()).ToShortDateString())
           {
               if(profile.DateOfBirth != null)
               {
                   oldValue.AppendFormat("DOB:{0}~", profile.DateOfBirth);
                   newValue.AppendFormat("DOB:{0}~", txtBirthDate.Text.Trim());
               }
               profile.DateOfBirth = DateTime.Parse(txtBirthDate.Text.Trim()).ToShortDateString();
           }

           string add1 = txtAddress1.Text.Trim();
           if (profile.ContactInfo.Address1 != add1)
           {
               if(profile.ContactInfo.Address1 != null)
               {
                   oldValue.AppendFormat("Add1:{0}~", profile.ContactInfo.Address1);
                   newValue.AppendFormat("Add1:{0}~", add1);
               }
               profile.ContactInfo.Address1 = add1;
           }

           string add2 = txtAddress2.Text.Trim();
           if (profile.ContactInfo.Address2 != add2)
           {
               if(profile.ContactInfo.Address2 != null)
               {
                   oldValue.AppendFormat("Add2:{0}~", profile.ContactInfo.Address2);
                   newValue.AppendFormat("Add2:{0}~", add2);
               }
               profile.ContactInfo.Address2 = add2;
           }

           string city = txtCity.Text.Trim();
           if (profile.ContactInfo.City != city)
           {
               if(profile.ContactInfo.City != null)
               {
                   oldValue.AppendFormat("City:{0}~", profile.ContactInfo.City);
                   newValue.AppendFormat("City:{0}~", city);
               }
               profile.ContactInfo.City = city;
           }

           string zip = txtPostalCode.Text.Trim();
           if (profile.ContactInfo.PostalCode != zip)
           {
               if(profile.ContactInfo.PostalCode != null)
               {
                   oldValue.AppendFormat("Zip:{0}~", profile.ContactInfo.PostalCode);
                   newValue.AppendFormat("Zip:{0}~", zip);
               }
               profile.ContactInfo.PostalCode = zip;
           }
           string state = txtState.Text.Trim();
           if (profile.ContactInfo.State != state)
           {
               if(profile.ContactInfo.State != null)
               {
                   oldValue.AppendFormat("State:{0}~", profile.ContactInfo.State);
                   newValue.AppendFormat("State:{0}~", state);
               }
               profile.ContactInfo.State = state;
           }
           string country = ddlCountry.SelectedValue;
           if (profile.ContactInfo.Country != country)
           {
               if(profile.ContactInfo.Country != null)
               {
                   oldValue.AppendFormat("CO:{0}~", profile.ContactInfo.Country);
                   newValue.AppendFormat("CO:{0}~", country);
               }
               profile.ContactInfo.Country = country;
           }
           string phone = txtPhone.Text.Trim();
           if (profile.ContactInfo.Phone != phone)
           {
               if(profile.ContactInfo.Phone != null)
               {
                   oldValue.AppendFormat("Phone:{0}~", profile.ContactInfo.Phone);
                   newValue.AppendFormat("Phone:{0}~", phone);
               }
               profile.ContactInfo.Phone = phone;
           }

           profile.Save();

           //if (this.Page.ToString().ToLower() == "asp.register_aspx" || this.Page.ToString().ToLower() == "asp.editprofile_aspx")
           if(DisplayUserSubscriptions)
           {
               this.UserSubscriptions1.UserName = profile.UserName;
               this.UserSubscriptions1.SaveSubscriptions();
           }

           if(oldValue.Length > 0)
           {
               string creatorName = (this.UserName.Trim().Length == 0) ? profile.UserName : this.Profile.UserName;

               UserEvent.NewUserEvent(profile.UserName, DateTime.Now, DateTime.Now, _Enums.EventQStatus.Success,
                   creatorName, _Enums.EventQContext.User, _Enums.EventQVerb.UserUpdate, null, "OldValues:", oldValue.ToString(), true);
               UserEvent.NewUserEvent(profile.UserName, DateTime.Now, DateTime.Now, _Enums.EventQStatus.Success,
                   creatorName, _Enums.EventQContext.User, _Enums.EventQVerb.UserUpdate, null, "NewValues:", newValue.ToString(), true);
           }
       }

       protected void ddlCountry_DataBinding(object sender, EventArgs e)
       {
           DropDownList ddl = (DropDownList)sender;

           if (ddl.Items.Count == 0)
           {
               ddl.DataSource = Utils.ListFiller.CountryListing;
               ddl.DataTextField = "Text";
               ddl.DataValueField = "Value";
           }
       }
       protected void ddlCountry_DataBound(object sender, EventArgs e)
       {
           DropDownList ddl = (DropDownList)sender;

           ddl.SelectedIndex = -1;
           string country = (profile.ContactInfo.Country != null && profile.ContactInfo.Country.Trim().Length > 0) ?
               Utils.ParseHelper.GetCountryCode(profile.ContactInfo.Country) : Wcss._Config._Default_CountryCode;
           ListItem li = ddl.Items.FindByValue(country);

           if (li != null)
               li.Selected = true;
           else
               ddl.SelectedIndex = 0;
       }
   }
}