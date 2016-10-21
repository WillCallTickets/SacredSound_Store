using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Web;
using System.Text;

using Wcss;

namespace WillCallWeb
{
    public partial class FeedShow_Json : BasePage
    {
        protected override void OnPreInit(EventArgs e)
        {
            this.Theme = string.Empty;
        }

        ShowDateCollection dates = new ShowDateCollection();
        protected ShowDateCollection Dates
        {
            get
            {
                return dates;
            }
        }
        protected string _JSONtext = string.Empty;

        protected DateTime startDate = DateTime.Now;
        protected DateTime endDate = DateTime.Now;
        protected string domain = _Config._DomainName;

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.ContentEncoding = Encoding.UTF8;

            dates.CopyFrom(Ctx.OrderedDisplayable_ShowDates);

            if (dates.Count > 0)
            {
                startDate = Dates[0].DateOfShow;
                endDate = Dates[dates.Count - 1].DateOfShow;
            }

            ConstructJSON();

            Response.Write(_JSONtext);
        }
        
        public static char[] trimchars = { ' ', ',' };

        public void ConstructJSON()
        {
            StringBuilder sb = new StringBuilder();
            
            sb.Append("{");
            string title = string.Format("{0} Shows - {1} thru {2}",
                _Config._Site_Entity_Name, 
                startDate.ToString("MM/dd/yy"), 
                endDate.ToString("MM/dd/yy"));

            sb.AppendFormat("{0}, ", Utils.ParseHelper.ReturnJSONFormat("publishDateStamp", Utils.ParseHelper.ParseJSON(DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss"))));
            sb.AppendFormat("{0}, ", Utils.ParseHelper.ReturnJSONFormat("title", Utils.ParseHelper.ParseJSON(title)));
            sb.AppendFormat("{0}, ", Utils.ParseHelper.ReturnJSONFormat("homePage", _Config._Site_Entity_HomePage));
            sb.AppendFormat("{0}, ", Utils.ParseHelper.ReturnJSONFormat("copyright", string.Format("Copyright {0} {1}", 
                DateTime.Now.Year.ToString(), Utils.ParseHelper.ParseJSON(_Config._Site_Entity_Name))));
            sb.AppendFormat("{0}, ", Utils.ParseHelper.ReturnJSONFormat("siteImage", _Config._SiteImageUrl));

            sb.AppendFormat("\"showDates\": [ ");

            int i = 0;

            //loop thru showdates
            foreach(ShowDate sd in Dates)
            {
                sb.AppendFormat("{{ \"{0}\": \"{1}\", ", "showDate", Utils.ParseHelper.ParseJSON(sd.DateOfShow.ToString("MM/dd/yyyy")));
                sb.AppendFormat("{0}, ", Utils.ParseHelper.ReturnJSONFormat("title", 
                    string.Format("{0} {1}", 
                    Utils.ParseHelper.ParseJSON(sd.ShowRecord.Display.Headliners_NoMarkup_Verbose_NoLinks),
                    Utils.ParseHelper.ParseJSON(sd.ShowRecord.Display.Openers_NoMarkup_Verbose_NoLinks).Trim())));

                Venue v = new Venue();
                sd.ShowRecord.VenueRecord.CopyTo(v);

                if(v != null)
                {
                    string city = (v.City != null && v.City.Trim().Length > 0) ? v.City : string.Empty;
                    string state = (v.State != null && v.State.Trim().Length > 0) ? v.State : string.Empty;
                    string country = (v.Country != null && v.Country.Trim().Length > 0) ? v.Country : string.Empty;

                    string venue = string.Format("{0}{1}{2}{3}{4}{5}",
                        v.Name,
                        (city.Length > 0 || state.Length > 0 || country.Length > 0) ? string.Format(" - ") : string.Empty, //add a separator
                        (city.Length > 0) ? city : string.Empty, 
                        (city.Length > 0 && state.Length > 0) ? string.Format(", ") : string.Empty,//city state separator
                        (state.Length > 0) ? state : string.Empty,
                        (country.Length > 0) ? string.Format(" {0}", country) : string.Empty
                        );
                    
                    sb.AppendFormat("{0}, ", Utils.ParseHelper.ReturnJSONFormat("venue", Utils.ParseHelper.ParseJSON(venue)));
                }


                sb.AppendFormat("{0}, ", Utils.ParseHelper.ReturnJSONFormat("showImage",                     
                    (sd.ShowRecord.ShowImageUrl != null && sd.ShowRecord.ShowImageUrl.Trim().Length > 0) ? string.Format("http://{0}{1}", domain, sd.ShowRecord.ShowImageUrl.Trim()) : 
                    (_Config._SiteImageUrl != null && _Config._SiteImageUrl.Trim().Length > 0) ? _Config._SiteImageUrl.Trim() : 
                    string.Empty));

                sb.AppendFormat("{0}, ", Utils.ParseHelper.ReturnJSONFormat("siteEventUrl",
                    string.Format("http://{0}/Store/ChooseTicket.aspx?sid={1}", domain, sd.TShowId.ToString())));
                
                sb.AppendFormat("{0}, ", Utils.ParseHelper.ReturnJSONFormat("ticketLink",
                    string.Format("http://{0}/Store/ChooseTicket.aspx?sid={1}", domain, sd.TShowId.ToString())));

                sb.AppendFormat("{0}, ", Utils.ParseHelper.ReturnJSONFormat("showId", sd.TShowId.ToString()));
                sb.AppendFormat("{0}, ", Utils.ParseHelper.ReturnJSONFormat("showStatus", Utils.ParseHelper.ParseJSON(sd.ShowRecord.StatusText)));
                sb.AppendFormat("{0}, ", Utils.ParseHelper.ReturnJSONFormat("dateStatus", Utils.ParseHelper.ParseJSON(sd.StatusText)));
                sb.AppendFormat("{0}, ", Utils.ParseHelper.ReturnJSONFormat("showTitle", Utils.ParseHelper.ParseJSON(sd.ShowRecord.ShowTitle)));
                
                ////string presents = Utils.ParseHelper.ParseJSON(sd.ShowRecord.Display.Promoters_NoMarkup_NoLinks);
                ////presents = presents.Replace(@"'", @"\'");
                ////presents = presents.Replace(@"/", @"\/");
                
                sb.AppendFormat("{0}, ", Utils.ParseHelper.ReturnJSONFormat("presentedBy", Utils.ParseHelper.ParseJSON(sd.ShowRecord.Display.Promoters_NoMarkup_NoLinks)));
                sb.AppendFormat("{0}, ", Utils.ParseHelper.ReturnJSONFormat("header", Utils.ParseHelper.ParseJSON(sd.ShowRecord.TopText)));
                sb.AppendFormat("{0}, ", Utils.ParseHelper.ReturnJSONFormat("headliner", Utils.ParseHelper.ParseJSON(sd.ShowRecord.Display.Headliners_NoMarkup_Verbose_NoLinks)));
                sb.AppendFormat("{0}, ", Utils.ParseHelper.ReturnJSONFormat("opener", Utils.ParseHelper.ParseJSON(sd.ShowRecord.Display.Openers_NoMarkup_Verbose_NoLinks)));
                sb.AppendFormat("{0}, ", Utils.ParseHelper.ReturnJSONFormat("showNotes", Utils.ParseHelper.ParseJSON(sd.ShowRecord.DisplayNotes)));
                sb.AppendFormat("{0}, ", Utils.ParseHelper.ReturnJSONFormat("dateNotes", Utils.ParseHelper.ParseJSON(sd.DisplayNotes)));
                sb.AppendFormat("{0}, ", Utils.ParseHelper.ReturnJSONFormat("doorTime", sd.DateOfShow.ToString("hh:mmtt")));
                sb.AppendFormat("{0}, ", Utils.ParseHelper.ReturnJSONFormat("showTime", Utils.ParseHelper.ParseJSON(sd.ShowTime)));
                sb.AppendFormat("{0}, ", Utils.ParseHelper.ReturnJSONFormat("ages", Utils.ParseHelper.ParseJSON(sd.AgesString)));
                sb.AppendFormat("{0}, ", Utils.ParseHelper.ReturnJSONFormat("ticketPricing", Utils.ParseHelper.ParseJSON(sd.PricingText)));

                //do not substring the writeup until you can ensure that a tag will not be split
                //int descLength = 175;
                string writeup = Utils.ParseHelper.StripHtmlTags( Utils.ParseHelper.ParseJSON(sd.ShowRecord.BotText));
                //if (writeup.Length > 0 && writeup.Length > descLength)
                //    writeup = string.Format("{0}...", writeup.Substring(0, descLength));
                //writeup = writeup.Replace("\'", @"\'").Replace("\"", @"\""").Replace("/", @"\/");
                //be sure to check for texts that only have tags

                if (writeup != null && writeup.Trim().Length > 0)
                {
                    string stripped = Utils.ParseHelper.StripHtmlTags(writeup);

                    if (stripped.Trim().Length == 0)
                        writeup = string.Empty;
                }
                sb.AppendFormat("{0}, ", Utils.ParseHelper.ReturnJSONFormat("writeup", writeup));

                sb.AppendFormat("}}");
                sb.AppendFormat("{0} ", (i == (Dates.Count - 1)) ? string.Empty : ",");
                i++;
                //end showdates
            }

            sb.AppendFormat(" ] ");
            //sb.AppendFormat(" }})");
            
            sb.AppendFormat("}}");

            ////Response.Write(sb.ToString());
            _JSONtext = sb.ToString();
        }
}
}
