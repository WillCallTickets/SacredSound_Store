using System;
using System.Threading;

public partial class ContactProcessing : WillCallWeb.BasePage
{
    protected override void OnPreInit(EventArgs e)
    {
        QualifySsl(false);
        base.OnPreInit(e);
    }
    protected void Page_Load(object sender, EventArgs e)
    {   
        if ((Request == null || Request.UrlReferrer == null) ||
            Request.UrlReferrer.ToString().ToLower().IndexOf("contact.aspx") == -1)
            base.Redirect("/Contact.aspx");
    }
}
