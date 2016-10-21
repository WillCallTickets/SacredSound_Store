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
	/// Strongly-typed collection for the MailQueue class.
	/// </summary>
    [Serializable]
	public partial class MailQueueCollection : ActiveList<MailQueue, MailQueueCollection>
	{	   
		public MailQueueCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>MailQueueCollection</returns>
		public MailQueueCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                MailQueue o = this[i];
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
	/// This is an ActiveRecord class which wraps the MailQueue table.
	/// </summary>
	[Serializable]
	public partial class MailQueue : ActiveRecord<MailQueue>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public MailQueue()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public MailQueue(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public MailQueue(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public MailQueue(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("MailQueue", TableType.Table, DataService.GetInstance("WillCall"));
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
				
				TableSchema.TableColumn colvarTEmailLetterId = new TableSchema.TableColumn(schema);
				colvarTEmailLetterId.ColumnName = "TEmailLetterId";
				colvarTEmailLetterId.DataType = DbType.Int32;
				colvarTEmailLetterId.MaxLength = 0;
				colvarTEmailLetterId.AutoIncrement = false;
				colvarTEmailLetterId.IsNullable = true;
				colvarTEmailLetterId.IsPrimaryKey = false;
				colvarTEmailLetterId.IsForeignKey = true;
				colvarTEmailLetterId.IsReadOnly = false;
				colvarTEmailLetterId.DefaultSetting = @"";
				
					colvarTEmailLetterId.ForeignKeyTableName = "EmailLetter";
				schema.Columns.Add(colvarTEmailLetterId);
				
				TableSchema.TableColumn colvarTSubscriptionEmailId = new TableSchema.TableColumn(schema);
				colvarTSubscriptionEmailId.ColumnName = "TSubscriptionEmailId";
				colvarTSubscriptionEmailId.DataType = DbType.Int32;
				colvarTSubscriptionEmailId.MaxLength = 0;
				colvarTSubscriptionEmailId.AutoIncrement = false;
				colvarTSubscriptionEmailId.IsNullable = true;
				colvarTSubscriptionEmailId.IsPrimaryKey = false;
				colvarTSubscriptionEmailId.IsForeignKey = true;
				colvarTSubscriptionEmailId.IsReadOnly = false;
				colvarTSubscriptionEmailId.DefaultSetting = @"";
				
					colvarTSubscriptionEmailId.ForeignKeyTableName = "SubscriptionEmail";
				schema.Columns.Add(colvarTSubscriptionEmailId);
				
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
				
				TableSchema.TableColumn colvarFromName = new TableSchema.TableColumn(schema);
				colvarFromName.ColumnName = "FromName";
				colvarFromName.DataType = DbType.AnsiString;
				colvarFromName.MaxLength = 80;
				colvarFromName.AutoIncrement = false;
				colvarFromName.IsNullable = true;
				colvarFromName.IsPrimaryKey = false;
				colvarFromName.IsForeignKey = false;
				colvarFromName.IsReadOnly = false;
				colvarFromName.DefaultSetting = @"";
				colvarFromName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarFromName);
				
				TableSchema.TableColumn colvarFromAddress = new TableSchema.TableColumn(schema);
				colvarFromAddress.ColumnName = "FromAddress";
				colvarFromAddress.DataType = DbType.AnsiString;
				colvarFromAddress.MaxLength = 300;
				colvarFromAddress.AutoIncrement = false;
				colvarFromAddress.IsNullable = true;
				colvarFromAddress.IsPrimaryKey = false;
				colvarFromAddress.IsForeignKey = false;
				colvarFromAddress.IsReadOnly = false;
				colvarFromAddress.DefaultSetting = @"";
				colvarFromAddress.ForeignKeyTableName = "";
				schema.Columns.Add(colvarFromAddress);
				
				TableSchema.TableColumn colvarToAddress = new TableSchema.TableColumn(schema);
				colvarToAddress.ColumnName = "ToAddress";
				colvarToAddress.DataType = DbType.AnsiString;
				colvarToAddress.MaxLength = 300;
				colvarToAddress.AutoIncrement = false;
				colvarToAddress.IsNullable = true;
				colvarToAddress.IsPrimaryKey = false;
				colvarToAddress.IsForeignKey = false;
				colvarToAddress.IsReadOnly = false;
				colvarToAddress.DefaultSetting = @"";
				colvarToAddress.ForeignKeyTableName = "";
				schema.Columns.Add(colvarToAddress);
				
				TableSchema.TableColumn colvarCc = new TableSchema.TableColumn(schema);
				colvarCc.ColumnName = "CC";
				colvarCc.DataType = DbType.AnsiString;
				colvarCc.MaxLength = 300;
				colvarCc.AutoIncrement = false;
				colvarCc.IsNullable = true;
				colvarCc.IsPrimaryKey = false;
				colvarCc.IsForeignKey = false;
				colvarCc.IsReadOnly = false;
				colvarCc.DefaultSetting = @"";
				colvarCc.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCc);
				
				TableSchema.TableColumn colvarBcc = new TableSchema.TableColumn(schema);
				colvarBcc.ColumnName = "BCC";
				colvarBcc.DataType = DbType.AnsiString;
				colvarBcc.MaxLength = 300;
				colvarBcc.AutoIncrement = false;
				colvarBcc.IsNullable = true;
				colvarBcc.IsPrimaryKey = false;
				colvarBcc.IsForeignKey = false;
				colvarBcc.IsReadOnly = false;
				colvarBcc.DefaultSetting = @"";
				colvarBcc.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBcc);
				
				TableSchema.TableColumn colvarStatus = new TableSchema.TableColumn(schema);
				colvarStatus.ColumnName = "Status";
				colvarStatus.DataType = DbType.AnsiString;
				colvarStatus.MaxLength = 1000;
				colvarStatus.AutoIncrement = false;
				colvarStatus.IsNullable = true;
				colvarStatus.IsPrimaryKey = false;
				colvarStatus.IsForeignKey = false;
				colvarStatus.IsReadOnly = false;
				colvarStatus.DefaultSetting = @"";
				colvarStatus.ForeignKeyTableName = "";
				schema.Columns.Add(colvarStatus);
				
				TableSchema.TableColumn colvarPriority = new TableSchema.TableColumn(schema);
				colvarPriority.ColumnName = "Priority";
				colvarPriority.DataType = DbType.Int32;
				colvarPriority.MaxLength = 0;
				colvarPriority.AutoIncrement = false;
				colvarPriority.IsNullable = false;
				colvarPriority.IsPrimaryKey = false;
				colvarPriority.IsForeignKey = false;
				colvarPriority.IsReadOnly = false;
				
						colvarPriority.DefaultSetting = @"((0))";
				colvarPriority.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPriority);
				
				TableSchema.TableColumn colvarBMassMailer = new TableSchema.TableColumn(schema);
				colvarBMassMailer.ColumnName = "bMassMailer";
				colvarBMassMailer.DataType = DbType.Boolean;
				colvarBMassMailer.MaxLength = 0;
				colvarBMassMailer.AutoIncrement = false;
				colvarBMassMailer.IsNullable = true;
				colvarBMassMailer.IsPrimaryKey = false;
				colvarBMassMailer.IsForeignKey = false;
				colvarBMassMailer.IsReadOnly = false;
				colvarBMassMailer.DefaultSetting = @"";
				colvarBMassMailer.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBMassMailer);
				
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
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["WillCall"].AddSchema("MailQueue",schema);
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
		  
		[XmlAttribute("TEmailLetterId")]
		[Bindable(true)]
		public int? TEmailLetterId 
		{
			get { return GetColumnValue<int?>(Columns.TEmailLetterId); }
			set { SetColumnValue(Columns.TEmailLetterId, value); }
		}
		  
		[XmlAttribute("TSubscriptionEmailId")]
		[Bindable(true)]
		public int? TSubscriptionEmailId 
		{
			get { return GetColumnValue<int?>(Columns.TSubscriptionEmailId); }
			set { SetColumnValue(Columns.TSubscriptionEmailId, value); }
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
		  
		[XmlAttribute("FromName")]
		[Bindable(true)]
		public string FromName 
		{
			get { return GetColumnValue<string>(Columns.FromName); }
			set { SetColumnValue(Columns.FromName, value); }
		}
		  
		[XmlAttribute("FromAddress")]
		[Bindable(true)]
		public string FromAddress 
		{
			get { return GetColumnValue<string>(Columns.FromAddress); }
			set { SetColumnValue(Columns.FromAddress, value); }
		}
		  
		[XmlAttribute("ToAddress")]
		[Bindable(true)]
		public string ToAddress 
		{
			get { return GetColumnValue<string>(Columns.ToAddress); }
			set { SetColumnValue(Columns.ToAddress, value); }
		}
		  
		[XmlAttribute("Cc")]
		[Bindable(true)]
		public string Cc 
		{
			get { return GetColumnValue<string>(Columns.Cc); }
			set { SetColumnValue(Columns.Cc, value); }
		}
		  
		[XmlAttribute("Bcc")]
		[Bindable(true)]
		public string Bcc 
		{
			get { return GetColumnValue<string>(Columns.Bcc); }
			set { SetColumnValue(Columns.Bcc, value); }
		}
		  
		[XmlAttribute("Status")]
		[Bindable(true)]
		public string Status 
		{
			get { return GetColumnValue<string>(Columns.Status); }
			set { SetColumnValue(Columns.Status, value); }
		}
		  
		[XmlAttribute("Priority")]
		[Bindable(true)]
		public int Priority 
		{
			get { return GetColumnValue<int>(Columns.Priority); }
			set { SetColumnValue(Columns.Priority, value); }
		}
		  
		[XmlAttribute("BMassMailer")]
		[Bindable(true)]
		public bool? BMassMailer 
		{
			get { return GetColumnValue<bool?>(Columns.BMassMailer); }
			set { SetColumnValue(Columns.BMassMailer, value); }
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
		
		#endregion
		
		
		#region PrimaryKey Methods		
		
        protected override void SetPrimaryKey(object oValue)
        {
            base.SetPrimaryKey(oValue);
            
            SetPKValues();
        }
        
		
		private Wcss.EmailParamCollection colEmailParamRecords;
		public Wcss.EmailParamCollection EmailParamRecords()
		{
			if(colEmailParamRecords == null)
			{
				colEmailParamRecords = new Wcss.EmailParamCollection().Where(EmailParam.Columns.TMailQueueId, Id).Load();
				colEmailParamRecords.ListChanged += new ListChangedEventHandler(colEmailParamRecords_ListChanged);
			}
			return colEmailParamRecords;
		}
				
		void colEmailParamRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colEmailParamRecords[e.NewIndex].TMailQueueId = Id;
				colEmailParamRecords.ListChanged += new ListChangedEventHandler(colEmailParamRecords_ListChanged);
            }
		}
		#endregion
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a AspnetApplication ActiveRecord object related to this MailQueue
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
		
		
		/// <summary>
		/// Returns a EmailLetter ActiveRecord object related to this MailQueue
		/// 
		/// </summary>
		private Wcss.EmailLetter EmailLetter
		{
			get { return Wcss.EmailLetter.FetchByID(this.TEmailLetterId); }
			set { SetColumnValue("TEmailLetterId", value.Id); }
		}
        //set up an alternate mechanism to avoid a database call
		private Wcss.EmailLetter _emailletterrecord = null;
		
		public Wcss.EmailLetter EmailLetterRecord
		{
		    get
            {
                if (_emailletterrecord == null)
                {
                    _emailletterrecord = new Wcss.EmailLetter();
                    _emailletterrecord.CopyFrom(this.EmailLetter);
                }
                return _emailletterrecord;
            }
            set
            {
                if(value != null && _emailletterrecord == null)
			        _emailletterrecord = new Wcss.EmailLetter();
                
                SetColumnValue("TEmailLetterId", value.Id);
                _emailletterrecord.CopyFrom(value);                
            }
		}
		
		
		/// <summary>
		/// Returns a SubscriptionEmail ActiveRecord object related to this MailQueue
		/// 
		/// </summary>
		private Wcss.SubscriptionEmail SubscriptionEmail
		{
			get { return Wcss.SubscriptionEmail.FetchByID(this.TSubscriptionEmailId); }
			set { SetColumnValue("TSubscriptionEmailId", value.Id); }
		}
        //set up an alternate mechanism to avoid a database call
		private Wcss.SubscriptionEmail _subscriptionemailrecord = null;
		
		public Wcss.SubscriptionEmail SubscriptionEmailRecord
		{
		    get
            {
                if (_subscriptionemailrecord == null)
                {
                    _subscriptionemailrecord = new Wcss.SubscriptionEmail();
                    _subscriptionemailrecord.CopyFrom(this.SubscriptionEmail);
                }
                return _subscriptionemailrecord;
            }
            set
            {
                if(value != null && _subscriptionemailrecord == null)
			        _subscriptionemailrecord = new Wcss.SubscriptionEmail();
                
                SetColumnValue("TSubscriptionEmailId", value.Id);
                _subscriptionemailrecord.CopyFrom(value);                
            }
		}
		
		
		#endregion
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(int? varTEmailLetterId,int? varTSubscriptionEmailId,DateTime? varDateToProcess,DateTime? varDateProcessed,string varFromName,string varFromAddress,string varToAddress,string varCc,string varBcc,string varStatus,int varPriority,bool? varBMassMailer,Guid? varThreadLock,int? varAttemptsRemaining,DateTime varDtStamp,Guid varApplicationId)
		{
			MailQueue item = new MailQueue();
			
			item.TEmailLetterId = varTEmailLetterId;
			
			item.TSubscriptionEmailId = varTSubscriptionEmailId;
			
			item.DateToProcess = varDateToProcess;
			
			item.DateProcessed = varDateProcessed;
			
			item.FromName = varFromName;
			
			item.FromAddress = varFromAddress;
			
			item.ToAddress = varToAddress;
			
			item.Cc = varCc;
			
			item.Bcc = varBcc;
			
			item.Status = varStatus;
			
			item.Priority = varPriority;
			
			item.BMassMailer = varBMassMailer;
			
			item.ThreadLock = varThreadLock;
			
			item.AttemptsRemaining = varAttemptsRemaining;
			
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
		public static void Update(int varId,int? varTEmailLetterId,int? varTSubscriptionEmailId,DateTime? varDateToProcess,DateTime? varDateProcessed,string varFromName,string varFromAddress,string varToAddress,string varCc,string varBcc,string varStatus,int varPriority,bool? varBMassMailer,Guid? varThreadLock,int? varAttemptsRemaining,DateTime varDtStamp,Guid varApplicationId)
		{
			MailQueue item = new MailQueue();
			
				item.Id = varId;
			
				item.TEmailLetterId = varTEmailLetterId;
			
				item.TSubscriptionEmailId = varTSubscriptionEmailId;
			
				item.DateToProcess = varDateToProcess;
			
				item.DateProcessed = varDateProcessed;
			
				item.FromName = varFromName;
			
				item.FromAddress = varFromAddress;
			
				item.ToAddress = varToAddress;
			
				item.Cc = varCc;
			
				item.Bcc = varBcc;
			
				item.Status = varStatus;
			
				item.Priority = varPriority;
			
				item.BMassMailer = varBMassMailer;
			
				item.ThreadLock = varThreadLock;
			
				item.AttemptsRemaining = varAttemptsRemaining;
			
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
        
        
        
        public static TableSchema.TableColumn TEmailLetterIdColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn TSubscriptionEmailIdColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn DateToProcessColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn DateProcessedColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn FromNameColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn FromAddressColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn ToAddressColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn CcColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn BccColumn
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        public static TableSchema.TableColumn StatusColumn
        {
            get { return Schema.Columns[10]; }
        }
        
        
        
        public static TableSchema.TableColumn PriorityColumn
        {
            get { return Schema.Columns[11]; }
        }
        
        
        
        public static TableSchema.TableColumn BMassMailerColumn
        {
            get { return Schema.Columns[12]; }
        }
        
        
        
        public static TableSchema.TableColumn ThreadLockColumn
        {
            get { return Schema.Columns[13]; }
        }
        
        
        
        public static TableSchema.TableColumn AttemptsRemainingColumn
        {
            get { return Schema.Columns[14]; }
        }
        
        
        
        public static TableSchema.TableColumn DtStampColumn
        {
            get { return Schema.Columns[15]; }
        }
        
        
        
        public static TableSchema.TableColumn ApplicationIdColumn
        {
            get { return Schema.Columns[16]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Id = @"Id";
			 public static string TEmailLetterId = @"TEmailLetterId";
			 public static string TSubscriptionEmailId = @"TSubscriptionEmailId";
			 public static string DateToProcess = @"DateToProcess";
			 public static string DateProcessed = @"DateProcessed";
			 public static string FromName = @"FromName";
			 public static string FromAddress = @"FromAddress";
			 public static string ToAddress = @"ToAddress";
			 public static string Cc = @"CC";
			 public static string Bcc = @"BCC";
			 public static string Status = @"Status";
			 public static string Priority = @"Priority";
			 public static string BMassMailer = @"bMassMailer";
			 public static string ThreadLock = @"ThreadLock";
			 public static string AttemptsRemaining = @"AttemptsRemaining";
			 public static string DtStamp = @"dtStamp";
			 public static string ApplicationId = @"ApplicationId";
						
		}
		#endregion
		
		#region Update PK Collections
		
        public void SetPKValues()
        {
                if (colEmailParamRecords != null)
                {
                    foreach (Wcss.EmailParam item in colEmailParamRecords)
                    {
                        if (item.TMailQueueId != Id)
                        {
                            item.TMailQueueId = Id;
                        }
                    }
               }
		}
        #endregion
    
        #region Deep Save
		
        public void DeepSave()
        {
            Save();
            
                if (colEmailParamRecords != null)
                {
                    colEmailParamRecords.SaveAll();
               }
		}
        #endregion
	}
}
