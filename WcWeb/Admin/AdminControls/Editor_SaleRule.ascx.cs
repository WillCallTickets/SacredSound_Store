using System;
using System.Collections;
using System.Web.UI.WebControls;

using Wcss;

namespace WillCallWeb.Admin.AdminControls
{
    public partial class Editor_SaleRule : BaseControl
    {
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            //set opacity for nav events
            if (this.HasControls() && this.UpdatePanel1.Visible)
                Atx.RegisterJQueryScript_BlockUI_AjaxMethod(this.UpdatePanel1, "#saleruleeditor", true);
        }
        private SaleRuleCollection OrderedCollection
        {
            get
            {
                SaleRuleCollection coll = new SaleRuleCollection();
                coll.AddRange(_Lookits.SaleRules);
                if (coll.Count > 1)
                    coll.Sort("IDisplayOrder", true);

                return coll;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
               GridView1.DataBind();
            }
        }
        protected void GridView1_DataBinding(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;

            grid.DataSource = OrderedCollection;
            string[] keyNames = { "Id" };
            grid.DataKeyNames = keyNames;
        }
        protected void GridView1_DataBound(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;

            if (grid.SelectedIndex == -1 && grid.Rows.Count > 0)
                grid.SelectedIndex = 0;

            FormView1.DataBind();
        }
        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            FormView1.DataBind();
        }
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridView grid = (GridView)sender;
            DataControlRowType typ = e.Row.RowType;
            DataControlRowState state = e.Row.RowState;

            if (typ == DataControlRowType.DataRow)
            {   
                SaleRule entity = (SaleRule)e.Row.DataItem;

                Literal text = (Literal)e.Row.FindControl("LiteralText");
                if (text != null && entity != null && entity.DisplayText != null)
                    text.Text = (entity.DisplayText.Length > 100) ?
                        string.Format("{0}...", entity.DisplayText.Substring(0, 100)) : entity.DisplayText;

                LinkButton button = (LinkButton)e.Row.FindControl("btnDelete");
                if (button != null && entity != null)
                {
                    string desc = (entity.DisplayText.Length > 50) ?
                        string.Format("{0}...", entity.DisplayText.Substring(0, 50)) : entity.DisplayText;
                    button.OnClientClick = string.Format("return confirm('Are you sure you want to delete {0}?')",
                        Utils.ParseHelper.ParseJsAlert(desc));
                }

                LinkButton up = (LinkButton)e.Row.FindControl("btnUp");
                LinkButton down = (LinkButton)e.Row.FindControl("btnDown");

                if (up != null && down != null)
                {
                    up.Enabled = (e.Row.RowIndex > 0);
                    down.Enabled = (e.Row.RowIndex < (((ICollection)grid.DataSource).Count - 1));
                }
            }
        }
        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridView grid = (GridView)sender;
            int idx = (int)grid.DataKeys[e.RowIndex]["Id"];

            try
            {
                _Lookits.SaleRules.DeleteFromCollection(idx);
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
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridView grid = (GridView)sender;
            string cmd = e.CommandName.ToLower();

            switch (cmd)
            {
                case "up":
                case "down":
                    SaleRule moved = _Lookits.SaleRules.ReorderItem(int.Parse(e.CommandArgument.ToString()), cmd);
                    //set the index of the moved item
                    grid.SelectedIndex = moved.DisplayOrder;
                    grid.DataBind();
                    break;
            }
        }
        protected void FormView1_DataBinding(object sender, EventArgs e)
        {
            FormView form = (FormView)sender;

            int idx = (GridView1.SelectedDataKey != null) ? (int)GridView1.SelectedDataKey["Id"] : 0;

            SaleRuleCollection coll = new SaleRuleCollection();
            SaleRule addRule = (SaleRule)OrderedCollection.Find(idx);
            if(addRule != null)
                coll.Add(addRule);

            form.DataSource = coll;
            string[] keyNames = { "Id" };
            form.DataKeyNames = keyNames;
        }
        protected void FormView1_DataBound(object sender, EventArgs e)
        {
            FormView form = (FormView)sender;

            int idx = (GridView1.SelectedDataKey != null) ? (int)GridView1.SelectedDataKey["Id"] : 0;
            SaleRule entity = (SaleRule)_Lookits.SaleRules.Find(idx);

            if (entity != null)
            {
                Literal desc = (Literal)form.FindControl("litDescription");
                if (desc != null && entity != null && entity.DisplayText != null)
                    desc.Text = (entity.DisplayText.Length > 40) ?
                        string.Format("{0}...", entity.DisplayText.Substring(0, 40)) : entity.DisplayText;

                //fill ddlContext
                DropDownList ddlContext = (DropDownList)form.FindControl("ddlContext");
                if (ddlContext != null && ddlContext.Items.Count == 0)
                {
                    ddlContext.DataSource = Enum.GetNames(typeof(_Enums.ProductContext));
                    ddlContext.DataBind();
                }

                if (ddlContext.Items.Count > 0 && ddlContext.SelectedIndex == -1)
                {
                    if (entity == null)
                        ddlContext.SelectedIndex = 0;
                    else
                        ddlContext.Items.FindByText(entity.Context.ToString()).Selected = true;
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
        protected void FormView1_ItemUpdating(object sender, FormViewUpdateEventArgs e)
        {
            if (Page.IsValid)
            {
                FormView form = (FormView)sender;

                try
                {
                    //name,context,isactive,text
                    TextBox name = (TextBox)form.FindControl("txtName");
                    DropDownList ddlContext = (DropDownList)form.FindControl("ddlContext"); 
                    CheckBox isActive = (CheckBox)form.FindControl("chkActive"); 
                    TextBox text = (TextBox)form.FindControl("txtText");
                    
                    string idx = GridView1.SelectedDataKey["Id"].ToString();

                    SaleRule entity = (SaleRule)_Lookits.SaleRules.Find(int.Parse(idx));

                    if (entity != null && name != null && ddlContext != null && isActive != null && text != null)
                    {
                        if (entity.Name != name.Text.Trim())
                            entity.Name = name.Text.Trim();

                        if (entity.Context.ToString() != ddlContext.SelectedValue)
                            entity.Context = (_Enums.ProductContext)Enum.Parse(typeof(_Enums.ProductContext), ddlContext.SelectedValue, true);

                        if (entity.IsActive != isActive.Checked)
                            entity.IsActive = isActive.Checked;

                        if (entity.DisplayText != text.Text.Trim())
                            entity.DisplayText = text.Text.Trim();

                        entity.Save();

                        _Lookits.RefreshLookup(_Enums.LookupTableNames.SaleRules.ToString());

                        e.Cancel = false;

                        GridView1.DataBind();
                    }
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
            if (Page.IsValid)
            {
                FormView form = (FormView)sender;

                try
                {
                    //context,text
                    DropDownList ddlContext = (DropDownList)form.FindControl("ddlContext");
                    TextBox name = (TextBox)form.FindControl("txtName");
                    TextBox text = (TextBox)form.FindControl("txtText");

                    SaleRule newItem = _Lookits.SaleRules.AddToCollection(ddlContext.SelectedValue, name.Text.Trim(), text.Text.Trim());

                    _Lookits.RefreshLookup(_Enums.LookupTableNames.SaleRules.ToString());

                    form.ChangeMode(FormViewMode.Edit);

                    GridView1.SelectedIndex = newItem.DisplayOrder;
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
}
}