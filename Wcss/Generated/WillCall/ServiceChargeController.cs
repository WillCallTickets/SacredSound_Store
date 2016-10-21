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
    /// Controller class for ServiceCharge
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class ServiceChargeController
    {
        // Preload our schema..
        ServiceCharge thisSchemaLoad = new ServiceCharge();
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
        public ServiceChargeCollection FetchAll()
        {
            ServiceChargeCollection coll = new ServiceChargeCollection();
            Query qry = new Query(ServiceCharge.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public ServiceChargeCollection FetchByID(object Id)
        {
            ServiceChargeCollection coll = new ServiceChargeCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public ServiceChargeCollection FetchByQuery(Query qry)
        {
            ServiceChargeCollection coll = new ServiceChargeCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (ServiceCharge.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (ServiceCharge.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(DateTime DtStamp,Guid ApplicationId,decimal MMaxValue,decimal MCharge,decimal MPercentage)
	    {
		    ServiceCharge item = new ServiceCharge();
		    
            item.DtStamp = DtStamp;
            
            item.ApplicationId = ApplicationId;
            
            item.MMaxValue = MMaxValue;
            
            item.MCharge = MCharge;
            
            item.MPercentage = MPercentage;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,DateTime DtStamp,Guid ApplicationId,decimal MMaxValue,decimal MCharge,decimal MPercentage)
	    {
		    ServiceCharge item = new ServiceCharge();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.DtStamp = DtStamp;
				
			item.ApplicationId = ApplicationId;
				
			item.MMaxValue = MMaxValue;
				
			item.MCharge = MCharge;
				
			item.MPercentage = MPercentage;
				
	        item.Save(UserName);
	    }
    }
}
