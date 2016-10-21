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
	/// Strongly-typed collection for the EntityValue class.
	/// </summary>
    [Serializable]
	public partial class EntityValueCollection : ActiveList<EntityValue, EntityValueCollection>
	{	   
		public EntityValueCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>EntityValueCollection</returns>
		public EntityValueCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                EntityValue o = this[i];
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
	/// This is an ActiveRecord class which wraps the EntityValue table.
	/// </summary>
	[Serializable]
	public partial class EntityValue : ActiveRecord<EntityValue>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public EntityValue()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public EntityValue(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public EntityValue(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public EntityValue(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("EntityValue", TableType.Table, DataService.GetInstance("WillCall"));
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
				
				TableSchema.TableColumn colvarDtCreated = new TableSchema.TableColumn(schema);
				colvarDtCreated.ColumnName = "dtCreated";
				colvarDtCreated.DataType = DbType.DateTime;
				colvarDtCreated.MaxLength = 0;
				colvarDtCreated.AutoIncrement = false;
				colvarDtCreated.IsNullable = false;
				colvarDtCreated.IsPrimaryKey = false;
				colvarDtCreated.IsForeignKey = false;
				colvarDtCreated.IsReadOnly = false;
				
						colvarDtCreated.DefaultSetting = @"(getdate())";
				colvarDtCreated.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDtCreated);
				
				TableSchema.TableColumn colvarDtModified = new TableSchema.TableColumn(schema);
				colvarDtModified.ColumnName = "dtModified";
				colvarDtModified.DataType = DbType.DateTime;
				colvarDtModified.MaxLength = 0;
				colvarDtModified.AutoIncrement = false;
				colvarDtModified.IsNullable = false;
				colvarDtModified.IsPrimaryKey = false;
				colvarDtModified.IsForeignKey = false;
				colvarDtModified.IsReadOnly = false;
				colvarDtModified.DefaultSetting = @"";
				colvarDtModified.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDtModified);
				
				TableSchema.TableColumn colvarUserId = new TableSchema.TableColumn(schema);
				colvarUserId.ColumnName = "UserId";
				colvarUserId.DataType = DbType.Guid;
				colvarUserId.MaxLength = 0;
				colvarUserId.AutoIncrement = false;
				colvarUserId.IsNullable = true;
				colvarUserId.IsPrimaryKey = false;
				colvarUserId.IsForeignKey = false;
				colvarUserId.IsReadOnly = false;
				colvarUserId.DefaultSetting = @"";
				colvarUserId.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserId);
				
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
				
				TableSchema.TableColumn colvarVcContext = new TableSchema.TableColumn(schema);
				colvarVcContext.ColumnName = "vcContext";
				colvarVcContext.DataType = DbType.AnsiString;
				colvarVcContext.MaxLength = 256;
				colvarVcContext.AutoIncrement = false;
				colvarVcContext.IsNullable = true;
				colvarVcContext.IsPrimaryKey = false;
				colvarVcContext.IsForeignKey = false;
				colvarVcContext.IsReadOnly = false;
				colvarVcContext.DefaultSetting = @"";
				colvarVcContext.ForeignKeyTableName = "";
				schema.Columns.Add(colvarVcContext);
				
				TableSchema.TableColumn colvarVcTableRelation = new TableSchema.TableColumn(schema);
				colvarVcTableRelation.ColumnName = "vcTableRelation";
				colvarVcTableRelation.DataType = DbType.AnsiString;
				colvarVcTableRelation.MaxLength = 256;
				colvarVcTableRelation.AutoIncrement = false;
				colvarVcTableRelation.IsNullable = true;
				colvarVcTableRelation.IsPrimaryKey = false;
				colvarVcTableRelation.IsForeignKey = false;
				colvarVcTableRelation.IsReadOnly = false;
				colvarVcTableRelation.DefaultSetting = @"";
				colvarVcTableRelation.ForeignKeyTableName = "";
				schema.Columns.Add(colvarVcTableRelation);
				
				TableSchema.TableColumn colvarTParentId = new TableSchema.TableColumn(schema);
				colvarTParentId.ColumnName = "tParentId";
				colvarTParentId.DataType = DbType.Int32;
				colvarTParentId.MaxLength = 0;
				colvarTParentId.AutoIncrement = false;
				colvarTParentId.IsNullable = true;
				colvarTParentId.IsPrimaryKey = false;
				colvarTParentId.IsForeignKey = false;
				colvarTParentId.IsReadOnly = false;
				colvarTParentId.DefaultSetting = @"";
				colvarTParentId.ForeignKeyTableName = "";
				schema.Columns.Add(colvarTParentId);
				
				TableSchema.TableColumn colvarVcValueContext = new TableSchema.TableColumn(schema);
				colvarVcValueContext.ColumnName = "vcValueContext";
				colvarVcValueContext.DataType = DbType.AnsiString;
				colvarVcValueContext.MaxLength = 150;
				colvarVcValueContext.AutoIncrement = false;
				colvarVcValueContext.IsNullable = false;
				colvarVcValueContext.IsPrimaryKey = false;
				colvarVcValueContext.IsForeignKey = false;
				colvarVcValueContext.IsReadOnly = false;
				colvarVcValueContext.DefaultSetting = @"";
				colvarVcValueContext.ForeignKeyTableName = "";
				schema.Columns.Add(colvarVcValueContext);
				
				TableSchema.TableColumn colvarVcValueType = new TableSchema.TableColumn(schema);
				colvarVcValueType.ColumnName = "vcValueType";
				colvarVcValueType.DataType = DbType.AnsiString;
				colvarVcValueType.MaxLength = 50;
				colvarVcValueType.AutoIncrement = false;
				colvarVcValueType.IsNullable = false;
				colvarVcValueType.IsPrimaryKey = false;
				colvarVcValueType.IsForeignKey = false;
				colvarVcValueType.IsReadOnly = false;
				
						colvarVcValueType.DefaultSetting = @"('string')";
				colvarVcValueType.ForeignKeyTableName = "";
				schema.Columns.Add(colvarVcValueType);
				
				TableSchema.TableColumn colvarVcValue = new TableSchema.TableColumn(schema);
				colvarVcValue.ColumnName = "vcValue";
				colvarVcValue.DataType = DbType.AnsiString;
				colvarVcValue.MaxLength = 2000;
				colvarVcValue.AutoIncrement = false;
				colvarVcValue.IsNullable = false;
				colvarVcValue.IsPrimaryKey = false;
				colvarVcValue.IsForeignKey = false;
				colvarVcValue.IsReadOnly = false;
				colvarVcValue.DefaultSetting = @"";
				colvarVcValue.ForeignKeyTableName = "";
				schema.Columns.Add(colvarVcValue);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["WillCall"].AddSchema("EntityValue",schema);
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
		  
		[XmlAttribute("DtCreated")]
		[Bindable(true)]
		public DateTime DtCreated 
		{
			get { return GetColumnValue<DateTime>(Columns.DtCreated); }
			set { SetColumnValue(Columns.DtCreated, value); }
		}
		  
		[XmlAttribute("DtModified")]
		[Bindable(true)]
		public DateTime DtModified 
		{
			get { return GetColumnValue<DateTime>(Columns.DtModified); }
			set { SetColumnValue(Columns.DtModified, value); }
		}
		  
		[XmlAttribute("UserId")]
		[Bindable(true)]
		public Guid? UserId 
		{
			get { return GetColumnValue<Guid?>(Columns.UserId); }
			set { SetColumnValue(Columns.UserId, value); }
		}
		  
		[XmlAttribute("IDisplayOrder")]
		[Bindable(true)]
		public int IDisplayOrder 
		{
			get { return GetColumnValue<int>(Columns.IDisplayOrder); }
			set { SetColumnValue(Columns.IDisplayOrder, value); }
		}
		  
		[XmlAttribute("VcContext")]
		[Bindable(true)]
		public string VcContext 
		{
			get { return GetColumnValue<string>(Columns.VcContext); }
			set { SetColumnValue(Columns.VcContext, value); }
		}
		  
		[XmlAttribute("VcTableRelation")]
		[Bindable(true)]
		public string VcTableRelation 
		{
			get { return GetColumnValue<string>(Columns.VcTableRelation); }
			set { SetColumnValue(Columns.VcTableRelation, value); }
		}
		  
		[XmlAttribute("TParentId")]
		[Bindable(true)]
		public int? TParentId 
		{
			get { return GetColumnValue<int?>(Columns.TParentId); }
			set { SetColumnValue(Columns.TParentId, value); }
		}
		  
		[XmlAttribute("VcValueContext")]
		[Bindable(true)]
		public string VcValueContext 
		{
			get { return GetColumnValue<string>(Columns.VcValueContext); }
			set { SetColumnValue(Columns.VcValueContext, value); }
		}
		  
		[XmlAttribute("VcValueType")]
		[Bindable(true)]
		public string VcValueType 
		{
			get { return GetColumnValue<string>(Columns.VcValueType); }
			set { SetColumnValue(Columns.VcValueType, value); }
		}
		  
		[XmlAttribute("VcValue")]
		[Bindable(true)]
		public string VcValue 
		{
			get { return GetColumnValue<string>(Columns.VcValue); }
			set { SetColumnValue(Columns.VcValue, value); }
		}
		
		#endregion
		
		
			
		
		//no foreign key tables defined (0)
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(DateTime varDtCreated,DateTime varDtModified,Guid? varUserId,int varIDisplayOrder,string varVcContext,string varVcTableRelation,int? varTParentId,string varVcValueContext,string varVcValueType,string varVcValue)
		{
			EntityValue item = new EntityValue();
			
			item.DtCreated = varDtCreated;
			
			item.DtModified = varDtModified;
			
			item.UserId = varUserId;
			
			item.IDisplayOrder = varIDisplayOrder;
			
			item.VcContext = varVcContext;
			
			item.VcTableRelation = varVcTableRelation;
			
			item.TParentId = varTParentId;
			
			item.VcValueContext = varVcValueContext;
			
			item.VcValueType = varVcValueType;
			
			item.VcValue = varVcValue;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varId,DateTime varDtCreated,DateTime varDtModified,Guid? varUserId,int varIDisplayOrder,string varVcContext,string varVcTableRelation,int? varTParentId,string varVcValueContext,string varVcValueType,string varVcValue)
		{
			EntityValue item = new EntityValue();
			
				item.Id = varId;
			
				item.DtCreated = varDtCreated;
			
				item.DtModified = varDtModified;
			
				item.UserId = varUserId;
			
				item.IDisplayOrder = varIDisplayOrder;
			
				item.VcContext = varVcContext;
			
				item.VcTableRelation = varVcTableRelation;
			
				item.TParentId = varTParentId;
			
				item.VcValueContext = varVcValueContext;
			
				item.VcValueType = varVcValueType;
			
				item.VcValue = varVcValue;
			
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
        
        
        
        public static TableSchema.TableColumn DtCreatedColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn DtModifiedColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn UserIdColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn IDisplayOrderColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn VcContextColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn VcTableRelationColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn TParentIdColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn VcValueContextColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn VcValueTypeColumn
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        public static TableSchema.TableColumn VcValueColumn
        {
            get { return Schema.Columns[10]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Id = @"Id";
			 public static string DtCreated = @"dtCreated";
			 public static string DtModified = @"dtModified";
			 public static string UserId = @"UserId";
			 public static string IDisplayOrder = @"iDisplayOrder";
			 public static string VcContext = @"vcContext";
			 public static string VcTableRelation = @"vcTableRelation";
			 public static string TParentId = @"tParentId";
			 public static string VcValueContext = @"vcValueContext";
			 public static string VcValueType = @"vcValueType";
			 public static string VcValue = @"vcValue";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
