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
namespace Erlg
{
	/// <summary>
	/// Strongly-typed collection for the Log class.
	/// </summary>
    [Serializable]
	public partial class LogCollection : ActiveList<Log, LogCollection>
	{	   
		public LogCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>LogCollection</returns>
		public LogCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                Log o = this[i];
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
	/// This is an ActiveRecord class which wraps the Log table.
	/// </summary>
	[Serializable]
	public partial class Log : ActiveRecord<Log>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public Log()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public Log(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public Log(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public Log(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("Log", TableType.Table, DataService.GetInstance("ErrorLog"));
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
				
				TableSchema.TableColumn colvarDateX = new TableSchema.TableColumn(schema);
				colvarDateX.ColumnName = "Date";
				colvarDateX.DataType = DbType.DateTime;
				colvarDateX.MaxLength = 0;
				colvarDateX.AutoIncrement = false;
				colvarDateX.IsNullable = false;
				colvarDateX.IsPrimaryKey = false;
				colvarDateX.IsForeignKey = false;
				colvarDateX.IsReadOnly = false;
				colvarDateX.DefaultSetting = @"";
				colvarDateX.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDateX);
				
				TableSchema.TableColumn colvarSource = new TableSchema.TableColumn(schema);
				colvarSource.ColumnName = "Source";
				colvarSource.DataType = DbType.AnsiString;
				colvarSource.MaxLength = 50;
				colvarSource.AutoIncrement = false;
				colvarSource.IsNullable = true;
				colvarSource.IsPrimaryKey = false;
				colvarSource.IsForeignKey = false;
				colvarSource.IsReadOnly = false;
				colvarSource.DefaultSetting = @"";
				colvarSource.ForeignKeyTableName = "";
				schema.Columns.Add(colvarSource);
				
				TableSchema.TableColumn colvarMessage = new TableSchema.TableColumn(schema);
				colvarMessage.ColumnName = "Message";
				colvarMessage.DataType = DbType.AnsiString;
				colvarMessage.MaxLength = 2000;
				colvarMessage.AutoIncrement = false;
				colvarMessage.IsNullable = true;
				colvarMessage.IsPrimaryKey = false;
				colvarMessage.IsForeignKey = false;
				colvarMessage.IsReadOnly = false;
				colvarMessage.DefaultSetting = @"";
				colvarMessage.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMessage);
				
				TableSchema.TableColumn colvarForm = new TableSchema.TableColumn(schema);
				colvarForm.ColumnName = "Form";
				colvarForm.DataType = DbType.AnsiString;
				colvarForm.MaxLength = 256;
				colvarForm.AutoIncrement = false;
				colvarForm.IsNullable = true;
				colvarForm.IsPrimaryKey = false;
				colvarForm.IsForeignKey = false;
				colvarForm.IsReadOnly = false;
				colvarForm.DefaultSetting = @"";
				colvarForm.ForeignKeyTableName = "";
				schema.Columns.Add(colvarForm);
				
				TableSchema.TableColumn colvarQuerystring = new TableSchema.TableColumn(schema);
				colvarQuerystring.ColumnName = "Querystring";
				colvarQuerystring.DataType = DbType.AnsiString;
				colvarQuerystring.MaxLength = 256;
				colvarQuerystring.AutoIncrement = false;
				colvarQuerystring.IsNullable = true;
				colvarQuerystring.IsPrimaryKey = false;
				colvarQuerystring.IsForeignKey = false;
				colvarQuerystring.IsReadOnly = false;
				colvarQuerystring.DefaultSetting = @"";
				colvarQuerystring.ForeignKeyTableName = "";
				schema.Columns.Add(colvarQuerystring);
				
				TableSchema.TableColumn colvarTargetSite = new TableSchema.TableColumn(schema);
				colvarTargetSite.ColumnName = "TargetSite";
				colvarTargetSite.DataType = DbType.AnsiString;
				colvarTargetSite.MaxLength = 512;
				colvarTargetSite.AutoIncrement = false;
				colvarTargetSite.IsNullable = true;
				colvarTargetSite.IsPrimaryKey = false;
				colvarTargetSite.IsForeignKey = false;
				colvarTargetSite.IsReadOnly = false;
				colvarTargetSite.DefaultSetting = @"";
				colvarTargetSite.ForeignKeyTableName = "";
				schema.Columns.Add(colvarTargetSite);
				
				TableSchema.TableColumn colvarStackTrace = new TableSchema.TableColumn(schema);
				colvarStackTrace.ColumnName = "StackTrace";
				colvarStackTrace.DataType = DbType.AnsiString;
				colvarStackTrace.MaxLength = -1;
				colvarStackTrace.AutoIncrement = false;
				colvarStackTrace.IsNullable = true;
				colvarStackTrace.IsPrimaryKey = false;
				colvarStackTrace.IsForeignKey = false;
				colvarStackTrace.IsReadOnly = false;
				colvarStackTrace.DefaultSetting = @"";
				colvarStackTrace.ForeignKeyTableName = "";
				schema.Columns.Add(colvarStackTrace);
				
				TableSchema.TableColumn colvarReferrer = new TableSchema.TableColumn(schema);
				colvarReferrer.ColumnName = "Referrer";
				colvarReferrer.DataType = DbType.AnsiString;
				colvarReferrer.MaxLength = 512;
				colvarReferrer.AutoIncrement = false;
				colvarReferrer.IsNullable = true;
				colvarReferrer.IsPrimaryKey = false;
				colvarReferrer.IsForeignKey = false;
				colvarReferrer.IsReadOnly = false;
				colvarReferrer.DefaultSetting = @"";
				colvarReferrer.ForeignKeyTableName = "";
				schema.Columns.Add(colvarReferrer);
				
				TableSchema.TableColumn colvarIpAddress = new TableSchema.TableColumn(schema);
				colvarIpAddress.ColumnName = "IpAddress";
				colvarIpAddress.DataType = DbType.AnsiString;
				colvarIpAddress.MaxLength = 25;
				colvarIpAddress.AutoIncrement = false;
				colvarIpAddress.IsNullable = true;
				colvarIpAddress.IsPrimaryKey = false;
				colvarIpAddress.IsForeignKey = false;
				colvarIpAddress.IsReadOnly = false;
				colvarIpAddress.DefaultSetting = @"";
				colvarIpAddress.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIpAddress);
				
				TableSchema.TableColumn colvarEmail = new TableSchema.TableColumn(schema);
				colvarEmail.ColumnName = "Email";
				colvarEmail.DataType = DbType.AnsiString;
				colvarEmail.MaxLength = 256;
				colvarEmail.AutoIncrement = false;
				colvarEmail.IsNullable = true;
				colvarEmail.IsPrimaryKey = false;
				colvarEmail.IsForeignKey = false;
				colvarEmail.IsReadOnly = false;
				colvarEmail.DefaultSetting = @"";
				colvarEmail.ForeignKeyTableName = "";
				schema.Columns.Add(colvarEmail);
				
				TableSchema.TableColumn colvarApplicationName = new TableSchema.TableColumn(schema);
				colvarApplicationName.ColumnName = "ApplicationName";
				colvarApplicationName.DataType = DbType.AnsiString;
				colvarApplicationName.MaxLength = 25;
				colvarApplicationName.AutoIncrement = false;
				colvarApplicationName.IsNullable = false;
				colvarApplicationName.IsPrimaryKey = false;
				colvarApplicationName.IsForeignKey = false;
				colvarApplicationName.IsReadOnly = false;
				colvarApplicationName.DefaultSetting = @"";
				colvarApplicationName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarApplicationName);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["ErrorLog"].AddSchema("Log",schema);
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
		  
		[XmlAttribute("DateX")]
		[Bindable(true)]
		public DateTime DateX 
		{
			get { return GetColumnValue<DateTime>(Columns.DateX); }
			set { SetColumnValue(Columns.DateX, value); }
		}
		  
		[XmlAttribute("Source")]
		[Bindable(true)]
		public string Source 
		{
			get { return GetColumnValue<string>(Columns.Source); }
			set { SetColumnValue(Columns.Source, value); }
		}
		  
		[XmlAttribute("Message")]
		[Bindable(true)]
		public string Message 
		{
			get { return GetColumnValue<string>(Columns.Message); }
			set { SetColumnValue(Columns.Message, value); }
		}
		  
		[XmlAttribute("Form")]
		[Bindable(true)]
		public string Form 
		{
			get { return GetColumnValue<string>(Columns.Form); }
			set { SetColumnValue(Columns.Form, value); }
		}
		  
		[XmlAttribute("Querystring")]
		[Bindable(true)]
		public string Querystring 
		{
			get { return GetColumnValue<string>(Columns.Querystring); }
			set { SetColumnValue(Columns.Querystring, value); }
		}
		  
		[XmlAttribute("TargetSite")]
		[Bindable(true)]
		public string TargetSite 
		{
			get { return GetColumnValue<string>(Columns.TargetSite); }
			set { SetColumnValue(Columns.TargetSite, value); }
		}
		  
		[XmlAttribute("StackTrace")]
		[Bindable(true)]
		public string StackTrace 
		{
			get { return GetColumnValue<string>(Columns.StackTrace); }
			set { SetColumnValue(Columns.StackTrace, value); }
		}
		  
		[XmlAttribute("Referrer")]
		[Bindable(true)]
		public string Referrer 
		{
			get { return GetColumnValue<string>(Columns.Referrer); }
			set { SetColumnValue(Columns.Referrer, value); }
		}
		  
		[XmlAttribute("IpAddress")]
		[Bindable(true)]
		public string IpAddress 
		{
			get { return GetColumnValue<string>(Columns.IpAddress); }
			set { SetColumnValue(Columns.IpAddress, value); }
		}
		  
		[XmlAttribute("Email")]
		[Bindable(true)]
		public string Email 
		{
			get { return GetColumnValue<string>(Columns.Email); }
			set { SetColumnValue(Columns.Email, value); }
		}
		  
		[XmlAttribute("ApplicationName")]
		[Bindable(true)]
		public string ApplicationName 
		{
			get { return GetColumnValue<string>(Columns.ApplicationName); }
			set { SetColumnValue(Columns.ApplicationName, value); }
		}
		
		#endregion
		
		
			
		
		//no foreign key tables defined (0)
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(DateTime varDateX,string varSource,string varMessage,string varForm,string varQuerystring,string varTargetSite,string varStackTrace,string varReferrer,string varIpAddress,string varEmail,string varApplicationName)
		{
			Log item = new Log();
			
			item.DateX = varDateX;
			
			item.Source = varSource;
			
			item.Message = varMessage;
			
			item.Form = varForm;
			
			item.Querystring = varQuerystring;
			
			item.TargetSite = varTargetSite;
			
			item.StackTrace = varStackTrace;
			
			item.Referrer = varReferrer;
			
			item.IpAddress = varIpAddress;
			
			item.Email = varEmail;
			
			item.ApplicationName = varApplicationName;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varId,DateTime varDateX,string varSource,string varMessage,string varForm,string varQuerystring,string varTargetSite,string varStackTrace,string varReferrer,string varIpAddress,string varEmail,string varApplicationName)
		{
			Log item = new Log();
			
				item.Id = varId;
			
				item.DateX = varDateX;
			
				item.Source = varSource;
			
				item.Message = varMessage;
			
				item.Form = varForm;
			
				item.Querystring = varQuerystring;
			
				item.TargetSite = varTargetSite;
			
				item.StackTrace = varStackTrace;
			
				item.Referrer = varReferrer;
			
				item.IpAddress = varIpAddress;
			
				item.Email = varEmail;
			
				item.ApplicationName = varApplicationName;
			
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
        
        
        
        public static TableSchema.TableColumn DateXColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn SourceColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn MessageColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn FormColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn QuerystringColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn TargetSiteColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn StackTraceColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn ReferrerColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn IpAddressColumn
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        public static TableSchema.TableColumn EmailColumn
        {
            get { return Schema.Columns[10]; }
        }
        
        
        
        public static TableSchema.TableColumn ApplicationNameColumn
        {
            get { return Schema.Columns[11]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Id = @"Id";
			 public static string DateX = @"Date";
			 public static string Source = @"Source";
			 public static string Message = @"Message";
			 public static string Form = @"Form";
			 public static string Querystring = @"Querystring";
			 public static string TargetSite = @"TargetSite";
			 public static string StackTrace = @"StackTrace";
			 public static string Referrer = @"Referrer";
			 public static string IpAddress = @"IpAddress";
			 public static string Email = @"Email";
			 public static string ApplicationName = @"ApplicationName";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
