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
    /// Controller class for InvoiceTransaction
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class InvoiceTransactionController
    {
        // Preload our schema..
        InvoiceTransaction thisSchemaLoad = new InvoiceTransaction();
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
        public InvoiceTransactionCollection FetchAll()
        {
            InvoiceTransactionCollection coll = new InvoiceTransactionCollection();
            Query qry = new Query(InvoiceTransaction.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public InvoiceTransactionCollection FetchByID(object Id)
        {
            InvoiceTransactionCollection coll = new InvoiceTransactionCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public InvoiceTransactionCollection FetchByQuery(Query qry)
        {
            InvoiceTransactionCollection coll = new InvoiceTransactionCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (InvoiceTransaction.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (InvoiceTransaction.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string ProcessorId,int TInvoiceId,string PerformedBy,string Admin,Guid UserId,int? CustomerId,int? TInvoiceItemId,string TransType,string FundsType,string FundsProcessor,string BatchId,decimal MAmount,string NameOnCard,string LastFourDigits,string UserIp,DateTime DtStamp)
	    {
		    InvoiceTransaction item = new InvoiceTransaction();
		    
            item.ProcessorId = ProcessorId;
            
            item.TInvoiceId = TInvoiceId;
            
            item.PerformedBy = PerformedBy;
            
            item.Admin = Admin;
            
            item.UserId = UserId;
            
            item.CustomerId = CustomerId;
            
            item.TInvoiceItemId = TInvoiceItemId;
            
            item.TransType = TransType;
            
            item.FundsType = FundsType;
            
            item.FundsProcessor = FundsProcessor;
            
            item.BatchId = BatchId;
            
            item.MAmount = MAmount;
            
            item.NameOnCard = NameOnCard;
            
            item.LastFourDigits = LastFourDigits;
            
            item.UserIp = UserIp;
            
            item.DtStamp = DtStamp;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,string ProcessorId,int TInvoiceId,string PerformedBy,string Admin,Guid UserId,int? CustomerId,int? TInvoiceItemId,string TransType,string FundsType,string FundsProcessor,string BatchId,decimal MAmount,string NameOnCard,string LastFourDigits,string UserIp,DateTime DtStamp)
	    {
		    InvoiceTransaction item = new InvoiceTransaction();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.ProcessorId = ProcessorId;
				
			item.TInvoiceId = TInvoiceId;
				
			item.PerformedBy = PerformedBy;
				
			item.Admin = Admin;
				
			item.UserId = UserId;
				
			item.CustomerId = CustomerId;
				
			item.TInvoiceItemId = TInvoiceItemId;
				
			item.TransType = TransType;
				
			item.FundsType = FundsType;
				
			item.FundsProcessor = FundsProcessor;
				
			item.BatchId = BatchId;
				
			item.MAmount = MAmount;
				
			item.NameOnCard = NameOnCard;
				
			item.LastFourDigits = LastFourDigits;
				
			item.UserIp = UserIp;
				
			item.DtStamp = DtStamp;
				
	        item.Save(UserName);
	    }
    }
}
