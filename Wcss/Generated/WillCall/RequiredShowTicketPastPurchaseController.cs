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
    /// Controller class for Required_ShowTicket_PastPurchase
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class RequiredShowTicketPastPurchaseController
    {
        // Preload our schema..
        RequiredShowTicketPastPurchase thisSchemaLoad = new RequiredShowTicketPastPurchase();
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
        public RequiredShowTicketPastPurchaseCollection FetchAll()
        {
            RequiredShowTicketPastPurchaseCollection coll = new RequiredShowTicketPastPurchaseCollection();
            Query qry = new Query(RequiredShowTicketPastPurchase.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public RequiredShowTicketPastPurchaseCollection FetchByID(object Id)
        {
            RequiredShowTicketPastPurchaseCollection coll = new RequiredShowTicketPastPurchaseCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public RequiredShowTicketPastPurchaseCollection FetchByQuery(Query qry)
        {
            RequiredShowTicketPastPurchaseCollection coll = new RequiredShowTicketPastPurchaseCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (RequiredShowTicketPastPurchase.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (RequiredShowTicketPastPurchase.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(DateTime DtStamp,int TShowTicketId,int TRequiredId,bool BLimitQtyToPastQty)
	    {
		    RequiredShowTicketPastPurchase item = new RequiredShowTicketPastPurchase();
		    
            item.DtStamp = DtStamp;
            
            item.TShowTicketId = TShowTicketId;
            
            item.TRequiredId = TRequiredId;
            
            item.BLimitQtyToPastQty = BLimitQtyToPastQty;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,DateTime DtStamp,int TShowTicketId,int TRequiredId,bool BLimitQtyToPastQty)
	    {
		    RequiredShowTicketPastPurchase item = new RequiredShowTicketPastPurchase();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.DtStamp = DtStamp;
				
			item.TShowTicketId = TShowTicketId;
				
			item.TRequiredId = TRequiredId;
				
			item.BLimitQtyToPastQty = BLimitQtyToPastQty;
				
	        item.Save(UserName);
	    }
    }
}
