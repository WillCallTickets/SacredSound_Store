using System;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using Wcss;
using Utils;

namespace WillCallWeb.Admin
{
    public partial class MailerViewer : WillCallWeb.BasePage
    {
        protected override void OnPreInit(EventArgs e)
        {
            QualifySsl(false);
            base.OnPreInit(e);
        }
        protected int _emailId = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            string req = this.Request.QueryString["mlr"];

            if (req != null && Utils.Validation.IsInteger(req))
                _emailId = int.Parse(req);
            else if (Atx.CurrentSubscriptionEmailId > 0)
                _emailId = Atx.CurrentSubscriptionEmailId;


            if(_emailId > 0)
            {
                pnlMain.Controls.Clear();

                SubscriptionEmail sub = SubscriptionEmail.FetchByID(_emailId);

                if (sub != null && sub.SubscriptionRecord.ApplicationId == _Config.APPLICATION_ID)
                {
                    //manipulate headers
                    this.Page.Title = sub.EmailLetterRecord.Name;


                    //***********************
                    //CANT DO THIS HERE
                    //Must be done in the preinit
                    //
                    //clear any existing styles
                    //this.StyleSheetTheme = string.Empty;
                    //this.Theme = string.Empty;

                    //insert any style info
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.AppendFormat("<style type=\"text/css\"><!--{0}", Constants.NewLine);

                    sb.AppendFormat("{0}{1}", SubscriptionEmail.Css_OptOut, Constants.NewLine);

                    string style = sub.EmailLetterRecord.StyleContent;
                    if (style != null && style.Trim().Length > 0)
                        sb.Append(style);

                    sb.AppendFormat("{0}--></style>{0}", Constants.NewLine);

                    Literal lit = new Literal();
                    lit.Text = sb.ToString();

                    this.Page.Header.Controls.Add(lit);

                    //add content
                    Literal content = new Literal();

                    content.Text = sub.CreateShell_Html(false, true, false, true);

                    pnlMain.Controls.Add(content);
                }
            }
        }
    }
}