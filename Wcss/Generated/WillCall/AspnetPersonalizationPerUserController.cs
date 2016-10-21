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
    /// Controller class for aspnet_PersonalizationPerUser
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class AspnetPersonalizationPerUserController
    {
        // Preload our schema..
        AspnetPersonalizationPerUser thisSchemaLoad = new AspnetPersonalizationPerUser();
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
        public AspnetPersonalizationPerUserCollection FetchAll()
        {
            AspnetPersonalizationPerUserCollection coll = new AspnetPersonalizationPerUserCollection();
            Query qry = new Query(AspnetPersonalizationPerUser.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public AspnetPersonalizationPerUserCollection FetchByID(object Id)
        {
            AspnetPersonalizationPerUserCollection coll = new AspnetPersonalizationPerUserCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public AspnetPersonalizationPerUserCollection FetchByQuery(Query qry)
        {
            AspnetPersonalizationPerUserCollection coll = new AspnetPersonalizationPerUserCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (AspnetPersonalizationPerUser.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (AspnetPersonalizationPerUser.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(Guid Id,Guid? PathId,Guid? UserId,byte[] PageSettings,DateTime LastUpdatedDate)
	    {
		    AspnetPersonalizationPerUser item = new AspnetPersonalizationPerUser();
		    
            item.Id = Id;
            
            item.PathId = PathId;
            
            item.UserId = UserId;
            
            item.PageSettings = PageSettings;
            
            item.LastUpdatedDate = LastUpdatedDate;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(Guid Id,Guid? PathId,Guid? UserId,byte[] PageSettings,DateTime LastUpdatedDate)
	    {
		    AspnetPersonalizationPerUser item = new AspnetPersonalizationPerUser();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.PathId = PathId;
				
			item.UserId = UserId;
				
			item.PageSettings = PageSettings;
				
			item.LastUpdatedDate = LastUpdatedDate;
				
	        item.Save(UserName);
	    }
    }
}
