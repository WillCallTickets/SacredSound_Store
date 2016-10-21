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
	#region Tables Struct
	public partial struct Tables
	{
		
		public static string Act = @"Act";
        
		public static string ActivationWindow = @"ActivationWindow";
        
		public static string Age = @"Age";
        
		public static string AspnetApplication = @"aspnet_Applications";
        
		public static string AspnetMembership = @"aspnet_Membership";
        
		public static string AspnetPath = @"aspnet_Paths";
        
		public static string AspnetPersonalizationAllUser = @"aspnet_PersonalizationAllUsers";
        
		public static string AspnetPersonalizationPerUser = @"aspnet_PersonalizationPerUser";
        
		public static string AspnetProfile = @"aspnet_Profile";
        
		public static string AspnetRole = @"aspnet_Roles";
        
		public static string AspnetSchemaVersion = @"aspnet_SchemaVersions";
        
		public static string AspnetUser = @"aspnet_Users";
        
		public static string AspnetUsersOld = @"aspnet_Users_Old";
        
		public static string AspnetUsersInRole = @"aspnet_UsersInRoles";
        
		public static string AspnetWebEventEvent = @"aspnet_WebEvent_Events";
        
		public static string AuthorizeNet = @"AuthorizeNet";
        
		public static string Cashew = @"Cashew";
        
		public static string ChargeHourly = @"Charge_Hourly";
        
		public static string ChargeStatement = @"Charge_Statement";
        
		public static string CharitableContribution = @"CharitableContribution";
        
		public static string CharitableListing = @"CharitableListing";
        
		public static string CharitableOrg = @"CharitableOrg";
        
		public static string Download = @"Download";
        
		public static string EmailLetter = @"EmailLetter";
        
		public static string EmailParam = @"EmailParam";
        
		public static string EmailParamArchive = @"EmailParamArchive";
        
		public static string EntityValue = @"EntityValue";
        
		public static string EventQ = @"EventQ";
        
		public static string EventQArchive = @"EventQArchive";
        
		public static string FaqCategorie = @"FaqCategorie";
        
		public static string FaqItem = @"FaqItem";
        
		public static string FbStat = @"FB_Stat";
        
		public static string FraudScreen = @"FraudScreen";
        
		public static string HeaderImage = @"HeaderImage";
        
		public static string HintQuestion = @"HintQuestion";
        
		public static string HistoryInventory = @"HistoryInventory";
        
		public static string HistoryPricing = @"HistoryPricing";
        
		public static string HistorySubscriptionEmail = @"HistorySubscriptionEmail";
        
		public static string Inventory = @"Inventory";
        
		public static string Invoice = @"Invoice";
        
		public static string InvoiceBillShip = @"InvoiceBillShip";
        
		public static string InvoiceEvent = @"InvoiceEvent";
        
		public static string InvoiceFee = @"InvoiceFee";
        
		public static string InvoiceItem = @"InvoiceItem";
        
		public static string InvoiceItemPostPurchaseText = @"InvoiceItemPostPurchaseText";
        
		public static string InvoiceShipment = @"InvoiceShipment";
        
		public static string InvoiceShipmentItem = @"InvoiceShipmentItem";
        
		public static string InvoiceTransaction = @"InvoiceTransaction";
        
		public static string ItemImage = @"ItemImage";
        
		public static string JShowAct = @"JShowAct";
        
		public static string JShowPromoter = @"JShowPromoter";
        
		public static string Lottery = @"Lottery";
        
		public static string LotteryRequest = @"LotteryRequest";
        
		public static string Mailer = @"Mailer";
        
		public static string MailerContent = @"MailerContent";
        
		public static string MailerTemplate = @"MailerTemplate";
        
		public static string MailerTemplateContent = @"MailerTemplateContent";
        
		public static string MailerTemplateSubstitution = @"MailerTemplateSubstitution";
        
		public static string MailQueue = @"MailQueue";
        
		public static string MailQueueArchive = @"MailQueueArchive";
        
		public static string Merch = @"Merch";
        
		public static string MerchBundle = @"MerchBundle";
        
		public static string MerchBundleItem = @"MerchBundleItem";
        
		public static string MerchCategorie = @"MerchCategorie";
        
		public static string MerchColor = @"MerchColor";
        
		public static string MerchDivision = @"MerchDivision";
        
		public static string MerchDownload = @"MerchDownload";
        
		public static string MerchJoinCat = @"MerchJoinCat";
        
		public static string MerchSize = @"MerchSize";
        
		public static string MerchStock = @"MerchStock";
        
		public static string MerchStockRemoved = @"MerchStock_Removed";
        
		public static string PendingOperation = @"PendingOperation";
        
		public static string PostPurchaseText = @"PostPurchaseText";
        
		public static string ProductAccess = @"ProductAccess";
        
		public static string ProductAccessProduct = @"ProductAccessProduct";
        
		public static string ProductAccessUser = @"ProductAccessUser";
        
		public static string Promoter = @"Promoter";
        
		public static string ReportDailySale = @"Report_DailySales";
        
		public static string Required = @"Required";
        
		public static string RequiredInvoiceFee = @"Required_InvoiceFee";
        
		public static string RequiredMerch = @"Required_Merch";
        
		public static string RequiredShowTicketPastPurchase = @"Required_ShowTicket_PastPurchase";
        
		public static string SalePromotion = @"SalePromotion";
        
		public static string SalePromotionAward = @"SalePromotionAward";
        
		public static string SaleRule = @"SaleRule";
        
		public static string Search = @"Search";
        
		public static string ServiceCharge = @"ServiceCharge";
        
		public static string ShipmentBatch = @"ShipmentBatch";
        
		public static string ShipmentBatchInvoiceShipment = @"ShipmentBatch_InvoiceShipment";
        
		public static string Show = @"Show";
        
		public static string ShowDate = @"ShowDate";
        
		public static string ShowEvent = @"ShowEvent";
        
		public static string ShowLink = @"ShowLink";
        
		public static string ShowStatus = @"ShowStatus";
        
		public static string ShowTicket = @"ShowTicket";
        
		public static string ShowTicketDosTicket = @"ShowTicketDosTicket";
        
		public static string ShowTicketPackageLink = @"ShowTicketPackageLink";
        
		public static string SiteConfig = @"SiteConfig";
        
		public static string StoreCredit = @"StoreCredit";
        
		public static string Subscription = @"Subscription";
        
		public static string SubscriptionEmail = @"SubscriptionEmail";
        
		public static string SubscriptionUser = @"SubscriptionUser";
        
		public static string TicketStock = @"TicketStock";
        
		public static string TicketStockRemoved = @"TicketStock_Removed";
        
		public static string UserPreviousEmail = @"User_PreviousEmail";
        
		public static string UserCouponRedemption = @"UserCouponRedemption";
        
		public static string UserEvent = @"UserEvent";
        
		public static string Vendor = @"Vendor";
        
		public static string Venue = @"Venue";
        
	}
	#endregion
    #region Schemas
    public partial class Schemas {
		
		public static TableSchema.Table Act{
            get { return DataService.GetSchema("Act","WillCall"); }
		}
        
		public static TableSchema.Table ActivationWindow{
            get { return DataService.GetSchema("ActivationWindow","WillCall"); }
		}
        
		public static TableSchema.Table Age{
            get { return DataService.GetSchema("Age","WillCall"); }
		}
        
		public static TableSchema.Table AspnetApplication{
            get { return DataService.GetSchema("aspnet_Applications","WillCall"); }
		}
        
		public static TableSchema.Table AspnetMembership{
            get { return DataService.GetSchema("aspnet_Membership","WillCall"); }
		}
        
		public static TableSchema.Table AspnetPath{
            get { return DataService.GetSchema("aspnet_Paths","WillCall"); }
		}
        
		public static TableSchema.Table AspnetPersonalizationAllUser{
            get { return DataService.GetSchema("aspnet_PersonalizationAllUsers","WillCall"); }
		}
        
		public static TableSchema.Table AspnetPersonalizationPerUser{
            get { return DataService.GetSchema("aspnet_PersonalizationPerUser","WillCall"); }
		}
        
		public static TableSchema.Table AspnetProfile{
            get { return DataService.GetSchema("aspnet_Profile","WillCall"); }
		}
        
		public static TableSchema.Table AspnetRole{
            get { return DataService.GetSchema("aspnet_Roles","WillCall"); }
		}
        
		public static TableSchema.Table AspnetSchemaVersion{
            get { return DataService.GetSchema("aspnet_SchemaVersions","WillCall"); }
		}
        
		public static TableSchema.Table AspnetUser{
            get { return DataService.GetSchema("aspnet_Users","WillCall"); }
		}
        
		public static TableSchema.Table AspnetUsersOld{
            get { return DataService.GetSchema("aspnet_Users_Old","WillCall"); }
		}
        
		public static TableSchema.Table AspnetUsersInRole{
            get { return DataService.GetSchema("aspnet_UsersInRoles","WillCall"); }
		}
        
		public static TableSchema.Table AspnetWebEventEvent{
            get { return DataService.GetSchema("aspnet_WebEvent_Events","WillCall"); }
		}
        
		public static TableSchema.Table AuthorizeNet{
            get { return DataService.GetSchema("AuthorizeNet","WillCall"); }
		}
        
		public static TableSchema.Table Cashew{
            get { return DataService.GetSchema("Cashew","WillCall"); }
		}
        
		public static TableSchema.Table ChargeHourly{
            get { return DataService.GetSchema("Charge_Hourly","WillCall"); }
		}
        
		public static TableSchema.Table ChargeStatement{
            get { return DataService.GetSchema("Charge_Statement","WillCall"); }
		}
        
		public static TableSchema.Table CharitableContribution{
            get { return DataService.GetSchema("CharitableContribution","WillCall"); }
		}
        
		public static TableSchema.Table CharitableListing{
            get { return DataService.GetSchema("CharitableListing","WillCall"); }
		}
        
		public static TableSchema.Table CharitableOrg{
            get { return DataService.GetSchema("CharitableOrg","WillCall"); }
		}
        
		public static TableSchema.Table Download{
            get { return DataService.GetSchema("Download","WillCall"); }
		}
        
		public static TableSchema.Table EmailLetter{
            get { return DataService.GetSchema("EmailLetter","WillCall"); }
		}
        
		public static TableSchema.Table EmailParam{
            get { return DataService.GetSchema("EmailParam","WillCall"); }
		}
        
		public static TableSchema.Table EmailParamArchive{
            get { return DataService.GetSchema("EmailParamArchive","WillCall"); }
		}
        
		public static TableSchema.Table EntityValue{
            get { return DataService.GetSchema("EntityValue","WillCall"); }
		}
        
		public static TableSchema.Table EventQ{
            get { return DataService.GetSchema("EventQ","WillCall"); }
		}
        
		public static TableSchema.Table EventQArchive{
            get { return DataService.GetSchema("EventQArchive","WillCall"); }
		}
        
		public static TableSchema.Table FaqCategorie{
            get { return DataService.GetSchema("FaqCategorie","WillCall"); }
		}
        
		public static TableSchema.Table FaqItem{
            get { return DataService.GetSchema("FaqItem","WillCall"); }
		}
        
		public static TableSchema.Table FbStat{
            get { return DataService.GetSchema("FB_Stat","WillCall"); }
		}
        
		public static TableSchema.Table FraudScreen{
            get { return DataService.GetSchema("FraudScreen","WillCall"); }
		}
        
		public static TableSchema.Table HeaderImage{
            get { return DataService.GetSchema("HeaderImage","WillCall"); }
		}
        
		public static TableSchema.Table HintQuestion{
            get { return DataService.GetSchema("HintQuestion","WillCall"); }
		}
        
		public static TableSchema.Table HistoryInventory{
            get { return DataService.GetSchema("HistoryInventory","WillCall"); }
		}
        
		public static TableSchema.Table HistoryPricing{
            get { return DataService.GetSchema("HistoryPricing","WillCall"); }
		}
        
		public static TableSchema.Table HistorySubscriptionEmail{
            get { return DataService.GetSchema("HistorySubscriptionEmail","WillCall"); }
		}
        
		public static TableSchema.Table Inventory{
            get { return DataService.GetSchema("Inventory","WillCall"); }
		}
        
		public static TableSchema.Table Invoice{
            get { return DataService.GetSchema("Invoice","WillCall"); }
		}
        
		public static TableSchema.Table InvoiceBillShip{
            get { return DataService.GetSchema("InvoiceBillShip","WillCall"); }
		}
        
		public static TableSchema.Table InvoiceEvent{
            get { return DataService.GetSchema("InvoiceEvent","WillCall"); }
		}
        
		public static TableSchema.Table InvoiceFee{
            get { return DataService.GetSchema("InvoiceFee","WillCall"); }
		}
        
		public static TableSchema.Table InvoiceItem{
            get { return DataService.GetSchema("InvoiceItem","WillCall"); }
		}
        
		public static TableSchema.Table InvoiceItemPostPurchaseText{
            get { return DataService.GetSchema("InvoiceItemPostPurchaseText","WillCall"); }
		}
        
		public static TableSchema.Table InvoiceShipment{
            get { return DataService.GetSchema("InvoiceShipment","WillCall"); }
		}
        
		public static TableSchema.Table InvoiceShipmentItem{
            get { return DataService.GetSchema("InvoiceShipmentItem","WillCall"); }
		}
        
		public static TableSchema.Table InvoiceTransaction{
            get { return DataService.GetSchema("InvoiceTransaction","WillCall"); }
		}
        
		public static TableSchema.Table ItemImage{
            get { return DataService.GetSchema("ItemImage","WillCall"); }
		}
        
		public static TableSchema.Table JShowAct{
            get { return DataService.GetSchema("JShowAct","WillCall"); }
		}
        
		public static TableSchema.Table JShowPromoter{
            get { return DataService.GetSchema("JShowPromoter","WillCall"); }
		}
        
		public static TableSchema.Table Lottery{
            get { return DataService.GetSchema("Lottery","WillCall"); }
		}
        
		public static TableSchema.Table LotteryRequest{
            get { return DataService.GetSchema("LotteryRequest","WillCall"); }
		}
        
		public static TableSchema.Table Mailer{
            get { return DataService.GetSchema("Mailer","WillCall"); }
		}
        
		public static TableSchema.Table MailerContent{
            get { return DataService.GetSchema("MailerContent","WillCall"); }
		}
        
		public static TableSchema.Table MailerTemplate{
            get { return DataService.GetSchema("MailerTemplate","WillCall"); }
		}
        
		public static TableSchema.Table MailerTemplateContent{
            get { return DataService.GetSchema("MailerTemplateContent","WillCall"); }
		}
        
		public static TableSchema.Table MailerTemplateSubstitution{
            get { return DataService.GetSchema("MailerTemplateSubstitution","WillCall"); }
		}
        
		public static TableSchema.Table MailQueue{
            get { return DataService.GetSchema("MailQueue","WillCall"); }
		}
        
		public static TableSchema.Table MailQueueArchive{
            get { return DataService.GetSchema("MailQueueArchive","WillCall"); }
		}
        
		public static TableSchema.Table Merch{
            get { return DataService.GetSchema("Merch","WillCall"); }
		}
        
		public static TableSchema.Table MerchBundle{
            get { return DataService.GetSchema("MerchBundle","WillCall"); }
		}
        
		public static TableSchema.Table MerchBundleItem{
            get { return DataService.GetSchema("MerchBundleItem","WillCall"); }
		}
        
		public static TableSchema.Table MerchCategorie{
            get { return DataService.GetSchema("MerchCategorie","WillCall"); }
		}
        
		public static TableSchema.Table MerchColor{
            get { return DataService.GetSchema("MerchColor","WillCall"); }
		}
        
		public static TableSchema.Table MerchDivision{
            get { return DataService.GetSchema("MerchDivision","WillCall"); }
		}
        
		public static TableSchema.Table MerchDownload{
            get { return DataService.GetSchema("MerchDownload","WillCall"); }
		}
        
		public static TableSchema.Table MerchJoinCat{
            get { return DataService.GetSchema("MerchJoinCat","WillCall"); }
		}
        
		public static TableSchema.Table MerchSize{
            get { return DataService.GetSchema("MerchSize","WillCall"); }
		}
        
		public static TableSchema.Table MerchStock{
            get { return DataService.GetSchema("MerchStock","WillCall"); }
		}
        
		public static TableSchema.Table MerchStockRemoved{
            get { return DataService.GetSchema("MerchStock_Removed","WillCall"); }
		}
        
		public static TableSchema.Table PendingOperation{
            get { return DataService.GetSchema("PendingOperation","WillCall"); }
		}
        
		public static TableSchema.Table PostPurchaseText{
            get { return DataService.GetSchema("PostPurchaseText","WillCall"); }
		}
        
		public static TableSchema.Table ProductAccess{
            get { return DataService.GetSchema("ProductAccess","WillCall"); }
		}
        
		public static TableSchema.Table ProductAccessProduct{
            get { return DataService.GetSchema("ProductAccessProduct","WillCall"); }
		}
        
		public static TableSchema.Table ProductAccessUser{
            get { return DataService.GetSchema("ProductAccessUser","WillCall"); }
		}
        
		public static TableSchema.Table Promoter{
            get { return DataService.GetSchema("Promoter","WillCall"); }
		}
        
		public static TableSchema.Table ReportDailySale{
            get { return DataService.GetSchema("Report_DailySales","WillCall"); }
		}
        
		public static TableSchema.Table Required{
            get { return DataService.GetSchema("Required","WillCall"); }
		}
        
		public static TableSchema.Table RequiredInvoiceFee{
            get { return DataService.GetSchema("Required_InvoiceFee","WillCall"); }
		}
        
		public static TableSchema.Table RequiredMerch{
            get { return DataService.GetSchema("Required_Merch","WillCall"); }
		}
        
		public static TableSchema.Table RequiredShowTicketPastPurchase{
            get { return DataService.GetSchema("Required_ShowTicket_PastPurchase","WillCall"); }
		}
        
		public static TableSchema.Table SalePromotion{
            get { return DataService.GetSchema("SalePromotion","WillCall"); }
		}
        
		public static TableSchema.Table SalePromotionAward{
            get { return DataService.GetSchema("SalePromotionAward","WillCall"); }
		}
        
		public static TableSchema.Table SaleRule{
            get { return DataService.GetSchema("SaleRule","WillCall"); }
		}
        
		public static TableSchema.Table Search{
            get { return DataService.GetSchema("Search","WillCall"); }
		}
        
		public static TableSchema.Table ServiceCharge{
            get { return DataService.GetSchema("ServiceCharge","WillCall"); }
		}
        
		public static TableSchema.Table ShipmentBatch{
            get { return DataService.GetSchema("ShipmentBatch","WillCall"); }
		}
        
		public static TableSchema.Table ShipmentBatchInvoiceShipment{
            get { return DataService.GetSchema("ShipmentBatch_InvoiceShipment","WillCall"); }
		}
        
		public static TableSchema.Table Show{
            get { return DataService.GetSchema("Show","WillCall"); }
		}
        
		public static TableSchema.Table ShowDate{
            get { return DataService.GetSchema("ShowDate","WillCall"); }
		}
        
		public static TableSchema.Table ShowEvent{
            get { return DataService.GetSchema("ShowEvent","WillCall"); }
		}
        
		public static TableSchema.Table ShowLink{
            get { return DataService.GetSchema("ShowLink","WillCall"); }
		}
        
		public static TableSchema.Table ShowStatus{
            get { return DataService.GetSchema("ShowStatus","WillCall"); }
		}
        
		public static TableSchema.Table ShowTicket{
            get { return DataService.GetSchema("ShowTicket","WillCall"); }
		}
        
		public static TableSchema.Table ShowTicketDosTicket{
            get { return DataService.GetSchema("ShowTicketDosTicket","WillCall"); }
		}
        
		public static TableSchema.Table ShowTicketPackageLink{
            get { return DataService.GetSchema("ShowTicketPackageLink","WillCall"); }
		}
        
		public static TableSchema.Table SiteConfig{
            get { return DataService.GetSchema("SiteConfig","WillCall"); }
		}
        
		public static TableSchema.Table StoreCredit{
            get { return DataService.GetSchema("StoreCredit","WillCall"); }
		}
        
		public static TableSchema.Table Subscription{
            get { return DataService.GetSchema("Subscription","WillCall"); }
		}
        
		public static TableSchema.Table SubscriptionEmail{
            get { return DataService.GetSchema("SubscriptionEmail","WillCall"); }
		}
        
		public static TableSchema.Table SubscriptionUser{
            get { return DataService.GetSchema("SubscriptionUser","WillCall"); }
		}
        
		public static TableSchema.Table TicketStock{
            get { return DataService.GetSchema("TicketStock","WillCall"); }
		}
        
		public static TableSchema.Table TicketStockRemoved{
            get { return DataService.GetSchema("TicketStock_Removed","WillCall"); }
		}
        
		public static TableSchema.Table UserPreviousEmail{
            get { return DataService.GetSchema("User_PreviousEmail","WillCall"); }
		}
        
		public static TableSchema.Table UserCouponRedemption{
            get { return DataService.GetSchema("UserCouponRedemption","WillCall"); }
		}
        
		public static TableSchema.Table UserEvent{
            get { return DataService.GetSchema("UserEvent","WillCall"); }
		}
        
		public static TableSchema.Table Vendor{
            get { return DataService.GetSchema("Vendor","WillCall"); }
		}
        
		public static TableSchema.Table Venue{
            get { return DataService.GetSchema("Venue","WillCall"); }
		}
        
	
    }
    #endregion
    #region View Struct
    public partial struct Views 
    {
		
		public static string VwAspnetApplication = @"vw_aspnet_Applications";
        
		public static string VwAspnetMembershipUser = @"vw_aspnet_MembershipUsers";
        
		public static string VwAspnetProfile = @"vw_aspnet_Profiles";
        
		public static string VwAspnetRole = @"vw_aspnet_Roles";
        
		public static string VwAspnetUser = @"vw_aspnet_Users";
        
		public static string VwAspnetUsersInRole = @"vw_aspnet_UsersInRoles";
        
		public static string VwAspnetWebPartStatePath = @"vw_aspnet_WebPartState_Paths";
        
		public static string VwAspnetWebPartStateShared = @"vw_aspnet_WebPartState_Shared";
        
		public static string VwAspnetWebPartStateUser = @"vw_aspnet_WebPartState_User";
        
		public static string VwShowTicketWithPending = @"vw_ShowTicketWithPending";
        
    }
    #endregion
    
    #region Query Factories
	public static partial class DB
	{
        public static DataProvider _provider = DataService.Providers["WillCall"];
        static ISubSonicRepository _repository;
        public static ISubSonicRepository Repository {
            get {
                if (_repository == null)
                    return new SubSonicRepository(_provider);
                return _repository; 
            }
            set { _repository = value; }
        }
	
        public static Select SelectAllColumnsFrom<T>() where T : RecordBase<T>, new()
	    {
            return Repository.SelectAllColumnsFrom<T>();
            
	    }
	    public static Select Select()
	    {
            return Repository.Select();
	    }
	    
		public static Select Select(params string[] columns)
		{
            return Repository.Select(columns);
        }
	    
		public static Select Select(params Aggregate[] aggregates)
		{
            return Repository.Select(aggregates);
        }
   
	    public static Update Update<T>() where T : RecordBase<T>, new()
	    {
            return Repository.Update<T>();
	    }
     
	    
	    public static Insert Insert()
	    {
            return Repository.Insert();
	    }
	    
	    public static Delete Delete()
	    {
            
            return Repository.Delete();
	    }
	    
	    public static InlineQuery Query()
	    {
            
            return Repository.Query();
	    }
	    	    
	    
	}
    #endregion
    
}
namespace Erlg
{
	#region Tables Struct
	public partial struct Tables
	{
		
		public static string Log = @"Log";
        
		public static string LogArchive = @"LogArchive";
        
	}
	#endregion
    #region Schemas
    public partial class Schemas {
		
		public static TableSchema.Table Log{
            get { return DataService.GetSchema("Log","ErrorLog"); }
		}
        
		public static TableSchema.Table LogArchive{
            get { return DataService.GetSchema("LogArchive","ErrorLog"); }
		}
        
	
    }
    #endregion
    #region View Struct
    public partial struct Views 
    {
		
    }
    #endregion
    
    #region Query Factories
	public static partial class DB
	{
        public static DataProvider _provider = DataService.Providers["ErrorLog"];
        static ISubSonicRepository _repository;
        public static ISubSonicRepository Repository {
            get {
                if (_repository == null)
                    return new SubSonicRepository(_provider);
                return _repository; 
            }
            set { _repository = value; }
        }
	
        public static Select SelectAllColumnsFrom<T>() where T : RecordBase<T>, new()
	    {
            return Repository.SelectAllColumnsFrom<T>();
            
	    }
	    public static Select Select()
	    {
            return Repository.Select();
	    }
	    
		public static Select Select(params string[] columns)
		{
            return Repository.Select(columns);
        }
	    
		public static Select Select(params Aggregate[] aggregates)
		{
            return Repository.Select(aggregates);
        }
   
	    public static Update Update<T>() where T : RecordBase<T>, new()
	    {
            return Repository.Update<T>();
	    }
     
	    
	    public static Insert Insert()
	    {
            return Repository.Insert();
	    }
	    
	    public static Delete Delete()
	    {
            
            return Repository.Delete();
	    }
	    
	    public static InlineQuery Query()
	    {
            
            return Repository.Query();
	    }
	    	    
	    
	}
    #endregion
    
}
#region Databases
public partial struct Databases 
{
	
	public static string WillCall = @"WillCall";
    
	public static string ErrorLog = @"ErrorLog";
    
}
#endregion