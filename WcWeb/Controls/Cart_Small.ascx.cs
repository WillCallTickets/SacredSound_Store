using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WillCallWeb.Controls
{
    public partial class Cart_Small : WillCallWeb.BaseControl
    {   
        private void EventHandler_CartChanged(object sender, EventArgs e)
        {
            BindCart(UpdatePanel1);

            UpdatePanel1.Update();
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            Ctx.Cart.CartChanged += new WillCallWeb.StoreObjects.ShoppingCart.CartChangedEvent(EventHandler_CartChanged);
        }

        public override void Dispose()
        {
            Ctx.Cart.CartChanged -= new WillCallWeb.StoreObjects.ShoppingCart.CartChangedEvent(EventHandler_CartChanged);

            base.Dispose();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void PanelLoad(object sender, EventArgs e)
        {
            if (!IsPostBack)
                BindCart(sender);
        }

        private void BindCart(object sender)
        {
            UpdatePanel panel = (UpdatePanel)sender;
            HyperLink edit = (HyperLink)panel.FindControl("lnkEdit");

            if (Ctx.Cart.ItemCount == 0 || this.Page.ToString().ToLower() == "asp.store_cart_edit_aspx")
                edit.NavigateUrl = string.Empty;
            else
                edit.NavigateUrl = "/Store/Cart_Edit.aspx"; 
        }
}
}