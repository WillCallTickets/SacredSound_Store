using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using Wcss;
using WillCallWeb;

public partial class Testing_Mine_TopLevelPage : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager scriptMgr = ScriptManager.GetCurrent(this.Page);

        if (IsAsync) { }
        if (scriptMgr.IsInAsyncPostBack)
        {
        }
    }
}
