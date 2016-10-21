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
	/// Strongly-typed collection for the UserCouponRedemption class.
	/// </summary>
    [Serializable]
	public partial class UserCouponRedemptionCollection : ActiveList<UserCouponRedemption, UserCouponRedemptionCollection>
	{	   
		public UserCouponRedemptionCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>UserCouponRedemptionCollection</returns>
		public UserCouponRedemptionCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                UserCouponRedemption o = this[i];
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
	/// This is an ActiveRecord class which wraps the UserCouponRedemption table.
	/// </summary>
	[Serializable]
	public partial class UserCouponRedemption : ActiveRecord<UserCouponRedemption>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public UserCouponRedemption()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public UserCouponRedemption(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public UserCouponRedemption(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public UserCouponRedemption(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("UserCouponRedemption", TableType.Table, DataService.GetInstance("WillCall"));
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
				
				TableSchema.TableColumn colvarDtApplied = new TableSchema.TableColumn(schema);
				colvarDtApplied.ColumnName = "dtApplied";
				colvarDtApplied.DataType = DbType.DateTime;
				colvarDtApplied.MaxLength = 0;
				colvarDtApplied.AutoIncrement = false;
				colvarDtApplied.IsNullable = true;
				colvarDtApplied.IsPrimaryKey = false;
				colvarDtApplied.IsForeignKey = false;
				colvarDtApplied.IsReadOnly = false;
				colvarDtApplied.DefaultSetting = @"";
				colvarDtApplied.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDtApplied);
				
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
				
				TableSchema.TableColumn colvarTSalePromotionId = new TableSchema.TableColumn(schema);
				colvarTSalePromotionId.ColumnName = "TSalePromotionId";
				colvarTSalePromotionId.DataType = DbType.Int32;
				colvarTSalePromotionId.MaxLength = 0;
				colvarTSalePromotionId.AutoIncrement = false;
				colvarTSalePromotionId.IsNullable = false;
				colvarTSalePromotionId.IsPrimaryKey = false;
				colvarTSalePromotionId.IsForeignKey = true;
				colvarTSalePromotionId.IsReadOnly = false;
				colvarTSalePromotionId.DefaultSetting = @"";
				
					colvarTSalePromotionId.ForeignKeyTableName = "SalePromotion";
				schema.Columns.Add(colvarTSalePromotionId);
				
				TableSchema.TableColumn colvarCouponCode = new TableSchema.TableColumn(schema);
				colvarCouponCode.ColumnName = "CouponCode";
				colvarCouponCode.DataType = DbType.AnsiString;
				colvarCouponCode.MaxLength = 256;
				colvarCouponCode.AutoIncrement = false;
				colvarCouponCode.IsNullable = false;
				colvarCouponCode.IsPrimaryKey = false;
				colvarCouponCode.IsForeignKey = false;
				colvarCouponCode.IsReadOnly = false;
				colvarCouponCode.DefaultSetting = @"";
				colvarCouponCode.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCouponCode);
				
				TableSchema.TableColumn colvarCodeRoot = new TableSchema.TableColumn(schema);
				colvarCodeRoot.ColumnName = "CodeRoot";
				colvarCodeRoot.DataType = DbType.AnsiString;
				colvarCodeRoot.MaxLength = 256;
				colvarCodeRoot.AutoIncrement = false;
				colvarCodeRoot.IsNullable = true;
				colvarCodeRoot.IsPrimaryKey = false;
				colvarCodeRoot.IsForeignKey = false;
				colvarCodeRoot.IsReadOnly = true;
				colvarCodeRoot.DefaultSetting = @"";
				colvarCodeRoot.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCodeRoot);
				
				TableSchema.TableColumn colvarMDiscountAmount = new TableSchema.TableColumn(schema);
				colvarMDiscountAmount.ColumnName = "mDiscountAmount";
				colvarMDiscountAmount.DataType = DbType.Currency;
				colvarMDiscountAmount.MaxLength = 0;
				colvarMDiscountAmount.AutoIncrement = false;
				colvarMDiscountAmount.IsNullable = false;
				colvarMDiscountAmount.IsPrimaryKey = false;
				colvarMDiscountAmount.IsForeignKey = false;
				colvarMDiscountAmount.IsReadOnly = false;
				
						colvarMDiscountAmount.DefaultSetting = @"((0))";
				colvarMDiscountAmount.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMDiscountAmount);
				
				TableSchema.TableColumn colvarMInvoiceAmount = new TableSchema.TableColumn(schema);
				colvarMInvoiceAmount.ColumnName = "mInvoiceAmount";
				colvarMInvoiceAmount.DataType = DbType.Currency;
				colvarMInvoiceAmount.MaxLength = 0;
				colvarMInvoiceAmount.AutoIncrement = false;
				colvarMInvoiceAmount.IsNullable = false;
				colvarMInvoiceAmount.IsPrimaryKey = false;
				colvarMInvoiceAmount.IsForeignKey = false;
				colvarMInvoiceAmount.IsReadOnly = false;
				
						colvarMInvoiceAmount.DefaultSetting = @"((0))";
				colvarMInvoiceAmount.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMInvoiceAmount);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["WillCall"].AddSchema("UserCouponRedemption",schema);
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
		  
		[XmlAttribute("DtApplied")]
		[Bindable(true)]
		public DateTime? DtApplied 
		{
			get { return GetColumnValue<DateTime?>(Columns.DtApplied); }
			set { SetColumnValue(Columns.DtApplied, value); }
		}
		  
		[XmlAttribute("UserId")]
		[Bindable(true)]
		public Guid UserId 
		{
			get { return GetColumnValue<Guid>(Columns.UserId); }
			set { SetColumnValue(Columns.UserId, value); }
		}
		  
		[XmlAttribute("TSalePromotionId")]
		[Bindable(true)]
		public int TSalePromotionId 
		{
			get { return GetColumnValue<int>(Columns.TSalePromotionId); }
			set { SetColumnValue(Columns.TSalePromotionId, value); }
		}
		  
		[XmlAttribute("CouponCode")]
		[Bindable(true)]
		public string CouponCode 
		{
			get { return GetColumnValue<string>(Columns.CouponCode); }
			set { SetColumnValue(Columns.CouponCode, value); }
		}
		  
		[XmlAttribute("CodeRoot")]
		[Bindable(true)]
		public string CodeRoot 
		{
			get { return GetColumnValue<string>(Columns.CodeRoot); }
			set { SetColumnValue(Columns.CodeRoot, value); }
		}
		  
		[XmlAttribute("MDiscountAmount")]
		[Bindable(true)]
		public decimal MDiscountAmount 
		{
			get { return GetColumnValue<decimal>(Columns.MDiscountAmount); }
			set { SetColumnValue(Columns.MDiscountAmount, value); }
		}
		  
		[XmlAttribute("MInvoiceAmount")]
		[Bindable(true)]
		public decimal MInvoiceAmount 
		{
			get { return GetColumnValue<decimal>(Columns.MInvoiceAmount); }
			set { SetColumnValue(Columns.MInvoiceAmount, value); }
		}
		
		#endregion
		
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a AspnetUser ActiveRecord object related to this UserCouponRedemption
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
		/// Returns a SalePromotion ActiveRecord object related to this UserCouponRedemption
		/// 
		/// </summary>
		private Wcss.SalePromotion SalePromotion
		{
			get { return Wcss.SalePromotion.FetchByID(this.TSalePromotionId); }
			set { SetColumnValue("TSalePromotionId", value.Id); }
		}
        //set up an alternate mechanism to avoid a database call
		private Wcss.SalePromotion _salepromotionrecord = null;
		
		public Wcss.SalePromotion SalePromotionRecord
		{
		    get
            {
                if (_salepromotionrecord == null)
                {
                    _salepromotionrecord = new Wcss.SalePromotion();
                    _salepromotionrecord.CopyFrom(this.SalePromotion);
                }
                return _salepromotionrecord;
            }
            set
            {
                if(value != null && _salepromotionrecord == null)
			        _salepromotionrecord = new Wcss.SalePromotion();
                
                SetColumnValue("TSalePromotionId", value.Id);
                _salepromotionrecord.CopyFrom(value);                
            }
		}
		
		
		#endregion
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(DateTime varDtStamp,DateTime? varDtApplied,Guid varUserId,int varTSalePromotionId,string varCouponCode,string varCodeRoot,decimal varMDiscountAmount,decimal varMInvoiceAmount)
		{
			UserCouponRedemption item = new UserCouponRedemption();
			
			item.DtStamp = varDtStamp;
			
			item.DtApplied = varDtApplied;
			
			item.UserId = varUserId;
			
			item.TSalePromotionId = varTSalePromotionId;
			
			item.CouponCode = varCouponCode;
			
			item.CodeRoot = varCodeRoot;
			
			item.MDiscountAmount = varMDiscountAmount;
			
			item.MInvoiceAmount = varMInvoiceAmount;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varId,DateTime varDtStamp,DateTime? varDtApplied,Guid varUserId,int varTSalePromotionId,string varCouponCode,string varCodeRoot,decimal varMDiscountAmount,decimal varMInvoiceAmount)
		{
			UserCouponRedemption item = new UserCouponRedemption();
			
				item.Id = varId;
			
				item.DtStamp = varDtStamp;
			
				item.DtApplied = varDtApplied;
			
				item.UserId = varUserId;
			
				item.TSalePromotionId = varTSalePromotionId;
			
				item.CouponCode = varCouponCode;
			
				item.CodeRoot = varCodeRoot;
			
				item.MDiscountAmount = varMDiscountAmount;
			
				item.MInvoiceAmount = varMInvoiceAmount;
			
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
        
        
        
        public static TableSchema.TableColumn DtAppliedColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn UserIdColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn TSalePromotionIdColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn CouponCodeColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn CodeRootColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn MDiscountAmountColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn MInvoiceAmountColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Id = @"Id";
			 public static string DtStamp = @"dtStamp";
			 public static string DtApplied = @"dtApplied";
			 public static string UserId = @"UserId";
			 public static string TSalePromotionId = @"TSalePromotionId";
			 public static string CouponCode = @"CouponCode";
			 public static string CodeRoot = @"CodeRoot";
			 public static string MDiscountAmount = @"mDiscountAmount";
			 public static string MInvoiceAmount = @"mInvoiceAmount";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
