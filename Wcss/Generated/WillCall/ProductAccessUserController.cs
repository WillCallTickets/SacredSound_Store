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
    /// Controller class for ProductAccessUser
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class ProductAccessUserController
    {
        // Preload our schema..
        ProductAccessUser thisSchemaLoad = new ProductAccessUser();
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
        public ProductAccessUserCollection FetchAll()
        {
            ProductAccessUserCollection coll = new ProductAccessUserCollection();
            Query qry = new Query(ProductAccessUser.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public ProductAccessUserCollection FetchByID(object Id)
        {
            ProductAccessUserCollection coll = new ProductAccessUserCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public ProductAccessUserCollection FetchByQuery(Query qry)
        {
            ProductAccessUserCollection coll = new ProductAccessUserCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (ProductAccessUser.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (ProductAccessUser.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(DateTime DtStamp,int TProductAccessId,string UserName,Guid? UserId,int IQuantityAllowed,string Referral,string Instructions)
	    {
		    ProductAccessUser item = new ProductAccessUser();
		    
            item.DtStamp = DtStamp;
            
            item.TProductAccessId = TProductAccessId;
            
            item.UserName = UserName;
            
            item.UserId = UserId;
            
            item.IQuantityAllowed = IQuantityAllowed;
            
            item.Referral = Referral;
            
            item.Instructions = Instructions;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,DateTime DtStamp,int TProductAccessId,string UserName,Guid? UserId,int IQuantityAllowed,string Referral,string Instructions)
	    {
		    ProductAccessUser item = new ProductAccessUser();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.DtStamp = DtStamp;
				
			item.TProductAccessId = TProductAccessId;
				
			item.UserName = UserName;
				
			item.UserId = UserId;
				
			item.IQuantityAllowed = IQuantityAllowed;
				
			item.Referral = Referral;
				
			item.Instructions = Instructions;
				
	        item.Save(UserName);
	    }
    }
}
