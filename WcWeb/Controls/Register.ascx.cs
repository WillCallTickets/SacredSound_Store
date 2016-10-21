using System;
using System.Web.Security;
using System.Web.UI.WebControls;

using System.Net.Mail;

using Wcss;

namespace WillCallWeb.Controls
{
    public partial class Register : BaseControl
    {
        protected string Email = "";

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            this.CreateUserWizard1.MailDefinition.BodyFileName = string.Format("/{0}/MailTemplates/SiteTemplates/RegisterEmail.txt", _Config._VirtualResourceDir);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Wcss._Config._AutoLogin)
            {
                bool valid = Membership.ValidateUser(_Config._AutoLoginName, _Config._AutoLoginPass);

                if (valid)
                {
                    Ctx.Cart.ValidateStoreCredit(_Config._AutoLoginName, this.Profile.UserName);
                    FormsAuthentication.RedirectFromLoginPage(_Config._AutoLoginName, false);
                }
            }

            string except = Ctx.CurrentPageException;
            Ctx.CurrentPageException = null;
            if (except != null && except.Trim().Length > 0)
            {
                updateProfile.Text = except;
                if (except.ToLower().IndexOf("creating a new account") != -1)
                    existing.Visible = false;
            }

            if (!this.IsPostBack)
            {
                DropDownList ddl = (DropDownList)CreateUserWizard1.CreateUserStep.ContentTemplateContainer.FindControl("Question");
                if (ddl != null && ddl.Items.Count == 1)//there is a default item
                {
                    ddl.DataSource = Wcss._Lookits.HintQuestions;
                    ddl.DataBind();
                }

                if (!string.IsNullOrEmpty(this.Request.QueryString["Email"]))
                {
                    Email = this.Request.QueryString["Email"];
                    CreateUserWizard1.DataBind();
                }
            }

            //TODO SET FOCUS
            string focii = Request.QueryString["p"];
            ////3 situations - 1) create account 2)setup name & address 3)logging in
            if((focii != null && focii.ToLower() == "create"))
            {
                TextBox userText = (TextBox)CreateUserWizard1.CreateUserStep.ContentTemplateContainer.FindControl("UserName");
                if(userText != null)
                    this.Page.SetFocus(userText.ClientID);
            }
            else
            {
                TextBox userText = (TextBox)Login1.FindControl("UserName");
                if (userText != null)
                    this.Page.SetFocus(userText.ClientID);
                else if(CreateUserWizard1.ActiveStep.ID == "profile")
                {
                    //get the profile control contained within - then get the firstname textbox
                    WillCallWeb.Controls.UserProfile userProfile = (WillCallWeb.Controls.UserProfile)CreateUserWizard1.ActiveStep.FindControl("UserProfile1");
                    if (userProfile != null)
                    {
                        TextBox firstName = (TextBox)userProfile.FindControl("txtFirstName");
                        if(firstName != null)
                            this.Page.SetFocus(firstName.ClientID);
                    }
                }
            }       
        }

        protected void linkLogout_Click(object sender, EventArgs e)
        {
            Ctx.LogoutUser();
        }

        protected void Login1_PreRender(object sender, EventArgs e)
        {
            HyperLink manage = (HyperLink)this.Login1.FindControl("linkManageEmail");
            if (manage != null)
                manage.Visible = _Config._SubscriptionsActive;

            base.OnPreRender(e);
        }  

        protected void CreateUserWizard1_FinishButtonClick(object sender, WizardNavigationEventArgs e)
        {
            UserProfile1.SaveProfile();

            string redir = FormsAuthentication.GetRedirectUrl(this.Page.User.Identity.Name, false);

            //do not allow a new user to have access to admin
            if (redir != null && redir.Trim().Length > 0)
            {
                if (!redir.StartsWith("/Admin"))
                {
                    if(redir.ToLower().IndexOf("confirmation.aspx") != -1)
                        base.Redirect("/EditProfile.aspx");
                    else
                        base.Redirect(redir);
                }
                else
                    base.Redirect("/Store/ChooseTicket.aspx");
            }
        }

        protected void SetUserInfo(string userName)
        {
            if (!Roles.IsUserInRole(userName, "WebUser"))
                Roles.AddUserToRole(userName, "WebUser");

            //add subscriptions to new user - create regardless of preference (optin)
            SubscriptionCollection coll = new SubscriptionCollection();
            coll.AddRange(_Lookits.Subscriptions.GetList().FindAll(delegate(Subscription match) { return (match.IsActive && match.AspnetRoleRecord.RoleName == "WebUser"); }));

            foreach (Subscription sub in coll)
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                //dont use exists
                sb.Append("INSERT SubscriptionUser(UserId, TSubscriptionId, bSubscribed, bHtmlFormat, dtLastActionDate) ");
                sb.Append("SELECT u.[UserId], @idx, 0, 1, ((getDate())) ");
                sb.Append("FROM [AspNet_Users] u ");
                //be sure to only create where necessary
                sb.Append("WHERE u.[ApplicationId] = @appId AND u.[LoweredUserName] = @username AND u.[UserId] NOT IN (SELECT su.[UserId] FROM [SubscriptionUser] su WHERE su.[TSubscriptionId] = @idx); ");

                SubSonic.QueryCommand cmd = new SubSonic.QueryCommand(sb.ToString(), SubSonic.DataService.Provider.Name);
                cmd.Parameters.Add("@idx", sub.Id, System.Data.DbType.Int32);
                cmd.Parameters.Add("@username", userName);
                cmd.Parameters.Add("@appId", _Config.APPLICATION_ID, System.Data.DbType.Guid);

                try
                {
                    SubSonic.DataService.ExecuteQuery(cmd);
                }
                catch (Exception ex)
                {
                    _Error.LogException(ex);
                }
            }

            UserEvent.NewUserEvent(userName, DateTime.Now, DateTime.Now, _Enums.EventQStatus.Success, userName,
                _Enums.EventQContext.User, _Enums.EventQVerb.UserCreated, string.Empty, string.Empty, this.Page.ToString(), true);

            //set username and rebind profile
            UserProfile1.BindPage(userName);
        }

        protected void CreateUserWizard1_CreatedUser(object sender, EventArgs e)
        {
            // add the current user to the WebUsers role
            string userName = CreateUserWizard1.UserName;

            SetUserInfo(userName);
        }

        protected void CreateUserWizard1_SendMailError(object sender, SendMailErrorEventArgs e)
        {
            _Error.LogException(e.Exception);
            e.Handled = true;
        }

        protected void CreateUserWizard1_SendingMail(object sender, MailMessageEventArgs e)
        {
            string userName = CreateUserWizard1.UserName; 

            try
           {
               e.Message.Body = e.Message.Body.Replace("<% userWebInfo %>", userWebInfo);

               e.Message.From = new MailAddress(_Config._CustomerService_Email, _Config._CustomerService_FromName);

               e.Message.Subject = "Thank you for registering";

               e.Cancel = false;

               UserEvent.NewUserEvent(userName, DateTime.Now, DateTime.Now, _Enums.EventQStatus.Success, userName,
                _Enums.EventQContext.User, _Enums.EventQVerb.UserSentRegistrationConfirm, string.Empty, string.Empty, string.Empty, true);
              
           }
           catch (Exception ex)
           {
               _Error.LogException(ex);
               e.Cancel = true;

               UserEvent.NewUserEvent(userName, DateTime.Now, DateTime.Now, _Enums.EventQStatus.Failed, userName,
                _Enums.EventQContext.User, _Enums.EventQVerb.UserSentRegistrationConfirm, string.Empty, string.Empty, string.Empty, true);
           }
        }

        protected void Login1_LoggedIn(object sender, EventArgs e)
        {
            Login login = (Login)sender;

            //do we have a profile and is it not anonymous?
            Ctx.Cart.ValidateStoreCredit(login.UserName, this.Profile.UserName);
            
            //Specify a default
            string redirect = FormsAuthentication.GetRedirectUrl(login.UserName, false);

            EventQ.LogEvent(DateTime.Now, DateTime.Now, _Enums.EventQStatus.Success, login.UserName, Guid.Empty, login.UserName,
                _Enums.EventQContext.User, _Enums.EventQVerb.UserLogin, redirect, "Login Redirect Page", null);

            if (redirect == null || redirect.Trim().Length == 0 || (redirect.ToLower().IndexOf("confirmation.aspx") != -1) || (redirect.ToLower().IndexOf("shipping.aspx") != -1))
                base.Redirect("/EditProfile.aspx");
        }

        protected void Login1_LoginError(object sender, EventArgs e)
        {
            Login login = (Login)sender;

            base.OldUserMustUpdate(login.UserName.Trim());//this will redirect if the user needs to update

            //if there is a user but no membership...tell the user the dealio
            object result = SPs.TxUserHasMembership(_Config.APPLICATION_ID, login.UserName).ExecuteScalar();

            //NOTE NOTE NOTE
            //the text "creating a new account" flags the register control to turn off the existing user login
            if (result != null && result.ToString().ToLower() == "false")
            {
                updateProfile.Text = "<div class=\"pagemessage\">Please update your profile by creating a new account.</div>";
                existing.Visible = false;
            }
            else if (result != null && result.ToString().ToLower() == "true")
            {
                string ps = string.Empty;
                int passLen = login.Password.Length;

                for (int i=0; i<passLen; i++)
                    ps += "*";

                if (ps.Length == 0)
                    ps = "***";

                EventQ.LogEvent(DateTime.Now, DateTime.Now, _Enums.EventQStatus.Failed, login.UserName, Guid.Empty, login.UserName,
                    _Enums.EventQContext.User, _Enums.EventQVerb.UserLogin, ps, "Invalid password", null);
            }
        }

        protected void Login1_LoggingIn(object sender, LoginCancelEventArgs e)
        {
            Login log = (Login)sender;
            log.UserName = log.UserName.Trim();

            //if we are logged in - ask to log out first
            if (!this.Profile.IsAnonymous)
            {
                Literal fail = (Literal)log.FindControl("FailureText");
                if (fail != null)
                    fail.Text = "Please logout before you attempt to login again.";

                e.Cancel = true;
            }

            if (base.OldUserMustUpdate(log.UserName.Trim()))
                e.Cancel = true;    
        }

        protected void CreateUserWizard1_CreatingUser(object sender, LoginCancelEventArgs e)
        {
            CreateUserWizard wiz = (CreateUserWizard)sender;            
            CreateUserWizardStep step = wiz.CreateUserStep;
            Label failure = ErrorMessage;            
            TextBox username = (TextBox)step.ContentTemplateContainer.FindControl("UserName");

            //do not allow usernames with www. or http...no urls
            if (username != null)
            {
                string user = username.Text.Trim();

                if (user.ToLower().StartsWith("http:") || user.ToLower().StartsWith("www."))
                    failure.Text += "<li>Please enter a valid email address.</li>";

                if (failure.Text.Trim().Length > 0)
                {
                    RegularExpressionValidator val = (RegularExpressionValidator)step.ContentTemplateContainer.FindControl("valEmailPattern");
                    if (val != null)
                    {
                        val.ErrorMessage = failure.Text;
                        val.IsValid = false;
                    } 
                }
            }


            DropDownList ddl = (DropDownList)step.ContentTemplateContainer.FindControl("Question");
            TextBox answer = (TextBox)step.ContentTemplateContainer.FindControl("Answer");
            TextBox confirmedPass = (TextBox)step.ContentTemplateContainer.FindControl("ConfirmPassword");
            
            if (ddl.SelectedIndex <= 0)//allow for security question title
            {
                CompareValidator req = (CompareValidator)step.ContentTemplateContainer.FindControl("reqReqQuestion");
                req.IsValid = false;
                failure.Text += "<li>Please select a security question.</li>";
            }
            if (answer.Text.Trim().Length == 0)
            {
                RequiredFieldValidator req = (RequiredFieldValidator)step.ContentTemplateContainer.FindControl("valRequireAnswer");
                req.IsValid = false;
                failure.Text += "<li>Please provide an answer for your question.</li>";
            }
            if (confirmedPass.Text.Trim().Length == 0)
            {
                RequiredFieldValidator req = (RequiredFieldValidator)step.ContentTemplateContainer.FindControl("valRequireConfirmPassword");
                req.IsValid = false;
                failure.Text += "<li>Please provide a new password.</li>";
            }
            
            if (failure.Text.Trim().Length > 0)
            {
                ValidationSummary summ = (ValidationSummary)step.ContentTemplateContainer.FindControl("ValidationSummary1");
                if (summ != null)
                {
                    summ.ShowMessageBox = true;
                }

                failure.Text = string.Format("<ul>{0}</ul>", failure.Text);
                failure.Visible = true;
                e.Cancel = true;
            }
        }

}
}
