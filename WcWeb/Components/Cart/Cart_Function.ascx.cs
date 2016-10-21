using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Collections.Generic;

using WillCallWeb.StoreObjects;

namespace WillCallWeb.Components.Cart
{
    public partial class Cart_Function : WillCallWeb.BaseControl
    {
        protected bool _useDotSeparator = false;
        [Category("Display"),
        Description("If true, will display a dot separator between the buttons/links"),
        PersistenceMode(PersistenceMode.Attribute),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)
        ]
        public bool UseDotSeparator
        {
            get
            {
                return _useDotSeparator;
            }
            set
            {
                _useDotSeparator = value;
            }
        }

        private void EventHandler_CartChanged(object sender, EventArgs e)
        {
            BindCart(UpdatePanel1);
            //UpdatePanel1.Update();
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
            BindCart(sender);
        }
        private void BindCart(object sender)
        {
            string pageName = this.Page.ToString().ToLower();
            UpdatePanel panel = (UpdatePanel)sender;

            LinkButton linkClear = (LinkButton)panel.FindControl("linkClear");
            HyperLink linkEdit = (HyperLink)panel.FindControl("linkEdit");
            HyperLink linkCheckout = (HyperLink)panel.FindControl("linkCheckout");
            System.Web.UI.HtmlControls.HtmlGenericControl dot =
                (System.Web.UI.HtmlControls.HtmlGenericControl)panel.FindControl("MidDot");

            linkClear.Visible = (Ctx.Cart.ItemCount > 0 && pageName == "asp.store_cart_edit_aspx");
            linkEdit.Visible = (Ctx.Cart.ItemCount > 0 && pageName != "asp.store_cart_edit_aspx");
            linkCheckout.Visible = (! Ctx.Cart.IsOverMaxTransactionAllowed) && (Ctx.Cart.ItemCount > 0 && pageName != "asp.store_checkout_aspx" && 
                this.Page.ToString().ToLower() != "asp.store_shipping_aspx");

            //links keeps track of displaying the dot separator
            int links = (linkClear.Visible) ? 1 : 0;
            if(linkEdit.Visible) 
                links++;
            if (linkCheckout.Visible)
                links++;

            if (dot != null)
                dot.Visible = (links > 1);
            //end links

            if (linkCheckout.Visible)
            {
                List<SaleItem_Base> sibs = new List<SaleItem_Base>();

                foreach (SaleItem_Ticket sit in Ctx.Cart.TicketItems)
                    sibs.Add((SaleItem_Base)sit);
                foreach (SaleItem_Merchandise mit in Ctx.Cart.MerchandiseItems)
                    sibs.Add((SaleItem_Base)mit);

                bool allChosen = true;
                foreach (SaleItem_Base sib in sibs)
                {
                    if (!sib.HasSelectedAllAvailableBundleItems(false))
                    {
                        allChosen = false;
                        break;
                    }
                }

                pnlMessage.Controls.Clear();
                pnlMessage.Visible = false;
                linkCheckout.Enabled = true;

                if ((!allChosen) && (!UseDotSeparator))
                {
                    pnlMessage.Visible = true;
                    Literal lit = new Literal();
                    lit.Text = string.Format("<div class=\"pendingbundle\">You have pending bundle selections!<div>Please select your bundled items prior to checkout.</div></div>");
                    pnlMessage.Controls.Add(lit);

                    linkCheckout.Enabled = false;
                }
            }

            //only show this on the cart total control and not on the purchase page
            if (Wcss._Config._StoreCredit_Active && this.Page.ToString().IndexOf("Confirmation") == -1 && this.NamingContainer.ToString() == "ASP.components_cart_cart_totals_ascx")
            {
                Literal litRedeem = (Literal)panel.FindControl("litRedeem");

                if (litRedeem != null && Wcss._Config._StoreCredit_Active && litRedeem.Text.Trim().Length == 0)
                    litRedeem.Text = string.Format("<span class=\"redeemer\"><a href=\"http://{0}/WebUser/Default.aspx?p=credit\">Redeem a gift certificate or credit?</a></span>",
                        Wcss._Config._DomainName);
            }
        }

        protected void linkClear_Click(object sender, EventArgs e)
        {
            Ctx.Cart.ClearCart();

            if (this.Page.ToString() == "ASP.store_cart_edit_aspx")
                base.Redirect("/Store/Cart_Edit.aspx");
        }
}
}