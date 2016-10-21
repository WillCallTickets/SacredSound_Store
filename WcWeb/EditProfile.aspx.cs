using System;

using System.Net.Mail;

namespace WillCallWeb
{
   public partial class EditProfile : BasePage
   {
       protected override void OnPreInit(EventArgs e)
       {
           QualifySsl(true);
           base.OnPreInit(e);
       }
      protected void Page_Load(object sender, EventArgs e)
      {
          string controlToLoad = @"\UserProfileMaster";

          panelContent.Controls.Add(LoadControl(string.Format(@"\Controls{0}.ascx", controlToLoad)));
      }
}
}
