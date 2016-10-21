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
    /// Controller class for Inventory
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class InventoryController
    {
        // Preload our schema..
        Inventory thisSchemaLoad = new Inventory();
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
        public InventoryCollection FetchAll()
        {
            InventoryCollection coll = new InventoryCollection();
            Query qry = new Query(Inventory.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public InventoryCollection FetchByID(object Id)
        {
            InventoryCollection coll = new InventoryCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public InventoryCollection FetchByQuery(Query qry)
        {
            InventoryCollection coll = new InventoryCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (Inventory.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (Inventory.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(DateTime DtStamp,string VcParentContext,int IParentInventoryId,string Code,string Description,Guid? GSaleItemId,int? TInvoiceItemId,DateTime? DtSold,Guid? UserId,DateTime? DtRedeemed,string IpRedeemed)
	    {
		    Inventory item = new Inventory();
		    
            item.DtStamp = DtStamp;
            
            item.VcParentContext = VcParentContext;
            
            item.IParentInventoryId = IParentInventoryId;
            
            item.Code = Code;
            
            item.Description = Description;
            
            item.GSaleItemId = GSaleItemId;
            
            item.TInvoiceItemId = TInvoiceItemId;
            
            item.DtSold = DtSold;
            
            item.UserId = UserId;
            
            item.DtRedeemed = DtRedeemed;
            
            item.IpRedeemed = IpRedeemed;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,DateTime DtStamp,string VcParentContext,int IParentInventoryId,string Code,string Description,Guid? GSaleItemId,int? TInvoiceItemId,DateTime? DtSold,Guid? UserId,DateTime? DtRedeemed,string IpRedeemed)
	    {
		    Inventory item = new Inventory();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.DtStamp = DtStamp;
				
			item.VcParentContext = VcParentContext;
				
			item.IParentInventoryId = IParentInventoryId;
				
			item.Code = Code;
				
			item.Description = Description;
				
			item.GSaleItemId = GSaleItemId;
				
			item.TInvoiceItemId = TInvoiceItemId;
				
			item.DtSold = DtSold;
				
			item.UserId = UserId;
				
			item.DtRedeemed = DtRedeemed;
				
			item.IpRedeemed = IpRedeemed;
				
	        item.Save(UserName);
	    }
    }
}
