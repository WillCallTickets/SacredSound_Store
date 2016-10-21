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
    /// Controller class for ProductAccess
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class ProductAccessController
    {
        // Preload our schema..
        ProductAccess thisSchemaLoad = new ProductAccess();
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
        public ProductAccessCollection FetchAll()
        {
            ProductAccessCollection coll = new ProductAccessCollection();
            Query qry = new Query(ProductAccess.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public ProductAccessCollection FetchByID(object Id)
        {
            ProductAccessCollection coll = new ProductAccessCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public ProductAccessCollection FetchByQuery(Query qry)
        {
            ProductAccessCollection coll = new ProductAccessCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (ProductAccess.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (ProductAccess.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(DateTime DtStamp,Guid ApplicationId,bool BActive,string CampaignName,string CampaignCode,int IDisplayOrder)
	    {
		    ProductAccess item = new ProductAccess();
		    
            item.DtStamp = DtStamp;
            
            item.ApplicationId = ApplicationId;
            
            item.BActive = BActive;
            
            item.CampaignName = CampaignName;
            
            item.CampaignCode = CampaignCode;
            
            item.IDisplayOrder = IDisplayOrder;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,DateTime DtStamp,Guid ApplicationId,bool BActive,string CampaignName,string CampaignCode,int IDisplayOrder)
	    {
		    ProductAccess item = new ProductAccess();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.DtStamp = DtStamp;
				
			item.ApplicationId = ApplicationId;
				
			item.BActive = BActive;
				
			item.CampaignName = CampaignName;
				
			item.CampaignCode = CampaignCode;
				
			item.IDisplayOrder = IDisplayOrder;
				
	        item.Save(UserName);
	    }
    }
}
