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
    /// Controller class for aspnet_UsersInRoles
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class AspnetUsersInRoleController
    {
        // Preload our schema..
        AspnetUsersInRole thisSchemaLoad = new AspnetUsersInRole();
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
        public AspnetUsersInRoleCollection FetchAll()
        {
            AspnetUsersInRoleCollection coll = new AspnetUsersInRoleCollection();
            Query qry = new Query(AspnetUsersInRole.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public AspnetUsersInRoleCollection FetchByID(object UserId)
        {
            AspnetUsersInRoleCollection coll = new AspnetUsersInRoleCollection().Where("UserId", UserId).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public AspnetUsersInRoleCollection FetchByQuery(Query qry)
        {
            AspnetUsersInRoleCollection coll = new AspnetUsersInRoleCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object UserId)
        {
            return (AspnetUsersInRole.Delete(UserId) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object UserId)
        {
            return (AspnetUsersInRole.Destroy(UserId) == 1);
        }
        
        
        
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(Guid UserId,Guid RoleId)
        {
            Query qry = new Query(AspnetUsersInRole.Schema);
            qry.QueryType = QueryType.Delete;
            qry.AddWhere("UserId", UserId).AND("RoleId", RoleId);
            qry.Execute();
            return (true);
        }        
       
    	
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(Guid UserId,Guid RoleId)
	    {
		    AspnetUsersInRole item = new AspnetUsersInRole();
		    
            item.UserId = UserId;
            
            item.RoleId = RoleId;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(Guid UserId,Guid RoleId)
	    {
		    AspnetUsersInRole item = new AspnetUsersInRole();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.UserId = UserId;
				
			item.RoleId = RoleId;
				
	        item.Save(UserName);
	    }
    }
}
