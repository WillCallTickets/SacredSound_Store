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
    /// Controller class for HistoryInventory
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class HistoryInventoryController
    {
        // Preload our schema..
        HistoryInventory thisSchemaLoad = new HistoryInventory();
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
        public HistoryInventoryCollection FetchAll()
        {
            HistoryInventoryCollection coll = new HistoryInventoryCollection();
            Query qry = new Query(HistoryInventory.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public HistoryInventoryCollection FetchByID(object Id)
        {
            HistoryInventoryCollection coll = new HistoryInventoryCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public HistoryInventoryCollection FetchByQuery(Query qry)
        {
            HistoryInventoryCollection coll = new HistoryInventoryCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (HistoryInventory.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (HistoryInventory.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(DateTime DtStamp,Guid? UserId,int? TMerchId,int? TShowTicketId,DateTime DtAdjusted,int ICurrentlyAllotted,int IAdjustment,string VcContext,string Description)
	    {
		    HistoryInventory item = new HistoryInventory();
		    
            item.DtStamp = DtStamp;
            
            item.UserId = UserId;
            
            item.TMerchId = TMerchId;
            
            item.TShowTicketId = TShowTicketId;
            
            item.DtAdjusted = DtAdjusted;
            
            item.ICurrentlyAllotted = ICurrentlyAllotted;
            
            item.IAdjustment = IAdjustment;
            
            item.VcContext = VcContext;
            
            item.Description = Description;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,DateTime DtStamp,Guid? UserId,int? TMerchId,int? TShowTicketId,DateTime DtAdjusted,int ICurrentlyAllotted,int IAdjustment,string VcContext,string Description)
	    {
		    HistoryInventory item = new HistoryInventory();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.DtStamp = DtStamp;
				
			item.UserId = UserId;
				
			item.TMerchId = TMerchId;
				
			item.TShowTicketId = TShowTicketId;
				
			item.DtAdjusted = DtAdjusted;
				
			item.ICurrentlyAllotted = ICurrentlyAllotted;
				
			item.IAdjustment = IAdjustment;
				
			item.VcContext = VcContext;
				
			item.Description = Description;
				
	        item.Save(UserName);
	    }
    }
}
