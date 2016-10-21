using System;

using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

using System.Collections.Generic;
using System.ComponentModel;
using Wcss;
using WillCallWeb.StoreObjects;

//    <asp:Label ID="lblControl" AssociatedControlID="listControl" CssClass="listlabel" Text="" runat="server" />

namespace WillCallWeb.Components.Util
{
    [ToolboxData("<{0}:ShipRateListing Title=\"This item must ship separately\" runat=\"server\" ShipmentNumber=\"\"></{0}:ShipRateListing>")]
    [DefaultPropertyAttribute("Title")]
    public partial class ShipRateListing : BaseControl
    {
        #region Properties

        private string _address = string.Empty;
        private string _country = string.Empty;
        private string _zip = string.Empty;
        private string _state = string.Empty;
        private string _title = string.Empty;
        private string _itemGuid = null;
        private DateTime _intendedShipDate = Wcss._Shipper.NowShip;

        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }
        [BrowsableAttribute(true)]
        [DescriptionAttribute("Sets the identifier for the associated shipment.")]
        [CategoryAttribute("Data")]
        public string ItemGuid
        {
            get { return _itemGuid; }
            set { _itemGuid = value; }
        }
        private SaleItem_Shipping _shipment = null;
        [BrowsableAttribute(true)]
        [DescriptionAttribute("Gets the associated shipment.")]
        [CategoryAttribute("Data")]
        public SaleItem_Shipping Shipment
        {
            get 
            {
                if (ItemGuid != null && _shipment == null || (ItemGuid != null && ItemGuid != _shipment.GUID.ToString()))
                    _shipment = Ctx.Cart.Shipments_Merch.Find(delegate(SaleItem_Shipping match) { return (match.GUID.ToString().Equals(ItemGuid)); });

                return _shipment;
            }
        }
        /// <summary>
        /// This gets set to NowShip for normal list. This is not affiliated with the cart shipping date though
        /// </summary>
        [BrowsableAttribute(true)]
        [DescriptionAttribute("Sets the ship date for options.")]
        [CategoryAttribute("Data")]
        public DateTime IntendedShipDate
        {
            get { return _intendedShipDate; }
            set { _intendedShipDate = value; }
        }
        protected bool IsBackorderOption { get { return (IntendedShipDate != Wcss._Shipper.NowShip); } }

        #endregion

        #region Page Overhead

        protected override void LoadControlState(object savedState)
        {
            object[] ctlState = (object[])savedState;
            base.LoadControlState(ctlState[0]);
            this.ItemGuid = (ctlState[1] == null) ? null : ctlState[1].ToString();
            this.IntendedShipDate = (ctlState[2] == null) ? Wcss._Shipper.NowShip : (DateTime)ctlState[2];
        }

        protected override object SaveControlState()
        {
            object[] ctlState = new object[3];
            ctlState[0] = base.SaveControlState();
            ctlState[1] = this.ItemGuid;
            ctlState[2] = this.IntendedShipDate;
            return ctlState;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (Ctx.SessionInvoice != null)
            {
                _address = Ctx.SessionInvoice.WorkingShippingAddress;
                _country = Ctx.SessionInvoice.WorkingCountry.Trim();
                if (_country.ToLower() == "usa")
                    _country = "us";

                _zip = Ctx.SessionInvoice.WorkingZip;
                _state = Ctx.SessionInvoice.WorkingState;
            }

           this.Page.RegisterRequiresControlState(this);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                listControl.DataBind();
        }

        #endregion

        #region Binding

        protected void listControl_DataBinding(object sender, EventArgs e)
        {
            DropDownList listControl = (DropDownList)sender;

            if (Ctx.SessionInvoice != null && Shipment != null)
            {
                List<ListItem> optionList = new List<ListItem>();
                decimal weight = 0;
                decimal salesTotal = 0;
                string items = string.Empty;


                if (Shipment.IsGeneral)
                {
                    //if(Shipment.Items_Merch.Count > 0 && ((Ctx.Cart.HasBackorderedMerch && Ctx.Cart.ShipMultipleMerch) || (Ctx.Cart.HasFlatShipMerch || Ctx.Cart.HasShipSeparateMerch)))
                    Title = string.Format("Item(s) in this shipment will ship from our warehouse on or about {0}.", Shipment.ShipDate.ToString("MM/dd/yyyy"));
                }
                else if (Shipment.IsBackOrder)
                {
                    Title = string.Format("Item(s) in this shipment will ship from our warehouse on or about {0}.", Shipment.ShipDate.ToString("MM/dd/yyyy"));
                }
                else if (Shipment.IsShipSeparate)
                {
                    Title = string.Format("This item will ship separately on or about {0}.", Shipment.ShipDate.ToString("MM/dd/yyyy"));
                }
                else if (Shipment.IsFlatShip)
                {
                    //if(Shipment.Items_Merch.Count > 0 && ((Ctx.Cart.HasBackorderedMerch && Ctx.Cart.ShipMultipleMerch) || (Ctx.Cart.HasFlatShipMerch || Ctx.Cart.HasShipSeparateMerch)))
                    Title = string.Format("Item(s) in this shipment will ship from our warehouse on or about {0}.", Shipment.ShipDate.ToString("MM/dd/yyyy"));
                }

                //verify shipping options
                try
                {
                    foreach (SaleItem_Merchandise sim in Shipment.Items_Merch)
                    {
                        weight += sim.Quantity * sim.MerchItem.Weight;
                        salesTotal += sim.LineTotal;
                        
                        //handle items with bundles diff - only include the name in the listing of shippables
                        //if the item contains bundles
                        //and there are selected items for those bundles
                        if (sim.MerchBundleSelections.Count > 0)
                        {
                            //compute for bundles
                            weight += sim.Cart_Bundle_Weight;
                            salesTotal += sim.Cart_Bundle_Price;

                            //and those items are parcels
                            List<MerchBundle_Listing> listings = new List<MerchBundle_Listing>();
                            listings.AddRange(sim.MerchBundleSelections
                                .FindAll(delegate(MerchBundle_Listing match) { return ((!match.IsOptOut) && match.SelectedInventory != null && match.SelectedInventory.IsParcelDelivery); }));

                            if (listings.Count > 0)
                            {
                                items += string.Format("<div>{0} @ {1}</div>", sim.Quantity, sim.MerchItem.DisplayNameWithAttribs);

                                if (listings.Count > 1)
                                    listings.Sort(delegate(MerchBundle_Listing x, MerchBundle_Listing y) { return (x.BundleId.CompareTo(y.BundleId)); });

                                foreach (MerchBundle_Listing listing in listings)
                                {
                                    //now add parcel items to the description
                                    items += string.Format("<div>* {0} @ {1}</div>", listing.Quantity, listing.SelectedInventory.DisplayNameWithAttribs);

                                }                                
                            }
                        }
                        else
                            items += string.Format("<div>{0} @ {1}</div>", sim.Quantity, sim.MerchItem.DisplayNameWithAttribs);
                    }



                    Shipment.HandlingCharges = ecommercemax_shipping.ComputeHandlingFee(Shipment, salesTotal);

                    optionList = ecommercemax_shipping.GetShipRates(Shipment.HandlingCharges, _address, _country, _zip, _state, weight,
                        false, Shipment);

                    if (optionList.Count == 0)
                        throw new Exception("We are unable to ship to the address you have provided. Please enter a different address or <a href=\"/contact.aspx\">contact us</a>.");

                    if (Shipment.IsFlatShip && Utils.Shipping.IsContinentalUsShipment(_country, _state))
                    {
                        SaleItem_Merchandise flatItem = Shipment.Items_Merch[0];

                        decimal shipAmount = flatItem.Quantity * flatItem.MerchItem.FlatShip;
                        string optionText = (flatItem.MerchItem.FlatMethod != null & flatItem.MerchItem.FlatMethod.Trim().Length > 0) ? flatItem.MerchItem.FlatMethod :
                            "Flat Rate Shipping";

                        string value = string.Format("{0}~{1}", optionText, shipAmount);

                        optionList.Clear();

                        optionList.Add(new ListItem(string.Format("{0} --> {1}  **Flat Rate**", optionText, shipAmount.ToString("c")), value));
                    }
                 
                }
                catch (Exception ex)
                {
                    Ctx.CurrentCartException = ex.Message;
                    base.Redirect("/Store/Checkout.aspx");
                }

                if (optionList.Count > 0)
                {
                    listControl.DataSource = optionList;
                    listControl.DataTextField = "Text";
                    listControl.DataValueField = "Value";

                    //not necessary to do this if ship method is correctly described
                    //listControl.Enabled = (optionList.Count > 1);

                    //show title
                    if (Title.Trim().Length > 0)
                    {
                        litTitle.Visible = true;
                        litTitle.Text = string.Format("<div class=\"ship-title\">{0}</div>", Title);
                    }
                    else
                        litTitle.Visible = false;

                    //show items
                    if (items.Trim().Length > 0)
                    {
                        litItems.Visible = true;
                        litItems.Text = string.Format("<div class=\"items\">{0}</div>", items);
                    }
                    else
                        litItems.Visible = false;
                }
            }
        }
        protected void listControl_DataBound(object sender, EventArgs e)
        {
            if (this.Page.ToString().ToLower() != "asp.store_shipping_aspx")
            {
                this.Visible = false;
                return;
            }

            DropDownList listControl = (DropDownList)sender;

            if (listControl.Items.Count > 0)
            {
                //ensure a selection
                if (listControl.SelectedIndex == -1)
                    listControl.SelectedIndex = 0;

                UpdateItemSelection(listControl.SelectedItem);
            }
            else
                this.Controls.Clear();
        }
        private void UpdateItemSelection(ListItem li)
        {
            //update ship total
            string val = li.Value;
            string[] args = val.Split('~');

            string method = args[0].ToString();
            decimal shipCost = decimal.Parse(args[1]);

            if (Shipment != null)
            {
                Shipment.ShipMethod = method;
                Shipment.ShipCost = shipCost;
            }
        }

        #endregion

        #region Event handlers

        protected void listControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList listControl = (DropDownList)sender;
            //update cart - throw event?
            UpdateItemSelection(listControl.SelectedItem);

            OnSelectedIndexChanged(new EventArgs());
        }

        private static readonly object SelectedIndexChangedEventKey = new object();

        public delegate void SelectedIndexChangedEventHandler(object sender, EventArgs e);

        public event SelectedIndexChangedEventHandler SelectedIndexChanged
        {
            add { Events.AddHandler(SelectedIndexChangedEventKey, value); }
            remove { Events.RemoveHandler(SelectedIndexChangedEventKey, value); }
        }
        public virtual void OnSelectedIndexChanged(EventArgs e)
        {
            SelectedIndexChangedEventHandler handler = (SelectedIndexChangedEventHandler)Events[SelectedIndexChangedEventKey];

            if (handler != null)
                handler(this, e);
        }

        #endregion
    }
}
