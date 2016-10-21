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
	/// Strongly-typed collection for the ShipmentBatchInvoiceShipment class.
	/// </summary>
    [Serializable]
	public partial class ShipmentBatchInvoiceShipmentCollection : ActiveList<ShipmentBatchInvoiceShipment, ShipmentBatchInvoiceShipmentCollection>
	{	   
		public ShipmentBatchInvoiceShipmentCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>ShipmentBatchInvoiceShipmentCollection</returns>
		public ShipmentBatchInvoiceShipmentCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                ShipmentBatchInvoiceShipment o = this[i];
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
	/// This is an ActiveRecord class which wraps the ShipmentBatch_InvoiceShipment table.
	/// </summary>
	[Serializable]
	public partial class ShipmentBatchInvoiceShipment : ActiveRecord<ShipmentBatchInvoiceShipment>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public ShipmentBatchInvoiceShipment()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public ShipmentBatchInvoiceShipment(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public ShipmentBatchInvoiceShipment(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public ShipmentBatchInvoiceShipment(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("ShipmentBatch_InvoiceShipment", TableType.Table, DataService.GetInstance("WillCall"));
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
				
				TableSchema.TableColumn colvarDtstamp = new TableSchema.TableColumn(schema);
				colvarDtstamp.ColumnName = "dtstamp";
				colvarDtstamp.DataType = DbType.DateTime;
				colvarDtstamp.MaxLength = 0;
				colvarDtstamp.AutoIncrement = false;
				colvarDtstamp.IsNullable = false;
				colvarDtstamp.IsPrimaryKey = false;
				colvarDtstamp.IsForeignKey = false;
				colvarDtstamp.IsReadOnly = false;
				
						colvarDtstamp.DefaultSetting = @"(getdate())";
				colvarDtstamp.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDtstamp);
				
				TableSchema.TableColumn colvarTShipmentBatchId = new TableSchema.TableColumn(schema);
				colvarTShipmentBatchId.ColumnName = "tShipmentBatchId";
				colvarTShipmentBatchId.DataType = DbType.Int32;
				colvarTShipmentBatchId.MaxLength = 0;
				colvarTShipmentBatchId.AutoIncrement = false;
				colvarTShipmentBatchId.IsNullable = false;
				colvarTShipmentBatchId.IsPrimaryKey = false;
				colvarTShipmentBatchId.IsForeignKey = true;
				colvarTShipmentBatchId.IsReadOnly = false;
				colvarTShipmentBatchId.DefaultSetting = @"";
				
					colvarTShipmentBatchId.ForeignKeyTableName = "ShipmentBatch";
				schema.Columns.Add(colvarTShipmentBatchId);
				
				TableSchema.TableColumn colvarTInvoiceShipmentId = new TableSchema.TableColumn(schema);
				colvarTInvoiceShipmentId.ColumnName = "tInvoiceShipmentId";
				colvarTInvoiceShipmentId.DataType = DbType.Int32;
				colvarTInvoiceShipmentId.MaxLength = 0;
				colvarTInvoiceShipmentId.AutoIncrement = false;
				colvarTInvoiceShipmentId.IsNullable = false;
				colvarTInvoiceShipmentId.IsPrimaryKey = false;
				colvarTInvoiceShipmentId.IsForeignKey = true;
				colvarTInvoiceShipmentId.IsReadOnly = false;
				colvarTInvoiceShipmentId.DefaultSetting = @"";
				
					colvarTInvoiceShipmentId.ForeignKeyTableName = "InvoiceShipment";
				schema.Columns.Add(colvarTInvoiceShipmentId);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["WillCall"].AddSchema("ShipmentBatch_InvoiceShipment",schema);
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
		  
		[XmlAttribute("Dtstamp")]
		[Bindable(true)]
		public DateTime Dtstamp 
		{
			get { return GetColumnValue<DateTime>(Columns.Dtstamp); }
			set { SetColumnValue(Columns.Dtstamp, value); }
		}
		  
		[XmlAttribute("TShipmentBatchId")]
		[Bindable(true)]
		public int TShipmentBatchId 
		{
			get { return GetColumnValue<int>(Columns.TShipmentBatchId); }
			set { SetColumnValue(Columns.TShipmentBatchId, value); }
		}
		  
		[XmlAttribute("TInvoiceShipmentId")]
		[Bindable(true)]
		public int TInvoiceShipmentId 
		{
			get { return GetColumnValue<int>(Columns.TInvoiceShipmentId); }
			set { SetColumnValue(Columns.TInvoiceShipmentId, value); }
		}
		
		#endregion
		
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a InvoiceShipment ActiveRecord object related to this ShipmentBatchInvoiceShipment
		/// 
		/// </summary>
		private Wcss.InvoiceShipment InvoiceShipment
		{
			get { return Wcss.InvoiceShipment.FetchByID(this.TInvoiceShipmentId); }
			set { SetColumnValue("tInvoiceShipmentId", value.Id); }
		}
        //set up an alternate mechanism to avoid a database call
		private Wcss.InvoiceShipment _invoiceshipmentrecord = null;
		
		public Wcss.InvoiceShipment InvoiceShipmentRecord
		{
		    get
            {
                if (_invoiceshipmentrecord == null)
                {
                    _invoiceshipmentrecord = new Wcss.InvoiceShipment();
                    _invoiceshipmentrecord.CopyFrom(this.InvoiceShipment);
                }
                return _invoiceshipmentrecord;
            }
            set
            {
                if(value != null && _invoiceshipmentrecord == null)
			        _invoiceshipmentrecord = new Wcss.InvoiceShipment();
                
                SetColumnValue("tInvoiceShipmentId", value.Id);
                _invoiceshipmentrecord.CopyFrom(value);                
            }
		}
		
		
		/// <summary>
		/// Returns a ShipmentBatch ActiveRecord object related to this ShipmentBatchInvoiceShipment
		/// 
		/// </summary>
		private Wcss.ShipmentBatch ShipmentBatch
		{
			get { return Wcss.ShipmentBatch.FetchByID(this.TShipmentBatchId); }
			set { SetColumnValue("tShipmentBatchId", value.Id); }
		}
        //set up an alternate mechanism to avoid a database call
		private Wcss.ShipmentBatch _shipmentbatchrecord = null;
		
		public Wcss.ShipmentBatch ShipmentBatchRecord
		{
		    get
            {
                if (_shipmentbatchrecord == null)
                {
                    _shipmentbatchrecord = new Wcss.ShipmentBatch();
                    _shipmentbatchrecord.CopyFrom(this.ShipmentBatch);
                }
                return _shipmentbatchrecord;
            }
            set
            {
                if(value != null && _shipmentbatchrecord == null)
			        _shipmentbatchrecord = new Wcss.ShipmentBatch();
                
                SetColumnValue("tShipmentBatchId", value.Id);
                _shipmentbatchrecord.CopyFrom(value);                
            }
		}
		
		
		#endregion
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(DateTime varDtstamp,int varTShipmentBatchId,int varTInvoiceShipmentId)
		{
			ShipmentBatchInvoiceShipment item = new ShipmentBatchInvoiceShipment();
			
			item.Dtstamp = varDtstamp;
			
			item.TShipmentBatchId = varTShipmentBatchId;
			
			item.TInvoiceShipmentId = varTInvoiceShipmentId;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varId,DateTime varDtstamp,int varTShipmentBatchId,int varTInvoiceShipmentId)
		{
			ShipmentBatchInvoiceShipment item = new ShipmentBatchInvoiceShipment();
			
				item.Id = varId;
			
				item.Dtstamp = varDtstamp;
			
				item.TShipmentBatchId = varTShipmentBatchId;
			
				item.TInvoiceShipmentId = varTInvoiceShipmentId;
			
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
        
        
        
        public static TableSchema.TableColumn DtstampColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn TShipmentBatchIdColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn TInvoiceShipmentIdColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Id = @"Id";
			 public static string Dtstamp = @"dtstamp";
			 public static string TShipmentBatchId = @"tShipmentBatchId";
			 public static string TInvoiceShipmentId = @"tInvoiceShipmentId";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
