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
	/// Strongly-typed collection for the RequiredInvoiceFee class.
	/// </summary>
    [Serializable]
	public partial class RequiredInvoiceFeeCollection : ActiveList<RequiredInvoiceFee, RequiredInvoiceFeeCollection>
	{	   
		public RequiredInvoiceFeeCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>RequiredInvoiceFeeCollection</returns>
		public RequiredInvoiceFeeCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                RequiredInvoiceFee o = this[i];
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
	/// This is an ActiveRecord class which wraps the Required_InvoiceFee table.
	/// </summary>
	[Serializable]
	public partial class RequiredInvoiceFee : ActiveRecord<RequiredInvoiceFee>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public RequiredInvoiceFee()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public RequiredInvoiceFee(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public RequiredInvoiceFee(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public RequiredInvoiceFee(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("Required_InvoiceFee", TableType.Table, DataService.GetInstance("WillCall"));
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
				
				TableSchema.TableColumn colvarTInvoiceFeeId = new TableSchema.TableColumn(schema);
				colvarTInvoiceFeeId.ColumnName = "tInvoiceFeeId";
				colvarTInvoiceFeeId.DataType = DbType.Int32;
				colvarTInvoiceFeeId.MaxLength = 0;
				colvarTInvoiceFeeId.AutoIncrement = false;
				colvarTInvoiceFeeId.IsNullable = false;
				colvarTInvoiceFeeId.IsPrimaryKey = false;
				colvarTInvoiceFeeId.IsForeignKey = true;
				colvarTInvoiceFeeId.IsReadOnly = false;
				colvarTInvoiceFeeId.DefaultSetting = @"";
				
					colvarTInvoiceFeeId.ForeignKeyTableName = "InvoiceFee";
				schema.Columns.Add(colvarTInvoiceFeeId);
				
				TableSchema.TableColumn colvarTRequiredId = new TableSchema.TableColumn(schema);
				colvarTRequiredId.ColumnName = "tRequiredId";
				colvarTRequiredId.DataType = DbType.Int32;
				colvarTRequiredId.MaxLength = 0;
				colvarTRequiredId.AutoIncrement = false;
				colvarTRequiredId.IsNullable = false;
				colvarTRequiredId.IsPrimaryKey = false;
				colvarTRequiredId.IsForeignKey = true;
				colvarTRequiredId.IsReadOnly = false;
				colvarTRequiredId.DefaultSetting = @"";
				
					colvarTRequiredId.ForeignKeyTableName = "Required";
				schema.Columns.Add(colvarTRequiredId);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["WillCall"].AddSchema("Required_InvoiceFee",schema);
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
		  
		[XmlAttribute("TInvoiceFeeId")]
		[Bindable(true)]
		public int TInvoiceFeeId 
		{
			get { return GetColumnValue<int>(Columns.TInvoiceFeeId); }
			set { SetColumnValue(Columns.TInvoiceFeeId, value); }
		}
		  
		[XmlAttribute("TRequiredId")]
		[Bindable(true)]
		public int TRequiredId 
		{
			get { return GetColumnValue<int>(Columns.TRequiredId); }
			set { SetColumnValue(Columns.TRequiredId, value); }
		}
		
		#endregion
		
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a InvoiceFee ActiveRecord object related to this RequiredInvoiceFee
		/// 
		/// </summary>
		private Wcss.InvoiceFee InvoiceFee
		{
			get { return Wcss.InvoiceFee.FetchByID(this.TInvoiceFeeId); }
			set { SetColumnValue("tInvoiceFeeId", value.Id); }
		}
        //set up an alternate mechanism to avoid a database call
		private Wcss.InvoiceFee _invoicefeerecord = null;
		
		public Wcss.InvoiceFee InvoiceFeeRecord
		{
		    get
            {
                if (_invoicefeerecord == null)
                {
                    _invoicefeerecord = new Wcss.InvoiceFee();
                    _invoicefeerecord.CopyFrom(this.InvoiceFee);
                }
                return _invoicefeerecord;
            }
            set
            {
                if(value != null && _invoicefeerecord == null)
			        _invoicefeerecord = new Wcss.InvoiceFee();
                
                SetColumnValue("tInvoiceFeeId", value.Id);
                _invoicefeerecord.CopyFrom(value);                
            }
		}
		
		
		/// <summary>
		/// Returns a Required ActiveRecord object related to this RequiredInvoiceFee
		/// 
		/// </summary>
		private Wcss.Required Required
		{
			get { return Wcss.Required.FetchByID(this.TRequiredId); }
			set { SetColumnValue("tRequiredId", value.Id); }
		}
        //set up an alternate mechanism to avoid a database call
		private Wcss.Required _requiredrecord = null;
		
		public Wcss.Required RequiredRecord
		{
		    get
            {
                if (_requiredrecord == null)
                {
                    _requiredrecord = new Wcss.Required();
                    _requiredrecord.CopyFrom(this.Required);
                }
                return _requiredrecord;
            }
            set
            {
                if(value != null && _requiredrecord == null)
			        _requiredrecord = new Wcss.Required();
                
                SetColumnValue("tRequiredId", value.Id);
                _requiredrecord.CopyFrom(value);                
            }
		}
		
		
		#endregion
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(DateTime varDtStamp,int varTInvoiceFeeId,int varTRequiredId)
		{
			RequiredInvoiceFee item = new RequiredInvoiceFee();
			
			item.DtStamp = varDtStamp;
			
			item.TInvoiceFeeId = varTInvoiceFeeId;
			
			item.TRequiredId = varTRequiredId;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varId,DateTime varDtStamp,int varTInvoiceFeeId,int varTRequiredId)
		{
			RequiredInvoiceFee item = new RequiredInvoiceFee();
			
				item.Id = varId;
			
				item.DtStamp = varDtStamp;
			
				item.TInvoiceFeeId = varTInvoiceFeeId;
			
				item.TRequiredId = varTRequiredId;
			
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
        
        
        
        public static TableSchema.TableColumn TInvoiceFeeIdColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn TRequiredIdColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Id = @"Id";
			 public static string DtStamp = @"dtStamp";
			 public static string TInvoiceFeeId = @"tInvoiceFeeId";
			 public static string TRequiredId = @"tRequiredId";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
