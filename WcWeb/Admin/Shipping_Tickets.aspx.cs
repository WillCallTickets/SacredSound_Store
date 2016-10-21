using System;

namespace WillCallWeb.Admin
{
    public partial class Shipping_Tickets : WillCallWeb.BasePage
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
            string controlToLoad = "batchview";
            string req = Request.QueryString["p"];

            if (req != null && req.Trim().Length > 0)
                controlToLoad = req;

            switch (controlToLoad.ToLower())
            {
                case "fill":
                    controlToLoad = "Shipping_FulfillmentCreate";
                    break;
                case "batchview":
                    controlToLoad = "Shipping_ShipmentBatchView";
                    break;
                //case "eventview":
                //    controlToLoad = "Shipping_ShipmentEventView";
                //    break;
            }

            Content.Controls.Add(LoadControl(string.Format(@"AdminControls\BulkShipping\{0}.ascx", controlToLoad)));
        }
    }
}
