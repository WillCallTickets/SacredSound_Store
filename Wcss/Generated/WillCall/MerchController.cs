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
    /// Controller class for Merch
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class MerchController
    {
        // Preload our schema..
        Merch thisSchemaLoad = new Merch();
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
        public MerchCollection FetchAll()
        {
            MerchCollection coll = new MerchCollection();
            Query qry = new Query(Merch.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public MerchCollection FetchByID(object Id)
        {
            MerchCollection coll = new MerchCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public MerchCollection FetchByQuery(Query qry)
        {
            MerchCollection coll = new MerchCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (Merch.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (Merch.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string Name,string Style,string Color,string Size,int? TParentListing,bool BActive,bool BInternalOnly,bool BTaxable,bool BFeaturedItem,string ShortText,string VcDisplayTemplate,string Description,bool BUnlockActive,string UnlockCode,DateTime? DtUnlockDate,DateTime? DtUnlockEndDate,DateTime? DtStartDate,DateTime? DtEndDate,decimal? MPrice,bool? BUseSalePrice,decimal? MSalePrice,string VcDeliveryType,bool? BLowRateQualified,decimal? MWeight,decimal? MFlatShip,string VcFlatMethod,DateTime? DtBackorder,bool? BShipSeparate,bool? BSoldOut,int IMaxQtyPerOrder,int IAllotment,int IDamaged,int IPending,int ISold,int? IAvailable,int IRefunded,DateTime DtStamp,Guid ApplicationId)
	    {
		    Merch item = new Merch();
		    
            item.Name = Name;
            
            item.Style = Style;
            
            item.Color = Color;
            
            item.Size = Size;
            
            item.TParentListing = TParentListing;
            
            item.BActive = BActive;
            
            item.BInternalOnly = BInternalOnly;
            
            item.BTaxable = BTaxable;
            
            item.BFeaturedItem = BFeaturedItem;
            
            item.ShortText = ShortText;
            
            item.VcDisplayTemplate = VcDisplayTemplate;
            
            item.Description = Description;
            
            item.BUnlockActive = BUnlockActive;
            
            item.UnlockCode = UnlockCode;
            
            item.DtUnlockDate = DtUnlockDate;
            
            item.DtUnlockEndDate = DtUnlockEndDate;
            
            item.DtStartDate = DtStartDate;
            
            item.DtEndDate = DtEndDate;
            
            item.MPrice = MPrice;
            
            item.BUseSalePrice = BUseSalePrice;
            
            item.MSalePrice = MSalePrice;
            
            item.VcDeliveryType = VcDeliveryType;
            
            item.BLowRateQualified = BLowRateQualified;
            
            item.MWeight = MWeight;
            
            item.MFlatShip = MFlatShip;
            
            item.VcFlatMethod = VcFlatMethod;
            
            item.DtBackorder = DtBackorder;
            
            item.BShipSeparate = BShipSeparate;
            
            item.BSoldOut = BSoldOut;
            
            item.IMaxQtyPerOrder = IMaxQtyPerOrder;
            
            item.IAllotment = IAllotment;
            
            item.IDamaged = IDamaged;
            
            item.IPending = IPending;
            
            item.ISold = ISold;
            
            item.IAvailable = IAvailable;
            
            item.IRefunded = IRefunded;
            
            item.DtStamp = DtStamp;
            
            item.ApplicationId = ApplicationId;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,string Name,string Style,string Color,string Size,int? TParentListing,bool BActive,bool BInternalOnly,bool BTaxable,bool BFeaturedItem,string ShortText,string VcDisplayTemplate,string Description,bool BUnlockActive,string UnlockCode,DateTime? DtUnlockDate,DateTime? DtUnlockEndDate,DateTime? DtStartDate,DateTime? DtEndDate,decimal? MPrice,bool? BUseSalePrice,decimal? MSalePrice,string VcDeliveryType,bool? BLowRateQualified,decimal? MWeight,decimal? MFlatShip,string VcFlatMethod,DateTime? DtBackorder,bool? BShipSeparate,bool? BSoldOut,int IMaxQtyPerOrder,int IAllotment,int IDamaged,int IPending,int ISold,int? IAvailable,int IRefunded,DateTime DtStamp,Guid ApplicationId)
	    {
		    Merch item = new Merch();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.Name = Name;
				
			item.Style = Style;
				
			item.Color = Color;
				
			item.Size = Size;
				
			item.TParentListing = TParentListing;
				
			item.BActive = BActive;
				
			item.BInternalOnly = BInternalOnly;
				
			item.BTaxable = BTaxable;
				
			item.BFeaturedItem = BFeaturedItem;
				
			item.ShortText = ShortText;
				
			item.VcDisplayTemplate = VcDisplayTemplate;
				
			item.Description = Description;
				
			item.BUnlockActive = BUnlockActive;
				
			item.UnlockCode = UnlockCode;
				
			item.DtUnlockDate = DtUnlockDate;
				
			item.DtUnlockEndDate = DtUnlockEndDate;
				
			item.DtStartDate = DtStartDate;
				
			item.DtEndDate = DtEndDate;
				
			item.MPrice = MPrice;
				
			item.BUseSalePrice = BUseSalePrice;
				
			item.MSalePrice = MSalePrice;
				
			item.VcDeliveryType = VcDeliveryType;
				
			item.BLowRateQualified = BLowRateQualified;
				
			item.MWeight = MWeight;
				
			item.MFlatShip = MFlatShip;
				
			item.VcFlatMethod = VcFlatMethod;
				
			item.DtBackorder = DtBackorder;
				
			item.BShipSeparate = BShipSeparate;
				
			item.BSoldOut = BSoldOut;
				
			item.IMaxQtyPerOrder = IMaxQtyPerOrder;
				
			item.IAllotment = IAllotment;
				
			item.IDamaged = IDamaged;
				
			item.IPending = IPending;
				
			item.ISold = ISold;
				
			item.IAvailable = IAvailable;
				
			item.IRefunded = IRefunded;
				
			item.DtStamp = DtStamp;
				
			item.ApplicationId = ApplicationId;
				
	        item.Save(UserName);
	    }
    }
}
