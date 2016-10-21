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
	/// Strongly-typed collection for the MerchStock class.
	/// </summary>
    [Serializable]
	public partial class MerchStockCollection : ActiveList<MerchStock, MerchStockCollection>
	{	   
		public MerchStockCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>MerchStockCollection</returns>
		public MerchStockCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                MerchStock o = this[i];
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
	/// This is an ActiveRecord class which wraps the MerchStock table.
	/// </summary>
	[Serializable]
	public partial class MerchStock : ActiveRecord<MerchStock>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public MerchStock()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public MerchStock(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public MerchStock(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public MerchStock(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("MerchStock", TableType.Table, DataService.GetInstance("WillCall"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarId = new TableSchema.TableColumn(schema);
				colvarId.ColumnName = "Id";
				colvarId.DataType = DbType.Int32;
				colvarId.MaxLength = 0;
				colvarId.AutoIncrement = true;
				colvarId.IsNullable = false;
				colvarId.IsPrimaryKey = false;
				colvarId.IsForeignKey = false;
				colvarId.IsReadOnly = false;
				colvarId.DefaultSetting = @"";
				colvarId.ForeignKeyTableName = "";
				schema.Columns.Add(colvarId);
				
				TableSchema.TableColumn colvarGuid = new TableSchema.TableColumn(schema);
				colvarGuid.ColumnName = "GUID";
				colvarGuid.DataType = DbType.Guid;
				colvarGuid.MaxLength = 0;
				colvarGuid.AutoIncrement = false;
				colvarGuid.IsNullable = false;
				colvarGuid.IsPrimaryKey = true;
				colvarGuid.IsForeignKey = false;
				colvarGuid.IsReadOnly = false;
				colvarGuid.DefaultSetting = @"";
				colvarGuid.ForeignKeyTableName = "";
				schema.Columns.Add(colvarGuid);
				
				TableSchema.TableColumn colvarSessionId = new TableSchema.TableColumn(schema);
				colvarSessionId.ColumnName = "SessionId";
				colvarSessionId.DataType = DbType.AnsiString;
				colvarSessionId.MaxLength = 256;
				colvarSessionId.AutoIncrement = false;
				colvarSessionId.IsNullable = false;
				colvarSessionId.IsPrimaryKey = false;
				colvarSessionId.IsForeignKey = false;
				colvarSessionId.IsReadOnly = false;
				
						colvarSessionId.DefaultSetting = @"('null')";
				colvarSessionId.ForeignKeyTableName = "";
				schema.Columns.Add(colvarSessionId);
				
				TableSchema.TableColumn colvarUserName = new TableSchema.TableColumn(schema);
				colvarUserName.ColumnName = "UserName";
				colvarUserName.DataType = DbType.AnsiString;
				colvarUserName.MaxLength = 256;
				colvarUserName.AutoIncrement = false;
				colvarUserName.IsNullable = false;
				colvarUserName.IsPrimaryKey = false;
				colvarUserName.IsForeignKey = false;
				colvarUserName.IsReadOnly = false;
				
						colvarUserName.DefaultSetting = @"('')";
				colvarUserName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserName);
				
				TableSchema.TableColumn colvarTMerchId = new TableSchema.TableColumn(schema);
				colvarTMerchId.ColumnName = "tMerchId";
				colvarTMerchId.DataType = DbType.Int32;
				colvarTMerchId.MaxLength = 0;
				colvarTMerchId.AutoIncrement = false;
				colvarTMerchId.IsNullable = false;
				colvarTMerchId.IsPrimaryKey = false;
				colvarTMerchId.IsForeignKey = false;
				colvarTMerchId.IsReadOnly = false;
				colvarTMerchId.DefaultSetting = @"";
				colvarTMerchId.ForeignKeyTableName = "";
				schema.Columns.Add(colvarTMerchId);
				
				TableSchema.TableColumn colvarIQty = new TableSchema.TableColumn(schema);
				colvarIQty.ColumnName = "iQty";
				colvarIQty.DataType = DbType.Int32;
				colvarIQty.MaxLength = 0;
				colvarIQty.AutoIncrement = false;
				colvarIQty.IsNullable = false;
				colvarIQty.IsPrimaryKey = false;
				colvarIQty.IsForeignKey = false;
				colvarIQty.IsReadOnly = false;
				colvarIQty.DefaultSetting = @"";
				colvarIQty.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIQty);
				
				TableSchema.TableColumn colvarDtTTL = new TableSchema.TableColumn(schema);
				colvarDtTTL.ColumnName = "dtTTL";
				colvarDtTTL.DataType = DbType.DateTime;
				colvarDtTTL.MaxLength = 0;
				colvarDtTTL.AutoIncrement = false;
				colvarDtTTL.IsNullable = false;
				colvarDtTTL.IsPrimaryKey = false;
				colvarDtTTL.IsForeignKey = false;
				colvarDtTTL.IsReadOnly = false;
				colvarDtTTL.DefaultSetting = @"";
				colvarDtTTL.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDtTTL);
				
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
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["WillCall"].AddSchema("MerchStock",schema);
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
		  
		[XmlAttribute("Guid")]
		[Bindable(true)]
		public Guid Guid 
		{
			get { return GetColumnValue<Guid>(Columns.Guid); }
			set { SetColumnValue(Columns.Guid, value); }
		}
		  
		[XmlAttribute("SessionId")]
		[Bindable(true)]
		public string SessionId 
		{
			get { return GetColumnValue<string>(Columns.SessionId); }
			set { SetColumnValue(Columns.SessionId, value); }
		}
		  
		[XmlAttribute("UserName")]
		[Bindable(true)]
		public string UserName 
		{
			get { return GetColumnValue<string>(Columns.UserName); }
			set { SetColumnValue(Columns.UserName, value); }
		}
		  
		[XmlAttribute("TMerchId")]
		[Bindable(true)]
		public int TMerchId 
		{
			get { return GetColumnValue<int>(Columns.TMerchId); }
			set { SetColumnValue(Columns.TMerchId, value); }
		}
		  
		[XmlAttribute("IQty")]
		[Bindable(true)]
		public int IQty 
		{
			get { return GetColumnValue<int>(Columns.IQty); }
			set { SetColumnValue(Columns.IQty, value); }
		}
		  
		[XmlAttribute("DtTTL")]
		[Bindable(true)]
		public DateTime DtTTL 
		{
			get { return GetColumnValue<DateTime>(Columns.DtTTL); }
			set { SetColumnValue(Columns.DtTTL, value); }
		}
		  
		[XmlAttribute("DtStamp")]
		[Bindable(true)]
		public DateTime DtStamp 
		{
			get { return GetColumnValue<DateTime>(Columns.DtStamp); }
			set { SetColumnValue(Columns.DtStamp, value); }
		}
		
		#endregion
		
		
			
		
		//no foreign key tables defined (0)
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(Guid varGuid,string varSessionId,string varUserName,int varTMerchId,int varIQty,DateTime varDtTTL,DateTime varDtStamp)
		{
			MerchStock item = new MerchStock();
			
			item.Guid = varGuid;
			
			item.SessionId = varSessionId;
			
			item.UserName = varUserName;
			
			item.TMerchId = varTMerchId;
			
			item.IQty = varIQty;
			
			item.DtTTL = varDtTTL;
			
			item.DtStamp = varDtStamp;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varId,Guid varGuid,string varSessionId,string varUserName,int varTMerchId,int varIQty,DateTime varDtTTL,DateTime varDtStamp)
		{
			MerchStock item = new MerchStock();
			
				item.Id = varId;
			
				item.Guid = varGuid;
			
				item.SessionId = varSessionId;
			
				item.UserName = varUserName;
			
				item.TMerchId = varTMerchId;
			
				item.IQty = varIQty;
			
				item.DtTTL = varDtTTL;
			
				item.DtStamp = varDtStamp;
			
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
        
        
        
        public static TableSchema.TableColumn GuidColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn SessionIdColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn UserNameColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn TMerchIdColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn IQtyColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn DtTTLColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn DtStampColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Id = @"Id";
			 public static string Guid = @"GUID";
			 public static string SessionId = @"SessionId";
			 public static string UserName = @"UserName";
			 public static string TMerchId = @"tMerchId";
			 public static string IQty = @"iQty";
			 public static string DtTTL = @"dtTTL";
			 public static string DtStamp = @"dtStamp";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
