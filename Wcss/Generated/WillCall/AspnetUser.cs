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
	/// Strongly-typed collection for the AspnetUser class.
	/// </summary>
    [Serializable]
	public partial class AspnetUserCollection : ActiveList<AspnetUser, AspnetUserCollection>
	{	   
		public AspnetUserCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>AspnetUserCollection</returns>
		public AspnetUserCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                AspnetUser o = this[i];
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
	/// This is an ActiveRecord class which wraps the aspnet_Users table.
	/// </summary>
	[Serializable]
	public partial class AspnetUser : ActiveRecord<AspnetUser>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public AspnetUser()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public AspnetUser(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public AspnetUser(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public AspnetUser(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("aspnet_Users", TableType.Table, DataService.GetInstance("WillCall"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
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
				
				TableSchema.TableColumn colvarUserId = new TableSchema.TableColumn(schema);
				colvarUserId.ColumnName = "UserId";
				colvarUserId.DataType = DbType.Guid;
				colvarUserId.MaxLength = 0;
				colvarUserId.AutoIncrement = false;
				colvarUserId.IsNullable = false;
				colvarUserId.IsPrimaryKey = true;
				colvarUserId.IsForeignKey = false;
				colvarUserId.IsReadOnly = false;
				
						colvarUserId.DefaultSetting = @"(newid())";
				colvarUserId.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserId);
				
				TableSchema.TableColumn colvarUserName = new TableSchema.TableColumn(schema);
				colvarUserName.ColumnName = "UserName";
				colvarUserName.DataType = DbType.String;
				colvarUserName.MaxLength = 256;
				colvarUserName.AutoIncrement = false;
				colvarUserName.IsNullable = false;
				colvarUserName.IsPrimaryKey = false;
				colvarUserName.IsForeignKey = false;
				colvarUserName.IsReadOnly = false;
				colvarUserName.DefaultSetting = @"";
				colvarUserName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserName);
				
				TableSchema.TableColumn colvarLoweredUserName = new TableSchema.TableColumn(schema);
				colvarLoweredUserName.ColumnName = "LoweredUserName";
				colvarLoweredUserName.DataType = DbType.String;
				colvarLoweredUserName.MaxLength = 256;
				colvarLoweredUserName.AutoIncrement = false;
				colvarLoweredUserName.IsNullable = false;
				colvarLoweredUserName.IsPrimaryKey = false;
				colvarLoweredUserName.IsForeignKey = false;
				colvarLoweredUserName.IsReadOnly = false;
				colvarLoweredUserName.DefaultSetting = @"";
				colvarLoweredUserName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarLoweredUserName);
				
				TableSchema.TableColumn colvarMobileAlias = new TableSchema.TableColumn(schema);
				colvarMobileAlias.ColumnName = "MobileAlias";
				colvarMobileAlias.DataType = DbType.String;
				colvarMobileAlias.MaxLength = 16;
				colvarMobileAlias.AutoIncrement = false;
				colvarMobileAlias.IsNullable = true;
				colvarMobileAlias.IsPrimaryKey = false;
				colvarMobileAlias.IsForeignKey = false;
				colvarMobileAlias.IsReadOnly = false;
				
						colvarMobileAlias.DefaultSetting = @"(NULL)";
				colvarMobileAlias.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMobileAlias);
				
				TableSchema.TableColumn colvarIsAnonymous = new TableSchema.TableColumn(schema);
				colvarIsAnonymous.ColumnName = "IsAnonymous";
				colvarIsAnonymous.DataType = DbType.Boolean;
				colvarIsAnonymous.MaxLength = 0;
				colvarIsAnonymous.AutoIncrement = false;
				colvarIsAnonymous.IsNullable = false;
				colvarIsAnonymous.IsPrimaryKey = false;
				colvarIsAnonymous.IsForeignKey = false;
				colvarIsAnonymous.IsReadOnly = false;
				
						colvarIsAnonymous.DefaultSetting = @"((0))";
				colvarIsAnonymous.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIsAnonymous);
				
				TableSchema.TableColumn colvarLastActivityDate = new TableSchema.TableColumn(schema);
				colvarLastActivityDate.ColumnName = "LastActivityDate";
				colvarLastActivityDate.DataType = DbType.DateTime;
				colvarLastActivityDate.MaxLength = 0;
				colvarLastActivityDate.AutoIncrement = false;
				colvarLastActivityDate.IsNullable = false;
				colvarLastActivityDate.IsPrimaryKey = false;
				colvarLastActivityDate.IsForeignKey = false;
				colvarLastActivityDate.IsReadOnly = false;
				colvarLastActivityDate.DefaultSetting = @"";
				colvarLastActivityDate.ForeignKeyTableName = "";
				schema.Columns.Add(colvarLastActivityDate);
				
				TableSchema.TableColumn colvarCustomerId = new TableSchema.TableColumn(schema);
				colvarCustomerId.ColumnName = "CustomerId";
				colvarCustomerId.DataType = DbType.Int32;
				colvarCustomerId.MaxLength = 0;
				colvarCustomerId.AutoIncrement = true;
				colvarCustomerId.IsNullable = false;
				colvarCustomerId.IsPrimaryKey = false;
				colvarCustomerId.IsForeignKey = false;
				colvarCustomerId.IsReadOnly = false;
				colvarCustomerId.DefaultSetting = @"";
				colvarCustomerId.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCustomerId);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["WillCall"].AddSchema("aspnet_Users",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("ApplicationId")]
		[Bindable(true)]
		public Guid ApplicationId 
		{
			get { return GetColumnValue<Guid>(Columns.ApplicationId); }
			set { SetColumnValue(Columns.ApplicationId, value); }
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
		  
		[XmlAttribute("LoweredUserName")]
		[Bindable(true)]
		public string LoweredUserName 
		{
			get { return GetColumnValue<string>(Columns.LoweredUserName); }
			set { SetColumnValue(Columns.LoweredUserName, value); }
		}
		  
		[XmlAttribute("MobileAlias")]
		[Bindable(true)]
		public string MobileAlias 
		{
			get { return GetColumnValue<string>(Columns.MobileAlias); }
			set { SetColumnValue(Columns.MobileAlias, value); }
		}
		  
		[XmlAttribute("IsAnonymous")]
		[Bindable(true)]
		public bool IsAnonymous 
		{
			get { return GetColumnValue<bool>(Columns.IsAnonymous); }
			set { SetColumnValue(Columns.IsAnonymous, value); }
		}
		  
		[XmlAttribute("LastActivityDate")]
		[Bindable(true)]
		public DateTime LastActivityDate 
		{
			get { return GetColumnValue<DateTime>(Columns.LastActivityDate); }
			set { SetColumnValue(Columns.LastActivityDate, value); }
		}
		  
		[XmlAttribute("CustomerId")]
		[Bindable(true)]
		public int CustomerId 
		{
			get { return GetColumnValue<int>(Columns.CustomerId); }
			set { SetColumnValue(Columns.CustomerId, value); }
		}
		
		#endregion
		
		
		#region PrimaryKey Methods		
		
        protected override void SetPrimaryKey(object oValue)
        {
            base.SetPrimaryKey(oValue);
            
            SetPKValues();
        }
        
		
		private Wcss.AspnetMembershipCollection colAspnetMembershipRecords;
		public Wcss.AspnetMembershipCollection AspnetMembershipRecords()
		{
			if(colAspnetMembershipRecords == null)
			{
				colAspnetMembershipRecords = new Wcss.AspnetMembershipCollection().Where(AspnetMembership.Columns.UserId, UserId).Load();
				colAspnetMembershipRecords.ListChanged += new ListChangedEventHandler(colAspnetMembershipRecords_ListChanged);
			}
			return colAspnetMembershipRecords;
		}
				
		void colAspnetMembershipRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colAspnetMembershipRecords[e.NewIndex].UserId = UserId;
				colAspnetMembershipRecords.ListChanged += new ListChangedEventHandler(colAspnetMembershipRecords_ListChanged);
            }
		}
		private Wcss.AspnetPersonalizationPerUserCollection colAspnetPersonalizationPerUserRecords;
		public Wcss.AspnetPersonalizationPerUserCollection AspnetPersonalizationPerUserRecords()
		{
			if(colAspnetPersonalizationPerUserRecords == null)
			{
				colAspnetPersonalizationPerUserRecords = new Wcss.AspnetPersonalizationPerUserCollection().Where(AspnetPersonalizationPerUser.Columns.UserId, UserId).Load();
				colAspnetPersonalizationPerUserRecords.ListChanged += new ListChangedEventHandler(colAspnetPersonalizationPerUserRecords_ListChanged);
			}
			return colAspnetPersonalizationPerUserRecords;
		}
				
		void colAspnetPersonalizationPerUserRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colAspnetPersonalizationPerUserRecords[e.NewIndex].UserId = UserId;
				colAspnetPersonalizationPerUserRecords.ListChanged += new ListChangedEventHandler(colAspnetPersonalizationPerUserRecords_ListChanged);
            }
		}
		private Wcss.AspnetProfileCollection colAspnetProfileRecords;
		public Wcss.AspnetProfileCollection AspnetProfileRecords()
		{
			if(colAspnetProfileRecords == null)
			{
				colAspnetProfileRecords = new Wcss.AspnetProfileCollection().Where(AspnetProfile.Columns.UserId, UserId).Load();
				colAspnetProfileRecords.ListChanged += new ListChangedEventHandler(colAspnetProfileRecords_ListChanged);
			}
			return colAspnetProfileRecords;
		}
				
		void colAspnetProfileRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colAspnetProfileRecords[e.NewIndex].UserId = UserId;
				colAspnetProfileRecords.ListChanged += new ListChangedEventHandler(colAspnetProfileRecords_ListChanged);
            }
		}
		private Wcss.AspnetUsersInRoleCollection colAspnetUsersInRoles;
		public Wcss.AspnetUsersInRoleCollection AspnetUsersInRoles()
		{
			if(colAspnetUsersInRoles == null)
			{
				colAspnetUsersInRoles = new Wcss.AspnetUsersInRoleCollection().Where(AspnetUsersInRole.Columns.UserId, UserId).Load();
				colAspnetUsersInRoles.ListChanged += new ListChangedEventHandler(colAspnetUsersInRoles_ListChanged);
			}
			return colAspnetUsersInRoles;
		}
				
		void colAspnetUsersInRoles_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colAspnetUsersInRoles[e.NewIndex].UserId = UserId;
				colAspnetUsersInRoles.ListChanged += new ListChangedEventHandler(colAspnetUsersInRoles_ListChanged);
            }
		}
		private Wcss.AuthorizeNetCollection colAuthorizeNetRecords;
		public Wcss.AuthorizeNetCollection AuthorizeNetRecords()
		{
			if(colAuthorizeNetRecords == null)
			{
				colAuthorizeNetRecords = new Wcss.AuthorizeNetCollection().Where(AuthorizeNet.Columns.UserId, UserId).Load();
				colAuthorizeNetRecords.ListChanged += new ListChangedEventHandler(colAuthorizeNetRecords_ListChanged);
			}
			return colAuthorizeNetRecords;
		}
				
		void colAuthorizeNetRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colAuthorizeNetRecords[e.NewIndex].UserId = UserId;
				colAuthorizeNetRecords.ListChanged += new ListChangedEventHandler(colAuthorizeNetRecords_ListChanged);
            }
		}
		private Wcss.CashewCollection colCashewRecords;
		public Wcss.CashewCollection CashewRecords()
		{
			if(colCashewRecords == null)
			{
				colCashewRecords = new Wcss.CashewCollection().Where(Cashew.Columns.UserId, UserId).Load();
				colCashewRecords.ListChanged += new ListChangedEventHandler(colCashewRecords_ListChanged);
			}
			return colCashewRecords;
		}
				
		void colCashewRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colCashewRecords[e.NewIndex].UserId = UserId;
				colCashewRecords.ListChanged += new ListChangedEventHandler(colCashewRecords_ListChanged);
            }
		}
		private Wcss.SubscriptionUserCollection colSubscriptionUserRecords;
		public Wcss.SubscriptionUserCollection SubscriptionUserRecords()
		{
			if(colSubscriptionUserRecords == null)
			{
				colSubscriptionUserRecords = new Wcss.SubscriptionUserCollection().Where(SubscriptionUser.Columns.UserId, UserId).Load();
				colSubscriptionUserRecords.ListChanged += new ListChangedEventHandler(colSubscriptionUserRecords_ListChanged);
			}
			return colSubscriptionUserRecords;
		}
				
		void colSubscriptionUserRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colSubscriptionUserRecords[e.NewIndex].UserId = UserId;
				colSubscriptionUserRecords.ListChanged += new ListChangedEventHandler(colSubscriptionUserRecords_ListChanged);
            }
		}
		private Wcss.FraudScreenCollection colFraudScreenRecords;
		public Wcss.FraudScreenCollection FraudScreenRecords()
		{
			if(colFraudScreenRecords == null)
			{
				colFraudScreenRecords = new Wcss.FraudScreenCollection().Where(FraudScreen.Columns.UserId, UserId).Load();
				colFraudScreenRecords.ListChanged += new ListChangedEventHandler(colFraudScreenRecords_ListChanged);
			}
			return colFraudScreenRecords;
		}
				
		void colFraudScreenRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colFraudScreenRecords[e.NewIndex].UserId = UserId;
				colFraudScreenRecords.ListChanged += new ListChangedEventHandler(colFraudScreenRecords_ListChanged);
            }
		}
		private Wcss.FraudScreenCollection colFraudScreenRecordsFromAspnetUser;
		public Wcss.FraudScreenCollection FraudScreenRecordsFromAspnetUser()
		{
			if(colFraudScreenRecordsFromAspnetUser == null)
			{
				colFraudScreenRecordsFromAspnetUser = new Wcss.FraudScreenCollection().Where(FraudScreen.Columns.CreatedById, UserId).Load();
				colFraudScreenRecordsFromAspnetUser.ListChanged += new ListChangedEventHandler(colFraudScreenRecordsFromAspnetUser_ListChanged);
			}
			return colFraudScreenRecordsFromAspnetUser;
		}
				
		void colFraudScreenRecordsFromAspnetUser_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colFraudScreenRecordsFromAspnetUser[e.NewIndex].CreatedById = UserId;
				colFraudScreenRecordsFromAspnetUser.ListChanged += new ListChangedEventHandler(colFraudScreenRecordsFromAspnetUser_ListChanged);
            }
		}
		private Wcss.HistoryInventoryCollection colHistoryInventoryRecords;
		public Wcss.HistoryInventoryCollection HistoryInventoryRecords()
		{
			if(colHistoryInventoryRecords == null)
			{
				colHistoryInventoryRecords = new Wcss.HistoryInventoryCollection().Where(HistoryInventory.Columns.UserId, UserId).Load();
				colHistoryInventoryRecords.ListChanged += new ListChangedEventHandler(colHistoryInventoryRecords_ListChanged);
			}
			return colHistoryInventoryRecords;
		}
				
		void colHistoryInventoryRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colHistoryInventoryRecords[e.NewIndex].UserId = UserId;
				colHistoryInventoryRecords.ListChanged += new ListChangedEventHandler(colHistoryInventoryRecords_ListChanged);
            }
		}
		private Wcss.HistoryPricingCollection colHistoryPricingRecords;
		public Wcss.HistoryPricingCollection HistoryPricingRecords()
		{
			if(colHistoryPricingRecords == null)
			{
				colHistoryPricingRecords = new Wcss.HistoryPricingCollection().Where(HistoryPricing.Columns.UserId, UserId).Load();
				colHistoryPricingRecords.ListChanged += new ListChangedEventHandler(colHistoryPricingRecords_ListChanged);
			}
			return colHistoryPricingRecords;
		}
				
		void colHistoryPricingRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colHistoryPricingRecords[e.NewIndex].UserId = UserId;
				colHistoryPricingRecords.ListChanged += new ListChangedEventHandler(colHistoryPricingRecords_ListChanged);
            }
		}
		private Wcss.InventoryCollection colInventoryRecords;
		public Wcss.InventoryCollection InventoryRecords()
		{
			if(colInventoryRecords == null)
			{
				colInventoryRecords = new Wcss.InventoryCollection().Where(Inventory.Columns.UserId, UserId).Load();
				colInventoryRecords.ListChanged += new ListChangedEventHandler(colInventoryRecords_ListChanged);
			}
			return colInventoryRecords;
		}
				
		void colInventoryRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colInventoryRecords[e.NewIndex].UserId = UserId;
				colInventoryRecords.ListChanged += new ListChangedEventHandler(colInventoryRecords_ListChanged);
            }
		}
		private Wcss.InvoiceCollection colInvoiceRecords;
		public Wcss.InvoiceCollection InvoiceRecords()
		{
			if(colInvoiceRecords == null)
			{
				colInvoiceRecords = new Wcss.InvoiceCollection().Where(Invoice.Columns.UserId, UserId).Load();
				colInvoiceRecords.ListChanged += new ListChangedEventHandler(colInvoiceRecords_ListChanged);
			}
			return colInvoiceRecords;
		}
				
		void colInvoiceRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colInvoiceRecords[e.NewIndex].UserId = UserId;
				colInvoiceRecords.ListChanged += new ListChangedEventHandler(colInvoiceRecords_ListChanged);
            }
		}
		private Wcss.InvoiceBillShipCollection colInvoiceBillShipRecords;
		public Wcss.InvoiceBillShipCollection InvoiceBillShipRecords()
		{
			if(colInvoiceBillShipRecords == null)
			{
				colInvoiceBillShipRecords = new Wcss.InvoiceBillShipCollection().Where(InvoiceBillShip.Columns.UserId, UserId).Load();
				colInvoiceBillShipRecords.ListChanged += new ListChangedEventHandler(colInvoiceBillShipRecords_ListChanged);
			}
			return colInvoiceBillShipRecords;
		}
				
		void colInvoiceBillShipRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colInvoiceBillShipRecords[e.NewIndex].UserId = UserId;
				colInvoiceBillShipRecords.ListChanged += new ListChangedEventHandler(colInvoiceBillShipRecords_ListChanged);
            }
		}
		private Wcss.InvoiceTransactionCollection colInvoiceTransactionRecords;
		public Wcss.InvoiceTransactionCollection InvoiceTransactionRecords()
		{
			if(colInvoiceTransactionRecords == null)
			{
				colInvoiceTransactionRecords = new Wcss.InvoiceTransactionCollection().Where(InvoiceTransaction.Columns.UserId, UserId).Load();
				colInvoiceTransactionRecords.ListChanged += new ListChangedEventHandler(colInvoiceTransactionRecords_ListChanged);
			}
			return colInvoiceTransactionRecords;
		}
				
		void colInvoiceTransactionRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colInvoiceTransactionRecords[e.NewIndex].UserId = UserId;
				colInvoiceTransactionRecords.ListChanged += new ListChangedEventHandler(colInvoiceTransactionRecords_ListChanged);
            }
		}
		private Wcss.LotteryRequestCollection colLotteryRequestRecords;
		public Wcss.LotteryRequestCollection LotteryRequestRecords()
		{
			if(colLotteryRequestRecords == null)
			{
				colLotteryRequestRecords = new Wcss.LotteryRequestCollection().Where(LotteryRequest.Columns.UserId, UserId).Load();
				colLotteryRequestRecords.ListChanged += new ListChangedEventHandler(colLotteryRequestRecords_ListChanged);
			}
			return colLotteryRequestRecords;
		}
				
		void colLotteryRequestRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colLotteryRequestRecords[e.NewIndex].UserId = UserId;
				colLotteryRequestRecords.ListChanged += new ListChangedEventHandler(colLotteryRequestRecords_ListChanged);
            }
		}
		private Wcss.ProductAccessUserCollection colProductAccessUserRecords;
		public Wcss.ProductAccessUserCollection ProductAccessUserRecords()
		{
			if(colProductAccessUserRecords == null)
			{
				colProductAccessUserRecords = new Wcss.ProductAccessUserCollection().Where(ProductAccessUser.Columns.UserId, UserId).Load();
				colProductAccessUserRecords.ListChanged += new ListChangedEventHandler(colProductAccessUserRecords_ListChanged);
			}
			return colProductAccessUserRecords;
		}
				
		void colProductAccessUserRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colProductAccessUserRecords[e.NewIndex].UserId = UserId;
				colProductAccessUserRecords.ListChanged += new ListChangedEventHandler(colProductAccessUserRecords_ListChanged);
            }
		}
		private Wcss.StoreCreditCollection colStoreCreditRecords;
		public Wcss.StoreCreditCollection StoreCreditRecords()
		{
			if(colStoreCreditRecords == null)
			{
				colStoreCreditRecords = new Wcss.StoreCreditCollection().Where(StoreCredit.Columns.UserId, UserId).Load();
				colStoreCreditRecords.ListChanged += new ListChangedEventHandler(colStoreCreditRecords_ListChanged);
			}
			return colStoreCreditRecords;
		}
				
		void colStoreCreditRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colStoreCreditRecords[e.NewIndex].UserId = UserId;
				colStoreCreditRecords.ListChanged += new ListChangedEventHandler(colStoreCreditRecords_ListChanged);
            }
		}
		private Wcss.UserCouponRedemptionCollection colUserCouponRedemptionRecords;
		public Wcss.UserCouponRedemptionCollection UserCouponRedemptionRecords()
		{
			if(colUserCouponRedemptionRecords == null)
			{
				colUserCouponRedemptionRecords = new Wcss.UserCouponRedemptionCollection().Where(UserCouponRedemption.Columns.UserId, UserId).Load();
				colUserCouponRedemptionRecords.ListChanged += new ListChangedEventHandler(colUserCouponRedemptionRecords_ListChanged);
			}
			return colUserCouponRedemptionRecords;
		}
				
		void colUserCouponRedemptionRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colUserCouponRedemptionRecords[e.NewIndex].UserId = UserId;
				colUserCouponRedemptionRecords.ListChanged += new ListChangedEventHandler(colUserCouponRedemptionRecords_ListChanged);
            }
		}
		#endregion
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a AspnetApplication ActiveRecord object related to this AspnetUser
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
		
		
		
		#region Many To Many Helpers
		
		 
		public Wcss.AspnetRoleCollection GetAspnetRoleCollection() { return AspnetUser.GetAspnetRoleCollection(this.UserId); }
		public static Wcss.AspnetRoleCollection GetAspnetRoleCollection(Guid varUserId)
		{
		    SubSonic.QueryCommand cmd = new SubSonic.QueryCommand("SELECT * FROM [dbo].[aspnet_Roles] INNER JOIN [aspnet_UsersInRoles] ON [aspnet_Roles].[RoleId] = [aspnet_UsersInRoles].[RoleId] WHERE [aspnet_UsersInRoles].[UserId] = @UserId", AspnetUser.Schema.Provider.Name);
			cmd.AddParameter("@UserId", varUserId, DbType.Guid);
			IDataReader rdr = SubSonic.DataService.GetReader(cmd);
			AspnetRoleCollection coll = new AspnetRoleCollection();
			coll.LoadAndCloseReader(rdr);
			return coll;
		}
		
		public static void SaveAspnetRoleMap(Guid varUserId, AspnetRoleCollection items)
		{
			QueryCommandCollection coll = new SubSonic.QueryCommandCollection();
			//delete out the existing
			QueryCommand cmdDel = new QueryCommand("DELETE FROM [aspnet_UsersInRoles] WHERE [aspnet_UsersInRoles].[UserId] = @UserId", AspnetUser.Schema.Provider.Name);
			cmdDel.AddParameter("@UserId", varUserId, DbType.Guid);
			coll.Add(cmdDel);
			DataService.ExecuteTransaction(coll);
			foreach (AspnetRole item in items)
			{
				AspnetUsersInRole varAspnetUsersInRole = new AspnetUsersInRole();
				varAspnetUsersInRole.SetColumnValue("UserId", varUserId);
				varAspnetUsersInRole.SetColumnValue("RoleId", item.GetPrimaryKeyValue());
				varAspnetUsersInRole.Save();
			}
		}
		public static void SaveAspnetRoleMap(Guid varUserId, System.Web.UI.WebControls.ListItemCollection itemList) 
		{
			QueryCommandCollection coll = new SubSonic.QueryCommandCollection();
			//delete out the existing
			 QueryCommand cmdDel = new QueryCommand("DELETE FROM [aspnet_UsersInRoles] WHERE [aspnet_UsersInRoles].[UserId] = @UserId", AspnetUser.Schema.Provider.Name);
			cmdDel.AddParameter("@UserId", varUserId, DbType.Guid);
			coll.Add(cmdDel);
			DataService.ExecuteTransaction(coll);
			foreach (System.Web.UI.WebControls.ListItem l in itemList) 
			{
				if (l.Selected) 
				{
					AspnetUsersInRole varAspnetUsersInRole = new AspnetUsersInRole();
					varAspnetUsersInRole.SetColumnValue("UserId", varUserId);
					varAspnetUsersInRole.SetColumnValue("RoleId", l.Value);
					varAspnetUsersInRole.Save();
				}
			}
		}
		public static void SaveAspnetRoleMap(Guid varUserId , Guid[] itemList) 
		{
			QueryCommandCollection coll = new SubSonic.QueryCommandCollection();
			//delete out the existing
			 QueryCommand cmdDel = new QueryCommand("DELETE FROM [aspnet_UsersInRoles] WHERE [aspnet_UsersInRoles].[UserId] = @UserId", AspnetUser.Schema.Provider.Name);
			cmdDel.AddParameter("@UserId", varUserId, DbType.Guid);
			coll.Add(cmdDel);
			DataService.ExecuteTransaction(coll);
			foreach (Guid item in itemList) 
			{
				AspnetUsersInRole varAspnetUsersInRole = new AspnetUsersInRole();
				varAspnetUsersInRole.SetColumnValue("UserId", varUserId);
				varAspnetUsersInRole.SetColumnValue("RoleId", item);
				varAspnetUsersInRole.Save();
			}
		}
		
		public static void DeleteAspnetRoleMap(Guid varUserId) 
		{
			QueryCommand cmdDel = new QueryCommand("DELETE FROM [aspnet_UsersInRoles] WHERE [aspnet_UsersInRoles].[UserId] = @UserId", AspnetUser.Schema.Provider.Name);
			cmdDel.AddParameter("@UserId", varUserId, DbType.Guid);
			DataService.ExecuteQuery(cmdDel);
		}
		
		#endregion
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(Guid varApplicationId,Guid varUserId,string varUserName,string varLoweredUserName,string varMobileAlias,bool varIsAnonymous,DateTime varLastActivityDate)
		{
			AspnetUser item = new AspnetUser();
			
			item.ApplicationId = varApplicationId;
			
			item.UserId = varUserId;
			
			item.UserName = varUserName;
			
			item.LoweredUserName = varLoweredUserName;
			
			item.MobileAlias = varMobileAlias;
			
			item.IsAnonymous = varIsAnonymous;
			
			item.LastActivityDate = varLastActivityDate;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(Guid varApplicationId,Guid varUserId,string varUserName,string varLoweredUserName,string varMobileAlias,bool varIsAnonymous,DateTime varLastActivityDate,int varCustomerId)
		{
			AspnetUser item = new AspnetUser();
			
				item.ApplicationId = varApplicationId;
			
				item.UserId = varUserId;
			
				item.UserName = varUserName;
			
				item.LoweredUserName = varLoweredUserName;
			
				item.MobileAlias = varMobileAlias;
			
				item.IsAnonymous = varIsAnonymous;
			
				item.LastActivityDate = varLastActivityDate;
			
				item.CustomerId = varCustomerId;
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn ApplicationIdColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn UserIdColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn UserNameColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn LoweredUserNameColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn MobileAliasColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn IsAnonymousColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn LastActivityDateColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn CustomerIdColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string ApplicationId = @"ApplicationId";
			 public static string UserId = @"UserId";
			 public static string UserName = @"UserName";
			 public static string LoweredUserName = @"LoweredUserName";
			 public static string MobileAlias = @"MobileAlias";
			 public static string IsAnonymous = @"IsAnonymous";
			 public static string LastActivityDate = @"LastActivityDate";
			 public static string CustomerId = @"CustomerId";
						
		}
		#endregion
		
		#region Update PK Collections
		
        public void SetPKValues()
        {
                if (colAspnetMembershipRecords != null)
                {
                    foreach (Wcss.AspnetMembership item in colAspnetMembershipRecords)
                    {
                        if (item.UserId != UserId)
                        {
                            item.UserId = UserId;
                        }
                    }
               }
		
                if (colAspnetPersonalizationPerUserRecords != null)
                {
                    foreach (Wcss.AspnetPersonalizationPerUser item in colAspnetPersonalizationPerUserRecords)
                    {
                        if (item.UserId != UserId)
                        {
                            item.UserId = UserId;
                        }
                    }
               }
		
                if (colAspnetProfileRecords != null)
                {
                    foreach (Wcss.AspnetProfile item in colAspnetProfileRecords)
                    {
                        if (item.UserId != UserId)
                        {
                            item.UserId = UserId;
                        }
                    }
               }
		
                if (colAspnetUsersInRoles != null)
                {
                    foreach (Wcss.AspnetUsersInRole item in colAspnetUsersInRoles)
                    {
                        if (item.UserId != UserId)
                        {
                            item.UserId = UserId;
                        }
                    }
               }
		
                if (colAuthorizeNetRecords != null)
                {
                    foreach (Wcss.AuthorizeNet item in colAuthorizeNetRecords)
                    {
                        if (item.UserId != UserId)
                        {
                            item.UserId = UserId;
                        }
                    }
               }
		
                if (colCashewRecords != null)
                {
                    foreach (Wcss.Cashew item in colCashewRecords)
                    {
                        if (item.UserId != UserId)
                        {
                            item.UserId = UserId;
                        }
                    }
               }
		
                if (colSubscriptionUserRecords != null)
                {
                    foreach (Wcss.SubscriptionUser item in colSubscriptionUserRecords)
                    {
                        if (item.UserId != UserId)
                        {
                            item.UserId = UserId;
                        }
                    }
               }
		
                if (colFraudScreenRecords != null)
                {
                    foreach (Wcss.FraudScreen item in colFraudScreenRecords)
                    {
                        if (item.UserId != UserId)
                        {
                            item.UserId = UserId;
                        }
                    }
               }
		
                if (colFraudScreenRecordsFromAspnetUser != null)
                {
                    foreach (Wcss.FraudScreen item in colFraudScreenRecordsFromAspnetUser)
                    {
                        if (item.CreatedById != UserId)
                        {
                            item.CreatedById = UserId;
                        }
                    }
               }
		
                if (colHistoryInventoryRecords != null)
                {
                    foreach (Wcss.HistoryInventory item in colHistoryInventoryRecords)
                    {
                        if (item.UserId != UserId)
                        {
                            item.UserId = UserId;
                        }
                    }
               }
		
                if (colHistoryPricingRecords != null)
                {
                    foreach (Wcss.HistoryPricing item in colHistoryPricingRecords)
                    {
                        if (item.UserId != UserId)
                        {
                            item.UserId = UserId;
                        }
                    }
               }
		
                if (colInventoryRecords != null)
                {
                    foreach (Wcss.Inventory item in colInventoryRecords)
                    {
                        if (item.UserId != UserId)
                        {
                            item.UserId = UserId;
                        }
                    }
               }
		
                if (colInvoiceRecords != null)
                {
                    foreach (Wcss.Invoice item in colInvoiceRecords)
                    {
                        if (item.UserId != UserId)
                        {
                            item.UserId = UserId;
                        }
                    }
               }
		
                if (colInvoiceBillShipRecords != null)
                {
                    foreach (Wcss.InvoiceBillShip item in colInvoiceBillShipRecords)
                    {
                        if (item.UserId != UserId)
                        {
                            item.UserId = UserId;
                        }
                    }
               }
		
                if (colInvoiceTransactionRecords != null)
                {
                    foreach (Wcss.InvoiceTransaction item in colInvoiceTransactionRecords)
                    {
                        if (item.UserId != UserId)
                        {
                            item.UserId = UserId;
                        }
                    }
               }
		
                if (colLotteryRequestRecords != null)
                {
                    foreach (Wcss.LotteryRequest item in colLotteryRequestRecords)
                    {
                        if (item.UserId != UserId)
                        {
                            item.UserId = UserId;
                        }
                    }
               }
		
                if (colProductAccessUserRecords != null)
                {
                    foreach (Wcss.ProductAccessUser item in colProductAccessUserRecords)
                    {
                        if (item.UserId != UserId)
                        {
                            item.UserId = UserId;
                        }
                    }
               }
		
                if (colStoreCreditRecords != null)
                {
                    foreach (Wcss.StoreCredit item in colStoreCreditRecords)
                    {
                        if (item.UserId != UserId)
                        {
                            item.UserId = UserId;
                        }
                    }
               }
		
                if (colUserCouponRedemptionRecords != null)
                {
                    foreach (Wcss.UserCouponRedemption item in colUserCouponRedemptionRecords)
                    {
                        if (item.UserId != UserId)
                        {
                            item.UserId = UserId;
                        }
                    }
               }
		}
        #endregion
    
        #region Deep Save
		
        public void DeepSave()
        {
            Save();
            
                if (colAspnetMembershipRecords != null)
                {
                    colAspnetMembershipRecords.SaveAll();
               }
		
                if (colAspnetPersonalizationPerUserRecords != null)
                {
                    colAspnetPersonalizationPerUserRecords.SaveAll();
               }
		
                if (colAspnetProfileRecords != null)
                {
                    colAspnetProfileRecords.SaveAll();
               }
		
                if (colAspnetUsersInRoles != null)
                {
                    colAspnetUsersInRoles.SaveAll();
               }
		
                if (colAuthorizeNetRecords != null)
                {
                    colAuthorizeNetRecords.SaveAll();
               }
		
                if (colCashewRecords != null)
                {
                    colCashewRecords.SaveAll();
               }
		
                if (colSubscriptionUserRecords != null)
                {
                    colSubscriptionUserRecords.SaveAll();
               }
		
                if (colFraudScreenRecords != null)
                {
                    colFraudScreenRecords.SaveAll();
               }
		
                if (colFraudScreenRecordsFromAspnetUser != null)
                {
                    colFraudScreenRecordsFromAspnetUser.SaveAll();
               }
		
                if (colHistoryInventoryRecords != null)
                {
                    colHistoryInventoryRecords.SaveAll();
               }
		
                if (colHistoryPricingRecords != null)
                {
                    colHistoryPricingRecords.SaveAll();
               }
		
                if (colInventoryRecords != null)
                {
                    colInventoryRecords.SaveAll();
               }
		
                if (colInvoiceRecords != null)
                {
                    colInvoiceRecords.SaveAll();
               }
		
                if (colInvoiceBillShipRecords != null)
                {
                    colInvoiceBillShipRecords.SaveAll();
               }
		
                if (colInvoiceTransactionRecords != null)
                {
                    colInvoiceTransactionRecords.SaveAll();
               }
		
                if (colLotteryRequestRecords != null)
                {
                    colLotteryRequestRecords.SaveAll();
               }
		
                if (colProductAccessUserRecords != null)
                {
                    colProductAccessUserRecords.SaveAll();
               }
		
                if (colStoreCreditRecords != null)
                {
                    colStoreCreditRecords.SaveAll();
               }
		
                if (colUserCouponRedemptionRecords != null)
                {
                    colUserCouponRedemptionRecords.SaveAll();
               }
		}
        #endregion
	}
}
