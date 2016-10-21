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
	/// Strongly-typed collection for the AspnetSchemaVersion class.
	/// </summary>
    [Serializable]
	public partial class AspnetSchemaVersionCollection : ActiveList<AspnetSchemaVersion, AspnetSchemaVersionCollection>
	{	   
		public AspnetSchemaVersionCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>AspnetSchemaVersionCollection</returns>
		public AspnetSchemaVersionCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                AspnetSchemaVersion o = this[i];
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
	/// This is an ActiveRecord class which wraps the aspnet_SchemaVersions table.
	/// </summary>
	[Serializable]
	public partial class AspnetSchemaVersion : ActiveRecord<AspnetSchemaVersion>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public AspnetSchemaVersion()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public AspnetSchemaVersion(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public AspnetSchemaVersion(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public AspnetSchemaVersion(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("aspnet_SchemaVersions", TableType.Table, DataService.GetInstance("WillCall"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarFeature = new TableSchema.TableColumn(schema);
				colvarFeature.ColumnName = "Feature";
				colvarFeature.DataType = DbType.String;
				colvarFeature.MaxLength = 128;
				colvarFeature.AutoIncrement = false;
				colvarFeature.IsNullable = false;
				colvarFeature.IsPrimaryKey = true;
				colvarFeature.IsForeignKey = false;
				colvarFeature.IsReadOnly = false;
				colvarFeature.DefaultSetting = @"";
				colvarFeature.ForeignKeyTableName = "";
				schema.Columns.Add(colvarFeature);
				
				TableSchema.TableColumn colvarCompatibleSchemaVersion = new TableSchema.TableColumn(schema);
				colvarCompatibleSchemaVersion.ColumnName = "CompatibleSchemaVersion";
				colvarCompatibleSchemaVersion.DataType = DbType.String;
				colvarCompatibleSchemaVersion.MaxLength = 128;
				colvarCompatibleSchemaVersion.AutoIncrement = false;
				colvarCompatibleSchemaVersion.IsNullable = false;
				colvarCompatibleSchemaVersion.IsPrimaryKey = true;
				colvarCompatibleSchemaVersion.IsForeignKey = false;
				colvarCompatibleSchemaVersion.IsReadOnly = false;
				colvarCompatibleSchemaVersion.DefaultSetting = @"";
				colvarCompatibleSchemaVersion.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCompatibleSchemaVersion);
				
				TableSchema.TableColumn colvarIsCurrentVersion = new TableSchema.TableColumn(schema);
				colvarIsCurrentVersion.ColumnName = "IsCurrentVersion";
				colvarIsCurrentVersion.DataType = DbType.Boolean;
				colvarIsCurrentVersion.MaxLength = 0;
				colvarIsCurrentVersion.AutoIncrement = false;
				colvarIsCurrentVersion.IsNullable = false;
				colvarIsCurrentVersion.IsPrimaryKey = false;
				colvarIsCurrentVersion.IsForeignKey = false;
				colvarIsCurrentVersion.IsReadOnly = false;
				colvarIsCurrentVersion.DefaultSetting = @"";
				colvarIsCurrentVersion.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIsCurrentVersion);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["WillCall"].AddSchema("aspnet_SchemaVersions",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("Feature")]
		[Bindable(true)]
		public string Feature 
		{
			get { return GetColumnValue<string>(Columns.Feature); }
			set { SetColumnValue(Columns.Feature, value); }
		}
		  
		[XmlAttribute("CompatibleSchemaVersion")]
		[Bindable(true)]
		public string CompatibleSchemaVersion 
		{
			get { return GetColumnValue<string>(Columns.CompatibleSchemaVersion); }
			set { SetColumnValue(Columns.CompatibleSchemaVersion, value); }
		}
		  
		[XmlAttribute("IsCurrentVersion")]
		[Bindable(true)]
		public bool IsCurrentVersion 
		{
			get { return GetColumnValue<bool>(Columns.IsCurrentVersion); }
			set { SetColumnValue(Columns.IsCurrentVersion, value); }
		}
		
		#endregion
		
		
			
		
		//no foreign key tables defined (0)
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(string varFeature,string varCompatibleSchemaVersion,bool varIsCurrentVersion)
		{
			AspnetSchemaVersion item = new AspnetSchemaVersion();
			
			item.Feature = varFeature;
			
			item.CompatibleSchemaVersion = varCompatibleSchemaVersion;
			
			item.IsCurrentVersion = varIsCurrentVersion;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(string varFeature,string varCompatibleSchemaVersion,bool varIsCurrentVersion)
		{
			AspnetSchemaVersion item = new AspnetSchemaVersion();
			
				item.Feature = varFeature;
			
				item.CompatibleSchemaVersion = varCompatibleSchemaVersion;
			
				item.IsCurrentVersion = varIsCurrentVersion;
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn FeatureColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn CompatibleSchemaVersionColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn IsCurrentVersionColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Feature = @"Feature";
			 public static string CompatibleSchemaVersion = @"CompatibleSchemaVersion";
			 public static string IsCurrentVersion = @"IsCurrentVersion";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
