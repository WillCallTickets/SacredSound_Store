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
	/// Strongly-typed collection for the InvoiceShipmentItem class.
	/// </summary>
    [Serializable]
	public partial class InvoiceShipmentItemCollection : ActiveList<InvoiceShipmentItem, InvoiceShipmentItemCollection>
	{	   
		public InvoiceShipmentItemCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>InvoiceShipmentItemCollection</returns>
		public InvoiceShipmentItemCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                InvoiceShipmentItem o = this[i];
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
	/// This is an ActiveRecord class which wraps the InvoiceShipmentItem table.
	/// </summary>
	[Serializable]
	public partial class InvoiceShipmentItem : ActiveRecord<InvoiceShipmentItem>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public InvoiceShipmentItem()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public InvoiceShipmentItem(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public InvoiceShipmentItem(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public InvoiceShipmentItem(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("InvoiceShipmentItem", TableType.Table, DataService.GetInstance("WillCall"));
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
				colvarDtStamp.IsNullable = true;
				colvarDtStamp.IsPrimaryKey = false;
				colvarDtStamp.IsForeignKey = false;
				colvarDtStamp.IsReadOnly = false;
				
						colvarDtStamp.DefaultSetting = @"(getdate())";
				colvarDtStamp.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDtStamp);
				
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
				
				TableSchema.TableColumn colvarTInvoiceItemId = new TableSchema.TableColumn(schema);
				colvarTInvoiceItemId.ColumnName = "tInvoiceItemId";
				colvarTInvoiceItemId.DataType = DbType.Int32;
				colvarTInvoiceItemId.MaxLength = 0;
				colvarTInvoiceItemId.AutoIncrement = false;
				colvarTInvoiceItemId.IsNullable = false;
				colvarTInvoiceItemId.IsPrimaryKey = false;
				colvarTInvoiceItemId.IsForeignKey = true;
				colvarTInvoiceItemId.IsReadOnly = false;
				colvarTInvoiceItemId.DefaultSetting = @"";
				
					colvarTInvoiceItemId.ForeignKeyTableName = "InvoiceItem";
				schema.Columns.Add(colvarTInvoiceItemId);
				
				TableSchema.TableColumn colvarIQuantity = new TableSchema.TableColumn(schema);
				colvarIQuantity.ColumnName = "iQuantity";
				colvarIQuantity.DataType = DbType.Int32;
				colvarIQuantity.MaxLength = 0;
				colvarIQuantity.AutoIncrement = false;
				colvarIQuantity.IsNullable = false;
				colvarIQuantity.IsPrimaryKey = false;
				colvarIQuantity.IsForeignKey = false;
				colvarIQuantity.IsReadOnly = false;
				colvarIQuantity.DefaultSetting = @"";
				colvarIQuantity.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIQuantity);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["WillCall"].AddSchema("InvoiceShipmentItem",schema);
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
		public DateTime? DtStamp 
		{
			get { return GetColumnValue<DateTime?>(Columns.DtStamp); }
			set { SetColumnValue(Columns.DtStamp, value); }
		}
		  
		[XmlAttribute("TInvoiceShipmentId")]
		[Bindable(true)]
		public int TInvoiceShipmentId 
		{
			get { return GetColumnValue<int>(Columns.TInvoiceShipmentId); }
			set { SetColumnValue(Columns.TInvoiceShipmentId, value); }
		}
		  
		[XmlAttribute("TInvoiceItemId")]
		[Bindable(true)]
		public int TInvoiceItemId 
		{
			get { return GetColumnValue<int>(Columns.TInvoiceItemId); }
			set { SetColumnValue(Columns.TInvoiceItemId, value); }
		}
		  
		[XmlAttribute("IQuantity")]
		[Bindable(true)]
		public int IQuantity 
		{
			get { return GetColumnValue<int>(Columns.IQuantity); }
			set { SetColumnValue(Columns.IQuantity, value); }
		}
		
		#endregion
		
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a InvoiceItem ActiveRecord object related to this InvoiceShipmentItem
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
		
		
		/// <summary>
		/// Returns a InvoiceShipment ActiveRecord object related to this InvoiceShipmentItem
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
		
		
		#endregion
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(DateTime? varDtStamp,int varTInvoiceShipmentId,int varTInvoiceItemId,int varIQuantity)
		{
			InvoiceShipmentItem item = new InvoiceShipmentItem();
			
			item.DtStamp = varDtStamp;
			
			item.TInvoiceShipmentId = varTInvoiceShipmentId;
			
			item.TInvoiceItemId = varTInvoiceItemId;
			
			item.IQuantity = varIQuantity;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varId,DateTime? varDtStamp,int varTInvoiceShipmentId,int varTInvoiceItemId,int varIQuantity)
		{
			InvoiceShipmentItem item = new InvoiceShipmentItem();
			
				item.Id = varId;
			
				item.DtStamp = varDtStamp;
			
				item.TInvoiceShipmentId = varTInvoiceShipmentId;
			
				item.TInvoiceItemId = varTInvoiceItemId;
			
				item.IQuantity = varIQuantity;
			
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
        
        
        
        public static TableSchema.TableColumn TInvoiceShipmentIdColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn TInvoiceItemIdColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn IQuantityColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Id = @"Id";
			 public static string DtStamp = @"dtStamp";
			 public static string TInvoiceShipmentId = @"tInvoiceShipmentId";
			 public static string TInvoiceItemId = @"tInvoiceItemId";
			 public static string IQuantity = @"iQuantity";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
