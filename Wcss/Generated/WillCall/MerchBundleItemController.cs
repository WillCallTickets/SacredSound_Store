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
    /// Controller class for MerchBundleItem
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class MerchBundleItemController
    {
        // Preload our schema..
        MerchBundleItem thisSchemaLoad = new MerchBundleItem();
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
        public MerchBundleItemCollection FetchAll()
        {
            MerchBundleItemCollection coll = new MerchBundleItemCollection();
            Query qry = new Query(MerchBundleItem.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public MerchBundleItemCollection FetchByID(object Id)
        {
            MerchBundleItemCollection coll = new MerchBundleItemCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public MerchBundleItemCollection FetchByQuery(Query qry)
        {
            MerchBundleItemCollection coll = new MerchBundleItemCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (MerchBundleItem.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (MerchBundleItem.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(DateTime DtStamp,bool BActive,int TMerchBundleId,int? TMerchId,int IDisplayOrder)
	    {
		    MerchBundleItem item = new MerchBundleItem();
		    
            item.DtStamp = DtStamp;
            
            item.BActive = BActive;
            
            item.TMerchBundleId = TMerchBundleId;
            
            item.TMerchId = TMerchId;
            
            item.IDisplayOrder = IDisplayOrder;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,DateTime DtStamp,bool BActive,int TMerchBundleId,int? TMerchId,int IDisplayOrder)
	    {
		    MerchBundleItem item = new MerchBundleItem();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.DtStamp = DtStamp;
				
			item.BActive = BActive;
				
			item.TMerchBundleId = TMerchBundleId;
				
			item.TMerchId = TMerchId;
				
			item.IDisplayOrder = IDisplayOrder;
				
	        item.Save(UserName);
	    }
    }
}
