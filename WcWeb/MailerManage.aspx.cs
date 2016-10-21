using System;
using System.Web.UI.WebControls;
using System.Net.Mail;

using Wcss;

namespace WillCallWeb
{
   public partial class MailerManage : BasePage
   {
       protected override void OnPreInit(EventArgs e)
       {
           QualifySsl(false);
           base.OnPreInit(e);
       }
      protected void Page_Load(object sender, EventArgs e) {}
}
}
