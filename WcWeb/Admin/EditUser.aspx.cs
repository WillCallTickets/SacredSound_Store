using System;
using System.Collections.Generic;
using System.Web.Security;
using System.Web.UI.WebControls;
using System.Net.Mail;

using Wcss;

namespace WillCallWeb.Admin
{
    public class FalseMembershipProvider : MembershipProvider
    {
        public FalseMembershipProvider() { }

        #region Override members

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }
        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }
        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }
        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }
        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }
        public override string GetUserNameByEmail(string email)
        {
            throw new NotImplementedException();
        }
        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException();
        }
        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            throw new NotImplementedException();
        }
        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }
        public override bool ValidateUser(string username, string password)
        {
            throw new NotImplementedException();
        }
        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException();
        }
        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }
        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }
        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }
        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            throw new NotImplementedException();
        }
        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }
        public override string PasswordStrengthRegularExpression
        {
            get { throw new NotImplementedException(); }
        }
        public override int MinRequiredNonAlphanumericCharacters
        {
            get { throw new NotImplementedException(); }
        }
        public override int MinRequiredPasswordLength
        {
            get { throw new NotImplementedException(); }
        }
        public override MembershipPasswordFormat PasswordFormat
        {
            get { throw new NotImplementedException(); }
        }
        public override bool RequiresUniqueEmail
        {
            get { throw new NotImplementedException(); }
        }
        public override int PasswordAttemptWindow
        {
            get { throw new NotImplementedException(); }
        }
        public override int MaxInvalidPasswordAttempts
        {
            get {throw new NotImplementedException(); }
        }
        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
        public override bool RequiresQuestionAndAnswer
        {
            get { throw new NotImplementedException(); }
        }
        public override bool EnablePasswordReset
        {
            get { throw new NotImplementedException(); }
        }
        public override bool EnablePasswordRetrieval
        {
            get { throw new NotImplementedException(); }
        }
        #endregion

        public string GetPasswordAnswer(Guid providerUserKey)
        {
            try
            {
                SubSonic.QueryCommand cmd = new SubSonic.QueryCommand("SELECT [PasswordAnswer] FROM [aspnet_Membership] WHERE UserID=@UserID", 
                    SubSonic.DataService.Provider.Name);
                cmd.Parameters.Add("@UserId", providerUserKey, System.Data.DbType.Guid);

                object answer = SubSonic.DataService.ExecuteScalar(cmd);

                if (answer != null)
                    return ProviderDecryptor(answer.ToString());
            }
            catch (Exception ex)
            {
                _Error.LogException(ex);
            }

            return null;
        }
        /// Generic Decryptor function. Can be used to decrypt Password and Password Answer.
        /// Only works if passwordFormat is set to "Encrypted" - wfp june 04 2009
        /////////
        internal string ProviderDecryptor(string encryptedText)
        {
            string decrypted = null;
            if (!string.IsNullOrEmpty(encryptedText))
            {
                byte[] encodedbytes = Convert.FromBase64String(encryptedText);
                byte[] decryptedbytes = base.DecryptPassword(encodedbytes);
                if (decryptedbytes != null)
                    decrypted = System.Text.Encoding.Unicode.GetString(decryptedbytes, 16, decryptedbytes.Length - 16);
            }
            return decrypted;
        }
    }

    public partial class EditUser : WillCallWeb.BasePage
    {
        protected void litHintAnswer_DataBinding(object sender, EventArgs e)
        {
            if (this.Page.User.IsInRole("Super"))
            {
                Literal litHintAnswer = (Literal)sender;

                FalseMembershipProvider prov = new FalseMembershipProvider();
                MembershipUser user = Membership.GetUser(userName);

                if (user != null && user.ProviderUserKey != null)
                {
                    try
                    {
                        string answer = prov.GetPasswordAnswer((Guid)user.ProviderUserKey);

                        if (answer != null)
                            litHintAnswer.Text = answer;
                    }
                    catch (Exception ex)
                    {
                        _Error.LogException(ex);
                    }
                }
            }
        }
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            //set opacity for nav events
            if (this.HasControls() && this.UpdatePanel1.Visible)
                Atx.RegisterJQueryScript_BlockUI_AjaxMethod(this.UpdatePanel1, "#edituser", true);
        }
        protected override void OnPreInit(EventArgs e)
        {
            QualifySsl(true);
            base.OnPreInit(e);
        }

        protected void btnSales_Click(object sender, EventArgs e)
        {
            base.Redirect(string.Format("/Admin/CustomerEditor.aspx?p=sales&UserName={0}", userName));
        }

        protected string userName = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            // retrieve the username from the querystring
            userName = this.Request.QueryString["UserName"];

            if (userName == null || userName.Trim().Length == 0)
                base.Redirect("CustomerEditor.aspx?p=customerpicker");

            rptPreviousUsername.DataBind();

            lblRolesFeedbackOK.Visible = false;
            lblProfileFeedbackOK.Visible = false;

            // show the user's details
            MembershipUser user = Membership.GetUser(userName);

            if (!this.IsPostBack)
            {
                UserSubscriptions1.UserName = userName;
                UserProfile1.UserName = userName;
                BindRoles();

                if (user != null)
                {
                    lblUserID.Text = user.ProviderUserKey.ToString();
                    lblUserName.Text = user.UserName;
                    lnkEmail.Text = user.Email;
                    lnkEmail.NavigateUrl = "mailto:" + user.Email;                    
                    lblRegistered.Text = user.CreationDate.ToString("ddd MM/dd/yyyy hh:mmtt");
                    lblLastLogin.Text = user.LastLoginDate.ToString("ddd MM/dd/yyyy hh:mmtt");
                    lblLastActivity.Text = user.LastActivityDate.ToString("ddd MM/dd/yyyy hh:mmtt");
                    chkOnlineNow.Checked = user.IsOnline;
                    chkApproved.Checked = user.IsApproved;
                    chkApproved.Enabled = this.User.IsInRole("Administrator");
                    chkLockedOut.Checked = user.IsLockedOut;
                    chkLockedOut.Enabled = user.IsLockedOut;

                    litUserIsEmail.Text = string.Empty;
                    panelRoles.Enabled = true;

                    btnUpdateProfile.Enabled = true;
                }
                else
                {
                    btnUpdateProfile.Enabled = false;

                    string qry = string.Format("SELECT a.* FROM Aspnet_Users a WHERE a.[UserName] = @userName");
                    SubSonic.QueryCommand cmd = new SubSonic.QueryCommand(qry, SubSonic.DataService.Provider.Name);
                    cmd.Parameters.Add("@userName", userName);

                    AspnetUserCollection aColl = new AspnetUserCollection();
                    aColl.LoadAndCloseReader(SubSonic.DataService.GetReader(cmd));

                    if (aColl.Count > 0)
                    {
                        AspnetUser usr = aColl[0];

                        lblUserID.Text = usr.UserId.ToString();
                        lblUserName.Text = usr.UserName;
                        lnkEmail.Text = usr.LoweredUserName;
                        lnkEmail.NavigateUrl = "mailto:" + usr.LoweredUserName;                        
                        lblLastActivity.Text = usr.LastActivityDate.ToString("f");                        
                        
                        //indicate if they had a previous account
                        //see if they are possibly an old user
                        AspnetUsersOld oldie = AspnetUsersOld.GetOldUser(userName);

                        //email only will be null
                        if (oldie != null)
                            lblRegistered.Text = "Has old account - not updated in new system.";
                        else
                            lblRegistered.Text = "Not registered in new system.";

                        chkOnlineNow.Enabled = false;
                        chkApproved.Enabled = false;
                        chkLockedOut.Enabled = false;

                        litUserIsEmail.Text = "<div style=\"font-weight: bold; color: #a40000;\">Roles cannot be edited for unregistered user.</div>"; ;
                        panelRoles.Enabled = false;
                    }
                }               
            }


            if (litPassword != null && litPassword.Visible)
                litPassword.DataBind();
                
            if(btnReset != null && btnReset.Visible && btnReset.Enabled)
                btnReset.Enabled = user != null && user.UserName.ToLower() != User.Identity.Name.ToLower();

            if (!IsPostBack)
            {
                txtCredit.DataBind();
                lblResetCredit.Text = string.Empty;
            }

            btnResetCredit.Enabled = this.Page.User.IsInRole("Super") || this.Page.User.IsInRole("Administrator");
        }

        protected void txtCredit_DataBinding(object sender, EventArgs e)
        {
            TextBox txt = (TextBox)sender;
            ProfileCommon p = Profile.GetProfile(userName);

            txt.Text = p.StoreCredit.ToString();
        }

        protected void btnSyncCredit_Click(object sender, EventArgs e)
        {
            //this will update the profile - record the sync event
            decimal realTimeAmount = WillCallWeb.StoreObjects.SaleItem_StoreCredit.Profile_StoreCredit_Sync(userName, this.Profile.UserName, true);

            //rebind
            txtCredit.DataBind();
            GridView1.DataBind();
        }

        protected void btnResetCredit_Click(object sender, EventArgs e)
        {
            if (txtCreditAdjustment.Text.Trim().Length > 0)
            {
                decimal input = 0;

                try
                {
                    if (decimal.TryParse(txtCreditAdjustment.Text.Trim(), out input))
                    {
                        if (input == 0)
                            throw new Exception("Store credit adjustment must have an amount.");

                        ProfileCommon p = Profile.GetProfile(userName);

                        //add/subtract amount
                        WillCallWeb.StoreObjects.SaleItem_StoreCredit.Profile_StoreCredit_Adjust(this.Profile, userName, input);

                        //make sure we are in sync
                        decimal realTimeAmount = WillCallWeb.StoreObjects.SaleItem_StoreCredit.Profile_StoreCredit_Sync(userName, this.Profile.UserName, false);

                        decimal profileCredit = decimal.Parse(p.StoreCredit.ToString());

                        //if (profileCredit != input)
                        //{
                        //    decimal difference = input - profileCredit;
                        //    WillCallWeb.StoreObjects.SaleItem_StoreCredit.Profile_StoreCredit_Adjust(this.Profile, userName, difference);
                        //    GridView1.DataBind();
                        //}
                    }
                    else
                        throw new Exception("Store credit must be a valid decimal amount");
                }
                catch (Exception ex)
                {
                    lblResetCredit.Text = ex.Message;
                }
            }

            txtCreditAdjustment.Text = string.Empty;
            txtCredit.DataBind();
            GridView1.DataBind();
        }

        protected void UserSubscriptions_Updated(object sender, EventArgs e)
        {
            GridView1.DataBind();
        }

        private void BindRoles()
        {
            if (this.User.IsInRole("Administrator"))
                chkRoles.DataBind();
        }

        protected void btnUpdateProfile_Click(object sender, EventArgs e)
        {
            UserProfile1.SaveProfile();
            lblProfileFeedbackOK.Visible = true;

            GridView1.DataBind();
        }

        protected void btnCreateRole_Click(object sender, EventArgs e)
        {
            if (!Roles.RoleExists(txtNewRole.Text.Trim()))
            {
                Roles.CreateRole(txtNewRole.Text.Trim());
                txtNewRole.Text = string.Empty;
                BindRoles();
            }
        }

        protected void btnUpdateRoles_Click(object sender, EventArgs e)
        {
            // first remove the user from all roles...
            List<string> addRoles = new List<string>();
            List<string> deleteRoles = new List<string>();

            foreach (ListItem item in chkRoles.Items)
            {
                if (item.Selected && (!Roles.IsUserInRole(userName, item.Text)))
                    addRoles.Add(item.Text);
                else if ((!item.Selected) && (Roles.IsUserInRole(userName, item.Text)))
                    deleteRoles.Add(item.Text);
            }

            if (deleteRoles.Count > 0)
            {
                Roles.RemoveUserFromRoles(userName, deleteRoles.ToArray());
                foreach (string s in deleteRoles)
                    UserEvent.NewUserEvent(userName, DateTime.Now, DateTime.Now, _Enums.EventQStatus.Success, Page.User.Identity.Name, _Enums.EventQContext.User,
                        _Enums.EventQVerb.Role_Delete, null, null, s, true);

                Subscription.RemoveUserFromUnauthorizedSubscriptions(userName);
            }

            if (addRoles.Count > 0)
            {
                Roles.AddUserToRoles(userName, addRoles.ToArray());
                foreach (string s in addRoles)
                    UserEvent.NewUserEvent(userName, DateTime.Now, DateTime.Now, _Enums.EventQStatus.Success, Page.User.Identity.Name, _Enums.EventQContext.User,
                        _Enums.EventQVerb.Role_Add, null, null, s, true);
            }

            lblRolesFeedbackOK.Visible = true;

            UpdatePanel1.DataBind();
            GridView1.DataBind();
        }
                
        protected void chkApproved_CheckedChanged(object sender, EventArgs e)
        {
            MembershipUser user = Membership.GetUser(userName);
            bool oldApproved = user.IsApproved;
            user.IsApproved = chkApproved.Checked;
            Membership.UpdateUser(user);

            UserEvent.NewUserEvent(userName, DateTime.Now, DateTime.Now, _Enums.EventQStatus.Success, Page.User.Identity.Name, _Enums.EventQContext.User,
                _Enums.EventQVerb.UserUpdate, oldApproved.ToString(), chkApproved.Checked.ToString(), "User Active", true);

            GridView1.DataBind();
        }

        protected void chkLockedOut_CheckedChanged(object sender, EventArgs e)
        {
            if (!chkLockedOut.Checked)
            {
                MembershipUser user = Membership.GetUser(userName);
                bool oldLocked = user.IsLockedOut;
                user.UnlockUser();
                chkLockedOut.Enabled = false;

                UserEvent.NewUserEvent(userName, DateTime.Now, DateTime.Now, _Enums.EventQStatus.Success, Page.User.Identity.Name, _Enums.EventQContext.User,
                    _Enums.EventQVerb.UserUpdate, oldLocked.ToString(), chkLockedOut.Checked.ToString(), "User Is LockedOut", true);

                GridView1.DataBind();
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            MembershipProvider mp = Membership.Providers["AdminMembershipProvider"];

            MembershipUser user = mp.GetUser(userName, false);

            System.Collections.Specialized.ListDictionary dict = new System.Collections.Specialized.ListDictionary();

            string newPass = Utils.ParseHelper.GenerateRandomPassword(7);

            user.ChangePassword(user.GetPassword(), newPass);

            //email customer with new password
            try
            {
                //always send html email - at this point
                string template = string.Format("/{0}/MailTemplates/SiteTemplates/PasswordResetEmail.txt", _Config._VirtualResourceDir);
                string mappedFile = (System.Web.HttpContext.Current != null) ?
                    System.Web.HttpContext.Current.Server.MapPath(template) :
                    string.Format("{0}\\WcWeb{1}", _Config._MappedRootDirectory, template);

                string content = Utils.FileLoader.FileToString(mappedFile);

                dict.Add("<% Password %>", newPass);

                content = Utils.ParseHelper.DoReplacements(content, dict, true);


                MailMessage email = new MailMessage(new MailAddress(_Config._CustomerService_Email, _Config._CustomerService_FromName),
                    new MailAddress((_Config._RefundTestMode) ? _Config._Admin_EmailAddress : user.UserName));

                email.Subject = "Your Requested Information";
                email.Body = content;

                SmtpClient client = new SmtpClient();
                client.Send(email);

                user.UnlockUser();
                chkLockedOut.Enabled = false;

                UserEvent.NewUserEvent(user.UserName, DateTime.Now, DateTime.Now, _Enums.EventQStatus.Success, Page.User.Identity.Name,
                   _Enums.EventQContext.User, _Enums.EventQVerb.PasswordReset, string.Empty, string.Empty, string.Empty, true);

                GridView1.DataBind();
            }
            catch (Exception ex)
            {
                _Error.LogException(ex);
            }
        }

        protected void chkRoles_DataBinding(object sender, EventArgs e)
        {
            // fill the CheckBoxList with all the available roles, and then select
            // those that the user belongs to            
            List<string> allRoles = new List<string>();
            allRoles.AddRange(Roles.GetAllRoles());

            if (!this.User.IsInRole("_Master"))
                allRoles.Remove("_Master");

            chkRoles.DataSource = allRoles;
        }
        
        protected void chkRoles_DataBound(object sender, EventArgs e)
        {
            List<string> allRoles = new List<string>();
            allRoles.AddRange(Roles.GetAllRoles());
            if (!this.User.IsInRole("_Master"))
                allRoles.Remove("_Master");

            List<string> inRoles = new List<string>();
            inRoles.AddRange(Roles.GetRolesForUser(userName));

            foreach (string role in allRoles)
            {
                ListItem li = chkRoles.Items.FindByText(role);
                if (li != null)
                {
                    foreach (string s in inRoles)
                        if (s.ToLower() == role.ToLower())
                            li.Selected = true;

                    if (role == "Administrator")
                        li.Enabled = this.User.IsInRole("Administrator");

                    //only master admin can create supers
                    if (role == "Super")
                        li.Enabled = this.User.IsInRole("_Master");

                    if (role == "_Master")
                        li.Enabled = false;// this.User.IsInRole("_Master") && inRoles.Contains("_Master");
                }
            }
        }

        protected void btnChangeEmail_Click(object sender, EventArgs e)
        {
            //verify email
            string input = txtNewEmail.Text.Trim();
            if (input.Trim().Length > 0 && Utils.Validation.IsValidEmail(input.Trim()))
            {
                string newUserName = input.ToLower();
                string currentUserName = this.Request.QueryString["UserName"].ToLower();

                try
                {
                    if (!Utils.Validation.IsValidEmail(currentUserName))
                        throw new Exception("Current email is invalid.");

                    //note that the redirection handles all of the profile reloading we need here
                    if (Customer.ChangeUserName(true, this.Profile.UserName, currentUserName, newUserName))
                        base.Redirect(string.Format("/Admin/EditUser.aspx?UserName={0}", newUserName));
                }
                catch (System.Threading.ThreadAbortException) { }
                catch (Exception ex)
                {
                    CustomValidator custom = (CustomValidator)this.Page.Master.FindControl("CustomValidator");
                    if (custom != null)
                    {
                        custom.ErrorMessage = ex.Message;
                        custom.IsValid = false;
                    }

                    _Error.LogException(ex);
                }
                finally
                {
                    GridView1.DataBind();
                }
            }
        }
        protected void valEmailPattern_Load(object sender, EventArgs e)
        {
            RegularExpressionValidator regex = (RegularExpressionValidator)sender;

            regex.ValidationExpression = Utils.Validation.regexEmail.ToString();
        }
        protected void rptPreviousUsername_DataBinding(object sender, EventArgs e)
        {
            Repeater rpt = (Repeater)sender;
            List<ListItem> list = new List<ListItem>();

            string qry = string.Format("SELECT a.* FROM Aspnet_Users a WHERE a.[UserName] = @userName");
            SubSonic.QueryCommand cmd = new SubSonic.QueryCommand(qry, SubSonic.DataService.Provider.Name);
            cmd.Parameters.Add("@userName", userName);

            AspnetUserCollection aColl = new AspnetUserCollection();
            aColl.LoadAndCloseReader(SubSonic.DataService.GetReader(cmd));

            if (aColl.Count > 0)
            {
                AspnetUser usr = aColl[0];
            //MembershipUser user = Membership.GetUser(userName);

            //if (user != null)
            //{
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("SET NOCOUNT ON SELECT DISTINCT [EmailAddress] as 'Email' FROM [User_PreviousEmail] WHERE [UserId] = @userId; ");
                SubSonic.QueryCommand cmd1 = new SubSonic.QueryCommand(sb.ToString(), SubSonic.DataService.Provider.Name);
                //cmd1.Parameters.Add("@userId", user.ProviderUserKey.ToString());
                cmd1.Parameters.Add("@userId", usr.UserId.ToString());

                using (System.Data.IDataReader dr = SubSonic.DataService.GetReader(cmd1))
                {
                    while (dr.Read())
                        list.Add(new ListItem(dr["Email"].ToString()));

                    dr.Close();
                }
            }

            rpt.DataSource = list;
        }

        protected void SqlDataSource1_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters["@appId"].Value = Wcss._Config.APPLICATION_ID;
        }
        
        protected void litPassword_DataBinding(object sender, EventArgs e)
	    {
	        Literal lit = (Literal)sender;
	
	        lit.Text = string.Empty;
	
	        if (this.Page.User.IsInRole("Super"))
	        {
	            MembershipUser usr = Membership.Providers["AdminMembershipProvider"].GetUser(userName, false);
	            if(usr != null)
	                lit.Text = usr.GetPassword();
	            else
	            {
	                //see if they are possibly an old user
	                AspnetUsersOld oldie = AspnetUsersOld.GetOldUser(userName);
	
	                //email only will be null
	                if (oldie != null)
	                {
	                    lit.Text = oldie.Password;
	                }
	            }
	        }
        }
        
}
}