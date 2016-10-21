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
    /// Controller class for ShipmentBatch_InvoiceShipment
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class ShipmentBatchInvoiceShipmentController
    {
        // Preload our schema..
        ShipmentBatchInvoiceShipment thisSchemaLoad = new ShipmentBatchInvoiceShipment();
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
        public ShipmentBatchInvoiceShipmentCollection FetchAll()
        {
            ShipmentBatchInvoiceShipmentCollection coll = new ShipmentBatchInvoiceShipmentCollection();
            Query qry = new Query(ShipmentBatchInvoiceShipment.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public ShipmentBatchInvoiceShipmentCollection FetchByID(object Id)
        {
            ShipmentBatchInvoiceShipmentCollection coll = new ShipmentBatchInvoiceShipmentCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public ShipmentBatchInvoiceShipmentCollection FetchByQuery(Query qry)
        {
            ShipmentBatchInvoiceShipmentCollection coll = new ShipmentBatchInvoiceShipmentCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (ShipmentBatchInvoiceShipment.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (ShipmentBatchInvoiceShipment.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(DateTime Dtstamp,int TShipmentBatchId,int TInvoiceShipmentId)
	    {
		    ShipmentBatchInvoiceShipment item = new ShipmentBatchInvoiceShipment();
		    
            item.Dtstamp = Dtstamp;
            
            item.TShipmentBatchId = TShipmentBatchId;
            
            item.TInvoiceShipmentId = TInvoiceShipmentId;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,DateTime Dtstamp,int TShipmentBatchId,int TInvoiceShipmentId)
	    {
		    ShipmentBatchInvoiceShipment item = new ShipmentBatchInvoiceShipment();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.Dtstamp = Dtstamp;
				
			item.TShipmentBatchId = TShipmentBatchId;
				
			item.TInvoiceShipmentId = TInvoiceShipmentId;
				
	        item.Save(UserName);
	    }
    }
}
