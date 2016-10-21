using System;
using System.Collections.Generic;
using System.ServiceModel.Syndication;
using System.Text;

using Wcss;

/// <summary>
/// Summary description for SyndicationHelper
/// </summary>
namespace WillCallWeb
{
    public static class SyndicationHelper
    {
        public static SyndicationFeed GetSyndicationHelper(string context)
        {
            Uri uri = new Uri(string.Format("http://{0}", _Config._DomainName));
            
            string id = string.Format("{0} {1} {2}", _Config._SiteTitle, context, "feed");

            SyndicationFeed syndicationFeed = new SyndicationFeed(
                id.ToUpper(), //title
                string.Format("Listing for {0} {1}", _Config._SiteTitle, context), //description
                uri,
                id,
                DateTime.Now);
                        
            List<SyndicationItem> itemList = new List<SyndicationItem>();

            WebContext ctx = new WebContext();
            StringBuilder sb = new StringBuilder();

            foreach (ShowDate sd in ctx.OrderedDisplayable_ShowDates)
            {
                sb.Length = 0;
                Show _show = sd.ShowRecord;
                bool drt = _show.IsDisplayRichText;

                sb.Append("<div class=\"datefeed\">");

                //show the venue in different places depending on displaymode
                if(_Config._Site_Entity_Mode != _Enums.SiteEntityMode.Venue)
                    sb.AppendFormat(_show.DisplayVenue_Wrapped(false, false, false));

                if(_show.ShowTitle != null && _show.ShowTitle.Trim().Length > 0)
                    sb.AppendFormat("<span class=\"showtitle\">{0}</span>", _show.ShowTitle.Trim());

                //this contains pricing info as well
                if (!drt)
                    sb.Append(Utils.ParseHelper.ParseCommasAndAmpersands(_show.wc_DisplayShowTimes(false, true)));

                if (_show.StatusText != null && _show.StatusText.Trim().Length > 0)
                    sb.AppendFormat("<span class=\"status\">{0}</span>", _show.StatusText.Trim());
                
                if ((! drt) && _show.cartPromoter != null && _show.cartPromoter.Trim().Length > 0)
                    sb.AppendFormat("<span class=\"promoter\">{0}</span>", _show.cartPromoter.Trim());

                if (_show.TopText != null && _show.TopText.Trim().Length > 0)
                    sb.AppendFormat("<span class=\"toptxt\">{0}</span>", _show.TopText.Trim());

                if (!drt)
                {
                    string allacts = string.Empty;
                    if (_show.OverrideActBilling)
                        sb.AppendFormat("<div class=\"actlist\">{0}</div>", (_show.ActBilling == null || _show.ActBilling.Trim().Length == 0) ?
                            string.Empty : _show.ActBilling.Trim());
                    else
                    {
                        string heads = _show.listHeadliners;
                        if (heads != null && heads.Trim().Length > 0)
                            sb.AppendFormat("<div class=\"mainact\">{0}</div>", heads);

                        string opens = _show.wc_DisplayOpeners;
                        if (opens != null && opens.Trim().Length > 0)
                            sb.AppendFormat("<span class=\"openerlist\">{0}</span>", opens.Trim());
                    }
                }

                //show links - not necessary for syndication

                //writeup
                if (_show.BotText != null && _show.BotText.Trim().Length > 0)
                    sb.AppendFormat("<div class=\"showdescription\">{0}</div>", _show.BotText.Trim());


                sb.Append("</div>");

                
                SyndicationItem oItem = new SyndicationItem();
                oItem.AddPermalink(new Uri(string.Format("http://{0}/Store/ChooseTicket.aspx?sid={1}", _Config._DomainName, sd.TShowId.ToString())));
                oItem.Title = SyndicationContent.CreatePlaintextContent(string.Format("{0} {1}", sd.DateOfShow.ToString("MM/dd/yyyy hh:mmtt"), sd.ShowRecord.ShowNamePart));
                oItem.BaseUri = new Uri(string.Format("http://{0}/Store/ChooseTicket.aspx?sid={1}", _Config._DomainName, sd.TShowId.ToString()));
                oItem.Content = SyndicationContent.CreateHtmlContent(Utils.ParseHelper.ParseCommasAndAmpersands(sb.ToString().Trim()));

                itemList.Add(oItem);
            }


            syndicationFeed.Items = itemList;
            return syndicationFeed;
        }
    }
}