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
    /// Controller class for UserEvent
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class UserEventController
    {
        // Preload our schema..
        UserEvent thisSchemaLoad = new UserEvent();
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
        public UserEventCollection FetchAll()
        {
            UserEventCollection coll = new UserEventCollection();
            Query qry = new Query(UserEvent.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public UserEventCollection FetchByID(object Id)
        {
            UserEventCollection coll = new UserEventCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public UserEventCollection FetchByQuery(Query qry)
        {
            UserEventCollection coll = new UserEventCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (UserEvent.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (UserEvent.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(Guid UserId,int TEventQId,DateTime? DtStamp)
	    {
		    UserEvent item = new UserEvent();
		    
            item.UserId = UserId;
            
            item.TEventQId = TEventQId;
            
            item.DtStamp = DtStamp;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,Guid UserId,int TEventQId,DateTime? DtStamp)
	    {
		    UserEvent item = new UserEvent();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.UserId = UserId;
				
			item.TEventQId = TEventQId;
				
			item.DtStamp = DtStamp;
				
	        item.Save(UserName);
	    }
    }
}
