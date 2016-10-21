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
    /// Controller class for aspnet_PersonalizationAllUsers
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class AspnetPersonalizationAllUserController
    {
        // Preload our schema..
        AspnetPersonalizationAllUser thisSchemaLoad = new AspnetPersonalizationAllUser();
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
        public AspnetPersonalizationAllUserCollection FetchAll()
        {
            AspnetPersonalizationAllUserCollection coll = new AspnetPersonalizationAllUserCollection();
            Query qry = new Query(AspnetPersonalizationAllUser.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public AspnetPersonalizationAllUserCollection FetchByID(object PathId)
        {
            AspnetPersonalizationAllUserCollection coll = new AspnetPersonalizationAllUserCollection().Where("PathId", PathId).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public AspnetPersonalizationAllUserCollection FetchByQuery(Query qry)
        {
            AspnetPersonalizationAllUserCollection coll = new AspnetPersonalizationAllUserCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object PathId)
        {
            return (AspnetPersonalizationAllUser.Delete(PathId) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object PathId)
        {
            return (AspnetPersonalizationAllUser.Destroy(PathId) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(Guid PathId,byte[] PageSettings,DateTime LastUpdatedDate)
	    {
		    AspnetPersonalizationAllUser item = new AspnetPersonalizationAllUser();
		    
            item.PathId = PathId;
            
            item.PageSettings = PageSettings;
            
            item.LastUpdatedDate = LastUpdatedDate;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(Guid PathId,byte[] PageSettings,DateTime LastUpdatedDate)
	    {
		    AspnetPersonalizationAllUser item = new AspnetPersonalizationAllUser();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.PathId = PathId;
				
			item.PageSettings = PageSettings;
				
			item.LastUpdatedDate = LastUpdatedDate;
				
	        item.Save(UserName);
	    }
    }
}
