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
    /// Controller class for UserCouponRedemption
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class UserCouponRedemptionController
    {
        // Preload our schema..
        UserCouponRedemption thisSchemaLoad = new UserCouponRedemption();
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
        public UserCouponRedemptionCollection FetchAll()
        {
            UserCouponRedemptionCollection coll = new UserCouponRedemptionCollection();
            Query qry = new Query(UserCouponRedemption.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public UserCouponRedemptionCollection FetchByID(object Id)
        {
            UserCouponRedemptionCollection coll = new UserCouponRedemptionCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public UserCouponRedemptionCollection FetchByQuery(Query qry)
        {
            UserCouponRedemptionCollection coll = new UserCouponRedemptionCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (UserCouponRedemption.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (UserCouponRedemption.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(DateTime DtStamp,DateTime? DtApplied,Guid UserId,int TSalePromotionId,string CouponCode,string CodeRoot,decimal MDiscountAmount,decimal MInvoiceAmount)
	    {
		    UserCouponRedemption item = new UserCouponRedemption();
		    
            item.DtStamp = DtStamp;
            
            item.DtApplied = DtApplied;
            
            item.UserId = UserId;
            
            item.TSalePromotionId = TSalePromotionId;
            
            item.CouponCode = CouponCode;
            
            item.CodeRoot = CodeRoot;
            
            item.MDiscountAmount = MDiscountAmount;
            
            item.MInvoiceAmount = MInvoiceAmount;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,DateTime DtStamp,DateTime? DtApplied,Guid UserId,int TSalePromotionId,string CouponCode,string CodeRoot,decimal MDiscountAmount,decimal MInvoiceAmount)
	    {
		    UserCouponRedemption item = new UserCouponRedemption();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.DtStamp = DtStamp;
				
			item.DtApplied = DtApplied;
				
			item.UserId = UserId;
				
			item.TSalePromotionId = TSalePromotionId;
				
			item.CouponCode = CouponCode;
				
			item.CodeRoot = CodeRoot;
				
			item.MDiscountAmount = MDiscountAmount;
				
			item.MInvoiceAmount = MInvoiceAmount;
				
	        item.Save(UserName);
	    }
    }
}
