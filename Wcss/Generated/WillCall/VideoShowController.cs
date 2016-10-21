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
    /// Controller class for VideoShow
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class VideoShowController
    {
        // Preload our schema..
        VideoShow thisSchemaLoad = new VideoShow();
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
        public VideoShowCollection FetchAll()
        {
            VideoShowCollection coll = new VideoShowCollection();
            Query qry = new Query(VideoShow.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public VideoShowCollection FetchByID(object Id)
        {
            VideoShowCollection coll = new VideoShowCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public VideoShowCollection FetchByQuery(Query qry)
        {
            VideoShowCollection coll = new VideoShowCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (VideoShow.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (VideoShow.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(int Id,DateTime DtStamp,int TShowId,int TVideoId,int IDisplayOrder)
	    {
		    VideoShow item = new VideoShow();
		    
            item.Id = Id;
            
            item.DtStamp = DtStamp;
            
            item.TShowId = TShowId;
            
            item.TVideoId = TVideoId;
            
            item.IDisplayOrder = IDisplayOrder;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,DateTime DtStamp,int TShowId,int TVideoId,int IDisplayOrder)
	    {
		    VideoShow item = new VideoShow();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.DtStamp = DtStamp;
				
			item.TShowId = TShowId;
				
			item.TVideoId = TVideoId;
				
			item.IDisplayOrder = IDisplayOrder;
				
	        item.Save(UserName);
	    }
    }
}
