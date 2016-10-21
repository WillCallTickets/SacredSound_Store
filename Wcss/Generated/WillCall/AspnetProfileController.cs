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
    /// Controller class for aspnet_Profile
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class AspnetProfileController
    {
        // Preload our schema..
        AspnetProfile thisSchemaLoad = new AspnetProfile();
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
        public AspnetProfileCollection FetchAll()
        {
            AspnetProfileCollection coll = new AspnetProfileCollection();
            Query qry = new Query(AspnetProfile.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public AspnetProfileCollection FetchByID(object UserId)
        {
            AspnetProfileCollection coll = new AspnetProfileCollection().Where("UserId", UserId).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public AspnetProfileCollection FetchByQuery(Query qry)
        {
            AspnetProfileCollection coll = new AspnetProfileCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object UserId)
        {
            return (AspnetProfile.Delete(UserId) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object UserId)
        {
            return (AspnetProfile.Destroy(UserId) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(Guid UserId,string PropertyNames,string PropertyValuesString,byte[] PropertyValuesBinary,DateTime LastUpdatedDate)
	    {
		    AspnetProfile item = new AspnetProfile();
		    
            item.UserId = UserId;
            
            item.PropertyNames = PropertyNames;
            
            item.PropertyValuesString = PropertyValuesString;
            
            item.PropertyValuesBinary = PropertyValuesBinary;
            
            item.LastUpdatedDate = LastUpdatedDate;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(Guid UserId,string PropertyNames,string PropertyValuesString,byte[] PropertyValuesBinary,DateTime LastUpdatedDate)
	    {
		    AspnetProfile item = new AspnetProfile();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.UserId = UserId;
				
			item.PropertyNames = PropertyNames;
				
			item.PropertyValuesString = PropertyValuesString;
				
			item.PropertyValuesBinary = PropertyValuesBinary;
				
			item.LastUpdatedDate = LastUpdatedDate;
				
	        item.Save(UserName);
	    }
    }
}
