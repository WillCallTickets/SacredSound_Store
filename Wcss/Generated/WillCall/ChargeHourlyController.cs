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
    /// Controller class for Charge_Hourly
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class ChargeHourlyController
    {
        // Preload our schema..
        ChargeHourly thisSchemaLoad = new ChargeHourly();
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
        public ChargeHourlyCollection FetchAll()
        {
            ChargeHourlyCollection coll = new ChargeHourlyCollection();
            Query qry = new Query(ChargeHourly.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public ChargeHourlyCollection FetchByID(object Id)
        {
            ChargeHourlyCollection coll = new ChargeHourlyCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public ChargeHourlyCollection FetchByQuery(Query qry)
        {
            ChargeHourlyCollection coll = new ChargeHourlyCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (ChargeHourly.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (ChargeHourly.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(DateTime DtStamp,int TChargeStatementId,DateTime DtPerformed,string ServiceDescription,int Hours,decimal Rate,decimal FlatRate,decimal? LineTotal)
	    {
		    ChargeHourly item = new ChargeHourly();
		    
            item.DtStamp = DtStamp;
            
            item.TChargeStatementId = TChargeStatementId;
            
            item.DtPerformed = DtPerformed;
            
            item.ServiceDescription = ServiceDescription;
            
            item.Hours = Hours;
            
            item.Rate = Rate;
            
            item.FlatRate = FlatRate;
            
            item.LineTotal = LineTotal;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,DateTime DtStamp,int TChargeStatementId,DateTime DtPerformed,string ServiceDescription,int Hours,decimal Rate,decimal FlatRate,decimal? LineTotal)
	    {
		    ChargeHourly item = new ChargeHourly();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.DtStamp = DtStamp;
				
			item.TChargeStatementId = TChargeStatementId;
				
			item.DtPerformed = DtPerformed;
				
			item.ServiceDescription = ServiceDescription;
				
			item.Hours = Hours;
				
			item.Rate = Rate;
				
			item.FlatRate = FlatRate;
				
			item.LineTotal = LineTotal;
				
	        item.Save(UserName);
	    }
    }
}
