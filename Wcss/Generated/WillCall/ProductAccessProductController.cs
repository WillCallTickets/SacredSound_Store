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
    /// Controller class for ProductAccessProduct
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class ProductAccessProductController
    {
        // Preload our schema..
        ProductAccessProduct thisSchemaLoad = new ProductAccessProduct();
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
        public ProductAccessProductCollection FetchAll()
        {
            ProductAccessProductCollection coll = new ProductAccessProductCollection();
            Query qry = new Query(ProductAccessProduct.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public ProductAccessProductCollection FetchByID(object Id)
        {
            ProductAccessProductCollection coll = new ProductAccessProductCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public ProductAccessProductCollection FetchByQuery(Query qry)
        {
            ProductAccessProductCollection coll = new ProductAccessProductCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (ProductAccessProduct.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (ProductAccessProduct.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(DateTime DtStamp,int TProductAccessId,string VcContext,int TParentId)
	    {
		    ProductAccessProduct item = new ProductAccessProduct();
		    
            item.DtStamp = DtStamp;
            
            item.TProductAccessId = TProductAccessId;
            
            item.VcContext = VcContext;
            
            item.TParentId = TParentId;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,DateTime DtStamp,int TProductAccessId,string VcContext,int TParentId)
	    {
		    ProductAccessProduct item = new ProductAccessProduct();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.DtStamp = DtStamp;
				
			item.TProductAccessId = TProductAccessId;
				
			item.VcContext = VcContext;
				
			item.TParentId = TParentId;
				
	        item.Save(UserName);
	    }
    }
}
