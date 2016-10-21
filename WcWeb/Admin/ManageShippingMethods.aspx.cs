using System;
using System.Web.UI.WebControls;

namespace WillCallWeb.Admin
{
    public partial class ManageShippingMethods : WillCallWeb.BasePage
   {
        protected override void OnPreInit(EventArgs e)
        {
            QualifySsl(true);
            base.OnPreInit(e);
        }
      protected void Page_Load(object sender, EventArgs e) {}

      protected void gvwShippingMethods_SelectedIndexChanged(object sender, EventArgs e)
      {
         dvwShippingMethod.ChangeMode(DetailsViewMode.Edit);
      }

      protected void gvwShippingMethods_RowDeleted(object sender, GridViewDeletedEventArgs e)
      {
         gvwShippingMethods.SelectedIndex = -1;
         gvwShippingMethods.DataBind();
         dvwShippingMethod.ChangeMode(DetailsViewMode.Insert);
      }

      protected void gvwShippingMethods_RowCreated(object sender, GridViewRowEventArgs e)
      {
         if (e.Row.RowType == DataControlRowType.DataRow)
         {
            LinkButton btn = e.Row.Cells[3].Controls[0] as LinkButton;
            btn.OnClientClick = "if (confirm('Are you sure you want to delete this shipping method?') == false) return false;";
         }
      }

      protected void dvwShippingMethod_ItemInserted(object sender, DetailsViewInsertedEventArgs e)
      {
         gvwShippingMethods.SelectedIndex = -1;
         gvwShippingMethods.DataBind();
      }

      protected void dvwShippingMethod_ItemUpdated(object sender, DetailsViewUpdatedEventArgs e)
      {
         gvwShippingMethods.SelectedIndex = -1;
         gvwShippingMethods.DataBind();
      }

      protected void dvwShippingMethod_ItemCommand(object sender, DetailsViewCommandEventArgs e)
      {
         if (e.CommandName == "Cancel")
         {
            gvwShippingMethods.SelectedIndex = -1;
            gvwShippingMethods.DataBind();
         }
      }
   }
}