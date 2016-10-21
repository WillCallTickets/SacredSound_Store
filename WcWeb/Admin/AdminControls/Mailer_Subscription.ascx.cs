using System;
using System.Data;
using System.Web.Security;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using Wcss;

namespace WillCallWeb.Admin.AdminControls
{
    public partial class Mailer_Subscription : BaseControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GridView1.DataBind();
            }
        }

        #region Grid View

        protected void GridView1_DataBinding(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;

            SubscriptionCollection coll = new SubscriptionCollection();
            coll.AddRange(_Lookits.Subscriptions);
            if (coll.Count > 1)
                coll.Sort("DtStamp", true);

            grid.DataSource = coll;
        }        
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridView grid = (GridView)sender;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {  
            }
        }
        protected void GridView1_DataBound(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;

            if (grid.Rows.Count > 0 && grid.SelectedIndex == -1)
                grid.SelectedIndex = 0;

            FormView1.DataBind();
        }
        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            FormView1.DataBind();
        }

        #endregion

        #region Form View

        protected void FormView1_DataBinding(object sender, EventArgs e)
        {
            FormView form = (FormView)sender;

            SubscriptionCollection coll = new SubscriptionCollection();
            int idx = (GridView1.SelectedDataKey != null && GridView1.SelectedDataKey["Id"] != null) ? int.Parse(GridView1.SelectedDataKey["Id"].ToString()) : 0;
            if (idx > 0)
            {
                Subscription addSub = (Subscription)_Lookits.Subscriptions.Find(idx);
                if(addSub != null)
                    coll.Add(addSub);
            }

            form.DataSource = coll;
        }
        protected void FormView1_DataBound(object sender, EventArgs e)
        {
            FormView form = (FormView)sender;
            Subscription sub = (Subscription)form.DataItem;

            Button delete = (Button)form.FindControl("btnDelete");
            if (delete != null && sub != null)
                delete.OnClientClick = string.Format("return confirm('Are you sure you want to delete {0}?')",
                    Utils.ParseHelper.ParseJsAlert(sub.Name));


            //only subscriptions that are for web users can be set as default
            Button setDefault = (Button)form.FindControl("btnSetDefault");
            if (setDefault != null && sub != null)
                setDefault.Enabled = (!sub.IsDefault && sub.AspnetRoleRecord.RoleName == "WebUser");


            DropDownList roles = (DropDownList)form.FindControl("ddlRoles");
            if (roles != null)
            {
                //roles.DataBind();
                if (sub != null)
                {
                    ListItem li = roles.Items.FindByText(sub.AspnetRoleRecord.RoleName);
                    if (li != null)
                    {
                        roles.SelectedIndex = -1;
                        li.Selected = true;
                    }
                }
                else
                {
                    ListItem li = roles.Items.FindByText("Administrator");
                    if (li != null)
                    {
                        roles.SelectedIndex = -1;
                        li.Selected = true;
                    }
                }
            }
        }
        protected void FormView1_ModeChanging(object sender, FormViewModeEventArgs e)
        {
            FormView form = (FormView)sender;
            form.ChangeMode(e.NewMode);

            if (e.CancelingEdit)
                form.DataBind();
        }
        protected void FormView1_ItemInserting(object sender, FormViewInsertEventArgs e)
        {
            if (Page.IsValid)
            {
                FormView form = (FormView)sender;

                //validate input
                bool isActive = false;
                CheckBox active = (CheckBox)form.FindControl("chkActive");
                if (active != null)
                    isActive = active.Checked;

                string name = string.Empty;
                TextBox txtName = (TextBox)form.FindControl("txtName");
                if (txtName != null)
                {
                    name = txtName.Text.Trim();
                    if (name.Length == 0)
                    {
                        RequiredFieldValidator req = (RequiredFieldValidator)form.FindControl("RequiredName");
                        if (req != null)
                        {
                            req.IsValid = false;
                            return;
                        }
                    }
                }

                string description = string.Empty;
                TextBox txtDesc = (TextBox)form.FindControl("txtDescription");
                if (txtDesc != null)
                    description = txtDesc.Text.Trim();

                string internalDescription = string.Empty;
                TextBox txtInternalDesc = (TextBox)form.FindControl("txtInternalDescription");
                if (txtInternalDesc != null)
                    internalDescription = txtInternalDesc.Text.Trim();

                string role = string.Empty;
                DropDownList roles = (DropDownList)form.FindControl("ddlRoles");
                if (roles != null)
                    role = roles.SelectedValue;

                bool subscribeAll = false;
                CheckBox sub = (CheckBox)form.FindControl("chkSubscribe");
                if (sub != null)
                    subscribeAll = sub.Checked;

                SubSonic.QueryCommand qry = new SubSonic.QueryCommand(string.Empty, SubSonic.DataService.Provider.Name);
                System.Text.StringBuilder sb = new System.Text.StringBuilder();

                qry.Parameters.Add("@active", isActive, DbType.Boolean);
                qry.Parameters.Add("@name", name);
                qry.Parameters.Add("@description", description);
                qry.Parameters.Add("@internalDescription", internalDescription);
                qry.Parameters.Add("@role", role);
                qry.Parameters.Add("@subscribeAll", subscribeAll, DbType.Boolean);
                qry.Parameters.Add("@appId", _Config.APPLICATION_ID, DbType.Guid);

                sb.Append("DECLARE @insertId int, @roleId uniqueidentifier; ");
                //create subscription
                sb.Append("SELECT @roleId = r.[RoleId] FROM Aspnet_Roles r WHERE r.[ApplicationId] = @appId AND r.[RoleName] = @role; ");
                sb.Append("INSERT INTO Subscription(ApplicationId, RoleId, bActive, Name, Description, InternalDescription) ");
                sb.Append("VALUES (@appId, @roleId, @active, @name, @description, @internalDescription); ");
                sb.Append("SET @insertId = SCOPE_IDENTITY(); ");

                //create user subscriptions for all users within role - default html
                sb.Append("INSERT SubscriptionUser(UserId, TSubscriptionId, bSubscribed, bHtmlFormat) ");
                sb.Append("SELECT u.[UserId], @insertId, @subscribeAll, 1 ");
                sb.Append("FROM Aspnet_UsersInRoles u ");
                sb.Append("WHERE u.[RoleId] = @roleId ");

                //return the id of the new item
                sb.Append("SELECT @insertId ");

                qry.CommandSql = sb.ToString();

                try
                {
                    int idx = (int)SubSonic.DataService.ExecuteScalar(qry);
                    //refresh lookits
                    _Lookits.RefreshLookup(_Enums.LookupTableNames.Subscriptions.ToString());

                    form.ChangeMode(FormViewMode.Edit);
                    GridView1.SelectedIndex = GridView1.Rows.Count;//works for zero based index
                    GridView1.DataBind();
                }
                catch (Exception ex)
                {
                    _Error.LogException(ex);

                    CustomValidator validation = (CustomValidator)form.FindControl("CustomValidation");
                    if (validation != null)
                    {
                        validation.IsValid = false;
                        validation.ErrorMessage = ex.Message;
                    }
                    e.Cancel = true;
                }
            }
        }
        protected void FormView1_ItemUpdating(object sender, FormViewUpdateEventArgs e)
        {
            FormView form = (FormView)sender;

            int idx = int.Parse(form.SelectedValue.ToString());

            bool isActive = false;
            CheckBox active = (CheckBox)form.FindControl("chkActive");
            if (active != null)
                isActive = active.Checked;

            string name = string.Empty;
            TextBox txtName = (TextBox)form.FindControl("txtName");
            if (txtName != null)
            {
                name = txtName.Text.Trim();
                if (name.Length == 0)
                {
                    RequiredFieldValidator req = (RequiredFieldValidator)form.FindControl("RequiredName");
                    if (req != null)
                    {
                        req.IsValid = false;
                        return;
                    }
                }
            }

            string description = string.Empty;
            TextBox txtDesc = (TextBox)form.FindControl("txtDescription");
            if (txtDesc != null)
                description = txtDesc.Text.Trim();

            string internalDescription = string.Empty;
            TextBox txtInternalDesc = (TextBox)form.FindControl("txtInternalDescription");
            if (txtInternalDesc != null)
                internalDescription = txtInternalDesc.Text.Trim();

            SubSonic.QueryCommand qry = new SubSonic.QueryCommand(string.Empty, SubSonic.DataService.Provider.Name);
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            qry.Parameters.Add("@active", isActive, DbType.Boolean);
            qry.Parameters.Add("@name", name);
            qry.Parameters.Add("@description", description);
            qry.Parameters.Add("@internalDescription", internalDescription);
            qry.Parameters.Add("@idx", idx, DbType.Int32);

            sb.Append("UPDATE Subscription ");
            sb.Append("SET [bActive] = @active, [Name] = @name, [Description] = @description, [InternalDescription] = @internalDescription ");
            sb.Append("WHERE [Id] = @idx ");

            qry.CommandSql = sb.ToString();

            try
            {
                SubSonic.DataService.ExecuteScalar(qry);
                //refresh lookits
                _Lookits.RefreshLookup(_Enums.LookupTableNames.Subscriptions.ToString());

                form.ChangeMode(FormViewMode.Edit);
                GridView1.DataBind();
            }
            catch (Exception ex)
            {
                _Error.LogException(ex);
                CustomValidator validation = (CustomValidator)form.FindControl("CustomValidation");
                if (validation != null)
                {
                    validation.IsValid = false;
                    validation.ErrorMessage = ex.Message;
                }
                e.Cancel = true;
            }
        }
        protected void FormView1_ItemCommand(object sender, FormViewCommandEventArgs e)
        {
            FormView form = (FormView)sender;

            string cmd = e.CommandName.ToLower();

            switch (cmd)
            {
                case "default":
                    int idx = int.Parse(e.CommandArgument.ToString());

                    foreach (Subscription sub in _Lookits.Subscriptions)
                    {
                        if (sub.Id != idx && sub.IsDefault)
                        {
                            sub.IsDefault = false;
                            sub.Save();
                        }
                        else if (sub.Id == idx && (!sub.IsDefault))
                        {
                            sub.IsDefault = true;
                            sub.Save();
                        }   
                    }

                    _Lookits.RefreshLookup(_Enums.LookupTableNames.Subscriptions.ToString());
                    GridView1.DataBind();
                    break;
            }
        }
        protected void FormView1_ItemDeleting(object sender, FormViewDeleteEventArgs e)
        {
            FormView form = (FormView)sender;

            int idx = int.Parse(form.SelectedValue.ToString());
            if (idx > 0)
            {
                Subscription deleter = (Subscription)_Lookits.Subscriptions.Find(idx);
                if (deleter != null)
                {
                    //clean up mailqueue
                    System.Text.StringBuilder del = new System.Text.StringBuilder();
                    //get a list of affected subscription emails
                    del.Append("CREATE TABLE #tmpSubEmails(Id int); INSERT #tmpSubEmails(Id) ");
                    del.Append("SELECT  DISTINCT se.[Id] FROM [SubscriptionEmail] se WHERE se.[TSubscriptionId] = @subId ");

                    //remove all mailqueued items that have not been processed - processed items will be moved to the archive
                    //params will cascade delete
                    del.Append("DELETE FROM [MailQueue] WHERE [TSubscriptionEmailId] IN (SELECT [Id] FROM #tmpSubEmails) AND [DateProcessed] IS NULL ");

                    del.Append("INSERT EmailParamArchive ([Id], [Name], [Value], [TMailQueueId], [dtStamp]) ");
                    del.Append("SELECT ep.[Id], ep.[Name], ep.[Value], ep.[TMailQueueId], ep.[dtStamp] ");
                    del.Append("FROM [EmailParam] ep, [MailQueue] mq WHERE mq.[TSubscriptionEmailId] IN (SELECT [Id] FROM #tmpSubEmails) ");
                    del.Append("AND mq.[Id] = ep.[TMailQueueId] ");

                    del.Append("INSERT MailQueueArchive ([ApplicationId], [Id], [dtStamp], [DateToProcess], [DateProcessed], [FromName], ");
                    del.Append("[FromAddress], [ToAddress], [CC], [BCC], [Status], [TEmailLetterId], ");
                    del.Append("[TSubscriptionEmailId], [Priority], [bMassMailer], [Threadlock], [AttemptsRemaining]) ");
                    del.Append("SELECT mq.[ApplicationId], mq.[Id], mq.[dtStamp], mq.[DateToProcess], mq.[DateProcessed], mq.[FromName], mq.[FromAddress], ");
                    del.Append("mq.[ToAddress], mq.[CC], mq.[BCC], mq.[Status], mq.[TEmailLetterId], mq.[TSubscriptionEmailId], mq.[Priority], ");
                    del.Append("mq.[bMassMailer], mq.[Threadlock], mq.[AttemptsRemaining] ");
                    del.Append("FROM [MailQueue] mq WHERE mq.[TSubscriptionEmailId] IN (SELECT [Id] FROM #tmpSubEmails) ");

                    //final cleanup -params will cascade delete
                    del.Append("DELETE FROM [MailQueue] WHERE [TSubscriptionEmailId] IN (SELECT [Id] FROM #tmpSubEmails) ");

                    SubSonic.QueryCommand cmd = new SubSonic.QueryCommand(del.ToString(), SubSonic.DataService.Provider.Name);
                    cmd.Parameters.Add("@subId", deleter.Id, DbType.Int32);

                    SubSonic.DataService.ExecuteQuery(cmd);

                    //clean up subscription email - done by cascade
                    //clean up subscription user - done by cascade
                    

                    string sql = "DELETE FROM [Subscription] WHERE [Id] = @deleteId ";
                    SubSonic.QueryCommand qry = new SubSonic.QueryCommand(sql, SubSonic.DataService.Provider.Name);
                    qry.AddParameter("@deleteId", idx, DbType.Int32);

                    try
                    {
                        int affected = (int)SubSonic.DataService.ExecuteQuery(qry);

                        _Lookits.RefreshLookup(_Enums.LookupTableNames.Subscriptions.ToString());
                        GridView1.SelectedIndex -= 1;
                        GridView1.DataBind();
                    }
                    catch (Exception ex)
                    {
                        _Error.LogException(ex);
                        CustomValidator validation = (CustomValidator)form.FindControl("CustomValidation");
                        if (validation != null)
                        {
                            validation.IsValid = false;
                            validation.ErrorMessage = ex.Message;
                        }
                        e.Cancel = true;
                    }
                }
            }
        }
        protected void Roles_DataBinding(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            string[] roles = Roles.GetAllRoles();
            List<string> valid = new List<string>();
            foreach (string s in roles)
                if (s.ToLower().Trim() != "super")
                    valid.Add(s);

            ddl.DataSource = valid;
        }
        protected void Roles_DataBound(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            //if the form is in insert mode...
            if (FormView1.CurrentMode == FormViewMode.Insert || (ddl.Items.Count > 0 && ddl.SelectedIndex == -1))
            {
                //ListItem li = ddl.Items.FindByText("Administrator");
                //if(li != null)
                //    li.Selected = true;
                //if(FormView1.CurrentMode == in)
                ddl.SelectedIndex = 0;
            }
        }

        #endregion
        
}
}