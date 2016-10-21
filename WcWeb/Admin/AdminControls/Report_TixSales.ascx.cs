using System;
using System.Web.UI.WebControls;

using Wcss;

namespace WillCallWeb.Admin.AdminControls
{
    public partial class Report_TixSales : BaseControl
    {
        #region New paging
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            GooglePager1.GooglePagerChanged += new WillCallWeb.Components.Navigation.gglPager.GooglePagerChangedEvent(GooglePager1_GooglePagerChanged);
        }
        public override void Dispose()
        {
            GooglePager1.GooglePagerChanged += new WillCallWeb.Components.Navigation.gglPager.GooglePagerChangedEvent(GooglePager1_GooglePagerChanged);
            base.Dispose();
        }
        protected void GooglePager1_GooglePagerChanged(object sender, WillCallWeb.Components.Navigation.gglPager.GooglePagerEventArgs e)
        {
            Atx.adminPageSize = e.NewPageSize;
            ddlVenue.DataBind();
        }

        #endregion

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            //set opacity for nav events
            if (this.HasControls() && this.UpdatePanel1.Visible)
                Atx.RegisterJQueryScript_BlockUI_AjaxMethod(this.UpdatePanel1, "#report", true);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlVenue.DataBind();
            }
        }
        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            ddlVenue.DataBind();
        }

        #region Listing

        private int _rowCount = 0;
        protected void Listing_Init(object sender, EventArgs e)
        {
            ListView listing = (ListView)sender;
            GooglePager1.PageSize = Atx.adminPageSize;
        }
        protected void Listing_DataBinding(object sender, EventArgs e)
        {
            _rowCount = 0;

            Atx.CurrentReport_DatesTickets = Wcss.QueryRow.Report_DatesTickets.GetShowDatesInRange(
                int.Parse(ddlVenue.SelectedValue), clockStart.SelectedDate, clockEnd.SelectedDate, 
                (GooglePager1.PageIndex * GooglePager1.PageSize) + 1, GooglePager1.PageSize);

            if (Atx.CurrentReport_DatesTickets != null)
            {
                ListView listing = (ListView)sender;

                listing.DataSource = Atx.CurrentReport_DatesTickets.SimpleShowDateRecords;

                GooglePager1.DataSetSize = Wcss.QueryRow.Report_DatesTickets.GetShowDatesInRangeCount(
                    int.Parse(ddlVenue.SelectedValue), clockStart.SelectedDate, clockEnd.SelectedDate);
            }
        }
        protected void Listing_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                _rowCount++;

                Literal litRowStart = (Literal)e.Item.FindControl("litRowStart");
                if (litRowStart != null)
                    litRowStart.Text = string.Format("<tr{0}>", (_rowCount % 2 != 0) ? " class=\"alternaterow\"" : string.Empty);

                ListView listing = (ListView)sender;
                ListViewDataItem viewItem = (ListViewDataItem)e.Item;
                Wcss.QueryRow.SimpleShowDateListing ent = (Wcss.QueryRow.SimpleShowDateListing)viewItem.DataItem;

                //increment row counter here (if utilized)

                ShowTicketCollection coll = new ShowTicketCollection();
                coll.AddRange(Atx.CurrentReport_DatesTickets.TicketRecords.FindAll(delegate(ShowTicket match) 
                    { return (match.TShowDateId == ent.ShowDateId); } ));

                Literal descCell = (Literal)e.Item.FindControl("ShowDescriptionCell");
                if (ent != null && descCell != null)
                {
                    //3 is header-spacer-footer
                    int rowspan = (coll.Count == 0) ? 3 : coll.Count + 2;
                    descCell.Text = string.Format("<td class=\"jqshowsect roundtopleft\" rowspan=\"{0}\" style=\"text-align:left;font-weight:bold;\"><div class=\"\">{1}<br/>{2}<br/>{3}</div></td>",
                        rowspan.ToString(),//add extra for footer
                        ent.ShowDateId.ToString(),
                        ent.DateOfShow.ToString("MM/dd/yyyy hh:mmtt"), ent.ShowNamePart);
                }

                //if we have showtickets then bind the repeater
                Repeater tix = (Repeater)viewItem.FindControl("rptTix");
                tix.DataSource = coll;
                tix.DataBinding += new EventHandler(rptTix_DataBinding);
                tix.DataBind();
            }

        }
        protected void Listing_DataBound(object sender, EventArgs e)
        {
            GooglePager1.DataBind();
        }

        #endregion

        #region Venue Selection
        //needed to set up the binding order this way because posting back on a the first - data appended value - was not firing rebind
        protected void ddlVenue_DataBound(object sender, EventArgs e)
        {
            Listing.DataBind();
        }
        protected void ddlVenue_SelectedIndexChanged(object sender, EventArgs e)
        {
            GooglePager1.PageIndex = 0;
            Listing.DataBind();
        }
        protected void sql1_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters["@appId"].Value = Wcss._Config.APPLICATION_ID.ToString(); 
        }

        #endregion

        #region Calendars

        protected void clock_Init(object sender, EventArgs e)
        {
            WillCallWeb.Components.Util.CalendarClock cal = (WillCallWeb.Components.Util.CalendarClock)sender;

            if (cal.ID.ToLower().IndexOf("start") != -1)
                cal.SelectedDate = DateTime.Now.AddDays(-DateTime.Now.Day + 1 - 3).Date;//TODO remove the plus 3
            else
                cal.SelectedDate = DateTime.Now.Date.AddMonths(3).AddDays(1).AddMinutes(-1);

            cal.UseTime = false;
        }
        protected void clock_SelectedDateChanged(object sender, WillCallWeb.Components.Util.CalendarClock.CalendarClockChangedEventArgs e)
        {
            WillCallWeb.Components.Util.CalendarClock cal = (WillCallWeb.Components.Util.CalendarClock)sender;

            if (cal.ID.ToLower().IndexOf("start") != -1)
                clockStart.SelectedDate = e.ChosenDate;
            else
                clockEnd.SelectedDate = e.ChosenDate;

            EnsureValidCalendarSelection();

            Listing.DataBind();
        }
        private void EnsureValidCalendarSelection()
        {
            DateTime startClock = clockStart.SelectedDate;
            DateTime endClock = clockEnd.SelectedDate;

            if (endClock < startClock)
                clockEnd.SelectedDate = startClock.AddDays(1);
        }

        #endregion

        #region Repeat Tickets

        //i for per item - t for totals
        protected int _iAvail = 0;
        protected int _iPend = 0;
        protected int _iSold = 0;
        protected int _iRef = 0;
        protected decimal _iTotBase = 0;
        protected decimal _iTotSvc = 0;
        protected decimal _iTot = 0;
        private int _rowRptCount = 0;

        protected void rptTix_DataBinding(object sender, EventArgs e)
        {
            _iAvail = 0;
            _iPend = 0;
            _iSold = 0;
            _iRef = 0;
            _iTotBase = 0;
            _iTotSvc = 0;
            _iTot = 0;
            _rowRptCount = 0;
        }
        protected void rptTix_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Repeater rpt = (Repeater)sender;
            ListItemType lit = e.Item.ItemType;

            string rowStart = string.Format("<tr{0}>", (_rowRptCount++ % 2 != 0) ? " class=\"altgridrow\"" : string.Empty);

            if (lit != ListItemType.Footer && lit != ListItemType.Header && lit != ListItemType.Pager && lit != ListItemType.Separator)
            {
                ShowTicket ent = (ShowTicket)e.Item.DataItem;

                Literal litRowStart = (Literal)e.Item.FindControl("litRowStart");
                if (litRowStart != null)
                    litRowStart.Text = rowStart;
                

                //if the ticket is not a package
                // and if the ticket is a package and is the base ticket
                bool useTotals = ((!ent.IsPackage) || (ent.IsPackage && ent.IsBaseOfPackage));

                Literal litAges = (Literal)e.Item.FindControl("litAges");
                string age = ent.AgeDescription.Trim();
                switch(age.ToLower())
                {
                    case "all ages":
                        litAges.Text = "AA";
                        break;
                    case "limited all ages":
                        litAges.Text = "LimAA";
                        break;
                    default:
                        litAges.Text = (age.Length > 5) ? age.Substring(0,5).Trim() : age;
                        break;
                }

                Literal allTix = (Literal)e.Item.FindControl("litItemBase");
                Literal allSvc = (Literal)e.Item.FindControl("litItemSvc");
                Literal net = (Literal)e.Item.FindControl("litItemTotal");

                allTix.Text = string.Format("{0}", (useTotals) ? ((decimal)(ent.Price * ent.Sold)).ToString() : "--na--");
                allSvc.Text = string.Format("{0}", (useTotals) ? ((decimal)(ent.ServiceCharge * ent.Sold)).ToString() : "--na--");
                net.Text = string.Format("{0}", (useTotals) ? ((decimal)(ent.PerItemPrice * ent.Sold)).ToString("c") : "--na--");


                Literal avail = (Literal)e.Item.FindControl("litAvail");
                Literal pend = (Literal)e.Item.FindControl("litPend");


                Literal sold = (Literal)e.Item.FindControl("litSold");
                Literal price = (Literal)e.Item.FindControl("litPrice");
                Literal service = (Literal)e.Item.FindControl("litService");
                Literal per = (Literal)e.Item.FindControl("litPer");
                Literal refund = (Literal)e.Item.FindControl("litRefund");

                //if the ticket is a package - the only # to show is sales, so we know how many tix sold
                //ignore sales figures unless it is the base ticket

                //so if it is a package item - create a copy and fill it with zeros? or do we need to do that in the binding?
                //perhaps we can change it all by cell index?
                if (useTotals)
                {
                    _iAvail += ent.Available;
                    _iPend += ent.Pending;
                    _iSold += ent.Sold;
                    _iTotBase += (ent.Sold * ent.Price);
                    _iTotSvc += (ent.Sold * ent.ServiceCharge);
                    _iTot += (ent.Sold * ent.PerItemPrice);
                    _iRef += ent.Refunded;

                    avail.Text = ent.Available.ToString();
                    pend.Text = ent.Pending.ToString();
                    sold.Text = ent.Sold.ToString();
                    price.Text = ent.Price.ToString();
                    service.Text = ent.ServiceCharge.ToString();
                    per.Text = ent.PerItemPrice.ToString();
                    refund.Text = ent.Refunded.ToString();
                }
                else
                {
                    avail.Text = "--na--";
                    pend.Text = "--na--";
                    sold.Text = "--na--";
                    price.Text = "--na--";
                    service.Text = "--na--";
                    per.Text = "--na--";
                    refund.Text = "--na--";
                }
            }
            else if (lit == ListItemType.Footer)
            {   
                Literal litRowStart = (Literal)e.Item.FindControl("litRowStart");
                if (litRowStart != null)
                {
                    litRowStart.Text = string.Format("<tr class=\"footer{0}\">",
                        (_rowCount % 2 != 0) ? " alternaterow" : string.Empty);

                    //if there are no items - create a fake row for proper spacing?
                    //colspan is one less than total
                    if (rpt.Items.Count == 0)
                        litRowStart.Text = litRowStart.Text.Insert(0, string.Format("<tr class=\"{0}\"><td colspan=\"14\" style=\"text-align:left;padding-left:8px;\">no tickets</td></tr>",
                        (_rowCount % 2 != 0) ? " alternaterow" : string.Empty));
                }

                Literal litRowStartClose = (Literal)e.Item.FindControl("litRowStartClose");
                if (litRowStartClose != null)
                    litRowStartClose.Text = string.Format("<tr{0}>", (_rowCount % 2 != 0) ? " class=\"alternaterow\"" : string.Empty);// rowStart;

                //<asp:Literal ID="litCellCloser" runat="server" />

                Literal avail = (Literal)e.Item.FindControl("litAvail");
                Literal pend = (Literal)e.Item.FindControl("litPend");
                Literal sold = (Literal)e.Item.FindControl("litSold");
                Literal reff = (Literal)e.Item.FindControl("litRef");

                Literal totBase = (Literal)e.Item.FindControl("litTotBase");
                Literal totSvc = (Literal)e.Item.FindControl("litTotSvc");
                Literal tot = (Literal)e.Item.FindControl("litTot");

                avail.Text = _iAvail.ToString();
                pend.Text = _iPend.ToString();
                sold.Text = _iSold.ToString();
                reff.Text = _iRef.ToString();
                totBase.Text = _iTotBase.ToString("c");
                totSvc.Text = _iTotSvc.ToString("c");
                tot.Text = _iTot.ToString("c");
            }
        }

        #endregion
    }
}
//365 09/11/14