using System;
using System.IO;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

using Wcss;

namespace WillCallWeb.Admin.AdminControls
{
    public partial class Prom_Banner : BaseControl
    {
        #region New paging

        bool isSelectCount;
        protected void objData_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            isSelectCount = e.ExecutingSelectCount;

            if (!isSelectCount)
            {
                e.Arguments.StartRowIndex = GooglePager1.StartRowIndex;// (GridView1.PageIndex * GridView1.PageSize) + 1;
                e.Arguments.MaximumRows = GooglePager1.PageSize;// GridView1.PageSize;
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

            //GridView1.DataBind();
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

        #region Page Overhead

        protected override void OnPreRender(EventArgs e) 
        { 
            base.OnPreRender(e);

            Button btnUpload = (Button)FormView1.FindControl("btnUpload");
            //register the upload button to do a full postback
            ScriptManager mgr = (ScriptManager)this.Page.Master.FindControl("ScriptManager1");
            if (btnUpload != null && mgr != null)
                mgr.RegisterPostBackControl(btnUpload);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GridView1.DataBind();
            }
        }

        protected void btnPromo_Click(object sender, EventArgs e)
        {
            base.Redirect("/Admin/PromotionEditor.aspx?p=promo");
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
                    SalePromotion entity = (SalePromotion)e.Row.DataItem;
                    //name display additional
                    string name = entity.Name;
                    string display = entity.DisplayText;
                    string additional = entity.AdditionalText;

                    litNaming.Text = string.Format("{0}{1}{2}", 
                        (name != null && name.Trim().Length > 0) ? string.Format("<div>{0}</div>", name) : string.Empty,
                        (display != null && display.Trim().Length > 0) ? string.Format("<div>{0}</div>", display) : string.Empty,
                        (additional != null && additional.Trim().Length > 0) ? string.Format("<div>{0}</div>", additional) : string.Empty);

                    string imageUrl = entity.BannerUrl;

                    string image = string.Empty;
                    if (imageUrl != null && imageUrl.Trim().Length > 0)
                    {
                        //get the image - dimensions
                        //display the image only so big - max height : 50 max width: 200
                        //so two versions of the image
                        string mappedPath = Server.MapPath(string.Format("{0}{1}", Wcss.SalePromotion.Banner_VirtualDirectory, imageUrl));

                        try
                        {
                            Pair p = Utils.ImageTools.GetDimensions(mappedPath);

                            //if over max width//if over max height
                            int width = (int)p.First;
                            int height = (int)p.Second;

                            imageUrl = string.Format("{0}{1}", Wcss.SalePromotion.Banner_VirtualDirectory, imageUrl);

                            if (height > 50)
                                image = string.Format("<img src=\"{0}\" border=\"0\" height=\"45\" />", imageUrl);
                            else
                                image = string.Format("<img src=\"{0}\" border=\"0\" width=\"180\" />", imageUrl);
                        }
                        catch (Exception ex)
                        {
                            Wcss._Error.LogException(ex);
                        }
                    }

                    litImage.Text = (image.Trim().Length > 0) ? image : string.Empty;

                    string start = (entity.DateStart <= DateTime.MinValue) ? string.Empty : entity.DateStart.ToString("MM/dd/yyyy hh:mmtt");
                    string end = (entity.DateEnd >= DateTime.MaxValue) ? string.Empty : entity.DateEnd.ToString("MM/dd/yyyy hh:mmtt");
                    litDates.Text = string.Format("{0} {2} {1}", (start.Trim().Length > 0) ? string.Format("<div>start {0}</div>", start) : string.Empty, 
                        (end.Trim().Length > 0) ? string.Format("<div>end {0}</div>", end) : string.Empty,
                        (start.Trim().Length > 0 && start.Trim().Length > 0) ? "-" : string.Empty).Trim();
                }
            }
        }
        #endregion

        #region FormView
        protected void FormView1_ItemInserting(object sender, FormViewInsertEventArgs e)
        {
            e.Values["ApplicationId"] = _Config.APPLICATION_ID.ToString();
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
                e.Command.Parameters["@dtStartDate"].Value = (selectedStart != defaultStart) ? selectedStart : System.Data.SqlTypes.SqlDateTime.Null;
                e.Command.Parameters["@dtEndDate"].Value = (selectedEnd != defaultEnd) ? selectedEnd : System.Data.SqlTypes.SqlDateTime.Null;
            }
        }
        protected void SqlNaming_Inserting(object sender, SqlDataSourceCommandEventArgs e)
        {
            e.Command.Parameters["@iBannerTimeoutMsecs"].Value = _Config._BannerDisplayTime;
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
            sb.Append("SELECT s.[Name] + ' - ' +  ");
            sb.Append("ISNULL(v.[City],'') + ' ' + ISNULL(v.[State],'') as ShowName, s.[Id] as ShowId  ");
            sb.Append("FROM #tmpShowIds ids, Show s LEFT OUTER JOIN [Venue] v ON s.[tVenueId] = v.[Id]  ");
            sb.Append("WHERE ids.[ShowId] = s.[Id] AND s.[ApplicationId] = @appId  ");
            //sb.Append("ORDER BY 'ShowName' ASC END ");
            sb.Append("ORDER BY ShowName ASC END ELSE BEGIN SELECT  ' [..NO Shows..]' as ShowName, 0 as ShowId END ");

            e.Command.CommandText = sb.ToString();
        }
        protected void FormView1_ItemCommand(object sender, FormViewCommandEventArgs e)
        {
            FormView form = (FormView)sender;

            string cmd = e.CommandName.ToLower();

            int idx = (form.DataKey != null && form.DataKey["Id"] != null) ? (int)form.DataKey["Id"] : 0;
            TextBox txtUrl = (TextBox)form.FindControl("txtBannerClickUrl");

            switch (cmd)
            {
                case "loadshow":
                    DropDownList ddlShow = (DropDownList)form.FindControl("ddlShowList");
                    if (ddlShow != null && ddlShow.SelectedValue != "0")
                    {
                        try
                        {
                            int selIdx = int.Parse(ddlShow.SelectedValue);
                            if (selIdx > 0)
                            {
                                string constructUrl = string.Format("/Store/ChooseTicket.aspx?sid={0}", selIdx.ToString());
                                if (txtUrl != null)
                                    txtUrl.Text = constructUrl;
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
                case "deletebanner":
                    string bannerUrl = form.DataKey["BannerUrl"] as string;

                    if (bannerUrl != null && bannerUrl.Trim().Length > 0)
                    {
                        try
                        {
                            string mappedUploadPath = Server.MapPath(Wcss.SalePromotion.Banner_VirtualDirectory);
                            string mappedExisting = string.Format("{0}{1}", mappedUploadPath, bannerUrl);
                            if (File.Exists(mappedExisting))
                                File.Delete(mappedExisting);

                            //save to db
                            SubSonic.QueryCommand command = new SubSonic.QueryCommand("UPDATE [SalePromotion] SET [BannerUrl] = NULL WHERE [Id] = @Id ",
                                SubSonic.DataService.Provider.Name);
                            command.Parameters.Add("@Id", idx, DbType.Int32);
                            SubSonic.DataService.ExecuteQuery(command);

                            GridView1.DataBind();
                            form.DataBind();
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
            
            if (form.CurrentMode != FormViewMode.Insert && form.SelectedValue != null)
            {
                DataRowView drv = (DataRowView)form.DataItem;
                DataRow row = drv.Row;

                //DropDownList ddlShow = (DropDownList)form.FindControl("ddlShowList");
                //if (ddlShow != null)
                //{
                //    ddlShow.DataBind();
                //}


                //(banners will be sized to 960wx90h)--fox
                //(images should be 650px wide and up to 150 in height)
                int width = 650;


                string bannerUrl = Utils.DataHelper.GetColumnValue(row, "BannerUrl", DbType.String) as string;
                Literal litImage = (Literal)form.FindControl("litImage");
                if (bannerUrl != null && bannerUrl.Trim().Length > 0 && litImage != null)
                {
                    litImage.Text = string.Format("<img src=\"{0}{1}\" border=\"0\" alt=\"\" width=\"{2}\" />",
                        Wcss.SalePromotion.Banner_VirtualDirectory, bannerUrl, width.ToString());
                }
                else if (litImage != null)
                    litImage.Text = string.Format("<img src=\"/Images/view.gif\" border=\"0\" alt=\"\" > no image specified");

                Button deleteBanner = (Button)form.FindControl("btnDeleteBanner");
                if(deleteBanner != null)
                    deleteBanner.Visible = bannerUrl != null && bannerUrl.Trim().Length > 0;
            }
        }
        #endregion

        #region Auxil
        protected void txtBannerClick_TextChanged(object sender, EventArgs e)
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
            TextBox txt = (TextBox)FormView1.FindControl("txtBannerClickUrl");
            if (txt != null)
            {
                string input = txt.Text.Trim();

                if (input.Length > 0)
                {
                    //string formatted = Utils.ParseHelper.FormatUrlFromString(input, false);
                    lit.Text = string.Format("<a target=\"_blank\" href=\"{0}\">test</a>", input);
                    
                    
                    //if (Utils.Validation.IsValidUrl(input))
                    //    lit.Text = string.Format("<a target=\"_blank\" class=\"btnadmin\" href=\"{0}\">test</a>", Utils.ParseHelper.FormatUrlFromString(input, true));
                    //else
                    //    lit.Text = "invalid url";
                }
            }
        }
        #endregion

        #region Uploads
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            FormView form = FormView1;
            FileUpload upload = (FileUpload)form.FindControl("FileUpload1");
            CustomValidator validator = (CustomValidator)GooglePager1.FindControl("CustomValidator1");
            
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

                string mappedFile = string.Empty;

                try
                {
                    int idx = (int)form.DataKey["Id"];
                    string existingBanner = form.DataKey["BannerUrl"] as string;

                    //if there is already a banner file - then delete
                    if (existingBanner != null && existingBanner.Trim().Length > 0)
                    {
                        string mappedExisting = Server.MapPath(string.Format("{0}{1}", SalePromotion.Banner_VirtualDirectory, existingBanner));
                        if (File.Exists(mappedExisting))
                            File.Delete(mappedExisting);
                    }

                    string mappedUploadPath = Server.MapPath(Wcss.SalePromotion.Banner_VirtualDirectory);

                    mappedFile = string.Format("{0}{1}", mappedUploadPath, uploadName);

                    //this will overwrite existing if not checked
                    upload.SaveAs(mappedFile);

                    //see if you can create dimensions
                    //if this does not work - then there is something wrong with the image - maybe cmyk                    
                    Utils.ImageTools.GetDimensions(mappedFile);
                    Utils.ImageTools.CreateAndSaveThumbnailImage(mappedFile, "", 20, false);

                    //save to db
                    SubSonic.QueryCommand cmd = new SubSonic.QueryCommand("UPDATE [SalePromotion] SET [BannerUrl] = @ImageName WHERE [Id] = @Id ",
                        SubSonic.DataService.Provider.Name);
                    cmd.Parameters.Add("@ImageName", uploadName);
                    cmd.Parameters.Add("@Id", idx, DbType.Int32);
                    SubSonic.DataService.ExecuteQuery(cmd);

                    GridView1.DataBind();
                    form.DataBind();
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
                }
            }
        }
        #endregion
}
}