using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Threading;
using System.Diagnostics;

using Wcss;

namespace WillCallWeb
{
    public partial class BasePage : System.Web.UI.Page
    {
        private HtmlMeta MakeNewMeta(string prop, string val, string content)
        {
            HtmlMeta meta = new HtmlMeta();
            meta.Attributes.Add(prop, val);// m1.Attributes.Add("property", "og:title");
            meta.Content = content;

            return meta;
        }
        private void AddMetaTagToPage(HtmlMeta meta)
        {
            System.Web.UI.WebControls.Literal nl = new System.Web.UI.WebControls.Literal();
            nl.Text = Environment.NewLine;
            this.Page.Header.Controls.Add(nl);

            this.Page.Header.Controls.Add(meta);            
        }

        public void FB_WRITEMETA(Show _show)
        {
            //todo ensure keys dont already exist?
            if (_show != null)
            {
                //FACEBOOK DOES THE HTML ENCODING!!!!
                System.Web.UI.Page _page = this.Page;

                AddMetaTagToPage(MakeNewMeta("property", "fb:app_id", _Config._FacebookIntegration_App_Id));

                //should be venue
                //date list - as a range
                //act
                string locale = string.Format("{0}{1}{2}",
                    (_show.VenueRecord.City != null && _show.VenueRecord.City.Trim().Length > 0) ? _show.VenueRecord.City.Trim() : string.Empty,
                    (_show.VenueRecord.City != null && _show.VenueRecord.City.Trim().Length > 0 && _show.VenueRecord.State != null && _show.VenueRecord.State.Trim().Length > 0) ?
                    ", " : string.Empty,
                    (_show.VenueRecord.State != null && _show.VenueRecord.State.Trim().Length > 0) ? _show.VenueRecord.State.Trim() : string.Empty);

                //first check show for billing
                //then check first show date - only if it is a single show
                //then use the show name part - generic
                //string billing = (_show.FbBilling != null && _show.FbBilling.Trim().Length > 0) ?
                //    _show.FbBilling.Trim() :
                //    //if we only have one show date and that showdate has its own billing
                //    (_show.FirstDate == _show.LastDate && _show.FirstShowDate.Billing != null && _show.FirstShowDate.Billing.Trim().Length > 0) ?
                //    _show.FirstShowDate.Billing.Trim() :
                string billing = _show.ShowEventPart;

                string title = string.Format("{0}{1} - {2} - {3}",
                    _show.VenueRecord.Name,
                    (locale.Trim().Length > 0) ? string.Format(" {0}", locale.Trim()) : string.Empty,
                    _show.Display.Date_NoMarkup_3Day_NoTime_Ranged_NoStatus,
                    billing
                    );

                AddMetaTagToPage(MakeNewMeta("property", "og:title", title));
                
                AddMetaTagToPage(MakeNewMeta("property", "og:type", "band"));

                AddMetaTagToPage(MakeNewMeta("property", "og:url", _page.Request.Url.ToString()));
                    //string.Format("http{0}://{1}{2}", (_page.Request.IsSecureConnection) ? "s" : string.Empty, _Config._DomainName, _page.Request.Path)));

                AddMetaTagToPage(MakeNewMeta("property", "og:site_name", _Config._FacebookIntegration_App_Name));

                //need to do absolute url for facebook to show a diff image
                if (_show.ShowImageUrl != null && _show.ShowImageUrl.Trim().Length > 0)
                {
                    //make the image a full path
                    string imgPath = string.Format("http{0}://{1}{2}", (_page.Request.IsSecureConnection) ? "s" : string.Empty, _Config._DomainName, _show.ShowImageUrl);
                    AddMetaTagToPage(MakeNewMeta("property", "og:image", imgPath));
                }
                else if (_Config._SiteImageUrl != null && _Config._SiteImageUrl.Trim().Length > 0)
                    AddMetaTagToPage(MakeNewMeta("property", "og:image", _Config._SiteImageUrl));
                
                if (_show.VenueRecord.Latitude != null && _show.VenueRecord.Latitude.Trim().Length > 0)
                    AddMetaTagToPage(MakeNewMeta("property", "og:latitude", _show.VenueRecord.Latitude));

                if (_show.VenueRecord.Longitude != null && _show.VenueRecord.Longitude.Trim().Length > 0)
                    AddMetaTagToPage(MakeNewMeta("property", "og:longitude", _show.VenueRecord.Longitude));

                if (_show.VenueRecord.Address != null && _show.VenueRecord.Address.Trim().Length > 0)
                    AddMetaTagToPage(MakeNewMeta("property", "og:street-address", _show.VenueRecord.Address));

                if (_show.VenueRecord.City != null && _show.VenueRecord.City.Trim().Length > 0)
                    AddMetaTagToPage(MakeNewMeta("property", "og:locality", _show.VenueRecord.City.Trim()));

                if (_show.VenueRecord.State != null && _show.VenueRecord.State.Trim().Length > 0)
                    AddMetaTagToPage(MakeNewMeta("property", "og:region", _show.VenueRecord.State.Trim()));

                if (_show.VenueRecord.ZipCode != null && _show.VenueRecord.ZipCode.Trim().Length > 0)
                    AddMetaTagToPage(MakeNewMeta("property", "og:postal-code", _show.VenueRecord.ZipCode.Trim()));

                if (_show.VenueRecord.Country != null && _show.VenueRecord.Country.Trim().Length > 0)
                    AddMetaTagToPage(MakeNewMeta("property", "og:country-name", _show.VenueRecord.Country.Trim()));

                System.Web.UI.WebControls.Literal nl = new System.Web.UI.WebControls.Literal();
                nl.Text = Environment.NewLine;
                this.Page.Header.Controls.Add(nl);

                //HtmlLink l = new HtmlLink();
                //l.Href = _page.Request.Url.ToString();//.RawUrl;
                //l.Attributes.Add("rel", "canonical");
                //_page.Header.Controls.Add(l);

                System.Web.UI.WebControls.Literal n2 = new System.Web.UI.WebControls.Literal();
                n2.Text = Environment.NewLine;
                this.Page.Header.Controls.Add(n2);
            }
        }

        /// <summary>
        /// must use this in the render event
        /// </summary>
        /// <param name="control"></param>
        /// <returns></returns>
        public string GetRenderedControl(System.Web.UI.Control control)
        {
            if(control == null)
                return null;

            System.Web.UI.HtmlTextWriter writer = new System.Web.UI.HtmlTextWriter(new System.IO.StringWriter());
            control.RenderControl(writer);
            return writer.InnerWriter.ToString();
        }

        public bool IsPageAdminContext { get { return this.MasterPageFile != null && 
            (this.MasterPageFile.IndexOf("TemplateAdmin") != -1 || this.MasterPageFile.IndexOf("TemplateBlank") != -1); } }

        public bool isFinalOrderFlowPage
        {
            get
            {
                return ((this.ToString().ToLower() == "asp.store_shipping_aspx" && (Ctx.Cart.HasMerchandiseItems_Shippable || Ctx.Cart.HasTicketItems_CurrentlyShippable)) ||
                    (this.ToString().ToLower() == "asp.store_checkout_aspx" && ((!Ctx.Cart.HasMerchandiseItems_Shippable) && (!Ctx.Cart.HasTicketItems_CurrentlyShippable))));
            }
        }

        public string userWebInfo
        {
            get
            {
                if(Request != null)
                    return string.Format("{0}: User: {1} Url: {2} IP: {3} Agent: {4} Platform: {5} Browser: {6} {7} {8}.{9}",
                        DateTime.Now.ToString(), 
                        (this.User != null) ? User.Identity.Name.Replace("@", " @ ") : "user unknown",
                        Request.Url, Request.UserHostAddress, Request.UserAgent, Request.Browser.Platform,
                        Request.Browser.Browser, Request.Browser.Version, Request.Browser.MajorVersion,
                        Request.Browser.MinorVersion);
                else
                    return string.Format("{0}: User: {1} Request Object Unknown",
                        DateTime.Now.ToString(),(this.User != null) ? User.Identity.Name : "user unknown");
            }
        }

        protected override void OnError(EventArgs e)
        {
            base.OnError(e);

            Exception objError = Server.GetLastError().GetBaseException();

            if (objError != null)
            {
                Wcss._Error.LogException(objError);

                string err = string.Format("Error Caught in Application_Error event\nError in: {0}\nError Message: {1}\nStack Trace: {2}",
                    Request.Url.ToString(), objError.Message.ToString(), (objError.StackTrace != null) ? objError.StackTrace.ToString() : string.Empty);

                Ctx.CurrentPageException = err;

                if(this.MasterPageFile == null || this.MasterPageFile.IndexOf("Admin") == -1)
                    this.Redirect("/Error.aspx");
            }
        }

        public void QualifySsl(bool sslRequired)
        {
            if (sslRequired && !Request.IsSecureConnection)
            {
                try
                {
                    string uri = Request.Url.AbsoluteUri.Replace("http://", "https://");
                    
                    this.Redirect(uri);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
            else if ((!sslRequired) && Request.IsSecureConnection)
            {
                try
                {
                    string uri = Request.Url.AbsoluteUri.Replace("https://", "http://");
                    
                    this.Redirect(uri);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        public void Redirect( string url)
        {
            try
            {
                Response.Redirect(url, true);
            }
            catch (System.Threading.ThreadAbortException) { }
            catch (Exception ex)
            {
                _Error.LogException(ex);
            }
        }

        public void RedirectControl( string controlPathAndQuery)
        {
            try
            {
                Response.Redirect( Request.FilePath + "?p=" + controlPathAndQuery);
            }
            catch (System.Threading.ThreadAbortException) { }
            catch (Exception ex)
            {
                _Error.LogException(ex);
            }
        }

        public void RedirectToDefault()
        {
            try
            {
                Response.Redirect(Request.FilePath);
            }
            catch (System.Threading.ThreadAbortException) { }
            catch (Exception ex)
            {
                _Error.LogException(ex);
            }
        }

        public static bool DisplayRightContent(System.Web.UI.Page page)
        {
            return (page.ToString().ToLower() == "asp.store_choosemerch_aspx" || 
                page.ToString().ToLower() == "asp.store_chooseticket_aspx" ||
                page.ToString().ToLower() == "asp.contact_aspx" ||
                page.ToString().ToLower() == "asp.contactsuccess_aspx" ||
                page.ToString().ToLower() == "asp.store_confirmation_aspx" || 
                page.ToString().ToLower() == "asp.search_aspx" || 
                page.ToString().ToLower() == "asp.store_about_aspx" || 
                page.ToString().ToLower() == "asp.faq_aspx" || 
                page.ToString().ToLower() == "asp.store_eventcalendar_aspx");
        }

        public bool OldUserMustUpdate(string userName)
        {
            bool retVal = false;

            //check to see if this is an OLD account
            //get the username and compare to old customer table
            //if there is a match and the match has not yet been updated - go to update page
            Ctx.OldUser = AspnetUsersOld.GetOldUser(userName);

            //email users will be null

            if (Ctx.OldUser != null && (!Ctx.OldUser.ProfileHasBeenUpdated))
            {
                //redirect to account update page
                Ctx.RedirectOnAuth = System.Web.Security.FormsAuthentication.GetRedirectUrl(userName, false);
                retVal = true;
                Redirect("~/AccountUpdate.aspx");
            }
            else
                Ctx.OldUser = null;

            return retVal;
        }

        /// <summary>
        /// indicates the most lax of admin access - for non customers
        /// </summary>
        public bool IsAuthdAdminUser
        {
            get
            {
                System.Security.Principal.IPrincipal user = HttpContext.Current.User;

                if (user.Identity.IsAuthenticated &&
                    (user.IsInRole("_Master") ||
                    user.IsInRole("Super") ||
                    user.IsInRole("Administrator") ||
                    user.IsInRole("Manifester") ||
                    user.IsInRole("MassMailer") ||
                    user.IsInRole("OrderFiller") ||
                    user.IsInRole("ReportViewer") ||
                    user.IsInRole("ContentEditor")))
                {
                    Session.Add("AuthdCKUser", true);
                    return true;
                }

                return false;
            }
        }

        private AdminContext _atx;
        public AdminContext Atx
        {
            get
            {
                if (this.IsAuthdAdminUser)
                {
                    if (_atx == null)
                        _atx = new AdminContext();

                    return _atx;
                }

                return null;
            }
            set
            {
                if (this.IsAuthdAdminUser)
                    _atx = value;
            }
        }

        

        private	WebContext _ctx;
        public WebContext Ctx
        {
            get
            {
                if(_ctx == null)
                    _ctx = new WebContext();

                _ctx.IsAuthd_CKFinder = this.IsAuthdAdminUser;

                return _ctx;
            }
            set
            {
                _ctx = value;
            }
        }

        protected void LoadContextualStylesheets()
        {
            //merch styles
            if (this.ToString().ToLower().IndexOf("choosemerch_aspx") != -1 || this.ToString().ToLower().IndexOf("display_merch_aspx") != -1
                || this.ToString().ToLower().IndexOf("showmerch_aspx") != -1 || this.ToString().ToLower().IndexOf("imagepopup_aspx") != -1)
            {
                AddHeaderControlToPage(string.Format("<link href=\"/Styles/{0}/merchitems.css\" type=\"text/css\" rel=\"stylesheet\" />", _Config._Site_Entity_Name));
            }

            //cartstyles
            if (this.ToString().ToLower().IndexOf("cart_edit_aspx") != -1 || this.ToString().ToLower().IndexOf("checkout_aspx") != -1 ||
                this.ToString().ToLower().IndexOf("confirmation_aspx") != -1 || this.ToString().ToLower().IndexOf("printconfirm_aspx") != -1 ||
                this.ToString().ToLower().IndexOf("shipping_aspx") != -1)
            {
                AddHeaderControlToPage(string.Format("<link href=\"/Styles/{0}/cartcheckout.css\" type=\"text/css\" rel=\"stylesheet\" />", _Config._Site_Entity_Name));
            }

            //aux styles
            if (this.ToString().ToLower().IndexOf("webuser_default_aspx") != -1 || this.ToString().ToLower().IndexOf("editprofile_aspx") != -1 ||
                this.ToString().ToLower().IndexOf("contact_aspx") != -1 || this.ToString().ToLower().IndexOf("store_about_aspx") != -1 ||
                this.ToString().ToLower().IndexOf("store_maintenance_aspx") != -1 || this.ToString().ToLower().IndexOf("accessdenied_aspx") != -1 ||
                this.ToString().ToLower().IndexOf("accountupdate_aspx") != -1 || this.ToString().ToLower().IndexOf("charitableorgs_aspx") != -1 ||
                this.ToString().ToLower().IndexOf("contactsuccess_aspx") != -1 || this.ToString().ToLower().IndexOf("error_aspx") != -1 ||
                this.ToString().ToLower().IndexOf("faq_aspx") != -1 || this.ToString().ToLower().IndexOf("mailerconfirm_aspx") != -1 ||
                this.ToString().ToLower().IndexOf("mailermanage_aspx") != -1 || this.ToString().ToLower().IndexOf("passwordrecovery_aspx") != -1 ||
                this.ToString().ToLower().IndexOf("passwordrecoverysuccess_aspx") != -1 || this.ToString().ToLower().IndexOf("register_aspx") != -1 ||
                this.ToString().ToLower().IndexOf("search_aspx") != -1 || this.ToString().ToLower().IndexOf("unsubscribe_aspx") != -1)
            {
                AddHeaderControlToPage(string.Format("<link href=\"/Styles/{0}/auxpages.css\" type=\"text/css\" rel=\"stylesheet\" />", _Config._Site_Entity_Name));
            }
        }

        private void AddHeaderControlToPage(string txt)
        {
            System.Web.UI.WebControls.Literal nl = new System.Web.UI.WebControls.Literal();
            nl.Text = Environment.NewLine;
            this.Page.Header.Controls.Add(nl);

            System.Web.UI.WebControls.Literal script = new System.Web.UI.WebControls.Literal();
            script.Text = txt;
            this.Page.Header.Controls.Add(script);
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

        protected void Analytics_ResetCode()
        {
            if (Header != null)
            {
                string googleId = (Wcss._Config._DomainName != "localhost" && Wcss._Config._DomainName != "local.sts9.com" &&
                    this.Request.UserHostName != "localhost" && this.Request.UserHostName != "local.sts9.com" && this.Request.UserHostName != "127.0.0.l") ?
	                _Config._GoogleAnalyticsId : "UA-000000-0";
	            System.Web.UI.WebControls.Literal lit = (System.Web.UI.WebControls.Literal)Header.FindControl("litGoogleAnalytics");
            
                if (lit != null && googleId != null && googleId.Trim().Length > 0)
                {
                    lit.Text = string.Empty;
                }
            }
        }

        protected void Analytics_AppendCode(System.Text.StringBuilder sb)
        {
            if (Header != null)
            {
                string googleId = (Wcss._Config._DomainName != "localhost" && Wcss._Config._DomainName != "local.sts9.com" &&
                    this.Request.UserHostName != "localhost" && this.Request.UserHostName != "local.sts9.com" && this.Request.UserHostName != "127.0.0.l") ?
	                _Config._GoogleAnalyticsId : "UA-000000-0";
	            System.Web.UI.WebControls.Literal lit = (System.Web.UI.WebControls.Literal)Header.FindControl("litGoogleAnalytics");

                if (lit != null && googleId != null && googleId.Trim().Length > 0)
                {
                    lit.Text += sb.ToString();
                }
            }
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

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            //PCI compliance
            //https://msdn.microsoft.com/en-us/library/ms972969.aspx
            //prevent one-click attacks
            ViewStateUserKey = Session.SessionID;

            //set page title
            if (Header != null)
                Header.Title = string.Format("{0} - {1}", _Config._SiteTitle, (Header.Title == null) ? string.Empty : Header.Title);
            
            LoadContextualStylesheets();
            
            //google analytics
            //bool isAna = Analytics_OpenCode();
            if (Analytics_OpenCode())
                Analytics_CloseCode();
        }

        protected override void OnPreInit(EventArgs e)
        {
            //dont disrupt a transaction in progress
            string page = this.ToString().ToLower();

            if (_Config._MaintenanceMode_On && page.IndexOf("processingorder") == -1 && page.IndexOf("register_aspx") == -1 &&
                page.IndexOf("confirmation_aspx") == -1 && page.IndexOf("controls_jpegimage_aspx") == -1)
            {
                if (!HttpContext.Current.User.IsInRole("Administrator"))
                {
                    Ctx.OrderProcessingVariables = null;
                    Redirect("~/Store/Maintenance.aspx");
                }
            }

            //remove theming for admin pages and others!
            if (
                
                this.ToString().ToLower() == "asp.store_cart_merchbundle_aspx" || 

                (this.MasterPageFile != null &&
                    (this.MasterPageFile.ToLower().IndexOf("responsiveflow.master") != -1 ||                     
                    this.MasterPageFile.ToLower().IndexOf("templateadmin.master") != -1 || 
                    this.ToString().ToLower() == "asp.admin_mailerviewer_aspx" || 
                    (this.MasterPageFile.ToLower().IndexOf("templateprint.master") != -1) && this.Page.ToString().ToLower().IndexOf("printconfirm_aspx") == -1))
                
                )
            {
                this.Theme = string.Empty;

                object obj = this.Header;
            }
            else
                this.Theme = _Config._ThemeFolder;

            //set marketing program key
            string mpk = Request.QueryString["mp"];
            if (mpk != null)
                Ctx.MarketingProgramKey = mpk;

            //show id
            string sid = Request.QueryString["sid"];
            if (sid != null && sid.Trim().Length <= 8 && Utils.Validation.IsInteger(sid.Trim()))
            {
                Globals.ShowId = int.Parse(sid);
            }
            else
            {
                Globals.ShowId = 0;
            }

            //determine ticket id
            string stid = Request["shite"];
            if (stid != null && stid.Trim().Length <=8 && Utils.Validation.IsInteger(stid.Trim()))
            {
                Globals.TicketId = int.Parse(stid.Trim()); //sets ticket item
            }
            else
            {
                Globals.TicketId = 0;
            }

            //determine month selected
            string num = Request["mo"];
            if (num != null && num.Trim().Length > 0)
            {
                try
                {
                    //Month numbers are 1-12
                    Globals.MonthSelected = DateTime.Parse(num.Trim().Replace("_", "/"));
                }
                catch (Exception)
                {
                    Globals.MonthSelected = Utils.Constants._MinDate;
                }
            }
            else//set to datetime.now for current month
                Globals.MonthSelected = Utils.Constants._MinDate;//set for current month on first entry to page

            //determine merch categorie
            string cat = Request["cat"];
            if (cat != null && Utils.Validation.IsInteger(cat))
                Globals.MerCat = (MerchCategorie)_Lookits.MerchCategories.Find(int.Parse(cat));
            else
                Globals.MerCat = null;

            //determine merch id
            string mrc = Request["mite"];
            if (mrc != null && Utils.Validation.IsInteger(mrc.Trim()))
            {
                int childId = int.Parse(mrc.Trim());
                Merch child = (Merch)Ctx.SaleMerch.Find(childId);
                if (child != null)
                {
                    Globals.MerchId = (child.IsParent) ? child.Id : child.TParentListing.Value;
                    Globals.MerchItem = (Merch)Ctx.SaleMerch.Find(Globals.MerchId);
                }
            }
            else
            {
                Globals.MerchId = 0;
                Globals.MerchItem = null;
            }

            base.OnPreInit(e);
        }
        public virtual void PageLogic()		{ }
        public virtual void PageInit()		{ }
        private void InitializeComponent()	
        {
        }

        protected void OnPageInit(object sender, System.EventArgs e)
        {
            try
            {
                this.PageInit();
            }
            catch(ThreadAbortException)
            {
                // do not log thread abort exceptions that occur on response.redirect
            }
            catch(Exception ex)
            {
                Wcss._Error.LogException(ex);
                Debug.WriteLine( ex.Message);
                Debug.Indent();
                Debug.WriteLine( ex.StackTrace);
                Debug.Unindent();
            }
        }

        protected void OnPageLoad(object sender, System.EventArgs e)
        {
            try
            {
                Utils.Helper.SetInputControlsHighlight(this, "highlight", false);

                this.PageLogic();
            }
            catch(ThreadAbortException)
            {
                // do not log thread abort exceptions that occur on response.redirect
            }
            catch(Exception ex)
            {
                Wcss._Error.LogException(ex);
                Debug.WriteLine( ex.Message);
                Debug.Indent();
                Debug.WriteLine( ex.StackTrace);
                Debug.Unindent();
            }
        }
    }
}
