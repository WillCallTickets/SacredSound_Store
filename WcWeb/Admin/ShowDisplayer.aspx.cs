using System;
using System.Web.UI.WebControls;
using System.Collections.Generic;

namespace WillCallWeb.Admin
{
    public partial class ShowDisplayer : BasePage
    {
        protected override void OnPreInit(EventArgs e)
        {
            QualifySsl(false);
            base.OnPreInit(e);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            panelDisplay.Controls.Add(LoadControl(string.Format(@"~\Controls\Listing_Ticket.ascx")));
        }
        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            base.Redirect(this.Request.RawUrl);
        }
    }
}