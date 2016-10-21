using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using Wcss;

namespace WillCallWeb.Admin.AdminControls.Downloads
{
    public partial class Download_Item : BaseControl
    {
        List<string> _errors = new List<string>();

        #region Page Overhead

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            //load up insert mode if we have an id of 0
            if (Atx.CurrentDownloadRecord == null)
                FormView1.ChangeMode(FormViewMode.Insert);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FormView1.DataBind();
            }
        }

        #endregion

        protected void btnEditor_Click(object sender, EventArgs e)
        {
            //Button btn = (Button)sender;
            //string name = btn.ID.ToLower();
            //switch (name)
            //{
            //    case "btneditcolor":
            //        base.Redirect("/Admin/EntityEditor.aspx?p=color");
            //        break;
            //    case "btneditsize":
            //        base.Redirect("/Admin/EntityEditor.aspx?p=size");
            //        break;
            //}
        }

        #region FormView1

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
            //lstInventory.InsertItemPosition = InsertItemPosition.None;

            FormView1.DeleteItem();
            FormView1.DataBind();
        }
        protected void btnNew_Click(object sender, EventArgs e)
        {
            FormView1.ChangeMode(FormViewMode.Insert);
        }
        protected void FormView1_ModeChanging(object sender, FormViewModeEventArgs e)
        {
            FormView form = (FormView)sender;
            form.ChangeMode(e.NewMode);
            if (e.CancelingEdit)
                form.DataBind();
        }
        protected void FormView1_DataBinding(object sender, EventArgs e)
        {   
            FormView form = (FormView)sender;

            bool editMode = (form.CurrentMode == FormViewMode.Edit);
            btnSave.CommandName =  editMode ? "Update" : "Insert";
            //cancel always the same
            btnDelete.Visible = editMode;
            if (Atx.CurrentDownloadRecord != null)
                btnDelete.OnClientClick = string.Format("return confirm('Are you sure you want to delete {0}?')", 
                    Utils.ParseHelper.ParseJsAlert(Atx.CurrentDownloadRecord.FileName));

            btnNew.Visible = editMode;

            DownloadCollection coll = new DownloadCollection();
            if (Atx.CurrentDownloadRecord != null)
                coll.Add(Atx.CurrentDownloadRecord);

            form.DataSource = coll;
        }
        protected void FormView1_DataBound(object sender, EventArgs e)
        {
            FormView form = (FormView)sender;
            Download entity = (Download)form.DataItem;
        }
        protected void FormView1_ItemCommand(object sender, FormViewCommandEventArgs e)
        {
            FormView form = (FormView)sender;
            Download entity = Atx.CurrentDownloadRecord;
            string cmd = e.CommandName.ToLower();

            switch (cmd)
            {
                case "refresh":
                    int idx = Atx.CurrentMerchRecord.Id;
                    Atx.Clear_CurrentDownloadListing();
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
            Download entity = Atx.CurrentDownloadRecord;

            if (entity != null)
            {
                try
                {
                    //bool activeStatusChanged = false;

                    TextBox txtName = (TextBox)form.FindControl("txtName");
                    Utils.Validation.ValidateRequiredField(_errors, "Name", txtName.Text.Trim());

                    TextBox txtShort = (TextBox)form.FindControl("txtShortDescription");

                    //if there are errors....
                    CustomValidator validation = (CustomValidator)form.FindControl("CustomValidation");

                    if (Utils.Validation.IncurredErrors(_errors, validation))
                    {
                        e.Cancel = true;
                        return;
                    }


                    //configure input

                    //Save parent and potential children
                    //entity.Save_AvoidRealTimeVars();

                    //if the file was moved - record the move


                    //we may need to update data
                    int index = entity.Id;
                    //Atx.Clear_CurrentDownloadListing();???
                    Atx.SetCurrentDownloadRecord(index);

                    //update any lookups

                    form.DataBind();
                }
                catch (Exception ex)
                {
                    _Error.LogException(ex);
                    CustomValidator custom = (CustomValidator)form.FindControl("CustomValidation");

                    if (custom != null)
                    {
                        custom.IsValid = false;
                        custom.ErrorMessage = ex.Message;
                    }

                    e.Cancel = true;
                }
            }
        }

        #endregion

        #region Inserting

        protected void FormView1_ItemInserting(object sender, FormViewInsertEventArgs e)
        {
            FormView form = (FormView)sender;
            _errors.Clear();

            try
            {
                //validate
                CustomValidator validation = CustomValidation;

                if (Utils.Validation.IncurredErrors(_errors, validation))
                {
                    e.Cancel = true;
                    return;
                }
                
                //insert genres, etc?
                
                Download newItem = new Download();
                newItem.ApplicationId = _Config.APPLICATION_ID;
                newItem.DtStamp = DateTime.Now;


                newItem.Save();//ok to use save here - insert is only dirty row

                //refresh any lookups
                //_Lookits.RefreshLookup(_Enums.LookupTableNames.MerchDivisions.ToString());
                //_Lookits.RefreshLookup(_Enums.LookupTableNames.MerchCategories.ToString());


                //Atx.CurrentMerchListing.Add(newItem);

                Atx.SetCurrentDownloadRecord(newItem.Id);

                form.ChangeMode(FormViewMode.Edit);//automatically binds the form

                //redirect to set all vars
                base.Redirect(string.Format("/Admin/DownloadEditor.aspx?p=Downloads&downloaditem={0}", Atx.CurrentDownloadRecord.Id));

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
                Download entity = Atx.CurrentDownloadRecord;

                //get item sales
                //if (entity.Sold > 0)
                //    throw new Exception("Item has sales and cannot be deleted. Please mark as not active.");

                //delete any related info
                //foreach (Merch child in entity.ChildMerchRecords())
                //    Merch.Delete(child.Id);

                Download.Delete(entity.Id);

                //refresh in-memory collection
                //Atx.CurrentMerchListing.Remove(entity);

                //update the entity in the division lookups
                //_Lookits.RefreshLookup(_Enums.LookupTableNames.MerchDivisions.ToString());

                Atx.Clear_CurrentDownloadListing();

                //if (entity.IsChild)
                //    base.Redirect(string.Format("/Admin/MerchEditor.aspx?p=ItemEdit&merchitem={0}", entity.ParentMerchRecord.Id));

                base.Redirect("/Admin/DownloadEditor.aspx");
            }
            catch (Exception ex)
            {
                _Error.LogException(ex);
                CustomValidator custom = (CustomValidator)form.FindControl("CustomValidation");

                if (custom != null)
                {
                    custom.IsValid = false;
                    custom.ErrorMessage = ex.Message;
                }
            }
        }

        #endregion

        #endregion

}
}