using System;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using Wcss;

namespace WillCallWeb.Admin.AdminControls
{
    public partial class ShowLinks : BaseControl
    {
        #region Collections and Page Objects

        protected ShowLinkCollection OrderedCollection
        {
            get
            {
                ShowLinkCollection coll = new ShowLinkCollection();
                coll.AddRange(Atx.CurrentShowRecord.ShowLinkRecords());
                if (coll.Count > 1)
                    coll.Sort("IDisplayOrder", true);
                return coll;
            }
        }

        #endregion

        protected void btnSales_Click(object sender, EventArgs e)
        {
            base.Redirect(string.Format("/Admin/Listings.aspx?p=tickets&showid={0}",
                (Atx.CurrentShowRecord != null) ? Atx.CurrentShowRecord.Id.ToString() : "0"));
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Atx.CurrentShowRecord == null)
                base.Redirect("/Admin/ShowEditor.aspx");

            litShowTitle.Text = Atx.CurrentShowRecord.Name;

            if (!IsPostBack)
            {
                GridView1.DataBind();
            }
        }

        #region GridView

        protected void GridView1_DataBinding(object sender, EventArgs e)
        {
            GridView1.DataSource = OrderedCollection;
            string[] keyNames = { "Id" };
            GridView1.DataKeyNames = keyNames;
        }
        
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridView grid = (GridView)sender;
            DataControlRowType typ = e.Row.RowType;

            if (typ == DataControlRowType.DataRow)
            {
                ShowLink ent = (ShowLink)e.Row.DataItem;

                Literal display = (Literal)e.Row.FindControl("litText");
                Literal link = (Literal)e.Row.FindControl("litLink");

                if (display != null && ent != null && link != null)
                {
                    display.Text = ent.LinkUrl_Formatted(true);
                    link.Text = ent.LinkUrl_BaseLink;
                }

                LinkButton delete = (LinkButton)e.Row.FindControl("btnDelete");
                LinkButton up = (LinkButton)e.Row.FindControl("btnUp");
                LinkButton down = (LinkButton)e.Row.FindControl("btnDown");

                if (delete != null && ent != null)
                    delete.OnClientClick = string.Format("return confirm('Are you sure you want to delete {0}?')",
                        Utils.ParseHelper.ParseJsAlert(ent.DisplayText));

                if (up != null && down != null)
                {
                    up.Enabled = (e.Row.RowIndex > 0);
                    down.Enabled = (e.Row.RowIndex < OrderedCollection.Count - 1);
                }
            }
        }

        protected void GridView1_DataBound(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;

            if (grid.DataSource != null && OrderedCollection.Count > 0 && grid.SelectedIndex == -1)
                grid.SelectedIndex = 0;

            FormView1.DataBind();
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridView grid = (GridView)sender;
            string cmd = e.CommandName.ToLower();

            if (FormView1.CurrentMode != FormViewMode.Edit)
                FormView1.ChangeMode(FormViewMode.Edit);

            switch (cmd)
            {
                case "up":
                case "down":
                    ShowLink moved = Atx.CurrentShowRecord.ShowLinkRecords().ReorderItem(int.Parse(e.CommandArgument.ToString()), cmd);
                    //set the index of the moved item
                    grid.SelectedIndex = moved.DisplayOrder;
                    grid.DataBind();
                    break;
            }
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //todo select idx from data key
            GridView grid = (GridView)sender;
            FormView1.DataBind();
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridView grid = (GridView)sender;
            int idx = (int)grid.DataKeys[e.RowIndex].Value;

            e.Cancel = (!Atx.CurrentShowRecord.ShowLinkRecords().DeleteFromCollection(idx));

            //reset show data
            int index = Atx.CurrentShowRecord.Id;
            Atx.SetCurrentShowRecord(index);

            if (!e.Cancel)
            {
                grid.SelectedIndex = e.RowIndex - 1;
                grid.DataBind();
            }
        }

        #endregion

        #region Show Listing

        protected void ddlShowLinks_DataBinding(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;

            System.Collections.Generic.List<ListItem> list = new System.Collections.Generic.List<ListItem>();

            ShowCollection coll = new ShowCollection();
            foreach (ShowDate sd in Atx.SaleShowDates_All)
            {
                if (Atx.CurrentShowRecord.Id != sd.TShowId && (!coll.GetList().Exists(delegate(Show match) { return (match.Id == sd.TShowId); })))
                    coll.Add(sd.ShowRecord);
            }
            foreach (Show s in coll)
                list.Add(new ListItem(s.Name_WithLocation, s.Id.ToString()));

            if (list.Count > 1)
                list.Sort(new Utils.Reflector.CompareEntities<ListItem>(Utils.Reflector.Direction.Ascending, "Text"));

            list.Insert(0, new ListItem("<-- Select A Show -->", "0"));

            ddl.DataSource = list;
            ddl.DataTextField = "Text";
            ddl.DataValueField = "Value";
        }
        protected void ddlShowLinks_DataBound(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;

            if (FormView1.SelectedValue != null)
            {
                int idx = int.Parse(FormView1.SelectedValue.ToString());
                if (idx > 0)
                {
                    ShowLink entity = (ShowLink)Atx.CurrentShowRecord.ShowLinkRecords().Find(idx);

                    if (entity != null && entity.IsShowLink)
                    {
                        ddl.SelectedIndex = -1;

                        ListItem li = ddl.Items.FindByValue(entity.LinkUrl.ToString());
                        if (li != null)
                            li.Selected = true;
                        else
                        {
                            Show s = Show.FetchByID(entity.LinkUrl.ToString());
                            if (s != null && s.ApplicationId == _Config.APPLICATION_ID)
                            {
                                ListItem oldShow = new ListItem(s.Name_WithLocation, s.Id.ToString());
                                oldShow.Selected = true;
                                ddl.Items.Add(oldShow);
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region Details

        protected void FormView1_DataBinding(object sender, EventArgs e)
        {
            int idx = (GridView1.SelectedDataKey != null) ? (int)GridView1.SelectedDataKey["Id"] : 0;

            ShowLinkCollection selected = new ShowLinkCollection();
            ShowLink addLink = (ShowLink)Atx.CurrentShowRecord.ShowLinkRecords().Find(idx);
            if(addLink != null)
                selected.Add(addLink);

            FormView1.DataSource = selected;
            string[] keyNames = { "Id" };
            FormView1.DataKeyNames = keyNames;
        }
        protected void FormView1_DataBound(object sender, EventArgs e)
        {
            FormView form = (FormView)sender;
            if (form.SelectedValue != null)
            {
                int idx = int.Parse(form.SelectedValue.ToString());
                if (idx > 0)
                {
                    ShowLink entity = (ShowLink)Atx.CurrentShowRecord.ShowLinkRecords().Find(idx);

                    if (entity != null)
                    {
                        TextBox display = (TextBox)form.FindControl("txtDisplayText");
                        if (display != null)
                            display.Enabled = entity.IsRemoteLink;

                        TextBox link = (TextBox)form.FindControl("txtLinkUrl");
                        if (link != null)
                            link.Enabled = entity.IsRemoteLink;

                        DropDownList ddl = (DropDownList)form.FindControl("ddlShowLinks");
                        if (ddl != null)
                            ddl.Enabled = false;// entity.IsShowLink;
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
        protected void FormView1_ItemCommand(object sender, FormViewCommandEventArgs e)
        {
            string cmd = e.CommandName.ToLower();

            switch (cmd)
            {
                case "cancel":
                    FormView1.ChangeMode(FormViewMode.Edit);
                    break;
                case "new":
                    Atx.CurrentShowLinkId = 0;
                    break;
            }
        }
        protected void FormView1_ItemInserting(object sender, FormViewInsertEventArgs e)
        {
            FormView form = (FormView)sender;

            TextBox linkUrl = (TextBox)form.FindControl("txtLinkUrl");
            TextBox displayText = (TextBox)form.FindControl("txtDisplayText");
            DropDownList ddlShow = (DropDownList)form.FindControl("ddlShowLinks");
            
            if (linkUrl != null && displayText != null && ddlShow != null)
            {
                try
                {
                    string link = linkUrl.Text.Trim();
                    string display = displayText.Text.Trim();
                    int showId = int.Parse(ddlShow.SelectedValue);

                    //display Text is required for remote links
                    if (showId <= 0 && display.Length == 0)
                        throw new Exception("Display text is required.");
                    else
                    {
                        ShowDateCollection coll = new ShowDateCollection();
                        coll.AddRange(Atx.SaleShowDates_All.GetList().FindAll(delegate(ShowDate match) { return (match.TShowId == showId); }));
                        if (coll.Count > 0)
                        {
                            Show linker = coll[0].ShowRecord;
                            display = linker.Name_WithLocation;
                        }
                    }

                    //ensure the link context is singular
                    if (link.Length > 0 && showId > 0)
                        throw new Exception("Please select either a link or a show. You cannot select both.");

                    //at least one must be chosen
                    if(link.Length == 0 && showId == 0)
                        throw new Exception("Please enter a link or select a show.");

                    string inputLink = string.Empty;

                    //validate input
                    if (link.Length > 0)
                    {
                        if (!Utils.Validation.IsValidUrl(link))
                            throw new Exception("Please enter a valid url for the link.");

                        inputLink = link.Trim();
                    }
                    else if (showId > 0)
                        inputLink = showId.ToString();
                    
                    if(inputLink.Trim().Length == 0)
                        throw new Exception("Input could not be configured.");

                    ShowLink added = Atx.CurrentShowRecord.ShowLinkRecords().AddToCollection(Atx.CurrentShowRecord.Id, inputLink, display);

                    FormView1.ChangeMode(FormViewMode.Edit);

                    GridView1.SelectedIndex = added.DisplayOrder;//set to new item
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
                }
            }
        }
        protected void FormView1_ItemUpdating(object sender, FormViewUpdateEventArgs e)
        {
            FormView form = (FormView)sender;

            TextBox linkUrl = (TextBox)form.FindControl("txtLinkUrl");
            TextBox displayText = (TextBox)form.FindControl("txtDisplayText");
            DropDownList ddlShow = (DropDownList)form.FindControl("ddlShowLinks");
            CheckBox chkActive = (CheckBox)form.FindControl("chkActive");

            int idx = int.Parse(form.SelectedValue.ToString());

            if (idx > 0)
            {
                ShowLink entity = (ShowLink)Atx.CurrentShowRecord.ShowLinkRecords().Find(idx);

                if (entity != null && linkUrl != null && displayText != null && ddlShow != null && chkActive != null)
                {
                    List<string> errors = new List<string>();

                    try
                    {
                        string link = linkUrl.Text.Trim();
                        string display = displayText.Text.Trim();
                        int showId = int.Parse(ddlShow.SelectedValue);

                        //display Text is required
                        if (showId <= 0 && display.Length == 0)
                            errors.Add("Display text is required.");

                        //ensure the link context is singular
                        if (link.Length > 0 && (!Utils.Validation.IsInteger(link)) && showId > 0)
                            errors.Add("Please select either a link or a show. You cannot select both.");

                        //at least one must be chosen
                        if (link.Length == 0 && showId == 0)
                            errors.Add("Please enter a link or select a show.");

                        if (errors.Count > 0)
                            throw new Exception();

                        string inputLink = string.Empty;

                        //validate input
                        if (link.Length > 0 && (!Utils.Validation.IsInteger(link)))
                        {
                            if (!Utils.Validation.IsValidUrl(link))
                                throw new Exception("Please enter a valid url for the link.");

                            inputLink = link.Trim();
                        }
                        else if (showId > 0)
                            inputLink = showId.ToString();

                        if (inputLink.Trim().Length == 0)
                            throw new Exception("Input could not be configured.");

                        ShowLink existing = (ShowLink)OrderedCollection.GetList().Find(
                                delegate(ShowLink match) { return (match.Id != entity.Id && match.LinkUrl == inputLink); });
                        if (existing != null)
                            throw new Exception("This link already exists in the collection.");

                        entity.IsActive = chkActive.Checked;
                        entity.DisplayText = display;
                        entity.LinkUrl = inputLink;

                        entity.Save();

                        //reset show data
                        int index = Atx.CurrentShowRecord.Id;
                        Atx.SetCurrentShowRecord(index);

                        GridView1.DataBind();
                    }
                    catch (Exception ex)
                    {
                        string msg = string.Empty;

                        if (errors.Count > 0)
                        {
                            _Error.LogException(new Exception(Utils.ParseHelper.StringListToLine(errors)));
                            _Error.LogException(ex);
                        }
                        else
                        {
                            _Error.LogException(ex);
                            msg = ex.Message;
                        }

                        CustomValidator validation = (CustomValidator)form.FindControl("CustomValidation");
                        if (validation != null)
                        {
                            validation.IsValid = false;
                            validation.ErrorMessage = ex.Message;
                        }
                    }
                }
            }
        }

        #endregion

    }
}