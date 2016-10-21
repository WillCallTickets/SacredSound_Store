using System;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WillCallWeb.Admin.AdminControls.MailerTemplating
{
    public partial class MailerTemplatingPreview : BasePage
    {
        protected override void OnPreInit(EventArgs e)
        {
            //base.OnPreInit(e);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Header != null && Atx.CurrentMailerTemplateCreation != null)
            {
                HtmlGenericControl styles = new HtmlGenericControl("style");
                styles.Attributes.Add("type", "text/css");

                styles.InnerText = Atx.CurrentMailerTemplateCreation.MailerObject.MailerTemplateRecord.Style;

                this.Header.Controls.Add(styles);
            }

            if (Atx.CurrentMailerTemplateCreation != null)
            {
                litHtmlVersion.Text = Atx.CurrentMailerTemplateCreation._htmlVersion.ToString()
                    .Replace(@"\r\n", Environment.NewLine).Replace(@"\t", Utils.Constants.Tab);
                
                litTextVersion.Text = string.Format("<div style=\"white-space: pre; border: solid blue 0px;\">{0}</div>", 
                    Atx.CurrentMailerTemplateCreation._textVersion.ToString().Replace(@"\r\n", Environment.NewLine).Replace(@"\t", string.Empty));
            }
        }
    }
}
