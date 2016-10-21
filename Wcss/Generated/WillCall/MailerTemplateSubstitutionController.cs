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
    /// Controller class for MailerTemplateSubstitution
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class MailerTemplateSubstitutionController
    {
        // Preload our schema..
        MailerTemplateSubstitution thisSchemaLoad = new MailerTemplateSubstitution();
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
        public MailerTemplateSubstitutionCollection FetchAll()
        {
            MailerTemplateSubstitutionCollection coll = new MailerTemplateSubstitutionCollection();
            Query qry = new Query(MailerTemplateSubstitution.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public MailerTemplateSubstitutionCollection FetchByID(object Id)
        {
            MailerTemplateSubstitutionCollection coll = new MailerTemplateSubstitutionCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public MailerTemplateSubstitutionCollection FetchByQuery(Query qry)
        {
            MailerTemplateSubstitutionCollection coll = new MailerTemplateSubstitutionCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (MailerTemplateSubstitution.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (MailerTemplateSubstitution.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(DateTime DtStamp,int TMailerTemplateContentId,string TagName,string TagValue)
	    {
		    MailerTemplateSubstitution item = new MailerTemplateSubstitution();
		    
            item.DtStamp = DtStamp;
            
            item.TMailerTemplateContentId = TMailerTemplateContentId;
            
            item.TagName = TagName;
            
            item.TagValue = TagValue;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,DateTime DtStamp,int TMailerTemplateContentId,string TagName,string TagValue)
	    {
		    MailerTemplateSubstitution item = new MailerTemplateSubstitution();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.DtStamp = DtStamp;
				
			item.TMailerTemplateContentId = TMailerTemplateContentId;
				
			item.TagName = TagName;
				
			item.TagValue = TagValue;
				
	        item.Save(UserName);
	    }
    }
}
