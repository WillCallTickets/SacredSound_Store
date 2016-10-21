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
    /// Controller class for EventQArchive
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class EventQArchiveController
    {
        // Preload our schema..
        EventQArchive thisSchemaLoad = new EventQArchive();
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
        public EventQArchiveCollection FetchAll()
        {
            EventQArchiveCollection coll = new EventQArchiveCollection();
            Query qry = new Query(EventQArchive.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public EventQArchiveCollection FetchByID(object Id)
        {
            EventQArchiveCollection coll = new EventQArchiveCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public EventQArchiveCollection FetchByQuery(Query qry)
        {
            EventQArchiveCollection coll = new EventQArchiveCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (EventQArchive.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (EventQArchive.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(int Id,DateTime? DtStamp,DateTime? DateToProcess,DateTime? DateProcessed,string Status,Guid? Threadlock,int? AttemptsRemaining,int? IPriority,Guid? CreatorId,string CreatorName,Guid? UserId,string UserName,string Context,string Verb,string OldValue,string NewValue,string Description,string Ip,Guid ApplicationId)
	    {
		    EventQArchive item = new EventQArchive();
		    
            item.Id = Id;
            
            item.DtStamp = DtStamp;
            
            item.DateToProcess = DateToProcess;
            
            item.DateProcessed = DateProcessed;
            
            item.Status = Status;
            
            item.Threadlock = Threadlock;
            
            item.AttemptsRemaining = AttemptsRemaining;
            
            item.IPriority = IPriority;
            
            item.CreatorId = CreatorId;
            
            item.CreatorName = CreatorName;
            
            item.UserId = UserId;
            
            item.UserName = UserName;
            
            item.Context = Context;
            
            item.Verb = Verb;
            
            item.OldValue = OldValue;
            
            item.NewValue = NewValue;
            
            item.Description = Description;
            
            item.Ip = Ip;
            
            item.ApplicationId = ApplicationId;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,DateTime? DtStamp,DateTime? DateToProcess,DateTime? DateProcessed,string Status,Guid? Threadlock,int? AttemptsRemaining,int? IPriority,Guid? CreatorId,string CreatorName,Guid? UserId,string UserName,string Context,string Verb,string OldValue,string NewValue,string Description,string Ip,Guid ApplicationId)
	    {
		    EventQArchive item = new EventQArchive();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.DtStamp = DtStamp;
				
			item.DateToProcess = DateToProcess;
				
			item.DateProcessed = DateProcessed;
				
			item.Status = Status;
				
			item.Threadlock = Threadlock;
				
			item.AttemptsRemaining = AttemptsRemaining;
				
			item.IPriority = IPriority;
				
			item.CreatorId = CreatorId;
				
			item.CreatorName = CreatorName;
				
			item.UserId = UserId;
				
			item.UserName = UserName;
				
			item.Context = Context;
				
			item.Verb = Verb;
				
			item.OldValue = OldValue;
				
			item.NewValue = NewValue;
				
			item.Description = Description;
				
			item.Ip = Ip;
				
			item.ApplicationId = ApplicationId;
				
	        item.Save(UserName);
	    }
    }
}
