using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Collections.Generic;

using Wcss;

/* This page does not create a published version. That step occurs in the send control
 * 
 * 
 */
 
namespace WillCallWeb.Admin.AdminControls
{
    public partial class Mailer_Edit : BaseControl
    {
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            Button btnUpload = (Button)FormView1.FindControl("btnUploadImage");
            //register the upload button to do a full postback
            ScriptManager mgr = (ScriptManager)this.Page.Master.FindControl("ScriptManager1");
            if (btnUpload != null && mgr != null)
                mgr.RegisterPostBackControl(btnUpload);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FormView1.DataBind();
            }
        }

        #region Css and Image Selection

        protected void ddlFile_DataBinding(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            List<ListItem> list = new List<ListItem>();

            string virtualPath = string.Format("{0}", (ddl.ID.ToLower() == "ddlcssclass") ? SubscriptionEmail.Path_PostedCss : SubscriptionEmail.Path_PostedImages);
            string mappedPath = Server.MapPath(virtualPath);
            string[] files = Directory.GetFiles(mappedPath);

            foreach (string s in files)
            {
                bool _add = false;
                string fileName = Path.GetFileName(s);

                //only add appropriate file for list type
                if (ddl.ID.ToLower().IndexOf("css") != -1 &&
                    Path.GetExtension(fileName).ToLower() == ".css" &&
                    fileName.ToLower() != "mailer.css")
                    _add = true;
                else if (Utils.Validation.IsValidImageFile(fileName))
                    _add = true;

                if (_add)
                    list.Add(new ListItem(fileName, s));
            }

            ddl.AppendDataBoundItems = true;
            ddl.DataSource = list;
            ddl.DataTextField = "Text";
            ddl.DataValueField = "Value";
        }
        //protected void ddlCssClass_DataBound(object sender, EventArgs e)
        //{
        //    DropDownList ddl = (DropDownList)sender;

        //    ddl.SelectedIndex = -1;

        //    SubscriptionEmail eml = (SubscriptionEmail)FormView1.DataItem;
        //    if (eml != null)
        //    {
        //        ListItem li = ddl.Items.FindByText(eml.CssFile ?? string.Empty);
        //        if (li != null)
        //            li.Selected = true;
        //    }

        //    if (ddl.Items.Count > 0 && ddl.SelectedIndex == -1)
        //        ddl.SelectedIndex = 0;
        //}

        #endregion

        #region Email Selection

        protected void ddlMailers_DataBinding(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;

            SubscriptionEmailCollection coll = new SubscriptionEmailCollection();
            SubSonic.QueryCommand cmd = new
                SubSonic.QueryCommand("SELECT se.* FROM [SubscriptionEmail] se, [Subscription] s WHERE s.[ApplicationId] = @appId AND s.[Id] = se.[tSubscriptionId] ORDER BY [Id] DESC ",
                SubSonic.DataService.Provider.Name);
            cmd.Parameters.Add("@appId", _Config.APPLICATION_ID, DbType.Guid);
            
            coll.LoadAndCloseReader(SubSonic.DataService.GetReader(cmd));

            ddl.AppendDataBoundItems = (FormView1.CurrentMode == FormViewMode.Insert);

            ddl.DataSource = coll;
            ddl.DataTextField = "EmailLetterName";
            ddl.DataValueField = "Id";
        }
        protected void ddlMailers_DataBound(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;

            if (ddl.Items.Count > 0)
            {
                ListItem li;
                ddl.SelectedIndex = -1;
                if (Atx.CurrentSubscriptionEmailId == 0)
                    li = ddl.Items[0];
                else
                    li = ddl.Items.FindByValue(Atx.CurrentSubscriptionEmailId.ToString());

                if (li != null)
                    li.Selected = true;
            }
        }
        protected void ddlMailers_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;

            Atx.CurrentSubscriptionEmailId = int.Parse(ddl.SelectedValue);
            FormView1.DataBind();
        }

        #endregion

        #region Subscription Selection

        protected void ddlSubscription_DataBinding(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;

            ddl.DataSource = _Lookits.Subscriptions;
            ddl.DataTextField = "NameAndRecipients";
            ddl.DataValueField = "Id";

        }
        protected void ddlSubscription_DataBound(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;

            if (ddl.Items.Count > 0)
            {
                ddl.SelectedIndex = -1;

                SubscriptionEmail eml = (SubscriptionEmail)FormView1.DataItem;
                if (eml != null)
                {
                    ListItem li = ddl.Items.FindByValue(eml.TSubscriptionId.ToString());
                    if (li != null)
                        li.Selected = true;
                }
                else if (FormView1.CurrentMode == FormViewMode.Insert)
                {
                    Subscription defaultSub = _Lookits.Subscriptions.GetList().Find(delegate(Subscription match) { return (match.IsDefault); } );
                    if (defaultSub != null)
                    {
                        ListItem li = ddl.Items.FindByValue(defaultSub.Id.ToString());
                        if (li != null)
                            li.Selected = true;
                    }
                }
            }
        }

        #endregion

        #region Form View

        protected void FormView1_ItemCreated(object sender, EventArgs e)
        {
            FormView form = (FormView)sender;

            FileUpload upl = (FileUpload)form.FindControl("uplMailerImage");
            if (upl != null)
                upl.Attributes.Add("size", "55");
        }
        protected void FormView1_DataBinding(object sender, EventArgs e)
        {
            FormView form = (FormView)sender;

            //FileUpload upload = (FileUpload)form.FindControl("uplMailerImage");
            //if(upload != null)
            //    upload.Attributes.Add("size", "55");

            SubscriptionEmailCollection coll = new SubscriptionEmailCollection();
            SubSonic.QueryCommand cmd = new
                SubSonic.QueryCommand("SELECT se.* FROM [SubscriptionEmail] se, [Subscription] s WHERE s.[ApplicationId] = @appId AND s.[Id] = se.[tSubscriptionId] ORDER BY [Id] DESC ",
                SubSonic.DataService.Provider.Name);
            cmd.Parameters.Add("@appId", _Config.APPLICATION_ID, DbType.Guid);

            coll.LoadAndCloseReader(SubSonic.DataService.GetReader(cmd));

            //SubscriptionEmailCollection coll = new SubscriptionEmailCollection();
            //IDataReader rdr = new SubSonic.Query("SubscriptionEmail").ORDER_BY("[Id] Desc").ExecuteReader();
            //coll.Load(rdr);

            if(coll.Count > 0)
            {
                SubscriptionEmail chosen;

                if (Atx.CurrentSubscriptionEmailId == 0)
                {
                    chosen = coll[0];
                    Atx.CurrentSubscriptionEmailId = chosen.Id;
                }
                else
                    chosen = (SubscriptionEmail)coll.Find(Atx.CurrentSubscriptionEmailId);

                coll.Clear();
                if(chosen != null)
                    coll.Add(chosen);
            }

            form.DataSource = coll;
        }
        protected void FormView1_DataBound(object sender, EventArgs e)
        {
            FormView form = (FormView)sender;
            SubscriptionEmail subEmail = (SubscriptionEmail)form.DataItem;

            Button delete = (Button)form.FindControl("btnDelete");
            if (subEmail != null && delete != null)
                delete.OnClientClick = string.Format("return confirm('Are you sure you want to delete {0}?')",
                    Utils.ParseHelper.ParseJsAlert(subEmail.EmailLetterName));

            TextBox txtName = (TextBox)form.FindControl("txtName");
            if (txtName != null && subEmail != null)
            {
                string[] parts = subEmail.EmailLetterName.Split('_');
                string namePart = string.Join("_", parts, 1, parts.Length - 1);
                txtName.Text = namePart;
            }
        }
        protected void FormView1_ModeChanging(object sender, FormViewModeEventArgs e)
        {
            FormView form = (FormView)sender;
            form.ChangeMode(e.NewMode);
            if (e.CancelingEdit)
                form.DataBind();
        }
        protected void FormView1_ItemDeleting(object sender, FormViewDeleteEventArgs e)
        {
            FormView form = (FormView)sender;

            int subId = (int)form.DataKey["Id"];
            int emlId = (int)form.DataKey["TEmailLetterId"];

            if(subId > 0 && emlId > 0)
            {
                try
                {
                    //remove posted file
                    SubscriptionEmail subEmail = SubscriptionEmail.FetchByID(subId);
                    if (subEmail.PostedFileName.Trim().Length > 0)
                    {
                        string mappedPath = subEmail.PublishedPathAndFile_Mapped;

                        if (File.Exists(mappedPath))
                            File.Delete(mappedPath);
                    }

                    //remove all mailqueued items that have not been processed - processed items will be moved to the archive
                    //params will cascade delete
                    System.Text.StringBuilder del = new System.Text.StringBuilder();
                    del.Append("DELETE FROM [MailQueue] WHERE [TSubscriptionEmailId] = @subId AND [DateProcessed] IS NULL ");

                    del.Append("INSERT EmailParamArchive ([Id], [Name], [Value], [TMailQueueId], [dtStamp]) ");
                    del.Append("SELECT ep.[Id], ep.[Name], ep.[Value], ep.[TMailQueueId], ep.[dtStamp] ");
                    del.Append("FROM [EmailParam] ep, [MailQueue] mq WHERE mq.[TSubscriptionEmailId] = @subId ");
                    del.Append("AND mq.[Id] = ep.[TMailQueueId] ");

                    del.Append("INSERT MailQueueArchive ([ApplicationId], [Id], [dtStamp], [DateToProcess], [DateProcessed], [FromName], ");
			        del.Append("[FromAddress], [ToAddress], [CC], [BCC], [Status], [TEmailLetterId], ");
			        del.Append("[TSubscriptionEmailId], [Priority], [bMassMailer], [Threadlock], [AttemptsRemaining]) ");
                    del.Append("SELECT mq.[ApplicationId], mq.[Id], mq.[dtStamp], mq.[DateToProcess], mq.[DateProcessed], mq.[FromName], mq.[FromAddress], ");
			        del.Append("mq.[ToAddress], mq.[CC], mq.[BCC], mq.[Status], mq.[TEmailLetterId], mq.[TSubscriptionEmailId], mq.[Priority], ");
			        del.Append("mq.[bMassMailer], mq.[Threadlock], mq.[AttemptsRemaining] ");
                    del.Append("FROM [MailQueue] mq WHERE mq.[TSubscriptionEmailId] = @subId ");

                    //final cleanup - params will cascade
                    del.Append("DELETE FROM [MailQueue] WHERE [TSubscriptionEmailId] = @subId ");

                    SubSonic.QueryCommand cmd = new SubSonic.QueryCommand(del.ToString(), SubSonic.DataService.Provider.Name);
                    cmd.Parameters.Add("@subId", subId, DbType.Int32);
                    SubSonic.DataService.ExecuteQuery(cmd);

                    SubscriptionEmail.Delete(subId);
                    EmailLetter.Delete(emlId);

                    Atx.CurrentSubscriptionEmailId = 0;
                    _Lookits.RefreshLookup(_Enums.LookupTableNames.Subscriptions.ToString());
                    form.DataBind();
                }
                catch(Exception ex)
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

            //get input
            TextBox txtName = (TextBox)form.FindControl("txtName");
            TextBox txtSubject = (TextBox)form.FindControl("txtSubject");            
            DropDownList ddlSubscription = (DropDownList)form.FindControl("ddlSubscription");

            string name = txtName.Text.Trim();
            string subject = txtSubject.Text.Trim();
            
            string subName = ddlSubscription.SelectedItem.Text.Trim();
            foreach(char c in Path.GetInvalidPathChars())
                subName = subName.Replace(c.ToString(),string.Empty);

            int idx = int.Parse(ddlSubscription.SelectedValue);

            //auto create filename
            string autoName = SubscriptionEmail.ConstructBodyName(txtName.Text, true);

            string styleContent = string.Empty;
            string htmlVersion = string.Empty;
            string textVersion = string.Empty;

            DropDownList ddlMailers = (DropDownList)form.FindControl("ddlMailers");
            CheckBox chkCopy = (CheckBox)form.FindControl("chkCopy");

            try
            {
                if (ddlMailers != null && chkCopy != null && chkCopy.Checked)
                {
                    if (ddlMailers.SelectedValue == "0")
                        throw new Exception("You have indicated that you wish to copy a previous email. Please select an email to copy from the list.");

                    SubscriptionEmail se = new SubscriptionEmail(int.Parse(ddlMailers.SelectedValue));
                    if (se != null)
                    {
                        styleContent = se.EmailLetterRecord.StyleContent;
                        htmlVersion = se.EmailLetterRecord.HtmlVersion;
                        textVersion = se.EmailLetterRecord.TextVersion;
                    }
                }

                int emailId = MailCreation.CreateEmailLetterAndSubscriptionEmail(idx, autoName, subject, styleContent, htmlVersion, textVersion);

                //set chosen id
                Atx.CurrentSubscriptionEmailId = emailId;
                form.ChangeMode(FormViewMode.Edit);
                form.DataBind();
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
        protected void FormView1_ItemUpdating(object sender, FormViewUpdateEventArgs e)
        {
            FormView form = (FormView)sender;
            
            int idx = (form.DataKey["Id"] != null) ? (int)form.DataKey["Id"] : 0;
            SubscriptionEmail subEmail = SubscriptionEmail.FetchByID(idx);
            bool updateEmail = false;           

            if (subEmail != null)
            {
                TextBox style = (TextBox)form.FindControl("txtStyle");
                TextBox html = (TextBox)form.FindControl("txtHtml");
                TextBox txtName = (TextBox)form.FindControl("txtName");
                DropDownList sub = (DropDownList)form.FindControl("ddlSubscription");
                TextBox txtSubject = (TextBox)form.FindControl("txtSubject");
                TextBox txtBody = (TextBox)form.FindControl("txtBody");

                string edited = html.Text.Trim();

                //try
                //{
                //    if (edited.Length > Utils.Constants.Varchar_MAX_Size)
                //        throw new Exception(string.Format("Please limit the HTML version to {0} chars or less.", Utils.Constants.Varchar_MAX_Size));
                //}
                //catch (System.Threading.ThreadAbortException) { }
                //catch (Exception ex)
                //{
                //    CustomValidation.IsValid = false;
                //    CustomValidation.ErrorMessage = ex.Message;
                //}

                //ensure that we have no html structure tags in the input -- these will be added later
                if (Utils.Validation.ContainsHtmlStructureTags(edited))
                {
                    CustomValidator validation = (CustomValidator)form.FindControl("CustomValidation");
                    if (validation != null)
                    {
                        validation.IsValid = false;
                        validation.ErrorMessage = "Please remove any structural html tags. The html input should be what is placed inside the body only. Html tags will be added automatically to the email later in the process.";
                    }

                    e.Cancel = true;
                    return;
                }

                if (edited != subEmail.EmailLetterRecord.HtmlVersion)
                {
                    subEmail.EmailLetterRecord.HtmlVersion = edited;
                    updateEmail = true;
                }


                ////
                if (txtName != null)
                {
                    string input = txtName.Text.Trim();

                    //name part is the body name without the extension or the leadin date and underscore
                    string[] parts = subEmail.EmailLetterName.Split('_');
                    string datePart = parts[0];
                    string namePart = string.Join("_", parts, 1, parts.Length - 1);

                    if (input != namePart)
                    {
                        //update bodyname
                        subEmail.EmailLetterRecord.Name = SubscriptionEmail.ConstructBodyName(input, true);

                        updateEmail = true;
                    }
                }

                if (sub != null)
                {
                    if (sub.SelectedValue != subEmail.TSubscriptionId.ToString())
                    {
                        subEmail.TSubscriptionId = int.Parse(sub.SelectedValue);
                        updateEmail = true;
                    }
                }

                if (style != null)
                {
                    string input = style.Text.Trim();
                    if (input != subEmail.EmailLetterRecord.StyleContent)
                    {
                        subEmail.EmailLetterRecord.StyleContent = input;
                        updateEmail = true;
                    }
                }

                if (txtSubject != null)
                {
                    string input = txtSubject.Text.Trim();
                    if (input != subEmail.EmailLetterRecord.Subject)
                    {
                        subEmail.EmailLetterRecord.Subject = input;
                        updateEmail = true;
                    }
                }

                if (txtBody != null)
                {
                    string txt = txtBody.Text.Trim();
                    if (txt != subEmail.EmailLetterRecord.TextVersion)
                        subEmail.EmailLetterRecord.TextVersion = txt;
                    updateEmail = true;
                }

                if (updateEmail)
                {
                    string sql = "UPDATE [EmailLetter] SET [Name] = @name, [Subject] = @subject, [StyleContent] = @style, [HtmlVersion] = @html, ";
                    sql += "[TextVersion] = @text ";
                    sql += "WHERE [Id] = @idx ";
                    SubSonic.QueryCommand cmd = new SubSonic.QueryCommand(sql, SubSonic.DataService.Provider.Name);
                    cmd.Parameters.Add("@name", subEmail.EmailLetterRecord.Name, DbType.String);
                    cmd.Parameters.Add("@subject", subEmail.EmailLetterRecord.Subject, DbType.String);
                    cmd.Parameters.Add("@style", subEmail.EmailLetterRecord.StyleContent, DbType.String);
                    cmd.Parameters.Add("@html", subEmail.EmailLetterRecord.HtmlVersion, DbType.String);
                    cmd.Parameters.Add("@text", subEmail.EmailLetterRecord.TextVersion, DbType.String);
                    cmd.Parameters.Add("@idx", subEmail.EmailLetterRecord.Id, DbType.Int32);

                    SubSonic.DataService.ExecuteQuery(cmd);

                    subEmail.EnsurePublication(true);

                    _Lookits.RefreshLookup(_Enums.LookupTableNames.Subscriptions.ToString());
                }
            }

            form.DataBind();
        }
        protected void FormView1_ItemCommand(object sender, FormViewCommandEventArgs e)
        {
            FormView form = (FormView)sender;
            string cmd = e.CommandName.ToLower();

            switch (cmd)
            {
                case "uploadimage":

                    //save work in progress
                    form.UpdateItem(false);

                    FileUpload upload = (FileUpload)form.FindControl("uplMailerImage");
                    string mappedFile = string.Empty;

                    if (upload != null && upload.HasFile)
                    {
                        //validate file name
                        string uploadExt = Path.GetExtension(upload.FileName).ToLower();

                        try
                        {
                            if (uploadExt.Trim().Length == 0 || (uploadExt != ".jpg" && uploadExt != ".jpeg" && uploadExt != ".gif" && uploadExt != ".png"))
                                throw new Exception("Valid file types are jpg, jpeg, gif and png only.");

                            string fileName = System.Text.RegularExpressions.Regex.Replace(Path.GetFileNameWithoutExtension(upload.FileName), @"\s+", string.Empty);
                            fileName = fileName.Replace("'", string.Empty).Replace("-", "_").Replace("&", "_");
                            //get the file name to save
                            fileName += uploadExt;

                            if (!Utils.Validation.IsValidImageFile(fileName))
                                throw new Exception("Please enter a valid file name. Valid filenames use letters, underscores and periods. Only jpg, jpeg, gif or png are valid");
                            //endvalidation

                            string pathFile = string.Format("/{0}/{1}", SubscriptionEmail.Path_PostedImages, fileName);
                            mappedFile = Server.MapPath(pathFile);

                            if (System.IO.File.Exists(mappedFile))
                            {
                                System.IO.File.Delete(mappedFile);
                                //fileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + uploadExt;
                                //mappedFile = Server.MapPath(string.Format("/{0}/{1}", SubscriptionEmail.Path_PostedImages, fileName));
                            }

                            //save the new file
                            upload.SaveAs(mappedFile);
                        }
                        catch (Exception ex)
                        {
                            CustomValidator validation = (CustomValidator)form.FindControl("CustomValidation");
                            if (validation != null)
                            {
                                validation.IsValid = false;
                                validation.ErrorMessage = ex.Message;
                            }
                        }
                    }

                    //rebind the form
                    form.DataBind();

                    break;
            }
        }

        #endregion
    }
}