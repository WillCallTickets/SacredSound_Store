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
	/// Strongly-typed collection for the EmailLetter class.
	/// </summary>
    [Serializable]
	public partial class EmailLetterCollection : ActiveList<EmailLetter, EmailLetterCollection>
	{	   
		public EmailLetterCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>EmailLetterCollection</returns>
		public EmailLetterCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                EmailLetter o = this[i];
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
	/// This is an ActiveRecord class which wraps the EmailLetter table.
	/// </summary>
	[Serializable]
	public partial class EmailLetter : ActiveRecord<EmailLetter>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public EmailLetter()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public EmailLetter(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public EmailLetter(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public EmailLetter(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("EmailLetter", TableType.Table, DataService.GetInstance("WillCall"));
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
				
				TableSchema.TableColumn colvarName = new TableSchema.TableColumn(schema);
				colvarName.ColumnName = "Name";
				colvarName.DataType = DbType.AnsiString;
				colvarName.MaxLength = 256;
				colvarName.AutoIncrement = false;
				colvarName.IsNullable = false;
				colvarName.IsPrimaryKey = false;
				colvarName.IsForeignKey = false;
				colvarName.IsReadOnly = false;
				colvarName.DefaultSetting = @"";
				colvarName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarName);
				
				TableSchema.TableColumn colvarSubject = new TableSchema.TableColumn(schema);
				colvarSubject.ColumnName = "Subject";
				colvarSubject.DataType = DbType.AnsiString;
				colvarSubject.MaxLength = 256;
				colvarSubject.AutoIncrement = false;
				colvarSubject.IsNullable = false;
				colvarSubject.IsPrimaryKey = false;
				colvarSubject.IsForeignKey = false;
				colvarSubject.IsReadOnly = false;
				colvarSubject.DefaultSetting = @"";
				colvarSubject.ForeignKeyTableName = "";
				schema.Columns.Add(colvarSubject);
				
				TableSchema.TableColumn colvarStyleContent = new TableSchema.TableColumn(schema);
				colvarStyleContent.ColumnName = "StyleContent";
				colvarStyleContent.DataType = DbType.AnsiString;
				colvarStyleContent.MaxLength = -1;
				colvarStyleContent.AutoIncrement = false;
				colvarStyleContent.IsNullable = true;
				colvarStyleContent.IsPrimaryKey = false;
				colvarStyleContent.IsForeignKey = false;
				colvarStyleContent.IsReadOnly = false;
				colvarStyleContent.DefaultSetting = @"";
				colvarStyleContent.ForeignKeyTableName = "";
				schema.Columns.Add(colvarStyleContent);
				
				TableSchema.TableColumn colvarHtmlVersion = new TableSchema.TableColumn(schema);
				colvarHtmlVersion.ColumnName = "HtmlVersion";
				colvarHtmlVersion.DataType = DbType.AnsiString;
				colvarHtmlVersion.MaxLength = -1;
				colvarHtmlVersion.AutoIncrement = false;
				colvarHtmlVersion.IsNullable = false;
				colvarHtmlVersion.IsPrimaryKey = false;
				colvarHtmlVersion.IsForeignKey = false;
				colvarHtmlVersion.IsReadOnly = false;
				colvarHtmlVersion.DefaultSetting = @"";
				colvarHtmlVersion.ForeignKeyTableName = "";
				schema.Columns.Add(colvarHtmlVersion);
				
				TableSchema.TableColumn colvarTextVersion = new TableSchema.TableColumn(schema);
				colvarTextVersion.ColumnName = "TextVersion";
				colvarTextVersion.DataType = DbType.AnsiString;
				colvarTextVersion.MaxLength = -1;
				colvarTextVersion.AutoIncrement = false;
				colvarTextVersion.IsNullable = true;
				colvarTextVersion.IsPrimaryKey = false;
				colvarTextVersion.IsForeignKey = false;
				colvarTextVersion.IsReadOnly = false;
				colvarTextVersion.DefaultSetting = @"";
				colvarTextVersion.ForeignKeyTableName = "";
				schema.Columns.Add(colvarTextVersion);
				
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
				DataService.Providers["WillCall"].AddSchema("EmailLetter",schema);
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
		  
		[XmlAttribute("Name")]
		[Bindable(true)]
		public string Name 
		{
			get { return GetColumnValue<string>(Columns.Name); }
			set { SetColumnValue(Columns.Name, value); }
		}
		  
		[XmlAttribute("Subject")]
		[Bindable(true)]
		public string Subject 
		{
			get { return GetColumnValue<string>(Columns.Subject); }
			set { SetColumnValue(Columns.Subject, value); }
		}
		  
		[XmlAttribute("StyleContent")]
		[Bindable(true)]
		public string StyleContent 
		{
			get { return GetColumnValue<string>(Columns.StyleContent); }
			set { SetColumnValue(Columns.StyleContent, value); }
		}
		  
		[XmlAttribute("HtmlVersion")]
		[Bindable(true)]
		public string HtmlVersion 
		{
			get { return GetColumnValue<string>(Columns.HtmlVersion); }
			set { SetColumnValue(Columns.HtmlVersion, value); }
		}
		  
		[XmlAttribute("TextVersion")]
		[Bindable(true)]
		public string TextVersion 
		{
			get { return GetColumnValue<string>(Columns.TextVersion); }
			set { SetColumnValue(Columns.TextVersion, value); }
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
        
		
		private Wcss.MailQueueCollection colMailQueueRecords;
		public Wcss.MailQueueCollection MailQueueRecords()
		{
			if(colMailQueueRecords == null)
			{
				colMailQueueRecords = new Wcss.MailQueueCollection().Where(MailQueue.Columns.TEmailLetterId, Id).Load();
				colMailQueueRecords.ListChanged += new ListChangedEventHandler(colMailQueueRecords_ListChanged);
			}
			return colMailQueueRecords;
		}
				
		void colMailQueueRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colMailQueueRecords[e.NewIndex].TEmailLetterId = Id;
				colMailQueueRecords.ListChanged += new ListChangedEventHandler(colMailQueueRecords_ListChanged);
            }
		}
		private Wcss.SubscriptionEmailCollection colSubscriptionEmailRecords;
		public Wcss.SubscriptionEmailCollection SubscriptionEmailRecords()
		{
			if(colSubscriptionEmailRecords == null)
			{
				colSubscriptionEmailRecords = new Wcss.SubscriptionEmailCollection().Where(SubscriptionEmail.Columns.TEmailLetterId, Id).Load();
				colSubscriptionEmailRecords.ListChanged += new ListChangedEventHandler(colSubscriptionEmailRecords_ListChanged);
			}
			return colSubscriptionEmailRecords;
		}
				
		void colSubscriptionEmailRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colSubscriptionEmailRecords[e.NewIndex].TEmailLetterId = Id;
				colSubscriptionEmailRecords.ListChanged += new ListChangedEventHandler(colSubscriptionEmailRecords_ListChanged);
            }
		}
		#endregion
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a AspnetApplication ActiveRecord object related to this EmailLetter
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
		public static void Insert(string varName,string varSubject,string varStyleContent,string varHtmlVersion,string varTextVersion,DateTime varDtStamp,Guid varApplicationId)
		{
			EmailLetter item = new EmailLetter();
			
			item.Name = varName;
			
			item.Subject = varSubject;
			
			item.StyleContent = varStyleContent;
			
			item.HtmlVersion = varHtmlVersion;
			
			item.TextVersion = varTextVersion;
			
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
		public static void Update(int varId,string varName,string varSubject,string varStyleContent,string varHtmlVersion,string varTextVersion,DateTime varDtStamp,Guid varApplicationId)
		{
			EmailLetter item = new EmailLetter();
			
				item.Id = varId;
			
				item.Name = varName;
			
				item.Subject = varSubject;
			
				item.StyleContent = varStyleContent;
			
				item.HtmlVersion = varHtmlVersion;
			
				item.TextVersion = varTextVersion;
			
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
        
        
        
        public static TableSchema.TableColumn NameColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn SubjectColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn StyleContentColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn HtmlVersionColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn TextVersionColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn DtStampColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn ApplicationIdColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Id = @"Id";
			 public static string Name = @"Name";
			 public static string Subject = @"Subject";
			 public static string StyleContent = @"StyleContent";
			 public static string HtmlVersion = @"HtmlVersion";
			 public static string TextVersion = @"TextVersion";
			 public static string DtStamp = @"dtStamp";
			 public static string ApplicationId = @"ApplicationId";
						
		}
		#endregion
		
		#region Update PK Collections
		
        public void SetPKValues()
        {
                if (colMailQueueRecords != null)
                {
                    foreach (Wcss.MailQueue item in colMailQueueRecords)
                    {
                        if (item.TEmailLetterId != Id)
                        {
                            item.TEmailLetterId = Id;
                        }
                    }
               }
		
                if (colSubscriptionEmailRecords != null)
                {
                    foreach (Wcss.SubscriptionEmail item in colSubscriptionEmailRecords)
                    {
                        if (item.TEmailLetterId != Id)
                        {
                            item.TEmailLetterId = Id;
                        }
                    }
               }
		}
        #endregion
    
        #region Deep Save
		
        public void DeepSave()
        {
            Save();
            
                if (colMailQueueRecords != null)
                {
                    colMailQueueRecords.SaveAll();
               }
		
                if (colSubscriptionEmailRecords != null)
                {
                    colSubscriptionEmailRecords.SaveAll();
               }
		}
        #endregion
	}
}
