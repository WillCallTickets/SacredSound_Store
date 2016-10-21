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
	/// Strongly-typed collection for the SubscriptionUser class.
	/// </summary>
    [Serializable]
	public partial class SubscriptionUserCollection : ActiveList<SubscriptionUser, SubscriptionUserCollection>
	{	   
		public SubscriptionUserCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>SubscriptionUserCollection</returns>
		public SubscriptionUserCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                SubscriptionUser o = this[i];
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
	/// This is an ActiveRecord class which wraps the SubscriptionUser table.
	/// </summary>
	[Serializable]
	public partial class SubscriptionUser : ActiveRecord<SubscriptionUser>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public SubscriptionUser()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public SubscriptionUser(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public SubscriptionUser(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public SubscriptionUser(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("SubscriptionUser", TableType.Table, DataService.GetInstance("WillCall"));
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
				
				TableSchema.TableColumn colvarUserId = new TableSchema.TableColumn(schema);
				colvarUserId.ColumnName = "UserId";
				colvarUserId.DataType = DbType.Guid;
				colvarUserId.MaxLength = 0;
				colvarUserId.AutoIncrement = false;
				colvarUserId.IsNullable = true;
				colvarUserId.IsPrimaryKey = false;
				colvarUserId.IsForeignKey = true;
				colvarUserId.IsReadOnly = false;
				colvarUserId.DefaultSetting = @"";
				
					colvarUserId.ForeignKeyTableName = "aspnet_Users";
				schema.Columns.Add(colvarUserId);
				
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
				
				TableSchema.TableColumn colvarBSubscribed = new TableSchema.TableColumn(schema);
				colvarBSubscribed.ColumnName = "bSubscribed";
				colvarBSubscribed.DataType = DbType.Boolean;
				colvarBSubscribed.MaxLength = 0;
				colvarBSubscribed.AutoIncrement = false;
				colvarBSubscribed.IsNullable = false;
				colvarBSubscribed.IsPrimaryKey = false;
				colvarBSubscribed.IsForeignKey = false;
				colvarBSubscribed.IsReadOnly = false;
				
						colvarBSubscribed.DefaultSetting = @"((0))";
				colvarBSubscribed.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBSubscribed);
				
				TableSchema.TableColumn colvarDtLastActionDate = new TableSchema.TableColumn(schema);
				colvarDtLastActionDate.ColumnName = "dtLastActionDate";
				colvarDtLastActionDate.DataType = DbType.DateTime;
				colvarDtLastActionDate.MaxLength = 0;
				colvarDtLastActionDate.AutoIncrement = false;
				colvarDtLastActionDate.IsNullable = true;
				colvarDtLastActionDate.IsPrimaryKey = false;
				colvarDtLastActionDate.IsForeignKey = false;
				colvarDtLastActionDate.IsReadOnly = false;
				colvarDtLastActionDate.DefaultSetting = @"";
				colvarDtLastActionDate.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDtLastActionDate);
				
				TableSchema.TableColumn colvarBHtmlFormat = new TableSchema.TableColumn(schema);
				colvarBHtmlFormat.ColumnName = "bHtmlFormat";
				colvarBHtmlFormat.DataType = DbType.Boolean;
				colvarBHtmlFormat.MaxLength = 0;
				colvarBHtmlFormat.AutoIncrement = false;
				colvarBHtmlFormat.IsNullable = false;
				colvarBHtmlFormat.IsPrimaryKey = false;
				colvarBHtmlFormat.IsForeignKey = false;
				colvarBHtmlFormat.IsReadOnly = false;
				
						colvarBHtmlFormat.DefaultSetting = @"((1))";
				colvarBHtmlFormat.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBHtmlFormat);
				
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
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["WillCall"].AddSchema("SubscriptionUser",schema);
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
		  
		[XmlAttribute("UserId")]
		[Bindable(true)]
		public Guid? UserId 
		{
			get { return GetColumnValue<Guid?>(Columns.UserId); }
			set { SetColumnValue(Columns.UserId, value); }
		}
		  
		[XmlAttribute("TSubscriptionId")]
		[Bindable(true)]
		public int TSubscriptionId 
		{
			get { return GetColumnValue<int>(Columns.TSubscriptionId); }
			set { SetColumnValue(Columns.TSubscriptionId, value); }
		}
		  
		[XmlAttribute("BSubscribed")]
		[Bindable(true)]
		public bool BSubscribed 
		{
			get { return GetColumnValue<bool>(Columns.BSubscribed); }
			set { SetColumnValue(Columns.BSubscribed, value); }
		}
		  
		[XmlAttribute("DtLastActionDate")]
		[Bindable(true)]
		public DateTime? DtLastActionDate 
		{
			get { return GetColumnValue<DateTime?>(Columns.DtLastActionDate); }
			set { SetColumnValue(Columns.DtLastActionDate, value); }
		}
		  
		[XmlAttribute("BHtmlFormat")]
		[Bindable(true)]
		public bool BHtmlFormat 
		{
			get { return GetColumnValue<bool>(Columns.BHtmlFormat); }
			set { SetColumnValue(Columns.BHtmlFormat, value); }
		}
		  
		[XmlAttribute("DtStamp")]
		[Bindable(true)]
		public DateTime DtStamp 
		{
			get { return GetColumnValue<DateTime>(Columns.DtStamp); }
			set { SetColumnValue(Columns.DtStamp, value); }
		}
		
		#endregion
		
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a AspnetUser ActiveRecord object related to this SubscriptionUser
		/// 
		/// </summary>
		private Wcss.AspnetUser AspnetUser
		{
			get { return Wcss.AspnetUser.FetchByID(this.UserId); }
			set { SetColumnValue("UserId", value.UserId); }
		}
        //set up an alternate mechanism to avoid a database call
		private Wcss.AspnetUser _aspnetuserrecord = null;
		
		public Wcss.AspnetUser AspnetUserRecord
		{
		    get
            {
                if (_aspnetuserrecord == null)
                {
                    _aspnetuserrecord = new Wcss.AspnetUser();
                    _aspnetuserrecord.CopyFrom(this.AspnetUser);
                }
                return _aspnetuserrecord;
            }
            set
            {
                if(value != null && _aspnetuserrecord == null)
			        _aspnetuserrecord = new Wcss.AspnetUser();
                
                SetColumnValue("UserId", value.UserId);
                _aspnetuserrecord.CopyFrom(value);                
            }
		}
		
		
		/// <summary>
		/// Returns a Subscription ActiveRecord object related to this SubscriptionUser
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
		public static void Insert(Guid? varUserId,int varTSubscriptionId,bool varBSubscribed,DateTime? varDtLastActionDate,bool varBHtmlFormat,DateTime varDtStamp)
		{
			SubscriptionUser item = new SubscriptionUser();
			
			item.UserId = varUserId;
			
			item.TSubscriptionId = varTSubscriptionId;
			
			item.BSubscribed = varBSubscribed;
			
			item.DtLastActionDate = varDtLastActionDate;
			
			item.BHtmlFormat = varBHtmlFormat;
			
			item.DtStamp = varDtStamp;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varId,Guid? varUserId,int varTSubscriptionId,bool varBSubscribed,DateTime? varDtLastActionDate,bool varBHtmlFormat,DateTime varDtStamp)
		{
			SubscriptionUser item = new SubscriptionUser();
			
				item.Id = varId;
			
				item.UserId = varUserId;
			
				item.TSubscriptionId = varTSubscriptionId;
			
				item.BSubscribed = varBSubscribed;
			
				item.DtLastActionDate = varDtLastActionDate;
			
				item.BHtmlFormat = varBHtmlFormat;
			
				item.DtStamp = varDtStamp;
			
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
        
        
        
        public static TableSchema.TableColumn UserIdColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn TSubscriptionIdColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn BSubscribedColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn DtLastActionDateColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn BHtmlFormatColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn DtStampColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Id = @"Id";
			 public static string UserId = @"UserId";
			 public static string TSubscriptionId = @"TSubscriptionId";
			 public static string BSubscribed = @"bSubscribed";
			 public static string DtLastActionDate = @"dtLastActionDate";
			 public static string BHtmlFormat = @"bHtmlFormat";
			 public static string DtStamp = @"dtStamp";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
