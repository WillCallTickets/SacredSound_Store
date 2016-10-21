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
	/// Strongly-typed collection for the StoreCredit class.
	/// </summary>
    [Serializable]
	public partial class StoreCreditCollection : ActiveList<StoreCredit, StoreCreditCollection>
	{	   
		public StoreCreditCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>StoreCreditCollection</returns>
		public StoreCreditCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                StoreCredit o = this[i];
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
	/// This is an ActiveRecord class which wraps the StoreCredit table.
	/// </summary>
	[Serializable]
	public partial class StoreCredit : ActiveRecord<StoreCredit>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public StoreCredit()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public StoreCredit(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public StoreCredit(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public StoreCredit(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("StoreCredit", TableType.Table, DataService.GetInstance("WillCall"));
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
				
				TableSchema.TableColumn colvarRedemptionId = new TableSchema.TableColumn(schema);
				colvarRedemptionId.ColumnName = "RedemptionId";
				colvarRedemptionId.DataType = DbType.Guid;
				colvarRedemptionId.MaxLength = 0;
				colvarRedemptionId.AutoIncrement = false;
				colvarRedemptionId.IsNullable = true;
				colvarRedemptionId.IsPrimaryKey = false;
				colvarRedemptionId.IsForeignKey = false;
				colvarRedemptionId.IsReadOnly = false;
				colvarRedemptionId.DefaultSetting = @"";
				colvarRedemptionId.ForeignKeyTableName = "";
				schema.Columns.Add(colvarRedemptionId);
				
				TableSchema.TableColumn colvarTInvoiceTransactionId = new TableSchema.TableColumn(schema);
				colvarTInvoiceTransactionId.ColumnName = "tInvoiceTransactionId";
				colvarTInvoiceTransactionId.DataType = DbType.Int32;
				colvarTInvoiceTransactionId.MaxLength = 0;
				colvarTInvoiceTransactionId.AutoIncrement = false;
				colvarTInvoiceTransactionId.IsNullable = true;
				colvarTInvoiceTransactionId.IsPrimaryKey = false;
				colvarTInvoiceTransactionId.IsForeignKey = true;
				colvarTInvoiceTransactionId.IsReadOnly = false;
				colvarTInvoiceTransactionId.DefaultSetting = @"";
				
					colvarTInvoiceTransactionId.ForeignKeyTableName = "InvoiceTransaction";
				schema.Columns.Add(colvarTInvoiceTransactionId);
				
				TableSchema.TableColumn colvarComment = new TableSchema.TableColumn(schema);
				colvarComment.ColumnName = "Comment";
				colvarComment.DataType = DbType.AnsiString;
				colvarComment.MaxLength = 1000;
				colvarComment.AutoIncrement = false;
				colvarComment.IsNullable = true;
				colvarComment.IsPrimaryKey = false;
				colvarComment.IsForeignKey = false;
				colvarComment.IsReadOnly = false;
				colvarComment.DefaultSetting = @"";
				colvarComment.ForeignKeyTableName = "";
				schema.Columns.Add(colvarComment);
				
				TableSchema.TableColumn colvarUserId = new TableSchema.TableColumn(schema);
				colvarUserId.ColumnName = "UserId";
				colvarUserId.DataType = DbType.Guid;
				colvarUserId.MaxLength = 0;
				colvarUserId.AutoIncrement = false;
				colvarUserId.IsNullable = true;
				colvarUserId.IsPrimaryKey = false;
				colvarUserId.IsForeignKey = true;
				colvarUserId.IsReadOnly = false;
				colvarUserId.DefaultSetting = @"";
				
					colvarUserId.ForeignKeyTableName = "aspnet_Users";
				schema.Columns.Add(colvarUserId);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["WillCall"].AddSchema("StoreCredit",schema);
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
		  
		[XmlAttribute("MAmount")]
		[Bindable(true)]
		public decimal MAmount 
		{
			get { return GetColumnValue<decimal>(Columns.MAmount); }
			set { SetColumnValue(Columns.MAmount, value); }
		}
		  
		[XmlAttribute("RedemptionId")]
		[Bindable(true)]
		public Guid? RedemptionId 
		{
			get { return GetColumnValue<Guid?>(Columns.RedemptionId); }
			set { SetColumnValue(Columns.RedemptionId, value); }
		}
		  
		[XmlAttribute("TInvoiceTransactionId")]
		[Bindable(true)]
		public int? TInvoiceTransactionId 
		{
			get { return GetColumnValue<int?>(Columns.TInvoiceTransactionId); }
			set { SetColumnValue(Columns.TInvoiceTransactionId, value); }
		}
		  
		[XmlAttribute("Comment")]
		[Bindable(true)]
		public string Comment 
		{
			get { return GetColumnValue<string>(Columns.Comment); }
			set { SetColumnValue(Columns.Comment, value); }
		}
		  
		[XmlAttribute("UserId")]
		[Bindable(true)]
		public Guid? UserId 
		{
			get { return GetColumnValue<Guid?>(Columns.UserId); }
			set { SetColumnValue(Columns.UserId, value); }
		}
		
		#endregion
		
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a AspnetApplication ActiveRecord object related to this StoreCredit
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
		/// Returns a AspnetUser ActiveRecord object related to this StoreCredit
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
		/// Returns a InvoiceTransaction ActiveRecord object related to this StoreCredit
		/// 
		/// </summary>
		private Wcss.InvoiceTransaction InvoiceTransaction
		{
			get { return Wcss.InvoiceTransaction.FetchByID(this.TInvoiceTransactionId); }
			set { SetColumnValue("tInvoiceTransactionId", value.Id); }
		}
        //set up an alternate mechanism to avoid a database call
		private Wcss.InvoiceTransaction _invoicetransactionrecord = null;
		
		public Wcss.InvoiceTransaction InvoiceTransactionRecord
		{
		    get
            {
                if (_invoicetransactionrecord == null)
                {
                    _invoicetransactionrecord = new Wcss.InvoiceTransaction();
                    _invoicetransactionrecord.CopyFrom(this.InvoiceTransaction);
                }
                return _invoicetransactionrecord;
            }
            set
            {
                if(value != null && _invoicetransactionrecord == null)
			        _invoicetransactionrecord = new Wcss.InvoiceTransaction();
                
                SetColumnValue("tInvoiceTransactionId", value.Id);
                _invoicetransactionrecord.CopyFrom(value);                
            }
		}
		
		
		#endregion
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(DateTime varDtStamp,Guid varApplicationId,decimal varMAmount,Guid? varRedemptionId,int? varTInvoiceTransactionId,string varComment,Guid? varUserId)
		{
			StoreCredit item = new StoreCredit();
			
			item.DtStamp = varDtStamp;
			
			item.ApplicationId = varApplicationId;
			
			item.MAmount = varMAmount;
			
			item.RedemptionId = varRedemptionId;
			
			item.TInvoiceTransactionId = varTInvoiceTransactionId;
			
			item.Comment = varComment;
			
			item.UserId = varUserId;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varId,DateTime varDtStamp,Guid varApplicationId,decimal varMAmount,Guid? varRedemptionId,int? varTInvoiceTransactionId,string varComment,Guid? varUserId)
		{
			StoreCredit item = new StoreCredit();
			
				item.Id = varId;
			
				item.DtStamp = varDtStamp;
			
				item.ApplicationId = varApplicationId;
			
				item.MAmount = varMAmount;
			
				item.RedemptionId = varRedemptionId;
			
				item.TInvoiceTransactionId = varTInvoiceTransactionId;
			
				item.Comment = varComment;
			
				item.UserId = varUserId;
			
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
        
        
        
        public static TableSchema.TableColumn DtStampColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn ApplicationIdColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn MAmountColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn RedemptionIdColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn TInvoiceTransactionIdColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn CommentColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn UserIdColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Id = @"Id";
			 public static string DtStamp = @"dtStamp";
			 public static string ApplicationId = @"ApplicationId";
			 public static string MAmount = @"mAmount";
			 public static string RedemptionId = @"RedemptionId";
			 public static string TInvoiceTransactionId = @"tInvoiceTransactionId";
			 public static string Comment = @"Comment";
			 public static string UserId = @"UserId";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
