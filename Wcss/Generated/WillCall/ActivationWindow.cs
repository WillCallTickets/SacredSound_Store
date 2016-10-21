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
	/// Strongly-typed collection for the ActivationWindow class.
	/// </summary>
    [Serializable]
	public partial class ActivationWindowCollection : ActiveList<ActivationWindow, ActivationWindowCollection>
	{	   
		public ActivationWindowCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>ActivationWindowCollection</returns>
		public ActivationWindowCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                ActivationWindow o = this[i];
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
	/// This is an ActiveRecord class which wraps the ActivationWindow table.
	/// </summary>
	[Serializable]
	public partial class ActivationWindow : ActiveRecord<ActivationWindow>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public ActivationWindow()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public ActivationWindow(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public ActivationWindow(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public ActivationWindow(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("ActivationWindow", TableType.Table, DataService.GetInstance("WillCall"));
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
				colvarDtStamp.ColumnName = "DtStamp";
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
				
				TableSchema.TableColumn colvarBUseCode = new TableSchema.TableColumn(schema);
				colvarBUseCode.ColumnName = "bUseCode";
				colvarBUseCode.DataType = DbType.Boolean;
				colvarBUseCode.MaxLength = 0;
				colvarBUseCode.AutoIncrement = false;
				colvarBUseCode.IsNullable = false;
				colvarBUseCode.IsPrimaryKey = false;
				colvarBUseCode.IsForeignKey = false;
				colvarBUseCode.IsReadOnly = false;
				
						colvarBUseCode.DefaultSetting = @"((0))";
				colvarBUseCode.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBUseCode);
				
				TableSchema.TableColumn colvarCode = new TableSchema.TableColumn(schema);
				colvarCode.ColumnName = "Code";
				colvarCode.DataType = DbType.AnsiString;
				colvarCode.MaxLength = 256;
				colvarCode.AutoIncrement = false;
				colvarCode.IsNullable = true;
				colvarCode.IsPrimaryKey = false;
				colvarCode.IsForeignKey = false;
				colvarCode.IsReadOnly = false;
				colvarCode.DefaultSetting = @"";
				colvarCode.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCode);
				
				TableSchema.TableColumn colvarDtCodeStart = new TableSchema.TableColumn(schema);
				colvarDtCodeStart.ColumnName = "dtCodeStart";
				colvarDtCodeStart.DataType = DbType.DateTime;
				colvarDtCodeStart.MaxLength = 0;
				colvarDtCodeStart.AutoIncrement = false;
				colvarDtCodeStart.IsNullable = true;
				colvarDtCodeStart.IsPrimaryKey = false;
				colvarDtCodeStart.IsForeignKey = false;
				colvarDtCodeStart.IsReadOnly = false;
				colvarDtCodeStart.DefaultSetting = @"";
				colvarDtCodeStart.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDtCodeStart);
				
				TableSchema.TableColumn colvarDtCodeEnd = new TableSchema.TableColumn(schema);
				colvarDtCodeEnd.ColumnName = "dtCodeEnd";
				colvarDtCodeEnd.DataType = DbType.DateTime;
				colvarDtCodeEnd.MaxLength = 0;
				colvarDtCodeEnd.AutoIncrement = false;
				colvarDtCodeEnd.IsNullable = true;
				colvarDtCodeEnd.IsPrimaryKey = false;
				colvarDtCodeEnd.IsForeignKey = false;
				colvarDtCodeEnd.IsReadOnly = false;
				colvarDtCodeEnd.DefaultSetting = @"";
				colvarDtCodeEnd.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDtCodeEnd);
				
				TableSchema.TableColumn colvarDtPublicStart = new TableSchema.TableColumn(schema);
				colvarDtPublicStart.ColumnName = "dtPublicStart";
				colvarDtPublicStart.DataType = DbType.DateTime;
				colvarDtPublicStart.MaxLength = 0;
				colvarDtPublicStart.AutoIncrement = false;
				colvarDtPublicStart.IsNullable = true;
				colvarDtPublicStart.IsPrimaryKey = false;
				colvarDtPublicStart.IsForeignKey = false;
				colvarDtPublicStart.IsReadOnly = false;
				colvarDtPublicStart.DefaultSetting = @"";
				colvarDtPublicStart.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDtPublicStart);
				
				TableSchema.TableColumn colvarDtPublicEnd = new TableSchema.TableColumn(schema);
				colvarDtPublicEnd.ColumnName = "dtPublicEnd";
				colvarDtPublicEnd.DataType = DbType.DateTime;
				colvarDtPublicEnd.MaxLength = 0;
				colvarDtPublicEnd.AutoIncrement = false;
				colvarDtPublicEnd.IsNullable = true;
				colvarDtPublicEnd.IsPrimaryKey = false;
				colvarDtPublicEnd.IsForeignKey = false;
				colvarDtPublicEnd.IsReadOnly = false;
				colvarDtPublicEnd.DefaultSetting = @"";
				colvarDtPublicEnd.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDtPublicEnd);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["WillCall"].AddSchema("ActivationWindow",schema);
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
		  
		[XmlAttribute("BUseCode")]
		[Bindable(true)]
		public bool BUseCode 
		{
			get { return GetColumnValue<bool>(Columns.BUseCode); }
			set { SetColumnValue(Columns.BUseCode, value); }
		}
		  
		[XmlAttribute("Code")]
		[Bindable(true)]
		public string Code 
		{
			get { return GetColumnValue<string>(Columns.Code); }
			set { SetColumnValue(Columns.Code, value); }
		}
		  
		[XmlAttribute("DtCodeStart")]
		[Bindable(true)]
		public DateTime? DtCodeStart 
		{
			get { return GetColumnValue<DateTime?>(Columns.DtCodeStart); }
			set { SetColumnValue(Columns.DtCodeStart, value); }
		}
		  
		[XmlAttribute("DtCodeEnd")]
		[Bindable(true)]
		public DateTime? DtCodeEnd 
		{
			get { return GetColumnValue<DateTime?>(Columns.DtCodeEnd); }
			set { SetColumnValue(Columns.DtCodeEnd, value); }
		}
		  
		[XmlAttribute("DtPublicStart")]
		[Bindable(true)]
		public DateTime? DtPublicStart 
		{
			get { return GetColumnValue<DateTime?>(Columns.DtPublicStart); }
			set { SetColumnValue(Columns.DtPublicStart, value); }
		}
		  
		[XmlAttribute("DtPublicEnd")]
		[Bindable(true)]
		public DateTime? DtPublicEnd 
		{
			get { return GetColumnValue<DateTime?>(Columns.DtPublicEnd); }
			set { SetColumnValue(Columns.DtPublicEnd, value); }
		}
		
		#endregion
		
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a AspnetApplication ActiveRecord object related to this ActivationWindow
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
		public static void Insert(DateTime varDtStamp,Guid varApplicationId,string varVcContext,int varTParentId,bool varBUseCode,string varCode,DateTime? varDtCodeStart,DateTime? varDtCodeEnd,DateTime? varDtPublicStart,DateTime? varDtPublicEnd)
		{
			ActivationWindow item = new ActivationWindow();
			
			item.DtStamp = varDtStamp;
			
			item.ApplicationId = varApplicationId;
			
			item.VcContext = varVcContext;
			
			item.TParentId = varTParentId;
			
			item.BUseCode = varBUseCode;
			
			item.Code = varCode;
			
			item.DtCodeStart = varDtCodeStart;
			
			item.DtCodeEnd = varDtCodeEnd;
			
			item.DtPublicStart = varDtPublicStart;
			
			item.DtPublicEnd = varDtPublicEnd;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varId,DateTime varDtStamp,Guid varApplicationId,string varVcContext,int varTParentId,bool varBUseCode,string varCode,DateTime? varDtCodeStart,DateTime? varDtCodeEnd,DateTime? varDtPublicStart,DateTime? varDtPublicEnd)
		{
			ActivationWindow item = new ActivationWindow();
			
				item.Id = varId;
			
				item.DtStamp = varDtStamp;
			
				item.ApplicationId = varApplicationId;
			
				item.VcContext = varVcContext;
			
				item.TParentId = varTParentId;
			
				item.BUseCode = varBUseCode;
			
				item.Code = varCode;
			
				item.DtCodeStart = varDtCodeStart;
			
				item.DtCodeEnd = varDtCodeEnd;
			
				item.DtPublicStart = varDtPublicStart;
			
				item.DtPublicEnd = varDtPublicEnd;
			
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
        
        
        
        public static TableSchema.TableColumn VcContextColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn TParentIdColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn BUseCodeColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn CodeColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn DtCodeStartColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn DtCodeEndColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn DtPublicStartColumn
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        public static TableSchema.TableColumn DtPublicEndColumn
        {
            get { return Schema.Columns[10]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Id = @"Id";
			 public static string DtStamp = @"DtStamp";
			 public static string ApplicationId = @"ApplicationId";
			 public static string VcContext = @"vcContext";
			 public static string TParentId = @"TParentId";
			 public static string BUseCode = @"bUseCode";
			 public static string Code = @"Code";
			 public static string DtCodeStart = @"dtCodeStart";
			 public static string DtCodeEnd = @"dtCodeEnd";
			 public static string DtPublicStart = @"dtPublicStart";
			 public static string DtPublicEnd = @"dtPublicEnd";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
