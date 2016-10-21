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
    /// Controller class for TicketStock
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class TicketStockController
    {
        // Preload our schema..
        TicketStock thisSchemaLoad = new TicketStock();
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
        public TicketStockCollection FetchAll()
        {
            TicketStockCollection coll = new TicketStockCollection();
            Query qry = new Query(TicketStock.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public TicketStockCollection FetchByID(object Guid)
        {
            TicketStockCollection coll = new TicketStockCollection().Where("GUID", Guid).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public TicketStockCollection FetchByQuery(Query qry)
        {
            TicketStockCollection coll = new TicketStockCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Guid)
        {
            return (TicketStock.Delete(Guid) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Guid)
        {
            return (TicketStock.Destroy(Guid) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(Guid Guid,string SessionId,int? TInvoiceId,string UserName,int TShowTicketId,int IQty,DateTime DtTTL,DateTime DtStamp)
	    {
		    TicketStock item = new TicketStock();
		    
            item.Guid = Guid;
            
            item.SessionId = SessionId;
            
            item.TInvoiceId = TInvoiceId;
            
            item.UserName = UserName;
            
            item.TShowTicketId = TShowTicketId;
            
            item.IQty = IQty;
            
            item.DtTTL = DtTTL;
            
            item.DtStamp = DtStamp;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,Guid Guid,string SessionId,int? TInvoiceId,string UserName,int TShowTicketId,int IQty,DateTime DtTTL,DateTime DtStamp)
	    {
		    TicketStock item = new TicketStock();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.Guid = Guid;
				
			item.SessionId = SessionId;
				
			item.TInvoiceId = TInvoiceId;
				
			item.UserName = UserName;
				
			item.TShowTicketId = TShowTicketId;
				
			item.IQty = IQty;
				
			item.DtTTL = DtTTL;
				
			item.DtStamp = DtStamp;
				
	        item.Save(UserName);
	    }
    }
}
