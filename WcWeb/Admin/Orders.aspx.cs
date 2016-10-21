using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace WillCallWeb.Admin
{
    public partial class Orders : WillCallWeb.BasePage
    {
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            //set opacity for nav events
            if (this.HasControls() && this.UpdatePanel1.Visible)
                Atx.RegisterJQueryScript_BlockUI_AjaxMethod(this.UpdatePanel1, "#orders", true);
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
            string controlToLoad = "RecentOrders";
            string req = Request.QueryString["p"];

            if (req != null && req.Trim().Length > 0)
                controlToLoad = req;

            switch (controlToLoad.ToLower())
            {
                case "recentorders":
                    controlToLoad = "Orders_Recent";
                    break;
                case "view":
                    controlToLoad = "Orders_View";
                    break;
                case "refund":
                    controlToLoad = "Orders_Refund";
                    break;
                case "tktrefund":
                    controlToLoad = "Orders_RefundTickets";
                    break;
                case "shipping":
                    controlToLoad = "Orders_Shipping";
                    break;
                case "shiplist":
                    controlToLoad = "Orders_ShipmentListing";
                    break;
                case "shippending":
                    controlToLoad = "Orders_ShipsPending";
                    break;
                case "exch":
                    controlToLoad = "Orders_Exchange";
                    break;
            }

            Content.Controls.Add(LoadControl(string.Format(@"AdminControls\{0}.ascx", controlToLoad)));
        }
    }
}