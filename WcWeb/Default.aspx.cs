using System;

namespace WillCallWeb
{
    public partial class _Default : WillCallWeb.BasePage
    {
        protected override void OnPreInit(EventArgs e)
        {
            //not necessary as all this page does is redirect
            //QualifySsl(false);
            base.OnPreInit(e);
        }
        protected override void OnInit(EventArgs e)
        {   
            //redirect to a default page
            string landing = Wcss._Config._LandingPageUrl;
            if (landing == string.Empty)
                Response.Redirect("/Store/ChooseTicket.aspx?sid=0", true);
            else
                Response.Redirect(landing, true);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }
    }
}