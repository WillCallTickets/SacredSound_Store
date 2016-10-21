using System;

using Wcss;

namespace WillCallWeb.StoreObjects
{
	/// <summary>
	/// Summary description for TicketItem.
	/// </summary>
	[Serializable]
	public class SaleItem_Ticket : SaleItem_Base
	{
		#region Constructors

        public SaleItem_Ticket(WebContext ctx, int showTicketId, int qty)
		{
			this.tShowTicketId = showTicketId;
			this.Quantity = qty;
			this.BornOnDate = DateTime.Now;
			this.TTL = BornOnDate.AddSeconds(_Config._TTL_Secs_CartItems);
			this.Ctx = ctx;
		}

		#endregion

		#region Properties And Methods
 
        private DateTime _shipDate = DateTime.Now;
        public DateTime ShipDate { get { return _shipDate; } set { _shipDate = value; } }
		private string _purchaseName = string.Empty;
		private string _pickupName = null;

		public int tShowTicketId
		{
			get
			{
                if (base._linkedItemIdx == 0)
					throw new System.ArgumentNullException("Show Ticket Id must be defined.");
                return base._linkedItemIdx;
			}
            set { base._linkedItemIdx = value; }
		}
		
		public string PurchaseName { get { return _purchaseName; } set { _purchaseName = value; } }

        /// <summary>
        /// Because tickets are timed - we allow them to stay in the user's cart
        /// </summary>
        public ShowTicket Ticket
        {
            get
            {
                ShowTicket _ticket = (Wcss.ShowTicket)Ctx.SaleTickets.Find(this.tShowTicketId);

                if (_ticket == null)
                {
                    //just get the object - do not worry about its activity or MP key
                    foreach (ShowDate sd in Ctx.SaleShowDates_All)
                        _ticket = (Wcss.ShowTicket)sd.ShowTicketRecords().Find(this.tShowTicketId);

                    if (_ticket == null)
                        _ticket = ShowTicket.FetchByID(this.tShowTicketId);

                    if (_ticket == null)
                        Ctx.Cart.SaleItem_Remove(_Enums.InvoiceItemContext.ticket, this.tShowTicketId);
                }

                return _ticket;
            }
        }

		//TICKET ITEMS do not have individual charges - buy 30 tickets and shipping is still x $$$ - recorded in the invoice
		public decimal ShippingCharge
		{
			get
			{
                if (this.Ticket.IsShipSeparate)
                    return this.Ticket.FlatShip;

				return 0;
			}
		}

        public decimal Price { get { return Ticket.Price; } }
        public decimal ServiceFee { get { return Ticket.ServiceCharge; } }
        /// <summary>
        /// Price + Service Charge
        /// </summary>
		public decimal PerItemPrice { get { return (this.Ticket != null) ? decimal.Round((Ticket.Price + Ticket.ServiceCharge), 2) : 0; } }
		public decimal LineTotal { get { return decimal.Round((this.PerItemPrice * this.Quantity), 2); } }
		public DateTime ItemShowDate { get { return Ticket.ShowDateRecord.DateOfShow; } }
		public bool IsDayOfShowItem { get { return (bool)(ItemShowDate.Date <= DateTime.Now.Date); } }
		
		public string PickupName
		{
			get
			{
				if(_pickupName == null)
					return string.Empty;

				return _pickupName;
			}
			set
			{
				if(value != null && value.Trim().Length > 256)
					throw new Exception("Pickup name must be less than 256 characters.");
				if(value != null)
					value = value.Trim();
				_pickupName = value;
			}
		}


		#endregion	
	}
}
