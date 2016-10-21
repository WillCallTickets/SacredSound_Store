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
	/// Strongly-typed collection for the Invoice class.
	/// </summary>
    [Serializable]
	public partial class InvoiceCollection : ActiveList<Invoice, InvoiceCollection>
	{	   
		public InvoiceCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>InvoiceCollection</returns>
		public InvoiceCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                Invoice o = this[i];
                foreach (SubSonic.Where w in this.wheres)
                {
                    bool remove = false;
                    System.Reflection.PropertyInfo pi = o.GetType().GetProperty(w.ColumnName);
                    if (pi.CanRead)
                    {
                        object val = pi.GetValue(o, null);
                        switch (w.Comparison)
                        {
                            case SubSonic.Comparison.Equals:
                                if (!val.Equals(w.ParameterValue))
                                {
                                    remove = true;
                                }
                                break;
                        }
                    }
                    if (remove)
                    {
                        this.Remove(o);
                        break;
                    }
                }
            }
            return this;
        }
		
		
	}
	/// <summary>
	/// This is an ActiveRecord class which wraps the Invoice table.
	/// </summary>
	[Serializable]
	public partial class Invoice : ActiveRecord<Invoice>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public Invoice()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public Invoice(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public Invoice(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public Invoice(string columnName, object columnValue)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByParam(columnName,columnValue);
		}
		
		protected static void SetSQLProps() { GetTableSchema(); }
		
		#endregion
		
		#region Schema and Query Accessor	
		public static Query CreateQuery() { return new Query(Schema); }
		public static TableSchema.Table Schema
		{
			get
			{
				if (BaseSchema == null)
					SetSQLProps();
				return BaseSchema;
			}
		}
		
		private static void GetTableSchema() 
		{
			if(!IsSchemaInitialized)
			{
				//Schema declaration
				TableSchema.Table schema = new TableSchema.Table("Invoice", TableType.Table, DataService.GetInstance("WillCall"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarId = new TableSchema.TableColumn(schema);
				colvarId.ColumnName = "Id";
				colvarId.DataType = DbType.Int32;
				colvarId.MaxLength = 0;
				colvarId.AutoIncrement = true;
				colvarId.IsNullable = false;
				colvarId.IsPrimaryKey = true;
				colvarId.IsForeignKey = false;
				colvarId.IsReadOnly = false;
				colvarId.DefaultSetting = @"";
				colvarId.ForeignKeyTableName = "";
				schema.Columns.Add(colvarId);
				
				TableSchema.TableColumn colvarUniqueId = new TableSchema.TableColumn(schema);
				colvarUniqueId.ColumnName = "UniqueId";
				colvarUniqueId.DataType = DbType.AnsiString;
				colvarUniqueId.MaxLength = 20;
				colvarUniqueId.AutoIncrement = false;
				colvarUniqueId.IsNullable = false;
				colvarUniqueId.IsPrimaryKey = false;
				colvarUniqueId.IsForeignKey = false;
				colvarUniqueId.IsReadOnly = false;
				colvarUniqueId.DefaultSetting = @"";
				colvarUniqueId.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUniqueId);
				
				TableSchema.TableColumn colvarTVendorId = new TableSchema.TableColumn(schema);
				colvarTVendorId.ColumnName = "TVendorId";
				colvarTVendorId.DataType = DbType.Int32;
				colvarTVendorId.MaxLength = 0;
				colvarTVendorId.AutoIncrement = false;
				colvarTVendorId.IsNullable = false;
				colvarTVendorId.IsPrimaryKey = false;
				colvarTVendorId.IsForeignKey = true;
				colvarTVendorId.IsReadOnly = false;
				colvarTVendorId.DefaultSetting = @"";
				
					colvarTVendorId.ForeignKeyTableName = "Vendor";
				schema.Columns.Add(colvarTVendorId);
				
				TableSchema.TableColumn colvarPurchaseEmail = new TableSchema.TableColumn(schema);
				colvarPurchaseEmail.ColumnName = "PurchaseEmail";
				colvarPurchaseEmail.DataType = DbType.AnsiString;
				colvarPurchaseEmail.MaxLength = 256;
				colvarPurchaseEmail.AutoIncrement = false;
				colvarPurchaseEmail.IsNullable = false;
				colvarPurchaseEmail.IsPrimaryKey = false;
				colvarPurchaseEmail.IsForeignKey = false;
				colvarPurchaseEmail.IsReadOnly = false;
				colvarPurchaseEmail.DefaultSetting = @"";
				colvarPurchaseEmail.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPurchaseEmail);
				
				TableSchema.TableColumn colvarUserId = new TableSchema.TableColumn(schema);
				colvarUserId.ColumnName = "UserId";
				colvarUserId.DataType = DbType.Guid;
				colvarUserId.MaxLength = 0;
				colvarUserId.AutoIncrement = false;
				colvarUserId.IsNullable = false;
				colvarUserId.IsPrimaryKey = false;
				colvarUserId.IsForeignKey = true;
				colvarUserId.IsReadOnly = false;
				colvarUserId.DefaultSetting = @"";
				
					colvarUserId.ForeignKeyTableName = "aspnet_Users";
				schema.Columns.Add(colvarUserId);
				
				TableSchema.TableColumn colvarCustomerId = new TableSchema.TableColumn(schema);
				colvarCustomerId.ColumnName = "CustomerId";
				colvarCustomerId.DataType = DbType.Int32;
				colvarCustomerId.MaxLength = 0;
				colvarCustomerId.AutoIncrement = false;
				colvarCustomerId.IsNullable = false;
				colvarCustomerId.IsPrimaryKey = false;
				colvarCustomerId.IsForeignKey = false;
				colvarCustomerId.IsReadOnly = false;
				colvarCustomerId.DefaultSetting = @"";
				colvarCustomerId.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCustomerId);
				
				TableSchema.TableColumn colvarDtInvoiceDate = new TableSchema.TableColumn(schema);
				colvarDtInvoiceDate.ColumnName = "dtInvoiceDate";
				colvarDtInvoiceDate.DataType = DbType.DateTime;
				colvarDtInvoiceDate.MaxLength = 0;
				colvarDtInvoiceDate.AutoIncrement = false;
				colvarDtInvoiceDate.IsNullable = false;
				colvarDtInvoiceDate.IsPrimaryKey = false;
				colvarDtInvoiceDate.IsForeignKey = false;
				colvarDtInvoiceDate.IsReadOnly = false;
				colvarDtInvoiceDate.DefaultSetting = @"";
				colvarDtInvoiceDate.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDtInvoiceDate);
				
				TableSchema.TableColumn colvarCreator = new TableSchema.TableColumn(schema);
				colvarCreator.ColumnName = "Creator";
				colvarCreator.DataType = DbType.AnsiString;
				colvarCreator.MaxLength = 50;
				colvarCreator.AutoIncrement = false;
				colvarCreator.IsNullable = true;
				colvarCreator.IsPrimaryKey = false;
				colvarCreator.IsForeignKey = false;
				colvarCreator.IsReadOnly = false;
				colvarCreator.DefaultSetting = @"";
				colvarCreator.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCreator);
				
				TableSchema.TableColumn colvarOrderType = new TableSchema.TableColumn(schema);
				colvarOrderType.ColumnName = "OrderType";
				colvarOrderType.DataType = DbType.AnsiString;
				colvarOrderType.MaxLength = 50;
				colvarOrderType.AutoIncrement = false;
				colvarOrderType.IsNullable = false;
				colvarOrderType.IsPrimaryKey = false;
				colvarOrderType.IsForeignKey = false;
				colvarOrderType.IsReadOnly = false;
				colvarOrderType.DefaultSetting = @"";
				colvarOrderType.ForeignKeyTableName = "";
				schema.Columns.Add(colvarOrderType);
				
				TableSchema.TableColumn colvarVcProducts = new TableSchema.TableColumn(schema);
				colvarVcProducts.ColumnName = "vcProducts";
				colvarVcProducts.DataType = DbType.AnsiString;
				colvarVcProducts.MaxLength = 1500;
				colvarVcProducts.AutoIncrement = false;
				colvarVcProducts.IsNullable = true;
				colvarVcProducts.IsPrimaryKey = false;
				colvarVcProducts.IsForeignKey = false;
				colvarVcProducts.IsReadOnly = false;
				colvarVcProducts.DefaultSetting = @"";
				colvarVcProducts.ForeignKeyTableName = "";
				schema.Columns.Add(colvarVcProducts);
				
				TableSchema.TableColumn colvarMBalanceDue = new TableSchema.TableColumn(schema);
				colvarMBalanceDue.ColumnName = "mBalanceDue";
				colvarMBalanceDue.DataType = DbType.Currency;
				colvarMBalanceDue.MaxLength = 0;
				colvarMBalanceDue.AutoIncrement = false;
				colvarMBalanceDue.IsNullable = false;
				colvarMBalanceDue.IsPrimaryKey = false;
				colvarMBalanceDue.IsForeignKey = false;
				colvarMBalanceDue.IsReadOnly = false;
				
						colvarMBalanceDue.DefaultSetting = @"((0))";
				colvarMBalanceDue.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMBalanceDue);
				
				TableSchema.TableColumn colvarMTotalPaid = new TableSchema.TableColumn(schema);
				colvarMTotalPaid.ColumnName = "mTotalPaid";
				colvarMTotalPaid.DataType = DbType.Currency;
				colvarMTotalPaid.MaxLength = 0;
				colvarMTotalPaid.AutoIncrement = false;
				colvarMTotalPaid.IsNullable = false;
				colvarMTotalPaid.IsPrimaryKey = false;
				colvarMTotalPaid.IsForeignKey = false;
				colvarMTotalPaid.IsReadOnly = false;
				
						colvarMTotalPaid.DefaultSetting = @"((0))";
				colvarMTotalPaid.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMTotalPaid);
				
				TableSchema.TableColumn colvarMTotalRefunds = new TableSchema.TableColumn(schema);
				colvarMTotalRefunds.ColumnName = "mTotalRefunds";
				colvarMTotalRefunds.DataType = DbType.Currency;
				colvarMTotalRefunds.MaxLength = 0;
				colvarMTotalRefunds.AutoIncrement = false;
				colvarMTotalRefunds.IsNullable = false;
				colvarMTotalRefunds.IsPrimaryKey = false;
				colvarMTotalRefunds.IsForeignKey = false;
				colvarMTotalRefunds.IsReadOnly = false;
				
						colvarMTotalRefunds.DefaultSetting = @"((0))";
				colvarMTotalRefunds.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMTotalRefunds);
				
				TableSchema.TableColumn colvarMNetPaid = new TableSchema.TableColumn(schema);
				colvarMNetPaid.ColumnName = "mNetPaid";
				colvarMNetPaid.DataType = DbType.Currency;
				colvarMNetPaid.MaxLength = 0;
				colvarMNetPaid.AutoIncrement = false;
				colvarMNetPaid.IsNullable = true;
				colvarMNetPaid.IsPrimaryKey = false;
				colvarMNetPaid.IsForeignKey = false;
				colvarMNetPaid.IsReadOnly = true;
				colvarMNetPaid.DefaultSetting = @"";
				colvarMNetPaid.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMNetPaid);
				
				TableSchema.TableColumn colvarInvoiceStatus = new TableSchema.TableColumn(schema);
				colvarInvoiceStatus.ColumnName = "InvoiceStatus";
				colvarInvoiceStatus.DataType = DbType.AnsiString;
				colvarInvoiceStatus.MaxLength = 50;
				colvarInvoiceStatus.AutoIncrement = false;
				colvarInvoiceStatus.IsNullable = false;
				colvarInvoiceStatus.IsPrimaryKey = false;
				colvarInvoiceStatus.IsForeignKey = false;
				colvarInvoiceStatus.IsReadOnly = false;
				colvarInvoiceStatus.DefaultSetting = @"";
				colvarInvoiceStatus.ForeignKeyTableName = "";
				schema.Columns.Add(colvarInvoiceStatus);
				
				TableSchema.TableColumn colvarTCashewId = new TableSchema.TableColumn(schema);
				colvarTCashewId.ColumnName = "TCashewId";
				colvarTCashewId.DataType = DbType.Int32;
				colvarTCashewId.MaxLength = 0;
				colvarTCashewId.AutoIncrement = false;
				colvarTCashewId.IsNullable = true;
				colvarTCashewId.IsPrimaryKey = false;
				colvarTCashewId.IsForeignKey = true;
				colvarTCashewId.IsReadOnly = false;
				colvarTCashewId.DefaultSetting = @"";
				
					colvarTCashewId.ForeignKeyTableName = "Cashew";
				schema.Columns.Add(colvarTCashewId);
				
				TableSchema.TableColumn colvarMarketingKeys = new TableSchema.TableColumn(schema);
				colvarMarketingKeys.ColumnName = "MarketingKeys";
				colvarMarketingKeys.DataType = DbType.AnsiString;
				colvarMarketingKeys.MaxLength = 100;
				colvarMarketingKeys.AutoIncrement = false;
				colvarMarketingKeys.IsNullable = true;
				colvarMarketingKeys.IsPrimaryKey = false;
				colvarMarketingKeys.IsForeignKey = false;
				colvarMarketingKeys.IsReadOnly = false;
				colvarMarketingKeys.DefaultSetting = @"";
				colvarMarketingKeys.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMarketingKeys);
				
				TableSchema.TableColumn colvarDtStamp = new TableSchema.TableColumn(schema);
				colvarDtStamp.ColumnName = "dtStamp";
				colvarDtStamp.DataType = DbType.DateTime;
				colvarDtStamp.MaxLength = 0;
				colvarDtStamp.AutoIncrement = false;
				colvarDtStamp.IsNullable = false;
				colvarDtStamp.IsPrimaryKey = false;
				colvarDtStamp.IsForeignKey = false;
				colvarDtStamp.IsReadOnly = false;
				
						colvarDtStamp.DefaultSetting = @"(getdate())";
				colvarDtStamp.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDtStamp);
				
				TableSchema.TableColumn colvarApplicationId = new TableSchema.TableColumn(schema);
				colvarApplicationId.ColumnName = "ApplicationId";
				colvarApplicationId.DataType = DbType.Guid;
				colvarApplicationId.MaxLength = 0;
				colvarApplicationId.AutoIncrement = false;
				colvarApplicationId.IsNullable = false;
				colvarApplicationId.IsPrimaryKey = false;
				colvarApplicationId.IsForeignKey = true;
				colvarApplicationId.IsReadOnly = false;
				colvarApplicationId.DefaultSetting = @"";
				
					colvarApplicationId.ForeignKeyTableName = "aspnet_Applications";
				schema.Columns.Add(colvarApplicationId);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["WillCall"].AddSchema("Invoice",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("Id")]
		[Bindable(true)]
		public int Id 
		{
			get { return GetColumnValue<int>(Columns.Id); }
			set { SetColumnValue(Columns.Id, value); }
		}
		  
		[XmlAttribute("UniqueId")]
		[Bindable(true)]
		public string UniqueId 
		{
			get { return GetColumnValue<string>(Columns.UniqueId); }
			set { SetColumnValue(Columns.UniqueId, value); }
		}
		  
		[XmlAttribute("TVendorId")]
		[Bindable(true)]
		public int TVendorId 
		{
			get { return GetColumnValue<int>(Columns.TVendorId); }
			set { SetColumnValue(Columns.TVendorId, value); }
		}
		  
		[XmlAttribute("PurchaseEmail")]
		[Bindable(true)]
		public string PurchaseEmail 
		{
			get { return GetColumnValue<string>(Columns.PurchaseEmail); }
			set { SetColumnValue(Columns.PurchaseEmail, value); }
		}
		  
		[XmlAttribute("UserId")]
		[Bindable(true)]
		public Guid UserId 
		{
			get { return GetColumnValue<Guid>(Columns.UserId); }
			set { SetColumnValue(Columns.UserId, value); }
		}
		  
		[XmlAttribute("CustomerId")]
		[Bindable(true)]
		public int CustomerId 
		{
			get { return GetColumnValue<int>(Columns.CustomerId); }
			set { SetColumnValue(Columns.CustomerId, value); }
		}
		  
		[XmlAttribute("DtInvoiceDate")]
		[Bindable(true)]
		public DateTime DtInvoiceDate 
		{
			get { return GetColumnValue<DateTime>(Columns.DtInvoiceDate); }
			set { SetColumnValue(Columns.DtInvoiceDate, value); }
		}
		  
		[XmlAttribute("Creator")]
		[Bindable(true)]
		public string Creator 
		{
			get { return GetColumnValue<string>(Columns.Creator); }
			set { SetColumnValue(Columns.Creator, value); }
		}
		  
		[XmlAttribute("OrderType")]
		[Bindable(true)]
		public string OrderType 
		{
			get { return GetColumnValue<string>(Columns.OrderType); }
			set { SetColumnValue(Columns.OrderType, value); }
		}
		  
		[XmlAttribute("VcProducts")]
		[Bindable(true)]
		public string VcProducts 
		{
			get { return GetColumnValue<string>(Columns.VcProducts); }
			set { SetColumnValue(Columns.VcProducts, value); }
		}
		  
		[XmlAttribute("MBalanceDue")]
		[Bindable(true)]
		public decimal MBalanceDue 
		{
			get { return GetColumnValue<decimal>(Columns.MBalanceDue); }
			set { SetColumnValue(Columns.MBalanceDue, value); }
		}
		  
		[XmlAttribute("MTotalPaid")]
		[Bindable(true)]
		public decimal MTotalPaid 
		{
			get { return GetColumnValue<decimal>(Columns.MTotalPaid); }
			set { SetColumnValue(Columns.MTotalPaid, value); }
		}
		  
		[XmlAttribute("MTotalRefunds")]
		[Bindable(true)]
		public decimal MTotalRefunds 
		{
			get { return GetColumnValue<decimal>(Columns.MTotalRefunds); }
			set { SetColumnValue(Columns.MTotalRefunds, value); }
		}
		  
		[XmlAttribute("MNetPaid")]
		[Bindable(true)]
		public decimal? MNetPaid 
		{
			get { return GetColumnValue<decimal?>(Columns.MNetPaid); }
			set { SetColumnValue(Columns.MNetPaid, value); }
		}
		  
		[XmlAttribute("InvoiceStatus")]
		[Bindable(true)]
		public string InvoiceStatus 
		{
			get { return GetColumnValue<string>(Columns.InvoiceStatus); }
			set { SetColumnValue(Columns.InvoiceStatus, value); }
		}
		  
		[XmlAttribute("TCashewId")]
		[Bindable(true)]
		public int? TCashewId 
		{
			get { return GetColumnValue<int?>(Columns.TCashewId); }
			set { SetColumnValue(Columns.TCashewId, value); }
		}
		  
		[XmlAttribute("MarketingKeys")]
		[Bindable(true)]
		public string MarketingKeys 
		{
			get { return GetColumnValue<string>(Columns.MarketingKeys); }
			set { SetColumnValue(Columns.MarketingKeys, value); }
		}
		  
		[XmlAttribute("DtStamp")]
		[Bindable(true)]
		public DateTime DtStamp 
		{
			get { return GetColumnValue<DateTime>(Columns.DtStamp); }
			set { SetColumnValue(Columns.DtStamp, value); }
		}
		  
		[XmlAttribute("ApplicationId")]
		[Bindable(true)]
		public Guid ApplicationId 
		{
			get { return GetColumnValue<Guid>(Columns.ApplicationId); }
			set { SetColumnValue(Columns.ApplicationId, value); }
		}
		
		#endregion
		
		
		#region PrimaryKey Methods		
		
        protected override void SetPrimaryKey(object oValue)
        {
            base.SetPrimaryKey(oValue);
            
            SetPKValues();
        }
        
		
		private Wcss.AuthorizeNetCollection colAuthorizeNetRecords;
		public Wcss.AuthorizeNetCollection AuthorizeNetRecords()
		{
			if(colAuthorizeNetRecords == null)
			{
				colAuthorizeNetRecords = new Wcss.AuthorizeNetCollection().Where(AuthorizeNet.Columns.TInvoiceId, Id).Load();
				colAuthorizeNetRecords.ListChanged += new ListChangedEventHandler(colAuthorizeNetRecords_ListChanged);
			}
			return colAuthorizeNetRecords;
		}
				
		void colAuthorizeNetRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colAuthorizeNetRecords[e.NewIndex].TInvoiceId = Id;
				colAuthorizeNetRecords.ListChanged += new ListChangedEventHandler(colAuthorizeNetRecords_ListChanged);
            }
		}
		private Wcss.InvoiceBillShipCollection colInvoiceBillShipRecords;
		public Wcss.InvoiceBillShipCollection InvoiceBillShipRecords()
		{
			if(colInvoiceBillShipRecords == null)
			{
				colInvoiceBillShipRecords = new Wcss.InvoiceBillShipCollection().Where(InvoiceBillShip.Columns.TInvoiceId, Id).Load();
				colInvoiceBillShipRecords.ListChanged += new ListChangedEventHandler(colInvoiceBillShipRecords_ListChanged);
			}
			return colInvoiceBillShipRecords;
		}
				
		void colInvoiceBillShipRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colInvoiceBillShipRecords[e.NewIndex].TInvoiceId = Id;
				colInvoiceBillShipRecords.ListChanged += new ListChangedEventHandler(colInvoiceBillShipRecords_ListChanged);
            }
		}
		private Wcss.InvoiceEventCollection colInvoiceEventRecords;
		public Wcss.InvoiceEventCollection InvoiceEventRecords()
		{
			if(colInvoiceEventRecords == null)
			{
				colInvoiceEventRecords = new Wcss.InvoiceEventCollection().Where(InvoiceEvent.Columns.TInvoiceId, Id).Load();
				colInvoiceEventRecords.ListChanged += new ListChangedEventHandler(colInvoiceEventRecords_ListChanged);
			}
			return colInvoiceEventRecords;
		}
				
		void colInvoiceEventRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colInvoiceEventRecords[e.NewIndex].TInvoiceId = Id;
				colInvoiceEventRecords.ListChanged += new ListChangedEventHandler(colInvoiceEventRecords_ListChanged);
            }
		}
		private Wcss.InvoiceItemCollection colInvoiceItemRecords;
		public Wcss.InvoiceItemCollection InvoiceItemRecords()
		{
			if(colInvoiceItemRecords == null)
			{
				colInvoiceItemRecords = new Wcss.InvoiceItemCollection().Where(InvoiceItem.Columns.TInvoiceId, Id).Load();
				colInvoiceItemRecords.ListChanged += new ListChangedEventHandler(colInvoiceItemRecords_ListChanged);
			}
			return colInvoiceItemRecords;
		}
				
		void colInvoiceItemRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colInvoiceItemRecords[e.NewIndex].TInvoiceId = Id;
				colInvoiceItemRecords.ListChanged += new ListChangedEventHandler(colInvoiceItemRecords_ListChanged);
            }
		}
		private Wcss.InvoiceShipmentCollection colInvoiceShipmentRecords;
		public Wcss.InvoiceShipmentCollection InvoiceShipmentRecords()
		{
			if(colInvoiceShipmentRecords == null)
			{
				colInvoiceShipmentRecords = new Wcss.InvoiceShipmentCollection().Where(InvoiceShipment.Columns.TInvoiceId, Id).Load();
				colInvoiceShipmentRecords.ListChanged += new ListChangedEventHandler(colInvoiceShipmentRecords_ListChanged);
			}
			return colInvoiceShipmentRecords;
		}
				
		void colInvoiceShipmentRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colInvoiceShipmentRecords[e.NewIndex].TInvoiceId = Id;
				colInvoiceShipmentRecords.ListChanged += new ListChangedEventHandler(colInvoiceShipmentRecords_ListChanged);
            }
		}
		private Wcss.InvoiceTransactionCollection colInvoiceTransactionRecords;
		public Wcss.InvoiceTransactionCollection InvoiceTransactionRecords()
		{
			if(colInvoiceTransactionRecords == null)
			{
				colInvoiceTransactionRecords = new Wcss.InvoiceTransactionCollection().Where(InvoiceTransaction.Columns.TInvoiceId, Id).Load();
				colInvoiceTransactionRecords.ListChanged += new ListChangedEventHandler(colInvoiceTransactionRecords_ListChanged);
			}
			return colInvoiceTransactionRecords;
		}
				
		void colInvoiceTransactionRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colInvoiceTransactionRecords[e.NewIndex].TInvoiceId = Id;
				colInvoiceTransactionRecords.ListChanged += new ListChangedEventHandler(colInvoiceTransactionRecords_ListChanged);
            }
		}
		#endregion
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a AspnetApplication ActiveRecord object related to this Invoice
		/// 
		/// </summary>
		private Wcss.AspnetApplication AspnetApplication
		{
			get { return Wcss.AspnetApplication.FetchByID(this.ApplicationId); }
			set { SetColumnValue("ApplicationId", value.ApplicationId); }
		}
        //set up an alternate mechanism to avoid a database call
		private Wcss.AspnetApplication _aspnetapplicationrecord = null;
		
		public Wcss.AspnetApplication AspnetApplicationRecord
		{
		    get
            {
                if (_aspnetapplicationrecord == null)
                {
                    _aspnetapplicationrecord = new Wcss.AspnetApplication();
                    _aspnetapplicationrecord.CopyFrom(this.AspnetApplication);
                }
                return _aspnetapplicationrecord;
            }
            set
            {
                if(value != null && _aspnetapplicationrecord == null)
			        _aspnetapplicationrecord = new Wcss.AspnetApplication();
                
                SetColumnValue("ApplicationId", value.ApplicationId);
                _aspnetapplicationrecord.CopyFrom(value);                
            }
		}
		
		
		/// <summary>
		/// Returns a AspnetUser ActiveRecord object related to this Invoice
		/// 
		/// </summary>
		private Wcss.AspnetUser AspnetUser
		{
			get { return Wcss.AspnetUser.FetchByID(this.UserId); }
			set { SetColumnValue("UserId", value.UserId); }
		}
        //set up an alternate mechanism to avoid a database call
		private Wcss.AspnetUser _aspnetuserrecord = null;
		
		public Wcss.AspnetUser AspnetUserRecord
		{
		    get
            {
                if (_aspnetuserrecord == null)
                {
                    _aspnetuserrecord = new Wcss.AspnetUser();
                    _aspnetuserrecord.CopyFrom(this.AspnetUser);
                }
                return _aspnetuserrecord;
            }
            set
            {
                if(value != null && _aspnetuserrecord == null)
			        _aspnetuserrecord = new Wcss.AspnetUser();
                
                SetColumnValue("UserId", value.UserId);
                _aspnetuserrecord.CopyFrom(value);                
            }
		}
		
		
		/// <summary>
		/// Returns a Cashew ActiveRecord object related to this Invoice
		/// 
		/// </summary>
		private Wcss.Cashew Cashew
		{
			get { return Wcss.Cashew.FetchByID(this.TCashewId); }
			set { SetColumnValue("TCashewId", value.Id); }
		}
        //set up an alternate mechanism to avoid a database call
		private Wcss.Cashew _cashewrecord = null;
		
		public Wcss.Cashew CashewRecord
		{
		    get
            {
                if (_cashewrecord == null)
                {
                    _cashewrecord = new Wcss.Cashew();
                    _cashewrecord.CopyFrom(this.Cashew);
                }
                return _cashewrecord;
            }
            set
            {
                if(value != null && _cashewrecord == null)
			        _cashewrecord = new Wcss.Cashew();
                
                SetColumnValue("TCashewId", value.Id);
                _cashewrecord.CopyFrom(value);                
            }
		}
		
		
		/// <summary>
		/// Returns a Vendor ActiveRecord object related to this Invoice
		/// 
		/// </summary>
		private Wcss.Vendor Vendor
		{
			get { return Wcss.Vendor.FetchByID(this.TVendorId); }
			set { SetColumnValue("TVendorId", value.Id); }
		}
        //set up an alternate mechanism to avoid a database call
		private Wcss.Vendor _vendorrecord = null;
		
		public Wcss.Vendor VendorRecord
		{
		    get
            {
                if (_vendorrecord == null)
                {
                    _vendorrecord = new Wcss.Vendor();
                    _vendorrecord.CopyFrom(this.Vendor);
                }
                return _vendorrecord;
            }
            set
            {
                if(value != null && _vendorrecord == null)
			        _vendorrecord = new Wcss.Vendor();
                
                SetColumnValue("TVendorId", value.Id);
                _vendorrecord.CopyFrom(value);                
            }
		}
		
		
		#endregion
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(string varUniqueId,int varTVendorId,string varPurchaseEmail,Guid varUserId,int varCustomerId,DateTime varDtInvoiceDate,string varCreator,string varOrderType,string varVcProducts,decimal varMBalanceDue,decimal varMTotalPaid,decimal varMTotalRefunds,decimal? varMNetPaid,string varInvoiceStatus,int? varTCashewId,string varMarketingKeys,DateTime varDtStamp,Guid varApplicationId)
		{
			Invoice item = new Invoice();
			
			item.UniqueId = varUniqueId;
			
			item.TVendorId = varTVendorId;
			
			item.PurchaseEmail = varPurchaseEmail;
			
			item.UserId = varUserId;
			
			item.CustomerId = varCustomerId;
			
			item.DtInvoiceDate = varDtInvoiceDate;
			
			item.Creator = varCreator;
			
			item.OrderType = varOrderType;
			
			item.VcProducts = varVcProducts;
			
			item.MBalanceDue = varMBalanceDue;
			
			item.MTotalPaid = varMTotalPaid;
			
			item.MTotalRefunds = varMTotalRefunds;
			
			item.MNetPaid = varMNetPaid;
			
			item.InvoiceStatus = varInvoiceStatus;
			
			item.TCashewId = varTCashewId;
			
			item.MarketingKeys = varMarketingKeys;
			
			item.DtStamp = varDtStamp;
			
			item.ApplicationId = varApplicationId;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varId,string varUniqueId,int varTVendorId,string varPurchaseEmail,Guid varUserId,int varCustomerId,DateTime varDtInvoiceDate,string varCreator,string varOrderType,string varVcProducts,decimal varMBalanceDue,decimal varMTotalPaid,decimal varMTotalRefunds,decimal? varMNetPaid,string varInvoiceStatus,int? varTCashewId,string varMarketingKeys,DateTime varDtStamp,Guid varApplicationId)
		{
			Invoice item = new Invoice();
			
				item.Id = varId;
			
				item.UniqueId = varUniqueId;
			
				item.TVendorId = varTVendorId;
			
				item.PurchaseEmail = varPurchaseEmail;
			
				item.UserId = varUserId;
			
				item.CustomerId = varCustomerId;
			
				item.DtInvoiceDate = varDtInvoiceDate;
			
				item.Creator = varCreator;
			
				item.OrderType = varOrderType;
			
				item.VcProducts = varVcProducts;
			
				item.MBalanceDue = varMBalanceDue;
			
				item.MTotalPaid = varMTotalPaid;
			
				item.MTotalRefunds = varMTotalRefunds;
			
				item.MNetPaid = varMNetPaid;
			
				item.InvoiceStatus = varInvoiceStatus;
			
				item.TCashewId = varTCashewId;
			
				item.MarketingKeys = varMarketingKeys;
			
				item.DtStamp = varDtStamp;
			
				item.ApplicationId = varApplicationId;
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn IdColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn UniqueIdColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn TVendorIdColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn PurchaseEmailColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn UserIdColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn CustomerIdColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn DtInvoiceDateColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatorColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn OrderTypeColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn VcProductsColumn
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        public static TableSchema.TableColumn MBalanceDueColumn
        {
            get { return Schema.Columns[10]; }
        }
        
        
        
        public static TableSchema.TableColumn MTotalPaidColumn
        {
            get { return Schema.Columns[11]; }
        }
        
        
        
        public static TableSchema.TableColumn MTotalRefundsColumn
        {
            get { return Schema.Columns[12]; }
        }
        
        
        
        public static TableSchema.TableColumn MNetPaidColumn
        {
            get { return Schema.Columns[13]; }
        }
        
        
        
        public static TableSchema.TableColumn InvoiceStatusColumn
        {
            get { return Schema.Columns[14]; }
        }
        
        
        
        public static TableSchema.TableColumn TCashewIdColumn
        {
            get { return Schema.Columns[15]; }
        }
        
        
        
        public static TableSchema.TableColumn MarketingKeysColumn
        {
            get { return Schema.Columns[16]; }
        }
        
        
        
        public static TableSchema.TableColumn DtStampColumn
        {
            get { return Schema.Columns[17]; }
        }
        
        
        
        public static TableSchema.TableColumn ApplicationIdColumn
        {
            get { return Schema.Columns[18]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Id = @"Id";
			 public static string UniqueId = @"UniqueId";
			 public static string TVendorId = @"TVendorId";
			 public static string PurchaseEmail = @"PurchaseEmail";
			 public static string UserId = @"UserId";
			 public static string CustomerId = @"CustomerId";
			 public static string DtInvoiceDate = @"dtInvoiceDate";
			 public static string Creator = @"Creator";
			 public static string OrderType = @"OrderType";
			 public static string VcProducts = @"vcProducts";
			 public static string MBalanceDue = @"mBalanceDue";
			 public static string MTotalPaid = @"mTotalPaid";
			 public static string MTotalRefunds = @"mTotalRefunds";
			 public static string MNetPaid = @"mNetPaid";
			 public static string InvoiceStatus = @"InvoiceStatus";
			 public static string TCashewId = @"TCashewId";
			 public static string MarketingKeys = @"MarketingKeys";
			 public static string DtStamp = @"dtStamp";
			 public static string ApplicationId = @"ApplicationId";
						
		}
		#endregion
		
		#region Update PK Collections
		
        public void SetPKValues()
        {
                if (colAuthorizeNetRecords != null)
                {
                    foreach (Wcss.AuthorizeNet item in colAuthorizeNetRecords)
                    {
                        if (item.TInvoiceId != Id)
                        {
                            item.TInvoiceId = Id;
                        }
                    }
               }
		
                if (colInvoiceBillShipRecords != null)
                {
                    foreach (Wcss.InvoiceBillShip item in colInvoiceBillShipRecords)
                    {
                        if (item.TInvoiceId != Id)
                        {
                            item.TInvoiceId = Id;
                        }
                    }
               }
		
                if (colInvoiceEventRecords != null)
                {
                    foreach (Wcss.InvoiceEvent item in colInvoiceEventRecords)
                    {
                        if (item.TInvoiceId != Id)
                        {
                            item.TInvoiceId = Id;
                        }
                    }
               }
		
                if (colInvoiceItemRecords != null)
                {
                    foreach (Wcss.InvoiceItem item in colInvoiceItemRecords)
                    {
                        if (item.TInvoiceId != Id)
                        {
                            item.TInvoiceId = Id;
                        }
                    }
               }
		
                if (colInvoiceShipmentRecords != null)
                {
                    foreach (Wcss.InvoiceShipment item in colInvoiceShipmentRecords)
                    {
                        if (item.TInvoiceId != Id)
                        {
                            item.TInvoiceId = Id;
                        }
                    }
               }
		
                if (colInvoiceTransactionRecords != null)
                {
                    foreach (Wcss.InvoiceTransaction item in colInvoiceTransactionRecords)
                    {
                        if (item.TInvoiceId != Id)
                        {
                            item.TInvoiceId = Id;
                        }
                    }
               }
		}
        #endregion
    
        #region Deep Save
		
        public void DeepSave()
        {
            Save();
            
                if (colAuthorizeNetRecords != null)
                {
                    colAuthorizeNetRecords.SaveAll();
               }
		
                if (colInvoiceBillShipRecords != null)
                {
                    colInvoiceBillShipRecords.SaveAll();
               }
		
                if (colInvoiceEventRecords != null)
                {
                    colInvoiceEventRecords.SaveAll();
               }
		
                if (colInvoiceItemRecords != null)
                {
                    colInvoiceItemRecords.SaveAll();
               }
		
                if (colInvoiceShipmentRecords != null)
                {
                    colInvoiceShipmentRecords.SaveAll();
               }
		
                if (colInvoiceTransactionRecords != null)
                {
                    colInvoiceTransactionRecords.SaveAll();
               }
		}
        #endregion
	}
}
