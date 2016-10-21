using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using Wcss;

namespace WillCallWeb.Admin.AdminControls
{
    public partial class Merch_Bundle : BaseControl
    {
        List<string> _errors = new List<string>();

        #region Page Overhead

        protected void Page_Load(object sender, EventArgs e)
        {
            litTitle.DataBind();

            if (!IsPostBack)
            {
                GridView1.DataBind();
            }
            else
                ListView1.DataBind();
        }

        protected void litTitle_DataBinding(object sender, EventArgs e)
        {

            ((Literal)sender).Text = string.Format("<a href=\"/Admin/MerchEditor.aspx?p=itemedit&merchitem={0}\">{1}</a>", 
                (Atx.CurrentMerchRecord != null) ? Atx.CurrentMerchRecord.Id.ToString() : "0", 
                (Atx.CurrentMerchRecord != null) ? Atx.CurrentMerchRecord.DisplayNameWithAttribs : string.Empty);
        }

        #endregion

        protected MerchBundle GetCurrentBundle()
        {
            return (GridView1.SelectedValue != null) ?
                (MerchBundle)Atx.CurrentMerchRecord.MerchBundleRecords().Find((int)GridView1.SelectedValue) : null;
        }

        protected void ddlQty_DataBinding(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            int start = 1;
            int end = 25;

            if (ddl.ID == "ddlMaxSelection")
            {
                start = 1;
                end = 10;
            }

            if (ddl.Items.Count == 0)
                Utils.ParseHelper.FillListWithNums(ddl, start, end);
        }

        protected void BindMaxSelection(DropDownList ddl, MerchBundle bundle)
        {
            if (ddl == null)
                return;

            int max = (bundle != null) ? bundle.MaxSelections : 1;

            ListItem li = ddl.Items.FindByValue(max.ToString());
            if (li != null)
            {
                li.Selected = true;
            }
        }

        #region GridView
        
        protected void GridView1_DataBinding(object sender, EventArgs e)
        {
            //bind it to the active records item images
            GridView grid = (GridView)sender;

            MerchBundleCollection coll = new MerchBundleCollection();
            coll.AddRange(Atx.CurrentMerchRecord.MerchBundleRecords());
            if (coll.Count > 1)
                coll.Sort("IDisplayOrder", true);

            grid.DataSource = coll;
            string[] keyNames = { "Id" };
            grid.DataKeyNames = keyNames;
        }
        protected void GridView1_DataBound(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;

            if (grid.SelectedIndex == -1 && grid.Rows.Count > 0)
            {
                grid.SelectedIndex = 0;

                //notify everyone we have changed the value
                //AdminEvent.OnItemImageChosen(this, (int)grid.SelectedDataKey["Id"]);
            }

            ListView1.DataBind();
        }
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridView grid = (GridView)sender;
            GridViewRow row = e.Row;// (GridViewRow)sender;
            MerchBundle entity = (MerchBundle)row.DataItem;

            if (entity != null)
            {
                DropDownList ddlParentQty = (DropDownList)row.FindControl("ddlParentQty");
                if (ddlParentQty != null)//edit mode
                {
                    ListItem li = ddlParentQty.Items.FindByValue(entity.RequiredParentQty.ToString());
                    if (li != null)
                        li.Selected = true;
                    else
                        ddlParentQty.SelectedIndex = 0;
                }

                DropDownList ddlMaxSelection = (DropDownList)row.FindControl("ddlMaxSelection");
                BindMaxSelection(ddlMaxSelection, entity);

                LinkButton delete = (LinkButton)e.Row.FindControl("btnDelete");
                LinkButton up = (LinkButton)e.Row.FindControl("btnUp");
                LinkButton down = (LinkButton)e.Row.FindControl("btnDown");

                if (delete != null)
                    delete.OnClientClick = string.Format("return confirm('Are you sure you want to delete {0}?')",
                        Utils.ParseHelper.ParseJsAlert(entity.Title));

                if (up != null && down != null)
                {
                    up.Enabled = (e.Row.RowIndex > 0);
                    down.Enabled = (e.Row.RowIndex < (((ICollection)grid.DataSource).Count - 1));
                }
            }
        }
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridView grid = (GridView)sender;
            string cmd = e.CommandName.ToLower();

            switch (cmd)
            {
                case "up":
                case "down":
                    MerchBundle moved = Atx.CurrentMerchRecord.MerchBundleRecords().ReorderItem(int.Parse(e.CommandArgument.ToString()), cmd);
                    //set the index of the moved item
                    grid.SelectedIndex = moved.DisplayOrder;
                    grid.DataBind();
                    break;
                case "new":
                    grid.DataBind();
                    FormView1.Visible = true;
                    FormView1.DataBind();
                    this.updPnlCustomerDetail.Update();
                    this.mdlPopup.Show();
                    break;
            }
        }
        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridView grid = (GridView)sender;
            int idx = (int)grid.DataKeys[e.RowIndex]["Id"];

            try
            {
                //call collection method to ensure reordering and thumb deletion
                Atx.CurrentMerchRecord.MerchBundleRecords().DeleteFromCollection(idx);
                grid.SelectedIndex = (Atx.CurrentMerchRecord.MerchBundleRecords().Count > 0) ? 0 : -1;
                grid.DataBind();
            }
            catch (Exception ex)
            {
                _Error.LogException(ex);

                CustomValidator validation = (CustomValidator)grid.FindControl("CustomValidation");
                if (validation != null)
                {
                    validation.IsValid = false;
                    validation.ErrorMessage = ex.Message;
                }

                e.Cancel = true;
            }
        }
        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;
            grid.DataBind();
        }
        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView grid = (GridView)sender;

            grid.EditIndex = e.NewEditIndex;
            grid.SelectedIndex = grid.EditIndex;

            grid.DataBind();
        }
        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView grid = (GridView)sender;

            grid.EditIndex = -1;

            grid.DataBind();
        }
        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridView grid = (GridView)sender;
            if(grid.EditIndex == -1)
            {
                e.Cancel = false;
                return;
            }

            //int idx = (int)grid.DataKeys[e.RowIndex]["Id"];
            int idx = (int)grid.DataKeys[grid.EditIndex]["Id"];
            MerchBundle entity = (MerchBundle)Atx.CurrentMerchRecord.MerchBundleRecords().Find(idx);
            
            _errors.Clear();

            if (entity != null)
            {
                GridViewRow gvr = grid.Rows[e.RowIndex];
                CustomValidator validation = (CustomValidator)gvr.FindControl("CustomValidation");

                CheckBox chkActive = (CheckBox)gvr.FindControl("chkActive");
                CheckBox chkWeight = (CheckBox)gvr.FindControl("chkWeight");
                CheckBox chkPriced = (CheckBox)gvr.FindControl("chkPriced");

                TextBox txtTitle = (TextBox)gvr.FindControl("txtTitle");
                string title = txtTitle.Text.Trim();
                Utils.Validation.ValidateRequiredField(_errors, "Title", title);

                TextBox txtComment = (TextBox)gvr.FindControl("txtComment");
                string comment = txtComment.Text.Trim();

                DropDownList ddlParentQty = (DropDownList)gvr.FindControl("ddlParentQty");
                int parentQty = int.Parse(ddlParentQty.SelectedValue);

                DropDownList ddlMaxSelection = (DropDownList)gvr.FindControl("ddlMaxSelection");
                int maxSelection = int.Parse(ddlMaxSelection.SelectedValue);

                TextBox txtPrice = (TextBox)gvr.FindControl("txtPrice");
                string inputPrice = txtPrice.Text.Trim();
                if (inputPrice.Trim().Length == 0)
                    inputPrice = "0";
                decimal price = (inputPrice.Length > 0) ? decimal.Parse(inputPrice) : 0;

                if (Utils.Validation.IncurredErrors(_errors, validation))
                {
                    e.Cancel = true;
                    return;
                }
                

                try
                {
                    entity.IsActive = chkActive.Checked;
                    entity.Title = title;
                    entity.Comment = comment;
                    entity.IncludeWeight = chkWeight.Checked;
                    entity.RequiredParentQty = parentQty;
                    entity.Price = price;
                    entity.PricedPerSelection = chkPriced.Checked;
                    entity.MaxSelections = maxSelection;
                    entity.Save();

                    Atx.RefreshCurrentMerchRecord();

                    grid.EditIndex = -1;
                    grid.DataBind();

                    e.Cancel = false;
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
            }
            
            e.Cancel = false;
        }

        #endregion

        #region FormView

        protected void btnFormView1Save_Click(object sender, EventArgs e)
        {
            FormView1.InsertItem(false);

            if (CustomValidation.ErrorMessage.Trim().Length > 0)
            {
                this.mdlPopup.Show();
            }
        }

        protected void FormView1_DataBinding(object sender, EventArgs e)
        {
            FormView form = (FormView)sender;

            int idx = (GridView1.SelectedValue != null) ? (int)GridView1.SelectedValue : 0;

            if (idx > 0)
            {
                MerchBundleCollection coll = new MerchBundleCollection();
                MerchBundle pkg = (MerchBundle)Atx.CurrentMerchRecord.MerchBundleRecords().Find(idx);
                if (pkg != null)
                    coll.Add(pkg);

                form.DataSource = coll;
                string[] keyNames = { "Id" };
                form.DataKeyNames = keyNames;
            }
        }
        protected void FormView1_DataBound(object sender, EventArgs e)
        {
            FormView form = (FormView)sender;
            MerchBundle entity = (MerchBundle)form.DataItem;

            DropDownList ddlMaxSelections = (DropDownList)form.FindControl("ddlMaxSelections");
            if (ddlMaxSelections != null)
            { 
            }

            DropDownList ddlParentQty = (DropDownList)form.FindControl("ddlParentQty");
            if (ddlParentQty != null)
            {
            }

            /* example of some of the voodo needed to handle uploads
            Button btnUpload = (Button)form.FindControl("btnUpload");
            if (btnUpload != null)
            {
                System.Web.UI.ScriptManager mgr = (System.Web.UI.ScriptManager)this.Page.Master.FindControl("ScriptManager1");
                if (mgr != null)
                    mgr.RegisterPostBackControl(btnUpload);
            }*/
        }
        protected void FormView1_ItemInserting(object sender, FormViewInsertEventArgs e)
        {
            FormView form = (FormView)sender;
            CustomValidator validation = CustomValidation;// (CustomValidator)form.FindControl("CustomFileUpload");
            validation.ErrorMessage = string.Empty;

            //comment, active, maxselections, parent qty, price
            TextBox txtTitle = (TextBox)form.FindControl("txtTitle");
            string title = txtTitle.Text.Trim();
            Utils.Validation.ValidateRequiredField(_errors, "Title", title);

            TextBox txtComment = (TextBox)form.FindControl("txtComment");
            string comment = txtComment.Text.Trim();

            //CheckBox chkActive = (CheckBox)form.FindControl("chkActive");
            //bool active = chkActive.Checked;

            DropDownList ddlMaxSelection = (DropDownList)form.FindControl("ddlMaxSelection");
            int maxSelection = int.Parse(ddlMaxSelection.SelectedValue);
            if (maxSelection == -1)
                maxSelection = 0;

            DropDownList ddlParentQty = (DropDownList)form.FindControl("ddlParentQty");
            int parentQty = int.Parse(ddlParentQty.SelectedValue);
            if (parentQty == -1)
                parentQty = 0;

            string price = string.Empty;
            TextBox txtPrice = (TextBox)form.FindControl("txtPrice");
            if (txtPrice != null)
                price = txtPrice.Text.Trim();

            Utils.Validation.ValidateNumericField(_errors, "Price", price);
            decimal dPrice = (price.Length == 0) ? 0 : decimal.Parse(price);
            if (dPrice < 0)
                _errors.Add("Price must be zero or greater.");

            //handle errors
            if (Utils.Validation.IncurredErrors(_errors, validation))
            {
                e.Cancel = true;
                updPnlCustomerDetail.Update();
                return;
            }

            CheckBox chkPriced = (CheckBox)form.FindControl("chkPriced");

            try
            {
                MerchBundle entity = Atx.CurrentMerchRecord.MerchBundleRecords().AddToCollection(Atx.CurrentMerchRecord, parentQty, title, comment, dPrice, chkPriced.Checked, maxSelection);
                    
                Atx.RefreshCurrentMerchRecord();

                //reset mode of form
                form.Visible = false;
                this.mdlPopup.Hide();

                //set new index - bind grid
                GridView1.SelectedIndex = Atx.CurrentMerchRecord.MerchBundleRecords().Count - 1;                
                GridView1.DataBind();
                updatePanel.Update();

                e.Cancel = false;               
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

        }
        protected void FormView1_ItemCreated(object sender, EventArgs e)
        {
            //FormView form = (FormView)sender;

            //if (form.CurrentMode == FormViewMode.Edit || form.CurrentMode == FormViewMode.Insert)
            //{

            //}
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
            FormView form = (FormView)sender;
            string cmd = e.CommandName.ToLower();

            switch (cmd)
            {
                case "cancel":
                    //just rebind the control to reset
                    //if we are cancelling an insert in new child mode
                    //form.ChangeMode(FormViewMode.Edit);
                    break;
            }
        }

        #endregion

        #region Inventory Listing

        protected void chkActive_CheckChanged(object sender, EventArgs e)
        {
            CheckBox chk = (CheckBox)sender;

            //get the item
            int rowIdx = ((ListViewDataItem)chk.NamingContainer).DataItemIndex;
            int bundleItemIdx = (int)ListView1.DataKeys[rowIdx].Value;

            MerchBundle bundle = GetCurrentBundle();
            if (bundle != null)
            {
                MerchBundleItem bundleItem = (MerchBundleItem)bundle.MerchBundleItemRecords().Find(bundleItemIdx);

                if (bundleItem != null)
                {
                    bundleItem.IsActive = chk.Checked;
                    bundleItem.Save();

                    //refresh data
                    int idx = Atx.CurrentMerchRecord.Id;
                    Atx.Clear_CurrentMerchListing();
                    Atx.SetCurrentMerchRecord(idx);

                    GridView1.DataBind();
                }
            }
        }
        protected void rptInventory_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Repeater rpt = (Repeater)sender;
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.EditItem ||
                e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.SelectedItem)
            {
                Merch item = (Merch)e.Item.DataItem;
                Literal litProductName = (Literal)e.Item.FindControl("litProductName");
                if (item != null && litProductName != null)
                {
                    litProductName.Text = item.DisplayNameWithAttribs;
                }
            }
        }
        protected void ListView1_DataBinding(object sender, EventArgs e)
        {
            ListView view = (ListView)sender;
            MerchBundleItemCollection coll = new MerchBundleItemCollection();

            if (GridView1.SelectedDataKey != null)
            {
                //get selected grid row
                int idx = (int)GridView1.SelectedDataKey["Id"];

                if (idx != -1)
                {
                    MerchBundle bundle = (MerchBundle)Atx.CurrentMerchRecord.MerchBundleRecords().Find(idx);
                    coll.AddRange(bundle.MerchBundleItemRecords());

                    if (coll.Count > 1)
                        coll.Sort("IDisplayOrder", true);
                }

                view.InsertItemPosition = InsertItemPosition.LastItem;
            }

            view.DataSource = coll;
            string[] keyNames = { "Id" };
            view.DataKeyNames = keyNames;
            
            if (coll.Count > 0 && view.SelectedIndex == -1)
                view.SelectedIndex = 0;
        }
        protected void ListView1_DataBound(object sender, EventArgs e)
        {
            ListView list = (ListView)sender;


        }
        protected void ListView1_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            ListView view = (ListView)sender;
            ListViewDataItem viewItem = (ListViewDataItem)e.Item;
            MerchBundleItem entity = (MerchBundleItem)viewItem.DataItem;

            Repeater rpt = (Repeater)viewItem.FindControl("rptInventory");
            MerchCollection coll = new MerchCollection();
            if (entity.MerchRecord.IsParent)
                coll.AddRange(entity.MerchRecord.ChildMerchRecords_Active);
            else
                coll.Add(entity.MerchRecord);

            rpt.DataSource = coll;
            rpt.DataBind();
            

            LinkButton delete = (LinkButton)viewItem.FindControl("btnDelete");
            LinkButton up = (LinkButton)viewItem.FindControl("btnUp");
            LinkButton down = (LinkButton)viewItem.FindControl("btnDown");

            if (delete != null)
                delete.OnClientClick = string.Format("return confirm('Are you sure you want to delete {0}?')",
                    Utils.ParseHelper.ParseJsAlert(entity.MerchRecord.DisplayNameWithAttribs));

            if (up != null && down != null)
            {
                up.Enabled = (viewItem.DataItemIndex > 0);
                down.Enabled = (viewItem.DataItemIndex < (((ICollection)view.DataSource).Count - 1));
            }

        }
        protected void ListView1_ItemInserting(object sender, ListViewInsertEventArgs e)
        {
            ListView view = (ListView)sender;
            ListViewItem viewItem = (ListViewItem)e.Item;
            CustomValidator validation = (CustomValidator)viewItem.FindControl("CustomValidation");
            _errors.Clear();

            //2 things can happen
            //either nothing was chosen
            //or we have a duplicate
            DropDownList ddlMerch = (DropDownList)viewItem.FindControl("ddlMerch");
            if (ddlMerch.SelectedIndex <= 0)//allow for "select"
                _errors.Add("Please select a merchandise item.");

            int tMerchId = int.Parse(ddlMerch.SelectedValue);
            Merch merch = new Merch(tMerchId);

            //get parent bundle and add new item to collection
            int bundleId = (int)GridView1.SelectedValue;
            MerchBundle bundle = (MerchBundle)Atx.CurrentMerchRecord.MerchBundleRecords().Find(bundleId);

            int exists = bundle.MerchBundleItemRecords().GetList().FindIndex(delegate(MerchBundleItem match) { return match.TMerchId == tMerchId; });
            if(exists != -1)
                _errors.Add("The item you have selected is already in the item collection.");

            if (Utils.Validation.IncurredErrors(_errors, validation))
            {
                e.Cancel = true;
                return;
            }
            //end of input validation


            try
            {
                MerchBundleItem newItem = bundle.MerchBundleItemRecords().AddToCollection(bundle, tMerchId);

                //refresh data
                int idx = Atx.CurrentMerchRecord.Id;
                Atx.Clear_CurrentMerchListing();
                Atx.SetCurrentMerchRecord(idx);

                view.SelectedIndex = newItem.DisplayOrder;

                //reset insert index
                //view.InsertItemPosition = InsertItemPosition.None;

                GridView1.DataBind();
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
        protected void ListView1_ItemDeleting(object sender, ListViewDeleteEventArgs e)
        {
            ListView list = (ListView)sender;

            int bundleItemIdx = (int)ListView1.DataKeys[e.ItemIndex].Value;

            MerchBundle bundle = GetCurrentBundle();
            if (bundle != null)
            {
                bundle.MerchBundleItemRecords().DeleteFromCollection(bundleItemIdx);

                //refresh data
                int idx = Atx.CurrentMerchRecord.Id;
                Atx.Clear_CurrentMerchListing();
                Atx.SetCurrentMerchRecord(idx);

                GridView1.DataBind();
            }
        }
        protected void ListView1_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            ListView view = (ListView)sender;
            string cmd = e.CommandName.ToLower();
            switch (cmd)
            {
                case "up":
                case "down":
                    MerchBundle bundle = (MerchBundle)Atx.CurrentMerchRecord.MerchBundleRecords().Find((int)GridView1.SelectedValue);
                    MerchBundleItem moved = bundle.MerchBundleItemRecords().ReorderItem(int.Parse(e.CommandArgument.ToString()), cmd);
                    //set the index of the moved item
                    view.SelectedIndex = moved.DisplayOrder;
                    view.DataBind();
                    break;
            }
        }

        #endregion
    }
}