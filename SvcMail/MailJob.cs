using System;
using System.Net.Mail;

using Jobs;
using Wcss;

namespace SvcMail
{
	/// <summary>
	/// runs a multithreadable Mail Job
	/// </summary>
	public class MailJob : Jobs.Job
	{		
		/// <summary>
		/// provides mails to process
		/// </summary>
		private MailData mailData = null;

		/// <summary>
		/// Only constructor
		/// </summary>
		public MailJob() : base()
		{
            //testing only!!!
            //_Error.LogToFile("Testing log", string.Format("{0}{1}", _Config._ErrorLogTitle, DateTime.Now.ToString("MM_dd_yyyy")));

            if (Wcss._Config._ErrorsToDebugger)
			    System.Diagnostics.Debug.WriteLine("Starting Job...", "MailJob");

            mailData = new MailData(this.JobId);
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

			MailQueue mq = null;

            try
            {
                //keep this for testing
               // throw new Exception("find the dir");

                mq = mailData.GetNextMail();

                if (mq == null) return true;

                MailMessage mail = new MailMessage();
                mail.Subject = mq.EmailLetterRecord.Subject;

                mail.From = new MailAddress(mq.FromAddress, mq.FromName);
                mail.To.Add(new MailAddress((_Config.svc_ServiceTestMode) ? _Config.svc_ServiceTestEmail : mq.ToAddress));

                //MassMailers need to be handled in a special manner
                //if it is a mass email - then it has a subscription
                if (mq.IsMassMailer)
                {
                    SubscriptionEmail subEmail = mq.SubscriptionEmailRecord;

                    if (mq.EmailLetterRecord.TextVersion != null && mq.EmailLetterRecord.TextVersion.Trim().Length > 0)
                    {
                        //only do this once - this gets set to null in sender
                        if (subEmail.ConstructedText == null || subEmail.ConstructedText.Trim().Length == 0)
                        {
                            string update = string.Format("UPDATE [SubscriptionEmail] SET [Constructed_Text] = @txt WHERE [Id] = @id; ");
                            SubSonic.QueryCommand construct = new SubSonic.QueryCommand(update, SubSonic.DataService.Provider.Name);
                            subEmail.ConstructedText = subEmail.CreateShell_Text(false, true);
                            construct.Parameters.Add("@txt", subEmail.ConstructedText);
                            construct.Parameters.Add("@id", subEmail.Id, System.Data.DbType.Int32);
                            SubSonic.DataService.ExecuteQuery(construct);
                        }

                        string plainText = subEmail.ConstructedText;

                        AlternateView plainView = AlternateView
                            .CreateAlternateViewFromString(plainText, new System.Net.Mime.ContentType("text/plain"));
                        
                        // if this is not set, it passes it as base64
                        plainView.TransferEncoding = System.Net.Mime.TransferEncoding.SevenBit;

                        mail.AlternateViews.Add(plainView);
                    }


                    //only do this once - this gets set to null in sender
                    if (subEmail.ConstructedHtml == null || subEmail.ConstructedHtml.Trim().Length == 0)
                    {
                        string update = string.Format("UPDATE [SubscriptionEmail] SET [Constructed_Html] = @html WHERE [Id] = @id; ");
                        SubSonic.QueryCommand construct = new SubSonic.QueryCommand(update, SubSonic.DataService.Provider.Name);
                        subEmail.ConstructedHtml = subEmail.CreateShell_Html(true, true, true, true);
                        construct.Parameters.Add("@html", subEmail.ConstructedHtml);
                        construct.Parameters.Add("@id", subEmail.Id, System.Data.DbType.Int32);
                        SubSonic.DataService.ExecuteQuery(construct);
                    }

                    //use full html specs if css is included
                    string emailer = subEmail.ConstructedHtml;//.ConstructEmailView();

                    AlternateView htmlView = AlternateView.CreateAlternateViewFromString(emailer,
                        new System.Net.Mime.ContentType("text/html"));

                    // if this is not set, it passes it as base64
                    htmlView.TransferEncoding = System.Net.Mime.TransferEncoding.QuotedPrintable;

                    mail.AlternateViews.Add(htmlView);
                    
                }
                else
                    mail.Body = mq.EmailLetterRecord.HtmlVersion;

                
                //TODO
                //RUN SUBS

                SmtpClient client = new SmtpClient();
                client.Send(mail);

                //update mq row
                mq.Status = "Sent";
                mq.DateProcessed = DateTime.Now;
                mq.ThreadLock = null;
                mq.Save();

                System.Diagnostics.Debug.WriteLine(string.Format("Email has been sent to {0}", mq.ToAddress));
            }
            catch (Exception e)
            {
                if (Wcss._Config._ErrorsToDebugger)
                {
                    System.Diagnostics.Debug.WriteLine("Mail service failure...");
                    System.Diagnostics.Debug.WriteLine(e.Message);
                    System.Diagnostics.Debug.WriteLine(e.StackTrace);
                }

                if (mq != null)
                {
                    mq.Status = e.Message + "\n" + e.StackTrace;
                    if (mq.Status.Length > 2000)
                        mq.Status = mq.Status.Substring(0, 1995);

                    mq.DateProcessed = DateTime.Now;
                    mq.ThreadLock = null;
                    mq.AttemptsRemaining -= 1;
                    mq.Save();
                }

                Wcss._Error.LogException(e);
            }
						
			return true;
		}

        public override void CleanUp()
        {
        }
	}
}
