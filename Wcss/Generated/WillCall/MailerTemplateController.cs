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
    /// Controller class for MailerTemplate
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class MailerTemplateController
    {
        // Preload our schema..
        MailerTemplate thisSchemaLoad = new MailerTemplate();
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
        public MailerTemplateCollection FetchAll()
        {
            MailerTemplateCollection coll = new MailerTemplateCollection();
            Query qry = new Query(MailerTemplate.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public MailerTemplateCollection FetchByID(object Id)
        {
            MailerTemplateCollection coll = new MailerTemplateCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public MailerTemplateCollection FetchByQuery(Query qry)
        {
            MailerTemplateCollection coll = new MailerTemplateCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (MailerTemplate.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (MailerTemplate.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(DateTime DtStamp,Guid ApplicationId,string Name,string Description,string Style,string Header,string Footer)
	    {
		    MailerTemplate item = new MailerTemplate();
		    
            item.DtStamp = DtStamp;
            
            item.ApplicationId = ApplicationId;
            
            item.Name = Name;
            
            item.Description = Description;
            
            item.Style = Style;
            
            item.Header = Header;
            
            item.Footer = Footer;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,DateTime DtStamp,Guid ApplicationId,string Name,string Description,string Style,string Header,string Footer)
	    {
		    MailerTemplate item = new MailerTemplate();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.DtStamp = DtStamp;
				
			item.ApplicationId = ApplicationId;
				
			item.Name = Name;
				
			item.Description = Description;
				
			item.Style = Style;
				
			item.Header = Header;
				
			item.Footer = Footer;
				
	        item.Save(UserName);
	    }
    }
}
