using System;
using System.Collections.Generic;

using Wcss;

namespace WillCallWeb.StoreObjects
{
    /// <summary>
    /// Summary description for Merch
    /// </summary>
    [Serializable]
    public class SaleItem_Shipping
    {
        #region Constructors

        /// <summary>
        /// use for general ticket shipping
        /// </summary>
        /// <param name="item"></param>
        public SaleItem_Shipping(List<SaleItem_Ticket> itemList)
        {
            _shipContext = _Enums.InvoiceItemContext.shippingticket;

            //int idx = itemList.FindIndex(delegate(SaleItem_Ticket match) { return (!match.Ticket.IsAllowWillCall); });
            //if (idx == -1)
            //{
            _shipCost = 0;
            _shipMethod = Wcss.ShipMethod.WillCall;
            //}
            //else
            //{
            //    _shipCost = Wcss._Config._Shipping_Tickets_FixedAmount;
            //    _shipMethod = Wcss._Config._Shipping_Tickets_DefaultMethod;
            //}

            _handlingCharges = 0;

            if (itemList != null)
                Items_Tickets.AddRange(itemList);

            IsGeneral = true;
        }
        /// <summary>
        /// use for flat and separate items
        /// </summary>
        /// <param name="item"></param>
        public SaleItem_Shipping(SaleItem_Ticket item)
        {
            _shipContext = _Enums.InvoiceItemContext.shippingticket;
            _shipCost = item.ShippingCharge;
            _shipMethod = item.Ticket.FlatMethod;

            Items_Tickets.Add(item);

            IsShipSeparate = true;
            IsFlatShip = true;
        }
        /// <summary>
        /// use for flat and separate items
        /// </summary>
        /// <param name="item"></param>
        public SaleItem_Shipping(SaleItem_Merchandise item)
        {
            _shipContext = _Enums.InvoiceItemContext.shippingmerch;
            _shipCost = 0;
            _shipMethod = string.Empty;
            if (item.MerchItem.IsBackordered)
                _shipDate = Wcss._Shipper.CalculateShipDate(item.MerchItem.BackorderDate);

            AddShippingItemsForSaleItem(item);
            IsShipSeparate = item.MerchItem.IsShipSeparate;
            IsFlatShip = item.MerchItem.IsFlatShip;
        }
        /// <summary>
        /// use for back order items - merchandise
        /// </summary>
        public SaleItem_Shipping(DateTime shipDate, List<SaleItem_Merchandise> itemList)
        {
            _shipContext = _Enums.InvoiceItemContext.shippingmerch;
            _shipCost = 0;
            _shipMethod = string.Empty;
            _shipDate = shipDate;

            foreach (SaleItem_Merchandise sim in itemList)
                AddShippingItemsForSaleItem(sim);

            IsBackOrder = true;
        }

        private void AddShippingItemsForSaleItem(SaleItem_Merchandise item)
        {
            //if this is a parcel item or its bundle selections contain a parcel item - then add it
            if (item.MerchItem.IsParcelDelivery)
                this.Items_Merch.Add(item);
            else if (item.MerchBundleSelections.Count > 0)
            {
                int idx = item.MerchBundleSelections.FindIndex(delegate(MerchBundle_Listing match) { return ((!match.IsOptOut) && match.SelectedInventory.IsParcelDelivery); });
                if (idx != -1)
                    this.Items_Merch.Add(item);
            }
        }
        /// <summary>
        /// Use for general shipping - merchandise
        /// </summary>
        public SaleItem_Shipping(List<SaleItem_Merchandise> itemList)
        {
            DateTime shipDate = Wcss._Shipper.NowShip;

            //set ship date
            foreach (SaleItem_Merchandise sim in itemList)
            {
                if (sim.MerchItem.IsBackordered)
                {
                    DateTime backDate = Wcss._Shipper.CalculateShipDate(sim.MerchItem.BackorderDate);
                    if (backDate > shipDate)
                        shipDate = backDate;
                }
            }

            _shipContext = _Enums.InvoiceItemContext.shippingmerch;
            _shipCost = 0;
            _shipMethod = string.Empty;
            _shipDate = shipDate;

            foreach (SaleItem_Merchandise sim in itemList)
                AddShippingItemsForSaleItem(sim);

            IsGeneral = true;
        }




        #endregion

        #region Properties And Methods

        [NonSerialized]
        private System.Guid _guid;
        private _Enums.InvoiceItemContext _shipContext = _Enums.InvoiceItemContext.shippingmerch;
        private string _shipMethod = string.Empty;
        private decimal _shipCost = 0;
        private decimal _handlingCharges = 0;
        private DateTime _shipDate = Wcss._Shipper.CalculateShipDate(DateTime.Now);
        private List<SaleItem_Merchandise> _items_Merch;
        public List<SaleItem_Merchandise> Items_Merch
        {
            get
            {
                if (_items_Merch == null)
                    _items_Merch = new List<SaleItem_Merchandise>();

                return _items_Merch;
            }
            set { _items_Merch = value; }
        }
        //private List<MerchWithQuantity> _items_Merch_All;
        public List<MerchWithQuantity> Items_Merch_All
        {
            get
            {
                List<MerchWithQuantity> list = new List<MerchWithQuantity>();

                foreach (SaleItem_Merchandise sim in this.Items_Merch)
                {
                    if (sim.MerchItem.IsParcelDelivery)
                        list.Add(new MerchWithQuantity(sim.Quantity, sim.MerchItem));

                    if (sim.MerchBundleSelections.Count > 0)
                    {
                        List<MerchBundle_Listing> selections = new List<MerchBundle_Listing>();
                        selections.AddRange(sim.MerchBundleSelections.FindAll(delegate(MerchBundle_Listing match) { return ((!match.IsOptOut) && match.SelectedInventory.IsParcelDelivery); }));

                        foreach (MerchBundle_Listing listing in selections)
                            list.Add(new MerchWithQuantity(listing.Quantity, listing.SelectedInventory));
                    }
                }

                return list;
            }
        }

        private List<SaleItem_Promotion> _items_Promo;
        public List<SaleItem_Promotion> Items_Promo
        {
            get
            {
                if (_items_Promo == null)
                    _items_Promo = new List<SaleItem_Promotion>();

                return _items_Promo;
            }
            set { _items_Promo = value; }
        }
        private List<SaleItem_Ticket> _items_Tickets;
        public List<SaleItem_Ticket> Items_Tickets
        {
            get
            {
                if (_items_Tickets == null)
                    _items_Tickets = new List<SaleItem_Ticket>();

                return _items_Tickets;
            }
            set { _items_Tickets = value; }
        }

        public System.Guid GUID
        {
            get
            {
                if (_guid == System.Guid.Empty)
                    _guid = System.Guid.NewGuid();
                return _guid;
            }
        }
        public _Enums.InvoiceItemContext ShipContext { get { return _shipContext; } }
        public string ShipMethod { get { return _shipMethod; } set { _shipMethod = value; } }
        public decimal ShipCost { get { return _shipCost; } set { _shipCost = value; } }
        public decimal HandlingCharges { get { return _handlingCharges; } set { _handlingCharges = value; } }
        public DateTime ShipDate { get { return _shipDate; } set { _shipDate = value; } }

        public int Quantity { get { return 1; } }

        public decimal LineTotal { get { return decimal.Round((this.ShipCost * this.Quantity), 2); } }

        private bool _general = false;
        /// <summary>
        /// Is general is the default shipping type - others: backorder, flatship, shipseparate
        /// </summary>
        public bool IsGeneral { get { return _general; } set { _general = value; } }
        private bool _backorder = false;
        public bool IsBackOrder { get { return _backorder; } set { _backorder = value; } }
        private bool _flatShip = false;
        public bool IsFlatShip { get { return _flatShip; } set { _flatShip = value; } }
        private bool _shipSeparate = false;
        public bool IsShipSeparate { get { return _shipSeparate; } set { _shipSeparate = value; } }

        #endregion
    }
}
