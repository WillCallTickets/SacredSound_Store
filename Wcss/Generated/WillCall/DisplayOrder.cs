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
	/// Strongly-typed collection for the DisplayOrder class.
	/// </summary>
    [Serializable]
	public partial class DisplayOrderCollection : ActiveList<DisplayOrder, DisplayOrderCollection>
	{	   
		public DisplayOrderCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>DisplayOrderCollection</returns>
		public DisplayOrderCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                DisplayOrder o = this[i];
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
	/// This is an ActiveRecord class which wraps the DisplayOrder table.
	/// </summary>
	[Serializable]
	public partial class DisplayOrder : ActiveRecord<DisplayOrder>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public DisplayOrder()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public DisplayOrder(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public DisplayOrder(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public DisplayOrder(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("DisplayOrder", TableType.Table, DataService.GetInstance("WillCall"));
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
				
				TableSchema.TableColumn colvarVcOrderContext = new TableSchema.TableColumn(schema);
				colvarVcOrderContext.ColumnName = "vcOrderContext";
				colvarVcOrderContext.DataType = DbType.AnsiString;
				colvarVcOrderContext.MaxLength = 50;
				colvarVcOrderContext.AutoIncrement = false;
				colvarVcOrderContext.IsNullable = false;
				colvarVcOrderContext.IsPrimaryKey = false;
				colvarVcOrderContext.IsForeignKey = false;
				colvarVcOrderContext.IsReadOnly = false;
				colvarVcOrderContext.DefaultSetting = @"";
				colvarVcOrderContext.ForeignKeyTableName = "";
				schema.Columns.Add(colvarVcOrderContext);
				
				TableSchema.TableColumn colvarIItemId = new TableSchema.TableColumn(schema);
				colvarIItemId.ColumnName = "iItemId";
				colvarIItemId.DataType = DbType.Int32;
				colvarIItemId.MaxLength = 0;
				colvarIItemId.AutoIncrement = false;
				colvarIItemId.IsNullable = false;
				colvarIItemId.IsPrimaryKey = false;
				colvarIItemId.IsForeignKey = false;
				colvarIItemId.IsReadOnly = false;
				colvarIItemId.DefaultSetting = @"";
				colvarIItemId.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIItemId);
				
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
				DataService.Providers["WillCall"].AddSchema("DisplayOrder",schema);
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
		  
		[XmlAttribute("VcOrderContext")]
		[Bindable(true)]
		public string VcOrderContext 
		{
			get { return GetColumnValue<string>(Columns.VcOrderContext); }
			set { SetColumnValue(Columns.VcOrderContext, value); }
		}
		  
		[XmlAttribute("IItemId")]
		[Bindable(true)]
		public int IItemId 
		{
			get { return GetColumnValue<int>(Columns.IItemId); }
			set { SetColumnValue(Columns.IItemId, value); }
		}
		  
		[XmlAttribute("IDisplayOrder")]
		[Bindable(true)]
		public int IDisplayOrder 
		{
			get { return GetColumnValue<int>(Columns.IDisplayOrder); }
			set { SetColumnValue(Columns.IDisplayOrder, value); }
		}
		
		#endregion
		
		
			
		
		//no foreign key tables defined (0)
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(DateTime varDtStamp,string varVcOrderContext,int varIItemId,int varIDisplayOrder)
		{
			DisplayOrder item = new DisplayOrder();
			
			item.DtStamp = varDtStamp;
			
			item.VcOrderContext = varVcOrderContext;
			
			item.IItemId = varIItemId;
			
			item.IDisplayOrder = varIDisplayOrder;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varId,DateTime varDtStamp,string varVcOrderContext,int varIItemId,int varIDisplayOrder)
		{
			DisplayOrder item = new DisplayOrder();
			
				item.Id = varId;
			
				item.DtStamp = varDtStamp;
			
				item.VcOrderContext = varVcOrderContext;
			
				item.IItemId = varIItemId;
			
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
        
        
        
        public static TableSchema.TableColumn VcOrderContextColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn IItemIdColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn IDisplayOrderColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Id = @"Id";
			 public static string DtStamp = @"dtStamp";
			 public static string VcOrderContext = @"vcOrderContext";
			 public static string IItemId = @"iItemId";
			 public static string IDisplayOrder = @"iDisplayOrder";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
