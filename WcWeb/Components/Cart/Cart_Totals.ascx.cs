using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;


using WillCallWeb.StoreObjects;
using Wcss;
using Utils;

namespace WillCallWeb.Components.Cart
{
    public partial class Cart_Totals : WillCallWeb.BaseControl
    {
        public void UpdateTotals()
        {
            UpdatePanelCartTotals.Update();
            rptPromotion.DataBind();
        }

        #region Page Overhead

        private void EventHandler_CartChanged(object sender, EventArgs e)
        {
            UpdatePanelCartTotals.Update();
            rptPromotion.DataBind();
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
            litError.Text = string.Empty;

            if (_Config._StoreCredit_Active && Ctx.Cart.StoreCreditCurrentlyAvailableForProfile > 0)
                txtCreditAmount.DataBind();
        }

        #endregion

        #region Update Panels

        protected void PanelTotals_Load(object sender, EventArgs e)
        {
            rptPromotion.DataBind();
        }

        #endregion

        #region Promotion Panel

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rptPromotion_DataBinding(object sender, EventArgs e)
        {
            //TESTING WITHOUT BEING DISTRACTED BY THIS CONTROL FOR THE TIME BEING RETURN
            //return;
            Ctx.Cart.FullfillPromotions();

            //show the availables
            Promotion_Listing1.DataBind();

            Repeater rpt = (Repeater)sender;
            List<SaleItem_Promotion> promos = new List<SaleItem_Promotion>();
      
            //display qualified discounts
            promos.AddRange(Ctx.Cart.PromotionItems.FindAll(delegate(SaleItem_Promotion match) { return (match.SalePromotion.IsDiscountPromotion); }));

            if (promos.Count > 0)
            {
                rpt.Visible = true;
                rpt.DataSource = promos;
            }
            else
                rpt.Visible = false;
        }
        protected void rptPromotion_ItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            Repeater rpt = (Repeater)sender;

            switch (e.CommandName.ToLower())
            {
                case "delete":
                    if (e.CommandArgument.ToString().Length > 0 && Utils.Validation.IsInteger(e.CommandArgument.ToString()))
                    {
                        int arg1 = int.Parse(e.CommandArgument.ToString());//the sale promotion id
                        SalePromotion promo = (SalePromotion)_Lookits.SalePromotions.Find(arg1);
                        if(promo != null && promo.Id > 0)
                        {
                            //remove the promotionCode
                            string baseCode = promo.RequiredPromotionCode.ToLower();
                            string couponCode = Ctx.SalePromotion_CouponCodes.Find(delegate(string match) { return match.ToLower() == baseCode; });

                            if (couponCode != null)
                                Ctx.SalePromotion_CouponCodes.Remove(couponCode);

                            //remove the SaleItem_Promotion
                            try
                            {
                                Ctx.Cart.PromotionItems.RemoveAt(Ctx.Cart.PromotionItems.FindIndex(delegate(SaleItem_Promotion match)
                                { return (match.tSalePromotionId == arg1); }));
                            }
                            catch (Exception ex)
                            {
                                _Error.LogException(ex);
                            }
                        }
                    }
                    break;
            }

            rpt.DataBind();            
        }
        protected void rptPromotion_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Repeater rpt = (Repeater)sender;

            if (e.Item.ItemType != ListItemType.Footer && e.Item.ItemType != ListItemType.Header &&
                e.Item.ItemType != ListItemType.Pager && e.Item.ItemType != ListItemType.Separator)
            {
                if (e.Item.DataItem != null)
                {
                    SaleItem_Promotion ent = (SaleItem_Promotion)e.Item.DataItem;
                    SalePromotion promotion = ent.SalePromotion;

                    Literal litDesc = (Literal)e.Item.FindControl("litDesc");
                    Literal litAmount = (Literal)e.Item.FindControl("litAmount");
                    Literal litEligible = (Literal)e.Item.FindControl("litEligible");
                    Literal litCode = (Literal)e.Item.FindControl("litCode");
                    LinkButton btnDelete = (LinkButton)e.Item.FindControl("btnDelete");
                    
                    if (btnDelete != null) btnDelete.Visible = false;
                    litDesc.Text = "&nbsp;";

                    //if we have met the requirements
                    //show the options
                    //otherwise show what we need to fulfill the offer
                    
                    //a null return value indicates no error msgs
                    string metReqs = Ctx.Cart.PromotionRequirementsMet(promotion);
                    if (metReqs != null)
                        metReqs = metReqs.Replace("~", "<br/>");

                    decimal discount = 0;
                    string eligibility = string.Empty;

                    //if we have a shipping promotion - the amount will be set when we have chosen a shipping method that matches the promo method
                    if (promotion.IsShippingPromotion)
                    {
                        if(metReqs == null)
                        {
                            discount = ent.Price;  

                            if (discount != 0)
                                litDesc.Text = "discount:";

                            eligibility = promotion.DisplayText;
                        }
                        else
                            eligibility = metReqs;

                    }
                    else if (promotion.IsDiscountPromotion)
                    {
                        eligibility = promotion.DisplayText;

                        litDesc.Text = "discount:";
                        discount = ent.Price;
                    }

                    litEligible.Text = string.Empty;
                    litAmount.Text = (discount == 0) ? "&nbsp;" : string.Format("({0})", discount.ToString());
                    litEligible.Text = eligibility;

                    //if we have used a code - display it
                    //if the promo required a code - get the code from the sale item
                    string code = (promotion.Requires_PromotionCode) ? string.Format ("- {0}", ent.GetDisplayableCouponCode()) : string.Empty;
                    litCode.Text = code;

                    btnDelete.Visible = (code != null && code.Trim().Length > 0);
                }
            }
        }
        #endregion

        #region Coupon click

        protected void btnCoupon_Click(object sender, EventArgs e)
        {
            if (this.Profile.IsAnonymous)
                throw new Exception("You must be logged in to add a coupon code.");

            //get the string out of text box and attempt to add to session coupons
            if (txtCoupon != null)
            {
                string coupon = txtCoupon.Text.Trim();
                if (coupon.Length > 0)
                {
                    //make sure we are not already using the coupon
                    //string matchingCode = Ctx.SalePromotion_CouponCodes.Contains(coupon.ToLower());

                    try
                    {
                        //must be logged in to add a coupon
                        string userName = this.Profile.UserName;

                        //get the matching promotion and retrieve the maxUses
                        string baseCoupon = UserCouponRedemption.GetCouponBase(coupon);

                       //see if we have a matching saleItem_Promotion
                        if (Ctx.Cart.PromotionItems.FindIndex(delegate(SaleItem_Promotion match)
                        {
                            return (match.SalePromotion.Requires_PromotionCode && 
                            match.SalePromotion.RequiredPromotionCode.ToLower() == baseCoupon.ToLower()); }) == -1)
                        {

                            //note here that we cannot use the promotions running list from the cart - the promos are not active until we enter the coupon
                            SalePromotion promo = _Lookits.SalePromotions.GetList().Find(delegate(SalePromotion match)
                            { return (match.IsCurrentlyRunning(Ctx.SalePromotionUnlock) && match.Requires_PromotionCode && match.RequiredPromotionCode.ToLower() == baseCoupon.ToLower()); });

                            if (promo != null && promo.Id > 0)
                            {
                                bool OK = UserCouponRedemption.IsAllowedRedemption(userName, coupon, promo.MaxUsesPerUser);
                                if (!OK)
                                    throw new Exception("This coupon code has been used the maximum amount of times.");

                                Ctx.SalePromotion_CouponCodes_Add(coupon, true);                                

                                Promotion_Listing1.DataBind();
                                
                            }
                            else
                            {
                                txtCoupon.Text = string.Empty;
                                throw new Exception("We're sorry, the coupon you have entered is no longer valid.");
                            }

                            rptPromotion.DataBind();
                        }

                        txtCoupon.Text = string.Empty;
                    }
                    catch (Exception ex)
                    {
                        litError.Text = Ctx.DisplayErrorText(ex.Message);
                        litError.Visible = true;
                        _Error.LogException(ex);
                    }
                }
            }
        }

        #endregion

        #region Store Credit

        protected void txtCreditAmount_DataBind(object sender, EventArgs e)
        {
            txtCreditAmount.Text = string.Format("{0}", Ctx.Cart.StoreCreditAvailableToApplyToInvoice);
        }
        protected void btnCreditApplyInitial_Click(object sender, EventArgs e)
        {
            //cannot apply to gift certificate purchases
            //if (Ctx.Cart.HasGiftCertificateItems)
            //{
            //    Ctx.Cart.StoreCredit.Price = 0;
            //    //valCustom.IsValid = false;
            //    litError.Text = "<div>We're sorry, but store credit cannot be applied to orders with gift certificates</div>";
            //    return;
            //}

            Ctx.Cart.StoreCredit.Price = decimal.Parse(this.Profile.StoreCredit.ToString());
            Ctx.Cart.OnCartChanged();
        }
        protected void btnCreditApplyPost_Click(object sender, EventArgs e)
        {
            //cannot apply to gift certificate purchases
            //if (Ctx.Cart.HasGiftCertificateItems)
            //{
            //    Ctx.Cart.StoreCredit.Price = 0;

            //    //valCustom.IsValid = false;
            //    litError.Text = "<div>We're sorry, but store credit cannot be applied to orders with gift certificates</div>";
            //    Ctx.Cart.OnCartChanged();
            //    return;
            //}

            string input = txtCreditAmount.Text;
            decimal d;

            try
            {   
                if (decimal.TryParse(input, out d))
                    Ctx.Cart.StoreCredit.Price = d;
            }
            catch (Exception ex)
            {
                valCustom.IsValid = false;
                valCustom.ErrorMessage = ex.Message;
                return;
            }

            Ctx.Cart.OnCartChanged();
            txtCreditAmount.DataBind();
        }
        protected void btnCreditRemove_Click(object sender, EventArgs e)
        {
            Ctx.Cart.StoreCredit.Price = 0;
            Ctx.Cart.OnCartChanged();
        }

        #endregion
    }
}
