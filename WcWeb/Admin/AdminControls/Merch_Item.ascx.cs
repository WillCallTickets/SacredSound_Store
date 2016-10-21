using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using Wcss;

namespace WillCallWeb.Admin.AdminControls
{
    //<iframe id="productdescription" frameborder="0" scrolling="auto" src="/Admin/Display_Merch.aspx"></iframe>

    public partial class Merch_Item : BaseControl, IPostBackEventHandler
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
                    break;
            }
        }


        List<string> _errors = new List<string>();

        #region Page Overhead

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            //load up insert mode if we have an id of 0
            if (Atx.CurrentMerchRecord == null)
                FormView1.ChangeMode(FormViewMode.Insert);

            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), 
            //    "<script type=\"text/javascript\" src=\"/JQueryUI/admin-overlay.js\"></script>", false);

            Merch_Inventory1.ParentForm = this.FormView1;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FormView1.DataBind();
            }

            btnSync.Visible = (Atx.CurrentMerchRecord != null && this.Page.User.IsInRole("Super"));
        }
        
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            
            //Atx.Register_CkFinder(this, FormView1, "Ck_Edit");

        }

        #endregion

        protected void txtSalePrice_DataBinding(object sender, EventArgs e)
        {
            TextBox txt = (TextBox)sender;
            decimal dec = 0;
            if (decimal.TryParse(txt.Text.Trim(), out dec))
            {
                if (dec == 0)
                    txt.Text = string.Empty;
            }
        }
        
        protected void ddlDeliveryType_DataBound(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;

            if (FormView1.CurrentMode != FormViewMode.Insert)
            {
                Merch entity = Atx.CurrentMerchRecord;

                foreach (ListItem li in ddl.Items)
                {
                    if (entity != null && li.Value.ToLower() == entity.DeliveryType.ToString().ToLower())
                    {
                        li.Selected = true;
                        break;
                    }
                    else if (entity == null && li.Value.ToLower() == _Config.DeliveryTypeDefault.ToString().ToLower())
                    {
                        li.Selected = true;
                        break;
                    }
                }
            }

            if (ddl.SelectedIndex == -1)
                ddl.SelectedIndex = 0;
        }
        protected void ddlTemplate_DataBound(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;

            Merch entity = Atx.CurrentMerchRecord;

            foreach (ListItem li in ddl.Items)
            {
                if (entity != null && li.Value.ToLower() == entity.DisplayTemplate.ToString().ToLower())
                {
                    li.Selected = true;
                    break;
                }
                //else if (entity == null && li.Value.ToLower() == _Config.DeliveryTypeDefault.ToString().ToLower())
                //{
                //    li.Selected = true;
                //    break;
                //}
            }

            if (ddl.SelectedIndex == -1)
                ddl.SelectedIndex = 0;
        }

        #region FormView1

        protected void btnSync_Click(object sender, EventArgs e)
        {
            if (FormView1.CurrentMode == FormViewMode.Insert)
                FormView1.InsertItem(false);
            else
                FormView1.UpdateItem(false);

            //get the current id
            if (Atx.CurrentMerchRecord != null && Atx.CurrentMerchRecord.IsParent)
            {
                MerchCollection coll = new MerchCollection();
                coll.AddRange(Atx.CurrentMerchRecord.ChildMerchRecords());

                foreach (Merch child in coll)
                {
                    if (child.IsActivationCodeDelivery)
                    {
                        //update activation coded inventory - allotment
                        string sql = "SELECT COUNT(*) FROM [Inventory] WHERE [iParentInventoryId] = @itemIdx AND [tInvoiceItemId] IS NULL ";
                        _DatabaseCommandHelper cmd = new _DatabaseCommandHelper(sql);
                        cmd.AddCmdParameter("itemIdx", child.Id, System.Data.DbType.Int32);
                        int availables = cmd.PerformQuery("SyncSoldActivationCodeAllotmentCheck");

                        if (availables != child.Available)
                        {
                            //mark inventory change
                            int newAllotment = child.Sold + child.Damaged + availables;
                            string sql2 = "UPDATE [Merch] SET [iAllotment] = @newAllotment WHERE [Id] = @itemIdx; SELECT 0 ";
                            _DatabaseCommandHelper cmd2 = new _DatabaseCommandHelper(sql2);
                            cmd2.AddCmdParameter("newAllotment", newAllotment, System.Data.DbType.Int32);
                            cmd2.AddCmdParameter("itemIdx", child.Id, System.Data.DbType.Int32);

                            cmd2.PerformQuery("SyncSoldUpdateMerchAllotment");

                            Inventory.AdjustInventoryHistory(
                                child,
                                child.Allotment,
                                newAllotment - child.Allotment,
                                System.Web.HttpContext.Current.Profile.UserName,
                                _Enums.HistoryInventoryContext.Allotment);
                        }
                    }

                    SPs.TxInventorySyncSold(child.Id, true, _Enums.InvoiceItemContext.merch.ToString()).Execute();
                }
            }

            //reset the current merch record - rebind page
            int index = Atx.CurrentMerchRecord.Id;
            Atx.Clear_CurrentMerchListing();
            Atx.SetCurrentMerchRecord(index);
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (FormView1.CurrentMode == FormViewMode.Insert)
                FormView1.InsertItem(false);
            else
                FormView1.UpdateItem(false);
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            FormView1.ChangeMode(FormViewMode.Edit);
            FormView1.DataBind();
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            Merch_Inventory1.InventoryListing.InsertItemPosition = InsertItemPosition.None;

            FormView1.DeleteItem();
            FormView1.DataBind();
        }
        protected void btnNew_Click(object sender, EventArgs e)
        {
            Merch_Inventory1.InventoryListing.InsertItemPosition = InsertItemPosition.None;
            FormView1.ChangeMode(FormViewMode.Insert);
            //FormAddChild.DataBind();
            Merch_Inventory1.Visible = false;
        }
        protected void FormView1_ModeChanging(object sender, FormViewModeEventArgs e)
        {
            FormView form = (FormView)sender;
            form.ChangeMode(e.NewMode);
            if (e.CancelingEdit)
                form.DataBind();

            if (e.NewMode == FormViewMode.Insert)
                Merch_Inventory1.Visible = false;

            if (Merch_Inventory1.InventoryListing.InsertItemPosition != InsertItemPosition.None)
                Merch_Inventory1.InventoryListing.InsertItemPosition = InsertItemPosition.None;
        }
        protected void FormView1_DataBinding(object sender, EventArgs e)
        {   
            FormView form = (FormView)sender;

            bool editMode = (form.CurrentMode == FormViewMode.Edit);
            btnSave.CommandName =  editMode ? "Update" : "Insert";
            //cancel always the same
            btnDelete.Visible = editMode;
            if (Atx.CurrentMerchRecord != null)
                btnDelete.OnClientClick = string.Format("return confirm('Are you sure you want to delete {0}?')", 
                    Utils.ParseHelper.ParseJsAlert(Atx.CurrentMerchRecord.DisplayNameWithAttribs));

            btnNew.Visible = editMode;

            MerchCollection coll = new MerchCollection();
            if (Atx.CurrentMerchRecord != null)
                coll.Add(Atx.CurrentMerchRecord);

            form.DataSource = coll;
        }
        protected void FormView1_ItemCreated(object sender, EventArgs e)
        {
            FormView form = (FormView)sender;

            if (form.CurrentMode == FormViewMode.Insert)
            {
                DropDownList ddlDeliveryType = (DropDownList)form.FindControl("ddlDeliveryType");
                if (ddlDeliveryType != null)
                    ddlDeliveryType.SelectedIndex = -1;
            }
        }
        protected void FormView1_DataBound(object sender, EventArgs e)
        {
            FormView form = (FormView)sender;
            Merch entity = (Merch)form.DataItem;

            DropDownList categories = (DropDownList)form.FindControl("ddlCategories");
            if (categories != null && categories.Items.Count == 1)
            {
                categories.AppendDataBoundItems = true;

                MerchCategorieCollection coll = new MerchCategorieCollection();
                coll.AddRange(_Lookits.MerchCategories);
                if (coll.Count > 1)
                    coll.Sort("Name", true);

                categories.DataSource = coll;
                categories.DataTextField = "Name";
                categories.DataValueField = "Id";
                categories.DataBind();
            }

            ListBox joinCats = (ListBox)form.FindControl("listCategories");
            if (joinCats != null && joinCats.Items.Count == 0)
            {
                joinCats.DataSource = entity.MerchJoinCatRecords();
                joinCats.DataTextField = "MerchCategorieName";
                joinCats.DataValueField = "Id";
                joinCats.DataBind();
            }

            //delete category
            Button deleteCat = (Button)form.FindControl("linkDeleteCat");
            if (deleteCat != null)
                deleteCat.OnClientClick = string.Format("return confirm('Are you sure you want to delete {0}?')", 
                    Utils.ParseHelper.ParseJsAlert("the chosen category"));

            //command buttons
            Button delete = (Button)form.FindControl("btnDelete");
            if (delete != null && entity != null)
                delete.OnClientClick = string.Format("return confirm('Are you sure you want to delete {0}?')", 
                    Utils.ParseHelper.ParseJsAlert(entity.Name));
            
            btnSync.Visible = (Atx.CurrentMerchRecord != null && form.CurrentMode != FormViewMode.Insert);

            if (entity != null)
            {
                Button btnWys = (Button)form.FindControl("btnWys");
                if (btnWys != null)
                    btnWys.ToolTip = string.Format("/Admin/AdminControls/Wysiwyg/Wysiwyg.aspx?context=m&ctrl={0}", this.UniqueID);

                Literal litDesc = (Literal)form.FindControl("litDesc");
                if(litDesc != null)
                    litDesc.Text = string.Format("<div class=\"desc-control\">{0}</div>", entity.Description_Derived);

                ((WebControl)form.FindControl("chkLowRate")).Enabled = (entity.DeliveryType == _Enums.DeliveryType.parcel);

                //disable controls not necessary for GCs
                if (entity.DeliveryType == _Enums.DeliveryType.giftcertificate)
                {
                    ((WebControl)form.FindControl("txtPrice")).Enabled = false;
                    ((WebControl)form.FindControl("chkUseSalePrice")).Enabled = false;
                    ((WebControl)form.FindControl("txtSalePrice")).Enabled = false;
                    
                    WebControl ddlDeliveryType = (WebControl)form.FindControl("ddlDeliveryType");
                    if (ddlDeliveryType != null)
                        ddlDeliveryType.Enabled = false;

                    ((WebControl)form.FindControl("txtWeight")).Enabled = false;
                    ((WebControl)form.FindControl("txtMax")).Enabled = false;
                    ((WebControl)form.FindControl("txtFlatShip")).Enabled = false;
                    ((WebControl)form.FindControl("txtFlatMethod")).Enabled = false;
                    ((WebControl)form.FindControl("chkSeparate")).Enabled = false;
                    ((WebControl)form.FindControl("btnShipNotes")).Enabled = false;
                }
            }

            Merch_Inventory1.InventoryListing.Visible = (form.CurrentMode != FormViewMode.Insert);
            Merch_Inventory1.InventoryListing.DataBind();
        }
        protected void FormView1_ItemCommand(object sender, FormViewCommandEventArgs e)
        {
            FormView form = (FormView)sender;
            Merch entity = Atx.CurrentMerchRecord;
            string cmd = e.CommandName.ToLower();

            switch (cmd)
            {
                case "images":
                    base.Redirect(string.Format("/Admin/MerchEditor.aspx?p=images&merchitem={0}", e.CommandArgument));
                    break;
                case "newcat":
                    //todo: allow return url
                    base.Redirect("/Admin/EntityEditor.aspx?p=categorie");
                    break;
                case "addcat":
                    DropDownList ddlCategories = (DropDownList)form.FindControl("ddlCategories");
                    if (entity != null && ddlCategories != null && ddlCategories.SelectedIndex > 0)
                    {
                        int addCatId = int.Parse(ddlCategories.SelectedValue);
                        if (!entity.IsInCategorie(addCatId))
                        {
                            MerchCategorie categorie = (MerchCategorie)_Lookits.MerchCategories.Find(addCatId);
                            if (categorie == null)
                                throw new Exception("No categorie was found.");

                            MerchJoinCat join = categorie.MerchJoinCatRecords().AddMerchToCollection(addCatId, entity.Id);
                            entity.MerchJoinCatRecords().Add(join);

                            _Lookits.RefreshLookup(_Enums.LookupTableNames.MerchDivisions.ToString());
                            _Lookits.RefreshLookup(_Enums.LookupTableNames.MerchCategories.ToString());

                            //Atx.SetCurrentMerchRecord(entity.Id);

                            form.DataBind();
                        }
                    }
                    break;
                case "deletecat":
                    ListBox categories = (ListBox)form.FindControl("listCategories");
                    if (entity != null && categories != null && categories.SelectedIndex != -1 && categories.Items.Count > 1)
                    {
                        int deleteCatId = int.Parse(categories.SelectedValue);
                        MerchJoinCat mjc = (MerchJoinCat)entity.MerchJoinCatRecords().Find(deleteCatId);
                        if (mjc != null)
                        {
                            mjc.MerchCategorieRecord.MerchJoinCatRecords().DeleteFromCollection(mjc.Id);
                            entity.MerchJoinCatRecords().Remove(mjc);
                        }

                        _Lookits.RefreshLookup(_Enums.LookupTableNames.MerchDivisions.ToString());
                        _Lookits.RefreshLookup(_Enums.LookupTableNames.MerchCategories.ToString());

                        form.DataBind();
                    }
                    break;
                case "refresh":
                    int idx = Atx.CurrentMerchRecord.Id;
                    Atx.Clear_CurrentMerchListing();
                    Atx.SetCurrentMerchRecord(idx);
                    form.DataBind();
                    break;
                case "cancel":
                    //just rebind the control to reset
                    //if we are cancelling an insert in new child mode
                    form.ChangeMode(FormViewMode.Edit);
                    break;
            }
        }

        #region Updating

        protected void FormView1_ItemUpdating(object sender, FormViewUpdateEventArgs e)
        {
            FormView form = (FormView)sender;
            _errors.Clear();
            Merch entity = Atx.CurrentMerchRecord;

            if (entity != null)
            {
                try
                {
                    //bool activeStatusChanged = false;
                    CheckBox chkActive = (CheckBox)form.FindControl("chkActive");
                    CheckBox chkFeatured = (CheckBox)form.FindControl("chkFeatured");
                    CheckBox chkSoldOut = (CheckBox)form.FindControl("chkSoldOut");
                    //CheckBox chkTaxable = (CheckBox)form.FindControl("chkTaxable");
                    CheckBox chkInternal = (CheckBox)form.FindControl("chkInternal");
                    
                    if (entity.IsActive != chkActive.Checked) entity.IsActive = chkActive.Checked;
                    if (entity.IsFeaturedItem != chkFeatured.Checked) entity.IsFeaturedItem = chkFeatured.Checked;
                    if (entity.IsSoldOut != chkSoldOut.Checked) entity.IsSoldOut = chkSoldOut.Checked;
                    //if (entity.IsTaxable != chkTaxable.Checked) entity.IsTaxable = chkTaxable.Checked;
                    if (entity.IsInternalOnly != chkInternal.Checked) entity.IsInternalOnly = chkInternal.Checked;

                    TextBox txtName = (TextBox)form.FindControl("txtName");
                    Utils.Validation.ValidateRequiredField(_errors, "Name", txtName.Text.Trim());

                    //TODO: figure out why I did this - was there an issue with having a comma in the name?
                    //if (entity.Name != txtName.Text.Replace(',','_').Trim())
                    if (entity.Name != txtName.Text.Trim()) 
                        entity.Name = txtName.Text.Trim();

                    TextBox txtShort = (TextBox)form.FindControl("txtShortDescription");
                    if (txtShort != null)
                    {
                        string shrt = txtShort.Text.Trim();
                        if (entity.ShortText != shrt) entity.ShortText = (shrt.Length > 0) ? shrt : null;
                    }                    

                    bool priceChange = false;
                    TextBox txtPrice = (TextBox)form.FindControl("txtPrice");
                    string priceInput = txtPrice.Text.Trim();
                    Utils.Validation.ValidateRequiredField(_errors, "Price", priceInput);
                    Utils.Validation.ValidateNumericField(_errors, "Price", priceInput);
                    if (Utils.Validation.IsDecimal(priceInput))
                        if (decimal.Parse(priceInput) < 0)
                            _errors.Add("Price cannot be less than zero");

                    CheckBox chkUseSalePrice = (CheckBox)form.FindControl("chkUseSalePrice");
                    TextBox txtSalePrice = (TextBox)form.FindControl("txtSalePrice");
                    string salePriceInput = txtSalePrice.Text.Trim();
                    Utils.Validation.ValidateNumericField(_errors, "Price", salePriceInput);
                    if (salePriceInput.Trim().Length > 0 && Utils.Validation.IsDecimal(salePriceInput))
                        if (decimal.Parse(salePriceInput) < 0)
                            _errors.Add("Sale Price cannot be less than zero");

                    TextBox txtMaxQty = (TextBox)form.FindControl("txtMax");
                    Utils.Validation.ValidateIntegerField(_errors, "Max Per Order", txtMaxQty.Text.Trim());


                    TextBox txtFlat = (TextBox)form.FindControl("txtFlatShip");
                    string flatInput = txtFlat.Text.Trim();
                    Utils.Validation.ValidateNumericField(_errors, "Flat Shipping", flatInput);
                    if (flatInput.Trim().Length > 0 && Utils.Validation.IsDecimal(flatInput))
                        if (decimal.Parse(flatInput) < 0)
                            _errors.Add("Flat Shipping cannot be less than zero");

                    //if there are errors....
                    CustomValidator validator = (CustomValidator)form.FindControl("CustomValidation");

                    if (Utils.Validation.IncurredErrors(_errors, validator))
                    {
                        e.Cancel = true;
                        return;
                    }

                    //configure input
                    decimal price = decimal.Round(decimal.Parse(txtPrice.Text.Trim()), 2); 
                    decimal salePrice = (txtSalePrice.Text.Trim().Length > 0 && Utils.Validation.IsDecimal(txtSalePrice.Text.Trim())) ? decimal.Parse(txtSalePrice.Text.Trim()) : 0;
                    bool useSalePrice = chkUseSalePrice.Checked;
                    if (useSalePrice && salePrice <= 0)
                        throw new Exception("You must specify a sale price if you wish to use the sale price");
                    else if(useSalePrice && salePrice == price)
                        throw new Exception("Please specify a sale price that is different than the original price");

                    decimal inputPrice = (useSalePrice) ? salePrice : price;
                    decimal currentPrice = entity.Price_Effective;

                    if (currentPrice != inputPrice)
                    {
                        priceChange = true;
                        entity.Price = price;
                        entity.UseSalePrice = useSalePrice;
                        entity.SalePrice = salePrice;
                    }

                    DropDownList ddlTemplate = (DropDownList)form.FindControl("ddlTemplate");
                    if (entity.DisplayTemplate.ToString().ToLower() != ddlTemplate.SelectedValue.ToLower())
                        entity.DisplayTemplate = (_Enums.MerchDisplayTemplate)Enum.Parse(typeof(_Enums.MerchDisplayTemplate), ddlTemplate.SelectedValue, true);

                    CheckBox chkLowRate = (CheckBox)form.FindControl("chkLowRate");
                    bool lowRateChange = false;

                    //do not let non-parcels have a lowrate
                    if (entity.DeliveryType == _Enums.DeliveryType.parcel)
                    {
                        if (entity.IsLowRateQualified != chkLowRate.Checked)
                        {
                            lowRateChange = true;
                            entity.IsLowRateQualified = chkLowRate.Checked;
                        }
                    }
                    else if (entity.IsLowRateQualified)
                    {
                        lowRateChange = true;
                        entity.IsLowRateQualified = false;
                    }

                    bool weightChange = false;
                    TextBox txtWeight = (TextBox)form.FindControl("txtWeight");
                    decimal currentWeight = entity.Weight;

                    decimal weight = decimal.Round(ValidateWeight(entity.DeliveryType, txtWeight.Text.Trim()), 2);

                    if (currentWeight != weight)
                    {
                        weightChange = true;
                        entity.Weight = weight;
                    }

                    bool maxperChange = false;
                    int max = (txtMaxQty.Text.Trim().Length > 0) ? int.Parse(txtMaxQty.Text.Trim()) : 0;

                    if (entity.MaxQuantityPerOrder != max)
                    {
                        maxperChange = true;
                        entity.MaxQuantityPerOrder = max;
                    }

                    //handle shipping changes
                    bool shipChange = false;

                    string flat2 = txtFlat.Text.Trim();
                    if (flat2.Trim().Length > 0)
                    {
                        decimal flat = decimal.Round(decimal.Parse(flat2), 2);

                        if (entity.FlatShip != flat)
                        {
                            shipChange = true;
                            entity.FlatShip = flat;
                        }
                    }

                    TextBox txtFlatMethod = (TextBox)form.FindControl("txtFlatMethod");
                    string flatMethod = txtFlatMethod.Text.Trim();   
                    if (entity.FlatMethod != flatMethod)
                    {
                        shipChange = true;
                        entity.FlatMethod = flatMethod;
                    }

                    CheckBox chkSeparate = (CheckBox)form.FindControl("chkSeparate");
                    if (entity.IsShipSeparate != chkSeparate.Checked)
                    {
                        shipChange = true;
                        entity.IsShipSeparate = chkSeparate.Checked;
                    }                        
                       
                    //WillCallWeb.Components.Util.CalendarClock clk = (WillCallWeb.Components.Util.CalendarClock)form.FindControl("clockBackorder");
                    //if (entity.BackorderDate != clk.SelectedDate)
                    //{
                    //    shipChange = true;

                    //    //backorder default is set to Constants_minDate which is sqlMinDate
                    //    entity.BackorderDate = clk.SelectedDate;
                    //}
                    //end shipping changes


                    //Save parent and potential children
                    entity.Save_AvoidRealTimeVars();

                    if (priceChange || weightChange || maxperChange || shipChange || lowRateChange)// || deliveryChange)
                    {
                        //we need to update the children here as well
                        MerchCollection childrenToUpdate = new MerchCollection();
                        foreach (Merch child in entity.ChildMerchRecords())
                        {
                            child.Price = entity.Price;
                            child.UseSalePrice = useSalePrice;
                            child.SalePrice = entity.SalePrice;
                            child.DeliveryType = entity.DeliveryType;
                            child.IsLowRateQualified = entity.IsLowRateQualified;
                            child.Weight = entity.Weight;
                            child.FlatShip = entity.FlatShip;
                            child.FlatMethod = entity.FlatMethod;
                            child.BackorderDate = entity.BackorderDate;
                            child.IsShipSeparate = entity.IsShipSeparate;
                            child.MaxQuantityPerOrder = max;
                            childrenToUpdate.Add(child);
                        }

                        if (childrenToUpdate.Count > 0)
                            foreach (Merch child in childrenToUpdate)
                                child.Save_AvoidRealTimeVars();

                        if (priceChange)
                        {
                            HistoryPricing hist = new HistoryPricing();
                            hist.DtStamp = DateTime.Now;
                            System.Web.Security.MembershipUser mem = System.Web.Security.Membership.GetUser(System.Web.HttpContext.Current.Profile.UserName);
                            hist.UserId = (Guid)mem.ProviderUserKey;
                            hist.TMerchId = entity.Id;
                            hist.DateAdjusted = DateTime.Now;
                            hist.OldPrice = currentPrice;
                            hist.NewPrice = inputPrice;
                            hist.Context = _Enums.HistoryInventoryContext.ParentPrice;
                            hist.Save();
                        }
                    }

                    //we may need to update data
                    int index = entity.Id;
                    Atx.Clear_CurrentMerchListing();
                    Atx.SetCurrentMerchRecord(index);

                    //update the entity in the division lookups
                    _Lookits.RefreshLookup(_Enums.LookupTableNames.MerchDivisions.ToString());
                    _Lookits.RefreshLookup(_Enums.LookupTableNames.MerchCategories.ToString());

                    form.DataBind();
                }
                catch (Exception ex)
                {
                    _Error.LogException(ex);
                    CustomValidator custom = CustomValidation;

                    if (custom != null)
                    {
                        custom.IsValid = false;
                        custom.ErrorMessage = ex.Message;
                    }

                    e.Cancel = true;
                }
            }
        }
        private decimal ValidateWeight(_Enums.DeliveryType delivery, string input)
        {
            //gift certificate items can have zero weight and are not required
            if (delivery != _Enums.DeliveryType.parcel)
                return 0;

            if (input == null || input.Trim().Length == 0)
                throw new Exception("Weight is required");

            decimal retVal = 0;
            if(!decimal.TryParse(input, out retVal))
                throw new Exception("Please enter a valid weight");

            if(retVal < 0.01M || retVal > 30.0M)
                throw new Exception("Weight must be between 0.01 and 30");

            return retVal;
        }

        #endregion

        #region Inserting

        protected void FormView1_ItemInserting(object sender, FormViewInsertEventArgs e)
        {
            FormView form = (FormView)sender;
            _errors.Clear();

            try
            {
                //create a new join to category
                DropDownList ddlCats = (DropDownList)form.FindControl("ddlCategories");
                int idxCategorie = int.Parse(ddlCats.SelectedValue);
                if (idxCategorie == 0)
                    _errors.Add("No categorie was selected");

                //validate that the name does not already exist
                TextBox txtName = (TextBox)form.FindControl("txtName");
                string newName = txtName.Text.Replace(',', '_').Trim();

                if (newName.Length == 0)
                    _errors.Add("Name is required");
                

                CustomValidator validator = CustomValidation;

                if (Utils.Validation.IncurredErrors(_errors, validator))
                {
                    e.Cancel = true;
                    return;
                }

                DropDownList ddlDeliveryType = (DropDownList)form.FindControl("ddlDeliveryType");
                _Enums.DeliveryType delivery = _Enums.DeliveryType.parcel;
                if (ddlDeliveryType != null)
                    delivery = (_Enums.DeliveryType)Enum.Parse(typeof(_Enums.DeliveryType), ddlDeliveryType.SelectedValue, true);

                CheckBox chkLowRate = (CheckBox)form.FindControl("chkLowRate");

                //decimal  = 0;
                TextBox txtWeight = (TextBox)form.FindControl("txtWeight");
                decimal newWeight = decimal.Round(ValidateWeight(delivery, txtWeight.Text.Trim()), 2);

                SubSonic.QueryCommand cmd = new SubSonic.QueryCommand(string.Empty, SubSonic.DataService.Provider.Name);
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                cmd.AddParameter("@newName", newName, System.Data.DbType.String);
                sb.Append("SELECT [Id] FROM Merch m WHERE m.[Name] = @newName AND m.[tParentListing] IS NULL AND m.[Id] IN ");
                sb.AppendFormat("(SELECT [tMerchId] FROM MerchJoinCat mjc WHERE mjc.[tMerchCategorieId] = {0}) ", idxCategorie);

                cmd.CommandSql = sb.ToString();
                object returnVal = SubSonic.DataService.ExecuteScalar(cmd);

                if (returnVal != null && (int)returnVal > 0)
                    throw new Exception("There is another item in the chosen category with the same name. Please change the name (or categorie) and try again");
                
                Merch newItem = new Merch();
                newItem.ApplicationId = _Config.APPLICATION_ID;
                newItem.DtStamp = DateTime.Now;
                newItem.IsActive = true;
                newItem.Name = newName; 
                newItem.IsTaxable = false;
                newItem.IsFeaturedItem = false;
                newItem.IsSoldOut = false;
                newItem.MaxQuantityPerOrder = (delivery != _Enums.DeliveryType.download) ? _Config._MaxMerchPurchaseQuantity : 1;
                newItem.Weight = newWeight;
                newItem.DeliveryType = delivery;
                //must be a parcel to qualify
                newItem.IsLowRateQualified = (delivery == _Enums.DeliveryType.parcel && chkLowRate.Checked);

                MerchCategorie chosenCategorie = (MerchCategorie)_Lookits.MerchCategories.Find(int.Parse(ddlCats.SelectedValue));
                if (chosenCategorie == null)
                    throw new Exception("No categorie was found.");

                MerchDivision division = (MerchDivision)_Lookits.MerchDivisions.Find(chosenCategorie.TMerchDivisionId);
                if (division == null)
                    throw new Exception("No division was found.");

                MerchCategorie descendantCategorie = (MerchCategorie)division.MerchCategorieRecords().Find(chosenCategorie.Id);
                if (descendantCategorie == null)
                    throw new Exception("No descendantCategorie was found.");

                newItem.Save();//ok to use save here - insert is only dirty row

                //add auto inventory to downloads
                if (newItem.DeliveryType == _Enums.DeliveryType.download)
                {
                    int newAllotment = _Config._Default_DownloadInventoryAllotment;

                    Merch newInventory = new Merch();
                    newInventory.ApplicationId = _Config.APPLICATION_ID;
                    newInventory.DtStamp = DateTime.Now;
                    newInventory.TParentListing = newItem.Id;
                    newInventory.Allotment = newAllotment;
                    newInventory.IsLowRateQualified = newItem.IsLowRateQualified;
                    newInventory.IsActive = true;

                    //gift certificates will ignore this
                    newInventory.Price = newItem.Price;
                    newInventory.IsSoldOut = false;
                    newInventory.UseSalePrice = false;
                    newInventory.SalePrice = 0.0M;
                    newInventory.Weight = 0.0M;
                    newInventory.MaxQuantityPerOrder = newItem.MaxQuantityPerOrder;

                    newInventory.Save();//ok to use save method here - insert is only dirty row

                    HistoryInventory hist = new HistoryInventory();
                    System.Web.Security.MembershipUser mem = System.Web.Security.Membership.GetUser(System.Web.HttpContext.Current.Profile.UserName);
                    hist.UserId = (Guid)mem.ProviderUserKey;
                    hist.DtStamp = DateTime.Now;
                    hist.TMerchId = newItem.Id;
                    hist.DateAdjusted = DateTime.Now;
                    hist.CurrentlyAllotted = 0;
                    hist.Adjustment = newAllotment;
                    hist.Context = _Enums.HistoryInventoryContext.Allotment;
                    hist.Save();
                }

                descendantCategorie.MerchJoinCatRecords().AddMerchToCollection(descendantCategorie.Id, newItem.Id);
                
                _Lookits.RefreshLookup(_Enums.LookupTableNames.MerchDivisions.ToString());
                _Lookits.RefreshLookup(_Enums.LookupTableNames.MerchCategories.ToString());

                Atx.CurrentMerchListing.Add(newItem);

                Atx.SetCurrentMerchRecord(newItem.Id);

                Atx.MerchParents = null;

                form.ChangeMode(FormViewMode.Edit);//automatically binds the form

                //redirect to reset all vars
                base.Redirect(string.Format("/Admin/MerchEditor.aspx?p=ItemEdit&merchitem={0}", Atx.CurrentMerchRecord.Id));

                //form.DataBind();

            }
            catch (System.Threading.ThreadAbortException) { }
            catch (Exception ex)
            {
                _Error.LogException(ex);
                CustomValidator validation = CustomValidation;

                if (validation != null)
                {
                    validation.IsValid = false;
                    validation.ErrorMessage = ex.Message;
                }
            }
        }

        #endregion

        #region Deleting

        protected void FormView1_ItemDeleting(object sender, FormViewDeleteEventArgs e)
        {
            FormView form = (FormView)sender;

            try
            {   
                Merch entity = Atx.CurrentMerchRecord;

                if (entity.Sold > 0)
                    throw new Exception("Item has sales and cannot be deleted. Please mark as not active.");

                foreach (Merch child in entity.ChildMerchRecords())
                    Merch.Delete(child.Id);

                //do separately - don't allow image mgmt to interfere with deletion
                try
                {
                    foreach (ItemImage img in entity.ItemImageRecords())
                        img.ImageManager.Delete();
                }
                catch (Exception ex)
                {
                    _Error.LogException(ex);
                }

                Merch.Delete(entity.Id);

                Atx.CurrentMerchListing.Remove(entity);

                //update the entity in the division lookups
                _Lookits.RefreshLookup(_Enums.LookupTableNames.MerchDivisions.ToString());

                Atx.Clear_CurrentMerchListing();

                if (entity.IsChild)
                    base.Redirect(string.Format("/Admin/MerchEditor.aspx?p=ItemEdit&merchitem={0}", entity.ParentMerchRecord.Id));

                Atx.SetCurrentMerchRecord(0);

                base.Redirect("/Admin/MerchEditor.aspx");
            }
            catch (Exception ex)
            {
                _Error.LogException(ex);
                CustomValidator custom = this.CustomValidation;

                if (custom != null)
                {
                    custom.IsValid = false;
                    custom.ErrorMessage = ex.Message;
                }
            }
        }

        #endregion

        #endregion

        protected MerchCollection OrderedCollection
        {
            get
            {
                MerchCollection _merch = new MerchCollection();

                if(Atx.CurrentMerchRecord != null)
                {
                    bool _sortAscending = true;

                    _merch.LoadAndCloseReader(
                        SPs.TxMerchChildrenSorted(
                            Atx.CurrentMerchRecord.Id,
                            string.Format("{0} {1}", (Atx.CurrentMerchRecord.IsGiftCertificateDelivery) ? "mPrice" : "Style", 
                            (_sortAscending) ? "ASC" : "DESC")
                    ).GetReader());
                }

                return _merch;
            }
        }
        
}
}