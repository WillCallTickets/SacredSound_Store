using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using Wcss;
using Wcss.QueryRow;

namespace WillCallWeb.Admin.AdminControls
{
    public partial class Listings_Tickets : BaseControl
    {
        #region New Paging

        bool isSelectCount;
        protected void objData_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            isSelectCount = e.ExecutingSelectCount;

            if (!isSelectCount)
            {
                e.Arguments.StartRowIndex = (GridSales.PageIndex * GridSales.PageSize) + 1;
                e.Arguments.MaximumRows = GridSales.PageSize;
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
            btnPrint.DataBind();
            btnCSV.DataBind();
            litSelection.DataBind();
            GooglePager1.DataBind();
        }

        #endregion

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
                if(Session["OrderEvent"] == null)
                    return null;

                return (ShowDate)Session["OrderEvent"];
            }
            set
            {                  
                Session.Remove("OrderEvent");
                
                if(value != null && value.ShowRecord.ApplicationId == _Config.APPLICATION_ID)
                    Session["OrderEvent"] = value;
            }
        }
        protected ShowTicket _showTicket
        {
            get
            {
                if (Session["OrderTicket"] == null)
                    return null;

                return (ShowTicket)Session["OrderTicket"];
            }
            set
            {
                Session.Remove("OrderTicket");
             
                if(value != null && value.ShowDateRecord.ShowRecord.ApplicationId == _Config.APPLICATION_ID)
                    Session["OrderTicket"] = value;
            }
        }

        #endregion

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            string script = "$('#" + chkPhone.ClientID + "').on('click', function() { $('.phoner').toggle(); })";
            Atx.RegisterJQueryScript(this, script, false);
        }

        #region ShipContext

        protected string ShipContext { get { return rblShipContext.SelectedValue; } }
        protected void rblShipContext_DataBinding(object sender, EventArgs e)
        {
            RadioButtonList rbl = (RadioButtonList)sender;
            
            if (rbl.Items.Count == 0)
            {
                rbl.Items.Add(new ListItem("All","all"));
                rbl.Items.Add(new ListItem("Shipped","shipped"));
                rbl.Items.Add(new ListItem("Will_Call_Only",ShipMethod.WillCall));
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

        #region Blocking

        //protected override void OnPreRender(EventArgs e)
        //{
        //    base.OnPreRender(e);

        //    string roundTable = " $('TABLE.opttbl').wrap('<div class=\"rndopttbl rounded\"></div>'); ";

        //    //set opacity for nav events
        //    if (this.HasControls() && this.UpdatePanel1.Visible)
        //        Atx.RegisterJQueryScript_BlockUI_AjaxMethod(this.UpdatePanel1, "#listingtickets", true, roundTable);
        //}

        #endregion

        #region Page Overhead
       
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                clockContext.SelectedValue = _listingDate.ToString();

                SetInputs();

                BindLists();
            }
        }
        protected void btnGetEmails_Click(object sender, EventArgs e)
        {
            //get & set the data
            Atx.CurrentDisplayList = TicketSalesRow.GetEmailOfTicketIdSales(
                (ddlShowDates.SelectedValue != null) ? int.Parse(ddlShowDates.SelectedValue) : 0,
                (GridTickets.SelectedValue != null) ? GridTickets.SelectedValue.ToString() : "0", 
                lstSortContext.SelectedValue, rblShipContext.SelectedValue,
                rdoPurchase.SelectedValue);

            if (Atx.CurrentDisplayList != null && Atx.CurrentDisplayList.Count > 0)
            {
                //do the popup
                string script = "doPagePopup('/Admin/DisplayList.aspx', 'true');";

                System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(),
                    Guid.NewGuid().ToString(), " ;" + script, true);
            }
        }
        protected void btnBatch_Click(object sender, EventArgs e)
        {
            base.Redirect("/Admin/Shipping_Tickets.aspx?p=batchview");
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
                        else if(lst.Count == 1)
                            _showDate = lst[0];
                    }
                    else
                        show = null;
                }
            }

            if (req2 != null && req2 != "0" && Utils.Validation.IsInteger(req2))
            {
                if(_showTicket == null || (_showTicket != null && _showTicket.Id.ToString() != req2))
                    _showTicket = ShowTicket.FetchByID(req2);

                if (_showTicket != null && _showTicket.ShowDateRecord.ShowRecord.ApplicationId != _Config.APPLICATION_ID)
                {
                    _showTicket = null;
                    _showDate = null;
                }
                else if (_showTicket != null && (_showDate == null || _showDate.Id != _showTicket.ShowDateRecord.Id))
                    _showDate = _showTicket.ShowDateRecord;
            }
            //else if(_showTicket != null)
            //    //reset ticket id on first entry
            //    _showTicket = null;

            //if the show date is not null and the calendar is not in range
            //reset the calendar to the showdate's date
            if (_showDate != null && _showDate.DateOfShow < clockContext.SelectedDate)
            {
                clockContext.SelectedDate = _showDate.DateOfShow.Date;
            }
        }

        private void BindLists()
        {
            rblShipContext.DataBind();
            lstSortContext.DataBind(); 
            ddlShowDates.DataBind();
        }

        #endregion

        #region DateSelection

        protected void ddlShowDates_DataBound(object sender, EventArgs e)
        {
            DropDownList list = (DropDownList)sender;

            list.SelectedIndex = -1;

            if (_showDate != null)
            {
                ListItem li = list.Items.FindByValue(_showDate.Id.ToString());

                if (li != null)
                    li.Selected = true;
            }
        }
        protected void clock_DateChange(object sender, EventArgs e)
        {
            _listingDate = clockContext.SelectedDate;
            ddlShowDates.Items.Clear();
            ddlShowDates.DataBind();
        }
        protected void ddlShowDates_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList list = (DropDownList)sender;

            int DateId = int.Parse(list.SelectedValue);

            ShowDate showDate = ShowDate.FetchByID(DateId);
            if (showDate != null)
                base.Redirect(string.Format("/Admin/Listings.aspx?p=tickets&shodateid={0}", showDate.Id));
        }

        #endregion

        #region Event List

        protected void SqlEventList_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters["@appId"].Value = Wcss._Config.APPLICATION_ID;
            e.Command.Parameters["@date"].Value = (clockContext.SelectedDate != clockContext.DefaultValue) ?
                clockContext.SelectedDate.ToString("MM/dd/yyyy") : DateTime.Now.ToString("MM/1/yyyy");
        }

        #endregion

        #region Ticket Grid

        protected void GridTickets_DataBound(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;

            GridViewRow header = grid.HeaderRow;

            if (header != null)
            {
                if (_showTicket == null)
                    header.CssClass = "selected";
                else
                {
                    //header.CssClass = "";
                    //header.BackColor = System.Drawing.Color.White;
                }
            }

            //set the highlight index
            if (_showTicket == null)
                grid.SelectedIndex = -1;
            //show ticket is not null now
            else //if (grid.SelectedIndex == -1 && grid.Rows.Count > 0)//set default on first entry
            {
                int idx = 0;

                foreach(DataKey key in grid.DataKeys)
                {
                    if(key["Id"].ToString() == _showTicket.Id.ToString())
                        break;

                    idx++;
                }

                grid.SelectedIndex = idx;
            }

            GooglePager1.DataBind();
        }
        protected void GridTickets_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridView grid = (GridView)sender;

            int currentTicketId = int.Parse(e.CommandArgument.ToString());

            if (currentTicketId == 0)
            {
                _showTicket = null;
            }
            else
                _showTicket = (ShowTicket)_showDate.ShowTicketRecords().Find(currentTicketId);

            grid.DataBind();

            GooglePager1.OnGooglePagerChanged(0);
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
        protected void GridTickets_DataBinding(object sender, EventArgs e)
        {
            _sold = 0;
            _allot = 0;
            _pend = 0;
            _ref = 0;
            _willCall = 0;
            _willOrders = 0;
            _shipped = 0;
            _shipOrders = 0;
            _avail = 0;
            _allOrders = 0;
            _base = 0;
            _fees = 0;
            _sales = 0;
        }
        protected void GridTickets_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridView grid = (GridView)sender;
            
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView rowView = (DataRowView)e.Row.DataItem;
                DataRow row = rowView.Row;

                int rowSold = (int)row.ItemArray.GetValue(row.Table.Columns.IndexOf("iSold"));

                Literal offsale = (Literal)e.Row.FindControl("litOffsale");
                object offs = row.ItemArray.GetValue(row.Table.Columns.IndexOf("dtEndDate"));
                DateTime OffSaleDate = (offs == null || offs.ToString().Trim().Length == 0) ? DateTime.MaxValue : (DateTime)offs;
                if (offsale != null && OffSaleDate.Year < DateTime.MaxValue.Year)
                    offsale.Text = OffSaleDate.ToString("MM/dd/yyyy hh:mmtt");
                _sold += rowSold;
                _willCall += (int)row.ItemArray.GetValue(row.Table.Columns.IndexOf("WillCall"));
                int _willOrders_local = (int)row.ItemArray.GetValue(row.Table.Columns.IndexOf("WillOrders"));
                _shipped += (int)row.ItemArray.GetValue(row.Table.Columns.IndexOf("Shipped"));
                int _shipOrders_local = (int)row.ItemArray.GetValue(row.Table.Columns.IndexOf("ShipOrders"));

                ////availability
                //int avail = (int)row.ItemArray.GetValue(row.Table.Columns.IndexOf("iAllotment")) -
                //        (int)row.ItemArray.GetValue(row.Table.Columns.IndexOf("pendingStock")) -
                //        (int)row.ItemArray.GetValue(row.Table.Columns.IndexOf("iSold"));
                //Literal litAvailable = (Literal)e.Row.FindControl("litAvailable");
                //if (litAvailable != null)   
                //    litAvailable.Text = avail.ToString();

                

                _willOrders += _willOrders_local;
                _shipOrders += _shipOrders_local;
                _allOrders += _willOrders_local + _shipOrders_local;

                _allot += (int)row.ItemArray.GetValue(row.Table.Columns.IndexOf("iAllotment"));
                _pend += (int)row.ItemArray.GetValue(row.Table.Columns.IndexOf("pendingStock"));
                _avail += (int)row.ItemArray.GetValue(row.Table.Columns.IndexOf("iAvailable"));
                _ref += (int)row.ItemArray.GetValue(row.Table.Columns.IndexOf("iRefunded"));
                _base += (decimal)row.ItemArray.GetValue(row.Table.Columns.IndexOf("Base"));
                _fees += (decimal)row.ItemArray.GetValue(row.Table.Columns.IndexOf("Fees"));
                _sales += (decimal)row.ItemArray.GetValue(row.Table.Columns.IndexOf("Sales"));

                Literal litDescCrit = (Literal)e.Row.FindControl("litDescCrit");
                if (litDescCrit != null)
                {
                    string criteria = row.ItemArray.GetValue(row.Table.Columns.IndexOf("CriteriaText")).ToString();
                    string description = row.ItemArray.GetValue(row.Table.Columns.IndexOf("SalesDescription")).ToString();

                    if (criteria != null && criteria.Trim().Length > 40)
                        criteria = string.Format("{0}...", criteria.Trim().Substring(0, 38).Trim());
                    if (description != null && description.Trim().Length > 40)
                        description = string.Format("{0}...", description.Trim().Substring(0, 38).Trim());

                    string theDescription = string.Format("{0}{1}",                        
                        (description != null && description.Trim().Length > 0) ? string.Format("<div>{0}</div>", description.Trim()) : string.Empty,
                        (criteria != null && criteria.Trim().Length > 0) ? criteria.Trim() : string.Empty);

                    if (theDescription.Trim().Length > 0)
                        litDescCrit.Text = string.Format("{0}{1}", (ShowTicket.IsCampingPass(theDescription)) ? "CAMPING " : string.Empty, theDescription).Trim();
                }
            }
        }

        #endregion

        #region Selections And Button text

        protected void btnPrint_DataBinding(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            if (_showDate != null)
                btn.OnClientClick = string.Format("doPagePopup('/Admin/PrintTicketList.aspx?date={0}&tik={1}&ctx={2}&purch={3}', 'true');",
                    _showDate.Id, (_showTicket != null) ? _showTicket.Id.ToString() : "0", this.ShipContext, 
                    rdoPurchase.SelectedValue);
            else
                btn.OnClientClick = string.Empty;
        }
        protected void btnCSV_DataBinding(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            if (_showDate != null)
                btn.OnClientClick = string.Format("doPagePopup('/Admin/PrintTicketList_CSV_Download.aspx?date={0}&tik={1}&ctx={2}', 'true');",
                    _showDate.Id, (_showTicket != null) ? _showTicket.Id.ToString() : "0", this.ShipContext);
            else
                btn.OnClientClick = string.Empty;
        }
        protected void litSelection_DataBinding(object sender, EventArgs e)
        {
            Literal lit = (Literal)sender;

            if (_showTicket == null)
                lit.Text = " All Tickets For This Event (may include camping passes!)";
            else
                lit.Text = string.Format("Sales For **SELECTION ONLY*** &nbsp;{0} - {1}{2}",
                    _showTicket.PerItemPrice.ToString("c"),
                    _showTicket.AgeDescription,
                    (_showTicket.CriteriaText_Derived.Length > 0) ? string.Format(" - {0}", _showTicket.CriteriaText_Derived.Trim()) : string.Empty);
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
                {
                    _qty += entity.Qty;//aggs

                    //<asp:HyperLink ID="linkEditUser" runat="server" 
                    //NavigateUrl='<%#"/Admin/Orders.aspx?p=view&Inv=" + Eval("ParentInvoiceId") %>' 
                    //ToolTip="Go to customer's invoice for this purchase" Text='<%#Eval("Email") %>' />
                    Literal lit = (Literal)e.Row.FindControl("litOrderLink");
                    if (lit != null)
                    {
                        if (this.Page.User.IsInRole("Super") || this.Page.User.IsInRole("Administrator") || this.Page.User.IsInRole("OrderFiller"))
                            lit.Text = string.Format("<a href=\"/Admin/Orders.aspx?p=view&Inv={0}\">{1}</a>", entity.ParentInvoiceId, entity.Email);
                        else
                            lit.Text = entity.Email;
                    }
                }
            }
        }  

        #endregion

        #region Other Buttons

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            //GridSales.DataBind();
        }
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            if (_showDate != null)
            {
                Atx.SetCurrentShowRecord(_showDate.ShowRecord.Id);
                base.Redirect(string.Format("/Admin/ShowEditor.aspx?p=details"));
            }
        }

        #endregion
}
}
//626 - 091112
/*

protected void btnLoadDate_Click(object sender, EventArgs e)
        {
            if (ddlShowDates.SelectedIndex != -1)
            {
                int idx = int.Parse(ddlShowDates.SelectedValue);

                ShowDate showDate = ShowDate.FetchByID(idx);
                if (showDate != null)
                    base.Redirect(string.Format("/Admin/Listings.aspx?p=tickets&shodateid={0}", showDate.Id));
            }
        }

*/