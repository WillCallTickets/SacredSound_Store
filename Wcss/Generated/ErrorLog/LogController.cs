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
    /// Controller class for Log
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class LogController
    {
        // Preload our schema..
        Log thisSchemaLoad = new Log();
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
        public LogCollection FetchAll()
        {
            LogCollection coll = new LogCollection();
            Query qry = new Query(Log.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public LogCollection FetchByID(object Id)
        {
            LogCollection coll = new LogCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public LogCollection FetchByQuery(Query qry)
        {
            LogCollection coll = new LogCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (Log.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (Log.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(DateTime DateX,string Source,string Message,string Form,string Querystring,string TargetSite,string StackTrace,string Referrer,string IpAddress,string Email,string ApplicationName)
	    {
		    Log item = new Log();
		    
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
		    Log item = new Log();
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
