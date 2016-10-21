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
    /// Controller class for Download
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class DownloadController
    {
        // Preload our schema..
        Download thisSchemaLoad = new Download();
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
        public DownloadCollection FetchAll()
        {
            DownloadCollection coll = new DownloadCollection();
            Query qry = new Query(Download.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public DownloadCollection FetchByID(object Id)
        {
            DownloadCollection coll = new DownloadCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public DownloadCollection FetchByQuery(Query qry)
        {
            DownloadCollection coll = new DownloadCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (Download.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (Download.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string TrackNumber,string Title,string VcFileContext,string VcTrackContext,string VcGenre,string VcKeywords,int? TActId,string BaseStoragePath,string ApplicationName,string Compilation,string Artist,string Album,string FileName,string VcFormat,byte[] FileBinary,string FileTime,int IFileBytes,string SampleFile,string SampleBinary,int ISampleClick,int IAttempted,int ISuccessful,DateTime DtStamp,DateTime? DtLastValidated,Guid ApplicationId)
	    {
		    Download item = new Download();
		    
            item.TrackNumber = TrackNumber;
            
            item.Title = Title;
            
            item.VcFileContext = VcFileContext;
            
            item.VcTrackContext = VcTrackContext;
            
            item.VcGenre = VcGenre;
            
            item.VcKeywords = VcKeywords;
            
            item.TActId = TActId;
            
            item.BaseStoragePath = BaseStoragePath;
            
            item.ApplicationName = ApplicationName;
            
            item.Compilation = Compilation;
            
            item.Artist = Artist;
            
            item.Album = Album;
            
            item.FileName = FileName;
            
            item.VcFormat = VcFormat;
            
            item.FileBinary = FileBinary;
            
            item.FileTime = FileTime;
            
            item.IFileBytes = IFileBytes;
            
            item.SampleFile = SampleFile;
            
            item.SampleBinary = SampleBinary;
            
            item.ISampleClick = ISampleClick;
            
            item.IAttempted = IAttempted;
            
            item.ISuccessful = ISuccessful;
            
            item.DtStamp = DtStamp;
            
            item.DtLastValidated = DtLastValidated;
            
            item.ApplicationId = ApplicationId;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,string TrackNumber,string Title,string VcFileContext,string VcTrackContext,string VcGenre,string VcKeywords,int? TActId,string BaseStoragePath,string ApplicationName,string Compilation,string Artist,string Album,string FileName,string VcFormat,byte[] FileBinary,string FileTime,int IFileBytes,string SampleFile,string SampleBinary,int ISampleClick,int IAttempted,int ISuccessful,DateTime DtStamp,DateTime? DtLastValidated,Guid ApplicationId)
	    {
		    Download item = new Download();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.TrackNumber = TrackNumber;
				
			item.Title = Title;
				
			item.VcFileContext = VcFileContext;
				
			item.VcTrackContext = VcTrackContext;
				
			item.VcGenre = VcGenre;
				
			item.VcKeywords = VcKeywords;
				
			item.TActId = TActId;
				
			item.BaseStoragePath = BaseStoragePath;
				
			item.ApplicationName = ApplicationName;
				
			item.Compilation = Compilation;
				
			item.Artist = Artist;
				
			item.Album = Album;
				
			item.FileName = FileName;
				
			item.VcFormat = VcFormat;
				
			item.FileBinary = FileBinary;
				
			item.FileTime = FileTime;
				
			item.IFileBytes = IFileBytes;
				
			item.SampleFile = SampleFile;
				
			item.SampleBinary = SampleBinary;
				
			item.ISampleClick = ISampleClick;
				
			item.IAttempted = IAttempted;
				
			item.ISuccessful = ISuccessful;
				
			item.DtStamp = DtStamp;
				
			item.DtLastValidated = DtLastValidated;
				
			item.ApplicationId = ApplicationId;
				
	        item.Save(UserName);
	    }
    }
}
