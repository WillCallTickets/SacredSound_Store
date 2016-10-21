using System;

namespace WillCallWeb.Admin
{
    public partial class CustomerEditor : WillCallWeb.BasePage
    {
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            //set opacity for nav events
            if (this.HasControls() && this.UpdatePanel1.Visible)
                Atx.RegisterJQueryScript_BlockUI_AjaxMethod(this.UpdatePanel1, "#customer", true);
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
            string controlToLoad = "CustomerPicker";
            string req = Request.QueryString["p"];

            if (req != null && req.Trim().Length > 0)
                controlToLoad = req;

            switch (controlToLoad.ToLower())
            {
                case "customerpicker":
                    controlToLoad = "CustomerPicker";
                    break;
                case "sales":
                    controlToLoad = "CustomerSales";
                    break;
            }

            Content.Controls.Add(LoadControl(string.Format(@"AdminControls\{0}.ascx", controlToLoad)));
        }
    }
}