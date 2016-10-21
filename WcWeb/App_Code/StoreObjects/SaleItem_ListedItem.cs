using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for SaleItem_ListedItem
/// </summary>
namespace WillCallWeb.StoreObjects
{
    public class SaleItem_Listeditem
    {
        /// <summary>
        /// Context will help us track if we need to display an item timer or not
        /// </summary>
        public Wcss._Enums.InvoiceItemContext Context { get; set; }
        public int ItemProductId { get; set; }
        public bool IsExtended { get; set; }
        public DateTime Ttl { get; set; }
        public string PriceEach { get; set; }
        public string Quantity { get; set; }
        public string LineTotal { get; set; }
        public string Description { get; set; }

        public SaleItem_Listeditem(Wcss._Enums.InvoiceItemContext context, int itemProductId, bool isExtended, DateTime ttl, 
            string priceEach, string quantity, string lineTotal, string description)
        {
            Context = context;
            ItemProductId = itemProductId;
            IsExtended = isExtended;
            Ttl = ttl;
            PriceEach = priceEach ?? string.Empty;
            Quantity = quantity ?? string.Empty;
            LineTotal = lineTotal ?? string.Empty;
            Description = description ?? string.Empty;
        }
    }
}