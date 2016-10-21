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
namespace Erlg
{
    /// <summary>
    /// Controller class for LogArchive
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class LogArchiveController
    {
        // Preload our schema..
        LogArchive thisSchemaLoad = new LogArchive();
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
        public LogArchiveCollection FetchAll()
        {
            LogArchiveCollection coll = new LogArchiveCollection();
            Query qry = new Query(LogArchive.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public LogArchiveCollection FetchByID(object Id)
        {
            LogArchiveCollection coll = new LogArchiveCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public LogArchiveCollection FetchByQuery(Query qry)
        {
            LogArchiveCollection coll = new LogArchiveCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (LogArchive.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (LogArchive.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(int Id,DateTime DateX,string Source,string Message,string Form,string Querystring,string TargetSite,string StackTrace,string Referrer,string IpAddress,string Email,string ApplicationName)
	    {
		    LogArchive item = new LogArchive();
		    
            item.Id = Id;
            
            item.DateX = DateX;
            
            item.Source = Source;
            
            item.Message = Message;
            
            item.Form = Form;
            
            item.Querystring = Querystring;
            
            item.TargetSite = TargetSite;
            
            item.StackTrace = StackTrace;
            
            item.Referrer = Referrer;
            
            item.IpAddress = IpAddress;
            
            item.Email = Email;
            
            item.ApplicationName = ApplicationName;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,DateTime DateX,string Source,string Message,string Form,string Querystring,string TargetSite,string StackTrace,string Referrer,string IpAddress,string Email,string ApplicationName)
	    {
		    LogArchive item = new LogArchive();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.DateX = DateX;
				
			item.Source = Source;
				
			item.Message = Message;
				
			item.Form = Form;
				
			item.Querystring = Querystring;
				
			item.TargetSite = TargetSite;
				
			item.StackTrace = StackTrace;
				
			item.Referrer = Referrer;
				
			item.IpAddress = IpAddress;
				
			item.Email = Email;
				
			item.ApplicationName = ApplicationName;
				
	        item.Save(UserName);
	    }
    }
}
