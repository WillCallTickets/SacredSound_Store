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
    /// Controller class for InvoiceShipmentItem
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class InvoiceShipmentItemController
    {
        // Preload our schema..
        InvoiceShipmentItem thisSchemaLoad = new InvoiceShipmentItem();
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
        public InvoiceShipmentItemCollection FetchAll()
        {
            InvoiceShipmentItemCollection coll = new InvoiceShipmentItemCollection();
            Query qry = new Query(InvoiceShipmentItem.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public InvoiceShipmentItemCollection FetchByID(object Id)
        {
            InvoiceShipmentItemCollection coll = new InvoiceShipmentItemCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public InvoiceShipmentItemCollection FetchByQuery(Query qry)
        {
            InvoiceShipmentItemCollection coll = new InvoiceShipmentItemCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (InvoiceShipmentItem.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (InvoiceShipmentItem.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(DateTime? DtStamp,int TInvoiceShipmentId,int TInvoiceItemId,int IQuantity)
	    {
		    InvoiceShipmentItem item = new InvoiceShipmentItem();
		    
            item.DtStamp = DtStamp;
            
            item.TInvoiceShipmentId = TInvoiceShipmentId;
            
            item.TInvoiceItemId = TInvoiceItemId;
            
            item.IQuantity = IQuantity;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,DateTime? DtStamp,int TInvoiceShipmentId,int TInvoiceItemId,int IQuantity)
	    {
		    InvoiceShipmentItem item = new InvoiceShipmentItem();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.DtStamp = DtStamp;
				
			item.TInvoiceShipmentId = TInvoiceShipmentId;
				
			item.TInvoiceItemId = TInvoiceItemId;
				
			item.IQuantity = IQuantity;
				
	        item.Save(UserName);
	    }
    }
}
