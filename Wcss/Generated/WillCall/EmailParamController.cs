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
    /// Controller class for EmailParam
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class EmailParamController
    {
        // Preload our schema..
        EmailParam thisSchemaLoad = new EmailParam();
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
        public EmailParamCollection FetchAll()
        {
            EmailParamCollection coll = new EmailParamCollection();
            Query qry = new Query(EmailParam.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public EmailParamCollection FetchByID(object Id)
        {
            EmailParamCollection coll = new EmailParamCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public EmailParamCollection FetchByQuery(Query qry)
        {
            EmailParamCollection coll = new EmailParamCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (EmailParam.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (EmailParam.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string Name,string ValueX,int TMailQueueId,DateTime DtStamp)
	    {
		    EmailParam item = new EmailParam();
		    
            item.Name = Name;
            
            item.ValueX = ValueX;
            
            item.TMailQueueId = TMailQueueId;
            
            item.DtStamp = DtStamp;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,string Name,string ValueX,int TMailQueueId,DateTime DtStamp)
	    {
		    EmailParam item = new EmailParam();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.Name = Name;
				
			item.ValueX = ValueX;
				
			item.TMailQueueId = TMailQueueId;
				
			item.DtStamp = DtStamp;
				
	        item.Save(UserName);
	    }
    }
}
