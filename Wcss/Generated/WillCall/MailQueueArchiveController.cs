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
    /// Controller class for MailQueueArchive
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class MailQueueArchiveController
    {
        // Preload our schema..
        MailQueueArchive thisSchemaLoad = new MailQueueArchive();
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
        public MailQueueArchiveCollection FetchAll()
        {
            MailQueueArchiveCollection coll = new MailQueueArchiveCollection();
            Query qry = new Query(MailQueueArchive.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public MailQueueArchiveCollection FetchByID(object Id)
        {
            MailQueueArchiveCollection coll = new MailQueueArchiveCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public MailQueueArchiveCollection FetchByQuery(Query qry)
        {
            MailQueueArchiveCollection coll = new MailQueueArchiveCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (MailQueueArchive.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (MailQueueArchive.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(int Id,DateTime? DtStamp,DateTime? DateToProcess,DateTime? DateProcessed,string FromName,string FromAddress,string ToAddress,string Cc,string Bcc,string Status,int? TEmailLetterId,int? TSubscriptionEmailId,int? Priority,bool? BMassMailer,Guid? ThreadLock,int? AttemptsRemaining,Guid ApplicationId)
	    {
		    MailQueueArchive item = new MailQueueArchive();
		    
            item.Id = Id;
            
            item.DtStamp = DtStamp;
            
            item.DateToProcess = DateToProcess;
            
            item.DateProcessed = DateProcessed;
            
            item.FromName = FromName;
            
            item.FromAddress = FromAddress;
            
            item.ToAddress = ToAddress;
            
            item.Cc = Cc;
            
            item.Bcc = Bcc;
            
            item.Status = Status;
            
            item.TEmailLetterId = TEmailLetterId;
            
            item.TSubscriptionEmailId = TSubscriptionEmailId;
            
            item.Priority = Priority;
            
            item.BMassMailer = BMassMailer;
            
            item.ThreadLock = ThreadLock;
            
            item.AttemptsRemaining = AttemptsRemaining;
            
            item.ApplicationId = ApplicationId;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,DateTime? DtStamp,DateTime? DateToProcess,DateTime? DateProcessed,string FromName,string FromAddress,string ToAddress,string Cc,string Bcc,string Status,int? TEmailLetterId,int? TSubscriptionEmailId,int? Priority,bool? BMassMailer,Guid? ThreadLock,int? AttemptsRemaining,Guid ApplicationId)
	    {
		    MailQueueArchive item = new MailQueueArchive();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.DtStamp = DtStamp;
				
			item.DateToProcess = DateToProcess;
				
			item.DateProcessed = DateProcessed;
				
			item.FromName = FromName;
				
			item.FromAddress = FromAddress;
				
			item.ToAddress = ToAddress;
				
			item.Cc = Cc;
				
			item.Bcc = Bcc;
				
			item.Status = Status;
				
			item.TEmailLetterId = TEmailLetterId;
				
			item.TSubscriptionEmailId = TSubscriptionEmailId;
				
			item.Priority = Priority;
				
			item.BMassMailer = BMassMailer;
				
			item.ThreadLock = ThreadLock;
				
			item.AttemptsRemaining = AttemptsRemaining;
				
			item.ApplicationId = ApplicationId;
				
	        item.Save(UserName);
	    }
    }
}
