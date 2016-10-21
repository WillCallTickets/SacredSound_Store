using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace WillCallWeb.WebUser
{
   public partial class _Default : BasePage
   {
       protected override void OnPreInit(EventArgs e)
       {
           QualifySsl(true);
           base.OnPreInit(e);
       }
       protected void Page_Load(object sender, EventArgs e)
       {
           SetPageControl();
       }

       private void SetPageControl()
       {
           //SET UP PAGE BASED UPON QS
           //string controlToLoad = "history";
           //string req = Request.QueryString["p"];

           string controlToLoad = "/Customer_Navigation";

           PanelContent.Controls.Add(LoadControl(string.Format(@"/Controls{0}.ascx", controlToLoad)));
       }
   }
}