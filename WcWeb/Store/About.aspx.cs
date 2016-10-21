using System;


//<% @ OutputCache Duration="60" VaryByParam="p" %>

public partial class Store_About : WillCallWeb.BasePage
{
    protected override void OnPreInit(EventArgs e)
    {
        QualifySsl(false);
        base.OnPreInit(e);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        //avoid double clicks! always reset
        Ctx.OrderProcessingVariables = null;

        if (!IsPostBack)
            SetPageControl();
    }

    private void SetPageControl()
    {
        //SET UP PAGE BASED UPON QS
        string controlToLoad = string.Empty;
        string req = Request.QueryString["p"];

        if (req != null && req.Trim().Length > 0)
            controlToLoad = req;

        //use separate files to utlize caching
        switch (controlToLoad.ToLower())
        {
            case "privacy":
                controlToLoad = "PrivacyPolicy";
                break;
            case "terms":
                controlToLoad = "TermsOfSale";
                break;
        }

        Content.Controls.Add(LoadControl(string.Format(@"..\controls\{0}.ascx", controlToLoad)));
    }
}
