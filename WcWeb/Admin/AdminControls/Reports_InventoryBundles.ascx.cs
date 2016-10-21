using System;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using Wcss;
using Wcss.QueryRow;

namespace WillCallWeb.Admin.AdminControls
{
    public partial class Reports_InventoryBundles : BaseControl
    {
        #region New paging

        bool isSelectCount;
        protected void objData_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            isSelectCount = e.ExecutingSelectCount;

            if (!isSelectCount)
            {
                e.Arguments.StartRowIndex = GooglePager1.StartRowIndex;// (GridView1.PageIndex * GridView1.PageSize) + 1;
                e.Arguments.MaximumRows = GooglePager1.PageSize;// GridView1.PageSize;
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

        #region Calendars

        protected void clock_Init(object sender, EventArgs e)
        {
            WillCallWeb.Components.Util.CalendarClock cal = (WillCallWeb.Components.Util.CalendarClock)sender;

            if (cal.ID.ToLower().IndexOf("start") != -1)//set to first of month
                cal.SelectedDate = DateTime.Parse(string.Format("{0}/1/{1} 12AM", DateTime.Now.Month.ToString(), DateTime.Now.Year.ToString()));
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

            GridView1.DataBind();
        }
        private void EnsureValidCalendarSelection()
        {
            DateTime startClock = clockStart.SelectedDate;
            DateTime endClock = clockEnd.SelectedDate;

            if (endClock < startClock)
                clockEnd.SelectedDate = startClock.AddDays(1);
        }

        #endregion


        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            //set opacity for nav events
            if (this.HasControls() && this.UpdatePanel1.Visible)
                Atx.RegisterJQueryScript_BlockUI_AjaxMethod(this.UpdatePanel1, "#report", true);
        }

        #region Page Overhead

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!IsPostBack)
            {

                //GridView1.DataBind();
            }
        }
        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            //GridView1.DataBind();
        }

        #endregion

        #region GridView1

        protected void GridView1_DataBinding(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;
        }
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridViewRow gvr = e.Row;

            SalesReportBundleRow entity = (SalesReportBundleRow)gvr.DataItem;

            if (entity != null)
            {
                HyperLink linkEdit = (HyperLink)e.Row.FindControl("linkEdit");
                if (linkEdit != null)
                {
                    if (entity.TMerchId.HasValue)
                        linkEdit.NavigateUrl = string.Format("/Admin/MerchEditor.aspx?p=Bundle&merchitem={0}", entity.TMerchId.ToString());
                    else if (entity.TShowTicketId.HasValue)
                        linkEdit.NavigateUrl = string.Format("/Admin/ShowEditor.aspx?p=Bundle&tixid={0}", entity.TShowTicketId.ToString());
                }

                Literal litDesc = (Literal)e.Row.FindControl("litDescription");
                if (litDesc != null)
                {
                    
                    litDesc.Text += string.Format("<div>{0}</div>", entity.Title);

                    if (entity.Comment != null)
                        litDesc.Text += string.Format("<div>{0}</div>", entity.Comment);

                }
            }
        }

        #endregion

        #region Cats and Statii
        
        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridView1.PageIndex = 0;
            //GridView1.DataBind();
        }
        protected void ddlCategory_DataBound(object sender, EventArgs e)
        {
            DropDownList list = (DropDownList)sender;

            if (list.SelectedIndex == -1 && list.Items.Count > 0)
                list.SelectedIndex = 0;
        }
        protected void rdoStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridView1.PageIndex = 0;
            //GridView1.DataBind();
        }

        #endregion

        protected void CSV_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            string fileAttachmentName = string.Empty;

            if (btn.CommandName == "csvall")
            {
                fileAttachmentName = string.Format("attachment; filename=Merch_{0}.csv",
                    Utils.ParseHelper.StripInvalidChars_Filename(DateTime.Now.ToString("MMddyyyy")));

                List<SalesReportBundleRow> list = SalesReportBundleRow
                    .GetBundle_CSVReport(ddlCategory.SelectedValue, bool.Parse(rdoStatus.SelectedValue), clockStart.SelectedDate, clockEnd.SelectedDate);
                
                SalesReportBundleRow.CSV_ProvideDownload(list, fileAttachmentName, null);
            }
        }
}
}