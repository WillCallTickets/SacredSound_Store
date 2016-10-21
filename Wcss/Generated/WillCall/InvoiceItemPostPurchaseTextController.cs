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
    /// Controller class for InvoiceItemPostPurchaseText
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class InvoiceItemPostPurchaseTextController
    {
        // Preload our schema..
        InvoiceItemPostPurchaseText thisSchemaLoad = new InvoiceItemPostPurchaseText();
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
        public InvoiceItemPostPurchaseTextCollection FetchAll()
        {
            InvoiceItemPostPurchaseTextCollection coll = new InvoiceItemPostPurchaseTextCollection();
            Query qry = new Query(InvoiceItemPostPurchaseText.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public InvoiceItemPostPurchaseTextCollection FetchByID(object Id)
        {
            InvoiceItemPostPurchaseTextCollection coll = new InvoiceItemPostPurchaseTextCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public InvoiceItemPostPurchaseTextCollection FetchByQuery(Query qry)
        {
            InvoiceItemPostPurchaseTextCollection coll = new InvoiceItemPostPurchaseTextCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (InvoiceItemPostPurchaseText.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (InvoiceItemPostPurchaseText.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(DateTime DtStamp,int TInvoiceItemId,int TPostPurchaseTextId,string PostText,int IDisplayOrder)
	    {
		    InvoiceItemPostPurchaseText item = new InvoiceItemPostPurchaseText();
		    
            item.DtStamp = DtStamp;
            
            item.TInvoiceItemId = TInvoiceItemId;
            
            item.TPostPurchaseTextId = TPostPurchaseTextId;
            
            item.PostText = PostText;
            
            item.IDisplayOrder = IDisplayOrder;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,DateTime DtStamp,int TInvoiceItemId,int TPostPurchaseTextId,string PostText,int IDisplayOrder)
	    {
		    InvoiceItemPostPurchaseText item = new InvoiceItemPostPurchaseText();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.DtStamp = DtStamp;
				
			item.TInvoiceItemId = TInvoiceItemId;
				
			item.TPostPurchaseTextId = TPostPurchaseTextId;
				
			item.PostText = PostText;
				
			item.IDisplayOrder = IDisplayOrder;
				
	        item.Save(UserName);
	    }
    }
}
