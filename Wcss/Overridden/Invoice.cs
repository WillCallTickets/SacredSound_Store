using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wcss
{
    public partial class Invoice
    {
        #region Working Vars
        public string WorkingShippingAddress
        {
            get
            {
                InvoiceBillShip lading = this.InvoiceBillShip;
                return string.Format("{0} {1}", lading.Address1_Working, lading.Address2_Working).Trim();
            }
        }
        public string WorkingCountry
        {
            get
            {
                InvoiceBillShip lading = this.InvoiceBillShip;
                return lading.Country_Working;
            }
        }
        public string WorkingState
        {
            get
            {
                InvoiceBillShip lading = this.InvoiceBillShip;
                return lading.State_Working;
            }
        }
        public string WorkingZip
        {
            get
            {
                InvoiceBillShip lading = this.InvoiceBillShip;
                return lading.Zip_Working;
            }
        }
        #endregion

        
        /// <summary>
        /// returns the shipment date for each item in the or if null - a Pending message. Returns HTML
        /// </summary>
        public string ShippingStatus_PostSale
        {
            get
            {
                if (this.ShippingItems.Count == 0)
                    return string.Empty;

                System.Text.StringBuilder sb = new System.Text.StringBuilder();

                foreach (InvoiceItem ent in this.ShippingItems)
                {
                    if (ent.PurchaseAction.ToLower() == _Enums.PurchaseActions.Purchased.ToString().ToLower())
                    {
                        //display the method and highlight if not default
                        sb.AppendLine(ent.AdminDisplayShipMethod);

                        //display the items that have shipped or the status
                        if(ent.DateShipped < DateTime.MaxValue)
                        {
                            //display the ship date
                            sb.AppendFormat("<div>{0} {1}</div>", (ent.IsShippingItem_Merch) ? "mrch" : "tkt", ent.DateShipped.ToString("MM/dd/yyyy hh:mmtt"));

                            //for every item slated to ship in this shipment....
                            foreach(InvoiceItem itemToShip in ent.ChildInvoiceItemRecords())
                            {
                                if(itemToShip.PurchaseAction.ToLower() == _Enums.PurchaseActions.Purchased.ToString().ToLower())
                                {
                                    if(itemToShip.DateShipped == DateTime.MaxValue)
                                        sb.AppendFormat("<div>{0} <span style=\"Color: Red;Font-Weight: Bold;\">PENDING Id: {1}</span></div>", 
                                            (itemToShip.IsMerchandiseItem) ? "mrch" : "tkt", itemToShip.Id.ToString());
                                }
                            }                                
                        }
                        else
                        {
                            //nothing has shipped yet
                            sb.AppendFormat("<div>{0} <span style=\"Color: Red;Font-Weight: Bold;\">PENDING</span></div>", (ent.IsShippingItem_Merch) ? "mrch" : "tkt");                            

                        }
                    }
                }

                return sb.ToString();
            }
        }

        #region InvoiceProductListing

        public class InvoiceProductListing
        {
            private _Enums.ItemContextCode _code;
            private int _qty;
            private int _itemId;

            private InvoiceProductListing()
            {
                _code = _Enums.ItemContextCode.o;
                _qty = 0;
                _itemId = 0;
            }
            private InvoiceProductListing(_Enums.ItemContextCode code, int qty, int itemId)
            {
                _code = code;
                _qty = qty;
                _itemId = itemId;
            }

            public _Enums.ItemContextCode Code { get { return _code; } set { _code = value; } }
            public int Qty { get { return _qty; } set { _qty = value; } }
            public int ItemId { get { return _itemId; } set { _itemId = value; } }

            public static List<InvoiceProductListing> InvoiceProducts(string productList)
            {
                List<InvoiceProductListing> products = new List<InvoiceProductListing>();

                if (productList != null && productList.Trim().Length > 0)
                {
                    string[] prods = productList.TrimEnd('~').Split('~');

                    for (int i = 0; i < prods.Length; i++)
                    {
                        string[] pieces = prods[i].TrimEnd('~', ',').Split(',');

                        if (pieces.Length == 3)
                        {
                            InvoiceProductListing listing = new InvoiceProductListing(
                                (_Enums.ItemContextCode)Enum.Parse(typeof(_Enums.ItemContextCode), pieces[0], true),
                                int.Parse(pieces[1]),
                                int.Parse(pieces[2]));

                            products.Add(listing);
                        }
                    }
                }

                return products;
            }
        }
        
        #endregion 

        #region Shippable Items - Post Sale
        /// <summary>
        /// To be used post sale - do not ref MerchItems as we want promotional items as well
        /// Allow unique codes to be delivered in the packing slip?
        /// </summary>
        public InvoiceItemCollection ShippableMerchItems_PostSale_AllowActivationCodeDelivery
        {
            get
            {
                InvoiceItemCollection coll = new InvoiceItemCollection();

                if (this.MerchandiseShipmentItems.Count > 0)
                {
                    foreach (InvoiceItem ii in this.InvoiceItemRecords())
                        if (ii.Context == _Enums.InvoiceItemContext.merch &&
                            //Allow unique codes to be delivered in the packing slip?
                            ((!ii.IsDownloadDelivery) && (!ii.IsGiftCertificateDelivery)))
                            coll.Add(ii);
                }

                return coll;
            }
        }

        /// <summary>
        /// Finds ticket items that weren't slated for shipping at time of sale - user would like to ship availables now
        /// </summary>
        public bool HasTicketItems_ThatCanBeShippedPostSale { get { return TicketItems_ThatCanBeShippedPostSale.Count > 0; } }
        /// <summary>
        /// To be used post sale
        /// </summary>
        public InvoiceItemCollection TicketItems_ThatCanBeShippedPostSale
        {
            get
            {
                InvoiceItemCollection coll = new InvoiceItemCollection();

                foreach (InvoiceItem ii in this.InvoiceItemRecords())
                {
                    if (ii.PurchaseAction == _Enums.PurchaseActions.Purchased.ToString() && ii.Context == _Enums.InvoiceItemContext.ticket && ii.ShowTicketRecord.IsCurrentlyShippable &&
                        (ii.ShippingMethod == null || ii.ShippingMethod.Trim().Length == 0 || ii.ShippingMethod == ShipMethod.WillCall))
                        coll.Add(ii);
                }

                return coll;
            }
        }

        /// <summary>
        /// To be used post sale - only for admin - we removed the date cutoff restriction here
        /// </summary>
        public bool HasTicketItems_NotYetShippedPostSale { get { return TicketItems_NotYetShippedPostSale.Count > 0; } }
        /// <summary>
        /// To be used post sale - only for admin - we removed the date cutoff restriction here
        /// </summary>
        public InvoiceItemCollection TicketItems_NotYetShippedPostSale
        {
            get
            {
                InvoiceItemCollection coll = new InvoiceItemCollection();

                if (this.HasTicketShipmentItemsOtherThanWillCall)
                {
                    foreach (InvoiceItem ii in this.InvoiceItemRecords())
                        if (ii.PurchaseAction == _Enums.PurchaseActions.Purchased.ToString() && ii.Context == _Enums.InvoiceItemContext.ticket && ii.ShowTicketRecord.IsCurrentlyShippable &&
                            (ii.ShippingMethod != null && ii.ShippingMethod.Trim() != ShipMethod.WillCall))
                            coll.Add(ii);
                }

                return coll;
            }
        }

        #endregion

        #region ProductListing

        /// <summary>
        /// Provides a way to create links and descriptions for invoiceitems listed in grids. Promotional items are 
        /// included insofar as they are merch or ticket items. ie: There are no links to discounts and shipping items are not listed.
        /// Note that we do not use the invoice item's LineItemDescription_ here because we need to create links to products based on IsAdmin.
        /// Also retrieves items from current collections to avoid DB hits
        /// </summary>
        public static string InterpretProductDescription(bool isAdmin, Invoice invoice, ShowTicketCollection currentTickets, MerchCollection currentMerch)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            List<InvoiceItem> sold = new List<InvoiceItem>();
            sold.AddRange(invoice.InvoiceItemRecords().GetList().FindAll(delegate(InvoiceItem match)
            {
                //note that we allow for legacy products here - linked ticket shipping
                return (match.PurchaseAction != _Enums.PurchaseActions.NotYetPurchased.ToString() &&
                    (match.IsMerchandiseItem || match.IsTicketItem || match.IsPromotionItem || match.IsCharitableItem || 
                    (match.MainActName != null && match.MainActName.Trim().Length > 0 && match.MainActName.IndexOf("SHIPPING PURCHASED FOR") != -1)));
            }));

            foreach (InvoiceItem ii in sold)
            {
                bool notPurchased = (ii.PurchaseAction != _Enums.PurchaseActions.Purchased.ToString());

                int idx = 0;

                if (ii.IsMerchandiseItem)
                    idx = ii.TMerchId.Value;
                else if (ii.IsTicketItem)
                    idx = ii.TShowTicketId.Value;
                else if (ii.IsPromotionItem)
                    idx = ii.TSalePromotionId.Value;//discounts and shipping discounts
                else if (ii.IsCharitableItem)
                {
                    //orgid is in criteria
                    idx = int.Parse(ii.Criteria);
                }
                else
                    idx = 0;

                if (notPurchased)
                    sb.Append("<div class=\"descreturned\">");

                if (idx == 0)
                {
                    sb.AppendFormat("<a class=\"descrow\">{0}</a>", ii.MainActName.Trim());
                }
                else if (idx > 0)
                {
                    int qty = ii.Quantity;

                    if (ii.IsTicketItem)
                    {
                        ShowTicket tik = (ShowTicket)currentTickets.Find(idx);
                        if (tik == null)
                            tik = ShowTicket.FetchByID(idx);

                        if (tik == null)
                            sb.AppendFormat("<a class=\"descrow\">(qty{0}) ticket #{1} could not be found</a>", qty, idx);
                        else
                        {
                            string description = Utils.ParseHelper.StripHtmlTags(string.Format("{0} {1}", tik.SalesDescription_Derived, tik.CriteriaText_Derived)).Trim();

                            string desc = string.Format("{0} ({1}) {2} {3} ", 
                                (tik.IsCampingPass()) ? "CAMPING" : tik.DateOfShow.ToString("MM/dd/yy hh:mmtt"),
                                (tik.AgeDescription.Length > 8) ? tik.AgeDescription.Substring(0, 8).Trim() : tik.AgeDescription,
                                tik.ShowDateRecord.ShowRecord.ShowNamePart, description).Trim();

                            //dont link to promo items for customer
                            string href = (isAdmin) ? string.Format("/Admin/Listings.aspx?p=tickets&tixid={0}", idx) :
                                ((!ii.IsPromotionItem) && ii.DateOfShow > DateTime.Now) ? string.Format("/Store/ChooseTicket.aspx?sid={0}", ii.TShowId) :
                                string.Empty;

                            sb.AppendFormat("<a class=\"descrow\"{0}>{1}@ {2}</a>",
                                (href.Trim().Length > 0) ? string.Format(" href=\"{0}\"", href) : string.Empty,
                                qty, (desc.Length > 100) ? desc.Substring(0, 100) : desc);

                            //Post PurchaseText
                            if (!notPurchased)
                            {
                                //POST PURCHASE TEXTS
                                InvoiceItemPostPurchaseTextCollection postColl = new InvoiceItemPostPurchaseTextCollection();
                                postColl.AddRange(ii.InvoiceItemPostPurchaseTextRecords());

                                if (postColl.Count > 0)
                                {
                                    postColl.Sort("IDisplayOrder", true);
                                    sb.Append("<div class=\"postpurchase\">");

                                    foreach (InvoiceItemPostPurchaseText pp in postColl)
                                        sb.AppendFormat("<div class=\"pptext\">{0}</div>", pp.PostText.Trim());

                                    sb.Append("</div>");
                                }
                            }
                        }
                    }
                    else if (ii.IsMerchandiseItem)
                    {
                        Merch mer = (Merch)currentMerch.Find(idx);
                        if (mer == null)
                            mer = Merch.FetchByID(idx);

                        if (mer == null)
                            sb.AppendFormat("<a class=\"descrow\" href=\"\">(qty{0})merch item #{1} could not be found</a>", qty, idx);
                        else
                        {
                            string desc = string.Format("{0}", mer.DisplayNameWithAttribs).Trim();

                            //dont link customer to promo items
                            string href = (isAdmin) ? string.Format("/Admin/MerchEditor.aspx?p=ItemEdit&merchitem={0}", idx) :
                                (ii.IsPromotionItem) ? string.Empty : string.Format("/Store/ChooseMerch.aspx?mite={0}", idx);

                            sb.AppendFormat("<a class=\"descrow\"{0}>{1}@ {2}</a>",
                                (href.Trim().Length > 0) ? string.Format(" href=\"{0}\"", href) : string.Empty,
                                qty, (desc.Length > 100) ? desc.Substring(0, 100) : desc);

                            ////Post PurchaseText
                            //if (!notPurchased)
                            //{
                            //    //POST PURCHASE TEXTS
                            //    InvoiceItemPostPurchaseTextCollection postColl = new InvoiceItemPostPurchaseTextCollection();
                            //    postColl.AddRange(ii.InvoiceItemPostPurchaseTextRecords());

                            //    if (postColl.Count > 0)
                            //    {
                            //        postColl.Sort("IDisplayOrder", true);
                            //        sb.Append("<div class=\"postpurchase\">");

                            //        foreach (InvoiceItemPostPurchaseText pp in postColl)
                            //            sb.AppendFormat("<div class=\"pptext\">{0}</div>", pp.PostText.Trim());

                            //        sb.Append("</div>");
                            //    }
                            //}
                        }
                    }
                    else if (ii.IsPromotionItem)//discounts and shipping discounts
                    {
                        string desc = string.Format("{0} {1}", ii.MainActName.Trim(), ii.Description ?? string.Empty).Trim();

                        sb.AppendFormat("<a class=\"descrow\">{0}@ {1}</a>", qty, (desc.Length > 100) ? desc.Substring(0, 100) : desc);
                    }
                    else if (ii.IsCharitableItem)//discounts and shipping discounts
                    {
                        string desc = ii.MainActName.Trim();

                        sb.AppendFormat("{0} donation to {1}", ii.LineItemTotal.ToString("c"), (desc.Length > 100) ? desc.Substring(0, 100) : desc);
                    }
                }

                if (notPurchased)
                    sb.Append("</div>");
            }

            return sb.ToString().Trim();
        }

        #endregion

        #region Collections & Children

        [XmlAttribute("ProcessingItems")]
        public InvoiceItemCollection ProcessingItems
        {
            get
            {
                InvoiceItemCollection coll = new InvoiceItemCollection();
                coll.AddRange(this.InvoiceItemRecords().GetList().FindAll(
                    delegate(InvoiceItem match) { return (match.IsProcessingFee && (!match.IsPromotionItem)); }));

                return coll;
            }
        }
        [XmlAttribute("ShippingItems")]
        public InvoiceItemCollection ShippingItems
        {
            get
            {
                InvoiceItemCollection coll = new InvoiceItemCollection();
                coll.AddRange(TicketShipmentItems);
                coll.AddRange(MerchandiseShipmentItems);

                return coll;
            }
        }
        [XmlAttribute("TicketItems")]
        public InvoiceItemCollection TicketItems
        {
            get
            {
                InvoiceItemCollection coll = new InvoiceItemCollection();
                coll.AddRange(this.InvoiceItemRecords().GetList().FindAll(
                    delegate(InvoiceItem match) { return (match.IsTicketItem && (!match.IsPromotionItem)); }));

                return coll;
            }
        }
        [XmlAttribute("TicketShipmentItems")]
        public InvoiceItemCollection TicketShipmentItems
        {
            get
            {
                InvoiceItemCollection coll = new InvoiceItemCollection();
                coll.AddRange(this.InvoiceItemRecords().GetList().FindAll(
                    delegate(InvoiceItem match) { return (match.Context == _Enums.InvoiceItemContext.shippingticket && (!match.IsPromotionItem)); }));

                return coll;
            }
        }
        [XmlAttribute("MerchandiseShipmentItems")]
        public InvoiceItemCollection MerchandiseShipmentItems
        {
            get
            {
                InvoiceItemCollection coll = new InvoiceItemCollection();
                coll.AddRange(this.InvoiceItemRecords().GetList().FindAll(
                    delegate(InvoiceItem match) { return (match.Context == _Enums.InvoiceItemContext.shippingmerch && (!match.IsPromotionItem)); }));

                return coll;
            }
        }
        [XmlAttribute("MerchItems")]
        public InvoiceItemCollection MerchItems
        {
            get
            {
                InvoiceItemCollection coll = new InvoiceItemCollection();
                coll.AddRange(this.InvoiceItemRecords().GetList().FindAll(
                    delegate(InvoiceItem match) { return (match.IsMerchandiseItem && (!match.IsPromotionItem)); }));

                return coll;
            }
        }
        private InvoiceItemCollection _bundleItems = null;
        [XmlAttribute("BundleItems")]
        public InvoiceItemCollection BundleItems
        {
            get
            {
                if (_bundleItems == null)
                {
                    _bundleItems = new InvoiceItemCollection();
                    _bundleItems.AddRange(this.InvoiceItemRecords().GetList().FindAll(
                        delegate(InvoiceItem match) { return (match.IsBundle); }));
                }
                return _bundleItems;
            }
        }
        private InvoiceItemCollection _bundleSelectionItems = null;
        [XmlAttribute("BundleSelectionItems")]
        public InvoiceItemCollection BundleSelectionItems
        {
            get
            {
                if (_bundleSelectionItems == null)
                {
                    _bundleSelectionItems = new InvoiceItemCollection();
                    if (this.HasBundles)
                    {
                        _bundleSelectionItems.AddRange(this.InvoiceItemRecords().GetList().FindAll(
                            delegate(InvoiceItem match) { return (match.IsBundleSelection); }));
                    }
                }
                return _bundleSelectionItems;
            }
        }
        [XmlAttribute("PromotionItems")]
        public InvoiceItemCollection PromotionItems
        {
            get
            {
                InvoiceItemCollection coll = new InvoiceItemCollection();
                coll.AddRange(this.InvoiceItemRecords().GetList().FindAll(
                    delegate(InvoiceItem match) { return (match.IsPromotionItem); }));

                return coll;
            }
        }
        [XmlAttribute("CharitableItems")]
        public InvoiceItemCollection CharitableItems
        {
            get
            {
                InvoiceItemCollection coll = new InvoiceItemCollection();
                coll.AddRange(this.InvoiceItemRecords().GetList().FindAll(
                    delegate(InvoiceItem match) { return (match.IsCharitableItem); }));

                return coll;
            }
        }
        [XmlAttribute("GiftDeliverableItems")]
        public InvoiceItemCollection GiftDeliverableItems
        {
            get
            {
                InvoiceItemCollection coll = new InvoiceItemCollection();
                coll.AddRange(this.InvoiceItemRecords().GetList().FindAll(
                    delegate(InvoiceItem match) { return (match.IsGiftCertificateDelivery); }));

                return coll;
            }
        }
        [XmlAttribute("DownloadDeliveryItems")]
        public InvoiceItemCollection DownloadDeliveryItems
        {
            get
            {
                InvoiceItemCollection coll = new InvoiceItemCollection();
                coll.AddRange(this.InvoiceItemRecords().GetList().FindAll(
                    delegate(InvoiceItem match) { return (match.IsDownloadDelivery); }));

                return coll;
            }
        }
        [XmlAttribute("StoreCreditTransactions")]
        public InvoiceTransactionCollection StoreCreditTransactions
        {
            get
            {
                InvoiceTransactionCollection coll = new InvoiceTransactionCollection();
                coll.AddRange(this.InvoiceTransactionRecords().GetList().FindAll(
                    delegate(InvoiceTransaction match) { return (match.FundsType.ToLower() == _Enums.FundsTypes.StoreCredit.ToString().ToLower()); }));

                return coll;
            }
        }
        [XmlAttribute("CreditCardTransactions")]
        public InvoiceTransactionCollection CreditCardTransactions
        {
            get
            {
                InvoiceTransactionCollection coll = new InvoiceTransactionCollection();
                coll.AddRange(this.InvoiceTransactionRecords().GetList().FindAll(
                    delegate(InvoiceTransaction match) { return (match.FundsType.ToLower() == _Enums.FundsTypes.CreditCard.ToString().ToLower()); }));

                return coll;
            }
        }
        [XmlAttribute("CompanyCheckTransactions")]
        public InvoiceTransactionCollection CompanyCheckTransactions
        {
            get
            {
                InvoiceTransactionCollection coll = new InvoiceTransactionCollection();
                coll.AddRange(this.InvoiceTransactionRecords().GetList().FindAll(
                    delegate(InvoiceTransaction match) { return (match.FundsType.ToLower() == _Enums.FundsTypes.CompanyCheck.ToString().ToLower()); }));

                return coll;
            }
        }
        [XmlAttribute("InvoiceBillShip")]
        public InvoiceBillShip InvoiceBillShip
        {
            get
            {
                if (this.InvoiceBillShipRecords().Count == 0)
                    return null;

                return this.InvoiceBillShipRecords()[0];
            }
        }

        [XmlAttribute("HasTicketItems")]
        public bool HasTicketItems { get { return (TicketItems.Count > 0); } }
        [XmlAttribute("HasTicketShipmentItemsOtherThanWillCall")]
        public bool HasTicketShipmentItemsOtherThanWillCall 
        { 
            get 
            {
                if (TicketShipmentItems.Count > 0)
                {
                    //if we any have shipment items that are not will call 
                    int idx = this.TicketShipmentItems.GetList().FindIndex(delegate(InvoiceItem match) { 
                        return 
                            (match.MainActName.ToLower().IndexOf(ShipMethod.WillCall.ToLower()) == -1 && 
                            match.MainActName.ToLower().IndexOf("willcall") == -1); } );

                    //then return if there was an item found
                    return (idx != -1);
                }

                return false;
            } 
        }
        [XmlAttribute("HasMerchandiseShipmentItems")]
        public bool HasMerchandiseShipmentItems { get { return (MerchandiseShipmentItems.Count > 0); } }
        [XmlAttribute("HasMerchItems")]
        public bool HasMerchItems { get { return (MerchItems.Count > 0); } }
        [XmlAttribute("HasPromotionItems")]
        public bool HasPromotionItems { get { return (PromotionItems.Count > 0); } }
        [XmlAttribute("HasCharitableItems")]
        public bool HasCharitableItems { get { return (CharitableItems.Count > 0); } }
        [XmlAttribute("HasGiftDeliverableItems")]
        public bool HasGiftDeliverableItems { get { return (GiftDeliverableItems.Count > 0); } }
        [XmlAttribute("HasStoreCreditTransactions")]
        public bool HasStoreCreditTransactions { get { return (StoreCreditTransactions.Count > 0); } }
        [XmlAttribute("HasCreditCardTransactions")]
        public bool HasCreditCardTransactions { get { return (CreditCardTransactions.Count > 0); } }
        [XmlAttribute("HasCompanyCheckTransactions")]
        public bool HasCompanyCheckTransactions { get { return (CompanyCheckTransactions.Count > 0); } }
        [XmlAttribute("HasBundles")]
        public bool HasBundles { get { return (BundleItems.Count > 0); } }
        [XmlAttribute("HasBundleSelections")]
        public bool HasBundleSelections { get { return (BundleSelectionItems.Count > 0); } }
        [XmlAttribute("HasDownloadDeliveryItems")]
        public bool HasDownloadDeliveryItems { get { return (DownloadDeliveryItems.Count > 0); } }

        #endregion

        #region Properties

        [XmlAttribute("InvoiceDate")]
        public DateTime InvoiceDate
        {
            get { return this.DtInvoiceDate; }
            set { this.DtInvoiceDate = value; }
        }        
        [XmlAttribute("BalanceDue")]
        public decimal BalanceDue
        {
            get { return decimal.Round(this.MBalanceDue, 2); }
            set { this.MBalanceDue = value; }
        }
        [XmlAttribute("TotalPaid")]
        public decimal TotalPaid
        {
            get { return decimal.Round(this.MTotalPaid,2); }
            set { this.MTotalPaid = value; }
        }
        [XmlAttribute("TotalRefunds")]
        public decimal TotalRefunds
        {
            get { return decimal.Round(this.MTotalRefunds,2); }
            set { this.MTotalRefunds = value; }
        }
        [XmlAttribute("NetPaid")]
        public decimal NetPaid
        {
            get { return TotalPaid - TotalRefunds; }
        }
        [XmlAttribute("TicketShipping")]
        public decimal TicketShipping
        {
            get 
            {
                decimal total = 0;

                foreach (InvoiceItem itm in this.ShippingItems)
                    if (itm.IsShippingItem_Ticket)
                        total += itm.LineItemTotal;

                return decimal.Round(total, 2);
            }
        }
        [XmlAttribute("MerchandiseShipping")]
        public decimal MerchandiseShipping
        {
            get 
            {
                decimal total = 0;

                foreach (InvoiceItem itm in this.ShippingItems)
                    if(itm.IsShippingItem_Merch)
                        total += itm.LineItemTotal;

                return decimal.Round(total, 2);
            }
        }
        /// <summary>
        /// The total of ticket and merchandise shipping
        /// </summary>
        [XmlAttribute("ShippingAndHandling")]
        public decimal ShippingAndHandling
        {
            get { return decimal.Round(this.TicketShipping + this.MerchandiseShipping, 2); }
        }
        [XmlAttribute("CharitableTotal")]
        public decimal CharitableTotal
        {
            get
            {
                decimal total = 0;

                foreach (InvoiceItem itm in this.CharitableItems)
                    if (itm.PurchaseAction == _Enums.PurchaseActions.Purchased.ToString())
                        total += itm.LineItemTotal;

                return decimal.Round(total, 2);
            }
        }
        [XmlAttribute("StoreCreditPaymentsTotal")]
        public decimal StoreCreditPaymentsTotal
        {
            get
            {
                decimal total = 0;

                foreach (InvoiceTransaction itm in this.StoreCreditTransactions)
                    total += (itm.TransType.ToLower() == _Enums.TransTypes.Payment.ToString().ToLower()) ? itm.Amount : 0;

                return decimal.Round(total, 2);
            }
        }
        [XmlAttribute("StoreCreditTotal")]
        public decimal StoreCreditTotal
        {
            get
            {
                decimal total = 0;

                foreach (InvoiceTransaction itm in this.StoreCreditTransactions)
                    total += (itm.TransType.ToLower() == _Enums.TransTypes.Payment.ToString().ToLower()) ? itm.Amount : (itm.Amount * -1);

                return decimal.Round(total, 2);
            }
        }
        [XmlAttribute("CreditCardPaymentsTotal")]
        public decimal CreditCardPaymentsTotal
        {
            get
            {
                decimal total = 0;

                foreach (InvoiceTransaction itm in this.CreditCardTransactions)
                    total += (itm.TransType.ToLower() == _Enums.TransTypes.Payment.ToString().ToLower()) ? itm.Amount : 0;

                return decimal.Round(total, 2);
            }
        }
        [XmlAttribute("CompanyCheckPaymentsTotal")]
        public decimal CompanyCheckPaymentsTotal
        {
            get
            {
                decimal total = 0;

                foreach (InvoiceTransaction itm in this.CompanyCheckTransactions)
                    total += (itm.TransType.ToLower() == _Enums.TransTypes.Payment.ToString().ToLower()) ? itm.Amount : (itm.Amount * -1);

                return decimal.Round(total, 2);
            }
        }
        #endregion

        #region Totals/Amounts

        /// <summary>
        /// Shown on the cart purchase page to indicate the original amount paid for the invoice. 
        /// Discounts/Exchanges/Refunds after the fact are not included here. 
        /// Essentially reset/ignore.negate the promotion
        /// </summary>
        [XmlAttribute("OriginalCartTotal")]
        public decimal OriginalCartTotal 
        { 
            get 
            {
                return this.TotalPaid - this.OriginalProcessingFee - this.ShippingAndHandling + Utils.ParseHelper.AbsoluteValue(this.PromotionalDiscountTotal); 
            } 
        }
        [XmlAttribute("OriginalProcessingFee")]
        public decimal OriginalProcessingFee
        {
            get
            {
                decimal fee = 0;

                foreach (InvoiceItem ii in this.ProcessingItems)
                    if (ii.PurchaseAction != _Enums.PurchaseActions.NotYetPurchased.ToString())
                        fee += Utils.ParseHelper.AbsoluteValue(ii.LineItemTotal);

                return decimal.Round(fee, 2);
            }
        }

        [XmlAttribute("TotalPaidAfterCredit")]
        public decimal TotalPaidAfterCredit { get { return this.TotalPaid - this.StoreCreditTotal; } }

        /// <summary>
        /// Use this for orders not refunded. Does not include shipping or discounts. Total of items only. Needs to include promotion items that are not discounts.
        /// </summary>
        [XmlAttribute("Total_TicketsAndMerch")]
        public decimal Total_TicketsAndMerch
        {
            get
            {
                decimal sub = 0;

                foreach (InvoiceItem ii in this.InvoiceItemRecords())
                {
                    //includes promotion items that are not less than zero(discounts)
                    if (ii.PurchaseAction == _Enums.PurchaseActions.Purchased.ToString() &&
                        (ii.IsTicketItem || ii.IsServiceChargeItem || ii.IsMerchandiseItem || ii.IsPromotionItem || ii.IsCharitableItem || ii.IsBundle)
                        && ii.LineItemTotal > 0)
                        sub += ii.LineItemTotal;
                }

                return decimal.Round(sub, 2);
            }
        }
        /// <summary>
        /// this will be a positive amount - subtract in appropriate total
        /// </summary>
        [XmlAttribute("PromotionalDiscountTotal")]
        public decimal PromotionalDiscountTotal
        {
            get
            {
                decimal sub = 0;

                foreach (InvoiceItem ii in this.PromotionItems)
                {
                    if (ii.PurchaseAction == _Enums.PurchaseActions.Purchased.ToString() &&
                        ii.Context == _Enums.InvoiceItemContext.discount && ii.LineItemTotal != 0)
                        sub += Utils.ParseHelper.AbsoluteValue(ii.LineItemTotal);
                }

                return sub;
            }
        }
        private decimal _processingCharge = 0;
        [XmlAttribute("ProcessingCharge")]
        public decimal ProcessingCharge
        {
            get
            {
                if (_processingCharge == 0)
                {
                    foreach (InvoiceItem ii in this.ProcessingItems)
                    {
                        if (ii.PurchaseAction == _Enums.PurchaseActions.Purchased.ToString())
                            _processingCharge += Utils.ParseHelper.AbsoluteValue(ii.LineItemTotal);
                    }
                }

                return decimal.Round(_processingCharge, 2);
            }
        }

        #endregion

        #region DB conversion

        //SAVE!!! this was needed to update db
        //public static void CreateInvoiceProducts()
        //{
            //int maxRows;// = 10000;
            ////get a list of invoices where vcproducts is null
            //InvoiceCollection orig = new InvoiceCollection().Where("vcProducts", null).Load();

            //InvoiceCollection coll = new InvoiceCollection();

            //if (orig.Count > 0)
            //{
            //   // if (orig.Count < maxRows)
            //        maxRows = orig.Count;

            //    for (int i = 0; i < maxRows; i++)
            //    {
            //        coll.Add(orig[i]);
            //    }

            //    orig.Clear();


            //    foreach (Invoice i in coll)
            //    {
            //        System.Text.StringBuilder sb = new System.Text.StringBuilder();

            //        foreach (InvoiceItem ii in i.InvoiceItemRecords())
            //        {
            //            sb.AppendFormat(FormatItemAsProductListing(ii));
            //        }

            //        i.VcProducts = sb.ToString();

            //        System.Text.StringBuilder query = new System.Text.StringBuilder();
            //        query.AppendFormat("update invoice set vcProducts='{0}' where id={1}", sb.ToString(), i.Id);

            //        Utils.DataHelper.ExecuteQuery(query, 
            //            System.Configuration.ConfigurationManager.ConnectionStrings["WillCallConnectionString"].ToString());
            //    }
            //}

        //}

        #endregion

    }
}
