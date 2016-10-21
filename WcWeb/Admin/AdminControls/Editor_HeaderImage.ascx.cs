using System;
using System.IO;
using System.Data;
using System.Web.UI;
using System.Collections;
using System.Web.UI.WebControls;

using Wcss;

namespace WillCallWeb.Admin.AdminControls
{
    public partial class Editor_HeaderImage : BaseControl
    {
        #region New paging

        bool isSelectCount;
        protected void objData_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            isSelectCount = e.ExecutingSelectCount;

            e.InputParameters["activeStatus"] = "active";

            if (!isSelectCount)
            {
                e.Arguments.StartRowIndex = GooglePager1.StartRowIndex;
                e.Arguments.MaximumRows = GooglePager1.PageSize;
            }
        }
        protected void objData_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (isSelectCount && e.ReturnValue != null && e.ReturnValue.GetType().Name == "Int32")
            {
                GooglePager1.DataSetSize = (int)e.ReturnValue;
            }
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            GooglePager1.GooglePagerChanged += new WillCallWeb.Components.Navigation.gglPager.GooglePagerChangedEvent(GooglePager1_GooglePagerChanged);
        }
        public override void Dispose()
        {
            GooglePager1.GooglePagerChanged += new WillCallWeb.Components.Navigation.gglPager.GooglePagerChangedEvent(GooglePager1_GooglePagerChanged);
            base.Dispose();
        }
        protected void GooglePager1_GooglePagerChanged(object sender, WillCallWeb.Components.Navigation.gglPager.GooglePagerEventArgs e)
        {
            Atx.adminPageSize = e.NewPageSize;
            GridView1.PageIndex = e.NewPageIndex;
            GridView1.PageSize = Atx.adminPageSize;
        }
        protected void GridView_Init(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;
            GooglePager1.PageSize = Atx.adminPageSize;
            grid.PageSize = GooglePager1.PageSize;
            grid.PageIndex = GooglePager1.PageIndex;
        }
        protected void GridView_DataBound(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;

            if (grid.SelectedIndex == -1 && grid.Rows.Count > 0)
                grid.SelectedIndex = 0;

            GooglePager1.DataBind();

            FormView1.DataBind();
        }

        #endregion

        protected void chkRandom_CheckChanged(object sender, EventArgs e)
        {
            CheckBox chk = (CheckBox)sender;

            SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Images.ToString().ToLower() &&
                            match.Name.ToLower() == "headerimages_ignoreorder");
                    });

            if (config != null && config.Id > 0)
            {
                config.ValueX = chk.Checked.ToString();
                config.Save();
            }


            //_Config._HeaderImages_IgnoreOrder = chk.Checked;
        }

        #region Page Overhead

        protected override void OnPreRender(EventArgs e) 
        { 
            base.OnPreRender(e);

            //register upload buttons to do full postbacks
            //ScriptManager mgr = (ScriptManager)this.Page.Master.FindControl("ScriptManager1");

            ////edit template
            //Button btnUpload = (Button)FormView1.FindControl("btnUpload");            
            //if (btnUpload != null && mgr != null)
            //    mgr.RegisterPostBackControl(btnUpload);

            
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GridView1.DataBind();
            }
        }

        #endregion

        #region Grid

        protected int _rowCounter = 0;
        protected void GridView_DataBinding(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;
            _rowCounter = grid.PageSize * grid.PageIndex;
        }        
        protected void GridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridView grid = (GridView)sender;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //display row number
                _rowCounter += 1;
                Literal rowCounter = (Literal)e.Row.FindControl("LiteralRowCounter");
                if (rowCounter != null)
                    rowCounter.Text = _rowCounter.ToString();

                Literal litNaming = (Literal)e.Row.FindControl("litNaming");
                Literal litImage = (Literal)e.Row.FindControl("litImage");
                Literal litDates = (Literal)e.Row.FindControl("litDates");

                if (litNaming != null && litImage != null && litDates != null)
                {
                    HeaderImage entity = (HeaderImage)e.Row.DataItem;
                    //name display additional
                    string filename = entity.FileName;
                    string display = entity.DisplayText;
                    string context = entity.VcContext.Replace(",", ", ");
                    string priority = (entity.IsDisplayPriority) ? "Has display priority" : string.Empty;
                    string exclusive = (entity.IsExclusive) ? "Is exclusive" : string.Empty;

                    litNaming.Text = string.Format("{0}{1}{2}{3}{4}",
                        (filename != null && filename.Trim().Length > 0) ? string.Format("<div>{0}</div>", filename) : string.Empty,
                        (display != null && display.Trim().Length > 0) ? string.Format("<div>{0}</div>", display) : string.Empty,
                        (context != null && context.Trim().Length > 0) ? string.Format("<div>{0}</div>", context) : string.Empty,
                        (priority != null && priority.Trim().Length > 0) ? string.Format("<div>{0}</div>", priority) : string.Empty,
                        (exclusive != null && exclusive.Trim().Length > 0) ? string.Format("<div>{0}</div>", exclusive) : string.Empty);

                    if (litImage != null)
                    {
                        string imageUrl = entity.VirtualFilePath;
                        string displayImage = string.Empty;

                        if (imageUrl != null && imageUrl.Trim().Length > 0)
                        {
                            //get the image - dimensions
                            //display the image only so big - max height : 50 max width: 200
                            //so two versions of the image
                            string mappedPath = Server.MapPath(imageUrl);

                            try
                            {
                                Pair p = Utils.ImageTools.GetDimensions(mappedPath);

                                //if over max width//if over max height
                                int width = (int)p.First;
                                int height = (int)p.Second;

                                if (height > 40)
                                    displayImage = string.Format("<img src=\"{0}\" border=\"0\" height=\"25\" />", imageUrl);
                                else
                                    displayImage = string.Format("<img src=\"{0}\" border=\"0\" width=\"180\" />", imageUrl);
                            }
                            catch (Exception ex)
                            {
                                Wcss._Error.LogException(ex);
                            }
                        }

                        litImage.Text = (displayImage.Trim().Length > 0) ? displayImage : string.Empty;
                    }

                    string start = (entity.DateStart <= DateTime.MinValue) ? string.Empty : entity.DateStart.ToString("MM/dd/yyyy hh:mmtt");
                    string end = (entity.DateEnd >= DateTime.MaxValue) ? string.Empty : entity.DateEnd.ToString("MM/dd/yyyy hh:mmtt");
                    litDates.Text = string.Format("{0} {2} {1}", (start.Trim().Length > 0) ? string.Format("<div>start {0}</div>", start) : string.Empty, 
                        (end.Trim().Length > 0) ? string.Format("<div>end {0}</div>", end) : string.Empty,
                        (start.Trim().Length > 0 && start.Trim().Length > 0) ? "-" : string.Empty).Trim();

                    LinkButton delete = (LinkButton)e.Row.FindControl("btnDelete");
                    LinkButton up = (LinkButton)e.Row.FindControl("btnUp");
                    LinkButton down = (LinkButton)e.Row.FindControl("btnDown");

                    if (delete != null)
                        delete.OnClientClick = string.Format("return confirm('Are you sure you want to delete {0}?')",
                            Utils.ParseHelper.ParseJsAlert(entity.FileName));

                    if (up != null && down != null)
                    {
                        up.Enabled = (e.Row.RowIndex > 0);
                        down.Enabled = (e.Row.RowIndex < (GooglePager1.DataSetSize - 1));

                        //TODO figure out the css selector tp get at this element and change the cursor
                        //up.Attributes.Add("class", (up.Enabled) ? "btn-up" : "btn-up disbled");
                        //down.Attributes.Add("class", (down.Enabled) ? "btn-up" : "btn-down disbled");
                    }
                }
            }
        }
        protected void GridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridView grid = (GridView)sender;
            string cmd = e.CommandName.ToLower();

            switch (cmd)
            {
                case "up":
                case "down":                    

                    int ordHold = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer).DataItemIndex;
                    int ordSwap = (cmd == "up") ? ordHold - 1 : ordHold + 1;

                    int idxHold = (int)grid.DataKeys[ordHold]["Id"];
                    int idxSwap = (int)grid.DataKeys[ordSwap]["Id"];
                  
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append("DECLARE @holdOrder int SELECT @holdOrder = [iDisplayOrder] FROM [HeaderImage] WHERE [Id] = @idxHold ");
                    sb.Append("DECLARE @swapOrder int SELECT @swapOrder = [iDisplayOrder] FROM [HeaderImage] WHERE [Id] = @idxSwap ");
                    sb.Append("UPDATE [HeaderImage] SET [iDisplayOrder] = @swapOrder WHERE [Id] = @idxHold ");
                    sb.Append("UPDATE [HeaderImage] SET [iDisplayOrder] = @holdOrder WHERE [Id] = @idxSwap ");

                    SubSonic.QueryCommand command = new SubSonic.QueryCommand(sb.ToString(), SubSonic.DataService.Provider.Name);
                    command.Parameters.Add("@idxHold", idxHold, DbType.Int32);
                    command.Parameters.Add("@idxSwap", idxSwap, DbType.Int32);

                    SubSonic.DataService.ExecuteQuery(command);

                    //select the index that is being moved into
                    grid.SelectedIndex = ordSwap;
                    grid.DataBind();
                  
                    break;
            }
        }

        #endregion

        #region FormView
        
        protected void FormView1_ItemInserting(object sender, FormViewInsertEventArgs e)
        {
            //determine if we can upload the file first etc
            //transfer the file to the file name for the entity
            DoUpload(true);

        }
        protected void FormView1_ItemInserted(object sender, FormViewInsertedEventArgs e)
        {
            if (e.Exception != null)
            {
                CustomValidator validator = (CustomValidator)GooglePager1.FindControl("CustomValidator1");
                if (validator != null)
                {
                    validator.IsValid = false;
                    validator.ErrorMessage = e.Exception.Message;
                }
                return;
            }

            GridView1.PageIndex = 0;
            GridView1.SelectedIndex = -1;
            GridView1.DataBind();
        }
        protected void SqlNaming_Updating(object sender, SqlDataSourceCommandEventArgs e)
        {
            FormView form = FormView1;

            //update context selections
            //tshowid, tmerchid, contextlist
            DropDownList ddlContextShowList = (DropDownList)form.FindControl("ddlContextShowList");
            e.Command.Parameters["@tShowId"].Value = (ddlContextShowList != null && ddlContextShowList.SelectedIndex > 0) ? 
                int.Parse(ddlContextShowList.SelectedValue) : System.Data.SqlTypes.SqlInt32.Null;

            DropDownList ddlContextMerchList = (DropDownList)form.FindControl("ddlContextMerchList");
            e.Command.Parameters["@tMerchId"].Value = (ddlContextMerchList != null && ddlContextMerchList.SelectedIndex > 0) ?
                int.Parse(ddlContextMerchList.SelectedValue) : System.Data.SqlTypes.SqlInt32.Null;

            //convert context to a list
            //bind the list
            //assign selections to list
            CheckBoxList chkContext = (CheckBoxList)form.FindControl("chkContext");

            if (chkContext != null)
            {
                string ctx = string.Empty;
                
                foreach (ListItem li in chkContext.Items)
                    if (li.Selected)
                        ctx += string.Format("{0},", li.Value.Trim());

                e.Command.Parameters["@vcContext"].Value = (ctx.Trim().Length > 0) ? ctx.Trim(new char[] {',', ' '}) : null;
            }

            //Set start and end date
            WillCallWeb.Components.Util.CalendarClock start = (WillCallWeb.Components.Util.CalendarClock)FormView1.FindControl("CalendarClockStart");
            WillCallWeb.Components.Util.CalendarClock end = (WillCallWeb.Components.Util.CalendarClock)FormView1.FindControl("CalendarClockEnd");
            if (start != null && end != null)
            {
                //if the value is required - default is min value - but you need to decide for every case
                DateTime defaultStart = start.DefaultValue;
                DateTime selectedStart = start.SelectedDate;
                DateTime defaultEnd = end.DefaultValue;
                DateTime selectedEnd = end.SelectedDate;

                //this will change according to control's needs
                e.Command.Parameters["@dtStart"].Value = (selectedStart != defaultStart) ? selectedStart : System.Data.SqlTypes.SqlDateTime.Null;
                e.Command.Parameters["@dtEnd"].Value = (selectedEnd != defaultEnd) ? selectedEnd : System.Data.SqlTypes.SqlDateTime.Null;
            }

            e.Command.Parameters["@dtModified"].Value = DateTime.Now;
        }
        protected void SqlNaming_Deleting(object sender, SqlDataSourceCommandEventArgs e)
        {
            FormView form = FormView1;

            int idx = (int)e.Command.Parameters["@Idx"].Value;
            e.Command.Parameters["@FileName"].Value = form.DataKey["FileName"];

            if (idx > 0)
            {
                string sql = "DECLARE @deletedDisplayOrder int; SELECT @deletedDisplayOrder = [iDisplayOrder] FROM [HeaderImage] WHERE [Id] = @Idx; ";
                sql += "UPDATE [HeaderImage] SET [iDisplayOrder] = -10000 WHERE [Id] = @Idx; ";
                sql += "UPDATE [HeaderImage] SET [iDisplayOrder] = [iDisplayOrder] - 1 WHERE [iDisplayOrder] > @deletedDisplayOrder; ";
                sql += "DELETE FROM [HeaderImage] WHERE [Id] = @Idx; ";
                e.Command.CommandText = sql;                
            }
        }
        protected void SqlNaming_Deleted(object sender, SqlDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                string filename = e.Command.Parameters["@FileName"].Value.ToString();
                //string filename = grid.DataKeys[itemIdx]["FileName"].ToString();
                if (filename != null && filename.Trim().Length > 0)
                {
                    string mappedFile = Server.MapPath(string.Format("{0}{1}", HeaderImage.VirtualDirectory, filename));
                    if (File.Exists(mappedFile))
                        File.Delete(mappedFile);
                }

                GridView1.SelectedIndex = -1;
                GridView1.DataBind();
            }
        }
        protected void FormView1_ItemUpdated(object sender, FormViewUpdatedEventArgs e)
        {
            FormView form = (FormView)sender;
            if(e.AffectedRows > 0)
                GridView1.DataBind();
        }
        protected void FormView1_ModeChanging(object sender, FormViewModeEventArgs e)
        {
            FormView form = (FormView)sender;
            FormViewMode oldMode = form.CurrentMode;
            form.ChangeMode(e.NewMode);
            if (e.CancelingEdit || (oldMode == FormViewMode.Insert && form.CurrentMode == FormViewMode.Edit))//handles cancel correctly
                form.DataBind();
        }
        protected void SqlShowList_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters["@appId"].Value = Wcss._Config.APPLICATION_ID;
            e.Command.Parameters["@startDate"].Value = DateTime.Now.Date;

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("CREATE TABLE #tmpShowIds(ShowId int); INSERT #tmpShowIds(ShowId) ");
            sb.Append("SELECT DISTINCT(s.[Id]) AS 'ShowId' FROM [ShowDate] sd, [Show] s ");
            sb.Append("WHERE sd.[dtDateOfShow] > @startDate AND sd.[tShowId] = s.[Id] AND s.[ApplicationId] = @appId ");
            sb.Append("IF EXISTS (SELECT * FROM [#tmpShowIds]) BEGIN  ");
            sb.Append("SELECT ' [..Select Show..]' as ShowName, 0 as ShowId UNION   ");
            sb.Append("SELECT s.[Name] + ");
            sb.Append("CASE WHEN v.City IS NULL AND v.State IS NULL THEN '' ELSE ");
            sb.Append("ISNULL(v.[City],'') + ' ' + ISNULL(v.[State],'') END as ShowName, s.[Id] as ShowId  ");
            sb.Append("FROM #tmpShowIds ids, Show s LEFT OUTER JOIN [Venue] v ON s.[tVenueId] = v.[Id]  ");
            sb.Append("WHERE ids.[ShowId] = s.[Id] AND s.[ApplicationId] = @appId  ");
            sb.Append("ORDER BY ShowName ASC END ELSE BEGIN SELECT  ' [..NO Shows..]' as ShowName, 0 as ShowId END ");

            e.Command.CommandText = sb.ToString();
        }
        protected void SqlMerchList_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters["@appId"].Value = Wcss._Config.APPLICATION_ID;

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("CREATE TABLE #tmpIds(Idx int); ");
            sb.AppendLine();
            sb.Append("INSERT #tmpIds(Idx) ");
            sb.Append("SELECT DISTINCT(m.[Id]) AS [Idx] FROM [Merch] m ");
            sb.Append("WHERE m.[ApplicationId] = @appId AND m.[tParentListing] IS NULL AND m.[bActive] = 1; ");
            sb.AppendLine();
            sb.Append("IF EXISTS (SELECT * FROM [#tmpIds]) BEGIN ");
            sb.Append("SELECT ' [..Select Merch..]' as [ItemName], 0 as [ItemId] UNION ");
            sb.AppendLine();
            sb.Append("SELECT md.[Name] + ' - ' + mc.[Name] + ' - ' + m.[Name] as [ItemName], m.[Id] as [ItemId] ");
            sb.AppendLine();
            sb.Append("FROM #tmpIds ids, Merch m LEFT OUTER JOIN [MerchJoinCat] mjc ON mjc.[tMerchId] = m.[ID] ");
            sb.Append("LEFT OUTER JOIN [MerchCategorie] mc ON  mc.[Id] = mjc.[tMerchCategorieId] ");
            sb.Append("LEFT OUTER JOIN [MerchDivision] md ON md.[Id] = mc.[tMerchDivisionId] ");
            sb.AppendLine();
            sb.Append("	WHERE ids.[Idx] = m.[Id] ");
            sb.AppendLine();
            sb.Append("ORDER BY [ItemName] ASC END ELSE BEGIN SELECT  ' [..NO Merch..]' as [ItemName], 0 as [ItemId] ");
            sb.AppendLine();
            sb.Append("END ");

            e.Command.CommandText = sb.ToString();
        }
        protected void FormView1_ItemCommand(object sender, FormViewCommandEventArgs e)
        {
            FormView form = (FormView)sender;

            string cmd = e.CommandName.ToLower();

            int idx = (form.DataKey != null && form.DataKey["Id"] != null) ? (int)form.DataKey["Id"] : 0;
            TextBox txtUrl = (TextBox)form.FindControl("txtClickUrl");

            switch (cmd)
            {
                case "loadshow":
                    DropDownList ddlShow = (DropDownList)form.FindControl("ddlCreateLinkShowList");
                    if (ddlShow != null && ddlShow.SelectedValue != "0")
                    {
                        try
                        {
                            int selIdx = int.Parse(ddlShow.SelectedValue);
                            if (txtUrl != null && selIdx > 0)
                            {
                                Show s = new Show(selIdx);

                                //we do not save to the entity here - we leave that for the save button

                                if (s != null)
                                    txtUrl.Text = s.FriendlyUrl;

                                Literal litUrlTest = (Literal)form.FindControl("litUrlTest");
                                if (litUrlTest != null)
                                    litUrlTest.DataBind();
                            }
                        }
                        catch (Exception ex)
                        {
                            _Error.LogException(ex);
                            CustomValidator validator = (CustomValidator)GooglePager1.FindControl("CustomValidator1");
                            if (validator != null)
                            {
                                validator.IsValid = false;
                                validator.ErrorMessage = ex.Message;
                            }
                        }
                    }
                    break;
                case "clearurl":
                    if (txtUrl != null)
                    {
                        string inp = txtUrl.Text.Trim();
                        if (inp.Length > 0)
                        {
                            if(txtUrl != null)
                                txtUrl.Text = string.Empty;
                        }
                    }
                    break;
            }
        }
        protected void FormView1_DataBinding(object sender, EventArgs e)
        {
            FormView form = (FormView)sender;
        }
        protected void FormView1_ItemCreated(object sender, EventArgs e)
        {
            FormView form = (FormView)sender;

            FileUpload upl = (FileUpload)form.FindControl("FileUpload1");
            if (upl != null)
                upl.Attributes.Add("size", "55");
        }
        protected void FormView1_DataBound(object sender, EventArgs e)
        {
            FormView form = (FormView)sender;

            if (form.CurrentMode == FormViewMode.Insert)
            {
                ScriptManager mgr = (ScriptManager)this.Page.Master.FindControl("ScriptManager1");

                //insert template
                Button btnSaveInsert = (Button)form.FindControl("btnSaveInsert");
                if (btnSaveInsert != null && mgr != null)
                {
                    mgr.RegisterPostBackControl(btnSaveInsert);
                }
            }
            else if (form.CurrentMode != FormViewMode.Insert && form.SelectedValue != null)
            {
                DataRowView drv = (DataRowView)form.DataItem;
                DataRow row = drv.Row;

                ScriptManager mgr = (ScriptManager)this.Page.Master.FindControl("ScriptManager1");
                Button btnUpload = (Button)form.FindControl("btnUpload");
                if (btnUpload != null && mgr != null)
                {
                    mgr.RegisterPostBackControl(btnUpload);
                }

                #region Bind To Context

                string context = Utils.DataHelper.GetColumnValue(row, "vcContext", DbType.String) as string;
                int showid = (int)Utils.DataHelper.GetColumnValue(row, "tShowId", DbType.Int32);
                int merchid = (int)Utils.DataHelper.GetColumnValue(row, "tMerchId", DbType.Int32);

                //convert context to a list
                //bind the list
                //assign selections to list                
                CheckBoxList chkContext = (CheckBoxList)form.FindControl("chkContext");
                if (chkContext != null)
                {
                    chkContext.ClearSelection();//.SelectedIndex = -1;
                 
                    if (context != null)
                    {
                        string[] ctx = context.Split(',');
                        foreach (string s in ctx)
                        {
                            ListItem li = chkContext.Items.FindByValue(s.Trim());
                            if (li != null)
                                li.Selected = true;
                        }
                    }
                }

                
                if (showid > 0)
                {
                    DropDownList ddlContextShowList = (DropDownList)form.FindControl("ddlContextShowList");
                    if (ddlContextShowList != null)
                    {
                        ddlContextShowList.ClearSelection();

                        ListItem li = ddlContextShowList.Items.FindByValue(showid.ToString());
                        if (li != null && (! li.Selected))
                        {
                            ddlContextShowList.SelectedIndex = -1;
                            li.Selected = true;
                        }
                    }
                }

                if (merchid > 0)
                {
                    DropDownList ddlContextMerchList = (DropDownList)form.FindControl("ddlContextMerchList");
                    if (ddlContextMerchList != null)
                    {
                        ddlContextMerchList.ClearSelection();

                        ListItem li = ddlContextMerchList.Items.FindByValue(merchid.ToString());
                        if (li != null && (!li.Selected))
                        {
                            ddlContextMerchList.SelectedIndex = -1;
                            li.Selected = true;
                        }
                    }
                }
                
                #endregion



                //fit to appropriate width for admin display
                string fileName = Utils.DataHelper.GetColumnValue(row, "FileName", DbType.String) as string;
                Literal litBigImage = (Literal)form.FindControl("litBigImage");

                if (fileName != null && fileName.Trim().Length > 0 && litBigImage != null)
                {
                    string url = string.Format("{0}{1}", Wcss.HeaderImage.VirtualDirectory, fileName);

                    string mappedFile = Server.MapPath(url);

                    Pair p = Utils.ImageTools.GetDimensions(mappedFile);

                    //if over max width//if over max height
                    int maxWidth = 600;
                    int maxHeight = 80;
                    int width = (int)p.First;
                    int height = (int)p.Second;

                    
                    if (width > maxWidth)
                    {
                        decimal h = height * (maxWidth / (decimal)width);
                        height = (int)h;
                        width = maxWidth;
                    }

                    if (height > maxHeight)
                    {
                        decimal w = width * (maxHeight / (decimal)height);
                        width = (int)w;
                        height = maxHeight;
                    }

                    litBigImage.Text = string.Format("<img src=\"{0}\" border=\"0\" alt=\"\" width=\"{1}\" height=\"{2}\" />",
                           url, width.ToString(), height.ToString());
                }
            }
        }

        #endregion

        #region Auxil

        protected void txtClickUrl_TextChanged(object sender, EventArgs e)
        {
            Literal lit = (Literal)FormView1.FindControl("litUrlTest");
            if (lit != null)
                lit.DataBind();
        }
        protected void litUrlTest_DataBinding(object sender, EventArgs e)
        {
            Literal lit = (Literal)sender;
            lit.Text = string.Empty;

            //if the text box has text - create a url - else empty
            TextBox txt = (TextBox)FormView1.FindControl("txtClickUrl");
            if (txt != null)
            {
                string input = txt.Text.Trim();

                if (input.Length > 0)
                    lit.Text = string.Format("<a target=\"_blank\" href=\"{0}\">test</a>", input);
            }
        }
        #endregion

        #region Uploads

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            DoUpload(false);
        }
        protected void DoUpload(bool IsNewEntry)
        {
            FormView form = FormView1;
            FileUpload upload = (FileUpload)form.FindControl("FileUpload1");
            CustomValidator validator = (CustomValidator)GooglePager1.FindControl("CustomValidator1");
            
            //update the other values so the changes do not get lost based on the uploads failure/success
            if(! IsNewEntry)
                form.UpdateItem(false);

            if (upload != null && upload.HasFile)
            {
                string uploadName = System.Text.RegularExpressions.Regex.Replace(upload.FileName, @"\s+", string.Empty);
                string uploadExt = Path.GetExtension(uploadName).ToLower();

                if (uploadExt != ".jpg" && uploadExt != ".jpeg" && uploadExt != ".gif" && uploadExt != ".png")
                {
                    validator.IsValid = false;
                    validator.ErrorMessage = "Valid file types are jpg, jpeg, gif and png only.";
                    return;
                }
                if (uploadName.Length > 256)
                {
                    validator.IsValid = false;
                    validator.ErrorMessage = "Valid file names must be 256 chars or less.";
                    return;
                }

                /*ERROR CHECK for existing files - new files should throw error - existing files will essentially overwrite*/
                string existing = null;

                if (!IsNewEntry)
                {
                    int idx = (int)form.DataKey["Id"];
                    existing = form.DataKey["FileName"] as string;
                }
                else
                    existing = uploadName;

                //if there is already a banner file - then delete - if it is a new entry then return error
                if (existing != null && existing.Trim().Length > 0)
                {
                    string mappedExisting = Server.MapPath(string.Format("{0}{1}", HeaderImage.VirtualDirectory, existing));
                    if (File.Exists(mappedExisting))
                    {
                        if (IsNewEntry)
                        {
                            validator.IsValid = false;
                            validator.ErrorMessage = "This file you are trying to upload already exists and may not be used as a NEW Header Image.";
                            return;
                        }

                        File.Delete(mappedExisting);
                    }
                }
                /*Done handling existing files */


                //declare outside of try block so that it is in scope of exception blocks
                string mappedFile = string.Empty;

                try
                {
                    string mappedUploadPath = Server.MapPath(HeaderImage.VirtualDirectory);

                    mappedFile = string.Format("{0}{1}", mappedUploadPath, uploadName);

                    //this will overwrite existing if not checked
                    upload.SaveAs(mappedFile);

                    //see if you can create dimensions
                    //if this does not work - then there is something wrong with the image - maybe cmyk                    
                    Utils.ImageTools.GetDimensions(mappedFile);

                    //save to db
                    string sql = "UPDATE [HeaderImage] SET [FileName] = @FileName WHERE [Id] = @Id ";
                    
                    SubSonic.QueryCommand cmd = new SubSonic.QueryCommand(sql, SubSonic.DataService.Provider.Name);
                    cmd.Parameters.Add("@FileName", uploadName);

                    if (IsNewEntry)
                    {
                        //display order - add to head of collection!! Update others to allow entry
                        sql = "UPDATE [HeaderImage] SET [iDisplayOrder] = [iDisplayOrder] + 1 WHERE [bActive] = 1; ";
                        sql += "INSERT [HeaderImage] (bActive, iDisplayOrder, bDisplayPriority, iTimeoutMsec, FileName, DisplayText, vcContext, ApplicationId) ";
                        sql += "VALUES (1, 0, 0, @timeout, @fileName, @displayText, @vcContext, @appId)";

                        cmd.Parameters.Add("@timeout", _Config._HeaderImage_Default_TimeoutMsecs, DbType.Int32);
                        TextBox displayText = (TextBox)form.FindControl("txtDisplayText");
                        if(displayText != null && displayText.Text.Trim().Length > 0)
                            cmd.Parameters.Add("@displayText", displayText.Text.Trim());
                        else
                            cmd.Parameters.Add("@displayText", null);

                        cmd.Parameters.Add("@vcContext", _Enums.HeaderImageContext.All.ToString());
                        cmd.Parameters.Add("@appId", _Config.APPLICATION_ID, DbType.Guid);

                        cmd.CommandSql = sql;
                    }
                    else
                    {
                        cmd.Parameters.Add("@Id", (int)form.DataKey["Id"], DbType.Int32);
                    }

                    SubSonic.DataService.ExecuteQuery(cmd);

                    GridView1.DataBind();                    
                }
                catch (OutOfMemoryException)
                {
                    if (File.Exists(mappedFile))
                        File.Delete(mappedFile);

                    throw new System.ArgumentOutOfRangeException(string.Format("An Image file could not be created from the file specified - \"{0}\" ", mappedFile));
                }
                catch (Exception ex)
                {
                    if (File.Exists(mappedFile))
                        File.Delete(mappedFile);

                    Wcss._Error.LogException(ex);
                    validator.IsValid = false;
                    validator.ErrorMessage = ex.Message;
                    return;
                }

                form.DataBind();
            }
        }

        #endregion
}
}