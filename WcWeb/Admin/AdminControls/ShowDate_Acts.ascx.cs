using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

using Wcss;

namespace WillCallWeb.Admin.AdminControls
{
    public partial class ShowDate_Acts : BaseControl, IPostBackEventHandler
    {
        //handles description update from iframe
        void IPostBackEventHandler.RaisePostBackEvent(string eventArgument)
        {
            string[] args = eventArgument.Split('~');
            string command = args[0];
            int idx = (args.Length > 1 && Utils.Validation.IsInteger((string)args[1])) ? int.Parse(args[1]) : 0;
            string result = string.Empty;

            switch (command.ToLower())
            {
                case "rebind":
                    FormView1.DataBind();
                    litDesc.DataBind();
                    break;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Atx.CurrentShowRecord == null)
                base.Redirect("/Admin/ShowEditor.aspx");

            litShowTitle.Text = Atx.CurrentShowRecord.Name;

            if (!IsPostBack)
            {   
                GridView1.DataBind();
                rdoBilling.DataBind();
                litDesc.DataBind();
            }

            btnWys.ToolTip = string.Format("/Admin/AdminControls/Wysiwyg/Wysiwyg.aspx?context=sb&ctrl={0}", this.UniqueID);
        }
        protected void rdoBilling_DataBound(object sender, EventArgs e)
        {
            RadioButtonList rbl = (RadioButtonList)sender;

            //get selected date
            int idx = (int)GridView1.SelectedDataKey["Id"];
            ShowDate sd = (ShowDate)Atx.CurrentShowRecord.ShowDateRecords().Find(idx);

            string dateBillingMethod = "Auto";

            if(sd != null)
            {
                dateBillingMethod = (Atx.CurrentShowRecord.OverrideActBilling) ? "Custom" : (sd.IsAutoBilling) ? "Auto" : "Legacy";
            }

            rbl.SelectedIndex = -1;
            ListItem li = rbl.Items.FindByValue(dateBillingMethod);
            if (li != null)
                li.Selected = true;
            else
                rbl.SelectedIndex = 0;//auto
        }
        protected void litDesc_DataBinding(object sender, EventArgs e)
        {
            Literal lit = (Literal)sender;
            lit.Visible = (rdoBilling.SelectedValue.ToLower() == "custom");

            if(lit.Visible)
                lit.Text = string.Format("<div class=\"desc-control\">{0}</div>", (Atx.CurrentShowRecord.ActBilling != null) ? Atx.CurrentShowRecord.ActBilling : string.Empty);
        }
        protected void rdoBilling_SelectedIndexChanged(object sender, EventArgs e)
        {
            RadioButtonList rbl = (RadioButtonList)sender;
            string selection = rbl.SelectedValue.ToLower();

            //change the showdates billing method
            string sql = "UPDATE ";
            if (selection == "custom")
                sql += " [Show] SET [bOverrideActBilling] = 1 WHERE [Id] = @showIdx; ";
            else
            {
                sql += " [Show] SET [bOverrideActBilling] = 0 WHERE [Id] = @showIdx; ";
                sql += string.Format("UPDATE [ShowDate] SET [bAutoBilling] = {0} ", (selection == "auto") ? "1" : "0");
                sql += " WHERE [Id] = @dateIdx ";
            }

            SubSonic.QueryCommand cmd = new SubSonic.QueryCommand(sql, SubSonic.DataService.Provider.Name);
            cmd.Parameters.Add("@showIdx", Atx.CurrentShowRecord.Id, DbType.Int32);
            int dateIdx = (int)GridView1.SelectedDataKey["Id"];            
            cmd.Parameters.Add("@dateIdx", dateIdx, DbType.Int32);

            try
            {
                SubSonic.DataService.ExecuteQuery(cmd);

                //reset show data
                int index = Atx.CurrentShowRecord.Id;
                Atx.SetCurrentShowRecord(index);
            }
            catch (Exception ex)
            {
                _Error.LogException(ex);
            }

            //rebind controls
            GridView1.DataBind();
            litDesc.DataBind();
        }
       
        #region GridView1 - Show Date Listing

        protected void GridView1_DataBound(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;

            if (grid.Rows.Count > 0 && grid.SelectedIndex == -1)
                grid.SelectedIndex = 0;

            //set auto bill according to selection
            //btnAuto.DataBind();
        }
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (FormView1.CurrentMode != FormViewMode.Edit)
                FormView1.ChangeMode(FormViewMode.Edit);
        }
        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;

            FormView1.DataBind();
        }

        #endregion

        #region GridViewEntity - JShowActListing

        protected void GridViewEntity_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridView grid = (GridView)sender;
            DataRowView rowView = (DataRowView)e.Row.DataItem;

            if (rowView != null)
            {
                DataRow row = rowView.Row;
                LinkButton select = (LinkButton)e.Row.FindControl("btnSelect");
                LinkButton up = (LinkButton)e.Row.FindControl("btnUp");
                LinkButton down = (LinkButton)e.Row.FindControl("btnDown");
                LinkButton delete = (LinkButton)e.Row.FindControl("btnDelete");

                string actName = string.Empty;

                if(row != null)
                    actName = System.Text.RegularExpressions.Regex.Replace(string.Format("{0} {1} {2} {3} {4}",
                        row.ItemArray.GetValue(row.Table.Columns.IndexOf("PreText")),
                        row.ItemArray.GetValue(row.Table.Columns.IndexOf("ActName")),
                        row.ItemArray.GetValue(row.Table.Columns.IndexOf("ActText")),
                        row.ItemArray.GetValue(row.Table.Columns.IndexOf("Featuring")),
                        row.ItemArray.GetValue(row.Table.Columns.IndexOf("PostText"))), @"\s+", " ").Trim();

                Literal litDesc = (Literal)e.Row.FindControl("litDesc");
                if (litDesc != null)
                    litDesc.Text = actName;

                if (delete != null)
                    delete.OnClientClick = string.Format("return confirm('Are you sure you want to delete {0} from this show?')",
                        Utils.ParseHelper.ParseJsAlert(actName));

                if (up != null && down != null)
                {
                    up.Enabled = (e.Row.RowIndex > 0);

                    ShowDate selectedDate = (ShowDate)Atx.CurrentShowRecord.ShowDateRecords().Find(GridView1.SelectedValue);
                    
                    int rowCount = (selectedDate != null) ? selectedDate.JShowActRecords().Count : 0;

                    down.Enabled = (e.Row.RowIndex < rowCount - 1);
                }
            }
        }
        protected void GridViewEntity_DataBound(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;

            if (grid.Rows.Count > 0 && grid.SelectedIndex == -1)
                grid.SelectedIndex = 0;

            litShowTitle.Text = Atx.CurrentShowRecord.Name;
        }
        protected void GridViewEntity_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridView grid = (GridView)sender;
            string cmd = e.CommandName.ToLower();

            switch (cmd)
            {
                case "up":
                case "down":
                    ShowDate selectedDate = (ShowDate)Atx.CurrentShowRecord.ShowDateRecords().Find(GridView1.SelectedValue);
                    if (selectedDate != null)
                    {
                        JShowAct moved = selectedDate.JShowActRecords().ReorderItem(int.Parse(e.CommandArgument.ToString()), cmd);
                        ////set the index of the moved item
                        grid.SelectedIndex = moved.DisplayOrder;
                        grid.DataBind();
                    }
                    break;
            }

            if (FormView1.CurrentMode != FormViewMode.Edit)
                FormView1.ChangeMode(FormViewMode.Edit);
        }
        protected void GridViewEntity_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridView grid = (GridView)sender;
            CustomValidator validation = (CustomValidator)grid.FindControl("CustomValidation");

            //cannot delete final only act
            if (grid.Rows.Count <= 1)
            {
                if (validation != null)
                {
                    validation.IsValid = false;
                    validation.ErrorMessage = string.Format("The only act cannot be deleted. If you wish, you may add a new act and then delete this act.");
                }
                e.Cancel = true;
            }
            else
            {
                int idx = (int)grid.DataKeys[e.RowIndex].Value;

                try
                {
                    ShowDate selectedDate = (ShowDate)Atx.CurrentShowRecord.ShowDateRecords().Find(GridView1.SelectedValue);
               
                    e.Cancel = ((selectedDate == null) || (!selectedDate.JShowActRecords().DeleteFromCollection(idx)));

                    //reset show data
                    int index = Atx.CurrentShowRecord.Id;
                    Atx.SetCurrentShowRecord(index);
                }
                catch (Exception ex)
                {
                    _Error.LogException(ex);
                    if (validation != null)
                    {
                        validation.IsValid = false;
                        validation.ErrorMessage = ex.Message;
                    }

                    e.Cancel = true;
                }

                if (!e.Cancel)
                {
                    grid.SelectedIndex = e.RowIndex - 1;
                    grid.DataBind();
                }
            }
        }

        #endregion

        #region FormView1

        protected void FormView1_DataBound(object sender, EventArgs e)
        {
            FormView form = (FormView)sender;

            if (form.DataItemCount == 0 && form.CurrentMode != FormViewMode.Insert)
            {
                form.ChangeMode(FormViewMode.Insert);
                return;
            }

            if (form.CurrentMode == FormViewMode.Insert)
            {
                Panel pnlCopy = (Panel)form.FindControl("pnlCopy");
                if (pnlCopy != null && Atx.CurrentShowRecord.ShowDateRecords().Count > 1)
                {
                    //Do not add package tickets here! It would be very messy to manage!
                    JShowActCollection coll = new JShowActCollection();
                    
                    ShowDate selectedDate = (ShowDate)Atx.CurrentShowRecord.ShowDateRecords().Find(int.Parse(GridView1.SelectedValue.ToString()));

                    if (selectedDate != null)
                    {
                        foreach (ShowDate sd in Atx.CurrentShowRecord.ShowDateRecords())
                        {
                            //if we have a different date
                            if (sd.Id != selectedDate.Id)
                            {
                                foreach (JShowAct jsa in sd.JShowActRecords())
                                {
                                    //if it is not in the collection...
                                    int thisDateContains = selectedDate.JShowActRecords().GetList().FindIndex(delegate(JShowAct match) { return (match.TActId == jsa.TActId); });
                                    if (thisDateContains < 0)
                                    {
                                        int exists = coll.GetList().FindIndex(delegate(JShowAct match) { return (match.TActId == jsa.TActId); });

                                        if (exists < 0)
                                            coll.Add(jsa);
                                    }
                                }
                            }
                        }
                    }

                    if (coll.Count > 0)
                    {
                        pnlCopy.Visible = true;

                        //find the ddl and bind
                        DropDownList ddlCopy = (DropDownList)form.FindControl("ddlCopyAct");
                        if (ddlCopy != null)
                        {
                            ddlCopy.DataSource = coll;
                            ddlCopy.DataTextField = "DisplayNameWithAttributes";
                            ddlCopy.DataValueField = "Id";

                            ddlCopy.DataBind();
                            ddlCopy.Items.Insert(0, new ListItem("<-- Select an act to copy -->", "0"));
                        }
                    }
                    else
                        pnlCopy.Visible = false;                    
                }
                else if(pnlCopy != null)
                    pnlCopy.Visible = false;
            }
        }
        protected void FormView1_ItemInserting(object sender, FormViewInsertEventArgs e)
        {
            FormView form = (FormView)sender;

            Editor_Act eAct = (Editor_Act)form.FindControl("Editor_Act1");

            if (eAct != null)
            {
                int idx = eAct.SelectedIdx;

                try
                {
                    if (idx == 0)
                        throw new Exception("Please make a selection before attempting to add.");

                    ShowDate selectedDate = (ShowDate)Atx.CurrentShowRecord.ShowDateRecords().Find(GridView1.SelectedValue);

                    if (selectedDate != null)
                    {
                        System.Web.Security.MembershipUser mem = System.Web.Security.Membership.GetUser(Profile.UserName);
                        selectedDate.JShowActRecords().AddActToCollection(selectedDate, idx, eAct.SelectedName, mem.UserName, (Guid)mem.ProviderUserKey);
//                        selectedDate.JShowActRecords().AddActToCollection(selectedDate.Id, idx);
                        form.ChangeMode(FormViewMode.Edit);

                        GridViewEntity.SelectedIndex = selectedDate.JShowActRecords().Count - 1;
                        GridViewEntity.DataBind();
                    }
                }
                catch (Exception ex)
                {
                    e.Cancel = true;
                    _Error.LogException(ex);

                    CustomValidator validation = (CustomValidator)form.FindControl("CustomValidation");
                    if (validation != null)
                    {
                        validation.IsValid = false;
                        validation.ErrorMessage = ex.Message;
                    }
                }
            }
        }
        protected void FormView1_ItemUpdated(object sender, FormViewUpdatedEventArgs e)
        {
            //reset show data
            int index = Atx.CurrentShowRecord.Id;
            Atx.SetCurrentShowRecord(index);

            GridViewEntity.DataBind();
        }
        protected void FormView1_ItemCommand(object sender, FormViewCommandEventArgs e)
        {
            FormView form = (FormView)sender;

            string command = e.CommandName.ToLower();

            switch (command)
            {
                case "viewsales":
                    base.Redirect(string.Format("/Admin/Listings.aspx?p=tickets&shodateid={0}", 
                        (GridView1.SelectedValue != null) ? GridView1.SelectedValue.ToString() : "0"));
                    break;
                case "new":
                    Atx.CurrentActId = 0;
                    break;
                case "copy":
                    //ensure valid input
                    DropDownList ddlCopy = (DropDownList)form.FindControl("ddlCopyAct");
                    if (ddlCopy != null)//aslo ensures that we are in insert mode
                    {
                        int idx = int.Parse(ddlCopy.SelectedValue);

                        try
                        {
                            if (idx == 0)
                                throw new Exception("You must select an act to copy in order to copy.");

                            //get ticket chosen
                            JShowAct chosen = JShowAct.FetchByID(idx);

                            if (chosen == null)
                                throw new Exception("Sorry, that act could not be found.");

                            //copy ticket info and add to this date
                            ShowDate selectedDate = (ShowDate)Atx.CurrentShowRecord.ShowDateRecords().Find(GridView1.SelectedValue);
                            if (selectedDate != null)
                            {
                                System.Web.Security.MembershipUser mem = System.Web.Security.Membership.GetUser(Profile.UserName);
                                JShowAct copied = selectedDate.JShowActRecords().AddActToCollection(selectedDate, chosen.TActId, 
                                    chosen.ActRecord.Name, mem.UserName, (Guid)mem.ProviderUserKey);

                                //JShowAct copied = selectedDate.JShowActRecords().AddActToCollection(selectedDate.Id, chosen.TActId);

                                copied.PreText = chosen.PreText;
                                copied.ActText = chosen.ActText;
                                copied.PostText = chosen.PostText;
                                copied.DtStamp = DateTime.Now;

                                form.ChangeMode(FormViewMode.Edit);

                                //ensure Show has latest data - refresh
                                int index = Atx.CurrentShowRecord.Id;
                                Atx.SetCurrentShowRecord(index);

                                GridViewEntity.SelectedIndex = copied.DisplayOrder;
                                GridViewEntity.DataBind();
                            }
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
                        }
                    }
                    break;
            }
        }

        #endregion

        protected void btnChangeShowName_Click(object sender, EventArgs e)
        {
            if (!Atx.CurrentShowRecord.ShowNameMatches(false))
            {
                Atx.CurrentShowRecord.ShowNameMatches(true);

                AdminEvent.OnShowNameChanged(this);

                Atx.ResetCurrentShowRecord();

                litShowTitle.Text = Atx.CurrentShowRecord.Name;
            }
        }
}
}