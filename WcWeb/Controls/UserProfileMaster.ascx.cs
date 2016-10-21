using System;

using System.Net.Mail;

namespace WillCallWeb.Controls
{
   public partial class UserProfileMaster : BaseControl
   {
       protected override System.Web.HttpContext Context
       {
           get
           {
               return base.Context;
           }
       }
       protected override void OnInit(EventArgs e)
       {
           base.OnInit(e);
       }
       public override void PageInit()
       {
           base.PageInit();
       }
      protected void Page_Load(object sender, EventArgs e)
      {
      }
      protected void btnUpdate_Click(object sender, EventArgs e)
      {
         UserProfile1.SaveProfile();
         lblFeedbackOK.Visible = true;
      }
}
}
