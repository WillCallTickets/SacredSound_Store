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
    /// Controller class for Subscription
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class SubscriptionController
    {
        // Preload our schema..
        Subscription thisSchemaLoad = new Subscription();
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
        public SubscriptionCollection FetchAll()
        {
            SubscriptionCollection coll = new SubscriptionCollection();
            Query qry = new Query(Subscription.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public SubscriptionCollection FetchByID(object Id)
        {
            SubscriptionCollection coll = new SubscriptionCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public SubscriptionCollection FetchByQuery(Query qry)
        {
            SubscriptionCollection coll = new SubscriptionCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (Subscription.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (Subscription.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(Guid RoleId,bool BActive,bool BDefault,string Name,string Description,string InternalDescription,DateTime DtStamp,Guid ApplicationId)
	    {
		    Subscription item = new Subscription();
		    
            item.RoleId = RoleId;
            
            item.BActive = BActive;
            
            item.BDefault = BDefault;
            
            item.Name = Name;
            
            item.Description = Description;
            
            item.InternalDescription = InternalDescription;
            
            item.DtStamp = DtStamp;
            
            item.ApplicationId = ApplicationId;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,Guid RoleId,bool BActive,bool BDefault,string Name,string Description,string InternalDescription,DateTime DtStamp,Guid ApplicationId)
	    {
		    Subscription item = new Subscription();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.RoleId = RoleId;
				
			item.BActive = BActive;
				
			item.BDefault = BDefault;
				
			item.Name = Name;
				
			item.Description = Description;
				
			item.InternalDescription = InternalDescription;
				
			item.DtStamp = DtStamp;
				
			item.ApplicationId = ApplicationId;
				
	        item.Save(UserName);
	    }
    }
}
