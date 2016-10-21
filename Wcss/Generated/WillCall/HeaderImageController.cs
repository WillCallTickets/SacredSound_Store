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
    /// Controller class for HeaderImage
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class HeaderImageController
    {
        // Preload our schema..
        HeaderImage thisSchemaLoad = new HeaderImage();
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
        public HeaderImageCollection FetchAll()
        {
            HeaderImageCollection coll = new HeaderImageCollection();
            Query qry = new Query(HeaderImage.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public HeaderImageCollection FetchByID(object Id)
        {
            HeaderImageCollection coll = new HeaderImageCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public HeaderImageCollection FetchByQuery(Query qry)
        {
            HeaderImageCollection coll = new HeaderImageCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (HeaderImage.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (HeaderImage.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(bool BActive,int IDisplayOrder,bool BDisplayPriority,bool BExclusive,int ITimeoutMsec,string FileName,string DisplayText,string NavigateUrl,int? TShowId,int? TMerchId,string VcContext,string UnlockCode,DateTime? DtStart,DateTime? DtEnd,Guid ApplicationId,DateTime DtStamp,DateTime DtModified)
	    {
		    HeaderImage item = new HeaderImage();
		    
            item.BActive = BActive;
            
            item.IDisplayOrder = IDisplayOrder;
            
            item.BDisplayPriority = BDisplayPriority;
            
            item.BExclusive = BExclusive;
            
            item.ITimeoutMsec = ITimeoutMsec;
            
            item.FileName = FileName;
            
            item.DisplayText = DisplayText;
            
            item.NavigateUrl = NavigateUrl;
            
            item.TShowId = TShowId;
            
            item.TMerchId = TMerchId;
            
            item.VcContext = VcContext;
            
            item.UnlockCode = UnlockCode;
            
            item.DtStart = DtStart;
            
            item.DtEnd = DtEnd;
            
            item.ApplicationId = ApplicationId;
            
            item.DtStamp = DtStamp;
            
            item.DtModified = DtModified;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,bool BActive,int IDisplayOrder,bool BDisplayPriority,bool BExclusive,int ITimeoutMsec,string FileName,string DisplayText,string NavigateUrl,int? TShowId,int? TMerchId,string VcContext,string UnlockCode,DateTime? DtStart,DateTime? DtEnd,Guid ApplicationId,DateTime DtStamp,DateTime DtModified)
	    {
		    HeaderImage item = new HeaderImage();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.BActive = BActive;
				
			item.IDisplayOrder = IDisplayOrder;
				
			item.BDisplayPriority = BDisplayPriority;
				
			item.BExclusive = BExclusive;
				
			item.ITimeoutMsec = ITimeoutMsec;
				
			item.FileName = FileName;
				
			item.DisplayText = DisplayText;
				
			item.NavigateUrl = NavigateUrl;
				
			item.TShowId = TShowId;
				
			item.TMerchId = TMerchId;
				
			item.VcContext = VcContext;
				
			item.UnlockCode = UnlockCode;
				
			item.DtStart = DtStart;
				
			item.DtEnd = DtEnd;
				
			item.ApplicationId = ApplicationId;
				
			item.DtStamp = DtStamp;
				
			item.DtModified = DtModified;
				
	        item.Save(UserName);
	    }
    }
}
