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
	/// Strongly-typed collection for the FraudScreen class.
	/// </summary>
    [Serializable]
	public partial class FraudScreenCollection : ActiveList<FraudScreen, FraudScreenCollection>
	{	   
		public FraudScreenCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>FraudScreenCollection</returns>
		public FraudScreenCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                FraudScreen o = this[i];
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
	/// This is an ActiveRecord class which wraps the FraudScreen table.
	/// </summary>
	[Serializable]
	public partial class FraudScreen : ActiveRecord<FraudScreen>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public FraudScreen()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public FraudScreen(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public FraudScreen(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public FraudScreen(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("FraudScreen", TableType.Table, DataService.GetInstance("WillCall"));
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
				
				TableSchema.TableColumn colvarCreatedById = new TableSchema.TableColumn(schema);
				colvarCreatedById.ColumnName = "CreatedById";
				colvarCreatedById.DataType = DbType.Guid;
				colvarCreatedById.MaxLength = 0;
				colvarCreatedById.AutoIncrement = false;
				colvarCreatedById.IsNullable = false;
				colvarCreatedById.IsPrimaryKey = false;
				colvarCreatedById.IsForeignKey = true;
				colvarCreatedById.IsReadOnly = false;
				colvarCreatedById.DefaultSetting = @"";
				
					colvarCreatedById.ForeignKeyTableName = "aspnet_Users";
				schema.Columns.Add(colvarCreatedById);
				
				TableSchema.TableColumn colvarCreatedBy = new TableSchema.TableColumn(schema);
				colvarCreatedBy.ColumnName = "CreatedBy";
				colvarCreatedBy.DataType = DbType.AnsiString;
				colvarCreatedBy.MaxLength = 256;
				colvarCreatedBy.AutoIncrement = false;
				colvarCreatedBy.IsNullable = false;
				colvarCreatedBy.IsPrimaryKey = false;
				colvarCreatedBy.IsForeignKey = false;
				colvarCreatedBy.IsReadOnly = false;
				colvarCreatedBy.DefaultSetting = @"";
				colvarCreatedBy.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCreatedBy);
				
				TableSchema.TableColumn colvarVcAction = new TableSchema.TableColumn(schema);
				colvarVcAction.ColumnName = "vcAction";
				colvarVcAction.DataType = DbType.AnsiString;
				colvarVcAction.MaxLength = 50;
				colvarVcAction.AutoIncrement = false;
				colvarVcAction.IsNullable = false;
				colvarVcAction.IsPrimaryKey = false;
				colvarVcAction.IsForeignKey = false;
				colvarVcAction.IsReadOnly = false;
				colvarVcAction.DefaultSetting = @"";
				colvarVcAction.ForeignKeyTableName = "";
				schema.Columns.Add(colvarVcAction);
				
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
				
				TableSchema.TableColumn colvarFirstName = new TableSchema.TableColumn(schema);
				colvarFirstName.ColumnName = "FirstName";
				colvarFirstName.DataType = DbType.AnsiString;
				colvarFirstName.MaxLength = 256;
				colvarFirstName.AutoIncrement = false;
				colvarFirstName.IsNullable = true;
				colvarFirstName.IsPrimaryKey = false;
				colvarFirstName.IsForeignKey = false;
				colvarFirstName.IsReadOnly = false;
				colvarFirstName.DefaultSetting = @"";
				colvarFirstName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarFirstName);
				
				TableSchema.TableColumn colvarMi = new TableSchema.TableColumn(schema);
				colvarMi.ColumnName = "MI";
				colvarMi.DataType = DbType.AnsiString;
				colvarMi.MaxLength = 2;
				colvarMi.AutoIncrement = false;
				colvarMi.IsNullable = true;
				colvarMi.IsPrimaryKey = false;
				colvarMi.IsForeignKey = false;
				colvarMi.IsReadOnly = false;
				colvarMi.DefaultSetting = @"";
				colvarMi.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMi);
				
				TableSchema.TableColumn colvarLastName = new TableSchema.TableColumn(schema);
				colvarLastName.ColumnName = "LastName";
				colvarLastName.DataType = DbType.AnsiString;
				colvarLastName.MaxLength = 256;
				colvarLastName.AutoIncrement = false;
				colvarLastName.IsNullable = true;
				colvarLastName.IsPrimaryKey = false;
				colvarLastName.IsForeignKey = false;
				colvarLastName.IsReadOnly = false;
				colvarLastName.DefaultSetting = @"";
				colvarLastName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarLastName);
				
				TableSchema.TableColumn colvarFullName = new TableSchema.TableColumn(schema);
				colvarFullName.ColumnName = "FullName";
				colvarFullName.DataType = DbType.AnsiString;
				colvarFullName.MaxLength = 516;
				colvarFullName.AutoIncrement = false;
				colvarFullName.IsNullable = true;
				colvarFullName.IsPrimaryKey = false;
				colvarFullName.IsForeignKey = false;
				colvarFullName.IsReadOnly = true;
				colvarFullName.DefaultSetting = @"";
				colvarFullName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarFullName);
				
				TableSchema.TableColumn colvarNameOnCard = new TableSchema.TableColumn(schema);
				colvarNameOnCard.ColumnName = "NameOnCard";
				colvarNameOnCard.DataType = DbType.AnsiString;
				colvarNameOnCard.MaxLength = 256;
				colvarNameOnCard.AutoIncrement = false;
				colvarNameOnCard.IsNullable = true;
				colvarNameOnCard.IsPrimaryKey = false;
				colvarNameOnCard.IsForeignKey = false;
				colvarNameOnCard.IsReadOnly = false;
				colvarNameOnCard.DefaultSetting = @"";
				colvarNameOnCard.ForeignKeyTableName = "";
				schema.Columns.Add(colvarNameOnCard);
				
				TableSchema.TableColumn colvarCity = new TableSchema.TableColumn(schema);
				colvarCity.ColumnName = "City";
				colvarCity.DataType = DbType.AnsiString;
				colvarCity.MaxLength = 100;
				colvarCity.AutoIncrement = false;
				colvarCity.IsNullable = true;
				colvarCity.IsPrimaryKey = false;
				colvarCity.IsForeignKey = false;
				colvarCity.IsReadOnly = false;
				colvarCity.DefaultSetting = @"";
				colvarCity.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCity);
				
				TableSchema.TableColumn colvarZip = new TableSchema.TableColumn(schema);
				colvarZip.ColumnName = "Zip";
				colvarZip.DataType = DbType.AnsiString;
				colvarZip.MaxLength = 25;
				colvarZip.AutoIncrement = false;
				colvarZip.IsNullable = true;
				colvarZip.IsPrimaryKey = false;
				colvarZip.IsForeignKey = false;
				colvarZip.IsReadOnly = false;
				colvarZip.DefaultSetting = @"";
				colvarZip.ForeignKeyTableName = "";
				schema.Columns.Add(colvarZip);
				
				TableSchema.TableColumn colvarCreditCardNum = new TableSchema.TableColumn(schema);
				colvarCreditCardNum.ColumnName = "CreditCardNum";
				colvarCreditCardNum.DataType = DbType.AnsiString;
				colvarCreditCardNum.MaxLength = 50;
				colvarCreditCardNum.AutoIncrement = false;
				colvarCreditCardNum.IsNullable = true;
				colvarCreditCardNum.IsPrimaryKey = false;
				colvarCreditCardNum.IsForeignKey = false;
				colvarCreditCardNum.IsReadOnly = false;
				colvarCreditCardNum.DefaultSetting = @"";
				colvarCreditCardNum.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCreditCardNum);
				
				TableSchema.TableColumn colvarLastFourDigits = new TableSchema.TableColumn(schema);
				colvarLastFourDigits.ColumnName = "LastFourDigits";
				colvarLastFourDigits.DataType = DbType.AnsiStringFixedLength;
				colvarLastFourDigits.MaxLength = 4;
				colvarLastFourDigits.AutoIncrement = false;
				colvarLastFourDigits.IsNullable = true;
				colvarLastFourDigits.IsPrimaryKey = false;
				colvarLastFourDigits.IsForeignKey = false;
				colvarLastFourDigits.IsReadOnly = false;
				colvarLastFourDigits.DefaultSetting = @"";
				colvarLastFourDigits.ForeignKeyTableName = "";
				schema.Columns.Add(colvarLastFourDigits);
				
				TableSchema.TableColumn colvarUserIp = new TableSchema.TableColumn(schema);
				colvarUserIp.ColumnName = "UserIp";
				colvarUserIp.DataType = DbType.AnsiString;
				colvarUserIp.MaxLength = 25;
				colvarUserIp.AutoIncrement = false;
				colvarUserIp.IsNullable = true;
				colvarUserIp.IsPrimaryKey = false;
				colvarUserIp.IsForeignKey = false;
				colvarUserIp.IsReadOnly = false;
				colvarUserIp.DefaultSetting = @"";
				colvarUserIp.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserIp);
				
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
				DataService.Providers["WillCall"].AddSchema("FraudScreen",schema);
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
		  
		[XmlAttribute("CreatedById")]
		[Bindable(true)]
		public Guid CreatedById 
		{
			get { return GetColumnValue<Guid>(Columns.CreatedById); }
			set { SetColumnValue(Columns.CreatedById, value); }
		}
		  
		[XmlAttribute("CreatedBy")]
		[Bindable(true)]
		public string CreatedBy 
		{
			get { return GetColumnValue<string>(Columns.CreatedBy); }
			set { SetColumnValue(Columns.CreatedBy, value); }
		}
		  
		[XmlAttribute("VcAction")]
		[Bindable(true)]
		public string VcAction 
		{
			get { return GetColumnValue<string>(Columns.VcAction); }
			set { SetColumnValue(Columns.VcAction, value); }
		}
		  
		[XmlAttribute("UserId")]
		[Bindable(true)]
		public Guid? UserId 
		{
			get { return GetColumnValue<Guid?>(Columns.UserId); }
			set { SetColumnValue(Columns.UserId, value); }
		}
		  
		[XmlAttribute("FirstName")]
		[Bindable(true)]
		public string FirstName 
		{
			get { return GetColumnValue<string>(Columns.FirstName); }
			set { SetColumnValue(Columns.FirstName, value); }
		}
		  
		[XmlAttribute("Mi")]
		[Bindable(true)]
		public string Mi 
		{
			get { return GetColumnValue<string>(Columns.Mi); }
			set { SetColumnValue(Columns.Mi, value); }
		}
		  
		[XmlAttribute("LastName")]
		[Bindable(true)]
		public string LastName 
		{
			get { return GetColumnValue<string>(Columns.LastName); }
			set { SetColumnValue(Columns.LastName, value); }
		}
		  
		[XmlAttribute("FullName")]
		[Bindable(true)]
		public string FullName 
		{
			get { return GetColumnValue<string>(Columns.FullName); }
			set { SetColumnValue(Columns.FullName, value); }
		}
		  
		[XmlAttribute("NameOnCard")]
		[Bindable(true)]
		public string NameOnCard 
		{
			get { return GetColumnValue<string>(Columns.NameOnCard); }
			set { SetColumnValue(Columns.NameOnCard, value); }
		}
		  
		[XmlAttribute("City")]
		[Bindable(true)]
		public string City 
		{
			get { return GetColumnValue<string>(Columns.City); }
			set { SetColumnValue(Columns.City, value); }
		}
		  
		[XmlAttribute("Zip")]
		[Bindable(true)]
		public string Zip 
		{
			get { return GetColumnValue<string>(Columns.Zip); }
			set { SetColumnValue(Columns.Zip, value); }
		}
		  
		[XmlAttribute("CreditCardNum")]
		[Bindable(true)]
		public string CreditCardNum 
		{
			get { return GetColumnValue<string>(Columns.CreditCardNum); }
			set { SetColumnValue(Columns.CreditCardNum, value); }
		}
		  
		[XmlAttribute("LastFourDigits")]
		[Bindable(true)]
		public string LastFourDigits 
		{
			get { return GetColumnValue<string>(Columns.LastFourDigits); }
			set { SetColumnValue(Columns.LastFourDigits, value); }
		}
		  
		[XmlAttribute("UserIp")]
		[Bindable(true)]
		public string UserIp 
		{
			get { return GetColumnValue<string>(Columns.UserIp); }
			set { SetColumnValue(Columns.UserIp, value); }
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
		/// Returns a AspnetApplication ActiveRecord object related to this FraudScreen
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
		/// Returns a AspnetUser ActiveRecord object related to this FraudScreen
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
		/// Returns a AspnetUser ActiveRecord object related to this FraudScreen
		/// 
		/// </summary>
		private Wcss.AspnetUser AspnetUserToCreatedById
		{
			get { return Wcss.AspnetUser.FetchByID(this.CreatedById); }
			set { SetColumnValue("CreatedById", value.UserId); }
		}
        //set up an alternate mechanism to avoid a database call
		private Wcss.AspnetUser _aspnetusertocreatedbyidrecord = null;
		
		public Wcss.AspnetUser AspnetUserToCreatedByIdRecord
		{
		    get
            {
                if (_aspnetusertocreatedbyidrecord == null)
                {
                    _aspnetusertocreatedbyidrecord = new Wcss.AspnetUser();
                    _aspnetusertocreatedbyidrecord.CopyFrom(this.AspnetUserToCreatedById);
                }
                return _aspnetusertocreatedbyidrecord;
            }
            set
            {
                if(value != null && _aspnetusertocreatedbyidrecord == null)
			        _aspnetusertocreatedbyidrecord = new Wcss.AspnetUser();
                
                SetColumnValue("CreatedById", value.UserId);
                _aspnetusertocreatedbyidrecord.CopyFrom(value);                
            }
		}
		
		
		#endregion
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(DateTime varDtStamp,Guid varCreatedById,string varCreatedBy,string varVcAction,Guid? varUserId,string varFirstName,string varMi,string varLastName,string varFullName,string varNameOnCard,string varCity,string varZip,string varCreditCardNum,string varLastFourDigits,string varUserIp,Guid varApplicationId)
		{
			FraudScreen item = new FraudScreen();
			
			item.DtStamp = varDtStamp;
			
			item.CreatedById = varCreatedById;
			
			item.CreatedBy = varCreatedBy;
			
			item.VcAction = varVcAction;
			
			item.UserId = varUserId;
			
			item.FirstName = varFirstName;
			
			item.Mi = varMi;
			
			item.LastName = varLastName;
			
			item.FullName = varFullName;
			
			item.NameOnCard = varNameOnCard;
			
			item.City = varCity;
			
			item.Zip = varZip;
			
			item.CreditCardNum = varCreditCardNum;
			
			item.LastFourDigits = varLastFourDigits;
			
			item.UserIp = varUserIp;
			
			item.ApplicationId = varApplicationId;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varId,DateTime varDtStamp,Guid varCreatedById,string varCreatedBy,string varVcAction,Guid? varUserId,string varFirstName,string varMi,string varLastName,string varFullName,string varNameOnCard,string varCity,string varZip,string varCreditCardNum,string varLastFourDigits,string varUserIp,Guid varApplicationId)
		{
			FraudScreen item = new FraudScreen();
			
				item.Id = varId;
			
				item.DtStamp = varDtStamp;
			
				item.CreatedById = varCreatedById;
			
				item.CreatedBy = varCreatedBy;
			
				item.VcAction = varVcAction;
			
				item.UserId = varUserId;
			
				item.FirstName = varFirstName;
			
				item.Mi = varMi;
			
				item.LastName = varLastName;
			
				item.FullName = varFullName;
			
				item.NameOnCard = varNameOnCard;
			
				item.City = varCity;
			
				item.Zip = varZip;
			
				item.CreditCardNum = varCreditCardNum;
			
				item.LastFourDigits = varLastFourDigits;
			
				item.UserIp = varUserIp;
			
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
        
        
        
        public static TableSchema.TableColumn CreatedByIdColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedByColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn VcActionColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn UserIdColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn FirstNameColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn MiColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn LastNameColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn FullNameColumn
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        public static TableSchema.TableColumn NameOnCardColumn
        {
            get { return Schema.Columns[10]; }
        }
        
        
        
        public static TableSchema.TableColumn CityColumn
        {
            get { return Schema.Columns[11]; }
        }
        
        
        
        public static TableSchema.TableColumn ZipColumn
        {
            get { return Schema.Columns[12]; }
        }
        
        
        
        public static TableSchema.TableColumn CreditCardNumColumn
        {
            get { return Schema.Columns[13]; }
        }
        
        
        
        public static TableSchema.TableColumn LastFourDigitsColumn
        {
            get { return Schema.Columns[14]; }
        }
        
        
        
        public static TableSchema.TableColumn UserIpColumn
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
			 public static string DtStamp = @"dtStamp";
			 public static string CreatedById = @"CreatedById";
			 public static string CreatedBy = @"CreatedBy";
			 public static string VcAction = @"vcAction";
			 public static string UserId = @"UserId";
			 public static string FirstName = @"FirstName";
			 public static string Mi = @"MI";
			 public static string LastName = @"LastName";
			 public static string FullName = @"FullName";
			 public static string NameOnCard = @"NameOnCard";
			 public static string City = @"City";
			 public static string Zip = @"Zip";
			 public static string CreditCardNum = @"CreditCardNum";
			 public static string LastFourDigits = @"LastFourDigits";
			 public static string UserIp = @"UserIp";
			 public static string ApplicationId = @"ApplicationId";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
