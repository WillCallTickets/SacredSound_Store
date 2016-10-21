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
    /// Controller class for FraudScreen
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class FraudScreenController
    {
        // Preload our schema..
        FraudScreen thisSchemaLoad = new FraudScreen();
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
        public FraudScreenCollection FetchAll()
        {
            FraudScreenCollection coll = new FraudScreenCollection();
            Query qry = new Query(FraudScreen.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public FraudScreenCollection FetchByID(object Id)
        {
            FraudScreenCollection coll = new FraudScreenCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public FraudScreenCollection FetchByQuery(Query qry)
        {
            FraudScreenCollection coll = new FraudScreenCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (FraudScreen.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (FraudScreen.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(DateTime DtStamp,Guid CreatedById,string CreatedBy,string VcAction,Guid? UserId,string FirstName,string Mi,string LastName,string FullName,string NameOnCard,string City,string Zip,string CreditCardNum,string LastFourDigits,string UserIp,Guid ApplicationId)
	    {
		    FraudScreen item = new FraudScreen();
		    
            item.DtStamp = DtStamp;
            
            item.CreatedById = CreatedById;
            
            item.CreatedBy = CreatedBy;
            
            item.VcAction = VcAction;
            
            item.UserId = UserId;
            
            item.FirstName = FirstName;
            
            item.Mi = Mi;
            
            item.LastName = LastName;
            
            item.FullName = FullName;
            
            item.NameOnCard = NameOnCard;
            
            item.City = City;
            
            item.Zip = Zip;
            
            item.CreditCardNum = CreditCardNum;
            
            item.LastFourDigits = LastFourDigits;
            
            item.UserIp = UserIp;
            
            item.ApplicationId = ApplicationId;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,DateTime DtStamp,Guid CreatedById,string CreatedBy,string VcAction,Guid? UserId,string FirstName,string Mi,string LastName,string FullName,string NameOnCard,string City,string Zip,string CreditCardNum,string LastFourDigits,string UserIp,Guid ApplicationId)
	    {
		    FraudScreen item = new FraudScreen();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.DtStamp = DtStamp;
				
			item.CreatedById = CreatedById;
				
			item.CreatedBy = CreatedBy;
				
			item.VcAction = VcAction;
				
			item.UserId = UserId;
				
			item.FirstName = FirstName;
				
			item.Mi = Mi;
				
			item.LastName = LastName;
				
			item.FullName = FullName;
				
			item.NameOnCard = NameOnCard;
				
			item.City = City;
				
			item.Zip = Zip;
				
			item.CreditCardNum = CreditCardNum;
				
			item.LastFourDigits = LastFourDigits;
				
			item.UserIp = UserIp;
				
			item.ApplicationId = ApplicationId;
				
	        item.Save(UserName);
	    }
    }
}
