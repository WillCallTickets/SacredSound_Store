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
    /// Controller class for EmailLetter
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class EmailLetterController
    {
        // Preload our schema..
        EmailLetter thisSchemaLoad = new EmailLetter();
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
        public EmailLetterCollection FetchAll()
        {
            EmailLetterCollection coll = new EmailLetterCollection();
            Query qry = new Query(EmailLetter.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public EmailLetterCollection FetchByID(object Id)
        {
            EmailLetterCollection coll = new EmailLetterCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public EmailLetterCollection FetchByQuery(Query qry)
        {
            EmailLetterCollection coll = new EmailLetterCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (EmailLetter.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (EmailLetter.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string Name,string Subject,string StyleContent,string HtmlVersion,string TextVersion,DateTime DtStamp,Guid ApplicationId)
	    {
		    EmailLetter item = new EmailLetter();
		    
            item.Name = Name;
            
            item.Subject = Subject;
            
            item.StyleContent = StyleContent;
            
            item.HtmlVersion = HtmlVersion;
            
            item.TextVersion = TextVersion;
            
            item.DtStamp = DtStamp;
            
            item.ApplicationId = ApplicationId;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,string Name,string Subject,string StyleContent,string HtmlVersion,string TextVersion,DateTime DtStamp,Guid ApplicationId)
	    {
		    EmailLetter item = new EmailLetter();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.Name = Name;
				
			item.Subject = Subject;
				
			item.StyleContent = StyleContent;
				
			item.HtmlVersion = HtmlVersion;
				
			item.TextVersion = TextVersion;
				
			item.DtStamp = DtStamp;
				
			item.ApplicationId = ApplicationId;
				
	        item.Save(UserName);
	    }
    }
}
