using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;

using Wcss;

namespace WillCallWeb.Admin.AdminControls
{
    //this page does not allow inserts - that must be done from show picker
    public partial class ShowDetails : BaseControl, IPostBackEventHandler
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

        protected void litFB_DataBinding(object sender, EventArgs e)
        {
            Literal lit = (Literal)sender;

            //do a little ding on the db to get info
            if (Atx.CurrentShowRecord != null)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("DECLARE @like int, @unlike int, @agglike int; ");
                sb.Append("SET @like = 0; SET @unlike = 0; SET @agglike = 0; ");

                sb.Append("SELECT @like = ISNULL(SUM(Total),0) FROM [Fb_Stat] WHERE [EntityId] = @idx AND [ApiFunction] = @fblikefunction; ");
                sb.Append("SELECT @unlike = ISNULL(SUM(Total),0) FROM [Fb_Stat] WHERE [EntityId] = @idx AND [ApiFunction] = @fbunlikefunction; ");
                sb.Append("SELECT @agglike = ISNULL(SUM(Total),0) FROM [Fb_Stat] WHERE [EntityId] = @idx AND [ApiFunction] = @fbagglikefunction; ");
                sb.Append("SELECT @like as 'like', @unlike as 'unlike', @agglike as 'agglike'; ");

                SubSonic.QueryCommand cmd = new SubSonic.QueryCommand(sb.ToString(), SubSonic.DataService.Provider.Name);
                cmd.Parameters.Add("@idx", Atx.CurrentShowRecord.Id, System.Data.DbType.Int32);
                cmd.Parameters.Add("@fblikefunction", _Enums.FB_Api.FB_Like.ToString());
                cmd.Parameters.Add("@fbunlikefunction", _Enums.FB_Api.FB_UnLike.ToString());
                cmd.Parameters.Add("@fbagglikefunction", _Enums.FB_Api.Likes.ToString());
                
                try
                {
                    int like = 0, unlike = 0, agglike = 0;

                    using (System.Data.IDataReader dr = SubSonic.DataService.GetReader(cmd))
                    {
                        while (dr.Read())
                        {
                            like = (int)dr.GetValue(dr.GetOrdinal("like"));
                            unlike = (int)dr.GetValue(dr.GetOrdinal("unlike"));
                            agglike = (int)dr.GetValue(dr.GetOrdinal("agglike"));
                        }

                        dr.Close();
                    }

                    lit.Text = string.Format("<div class=\"fbs-likepanel\"><span class=\"fbs-like\">{0}</span><span class=\"fbs-unlike\">{1}</span></div>",
                        like.ToString(), unlike.ToString());

                    return;
                }
                catch (Exception ex)
                {
                    _Error.LogException(ex);
                }
            }

            lit.Text = string.Empty;
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            //do blocking for upload controls differently
            Atx.RegisterJQueryScript_BlockUIEvents(this.FormView1, "#srceditor", "('.btnupload')", null);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Atx.CurrentShowRecord == null)
                base.Redirect("/Admin/ShowEditor.aspx");

            if (!IsPostBack)
            {
                string link = string.Format("return confirm('Are you sure you want to delete {0}?')", 
                    Utils.ParseHelper.ParseJsAlert(Atx.CurrentShowRecord.Name));
                btnDelete.OnClientClick = link;

                FormView1.DataBind();
            }

            litShowTitle.Text = Atx.CurrentShowRecord.Name;
        }

        protected void btnSales_Click(object sender, EventArgs e)
        {
            base.Redirect(string.Format("/Admin/Listings.aspx?p=tickets&showid={0}", 
                (Atx.CurrentShowRecord != null) ? Atx.CurrentShowRecord.Id.ToString() : "0"));
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            Atx.CurrentShowRecord.ImageManager_Delete();

            FormView1.DataBind();
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            FileUpload upload = (FileUpload)FormView1.FindControl("uplPicture");
            CustomValidator validation = (CustomValidator)FormView1.FindControl("CustomUpload");;
            Show Entity = Atx.CurrentShowRecord;

            if (Entity != null && validation != null && upload != null && upload.HasFile)
            {
                string mappedFile = string.Empty;

                try
                {
                    //validate file name
                    string uploadExt = Path.GetExtension(upload.FileName).ToLower();

                    if (uploadExt.Trim().Length == 0 || (uploadExt != ".jpg" && uploadExt != ".jpeg" && uploadExt != ".gif" && uploadExt != ".png"))
                        throw new Exception("Valid file types are jpg, jpeg, gif and png only.");

                    //we handle the file name a bit differently for shows....
                    //we make the file name unique to the show
                    string fileName = string.Format("shimg{0}{1}", Entity.Id.ToString(), uploadExt);

                    if (!Utils.Validation.IsValidImageFile(fileName))
                        throw new Exception("Please enter a valid file name. Valid filenames use letters, underscores and periods. Only jpg, jpeg, gif or png are valid");
                    //endvalidation

                    string pathFile = string.Format("{0}{1}", _ImageManager._ShowImageStorage_Local, fileName);
                    mappedFile = Server.MapPath(pathFile);

                    //if (System.IO.File.Exists(mappedFile))
                    //{
                    //    fileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + uploadExt;
                    //    mappedFile = Server.MapPath(string.Format("{0}{1}", Wcss._Config._ShowImageStorage_Local, fileName));
                    //}

                    if(Entity.ImageManager != null)
                        Entity.ImageManager.Delete();

                    //save the new file
                    upload.SaveAs(mappedFile);

                    //assign new image to entity
                    Entity.DisplayUrl = fileName;

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
                    Entity.ImageManager = new _ImageManager(string.Format("{0}{1}", _ImageManager._ShowImageStorage_Local, Entity.DisplayUrl.ToLower()));
                    Entity.ImageManager.CreateAllThumbs();

                    FormView1.DataBind();
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

                Atx.UnblockUI(this.FormView1, "#srceditor");
            }
        }

        protected void btnCancelUpload_Click(object sender, EventArgs e)
        {
            FormView1.DataBind();
        }

        protected void litImage_DataBinding(object sender, EventArgs e)
        {
            Literal lit = (Literal)sender;

            if (Atx.CurrentShowRecord.ImageManager != null)
            {
                string origin = (Atx.CurrentShowRecord.DisplayUrl != null && Atx.CurrentShowRecord.DisplayUrl.Trim().Length > 0) ? "from show" : "from act";
                
                lit.Text = string.Format("<div style=\"text-align:center;\">{0}</div><img src=\"{1}\" alt=\"\" width=\"100px\" />", 
                    origin, Atx.CurrentShowRecord.ImageManager.Thumbnail_Large);
            }
        }

        #region Details

        protected void FormView1_ItemCreated(object sender, EventArgs e)
        {
            FormView form = (FormView)sender;
            FileUpload upl = (FileUpload)form.FindControl("uplPicture");
            if (upl != null)
                upl.Attributes.Add("size", "55");

            //register upload button for full postback
            Button btnUpload = (Button)form.FindControl("btnUpload");
            System.Web.UI.ScriptManager mgr = (System.Web.UI.ScriptManager)this.Page.Master.FindControl("ScriptManager1");
            if (mgr != null && btnUpload != null)
                mgr.RegisterPostBackControl(btnUpload);
        }

        protected void FormView1_DataBinding(object sender, EventArgs e)
        {
            FormView form = (FormView)sender;

            ShowCollection coll = new ShowCollection();
            coll.Add(Atx.CurrentShowRecord);

            form.DataSource = coll;
            string[] keyNames = { "Id", "Name" };
            form.DataKeyNames = keyNames;
        }

        protected void FormView1_DataBound(object sender, EventArgs e)
        {
            FormView form = (FormView)sender;
            Show show = Atx.CurrentShowRecord;

            Literal litFB = (Literal)form.FindControl("litFB");
            if(litFB != null)
                litFB.DataBind();

            Literal litDesc = (Literal)form.FindControl("litDesc");
            if (litDesc != null)
                litDesc.Text = string.Format("<div class=\"desc-control\">{0}</div>", show.BotText);

            WillCallWeb.Components.Util.CalendarClock clockAnnounce = (WillCallWeb.Components.Util.CalendarClock)FormView1.FindControl("clockAnnounce");
            SetClock_AM(form, show.AnnounceDate, clockAnnounce);
            WillCallWeb.Components.Util.CalendarClock clockOnsale = (WillCallWeb.Components.Util.CalendarClock)FormView1.FindControl("clockOnsale");
            SetClock_AM(form, show.DateOnSale, clockOnsale);

            Button btnWys = (Button)form.FindControl("btnWys");
            if (btnWys != null)
                btnWys.ToolTip = string.Format("/Admin/AdminControls/Wysiwyg/Wysiwyg.aspx?context=s&ctrl={0}", this.UniqueID);

            Button btnClear = (Button)form.FindControl("btnClear");
            if (btnClear != null)
                btnClear.Visible = Atx.CurrentShowRecord.DisplayUrl != null && Atx.CurrentShowRecord.DisplayUrl.Trim().Length > 0;

            //explain where the image is from 
            //from headline - from show

            //if (form.CurrentMode == FormViewMode.Edit)
            //{
            //    <tr>
            //                    <th>
            //                        <a href="javascript: alert('Settings for displaying a Facebook RSVP link. Leave off the http:// portion')" class="infomark">?</a>
            //                        Use FB Rsvp
            //                    </th>
            //                    <td>
            //                        <asp:CheckBox id="chkRsvp" runat="server" checked='<%#Eval("UseFbRsvp")%>' />
            //                        <asp:TextBox ID="txtRsvp" MaxLength="256" Width="320px" runat="server" Text='<%#Eval("FbRsvpUrl") %>' />
            //                    </td>
            //                    <td>
            //                        <asp:HyperLink ID="lnkRsvp" runat="server" Target="_blank" Text="test" />
            //                    </td>
            //                </tr>

            //    //activate controls based on config
            //    CheckBox chkRsvp = (CheckBox)form.FindControl("chkRsvp");
            //    TextBox txtRsvp = (TextBox)form.FindControl("txtRsvp");
            //    HyperLink lnkRsvp = (HyperLink)form.FindControl("lnkRsvp");
            //    lnkRsvp.Enabled = txtRsvp.Enabled = chkRsvp.Enabled = _Config._Facebook_RSVP_ShowDates_Active;

            //    if (txtRsvp.Text.Trim().Length > 0)
            //    {
            //        lnkRsvp.Visible = true;
            //        lnkRsvp.NavigateUrl = string.Format("{0}{1}", (!txtRsvp.Text.Trim().ToLower().StartsWith("http")) ? "http://" : string.Empty, txtRsvp.Text.Trim());
            //    }
            //    else
            //        lnkRsvp.Visible = false;
            //}
        }
        private void SetClock_AM(FormView form, DateTime CompareDate, WillCallWeb.Components.Util.CalendarClock clock)
        {
            if (clock != null && CompareDate == Utils.Constants._MinDate)
            {
                clock.AMPM.SelectedIndex = -1;
                ListItem lia = clock.AMPM.Items.FindByText("AM");
                if (lia != null)
                    lia.Selected = true;
            }
        }

        protected void FormView1_ItemUpdating(object sender, FormViewUpdateEventArgs e)
        {
            FormView form = (FormView)sender;
            Show show = Atx.CurrentShowRecord;

            if (show != null)
            {
                try
                {
                    bool onsaleDateChange = false;
                    CheckBox chkActive = (CheckBox)form.FindControl("chkActive");
                    CheckBox chkSoldOut = (CheckBox)form.FindControl("chkSoldOut");
                    CheckBox chkRichText = (CheckBox)form.FindControl("chkRichText");
                    CheckBox chkFacebookLike = (CheckBox)form.FindControl("chkFacebookLike");
                    CheckBox chkHideAuto = (CheckBox)form.FindControl("chkHideAuto");
                    
                    if (chkActive != null && show.IsActive != chkActive.Checked)
                        show.IsActive = chkActive.Checked;

                    bool deactivateChildrenOnSellout = false;
                    if (chkSoldOut != null && show.IsSoldOut != chkSoldOut.Checked)
                    {
                        show.IsSoldOut = chkSoldOut.Checked;
                        
                        if (show.IsSoldOut)
                            deactivateChildrenOnSellout = true;
                    }

                    if (chkRichText != null && show.IsDisplayRichText != chkRichText.Checked)
                        show.IsDisplayRichText = chkRichText.Checked;

                    if (chkFacebookLike != null && show.IsAllowFacebookLike != chkFacebookLike.Checked)
                        show.IsAllowFacebookLike = chkFacebookLike.Checked;

                    if (chkHideAuto != null && show.IsHideAutoGenerated != chkHideAuto.Checked)
                        show.IsHideAutoGenerated = chkHideAuto.Checked;

                    //get announce & onsale date/time
                    WillCallWeb.Components.Util.CalendarClock announce = (WillCallWeb.Components.Util.CalendarClock)form.FindControl("clockAnnounce");
                    if (announce.SelectedDate != show.AnnounceDate)
                        show.AnnounceDate = announce.SelectedDate;

                    WillCallWeb.Components.Util.CalendarClock onsale = (WillCallWeb.Components.Util.CalendarClock)form.FindControl("clockOnsale");
                    if (onsale.SelectedDate != show.DateOnSale)
                    {
                        show.DateOnSale = onsale.SelectedDate;// > Utils.Constants._MinDate) ? onsale.SelectedDate : System.Data.SqlTypes.SqlDateTime.Null;
                        onsaleDateChange = true;
                    }

                    TextBox txtDisplayNotes = (TextBox)form.FindControl("txtDisplayNotes");
                    if (txtDisplayNotes != null && show.DisplayNotes != txtDisplayNotes.Text.Trim())
                        show.DisplayNotes = txtDisplayNotes.Text.Trim();

                    TextBox txtStatus = (TextBox)form.FindControl("txtStatus");
                    if (txtStatus != null && show.StatusText != txtStatus.Text.Trim())
                        show.StatusText = txtStatus.Text.Trim();

                    TextBox txtTitle = (TextBox)form.FindControl("txtTitle");
                    TextBox txtVenuePreText = (TextBox)form.FindControl("txtVenuePreText");
                    TextBox txtVenuePostText = (TextBox)form.FindControl("txtVenuePostText");
                    TextBox txtTopText = (TextBox)form.FindControl("txtTopText");
                    TextBox txtMidText = (TextBox)form.FindControl("txtMidText");
                    TextBox txtBotText = (TextBox)form.FindControl("txtBotText");
                    TextBox txtNotes = (TextBox)form.FindControl("txtNotes");
                    TextBox txtExternalTixUrl = (TextBox)form.FindControl("txtExternalTixUrl");

                    if (show.ShowTitle != txtTitle.Text.Trim())             show.ShowTitle = txtTitle.Text.Trim();
                    if (show.VenuePreText != txtVenuePreText.Text.Trim())   show.VenuePreText = txtVenuePreText.Text.Trim();
                    if (show.VenuePostText != txtVenuePostText.Text.Trim()) show.VenuePostText = txtVenuePostText.Text.Trim();
                    if (show.InternalNotes != txtNotes.Text.Trim())         show.InternalNotes = txtNotes.Text.Trim();
                    if (show.TopText != txtTopText.Text.Trim())             show.TopText = txtTopText.Text.Trim();
                    if (show.MidText != txtMidText.Text.Trim())             show.MidText = txtMidText.Text.Trim();

                    string extTix = txtExternalTixUrl.Text.Trim();
                    if (extTix.Length > 0 && (show.ExternalTixUrl == null || show.ExternalTixUrl != extTix))
                    {
                        //validate the url
                        if (!Utils.Validation.IsValidUrl(extTix))
                        {
                            //no way bogus man
                            throw new ArgumentOutOfRangeException("ExternalTixUrl", "Value is invalid -");
                        }

                        show.ExternalTixUrl = extTix;
                    }
                    else
                        show.ExternalTixUrl = null;



                    show.Save();

                    if (onsaleDateChange)
                    {
                        ShowDateCollection coll = new ShowDateCollection();
                        coll.AddRange(show.ShowDateRecords());
                        foreach (ShowDate sd in coll)
                        {
                            ShowTicketCollection toll = new ShowTicketCollection();
                            toll.AddRange(sd.ShowTicketRecords());

                            foreach (ShowTicket st in toll)
                            {
                                st.PublicOnsaleDate = show.DateOnSale;
                                st.Save();
                            }
                        }
                    }

                    if (deactivateChildrenOnSellout)
                    {
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();

                        sb.AppendLine("DECLARE @statusId int; SELECT @statusId = ss.[Id] FROM [ShowStatus] ss WHERE ss.[Name] = 'SoldOut'; ");
                        sb.AppendLine("UPDATE [ShowDate] SET [TStatusId] = @statusId WHERE [tShowId] = @showId; ");
                        sb.AppendLine("CREATE TABLE #relatedTix ( [idx] int ); ");
                        sb.AppendLine("INSERT #relatedTix(idx) SELECT st.[Id] as 'idx' FROM [ShowTicket] st WHERE st.[TShowId] = @showId; ");
                        sb.AppendLine("INSERT #relatedTix(idx) SELECT DISTINCT link.[LinkedShowTicketId] as 'idx' FROM [ShowTicketPackageLink] link, #relatedTix rel ");
                        sb.AppendLine("WHERE rel.[idx] = link.[ParentShowTicketId]; ");
                        sb.AppendLine("UPDATE [ShowTicket] SET [bActive] = @active WHERE [Id] IN (SELECT [idx] FROM #relatedTix); ");

                        SubSonic.QueryCommand cmd = new SubSonic.QueryCommand(sb.ToString(), SubSonic.DataService.Provider.Name);
                        cmd.Parameters.Add("@showId", show.Id, System.Data.DbType.Int32);
                        cmd.Parameters.Add("@active", false, System.Data.DbType.Boolean);
    
                        SubSonic.DataService.ExecuteQuery(cmd);

                        //***Note this may have to move out of this scope and to larger scope if other show objects are affected and need to be refreshed
                        //ensure Show has latest data
                        
                    }

                    //ensure Show has latest data
                    Atx.ResetCurrentShowRecord();

                    form.DataBind();
                }
                catch (Exception ex)
                {
                    _Error.LogException(ex);
                    CustomValidation.IsValid = false;
                    CustomValidation.ErrorMessage = ex.Message;
                }
            }
        }

        protected void FormView1_ItemDeleting(object sender, FormViewDeleteEventArgs e)
        {
            FormView form = (FormView)sender;

            Show show = Atx.CurrentShowRecord;

            if (show != null)
            {
                try
                {
                    //TODO
                    //see if show has any sales
                    ShowDateCollection coll = new ShowDateCollection();
                    coll.AddRange(Atx.CurrentShowRecord.ShowDateRecords());

                    while (coll.Count > 0)
                    {
                        Atx.CurrentShowRecord.DeleteShowDate(coll[0].Id);
                        coll.RemoveAt(0);
                    }

                    Show.Delete(Atx.CurrentShowRecord.Id);
                    Atx.SetCurrentShowRecord(0);

                    base.Redirect("/Admin/ShowEditor.aspx");
                }
                catch (Exception ex)
                {
                    _Error.LogException(ex);
                    CustomValidation.IsValid = false;
                    CustomValidation.ErrorMessage = ex.Message;
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

        #endregion

        protected void btnVenueEditor_Click(object sender, EventArgs e)
        {
            base.Redirect("/Admin/EntityEditor.aspx?p=venue");
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            FormView1.DeleteItem();
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            FormView1.DataBind();
        }
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            FormView1.UpdateItem(false);
        }
        protected void btnChangeShowName_Click(object sender, EventArgs e)
        {
            if (!Atx.CurrentShowRecord.ShowNameMatches(false))
            {
                Atx.CurrentShowRecord.ShowNameMatches(true);

                AdminEvent.OnShowNameChanged(this);

                Atx.ResetCurrentShowRecord();
                litShowTitle.Text = Atx.CurrentShowRecord.Name;
                FormView1.DataBind();
            }
        }
        
}
}