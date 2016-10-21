using System;
using System.Web.Security;
using System.Web.UI.WebControls;

using System.Net.Mail;

using Wcss;

namespace WillCallWeb
{
    public partial class Register : BasePage
    {
        protected override void OnPreInit(EventArgs e)
        {
            QualifySsl(true);
            base.OnPreInit(e);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            string controlToLoad = @"\Register";

            panelContent.Controls.Add(LoadControl(string.Format(@"\Controls{0}.ascx", controlToLoad)));       
        }
}
}
