using System;
using System.Net.Mail;

using Wcss;

namespace WillCallWeb
{
   public partial class Contact : BasePage
   {
       protected override void OnPreInit(EventArgs e)
       {
           QualifySsl(false);
           base.OnPreInit(e);
       }
       protected void Page_Load(object sender, EventArgs e)
       {
           string controlToLoad = @"\Contact";
           
           panelContent.Controls.Add(LoadControl(string.Format(@"\Controls{0}.ascx", controlToLoad)));
       }
   }
}
