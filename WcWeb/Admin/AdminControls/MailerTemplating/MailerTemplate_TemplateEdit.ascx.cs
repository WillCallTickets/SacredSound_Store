using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

using Wcss;

namespace WillCallWeb.Admin.AdminControls.MailerTemplating
{
    public partial class MailerTemplate_TemplateEdit : BaseControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(Atx.CurrentMailerTemplate == null)
                base.Redirect("/Admin/Mailers.aspx?p=select");

            if (!IsPostBack)
            {
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            FormTemplate.UpdateItem(true);
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            FormTemplate.DataBind();
        }
        //select
        protected void SqlTemplate_SelectId(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters["@idx"].Value = Atx.CurrentMailerTemplate.Id;
        }
        //update
        protected void SqlTemplate_UpdateId(object sender, SqlDataSourceCommandEventArgs e)
        {
            e.Command.Parameters["@idx"].Value = Atx.CurrentMailerTemplate.Id;
        }
        protected void FormTemplate_ItemUpdated(object sender, FormViewUpdatedEventArgs e)
        {
            if (ShowException(e.Exception))
                e.ExceptionHandled = true;
            else
                Atx.RefreshMailer();
        }
        protected void FormTemplate_ModeChanging(object sender, FormViewModeEventArgs e)
        {
            FormView form = (FormView)sender;
            form.ChangeMode(e.NewMode);

            if (e.CancelingEdit)//handles cancel correctly
                form.DataBind();
        }
    }
}