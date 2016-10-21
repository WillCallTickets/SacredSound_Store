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
    /// Controller class for MerchDownload
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class MerchDownloadController
    {
        // Preload our schema..
        MerchDownload thisSchemaLoad = new MerchDownload();
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
        public MerchDownloadCollection FetchAll()
        {
            MerchDownloadCollection coll = new MerchDownloadCollection();
            Query qry = new Query(MerchDownload.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public MerchDownloadCollection FetchByID(object Id)
        {
            MerchDownloadCollection coll = new MerchDownloadCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public MerchDownloadCollection FetchByQuery(Query qry)
        {
            MerchDownloadCollection coll = new MerchDownloadCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (MerchDownload.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (MerchDownload.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(DateTime DtStamp,int TMerchId,int TDownloadId)
	    {
		    MerchDownload item = new MerchDownload();
		    
            item.DtStamp = DtStamp;
            
            item.TMerchId = TMerchId;
            
            item.TDownloadId = TDownloadId;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,DateTime DtStamp,int TMerchId,int TDownloadId)
	    {
		    MerchDownload item = new MerchDownload();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.DtStamp = DtStamp;
				
			item.TMerchId = TMerchId;
				
			item.TDownloadId = TDownloadId;
				
	        item.Save(UserName);
	    }
    }
}
