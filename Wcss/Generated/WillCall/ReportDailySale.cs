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
	/// Strongly-typed collection for the ReportDailySale class.
	/// </summary>
    [Serializable]
	public partial class ReportDailySaleCollection : ActiveList<ReportDailySale, ReportDailySaleCollection>
	{	   
		public ReportDailySaleCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>ReportDailySaleCollection</returns>
		public ReportDailySaleCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                ReportDailySale o = this[i];
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
	/// This is an ActiveRecord class which wraps the Report_DailySales table.
	/// </summary>
	[Serializable]
	public partial class ReportDailySale : ActiveRecord<ReportDailySale>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public ReportDailySale()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public ReportDailySale(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public ReportDailySale(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public ReportDailySale(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("Report_DailySales", TableType.Table, DataService.GetInstance("WillCall"));
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
				colvarDtStamp.IsNullable = true;
				colvarDtStamp.IsPrimaryKey = false;
				colvarDtStamp.IsForeignKey = false;
				colvarDtStamp.IsReadOnly = false;
				
						colvarDtStamp.DefaultSetting = @"(getdate())";
				colvarDtStamp.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDtStamp);
				
				TableSchema.TableColumn colvarReportDate = new TableSchema.TableColumn(schema);
				colvarReportDate.ColumnName = "ReportDate";
				colvarReportDate.DataType = DbType.DateTime;
				colvarReportDate.MaxLength = 0;
				colvarReportDate.AutoIncrement = false;
				colvarReportDate.IsNullable = false;
				colvarReportDate.IsPrimaryKey = false;
				colvarReportDate.IsForeignKey = false;
				colvarReportDate.IsReadOnly = false;
				colvarReportDate.DefaultSetting = @"";
				colvarReportDate.ForeignKeyTableName = "";
				schema.Columns.Add(colvarReportDate);
				
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
				
				TableSchema.TableColumn colvarItemId = new TableSchema.TableColumn(schema);
				colvarItemId.ColumnName = "ItemId";
				colvarItemId.DataType = DbType.Int32;
				colvarItemId.MaxLength = 0;
				colvarItemId.AutoIncrement = false;
				colvarItemId.IsNullable = false;
				colvarItemId.IsPrimaryKey = false;
				colvarItemId.IsForeignKey = false;
				colvarItemId.IsReadOnly = false;
				colvarItemId.DefaultSetting = @"";
				colvarItemId.ForeignKeyTableName = "";
				schema.Columns.Add(colvarItemId);
				
				TableSchema.TableColumn colvarDescription = new TableSchema.TableColumn(schema);
				colvarDescription.ColumnName = "Description";
				colvarDescription.DataType = DbType.AnsiString;
				colvarDescription.MaxLength = 1000;
				colvarDescription.AutoIncrement = false;
				colvarDescription.IsNullable = true;
				colvarDescription.IsPrimaryKey = false;
				colvarDescription.IsForeignKey = false;
				colvarDescription.IsReadOnly = false;
				colvarDescription.DefaultSetting = @"";
				colvarDescription.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDescription);
				
				TableSchema.TableColumn colvarMiniDesc = new TableSchema.TableColumn(schema);
				colvarMiniDesc.ColumnName = "MiniDesc";
				colvarMiniDesc.DataType = DbType.AnsiString;
				colvarMiniDesc.MaxLength = 500;
				colvarMiniDesc.AutoIncrement = false;
				colvarMiniDesc.IsNullable = true;
				colvarMiniDesc.IsPrimaryKey = false;
				colvarMiniDesc.IsForeignKey = false;
				colvarMiniDesc.IsReadOnly = false;
				colvarMiniDesc.DefaultSetting = @"";
				colvarMiniDesc.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMiniDesc);
				
				TableSchema.TableColumn colvarAlloted = new TableSchema.TableColumn(schema);
				colvarAlloted.ColumnName = "Alloted";
				colvarAlloted.DataType = DbType.Int32;
				colvarAlloted.MaxLength = 0;
				colvarAlloted.AutoIncrement = false;
				colvarAlloted.IsNullable = false;
				colvarAlloted.IsPrimaryKey = false;
				colvarAlloted.IsForeignKey = false;
				colvarAlloted.IsReadOnly = false;
				
						colvarAlloted.DefaultSetting = @"((0))";
				colvarAlloted.ForeignKeyTableName = "";
				schema.Columns.Add(colvarAlloted);
				
				TableSchema.TableColumn colvarSold = new TableSchema.TableColumn(schema);
				colvarSold.ColumnName = "Sold";
				colvarSold.DataType = DbType.Int32;
				colvarSold.MaxLength = 0;
				colvarSold.AutoIncrement = false;
				colvarSold.IsNullable = false;
				colvarSold.IsPrimaryKey = false;
				colvarSold.IsForeignKey = false;
				colvarSold.IsReadOnly = false;
				colvarSold.DefaultSetting = @"";
				colvarSold.ForeignKeyTableName = "";
				schema.Columns.Add(colvarSold);
				
				TableSchema.TableColumn colvarTotalSold = new TableSchema.TableColumn(schema);
				colvarTotalSold.ColumnName = "TotalSold";
				colvarTotalSold.DataType = DbType.Int32;
				colvarTotalSold.MaxLength = 0;
				colvarTotalSold.AutoIncrement = false;
				colvarTotalSold.IsNullable = false;
				colvarTotalSold.IsPrimaryKey = false;
				colvarTotalSold.IsForeignKey = false;
				colvarTotalSold.IsReadOnly = false;
				
						colvarTotalSold.DefaultSetting = @"((0))";
				colvarTotalSold.ForeignKeyTableName = "";
				schema.Columns.Add(colvarTotalSold);
				
				TableSchema.TableColumn colvarAvailable = new TableSchema.TableColumn(schema);
				colvarAvailable.ColumnName = "Available";
				colvarAvailable.DataType = DbType.Int32;
				colvarAvailable.MaxLength = 0;
				colvarAvailable.AutoIncrement = false;
				colvarAvailable.IsNullable = false;
				colvarAvailable.IsPrimaryKey = false;
				colvarAvailable.IsForeignKey = false;
				colvarAvailable.IsReadOnly = false;
				colvarAvailable.DefaultSetting = @"";
				colvarAvailable.ForeignKeyTableName = "";
				schema.Columns.Add(colvarAvailable);
				
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
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["WillCall"].AddSchema("Report_DailySales",schema);
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
		public DateTime? DtStamp 
		{
			get { return GetColumnValue<DateTime?>(Columns.DtStamp); }
			set { SetColumnValue(Columns.DtStamp, value); }
		}
		  
		[XmlAttribute("ReportDate")]
		[Bindable(true)]
		public DateTime ReportDate 
		{
			get { return GetColumnValue<DateTime>(Columns.ReportDate); }
			set { SetColumnValue(Columns.ReportDate, value); }
		}
		  
		[XmlAttribute("VcContext")]
		[Bindable(true)]
		public string VcContext 
		{
			get { return GetColumnValue<string>(Columns.VcContext); }
			set { SetColumnValue(Columns.VcContext, value); }
		}
		  
		[XmlAttribute("ItemId")]
		[Bindable(true)]
		public int ItemId 
		{
			get { return GetColumnValue<int>(Columns.ItemId); }
			set { SetColumnValue(Columns.ItemId, value); }
		}
		  
		[XmlAttribute("Description")]
		[Bindable(true)]
		public string Description 
		{
			get { return GetColumnValue<string>(Columns.Description); }
			set { SetColumnValue(Columns.Description, value); }
		}
		  
		[XmlAttribute("MiniDesc")]
		[Bindable(true)]
		public string MiniDesc 
		{
			get { return GetColumnValue<string>(Columns.MiniDesc); }
			set { SetColumnValue(Columns.MiniDesc, value); }
		}
		  
		[XmlAttribute("Alloted")]
		[Bindable(true)]
		public int Alloted 
		{
			get { return GetColumnValue<int>(Columns.Alloted); }
			set { SetColumnValue(Columns.Alloted, value); }
		}
		  
		[XmlAttribute("Sold")]
		[Bindable(true)]
		public int Sold 
		{
			get { return GetColumnValue<int>(Columns.Sold); }
			set { SetColumnValue(Columns.Sold, value); }
		}
		  
		[XmlAttribute("TotalSold")]
		[Bindable(true)]
		public int TotalSold 
		{
			get { return GetColumnValue<int>(Columns.TotalSold); }
			set { SetColumnValue(Columns.TotalSold, value); }
		}
		  
		[XmlAttribute("Available")]
		[Bindable(true)]
		public int Available 
		{
			get { return GetColumnValue<int>(Columns.Available); }
			set { SetColumnValue(Columns.Available, value); }
		}
		  
		[XmlAttribute("ApplicationId")]
		[Bindable(true)]
		public Guid ApplicationId 
		{
			get { return GetColumnValue<Guid>(Columns.ApplicationId); }
			set { SetColumnValue(Columns.ApplicationId, value); }
		}
		
		#endregion
		
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a AspnetApplication ActiveRecord object related to this ReportDailySale
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
		public static void Insert(DateTime? varDtStamp,DateTime varReportDate,string varVcContext,int varItemId,string varDescription,string varMiniDesc,int varAlloted,int varSold,int varTotalSold,int varAvailable,Guid varApplicationId)
		{
			ReportDailySale item = new ReportDailySale();
			
			item.DtStamp = varDtStamp;
			
			item.ReportDate = varReportDate;
			
			item.VcContext = varVcContext;
			
			item.ItemId = varItemId;
			
			item.Description = varDescription;
			
			item.MiniDesc = varMiniDesc;
			
			item.Alloted = varAlloted;
			
			item.Sold = varSold;
			
			item.TotalSold = varTotalSold;
			
			item.Available = varAvailable;
			
			item.ApplicationId = varApplicationId;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varId,DateTime? varDtStamp,DateTime varReportDate,string varVcContext,int varItemId,string varDescription,string varMiniDesc,int varAlloted,int varSold,int varTotalSold,int varAvailable,Guid varApplicationId)
		{
			ReportDailySale item = new ReportDailySale();
			
				item.Id = varId;
			
				item.DtStamp = varDtStamp;
			
				item.ReportDate = varReportDate;
			
				item.VcContext = varVcContext;
			
				item.ItemId = varItemId;
			
				item.Description = varDescription;
			
				item.MiniDesc = varMiniDesc;
			
				item.Alloted = varAlloted;
			
				item.Sold = varSold;
			
				item.TotalSold = varTotalSold;
			
				item.Available = varAvailable;
			
				item.ApplicationId = varApplicationId;
			
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
        
        
        
        public static TableSchema.TableColumn ReportDateColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn VcContextColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn ItemIdColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn DescriptionColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn MiniDescColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn AllotedColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn SoldColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn TotalSoldColumn
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        public static TableSchema.TableColumn AvailableColumn
        {
            get { return Schema.Columns[10]; }
        }
        
        
        
        public static TableSchema.TableColumn ApplicationIdColumn
        {
            get { return Schema.Columns[11]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Id = @"Id";
			 public static string DtStamp = @"dtStamp";
			 public static string ReportDate = @"ReportDate";
			 public static string VcContext = @"vcContext";
			 public static string ItemId = @"ItemId";
			 public static string Description = @"Description";
			 public static string MiniDesc = @"MiniDesc";
			 public static string Alloted = @"Alloted";
			 public static string Sold = @"Sold";
			 public static string TotalSold = @"TotalSold";
			 public static string Available = @"Available";
			 public static string ApplicationId = @"ApplicationId";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
