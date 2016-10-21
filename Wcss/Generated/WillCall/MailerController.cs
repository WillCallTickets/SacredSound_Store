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
    /// Controller class for Mailer
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class MailerController
    {
        // Preload our schema..
        Mailer thisSchemaLoad = new Mailer();
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
        public MailerCollection FetchAll()
        {
            MailerCollection coll = new MailerCollection();
            Query qry = new Query(Mailer.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public MailerCollection FetchByID(object Id)
        {
            MailerCollection coll = new MailerCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public MailerCollection FetchByQuery(Query qry)
        {
            MailerCollection coll = new MailerCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (Mailer.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (Mailer.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(DateTime DtStamp,int TMailerTemplateId,string Name,string Subject)
	    {
		    Mailer item = new Mailer();
		    
            item.DtStamp = DtStamp;
            
            item.TMailerTemplateId = TMailerTemplateId;
            
            item.Name = Name;
            
            item.Subject = Subject;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,DateTime DtStamp,int TMailerTemplateId,string Name,string Subject)
	    {
		    Mailer item = new Mailer();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.DtStamp = DtStamp;
				
			item.TMailerTemplateId = TMailerTemplateId;
				
			item.Name = Name;
				
			item.Subject = Subject;
				
	        item.Save(UserName);
	    }
    }
}
