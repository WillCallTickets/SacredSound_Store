using System;


/// <summary>
/// This page is reponsible for redirecting legacy links to the proper new control
/// </summary>
public partial class Index2 : System.Web.UI.Page
{
    protected override void OnPreInit(EventArgs e)
    {
        //not necessary as page just redirects
        //QualifySsl(false);
        base.OnPreInit(e);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        string idx = Request.QueryString["ShowDisplayId"];

        if (idx != null && Utils.Validation.IsInteger(idx))
        {
            Response.Redirect(string.Format("/Store/ChooseTicket.aspx?sid={0}", idx), true);
        }

        //redirect to a default page
        string landing = Wcss._Config._LandingPageUrl;
        if(landing == string.Empty)
            Response.Redirect("/Store/ChooseTicket.aspx?sid=0", true);
        else
            Response.Redirect(landing, true);
    }
}