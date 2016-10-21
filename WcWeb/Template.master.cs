using System;
using System.Web;
using System.Web.UI.WebControls;

using Wcss;

//<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">
//<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">

namespace WillCallWeb
{
    public partial class TemplateMaster : System.Web.UI.MasterPage
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

            //take care of home link
            LiteralHomelink.Text = string.Format("<div id=\"homelink-container\"><a href=\"{0}\" title=\"{1}\" id=\"homelink\"></a></div>", 
                Utils.ParseHelper.FormatUrlFromString(_Config._Site_Entity_HomePage), string.Format("{0} Home", _Config._Site_Entity_Name));

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (! IsPostBack)
            {
                WebContext ctx = new WebContext();

                string result = ctx.Cart.RemoveAnyExpiredItemsFromCart();

                if (result != null && result.Trim().Length > 0)
                {
                    ValidationSummary1.HeaderText = "<b>The following items were removed from your cart:</b>";
                    RemovalValidator.IsValid = false;
                    RemovalValidator.ErrorMessage = result;
                }
            }
        }
    }
}