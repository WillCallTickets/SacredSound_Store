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
    /// Controller class for ShipmentBatch
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class ShipmentBatchController
    {
        // Preload our schema..
        ShipmentBatch thisSchemaLoad = new ShipmentBatch();
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
        public ShipmentBatchCollection FetchAll()
        {
            ShipmentBatchCollection coll = new ShipmentBatchCollection();
            Query qry = new Query(ShipmentBatch.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public ShipmentBatchCollection FetchByID(object Id)
        {
            ShipmentBatchCollection coll = new ShipmentBatchCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public ShipmentBatchCollection FetchByQuery(Query qry)
        {
            ShipmentBatchCollection coll = new ShipmentBatchCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (ShipmentBatch.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (ShipmentBatch.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(DateTime DtStamp,Guid ApplicationId,string BatchId,string Name,string Description,int? EventId,string CsvEventTix,string CsvOtherTix,string CsvMethods,DateTime? DtEstShipDate)
	    {
		    ShipmentBatch item = new ShipmentBatch();
		    
            item.DtStamp = DtStamp;
            
            item.ApplicationId = ApplicationId;
            
            item.BatchId = BatchId;
            
            item.Name = Name;
            
            item.Description = Description;
            
            item.EventId = EventId;
            
            item.CsvEventTix = CsvEventTix;
            
            item.CsvOtherTix = CsvOtherTix;
            
            item.CsvMethods = CsvMethods;
            
            item.DtEstShipDate = DtEstShipDate;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,DateTime DtStamp,Guid ApplicationId,string BatchId,string Name,string Description,int? EventId,string CsvEventTix,string CsvOtherTix,string CsvMethods,DateTime? DtEstShipDate)
	    {
		    ShipmentBatch item = new ShipmentBatch();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.DtStamp = DtStamp;
				
			item.ApplicationId = ApplicationId;
				
			item.BatchId = BatchId;
				
			item.Name = Name;
				
			item.Description = Description;
				
			item.EventId = EventId;
				
			item.CsvEventTix = CsvEventTix;
				
			item.CsvOtherTix = CsvOtherTix;
				
			item.CsvMethods = CsvMethods;
				
			item.DtEstShipDate = DtEstShipDate;
				
	        item.Save(UserName);
	    }
    }
}
