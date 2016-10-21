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
    /// Controller class for SubscriptionUser
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class SubscriptionUserController
    {
        // Preload our schema..
        SubscriptionUser thisSchemaLoad = new SubscriptionUser();
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
        public SubscriptionUserCollection FetchAll()
        {
            SubscriptionUserCollection coll = new SubscriptionUserCollection();
            Query qry = new Query(SubscriptionUser.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public SubscriptionUserCollection FetchByID(object Id)
        {
            SubscriptionUserCollection coll = new SubscriptionUserCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public SubscriptionUserCollection FetchByQuery(Query qry)
        {
            SubscriptionUserCollection coll = new SubscriptionUserCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (SubscriptionUser.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (SubscriptionUser.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(Guid? UserId,int TSubscriptionId,bool BSubscribed,DateTime? DtLastActionDate,bool BHtmlFormat,DateTime DtStamp)
	    {
		    SubscriptionUser item = new SubscriptionUser();
		    
            item.UserId = UserId;
            
            item.TSubscriptionId = TSubscriptionId;
            
            item.BSubscribed = BSubscribed;
            
            item.DtLastActionDate = DtLastActionDate;
            
            item.BHtmlFormat = BHtmlFormat;
            
            item.DtStamp = DtStamp;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,Guid? UserId,int TSubscriptionId,bool BSubscribed,DateTime? DtLastActionDate,bool BHtmlFormat,DateTime DtStamp)
	    {
		    SubscriptionUser item = new SubscriptionUser();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.UserId = UserId;
				
			item.TSubscriptionId = TSubscriptionId;
				
			item.BSubscribed = BSubscribed;
				
			item.DtLastActionDate = DtLastActionDate;
				
			item.BHtmlFormat = BHtmlFormat;
				
			item.DtStamp = DtStamp;
				
	        item.Save(UserName);
	    }
    }
}
