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
    /// Controller class for Act
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class ActController
    {
        // Preload our schema..
        Act thisSchemaLoad = new Act();
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
        public ActCollection FetchAll()
        {
            ActCollection coll = new ActCollection();
            Query qry = new Query(Act.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public ActCollection FetchByID(object Id)
        {
            ActCollection coll = new ActCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public ActCollection FetchByQuery(Query qry)
        {
            ActCollection coll = new ActCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (Act.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (Act.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string Name,string NameRoot,string DisplayName,string Website,string PictureUrl,int IPicWidth,int IPicHeight,bool BListInDirectory,DateTime DtStamp,Guid ApplicationId)
	    {
		    Act item = new Act();
		    
            item.Name = Name;
            
            item.NameRoot = NameRoot;
            
            item.DisplayName = DisplayName;
            
            item.Website = Website;
            
            item.PictureUrl = PictureUrl;
            
            item.IPicWidth = IPicWidth;
            
            item.IPicHeight = IPicHeight;
            
            item.BListInDirectory = BListInDirectory;
            
            item.DtStamp = DtStamp;
            
            item.ApplicationId = ApplicationId;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,string Name,string NameRoot,string DisplayName,string Website,string PictureUrl,int IPicWidth,int IPicHeight,bool BListInDirectory,DateTime DtStamp,Guid ApplicationId)
	    {
		    Act item = new Act();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.Name = Name;
				
			item.NameRoot = NameRoot;
				
			item.DisplayName = DisplayName;
				
			item.Website = Website;
				
			item.PictureUrl = PictureUrl;
				
			item.IPicWidth = IPicWidth;
				
			item.IPicHeight = IPicHeight;
				
			item.BListInDirectory = BListInDirectory;
				
			item.DtStamp = DtStamp;
				
			item.ApplicationId = ApplicationId;
				
	        item.Save(UserName);
	    }
    }
}
