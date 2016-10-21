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
    /// Controller class for aspnet_Roles
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class AspnetRoleController
    {
        // Preload our schema..
        AspnetRole thisSchemaLoad = new AspnetRole();
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
        public AspnetRoleCollection FetchAll()
        {
            AspnetRoleCollection coll = new AspnetRoleCollection();
            Query qry = new Query(AspnetRole.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public AspnetRoleCollection FetchByID(object RoleId)
        {
            AspnetRoleCollection coll = new AspnetRoleCollection().Where("RoleId", RoleId).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public AspnetRoleCollection FetchByQuery(Query qry)
        {
            AspnetRoleCollection coll = new AspnetRoleCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object RoleId)
        {
            return (AspnetRole.Delete(RoleId) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object RoleId)
        {
            return (AspnetRole.Destroy(RoleId) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(Guid ApplicationId,Guid RoleId,string RoleName,string LoweredRoleName,string Description)
	    {
		    AspnetRole item = new AspnetRole();
		    
            item.ApplicationId = ApplicationId;
            
            item.RoleId = RoleId;
            
            item.RoleName = RoleName;
            
            item.LoweredRoleName = LoweredRoleName;
            
            item.Description = Description;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(Guid ApplicationId,Guid RoleId,string RoleName,string LoweredRoleName,string Description)
	    {
		    AspnetRole item = new AspnetRole();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.ApplicationId = ApplicationId;
				
			item.RoleId = RoleId;
				
			item.RoleName = RoleName;
				
			item.LoweredRoleName = LoweredRoleName;
				
			item.Description = Description;
				
	        item.Save(UserName);
	    }
    }
}