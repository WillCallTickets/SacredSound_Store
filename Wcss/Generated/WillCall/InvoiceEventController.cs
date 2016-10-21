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
    /// Controller class for InvoiceEvent
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class InvoiceEventController
    {
        // Preload our schema..
        InvoiceEvent thisSchemaLoad = new InvoiceEvent();
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
        public InvoiceEventCollection FetchAll()
        {
            InvoiceEventCollection coll = new InvoiceEventCollection();
            Query qry = new Query(InvoiceEvent.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public InvoiceEventCollection FetchByID(object Id)
        {
            InvoiceEventCollection coll = new InvoiceEventCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public InvoiceEventCollection FetchByQuery(Query qry)
        {
            InvoiceEventCollection coll = new InvoiceEventCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (InvoiceEvent.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (InvoiceEvent.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(int TInvoiceId,int TEventQId,DateTime? DtStamp)
	    {
		    InvoiceEvent item = new InvoiceEvent();
		    
            item.TInvoiceId = TInvoiceId;
            
            item.TEventQId = TEventQId;
            
            item.DtStamp = DtStamp;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,int TInvoiceId,int TEventQId,DateTime? DtStamp)
	    {
		    InvoiceEvent item = new InvoiceEvent();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.TInvoiceId = TInvoiceId;
				
			item.TEventQId = TEventQId;
				
			item.DtStamp = DtStamp;
				
	        item.Save(UserName);
	    }
    }
}
