using System;
using System.Web.Security;
using System.Web.UI.WebControls;
using System.Net.Mail;

using Wcss;

namespace WillCallWeb.Controls
{
    public partial class Customer_ChangePass : WillCallWeb.BaseControl
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            this.ChangePassword1.MailDefinition.BodyFileName = string.Format("/{0}/MailTemplates/SiteTemplates/ChangePasswordEmail.txt", _Config._VirtualResourceDir);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                // show the user's details
                DropDownList ddl = (DropDownList)ChangePassword1.ChangePasswordTemplateContainer.FindControl("Question");
                if (ddl != null && ddl.Items.Count == 1)//there is a default item
                {
                    ddl.DataSource = Wcss._Lookits.HintQuestions;
                    ddl.DataBind();
                }
            }
        }

        protected void ChangePassword1_ChangingPassword(object sender, LoginCancelEventArgs e)
        {
            ChangePassword change = (ChangePassword)sender;

            bool valid = Membership.ValidateUser(Profile.UserName, change.CurrentPassword);

            if (!valid)
            {
                RequiredFieldValidator req = (RequiredFieldValidator)change.Controls[0].FindControl("valRequireCurrentPassword");
                Label failure = (Label)change.Controls[0].FindControl("FailureText");

                if (req != null && failure != null)
                {
                    req.IsValid = false;
                    req.ErrorMessage = "Your current password does not match.";

                    failure.Text = req.ErrorMessage;
                }

                e.Cancel = true;
            }
        }
        protected void ChangePassword1_SendingMail(object sender, MailMessageEventArgs e)
        {
            try
            {
                e.Message.Body = e.Message.Body.Replace("<% userWebInfo %>", userWebInfo);

                e.Message.From = new MailAddress(_Config._CustomerService_Email, _Config._CustomerService_FromName);

                //if (_Config._CCDev.Length > 0)
                //    e.Message.CC.Add(_Config._CCDev);

                e.Message.Subject = "Your Information Has Been Changed";

                e.Cancel = false;
            }
            catch (Exception ex)
            {
                _Error.LogException(ex);
                e.Cancel = true;
            }
        }
        protected void ChangePassword1_SendMailError(object sender, SendMailErrorEventArgs e)
        {
            _Error.LogException(e.Exception);
        }
        protected void ChangePassword1_ChangedPassword(object sender, EventArgs e)
        {
            ChangePassword change = (ChangePassword)sender;

            MembershipUser u = Membership.GetUser(Profile.UserName);
            if (u != null)
            {
                DropDownList ddl = (DropDownList)change.ChangePasswordTemplateContainer.FindControl("Question");
                TextBox answer = (TextBox)change.ChangePasswordTemplateContainer.FindControl("Answer");
                if (ddl != null && ddl.SelectedIndex > -1 && answer != null && answer.Text.Trim().Length > 0)
                {
                    string question = ddl.SelectedValue;

                    u.ChangePasswordQuestionAndAnswer(change.NewPassword, question, answer.Text.Trim());
                }
            }
        }
}
}
