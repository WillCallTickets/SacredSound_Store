using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.ComponentModel;
using WillCallWeb.StoreObjects;
using Wcss;
using Utils;

namespace WillCallWeb.Components.Cart
{
    //this class will just be the sale promotion and its fullfilled status
    public partial class PromotionListing
    {
        public PromotionListing(SalePromotion promo, string fullfillment) 
        {
            _salePromotion = promo;
            _fullfillment = fullfillment;
        }

        private SalePromotion _salePromotion = null;
        public SalePromotion SalePromo
        {
            get
            {
                return _salePromotion;
            }
        }
        private string _fullfillment = null;
        public string Fullfillment
        {
            get
            {
                return _fullfillment;
            }
        }
    }

    /// <summary>
    /// Displays a list of promotions available to the customer
    /// </summary>
    [ToolboxData("<{0}:Promotion_Listing ></{0}:Promotion_Listing>")]
    public partial class Promotion_Listing : WillCallWeb.BaseControl
    {
        protected void Page_Load(object sender, EventArgs e) { }

        #region Qty for download additions

        protected void BindDisplayQty(SaleItem_Promotion salePromo, RepeaterItemEventArgs e)
        {
            System.Web.UI.HtmlControls.HtmlGenericControl promoqty = (System.Web.UI.HtmlControls.HtmlGenericControl)e.Item.FindControl("promoQty");
            RadioButtonList ddlawards = (RadioButtonList)e.Item.FindControl("ddlAwards");

            if(salePromo != null && promoqty != null && ddlawards != null)
            {
                if (salePromo.HasProductSelections && salePromo.SalePromotion.Price > 0)
                {                    
                    //get the chosen award and see if it is a download
                    if (salePromo.SalePromotion.SalePromotionAwardRecords().Count == 1 && salePromo.SalePromotion.SalePromotionAwardRecords()[0].MerchRecord_Parent.IsDownloadDelivery)
                    {
                        if (salePromo.SalePromotion.RequiredMerchListing.Count > 0 || salePromo.SalePromotion.TRequiredParentShowDateId.HasValue || salePromo.SalePromotion.TRequiredParentShowTicketId.HasValue)
                        {
                            //get qty of promotion TRIGGER items
                            int parentQtyInCart = 0;
                            
                            if (salePromo.SalePromotion.RequiredMerchListing.Count > 0)
                            {
                                List<SaleItem_Merchandise> list = Ctx.Cart.FindSaleItem_MerchandiseByRequired(salePromo.SalePromotion);
                                if(list != null)
                                    foreach (SaleItem_Merchandise itm in list)
                                        parentQtyInCart += itm.Quantity;
                            }
                            else if (salePromo.SalePromotion.TRequiredParentShowDateId.HasValue)
                            {
                                List<SaleItem_Ticket> list = Ctx.Cart.FindSaleItem_TicketsByShowDateId(salePromo.SalePromotion.TRequiredParentShowDateId.Value);
                                if (list != null)
                                    foreach (SaleItem_Ticket itm in list)
                                        parentQtyInCart += itm.Quantity;
                            }
                            else if (salePromo.SalePromotion.TRequiredParentShowTicketId.HasValue)
                            {
                                SaleItem_Ticket list = Ctx.Cart.FindSaleItem_TicketById(salePromo.SalePromotion.TRequiredParentShowTicketId.Value);
                                if(list != null)
                                    parentQtyInCart += list.Quantity;
                            }

                            //set this to 1
                            //only show if we can choose a quantity
                            if (parentQtyInCart > 1)
                            {
                                promoqty.Visible = true;

                                DropDownList ddl = (DropDownList)e.Item.FindControl("ddlAwardQty");
                                if (ddl != null)
                                {
                                    if (!salePromo.SalePromotion.AllowMultipleAwardSelections)
                                    {
                                        Utils.ParseHelper.FillListWithNums(ddl, 0, parentQtyInCart);

                                        ddl.SelectedIndex = -1;
                                        if (salePromo.Quantity > 0)
                                            ddl.SelectedIndex = salePromo.Quantity;
                                    }
                                    else
                                        ddl.Visible = false;
                                }

                                Literal lit = (Literal)e.Item.FindControl("litAllowable");
                                if (lit != null)
                                {
                                    lit.Text = string.Format("<span class=\"promo-allowable\">You may select up to {0} downloads at a price of {1} each.</span>", 
                                        parentQtyInCart.ToString(), salePromo.SalePromotion.Price.ToString("c"));
                                }
                            }


                            //to display - the sale promotion must be a ticket promo
                            //the ticket promo must have a single award that is a download
                            //there must be a selection - tselectemerchid > 0 and != config.now merchselected
                        }
                    }
                }
            }
        }

        protected void ddlAwardQty_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;

            RadioButtonList ddlawards = (RadioButtonList)((RepeaterItem)ddl.NamingContainer).FindControl("ddlAwards");

            ListItem li = ddlawards.SelectedItem;
            string[] val = li.Value.Split('~');
            if (val.Length == 2)
            {
                string idx = val[0];
                string promo = val[1];

                //get the saleItem_promotion and assign the selection
                SaleItem_Promotion salePromo = Ctx.Cart.PromotionItems
                    .Find(delegate(SaleItem_Promotion match) { return (match.tSalePromotionId.ToString() == promo); });

                int selected = int.Parse(ddl.SelectedValue);
                if (selected == 0)
                {
                    salePromo.Quantity = 1;
                    salePromo.AddSelectedAward(_Config._NoSelectionIdValue);
                }
                else
                    salePromo.Quantity = selected;

                Ctx.Cart.OnCartChanged();
            }

            //get the sale item promotion
            //change the quantity of the promotion
            //rebind the cart
            //notify cart change
        }

        #endregion
        
        /// <summary>
        /// promotions to show
        /// case: merch required - only show if that merch is in cart
        /// case: ticket required - only show if that tkt is in cart
        /// 
        /// case: merch $$
        ///     - above thresh - show what they have been awarded
        ///     - below thresh - show how much more to go
        /// case: ticket $$
        ///     - above thresh - show what they have been awarded
        ///     - below thresh - show how much more to go
        ///     
        /// case shipping!!!!
        /// 
        /// case discount
        /// 
        /// A NULL value indicates no potential
        /// </summary>
        /// <param name="promo"></param>
        private string Promotion_HasPotential(SalePromotion promo)
        {
            return Promotion_HasPotential(promo, false);
        }
        private string Promotion_HasPotential(SalePromotion promo, bool compareTier)
        {
            string furtherRequirements = null;

            //merch and tkt promos are not combined!!!
            if (promo.Requires_MerchItem || promo.Requires_TicketItem || promo.Requires_ShowDatePurchase)
                furtherRequirements = PotentialForMerchOrTicketPromo(promo);
            else
            {
                decimal includePromoDollars = (promo.AllowPromoTotalInMinimum) ? Ctx.Cart.PreFeeSalePromotion_ItemTotal : 0;

                if (promo.IsShippingPromotion)
                    furtherRequirements = PotentialForShippingPromo(promo);

                if (promo.Requires_MinTotalPurchase)
                    furtherRequirements += FormatMinPurchaseResponse(Ctx.Cart.PreFeeTotal - Ctx.Cart.GiftCertificateTotal + includePromoDollars, promo.MinimumTotalPurchase, "store", furtherRequirements, promo, compareTier);

                if (promo.Requires_MinMerchPurchase)
                    furtherRequirements += FormatMinPurchaseResponse(Ctx.Cart.PreFeeMerchTotal - Ctx.Cart.GiftCertificateTotal + includePromoDollars, promo.MinimumMerchandisePurchase, "merchandise", furtherRequirements, promo, compareTier);

                if (promo.Requires_MinTicketPurchase)
                    furtherRequirements += FormatMinPurchaseResponse(Ctx.Cart.PreFeeTicketTotal + includePromoDollars, promo.MinimumTicketPurchase, "tickets", furtherRequirements, promo, compareTier);
            }

            return furtherRequirements;
        }

        private string FormatMinPurchaseResponse(decimal cartAmount, decimal targetAmount, string context, string existingFullfillment, SalePromotion promo, bool compareTier)
        {
            //if we have not yet satisfied the reqs
            if (compareTier)
            {
                if (promo.IsGiftCertificatePromotion && promo.Meta.TieredRewards.Count > 0)
                {
                    string retVal = string.Empty;

                    if (promo.Meta.TieredRewards.Count > 0)
                    {
                        List<TieredReward> lst = new List<TieredReward>();
                        lst.AddRange(promo.Meta.TieredRewards);
                        if (lst.Count > 1)
                            lst.Sort(delegate(TieredReward x, TieredReward y) { return (x.MinAmount.CompareTo(y.MinAmount)); });

                        foreach (TieredReward tier in lst)
                        {
                            if (cartAmount < tier.MinAmount)
                                retVal += string.Format("Spend {0} more in {1} to receive {2} store credit.{3}",
                                    (tier.MinAmount - cartAmount).ToString("c"), context, tier.RewardAmount.ToString("c"), Utils.Constants.Separator);
                        }
                    }

                    if (retVal.TrimStart().Length > 0)
                        return string.Format("{0}{1}", retVal,
                            (!promo.AllowPromoTotalInMinimum) ? string.Format("(shipping, promotional items and gift certificates do not apply){0}", Utils.Constants.Separator) :
                                string.Format("(shipping and gift certificates do not apply){0}", Utils.Constants.Separator)
                                );
                }
                return null;
            }
            else if (cartAmount < targetAmount)
            {
                if (promo.IsGiftCertificatePromotion && promo.Meta.TieredRewards.Count > 0)
                {
                    string retVal = string.Empty;

                    if (promo.Meta.TieredRewards.Count > 0)
                    {
                        List<TieredReward> lst = new List<TieredReward>();
                        lst.AddRange(promo.Meta.TieredRewards);
                        if (lst.Count > 1)
                            lst.Sort(delegate(TieredReward x, TieredReward y) { return (x.MinAmount.CompareTo(y.MinAmount)); });

                        foreach (TieredReward tier in lst)
                        {
                            retVal += string.Format("Spend {0} more in {1} to receive {2} store credit.{3}",
                                (tier.MinAmount - cartAmount).ToString("c"), context, tier.RewardAmount.ToString("c"), Utils.Constants.Separator);
                        }
                    }

                    return string.Format("{0}{1}", retVal,
                        (!promo.AllowPromoTotalInMinimum) ? string.Format("(shipping, promotional items and gift certificates do not apply){0}", Utils.Constants.Separator) :
                            string.Format("(shipping and gift certificates do not apply){0}", Utils.Constants.Separator)
                            );
                }

                return string.Format("Spend {0} more in {1} to receive this promotion{2}{3}",
                    (targetAmount - cartAmount).ToString("c"), context, Utils.Constants.Separator,
                    (!promo.AllowPromoTotalInMinimum) ? string.Format("(shipping, promotional items and gift certificates do not apply){0}", Utils.Constants.Separator) :
                    string.Format("(shipping and gift certificates do not apply){0}", Utils.Constants.Separator)
                    );
            }

            //otherwise we have satisfied and should not be null - allowing choices to be made
            return (existingFullfillment == null) ? null : string.Empty;
        }

        #region Shipping Promo Potential

        private string PotentialForShippingPromo(SalePromotion promo)
        {
            //merchshipping promotions need merch items - ticket shipping promotions need shippable tickets
            //this also covers cases where discount applies to all shipping
            bool hasPotential = (promo.IsDiscountContext_MerchShipping && Ctx.Cart.HasMerchandiseItems_Shippable) ||
                (promo.IsDiscountContext_TicketShipping && Ctx.Cart.HasTicketItems_CurrentlyShippable);

            if (!hasPotential)
                return null;

            //return a msg of we need to meet a certain type of shipping
            if (promo.ShipOfferMethod == "all")
                return string.Empty;

            //indicate required shipmethod - if not already chosen
            string requirements = string.Empty;
            string reqMethod = promo.ShipOfferMethod.ToLower();

            List<SaleItem_Shipping> ship = new List<SaleItem_Shipping>();

            //as only one ship method can be specified - the else statement will handle those promos with both merch and ship
            if (promo.IsDiscountContext_TicketShipping && Ctx.Cart.HasTicketItems_CurrentlyShippable)
            {
                ship.AddRange(Ctx.Cart.Shipments_Tickets.FindAll(delegate(SaleItem_Shipping match) { return (match.ShipMethod.ToLower() == reqMethod); }));

                if (ship.Count == 0)
                    requirements = FormatShipResponse(promo, "ticket");
            }
            if (promo.IsDiscountContext_MerchShipping && Ctx.Cart.HasMerchandiseItems_Shippable)
            {
                ship.AddRange(Ctx.Cart.Shipments_Merch.FindAll(delegate(SaleItem_Shipping match) { return (match.ShipMethod.ToLower() == reqMethod); }));

                if (ship.Count == 0)
                {
                    string response = FormatShipResponse(promo, "merch");
                    if (requirements == null || requirements.IndexOf(response) == -1)
                        requirements += response;
                }
            }

            return requirements;
        }
        private string FormatShipResponse(SalePromotion promo, string context)
        {
            return string.Format("Select {0} as your {1} shipping option to receive this promotion{2}", 
                promo.ShipOfferMethod, context, Utils.Constants.Separator);
        }

        #endregion

        #region Merch and Ticket Promo Potential
        private string PotentialForMerchOrTicketPromo(SalePromotion promo)
        {
            int reqIdx = 0;

            if (promo.Requires_MerchItem)
            {
                int hits = 0;

                //get the merch that we are to match                                
                //not only do we wish to match the merch product in question
                //but if we have a child item - that should also be a match
                foreach (SaleItem_Merchandise sim in Ctx.Cart.MerchandiseItems)
                {
                    if ((sim.MerchItem.TParentListing.HasValue && promo.RequiredMerchListing.Contains(sim.MerchItem.TParentListing.Value)) || 
                        promo.RequiredMerchListing.Contains(sim.tMerchId))
                        hits += sim.Quantity;
                }
                //if there are none - then return null
                if (hits > 0)
                    return IndicateFulfilledOrAddMoreToQualify(promo, hits);
            }
            else if (promo.Requires_TicketItem)
            {
                reqIdx = promo.TRequiredParentShowTicketId.Value;
                SaleItem_Ticket sit = Ctx.Cart.FindSaleItem_TicketById(reqIdx);
                //if there are none - then return null
                if (sit != null)
                    return IndicateFulfilledOrAddMoreToQualify(promo, sit.Quantity);
            }
            else if (promo.Requires_ShowDatePurchase)
            {
                reqIdx = promo.TRequiredParentShowDateId.Value;
                List<SaleItem_Ticket> sitColl = new List<SaleItem_Ticket>();
                sitColl = Ctx.Cart.FindSaleItem_TicketsByShowDateId(reqIdx);
                //sitColl.AddRange(Ctx.Cart.FindSaleItem_TicketsByShowDateId(reqIdx));
                //if there are none - then return null
                if (sitColl != null && sitColl.Count > 0)
                {
                    int qty = 0;
                    foreach (SaleItem_Ticket sit in sitColl)
                        qty += sit.Quantity;

                    return IndicateFulfilledOrAddMoreToQualify(promo, qty);
                }
            }

            return null;
        }
        private string IndicateFulfilledOrAddMoreToQualify(SalePromotion promo, int currentQty)
        {
            if (currentQty >= promo.RequiredParentQty)//fulfilled!
                return string.Empty;
            else 
            {
                return string.Format("Add {0} more {1} to your cart to receive this promotion",
                promo.RequiredParentQty - currentQty,

                (promo.IsMerchPromotion) ? 
                
                (promo.RequiredMerchListing.Count == 1) ? 
                Utils.ParseHelper.StripHtmlTags(promo.RequiredMerchItems(Ctx)[0].DisplayNameWithAttribs) :
                "(see promotion for details)" :
                
                Utils.ParseHelper.StripHtmlTags(promo.ShowTicketRecord_RequiredForPromotion.DisplayNameWithAttribsAndDescription));
            }
            //tags are not proper here - notification
        }
        #endregion

        public List<PromotionListing> Promotions_Potential
        {
            get
            {
                List<PromotionListing> list = new List<PromotionListing>();

                if (Ctx.Cart.HasItems)
                {
                    foreach (SalePromotion promo in Ctx.Cart.SalePromotions_RunningAndAvailable)
                    {
                        string full = Promotion_HasPotential(promo);
                        // dont include qualified(string.empty) discount promotions
                        if (full != null)
                            list.Add(new PromotionListing(promo, full));
                    }
                }

                return list;
            }
        }

        protected void BindAwardsList(ListControl awards, SaleItem_Promotion promo, MerchCollection selections)
        {
            if (selections.Count > 0)
            {
                awards.Visible = (promo.SalePromotion.AllowMultipleAwardSelections && awards is CheckBoxList) || 
                    ((!promo.SalePromotion.AllowMultipleAwardSelections) && awards is RadioButtonList);
                awards.Items.Clear();

                if (!awards.Visible)
                    return;

                awards.AppendDataBoundItems = true;
                List<ListItem> list = new List<ListItem>();

                //allow the user not to be forced into a choice
                //make this the first choice to line up with default selection
                if (promo.SalePromotion.Price > 0)
                    list.Add(new ListItem("I DO NOT WANT A PROMOTIONAL ITEM",
                        string.Format("{0}~{1}", _Config._NoSelectionIdValue.ToString(), promo.tSalePromotionId.ToString())));

                foreach (Merch m in selections)
                    list.Add(new ListItem(m.DisplayNameWithAttribs,
                        string.Format("{0}~{1}", m.Id.ToString(), promo.tSalePromotionId.ToString())));

                awards.DataSource = list;
                awards.DataTextField = "Text";
                awards.DataValueField = "Value";

                awards.DataBind();

                if (promo.HasProductSelections)
                {
                    awards.SelectedIndex = -1;

                    foreach (int idx in promo.SelectedAwardIds)
                    {
                        foreach (ListItem li in awards.Items)
                        {
                            if (li.Value == string.Format("{0}~{1}", idx.ToString(), promo.tSalePromotionId.ToString()))
                            {
                                li.Selected = true;

                                //radio can only have one choice
                                if (awards is RadioButtonList)
                                    break;
                            }
                        }
                    }
                }

                //if no selections - reset the promotion items selections
                if (awards.SelectedIndex == -1)
                {
                    awards.SelectedIndex = 0;

                    ListItem li = awards.SelectedItem;
                    string[] val = li.Value.Split('~');

                    if (val.Length == 2)
                    {
                        string idx = val[0];
                        
                        int idd = int.Parse(idx);
                        if (idd == 0)
                            idd = _Config._NoSelectionIdValue;
                        promo.AddSelectedAward(idd);
                    }
                }
            }
            else
                awards.Visible = false;
        }

        protected void rptPromo_DataBinding(object sender, EventArgs e)
        {
            Repeater rpt = (Repeater)sender;

            if (this.Promotions_Potential.Count > 0)
            {
                promoavailable.Visible = true;
                rptPromo.Visible = true;
                rpt.DataSource = this.Promotions_Potential;
            }
            else
                promoavailable.Visible = false;
        }
        
        protected void rptPromo_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            ListItemType lit = (ListItemType)e.Item.ItemType;

            if (lit != ListItemType.Footer && lit != ListItemType.Header && lit != ListItemType.Pager && lit != ListItemType.Separator)
            {
                PromotionListing listing = (PromotionListing)e.Item.DataItem;
                //if we have choices - show choices
                SaleItem_Promotion salePromo = Ctx.Cart.PromotionItems
                    .Find(delegate(SaleItem_Promotion match) { return (match.tSalePromotionId == listing.SalePromo.Id); });

                //if (salePromo != null)
                //{

                    Literal additional = (Literal)e.Item.FindControl("litAdditional");
                    Literal caveat = (Literal)e.Item.FindControl("litCaveat");
                    Literal selection = (Literal)e.Item.FindControl("litSelection");

                    //a promotion in the context of this control must have a string that is not null
                    //this will show what else we need to qualify for the promotion
                    //EXCEPT for postbacks where the promos/quals may have been removed
                    string qualification = listing.Fullfillment;

                    if (selection != null)
                    {
                        if (qualification.Trim().Length == 0)
                        {
                            MerchCollection selections = FindAvailableSelections(salePromo, true);

                            //correspond to cart choice
                            RadioButtonList awards = (RadioButtonList)e.Item.FindControl("ddlAwards");
                            CheckBoxList awardSelections = (CheckBoxList)e.Item.FindControl("chkAwardSelections");

                            if (awards != null && awardSelections != null)
                            {

                                BindAwardsList(awards, salePromo, selections);
                                BindAwardsList(awardSelections, salePromo, selections);

                                //if(awards.Visible)
                                BindDisplayQty(salePromo, e);
                            }

                            if (salePromo != null && salePromo.SalePromotion.IsGiftCertificatePromotion && salePromo.SalePromotion.Meta.TieredRewards.Count > 0)
                            {
                                string full = Promotion_HasPotential(salePromo.SalePromotion, true);
                                // dont include qualified(string.empty) discount promotions
                                if (full != null && full.Trim().Length > 0)
                                {
                                    litExtra.Text = string.Format("<div class=\"promoselectionsection\">{0}</div>", full.Replace("~", "<br/>"));
                                }
                            }
                        }
                        else
                            selection.Text = string.Format("<div class=\"promoselectionsection\">{0}</div>", qualification.Replace("~", "<br/>"));
                    }

                    string addt = listing.SalePromo.AdditionalText;
                    if (additional != null && addt != null && addt.Trim().Length > 0)
                        additional.Text = string.Format("<div class=\"promoadditionaltext\">{0}</div>", addt.Trim());

                    string promt = _Config._Promotion_Text;
                    if (caveat != null && promt != null && promt.Trim().Length > 0)
                        caveat.Text = string.Format("<div class=\"promocaveat\">{0}</div>", promt.Trim());

                    //total section
                    Panel pricing = (Panel)e.Item.FindControl("pnlPricing");
                    if (pricing != null)
                    {
                        //only show this section if the promo has a price
                        if (salePromo != null && salePromo.Price > 0)
                        {
                            pricing.Visible = true;
                            Literal litPrice = (Literal)e.Item.FindControl("litPrice");
                            if (litPrice != null)
                                litPrice.Text = salePromo.LineTotal.ToString("c");
                        }
                        else
                            pricing.Visible = false;

                    }
                //}
            }
        }

        private MerchCollection FindAvailableSelections(SaleItem_Promotion promo, bool doAction)
        {
            MerchCollection availableChildren = new MerchCollection();

            if (promo != null && promo.SalePromotion.HasMerchAwards)
            {
                MerchCollection availableParents = new MerchCollection();
            
                //fill a list with items that are in merch awards that have available inventory
                foreach (SalePromotionAward a in promo.SalePromotion.SalePromotionAwardRecords())
                {
                    Merch parent = Ctx.MerchParents.GetList()
                        .Find(delegate(Merch match) { return (a.TParentMerchId == match.Id && match.Available > 0); });
                    if (parent != null)
                        availableParents.Add(parent);
                }

                



                //find available inventory
                foreach (Merch parent in availableParents)
                {
                    MerchCollection sortedChildren = new MerchCollection();
                    sortedChildren.AddRange(parent.ChildMerchRecords_Active);

                    //if(sortedChildren.Count > 1)
                    //    sortedChildren.Sort((promo.SalePromotion.IsGiftCertificatePromotion) ? 
                    //        "MPrice" : "Style", true);

                    foreach (Merch child in sortedChildren)
                    {
                        Wcss.QueryRow.InventoryCheck check = new Wcss.QueryRow.InventoryCheck(child);
                        if (check.Available > 0)
                        {
                            if (promo.SalePromotion.IsGiftCertificatePromotion && promo.SalePromotion.Meta.TieredRewards.Count > 0)
                            {
                                decimal cartAmount = 0;
                                //match cart total to tiers and remove
                                decimal includePromoDollars = (promo.SalePromotion.AllowPromoTotalInMinimum) ? Ctx.Cart.PreFeeSalePromotion_ItemTotal : 0;

                                if (promo.SalePromotion.Requires_MinTotalPurchase)
                                    cartAmount = Ctx.Cart.PreFeeTotal - Ctx.Cart.GiftCertificateTotal + includePromoDollars;
                                if (promo.SalePromotion.Requires_MinMerchPurchase)
                                    cartAmount = Ctx.Cart.PreFeeMerchTotal - Ctx.Cart.GiftCertificateTotal;
                                if (promo.SalePromotion.Requires_MinTicketPurchase)
                                    cartAmount = Ctx.Cart.PreFeeTicketTotal + includePromoDollars;

                                //find matching tier
                                List<TieredReward> trs = promo.SalePromotion.Meta.TieredRewards
                                    .FindAll(delegate(TieredReward match) { return (cartAmount >= match.MinAmount && match.RewardAmount == child.Price); } );

                                //if we have results add it - clear out old collection
                                if (trs.Count > 0)
                                {
                                    availableChildren.Clear();
                                    promo.SelectedAwardIds.Clear();
                                    availableChildren.Add(child);
                                }
                            }
                            else 
                                availableChildren.Add(child);
                        }
                    }
                }

                //if we have no available selections, expire the promotion and go to the cart edit page
                if (doAction && availableChildren.Count == 0)
                {
                    //note the order of events here!!!                           
                    Ctx.CurrentCartException = string.Format("We're sorry, the {0} promotion is no longer available.", promo.SalePromotion.DisplayText);

                    SubSonic.QueryCommand cmd = new SubSonic.QueryCommand(string.Format("UPDATE [SalePromotion] SET [bActive] = 0 WHERE [Id] = {0} ",
                        promo.tSalePromotionId), SubSonic.DataService.Provider.Name);
                    SubSonic.DataService.ExecuteQuery(cmd);

                    promo.SelectedAwardIds.Clear();
                    promo.AddSelectedAward(_Config._NoSelectionIdValue);
                    Ctx.Cart.PromotionItems.Remove(promo);

                    //be sure to do this after extracting info!!!
                    _Lookits.RefreshLookup(_Enums.LookupTableNames.SalePromotions.ToString());

                    base.Redirect("/Store/Cart_Edit.aspx");
                }
            }

            return availableChildren;
        }

        protected void lstAwards_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListControl listControl = (ListControl)sender;

            //register all of the selected items
            List<ListItem> selections = new List<ListItem>();
                        
            ListItem firstItem = listControl.Items[0];
            string[] parts = firstItem.Value.Split('~');
            int promoId = int.Parse(parts[1]);

            SaleItem_Promotion salePromo = Ctx.Cart.PromotionItems
                .Find(delegate(SaleItem_Promotion match) { return (match.tSalePromotionId == promoId); });

            if (salePromo != null)
            {
                bool hasCurrentSelections = salePromo.HasProductSelections;

                //reset selections and build from the list
                salePromo.AddSelectedAward(0);

                foreach (ListItem li in listControl.Items)
                {
                    string[] val = li.Value.Split('~');
                    if (val.Length == 2)
                    {
                        string idx = val[0];

                        if (li.Selected)
                        {
                            if (idx == "0" || idx == _Config._NoSelectionIdValue.ToString())
                            {
                                salePromo.Quantity = 1;

                                if (hasCurrentSelections)
                                {
                                    salePromo.AddSelectedAward(_Config._NoSelectionIdValue);
                                    break;
                                }
                            }
                            else
                            {
                                salePromo.AddSelectedAward(int.Parse(idx));
                            }
                        }
                    }
                }

                //then rebind cart totals
                Ctx.Cart.OnCartChanged();
            }
        }
    }
}
