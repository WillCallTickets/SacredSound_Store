using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.ComponentModel;

using Wcss;

namespace WillCallWeb.Admin.AdminControls
{
    [ToolboxData("<{0}:Editor_Venue runat=\"Server\" AbbreviatedDisplay=\"false\" DisplayTitle=\"true\" TitleText=\"VENUE EDITOR\" SelectText=\"SELECT VENUE\" ></{0}:Editor_Venue>")]
    public partial class Editor_Venue : BaseControl
    {
        #region Wizard

        protected void OnNextStep(object sender, WizardNavigationEventArgs e)
        {
            if (e.NextStepIndex > 0 && Entity == null)
            {
                e.Cancel = true;
                return;
            }
        }
        protected void wizEdit_ActiveStepChanged(object sender, EventArgs e)
        {
            Wizard wiz = (Wizard)sender;
            switch(wiz.ActiveStep.ID.ToLower())
            {
                case "editor":
                    FormView1.DataBind();
                    break;
                case "images":
                    txtImageName.Text = Entity.Name;
                    litImage.DataBind();
                    break;
            }
        }
        protected void SideBarList_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            //no items should be enabled when the entity is null
            Button sideBarButton = (Button)e.Item.FindControl("SideBarButton");

            if (sideBarButton != null)
            {
                WizardStep wStep = (WizardStep)e.Item.DataItem;
                string activeName = wizEdit.ActiveStep.Name.ToLower();

                sideBarButton.Enabled = (Entity != null && wStep.Name.ToLower() != activeName);
            }
            //items should not be enabled when on like-page
        }

        #endregion

        #region Ajax selection

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            string val = hidSelectedValue.Value;
            if (Utils.Validation.IsInteger(val))
                Atx.CurrentVenueId = int.Parse(val);
            else
                Atx.CurrentVenueId = 0;

            OnSelectedVenueChanged(new AdminEvent.EditorEntityChangedEventArgs(Atx.CurrentVenueId, ""));

            txtSelection.Text = string.Empty;
            hidSelectedValue.Value = "0";

            FormView1.ChangeMode(FormViewMode.ReadOnly);
            FormView1.DataBind();
            litImage.DataBind();
            //GridViewListing.DataBind();
        }
        private static readonly object SelectedEditVenueChangedEventKey = new object();

        public delegate void SelectedEditVenueChangedEventHandler(object sender, WillCallWeb.Admin.AdminEvent.EditorEntityChangedEventArgs e);

        public event SelectedEditVenueChangedEventHandler SelectedVenueChanged
        {
            add { Events.AddHandler(SelectedEditVenueChangedEventKey, value); }
            remove { Events.RemoveHandler(SelectedEditVenueChangedEventKey, value); }
        }
        public virtual void OnSelectedVenueChanged(WillCallWeb.Admin.AdminEvent.EditorEntityChangedEventArgs e)
        {
            SelectedEditVenueChangedEventHandler handler = (SelectedEditVenueChangedEventHandler)Events[SelectedEditVenueChangedEventKey];

            if (handler != null)
                handler(this, e);
        }

        #endregion

        #region Imagery

        protected void btnThumbnails_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            //if we have an image
            if (Entity != null && Entity.PictureUrl != null && Entity.PictureUrl.Trim().Length > 0)
            {
                //ensure the file exists
                if (!File.Exists(Server.MapPath(Entity.ImageManager.OriginalUrl)))
                {
                    Entity.ImageManager.Delete();
                    Entity.PicHeight = 0;
                    Entity.PicWidth = 0;
                    Entity.PictureUrl = null;

                    lblAlert.Text = "The image does not exist and has been cleared.";
                    lblAlert.ForeColor = System.Drawing.Color.Red;
                    return;
                }

                //delete any existing thumbs
                Entity.ImageManager.DeleteThumbnails();

                //recreate the thumbnails
                Entity.ImageManager.CreateAllThumbs();

                lblAlert.Text = "Image thumbnails have been rebuilt.";
                lblAlert.ForeColor = System.Drawing.Color.Green;

                litImage.DataBind();
            }
        }
        protected void litImageThumb_DataBinding(object sender, EventArgs e)
        {
            if (wizEdit.ActiveStep.ID.ToLower() == "editor")
            {
                Literal lit = (Literal)sender;
                
                lit.Text = string.Format("<img src=\"/Images/spacer.gif\" alt=\"\" width=\"{0}px\" height=\"1px\" />", MaxImageDimension.ToString());

                bool entityHasPicture = (Entity != null && Entity.PictureUrl != null && Entity.PictureUrl.Trim().Length > 0);

                if (entityHasPicture)
                {
                    int width = MaxImageDimension;// (Entity.PicWidth < _Config._ActThumbSizeLg) ? Entity.PicWidth : _Config._ActThumbSizeLg;
                    lit.Text += string.Format("<div><img src=\"{0}\" alt=\"\" width=\"{1}px\" /></div>", 
                        Entity.ImageManager.Thumbnail_Max,
                        width.ToString());

                    btnClear.Visible = true;
                }
            }
        }
        protected void litImage_DataBinding(object sender, EventArgs e)
        {
            if (wizEdit.ActiveStep.ID.ToLower() == "images")
            {
                Literal lit = (Literal)sender;

                btnClear.Visible = false;

                //if we are in the IMAGE wizard step - allow jcrop jquery control
                bool useJcrop = true;
                string jcropId = string.Empty;

                if (useJcrop)
                    jcropId = string.Format("id=\"jcropper-ven\" ");

                lit.Text = string.Format("<img src=\"/Images/spacer.gif\" alt=\"\" width=\"{0}px\" height=\"1px\" />", _Config._VenueThumbSizeMax.ToString());

                bool entityHasPicture = (Entity != null && Entity.PictureUrl != null && Entity.PictureUrl.Trim().Length > 0);
                if (entityHasPicture)
                {
                    int width = (Entity.PicWidth < _Config._VenueThumbSizeMax) ? Entity.PicWidth : _Config._VenueThumbSizeMax;
                    lit.Text += string.Format("<div><img src=\"{0}\" {1}alt=\"\" width=\"{2}px\" /></div>", Entity.ImageManager.Thumbnail_Max, jcropId,
                        width.ToString());

                    btnClear.Visible = true;
                }

                //crop controls
                pnlInstruction.Visible = entityHasPicture;
            }
        }
        protected void btnCrop_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            switch (btn.CommandName.ToLower())
            {
                case "savecrop":
                    //can we get values from request string?
                    if (Entity != null && Entity.PictureUrl != null && Entity.PictureUrl.Trim().Length > 0)
                    {
                        string imageToCrop = Entity.ImageManager.OriginalUrl;
                        string mappedImageToCrop = Server.MapPath(imageToCrop);
                        string fileNameToCrop = Path.GetFileName(imageToCrop);

                        string w1 = Request["w1"];
                        string h1 = Request["h1"];
                        string x1 = Request["x1"];
                        string y1 = Request["y1"];

                        if (w1 != null && h1 != null && x1 != null && y1 != null
                            && w1.Trim().Length > 0 && h1.Trim().Length > 0 && x1.Trim().Length > 0 && y1.Trim().Length > 0)
                        {
                            //get the dims of the image being viewed
                            if (litImage.Text.Trim() == string.Empty)
                                litImage.DataBind();
                            if (litImage.Text.Trim() == string.Empty)
                            {
                                _Error.LogException(new ArgumentNullException("litImage has no file name."));
                                return;
                            }
                            string imageText = litImage.Text.ToLower();

                            string pathCompare = Path.GetDirectoryName(imageToCrop).ToLower().Replace("\\", "/");
                            int start = imageText.IndexOf(pathCompare);
                            int end = imageText.IndexOf(fileNameToCrop.ToLower(), start + pathCompare.Length);

                            string imageBeingViewed = string.Empty;

                            if (start > -1 && end > -1)
                                imageBeingViewed = string.Format("{0}{1}", imageText.Substring(start, end - start), fileNameToCrop);
                            else
                                //default to max thumbnail
                                imageBeingViewed = Entity.Thumbnail_Max;

                            System.Web.UI.Pair vw = Utils.ImageTools.GetDimensions(Server.MapPath(imageBeingViewed));
                            int viewedWidth = (int)vw.First;
                            //int viewedHeight = (int)vw.Second;

                            System.Web.UI.Pair ic = Utils.ImageTools.GetDimensions(mappedImageToCrop);
                            int orgWidth = (int)ic.First;
                            //int orgHeight = (int)ic.Second;

                            double conversionRatio = (double)orgWidth / viewedWidth;

                            int w = Convert.ToInt32(Request["w1"]);
                            int h = Convert.ToInt32(Request["h1"]);
                            int x = Convert.ToInt32(Request["x1"]);
                            int y = Convert.ToInt32(Request["y1"]);

                            w = (int)Math.Floor(w * conversionRatio);
                            h = (int)Math.Floor(h * conversionRatio);
                            x = (int)Math.Floor(x * conversionRatio);
                            y = (int)Math.Floor(y * conversionRatio);

                            byte[] CropImage = Utils.ImageTools.Crop(mappedImageToCrop, w, h, x, y);

                            string croppedFile = string.Empty;

                            using (MemoryStream ms = new MemoryStream(CropImage, 0, CropImage.Length))
                            {
                                ms.Write(CropImage, 0, CropImage.Length);

                                using (System.Drawing.Image CroppedImage = System.Drawing.Image.FromStream(ms, true))
                                {
                                    //save the image to the ... directory
                                    croppedFile = string.Format("{0}\\", Path.GetDirectoryName(mappedImageToCrop));
                                    croppedFile += Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + Path.GetExtension(imageToCrop);

                                    CroppedImage.Save(croppedFile, CroppedImage.RawFormat);
                                }
                            }

                            //move the orginal image to a cropped folder
                            string dirToWrite = string.Format("{0}\\cropped", Path.GetDirectoryName(mappedImageToCrop));
                            if (!Directory.Exists(dirToWrite))
                                Directory.CreateDirectory(dirToWrite);

                            File.Copy(mappedImageToCrop, string.Format("{0}\\{1}", dirToWrite, Path.GetFileName(mappedImageToCrop)), true);

                            if (Entity.ImageManager != null) 
                                Entity.ImageManager.Delete();

                            Entity.PictureUrl = Path.GetFileName(croppedFile);

                            //set dimensions for new pic - entity
                            try
                            {
                                System.Web.UI.Pair p2 = Utils.ImageTools.GetDimensions(croppedFile);
                                Entity.PicWidth = (int)p2.First;
                                Entity.PicHeight = (int)p2.Second;
                            }
                            catch (Exception ex)
                            {
                                _Error.LogException(ex);

                                Entity.PicWidth = 0;
                                Entity.PicHeight = 0;
                            }

                            //save the entity
                            Entity.Save();

                            //create thumbs
                            Entity.ImageManager.CreateAllThumbs();

                            litImage.DataBind();
                        }
                    }

                    break;
                case "cancelcrop":
                    //reset crop dimensions
                    break;
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            if (Entity.ImageManager != null) 
                Entity.ImageManager.Delete();

            Entity.PictureUrl = null;
            Entity.PicWidth = 0;
            Entity.PicHeight = 0;
            Entity.Save();

            litImage.DataBind();
        }

        #endregion

        #region Properties

        public bool AllowSelect
        {
            get { return pnlSelect.Visible; }
            set
            {
                pnlSelect.Visible = value;
            }
        }
        //save the id and name of the current ... into object state
        public int SelectedIdx
        {
            get
            {
                return Atx.CurrentVenueId;
            }
            set
            {
                Atx.CurrentVenueId = value;
            }
        }
        protected Venue _venue = null;
        protected Venue Entity
        {
            get
            {
                if (_venue == null && SelectedIdx > 0 || (_venue != null && _venue.Id != SelectedIdx))
                {
                    if (SelectedIdx == 0)
                        _venue = null;
                    else
                    {
                        _venue = Venue.FetchByID(SelectedIdx);

                        if (_venue != null && _venue.ApplicationId != _Config.APPLICATION_ID)
                        {
                            SelectedIdx = 0;
                            _venue = null;
                        }
                    }
                }

                return _venue;
            }
        }
        public string SelectedName
        {
            get
            {
                TextBox txt = (TextBox)FormView1.FindControl("NameTextBox");
                return (txt != null) ? txt.Text.Trim() : string.Empty;
            }
        }
        private bool _abbreviatedDisplay = false;
        public bool AbbreviatedDisplay
        {
            get
            {
                return _abbreviatedDisplay;
            }
            set
            {
                _abbreviatedDisplay = value;
            }
        }
        private bool _displayTitle = true;
        public bool DisplayTitle
        {
            get
            {
                return _displayTitle;
            }
            set
            {
                _displayTitle = value;
            }
        }
        private string _titleText = "VENUE EDITOR";
        public string TitleText
        {
            get
            {
                return _titleText;
            }
            set
            {
                _titleText = value;
            }
        }
        private string _selectText = "SELECT VENUE";
        public string SelectText
        {
            get
            {
                return _selectText;
            }
            set
            {
                _selectText = value;
            }
        }
        private int _maxImageDimension = 150;
        public int MaxImageDimension
        {
            get
            {
                return _maxImageDimension;
            }
            set
            {
                _maxImageDimension = value;
            }
        }
        protected override object SaveControlState()
        {
            object[] ctlState = new object[5];
            ctlState[0] = base.SaveControlState();
            ctlState[1] = this._abbreviatedDisplay;
            ctlState[2] = this._displayTitle;
            ctlState[3] = this._titleText;
            ctlState[4] = this._maxImageDimension;
            return ctlState;
        }
        protected override void LoadControlState(object savedState)
        {
            if (savedState == null)
                return;
            object[] ctlState = (object[])savedState;
            base.LoadControlState(ctlState[0]);
            this._abbreviatedDisplay = (bool)ctlState[1];
            this._displayTitle = (bool)ctlState[2];
            this._titleText = (string)ctlState[3];
            this._maxImageDimension = (int)ctlState[4];
        }

        #endregion

        #region Page Overhead

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            //set up crop control
            //string script = " $('#jcropper-ven').Jcrop({onSelect: showCoords,onChange: showCoords,bgColor: '#333333',bgOpacity: .7, ";
            ////script += "setSelect: [100, 100, 50, 50], aspectRatio: 1 }); ";
            //script += "aspectRatio: 1 }); ";
            //Atx.RegisterJQueryScript_BlockUI_AjaxMethod(this.wizEdit, null, false, script);

            //do blocking for upload controls differently
            Atx.RegisterJQueryScript_BlockUIEvents(this.wizEdit, "#srceditor", "('.btnupload')", null);

            //register ajax function
            System.Text.StringBuilder regFunc = new System.Text.StringBuilder();
            regFunc.AppendFormat("function VenueItemSelected(source, eventArgs) {{ {0}", Environment.NewLine);
            regFunc.AppendFormat("{1}var selectionPanelId = '{2}'; {0}",
                Environment.NewLine, Utils.Constants.Tabs(1), this.PanelHidden.UniqueID);
            regFunc.AppendFormat("{1}var hidden = getChildElement(selectionPanelId, \"hidSelectedValue\"); {0}", Environment.NewLine, Utils.Constants.Tabs(1));
            regFunc.AppendFormat("{1}hidden.value = eventArgs.get_value(); {0}", Environment.NewLine, Utils.Constants.Tabs(1));
            regFunc.AppendFormat("{1}var elemId = '{2}'; {0}",
                Environment.NewLine, Utils.Constants.Tabs(1), this.btnLoad.ClientID);
            regFunc.AppendFormat("{1}var elem = $get(elemId); {0}", Environment.NewLine, Utils.Constants.Tabs(1));
            regFunc.AppendFormat("{1}try {{ elem.click(); }} {0}", Environment.NewLine, Utils.Constants.Tabs(1));
            regFunc.AppendFormat("{1}catch (ex) {{ __doPostBack('{2}', ''); }} {0}",
                Environment.NewLine, Utils.Constants.Tabs(1), this.btnLoad.ClientID);
            regFunc.AppendFormat("}} {0}", Environment.NewLine, Utils.Constants.Tabs(1));

            System.Web.UI.ScriptManager.RegisterStartupScript(this.wizEdit, this.wizEdit.GetType(),
                    Guid.NewGuid().ToString(), " ;" + regFunc.ToString(), true);
        }
        
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            txtSelection.Attributes.Add("autocomplete", "off");
            uplPicture.Attributes.Add("size", "55");

            this.Page.RegisterRequiresControlState(this);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            ////register the upload buttons to do a full postback
            //also note that there is a hidden upload control to force the page to emit enctype="multipart/...." when 
            //particular portions of the wizard are hidden
            ScriptManager mgr = (ScriptManager)this.Page.Master.FindControl("ScriptManager1");
            if (mgr != null)
                mgr.RegisterPostBackControl(this.btnUpload);

            if (!IsPostBack)
            {
                lblAlert.Text = string.Empty;
                litImage.DataBind();
            }
        }

        #endregion

        #region FormView

        protected void WebsiteTextBox_TextChanged(object sender, EventArgs e)
        {
            HyperLink test = (HyperLink)FormView1.FindControl("linkTestWebsite");
            if (test != null)
                test.DataBind();
        }
        protected void linkTestWebsite_DataBinding(object sender, EventArgs e)
        {
            TextBox txt = (TextBox)FormView1.FindControl("WebsiteTextBox");

            if (txt != null)
            {
                string input = txt.Text.Trim();

                //update the test link button
                HyperLink test = (HyperLink)sender;

                if (Utils.Validation.IsValidUrl(input))
                    test.NavigateUrl = Utils.ParseHelper.FormatUrlFromString(input, true, false);
            }
        }
        //data is bound by a sqldataobject - the key is the admin cookie for current...Id 
        protected void FormView1_DataBound(object sender, EventArgs e)
        {
            FormView view = (FormView)sender;

            if (view.DataItem != null)
            {
                Button newB = (Button)view.FindControl("NewButton");
                if (newB != null)
                    newB.Visible = this.AllowSelect;

                Literal img = (Literal)view.FindControl("litImgThumb");

                if (img != null && Entity != null)
                    img.DataBind();

                RegularExpressionValidator regexWeb = (RegularExpressionValidator)view.FindControl("regexWebsite");
                if (regexWeb != null)
                    regexWeb.ValidationExpression = Utils.Validation.regexUrl.ToString();

                Button delete = (Button)view.FindControl("DeleteButton");

                if (delete != null)
                {
                    delete.Visible = this.AllowSelect;
                    delete.OnClientClick = string.Format("return confirm('Are you sure you want to delete {0}?')",
                        Utils.ParseHelper.ParseJsAlert(Entity.Name));
                }

                HyperLink btnWeb = (HyperLink)view.FindControl("btnWebsiteUrl");
                if (btnWeb != null && Entity != null)
                {
                    if (btnWeb.Visible)
                        btnWeb.NavigateUrl = Utils.ParseHelper.FormatUrlFromString(Entity.WebsiteUrl, true, false);
                    else 
                        btnWeb.NavigateUrl = string.Empty;
                }

                GridView tunes = (GridView)view.FindControl("GridViewListing");
                if (tunes != null)
                    tunes.DataBind();
            }
        }
        protected void FormView1_ItemDeleting(object sender, FormViewDeleteEventArgs e)
        {
            if (Entity.ImageManager != null) 
                Entity.ImageManager.Delete();

            Entity.PictureUrl = null;
        }
        protected void FormView1_ItemDeleted(object sender, FormViewDeletedEventArgs e)
        {
            if (ShowException(e.Exception))
                e.ExceptionHandled = true;
        }
        protected void FormView1_ItemInserting(object sender, FormViewInsertEventArgs e)
        {
            //make sure input is uppercase
            e.Values["Name"] = e.Values["Name"].ToString().ToUpper();
        }
        protected void FormView1_ItemUpdating(object sender, FormViewUpdateEventArgs e)
        {
            //make sure input is uppercase
            e.NewValues["Name"] = e.NewValues["Name"].ToString().ToUpper();
        }
        protected void SqlDetails_Inserted(object sender, SqlDataSourceStatusEventArgs e)
        {
            if (e.Command.Parameters["@NewId"] != null && Utils.Validation.IsInteger(e.Command.Parameters["@NewId"].Value.ToString()))
            {
                SelectedIdx = (int)e.Command.Parameters["@NewId"].Value;
                OnSelectedVenueChanged(new AdminEvent.EditorEntityChangedEventArgs(SelectedIdx, ""));
            }
            else
                SelectedIdx = 0;
        }
        protected void FormView1_ItemInserted(object sender, FormViewInsertedEventArgs e)
        {
            //there is a certain exception that we want to handle
            //and that is when we are trying to add an existing ... - in this case we would like
            //to just select the ... that was being inserted
            if(e.Exception != null && e.Exception.Message.ToLower().IndexOf("cannot insert duplicate key row in object") != -1)
            {
                //get the ...
                //assign to the editor
                Venue venue = new Venue();
                venue.LoadAndCloseReader(Venue.FetchByParameter("NameRoot", e.Values["Name"].ToString()));

                //if found
                if (venue.Id > 0)
                {
                    SelectedIdx = venue.Id;
                    e.ExceptionHandled = true;
                    e.KeepInInsertMode = false;
                    FormView view = (FormView)sender;
                    view.ChangeMode(FormViewMode.Edit);

                    OnSelectedVenueChanged(new AdminEvent.EditorEntityChangedEventArgs(venue.Id, ""));

                    return;
                }
                //otherwise - show the error
            }

            if (ShowException(e.Exception))
            {
                e.KeepInInsertMode = true;
                e.ExceptionHandled = true;
            }
            else
            {
                e.KeepInInsertMode = false;
                FormView view = (FormView)sender;
                view.ChangeMode(FormViewMode.Edit);
            }
        }
        protected void FormView1_ItemUpdated(object sender, FormViewUpdatedEventArgs e)
        {
            if (e.Exception != null)
            {
                CustomValidator custom = (CustomValidator)wizEdit.FindControl("CustomValidation");
                if (custom != null)
                {
                    string ec = e.Exception.Message;
                    if (ec.IndexOf("Cannot insert duplicate key row") != -1)
                        ec = "The entry you have made already exists.";

                    custom.IsValid = false;
                    custom.ErrorMessage = ec;
                    custom.Text = ec;
                }

                e.KeepInEditMode = true;
                e.ExceptionHandled = true;                
            }
        }
        protected void SqlDetails_Inserting(object sender, SqlDataSourceCommandEventArgs e)
        {
            e.Command.Parameters["@appId"].Value = Wcss._Config.APPLICATION_ID;
        }
        protected void SqlDetails_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters["@appId"].Value = Wcss._Config.APPLICATION_ID;
        }

        #endregion

        protected void pnlShowList_DataBinding(object sender, EventArgs e)
        {
            Panel p = (Panel)sender;
            if (this.Page.ToString().ToLower().IndexOf("entityeditor") != -1 &&
                (_Config._Site_Entity_Mode != _Enums.SiteEntityMode.Venue ||
                (Entity.Name.ToLower() != _Config._Default_VenueName))
                )
            {
                p.Visible = true;
            }
            else
            {
                p.Controls.Clear();
                p.Visible = false;
            }
        }
        protected void rptShowList_OnDataBinding(object sender, EventArgs e)
        {
            Repeater rpt = (Repeater)sender;
        }
        protected void rptShowList_ItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            Repeater rpt = (Repeater)sender;
            string cmd = e.CommandName.ToLower();

            switch (cmd)
            {
                case "viewshow":
                    int idx = int.Parse(e.CommandArgument.ToString());
                    //set the current show
                    Atx.SetCurrentShowRecord(idx);
                    //set the view date to sync the calendar picker
                    Atx.ShowChooserStartDate = Atx.CurrentShowRecord.FirstDate.AddDays(-10);
                    //go to the show editor page
                    base.Redirect("ShowEditor.aspx?p=details");
                    break;
            }
        }

        #region Lists Etc

        protected void rdoSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            RadioButtonList rbl = (RadioButtonList)sender;

            txtSelection.Text = string.Empty;
            Ctx.SearchLike_Venue = (rbl.SelectedValue == "true");
        }

        //use this in this instance because the items are specified in the aspx and therefore no databinding event is called
        protected void rdoSearch_Load(object sender, EventArgs e)
        {
            RadioButtonList rbl = (RadioButtonList)sender;

            rbl.SelectedIndex = -1;

            ListItem li = rbl.Items.FindByValue(Ctx.SearchLike_Venue.ToString().ToLower());
            if (li != null)
                li.Selected = true;
        }

        #endregion

        #region Uploads

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            //redirect back to editor   
            this.wizEdit.ActiveStepIndex = 0;
        }
        /// <summary>
        /// for images
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            FileUpload upload = uplPicture;
            CustomValidator validation = CustomUpload;

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

                    string pathFile = string.Format("{0}{1}", _ImageManager._VenueImageStorage_Local, fileName);
                    mappedFile = Server.MapPath(pathFile);

                    if (System.IO.File.Exists(mappedFile))
                    {
                        fileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + uploadExt;
                        mappedFile = Server.MapPath(string.Format("{0}{1}", _ImageManager._VenueImageStorage_Local, fileName));
                    }

                    if (Entity.ImageManager != null) 
                        Entity.ImageManager.Delete();

                    //save the new file
                    upload.SaveAs(mappedFile);                    

                    //assign new image to entity
                    Entity.PictureUrl = fileName;

                    //set dimensions for new pic - entity
                    try
                    {
                        System.Web.UI.Pair p = Utils.ImageTools.GetDimensions(mappedFile);
                        Entity.PicWidth = (int)p.First;
                        Entity.PicHeight = (int)p.Second;
                    }
                    catch (Exception ex)
                    {
                        _Error.LogException(ex);

                        Entity.PicWidth = 0;
                        Entity.PicHeight = 0;
                    }

                    //save the entity
                    Entity.Save();

                    //create thumbs
                    Entity.ImageManager.CreateAllThumbs();

                    //redirect back to editor if we are NOT within the editor page   
                    if (this.Page.ToString().ToLower().IndexOf("admin_entityeditor_aspx") == -1)
                        this.wizEdit.ActiveStepIndex = 0;
                    //this.wizEdit.ActiveStepIndex = 1;

                    litImage.DataBind();
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
                    validation.IsValid = false;
                    validation.ErrorMessage = ex.Message;
                }

                Atx.UnblockUI(this.wizEdit, "#srceditor");
            }
        }

        #endregion        
}
}