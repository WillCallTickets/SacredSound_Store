using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;

using Wcss;
using Wcss.QueryRow;

namespace WillCallWeb.Admin.AdminControls.Reports
{
    public partial class TicketManifest : BaseControl
    {
        #region Props

        protected DateTime _listingDate
        {
            get
            {
                if (Session["ListingDate"] == null)
                    Session["ListingDate"] = DateTime.Parse(DateTime.Now.ToString("MM/1/yyyy"));

                return (DateTime)Session["ListingDate"];
            }
            set
            {
                Session.Remove("ListingDate");

                Session["ListingDate"] = value;
            }
        }
        protected ShowDate _showDate
        {
            get
            {
                if (Session["OrderEvent"] == null)
                    return null;

                return (ShowDate)Session["OrderEvent"];
            }
            set
            {
                Session.Remove("OrderEvent");

                if (value != null && value.ShowRecord.ApplicationId == _Config.APPLICATION_ID)
                    Session["OrderEvent"] = value;
            }
        }
        protected List<int> _selTix
        {
            get
            {
                if (Session["SelTix"] == null)
                {
                    Session.Add("SelTix", new List<int>());
                }

                return (List<int>)Session["SelTix"];
            }
            set
            {
                Session.Remove("SelTix");

                if (value != null && value.Count > 0)
                    Session["SelTix"] = value;
            }
        }
        protected string _currentTicketSelections
        {
            get
            {
                string tixList = string.Empty;

                if (_selTix.Count > 0)
                {
                    foreach (int ix in _selTix)
                        tixList += string.Format("{0}~", ix.ToString());

                    tixList.TrimEnd('~');
                }
                else
                    tixList = "0";

                return tixList;
            }
        }

        protected int _sold;
        protected int _allot;
        protected int _pend;
        protected int _ref;
        protected int _willCall;
        protected int _shipped;
        protected int _avail;
        protected int _willOrders;
        protected int _shipOrders;
        protected int _allOrders;
        protected decimal _base;
        protected decimal _fees;
        protected decimal _sales;

        protected int sel_sold;
        protected int sel_allot;
        protected int sel_pend;
        protected int sel_ref;
        protected int sel_willCall;
        protected int sel_shipped;
        protected int sel_avail;
        protected int sel_willOrders;
        protected int sel_shipOrders;
        protected int sel_allOrders;
        protected decimal sel_base;
        protected decimal sel_fees;
        protected decimal sel_sales;

        #endregion

        #region Page Overhead

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            System.Web.UI.HtmlControls.HtmlInputCheckBox chk = (System.Web.UI.HtmlControls.HtmlInputCheckBox)GooglePager1.FindControl("chkPhone");
            string script = "$('#" + chk.ClientID + "').on('click', function() { $('.phoner').toggle(); })";
            Atx.RegisterJQueryScript(this, script, false);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                _selTix = null;

                clockContext.SelectedValue = _listingDate.ToString();

                SetInputs();

                BindLists();
            }
        }

        protected void _packAllSelections()
        {
            _selTix.Clear();
            foreach (DataKey dk in GridTickets.DataKeys)
                _selTix.Add((int)dk.Value);
        }

        protected void EnableButtons()
        {
            bool enable = GridSales.PageCount > 0;

            Button btnprint = (Button)GooglePager1.FindControl("btnPrint");
            if (btnprint != null)
            {
                btnprint.Enabled = enable;
                btnprint.DataBind();
            }
            Button btncsv = (Button)GooglePager1.FindControl("btnCSV");
            if (btncsv != null)
            {
                btncsv.Enabled = enable;
                btncsv.DataBind();
            }
            Button btnemail = (Button)GooglePager1.FindControl("btnEmailList");
            if (btnemail != null)
                btnemail.Enabled = enable;
            Button btnbatch = (Button)GooglePager1.FindControl("btnBatch");
            if (btnbatch != null)
                btnbatch.Enabled = enable;

            System.Web.UI.HtmlControls.HtmlInputCheckBox chkPhone = (System.Web.UI.HtmlControls.HtmlInputCheckBox)GooglePager1.FindControl("chkPhone");
            if (chkPhone != null)
                chkPhone.Disabled = (!enable);
        }

        private void SetInputs()
        {
            //dates have precedence
            string req0 = Request["shodateid"];
            string req1 = Request["showid"];
            string req2 = Request["tixid"];

            if (req0 != null && Utils.Validation.IsInteger(req0) && (_showDate == null || _showDate.Id.ToString() != req0))
            {
                _showDate = ShowDate.FetchByID(req0);

                if (_showDate != null && _showDate.ShowRecord.ApplicationId != _Config.APPLICATION_ID)
                    _showDate = null;
            }

            if (req1 != null && Utils.Validation.IsInteger(req1) && (_showDate == null || _showDate.TShowId.ToString() != req1))
            {
                _showDate = null;

                Show show = Show.FetchByID(req1);
                if (show != null)
                {
                    if (show.ApplicationId == _Config.APPLICATION_ID)
                    {
                        List<ShowDate> lst = show.ShowDateRecords().GetList().FindAll(delegate(ShowDate match) { return (match.IsActive); });
                        if (lst.Count > 1)
                        {
                            ShowDateCollection coll = new ShowDateCollection();
                            coll.AddRange(lst);
                            if (coll.Count > 1)
                                coll.Sort("DtDateOfShow", true);
                            _showDate = coll[0];
                        }
                        else if (lst.Count == 1)
                            _showDate = lst[0];
                    }
                    else
                        show = null;
                }
            }

            if (req2 != null && req2 != "0" && Utils.Validation.IsInteger(req2))
            {
                //we will only get one selection via this method
                _selTix.Clear();
                ShowTicket st = ShowTicket.FetchByID(req2);

                if (st != null)
                {
                    //verify it belongs to the app
                    if (st.ShowDateRecord.ShowRecord.ApplicationId != _Config.APPLICATION_ID)
                        _showDate = null;
                    else if (_showDate == null || _showDate.Id != st.ShowDateRecord.Id)
                        _showDate = st.ShowDateRecord;
                }
            }

            //if the show date is not null and the calendar is not in range
            //reset the calendar to the showdate's date
            if (_showDate != null && _showDate.DateOfShow < clockContext.SelectedDate)
                clockContext.SelectedDate = _showDate.DateOfShow.Date;
        }

        private void BindLists()
        {
            rblShipContext.DataBind();
            lstSortContext.DataBind();
            ddlShowDates.DataBind();
        }

        protected void ResetAggregates()
        {

            _allot = 0;
            _pend = 0;
            _sold = 0;
            _willCall = 0;
            _willOrders = 0;
            _shipped = 0;
            _shipOrders = 0;
            _avail = 0;
            _allOrders = 0;
            _ref = 0;
            _base = 0;
            _fees = 0;
            _sales = 0;

            sel_allot = 0;
            sel_pend = 0;
            sel_sold = 0;
            sel_willCall = 0;
            sel_willOrders = 0;
            sel_shipped = 0;
            sel_shipOrders = 0;
            sel_avail = 0;
            sel_allOrders = 0;
            sel_ref = 0;
            sel_base = 0;
            sel_fees = 0;
            sel_sales = 0;
        }

        #endregion

        #region Other Buttons

        protected void clock_DateChange(object sender, EventArgs e)
        {
            _listingDate = clockContext.SelectedDate;
            ddlShowDates.Items.Clear();
            ddlShowDates.DataBind();
        }
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            if (_showDate != null)
            {
                Atx.SetCurrentShowRecord(_showDate.ShowRecord.Id);
                base.Redirect(string.Format("/Admin/ShowEditor.aspx?p=details"));
            }
        }
        protected void btnBatch_Click(object sender, EventArgs e)
        {
            base.Redirect("/Admin/Shipping_Tickets.aspx?p=batchview");
        }
        protected void btnCSV_DataBinding(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            //CheckBox chkPhone = (CheckBox)btn.NamingContainer.FindControl("chkPhone");
            //string phone = (chkPhone != null && chkPhone.Checked) ? "true" : "false";

            if (_showDate != null)
                btn.OnClientClick = string.Format("doPagePopup('/Admin/PrintTicketList_CSV_Download.aspx?date={0}&tik={1}&ctx={2}', 'true');",
                    _showDate.Id, _currentTicketSelections, this.ShipContext);
            else
                btn.OnClientClick = string.Empty;
        }
        protected void btnGetEmails_Click(object sender, EventArgs e)
        {
            //get & set the data
            Atx.CurrentDisplayList = TicketSalesRow.GetEmailOfTicketIdSales(
                (ddlShowDates.SelectedValue != null) ? int.Parse(ddlShowDates.SelectedValue) : 0,
                _currentTicketSelections,
                rblShipContext.SelectedValue,
                rdoPurchase.SelectedValue,
                lstSortContext.SelectedValue);

            if (Atx.CurrentDisplayList != null && Atx.CurrentDisplayList.Count > 0)
            {
                //do the popup
                string script = "doPagePopup('/Admin/DisplayList.aspx', 'true');";

                System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(),
                    Guid.NewGuid().ToString(), " ;" + script, true);
            }
        }
        protected void btnPrint_DataBinding(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            //CheckBox chkPhone = (CheckBox)btn.NamingContainer.FindControl("chkPhone");
            //string phone = (chkPhone != null && chkPhone.Checked) ? "true" : "false";

            if (_showDate != null)
                btn.OnClientClick = string.Format("doPagePopup('/Admin/PrintTicketList.aspx?date={0}&tik={1}&ctx={2}&purch={3}', 'true');",
                    _showDate.Id, _currentTicketSelections, this.ShipContext,
                    rdoPurchase.SelectedValue);
            else
                btn.OnClientClick = string.Empty;
        }
        //protected void chkPhone_Changed(object sender, EventArgs e)
        //{
        //    CheckBox chk = (CheckBox)sender;
        //    Button btnPrint = (Button)chk.NamingContainer.FindControl("btnPrint");
        //    if (btnPrint != null)
        //        btnPrint.DataBind();
        //    Button btnCSV = (Button)chk.NamingContainer.FindControl("btnCSV");
        //    if (btnCSV != null)
        //        btnCSV.DataBind();


        //}

        #endregion

        #region Paging

        bool isSelectCount;

        protected void objData_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            isSelectCount = e.ExecutingSelectCount;

            if (!isSelectCount)
            {
                e.Arguments.StartRowIndex = (GridSales.PageIndex * GridSales.PageSize) + 1;
                e.Arguments.MaximumRows = GridSales.PageSize;
            }

            e.InputParameters["showTicketIds"] = _currentTicketSelections;
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
            GridSales.PageIndex = e.NewPageIndex;
            GridSales.PageSize = Atx.adminPageSize;
        }
        //Sync with grid
        protected void GridSales_Init(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;
            GooglePager1.PageSize = Atx.adminPageSize;
            grid.PageSize = GooglePager1.PageSize;
            grid.PageIndex = GooglePager1.PageIndex;
        }
        protected void GridSales_DataBound(object sender, EventArgs e)
        {
            litSelection.DataBind();
            GooglePager1.DataBind();
            EnableButtons();
        }

        #endregion

        #region Event List

        protected void ddlShowDates_DataBound(object sender, EventArgs e)
        {
            DropDownList list = (DropDownList)sender;

            list.SelectedIndex = -1;

            ListItem liSelector = new ListItem("...Select An Event...", "0");

            if ((list.Items.Count == 0 || list.Items[0].Value != "") && (!list.Items.Contains(liSelector)))
                list.Items.Insert(0, liSelector);

            if (_showDate != null)
            {
                ListItem li = list.Items.FindByValue(_showDate.Id.ToString());

                if (li != null)
                    li.Selected = true;
            }
        }

        protected void ddlShowDates_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList list = (DropDownList)sender;

            int DateId = int.Parse(list.SelectedValue);

            ShowDate showDate = ShowDate.FetchByID(DateId);
            if (showDate != null)
                base.Redirect(string.Format("/Admin/Listings.aspx?p=tix&shodateid={0}", showDate.Id));
        }

        protected void SqlEventList_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters["@appId"].Value = Wcss._Config.APPLICATION_ID;
            e.Command.Parameters["@date"].Value = (clockContext.SelectedDate != clockContext.DefaultValue) ?
                clockContext.SelectedDate.ToString("MM/dd/yyyy") : DateTime.Now.ToString("MM/1/yyyy");
        }

        #endregion

        #region Grid Tickets

        protected void GridTickets_DataBinding(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;
            ResetAggregates();
        }

        protected void GridTickets_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridView grid = (GridView)sender;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView rowView = (DataRowView)e.Row.DataItem;
                DataRow row = rowView.Row;

                int idx = (int)row.ItemArray.GetValue(row.Table.Columns.IndexOf("Id"));

                //CheckBox chkOne = (CheckBox)e.Row.FindControl("chkOne");
                Literal offsale = (Literal)e.Row.FindControl("litOffsale");
                object offs = row.ItemArray.GetValue(row.Table.Columns.IndexOf("dtEndDate"));
                DateTime OffSaleDate = (offs == null || offs.ToString().Trim().Length == 0) ? DateTime.MaxValue : (DateTime)offs;
                if (offsale != null && OffSaleDate.Year < DateTime.MaxValue.Year)
                    offsale.Text = OffSaleDate.ToString("MM/dd/yyyy hh:mmtt");

                AggregateRowTotals(ref _allot, ref _pend, ref _sold,
                    ref _willCall, ref _willOrders,
                    ref _shipped, ref _shipOrders,
                    ref _avail, ref _allOrders, ref _ref,
                    ref _base, ref _fees, ref _sales,
                    rowView, row);

                //Selected totals
                if (_selTix.Count == 0 || _selTix.Contains(idx))
                {
                    AggregateRowTotals(ref sel_allot, ref sel_pend, ref sel_sold,
                        ref sel_willCall, ref sel_willOrders,
                        ref sel_shipped, ref sel_shipOrders,
                        ref sel_avail, ref sel_allOrders, ref sel_ref,
                        ref sel_base, ref sel_fees, ref sel_sales,
                        rowView, row);
                }

                Literal litDescCrit = (Literal)e.Row.FindControl("litDescCrit");
                if (litDescCrit != null)
                {
                    string criteria = row.ItemArray.GetValue(row.Table.Columns.IndexOf("CriteriaText")).ToString();
                    string description = row.ItemArray.GetValue(row.Table.Columns.IndexOf("SalesDescription")).ToString();

                    string desc = (criteria != null && criteria.Trim().Length > 0) ?
                        string.Format("<div>{0}</div>", Utils.ParseHelper.StripHtmlTags(criteria.Trim())) : string.Empty;
                    desc += (description != null && description.Trim().Length > 0) ?
                        string.Format("<div>{0}</div>", Utils.ParseHelper.StripHtmlTags(description.Trim())) : string.Empty;

                    if (desc.Trim().Length > 0)
                        litDescCrit.Text = string.Format("<div class=\"describe\">{0}{1}</div>", (ShowTicket.IsCampingPass(desc)) ? "CAMPING " : string.Empty, desc).Trim();
                }
            }

        }

        private void AggregateRowTotals(
            ref int loc_allot, ref int loc_pend, ref int loc_sold,
            ref int loc_willCall, ref int loc_willOrders,
            ref int loc_shipped, ref int loc_shipOrders,
            ref int loc_avail, ref int loc_allOrders, ref int loc_ref,
            ref decimal loc_base, ref decimal loc_fees, ref decimal loc_sales,
            DataRowView rowView, DataRow datarow)
        {
            loc_allot += (int)datarow.ItemArray.GetValue(datarow.Table.Columns.IndexOf("iAllotment")); ;
            loc_pend += (int)datarow.ItemArray.GetValue(datarow.Table.Columns.IndexOf("pendingStock")); ;
            loc_sold += (int)datarow.ItemArray.GetValue(datarow.Table.Columns.IndexOf("iSold")); ;
            loc_willCall += (int)datarow.ItemArray.GetValue(datarow.Table.Columns.IndexOf("WillCall"));
            loc_willOrders += (int)datarow.ItemArray.GetValue(datarow.Table.Columns.IndexOf("WillOrders"));
            loc_shipped += (int)datarow.ItemArray.GetValue(datarow.Table.Columns.IndexOf("Shipped"));
            loc_shipOrders += (int)datarow.ItemArray.GetValue(datarow.Table.Columns.IndexOf("ShipOrders"));
            loc_avail += (int)datarow.ItemArray.GetValue(datarow.Table.Columns.IndexOf("iAvailable")); ;
            loc_allOrders += (int)datarow.ItemArray.GetValue(datarow.Table.Columns.IndexOf("WillOrders")) +
                (int)datarow.ItemArray.GetValue(datarow.Table.Columns.IndexOf("ShipOrders"));
            loc_ref += (int)datarow.ItemArray.GetValue(datarow.Table.Columns.IndexOf("iRefunded")); ;
            loc_base += (decimal)datarow.ItemArray.GetValue(datarow.Table.Columns.IndexOf("Base"));
            loc_fees += (decimal)datarow.ItemArray.GetValue(datarow.Table.Columns.IndexOf("Fees"));
            loc_sales += (decimal)datarow.ItemArray.GetValue(datarow.Table.Columns.IndexOf("Sales"));
        }

        protected void GridTickets_DataBound(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;
            GridViewRow header = grid.HeaderRow;

            //we are not using a no selection model - some or all!
            if (_selTix.Count == 0)
                _packAllSelections();

            //int countSelected = 0;
            //int countRows = 0;

            foreach (GridViewRow gvr in grid.Rows)
            {
                if (gvr.RowType == DataControlRowType.DataRow)
                {
                    //countRows++;

                    int idx = (int)grid.DataKeys[gvr.DataItemIndex].Value;
                    if (_selTix.Contains(idx))
                    {
                        //countSelected++;

                        //DataRowView rowView = (DataRowView)gvr.DataItem;

                        //DataRow row = rowView.Row;

                        //AggregateRowTotals(ref sel_allot, ref sel_pend, ref sel_sold,
                        //    ref sel_willCall, ref sel_willOrders,
                        //    ref sel_shipped, ref sel_shipOrders,
                        //    ref sel_avail, ref sel_allOrders, ref sel_ref,
                        //    ref sel_base, ref sel_fees, ref sel_sales,
                        //    rowView, row);

                        gvr.CssClass = "selected";
                        CheckBox sel = (CheckBox)gvr.FindControl("chkOne");

                        if (sel != null && (!sel.Checked))
                            sel.Checked = true;
                    }
                    else
                        gvr.CssClass = string.Empty;
                }
                else if (gvr.RowType == DataControlRowType.Footer)
                {
                    string g = "l";//check to see if we van add rows here?
                }
            }

            litSelection.DataBind();

            GooglePager1.DataBind();
        }

        protected void TicketCheckChanged(object sender, EventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            GridViewRow gvr = (GridViewRow)chk.NamingContainer;

            int idx = (int)GridTickets.DataKeys[gvr.DataItemIndex].Value;

            if ((!chk.Checked) && _selTix.Contains(idx))
                _selTix.Remove(idx);
            else if (chk.Checked && (!_selTix.Contains(idx)))
                _selTix.Add(idx);

            GridTickets.DataBind();
        }

        /// <summary>
        /// shows current grid selections
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void litSelection_DataBinding(object sender, EventArgs e)
        {
            Literal lit = (Literal)sender;

            if (_selTix.Count == 0)
                lit.Text = "<div>NO TICKETS SELECTED</div>";
            else if (_selTix.Count == GridTickets.DataKeys.Count)
                lit.Text = "<div>All Tickets For This Event (may include camping passes!)</div>";
            else
            {
                lit.Text = string.Empty;//init

                if (_currentTicketSelections.Trim().Length > 0 && _currentTicketSelections.Trim() != "0")
                {
                    if (_showDate == null)
                    {
                        string sql = "SELECT sd.* FROM [ShowDate] sd LEFT OUTER JOIN [ShowTicket] st ON sd.[Id] = st.[tShowDateId] WHERE st.[Id] = @tixId ";
                        Wcss._DatabaseCommandHelper cmd = new Wcss._DatabaseCommandHelper(sql);
                        cmd.AddCmdParameter("tixid", _selTix[0], System.Data.DbType.Int32);

                        ShowDate sd = new ShowDate();
                        sd.LoadAndCloseReader(cmd.GetReader());

                        _showDate = sd;
                    }

                    foreach (int ix in _selTix)
                    {
                        ShowTicket st = (ShowTicket)_showDate.ShowTicketRecords().Find(ix);
                        if (st != null)
                        {
                            lit.Text += Regex.Replace(string.Format("<div>{0} {1} - {2} {3} {4}</div>",
                                st.Id.ToString(),
                                st.PerItemPrice.ToString("c"),
                                st.AgeDescription,
                                st.CriteriaText_Derived.Trim(), st.SalesDescription_Derived.Trim()), @"\s+", " ").Trim();
                        }
                    }
                }
            }
        }

        #endregion

        #region ShipContext

        protected string ShipContext { get { return rblShipContext.SelectedValue; } }

        protected void rblShipContext_DataBinding(object sender, EventArgs e)
        {
            RadioButtonList rbl = (RadioButtonList)sender;

            if (rbl.Items.Count == 0)
            {
                rbl.Items.Add(new ListItem("All", "all"));
                rbl.Items.Add(new ListItem("Shipped", "shipped"));
                rbl.Items.Add(new ListItem("Will_Call_Only", ShipMethod.WillCall));
            }
        }

        protected void rblShipContext_DataBound(object sender, EventArgs e)
        {
            RadioButtonList rbl = (RadioButtonList)sender;
            if (rbl.Items.Count > 0 && rbl.SelectedIndex == -1)
                rbl.SelectedIndex = 0;
        }

        protected void rblShipContext_SelectedIndexChanged(object sender, EventArgs e)
        {
            RadioButtonList list = (RadioButtonList)sender;

            GooglePager1.OnGooglePagerChanged(0);
        }

        #endregion
        
        #region Sort Context

        protected void lstSortContext_DataBinding(object sender, EventArgs e)
        {
            RadioButtonList rbl = (RadioButtonList)sender;
            if (rbl.Items.Count == 0)
                rbl.DataSource = Enum.GetNames(typeof(_Enums.TicketManifestSortCriteria));
        }

        protected void lstSortContext_SelectedIndexChanged(object sender, EventArgs e)
        {
            RadioButtonList rbl = (RadioButtonList)sender;
            Ctx.Search_TicketManifestSort = (_Enums.TicketManifestSortCriteria)Enum.Parse(typeof(_Enums.TicketManifestSortCriteria), rbl.SelectedValue, true);

            GooglePager1.OnGooglePagerChanged(0);
        }

        protected void lstSortContext_DataBound(object sender, EventArgs e)
        {
            RadioButtonList rbl = (RadioButtonList)sender;
            if (rbl.SelectedIndex == -1)
            {
                foreach (ListItem li in rbl.Items)
                {
                    if (li.Text == Ctx.Search_TicketManifestSort.ToString())
                    {
                        li.Selected = true;
                        break;
                    }
                }
            }
        }

        #endregion

        #region Sales Grid

        protected int _rowCounter = 0;
        protected int _qty;

        protected void GridSales_DataBinding(object sender, EventArgs e)
        {
            _qty = 0;
            _rowCounter = GridSales.PageSize * GridSales.PageIndex;
        }

        protected void GridSales_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                _rowCounter += 1;

                TicketSalesRow item = (TicketSalesRow)e.Row.DataItem;
                string shipped = item.DateShipped;
                if ((!item.IsReturned) && shipped != null && shipped.Trim().Length > 0 && Utils.Validation.IsDate(shipped))
                {
                    e.Row.CssClass = "rowshipped";
                }

                Literal rowCounter = (Literal)e.Row.FindControl("LiteralRowCounter");
                if (rowCounter != null)
                    rowCounter.Text = _rowCounter.ToString();

                //aggs
                TicketSalesRow entity = (TicketSalesRow)e.Row.DataItem;

                if (entity != null)
                    _qty += entity.Qty;
            }
        }

        #endregion
    }
}