using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using Wcss;

namespace WillCallWeb.Admin.AdminControls
{
    public partial class ShowDate_TicketPostPurchase : BaseControl, IPostBackEventHandler
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
                    int index = Atx.CurrentShowRecord.Id;
                    Atx.SetCurrentShowRecord(index);
                    GridPost.DataBind();
                    if (FormPost.CurrentMode != FormViewMode.Edit)
                        FormPost.ChangeMode(FormViewMode.Edit);
                    break;
            }
        }

        List<string> _errors = new List<string>();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Atx.CurrentShowRecord == null)
                base.Redirect("/Admin/ShowEditor.aspx");

            litShowTitle.Text = Atx.CurrentShowRecord.Name;

            if (!IsPostBack)
            {
                GridDates.DataBind();
            }
        }

        #region GridDates

        protected void GridDates_DataBound(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;

            if (grid.Rows.Count > 0 && grid.SelectedIndex == -1)
                grid.SelectedIndex = 0;

            GridTickets.DataBind();
        }
        protected void GridDates_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (FormPost.CurrentMode != FormViewMode.Edit)
                FormPost.ChangeMode(FormViewMode.Edit);
        }
        protected void GridDates_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;

            GridTickets.DataBind();
        }

        #endregion

        #region GridTickets

        protected void GridTickets_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridView grid = (GridView)sender;
            DataRowView rowView = (DataRowView)e.Row.DataItem;
            ShowDate selectedDate = (ShowDate)Atx.CurrentShowRecord.ShowDateRecords().Find(GridDates.SelectedValue);

            if (rowView != null)
            {
                DataRow row = rowView.Row;

                LinkButton select = (LinkButton)e.Row.FindControl("btnSelect");

                CheckBox pkg = (CheckBox)e.Row.FindControl("chkPackage");
                int linkCount = (int)row.ItemArray.GetValue(row.Table.Columns.IndexOf("LinkCount"));
                pkg.Checked = linkCount > 0;

                Literal vendor = (Literal)e.Row.FindControl("litVendor");
                int vendorId = (int)row.ItemArray.GetValue(row.Table.Columns.IndexOf("TVendorId"));

                Vendor v = (Vendor)_Lookits.Vendors.Find(vendorId);
                if (vendor != null && v != null)
                    vendor.Text = v.Name;

                Literal litAvailable = (Literal)e.Row.FindControl("litAvailable");
                if (litAvailable != null)
                {
                    int avail = (int)row.ItemArray.GetValue(row.Table.Columns.IndexOf("iAllotment")) -
                        (int)row.ItemArray.GetValue(row.Table.Columns.IndexOf("pendingStock")) -
                        (int)row.ItemArray.GetValue(row.Table.Columns.IndexOf("iSold"));
                    litAvailable.Text = avail.ToString();
                }

                string theName = string.Empty;

                //ent.[PriceText], ent.[mPrice], ent.[DosText], ent.[mDosPrice], ent.[mServiceCharge], a.[Name] as 'AgeName', ent.[CriteriaText],
                if (row != null)
                {
                    string criteria = row.ItemArray.GetValue(row.Table.Columns.IndexOf("CriteriaText")).ToString();
                    string description = row.ItemArray.GetValue(row.Table.Columns.IndexOf("SalesDescription")).ToString();

                    if (criteria != null && criteria.Trim().Length > 75)
                        criteria = string.Format("{0}...", criteria.Trim().Substring(0, 72).Trim());
                    if (description != null && description.Trim().Length > 75)
                        description = string.Format("{0}...", description.Trim().Substring(0, 72).Trim());

                    theName = System.Text.RegularExpressions.Regex.Replace(string.Format("{0} {1} svc {2}<div>Ages {3}</div>{4}{5}",
                        row.ItemArray.GetValue(row.Table.Columns.IndexOf("PriceText")),
                        ((decimal)row.ItemArray.GetValue(row.Table.Columns.IndexOf("mPrice"))).ToString("c"),
                        ((decimal)row.ItemArray.GetValue(row.Table.Columns.IndexOf("mServiceCharge"))).ToString("n2"),
                        row.ItemArray.GetValue(row.Table.Columns.IndexOf("AgeName")),
                        (description != null && description.Trim().Length > 0) ? string.Format("<div>{0}</div>", description.Trim()) : string.Empty,
                        (criteria != null && criteria.Trim().Length > 0) ? criteria.Trim() : string.Empty),
                        @"\s+", " ").Trim();
                }

                Literal litDesc = (Literal)e.Row.FindControl("litDesc");
                if (litDesc != null && theName.Trim().Length > 0)
                    litDesc.Text = theName;
            }
        }
        protected void GridTickets_DataBound(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;

            if(grid.Rows.Count == 0)
                base.Redirect("/Admin/ShowEditor.aspx?p=tickets");

            if (grid.Rows.Count > 0 && grid.SelectedIndex == -1)
                grid.SelectedIndex = 0;

            GridPost.DataBind();
        }
        protected void GridTickets_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (FormPost.CurrentMode != FormViewMode.Edit)
                FormPost.ChangeMode(FormViewMode.Edit);
        }

        #endregion

        #region GridPost

        protected void GridPost_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridView grid = (GridView)sender;
            DataRowView rowView = (DataRowView)e.Row.DataItem;

            if (rowView != null)
            {
                DataRow row = rowView.Row;
                LinkButton up = (LinkButton)e.Row.FindControl("btnUp");
                LinkButton down = (LinkButton)e.Row.FindControl("btnDown");
                LinkButton delete = (LinkButton)e.Row.FindControl("btnDelete");

                Literal litDesc = (Literal)e.Row.FindControl("litDesc");

                if (row != null && litDesc != null)
                {
                    string inProcess = row.ItemArray.GetValue(row.Table.Columns.IndexOf("InProcessDescription")).ToString();
                    string postText = row.ItemArray.GetValue(row.Table.Columns.IndexOf("PostText")).ToString();

                    if (inProcess != null && inProcess.Trim().Length > 0)
                        litDesc.Text = string.Format("{0}<hr />", inProcess.Trim());

                    litDesc.Text += postText;
                }

                if (delete != null)
                    delete.OnClientClick = string.Format("return confirm('Are you sure you want to delete this post purchase Text?')");

                if (up != null && down != null)
                {
                    int rowIdx = e.Row.RowIndex;

                    up.Enabled = (rowIdx > 0);

                    int postCount = 0;
                    int selectedDateId = (GridDates.SelectedValue != null) ? int.Parse(GridDates.SelectedValue.ToString()) : 0;
                    ShowDate selectedDate = (ShowDate)Atx.CurrentShowRecord.ShowDateRecords().Find(selectedDateId);
                    if (selectedDate != null && GridTickets.SelectedValue != null && (int)GridTickets.SelectedValue > 0)
                    {
                        ShowTicket selectedTicket = (ShowTicket)selectedDate.ShowTicketRecords().Find((int)GridTickets.SelectedValue);

                        if (selectedTicket != null)
                            postCount = selectedTicket.PostPurchaseTextRecords().Count;

                    }
                    down.Enabled = (rowIdx < postCount - 1);
                }
            }
        }
        protected void GridPost_DataBound(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;

            if (grid.Rows.Count > 0 && grid.SelectedIndex == -1)
                grid.SelectedIndex = 0;

            FormPost.DataBind();
        }
        protected void GridPost_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (FormPost.CurrentMode != FormViewMode.Edit)
                FormPost.ChangeMode(FormViewMode.Edit);
        }
        protected void GridPost_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridView grid = (GridView)sender;
            string cmd = e.CommandName.ToLower();

            switch (cmd)
            {
                case "up":
                case "down":
                    int selectedDateId = (GridDates.SelectedValue != null) ? int.Parse(GridDates.SelectedValue.ToString()) : 0;
                    ShowDate selectedDate = (ShowDate)Atx.CurrentShowRecord.ShowDateRecords().Find(selectedDateId);
                    if (selectedDate != null && GridTickets.SelectedValue != null && (int)GridTickets.SelectedValue > 0)
                    {
                        ShowTicket selectedTicket = (ShowTicket)selectedDate.ShowTicketRecords().Find((int)GridTickets.SelectedValue);

                        if (selectedTicket != null)
                        {
                            PostPurchaseText moved = selectedTicket.PostPurchaseTextRecords().ReorderItem(int.Parse(e.CommandArgument.ToString()), cmd);
                            ////set the index of the moved item
                            grid.SelectedIndex = moved.DisplayOrder;
                            grid.DataBind();
                        }
                    }
                    break;
            }

            if (FormPost.CurrentMode != FormViewMode.Edit)
                FormPost.ChangeMode(FormViewMode.Edit);
        }
        protected void GridPost_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridView grid = (GridView)sender;
            CustomValidator validation = (CustomValidator)grid.FindControl("CustomValidation");

            int idx = (int)grid.DataKeys[e.RowIndex].Value;

            try
            {
                int selectedDateId = (GridDates.SelectedValue != null) ? int.Parse(GridDates.SelectedValue.ToString()) : 0;
                ShowDate selectedDate = (ShowDate)Atx.CurrentShowRecord.ShowDateRecords().Find(selectedDateId);
                if (selectedDate != null && GridTickets.SelectedValue != null && (int)GridTickets.SelectedValue > 0)
                {
                    ShowTicket selectedTicket = (ShowTicket)selectedDate.ShowTicketRecords().Find((int)GridTickets.SelectedValue);

                    if (selectedTicket != null)
                    {
                        PostPurchaseText selectedPost = (PostPurchaseText)selectedTicket.PostPurchaseTextRecords().Find(GridPost.SelectedValue);
                        e.Cancel = ((selectedPost == null) || (!selectedTicket.PostPurchaseTextRecords().DeleteFromCollection(idx)));

                        //reset data
                        int index = Atx.CurrentShowRecord.Id;
                        Atx.SetCurrentShowRecord(index);

                    }
                }
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
        protected void GridPost_RowDeleted(object sender, GridViewDeletedEventArgs e)
        {
            GridView grid = (GridView)sender;

            //grid.DataBind();
        }

        #endregion

        #region FormPost

        protected void CreateNewPostPurchase(FormView form)
        {
            int selectedDateId = (GridDates.SelectedValue != null) ? int.Parse(GridDates.SelectedValue.ToString()) : 0;
            ShowDate selectedDate = (ShowDate)Atx.CurrentShowRecord.ShowDateRecords().Find(selectedDateId);
            if (selectedDate != null && GridTickets.SelectedValue != null && (int)GridTickets.SelectedValue > 0)
            {
                ShowTicket selectedTicket = (ShowTicket)selectedDate.ShowTicketRecords().Find((int)GridTickets.SelectedValue);

                if (selectedTicket != null)
                {
                    try
                    {
                        selectedTicket.PostPurchaseTextRecords().AddPostPurchaseTextToCollection(selectedTicket, string.Empty, string.Empty);

                        //ensure latest data
                        int index = Atx.CurrentShowRecord.Id;
                        Atx.SetCurrentShowRecord(index);

                        GridPost.SelectedIndex = selectedTicket.PostPurchaseTextRecords().Count - 1;
                        GridPost.DataBind();                        
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

                        return;
                    }
                }
            }
        }
    
        protected void FormPost_ItemUpdated(object sender, FormViewUpdatedEventArgs e)
        {            
            FormView form = (FormView)sender;

            //reset data
            int index = (int)form.SelectedValue;

            Atx.SetCurrentShowRecord(Atx.CurrentShowRecord.Id);

            GridTickets.DataBind();
        }
        protected void SqlEntity_Updated(object sender, SqlDataSourceStatusEventArgs e)
        {
            GridDates.DataBind();
        }
        protected void FormPost_ItemCommand(object sender, FormViewCommandEventArgs e)
        {
            FormView form = (FormView)sender;
            string cmd = e.CommandName.ToLower();

            switch (cmd)
            {
                case "new":
                    CreateNewPostPurchase(form);
                    form.ChangeMode(FormViewMode.Edit);
                    break;
                case "copy":
                    //ensure valid input
                    DropDownList ddlCopy = (DropDownList)form.FindControl("ddlCopyText");
                    if (ddlCopy != null)//aslo ensures that we are in insert mode
                    {
                        int idx = int.Parse(ddlCopy.SelectedValue);

                        try
                        {
                            if (idx == 0)
                                throw new Exception("You must select a text to copy in order to copy.");

                            //get ticket chosen
                            PostPurchaseText chosen = PostPurchaseText.FetchByID(idx);

                            if (chosen == null)
                                throw new Exception("Sorry, that text could not be found.");

                            //copy ticket info and add to this date
                            ShowDate selDate = (ShowDate)Atx.CurrentShowRecord.ShowDateRecords().Find(GridDates.SelectedValue);
                            if (selDate != null)
                            {
                                //retrieve the current ticket selection
                                ShowTicket selTicket = (ShowTicket)selDate.ShowTicketRecords().Find(GridTickets.SelectedValue);
                                if(selTicket != null)
                                {   
                                    //ensure we do not already have this text in there
                                     int exists = selTicket.PostPurchaseTextRecords().GetList().FindIndex(delegate(PostPurchaseText match) { return (match.InProcessDescription == chosen.InProcessDescription && 
                                        match.PostText == chosen.PostText); } );
                                     //add text to ticket's collection of post purchase texts
                                     if (exists == -1)
                                     {
                                         PostPurchaseText newPP = selTicket.PostPurchaseTextRecords().AddPostPurchaseTextToCollection(selTicket, chosen.InProcessDescription, chosen.PostText);

                                         //refresh the collection and reset the grid selected index

                                         //refresh the form
                                         form.ChangeMode(FormViewMode.Edit);
                                         GridPost.SelectedIndex = newPP.DisplayOrder;
                                         GridPost.DataBind();
                                     }
                                }
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
        protected void FormPost_DataBound(object sender, EventArgs e)
        {
            FormView form = (FormView)sender;
            DataRowView drv = (DataRowView)form.DataItem;

            if (form.CurrentMode != FormViewMode.Edit)
                form.ChangeMode(FormViewMode.Edit);

            if (drv != null)
            {
                DataRow row = drv.Row;

                Button btnWys = (Button)form.FindControl("btnWys");
                if (btnWys != null)
                    btnWys.ToolTip = string.Format("/Admin/AdminControls/Wysiwyg/Wysiwyg.aspx?context=ppt&ctrl={0}&ppid={1}", this.UniqueID, GridPost.SelectedValue.ToString());

                Literal litDesc = (Literal)form.FindControl("litDesc");
                if (litDesc != null)
                    litDesc.Text = string.Format("<div class=\"desc-control\" >{0}</div>",
                        drv.Row.ItemArray.GetValue(drv.Row.Table.Columns.IndexOf("PostText")));
            }
        }

        #endregion
}
}