using System;
using System.Web;
using System.Configuration;
using System.Diagnostics;

namespace Erlg
{
    public partial class ErrorLogger
    {
        public ErrorLogger()
        {
        }

        /// <summary>
        /// Writes exception information to the error logging db. 090425 form information has been removed.
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static int WriteErrorToLog(Exception ex)
        {
            string _app = Wcss._Config.APPLICATION_NAME;
            bool LogToDB = false;
            bool LogToEventViewer = false;
            
            //get access to provider vars/settings
            SubSonic.SubSonicSection section = (SubSonic.SubSonicSection)ConfigurationManager.GetSection("SubSonicService");
            SubSonic.SqlDataProvider _provider = (SubSonic.SqlDataProvider)SubSonic.DataService.Providers["ErrorLog"];

            if (_provider == null)
                System.Diagnostics.EventLog.WriteEntry("ErrorLogger", "Provider cannot be located", System.Diagnostics.EventLogEntryType.Error, 0);
            else
            {
                bool passMuster = true;
                ProviderSettings settings = section.Providers[_provider.Name];

                //ensure we have log to db and viewer settings
                if(settings.Parameters["LogToDB"] == null)
                {
                    System.Diagnostics.EventLog.WriteEntry("ErrorLogger", "LogToDB parameter not specified in provider string", System.Diagnostics.EventLogEntryType.Error, 0);
                    passMuster = false;
                }
                if (settings.Parameters["LogToEventViewer"] == null)
                {
                    System.Diagnostics.EventLog.WriteEntry("ErrorLogger", "LogToEventViewer parameter not specified in provider string", System.Diagnostics.EventLogEntryType.Error, 0);
                    passMuster = false;
                }

                if (!passMuster)
                    return 0;

                string db = settings.Parameters["LogToDB"].ToString();
                string vw = settings.Parameters["LogToEventViewer"].ToString();

                LogToDB = bool.Parse(db);
                LogToEventViewer = bool.Parse(vw);
            }

            int eventId = 0;
            HttpContext ctx = HttpContext.Current;

            string _source = string.Empty;
            string _message = string.Empty;
            string _form = string.Empty;
            string _querystring = string.Empty;
            string _targetsite = string.Empty;
            string _stacktrace = string.Empty;
            string _referrer = string.Empty;
            string _ipaddress = string.Empty;
            string _email = string.Empty;


            //FORM
            if (ctx == null)
                _form = "no ctx";
            else
            {
                if (ctx.Request != null)
                    _form = ctx.Request.Url.ToString().ToLower();
                else
                    _form = "no request";
            }


            //REFERRER
            if (ctx != null)
            {
                if (ctx.Request != null)
                {
                    try
                    {
                        _referrer = ctx.Request.Url.ToString();
                        _referrer += string.Format(" (ref) {0}",
                            (ctx.Request.ServerVariables["HTTP_REFERER"] != null) ? ctx.Request.ServerVariables["HTTP_REFERER"].ToString() : "no referrer");
                    }
                    catch (Exception ex1)
                    {
                        _referrer = string.Format("ref exception: {0}", ex1.Message);
                    }

                    if (ctx.Request.QueryString != null)
                        _querystring = ctx.Request.QueryString.ToString();
                    if (ctx.Request.UserHostAddress != null)
                        _ipaddress = ctx.Request.UserHostAddress;

                }
                if (ctx.User != null)
                {
                    _email = (ctx.User.Identity != null && ctx.User.Identity.IsAuthenticated) ?
                        ctx.User.Identity.Name : "not authd";
                }
            }
            else {
                _referrer = "HttpContext.Current = null";
            }

            _source = (ex.Source != null) ? ex.Source : string.Empty;
            _message = (ex.Message != null) ? ex.Message : string.Empty;


            if (ex.TargetSite != null)
            {
                try
                {
                    _targetsite = string.Format("{0}_{1}", ex.TargetSite.DeclaringType.FullName, ex.TargetSite.Name);
                }
                catch (Exception er)
                {
                    _targetsite = string.Format("error: {0}", er.Message);
                }
            }
            else
                _targetsite = "not available";

            
            _stacktrace = (ex.StackTrace != null) ? ex.StackTrace : string.Empty;
            
            //ensure that we dont overdo the input length
            _source = Utils.ParseHelper.ParseToLength(_source, 50);
            _message = Utils.ParseHelper.ParseToLength(_message, 2000);
            _form = Utils.ParseHelper.ParseToLength(_form, 256);
            _querystring = Utils.ParseHelper.ParseToLength(_querystring, 256);
            _targetsite = Utils.ParseHelper.ParseToLength(_targetsite, 512);
            _stacktrace = Utils.ParseHelper.ParseToLength(_stacktrace, 8000);
            _referrer = Utils.ParseHelper.ParseToLength(_referrer, 512);
            _ipaddress = Utils.ParseHelper.ParseToLength(_ipaddress, 25);
            _email = Utils.ParseHelper.ParseToLength(_email, 256);
            _app = Utils.ParseHelper.ParseToLength(_app, 25);

            //for logging to event log
            string _data = string.Format("{0}APP: {1}{0}SOURCE: {2}{0}MESSAGE: {3}{0}FORM: {4}{0}QUERYSTRING: {5}{0}TARGETSITE: {6}{0}STACKTRACE: {7}{0}REFERRER: {8}{0}EMAIL: {9}",
                Environment.NewLine, _app, _source, _message, _form, _querystring, _targetsite, _stacktrace, _referrer, _email);

            //Logging
            if (LogToDB)
            {
                try
                {
                    SubSonic.StoredProcedure s = SPs.ElxLogError(_source, DateTime.Now, _message, _form, _querystring, _targetsite, _stacktrace, _referrer, _ipaddress, _email, _app, eventId);
                    s.Execute();

                    if (s.OutputValues.Count == 1)
                    {
                        try
                        {
                            eventId = (int)s.OutputValues[0];
                        }
                        catch (Exception)
                        {
                            eventId++;
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }

            if (LogToEventViewer)
            {
                try
                {
                    if (_source == null || _source.Trim().Length == 0)
                        _source = "unknown source";

                    if (!EventLog.SourceExists(_source))
                        EventLog.CreateEventSource(_source, "Application");

                    EventLog.WriteEntry(_source, _data, EventLogEntryType.Error, eventId);

                }
                catch (Exception)
                {
                    throw;
                }
            }

            return eventId;
        }
    }
}
