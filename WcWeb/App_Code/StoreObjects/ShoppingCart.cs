using System;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Collections.Generic;

using Wcss;
using SubSonic;

namespace WillCallWeb.StoreObjects
{
    /// <summary>
    /// Summary description for ShoppingCart.
    /// </summary>	
    public class ShoppingCart
    {
        /// <summary>
        /// Note that a userName needs to be provided along with the profile as the profile can be annonymous in this call.
        /// Syncs the profile with actual store credit in the db. If they do not have enough to cover their cart credit - it resets the credit in the 
        /// cart to zero. Returns the result of having enough credit in the account. Callers can determine what to do based on this validation. Don't
        /// record the sync event here - we do not need the extra db call in the order flow
        /// </summary>
        /// <param name="profile"></param>
        /// <returns></returns>
        public bool ValidateStoreCredit(string userName, string creatorName)
        {            
            decimal currentStoreCreditInProfile = WillCallWeb.StoreObjects.SaleItem_StoreCredit.Profile_StoreCredit_Sync(userName, creatorName, false);

            //if the current amount of store credit in the users account is less than the amount they intend to apply to the invoice            
            if(currentStoreCreditInProfile < this.StoreCreditToApply)
            {
                //then reset store credit
                this.StoreCredit.Price = 0;
                return false;
            }

            return true;
        }

        public string FormatMaxTransactionError()
        {
            return string.Format("<div class=\"emptycart\">Your cart has exceeded the maximum amount ({0}) allowed per transaction! Please remove some items from your cart{1}.</div>",
                Wcss._Config._AuthorizeNetLimit.ToString("c"), (this.ShippingAndHandling > 0) ? " or choose a different shipping option" : string.Empty);
        }

        #region Product Listing

        public string ItemList_Delimited
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                //tickets
                List<SaleItem_Ticket> tcoll = new List<SaleItem_Ticket>();
                tcoll.AddRange(this.TicketItems);
                foreach (SaleItem_Ticket itm in tcoll)
                {
                    sb.AppendFormat("{0} Qty: {1}~", itm.Ticket.DisplayNameWithAttribsAndDescription, itm.Quantity);

                    foreach (MerchBundle_Listing listing in itm.MerchBundleSelections)
                    {
                        if (listing.Quantity > 0)
                            sb.AppendFormat("BUNDLE {0}: {1} Qty: {2}~",
                                listing.BundleId.ToString(),
                                listing.SelectedInventory.DisplayNameWithAttribs,
                                listing.Quantity.ToString());
                    }
                }

                //ticketshipping
                List<SaleItem_Shipping> tscoll = new List<SaleItem_Shipping>();
                tscoll.AddRange(this.Shipments_Tickets);
                foreach (SaleItem_Shipping itm in tscoll)
                    sb.AppendFormat("TicktShip: {0} {1} (esd: {2})~", itm.ShipMethod, itm.ShipCost.ToString("c"), itm.ShipDate.ToString("M-d-yyyy"));
                
                //merch
                List<SaleItem_Merchandise> mcoll = new List<SaleItem_Merchandise>();
                mcoll.AddRange(this.MerchandiseItems);
                foreach (SaleItem_Merchandise itm in mcoll)
                {
                    sb.AppendFormat("{0} Qty: {1}~", itm.MerchItem.DisplayNameWithAttribs, itm.Quantity);

                    foreach (MerchBundle_Listing listing in itm.MerchBundleSelections)
                    {
                        if(listing.Quantity > 0)
                            sb.AppendFormat("BUNDLE {0}: {1} Qty: {2}~",
                                listing.BundleId.ToString(),
                                listing.SelectedInventory.DisplayNameWithAttribs,
                                listing.Quantity.ToString());
                    }
                }

                //merch shipping
                List<SaleItem_Shipping> smcoll = new List<SaleItem_Shipping>();
                smcoll.AddRange(this.Shipments_Merch);
                foreach (SaleItem_Shipping itm in smcoll)
                    sb.AppendFormat("MerchShip: {0} {1} (esd: {2})~", itm.ShipMethod, itm.ShipCost.ToString("c"), itm.ShipDate.ToString("M-d-yyyy"));
                
                //promotions
                List<SaleItem_Promotion> pcoll = new List<SaleItem_Promotion>();
                pcoll.AddRange(this.PromotionItems);
                foreach (SaleItem_Promotion itm in pcoll)
                    if(itm.SalePromotion != null)
                        sb.AppendFormat("{0} Qty: {1}~", itm.SalePromotion.DisplayNameWithAttribs, itm.Quantity);
                
                //donations
                if(this.CharityAmount > 0)
                    sb.AppendFormat("{0} Donation to: {1}~", this.CharityAmount.ToString("c"), this.CharityOrg.Name_Displayable.Trim());

                return sb.ToString();
            }
        }
        public static string FormatItemProductListing(_Enums.InvoiceItemContext context, int productId, int qty)
        {
            _Enums.ItemContextCode code = _Enums.ItemContextCode.o;

            if (context == _Enums.InvoiceItemContext.merch)
            {
                code = _Enums.ItemContextCode.m;
            }
            else if (context == _Enums.InvoiceItemContext.bundle)
            {
                code = _Enums.ItemContextCode.b;
            }
            else if (context == _Enums.InvoiceItemContext.charity)
            {
                code = _Enums.ItemContextCode.y;
            }

            if (productId > 0)
                return string.Format("{0},{1},{2}~", code.ToString(), qty, productId);

            return string.Empty;
        }
        public static string FormatItemProductListing(object saleItem)
        {
            int productId = 0;
            int qty = 0;
            _Enums.ItemContextCode code = _Enums.ItemContextCode.o;
            //decide context
            string typeName = saleItem.GetType().Name.ToLower();

            //gift certificates are included here
            switch (typeName)
            {
                case "saleitem_merchandise":
                    productId = ((SaleItem_Merchandise)saleItem).tMerchId;
                    qty = ((SaleItem_Merchandise)saleItem).Quantity;
                    code = _Enums.ItemContextCode.m;
                    break;
                case "saleitem_ticket":
                    productId = ((SaleItem_Ticket)saleItem).tShowTicketId;
                    qty = ((SaleItem_Ticket)saleItem).Quantity;
                    code = _Enums.ItemContextCode.t;
                    break;
                case "saleitem_promotion":
                    productId = ((SaleItem_Promotion)saleItem).tSalePromotionId;
                    qty = ((SaleItem_Promotion)saleItem).Quantity;
                    code = _Enums.ItemContextCode.f;
                    break;
            }

            //ignore shipping items
            if (productId > 0)
                return string.Format("{0},{1},{2}~", code.ToString(), qty, productId);

            return string.Empty;
        }
        #endregion

        #region Shipments
        public List<SaleItem_Shipping> Shipments_All
        {
            get
            {
                List<SaleItem_Shipping> list = new List<SaleItem_Shipping>();
                list.AddRange(Shipments_Merch);
                list.AddRange(Shipments_Tickets);

                return list;
            }
        }
        public SaleItem_Shipping Shipment_Tickets_Main
        {
            get
            {
                return this.Shipments_Tickets.Find(delegate (SaleItem_Shipping match) { return (match.IsGeneral); } );
            }
        }
        public List<SaleItem_Shipping> Shipment_Tickets_Separate
        {
            get
            {
                return this.Shipments_Tickets.FindAll(delegate(SaleItem_Shipping match) { return (match.IsShipSeparate); });
            }
        }
        public List<SaleItem_Shipping> _shipments_Tickets;
        public List<SaleItem_Shipping> Shipments_Tickets
        {
            get 
            {
                List<SaleItem_Ticket> shipTickets = new List<SaleItem_Ticket>();
                shipTickets.AddRange(this.TicketItems_CurrentlyShippable);

                //create shipments - fill lists
                if (_shipments_Tickets == null || _shipments_Tickets.Count == 0 && shipTickets.Count > 0)
                {
                    List<SaleItem_Ticket> standard = new List<SaleItem_Ticket>();
                    List<SaleItem_Ticket> separate = new List<SaleItem_Ticket>();

                    //add tickets - we have already determined they are shippable
                    standard.AddRange(shipTickets.FindAll(delegate(SaleItem_Ticket match)
                    {
                        return (!match.Ticket.IsShipSeparate);
                    }));                    

                    //separate 
                    separate.AddRange(shipTickets.FindAll(delegate(SaleItem_Ticket match)
                    {
                        return (match.Ticket.IsShipSeparate);
                    }));

                    if (standard.Count > 0)
                    {
                        SaleItem_Shipping shipment = new SaleItem_Shipping(standard);
                        _shipments_Tickets.Add(shipment);
                    }

                    if (separate.Count > 0)
                    {
                        foreach (SaleItem_Ticket sit in separate)
                            _shipments_Tickets.Add(new SaleItem_Shipping(sit));
                    }
                }
                
                return _shipments_Tickets; 
            }
            set { _shipments_Tickets = value; }
        }
        public List<SaleItem_Shipping> _shipments_Merch;
        public List<SaleItem_Shipping> Shipments_Merch
        {
            get
            {
                List<SaleItem_Merchandise> parcels = new List<SaleItem_Merchandise>();
                parcels.AddRange(this.MerchandiseItems.FindAll(delegate(SaleItem_Merchandise match) { return (match.HasParcelDelivery); }));

                //create shipments - fill lists
                if (_shipments_Merch.Count == 0 && parcels.Count > 0)
                {
                    //this shipment may include backordered items - if cart shipmultmerch is set
                    List<SaleItem_Merchandise> standard = new List<SaleItem_Merchandise>();
                    List<SaleItem_Merchandise> backorder = new List<SaleItem_Merchandise>();
                    List<DateTime> backDates = new List<DateTime>();
                    List<SaleItem_Merchandise> separate = new List<SaleItem_Merchandise>();

                    standard.AddRange(parcels.FindAll(delegate(SaleItem_Merchandise match)
                    {
                        return ((!match.MerchItem.IsFlatShip) && 
                            (!match.MerchItem.IsShipSeparate) && ((!match.MerchItem.IsBackordered) ||
                          (match.MerchItem.IsBackordered && (!this.IsShipMultiple_Merch))));
                    }));

                    backorder.AddRange(parcels.FindAll(delegate(SaleItem_Merchandise match)
                    {
                        return ((!match.MerchItem.IsFlatShip) && (!match.MerchItem.IsShipSeparate) && ((match.MerchItem.IsBackordered && this.IsShipMultiple_Merch)));
                    }));

                    foreach (SaleItem_Merchandise sim in backorder)
                    {
                        DateTime calcd = Wcss._Shipper.CalculateShipDate(sim.MerchItem.BackorderDate);
                        if (!backDates.Contains(calcd))
                            backDates.Add(calcd);
                    }

                    //separate gets both flat and separate
                    separate.AddRange(parcels.FindAll(delegate(SaleItem_Merchandise match)
                    {
                        return (match.MerchItem.IsFlatShip || match.MerchItem.IsShipSeparate);
                    }));

                    if (standard.Count > 0)
                    {
                        SaleItem_Shipping shipment = new SaleItem_Shipping(standard);
                        _shipments_Merch.Add(shipment);
                    }

                    foreach (DateTime dt in backDates)
                    {
                        List<SaleItem_Merchandise> backShipItems = new List<SaleItem_Merchandise>();
                        backShipItems.AddRange(backorder.FindAll(delegate(SaleItem_Merchandise match) { return (Wcss._Shipper.CalculateShipDate(match.MerchItem.BackorderDate) == dt); }));

                        _shipments_Merch.Add(new SaleItem_Shipping(dt, backShipItems));
                    }

                    if (separate.Count > 0)
                    {
                        foreach (SaleItem_Merchandise sim in separate)
                            _shipments_Merch.Add(new SaleItem_Shipping(sim));

                    }
                }

                return _shipments_Merch;
            }
            set { _shipments_Merch = value; }

        }
        public bool HasBackorderedMerch
        {
            get
            {
                List<SaleItem_Merchandise> coll = new List<SaleItem_Merchandise>();
                coll.AddRange(this.MerchandiseItems.FindAll(delegate(SaleItem_Merchandise match) { return (match.HasParcelDelivery); }));

                foreach (SaleItem_Merchandise sim in coll)
                    if ((!sim.MerchItem.IsFlatShip) && (!sim.MerchItem.IsShipSeparate) && sim.MerchItem.IsBackordered)
                        return true;

                return false;
            }
        }
        public bool HasFlatShipMerch
        {
            get
            {
                List<SaleItem_Merchandise> coll = new List<SaleItem_Merchandise>();
                coll.AddRange(this.MerchandiseItems.FindAll(delegate(SaleItem_Merchandise match) { return (match.HasParcelDelivery); }));

                foreach (SaleItem_Merchandise sim in coll)
                    if (sim.MerchItem.IsFlatShip)
                        return true;

                return false;
            }
        }
        public bool HasShipSeparateMerch
        {
            get
            {
                List<SaleItem_Merchandise> coll = new List<SaleItem_Merchandise>();
                coll.AddRange(this.MerchandiseItems.FindAll(delegate(SaleItem_Merchandise match) { return (match.HasParcelDelivery); }));

                foreach (SaleItem_Merchandise sim in coll)
                    if (sim.MerchItem.IsShipSeparate)
                        return true;

                return false;
            }
        }
        private bool _shipMultipleMerch = false;
        public bool IsShipMultiple_Merch { get { return _shipMultipleMerch; } set { _shipMultipleMerch = value; } }
        #endregion

        #region Checkout SQL

        public int CreateInvoiceForOrderFlow(string invoiceKey, string aspnetUserId, int customerId, int cashewId, 
            string creator, string purchaseEmail)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            QueryCommand cmd = new QueryCommand(string.Empty, SubSonic.DataService.Provider.Name);

            //invoiceid must be first to later reference its value
            cmd.AddParameter("@vendorId", ((Vendor)_Lookits.Vendors.GetVendor_Online()).Id, DbType.Int32);
            cmd.AddParameter("@invoiceId", Ctx.InvoiceId, DbType.Int32);
            cmd.AddParameter("@invoiceKey", invoiceKey, DbType.String);
            cmd.AddParameter("@purchaseEmail", purchaseEmail, DbType.String);
            cmd.AddParameter("@aspnetUserId", aspnetUserId, DbType.String);
            cmd.AddParameter("@customerId", customerId, DbType.Int32);
            cmd.AddParameter("@appId", _Config.APPLICATION_ID, DbType.Guid);
            cmd.AddParameter("@creator", creator, DbType.String);
            cmd.AddParameter("@amountDue", Ctx.Cart.ChargeTotal, DbType.Decimal);
            cmd.AddParameter("@cashewId", cashewId, DbType.Int32);
            cmd.AddParameter("@mrktKey", Ctx.MarketingProgramKey, DbType.String);            
            cmd.AddParameter("@sessionId", System.Web.HttpContext.Current.Session.SessionID, DbType.String);
            
            sb.Append("BEGIN TRANSACTION ");

            if (Ctx.InvoiceId == 0)//if we dont have a current invoice
            {
                //*****************************************
                // INVOICE
                //*****************************************
                sb.Append("INSERT INTO Invoice([ApplicationId], [TVendorId],[UniqueId],[PurchaseEmail],[UserId],[CustomerId],[dtInvoiceDate],");
                sb.Append("[Creator],[OrderType],[mBalanceDue],[InvoiceStatus],[TCashewId],[MarketingKeys]) ");
                sb.Append("VALUES (@appId, @vendorId, @invoiceKey, @purchaseEmail, @aspnetUserId, @customerId, getDate(), ");
                sb.Append("@creator, 'Purchase', @amountDue, 'NotPaid', @cashewId, @mrktKey) ");
                sb.Append("SET @invoiceId = SCOPE_IDENTITY() ");
            }
            else//we only need to update the invoice - cartitems are the same - most likely a failed trans
            {
                // Ensure that invoice has latest total!!
                sb.Append("UPDATE Invoice SET [TCashewId] = @cashewId, [mBalanceDue] = @amountDue WHERE [Id] = @invoiceId ");
            }

            sb.Append("UPDATE TicketStock SET [TInvoiceId] = @invoiceId, [UserName] = @purchaseemail WHERE [SessionId] = @sessionId AND [TInvoiceId] IS NULL ");

            sb.Append("COMMIT TRANSACTION ");

            //this is all we want back
            sb.Append("SELECT @invoiceId RETURN ");

            cmd.CommandSql = sb.ToString();

            try
            {
                int returnVal = (int)DataService.ExecuteScalar(cmd);

                return returnVal;
            }
            catch (System.Data.SqlClient.SqlException sex)
            {
                _Error.LogException(sex);
                throw new Exception(string.Format("CheckoutInvoiceAndItems Sql Error.\r\n{0}\r\n{1}", sex.Message, sex.StackTrace));
            }
            catch (Exception ex)
            {
                _Error.LogException(ex);
                throw new Exception(string.Format("CheckoutInvoiceAndItems Error.\r\n{0}\r\n{1}", ex.Message, ex.StackTrace));
            }
        }                        

        #endregion

        #region EventDelegation

        public delegate void CartChangedEvent(object sender, EventArgs e);
        public event CartChangedEvent CartChanged;
        public void OnCartChanged()
        {
            //reset
            if (this.MerchandiseItems.Count > 0 && this.IsShipMultiple_Merch)
                this.IsShipMultiple_Merch = false;

            _processingFee = null;

            if (CartChanged != null) { CartChanged(this, EventArgs.Empty); }
        }
        public class PromoMessageEventArgs : EventArgs
        {
            protected int _idx;
            protected string _reason;

            //Default Constructor
            public PromoMessageEventArgs()
            {
                _idx = 0;
                _reason = string.Empty;
            }

            //Alt Constructor
            public PromoMessageEventArgs(int promoIdx, string reason)
            {
                _idx = promoIdx;
                _reason = reason;
            }

            public int ExpiredId { get { return _idx; } }
            public string Reason { get { return _reason; } }
        }
        public delegate void PromoMessageEvent(object sender, PromoMessageEventArgs e);
        public event PromoMessageEvent PromoMessage;
        public void OnPromoMessage(int salePromotionId, string reason)
        {
            //show that promotion has expired
            if (PromoMessage != null) { PromoMessage(this, new PromoMessageEventArgs(salePromotionId, reason)); }
            else Ctx.CurrentCartException = reason;
        }
        #endregion

        #region Constructors

        [NonSerialized]
        private WebContext _ctx;
        private WebContext Ctx { get { return _ctx; } set { _ctx = value; } }
        
        public ShoppingCart()
        {
            Ctx = new WebContext();

            TicketItems = new List<SaleItem_Ticket>();
            MerchandiseItems = new List<SaleItem_Merchandise>();
            PromotionItems = new List<SaleItem_Promotion>();
            Shipments_Merch = new List<SaleItem_Shipping>();
            Shipments_Tickets = new List<SaleItem_Shipping>();
        }

        #endregion

        #region Properties

        public List<SaleItem_Ticket> TicketItems;
        public List<SaleItem_Merchandise> MerchandiseItems;

        private void AddItemsToParcelList(List<MerchWithQuantity> list, SaleItem_Base saleItem)
        {
            if(saleItem is SaleItem_Merchandise)
                if(((SaleItem_Merchandise)saleItem).MerchItem.IsParcelDelivery)
                    list.Add(new MerchWithQuantity(saleItem.Quantity, ((SaleItem_Merchandise)saleItem).MerchItem));

            //only add if we have selections for this particular item
            List<MerchBundle_Listing> selections = new List<MerchBundle_Listing>();

            selections.AddRange(saleItem.MerchBundleSelections
                .FindAll(delegate(MerchBundle_Listing match) { return (!match.IsOptOut && match.SelectedInventory.IsParcelDelivery); }));

            foreach(MerchBundle_Listing listing in selections)
                list.Add(new MerchWithQuantity(listing.Quantity, listing.SelectedInventory));

        }
        public List<MerchWithQuantity> ParcelItemsInOrder
        { 
            get 
            {
                List<MerchWithQuantity> list = new List<MerchWithQuantity>();

                foreach(SaleItem_Merchandise sim in this.MerchandiseItems)
                {
                    AddItemsToParcelList(list, sim);
                }

                //TODO: allow shippable items for tickets
                //foreach(SaleItem_Ticket sit in this.TicketItems)
                //{
                //    //do not add the ticket!
                //    //add any relevant bundled items
                //    AddItemsToParcelList(list, sit);
                //}

                return list;
            } 
        }

        public List<SaleItem_Merchandise> GiftCertificateItems { get { return this.MerchandiseItems.FindAll(delegate(SaleItem_Merchandise match) { return (match.MerchItem.DeliveryType == _Enums.DeliveryType.giftcertificate); }); } }        
        //public List<SaleItem_Merchandise> DownloadItems { get { return this.MerchandiseItems.FindAll(delegate(SaleItem_Merchandise match) { return (match.MerchItem.DeliveryType == _Enums.DeliveryType.download); }); } }
        //public List<SaleItem_Merchandise> EmailableItems { get { return this.MerchandiseItems.FindAll(delegate(SaleItem_Merchandise match) { return (match.MerchItem.DeliveryType == _Enums.DeliveryType.email); }); } }

        public decimal GiftCertificateTotal
        {
            get
            {
                decimal d = 0;

                List<SaleItem_Merchandise> coll = new List<SaleItem_Merchandise>();
                coll.AddRange(this.GiftCertificateItems);

                foreach (SaleItem_Merchandise sim in coll)
                    d += sim.LineTotal;

                return d;
            }
        }
        public List<SaleItem_Promotion> PromotionItems;
        
        public List<SaleItem_Ticket> TicketItems_CurrentlyShippable { get { return TicketItems.FindAll(
            delegate(SaleItem_Ticket match) { return (match.Ticket.IsCurrentlyShippable); }); } }
        public List<SaleItem_Ticket> TicketItems_ShipsSeparately
        {
            get
            {
                return TicketItems.FindAll(
                    delegate(SaleItem_Ticket match) { return (match.Ticket.IsShipSeparate); });
            }
        }
        public List<SaleItem_Ticket> TicketItems_ShipsInGeneralBatch
        {
            get
            {
                return TicketItems.FindAll(
                    delegate(SaleItem_Ticket match) { return (!match.Ticket.IsShipSeparate); });
            }
        }


        /// <summary>
        /// only applies to tickets that do not ship separately
        /// </summary>
        public List<SaleItem_Ticket> TicketItems_NotAvailableForWillCall//these items require shipping
        {
            get
            {
                return TicketItems_ShipsInGeneralBatch.FindAll(
                    delegate(SaleItem_Ticket match) { return (match.Ticket.IsAllowWillCall == false); });
            }
        }



        public bool HasTicketItems { get { return (this.TicketItems != null && this.TicketItems.Count > 0); } }
        public bool HasTicketItems_CurrentlyShippable { get { return (this.TicketItems_CurrentlyShippable != null && this.TicketItems_CurrentlyShippable.Count > 0); } }
        public bool HasTicketItems_NotAvailableForWillCall { get { return (this.TicketItems_NotAvailableForWillCall != null && this.TicketItems_NotAvailableForWillCall.Count > 0); } }
        public bool HasMerchandiseItems_Shippable 
        { 
            get 
            {
                return (this.ParcelItemsInOrder.Count > 0); 
            } 
        }
        public bool HasTicketItems_AllHiddenShipMethod
        {
            get
            {
                return (_Config._Allow_HideShipMethod && (this.TicketItems.Count == 
                    this.TicketItems.FindAll(delegate(SaleItem_Ticket match) { return (match.Ticket.HideShipMethod); }).Count));
            }
        }
        public bool HasMerchandiseItems { get { return (this.MerchandiseItems != null && this.MerchandiseItems.Count > 0); } }
        public bool HasPromotionItems { get { return (this.PromotionItems != null && this.PromotionItems.Count > 0); } }
        
        
        public bool RequiresTicketShippingMethod { get { return (this.TicketItems_ShipsSeparately.Count > 0) || 
            (this.TicketItems_NotAvailableForWillCall != null && this.TicketItems_NotAvailableForWillCall.Count > 0); } }

        /// <summary>
        /// Does not include promotion items
        /// </summary>
        public int ItemCount { get { return this.TicketItems.Count + this.MerchandiseItems.Count; } }
        /// <summary>
        /// Does not include promotion items
        /// </summary>
        public bool HasItems { get { return (ItemCount > 0); } }

        public bool HasGiftCertificateItems { get { return 
            (this.MerchandiseItems.FindAll(delegate(SaleItem_Merchandise match) { return (match.MerchItem.IsGiftCertificateDelivery); } ).Count > 0); } }
        /// <summary>
        /// Does not include promotion items
        /// </summary>
        public bool IsEmpty { get { return (ItemCount == 0); } }

        private string _purchaseName = string.Empty;
        public string PurchaseName { get { return _purchaseName; } set { _purchaseName = value; } }

        public decimal Shipping_Tickets
        {
            get
            {
                decimal total = 0;

                List<SaleItem_Shipping> coll = new List<SaleItem_Shipping>();
                coll.AddRange(this.Shipments_Tickets);

                foreach (SaleItem_Shipping ship in coll)
                    total += ship.ShipCost;

                return total;
            }
        }
        public decimal Shipping_Merch
        {
            get
            {
                decimal total = 0;

                List<SaleItem_Shipping> coll = new List<SaleItem_Shipping>();
                coll.AddRange(this.Shipments_Merch);

                foreach (SaleItem_Shipping ship in coll)
                    total += ship.ShipCost;

                return total;
            }
        }
        /// <summary>
        /// The total of ticket and merchandise shipping
        /// </summary>
        public decimal ShippingAndHandling { get { return Shipping_Tickets + Shipping_Merch; } }

        /// <summary>
        /// Requirements are done as OR not AND
        /// </summary>
        /// <param name="ent"></param>
        /// <returns></returns>
        private bool ProcessingFeeRequirementsMet(InvoiceFee ent)
        {
            bool met = false;

            foreach (RequiredInvoiceFee requirement in ent.RequiredInvoiceFeeRecords())
            {
                //bool alsoMet = true;

                Required req = requirement.RequiredRecord;

                if (req.IsCurrentlyRunning(string.Empty))
                {   
                    //needs amount
                    if(req.Requires_MinPurchase)
                    {
                        if((!(req.MinimumMerchandisePurchase > 0 && this.PreFeeMerchTotal >= req.MinimumMerchandisePurchase)) &&
                            (!(req.MinimumTicketPurchase > 0 && this.PreFeeTicketTotal >= req.MinimumTicketPurchase)) &&
                            (!(req.MinimumMerchandisePurchase > 0 && this.PreFeeTotal >= req.MinimumTotalPurchase)))
                            return false;
                    }

                    //met = false;//reset mult reqs

                    //needs id
                    if(req.Requires_Merch)
                    {
                        List<SaleItem_Merchandise> coll = new List<SaleItem_Merchandise>();
                        coll.AddRange(this.MerchandiseItems);

                        foreach (SaleItem_Merchandise sim in coll)
                        {
                            //check children first - then parents
                            if (req.Id_IsInRequiredListing(sim.tMerchId.ToString(), sim.Quantity) ||
                                (sim.MerchItem.IsChild && req.Id_IsInRequiredListing(sim.MerchItem.TParentListing.Value.ToString(), sim.Quantity)))
                            {
                                //if the req is exclusive - we cannot have other items
                                if (req.IsExclusive)
                                {
                                    //cycle thru other merch items - and if they are not in the required list - then return false
                                    if (this.MerchandiseItems.Count > 1)
                                    {
                                        foreach (SaleItem_Merchandise sime in coll)
                                        {
                                            if (sime.tMerchId != sim.tMerchId)//only check other items
                                                if (!(req.Id_IsInRequiredListing(sime.tMerchId.ToString(), sime.Quantity) ||
                                                    (sime.MerchItem.IsChild && req.Id_IsInRequiredListing(sime.MerchItem.TParentListing.Value.ToString(), sime.Quantity))))
                                                    return false;
                                        }
                                    }

                                    if (this.TicketItems.Count > 0)
                                        return false;
                                }

                                met = true;
                                break;
                            }
                        }
                    }
                    //we group these because they are all derived from the ticket
                    else if(req.Requires_Ticket || req.Requires_ShowDate || req.Requires_Show)
                    {
                        List<SaleItem_Ticket> coll = new List<SaleItem_Ticket>();
                        coll.AddRange(this.TicketItems);

                        foreach (SaleItem_Ticket sit in coll)
                        {
                            //only compare base tickets
                            //if ((!sit.Ticket.IsPackage) || sit.Ticket.IsBaseOfPackage)
                            //{
                            switch (req.RequiredContext)
                            {
                                case _Enums.RequirementContext.show:
                                    if (req.Id_IsInRequiredListing(sit.Ticket.TShowId.ToString(), sit.Quantity))
                                    {
                                        //if the req is exclusive - we cannot have other items
                                        if (req.IsExclusive)
                                        {
                                            //cycle thru other ticket items - and if they are not in the required list - then return false
                                            if (coll.Count > 1)
                                            {
                                                foreach (SaleItem_Ticket site in coll)
                                                {
                                                    if (site.tShowTicketId != sit.tShowTicketId)//only check other items
                                                        if (!(req.Id_IsInRequiredListing(site.Ticket.TShowId.ToString(), site.Quantity)))
                                                            return false;
                                                }
                                            }

                                            if (this.MerchandiseItems.Count > 0)
                                                return false;
                                        }

                                        met = true;
                                    }
                                    break; 

                                case _Enums.RequirementContext.showdate:
                                    if (req.Id_IsInRequiredListing(sit.Ticket.TShowDateId.ToString(), sit.Quantity))
                                    {
                                        //if the req is exclusive - we cannot have other items
                                        if (req.IsExclusive)
                                        {
                                            //cycle thru other ticket items - and if they are not in the required list - then return false
                                            if (coll.Count > 1)
                                            {
                                                foreach (SaleItem_Ticket site in coll)
                                                {
                                                    if (site.tShowTicketId != sit.tShowTicketId)//only check other items
                                                        if (!(req.Id_IsInRequiredListing(site.Ticket.TShowDateId.ToString(), site.Quantity)))
                                                            return false;
                                                }
                                            }

                                            if (this.MerchandiseItems.Count > 0)
                                                return false;
                                        }

                                        met = true;
                                    }
                                    break;

                                case _Enums.RequirementContext.ticket:
                                    if (req.Id_IsInRequiredListing(sit.tShowTicketId.ToString(), sit.Quantity) ||
                                        (sit.Ticket.IsPackage && req.Id_IsInRequiredListing(sit.Ticket.PackageBase.Id.ToString(), sit.Quantity)))
                                    {
                                        //if the req is exclusive - we cannot have other items
                                        if (req.IsExclusive)
                                        {
                                            //cycle thru other ticket items - and if they are not in the required list - then return false
                                            if (coll.Count > 1)
                                            {
                                                foreach (SaleItem_Ticket site in coll)
                                                {
                                                    if (site.tShowTicketId != sit.tShowTicketId)//only check other items
                                                        if (!(req.Id_IsInRequiredListing(site.tShowTicketId.ToString(), site.Quantity) ||
                                                            (site.Ticket.IsPackage && req.Id_IsInRequiredListing(site.Ticket.PackageBase.Id.ToString(), site.Quantity))))
                                                            return false;
                                                }
                                            }

                                            if (this.MerchandiseItems.Count > 0)
                                                return false;
                                        }

                                        met = true;
                                    }
                                    break;
                            }
                        }
                    }
                    else if (req.Requires_MerchShipping)
                    {
                        //loop thru ship methods
                        List<SaleItem_Shipping> coll = new List<SaleItem_Shipping>();
                        coll.AddRange(this.Shipments_Merch);

                        foreach(SaleItem_Shipping sis in coll)
                            if (req.Id_IsInRequiredListing(sis.ShipMethod, 1))
                                met = true;
                    }
                    else if (req.Requires_TicketShipping)
                    {
                        //loop thru ship methods
                        List<SaleItem_Shipping> coll = new List<SaleItem_Shipping>();
                        coll.AddRange(this.Shipments_Tickets);

                        foreach(SaleItem_Shipping sis in coll)
                            if (req.Id_IsInRequiredListing(sis.ShipMethod, 1))
                                met = true;
                    }
                }

                if(met)
                    return met;
            }

            return met;
        }

        private bool CartIsGiftCertificatesOnly
        {
            get
            {
                //find merch items that re not gift certificate delivery
                int idx = -1;
                List<SaleItem_Merchandise> coll = new List<SaleItem_Merchandise>();
                coll.AddRange(this.MerchandiseItems);

                if(coll.Count > 0)
                    idx = coll.FindIndex(delegate(SaleItem_Merchandise match) { return (!match.MerchItem.IsGiftCertificateDelivery); });

                return (idx == -1 && this.TicketItems.Count == 0);
            }
        }
        //processing fees are overrideable!!!!
        private InvoiceFee _processingFee;
        public InvoiceFee ProcessingFee
        {
            get
            {
                //if all they have in the cart is gift certificates than processing fee should not be charged
                if (Wcss._Config._WaiveProcFeeOnGCs && CartIsGiftCertificatesOnly)
                {
                    _processingFee = null;
                    return _processingFee;
                }

                if(_processingFee == null)
                {
                    //see if there are overrideable invoice fees currentlyRunning                    
                    _processingFee = (Wcss.InvoiceFee)_Lookits.InvoiceFees.GetList().Find(
                        delegate(InvoiceFee match) { return (match.IsOverride && match.IsActive); });
                    
                    if (_processingFee != null)
                    {
                        //if the requirements are met - use that processing fee
                        if(!ProcessingFeeRequirementsMet(_processingFee))
                            _processingFee = null;
                    }

                    if (_processingFee == null)//otherwise
                        _processingFee = (Wcss.InvoiceFee)_Lookits.InvoiceFees.GetList().Find(
                            delegate(InvoiceFee match) { return ((!match.IsOverride) && match.IsActive); }
                            ); 
                }
                return _processingFee;
            }
            set { _processingFee = value; } 
        }

        private decimal _charityAmount = 0;
        public decimal CharityAmount
        {
            get
            {
                if (_charityAmount > 0 && this.PreFeeTotal == 0)
                    CharityAmount = 0;

                return _charityAmount;
            }
            set
            {
                _charityAmount = value;
                if (_charityAmount == 0)
                    CharityOrg = null;
            }
        }
       
        private CharitableOrg _charityOrg = null;
        public CharitableOrg CharityOrg { get { return _charityOrg; } set { _charityOrg = value; } }
        #endregion

        #region Cart CRUD

        public string AddMerchFromAttribs(int parentId, string style, string color, string size, int qty, ProfileCommon profile)
        {
            Merch addMerch = null;

            Merch parentMerch = (Merch)Ctx.SaleMerch.Find(parentId);
            if (parentMerch == null)
                throw new Exception("Parent Merch could not be found");

            //only allow one gift certificate at a time to be added
            if (parentMerch.IsGiftCertificateDelivery && Ctx.Cart.GiftCertificateItems.Count > 0)
                return "We&#39;re sorry, only one gift certificate amount may be added to your cart per order. However, you may purchase multiple items of that selection.";

            addMerch = parentMerch.FindChildItem(style, color, size);

            //determine if we have this in our cart
            WillCallWeb.StoreObjects.SaleItem_Merchandise cartItem = Ctx.Cart.FindSaleItem_MerchandiseById(addMerch.Id);
            if (cartItem != null)
                return string.Empty;

            return SaleItem_AddUpdate(_Enums.InvoiceItemContext.merch, addMerch.Id, qty, profile);
        }

        public string SaleItem_Remove(_Enums.InvoiceItemContext context, int idx)
        {
            return SaleItem_AddUpdate(context, idx, 0, null);
        }
        public string SaleItem_AddUpdate(_Enums.InvoiceItemContext context, int idx, int qty, ProfileCommon profile)
        {
            SaleItem_Base itm = null;

            string userName = (profile != null && (! profile.IsAnonymous)) ? profile.UserName : null;

            if (context == _Enums.InvoiceItemContext.ticket)
            {
                //attempt to find existing
                itm = this.FindSaleItem_TicketById(idx);

                if (itm == null)//we are adding a new item
                    itm = new SaleItem_Ticket(Ctx, idx, 0);

                #region Requirements met

                //only worry about all of this if we are adding tix
                if (qty > 0)
                {
                    Utils.Helper.Quartet<bool, int, int, string> retQt = RequiredShowTicketPastPurchase.ScanPurchaseRequirements(userName, ((SaleItem_Ticket)itm).Ticket);
                    bool requirementWasMet = retQt.First;
                    int currentTicketPurchaseQty = int.Parse(retQt.Second.ToString());

                    //if we cannot purchase for some reason - error - reason given for not allowing
                    if (!requirementWasMet)
                    {
                        return retQt.Fourth.ToString();//pass along delimiter?
                    }
                    //else if we need to look at the quantities of past and current purchases to determine allowability
                    // -1 indicates that we need not bother looking at current Qty or past qty
                    else if (currentTicketPurchaseQty > -1)
                    {
                        //2 things to do here
                        //do we allow unlimited purchases or limit
                        //if we limit to past purchases
                        ShowTicket st = ((SaleItem_Ticket)itm).Ticket;
                        int hasLimit = st.RequiredShowTicketPastPurchaseRecords().GetList()
                            .FindIndex(delegate(RequiredShowTicketPastPurchase match) { return match.LimitQtyToPastQty == true; });

                        //limit purchases to past qty
                        if (hasLimit != -1)
                        {
                            //if the currentqty that is being asked to change to,
                            //if this qty is greater than what has been purchased + what they are trying to purchase
                            //indicate that they may only buy so many
                            int maxAllowedPurchases = int.Parse(retQt.Third.ToString());

                            if (maxAllowedPurchases - currentTicketPurchaseQty <= 0)
                                return string.Format("You have already purchased {0} tickets and have used all of your allowed purchases.",
                                    currentTicketPurchaseQty.ToString());
                            else if (maxAllowedPurchases - currentTicketPurchaseQty - qty < 0)//qty is qty in cart
                            {
                                int purchasesLeft = maxAllowedPurchases - currentTicketPurchaseQty;
                                return string.Format("You have already purchased {0} tickets and may only purchase {1} more ticket{2}.",
                                    currentTicketPurchaseQty.ToString(), purchasesLeft.ToString(), (purchasesLeft > 1) ? "s" : string.Empty);
                            }
                        }

                        //otherwise ignore any limits
                    }
                }

                #endregion
            }
            else if (context == _Enums.InvoiceItemContext.merch)
            {
                //attempt to find existing
                itm = this.FindSaleItem_MerchandiseById(idx);

                if (itm == null)//we are adding a new item
                    itm = new SaleItem_Merchandise(Ctx, idx, 0);

                #region Requirements met

                //only worry about all of this if we are adding
                if (qty > 0)
                {
                    Utils.Helper.Quartet<bool, int, int, string> retQt = RequiredMerch.ScanPurchaseRequirements(userName, ((SaleItem_Merchandise)itm).MerchItem.ParentMerchRecord);
                    bool requirementWasMet = retQt.First;
                    int currentMerchPurchaseQty = int.Parse(retQt.Second.ToString());

                    //if we cannot purchase for some reason - error - reason given for not allowing
                    if (!requirementWasMet)
                    {
                        return retQt.Fourth.ToString();//pass along delimiter?
                    }
                    //else if we need to look at the quantities of past and current purchases to determine allowability
                    // -1 indicates that we need not bother looking at current Qty or past qty
                    else if (currentMerchPurchaseQty > -1)
                    {
                        //2 things to do here
                        //do we allow unlimited purchases or limit
                        //if we limit to past purchases
                        Merch m = ((SaleItem_Merchandise)itm).MerchItem.ParentMerchRecord;
                        int hasLimit = m.RequiredMerchRecords().GetList()
                            .FindIndex(delegate(RequiredMerch match) { return match.LimitQtyToPastQty == true; });

                        //limit purchases to past qty
                        if (hasLimit != -1)
                        {
                            //if the currentqty that is being asked to change to,
                            //if this qty is greater than what has been purchased + what they are trying to purchase
                            //indicate that they may only buy so many
                            int maxAllowedPurchases = int.Parse(retQt.Third.ToString());

                            if (maxAllowedPurchases - currentMerchPurchaseQty <= 0)
                                return string.Format("You have already purchased {0} items and have used all of your allowed purchases.",
                                    currentMerchPurchaseQty.ToString());
                            else if (maxAllowedPurchases - currentMerchPurchaseQty - qty < 0)//qty is qty in cart
                            {
                                int purchasesLeft = maxAllowedPurchases - currentMerchPurchaseQty;
                                return string.Format("You have already purchased {0} items and may only purchase {1} more item{2}.",
                                    currentMerchPurchaseQty.ToString(), purchasesLeft.ToString(), (purchasesLeft > 1) ? "s" : string.Empty);
                            }
                        }

                        //otherwise ignore any limits
                    }
                }

                #endregion
            }

            if (itm != null)
                return Ctx.Cart.ManageInventory(itm, idx, qty);

            return string.Empty;
        }
        internal string ManageInventory(SaleItem_Base saleItem, int idx, int changeToQty)
        {
            DataTable dt;
            DataSet ds;
            string result;
            string explanation = string.Empty;
            string userName = (System.Web.HttpContext.Current.User.Identity.IsAuthenticated) ? System.Web.HttpContext.Current.User.Identity.Name : string.Empty;
            _Enums.InvoiceItemContext context = (saleItem.GetType() == typeof(SaleItem_Ticket)) ? 
                _Enums.InvoiceItemContext.ticket : _Enums.InvoiceItemContext.merch;

            //for tickets, we use the amount that the cart item may be changed by
            //for merch, we use the cart item total
            int diffQty = (context == _Enums.InvoiceItemContext.ticket) ? changeToQty - saleItem.Quantity : 
                changeToQty;

            if (context == _Enums.InvoiceItemContext.ticket && diffQty == 0)
                return string.Empty;

            //removing merch items should always be successful
            if (context == _Enums.InvoiceItemContext.merch && diffQty == 0)
            {
                result = "SUCCESS";
            }
            else
            {
                ds = SPs.TxInventoryAddUpdate(saleItem.GUID, System.Web.HttpContext.Current.Session.SessionID,
                        userName, idx, diffQty, saleItem.TTL, context.ToString()).GetDataSet();

                dt = ds.Tables[0];
                result = dt.Rows[0].ItemArray[0].ToString();
            }

            if (result == "SUCCESS")
            {
                int removeNitems = (saleItem.Quantity > changeToQty) ? saleItem.Quantity - changeToQty : 0;

                saleItem.Quantity = changeToQty;

                if (context == _Enums.InvoiceItemContext.ticket)
                {
                    if (saleItem.Quantity <= 0)
                        TicketItems.Remove((SaleItem_Ticket)saleItem);
                    else if (!TicketIsInCollection(idx))
                        TicketItems.Add((SaleItem_Ticket)saleItem);
                }
                else if (context == _Enums.InvoiceItemContext.merch)
                {
                    if (saleItem.Quantity <= 0)
                        MerchandiseItems.Remove((SaleItem_Merchandise)saleItem);
                    else if (!MerchIsInCollection(idx))
                        MerchandiseItems.Add((SaleItem_Merchandise)saleItem);
                }                

                //handle removes
                if (saleItem.Quantity <= 0)
                {
                    saleItem = null;
                }
                //else if (removeNitems > 0)
                    

                OnCartChanged();

                return string.Empty;
            }
            else if (result.StartsWith("ERROR"))
            {
                //indicates that the item to be updated had an incorrect context/not handled, etc
                explanation = string.Format("We're sorry, your request could not be processed. Please try again or clear your cart.");
                _Error.LogException(new Exception(string.Format("{0} error in addupdate proc. IP: {1} User: {2}. \r\nSaleItem - id to add {3}, changeToQty {4}", 
                    DateTime.Now.ToLongTimeString(), System.Web.HttpContext.Current.Request.UserHostAddress, userName, idx, changeToQty)));
            }
            else//we were unable to process the request - not necessarily an error
            {                
                int availableResult = int.Parse(result);

                //if we are trying to remove items and the proc cannot process....
                //this indicates that the items were not in the pending stock table
                //so - remove the items
                //this happens when the items have expired - then the items are removed from the pending table by sql job - and 
                //then the user clicks the alert window re: items being removed from the cart

                //to recreate sequence of events
                //items expire - alert is shown (but not responded to) - job clears expired items - user hits alert(which tries to update cart)

                if (diffQty < 0)
                {
                    saleItem.Quantity -= Math.Abs(diffQty);
                    if (saleItem.Quantity <= 0)
                    {
                        if (context == _Enums.InvoiceItemContext.ticket)
                            this.TicketItems.Remove((SaleItem_Ticket)saleItem);
                        if (context == _Enums.InvoiceItemContext.merch)
                            this.MerchandiseItems.Remove((SaleItem_Merchandise)saleItem);

                        try
                        {
                            DateTime now = DateTime.Now;
                            EventQ.Insert(now, now, _Enums.EventQStatus.Success.ToString(), null, 0, 0, null, userName, null, userName,
                                _Enums.EventQContext.Report.ToString(), _Enums.EventQVerb.InventoryError.ToString(),
                                saleItem.Quantity.ToString(), changeToQty.ToString(), string.Format("Removing expired inventory {0} {1} and pending has been cleared: {2}",
                                context.ToString(), idx, availableResult),
                                System.Web.HttpContext.Current.Request.UserHostAddress, DateTime.Now, _Config.APPLICATION_ID);
                        }
                        catch (Exception ex)
                        {
                            _Error.LogException(ex);
                        }

                        explanation = string.Empty;

                        //handle removes
                        if (saleItem.Quantity <= 0)
                        {
                            saleItem = null;
                        }

                        OnCartChanged();
                    }
                    else
                    {
                        explanation = string.Format("We're sorry, your request could not be processed. Please try again or clear your cart.");

                        //TODO: log when this happens
                        //but we need to insure that if there are alot of these events in a row that it does not bog down the system

                        try
                        {
                            DateTime now = DateTime.Now;
                            EventQ.Insert(now, null, null, null, 0, 0, null, userName, null, userName,
                                _Enums.EventQContext.Report.ToString(), _Enums.EventQVerb.InventoryError.ToString(),
                                saleItem.Quantity.ToString(), changeToQty.ToString(), string.Format("Removing inventory {0} {1} and available is: {2}", context.ToString(), idx, availableResult),
                                System.Web.HttpContext.Current.Request.UserHostAddress, DateTime.Now, _Config.APPLICATION_ID);
                        }
                        catch (Exception ex)
                        {
                            _Error.LogException(ex);
                        }
                    }
                }
                else //display available inventory
                {
                    string type = string.Empty;
                    string info = string.Empty;

                    if (context == _Enums.InvoiceItemContext.ticket)
                    {
                        type = "tickets";
                        SaleItem_Ticket itm = (SaleItem_Ticket)saleItem;
                        //date,ages,mainact,criteria,salesdescription
                        info = System.Text.RegularExpressions.Regex.Replace(string.Format("{0} {1} {2} {3} {4}", itm.Ticket.DateOfShow.ToString("MM/dd/yyyy hh:mmtt"),
                             itm.Ticket.AgeRecord.Name, itm.Ticket.ShowDateRecord.ShowRecord.ShowNamePartCondense,
                             itm.Ticket.SalesDescription_Derived, 
                             itm.Ticket.CriteriaText_Derived), @"\s+", " ").Trim();

                        //clear out item if we were unable to add - existing items will remain unchanged
                        if (!TicketIsInCollection(idx))
                            saleItem = null;
                    }
                    else if (context == _Enums.InvoiceItemContext.merch)
                    {
                        type = "items";
                        info = ((SaleItem_Merchandise)saleItem).MerchItem.DisplayNameWithAttribs;

                        //clear out item if we were unable to add - existing items will remain unchanged
                        if (!MerchIsInCollection(idx))
                            saleItem = null;
                    }

                    //it is possible that the proc could have returned a diff amount due to concurrency, etc - see 2nd case
                    if (availableResult <= 0)
                        explanation = string.Format("Sorry, there are no more {0} available for {1} at this time.", type, info);
                    else if(availableResult >= diffQty)
                        explanation = "Your request could not be processed at this time, please try again.";
                    else
                        explanation = string.Format("Sorry, there {0} currently only {1} more available for {2}.",
                            (availableResult == 1) ? "is" : "are", availableResult, info);
                }
            }

            return explanation;
        }

        //Leave this here CleanupReservations -- to help insure that we dont erase the proc as it is not called by code but by sqlagent only right now
        public string ClearCart()
        {
            return ClearCart(false);
        }
        
        private void AddMerchBundleGuidsMaster(List<System.Web.UI.WebControls.ListItem> merchGuids)
        {
            //loop again for bundle items
            foreach (SaleItem_Base sim in this.MerchandiseItems)
                AddMerchBundleGuids(sim, merchGuids);

            foreach (SaleItem_Base sim in this.TicketItems)
                AddMerchBundleGuids(sim, merchGuids);
        }
        /// <summary>
        /// Bundles do not have quantities in the traditional sense - so no need to record here. Just record the item quantities
        /// </summary>
        /// <param name="saleItem"></param>
        /// <param name="merchGuids"></param>
        private void AddMerchBundleGuids(SaleItem_Base saleItem, List<System.Web.UI.WebControls.ListItem> merchGuids)
        {   
            if (saleItem.MerchBundleSelections != null)
            {
                foreach (MerchBundle_Listing listing in saleItem.MerchBundleSelections)
                {
                    if (listing.SelectedInventory != null && listing.Quantity > 0)
                    {
                        System.Web.UI.WebControls.ListItem exists =
                            merchGuids.Find(delegate(System.Web.UI.WebControls.ListItem match)
                            { return (match.Text == listing.SelectedInventoryId.ToString()); });

                        if (exists != null)
                            exists.Value = (int.Parse(exists.Value) + listing.Quantity).ToString();
                        else
                            merchGuids.Add(new System.Web.UI.WebControls.ListItem(listing.SelectedInventoryId.ToString(),
                                listing.Quantity.ToString()));
                    }
                }
            }
        }
        
        public string ClearCart(bool postSale_IncrementSales)
        {
            string retVal = string.Empty;

            #region tickets and their promotions

            List<SaleItem_Promotion> ticketPromos = new List<SaleItem_Promotion>();
            ticketPromos.AddRange(this.PromotionItems.FindAll(delegate(SaleItem_Promotion match)
                { return (match.SalePromotion.TShowTicketId != null); } ));

            string[] guids = new string[this.TicketItems.Count + ticketPromos.Count];
            for (int i = 0; i < this.TicketItems.Count; i++)
                guids[i] = ((SaleItem_Ticket)TicketItems[i]).GUID.ToString();
            for (int i = 0; i < ticketPromos.Count; i++)
                guids[this.TicketItems.Count + i] = ((SaleItem_Promotion)ticketPromos[i]).GUID.ToString();

            #endregion

            #region merch items
            
            List<System.Web.UI.WebControls.ListItem> merchGuids = new List<System.Web.UI.WebControls.ListItem>();
            foreach (SaleItem_Merchandise sim in this.MerchandiseItems)
                merchGuids.Add(new System.Web.UI.WebControls.ListItem(sim.tMerchId.ToString(), sim.Quantity.ToString()));
            
            //
            AddMerchBundleGuidsMaster(merchGuids);
            

            if (guids.Length > 0)
            {
                TicketItems.Clear();

                //if there is an error in the sp - we do not want user going back to checkout
                //we clear their cart items first - above
                //then we try sp
                //if THIS sp fails we notify them on confirm page as their order has already gone through
                try
                {
                    using (DataSet ds = SPs.TxInventoryClearCartReturnItemNotification
                        (_Enums.InvoiceItemContext.ticket.ToString(), _Config._Inventory_LowTickets_Threshold,
                        string.Join(",", guids), postSale_IncrementSales).GetDataSet())
                    {
                        DataTable dt = ds.Tables[0];
                        string result = dt.Rows[0].ItemArray[0].ToString();

                        if (result != "SUCCESS")
                        {
                            _Error.LogException(new Exception("Clear Cart Error - ticket" + Environment.NewLine + result));
                            retVal = result;
                        }

                        //evaluate possible inventory threshold items
                        if (ds.Tables.Count > 1)
                        {
                            DataTable thresh = ds.Tables[1];
                            ShowTicketCollection tixColl = new ShowTicketCollection();

                            foreach (DataRow dr in thresh.Rows)
                            {
                                int idx = (int)dr.ItemArray[1];
                                ShowTicket sit = Ctx.SaleTickets.GetList()
                                    .Find(delegate(ShowTicket match) { return match.Id == idx; });

                                if (sit == null)
                                    sit = new ShowTicket(idx);
                                
                                if (sit != null)
                                {
                                    ShowTicket ticketToNotify = (sit.PackageBase != null) ? sit.PackageBase : sit;

                                    if (ticketToNotify != null)
                                    {
                                        int index = tixColl.GetList().FindIndex(delegate(ShowTicket match) { return (match.Id == ticketToNotify.Id); });
                                        if (index == -1)
                                        {
                                            try
                                            {
                                                tixColl.Add(ticketToNotify);

                                                string severity = dr.ItemArray[0].ToString().ToLower();
                                                string userName = System.Web.HttpContext.Current.User.Identity.Name;

                                                EventQ.CreateInventoryNotification(userName, severity, 
                                                    //tags not necessary for this notification
                                                    Utils.ParseHelper.StripHtmlTags(ticketToNotify.DisplayNameWithAttribsAndDescription),
                                                    _Enums.InvoiceItemContext.ticket, idx,
                                                    _Config._Inventory_LowTickets_Threshold);
                                            }
                                            catch (Exception ex)
                                            {
                                                _Error.LogException(ex);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _Error.LogException(ex);
                    retVal = ex.Message;
                }
            }

            int ii = 0;
            string[] mGuids = new string[merchGuids.Count];
            foreach (System.Web.UI.WebControls.ListItem li in merchGuids)
                mGuids[ii++] = string.Format("{0}={1}", li.Text, li.Value);


            if (mGuids.Length > 0)
            {
                MerchandiseItems.Clear();

                try
                {
                    //TODO: last param - log - should be in config
                    using (DataSet ds = SPs.TxInventoryClearCartReturnItemNotification
                       (_Enums.InvoiceItemContext.merch.ToString(), _Config._Inventory_LowMerch_Threshold, string.Join(",", mGuids), postSale_IncrementSales).GetDataSet())
                    {
                        DataTable dt = ds.Tables[0];
                        string result = dt.Rows[0].ItemArray[0].ToString();

                        if (result != "SUCCESS")
                        {
                            _Error.LogException(new Exception("Clear Cart Error - merch" + Environment.NewLine + result));
                            retVal = result;
                        }

                        //evaluate possible inventory threshold items
                        if (ds.Tables.Count > 1)
                        {
                            DataTable thresh = ds.Tables[1];

                            foreach (DataRow dr in thresh.Rows)
                            {
                                //create an event for each
                                string userName = string.Empty;

                                try
                                {
                                    string severity = dr.ItemArray[0].ToString();
                                    int idx = (int)dr.ItemArray[1];
                                    userName = System.Web.HttpContext.Current.User.Identity.Name;

                                    Merch merch = Ctx.SaleMerch.GetList()
                                        .Find(delegate(Merch match) { return match.Id == idx; });

                                    if (merch == null)
                                        merch = new Merch(idx);

                                    if (merch != null)
                                    {
                                        //if the merch is tied to a merchbundle - evaluate the merchbundle to see if it needs to be deactivated
                                        if(severity == "soldout")
                                            merch.EvaluateMerchBundleStatus(severity);

                                        EventQ.CreateInventoryNotification(userName, severity, merch.DisplayNameWithAttribs, _Enums.InvoiceItemContext.merch, idx,
                                            _Config._Inventory_LowMerch_Threshold);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    _Error.LogException(ex);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _Error.LogException(ex);
                    retVal = ex.Message;
                }
            }

            #endregion

            //child items
            PromotionItems.Clear();
            Shipments_Merch.Clear();
            Shipments_Tickets.Clear();
            //Ctx.SalePromotion_CouponCodes_Clear();//clear all coupons
            this.CharityAmount = 0;//automatically clears out charity org as well
            this.StoreCredit.Price = 0;

            this.IsShipMultiple_Merch = false;

            OnCartChanged();

            //notify admin
            if (retVal.Trim().Length > 0)
            {
                string username = "unknown user";                
                try
                {
                    username = System.Web.HttpContext.Current.User.Identity.Name;
                } 
                catch{}

                string sessionId = "unknown session";
                try
                {
                    sessionId = string.Format("{0}: ", System.Web.HttpContext.Current.Session.SessionID);
                }
                catch { }


                EventQ.CreateAdminNotification(DateTime.Now, "ClearCart", username, _Enums.EventQVerb.CartCleared,
                    "Error", (postSale_IncrementSales) ? "Post Sale" : "Not Post Sale", string.Format("SessionId: {0} - {1}", sessionId, retVal));
            }

            return retVal.Trim();
        }


        /// <summary>
        /// ensure OrderItems have not expired - check that items are within 90 seconds of expiry                
        /// run a proc to see if items are in the stock table - notify user and log removals
        /// </summary>
        public bool CartHasTicketItemsThatShouldBeConsideredExpired(string userName, string clientId)
        {
            string removals = Ctx.Cart.RemoveExpiredTicketItems(90);

            if (removals.Length > 0)
            {
                string removed = Utils.ParseHelper.StripHtmlTags(removals);

                //TODO: log occurrence - see how often this actually happens
                _Error.LogException(new Exception(string.Format(
                    "Ticket(s) expired at {3}:{0}{1}{0}{2}", Environment.NewLine, userName, removed, clientId)));

                UserEvent.NewUserEvent(userName, DateTime.Now, DateTime.Now, _Enums.EventQStatus.Success, userName,
                    _Enums.EventQContext.User, _Enums.EventQVerb.Checkout_TicketItemsExpired, string.Empty, string.Empty, removed, true);

                Ctx.CurrentCartException = string.Format(
                    "<span class='expired'>these items have expired in your cart.</span><br/>{0}", removals);

                return true;
            }

            return false;
        }

        public string RemoveAnyExpiredItemsFromCart()
        {
            return RemoveExpiredTicketItems();
        }
        /// <summary>
        /// returns a string describing the items that have been removed to display to the customer 
        /// child items do not expire. Promotions need to be updated?
        /// </summary>
        /// <returns></returns>
        public string RemoveExpiredTicketItems()
        {
            return RemoveExpiredTicketItems(0);
        }
        public string RemoveExpiredTicketItems(int offsetSeconds)
        {
            if (TicketItems.Count > 0)
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();

                int incidents = 0;

                //offset seconds needs to add more time - so we subtract
                DateTime expiry = DateTime.Now.AddSeconds(-offsetSeconds);

                for (int i = TicketItems.Count - 1; i >= 0; i--)
                {
                    SaleItem_Ticket tik = (SaleItem_Ticket)TicketItems[i];
                    if (tik.TTL < expiry)
                    {
                        //show what has expired
                        sb.AppendFormat("{0} ticket{1} for {2} ",
                            tik.Quantity, (tik.Quantity > 1) ? "s" : string.Empty,
                            //tags are unnecessary for displaying the expired item
                            Utils.ParseHelper.StripHtmlTags(tik.Ticket.DisplayNameWithAttribsAndDescription));

                        //show link to check availability
                        sb.AppendFormat("<a href=\"/Store/ChooseTicket.aspx?sid={0}\">check availabilty</a><br/>", tik.Ticket.TShowId);

                        Ctx.Cart.SaleItem_Remove(_Enums.InvoiceItemContext.ticket, tik.tShowTicketId);

                        incidents++;
                    }
                }

                if (incidents > 0)
                {
                    this.Shipments_Tickets.Clear();
                    incidents = 0;
                }

                return sb.ToString();
            }

            return string.Empty;
        }

        #endregion

        #region Methods

        /// <summary>
        /// returns the peritem price with shipping for merch - the ticket per item price and ticketshipping
        /// </summary>
        public decimal PreFeeMerchTotal
        {
            get
            {
                decimal total = 0;

                List<SaleItem_Merchandise> coll = new List<SaleItem_Merchandise>();
                coll.AddRange(this.MerchandiseItems);

                foreach (SaleItem_Merchandise sim in coll)
                {
                    total += sim.LineTotal;
                    total += sim.Cart_Bundle_Price;
                }

                return total;
            }
        }
        public decimal PreFeeTicketTotal
        {
            get
            {
                decimal total = 0;

                List<SaleItem_Ticket> coll = new List<SaleItem_Ticket>();
                coll.AddRange(this.TicketItems);

                foreach (SaleItem_Ticket tik in coll)
                {
                    total += tik.LineTotal;                    
                    total += tik.Cart_Bundle_Price;
                }

                return total;
            }
        }
        /// <summary>
        /// does not include discounts
        /// </summary>
        public decimal PreFeeSalePromotion_ItemTotal
        {
            get
            {
                decimal total = 0;

                foreach (SaleItem_Promotion pro in PromotionItems)
                    if (pro.LineTotal > 0)
                        total += pro.LineTotal;
                
                return total;
            }
        }
        /// <summary>
        /// does not include items. will be a positive amount. subtract in appropriate total
        /// </summary>
        public decimal Promotion_DiscountTotal
        {
            get
            {
                decimal total = 0;

                foreach (SaleItem_Promotion pro in PromotionItems)
                    if (pro != null && pro.SalePromotion != null && pro.LineTotal < 0)
                        total += Utils.ParseHelper.AbsoluteValue(pro.LineTotal);

                return total;
            }
        }
        /// <summary>
        /// total of all items not including shipping fees
        /// </summary>
        public decimal PreFeeTotal
        {
            get 
            {
                return PreFeeMerchTotal + PreFeeTicketTotal + PreFeeSalePromotion_ItemTotal;
            }
        }
        
        public bool IsOverMaxTransactionAllowed 
        { 
            get 
            { 
                return (this.ChargeTotal > _Config._AuthorizeNetLimit);  
            } 
        }

        public decimal ProcessingCharge
        {
            get 
            {
                return (this.HasItems && ProcessingFee != null) ? ProcessingFee.Price : 0;
            }
        }
        public decimal Total_NonDiscounted
        {
            get
            {
                return PreFeeTotal + ShippingAndHandling + ProcessingCharge + this.CharityAmount;
            }
        }
        /// <summary>
        /// This is the cart total before any store credits. Includes: preFeeTotal, shipping, processing, charity, and promotional DISCOUNTS
        /// </summary>
        public decimal SubTotal
        {
            get
            {
                return PreFeeTotal + ShippingAndHandling + ProcessingCharge + this.CharityAmount - Promotion_DiscountTotal;
            }
        }
        /// <summary>
        /// the amount to charge to the customer. subtotal minus storecredit applied to the order
        /// </summary>
        public decimal ChargeTotal
        {
            get { return SubTotal - StoreCreditToApply; }
        }
        public bool TicketIsInCollection(int idx)
        {
            List<SaleItem_Ticket> coll = new List<SaleItem_Ticket>();
            coll.AddRange(this.TicketItems);

            foreach(SaleItem_Ticket sit in coll)
            {
                if(sit.tShowTicketId == idx)
                    return true;

                if (sit.Ticket.IsPackage)
                {
                    List<ShowTicket> pkg = new List<ShowTicket>();
                    pkg.AddRange(sit.Ticket.LinkedShowTickets);

                    foreach (ShowTicket st in pkg)
                        if (st.Id == idx)
                            return true;
                }
            }
            return false;
        }
        public bool MerchIsInCollection(int idx)
        {
            List<SaleItem_Merchandise> coll = new List<SaleItem_Merchandise>();
            coll.AddRange(this.MerchandiseItems);

            foreach(SaleItem_Merchandise sim in coll)
            {
                if(sim.tMerchId == idx)
                    return true;
            }
            return false;
        }         
        public SaleItem_Ticket FindSaleItem_TicketById(int idx)
        {
            List<SaleItem_Ticket> coll = new List<SaleItem_Ticket>();
            coll.AddRange(this.TicketItems);

            foreach(SaleItem_Ticket sit in coll)
            {
                if(sit.tShowTicketId == idx)
                    return sit;

                if (sit.Ticket.IsPackage)
                {
                    List<ShowTicket> pkg = new List<ShowTicket>();
                    pkg.AddRange(sit.Ticket.LinkedShowTickets);

                    foreach (ShowTicket st in pkg)
                        if (st.Id == idx)
                            return sit;
                }
            }
            return null;
        }
        public SaleItem_Ticket FindSaleItem_TicketByGUID(Guid guid)
        {
            return this.TicketItems.Find(delegate(SaleItem_Ticket match) { return (match.GUID == guid); });
        }
        public List<SaleItem_Ticket> FindSaleItem_TicketsByShowDateId(int showDateIdx)
        {
            List<SaleItem_Ticket> list = new List<SaleItem_Ticket>();

            List<SaleItem_Ticket> coll = new List<SaleItem_Ticket>();
            coll.AddRange(this.TicketItems);

            foreach(SaleItem_Ticket sit in coll)
                if(sit.Ticket.TShowDateId == showDateIdx)
                    list.Add(sit);

            if(list.Count > 0)
                return list;

            return null;
        }
        public SaleItem_Merchandise FindSaleItem_MerchandiseById(int idx)
        {
            List<SaleItem_Merchandise> coll = new List<SaleItem_Merchandise>();
            coll.AddRange(this.MerchandiseItems);

            foreach(SaleItem_Merchandise sim in coll)
            {
                if(sim.tMerchId == idx)
                    return sim;
            }
            return null;
        }
        public SaleItem_Merchandise FindSaleItem_MerchandiseByGUID(Guid guid)
        {
            return this.MerchandiseItems.Find(delegate(SaleItem_Merchandise match) { return (match.GUID == guid); });
        }
        public List<SaleItem_Merchandise> FindSaleItem_MerchandiseByRequired(SalePromotion promo)
        {
            List<SaleItem_Merchandise> list = new List<SaleItem_Merchandise>();

            List<SaleItem_Merchandise> coll = new List<SaleItem_Merchandise>();
            coll.AddRange(this.MerchandiseItems);

            foreach (SaleItem_Merchandise sim in coll)
            {
                if ((sim.MerchItem.TParentListing.HasValue && promo.RequiredMerchListing.Contains(sim.MerchItem.TParentListing.Value)) ||
                    promo.RequiredMerchListing.Contains(sim.tMerchId))
                    list.Add(sim);
            }

            if (list.Count > 0)
                return list;

            return null;
        }

        #endregion

        #region Sql Calls

        public void UpdateReturnedPaymentData(System.Text.StringBuilder sb, AuthorizeNet auth)
        {
            if(auth == null)
                throw new Exception("Authorization cannot be null.");

            sb.AppendFormat("{0}{0}Update\tAuthorizeNet{0}Set\tbAuthorized = {1},{0}", "\r\n", (auth.IsAuthorized) ? "1" : "0");
            sb.AppendFormat("{0}ProcessorId = '{2}',{1}", "\t", "\r\n", auth.ProcessorId);
            sb.AppendFormat("{0}iResponseCode = {2},{1}", "\t", "\r\n", auth.ResponseCode.ToString());
            sb.AppendFormat("{0}ResponseSubcode = '{2}',{1}", "\t", "\r\n", auth.ResponseSubcode);
            sb.AppendFormat("{0}iResponseReasonCode = {2},{1}", "\t", "\r\n", auth.ResponseReasonCode.ToString());
            sb.AppendFormat("{0}bMd5Match = {2},{1}", "\t", "\r\n", (auth.IsMD5Match) ? "1" : "0");
            sb.AppendFormat("{0}ResponseReasonText = '{2}',{1}", "\t", "\r\n", auth.ResponseReasonText);
            sb.AppendFormat("{0}ApprovalCode = '{2}',{1}", "\t", "\r\n", auth.ApprovalCode);
            sb.AppendFormat("{0}AVSResultCode = '{2}',{1}", "\t", "\r\n", auth.AVSResultCode);
            sb.AppendFormat("{0}CardCodeResponseCode = '{2}'{1}", "\t", "\r\n", auth.CardCodeResponseCode);
            sb.AppendFormat("Where\tId = {1}{0}{0}{0}", "\r\n", auth.Id.ToString());
        }

        #endregion

        #region Merch Bundles
        
        /// <summary>
        /// we need to insert the cart items and the bundle itself
        /// </summary>
        public int GetMaxPossibleSelectionsAllowedForBundle(SaleItem_Base saleItem, int bundleId)
        {
            int cartQty = 0;
            int allowedSelections = 0;
            MerchBundle bundle = saleItem.GetMerchBundle(bundleId);
            List<SaleItem_Base> list = new List<SaleItem_Base>();

            //first get the quantity of like items - merch can have diff sizes - tickets will be unique
            //establish the bundle
            if (saleItem is WillCallWeb.StoreObjects.SaleItem_Merchandise)
            {
                int? parentId = ((SaleItem_Merchandise)saleItem).MerchItem.TParentListing;
                if (parentId.HasValue)
                {
                    List<SaleItem_Merchandise> likeMerch = new List<SaleItem_Merchandise>();
                    likeMerch.AddRange(this.MerchandiseItems.FindAll(delegate(SaleItem_Merchandise match) { return (match.MerchItem.TParentListing == parentId); } ));
                    foreach (SaleItem_Merchandise like in likeMerch)
                        cartQty += like.Quantity;
                }
            }
            else if (saleItem is WillCallWeb.StoreObjects.SaleItem_Ticket)
                cartQty = saleItem.Quantity;

            if (cartQty > 0 && bundle != null)
            {
                int reqParent = bundle.RequiredParentQty;
                int bundleQty = cartQty / reqParent;
                allowedSelections = bundle.MaxSelections * bundleQty;
            }

            return allowedSelections;
        }

        #endregion

        #region Promotions

        /// <summary>
        /// A null value indicates success!!! Allows for multiple conditions to be met. 
        /// </summary>
        public string PromotionRequirementsMet(SalePromotion promo)
        {
            string meet = null;
            bool met = false;

            if (promo.IsAwardable && promo.IsUnlocked(_ctx.MarketingProgramKey) && promo.CouponMatch(Ctx.SalePromotion_CouponCodes, _Config._Coupon_IgnoreCase) != null)
            {
                if (promo.Requires_MerchItem || promo.Requires_TicketItem || promo.Requires_ShowDatePurchase)
                {
                    if (promo.Requires_MerchItem)
                    {
                        //get the merch that we are to match
                        int merchHits = 0;

                        //not only do we wish to match the merch product in question
                        //but if we have a child item - that should also be a match
                        List<SaleItem_Merchandise> coll = new List<SaleItem_Merchandise>();
                        coll.AddRange(this.MerchandiseItems);

                        foreach (SaleItem_Merchandise sim in coll)
                        {
                            if (sim.MerchItem.TParentListing.HasValue)
                            {
                                if (promo.RequiredMerchListing.Contains(sim.MerchItem.TParentListing.Value) ||
                                    promo.RequiredMerchListing.Contains(sim.MerchItem.Id))
                                    merchHits += sim.Quantity;
                            }
                        }

                        met = merchHits >= promo.RequiredParentQty;

                        if (met)
                            return meet;
                        else
                        {                            
                            SalePromotion saleItem = (SalePromotion)this.SalePromotions_RunningAndAvailable.Find(promo.Id);
                            if (saleItem != null)
                            {
                                //if we have a singular item to match
                                if (promo.RequiredMerchListing.Count == 1)
                                    return string.Format("Add {0} to your cart to receive this promotion", saleItem.RequiredMerchItems(Ctx)[0].DisplayNameWithAttribs);
                            }
                        }  
                    }
                    if (promo.Requires_TicketItem)
                    {
                        int tktIdx = promo.TRequiredParentShowTicketId.Value;

                        SaleItem_Ticket sit = this.FindSaleItem_TicketById(tktIdx);
                        met = sit != null && sit.Quantity >= promo.RequiredParentQty;

                        if (met)
                            return meet;
                        else return string.Format("Add {0} to your cart to receive this promotion", 
                            //dont use tags in a notification
                            Utils.ParseHelper.StripHtmlTags(promo.ShowTicketRecord_RequiredForPromotion.DisplayNameWithAttribsAndDescription));
                    }
                    if (promo.Requires_ShowDatePurchase)
                    {
                        int dateIdx = promo.TRequiredParentShowDateId.Value;

                        List<SaleItem_Ticket> list = this.FindSaleItem_TicketsByShowDateId(dateIdx);

                        if (list != null && list.Count > 0)
                        {
                            int qty = 0;
                            foreach(SaleItem_Ticket sit in list)
                                qty += sit.Quantity;

                            met = qty >= promo.RequiredParentQty;
                        }

                        if (met)
                            return meet;
                        else return string.Format("Add a ticket from {0} to your cart to receive this promotion", promo.ShowDateRecord_RequiredForPromotion.Display.Billing);
                    }
                }
                else//we have a discount promo //the following reqs can be combined - min purchases and shipping and coupon
                {
                    decimal includedPromoTotal = 0;
                    if (promo.AllowPromoTotalInMinimum)
                        includedPromoTotal = this.PreFeeSalePromotion_ItemTotal;
                    
                    if (promo.Requires_MinMerchPurchase)
                    {
                        decimal cartTotal = this.PreFeeMerchTotal + includedPromoTotal;

                        met = cartTotal >= promo.MinimumMerchandisePurchase;

                        if (!met)
                        {
                            decimal diff = promo.MinimumMerchandisePurchase - cartTotal;
                            meet = string.Format("Spend {0} more in merchandise to receive this promotion{1}", diff.ToString("c"), Utils.Constants.Separator);
                        }                        
                    }
                    if (promo.Requires_MinTicketPurchase)
                    {
                        met = (this.PreFeeTicketTotal + includedPromoTotal) >= promo.MinimumTicketPurchase;

                        if (!met)
                        {
                            decimal diff = promo.MinimumTicketPurchase - (this.PreFeeTicketTotal + includedPromoTotal);
                            meet += string.Format("Spend {0} more in tickets to receive this promotion{1}", diff.ToString("c"), Utils.Constants.Separator);
                        }
                    }
                    //this should apply to shipping promotions - just ensure we have a $$ amount and the cart
                    // will price it if we meet the other ship method reqs
                    if (promo.Requires_MinTotalPurchase)
                    {
                        met = (this.PreFeeTotal - this.GiftCertificateTotal + includedPromoTotal) >= promo.MinimumTotalPurchase;

                        if (!met)
                        {
                            decimal diff = promo.MinimumTotalPurchase - (this.PreFeeTotal - this.GiftCertificateTotal + includedPromoTotal);
                            meet += string.Format("Spend {0} more in our store to receive this promotion{1}", diff.ToString("c"), Utils.Constants.Separator);
                        }
                    }

                    if (promo.IsShippingPromotion)//shipping items are added but not necessarily displayed
                    {
                        if (promo.IsDiscountContext_MerchShipping && promo.IsDiscountContext_TicketShipping)
                        {
                            if ((!this.HasMerchandiseItems_Shippable) && (!this.HasTicketItems_CurrentlyShippable))
                                meet += string.Format("you need shippable items for this promotion{0}", Utils.Constants.Separator);
                        }
                        else if (promo.IsDiscountContext_MerchShipping && (!this.HasMerchandiseItems_Shippable))
                            meet += string.Format("you need shippable MERCH items for this promotion{0}", Utils.Constants.Separator);
                        else if (promo.IsDiscountContext_TicketShipping && (!this.HasTicketItems_CurrentlyShippable))
                            meet += string.Format("you need shippable TICKET items for this promotion{0}", Utils.Constants.Separator);

                        //display what shipmethod is needed
                        //if method is all - show nothing
                        if (promo.ShipOfferMethod != "all")
                        {
                            string reqMethod = promo.ShipOfferMethod.ToLower();
                            List<SaleItem_Shipping> ship = new List<SaleItem_Shipping>();

                            //as only one ship method can be specified - the else statement will handle those promos with both applications
                            if (promo.DiscountContext.Contains(_Enums.DiscountContext.ticketshipping) && Ctx.Cart.HasTicketItems_CurrentlyShippable)
                            {
                                //if the ticket method matches.....
                                //only show a msg if they choose a diff method
                                ship.AddRange(Ctx.Cart.Shipments_Tickets.FindAll(delegate(SaleItem_Shipping match) { return (match.ShipMethod.ToLower() == reqMethod); }));

                                if (ship.Count == 0)
                                    meet += string.Format("Select {0} as your ticket shipping option to receive this promotion{1}", promo.ShipOfferMethod, Utils.Constants.Separator);
                            }
                            if (promo.DiscountContext.Contains(_Enums.DiscountContext.merchshipping))
                            {
                                //if the ticket method matches.....
                                //only show a msg if they choose a diff method
                                ship.AddRange(Ctx.Cart.Shipments_Merch.FindAll(delegate(SaleItem_Shipping match) { return (match.ShipMethod.ToLower() == reqMethod); }));

                                if (ship.Count == 0)
                                {
                                    string response = string.Format("Select {0} as your ticket shipping option to receive this promotion{1}", promo.ShipOfferMethod, Utils.Constants.Separator);
                                    if (meet == null || meet.IndexOf(response) == -1)
                                        meet += response;
                                }
                            }
                        }
                    }
                }
            }

            return meet;
        }


        /// <summary>
        /// Returns a collection of the currently running (active, start and end date matches) as well as those 
        /// promotions which require a code (unlock and street team). Does NOT include banners_only
        /// </summary>
        public Wcss.SalePromotionCollection SalePromotions_RunningAndAvailable
        {
            get
            {
                SalePromotionCollection coll = new SalePromotionCollection();
                coll.AddRange(_Lookits.SalePromotions.GetList().FindAll(delegate(SalePromotion match)
                {
                    bool hasAllotment = true;
                    if (match.IsMerchPromotion)
                    {
                        SalePromotionAwardCollection collA = new SalePromotionAwardCollection();
                        collA.AddRange(match.SalePromotionAwardRecords());
                     
                        foreach (SalePromotionAward spa in collA)
                        {
                            if (!spa.MerchRecord_Parent.IsActive)
                                hasAllotment = false;
                            if (spa.MerchRecord_Parent.Allotment <= 0)
                                hasAllotment = false;

                            if (!hasAllotment)
                                break;
                        }
                    }

                    return (
                        hasAllotment && 
                        match.IsCurrentlyRunning(Ctx.SalePromotionUnlock) &&
                        match.CouponMatch(Ctx.SalePromotion_CouponCodes, true) != null);
                        }));

                return coll;
            }
        }
        /// <summary>
        /// Use when the site is displaying informational banners
        /// </summary>
        public Wcss.SalePromotionCollection SalePromotions_BannersOnly_RunningAndAvailable
        {
            get
            {
                SalePromotionCollection coll = new SalePromotionCollection();
                coll.AddRange(_Lookits.SalePromotions.GetList().FindAll(delegate(SalePromotion match)
                {
                    //when not awardable - indicates a banner
                    return ((!match.IsAwardable) && match.IsCurrentlyRunning(Ctx.SalePromotionUnlock));//banners are not affected by coupons
                }));

                return coll;
            }
        }

        //Promotions are added automatically to cart
        //TODO: be able to select/deselect promo items
        //allow only one promotion
        public void FullfillPromotions()
        {
            if (Ctx.Cart.ItemCount == 0)
            {
                PromotionItems.Clear();
                return;
            }

            //loop thru promotions - which are generally few
            SalePromotionCollection awardable = new SalePromotionCollection();
            awardable.AddRange(this.SalePromotions_RunningAndAvailable.GetList().FindAll(delegate(SalePromotion match) { return (match.IsAwardable); }));

            foreach (SalePromotion promo in awardable)
            {
                string req = this.PromotionRequirementsMet(promo);
                bool RequirementsMet = (req == null);

                //if the salePromotion item is not in the cart...add it, etc
                SaleItem_Promotion sip = (SaleItem_Promotion)this.PromotionItems
                    .Find(delegate(SaleItem_Promotion match) { return (match.tSalePromotionId == promo.Id); });

                //always add the ship promo
                if (sip == null && RequirementsMet)
                {
                    //check for available inventory - we are looking at a parent item - so just do a quick check on its availability
                    //a finer check will be done at checkout
                    bool hasAvailableInventory = false;

                    if (promo.IsMerchPromotion)
                    {
                        foreach (SalePromotionAward award in promo.SalePromotionAwardRecords())
                        {
                            if (award.MerchRecord_Parent.Available > 0)
                            {
                                hasAvailableInventory = true;
                                break;
                            }
                        }
                    }
                    else if (promo.IsTicketPromotion)
                    {
                        //#w
                        if (promo.ShowTicketRecord.Available > 0)
                        {
                            hasAvailableInventory = true;
                            break;
                        }
                    }
                    else if (promo.IsDiscountPromotion)
                        hasAvailableInventory = true;

                    //only add the promotion if we have something to choose from
                    if(hasAvailableInventory)
                    {
                        //add the item - inventory will be further checked at checkout
                        SaleItem_Promotion newItem = new SaleItem_Promotion(this.Ctx, promo.Id, 1);
                        PromotionItems.Add(newItem);
                    }
                    else
                    {
                        OnPromoMessage(promo.Id, string.Format("Sorry, the <span style=\"color: Black;\">{0}</span> promotion is no longer available.", promo.DisplayText));
                    }
                }
                else if (sip != null && (!RequirementsMet))
                {
                    PromotionItems.Remove(sip);

                    sip = null;

                    //we dont show the message here - only when expired items are removed
                }
            }
        }


        #endregion

        #region Store Credits

        private SaleItem_StoreCredit _storeCredit = null;
        /// <summary>
        /// This refers to the object that is a request for the amount of store credit to be used.
        /// </summary>
        public SaleItem_StoreCredit StoreCredit
        {
            get
            {
                if (_storeCredit == null)
                    _storeCredit = new SaleItem_StoreCredit();

                return _storeCredit;
            }
        }

        public decimal AmountOfCartInGiftCertificates
        {
            get
            {
                decimal d = 0;

                List<SaleItem_Merchandise> gifts = new List<SaleItem_Merchandise>();
                gifts.AddRange(this.GiftCertificateItems);

                if (gifts.Count > 0)
                {
                    List<SaleItem_Promotion> proms = new List<SaleItem_Promotion>();
                    proms.AddRange(this.PromotionItems.FindAll(delegate(SaleItem_Promotion match) { return (match.SalePromotion.IsDiscountContext_TriggerItemOnly && 

                        match.SalePromotion.RequiredMerchListing != null && match.SalePromotion.RequiredMerchListing.Count > 0 && 

                        match.AllRequiredMerchItemsAreGifts); } ));

                    foreach (SaleItem_Merchandise sim in gifts)
                    {
                        d += sim.LineTotal;
                        //if the item qualifies for the promotion
                        if (proms.Count > 0)
                        {
                            //find a match
                            if(sim.MerchItem.TParentListing.HasValue)
                            {
                                SaleItem_Promotion matchingPromo = proms.Find(delegate(SaleItem_Promotion match)
                                {
                                    return (match.SalePromotion.RequiredMerchListing.Contains(sim.MerchItem.TParentListing.Value) || 
                                        match.SalePromotion.RequiredMerchListing.Contains(sim.MerchItem.Id));
                                });

                                if(matchingPromo != null)
                                    d += matchingPromo.LineTotal;
                            }
                        }
                    }
                }

                return d;
            }
        }
        /// <summary>
        /// this is the actual amount that will get applied to the invoice. Even if the customer has specified more $$ to be applied, this
        /// will enforce a max set at the invoice amount. (Previously CreditAmount)
        /// </summary>
        public decimal StoreCreditToApply//CreditAmount
        {
            get
            {
                if (StoreCredit.Price > SubTotal - AmountOfCartInGiftCertificates)
                    return SubTotal - AmountOfCartInGiftCertificates;

                return StoreCredit.Price;
            }
        }
        
        /// <summary>
        /// this is the amount of store credit minus the current credit amount being applied to the invoice. In plain terms, it is the
        /// amount of store credit currently available in the profile to apply
        /// </summary>
        public decimal StoreCreditCurrentlyAvailableForProfile
        {
            get
            {
                if (System.Web.HttpContext.Current != null)
                {
                    ProfileCommon prof = System.Web.HttpContext.Current.Profile as ProfileCommon;
                    if (!prof.IsAnonymous && prof.StoreCredit > 0)
                    {
                        return decimal.Round(decimal.Parse(prof.StoreCredit.ToString()) - this.StoreCreditToApply, 2);
                    }
                }

                return 0;
            }
        }
        /// <summary>
        /// the amount of store credit that can be applied to a particular invoice. Will not let an amount greater than the invoice be offered
        /// </summary>
        public decimal StoreCreditAvailableToApplyToInvoice
        {
            get
            {
                if (System.Web.HttpContext.Current != null)
                {
                    ProfileCommon prof = System.Web.HttpContext.Current.Profile as ProfileCommon;
                    if (!prof.IsAnonymous && prof.StoreCredit > 0)
                    {
                        decimal profileCredit = decimal.Parse(prof.StoreCredit.ToString());
                        decimal cartAmount = Ctx.Cart.SubTotal - AmountOfCartInGiftCertificates;
                        decimal leastOfTwo = (profileCredit.CompareTo(cartAmount) == 1) ? cartAmount : profileCredit;
                        return leastOfTwo;
                    }
                }

                return 0;
            }
        }

        #endregion

    }
}
