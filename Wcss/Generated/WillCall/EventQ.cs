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
	/// Strongly-typed collection for the EventQ class.
	/// </summary>
    [Serializable]
	public partial class EventQCollection : ActiveList<EventQ, EventQCollection>
	{	   
		public EventQCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>EventQCollection</returns>
		public EventQCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                EventQ o = this[i];
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
	/// This is an ActiveRecord class which wraps the EventQ table.
	/// </summary>
	[Serializable]
	public partial class EventQ : ActiveRecord<EventQ>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public EventQ()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public EventQ(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public EventQ(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public EventQ(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("EventQ", TableType.Table, DataService.GetInstance("WillCall"));
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
				
				TableSchema.TableColumn colvarDateToProcess = new TableSchema.TableColumn(schema);
				colvarDateToProcess.ColumnName = "DateToProcess";
				colvarDateToProcess.DataType = DbType.DateTime;
				colvarDateToProcess.MaxLength = 0;
				colvarDateToProcess.AutoIncrement = false;
				colvarDateToProcess.IsNullable = true;
				colvarDateToProcess.IsPrimaryKey = false;
				colvarDateToProcess.IsForeignKey = false;
				colvarDateToProcess.IsReadOnly = false;
				colvarDateToProcess.DefaultSetting = @"";
				colvarDateToProcess.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDateToProcess);
				
				TableSchema.TableColumn colvarDateProcessed = new TableSchema.TableColumn(schema);
				colvarDateProcessed.ColumnName = "DateProcessed";
				colvarDateProcessed.DataType = DbType.DateTime;
				colvarDateProcessed.MaxLength = 0;
				colvarDateProcessed.AutoIncrement = false;
				colvarDateProcessed.IsNullable = true;
				colvarDateProcessed.IsPrimaryKey = false;
				colvarDateProcessed.IsForeignKey = false;
				colvarDateProcessed.IsReadOnly = false;
				colvarDateProcessed.DefaultSetting = @"";
				colvarDateProcessed.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDateProcessed);
				
				TableSchema.TableColumn colvarStatus = new TableSchema.TableColumn(schema);
				colvarStatus.ColumnName = "Status";
				colvarStatus.DataType = DbType.AnsiString;
				colvarStatus.MaxLength = 2000;
				colvarStatus.AutoIncrement = false;
				colvarStatus.IsNullable = true;
				colvarStatus.IsPrimaryKey = false;
				colvarStatus.IsForeignKey = false;
				colvarStatus.IsReadOnly = false;
				colvarStatus.DefaultSetting = @"";
				colvarStatus.ForeignKeyTableName = "";
				schema.Columns.Add(colvarStatus);
				
				TableSchema.TableColumn colvarThreadLock = new TableSchema.TableColumn(schema);
				colvarThreadLock.ColumnName = "ThreadLock";
				colvarThreadLock.DataType = DbType.Guid;
				colvarThreadLock.MaxLength = 0;
				colvarThreadLock.AutoIncrement = false;
				colvarThreadLock.IsNullable = true;
				colvarThreadLock.IsPrimaryKey = false;
				colvarThreadLock.IsForeignKey = false;
				colvarThreadLock.IsReadOnly = false;
				colvarThreadLock.DefaultSetting = @"";
				colvarThreadLock.ForeignKeyTableName = "";
				schema.Columns.Add(colvarThreadLock);
				
				TableSchema.TableColumn colvarAttemptsRemaining = new TableSchema.TableColumn(schema);
				colvarAttemptsRemaining.ColumnName = "AttemptsRemaining";
				colvarAttemptsRemaining.DataType = DbType.Int32;
				colvarAttemptsRemaining.MaxLength = 0;
				colvarAttemptsRemaining.AutoIncrement = false;
				colvarAttemptsRemaining.IsNullable = true;
				colvarAttemptsRemaining.IsPrimaryKey = false;
				colvarAttemptsRemaining.IsForeignKey = false;
				colvarAttemptsRemaining.IsReadOnly = false;
				
						colvarAttemptsRemaining.DefaultSetting = @"((3))";
				colvarAttemptsRemaining.ForeignKeyTableName = "";
				schema.Columns.Add(colvarAttemptsRemaining);
				
				TableSchema.TableColumn colvarIPriority = new TableSchema.TableColumn(schema);
				colvarIPriority.ColumnName = "iPriority";
				colvarIPriority.DataType = DbType.Int32;
				colvarIPriority.MaxLength = 0;
				colvarIPriority.AutoIncrement = false;
				colvarIPriority.IsNullable = false;
				colvarIPriority.IsPrimaryKey = false;
				colvarIPriority.IsForeignKey = false;
				colvarIPriority.IsReadOnly = false;
				
						colvarIPriority.DefaultSetting = @"((0))";
				colvarIPriority.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIPriority);
				
				TableSchema.TableColumn colvarCreatorId = new TableSchema.TableColumn(schema);
				colvarCreatorId.ColumnName = "CreatorId";
				colvarCreatorId.DataType = DbType.Guid;
				colvarCreatorId.MaxLength = 0;
				colvarCreatorId.AutoIncrement = false;
				colvarCreatorId.IsNullable = true;
				colvarCreatorId.IsPrimaryKey = false;
				colvarCreatorId.IsForeignKey = false;
				colvarCreatorId.IsReadOnly = false;
				colvarCreatorId.DefaultSetting = @"";
				colvarCreatorId.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCreatorId);
				
				TableSchema.TableColumn colvarCreatorName = new TableSchema.TableColumn(schema);
				colvarCreatorName.ColumnName = "CreatorName";
				colvarCreatorName.DataType = DbType.AnsiString;
				colvarCreatorName.MaxLength = 256;
				colvarCreatorName.AutoIncrement = false;
				colvarCreatorName.IsNullable = true;
				colvarCreatorName.IsPrimaryKey = false;
				colvarCreatorName.IsForeignKey = false;
				colvarCreatorName.IsReadOnly = false;
				colvarCreatorName.DefaultSetting = @"";
				colvarCreatorName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCreatorName);
				
				TableSchema.TableColumn colvarUserId = new TableSchema.TableColumn(schema);
				colvarUserId.ColumnName = "UserId";
				colvarUserId.DataType = DbType.Guid;
				colvarUserId.MaxLength = 0;
				colvarUserId.AutoIncrement = false;
				colvarUserId.IsNullable = true;
				colvarUserId.IsPrimaryKey = false;
				colvarUserId.IsForeignKey = false;
				colvarUserId.IsReadOnly = false;
				colvarUserId.DefaultSetting = @"";
				colvarUserId.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserId);
				
				TableSchema.TableColumn colvarUserName = new TableSchema.TableColumn(schema);
				colvarUserName.ColumnName = "UserName";
				colvarUserName.DataType = DbType.AnsiString;
				colvarUserName.MaxLength = 256;
				colvarUserName.AutoIncrement = false;
				colvarUserName.IsNullable = true;
				colvarUserName.IsPrimaryKey = false;
				colvarUserName.IsForeignKey = false;
				colvarUserName.IsReadOnly = false;
				colvarUserName.DefaultSetting = @"";
				colvarUserName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserName);
				
				TableSchema.TableColumn colvarContext = new TableSchema.TableColumn(schema);
				colvarContext.ColumnName = "Context";
				colvarContext.DataType = DbType.AnsiString;
				colvarContext.MaxLength = 50;
				colvarContext.AutoIncrement = false;
				colvarContext.IsNullable = false;
				colvarContext.IsPrimaryKey = false;
				colvarContext.IsForeignKey = false;
				colvarContext.IsReadOnly = false;
				colvarContext.DefaultSetting = @"";
				colvarContext.ForeignKeyTableName = "";
				schema.Columns.Add(colvarContext);
				
				TableSchema.TableColumn colvarVerb = new TableSchema.TableColumn(schema);
				colvarVerb.ColumnName = "Verb";
				colvarVerb.DataType = DbType.AnsiString;
				colvarVerb.MaxLength = 50;
				colvarVerb.AutoIncrement = false;
				colvarVerb.IsNullable = false;
				colvarVerb.IsPrimaryKey = false;
				colvarVerb.IsForeignKey = false;
				colvarVerb.IsReadOnly = false;
				colvarVerb.DefaultSetting = @"";
				colvarVerb.ForeignKeyTableName = "";
				schema.Columns.Add(colvarVerb);
				
				TableSchema.TableColumn colvarOldValue = new TableSchema.TableColumn(schema);
				colvarOldValue.ColumnName = "OldValue";
				colvarOldValue.DataType = DbType.AnsiString;
				colvarOldValue.MaxLength = 1500;
				colvarOldValue.AutoIncrement = false;
				colvarOldValue.IsNullable = true;
				colvarOldValue.IsPrimaryKey = false;
				colvarOldValue.IsForeignKey = false;
				colvarOldValue.IsReadOnly = false;
				colvarOldValue.DefaultSetting = @"";
				colvarOldValue.ForeignKeyTableName = "";
				schema.Columns.Add(colvarOldValue);
				
				TableSchema.TableColumn colvarNewValue = new TableSchema.TableColumn(schema);
				colvarNewValue.ColumnName = "NewValue";
				colvarNewValue.DataType = DbType.AnsiString;
				colvarNewValue.MaxLength = 1500;
				colvarNewValue.AutoIncrement = false;
				colvarNewValue.IsNullable = true;
				colvarNewValue.IsPrimaryKey = false;
				colvarNewValue.IsForeignKey = false;
				colvarNewValue.IsReadOnly = false;
				colvarNewValue.DefaultSetting = @"";
				colvarNewValue.ForeignKeyTableName = "";
				schema.Columns.Add(colvarNewValue);
				
				TableSchema.TableColumn colvarDescription = new TableSchema.TableColumn(schema);
				colvarDescription.ColumnName = "Description";
				colvarDescription.DataType = DbType.AnsiString;
				colvarDescription.MaxLength = 2000;
				colvarDescription.AutoIncrement = false;
				colvarDescription.IsNullable = true;
				colvarDescription.IsPrimaryKey = false;
				colvarDescription.IsForeignKey = false;
				colvarDescription.IsReadOnly = false;
				colvarDescription.DefaultSetting = @"";
				colvarDescription.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDescription);
				
				TableSchema.TableColumn colvarIp = new TableSchema.TableColumn(schema);
				colvarIp.ColumnName = "IP";
				colvarIp.DataType = DbType.AnsiString;
				colvarIp.MaxLength = 20;
				colvarIp.AutoIncrement = false;
				colvarIp.IsNullable = true;
				colvarIp.IsPrimaryKey = false;
				colvarIp.IsForeignKey = false;
				colvarIp.IsReadOnly = false;
				colvarIp.DefaultSetting = @"";
				colvarIp.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIp);
				
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
				DataService.Providers["WillCall"].AddSchema("EventQ",schema);
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
		  
		[XmlAttribute("DateToProcess")]
		[Bindable(true)]
		public DateTime? DateToProcess 
		{
			get { return GetColumnValue<DateTime?>(Columns.DateToProcess); }
			set { SetColumnValue(Columns.DateToProcess, value); }
		}
		  
		[XmlAttribute("DateProcessed")]
		[Bindable(true)]
		public DateTime? DateProcessed 
		{
			get { return GetColumnValue<DateTime?>(Columns.DateProcessed); }
			set { SetColumnValue(Columns.DateProcessed, value); }
		}
		  
		[XmlAttribute("Status")]
		[Bindable(true)]
		public string Status 
		{
			get { return GetColumnValue<string>(Columns.Status); }
			set { SetColumnValue(Columns.Status, value); }
		}
		  
		[XmlAttribute("ThreadLock")]
		[Bindable(true)]
		public Guid? ThreadLock 
		{
			get { return GetColumnValue<Guid?>(Columns.ThreadLock); }
			set { SetColumnValue(Columns.ThreadLock, value); }
		}
		  
		[XmlAttribute("AttemptsRemaining")]
		[Bindable(true)]
		public int? AttemptsRemaining 
		{
			get { return GetColumnValue<int?>(Columns.AttemptsRemaining); }
			set { SetColumnValue(Columns.AttemptsRemaining, value); }
		}
		  
		[XmlAttribute("IPriority")]
		[Bindable(true)]
		public int IPriority 
		{
			get { return GetColumnValue<int>(Columns.IPriority); }
			set { SetColumnValue(Columns.IPriority, value); }
		}
		  
		[XmlAttribute("CreatorId")]
		[Bindable(true)]
		public Guid? CreatorId 
		{
			get { return GetColumnValue<Guid?>(Columns.CreatorId); }
			set { SetColumnValue(Columns.CreatorId, value); }
		}
		  
		[XmlAttribute("CreatorName")]
		[Bindable(true)]
		public string CreatorName 
		{
			get { return GetColumnValue<string>(Columns.CreatorName); }
			set { SetColumnValue(Columns.CreatorName, value); }
		}
		  
		[XmlAttribute("UserId")]
		[Bindable(true)]
		public Guid? UserId 
		{
			get { return GetColumnValue<Guid?>(Columns.UserId); }
			set { SetColumnValue(Columns.UserId, value); }
		}
		  
		[XmlAttribute("UserName")]
		[Bindable(true)]
		public string UserName 
		{
			get { return GetColumnValue<string>(Columns.UserName); }
			set { SetColumnValue(Columns.UserName, value); }
		}
		  
		[XmlAttribute("Context")]
		[Bindable(true)]
		public string Context 
		{
			get { return GetColumnValue<string>(Columns.Context); }
			set { SetColumnValue(Columns.Context, value); }
		}
		  
		[XmlAttribute("Verb")]
		[Bindable(true)]
		public string Verb 
		{
			get { return GetColumnValue<string>(Columns.Verb); }
			set { SetColumnValue(Columns.Verb, value); }
		}
		  
		[XmlAttribute("OldValue")]
		[Bindable(true)]
		public string OldValue 
		{
			get { return GetColumnValue<string>(Columns.OldValue); }
			set { SetColumnValue(Columns.OldValue, value); }
		}
		  
		[XmlAttribute("NewValue")]
		[Bindable(true)]
		public string NewValue 
		{
			get { return GetColumnValue<string>(Columns.NewValue); }
			set { SetColumnValue(Columns.NewValue, value); }
		}
		  
		[XmlAttribute("Description")]
		[Bindable(true)]
		public string Description 
		{
			get { return GetColumnValue<string>(Columns.Description); }
			set { SetColumnValue(Columns.Description, value); }
		}
		  
		[XmlAttribute("Ip")]
		[Bindable(true)]
		public string Ip 
		{
			get { return GetColumnValue<string>(Columns.Ip); }
			set { SetColumnValue(Columns.Ip, value); }
		}
		  
		[XmlAttribute("DtStamp")]
		[Bindable(true)]
		public DateTime? DtStamp 
		{
			get { return GetColumnValue<DateTime?>(Columns.DtStamp); }
			set { SetColumnValue(Columns.DtStamp, value); }
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
		/// Returns a AspnetApplication ActiveRecord object related to this EventQ
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
		public static void Insert(DateTime? varDateToProcess,DateTime? varDateProcessed,string varStatus,Guid? varThreadLock,int? varAttemptsRemaining,int varIPriority,Guid? varCreatorId,string varCreatorName,Guid? varUserId,string varUserName,string varContext,string varVerb,string varOldValue,string varNewValue,string varDescription,string varIp,DateTime? varDtStamp,Guid varApplicationId)
		{
			EventQ item = new EventQ();
			
			item.DateToProcess = varDateToProcess;
			
			item.DateProcessed = varDateProcessed;
			
			item.Status = varStatus;
			
			item.ThreadLock = varThreadLock;
			
			item.AttemptsRemaining = varAttemptsRemaining;
			
			item.IPriority = varIPriority;
			
			item.CreatorId = varCreatorId;
			
			item.CreatorName = varCreatorName;
			
			item.UserId = varUserId;
			
			item.UserName = varUserName;
			
			item.Context = varContext;
			
			item.Verb = varVerb;
			
			item.OldValue = varOldValue;
			
			item.NewValue = varNewValue;
			
			item.Description = varDescription;
			
			item.Ip = varIp;
			
			item.DtStamp = varDtStamp;
			
			item.ApplicationId = varApplicationId;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varId,DateTime? varDateToProcess,DateTime? varDateProcessed,string varStatus,Guid? varThreadLock,int? varAttemptsRemaining,int varIPriority,Guid? varCreatorId,string varCreatorName,Guid? varUserId,string varUserName,string varContext,string varVerb,string varOldValue,string varNewValue,string varDescription,string varIp,DateTime? varDtStamp,Guid varApplicationId)
		{
			EventQ item = new EventQ();
			
				item.Id = varId;
			
				item.DateToProcess = varDateToProcess;
			
				item.DateProcessed = varDateProcessed;
			
				item.Status = varStatus;
			
				item.ThreadLock = varThreadLock;
			
				item.AttemptsRemaining = varAttemptsRemaining;
			
				item.IPriority = varIPriority;
			
				item.CreatorId = varCreatorId;
			
				item.CreatorName = varCreatorName;
			
				item.UserId = varUserId;
			
				item.UserName = varUserName;
			
				item.Context = varContext;
			
				item.Verb = varVerb;
			
				item.OldValue = varOldValue;
			
				item.NewValue = varNewValue;
			
				item.Description = varDescription;
			
				item.Ip = varIp;
			
				item.DtStamp = varDtStamp;
			
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
        
        
        
        public static TableSchema.TableColumn DateToProcessColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn DateProcessedColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn StatusColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn ThreadLockColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn AttemptsRemainingColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn IPriorityColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatorIdColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatorNameColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn UserIdColumn
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        public static TableSchema.TableColumn UserNameColumn
        {
            get { return Schema.Columns[10]; }
        }
        
        
        
        public static TableSchema.TableColumn ContextColumn
        {
            get { return Schema.Columns[11]; }
        }
        
        
        
        public static TableSchema.TableColumn VerbColumn
        {
            get { return Schema.Columns[12]; }
        }
        
        
        
        public static TableSchema.TableColumn OldValueColumn
        {
            get { return Schema.Columns[13]; }
        }
        
        
        
        public static TableSchema.TableColumn NewValueColumn
        {
            get { return Schema.Columns[14]; }
        }
        
        
        
        public static TableSchema.TableColumn DescriptionColumn
        {
            get { return Schema.Columns[15]; }
        }
        
        
        
        public static TableSchema.TableColumn IpColumn
        {
            get { return Schema.Columns[16]; }
        }
        
        
        
        public static TableSchema.TableColumn DtStampColumn
        {
            get { return Schema.Columns[17]; }
        }
        
        
        
        public static TableSchema.TableColumn ApplicationIdColumn
        {
            get { return Schema.Columns[18]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Id = @"Id";
			 public static string DateToProcess = @"DateToProcess";
			 public static string DateProcessed = @"DateProcessed";
			 public static string Status = @"Status";
			 public static string ThreadLock = @"ThreadLock";
			 public static string AttemptsRemaining = @"AttemptsRemaining";
			 public static string IPriority = @"iPriority";
			 public static string CreatorId = @"CreatorId";
			 public static string CreatorName = @"CreatorName";
			 public static string UserId = @"UserId";
			 public static string UserName = @"UserName";
			 public static string Context = @"Context";
			 public static string Verb = @"Verb";
			 public static string OldValue = @"OldValue";
			 public static string NewValue = @"NewValue";
			 public static string Description = @"Description";
			 public static string Ip = @"IP";
			 public static string DtStamp = @"dtStamp";
			 public static string ApplicationId = @"ApplicationId";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
