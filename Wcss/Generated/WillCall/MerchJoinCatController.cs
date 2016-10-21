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
    /// Controller class for MerchJoinCat
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class MerchJoinCatController
    {
        // Preload our schema..
        MerchJoinCat thisSchemaLoad = new MerchJoinCat();
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
        public MerchJoinCatCollection FetchAll()
        {
            MerchJoinCatCollection coll = new MerchJoinCatCollection();
            Query qry = new Query(MerchJoinCat.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public MerchJoinCatCollection FetchByID(object Id)
        {
            MerchJoinCatCollection coll = new MerchJoinCatCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public MerchJoinCatCollection FetchByQuery(Query qry)
        {
            MerchJoinCatCollection coll = new MerchJoinCatCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (MerchJoinCat.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (MerchJoinCat.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(int TMerchId,int TMerchCategorieId,int IDisplayOrder,DateTime DtStamp)
	    {
		    MerchJoinCat item = new MerchJoinCat();
		    
            item.TMerchId = TMerchId;
            
            item.TMerchCategorieId = TMerchCategorieId;
            
            item.IDisplayOrder = IDisplayOrder;
            
            item.DtStamp = DtStamp;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,int TMerchId,int TMerchCategorieId,int IDisplayOrder,DateTime DtStamp)
	    {
		    MerchJoinCat item = new MerchJoinCat();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.TMerchId = TMerchId;
				
			item.TMerchCategorieId = TMerchCategorieId;
				
			item.IDisplayOrder = IDisplayOrder;
				
			item.DtStamp = DtStamp;
				
	        item.Save(UserName);
	    }
    }
}
