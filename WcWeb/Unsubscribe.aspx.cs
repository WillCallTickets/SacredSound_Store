using System;
using System.Web.UI.WebControls;

using Wcss;

namespace WillCallWeb
{
    /// <summary>
    /// The purpose of this page os to offer a quick and easy way to unsubscribe a customer from "webuser" mailings.
    /// This page will only function with a seid (subscription email id) in the querystring. Otherwise it will redirect to the contact us page.
    /// seid is there so that the client is sent from the email, we are trying to avoid just being able to call this page directly, because in a perfect
    /// world, users should manage subscriptions in their account
    /// </summary>
    public partial class Unsubscribe : BasePage
    {
        protected int _seid = 0;

        protected override void OnPreInit(EventArgs e)
        {
            QualifySsl(false);
            base.OnPreInit(e);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            lblFeedbackOK.Visible = false;
            lblFeedbackKO.Visible = false;

            _seid = 10008;

            string req = Request.QueryString["seid"];

            if (req == null || req.Trim().Length == 0 || (!Utils.Validation.IsInteger(req)))
                base.Redirect("/MailerManage.aspx");

            _seid = int.Parse(req);
                
        }

        protected void btnUnsub_Click(object sender, EventArgs e)
        {
            //validate input
            if (Page.IsValid)
            {
                //decide which subscription to unsubscribe from based on seid
                SubscriptionEmail mailer = SubscriptionEmail.FetchByID(_seid);

                if (mailer.SubscriptionRecord.ApplicationId == _Config.APPLICATION_ID)
                {
                    int subId = mailer.TSubscriptionId;
                    string email = txtEmail.Text.Trim();

                    string query = "SELECT su.* FROM [SubscriptionUser] su, [Aspnet_Users] u WHERE u.[ApplicationId] = @appId AND u.[LoweredUserName] = LOWER(@email) AND su.[UserId] = u.[UserId]; ";
                    SubSonic.QueryCommand cmd = new SubSonic.QueryCommand(query, SubSonic.DataService.Provider.Name);
                    cmd.Parameters.Add("@email", email);
                    cmd.Parameters.Add("@appId", _Config.APPLICATION_ID, System.Data.DbType.Guid);

                    SubscriptionUserCollection coll = new SubscriptionUserCollection();
                    coll.LoadAndCloseReader(SubSonic.DataService.GetReader(cmd));

                    if (coll.Count > 0)
                    {
                        foreach (SubscriptionUser sUser in coll)
                        {
                            sUser.IsSubscribed = false;
                            sUser.LastActionDate = DateTime.Now;
                            sUser.Save();

                            //TODO create a user event - or a subscripton user event
                            UserEvent.NewUserEvent(email, DateTime.Now, DateTime.Now, _Enums.EventQStatus.Success, email,
                                _Enums.EventQContext.User, _Enums.EventQVerb.SubscriptionUpdate, "Subscribed", "Not Subscribed", string.Format("{0}~{1}",
                                sUser.TSubscriptionId, sUser.SubscriptionRecord.NameAndRecipients), true);
                        }

                        txtEmail.Text = string.Empty;
                        lblFeedbackOK.Text = string.Format("{0} has been successfully removed from our mailings.", email);
                        lblFeedbackOK.Visible = true;
                    }
                    else
                    {
                        UserEvent.NewUserEvent(email, DateTime.Now, DateTime.Now, _Enums.EventQStatus.Failed, email,
                            _Enums.EventQContext.User, _Enums.EventQVerb.SubscriptionUpdate, string.Empty, string.Empty, "User not found", true);

                        txtEmail.Text = string.Empty;
                        lblFeedbackKO.Text = string.Format("{0} could not be found in our database. Please ensure that {0} is the primary recipient of the emails and not a forwarded address. Please <a href=\"Contact.aspx\">contact us</a> if you have any further questions.", email);
                        lblFeedbackKO.Visible = true;
                    }

                    //hide the button to avoid excess calls
                    btnUnsub.Visible = false;
                }
            }
        }

        protected void valEmailPattern_Load(object sender, EventArgs e)
        {
            RegularExpressionValidator regex = (RegularExpressionValidator)sender;

            regex.ValidationExpression = Utils.Validation.regexEmail.ToString();
        }
}
}
