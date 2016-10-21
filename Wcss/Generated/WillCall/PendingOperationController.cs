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
    /// Controller class for PendingOperation
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class PendingOperationController
    {
        // Preload our schema..
        PendingOperation thisSchemaLoad = new PendingOperation();
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
        public PendingOperationCollection FetchAll()
        {
            PendingOperationCollection coll = new PendingOperationCollection();
            Query qry = new Query(PendingOperation.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public PendingOperationCollection FetchByID(object Id)
        {
            PendingOperationCollection coll = new PendingOperationCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public PendingOperationCollection FetchByQuery(Query qry)
        {
            PendingOperationCollection coll = new PendingOperationCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (PendingOperation.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (PendingOperation.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(DateTime DtStamp,Guid ApplicationId,int IdentifierId,DateTime DtValidUntil,string VcContext,string UserName,string Criteria)
	    {
		    PendingOperation item = new PendingOperation();
		    
            item.DtStamp = DtStamp;
            
            item.ApplicationId = ApplicationId;
            
            item.IdentifierId = IdentifierId;
            
            item.DtValidUntil = DtValidUntil;
            
            item.VcContext = VcContext;
            
            item.UserName = UserName;
            
            item.Criteria = Criteria;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,DateTime DtStamp,Guid ApplicationId,int IdentifierId,DateTime DtValidUntil,string VcContext,string UserName,string Criteria)
	    {
		    PendingOperation item = new PendingOperation();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.DtStamp = DtStamp;
				
			item.ApplicationId = ApplicationId;
				
			item.IdentifierId = IdentifierId;
				
			item.DtValidUntil = DtValidUntil;
				
			item.VcContext = VcContext;
				
			item.UserName = UserName;
				
			item.Criteria = Criteria;
				
	        item.Save(UserName);
	    }
    }
}
