using System;
using System.Web.UI.WebControls;

namespace WillCallWeb
{
   public partial class AccessDenied : BasePage
   {
       protected override void OnPreInit(EventArgs e)
       {
           QualifySsl(false);
           base.OnPreInit(e);
       }
      protected void Page_Load(object sender, EventArgs e)
      {
         lblInsufficientPermissions.Visible = this.User.Identity.IsAuthenticated;

         lblLoginRequired.Visible = (!this.User.Identity.IsAuthenticated &&
            string.IsNullOrEmpty(this.Request.QueryString["loginfailure"]));
         
          lblInvalidCredentials.Visible = (this.Request.QueryString["loginfailure"] != null &&
            this.Request.QueryString["loginfailure"] == "1");
      }
   }
}