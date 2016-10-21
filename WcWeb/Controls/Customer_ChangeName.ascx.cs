using System;
using System.Web.Security;
using System.Web.UI.WebControls;
using System.Net.Mail;

using Wcss;

namespace WillCallWeb.Controls
{
    public partial class Customer_ChangeName : WillCallWeb.BaseControl
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            this.valUsername.ValidationExpression = Utils.Validation.regexEmail.ToString();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                // show the user's details
                if (Question != null && Question.Items.Count == 1)//there is a default item
                {
                    Question.DataSource = Wcss._Lookits.HintQuestions;
                    Question.DataBind();
                }
            }
        }

        protected void btnChangeName_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                //do this first so that we can use it in error reporting
                string userName = this.Profile.UserName;
                string newUserName = NewUsername.Text.Trim();

                try
                {
                    //revalidate - this is too important
                    if (!_Config._AllowCustomerInitiatedNameChanges)
                        throw new Exception("Sorry, this feature is currently disabled.");

                    //Validate inputs
                    if (!Utils.Validation.IsValidEmail(userName))
                        throw new Exception("The current email address is not valid.");

                    if (!Utils.Validation.IsValidEmail(newUserName.Trim()))
                        throw new Exception("Please enter a valid email address.");

                    string pass = CurrentPassword.Text.Trim();
                    bool valid = Membership.ValidateUser(userName, pass);

                    if (!valid)
                    {
                        valRequireCurrentPassword.IsValid = false;
                        valRequireCurrentPassword.ErrorMessage = "The password you have entered is not valid. Please try again";
                        return;
                    }

                    if (Question.SelectedIndex <= 0)
                    {
                        reqReqQuestion.IsValid = false;
                        reqReqQuestion.ErrorMessage = "You must select a security question.";
                        return;
                    }

                    //End input validation

                    //if the username chage is ok - prepare for redirection
                    //logging is handled in the static method
                    if (Customer.ChangeUserName(true, userName, userName, newUserName))
                    {
                        MembershipUser mem = Membership.GetUser(newUserName);
                        if (mem != null)
                        {
                            mem.ChangePasswordQuestionAndAnswer(pass, Question.SelectedValue, Answer.Text.Trim());

                            //load new profile
                            this.Profile.GetProfile(newUserName);

                            //ensure the auth objects are updated - if we do not do this, it is possible to create additional accounts - A MESS!
                            FormsAuthentication.SignOut();
                            //FormsAuthentication.Authenticate(newUserName, pass);
                            bool validUser = Membership.ValidateUser(newUserName, pass);
                            if (validUser)
                            {
                                FormsAuthentication.SetAuthCookie(newUserName, false);

                                base.Redirect(string.Format("/EditProfile.aspx", newUserName));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    CustomValidator custom = (CustomValidator)this.Page.Master.FindControl("RemovalValidator");
                    if (custom != null)
                    {
                        custom.ErrorMessage = ex.Message;
                        custom.IsValid = false;
                    }

                    _Error.LogException(ex);
                }
            }
        }
}
}
