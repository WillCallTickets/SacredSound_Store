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
    /// Controller class for SalePromotion
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class SalePromotionController
    {
        // Preload our schema..
        SalePromotion thisSchemaLoad = new SalePromotion();
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
        public SalePromotionCollection FetchAll()
        {
            SalePromotionCollection coll = new SalePromotionCollection();
            Query qry = new Query(SalePromotion.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public SalePromotionCollection FetchByID(object Id)
        {
            SalePromotionCollection coll = new SalePromotionCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public SalePromotionCollection FetchByQuery(Query qry)
        {
            SalePromotionCollection coll = new SalePromotionCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (SalePromotion.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (SalePromotion.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(DateTime? DtStamp,Guid ApplicationId,bool BActive,int IDisplayOrder,int IBannerTimeoutMsecs,string Name,string DisplayText,string AdditionalText,int? TShowTicketId,string RequiredPromotionCode,int? TRequiredParentShowTicketId,int? TRequiredParentShowDateId,int IRequiredParentQty,decimal MPrice,decimal MDiscountAmount,int IDiscountPercent,string VcDiscountContext,decimal MMinMerch,decimal MMinTicket,decimal MMinTotal,string BannerUrl,string BannerClickUrl,bool BDisplayAtParent,bool BBannerMerch,bool BBannerTicket,bool BBannerCartEdit,bool BBannerCheckout,bool BBannerShipping,string ShipOfferMethod,string UnlockCode,DateTime? DtStartDate,DateTime? DtEndDate,int IMaxPerOrder,decimal? MMaxValue,decimal MWeight,bool BDeactivateOnNoInventory,int IMaxUsesPerUser,string VcTriggerListMerch,bool? BAllowMultSelections,bool? BAllowPromoTotalInMinimum,string JsonMeta)
	    {
		    SalePromotion item = new SalePromotion();
		    
            item.DtStamp = DtStamp;
            
            item.ApplicationId = ApplicationId;
            
            item.BActive = BActive;
            
            item.IDisplayOrder = IDisplayOrder;
            
            item.IBannerTimeoutMsecs = IBannerTimeoutMsecs;
            
            item.Name = Name;
            
            item.DisplayText = DisplayText;
            
            item.AdditionalText = AdditionalText;
            
            item.TShowTicketId = TShowTicketId;
            
            item.RequiredPromotionCode = RequiredPromotionCode;
            
            item.TRequiredParentShowTicketId = TRequiredParentShowTicketId;
            
            item.TRequiredParentShowDateId = TRequiredParentShowDateId;
            
            item.IRequiredParentQty = IRequiredParentQty;
            
            item.MPrice = MPrice;
            
            item.MDiscountAmount = MDiscountAmount;
            
            item.IDiscountPercent = IDiscountPercent;
            
            item.VcDiscountContext = VcDiscountContext;
            
            item.MMinMerch = MMinMerch;
            
            item.MMinTicket = MMinTicket;
            
            item.MMinTotal = MMinTotal;
            
            item.BannerUrl = BannerUrl;
            
            item.BannerClickUrl = BannerClickUrl;
            
            item.BDisplayAtParent = BDisplayAtParent;
            
            item.BBannerMerch = BBannerMerch;
            
            item.BBannerTicket = BBannerTicket;
            
            item.BBannerCartEdit = BBannerCartEdit;
            
            item.BBannerCheckout = BBannerCheckout;
            
            item.BBannerShipping = BBannerShipping;
            
            item.ShipOfferMethod = ShipOfferMethod;
            
            item.UnlockCode = UnlockCode;
            
            item.DtStartDate = DtStartDate;
            
            item.DtEndDate = DtEndDate;
            
            item.IMaxPerOrder = IMaxPerOrder;
            
            item.MMaxValue = MMaxValue;
            
            item.MWeight = MWeight;
            
            item.BDeactivateOnNoInventory = BDeactivateOnNoInventory;
            
            item.IMaxUsesPerUser = IMaxUsesPerUser;
            
            item.VcTriggerListMerch = VcTriggerListMerch;
            
            item.BAllowMultSelections = BAllowMultSelections;
            
            item.BAllowPromoTotalInMinimum = BAllowPromoTotalInMinimum;
            
            item.JsonMeta = JsonMeta;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,DateTime? DtStamp,Guid ApplicationId,bool BActive,int IDisplayOrder,int IBannerTimeoutMsecs,string Name,string DisplayText,string AdditionalText,int? TShowTicketId,string RequiredPromotionCode,int? TRequiredParentShowTicketId,int? TRequiredParentShowDateId,int IRequiredParentQty,decimal MPrice,decimal MDiscountAmount,int IDiscountPercent,string VcDiscountContext,decimal MMinMerch,decimal MMinTicket,decimal MMinTotal,string BannerUrl,string BannerClickUrl,bool BDisplayAtParent,bool BBannerMerch,bool BBannerTicket,bool BBannerCartEdit,bool BBannerCheckout,bool BBannerShipping,string ShipOfferMethod,string UnlockCode,DateTime? DtStartDate,DateTime? DtEndDate,int IMaxPerOrder,decimal? MMaxValue,decimal MWeight,bool BDeactivateOnNoInventory,int IMaxUsesPerUser,string VcTriggerListMerch,bool? BAllowMultSelections,bool? BAllowPromoTotalInMinimum,string JsonMeta)
	    {
		    SalePromotion item = new SalePromotion();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.DtStamp = DtStamp;
				
			item.ApplicationId = ApplicationId;
				
			item.BActive = BActive;
				
			item.IDisplayOrder = IDisplayOrder;
				
			item.IBannerTimeoutMsecs = IBannerTimeoutMsecs;
				
			item.Name = Name;
				
			item.DisplayText = DisplayText;
				
			item.AdditionalText = AdditionalText;
				
			item.TShowTicketId = TShowTicketId;
				
			item.RequiredPromotionCode = RequiredPromotionCode;
				
			item.TRequiredParentShowTicketId = TRequiredParentShowTicketId;
				
			item.TRequiredParentShowDateId = TRequiredParentShowDateId;
				
			item.IRequiredParentQty = IRequiredParentQty;
				
			item.MPrice = MPrice;
				
			item.MDiscountAmount = MDiscountAmount;
				
			item.IDiscountPercent = IDiscountPercent;
				
			item.VcDiscountContext = VcDiscountContext;
				
			item.MMinMerch = MMinMerch;
				
			item.MMinTicket = MMinTicket;
				
			item.MMinTotal = MMinTotal;
				
			item.BannerUrl = BannerUrl;
				
			item.BannerClickUrl = BannerClickUrl;
				
			item.BDisplayAtParent = BDisplayAtParent;
				
			item.BBannerMerch = BBannerMerch;
				
			item.BBannerTicket = BBannerTicket;
				
			item.BBannerCartEdit = BBannerCartEdit;
				
			item.BBannerCheckout = BBannerCheckout;
				
			item.BBannerShipping = BBannerShipping;
				
			item.ShipOfferMethod = ShipOfferMethod;
				
			item.UnlockCode = UnlockCode;
				
			item.DtStartDate = DtStartDate;
				
			item.DtEndDate = DtEndDate;
				
			item.IMaxPerOrder = IMaxPerOrder;
				
			item.MMaxValue = MMaxValue;
				
			item.MWeight = MWeight;
				
			item.BDeactivateOnNoInventory = BDeactivateOnNoInventory;
				
			item.IMaxUsesPerUser = IMaxUsesPerUser;
				
			item.VcTriggerListMerch = VcTriggerListMerch;
				
			item.BAllowMultSelections = BAllowMultSelections;
				
			item.BAllowPromoTotalInMinimum = BAllowPromoTotalInMinimum;
				
			item.JsonMeta = JsonMeta;
				
	        item.Save(UserName);
	    }
    }
}
