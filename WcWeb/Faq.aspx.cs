using System;

namespace WillCallWeb
{
   public partial class Faq : BasePage
   {
       protected override void OnPreInit(EventArgs e)
       {
           QualifySsl(false);
           base.OnPreInit(e);
       }
      protected void Page_Load(object sender, EventArgs e)
      {
      }
   }
}
