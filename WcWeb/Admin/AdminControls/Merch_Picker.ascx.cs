using System;
using System.Web.UI.WebControls;

using Wcss;

namespace WillCallWeb.Admin.AdminControls
{   
    public partial class Merch_Picker : BaseControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ListItem li = rdoListContext.Items.FindByValue(Atx.AdminMerchListingContext.ToString());
                if (li != null)
                    li.Selected = true;
            }
        }
        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            Atx.SetCurrentMerchRecord(0);

            base.Redirect("/Admin/MerchEditor.aspx?p=ItemEdit");
        }
        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            Atx.Clear_CurrentMerchListing();
            _Lookits.RefreshLookup(_Enums.LookupTableNames.MerchDivisions.ToString());
        }
        protected void rdoListContext_SelectedIndexChanged(object sender, EventArgs e)
        {
            RadioButtonList rbl = (RadioButtonList)sender;
            int selectedValue = int.Parse(rbl.SelectedValue);
            Atx.AdminMerchListingContext = selectedValue;

            Menu_MerchListing1.DataBind();
        }
}
}