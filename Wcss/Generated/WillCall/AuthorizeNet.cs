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
	/// Strongly-typed collection for the AuthorizeNet class.
	/// </summary>
    [Serializable]
	public partial class AuthorizeNetCollection : ActiveList<AuthorizeNet, AuthorizeNetCollection>
	{	   
		public AuthorizeNetCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>AuthorizeNetCollection</returns>
		public AuthorizeNetCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                AuthorizeNet o = this[i];
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
	/// This is an ActiveRecord class which wraps the AuthorizeNet table.
	/// </summary>
	[Serializable]
	public partial class AuthorizeNet : ActiveRecord<AuthorizeNet>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public AuthorizeNet()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public AuthorizeNet(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public AuthorizeNet(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public AuthorizeNet(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("AuthorizeNet", TableType.Table, DataService.GetInstance("WillCall"));
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
				
				TableSchema.TableColumn colvarInvoiceNumber = new TableSchema.TableColumn(schema);
				colvarInvoiceNumber.ColumnName = "InvoiceNumber";
				colvarInvoiceNumber.DataType = DbType.AnsiString;
				colvarInvoiceNumber.MaxLength = 20;
				colvarInvoiceNumber.AutoIncrement = false;
				colvarInvoiceNumber.IsNullable = true;
				colvarInvoiceNumber.IsPrimaryKey = false;
				colvarInvoiceNumber.IsForeignKey = false;
				colvarInvoiceNumber.IsReadOnly = false;
				colvarInvoiceNumber.DefaultSetting = @"";
				colvarInvoiceNumber.ForeignKeyTableName = "";
				schema.Columns.Add(colvarInvoiceNumber);
				
				TableSchema.TableColumn colvarBAuthorized = new TableSchema.TableColumn(schema);
				colvarBAuthorized.ColumnName = "bAuthorized";
				colvarBAuthorized.DataType = DbType.Boolean;
				colvarBAuthorized.MaxLength = 0;
				colvarBAuthorized.AutoIncrement = false;
				colvarBAuthorized.IsNullable = true;
				colvarBAuthorized.IsPrimaryKey = false;
				colvarBAuthorized.IsForeignKey = false;
				colvarBAuthorized.IsReadOnly = false;
				colvarBAuthorized.DefaultSetting = @"";
				colvarBAuthorized.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBAuthorized);
				
				TableSchema.TableColumn colvarTInvoiceId = new TableSchema.TableColumn(schema);
				colvarTInvoiceId.ColumnName = "TInvoiceId";
				colvarTInvoiceId.DataType = DbType.Int32;
				colvarTInvoiceId.MaxLength = 0;
				colvarTInvoiceId.AutoIncrement = false;
				colvarTInvoiceId.IsNullable = true;
				colvarTInvoiceId.IsPrimaryKey = false;
				colvarTInvoiceId.IsForeignKey = true;
				colvarTInvoiceId.IsReadOnly = false;
				colvarTInvoiceId.DefaultSetting = @"";
				
					colvarTInvoiceId.ForeignKeyTableName = "Invoice";
				schema.Columns.Add(colvarTInvoiceId);
				
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
				
				TableSchema.TableColumn colvarCustomerId = new TableSchema.TableColumn(schema);
				colvarCustomerId.ColumnName = "CustomerId";
				colvarCustomerId.DataType = DbType.Int32;
				colvarCustomerId.MaxLength = 0;
				colvarCustomerId.AutoIncrement = false;
				colvarCustomerId.IsNullable = true;
				colvarCustomerId.IsPrimaryKey = false;
				colvarCustomerId.IsForeignKey = false;
				colvarCustomerId.IsReadOnly = false;
				colvarCustomerId.DefaultSetting = @"";
				colvarCustomerId.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCustomerId);
				
				TableSchema.TableColumn colvarProcessorId = new TableSchema.TableColumn(schema);
				colvarProcessorId.ColumnName = "ProcessorId";
				colvarProcessorId.DataType = DbType.AnsiString;
				colvarProcessorId.MaxLength = 50;
				colvarProcessorId.AutoIncrement = false;
				colvarProcessorId.IsNullable = true;
				colvarProcessorId.IsPrimaryKey = false;
				colvarProcessorId.IsForeignKey = false;
				colvarProcessorId.IsReadOnly = false;
				colvarProcessorId.DefaultSetting = @"";
				colvarProcessorId.ForeignKeyTableName = "";
				schema.Columns.Add(colvarProcessorId);
				
				TableSchema.TableColumn colvarMethod = new TableSchema.TableColumn(schema);
				colvarMethod.ColumnName = "Method";
				colvarMethod.DataType = DbType.AnsiString;
				colvarMethod.MaxLength = 10;
				colvarMethod.AutoIncrement = false;
				colvarMethod.IsNullable = true;
				colvarMethod.IsPrimaryKey = false;
				colvarMethod.IsForeignKey = false;
				colvarMethod.IsReadOnly = false;
				colvarMethod.DefaultSetting = @"";
				colvarMethod.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMethod);
				
				TableSchema.TableColumn colvarTransactionType = new TableSchema.TableColumn(schema);
				colvarTransactionType.ColumnName = "TransactionType";
				colvarTransactionType.DataType = DbType.AnsiString;
				colvarTransactionType.MaxLength = 20;
				colvarTransactionType.AutoIncrement = false;
				colvarTransactionType.IsNullable = true;
				colvarTransactionType.IsPrimaryKey = false;
				colvarTransactionType.IsForeignKey = false;
				colvarTransactionType.IsReadOnly = false;
				colvarTransactionType.DefaultSetting = @"";
				colvarTransactionType.ForeignKeyTableName = "";
				schema.Columns.Add(colvarTransactionType);
				
				TableSchema.TableColumn colvarMAmount = new TableSchema.TableColumn(schema);
				colvarMAmount.ColumnName = "mAmount";
				colvarMAmount.DataType = DbType.Currency;
				colvarMAmount.MaxLength = 0;
				colvarMAmount.AutoIncrement = false;
				colvarMAmount.IsNullable = true;
				colvarMAmount.IsPrimaryKey = false;
				colvarMAmount.IsForeignKey = false;
				colvarMAmount.IsReadOnly = false;
				colvarMAmount.DefaultSetting = @"";
				colvarMAmount.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMAmount);
				
				TableSchema.TableColumn colvarMTaxAmount = new TableSchema.TableColumn(schema);
				colvarMTaxAmount.ColumnName = "mTaxAmount";
				colvarMTaxAmount.DataType = DbType.Currency;
				colvarMTaxAmount.MaxLength = 0;
				colvarMTaxAmount.AutoIncrement = false;
				colvarMTaxAmount.IsNullable = true;
				colvarMTaxAmount.IsPrimaryKey = false;
				colvarMTaxAmount.IsForeignKey = false;
				colvarMTaxAmount.IsReadOnly = false;
				colvarMTaxAmount.DefaultSetting = @"";
				colvarMTaxAmount.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMTaxAmount);
				
				TableSchema.TableColumn colvarMFreightAmount = new TableSchema.TableColumn(schema);
				colvarMFreightAmount.ColumnName = "mFreightAmount";
				colvarMFreightAmount.DataType = DbType.Currency;
				colvarMFreightAmount.MaxLength = 0;
				colvarMFreightAmount.AutoIncrement = false;
				colvarMFreightAmount.IsNullable = true;
				colvarMFreightAmount.IsPrimaryKey = false;
				colvarMFreightAmount.IsForeignKey = false;
				colvarMFreightAmount.IsReadOnly = false;
				colvarMFreightAmount.DefaultSetting = @"";
				colvarMFreightAmount.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMFreightAmount);
				
				TableSchema.TableColumn colvarDescription = new TableSchema.TableColumn(schema);
				colvarDescription.ColumnName = "Description";
				colvarDescription.DataType = DbType.AnsiString;
				colvarDescription.MaxLength = 1000;
				colvarDescription.AutoIncrement = false;
				colvarDescription.IsNullable = true;
				colvarDescription.IsPrimaryKey = false;
				colvarDescription.IsForeignKey = false;
				colvarDescription.IsReadOnly = false;
				colvarDescription.DefaultSetting = @"";
				colvarDescription.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDescription);
				
				TableSchema.TableColumn colvarIDupeSeconds = new TableSchema.TableColumn(schema);
				colvarIDupeSeconds.ColumnName = "iDupeSeconds";
				colvarIDupeSeconds.DataType = DbType.Int32;
				colvarIDupeSeconds.MaxLength = 0;
				colvarIDupeSeconds.AutoIncrement = false;
				colvarIDupeSeconds.IsNullable = true;
				colvarIDupeSeconds.IsPrimaryKey = false;
				colvarIDupeSeconds.IsForeignKey = false;
				colvarIDupeSeconds.IsReadOnly = false;
				colvarIDupeSeconds.DefaultSetting = @"";
				colvarIDupeSeconds.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIDupeSeconds);
				
				TableSchema.TableColumn colvarIResponseCode = new TableSchema.TableColumn(schema);
				colvarIResponseCode.ColumnName = "iResponseCode";
				colvarIResponseCode.DataType = DbType.Int32;
				colvarIResponseCode.MaxLength = 0;
				colvarIResponseCode.AutoIncrement = false;
				colvarIResponseCode.IsNullable = true;
				colvarIResponseCode.IsPrimaryKey = false;
				colvarIResponseCode.IsForeignKey = false;
				colvarIResponseCode.IsReadOnly = false;
				colvarIResponseCode.DefaultSetting = @"";
				colvarIResponseCode.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIResponseCode);
				
				TableSchema.TableColumn colvarResponseSubcode = new TableSchema.TableColumn(schema);
				colvarResponseSubcode.ColumnName = "ResponseSubcode";
				colvarResponseSubcode.DataType = DbType.AnsiString;
				colvarResponseSubcode.MaxLength = 10;
				colvarResponseSubcode.AutoIncrement = false;
				colvarResponseSubcode.IsNullable = true;
				colvarResponseSubcode.IsPrimaryKey = false;
				colvarResponseSubcode.IsForeignKey = false;
				colvarResponseSubcode.IsReadOnly = false;
				colvarResponseSubcode.DefaultSetting = @"";
				colvarResponseSubcode.ForeignKeyTableName = "";
				schema.Columns.Add(colvarResponseSubcode);
				
				TableSchema.TableColumn colvarIResponseReasonCode = new TableSchema.TableColumn(schema);
				colvarIResponseReasonCode.ColumnName = "iResponseReasonCode";
				colvarIResponseReasonCode.DataType = DbType.Int32;
				colvarIResponseReasonCode.MaxLength = 0;
				colvarIResponseReasonCode.AutoIncrement = false;
				colvarIResponseReasonCode.IsNullable = true;
				colvarIResponseReasonCode.IsPrimaryKey = false;
				colvarIResponseReasonCode.IsForeignKey = false;
				colvarIResponseReasonCode.IsReadOnly = false;
				colvarIResponseReasonCode.DefaultSetting = @"";
				colvarIResponseReasonCode.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIResponseReasonCode);
				
				TableSchema.TableColumn colvarBMd5Match = new TableSchema.TableColumn(schema);
				colvarBMd5Match.ColumnName = "bMd5Match";
				colvarBMd5Match.DataType = DbType.Boolean;
				colvarBMd5Match.MaxLength = 0;
				colvarBMd5Match.AutoIncrement = false;
				colvarBMd5Match.IsNullable = true;
				colvarBMd5Match.IsPrimaryKey = false;
				colvarBMd5Match.IsForeignKey = false;
				colvarBMd5Match.IsReadOnly = false;
				colvarBMd5Match.DefaultSetting = @"";
				colvarBMd5Match.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBMd5Match);
				
				TableSchema.TableColumn colvarResponseReasonText = new TableSchema.TableColumn(schema);
				colvarResponseReasonText.ColumnName = "ResponseReasonText";
				colvarResponseReasonText.DataType = DbType.AnsiString;
				colvarResponseReasonText.MaxLength = 255;
				colvarResponseReasonText.AutoIncrement = false;
				colvarResponseReasonText.IsNullable = true;
				colvarResponseReasonText.IsPrimaryKey = false;
				colvarResponseReasonText.IsForeignKey = false;
				colvarResponseReasonText.IsReadOnly = false;
				colvarResponseReasonText.DefaultSetting = @"";
				colvarResponseReasonText.ForeignKeyTableName = "";
				schema.Columns.Add(colvarResponseReasonText);
				
				TableSchema.TableColumn colvarApprovalCode = new TableSchema.TableColumn(schema);
				colvarApprovalCode.ColumnName = "ApprovalCode";
				colvarApprovalCode.DataType = DbType.AnsiString;
				colvarApprovalCode.MaxLength = 6;
				colvarApprovalCode.AutoIncrement = false;
				colvarApprovalCode.IsNullable = true;
				colvarApprovalCode.IsPrimaryKey = false;
				colvarApprovalCode.IsForeignKey = false;
				colvarApprovalCode.IsReadOnly = false;
				colvarApprovalCode.DefaultSetting = @"";
				colvarApprovalCode.ForeignKeyTableName = "";
				schema.Columns.Add(colvarApprovalCode);
				
				TableSchema.TableColumn colvarAVSResultCode = new TableSchema.TableColumn(schema);
				colvarAVSResultCode.ColumnName = "AVSResultCode";
				colvarAVSResultCode.DataType = DbType.AnsiString;
				colvarAVSResultCode.MaxLength = 10;
				colvarAVSResultCode.AutoIncrement = false;
				colvarAVSResultCode.IsNullable = true;
				colvarAVSResultCode.IsPrimaryKey = false;
				colvarAVSResultCode.IsForeignKey = false;
				colvarAVSResultCode.IsReadOnly = false;
				colvarAVSResultCode.DefaultSetting = @"";
				colvarAVSResultCode.ForeignKeyTableName = "";
				schema.Columns.Add(colvarAVSResultCode);
				
				TableSchema.TableColumn colvarCardCodeResponseCode = new TableSchema.TableColumn(schema);
				colvarCardCodeResponseCode.ColumnName = "CardCodeResponseCode";
				colvarCardCodeResponseCode.DataType = DbType.AnsiString;
				colvarCardCodeResponseCode.MaxLength = 10;
				colvarCardCodeResponseCode.AutoIncrement = false;
				colvarCardCodeResponseCode.IsNullable = true;
				colvarCardCodeResponseCode.IsPrimaryKey = false;
				colvarCardCodeResponseCode.IsForeignKey = false;
				colvarCardCodeResponseCode.IsReadOnly = false;
				colvarCardCodeResponseCode.DefaultSetting = @"";
				colvarCardCodeResponseCode.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCardCodeResponseCode);
				
				TableSchema.TableColumn colvarEmail = new TableSchema.TableColumn(schema);
				colvarEmail.ColumnName = "Email";
				colvarEmail.DataType = DbType.AnsiString;
				colvarEmail.MaxLength = 255;
				colvarEmail.AutoIncrement = false;
				colvarEmail.IsNullable = true;
				colvarEmail.IsPrimaryKey = false;
				colvarEmail.IsForeignKey = false;
				colvarEmail.IsReadOnly = false;
				colvarEmail.DefaultSetting = @"";
				colvarEmail.ForeignKeyTableName = "";
				schema.Columns.Add(colvarEmail);
				
				TableSchema.TableColumn colvarFirstName = new TableSchema.TableColumn(schema);
				colvarFirstName.ColumnName = "FirstName";
				colvarFirstName.DataType = DbType.AnsiString;
				colvarFirstName.MaxLength = 50;
				colvarFirstName.AutoIncrement = false;
				colvarFirstName.IsNullable = true;
				colvarFirstName.IsPrimaryKey = false;
				colvarFirstName.IsForeignKey = false;
				colvarFirstName.IsReadOnly = false;
				colvarFirstName.DefaultSetting = @"";
				colvarFirstName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarFirstName);
				
				TableSchema.TableColumn colvarLastName = new TableSchema.TableColumn(schema);
				colvarLastName.ColumnName = "LastName";
				colvarLastName.DataType = DbType.AnsiString;
				colvarLastName.MaxLength = 50;
				colvarLastName.AutoIncrement = false;
				colvarLastName.IsNullable = true;
				colvarLastName.IsPrimaryKey = false;
				colvarLastName.IsForeignKey = false;
				colvarLastName.IsReadOnly = false;
				colvarLastName.DefaultSetting = @"";
				colvarLastName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarLastName);
				
				TableSchema.TableColumn colvarNameOnCard = new TableSchema.TableColumn(schema);
				colvarNameOnCard.ColumnName = "NameOnCard";
				colvarNameOnCard.DataType = DbType.AnsiString;
				colvarNameOnCard.MaxLength = 50;
				colvarNameOnCard.AutoIncrement = false;
				colvarNameOnCard.IsNullable = true;
				colvarNameOnCard.IsPrimaryKey = false;
				colvarNameOnCard.IsForeignKey = false;
				colvarNameOnCard.IsReadOnly = false;
				colvarNameOnCard.DefaultSetting = @"";
				colvarNameOnCard.ForeignKeyTableName = "";
				schema.Columns.Add(colvarNameOnCard);
				
				TableSchema.TableColumn colvarCompany = new TableSchema.TableColumn(schema);
				colvarCompany.ColumnName = "Company";
				colvarCompany.DataType = DbType.AnsiString;
				colvarCompany.MaxLength = 50;
				colvarCompany.AutoIncrement = false;
				colvarCompany.IsNullable = true;
				colvarCompany.IsPrimaryKey = false;
				colvarCompany.IsForeignKey = false;
				colvarCompany.IsReadOnly = false;
				colvarCompany.DefaultSetting = @"";
				colvarCompany.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCompany);
				
				TableSchema.TableColumn colvarBillingAddress = new TableSchema.TableColumn(schema);
				colvarBillingAddress.ColumnName = "BillingAddress";
				colvarBillingAddress.DataType = DbType.AnsiString;
				colvarBillingAddress.MaxLength = 60;
				colvarBillingAddress.AutoIncrement = false;
				colvarBillingAddress.IsNullable = true;
				colvarBillingAddress.IsPrimaryKey = false;
				colvarBillingAddress.IsForeignKey = false;
				colvarBillingAddress.IsReadOnly = false;
				colvarBillingAddress.DefaultSetting = @"";
				colvarBillingAddress.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBillingAddress);
				
				TableSchema.TableColumn colvarCity = new TableSchema.TableColumn(schema);
				colvarCity.ColumnName = "City";
				colvarCity.DataType = DbType.AnsiString;
				colvarCity.MaxLength = 40;
				colvarCity.AutoIncrement = false;
				colvarCity.IsNullable = true;
				colvarCity.IsPrimaryKey = false;
				colvarCity.IsForeignKey = false;
				colvarCity.IsReadOnly = false;
				colvarCity.DefaultSetting = @"";
				colvarCity.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCity);
				
				TableSchema.TableColumn colvarState = new TableSchema.TableColumn(schema);
				colvarState.ColumnName = "State";
				colvarState.DataType = DbType.AnsiString;
				colvarState.MaxLength = 40;
				colvarState.AutoIncrement = false;
				colvarState.IsNullable = true;
				colvarState.IsPrimaryKey = false;
				colvarState.IsForeignKey = false;
				colvarState.IsReadOnly = false;
				colvarState.DefaultSetting = @"";
				colvarState.ForeignKeyTableName = "";
				schema.Columns.Add(colvarState);
				
				TableSchema.TableColumn colvarZip = new TableSchema.TableColumn(schema);
				colvarZip.ColumnName = "Zip";
				colvarZip.DataType = DbType.AnsiString;
				colvarZip.MaxLength = 20;
				colvarZip.AutoIncrement = false;
				colvarZip.IsNullable = true;
				colvarZip.IsPrimaryKey = false;
				colvarZip.IsForeignKey = false;
				colvarZip.IsReadOnly = false;
				colvarZip.DefaultSetting = @"";
				colvarZip.ForeignKeyTableName = "";
				schema.Columns.Add(colvarZip);
				
				TableSchema.TableColumn colvarCountry = new TableSchema.TableColumn(schema);
				colvarCountry.ColumnName = "Country";
				colvarCountry.DataType = DbType.AnsiString;
				colvarCountry.MaxLength = 60;
				colvarCountry.AutoIncrement = false;
				colvarCountry.IsNullable = true;
				colvarCountry.IsPrimaryKey = false;
				colvarCountry.IsForeignKey = false;
				colvarCountry.IsReadOnly = false;
				colvarCountry.DefaultSetting = @"";
				colvarCountry.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCountry);
				
				TableSchema.TableColumn colvarPhone = new TableSchema.TableColumn(schema);
				colvarPhone.ColumnName = "Phone";
				colvarPhone.DataType = DbType.AnsiString;
				colvarPhone.MaxLength = 25;
				colvarPhone.AutoIncrement = false;
				colvarPhone.IsNullable = true;
				colvarPhone.IsPrimaryKey = false;
				colvarPhone.IsForeignKey = false;
				colvarPhone.IsReadOnly = false;
				colvarPhone.DefaultSetting = @"";
				colvarPhone.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPhone);
				
				TableSchema.TableColumn colvarIpAddress = new TableSchema.TableColumn(schema);
				colvarIpAddress.ColumnName = "IpAddress";
				colvarIpAddress.DataType = DbType.AnsiString;
				colvarIpAddress.MaxLength = 15;
				colvarIpAddress.AutoIncrement = false;
				colvarIpAddress.IsNullable = true;
				colvarIpAddress.IsPrimaryKey = false;
				colvarIpAddress.IsForeignKey = false;
				colvarIpAddress.IsReadOnly = false;
				colvarIpAddress.DefaultSetting = @"";
				colvarIpAddress.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIpAddress);
				
				TableSchema.TableColumn colvarShipToFirstName = new TableSchema.TableColumn(schema);
				colvarShipToFirstName.ColumnName = "ShipToFirstName";
				colvarShipToFirstName.DataType = DbType.AnsiString;
				colvarShipToFirstName.MaxLength = 50;
				colvarShipToFirstName.AutoIncrement = false;
				colvarShipToFirstName.IsNullable = true;
				colvarShipToFirstName.IsPrimaryKey = false;
				colvarShipToFirstName.IsForeignKey = false;
				colvarShipToFirstName.IsReadOnly = false;
				colvarShipToFirstName.DefaultSetting = @"";
				colvarShipToFirstName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarShipToFirstName);
				
				TableSchema.TableColumn colvarShipToLastName = new TableSchema.TableColumn(schema);
				colvarShipToLastName.ColumnName = "ShipToLastName";
				colvarShipToLastName.DataType = DbType.AnsiString;
				colvarShipToLastName.MaxLength = 50;
				colvarShipToLastName.AutoIncrement = false;
				colvarShipToLastName.IsNullable = true;
				colvarShipToLastName.IsPrimaryKey = false;
				colvarShipToLastName.IsForeignKey = false;
				colvarShipToLastName.IsReadOnly = false;
				colvarShipToLastName.DefaultSetting = @"";
				colvarShipToLastName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarShipToLastName);
				
				TableSchema.TableColumn colvarShipToCompany = new TableSchema.TableColumn(schema);
				colvarShipToCompany.ColumnName = "ShipToCompany";
				colvarShipToCompany.DataType = DbType.AnsiString;
				colvarShipToCompany.MaxLength = 50;
				colvarShipToCompany.AutoIncrement = false;
				colvarShipToCompany.IsNullable = true;
				colvarShipToCompany.IsPrimaryKey = false;
				colvarShipToCompany.IsForeignKey = false;
				colvarShipToCompany.IsReadOnly = false;
				colvarShipToCompany.DefaultSetting = @"";
				colvarShipToCompany.ForeignKeyTableName = "";
				schema.Columns.Add(colvarShipToCompany);
				
				TableSchema.TableColumn colvarShipToAddress = new TableSchema.TableColumn(schema);
				colvarShipToAddress.ColumnName = "ShipToAddress";
				colvarShipToAddress.DataType = DbType.AnsiString;
				colvarShipToAddress.MaxLength = 60;
				colvarShipToAddress.AutoIncrement = false;
				colvarShipToAddress.IsNullable = true;
				colvarShipToAddress.IsPrimaryKey = false;
				colvarShipToAddress.IsForeignKey = false;
				colvarShipToAddress.IsReadOnly = false;
				colvarShipToAddress.DefaultSetting = @"";
				colvarShipToAddress.ForeignKeyTableName = "";
				schema.Columns.Add(colvarShipToAddress);
				
				TableSchema.TableColumn colvarShipToCity = new TableSchema.TableColumn(schema);
				colvarShipToCity.ColumnName = "ShipToCity";
				colvarShipToCity.DataType = DbType.AnsiString;
				colvarShipToCity.MaxLength = 40;
				colvarShipToCity.AutoIncrement = false;
				colvarShipToCity.IsNullable = true;
				colvarShipToCity.IsPrimaryKey = false;
				colvarShipToCity.IsForeignKey = false;
				colvarShipToCity.IsReadOnly = false;
				colvarShipToCity.DefaultSetting = @"";
				colvarShipToCity.ForeignKeyTableName = "";
				schema.Columns.Add(colvarShipToCity);
				
				TableSchema.TableColumn colvarShipToState = new TableSchema.TableColumn(schema);
				colvarShipToState.ColumnName = "ShipToState";
				colvarShipToState.DataType = DbType.AnsiString;
				colvarShipToState.MaxLength = 40;
				colvarShipToState.AutoIncrement = false;
				colvarShipToState.IsNullable = true;
				colvarShipToState.IsPrimaryKey = false;
				colvarShipToState.IsForeignKey = false;
				colvarShipToState.IsReadOnly = false;
				colvarShipToState.DefaultSetting = @"";
				colvarShipToState.ForeignKeyTableName = "";
				schema.Columns.Add(colvarShipToState);
				
				TableSchema.TableColumn colvarShipToZip = new TableSchema.TableColumn(schema);
				colvarShipToZip.ColumnName = "ShipToZip";
				colvarShipToZip.DataType = DbType.AnsiString;
				colvarShipToZip.MaxLength = 20;
				colvarShipToZip.AutoIncrement = false;
				colvarShipToZip.IsNullable = true;
				colvarShipToZip.IsPrimaryKey = false;
				colvarShipToZip.IsForeignKey = false;
				colvarShipToZip.IsReadOnly = false;
				colvarShipToZip.DefaultSetting = @"";
				colvarShipToZip.ForeignKeyTableName = "";
				schema.Columns.Add(colvarShipToZip);
				
				TableSchema.TableColumn colvarShipToCountry = new TableSchema.TableColumn(schema);
				colvarShipToCountry.ColumnName = "ShipToCountry";
				colvarShipToCountry.DataType = DbType.AnsiString;
				colvarShipToCountry.MaxLength = 60;
				colvarShipToCountry.AutoIncrement = false;
				colvarShipToCountry.IsNullable = true;
				colvarShipToCountry.IsPrimaryKey = false;
				colvarShipToCountry.IsForeignKey = false;
				colvarShipToCountry.IsReadOnly = false;
				colvarShipToCountry.DefaultSetting = @"";
				colvarShipToCountry.ForeignKeyTableName = "";
				schema.Columns.Add(colvarShipToCountry);
				
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
				DataService.Providers["WillCall"].AddSchema("AuthorizeNet",schema);
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
		  
		[XmlAttribute("InvoiceNumber")]
		[Bindable(true)]
		public string InvoiceNumber 
		{
			get { return GetColumnValue<string>(Columns.InvoiceNumber); }
			set { SetColumnValue(Columns.InvoiceNumber, value); }
		}
		  
		[XmlAttribute("BAuthorized")]
		[Bindable(true)]
		public bool? BAuthorized 
		{
			get { return GetColumnValue<bool?>(Columns.BAuthorized); }
			set { SetColumnValue(Columns.BAuthorized, value); }
		}
		  
		[XmlAttribute("TInvoiceId")]
		[Bindable(true)]
		public int? TInvoiceId 
		{
			get { return GetColumnValue<int?>(Columns.TInvoiceId); }
			set { SetColumnValue(Columns.TInvoiceId, value); }
		}
		  
		[XmlAttribute("UserId")]
		[Bindable(true)]
		public Guid UserId 
		{
			get { return GetColumnValue<Guid>(Columns.UserId); }
			set { SetColumnValue(Columns.UserId, value); }
		}
		  
		[XmlAttribute("CustomerId")]
		[Bindable(true)]
		public int? CustomerId 
		{
			get { return GetColumnValue<int?>(Columns.CustomerId); }
			set { SetColumnValue(Columns.CustomerId, value); }
		}
		  
		[XmlAttribute("ProcessorId")]
		[Bindable(true)]
		public string ProcessorId 
		{
			get { return GetColumnValue<string>(Columns.ProcessorId); }
			set { SetColumnValue(Columns.ProcessorId, value); }
		}
		  
		[XmlAttribute("Method")]
		[Bindable(true)]
		public string Method 
		{
			get { return GetColumnValue<string>(Columns.Method); }
			set { SetColumnValue(Columns.Method, value); }
		}
		  
		[XmlAttribute("TransactionType")]
		[Bindable(true)]
		public string TransactionType 
		{
			get { return GetColumnValue<string>(Columns.TransactionType); }
			set { SetColumnValue(Columns.TransactionType, value); }
		}
		  
		[XmlAttribute("MAmount")]
		[Bindable(true)]
		public decimal? MAmount 
		{
			get { return GetColumnValue<decimal?>(Columns.MAmount); }
			set { SetColumnValue(Columns.MAmount, value); }
		}
		  
		[XmlAttribute("MTaxAmount")]
		[Bindable(true)]
		public decimal? MTaxAmount 
		{
			get { return GetColumnValue<decimal?>(Columns.MTaxAmount); }
			set { SetColumnValue(Columns.MTaxAmount, value); }
		}
		  
		[XmlAttribute("MFreightAmount")]
		[Bindable(true)]
		public decimal? MFreightAmount 
		{
			get { return GetColumnValue<decimal?>(Columns.MFreightAmount); }
			set { SetColumnValue(Columns.MFreightAmount, value); }
		}
		  
		[XmlAttribute("Description")]
		[Bindable(true)]
		public string Description 
		{
			get { return GetColumnValue<string>(Columns.Description); }
			set { SetColumnValue(Columns.Description, value); }
		}
		  
		[XmlAttribute("IDupeSeconds")]
		[Bindable(true)]
		public int? IDupeSeconds 
		{
			get { return GetColumnValue<int?>(Columns.IDupeSeconds); }
			set { SetColumnValue(Columns.IDupeSeconds, value); }
		}
		  
		[XmlAttribute("IResponseCode")]
		[Bindable(true)]
		public int? IResponseCode 
		{
			get { return GetColumnValue<int?>(Columns.IResponseCode); }
			set { SetColumnValue(Columns.IResponseCode, value); }
		}
		  
		[XmlAttribute("ResponseSubcode")]
		[Bindable(true)]
		public string ResponseSubcode 
		{
			get { return GetColumnValue<string>(Columns.ResponseSubcode); }
			set { SetColumnValue(Columns.ResponseSubcode, value); }
		}
		  
		[XmlAttribute("IResponseReasonCode")]
		[Bindable(true)]
		public int? IResponseReasonCode 
		{
			get { return GetColumnValue<int?>(Columns.IResponseReasonCode); }
			set { SetColumnValue(Columns.IResponseReasonCode, value); }
		}
		  
		[XmlAttribute("BMd5Match")]
		[Bindable(true)]
		public bool? BMd5Match 
		{
			get { return GetColumnValue<bool?>(Columns.BMd5Match); }
			set { SetColumnValue(Columns.BMd5Match, value); }
		}
		  
		[XmlAttribute("ResponseReasonText")]
		[Bindable(true)]
		public string ResponseReasonText 
		{
			get { return GetColumnValue<string>(Columns.ResponseReasonText); }
			set { SetColumnValue(Columns.ResponseReasonText, value); }
		}
		  
		[XmlAttribute("ApprovalCode")]
		[Bindable(true)]
		public string ApprovalCode 
		{
			get { return GetColumnValue<string>(Columns.ApprovalCode); }
			set { SetColumnValue(Columns.ApprovalCode, value); }
		}
		  
		[XmlAttribute("AVSResultCode")]
		[Bindable(true)]
		public string AVSResultCode 
		{
			get { return GetColumnValue<string>(Columns.AVSResultCode); }
			set { SetColumnValue(Columns.AVSResultCode, value); }
		}
		  
		[XmlAttribute("CardCodeResponseCode")]
		[Bindable(true)]
		public string CardCodeResponseCode 
		{
			get { return GetColumnValue<string>(Columns.CardCodeResponseCode); }
			set { SetColumnValue(Columns.CardCodeResponseCode, value); }
		}
		  
		[XmlAttribute("Email")]
		[Bindable(true)]
		public string Email 
		{
			get { return GetColumnValue<string>(Columns.Email); }
			set { SetColumnValue(Columns.Email, value); }
		}
		  
		[XmlAttribute("FirstName")]
		[Bindable(true)]
		public string FirstName 
		{
			get { return GetColumnValue<string>(Columns.FirstName); }
			set { SetColumnValue(Columns.FirstName, value); }
		}
		  
		[XmlAttribute("LastName")]
		[Bindable(true)]
		public string LastName 
		{
			get { return GetColumnValue<string>(Columns.LastName); }
			set { SetColumnValue(Columns.LastName, value); }
		}
		  
		[XmlAttribute("NameOnCard")]
		[Bindable(true)]
		public string NameOnCard 
		{
			get { return GetColumnValue<string>(Columns.NameOnCard); }
			set { SetColumnValue(Columns.NameOnCard, value); }
		}
		  
		[XmlAttribute("Company")]
		[Bindable(true)]
		public string Company 
		{
			get { return GetColumnValue<string>(Columns.Company); }
			set { SetColumnValue(Columns.Company, value); }
		}
		  
		[XmlAttribute("BillingAddress")]
		[Bindable(true)]
		public string BillingAddress 
		{
			get { return GetColumnValue<string>(Columns.BillingAddress); }
			set { SetColumnValue(Columns.BillingAddress, value); }
		}
		  
		[XmlAttribute("City")]
		[Bindable(true)]
		public string City 
		{
			get { return GetColumnValue<string>(Columns.City); }
			set { SetColumnValue(Columns.City, value); }
		}
		  
		[XmlAttribute("State")]
		[Bindable(true)]
		public string State 
		{
			get { return GetColumnValue<string>(Columns.State); }
			set { SetColumnValue(Columns.State, value); }
		}
		  
		[XmlAttribute("Zip")]
		[Bindable(true)]
		public string Zip 
		{
			get { return GetColumnValue<string>(Columns.Zip); }
			set { SetColumnValue(Columns.Zip, value); }
		}
		  
		[XmlAttribute("Country")]
		[Bindable(true)]
		public string Country 
		{
			get { return GetColumnValue<string>(Columns.Country); }
			set { SetColumnValue(Columns.Country, value); }
		}
		  
		[XmlAttribute("Phone")]
		[Bindable(true)]
		public string Phone 
		{
			get { return GetColumnValue<string>(Columns.Phone); }
			set { SetColumnValue(Columns.Phone, value); }
		}
		  
		[XmlAttribute("IpAddress")]
		[Bindable(true)]
		public string IpAddress 
		{
			get { return GetColumnValue<string>(Columns.IpAddress); }
			set { SetColumnValue(Columns.IpAddress, value); }
		}
		  
		[XmlAttribute("ShipToFirstName")]
		[Bindable(true)]
		public string ShipToFirstName 
		{
			get { return GetColumnValue<string>(Columns.ShipToFirstName); }
			set { SetColumnValue(Columns.ShipToFirstName, value); }
		}
		  
		[XmlAttribute("ShipToLastName")]
		[Bindable(true)]
		public string ShipToLastName 
		{
			get { return GetColumnValue<string>(Columns.ShipToLastName); }
			set { SetColumnValue(Columns.ShipToLastName, value); }
		}
		  
		[XmlAttribute("ShipToCompany")]
		[Bindable(true)]
		public string ShipToCompany 
		{
			get { return GetColumnValue<string>(Columns.ShipToCompany); }
			set { SetColumnValue(Columns.ShipToCompany, value); }
		}
		  
		[XmlAttribute("ShipToAddress")]
		[Bindable(true)]
		public string ShipToAddress 
		{
			get { return GetColumnValue<string>(Columns.ShipToAddress); }
			set { SetColumnValue(Columns.ShipToAddress, value); }
		}
		  
		[XmlAttribute("ShipToCity")]
		[Bindable(true)]
		public string ShipToCity 
		{
			get { return GetColumnValue<string>(Columns.ShipToCity); }
			set { SetColumnValue(Columns.ShipToCity, value); }
		}
		  
		[XmlAttribute("ShipToState")]
		[Bindable(true)]
		public string ShipToState 
		{
			get { return GetColumnValue<string>(Columns.ShipToState); }
			set { SetColumnValue(Columns.ShipToState, value); }
		}
		  
		[XmlAttribute("ShipToZip")]
		[Bindable(true)]
		public string ShipToZip 
		{
			get { return GetColumnValue<string>(Columns.ShipToZip); }
			set { SetColumnValue(Columns.ShipToZip, value); }
		}
		  
		[XmlAttribute("ShipToCountry")]
		[Bindable(true)]
		public string ShipToCountry 
		{
			get { return GetColumnValue<string>(Columns.ShipToCountry); }
			set { SetColumnValue(Columns.ShipToCountry, value); }
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
		
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a AspnetApplication ActiveRecord object related to this AuthorizeNet
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
		/// Returns a AspnetUser ActiveRecord object related to this AuthorizeNet
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
		/// Returns a Invoice ActiveRecord object related to this AuthorizeNet
		/// 
		/// </summary>
		private Wcss.Invoice Invoice
		{
			get { return Wcss.Invoice.FetchByID(this.TInvoiceId); }
			set { SetColumnValue("TInvoiceId", value.Id); }
		}
        //set up an alternate mechanism to avoid a database call
		private Wcss.Invoice _invoicerecord = null;
		
		public Wcss.Invoice InvoiceRecord
		{
		    get
            {
                if (_invoicerecord == null)
                {
                    _invoicerecord = new Wcss.Invoice();
                    _invoicerecord.CopyFrom(this.Invoice);
                }
                return _invoicerecord;
            }
            set
            {
                if(value != null && _invoicerecord == null)
			        _invoicerecord = new Wcss.Invoice();
                
                SetColumnValue("TInvoiceId", value.Id);
                _invoicerecord.CopyFrom(value);                
            }
		}
		
		
		#endregion
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(string varInvoiceNumber,bool? varBAuthorized,int? varTInvoiceId,Guid varUserId,int? varCustomerId,string varProcessorId,string varMethod,string varTransactionType,decimal? varMAmount,decimal? varMTaxAmount,decimal? varMFreightAmount,string varDescription,int? varIDupeSeconds,int? varIResponseCode,string varResponseSubcode,int? varIResponseReasonCode,bool? varBMd5Match,string varResponseReasonText,string varApprovalCode,string varAVSResultCode,string varCardCodeResponseCode,string varEmail,string varFirstName,string varLastName,string varNameOnCard,string varCompany,string varBillingAddress,string varCity,string varState,string varZip,string varCountry,string varPhone,string varIpAddress,string varShipToFirstName,string varShipToLastName,string varShipToCompany,string varShipToAddress,string varShipToCity,string varShipToState,string varShipToZip,string varShipToCountry,DateTime varDtStamp,Guid varApplicationId)
		{
			AuthorizeNet item = new AuthorizeNet();
			
			item.InvoiceNumber = varInvoiceNumber;
			
			item.BAuthorized = varBAuthorized;
			
			item.TInvoiceId = varTInvoiceId;
			
			item.UserId = varUserId;
			
			item.CustomerId = varCustomerId;
			
			item.ProcessorId = varProcessorId;
			
			item.Method = varMethod;
			
			item.TransactionType = varTransactionType;
			
			item.MAmount = varMAmount;
			
			item.MTaxAmount = varMTaxAmount;
			
			item.MFreightAmount = varMFreightAmount;
			
			item.Description = varDescription;
			
			item.IDupeSeconds = varIDupeSeconds;
			
			item.IResponseCode = varIResponseCode;
			
			item.ResponseSubcode = varResponseSubcode;
			
			item.IResponseReasonCode = varIResponseReasonCode;
			
			item.BMd5Match = varBMd5Match;
			
			item.ResponseReasonText = varResponseReasonText;
			
			item.ApprovalCode = varApprovalCode;
			
			item.AVSResultCode = varAVSResultCode;
			
			item.CardCodeResponseCode = varCardCodeResponseCode;
			
			item.Email = varEmail;
			
			item.FirstName = varFirstName;
			
			item.LastName = varLastName;
			
			item.NameOnCard = varNameOnCard;
			
			item.Company = varCompany;
			
			item.BillingAddress = varBillingAddress;
			
			item.City = varCity;
			
			item.State = varState;
			
			item.Zip = varZip;
			
			item.Country = varCountry;
			
			item.Phone = varPhone;
			
			item.IpAddress = varIpAddress;
			
			item.ShipToFirstName = varShipToFirstName;
			
			item.ShipToLastName = varShipToLastName;
			
			item.ShipToCompany = varShipToCompany;
			
			item.ShipToAddress = varShipToAddress;
			
			item.ShipToCity = varShipToCity;
			
			item.ShipToState = varShipToState;
			
			item.ShipToZip = varShipToZip;
			
			item.ShipToCountry = varShipToCountry;
			
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
		public static void Update(int varId,string varInvoiceNumber,bool? varBAuthorized,int? varTInvoiceId,Guid varUserId,int? varCustomerId,string varProcessorId,string varMethod,string varTransactionType,decimal? varMAmount,decimal? varMTaxAmount,decimal? varMFreightAmount,string varDescription,int? varIDupeSeconds,int? varIResponseCode,string varResponseSubcode,int? varIResponseReasonCode,bool? varBMd5Match,string varResponseReasonText,string varApprovalCode,string varAVSResultCode,string varCardCodeResponseCode,string varEmail,string varFirstName,string varLastName,string varNameOnCard,string varCompany,string varBillingAddress,string varCity,string varState,string varZip,string varCountry,string varPhone,string varIpAddress,string varShipToFirstName,string varShipToLastName,string varShipToCompany,string varShipToAddress,string varShipToCity,string varShipToState,string varShipToZip,string varShipToCountry,DateTime varDtStamp,Guid varApplicationId)
		{
			AuthorizeNet item = new AuthorizeNet();
			
				item.Id = varId;
			
				item.InvoiceNumber = varInvoiceNumber;
			
				item.BAuthorized = varBAuthorized;
			
				item.TInvoiceId = varTInvoiceId;
			
				item.UserId = varUserId;
			
				item.CustomerId = varCustomerId;
			
				item.ProcessorId = varProcessorId;
			
				item.Method = varMethod;
			
				item.TransactionType = varTransactionType;
			
				item.MAmount = varMAmount;
			
				item.MTaxAmount = varMTaxAmount;
			
				item.MFreightAmount = varMFreightAmount;
			
				item.Description = varDescription;
			
				item.IDupeSeconds = varIDupeSeconds;
			
				item.IResponseCode = varIResponseCode;
			
				item.ResponseSubcode = varResponseSubcode;
			
				item.IResponseReasonCode = varIResponseReasonCode;
			
				item.BMd5Match = varBMd5Match;
			
				item.ResponseReasonText = varResponseReasonText;
			
				item.ApprovalCode = varApprovalCode;
			
				item.AVSResultCode = varAVSResultCode;
			
				item.CardCodeResponseCode = varCardCodeResponseCode;
			
				item.Email = varEmail;
			
				item.FirstName = varFirstName;
			
				item.LastName = varLastName;
			
				item.NameOnCard = varNameOnCard;
			
				item.Company = varCompany;
			
				item.BillingAddress = varBillingAddress;
			
				item.City = varCity;
			
				item.State = varState;
			
				item.Zip = varZip;
			
				item.Country = varCountry;
			
				item.Phone = varPhone;
			
				item.IpAddress = varIpAddress;
			
				item.ShipToFirstName = varShipToFirstName;
			
				item.ShipToLastName = varShipToLastName;
			
				item.ShipToCompany = varShipToCompany;
			
				item.ShipToAddress = varShipToAddress;
			
				item.ShipToCity = varShipToCity;
			
				item.ShipToState = varShipToState;
			
				item.ShipToZip = varShipToZip;
			
				item.ShipToCountry = varShipToCountry;
			
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
        
        
        
        public static TableSchema.TableColumn InvoiceNumberColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn BAuthorizedColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn TInvoiceIdColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn UserIdColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn CustomerIdColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn ProcessorIdColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn MethodColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn TransactionTypeColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn MAmountColumn
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        public static TableSchema.TableColumn MTaxAmountColumn
        {
            get { return Schema.Columns[10]; }
        }
        
        
        
        public static TableSchema.TableColumn MFreightAmountColumn
        {
            get { return Schema.Columns[11]; }
        }
        
        
        
        public static TableSchema.TableColumn DescriptionColumn
        {
            get { return Schema.Columns[12]; }
        }
        
        
        
        public static TableSchema.TableColumn IDupeSecondsColumn
        {
            get { return Schema.Columns[13]; }
        }
        
        
        
        public static TableSchema.TableColumn IResponseCodeColumn
        {
            get { return Schema.Columns[14]; }
        }
        
        
        
        public static TableSchema.TableColumn ResponseSubcodeColumn
        {
            get { return Schema.Columns[15]; }
        }
        
        
        
        public static TableSchema.TableColumn IResponseReasonCodeColumn
        {
            get { return Schema.Columns[16]; }
        }
        
        
        
        public static TableSchema.TableColumn BMd5MatchColumn
        {
            get { return Schema.Columns[17]; }
        }
        
        
        
        public static TableSchema.TableColumn ResponseReasonTextColumn
        {
            get { return Schema.Columns[18]; }
        }
        
        
        
        public static TableSchema.TableColumn ApprovalCodeColumn
        {
            get { return Schema.Columns[19]; }
        }
        
        
        
        public static TableSchema.TableColumn AVSResultCodeColumn
        {
            get { return Schema.Columns[20]; }
        }
        
        
        
        public static TableSchema.TableColumn CardCodeResponseCodeColumn
        {
            get { return Schema.Columns[21]; }
        }
        
        
        
        public static TableSchema.TableColumn EmailColumn
        {
            get { return Schema.Columns[22]; }
        }
        
        
        
        public static TableSchema.TableColumn FirstNameColumn
        {
            get { return Schema.Columns[23]; }
        }
        
        
        
        public static TableSchema.TableColumn LastNameColumn
        {
            get { return Schema.Columns[24]; }
        }
        
        
        
        public static TableSchema.TableColumn NameOnCardColumn
        {
            get { return Schema.Columns[25]; }
        }
        
        
        
        public static TableSchema.TableColumn CompanyColumn
        {
            get { return Schema.Columns[26]; }
        }
        
        
        
        public static TableSchema.TableColumn BillingAddressColumn
        {
            get { return Schema.Columns[27]; }
        }
        
        
        
        public static TableSchema.TableColumn CityColumn
        {
            get { return Schema.Columns[28]; }
        }
        
        
        
        public static TableSchema.TableColumn StateColumn
        {
            get { return Schema.Columns[29]; }
        }
        
        
        
        public static TableSchema.TableColumn ZipColumn
        {
            get { return Schema.Columns[30]; }
        }
        
        
        
        public static TableSchema.TableColumn CountryColumn
        {
            get { return Schema.Columns[31]; }
        }
        
        
        
        public static TableSchema.TableColumn PhoneColumn
        {
            get { return Schema.Columns[32]; }
        }
        
        
        
        public static TableSchema.TableColumn IpAddressColumn
        {
            get { return Schema.Columns[33]; }
        }
        
        
        
        public static TableSchema.TableColumn ShipToFirstNameColumn
        {
            get { return Schema.Columns[34]; }
        }
        
        
        
        public static TableSchema.TableColumn ShipToLastNameColumn
        {
            get { return Schema.Columns[35]; }
        }
        
        
        
        public static TableSchema.TableColumn ShipToCompanyColumn
        {
            get { return Schema.Columns[36]; }
        }
        
        
        
        public static TableSchema.TableColumn ShipToAddressColumn
        {
            get { return Schema.Columns[37]; }
        }
        
        
        
        public static TableSchema.TableColumn ShipToCityColumn
        {
            get { return Schema.Columns[38]; }
        }
        
        
        
        public static TableSchema.TableColumn ShipToStateColumn
        {
            get { return Schema.Columns[39]; }
        }
        
        
        
        public static TableSchema.TableColumn ShipToZipColumn
        {
            get { return Schema.Columns[40]; }
        }
        
        
        
        public static TableSchema.TableColumn ShipToCountryColumn
        {
            get { return Schema.Columns[41]; }
        }
        
        
        
        public static TableSchema.TableColumn DtStampColumn
        {
            get { return Schema.Columns[42]; }
        }
        
        
        
        public static TableSchema.TableColumn ApplicationIdColumn
        {
            get { return Schema.Columns[43]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Id = @"Id";
			 public static string InvoiceNumber = @"InvoiceNumber";
			 public static string BAuthorized = @"bAuthorized";
			 public static string TInvoiceId = @"TInvoiceId";
			 public static string UserId = @"UserId";
			 public static string CustomerId = @"CustomerId";
			 public static string ProcessorId = @"ProcessorId";
			 public static string Method = @"Method";
			 public static string TransactionType = @"TransactionType";
			 public static string MAmount = @"mAmount";
			 public static string MTaxAmount = @"mTaxAmount";
			 public static string MFreightAmount = @"mFreightAmount";
			 public static string Description = @"Description";
			 public static string IDupeSeconds = @"iDupeSeconds";
			 public static string IResponseCode = @"iResponseCode";
			 public static string ResponseSubcode = @"ResponseSubcode";
			 public static string IResponseReasonCode = @"iResponseReasonCode";
			 public static string BMd5Match = @"bMd5Match";
			 public static string ResponseReasonText = @"ResponseReasonText";
			 public static string ApprovalCode = @"ApprovalCode";
			 public static string AVSResultCode = @"AVSResultCode";
			 public static string CardCodeResponseCode = @"CardCodeResponseCode";
			 public static string Email = @"Email";
			 public static string FirstName = @"FirstName";
			 public static string LastName = @"LastName";
			 public static string NameOnCard = @"NameOnCard";
			 public static string Company = @"Company";
			 public static string BillingAddress = @"BillingAddress";
			 public static string City = @"City";
			 public static string State = @"State";
			 public static string Zip = @"Zip";
			 public static string Country = @"Country";
			 public static string Phone = @"Phone";
			 public static string IpAddress = @"IpAddress";
			 public static string ShipToFirstName = @"ShipToFirstName";
			 public static string ShipToLastName = @"ShipToLastName";
			 public static string ShipToCompany = @"ShipToCompany";
			 public static string ShipToAddress = @"ShipToAddress";
			 public static string ShipToCity = @"ShipToCity";
			 public static string ShipToState = @"ShipToState";
			 public static string ShipToZip = @"ShipToZip";
			 public static string ShipToCountry = @"ShipToCountry";
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
