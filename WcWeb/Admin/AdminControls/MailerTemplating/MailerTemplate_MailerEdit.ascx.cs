using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using SubSonic;
using Wcss;

namespace WillCallWeb.Admin.AdminControls.MailerTemplating
{
    public partial class MailerTemplate_MailerEdit : BaseControl, IPostBackEventHandler
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
                    FormEntity1.DataBind();
                    break;
            }
        }

        protected MailerContent GetCurrentMailerContent
        {
            get
            {
                int tMailerTemplateContentId = (int)GridList.SelectedDataKey["Id"];
                MailerContent mc = Atx.CurrentMailer.MailerContentRecords().GetList()
                    .Find(delegate(MailerContent match) { return (match.TMailerTemplateContentId == tMailerTemplateContentId); });

                if (mc == null)
                {
                    Atx.RefreshMailer();

                    mc = Atx.CurrentMailer.MailerContentRecords().GetList()
                        .Find(delegate(MailerContent match) { return (match.TMailerTemplateContentId == tMailerTemplateContentId); });
                }

                return mc;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if(Atx.CurrentMailer == null)
                base.Redirect("/Admin/Mailers.aspx?p=select");
        }

        #region MailerTemplateContent GRID

        protected void GridList_DataBound(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;

            if (grid.Rows.Count > 0 && grid.SelectedIndex == -1)
            {
                grid.SelectedIndex = 0;
            }
        }
        //select
        protected void SqlMailerTemplateContentList_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters["@templateId"].Value = Atx.CurrentMailer.TMailerTemplateId;
            e.Command.Parameters["@mailerId"].Value = Atx.CurrentMailer.Id;
        }

        #endregion

        #region Entity FORM

        protected void FormEntity1_DataBound(object sender, EventArgs e)
        {
            FormView form = (FormView)sender;
            Literal litTagsUsed = (Literal)form.FindControl("litTagsUsed");

            //show the tags that are in use for this template 
            if (litTagsUsed != null)
            {
                MailerContent mc = GetCurrentMailerContent;
                litTagsUsed.Text = mc.MailerTemplateContentRecord.TagNameListing;
            }
        }
        protected void FormEntity1_ItemCommand(object sender, FormViewCommandEventArgs e)
        {
            FormView form = (FormView)sender;
            string cmd = e.CommandName.Trim().ToLower();

            if (cmd == "allon" || cmd == "alloff" || cmd == "refresh")// || cmd == "up" || cmd == "down")
            {
                MailerContent mc = GetCurrentMailerContent;

                switch (cmd)
                {
                    case "allon":
                    case "alloff":
                        foreach (ShowEvent s in mc.ShowEventRecords)
                            if((cmd == "allon" && (!s.IsActive)) || (cmd == "alloff" && (s.IsActive)))
                                s.IsActive = (cmd == "allon") ? true : false;

                        //mc.ShowEventRecords.SaveAll();
                        break;
                    case "refresh":
                        int maxListItems = (int)GridList.SelectedDataKey["iMaxListItems"];

                        //we need to know how to fill list based upon hit pick/upcoming or more upcoming
                        mc.PopulateShowList(maxListItems, true, true,
                            (mc.MailerTemplateContentRecord.TemplateAsset == MailerTemplateContent.ContentAsset.showlinear) ? 
                            _Config._MailerTemplate_ShowLinearEvent_DateFormat : _Config._MailerTemplate_ShowEvent_DateFormat);
                        break;
                }

                
                GridView grid = (GridView)form.FindControl("GridEvent");
                if(grid != null)
                    grid.DataBind();
            }
        }
        protected void FormEntity1_Updating(object sender, FormViewUpdateEventArgs e)
        {
            FormView form = (FormView)sender;
            string name = GridList.SelectedDataKey["Name"].ToString();
            int maxSelect = (int)GridList.SelectedDataKey["iMaxSelections"];

            MailerContent mc = GetCurrentMailerContent;

            if (mc.MailerTemplateContentRecord.TemplateAsset == MailerTemplateContent.ContentAsset.simple)
            {   
                e.NewValues["vcContent"] = mc.VcContent;
            }

            //grid items
            #region Grid ShowEvents
    

            if (mc.MailerTemplateContentRecord.TemplateAsset == MailerTemplateContent.ContentAsset.show || 
                mc.MailerTemplateContentRecord.TemplateAsset == MailerTemplateContent.ContentAsset.showlinear)
            {
                e.NewValues["vcContent"] = string.Empty;//e.OldValues["vcContent"];

                GridView events = (GridView)form.FindControl("gridEvent");
                if (events != null && events.Visible && events.Rows.Count > 0)
                {   
                    ShowEventCollection coll = new ShowEventCollection();
                    coll.AddRange(mc.ShowEventRecords);
                    coll.Sort("IOrdinal", true);

                    int selectCount = 0;

                    foreach (GridViewRow gvr in events.Rows)
                    {
                        int idx = gvr.RowIndex;
                        if (idx < coll.Count)
                        {
                            ShowEvent evt = coll[idx];

                            CheckBox chkActive = (CheckBox)gvr.FindControl("chkActive");
                            if (chkActive != null && evt.IsActive != chkActive.Checked)
                                evt.IsActive = chkActive.Checked;

                            if (chkActive.Checked)
                                selectCount++;

                            TextBox txtDate = (TextBox)gvr.FindControl("txtDate");
                            if (txtDate != null && evt.DateString != txtDate.Text.Trim())
                                evt.DateString = txtDate.Text.Trim();

                            TextBox txtStatus = (TextBox)gvr.FindControl("txtStatus");
                            if (txtStatus != null && evt.Status != txtStatus.Text.Trim())
                                evt.Status = txtStatus.Text.Trim();

                            TextBox txtShowTitle = (TextBox)gvr.FindControl("txtShowTitle");
                            if (txtShowTitle != null && evt.ShowTitle != txtShowTitle.Text.Trim())
                                evt.ShowTitle = txtShowTitle.Text.Trim();

                            TextBox txtPromoter = (TextBox)gvr.FindControl("txtPromoter");
                            if (txtPromoter != null && evt.Promoter != txtPromoter.Text.Trim())
                                evt.Promoter = txtPromoter.Text.Trim();

                            TextBox txtHeader = (TextBox)gvr.FindControl("txtHeader");
                            if (txtHeader != null && evt.Header != txtHeader.Text.Trim())
                                evt.Header = txtHeader.Text.Trim();

                            TextBox txtHeadliner = (TextBox)gvr.FindControl("txtHeadliner");
                            if (txtHeadliner != null && evt.Headliner != txtHeadliner.Text.Trim())
                                evt.Headliner = txtHeadliner.Text.Trim();

                            TextBox txtOpener = (TextBox)gvr.FindControl("txtOpener");
                            if (txtOpener != null && evt.Opener != txtOpener.Text.Trim())
                                evt.Opener = txtOpener.Text.Trim();

                            TextBox txtVenue = (TextBox)gvr.FindControl("txtVenue");
                            if (txtVenue != null && evt.Venue != txtVenue.Text.Trim())
                                evt.Venue = txtVenue.Text.Trim();

                            TextBox txtTimes = (TextBox)gvr.FindControl("txtTimes");
                            if (txtTimes != null && evt.Times != txtTimes.Text.Trim())
                                evt.Times = txtTimes.Text.Trim();

                            TextBox txtAges = (TextBox)gvr.FindControl("txtAges");
                            if (txtAges != null && evt.Ages != txtAges.Text.Trim())
                                evt.Ages = txtAges.Text.Trim();

                            TextBox txtPricing = (TextBox)gvr.FindControl("txtPricing");
                            if (txtPricing != null && evt.Pricing != txtPricing.Text.Trim())
                                evt.Pricing = txtPricing.Text.Trim();

                            TextBox txtUrl = (TextBox)gvr.FindControl("txtUrl");
                            if (txtUrl != null && evt.Url != txtUrl.Text.Trim())
                                evt.Url = txtUrl.Text.Trim();

                            TextBox txtImage = (TextBox)gvr.FindControl("txtImage");
                            if (txtImage != null && evt.ImageUrl != txtImage.Text.Trim())
                                evt.ImageUrl = txtImage.Text.Trim();
                        }
                    }

                    if (maxSelect > 0 && selectCount > maxSelect)
                    {
                        CustomValidation.IsValid = false;
                        CustomValidation.ErrorMessage = string.Format("Only {0} selection(s) are allowed.", maxSelect.ToString());
                        e.Cancel = true;
                    }
                    else
                        coll.SaveAll();
                }
            }
            #endregion

            
            string vcc = null;
            if (e.NewValues["vcContent"] != null)
            {
                vcc = e.NewValues["vcContent"].ToString();
                if (vcc != null)
                {
                    vcc = vcc.TrimEnd();

                    if (vcc.ToLower().EndsWith("<br/>"))
                        vcc = vcc.Remove(vcc.Length - 5);
                    else if (vcc.ToLower().EndsWith("<br />"))
                        vcc = vcc.Remove(vcc.Length - 6);
                }
            }
            e.NewValues["vcContent"] = vcc;

        }
        protected void FormEntity1_ItemUpdated(object sender, FormViewUpdatedEventArgs e)
        {
            if (ShowException(e.Exception))
                e.ExceptionHandled = true;
            else
            {
                Atx.RefreshMailer();
                GridList.DataBind();
            }
        }
        protected void FormEntity1_ModeChanging(object sender, FormViewModeEventArgs e)
        {
            FormView form = (FormView)sender;
            form.ChangeMode(e.NewMode);

            if (e.CancelingEdit)//handles cancel correctly
                form.DataBind();
        }        
        //select
        protected void SqlEntity1_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters["@tMailerId"].Value = Atx.CurrentMailer.Id;
            e.Command.Parameters["@title"].Value = (GridList.SelectedDataKey != null) ? GridList.SelectedDataKey["Title"].ToString() : string.Empty;
        }

        #endregion

        #region Event Grid - Show Listing

        /// <summary>
        /// For Templates that use a list and no flags - Show list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridEvent_DataBinding(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;

            int maxListItems = (int)GridList.SelectedDataKey["iMaxListItems"];
            int maxSelections = (int)GridList.SelectedDataKey["iMaxSelections"];
            string assets = GridList.SelectedDataKey["vcTemplateAsset"].ToString();

            if (assets.IndexOf(MailerTemplateContent.ContentAsset.show.ToString()) != -1)
            {
                MailerContent mc = GetCurrentMailerContent;
                
                if (mc.ShowEventRecords.Count == 0)
                {
                    mc.PopulateShowList(maxListItems, true, false,
                        (mc.MailerTemplateContentRecord.TemplateAsset == MailerTemplateContent.ContentAsset.showlinear) ?
                            _Config._MailerTemplate_ShowLinearEvent_DateFormat : _Config._MailerTemplate_ShowEvent_DateFormat);
                }

                mc.ShowEventRecords.Sort("IOrdinal", true);

                ShowEventCollection coll = new ShowEventCollection();
                coll.AddRange(mc.ShowEventRecords);

                ShowEvent blank = new ShowEvent();
                blank.Ordinal = 10000;
                
                coll.Add(blank);

                grid.DataSource = coll;
            }
            else
            {
                grid.DataSource = null;
                grid.Visible = false;
            }

            Panel pnlShowList = (Panel)FormEntity1.FindControl("pnlShowList");
            if(pnlShowList != null)
            {
                pnlShowList.Visible = grid.Visible;

                Button btnAllOn = (Button)pnlShowList.FindControl("btnAllOn");
                Button btnAllOff = (Button)pnlShowList.FindControl("btnAllOff");

                if (btnAllOn != null && btnAllOff != null)
                {
                    btnAllOn.Visible = maxSelections == 0;
                    btnAllOff.Visible = btnAllOn.Visible;
                }
            }
        }
        protected void GridEvent_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridView grid = (GridView)sender;

            //get the rows' header text
            //enable row only if it has a matching tag
            MailerContent mc = GetCurrentMailerContent;
            List<string> tagList = mc.MailerTemplateContentRecord.TagNameList;

            for(int i=0;i<e.Row.Cells.Count;i++)
            {
                string headText = grid.Columns[i].AccessibleHeaderText;//.HeaderText.ToUpper();
                if (headText != null && headText.TrimEnd().Length > 0)
                {
                    if (!tagList.Contains(headText.ToUpper()))
                    {
                        //todo - figure this out
                        //grid.Columns[i].HeaderText = string.Format("<span style=\"color:blue;text-decoration: line-through;\"></span>", headText);

                        foreach (Control c in e.Row.Cells[i].Controls)
                        {
                            if (c.GetType().Name == "TextBox")
                            {
                                ((TextBox)c).Enabled = false;
                                ((TextBox)c).Width = Unit.Pixel(30);
                            }
                        }   
                    }
                }
            }

            Grid_GenericBind(sender, e);
        }
        protected void GridEvent_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridView grid = (GridView)sender;
            string cmd = e.CommandName.Trim().ToLower();

            if (cmd == "up" || cmd == "down" || cmd == "addevent")
            {
                int idx = int.Parse(e.CommandArgument.ToString());

                MailerContent mc = GetCurrentMailerContent;

                ShowEventCollection coll = new ShowEventCollection();
                coll.AddRange(mc.ShowEventRecords);
                coll.Sort("IOrdinal", true);

                switch (cmd)
                {
                    case "up":
                    case "down":
                        coll.ReorderItem(idx, cmd);
                        coll.SaveAll();
                        break;

                    case "addevent":
                        //get values
                        GridViewRow gvr = grid.Rows[grid.Rows.Count-1];
                        TextBox txtDate = (TextBox)gvr.FindControl("txtDate");
                        TextBox txtStatus = (TextBox)gvr.FindControl("txtStatus");
                        TextBox txtShowTitle = (TextBox)gvr.FindControl("txtShowTitle");
                        TextBox txtPromoter = (TextBox)gvr.FindControl("txtPromoter");
                        TextBox txtHeader = (TextBox)gvr.FindControl("txtHeader");
                        TextBox txtHeadliner = (TextBox)gvr.FindControl("txtHeadliner");
                        TextBox txtOpener = (TextBox)gvr.FindControl("txtOpener");
                        TextBox txtVenue = (TextBox)gvr.FindControl("txtVenue");
                        TextBox txtTimes = (TextBox)gvr.FindControl("txtTimes");
                        TextBox txtAges = (TextBox)gvr.FindControl("txtAges");
                        TextBox txtPricing = (TextBox)gvr.FindControl("txtPricing");
                        TextBox txtUrl = (TextBox)gvr.FindControl("txtUrl");
                        TextBox txtImage = (TextBox)gvr.FindControl("txtImage");

                        if (txtDate != null && txtStatus != null && txtHeadliner != null && txtOpener != null && txtVenue != null && txtUrl != null &&
                             txtShowTitle != null && txtPromoter != null && txtHeader != null && txtTimes != null && txtAges != null && txtPricing != null && txtImage != null)
                        {
                            if (txtDate.Text.Trim().Length > 0 || txtStatus.Text.Trim().Length > 0 || txtHeadliner.Text.Trim().Length > 0 || txtOpener.Text.Trim().Length > 0 ||
                                txtVenue.Text.Trim().Length > 0 || txtUrl.Text.Trim().Length > 0 || txtShowTitle.Text.Trim().Length > 0 || txtPromoter.Text.Trim().Length > 0 ||
                                txtHeader.Text.Trim().Length > 0 || txtTimes.Text.Trim().Length > 0 || txtAges.Text.Trim().Length > 0 || txtPricing.Text.Trim().Length > 0 || txtImage.Text.Trim().Length > 0)
                            {
                                mc.ShowEventRecords.AddToCollection(mc.Id, ShowEvent.OwnerTypes.MailerContent, 0, ShowEvent.ParentTypes.NA, true, txtDate.Text.Trim(), txtStatus.Text.Trim(), 
                                    txtShowTitle.Text.Trim(), txtPromoter.Text.Trim(), txtHeader.Text.Trim(), txtHeadliner.Text.Trim(), txtOpener.Text.Trim(), txtVenue.Text.Trim(), 
                                    txtTimes.Text.Trim(), txtAges.Text.Trim(), txtPricing.Text.Trim(), txtUrl.Text.Trim(), txtImage.Text.Trim());

                                //coll.SaveAll();
                                mc.ShowEventRecords.SaveAll();
                            }
                        }

                        break;
                }

                
                //rebind
                grid.DataBind();
            }
        }

        #endregion

        protected void litDesc_DataBinding(object sender, EventArgs e)
        {
            Literal lit = (Literal)sender;
            MailerContent mc = GetCurrentMailerContent;

            //only show editor in editor mode and in simple mode - disable
            lit.Visible = (mc.MailerTemplateContentRecord.TemplateAsset == MailerTemplateContent.ContentAsset.editor);

            Button btnWys = (Button)FormEntity1.FindControl("btnWys");
            if (btnWys != null)
            {
                btnWys.Visible = lit.Visible;
                if(btnWys.Visible)
                    btnWys.ToolTip = string.Format("/Admin/AdminControls/Wysiwyg/Wysiwyg.aspx?context=med&ctrl={0}&medid={1}", 
                        this.UniqueID, mc.Id.ToString());
            }
            
            Label lblContent = (Label)FormEntity1.FindControl("lblContent");
            if(lblContent != null)
                lblContent.Visible = lit.Visible;

            if(lit.Visible)
                lit.Text = string.Format("<div class=\"desc-control\" style=\"max-width:1600px;width:900px !important;height:500px;overflow:auto;\">{0}</div>", mc.VcContent);
        }

        #region Simple Show Grid

        protected void GridShow_DataBinding(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;
            GridView source = GridList;
            MailerContent mc = GetCurrentMailerContent;
            string flags = mc.MailerTemplateContentRecord.VcFlags;

            if (flags != null && flags == SimpleShow.flagString)
            {
                List<SimpleShow> list = SimpleShow.ShowList(mc.VcContent);
                grid.Visible = true;
                grid.DataSource = list;
            }
            else grid.Visible = false;
        }
        protected void Grid_GenericBind(object sender, GridViewRowEventArgs e)
        {
            GridView grid = (GridView)sender;
            bool eventGrid = grid.ID.ToLower().IndexOf("gridevent") != -1;
            int edit = grid.EditIndex;
            //id and ordinal
            int itemId = (e.Row.DataItem == null) ? 0 : (eventGrid) ? ((ShowEvent)e.Row.DataItem).Id : ((SimpleShow)e.Row.DataItem).IDX;

            //when editing - turn of the other rows buttons
            LinkButton up = (LinkButton)e.Row.FindControl("btnUp");
            LinkButton down = (LinkButton)e.Row.FindControl("btnDown");
            Button btnNew = (Button)e.Row.FindControl("btnNew");
            Button btnAddEvent = (Button)e.Row.FindControl("btnAddEvent");
            LinkButton btnDelete = (LinkButton)e.Row.FindControl("btnDelete");
            LinkButton btnSelect = (LinkButton)e.Row.FindControl("btnSelect");
            CheckBox chkActive = (CheckBox)e.Row.FindControl("chkActive");

            if (btnNew != null)
                btnNew.Enabled = (edit == -1);
            if(btnDelete != null)
                btnDelete.Enabled = (edit == -1);
            if(btnSelect != null)
                btnSelect.Enabled = (edit == -1);

            if (up != null && down != null)
            {
                up.Visible = (!eventGrid) || (itemId > 0);
                if(up.Visible)
                    up.Enabled = (edit == -1) && (e.Row.RowIndex > 0);

                int rowCount = 0;
                if(eventGrid)
                    rowCount = ((ShowEventCollection)grid.DataSource).Count;
                else
                    rowCount = ((List<SimpleShow>)grid.DataSource).Count;

                down.Visible = (!eventGrid) || (itemId > 0);
                if(down.Visible)
                    down.Enabled = (edit == -1) && (e.Row.RowIndex < rowCount - 2);//allow for add row
            } 
            if(btnAddEvent != null)
                btnAddEvent.Visible = (itemId == 0);
            if(chkActive != null && itemId == 0)
                chkActive.Visible = false;
        }
        protected void GridShow_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridView grid = (GridView)sender;
            if (e.Row.RowState == DataControlRowState.Edit)
            {
                //get the rows' header text
                //enable row only if it has a matching tag
                MailerContent mc = GetCurrentMailerContent;
                List<string> tagList = mc.MailerTemplateContentRecord.TagNameList;

                TextBox txtDate = (TextBox)e.Row.FindControl("txtDate");
                TextBox txtStatus = (TextBox)e.Row.FindControl("txtStatus");
                TextBox txtHeadliner = (TextBox)e.Row.FindControl("txtHeadliner");
                TextBox txtOpener = (TextBox)e.Row.FindControl("txtOpener");
                TextBox txtVenue = (TextBox)e.Row.FindControl("txtVenue");

                if (txtDate != null && (!tagList.Contains("DATE")))
                {
                    txtDate.Enabled = false;
                    txtDate.Width = Unit.Pixel(30);
                }
                if (txtStatus != null && (!tagList.Contains("STATUS")))
                {
                    txtStatus.Enabled = false;
                    txtStatus.Width = Unit.Pixel(30);
                }
                if(txtHeadliner != null && (!tagList.Contains("HEADLINER")))
                {
                    txtHeadliner.Enabled = false;
                    txtHeadliner.Width = Unit.Pixel(30);
                }
                if(txtOpener != null && (!tagList.Contains("OPENER")))
                {
                    txtOpener.Enabled = false;
                    txtOpener.Width = Unit.Pixel(30);
                }
                if(txtVenue != null && (!tagList.Contains("VENUE")))
                {
                    txtVenue.Enabled = false;
                    txtVenue.Width = Unit.Pixel(30);
                }
            }

            Grid_GenericBind(sender, e);
        }
        protected void GridShow_DataBound(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;

            if (grid.DataSource != null)
            {
                int rowCount = ((List<SimpleShow>)grid.DataSource).Count;
                if (rowCount > 0 && grid.SelectedIndex == -1)
                    grid.SelectedIndex = 0;
            }
        }
        protected void GridShow_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridView grid = (GridView)sender;

            MailerContent mc = GetCurrentMailerContent;

            //get the values of the selected row
            string date         = SimpleShow.RemoveDelimiterAndSeparator(((TextBox)grid.Rows[grid.EditIndex].FindControl("txtDate")).Text);
            string status       = SimpleShow.RemoveDelimiterAndSeparator(((TextBox)grid.Rows[grid.EditIndex].FindControl("txtStatus")).Text);
            string headliner    = SimpleShow.RemoveDelimiterAndSeparator(((TextBox)grid.Rows[grid.EditIndex].FindControl("txtHeadliner")).Text);
            string opener       = SimpleShow.RemoveDelimiterAndSeparator(((TextBox)grid.Rows[grid.EditIndex].FindControl("txtOpener")).Text);
            string venue        = SimpleShow.RemoveDelimiterAndSeparator(((TextBox)grid.Rows[grid.EditIndex].FindControl("txtVenue")).Text);

            if (date.Length == 0 || headliner.Length == 0)
            {
                CustomValidation.IsValid = false;
                CustomValidation.ErrorMessage = "Date and Headliner are required.";
                return;
            }

            //update object
            //first get the edited row
            //if any changes - update....
            List<SimpleShow> shows = SimpleShow.ShowList(mc.VcContent);
            SimpleShow update = new SimpleShow(date, status, headliner, opener, venue);
            update.IDX = e.RowIndex;
            shows[e.RowIndex] = update;
            mc.VcContent = SimpleShow.ListToContent(shows);
            mc.Save();

            e.NewValues["vcContent"] = mc.VcContent;

            //update parents
            Atx.RefreshMailer();        

            //bind all relevant
            if (grid.EditIndex != -1)
                grid.SelectedIndex = grid.EditIndex;

            grid.EditIndex = -1;

            FormEntity1.DataBind();
        }
        protected void GridShow_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridView grid = (GridView)sender;

            MailerContent mc = GetCurrentMailerContent;

            List<SimpleShow> shows = SimpleShow.ShowList(mc.VcContent);
            shows.RemoveAt(e.RowIndex);

            //rewrite the current vcContent
            mc.VcContent = SimpleShow.ListToContent(shows);
            mc.Save();

            //rebind 
            grid.SelectedIndex = -1;
            //grid.DataBind();

            FormEntity1.DataBind();
        }
        protected void GridShow_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridView grid = (GridView)sender;
            string cmd = e.CommandName.ToLower();
            if (cmd == "new" || cmd == "up" || cmd == "down")
            {
                MailerContent mc = GetCurrentMailerContent;

                List<SimpleShow> shows = SimpleShow.ShowList(mc.VcContent);

                switch (cmd)
                {
                    case "new":
                        //add a new blank show to the end of the collection
                        SimpleShow simp = new SimpleShow(shows.Count);
                        shows.Add(simp);
                        mc.VcContent = SimpleShow.ListToContent(shows);
                        mc.Save();
                        grid.EditIndex = shows.Count - 1;
                        grid.DataBind();
                        break;

                    case "up":
                    case "down":
                        int idx = int.Parse(e.CommandArgument.ToString());
                        mc.VcContent = SimpleShow.ReorderItem(shows, idx, cmd);
                        mc.Save();
                        int newIdx = (cmd == "up") ? idx - 1 : idx + 1;
                        if (newIdx < 0)
                            newIdx = 0;
                        if (newIdx >= shows.Count)
                            newIdx = shows.Count - 1;

                        //rebind 
                        grid.SelectedIndex = newIdx;
                        grid.DataBind();
                        break;
                }

                //TODO: also bind content??
            }
        }        
        protected void GridShow_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView grid = (GridView)sender;

            grid.EditIndex = e.NewEditIndex;
            grid.DataBind();
        }
        protected void GridShow_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView grid = (GridView)sender;

            //if we are cancelling edit - and the edit item is empty - delete the item
            string date = ((TextBox)grid.Rows[grid.EditIndex].FindControl("txtDate")).Text.Trim();
            string status = ((TextBox)grid.Rows[grid.EditIndex].FindControl("txtStatus")).Text.Trim();
            string headliner = ((TextBox)grid.Rows[grid.EditIndex].FindControl("txtHeadliner")).Text.Trim();
            string opener = ((TextBox)grid.Rows[grid.EditIndex].FindControl("txtOpener")).Text.Trim();
            string venue = ((TextBox)grid.Rows[grid.EditIndex].FindControl("txtVenue")).Text.Trim();

            grid.EditIndex = -1;

            //only delete the entry if it is the last entry
            if ((e.RowIndex == grid.Rows.Count - 1) && date.Length == 0 && status.Length == 0 && headliner.Length == 0 && opener.Length == 0 && venue.Length == 0)
            {
                MailerContent mc = GetCurrentMailerContent;

                List<SimpleShow> shows = SimpleShow.ShowList(mc.VcContent);
                shows.RemoveAt(shows.Count - 1);
                
                //rewrite the current vcContent
                mc.VcContent = SimpleShow.ListToContent(shows);
                mc.Save();

            }
             
            grid.DataBind();
        }

        #endregion   
    }
}