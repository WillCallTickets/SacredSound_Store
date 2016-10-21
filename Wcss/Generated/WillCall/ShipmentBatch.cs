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
	/// Strongly-typed collection for the ShipmentBatch class.
	/// </summary>
    [Serializable]
	public partial class ShipmentBatchCollection : ActiveList<ShipmentBatch, ShipmentBatchCollection>
	{	   
		public ShipmentBatchCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>ShipmentBatchCollection</returns>
		public ShipmentBatchCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                ShipmentBatch o = this[i];
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
	/// This is an ActiveRecord class which wraps the ShipmentBatch table.
	/// </summary>
	[Serializable]
	public partial class ShipmentBatch : ActiveRecord<ShipmentBatch>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public ShipmentBatch()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public ShipmentBatch(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public ShipmentBatch(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public ShipmentBatch(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("ShipmentBatch", TableType.Table, DataService.GetInstance("WillCall"));
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
				
				TableSchema.TableColumn colvarBatchId = new TableSchema.TableColumn(schema);
				colvarBatchId.ColumnName = "BatchId";
				colvarBatchId.DataType = DbType.AnsiString;
				colvarBatchId.MaxLength = 50;
				colvarBatchId.AutoIncrement = false;
				colvarBatchId.IsNullable = false;
				colvarBatchId.IsPrimaryKey = false;
				colvarBatchId.IsForeignKey = false;
				colvarBatchId.IsReadOnly = false;
				colvarBatchId.DefaultSetting = @"";
				colvarBatchId.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBatchId);
				
				TableSchema.TableColumn colvarName = new TableSchema.TableColumn(schema);
				colvarName.ColumnName = "Name";
				colvarName.DataType = DbType.AnsiString;
				colvarName.MaxLength = 256;
				colvarName.AutoIncrement = false;
				colvarName.IsNullable = false;
				colvarName.IsPrimaryKey = false;
				colvarName.IsForeignKey = false;
				colvarName.IsReadOnly = false;
				colvarName.DefaultSetting = @"";
				colvarName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarName);
				
				TableSchema.TableColumn colvarDescription = new TableSchema.TableColumn(schema);
				colvarDescription.ColumnName = "Description";
				colvarDescription.DataType = DbType.AnsiString;
				colvarDescription.MaxLength = 1000;
				colvarDescription.AutoIncrement = false;
				colvarDescription.IsNullable = true;
				colvarDescription.IsPrimaryKey = false;
				colvarDescription.IsForeignKey = false;
				colvarDescription.IsReadOnly = false;
				colvarDescription.DefaultSetting = @"";
				colvarDescription.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDescription);
				
				TableSchema.TableColumn colvarEventId = new TableSchema.TableColumn(schema);
				colvarEventId.ColumnName = "EventId";
				colvarEventId.DataType = DbType.Int32;
				colvarEventId.MaxLength = 0;
				colvarEventId.AutoIncrement = false;
				colvarEventId.IsNullable = true;
				colvarEventId.IsPrimaryKey = false;
				colvarEventId.IsForeignKey = false;
				colvarEventId.IsReadOnly = false;
				colvarEventId.DefaultSetting = @"";
				colvarEventId.ForeignKeyTableName = "";
				schema.Columns.Add(colvarEventId);
				
				TableSchema.TableColumn colvarCsvEventTix = new TableSchema.TableColumn(schema);
				colvarCsvEventTix.ColumnName = "csvEventTix";
				colvarCsvEventTix.DataType = DbType.AnsiString;
				colvarCsvEventTix.MaxLength = 1000;
				colvarCsvEventTix.AutoIncrement = false;
				colvarCsvEventTix.IsNullable = true;
				colvarCsvEventTix.IsPrimaryKey = false;
				colvarCsvEventTix.IsForeignKey = false;
				colvarCsvEventTix.IsReadOnly = false;
				colvarCsvEventTix.DefaultSetting = @"";
				colvarCsvEventTix.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCsvEventTix);
				
				TableSchema.TableColumn colvarCsvOtherTix = new TableSchema.TableColumn(schema);
				colvarCsvOtherTix.ColumnName = "csvOtherTix";
				colvarCsvOtherTix.DataType = DbType.AnsiString;
				colvarCsvOtherTix.MaxLength = 1000;
				colvarCsvOtherTix.AutoIncrement = false;
				colvarCsvOtherTix.IsNullable = true;
				colvarCsvOtherTix.IsPrimaryKey = false;
				colvarCsvOtherTix.IsForeignKey = false;
				colvarCsvOtherTix.IsReadOnly = false;
				colvarCsvOtherTix.DefaultSetting = @"";
				colvarCsvOtherTix.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCsvOtherTix);
				
				TableSchema.TableColumn colvarCsvMethods = new TableSchema.TableColumn(schema);
				colvarCsvMethods.ColumnName = "csvMethods";
				colvarCsvMethods.DataType = DbType.AnsiString;
				colvarCsvMethods.MaxLength = 1000;
				colvarCsvMethods.AutoIncrement = false;
				colvarCsvMethods.IsNullable = true;
				colvarCsvMethods.IsPrimaryKey = false;
				colvarCsvMethods.IsForeignKey = false;
				colvarCsvMethods.IsReadOnly = false;
				colvarCsvMethods.DefaultSetting = @"";
				colvarCsvMethods.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCsvMethods);
				
				TableSchema.TableColumn colvarDtEstShipDate = new TableSchema.TableColumn(schema);
				colvarDtEstShipDate.ColumnName = "dtEstShipDate";
				colvarDtEstShipDate.DataType = DbType.DateTime;
				colvarDtEstShipDate.MaxLength = 0;
				colvarDtEstShipDate.AutoIncrement = false;
				colvarDtEstShipDate.IsNullable = true;
				colvarDtEstShipDate.IsPrimaryKey = false;
				colvarDtEstShipDate.IsForeignKey = false;
				colvarDtEstShipDate.IsReadOnly = false;
				colvarDtEstShipDate.DefaultSetting = @"";
				colvarDtEstShipDate.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDtEstShipDate);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["WillCall"].AddSchema("ShipmentBatch",schema);
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
		  
		[XmlAttribute("BatchId")]
		[Bindable(true)]
		public string BatchId 
		{
			get { return GetColumnValue<string>(Columns.BatchId); }
			set { SetColumnValue(Columns.BatchId, value); }
		}
		  
		[XmlAttribute("Name")]
		[Bindable(true)]
		public string Name 
		{
			get { return GetColumnValue<string>(Columns.Name); }
			set { SetColumnValue(Columns.Name, value); }
		}
		  
		[XmlAttribute("Description")]
		[Bindable(true)]
		public string Description 
		{
			get { return GetColumnValue<string>(Columns.Description); }
			set { SetColumnValue(Columns.Description, value); }
		}
		  
		[XmlAttribute("EventId")]
		[Bindable(true)]
		public int? EventId 
		{
			get { return GetColumnValue<int?>(Columns.EventId); }
			set { SetColumnValue(Columns.EventId, value); }
		}
		  
		[XmlAttribute("CsvEventTix")]
		[Bindable(true)]
		public string CsvEventTix 
		{
			get { return GetColumnValue<string>(Columns.CsvEventTix); }
			set { SetColumnValue(Columns.CsvEventTix, value); }
		}
		  
		[XmlAttribute("CsvOtherTix")]
		[Bindable(true)]
		public string CsvOtherTix 
		{
			get { return GetColumnValue<string>(Columns.CsvOtherTix); }
			set { SetColumnValue(Columns.CsvOtherTix, value); }
		}
		  
		[XmlAttribute("CsvMethods")]
		[Bindable(true)]
		public string CsvMethods 
		{
			get { return GetColumnValue<string>(Columns.CsvMethods); }
			set { SetColumnValue(Columns.CsvMethods, value); }
		}
		  
		[XmlAttribute("DtEstShipDate")]
		[Bindable(true)]
		public DateTime? DtEstShipDate 
		{
			get { return GetColumnValue<DateTime?>(Columns.DtEstShipDate); }
			set { SetColumnValue(Columns.DtEstShipDate, value); }
		}
		
		#endregion
		
		
		#region PrimaryKey Methods		
		
        protected override void SetPrimaryKey(object oValue)
        {
            base.SetPrimaryKey(oValue);
            
            SetPKValues();
        }
        
		
		private Wcss.ShipmentBatchInvoiceShipmentCollection colShipmentBatchInvoiceShipmentRecords;
		public Wcss.ShipmentBatchInvoiceShipmentCollection ShipmentBatchInvoiceShipmentRecords()
		{
			if(colShipmentBatchInvoiceShipmentRecords == null)
			{
				colShipmentBatchInvoiceShipmentRecords = new Wcss.ShipmentBatchInvoiceShipmentCollection().Where(ShipmentBatchInvoiceShipment.Columns.TShipmentBatchId, Id).Load();
				colShipmentBatchInvoiceShipmentRecords.ListChanged += new ListChangedEventHandler(colShipmentBatchInvoiceShipmentRecords_ListChanged);
			}
			return colShipmentBatchInvoiceShipmentRecords;
		}
				
		void colShipmentBatchInvoiceShipmentRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colShipmentBatchInvoiceShipmentRecords[e.NewIndex].TShipmentBatchId = Id;
				colShipmentBatchInvoiceShipmentRecords.ListChanged += new ListChangedEventHandler(colShipmentBatchInvoiceShipmentRecords_ListChanged);
            }
		}
		#endregion
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a AspnetApplication ActiveRecord object related to this ShipmentBatch
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
		
		
		#endregion
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(DateTime varDtStamp,Guid varApplicationId,string varBatchId,string varName,string varDescription,int? varEventId,string varCsvEventTix,string varCsvOtherTix,string varCsvMethods,DateTime? varDtEstShipDate)
		{
			ShipmentBatch item = new ShipmentBatch();
			
			item.DtStamp = varDtStamp;
			
			item.ApplicationId = varApplicationId;
			
			item.BatchId = varBatchId;
			
			item.Name = varName;
			
			item.Description = varDescription;
			
			item.EventId = varEventId;
			
			item.CsvEventTix = varCsvEventTix;
			
			item.CsvOtherTix = varCsvOtherTix;
			
			item.CsvMethods = varCsvMethods;
			
			item.DtEstShipDate = varDtEstShipDate;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varId,DateTime varDtStamp,Guid varApplicationId,string varBatchId,string varName,string varDescription,int? varEventId,string varCsvEventTix,string varCsvOtherTix,string varCsvMethods,DateTime? varDtEstShipDate)
		{
			ShipmentBatch item = new ShipmentBatch();
			
				item.Id = varId;
			
				item.DtStamp = varDtStamp;
			
				item.ApplicationId = varApplicationId;
			
				item.BatchId = varBatchId;
			
				item.Name = varName;
			
				item.Description = varDescription;
			
				item.EventId = varEventId;
			
				item.CsvEventTix = varCsvEventTix;
			
				item.CsvOtherTix = varCsvOtherTix;
			
				item.CsvMethods = varCsvMethods;
			
				item.DtEstShipDate = varDtEstShipDate;
			
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
        
        
        
        public static TableSchema.TableColumn BatchIdColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn NameColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn DescriptionColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn EventIdColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn CsvEventTixColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn CsvOtherTixColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn CsvMethodsColumn
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        public static TableSchema.TableColumn DtEstShipDateColumn
        {
            get { return Schema.Columns[10]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Id = @"Id";
			 public static string DtStamp = @"dtStamp";
			 public static string ApplicationId = @"ApplicationId";
			 public static string BatchId = @"BatchId";
			 public static string Name = @"Name";
			 public static string Description = @"Description";
			 public static string EventId = @"EventId";
			 public static string CsvEventTix = @"csvEventTix";
			 public static string CsvOtherTix = @"csvOtherTix";
			 public static string CsvMethods = @"csvMethods";
			 public static string DtEstShipDate = @"dtEstShipDate";
						
		}
		#endregion
		
		#region Update PK Collections
		
        public void SetPKValues()
        {
                if (colShipmentBatchInvoiceShipmentRecords != null)
                {
                    foreach (Wcss.ShipmentBatchInvoiceShipment item in colShipmentBatchInvoiceShipmentRecords)
                    {
                        if (item.TShipmentBatchId != Id)
                        {
                            item.TShipmentBatchId = Id;
                        }
                    }
               }
		}
        #endregion
    
        #region Deep Save
		
        public void DeepSave()
        {
            Save();
            
                if (colShipmentBatchInvoiceShipmentRecords != null)
                {
                    colShipmentBatchInvoiceShipmentRecords.SaveAll();
               }
		}
        #endregion
	}
}
