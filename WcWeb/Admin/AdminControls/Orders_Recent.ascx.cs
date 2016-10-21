using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using Wcss;

namespace WillCallWeb.Admin.AdminControls
{
    /// <summary>
    /// NOTE THAT CLOCKS ARE REVERSE!!! because we view the search starting from now and going until then(end)
    /// </summary>
    public partial class Orders_Recent : BaseControl
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
            GooglePager1.GooglePagerChanged += new WillCallWeb.Components.Navigation.gglPager.GooglePagerChangedEvent(GooglePager1_GooglePagerChanged);
            base.Dispose();
        }
        protected void GooglePager1_GooglePagerChanged(object sender, WillCallWeb.Components.Navigation.gglPager.GooglePagerEventArgs e)
        {
            Atx.adminPageSize = e.NewPageSize;
            GridView1.PageIndex = e.NewPageIndex;
            GridView1.PageSize = Atx.adminPageSize;
        }
        //Sync with grid
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

        protected override void OnLoad(EventArgs e)
        {
            if (!IsPostBack)
            {
                DataListContext.DataBind();    
                EnsureContextChoice();
            }
        }

        #region Calendars

        protected void clock_Init(object sender, EventArgs e)
        {
            WillCallWeb.Components.Util.CalendarClock cal = (WillCallWeb.Components.Util.CalendarClock)sender;

            if (cal.ID.ToLower().IndexOf("start") != -1)
                cal.SelectedDate = DateTime.Now.AddMonths(-6);
            else
                cal.SelectedDate = DateTime.Now.Date.AddDays(1).AddMinutes(-1);

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

            GooglePager1.PageIndex = 0;
            GridView1.PageIndex = GooglePager1.PageIndex;
        }
        private void EnsureValidCalendarSelection()
        {
            DateTime startClock = clockStart.SelectedDate;
            DateTime endClock = clockEnd.SelectedDate;

            if (endClock < startClock)
                clockEnd.SelectedDate = startClock.AddDays(1);
        }

        #endregion

        #region GridView1

        protected int _rowCounter = 0;
        protected void GridView1_DataBinding(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;
            _rowCounter = grid.PageSize * grid.PageIndex;
        }
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                _rowCounter += 1;

                Literal rowCounter = (Literal)e.Row.FindControl("LiteralRowCounter");
                if (rowCounter != null)
                    rowCounter.Text = _rowCounter.ToString();

                //set up link to purchased items
                CustomerInvoiceRow entity = (CustomerInvoiceRow)e.Row.DataItem;
                Literal desc = (Literal)e.Row.FindControl("literalDescription");

                if (entity != null)
                {
                    Invoice _invoice = Invoice.FetchByID(entity.InvoiceId);

                    if (desc != null)
                        desc.Text = Invoice.InterpretProductDescription(this.Page.User.IsInRole("Administrator"),
                            _invoice, Atx.SaleTickets, Atx.SaleMerch);

                    Literal ship = (Literal)e.Row.FindControl("litShipping");

                    if (ship != null)
                        ship.Text = _invoice.ShippingStatus_PostSale;
                }
            }
        }
        #endregion

        #region Context

        protected void DataListContext_DataBinding(object sender, EventArgs e)
        {
            DataList list = (DataList)sender;

            if (list.Items.Count == 0)
            {
                List<ListItem> coll = new List<ListItem>();
                foreach (string s in Enum.GetNames(typeof(_Enums.ProductContext)))
                    coll.Add(new ListItem(s));

                list.DataSource = coll;
            }
        }

        private void EnsureContextChoice()
        {            
            DataList list = DataListContext;

            switch (Ctx.Orders_RecentSort)
            {   
                case _Enums.ProductContext.merch:
                    if (list.SelectedIndex != 1)
                    {
                        list.SelectedIndex = -1;
                        list.SelectedIndex = 1;
                    }
                    break;
                case _Enums.ProductContext.ticket:
                    if (list.SelectedIndex != 2)
                    {
                        list.SelectedIndex = -1;
                        list.SelectedIndex = 2;
                    }
                    break;
                default:
                    list.SelectedIndex = -1;
                    list.SelectedIndex = 0;
                    break;
            }
        }

        protected void DataListContext_SelectedIndexChanged(object sender, EventArgs e)
        {
            //reset this as we don't know if counts will match between pages in context
            DataList list = (DataList)sender;

            Ctx.Orders_RecentSort = (_Enums.ProductContext)Enum.Parse(typeof(_Enums.ProductContext), list.SelectedValue.ToString(), true);

            GooglePager1.PageIndex = 0;
            GridView1.PageIndex = GooglePager1.PageIndex;
        }

        #endregion

        #region Other Buttons

        //protected void btnRefresh_Click(object sender, EventArgs e)
        //{
        //    //GridView1.DataBind();
        //}

        #endregion

    }
}