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
    /// Controller class for MerchStock
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class MerchStockController
    {
        // Preload our schema..
        MerchStock thisSchemaLoad = new MerchStock();
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
        public MerchStockCollection FetchAll()
        {
            MerchStockCollection coll = new MerchStockCollection();
            Query qry = new Query(MerchStock.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public MerchStockCollection FetchByID(object Guid)
        {
            MerchStockCollection coll = new MerchStockCollection().Where("GUID", Guid).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public MerchStockCollection FetchByQuery(Query qry)
        {
            MerchStockCollection coll = new MerchStockCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Guid)
        {
            return (MerchStock.Delete(Guid) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Guid)
        {
            return (MerchStock.Destroy(Guid) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(Guid Guid,string SessionId,string UserName,int TMerchId,int IQty,DateTime DtTTL,DateTime DtStamp)
	    {
		    MerchStock item = new MerchStock();
		    
            item.Guid = Guid;
            
            item.SessionId = SessionId;
            
            item.UserName = UserName;
            
            item.TMerchId = TMerchId;
            
            item.IQty = IQty;
            
            item.DtTTL = DtTTL;
            
            item.DtStamp = DtStamp;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,Guid Guid,string SessionId,string UserName,int TMerchId,int IQty,DateTime DtTTL,DateTime DtStamp)
	    {
		    MerchStock item = new MerchStock();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.Guid = Guid;
				
			item.SessionId = SessionId;
				
			item.UserName = UserName;
				
			item.TMerchId = TMerchId;
				
			item.IQty = IQty;
				
			item.DtTTL = DtTTL;
				
			item.DtStamp = DtStamp;
				
	        item.Save(UserName);
	    }
    }
}
