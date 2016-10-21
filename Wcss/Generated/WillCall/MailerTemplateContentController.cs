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
    /// Controller class for MailerTemplateContent
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class MailerTemplateContentController
    {
        // Preload our schema..
        MailerTemplateContent thisSchemaLoad = new MailerTemplateContent();
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
        public MailerTemplateContentCollection FetchAll()
        {
            MailerTemplateContentCollection coll = new MailerTemplateContentCollection();
            Query qry = new Query(MailerTemplateContent.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public MailerTemplateContentCollection FetchByID(object Id)
        {
            MailerTemplateContentCollection coll = new MailerTemplateContentCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public MailerTemplateContentCollection FetchByQuery(Query qry)
        {
            MailerTemplateContentCollection coll = new MailerTemplateContentCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (MailerTemplateContent.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (MailerTemplateContent.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(DateTime DtStamp,int TMailerTemplateId,int IDisplayOrder,string VcTemplateAsset,string Name,string Title,string Template,string SeparatorTemplate,int IMaxListItems,int IMaxSelections,string VcFlags)
	    {
		    MailerTemplateContent item = new MailerTemplateContent();
		    
            item.DtStamp = DtStamp;
            
            item.TMailerTemplateId = TMailerTemplateId;
            
            item.IDisplayOrder = IDisplayOrder;
            
            item.VcTemplateAsset = VcTemplateAsset;
            
            item.Name = Name;
            
            item.Title = Title;
            
            item.Template = Template;
            
            item.SeparatorTemplate = SeparatorTemplate;
            
            item.IMaxListItems = IMaxListItems;
            
            item.IMaxSelections = IMaxSelections;
            
            item.VcFlags = VcFlags;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,DateTime DtStamp,int TMailerTemplateId,int IDisplayOrder,string VcTemplateAsset,string Name,string Title,string Template,string SeparatorTemplate,int IMaxListItems,int IMaxSelections,string VcFlags)
	    {
		    MailerTemplateContent item = new MailerTemplateContent();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.DtStamp = DtStamp;
				
			item.TMailerTemplateId = TMailerTemplateId;
				
			item.IDisplayOrder = IDisplayOrder;
				
			item.VcTemplateAsset = VcTemplateAsset;
				
			item.Name = Name;
				
			item.Title = Title;
				
			item.Template = Template;
				
			item.SeparatorTemplate = SeparatorTemplate;
				
			item.IMaxListItems = IMaxListItems;
				
			item.IMaxSelections = IMaxSelections;
				
			item.VcFlags = VcFlags;
				
	        item.Save(UserName);
	    }
    }
}
