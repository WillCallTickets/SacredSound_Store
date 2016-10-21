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
    /// Controller class for Video
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class VideoController
    {
        // Preload our schema..
        Video thisSchemaLoad = new Video();
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
        public VideoCollection FetchAll()
        {
            VideoCollection coll = new VideoCollection();
            Query qry = new Query(Video.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public VideoCollection FetchByID(object Id)
        {
            VideoCollection coll = new VideoCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public VideoCollection FetchByQuery(Query qry)
        {
            VideoCollection coll = new VideoCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (Video.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (Video.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(DateTime DtStamp,Guid ApplicationId,string Path,string FileName,string DisplayText,string Description)
	    {
		    Video item = new Video();
		    
            item.DtStamp = DtStamp;
            
            item.ApplicationId = ApplicationId;
            
            item.Path = Path;
            
            item.FileName = FileName;
            
            item.DisplayText = DisplayText;
            
            item.Description = Description;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,DateTime DtStamp,Guid ApplicationId,string Path,string FileName,string DisplayText,string Description)
	    {
		    Video item = new Video();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.DtStamp = DtStamp;
				
			item.ApplicationId = ApplicationId;
				
			item.Path = Path;
				
			item.FileName = FileName;
				
			item.DisplayText = DisplayText;
				
			item.Description = Description;
				
	        item.Save(UserName);
	    }
    }
}
