using System;
using System.IO;
using System.Xml;

using Wcss;

namespace WillCallWeb.Controls
{
    public partial class Maintenance : System.Web.UI.UserControl
    {   
        protected void Page_Load(object sender, EventArgs e)
        {
            //TODO: create a cache dependency

            if (!IsPostBack)
            {
                //get the page from the resources dir
                XmlDocument doc = new XmlDocument();
                doc.XmlResolver = null;

                try
                {
                    string mappedPath = Server.MapPath(string.Format("/WillCallResources/Html/Maintenance.html"));
                    doc.Load(mappedPath);
                }
                catch (Exception ex)
                {
                    _Error.LogException(ex);
                    return;
                }

                XmlNodeList xlist = doc.GetElementsByTagName("title");
                if (xlist.Count > 0)
                {
                    string title = xlist[0].InnerXml;
                    this.Page.Title = title;
                }

                XmlNodeList xbody = doc.GetElementsByTagName("body");
                if (xbody.Count > 0)
                {
                    string body = xbody[0].InnerXml;
                    this.litBody.Text = body;
                }
            }
        }
    }
}