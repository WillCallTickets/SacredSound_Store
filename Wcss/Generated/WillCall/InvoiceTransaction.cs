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
	/// Strongly-typed collection for the InvoiceTransaction class.
	/// </summary>
    [Serializable]
	public partial class InvoiceTransactionCollection : ActiveList<InvoiceTransaction, InvoiceTransactionCollection>
	{	   
		public InvoiceTransactionCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>InvoiceTransactionCollection</returns>
		public InvoiceTransactionCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                InvoiceTransaction o = this[i];
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
	/// This is an ActiveRecord class which wraps the InvoiceTransaction table.
	/// </summary>
	[Serializable]
	public partial class InvoiceTransaction : ActiveRecord<InvoiceTransaction>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public InvoiceTransaction()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public InvoiceTransaction(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public InvoiceTransaction(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public InvoiceTransaction(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("InvoiceTransaction", TableType.Table, DataService.GetInstance("WillCall"));
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
				
				TableSchema.TableColumn colvarProcessorId = new TableSchema.TableColumn(schema);
				colvarProcessorId.ColumnName = "ProcessorId";
				colvarProcessorId.DataType = DbType.AnsiString;
				colvarProcessorId.MaxLength = 50;
				colvarProcessorId.AutoIncrement = false;
				colvarProcessorId.IsNullable = false;
				colvarProcessorId.IsPrimaryKey = false;
				colvarProcessorId.IsForeignKey = false;
				colvarProcessorId.IsReadOnly = false;
				colvarProcessorId.DefaultSetting = @"";
				colvarProcessorId.ForeignKeyTableName = "";
				schema.Columns.Add(colvarProcessorId);
				
				TableSchema.TableColumn colvarTInvoiceId = new TableSchema.TableColumn(schema);
				colvarTInvoiceId.ColumnName = "TInvoiceId";
				colvarTInvoiceId.DataType = DbType.Int32;
				colvarTInvoiceId.MaxLength = 0;
				colvarTInvoiceId.AutoIncrement = false;
				colvarTInvoiceId.IsNullable = false;
				colvarTInvoiceId.IsPrimaryKey = false;
				colvarTInvoiceId.IsForeignKey = true;
				colvarTInvoiceId.IsReadOnly = false;
				colvarTInvoiceId.DefaultSetting = @"";
				
					colvarTInvoiceId.ForeignKeyTableName = "Invoice";
				schema.Columns.Add(colvarTInvoiceId);
				
				TableSchema.TableColumn colvarPerformedBy = new TableSchema.TableColumn(schema);
				colvarPerformedBy.ColumnName = "PerformedBy";
				colvarPerformedBy.DataType = DbType.AnsiString;
				colvarPerformedBy.MaxLength = 20;
				colvarPerformedBy.AutoIncrement = false;
				colvarPerformedBy.IsNullable = false;
				colvarPerformedBy.IsPrimaryKey = false;
				colvarPerformedBy.IsForeignKey = false;
				colvarPerformedBy.IsReadOnly = false;
				colvarPerformedBy.DefaultSetting = @"";
				colvarPerformedBy.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPerformedBy);
				
				TableSchema.TableColumn colvarAdmin = new TableSchema.TableColumn(schema);
				colvarAdmin.ColumnName = "Admin";
				colvarAdmin.DataType = DbType.AnsiString;
				colvarAdmin.MaxLength = 50;
				colvarAdmin.AutoIncrement = false;
				colvarAdmin.IsNullable = true;
				colvarAdmin.IsPrimaryKey = false;
				colvarAdmin.IsForeignKey = false;
				colvarAdmin.IsReadOnly = false;
				colvarAdmin.DefaultSetting = @"";
				colvarAdmin.ForeignKeyTableName = "";
				schema.Columns.Add(colvarAdmin);
				
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
				colvarCustomerId.IsNullable = true;
				colvarCustomerId.IsPrimaryKey = false;
				colvarCustomerId.IsForeignKey = false;
				colvarCustomerId.IsReadOnly = false;
				colvarCustomerId.DefaultSetting = @"";
				colvarCustomerId.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCustomerId);
				
				TableSchema.TableColumn colvarTInvoiceItemId = new TableSchema.TableColumn(schema);
				colvarTInvoiceItemId.ColumnName = "TInvoiceItemId";
				colvarTInvoiceItemId.DataType = DbType.Int32;
				colvarTInvoiceItemId.MaxLength = 0;
				colvarTInvoiceItemId.AutoIncrement = false;
				colvarTInvoiceItemId.IsNullable = true;
				colvarTInvoiceItemId.IsPrimaryKey = false;
				colvarTInvoiceItemId.IsForeignKey = false;
				colvarTInvoiceItemId.IsReadOnly = false;
				colvarTInvoiceItemId.DefaultSetting = @"";
				colvarTInvoiceItemId.ForeignKeyTableName = "";
				schema.Columns.Add(colvarTInvoiceItemId);
				
				TableSchema.TableColumn colvarTransType = new TableSchema.TableColumn(schema);
				colvarTransType.ColumnName = "TransType";
				colvarTransType.DataType = DbType.AnsiString;
				colvarTransType.MaxLength = 50;
				colvarTransType.AutoIncrement = false;
				colvarTransType.IsNullable = false;
				colvarTransType.IsPrimaryKey = false;
				colvarTransType.IsForeignKey = false;
				colvarTransType.IsReadOnly = false;
				colvarTransType.DefaultSetting = @"";
				colvarTransType.ForeignKeyTableName = "";
				schema.Columns.Add(colvarTransType);
				
				TableSchema.TableColumn colvarFundsType = new TableSchema.TableColumn(schema);
				colvarFundsType.ColumnName = "FundsType";
				colvarFundsType.DataType = DbType.AnsiString;
				colvarFundsType.MaxLength = 50;
				colvarFundsType.AutoIncrement = false;
				colvarFundsType.IsNullable = false;
				colvarFundsType.IsPrimaryKey = false;
				colvarFundsType.IsForeignKey = false;
				colvarFundsType.IsReadOnly = false;
				colvarFundsType.DefaultSetting = @"";
				colvarFundsType.ForeignKeyTableName = "";
				schema.Columns.Add(colvarFundsType);
				
				TableSchema.TableColumn colvarFundsProcessor = new TableSchema.TableColumn(schema);
				colvarFundsProcessor.ColumnName = "FundsProcessor";
				colvarFundsProcessor.DataType = DbType.AnsiString;
				colvarFundsProcessor.MaxLength = 50;
				colvarFundsProcessor.AutoIncrement = false;
				colvarFundsProcessor.IsNullable = false;
				colvarFundsProcessor.IsPrimaryKey = false;
				colvarFundsProcessor.IsForeignKey = false;
				colvarFundsProcessor.IsReadOnly = false;
				colvarFundsProcessor.DefaultSetting = @"";
				colvarFundsProcessor.ForeignKeyTableName = "";
				schema.Columns.Add(colvarFundsProcessor);
				
				TableSchema.TableColumn colvarBatchId = new TableSchema.TableColumn(schema);
				colvarBatchId.ColumnName = "BatchId";
				colvarBatchId.DataType = DbType.AnsiString;
				colvarBatchId.MaxLength = 50;
				colvarBatchId.AutoIncrement = false;
				colvarBatchId.IsNullable = true;
				colvarBatchId.IsPrimaryKey = false;
				colvarBatchId.IsForeignKey = false;
				colvarBatchId.IsReadOnly = false;
				colvarBatchId.DefaultSetting = @"";
				colvarBatchId.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBatchId);
				
				TableSchema.TableColumn colvarMAmount = new TableSchema.TableColumn(schema);
				colvarMAmount.ColumnName = "mAmount";
				colvarMAmount.DataType = DbType.Currency;
				colvarMAmount.MaxLength = 0;
				colvarMAmount.AutoIncrement = false;
				colvarMAmount.IsNullable = false;
				colvarMAmount.IsPrimaryKey = false;
				colvarMAmount.IsForeignKey = false;
				colvarMAmount.IsReadOnly = false;
				colvarMAmount.DefaultSetting = @"";
				colvarMAmount.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMAmount);
				
				TableSchema.TableColumn colvarNameOnCard = new TableSchema.TableColumn(schema);
				colvarNameOnCard.ColumnName = "NameOnCard";
				colvarNameOnCard.DataType = DbType.AnsiString;
				colvarNameOnCard.MaxLength = 50;
				colvarNameOnCard.AutoIncrement = false;
				colvarNameOnCard.IsNullable = true;
				colvarNameOnCard.IsPrimaryKey = false;
				colvarNameOnCard.IsForeignKey = false;
				colvarNameOnCard.IsReadOnly = false;
				colvarNameOnCard.DefaultSetting = @"";
				colvarNameOnCard.ForeignKeyTableName = "";
				schema.Columns.Add(colvarNameOnCard);
				
				TableSchema.TableColumn colvarLastFourDigits = new TableSchema.TableColumn(schema);
				colvarLastFourDigits.ColumnName = "LastFourDigits";
				colvarLastFourDigits.DataType = DbType.AnsiString;
				colvarLastFourDigits.MaxLength = 4;
				colvarLastFourDigits.AutoIncrement = false;
				colvarLastFourDigits.IsNullable = true;
				colvarLastFourDigits.IsPrimaryKey = false;
				colvarLastFourDigits.IsForeignKey = false;
				colvarLastFourDigits.IsReadOnly = false;
				colvarLastFourDigits.DefaultSetting = @"";
				colvarLastFourDigits.ForeignKeyTableName = "";
				schema.Columns.Add(colvarLastFourDigits);
				
				TableSchema.TableColumn colvarUserIp = new TableSchema.TableColumn(schema);
				colvarUserIp.ColumnName = "UserIp";
				colvarUserIp.DataType = DbType.AnsiString;
				colvarUserIp.MaxLength = 50;
				colvarUserIp.AutoIncrement = false;
				colvarUserIp.IsNullable = false;
				colvarUserIp.IsPrimaryKey = false;
				colvarUserIp.IsForeignKey = false;
				colvarUserIp.IsReadOnly = false;
				colvarUserIp.DefaultSetting = @"";
				colvarUserIp.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserIp);
				
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
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["WillCall"].AddSchema("InvoiceTransaction",schema);
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
		  
		[XmlAttribute("ProcessorId")]
		[Bindable(true)]
		public string ProcessorId 
		{
			get { return GetColumnValue<string>(Columns.ProcessorId); }
			set { SetColumnValue(Columns.ProcessorId, value); }
		}
		  
		[XmlAttribute("TInvoiceId")]
		[Bindable(true)]
		public int TInvoiceId 
		{
			get { return GetColumnValue<int>(Columns.TInvoiceId); }
			set { SetColumnValue(Columns.TInvoiceId, value); }
		}
		  
		[XmlAttribute("PerformedBy")]
		[Bindable(true)]
		public string PerformedBy 
		{
			get { return GetColumnValue<string>(Columns.PerformedBy); }
			set { SetColumnValue(Columns.PerformedBy, value); }
		}
		  
		[XmlAttribute("Admin")]
		[Bindable(true)]
		public string Admin 
		{
			get { return GetColumnValue<string>(Columns.Admin); }
			set { SetColumnValue(Columns.Admin, value); }
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
		public int? CustomerId 
		{
			get { return GetColumnValue<int?>(Columns.CustomerId); }
			set { SetColumnValue(Columns.CustomerId, value); }
		}
		  
		[XmlAttribute("TInvoiceItemId")]
		[Bindable(true)]
		public int? TInvoiceItemId 
		{
			get { return GetColumnValue<int?>(Columns.TInvoiceItemId); }
			set { SetColumnValue(Columns.TInvoiceItemId, value); }
		}
		  
		[XmlAttribute("TransType")]
		[Bindable(true)]
		public string TransType 
		{
			get { return GetColumnValue<string>(Columns.TransType); }
			set { SetColumnValue(Columns.TransType, value); }
		}
		  
		[XmlAttribute("FundsType")]
		[Bindable(true)]
		public string FundsType 
		{
			get { return GetColumnValue<string>(Columns.FundsType); }
			set { SetColumnValue(Columns.FundsType, value); }
		}
		  
		[XmlAttribute("FundsProcessor")]
		[Bindable(true)]
		public string FundsProcessor 
		{
			get { return GetColumnValue<string>(Columns.FundsProcessor); }
			set { SetColumnValue(Columns.FundsProcessor, value); }
		}
		  
		[XmlAttribute("BatchId")]
		[Bindable(true)]
		public string BatchId 
		{
			get { return GetColumnValue<string>(Columns.BatchId); }
			set { SetColumnValue(Columns.BatchId, value); }
		}
		  
		[XmlAttribute("MAmount")]
		[Bindable(true)]
		public decimal MAmount 
		{
			get { return GetColumnValue<decimal>(Columns.MAmount); }
			set { SetColumnValue(Columns.MAmount, value); }
		}
		  
		[XmlAttribute("NameOnCard")]
		[Bindable(true)]
		public string NameOnCard 
		{
			get { return GetColumnValue<string>(Columns.NameOnCard); }
			set { SetColumnValue(Columns.NameOnCard, value); }
		}
		  
		[XmlAttribute("LastFourDigits")]
		[Bindable(true)]
		public string LastFourDigits 
		{
			get { return GetColumnValue<string>(Columns.LastFourDigits); }
			set { SetColumnValue(Columns.LastFourDigits, value); }
		}
		  
		[XmlAttribute("UserIp")]
		[Bindable(true)]
		public string UserIp 
		{
			get { return GetColumnValue<string>(Columns.UserIp); }
			set { SetColumnValue(Columns.UserIp, value); }
		}
		  
		[XmlAttribute("DtStamp")]
		[Bindable(true)]
		public DateTime DtStamp 
		{
			get { return GetColumnValue<DateTime>(Columns.DtStamp); }
			set { SetColumnValue(Columns.DtStamp, value); }
		}
		
		#endregion
		
		
		#region PrimaryKey Methods		
		
        protected override void SetPrimaryKey(object oValue)
        {
            base.SetPrimaryKey(oValue);
            
            SetPKValues();
        }
        
		
		private Wcss.StoreCreditCollection colStoreCreditRecords;
		public Wcss.StoreCreditCollection StoreCreditRecords()
		{
			if(colStoreCreditRecords == null)
			{
				colStoreCreditRecords = new Wcss.StoreCreditCollection().Where(StoreCredit.Columns.TInvoiceTransactionId, Id).Load();
				colStoreCreditRecords.ListChanged += new ListChangedEventHandler(colStoreCreditRecords_ListChanged);
			}
			return colStoreCreditRecords;
		}
				
		void colStoreCreditRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colStoreCreditRecords[e.NewIndex].TInvoiceTransactionId = Id;
				colStoreCreditRecords.ListChanged += new ListChangedEventHandler(colStoreCreditRecords_ListChanged);
            }
		}
		#endregion
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a AspnetUser ActiveRecord object related to this InvoiceTransaction
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
		/// Returns a Invoice ActiveRecord object related to this InvoiceTransaction
		/// 
		/// </summary>
		private Wcss.Invoice Invoice
		{
			get { return Wcss.Invoice.FetchByID(this.TInvoiceId); }
			set { SetColumnValue("TInvoiceId", value.Id); }
		}
        //set up an alternate mechanism to avoid a database call
		private Wcss.Invoice _invoicerecord = null;
		
		public Wcss.Invoice InvoiceRecord
		{
		    get
            {
                if (_invoicerecord == null)
                {
                    _invoicerecord = new Wcss.Invoice();
                    _invoicerecord.CopyFrom(this.Invoice);
                }
                return _invoicerecord;
            }
            set
            {
                if(value != null && _invoicerecord == null)
			        _invoicerecord = new Wcss.Invoice();
                
                SetColumnValue("TInvoiceId", value.Id);
                _invoicerecord.CopyFrom(value);                
            }
		}
		
		
		#endregion
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(string varProcessorId,int varTInvoiceId,string varPerformedBy,string varAdmin,Guid varUserId,int? varCustomerId,int? varTInvoiceItemId,string varTransType,string varFundsType,string varFundsProcessor,string varBatchId,decimal varMAmount,string varNameOnCard,string varLastFourDigits,string varUserIp,DateTime varDtStamp)
		{
			InvoiceTransaction item = new InvoiceTransaction();
			
			item.ProcessorId = varProcessorId;
			
			item.TInvoiceId = varTInvoiceId;
			
			item.PerformedBy = varPerformedBy;
			
			item.Admin = varAdmin;
			
			item.UserId = varUserId;
			
			item.CustomerId = varCustomerId;
			
			item.TInvoiceItemId = varTInvoiceItemId;
			
			item.TransType = varTransType;
			
			item.FundsType = varFundsType;
			
			item.FundsProcessor = varFundsProcessor;
			
			item.BatchId = varBatchId;
			
			item.MAmount = varMAmount;
			
			item.NameOnCard = varNameOnCard;
			
			item.LastFourDigits = varLastFourDigits;
			
			item.UserIp = varUserIp;
			
			item.DtStamp = varDtStamp;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varId,string varProcessorId,int varTInvoiceId,string varPerformedBy,string varAdmin,Guid varUserId,int? varCustomerId,int? varTInvoiceItemId,string varTransType,string varFundsType,string varFundsProcessor,string varBatchId,decimal varMAmount,string varNameOnCard,string varLastFourDigits,string varUserIp,DateTime varDtStamp)
		{
			InvoiceTransaction item = new InvoiceTransaction();
			
				item.Id = varId;
			
				item.ProcessorId = varProcessorId;
			
				item.TInvoiceId = varTInvoiceId;
			
				item.PerformedBy = varPerformedBy;
			
				item.Admin = varAdmin;
			
				item.UserId = varUserId;
			
				item.CustomerId = varCustomerId;
			
				item.TInvoiceItemId = varTInvoiceItemId;
			
				item.TransType = varTransType;
			
				item.FundsType = varFundsType;
			
				item.FundsProcessor = varFundsProcessor;
			
				item.BatchId = varBatchId;
			
				item.MAmount = varMAmount;
			
				item.NameOnCard = varNameOnCard;
			
				item.LastFourDigits = varLastFourDigits;
			
				item.UserIp = varUserIp;
			
				item.DtStamp = varDtStamp;
			
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
        
        
        
        public static TableSchema.TableColumn ProcessorIdColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn TInvoiceIdColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn PerformedByColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn AdminColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn UserIdColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn CustomerIdColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn TInvoiceItemIdColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn TransTypeColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn FundsTypeColumn
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        public static TableSchema.TableColumn FundsProcessorColumn
        {
            get { return Schema.Columns[10]; }
        }
        
        
        
        public static TableSchema.TableColumn BatchIdColumn
        {
            get { return Schema.Columns[11]; }
        }
        
        
        
        public static TableSchema.TableColumn MAmountColumn
        {
            get { return Schema.Columns[12]; }
        }
        
        
        
        public static TableSchema.TableColumn NameOnCardColumn
        {
            get { return Schema.Columns[13]; }
        }
        
        
        
        public static TableSchema.TableColumn LastFourDigitsColumn
        {
            get { return Schema.Columns[14]; }
        }
        
        
        
        public static TableSchema.TableColumn UserIpColumn
        {
            get { return Schema.Columns[15]; }
        }
        
        
        
        public static TableSchema.TableColumn DtStampColumn
        {
            get { return Schema.Columns[16]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Id = @"Id";
			 public static string ProcessorId = @"ProcessorId";
			 public static string TInvoiceId = @"TInvoiceId";
			 public static string PerformedBy = @"PerformedBy";
			 public static string Admin = @"Admin";
			 public static string UserId = @"UserId";
			 public static string CustomerId = @"CustomerId";
			 public static string TInvoiceItemId = @"TInvoiceItemId";
			 public static string TransType = @"TransType";
			 public static string FundsType = @"FundsType";
			 public static string FundsProcessor = @"FundsProcessor";
			 public static string BatchId = @"BatchId";
			 public static string MAmount = @"mAmount";
			 public static string NameOnCard = @"NameOnCard";
			 public static string LastFourDigits = @"LastFourDigits";
			 public static string UserIp = @"UserIp";
			 public static string DtStamp = @"dtStamp";
						
		}
		#endregion
		
		#region Update PK Collections
		
        public void SetPKValues()
        {
                if (colStoreCreditRecords != null)
                {
                    foreach (Wcss.StoreCredit item in colStoreCreditRecords)
                    {
                        if (item.TInvoiceTransactionId != Id)
                        {
                            item.TInvoiceTransactionId = Id;
                        }
                    }
               }
		}
        #endregion
    
        #region Deep Save
		
        public void DeepSave()
        {
            Save();
            
                if (colStoreCreditRecords != null)
                {
                    colStoreCreditRecords.SaveAll();
               }
		}
        #endregion
	}
}
