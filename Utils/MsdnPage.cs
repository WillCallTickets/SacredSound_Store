using System;
using System.IO;
using System.Web.UI;
using System.Web;
using System.Text;
using System.Text.RegularExpressions;


namespace Utils
{
	public class MsdnPage : System.Web.UI.Page
	{
        // ***********************************************************
        // Ctor
        public MsdnPage()
        {
            //m_focusedControl = "";

            // Register a PreRender handler
            //this.PreRender += new EventHandler(RefreshPage_PreRender);
        }
        // ***********************************************************

		#region Constants

		// ***********************************************************
		// Constants
        //public const string RefreshTicketCounter = "RefreshTicketCounter";
        //private const string SetFocusFunctionName = "__setFocus";
        //private const string SetFocusScriptName = "__inputFocusHandler";
		
//		private WillCallBiz.SessionCookieManager _scm;
//		public WillCallBiz.SessionCookieManager Scm
//		{
//			get
//			{
//				if(_scm == null)
//				{
//					_scm = new WillCallBiz.SessionCookieManager(this.Request, this.Response);
//				}
//
//				return _scm;
//			}
//			set
//			{
//				_scm = value;
//			}
//		}

		#endregion

		#region Scroll Persistence
        //private bool _useScrollPersistence = true;
        ///// <summary>
        ///// There could be PostBack senarios where we do not want to remember the scroll position. Set this property to false
        ///// if you would like the page to forget the current scroll position
        ///// </summary>
        //public bool UseScrollPersistence
        //{
        //    get {return this._useScrollPersistence;}
        //    set {this._useScrollPersistence = value;}
        //}

        //private string _bodyID;
        ///// <summary>
        ///// Some pages might already have the ID attribute set for the body tag. Setting this property will not render the ID or change
        ///// the existing value. It will simply update the javascript written out to the browser.
        ///// </summary>
        //public string BodyID
        //{
        //    get {return this._bodyID;}
        //    set {this._bodyID = value;}
        //}

        //Last chance. Do we want to maintain the current scroll position
        //protected override void OnPreRender(EventArgs e)
        //{
        //    if(UseScrollPersistence)
        //    {
        //        RetainScrollPosition();
        //    }
        //    base.OnPreRender (e);
        //}        

        //protected override void Render(HtmlTextWriter writer)
        //{
        //    //No need processing the HTML if the user does not want to maintain scroll position or already has
        //    //set the body ID value
        //    if(UseScrollPersistence && BodyID == null)
        //    {
        //        TextWriter tempWriter = new StringWriter();
        //        base.Render(new HtmlTextWriter(tempWriter));
        //        writer.Write(Regex.Replace(tempWriter.ToString(),"<body","<body id=\"thebody\" ",RegexOptions.IgnoreCase));
        //    }
        //    else
        //    {
        //        base.Render(writer);
        //    }
        //}

        //private static string saveScrollPosition = "<script language='javascript'>function saveScrollPosition() {{document.forms[0].__SCROLLPOS.value = {0}.scrollTop;}}{0}.onscroll=saveScrollPosition;</script>";
        //private static string setScrollPosition = "<script language='javascript'>function setScrollPosition() {{{0}.scrollTop =\"{1}\";}}{0}.onload=setScrollPosition;</script>";

        //Write out javascript and hidden field
        //private void RetainScrollPosition()
        //{
        //    ClientScript.RegisterHiddenField("__SCROLLPOS", "0");
        //    string __bodyID = BodyID == null ? "thebody" : BodyID;
        //    ClientScript.RegisterStartupScript(this.GetType(),"saveScroll", string.Format(saveScrollPosition,__bodyID));

        //    if(Page.IsPostBack)
        //    {
        //        ClientScript.RegisterStartupScript(this.GetType(), "setScroll", string.Format(setScrollPosition, __bodyID, Request.Form["__SCROLLPOS"]));
        //    }
        //}
		#endregion

		// **************************************************************
//		// Set the control with the input focus
        //public void SetFocus(string ctlId)
        //{
        //    m_focusedControl = ctlId;
        //}
//		// **************************************************************


		#region Private Members

		// **************************************************************
		// Handle the PreRender event
        //private void RefreshPage_PreRender(object sender, EventArgs e)
        //{
        //    AddSetFocusScript();
        //}
		// **************************************************************

		// **************************************************************
		// Add any script code required for the SetFocus feature
        //private void AddSetFocusScript()
        //{
        //    if (m_focusedControl == "")
        //        return;

        //    // Add the script to declare the function
        //    // (Only one form in ASP.NET pages)
        //    StringBuilder sb = new StringBuilder("");
        //    sb.Append("<script language=javascript>");
        //    sb.Append("function ");
        //    sb.Append(SetFocusFunctionName);
        //    sb.Append("(ctl) {");
        //    sb.Append("  if (document.forms[0][ctl] != null)");
        //    sb.Append("  {document.forms[0][ctl].focus();}");
        //    sb.Append("}");

        //    // Add the script to call the function
        //    sb.Append(SetFocusFunctionName);
        //    sb.Append("('");
        //    sb.Append(m_focusedControl);
        //    sb.Append("');<");
        //    sb.Append("/");   // break like this to avoid misunderstandings...
        //    sb.Append("script>");

        //    // Register the script (names are CASE-SENSITIVE)
        //    if (!ClientScript.IsStartupScriptRegistered(SetFocusScriptName))
        //        ClientScript.RegisterStartupScript(this.GetType(), SetFocusScriptName, sb.ToString());
        //}
		// **************************************************************

		#endregion


		#region Private Properties
		// ***********************************************************
		// Private properties
		//private string m_focusedControl;
		// ***********************************************************
		#endregion
	}
}
