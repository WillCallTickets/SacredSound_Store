using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using SubSonic;

using Newtonsoft.Json.Bson;
using Newtonsoft.Json;

namespace Wcss
{
    /// <summary>
    /// 
    /// </summary>
    public class TieredReward
    {
        //the minimum amount to spend - so greater than or equal
        public decimal MinAmount { get; set; }
        public decimal RewardAmount { get; set; }

        public TieredReward()
        {
        }

        public TieredReward(decimal _minAmount, decimal _rewardAmount)
        {
            MinAmount = _minAmount;
            RewardAmount = _rewardAmount;
        }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class SalePromotionMeta
    {
        public SalePromotionMeta() 
        {
            //if (TieredRewards == null)
            //    TieredRewards = new List<TieredReward>();
        }

        /// <summary>
        /// This value was reset after serialization.
        /// Use this to init collections to empty if not found 
        /// </summary>
        [OnSerialized]
        internal void OnSerializedMethod(StreamingContext context)
        {
            //if (TieredRewards == null)
            //    TieredRewards = new List<TieredReward>();
        }

        [OnSerializing]
        internal void OnSerializingMethod(StreamingContext context)
        {
            //if (TieredRewards == null)
            //    TieredRewards = new List<TieredReward>();
        }

        /// <summary>
        /// This value was set during deserialization.
        /// Sort the list before saving. This way, newly added items are auto sorted.
        /// </summary>
        [OnDeserializing]
        internal void OnDeserializingMethod(StreamingContext context)
        {
            //if (TieredRewards != null)
            //{
            //    if(TieredRewards.Count > 1)
            //        TieredRewards.Sort(delegate(TieredReward x, TieredReward y) { return (x.MinAmount.CompareTo(y.MinAmount)); });
            //}
        }

        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            
        }

        [JsonProperty]
        public List<TieredReward> TieredRewards { get; set; }
        public bool HasTieredRewards { get { return this.TieredRewards.Count > 0;  } }
     
        /// <summary>
        /// Items to add must not share the same minimum amount
        /// </summary>
        public string AddUpdateTieredReward(string _oldAmount, string _minAmount, string _rewardAmount, string commandName)
        {
            if (_minAmount == null || _minAmount.Trim().Length == 0 || _rewardAmount == null || _rewardAmount.Trim().Length == 0)
                return null;

            decimal minAmount = 0;
            decimal rewardAmount = 0;

            bool isMin = decimal.TryParse(_minAmount, out minAmount);
            bool isRew = decimal.TryParse(_rewardAmount, out rewardAmount);

            if(!isMin)
                throw new ArgumentOutOfRangeException("Min amount is not a decimal.");
            if(!isRew)
                throw new ArgumentOutOfRangeException("Reward amount is not a decimal.");

            if (minAmount <= 0)
                throw new ArgumentOutOfRangeException("Min amount must be greater than zero.");
            if (rewardAmount <= 0)
                throw new ArgumentOutOfRangeException("A reward amount greater than zero must be specified.");

            if(commandName.ToLower() == "insert" || commandName.ToLower() == "add")
                return AddTieredReward(minAmount, rewardAmount);
            else if(commandName.ToLower() == "update")
                return UpdateTieredReward(_oldAmount, minAmount, rewardAmount);

            return null;
        }
        private string AddTieredReward(decimal _minAmount, decimal _rewardAmount)
        {
            if (this.TieredRewards == null)
                this.TieredRewards = new List<TieredReward>();

            TieredReward t = this.TieredRewards.Find(delegate(TieredReward match) { return (match.MinAmount == _minAmount); });
            if (t != null)
                throw new ArgumentException("The listing already has a tier specified at that minimum amount.");

            TieredRewards.Add(new TieredReward(_minAmount, _rewardAmount));
            if(TieredRewards.Count > 1)
                TieredRewards.Sort(delegate(TieredReward x, TieredReward y) { return (x.MinAmount.CompareTo(y.MinAmount)); });
            return JsonConvert.SerializeObject(this, Formatting.None);
        }
        private string UpdateTieredReward(string _oldAmount, decimal _minAmount, decimal _rewardAmount)
        {
            TieredReward t = this.TieredRewards.Find(delegate(TieredReward match) { return (match.MinAmount.ToString() == _oldAmount); });
            if (t == null)
                throw new ArgumentException("The listing could not be found.");

            t.MinAmount = _minAmount;
            t.RewardAmount = _rewardAmount;
            if (TieredRewards.Count > 1)
                TieredRewards.Sort(delegate(TieredReward x, TieredReward y) { return (x.MinAmount.CompareTo(y.MinAmount)); });
            return JsonConvert.SerializeObject(this, Formatting.None);
        }

        public string DeleteTieredReward(string _minAmount)
        {
            if (_minAmount == null || _minAmount.Trim().Length == 0)
                return null;
            decimal minAmount = 0;

            bool isMin = decimal.TryParse(_minAmount, out minAmount);
            if (!isMin)
                throw new ArgumentOutOfRangeException("Min amount is not a decimal.");

            DeleteTieredReward(minAmount);
            return JsonConvert.SerializeObject(this, Formatting.None);
        }
        private void DeleteTieredReward(decimal _minAmount)
        {
            TieredRewards.Remove(
                this.TieredRewards.Find(delegate(TieredReward match) { return (match.MinAmount == _minAmount); })
                );
            if (TieredRewards.Count > 1)
                TieredRewards.Sort(delegate(TieredReward x, TieredReward y) { return (x.MinAmount.CompareTo(y.MinAmount)); });
        }

    }

    public partial class SalePromotion
    {
        private SalePromotionMeta _meta = null;
        public SalePromotionMeta Meta
        {
            get
            {
                if (_meta == null)
                {
                    if (this.JsonMeta == null || this.JsonMeta.Trim().Length == 0)
                        _meta = new SalePromotionMeta();
                    else 
                        _meta = JsonConvert.DeserializeObject<SalePromotionMeta>(this.JsonMeta);
                }

                return _meta;
            }
            set
            {
                _meta = value;
                this.JsonMeta = JsonConvert.SerializeObject(_meta, Formatting.None);
            }
        }



        public enum BannerContext
        {
            All,
            BannersOnly,
            NoBanners
        }

        #region Table Properties
        
        private List<int> _requiredMerchListing = null;
        public List<int> RequiredMerchListing
        {
            get
            {
                if (_requiredMerchListing == null) //init from db
                {
                    _requiredMerchListing = new List<int>();

                    if (this.VcTriggerListMerch != null && this.VcTriggerListMerch.Trim().Length > 0)
                    {
                        string[] pieces = this.VcTriggerListMerch.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        if (pieces.Length > 0)
                        {
                            foreach (string s in pieces)
                            {
                                int part = 0;
                                if (int.TryParse(s, out part))
                                    _requiredMerchListing.Add(part);
                            }

                            _requiredMerchListing.Sort();
                        }
                    }
                }

                return _requiredMerchListing;
            }
            //set
            //{
            //    this._requiredMerchListing = value;
            //}
        }
        public string ReCalculate_RequiredMerchListing_String()
        {
            string retVal = null;

            if (_requiredMerchListing.Count != 0)
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                _requiredMerchListing.Sort();

                foreach (int i in _requiredMerchListing)
                    sb.AppendFormat("{0},", i.ToString());

                sb.Length = sb.Length - 1;//remove trailing comma

                retVal = sb.ToString();
            }

            _requiredMerchListing = null;//force reset
            _requiredMerchItems = null;

            return retVal;
        }
        /// <summary>
        /// This not only allows multiple selections - but also prices the promotion per selection
        /// 3 items chosen = 3 * promotion price
        /// </summary>
        public bool AllowMultipleAwardSelections
        {
            get
            {
                if (!this.BAllowMultSelections.HasValue)
                    return false;

                return this.BAllowMultSelections.Value;
            }
            set
            {
                this.BAllowMultSelections = value;
            }
        }
        /// <summary>
        /// Indicates if the amount spent in promotions should count towards the minimum totals
        /// </summary>
        public bool AllowPromoTotalInMinimum
        {
            get
            {
                if (!this.BAllowPromoTotalInMinimum.HasValue)
                    return false;

                return this.BAllowPromoTotalInMinimum.Value;
            }
            set
            {
                this.BAllowPromoTotalInMinimum = value;
            }
        }
        private List<Merch> _requiredMerchItems = null;
        public List<Merch> RequiredMerchItems(_ContextBase ctx)
        {
            if (_requiredMerchItems == null)
            {
                _requiredMerchItems = new List<Merch>();

                if (this.Requires_MerchItem)
                {
                    foreach (int i in this.RequiredMerchListing)
                    {
                        Merch m = (Merch)ctx.SaleMerch.Find(i);
                        if (m == null || m.Id <= 0)
                            m = Merch.FetchByID(i);

                        //if it still not found - something is wrong - log 
                        if (m == null)
                        {
                            //Do not modify the list as in the ine below
                            //list.Remove(i);
                            ////TODO TEST - does this auto remove from underlying collection?

                            //this.RequiredMerchListing_Save();

                            _Error.LogException(new Exception(string.Format("SalePromotion ({0}) item id: ({1}) not found - list is reset",
                                this.DisplayNameWithAttribs, i.ToString())));
                        }
                        else
                            _requiredMerchItems.Add(m);
                    }
                }
            }

            return _requiredMerchItems;         
        }

        public static List<_Enums.DiscountContext> TransformDiscountContextFromString(string contexts)
        {
            List<_Enums.DiscountContext> list = new List<_Enums.DiscountContext>();

            if (contexts != null && contexts.Trim().Length > 0)
            {
                string[] ctxs = contexts.Split(',');
                foreach (string s in ctxs)
                    list.Add((_Enums.DiscountContext)Enum.Parse(typeof(_Enums.DiscountContext), s, true));
            }

            return list; 
        }

        public bool IsDiscountContext_Merch { get { return this.DiscountContext.Contains(_Enums.DiscountContext.merch); } }
        public bool IsDiscountContext_Ticket { get { return this.DiscountContext.Contains(_Enums.DiscountContext.ticket); } }
        public bool IsDiscountContext_MerchShipping { get { return this.DiscountContext.Contains(_Enums.DiscountContext.merchshipping); } }
        public bool IsDiscountContext_TicketShipping { get { return this.DiscountContext.Contains(_Enums.DiscountContext.ticketshipping); } }
        public bool IsDiscountContext_Processing { get { return this.DiscountContext.Contains(_Enums.DiscountContext.processing); } }
        public bool IsDiscountContext_ServiceFees { get { return this.DiscountContext.Contains(_Enums.DiscountContext.servicefees); } }
        public bool IsDiscountContext_TriggerItemOnly { get { return this.DiscountContext.Contains(_Enums.DiscountContext.triggeritemonly); } }
        
        [XmlAttribute("DiscountContext")]
        public List<_Enums.DiscountContext> DiscountContext
        {
            get { return TransformDiscountContextFromString(this.VcDiscountContext); }
            set 
            {
                if (value.Count == 0)
                    this.VcDiscountContext = null;
                else
                {
                    string list = string.Empty;
                    foreach (_Enums.DiscountContext app in value)
                        list += string.Format("{0},", app.ToString());

                    list.TrimEnd(',');

                    this.VcDiscountContext = list;
                }
            }
        }
        //bBannerMerch
        [XmlAttribute("DisplayBannerOnMerch")]
        public bool DisplayBannerOnMerch
        {
            get { return this.BBannerMerch; }
            set { this.BBannerMerch = value; }
        }
        //bBannerTicket
        [XmlAttribute("DisplayBannerOnTicketing")]
        public bool DisplayBannerOnTicketing
        {
            get { return this.BBannerTicket; }
            set { this.BBannerTicket = value; }
        }
        /// <summary>
        /// this will display the banner when at the item that needs to be purchased to activate the promotion
        /// </summary>
        [XmlAttribute("DisplayPromotionAtParentItem")]
        public bool DisplayPromotionAtParentItem
        {
            get { return this.BDisplayAtParent; }
            set { this.BDisplayAtParent = value; }
        }
        [XmlAttribute("DisplayBannerOnCart")]
        public bool DisplayBannerOnCart
        {
            get { return this.BBannerCartEdit; }
            set { this.BBannerCartEdit = value; }
        }
        [XmlAttribute("DisplayBannerOnCheckout")]
        public bool DisplayBannerOnCheckout
        {
            get { return this.BBannerCheckout; }
            set { this.BBannerCheckout = value; }
        }
        [XmlAttribute("DisplayBannerOnShipping")]
        public bool DisplayBannerOnShipping
        {
            get { return this.BBannerShipping; }
            set { this.BBannerShipping = value; }
        }

        [XmlAttribute("IsActive")]
        public bool IsActive
        {
            get { return this.BActive; }
            set { this.BActive = value; }
        }
        [XmlAttribute("IsDeactivateOnNoInventory")]
        public bool IsDeactivateOnNoInventory
        {
            get { return this.BDeactivateOnNoInventory; }
            set { this.BDeactivateOnNoInventory = value; }
        }
        [XmlAttribute("DisplayOrder")]
        public int DisplayOrder
        {
            get { return this.IDisplayOrder; }
            set { this.IDisplayOrder = value; }
        }
        /// <summary>
        /// If the promotion is a banner....then this will hold the timeout - the length of time to display the particular banner
        /// </summary>
        [XmlAttribute("BannerTimeout")]
        public int BannerTimeout
        {
            get { return this.IBannerTimeoutMsecs; }
            set { this.IBannerTimeoutMsecs = value; }
        }
        [XmlAttribute("DateStart")]
        public DateTime DateStart
        {
            get { return (!this.DtStartDate.HasValue) ? DateTime.MinValue : this.DtStartDate.Value; }
            set { this.DtStartDate = value; }
        }
        //dtEndDate
        [XmlAttribute("DateEnd")]
        public DateTime DateEnd
        {
            get { return (!this.DtEndDate.HasValue) ? DateTime.MaxValue : this.DtEndDate.Value; }
            set { this.DtEndDate = value; }
        }

        //mMinMerch
        [XmlAttribute("MinimumMerchandisePurchase")]
        public decimal MinimumMerchandisePurchase
        {
            get { return this.MMinMerch; }
            set { this.MMinMerch = value; }
        }
        //mMinTicket
        [XmlAttribute("MinimumTicketPurchase")]
        public decimal MinimumTicketPurchase
        {
            get { return this.MMinTicket; }
            set { this.MMinTicket = value; }
        }
        [XmlAttribute("RequiredParentQty")]
        public int RequiredParentQty
        {
            get { return this.IRequiredParentQty; }
            set { this.IRequiredParentQty = value; }
        }
        //mMinTotal
        [XmlAttribute("MinimumTotalPurchase")]
        public decimal MinimumTotalPurchase
        {
            get { return this.MMinTotal; }
            set { this.MMinTotal = value; }
        }

        [XmlAttribute("Price")]
        public decimal Price
        {
            get { return this.MPrice; }
            set { this.MPrice = value; }
        }
        [XmlAttribute("Weight")]
        public decimal Weight
        {
            get { return this.MWeight; }
            set { this.MWeight = value; }
        }
        [XmlAttribute("DiscountAmount")]
        public decimal DiscountAmount
        {
            get { return this.MDiscountAmount; }
            set { this.MDiscountAmount = value; }
        }
        [XmlAttribute("DiscountPercent")]
        public int DiscountPercent
        {
            get { return this.IDiscountPercent; }
            set { this.IDiscountPercent = value; }
        }

        [XmlAttribute("MaxPerOrder")]
        public int MaxPerOrder
        {
            get { return this.IMaxPerOrder; }
            set { this.IMaxPerOrder = value; }
        }
        [XmlAttribute("MaxValue")]
        public decimal MaxValue
        {
            get { return (this.MMaxValue.HasValue) ? this.MMaxValue.Value : decimal.MaxValue; }
            set { this.MMaxValue = value; }
        }
        /// <summary>
        /// 0 indicates unlimited uses
        /// </summary>
        [XmlAttribute("MaxUsesPerUser")]
        public int MaxUsesPerUser
        {
            get { return this.IMaxUsesPerUser; }
            set { this.IMaxUsesPerUser = value; }
        }

        #endregion

        #region Derived Properties

        public bool HasValidBannerEntry
        {
            get
            {
                return (this.BannerUrl != null && this.BannerUrl.Trim().Length > 0);
            }
        }

        /// <summary>
        /// returns a list with explanations as to why the object is not yet valid. An empty list is valid
        /// this is used in admin - not in order flow
        /// </summary>
        public List<string> ValidityCheckList
        {
            get
            {
                List<string> list = new List<string>();

                //verify awards
                if (this.IsMerchPromotion || this.IsTicketPromotion || this.IsDiscountPromotion)
                {
                    if (this.IsMerchPromotion)
                    {
                        SalePromotionAwardCollection collA = new SalePromotionAwardCollection();
                        collA.AddRange(this.SalePromotionAwardRecords());
                        foreach (SalePromotionAward spa in collA)
                        {
                            if (!spa.MerchRecord_Parent.IsActive)
                                list.Add(string.Format("{0} is not active.", spa.MerchRecord_Parent.Name));
                            if (spa.MerchRecord_Parent.Allotment <= 0)
                                list.Add(string.Format("{0} has no inventory.", spa.MerchRecord_Parent.Name));
                        }

                        if (this.AllowMultipleAwardSelections && this.Price == 0)
                            list.Add("You have specified that multiple items can be selected - but price is zero.");
                    }   
                }
                else
                    list.Add("No award has been specified.");

                //verify triggers
                if (this.Requires_PromotionCode || this.Requires_MerchItem || this.Requires_TicketItem || 
                    this.Requires_ShowDatePurchase || this.Requires_ShippingMethod || 
                    this.Requires_MinMerchPurchase || this.Requires_MinTicketPurchase || this.Requires_MinTotalPurchase)
                { }
                else
                    list.Add("No triggers have been specified.");

                if (this.IsGiftCertificatePromotion && (this.Meta.TieredRewards == null || this.Meta.TieredRewards.Count == 0))
                    list.Add("There are no tiers specified for this promotion.");

                return list;
                
            }
        }

        /// <summary>
        /// Indicates no unlock code is necessary
        /// </summary>
        [XmlAttribute("IsPublicOffer")]
        public bool IsPublicOffer
        {
            get { return this.UnlockCode == null || this.UnlockCode.Trim().Length == 0; }
        }
        /// <summary>
        /// Indicates that an unlock code is necessary to purchase the item
        /// </summary>
        [XmlAttribute("IsPrivateOffer")]
        public bool IsPrivateOffer
        {
            get { return (!this.IsPublicOffer); }
        }
        
        public bool IsUnlocked(string unlockCode)
        {
            if (this.IsPublicOffer)
                return true;

            if (unlockCode != null && unlockCode.Trim().Length > 0 && unlockCode == this.UnlockCode)
                return true;

            return false;
        }

        private bool _hasStarted
        {
            get
            {
                return this.DateStart < DateTime.Now;
            }
        }
        private bool _hasEnded
        {
            get
            {
                return this.DateEnd < DateTime.Now;
            }
        }
        public bool IsCurrentlyRunning(string unlockCode) 
        { 
          return this.IsActive && this.IsUnlocked(unlockCode) && this._hasStarted && (!this._hasEnded);           
        }

        //AWARDS
        /// <summary>
        /// Indicates if the promotion is a merch, ticket or discount promotion (shipping discounts are included here too)
        /// </summary>
        public bool IsAwardable { get { return (IsMerchPromotion || IsTicketPromotion || IsDiscountPromotion); } } 
        /// <summary>
        /// Indicates if the promotion has salePromotion Awards
        /// </summary>
        public bool IsMerchPromotion { get { return this.HasMerchAwards; } }        
        public bool HasMerchAwards { get { return this.SalePromotionAwardRecords().Count > 0; } }
        public bool IsTicketPromotion { get { return this.TShowTicketId.HasValue; } }        
        public bool IsFreeShippingPromotion { get { return this.IsShippingPromotion && this.DiscountPercent == 100; } }

        /// <summary>
        /// Indicates if ShipOfferMethod is null
        /// </summary>
        public bool IsShippingPromotion { get { return this.ShipOfferMethod != null && this.ShipOfferMethod.Trim().Length > 0; } }
        public bool IsShippingAmountPromotion { get { return this.IsShippingPromotion && IsDiscountAmountPromotion; } }
        public bool IsShippingPercentPromotion { get { return this.IsShippingPromotion && IsDiscountPercentPromotion; } }
        public bool IsGiftCertificatePromotion
        {
            get 
            {
                if (this.IsMerchPromotion)
                {
                    if (this.SalePromotionAwardRecords().Count > 0)
                    {
                        foreach (SalePromotionAward spa in this.SalePromotionAwardRecords())
                        {
                            if (!spa.MerchRecord_Parent.IsGiftCertificateDelivery)
                                return false;
                        }
                        return true;
                    }
                }

                return false;
            }
        }
        public bool IsDiscountPromotion { get { return IsDiscountAmountPromotion || IsDiscountPercentPromotion; } }
        public bool IsDiscountAmountPromotion { get { return this.DiscountAmount > 0; } }
        public bool IsDiscountPercentPromotion { get { return this.DiscountPercent > 0; } }
        public bool HasMaxUses { get { return this.MaxUsesPerUser > 0; } }
        
        //triggers
        private bool IsUnlockActive { get { return this.UnlockCode != null && this.UnlockCode.Trim().Length > 0; } }
        public bool Requires_PromotionCode { get { return this.RequiredPromotionCode != null && this.RequiredPromotionCode.Trim().Length > 0; } }
        public bool Requires_MerchItem { get { return this.RequiredMerchListing.Count > 0; } }
        public bool Requires_TicketItem { get { return this.TRequiredParentShowTicketId.HasValue; } }
        public bool Requires_ShowDatePurchase { get { return this.TRequiredParentShowDateId.HasValue; } }
        public bool Requires_ShippingMethod { get { return this.IsShippingPromotion; } }
        public bool RequiresMinimumPurchase { get { return this.Requires_MinMerchPurchase || this.Requires_MinTicketPurchase || this.Requires_MinTotalPurchase; } }
        public bool Requires_MinMerchPurchase { get { return this.MinimumMerchandisePurchase > 0; } }
        public bool Requires_MinTicketPurchase { get { return this.MinimumTicketPurchase > 0; } }
        public bool Requires_MinTotalPurchase { get { return this.MinimumTotalPurchase > 0; } }

        

        //public List<System.Web.UI.Pair> TieredAmountSpentRewardGranted
        //{
        //    get
        //    {
        //        if (this.Meta != null && this.Meta.Trim().Length > 0 && this.Meta.IndexOf("{\"TierAmountReward\"") != -1)
        //        {
        //            int start = this.Meta.IndexOf("{\"TierAmountReward\"");
        //             Json
        //        }
        //    }
        //}


        /// <summary>
        /// 
        /// </summary>
        public _Enums.InvoiceItemContext Context_Award
        {
            get
            {
                if (IsMerchPromotion)
                    return _Enums.InvoiceItemContext.merch;
                else if (IsTicketPromotion)
                    return _Enums.InvoiceItemContext.ticket;
                else if (IsDiscountPromotion)
                    return _Enums.InvoiceItemContext.discount;
                
                return _Enums.InvoiceItemContext.notassigned;
            }
        }

        /// <summary>
        /// shows the awards
        /// </summary>
        public string DisplayNameWithAttribs
        {
            get
            {
                string desc = desc = this.DisplayText;

                switch (Context_Award)
                {
                    case _Enums.InvoiceItemContext.ticket:
                        desc = this.ShowTicketRecord.DisplayNameWithAttribsAndDescription;
                        break;
                    case _Enums.InvoiceItemContext.merch:
                        //if there is only one item to choose from - display the name
                        if(this.SalePromotionAwardRecords().Count == 1)
                            desc = this.SalePromotionAwardRecords()[0].MerchRecord.DisplayNameWithAttribs;
                        break;
                    //these are handled in display text
                    case _Enums.InvoiceItemContext.shippingmerch:
                    case _Enums.InvoiceItemContext.shippingticket:
                    case _Enums.InvoiceItemContext.discount:
                        break;
                }

                return desc;
            }
        }

        /// <summary>
        /// returns an empty string if no coupon is required. If NOT NULL then TRUE. 
        /// returns the string that matches. If no match, it returns null. Ignores timeliness - just want a match on the coupon
        /// </summary>
        /// <param name="couponCodes"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        public string CouponMatch(List<string> couponCodes, bool ignoreCase)
        {
            if (!this.Requires_PromotionCode)
                return string.Empty;

            foreach (String s in couponCodes)
            {   
                if((ignoreCase && s.ToLower() == this.RequiredPromotionCode.ToLower()) ||
                    ((!ignoreCase) && s == this.RequiredPromotionCode))
                    return s;
            }

            return null;
        }

        public decimal CalculateShippingDiscountAmount_SaleItem_Promotion(decimal amountToApply)
        {
            decimal calc = 0;

            if (this.IsFreeShippingPromotion)//put here for clarity
                calc = amountToApply;
            //perform calculation
            else if (this.IsShippingAmountPromotion)
                calc = (this.DiscountAmount <= amountToApply) ? this.DiscountAmount : amountToApply;
            else if (this.IsShippingPercentPromotion)
                calc = (decimal)(this.DiscountPercent * (.01)) * amountToApply;

            if (this.MaxValue > 0 && calc > this.MaxValue)
                calc = this.MaxValue;

            return calc;
        }
        #endregion

        public static string Banner_VirtualDirectory
        {
            get
            {
                //ensure directories exist
                string path = string.Format("/{0}/Images/Banners/", _Config._VirtualResourceDir);
                string mappedPath = System.Web.HttpContext.Current.Server.MapPath(path);

                if (!System.IO.Directory.Exists(mappedPath))
                    System.IO.Directory.CreateDirectory(mappedPath);

                return path;
            }
        }
        public string Banner_VirtualFilePath
        {
            get
            {
                if (this.BannerUrl != null && this.BannerUrl.Trim().Length > 0)
                {
                    string filePath = string.Format("{0}{1}", Banner_VirtualDirectory, this.BannerUrl);
                    string mappedFile = System.Web.HttpContext.Current.Server.MapPath(filePath);

                    if (System.IO.File.Exists(mappedFile))
                        return filePath;
                }

                return string.Empty;
            }
        }

        public void SendLowInventoryNotification(int remaining)
        {
           
            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();

            mail.From = new System.Net.Mail.MailAddress(_Config._CustomerService_Email, _Config._CustomerService_FromName);
            mail.To.Add(new System.Net.Mail.MailAddress(_Config._Inventory_Notification_Email));

            mail.Subject = string.Format("Sale Promotion (Id: {0}) Update {1}", this.Id, DateTime.Now.AddDays(-1).Date.ToString("MM/dd/yyyy hh:mmtt"));
            mail.Body = string.Format("Promotion Id: {0} {1} '{2}' has {3} items remaining.", this.Id, this.Name, this.DisplayText, remaining);

            System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();

            try
            {
                client.Send(mail);
            }
            catch (Exception ex)
            {
                _Error.LogException(ex);
            }
        }

        #region DataSource methods

        public static SalePromotionCollection GetSalePromotions(SalePromotion.BannerContext bannerContext, int startRowIndex, int maximumRows)
        {
            SalePromotionCollection coll = new SalePromotionCollection();
            
            coll.LoadAndCloseReader(SPs.TxGetSalePromotions(_Config.APPLICATION_ID, bannerContext.ToString(), startRowIndex, maximumRows).GetReader());
            
            return coll;
        }

        public static int GetSalePromotionsCount(SalePromotion.BannerContext bannerContext)
        {
            int count = 0;
            using (System.Data.IDataReader dr = SPs.TxGetSalePromotionsCount(_Config.APPLICATION_ID, bannerContext.ToString()).GetReader())
            {
                while (dr.Read())
                    count = (int)dr.GetValue(0);
                dr.Close();
            }
            return count;
        }

        #endregion


    }
}
