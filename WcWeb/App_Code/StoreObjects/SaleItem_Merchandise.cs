using System;
using System.Collections.Generic;

using Wcss;

namespace WillCallWeb.StoreObjects
{
	/// <summary>
	/// Summary description for Merch
	/// </summary>
	[Serializable]
	public partial class SaleItem_Merchandise : SaleItem_Base
	{
		#region Constructors

        public SaleItem_Merchandise(WebContext ctx, int merchId, int qty)
		{
			this.tMerchId = merchId;
			this.Quantity = qty;
			this.BornOnDate = DateTime.Now;
            this.TTL = BornOnDate.AddSeconds(_Config._TTL_Secs_CartItems);
			this.Ctx = ctx;
		}
		#endregion

		#region Properties And Methods

		public int tMerchId
		{
			get
			{
				if(base._linkedItemIdx == 0)
					throw new System.ArgumentNullException("Merch Id must be defined.");
                return base._linkedItemIdx;
			}
            set { base._linkedItemIdx = value; }
		}
        
        public Merch MerchItem 
        { 
            get 
            {
                Merch _merch = (Wcss.Merch)Ctx.SaleMerch.Find(this.tMerchId);

                if (_merch == null)
                    Ctx.Cart.SaleItem_Remove(_Enums.InvoiceItemContext.merch, this.tMerchId);

                return _merch; 
            }
        }
        /// <summary>
        /// The effective price of the item. If an item is on sale - provides the sale price
        /// </summary>
        public decimal Price { get { return (this.MerchItem != null) ? this.MerchItem.Price_Effective : 0; } }
		public decimal LineTotal { get { return decimal.Round((this.Price * this.Quantity), 2); } }
        public DateTime MerchandiseShipDate { get { return (this.MerchItem.IsBackordered) ? Wcss._Shipper.CalculateShipDate(this.MerchItem.BackorderDate) : Wcss._Shipper.NowShip; } }

        public bool HasParcelDelivery
        {
            get
            {
                if (this.MerchItem.IsParcelDelivery)
                    return true;

                if(this.MerchBundleSelections.Count > 0)
                    return (this.MerchBundleSelections.FindIndex(delegate(MerchBundle_Listing match) { return ((!match.IsOptOut) && match.SelectedInventory.IsParcelDelivery); }) != -1);

                return false;
            }
        }
		#endregion	
    }
}
