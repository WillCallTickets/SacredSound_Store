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
    /// Controller class for DisplayOrder
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class DisplayOrderController
    {
        // Preload our schema..
        DisplayOrder thisSchemaLoad = new DisplayOrder();
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
        public DisplayOrderCollection FetchAll()
        {
            DisplayOrderCollection coll = new DisplayOrderCollection();
            Query qry = new Query(DisplayOrder.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public DisplayOrderCollection FetchByID(object Id)
        {
            DisplayOrderCollection coll = new DisplayOrderCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public DisplayOrderCollection FetchByQuery(Query qry)
        {
            DisplayOrderCollection coll = new DisplayOrderCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (DisplayOrder.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (DisplayOrder.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(DateTime DtStamp,string VcOrderContext,int IItemId,int IDisplayOrder)
	    {
		    DisplayOrder item = new DisplayOrder();
		    
            item.DtStamp = DtStamp;
            
            item.VcOrderContext = VcOrderContext;
            
            item.IItemId = IItemId;
            
            item.IDisplayOrder = IDisplayOrder;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,DateTime DtStamp,string VcOrderContext,int IItemId,int IDisplayOrder)
	    {
		    DisplayOrder item = new DisplayOrder();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.DtStamp = DtStamp;
				
			item.VcOrderContext = VcOrderContext;
				
			item.IItemId = IItemId;
				
			item.IDisplayOrder = IDisplayOrder;
				
	        item.Save(UserName);
	    }
    }
}
