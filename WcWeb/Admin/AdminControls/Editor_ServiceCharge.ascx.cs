using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using Wcss;

namespace WillCallWeb.Admin.AdminControls
{
    public partial class Editor_ServiceCharge : BaseControl
    {
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            //set opacity for nav events
            if (this.HasControls() && this.UpdatePanel1.Visible)
                Atx.RegisterJQueryScript_BlockUI_AjaxMethod(this.UpdatePanel1, "#servicechargeeditor", true);
        }
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

            grid.DataSource = _Lookits.ServiceCharges;

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
                ServiceCharge entity = (ServiceCharge)e.Row.DataItem;

                LinkButton button = (LinkButton)e.Row.FindControl("btnDelete");
                if (button != null && entity != null)
                    button.OnClientClick = string.Format("return confirm('Are you sure you want to delete {0}?')",
                        Utils.ParseHelper.ParseJsAlert(string.Format("max value: {0} charge: {1}", entity.MaxValue.ToString("c"), entity.Charge.ToString("c"))));
            }
        }
        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridView grid = (GridView)sender;
            
            int idx = (int)grid.DataKeys[e.RowIndex]["Id"];//.SelectedDataKey.Value;

            try
            {
                ServiceCharge.Delete(idx);
                _Lookits.RefreshLookup(_Enums.LookupTableNames.ServiceCharges.ToString());
                
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

            ServiceChargeCollection coll = new ServiceChargeCollection();
            ServiceCharge addCharge = (ServiceCharge)_Lookits.ServiceCharges.Find(idx);
            if(addCharge != null)
                coll.Add(addCharge);

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
                    TextBox txtMax = (TextBox)form.FindControl("txtMax");
                    TextBox txtCharge = (TextBox)form.FindControl("txtCharge");
                    TextBox txtPct = (TextBox)form.FindControl("txtPct");

                    string max = txtMax.Text.Trim();
                    string charge = txtCharge.Text.Trim();
                    string pct = txtPct.Text.Trim();

                    if (max.Length == 0)
                        throw new Exception("Max value is required.");
                    if (charge.Length == 0)
                        throw new Exception("Charge is required.");
                    if (!Utils.Validation.IsDecimal(max))
                        throw new Exception("Please enter a valid max value.");
                    if (!Utils.Validation.IsDecimal(charge))
                        throw new Exception("Please enter a valid charge.");
                    if (!Utils.Validation.IsDecimal(pct))
                        throw new Exception("Please enter a valid percentage.");
                    else if (decimal.Parse(pct) > 0.99M)
                        throw new Exception("Please enter a valid percentage. (between .00 and .99)");

                    decimal mx = decimal.Parse(max);
                    decimal crg = decimal.Parse(charge);
                    decimal percent = (pct.Length > 0) ? decimal.Parse(pct) : 0;

                    ServiceCharge entity = (ServiceCharge)_Lookits.ServiceCharges.Find((GridView1.SelectedDataKey != null) ? (int)GridView1.SelectedDataKey["Id"] : 0);

                    if (entity != null)
                    {
                        if (entity.MaxValue != mx)
                            entity.MaxValue = mx;

                        if (entity.Charge != crg)
                            entity.Charge = crg;

                        if (entity.Percentage != percent)
                            entity.Percentage = percent;

                        entity.Save();

                        _Lookits.RefreshLookup(_Enums.LookupTableNames.ServiceCharges.ToString());

                        //this will work because the lookit is ordered by name
                        GridView1.SelectedIndex = _Lookits.ServiceCharges.GetList().FindIndex(delegate(ServiceCharge match) { return (match.Id == entity.Id); });
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
                    TextBox txtMax = (TextBox)form.FindControl("txtMax");
                    TextBox txtCharge = (TextBox)form.FindControl("txtCharge");
                    TextBox txtPct = (TextBox)form.FindControl("txtPct");

                    string max = txtMax.Text.Trim();
                    string charge = txtCharge.Text.Trim();
                    string pct = txtPct.Text.Trim();

                    if (pct.Length == 0)
                        pct = ".00";

                    if (max.Length == 0)
                        throw new Exception("Max value is required.");
                    if (charge.Length == 0)
                        throw new Exception("Charge is required.");
                    if (!Utils.Validation.IsDecimal(max))
                        throw new Exception("Please enter a valid max value.");
                    if (!Utils.Validation.IsDecimal(charge))
                        throw new Exception("Please enter a valid charge.");
                    if (!Utils.Validation.IsDecimal(pct))
                        throw new Exception("Please enter a valid percentage.");
                    else if (decimal.Parse(pct) > 0.99M)
                        throw new Exception("Please enter a valid percentage. (between .00 and .99)");

                    decimal mx = decimal.Parse(max);
                    decimal crg = decimal.Parse(charge);
                    decimal percent = (pct.Length > 0) ? decimal.Parse(pct) : 0;

                    List<ServiceCharge> existing = _Lookits.ServiceCharges.GetList().FindAll(delegate(ServiceCharge match) { return (match.MaxValue == mx); });                    
                    if(existing.Count > 0)
                        throw new Exception("There is already a service charge on this tier(max value).");

                    ServiceCharge newItem = new ServiceCharge();
                    newItem.ApplicationId = _Config.APPLICATION_ID;
                    newItem.MaxValue = mx;
                    newItem.Charge = crg;
                    newItem.Percentage = percent;
                    newItem.Save();

                    _Lookits.RefreshLookup(_Enums.LookupTableNames.ServiceCharges.ToString());

                    form.ChangeMode(FormViewMode.Edit);

                    //this will work because the lookit is ordered by name
                    GridView1.SelectedIndex = _Lookits.ServiceCharges.GetList().FindIndex(delegate(ServiceCharge match) { return (match.Id == newItem.Id); });

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