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
    /// Controller class for MailQueue
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class MailQueueController
    {
        // Preload our schema..
        MailQueue thisSchemaLoad = new MailQueue();
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
        public MailQueueCollection FetchAll()
        {
            MailQueueCollection coll = new MailQueueCollection();
            Query qry = new Query(MailQueue.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public MailQueueCollection FetchByID(object Id)
        {
            MailQueueCollection coll = new MailQueueCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public MailQueueCollection FetchByQuery(Query qry)
        {
            MailQueueCollection coll = new MailQueueCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (MailQueue.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (MailQueue.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(int? TEmailLetterId,int? TSubscriptionEmailId,DateTime? DateToProcess,DateTime? DateProcessed,string FromName,string FromAddress,string ToAddress,string Cc,string Bcc,string Status,int Priority,bool? BMassMailer,Guid? ThreadLock,int? AttemptsRemaining,DateTime DtStamp,Guid ApplicationId)
	    {
		    MailQueue item = new MailQueue();
		    
            item.TEmailLetterId = TEmailLetterId;
            
            item.TSubscriptionEmailId = TSubscriptionEmailId;
            
            item.DateToProcess = DateToProcess;
            
            item.DateProcessed = DateProcessed;
            
            item.FromName = FromName;
            
            item.FromAddress = FromAddress;
            
            item.ToAddress = ToAddress;
            
            item.Cc = Cc;
            
            item.Bcc = Bcc;
            
            item.Status = Status;
            
            item.Priority = Priority;
            
            item.BMassMailer = BMassMailer;
            
            item.ThreadLock = ThreadLock;
            
            item.AttemptsRemaining = AttemptsRemaining;
            
            item.DtStamp = DtStamp;
            
            item.ApplicationId = ApplicationId;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,int? TEmailLetterId,int? TSubscriptionEmailId,DateTime? DateToProcess,DateTime? DateProcessed,string FromName,string FromAddress,string ToAddress,string Cc,string Bcc,string Status,int Priority,bool? BMassMailer,Guid? ThreadLock,int? AttemptsRemaining,DateTime DtStamp,Guid ApplicationId)
	    {
		    MailQueue item = new MailQueue();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.TEmailLetterId = TEmailLetterId;
				
			item.TSubscriptionEmailId = TSubscriptionEmailId;
				
			item.DateToProcess = DateToProcess;
				
			item.DateProcessed = DateProcessed;
				
			item.FromName = FromName;
				
			item.FromAddress = FromAddress;
				
			item.ToAddress = ToAddress;
				
			item.Cc = Cc;
				
			item.Bcc = Bcc;
				
			item.Status = Status;
				
			item.Priority = Priority;
				
			item.BMassMailer = BMassMailer;
				
			item.ThreadLock = ThreadLock;
				
			item.AttemptsRemaining = AttemptsRemaining;
				
			item.DtStamp = DtStamp;
				
			item.ApplicationId = ApplicationId;
				
	        item.Save(UserName);
	    }
    }
}
