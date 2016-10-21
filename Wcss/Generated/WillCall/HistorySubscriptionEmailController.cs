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
    /// Controller class for HistorySubscriptionEmail
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class HistorySubscriptionEmailController
    {
        // Preload our schema..
        HistorySubscriptionEmail thisSchemaLoad = new HistorySubscriptionEmail();
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
        public HistorySubscriptionEmailCollection FetchAll()
        {
            HistorySubscriptionEmailCollection coll = new HistorySubscriptionEmailCollection();
            Query qry = new Query(HistorySubscriptionEmail.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public HistorySubscriptionEmailCollection FetchByID(object Id)
        {
            HistorySubscriptionEmailCollection coll = new HistorySubscriptionEmailCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public HistorySubscriptionEmailCollection FetchByQuery(Query qry)
        {
            HistorySubscriptionEmailCollection coll = new HistorySubscriptionEmailCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (HistorySubscriptionEmail.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (HistorySubscriptionEmail.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(DateTime DtStamp,int TSubscriptionEmailId,DateTime DtSent,int IRecipients)
	    {
		    HistorySubscriptionEmail item = new HistorySubscriptionEmail();
		    
            item.DtStamp = DtStamp;
            
            item.TSubscriptionEmailId = TSubscriptionEmailId;
            
            item.DtSent = DtSent;
            
            item.IRecipients = IRecipients;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,DateTime DtStamp,int TSubscriptionEmailId,DateTime DtSent,int IRecipients)
	    {
		    HistorySubscriptionEmail item = new HistorySubscriptionEmail();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.DtStamp = DtStamp;
				
			item.TSubscriptionEmailId = TSubscriptionEmailId;
				
			item.DtSent = DtSent;
				
			item.IRecipients = IRecipients;
				
	        item.Save(UserName);
	    }
    }
}
