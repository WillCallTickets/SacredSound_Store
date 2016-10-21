using System;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Xml;

namespace WillCallWeb
{
    public partial class FeedShow : System.Web.UI.Page
    {
        protected override void OnPreInit(EventArgs e)
        {
            this.Theme = string.Empty;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Clear();
            Response.ContentType = "application/atom+xml";
            Response.ContentEncoding = Encoding.UTF8;

            SyndicationFeed feed = WillCallWeb.SyndicationHelper.GetSyndicationHelper("show");

            if (feed != null && feed.Items.Count() > 0)
            {
                Atom10FeedFormatter formatter = new Atom10FeedFormatter(feed);

                XmlWriter writer = XmlWriter.Create(Response.Output, null);

                formatter.WriteTo(writer);

                writer.Flush();
            }
        }
    }
}