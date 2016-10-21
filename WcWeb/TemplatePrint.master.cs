using System;
using System.Web;

namespace WillCallWeb
{
    public partial class TemplatePrint : System.Web.UI.MasterPage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            Response.Expires = -300;
            Response.ExpiresAbsolute = DateTime.Now.AddDays(-1);
            Response.CacheControl = "no-cache";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.Now.AddDays(-1));
            Response.AddHeader("pragma", "no-cache");
        }
        protected void Page_Load(object sender, EventArgs e)
        {
        }
    }
}