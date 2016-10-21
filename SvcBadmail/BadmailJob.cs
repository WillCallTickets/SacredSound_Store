using System;
using System.IO;
using System.Collections.Generic;
using System.Net.Mail;

using Jobs;
using Wcss;

namespace SvcBadmail
{
	/// <summary>
	/// runs a multithreadable Job
	/// </summary>
	public class BadmailJob : Jobs.Job
	{		
		/// <summary>
		/// provides mails to process
		/// </summary>
		//private EventData eventData = null;
        private readonly int _batchSize = 1000;

		/// <summary>
		/// Only constructor
		/// </summary>
        public BadmailJob() : base()
		{
            //if (_Config._ErrorsToDebugger)
            //    Debug.WriteLine("Starting Job...", "BadmailJob");

            //eventData = new EventData(this.JobId);
        }

        #region Properties
        //props
        private DirectoryInfo _badmailDirectory = null;
        protected DirectoryInfo BadmailDirectory
        {
            get
            {
                if (_badmailDirectory == null)
                {
                    //set initial directory
                    string initialDir = _Config.svc_AbsoluteBadmailPath;

                    if (!Directory.Exists(initialDir))
                        throw new DirectoryNotFoundException("Badmail directory specified does not exist.");

                    _badmailDirectory = new DirectoryInfo(initialDir);
                }

                return _badmailDirectory;
            }
        }
        private DirectoryInfo _toProcessDirectory = null;
        protected DirectoryInfo ToProcessDirectory
        {
            get
            {
                if (_toProcessDirectory == null)
                {
                    string processDir = string.Format("{0}{1}", _Config._MappedRootDirectory, @"\BadmailService\ToProcess\");

                    if (!Directory.Exists(processDir))
                        Directory.CreateDirectory(processDir);

                    _toProcessDirectory = new DirectoryInfo(processDir);
                }

                return _toProcessDirectory;
            }
        }
        private DirectoryInfo _nothandledDirectory = null;
        protected DirectoryInfo NotHandledDirectory
        {
            get
            {
                if (_nothandledDirectory == null)
                {
                    string nothandledDir = string.Format("{0}{1}", _Config._MappedRootDirectory, @"\BadmailService\NotHandled\");

                    if (!Directory.Exists(nothandledDir))
                        Directory.CreateDirectory(nothandledDir);

                    _nothandledDirectory = new DirectoryInfo(nothandledDir);
                }

                return _nothandledDirectory;
            }
        }
        #endregion


        /// <summary>
		/// Entry point
        /// see http://noggle.com/?p=23 
		/// </summary>
		/// <returns></returns>
		public override bool DoJob()
		{
            if (_Config.SqlServerIsAvailable())
            {
                //delete non-essential files and move others into process dir
                RemoveUnwantedFilesAndMoveToProcessDir();

                //process email in process dir
                ProcessBadmail();
            }
            else
            {   
                _Error.LogToFile("Sql Server not available.", string.Format("{0}{1}", _Config._ErrorLogTitle, DateTime.Now.ToString("MM_dd_yyyy")));

                System.Threading.AutoResetEvent waitingEvent = new System.Threading.AutoResetEvent(false);
                waitingEvent.WaitOne(28800000, true);//every 8 minutes - 8 * 60 * 60 * 1000 = 28800000
            }

            return true;
		}

        

        protected void ProcessBadmail()
        {
            List<FileInfo> files = new List<FileInfo>();
            
            files.AddRange(ToProcessDirectory.GetFiles("*.bad"));

            //restrain the workload
            if (files.Count > _batchSize)
                files.RemoveRange(_batchSize, files.Count - _batchSize);

            foreach (FileInfo file in files)
            {
                //string sender = null;
                string recipient = null;
                string status = null;
                string diagCode = null;
                string subId = null;
                bool handled = false;

                if (File.Exists(file.FullName))
                {
                    using (StreamReader reader = new StreamReader(file.FullName))
                    {
                        string line;

                        try
                        {
                            while ((line = reader.ReadLine()) != null)
                            {
                                //**note add an extra char to avoid space
                                if (line.IndexOf("Final-Recipient:", StringComparison.OrdinalIgnoreCase) != -1)
                                {
                                    recipient = line.Substring(line.IndexOf("rfc822;") + 7);
                                }
                                else if (line.IndexOf("Status:", StringComparison.OrdinalIgnoreCase) != -1)
                                {
                                    status = line.Substring(line.IndexOf("Status:") + 8);
                                }
                                else if (line.IndexOf("Diagnostic-Code:", StringComparison.OrdinalIgnoreCase) != -1)
                                {
                                    diagCode = line.Substring(line.IndexOf("Diagnostic-Code:") + 17);
                                }
                                else if (line.IndexOf("subemlid=", StringComparison.OrdinalIgnoreCase) != -1)//subemlid=3D'10006'
                                {
                                    string _line = line;
                                    int idx = _line.IndexOf("subemlid=");
                                    char[] badChars = { '=','\''};
                                    string part1 = _line.Substring(idx + 12, 7).Replace("\"",string.Empty).TrimStart(badChars).TrimEnd(badChars);
                                    subId = part1;// line.Substring(line.IndexOf("subemlid=3D'") + 10, 7).TrimEnd('"').TrimEnd('\'').Trim();//subemlid=3D'10006'
                                    //no reason to read the email any further
                                    break;
                                }
                            }

                            //disregard non list emails
                            if (subId == null)
                                handled = true;
                            else if (recipient != null && status != null)
                            {
                                MailAddress email = new MailAddress(recipient);

                                string sts = status.Replace(".", string.Empty).Trim();

                                switch (sts)
                                {
                                    case "520"://Message identified as SPAM 
                                    case "560":// Lone CR or LF in body (see RFC2822 section 2.3)
                                        //dont handle and notify admin
                                        //currently we notfiy admin - which means the admin must check msgs to find issues
                                        //TODO: notify admin properly
                                        
                                        handled = false;
                                        break;



                                    //dont handle and dont notify admin
                                    case "518"://MX invalid #439
                                    case "533"://unrecognized command
                                    case "554"://unrecognized parameter
                                        handled = false;
                                        break;





                                    //content or security violation
                                    //notify admin
                                    case "571":
                                    //notify admin of connection issues - be sure to once a day at most?
                                    //put into handled folder
                                    case "447":
                                        //if no other eventQ row found within last 24 hours
                                        //construct an event that will send email to admin for notification 
                                        //string subject = string.Format("Connection error {0} - {1}", sts, email.Host);

                                        //System.Text.StringBuilder sb = new System.Text.StringBuilder();
                                        ////sb.Append("SELECT mq.[Id] FROM [MailQueue] mq WHERE mq.[Subject] = {0} AND mq.[dtProcess] > DATEADD(hh,-24,(getDate())) ", subject);
                                        //sb.Append("SELECT eq.[Id] FROM [EventQ] eq WHERE eq.[ApplicationId] = @appId AND ");
                                        //sb.Append("eq.[Context] = @context AND eq.[Verb] = @verb AND eq.[NewValue] = @subject AND ");
                                        //sb.Append("eq.[dtStamp] > DATEADD(hh,-24,(getDate())); ");

                                        //SubSonic.QueryCommand cmd = new SubSonic.QueryCommand(sb.ToString());
                                        //cmd.Parameters.Add("@appId", _Config.APPLICATION_ID, System.Data.DbType.Guid);
                                        //cmd.Parameters.Add("@subject", subject);
                                        //cmd.Parameters.Add("@context", _Enums.EventQContext.Mailer.ToString());
                                        //cmd.Parameters.Add("@verb", _Enums.EventQVerb.Mailer_FailureNotification);

                                        //int affectedRows = SubSonic.DataService.ExecuteQuery(cmd);
                                        //if (affectedRows < 1)
                                        //{
                                        //    //do we need any notification? 090412

                                        //    //EventQ.CreateMailerNotification(DateTime.Now, "BadmailService", recipient, _Enums.EventQVerb.Mailer_FailureNotification, null, subject, status);

                                        //    //note that handle is not reset - we want a copy of this one
                                        //}
                                        //else// the mail will be deleted
                                        handled = true;

                                        break;
                                    //mailbox full
                                    //handle and ignore
                                    case "522":
                                    //case "417"://sender address rejected
                                    case "421"://sender verification in progress
                                        handled = true;
                                        break;
                                    //no user or discontinued
                                    //remove email from list
                                    //handle
                                    case "510"://inactive user
                                    case "511":
                                    case "550":
                                    case "551":
                                    
                                    case "500"://userunknown - no mailbox
                                    case "521"://mailbox inactive
                                    case "530"://addressee unknown
                                    case "535"://bad address
                                    case "540"://mispelled domain eg. msn.co, yahho.com, colrado.edu
                                    case "541"://Recipient address rejected: Access Denied
                                    case "570"://email not valid, local policy violation
                                    case "400"://size limit exceeded, mailbox disabled, no such recipient
                                    case "516"://mailbox no longer valid

                                    //these are not unknown user codes - but the email domains will never accept our address
                                    case "417"://sender address rejected
                                    case "471"://bad envelope from address - does not like "pleasedonotreply" address

                                        //insert a service event to remove the user from our lists
                                        EventQ.CreateMailerNotification(DateTime.Now, "BadmailService", recipient, _Enums.EventQVerb.Mailer_Remove, subId,
                                            status, (diagCode != null) ? diagCode : "Recipient was not found at host");
                                    
                                        handled = true;

                                        break;
                                }
                            }

                        }
                        catch (System.FormatException fe)
                        {
                            _Error.LogException(fe);
                            handled = true;
                        }
                        catch (Exception ex)
                        {
                            //services do not log exception
                            _Error.LogException(ex);
                            if (recipient != null)
                            {
                                _Error.LogException(new Exception(recipient));
                            }
                        }
                        finally
                        {
                            reader.Close();
                        }
                    }

                    //if we cannot process - move to unhandled directory
                    if (handled)//toss it out
                    {
                        file.Delete();
                    }
                    else
                    {
                        string destination = string.Format(@"{0}{1}", this.NotHandledDirectory, file.Name);

                        if (File.Exists(destination))
                            File.Delete(destination);

                        file.MoveTo(destination);
                    }

                }

            }

        }

        #region Collation

        protected void RemoveUnwantedFilesAndMoveToProcessDir()
        {
            List<FileInfo> files = new List<FileInfo>();

            files.AddRange(BadmailDirectory.GetFiles("*.bdp"));
            for (int i = 0; i < files.Count; i++)
                files[i].Delete();

            files.Clear();

            files.AddRange(BadmailDirectory.GetFiles("*.bdr"));
            for (int i = 0; i < files.Count; i++)
                files[i].Delete();

            files.Clear();

            files.AddRange(BadmailDirectory.GetFiles("*.bad"));

            //restrain the workload
            if (files.Count > _batchSize)
                files.RemoveRange(_batchSize, files.Count - _batchSize);

            //remove delays and move failures to process dir
            foreach (FileInfo file in files)
            {
                if (File.Exists(file.FullName))
                {
                    using (StreamReader reader = new StreamReader(file.FullName))
                    {
                        string line;

                        try
                        {
                            while ((line = reader.ReadLine()) != null)
                            {
                                if (line.IndexOf("Subject: Delivery Status Notification (Delay)", StringComparison.OrdinalIgnoreCase) != -1)
                                {
                                    reader.Close();
                                    file.Delete();
                                    break;
                                }
                                else if (line.IndexOf("Subject: Delivery Status Notification (Failure)", StringComparison.OrdinalIgnoreCase) != -1)
                                {
                                    reader.Close();
                                    //if the file exists in the destination - delete it
                                    string destination = string.Format(@"{0}{1}", this.ToProcessDirectory, file.Name);
                                    
                                    if (File.Exists(destination))
                                        File.Delete(destination);

                                    file.MoveTo(destination);

                                    break;
                                }
                            }
                        }
                        catch (Exception)
                        {
                        }
                        finally
                        {
                            reader.Close();
                        }
                    }
                }
            }
        }
        #endregion

        public override void CleanUp()
        {
            //TODO
            //eventData.CommitAll();
        }
	}
}
