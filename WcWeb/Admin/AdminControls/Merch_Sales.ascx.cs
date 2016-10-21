using System;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using Wcss;
using Utils;

namespace WillCallWeb.Admin.AdminControls
{
    public partial class Merch_Sales : BaseControl
    {
        #region New paging

        bool isSelectCount;
        protected void objData_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            ObjectDataSourceView source = (ObjectDataSourceView)sender;
            isSelectCount = e.ExecutingSelectCount;

            if (!isSelectCount)
            {
                e.Arguments.StartRowIndex = (source.TypeName == "Wcss.InventoryMerchInRange") ? GooglePager1.StartRowIndex : GooglePagerSales.StartRowIndex;
                e.Arguments.MaximumRows = (source.TypeName == "Wcss.InventoryMerchInRange") ? GooglePager1.PageSize : GooglePagerSales.PageSize;
            }
        }
        protected void objData_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            ObjectDataSourceView source = (ObjectDataSourceView)sender;
            if (isSelectCount && e.ReturnValue != null && e.ReturnValue.GetType().Name == "Int32")
            {
                if (source.TypeName == "Wcss.InventoryMerchInRange")
                    GooglePager1.DataSetSize = (int)e.ReturnValue;
                else
                    GooglePagerSales.DataSetSize = (int)e.ReturnValue;
            }
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            GooglePager1.GooglePagerChanged += new WillCallWeb.Components.Navigation.gglPager.GooglePagerChangedEvent(GooglePager_GooglePagerChanged);
            GooglePagerSales.GooglePagerChanged += new WillCallWeb.Components.Navigation.gglPager.GooglePagerChangedEvent(GooglePager_GooglePagerChanged);

            //SetPageControl();
        }
        public override void Dispose()
        {
            GooglePager1.GooglePagerChanged += new WillCallWeb.Components.Navigation.gglPager.GooglePagerChangedEvent(GooglePager_GooglePagerChanged);
            GooglePagerSales.GooglePagerChanged += new WillCallWeb.Components.Navigation.gglPager.GooglePagerChangedEvent(GooglePager_GooglePagerChanged);
            base.Dispose();
        }
        protected void GooglePager_GooglePagerChanged(object sender, WillCallWeb.Components.Navigation.gglPager.GooglePagerEventArgs e)
        {
            WillCallWeb.Components.Navigation.gglPager source = (WillCallWeb.Components.Navigation.gglPager)sender;
            Atx.adminPageSize = e.NewPageSize;
            if (source.ID == "GooglePager1")
            {
                GridView1.PageIndex = e.NewPageIndex;
                GridView1.PageSize = Atx.adminPageSize;
            }
            else
            {
                SalesGrid.PageIndex = e.NewPageIndex;
                SalesGrid.PageSize = Atx.adminPageSize;
            }
        }
        protected void GridView1_Init(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;
            GooglePager1.PageSize = Atx.adminPageSize;
            grid.PageSize = GooglePager1.PageSize;
            grid.PageIndex = GooglePager1.PageIndex;
        }
        protected void SalesGrid_Init(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;
            GooglePagerSales.PageSize = Atx.adminPageSize;
            grid.PageSize = GooglePagerSales.PageSize;
            grid.PageIndex = GooglePagerSales.PageIndex;
        }

        #endregion

        protected void btnGetEmails_Click(object sender, EventArgs e)
        {
            //get & set the data
            int gridIdx = (GridView1.SelectedValue != null) ? (int)GridView1.SelectedValue : 0;
            Atx.CurrentDisplayList = CustomerInvoiceRow.GetEmailOfMerchSalesInRange(Atx.CurrentMerchRecord.Id, gridIdx,
                ddlStyle.SelectedValue, ddlColor.SelectedValue, ddlSize.SelectedValue, rdoStatus.SelectedValue,
                clockStart.SelectedDate, clockEnd.SelectedDate);

            if (Atx.CurrentDisplayList != null && Atx.CurrentDisplayList.Count > 0)
            {
                //do the popup
                string script = "doPagePopup('/Admin/DisplayList.aspx', 'true');";

                System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(),
                    Guid.NewGuid().ToString(), " ;" + script, true);
            }
        }
        protected void btnGetEmailAndIds_Click(object sender, EventArgs e)
        {
            //get & set the data
            int gridIdx = (GridView1.SelectedValue != null) ? (int)GridView1.SelectedValue : 0;

            Atx.CurrentDisplayList = CustomerInvoiceRow.GetEmailOfMerchSalesInRange(Atx.CurrentMerchRecord.Id, gridIdx,
                ddlStyle.SelectedValue, ddlColor.SelectedValue, ddlSize.SelectedValue, rdoStatus.SelectedValue,
                clockStart.SelectedDate, clockEnd.SelectedDate, true);

            if (Atx.CurrentDisplayList != null && Atx.CurrentDisplayList.Count > 0)
            {
                //do the popup
                string script = "doPagePopup('/Admin/DisplayList.aspx', 'true');";

                System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(),
                    Guid.NewGuid().ToString(), " ;" + script, true);
            }
        }
        protected void btnCodes_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            //get & set the data
            int gridIdx = (GridView1.SelectedValue != null) ? (int)GridView1.SelectedValue : 0;

            Atx.CurrentDisplayList = CustomerInvoiceRow.GetMerchCodesInRange(Atx.CurrentMerchRecord.Id, gridIdx,
                ddlStyle.SelectedValue, ddlColor.SelectedValue, ddlSize.SelectedValue, rdoStatus.SelectedValue,
                clockStart.SelectedDate, clockEnd.SelectedDate);

            if (Atx.CurrentDisplayList != null && Atx.CurrentDisplayList.Count > 0)
            {
                if (btn.CommandName.ToLower() == "codeonly")
                {
                    List<string> list = new List<string>();

                    foreach (string s in Atx.CurrentDisplayList)
                    {
                        list.Add(s.Substring(s.IndexOf("=")+1));
                    }

                    Atx.CurrentDisplayList.Clear();
                    Atx.CurrentDisplayList.AddRange(list);
                }

                //do the popup
                string script = "doPagePopup('/Admin/DisplayList.aspx', 'true');";

                System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(),
                    Guid.NewGuid().ToString(), " ;" + script, true);
            }
        }

        private int _rowCounter = 0;
        protected decimal _totalTix = 0;
        protected decimal _totalFee = 0;
        protected decimal _totalAll = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindAll();
            }
        }

        protected Merch Entity
        {
            get
            {
                string req = Request.QueryString["merchitem"];

                //MIMIC the mercheditor record assign
                if (Atx.CurrentMerchRecord == null || (Atx.CurrentMerchRecord.Id.ToString() != req))
                {
                    
                    if (req != null && Utils.Validation.IsInteger(req))
                    {
                        int idx = int.Parse(req);

                        if (idx > 0)
                        {
                            Atx.SetCurrentMerchRecord(idx);

                            if (Atx.CurrentMerchRecord.IsChild)
                                Atx.SetCurrentMerchRecord(Atx.CurrentMerchRecord.TParentListing.Value);
                        }
                        else if (idx == 0 && Atx.CurrentMerchRecord != null)
                            Atx.SetCurrentMerchRecord(0);
                    }
                }

                return Atx.CurrentMerchRecord;
            }
        }

        #region Calendars

        protected void clock_Init(object sender, EventArgs e)
        {
            WillCallWeb.Components.Util.CalendarClock cal = (WillCallWeb.Components.Util.CalendarClock)sender;

            if (Entity != null && cal.ID.ToLower().IndexOf("start") != -1)
                cal.SelectedDate = Entity.DtStamp;
            else
                cal.SelectedDate = DateTime.Now.Date.AddDays(1).AddMinutes(-1);
        }
        protected void clock_SelectedDateChanged(object sender, WillCallWeb.Components.Util.CalendarClock.CalendarClockChangedEventArgs e)
        {
            WillCallWeb.Components.Util.CalendarClock cal = (WillCallWeb.Components.Util.CalendarClock)sender;

            if (cal.ID.ToLower().IndexOf("start") != -1)
                cal.SelectedDate = e.ChosenDate;
            else
                cal.SelectedDate = e.ChosenDate;

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

        #region Control Binding Control

        private void BindAll()
        {
            BindInventorySelection();

            BindGrids();
        }
        private void BindGrids()
        {
            GridView1.SelectedIndex = -1;
            GridView1.DataBind();
            SqlItemReport.DataBind();
            SalesGrid.DataBind();
        }
        private void BindInventorySelection()
        {
            ddlStyle.DataBind();
            ddlColor.DataBind();
            ddlSize.DataBind();
        }

        #endregion

        #region GridView

        protected void GridView1_DataBinding(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;
            _rowCounter = grid.PageSize * grid.PageIndex;

            string[] keyNames = { "merchId" };
            grid.DataKeyNames = keyNames;
        }
        protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
        {
            GridView grid = (GridView)sender;
            if (e.Row.RowType == DataControlRowType.Header && grid.SelectedIndex == -1)
            {
               // e.Row.CssClass = "selected";

            }
        }
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if(GridView1.SelectedIndex == -1)
                    e.Row.Cells[0].CssClass = "selected";

                Literal allot = (Literal)e.Row.FindControl("litAllot");
                Literal pend = (Literal)e.Row.FindControl("litPend");
                Literal sold = (Literal)e.Row.FindControl("litSold");
                Literal avail = (Literal)e.Row.FindControl("litAvail");
                Literal refund = (Literal)e.Row.FindControl("litRefund");

                if (Entity != null)
                {
                    if (allot != null && pend != null && sold != null && avail != null && refund != null)
                    {
                        allot.Text = string.Format("({0})", Entity.Allotment);
                        pend.Text = string.Format("({0})", "$?p");//#w Entity.Pending);
                        sold.Text = string.Format("({0})", Entity.Sold);
                        avail.Text = string.Format("({0})", Entity.Available);
                        refund.Text = string.Format("({0})", Entity.Refunded);
                    }
                }
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {
                InventoryMerchInRange entity = (InventoryMerchInRange)e.Row.DataItem;

                _rowCounter += 1;

                Panel rowCounter = (Panel)e.Row.FindControl("PanelRowCounter");
                if (rowCounter != null)
                {
                    LinkButton link = new LinkButton();
                    link.ID = "linkSelect";
                    link.CommandName = "Select";
                    link.Text = _rowCounter.ToString();
                    link.CssClass = "btnadmin";

                    rowCounter.Controls.Add(link);
                }

                Literal pendingDisc = (Literal)e.Row.FindControl("litPending");
                Literal purchasedDisc = (Literal)e.Row.FindControl("litPurchased");
                Literal refundDisc = (Literal)e.Row.FindControl("litRefund");

                if (pendingDisc != null)
                    pendingDisc.Text = string.Format("<div {0}>{1}</div>", (entity.SalesPend != entity.Pend) ?
                        "style=\"background-color: red; color: white;\"" : string.Empty, entity.SalesPend);
                if (purchasedDisc != null)
                    purchasedDisc.Text = string.Format("<div {0}>{1}</div>", (entity.SalesSold != entity.Sold) ?
                        "style=\"background-color: red; color: white;\"" : string.Empty, entity.SalesSold);
                if (refundDisc != null)
                    refundDisc.Text = string.Format("<div {0}>{1}</div>", (entity.SalesRefund != entity.Refund) ?
                        "style=\"background-color: red; color: white;\"" : string.Empty, entity.SalesRefund);
            }
        }
        protected void GridView1_DataBound(object sender, EventArgs e)
        {
            string range = string.Format("{0} -style: {1} -color: {2} -size: {3} -status: {4} ",
                (Entity != null) ? Entity.DisplayNameWithAttribs : string.Empty, 
                ddlStyle.SelectedValue, ddlColor.SelectedValue, ddlSize.SelectedValue, rdoStatus.SelectedValue);
            litLifetime.Text = string.Format("Lifetime: {0}", range);

            GooglePager1.DataBind();
        }
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridView grid = (GridView)sender;
            string command = e.CommandName.ToLower();

            switch (command)
            {
                case "deselect":
                    grid.SelectedIndex = -1;
                    GridView1.HeaderRow.CssClass = "selected";
                    break;
                case "csvreport":
                    CsvReport(int.Parse(e.CommandArgument.ToString()));
                    break;
            }
        }
        protected void btnCsvClick(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            CsvReport(Entity.Id);
        }
        protected void CsvReport(int merchId)
        {
            string fileAttachmentName = string.Empty;
            
            string filename = string.Format("ExclusiveSales_Report_MerchId_{0}", merchId.ToString());
            fileAttachmentName = string.Format("attachment; filename={0}.csv",
                Utils.ParseHelper.StripInvalidChars_Filename(filename));

            //GetItemBillShips_CSVReport(int itemId, bool isExclusive, int minQty, DateTime startDate, DateTime endDate)
            Wcss.QueryRow.ItemBillShips.GetItemBillShips_CSVReport(
                merchId, true, 1, DateTime.Parse("1/1/2008"), DateTime.Now.AddDays(7), fileAttachmentName, null);
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                GridView1.HeaderRow.CssClass = string.Empty;

                if(GridView1.SelectedIndex == -1)
                    GridView1.HeaderRow.CssClass = "selected";

                SalesGrid.DataBind();
            }
            catch (Exception ex)
            {
                _Error.LogException(ex);
            }
        }

        #endregion

        protected void SqlItemReport_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Literal headTitle = (Literal)e.Row.FindControl("litRangedTitle");
            if (headTitle != null)
                headTitle.Text = string.Format("{0} sales {1} thru {2}", (Entity != null) ? Entity.DisplayNameWithAttribs : string.Empty, 
                    clockStart.SelectedDate.ToString("MM/dd/yyyy"), clockEnd.SelectedDate.ToString("MM/dd/yyyy"));

            Literal attribs = (Literal)e.Row.FindControl("litRangedAttribs");
            if (attribs != null)
                attribs.Text = string.Format("(style) {0} (color) {1} (size) {2} (status) {3} ",
                    ddlStyle.SelectedValue, ddlColor.SelectedValue, ddlSize.SelectedValue, rdoStatus.SelectedValue);
        }

        #region SalesGrid

        protected decimal _gridFreight = 0;
        protected decimal _gridTotal = 0;

        protected void SalesGrid_DataBinding(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;
            _rowCounter = grid.PageSize * grid.PageIndex;

            _gridFreight = 0;
            _gridTotal = 0;
        }
        protected void SalesGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {            
            if (e.Row.RowType == DataControlRowType.Header)
            {
                GridView grid = (GridView)sender;

                List<CustomerInvoiceAggregateRow> agg = CustomerInvoiceRow.GetMerchSalesInRangeAggregates(
                    (Entity != null) ? Entity.Id : 0,
                    (GridView1.SelectedValue != null) ? (int)GridView1.SelectedValue : 0,
                    (ddlStyle.Visible) ? ddlStyle.SelectedValue : string.Empty,
                    (ddlColor.Visible) ? ddlColor.SelectedValue : string.Empty,
                    (ddlSize.Visible) ? ddlSize.SelectedValue : string.Empty,
                    rdoStatus.SelectedValue, clockStart.SelectedDate, clockEnd.SelectedDate);

                e.Row.Cells[5].Text += string.Format(" ({0})", agg[0].TotalFreight.ToString("c"));
                e.Row.Cells[6].Text += string.Format(" ({0})", agg[0].TotalPaid.ToString("c"));
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CustomerInvoiceRow entity = (CustomerInvoiceRow)e.Row.DataItem;

                _rowCounter += 1;

                Literal rowCounter = (Literal)e.Row.FindControl("LiteralRowCounter");
                if (rowCounter != null)
                    rowCounter.Text = _rowCounter.ToString();

                Literal desc = (Literal)e.Row.FindControl("literalDescription");
                //Literal ship = (Literal)e.Row.FindControl("LiteralShipDate");
                Literal freight = (Literal)e.Row.FindControl("litFreight");
                Literal total = (Literal)e.Row.FindControl("litTotal");

                if (entity != null)
                {
                    Invoice _invoice = Invoice.FetchByID(entity.InvoiceId);
                    if (desc != null)
                        desc.Text = Invoice.InterpretProductDescription(this.Page.User.IsInRole("Administrator"), 
                            _invoice, Atx.SaleTickets, Atx.SaleMerch);
                    //if (ship != null)
                    //    ship.Text = entity.ShipStatus;// (entity.ShipDate < DateTime.MaxValue) ? entity.ShipDate.ToString("MM/dd/yy") : string.Empty;
                    if (freight != null)
                    {
                        _gridFreight += entity.FreightAmount;
                        freight.Text = (entity.FreightAmount > 0) ? entity.FreightAmount.ToString("n") : string.Empty;
                    }
                    if (total != null)
                    {
                        _gridTotal += entity.TotalPaid;
                        total.Text = (entity.TotalPaid > 0) ? entity.TotalPaid.ToString("n") : string.Empty;
                    }
                }
            }
        }
        protected void SalesGrid_DataBound(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;

            string attribs = string.Empty;

            if (rdoStatus.SelectedIndex > 0)
                attribs = string.Format("{0} records only, ", rdoStatus.SelectedItem.Text);
            if (ddlStyle.Visible && ddlStyle.SelectedValue != string.Empty)
                attribs += string.Format("style({0}), ", ddlStyle.SelectedValue.ToLower());
            if (ddlColor.Visible && ddlColor.SelectedValue != string.Empty)
                attribs += string.Format("color({0}), ", ddlColor.SelectedValue.ToLower());
            if (ddlSize.Visible && ddlSize.SelectedValue != string.Empty)
                attribs += string.Format("size({0}) ", ddlSize.SelectedValue.ToLower());

            if (attribs.Trim().Length > 0)
                attribs = string.Format(" {0} - ", attribs.Trim().TrimEnd(','));

            GooglePagerSales.DataBind();
            //litRange.Text = string.Format("range: {0}{1} to {2}", attribs,
            //    clockStart.SelectedDate.ToString("MM/dd/yyyy"), clockEnd.SelectedDate.ToString("MM/dd/yyyy"));
        }

        #endregion

        #region Inventory Selection
        protected void ddlStyle_DataBinding(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;

            if (Entity.IsParent)
            {
                List<string> coll = new List<string>();

                switch(rdoStatus.SelectedValue.ToLower()) //All, Active, InActive
                {
                    case "all":
                        coll.AddRange(Entity.ChildStyleList_All);
                        break;
                    case "active":
                        coll.AddRange(Entity.ChildStyleList);
                        break;
                    case "inactive":
                        List<Merch> children = Entity.ChildMerchRecords().GetList().FindAll(delegate(Merch match) { return (match.IsActive == false); });
                        foreach (Merch m in children)
                            if (m.Style != null && m.Style.Trim().Length > 0 && (!coll.Contains(m.Style)))
                                coll.Add(m.Style.Trim());
                        break;
                }

                if (coll.Count > 1)
                    coll.Sort();

                coll.Insert(0, "All Styles");
                ddl.Visible = true;
                ddl.DataSource = coll;
            }
        }
        protected void ddlColor_DataBinding(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;

            if (Entity.IsParent)
            {
                List<string> coll = new List<string>();

                switch (rdoStatus.SelectedValue.ToLower()) //All, Active, InActive
                {
                    case "all":
                        coll.AddRange(Entity.ChildColorList_All);
                        break;
                    case "active":
                        coll.AddRange(Entity.ChildColorList);
                        break;
                    case "inactive":
                        List<Merch> children = Entity.ChildMerchRecords().GetList().FindAll(delegate(Merch match) { return (match.IsActive == false); });
                        foreach (Merch m in children)
                            if (m.Color != null && m.Color.Trim().Length > 0 && (!coll.Contains(m.Color)))
                                coll.Add(m.Color.Trim());
                        break;
                }

                if (coll.Count > 1)
                    coll.Sort();

                coll.Insert(0, "All Colors");

                ddl.Visible = true;
                ddl.DataSource = coll;
            }
        }
        protected void ddlSize_DataBinding(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;

            if (Entity.IsParent)
            {
                List<string> coll = new List<string>();

                switch (rdoStatus.SelectedValue.ToLower()) //All, Active, InActive
                {
                    case "all":
                        coll.AddRange(Entity.ChildSizeList_All);
                        break;
                    case "active":
                        coll.AddRange(Entity.ChildSizeList);
                        break;
                    case "inactive":
                        List<Merch> children = Entity.ChildMerchRecords().GetList().FindAll(delegate(Merch match) { return (match.IsActive == false); });
                        foreach (Merch m in children)
                            if (m.Size != null && m.Size.Trim().Length > 0 && (!coll.Contains(m.Size)))
                                coll.Add(m.Size.Trim());
                        break;
                }

                if (coll.Count > 1)
                    coll.Sort();

                coll.Insert(0, "All Sizes");

                ddl.Visible = true;
                ddl.DataSource = coll;
            }
        }
        protected void Select_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridView1.PageIndex = 0;
            BindGrids();
        }

        #endregion
        protected void SqlItemReporter_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            string style = (ddlStyle != null && ddlStyle.Items.Count > 0) ? ddlStyle.SelectedValue : string.Empty;
            string color = (ddlColor != null && ddlColor.Items.Count > 0) ? ddlColor.SelectedValue : string.Empty;
            string size = (ddlSize != null && ddlSize.Items.Count > 0) ? ddlSize.SelectedValue : string.Empty;

            if (style != null && style.ToLower().IndexOf("all") != -1)
                style = string.Empty;
            if (color != null && color.ToLower().IndexOf("all") != -1)
                color = string.Empty;
            if (size != null && size.ToLower().IndexOf("all") != -1)
                size = string.Empty;

            //add params for drop down lists
            e.Command.Parameters["@style"].Value = style;
            e.Command.Parameters["@color"].Value = color;
            e.Command.Parameters["@size"].Value = size;
        }
}
}

