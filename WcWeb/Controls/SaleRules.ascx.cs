using System;

using Wcss;

namespace WillCallWeb.Controls
{
    public partial class SaleRules : WillCallWeb.BaseControl
    {   
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                BindItems();
        }
        public void BindItems()
        {
            SaleRuleCollection coll = new SaleRuleCollection();

            coll.AddRange(_Lookits.SaleRules.GetList().FindAll(
               delegate(SaleRule match) { return (match.IsActive && match.Context == _Enums.ProductContext.all); }));

            bool hasTix = ((Ctx.Cart != null && Ctx.Cart.HasTicketItems) || (Ctx.SessionAuthorizeNet != null && Ctx.SessionAuthorizeNet.InvoiceRecord.HasTicketItems));
            bool hasMerch = ((Ctx.Cart != null && Ctx.Cart.HasMerchandiseItems) || (Ctx.SessionAuthorizeNet != null && Ctx.SessionAuthorizeNet.InvoiceRecord.HasMerchItems));

            if(hasTix && hasMerch)
                coll.AddRange(_Lookits.SaleRules.GetList().FindAll(
                    delegate(SaleRule match) { return (match.IsActive && (match.Context == _Enums.ProductContext.merch || match.Context == _Enums.ProductContext.ticket)); }));
            else if(hasTix)
                coll.AddRange(_Lookits.SaleRules.GetList().FindAll(
                    delegate(SaleRule match) { return (match.IsActive && match.Context == _Enums.ProductContext.ticket); }));
            else if(hasMerch)
                coll.AddRange(_Lookits.SaleRules.GetList().FindAll(
                    delegate(SaleRule match) { return (match.IsActive && match.Context == _Enums.ProductContext.merch); }));

            if (coll.Count > 0)
            {
                if (coll.Count > 1)
                    coll.Sort("IDisplayOrder", true);
                rptFeatured.DataSource = coll;
                rptFeatured.DataBind();
            }
        }
    }
}