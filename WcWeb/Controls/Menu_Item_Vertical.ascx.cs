using System;
using System.Web;
using System.Web.UI.WebControls;

namespace WillCallWeb.Controls
{
    public partial class Menu_Item_Vertical : WillCallWeb.BaseControl
    {
        protected void Page_Load(object sender, EventArgs e) 
        {
            string controlToLoad = @"\Components\Navigation\LeftMenu";

            panelContent.Controls.Add(LoadControl(string.Format(@"..\{0}.ascx", controlToLoad)));
        }
    }
}