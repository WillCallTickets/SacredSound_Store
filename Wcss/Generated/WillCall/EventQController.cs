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
    /// Controller class for EventQ
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class EventQController
    {
        // Preload our schema..
        EventQ thisSchemaLoad = new EventQ();
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
        public EventQCollection FetchAll()
        {
            EventQCollection coll = new EventQCollection();
            Query qry = new Query(EventQ.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public EventQCollection FetchByID(object Id)
        {
            EventQCollection coll = new EventQCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public EventQCollection FetchByQuery(Query qry)
        {
            EventQCollection coll = new EventQCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (EventQ.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (EventQ.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(DateTime? DateToProcess,DateTime? DateProcessed,string Status,Guid? ThreadLock,int? AttemptsRemaining,int IPriority,Guid? CreatorId,string CreatorName,Guid? UserId,string UserName,string Context,string Verb,string OldValue,string NewValue,string Description,string Ip,DateTime? DtStamp,Guid ApplicationId)
	    {
		    EventQ item = new EventQ();
		    
            item.DateToProcess = DateToProcess;
            
            item.DateProcessed = DateProcessed;
            
            item.Status = Status;
            
            item.ThreadLock = ThreadLock;
            
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
            
            item.DtStamp = DtStamp;
            
            item.ApplicationId = ApplicationId;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,DateTime? DateToProcess,DateTime? DateProcessed,string Status,Guid? ThreadLock,int? AttemptsRemaining,int IPriority,Guid? CreatorId,string CreatorName,Guid? UserId,string UserName,string Context,string Verb,string OldValue,string NewValue,string Description,string Ip,DateTime? DtStamp,Guid ApplicationId)
	    {
		    EventQ item = new EventQ();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.DateToProcess = DateToProcess;
				
			item.DateProcessed = DateProcessed;
				
			item.Status = Status;
				
			item.ThreadLock = ThreadLock;
				
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
				
			item.DtStamp = DtStamp;
				
			item.ApplicationId = ApplicationId;
				
	        item.Save(UserName);
	    }
    }
}
