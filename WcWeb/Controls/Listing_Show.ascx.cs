using System;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Linq;

using Wcss;
using Utils;
using Utils.ExtensionMethods;

namespace WillCallWeb.Controls
{
    public partial class Listing_Show : WillCallWeb.BaseControl
    {
        protected string _monthName = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack || base.IsPageAdminContext)
                BindList();
        }

        private void BindList()
        {
            DateTime month = Globals.MonthSelected;
            _monthName = (month == Utils.Constants._MinDate) ?
                (_Config._Site_Entity_Mode == _Enums.SiteEntityMode.Act) ? string.Format("{0} Tour Dates", _Config._Site_Entity_Name.ToUpper())
                : string.Format("Upcoming Events") //_Config._SiteTitle.ToUpper()
                : month.ToString("MMMM &#39;yy");

            if (Ctx.SaleShowDates_All != null && Ctx.SaleShowDates_All.Count > 0)
            {
                //get displayable show dates
                ShowDateCollection baseColl = new ShowDateCollection();
                baseColl.AddRange(Ctx.OrderedDisplayable_ShowDates);

                //if we have a date range selected....
                List<ShowDate> showDateColl = (month == Utils.Constants._MinDate) ? baseColl.GetList() :
                    showDateColl = baseColl.GetList().FindAll(
                        delegate(ShowDate match) { return (match.DateOfShow.ToString("MM/yy") == month.ToString("MM/yy")); }
                    );
                
                if (showDateColl.Count == 0)
                {
                    if (month == Utils.Constants._MinDate)
                        lblEmptyData.Text = string.Format("<div class=\"noneavailable\">Sorry there are no shows on sale online at this time.<br/>Please check back soon.</div>");
                    else
                        lblEmptyData.Text = string.Format("<div class=\"noneavailable\">Sorry there are no shows scheduled for {0} at this time.<br/>Please check back soon.</div>",
                            month.ToString("MMMM"));

                    rptShows.DataSource = new List<ShowDate>();
                }
                else
                {
                    if (showDateColl.Count > 1)
                        showDateColl.Sort(new Reflector.CompareEntities<ShowDate>(Reflector.Direction.Ascending, "DtDateOfShow"));

                    //remove entries with dupe shows - keep the lowest date
                    List<ShowDate> filteredByShow = new List<ShowDate>();
                    foreach(ShowDate sd in showDateColl)
                    {
                        int idx = filteredByShow.FindIndex(delegate(ShowDate match) { return (sd.TShowId == match.TShowId); });
                        if (idx == -1)
                            filteredByShow.Add(sd);
                    }

                    if (filteredByShow.Count > 1)
                        filteredByShow.Sort(new Reflector.CompareEntities<ShowDate>(Reflector.Direction.Ascending, "DtDateOfShow"));

                    rptShows.DataSource = filteredByShow;
                }                
            }
            else 
                rptShows.DataSource = new List<ShowDate>();
            
            rptShows.DataBind();
        }

        private string SetLink(Show sho, string mktKey)//, bool isStart)
        {
            //bool isStart = false;
            bool dayOfShow = sho.AllShowsAreDayOfShowOrPast;
            bool soldOut = (sho.IsSoldOut || sho.IsVirtuallySoldOut);
            string userName = (this.Profile != null && (! this.Profile.IsAnonymous)) ? this.Profile.UserName : null;
            string externalTixUrl = (sho.ExternalTixUrl != null && sho.ExternalTixUrl.Trim().Length > 0) ? sho.ExternalTixUrl.Trim() : string.Empty;




            //This page will always show that the ticket is onsale at the onsale date - ticket page will deal with valid keys
            //this is because customer will be directly linked to ticket page, bypassing this page
            ShowTicketCollection availableShowTickets = new ShowTicketCollection();
            availableShowTickets.AddRange(sho.GetDisplayableTickets(_Enums.VendorTypes.online, mktKey, true, false).GetList().FindAll(
                delegate(ShowTicket match)
                {
                    return (match.IsAvailableForListing(_Enums.VendorTypes.online, mktKey, true, false, availableShowTickets) == true) &&
                    ((bool)RequiredShowTicketPastPurchase.ScanPurchaseRequirements(userName, match).First == true && 
                    (! match.IsOverrideSellout));
                }));

            //these are put here so that the literal  can contain whitespace - the css whizte-space property is pre - preserves whitespace
            string moreInfo     = " More   Info ";
            string buyTickets   = "Buy Tickets";
            string tickets = (externalTixUrl.Length > 0) ? "Tickets" : null;
           
            if (soldOut)//sold out is already displayed above
            {
                return string.Format("<span class=\"buylink\"><a href=\"/Store/ChooseTicket.aspx?sid={0}\" class=\"btntribe\" title=\"more information\">{1}</a></span>",
                    sho.Id, "Sold Out!");
            }
            else if (tickets != null)
            {
                return string.Format("<span class=\"buylink\"><a href=\"{0}\" target=\"_blank\" class=\"btntribe\" title=\"see tickets\">{1}</a></span>",
                    sho.ExternalTixUrl, tickets);
            }
            else if (availableShowTickets.Count > 0)
            {
                string availers = string.Empty;

                //FEATURE GOFAST
                //if we are using this feature - we have a threshhold and text
                //if (_Config._Inventory_Tickets_GoingFast_Threshhold > 0 && _Config._Inventory_Tickets_GoingFast_Text.Trim().Length > 0)
                //{
                //    int availableInventory = 0;

                //    foreach (ShowDate sd in sho.ShowDateRecords())
                //    {
                //        if (sd.IsActive)
                //        {
                //            int avail = 0;
                //            Ctx.ShowDateInventory.TryGetValue(sd.Id, out avail);
                //            availableInventory += avail;
                //        }
                //    }

                //    if (availableInventory > 0 && availableInventory < _Config._Inventory_Tickets_GoingFast_Threshhold)
                //        availers = string.Format("<span class=\"tktgofast\" >{0}</span>", _Config._Inventory_Tickets_GoingFast_Text.Trim());
                //}

                return string.Format("<span class=\"buylink\"><a href=\"/Store/ChooseTicket.aspx?sid={0}\" class=\"btntribe\" title=\"See tickets\">{1}</a></span>{2}",
                    sho.Id, buyTickets, (availers.Trim().Length > 0) ? availers.Trim() : string.Empty);
            }
            else if (sho.DateOnSale > DateTime.Now) //note that fix length here allows us to wrap the line because "onsale 3/13 12pm" does not fit on one line
                return string.Format("<span class=\"buylink\"><a href=\"/Store/ChooseTicket.aspx?sid={0}\" class=\"btntribe fixlength\" title=\"more information\" >{1}</a></span>",
                        sho.Id, string.Format("ONSALE {0} <span class=\"timezone\">{1}</span>", sho.DateOnSale.ToString("MM/dd h:mmtt"), System.TimeZoneInfo.Local.AbbreviateTimeZone()));//moreinfo
            else //we just dont have any tickets to show now
            {
                //we are trying to see if all of the dates have the same satus
                string allStatus = string.Empty;
                ShowDateCollection sdColl = new ShowDateCollection();
                sdColl.AddRange(sho.ShowDateRecords().GetList().FindAll(delegate(ShowDate match) { return (match.IsActive && match.DtDateOfShow.Date >= _Config.SHOWOFFSETDATE); }));
                foreach (ShowDate sd in sdColl)
                {
                    string stat = sd.ShowStatusRecord.Name;
                    if (allStatus.Length == 0)
                        allStatus = stat;
                    else if (allStatus != stat)
                    {
                        //if we have a diff status for oneof the shows..then reset
                        allStatus = string.Empty;
                        break;
                    }
                }

                //if the status is not onsale and isone we want to report......
                //cancelled, moved, pending, postponed
                if (allStatus.Length > 0 &&
                    (allStatus == _Enums.ShowDateStatus.Cancelled.ToString() || allStatus == _Enums.ShowDateStatus.Moved.ToString() ||
                    allStatus == _Enums.ShowDateStatus.Postponed.ToString()))
                    moreInfo = allStatus;
                else
                    allStatus = string.Empty;

                return string.Format("<span class=\"buylink\"><a href=\"/Store/ChooseTicket.aspx?sid={0}\" class=\"btntribe\" title=\"more information\" >{1}</a></span>",
                    sho.Id, moreInfo);
            }
        }

        protected void rptShows_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            ListItemType lit = (ListItemType)e.Item.ItemType;

            if (lit != ListItemType.Footer && lit != ListItemType.Header && lit != ListItemType.Pager && lit != ListItemType.Separator)
            {
                Repeater rpr = (Repeater)sender;
                ShowDate _showDate = (ShowDate)e.Item.DataItem;

                Literal title = (Literal)e.Item.FindControl("LiteralShowTitle");
                if (title != null && _showDate.ShowRecord.ShowTitle != null && _showDate.ShowRecord.ShowTitle.Trim().Length > 0)
                    title.Text = string.Format("<div class=\"showtitle\">{0}</div>", _showDate.ShowRecord.ShowTitle.Trim());

                Literal venue = (Literal)e.Item.FindControl("litVenue");
                if (venue != null)
                    venue.Text = _showDate.ShowRecord.DisplayVenue_Wrapped(true, true, false);

                Literal descripton = (Literal)e.Item.FindControl("LiteralDescription");
                if (descripton != null)
                {
                    if(_Config._Site_Entity_Mode != _Enums.SiteEntityMode.Act)
                        descripton.Text = string.Format("<div class=\"mainact\">{0}</div>", Utils.ParseHelper.HtmlEncode_Extended(_showDate.ShowRecord.CalculatedActName.Trim()));
                    if (_showDate.ShowRecord.MidText != null && _showDate.ShowRecord.MidText.Trim().Length > 0)
                        descripton.Text += string.Format("<div class=\"midtxt\">{0}</div>", _showDate.ShowRecord.MidText.Trim());
                    if (_showDate.ShowRecord.StatusText != null && _showDate.ShowRecord.StatusText.Trim().Length > 0)
                        descripton.Text += string.Format("<div class=\"status\">{0}</div>", _showDate.ShowRecord.StatusText.Trim());
                }

                Literal buylink = (Literal)e.Item.FindControl("LiteralBuyLink");
                if (buylink != null)
                    buylink.Text = this.SetLink(_showDate.ShowRecord, Ctx.MarketingProgramKey);
            }
        }
    }
}