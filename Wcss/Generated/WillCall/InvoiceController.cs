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
    /// Controller class for Invoice
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class InvoiceController
    {
        // Preload our schema..
        Invoice thisSchemaLoad = new Invoice();
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
        public InvoiceCollection FetchAll()
        {
            InvoiceCollection coll = new InvoiceCollection();
            Query qry = new Query(Invoice.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public InvoiceCollection FetchByID(object Id)
        {
            InvoiceCollection coll = new InvoiceCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public InvoiceCollection FetchByQuery(Query qry)
        {
            InvoiceCollection coll = new InvoiceCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (Invoice.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (Invoice.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string UniqueId,int TVendorId,string PurchaseEmail,Guid UserId,int CustomerId,DateTime DtInvoiceDate,string Creator,string OrderType,string VcProducts,decimal MBalanceDue,decimal MTotalPaid,decimal MTotalRefunds,decimal? MNetPaid,string InvoiceStatus,int? TCashewId,string MarketingKeys,DateTime DtStamp,Guid ApplicationId)
	    {
		    Invoice item = new Invoice();
		    
            item.UniqueId = UniqueId;
            
            item.TVendorId = TVendorId;
            
            item.PurchaseEmail = PurchaseEmail;
            
            item.UserId = UserId;
            
            item.CustomerId = CustomerId;
            
            item.DtInvoiceDate = DtInvoiceDate;
            
            item.Creator = Creator;
            
            item.OrderType = OrderType;
            
            item.VcProducts = VcProducts;
            
            item.MBalanceDue = MBalanceDue;
            
            item.MTotalPaid = MTotalPaid;
            
            item.MTotalRefunds = MTotalRefunds;
            
            item.MNetPaid = MNetPaid;
            
            item.InvoiceStatus = InvoiceStatus;
            
            item.TCashewId = TCashewId;
            
            item.MarketingKeys = MarketingKeys;
            
            item.DtStamp = DtStamp;
            
            item.ApplicationId = ApplicationId;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,string UniqueId,int TVendorId,string PurchaseEmail,Guid UserId,int CustomerId,DateTime DtInvoiceDate,string Creator,string OrderType,string VcProducts,decimal MBalanceDue,decimal MTotalPaid,decimal MTotalRefunds,decimal? MNetPaid,string InvoiceStatus,int? TCashewId,string MarketingKeys,DateTime DtStamp,Guid ApplicationId)
	    {
		    Invoice item = new Invoice();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.UniqueId = UniqueId;
				
			item.TVendorId = TVendorId;
				
			item.PurchaseEmail = PurchaseEmail;
				
			item.UserId = UserId;
				
			item.CustomerId = CustomerId;
				
			item.DtInvoiceDate = DtInvoiceDate;
				
			item.Creator = Creator;
				
			item.OrderType = OrderType;
				
			item.VcProducts = VcProducts;
				
			item.MBalanceDue = MBalanceDue;
				
			item.MTotalPaid = MTotalPaid;
				
			item.MTotalRefunds = MTotalRefunds;
				
			item.MNetPaid = MNetPaid;
				
			item.InvoiceStatus = InvoiceStatus;
				
			item.TCashewId = TCashewId;
				
			item.MarketingKeys = MarketingKeys;
				
			item.DtStamp = DtStamp;
				
			item.ApplicationId = ApplicationId;
				
	        item.Save(UserName);
	    }
    }
}
