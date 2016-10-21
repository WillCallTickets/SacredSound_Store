using System;

namespace WillCallWeb.Admin
{
    public partial class Listings : WillCallWeb.BasePage
    {
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            //set opacity for nav events
            if (this.HasControls() && this.UpdatePanel1.Visible)
            {
                Atx.RegisterJQueryScript_BlockUI_AjaxMethod(this.UpdatePanel1, "#listings", true);
            }
        }
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
            string controlToLoad = "tix";
            string req = Request.QueryString["p"];

            if (req != null && req.Trim().Length > 0)
                controlToLoad = req;

            switch (controlToLoad.ToLower())
            {
                case "tickets":
                //    controlToLoad = "Listings_Tickets";
                //    break;
                case "tix":
                    //controlToLoad = "Listing_Tix";
                    controlToLoad = @"Reports\TicketManifest";
                    break;
            }

            Content.Controls.Add(LoadControl(string.Format(@"AdminControls\{0}.ascx", controlToLoad)));
        }
    }
}