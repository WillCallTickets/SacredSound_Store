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
	/// Strongly-typed collection for the Inventory class.
	/// </summary>
    [Serializable]
	public partial class InventoryCollection : ActiveList<Inventory, InventoryCollection>
	{	   
		public InventoryCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>InventoryCollection</returns>
		public InventoryCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                Inventory o = this[i];
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
	/// This is an ActiveRecord class which wraps the Inventory table.
	/// </summary>
	[Serializable]
	public partial class Inventory : ActiveRecord<Inventory>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public Inventory()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public Inventory(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public Inventory(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public Inventory(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("Inventory", TableType.Table, DataService.GetInstance("WillCall"));
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
				
				TableSchema.TableColumn colvarVcParentContext = new TableSchema.TableColumn(schema);
				colvarVcParentContext.ColumnName = "vcParentContext";
				colvarVcParentContext.DataType = DbType.AnsiString;
				colvarVcParentContext.MaxLength = 1;
				colvarVcParentContext.AutoIncrement = false;
				colvarVcParentContext.IsNullable = false;
				colvarVcParentContext.IsPrimaryKey = false;
				colvarVcParentContext.IsForeignKey = false;
				colvarVcParentContext.IsReadOnly = false;
				colvarVcParentContext.DefaultSetting = @"";
				colvarVcParentContext.ForeignKeyTableName = "";
				schema.Columns.Add(colvarVcParentContext);
				
				TableSchema.TableColumn colvarIParentInventoryId = new TableSchema.TableColumn(schema);
				colvarIParentInventoryId.ColumnName = "iParentInventoryId";
				colvarIParentInventoryId.DataType = DbType.Int32;
				colvarIParentInventoryId.MaxLength = 0;
				colvarIParentInventoryId.AutoIncrement = false;
				colvarIParentInventoryId.IsNullable = false;
				colvarIParentInventoryId.IsPrimaryKey = false;
				colvarIParentInventoryId.IsForeignKey = false;
				colvarIParentInventoryId.IsReadOnly = false;
				colvarIParentInventoryId.DefaultSetting = @"";
				colvarIParentInventoryId.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIParentInventoryId);
				
				TableSchema.TableColumn colvarCode = new TableSchema.TableColumn(schema);
				colvarCode.ColumnName = "Code";
				colvarCode.DataType = DbType.AnsiString;
				colvarCode.MaxLength = 25;
				colvarCode.AutoIncrement = false;
				colvarCode.IsNullable = false;
				colvarCode.IsPrimaryKey = false;
				colvarCode.IsForeignKey = false;
				colvarCode.IsReadOnly = false;
				colvarCode.DefaultSetting = @"";
				colvarCode.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCode);
				
				TableSchema.TableColumn colvarDescription = new TableSchema.TableColumn(schema);
				colvarDescription.ColumnName = "Description";
				colvarDescription.DataType = DbType.AnsiString;
				colvarDescription.MaxLength = 250;
				colvarDescription.AutoIncrement = false;
				colvarDescription.IsNullable = true;
				colvarDescription.IsPrimaryKey = false;
				colvarDescription.IsForeignKey = false;
				colvarDescription.IsReadOnly = false;
				colvarDescription.DefaultSetting = @"";
				colvarDescription.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDescription);
				
				TableSchema.TableColumn colvarGSaleItemId = new TableSchema.TableColumn(schema);
				colvarGSaleItemId.ColumnName = "gSaleItemId";
				colvarGSaleItemId.DataType = DbType.Guid;
				colvarGSaleItemId.MaxLength = 0;
				colvarGSaleItemId.AutoIncrement = false;
				colvarGSaleItemId.IsNullable = true;
				colvarGSaleItemId.IsPrimaryKey = false;
				colvarGSaleItemId.IsForeignKey = false;
				colvarGSaleItemId.IsReadOnly = false;
				colvarGSaleItemId.DefaultSetting = @"";
				colvarGSaleItemId.ForeignKeyTableName = "";
				schema.Columns.Add(colvarGSaleItemId);
				
				TableSchema.TableColumn colvarTInvoiceItemId = new TableSchema.TableColumn(schema);
				colvarTInvoiceItemId.ColumnName = "tInvoiceItemId";
				colvarTInvoiceItemId.DataType = DbType.Int32;
				colvarTInvoiceItemId.MaxLength = 0;
				colvarTInvoiceItemId.AutoIncrement = false;
				colvarTInvoiceItemId.IsNullable = true;
				colvarTInvoiceItemId.IsPrimaryKey = false;
				colvarTInvoiceItemId.IsForeignKey = true;
				colvarTInvoiceItemId.IsReadOnly = false;
				colvarTInvoiceItemId.DefaultSetting = @"";
				
					colvarTInvoiceItemId.ForeignKeyTableName = "InvoiceItem";
				schema.Columns.Add(colvarTInvoiceItemId);
				
				TableSchema.TableColumn colvarDtSold = new TableSchema.TableColumn(schema);
				colvarDtSold.ColumnName = "dtSold";
				colvarDtSold.DataType = DbType.DateTime;
				colvarDtSold.MaxLength = 0;
				colvarDtSold.AutoIncrement = false;
				colvarDtSold.IsNullable = true;
				colvarDtSold.IsPrimaryKey = false;
				colvarDtSold.IsForeignKey = false;
				colvarDtSold.IsReadOnly = false;
				colvarDtSold.DefaultSetting = @"";
				colvarDtSold.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDtSold);
				
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
				
				TableSchema.TableColumn colvarDtRedeemed = new TableSchema.TableColumn(schema);
				colvarDtRedeemed.ColumnName = "dtRedeemed";
				colvarDtRedeemed.DataType = DbType.DateTime;
				colvarDtRedeemed.MaxLength = 0;
				colvarDtRedeemed.AutoIncrement = false;
				colvarDtRedeemed.IsNullable = true;
				colvarDtRedeemed.IsPrimaryKey = false;
				colvarDtRedeemed.IsForeignKey = false;
				colvarDtRedeemed.IsReadOnly = false;
				colvarDtRedeemed.DefaultSetting = @"";
				colvarDtRedeemed.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDtRedeemed);
				
				TableSchema.TableColumn colvarIpRedeemed = new TableSchema.TableColumn(schema);
				colvarIpRedeemed.ColumnName = "ipRedeemed";
				colvarIpRedeemed.DataType = DbType.AnsiString;
				colvarIpRedeemed.MaxLength = 15;
				colvarIpRedeemed.AutoIncrement = false;
				colvarIpRedeemed.IsNullable = true;
				colvarIpRedeemed.IsPrimaryKey = false;
				colvarIpRedeemed.IsForeignKey = false;
				colvarIpRedeemed.IsReadOnly = false;
				colvarIpRedeemed.DefaultSetting = @"";
				colvarIpRedeemed.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIpRedeemed);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["WillCall"].AddSchema("Inventory",schema);
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
		  
		[XmlAttribute("VcParentContext")]
		[Bindable(true)]
		public string VcParentContext 
		{
			get { return GetColumnValue<string>(Columns.VcParentContext); }
			set { SetColumnValue(Columns.VcParentContext, value); }
		}
		  
		[XmlAttribute("IParentInventoryId")]
		[Bindable(true)]
		public int IParentInventoryId 
		{
			get { return GetColumnValue<int>(Columns.IParentInventoryId); }
			set { SetColumnValue(Columns.IParentInventoryId, value); }
		}
		  
		[XmlAttribute("Code")]
		[Bindable(true)]
		public string Code 
		{
			get { return GetColumnValue<string>(Columns.Code); }
			set { SetColumnValue(Columns.Code, value); }
		}
		  
		[XmlAttribute("Description")]
		[Bindable(true)]
		public string Description 
		{
			get { return GetColumnValue<string>(Columns.Description); }
			set { SetColumnValue(Columns.Description, value); }
		}
		  
		[XmlAttribute("GSaleItemId")]
		[Bindable(true)]
		public Guid? GSaleItemId 
		{
			get { return GetColumnValue<Guid?>(Columns.GSaleItemId); }
			set { SetColumnValue(Columns.GSaleItemId, value); }
		}
		  
		[XmlAttribute("TInvoiceItemId")]
		[Bindable(true)]
		public int? TInvoiceItemId 
		{
			get { return GetColumnValue<int?>(Columns.TInvoiceItemId); }
			set { SetColumnValue(Columns.TInvoiceItemId, value); }
		}
		  
		[XmlAttribute("DtSold")]
		[Bindable(true)]
		public DateTime? DtSold 
		{
			get { return GetColumnValue<DateTime?>(Columns.DtSold); }
			set { SetColumnValue(Columns.DtSold, value); }
		}
		  
		[XmlAttribute("UserId")]
		[Bindable(true)]
		public Guid? UserId 
		{
			get { return GetColumnValue<Guid?>(Columns.UserId); }
			set { SetColumnValue(Columns.UserId, value); }
		}
		  
		[XmlAttribute("DtRedeemed")]
		[Bindable(true)]
		public DateTime? DtRedeemed 
		{
			get { return GetColumnValue<DateTime?>(Columns.DtRedeemed); }
			set { SetColumnValue(Columns.DtRedeemed, value); }
		}
		  
		[XmlAttribute("IpRedeemed")]
		[Bindable(true)]
		public string IpRedeemed 
		{
			get { return GetColumnValue<string>(Columns.IpRedeemed); }
			set { SetColumnValue(Columns.IpRedeemed, value); }
		}
		
		#endregion
		
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a AspnetUser ActiveRecord object related to this Inventory
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
		/// Returns a InvoiceItem ActiveRecord object related to this Inventory
		/// 
		/// </summary>
		private Wcss.InvoiceItem InvoiceItem
		{
			get { return Wcss.InvoiceItem.FetchByID(this.TInvoiceItemId); }
			set { SetColumnValue("tInvoiceItemId", value.Id); }
		}
        //set up an alternate mechanism to avoid a database call
		private Wcss.InvoiceItem _invoiceitemrecord = null;
		
		public Wcss.InvoiceItem InvoiceItemRecord
		{
		    get
            {
                if (_invoiceitemrecord == null)
                {
                    _invoiceitemrecord = new Wcss.InvoiceItem();
                    _invoiceitemrecord.CopyFrom(this.InvoiceItem);
                }
                return _invoiceitemrecord;
            }
            set
            {
                if(value != null && _invoiceitemrecord == null)
			        _invoiceitemrecord = new Wcss.InvoiceItem();
                
                SetColumnValue("tInvoiceItemId", value.Id);
                _invoiceitemrecord.CopyFrom(value);                
            }
		}
		
		
		#endregion
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(DateTime varDtStamp,string varVcParentContext,int varIParentInventoryId,string varCode,string varDescription,Guid? varGSaleItemId,int? varTInvoiceItemId,DateTime? varDtSold,Guid? varUserId,DateTime? varDtRedeemed,string varIpRedeemed)
		{
			Inventory item = new Inventory();
			
			item.DtStamp = varDtStamp;
			
			item.VcParentContext = varVcParentContext;
			
			item.IParentInventoryId = varIParentInventoryId;
			
			item.Code = varCode;
			
			item.Description = varDescription;
			
			item.GSaleItemId = varGSaleItemId;
			
			item.TInvoiceItemId = varTInvoiceItemId;
			
			item.DtSold = varDtSold;
			
			item.UserId = varUserId;
			
			item.DtRedeemed = varDtRedeemed;
			
			item.IpRedeemed = varIpRedeemed;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varId,DateTime varDtStamp,string varVcParentContext,int varIParentInventoryId,string varCode,string varDescription,Guid? varGSaleItemId,int? varTInvoiceItemId,DateTime? varDtSold,Guid? varUserId,DateTime? varDtRedeemed,string varIpRedeemed)
		{
			Inventory item = new Inventory();
			
				item.Id = varId;
			
				item.DtStamp = varDtStamp;
			
				item.VcParentContext = varVcParentContext;
			
				item.IParentInventoryId = varIParentInventoryId;
			
				item.Code = varCode;
			
				item.Description = varDescription;
			
				item.GSaleItemId = varGSaleItemId;
			
				item.TInvoiceItemId = varTInvoiceItemId;
			
				item.DtSold = varDtSold;
			
				item.UserId = varUserId;
			
				item.DtRedeemed = varDtRedeemed;
			
				item.IpRedeemed = varIpRedeemed;
			
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
        
        
        
        public static TableSchema.TableColumn VcParentContextColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn IParentInventoryIdColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn CodeColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn DescriptionColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn GSaleItemIdColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn TInvoiceItemIdColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn DtSoldColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn UserIdColumn
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        public static TableSchema.TableColumn DtRedeemedColumn
        {
            get { return Schema.Columns[10]; }
        }
        
        
        
        public static TableSchema.TableColumn IpRedeemedColumn
        {
            get { return Schema.Columns[11]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Id = @"Id";
			 public static string DtStamp = @"dtStamp";
			 public static string VcParentContext = @"vcParentContext";
			 public static string IParentInventoryId = @"iParentInventoryId";
			 public static string Code = @"Code";
			 public static string Description = @"Description";
			 public static string GSaleItemId = @"gSaleItemId";
			 public static string TInvoiceItemId = @"tInvoiceItemId";
			 public static string DtSold = @"dtSold";
			 public static string UserId = @"UserId";
			 public static string DtRedeemed = @"dtRedeemed";
			 public static string IpRedeemed = @"ipRedeemed";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
