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
    /// Controller class for Cashew
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class CashewController
    {
        // Preload our schema..
        Cashew thisSchemaLoad = new Cashew();
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
        public CashewCollection FetchAll()
        {
            CashewCollection coll = new CashewCollection();
            Query qry = new Query(Cashew.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public CashewCollection FetchByID(object Id)
        {
            CashewCollection coll = new CashewCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public CashewCollection FetchByQuery(Query qry)
        {
            CashewCollection coll = new CashewCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (Cashew.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (Cashew.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string ENumber,string EMonth,string EYear,string EName,Guid UserId,int CustomerId,DateTime DtStamp)
	    {
		    Cashew item = new Cashew();
		    
            item.ENumber = ENumber;
            
            item.EMonth = EMonth;
            
            item.EYear = EYear;
            
            item.EName = EName;
            
            item.UserId = UserId;
            
            item.CustomerId = CustomerId;
            
            item.DtStamp = DtStamp;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,string ENumber,string EMonth,string EYear,string EName,Guid UserId,int CustomerId,DateTime DtStamp)
	    {
		    Cashew item = new Cashew();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.ENumber = ENumber;
				
			item.EMonth = EMonth;
				
			item.EYear = EYear;
				
			item.EName = EName;
				
			item.UserId = UserId;
				
			item.CustomerId = CustomerId;
				
			item.DtStamp = DtStamp;
				
	        item.Save(UserName);
	    }
    }
}
