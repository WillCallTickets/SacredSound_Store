using System;
using System.Collections;
using System.Web.UI.WebControls;
using System.IO;

using Wcss;

namespace WillCallWeb.Admin.AdminControls
{
    public partial class Merch_ImageUpload : BaseControl
    {
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            //do blocking for upload controls differently
            Atx.RegisterJQueryScript_BlockUIEvents(this.FormView1, "#srceditor", "('.btnupload')", null);
        }
        public override void Dispose()
        {
            //AdminEvent.ItemImageChosen -= new AdminEvent.ItemImageChosenEvent(EventHandler_ItemImageChosen);
            base.Dispose();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GridView1.DataBind();
            }
        }

        #region GridView

        protected void GridView1_DataBinding(object sender, EventArgs e)
        {
            //bind it to the active records item images
            GridView grid = (GridView)sender;

            ItemImageCollection coll = new ItemImageCollection();
            coll.AddRange(Atx.CurrentMerchRecord.ItemImageRecords());
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
                AdminEvent.OnItemImageChosen(this, (int)grid.SelectedDataKey["Id"]);
            }

            FormView1.DataBind();
        }
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridView grid = (GridView)sender;
            GridViewRow row = e.Row;// (GridViewRow)sender;
            ItemImage entity = (ItemImage)row.DataItem;

            if (entity != null)
            {
                //need to do this to force getting image outside of cache
                Image select = (Image)row.FindControl("imgSelect");
                if (select != null)
                {
                    select.ImageUrl = string.Format("{0}", entity.Thumbnail_Small, DateTime.Now.Ticks.ToString());
                }

                //literal dimensions original and current
                Literal dime = (Literal)row.FindControl("LiteralDimension");
                if (dime != null)
                {
                    dime.Text = string.Format("width: {0} height: {1}", entity.ImageWidth, entity.ImageHeight);

                    if (entity.DetailDescription != null && entity.DetailDescription.Trim().Length > 0)
                        dime.Text += string.Format("<br/>{0}", entity.DetailDescription.Trim());
                }

                LinkButton delete = (LinkButton)e.Row.FindControl("btnDelete");
                LinkButton up = (LinkButton)e.Row.FindControl("btnUp");
                LinkButton down = (LinkButton)e.Row.FindControl("btnDown");

                if (delete != null)
                    delete.OnClientClick = string.Format("return confirm('Are you sure you want to delete {0}?')",
                        Utils.ParseHelper.ParseJsAlert(entity.ImageName));

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
                    ItemImage moved = Atx.CurrentMerchRecord.ItemImageRecords().ReorderItem(int.Parse(e.CommandArgument.ToString()), cmd);
                    //set the index of the moved item
                    grid.SelectedIndex = moved.DisplayOrder;
                    grid.DataBind();
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
                Atx.CurrentMerchRecord.ItemImageRecords().DeleteFromCollection(idx);
                grid.SelectedIndex = (Atx.CurrentMerchRecord.ItemImageRecords().Count > 0) ? 0 : -1;
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
            //change index for listeners
            //make sure formview is in edit mode?
            FormView1.ChangeMode(FormViewMode.Edit);
            FormView1.DataBind();
        }

        #endregion

        #region FormView

        protected void FormView1_DataBinding(object sender, EventArgs e)
        {
            FormView form = (FormView)sender;

            int idx = (GridView1.SelectedDataKey != null) ? (int)GridView1.SelectedDataKey["Id"] : 0;

            ItemImageCollection coll = new ItemImageCollection();
            ItemImage addImage = (ItemImage)Atx.CurrentMerchRecord.ItemImageRecords().Find(idx);
            if(addImage != null)
                coll.Add(addImage);

            form.DataSource = coll;
            string[] keyNames = { "Id" };
            form.DataKeyNames = keyNames;
        }
        protected void FormView1_DataBound(object sender, EventArgs e)
        {
            FormView form = (FormView)sender;
            ItemImage entity = (ItemImage)form.DataItem;
            CheckBox chkDetail = (CheckBox)form.FindControl("chkDetail");
            TextBox txtName = (TextBox)form.FindControl("txtName");

            if (txtName != null && entity != null && chkDetail != null && form.CurrentMode == FormViewMode.Edit)
            {
                chkDetail.Enabled = entity.MerchRecord.IsParent;
            }

            RegularExpressionValidator regex = (RegularExpressionValidator)form.FindControl("RegexImageFile");
            if (regex != null)
                regex.ValidationExpression = Utils.Validation.regexValidImageFile.ToString();

            Button btnUpload = (Button)form.FindControl("btnUpload");
            if (btnUpload != null)
            {
                System.Web.UI.ScriptManager mgr = (System.Web.UI.ScriptManager)this.Page.Master.FindControl("ScriptManager1");
                if (mgr != null)
                    mgr.RegisterPostBackControl(btnUpload);
            }

            if (form.CurrentMode == FormViewMode.Insert)
                litImage.Text = string.Empty;
            else
            {
                int maxWidth = 650;

                if (entity != null && entity.Url_Original != null)
                {
                    int width = (entity.ImageWidth > maxWidth) ? maxWidth : entity.ImageWidth;

                    litImage.Text = string.Format("<img src=\"{0}\" border=\"0\" alt=\"\" width=\"{1}\" />",
                        entity.Url_Original, width.ToString());
                }
                else
                    litImage.Text = string.Format("<img src=\"/Images/view.gif\" border=\"0\" alt=\"\" > no image specified", maxWidth.ToString());
            }
        }
        protected void FormView1_ItemUpdating(object sender, FormViewUpdateEventArgs e)
        {
            //validate data
            FormView form = (FormView)sender;
            int idx = (GridView1.SelectedDataKey != null) ? (int)GridView1.SelectedDataKey["Id"] : 0;

            ItemImage entity = (ItemImage)Atx.CurrentMerchRecord.ItemImageRecords().Find(idx);

            //TextBox txtName = (TextBox)form.FindControl("txtName");
            //TextBox txtClass = (TextBox)form.FindControl("txtClass");
            CheckBox chkImage = (CheckBox)form.FindControl("chkImage");
            CheckBox chkDetail = (CheckBox)form.FindControl("chkDetail");
            CheckBox chkThumb = (CheckBox)form.FindControl("chkThumb");
            TextBox txtDescription = (TextBox)form.FindControl("txtDescription");
            
            if (entity != null && chkDetail != null && txtDescription != null)
            {
                //string newName = txtName.Text.Trim();
                //string newClass = txtClass.Text.Trim();
                string newDescription = txtDescription.Text.Trim();

                try
                {
                    if (chkImage.Enabled && chkImage.Checked != entity.IsItemImage)
                        entity.IsItemImage = chkImage.Checked;

                    if (chkDetail.Enabled && chkDetail.Checked != entity.IsDetailImage)
                        entity.IsDetailImage = chkDetail.Checked;

                    if (chkThumb.Enabled && chkThumb.Checked != entity.OverrideThumbnail)
                        entity.OverrideThumbnail = chkThumb.Checked;

                    if (entity.DetailDescription != newDescription)
                        entity.DetailDescription = newDescription;

                    //if (entity.ThumbClass != newClass)
                    //    entity.ThumbClass = newClass;

                    entity.Save();

                    Atx.RefreshCurrentMerchRecord();

                    GridView1.DataBind();
                    
                    e.Cancel = false;
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

                    e.Cancel = true;
                }
                
            }
        }
        protected void FormView1_ItemInserting(object sender, FormViewInsertEventArgs e)
        {
            FormView form = (FormView)sender;
            FileUpload upload = (FileUpload)form.FindControl("FileUpload1");
            Button btnUpload = (Button)form.FindControl("btnUpload");
            CustomValidator validation = (CustomValidator)form.FindControl("CustomFileUpload");

            if (upload != null && upload.HasFile)
            {
                string mappedFile = string.Empty;

                try
                {
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
                    
                    //ensure upload directory is created
                    string division = (Atx.CurrentMerchRecord.IsParent) ? Atx.CurrentMerchRecord.MerchDivisionCollection[0].Name :
                        Atx.CurrentMerchRecord.ParentMerchRecord.MerchDivisionCollection[0].Name;

                    division = division.Replace(" ", string.Empty).Replace("'", string.Empty).Replace("-", "_")
                        .Replace("&", "_");

                    string pathFile = string.Format("{0}{1}", _ImageManager._MerchImageStorage_Local, division);

                    mappedFile = Server.MapPath(pathFile);

                    //see if the directory is valid
                    if (!System.IO.Directory.Exists(mappedFile))
                        System.IO.Directory.CreateDirectory(mappedFile);

                    //now add on the filenames
                    pathFile += string.Format("/{0}", fileName);
                    mappedFile += string.Format("\\{0}", fileName);


                    //now we can check for an existing file....
                    if (System.IO.File.Exists(mappedFile))
                    {
                        fileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + uploadExt;
                        mappedFile = Server.MapPath(string.Format("{0}{1}\\{2}",
                            _ImageManager._MerchImageStorage_Local, division, fileName));
                    }

                    //save the new file
                    upload.SaveAs(mappedFile);



                    //get dimensions 
                    int width, height;
                    using (System.Drawing.Image img = System.Drawing.Image.FromFile(mappedFile))
                    {
                        //insurance
                        if ((!img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Gif)) && (!img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Jpeg))
                             && (!img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Png)))
                            throw new System.NotSupportedException("Valid images must be jpg (jpeg), gif or png only.");

                        if (img.Height < 1 || img.Width < 1)
                            throw new System.ArgumentOutOfRangeException("This image has invalid dimensions. Valid images must be at least 1 pixel wide by 1 pixel high.");

                        width = img.Width;
                        height = img.Height;
                    }

                    //add the image to the collection of images
                    Atx.CurrentMerchRecord.ItemImageRecords().AddToCollection(Atx.CurrentMerchRecord, division, null, fileName, height, width);
                    ItemImage newAddition = Atx.CurrentMerchRecord.ItemImageRecords()[Atx.CurrentMerchRecord.ItemImageRecords().Count - 1];
                    newAddition.ImageManager.CreateAllThumbs();

                    Atx.RefreshCurrentMerchRecord();

                    //reset mode of form
                    form.ChangeMode(FormViewMode.Edit);

                    //set new index - bind grid
                    GridView1.SelectedIndex = Atx.CurrentMerchRecord.ItemImageRecords().Count - 1;
                    GridView1.DataBind();

                    e.Cancel = false;                        
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
                    
                    if (validation != null)
                    {
                        validation.IsValid = false;
                        validation.ErrorMessage = ex.Message;
                    }

                    e.Cancel = true;
                }

                Atx.UnblockUI(this.FormView1, "#srceditor");
            }
        }
        protected void FormView1_ItemCreated(object sender, EventArgs e)
        {
            FormView form = (FormView)sender;

            if (form.CurrentMode == FormViewMode.Edit || form.CurrentMode == FormViewMode.Insert)
            {
                FileUpload upl = (FileUpload)form.FindControl("FileUpload1");
                if(upl != null)
                    upl.Attributes.Add("size", "55");

                RadioButtonList size = (RadioButtonList)form.FindControl("radSize");
                if (size != null)
                {
                    if (size.Items.Count == 0)
                    {
                        ListItem li0 = new ListItem(string.Format("sm({0})", _Config._MerchThumbSizeSm), _Config._MerchThumbSizeSm.ToString());
                        ListItem li1 = new ListItem(string.Format("lg({0})", _Config._MerchThumbSizeLg), _Config._MerchThumbSizeLg.ToString());
                        ListItem li2 = new ListItem("other", "0");

                        li0.Selected = true;

                        size.Items.Add(li0);
                        size.Items.Add(li1);
                        size.Items.Add(li2);
                    }

                    if (size.SelectedIndex == -1)
                        size.SelectedIndex = 0;
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
        protected void CustomFileUpload_ServerValidate(object source, ServerValidateEventArgs args)
        {
            FileUpload upload = (FileUpload)FormView1.FindControl("FileUpload1");
            CustomValidator custom = (CustomValidator)source;

            if (upload != null && upload.HasFile && custom != null)
            {
                string uploadName = upload.FileName;

                if (!upload.HasFile)
                {
                    args.IsValid = false;
                    custom.ErrorMessage += string.Format("<li>Please enter a valid image type.</li>");
                    return;
                }

                args.IsValid = true;
                return;
            }

            args.IsValid = false;
            custom.ErrorMessage += string.Format("<li>The upload controls could not be found or no file was specified.</li>");
        }

        #endregion
    }
}