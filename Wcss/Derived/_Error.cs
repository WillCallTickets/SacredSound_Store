using System;
using System.IO;
using System.Diagnostics;
using System.Reflection;

using System.Net.Mail;

namespace Wcss
{
	public class _Error
	{
		public static string lockObj = "";
        public const string separator = "**************************************************";
        public const string sep = "__________________________________________________";

		public _Error() {}

        public static void LogPublishEvent(DateTime dtPublished, _Enums.PublishEvent pubEvent, string userName)
        {
            LogPublishEvent(dtPublished, pubEvent, "all keys", userName);
        }

        public static void LogPublishEvent(DateTime dtPublished, _Enums.PublishEvent pubEvent, string publishEntity, string userName)
        {
            string msg = string.Format("{0}: {1}{2} by {3}",
                dtPublished.ToString("MM/dd/yyyy hh:mm:ssstt"), pubEvent.ToString(),
                (publishEntity != null && publishEntity.Trim().Length > 0) ? string.Format(" {0}", publishEntity.Trim()) : string.Empty, userName);
            _Error.LogToFile(msg, string.Format("Publish_{0}", dtPublished.ToString("MM_dd_yyyy")));
        }

        /// <summary>
        /// Log to a filename. This file will reside in the errorlog path dir
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="fileName"></param>
        public static void LogToFile(string msg, string file)
        {
            FileStream fs = null;
            StreamWriter sw = null;

            try
			{
                //lock(lockObj)
                //{					
					string fileName = string.Format("{0}.log", file);
					string fullPath = string.Format("{0}\\{1}", _Config._ErrorLogPath, fileName);

                    string mappedPath = string.Empty;
                    string ip = "Ip not available";
                    string handler = "Request information not available";
                    string reqUrl = "url not available";
                    string userAgent = "agent info not available";
                    string newSess = "false";

                    if (System.Web.HttpContext.Current != null)
                    {
                        try
                        {
                            ip = System.Web.HttpContext.Current.Request.UserHostAddress;
                            handler = System.Web.HttpContext.Current.Handler.ToString();
                            reqUrl = System.Web.HttpContext.Current.Request.Url.ToString();
                            userAgent = System.Web.HttpContext.Current.Request.UserAgent;
                            newSess = System.Web.HttpContext.Current.Session.IsNewSession.ToString();
                        }
                        catch (Exception) { }

                        mappedPath = System.Web.HttpContext.Current.Server.MapPath(fullPath);
                    }
                    else
                        mappedPath = string.Format(@"{0}{1}", _Config._MappedRootDirectory,
                            fullPath.Replace("/", @"\")).Replace(@"\\", @"\");

                    fs = new FileStream(mappedPath, FileMode.Append, FileAccess.Write);
                    sw = new StreamWriter(fs);

                    sw.WriteLine(separator);
                    sw.WriteLine(string.Format("IP: {0}", ip));
                    sw.WriteLine(string.Format("Handler: {0}", handler));
                    sw.WriteLine(string.Format("Request: {0}", reqUrl));
                    sw.WriteLine(string.Format("UserAgent: {0}", userAgent));
                    sw.WriteLine(string.Format("New Session: {0}", newSess));
                    sw.Write(string.Format("{0}{1}{0}{2}{0}{0}", Environment.NewLine, msg, separator));
                //}
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            if (sw != null) sw.Close();
            if (fs != null) fs.Close();

        }

		/// <summary>
		/// this will write to the error log. It adds a seperator at the top of each entry
		/// </summary>
		/// <param name="msg"></param>        
        private static void WriteToLog( string msg )
        {
            FileStream fs = null;
            StreamWriter sw = null;

            try
            {
                //lock(lockObj)
                //{					
                    string fileName = string.Format("ErrorLog_{0}.log", DateTime.Now.ToString("MM_dd_yyyy"));
                    string fullPath = string.Format("{0}\\{1}", _Config._ErrorLogPath, fileName);

                    string mappedPath = string.Empty;
                    string ip = "Ip not available";
                    string handler = "Request information not available";
                    string reqUrl = "url not available";
                    string userAgent = "agent info not available";
                    string newSess = "false";

                    if (System.Web.HttpContext.Current != null)
                    {
                        try
                        {
                            ip = System.Web.HttpContext.Current.Request.UserHostAddress;
                            handler = System.Web.HttpContext.Current.Handler.ToString();
                            reqUrl = System.Web.HttpContext.Current.Request.Url.ToString();
                            userAgent = System.Web.HttpContext.Current.Request.UserAgent;
                            newSess = System.Web.HttpContext.Current.Session.IsNewSession.ToString();
                        }
                        catch(Exception){}

                        mappedPath = System.Web.HttpContext.Current.Server.MapPath(fullPath);
                    }
                    else
                        mappedPath = string.Format(@"{0}{1}", _Config._MappedRootDirectory,
                            fullPath.Replace("/", @"\")).Replace(@"\\", @"\");

                    fs = new FileStream(mappedPath, FileMode.Append, FileAccess.Write);
                    sw = new StreamWriter(fs);

                    sw.WriteLine(separator);
                    sw.WriteLine(DateTime.Now.ToString("MM/dd/yyyy hh:mmtt"));
                    sw.WriteLine(string.Format("IP: {0}", ip));
                    sw.WriteLine(string.Format("Handler: {0}", handler));
                    sw.WriteLine(string.Format("Request: {0}", reqUrl));
                    sw.WriteLine(string.Format("UserAgent: {0}", userAgent));
                    sw.WriteLine(string.Format("New Session: {0}", newSess));
                    sw.Write(string.Format("{0}{1}{0}{2}{0}{0}", Environment.NewLine, msg, separator));
               // }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            if (sw != null) sw.Close();
            if (fs != null) fs.Close();

        }

		private static string ReflectObjectToString(object o, int indentLevel)
		{
			string msg = "";
			string indent = new String('\t', indentLevel);

			Type type = o.GetType();
			PropertyInfo[] pinfos = type.GetProperties();
			FieldInfo[] finfos = type.GetFields();

			foreach( PropertyInfo pi in pinfos)
			{
				msg += type.Name + "." + pi.Name + ":  ";
				object val = pi.GetValue( o, null);

				if (val is string)
					msg += (string)val;
				if (val is DateTime)
					msg += ((DateTime)val).ToString();
				if (val is int)
					msg += ((int)val).ToString();
				if (val is decimal)
					msg += ((decimal)val).ToString();
				else
					msg += ReflectObjectToString(val, indentLevel + 1);						
				
				msg += "\r\n" + indent;
			}

			foreach( FieldInfo fi in finfos)
			{
				msg += type.Name + "." + fi.Name + ":  ";
				object val = fi.GetValue( o );

				if (val is string)
					msg += (string)val;
				else if (val is DateTime)
					msg += ((DateTime)val).ToString();
				else if (val is int)
					msg += ((int)val).ToString();
				else if (val is decimal)
					msg += ((decimal)val).ToString();
				else
					msg += ReflectObjectToString(val, indentLevel + 1);						
				
				msg += "\r\n" + indent;
			}

			return msg;
		}
        
        public static void LogException(Exception e, params object[] contextInfo)
        {
            LogException(e, false, contextInfo);
        }
        
		public static void LogException(Exception e, bool sendAdminEmail, params object[] contextInfo)
		{
            int eventId = 0;

            //log to a db and only write to text file if failure
            try
            {
                eventId = Erlg.ErrorLogger.WriteErrorToLog(e);

                WriteDebugger(e.Message);
            }
            catch (Exception ex)
            {
                WriteToLog(string.Format("Error in logging module.{0}{1}", Environment.NewLine, ex.Message));

                WriteDebugger(ex.Message);
                WriteDebugger(ex.StackTrace);
            }

            //if the above did not work or if we need to send an email
            if (eventId == 0 || sendAdminEmail)
            {
                try
                {
                    System.Text.StringBuilder msg = new System.Text.StringBuilder();

                    msg.AppendFormat("{0}{1}", DateTime.Now.ToString("MM/dd/yyyy hh:mm.ss tt"), Environment.NewLine);
                    msg.AppendFormat("Target Site: {0}{1}", (e != null && e.TargetSite != null) ? e.TargetSite.DeclaringType.ToString() : string.Empty, Environment.NewLine);
                    msg.AppendFormat("{0}{1}", sep, Environment.NewLine);

                    msg.AppendFormat("{0}{1}", e.Message, Environment.NewLine);
                    msg.AppendFormat("{0}{1}", e.StackTrace, Environment.NewLine);

                    if (e != null)
                    {
                        if (e.Data != null && e.Data.Keys.Count > 0)
                        {
                            msg.AppendFormat("Keys:{0}", Environment.NewLine);

                            foreach (object o in e.Data.Keys)
                                msg.AppendFormat("{0}{1} ~ {2}", o.ToString(), e.Data[o.ToString()].ToString(), Environment.NewLine);
                        }
                    }

                    foreach (object o in contextInfo)
                    {
                        msg.AppendFormat("Context Info:{0}", Environment.NewLine);
                        msg.Append(ReflectObjectToString(o, 1));
                    }

                    if (sendAdminEmail)
                        SendAdministrativeEmail(msg.ToString());

                    WriteDebugger(msg.ToString());

                    if(eventId == 0)
                        WriteToLog(msg.ToString());
                }
                catch (Exception ex)
                {
                    WriteToLog(string.Format("Error in log exception.{0}{1}", Environment.NewLine, ex.Message));


                    WriteDebugger(ex.Message);
                    WriteDebugger(ex.StackTrace);
                }
            }
		}

		public	static void WriteDebugger(string format, params object[] args)
		{
			if (_Config._ErrorsToDebugger)
			{
				Debug.WriteLine( string.Format( format, args));
			}
		}

        public static void SendAdministrativeEmail(string message)
        {
            SmtpClient client = new SmtpClient();

            try
            {
                client.Send(_Config._CustomerService_Email, _Config._Admin_EmailAddress, "System Error", message);
            }
            catch (Exception ex)
            {
                _Error.LogException(ex);
            }
        }
		
	}
}
