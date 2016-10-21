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
    /// Controller class for ItemImage
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class ItemImageController
    {
        // Preload our schema..
        ItemImage thisSchemaLoad = new ItemImage();
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
        public ItemImageCollection FetchAll()
        {
            ItemImageCollection coll = new ItemImageCollection();
            Query qry = new Query(ItemImage.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public ItemImageCollection FetchByID(object Id)
        {
            ItemImageCollection coll = new ItemImageCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public ItemImageCollection FetchByQuery(Query qry)
        {
            ItemImageCollection coll = new ItemImageCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (ItemImage.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (ItemImage.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(int? TMerchId,int? TFutureId,bool? BItemImage,bool? BDetailImage,bool? BOverrideThumbnail,string DetailDescription,string StorageRemote,string Path,string ImageName,int ImageHeight,int ImageWidth,string ThumbClass,int IDisplayOrder,DateTime DtStamp)
	    {
		    ItemImage item = new ItemImage();
		    
            item.TMerchId = TMerchId;
            
            item.TFutureId = TFutureId;
            
            item.BItemImage = BItemImage;
            
            item.BDetailImage = BDetailImage;
            
            item.BOverrideThumbnail = BOverrideThumbnail;
            
            item.DetailDescription = DetailDescription;
            
            item.StorageRemote = StorageRemote;
            
            item.Path = Path;
            
            item.ImageName = ImageName;
            
            item.ImageHeight = ImageHeight;
            
            item.ImageWidth = ImageWidth;
            
            item.ThumbClass = ThumbClass;
            
            item.IDisplayOrder = IDisplayOrder;
            
            item.DtStamp = DtStamp;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,int? TMerchId,int? TFutureId,bool? BItemImage,bool? BDetailImage,bool? BOverrideThumbnail,string DetailDescription,string StorageRemote,string Path,string ImageName,int ImageHeight,int ImageWidth,string ThumbClass,int IDisplayOrder,DateTime DtStamp)
	    {
		    ItemImage item = new ItemImage();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.TMerchId = TMerchId;
				
			item.TFutureId = TFutureId;
				
			item.BItemImage = BItemImage;
				
			item.BDetailImage = BDetailImage;
				
			item.BOverrideThumbnail = BOverrideThumbnail;
				
			item.DetailDescription = DetailDescription;
				
			item.StorageRemote = StorageRemote;
				
			item.Path = Path;
				
			item.ImageName = ImageName;
				
			item.ImageHeight = ImageHeight;
				
			item.ImageWidth = ImageWidth;
				
			item.ThumbClass = ThumbClass;
				
			item.IDisplayOrder = IDisplayOrder;
				
			item.DtStamp = DtStamp;
				
	        item.Save(UserName);
	    }
    }
}
