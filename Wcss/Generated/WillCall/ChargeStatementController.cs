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
    /// Controller class for Charge_Statement
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class ChargeStatementController
    {
        // Preload our schema..
        ChargeStatement thisSchemaLoad = new ChargeStatement();
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
        public ChargeStatementCollection FetchAll()
        {
            ChargeStatementCollection coll = new ChargeStatementCollection();
            Query qry = new Query(ChargeStatement.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public ChargeStatementCollection FetchByID(object Id)
        {
            ChargeStatementCollection coll = new ChargeStatementCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public ChargeStatementCollection FetchByQuery(Query qry)
        {
            ChargeStatementCollection coll = new ChargeStatementCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (ChargeStatement.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (ChargeStatement.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(DateTime DtStamp,Guid ApplicationId,Guid ChargeStatementId,int IMonth,int IYear,string MonthYear,int SalesQty,decimal SalesQtyPct,decimal? SalesQtyPortion,int RefundQty,decimal RefundQtyPct,decimal? RefundQtyPortion,decimal Gross,decimal GrossPct,decimal GrossThreshhold,decimal? GrossPortion,int TicketInvoiceQty,decimal TicketInvoicePct,int TicketUnitQty,decimal TicketUnitPct,decimal TicketSales,decimal TicketSalesPct,decimal? TicketPortion,int MerchInvoiceQty,decimal MerchInvoicePct,int MerchUnitQty,decimal MerchUnitPct,decimal MerchSales,decimal MerchSalesPct,decimal? MerchPortion,int ShipUnitQty,decimal ShipUnitPct,decimal ShipSales,decimal ShipSalesPct,decimal? ShipPortion,int SubscriptionsSent,decimal PerSubscription,int MailSent,decimal PerMailSent,decimal? MailerPortion,decimal HourlyPortion,decimal Discount,decimal? LineTotal,decimal AmountPaid,DateTime? DtPaid,string CheckNumber,string PayNotes)
	    {
		    ChargeStatement item = new ChargeStatement();
		    
            item.DtStamp = DtStamp;
            
            item.ApplicationId = ApplicationId;
            
            item.ChargeStatementId = ChargeStatementId;
            
            item.IMonth = IMonth;
            
            item.IYear = IYear;
            
            item.MonthYear = MonthYear;
            
            item.SalesQty = SalesQty;
            
            item.SalesQtyPct = SalesQtyPct;
            
            item.SalesQtyPortion = SalesQtyPortion;
            
            item.RefundQty = RefundQty;
            
            item.RefundQtyPct = RefundQtyPct;
            
            item.RefundQtyPortion = RefundQtyPortion;
            
            item.Gross = Gross;
            
            item.GrossPct = GrossPct;
            
            item.GrossThreshhold = GrossThreshhold;
            
            item.GrossPortion = GrossPortion;
            
            item.TicketInvoiceQty = TicketInvoiceQty;
            
            item.TicketInvoicePct = TicketInvoicePct;
            
            item.TicketUnitQty = TicketUnitQty;
            
            item.TicketUnitPct = TicketUnitPct;
            
            item.TicketSales = TicketSales;
            
            item.TicketSalesPct = TicketSalesPct;
            
            item.TicketPortion = TicketPortion;
            
            item.MerchInvoiceQty = MerchInvoiceQty;
            
            item.MerchInvoicePct = MerchInvoicePct;
            
            item.MerchUnitQty = MerchUnitQty;
            
            item.MerchUnitPct = MerchUnitPct;
            
            item.MerchSales = MerchSales;
            
            item.MerchSalesPct = MerchSalesPct;
            
            item.MerchPortion = MerchPortion;
            
            item.ShipUnitQty = ShipUnitQty;
            
            item.ShipUnitPct = ShipUnitPct;
            
            item.ShipSales = ShipSales;
            
            item.ShipSalesPct = ShipSalesPct;
            
            item.ShipPortion = ShipPortion;
            
            item.SubscriptionsSent = SubscriptionsSent;
            
            item.PerSubscription = PerSubscription;
            
            item.MailSent = MailSent;
            
            item.PerMailSent = PerMailSent;
            
            item.MailerPortion = MailerPortion;
            
            item.HourlyPortion = HourlyPortion;
            
            item.Discount = Discount;
            
            item.LineTotal = LineTotal;
            
            item.AmountPaid = AmountPaid;
            
            item.DtPaid = DtPaid;
            
            item.CheckNumber = CheckNumber;
            
            item.PayNotes = PayNotes;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,DateTime DtStamp,Guid ApplicationId,Guid ChargeStatementId,int IMonth,int IYear,string MonthYear,int SalesQty,decimal SalesQtyPct,decimal? SalesQtyPortion,int RefundQty,decimal RefundQtyPct,decimal? RefundQtyPortion,decimal Gross,decimal GrossPct,decimal GrossThreshhold,decimal? GrossPortion,int TicketInvoiceQty,decimal TicketInvoicePct,int TicketUnitQty,decimal TicketUnitPct,decimal TicketSales,decimal TicketSalesPct,decimal? TicketPortion,int MerchInvoiceQty,decimal MerchInvoicePct,int MerchUnitQty,decimal MerchUnitPct,decimal MerchSales,decimal MerchSalesPct,decimal? MerchPortion,int ShipUnitQty,decimal ShipUnitPct,decimal ShipSales,decimal ShipSalesPct,decimal? ShipPortion,int SubscriptionsSent,decimal PerSubscription,int MailSent,decimal PerMailSent,decimal? MailerPortion,decimal HourlyPortion,decimal Discount,decimal? LineTotal,decimal AmountPaid,DateTime? DtPaid,string CheckNumber,string PayNotes)
	    {
		    ChargeStatement item = new ChargeStatement();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.DtStamp = DtStamp;
				
			item.ApplicationId = ApplicationId;
				
			item.ChargeStatementId = ChargeStatementId;
				
			item.IMonth = IMonth;
				
			item.IYear = IYear;
				
			item.MonthYear = MonthYear;
				
			item.SalesQty = SalesQty;
				
			item.SalesQtyPct = SalesQtyPct;
				
			item.SalesQtyPortion = SalesQtyPortion;
				
			item.RefundQty = RefundQty;
				
			item.RefundQtyPct = RefundQtyPct;
				
			item.RefundQtyPortion = RefundQtyPortion;
				
			item.Gross = Gross;
				
			item.GrossPct = GrossPct;
				
			item.GrossThreshhold = GrossThreshhold;
				
			item.GrossPortion = GrossPortion;
				
			item.TicketInvoiceQty = TicketInvoiceQty;
				
			item.TicketInvoicePct = TicketInvoicePct;
				
			item.TicketUnitQty = TicketUnitQty;
				
			item.TicketUnitPct = TicketUnitPct;
				
			item.TicketSales = TicketSales;
				
			item.TicketSalesPct = TicketSalesPct;
				
			item.TicketPortion = TicketPortion;
				
			item.MerchInvoiceQty = MerchInvoiceQty;
				
			item.MerchInvoicePct = MerchInvoicePct;
				
			item.MerchUnitQty = MerchUnitQty;
				
			item.MerchUnitPct = MerchUnitPct;
				
			item.MerchSales = MerchSales;
				
			item.MerchSalesPct = MerchSalesPct;
				
			item.MerchPortion = MerchPortion;
				
			item.ShipUnitQty = ShipUnitQty;
				
			item.ShipUnitPct = ShipUnitPct;
				
			item.ShipSales = ShipSales;
				
			item.ShipSalesPct = ShipSalesPct;
				
			item.ShipPortion = ShipPortion;
				
			item.SubscriptionsSent = SubscriptionsSent;
				
			item.PerSubscription = PerSubscription;
				
			item.MailSent = MailSent;
				
			item.PerMailSent = PerMailSent;
				
			item.MailerPortion = MailerPortion;
				
			item.HourlyPortion = HourlyPortion;
				
			item.Discount = Discount;
				
			item.LineTotal = LineTotal;
				
			item.AmountPaid = AmountPaid;
				
			item.DtPaid = DtPaid;
				
			item.CheckNumber = CheckNumber;
				
			item.PayNotes = PayNotes;
				
	        item.Save(UserName);
	    }
    }
}
