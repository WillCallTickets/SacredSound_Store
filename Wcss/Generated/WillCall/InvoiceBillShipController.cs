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
    /// Controller class for InvoiceBillShip
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class InvoiceBillShipController
    {
        // Preload our schema..
        InvoiceBillShip thisSchemaLoad = new InvoiceBillShip();
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
        public InvoiceBillShipCollection FetchAll()
        {
            InvoiceBillShipCollection coll = new InvoiceBillShipCollection();
            Query qry = new Query(InvoiceBillShip.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public InvoiceBillShipCollection FetchByID(object Id)
        {
            InvoiceBillShipCollection coll = new InvoiceBillShipCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public InvoiceBillShipCollection FetchByQuery(Query qry)
        {
            InvoiceBillShipCollection coll = new InvoiceBillShipCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (InvoiceBillShip.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (InvoiceBillShip.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(int TInvoiceId,Guid? UserId,int CustomerId,string BlCompany,string BlFirstName,string BlLastName,string BlAddress1,string BlAddress2,string BlCity,string BlStateProvince,string BlPostalCode,string BlCountry,string BlPhone,bool BSameAsBilling,string CompanyName,string FirstName,string LastName,string Address1,string Address2,string City,string StateProvince,string PostalCode,string Country,string Phone,string ShipMessage,DateTime? DtShipped,string TrackingInformation,Guid? ReferenceNumber,decimal? MActualShipping,decimal? MHandlingComputed,DateTime DtStamp)
	    {
		    InvoiceBillShip item = new InvoiceBillShip();
		    
            item.TInvoiceId = TInvoiceId;
            
            item.UserId = UserId;
            
            item.CustomerId = CustomerId;
            
            item.BlCompany = BlCompany;
            
            item.BlFirstName = BlFirstName;
            
            item.BlLastName = BlLastName;
            
            item.BlAddress1 = BlAddress1;
            
            item.BlAddress2 = BlAddress2;
            
            item.BlCity = BlCity;
            
            item.BlStateProvince = BlStateProvince;
            
            item.BlPostalCode = BlPostalCode;
            
            item.BlCountry = BlCountry;
            
            item.BlPhone = BlPhone;
            
            item.BSameAsBilling = BSameAsBilling;
            
            item.CompanyName = CompanyName;
            
            item.FirstName = FirstName;
            
            item.LastName = LastName;
            
            item.Address1 = Address1;
            
            item.Address2 = Address2;
            
            item.City = City;
            
            item.StateProvince = StateProvince;
            
            item.PostalCode = PostalCode;
            
            item.Country = Country;
            
            item.Phone = Phone;
            
            item.ShipMessage = ShipMessage;
            
            item.DtShipped = DtShipped;
            
            item.TrackingInformation = TrackingInformation;
            
            item.ReferenceNumber = ReferenceNumber;
            
            item.MActualShipping = MActualShipping;
            
            item.MHandlingComputed = MHandlingComputed;
            
            item.DtStamp = DtStamp;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,int TInvoiceId,Guid? UserId,int CustomerId,string BlCompany,string BlFirstName,string BlLastName,string BlAddress1,string BlAddress2,string BlCity,string BlStateProvince,string BlPostalCode,string BlCountry,string BlPhone,bool BSameAsBilling,string CompanyName,string FirstName,string LastName,string Address1,string Address2,string City,string StateProvince,string PostalCode,string Country,string Phone,string ShipMessage,DateTime? DtShipped,string TrackingInformation,Guid? ReferenceNumber,decimal? MActualShipping,decimal? MHandlingComputed,DateTime DtStamp)
	    {
		    InvoiceBillShip item = new InvoiceBillShip();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.TInvoiceId = TInvoiceId;
				
			item.UserId = UserId;
				
			item.CustomerId = CustomerId;
				
			item.BlCompany = BlCompany;
				
			item.BlFirstName = BlFirstName;
				
			item.BlLastName = BlLastName;
				
			item.BlAddress1 = BlAddress1;
				
			item.BlAddress2 = BlAddress2;
				
			item.BlCity = BlCity;
				
			item.BlStateProvince = BlStateProvince;
				
			item.BlPostalCode = BlPostalCode;
				
			item.BlCountry = BlCountry;
				
			item.BlPhone = BlPhone;
				
			item.BSameAsBilling = BSameAsBilling;
				
			item.CompanyName = CompanyName;
				
			item.FirstName = FirstName;
				
			item.LastName = LastName;
				
			item.Address1 = Address1;
				
			item.Address2 = Address2;
				
			item.City = City;
				
			item.StateProvince = StateProvince;
				
			item.PostalCode = PostalCode;
				
			item.Country = Country;
				
			item.Phone = Phone;
				
			item.ShipMessage = ShipMessage;
				
			item.DtShipped = DtShipped;
				
			item.TrackingInformation = TrackingInformation;
				
			item.ReferenceNumber = ReferenceNumber;
				
			item.MActualShipping = MActualShipping;
				
			item.MHandlingComputed = MHandlingComputed;
				
			item.DtStamp = DtStamp;
				
	        item.Save(UserName);
	    }
    }
}
