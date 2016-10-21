using System;
using System.Web.Security;
using System.Web.UI.WebControls;
using System.Net.Mail;

using Wcss;

namespace WillCallWeb.Controls
{
    public partial class Cust_AccountUpdate : WillCallWeb.BaseControl
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            this.ChangePassword1.MailDefinition.BodyFileName = string.Format("/{0}/MailTemplates/SiteTemplates/ChangePasswordEmail.txt", _Config._VirtualResourceDir);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Ctx.OldUser == null)
                base.Redirect("~/Register.aspx");

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

            //verify the old user
            TextBox currentPass = (TextBox)change.ChangePasswordTemplateContainer.FindControl("CurrentPassword");
            Label failure = (Label)change.ChangePasswordTemplateContainer.FindControl("FailureText");

            if (currentPass.Text != Ctx.OldUser.Password)
            {
                RequiredFieldValidator req = (RequiredFieldValidator)change.Controls[0].FindControl("valRequireCurrentPassword");
                if (req != null)
                {
                    req.IsValid = false;
                    req.ErrorMessage = "Your old password does not match. Please try again.";
                    failure.Text = req.ErrorMessage;
                }

                e.Cancel = true;
            }
            else
            {
                DropDownList ddl = (DropDownList)change.ChangePasswordTemplateContainer.FindControl("Question");
                TextBox answer = (TextBox)change.ChangePasswordTemplateContainer.FindControl("Answer");
                TextBox newPass = (TextBox)change.ChangePasswordTemplateContainer.FindControl("NewPassword");
                TextBox confirmedPass = (TextBox)change.ChangePasswordTemplateContainer.FindControl("ConfirmNewPassword");

                if (ddl.SelectedIndex == -1)
                    failure.Text += "<li>Please select a security question.</li>";
                if (answer.Text.Trim().Length == 0)
                    failure.Text += "<li>Please provide an answer for your question.</li>";

                //ensure new pass is valid pass and confirmed
                if (newPass.Text != confirmedPass.Text)
                    failure.Text += "<li>Your new password and your confirm password do not match. Please try again.</li>";
                else if (!Utils.Validation.VerifyPassword(confirmedPass.Text))
                    failure.Text += "<li>Your new password is invalid. Please try again.</li>";

                if (failure.Text.Trim().Length > 0)
                {
                    failure.Text = string.Format("<ul>{0}</ul>", failure.Text);
                    return;
                }

                try
                {
                    //create a new user w/mem and profile
                    MembershipCreateStatus status = new MembershipCreateStatus();
                    MembershipUser usr = Membership.CreateUser(Ctx.OldUser.UserName, confirmedPass.Text, Ctx.OldUser.UserName, 
                        ddl.SelectedValue, answer.Text.Trim(), true, out status);

                    if (usr != null)
                    {
                        if (!Roles.IsUserInRole(usr.UserName, "WebUser"))
                            Roles.AddUserToRole(usr.UserName, "WebUser");

                        bool valid = Membership.ValidateUser(usr.UserName, confirmedPass.Text);

                        if (valid)
                        {
                            //update old user - set updated date and ip address
                            SubSonic.QueryCommand cmd = new SubSonic.QueryCommand("UPDATE Aspnet_Users_Old SET dtUpdated = @nowDate, IpAddress = @ip WHERE Id = @idx ",
                                SubSonic.DataService.Provider.Name);
                            cmd.Parameters.Add("@nowDate", DateTime.Now.ToString("MM/dd/yyyy hh:mmtt"), System.Data.DbType.DateTime);
                            cmd.Parameters.Add("@ip", Request.UserHostAddress);
                            cmd.Parameters.Add("@idx", Ctx.OldUser.Id, System.Data.DbType.Int32);
                            SubSonic.DataService.ExecuteQuery(cmd);

                            //log the event
                            UserEvent.NewUserEvent(usr.UserName, DateTime.Now, DateTime.Now, _Enums.EventQStatus.Success, usr.UserName, _Enums.EventQContext.User, _Enums.EventQVerb.AccountUpdate,
                                null, null, "Account updated", true);

                            //clear out current vals
                            Ctx.OldUser = null;
                            string redir = Ctx.RedirectOnAuth;
                            Ctx.RedirectOnAuth = null;

                            if (redir == null)
                                redir = "~/Store/ChooseTicket.aspx";

                            //then redirect to the redirect page

                            FormsAuthentication.SetAuthCookie(usr.UserName, false);
                            //NO NO - doesn't work 
                            //FormsAuthentication.RedirectFromLoginPage(usr.UserName, false);
                            base.Redirect(redir);
                        }
                    }

                    //  do not cancel the change pass flow
                    //  now we have an updated user with a membership
                }
                catch (System.Threading.ThreadAbortException) { }
                catch (Exception ex)
                {
                    _Error.LogException(ex);

                    if (failure != null)
                        failure.Text = ex.Message;

                    e.Cancel = true;
                }
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

            //MembershipUser u = Membership.GetUser(change.UserName);
            //if (u != null)
            //{
            //    DropDownList ddl = (DropDownList)change.ChangePasswordTemplateContainer.FindControl("Question");
            //    TextBox answer = (TextBox)change.ChangePasswordTemplateContainer.FindControl("Answer");
            //    if (ddl != null && ddl.SelectedIndex > -1 && answer != null && answer.Text.Trim().Length > 0)
            //    {
            //        string question = ddl.SelectedValue;

            //        u.ChangePasswordQuestionAndAnswer(change.NewPassword, question, answer.Text.Trim());

            //        //login the user
            //        TextBox pass = (TextBox)change.ChangePasswordTemplateContainer.FindControl("ConfirmNewPassword");

            //        //then redirect to the redirect page
            //        base.Redirect(FormsAuthentication.GetRedirectUrl(u.UserName, false));
            //    }
            //}            
        }
}
}
