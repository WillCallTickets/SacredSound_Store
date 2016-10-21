using System;
using System.Collections.Generic;

using Wcss;

namespace WillCallWeb.StoreObjects
{
    [Serializable]
    public partial class SaleItem_Base
    {
        private WebContext _ctx;
        private System.Guid _guid;
        private int _qty = 0;
        private DateTime _bornOnDate = Utils.Constants._MinDate;
        private bool _expiryRefreshed = false;
        private string _status = string.Empty;
        protected int _linkedItemIdx = 0;

        public WebContext Ctx
        {
            get
            {
                return _ctx;
            }
            set { _ctx = value; }
        }
        public System.Guid GUID
        {
            get
            {
                if (_guid == System.Guid.Empty)
                    _guid = System.Guid.NewGuid();
                return _guid;
            }
            set { _guid = value; }
        }
        public int Quantity { get { return _qty; } set { _qty = value; } }
        public DateTime BornOnDate
        {
            get
            {
                return _bornOnDate;
            }
            set { _bornOnDate = value; }
        }
        private DateTime _ttl = Utils.Constants._MinDate;
        public DateTime TTL
        {
            get
            {
                if (_ttl == Utils.Constants._MinDate)
                    _ttl = BornOnDate.AddSeconds(_Config._TTL_Secs_CartItems);

                return _ttl;
            }
            set
            {
                _ttl = value;
            }
        }
        public bool TTL_Extended
        {
            get
            {
                TimeSpan diff = TTL.Subtract(BornOnDate);
                int secs = (diff.Days * 24 * 60 * 60) + (diff.Hours * 60 * 60) + (diff.Minutes * 60) + diff.Seconds;
                if (secs > 0 && secs > _Config._TTL_Secs_CartItems)
                    return true;

                return false;
            }
        }
        public bool TTL_Expired { get { return TTL < DateTime.Now; } }
        public bool IsExpiryRefreshed { get { return _expiryRefreshed; } set { _expiryRefreshed = value; } }

        //public string Status { get { return _status; } set { _status = value; } }

        public void AddMoreTimeToLive()
        {
            if (!TTL_Extended && !TTL_Expired)
            {
                //add more time to the stock item
                //this proc returns rows affected - if zero let's log, because this shouldn't happen
                //only do this for tickets
                string context = string.Empty;

                if (this.GetType() == typeof(SaleItem_Ticket))
                {
                    TTL = TTL.AddSeconds(_Config._TTL_Secs_Extend);
                    context = _Enums.InvoiceItemContext.ticket.ToString();

                    //this proc only looks for tix and other(merch)
                    object ret = SPs.TxInventoryPendingTimeUpdate(this.GUID, context, this.TTL).ExecuteScalar();

                    string rows = (ret == null) ? "-1" : ret.ToString();
                    if ((!Utils.Validation.IsInteger(rows)) || int.Parse(rows) <= 0)
                    {
                        string fm = string.Format("{0}: {1}Stock not found for SaleItem_{1} when adding time \r\nGuid: {2}\r\n",
                            DateTime.Now, context, this.GUID);
                        fm += string.Format("{0}Id: {1} Qty: {2} ", context, this._linkedItemIdx, this.Quantity);
                        fm += string.Format("TTL: {0} DtStamp: {1} ", this.TTL.ToString("MM/dd/yyyy hh:mm:ssstt"), this.BornOnDate
                            .ToString("MM/dd/yyyy hh:mm:ssstt"));

                        _Error.LogException(new Exception(fm.ToString()));
                    }
                }
            }
        }
        
        public bool HasSelectedAllAvailableBundleItems(bool considerPricedBundles)
        {
            //only merch and tickets can have bundles
            SaleItem_Merchandise sim = this as SaleItem_Merchandise;
            SaleItem_Ticket sit = this as SaleItem_Ticket;
            List<MerchBundle> bundles = new List<MerchBundle>();

            if (sim != null)
            {
                bundles.AddRange(sim.MerchItem.ParentMerchRecord.MerchBundleRecords().Get_MerchBundleRecords_RunningAndAvailable());
            }
            else if (sit != null)
            {
                bundles.AddRange(sit.Ticket.MerchBundleRecords().Get_MerchBundleRecords_RunningAndAvailable());
            }

            if (bundles.Count > 0)
            {
                //remove any auto-select bundles
                bundles.RemoveAll(delegate(MerchBundle match) { return (match.HasOnlyOneAvailableSelection); });

                if (!considerPricedBundles)
                    bundles.RemoveAll(delegate(MerchBundle match) { return (match.OffersOptout); });

                foreach (MerchBundle bundle in bundles)
                {
                    //determine max seletcions available                    
                    //determine selections selected

                    List<MerchBundle_Listing> selections = this.GetValidMerchBundleListings_Selected(bundle.Id);

                    int maxSelectionsAllowed = Ctx.Cart.GetMaxPossibleSelectionsAllowedForBundle(this, bundle.Id);
                    int qtySelected = WillCallWeb.StoreObjects.SaleItem_Services.GetQtySelected(selections);

                    if (qtySelected < maxSelectionsAllowed)
                        return false;
                }
            }

            return true;
        }
    }
}
