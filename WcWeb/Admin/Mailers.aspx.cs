using System;

namespace WillCallWeb.Admin
{
    public partial class Mailers : WillCallWeb.BasePage
    {
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            //set opacity for nav events
            string control = string.Empty;
            string req = Request.QueryString["p"];
            if (req != null && req.Trim().Length > 0)
                control = req;

            //if (this.HasControls() && this.UpdatePanel1.Visible)
            if (control.ToLower() != "send" && this.HasControls() && this.UpdatePanel1.Visible)
                Atx.RegisterJQueryScript_BlockUI_AjaxMethod(this.UpdatePanel1, "#mailers", true);
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
            string controlToLoad = "Mailer_Edit";

            string req = Request.QueryString["p"];

            if (req != null && req.Trim().Length > 0)
                controlToLoad = req;

            switch (controlToLoad.ToLower())
            {
                case "edit":
                    controlToLoad = "Mailer_Edit";
                    break;
                case "subscription":
                    controlToLoad = "Mailer_Subscription";
                    break;
                case "send":
                    controlToLoad = "Mailer_Send";
                    break;
                case "list":
                    controlToLoad = "Mailer_ListGenerator";
                    break;
                case "customer":
                    controlToLoad = "Mailer_Customer";
                    break;

                //templating
                case "select":
                    controlToLoad = @"MailerTemplating\MailerTemplate_Select";
                    break;
                case "mlredit":
                    controlToLoad = @"MailerTemplating\MailerTemplate_MailerEdit";
                    break;
                case "mlrgenerate":
                    controlToLoad = @"MailerTemplating\MailerTemplate_MailerGenerate";
                    break;
                case "mlrupload":
                    controlToLoad = @"MailerTemplating\MailerTemplate_ImageUpload";
                    break;


                //case "tplcreate":
                //    controlToLoad = @"MailerTemplating\MailerTemplate_TemplateCreate";
                //    break;
                case "tpledit":
                    controlToLoad = @"MailerTemplating\MailerTemplate_TemplateEdit";
                    break;
                case "tplparts":
                    controlToLoad = @"MailerTemplating\MailerTemplate_TemplatePart";
                    break;
                case "tplsubs":
                    controlToLoad = @"MailerTemplating\MailerTemplate_TemplateSubs";
                    break;
            }

            Content.Controls.Add(LoadControl(string.Format(@"AdminControls\{0}.ascx", controlToLoad)));
        }
    }
}