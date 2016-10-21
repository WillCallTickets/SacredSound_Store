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
    /// Controller class for aspnet_Applications
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class AspnetApplicationController
    {
        // Preload our schema..
        AspnetApplication thisSchemaLoad = new AspnetApplication();
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
        public AspnetApplicationCollection FetchAll()
        {
            AspnetApplicationCollection coll = new AspnetApplicationCollection();
            Query qry = new Query(AspnetApplication.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public AspnetApplicationCollection FetchByID(object ApplicationId)
        {
            AspnetApplicationCollection coll = new AspnetApplicationCollection().Where("ApplicationId", ApplicationId).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public AspnetApplicationCollection FetchByQuery(Query qry)
        {
            AspnetApplicationCollection coll = new AspnetApplicationCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object ApplicationId)
        {
            return (AspnetApplication.Delete(ApplicationId) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object ApplicationId)
        {
            return (AspnetApplication.Destroy(ApplicationId) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string ApplicationName,string LoweredApplicationName,Guid ApplicationId,string Description)
	    {
		    AspnetApplication item = new AspnetApplication();
		    
            item.ApplicationName = ApplicationName;
            
            item.LoweredApplicationName = LoweredApplicationName;
            
            item.ApplicationId = ApplicationId;
            
            item.Description = Description;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(string ApplicationName,string LoweredApplicationName,Guid ApplicationId,string Description)
	    {
		    AspnetApplication item = new AspnetApplication();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.ApplicationName = ApplicationName;
				
			item.LoweredApplicationName = LoweredApplicationName;
				
			item.ApplicationId = ApplicationId;
				
			item.Description = Description;
				
	        item.Save(UserName);
	    }
    }
}
