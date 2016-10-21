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
    /// Controller class for InvoiceShipment
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class InvoiceShipmentController
    {
        // Preload our schema..
        InvoiceShipment thisSchemaLoad = new InvoiceShipment();
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
        public InvoiceShipmentCollection FetchAll()
        {
            InvoiceShipmentCollection coll = new InvoiceShipmentCollection();
            Query qry = new Query(InvoiceShipment.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public InvoiceShipmentCollection FetchByID(object Id)
        {
            InvoiceShipmentCollection coll = new InvoiceShipmentCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public InvoiceShipmentCollection FetchByQuery(Query qry)
        {
            InvoiceShipmentCollection coll = new InvoiceShipmentCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (InvoiceShipment.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (InvoiceShipment.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(int TInvoiceId,Guid? UserId,DateTime DtCreated,Guid ReferenceNumber,string VcContext,int? TShipItemId,bool BLabelPrinted,string CompanyName,string FirstName,string LastName,string Address1,string Address2,string City,string StateProvince,string PostalCode,string Country,string Phone,string ShipMessage,DateTime? DtShipped,bool? BRTS,string Status,string VcCarrier,string ShipMethod,string TrackingInformation,string PackingList,string PackingAdditional,decimal MWeightCalculated,decimal MWeightActual,decimal MHandlingCalculated,decimal MShippingCharged,decimal MShippingActual,DateTime DtStamp)
	    {
		    InvoiceShipment item = new InvoiceShipment();
		    
            item.TInvoiceId = TInvoiceId;
            
            item.UserId = UserId;
            
            item.DtCreated = DtCreated;
            
            item.ReferenceNumber = ReferenceNumber;
            
            item.VcContext = VcContext;
            
            item.TShipItemId = TShipItemId;
            
            item.BLabelPrinted = BLabelPrinted;
            
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
            
            item.BRTS = BRTS;
            
            item.Status = Status;
            
            item.VcCarrier = VcCarrier;
            
            item.ShipMethod = ShipMethod;
            
            item.TrackingInformation = TrackingInformation;
            
            item.PackingList = PackingList;
            
            item.PackingAdditional = PackingAdditional;
            
            item.MWeightCalculated = MWeightCalculated;
            
            item.MWeightActual = MWeightActual;
            
            item.MHandlingCalculated = MHandlingCalculated;
            
            item.MShippingCharged = MShippingCharged;
            
            item.MShippingActual = MShippingActual;
            
            item.DtStamp = DtStamp;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,int TInvoiceId,Guid? UserId,DateTime DtCreated,Guid ReferenceNumber,string VcContext,int? TShipItemId,bool BLabelPrinted,string CompanyName,string FirstName,string LastName,string Address1,string Address2,string City,string StateProvince,string PostalCode,string Country,string Phone,string ShipMessage,DateTime? DtShipped,bool? BRTS,string Status,string VcCarrier,string ShipMethod,string TrackingInformation,string PackingList,string PackingAdditional,decimal MWeightCalculated,decimal MWeightActual,decimal MHandlingCalculated,decimal MShippingCharged,decimal MShippingActual,DateTime DtStamp)
	    {
		    InvoiceShipment item = new InvoiceShipment();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.TInvoiceId = TInvoiceId;
				
			item.UserId = UserId;
				
			item.DtCreated = DtCreated;
				
			item.ReferenceNumber = ReferenceNumber;
				
			item.VcContext = VcContext;
				
			item.TShipItemId = TShipItemId;
				
			item.BLabelPrinted = BLabelPrinted;
				
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
				
			item.BRTS = BRTS;
				
			item.Status = Status;
				
			item.VcCarrier = VcCarrier;
				
			item.ShipMethod = ShipMethod;
				
			item.TrackingInformation = TrackingInformation;
				
			item.PackingList = PackingList;
				
			item.PackingAdditional = PackingAdditional;
				
			item.MWeightCalculated = MWeightCalculated;
				
			item.MWeightActual = MWeightActual;
				
			item.MHandlingCalculated = MHandlingCalculated;
				
			item.MShippingCharged = MShippingCharged;
				
			item.MShippingActual = MShippingActual;
				
			item.DtStamp = DtStamp;
				
	        item.Save(UserName);
	    }
    }
}
