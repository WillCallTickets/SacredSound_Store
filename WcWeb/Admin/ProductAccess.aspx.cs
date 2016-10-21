using System;

namespace WillCallWeb.Admin
{
    public partial class ProductAccess : WillCallWeb.BasePage
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
            string controlToLoad = "CampaignList";
            string req = Request.QueryString["p"];

            if (req != null && req.Trim().Length > 0)
                controlToLoad = req;

            switch (controlToLoad.ToLower())
            {
                case "campaign":
                    controlToLoad = "CampaignList";
                    break;
                case "usr":
                    controlToLoad = "CampaignUser";
                    break;
                case "mlr":
                    controlToLoad = "CampaignMailer";
                    break;
            }

            Content.Controls.Add(LoadControl(string.Format(@"AdminControls\ProductAccessor\{0}.ascx", controlToLoad)));
        }
    }
}