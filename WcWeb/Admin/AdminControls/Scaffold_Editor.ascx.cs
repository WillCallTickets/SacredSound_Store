using System;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;

using Wcss;

namespace WillCallWeb.Admin.AdminControls
{
    public partial class Scaffold_Editor : BaseControl
    {
        // <subsonic:Scaffold ID="ProductsScaffold" runat="server" TableName="CartDeal" HiddenEditorColumns="dtStamp, iDisplayOrder" HiddenGridColumns="dtStamp, iDisplayOrder" />

        #region Collections and Page Objects

        //protected string _context = "CartDeals";

        //protected void SetPageContext()
        //{
        //    string req = Request.QueryString["p"];

        //    if (req == null)
        //        req = "CartDeals";
            
        //    switch (req.ToLower())
        //    {
        //        case "CartDeals":
        //            _context = "Cart Deals";
        //            break;
        //    }
        //}

        protected void ddlDataType_DataBinding(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            //ddl.DataSource = Enum.GetNames(typeof(_Enums.DealContext));
        }
        
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            //SetPageContext();
            //lblResult.Text = string.Empty;

            //if (!IsPostBack)
            //{
            //    GridView1.DataBind();
            //}

            //only use this when you need to test
            //_Config.ConfigTest();
        }

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
        }

        #region GridView

        protected void GridView1_DataBinding(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;
        }
        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView grid = (GridView)sender;

            grid.EditIndex = e.NewEditIndex;

            grid.DataBind();
        }
        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView grid = (GridView)sender;

            grid.EditIndex = -1;

            grid.DataBind();
        }
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridView grid = (GridView)sender;
            GridViewRow row = (GridViewRow)e.Row;

           
        }
        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridView grid = (GridView)sender;
            GridViewRow row = grid.Rows[e.RowIndex];   
            
            //SiteConfig config = (SiteConfig)OrderedCollection.Find(int.Parse(grid.DataKeys[e.RowIndex].Value.ToString()));

            //if (config != null)
            //{   
               

            //    grid.EditIndex = -1;//get out of edit mode
            //    grid.DataBind();
            //}
        }

        #endregion
    }
}