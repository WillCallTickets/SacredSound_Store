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
    /// Controller class for MerchBundle
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class MerchBundleController
    {
        // Preload our schema..
        MerchBundle thisSchemaLoad = new MerchBundle();
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
        public MerchBundleCollection FetchAll()
        {
            MerchBundleCollection coll = new MerchBundleCollection();
            Query qry = new Query(MerchBundle.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public MerchBundleCollection FetchByID(object Id)
        {
            MerchBundleCollection coll = new MerchBundleCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public MerchBundleCollection FetchByQuery(Query qry)
        {
            MerchBundleCollection coll = new MerchBundleCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (MerchBundle.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (MerchBundle.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(DateTime DtStamp,bool BActive,int IDisplayOrder,int? TMerchId,string Title,string Comment,int IRequiredParentQty,int IMaxSelections,decimal MPrice,bool? BPricedPerSelection,bool BIncludeWeight,int? TShowTicketId)
	    {
		    MerchBundle item = new MerchBundle();
		    
            item.DtStamp = DtStamp;
            
            item.BActive = BActive;
            
            item.IDisplayOrder = IDisplayOrder;
            
            item.TMerchId = TMerchId;
            
            item.Title = Title;
            
            item.Comment = Comment;
            
            item.IRequiredParentQty = IRequiredParentQty;
            
            item.IMaxSelections = IMaxSelections;
            
            item.MPrice = MPrice;
            
            item.BPricedPerSelection = BPricedPerSelection;
            
            item.BIncludeWeight = BIncludeWeight;
            
            item.TShowTicketId = TShowTicketId;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,DateTime DtStamp,bool BActive,int IDisplayOrder,int? TMerchId,string Title,string Comment,int IRequiredParentQty,int IMaxSelections,decimal MPrice,bool? BPricedPerSelection,bool BIncludeWeight,int? TShowTicketId)
	    {
		    MerchBundle item = new MerchBundle();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.DtStamp = DtStamp;
				
			item.BActive = BActive;
				
			item.IDisplayOrder = IDisplayOrder;
				
			item.TMerchId = TMerchId;
				
			item.Title = Title;
				
			item.Comment = Comment;
				
			item.IRequiredParentQty = IRequiredParentQty;
				
			item.IMaxSelections = IMaxSelections;
				
			item.MPrice = MPrice;
				
			item.BPricedPerSelection = BPricedPerSelection;
				
			item.BIncludeWeight = BIncludeWeight;
				
			item.TShowTicketId = TShowTicketId;
				
	        item.Save(UserName);
	    }
    }
}
