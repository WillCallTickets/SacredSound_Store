using System;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using Wcss;
using Wcss.QueryRow;

namespace WillCallWeb.Admin.AdminControls
{
    public partial class Reports_InventoryMerch : BaseControl
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
                ddlDivision.DataBind();

                ddlDeliveryType.DataBind();

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

            InventoryMerchItemRow entity = (InventoryMerchItemRow)gvr.DataItem;

            if (entity != null)
            {
                Literal division = (Literal)gvr.FindControl("litDivision");
                if (division != null && entity.DivId > 0)
                {
                    MerchDivision div = (MerchDivision)_Lookits.MerchDivisions.Find(entity.DivId);
                    if(div != null)
                        division.Text = div.Name;
                }

                Literal category = (Literal)gvr.FindControl("litCategory");
                if (category != null && entity.CatId > 0)
                {
                    MerchCategorie cat = (MerchCategorie)_Lookits.MerchCategories.Find(entity.CatId);
                    if(cat != null)
                        category.Text = cat.Name;
                }
            }
        }
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string cmd = e.CommandName.ToLower();
            string args = e.CommandArgument.ToString();

            switch(cmd)
            {
                case "getsales":
                    base.Redirect(string.Format("/Admin/Reports.aspx?p=merch&merchitem={0}", args));
                    break;
                //case "edititem":
                //    base.Redirect(string.Format("/Admin/MerchEditor.aspx?p=itemedit&merchitem={0}", int.Parse(args)));
                //    break;
            }
        }

        #endregion

        #region Divs, Cats and Statii

        protected void ddlDivision_DataBinding(object sender, EventArgs e)
        {
            DropDownList list = (DropDownList)sender;

            List<ListItem> coll = new List<ListItem>();

            coll.Add(new ListItem("All", "0"));

            foreach (MerchDivision div in _Lookits.MerchDivisions)
                coll.Add(new ListItem(div.Name, div.Id.ToString()));

            list.DataSource = coll;
            list.DataTextField = "Text";
            list.DataValueField = "Value";
        }
        protected void ddlDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlCategory.DataBind();

            GridView1.PageIndex = 0;
            //GridView1.DataBind();
        }
        protected void ddlDivision_DataBound(object sender, EventArgs e)
        {
            DropDownList list = (DropDownList)sender;

            if (list.SelectedIndex == -1 && list.Items.Count > 0)
                list.SelectedIndex = 0;

            ddlCategory.DataBind();
        }
        protected void ddlDeliveryType_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridView1.PageIndex = 0;
            //GridView1.DataBind();
        }
        protected void ddlDeliveryType_DataBinding(object sender, EventArgs e)
        {
            DropDownList list = (DropDownList)sender;

            if (list.Items.Count == 0)
            {
                List<ListItem> items = new List<ListItem>();
                List<string> coll = new List<string>();

                items.Add(new ListItem("All Types", string.Empty));

                coll.AddRange(Enum.GetNames(typeof(_Enums.DeliveryType)));

                foreach (string s in coll)
                    items.Add(new ListItem(s, s));

                list.DataSource = items;
                list.DataTextField = "Text";
                list.DataValueField = "Value";
            }
        }
        protected void ddlDeliveryType_DataBound(object sender, EventArgs e)
        {
            DropDownList list = (DropDownList)sender;

            if (list.SelectedIndex == -1 && list.Items.Count > 0)
                list.SelectedIndex = 0;
        }
        protected void ddlCategory_DataBinding(object sender, EventArgs e)
        {
            DropDownList list = (DropDownList)sender;

            List<ListItem> coll = new List<ListItem>();

            coll.Add(new ListItem("All", "0"));

            //get the selected division
            int divContext = int.Parse(ddlDivision.SelectedValue);

            if (divContext == 0)
                foreach (MerchCategorie cat in _Lookits.MerchCategories)
                    coll.Add(new ListItem(cat.Name, cat.Id.ToString()));
            else
            {
                MerchDivision div = (MerchDivision)_Lookits.MerchDivisions.Find(divContext);
                if (div != null)
                    foreach (MerchCategorie cat in div.MerchCategorieRecords())
                        coll.Add(new ListItem(cat.Name, cat.Id.ToString()));
            }

            list.DataSource = coll;
            list.DataTextField = "Text";
            list.DataValueField = "Value";
        }
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

                List<InventoryMerchItemRow> merchList = InventoryMerchItemRow.GetMerch_CSVReport(ddlDeliveryType.SelectedValue, int.Parse(ddlDivision.SelectedValue), 
                    int.Parse(ddlCategory.SelectedValue), rdoStatus.SelectedValue);
                    
                InventoryMerchItemRow.CSV_ProvideDownload(merchList, fileAttachmentName, null);
            }

        }
}
}