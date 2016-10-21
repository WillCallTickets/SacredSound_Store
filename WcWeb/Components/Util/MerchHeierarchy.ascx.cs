using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Data;
using System.Web.Services;

using Wcss;

namespace WillCallWeb.Components.Util
{
    /// <summary>
    /// Note that viewstate may need to be reinstated to have the time portion function properly
    /// </summary>
    [ToolboxData("<{0}:MerchHeierarchy runat=\"Server\" ContainerId=\"\" OrdinalContextString=\"\" ValidationGroup=\"\" ></{0}:MerchHeierarchy>")]
    [DefaultPropertyAttribute("TitleText")]
    public partial class MerchHeierarchy : BaseControl, IPostBackEventHandler
    {
        public string ContainerId { get; set; }
        public string OrdinalContextString { get; set; }
        public string ValidationGroup { get; set; }
        private StringBuilder _sql = new StringBuilder();
        protected StringBuilder SQL { get { return _sql; } set { _sql = value; } }
        protected int _rownum = 0;

        void IPostBackEventHandler.RaisePostBackEvent(string eventArgument)
        {
            string[] args = eventArgument.Split('~');
            string command = args[0];
            int idx = (args.Length > 1 && Utils.Validation.IsInteger((string)args[1])) ? int.Parse(args[1]) : 0;
            string result = string.Empty;

            switch (command.ToLower())
            {
                case "ordinal_changed":
                    //rebind the list by postback - auto
                    break;
                case "deleteordinal":
                    try
                    {
                        switch (this.OrdinalContext)
                        {
                            case _Enums.OrdinalContext.merchdivision:
                                MerchDivisionCollection mColl = new MerchDivisionCollection().Load();
                                mColl.DeleteFromCollection(idx);
                                break;
                            case _Enums.OrdinalContext.merchcategorie:
                                MerchCategorieCollection cColl = new MerchCategorieCollection()
                                    .Where(MerchCategorie.Columns.TMerchDivisionId, int.Parse(ddlDivision.SelectedValue))
                                    .Load();
                                cColl.DeleteFromCollection(idx);
                                break;
                        }

                        //rebind the list by postback - auto
                    }
                    catch(Exception ex)
                    {
                        ControlValidation.IsValid = false;
                        ControlValidation.ErrorMessage = ex.Message;
                    }
                    break;
            }
        }

        public _Enums.OrdinalContext OrdinalContext
        {
            get
            {
                return (_Enums.OrdinalContext)Enum.Parse(typeof(_Enums.OrdinalContext), this.OrdinalContextString, true);
            }
        }

        private void InitContext()
        {
            SQL.Length = 0;

            switch (OrdinalContext)
            {
                case _Enums.OrdinalContext.merchdivision:
                    SQL.AppendLine("SELECT md.[Id], md.[Name], md.[Description], md.[iDisplayOrder], ISNULL(md.[bInternalOnly],0) as 'IsInternal', COUNT(mjc.[Id]) as 'ProductCount' ");
                    SQL.AppendLine("FROM [MerchDivision] md ");
                    SQL.AppendLine("LEFT OUTER JOIN [MerchCategorie] mc ON mc.[tMerchDivisionId] = md.[Id] ");
                    SQL.AppendLine("LEFT OUTER JOIN [MerchJoinCat] mjc ON mjc.[tMerchCategorieId] = mc.[Id] ");
                    SQL.AppendLine("GROUP BY md.[Id], md.[Name], md.[Description], md.[iDisplayOrder], ISNULL(md.[bInternalOnly],0) ");
                    SQL.AppendLine("ORDER BY [iDisplayOrder] ASC");
                    break;

                case _Enums.OrdinalContext.merchcategorie:
                    SQL.AppendLine("SELECT mc.[Id], mc.[Name], mc.[Description], mc.[tMerchDivisionId], mc.[iDisplayOrder], ISNULL(mc.[bInternalOnly],0) as 'IsInternal' , COUNT(mjc.[Id]) as 'ProductCount' ");
                    SQL.AppendLine("FROM [MerchCategorie] mc ");
                    SQL.AppendLine("LEFT OUTER JOIN [MerchJoinCat] mjc ON mjc.[tMerchCategorieId] = mc.[Id] ");
                    SQL.AppendLine("WHERE mc.[tMerchDivisionId] = @divId ");
                    SQL.AppendLine("GROUP BY mc.[Id], mc.[Name], mc.[Description], mc.[tMerchDivisionId], mc.[iDisplayOrder], ISNULL(mc.[bInternalOnly],0) ");
                    SQL.AppendLine("ORDER BY [iDisplayOrder] ASC");
                    break;

                case _Enums.OrdinalContext.merchjoincat:
                    SQL.AppendLine("SELECT mjc.[Id], mjc.[tMerchId], mjc.[tMerchCategorieId], mjc.[iDisplayOrder], m.[Name], ISNULL(m.[bActive],0) as 'IsActive' ");
                    SQL.AppendLine("FROM [MerchJoinCat] mjc, [Merch] m WHERE mjc.[tMerchCategorieId] = @catId AND mjc.[tMerchId] = m.[Id] ORDER BY [iDisplayOrder] ASC");
                    break;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            InitContext();

            //this.Page.RegisterRequiresControlState(this);
        }
        protected void Page_Load(object sender, EventArgs e) {}

        protected void SetVisibilityByContext()
        {
            pnlDivision.Visible = (this.OrdinalContext != _Enums.OrdinalContext.merchdivision);
            pnlCategorie.Visible = (this.OrdinalContext == _Enums.OrdinalContext.merchjoincat);
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            SetVisibilityByContext();

            ChildControlsCreated = true;
        }

        protected void ddlDivision_DataBound(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;

            if (ddl.SelectedIndex == -1 && ddl.Items.Count > 0)
                ddl.SelectedIndex = 0;
        }

        protected void ddlCategorie_DataBound(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;

            if (ddl.SelectedIndex == -1 && ddl.Items.Count > 0)
                ddl.SelectedIndex = 0;
        }

        protected void SqlDivision_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters["@appId"].Value = Wcss._Config.APPLICATION_ID;
        }

        protected void SqlList_Init(object sender, EventArgs e)
        {
            SqlDataSource sds = (SqlDataSource)sender;
            sds.SelectParameters.Clear();

            switch (OrdinalContext)
            {
                case _Enums.OrdinalContext.merchdivision:
                    break;

                case _Enums.OrdinalContext.merchcategorie:
                    sds.SelectParameters.Add(new ControlParameter("divId", DbType.Int32, "ddlDivision", "SelectedValue"));
                    break;

                case _Enums.OrdinalContext.merchjoincat:
                    sds.SelectParameters.Add(new ControlParameter("divId", DbType.Int32, "ddlDivision", "SelectedValue"));
                    sds.SelectParameters.Add(new ControlParameter("catId", DbType.Int32, "ddlCategorie", "SelectedValue"));
                    break;
            }
        }

        protected void SqlList_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.CommandText = SQL.ToString();
        }

        protected void rptList_DataBinding(object sender, EventArgs e)
        {
            _rownum = 0;
        }

        protected void rptList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            Literal lit = (Literal)e.Item.FindControl("litRowNum");
            Literal litInfo = (Literal)e.Item.FindControl("litInfo");
            Literal litDesc = (Literal)e.Item.FindControl("litDescription");
            Literal litLIStart = (Literal)e.Item.FindControl("litLIStart");
            Button btnAdd = (Button)e.Item.FindControl("btnAddNew");

            //add button exists only in header - no dataitem exists there
            if (btnAdd != null)
            {
                btnAdd.Visible = (this.OrdinalContext != _Enums.OrdinalContext.merchjoincat);
                btnAdd.Text = (btnAdd.Visible && this.OrdinalContext == _Enums.OrdinalContext.merchcategorie) ? "Add Categorie" : "Add Division";
            }

            if(drv != null)
            {
                int productId = int.Parse(Utils.DataHelper.GetColumnValue(drv.Row, "Id", DbType.Int32).ToString());

                if (lit != null)
                {
                    lit.Text = _rownum++.ToString();
                    if (this.Page.User.IsInRole("Super"))
                    {
                        int displayOrder = (int)Utils.DataHelper.GetColumnValue(drv.Row, "IDisplayOrder", DbType.Int32);
                        lit.Text += string.Format("<span class=\"super-display\"> * {0}</span>", displayOrder.ToString());
                    }
                }

                if (litInfo != null && litLIStart != null)
                {
                    if (this.OrdinalContext == _Enums.OrdinalContext.merchjoincat)
                    {
                        bool isActive = bool.Parse(Utils.DataHelper.GetColumnValue(drv.Row, "IsActive", DbType.String).ToString());
                        if (!isActive)
                            litInfo.Text = "NotActive";
                    }
                    else
                    {
                        bool isInternal = bool.Parse(Utils.DataHelper.GetColumnValue(drv.Row, "IsInternal", DbType.String).ToString());
                        if (isInternal)
                            litInfo.Text = "Internal";
                    }

                    litLIStart.Text = string.Format("<li id=\"ordinal_{0}\" {1}>", productId.ToString(),
                        (litInfo.Text.Trim().Length > 0) ? string.Format("class=\"{0}\" ", litInfo.Text.ToLower()) : string.Empty);
                }

                if (litDesc != null)
                {
                    string description = (string)Utils.DataHelper.GetColumnValue(drv.Row, "Description", DbType.String);
                    if (description != null && description.Trim().Length > 0)
                        litDesc.Text = string.Format("<a href=\"\" title=\"{0}\" class=\"infodot\">i</a>", 
                            System.Web.HttpUtility.HtmlEncode(description.Trim()));
                }

                Literal litDelete = (Literal)e.Item.FindControl("litDelete");
                if (litDelete != null && this.OrdinalContext != _Enums.OrdinalContext.merchjoincat)
                {
                    //dont allow divs or cats with existing items to be deleted
                    int productCount = int.Parse(Utils.DataHelper.GetColumnValue(drv.Row, "ProductCount", DbType.Int32).ToString());

                    if (productCount == 0)
                    {   
                        string href = this.Page.ClientScript
                            .GetPostBackClientHyperlink(this, string.Format("deleteordinal~{0}", productId.ToString()), false);
                        
                        litDelete.Text = string.Format("<a href=\"{0}\" title=\"Delete\" class=\"ordinal-delete-link\">Delete</a>", href);
                    }
                }
            }
        }       
}
}

