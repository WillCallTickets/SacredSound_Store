using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using SubSonic;

using System.Net.Mail;
using Utils.ExtensionMethods;

namespace Wcss
{
    public partial class MailQueue
    {
        #region Properties

        [XmlAttribute("IsMassMailer")]
        public bool IsMassMailer
        {
            get { return (!this.BMassMailer.HasValue) ? false : this.BMassMailer.Value; }
            set { this.BMassMailer = value; }
        }

        #endregion

        public static void LogSent_Subscription(string cc, string bcc, DateTime toProcess, DateTime processed,
            string fromAddress, string fromName, string toAddress, string status, bool massMailer,
            int priority, int subEmailId)
        {
            LogSentMail(cc, bcc, toProcess, processed, fromAddress, fromName, toAddress, status, massMailer,
                priority, subEmailId, 0);
        }
        public static void LogSent_EmailLetter(string cc, string bcc, DateTime toProcess, DateTime processed,
            string fromAddress, string fromName, string toAddress, string status, bool massMailer,
            int priority, int emailLetterId)
        {
            LogSentMail(cc, bcc, toProcess, processed, fromAddress, fromName, toAddress, status, massMailer,
                priority, 0, emailLetterId);
        }
        public static void LogSent(string cc, string bcc, DateTime toProcess, DateTime processed,
            string fromAddress, string fromName, string toAddress, string status, bool massMailer,
            int priority)
        {
            LogSentMail(cc, bcc, toProcess, processed, fromAddress, fromName, toAddress, status, massMailer,
                priority, 0, 0);
        }
        private static void LogSentMail(string cc, string bcc, DateTime toProcess, DateTime processed,
            string fromAddress, string fromName, string toAddress, string status, bool massMailer, 
            int priority, int subEmailId, int emailLetterId)
        {
            MailQueue q = new MailQueue();
            q.ApplicationId = _Config.APPLICATION_ID;
            q.DtStamp = DateTime.Now;
            q.AttemptsRemaining = 0;
            q.Bcc = bcc;
            q.Cc = cc;
            q.DateProcessed = processed;
            q.DateToProcess = toProcess;
            q.FromAddress = fromAddress;
            q.FromName = fromName;
            q.IsMassMailer = massMailer;
            q.Priority = priority;
            q.Status = status;
            q.ToAddress = toAddress;

            if(subEmailId > 0)
                q.TSubscriptionEmailId = subEmailId;
            if(emailLetterId > 0)
                q.TEmailLetterId = emailLetterId;

            q.Save();
        }

        private static void SetSubscriptionEmailToListCommandParams(SubSonic.QueryCommand cmd, EmailLetter ltr, SubscriptionEmail subEmail,
            DateTime dateToSend, int priority, bool isMassEmail)
        {
            cmd.Parameters.Clear();
            cmd.Parameters.Add("@appId", _Config.APPLICATION_ID, System.Data.DbType.Guid);
            cmd.Parameters.Add("@letterId", ltr.Id, System.Data.DbType.Int32);
            cmd.Parameters.Add("@subscriptionEmailId", subEmail.Id, System.Data.DbType.Int32);
            cmd.Parameters.Add("@dateToSend", dateToSend, System.Data.DbType.DateTime);
            cmd.Parameters.Add("@fromName", (isMassEmail) ? _Config._MassMailService_FromName : _Config._CustomerService_FromName);
            cmd.Parameters.Add("@fromAddress", (isMassEmail) ? _Config._MassMailService_Email : _Config._CustomerService_Email);
            cmd.Parameters.Add("@priority", priority, System.Data.DbType.Int32);
            cmd.Parameters.Add("@mass", isMassEmail, System.Data.DbType.Boolean);
        }

        /// <summary>
        /// TODO: This is a wee bit convoluted - work on this to make the loop more logical
        /// Sends the email to the given list and records a history object for subscription email
        /// Uses the SetSubscriptionEmailToListCommandParams() method within the loop to reset command vars. This is done to keep the command size limited.
        /// Subscriptions are inherintly massemails
        /// </summary>
        /// <param name="subscriptionEmailId"></param>
        /// <param name="validatedEmail"></param>
        /// <param name="dateToSend"></param>
        /// <param name="priority">0 is quickest</param>
        public static void SendSubscriptionEmailToList(int subscriptionEmailId, List<string> validatedEmail,
            DateTime dateToSend, int priority)
        {
            bool isMassEmail = true;

            SubscriptionEmail subEmail = SubscriptionEmail.FetchByID(subscriptionEmailId);

            if (subEmail != null && subEmail.SubscriptionRecord.ApplicationId == _Config.APPLICATION_ID)
            {
                EmailLetter ltr = subEmail.EmailLetterRecord;

                StringBuilder sb = new StringBuilder();
                SubSonic.QueryCommand cmd = new QueryCommand(string.Empty, SubSonic.DataService.Provider.Name);
                SetSubscriptionEmailToListCommandParams(cmd, ltr, subEmail, dateToSend, priority, isMassEmail);

                int i = 0;

                foreach (string emailAddress in validatedEmail)
                {
                    //dont do too many at a time                    
                    string emailParamName = string.Format("@toAddress_{0}", i++);//increment i
                    cmd.Parameters.Add(emailParamName, emailAddress);

                    sb.Append("INSERT MailQueue ([ApplicationId], [TEmailLetterId], [TSubscriptionEmailId], [DateToProcess], [FromName], [FromAddress], [Priority], [bMassMailer], ");
                    sb.Append("[ToAddress]) ");
                    sb.Append("VALUES (@appId, @letterId, @subscriptionEmailId, @dateToSend, @fromName, @fromAddress, @priority, @mass, ");
                    sb.AppendFormat("{0} )", emailParamName);

                    //do one hundred at a time
                    if (i % 100 == 0)
                    {
                        cmd.CommandSql = sb.ToString();

                        try
                        {
                            SubSonic.DataService.ExecuteQuery(cmd);
                        }
                        catch (Exception ex)
                        {
                            _Error.LogException(ex, true);
                            throw ex;
                        }

                        //reset command params, string and counter
                        SetSubscriptionEmailToListCommandParams(cmd, ltr, subEmail, dateToSend, priority, isMassEmail);
                        sb.Length = 0;
                        i = 0;
                    }
                }

                //do the leftovers
                if (sb.Length > 0)
                {
                    cmd.CommandSql = sb.ToString();

                    try
                    {
                        SubSonic.DataService.ExecuteQuery(cmd);
                    }
                    catch (Exception ex)
                    {
                        _Error.LogException(ex, true);
                        throw ex;
                    }
                }

                //create a history event for the subemail
                HistorySubscriptionEmail.Insert(DateTime.Now, subscriptionEmailId, dateToSend, validatedEmail.Count);
            }
        }

        /// <summary>
        /// sends 2 emails - 1 to the old and one to the new
        /// </summary>
        /// <param name="oldName"></param>
        /// <param name="newName"></param>
        public static void SendMailerSignupNotification(string email, string userId, DateTime requestDate, string originDescription, Subscription sub)
        {
            //email signing up
            //userid
            //time of request
            //where the mail was sent from - site
            System.Collections.Specialized.ListDictionary dict = new System.Collections.Specialized.ListDictionary();

            dict.Add("<% Email %>", email);
            dict.Add("<% UserId %>", userId);
            dict.Add("<% SubscriptionName %>", sub.Name);
            dict.Add("<% RequestDate %>", requestDate.ToString("MM/dd/yyyy hh:mmtt"));
            dict.Add("<% OriginDescription %>", originDescription);
            dict.Add("<% SiteEntityName %>", _Config._Site_Entity_Name);
            dict.Add("<% DomainName %>", _Config._DomainName);

            string templateName = "MailerSignupNotification.txt";
            string template = string.Format("/{0}/MailTemplates/SiteTemplates/{1}", _Config._VirtualResourceDir, templateName);
            string mappedFile = (System.Web.HttpContext.Current != null) ?
                System.Web.HttpContext.Current.Server.MapPath(template) :
                string.Format("{0}\\WcWeb{1}", _Config._MappedRootDirectory, template);

            //we do the replacements here because this will be sent as text
            string body = Utils.FileLoader.FileToString(mappedFile);
            body = Utils.ParseHelper.DoReplacements(body, dict, false);//dont do html replacements in this case

            MailQueue.SendEmail(_Config._CustomerService_Email, _Config._CustomerService_FromName,
                (_Config._RefundTestMode) ? _Config._Admin_EmailAddress : email, null, null,
                "Mailing List Confirmation Request", null, body, null, false, templateName);
        }

        /// <summary>
        /// This should be wrapped in the callers
        /// </summary>
        /// <param name="fromEmail"></param>
        /// <param name="fromName"></param>
        /// <param name="toName"></param>
        /// <param name="bcc"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="replacements"></param>
        /// <param name="htmlEmail"></param>
        public static void SendEmail(string fromEmail, string fromName, string toEmail, string cc, string bcc, string subject, 
            string body, string textVersion, System.Collections.Specialized.ListDictionary replacements, bool htmlEmail, 
            string emailLetterName)
        {
            MailMessage mail = new MailMessage();

            try
            {
                if (textVersion != null && textVersion.Length > 0)
                {
                    AlternateView plainView = AlternateView
                                .CreateAlternateViewFromString(textVersion, new System.Net.Mime.ContentType("text/plain"));

                    // if this is not set, it passes it as base64
                    plainView.TransferEncoding = System.Net.Mime.TransferEncoding.SevenBit;

                    mail.AlternateViews.Add(plainView);
                }

                if (body != null && body.Length > 0)
                {
                    if (replacements != null && replacements.Count > 0)
                        body = Utils.ParseHelper.DoReplacements(body, replacements, htmlEmail);

                    AlternateView htmlView = AlternateView.CreateAlternateViewFromString(body,
                            new System.Net.Mime.ContentType("text/html"));

                    // if this is not set, it passes it as base64
                    htmlView.TransferEncoding = System.Net.Mime.TransferEncoding.QuotedPrintable;

                    mail.AlternateViews.Add(htmlView);
                }

                mail.From = new MailAddress(fromEmail, fromName);
                mail.To.Add(new MailAddress(toEmail));
                if (cc != null && cc.Trim().Length > 0)
                    mail.CC.Add(new MailAddress(cc));
                if (bcc != null && bcc.Trim().Length > 0)
                    mail.Bcc.Add(new MailAddress(bcc));
                mail.Subject = subject;

                SmtpClient client = new SmtpClient();
                client.Send(mail);

                //if the email derives its content from an EmailLetter object then log this action
                if (emailLetterName != null && emailLetterName.Trim().Length > 0)
                {
                    QueryCommand cmd = new QueryCommand("SELECT Id FROM EmailLetter WHERE [ApplicationId] = @appId AND [Name] = @name ", 
                        SubSonic.DataService.Provider.Name);
                    cmd.Parameters.Add("@appId", _Config.APPLICATION_ID, System.Data.DbType.Guid);
                    cmd.Parameters.Add("@name", emailLetterName.Trim());

                    object obj = DataService.ExecuteScalar(cmd);

                    if (obj != null)
                    {
                        DateTime nowDate = DateTime.Now;
                        MailQueue mq = new MailQueue();
                        mq.ApplicationId = _Config.APPLICATION_ID;
                        mq.AttemptsRemaining = 3;
                        mq.Bcc = bcc;
                        mq.IsMassMailer = false;
                        mq.Cc = cc;
                        mq.DateToProcess = nowDate;
                        mq.DateProcessed = nowDate;
                        mq.DtStamp = nowDate;
                        mq.TEmailLetterId = (int)obj;
                        mq.ToAddress = toEmail;
                        mq.FromAddress = fromEmail;
                        mq.FromName = fromName;
                        mq.Priority = 10;
                        mq.Status = "Sent";
                        mq.Save();
                    }
                }
            }
            catch (Exception ex)
            {
                _Error.LogException(ex, true);
            }
        }

        public static void SendExchangeInformation(Invoice invoice, List<System.Web.UI.WebControls.ListItem> exchangedItems)
        {
            string customerEmail = invoice.AspnetUserRecord.UserName;

            System.Collections.Specialized.ListDictionary dict = new System.Collections.Specialized.ListDictionary();
            StringBuilder sb = new StringBuilder();
            string body = string.Empty;

            dict.Add("<% Greeting %>", string.Format("CustomerId: {0}<br/><br/><b>Dear {1},</b>", invoice.UserId.ToString(), customerEmail));

            dict.Add("<% InvoiceInfo %>", string.Format("Invoice Id: {0} Dated: {1}", invoice.UniqueId, invoice.InvoiceDate.ToString("ddd MM/dd/yyyy hh:mmtt")));
            dict.Add("<% DateOfExchange %>", DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));

            sb.AppendFormat("<table border=\"0\" cellpadding=\"0\" cellspacing=\"3\">", Utils.Constants.NewLine);

            sb.AppendFormat("<tr><th align=\"left\">Original Purchase</th><td rowspan=\"99\">&nbsp;&nbsp;&nbsp;</td><th align=\"left\">Exchanged For</th></tr>", Utils.Constants.NewLine, Utils.Constants.Tab);

            foreach (System.Web.UI.WebControls.ListItem li in exchangedItems)
            {
                string original = li.Text;
                string exchange = li.Value.TrimEnd('~');

                sb.AppendFormat("<tr><td valign=\"top\">{2}</td><td>{3}</td></tr>", Utils.Constants.NewLine, Utils.Constants.Tab,
                    original, exchange);
            }

            sb.AppendFormat("</table>", Utils.Constants.NewLine);

            dict.Add("<% ItemsExchanged %>", sb.ToString());

            string templateName = "CustomerExchange.html";
            string template = string.Format("/{0}/MailTemplates/SiteTemplates/{1}", _Config._VirtualResourceDir, templateName);
            string mappedFile = (System.Web.HttpContext.Current != null) ?
                System.Web.HttpContext.Current.Server.MapPath(template) :
                string.Format("{0}\\WcWeb{1}", _Config._MappedRootDirectory, template);

            body = Utils.FileLoader.FileToString(mappedFile);
            body = Utils.ParseHelper.DoReplacements(body, dict, false);//if this works (true) see if we are doing refund email corretly!!!!!

            MailQueue.SendEmail(_Config._CustomerService_Email, _Config._CustomerService_FromName,
                (_Config._RefundTestMode) ? _Config._Admin_EmailAddress : customerEmail, null, 
                _Config._CustomerService_Email, "Your Exchange Information", body, null, null, true, templateName);
        }
        public static void SendRefundInformation(Invoice invoice, List<System.Web.UI.WebControls.ListItem> refundedItems,
            decimal creditRefund, decimal authNetRefund, decimal discountAmount, string discountDescription, bool processLocallyOnly, string emailLink)
        {
            string customerEmail = invoice.AspnetUserRecord.UserName;

            System.Collections.Specialized.ListDictionary dict = new System.Collections.Specialized.ListDictionary();
            StringBuilder sb = new StringBuilder();
            string body = string.Empty;

            dict.Add("<% Greeting %>", string.Format("CustomerId: {0}<br/><br/><b>Dear {1},</b>", invoice.UserId.ToString(), customerEmail));

            dict.Add("<% InvoiceInfo %>", string.Format("for Invoice Id: {0} Dated: {1}", invoice.UniqueId, invoice.InvoiceDate.ToString("ddd MM/dd/yyyy hh:mmtt")));

            sb.AppendFormat("<table border=\"0\" cellpadding=\"0\" cellspacing=\"3\">", Utils.Constants.NewLine);

            if (emailLink != null && emailLink.Trim().Length > 0)
            {
                sb.AppendFormat("<tr><th style=\"text-align:left\" colspan=\"99\">{0}<br/><br/></th></tr>", emailLink);
            }

            sb.AppendFormat("<tr><th>Amount</th><td rowspan=\"99\">&nbsp;&nbsp;&nbsp;</td><th align=\"left\">Description</th></tr>", Utils.Constants.NewLine, Utils.Constants.Tab);

            foreach (System.Web.UI.WebControls.ListItem li in refundedItems)
            {
                decimal amt = decimal.Round(decimal.Parse(li.Value), 2);

                try
                {
                    int qty = int.Parse(li.Text.Substring(0, li.Text.IndexOf("@")).Trim());

                    decimal perItemPrice = (amt > 0) ? (decimal)(amt / qty) : 0;
                    sb.AppendFormat("<tr><td valign=\"top\">{2}</td><td>{3}</td></tr>", Utils.Constants.NewLine, Utils.Constants.Tab,
                        (perItemPrice > 0) ? perItemPrice.ToString("c") : "&nbsp;", li.Text);
                }
                catch (Exception ex)
                {
                    _Error.LogException(ex);

                    //write al alternative line
                    sb.AppendFormat("<tr><td valign=\"top\">{2}</td><td>{3}</td></tr>", Utils.Constants.NewLine, Utils.Constants.Tab,
                        (amt > 0) ? amt.ToString("c") : "&nbsp;", li.Text);
                }
            }

            if(discountAmount > 0)
                sb.AppendFormat("<tr><td valign=\"top\">{2}</td><td>DISCOUNT APPLIED: {3}</td></tr>", Utils.Constants.NewLine, Utils.Constants.Tab,
                    discountAmount.ToString("c"), discountDescription);

            //todo: setup system for store credit
            //todo: specify address that the check was sent to - include check #?
            //let them know we have sent a check....
            if (processLocallyOnly)
            {
                sb.Append("<tr><td colspan=\"99\"><br /><p>A check will be issued for your refund.</p></td></tr>");
            }
            else if (authNetRefund > 0 && _Config._Merchant_ChargeStatement_Descriptor.HasValueLength())
                    sb.AppendFormat("<tr><td colspan=\"99\"><br /><p>*Note: You will see a credit from {0} on your statement.</p></td></tr>",
                        Wcss._Config._Merchant_ChargeStatement_Descriptor);

            sb.AppendFormat("</table>", Utils.Constants.NewLine);

            dict.Add("<% ItemsRefunded %>", sb.ToString());

            dict.Add("<% StoreCreditRefund %>", ((!processLocallyOnly) && creditRefund > 0) ? 
                string.Format("<p><b>===Store Credits Issued</b><br/>{0}</p>", creditRefund.ToString("n2")) : string.Empty);
            dict.Add("<% CreditCardRefund %>", (authNetRefund > 0) ? 
                string.Format("<p><b>===Credit Card Refund</b><br/>{0}</p>", authNetRefund.ToString("n2")) : string.Empty);
            dict.Add("<% TotalRefund %>", ((decimal)(creditRefund + authNetRefund)).ToString("c"));

            string templateName = "CustomerRefund.html";
            string template = string.Format("/{0}/MailTemplates/SiteTemplates/{1}",_Config._VirtualResourceDir, templateName);
            string mappedFile = (System.Web.HttpContext.Current != null) ?
                System.Web.HttpContext.Current.Server.MapPath(template) :
                string.Format("{0}\\WcWeb{1}", _Config._MappedRootDirectory, template);

            body = Utils.FileLoader.FileToString(mappedFile);
            body = Utils.ParseHelper.DoReplacements(body, dict, true);//dont do replacements in this case

            MailQueue.SendEmail(_Config._CustomerService_Email, _Config._CustomerService_FromName,
                (_Config._RefundTestMode) ? _Config._Admin_EmailAddress : customerEmail, null,
                _Config._CustomerService_Email, "Your Refund Information", body, null, null, true, templateName);
        }

        public static void SendForgotPass(string email, string pass)
        {
            System.Collections.Specialized.ListDictionary dict = new System.Collections.Specialized.ListDictionary();

            dict.Add("<% UserPass %>", pass);

            string templateName = "CustomerForgotPassword.txt";
            string template = string.Format("/{0}/MailTemplates/SiteTemplates/{1}", _Config._VirtualResourceDir, templateName);
            string mappedFile = (System.Web.HttpContext.Current != null) ?
                System.Web.HttpContext.Current.Server.MapPath(template) :
                string.Format("{0}\\WcWeb{1}", _Config._MappedRootDirectory, template);

            string body = Utils.FileLoader.FileToString(mappedFile);
            body = Utils.ParseHelper.DoReplacements(body, dict, false);//dont do html replacements in this case

            //send info to old email - customer service is cc'd on this one
            MailQueue.SendEmail(_Config._CustomerService_Email, _Config._CustomerService_FromName,
                (_Config._RefundTestMode) ? _Config._Admin_EmailAddress : email, null, null,                
                "Your Requested Information", null, body, null, false, templateName);
        }

        public static void SendReissuedCode(string toAddress, string fromAddress, string toName, string fromName, string reissuedCode, InvoiceItem item)
        {
            StringBuilder tbl = new StringBuilder();
            StringBuilder txt = new StringBuilder();

            if (toName == null || toName.Trim().Length == 0)
                toName = toAddress;
            if (fromName == null || fromName.Trim().Length == 0)
                fromName = fromAddress;

            tbl.AppendLine("<table border=\"0\" cellpadding=\"0\" cellspacing=\"6\" class=\"gift\">");
            tbl.AppendLine("<tr><td rowspan=\"99\">&nbsp;&nbsp;</td><td colspan=\"2\">&nbsp;</td></tr>");
            tbl.AppendFormat("<tr><td><img src=\"http://{0}/WillCallResources/Images/UI/{1}\" alt=\"{2}\" /></td>",
                Wcss._Config._DomainName, Wcss._Config._GiftLogo, Wcss._Config._Site_Entity_Name);
            tbl.AppendLine();
            tbl.AppendLine("<td><div style=\"font-weight: bold; font-size: 36px;\">Your new code</div></td></tr>");
            tbl.AppendLine("<tr><td colspan=\"2\">&nbsp;</td></tr>");
            tbl.AppendFormat("<tr><td>New code</td><td>{0}</td></tr>", item.DeliveryCode);
            tbl.AppendLine();
            tbl.AppendLine("<tr><td colspan=\"2\">&nbsp;</td></tr>");
            tbl.AppendFormat("<tr><td>Item reissued for</td><td>{0}</td></tr>", item.MainActName);
            tbl.AppendLine();
            tbl.AppendFormat("<tr><td>Item purchased on</td><td>{0}</td></tr>", item.DtStamp.ToString("MM/dd/yyyy"));
            tbl.AppendLine();
            tbl.AppendLine("<tr><td colspan=\"2\">&nbsp;</td></tr>");


            txt.AppendLine("Your new code");
            txt.AppendLine();
            txt.AppendLine();
            txt.AppendLine();
            txt.AppendFormat("New code: {0}", item.DeliveryCode);
            txt.AppendLine();
            txt.AppendLine();
            txt.AppendFormat("Item reissued for: {0}", item.MainActName);
            txt.AppendLine();
            txt.AppendLine();
            txt.AppendFormat("Item purchased on: {0}", item.DtStamp.ToString("MM/dd/yyyy"));
            txt.AppendLine();
            txt.AppendLine();
            

            foreach (InvoiceItemPostPurchaseText t in item.InvoiceItemPostPurchaseTextRecords())
            {   
                tbl.AppendFormat("<tr><td colspan=\"2\">{0}</td></tr>", t.PostText);
                tbl.AppendLine("<tr><td colspan=\"2\">&nbsp;</td></tr>");

                txt.AppendFormat("Item reissued for: {0}", item.MainActName);
                txt.AppendLine();
            }
            
            
            tbl.AppendLine("</table><br />");
            txt.AppendLine();      

            
            //
            MailQueue.SendEmail(fromAddress, fromName, (_Config._RefundTestMode) ? _Config._Admin_EmailAddress : toAddress, null, null,
                "Your new code", tbl.ToString(), txt.ToString(), null, false, null);

            //log
            MailQueue.LogSent(null, null, DateTime.Now, DateTime.Now, fromAddress, fromName, toAddress, "Sent", false, 10);
        }

        public static void SendGiftCertificate(string toAddress, string fromAddress, string toName, string fromName, string amount, string giftCode)
        {
            StringBuilder tbl = new StringBuilder();
            StringBuilder txt = new StringBuilder();

            if(toName == null || toName.Trim().Length == 0)
                toName = toAddress;
            if(fromName == null || fromName.Trim().Length == 0)
                fromName = fromAddress;

            tbl.AppendLine("<table border=\"0\" cellpadding=\"0\" cellspacing=\"6\" class=\"gift\">");
            tbl.AppendLine("<tr><td rowspan=\"99\">&nbsp;&nbsp;</td><td colspan=\"2\">&nbsp;</td></tr>");
            tbl.AppendFormat("<tr><td><img src=\"http://{0}/WillCallResources/Images/UI/{1}\" alt=\"{2}\" /></td>", 
                Wcss._Config._DomainName, Wcss._Config._GiftLogo, Wcss._Config._Site_Entity_Name);
            tbl.AppendLine();
            tbl.AppendLine("<td><div style=\"font-weight: bold; font-size: 36px;\">Gift Certificate</div></td></tr><tr><td colspan=\"2\">&nbsp;</td></tr>");
            tbl.AppendFormat("<tr><td>Amount</td><td>$ {0}</td></tr>", amount);
            tbl.AppendLine();
            tbl.AppendFormat("<tr><td>To</td><td>{0}</td></tr>", toName);
            tbl.AppendLine();
            tbl.AppendFormat("<tr><td>From</td><td>{0}</td></tr>", fromName);
            tbl.AppendLine();
            tbl.AppendFormat("<tr><td>Code</td><td>{0}</td></tr>", giftCode);
            tbl.AppendLine();
            tbl.AppendFormat("</table><br /><hr />{0}{1}", Wcss._Config._GiftRedemptionInstructions, Wcss._Config._GiftTerms);
        
            txt.AppendFormat("{0} Gift Certificate", _Config._Site_Entity_Name);
            txt.AppendLine();
            txt.AppendLine();
            txt.AppendFormat("Amount $ {0}", amount);
            txt.AppendLine();
            txt.AppendFormat("To {0}", toName);
            txt.AppendLine();
            txt.AppendFormat("From {0}", fromName);
            txt.AppendLine();
            txt.AppendFormat("Code {0}", giftCode);
            txt.AppendLine();
            txt.AppendLine();
            txt.AppendFormat("{0}", Utils.ParseHelper.XmlToText(Wcss._Config._GiftRedemptionInstructions));
            txt.AppendLine();
            txt.AppendLine();
            txt.AppendFormat("{0}", Utils.ParseHelper.XmlToText(Wcss._Config._GiftTerms));

            //
            MailQueue.SendEmail(fromAddress, fromName, (_Config._RefundTestMode) ? _Config._Admin_EmailAddress : toAddress, null, null,
                "A Gift For You", tbl.ToString(), txt.ToString(), null, false, null);

            //log
            MailQueue.LogSent(null, null, DateTime.Now, DateTime.Now, fromAddress, fromName, toAddress, "Sent", false, 10);
        }

        /// <summary>
        /// sends 2 emails - 1 to the old and one to the new
        /// </summary>
        /// <param name="oldName"></param>
        /// <param name="newName"></param>
        public static void SendUserChangeEmail(string oldName, string newName)
        {
            System.Collections.Specialized.ListDictionary dict = new System.Collections.Specialized.ListDictionary();

            dict.Add("<% OldName %>", oldName);
            dict.Add("<% NewName %>", newName);

            string templateName = "ChangeUserName.txt";
            string template = string.Format("/{0}/MailTemplates/SiteTemplates/{1}", _Config._VirtualResourceDir, templateName);
            string mappedFile = (System.Web.HttpContext.Current != null) ?
                System.Web.HttpContext.Current.Server.MapPath(template) :
                string.Format("{0}\\WcWeb{1}", _Config._MappedRootDirectory, template);

            string body = Utils.FileLoader.FileToString(mappedFile);
            body = Utils.ParseHelper.DoReplacements(body, dict, false);//dont do html replacements in this case

            //send info to old email - customer service is cc'd on this one
            MailQueue.SendEmail(_Config._CustomerService_Email, _Config._CustomerService_FromName,
                (_Config._RefundTestMode) ? _Config._Admin_EmailAddress : oldName, _Config._CustomerService_Email, null,
                "Your Information Has Changed", null, body, null, false, templateName);

            //send email to new address
            //**note only one copy needs to be sent to customer service - don't cc here
            MailQueue.SendEmail(_Config._CustomerService_Email, _Config._CustomerService_FromName,
                (_Config._RefundTestMode) ? _Config._Admin_EmailAddress : newName, null,
                null, "Your Information Has Changed", null, body, null, false, templateName);

        }

        private static void SendEmailTemplate(string templateName, DateTime sendTime, string fromName, string fromAddress,
            string toAddress, string paramNames, string paramValues)
        {
            SendEmailTemplate(templateName, sendTime, fromName, fromAddress, toAddress, paramNames, paramValues, string.Empty, 1);
        }
        private static void SendEmailTemplate(string templateName, DateTime sendTime, string fromName, string fromAddress,
            string toAddress, string paramNames, string paramValues, string bccEmail, int priority)
        {
            if (!Utils.Validation.IsValidEmail(toAddress))
                throw new Exception(string.Format("{0} is not a valid email address.", toAddress));

            string result = string.Empty;

            StoredProcedure proc = Wcss.SPs.TxSendEmailTemplate(_Config.APPLICATION_ID, templateName, Utils.Helper.Parse_DateForTableInsert(sendTime),
                fromName, fromAddress, toAddress, paramNames, paramValues, bccEmail, priority, result);

            proc.Execute();
            result = proc.OutputValues[0].ToString();

            if (!result.ToLower().Equals("success"))
                throw new Exception(string.Format("{0}", result));
        }

    }
}
