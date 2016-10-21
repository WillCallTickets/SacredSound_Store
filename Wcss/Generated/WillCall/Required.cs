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
	/// Strongly-typed collection for the Required class.
	/// </summary>
    [Serializable]
	public partial class RequiredCollection : ActiveList<Required, RequiredCollection>
	{	   
		public RequiredCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>RequiredCollection</returns>
		public RequiredCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                Required o = this[i];
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
	/// This is an ActiveRecord class which wraps the Required table.
	/// </summary>
	[Serializable]
	public partial class Required : ActiveRecord<Required>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public Required()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public Required(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public Required(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public Required(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("Required", TableType.Table, DataService.GetInstance("WillCall"));
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
				
				TableSchema.TableColumn colvarBActive = new TableSchema.TableColumn(schema);
				colvarBActive.ColumnName = "bActive";
				colvarBActive.DataType = DbType.Boolean;
				colvarBActive.MaxLength = 0;
				colvarBActive.AutoIncrement = false;
				colvarBActive.IsNullable = false;
				colvarBActive.IsPrimaryKey = false;
				colvarBActive.IsForeignKey = false;
				colvarBActive.IsReadOnly = false;
				
						colvarBActive.DefaultSetting = @"((1))";
				colvarBActive.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBActive);
				
				TableSchema.TableColumn colvarBExclusive = new TableSchema.TableColumn(schema);
				colvarBExclusive.ColumnName = "bExclusive";
				colvarBExclusive.DataType = DbType.Boolean;
				colvarBExclusive.MaxLength = 0;
				colvarBExclusive.AutoIncrement = false;
				colvarBExclusive.IsNullable = false;
				colvarBExclusive.IsPrimaryKey = false;
				colvarBExclusive.IsForeignKey = false;
				colvarBExclusive.IsReadOnly = false;
				
						colvarBExclusive.DefaultSetting = @"((0))";
				colvarBExclusive.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBExclusive);
				
				TableSchema.TableColumn colvarDtStart = new TableSchema.TableColumn(schema);
				colvarDtStart.ColumnName = "dtStart";
				colvarDtStart.DataType = DbType.DateTime;
				colvarDtStart.MaxLength = 0;
				colvarDtStart.AutoIncrement = false;
				colvarDtStart.IsNullable = true;
				colvarDtStart.IsPrimaryKey = false;
				colvarDtStart.IsForeignKey = false;
				colvarDtStart.IsReadOnly = false;
				colvarDtStart.DefaultSetting = @"";
				colvarDtStart.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDtStart);
				
				TableSchema.TableColumn colvarDtEnd = new TableSchema.TableColumn(schema);
				colvarDtEnd.ColumnName = "dtEnd";
				colvarDtEnd.DataType = DbType.DateTime;
				colvarDtEnd.MaxLength = 0;
				colvarDtEnd.AutoIncrement = false;
				colvarDtEnd.IsNullable = true;
				colvarDtEnd.IsPrimaryKey = false;
				colvarDtEnd.IsForeignKey = false;
				colvarDtEnd.IsReadOnly = false;
				colvarDtEnd.DefaultSetting = @"";
				colvarDtEnd.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDtEnd);
				
				TableSchema.TableColumn colvarVcRequiredContext = new TableSchema.TableColumn(schema);
				colvarVcRequiredContext.ColumnName = "vcRequiredContext";
				colvarVcRequiredContext.DataType = DbType.AnsiString;
				colvarVcRequiredContext.MaxLength = 50;
				colvarVcRequiredContext.AutoIncrement = false;
				colvarVcRequiredContext.IsNullable = false;
				colvarVcRequiredContext.IsPrimaryKey = false;
				colvarVcRequiredContext.IsForeignKey = false;
				colvarVcRequiredContext.IsReadOnly = false;
				colvarVcRequiredContext.DefaultSetting = @"";
				colvarVcRequiredContext.ForeignKeyTableName = "";
				schema.Columns.Add(colvarVcRequiredContext);
				
				TableSchema.TableColumn colvarVcIdx = new TableSchema.TableColumn(schema);
				colvarVcIdx.ColumnName = "vcIdx";
				colvarVcIdx.DataType = DbType.AnsiString;
				colvarVcIdx.MaxLength = 100;
				colvarVcIdx.AutoIncrement = false;
				colvarVcIdx.IsNullable = true;
				colvarVcIdx.IsPrimaryKey = false;
				colvarVcIdx.IsForeignKey = false;
				colvarVcIdx.IsReadOnly = false;
				colvarVcIdx.DefaultSetting = @"";
				colvarVcIdx.ForeignKeyTableName = "";
				schema.Columns.Add(colvarVcIdx);
				
				TableSchema.TableColumn colvarIRequiredQty = new TableSchema.TableColumn(schema);
				colvarIRequiredQty.ColumnName = "iRequiredQty";
				colvarIRequiredQty.DataType = DbType.Int32;
				colvarIRequiredQty.MaxLength = 0;
				colvarIRequiredQty.AutoIncrement = false;
				colvarIRequiredQty.IsNullable = false;
				colvarIRequiredQty.IsPrimaryKey = false;
				colvarIRequiredQty.IsForeignKey = false;
				colvarIRequiredQty.IsReadOnly = false;
				
						colvarIRequiredQty.DefaultSetting = @"((1))";
				colvarIRequiredQty.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIRequiredQty);
				
				TableSchema.TableColumn colvarMMinAmount = new TableSchema.TableColumn(schema);
				colvarMMinAmount.ColumnName = "mMinAmount";
				colvarMMinAmount.DataType = DbType.Currency;
				colvarMMinAmount.MaxLength = 0;
				colvarMMinAmount.AutoIncrement = false;
				colvarMMinAmount.IsNullable = false;
				colvarMMinAmount.IsPrimaryKey = false;
				colvarMMinAmount.IsForeignKey = false;
				colvarMMinAmount.IsReadOnly = false;
				
						colvarMMinAmount.DefaultSetting = @"((0.0))";
				colvarMMinAmount.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMMinAmount);
				
				TableSchema.TableColumn colvarDescription = new TableSchema.TableColumn(schema);
				colvarDescription.ColumnName = "Description";
				colvarDescription.DataType = DbType.AnsiString;
				colvarDescription.MaxLength = 500;
				colvarDescription.AutoIncrement = false;
				colvarDescription.IsNullable = true;
				colvarDescription.IsPrimaryKey = false;
				colvarDescription.IsForeignKey = false;
				colvarDescription.IsReadOnly = false;
				colvarDescription.DefaultSetting = @"";
				colvarDescription.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDescription);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["WillCall"].AddSchema("Required",schema);
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
		  
		[XmlAttribute("BActive")]
		[Bindable(true)]
		public bool BActive 
		{
			get { return GetColumnValue<bool>(Columns.BActive); }
			set { SetColumnValue(Columns.BActive, value); }
		}
		  
		[XmlAttribute("BExclusive")]
		[Bindable(true)]
		public bool BExclusive 
		{
			get { return GetColumnValue<bool>(Columns.BExclusive); }
			set { SetColumnValue(Columns.BExclusive, value); }
		}
		  
		[XmlAttribute("DtStart")]
		[Bindable(true)]
		public DateTime? DtStart 
		{
			get { return GetColumnValue<DateTime?>(Columns.DtStart); }
			set { SetColumnValue(Columns.DtStart, value); }
		}
		  
		[XmlAttribute("DtEnd")]
		[Bindable(true)]
		public DateTime? DtEnd 
		{
			get { return GetColumnValue<DateTime?>(Columns.DtEnd); }
			set { SetColumnValue(Columns.DtEnd, value); }
		}
		  
		[XmlAttribute("VcRequiredContext")]
		[Bindable(true)]
		public string VcRequiredContext 
		{
			get { return GetColumnValue<string>(Columns.VcRequiredContext); }
			set { SetColumnValue(Columns.VcRequiredContext, value); }
		}
		  
		[XmlAttribute("VcIdx")]
		[Bindable(true)]
		public string VcIdx 
		{
			get { return GetColumnValue<string>(Columns.VcIdx); }
			set { SetColumnValue(Columns.VcIdx, value); }
		}
		  
		[XmlAttribute("IRequiredQty")]
		[Bindable(true)]
		public int IRequiredQty 
		{
			get { return GetColumnValue<int>(Columns.IRequiredQty); }
			set { SetColumnValue(Columns.IRequiredQty, value); }
		}
		  
		[XmlAttribute("MMinAmount")]
		[Bindable(true)]
		public decimal MMinAmount 
		{
			get { return GetColumnValue<decimal>(Columns.MMinAmount); }
			set { SetColumnValue(Columns.MMinAmount, value); }
		}
		  
		[XmlAttribute("Description")]
		[Bindable(true)]
		public string Description 
		{
			get { return GetColumnValue<string>(Columns.Description); }
			set { SetColumnValue(Columns.Description, value); }
		}
		
		#endregion
		
		
		#region PrimaryKey Methods		
		
        protected override void SetPrimaryKey(object oValue)
        {
            base.SetPrimaryKey(oValue);
            
            SetPKValues();
        }
        
		
		private Wcss.RequiredInvoiceFeeCollection colRequiredInvoiceFeeRecords;
		public Wcss.RequiredInvoiceFeeCollection RequiredInvoiceFeeRecords()
		{
			if(colRequiredInvoiceFeeRecords == null)
			{
				colRequiredInvoiceFeeRecords = new Wcss.RequiredInvoiceFeeCollection().Where(RequiredInvoiceFee.Columns.TRequiredId, Id).Load();
				colRequiredInvoiceFeeRecords.ListChanged += new ListChangedEventHandler(colRequiredInvoiceFeeRecords_ListChanged);
			}
			return colRequiredInvoiceFeeRecords;
		}
				
		void colRequiredInvoiceFeeRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colRequiredInvoiceFeeRecords[e.NewIndex].TRequiredId = Id;
				colRequiredInvoiceFeeRecords.ListChanged += new ListChangedEventHandler(colRequiredInvoiceFeeRecords_ListChanged);
            }
		}
		private Wcss.RequiredMerchCollection colRequiredMerchRecords;
		public Wcss.RequiredMerchCollection RequiredMerchRecords()
		{
			if(colRequiredMerchRecords == null)
			{
				colRequiredMerchRecords = new Wcss.RequiredMerchCollection().Where(RequiredMerch.Columns.TRequiredId, Id).Load();
				colRequiredMerchRecords.ListChanged += new ListChangedEventHandler(colRequiredMerchRecords_ListChanged);
			}
			return colRequiredMerchRecords;
		}
				
		void colRequiredMerchRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colRequiredMerchRecords[e.NewIndex].TRequiredId = Id;
				colRequiredMerchRecords.ListChanged += new ListChangedEventHandler(colRequiredMerchRecords_ListChanged);
            }
		}
		private Wcss.RequiredShowTicketPastPurchaseCollection colRequiredShowTicketPastPurchaseRecords;
		public Wcss.RequiredShowTicketPastPurchaseCollection RequiredShowTicketPastPurchaseRecords()
		{
			if(colRequiredShowTicketPastPurchaseRecords == null)
			{
				colRequiredShowTicketPastPurchaseRecords = new Wcss.RequiredShowTicketPastPurchaseCollection().Where(RequiredShowTicketPastPurchase.Columns.TRequiredId, Id).Load();
				colRequiredShowTicketPastPurchaseRecords.ListChanged += new ListChangedEventHandler(colRequiredShowTicketPastPurchaseRecords_ListChanged);
			}
			return colRequiredShowTicketPastPurchaseRecords;
		}
				
		void colRequiredShowTicketPastPurchaseRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colRequiredShowTicketPastPurchaseRecords[e.NewIndex].TRequiredId = Id;
				colRequiredShowTicketPastPurchaseRecords.ListChanged += new ListChangedEventHandler(colRequiredShowTicketPastPurchaseRecords_ListChanged);
            }
		}
		#endregion
		
			
		
		//no foreign key tables defined (0)
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(DateTime varDtStamp,bool varBActive,bool varBExclusive,DateTime? varDtStart,DateTime? varDtEnd,string varVcRequiredContext,string varVcIdx,int varIRequiredQty,decimal varMMinAmount,string varDescription)
		{
			Required item = new Required();
			
			item.DtStamp = varDtStamp;
			
			item.BActive = varBActive;
			
			item.BExclusive = varBExclusive;
			
			item.DtStart = varDtStart;
			
			item.DtEnd = varDtEnd;
			
			item.VcRequiredContext = varVcRequiredContext;
			
			item.VcIdx = varVcIdx;
			
			item.IRequiredQty = varIRequiredQty;
			
			item.MMinAmount = varMMinAmount;
			
			item.Description = varDescription;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varId,DateTime varDtStamp,bool varBActive,bool varBExclusive,DateTime? varDtStart,DateTime? varDtEnd,string varVcRequiredContext,string varVcIdx,int varIRequiredQty,decimal varMMinAmount,string varDescription)
		{
			Required item = new Required();
			
				item.Id = varId;
			
				item.DtStamp = varDtStamp;
			
				item.BActive = varBActive;
			
				item.BExclusive = varBExclusive;
			
				item.DtStart = varDtStart;
			
				item.DtEnd = varDtEnd;
			
				item.VcRequiredContext = varVcRequiredContext;
			
				item.VcIdx = varVcIdx;
			
				item.IRequiredQty = varIRequiredQty;
			
				item.MMinAmount = varMMinAmount;
			
				item.Description = varDescription;
			
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
        
        
        
        public static TableSchema.TableColumn BActiveColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn BExclusiveColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn DtStartColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn DtEndColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn VcRequiredContextColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn VcIdxColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn IRequiredQtyColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn MMinAmountColumn
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        public static TableSchema.TableColumn DescriptionColumn
        {
            get { return Schema.Columns[10]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Id = @"Id";
			 public static string DtStamp = @"dtStamp";
			 public static string BActive = @"bActive";
			 public static string BExclusive = @"bExclusive";
			 public static string DtStart = @"dtStart";
			 public static string DtEnd = @"dtEnd";
			 public static string VcRequiredContext = @"vcRequiredContext";
			 public static string VcIdx = @"vcIdx";
			 public static string IRequiredQty = @"iRequiredQty";
			 public static string MMinAmount = @"mMinAmount";
			 public static string Description = @"Description";
						
		}
		#endregion
		
		#region Update PK Collections
		
        public void SetPKValues()
        {
                if (colRequiredInvoiceFeeRecords != null)
                {
                    foreach (Wcss.RequiredInvoiceFee item in colRequiredInvoiceFeeRecords)
                    {
                        if (item.TRequiredId != Id)
                        {
                            item.TRequiredId = Id;
                        }
                    }
               }
		
                if (colRequiredMerchRecords != null)
                {
                    foreach (Wcss.RequiredMerch item in colRequiredMerchRecords)
                    {
                        if (item.TRequiredId != Id)
                        {
                            item.TRequiredId = Id;
                        }
                    }
               }
		
                if (colRequiredShowTicketPastPurchaseRecords != null)
                {
                    foreach (Wcss.RequiredShowTicketPastPurchase item in colRequiredShowTicketPastPurchaseRecords)
                    {
                        if (item.TRequiredId != Id)
                        {
                            item.TRequiredId = Id;
                        }
                    }
               }
		}
        #endregion
    
        #region Deep Save
		
        public void DeepSave()
        {
            Save();
            
                if (colRequiredInvoiceFeeRecords != null)
                {
                    colRequiredInvoiceFeeRecords.SaveAll();
               }
		
                if (colRequiredMerchRecords != null)
                {
                    colRequiredMerchRecords.SaveAll();
               }
		
                if (colRequiredShowTicketPastPurchaseRecords != null)
                {
                    colRequiredShowTicketPastPurchaseRecords.SaveAll();
               }
		}
        #endregion
	}
}
