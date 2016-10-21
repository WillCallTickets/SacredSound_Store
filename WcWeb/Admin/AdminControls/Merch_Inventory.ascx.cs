using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using Wcss;

namespace WillCallWeb.Admin.AdminControls
{
    //<iframe id="productdescription" frameborder="0" scrolling="auto" src="/Admin/Display_Merch.aspx"></iframe>

    public partial class Merch_Inventory : BaseControl, IPostBackEventHandler
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
                    ParentForm.DataBind();
                    break;
            }
        }

        //allow to be set by parent
        private FormView _parentForm = null;
        public FormView ParentForm
        {
            get
            {
                return _parentForm;
            }
            set
            {
                _parentForm = value;
            }
        }

        //expose to parent
        public ListView InventoryListing
        {
            get { return this.lstInventory; }
        }

        protected MerchCollection OrderedCollection
        {
            get
            {
                MerchCollection _merch = new MerchCollection();

                if (Atx.CurrentMerchRecord != null)
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

        List<string> _errors = new List<string>();
        
        protected void btnEditor_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            string name = btn.ID.ToLower();
            switch (name)
            {
                case "btneditcolor":
                    base.Redirect("/Admin/EntityEditor.aspx?p=color");
                    break;
                case "btneditsize":
                    base.Redirect("/Admin/EntityEditor.aspx?p=size");
                    break;
            }
        }

        #region ListView

        protected void BindNonConformists()
        {
            Label lblAttribsInSync = (Label)lstInventory.FindControl("lblAttribsInSync");
            
            if (lblAttribsInSync != null)
            {
                lblAttribsInSync.Text = string.Empty;

                List<string> nonconform = new List<string>();

                if (Atx.CurrentMerchRecord != null && OrderedCollection.Count > 0)
                {
                    if (Atx.CurrentMerchRecord.HasChildStyles)
                    {
                        foreach (Merch child in Atx.CurrentMerchRecord.ChildMerchRecords())
                            if (child.Style == null || child.Style.Trim().Length == 0)
                                nonconform.Add(string.Format("{0} does not have a valid style and will not be listed in the item's page", child.AttribChoice));
                    }

                    if (Atx.CurrentMerchRecord.HasChildColors)
                    {
                        foreach (Merch child in Atx.CurrentMerchRecord.ChildMerchRecords())
                            if (child.Color == null || child.Color.Trim().Length == 0)
                                nonconform.Add(string.Format("{0} does not have a valid color and will not be listed in the item's page", child.AttribChoice));
                    }

                    if (Atx.CurrentMerchRecord.HasChildSizes)
                    {
                        foreach (Merch child in Atx.CurrentMerchRecord.ChildMerchRecords())
                            if (child.Size == null || child.Size.Trim().Length == 0)
                                nonconform.Add(string.Format("{0} does not have a valid size and will not be listed in the item's page", child.AttribChoice));
                    }
                }

                if (nonconform.Count > 0)
                {
                    lblAttribsInSync.Text = "<div class=\"validationsummary\" style=\"display:block;font-size:10px;\" ><ul>";
                    lblAttribsInSync.Text += string.Format("<li>{0}</li>", "All inventory must have matching entries for style/color/size. ");
                    foreach (string s in nonconform)
                        lblAttribsInSync.Text += string.Format("<li>{0}</li>", s);

                    lblAttribsInSync.Text += "</ul></div>";
                }
            }
        }
        protected void lstInventory_DataBinding(object sender, EventArgs e)
        {
            ListView view = (ListView)sender;

            view.Visible = (Atx.CurrentMerchRecord != null && ParentForm.CurrentMode != FormViewMode.Insert);

            if (view.Visible)
            {
                view.DataSource = OrderedCollection;
                string[] keyNames = { "Id" };
                view.DataKeyNames = keyNames;
            }

            BindNonConformists();
        }
        protected void lstInventory_ItemCreated(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.InsertItem)
            {
                ListView view = (ListView)sender;
                ListViewItem viewItem = (ListViewItem)e.Item;

                DropDownList ddlStyle = (DropDownList)viewItem.FindControl("ddlStyle");
                DropDownList ddlColor = (DropDownList)viewItem.FindControl("ddlColor");
                DropDownList ddlSize = (DropDownList)viewItem.FindControl("ddlSize");
                base.FillMerchStyleColorSizeLists(ddlStyle, ddlColor, ddlSize, Atx.CurrentMerchRecord, null, true, true, false);

                //if we have a gift certificate - than allow price entry
                //otherwise enter the parents price
                TextBox txtInvPrice = (TextBox)viewItem.FindControl("txtInvPrice");
                if (txtInvPrice != null)
                {
                    //txtInvPrice.ReadOnly = (Atx.CurrentMerchRecord == null || Atx.CurrentMerchRecord.DeliveryType != _Enums.DeliveryType.giftcertificate);
                    txtInvPrice.Text = (Atx.CurrentMerchRecord != null && Atx.CurrentMerchRecord.DeliveryType != _Enums.DeliveryType.giftcertificate) ?
                        Atx.CurrentMerchRecord.Price.ToString("n2") : "";
                }

                TextBox txtAllot = (TextBox)viewItem.FindControl("txtAllot");
                TextBox lstAllot = (TextBox)viewItem.FindControl("lstAllot");
                if (txtAllot != null && lstAllot != null)
                {
                    lstAllot.Visible = (Atx.CurrentMerchRecord.IsActivationCodeDelivery);
                    txtAllot.Visible = (!lstAllot.Visible);
                }

                Button btnNewItem = (Button)view.FindControl("btnNewItem");
                if (btnNewItem != null)
                    btnNewItem.Visible = (view.InsertItemPosition == InsertItemPosition.None && view.EditIndex < 0);
            }
        }
        protected void lstInventory_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            ListView view = (ListView)sender;
            ListViewDataItem viewItem = (ListViewDataItem)e.Item;
            Merch child = (Merch)viewItem.DataItem;

            DropDownList ddlStyle = (DropDownList)viewItem.FindControl("ddlStyle");
            DropDownList ddlColor = (DropDownList)viewItem.FindControl("ddlColor");
            DropDownList ddlSize = (DropDownList)viewItem.FindControl("ddlSize");
            base.FillMerchStyleColorSizeLists(ddlStyle, ddlColor, ddlSize, Atx.CurrentMerchRecord, child, true, true, false);

            TextBox txtStyle = (TextBox)viewItem.FindControl("txtStyle");

            TextBox txtPrice = (TextBox)viewItem.FindControl("txtPrice");
            //if (txtPrice != null)
            //    txtPrice.ReadOnly = (Atx.CurrentMerchRecord == null || Atx.CurrentMerchRecord.DeliveryType != _Enums.DeliveryType.giftcertificate);

            LinkButton btnDelete = (LinkButton)viewItem.FindControl("btnDelete");
            if (btnDelete != null && child != null)
                btnDelete.OnClientClick = string.Format("return confirm('Are you sure you want to delete {0}?')",
                        Utils.ParseHelper.ParseJsAlert(child.DisplayNameWithAttribs));

            LinkButton btnEdit = (LinkButton)viewItem.FindControl("btnEdit");
            if (btnEdit != null)
            {
                btnEdit.ToolTip = string.Format("Edit - {0}", child.Id.ToString());
            }

            TextBox txtAllot = (TextBox)viewItem.FindControl("txtAllot");
            TextBox lstAllot = (TextBox)viewItem.FindControl("lstAllot");
            Label lblAllot = (Label)viewItem.FindControl("lblAllot");    
            if (txtAllot != null && lstAllot != null && lblAllot != null)
            {
                lstAllot.Visible = (Atx.CurrentMerchRecord.IsActivationCodeDelivery);
                txtAllot.Visible = (!lstAllot.Visible);
                lblAllot.Visible = lstAllot.Visible;
            }

            //Button btnAdd = (Button)viewItem.FindControl("btnAdd");
            Button btnRemoveUnused = (Button)viewItem.FindControl("btnRemoveUnused");
            if (lstAllot != null && btnRemoveUnused != null)
            {
                //btnAdd.Visible = lstAllot.Visible;
                btnRemoveUnused.Visible = lstAllot.Visible;
            }

            Button btnNewItem = (Button)view.FindControl("btnNewItem");
            if (btnNewItem != null)
                btnNewItem.Visible = (view.InsertItemPosition == InsertItemPosition.None && view.EditIndex < 0);
        }
        protected void lstInventory_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            string cmd = e.CommandName.ToLower();
            Merch child = null;
            ListView view = (ListView)sender;
            ListViewItem viewItem = (ListViewItem)e.Item;
            CustomValidator validation = (CustomValidator)viewItem.FindControl("CustomValidation");
            _errors.Clear();

            int arg = 0;
            if(e.CommandArgument != null)
                int.TryParse(e.CommandArgument.ToString(), out arg);

            //errors are ignored when not used
            if(arg == 0)
                _errors.Add("Invalid argument in command control.");
            else
                child = (Merch)Atx.CurrentMerchRecord.ChildMerchRecords().Find(arg);
            

            switch (cmd)
            {
                case "new":            
                    view.EditIndex = -1;
                    view.InsertItemPosition = InsertItemPosition.LastItem;
                    view.DataBind();
                    break;
                    
                case "removeunusedcodes":
                    //delete unused codes
                    int deleted = Inventory.DeleteUnusedCodes(_Enums.ItemContextCode.m, arg);
                    //get number of used - should match sold + damaged (refund too?)
                    //if (deleted != child.Available)
                    //{
                    //    //send notification
                    //}

                    if (deleted > 0)
                    {
                        //reset/resync allotment/sold/available
                        Inventory.AdjustInventoryHistory(
                            child,
                            child.Allotment,
                            (-1) * deleted,
                            System.Web.HttpContext.Current.Profile.UserName,
                            _Enums.HistoryInventoryContext.Allotment);

                        child.Allotment -= deleted;
                        child.Save();

                        int pid = Atx.CurrentMerchRecord.Id;
                        Atx.Clear_CurrentMerchListing();
                        Atx.SetCurrentMerchRecord(pid);

                        ParentForm.DataBind();
                    }
                    break;

                //case "syncinventorywithcodes":
                    
                //    if (Utils.Validation.IncurredErrors(_errors, validation))
                //        return;

                //    //if # available codes does not match merch inventory
                //    int available = Inventory.GetNumberOfAvailableCodes(_Enums.ItemContextCode.m, arg);

                //    //sync it - adjust history
                //    if (child.Available < available)
                //    {
                //        //add more allotment
                //    }
                //    else if (child.Available > available)
                //    {
                //        //set allotment to lower number or to where there is zero
                //    }

                //    //send notification
                //    ParentForm.DataBind();
                //    break;

                case "reportdamage":
                    
                    string dmg = string.Empty;
                    TextBox txtDmg = (TextBox)viewItem.FindControl("txtDmg");
                    if (txtDmg != null)
                        dmg = txtDmg.Text.Trim();
                    Utils.Validation.ValidateIntegerField(_errors, "Damaged", dmg);

                    if (Utils.Validation.IncurredErrors(_errors, validation))
                        return;

                    if (dmg != null && dmg.Trim().Length > 0 && Utils.Validation.IsInteger(dmg))
                    {
                        int damaged = int.Parse(dmg);

                        Inventory.AdjustInventoryHistory(
                            child,
                            child.Allotment,
                            damaged,
                            System.Web.HttpContext.Current.Profile.UserName,
                            _Enums.HistoryInventoryContext.Damage);

                        child.Damaged += damaged;
                        child.Save();

                        int pid = Atx.CurrentMerchRecord.Id;
                        Atx.Clear_CurrentMerchListing();
                        Atx.SetCurrentMerchRecord(pid);

                        txtDmg.Text = string.Empty;
                        ParentForm.DataBind();
                    }
                    break;
            }
        }
        protected void lstInventory_ItemEditing(object sender, ListViewEditEventArgs e)
        {
            ListView view = (ListView)sender;
            view.InsertItemPosition = InsertItemPosition.None;
            view.EditIndex = e.NewEditIndex;
            view.DataBind();
        }
        protected void FillCodeListFromText(TextBox txt, List<string> list)
        {
            list.Clear();

            //see if codes have been entered and validate? codes
            string input = txt.Text.Trim();

            //do this next step to 1)remove any angle brackets 2)format the string so that we can SPLIT it later on ~
            //this may seem redundant - but it is necessary to facilitate how the production server may see things
            string parsed = Utils.ParseHelper.RemoveSpaces(input.Replace("<", string.Empty).Replace(">", string.Empty).Replace(";", "~").Replace(",", "~")
                .Replace(Environment.NewLine, "~").Replace("\r\n", "~").Replace("\n", "~"));

            string[] pieces = parsed.Split(new char[] { '~' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string s in pieces)
                if (!list.Contains(s))
                    list.Add(s);
        }
        protected void lstInventory_ItemInserting(object sender, ListViewInsertEventArgs e)
        {
            ListView view = (ListView)sender;
            ListViewItem viewItem = (ListViewItem)e.Item;
            CustomValidator validation = (CustomValidator)viewItem.FindControl("CustomValidation");
            _errors.Clear();

            //validate inputs - price, allotment
            string price = string.Empty;
            TextBox txtInvPrice = (TextBox)viewItem.FindControl("txtInvPrice");
            if (txtInvPrice != null)
                price = txtInvPrice.Text.Trim();
            Utils.Validation.ValidateNumericField(_errors, "Price", price);


            TextBox txtAllot = (TextBox)viewItem.FindControl("txtAllot");
            TextBox lstAllot = (TextBox)viewItem.FindControl("lstAllot");
            string allt = "0";
            List<string> allotCodes = new List<string>();

            if (txtAllot != null && txtAllot.Visible)
            {
                allt = txtAllot.Text.Trim();
                Utils.Validation.ValidateIntegerField(_errors, "Allotment", allt);
            }
            else if (lstAllot != null && lstAllot.Visible && lstAllot.Text.Trim().Length > 0)
            {
                FillCodeListFromText(lstAllot, allotCodes);
                allt = allotCodes.Count.ToString();
                //if (lstAllot.Items.Count != allotCodes.Count)
                //    _errors.Add("Your code submission contains duplicates.");
                
            }                        

            if (Utils.Validation.IncurredErrors(_errors, validation))
            {
                e.Cancel = true;
                return;
            }
            //end of input validation

            DropDownList ddlStyle = (DropDownList)viewItem.FindControl("ddlStyle");
            TextBox txtStyle = (TextBox)viewItem.FindControl("txtStyle");
            DropDownList ddlColor = (DropDownList)viewItem.FindControl("ddlColor");
            DropDownList ddlSize = (DropDownList)viewItem.FindControl("ddlSize");
            bool isGiftCert = (Atx.CurrentMerchRecord.DeliveryType == _Enums.DeliveryType.giftcertificate);

            try
            {
                string style = (ddlStyle.SelectedIndex > 0) ? ddlStyle.SelectedItem.Text : null;
                if (txtStyle.Text.Trim().Length > 0)
                    style = txtStyle.Text.Trim();

                string color = (ddlColor.SelectedIndex > 0) ? ddlColor.SelectedItem.Text : null;
                string size = (ddlSize.SelectedIndex > 0) ? ddlSize.SelectedItem.Text : null;
                
                // determine if the item is unique
                Merch exists = Atx.CurrentMerchRecord.FindChildItem(style, color, size);

                if (exists != null)
                    throw new Exception(string.Format("Item currently exists in this style({0}) color({1}) and size({2}).", style, color, size));

                decimal priceInput = 0;
                priceInput = (Utils.Validation.IsDecimal(price)) ? decimal.Parse(price) : 0;
                if (isGiftCert && priceInput <= 0)
                    throw new Exception("Price is required for gift certificates and must be greater than zero");


                //if (priceInput <= 0)
                //    throw new Exception("Price is required and must be greater than zero");
                
                //if (isGiftCert && price != null)
                //{
                //    priceInput = (Utils.Validation.IsDecimal(price)) ? decimal.Parse(price) : 0;
                //    if (priceInput <= 0)
                //        throw new Exception("Price is required for gift certificates and must be greater than zero");
                //}
                //else
                //    priceInput = Atx.CurrentMerchRecord.Price;


                int allot = int.Parse(allt);

                Merch newItem = new Merch();
                newItem.ApplicationId = _Config.APPLICATION_ID;
                newItem.DtStamp = DateTime.Now;
                newItem.TParentListing = Atx.CurrentMerchRecord.Id;
                newItem.Style = style;
                newItem.Color = color;
                newItem.Size = size;
                newItem.Allotment = allot;
                //delivery type  is calc'd by parent
                //newItem.DeliveryType
                newItem.IsLowRateQualified = Atx.CurrentMerchRecord.IsLowRateQualified;

                newItem.IsActive = true;
                newItem.IsFeaturedItem = false;
                newItem.IsSoldOut = false;
                newItem.Price = priceInput;

                //gift certificates will ignore this
                newItem.UseSalePrice = Atx.CurrentMerchRecord.UseSalePrice;
                newItem.SalePrice = Atx.CurrentMerchRecord.SalePrice;
                newItem.Weight = Atx.CurrentMerchRecord.Weight;
                newItem.MaxQuantityPerOrder = Atx.CurrentMerchRecord.MaxQuantityPerOrder;

                newItem.Save();//ok to use save method here - insert is only dirty row

                int actually = 0;

                if (allotCodes.Count > 0)
                {
                    try
                    {
                        actually = Inventory.BulkEnterCodes(_Enums.ItemContextCode.m, newItem.Id, allotCodes);
                    }
                    catch (Exception ex)
                    {
                        //rollback!
                        Merch.Delete(newItem.Id);
                        _errors.Add(ex.Message);

                        if (Utils.Validation.IncurredErrors(_errors, validation))
                        {
                            e.Cancel = true;
                            return;
                        }
                    }
                }
                else
                {
                    actually = allot;
                    //Atx.CurrentMerchRecord.ChildMerchRecords().Add(newItem);
                }

                if (actually > 0)
                {
                    Inventory.AdjustInventoryHistory(
                            newItem,
                            0,
                            actually,
                            System.Web.HttpContext.Current.Profile.UserName,
                            _Enums.HistoryInventoryContext.Allotment);
                }

                //Atx.CurrentMerchRecord.ChildMerchRecords().Add(newItem);


                if (Atx.CurrentMerchRecord.IAllotment > 0)
                {
                    Atx.CurrentMerchRecord.Allotment = 0;//reset
                    Atx.CurrentMerchRecord.Sold = 0;
                    Atx.CurrentMerchRecord.Damaged = 0;
                    Atx.CurrentMerchRecord.Refunded = 0;

                    Atx.CurrentMerchRecord.Save();// _AvoidRealTimeVars();
                }

                //refresh data
                int idx = Atx.CurrentMerchRecord.Id;
                Atx.Clear_CurrentMerchListing();
                Atx.SetCurrentMerchRecord(idx);

                //reset insert index
                view.InsertItemPosition = InsertItemPosition.None;

                ParentForm.DataBind();
            }
            catch (Exception ex)
            {
                if (validation != null)
                {
                    validation.IsValid = false;
                    validation.ErrorMessage = ex.Message;
                }
            }
        }
        protected void lstInventory_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
            ListView view = (ListView)sender;
            ListViewDataItem viewItem = (ListViewDataItem)view.Items[e.ItemIndex];
            CustomValidator validation = (CustomValidator)viewItem.FindControl("CustomValidation");
            _errors.Clear();

            //validate inputs - price, allotment, damaged
            string price = string.Empty;
            TextBox txtPrice = (TextBox)viewItem.FindControl("txtPrice");
            if (txtPrice != null)
                price = txtPrice.Text.Trim();
            Utils.Validation.ValidateNumericField(_errors, "Price", price);

            TextBox txtAllot = (TextBox)viewItem.FindControl("txtAllot");
            TextBox lstAllot = (TextBox)viewItem.FindControl("lstAllot");
            string allt = "0";
            List<string> allotCodes = new List<string>();

            if (txtAllot != null && txtAllot.Visible)
            {
                allt = txtAllot.Text.Trim();
                Utils.Validation.ValidateIntegerField(_errors, "Allotment", allt);
            }
            else if (lstAllot != null && lstAllot.Visible && lstAllot.Text.Trim().Length > 0)
            {
                FillCodeListFromText(lstAllot, allotCodes);
                allt = allotCodes.Count.ToString();
            }    

            if (Utils.Validation.IncurredErrors(_errors, validation))
            {
                e.Cancel = true;
                return;
            }
            //end of input validation

            int allotedNum = int.Parse(allt);
            int idx = (int)view.DataKeys[e.ItemIndex].Value;

            if (Atx.CurrentMerchRecord.ChildMerchRecords().Count > 0)
            {
                Merch child = (Merch)Atx.CurrentMerchRecord.ChildMerchRecords().Find(idx);

                if (child != null)
                {
                    DropDownList ddlStyle = (DropDownList)viewItem.FindControl("ddlStyle");
                    TextBox txtStyle = (TextBox)viewItem.FindControl("txtStyle");
                    DropDownList ddlColor = (DropDownList)viewItem.FindControl("ddlColor");
                    DropDownList ddlSize = (DropDownList)viewItem.FindControl("ddlSize");

                    try
                    {
                        if (ddlStyle != null)
                        {
                            string style = (ddlStyle.SelectedIndex > 0) ? ddlStyle.SelectedItem.Text : null;
                            //text input overrides any existing style
                            if (txtStyle.Text.Trim().Length > 0)
                                style = txtStyle.Text.Trim();

                            if (style != child.Style)
                                child.Style = style;
                        }
                        if (ddlColor != null)
                        {
                            string color = (ddlColor.SelectedIndex > 0) ? ddlColor.SelectedItem.Text : null;
                            if (color != child.Color)
                                child.Color = color;
                        }
                        if (ddlSize != null)
                        {
                            string size = null;
                            if (ddlSize.SelectedIndex > 0)
                            {
                                MerchSize mSize = (MerchSize)_Lookits.MerchSizes.Find(int.Parse(ddlSize.SelectedValue));
                                if (mSize != null)
                                    size = mSize.Name;
                            }
                            if (size != child.Size)
                                child.Size = size;
                        }


                        CheckBox chkActive = (CheckBox)viewItem.FindControl("chkActive");
                        CheckBox chkSoldOut = (CheckBox)viewItem.FindControl("chkSoldOut");

                        if (chkActive != null && chkActive.Checked != child.IsActive)
                            child.IsActive = chkActive.Checked;
                        if (chkSoldOut != null && chkSoldOut.Checked != child.IsSoldOut)
                            child.IsSoldOut = chkSoldOut.Checked;

                        //prices only change for gift certs - all other items reflect the parents price
                        //if (child.DeliveryType == _Enums.DeliveryType.giftcertificate && txtPrice != null)
                        if (txtPrice != null)
                        {
                            string s = txtPrice.Text.Trim();
                            decimal d = (s.Length > 0 && Utils.Validation.IsDecimal(s)) ? decimal.Parse(s) : 0;

                            if (child.DeliveryType == _Enums.DeliveryType.giftcertificate && d == 0)
                                throw new Exception("Gift Certificates must have a price greater than zero.");

                            if (d != child.Price) child.Price = d;
                        }

                        child.Save_AvoidRealTimeVars();


                        //now play with inventory
                        //now decide if txt or from list
                        if (txtAllot != null && txtAllot.Visible && txtAllot.Text.Trim().Length > 0 && allotedNum != child.Allotment)
                        {
                            if (allotedNum - child.Damaged < child.Sold)
                                throw new Exception("<li>Allotment (including damaged goods) cannot be set to less than quantity sold.</li>");

                            Inventory.AdjustInventoryHistory(
                                child,
                                child.Allotment,
                                allotedNum - child.Allotment,
                                System.Web.HttpContext.Current.Profile.UserName,
                                _Enums.HistoryInventoryContext.Allotment);

                            child.Allotment = allotedNum;//set new inventory to input
                            child.Save();
                        }
                        else if (lstAllot != null && lstAllot.Visible && allotedNum > 0)
                        {
                            int actual = Inventory.BulkEnterCodes(_Enums.ItemContextCode.m, child.Id, allotCodes);

                            if (actual > 0)
                            {
                                Inventory.AdjustInventoryHistory(
                                    child,
                                    child.Allotment,
                                    actual,
                                    System.Web.HttpContext.Current.Profile.UserName,
                                    _Enums.HistoryInventoryContext.Allotment);

                                child.Allotment += actual;//add new codes to inventory
                                child.Save();
                            }
                        }

                        //ensure parent is not holding inventory
                        if (Atx.CurrentMerchRecord.IAllotment > 0)
                        {
                            Atx.CurrentMerchRecord.Allotment = 0;//reset
                            Atx.CurrentMerchRecord.Sold = 0;
                            Atx.CurrentMerchRecord.Damaged = 0;
                            Atx.CurrentMerchRecord.Refunded = 0;

                            Atx.CurrentMerchRecord.Save();
                        }


                        //refresh data
                        int pid = Atx.CurrentMerchRecord.Id;
                        Atx.Clear_CurrentMerchListing();
                        Atx.SetCurrentMerchRecord(pid);
                        
                        ParentForm.DataBind();

                        //if the ordering has changed due to a name/style etc change - then be sure to stick with the item
                        if (lstInventory.DataKeys[lstInventory.EditIndex] == null ||
                            (int)lstInventory.DataKeys[lstInventory.EditIndex].Value != idx)
                        {
                            lstInventory.EditIndex = OrderedCollection.GetList().FindIndex(delegate(Merch match) { return (match.Id == idx); });
                            lstInventory.DataBind();
                        }

                    }
                    catch (Exception ex)
                    {
                        if (validation != null)
                        {
                            _Error.LogException(ex);
                            validation.IsValid = false;
                            validation.ErrorMessage = ex.Message;

                            e.Cancel = true;
                        }
                    }
                }
            }
        }
        protected void lstInventory_ItemUpdated(object sender, ListViewUpdatedEventArgs e) 
        {
            Page.ClientScript.RegisterStartupScript(
                this.GetType(), "update_success", "alert('Update Successful!');", true);
        }
        protected void lstInventory_ItemCancelling(object sender, ListViewCancelEventArgs e)
        {   
            ListView view = (ListView)sender;

            if (e.CancelMode == ListViewCancelMode.CancelingEdit)
                view.EditIndex = -1;
            else if(e.CancelMode == ListViewCancelMode.CancelingInsert)
                view.InsertItemPosition = InsertItemPosition.None;

            view.DataBind();
        }
        protected void lstInventory_ItemDeleting(object sender, ListViewDeleteEventArgs e)
        {
            ListView view = (ListView)sender;
            int idx = (int)view.DataKeys[e.ItemIndex].Value;

            if (idx > 0)
            {
                //determine if any codes have been used - if so, then we cannot delete this item
                if (Atx.CurrentMerchRecord.IsActivationCodeDelivery)
                {
                    int used = Inventory.GetNumberOfUsedCodes(_Enums.ItemContextCode.m, idx);

                    if (used > 0)
                    {
                        ListViewDataItem viewItem = (ListViewDataItem)view.Items[e.ItemIndex];
                        CustomValidator validation = (CustomValidator)viewItem.FindControl("CustomValidation");
                        _errors.Clear();
                        _errors.Add("This item has used activation codes and may not be deleted. You may remove the remaining available codes and deactivate the item.");

                        if (Utils.Validation.IncurredErrors(_errors, validation))
                        {
                            e.Cancel = true;
                            return;
                        }
                    }
                }

                Merch.Delete(idx);
                Atx.CurrentMerchRecord.ChildMerchRecords().GetList().RemoveAll(
                    delegate(Merch match) { return match.Id == idx; });
            }

            ParentForm.DataBind();
        }
        #endregion

        #region Page Overhead

        protected void Page_Load(object sender, EventArgs e)
        {
            //btnSync.Visible = (Atx.CurrentMerchRecord != null && this.Page.User.IsInRole("Super"));
        }
        
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
        }

        #endregion
       
}
}