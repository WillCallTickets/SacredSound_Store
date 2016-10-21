using System;
using System.Data;
using System.Web.UI.WebControls;

using Wcss;

namespace WillCallWeb.Admin.AdminControls
{
    public partial class Charitable_Listings : BaseControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GridViewEntity.DataBind();
            }
        }

        #region Grid

        protected void GridViewEntity_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridView grid = (GridView)sender;
            DataRowView rowView = (DataRowView)e.Row.DataItem;

            if (rowView != null)
            {
                DataRow row = rowView.Row;
                LinkButton up = (LinkButton)e.Row.FindControl("btnUp");
                LinkButton down = (LinkButton)e.Row.FindControl("btnDown");
                LinkButton delete = (LinkButton)e.Row.FindControl("btnDelete");

                string itemName = string.Empty;

                if (row != null)
                {
                    object displayName = row.ItemArray.GetValue(row.Table.Columns.IndexOf("DisplayName"));
                    itemName = (displayName != null && displayName.ToString().Trim().Length > 0) ? displayName.ToString() : 
                        row.ItemArray.GetValue(row.Table.Columns.IndexOf("Name")).ToString();
                }

                if (delete != null)
                    delete.OnClientClick = string.Format("return confirm('Are you sure you want to delete {0} from this listing?')",
                        Utils.ParseHelper.ParseJsAlert(itemName));

                if (up != null && down != null)
                {
                    int rowIdx = e.Row.RowIndex;

                    up.Enabled = (rowIdx > 0);

                    down.Enabled = (rowIdx < Atx.CharitableListings.Count - 1);
                }
            }
        }
        protected void GridViewEntity_DataBound(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;

            if (grid.Rows.Count > 0 && grid.SelectedIndex == -1)
                grid.SelectedIndex = 0;
            else if (grid.Rows.Count == 0)//turn off the other controls
            {
                FormView1.DataBind();

                //if(FormView1.CurrentMode != FormViewMode.Insert)
                //    Editor_CharitableOrg1.Visible = false;
            }
        }
        protected void GridViewEntity_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridView grid = (GridView)sender;

            string cmd = e.CommandName.ToLower();

            switch (cmd)
            {
                case "up":
                case "down":
                    CharitableListing moved = Atx.CharitableListings.ReorderItem(int.Parse(e.CommandArgument.ToString()), cmd);
                    //set the index of the moved item
                    grid.SelectedIndex = moved.DisplayOrder;
                    grid.DataBind();
                
                    break;
            }         
        }
        protected void GridViewEntity_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridView grid = (GridView)sender;

            //get selected
            object idx = e.Keys["Id"];
            if (idx != null && Utils.Validation.IsInteger(idx.ToString()))
            {
                //delete and reorder
                Atx.CharitableListings.DeleteFromCollection(int.Parse(idx.ToString()));
            }
        }
        protected void Source_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters["@appId"].Value = Wcss._Config.APPLICATION_ID;
        }

        #endregion

        #region Form

        protected void FormView1_ModeChanged(object sender, EventArgs e)
        {
            FormView form = (FormView)sender;
        }
        protected void FormView1_DataBinding(object sender, EventArgs e)
        {
            FormView form = (FormView)sender;
        }
        protected void FormView1_DataBound(object sender, EventArgs e)
        {
            FormView form = (FormView)sender;

            //charity item should follow along
            object orgId = form.DataKey["tCharitableOrgId"];
            if (orgId != null && Utils.Validation.IsInteger(orgId.ToString()))
            {
                WillCallWeb.Admin.AdminControls.Editor_CharitableOrg charity = 
                    (WillCallWeb.Admin.AdminControls.Editor_CharitableOrg)form.FindControl("Editor_CharitableOrg1");
                
                if(charity != null)
                {
                    charity.SelectedIdx = int.Parse(orgId.ToString());
                    charity.DataBind();
                }
            }
        }
        protected void FormView1_ItemInserting(object sender, FormViewInsertEventArgs e)
        {
            FormView form = (FormView)sender;

            int idx = 0;

            WillCallWeb.Admin.AdminControls.Editor_CharitableOrg charity = 
                    (WillCallWeb.Admin.AdminControls.Editor_CharitableOrg)form.FindControl("Editor_CharitableOrg1");
                
            if(charity != null)
                idx = charity.SelectedIdx;

            //if (idx == 0)
            //    throw new Exception("Please make a selection before attempting to add.");

            if (idx > 0)
            {
                try
                {
                    Atx.CharitableListings.AddToCollection(idx, false, false);

                    //after inserting new record reset controls
                    form.ChangeMode(FormViewMode.Edit);

                    GridViewEntity.SelectedIndex = Atx.CharitableListings.Count - 1;
                    Atx.Clear_CharitableListings();
                    GridViewEntity.DataBind();
                }
                catch (Exception ex)
                {
                    e.Cancel = true;
                    _Error.LogException(ex);

                    CustomValidator custom = (CustomValidator)form.FindControl("CustomValidation");
                    if (custom != null)
                    {
                        custom.IsValid = false;
                        custom.ErrorMessage = ex.Message;
                    }
                }
            }
        }
        protected void FormView1_ItemCommand(object sender, FormViewCommandEventArgs e)
        {
            FormView form = (FormView)sender;

            string command = e.CommandName.ToLower();

            switch (command)
            {
                case "new":
                    WillCallWeb.Admin.AdminControls.Editor_CharitableOrg charity = 
                    (WillCallWeb.Admin.AdminControls.Editor_CharitableOrg)form.FindControl("Editor_CharitableOrg1");
                
                    if(charity != null)
                        charity.SelectedIdx = 0;
                    break;
            }
        }

        #endregion      
        
}
}
