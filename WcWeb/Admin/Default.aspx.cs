using System;

namespace WillCallWeb.Admin
{
    public partial class _Default : WillCallWeb.BasePage
    {
        protected override void OnPreInit(EventArgs e)
        {
            QualifySsl(true);
            base.OnPreInit(e);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.User.IsInRole("Administrator") || this.User.IsInRole("OrderFiller") || this.User.IsInRole("Super"))
                base.Redirect("/Admin/Orders.aspx");
            else if (this.User.IsInRole("Manifester"))
                base.Redirect("/Admin/Listings.aspx?p=tickets");
            else if (this.User.IsInRole("MassMailer"))
                base.Redirect("/Admin/Mailers.aspx");
            else if (this.User.IsInRole("ReportViewer"))
                base.Redirect("/Admin/Reports.aspx");
            else
                base.Redirect("/ChooseTicket.aspx");
        }
    }
}