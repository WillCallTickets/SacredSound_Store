using System;
using System.Web.UI.WebControls;

using Wcss;

namespace WillCallWeb.Admin.AdminControls
{
    public partial class CustomerSales : BaseControl
    {
        string userName = string.Empty;

        #region New Paging

        bool isSelectCount;
        protected void objData_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            isSelectCount = e.ExecutingSelectCount;

            if (!isSelectCount)
            {
                e.Arguments.StartRowIndex = GooglePager1.StartRowIndex;
                e.Arguments.MaximumRows = GooglePager1.PageSize;
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

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            // retrieve the username from the querystring
            userName = this.Request.QueryString["UserName"];

            if (!this.IsPostBack)
            {
                GridView1.DataBind();

                LiteralUserName.Text = string.Format("<a href=\"/Admin/EditUser.aspx?username={0}\">{0}</a>", userName);
            }
        }

        #region GridView1

        protected int _rowCounter = 0;
        protected void GridView1_DataBinding(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;

            _rowCounter = grid.PageSize * grid.PageIndex;
        }
        protected void GridView1_DataBound(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;

            if (grid.SelectedIndex == -1 && grid.Rows.Count > 0)
                grid.SelectedIndex = 0;

            GooglePager1.DataBind();
        }
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridView grid = (GridView)sender;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                _rowCounter += 1;

                Literal rowCounter = (Literal)e.Row.FindControl("LiteralRowCounter");
                if (rowCounter != null)
                    rowCounter.Text = _rowCounter.ToString();

                Literal select = (Literal)e.Row.FindControl("LiteralSelect");
                Literal desc = (Literal)e.Row.FindControl("LiteralDescription");
                CustomerInvoiceRow entity = (CustomerInvoiceRow)e.Row.DataItem;

                if (select != null)
                    select.Text = string.Format("<a href=\"/Admin/Orders.aspx?p=view&amp;Inv={0}\">{1}</a><div class=\"list-inv-id\">{2}</div>", 
                        entity.InvoiceId, entity.InvoiceDate.ToString("MM/dd/yyyy hh:mmtt"), entity.UniqueId);

                if (desc != null && entity.ProductListing != null)
                {
                    Invoice _invoice = Invoice.FetchByID(entity.InvoiceId);
                    desc.Text = Invoice.InterpretProductDescription(this.Page.User.IsInRole("Administrator"), 
                        _invoice, Atx.SaleTickets, Atx.SaleMerch);
                }
            }
        }

        #endregion

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            //GridView1.DataBind();
        }
    }
}
