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
    /// Controller class for FB_Stat
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class FbStatController
    {
        // Preload our schema..
        FbStat thisSchemaLoad = new FbStat();
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
        public FbStatCollection FetchAll()
        {
            FbStatCollection coll = new FbStatCollection();
            Query qry = new Query(FbStat.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public FbStatCollection FetchByID(object Id)
        {
            FbStatCollection coll = new FbStatCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public FbStatCollection FetchByQuery(Query qry)
        {
            FbStatCollection coll = new FbStatCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (FbStat.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (FbStat.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(DateTime DtStamp,Guid ApplicationId,int? EntityId,string Url,string ApiFunction,int Total,DateTime DtModified)
	    {
		    FbStat item = new FbStat();
		    
            item.DtStamp = DtStamp;
            
            item.ApplicationId = ApplicationId;
            
            item.EntityId = EntityId;
            
            item.Url = Url;
            
            item.ApiFunction = ApiFunction;
            
            item.Total = Total;
            
            item.DtModified = DtModified;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,DateTime DtStamp,Guid ApplicationId,int? EntityId,string Url,string ApiFunction,int Total,DateTime DtModified)
	    {
		    FbStat item = new FbStat();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.DtStamp = DtStamp;
				
			item.ApplicationId = ApplicationId;
				
			item.EntityId = EntityId;
				
			item.Url = Url;
				
			item.ApiFunction = ApiFunction;
				
			item.Total = Total;
				
			item.DtModified = DtModified;
				
	        item.Save(UserName);
	    }
    }
}
