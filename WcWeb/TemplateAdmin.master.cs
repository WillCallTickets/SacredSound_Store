using System;
using System.Web;
using System.Web.UI.HtmlControls;

namespace WillCallWeb
{
    public partial class TemplateAdmin : System.Web.UI.MasterPage
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

            LoadContextualStylesheets();
            
            HtmlGenericControl editor = new HtmlGenericControl("script");
            editor.Attributes["type"] = "text/javascript";
            editor.Attributes["src"] = string.Format("/{0}/ckeditor/ckeditor.js", Wcss._Config._VirtualResourceDir);
            Page.Header.Controls.Add(editor);
            HtmlGenericControl finder = new HtmlGenericControl("script");
            finder.Attributes["type"] = "text/javascript";
            finder.Attributes["src"] = string.Format("/{0}/ckfinder/ckfinder.js", Wcss._Config._VirtualResourceDir);
            Page.Header.Controls.Add(finder);
        }
        protected void LoadContextualStylesheets()
        {
            System.Web.UI.WebControls.Literal link = new System.Web.UI.WebControls.Literal();
            link.Text = string.Format("<link href=\"/Styles/admin.css\" type=\"text/css\" rel=\"stylesheet\">", Wcss._Config._Site_Entity_Name);
            this.Page.Header.Controls.Add(link);        
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            adminbody.Attributes.Add("class", string.Format("{0}_bg", Wcss._Config._ThemeFolder.ToLower()));
            settheme.Attributes.Add("class", string.Format("{0}_thm", Wcss._Config._ThemeFolder.ToLower()));

            if (this.Page.User.Identity.IsAuthenticated && this.Page.User.IsInRole("Administrator"))
                this.Session["IsAdmin"] = true;
        }
    }
}