using System;

namespace WillCallWeb.Admin
{
    public partial class Reports : WillCallWeb.BasePage
    {
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            //set opacity for nav events
            //if (this.HasControls() && this.UpdatePanel1.Visible)
            //    Atx.RegisterJQueryScript_BlockUI_AjaxMethod(this.UpdatePanel1, "#report", true);
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
            string controlToLoad = "Report_SalesAll";
            string req = Request.QueryString["p"];

            if (req != null && req.Trim().Length > 0)
                controlToLoad = req;

            switch (controlToLoad.ToLower())
            {
                case "allsales":
                    controlToLoad = "Report_SalesAll";
                    break;
                case "tickets":
                    controlToLoad = "Report_TixSales";
                    break;
                case "counts":
                    controlToLoad = "Report_TixCounts";
                    break;
                //old inventory pages
                case "tix":
                    controlToLoad = "Reports_InventoryTickets";
                    break;
                case "merch":
                    string merchId = Request.QueryString["merchitem"];
                    if (merchId != null)
                        controlToLoad = "Merch_Sales";//todo find merch_sales
                    else
                        controlToLoad = "Reports_InventoryMerch";
                    break;
                //newer reports
                case "period":
                    controlToLoad = "Report_Period";
                    break;
                case "merchdetail":
                    controlToLoad = "Report_MerchDetail";
                    break;
                case "bundle":
                    controlToLoad = "Reports_InventoryBundles";
                    break;
            }

            Content.Controls.Add(LoadControl(string.Format(@"AdminControls\{0}.ascx", controlToLoad)));
        }
    }
}