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
    /// Controller class for MailerContent
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class MailerContentController
    {
        // Preload our schema..
        MailerContent thisSchemaLoad = new MailerContent();
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
        public MailerContentCollection FetchAll()
        {
            MailerContentCollection coll = new MailerContentCollection();
            Query qry = new Query(MailerContent.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public MailerContentCollection FetchByID(object Id)
        {
            MailerContentCollection coll = new MailerContentCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public MailerContentCollection FetchByQuery(Query qry)
        {
            MailerContentCollection coll = new MailerContentCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (MailerContent.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (MailerContent.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(DateTime DtStamp,int TMailerId,int TMailerTemplateContentId,bool BActive,string VcTitle,string VcContent)
	    {
		    MailerContent item = new MailerContent();
		    
            item.DtStamp = DtStamp;
            
            item.TMailerId = TMailerId;
            
            item.TMailerTemplateContentId = TMailerTemplateContentId;
            
            item.BActive = BActive;
            
            item.VcTitle = VcTitle;
            
            item.VcContent = VcContent;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,DateTime DtStamp,int TMailerId,int TMailerTemplateContentId,bool BActive,string VcTitle,string VcContent)
	    {
		    MailerContent item = new MailerContent();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.DtStamp = DtStamp;
				
			item.TMailerId = TMailerId;
				
			item.TMailerTemplateContentId = TMailerTemplateContentId;
				
			item.BActive = BActive;
				
			item.VcTitle = VcTitle;
				
			item.VcContent = VcContent;
				
	        item.Save(UserName);
	    }
    }
}
