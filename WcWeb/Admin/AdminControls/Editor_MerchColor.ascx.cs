using System;
using System.Collections;
using System.Web.UI.WebControls;

using Wcss;

namespace WillCallWeb.Admin.AdminControls
{
    public partial class Editor_MerchColor : BaseControl
    {
        private MerchColorCollection OrderedCollection
        {
            get
            {
                MerchColorCollection coll = new MerchColorCollection();
                coll.AddRange(_Lookits.MerchColors);
                if (coll.Count > 1)
                    coll.Sort("IDisplayOrder", true);

                return coll;
            }
        }
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            //set opacity for nav events
            if (this.HasControls() && this.UpdatePanel1.Visible)
                Atx.RegisterJQueryScript_BlockUI_AjaxMethod(this.UpdatePanel1, "#merchcoloreditor", true);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GridView1.DataBind();
            }

            btnMerch.Text = (Atx.CurrentMerchRecord != null) ? Atx.CurrentMerchRecord.DisplayName : "Merch Item Editor";
        }
        protected void btnMerch_Click(object sender, EventArgs e)
        {
            base.Redirect(string.Format("/Admin/MerchEditor.aspx?p=itemEdit&merchitem={0}", (Atx.CurrentMerchRecord != null) ? Atx.CurrentMerchRecord.Id : 0));
        }

        #region GridView

        protected void GridView1_DataBinding(object sender, EventArgs e)
        {
            //set data source
            GridView grid = (GridView)sender;
            grid.DataSource = OrderedCollection;
            string[] keyNames = { "Id", "Name" };
            grid.DataKeyNames = keyNames;
        }
        protected void GridView1_DataBound(object sender, EventArgs e)
        {
            //select default index
            GridView grid = (GridView)sender;

            if (grid.DataSource != null && ((ICollection)grid.DataSource).Count > 0 && grid.SelectedIndex == -1)
                grid.SelectedIndex = 0;

            this.FormView1.DataBind();
        }
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridView grid = (GridView)sender;
            DataControlRowType typ = e.Row.RowType;
            DataControlRowState state = e.Row.RowState;

            if (typ == DataControlRowType.DataRow)
            {
                LinkButton button = (LinkButton)e.Row.FindControl("btnDelete");

                if (button != null && e.Row.DataItem != null)
                {
                    MerchColor entity = (MerchColor)e.Row.DataItem;
                    button.OnClientClick = string.Format("return confirm('Are you sure you want to delete {0}?')",
                        Utils.ParseHelper.ParseJsAlert(entity.Name));
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
                _Lookits.MerchColors.DeleteFromCollection(idx);
                grid.SelectedIndex = 0;
                grid.DataBind();
            }
            catch (Exception ex)
            {
                _Error.LogException(ex);
                CustomValidation.IsValid = false;
                CustomValidation.ErrorMessage = ex.Message;
            }
        }
        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;
            grid.DataBind();
        }
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridView grid = (GridView)sender;
            string cmd = e.CommandName.ToLower();

            switch (cmd)
            {
                case "up":
                case "down":
                    MerchColor moved = _Lookits.MerchColors.ReorderItem(int.Parse(e.CommandArgument.ToString()), cmd);
                    //set the index of the moved item
                    grid.SelectedIndex = moved.DisplayOrder;
                    grid.DataBind();
                    break;
            }
        }

        #endregion

        #region Details

        protected void FormView1_DataBinding(object sender, EventArgs e)
        {
            int idx = (GridView1.SelectedDataKey != null && GridView1.SelectedDataKey["Id"] != null) ? 
                (int)GridView1.SelectedDataKey["Id"] : 0;

            MerchColorCollection selected = new MerchColorCollection();
            if (idx > 0)
            {
                MerchColor addColor = (MerchColor)OrderedCollection.Find(idx);
                if(addColor != null)
                    selected.Add(addColor);
            }

            FormView1.DataSource = selected;
            string[] keyNames = { "Id"};
            FormView1.DataKeyNames = keyNames;
        }
        protected void FormView1_DataBound(object sender, EventArgs e)
        {
            //no lists to fill
        }
        protected void FormView1_ItemCommand(object sender, FormViewCommandEventArgs e)
        {
            FormView form = (FormView)sender;
            string cmd = e.CommandName.ToLower();

            switch (cmd)
            {
                case "cancel":
                    //just rebind the control to reset
                    form.ChangeMode(FormViewMode.Edit);
                    break;
            }
        }
        protected void FormView1_ModeChanging(object sender, FormViewModeEventArgs e)
        {
            FormView form = (FormView)sender;
            form.ChangeMode(e.NewMode);
            if (e.CancelingEdit)
                form.DataBind();
        }
        protected void FormView1_ItemInserting(object sender, FormViewInsertEventArgs e)
        {
            if (Page.IsValid)
            {
                FormView form = (FormView)sender;

                try
                {
                    TextBox name = (TextBox)form.FindControl("txtName");

                    MerchColor newItem = _Lookits.MerchColors.AddToCollection(name.Text.Trim());

                    _Lookits.RefreshLookup(_Enums.LookupTableNames.MerchColors.ToString());

                    form.ChangeMode(FormViewMode.Edit);

                    GridView1.SelectedIndex = newItem.DisplayOrder;
                    GridView1.DataBind();
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
        protected void FormView1_ItemUpdating(object sender, FormViewUpdateEventArgs e)
        {
            if (Page.IsValid)
            {
                FormView form = (FormView)sender;

                try
                {
                    TextBox name = (TextBox)form.FindControl("txtName");

                    int idx = (int)GridView1.SelectedDataKey["Id"];

                    MerchColor entity = (MerchColor)_Lookits.MerchColors.Find(idx);

                    if (entity != null && name != null)
                    {
                        if (entity.Name != name.Text.Trim())
                            entity.Name = name.Text.Trim();

                        entity.Save();

                        _Lookits.RefreshLookup(_Enums.LookupTableNames.MerchColors.ToString());

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

        #endregion
}
}