using System;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using Wcss;
using Wcss.QueryRow;

namespace WillCallWeb.Admin.AdminControls
{
    public partial class Reports_InventoryTickets : BaseControl
    {
        #region New paging

        bool isSelectCount;
        protected void objData_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            isSelectCount = e.ExecutingSelectCount;

            if (!isSelectCount)
            {
                e.Arguments.StartRowIndex = (GridView1.PageIndex * GridView1.PageSize) + 1;
                e.Arguments.MaximumRows = GridView1.PageSize;
            }
        }
        protected void objData_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (isSelectCount && e.ReturnValue != null && e.ReturnValue.GetType().Name == "Int32")
            {
                GooglePager1.DataSetSize = (int)e.ReturnValue;
            }
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            GooglePager1.GooglePagerChanged += new WillCallWeb.Components.Navigation.gglPager.GooglePagerChangedEvent(GooglePager1_GooglePagerChanged);
        }
        public override void Dispose()
        {
            GooglePager1.GooglePagerChanged -= new WillCallWeb.Components.Navigation.gglPager.GooglePagerChangedEvent(GooglePager1_GooglePagerChanged);
            base.Dispose();
        }
        protected void GooglePager1_GooglePagerChanged(object sender, WillCallWeb.Components.Navigation.gglPager.GooglePagerEventArgs e)
        {
            Atx.adminPageSize = e.NewPageSize;
            GridView1.PageIndex = e.NewPageIndex;
            GridView1.PageSize = Atx.adminPageSize;

            //GridView1.DataBind();
        }
        protected void GridView1_Init(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;
            GooglePager1.PageSize = Atx.adminPageSize;
            grid.PageSize = GooglePager1.PageSize;
            grid.PageIndex = GooglePager1.PageIndex;
        }
        protected void GridView1_DataBound(object sender, EventArgs e)
        {
            GooglePager1.DataBind();
        }

        #endregion

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            //set opacity for nav events
            if (this.HasControls() && this.UpdatePanel1.Visible)
                Atx.RegisterJQueryScript_BlockUI_AjaxMethod(this.UpdatePanel1, "#report", true);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!IsPostBack)
            {
                GridView1.DataBind();
            }
        }
        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            //GridView1.DataBind();
        }

        #region GridView1

        protected void GridView1_DataBinding(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;
        }
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridView grid = (GridView)sender;
            string cmd = e.CommandName.ToLower();
            switch (cmd)
            {
                case "sync":
                    int ticketidx = int.Parse(e.CommandArgument.ToString());
                    SPs.TxInventorySyncSold(ticketidx, true, _Enums.InvoiceItemContext.ticket.ToString()).Execute();
                    grid.DataBind();
                    break;
            }
        }
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridViewRow gvr = e.Row;

            InventoryDiscrep_TicketRow entity = (InventoryDiscrep_TicketRow)gvr.DataItem;

            if (entity != null)
            {
                HyperLink showDate = (HyperLink)gvr.FindControl("linkShowDate");
                HyperLink ticket = (HyperLink)gvr.FindControl("linkTicket");
                Literal status = (Literal)gvr.FindControl("litStatus");
                Literal offsale = (Literal)gvr.FindControl("litOffsale");

                if (showDate != null)
                {
                    showDate.Text = string.Format("{0} {1}", entity.ShowDate.ToString("MM/dd/yyyy hh:mmtt"), Show.ParseShowNamePart(entity.ShowName));
                    showDate.NavigateUrl = string.Format("/Admin/Listings.aspx?p=tickets&shodateid={0}", entity.ShowDateId);
                }
                if (ticket != null)
                {
                    ticket.NavigateUrl = string.Format("/Admin/Listings.aspx?p=tickets&tixid={0}", entity.ShowTicketId);
                    if(ShowTicket.IsCampingPass(string.Format("{0} {1}", entity.SalesDescription ?? string.Empty, entity.CriteriaText ?? string.Empty).Trim()))
                        ticket.Text = "CAMPING";
                }
                if (status != null && entity.Status != null && entity.Status.Trim().Length > 0)
                    status.Text = string.Format("<div><b>Status: &nbsp;</b>{0}</div>", entity.Status.Trim());
                if (offsale != null)
                {
                    offsale.Text = string.Format("<div>On:{0}</div>", (entity.OnSaleDate.Year > 2000) ? entity.OnSaleDate.ToString("MM/dd/yy hh:mmtt") : string.Empty);
                    offsale.Text += string.Format("<div>Off:{0}</div>", (entity.OffSaleDate.Year < DateTime.MaxValue.Year) ? entity.OffSaleDate.ToString("MM/dd/yy hh:mmtt") : string.Empty);
                }

                Literal purchasedDisc = (Literal)gvr.FindControl("litPurchased");
                Literal refundDisc = (Literal)gvr.FindControl("litRefund");

                Button btnSync = (Button)gvr.FindControl("btnSync");
                //visibility is also controlled in ASPX by role
                if (btnSync != null)
                    btnSync.Visible = (entity.SoldDisc);

                if (purchasedDisc != null)
                    purchasedDisc.Text = string.Format("<div{0}>{1}</div>", (entity.SoldDisc) ?
                        " style=\"background-color: red; color: white;\"" : string.Empty, entity.PurchasedActual);
                if (refundDisc != null)
                    refundDisc.Text = string.Format("<div{0}>{1}</div>", (entity.RefundDisc) ?
                        " style=\"background-color: red; color: white;\"" : string.Empty, entity.RemovedActual);

                Literal litDescCrit = (Literal)e.Row.FindControl("litDescCrit");
                if (litDescCrit != null)
                {
                    string criteria = entity.CriteriaText;
                    string description = entity.SalesDescription;

                    if (criteria != null && criteria.Trim().Length > 40)
                        criteria = string.Format("{0}...", criteria.Trim().Substring(0, 38).Trim());
                    if (description != null && description.Trim().Length > 40)
                        description = string.Format("{0}...", description.Trim().Substring(0, 38).Trim());

                    string theDescription = string.Format("{0}{1}",
                        (description != null && description.Trim().Length > 0) ? string.Format("<div>{0}</div>", description.Trim()) : string.Empty,
                        (criteria != null && criteria.Trim().Length > 0) ? criteria.Trim() : string.Empty);

                    if (theDescription.Trim().Length > 0)
                        litDescCrit.Text = theDescription.Trim();
                }


                Literal tix = (Literal)gvr.FindControl("litTix");
                Literal fee = (Literal)gvr.FindControl("litFee");
                Literal tot = (Literal)gvr.FindControl("litTotal");

                if (tix != null && fee != null && tot != null)
                {
                    ShowTicket t = null;
                    
                    t = (ShowTicket)Atx.SaleTickets.Find(entity.ShowTicketId);

                    if (t == null)
                        t = (ShowTicket)Wcss.ShowTicket.FetchByID(entity.ShowTicketId);

                    bool doit = false;

                    if(t != null && t.IsPackage)
                    {
                        ShowTicketCollection coll = new ShowTicketCollection();
                        coll.AddRange(t.LinkedShowTickets);
                        coll.Add(t);
                        if (coll.Count > 1)
                            coll.Sort("DtDateOfShow", true);

                        if(t.Id == coll[0].Id)
                            doit = true;
                    }

                    if (t != null && ((!t.IsPackage) || doit))
                    {
                        decimal tick = decimal.Round(entity.Price * entity.Sold, 2);
                        decimal fees = decimal.Round(entity.ServiceCharge * entity.Sold, 2);
                        decimal all = tick + fees;

                        _totalTix += tick;
                        _totalFee += fees;
                        _totalAll += all;

                        tix.Text = tick.ToString("n");
                        fee.Text = fees.ToString("n");
                        tot.Text = all.ToString("n");
                    }
                }
            }
        }

        protected decimal _totalTix = 0;
        protected decimal _totalFee = 0;
        protected decimal _totalAll = 0;

        #endregion

        #region Calendars

        protected void clock_Init(object sender, EventArgs e)
        {
            WillCallWeb.Components.Util.CalendarClock cal = (WillCallWeb.Components.Util.CalendarClock)sender;

            if (cal.ID.ToLower().IndexOf("start") != -1)
                cal.SelectedDate = DateTime.Parse(string.Format("{0}/1/{1}", DateTime.Now.Month, DateTime.Now.Year));
            else
                cal.SelectedDate = (_Config._Display_DefaultToAllShowsInReports) ? DateTime.Now.AddYears(2) : DateTime.Now.AddMonths(3).Date;
        }
        protected void clock_SelectedDateChanged(object sender, WillCallWeb.Components.Util.CalendarClock.CalendarClockChangedEventArgs e)
        {
            WillCallWeb.Components.Util.CalendarClock cal = (WillCallWeb.Components.Util.CalendarClock)sender;

            if (cal.ID.ToLower().IndexOf("start") != -1)
                clockStart.SelectedDate = e.ChosenDate;
            else
                clockEnd.SelectedDate = e.ChosenDate;

            EnsureValidCalendarSelection();
        }
        private void EnsureValidCalendarSelection()
        {
            DateTime startClock = clockStart.SelectedDate;
            DateTime endClock = clockEnd.SelectedDate;

            if (endClock < startClock)
                clockEnd.SelectedDate = startClock.AddDays(1);
        }

        #endregion        
    }
}