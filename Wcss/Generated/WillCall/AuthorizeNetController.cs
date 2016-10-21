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
    /// Controller class for AuthorizeNet
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class AuthorizeNetController
    {
        // Preload our schema..
        AuthorizeNet thisSchemaLoad = new AuthorizeNet();
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
        public AuthorizeNetCollection FetchAll()
        {
            AuthorizeNetCollection coll = new AuthorizeNetCollection();
            Query qry = new Query(AuthorizeNet.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public AuthorizeNetCollection FetchByID(object Id)
        {
            AuthorizeNetCollection coll = new AuthorizeNetCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public AuthorizeNetCollection FetchByQuery(Query qry)
        {
            AuthorizeNetCollection coll = new AuthorizeNetCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (AuthorizeNet.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (AuthorizeNet.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string InvoiceNumber,bool? BAuthorized,int? TInvoiceId,Guid UserId,int? CustomerId,string ProcessorId,string Method,string TransactionType,decimal? MAmount,decimal? MTaxAmount,decimal? MFreightAmount,string Description,int? IDupeSeconds,int? IResponseCode,string ResponseSubcode,int? IResponseReasonCode,bool? BMd5Match,string ResponseReasonText,string ApprovalCode,string AVSResultCode,string CardCodeResponseCode,string Email,string FirstName,string LastName,string NameOnCard,string Company,string BillingAddress,string City,string State,string Zip,string Country,string Phone,string IpAddress,string ShipToFirstName,string ShipToLastName,string ShipToCompany,string ShipToAddress,string ShipToCity,string ShipToState,string ShipToZip,string ShipToCountry,DateTime DtStamp,Guid ApplicationId)
	    {
		    AuthorizeNet item = new AuthorizeNet();
		    
            item.InvoiceNumber = InvoiceNumber;
            
            item.BAuthorized = BAuthorized;
            
            item.TInvoiceId = TInvoiceId;
            
            item.UserId = UserId;
            
            item.CustomerId = CustomerId;
            
            item.ProcessorId = ProcessorId;
            
            item.Method = Method;
            
            item.TransactionType = TransactionType;
            
            item.MAmount = MAmount;
            
            item.MTaxAmount = MTaxAmount;
            
            item.MFreightAmount = MFreightAmount;
            
            item.Description = Description;
            
            item.IDupeSeconds = IDupeSeconds;
            
            item.IResponseCode = IResponseCode;
            
            item.ResponseSubcode = ResponseSubcode;
            
            item.IResponseReasonCode = IResponseReasonCode;
            
            item.BMd5Match = BMd5Match;
            
            item.ResponseReasonText = ResponseReasonText;
            
            item.ApprovalCode = ApprovalCode;
            
            item.AVSResultCode = AVSResultCode;
            
            item.CardCodeResponseCode = CardCodeResponseCode;
            
            item.Email = Email;
            
            item.FirstName = FirstName;
            
            item.LastName = LastName;
            
            item.NameOnCard = NameOnCard;
            
            item.Company = Company;
            
            item.BillingAddress = BillingAddress;
            
            item.City = City;
            
            item.State = State;
            
            item.Zip = Zip;
            
            item.Country = Country;
            
            item.Phone = Phone;
            
            item.IpAddress = IpAddress;
            
            item.ShipToFirstName = ShipToFirstName;
            
            item.ShipToLastName = ShipToLastName;
            
            item.ShipToCompany = ShipToCompany;
            
            item.ShipToAddress = ShipToAddress;
            
            item.ShipToCity = ShipToCity;
            
            item.ShipToState = ShipToState;
            
            item.ShipToZip = ShipToZip;
            
            item.ShipToCountry = ShipToCountry;
            
            item.DtStamp = DtStamp;
            
            item.ApplicationId = ApplicationId;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,string InvoiceNumber,bool? BAuthorized,int? TInvoiceId,Guid UserId,int? CustomerId,string ProcessorId,string Method,string TransactionType,decimal? MAmount,decimal? MTaxAmount,decimal? MFreightAmount,string Description,int? IDupeSeconds,int? IResponseCode,string ResponseSubcode,int? IResponseReasonCode,bool? BMd5Match,string ResponseReasonText,string ApprovalCode,string AVSResultCode,string CardCodeResponseCode,string Email,string FirstName,string LastName,string NameOnCard,string Company,string BillingAddress,string City,string State,string Zip,string Country,string Phone,string IpAddress,string ShipToFirstName,string ShipToLastName,string ShipToCompany,string ShipToAddress,string ShipToCity,string ShipToState,string ShipToZip,string ShipToCountry,DateTime DtStamp,Guid ApplicationId)
	    {
		    AuthorizeNet item = new AuthorizeNet();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.InvoiceNumber = InvoiceNumber;
				
			item.BAuthorized = BAuthorized;
				
			item.TInvoiceId = TInvoiceId;
				
			item.UserId = UserId;
				
			item.CustomerId = CustomerId;
				
			item.ProcessorId = ProcessorId;
				
			item.Method = Method;
				
			item.TransactionType = TransactionType;
				
			item.MAmount = MAmount;
				
			item.MTaxAmount = MTaxAmount;
				
			item.MFreightAmount = MFreightAmount;
				
			item.Description = Description;
				
			item.IDupeSeconds = IDupeSeconds;
				
			item.IResponseCode = IResponseCode;
				
			item.ResponseSubcode = ResponseSubcode;
				
			item.IResponseReasonCode = IResponseReasonCode;
				
			item.BMd5Match = BMd5Match;
				
			item.ResponseReasonText = ResponseReasonText;
				
			item.ApprovalCode = ApprovalCode;
				
			item.AVSResultCode = AVSResultCode;
				
			item.CardCodeResponseCode = CardCodeResponseCode;
				
			item.Email = Email;
				
			item.FirstName = FirstName;
				
			item.LastName = LastName;
				
			item.NameOnCard = NameOnCard;
				
			item.Company = Company;
				
			item.BillingAddress = BillingAddress;
				
			item.City = City;
				
			item.State = State;
				
			item.Zip = Zip;
				
			item.Country = Country;
				
			item.Phone = Phone;
				
			item.IpAddress = IpAddress;
				
			item.ShipToFirstName = ShipToFirstName;
				
			item.ShipToLastName = ShipToLastName;
				
			item.ShipToCompany = ShipToCompany;
				
			item.ShipToAddress = ShipToAddress;
				
			item.ShipToCity = ShipToCity;
				
			item.ShipToState = ShipToState;
				
			item.ShipToZip = ShipToZip;
				
			item.ShipToCountry = ShipToCountry;
				
			item.DtStamp = DtStamp;
				
			item.ApplicationId = ApplicationId;
				
	        item.Save(UserName);
	    }
    }
}
