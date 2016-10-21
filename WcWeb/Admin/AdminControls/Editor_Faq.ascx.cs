using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Collections.Generic;

using Wcss;

namespace WillCallWeb.Admin.AdminControls
{
    /// <summary>
    /// if we are in the editor mode - we will show the tune list. can only edit picture in editor mode
    /// </summary>
    public partial class Editor_Faq : BaseControl, IPostBackEventHandler
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
                    FormView formItem = (FormView)FormView1.FindControl("FormItem");
                    if (formItem != null)
                    {
                        Literal litDesc = (Literal)formItem.FindControl("litDesc");
                        if (litDesc != null)
                        {
                            FormView1.DataBind();
                        }
                    }
                    break;
            }
        }

        protected void Page_Load(object sender, EventArgs e) 
        {
            if (!IsPostBack)
            {   
                listCategories.DataBind();

                if (listCategories.Items.Count > 0 && listCategories.SelectedIndex == -1)
                    listCategories.SelectedIndex = 0;

                FormView1.DataBind();
            }
        }

        #region DataList Categories

        protected void listCategories_DataBinding(object sender, EventArgs e)
        {
            DataList list = (DataList)sender;
            list.DataSource = _Lookits.FaqCategories;
        }
        protected void listCategories_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            DataList list = (DataList)sender;

            Button cat = (Button)e.Item.FindControl("btnCategory");
            if (cat != null)
            {
                FaqCategorie gorie = (FaqCategorie)e.Item.DataItem;
                if(gorie != null)
                    cat.Text = gorie.Name.Trim();
            }

            LinkButton up = (LinkButton)e.Item.FindControl("btnUp");
            LinkButton down = (LinkButton)e.Item.FindControl("btnDown");

            if (up != null && down != null)
            {
                up.Enabled = (e.Item.ItemIndex > 0);

                int rowCount = _Lookits.FaqCategories.Count;

                down.Enabled = (e.Item.ItemIndex < rowCount - 1);
            }
        }
        protected void listCategories_ItemCommand(object sender, DataListCommandEventArgs e)
        {
            DataList list = (DataList)sender;

            string cmd = e.CommandName.ToLower();

            switch (cmd)
            {
                case "new":
                    list.SelectedIndex = -1;
                    FormView1.ChangeMode(FormViewMode.Insert);
                    break;
                case "up":
                case "down":
                    FaqCategorie moved = _Lookits.FaqCategories.ReorderItem(int.Parse(e.CommandArgument.ToString()), cmd);
                    _Lookits.RefreshLookup(_Enums.LookupTableNames.FaqCategories.ToString());
                    //set the index of the moved item
                    list.SelectedIndex = moved.DisplayOrder;
                    list.DataBind();

                    if (FormView1.CurrentMode != FormViewMode.Edit)
                        FormView1.ChangeMode(FormViewMode.Edit);

                    FormView1.DataBind();
                    break;
            }
        }
        protected void listCategories_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataList list = (DataList)sender;

            list.DataBind();
            if (FormView1.CurrentMode != FormViewMode.Edit)
                FormView1.ChangeMode(FormViewMode.Edit);
            FormView1.DataBind();
        }
        protected void btnToggle_Click(object sender, EventArgs e)
        {
            SiteConfig config = _Lookits.SiteConfigs.GetList().Find(delegate(SiteConfig match)
            {
                return (match.Context == _Enums.SiteConfigContext.Admin.ToString() &&
                    match.Name.ToLower() == "faq_page_on");
            });

            if (config != null)
            {
                config.ValueX = (!_Config._FAQ_Page_On).ToString();
                config.Save();
            }
        }
        #endregion

        #region FormView1 - Edit categorie

        protected void FormView1_DataBinding(object sender, EventArgs e)
        {
            FormView form = (FormView)sender;

            List<FaqCategorie> list = new List<FaqCategorie>();

            //ensure we have a context selection
            if (listCategories.SelectedIndex > -1)
            {
                FaqCategorie addCat = (FaqCategorie)_Lookits.FaqCategories.Find(listCategories.SelectedValue);
                if(addCat != null)
                    list.Add(addCat);
            }

            form.DataSource = list;
        }
        protected void FormView1_DataBound(object sender, EventArgs e)
        {
            FormView form = (FormView)sender;
            Button delete = (Button)form.FindControl("btnDelete");

            FaqCategorie entity = (FaqCategorie)_Lookits.FaqCategories.Find(form.SelectedValue);

            if (entity != null && delete != null)
                delete.OnClientClick = string.Format("return confirm('Are you sure you want to delete {0}?')", entity.Name.Replace("'", " "));

            //bind the child items
            GridView grid = (GridView)form.FindControl("GridItems");
            if (grid != null)
            {
                grid.DataBind();
            }
        }
        protected void FormView1_ItemInserting(object sender, FormViewInsertEventArgs e)
        {
            FormView form = (FormView)sender;
            TextBox category = (TextBox)form.FindControl("txtName");
            TextBox display = (TextBox)form.FindControl("txtDisplay");

            if (category != null && display != null)
            {
                string catName = category.Text.Trim();

                try
                {
                    if (catName.Length == 0)
                        throw new Exception("Category Name is required.");

                    //ensure that the new catName is unique
                    FaqCategorie exists = _Lookits.FaqCategories.GetList().Find(delegate(FaqCategorie match) { return (match.Name == catName); });
                    if (exists != null)
                        throw new Exception("This category name already exists. The category name must be unique.");

                    _Lookits.FaqCategories.AddToCollection(catName, display.Text.Trim());

                    _Lookits.RefreshLookup(_Enums.LookupTableNames.FaqCategories.ToString());

                    listCategories.SelectedIndex = _Lookits.FaqCategories.Count - 1;
                    listCategories.DataBind();

                    form.ChangeMode(FormViewMode.Edit);
                    form.DataBind();
                }
                catch (System.Threading.ThreadAbortException) { }
                catch (Exception ex)
                {
                    CustomValidator custom = (CustomValidator)form.FindControl("cusCategory");

                    if (custom != null)
                    {
                        custom.IsValid = false;
                        custom.ErrorMessage = ex.Message;
                    }
                }
            }
        }
        protected void FormView1_ItemUpdating(object sender, FormViewUpdateEventArgs e)
        {
            FormView form = (FormView)sender;
            TextBox category = (TextBox)form.FindControl("txtName");
            TextBox display = (TextBox)form.FindControl("txtDisplay");
            TextBox desc = (TextBox)form.FindControl("txtDescription");
            CheckBox active = (CheckBox)form.FindControl("chkActive");

            FaqCategorie entity = (FaqCategorie)_Lookits.FaqCategories.Find(form.SelectedValue);

            if (entity != null && category != null && display != null && desc != null && active != null)
            {
                bool updateRequired = false;

                string catName = category.Text.Trim();

                try
                {
                    if (catName.Length == 0)
                        throw new Exception("Category Name is required.");

                    if (entity.Name != catName)
                    {
                        //ensure that the new catName is unique
                        FaqCategorie exists = _Lookits.FaqCategories.GetList().Find(delegate(FaqCategorie match) { return (match.Name == catName); });
                        if (exists != null)
                            throw new Exception("This category name already exists. The category name must be unique.");

                        entity.Name = catName;
                        updateRequired = true;
                    }

                    if (entity.DisplayText != display.Text.Trim())
                    {
                        entity.DisplayText = display.Text.Trim();
                        updateRequired = true;
                    }

                    if (entity.Description != desc.Text.Trim())
                    {
                        entity.Description = desc.Text.Trim();
                        updateRequired = true;
                    }

                    if (entity.IsActive != active.Checked)
                    {
                        entity.IsActive = active.Checked;
                        updateRequired = true;
                    }

                    if (updateRequired)
                    {
                        entity.Save();

                        _Lookits.RefreshLookup(_Enums.LookupTableNames.FaqCategories.ToString());

                        listCategories.DataBind();
                        form.DataBind();
                    }
                }
                catch (System.Threading.ThreadAbortException) { }
                catch (Exception ex)
                {
                    CustomValidator custom = (CustomValidator)form.FindControl("cusCategory");

                    if (custom != null)
                    {
                        custom.IsValid = false;
                        custom.ErrorMessage = ex.Message;
                    }
                }
            }
        }
        protected void FormView1_ItemDeleting(object sender, FormViewDeleteEventArgs e)
        {
            FormView form = (FormView)sender;

            _Lookits.FaqCategories.DeleteFromCollection((int)form.SelectedValue);

            _Lookits.RefreshLookup(_Enums.LookupTableNames.FaqCategories.ToString());

            int idx = listCategories.SelectedIndex - 1;

            if (idx < 0 && _Lookits.FaqCategories.Count > 0)
                idx = 0;

            listCategories.SelectedIndex = idx;
            listCategories.DataBind();
            form.DataBind();
        }
        protected void FormView1_ModeChanging(object sender, FormViewModeEventArgs e)
        {
            FormView form = (FormView)sender;

            form.ChangeMode(e.NewMode);

            if (e.CancelingEdit)
                form.DataBind();
        }

        #endregion

        #region GridItems

        protected void GridItems_DataBinding(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;

            FaqItemCollection coll = new FaqItemCollection();
            //get categories collection
            FaqCategorie parent = (FaqCategorie)_Lookits.FaqCategories.Find(listCategories.SelectedValue);

            if (parent != null)
            {
                coll.AddRange(parent.FaqItemRecords());
                if (coll.Count > 1)
                    coll.Sort("IDisplayOrder", true);
            }

            grid.DataSource = coll;
        }        
        protected void GridItems_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridView grid = (GridView)sender;

            FaqItem entity = (FaqItem)e.Row.DataItem;

            if (entity != null)
            {
                Literal question = (Literal)e.Row.FindControl("litQuestion");
                Literal answer = (Literal)e.Row.FindControl("litAnswer");

                if (question != null && answer != null)
                {
                    question.Text = string.Format("Q: {0}", entity.Question.Trim());
                    answer.Text = string.Format("A: {0}", entity.Answer.Trim());
                }

                LinkButton up = (LinkButton)e.Row.FindControl("btnUp");
                LinkButton down = (LinkButton)e.Row.FindControl("btnDown");
                LinkButton delete = (LinkButton)e.Row.FindControl("btnDelete");

                if(delete != null)
                    delete.OnClientClick = string.Format("return confirm('Are you sure you want to delete {0}?')", entity.Question.Replace("'", " "));

                if (up != null && down != null)
                {
                    up.Enabled = (e.Row.RowIndex > 0);

                    FaqCategorie parent = (FaqCategorie)_Lookits.FaqCategories.Find(listCategories.SelectedValue);

                    int rowCount = (parent != null) ? parent.FaqItemRecords().Count - 1 : -1;

                    down.Enabled = (e.Row.RowIndex < rowCount);
                }
            }
        }
        protected void GridItems_DataBound(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;

            if (grid.SelectedIndex == -1 && grid.Rows.Count > 0)
                grid.SelectedIndex = 0;

            FormView form = (FormView)FormView1.FindControl("FormItem");
            if (form != null)
                form.DataBind();
        }
        protected void GridItems_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridView grid = (GridView)sender;

            int idx = (int)grid.DataKeys[e.RowIndex].Value;

            try
            {
                FaqCategorie parent = (FaqCategorie)_Lookits.FaqCategories.Find(listCategories.SelectedValue);

                if (parent != null)
                {
                    parent.FaqItemRecords().DeleteFromCollection(idx);

                    _Lookits.RefreshLookup(_Enums.LookupTableNames.FaqCategories.ToString());

                    int newSelection = e.RowIndex - 1;
                    if (newSelection == -1 && _Lookits.FaqCategories.Count > 0)
                        newSelection = 0;

                    listCategories.DataBind();

                    FormView1.DataBind();
                }
            }
            catch (Exception ex)
            {
                _Error.LogException(ex);

                CustomValidator CustomDelete = (CustomValidator)grid.Rows[e.RowIndex].FindControl("CustomDelete");
                if (CustomDelete != null)
                {
                    CustomDelete.IsValid = false;
                    CustomDelete.ErrorMessage = ex.Message;
                }
            }
        }
        protected void GridItems_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridView grid = (GridView)sender;
            string cmd = e.CommandName.ToLower();

            switch (cmd)
            {
                case "up":
                case "down":
                    FaqCategorie parent = (FaqCategorie)_Lookits.FaqCategories.Find(listCategories.SelectedValue);
                    if (parent != null)
                    {
                        FaqItem moved = parent.FaqItemRecords().ReorderItem(int.Parse(e.CommandArgument.ToString()), cmd);
                        _Lookits.RefreshLookup(_Enums.LookupTableNames.FaqCategories.ToString());
                        //set the index of the moved item
                        grid.SelectedIndex = moved.DisplayOrder;
                        grid.DataBind();

                        FormView item = (FormView)FormView1.FindControl("FormItem");
                        if (item.CurrentMode != FormViewMode.Edit)
                            item.ChangeMode(FormViewMode.Edit);

                        grid.DataBind();
                    }
                    break;
            }
        }
        protected void GridItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;

            FormView item = (FormView)FormView1.FindControl("FormItem");
            if (item != null)
                item.DataBind();
        }

        #endregion

        #region FormItem

        protected void FormItem_DataBinding(object sender, EventArgs e)
        {
            FormView form = (FormView)sender;

            FaqItemCollection coll = new FaqItemCollection();

            //get the selected faq
            GridView grid = (GridView)FormView1.FindControl("GridItems");
            if (grid.SelectedIndex > -1)
            {
                FaqCategorie parent = (FaqCategorie)_Lookits.FaqCategories.Find(listCategories.SelectedValue);
                if (parent != null)
                {
                    FaqItem addFaq = (FaqItem)parent.FaqItemRecords().Find(grid.SelectedValue);
                    if(addFaq != null)
                        coll.Add(addFaq);
                }
            }

            form.DataSource = coll;
        }
        protected void FormItem_DataBound(object sender, EventArgs e)
        {
            FormView form = (FormView)sender;
            Button activate = (Button)form.FindControl("btnActivate");
            Literal litDesc = (Literal)form.FindControl("litDesc");
            Button btnWys = (Button)form.FindControl("btnWys");
            FaqItem entity = (FaqItem)form.DataItem;

            if (entity != null && activate != null)
            {
                activate.CommandName = (entity.IsActive) ? "deactivate" : "activate";
                activate.Text = (entity.IsActive) ? "Deactivate" : "Activate";
            }

            if (litDesc != null)
                litDesc.Text = string.Format("<div class=\"desc-control rounded\">{0}</div>", entity.Answer);

            if (entity != null && btnWys != null)
                btnWys.ToolTip = string.Format("/Admin/AdminControls/Wysiwyg/Wysiwyg.aspx?context=faq&ctrl={0}&faqId={1}", this.UniqueID, entity.Id.ToString());
        }
        protected void FormItem_ItemCommand(object sender, FormViewCommandEventArgs e)
        {
            FormView form = (FormView)sender;
            string cmd = e.CommandName.ToLower();

            switch (cmd)
            {
                case "activate":
                case "deactivate":
                    GridView grid = (GridView)FormView1.FindControl("GridItems");
                    if (grid.SelectedIndex > -1)
                    {
                        FaqCategorie parent = (FaqCategorie)_Lookits.FaqCategories.Find(listCategories.SelectedValue);
                        if (parent != null)
                        {
                            FaqItem selFaq = (FaqItem)parent.FaqItemRecords().Find(grid.SelectedValue);
                            if (selFaq != null)
                            {
                                selFaq.IsActive = (cmd == "activate");
                                selFaq.Save();
                                _Lookits.RefreshLookup(_Enums.LookupTableNames.FaqCategories.ToString());
                                grid.DataBind();

                                //FormView1.DataBind();
                                //form.DataBind();
                            }
                        }
                    }
                    break;
            }
        }
        protected void FormItem_ItemInserting(object sender, FormViewInsertEventArgs e)
        {
            FormView form = (FormView)sender;

            TextBox question = (TextBox)form.FindControl("txtQuestion");
            TextBox answer = (TextBox)form.FindControl("txtAnswer");

            if (question != null && answer != null)
            {
                FaqCategorie parent = (FaqCategorie)_Lookits.FaqCategories.Find(listCategories.SelectedValue);

                if (parent != null)
                {
                    string q = question.Text.Trim();
                    string a = answer.Text.Trim();

                    try
                    {
                        if (q.Length == 0)
                            throw new Exception("Question is required");

                        FaqItem itm = parent.FaqItemRecords().AddToCollection(parent.Id, q, a);

                        _Lookits.RefreshLookup(_Enums.LookupTableNames.FaqCategories.ToString());

                        listCategories.DataBind();

                        form.ChangeMode(FormViewMode.Edit);

                        GridView grid = (GridView)FormView1.FindControl("GridItems");
                        if (grid != null)
                        {
                            grid.SelectedIndex = parent.FaqItemRecords().Count - 1;
                            grid.DataBind();
                        }
                    }
                    catch (System.Threading.ThreadAbortException) { }
                    catch (Exception ex)
                    {
                        RequiredFieldValidator custom = (RequiredFieldValidator)form.FindControl("Required1");

                        if (custom != null)
                        {
                            custom.IsValid = false;
                            custom.ErrorMessage = ex.Message;
                        }
                    }
                }
            }
        }
        protected void FormItem_ItemUpdating(object sender, FormViewUpdateEventArgs e)
        {
            FormView form = (FormView)sender;

            TextBox question = (TextBox)form.FindControl("txtQuestion");

            FaqCategorie parent = (FaqCategorie)_Lookits.FaqCategories.Find(listCategories.SelectedValue);

            if(parent != null)
            {
                FaqItem entity = (FaqItem)parent.FaqItemRecords().Find((int)form.SelectedValue);

                if (entity != null && question != null)
                {
                    string q = question.Text.Trim();

                    try
                    {
                        if (q.Length == 0)
                            throw new Exception("Question is required");

                        entity.Question = q.Trim();

                        entity.Save();

                        _Lookits.RefreshLookup(_Enums.LookupTableNames.FaqCategories.ToString());

                        GridView grid = (GridView)FormView1.FindControl("GridItems");
                        if (grid != null)
                        {
                            int selIdx = grid.SelectedIndex;
                            grid.DataBind();
                        }
                    }
                    catch (System.Threading.ThreadAbortException) { }
                    catch (Exception ex)
                    {
                        RequiredFieldValidator custom = (RequiredFieldValidator)form.FindControl("Required1");

                        if (custom != null)
                        {
                            custom.IsValid = false;
                            custom.ErrorMessage = ex.Message;
                        }
                    }
                }
            }
        }
        protected void FormItem_ModeChanging(object sender, FormViewModeEventArgs e)
        {
            FormView form = (FormView)sender;

            form.ChangeMode(e.NewMode);

            if (e.CancelingEdit)
                form.DataBind();
        }
        protected void ddlCategories_DataBinding(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;

            ddl.DataSource = _Lookits.FaqCategories;
            ddl.DataTextField = "Name";
            ddl.DataValueField = "Id";
        }
        protected void ddlCategories_DataBound(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;

            ddl.SelectedIndex = -1;

            ListItem li = ddl.Items.FindByValue(listCategories.SelectedValue.ToString());//value is a string!
            if (li != null)
                li.Selected = true;
        }
        protected void ddlCategories_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;

            //blow off input

            //get the current item and update the item
            FaqCategorie oldParent = (FaqCategorie)_Lookits.FaqCategories.Find(listCategories.SelectedValue);

            if (oldParent != null)
            {
                FormView form = (FormView)FormView1.FindControl("FormItem");
                FaqItem entity = (FaqItem)oldParent.FaqItemRecords().Find(form.SelectedValue);

                if (entity != null)
                {
                    //remove the item from the current collection and add it to the new collection
                    int newCategorieId = int.Parse(ddl.SelectedValue);

                    FaqCategorie newParent = (FaqCategorie)_Lookits.FaqCategories.Find(newCategorieId);

                    if (newParent != null)
                    {
                        newParent.FaqItemRecords().AddToCollection(newParent.Id, entity.Question, entity.Answer);
                        oldParent.FaqItemRecords().DeleteFromCollection(entity.Id);

                        _Lookits.RefreshLookup(_Enums.LookupTableNames.FaqCategories.ToString());

                        //set newindex - we can do this because the lookup collection is ordered by display order
                        int idx = _Lookits.FaqCategories.GetList().FindIndex(delegate(FaqCategorie match) { return (match.Id == newCategorieId); });

                        listCategories.SelectedIndex = idx;
                        listCategories.DataBind();

                        FormView1.DataBind();

                        GridView grid = (GridView)FormView1.FindControl("GridItems");
                        if (grid != null)
                        {
                            grid.SelectedIndex = grid.Rows.Count - 1;//select last row
                            FormView farm = (FormView)FormView1.FindControl("FormItem");
                            if (farm != null)
                                farm.DataBind();
                        }
                    }
                }
            }
        }

        #endregion
}
}
