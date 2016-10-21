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
    /// Controller class for CharitableOrg
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class CharitableOrgController
    {
        // Preload our schema..
        CharitableOrg thisSchemaLoad = new CharitableOrg();
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
        public CharitableOrgCollection FetchAll()
        {
            CharitableOrgCollection coll = new CharitableOrgCollection();
            Query qry = new Query(CharitableOrg.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public CharitableOrgCollection FetchByID(object Id)
        {
            CharitableOrgCollection coll = new CharitableOrgCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public CharitableOrgCollection FetchByQuery(Query qry)
        {
            CharitableOrgCollection coll = new CharitableOrgCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (CharitableOrg.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (CharitableOrg.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(DateTime DtStamp,bool BActive,string Name,string NameRoot,string DisplayName,string WebsiteUrl,string PictureUrl,string ShortDescription,string Description,Guid ApplicationId)
	    {
		    CharitableOrg item = new CharitableOrg();
		    
            item.DtStamp = DtStamp;
            
            item.BActive = BActive;
            
            item.Name = Name;
            
            item.NameRoot = NameRoot;
            
            item.DisplayName = DisplayName;
            
            item.WebsiteUrl = WebsiteUrl;
            
            item.PictureUrl = PictureUrl;
            
            item.ShortDescription = ShortDescription;
            
            item.Description = Description;
            
            item.ApplicationId = ApplicationId;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,DateTime DtStamp,bool BActive,string Name,string NameRoot,string DisplayName,string WebsiteUrl,string PictureUrl,string ShortDescription,string Description,Guid ApplicationId)
	    {
		    CharitableOrg item = new CharitableOrg();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.DtStamp = DtStamp;
				
			item.BActive = BActive;
				
			item.Name = Name;
				
			item.NameRoot = NameRoot;
				
			item.DisplayName = DisplayName;
				
			item.WebsiteUrl = WebsiteUrl;
				
			item.PictureUrl = PictureUrl;
				
			item.ShortDescription = ShortDescription;
				
			item.Description = Description;
				
			item.ApplicationId = ApplicationId;
				
	        item.Save(UserName);
	    }
    }
}
