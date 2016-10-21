using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Net;
using System.IO;
using System.Xml;
using System.Text;
using System.Linq;
using System.Web.UI;
using System.ComponentModel;

using Wcss;

namespace WillCallWeb.Admin.AdminControls.Creators
{
    [ToolboxData("<{0}:CustomerMailer runat=\"Server\" MailerTypeTitle=\"\" MailTemplateSubDirectory=\"false\" StarterTemplate=\"\" MailParameters=\"FIRSTNAME,LASTNAME,EMAILADDRESS,INVOICEID\" Width=\"100%\"></{0}:CustomerMailer>")]
    public partial class CustomerMailer : BaseControl
    {
        #region props
        
        public string MailerTypeTitle { get; set; } 
        public string MailTemplateSubDirectory { get; set; } //"CustomerServiceSent";//"AccessCampaignMail";               
        public string StarterTemplate { get; set; } //_Config._CustomerInquiryTemplate;
        public string ParameterNames { get; set; } // { "FIRSTNAME", "LASTNAME", "EMAILADDRESS", "INVOICEID" };

        //ParamNames are converted to upper case
        private List<string> _paramNames = null;
        protected List<string> ParamNames
        {
            get
            {
                if(_paramNames == null)
                {
                    _paramNames = new List<string>();

                    if(ParameterNames.Trim().Length > 0)
                        _paramNames.AddRange(ParameterNames.ToUpper().Split(','));
                }

                return _paramNames;
            }
        }

        #region protected props

        protected List<string> _errors = new List<string>();
        private string _uniqueSessionLetterId = null;
        protected string UniqueSessionLetterId 
        { 
            get 
            {
                if (_uniqueSessionLetterId == null)
                    _uniqueSessionLetterId = string.Format("{0}_CtrlEmail", this.ClientID);

                return _uniqueSessionLetterId; 
            } 
        }

        #endregion 

        //a reference to the main entity
        public EmailLetter ControlLetter
        {
            get
            {
                return (EmailLetter)Session[UniqueSessionLetterId];
            }
            set
            {
                Session.Remove(UniqueSessionLetterId);

                if (value != null)
                    Session.Add(UniqueSessionLetterId, value);
            }
        }

        #endregion

        
        protected void Page_Load(object sender, EventArgs e)
        {
            System.Web.UI.ScriptManager mgr = (System.Web.UI.ScriptManager)this.Page.Master.FindControl("ScriptManager1");
            if (mgr != null)
                mgr.RegisterPostBackControl(this.btnLoadTemplate);

            if (!IsPostBack)
            {
                lblStatus.Text = string.Empty;
                ddlTemplates.DataBind();

                if (ControlLetter != null)
                    LoadTemplate();
            }

            litSample.DataBind();
        }
        protected void ddlTemplates_DataBinding(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;

            //retrieve a list of files from the "already sent" dir
            List<string> fileListing = new List<string>();

            string virtualPath = string.Format("/{0}/MailTemplates/{1}/", _Config._VirtualResourceDir, MailTemplateSubDirectory);
            string mappedPath = Server.MapPath(virtualPath);
            string[] files = Directory.GetFiles(mappedPath);

            foreach (string s in files)
                fileListing.Add(Path.GetFileName(s));

            fileListing.Add("Load Previous Template");
            fileListing.Reverse();

            ddl.DataSource = fileListing;
        }
        protected void ddlTemplates_DataBound(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;

            string selected = ddl.SelectedValue;
            if (selected != null && this.ControlLetter != null && selected.ToLower() != this.ControlLetter.Name.ToLower())
            {
                ddl.SelectedIndex = -1;

                foreach(ListItem li in ddl.Items)
                    if (li.Text.ToLower() == this.ControlLetter.Name.ToLower())
                    {
                        li.Selected = true;
                        break;
                    }
            }
        }
        protected void btnLoadTemplate_Click(object sender, EventArgs e)
        {
            LoadTemplate();
        }
        protected void LoadTemplate()
        {
            if (ddlTemplates.SelectedIndex > 0 && ddlTemplates.SelectedValue.Trim().Length > 0)//allow for list explanation
            {
                this.txtSubject.Text = string.Empty;
                this.txtHeader.Text = string.Empty;
                this.txtBody.Text = string.Empty;
                this.txtClosing.Text = string.Empty;

                string filename = ddlTemplates.SelectedValue;

                FileStream docIn = null;
                XmlDocument doc = new XmlDocument();

                try
                {
                    string mappedPath = Server.MapPath(string.Format("/{0}/MailTemplates/{1}/{2}",
                        _Config._VirtualResourceDir, MailTemplateSubDirectory, filename));

                    doc.XmlResolver = null;

                    //Open a FileStream on the Xml file
                    using (docIn = new FileStream(mappedPath, FileMode.Open, FileAccess.Read))
                    {
                        doc.Load(docIn);
                    }

                    XmlNodeList xlist = doc.GetElementsByTagName("title");
                    if (xlist.Count > 0)
                        this.txtSubject.Text = xlist[0].InnerXml;

                    //if we read it like an xml doc then
                    //<title>subject</title>
                    //<div class="header">header</div>
                    //<div class="body">body</div>
                    //<div class="closing">closing</div>

                    XmlNodeList xhtml = doc.GetElementsByTagName("body");

                    string xmlns = string.Format(" xmlns=\"{0}\"", xhtml[0].NamespaceURI);

                    foreach (XmlNode n in xhtml[0].ChildNodes)
                    {
                        if (n.Attributes.Count > 0 && n.Attributes[0].Name == "id" && n.Attributes[0].Value == "header")
                            this.txtHeader.Text = n.InnerXml.Replace(xmlns, string.Empty);
                        else if (n.Attributes.Count > 0 && n.Attributes[0].Name == "id" && n.Attributes[0].Value == "body")
                            this.txtBody.Text = n.InnerXml.Replace(xmlns, string.Empty);
                        else if (n.Attributes.Count > 0 && n.Attributes[0].Name == "id" && n.Attributes[0].Value == "closing")
                            this.txtClosing.Text = n.InnerXml.Replace(xmlns, string.Empty);
                    }

                    //reset session val
                    this.ControlLetter = null;

                    //construct sql and list
                    Wcss._DatabaseCommandHelper cmd = 
                        new Wcss._DatabaseCommandHelper("SELECT * FROM [EmailLetter] WHERE [ApplicationId] = @appId AND [Name] = @name ");
                    cmd.AddCmdParameter("appId", _Config.APPLICATION_ID.ToString(), DbType.String);
                    cmd.AddCmdParameter("name", filename, DbType.String);                    

                    EmailLetterCollection coll = new EmailLetterCollection();
                    cmd.PopulateCollectionByReader<EmailLetterCollection>(coll);

                    if (coll.Count > 0)
                        this.ControlLetter = coll[0];

                    if (this.ControlLetter != null)
                        txtTemplateName.Text = Path.GetFileNameWithoutExtension(this.ControlLetter.Name);
                }
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
                finally
                {
                    if (docIn != null)
                    {
                        docIn.Close();
                        docIn.Dispose();
                    }

                    //update inputs and refresh the sample section
                    litSample.DataBind();
                }
            }
        }
        protected void btnPreview_Click(object sender, EventArgs e)
        {
            litSample.DataBind();
        }
        
        #region GetEmailInputs

        private System.Collections.Specialized.ListDictionary GetEmailInputs(bool insertSample)
        {
            return GetEmailInputs(insertSample, null);
        }
        private System.Collections.Specialized.ListDictionary GetEmailInputs(bool insertSample, string email)
        {
            string subject = txtSubject.Text.Trim();
            string header = txtHeader.Text.Trim();

            string body = txtBody.Text.Trim();
            string closing = txtClosing.Text.Trim();

            //do replacements
            System.Collections.Specialized.ListDictionary dict = new System.Collections.Specialized.ListDictionary();

            dict.Add("<PARAM>SUBJECT</PARAM>", (subject.Trim().Length > 0) ? subject.Trim() : "subject of the email");
            dict.Add("<PARAM>HEADER</PARAM>", (header.Trim().Length > 0) ? header.Trim() : "header goes here");
            dict.Add("<PARAM>BODY</PARAM>", body);
            dict.Add("<PARAM>CLOSING</PARAM>", (closing.Trim().Length > 0) ? closing.Trim() : "closing goes here");
            
            //all param names are in uppercase
            foreach (string s in ParamNames)
            {
                if (s.ToLower() == "email" || s.ToLower() == "emailaddress")
                    dict.Add(string.Format("<PARAM>{0}</PARAM>", s),
                        (email != null && email.Trim().Length > 0) ? email : (insertSample) ? string.Format("sample@{0}.com", s.ToLower()) : string.Empty);
                else
                    dict.Add(string.Format("<PARAM>{0}</PARAM>", s), (insertSample) ? string.Format("sample_{0}", s.ToLower()) : string.Empty);
            }

            return dict;
        }
        #endregion

        private string CreateEmailDisplayFromPageValues()
        {
            return CreateEmailDisplayFromPageValues(null);
        }
        private string CreateEmailDisplayFromPageValues(string email)
        {
            //get template
            string mappedFile = Server.MapPath(string.Format("/{0}/MailTemplates/SiteTemplates/{1}",
                _Config._VirtualResourceDir, StarterTemplate));

            string file = Utils.FileLoader.FileToString(mappedFile);

            if (file.Trim().Length > 0 && file.IndexOf("Could not find file") == -1)
            {
                //remove body def
                int startBodyTag = file.IndexOf("<body>") + 6;
                int endBodyTag = file.IndexOf("</body>");

                file = file.Remove(endBodyTag).Substring(startBodyTag);

                //display
                System.Collections.Specialized.ListDictionary dict = GetEmailInputs(true, email);
                //return Utils.ParseHelper.DoReplacements(file, dict, true);

                string sd = Utils.ParseHelper.DoReplacements(file, dict, true);
                return sd;
            }
            else
                return string.Empty;
        }
        protected void litSample_DataBinding(object sender, EventArgs e)
        {
            litSample.Text = CreateEmailDisplayFromPageValues();
            litSubject.DataBind();
        }
        protected void litSubject_DataBinding(object sender, EventArgs e)
        {
            string subject = txtSubject.Text.Trim();
            litSubject.Text = (subject.Trim().Length > 0) ? subject.Trim() : "subject of the email";
        }
        protected void btnTest_Click(object sender, EventArgs e)
        {
            string email = txtTestEmail.Text.Trim();

            if (!Utils.Validation.IsValidEmail(email))
            {
                lblStatus.Text = "Please enter a valid email";
                lblStatus.ForeColor = System.Drawing.Color.Red;
            }
            else if (email.Length > 0)
            {
                string mappedFile = Server.MapPath(string.Format("/{0}/MailTemplates/SiteTemplates/{1}", 
                    _Config._VirtualResourceDir, StarterTemplate));
                string file = Utils.FileLoader.FileToString(mappedFile);

                System.Collections.Specialized.ListDictionary dict = GetEmailInputs(true);

                MailQueue.SendEmail(_Config._CustomerService_Email, _Config._CustomerService_FromName, email, null, null,
                    txtSubject.Text.Trim(), file, null, dict, true, null);

                lblStatus.Text = string.Format("A test email has been sent to {0}", email.ToLower());
                lblStatus.ForeColor = System.Drawing.Color.Green;

                txtTestEmail.Text = string.Empty;

                litSample.DataBind();
            }
        }
        private void SaveProper(string subject, string header, string body, string closing, string mappedFileAndPath)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            SubSonic.QueryCommand query = new SubSonic.QueryCommand(string.Empty, SubSonic.DataService.Provider.Name);

            //Save a record of this email to a file
            //get template
            string virtualTemplatePath = string.Format(string.Format("/{0}/MailTemplates/SiteTemplates/{1}",
                _Config._VirtualResourceDir, StarterTemplate));
            string mappedTemplate = Server.MapPath(virtualTemplatePath);
            string sourceTemplate = Utils.FileLoader.FileToString(mappedTemplate);

            if (sourceTemplate.Trim().Length > 0)
            {
                string fileName = Path.GetFileName(mappedFileAndPath);
                string virtualPathAndFile = string.Format("/{0}/MailTemplates/{1}/{2}", _Config._VirtualResourceDir, MailTemplateSubDirectory, fileName);


                //only replace header,subject,closing,footer
                //we want to leave the per customer substitutions in here
                System.Collections.Specialized.ListDictionary dict = new System.Collections.Specialized.ListDictionary();
                dict.Add("<PARAM>SUBJECT</PARAM>", subject);
                dict.Add("<PARAM>HEADER</PARAM>", header);

                //body = body.Replace(Environment.NewLine, "<br/>");
                dict.Add("<PARAM>BODY</PARAM>", body);
                dict.Add("<PARAM>CLOSING</PARAM>", closing);
                
                string fileContents = Utils.ParseHelper.DoReplacements(sourceTemplate, dict);
                
                
                Utils.FileLoader.SaveToFile(mappedFileAndPath, fileContents, true);

                //in the database - we don't need to save a copy of email - we are saving the email as a template already
                query.Parameters.Add("@name", fileName);
                query.Parameters.Add("@htmlVersion", virtualPathAndFile);
                query.Parameters.Add("@subject", subject);
                query.Parameters.Add("@now", DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));
                query.Parameters.Add("@appId", _Config.APPLICATION_ID, DbType.Guid);

                sb.Append("IF EXISTS (SELECT * FROM [EmailLetter] WHERE [Name] = @name) BEGIN ");
                sb.Append("UPDATE [EmailLetter] SET [Name] = @name, [HtmlVersion] = @htmlVersion, [Subject] = @subject WHERE [ApplicationId] = @appId AND [Name] = @name SELECT [Id] FROM [EmailLetter] WHERE [ApplicationId] = @appId AND [Name] = @name ");
                sb.Append(" END ELSE BEGIN ");
                sb.Append("INSERT [EmailLetter] ([ApplicationId], [Name], [HtmlVersion], [Subject], [dtStamp]) ");
                sb.Append("VALUES (@appId, @name, @htmlVersion, @subject, @now) SELECT SCOPE_IDENTITY() END ");

                query.CommandSql = sb.ToString();
                int emailId = int.Parse(SubSonic.DataService.ExecuteScalar(query).ToString());

                //set current emailletter
                this.ControlLetter = EmailLetter.FetchByID(emailId);
            }
        }
        
        protected void btnSaveTemplate_Click(object sender, EventArgs e)
        {
            try
            {
                Button btn = (Button)sender;
                if (btn.CommandName.ToLower() == "cancel")
                {
                    lblStatus.Text = string.Empty;
                    btnSaveTemplate.Enabled = true;
                    btnOverwriteTemplate.Enabled = false;
                    txtTemplateName.Text = (this.ControlLetter != null) ? Path.GetFileNameWithoutExtension(this.ControlLetter.Name) : string.Empty;
                    return;
                }

                _errors.Clear();

                //if we are not on the overwrite path of execution
                bool isOverwriteOperation = btn.CommandName.ToLower() == "overwrite";
                //otherwise an initial save operation with no overwrite capability

                //get file name & validate inputs
                string fileName = txtTemplateName.Text.Trim();

                if (fileName.Length == 0)
                    _errors.Add("File name required");

                fileName = System.Text.RegularExpressions.Regex.Replace(fileName, @"\s+", string.Empty);

                //check for bad chars
                if (!Utils.Validation.IsValidFileNameOnly(Path.GetFileNameWithoutExtension(fileName)))
                    _errors.Add("Invalid file name");

                //we also need valid input for constructing the email
                //subject is required
                string subject = txtSubject.Text.ToString();
                if (subject.Length == 0)
                    _errors.Add("Subject is required");
                string header = txtHeader.Text.ToString();
                if (header.Length == 0)
                    _errors.Add("Header is required");
                string body = txtBody.Text.ToString();
                if (body.Length == 0)
                    _errors.Add("Body is required");
                string closing = txtClosing.Text.ToString();
                if (closing.Length == 0)
                    _errors.Add("Complimentary closing is required");

                if (Utils.Validation.IncurredErrors(_errors, CustomSave))
                {
                    return;
                }

                //if exists - show option for overwrite
                string virtualPath = string.Format("/{0}/MailTemplates/{1}/", _Config._VirtualResourceDir, MailTemplateSubDirectory);
                string virtualPathAndFile = string.Format("{0}{1}.html", virtualPath, Path.GetFileNameWithoutExtension(fileName));//force html
                string mappedFile = Server.MapPath(virtualPathAndFile);

                if ((! isOverwriteOperation ) && File.Exists(mappedFile))
                {
                    //enable overwrite option
                    btnOverwriteTemplate.Enabled = true;
                    btnSaveTemplate.Enabled = false;
                    lblStatus.Text = "This file name already exists - would you like to overwrite the existing file?";
                    lblStatus.ForeColor = System.Drawing.Color.Red;
                    return;
                }

                //otherwise we do not have a matching file - so we dont care - 
                //or we have opted to overwrite and go ahead with the current chosen filename
                //so now we have a valid file name
                SaveProper(subject, header, body, closing, mappedFile);

                string finalFileName = Path.GetFileName(mappedFile);

                //notify user of success/failure
                //txtTemplateName.Text = string.Empty;
                lblStatus.Text = string.Format("File saved as {0}", finalFileName);
                lblStatus.ForeColor = System.Drawing.Color.Green;

                btnSaveTemplate.Enabled = true;
                btnOverwriteTemplate.Enabled = false;

                txtTemplateName.Text = string.Empty;

                //rebind the file list and select the current file
                ddlTemplates.DataBind();

                litSample.DataBind();
            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message;
                lblStatus.ForeColor = System.Drawing.Color.Red;
            }
        }
}
}