using System;
using System.Collections.Generic;

using Wcss;

namespace WillCallWeb.StoreObjects
{
	/// <summary>
	/// Summary description for SaleItem_Promotion
    /// these items do not expire on their own - they cannot cause the cart to be redrawn
    /// items added to cart update either ticket or merch stock tables
    /// 
    /// See notes on price property to understand the handling of shipping promotions
	/// </summary>
	[Serializable]
	public class SaleItem_Promotion : SaleItem_Base
    {
        #region Constructors

        public SaleItem_Promotion(WebContext ctx, int salePromotionId, int qty)
		{
            this.tSalePromotionId = salePromotionId;
			this.Quantity = qty;
			this.BornOnDate = DateTime.Now;
            this.TTL = BornOnDate.AddSeconds(_Config._TTL_Secs_CartItems);
			this.Ctx = ctx;
		}

		#endregion
                
        public bool AllRequiredMerchItemsAreGifts
        {
            get
            {
                foreach (Merch m in this.SalePromotion.RequiredMerchItems(this.Ctx))
                    if (!m.IsGiftCertificateDelivery)
                        return false;

                return true;
            }
        }

		#region Properties And Methods

        private string _pickupName = null;        
        private bool _customerOptIn = false;

		public int tSalePromotionId
		{
			get
			{
                if (base._linkedItemIdx == 0)
					throw new System.ArgumentNullException("SalePromotionId must be defined.");
                return base._linkedItemIdx;
			}
            set { base._linkedItemIdx = value; }
		}
        
        #region MerchAwards Selected

        public string SelectedAwardIdsString
        {
            get
            {
                string s = string.Empty;

                foreach (int idx in this.SelectedAwardIds)
                    s += string.Format("{0},", idx.ToString());

                return s.TrimEnd(',');
            }
        }
        public bool HasProductSelections
        {
            get
            {
                return (this.SelectedAwardIds.Count  > 0 && (!this.SelectedAwardIds.Contains(0)) && (!this.SelectedAwardIds.Contains(_Config._NoSelectionIdValue)));
            }
        }
        private new int _qty;
        public new int Quantity
        {
            get
            {
                if (this.SalePromotion.AllowMultipleAwardSelections && this.HasProductSelections)
                    return this.SelectedAwardIds.Count;

                return _qty;
            }
            set
            {
                _qty = value;
            }
        }
        public void AddSelectedAward(int idx)
        {
            //if adding a non-selection
            if (idx == 0 || idx == _Config._NoSelectionIdValue)
            {
                _selectedAwardIds = null;//automatically adds the default
            }
            else
            {
                SelectedAwardIds.Remove(0);
                SelectedAwardIds.Remove(_Config._NoSelectionIdValue);

                if (!this.SelectedAwardIds.Contains(idx))
                {
                    this.SelectedAwardIds.Add(idx);
                }
            }
        }
        private List<int> _selectedAwardIds = null;
        public List<int> SelectedAwardIds
        {
            get
            {
                if (_selectedAwardIds == null)
                {
                    _selectedAwardIds = new List<int>();
                    _selectedAwardIds.Add(0);//add a non choice
                }

                return _selectedAwardIds;
            }
        }
        private MerchCollection _selectedAwardsMerchCollection = new MerchCollection();
        public MerchCollection SelectedAwardsMerchCollection
        {
            get
            {
                //remove any items that are not contained in the awardIds list
                _selectedAwardsMerchCollection.GetList().RemoveAll(delegate(Merch match) { return (!this.SelectedAwardIds.Contains(match.Id)); });

                //add any missing items
                foreach (int idx in SelectedAwardIds)
                {
                    if (idx != 0 && idx != _Config._NoSelectionIdValue && 
                        _selectedAwardsMerchCollection.GetList().FindIndex(delegate(Merch match) { return (match.Id == idx); }) == -1)
                        _selectedAwardsMerchCollection.Add(new Merch(idx));
                }

                return _selectedAwardsMerchCollection;
            }
        }

        #endregion
        
        /// <summary>
        /// marked as false if the cust decides not to take the promotion item during checkout
        /// </summary>
        public bool CustomerOptIn
        {
            get
            {
                return _customerOptIn;
            }
            set
            {
                _customerOptIn = value;
            }
        }

        public SalePromotion SalePromotion
        {
            get
            {
                SalePromotion sp = (Wcss.SalePromotion)_Lookits.SalePromotions.Find(tSalePromotionId);
                if (sp == null)
                {
                    sp = new SalePromotion();
                    SubSonic.QueryCommand cmdp = new SubSonic.QueryCommand("SELECT * FROM SalePromotion WHERE Id = @ideo ",
                        SubSonic.DataService.Provider.Name);
                    cmdp.Parameters.Add("@ideo", this.tSalePromotionId, System.Data.DbType.Int32);
                    sp.LoadAndCloseReader(SubSonic.DataService.GetReader(cmdp));
                }

                return sp;
            }
        }
        public string PickupName
        {
            get
            {
                if (_pickupName == null)
                    return string.Empty;

                return _pickupName;
            }
            set
            {
                if (value != null && value.Trim().Length > 256)
                    throw new Exception("Pickup name must be less than 256 characters.");
                if (value != null)
                    value = value.Trim();
                _pickupName = value;
            }
        }
        public string GetDisplayableCouponCode()
        {
            //get the coupon code that matches
            string code = string.Empty;
            foreach (string s in Ctx.SalePromotion_CouponCodes)
            {
                //if its a two parter
                string part1 = s;
                if (s.IndexOf('-') != -1)
                {
                    string[] parts = part1.Split('-');
                    if (parts.Length == 2)
                    {
                        part1 = parts[0];
                    }
                }

                //if the code matches...
                if (part1.ToLower() == this.SalePromotion.RequiredPromotionCode.ToLower())
                    code = s;
            }

            return code;
        }

        /// <summary>
        /// Price is automatically updated to reflect the current context. Note that the cart allows ship promotions regardless of choice, but does not 
        /// assign a price (for shipping discount) until it matches the chosen shipping method. This allows the ship promo to be active in the promo panel. ie: visible to shopper.
        /// </summary>
        public decimal Price
        {
            get
            {
                decimal amountToApply = 0;

                if (SalePromotion != null)
                {
                    if (SalePromotion.Price > 0 && this.HasProductSelections)
                        amountToApply = SalePromotion.Price;
                    else if (SalePromotion.IsDiscountPromotion)
                    {
                        //determine if the promotion is unlocked - returns an empty string if no coupon is required
                        string coupon = SalePromotion.CouponMatch(Ctx.SalePromotion_CouponCodes, _Config._Coupon_IgnoreCase);

                        if (coupon != null)
                        {
                            if (SalePromotion.IsDiscountAmountPromotion)
                                amountToApply = SalePromotion.DiscountAmount;
                            else if (SalePromotion.IsDiscountPercentPromotion)
                            {
                                //cycle through the discount applications and apply amounts
                                foreach (_Enums.DiscountContext app in SalePromotion.DiscountContext)
                                {
                                    decimal holdAmount = 0;

                                    switch (app)
                                    {
                                        case _Enums.DiscountContext.triggeritemonly:
                                            //if they have the item in their cart
                                            int idxToMatch = 0;
                                            if (this.SalePromotion.Requires_MerchItem)
                                            {
                                                List<SaleItem_Merchandise> merchs = new List<SaleItem_Merchandise>();
                                                merchs.AddRange(Ctx.Cart.MerchandiseItems.FindAll(delegate(SaleItem_Merchandise match)
                                                 { return (match.MerchItem.TParentListing.HasValue && //must be a child
                                                     (this.SalePromotion.RequiredMerchListing.Contains(match.MerchItem.TParentListing.Value) ||
                                                     this.SalePromotion.RequiredMerchListing.Contains(match.MerchItem.Id))); }));

                                                foreach (SaleItem_Merchandise sim in merchs)
                                                    holdAmount += sim.LineTotal;
                                            }
                                            else if (this.SalePromotion.Requires_TicketItem)
                                            {
                                                idxToMatch = this.SalePromotion.TRequiredParentShowTicketId.Value;
                                                List<SaleItem_Ticket> tickets = new List<SaleItem_Ticket>();
                                                tickets.AddRange(Ctx.Cart.TicketItems.FindAll(delegate(SaleItem_Ticket match)
                                                { return (match.tShowTicketId == idxToMatch); }));

                                                foreach (SaleItem_Ticket sit in tickets)
                                                    holdAmount += sit.LineTotal;
                                            }
                                            else if (this.SalePromotion.Requires_ShowDatePurchase)
                                            {
                                                idxToMatch = this.SalePromotion.TRequiredParentShowDateId.Value;
                                                List<SaleItem_Ticket> datetickets = new List<SaleItem_Ticket>();
                                                datetickets.AddRange(Ctx.Cart.TicketItems.FindAll(delegate(SaleItem_Ticket match)
                                                { return (match.Ticket.TShowDateId == idxToMatch); }));

                                                foreach (SaleItem_Ticket dsit in datetickets)
                                                    holdAmount += dsit.LineTotal;
                                            }

                                            break;
                                        case _Enums.DiscountContext.merch:
                                            holdAmount += Ctx.Cart.PreFeeMerchTotal - Ctx.Cart.GiftCertificateTotal;
                                            break;
                                        case _Enums.DiscountContext.ticket:
                                            holdAmount += Ctx.Cart.PreFeeTicketTotal;
                                            break;
                                        case _Enums.DiscountContext.merchshipping:
                                        case _Enums.DiscountContext.ticketshipping:
                                            //we need to match the shipping type
                                            string shipMethodToMatch = (this.SalePromotion.ShipOfferMethod == null || this.SalePromotion.ShipOfferMethod.Trim().Length == 0) ? 
                                                "all" : this.SalePromotion.ShipOfferMethod.ToLower();

                                            if (shipMethodToMatch == "all" && Ctx.Cart.ShippingAndHandling > 0)
                                            {
                                                if(app == _Enums.DiscountContext.merchshipping)
                                                    holdAmount += Ctx.Cart.Shipping_Merch;
                                                else if(app == _Enums.DiscountContext.ticketshipping)
                                                    holdAmount += Ctx.Cart.Shipping_Tickets;
                                            }
                                            else//otherwise we need to cycle thru the shipments
                                            {
                                                foreach (SaleItem_Shipping shp in Ctx.Cart.Shipments_All)
                                                {
                                                    if (shipMethodToMatch == shp.ShipMethod.Trim().ToLower())
                                                    {
                                                        //TODO: change this back to item_list
                                                        if(app == _Enums.DiscountContext.merchshipping && shp.Items_Merch_All.Count > 0)
                                                            holdAmount += shp.ShipCost;
                                                        else if (app == _Enums.DiscountContext.ticketshipping && shp.Items_Tickets.Count > 0)
                                                            holdAmount += shp.ShipCost;
                                                    }
                                                }
                                            }
                                            break;
                                        case _Enums.DiscountContext.processing:
                                            holdAmount += Ctx.Cart.ProcessingCharge;
                                            break;
                                        case _Enums.DiscountContext.servicefees:
                                            //for each matching base ticket item - add up a discount
                                            foreach (SaleItem_Ticket sit in Ctx.Cart.TicketItems)
                                            {
                                                if ((!sit.Ticket.IsPackage) || sit.Ticket.IsBaseOfPackage)
                                                {
                                                    //if we are targeting a specific ticket
                                                    if (this.SalePromotion.Requires_ShowDatePurchase && sit.Ticket.TShowDateId == this.SalePromotion.TRequiredParentShowDateId)
                                                        holdAmount += sit.Ticket.ServiceCharge;
                                                    else if (this.SalePromotion.Requires_TicketItem && sit.tShowTicketId == this.SalePromotion.TRequiredParentShowTicketId)
                                                        holdAmount += sit.Ticket.ServiceCharge;
                                                    else if ((!this.SalePromotion.Requires_ShowDatePurchase) && (!this.SalePromotion.Requires_TicketItem))
                                                        holdAmount += sit.Ticket.ServiceCharge;
                                                }
                                            }

                                            break;
                                    }

                                    amountToApply += Utils.Helper.ConvertIntToPercent(SalePromotion.DiscountPercent) * holdAmount;
                                }
                            }
                        }                        
                    }

                    //check max value allowed
                    if (SalePromotion.MaxValue > 0 && amountToApply > SalePromotion.MaxValue)
                        amountToApply = SalePromotion.MaxValue;

                    //if we are doing a discount - then make it negative
                    if (SalePromotion.Price <= 0)
                        amountToApply *= (-1);
                }

                return decimal.Round(amountToApply, 2);
            }
        }		

        public decimal LineTotal { get { return this.Price * this.Quantity; } }

		#endregion	
    }
}
