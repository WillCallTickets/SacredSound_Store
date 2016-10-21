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
    /// Controller class for StoreCredit
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class StoreCreditController
    {
        // Preload our schema..
        StoreCredit thisSchemaLoad = new StoreCredit();
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
        public StoreCreditCollection FetchAll()
        {
            StoreCreditCollection coll = new StoreCreditCollection();
            Query qry = new Query(StoreCredit.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public StoreCreditCollection FetchByID(object Id)
        {
            StoreCreditCollection coll = new StoreCreditCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public StoreCreditCollection FetchByQuery(Query qry)
        {
            StoreCreditCollection coll = new StoreCreditCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (StoreCredit.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (StoreCredit.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(DateTime DtStamp,Guid ApplicationId,decimal MAmount,Guid? RedemptionId,int? TInvoiceTransactionId,string Comment,Guid? UserId)
	    {
		    StoreCredit item = new StoreCredit();
		    
            item.DtStamp = DtStamp;
            
            item.ApplicationId = ApplicationId;
            
            item.MAmount = MAmount;
            
            item.RedemptionId = RedemptionId;
            
            item.TInvoiceTransactionId = TInvoiceTransactionId;
            
            item.Comment = Comment;
            
            item.UserId = UserId;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,DateTime DtStamp,Guid ApplicationId,decimal MAmount,Guid? RedemptionId,int? TInvoiceTransactionId,string Comment,Guid? UserId)
	    {
		    StoreCredit item = new StoreCredit();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.DtStamp = DtStamp;
				
			item.ApplicationId = ApplicationId;
				
			item.MAmount = MAmount;
				
			item.RedemptionId = RedemptionId;
				
			item.TInvoiceTransactionId = TInvoiceTransactionId;
				
			item.Comment = Comment;
				
			item.UserId = UserId;
				
	        item.Save(UserName);
	    }
    }
}
