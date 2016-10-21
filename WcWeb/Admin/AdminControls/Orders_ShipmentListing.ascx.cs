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
    public partial class Orders_ShipmentListing : BaseControl
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

                InvoiceShipment _shipment = (InvoiceShipment)e.Row.DataItem;

                TextBox actual = (TextBox)e.Row.FindControl("txtActual");
                if (actual != null)
                    actual.Text = (_shipment == null) ? string.Empty : _shipment.ShippingActual.ToString("n2");

                if (_shipment != null)
                {
                    Literal ship = (Literal)e.Row.FindControl("litShipped");

                    if (ship != null)
                        ship.Text = string.Format("{0}", (_shipment.DateShipped < DateTime.MaxValue) ?
                            string.Format("<div style=\"font-weight: bold;\">Shipped On</div><div>{0}</div>",
                            _shipment.DateShipped.ToString("MM/dd/yyyy hh:mmtt")) : string.Empty);

                    Literal address = (Literal)e.Row.FindControl("litAddress");

                    if (address != null)
                        address.Text = string.Format("{0}<br/>{1} {2}<br/>{3} {4} {5} {6} {7} {8}<br/>{9}",
                            _shipment.InvoiceRecord.AspnetUserRecord.UserName,
                            _shipment.FirstName, _shipment.LastName,
                            _shipment.Address1, _shipment.Address2, _shipment.City, _shipment.StateProvince, _shipment.PostalCode,
                            _shipment.Country, _shipment.Phone);

                    Literal pack = (Literal)e.Row.FindControl("litPacking");

                    if (pack != null)
                        pack.Text = string.Format("<div>{0}</div>{1}", _shipment.PackingList.Replace("~", "</div><div>"),
                            (_shipment.PackingAdditional != null && _shipment.PackingAdditional.Trim().Length > 0) ?
                            string.Format("<div>{0}</div>", _shipment.PackingAdditional.Trim()) : string.Empty);
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

            if (list.Items.Count > 0 && list.SelectedIndex == -1)
                list.SelectedIndex = 0;//default to merch
        }

        protected void DataListContext_SelectedIndexChanged(object sender, EventArgs e)
        {
            //reset selected page as we don't know if counts will match between pages in context
            GooglePager1.PageIndex = 0;
            GridView1.PageIndex = GooglePager1.PageIndex;
        }

        #endregion

        #region Other Buttons

        //protected void btnRefresh_Click(object sender, EventArgs e)
        //{
        //    //GridView1.DataBind();
        //}

        protected void btnActual_Click(object sender, EventArgs e)
        {
            //go thru rows and save shipment amounts
            GridView grid = GridView1;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            foreach (GridViewRow gvr in grid.Rows)
            {
                TextBox actual = (TextBox)gvr.FindControl("txtActual");
                if (actual != null)
                {
                    string input = actual.Text.Trim();
                    if (!Utils.Validation.IsDecimal(input))
                    {
                        actual.ForeColor = System.Drawing.Color.Red;

                        //CustomValidation.IsValid = false;
                        lblError.Text = "Please enter a valid amount.";
                        lblError.Visible = true;
                        return;
                    }

                    int idx = (int)grid.DataKeys[gvr.RowIndex].Value;

                    sb.AppendFormat("UPDATE InvoiceShipment SET mShippingActual = {0} ", decimal.Parse(input));
                    sb.AppendFormat("WHERE [Id] = {0}; ", idx);
                }
            }

            if (sb.Length > 0)
            {
                try
                {
                    SubSonic.DataService.ExecuteQuery(new SubSonic.QueryCommand(sb.ToString(), SubSonic.DataService.Provider.Name));
                    GridView1.DataBind();
                }
                catch (Exception ex)
                {
                    _Error.LogException(ex);
                    lblError.Text = ex.Message;
                    lblError.Visible = true;
                }
            }
        }

        #endregion

    }
}