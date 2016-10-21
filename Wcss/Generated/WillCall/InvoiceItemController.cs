using System; 
using System.Text; 
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration; 
using System.Xml; 
using System.Xml.Serialization;
using SubSonic; 
using SubSonic.Utilities;
namespace Wcss
{
    /// <summary>
    /// Controller class for InvoiceItem
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class InvoiceItemController
    {
        // Preload our schema..
        InvoiceItem thisSchemaLoad = new InvoiceItem();
        private string userName = String.Empty;
        protected string UserName
        {
            get
            {
				if (userName.Length == 0) 
				{
    				if (System.Web.HttpContext.Current != null)
    				{
						userName=System.Web.HttpContext.Current.User.Identity.Name;
					}
					else
					{
						userName=System.Threading.Thread.CurrentPrincipal.Identity.Name;
					}
				}
				return userName;
            }
        }
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public InvoiceItemCollection FetchAll()
        {
            InvoiceItemCollection coll = new InvoiceItemCollection();
            Query qry = new Query(InvoiceItem.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public InvoiceItemCollection FetchByID(object Id)
        {
            InvoiceItemCollection coll = new InvoiceItemCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public InvoiceItemCollection FetchByQuery(Query qry)
        {
            InvoiceItemCollection coll = new InvoiceItemCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (InvoiceItem.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (InvoiceItem.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(Guid Guid,int TInvoiceId,string VcContext,int? TShowTicketId,int? TMerchId,int? TShowId,int? TShipItemId,int? TSalePromotionId,string PurchaseName,DateTime? DtDateOfShow,string AgeDescription,string MainActName,string Criteria,string Description,decimal MPrice,decimal MServiceCharge,decimal MAdjustment,decimal? MPricePerItem,int IQuantity,decimal? MLineItemTotal,string PurchaseAction,string Notes,string PickupName,bool? BRTS,DateTime? DtShipped,string ShippingNotes,string ShippingMethod,DateTime DtStamp)
	    {
		    InvoiceItem item = new InvoiceItem();
		    
            item.Guid = Guid;
            
            item.TInvoiceId = TInvoiceId;
            
            item.VcContext = VcContext;
            
            item.TShowTicketId = TShowTicketId;
            
            item.TMerchId = TMerchId;
            
            item.TShowId = TShowId;
            
            item.TShipItemId = TShipItemId;
            
            item.TSalePromotionId = TSalePromotionId;
            
            item.PurchaseName = PurchaseName;
            
            item.DtDateOfShow = DtDateOfShow;
            
            item.AgeDescription = AgeDescription;
            
            item.MainActName = MainActName;
            
            item.Criteria = Criteria;
            
            item.Description = Description;
            
            item.MPrice = MPrice;
            
            item.MServiceCharge = MServiceCharge;
            
            item.MAdjustment = MAdjustment;
            
            item.MPricePerItem = MPricePerItem;
            
            item.IQuantity = IQuantity;
            
            item.MLineItemTotal = MLineItemTotal;
            
            item.PurchaseAction = PurchaseAction;
            
            item.Notes = Notes;
            
            item.PickupName = PickupName;
            
            item.BRTS = BRTS;
            
            item.DtShipped = DtShipped;
            
            item.ShippingNotes = ShippingNotes;
            
            item.ShippingMethod = ShippingMethod;
            
            item.DtStamp = DtStamp;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,Guid Guid,int TInvoiceId,string VcContext,int? TShowTicketId,int? TMerchId,int? TShowId,int? TShipItemId,int? TSalePromotionId,string PurchaseName,DateTime? DtDateOfShow,string AgeDescription,string MainActName,string Criteria,string Description,decimal MPrice,decimal MServiceCharge,decimal MAdjustment,decimal? MPricePerItem,int IQuantity,decimal? MLineItemTotal,string PurchaseAction,string Notes,string PickupName,bool? BRTS,DateTime? DtShipped,string ShippingNotes,string ShippingMethod,DateTime DtStamp)
	    {
		    InvoiceItem item = new InvoiceItem();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.Guid = Guid;
				
			item.TInvoiceId = TInvoiceId;
				
			item.VcContext = VcContext;
				
			item.TShowTicketId = TShowTicketId;
				
			item.TMerchId = TMerchId;
				
			item.TShowId = TShowId;
				
			item.TShipItemId = TShipItemId;
				
			item.TSalePromotionId = TSalePromotionId;
				
			item.PurchaseName = PurchaseName;
				
			item.DtDateOfShow = DtDateOfShow;
				
			item.AgeDescription = AgeDescription;
				
			item.MainActName = MainActName;
				
			item.Criteria = Criteria;
				
			item.Description = Description;
				
			item.MPrice = MPrice;
				
			item.MServiceCharge = MServiceCharge;
				
			item.MAdjustment = MAdjustment;
				
			item.MPricePerItem = MPricePerItem;
				
			item.IQuantity = IQuantity;
				
			item.MLineItemTotal = MLineItemTotal;
				
			item.PurchaseAction = PurchaseAction;
				
			item.Notes = Notes;
				
			item.PickupName = PickupName;
				
			item.BRTS = BRTS;
				
			item.DtShipped = DtShipped;
				
			item.ShippingNotes = ShippingNotes;
				
			item.ShippingMethod = ShippingMethod;
				
			item.DtStamp = DtStamp;
				
	        item.Save(UserName);
	    }
    }
}
