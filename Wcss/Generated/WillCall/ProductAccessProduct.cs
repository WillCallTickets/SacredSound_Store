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
	/// Strongly-typed collection for the ProductAccessProduct class.
	/// </summary>
    [Serializable]
	public partial class ProductAccessProductCollection : ActiveList<ProductAccessProduct, ProductAccessProductCollection>
	{	   
		public ProductAccessProductCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>ProductAccessProductCollection</returns>
		public ProductAccessProductCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                ProductAccessProduct o = this[i];
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
	/// This is an ActiveRecord class which wraps the ProductAccessProduct table.
	/// </summary>
	[Serializable]
	public partial class ProductAccessProduct : ActiveRecord<ProductAccessProduct>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public ProductAccessProduct()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public ProductAccessProduct(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public ProductAccessProduct(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public ProductAccessProduct(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("ProductAccessProduct", TableType.Table, DataService.GetInstance("WillCall"));
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
				
				TableSchema.TableColumn colvarTProductAccessId = new TableSchema.TableColumn(schema);
				colvarTProductAccessId.ColumnName = "TProductAccessId";
				colvarTProductAccessId.DataType = DbType.Int32;
				colvarTProductAccessId.MaxLength = 0;
				colvarTProductAccessId.AutoIncrement = false;
				colvarTProductAccessId.IsNullable = false;
				colvarTProductAccessId.IsPrimaryKey = false;
				colvarTProductAccessId.IsForeignKey = true;
				colvarTProductAccessId.IsReadOnly = false;
				colvarTProductAccessId.DefaultSetting = @"";
				
					colvarTProductAccessId.ForeignKeyTableName = "ProductAccess";
				schema.Columns.Add(colvarTProductAccessId);
				
				TableSchema.TableColumn colvarVcContext = new TableSchema.TableColumn(schema);
				colvarVcContext.ColumnName = "vcContext";
				colvarVcContext.DataType = DbType.AnsiString;
				colvarVcContext.MaxLength = 50;
				colvarVcContext.AutoIncrement = false;
				colvarVcContext.IsNullable = false;
				colvarVcContext.IsPrimaryKey = false;
				colvarVcContext.IsForeignKey = false;
				colvarVcContext.IsReadOnly = false;
				colvarVcContext.DefaultSetting = @"";
				colvarVcContext.ForeignKeyTableName = "";
				schema.Columns.Add(colvarVcContext);
				
				TableSchema.TableColumn colvarTParentId = new TableSchema.TableColumn(schema);
				colvarTParentId.ColumnName = "TParentId";
				colvarTParentId.DataType = DbType.Int32;
				colvarTParentId.MaxLength = 0;
				colvarTParentId.AutoIncrement = false;
				colvarTParentId.IsNullable = false;
				colvarTParentId.IsPrimaryKey = false;
				colvarTParentId.IsForeignKey = false;
				colvarTParentId.IsReadOnly = false;
				colvarTParentId.DefaultSetting = @"";
				colvarTParentId.ForeignKeyTableName = "";
				schema.Columns.Add(colvarTParentId);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["WillCall"].AddSchema("ProductAccessProduct",schema);
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
		  
		[XmlAttribute("TProductAccessId")]
		[Bindable(true)]
		public int TProductAccessId 
		{
			get { return GetColumnValue<int>(Columns.TProductAccessId); }
			set { SetColumnValue(Columns.TProductAccessId, value); }
		}
		  
		[XmlAttribute("VcContext")]
		[Bindable(true)]
		public string VcContext 
		{
			get { return GetColumnValue<string>(Columns.VcContext); }
			set { SetColumnValue(Columns.VcContext, value); }
		}
		  
		[XmlAttribute("TParentId")]
		[Bindable(true)]
		public int TParentId 
		{
			get { return GetColumnValue<int>(Columns.TParentId); }
			set { SetColumnValue(Columns.TParentId, value); }
		}
		
		#endregion
		
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a ProductAccess ActiveRecord object related to this ProductAccessProduct
		/// 
		/// </summary>
		private Wcss.ProductAccess ProductAccess
		{
			get { return Wcss.ProductAccess.FetchByID(this.TProductAccessId); }
			set { SetColumnValue("TProductAccessId", value.Id); }
		}
        //set up an alternate mechanism to avoid a database call
		private Wcss.ProductAccess _productaccessrecord = null;
		
		public Wcss.ProductAccess ProductAccessRecord
		{
		    get
            {
                if (_productaccessrecord == null)
                {
                    _productaccessrecord = new Wcss.ProductAccess();
                    _productaccessrecord.CopyFrom(this.ProductAccess);
                }
                return _productaccessrecord;
            }
            set
            {
                if(value != null && _productaccessrecord == null)
			        _productaccessrecord = new Wcss.ProductAccess();
                
                SetColumnValue("TProductAccessId", value.Id);
                _productaccessrecord.CopyFrom(value);                
            }
		}
		
		
		#endregion
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(DateTime varDtStamp,int varTProductAccessId,string varVcContext,int varTParentId)
		{
			ProductAccessProduct item = new ProductAccessProduct();
			
			item.DtStamp = varDtStamp;
			
			item.TProductAccessId = varTProductAccessId;
			
			item.VcContext = varVcContext;
			
			item.TParentId = varTParentId;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varId,DateTime varDtStamp,int varTProductAccessId,string varVcContext,int varTParentId)
		{
			ProductAccessProduct item = new ProductAccessProduct();
			
				item.Id = varId;
			
				item.DtStamp = varDtStamp;
			
				item.TProductAccessId = varTProductAccessId;
			
				item.VcContext = varVcContext;
			
				item.TParentId = varTParentId;
			
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
        
        
        
        public static TableSchema.TableColumn TProductAccessIdColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn VcContextColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn TParentIdColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Id = @"Id";
			 public static string DtStamp = @"dtStamp";
			 public static string TProductAccessId = @"TProductAccessId";
			 public static string VcContext = @"vcContext";
			 public static string TParentId = @"TParentId";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
