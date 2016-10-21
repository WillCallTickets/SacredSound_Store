using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

using Wcss;

namespace WillCallWeb.Admin.AdminControls.MailerTemplating
{
    public partial class MailerTemplate_TemplatePart : BaseControl
    {
        protected MailerTemplateContent CurrentTemplateContent
        {
            get
            {
                if(GridList.DataKeys.Count == 0)
                   return null;

                if(GridList.SelectedIndex == -1)
                    GridList.SelectedIndex = 0;

                int idx = (int)GridList.SelectedValue;

                return (MailerTemplateContent)Atx.CurrentMailerTemplate.MailerTemplateContentRecords().Find(idx);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if(Atx.CurrentMailerTemplate == null)
                base.Redirect("/Admin/Mailers.aspx?p=select");

            if (!IsPostBack)
            {
                //FormView1.DataBind();
            }
        }

        #region GRID - TODO: bind form after this binds

        protected void GridList_DataBound(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;

            if (grid.Rows.Count > 0 && grid.SelectedIndex == -1)
            {
                grid.SelectedIndex = 0;
            }
        }
        protected void GridList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridView grid = (GridView)sender;

            string cmd = e.CommandName.ToLower();

            switch (cmd)
            {
                case "up":
                case "down":
                    MailerTemplateContent mtc = Atx.CurrentMailerTemplate.MailerTemplateContentRecords().ReorderItem(int.Parse(e.CommandArgument.ToString()), cmd);
                    //set the index of the moved item
                    grid.SelectedIndex = mtc.DisplayOrder;
                    grid.DataBind();
                    break;
            }

            //rebind the form
            if (FormEntity1.CurrentMode != FormViewMode.Edit)
            {
                FormEntity1.ChangeMode(FormViewMode.Edit);
                FormEntity1.DataBind();//we need to follow if the selected row has changed
            }
        }
        protected void GridList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridView grid = (GridView)sender;
            int idx = (int)grid.DataKeys[e.RowIndex]["Id"];

            try
            {
                //The cascade deletes corresponding mailercontent
                Atx.CurrentMailerTemplate.MailerTemplateContentRecords().DeleteFromCollection(idx);
                Atx.RefreshMailer();
                grid.SelectedIndex = -1;
                grid.DataBind();
            }
            catch (Exception ex)
            {
                _Error.LogException(ex);
                CustomValidation.IsValid = false;
                CustomValidation.ErrorMessage = ex.Message;
                e.Cancel = true;
            }
        }

        //select
        protected void SqlMailerTemplateContentList_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters["@templateId"].Value = Atx.CurrentMailerTemplate.Id;
        }

        #endregion

        #region FORM

        protected void FormEntity1_ItemUpdating(object sender, FormViewUpdateEventArgs e)
        {
            FormView form = (FormView)sender;
            DropDownList ddl = (DropDownList)form.FindControl("ddlAsset");
            if(ddl != null)
                e.NewValues["TemplateAsset"] = ddl.SelectedValue;
        }
        protected void FormEntity1_ItemUpdated(object sender, FormViewUpdatedEventArgs e)
        {
            if (ShowException(e.Exception))
                e.ExceptionHandled = true;
            else
                Atx.RefreshMailer();
        }
        protected void ddlAsset_DataBinding(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            string[] list = Enum.GetNames(typeof(MailerTemplateContent.ContentAsset));
            ddl.DataSource = list;
        }
        protected void ddlAsset_DataBound(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;

            if(FormEntity1.CurrentMode == FormViewMode.Insert && ddl.SelectedIndex == -1 && ddl.Items.Count > 0)
                ddl.SelectedIndex = 0;
            else if (FormEntity1.CurrentMode != FormViewMode.Insert && CurrentTemplateContent != null)
            {
                ListItem li = ddl.Items.FindByText(CurrentTemplateContent.TemplateAsset.ToString());
                if (li != null)
                {
                    ddl.SelectedIndex = -1;
                    li.Selected = true;
                }
            }
        }
        protected void FormEntity1_Inserting(object sender, FormViewInsertEventArgs e)
        {
            e.Values["tMailerTemplateId"] = Atx.CurrentMailerTemplate.Id;
            e.Values["iDisplayOrder"] = GridList.Rows.Count;

            FormView form = (FormView)sender;
            DropDownList ddl = (DropDownList)form.FindControl("ddlAsset");
            if(ddl != null)
                e.Values["TemplateAsset"] = ddl.SelectedValue;
        }
        protected void FormEntity1_Inserted(object sender, FormViewInsertedEventArgs e)
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

                //reset current objects
                //rebind the grid
                GridList.DataBind();
                //set selected to last
                GridList.SelectedIndex = GridList.Rows.Count - 1;
            }
        }
        protected void FormEntity1_ModeChanging(object sender, FormViewModeEventArgs e)
        {
            FormView form = (FormView)sender;
            form.ChangeMode(e.NewMode);

            if (e.CancelingEdit)//handles cancel correctly
                form.DataBind();
        }

        #endregion

    }
}