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
    /// Controller class for Required_InvoiceFee
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class RequiredInvoiceFeeController
    {
        // Preload our schema..
        RequiredInvoiceFee thisSchemaLoad = new RequiredInvoiceFee();
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
        public RequiredInvoiceFeeCollection FetchAll()
        {
            RequiredInvoiceFeeCollection coll = new RequiredInvoiceFeeCollection();
            Query qry = new Query(RequiredInvoiceFee.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public RequiredInvoiceFeeCollection FetchByID(object Id)
        {
            RequiredInvoiceFeeCollection coll = new RequiredInvoiceFeeCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public RequiredInvoiceFeeCollection FetchByQuery(Query qry)
        {
            RequiredInvoiceFeeCollection coll = new RequiredInvoiceFeeCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (RequiredInvoiceFee.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (RequiredInvoiceFee.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(DateTime DtStamp,int TInvoiceFeeId,int TRequiredId)
	    {
		    RequiredInvoiceFee item = new RequiredInvoiceFee();
		    
            item.DtStamp = DtStamp;
            
            item.TInvoiceFeeId = TInvoiceFeeId;
            
            item.TRequiredId = TRequiredId;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,DateTime DtStamp,int TInvoiceFeeId,int TRequiredId)
	    {
		    RequiredInvoiceFee item = new RequiredInvoiceFee();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.DtStamp = DtStamp;
				
			item.TInvoiceFeeId = TInvoiceFeeId;
				
			item.TRequiredId = TRequiredId;
				
	        item.Save(UserName);
	    }
    }
}
