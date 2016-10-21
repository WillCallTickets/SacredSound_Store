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
	/// Strongly-typed collection for the AspnetApplication class.
	/// </summary>
    [Serializable]
	public partial class AspnetApplicationCollection : ActiveList<AspnetApplication, AspnetApplicationCollection>
	{	   
		public AspnetApplicationCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>AspnetApplicationCollection</returns>
		public AspnetApplicationCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                AspnetApplication o = this[i];
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
	/// This is an ActiveRecord class which wraps the aspnet_Applications table.
	/// </summary>
	[Serializable]
	public partial class AspnetApplication : ActiveRecord<AspnetApplication>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public AspnetApplication()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public AspnetApplication(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public AspnetApplication(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public AspnetApplication(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("aspnet_Applications", TableType.Table, DataService.GetInstance("WillCall"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarApplicationName = new TableSchema.TableColumn(schema);
				colvarApplicationName.ColumnName = "ApplicationName";
				colvarApplicationName.DataType = DbType.String;
				colvarApplicationName.MaxLength = 256;
				colvarApplicationName.AutoIncrement = false;
				colvarApplicationName.IsNullable = false;
				colvarApplicationName.IsPrimaryKey = false;
				colvarApplicationName.IsForeignKey = false;
				colvarApplicationName.IsReadOnly = false;
				colvarApplicationName.DefaultSetting = @"";
				colvarApplicationName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarApplicationName);
				
				TableSchema.TableColumn colvarLoweredApplicationName = new TableSchema.TableColumn(schema);
				colvarLoweredApplicationName.ColumnName = "LoweredApplicationName";
				colvarLoweredApplicationName.DataType = DbType.String;
				colvarLoweredApplicationName.MaxLength = 256;
				colvarLoweredApplicationName.AutoIncrement = false;
				colvarLoweredApplicationName.IsNullable = false;
				colvarLoweredApplicationName.IsPrimaryKey = false;
				colvarLoweredApplicationName.IsForeignKey = false;
				colvarLoweredApplicationName.IsReadOnly = false;
				colvarLoweredApplicationName.DefaultSetting = @"";
				colvarLoweredApplicationName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarLoweredApplicationName);
				
				TableSchema.TableColumn colvarApplicationId = new TableSchema.TableColumn(schema);
				colvarApplicationId.ColumnName = "ApplicationId";
				colvarApplicationId.DataType = DbType.Guid;
				colvarApplicationId.MaxLength = 0;
				colvarApplicationId.AutoIncrement = false;
				colvarApplicationId.IsNullable = false;
				colvarApplicationId.IsPrimaryKey = true;
				colvarApplicationId.IsForeignKey = false;
				colvarApplicationId.IsReadOnly = false;
				
						colvarApplicationId.DefaultSetting = @"(newid())";
				colvarApplicationId.ForeignKeyTableName = "";
				schema.Columns.Add(colvarApplicationId);
				
				TableSchema.TableColumn colvarDescription = new TableSchema.TableColumn(schema);
				colvarDescription.ColumnName = "Description";
				colvarDescription.DataType = DbType.String;
				colvarDescription.MaxLength = 256;
				colvarDescription.AutoIncrement = false;
				colvarDescription.IsNullable = true;
				colvarDescription.IsPrimaryKey = false;
				colvarDescription.IsForeignKey = false;
				colvarDescription.IsReadOnly = false;
				colvarDescription.DefaultSetting = @"";
				colvarDescription.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDescription);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["WillCall"].AddSchema("aspnet_Applications",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("ApplicationName")]
		[Bindable(true)]
		public string ApplicationName 
		{
			get { return GetColumnValue<string>(Columns.ApplicationName); }
			set { SetColumnValue(Columns.ApplicationName, value); }
		}
		  
		[XmlAttribute("LoweredApplicationName")]
		[Bindable(true)]
		public string LoweredApplicationName 
		{
			get { return GetColumnValue<string>(Columns.LoweredApplicationName); }
			set { SetColumnValue(Columns.LoweredApplicationName, value); }
		}
		  
		[XmlAttribute("ApplicationId")]
		[Bindable(true)]
		public Guid ApplicationId 
		{
			get { return GetColumnValue<Guid>(Columns.ApplicationId); }
			set { SetColumnValue(Columns.ApplicationId, value); }
		}
		  
		[XmlAttribute("Description")]
		[Bindable(true)]
		public string Description 
		{
			get { return GetColumnValue<string>(Columns.Description); }
			set { SetColumnValue(Columns.Description, value); }
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
				colAspnetMembershipRecords = new Wcss.AspnetMembershipCollection().Where(AspnetMembership.Columns.ApplicationId, ApplicationId).Load();
				colAspnetMembershipRecords.ListChanged += new ListChangedEventHandler(colAspnetMembershipRecords_ListChanged);
			}
			return colAspnetMembershipRecords;
		}
				
		void colAspnetMembershipRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colAspnetMembershipRecords[e.NewIndex].ApplicationId = ApplicationId;
				colAspnetMembershipRecords.ListChanged += new ListChangedEventHandler(colAspnetMembershipRecords_ListChanged);
            }
		}
		private Wcss.AspnetPathCollection colAspnetPaths;
		public Wcss.AspnetPathCollection AspnetPaths()
		{
			if(colAspnetPaths == null)
			{
				colAspnetPaths = new Wcss.AspnetPathCollection().Where(AspnetPath.Columns.ApplicationId, ApplicationId).Load();
				colAspnetPaths.ListChanged += new ListChangedEventHandler(colAspnetPaths_ListChanged);
			}
			return colAspnetPaths;
		}
				
		void colAspnetPaths_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colAspnetPaths[e.NewIndex].ApplicationId = ApplicationId;
				colAspnetPaths.ListChanged += new ListChangedEventHandler(colAspnetPaths_ListChanged);
            }
		}
		private Wcss.AspnetRoleCollection colAspnetRoles;
		public Wcss.AspnetRoleCollection AspnetRoles()
		{
			if(colAspnetRoles == null)
			{
				colAspnetRoles = new Wcss.AspnetRoleCollection().Where(AspnetRole.Columns.ApplicationId, ApplicationId).Load();
				colAspnetRoles.ListChanged += new ListChangedEventHandler(colAspnetRoles_ListChanged);
			}
			return colAspnetRoles;
		}
				
		void colAspnetRoles_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colAspnetRoles[e.NewIndex].ApplicationId = ApplicationId;
				colAspnetRoles.ListChanged += new ListChangedEventHandler(colAspnetRoles_ListChanged);
            }
		}
		private Wcss.AspnetUserCollection colAspnetUsers;
		public Wcss.AspnetUserCollection AspnetUsers()
		{
			if(colAspnetUsers == null)
			{
				colAspnetUsers = new Wcss.AspnetUserCollection().Where(AspnetUser.Columns.ApplicationId, ApplicationId).Load();
				colAspnetUsers.ListChanged += new ListChangedEventHandler(colAspnetUsers_ListChanged);
			}
			return colAspnetUsers;
		}
				
		void colAspnetUsers_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colAspnetUsers[e.NewIndex].ApplicationId = ApplicationId;
				colAspnetUsers.ListChanged += new ListChangedEventHandler(colAspnetUsers_ListChanged);
            }
		}
		private Wcss.ActCollection colActRecords;
		public Wcss.ActCollection ActRecords()
		{
			if(colActRecords == null)
			{
				colActRecords = new Wcss.ActCollection().Where(Act.Columns.ApplicationId, ApplicationId).Load();
				colActRecords.ListChanged += new ListChangedEventHandler(colActRecords_ListChanged);
			}
			return colActRecords;
		}
				
		void colActRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colActRecords[e.NewIndex].ApplicationId = ApplicationId;
				colActRecords.ListChanged += new ListChangedEventHandler(colActRecords_ListChanged);
            }
		}
		private Wcss.ActivationWindowCollection colActivationWindowRecords;
		public Wcss.ActivationWindowCollection ActivationWindowRecords()
		{
			if(colActivationWindowRecords == null)
			{
				colActivationWindowRecords = new Wcss.ActivationWindowCollection().Where(ActivationWindow.Columns.ApplicationId, ApplicationId).Load();
				colActivationWindowRecords.ListChanged += new ListChangedEventHandler(colActivationWindowRecords_ListChanged);
			}
			return colActivationWindowRecords;
		}
				
		void colActivationWindowRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colActivationWindowRecords[e.NewIndex].ApplicationId = ApplicationId;
				colActivationWindowRecords.ListChanged += new ListChangedEventHandler(colActivationWindowRecords_ListChanged);
            }
		}
		private Wcss.AgeCollection colAgeRecords;
		public Wcss.AgeCollection AgeRecords()
		{
			if(colAgeRecords == null)
			{
				colAgeRecords = new Wcss.AgeCollection().Where(Age.Columns.ApplicationId, ApplicationId).Load();
				colAgeRecords.ListChanged += new ListChangedEventHandler(colAgeRecords_ListChanged);
			}
			return colAgeRecords;
		}
				
		void colAgeRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colAgeRecords[e.NewIndex].ApplicationId = ApplicationId;
				colAgeRecords.ListChanged += new ListChangedEventHandler(colAgeRecords_ListChanged);
            }
		}
		private Wcss.AuthorizeNetCollection colAuthorizeNetRecords;
		public Wcss.AuthorizeNetCollection AuthorizeNetRecords()
		{
			if(colAuthorizeNetRecords == null)
			{
				colAuthorizeNetRecords = new Wcss.AuthorizeNetCollection().Where(AuthorizeNet.Columns.ApplicationId, ApplicationId).Load();
				colAuthorizeNetRecords.ListChanged += new ListChangedEventHandler(colAuthorizeNetRecords_ListChanged);
			}
			return colAuthorizeNetRecords;
		}
				
		void colAuthorizeNetRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colAuthorizeNetRecords[e.NewIndex].ApplicationId = ApplicationId;
				colAuthorizeNetRecords.ListChanged += new ListChangedEventHandler(colAuthorizeNetRecords_ListChanged);
            }
		}
		private Wcss.ChargeStatementCollection colChargeStatementRecords;
		public Wcss.ChargeStatementCollection ChargeStatementRecords()
		{
			if(colChargeStatementRecords == null)
			{
				colChargeStatementRecords = new Wcss.ChargeStatementCollection().Where(ChargeStatement.Columns.ApplicationId, ApplicationId).Load();
				colChargeStatementRecords.ListChanged += new ListChangedEventHandler(colChargeStatementRecords_ListChanged);
			}
			return colChargeStatementRecords;
		}
				
		void colChargeStatementRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colChargeStatementRecords[e.NewIndex].ApplicationId = ApplicationId;
				colChargeStatementRecords.ListChanged += new ListChangedEventHandler(colChargeStatementRecords_ListChanged);
            }
		}
		private Wcss.CharitableListingCollection colCharitableListingRecords;
		public Wcss.CharitableListingCollection CharitableListingRecords()
		{
			if(colCharitableListingRecords == null)
			{
				colCharitableListingRecords = new Wcss.CharitableListingCollection().Where(CharitableListing.Columns.ApplicationId, ApplicationId).Load();
				colCharitableListingRecords.ListChanged += new ListChangedEventHandler(colCharitableListingRecords_ListChanged);
			}
			return colCharitableListingRecords;
		}
				
		void colCharitableListingRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colCharitableListingRecords[e.NewIndex].ApplicationId = ApplicationId;
				colCharitableListingRecords.ListChanged += new ListChangedEventHandler(colCharitableListingRecords_ListChanged);
            }
		}
		private Wcss.CharitableOrgCollection colCharitableOrgRecords;
		public Wcss.CharitableOrgCollection CharitableOrgRecords()
		{
			if(colCharitableOrgRecords == null)
			{
				colCharitableOrgRecords = new Wcss.CharitableOrgCollection().Where(CharitableOrg.Columns.ApplicationId, ApplicationId).Load();
				colCharitableOrgRecords.ListChanged += new ListChangedEventHandler(colCharitableOrgRecords_ListChanged);
			}
			return colCharitableOrgRecords;
		}
				
		void colCharitableOrgRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colCharitableOrgRecords[e.NewIndex].ApplicationId = ApplicationId;
				colCharitableOrgRecords.ListChanged += new ListChangedEventHandler(colCharitableOrgRecords_ListChanged);
            }
		}
		private Wcss.DownloadCollection colDownloadRecords;
		public Wcss.DownloadCollection DownloadRecords()
		{
			if(colDownloadRecords == null)
			{
				colDownloadRecords = new Wcss.DownloadCollection().Where(Download.Columns.ApplicationId, ApplicationId).Load();
				colDownloadRecords.ListChanged += new ListChangedEventHandler(colDownloadRecords_ListChanged);
			}
			return colDownloadRecords;
		}
				
		void colDownloadRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colDownloadRecords[e.NewIndex].ApplicationId = ApplicationId;
				colDownloadRecords.ListChanged += new ListChangedEventHandler(colDownloadRecords_ListChanged);
            }
		}
		private Wcss.EmailLetterCollection colEmailLetterRecords;
		public Wcss.EmailLetterCollection EmailLetterRecords()
		{
			if(colEmailLetterRecords == null)
			{
				colEmailLetterRecords = new Wcss.EmailLetterCollection().Where(EmailLetter.Columns.ApplicationId, ApplicationId).Load();
				colEmailLetterRecords.ListChanged += new ListChangedEventHandler(colEmailLetterRecords_ListChanged);
			}
			return colEmailLetterRecords;
		}
				
		void colEmailLetterRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colEmailLetterRecords[e.NewIndex].ApplicationId = ApplicationId;
				colEmailLetterRecords.ListChanged += new ListChangedEventHandler(colEmailLetterRecords_ListChanged);
            }
		}
		private Wcss.EventQCollection colEventQRecords;
		public Wcss.EventQCollection EventQRecords()
		{
			if(colEventQRecords == null)
			{
				colEventQRecords = new Wcss.EventQCollection().Where(EventQ.Columns.ApplicationId, ApplicationId).Load();
				colEventQRecords.ListChanged += new ListChangedEventHandler(colEventQRecords_ListChanged);
			}
			return colEventQRecords;
		}
				
		void colEventQRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colEventQRecords[e.NewIndex].ApplicationId = ApplicationId;
				colEventQRecords.ListChanged += new ListChangedEventHandler(colEventQRecords_ListChanged);
            }
		}
		private Wcss.FaqCategorieCollection colFaqCategorieRecords;
		public Wcss.FaqCategorieCollection FaqCategorieRecords()
		{
			if(colFaqCategorieRecords == null)
			{
				colFaqCategorieRecords = new Wcss.FaqCategorieCollection().Where(FaqCategorie.Columns.ApplicationId, ApplicationId).Load();
				colFaqCategorieRecords.ListChanged += new ListChangedEventHandler(colFaqCategorieRecords_ListChanged);
			}
			return colFaqCategorieRecords;
		}
				
		void colFaqCategorieRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colFaqCategorieRecords[e.NewIndex].ApplicationId = ApplicationId;
				colFaqCategorieRecords.ListChanged += new ListChangedEventHandler(colFaqCategorieRecords_ListChanged);
            }
		}
		private Wcss.FbStatCollection colFbStatRecords;
		public Wcss.FbStatCollection FbStatRecords()
		{
			if(colFbStatRecords == null)
			{
				colFbStatRecords = new Wcss.FbStatCollection().Where(FbStat.Columns.ApplicationId, ApplicationId).Load();
				colFbStatRecords.ListChanged += new ListChangedEventHandler(colFbStatRecords_ListChanged);
			}
			return colFbStatRecords;
		}
				
		void colFbStatRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colFbStatRecords[e.NewIndex].ApplicationId = ApplicationId;
				colFbStatRecords.ListChanged += new ListChangedEventHandler(colFbStatRecords_ListChanged);
            }
		}
		private Wcss.FraudScreenCollection colFraudScreenRecords;
		public Wcss.FraudScreenCollection FraudScreenRecords()
		{
			if(colFraudScreenRecords == null)
			{
				colFraudScreenRecords = new Wcss.FraudScreenCollection().Where(FraudScreen.Columns.ApplicationId, ApplicationId).Load();
				colFraudScreenRecords.ListChanged += new ListChangedEventHandler(colFraudScreenRecords_ListChanged);
			}
			return colFraudScreenRecords;
		}
				
		void colFraudScreenRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colFraudScreenRecords[e.NewIndex].ApplicationId = ApplicationId;
				colFraudScreenRecords.ListChanged += new ListChangedEventHandler(colFraudScreenRecords_ListChanged);
            }
		}
		private Wcss.HintQuestionCollection colHintQuestionRecords;
		public Wcss.HintQuestionCollection HintQuestionRecords()
		{
			if(colHintQuestionRecords == null)
			{
				colHintQuestionRecords = new Wcss.HintQuestionCollection().Where(HintQuestion.Columns.ApplicationId, ApplicationId).Load();
				colHintQuestionRecords.ListChanged += new ListChangedEventHandler(colHintQuestionRecords_ListChanged);
			}
			return colHintQuestionRecords;
		}
				
		void colHintQuestionRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colHintQuestionRecords[e.NewIndex].ApplicationId = ApplicationId;
				colHintQuestionRecords.ListChanged += new ListChangedEventHandler(colHintQuestionRecords_ListChanged);
            }
		}
		private Wcss.InvoiceCollection colInvoiceRecords;
		public Wcss.InvoiceCollection InvoiceRecords()
		{
			if(colInvoiceRecords == null)
			{
				colInvoiceRecords = new Wcss.InvoiceCollection().Where(Invoice.Columns.ApplicationId, ApplicationId).Load();
				colInvoiceRecords.ListChanged += new ListChangedEventHandler(colInvoiceRecords_ListChanged);
			}
			return colInvoiceRecords;
		}
				
		void colInvoiceRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colInvoiceRecords[e.NewIndex].ApplicationId = ApplicationId;
				colInvoiceRecords.ListChanged += new ListChangedEventHandler(colInvoiceRecords_ListChanged);
            }
		}
		private Wcss.InvoiceFeeCollection colInvoiceFeeRecords;
		public Wcss.InvoiceFeeCollection InvoiceFeeRecords()
		{
			if(colInvoiceFeeRecords == null)
			{
				colInvoiceFeeRecords = new Wcss.InvoiceFeeCollection().Where(InvoiceFee.Columns.ApplicationId, ApplicationId).Load();
				colInvoiceFeeRecords.ListChanged += new ListChangedEventHandler(colInvoiceFeeRecords_ListChanged);
			}
			return colInvoiceFeeRecords;
		}
				
		void colInvoiceFeeRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colInvoiceFeeRecords[e.NewIndex].ApplicationId = ApplicationId;
				colInvoiceFeeRecords.ListChanged += new ListChangedEventHandler(colInvoiceFeeRecords_ListChanged);
            }
		}
		private Wcss.MailQueueCollection colMailQueueRecords;
		public Wcss.MailQueueCollection MailQueueRecords()
		{
			if(colMailQueueRecords == null)
			{
				colMailQueueRecords = new Wcss.MailQueueCollection().Where(MailQueue.Columns.ApplicationId, ApplicationId).Load();
				colMailQueueRecords.ListChanged += new ListChangedEventHandler(colMailQueueRecords_ListChanged);
			}
			return colMailQueueRecords;
		}
				
		void colMailQueueRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colMailQueueRecords[e.NewIndex].ApplicationId = ApplicationId;
				colMailQueueRecords.ListChanged += new ListChangedEventHandler(colMailQueueRecords_ListChanged);
            }
		}
		private Wcss.MerchCollection colMerchRecords;
		public Wcss.MerchCollection MerchRecords()
		{
			if(colMerchRecords == null)
			{
				colMerchRecords = new Wcss.MerchCollection().Where(Merch.Columns.ApplicationId, ApplicationId).Load();
				colMerchRecords.ListChanged += new ListChangedEventHandler(colMerchRecords_ListChanged);
			}
			return colMerchRecords;
		}
				
		void colMerchRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colMerchRecords[e.NewIndex].ApplicationId = ApplicationId;
				colMerchRecords.ListChanged += new ListChangedEventHandler(colMerchRecords_ListChanged);
            }
		}
		private Wcss.MerchColorCollection colMerchColorRecords;
		public Wcss.MerchColorCollection MerchColorRecords()
		{
			if(colMerchColorRecords == null)
			{
				colMerchColorRecords = new Wcss.MerchColorCollection().Where(MerchColor.Columns.ApplicationId, ApplicationId).Load();
				colMerchColorRecords.ListChanged += new ListChangedEventHandler(colMerchColorRecords_ListChanged);
			}
			return colMerchColorRecords;
		}
				
		void colMerchColorRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colMerchColorRecords[e.NewIndex].ApplicationId = ApplicationId;
				colMerchColorRecords.ListChanged += new ListChangedEventHandler(colMerchColorRecords_ListChanged);
            }
		}
		private Wcss.MerchDivisionCollection colMerchDivisionRecords;
		public Wcss.MerchDivisionCollection MerchDivisionRecords()
		{
			if(colMerchDivisionRecords == null)
			{
				colMerchDivisionRecords = new Wcss.MerchDivisionCollection().Where(MerchDivision.Columns.ApplicationId, ApplicationId).Load();
				colMerchDivisionRecords.ListChanged += new ListChangedEventHandler(colMerchDivisionRecords_ListChanged);
			}
			return colMerchDivisionRecords;
		}
				
		void colMerchDivisionRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colMerchDivisionRecords[e.NewIndex].ApplicationId = ApplicationId;
				colMerchDivisionRecords.ListChanged += new ListChangedEventHandler(colMerchDivisionRecords_ListChanged);
            }
		}
		private Wcss.MerchSizeCollection colMerchSizeRecords;
		public Wcss.MerchSizeCollection MerchSizeRecords()
		{
			if(colMerchSizeRecords == null)
			{
				colMerchSizeRecords = new Wcss.MerchSizeCollection().Where(MerchSize.Columns.ApplicationId, ApplicationId).Load();
				colMerchSizeRecords.ListChanged += new ListChangedEventHandler(colMerchSizeRecords_ListChanged);
			}
			return colMerchSizeRecords;
		}
				
		void colMerchSizeRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colMerchSizeRecords[e.NewIndex].ApplicationId = ApplicationId;
				colMerchSizeRecords.ListChanged += new ListChangedEventHandler(colMerchSizeRecords_ListChanged);
            }
		}
		private Wcss.ProductAccessCollection colProductAccessRecords;
		public Wcss.ProductAccessCollection ProductAccessRecords()
		{
			if(colProductAccessRecords == null)
			{
				colProductAccessRecords = new Wcss.ProductAccessCollection().Where(ProductAccess.Columns.ApplicationId, ApplicationId).Load();
				colProductAccessRecords.ListChanged += new ListChangedEventHandler(colProductAccessRecords_ListChanged);
			}
			return colProductAccessRecords;
		}
				
		void colProductAccessRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colProductAccessRecords[e.NewIndex].ApplicationId = ApplicationId;
				colProductAccessRecords.ListChanged += new ListChangedEventHandler(colProductAccessRecords_ListChanged);
            }
		}
		private Wcss.PromoterCollection colPromoterRecords;
		public Wcss.PromoterCollection PromoterRecords()
		{
			if(colPromoterRecords == null)
			{
				colPromoterRecords = new Wcss.PromoterCollection().Where(Promoter.Columns.ApplicationId, ApplicationId).Load();
				colPromoterRecords.ListChanged += new ListChangedEventHandler(colPromoterRecords_ListChanged);
			}
			return colPromoterRecords;
		}
				
		void colPromoterRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colPromoterRecords[e.NewIndex].ApplicationId = ApplicationId;
				colPromoterRecords.ListChanged += new ListChangedEventHandler(colPromoterRecords_ListChanged);
            }
		}
		private Wcss.ReportDailySaleCollection colReportDailySales;
		public Wcss.ReportDailySaleCollection ReportDailySales()
		{
			if(colReportDailySales == null)
			{
				colReportDailySales = new Wcss.ReportDailySaleCollection().Where(ReportDailySale.Columns.ApplicationId, ApplicationId).Load();
				colReportDailySales.ListChanged += new ListChangedEventHandler(colReportDailySales_ListChanged);
			}
			return colReportDailySales;
		}
				
		void colReportDailySales_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colReportDailySales[e.NewIndex].ApplicationId = ApplicationId;
				colReportDailySales.ListChanged += new ListChangedEventHandler(colReportDailySales_ListChanged);
            }
		}
		private Wcss.SalePromotionCollection colSalePromotionRecords;
		public Wcss.SalePromotionCollection SalePromotionRecords()
		{
			if(colSalePromotionRecords == null)
			{
				colSalePromotionRecords = new Wcss.SalePromotionCollection().Where(SalePromotion.Columns.ApplicationId, ApplicationId).Load();
				colSalePromotionRecords.ListChanged += new ListChangedEventHandler(colSalePromotionRecords_ListChanged);
			}
			return colSalePromotionRecords;
		}
				
		void colSalePromotionRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colSalePromotionRecords[e.NewIndex].ApplicationId = ApplicationId;
				colSalePromotionRecords.ListChanged += new ListChangedEventHandler(colSalePromotionRecords_ListChanged);
            }
		}
		private Wcss.SaleRuleCollection colSaleRuleRecords;
		public Wcss.SaleRuleCollection SaleRuleRecords()
		{
			if(colSaleRuleRecords == null)
			{
				colSaleRuleRecords = new Wcss.SaleRuleCollection().Where(SaleRule.Columns.ApplicationId, ApplicationId).Load();
				colSaleRuleRecords.ListChanged += new ListChangedEventHandler(colSaleRuleRecords_ListChanged);
			}
			return colSaleRuleRecords;
		}
				
		void colSaleRuleRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colSaleRuleRecords[e.NewIndex].ApplicationId = ApplicationId;
				colSaleRuleRecords.ListChanged += new ListChangedEventHandler(colSaleRuleRecords_ListChanged);
            }
		}
		private Wcss.SearchCollection colSearchRecords;
		public Wcss.SearchCollection SearchRecords()
		{
			if(colSearchRecords == null)
			{
				colSearchRecords = new Wcss.SearchCollection().Where(Search.Columns.ApplicationId, ApplicationId).Load();
				colSearchRecords.ListChanged += new ListChangedEventHandler(colSearchRecords_ListChanged);
			}
			return colSearchRecords;
		}
				
		void colSearchRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colSearchRecords[e.NewIndex].ApplicationId = ApplicationId;
				colSearchRecords.ListChanged += new ListChangedEventHandler(colSearchRecords_ListChanged);
            }
		}
		private Wcss.ShipmentBatchCollection colShipmentBatchRecords;
		public Wcss.ShipmentBatchCollection ShipmentBatchRecords()
		{
			if(colShipmentBatchRecords == null)
			{
				colShipmentBatchRecords = new Wcss.ShipmentBatchCollection().Where(ShipmentBatch.Columns.ApplicationId, ApplicationId).Load();
				colShipmentBatchRecords.ListChanged += new ListChangedEventHandler(colShipmentBatchRecords_ListChanged);
			}
			return colShipmentBatchRecords;
		}
				
		void colShipmentBatchRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colShipmentBatchRecords[e.NewIndex].ApplicationId = ApplicationId;
				colShipmentBatchRecords.ListChanged += new ListChangedEventHandler(colShipmentBatchRecords_ListChanged);
            }
		}
		private Wcss.ShowCollection colShowRecords;
		public Wcss.ShowCollection ShowRecords()
		{
			if(colShowRecords == null)
			{
				colShowRecords = new Wcss.ShowCollection().Where(Show.Columns.ApplicationId, ApplicationId).Load();
				colShowRecords.ListChanged += new ListChangedEventHandler(colShowRecords_ListChanged);
			}
			return colShowRecords;
		}
				
		void colShowRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colShowRecords[e.NewIndex].ApplicationId = ApplicationId;
				colShowRecords.ListChanged += new ListChangedEventHandler(colShowRecords_ListChanged);
            }
		}
		private Wcss.SiteConfigCollection colSiteConfigRecords;
		public Wcss.SiteConfigCollection SiteConfigRecords()
		{
			if(colSiteConfigRecords == null)
			{
				colSiteConfigRecords = new Wcss.SiteConfigCollection().Where(SiteConfig.Columns.ApplicationId, ApplicationId).Load();
				colSiteConfigRecords.ListChanged += new ListChangedEventHandler(colSiteConfigRecords_ListChanged);
			}
			return colSiteConfigRecords;
		}
				
		void colSiteConfigRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colSiteConfigRecords[e.NewIndex].ApplicationId = ApplicationId;
				colSiteConfigRecords.ListChanged += new ListChangedEventHandler(colSiteConfigRecords_ListChanged);
            }
		}
		private Wcss.StoreCreditCollection colStoreCreditRecords;
		public Wcss.StoreCreditCollection StoreCreditRecords()
		{
			if(colStoreCreditRecords == null)
			{
				colStoreCreditRecords = new Wcss.StoreCreditCollection().Where(StoreCredit.Columns.ApplicationId, ApplicationId).Load();
				colStoreCreditRecords.ListChanged += new ListChangedEventHandler(colStoreCreditRecords_ListChanged);
			}
			return colStoreCreditRecords;
		}
				
		void colStoreCreditRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colStoreCreditRecords[e.NewIndex].ApplicationId = ApplicationId;
				colStoreCreditRecords.ListChanged += new ListChangedEventHandler(colStoreCreditRecords_ListChanged);
            }
		}
		private Wcss.SubscriptionCollection colSubscriptionRecords;
		public Wcss.SubscriptionCollection SubscriptionRecords()
		{
			if(colSubscriptionRecords == null)
			{
				colSubscriptionRecords = new Wcss.SubscriptionCollection().Where(Subscription.Columns.ApplicationId, ApplicationId).Load();
				colSubscriptionRecords.ListChanged += new ListChangedEventHandler(colSubscriptionRecords_ListChanged);
			}
			return colSubscriptionRecords;
		}
				
		void colSubscriptionRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colSubscriptionRecords[e.NewIndex].ApplicationId = ApplicationId;
				colSubscriptionRecords.ListChanged += new ListChangedEventHandler(colSubscriptionRecords_ListChanged);
            }
		}
		private Wcss.VendorCollection colVendorRecords;
		public Wcss.VendorCollection VendorRecords()
		{
			if(colVendorRecords == null)
			{
				colVendorRecords = new Wcss.VendorCollection().Where(Vendor.Columns.ApplicationId, ApplicationId).Load();
				colVendorRecords.ListChanged += new ListChangedEventHandler(colVendorRecords_ListChanged);
			}
			return colVendorRecords;
		}
				
		void colVendorRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colVendorRecords[e.NewIndex].ApplicationId = ApplicationId;
				colVendorRecords.ListChanged += new ListChangedEventHandler(colVendorRecords_ListChanged);
            }
		}
		private Wcss.VenueCollection colVenueRecords;
		public Wcss.VenueCollection VenueRecords()
		{
			if(colVenueRecords == null)
			{
				colVenueRecords = new Wcss.VenueCollection().Where(Venue.Columns.ApplicationId, ApplicationId).Load();
				colVenueRecords.ListChanged += new ListChangedEventHandler(colVenueRecords_ListChanged);
			}
			return colVenueRecords;
		}
				
		void colVenueRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colVenueRecords[e.NewIndex].ApplicationId = ApplicationId;
				colVenueRecords.ListChanged += new ListChangedEventHandler(colVenueRecords_ListChanged);
            }
		}
		#endregion
		
			
		
		//no foreign key tables defined (0)
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(string varApplicationName,string varLoweredApplicationName,Guid varApplicationId,string varDescription)
		{
			AspnetApplication item = new AspnetApplication();
			
			item.ApplicationName = varApplicationName;
			
			item.LoweredApplicationName = varLoweredApplicationName;
			
			item.ApplicationId = varApplicationId;
			
			item.Description = varDescription;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(string varApplicationName,string varLoweredApplicationName,Guid varApplicationId,string varDescription)
		{
			AspnetApplication item = new AspnetApplication();
			
				item.ApplicationName = varApplicationName;
			
				item.LoweredApplicationName = varLoweredApplicationName;
			
				item.ApplicationId = varApplicationId;
			
				item.Description = varDescription;
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn ApplicationNameColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn LoweredApplicationNameColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn ApplicationIdColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn DescriptionColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string ApplicationName = @"ApplicationName";
			 public static string LoweredApplicationName = @"LoweredApplicationName";
			 public static string ApplicationId = @"ApplicationId";
			 public static string Description = @"Description";
						
		}
		#endregion
		
		#region Update PK Collections
		
        public void SetPKValues()
        {
                if (colAspnetMembershipRecords != null)
                {
                    foreach (Wcss.AspnetMembership item in colAspnetMembershipRecords)
                    {
                        if (item.ApplicationId != ApplicationId)
                        {
                            item.ApplicationId = ApplicationId;
                        }
                    }
               }
		
                if (colAspnetPaths != null)
                {
                    foreach (Wcss.AspnetPath item in colAspnetPaths)
                    {
                        if (item.ApplicationId != ApplicationId)
                        {
                            item.ApplicationId = ApplicationId;
                        }
                    }
               }
		
                if (colAspnetRoles != null)
                {
                    foreach (Wcss.AspnetRole item in colAspnetRoles)
                    {
                        if (item.ApplicationId != ApplicationId)
                        {
                            item.ApplicationId = ApplicationId;
                        }
                    }
               }
		
                if (colAspnetUsers != null)
                {
                    foreach (Wcss.AspnetUser item in colAspnetUsers)
                    {
                        if (item.ApplicationId != ApplicationId)
                        {
                            item.ApplicationId = ApplicationId;
                        }
                    }
               }
		
                if (colActRecords != null)
                {
                    foreach (Wcss.Act item in colActRecords)
                    {
                        if (item.ApplicationId != ApplicationId)
                        {
                            item.ApplicationId = ApplicationId;
                        }
                    }
               }
		
                if (colActivationWindowRecords != null)
                {
                    foreach (Wcss.ActivationWindow item in colActivationWindowRecords)
                    {
                        if (item.ApplicationId != ApplicationId)
                        {
                            item.ApplicationId = ApplicationId;
                        }
                    }
               }
		
                if (colAgeRecords != null)
                {
                    foreach (Wcss.Age item in colAgeRecords)
                    {
                        if (item.ApplicationId != ApplicationId)
                        {
                            item.ApplicationId = ApplicationId;
                        }
                    }
               }
		
                if (colAuthorizeNetRecords != null)
                {
                    foreach (Wcss.AuthorizeNet item in colAuthorizeNetRecords)
                    {
                        if (item.ApplicationId != ApplicationId)
                        {
                            item.ApplicationId = ApplicationId;
                        }
                    }
               }
		
                if (colChargeStatementRecords != null)
                {
                    foreach (Wcss.ChargeStatement item in colChargeStatementRecords)
                    {
                        if (item.ApplicationId != ApplicationId)
                        {
                            item.ApplicationId = ApplicationId;
                        }
                    }
               }
		
                if (colCharitableListingRecords != null)
                {
                    foreach (Wcss.CharitableListing item in colCharitableListingRecords)
                    {
                        if (item.ApplicationId != ApplicationId)
                        {
                            item.ApplicationId = ApplicationId;
                        }
                    }
               }
		
                if (colCharitableOrgRecords != null)
                {
                    foreach (Wcss.CharitableOrg item in colCharitableOrgRecords)
                    {
                        if (item.ApplicationId != ApplicationId)
                        {
                            item.ApplicationId = ApplicationId;
                        }
                    }
               }
		
                if (colDownloadRecords != null)
                {
                    foreach (Wcss.Download item in colDownloadRecords)
                    {
                        if (item.ApplicationId != ApplicationId)
                        {
                            item.ApplicationId = ApplicationId;
                        }
                    }
               }
		
                if (colEmailLetterRecords != null)
                {
                    foreach (Wcss.EmailLetter item in colEmailLetterRecords)
                    {
                        if (item.ApplicationId != ApplicationId)
                        {
                            item.ApplicationId = ApplicationId;
                        }
                    }
               }
		
                if (colEventQRecords != null)
                {
                    foreach (Wcss.EventQ item in colEventQRecords)
                    {
                        if (item.ApplicationId != ApplicationId)
                        {
                            item.ApplicationId = ApplicationId;
                        }
                    }
               }
		
                if (colFaqCategorieRecords != null)
                {
                    foreach (Wcss.FaqCategorie item in colFaqCategorieRecords)
                    {
                        if (item.ApplicationId != ApplicationId)
                        {
                            item.ApplicationId = ApplicationId;
                        }
                    }
               }
		
                if (colFbStatRecords != null)
                {
                    foreach (Wcss.FbStat item in colFbStatRecords)
                    {
                        if (item.ApplicationId != ApplicationId)
                        {
                            item.ApplicationId = ApplicationId;
                        }
                    }
               }
		
                if (colFraudScreenRecords != null)
                {
                    foreach (Wcss.FraudScreen item in colFraudScreenRecords)
                    {
                        if (item.ApplicationId != ApplicationId)
                        {
                            item.ApplicationId = ApplicationId;
                        }
                    }
               }
		
                if (colHintQuestionRecords != null)
                {
                    foreach (Wcss.HintQuestion item in colHintQuestionRecords)
                    {
                        if (item.ApplicationId != ApplicationId)
                        {
                            item.ApplicationId = ApplicationId;
                        }
                    }
               }
		
                if (colInvoiceRecords != null)
                {
                    foreach (Wcss.Invoice item in colInvoiceRecords)
                    {
                        if (item.ApplicationId != ApplicationId)
                        {
                            item.ApplicationId = ApplicationId;
                        }
                    }
               }
		
                if (colInvoiceFeeRecords != null)
                {
                    foreach (Wcss.InvoiceFee item in colInvoiceFeeRecords)
                    {
                        if (item.ApplicationId != ApplicationId)
                        {
                            item.ApplicationId = ApplicationId;
                        }
                    }
               }
		
                if (colMailQueueRecords != null)
                {
                    foreach (Wcss.MailQueue item in colMailQueueRecords)
                    {
                        if (item.ApplicationId != ApplicationId)
                        {
                            item.ApplicationId = ApplicationId;
                        }
                    }
               }
		
                if (colMerchRecords != null)
                {
                    foreach (Wcss.Merch item in colMerchRecords)
                    {
                        if (item.ApplicationId != ApplicationId)
                        {
                            item.ApplicationId = ApplicationId;
                        }
                    }
               }
		
                if (colMerchColorRecords != null)
                {
                    foreach (Wcss.MerchColor item in colMerchColorRecords)
                    {
                        if (item.ApplicationId != ApplicationId)
                        {
                            item.ApplicationId = ApplicationId;
                        }
                    }
               }
		
                if (colMerchDivisionRecords != null)
                {
                    foreach (Wcss.MerchDivision item in colMerchDivisionRecords)
                    {
                        if (item.ApplicationId != ApplicationId)
                        {
                            item.ApplicationId = ApplicationId;
                        }
                    }
               }
		
                if (colMerchSizeRecords != null)
                {
                    foreach (Wcss.MerchSize item in colMerchSizeRecords)
                    {
                        if (item.ApplicationId != ApplicationId)
                        {
                            item.ApplicationId = ApplicationId;
                        }
                    }
               }
		
                if (colProductAccessRecords != null)
                {
                    foreach (Wcss.ProductAccess item in colProductAccessRecords)
                    {
                        if (item.ApplicationId != ApplicationId)
                        {
                            item.ApplicationId = ApplicationId;
                        }
                    }
               }
		
                if (colPromoterRecords != null)
                {
                    foreach (Wcss.Promoter item in colPromoterRecords)
                    {
                        if (item.ApplicationId != ApplicationId)
                        {
                            item.ApplicationId = ApplicationId;
                        }
                    }
               }
		
                if (colReportDailySales != null)
                {
                    foreach (Wcss.ReportDailySale item in colReportDailySales)
                    {
                        if (item.ApplicationId != ApplicationId)
                        {
                            item.ApplicationId = ApplicationId;
                        }
                    }
               }
		
                if (colSalePromotionRecords != null)
                {
                    foreach (Wcss.SalePromotion item in colSalePromotionRecords)
                    {
                        if (item.ApplicationId != ApplicationId)
                        {
                            item.ApplicationId = ApplicationId;
                        }
                    }
               }
		
                if (colSaleRuleRecords != null)
                {
                    foreach (Wcss.SaleRule item in colSaleRuleRecords)
                    {
                        if (item.ApplicationId != ApplicationId)
                        {
                            item.ApplicationId = ApplicationId;
                        }
                    }
               }
		
                if (colSearchRecords != null)
                {
                    foreach (Wcss.Search item in colSearchRecords)
                    {
                        if (item.ApplicationId != ApplicationId)
                        {
                            item.ApplicationId = ApplicationId;
                        }
                    }
               }
		
                if (colShipmentBatchRecords != null)
                {
                    foreach (Wcss.ShipmentBatch item in colShipmentBatchRecords)
                    {
                        if (item.ApplicationId != ApplicationId)
                        {
                            item.ApplicationId = ApplicationId;
                        }
                    }
               }
		
                if (colShowRecords != null)
                {
                    foreach (Wcss.Show item in colShowRecords)
                    {
                        if (item.ApplicationId != ApplicationId)
                        {
                            item.ApplicationId = ApplicationId;
                        }
                    }
               }
		
                if (colSiteConfigRecords != null)
                {
                    foreach (Wcss.SiteConfig item in colSiteConfigRecords)
                    {
                        if (item.ApplicationId != ApplicationId)
                        {
                            item.ApplicationId = ApplicationId;
                        }
                    }
               }
		
                if (colStoreCreditRecords != null)
                {
                    foreach (Wcss.StoreCredit item in colStoreCreditRecords)
                    {
                        if (item.ApplicationId != ApplicationId)
                        {
                            item.ApplicationId = ApplicationId;
                        }
                    }
               }
		
                if (colSubscriptionRecords != null)
                {
                    foreach (Wcss.Subscription item in colSubscriptionRecords)
                    {
                        if (item.ApplicationId != ApplicationId)
                        {
                            item.ApplicationId = ApplicationId;
                        }
                    }
               }
		
                if (colVendorRecords != null)
                {
                    foreach (Wcss.Vendor item in colVendorRecords)
                    {
                        if (item.ApplicationId != ApplicationId)
                        {
                            item.ApplicationId = ApplicationId;
                        }
                    }
               }
		
                if (colVenueRecords != null)
                {
                    foreach (Wcss.Venue item in colVenueRecords)
                    {
                        if (item.ApplicationId != ApplicationId)
                        {
                            item.ApplicationId = ApplicationId;
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
		
                if (colAspnetPaths != null)
                {
                    colAspnetPaths.SaveAll();
               }
		
                if (colAspnetRoles != null)
                {
                    colAspnetRoles.SaveAll();
               }
		
                if (colAspnetUsers != null)
                {
                    colAspnetUsers.SaveAll();
               }
		
                if (colActRecords != null)
                {
                    colActRecords.SaveAll();
               }
		
                if (colActivationWindowRecords != null)
                {
                    colActivationWindowRecords.SaveAll();
               }
		
                if (colAgeRecords != null)
                {
                    colAgeRecords.SaveAll();
               }
		
                if (colAuthorizeNetRecords != null)
                {
                    colAuthorizeNetRecords.SaveAll();
               }
		
                if (colChargeStatementRecords != null)
                {
                    colChargeStatementRecords.SaveAll();
               }
		
                if (colCharitableListingRecords != null)
                {
                    colCharitableListingRecords.SaveAll();
               }
		
                if (colCharitableOrgRecords != null)
                {
                    colCharitableOrgRecords.SaveAll();
               }
		
                if (colDownloadRecords != null)
                {
                    colDownloadRecords.SaveAll();
               }
		
                if (colEmailLetterRecords != null)
                {
                    colEmailLetterRecords.SaveAll();
               }
		
                if (colEventQRecords != null)
                {
                    colEventQRecords.SaveAll();
               }
		
                if (colFaqCategorieRecords != null)
                {
                    colFaqCategorieRecords.SaveAll();
               }
		
                if (colFbStatRecords != null)
                {
                    colFbStatRecords.SaveAll();
               }
		
                if (colFraudScreenRecords != null)
                {
                    colFraudScreenRecords.SaveAll();
               }
		
                if (colHintQuestionRecords != null)
                {
                    colHintQuestionRecords.SaveAll();
               }
		
                if (colInvoiceRecords != null)
                {
                    colInvoiceRecords.SaveAll();
               }
		
                if (colInvoiceFeeRecords != null)
                {
                    colInvoiceFeeRecords.SaveAll();
               }
		
                if (colMailQueueRecords != null)
                {
                    colMailQueueRecords.SaveAll();
               }
		
                if (colMerchRecords != null)
                {
                    colMerchRecords.SaveAll();
               }
		
                if (colMerchColorRecords != null)
                {
                    colMerchColorRecords.SaveAll();
               }
		
                if (colMerchDivisionRecords != null)
                {
                    colMerchDivisionRecords.SaveAll();
               }
		
                if (colMerchSizeRecords != null)
                {
                    colMerchSizeRecords.SaveAll();
               }
		
                if (colProductAccessRecords != null)
                {
                    colProductAccessRecords.SaveAll();
               }
		
                if (colPromoterRecords != null)
                {
                    colPromoterRecords.SaveAll();
               }
		
                if (colReportDailySales != null)
                {
                    colReportDailySales.SaveAll();
               }
		
                if (colSalePromotionRecords != null)
                {
                    colSalePromotionRecords.SaveAll();
               }
		
                if (colSaleRuleRecords != null)
                {
                    colSaleRuleRecords.SaveAll();
               }
		
                if (colSearchRecords != null)
                {
                    colSearchRecords.SaveAll();
               }
		
                if (colShipmentBatchRecords != null)
                {
                    colShipmentBatchRecords.SaveAll();
               }
		
                if (colShowRecords != null)
                {
                    colShowRecords.SaveAll();
               }
		
                if (colSiteConfigRecords != null)
                {
                    colSiteConfigRecords.SaveAll();
               }
		
                if (colStoreCreditRecords != null)
                {
                    colStoreCreditRecords.SaveAll();
               }
		
                if (colSubscriptionRecords != null)
                {
                    colSubscriptionRecords.SaveAll();
               }
		
                if (colVendorRecords != null)
                {
                    colVendorRecords.SaveAll();
               }
		
                if (colVenueRecords != null)
                {
                    colVenueRecords.SaveAll();
               }
		}
        #endregion
	}
}
