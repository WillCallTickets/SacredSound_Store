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
	/// Strongly-typed collection for the FbStat class.
	/// </summary>
    [Serializable]
	public partial class FbStatCollection : ActiveList<FbStat, FbStatCollection>
	{	   
		public FbStatCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>FbStatCollection</returns>
		public FbStatCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                FbStat o = this[i];
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
	/// This is an ActiveRecord class which wraps the FB_Stat table.
	/// </summary>
	[Serializable]
	public partial class FbStat : ActiveRecord<FbStat>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public FbStat()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public FbStat(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public FbStat(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public FbStat(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("FB_Stat", TableType.Table, DataService.GetInstance("WillCall"));
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
				
				TableSchema.TableColumn colvarEntityId = new TableSchema.TableColumn(schema);
				colvarEntityId.ColumnName = "EntityId";
				colvarEntityId.DataType = DbType.Int32;
				colvarEntityId.MaxLength = 0;
				colvarEntityId.AutoIncrement = false;
				colvarEntityId.IsNullable = true;
				colvarEntityId.IsPrimaryKey = false;
				colvarEntityId.IsForeignKey = false;
				colvarEntityId.IsReadOnly = false;
				colvarEntityId.DefaultSetting = @"";
				colvarEntityId.ForeignKeyTableName = "";
				schema.Columns.Add(colvarEntityId);
				
				TableSchema.TableColumn colvarUrl = new TableSchema.TableColumn(schema);
				colvarUrl.ColumnName = "Url";
				colvarUrl.DataType = DbType.AnsiString;
				colvarUrl.MaxLength = 500;
				colvarUrl.AutoIncrement = false;
				colvarUrl.IsNullable = false;
				colvarUrl.IsPrimaryKey = false;
				colvarUrl.IsForeignKey = false;
				colvarUrl.IsReadOnly = false;
				colvarUrl.DefaultSetting = @"";
				colvarUrl.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUrl);
				
				TableSchema.TableColumn colvarApiFunction = new TableSchema.TableColumn(schema);
				colvarApiFunction.ColumnName = "ApiFunction";
				colvarApiFunction.DataType = DbType.AnsiString;
				colvarApiFunction.MaxLength = 50;
				colvarApiFunction.AutoIncrement = false;
				colvarApiFunction.IsNullable = false;
				colvarApiFunction.IsPrimaryKey = false;
				colvarApiFunction.IsForeignKey = false;
				colvarApiFunction.IsReadOnly = false;
				colvarApiFunction.DefaultSetting = @"";
				colvarApiFunction.ForeignKeyTableName = "";
				schema.Columns.Add(colvarApiFunction);
				
				TableSchema.TableColumn colvarTotal = new TableSchema.TableColumn(schema);
				colvarTotal.ColumnName = "Total";
				colvarTotal.DataType = DbType.Int32;
				colvarTotal.MaxLength = 0;
				colvarTotal.AutoIncrement = false;
				colvarTotal.IsNullable = false;
				colvarTotal.IsPrimaryKey = false;
				colvarTotal.IsForeignKey = false;
				colvarTotal.IsReadOnly = false;
				
						colvarTotal.DefaultSetting = @"((0))";
				colvarTotal.ForeignKeyTableName = "";
				schema.Columns.Add(colvarTotal);
				
				TableSchema.TableColumn colvarDtModified = new TableSchema.TableColumn(schema);
				colvarDtModified.ColumnName = "dtModified";
				colvarDtModified.DataType = DbType.DateTime;
				colvarDtModified.MaxLength = 0;
				colvarDtModified.AutoIncrement = false;
				colvarDtModified.IsNullable = false;
				colvarDtModified.IsPrimaryKey = false;
				colvarDtModified.IsForeignKey = false;
				colvarDtModified.IsReadOnly = false;
				
						colvarDtModified.DefaultSetting = @"(getdate())";
				colvarDtModified.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDtModified);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["WillCall"].AddSchema("FB_Stat",schema);
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
		  
		[XmlAttribute("EntityId")]
		[Bindable(true)]
		public int? EntityId 
		{
			get { return GetColumnValue<int?>(Columns.EntityId); }
			set { SetColumnValue(Columns.EntityId, value); }
		}
		  
		[XmlAttribute("Url")]
		[Bindable(true)]
		public string Url 
		{
			get { return GetColumnValue<string>(Columns.Url); }
			set { SetColumnValue(Columns.Url, value); }
		}
		  
		[XmlAttribute("ApiFunction")]
		[Bindable(true)]
		public string ApiFunction 
		{
			get { return GetColumnValue<string>(Columns.ApiFunction); }
			set { SetColumnValue(Columns.ApiFunction, value); }
		}
		  
		[XmlAttribute("Total")]
		[Bindable(true)]
		public int Total 
		{
			get { return GetColumnValue<int>(Columns.Total); }
			set { SetColumnValue(Columns.Total, value); }
		}
		  
		[XmlAttribute("DtModified")]
		[Bindable(true)]
		public DateTime DtModified 
		{
			get { return GetColumnValue<DateTime>(Columns.DtModified); }
			set { SetColumnValue(Columns.DtModified, value); }
		}
		
		#endregion
		
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a AspnetApplication ActiveRecord object related to this FbStat
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
		public static void Insert(DateTime varDtStamp,Guid varApplicationId,int? varEntityId,string varUrl,string varApiFunction,int varTotal,DateTime varDtModified)
		{
			FbStat item = new FbStat();
			
			item.DtStamp = varDtStamp;
			
			item.ApplicationId = varApplicationId;
			
			item.EntityId = varEntityId;
			
			item.Url = varUrl;
			
			item.ApiFunction = varApiFunction;
			
			item.Total = varTotal;
			
			item.DtModified = varDtModified;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varId,DateTime varDtStamp,Guid varApplicationId,int? varEntityId,string varUrl,string varApiFunction,int varTotal,DateTime varDtModified)
		{
			FbStat item = new FbStat();
			
				item.Id = varId;
			
				item.DtStamp = varDtStamp;
			
				item.ApplicationId = varApplicationId;
			
				item.EntityId = varEntityId;
			
				item.Url = varUrl;
			
				item.ApiFunction = varApiFunction;
			
				item.Total = varTotal;
			
				item.DtModified = varDtModified;
			
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
        
        
        
        public static TableSchema.TableColumn EntityIdColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn UrlColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn ApiFunctionColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn TotalColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn DtModifiedColumn
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
			 public static string EntityId = @"EntityId";
			 public static string Url = @"Url";
			 public static string ApiFunction = @"ApiFunction";
			 public static string Total = @"Total";
			 public static string DtModified = @"dtModified";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
