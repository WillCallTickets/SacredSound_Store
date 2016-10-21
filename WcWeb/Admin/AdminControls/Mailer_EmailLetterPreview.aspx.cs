using System;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WillCallWeb.Admin.AdminControls
{
    public partial class Mailer_EmailLetterPreview : BasePage
    {
        protected override void OnPreInit(EventArgs e)
        {
            //base.OnPreInit(e);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            litEmailLetter.Text = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\"><html xmlns=\"http://www.w3.org/1999/xhtml\" ><head><title>Viewer</title></head><body></body></html>";

            if (Atx.CurrentEmailLetter != null && Atx.CurrentEmailLetter.HtmlVersion.StartsWith("/"))
            {
                try
                {
                    //string mappedFile = Server.MapPath(string.Format("/{0}/MailTemplates/CustomerServiceSent/{1}",
                    //        Wcss._Config._VirtualResourceDir, Atx.CurrentEmailLetter.Name));

                    string mappedFile = Server.MapPath(Atx.CurrentEmailLetter.HtmlVersion);
                    if (File.Exists(mappedFile))
                    {
                        string letter = Utils.FileLoader.FileToString(mappedFile);
                        litEmailLetter.Text = letter;
                    }
                }
                catch (Exception ) { }
            }
        }
    }
}
