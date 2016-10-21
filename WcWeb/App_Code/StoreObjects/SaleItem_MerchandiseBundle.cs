using System;
using System.Collections.Generic;
using Wcss;

namespace WillCallWeb.StoreObjects
{
    public partial class SaleItem_Base
    {
        private List<MerchBundle_Listing> _merchBundleSelections = null;
        public List<MerchBundle_Listing> MerchBundleSelections
        {
            get
            {
                if (_merchBundleSelections == null 
                    &&
                    ((this is WillCallWeb.StoreObjects.SaleItem_Merchandise) || (this is WillCallWeb.StoreObjects.SaleItem_Ticket))
                    )
                {
                    _merchBundleSelections = new List<MerchBundle_Listing>();
                }

                return _merchBundleSelections;
            }
        }

        public MerchBundle GetMerchBundle(int bundleId)
        {
            if(this is SaleItem_Merchandise)
                return (MerchBundle)((SaleItem_Merchandise)this).MerchItem.ParentMerchRecord.MerchBundleRecords().Find(bundleId);
            else if (this is SaleItem_Ticket)
                return (MerchBundle)((SaleItem_Ticket)this).Ticket.MerchBundleRecords().Find(bundleId);

            return null;
        }

        public List<MerchBundle_Listing> GetValidMerchBundleListings_Selected(int bundleId)
        {
            List<MerchBundle_Listing> selections = new List<MerchBundle_Listing>();

            if (this.MerchBundleSelections != null)
                selections.AddRange(this.MerchBundleSelections.FindAll(delegate(MerchBundle_Listing match) 
                    { return match.SelectedInventoryId > 0 && match.Quantity > 0 && match.BundleId == bundleId && (!match.IsOptOut); }));

            return selections;
        }

        /// <summary>
        /// determine how many selections need to be removed
        /// </summary>
        /// <param name="?"></param>
        public void RemoveSelectionsOverQuota(MerchBundle bundle)
        {
            System.Collections.Generic.List<MerchBundle_Listing> selections = new List<MerchBundle_Listing>();

            if (bundle.HasOnlyOneAvailableSelection)
            {
                //TODO: does this clear out underlying object?
                this.MerchBundleSelections.RemoveAll(delegate(MerchBundle_Listing match) { return match.BundleId == bundle.Id; });

                //determine how many should be in the cart - how many bundles are qualified
                int allowed = Ctx.Cart.GetMaxPossibleSelectionsAllowedForBundle(this, bundle.Id);
                int onlyId = bundle.GetOnlyAvailableSelectiondId();

                if (onlyId > 0 && allowed > 0)
                    this.MerchBundleSelections.Add(new MerchBundle_Listing(0, bundle, onlyId, allowed));
            }
            else
            {
                selections.AddRange(this.GetValidMerchBundleListings_Selected(bundle.Id));

                //get current count of selections
                int selectionQty = 0;
                foreach (MerchBundle_Listing listing in selections)
                    selectionQty += listing.Quantity;

                //compare to max selections allowed
                int maxSelections = Ctx.Cart.GetMaxPossibleSelectionsAllowedForBundle(this, bundle.Id);

                //if we are over - then start removing selections
                if (selectionQty > maxSelections)
                {
                    int runningTotal = 0;

                    //loop through the selections
                    for (int i = 0; i < selections.Count; i++)
                    {
                        MerchBundle_Listing selection = selections[i];

                        if (runningTotal >= maxSelections)
                            selection.Quantity = 0;
                        else
                        {
                            if (runningTotal + selection.Quantity <= maxSelections)
                                runningTotal += selection.Quantity;
                            else
                            {
                                int diff = maxSelections - runningTotal;
                                selection.Quantity = diff;
                                runningTotal = maxSelections;
                            }
                        }
                    }

                    //remove selections with zero quantities
                    this.MerchBundleSelections.RemoveAll(delegate(MerchBundle_Listing match) { return match.Quantity <= 0; });
                }
            }
        }

        public decimal GetIndividualBundlePrice(MerchBundle bundle)
        {
            int chargeInstances = 0;
            return GetIndividualBundlePrice(bundle, out chargeInstances);
        }
        public decimal GetIndividualBundlePrice(MerchBundle bundle, out int chargeInstances)
        {
            decimal price = 0;
            chargeInstances = 0;

            System.Collections.Generic.List<MerchBundle_Listing> selections = new List<MerchBundle_Listing>();
            selections.AddRange(this.MerchBundleSelections.FindAll(delegate(MerchBundle_Listing match) { return ((!match.IsOptOut) && match.BundleId == bundle.Id); }));

            int selectionCount = selections.Count;

            if (selectionCount > 0)
            {
                int selectionQty = 0;
                int maxSelections = bundle.MaxSelections;

                foreach (MerchBundle_Listing listing in selections)
                    selectionQty += listing.Quantity;

                //avoid divide by zero
                if (selectionQty > 0 && maxSelections > 0)
                {
                    if (bundle.PricedPerSelection)
                    {
                        chargeInstances = selectionQty;
                    }
                    else
                    {
                        int overage = 0;

                        //test for partials - does it return zero?
                        chargeInstances = System.Math.DivRem(selectionQty, maxSelections, out overage);

                        if (overage > 0)
                            chargeInstances += 1;
                    }

                    price += bundle.Price * chargeInstances;
                }
            }

            return price;
        }
        private List<MerchBundle> _activeMerchBundles = null;
        public List<MerchBundle> GetActiveMerchBundleList
        {
            get
            {
                if(_activeMerchBundles == null)
                {
                    _activeMerchBundles = new List<MerchBundle>();

                    if (this is SaleItem_Merchandise)
                        _activeMerchBundles.AddRange(((SaleItem_Merchandise)this).MerchItem.ParentMerchRecord.MerchBundleRecords().Get_MerchBundleRecords_RunningAndAvailable());
                    else if (this is SaleItem_Ticket)
                        _activeMerchBundles.AddRange(((SaleItem_Ticket)this).Ticket.MerchBundleRecords().Get_MerchBundleRecords_RunningAndAvailable());
                }

                return _activeMerchBundles;
            }
        }


        public decimal Cart_Bundle_Price
        {
            get
            {
                decimal price = 0;

                if (this.MerchBundleSelections != null && this.MerchBundleSelections.Count > 0)
                {
                    //get a list of distinct MerchBundles
                    List<MerchBundle> bundles = new List<MerchBundle>();
                    if (this is SaleItem_Merchandise && ((SaleItem_Merchandise)this).MerchItem != null)
                        bundles.AddRange(((SaleItem_Merchandise)this).MerchItem.ParentMerchRecord.MerchBundleRecords());
                    else if (this is SaleItem_Ticket && ((SaleItem_Ticket)this).Ticket != null)
                        bundles.AddRange(((SaleItem_Ticket)this).Ticket.MerchBundleRecords());

                    //remove bundles that are not priced
                    bundles.RemoveAll(delegate(MerchBundle match) { return (!match.OffersOptout); } );

                    if (bundles.Count > 0)
                    {
                        foreach (MerchBundle bundle in bundles)
                            price += GetIndividualBundlePrice(bundle);
                    }
                }

                return price;
            }
        }

        public decimal Cart_Bundle_Weight
        {
            get
            {
                decimal weight = 0;

                if (this.MerchBundleSelections != null)
                {
                    foreach (MerchBundle_Listing listing in this.MerchBundleSelections)
                    {
                        if (listing.SelectedInventory != null && listing.Quantity > 0 && listing.MerchBundleRecord.IncludeWeight && 
                            listing.SelectedInventory.IsParcelDelivery && listing.SelectedInventory.Weight > 0)
                            weight += listing.SelectedInventory.Weight * listing.Quantity;
                    }
                }

                return weight;
            }
        }
    }

    [Serializable]
    public class MerchBundle_Listing_WithPrice_ForInvoiceItemInsert : MerchBundle_Listing
    {
        public decimal PricePerItem { get; set; }

        public MerchBundle_Listing_WithPrice_ForInvoiceItemInsert(decimal pricePerItem, int qty, MerchBundle_Listing listing)
            : base(listing.Ordinal, listing.MerchBundleRecord, listing.SelectedInventoryId, qty)
        {
            PricePerItem = pricePerItem;
        }
    }
    /// <summary>
    /// The constructors handle 2 use cases. With a MerchBundleRecord, we are tracking an item
    /// to be listed in a control. The MerchBundleRecord is needed to provide a list of inventory items.
    /// The second case is just to track selections made by the user. In this case, we do not need 
    /// to track the MerchBundleRecord, but just the id of the bundle. This makes it lighter weight 
    /// when serializing
    /// </summary>
    [Serializable]
    public class MerchBundle_Listing
    {
        public int Ordinal { get; set; }
        public int BundleId
        {
            get
            {
                return MerchBundleRecord.Id;
            }
        }
        public int SelectedInventoryId { get; set; }
        public int Quantity { get; set; }
        public MerchBundle MerchBundleRecord { get; set; }
        public Merch SelectedInventory 
        {
            get
            {
                if (SelectedInventoryId == 0 || this.Quantity == 0)
                    return null;

                return (Merch)MerchBundleRecord.ActiveInventory.Find(SelectedInventoryId);
            }
        }
        
        public MerchBundle_Listing(int ordinal, MerchBundle bundle, int selectedInventoryId, int qty)
        {
            Ordinal = ordinal;
            MerchBundleRecord = bundle;
            SelectedInventoryId = selectedInventoryId;
            Quantity = qty;
        }

        public override string ToString()
        {
            return MerchBundle_Listing.FormatListing(this.Ordinal, this.BundleId, this.SelectedInventoryId, this.Quantity);
        }       
 
        public bool IsOptOut
        {
            get
            {
                return (this.SelectedInventoryId == 0 || this.Quantity == 0);
            }
        }

        public static string FormatOptOut(int ordinal, MerchBundle bundle)
        {
            return MerchBundle_Listing.FormatListing(ordinal, bundle.Id, 0, 0);
        }
        private static string FormatListing(int ordinal, int bundleId, int selectedId, int qty)
        {
            return string.Format("{0}~{1}~{2}~{3}", ordinal.ToString(), bundleId.ToString(),
                selectedId.ToString(), qty.ToString());
        }
    }
}
