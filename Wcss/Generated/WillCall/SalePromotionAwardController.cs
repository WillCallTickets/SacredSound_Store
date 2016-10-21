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
    /// Controller class for SalePromotionAward
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class SalePromotionAwardController
    {
        // Preload our schema..
        SalePromotionAward thisSchemaLoad = new SalePromotionAward();
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
        public SalePromotionAwardCollection FetchAll()
        {
            SalePromotionAwardCollection coll = new SalePromotionAwardCollection();
            Query qry = new Query(SalePromotionAward.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public SalePromotionAwardCollection FetchByID(object Id)
        {
            SalePromotionAwardCollection coll = new SalePromotionAwardCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public SalePromotionAwardCollection FetchByQuery(Query qry)
        {
            SalePromotionAwardCollection coll = new SalePromotionAwardCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (SalePromotionAward.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (SalePromotionAward.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(DateTime DtStamp,bool BActive,int TSalePromotionId,int? TParentMerchId)
	    {
		    SalePromotionAward item = new SalePromotionAward();
		    
            item.DtStamp = DtStamp;
            
            item.BActive = BActive;
            
            item.TSalePromotionId = TSalePromotionId;
            
            item.TParentMerchId = TParentMerchId;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,DateTime DtStamp,bool BActive,int TSalePromotionId,int? TParentMerchId)
	    {
		    SalePromotionAward item = new SalePromotionAward();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.DtStamp = DtStamp;
				
			item.BActive = BActive;
				
			item.TSalePromotionId = TSalePromotionId;
				
			item.TParentMerchId = TParentMerchId;
				
	        item.Save(UserName);
	    }
    }
}
