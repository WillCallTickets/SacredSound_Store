using System;
using System.Web.Services;

namespace WillCallWeb.Admin
{
    public partial class PromotionEditor : WillCallWeb.BasePage
    {
        [WebMethod]
        public static bool RemoveMerchChoiceFromSalePromotion(int removeId)
        {
            return AdminServices.RemoveMerchChoiceFromSalePromotion(removeId);
        }
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            //set opacity for nav events
            if (this.HasControls() && this.UpdatePanel1.Visible)
                Atx.RegisterJQueryScript_BlockUI_AjaxMethod(this.UpdatePanel1, "#editor", true);
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
            string controlToLoad = "Prom_Banner";

            string req = Request.QueryString["p"];

            if (req != null && req.Trim().Length > 0)
                controlToLoad = req;

            switch (controlToLoad.ToLower())
            {
                case "promo":
                    controlToLoad = "Prom_Picker";
                    break;
                case "order":
                    controlToLoad = "Prom_BannerOrder";
                    break;
                case "banner":
                    controlToLoad = "Prom_Banner";
                    break;
            }

            Content.Controls.Add(LoadControl(string.Format(@"AdminControls\{0}.ascx", controlToLoad)));
        }
    }
}