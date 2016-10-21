using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using Wcss;

namespace WillCallWeb.Admin.AdminControls
{
    public partial class Merch_PostPurchase : BaseControl, IPostBackEventHandler
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
                    int index = Atx.CurrentMerchRecord.Id;
                    Session.Remove("Admin_CurrentMerch");
                    Atx.SetCurrentMerchRecord(index);

                    GridPost.DataBind();
                    if (FormPost.CurrentMode != FormViewMode.Edit)
                        FormPost.ChangeMode(FormViewMode.Edit);
                    break;
            }
        }

        List<string> _errors = new List<string>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Atx.CurrentMerchRecord == null)
                base.Redirect("/Admin/MerchEditor.aspx");

            if (!IsPostBack)
            {
                GridPost.DataBind();
            }
        }

        #region GridPost

        protected void SqlPostPurchaseCollection_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters["@parentId"].Value = Atx.CurrentMerchRecord.Id;
        }

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
                    if (Atx.CurrentMerchRecord != null && Atx.CurrentMerchRecord.Id > 0)
                        postCount = Atx.CurrentMerchRecord.PostPurchaseTextRecords().Count;

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
                    PostPurchaseText moved = Atx.CurrentMerchRecord.PostPurchaseTextRecords().ReorderItem(int.Parse(e.CommandArgument.ToString()), cmd);
                    ////set the index of the moved item
                    grid.SelectedIndex = moved.DisplayOrder;
                    grid.DataBind();
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
                if (Atx.CurrentMerchRecord != null && Atx.CurrentMerchRecord.Id > 0)
                {
                    PostPurchaseText selectedPost = (PostPurchaseText)Atx.CurrentMerchRecord.PostPurchaseTextRecords().Find(GridPost.SelectedValue);
                    e.Cancel = ((selectedPost == null) || (!Atx.CurrentMerchRecord.PostPurchaseTextRecords().DeleteFromCollection(idx)));

                    //reset data
                    int index = Atx.CurrentMerchRecord.Id;
                    Atx.SetCurrentMerchRecord(index);

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

            grid.DataBind();
        }

        #endregion

        #region FormPost

        protected void CreateNewPostPurchase(FormView form)
        {
            if (Atx.CurrentMerchRecord != null)
            {
                try
                {
                    Atx.CurrentMerchRecord.PostPurchaseTextRecords().AddPostPurchaseTextToCollection(Atx.CurrentMerchRecord, string.Empty, string.Empty);

                    form.ChangeMode(FormViewMode.Edit);

                    //ensure latest data
                    int index = Atx.CurrentMerchRecord.Id;
                    Atx.SetCurrentMerchRecord(index);

                    GridPost.SelectedIndex = Atx.CurrentMerchRecord.PostPurchaseTextRecords().Count - 1;
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

        protected void FormPost_ItemUpdated(object sender, FormViewUpdatedEventArgs e)
        {            
            FormView form = (FormView)sender;

            //reset data
            int index = (int)form.SelectedValue;

            Atx.SetCurrentMerchRecord(Atx.CurrentMerchRecord.Id);

            GridPost.DataBind();
        }
        protected void SqlEntity_Updated(object sender, SqlDataSourceStatusEventArgs e)
        {
            GridPost.DataBind();
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

                            //copy merch info and add to this date
                            Merch merch = Atx.CurrentMerchRecord;

                            if (merch != null)
                            {
                                //ensure we do not already have this text in there
                                int exists = merch.PostPurchaseTextRecords().GetList().FindIndex(delegate(PostPurchaseText match)
                                {
                                    return (match.InProcessDescription == chosen.InProcessDescription &&
                                        match.PostText == chosen.PostText);
                                });
                                //add text to ticket's collection of post purchase texts
                                if (exists == -1)
                                {
                                    PostPurchaseText newPP = merch.PostPurchaseTextRecords().AddPostPurchaseTextToCollection(merch, chosen.InProcessDescription, chosen.PostText);

                                    //refresh the collection and reset the grid selected index

                                    //refresh the form
                                    form.ChangeMode(FormViewMode.Edit);
                                    GridPost.SelectedIndex = newPP.DisplayOrder;
                                    GridPost.DataBind();
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

            if(form.CurrentMode != FormViewMode.Edit)
                form.ChangeMode(FormViewMode.Edit);

            if (drv != null)
            {
                DataRow row = drv.Row;

                Button btnWys = (Button)form.FindControl("btnWys");
                if (btnWys != null)
                    btnWys.ToolTip = string.Format("/Admin/AdminControls/Wysiwyg/Wysiwyg.aspx?context=ppm&ctrl={0}&ppid={1}", 
                        this.UniqueID, GridPost.SelectedValue.ToString());

                Literal litDesc = (Literal)form.FindControl("litDesc");
                if (litDesc != null)
                    litDesc.Text = string.Format("<div class=\"desc-control\" >{0}</div>",
                        drv.Row.ItemArray.GetValue(drv.Row.Table.Columns.IndexOf("PostText")));
            }
        }

        #endregion
        
}
}