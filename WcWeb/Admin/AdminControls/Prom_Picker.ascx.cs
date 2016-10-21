using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Collections.Generic;

using Wcss;

/**old awards query with only parents selected - 
 * <asp:SqlDataSource ID="SqlAwardsAvailable" runat="server" ConnectionString="<%$ ConnectionStrings:WillCallConnectionString %>"
    SelectCommand="SELECT m.[Name], m.[Id] FROM [Merch] m WHERE m.[tParentListing] IS NULL AND m.[bActive] = 1 AND 
        m.[Id] NOT IN (SELECT TParentMerchId FROM [SalePromotionAward] spa WHERE spa.TSalePromotionId = @promoId)
        ORDER BY m.[Name] " >
    <SelectParameters>
        <asp:ControlParameter ControlID="GridView1" Name="promoId" PropertyName="SelectedValue" Type="Int32" />
    </SelectParameters>
</asp:SqlDataSource>
 * 
 * query that gets individual GCs
 * 
 *     SelectCommand="        
SELECT [tMerchId] INTO #tmpCats FROM MerchJoinCat WHERE [tMerchCategorieId] IN 
(SELECT [Id] FROM MerchCategorie WHERE Name = 'Gift Certificates')
SELECT parent.[Name] + ' ' + m.[Style] as [Name], m.[Id] FROM [Merch] m, [Merch] parent 
WHERE m.[bActive] = 1 AND parent.[Id] IN 
(SELECT [tMerchId] FROM #tmpCats)
AND m.[tParentListing] = parent.[Id]
UNION
SELECT m.[Name] as [Name], m.[Id] FROM [Merch] m 
WHERE m.[tParentListing] IS NULL AND m.[bActive] = 1 AND 
	m.[Id] NOT IN (SELECT TParentMerchId FROM [SalePromotionAward] spa WHERE spa.TSalePromotionId = 10000) AND
	m.[Id] NOT IN (SELECT [tMerchId] FROM #tmpCats)
ORDER BY [Name]
DROP TABLE #tmpCats " >
 */
namespace WillCallWeb.Admin.AdminControls
{
    public partial class Prom_Picker : BaseControl, IPostBackEventHandler
    {
        void IPostBackEventHandler.RaisePostBackEvent(string eventArgument)
        {
            string[] args = eventArgument.Split('~');
            string command = args[0];
            int idx = (args.Length > 1 && Utils.Validation.IsInteger((string)args[1])) ? int.Parse(args[1]) : 0;
            string result = string.Empty;

            switch (command.ToLower())
            {
                case "merchlist_changed":
                    //reset the current sale promo
                    Atx.CurrentSalePromotion = null;
                    //rebind the list
                    GridView1.DataBind();
                    break;
            }
        }
        protected void btnMerchSelect_Click(object sender, EventArgs e)
        {
            Components.Util.MerchSelector sel = (Components.Util.MerchSelector)this.FormView_Trigger.FindControl("MerchSelector1");
            
            if(sel != null)
            {
                int x = sel.SelectedInventoryId;

                if (x != 0)
                {
                    Merch m = Merch.FetchByID(x);

                    if (m != null && (!Atx.CurrentSalePromotion.RequiredMerchListing.Contains(m.Id)))
                    {
                        //if this is a gift certificate - all other items must be GCs too - can't mix and match GCs
                        bool isGift = m.IsGiftCertificateDelivery;

                        if (Atx.CurrentSalePromotion.Requires_MerchItem)
                        {
                            bool containsGifts = false;
                            foreach (Merch existing in Atx.CurrentSalePromotion.RequiredMerchItems(Ctx))
                            {
                                if (existing.IsGiftCertificateDelivery)
                                {
                                    containsGifts = true;
                                    break;
                                }
                            }

                            //if we are a gift and other items are not gifts
                            //if we are not a gift and other items are gifts
                            if ((isGift && (!containsGifts)) || ((!isGift) && containsGifts))
                            {
                                CustomValidator validation = (CustomValidator)FormView_Trigger.FindControl("CustomFormValidation");

                                if (validation != null)
                                {
                                    validation.ErrorMessage = "Promotional triggers cannot mix and match regular items and gift certificates";
                                    validation.IsValid = false;
                                }

                                return;
                            }
                        }

                        //if the id is not contained - add it
                        //if it is a parent id and there are any child ids - remove the child ids
                        if (m.IsParent)
                        {
                            foreach (Merch child in m.ChildMerchRecords())
                                if (Atx.CurrentSalePromotion.RequiredMerchListing.Contains(child.Id))
                                    Atx.CurrentSalePromotion.RequiredMerchListing.Remove(child.Id);
                        }
                        else
                        {
                            //if it is a child - remove any parent ids
                            if (Atx.CurrentSalePromotion.RequiredMerchListing.Contains(m.TParentListing.Value))
                                Atx.CurrentSalePromotion.RequiredMerchListing.Remove(m.TParentListing.Value);
                        }

                        Atx.CurrentSalePromotion.RequiredMerchListing.Add(m.Id);

                        string sql = string.Format("SET NOCOUNT ON; UPDATE [SalePromotion] SET tRequiredParentShowTicketId = null, tRequiredParentShowDateId = null, vcTriggerList_Merch = @newListing WHERE [Id] = @promoId; SELECT 0; RETURN");
                        _DatabaseCommandHelper helper = new _DatabaseCommandHelper(sql);
                        helper.AddCmdParameter("newListing", Atx.CurrentSalePromotion.ReCalculate_RequiredMerchListing_String(), System.Data.DbType.String);
                        helper.AddCmdParameter("promoId", Atx.CurrentSalePromotion.Id, System.Data.DbType.Int32);
                        object o = helper.PerformQuery("Promo_AddMerchTrigger");

                        Atx.CurrentSalePromotion = null;
                        GridView1.DataBind();
                    }
                }
            }                
        }

        protected int SalePromotionId
        {
            get
            {
                return (Atx.CurrentSalePromotion == null) ? 0 : Atx.CurrentSalePromotion.Id;
            }
        }

        protected string _uniqueId { get { return this.UniqueID; } }

        List<string> _errors = new List<string>();

        #region New paging

        bool isSelectCount;
        protected void objData_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            isSelectCount = e.ExecutingSelectCount;

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

            Wizard1.DataBind();
        }

        #endregion

        #region Wizard
        protected void wizard_DataBinding(object sender, EventArgs e)
        {
            Wizard wiz = (Wizard)sender;
            switch (wiz.ActiveStep.ID.ToLower())
            {
                case "stepnaming":
                    FormView_Naming.DataBind();
                    break;
                case "stepaward":
                    break;
                case "steptrigger":
                    break;
            }
        }
        protected void OnNextStep(object sender, WizardNavigationEventArgs e)
        {
            if (e.NextStepIndex > 0 && Atx.CurrentSalePromotion == null)
            {
                e.Cancel = true;
                return;
            }
        }
        protected void wizard_ActiveStepChanged(object sender, EventArgs e)
        {
            Wizard wiz = (Wizard)sender;
            wiz.DataBind();
        }
        protected void SideBarList_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            //no items should be enabled when the entity is null
            Button sideBarButton = (Button)e.Item.FindControl("SideBarButton");

            if (sideBarButton != null)
            {
                WizardStep wStep = (WizardStep)e.Item.DataItem;
                string activeName = Wizard1.ActiveStep.Name.ToLower();

                sideBarButton.Enabled = (Atx.CurrentSalePromotion != null && wStep.Name.ToLower() != activeName);
            }
            //items should not be enabled when on like-page
        }

        #endregion

        #region Page Overhead

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            string script = "addRemoveLinks();";
            System.Web.UI.ScriptManager.RegisterStartupScript(this.Wizard1, this.Wizard1.GetType(),
                Guid.NewGuid().ToString(), " ;" + script, true);


            //do blocking for upload controls differently
            Atx.RegisterJQueryScript_BlockUIEvents(this.Wizard1, "#srceditor", "('.btnupload')", null);
            
        }  
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Wizard1.ActiveStep.ID == "stepNaming")
                {
                    Literal lit = (Literal)FormView_Naming.FindControl("litUrlTest");
                    if (lit != null)
                        lit.DataBind();
                }
            }
        }
        protected void btnBanner_Click(object sender, EventArgs e)
        {
            base.Redirect("/Admin/PromotionEditor.aspx?p=banner");
        }
        protected void chkDiscApp_DataBinding(object sender, EventArgs e)
        {
            CheckBoxList chk = (CheckBoxList)sender;

            if (chk.Items.Count == 0)
                chk.DataSource = Enum.GetNames(typeof(_Enums.DiscountContext));
        }
        protected void txtBannerClick_TextChanged(object sender, EventArgs e)
        {
            if (Wizard1.ActiveStep.ID == "stepNaming")
            {
                Literal lit = (Literal)FormView_Naming.FindControl("litUrlTest");
                if (lit != null)
                    lit.DataBind();
            }
        }
        protected void litUrlTest_DataBinding(object sender, EventArgs e)
        {
            if (Wizard1.ActiveStep.ID == "stepNaming")
            {
                Literal lit = (Literal)sender;
                lit.Text = string.Empty;

                //if the text box has text - create a url - else empty
                TextBox txt = (TextBox)FormView_Naming.FindControl("txtBannerClickUrl");
                if (txt != null)
                {
                    string input = txt.Text.Trim();

                    if (input.Length > 0)
                    {
                        if (Utils.Validation.IsValidUrl(input))
                            lit.Text = string.Format("<a target=\"_blank\" href=\"{0}\">test</a>", Utils.ParseHelper.FormatUrlFromString(input, true));
                        else
                            lit.Text = "invalid url";
                    }
                }
            }
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            //GridView1.DataBind();
        }
        #endregion

        #region GridView - selector

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

                    if (entity != null)
                    {
                        CheckBox chkValid = (CheckBox)e.Row.FindControl("chkValid");
                        if (chkValid != null)
                            chkValid.Checked = (entity.ValidityCheckList.Count == 0);

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

                        string start = (entity.DateStart == DateTime.MinValue) ? string.Empty : entity.DateStart.ToString("MM/dd/yyyy hh:mmtt");
                        string end = (entity.DateEnd == DateTime.MaxValue) ? string.Empty : entity.DateEnd.ToString("MM/dd/yyyy hh:mmtt");
                        litDates.Text = string.Format("{0}{1}", (start.Trim().Length > 0) ? string.Format("<div>start {0}</div>", start) : string.Empty,
                            (end.Trim().Length > 0) ? string.Format("<div>end {0}</div>", end) : string.Empty);
                    }
                }
            }
        }
        
        #endregion

        #region Shared

        protected static string highlightRow = "<tr class=\"highlightrow\">";
        protected void DisplayValidity()
        {
            //if the selected row is not valid - show the validation errors
            SalePromotion current = Atx.CurrentSalePromotion;
            if (current != null)
            {
                if (current.ValidityCheckList.Count > 0)
                {
                    litValid.Text = "<div class=\"hilit\" style=\"color:red;font-size:16px;font-weight:bold;\"><ul>The promotion does not meet the following criteria for validity.";

                    foreach (string s in current.ValidityCheckList)
                        litValid.Text += string.Format("<li>{0}</li>", s);

                    litValid.Text += "</ul></div>";
                }
                else
                    litValid.Text = "<div class=\"hilit\">This promotion meets validation requirements.</div>";
            }
            else
                litValid.Text = string.Empty;
        }
        protected void FormView_ItemUpdated(object sender, FormViewUpdatedEventArgs e)
        {
            if (e.AffectedRows > 0)
            {
                Atx.CurrentSalePromotion = null;
                GridView1.DataBind();
            }
        }
        protected void FormView_ModeChanging(object sender, FormViewModeEventArgs e)
        {
            //TODO: is thi s correct?
            //FormView form = FormView(sender);//should not we be doing this?
            FormView_Naming.ChangeMode(e.NewMode);
            
            if (e.CancelingEdit)//handles cancel correctly
                FormView_Naming.DataBind();
        }
        protected void ddlNumList_DataBinding(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            if (ddl.Items.Count == 0)
            {
                for (int i = 1; i <= 10; i++)
                    ddl.Items.Add(new ListItem(i.ToString()));
            }
        }
        protected void FormView_Naming_ModeChanging(object sender, FormViewModeEventArgs e)
        {
            //TODO: is this correct?
            //FormView form = FormView(sender);//should not we be doing this?
            FormView_Naming.ChangeMode(e.NewMode);

            if (e.CancelingEdit)//handles cancel correctly
                FormView_Naming.DataBind();
        }
        #endregion

        protected void EnsureCurrentObjects(object key)
        {
            if (key == null)
                Atx.CurrentSalePromotion = null;
            else
            {
                int idx = (int)key;
                if (Atx.CurrentSalePromotion == null || Atx.CurrentSalePromotion.Id != idx)
                    Atx.CurrentSalePromotion = SalePromotion.FetchByID(idx);
            }
        }

        #region Naming

        protected void FormView_Naming_ItemCreated(object sender, EventArgs e)
        {
            FormView form = (FormView)sender;
            FileUpload upload = (FileUpload)form.FindControl("FileUpload1");
            if (upload != null)
                upload.Attributes.Add("size", "55");

            ScriptManager mgr = (ScriptManager)this.Page.Master.FindControl("ScriptManager1");
            Button btnupload = (Button)form.FindControl("btnUpload");
            if (mgr != null && btnupload != null)
                mgr.RegisterPostBackControl(btnupload);
        }
        protected void FormView_Naming_DataBound(object sender, EventArgs e)
        {
            FormView form = (FormView)sender;

            EnsureCurrentObjects(form.SelectedValue);

            if (Atx.CurrentSalePromotion != null)
            {
                Button deleteBanner = (Button)form.FindControl("btnDeleteBanner");
                deleteBanner.Visible = Atx.CurrentSalePromotion.BannerUrl != null && Atx.CurrentSalePromotion.BannerUrl.Trim().Length > 0;
            }

            DisplayValidity();
        }
        protected void SqlNaming_Updating(object sender, SqlDataSourceCommandEventArgs e)
        {
            //Set start and end date
            WillCallWeb.Components.Util.CalendarClock start = (WillCallWeb.Components.Util.CalendarClock)FormView_Naming.FindControl("CalendarClockStart");
            WillCallWeb.Components.Util.CalendarClock end = (WillCallWeb.Components.Util.CalendarClock)FormView_Naming.FindControl("CalendarClockEnd");
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
        protected void FormView_Naming_ItemUpdating(object sender, FormViewUpdateEventArgs e)
        {
            //validate inputs
            _errors.Clear();
            string name = e.NewValues["Name"].ToString();
            string displayText = e.NewValues["DisplayText"].ToString();
            Utils.Validation.ValidateRequiredField(_errors, "Name", name);
            Utils.Validation.ValidateRequiredField(_errors, "DisplayText", displayText);

            if (_errors.Count > 0)
            {
                FormView form = (FormView)sender;
                CustomValidator validation = (CustomValidator)form.FindControl("CustomFormValidation");

                if (Utils.Validation.IncurredErrors(_errors, validation))
                {
                    e.Cancel = true;
                    return;
                }
            }
        }
        protected void FormView_Naming_ItemInserting(object sender, FormViewInsertEventArgs e)
        {
            e.Values["ApplicationId"] = _Config.APPLICATION_ID.ToString();

            //validate inputs
            _errors.Clear();
            string name = e.Values["Name"].ToString(); 
            string displayText = e.Values["DisplayText"].ToString();
            Utils.Validation.ValidateRequiredField(_errors, "Name", name);
            Utils.Validation.ValidateRequiredField(_errors, "DisplayText", displayText);

            if (_errors.Count > 0)
            {
                FormView form = (FormView)sender;
                CustomValidator validation = (CustomValidator)form.FindControl("CustomFormValidation");

                if (Utils.Validation.IncurredErrors(_errors, validation))
                {
                    e.Cancel = true;
                    return;
                }
            }
        }
        protected void SqlNaming_Inserting(object sender, SqlDataSourceCommandEventArgs e)
        {
            e.Command.Parameters["@iBannerTimeoutMsecs"].Value = _Config._BannerDisplayTime;
        }
        protected void FormView_Naming_ItemInserted(object sender, FormViewInsertedEventArgs e)
        {
            if (e.Exception != null)
            {
                FormView form = (FormView)sender;
                CustomValidator custom = (CustomValidator)form.FindControl("CustomFormValidation");
                
                if (custom != null)
                {
                    custom.IsValid = false;
                    custom.ErrorMessage = e.Exception.Message;
                    return;
                }
            }

            GooglePager1.OnGooglePagerChanged(0);

            GridView1.SelectedIndex = 0;
            GridView1.DataBind();
        }
        protected void FormView_Naming_ItemCommand(object sender, FormViewCommandEventArgs e)
        {
            FormView form = (FormView)sender;

            string cmd = e.CommandName.ToLower();

            switch (cmd)
            {
                case "upload":
                    FileUpload upload = (FileUpload)form.FindControl("FileUpload1");
                    CustomValidator custom = (CustomValidator)form.FindControl("CustomFormValidation");
                    PerformImageUpload(upload, custom);
                    int idxA = Atx.CurrentSalePromotion.Id;
                    Atx.CurrentSalePromotion = null;
                    EnsureCurrentObjects(idxA);
                    GridView1.DataBind();
                    Atx.UnblockUI(this.Wizard1, "#srceditor");
                    break;
                case "clearcode":
                case "generatecode":

                    int idx = int.Parse(e.CommandArgument.ToString());

                    string qry = "UPDATE SalePromotion SET UnlockCode = @code WHERE [ID] = @idx ";

                    if (form.ID.ToLower() == "formview_trigger")
                    {
                        //ensure that we do not have other triggers set
                        SalePromotion promo = new SalePromotion(idx);
                        if (promo == null)
                        {
                            CustomValidator validation = (CustomValidator)form.FindControl("CustomFormValidation");
                            if (validation != null)
                            {
                                validation.IsValid = false;
                                validation.ErrorMessage = "SalePromotion could not be located.";
                            }
                            return;
                        }

                        qry = "UPDATE SalePromotion SET RequiredPromotionCode = @code WHERE [ID] = @idx ";
                    }


                    SubSonic.QueryCommand com = new SubSonic.QueryCommand(qry, SubSonic.DataService.Provider.Name);
                    com.Parameters.Add("@idx", idx, DbType.Int32);

                    string newCode = null;
                    if (cmd == "generatecode")
                        newCode = Utils.ParseHelper.GenerateRandomPassword(10);

                    com.Parameters.Add("@code", newCode);
                    SubSonic.DataService.ExecuteQuery(com);

                    form.DataBind();

                    break;
                case "deletebanner":
                    if (Atx.CurrentSalePromotion.BannerUrl != null && Atx.CurrentSalePromotion.BannerUrl.Trim().Length > 0)
                    {
                        try
                        {
                            string mappedExisting = Server.MapPath(Atx.CurrentSalePromotion.Banner_VirtualFilePath);
                            if (File.Exists(mappedExisting))
                                File.Delete(mappedExisting);

                            //update object
                            Atx.CurrentSalePromotion.BannerUrl = null;
                            //save to db
                            SubSonic.QueryCommand command = new SubSonic.QueryCommand("UPDATE [SalePromotion] SET [BannerUrl] = NULL WHERE [Id] = @Id ",
                                SubSonic.DataService.Provider.Name);
                            command.Parameters.Add("@Id", Atx.CurrentSalePromotion.Id, DbType.Int32);
                            SubSonic.DataService.ExecuteQuery(command);

                            Button deleteBanner = (Button)FormView_Naming.FindControl("btnDeleteBanner");
                            if (deleteBanner != null)
                                deleteBanner.Visible = Atx.CurrentSalePromotion.BannerUrl != null && Atx.CurrentSalePromotion.BannerUrl.Trim().Length > 0;

                            GridView1.DataBind();
                        }
                        catch (Exception ex)
                        {
                            _Error.LogException(ex);
                            CustomValidator validation = (CustomValidator)form.FindControl("CustomFormValidation");
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

        #region Award

        protected void FormView_Award_ItemCommand(object sender, FormViewCommandEventArgs e)
        {
            FormView form = (FormView)sender;
            bool dirty = false;
            string sql = string.Empty;
            SubSonic.QueryCommand cmd = new SubSonic.QueryCommand(string.Empty, SubSonic.DataService.Provider.Name);
            DropDownList ddlAward = (DropDownList)form.FindControl("ddlAwardsMerch");
            ListBox box = (ListBox)form.FindControl("listAwards");
            _Enums.EventQVerb verb = _Enums.EventQVerb._Read;
            int selectedIdx = 0;
            string selectedMerch = string.Empty;

            switch (e.CommandName.ToLower())
            {
                case "addaward":
                    
                    //if we have an item selected that is not already in the list - then add
                    if (ddlAward != null && ddlAward.SelectedIndex > -1)
                    {
                        selectedMerch = ddlAward.SelectedItem.Text;
                        selectedIdx = int.Parse(ddlAward.SelectedValue);
                        int existing = Atx.CurrentSalePromotion.SalePromotionAwardRecords().GetList()
                            .FindIndex(delegate(SalePromotionAward match) { return match.TParentMerchId == selectedIdx; });

                        //this should not happen due to how we fill the list - but just in case....
                        if (existing != -1)
                            throw new Exception("This item is already in the award list and cannot be added.");

                        //otherwise - do sql to insert row
                        sql = "INSERT SalePromotionAward ([dtStamp],[bActive],[TSalePromotionId],[TParentMerchId]) ";
                        sql += "VALUES (((getDate())), @active, @promoId, @merchId)";

                        cmd.Parameters.Add("@active", true, DbType.Boolean);
                        cmd.Parameters.Add("@promoId", Atx.CurrentSalePromotion.Id, DbType.Int32);
                        cmd.Parameters.Add("@merchId", selectedIdx, DbType.Int32);
                        verb = _Enums.EventQVerb._Create;

                        dirty = true;
                    }
                    break;
                case "removeaward":                    

                    if (box != null && box.SelectedIndex != -1)
                    {
                        selectedMerch = box.SelectedItem.Text;
                        selectedIdx = int.Parse(box.SelectedValue);

                        sql = "DELETE FROM SalePromotionAward WHERE [Id] = @awardId ";

                        cmd.Parameters.Add("@awardId", selectedIdx, DbType.Int32);
                        verb = _Enums.EventQVerb._Delete;

                        dirty = true;
                    }
                    break;
            }

            if (dirty)
            {
                try
                {
                    cmd.CommandSql = sql;
                    SubSonic.DataService.ExecuteQuery(cmd);

                    try
                    {
                        DateTime now = DateTime.Now;

                        EventQ.Insert(now, now, _Enums.EventQStatus.Success.ToString(), null, null, 0, null,
                            this.Page.User.Identity.Name, null, this.Page.User.Identity.Name,
                            _Enums.EventQContext.SalePromotion.ToString(), verb.ToString(),
                            (Atx.CurrentSalePromotion != null) ? Atx.CurrentSalePromotion.Id.ToString() : "0", 
                            selectedMerch, "salePromotionId/MerchAward", 
                            Request.UserHostAddress, now, _Config.APPLICATION_ID);
                    }
                    catch (Exception ex)
                    {
                        _Error.LogException(ex);
                    }

                    //bind the form - does that rebind these lists?
                    //form.DataBind();
                    //no need to refresh entire form - just ddl and list
                    ddlAward.DataBind();
                    box.DataBind();

                    ddlAward.SelectedIndex = -1;
                    box.SelectedIndex = -1;
                }
                catch (Exception ex)
                {   
                    _Error.LogException(ex);

                    CustomValidator validation = (CustomValidator)form.FindControl("CustomFormValidation");
                    if (validation != null)
                    {
                        validation.IsValid = false;
                        validation.ErrorMessage = ex.Message;
                    }
                }                
            }
        }
        protected void FormView_Award_DataBound(object sender, EventArgs e)
        {
            FormView form = (FormView)sender;

            EnsureCurrentObjects(form.SelectedValue);

            if (form.DataItem != null)
            {
                DataRowView drv = (DataRowView)form.DataItem;
                DataRow row = drv.Row;

                Literal litMerch = (Literal)form.FindControl("litMerchTR");
                Literal litMerch2 = (Literal)form.FindControl("litMerchTR2");
                Literal litTicket = (Literal)form.FindControl("litTicketTR");
                Literal litShip = (Literal)form.FindControl("litShipTR");
                Literal litDiscount = (Literal)form.FindControl("litDiscountTR");
                //Literal litDiscFlat = (Literal)form.FindControl("litDiscFlatTR");
                Literal litDiscApp = (Literal)form.FindControl("litDiscountAppTR");
                Literal litPercent = (Literal)form.FindControl("litPercentTR");
                Literal litMax = (Literal)form.FindControl("litMaxTR");

                if (row["tShowTicketId"].ToString().Trim().Length > 0)
                    litTicket.Text = highlightRow;
                if (row["ShipOfferMethod"].ToString().Trim().Length > 0)
                    litShip.Text = highlightRow;
                if (row["mDiscountAmount"].ToString().Trim().Length > 0 && decimal.Parse(row["mDiscountAmount"].ToString()) > 0)
                    litDiscount.Text = highlightRow;
                if (row["iDiscountPercent"].ToString().Trim().Length > 0 && int.Parse(row["iDiscountPercent"].ToString()) > 0)
                    litPercent.Text = highlightRow;
                //if (row["mDiscountToFlatFee"].ToString().Trim().Length > 0 && decimal.Parse(row["mDiscountToFlatFee"].ToString()) > 0)
                //    litDiscFlat.Text = highlightRow;
                if (row["mMaxValue"].ToString().Trim().Length > 0 && decimal.Parse(row["mMaxValue"].ToString()) > 0)
                    litMax.Text = highlightRow;

                if(litPercent.Text == highlightRow)
                    litDiscApp.Text = highlightRow;

                //bind the checkbox
                CheckBoxList chk = (CheckBoxList)form.FindControl("chkDiscApp");
                if (chk != null)
                {
                    chk.SelectedIndex = -1;
                    string discountCtx = row["vcDiscountContext"].ToString().Trim();
                    List<_Enums.DiscountContext> list = SalePromotion.TransformDiscountContextFromString(discountCtx);
                    
                    foreach (_Enums.DiscountContext app in list)
                    {
                        ListItem li = chk.Items.FindByText(app.ToString());
                        if (li != null) 
                            li.Selected = true;
                    }
                }

                ListBox box = (ListBox)form.FindControl("listAwards");
                if (box != null)
                {
                    box.DataBind();
                    if (box.Items.Count > 0)
                    {
                        litMerch.Text = highlightRow;
                        litMerch2.Text = highlightRow;
                    }
                }
            }

            DisplayValidity();
        }
        protected void FormView_Award_ItemUpdating(object sender, FormViewUpdateEventArgs e)
        {
            //****NOTE that some business rules regarding amount vs percent are in the datasource
            FormView form = (FormView)sender;

            //validate inputs
            _errors.Clear();

            //hidden values are more recent!!
            DropDownList ddlTicket = (DropDownList)form.FindControl("ddlTicket");
            DropDownList ddlShip = (DropDownList)form.FindControl("ddlShipping");
            HiddenField hdnTicket = (HiddenField)form.FindControl("hdnTicket");
            HiddenField hdnShipping = (HiddenField)form.FindControl("hdnShipping");

            DropDownList ddlMax = (DropDownList)form.FindControl("ddlMaxPerOrder");
            CheckBoxList chkApp = (CheckBoxList)form.FindControl("chkDiscApp");
            RadioButtonList rdoContext = (RadioButtonList)form.FindControl("rdoDiscountContext");
            TextBox txtAmount = (TextBox)form.FindControl("txtDiscountAmount");
            TextBox txtPercent = (TextBox)form.FindControl("txtDiscountPercent");

            int awardCount = 0;

            if (ddlTicket.SelectedIndex > 0 || hdnTicket.Value.Trim().Length > 0)
            {
                string sel = (hdnTicket.Value.Trim().Length > 0) ? hdnTicket.Value.Trim() : ddlTicket.SelectedValue.Trim();
                e.NewValues["tShowTicketId"] = sel;
                awardCount++;
            }

            //****NOTE that some business rules regarding amount vs percent are in the datasource
            //Ensure they have not Selected both An amount and a percent
            decimal dscAmount = 0;
            if (txtAmount.Text.Trim().Length > 0)
                decimal.TryParse(txtAmount.Text.Trim(), out dscAmount);

            decimal dscPercent = 0;
            if (txtPercent.Text.Trim().Length > 0)
                decimal.TryParse(txtPercent.Text.Trim(), out dscPercent);

            if (dscAmount > 0)
                awardCount++;
            if(dscPercent > 0)
                awardCount++;

            string apps = string.Empty;
            if (chkApp != null)
            {
                foreach (ListItem li in chkApp.Items)
                    if (li.Selected)
                        apps += string.Format("{0},", li.Text);
            }
            //triggeritemonly needs to be the only selection
            apps = apps.TrimEnd(',');
            if (apps.ToLower().IndexOf(_Enums.DiscountContext.triggeritemonly.ToString()) != -1 &&
                apps.IndexOf(',') != -1)
                _errors.Add("When selecting triggeritemonly - you may not select any other options");


            e.NewValues["vcDiscountContext"] = apps.TrimEnd(',');
            
            string select = (hdnShipping.Value.Trim().Length > 0) ? hdnShipping.Value.Trim() : ddlShip.SelectedValue.Trim();
            //ensure a ship method for promos with shipping
            if (e.NewValues["vcDiscountContext"].ToString().IndexOf("shipping") != -1 && select.Trim().Length == 0)
                select = "all";
            e.NewValues["ShipOfferMethod"] = select;

            if (awardCount > 1)
                _errors.Add("You may only use one award per promotion. Please also note that you may only discount by amount OR percent. You cannot use both discount methods.");

            //validate other inputs
            //weight,price, discountamount, maxvalue must be a decimal
            //discountpercent is an integer
            string weight = e.NewValues["mWeight"].ToString();
            string price = e.NewValues["mPrice"].ToString();
            string discountAmount = e.NewValues["mDiscountAmount"].ToString();
            string discountPercent = e.NewValues["iDiscountPercent"].ToString();
            string maxValue = e.NewValues["mMaxValue"].ToString();
            Utils.Validation.ValidateNumericField(_errors, "Weight", weight);
            Utils.Validation.ValidateNumericField(_errors, "Price", price);
            Utils.Validation.ValidateNumericField(_errors, "Discount Amount", discountAmount);
            Utils.Validation.ValidateIntegerField(_errors, "Discount Percent", discountPercent);
            Utils.Validation.ValidateNumericField(_errors, "Max Value", maxValue);

            if (_errors.Count > 0)
            {
                CustomValidator validation = (CustomValidator)form.FindControl("CustomFormValidation");

                if (Utils.Validation.IncurredErrors(_errors, validation))
                {
                    e.Cancel = true;
                    return;
                }
            }

            e.NewValues["iMaxPerOrder"] = ddlMax.SelectedValue;
            
            //****NOTE that some business rules regarding amount vs percent are in the datasource

            //ddlMerch.SelectedIndex = -1;
            hdnTicket.Value = "";
            //ddlTicket.SelectedIndex = -1;
            hdnShipping.Value = "";
            //ddlShip.SelectedIndex = -1;
        }
        protected void ddlMaxPerOrder_DataBound(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;

            if (ddl.Items.Count > 0)
            {
                //reset - cant have mult selections
                if (ddl.SelectedIndex != -1) ddl.SelectedIndex = -1;

                if (FormView_Award.DataItem != null)
                {
                    DataRowView drv = (DataRowView)FormView_Award.DataItem;
                    DataRow row = drv.Row;
                    string max = row["iMaxPerOrder"].ToString();

                    ListItem li = ddl.Items.FindByValue(max);
                    if (li != null)
                        li.Selected = true;
                }
            }
        }
        #endregion

        #region Trigger

        protected void ddlUses_DataBinding(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;

            //bind to parent items
            if (ddl.Items.Count <= 0)
            {
                int maxUses = 10;
                for (int i = 0; i <= maxUses; i++)//we ant the max use - so do less than or equal
                    ddl.Items.Add(new ListItem(i.ToString()));
            }
        }
        protected void listRequiredMerch_DataBinding(object sender, EventArgs e)
        {   
            ListBox list = (ListBox)sender;
            
            list.DataTextField = "DisplayNameWithAttribs";
            list.DataValueField = "Id";
            list.DataSource = Atx.CurrentSalePromotion.RequiredMerchItems(Ctx);
            if(((List<Merch>)list.DataSource).Count > 4)
                list.Rows = Atx.CurrentSalePromotion.RequiredMerchItems(Ctx).Count;
        }
        protected void listRequiredMerch_DataBound(object sender, EventArgs e)
        {
            ListBox list = (ListBox)sender;
        }
        protected void ddlReqShowDate_DataBinding(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;

            //bind to parent items
            if (ddl.Items.Count <= 0)
            {
                ListItem li = new ListItem("<-- Select a show date -->", "0");
                ddl.Items.Add(li);
                ddl.AppendDataBoundItems = true;

                //ShowDateCollection coll = new ShowDateCollection();
                //coll.AddRange(Atx.OrderedDisplayable_ShowDates);
                
                ShowDateCollection coll = new ShowDateCollection();

                System.Text.StringBuilder sb = new System.Text.StringBuilder();

                sb.Append("SELECT TOP 1000 * FROM ShowDate sd LEFT OUTER JOIN Show s ON sd.tShowId = s.Id ");
                sb.Append("WHERE sd.dtDateOfShow > @startDate AND s.ApplicationId = @appId ORDER BY sd.dtDateOfShow ");

                SubSonic.QueryCommand cmd = new SubSonic.QueryCommand(sb.ToString(), SubSonic.DataService.Provider.Name);
                cmd.Parameters.Add("@appId", _Config.APPLICATION_ID, System.Data.DbType.Guid);
                cmd.Parameters.Add("@startDate", DateTime.Now.AddDays(-4), System.Data.DbType.DateTime);

                coll.LoadAndCloseReader(SubSonic.DataService.GetReader(cmd));
                
                int req = (Atx.CurrentSalePromotion.TRequiredParentShowDateId.HasValue) ? Atx.CurrentSalePromotion.TRequiredParentShowDateId.Value : 0;
                if (req > 0)
                {
                    ShowDate selected = (ShowDate)coll.Find(req);

                    if (selected == null)
                    {
                        selected = ShowDate.FetchByID(req);
                        if(selected != null)
                            coll.Insert(0, selected);
                    }
                }

                ddl.DataSource = coll;
                ddl.DataTextField = "ListTitle";
                ddl.DataValueField = "Id";
            }
        }
        #region Event List

        protected void SqlEventList_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters["@appId"].Value = Wcss._Config.APPLICATION_ID;
            e.Command.Parameters["@date"].Value = DateTime.Now.AddDays(-4).ToString("MM/dd/yyyy");
        }

        #endregion
        protected void ddlReqTicket_DataBinding(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;

            //bind to parent items
            if (ddl.Items.Count <= 0)
            {
                ListItem li = new ListItem("<-- Select a ticket -->", "0");
                ddl.Items.Add(li);
                ddl.AppendDataBoundItems = true;

                ShowTicketCollection coll = Atx.SaleTickets;
                if (coll.Count > 0)
                    coll.Sort("DtDateOfShow", true);

                //find the existing choice in the current list - if not found - then load manually
                int req = (Atx.CurrentSalePromotion.TRequiredParentShowTicketId.HasValue) ? Atx.CurrentSalePromotion.TRequiredParentShowTicketId.Value : 0;
                if (req > 0)
                {
                    ShowTicket selected = (ShowTicket)coll.Find(req);

                    if (selected == null)
                    {
                        selected = ShowTicket.FetchByID(req);
                        coll.Insert(0, selected);
                    }
                }

                ddl.DataSource = coll;
                ddl.DataTextField = "DdlListing";
                ddl.DataValueField = "Id";
            }
        }
        protected void FormView_Trigger_DataBound(object sender, EventArgs e)
        {
            FormView form = (FormView)sender;

            EnsureCurrentObjects(form.SelectedValue);

            if (form.DataItem != null)
            {
                DataRowView drv = (DataRowView)form.DataItem;
                DataRow row = drv.Row;

                Literal litUse = (Literal)form.FindControl("litUseTR");
                Literal litCode = (Literal)form.FindControl("litCodeTR");
                Literal litMerch = (Literal)form.FindControl("litMerchTR");
                Literal litMerch2 = (Literal)form.FindControl("litMerchTR2"); 
                Literal litTicket = (Literal)form.FindControl("litTicketTR");
                Literal litShowDate = (Literal)form.FindControl("litShowDateTR");
                Literal litReqQty = (Literal)form.FindControl("litReqQtyTR");
                Literal litMinMerch = (Literal)form.FindControl("litMinMerchTR");
                Literal litMinTicket = (Literal)form.FindControl("litMinTicketTR");
                Literal litMinTotal = (Literal)form.FindControl("litMinTotalTR");

                int uses = (row["iMaxUsesPerUser"] != null && row["iMaxUsesPerUser"].ToString().Trim().Length > 0) ? (int)row["iMaxUsesPerUser"] : 0;
                DropDownList ddlUses = (DropDownList)form.FindControl("ddlUses");
                if (ddlUses != null)
                {   
                    ddlUses.DataBind();

                    if (uses.ToString() != ddlUses.SelectedValue)
                    {
                        ddlUses.SelectedIndex = -1;
                        ListItem li = ddlUses.Items.FindByValue(uses.ToString());
                        if (li != null)
                            li.Selected = true;
                    }
                }

                if(uses > 0)
                    litUse.Text = highlightRow;

                if (row["RequiredPromotionCode"] != null && row["RequiredPromotionCode"].ToString().Trim().Length > 0)
                    litCode.Text = highlightRow;

                if (row["vcTriggerList_Merch"].ToString().Trim().Length > 0)
                {
                    litMerch.Text = highlightRow;
                    litMerch2.Text = highlightRow;

                    DropDownList ddl = (DropDownList)form.FindControl("ddlReqParentMerch");
                    if (ddl != null && ddl.Items.Count > 0)
                    {
                        ddl.SelectedIndex = -1;
                        int idx = 0;
                        ListItem li = ddl.Items.FindByValue(idx.ToString());
                        if (li != null)
                            li.Selected = true;
                    }
                }
                
                if (int.Parse(row["iRequiredParentQty"].ToString()) > 1)
                    litReqQty.Text = highlightRow;

                bool isShowDateOrTicketTrigger = false;

                if (row["tRequiredParentShowDateId"].ToString().Trim().Length > 0)
                {
                    litShowDate.Text = highlightRow;

                    DropDownList ddl = (DropDownList)form.FindControl("ddlReqShowDate");
                    if (ddl != null && ddl.Items.Count > 0)
                    {
                        ddl.SelectedIndex = -1;
                        int idx = (int)row["tRequiredParentShowDateId"];
                        ListItem li = ddl.Items.FindByValue(idx.ToString());
                        if (li != null)
                            li.Selected = true;
                    }

                    isShowDateOrTicketTrigger = true;
                }
                if (row["tRequiredParentShowTicketId"].ToString().Trim().Length > 0)
                {
                    litTicket.Text = highlightRow;

                    DropDownList ddl = (DropDownList)form.FindControl("ddlReqTicket");
                    if (ddl != null && ddl.Items.Count > 0)
                    {
                        ddl.SelectedIndex = -1;
                        int idx = (int)row["tRequiredParentShowTicketId"];
                        ListItem li = ddl.Items.FindByValue(idx.ToString());
                        if (li != null)
                            li.Selected = true;
                    }

                    isShowDateOrTicketTrigger = true;
                }

                Button btnMerchSelect = (Button)form.FindControl("btnMerchSelect");
                if (btnMerchSelect != null)
                    btnMerchSelect.Enabled = (!isShowDateOrTicketTrigger);

                if (row["mMinMerch"].ToString().Trim().Length > 0 && decimal.Parse(row["mMinMerch"].ToString()) > 0)
                    litMinMerch.Text = highlightRow;
                if (row["mMinTicket"].ToString().Trim().Length > 0 && decimal.Parse(row["mMinTicket"].ToString()) > 0)
                    litMinTicket.Text = highlightRow;
                if (row["mMinTotal"].ToString().Trim().Length > 0 && decimal.Parse(row["mMinTotal"].ToString()) > 0)
                    litMinTotal.Text = highlightRow;                    
            }

            DisplayValidity();
        }

        #region Tiered GCs

        protected void gridTiers_DataBinding(object sender, EventArgs e)
        {
            GridView ctl = (GridView)sender;
            Wizard wiz = (Wizard)ctl.NamingContainer.NamingContainer;

            ctl.Visible = false;            

            if (Atx.CurrentSalePromotion != null && Atx.CurrentSalePromotion.IsGiftCertificatePromotion && 
                wiz != null && wiz.ActiveStep.ID.ToLower() == "steptrigger")
            {
                ctl.Visible = true;
                ctl.DataSource = Atx.CurrentSalePromotion.Meta.TieredRewards;
            }            
        }
        protected void gridTiers_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridView ctl = (GridView)sender;

            if (Atx.CurrentSalePromotion != null)
            {
                Button cmd = (Button)e.Row.FindControl("lnkCommand");
                Button cancel = (Button)e.Row.FindControl("lnkCancel");

                if (e.Row.RowType == DataControlRowType.Footer || e.Row.RowType == DataControlRowType.EmptyDataRow)
                {
                    TextBox min = (TextBox)e.Row.FindControl("txtMin");
                    DropDownList reward = (DropDownList)e.Row.FindControl("ddlTierReward");                    

                    if (min != null && reward != null && cmd != null && cancel != null)
                    {
                        min.Visible = reward.Visible = cancel.Visible = cmd.Visible =
                            (Atx.CurrentSalePromotion.IsGiftCertificatePromotion && ctl.EditIndex == -1);
                    }
                }
            }
        }
        protected void gridTiers_DataBound(object sender, EventArgs e)
        {
            GridView ctl = (GridView)sender;

        }
        
        protected void gridTiers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridView ctl = (GridView)sender;
            string cmd = e.CommandName.ToLower();
            string args = (e.CommandArgument != null) ? e.CommandArgument.ToString().Trim() : string.Empty;
            
            switch(cmd)
            {
                case "new":
                    ctl.EditIndex = -1;
                    break;
                
                case "insert":
                    TextBox min;
                    DropDownList u_reward;
                    GridViewRow row = (GridViewRow)((Button)e.CommandSource).NamingContainer;

                    if (row != null)
                    {
                        min = (TextBox)row.FindControl("txtMin");
                        u_reward = (DropDownList)row.FindControl("ddlTierReward");

                        if (Atx.CurrentSalePromotion != null && min != null && u_reward != null)
                        {
                            try
                            {
                                Atx.CurrentSalePromotion.JsonMeta = Atx.CurrentSalePromotion.Meta
                                    .AddUpdateTieredReward(null, min.Text.Trim(), u_reward.SelectedValue, cmd);
                                Atx.CurrentSalePromotion.Save();
                            }
                            catch (Exception ex)
                            {
                                CustomValidator validation = (CustomValidator)ctl.NamingContainer.FindControl("CustomFormValidation");
                                _errors.Add(ex.Message);

                                Utils.Validation.IncurredErrors(_errors, validation);
                            }                            
                        }
                    }
                break;

                case "delete":
                case "update":
                case "edit":
                case "cancel":
                    return;             
            }

            ctl.DataBind();
        }
        
        protected void gridTiers_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridView ctl = (GridView)sender;
            int idx = e.RowIndex;

            try
            {
                string value = Atx.CurrentSalePromotion.Meta.TieredRewards[idx].MinAmount.ToString();
                Atx.CurrentSalePromotion.JsonMeta = Atx.CurrentSalePromotion.Meta.DeleteTieredReward(value);
                Atx.CurrentSalePromotion.Save();

                ctl.DataBind();
            }
            catch (Exception ex)
            {
                CustomValidator validation = (CustomValidator)ctl.NamingContainer.FindControl("CustomFormValidation");
                _errors.Add(ex.Message);

                if (Utils.Validation.IncurredErrors(_errors, validation))
                {
                    e.Cancel = true;
                }
            }
        }
        protected void gridTiers_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView ctl = (GridView)sender;
            ctl.EditIndex = e.NewEditIndex;
            ctl.DataBind();
        }
        protected void gridTiers_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView ctl = (GridView)sender;
            ctl.EditIndex = -1;
            ctl.DataBind();
        }
        protected void gridTiers_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridView ctl = (GridView)sender;
            //get old value
            int idx = e.RowIndex;
            string oldAmount = Atx.CurrentSalePromotion.Meta.TieredRewards[idx].MinAmount.ToString();            
            TextBox min = (TextBox)ctl.Rows[idx].FindControl("txtMin");
            DropDownList u_reward = (DropDownList)ctl.Rows[idx].FindControl("ddlTierReward");

            try
            {
                Atx.CurrentSalePromotion.JsonMeta = Atx.CurrentSalePromotion.Meta
                    .AddUpdateTieredReward(oldAmount, min.Text.Trim(), u_reward.SelectedValue, "update");
                Atx.CurrentSalePromotion.Save();
                ctl.EditIndex = -1;
                ctl.DataBind();
            }
            catch (Exception ex)
            {
                CustomValidator validation = (CustomValidator)ctl.NamingContainer.FindControl("CustomFormValidation");
                _errors.Add(ex.Message);

                if (Utils.Validation.IncurredErrors(_errors, validation))
                {
                    e.Cancel = true;
                }
            }
        }

        protected void ddlTierReward_DataBinding(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            GridView ctl = (GridView)ddl.NamingContainer.NamingContainer;//gridviewrow -> gridview

            if (Atx.CurrentSalePromotion != null && Atx.CurrentSalePromotion.IsGiftCertificatePromotion)
            {
                SalePromotionAwardCollection coll = new SalePromotionAwardCollection();
                coll.AddRange(Atx.CurrentSalePromotion.SalePromotionAwardRecords().GetList()
                    .FindAll(delegate(SalePromotionAward match)
                {
                    return (match.MerchRecord_Parent.IsActive &&
                        match.MerchRecord_Parent.IsGiftCertificateDelivery);
                }));

                List<Merch> list = new List<Merch>();
                foreach (SalePromotionAward s in coll)
                    list.AddRange(s.MerchRecord_Parent.ChildMerchRecords_Active);

                if (list.Count > 1)
                    list.Sort(delegate(Merch x, Merch y) { return (x.Price.CompareTo(y.Price_Effective)); });

                ddl.DataSource = list;
                ddl.DataTextField = "DisplayNameWithAttribs";
                ddl.DataValueField = "Price";
            }
        }
        
        #endregion


        protected void SqlTrigger_Updating(object sender, SqlDataSourceCommandEventArgs e)
        {
        }
        protected void FormView_Trigger_ItemUpdating(object sender, FormViewUpdateEventArgs e)
        {
            FormView form = (FormView)sender;
            _errors.Clear();
            int numTriggers = 0;

            TextBox txtPromoCode = (TextBox)form.FindControl("txtPromoCode");
            ListBox listMerch = (ListBox)form.FindControl("listRequiredMerch");
            DropDownList ddlReqTicket = (DropDownList)form.FindControl("ddlReqTicket");
            DropDownList ddlReqShowDate = (DropDownList)form.FindControl("ddlReqShowDate");
            DropDownList ddlUses = (DropDownList)form.FindControl("ddlUses");
            HiddenField hdnTicket = (HiddenField)form.FindControl("hdnReqTicket");
            HiddenField hdnShowDate = (HiddenField)form.FindControl("hdnReqShowDate");
            DropDownList ddlReqQty = (DropDownList)form.FindControl("ddlRequiredQty");

            if(ddlUses != null)//this is not a trigger
            {
                int sel = int.Parse(ddlUses.SelectedValue.Trim());
                e.NewValues["iMaxUsesPerUser"] = sel;
            }
            if (txtPromoCode != null && txtPromoCode.Text.Trim().Length > 0)
                numTriggers++;
            
            if (listMerch.Items.Count > 0)
            {
            //    //string sel = ddlMerch.SelectedValue.Trim();
            //    //e.NewValues["vcTriggerList_Merch"] = sel;
                numTriggers++;
            }
            if (ddlReqTicket.SelectedIndex > 0)// || hdnTicket.Value.Trim().Length > 0)
            {
                //string sel = (hdnTicket.Value.Trim().Length > 0) ? hdnTicket.Value.Trim() : ddlTicket.SelectedValue.Trim();
                string sel = ddlReqTicket.SelectedValue.Trim();
                e.NewValues["tRequiredParentShowTicketId"] = sel;
                numTriggers++;
            }
            if (ddlReqShowDate.SelectedIndex > 0)// || hdnShowDate.Value.Trim().Length > 0)
            {
                //string sel = (hdnShowDate.Value.Trim().Length > 0) ? hdnShowDate.Value.Trim() : ddlShowDate.SelectedValue.Trim();
                string sel = ddlReqShowDate.SelectedValue.Trim();
                e.NewValues["tRequiredParentShowDateId"] = sel;
                numTriggers++;
            }

            e.NewValues["iRequiredParentQty"] = ddlReqQty.SelectedValue;

            //right nopw we only allow one trigger
            if (numTriggers > 1)
                _errors.Add("You may only choose one trigger for the promotion. Either ticket, merch or discount(includes shipping discounts).");

            //validate other inputs
            //weight,price, discountamount, maxvalue must be a decimal
            //discountpercent is an integer
            string minMerch = e.NewValues["mMinMerch"].ToString();
            string minTicket = e.NewValues["mMinTicket"].ToString();
            string minTotal = e.NewValues["mMinTotal"].ToString();
            Utils.Validation.ValidateNumericField(_errors, "Minimum Merch Purchase", minMerch);
            Utils.Validation.ValidateNumericField(_errors, "Minimum Ticket Purchase", minTicket);
            Utils.Validation.ValidateNumericField(_errors, "Minimum Total Purchase", minTotal);

            if (_errors.Count > 0)
            {
                CustomValidator validation = (CustomValidator)form.FindControl("CustomFormValidation");

                if (Utils.Validation.IncurredErrors(_errors, validation))
                {
                    e.Cancel = true;
                    return;
                }
            }
        }
        protected void ddlRequiredQty_DataBound(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;

            if (ddl.Items.Count > 0)
            {
                //reset - cant have mult selections
                if (ddl.SelectedIndex != -1) ddl.SelectedIndex = -1;

                if (FormView_Trigger.DataItem != null)
                {
                    DataRowView drv = (DataRowView)FormView_Trigger.DataItem;
                    DataRow row = drv.Row;
                    string qty = row["iRequiredParentQty"].ToString();

                    ListItem li = ddl.Items.FindByValue(qty);
                    if (li != null)
                        li.Selected = true;
                }
            }
        }

        #endregion

        #region Upload

        protected void PerformImageUpload(FileUpload upload, CustomValidator custom)
        {
            if (upload != null && upload.HasFile)
            {   
                string mappedFile = string.Empty;

                try
                {
                    SalePromotion Entity = Atx.CurrentSalePromotion;

                    //validate file name
                    string uploadExt = Path.GetExtension(upload.FileName).ToLower();

                    if (uploadExt.Trim().Length == 0 || (uploadExt != ".jpg" && uploadExt != ".jpeg" && uploadExt != ".gif" && uploadExt != ".png"))
                        throw new Exception("Valid file types are jpg, jpeg, gif and png only.");

                    string fileName = System.Text.RegularExpressions.Regex.Replace(Path.GetFileNameWithoutExtension(upload.FileName), @"\s+", string.Empty);
                    fileName = fileName.Replace("'", string.Empty).Replace("-", "_").Replace("&", "_");
                    //get the file name to save
                    fileName += uploadExt;

                    if (!Utils.Validation.IsValidImageFile(fileName))
                        throw new Exception("Please enter a valid file name. Valid filenames use letters, underscores and periods. Only jpg, jpeg, gif or png are valid");
                    //endvalidation

                    string pathFile = string.Format("{0}{1}", SalePromotion.Banner_VirtualDirectory, fileName);
                    mappedFile = Server.MapPath(pathFile);

                    if (System.IO.File.Exists(mappedFile))
                    {
                        fileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + uploadExt;
                        mappedFile = Server.MapPath(string.Format("{0}{1}", SalePromotion.Banner_VirtualDirectory, fileName));
                    }

                    //delete any existing images
                    if (Entity.BannerUrl != null && Entity.BannerUrl.Trim().Length > 0)
                    {
                        string mappedExisting = Server.MapPath(Atx.CurrentSalePromotion.Banner_VirtualFilePath);
                        if (File.Exists(mappedExisting))
                            File.Delete(mappedExisting);

                        Entity.BannerUrl = null;
                    }    

                    //save the new file
                    upload.SaveAs(mappedFile);

                    //assign new image to entity
                    Entity.BannerUrl = fileName;

                    //save the entity
                    Entity.Save();

                    //Old method
                    //SubSonic.QueryCommand cmd = new SubSonic.QueryCommand("UPDATE [SalePromotion] SET [BannerUrl] = @ImageName WHERE [Id] = @Id ",
                    //    SubSonic.DataService.Provider.Name);
                    //cmd.Parameters.Add("@ImageName", uploadName);
                    //cmd.Parameters.Add("@Id", Atx.CurrentSalePromotion.Id, DbType.Int32);
                    //SubSonic.DataService.ExecuteQuery(cmd);

                    ////create thumbs
                    //Entity.ImageManager.CreateAllThumbs();

                    //redirect back to editor   
                    //this.wizEdit.ActiveStepIndex = 0;
                    //this.wizEdit.ActiveStepIndex = 1;

                    
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
                    
                    if (custom != null)
                    {
                        custom.IsValid = false;
                        custom.ErrorMessage = ex.Message;
                    }
                }
            }
        }

        #endregion

        #region Set select params
        protected void SqlAward_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters["@appId"].Value = Wcss._Config.APPLICATION_ID;
        }
        protected void SqlTrigger_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters["@appId"].Value = Wcss._Config.APPLICATION_ID;
        }
        #endregion
        
}
}
