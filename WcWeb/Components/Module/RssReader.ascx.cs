using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Data;

using Wcss;

namespace WillCallWeb.Components.Module
{
    public partial class RssReader : WillCallWeb.BaseControl
    {
        public string RssUrl
        {
            get { return lnkRss.NavigateUrl; }
            set
            {
                string url = value;
                if(value.StartsWith("/") || value.StartsWith("~/"))
                {
                    char[] chars = { '~', '/' };
                    url = string.Format("http://{0}/{1}", _Config._DomainName, url.TrimStart(chars));
                }

                lnkRss.NavigateUrl = url;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {   
            if (this.RssUrl.Length == 0)
                throw new ApplicationException("The RSSUrl cannot be null.");

            try
            {
                //create a datatable and fill it with RSS data
                //then bind to the repeater
                XmlDataDocument feed = new XmlDataDocument();
                feed.Load(this.RssUrl);
                XmlNodeList posts = feed.GetElementsByTagName("item");

                DataTable table = new DataTable("Feed");
                table.Columns.Add("Title", typeof(string));
                table.Columns.Add("Link", typeof(string));
                table.Columns.Add("Description", typeof(string));

                foreach (XmlNode post in posts)
                {
                    DataRow row = table.NewRow();
                    row["Title"] = post["title"].InnerText;
                    row["Link"] = post["link"].InnerText;
                    row["Description"] = post["description"].InnerText.Trim();
                    table.Rows.Add(row);
                }

                lstRss.DataSource = table;
                lstRss.DataBind();
            }
            catch (Exception)
            {
                this.Visible = false;
            }
        }
    }
}