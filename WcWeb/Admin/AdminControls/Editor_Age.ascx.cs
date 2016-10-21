using System;
using System.Web.UI.WebControls;

using Wcss;

namespace WillCallWeb.Admin.AdminControls
{
    public partial class Editor_Age : BaseControl
    {
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
            GridView grid = (GridView)sender;

            grid.DataSource = _Lookits.Ages;

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
                Age entity = (Age)e.Row.DataItem;

                LinkButton button = (LinkButton)e.Row.FindControl("btnDelete");
                if (button != null && entity != null)
                    button.OnClientClick = string.Format("return confirm('Are you sure you want to delete {0}?')",
                        Utils.ParseHelper.ParseJsAlert(entity.Name));
            }
        }
        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridView grid = (GridView)sender;
            int idx = (int)grid.DataKeys[e.RowIndex]["Id"];

            try
            {   
                _Lookits.Ages.DeleteAgeFromCollection(idx);
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

        #endregion

        #region FormView

        protected void FormView1_DataBinding(object sender, EventArgs e)
        {
            FormView form = (FormView)sender;

            int idx = (GridView1.SelectedDataKey != null) ? (int)GridView1.SelectedDataKey["Id"] : 0;

            AgeCollection coll = new AgeCollection();
            Age addAge = (Age)_Lookits.Ages.Find(idx);
            if(addAge != null)
                coll.Add(addAge);

            form.DataSource = coll;
            string[] keyNames = { "Id" };
            form.DataKeyNames = keyNames;
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
                    //name,price, description
                    TextBox txtName = (TextBox)form.FindControl("txtName");

                    string name = txtName.Text.Trim();

                    if (name.Length == 0)
                        throw new Exception("Name is required.");
                    
                    int idx = (form.DataKey["Id"] != null) ? (int)form.DataKey["Id"] : 0;
                    Age entity = (Age)_Lookits.Ages.Find(idx);

                    if (entity != null)
                    {
                        if (entity.Name != name)
                            entity.Name = name;

                        entity.Save();

                        _Lookits.RefreshLookup(_Enums.LookupTableNames.Ages.ToString());

                        //this will work because the lookit is ordered by name
                        GridView1.SelectedIndex = _Lookits.Ages.GetList().FindIndex(delegate(Age match) { return (match.Id == entity.Id); });
                    }

                    e.Cancel = false;
                }
                catch (Exception ex)
                {
                    _Error.LogException(ex);
                    CustomValidation.IsValid = false;
                    CustomValidation.ErrorMessage = ex.Message;

                    e.Cancel = true;
                }

                GridView1.DataBind();
            }
        }
        protected void FormView1_ItemInserting(object sender, FormViewInsertEventArgs e)
        {
            if (Page.IsValid)
            {
                FormView form = (FormView)sender;

                try
                {
                    //name,price, description
                    TextBox txtName = (TextBox)form.FindControl("txtName");

                    string name = txtName.Text.Trim();

                    if (name.Length == 0)
                        throw new Exception("Name is required.");

                    Age newItem = _Lookits.Ages.AddItemToCollection(name);

                    _Lookits.RefreshLookup(_Enums.LookupTableNames.Ages.ToString());

                    form.ChangeMode(FormViewMode.Edit);

                    //this will work because the lookit is ordered by name
                    GridView1.SelectedIndex = _Lookits.Ages.GetList().FindIndex(delegate(Age match) { return (match.Id == newItem.Id); });

                    e.Cancel = false;
                }
                catch (Exception ex)
                {
                    _Error.LogException(ex);
                    CustomValidation.IsValid = false;
                    CustomValidation.ErrorMessage = ex.Message;

                    e.Cancel = true;
                }

                GridView1.DataBind();
            }
        }

        #endregion
    }
}