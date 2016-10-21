using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

using Wcss;

namespace WillCallWeb.Admin.AdminControls.MailerTemplating
{
    public partial class MailerTemplate_Select : BaseControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            FormTemplate.Visible = (this.Page.User.IsInRole("Super"));

            if (!IsPostBack)
            {
                //FormView1.DataBind();
            }
        }

        #region SELECTING

        protected void ddlMailerList_DataBound(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;

            //allow for selection title
            if (ddl.Items.Count > 1 && ddl.SelectedIndex <= 0 && Atx.CurrentMailer != null)
            {
                ddl.SelectedIndex = -1;
                ListItem li = ddl.Items.FindByValue(Atx.CurrentMailer.Id.ToString());
                if(li != null)
                    li.Selected = true;
            }
        }
        protected void ddlMailerList_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;

            int idx = int.Parse(ddl.SelectedValue);

            //assign current mailer
            Atx.CurrentMailer = (idx > 0) ? Mailer.FetchByID(idx) : null;

            //sync current mailer template
            Atx.CurrentMailerTemplate = (Atx.CurrentMailer != null) ? Wcss.MailerTemplate.FetchByID(Atx.CurrentMailer.TMailerTemplateId) : null;

            if (idx > 0)
                //go to mailercontent
                base.Redirect("/Admin/Mailers.aspx?p=mlredit");
        }
        protected void ddlTemplateList_DataBound(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;

            //allow for selection title
            if (ddl.Items.Count > 1 && ddl.SelectedIndex <= 0 && Atx.CurrentMailerTemplate != null)
            {
                ddl.SelectedIndex = -1;
                ListItem li = ddl.Items.FindByValue(Atx.CurrentMailerTemplate.Id.ToString());
                if(li != null)
                    li.Selected = true;

                //make sure the currentMailer is in sync
            }
        }
        protected void ddlTemplateList_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;

            int idx = int.Parse(ddl.SelectedValue);

            //assign current mailer
            Atx.CurrentMailerTemplate = (idx > 0) ? MailerTemplate.FetchByID(idx) : null;

            if(Atx.CurrentMailerTemplate == null)
                Atx.CurrentMailer = null;
            else if (Atx.CurrentMailer != null && Atx.CurrentMailer.TMailerTemplateId != Atx.CurrentMailerTemplate.Id)
                Atx.CurrentMailer = null;//reset the mailer
                
            if (idx > 0)
                //go to templatecontent
                base.Redirect("/Admin/Mailers.aspx?p=tpledit");
        }

        #endregion

        #region INSERTING

        #region MAILER

        protected void FormMailer_Inserting(object sender, FormViewInsertEventArgs e)
        {
            FormView form = (FormView)sender;
            DropDownList ddlTemplateList = (DropDownList)form.FindControl("ddlTemplateList");
            TextBox name = (TextBox)form.FindControl("txtName");
            TextBox subject = (TextBox)form.FindControl("txtSubject");
            bool isValidInput = true;

            if (name != null && subject != null)
            {
                if (name.Text.Trim().Length == 0)
                {
                    RequiredFieldValidator rName = (RequiredFieldValidator)form.FindControl("RequiredName");
                    if (rName != null) rName.IsValid = false;
                    isValidInput = false;
                }
                else
                    e.Values["Name"] = string.Format("{0}_{1}", DateTime.Now.ToString("yyMMddhhmmss"), e.Values["Name"]);

                if (subject.Text.Trim().Length == 0)
                {
                    RequiredFieldValidator rSubject = (RequiredFieldValidator)form.FindControl("RequiredSubject");
                    if (rSubject != null) rSubject.IsValid = false;
                    isValidInput = false;
                }
            }

            if (ddlTemplateList != null)
            {
                int idx = int.Parse(ddlTemplateList.SelectedValue);
                if (idx <= 0)
                {
                    CustomValidator val = (CustomValidator)form.FindControl("rowValidator");
                    if (val != null)
                    {
                        val.IsValid = false;
                        val.ErrorMessage = "You must select a template";
                    }
                    isValidInput = false;
                }
                else
                    e.Values["tMailerTemplateId"] = idx.ToString();
            }

            if (!isValidInput)
            {
                e.Cancel = true;
            }
        }
        protected void SqlInsertMailer_Inserted(object sender, SqlDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                int insertIdx = (int)e.Command.Parameters["@insId"].Value;
                if (insertIdx > 0)
                {
                    Mailer ent = Mailer.FetchByID(insertIdx);
                    Atx.CurrentMailer = ent;

                    Atx.CurrentMailerTemplate = MailerTemplate.FetchByID((Atx.CurrentMailer != null) ? Atx.CurrentMailer.TMailerTemplateId : 0);
                }
            }
        }
        protected void FormMailer_Inserted(object sender, FormViewInsertedEventArgs e)
        {
            if (ShowException(e.Exception))
            {
                e.KeepInInsertMode = true;
                e.ExceptionHandled = true;
            }
            else
            {
                e.KeepInInsertMode = false;
                FormView form = (FormView)sender;
                form.ChangeMode(FormViewMode.Edit);

                base.Redirect("/Admin/Mailers.aspx?p=mlredit");
            }
        }

        protected void ddlSubject_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            if (ddl.SelectedIndex > 0)//allow for selection item
            {
                TextBox txtSubject = (TextBox)FormMailer.FindControl("txtSubject");
                if (txtSubject != null)
                    txtSubject.Text = ddl.SelectedValue;
            }
        }
        protected void FormMailer_ModeChanging(object sender, FormViewModeEventArgs e)
        {
            FormView form = (FormView)sender;
            form.ChangeMode(e.NewMode);

            if (e.CancelingEdit)//handles cancel correctly
                form.DataBind();

            FormTemplate.Visible = (this.Page.User.IsInRole("Super") && e.NewMode != FormViewMode.Insert);
        }

        #endregion

        #region TEMPLATE

        protected void FormTemplate_Inserting(object sender, FormViewInsertEventArgs e)
        {
            FormView form = (FormView)sender;            
            TextBox name = (TextBox)form.FindControl("txtName");
            TextBox description = (TextBox)form.FindControl("txtDescription");
            bool isValidInput = true;

            if (name != null && description != null)
            {
                if (name.Text.Trim().Length == 0)
                {
                    RequiredFieldValidator rName = (RequiredFieldValidator)form.FindControl("RequiredName");
                    if (rName != null) rName.IsValid = false;
                    isValidInput = false;
                }
            }

            //if we are copying....make sure we have a valid selection
            CheckBox chkCopy = (CheckBox)form.FindControl("chkCopyTemplate");
            DropDownList ddlTemplate = (DropDownList)form.FindControl("ddlCopyTemplate");
            if (chkCopy != null && ddlTemplate != null)
            {
                if (chkCopy.Checked)
                {
                    int selIdx = int.Parse(ddlTemplate.SelectedValue);
                    if (selIdx <= 0)
                    {
                        CustomValidator rName = (CustomValidator)form.FindControl("CustomCopy");
                        if (rName != null) rName.IsValid = false;
                        isValidInput = false;
                    }
                }
            }

            if (!isValidInput)
            {
                e.Cancel = true;
            }
        }
        protected void SqlInsertTemplate_Inserting(object sender, SqlDataSourceCommandEventArgs e)
        {
            e.Command.Parameters["@appId"].Value = Wcss._Config.APPLICATION_ID;
        }
        protected void SqlInsertTemplate_Inserted(object sender, SqlDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                int insertIdx = (int)e.Command.Parameters["@insId"].Value;
                if (insertIdx > 0)
                {
                    MailerTemplate ent = MailerTemplate.FetchByID(insertIdx);
                    Atx.CurrentMailerTemplate = ent;

                    //a new template willl never have a matching mailer
                    Atx.CurrentMailer = null;
                }
            }
        }
        protected void FormTemplate_Inserted(object sender, FormViewInsertedEventArgs e)
        {
            if (ShowException(e.Exception))
            {
                e.KeepInInsertMode = true;
                e.ExceptionHandled = true;
            }
            else
            {
                e.KeepInInsertMode = false;
                FormView form = (FormView)sender;

                //if we were copying a template.....
                CheckBox chkCopy = (CheckBox)form.FindControl("chkCopyTemplate");
                DropDownList ddlTemplate = (DropDownList)form.FindControl("ddlCopyTemplate");
                if (chkCopy != null && ddlTemplate != null)
                {
                    if (chkCopy.Checked)
                    {
                        int selIdx = int.Parse(ddlTemplate.SelectedValue);
                        if (selIdx > 0)
                        {
                            //copy template from selection
                            MailerTemplate copyFrom = MailerTemplate.FetchByID(selIdx);
                            if (copyFrom != null)
                            {
                                MailerTemplate copyTo = Atx.CurrentMailerTemplate;
                                copyTo.Style = copyFrom.Style;
                                copyTo.Header = copyFrom.Header;
                                copyTo.Footer = copyFrom.Footer;
                                copyTo.Save();

                                foreach (MailerTemplateContent mtc in copyFrom.MailerTemplateContentRecords())
                                {
                                    MailerTemplateContent content = new MailerTemplateContent();
                                    content.DtStamp = DateTime.Now;
                                    content.TMailerTemplateId = copyTo.Id;
                                    content.DisplayOrder = mtc.DisplayOrder;
                                    content.VcTemplateAsset = mtc.VcTemplateAsset;
                                    content.Name = mtc.Name;
                                    content.Title = mtc.Title;
                                    content.Template = mtc.Template;
                                    content.SeparatorTemplate = mtc.SeparatorTemplate;
                                    content.MaxListItems = mtc.MaxListItems;
                                    content.MaxSelections = mtc.MaxSelections;
                                    content.VcFlags = mtc.VcFlags;
                                    content.Save();

                                    foreach (MailerTemplateSubstitution sub in mtc.MailerTemplateSubstitutionRecords())
                                    {
                                        MailerTemplateSubstitution newSub = new MailerTemplateSubstitution();
                                        newSub.DtStamp = DateTime.Now;
                                        newSub.TMailerTemplateContentId = content.Id;
                                        newSub.TagName = sub.TagName;
                                        newSub.TagValue = sub.TagValue;
                                        newSub.Save();
                                        content.MailerTemplateSubstitutionRecords().Add(newSub);
                                    }

                                    copyTo.MailerTemplateContentRecords().Add(content);
                                }

                                Atx.RefreshMailer();                                
                            }
                        }
                    }
                }

                form.ChangeMode(FormViewMode.Edit);

                base.Redirect("/Admin/Mailers.aspx?p=tpledit");
            }
        }

        protected void FormTemplate_ModeChanging(object sender, FormViewModeEventArgs e)
        {
            FormView form = (FormView)sender;
            form.ChangeMode(e.NewMode);

            if (e.CancelingEdit)//handles cancel correctly
                form.DataBind();

            FormMailer.Visible = (e.NewMode != FormViewMode.Insert);
        }

        #endregion

        #endregion
    }
}