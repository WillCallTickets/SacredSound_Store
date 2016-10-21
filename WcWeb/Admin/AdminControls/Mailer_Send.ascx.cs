using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using Wcss;

namespace WillCallWeb.Admin.AdminControls
{
    public partial class Mailer_Send : BaseControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {            
            lblFeedback.Text = string.Empty;
            lblFeedback.Visible = false;

            if (Ctx.CurrentPageException != null)
            {
                if (Ctx.CurrentPageException.Trim().Length > 0)
                {
                    lblFeedback.Text = Ctx.CurrentPageException;
                    lblFeedback.Visible = true;
                }

                Ctx.CurrentPageException = null;
            }

            if (!IsPostBack)
            {
                ddlMailers.DataBind();
                FormView1.DataBind();
            }
        }

        #region Email Selection

        protected void ddlMailers_DataBinding(object sender, EventArgs e)
        {
            SubscriptionEmailCollection coll = new SubscriptionEmailCollection();
            SubSonic.QueryCommand cmd = new
                SubSonic.QueryCommand("SELECT se.* FROM [SubscriptionEmail] se, [Subscription] s WHERE s.[ApplicationId] = @appId AND s.[Id] = se.[tSubscriptionId] ORDER BY [Id] DESC ",
                SubSonic.DataService.Provider.Name);
            cmd.Parameters.Add("@appId", _Config.APPLICATION_ID, DbType.Guid);

            coll.LoadAndCloseReader(SubSonic.DataService.GetReader(cmd));

            DropDownList ddl = (DropDownList)sender;

            ddl.DataSource = coll;
            ddl.DataTextField = "EmailLetterName";
            ddl.DataValueField = "Id";
        }
        protected void ddlMailers_DataBound(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;

            if (ddl.Items.Count > 0)
            {
                ListItem li;
                ddl.SelectedIndex = -1;
                if (Atx.CurrentSubscriptionEmailId == 0)
                    li = ddl.Items[0];
                else
                    li = ddl.Items.FindByValue(Atx.CurrentSubscriptionEmailId.ToString());
                    
                if (li != null)
                    li.Selected = true;
            }
        }
        protected void ddlMailers_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;

            Atx.CurrentSubscriptionEmailId = int.Parse(ddl.SelectedValue);
            FormView1.DataBind();
        }

        #endregion

        protected void btnEditor_Click(object sender, EventArgs e)
        {
            base.Redirect("/Admin/Mailers.aspx?p=edit");
        }

        #region Form View

        protected void FormView1_DataBinding(object sender, EventArgs e)
        {
            FormView form = (FormView)sender;

            SubscriptionEmailCollection coll = new SubscriptionEmailCollection();
            SubSonic.QueryCommand cmd = new
                SubSonic.QueryCommand("SELECT se.* FROM [SubscriptionEmail] se, [Subscription] s WHERE s.[ApplicationId] = @appId AND s.[Id] = se.[tSubscriptionId] ORDER BY [Id] DESC ",
                SubSonic.DataService.Provider.Name);
            cmd.Parameters.Add("@appId", _Config.APPLICATION_ID, DbType.Guid);

            coll.LoadAndCloseReader(SubSonic.DataService.GetReader(cmd));

            if (coll.Count > 0)
            {
                SubscriptionEmail chosen;

                if (Atx.CurrentSubscriptionEmailId == 0)
                {
                    chosen = coll[0];
                    Atx.CurrentSubscriptionEmailId = chosen.Id;
                }
                else
                    chosen = (SubscriptionEmail)coll.Find(Atx.CurrentSubscriptionEmailId);

                coll.Clear();
                if(chosen != null)
                    coll.Add(chosen);
            }

            form.DataSource = coll;
        }
        protected void FormView1_DataBound(object sender, EventArgs e)
        {
            FormView form = (FormView)sender;
            SubscriptionEmail subEmail = (SubscriptionEmail)form.DataItem;

            #region OnClientClick

            Button remove = (Button)form.FindControl("btnRemoveQ");
            if(remove != null)
                remove.OnClientClick = string.Format("return confirm('Are you sure you want to REMOVE all unsent ocurrences of {0} from the queue?')",
                        Utils.ParseHelper.ParseJsAlert(subEmail.EmailLetterName));

            Button pause = (Button)form.FindControl("btnPauseQ");
            if (pause != null)
                pause.OnClientClick = string.Format("return confirm('Are you sure you want to PAUSE all unsent ocurrences of {0} in the queue?')",
                        Utils.ParseHelper.ParseJsAlert(subEmail.EmailLetterName));

            Button restart = (Button)form.FindControl("btnRestartQ");
            if (restart != null)
                restart.OnClientClick = string.Format("return confirm('Are you sure you want to RESTART all unsent ocurrences of {0} in the queue?')",
                        Utils.ParseHelper.ParseJsAlert(subEmail.EmailLetterName));

            Button subscriber = (Button)form.FindControl("btnSubscribers");
            if (subscriber != null)
                subscriber.OnClientClick = string.Format("return confirm('Are you sure you have confirmed the correct recipients and the correct time to send the email?')",
                        Utils.ParseHelper.ParseJsAlert(subEmail.EmailLetterName));

            #endregion

            #region Literals

            Literal subscribed = (Literal)form.FindControl("litSubscribed");
            Literal inQueue = (Literal)form.FindControl("litInQueue");

            if (subscribed != null)
            {
                SubSonic.Query qry = new SubSonic.Query("SubscriptionUser");
                qry.WHERE(string.Format("TSubscriptionId = {0} ", subEmail.TSubscriptionId));
                qry.AND("bSubscribed", "true");

                int subbed = qry.GetRecordCount();

                subscribed.Text = string.Format("{0} {1}{2} subscribed.", subbed, 
                    subEmail.SubscriptionRecord.AspnetRoleRecord.RoleName,
                    (subbed == 1) ? " is" : "s are");
            }

            if (inQueue != null)
            {
                int queued = 0;
                int sent = 0;
                int total = 0;

                SubSonic.StoredProcedure s = SPs.TxMailerLetterStats(_Config.APPLICATION_ID, System.Data.SqlTypes.SqlDateTime.MinValue.ToString(), System.Data.SqlTypes.SqlDateTime.MaxValue.ToString(),
                    subEmail.TEmailLetterId, queued, sent, total);

                s.Execute();

                queued = (int)s.OutputValues[0];
                sent = (int)s.OutputValues[1];
                total = (int)s.OutputValues[2];

                inQueue.Text = string.Format("<th>Queued</th><td style=\"padding-right:24px;{3}\">{0}</td><th>Sent</th><td style=\"padding-right:24px;\">{1}</td><th>Total Occurrences</th><td style=\"padding-right:24px;\">{2}</td>",
                    queued.ToString(), sent.ToString(), total.ToString(),
                    (queued > 0) ? "color:red;" : string.Empty);


                //SubSonic.Query qry = new SubSonic.Query("MailQueue");
                //qry.WHERE(string.Format("TEmailLetterId = {0} ", subEmail.TEmailLetterId));
                //qry.AND("Status", SubSonic.Comparison.Is, null);
                //qry.AND("ApplicationId", _Config.APPLICATION_ID);

                //int qed = qry.GetRecordCount();

                //inQueue.Text = string.Format("{0} currently queued", qed);
            }

            #endregion
        }
        protected void FormView1_ModeChanging(object sender, FormViewModeEventArgs e)
        {
            FormView form = (FormView)sender;
            form.ChangeMode(e.NewMode);
            if (e.CancelingEdit)
                form.DataBind();
        }
        protected void FormView1_ItemCommand(object sender, FormViewCommandEventArgs e)
        {
            FormView form = (FormView)sender;

            string cmd = e.CommandName.ToLower();
            string msg = string.Empty;

            WillCallWeb.Components.Util.CalendarClock clockSend = (WillCallWeb.Components.Util.CalendarClock)form.FindControl("clockSend");

            try
            {
                switch (cmd)
                {
                    case "gotoeditpage":
                        base.Redirect("/Admin/Mailers.aspx?p=edit");
                        break;
                    case "qremove":
                        SPs.TxSubscriptionRemoveMailerFromQueue(Atx.CurrentSubscriptionEmailId).Execute();
                        break;
                    case "qpause":
                        SPs.TxSubscriptionPauseMailerInQueue(Atx.CurrentSubscriptionEmailId).Execute();
                        break;
                    case "qrestart":
                        SPs.TxSubscriptionRestartMailerInQueue(Atx.CurrentSubscriptionEmailId).Execute();
                        break;
                    case "sendtest":
                        TextBox testList = (TextBox)form.FindControl("txtTestList");
                        msg = ProcessMailList(testList, DateTime.Now, 0);
                        if (msg.Trim().Length > 0)
                        {
                            CustomValidator validation = (CustomValidator)form.FindControl("CustomTest");
                            if (validation != null)
                            {
                                validation.IsValid = false;
                                validation.ErrorMessage = msg;
                            }
                            return;
                        }
                        else
                        {
                            testList.Text = string.Empty;
                            lblFeedback.Visible = true;
                            lblFeedback.Text = "Test emails have been queued.";

                            if (clockSend != null)
                                clockSend.SelectedValue = string.Empty;
                        }
                        break;
                    case "sendshortlist":
                        TextBox shortList = (TextBox)form.FindControl("txtShortList");

                        if (clockSend == null || (!clockSend.HasSelection))
                            msg = "You must specify a date to send the custom list.";
                        else
                        {
                            msg = ProcessMailList(shortList, clockSend.SelectedDate, 0);

                        }
                        //  

                        if (msg.Trim().Length > 0)
                        {
                            CustomValidator validation = (CustomValidator)form.FindControl("CustomShort");
                            if (validation != null)
                            {
                                validation.IsValid = false;
                                validation.ErrorMessage = msg;
                            }
                            return;
                        }
                        else
                        {
                            shortList.Text = string.Empty;
                            lblFeedback.Visible = true;
                            lblFeedback.Text = "Listed emails have been queued.";

                            if (clockSend != null)
                                clockSend.SelectedValue = string.Empty;
                        }
                        break;
                    case "sendsubscription":
                        if (clockSend == null || (!clockSend.HasSelection))
                        {
                            CustomValidator validation = (CustomValidator)form.FindControl("CustomSubscription");
                            if (validation != null)
                            {
                                validation.IsValid = false;
                                validation.ErrorMessage = "You must specify a date to send the subscription.";
                            }
                            return;
                        }
                        else if (clockSend != null)
                        {
                            Ctx.CurrentPageException = string.Format("{0} Emails have been queued for the subscription on {1}.",
                                ProcessSubscription(clockSend.SelectedDate, 10), clockSend.SelectedDate.ToString());

                            clockSend.SelectedValue = string.Empty;

                            base.Redirect("/Admin/ProcessingMailer.aspx?redir=send");
                        }

                        break;
                }

                form.DataBind();
            }
            catch (System.Threading.ThreadAbortException) { }
            catch (Exception ex)
            {
                _Error.LogException(ex);

                lblFeedback.Visible = true;
                lblFeedback.Text = ex.Message;
                lblFeedback.ForeColor = System.Drawing.Color.Red;
            }
        }

        #endregion

        protected string ProcessSubscription(DateTime sendDate, int priority)
        {
            SubscriptionEmail subEmail = SubscriptionEmail.FetchByID(Atx.CurrentSubscriptionEmailId);

            if (subEmail != null && subEmail.SubscriptionRecord.ApplicationId != _Config.APPLICATION_ID)
                subEmail = null;

            if (subEmail != null)
            {
                subEmail.EnsurePublication(true);

                int res = (int)SPs.TxSubscriptionInsertMailerIntoQueue(_Config.APPLICATION_ID, Atx.CurrentSubscriptionEmailId, sendDate.ToString(),
                    _Config._MassMailService_FromName, _Config._MassMailService_Email, 10).ExecuteScalar();

                //create a history event for the subemail
                HistorySubscriptionEmail.Insert(DateTime.Now, Atx.CurrentSubscriptionEmailId, DateTime.Now, res);

                return res.ToString();
            }

            return "0";
        }
        protected string ProcessMailList(TextBox mailList, DateTime sendDate, int priority)
        {
            SubscriptionEmail subEmail = SubscriptionEmail.FetchByID(Atx.CurrentSubscriptionEmailId);

            if (subEmail != null && subEmail.SubscriptionRecord.ApplicationId != _Config.APPLICATION_ID)
                subEmail = null;

            if (subEmail != null)
            {
                subEmail.EnsurePublication(true);

                string msg = string.Empty;

                //parse out emails
                string input = mailList.Text.Trim();

                if (input.Length == 0)
                {
                    msg = "No email addresses were specified for delivery.";
                    return msg;
                }

                List<string> valid = new List<string>();
                List<string> invalid = new List<string>();

                //do this next step to 1)remove any angle brackets 2)format the string so that we can SPLIT it later on ~
                //this may seem redundant - but it is necessary to facilitate how the production server may see things
                string parsed = input.Replace("<", string.Empty).Replace(">", string.Empty).Replace(",", "~")
                    .Replace(Environment.NewLine, "~").Replace("\r\n", "~").Replace("\n", "~");

                string[] mails = parsed.Split('~');

                foreach (string s in mails)
                {
                    if (s.Trim().Length > 0)
                    {
                        if (!Utils.Validation.IsValidEmail(s.Trim()))
                            invalid.Add(s.Trim());
                        else
                            valid.Add(s.Trim());
                    }
                }

                //do not send partial lists - return here
                if (invalid.Count > 0)
                {
                    msg += string.Format("The following emails are invalid:<br/>");
                    foreach (string s in invalid)
                        msg += string.Format("{0}<br />", s);

                    return msg;
                }

                //send an email
                //test emails should be processed immediately and should have highest priority
                MailQueue.SendSubscriptionEmailToList(Atx.CurrentSubscriptionEmailId, valid, sendDate, priority);

                return msg;
            }

            return "Subscription email not found.";
        }
}
}