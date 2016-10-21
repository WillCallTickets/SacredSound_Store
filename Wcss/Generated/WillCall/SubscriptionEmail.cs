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
	/// Strongly-typed collection for the SubscriptionEmail class.
	/// </summary>
    [Serializable]
	public partial class SubscriptionEmailCollection : ActiveList<SubscriptionEmail, SubscriptionEmailCollection>
	{	   
		public SubscriptionEmailCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>SubscriptionEmailCollection</returns>
		public SubscriptionEmailCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                SubscriptionEmail o = this[i];
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
	/// This is an ActiveRecord class which wraps the SubscriptionEmail table.
	/// </summary>
	[Serializable]
	public partial class SubscriptionEmail : ActiveRecord<SubscriptionEmail>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public SubscriptionEmail()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public SubscriptionEmail(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public SubscriptionEmail(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public SubscriptionEmail(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("SubscriptionEmail", TableType.Table, DataService.GetInstance("WillCall"));
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
				
				TableSchema.TableColumn colvarTSubscriptionId = new TableSchema.TableColumn(schema);
				colvarTSubscriptionId.ColumnName = "TSubscriptionId";
				colvarTSubscriptionId.DataType = DbType.Int32;
				colvarTSubscriptionId.MaxLength = 0;
				colvarTSubscriptionId.AutoIncrement = false;
				colvarTSubscriptionId.IsNullable = false;
				colvarTSubscriptionId.IsPrimaryKey = false;
				colvarTSubscriptionId.IsForeignKey = true;
				colvarTSubscriptionId.IsReadOnly = false;
				colvarTSubscriptionId.DefaultSetting = @"";
				
					colvarTSubscriptionId.ForeignKeyTableName = "Subscription";
				schema.Columns.Add(colvarTSubscriptionId);
				
				TableSchema.TableColumn colvarTEmailLetterId = new TableSchema.TableColumn(schema);
				colvarTEmailLetterId.ColumnName = "TEmailLetterId";
				colvarTEmailLetterId.DataType = DbType.Int32;
				colvarTEmailLetterId.MaxLength = 0;
				colvarTEmailLetterId.AutoIncrement = false;
				colvarTEmailLetterId.IsNullable = false;
				colvarTEmailLetterId.IsPrimaryKey = false;
				colvarTEmailLetterId.IsForeignKey = true;
				colvarTEmailLetterId.IsReadOnly = false;
				colvarTEmailLetterId.DefaultSetting = @"";
				
					colvarTEmailLetterId.ForeignKeyTableName = "EmailLetter";
				schema.Columns.Add(colvarTEmailLetterId);
				
				TableSchema.TableColumn colvarPostedFileName = new TableSchema.TableColumn(schema);
				colvarPostedFileName.ColumnName = "PostedFileName";
				colvarPostedFileName.DataType = DbType.AnsiString;
				colvarPostedFileName.MaxLength = 256;
				colvarPostedFileName.AutoIncrement = false;
				colvarPostedFileName.IsNullable = false;
				colvarPostedFileName.IsPrimaryKey = false;
				colvarPostedFileName.IsForeignKey = false;
				colvarPostedFileName.IsReadOnly = false;
				colvarPostedFileName.DefaultSetting = @"";
				colvarPostedFileName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPostedFileName);
				
				TableSchema.TableColumn colvarCssFile = new TableSchema.TableColumn(schema);
				colvarCssFile.ColumnName = "CssFile";
				colvarCssFile.DataType = DbType.AnsiString;
				colvarCssFile.MaxLength = 256;
				colvarCssFile.AutoIncrement = false;
				colvarCssFile.IsNullable = true;
				colvarCssFile.IsPrimaryKey = false;
				colvarCssFile.IsForeignKey = false;
				colvarCssFile.IsReadOnly = false;
				colvarCssFile.DefaultSetting = @"";
				colvarCssFile.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCssFile);
				
				TableSchema.TableColumn colvarDtLastSent = new TableSchema.TableColumn(schema);
				colvarDtLastSent.ColumnName = "dtLastSent";
				colvarDtLastSent.DataType = DbType.DateTime;
				colvarDtLastSent.MaxLength = 0;
				colvarDtLastSent.AutoIncrement = false;
				colvarDtLastSent.IsNullable = true;
				colvarDtLastSent.IsPrimaryKey = false;
				colvarDtLastSent.IsForeignKey = false;
				colvarDtLastSent.IsReadOnly = false;
				colvarDtLastSent.DefaultSetting = @"";
				colvarDtLastSent.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDtLastSent);
				
				TableSchema.TableColumn colvarConstructedHtml = new TableSchema.TableColumn(schema);
				colvarConstructedHtml.ColumnName = "Constructed_Html";
				colvarConstructedHtml.DataType = DbType.AnsiString;
				colvarConstructedHtml.MaxLength = 2147483647;
				colvarConstructedHtml.AutoIncrement = false;
				colvarConstructedHtml.IsNullable = true;
				colvarConstructedHtml.IsPrimaryKey = false;
				colvarConstructedHtml.IsForeignKey = false;
				colvarConstructedHtml.IsReadOnly = false;
				colvarConstructedHtml.DefaultSetting = @"";
				colvarConstructedHtml.ForeignKeyTableName = "";
				schema.Columns.Add(colvarConstructedHtml);
				
				TableSchema.TableColumn colvarConstructedText = new TableSchema.TableColumn(schema);
				colvarConstructedText.ColumnName = "Constructed_Text";
				colvarConstructedText.DataType = DbType.AnsiString;
				colvarConstructedText.MaxLength = 2147483647;
				colvarConstructedText.AutoIncrement = false;
				colvarConstructedText.IsNullable = true;
				colvarConstructedText.IsPrimaryKey = false;
				colvarConstructedText.IsForeignKey = false;
				colvarConstructedText.IsReadOnly = false;
				colvarConstructedText.DefaultSetting = @"";
				colvarConstructedText.ForeignKeyTableName = "";
				schema.Columns.Add(colvarConstructedText);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["WillCall"].AddSchema("SubscriptionEmail",schema);
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
		  
		[XmlAttribute("TSubscriptionId")]
		[Bindable(true)]
		public int TSubscriptionId 
		{
			get { return GetColumnValue<int>(Columns.TSubscriptionId); }
			set { SetColumnValue(Columns.TSubscriptionId, value); }
		}
		  
		[XmlAttribute("TEmailLetterId")]
		[Bindable(true)]
		public int TEmailLetterId 
		{
			get { return GetColumnValue<int>(Columns.TEmailLetterId); }
			set { SetColumnValue(Columns.TEmailLetterId, value); }
		}
		  
		[XmlAttribute("PostedFileName")]
		[Bindable(true)]
		public string PostedFileName 
		{
			get { return GetColumnValue<string>(Columns.PostedFileName); }
			set { SetColumnValue(Columns.PostedFileName, value); }
		}
		  
		[XmlAttribute("CssFile")]
		[Bindable(true)]
		public string CssFile 
		{
			get { return GetColumnValue<string>(Columns.CssFile); }
			set { SetColumnValue(Columns.CssFile, value); }
		}
		  
		[XmlAttribute("DtLastSent")]
		[Bindable(true)]
		public DateTime? DtLastSent 
		{
			get { return GetColumnValue<DateTime?>(Columns.DtLastSent); }
			set { SetColumnValue(Columns.DtLastSent, value); }
		}
		  
		[XmlAttribute("ConstructedHtml")]
		[Bindable(true)]
		public string ConstructedHtml 
		{
			get { return GetColumnValue<string>(Columns.ConstructedHtml); }
			set { SetColumnValue(Columns.ConstructedHtml, value); }
		}
		  
		[XmlAttribute("ConstructedText")]
		[Bindable(true)]
		public string ConstructedText 
		{
			get { return GetColumnValue<string>(Columns.ConstructedText); }
			set { SetColumnValue(Columns.ConstructedText, value); }
		}
		
		#endregion
		
		
		#region PrimaryKey Methods		
		
        protected override void SetPrimaryKey(object oValue)
        {
            base.SetPrimaryKey(oValue);
            
            SetPKValues();
        }
        
		
		private Wcss.HistorySubscriptionEmailCollection colHistorySubscriptionEmailRecords;
		public Wcss.HistorySubscriptionEmailCollection HistorySubscriptionEmailRecords()
		{
			if(colHistorySubscriptionEmailRecords == null)
			{
				colHistorySubscriptionEmailRecords = new Wcss.HistorySubscriptionEmailCollection().Where(HistorySubscriptionEmail.Columns.TSubscriptionEmailId, Id).Load();
				colHistorySubscriptionEmailRecords.ListChanged += new ListChangedEventHandler(colHistorySubscriptionEmailRecords_ListChanged);
			}
			return colHistorySubscriptionEmailRecords;
		}
				
		void colHistorySubscriptionEmailRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colHistorySubscriptionEmailRecords[e.NewIndex].TSubscriptionEmailId = Id;
				colHistorySubscriptionEmailRecords.ListChanged += new ListChangedEventHandler(colHistorySubscriptionEmailRecords_ListChanged);
            }
		}
		private Wcss.MailQueueCollection colMailQueueRecords;
		public Wcss.MailQueueCollection MailQueueRecords()
		{
			if(colMailQueueRecords == null)
			{
				colMailQueueRecords = new Wcss.MailQueueCollection().Where(MailQueue.Columns.TSubscriptionEmailId, Id).Load();
				colMailQueueRecords.ListChanged += new ListChangedEventHandler(colMailQueueRecords_ListChanged);
			}
			return colMailQueueRecords;
		}
				
		void colMailQueueRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colMailQueueRecords[e.NewIndex].TSubscriptionEmailId = Id;
				colMailQueueRecords.ListChanged += new ListChangedEventHandler(colMailQueueRecords_ListChanged);
            }
		}
		#endregion
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a EmailLetter ActiveRecord object related to this SubscriptionEmail
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
		/// Returns a Subscription ActiveRecord object related to this SubscriptionEmail
		/// 
		/// </summary>
		private Wcss.Subscription Subscription
		{
			get { return Wcss.Subscription.FetchByID(this.TSubscriptionId); }
			set { SetColumnValue("TSubscriptionId", value.Id); }
		}
        //set up an alternate mechanism to avoid a database call
		private Wcss.Subscription _subscriptionrecord = null;
		
		public Wcss.Subscription SubscriptionRecord
		{
		    get
            {
                if (_subscriptionrecord == null)
                {
                    _subscriptionrecord = new Wcss.Subscription();
                    _subscriptionrecord.CopyFrom(this.Subscription);
                }
                return _subscriptionrecord;
            }
            set
            {
                if(value != null && _subscriptionrecord == null)
			        _subscriptionrecord = new Wcss.Subscription();
                
                SetColumnValue("TSubscriptionId", value.Id);
                _subscriptionrecord.CopyFrom(value);                
            }
		}
		
		
		#endregion
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(DateTime varDtStamp,int varTSubscriptionId,int varTEmailLetterId,string varPostedFileName,string varCssFile,DateTime? varDtLastSent,string varConstructedHtml,string varConstructedText)
		{
			SubscriptionEmail item = new SubscriptionEmail();
			
			item.DtStamp = varDtStamp;
			
			item.TSubscriptionId = varTSubscriptionId;
			
			item.TEmailLetterId = varTEmailLetterId;
			
			item.PostedFileName = varPostedFileName;
			
			item.CssFile = varCssFile;
			
			item.DtLastSent = varDtLastSent;
			
			item.ConstructedHtml = varConstructedHtml;
			
			item.ConstructedText = varConstructedText;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varId,DateTime varDtStamp,int varTSubscriptionId,int varTEmailLetterId,string varPostedFileName,string varCssFile,DateTime? varDtLastSent,string varConstructedHtml,string varConstructedText)
		{
			SubscriptionEmail item = new SubscriptionEmail();
			
				item.Id = varId;
			
				item.DtStamp = varDtStamp;
			
				item.TSubscriptionId = varTSubscriptionId;
			
				item.TEmailLetterId = varTEmailLetterId;
			
				item.PostedFileName = varPostedFileName;
			
				item.CssFile = varCssFile;
			
				item.DtLastSent = varDtLastSent;
			
				item.ConstructedHtml = varConstructedHtml;
			
				item.ConstructedText = varConstructedText;
			
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
        
        
        
        public static TableSchema.TableColumn TSubscriptionIdColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn TEmailLetterIdColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn PostedFileNameColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn CssFileColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn DtLastSentColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn ConstructedHtmlColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn ConstructedTextColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Id = @"Id";
			 public static string DtStamp = @"dtStamp";
			 public static string TSubscriptionId = @"TSubscriptionId";
			 public static string TEmailLetterId = @"TEmailLetterId";
			 public static string PostedFileName = @"PostedFileName";
			 public static string CssFile = @"CssFile";
			 public static string DtLastSent = @"dtLastSent";
			 public static string ConstructedHtml = @"Constructed_Html";
			 public static string ConstructedText = @"Constructed_Text";
						
		}
		#endregion
		
		#region Update PK Collections
		
        public void SetPKValues()
        {
                if (colHistorySubscriptionEmailRecords != null)
                {
                    foreach (Wcss.HistorySubscriptionEmail item in colHistorySubscriptionEmailRecords)
                    {
                        if (item.TSubscriptionEmailId != Id)
                        {
                            item.TSubscriptionEmailId = Id;
                        }
                    }
               }
		
                if (colMailQueueRecords != null)
                {
                    foreach (Wcss.MailQueue item in colMailQueueRecords)
                    {
                        if (item.TSubscriptionEmailId != Id)
                        {
                            item.TSubscriptionEmailId = Id;
                        }
                    }
               }
		}
        #endregion
    
        #region Deep Save
		
        public void DeepSave()
        {
            Save();
            
                if (colHistorySubscriptionEmailRecords != null)
                {
                    colHistorySubscriptionEmailRecords.SaveAll();
               }
		
                if (colMailQueueRecords != null)
                {
                    colMailQueueRecords.SaveAll();
               }
		}
        #endregion
	}
}
