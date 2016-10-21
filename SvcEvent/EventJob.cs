using System;
using System.IO;
using System.Net.Mail;

using Jobs;
using Wcss;

namespace SvcEvent
{
	/// <summary>
	/// runs a multithreadable Job
	/// </summary>
	public class EventJob : Jobs.Job
	{		
		/// <summary>
		/// provides mails to process
		/// </summary>
		private EventData eventData = null;

		/// <summary>
		/// Only constructor
		/// </summary>
		public EventJob() : base()
		{
            if (Wcss._Config._ErrorsToDebugger)
			    System.Diagnostics.Debug.WriteLine("Starting Job...", "EventJob");

            eventData = new EventData(this.JobId);
		}

		/// <summary>
		/// Entry point
        /// see http://noggle.com/?p=23 
		/// </summary>
		/// <returns></returns>
		public override bool DoJob()
		{
            if (!_Config.SqlServerIsAvailable())
            {
                _Error.LogToFile("Sql Server not available.", string.Format("{0}{1}", _Config._ErrorLogTitle, DateTime.Now.ToString("MM_dd_yyyy")));

                System.Threading.AutoResetEvent waitingEvent = new System.Threading.AutoResetEvent(false);
                waitingEvent.WaitOne(28800000, true);//every 8 minutes - 8 * 60 * 60 * 1000 = 28800000

                return true;
            }

			EventQ q = null;

            try
            {
                //keep this for testing
                //throw new Exception("find the dir");

                q = eventData.GetNextEvent();

                if (q == null) return true;

                //Process row by verb
                if (ProcessEvent(q))
                {
                    //update mq row
                    q.Status = "Success";
                    q.DateProcessed = DateTime.Now;
                    q.ThreadLock = null;
                    q.Save();
                }

                System.Diagnostics.Debug.WriteLine(string.Format("Event ({0}) has been processed", q.Verb));
            }
            catch (Exception e)
            {
                if (Wcss._Config._ErrorsToDebugger)
                {
                    System.Diagnostics.Debug.WriteLine("Event service failure...");
                    System.Diagnostics.Debug.WriteLine(e.Message);
                    System.Diagnostics.Debug.WriteLine(e.StackTrace);
                }

                if (q != null)
                {
                    q.Status = e.Message + "\n" + e.StackTrace;
                    if (q.Status.Length > 2000)
                        q.Status = q.Status.Substring(0, 1995);

                    q.DateProcessed = DateTime.Now;
                    q.ThreadLock = null;
                    q.AttemptsRemaining -= 1;
                    q.Save();
                }

                Wcss._Error.LogException(e);
            }
						
			return true;
		}

        public void SaveToFile(string mappedPathAndName, string body)
        {
            FileStream fs = null;
            StreamWriter sw = null;

            try
            {
                fs = new FileStream(mappedPathAndName, FileMode.Append, FileAccess.Write);
                sw = new StreamWriter(fs);

                sw.Write(body);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (sw != null) sw.Close();
            if (fs != null) fs.Close();

        }

        public bool ProcessEvent(EventQ q)
        {
            _Enums.EventQContext context = (_Enums.EventQContext)Enum.Parse(typeof(_Enums.EventQContext), q.Context, true);
            _Enums.EventQVerb verb = (_Enums.EventQVerb)Enum.Parse(typeof(_Enums.EventQVerb), q.Verb, true);

            #region Context Merch
            
            switch (context)
            {
                case _Enums.EventQContext.AdminNotification:
                    //EventQ.CreateAdminNotification(DateTime.Now, "ClearCart", username, _Enums.EventQVerb.CartCleared,
                    //"Error", 
                    //(postSale_IncrementSales) ? "Post Sale" : "Not Post Sale", 
                    //retVal);
                    //public static EventQ CreateAdminNotification(DateTime dateToProcess, string processGoneBad, string affectedUserName, _Enums.EventQVerb verb, 
                    //string oldValue,
                    //string newValue, 
                    //string description)
                    string details = string.Format("{1}: {2}{0}{3}: {4}", Environment.NewLine, q.CreatorName, q.OldValue, 
                        q.NewValue, q.Description);
                    MailQueue.SendEmail(_Config.svc_ServiceEmail, q.CreatorName, _Config._CC_DeveloperEmail, null, null, details, details, null, null, true, null);
                    break;

                case _Enums.EventQContext.Merch:
                    {
                        switch (verb)
                        {
                            case _Enums.EventQVerb.Publish:
                                {
                                    //rewrite dependency files
                                    SaveToFile(string.Format(@"{0}\DependencyFiles\MerchDepend.txt",
                                        _Config.svc_MappedVirtualResourceDirectory),
                                        string.Format("{0}\r\n", DateTime.Now.ToString()));

                                    //find cache items to refresh
                                    return true;
                                }
                            case _Enums.EventQVerb.Merch_SalePriceChange:
                                {
                                    Merch entity = Merch.FetchByID(int.Parse(q.OldValue));

                                    if (entity != null)
                                    {
                                        decimal oldPrice = entity.Price_Effective;
                                        decimal newPrice = decimal.Parse(q.NewValue);

                                        //update merch to flagSalePrice and set sale price
                                        entity.UseSalePrice = true;
                                        entity.SalePrice = newPrice;
                                        entity.Save();

                                        //we need to update the children here as well
                                        foreach (Merch child in entity.ChildMerchRecords())
                                        {
                                            child.UseSalePrice = entity.UseSalePrice;
                                            child.SalePrice = entity.SalePrice;
                                            child.Save_AvoidRealTimeVars();
                                        }

                                        HistoryPricing hist = new HistoryPricing();
                                        hist.DtStamp = DateTime.Now;
                                        hist.UserId = null;
                                        hist.TMerchId = entity.Id;
                                        hist.DateAdjusted = DateTime.Now;
                                        hist.OldPrice = oldPrice;
                                        hist.NewPrice = newPrice;
                                        hist.Context = _Enums.HistoryInventoryContext.ParentPrice;
                                        hist.Save();

                                        return true;
                                    }
                                }
                                break;

                            case _Enums.EventQVerb.InventoryError:
                                MailQueue.SendEmail(_Config.svc_ServiceEmail, "Inventory Error Notifier", _Config._CC_DeveloperEmail, null, null,
                                    verb.ToString(), string.Format("{0}<br/>Was: {1}<br/>Now: {2}", q.Description, q.OldValue, q.NewValue), null, null, true, null);
                                break;

                                return true;
                        }

                        return true;
                    }

                
            }
            #endregion

            switch (verb)
            {
                //case _Enums.EventQVerb.UserPointsActivity:

                //    _Enums.UserPointAction action = (_Enums.UserPointAction)Enum.Parse(typeof(_Enums.UserPointAction), q.OldValue, true);
                //    string[] parts = q.Description.Split('~');
                //    string code = string.Empty;
                //    string promoId = string.Empty;
                //    string invoiceId = string.Empty;
                //    string notes = null;
                //    if (parts.Length >= 3)
                //    {
                //        code = parts[0];
                //        promoId = parts[1];
                //        invoiceId = parts[2];
                //        if (parts.Length > 3)
                //        {
                //            notes = string.Join("~", parts, 3, parts.Length - 3);
                //            if (notes.Length > 500)
                //                notes = notes.Substring(0, 499);
                //        }
                //    }

                //    UserPoint up = UserPoint.RecordUserPoints(code, (q.UserId != null) ? q.UserId.ToString() : null, q.UserName,
                //        (q.CreatorId != null) ? q.CreatorId.ToString() : null, q.CreatorName,
                //        _Enums.ReferenceSources.Invoice, invoiceId, int.Parse(q.NewValue), action, notes, false);

                //    return (up != null);
                    
                case _Enums.EventQVerb.Mailer_FailureNotification:
                    //send an email to the admin to tell him that there are mailer issues
                    MailQueue.SendEmail(_Config.svc_ServiceEmail, "Badmail Service", _Config._CC_DeveloperEmail, null, null, q.NewValue, q.Description, null, null, true, null);
                    break;

                case _Enums.EventQVerb.InventoryError:
                    //if (context == _Enums.EventQContext.ShowTicket)
                        MailQueue.SendEmail(_Config.svc_ServiceEmail, "Inventory Error Notifier",  _Config._CC_DeveloperEmail, null, null, 
                            verb.ToString(), string.Format("{0}<br/>Was: {1}<br/>Now: {2}", q.Description, q.OldValue, q.NewValue), null, null, true, null);
                    break;

                #region MailerRemove
                case _Enums.EventQVerb.Mailer_Remove:
                    //find the user
                    if (q.UserId != null && q.UserId != Guid.Empty)
                    {
                        //find the users subscription and disable
                        string subId = q.OldValue;//this is the subscription email id
                        int refInt = 0;
                        if (int.TryParse(subId, out refInt))
                        {
                            //from the sub email - get the subscription
                            SubscriptionEmail subEmail = SubscriptionEmail.FetchByID(int.Parse(subId));
                            //retrieve the subscription user
                            if (subEmail != null)
                            {
                                //tie them together and mark as removed
                                SubSonic.QueryCommand cmd = new SubSonic.QueryCommand("UPDATE [SubscriptionUser] SET [bSubscribed] = 0, [dtLastActionDate] = (getDate()) WHERE [UserId] = @userId AND [TSubscriptionId] = @subId", 
                                    SubSonic.DataService.Provider.Name);
                                cmd.Parameters.Add("@userId", q.UserId, System.Data.DbType.Guid);
                                cmd.Parameters.Add("@subId", subEmail.TSubscriptionId, System.Data.DbType.Int32);

                                int res = SubSonic.DataService.ExecuteQuery(cmd);
                                if (res > 0)
                                    return true;
                            }
                        }
                        else
                        {
                            string err = string.Format("Email Id not valid: {0} in Mailer_Remove event. EventQ Id: {1}", subId, q.Id);

                            _Error.LogException(new Exception(err));
                        }
                    }

                    throw new Exception("User or subscription not found");

                    //if the user is not an admin
                    //change subscription status

                #endregion

                #region Inventory Notification
                case _Enums.EventQVerb.InventoryNotification:
                    
                    // Get subscription - oldValue sets the context to ticket or merch
                    // this requires 2 subscriptions to exist - ensure they exist
                    // InventoryNotification_Merch, InventoryNotification_Ticket
                    
                    // parse subscription name from q values
                    string subscriptionName = string.Format("{0}_{1}", _Enums.EventQVerb.InventoryNotification.ToString(), q.OldValue);

                    Subscription subs = Subscription.GetSubscriptionByName(subscriptionName);
                    
                    //create the subscription if it does not exist
                    if (subs.ApplicationId.ToString() == Guid.Empty.ToString())
                        subs = Subscription.CreateNewSubscription(subscriptionName);

                    if (subs.ApplicationId.ToString() != Guid.Empty.ToString())
                    {
                        //get subscribers
                        SubscriptionUserCollection coll = new SubscriptionUserCollection();
                        coll.AddRange(subs.SubscriptionUserRecords().GetList().FindAll(delegate(SubscriptionUser match)
                            { return (match.TSubscriptionId == subs.Id && match.IsSubscribed); }));

                        foreach (SubscriptionUser usr in coll)
                        {
                            MailMessage mail = new MailMessage();

                            try
                            {
                                AlternateView plainView = AlternateView
                                    .CreateAlternateViewFromString(q.Description, new System.Net.Mime.ContentType("text/plain"));
                                // if this is not set, it passes it as base64
                                plainView.TransferEncoding = System.Net.Mime.TransferEncoding.SevenBit;
                                mail.AlternateViews.Add(plainView);

                                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(q.Description,
                                    new System.Net.Mime.ContentType("text/html"));
                                // if this is not set, it passes it as base64
                                htmlView.TransferEncoding = System.Net.Mime.TransferEncoding.QuotedPrintable;
                                mail.AlternateViews.Add(htmlView);

                                mail.From = new MailAddress(_Config.svc_ServiceEmail, _Config.svc_ServiceFromName);
                                mail.To.Add(new MailAddress(usr.AspnetUserRecord.UserName));

                                mail.Subject = q.Description;

                                SmtpClient client = new SmtpClient();
                                client.Send(mail);

                                //log to mailqueue
                                MailQueue.LogSent_Subscription(null, null, DateTime.Now, DateTime.Now, _Config.svc_ServiceEmail, _Config.svc_ServiceFromName,
                                    usr.AspnetUserRecord.UserName, _Enums.EventQStatus.Success.ToString(), false, 10, subs.Id);
                            }
                            catch (Exception ex)
                            {
                                _Error.LogException(ex, true);

                                return false;
                            }
                        }

                        return true;
                    }

                    break;
                #endregion

                #region ReportMailerDaily
                case _Enums.EventQVerb.Report_Mailer_Daily:

                    //if we have a daily report request - get the date to run
                    DateTime reportDate = DateTime.Parse(q.OldValue);

                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    System.Text.StringBuilder txt = new System.Text.StringBuilder();
                    sb.AppendFormat("<div style=\"font-weight: bold; margin: 1em;\">Sales report for {0}</div>", reportDate.ToString("yyyy/MM/dd"));
                    sb.Append("<div><table border=\"0\" cellspacing=\"3\" cellpadding=\"0\" width=\"100%\" style=\"font-family: Monospace; font-size: 1em;\">");
                    sb.Append("<tr><th style=\"text-align: left;\">Desc</th><th>Sold</th><th>Ttl</th><th>Avail</th></tr>");


                    //run yesterday's sales data
                    using (System.Data.IDataReader dr = SPs.TxReportDailySalesInfo(_Config.APPLICATION_ID, reportDate, true, true).GetReader())
                    {
                        while (dr.Read())
                        {
                            string ctx = dr.GetString(dr.GetOrdinal("vcContext"));
                            int idx = dr.GetInt32(dr.GetOrdinal("ItemId"));
                            string desc = dr.GetString(dr.GetOrdinal("Description"));
                            string mini = dr.GetString(dr.GetOrdinal("MiniDesc"));
                            string crit = dr.GetString(dr.GetOrdinal("CriteriaText"));
                            int alot = dr.GetInt32(dr.GetOrdinal("Alloted"));
                            int sold = dr.GetInt32(dr.GetOrdinal("Sold"));
                            int total = dr.GetInt32(dr.GetOrdinal("TotalSold"));
                            int avail = dr.GetInt32(dr.GetOrdinal("Available"));

                            crit = (crit.Length > 0 && crit.Length > 10) ? string.Format(" {0}", crit.Substring(0, 10)) : string.Empty;

                            sb.AppendFormat("<tr style=\"border-top: solid 1px #333333;\"><td>{0}{1}</td><td style=\"text-align: center;{5}\">{2}</td><td style=\"text-align: center;{5}\">{3}</td><td style=\"text-align: center;{5}\">{4}</td></tr>",
                                mini, (crit.Length > 0) ? string.Format("<br/>&nbsp;&nbsp;{0}", crit) : string.Empty, sold, total, avail,
                                (crit.Length > 0) ? "vertical-align: top;" : string.Empty);

                            txt.AppendFormat("{0}\ts({1})\tt({2})\ta({3})\r\n{4}", mini, sold, total, avail,
                                (crit.Length > 0) ? string.Format("\t{0}\r\n", crit) : string.Empty);
                        }

                        dr.Close();
                    }

                    sb.AppendFormat("</table></div>");


                    // Get subscription
                    Subscription sub = new Subscription();
                    sub.LoadAndCloseReader(Subscription.FetchByParameter("Name", "Sales_Daily"));

                    if (sub != null)
                    {
                        //get subscribers
                        SubscriptionUserCollection coll = new SubscriptionUserCollection();
                        coll.AddRange(sub.SubscriptionUserRecords().GetList().FindAll(delegate(SubscriptionUser match)
                            { return (match.SubscriptionRecord.Name == "Sales_Daily" && match.IsSubscribed); }));

                        foreach (SubscriptionUser usr in coll)
                        {
                            MailMessage mail = new MailMessage();

                            try
                            {
                                AlternateView plainView = AlternateView
                                    .CreateAlternateViewFromString(txt.ToString(), new System.Net.Mime.ContentType("text/plain"));
                                // if this is not set, it passes it as base64
                                plainView.TransferEncoding = System.Net.Mime.TransferEncoding.SevenBit;
                                mail.AlternateViews.Add(plainView);

                                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(sb.ToString(),
                                    new System.Net.Mime.ContentType("text/html"));
                                // if this is not set, it passes it as base64
                                htmlView.TransferEncoding = System.Net.Mime.TransferEncoding.QuotedPrintable;
                                mail.AlternateViews.Add(htmlView);

                                mail.From = new MailAddress(_Config.svc_ServiceEmail, _Config.svc_ServiceFromName);
                                mail.To.Add(new MailAddress(usr.AspnetUserRecord.UserName));

                                mail.Subject = string.Format("{0} {1}", sub.Name, DateTime.Now.AddDays(-1).Date.ToString("MM/dd/yyyy"));

                                SmtpClient client = new SmtpClient();
                                client.Send(mail);
                            }
                            catch (Exception ex)
                            {
                                _Error.LogException(ex, true);
                            }
                        }

                        return true;
                    }
                    else
                        throw new Exception("Subscription not found");

                #endregion

            }

            return false;
        }

        public override void CleanUp()
        {
            //TODO
            //eventData.CommitAll();
        }
	}
}
