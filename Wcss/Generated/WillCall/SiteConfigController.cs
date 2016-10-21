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
    /// Controller class for SiteConfig
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class SiteConfigController
    {
        // Preload our schema..
        SiteConfig thisSchemaLoad = new SiteConfig();
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
        public SiteConfigCollection FetchAll()
        {
            SiteConfigCollection coll = new SiteConfigCollection();
            Query qry = new Query(SiteConfig.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public SiteConfigCollection FetchByID(object Id)
        {
            SiteConfigCollection coll = new SiteConfigCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public SiteConfigCollection FetchByQuery(Query qry)
        {
            SiteConfigCollection coll = new SiteConfigCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (SiteConfig.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (SiteConfig.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string DataType,int MaxLength,string Context,string Description,string Name,string ValueX,DateTime DtStamp,Guid ApplicationId)
	    {
		    SiteConfig item = new SiteConfig();
		    
            item.DataType = DataType;
            
            item.MaxLength = MaxLength;
            
            item.Context = Context;
            
            item.Description = Description;
            
            item.Name = Name;
            
            item.ValueX = ValueX;
            
            item.DtStamp = DtStamp;
            
            item.ApplicationId = ApplicationId;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,string DataType,int MaxLength,string Context,string Description,string Name,string ValueX,DateTime DtStamp,Guid ApplicationId)
	    {
		    SiteConfig item = new SiteConfig();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.DataType = DataType;
				
			item.MaxLength = MaxLength;
				
			item.Context = Context;
				
			item.Description = Description;
				
			item.Name = Name;
				
			item.ValueX = ValueX;
				
			item.DtStamp = DtStamp;
				
			item.ApplicationId = ApplicationId;
				
	        item.Save(UserName);
	    }
    }
}
