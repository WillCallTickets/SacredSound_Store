using System;
using System.Web.UI.WebControls;
using System.Net.Mail;

using Wcss;

namespace WillCallWeb.Controls
{
    public partial class PasswordRecovery : BaseControl
    {
        protected void Page_Load(object sender, EventArgs e) { }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            this.PasswordRecovery1.MailDefinition.BodyFileName = 
                string.Format("/{0}/MailTemplates/SiteTemplates/PasswordRecoveryEmail.txt", _Config._VirtualResourceDir);
        }

        protected void PasswordRecovery1_VerifyUser(object sender, LoginCancelEventArgs e)
        {
            string userName = PasswordRecovery1.UserName.Trim();

            //check oldUser first - if no old user then check for emailUser
            //if we pass thru that - we are existing and will move onto hint question verification
            AspnetUsersOld oldUser = AspnetUsersOld.GetOldUser(userName);

            if (oldUser != null && (!oldUser.ProfileHasBeenUpdated))
            {
                e.Cancel = true;
                //if we have an old account - don't bother with the question and answer
                //send an email and show the message
                MailQueue.SendForgotPass(oldUser.UserName, oldUser.Password);
                UserEvent.NewUserEvent(oldUser.UserName, DateTime.Now, DateTime.Now, _Enums.EventQStatus.Success, oldUser.UserName,
                     _Enums.EventQContext.User, _Enums.EventQVerb.RequestPassword, string.Empty, string.Empty, string.Empty, true);

                base.Redirect("~/PasswordRecoverySuccess.aspx");
            }

            //if there is a user but no membership...
            object result = SPs.TxUserHasMembership(_Config.APPLICATION_ID, userName).ExecuteScalar();

            //NOTE NOTE NOTE
            //the text "creating a new account" flags the register control to turn off the existing user login
            if (result != null && result.ToString().ToLower() == "false")
            {
                Ctx.CurrentPageException = "<div class=\"pagemessage\">Please update your profile by creating a new account.</div>";
                base.Redirect("~/Register.aspx");
            }
            else if (result != null && result.ToString().ToLower() == "nouser")
            {
                //indicate to user that their info was not found
            }
        }

        protected void PasswordRecovery1_SendingMail(object sender, MailMessageEventArgs e)
        {
            try
            {
                e.Message.Body = e.Message.Body.Replace("<% userWebInfo %>", userWebInfo);

                e.Message.From = new MailAddress(_Config._CustomerService_Email, _Config._CustomerService_FromName);

                //if (_Config._CC_DeveloperEmail.Length > 0)
                //    e.Message.CC.Add(_Config._CC_DeveloperEmail);

                e.Message.Subject = "Your Requested Information";

                string toAddress = e.Message.To[0].Address;
                UserEvent.NewUserEvent(toAddress, DateTime.Now, DateTime.Now, _Enums.EventQStatus.Success, toAddress,
                    _Enums.EventQContext.User, _Enums.EventQVerb.RequestPassword, string.Empty, string.Empty, string.Empty, true);

                e.Cancel = false;

                PasswordRecovery1.SuccessText = string.Format("Your password has been sent to {0}", toAddress);

            }
            catch (Exception ex)
            {
                _Error.LogException(ex);
                e.Cancel = true;
            }
        }

        protected void PasswordRecovery1_SendMailError(object sender, SendMailErrorEventArgs e)
        {
            _Error.LogException(e.Exception);
        }

        protected void PasswordRecovery1_AnswerLookupError(object sender, EventArgs e)
        {
            try
            {
                string user = PasswordRecovery1.UserName;
                string answer = (PasswordRecovery1.Answer != null) ? PasswordRecovery1.Answer : "unknown";

                UserEvent.NewUserEvent(user, DateTime.Now, DateTime.Now, _Enums.EventQStatus.Failed, user, _Enums.EventQContext.User, _Enums.EventQVerb.IncorrectPasswordHintSubmitted,
                    answer, string.Empty, null, true);
            }
            catch (Exception ex)
            {
                _Error.LogException(ex);
            }
        }
    }
}
