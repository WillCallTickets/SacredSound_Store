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
	/// Strongly-typed collection for the PendingOperation class.
	/// </summary>
    [Serializable]
	public partial class PendingOperationCollection : ActiveList<PendingOperation, PendingOperationCollection>
	{	   
		public PendingOperationCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>PendingOperationCollection</returns>
		public PendingOperationCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                PendingOperation o = this[i];
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
	/// This is an ActiveRecord class which wraps the PendingOperation table.
	/// </summary>
	[Serializable]
	public partial class PendingOperation : ActiveRecord<PendingOperation>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public PendingOperation()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public PendingOperation(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public PendingOperation(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public PendingOperation(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("PendingOperation", TableType.Table, DataService.GetInstance("WillCall"));
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
				colvarApplicationId.IsForeignKey = false;
				colvarApplicationId.IsReadOnly = false;
				colvarApplicationId.DefaultSetting = @"";
				colvarApplicationId.ForeignKeyTableName = "";
				schema.Columns.Add(colvarApplicationId);
				
				TableSchema.TableColumn colvarIdentifierId = new TableSchema.TableColumn(schema);
				colvarIdentifierId.ColumnName = "IdentifierId";
				colvarIdentifierId.DataType = DbType.Int32;
				colvarIdentifierId.MaxLength = 0;
				colvarIdentifierId.AutoIncrement = false;
				colvarIdentifierId.IsNullable = false;
				colvarIdentifierId.IsPrimaryKey = false;
				colvarIdentifierId.IsForeignKey = false;
				colvarIdentifierId.IsReadOnly = false;
				colvarIdentifierId.DefaultSetting = @"";
				colvarIdentifierId.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIdentifierId);
				
				TableSchema.TableColumn colvarDtValidUntil = new TableSchema.TableColumn(schema);
				colvarDtValidUntil.ColumnName = "dtValidUntil";
				colvarDtValidUntil.DataType = DbType.DateTime;
				colvarDtValidUntil.MaxLength = 0;
				colvarDtValidUntil.AutoIncrement = false;
				colvarDtValidUntil.IsNullable = false;
				colvarDtValidUntil.IsPrimaryKey = false;
				colvarDtValidUntil.IsForeignKey = false;
				colvarDtValidUntil.IsReadOnly = false;
				colvarDtValidUntil.DefaultSetting = @"";
				colvarDtValidUntil.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDtValidUntil);
				
				TableSchema.TableColumn colvarVcContext = new TableSchema.TableColumn(schema);
				colvarVcContext.ColumnName = "vcContext";
				colvarVcContext.DataType = DbType.AnsiString;
				colvarVcContext.MaxLength = 256;
				colvarVcContext.AutoIncrement = false;
				colvarVcContext.IsNullable = false;
				colvarVcContext.IsPrimaryKey = false;
				colvarVcContext.IsForeignKey = false;
				colvarVcContext.IsReadOnly = false;
				colvarVcContext.DefaultSetting = @"";
				colvarVcContext.ForeignKeyTableName = "";
				schema.Columns.Add(colvarVcContext);
				
				TableSchema.TableColumn colvarUserName = new TableSchema.TableColumn(schema);
				colvarUserName.ColumnName = "UserName";
				colvarUserName.DataType = DbType.AnsiString;
				colvarUserName.MaxLength = 300;
				colvarUserName.AutoIncrement = false;
				colvarUserName.IsNullable = false;
				colvarUserName.IsPrimaryKey = false;
				colvarUserName.IsForeignKey = false;
				colvarUserName.IsReadOnly = false;
				colvarUserName.DefaultSetting = @"";
				colvarUserName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserName);
				
				TableSchema.TableColumn colvarCriteria = new TableSchema.TableColumn(schema);
				colvarCriteria.ColumnName = "Criteria";
				colvarCriteria.DataType = DbType.AnsiString;
				colvarCriteria.MaxLength = 256;
				colvarCriteria.AutoIncrement = false;
				colvarCriteria.IsNullable = true;
				colvarCriteria.IsPrimaryKey = false;
				colvarCriteria.IsForeignKey = false;
				colvarCriteria.IsReadOnly = false;
				colvarCriteria.DefaultSetting = @"";
				colvarCriteria.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCriteria);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["WillCall"].AddSchema("PendingOperation",schema);
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
		  
		[XmlAttribute("IdentifierId")]
		[Bindable(true)]
		public int IdentifierId 
		{
			get { return GetColumnValue<int>(Columns.IdentifierId); }
			set { SetColumnValue(Columns.IdentifierId, value); }
		}
		  
		[XmlAttribute("DtValidUntil")]
		[Bindable(true)]
		public DateTime DtValidUntil 
		{
			get { return GetColumnValue<DateTime>(Columns.DtValidUntil); }
			set { SetColumnValue(Columns.DtValidUntil, value); }
		}
		  
		[XmlAttribute("VcContext")]
		[Bindable(true)]
		public string VcContext 
		{
			get { return GetColumnValue<string>(Columns.VcContext); }
			set { SetColumnValue(Columns.VcContext, value); }
		}
		  
		[XmlAttribute("UserName")]
		[Bindable(true)]
		public string UserName 
		{
			get { return GetColumnValue<string>(Columns.UserName); }
			set { SetColumnValue(Columns.UserName, value); }
		}
		  
		[XmlAttribute("Criteria")]
		[Bindable(true)]
		public string Criteria 
		{
			get { return GetColumnValue<string>(Columns.Criteria); }
			set { SetColumnValue(Columns.Criteria, value); }
		}
		
		#endregion
		
		
			
		
		//no foreign key tables defined (0)
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(DateTime varDtStamp,Guid varApplicationId,int varIdentifierId,DateTime varDtValidUntil,string varVcContext,string varUserName,string varCriteria)
		{
			PendingOperation item = new PendingOperation();
			
			item.DtStamp = varDtStamp;
			
			item.ApplicationId = varApplicationId;
			
			item.IdentifierId = varIdentifierId;
			
			item.DtValidUntil = varDtValidUntil;
			
			item.VcContext = varVcContext;
			
			item.UserName = varUserName;
			
			item.Criteria = varCriteria;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varId,DateTime varDtStamp,Guid varApplicationId,int varIdentifierId,DateTime varDtValidUntil,string varVcContext,string varUserName,string varCriteria)
		{
			PendingOperation item = new PendingOperation();
			
				item.Id = varId;
			
				item.DtStamp = varDtStamp;
			
				item.ApplicationId = varApplicationId;
			
				item.IdentifierId = varIdentifierId;
			
				item.DtValidUntil = varDtValidUntil;
			
				item.VcContext = varVcContext;
			
				item.UserName = varUserName;
			
				item.Criteria = varCriteria;
			
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
        
        
        
        public static TableSchema.TableColumn IdentifierIdColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn DtValidUntilColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn VcContextColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn UserNameColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn CriteriaColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Id = @"Id";
			 public static string DtStamp = @"dtStamp";
			 public static string ApplicationId = @"ApplicationId";
			 public static string IdentifierId = @"IdentifierId";
			 public static string DtValidUntil = @"dtValidUntil";
			 public static string VcContext = @"vcContext";
			 public static string UserName = @"UserName";
			 public static string Criteria = @"Criteria";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
