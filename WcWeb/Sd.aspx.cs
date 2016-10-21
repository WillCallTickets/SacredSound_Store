using System;
using Wcss;

namespace WillCallWeb
{
    /// <summary>
    /// The Sd stands for "Site Director". This page is simply a redirect page. It will redirect to the url specified in the querystring. 
    /// It is used for logging/tracking requests. 
    /// IIS logging will handle the actual logging
    /// url - the url to redirect to
    /// seid - the subscriptionEmail Id
    /// </summary>
    public partial class Sd : System.Web.UI.Page
    {
        protected string url = string.Empty;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            //google analytics            
            if (Analytics_OpenCode())
                Analytics_CloseCode();
        }

        // no need to qualify ssl as this is just a redirector
        protected void Page_Load(object sender, EventArgs e)
        {
            string url = Request.QueryString["url"];

            if (url == null)
            {
                //redirect to a default page
                string landing = Wcss._Config._LandingPageUrl;

                if (landing == string.Empty)
                    Response.Redirect("/Store/ChooseTicket.aspx", true);
                else
                    Response.Redirect(landing, true);
            }
            else
            {
                //if the url begins with www add http
                if (url.ToLower().StartsWith("www.") || url.ToLower().StartsWith("www.sts9store.com"))
                    url = string.Format("http://{0}", url);

                Response.Redirect(url);
            }
        }

        private System.Text.StringBuilder gag = new System.Text.StringBuilder();
        protected bool Analytics_OpenCode()
        {
            if (Header != null)
            {
                string googleId = (Wcss._Config._DomainName != "localhost" && Wcss._Config._DomainName != "local.sts9.com" &&
                    this.Request.UserHostName != "localhost" && this.Request.UserHostName != "local.sts9.com" && this.Request.UserHostName != "127.0.0.l") ?
                    _Config._GoogleAnalyticsId : "UA-000000-0";
                System.Web.UI.WebControls.Literal lit = (System.Web.UI.WebControls.Literal)Header.FindControl("litGoogleAnalytics");

                if (lit != null && googleId != null && googleId.Trim().Length > 0)
                {
                    //reset stringbuilder
                    gag.Length = 0;

                    gag.AppendLine();
                    gag.AppendLine();
                    gag.AppendLine("<script type=\"text/javascript\">  try { ");
                    gag.Append(Utils.Constants.Tab);
                    gag.AppendLine("var _gaq = _gaq || [];");
                    gag.Append(Utils.Constants.Tab);
                    gag.AppendFormat("_gaq.push(['_setAccount', '{0}']);", googleId);
                    gag.AppendLine();
                    gag.Append(Utils.Constants.Tab);
                    gag.AppendLine("_gaq.push(['_trackPageview']);");
                    gag.AppendLine();

                    lit.Text = gag.ToString();

                    return true;

                }
            }

            return false;
        }
        protected void Analytics_CloseCode()
        {
            if (Header != null)
            {
                string googleId = (Wcss._Config._DomainName != "localhost" && Wcss._Config._DomainName != "local.sts9.com" && 
                    this.Request.UserHostName != "localhost" && this.Request.UserHostName != "local.sts9.com" && this.Request.UserHostName != "127.0.0.l") ?
                _Config._GoogleAnalyticsId : "UA-000000-0";
                System.Web.UI.WebControls.Literal lit = (System.Web.UI.WebControls.Literal)Header.FindControl("litGoogleAnalytics");

                if (lit != null && googleId != null && googleId.Trim().Length > 0)
                {
                    gag.Length = 0;

                    gag.Append(Utils.Constants.Tab);
                    gag.AppendLine("(function() {");
                    gag.Append(Utils.Constants.Tabs(2));
                    gag.AppendLine("var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;");
                    gag.Append(Utils.Constants.Tabs(2));
                    gag.AppendLine("ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';");
                    gag.Append(Utils.Constants.Tabs(2));
                    gag.AppendLine("var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);");
                    gag.Append(Utils.Constants.Tab);
                    gag.AppendLine("})();");
                    gag.AppendLine();
                    gag.AppendLine("} catch(err) {} </script>");
                    gag.AppendLine();
                    gag.AppendLine();

                    lit.Text += gag.ToString();
                }
            }
        }    
    }
}

