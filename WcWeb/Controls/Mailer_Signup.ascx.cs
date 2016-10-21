using System;
using System.Web.UI.WebControls;

using Wcss;

namespace WillCallWeb.Controls
{
    //do not use in admin - unless the profile used to populate the user issue is worked around
    public partial class Mailer_Signup : WillCallWeb.BaseControl
    {
        public override void PageLogic()
        {
            lblOk.Visible = false;
            lblKo.Visible = false;
        }
        protected void valEmailPattern_Load(object sender, EventArgs e)
        {
            RegularExpressionValidator regex = (RegularExpressionValidator)sender;

            regex.ValidationExpression = Utils.Validation.regexEmail.ToString();
        }
        protected void btnProcess_Click(object sender, EventArgs e)
        {
            lblOk.Text = string.Empty;
            lblKo.Text = string.Empty;

            LinkButton action = (LinkButton)sender;
            bool subscribing = action.ID.IndexOf("btnSub") != -1;
            bool unsubscribing = action.ID.IndexOf("btnUnsub") != -1;

            string input = txtUsername.Text.Trim().ToLower();

            //ignore if no input
            if (input.Length == 0)
                return;

            //do not ignore invalid input
            if (!Utils.Validation.IsValidEmail(input))
            {
                valEmailPattern.IsValid = false;
                valEmailPattern.ErrorMessage = "Please enter a valid email address";
                return;
            }

            //event vars
            string result = string.Empty;
            DateTime now = DateTime.Now;
            Subscription defaultNewsletter = (Subscription)_Lookits.Subscriptions.GetList().Find(delegate(Subscription match) { return match.IsDefault; });

            if (defaultNewsletter == null)
            {
                lblKo.Text = "Sorry. There are currently no active subscriptions.";
                lblKo.Visible = true;
                return;
            }

            //get the user/mem
            AspnetUser user = AspnetUser.GetUserByUserName(input);

            if (unsubscribing)
            {
                //if no user than return/ignore
                if (user == null || user.UserId == Guid.Empty)
                {
                    txtUsername.Text = string.Empty;

                    lblKo.Text = string.Format("{0} is not in our database. Please check your entry and try again.", input);
                    lblKo.Visible = true;

                    return;
                }

                //get the subs for 
                SubscriptionUserCollection coll = new SubscriptionUserCollection().Where("UserId", user.UserId.ToString()).Load();

                bool hasSubscription = false;

                foreach (SubscriptionUser su in coll)
                {
                    if (su.SubscriptionRecord.AspnetRoleRecord.RoleName.ToLower() == "webuser" && su.IsSubscribed)
                    {
                        hasSubscription = true;
                        su.IsSubscribed = false;
                        su.Save();

                        string subInfo = string.Format("{0}~{1}~ Page: {2}", su.SubscriptionRecord.Id, su.SubscriptionRecord.Name, this.Page.ToString());

                        UserEvent.NewUserEvent(user.LoweredUserName, now, now, _Enums.EventQStatus.Success, user.LoweredUserName, _Enums.EventQContext.User,
                            _Enums.EventQVerb.SubscriptionUpdate, "Subscribed", "Not Subscribed", subInfo, true);
                    }
                }

                if (hasSubscription)
                {
                    //remove all instances from the mailqueue
                    SubSonic.QueryCommand cmd =
                        new SubSonic.QueryCommand("DELETE FROM [MailQueue] WHERE [ToAddress] = @username AND ([DateProcessed] IS NULL OR [Status] IS NULL); ",
                            SubSonic.DataService.Provider.Name);
                    cmd.Parameters.Add("@username", user.LoweredUserName);
                    SubSonic.DataService.ExecuteQuery(cmd);

                    //return a result
                    result = string.Format("{0} has been removed from our mailers.", user.LoweredUserName);
                }
                else
                    result = string.Format("{0} is not subscribed to our mailer.", user.LoweredUserName);
            }
            else//if subscribing
            {
                if (user == null || user.UserId == Guid.Empty)
                {
                    user = new AspnetUser();
                    user.ApplicationId = _Config.APPLICATION_ID;
                    user.UserName = input;
                    user.LoweredUserName = input;
                    user.LastActivityDate = DateTime.Now;
                    user.Save();
                }

                //get the subs for 
                SubscriptionUserCollection coll = new SubscriptionUserCollection().Where("UserId", user.UserId.ToString()).Load();

                //if the user is authenticated for the email in question...
                if (user.UserName == this.Profile.UserName.ToLower() && this.Page.User.Identity.IsAuthenticated)
                {
                    //subscribe to the default subscription
                    SubscriptionUser defaultsub = coll.GetList().Find(delegate(SubscriptionUser match) { return match.SubscriptionRecord.IsDefault; });

                    //if not there create
                    if (defaultsub == null)
                    {
                        defaultsub = new SubscriptionUser();
                        defaultsub.DtStamp = DateTime.Now;
                        defaultsub.IsHtmlFormat = true;
                        defaultsub.IsSubscribed = false;//this will be subscribed in a moment - see below
                        defaultsub.LastActionDate = DateTime.Now;
                        defaultsub.TSubscriptionId = defaultNewsletter.Id;
                        defaultsub.UserId = user.UserId;
                    }

                    //update the sub and save and record event
                    if (!defaultsub.IsSubscribed)
                    {
                        defaultsub.IsSubscribed = true;
                        defaultsub.Save();

                        //record events
                        string subInfo = string.Format("{0}~{1}~ Page: {2}", defaultsub.SubscriptionRecord.Id, defaultsub.SubscriptionRecord.Name, this.Page.ToString());

                        UserEvent.NewUserEvent(user.LoweredUserName, now, now, _Enums.EventQStatus.Success, user.LoweredUserName, _Enums.EventQContext.User,
                            _Enums.EventQVerb.SubscriptionUpdate, "Not Subscribed", "Subscribed", subInfo, true);
                    }

                    result = string.Format("{0} is subscribed to our newsletter.", user.LoweredUserName);
                }
                else
                {  
                    //TODO: investigate this when there is more data
                    string freqResult = EventQ.CheckMailerSignupEventFrequency(user.UserId, this.Request.UserHostAddress);

                    if (freqResult != null)
                    {
                        valEmailPattern.IsValid = false;
                        valEmailPattern.ErrorMessage = freqResult;
                        lblKo.Text = freqResult;
                        lblKo.Visible = true;
                        return;
                    }

                    //if the name is already subscribed then quit
                    if (coll.Count > 0)
                    {
                        SubscriptionUser defaultsub = coll.GetList().Find(delegate(SubscriptionUser match) { return match.SubscriptionRecord.IsDefault; });
                        if (defaultsub != null && defaultsub.IsSubscribed)
                        {
                            txtUsername.Text = string.Empty;

                            lblKo.Text = string.Format("{0} is currently subscribed to the newsletter. Did you mean to unsubscribe?", user.UserName);
                            lblKo.Visible = true;

                            return;
                        }
                    }

                    //NOW we can go ahead and sign up the name - send an email
                    //dont create a subscriptionuser here - wait for signup page - where we will create a tracking event
                    //send account id in email to track
                    MailQueue.SendMailerSignupNotification(user.UserName, user.UserId.ToString(), DateTime.Now,
                        string.Format("the sign up control located on our MailerManage page.", this.Page.ToString()), defaultNewsletter);

                    //and return a msg to wait for email
                    result = string.Format("A confirmation email has been sent to {0}.", user.UserName);

                    //where from and what subscription
                    string subInfo = string.Format("{0}~{1}~ Page: {2}", defaultNewsletter.Id, defaultNewsletter.Name, this.Page.ToString());

                    UserEvent.NewUserEvent(user.LoweredUserName, now, now, _Enums.EventQStatus.Success, user.LoweredUserName, _Enums.EventQContext.User,
                        _Enums.EventQVerb.Mailer_SignupAwaitConfirm, null, null, subInfo, true);
                }
            }

            //clear input
            txtUsername.Text = string.Empty;

            lblOk.Text = result;
            lblOk.Visible = result.Trim().Length > 0;
        }

        
    
}
}
