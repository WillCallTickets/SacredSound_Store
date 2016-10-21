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
	/// Strongly-typed collection for the InvoiceItemPostPurchaseText class.
	/// </summary>
    [Serializable]
	public partial class InvoiceItemPostPurchaseTextCollection : ActiveList<InvoiceItemPostPurchaseText, InvoiceItemPostPurchaseTextCollection>
	{	   
		public InvoiceItemPostPurchaseTextCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>InvoiceItemPostPurchaseTextCollection</returns>
		public InvoiceItemPostPurchaseTextCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                InvoiceItemPostPurchaseText o = this[i];
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
	/// This is an ActiveRecord class which wraps the InvoiceItemPostPurchaseText table.
	/// </summary>
	[Serializable]
	public partial class InvoiceItemPostPurchaseText : ActiveRecord<InvoiceItemPostPurchaseText>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public InvoiceItemPostPurchaseText()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public InvoiceItemPostPurchaseText(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public InvoiceItemPostPurchaseText(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public InvoiceItemPostPurchaseText(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("InvoiceItemPostPurchaseText", TableType.Table, DataService.GetInstance("WillCall"));
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
				
				TableSchema.TableColumn colvarTInvoiceItemId = new TableSchema.TableColumn(schema);
				colvarTInvoiceItemId.ColumnName = "TInvoiceItemId";
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
				
				TableSchema.TableColumn colvarTPostPurchaseTextId = new TableSchema.TableColumn(schema);
				colvarTPostPurchaseTextId.ColumnName = "TPostPurchaseTextId";
				colvarTPostPurchaseTextId.DataType = DbType.Int32;
				colvarTPostPurchaseTextId.MaxLength = 0;
				colvarTPostPurchaseTextId.AutoIncrement = false;
				colvarTPostPurchaseTextId.IsNullable = false;
				colvarTPostPurchaseTextId.IsPrimaryKey = false;
				colvarTPostPurchaseTextId.IsForeignKey = false;
				colvarTPostPurchaseTextId.IsReadOnly = false;
				colvarTPostPurchaseTextId.DefaultSetting = @"";
				colvarTPostPurchaseTextId.ForeignKeyTableName = "";
				schema.Columns.Add(colvarTPostPurchaseTextId);
				
				TableSchema.TableColumn colvarPostText = new TableSchema.TableColumn(schema);
				colvarPostText.ColumnName = "PostText";
				colvarPostText.DataType = DbType.AnsiString;
				colvarPostText.MaxLength = -1;
				colvarPostText.AutoIncrement = false;
				colvarPostText.IsNullable = false;
				colvarPostText.IsPrimaryKey = false;
				colvarPostText.IsForeignKey = false;
				colvarPostText.IsReadOnly = false;
				colvarPostText.DefaultSetting = @"";
				colvarPostText.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPostText);
				
				TableSchema.TableColumn colvarIDisplayOrder = new TableSchema.TableColumn(schema);
				colvarIDisplayOrder.ColumnName = "iDisplayOrder";
				colvarIDisplayOrder.DataType = DbType.Int32;
				colvarIDisplayOrder.MaxLength = 0;
				colvarIDisplayOrder.AutoIncrement = false;
				colvarIDisplayOrder.IsNullable = false;
				colvarIDisplayOrder.IsPrimaryKey = false;
				colvarIDisplayOrder.IsForeignKey = false;
				colvarIDisplayOrder.IsReadOnly = false;
				
						colvarIDisplayOrder.DefaultSetting = @"((-1))";
				colvarIDisplayOrder.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIDisplayOrder);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["WillCall"].AddSchema("InvoiceItemPostPurchaseText",schema);
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
		  
		[XmlAttribute("TInvoiceItemId")]
		[Bindable(true)]
		public int TInvoiceItemId 
		{
			get { return GetColumnValue<int>(Columns.TInvoiceItemId); }
			set { SetColumnValue(Columns.TInvoiceItemId, value); }
		}
		  
		[XmlAttribute("TPostPurchaseTextId")]
		[Bindable(true)]
		public int TPostPurchaseTextId 
		{
			get { return GetColumnValue<int>(Columns.TPostPurchaseTextId); }
			set { SetColumnValue(Columns.TPostPurchaseTextId, value); }
		}
		  
		[XmlAttribute("PostText")]
		[Bindable(true)]
		public string PostText 
		{
			get { return GetColumnValue<string>(Columns.PostText); }
			set { SetColumnValue(Columns.PostText, value); }
		}
		  
		[XmlAttribute("IDisplayOrder")]
		[Bindable(true)]
		public int IDisplayOrder 
		{
			get { return GetColumnValue<int>(Columns.IDisplayOrder); }
			set { SetColumnValue(Columns.IDisplayOrder, value); }
		}
		
		#endregion
		
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a InvoiceItem ActiveRecord object related to this InvoiceItemPostPurchaseText
		/// 
		/// </summary>
		private Wcss.InvoiceItem InvoiceItem
		{
			get { return Wcss.InvoiceItem.FetchByID(this.TInvoiceItemId); }
			set { SetColumnValue("TInvoiceItemId", value.Id); }
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
                
                SetColumnValue("TInvoiceItemId", value.Id);
                _invoiceitemrecord.CopyFrom(value);                
            }
		}
		
		
		#endregion
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(DateTime varDtStamp,int varTInvoiceItemId,int varTPostPurchaseTextId,string varPostText,int varIDisplayOrder)
		{
			InvoiceItemPostPurchaseText item = new InvoiceItemPostPurchaseText();
			
			item.DtStamp = varDtStamp;
			
			item.TInvoiceItemId = varTInvoiceItemId;
			
			item.TPostPurchaseTextId = varTPostPurchaseTextId;
			
			item.PostText = varPostText;
			
			item.IDisplayOrder = varIDisplayOrder;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varId,DateTime varDtStamp,int varTInvoiceItemId,int varTPostPurchaseTextId,string varPostText,int varIDisplayOrder)
		{
			InvoiceItemPostPurchaseText item = new InvoiceItemPostPurchaseText();
			
				item.Id = varId;
			
				item.DtStamp = varDtStamp;
			
				item.TInvoiceItemId = varTInvoiceItemId;
			
				item.TPostPurchaseTextId = varTPostPurchaseTextId;
			
				item.PostText = varPostText;
			
				item.IDisplayOrder = varIDisplayOrder;
			
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
        
        
        
        public static TableSchema.TableColumn TInvoiceItemIdColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn TPostPurchaseTextIdColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn PostTextColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn IDisplayOrderColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Id = @"Id";
			 public static string DtStamp = @"dtStamp";
			 public static string TInvoiceItemId = @"TInvoiceItemId";
			 public static string TPostPurchaseTextId = @"TPostPurchaseTextId";
			 public static string PostText = @"PostText";
			 public static string IDisplayOrder = @"iDisplayOrder";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
