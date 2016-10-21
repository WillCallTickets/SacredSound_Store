using System;
using System.Web.UI.WebControls;

using Wcss;
using WillCallWeb.StoreObjects;

namespace WillCallWeb.Controls
{
    public partial class Login_Mini : WillCallWeb.BaseControl
    {
        protected void Page_Load(object sender, EventArgs e) {}

        protected void linkLogout_Click(object sender, EventArgs e)
        {
            //if we are on a secure page - redirect to home page after logout
            Ctx.LogoutUser();

            //Server.Transfer does not work if a page is ssl and going to non-ssl, etc 081106
            //Server.Transfer("~/Store/ChooseTicket.aspx?sid=0");

            //base.Redirect("~/Store/ChooseTicket.aspx?sid=0");

            //redirect to a default page
            string landing = Wcss._Config._LandingPageUrl;
            if (landing == string.Empty)
                Response.Redirect("/Store/ChooseTicket.aspx?sid=0", true);
            else
                Response.Redirect(landing, true);
        }
        protected void linkRegister_Click(object sender, EventArgs e)
        {
            //redirect to register page - be sure to include current url for redirecturl
            base.Redirect(string.Format("~/Register.aspx?ReturnUrl={0}", System.Web.HttpUtility.UrlEncode(this.Request.RawUrl)));
        }
    }
}