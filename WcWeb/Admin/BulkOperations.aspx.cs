using System;

namespace WillCallWeb.Admin
{
    public partial class BulkOperations : WillCallWeb.BasePage
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
            string controlToLoad = "salepricingmerch";
            string req = Request.QueryString["p"];

            if (req != null && req.Trim().Length > 0)
                controlToLoad = req;

            switch (controlToLoad.ToLower())
            {
                case "salepricingmerch":
                    controlToLoad = "Bulk_SalePrice_Merch";
                    break;
            }

            Content.Controls.Add(LoadControl(string.Format(@"AdminControls\{0}.ascx", controlToLoad)));
        }
    }
}