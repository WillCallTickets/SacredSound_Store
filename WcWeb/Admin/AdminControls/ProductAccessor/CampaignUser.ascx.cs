using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Wcss;

namespace WillCallWeb.Admin.AdminControls.ProductAccessor
{
    /// <summary>
    /// 
    /// </summary>
    public partial class CampaignUser : BaseControl
    {
        List<string> _errors = new List<string>();

        #region Page Overhead
        
        protected override void OnLoad(EventArgs e)
        {
            if (Atx.ProductAccessCampaigns.Count == 0 || Atx.CurrentAccessCampaign == null)
                base.Redirect("/Admin/ProductAccess.aspx?p=campaign");

            if (!IsPostBack)
            {
                GridListing.DataBind();
            }

           btnCampaignList.Enabled = (Atx.CurrentAccessCampaign != null);
        }

        protected void btnCampaignList_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            base.Redirect("/Admin/ProductAccess.aspx?p=campaign");
        }
        protected void btnCampaignMailer_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            base.Redirect("/Admin/ProductAccess.aspx?p=mlr");
        }

        protected void btnPublish_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            _Lookits.RefreshLookup(_Enums.LookupTableNames.ProductAccessors, Profile.UserName);
        }
       
        #endregion

        #region GridListing

        protected void GridListing_Init(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;
            grid.PageSize = 25;
        }
        protected void GridListing_DataBinding(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;

            if (Atx.ProductAccessCampaigns.Count > 0)
            {
                Atx.ProductAccessCampaigns.Sort("IDisplayOrder", true);
                grid.DataSource = Atx.ProductAccessCampaigns;
            }
        }
        protected void GridListing_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridView grid = (GridView)sender;

            GridViewRow gvr = e.Row;

            if (gvr != null && gvr.RowType == DataControlRowType.DataRow)
            {
                ProductAccess pa = (ProductAccess)gvr.DataItem;

                ActivationWindow aw = pa.ActivationWindowRecord;

                if (aw != null)
                {
                    Literal litPublicStart = (Literal)gvr.FindControl("litPublicStart");
                    Literal litPublicEnd = (Literal)gvr.FindControl("litPublicEnd");

                    if (litPublicStart != null)
                        litPublicStart.Text = (aw.DatePublicStart != Utils.Constants._MinDate) ? aw.DatePublicStart.ToString("MM/dd/yyyy hh:mmtt") : string.Empty;
                    if (litPublicEnd != null)
                        litPublicEnd.Text = (aw.DatePublicEnd != DateTime.MaxValue) ? aw.DatePublicEnd.ToString("MM/dd/yyyy hh:mmtt") : string.Empty;
                }
            }
        }
        protected void GridListing_DataBound(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;

            if (grid.Rows.Count > 0 && grid.SelectedIndex == -1)
            {
                if (Atx.CurrentAccessCampaign == null)
                {
                    grid.SelectedIndex = 0;
                    int selIdx = (int)grid.SelectedDataKey.Value;
                    Atx.SetCurrentAccessCampaign(selIdx);
                }
                else
                {
                    int idx = Atx.CurrentAccessCampaign.Id;
                    foreach (GridViewRow gvr in grid.Rows)
                    {
                        if ((int)grid.DataKeys[gvr.DataItemIndex]["Id"] == idx)
                        {
                            grid.SelectedIndex = gvr.DataItemIndex;
                            break;
                        }
                    }
                }
            }

            //FormEditor.DataBind();
            GridUsers.DataBind();
        }
        protected void GridListing_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;

            //if (FormEditor.CurrentMode != FormViewMode.Edit)
            //    FormEditor.ChangeMode(FormViewMode.Edit);

            //match selected index to current object
            int selIdx = (int)grid.SelectedDataKey.Value;
            Atx.SetCurrentAccessCampaign(selIdx);

            //FormEditor.DataBind();
            //GridUsers_ClearSelectedRows();
            //GridUsers.DataBind();
        }
        protected void GridListing_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridView grid = (GridView)sender;
            string cmd = e.CommandName.ToLower();

            //if (FormEditor.CurrentMode != FormViewMode.Edit)
            //    FormEditor.ChangeMode(FormViewMode.Edit);
        }

        #endregion       

        #region Grid Users
        //http://www.4guysfromrolla.com/articles/053106-1.aspx

        protected void GridCmd_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;            
            string id = btn.ID;

            //GridView grid = GridUsers;
            CustomValidator custom = null;
            bool clearSelections = false;

            string sql = string.Empty;
            SubSonic.QueryCommand cmd = new SubSonic.QueryCommand(sql, SubSonic.DataService.Provider.Name);
            int counter = 0;
            _errors.Clear();
            List<int> selectedIds = null;

            //inputs
            string qty = txtQty.Text.Trim();
            int inpQty = 0;
            string referral = txtReferral.Text.Trim();
            string instructions = txtInstructions.Text.Trim();

            #region Add Users

            if (id == "btnAddUsers" && txtAddUsers.Text.Trim().Length > 0)
            {
                custom = CustomAddUsers;
                //validate input
                string emailListing = string.Empty;
                List<string> emailList = new List<string>();
                emailList.AddRange(txtAddUsers.Text.Split(new string[] { ",", "\r\n" }, StringSplitOptions.RemoveEmptyEntries));

                List<string> invalids = Utils.Validation.InvalidArrayOfEmails(emailList);

                if (invalids.Count > 0)
                {
                    _errors.Add("The following emails are invalid");
                    _errors.AddRange(invalids);
                }

                //qty
                Utils.Validation.ValidateIntegerField(_errors, "Quantity", qty);
                Utils.Validation.ValidateIntegerRange(_errors, "Quantity", qty, 0, 100);

                //did we have errors? display them
                if (Utils.Validation.IncurredErrors(_errors, custom))
                {
                    return;
                }

                //end of validation

                if (Utils.Validation.IsInteger(qty))
                    inpQty = int.Parse(qty);

                List<string> valids = Utils.Validation.ValidArrayOfEmails(emailList);

                cmd.Parameters.Add("@dateNow", DateTime.Now, DbType.DateTime);
                cmd.Parameters.Add("@productAccessId", Atx.CurrentAccessCampaign.Id, DbType.Int32);
                cmd.Parameters.Add("@qty", inpQty, DbType.Int32);
                cmd.Parameters.Add("@referral", (referral.Length > 0) ? referral : null);
                cmd.Parameters.Add("@instructions", (instructions.Length > 0) ? instructions : null);

                foreach (string s in valids)
                {
                    string ctx = string.Format("@ctx_UserName_{0}", counter);
                    cmd.Parameters.Add(ctx, s);

                    //avoid dupes
                    sql += string.Format("IF NOT EXISTS (SELECT * FROM [ProductAccessUser] WHERE [TProductAccessId] = @productAccessId AND [UserName] = {0}) BEGIN ", ctx);
                    sql += "INSERT [ProductAccessUser] ([dtStamp], [TProductAccessId], [UserName], [iQuantityAllowed], [Referral], [Instructions]) ";
                    sql += string.Format("SELECT @dateNow, @productAccessId, {0}, @qty, @referral, @instructions ", ctx);
                    sql += "END ";
                    counter++;
                }

                sql += "UPDATE [ProductAccessUser] SET [UserId] = ISNULL(au.[UserId], null) ";
                sql += "FROM [ProductAccessUser] pau LEFT OUTER JOIN [aspnet_Users] au ON au.[UserName] = pau.[UserName] ";
                sql += "WHERE pau.[tProductAccessId] = @productAccessId AND pau.[UserId] IS NULL ";

                clearSelections = true;
            }

            #endregion

            else
            {
                //get a list of selected rows to apply changes to
                selectedIds = GridUsers_GetSelectedRows();

                if (selectedIds.Count > 0)
                {
                    string idList = string.Join(",", selectedIds.ConvertAll(s => s.ToString()).ToArray());
                    
                    switch (id)
                    {
                        case "btnDeleteSelected":
                            sql = "DELETE FROM [ProductAccessUser] ";

                            //this may be unnecessary - but we don't wan't to have any leftovers
                            clearSelections = true;
                            break;
                        case "btnUpdateQty":
                            custom = CustomQty;
                            Utils.Validation.ValidateIntegerField(_errors, "Quantity", qty);
                            Utils.Validation.ValidateIntegerRange(_errors, "Quantity", qty, 0, 100);
                            if (Utils.Validation.IsInteger(qty))
                                inpQty = int.Parse(qty);

                            if (inpQty < 0)
                                _errors.Add("Quantity must be a positive value.");
                            sql = "UPDATE [ProductAccessUser] SET [iQuantityAllowed] = @qty ";
                            cmd.Parameters.Add("@qty", inpQty, DbType.Int32);
                            break;
                        case "btnUpdateReferral":
                            custom = CustomReferral;
                            sql = "UPDATE [ProductAccessUser] SET [Referral] = @referral ";
                            cmd.Parameters.Add("@referral", (referral.Length > 0) ? referral : null);                
                            break;
                        case "btnUpdateInstructions":
                            custom = CustomInstructions;
                            sql = "UPDATE [ProductAccessUser] SET [Instructions] = @instructions ";
                            cmd.Parameters.Add("@instructions", (instructions.Length > 0) ? instructions : null);
                            break;
                    }

                    sql += "WHERE [Id] IN (SELECT CAST(ListItem AS int) FROM dbo.fn_ListToTable(@idList)) ";
                    cmd.Parameters.Add("@idList", idList);
                }
            }


            //determine if we should show errors or run a query
            if (Utils.Validation.IncurredErrors(_errors, custom))
            {
                return;
            }
            else if (sql.Length > 0)
            {
                cmd.CommandSql = sql;

                try
                {
                    SubSonic.DataService.ExecuteQuery(cmd);
                }
                catch (Exception ex)
                {
                    _errors.Add(ex.Message);

                    if (custom != null && Utils.Validation.IncurredErrors(_errors, custom))
                        return;
                }

                //reset inputs
                txtReferral.Text = string.Empty;
                txtInstructions.Text = string.Empty;
                txtQty.Text = string.Empty;
                txtAddUsers.Text = string.Empty;

                GridUsers.DataBind();

                if (clearSelections)
                    GridUsers_ClearSelectedRows();
                else if (selectedIds != null && selectedIds.Count > 0)
                    GridUsers_ResetSelectedRows(selectedIds);
            }
        }

        protected List<int> GridUsers_GetSelectedRows()
        {
            List<int> list = new List<int>();

            foreach (GridViewRow gvr in GridUsers.Rows)
                if (((CheckBox)gvr.FindControl("chkSelect")).Checked)
                    list.Add((int)GridUsers.DataKeys[gvr.DataItemIndex]["Id"]);

            return list;
        }
        protected void GridUsers_ClearSelectedRows()
        {
            ((CheckBox)GridUsers.HeaderRow.FindControl("chkMaster")).Checked = false;
            foreach (GridViewRow gvr in GridUsers.Rows)
                if(((CheckBox)gvr.FindControl("chkSelect")).Checked)
                    ((CheckBox)gvr.FindControl("chkSelect")).Checked = false;
        }
        protected void GridUsers_ResetSelectedRows(List<int> selectedIds)
        {
            foreach (GridViewRow gvr in GridUsers.Rows)
                if(selectedIds.Contains((int)GridUsers.DataKeys[gvr.DataItemIndex]["Id"]))
                    ((CheckBox)gvr.FindControl("chkSelect")).Checked = true;
        }

        protected void GridUsers_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridView grid = (GridView)sender;
            GridViewRow gvr = e.Row;

            if (gvr.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = (DataRowView)gvr.DataItem;
                object userId = drv["UserId"];

                CheckBox chkSelect = (CheckBox)gvr.FindControl("chkSelect");
                if (chkSelect != null)
                {

                }

                CheckBox chkRegistered = (CheckBox)gvr.FindControl("chkRegistered");
                if (chkRegistered != null)
                    chkRegistered.Checked = userId.ToString().Length > 0;
            }
        }

        #endregion
        
        protected void SqlAccess_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters["@appId"].Value = Wcss._Config.APPLICATION_ID;
        }
}
}
