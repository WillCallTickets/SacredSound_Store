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
    /// Controller class for SalePromotionTrigger
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class SalePromotionTriggerController
    {
        // Preload our schema..
        SalePromotionTrigger thisSchemaLoad = new SalePromotionTrigger();
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
        public SalePromotionTriggerCollection FetchAll()
        {
            SalePromotionTriggerCollection coll = new SalePromotionTriggerCollection();
            Query qry = new Query(SalePromotionTrigger.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public SalePromotionTriggerCollection FetchByID(object Id)
        {
            SalePromotionTriggerCollection coll = new SalePromotionTriggerCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public SalePromotionTriggerCollection FetchByQuery(Query qry)
        {
            SalePromotionTriggerCollection coll = new SalePromotionTriggerCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (SalePromotionTrigger.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (SalePromotionTrigger.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(DateTime DtStamp,int TSalePromotionId,int TMerchId)
	    {
		    SalePromotionTrigger item = new SalePromotionTrigger();
		    
            item.DtStamp = DtStamp;
            
            item.TSalePromotionId = TSalePromotionId;
            
            item.TMerchId = TMerchId;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,DateTime DtStamp,int TSalePromotionId,int TMerchId)
	    {
		    SalePromotionTrigger item = new SalePromotionTrigger();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.DtStamp = DtStamp;
				
			item.TSalePromotionId = TSalePromotionId;
				
			item.TMerchId = TMerchId;
				
	        item.Save(UserName);
	    }
    }
}
