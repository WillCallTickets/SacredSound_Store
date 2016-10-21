using System;
using System.Web.Services;

using Wcss;

namespace WillCallWeb.Admin
{
    public partial class EntityEditor : WillCallWeb.BasePage
    {
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            //set opacity for nav events
            if (this.HasControls() && this.UpdatePanel1.Visible)
                Atx.RegisterJQueryScript_BlockUI_AjaxMethod(this.UpdatePanel1, "#srceditor", true);
        }
        protected override void OnPreInit(EventArgs e)
        {
            QualifySsl(false);
            base.OnPreInit(e);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            SetPageControl();
        }
        private void SetPageControl()
        {
            //SET UP PAGE BASED UPON QS
            string controlToLoad = "act";//"division";// "act";
            string req = Request.QueryString["p"];

            if (req != null && req.Trim().Length > 0)
                controlToLoad = req.ToLower();

            switch (controlToLoad)
            {
                case "promoter":
                    controlToLoad = "Editor_Promoter";
                    break;
                case "act":
                    controlToLoad = "Editor_Act";
                    break;
                case "age":
                    controlToLoad = "Editor_Age";
                    break;
                case "charity":
                    controlToLoad = "Editor_CharitableOrg";
                    break;
                case "faq":
                    controlToLoad = "Editor_Faq";
                    break;
                case "emp":
                    controlToLoad = "Editor_Employee";
                    break;
                case "gen":
                    controlToLoad = "Editor_Genre";
                    break;
                case "chrg":
                    controlToLoad = "Editor_ServiceCharge";
                    break;
                case "venue":
                    controlToLoad = "Editor_Venue";
                    break;

                case "division":
                case "categorie":
                case "mjcorder":                    
                    if(!IsPostBack)                    
                        Atx.OrdinalTabCookie = 
                            (controlToLoad == "mjcorder") ? 0 : 
                            (controlToLoad == "categorie") ? 1 : 2;//mjc default
                    controlToLoad = "Editor_MerchOrganization";
                    break;

                case "color":
                    controlToLoad = "Editor_MerchColor";
                    break;                
                case "size":
                    controlToLoad = "Editor_MerchSize";
                    break;
                case "rule":
                    controlToLoad = "Editor_SaleRule";
                    break;
                case "invoicefee":
                    if (this.Page.User.IsInRole("Administrator"))
                        controlToLoad = "Editor_InvoiceFee";
                    else
                        base.Redirect("/AccessDenied.aspx");
                    break;
            }
                        
            Content.Controls.Add(LoadControl(string.Format(@"AdminControls\{0}.ascx", controlToLoad)));
        }
    }
}
