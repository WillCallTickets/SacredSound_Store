using System;
using System.Xml.Serialization;

namespace Wcss
{
    public partial class InvoiceItemCollection
    {
        public void SortTicketItemsBy_DateToOrderBy()
        {
            if (this != null && this.Count > 1)
            {
                //make sure they are all tickets
                bool allAreTickets = true;
                foreach(InvoiceItem ii in this)
                    if (!ii.IsTicketItem)
                    {
                        allAreTickets = false;
                        break;
                    }

                if (allAreTickets)
                {
                    this.GetList().Sort(delegate(InvoiceItem x, InvoiceItem y) { return (x.ShowTicketRecord.ShowDateRecord.DateOfShow_ToSortBy
                        .CompareTo(y.ShowTicketRecord.ShowDateRecord.DateOfShow_ToSortBy)); });
                }
            }   
        }
    }

    public partial class InvoiceItem
    {
        /// <summary>
        /// Note this include a trailing = sign
        /// these strings create matches for invoiceitems
        /// MerchBundleId can be found in the criteria
        /// </summary>
        public static readonly string MerchBundleIdConstant = "BundleId=";

        /// <summary>
        /// Note this include a trailing = sign
        /// these strings create matches for invoiceitems
        /// ParentItemId can be found in the criteria
        /// </summary>
        public static readonly string ParentItemIdConstant = "ParentId=";
        
        /// <summary>
        /// Note this include a trailing = sign
        /// these strings create context info for downloads and activation coded items
        /// DownloadCode can be found in the criteria
        /// </summary>
        public static readonly string DownloadCodeDeliveryConstant = "DownloadCode=";
        public static readonly string ActivationCodeDeliveryConstant = "ActivationCode=";
        public static readonly string GiftCertificateDeliveryConstant = "GiftCode=";
        public static readonly string DownloadCodeDeliveryLabel = "Download Code";
        public static readonly string ActivationCodeDeliveryLabel = "Activation Code";
        public static readonly string GiftCertificateDeliveryLabel = "Gift Code";


        public string DeliveryCode
        {
            get
            {
                if (!this.IsMerchandiseItem)
                    return null;

                if (this.DownloadDeliveryCode != null)
                    return DownloadDeliveryCode;

                if (this.GiftCertificateCode != null)
                    return GiftCertificateCode;

                if(this.ActivationDeliveryCode != null)
                    return ActivationDeliveryCode;

                return null;
            }
        }

        /// <summary>
        /// display the method and highlight if not default
        /// </summary>
        public string AdminDisplayShipMethod
        {
            get
            {
                bool isDefaultMethod =
                    ((this.IsShippingItem_Ticket && this.MainActName == _Config._Shipping_Tickets_DefaultMethod)
                ||
                    (this.IsShippingItem_Merch && this.MainActName == _Config._Shipping_Merch_DefaultMethod));

                return string.Format("<div class=\"ship-method\" >{0}</div>", (isDefaultMethod) ? 
                    this.MainActName : string.Format("<span class=\"ship-highlight\" >{0}</span>", this.MainActName));
            }
        }

        public static string FormatItemProductListing(InvoiceItem itm)
        {
            int productId = 0;
            int qty = itm.Quantity;
            _Enums.ItemContextCode code = _Enums.ItemContextCode.o;
            //decide context
            //get qty
            //get id
            
            //do not include processing fees
            //IGNORE because it is not a product but a payment -- if (itm.IsStoreCreditItem)
            
            if (itm.IsMerchandiseItem)
            {
                productId = itm.TMerchId.Value;
                code = _Enums.ItemContextCode.m;
            }
            else if (itm.IsTicketItem)
            {
                productId = itm.TShowTicketId.Value;
                code = _Enums.ItemContextCode.t;
            }
            else if (itm.IsPromotionItem)
            {
                productId = itm.TSalePromotionId.Value;
                code = _Enums.ItemContextCode.f;
            }
            else if (itm.IsCharitableItem)
            {
                productId = int.Parse(itm.Criteria);
                code = _Enums.ItemContextCode.y;
            }
            else if (itm.IsBundle)
            {
                productId = itm.TMerchBundleId.Value;
                code = _Enums.ItemContextCode.b;
            }
            //else if (itm.IsBundleSelection) this will show as a merch item
            //{
            //    productId = itm.TMerchId.Value;
            //    code = _Enums.ItemContextCode.b;
            //}

            //ignore shipping items

            if (productId > 0)
                return string.Format("{0},{1},{2}~", code.ToString(), qty, productId);

            return string.Empty;
        }

        #region Item Descriptions

        /// <summary>
        /// returns [Ticket: Date Ages MainActName][Merch: MainActName] - a bare bones description
        /// </summary>
        private string LineItemDescription_Brief
        {
            get
            {
                string desc = this.MainActName;

                if (this.IsTicketItem)
                {
                    string descript = string.Format("{0} {1}", this.Description ?? string.Empty, this.Criteria ?? string.Empty).Trim();

                    desc = desc.Insert(0, string.Format("{0} {1} ",
                        ShowTicket.IsCampingPass(descript) ? string.Empty : this.DateOfShow.ToString("ddd MM/dd/yyyy hh:mmtt"),
                        this.AgeDescription));
                }
                
                return System.Text.RegularExpressions.Regex.Replace(desc, @"\s+", " ");
            }
        }

        public string LineItemDescription_CriteriaAndDescription(bool DisplayEstimatedDates)
        {
            string itemListing = string.Empty;

            if (this.IsTicketItem)
                itemListing = string.Format("{0} @ {1}", Quantity.ToString(), LineItemDescription_Brief);
            else if (this.IsMerchandiseItem || this.IsPromotionItem)
                itemListing = string.Format("{0} @ {1}", Quantity.ToString(), LineItemDescription_Brief);
            else if (this.IsShippingItem && DisplayEstimatedDates)
                itemListing = string.Format("{0} @ {1}{2}", Quantity.ToString(), LineItemDescription_Brief,
                    (this.DateOfShow < DateTime.MaxValue) ? string.Format(" - Item(s) in this shipment will ship on or about ({0})", this.DateOfShow.ToString("MM/dd/yyyy")) : string.Empty);
            else
                itemListing = string.Format("{0} @ {1}", Quantity.ToString(), LineItemDescription_Brief);

            //tack on the criteria and description
            if (this.Description != null && this.Description.Trim().Length > 0)
                itemListing += string.Format(" - {0}", this.Description.Trim()); 
            
            if ((!this.IsBundle) && (!this.IsBundleSelection))
                if (this.Criteria != null && this.Criteria.Trim().Length > 0)
                    itemListing += string.Format(" - {0}", this.Criteria.Trim());

            return itemListing;
        }
        #endregion

        #region Properties

        [XmlAttribute("DateOfShow")]
        public DateTime DateOfShow
        {
            get { return (!this.DtDateOfShow.HasValue) ? DateTime.MaxValue : this.DtDateOfShow.Value; }
            set { this.DtDateOfShow = value; }
        }
        [XmlAttribute("Price")]
        public decimal Price
        {
            get { return decimal.Round(this.MPrice, 2); }
            set { this.MPrice = value; }
        }
        [XmlAttribute("ServiceCharge")]
        public decimal ServiceCharge
        {
            get { return decimal.Round(this.MServiceCharge, 2); }
            set { this.MServiceCharge = value; }
        }
        [XmlAttribute("Adjustment")]
        public decimal Adjustment
        {
            get { return decimal.Round(this.MAdjustment, 2); }
            set { this.MAdjustment = value; }
        }
        /// <summary>
        /// Base price + service charge
        /// </summary>
        [XmlAttribute("PricePerItem")]
        public decimal PricePerItem
        {
            get { return Price + ServiceCharge + Adjustment; }
        }
        [XmlAttribute("Quantity")]
        public int Quantity
        {
            get { return this.IQuantity; }
            set { this.IQuantity = value; }
        }
        [XmlAttribute("LineItemTotal")]
        public decimal LineItemTotal
        {
            get { return PricePerItem * Quantity; }
        }
        [XmlAttribute("DateShipped")]
        public DateTime DateShipped
        {
            get { return (!this.DtShipped.HasValue) ? DateTime.MaxValue : this.DtShipped.Value; }
            set { this.DtShipped = value; }
        }
        [XmlAttribute("Context")]
        public _Enums.InvoiceItemContext Context
        {
            get { return (_Enums.InvoiceItemContext)Enum.Parse(typeof(_Enums.InvoiceItemContext), this.VcContext, true); }
            set { this.VcContext = value.ToString(); }
        }
        [XmlAttribute("ReturnedToSender")]
        public bool ReturnedToSender
        {
            get { return (this.BRTS.HasValue) ? BRTS.Value : false; }
            set { this.BRTS = value; }
        }

        #endregion

        #region Derived Properties

        [XmlAttribute("IsTicketItem")]
        public bool IsTicketItem
        {
            get { return this.TShowTicketId.HasValue && this.Context == _Enums.InvoiceItemContext.ticket; }
        }
        [XmlAttribute("IsServiceChargeItem")]
        public bool IsServiceChargeItem
        {
            get { return this.TShowTicketId.HasValue && this.Context == _Enums.InvoiceItemContext.servicecharge; }
        }
        [XmlAttribute("IsShippingItem")]
        public bool IsShippingItem
        {
            get { return (this.IsShippingItem_Merch || this.IsShippingItem_Ticket); }
        }
        [XmlAttribute("IsShippingItem_Merch")]
        public bool IsShippingItem_Merch
        {
            get { return (this.Context == _Enums.InvoiceItemContext.shippingmerch); }
        }
        [XmlAttribute("IsShippingItem_Ticket")]
        public bool IsShippingItem_Ticket
        {
            get { return (this.Context == _Enums.InvoiceItemContext.shippingticket); }
        }
        [XmlAttribute("IsProcessingFee")]
        public bool IsProcessingFee
        {
            get { return (this.Context == _Enums.InvoiceItemContext.processing); }
        }
        [XmlAttribute("IsMerchandiseItem")]
        public bool IsMerchandiseItem
        {
            get
            {
                return this.TMerchId.HasValue && (this.Context == _Enums.InvoiceItemContext.merch);
            }
        }

       
        /// <summary>
        /// ParentItemId is in criteria
        /// MerchBundleId is in Description
        /// </summary>
        [XmlAttribute("IsBundle")]
        public bool IsBundle
        {
            get { return this.Context == _Enums.InvoiceItemContext.bundle; }
        }
        [XmlAttribute("IsBundleSelection")]
        public bool IsBundleSelection
        {
            get { return (this.TMerchBundleId.HasValue && this.Context == _Enums.InvoiceItemContext.merch); }
        }

        private MerchBundle _merchBundle = null;
        public MerchBundle MerchBundleRecord
        {
            get
            {
                if (this.IsBundle && _merchBundle == null)
                {
                    if(this.TMerchBundleId.HasValue)
                        _merchBundle = (MerchBundle)_Lookits.MerchBundles.Find(this.TMerchBundleId.Value);

                    //if not found - give it another try
                    if(_merchBundle == null)
                        _merchBundle = new MerchBundle(this.TMerchBundleId);
                }

                return _merchBundle;
            }
        }
        public System.Collections.Generic.List<InvoiceItem> AssociatedBundles
        {
            get
            {
                if ((!this.IsBundle) && (!this.IsBundleSelection))
                {
                    //get any bundles tied to this object
                    return this.Invoice.InvoiceItemRecords().GetList()
                        .FindAll(delegate(InvoiceItem match) { return 
                            match.IsBundle && match.TParentInvoiceItemId == this.Id; });
                }

                return null;
            }
        }
        public System.Collections.Generic.List<InvoiceItem> AssociatedBundleSelections
        {
            get
            {
                if (this.IsBundle)
                {
                    //get any bundles tied to this object
                    return this.Invoice.InvoiceItemRecords().GetList()
                        .FindAll(delegate(InvoiceItem match) { return 
                            match.IsBundleSelection && match.TParentInvoiceItemId == this.Id; });
                }

                return null;
            }
        }

        
        [XmlAttribute("IsDownloadDelivery")]
        public bool IsDownloadDelivery
        {
            get 
            {
                return (this.Context == _Enums.InvoiceItemContext.merch && DownloadDeliveryCode != null);
            }
        }
        [XmlAttribute("IsActivationCodeDelivery")]
        public bool IsActivationCodeDelivery
        {
            get
            {
                return (this.Context == _Enums.InvoiceItemContext.merch && ActivationDeliveryCode != null);
            }
        }
        [XmlAttribute("IsGiftCertificateDelivery")]
        public bool IsGiftCertificateDelivery
        {
            get
            {
                return (this.Context == _Enums.InvoiceItemContext.merch && GiftCertificateCode != null);
            }
        }
        /// <summary>
        /// indicates if the item is deliverable by a code as opposed to parcel or physical good
        /// </summary>
        [XmlAttribute("IsDeliverableByCode")]
        public bool IsDeliverableByCode
        {
            get
            {
                return (this.DeliveryCode != null);
            }
        }

        public string GetDeliveryCodeLabel()
        {
            if (this.IsDeliverableByCode)
            {
                if(this.IsGiftCertificateDelivery)
                    return InvoiceItem.GiftCertificateDeliveryLabel;

                if (this.IsActivationCodeDelivery)
                    return InvoiceItem.ActivationCodeDeliveryLabel;

                if (this.IsDownloadDelivery)
                    return InvoiceItem.DownloadCodeDeliveryLabel;
            }

            return null;
        }

        private string _downloadDeliveryCode = null;
        public string DownloadDeliveryCode
        {
            get
            {
                if (_downloadDeliveryCode == null)
                    _downloadDeliveryCode = this.GetCriteriaCode(InvoiceItem.DownloadCodeDeliveryConstant);

                return _downloadDeliveryCode;
            }
        }
        private string _activationDeliveryCode = null;
        public string ActivationDeliveryCode
        {
            get
            {
                if (_activationDeliveryCode == null)
                    _activationDeliveryCode = this.GetCriteriaCode(InvoiceItem.ActivationCodeDeliveryConstant);

                return _activationDeliveryCode;
            }
        }
        private string _giftCertificateCode = null;
        public string GiftCertificateCode
        {
            get
            {
                if (_giftCertificateCode == null)
                    _giftCertificateCode = this.GetCriteriaCode(InvoiceItem.GiftCertificateDeliveryConstant);

                return _giftCertificateCode;
            }
        }

        private string GetCriteriaCode(string paramKey)
        {
            if (this.IsMerchandiseItem && this.Criteria != null && this.Criteria.IndexOf(paramKey) != -1)
            {
                string[] pieces = this.Criteria.Split('&');

                foreach (string s in pieces)
                {
                    if (s.StartsWith(paramKey))
                    {
                        string[] parts = s.Split('=');
                        return parts[1];
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Corresponds to the MerchBundle tied to a merch or ticket
        /// </summary>
        private int? _TMerchBundleId = null;
        public int? TMerchBundleId
        {
            get
            {
                if (_TMerchBundleId == null && this.Criteria != null && this.Criteria.IndexOf(InvoiceItem.MerchBundleIdConstant) != -1)
                {
                    string[] pieces = this.Criteria.Split('&');

                    foreach (string s in pieces)
                    {
                        if (s.StartsWith(InvoiceItem.MerchBundleIdConstant))
                        {
                            string[] parts = s.Split('=');

                            int idx = 0;

                            if (int.TryParse(parts[1], out idx))
                                _TMerchBundleId = idx;
                        }
                    }
                }

                return _TMerchBundleId;
            }
        }
        private int? _TParentInvoiceItemId = null;
        public int? TParentInvoiceItemId
        {
            get
            {
                if (_TParentInvoiceItemId == null && this.Criteria != null && this.Criteria.IndexOf(InvoiceItem.ParentItemIdConstant) != -1)
                {
                    string[] pieces = this.Criteria.Split('&');

                    foreach (string s in pieces)
                    {
                        if (s.StartsWith(InvoiceItem.ParentItemIdConstant))
                        {
                            string[] parts = s.Split('=');

                            int idx = 0;

                            if (int.TryParse(parts[1], out idx))
                                _TParentInvoiceItemId = idx;
                        }
                    }
                }

                return _TParentInvoiceItemId;
            }
        }

        [XmlAttribute("IsCharitableItem")]
        public bool IsCharitableItem
        {
            get { return this.Context == _Enums.InvoiceItemContext.charity; }
        }
        /// <summary>
        /// Indicates if the item has been removed from theinvoice - refunded, credited
        /// </summary>
        [XmlAttribute("HasBeenRemoved")]
        public bool HasBeenRemoved
        {
            get { return (this.PurchaseAction == _Enums.PurchaseActions.PurchasedThenRemoved.ToString() || this.PurchaseAction == _Enums.PurchaseActions.Credited.ToString()); }
        }
        /// <summary>
        /// PLEASE NOTE! this is for discounts applied after the sale - promotional discounts are different!
        /// </summary>
        [XmlAttribute("IsDiscountItem")]
        public bool IsDiscountItem
        {
            get { return this.Context == _Enums.InvoiceItemContext.discount; }
        }
        [XmlAttribute("IsPromotionItem")]
        public bool IsPromotionItem
        {
            get { return this.TSalePromotionId.HasValue && this.TSalePromotionId > 0; }
        }

        [XmlAttribute("PickupName_Derived")]
        public string PickupName_Derived
        {
            get { return (this.PickupName == null) ? string.Empty : this.PickupName; }
        }

        #endregion
    }
}
