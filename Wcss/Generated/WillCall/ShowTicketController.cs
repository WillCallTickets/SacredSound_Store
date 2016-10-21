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
    /// Controller class for ShowTicket
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class ShowTicketController
    {
        // Preload our schema..
        ShowTicket thisSchemaLoad = new ShowTicket();
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
        public ShowTicketCollection FetchAll()
        {
            ShowTicketCollection coll = new ShowTicketCollection();
            Query qry = new Query(ShowTicket.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public ShowTicketCollection FetchByID(object Id)
        {
            ShowTicketCollection coll = new ShowTicketCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public ShowTicketCollection FetchByQuery(Query qry)
        {
            ShowTicketCollection coll = new ShowTicketCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (ShowTicket.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (ShowTicket.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(int TVendorId,DateTime DtDateOfShow,string CriteriaText,string SalesDescription,int TShowDateId,int TShowId,int TAgeId,bool BActive,bool BSoldOut,string Status,bool BDosTicket,int IDisplayOrder,string PriceText,decimal? MPrice,string DosText,decimal? MDosPrice,decimal? MServiceCharge,bool BAllowShipping,bool BAllowWillCall,bool BHideShipMethod,DateTime DtShipCutoff,bool BOverrideSellout,bool BUnlockActive,string UnlockCode,DateTime? DtUnlockDate,DateTime? DtUnlockEndDate,DateTime? DtPublicOnsale,DateTime? DtEndDate,int? IMaxQtyPerOrder,int IAllotment,int IPending,int ISold,int? IAvailable,int IRefunded,decimal? MFlatShip,string VcFlatMethod,DateTime? DtBackorder,bool? BShipSeparate,DateTime DtStamp)
	    {
		    ShowTicket item = new ShowTicket();
		    
            item.TVendorId = TVendorId;
            
            item.DtDateOfShow = DtDateOfShow;
            
            item.CriteriaText = CriteriaText;
            
            item.SalesDescription = SalesDescription;
            
            item.TShowDateId = TShowDateId;
            
            item.TShowId = TShowId;
            
            item.TAgeId = TAgeId;
            
            item.BActive = BActive;
            
            item.BSoldOut = BSoldOut;
            
            item.Status = Status;
            
            item.BDosTicket = BDosTicket;
            
            item.IDisplayOrder = IDisplayOrder;
            
            item.PriceText = PriceText;
            
            item.MPrice = MPrice;
            
            item.DosText = DosText;
            
            item.MDosPrice = MDosPrice;
            
            item.MServiceCharge = MServiceCharge;
            
            item.BAllowShipping = BAllowShipping;
            
            item.BAllowWillCall = BAllowWillCall;
            
            item.BHideShipMethod = BHideShipMethod;
            
            item.DtShipCutoff = DtShipCutoff;
            
            item.BOverrideSellout = BOverrideSellout;
            
            item.BUnlockActive = BUnlockActive;
            
            item.UnlockCode = UnlockCode;
            
            item.DtUnlockDate = DtUnlockDate;
            
            item.DtUnlockEndDate = DtUnlockEndDate;
            
            item.DtPublicOnsale = DtPublicOnsale;
            
            item.DtEndDate = DtEndDate;
            
            item.IMaxQtyPerOrder = IMaxQtyPerOrder;
            
            item.IAllotment = IAllotment;
            
            item.IPending = IPending;
            
            item.ISold = ISold;
            
            item.IAvailable = IAvailable;
            
            item.IRefunded = IRefunded;
            
            item.MFlatShip = MFlatShip;
            
            item.VcFlatMethod = VcFlatMethod;
            
            item.DtBackorder = DtBackorder;
            
            item.BShipSeparate = BShipSeparate;
            
            item.DtStamp = DtStamp;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,int TVendorId,DateTime DtDateOfShow,string CriteriaText,string SalesDescription,int TShowDateId,int TShowId,int TAgeId,bool BActive,bool BSoldOut,string Status,bool BDosTicket,int IDisplayOrder,string PriceText,decimal? MPrice,string DosText,decimal? MDosPrice,decimal? MServiceCharge,bool BAllowShipping,bool BAllowWillCall,bool BHideShipMethod,DateTime DtShipCutoff,bool BOverrideSellout,bool BUnlockActive,string UnlockCode,DateTime? DtUnlockDate,DateTime? DtUnlockEndDate,DateTime? DtPublicOnsale,DateTime? DtEndDate,int? IMaxQtyPerOrder,int IAllotment,int IPending,int ISold,int? IAvailable,int IRefunded,decimal? MFlatShip,string VcFlatMethod,DateTime? DtBackorder,bool? BShipSeparate,DateTime DtStamp)
	    {
		    ShowTicket item = new ShowTicket();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.TVendorId = TVendorId;
				
			item.DtDateOfShow = DtDateOfShow;
				
			item.CriteriaText = CriteriaText;
				
			item.SalesDescription = SalesDescription;
				
			item.TShowDateId = TShowDateId;
				
			item.TShowId = TShowId;
				
			item.TAgeId = TAgeId;
				
			item.BActive = BActive;
				
			item.BSoldOut = BSoldOut;
				
			item.Status = Status;
				
			item.BDosTicket = BDosTicket;
				
			item.IDisplayOrder = IDisplayOrder;
				
			item.PriceText = PriceText;
				
			item.MPrice = MPrice;
				
			item.DosText = DosText;
				
			item.MDosPrice = MDosPrice;
				
			item.MServiceCharge = MServiceCharge;
				
			item.BAllowShipping = BAllowShipping;
				
			item.BAllowWillCall = BAllowWillCall;
				
			item.BHideShipMethod = BHideShipMethod;
				
			item.DtShipCutoff = DtShipCutoff;
				
			item.BOverrideSellout = BOverrideSellout;
				
			item.BUnlockActive = BUnlockActive;
				
			item.UnlockCode = UnlockCode;
				
			item.DtUnlockDate = DtUnlockDate;
				
			item.DtUnlockEndDate = DtUnlockEndDate;
				
			item.DtPublicOnsale = DtPublicOnsale;
				
			item.DtEndDate = DtEndDate;
				
			item.IMaxQtyPerOrder = IMaxQtyPerOrder;
				
			item.IAllotment = IAllotment;
				
			item.IPending = IPending;
				
			item.ISold = ISold;
				
			item.IAvailable = IAvailable;
				
			item.IRefunded = IRefunded;
				
			item.MFlatShip = MFlatShip;
				
			item.VcFlatMethod = VcFlatMethod;
				
			item.DtBackorder = DtBackorder;
				
			item.BShipSeparate = BShipSeparate;
				
			item.DtStamp = DtStamp;
				
	        item.Save(UserName);
	    }
    }
}
