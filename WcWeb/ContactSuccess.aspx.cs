using System;

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

       }
   }
}
