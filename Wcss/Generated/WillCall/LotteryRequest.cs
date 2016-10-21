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
	/// Strongly-typed collection for the LotteryRequest class.
	/// </summary>
    [Serializable]
	public partial class LotteryRequestCollection : ActiveList<LotteryRequest, LotteryRequestCollection>
	{	   
		public LotteryRequestCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>LotteryRequestCollection</returns>
		public LotteryRequestCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                LotteryRequest o = this[i];
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
	/// This is an ActiveRecord class which wraps the LotteryRequest table.
	/// </summary>
	[Serializable]
	public partial class LotteryRequest : ActiveRecord<LotteryRequest>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public LotteryRequest()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public LotteryRequest(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public LotteryRequest(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public LotteryRequest(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("LotteryRequest", TableType.Table, DataService.GetInstance("WillCall"));
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
				
				TableSchema.TableColumn colvarGuid = new TableSchema.TableColumn(schema);
				colvarGuid.ColumnName = "GUID";
				colvarGuid.DataType = DbType.Guid;
				colvarGuid.MaxLength = 0;
				colvarGuid.AutoIncrement = false;
				colvarGuid.IsNullable = false;
				colvarGuid.IsPrimaryKey = false;
				colvarGuid.IsForeignKey = false;
				colvarGuid.IsReadOnly = false;
				
						colvarGuid.DefaultSetting = @"(newid())";
				colvarGuid.ForeignKeyTableName = "";
				schema.Columns.Add(colvarGuid);
				
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
				
				TableSchema.TableColumn colvarTLotteryId = new TableSchema.TableColumn(schema);
				colvarTLotteryId.ColumnName = "TLotteryId";
				colvarTLotteryId.DataType = DbType.Int32;
				colvarTLotteryId.MaxLength = 0;
				colvarTLotteryId.AutoIncrement = false;
				colvarTLotteryId.IsNullable = false;
				colvarTLotteryId.IsPrimaryKey = false;
				colvarTLotteryId.IsForeignKey = true;
				colvarTLotteryId.IsReadOnly = false;
				colvarTLotteryId.DefaultSetting = @"";
				
					colvarTLotteryId.ForeignKeyTableName = "Lottery";
				schema.Columns.Add(colvarTLotteryId);
				
				TableSchema.TableColumn colvarUserId = new TableSchema.TableColumn(schema);
				colvarUserId.ColumnName = "UserId";
				colvarUserId.DataType = DbType.Guid;
				colvarUserId.MaxLength = 0;
				colvarUserId.AutoIncrement = false;
				colvarUserId.IsNullable = false;
				colvarUserId.IsPrimaryKey = false;
				colvarUserId.IsForeignKey = true;
				colvarUserId.IsReadOnly = false;
				colvarUserId.DefaultSetting = @"";
				
					colvarUserId.ForeignKeyTableName = "aspnet_Users";
				schema.Columns.Add(colvarUserId);
				
				TableSchema.TableColumn colvarUserName = new TableSchema.TableColumn(schema);
				colvarUserName.ColumnName = "UserName";
				colvarUserName.DataType = DbType.String;
				colvarUserName.MaxLength = 50;
				colvarUserName.AutoIncrement = false;
				colvarUserName.IsNullable = false;
				colvarUserName.IsPrimaryKey = false;
				colvarUserName.IsForeignKey = false;
				colvarUserName.IsReadOnly = false;
				colvarUserName.DefaultSetting = @"";
				colvarUserName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserName);
				
				TableSchema.TableColumn colvarVcStatus = new TableSchema.TableColumn(schema);
				colvarVcStatus.ColumnName = "vcStatus";
				colvarVcStatus.DataType = DbType.AnsiString;
				colvarVcStatus.MaxLength = 50;
				colvarVcStatus.AutoIncrement = false;
				colvarVcStatus.IsNullable = true;
				colvarVcStatus.IsPrimaryKey = false;
				colvarVcStatus.IsForeignKey = false;
				colvarVcStatus.IsReadOnly = false;
				colvarVcStatus.DefaultSetting = @"";
				colvarVcStatus.ForeignKeyTableName = "";
				schema.Columns.Add(colvarVcStatus);
				
				TableSchema.TableColumn colvarDtStatus = new TableSchema.TableColumn(schema);
				colvarDtStatus.ColumnName = "dtStatus";
				colvarDtStatus.DataType = DbType.DateTime;
				colvarDtStatus.MaxLength = 0;
				colvarDtStatus.AutoIncrement = false;
				colvarDtStatus.IsNullable = true;
				colvarDtStatus.IsPrimaryKey = false;
				colvarDtStatus.IsForeignKey = false;
				colvarDtStatus.IsReadOnly = false;
				colvarDtStatus.DefaultSetting = @"";
				colvarDtStatus.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDtStatus);
				
				TableSchema.TableColumn colvarStatusBy = new TableSchema.TableColumn(schema);
				colvarStatusBy.ColumnName = "StatusBy";
				colvarStatusBy.DataType = DbType.AnsiString;
				colvarStatusBy.MaxLength = 256;
				colvarStatusBy.AutoIncrement = false;
				colvarStatusBy.IsNullable = true;
				colvarStatusBy.IsPrimaryKey = false;
				colvarStatusBy.IsForeignKey = false;
				colvarStatusBy.IsReadOnly = false;
				colvarStatusBy.DefaultSetting = @"";
				colvarStatusBy.ForeignKeyTableName = "";
				schema.Columns.Add(colvarStatusBy);
				
				TableSchema.TableColumn colvarStatusNotes = new TableSchema.TableColumn(schema);
				colvarStatusNotes.ColumnName = "StatusNotes";
				colvarStatusNotes.DataType = DbType.AnsiString;
				colvarStatusNotes.MaxLength = 500;
				colvarStatusNotes.AutoIncrement = false;
				colvarStatusNotes.IsNullable = true;
				colvarStatusNotes.IsPrimaryKey = false;
				colvarStatusNotes.IsForeignKey = false;
				colvarStatusNotes.IsReadOnly = false;
				colvarStatusNotes.DefaultSetting = @"";
				colvarStatusNotes.ForeignKeyTableName = "";
				schema.Columns.Add(colvarStatusNotes);
				
				TableSchema.TableColumn colvarDtFulfilled = new TableSchema.TableColumn(schema);
				colvarDtFulfilled.ColumnName = "dtFulfilled";
				colvarDtFulfilled.DataType = DbType.DateTime;
				colvarDtFulfilled.MaxLength = 0;
				colvarDtFulfilled.AutoIncrement = false;
				colvarDtFulfilled.IsNullable = true;
				colvarDtFulfilled.IsPrimaryKey = false;
				colvarDtFulfilled.IsForeignKey = false;
				colvarDtFulfilled.IsReadOnly = false;
				colvarDtFulfilled.DefaultSetting = @"";
				colvarDtFulfilled.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDtFulfilled);
				
				TableSchema.TableColumn colvarIRequested = new TableSchema.TableColumn(schema);
				colvarIRequested.ColumnName = "iRequested";
				colvarIRequested.DataType = DbType.Int32;
				colvarIRequested.MaxLength = 0;
				colvarIRequested.AutoIncrement = false;
				colvarIRequested.IsNullable = false;
				colvarIRequested.IsPrimaryKey = false;
				colvarIRequested.IsForeignKey = false;
				colvarIRequested.IsReadOnly = false;
				
						colvarIRequested.DefaultSetting = @"((0))";
				colvarIRequested.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIRequested);
				
				TableSchema.TableColumn colvarIPurchased = new TableSchema.TableColumn(schema);
				colvarIPurchased.ColumnName = "iPurchased";
				colvarIPurchased.DataType = DbType.Int32;
				colvarIPurchased.MaxLength = 0;
				colvarIPurchased.AutoIncrement = false;
				colvarIPurchased.IsNullable = false;
				colvarIPurchased.IsPrimaryKey = false;
				colvarIPurchased.IsForeignKey = false;
				colvarIPurchased.IsReadOnly = false;
				
						colvarIPurchased.DefaultSetting = @"((0))";
				colvarIPurchased.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIPurchased);
				
				TableSchema.TableColumn colvarStatusIP = new TableSchema.TableColumn(schema);
				colvarStatusIP.ColumnName = "StatusIP";
				colvarStatusIP.DataType = DbType.AnsiString;
				colvarStatusIP.MaxLength = 25;
				colvarStatusIP.AutoIncrement = false;
				colvarStatusIP.IsNullable = true;
				colvarStatusIP.IsPrimaryKey = false;
				colvarStatusIP.IsForeignKey = false;
				colvarStatusIP.IsReadOnly = false;
				colvarStatusIP.DefaultSetting = @"";
				colvarStatusIP.ForeignKeyTableName = "";
				schema.Columns.Add(colvarStatusIP);
				
				TableSchema.TableColumn colvarRequestIP = new TableSchema.TableColumn(schema);
				colvarRequestIP.ColumnName = "RequestIP";
				colvarRequestIP.DataType = DbType.AnsiString;
				colvarRequestIP.MaxLength = 25;
				colvarRequestIP.AutoIncrement = false;
				colvarRequestIP.IsNullable = true;
				colvarRequestIP.IsPrimaryKey = false;
				colvarRequestIP.IsForeignKey = false;
				colvarRequestIP.IsReadOnly = false;
				colvarRequestIP.DefaultSetting = @"";
				colvarRequestIP.ForeignKeyTableName = "";
				schema.Columns.Add(colvarRequestIP);
				
				TableSchema.TableColumn colvarFulfillIP = new TableSchema.TableColumn(schema);
				colvarFulfillIP.ColumnName = "FulfillIP";
				colvarFulfillIP.DataType = DbType.AnsiString;
				colvarFulfillIP.MaxLength = 25;
				colvarFulfillIP.AutoIncrement = false;
				colvarFulfillIP.IsNullable = true;
				colvarFulfillIP.IsPrimaryKey = false;
				colvarFulfillIP.IsForeignKey = false;
				colvarFulfillIP.IsReadOnly = false;
				colvarFulfillIP.DefaultSetting = @"";
				colvarFulfillIP.ForeignKeyTableName = "";
				schema.Columns.Add(colvarFulfillIP);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["WillCall"].AddSchema("LotteryRequest",schema);
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
		  
		[XmlAttribute("Guid")]
		[Bindable(true)]
		public Guid Guid 
		{
			get { return GetColumnValue<Guid>(Columns.Guid); }
			set { SetColumnValue(Columns.Guid, value); }
		}
		  
		[XmlAttribute("DtStamp")]
		[Bindable(true)]
		public DateTime? DtStamp 
		{
			get { return GetColumnValue<DateTime?>(Columns.DtStamp); }
			set { SetColumnValue(Columns.DtStamp, value); }
		}
		  
		[XmlAttribute("TLotteryId")]
		[Bindable(true)]
		public int TLotteryId 
		{
			get { return GetColumnValue<int>(Columns.TLotteryId); }
			set { SetColumnValue(Columns.TLotteryId, value); }
		}
		  
		[XmlAttribute("UserId")]
		[Bindable(true)]
		public Guid UserId 
		{
			get { return GetColumnValue<Guid>(Columns.UserId); }
			set { SetColumnValue(Columns.UserId, value); }
		}
		  
		[XmlAttribute("UserName")]
		[Bindable(true)]
		public string UserName 
		{
			get { return GetColumnValue<string>(Columns.UserName); }
			set { SetColumnValue(Columns.UserName, value); }
		}
		  
		[XmlAttribute("VcStatus")]
		[Bindable(true)]
		public string VcStatus 
		{
			get { return GetColumnValue<string>(Columns.VcStatus); }
			set { SetColumnValue(Columns.VcStatus, value); }
		}
		  
		[XmlAttribute("DtStatus")]
		[Bindable(true)]
		public DateTime? DtStatus 
		{
			get { return GetColumnValue<DateTime?>(Columns.DtStatus); }
			set { SetColumnValue(Columns.DtStatus, value); }
		}
		  
		[XmlAttribute("StatusBy")]
		[Bindable(true)]
		public string StatusBy 
		{
			get { return GetColumnValue<string>(Columns.StatusBy); }
			set { SetColumnValue(Columns.StatusBy, value); }
		}
		  
		[XmlAttribute("StatusNotes")]
		[Bindable(true)]
		public string StatusNotes 
		{
			get { return GetColumnValue<string>(Columns.StatusNotes); }
			set { SetColumnValue(Columns.StatusNotes, value); }
		}
		  
		[XmlAttribute("DtFulfilled")]
		[Bindable(true)]
		public DateTime? DtFulfilled 
		{
			get { return GetColumnValue<DateTime?>(Columns.DtFulfilled); }
			set { SetColumnValue(Columns.DtFulfilled, value); }
		}
		  
		[XmlAttribute("IRequested")]
		[Bindable(true)]
		public int IRequested 
		{
			get { return GetColumnValue<int>(Columns.IRequested); }
			set { SetColumnValue(Columns.IRequested, value); }
		}
		  
		[XmlAttribute("IPurchased")]
		[Bindable(true)]
		public int IPurchased 
		{
			get { return GetColumnValue<int>(Columns.IPurchased); }
			set { SetColumnValue(Columns.IPurchased, value); }
		}
		  
		[XmlAttribute("StatusIP")]
		[Bindable(true)]
		public string StatusIP 
		{
			get { return GetColumnValue<string>(Columns.StatusIP); }
			set { SetColumnValue(Columns.StatusIP, value); }
		}
		  
		[XmlAttribute("RequestIP")]
		[Bindable(true)]
		public string RequestIP 
		{
			get { return GetColumnValue<string>(Columns.RequestIP); }
			set { SetColumnValue(Columns.RequestIP, value); }
		}
		  
		[XmlAttribute("FulfillIP")]
		[Bindable(true)]
		public string FulfillIP 
		{
			get { return GetColumnValue<string>(Columns.FulfillIP); }
			set { SetColumnValue(Columns.FulfillIP, value); }
		}
		
		#endregion
		
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a AspnetUser ActiveRecord object related to this LotteryRequest
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
		/// Returns a Lottery ActiveRecord object related to this LotteryRequest
		/// 
		/// </summary>
		private Wcss.Lottery Lottery
		{
			get { return Wcss.Lottery.FetchByID(this.TLotteryId); }
			set { SetColumnValue("TLotteryId", value.Id); }
		}
        //set up an alternate mechanism to avoid a database call
		private Wcss.Lottery _lotteryrecord = null;
		
		public Wcss.Lottery LotteryRecord
		{
		    get
            {
                if (_lotteryrecord == null)
                {
                    _lotteryrecord = new Wcss.Lottery();
                    _lotteryrecord.CopyFrom(this.Lottery);
                }
                return _lotteryrecord;
            }
            set
            {
                if(value != null && _lotteryrecord == null)
			        _lotteryrecord = new Wcss.Lottery();
                
                SetColumnValue("TLotteryId", value.Id);
                _lotteryrecord.CopyFrom(value);                
            }
		}
		
		
		#endregion
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(Guid varGuid,DateTime? varDtStamp,int varTLotteryId,Guid varUserId,string varUserName,string varVcStatus,DateTime? varDtStatus,string varStatusBy,string varStatusNotes,DateTime? varDtFulfilled,int varIRequested,int varIPurchased,string varStatusIP,string varRequestIP,string varFulfillIP)
		{
			LotteryRequest item = new LotteryRequest();
			
			item.Guid = varGuid;
			
			item.DtStamp = varDtStamp;
			
			item.TLotteryId = varTLotteryId;
			
			item.UserId = varUserId;
			
			item.UserName = varUserName;
			
			item.VcStatus = varVcStatus;
			
			item.DtStatus = varDtStatus;
			
			item.StatusBy = varStatusBy;
			
			item.StatusNotes = varStatusNotes;
			
			item.DtFulfilled = varDtFulfilled;
			
			item.IRequested = varIRequested;
			
			item.IPurchased = varIPurchased;
			
			item.StatusIP = varStatusIP;
			
			item.RequestIP = varRequestIP;
			
			item.FulfillIP = varFulfillIP;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varId,Guid varGuid,DateTime? varDtStamp,int varTLotteryId,Guid varUserId,string varUserName,string varVcStatus,DateTime? varDtStatus,string varStatusBy,string varStatusNotes,DateTime? varDtFulfilled,int varIRequested,int varIPurchased,string varStatusIP,string varRequestIP,string varFulfillIP)
		{
			LotteryRequest item = new LotteryRequest();
			
				item.Id = varId;
			
				item.Guid = varGuid;
			
				item.DtStamp = varDtStamp;
			
				item.TLotteryId = varTLotteryId;
			
				item.UserId = varUserId;
			
				item.UserName = varUserName;
			
				item.VcStatus = varVcStatus;
			
				item.DtStatus = varDtStatus;
			
				item.StatusBy = varStatusBy;
			
				item.StatusNotes = varStatusNotes;
			
				item.DtFulfilled = varDtFulfilled;
			
				item.IRequested = varIRequested;
			
				item.IPurchased = varIPurchased;
			
				item.StatusIP = varStatusIP;
			
				item.RequestIP = varRequestIP;
			
				item.FulfillIP = varFulfillIP;
			
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
        
        
        
        public static TableSchema.TableColumn GuidColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn DtStampColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn TLotteryIdColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn UserIdColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn UserNameColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn VcStatusColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn DtStatusColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn StatusByColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn StatusNotesColumn
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        public static TableSchema.TableColumn DtFulfilledColumn
        {
            get { return Schema.Columns[10]; }
        }
        
        
        
        public static TableSchema.TableColumn IRequestedColumn
        {
            get { return Schema.Columns[11]; }
        }
        
        
        
        public static TableSchema.TableColumn IPurchasedColumn
        {
            get { return Schema.Columns[12]; }
        }
        
        
        
        public static TableSchema.TableColumn StatusIPColumn
        {
            get { return Schema.Columns[13]; }
        }
        
        
        
        public static TableSchema.TableColumn RequestIPColumn
        {
            get { return Schema.Columns[14]; }
        }
        
        
        
        public static TableSchema.TableColumn FulfillIPColumn
        {
            get { return Schema.Columns[15]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Id = @"Id";
			 public static string Guid = @"GUID";
			 public static string DtStamp = @"dtStamp";
			 public static string TLotteryId = @"TLotteryId";
			 public static string UserId = @"UserId";
			 public static string UserName = @"UserName";
			 public static string VcStatus = @"vcStatus";
			 public static string DtStatus = @"dtStatus";
			 public static string StatusBy = @"StatusBy";
			 public static string StatusNotes = @"StatusNotes";
			 public static string DtFulfilled = @"dtFulfilled";
			 public static string IRequested = @"iRequested";
			 public static string IPurchased = @"iPurchased";
			 public static string StatusIP = @"StatusIP";
			 public static string RequestIP = @"RequestIP";
			 public static string FulfillIP = @"FulfillIP";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
