using System;
using System.Drawing.Imaging;
using System.Text;
using System.Collections.Generic;

using Wcss;
using Utils;

namespace WillCallWeb.Admin
{
	public partial class ShowMerch : WillCallWeb.BasePage
	{
        protected override void OnPreInit(EventArgs e)
        {
            QualifySsl(false);
            base.OnPreInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            panelDisplay.Controls.Add(LoadControl(string.Format(@"~\Controls\Listing_Merch.ascx")));
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            base.Redirect(this.Request.RawUrl);
        }
	}
}
