using System;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Text.RegularExpressions;

using Utils;

namespace Wcss
{
    public partial class SubscriptionEmail
    {
        [XmlAttribute("DateLastSent")]
        public DateTime DateLastSent
        {
            get { return (this.DtLastSent.HasValue) ? this.DtLastSent.Value : DateTime.MinValue; }
            set { this.DtLastSent = (value > DateTime.MinValue) ? (DateTime?)value : null; }
        }

        public static string Path_MailTemplate = string.Format("/{0}/MailTemplates/", _Config._VirtualResourceDir);
        public static string Path_PostedSubscriptions = string.Format("{0}Subscriptions/", Path_MailTemplate);
        public static string Path_PostedImages = string.Format("{0}Images/", Path_MailTemplate);
        public static string Path_PostedCss = string.Format("{0}Css/", Path_MailTemplate);
        public static string Css_OptOut = ".publishlink{margin-bottom: 1em;} .publishlink A, .optout A{display:inline;} .optout{margin-top: 1em;} ";

        public string PublishedPathAndFile_Virtual
        {
            get
            {
                return string.Format("{0}{1}", SubscriptionEmail.Path_PostedSubscriptions, this.PostedFileName);
            }
        }
        public string PublishedPathAndFile_Mapped
        {
            get
            {
                if (System.Web.HttpContext.Current != null)
                    return System.Web.HttpContext.Current.Server.MapPath(this.PublishedPathAndFile_Virtual);

                return string.Format("{0}{1}", _Config._MappedRootDirectory, this.PublishedPathAndFile_Virtual);
            }
        }
        public string PublishedPathAndFile_Url
        {
            get
            {
                return string.Format("http://{0}{1}", _Config._DomainName, this.PublishedPathAndFile_Virtual);
            }
        }
        [XmlAttribute("EmailLetterName")]
        public string EmailLetterName
        {
            get { return this.EmailLetterRecord.Name; }
        }
        [XmlAttribute("CssFile_Parsed")]
        public string CssFile_Parsed
        {
            get { return this.CssFile ?? string.Empty; }
        }
        public static string ConstructPostedFileName()
        {
            return string.Format("{0}.html", Guid.NewGuid().ToString().Replace("-", string.Empty));
        }
        public static string ConstructBodyName(string name, bool appendHtml)
        {
            string datePart = string.Empty;
            string strippedName = name.Trim();

            if (strippedName.IndexOf("_") != -1)
            {
                string leading = strippedName.Substring(0, strippedName.IndexOf("_") - 1);
                if (Utils.Validation.IsDate(leading))
                    datePart = leading;

                strippedName = strippedName.Substring(strippedName.IndexOf("_")).TrimStart('_');
            }

            strippedName = Utils.ParseHelper.StringToProperCase(strippedName).Replace(" ", string.Empty).Replace("/","-").Replace("\\","-");

            if(appendHtml && (!strippedName.EndsWith(".html", StringComparison.OrdinalIgnoreCase)))
                strippedName = string.Format("{0}.html", strippedName);

            return string.Format("{0}_{1}", (datePart.Trim().Length > 0) ? datePart : DateTime.Now.ToString("yyMMddhhmmtt"), 
                strippedName);
        }

        /// <summary>
        /// Sets the constructed email values to null to allow the service to rewrite. This needs to be rewritten by the service to ensure 
        /// the most up to dat version
        /// </summary>
        private void PrepareForPublication()
        {
            this.ConstructedHtml = null;
            this.ConstructedText = null;
            this.Save();
        }

        /// <summary>
        /// Must be called prior to publishing the path in the email as it will change the body name and 
        /// where it is posted if needed. Ensures that the email has been converted to html and is published to the web
        /// </summary>
        /// <param name="subEmailIdx"></param>
        public void EnsurePublication(bool includeSubscriptionOptOut)
        {
            this.PrepareForPublication();

            //posted file and mapped resource dir overlap - so use get directory
            //map the path
            string mappedPath = Path.GetFullPath(PublishedPathAndFile_Mapped);

            //if file exists - delete
            FileStream fs = null;
            StreamWriter sw = null;

            try
            {
                // clear existing contents
                if (File.Exists(mappedPath))
                    File.Delete(mappedPath);

                fs = new FileStream(mappedPath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
                sw = new StreamWriter(fs, Encoding.GetEncoding("utf-8"));

                sw.Write(this.CreateShell_Html(true, false, false, includeSubscriptionOptOut));
            }
            catch (Exception ex)
            {
                _Error.LogException(ex);
            }
            finally
            {
                if (sw != null) sw.Close();
                if (fs != null) fs.Close();
            }
        }

        /// <summary>
        /// Creates a plain text view email and adds "cannot view" and opt-out links
        /// </summary>
        /// <param name="useComments">Comments are not necessary for a plain text view</param>
        public string CreateShell_Text(bool useComments, bool includeSubscriptionOptOut)
        {
            if (this.EmailLetterRecord.TextVersion == null || this.EmailLetterRecord.TextVersion.Trim().Length == 0)
                return string.Empty;

            string domainName = _Config._DomainName;

            StringBuilder sb = new StringBuilder();
            
            if(useComments)
                sb.AppendFormat("<!--{0}", Constants.NewLine);

            //this can be found in the meta tag of the html
            //sb.AppendFormat("SubscriptionEmailId={1}{0}", Constants.NewLines(2), this.Id);

            sb.AppendFormat("{1}{0}", Constants.NewLines(2), this.EmailLetterRecord.Subject.Trim());

            //insert cannot view link
            //note that we are using encoding here - to help with cut and paste
            //the rest of the TEXT email should remain as is
            sb.AppendFormat("If you are having problems reading this email, \r\nplease visit {0}{1}",
                System.Web.HttpUtility.HtmlEncode(this.PublishedPathAndFile_Url), Constants.NewLines(2));

            //content
            sb.AppendFormat("{0}{1}", ConfigureBodyLinks_Text(this.EmailLetterRecord.TextVersion), Constants.NewLines(2));
            
            //insert optout link
            sb.AppendFormat(this.AddOptOut_Text(domainName, includeSubscriptionOptOut));

            if (useComments)
                sb.AppendFormat("{0}-->{0}", Constants.NewLine);

            return sb.ToString();
        }

        /// <summary>
        /// creates an html formatted string from a text file - with options for "cannot view" and opt-out links
        /// </summary>
        /// <param name="insertHtmlStructure">Determines if we need to add the doctype, html, head, etc. This should only be skipped(false) for the 
        /// file viewer that is contained in the html rendered for the viewer page</param>
        /// <param name="includeCantViewAndOptOutLinks">Determines if the "cannot view" and opt out links are included. Not necessary for published versions</param>
        /// <param name="convertVirtualLinksToUrls">True for an email that does not contain embedded images. Sets a link to the hosted images, etc.</param>
        public string CreateShell_Html(bool insertHtmlStructure, bool includeViewAndOptOutLinks, 
            bool convertVirtualLinksToUrls, bool includeSubscriptionOptOut)
        {
            string domainName = _Config._DomainName;
            string virtResourceDirectory = _Config._VirtualResourceDir;
            
            string cssPathAndFile = (this.CssFile == null) ? string.Empty :
                string.Format("http://{0}{1}{2}", domainName, SubscriptionEmail.Path_PostedCss, this.CssFile.Trim());

            string content = this.EmailLetterRecord.HtmlVersion.Trim();

            int tabOrdinal = 1;
            StringBuilder sb = new StringBuilder();

            //this should only be skipped for the file viewer that is contained in html already
            if (insertHtmlStructure)
            {
                sb.Append("<!DOCTYPE HTML PUBLIC '-//W3C//DTD HTML 4.01 Transitional//EN'>");
                sb.AppendFormat("{0}<html>{0}{1}<head>", Constants.NewLine, Constants.Tabs(tabOrdinal));
                sb.AppendFormat("{0}{1}<title>{2}</title>", Constants.NewLine, Constants.Tabs(tabOrdinal),
                    this.EmailLetterRecord.Subject.Trim());

                sb.AppendFormat("<meta subemlid=\"{1}\" />{0}", Constants.NewLine, this.Id);

                //always add these style to deal with opt out links, problems viewing link and address info
                sb.AppendFormat("<style type=\"text/css\"><!--{0}", Constants.NewLine);

                //add in standard css here
                if (includeViewAndOptOutLinks)
                    sb.AppendFormat("{0}{1}", SubscriptionEmail.Css_OptOut, Constants.NewLine);

                string style = this.EmailLetterRecord.StyleContent;
                if (style != null && style.Trim().Length > 0)
                    sb.Append(style);

                sb.AppendFormat("{0}--></style>{0}", Constants.NewLine);

                sb.AppendFormat("{0}{1}</head>", Constants.NewLine, Constants.Tabs(tabOrdinal));
                sb.AppendFormat("{0}{1}{0}<body>", Constants.NewLine, Constants.Tabs(tabOrdinal));
            }

            //insert cannot view link - track seid
            if ((includeViewAndOptOutLinks) && this.PostedFileName.Trim().Length > 0)
                sb.AppendFormat("<div class=\"publishlink\">If you are having problems reading this email, please visit {0}</div>{1}",
                    string.Format("<a href=\"{0}?seid={1}\" target=\"_blank\">{2}</a>", 
                        System.Web.HttpUtility.HtmlEncode(Utils.ParseHelper.FormatUrlFromString(this.PublishedPathAndFile_Url, true, false)), 
                        this.Id, this.EmailLetterRecord.Name), 
                    Constants.NewLines(2));

            content = ConfigureBodyLinks_Html(content);

            //emails should have non-relative links
            if (convertVirtualLinksToUrls)
                content = ConvertImgVirtualPathsToUrlPaths(content);

            sb.Append(content);

            if (includeViewAndOptOutLinks)
                sb.AppendFormat("{0}", this.AddOptOut_Html(domainName, includeSubscriptionOptOut));

            if (insertHtmlStructure)
                sb.Append("</body></html>");

            return sb.ToString();
        }

        private string _optOutHtml = string.Empty;
        private string AddOptOut_Html(string domainName, bool includeSubscriptionOptOut)
        {
            if(_optOutHtml.Length == 0)
            {
                StringBuilder sb = new StringBuilder();
                if(includeSubscriptionOptOut)
                    sb.AppendFormat("{1}<div class=\"optout\">To remove your email from our subscriptions, please visit our <a href=\"http://{0}/MailerManage.aspx\" target=\"_blank\" >mail manager</a></div>",
                        domainName, Constants.NewLines(2));
                sb.AppendFormat("{0}<div class=\"optout\">If you have an account with our store, you may edit your <a href=\"http://{1}/EditProfile.aspx\">profile</a></div>",
                    Constants.NewLine, domainName);
                sb.AppendFormat("{2}<div class=\"optout\">You may also contact us at {0} or send a polite email to {1}</div>",
                    _Config._Site_Entity_PhysicalAddress, _Config._CustomerService_Email, Constants.NewLine);

                _optOutHtml = sb.ToString();
            }

            return _optOutHtml;
        }
        private string _optOutTxt = string.Empty;
        private string AddOptOut_Text(string domainName, bool includeSubscriptionOptOut)
        {
            if (_optOutTxt.Length == 0)
            {
                StringBuilder sb = new StringBuilder();
                if(includeSubscriptionOptOut)
                    sb.AppendFormat("{1}To remove your email from our subscriptions, please visit http://{0}/MailerManage.aspx \r\n",
                        domainName, Constants.NewLines(2));
                sb.AppendFormat("{0}If you have an account with our store, you may edit your http://{1}/EditProfile.aspx\r\n",
                     Constants.NewLine, domainName);
                sb.AppendFormat("{2}You may also contact us at {0} or send a polite email to {1}\r\n",
                    _Config._Site_Entity_PhysicalAddress, _Config._CustomerService_Email, Constants.NewLine);

                _optOutTxt = sb.ToString();
            }

            return _optOutTxt;
        }

        ////TODO: work out css and other attachment besides images
        private string ConvertImgVirtualPathsToUrlPaths(string content)
        {
            content = content.Replace("src=\"/", "~&**)%");
            content = content.Replace("src='/", "~&**)%");
            return content.Replace("~&**)%", string.Format("src=\"http://{0}/", _Config._DomainName));
        }

        #region Link handling


        //helps with tracking links
        private string EvaluateLink(Match m)
        {
            //we have start and end because the quoting could be different
            string start = m.Groups["linkStart"].Value;
            string link = m.Groups["linkProper"].Value.TrimEnd('.');
            string end = (m.Groups["linkEnd"] != null) ? m.Groups["linkEnd"].Value : string.Empty;

            Regex storeSite = new Regex(string.Format(@"{0}", Wcss._Config._DomainName.Replace("www.", string.Empty)));
            //leave for testing
            //Regex storeSite = new Regex(string.Format(@"{0}", "foxtheatre.com"));

            Match isStoreLink = storeSite.Match(link);

            //only append a querystring to those links on our site
            string qsIdName = "seid";
            string googleCampaign = string.Format("utm_source=WctMlr&utm_medium=email&utm_campaign={0}",
                System.Web.HttpUtility.HtmlEncode(this.EmailLetterName));
            string newLink = string.Empty;
            
            if(link.ToLower().IndexOf("mailto:") != -1)
            {
                //dont include the other "qs" var for a subject - just add the name
                newLink = string.Format("{0}{1}{2}{3}{4}{5}", start, link, 
                    (link.ToLower().IndexOf("?") == -1) ? "?" : string.Empty, 
                    (link.ToLower().IndexOf("subject=") == -1) ? "subject=" : " - ", 
                    this.EmailLetterName, end);
            }
            else
                newLink = 
                    (isStoreLink.Success) ?
                    string.Format("{0}{1}{2}{3}{4}", start, link, (link.IndexOf("?") != -1) ? "&" : "?", googleCampaign, end) :
                    string.Format("{0}http://{1}/Sd.aspx?{2}={3}&url={4}&{5}{6}", start, Wcss._Config._DomainName, qsIdName, this.Id, 
                        System.Web.HttpUtility.HtmlEncode(link), googleCampaign, end);

            return newLink;
        }

        private string ConfigureBodyLinks_Html(string txt)
        {
            //Debug.WriteLine(url);

            //Regex getLinks = new Regex(@"(?<linkStart><A[^>]*?HREF\s*=\s*[""']?)(?<linkProper>[^'"" >]+?)(?<linkEnd>[ '""]?>)", RegexOptions.IgnoreCase | RegexOptions.Multiline);

            //100111 - changed regex because it (previous version above) was evaluating "<area" tags - needed to add tags to href instead
            //Regex getLinks = new Regex(@"(?<linkStart><A[^>]*?HREF\s*=\s*[""']?)(?<linkProper>[^'"">]+?)(?<linkEnd>[ '""]?\s*>)", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            Regex getLinks2 = new Regex(@"(?<linkStart> HREF\s*=\s*[""']?)(?<linkProper>[^#'"">]+?)(?<linkEnd>[ '""]+?)", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            string result = getLinks2.Replace(txt, new MatchEvaluator(this.EvaluateLink));

            //Debug.WriteLine(result);

            return result;

        }

        //TODO
        private string ConfigureBodyLinks_Text(string txt)
        {
            //Regex getLinks = new Regex(@"(?<linkStart>(http(s)?://)?(www.))(?<linkProper>[^'"" >]+?)", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            Regex getLinks = new Regex(@"((http(s)?://))(?<linkProper>[^'"" >]+?)", RegexOptions.IgnoreCase | RegexOptions.Multiline);

            string result = getLinks.Replace(txt, new MatchEvaluator(this.EvaluateLink));

            return result;

        }

        #endregion
    }
}
