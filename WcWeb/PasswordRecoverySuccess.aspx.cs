using System;
using System.Web.UI.WebControls;
using System.Net.Mail;

using Wcss;

namespace WillCallWeb
{
    public partial class PasswordRecoverySuccess : BasePage
   {
        protected override void OnPreInit(EventArgs e)
        {
            QualifySsl(true);
            base.OnPreInit(e);
        }
      protected void Page_Load(object sender, EventArgs e) 
      {
      }
}
}
