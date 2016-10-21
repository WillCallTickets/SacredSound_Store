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
	/// Strongly-typed collection for the ServiceCharge class.
	/// </summary>
    [Serializable]
	public partial class ServiceChargeCollection : ActiveList<ServiceCharge, ServiceChargeCollection>
	{	   
		public ServiceChargeCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>ServiceChargeCollection</returns>
		public ServiceChargeCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                ServiceCharge o = this[i];
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
	/// This is an ActiveRecord class which wraps the ServiceCharge table.
	/// </summary>
	[Serializable]
	public partial class ServiceCharge : ActiveRecord<ServiceCharge>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public ServiceCharge()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public ServiceCharge(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public ServiceCharge(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public ServiceCharge(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("ServiceCharge", TableType.Table, DataService.GetInstance("WillCall"));
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
				
				TableSchema.TableColumn colvarMMaxValue = new TableSchema.TableColumn(schema);
				colvarMMaxValue.ColumnName = "mMaxValue";
				colvarMMaxValue.DataType = DbType.Currency;
				colvarMMaxValue.MaxLength = 0;
				colvarMMaxValue.AutoIncrement = false;
				colvarMMaxValue.IsNullable = false;
				colvarMMaxValue.IsPrimaryKey = false;
				colvarMMaxValue.IsForeignKey = false;
				colvarMMaxValue.IsReadOnly = false;
				colvarMMaxValue.DefaultSetting = @"";
				colvarMMaxValue.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMMaxValue);
				
				TableSchema.TableColumn colvarMCharge = new TableSchema.TableColumn(schema);
				colvarMCharge.ColumnName = "mCharge";
				colvarMCharge.DataType = DbType.Currency;
				colvarMCharge.MaxLength = 0;
				colvarMCharge.AutoIncrement = false;
				colvarMCharge.IsNullable = false;
				colvarMCharge.IsPrimaryKey = false;
				colvarMCharge.IsForeignKey = false;
				colvarMCharge.IsReadOnly = false;
				
						colvarMCharge.DefaultSetting = @"((0))";
				colvarMCharge.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMCharge);
				
				TableSchema.TableColumn colvarMPercentage = new TableSchema.TableColumn(schema);
				colvarMPercentage.ColumnName = "mPercentage";
				colvarMPercentage.DataType = DbType.Currency;
				colvarMPercentage.MaxLength = 0;
				colvarMPercentage.AutoIncrement = false;
				colvarMPercentage.IsNullable = false;
				colvarMPercentage.IsPrimaryKey = false;
				colvarMPercentage.IsForeignKey = false;
				colvarMPercentage.IsReadOnly = false;
				
						colvarMPercentage.DefaultSetting = @"((0))";
				colvarMPercentage.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMPercentage);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["WillCall"].AddSchema("ServiceCharge",schema);
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
		  
		[XmlAttribute("MMaxValue")]
		[Bindable(true)]
		public decimal MMaxValue 
		{
			get { return GetColumnValue<decimal>(Columns.MMaxValue); }
			set { SetColumnValue(Columns.MMaxValue, value); }
		}
		  
		[XmlAttribute("MCharge")]
		[Bindable(true)]
		public decimal MCharge 
		{
			get { return GetColumnValue<decimal>(Columns.MCharge); }
			set { SetColumnValue(Columns.MCharge, value); }
		}
		  
		[XmlAttribute("MPercentage")]
		[Bindable(true)]
		public decimal MPercentage 
		{
			get { return GetColumnValue<decimal>(Columns.MPercentage); }
			set { SetColumnValue(Columns.MPercentage, value); }
		}
		
		#endregion
		
		
			
		
		//no foreign key tables defined (0)
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(DateTime varDtStamp,Guid varApplicationId,decimal varMMaxValue,decimal varMCharge,decimal varMPercentage)
		{
			ServiceCharge item = new ServiceCharge();
			
			item.DtStamp = varDtStamp;
			
			item.ApplicationId = varApplicationId;
			
			item.MMaxValue = varMMaxValue;
			
			item.MCharge = varMCharge;
			
			item.MPercentage = varMPercentage;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varId,DateTime varDtStamp,Guid varApplicationId,decimal varMMaxValue,decimal varMCharge,decimal varMPercentage)
		{
			ServiceCharge item = new ServiceCharge();
			
				item.Id = varId;
			
				item.DtStamp = varDtStamp;
			
				item.ApplicationId = varApplicationId;
			
				item.MMaxValue = varMMaxValue;
			
				item.MCharge = varMCharge;
			
				item.MPercentage = varMPercentage;
			
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
        
        
        
        public static TableSchema.TableColumn MMaxValueColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn MChargeColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn MPercentageColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Id = @"Id";
			 public static string DtStamp = @"dtStamp";
			 public static string ApplicationId = @"ApplicationId";
			 public static string MMaxValue = @"mMaxValue";
			 public static string MCharge = @"mCharge";
			 public static string MPercentage = @"mPercentage";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
