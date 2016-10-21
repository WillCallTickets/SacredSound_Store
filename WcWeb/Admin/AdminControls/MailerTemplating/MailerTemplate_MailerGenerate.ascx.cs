using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

using Wcss;

namespace WillCallWeb.Admin.AdminControls.MailerTemplating
{
    public partial class MailerTemplate_MailerGenerate : BaseControl
    {
        private string _textVersion = string.Empty;
        protected string TextVersion { get { return _textVersion; } set { _textVersion = value; } }
        protected MailerTemplateCreation Creator
        {
            get { return Atx.CurrentMailerTemplateCreation; }
            set { Atx.CurrentMailerTemplateCreation = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if(Atx.CurrentMailer == null)
                base.Redirect("/Admin/Mailers.aspx?p=select");

            if (!IsPostBack)
            {
                //reset creator
                if (Creator != null)
                    Creator = null;
            }
        }

        protected void btnPreview_Click(object sender, EventArgs e)
        {
            if (Creator != null)
                Creator = null;

            Creator = new MailerTemplateCreation(Atx.CurrentMailer);

            Creator.GenerateMailer();

            if (Creator.MailerObject == null)
                return;
        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            if (Creator == null)
            {
                return;
            }

            //create a new mailer
            Subscription defaultSub = _Lookits.Subscriptions.GetList().Find(delegate(Subscription match) { return (match.IsDefault); });

            if (defaultSub != null)
            {
                //auto create filename
                string autoName = SubscriptionEmail.ConstructBodyName(Creator.MailerObject.Name, true);

                int emailId = MailCreation.CreateEmailLetterAndSubscriptionEmail(defaultSub.Id, autoName, Creator.MailerObject.Subject,
                    (Creator.MailerObject.MailerTemplateRecord.Style != null) ?
                    Creator.MailerObject.MailerTemplateRecord.Style.Replace(@"\r\n", Environment.NewLine).Replace(@"\t", Utils.Constants.Tab) : string.Empty,
                    Creator._htmlVersion.ToString().Replace(@"\r\n", Environment.NewLine).Replace(@"\t", Utils.Constants.Tab),
                    Creator._textVersion.ToString().Replace(@"\r\n", Environment.NewLine).Replace(@"\t", string.Empty));

                //select as the current ATX mailer
                Atx.CurrentSubscriptionEmailId = emailId;

                //redirect to edit page
                base.Redirect("~/Admin/Mailers.aspx?p=edit");
            }
        }
    }
}