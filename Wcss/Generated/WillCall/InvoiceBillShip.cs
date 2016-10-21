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
	/// Strongly-typed collection for the InvoiceBillShip class.
	/// </summary>
    [Serializable]
	public partial class InvoiceBillShipCollection : ActiveList<InvoiceBillShip, InvoiceBillShipCollection>
	{	   
		public InvoiceBillShipCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>InvoiceBillShipCollection</returns>
		public InvoiceBillShipCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                InvoiceBillShip o = this[i];
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
	/// This is an ActiveRecord class which wraps the InvoiceBillShip table.
	/// </summary>
	[Serializable]
	public partial class InvoiceBillShip : ActiveRecord<InvoiceBillShip>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public InvoiceBillShip()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public InvoiceBillShip(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public InvoiceBillShip(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public InvoiceBillShip(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("InvoiceBillShip", TableType.Table, DataService.GetInstance("WillCall"));
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
				
				TableSchema.TableColumn colvarTInvoiceId = new TableSchema.TableColumn(schema);
				colvarTInvoiceId.ColumnName = "tInvoiceId";
				colvarTInvoiceId.DataType = DbType.Int32;
				colvarTInvoiceId.MaxLength = 0;
				colvarTInvoiceId.AutoIncrement = false;
				colvarTInvoiceId.IsNullable = false;
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
				colvarUserId.IsNullable = true;
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
				colvarCustomerId.IsNullable = false;
				colvarCustomerId.IsPrimaryKey = false;
				colvarCustomerId.IsForeignKey = false;
				colvarCustomerId.IsReadOnly = false;
				colvarCustomerId.DefaultSetting = @"";
				colvarCustomerId.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCustomerId);
				
				TableSchema.TableColumn colvarBlCompany = new TableSchema.TableColumn(schema);
				colvarBlCompany.ColumnName = "blCompany";
				colvarBlCompany.DataType = DbType.AnsiString;
				colvarBlCompany.MaxLength = 100;
				colvarBlCompany.AutoIncrement = false;
				colvarBlCompany.IsNullable = false;
				colvarBlCompany.IsPrimaryKey = false;
				colvarBlCompany.IsForeignKey = false;
				colvarBlCompany.IsReadOnly = false;
				colvarBlCompany.DefaultSetting = @"";
				colvarBlCompany.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBlCompany);
				
				TableSchema.TableColumn colvarBlFirstName = new TableSchema.TableColumn(schema);
				colvarBlFirstName.ColumnName = "blFirstName";
				colvarBlFirstName.DataType = DbType.AnsiString;
				colvarBlFirstName.MaxLength = 50;
				colvarBlFirstName.AutoIncrement = false;
				colvarBlFirstName.IsNullable = false;
				colvarBlFirstName.IsPrimaryKey = false;
				colvarBlFirstName.IsForeignKey = false;
				colvarBlFirstName.IsReadOnly = false;
				colvarBlFirstName.DefaultSetting = @"";
				colvarBlFirstName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBlFirstName);
				
				TableSchema.TableColumn colvarBlLastName = new TableSchema.TableColumn(schema);
				colvarBlLastName.ColumnName = "blLastName";
				colvarBlLastName.DataType = DbType.AnsiString;
				colvarBlLastName.MaxLength = 50;
				colvarBlLastName.AutoIncrement = false;
				colvarBlLastName.IsNullable = false;
				colvarBlLastName.IsPrimaryKey = false;
				colvarBlLastName.IsForeignKey = false;
				colvarBlLastName.IsReadOnly = false;
				colvarBlLastName.DefaultSetting = @"";
				colvarBlLastName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBlLastName);
				
				TableSchema.TableColumn colvarBlAddress1 = new TableSchema.TableColumn(schema);
				colvarBlAddress1.ColumnName = "blAddress1";
				colvarBlAddress1.DataType = DbType.AnsiString;
				colvarBlAddress1.MaxLength = 60;
				colvarBlAddress1.AutoIncrement = false;
				colvarBlAddress1.IsNullable = false;
				colvarBlAddress1.IsPrimaryKey = false;
				colvarBlAddress1.IsForeignKey = false;
				colvarBlAddress1.IsReadOnly = false;
				colvarBlAddress1.DefaultSetting = @"";
				colvarBlAddress1.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBlAddress1);
				
				TableSchema.TableColumn colvarBlAddress2 = new TableSchema.TableColumn(schema);
				colvarBlAddress2.ColumnName = "blAddress2";
				colvarBlAddress2.DataType = DbType.AnsiString;
				colvarBlAddress2.MaxLength = 60;
				colvarBlAddress2.AutoIncrement = false;
				colvarBlAddress2.IsNullable = true;
				colvarBlAddress2.IsPrimaryKey = false;
				colvarBlAddress2.IsForeignKey = false;
				colvarBlAddress2.IsReadOnly = false;
				colvarBlAddress2.DefaultSetting = @"";
				colvarBlAddress2.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBlAddress2);
				
				TableSchema.TableColumn colvarBlCity = new TableSchema.TableColumn(schema);
				colvarBlCity.ColumnName = "blCity";
				colvarBlCity.DataType = DbType.AnsiString;
				colvarBlCity.MaxLength = 40;
				colvarBlCity.AutoIncrement = false;
				colvarBlCity.IsNullable = false;
				colvarBlCity.IsPrimaryKey = false;
				colvarBlCity.IsForeignKey = false;
				colvarBlCity.IsReadOnly = false;
				colvarBlCity.DefaultSetting = @"";
				colvarBlCity.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBlCity);
				
				TableSchema.TableColumn colvarBlStateProvince = new TableSchema.TableColumn(schema);
				colvarBlStateProvince.ColumnName = "blStateProvince";
				colvarBlStateProvince.DataType = DbType.AnsiString;
				colvarBlStateProvince.MaxLength = 40;
				colvarBlStateProvince.AutoIncrement = false;
				colvarBlStateProvince.IsNullable = false;
				colvarBlStateProvince.IsPrimaryKey = false;
				colvarBlStateProvince.IsForeignKey = false;
				colvarBlStateProvince.IsReadOnly = false;
				colvarBlStateProvince.DefaultSetting = @"";
				colvarBlStateProvince.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBlStateProvince);
				
				TableSchema.TableColumn colvarBlPostalCode = new TableSchema.TableColumn(schema);
				colvarBlPostalCode.ColumnName = "blPostalCode";
				colvarBlPostalCode.DataType = DbType.AnsiString;
				colvarBlPostalCode.MaxLength = 20;
				colvarBlPostalCode.AutoIncrement = false;
				colvarBlPostalCode.IsNullable = false;
				colvarBlPostalCode.IsPrimaryKey = false;
				colvarBlPostalCode.IsForeignKey = false;
				colvarBlPostalCode.IsReadOnly = false;
				colvarBlPostalCode.DefaultSetting = @"";
				colvarBlPostalCode.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBlPostalCode);
				
				TableSchema.TableColumn colvarBlCountry = new TableSchema.TableColumn(schema);
				colvarBlCountry.ColumnName = "blCountry";
				colvarBlCountry.DataType = DbType.AnsiString;
				colvarBlCountry.MaxLength = 60;
				colvarBlCountry.AutoIncrement = false;
				colvarBlCountry.IsNullable = false;
				colvarBlCountry.IsPrimaryKey = false;
				colvarBlCountry.IsForeignKey = false;
				colvarBlCountry.IsReadOnly = false;
				colvarBlCountry.DefaultSetting = @"";
				colvarBlCountry.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBlCountry);
				
				TableSchema.TableColumn colvarBlPhone = new TableSchema.TableColumn(schema);
				colvarBlPhone.ColumnName = "blPhone";
				colvarBlPhone.DataType = DbType.AnsiString;
				colvarBlPhone.MaxLength = 25;
				colvarBlPhone.AutoIncrement = false;
				colvarBlPhone.IsNullable = false;
				colvarBlPhone.IsPrimaryKey = false;
				colvarBlPhone.IsForeignKey = false;
				colvarBlPhone.IsReadOnly = false;
				colvarBlPhone.DefaultSetting = @"";
				colvarBlPhone.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBlPhone);
				
				TableSchema.TableColumn colvarBSameAsBilling = new TableSchema.TableColumn(schema);
				colvarBSameAsBilling.ColumnName = "bSameAsBilling";
				colvarBSameAsBilling.DataType = DbType.Boolean;
				colvarBSameAsBilling.MaxLength = 0;
				colvarBSameAsBilling.AutoIncrement = false;
				colvarBSameAsBilling.IsNullable = false;
				colvarBSameAsBilling.IsPrimaryKey = false;
				colvarBSameAsBilling.IsForeignKey = false;
				colvarBSameAsBilling.IsReadOnly = false;
				
						colvarBSameAsBilling.DefaultSetting = @"((1))";
				colvarBSameAsBilling.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBSameAsBilling);
				
				TableSchema.TableColumn colvarCompanyName = new TableSchema.TableColumn(schema);
				colvarCompanyName.ColumnName = "CompanyName";
				colvarCompanyName.DataType = DbType.AnsiString;
				colvarCompanyName.MaxLength = 100;
				colvarCompanyName.AutoIncrement = false;
				colvarCompanyName.IsNullable = true;
				colvarCompanyName.IsPrimaryKey = false;
				colvarCompanyName.IsForeignKey = false;
				colvarCompanyName.IsReadOnly = false;
				colvarCompanyName.DefaultSetting = @"";
				colvarCompanyName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCompanyName);
				
				TableSchema.TableColumn colvarFirstName = new TableSchema.TableColumn(schema);
				colvarFirstName.ColumnName = "FirstName";
				colvarFirstName.DataType = DbType.AnsiString;
				colvarFirstName.MaxLength = 50;
				colvarFirstName.AutoIncrement = false;
				colvarFirstName.IsNullable = false;
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
				colvarLastName.IsNullable = false;
				colvarLastName.IsPrimaryKey = false;
				colvarLastName.IsForeignKey = false;
				colvarLastName.IsReadOnly = false;
				colvarLastName.DefaultSetting = @"";
				colvarLastName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarLastName);
				
				TableSchema.TableColumn colvarAddress1 = new TableSchema.TableColumn(schema);
				colvarAddress1.ColumnName = "Address1";
				colvarAddress1.DataType = DbType.AnsiString;
				colvarAddress1.MaxLength = 60;
				colvarAddress1.AutoIncrement = false;
				colvarAddress1.IsNullable = false;
				colvarAddress1.IsPrimaryKey = false;
				colvarAddress1.IsForeignKey = false;
				colvarAddress1.IsReadOnly = false;
				colvarAddress1.DefaultSetting = @"";
				colvarAddress1.ForeignKeyTableName = "";
				schema.Columns.Add(colvarAddress1);
				
				TableSchema.TableColumn colvarAddress2 = new TableSchema.TableColumn(schema);
				colvarAddress2.ColumnName = "Address2";
				colvarAddress2.DataType = DbType.AnsiString;
				colvarAddress2.MaxLength = 60;
				colvarAddress2.AutoIncrement = false;
				colvarAddress2.IsNullable = true;
				colvarAddress2.IsPrimaryKey = false;
				colvarAddress2.IsForeignKey = false;
				colvarAddress2.IsReadOnly = false;
				colvarAddress2.DefaultSetting = @"";
				colvarAddress2.ForeignKeyTableName = "";
				schema.Columns.Add(colvarAddress2);
				
				TableSchema.TableColumn colvarCity = new TableSchema.TableColumn(schema);
				colvarCity.ColumnName = "City";
				colvarCity.DataType = DbType.AnsiString;
				colvarCity.MaxLength = 40;
				colvarCity.AutoIncrement = false;
				colvarCity.IsNullable = false;
				colvarCity.IsPrimaryKey = false;
				colvarCity.IsForeignKey = false;
				colvarCity.IsReadOnly = false;
				colvarCity.DefaultSetting = @"";
				colvarCity.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCity);
				
				TableSchema.TableColumn colvarStateProvince = new TableSchema.TableColumn(schema);
				colvarStateProvince.ColumnName = "StateProvince";
				colvarStateProvince.DataType = DbType.AnsiString;
				colvarStateProvince.MaxLength = 40;
				colvarStateProvince.AutoIncrement = false;
				colvarStateProvince.IsNullable = false;
				colvarStateProvince.IsPrimaryKey = false;
				colvarStateProvince.IsForeignKey = false;
				colvarStateProvince.IsReadOnly = false;
				colvarStateProvince.DefaultSetting = @"";
				colvarStateProvince.ForeignKeyTableName = "";
				schema.Columns.Add(colvarStateProvince);
				
				TableSchema.TableColumn colvarPostalCode = new TableSchema.TableColumn(schema);
				colvarPostalCode.ColumnName = "PostalCode";
				colvarPostalCode.DataType = DbType.AnsiString;
				colvarPostalCode.MaxLength = 20;
				colvarPostalCode.AutoIncrement = false;
				colvarPostalCode.IsNullable = false;
				colvarPostalCode.IsPrimaryKey = false;
				colvarPostalCode.IsForeignKey = false;
				colvarPostalCode.IsReadOnly = false;
				colvarPostalCode.DefaultSetting = @"";
				colvarPostalCode.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPostalCode);
				
				TableSchema.TableColumn colvarCountry = new TableSchema.TableColumn(schema);
				colvarCountry.ColumnName = "Country";
				colvarCountry.DataType = DbType.AnsiString;
				colvarCountry.MaxLength = 60;
				colvarCountry.AutoIncrement = false;
				colvarCountry.IsNullable = false;
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
				colvarPhone.IsNullable = false;
				colvarPhone.IsPrimaryKey = false;
				colvarPhone.IsForeignKey = false;
				colvarPhone.IsReadOnly = false;
				colvarPhone.DefaultSetting = @"";
				colvarPhone.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPhone);
				
				TableSchema.TableColumn colvarShipMessage = new TableSchema.TableColumn(schema);
				colvarShipMessage.ColumnName = "ShipMessage";
				colvarShipMessage.DataType = DbType.AnsiString;
				colvarShipMessage.MaxLength = 1000;
				colvarShipMessage.AutoIncrement = false;
				colvarShipMessage.IsNullable = true;
				colvarShipMessage.IsPrimaryKey = false;
				colvarShipMessage.IsForeignKey = false;
				colvarShipMessage.IsReadOnly = false;
				colvarShipMessage.DefaultSetting = @"";
				colvarShipMessage.ForeignKeyTableName = "";
				schema.Columns.Add(colvarShipMessage);
				
				TableSchema.TableColumn colvarDtShipped = new TableSchema.TableColumn(schema);
				colvarDtShipped.ColumnName = "dtShipped";
				colvarDtShipped.DataType = DbType.DateTime;
				colvarDtShipped.MaxLength = 0;
				colvarDtShipped.AutoIncrement = false;
				colvarDtShipped.IsNullable = true;
				colvarDtShipped.IsPrimaryKey = false;
				colvarDtShipped.IsForeignKey = false;
				colvarDtShipped.IsReadOnly = false;
				colvarDtShipped.DefaultSetting = @"";
				colvarDtShipped.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDtShipped);
				
				TableSchema.TableColumn colvarTrackingInformation = new TableSchema.TableColumn(schema);
				colvarTrackingInformation.ColumnName = "TrackingInformation";
				colvarTrackingInformation.DataType = DbType.AnsiString;
				colvarTrackingInformation.MaxLength = 500;
				colvarTrackingInformation.AutoIncrement = false;
				colvarTrackingInformation.IsNullable = true;
				colvarTrackingInformation.IsPrimaryKey = false;
				colvarTrackingInformation.IsForeignKey = false;
				colvarTrackingInformation.IsReadOnly = false;
				colvarTrackingInformation.DefaultSetting = @"";
				colvarTrackingInformation.ForeignKeyTableName = "";
				schema.Columns.Add(colvarTrackingInformation);
				
				TableSchema.TableColumn colvarReferenceNumber = new TableSchema.TableColumn(schema);
				colvarReferenceNumber.ColumnName = "ReferenceNumber";
				colvarReferenceNumber.DataType = DbType.Guid;
				colvarReferenceNumber.MaxLength = 0;
				colvarReferenceNumber.AutoIncrement = false;
				colvarReferenceNumber.IsNullable = true;
				colvarReferenceNumber.IsPrimaryKey = false;
				colvarReferenceNumber.IsForeignKey = false;
				colvarReferenceNumber.IsReadOnly = false;
				
						colvarReferenceNumber.DefaultSetting = @"(newid())";
				colvarReferenceNumber.ForeignKeyTableName = "";
				schema.Columns.Add(colvarReferenceNumber);
				
				TableSchema.TableColumn colvarMActualShipping = new TableSchema.TableColumn(schema);
				colvarMActualShipping.ColumnName = "mActualShipping";
				colvarMActualShipping.DataType = DbType.Currency;
				colvarMActualShipping.MaxLength = 0;
				colvarMActualShipping.AutoIncrement = false;
				colvarMActualShipping.IsNullable = true;
				colvarMActualShipping.IsPrimaryKey = false;
				colvarMActualShipping.IsForeignKey = false;
				colvarMActualShipping.IsReadOnly = false;
				
						colvarMActualShipping.DefaultSetting = @"((0))";
				colvarMActualShipping.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMActualShipping);
				
				TableSchema.TableColumn colvarMHandlingComputed = new TableSchema.TableColumn(schema);
				colvarMHandlingComputed.ColumnName = "mHandlingComputed";
				colvarMHandlingComputed.DataType = DbType.Currency;
				colvarMHandlingComputed.MaxLength = 0;
				colvarMHandlingComputed.AutoIncrement = false;
				colvarMHandlingComputed.IsNullable = true;
				colvarMHandlingComputed.IsPrimaryKey = false;
				colvarMHandlingComputed.IsForeignKey = false;
				colvarMHandlingComputed.IsReadOnly = false;
				
						colvarMHandlingComputed.DefaultSetting = @"((0))";
				colvarMHandlingComputed.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMHandlingComputed);
				
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
				DataService.Providers["WillCall"].AddSchema("InvoiceBillShip",schema);
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
		  
		[XmlAttribute("TInvoiceId")]
		[Bindable(true)]
		public int TInvoiceId 
		{
			get { return GetColumnValue<int>(Columns.TInvoiceId); }
			set { SetColumnValue(Columns.TInvoiceId, value); }
		}
		  
		[XmlAttribute("UserId")]
		[Bindable(true)]
		public Guid? UserId 
		{
			get { return GetColumnValue<Guid?>(Columns.UserId); }
			set { SetColumnValue(Columns.UserId, value); }
		}
		  
		[XmlAttribute("CustomerId")]
		[Bindable(true)]
		public int CustomerId 
		{
			get { return GetColumnValue<int>(Columns.CustomerId); }
			set { SetColumnValue(Columns.CustomerId, value); }
		}
		  
		[XmlAttribute("BlCompany")]
		[Bindable(true)]
		public string BlCompany 
		{
			get { return GetColumnValue<string>(Columns.BlCompany); }
			set { SetColumnValue(Columns.BlCompany, value); }
		}
		  
		[XmlAttribute("BlFirstName")]
		[Bindable(true)]
		public string BlFirstName 
		{
			get { return GetColumnValue<string>(Columns.BlFirstName); }
			set { SetColumnValue(Columns.BlFirstName, value); }
		}
		  
		[XmlAttribute("BlLastName")]
		[Bindable(true)]
		public string BlLastName 
		{
			get { return GetColumnValue<string>(Columns.BlLastName); }
			set { SetColumnValue(Columns.BlLastName, value); }
		}
		  
		[XmlAttribute("BlAddress1")]
		[Bindable(true)]
		public string BlAddress1 
		{
			get { return GetColumnValue<string>(Columns.BlAddress1); }
			set { SetColumnValue(Columns.BlAddress1, value); }
		}
		  
		[XmlAttribute("BlAddress2")]
		[Bindable(true)]
		public string BlAddress2 
		{
			get { return GetColumnValue<string>(Columns.BlAddress2); }
			set { SetColumnValue(Columns.BlAddress2, value); }
		}
		  
		[XmlAttribute("BlCity")]
		[Bindable(true)]
		public string BlCity 
		{
			get { return GetColumnValue<string>(Columns.BlCity); }
			set { SetColumnValue(Columns.BlCity, value); }
		}
		  
		[XmlAttribute("BlStateProvince")]
		[Bindable(true)]
		public string BlStateProvince 
		{
			get { return GetColumnValue<string>(Columns.BlStateProvince); }
			set { SetColumnValue(Columns.BlStateProvince, value); }
		}
		  
		[XmlAttribute("BlPostalCode")]
		[Bindable(true)]
		public string BlPostalCode 
		{
			get { return GetColumnValue<string>(Columns.BlPostalCode); }
			set { SetColumnValue(Columns.BlPostalCode, value); }
		}
		  
		[XmlAttribute("BlCountry")]
		[Bindable(true)]
		public string BlCountry 
		{
			get { return GetColumnValue<string>(Columns.BlCountry); }
			set { SetColumnValue(Columns.BlCountry, value); }
		}
		  
		[XmlAttribute("BlPhone")]
		[Bindable(true)]
		public string BlPhone 
		{
			get { return GetColumnValue<string>(Columns.BlPhone); }
			set { SetColumnValue(Columns.BlPhone, value); }
		}
		  
		[XmlAttribute("BSameAsBilling")]
		[Bindable(true)]
		public bool BSameAsBilling 
		{
			get { return GetColumnValue<bool>(Columns.BSameAsBilling); }
			set { SetColumnValue(Columns.BSameAsBilling, value); }
		}
		  
		[XmlAttribute("CompanyName")]
		[Bindable(true)]
		public string CompanyName 
		{
			get { return GetColumnValue<string>(Columns.CompanyName); }
			set { SetColumnValue(Columns.CompanyName, value); }
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
		  
		[XmlAttribute("Address1")]
		[Bindable(true)]
		public string Address1 
		{
			get { return GetColumnValue<string>(Columns.Address1); }
			set { SetColumnValue(Columns.Address1, value); }
		}
		  
		[XmlAttribute("Address2")]
		[Bindable(true)]
		public string Address2 
		{
			get { return GetColumnValue<string>(Columns.Address2); }
			set { SetColumnValue(Columns.Address2, value); }
		}
		  
		[XmlAttribute("City")]
		[Bindable(true)]
		public string City 
		{
			get { return GetColumnValue<string>(Columns.City); }
			set { SetColumnValue(Columns.City, value); }
		}
		  
		[XmlAttribute("StateProvince")]
		[Bindable(true)]
		public string StateProvince 
		{
			get { return GetColumnValue<string>(Columns.StateProvince); }
			set { SetColumnValue(Columns.StateProvince, value); }
		}
		  
		[XmlAttribute("PostalCode")]
		[Bindable(true)]
		public string PostalCode 
		{
			get { return GetColumnValue<string>(Columns.PostalCode); }
			set { SetColumnValue(Columns.PostalCode, value); }
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
		  
		[XmlAttribute("ShipMessage")]
		[Bindable(true)]
		public string ShipMessage 
		{
			get { return GetColumnValue<string>(Columns.ShipMessage); }
			set { SetColumnValue(Columns.ShipMessage, value); }
		}
		  
		[XmlAttribute("DtShipped")]
		[Bindable(true)]
		public DateTime? DtShipped 
		{
			get { return GetColumnValue<DateTime?>(Columns.DtShipped); }
			set { SetColumnValue(Columns.DtShipped, value); }
		}
		  
		[XmlAttribute("TrackingInformation")]
		[Bindable(true)]
		public string TrackingInformation 
		{
			get { return GetColumnValue<string>(Columns.TrackingInformation); }
			set { SetColumnValue(Columns.TrackingInformation, value); }
		}
		  
		[XmlAttribute("ReferenceNumber")]
		[Bindable(true)]
		public Guid? ReferenceNumber 
		{
			get { return GetColumnValue<Guid?>(Columns.ReferenceNumber); }
			set { SetColumnValue(Columns.ReferenceNumber, value); }
		}
		  
		[XmlAttribute("MActualShipping")]
		[Bindable(true)]
		public decimal? MActualShipping 
		{
			get { return GetColumnValue<decimal?>(Columns.MActualShipping); }
			set { SetColumnValue(Columns.MActualShipping, value); }
		}
		  
		[XmlAttribute("MHandlingComputed")]
		[Bindable(true)]
		public decimal? MHandlingComputed 
		{
			get { return GetColumnValue<decimal?>(Columns.MHandlingComputed); }
			set { SetColumnValue(Columns.MHandlingComputed, value); }
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
		/// Returns a AspnetUser ActiveRecord object related to this InvoiceBillShip
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
		/// Returns a Invoice ActiveRecord object related to this InvoiceBillShip
		/// 
		/// </summary>
		private Wcss.Invoice Invoice
		{
			get { return Wcss.Invoice.FetchByID(this.TInvoiceId); }
			set { SetColumnValue("tInvoiceId", value.Id); }
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
                
                SetColumnValue("tInvoiceId", value.Id);
                _invoicerecord.CopyFrom(value);                
            }
		}
		
		
		#endregion
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(int varTInvoiceId,Guid? varUserId,int varCustomerId,string varBlCompany,string varBlFirstName,string varBlLastName,string varBlAddress1,string varBlAddress2,string varBlCity,string varBlStateProvince,string varBlPostalCode,string varBlCountry,string varBlPhone,bool varBSameAsBilling,string varCompanyName,string varFirstName,string varLastName,string varAddress1,string varAddress2,string varCity,string varStateProvince,string varPostalCode,string varCountry,string varPhone,string varShipMessage,DateTime? varDtShipped,string varTrackingInformation,Guid? varReferenceNumber,decimal? varMActualShipping,decimal? varMHandlingComputed,DateTime varDtStamp)
		{
			InvoiceBillShip item = new InvoiceBillShip();
			
			item.TInvoiceId = varTInvoiceId;
			
			item.UserId = varUserId;
			
			item.CustomerId = varCustomerId;
			
			item.BlCompany = varBlCompany;
			
			item.BlFirstName = varBlFirstName;
			
			item.BlLastName = varBlLastName;
			
			item.BlAddress1 = varBlAddress1;
			
			item.BlAddress2 = varBlAddress2;
			
			item.BlCity = varBlCity;
			
			item.BlStateProvince = varBlStateProvince;
			
			item.BlPostalCode = varBlPostalCode;
			
			item.BlCountry = varBlCountry;
			
			item.BlPhone = varBlPhone;
			
			item.BSameAsBilling = varBSameAsBilling;
			
			item.CompanyName = varCompanyName;
			
			item.FirstName = varFirstName;
			
			item.LastName = varLastName;
			
			item.Address1 = varAddress1;
			
			item.Address2 = varAddress2;
			
			item.City = varCity;
			
			item.StateProvince = varStateProvince;
			
			item.PostalCode = varPostalCode;
			
			item.Country = varCountry;
			
			item.Phone = varPhone;
			
			item.ShipMessage = varShipMessage;
			
			item.DtShipped = varDtShipped;
			
			item.TrackingInformation = varTrackingInformation;
			
			item.ReferenceNumber = varReferenceNumber;
			
			item.MActualShipping = varMActualShipping;
			
			item.MHandlingComputed = varMHandlingComputed;
			
			item.DtStamp = varDtStamp;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varId,int varTInvoiceId,Guid? varUserId,int varCustomerId,string varBlCompany,string varBlFirstName,string varBlLastName,string varBlAddress1,string varBlAddress2,string varBlCity,string varBlStateProvince,string varBlPostalCode,string varBlCountry,string varBlPhone,bool varBSameAsBilling,string varCompanyName,string varFirstName,string varLastName,string varAddress1,string varAddress2,string varCity,string varStateProvince,string varPostalCode,string varCountry,string varPhone,string varShipMessage,DateTime? varDtShipped,string varTrackingInformation,Guid? varReferenceNumber,decimal? varMActualShipping,decimal? varMHandlingComputed,DateTime varDtStamp)
		{
			InvoiceBillShip item = new InvoiceBillShip();
			
				item.Id = varId;
			
				item.TInvoiceId = varTInvoiceId;
			
				item.UserId = varUserId;
			
				item.CustomerId = varCustomerId;
			
				item.BlCompany = varBlCompany;
			
				item.BlFirstName = varBlFirstName;
			
				item.BlLastName = varBlLastName;
			
				item.BlAddress1 = varBlAddress1;
			
				item.BlAddress2 = varBlAddress2;
			
				item.BlCity = varBlCity;
			
				item.BlStateProvince = varBlStateProvince;
			
				item.BlPostalCode = varBlPostalCode;
			
				item.BlCountry = varBlCountry;
			
				item.BlPhone = varBlPhone;
			
				item.BSameAsBilling = varBSameAsBilling;
			
				item.CompanyName = varCompanyName;
			
				item.FirstName = varFirstName;
			
				item.LastName = varLastName;
			
				item.Address1 = varAddress1;
			
				item.Address2 = varAddress2;
			
				item.City = varCity;
			
				item.StateProvince = varStateProvince;
			
				item.PostalCode = varPostalCode;
			
				item.Country = varCountry;
			
				item.Phone = varPhone;
			
				item.ShipMessage = varShipMessage;
			
				item.DtShipped = varDtShipped;
			
				item.TrackingInformation = varTrackingInformation;
			
				item.ReferenceNumber = varReferenceNumber;
			
				item.MActualShipping = varMActualShipping;
			
				item.MHandlingComputed = varMHandlingComputed;
			
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
        
        
        
        public static TableSchema.TableColumn TInvoiceIdColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn UserIdColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn CustomerIdColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn BlCompanyColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn BlFirstNameColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn BlLastNameColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn BlAddress1Column
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn BlAddress2Column
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn BlCityColumn
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        public static TableSchema.TableColumn BlStateProvinceColumn
        {
            get { return Schema.Columns[10]; }
        }
        
        
        
        public static TableSchema.TableColumn BlPostalCodeColumn
        {
            get { return Schema.Columns[11]; }
        }
        
        
        
        public static TableSchema.TableColumn BlCountryColumn
        {
            get { return Schema.Columns[12]; }
        }
        
        
        
        public static TableSchema.TableColumn BlPhoneColumn
        {
            get { return Schema.Columns[13]; }
        }
        
        
        
        public static TableSchema.TableColumn BSameAsBillingColumn
        {
            get { return Schema.Columns[14]; }
        }
        
        
        
        public static TableSchema.TableColumn CompanyNameColumn
        {
            get { return Schema.Columns[15]; }
        }
        
        
        
        public static TableSchema.TableColumn FirstNameColumn
        {
            get { return Schema.Columns[16]; }
        }
        
        
        
        public static TableSchema.TableColumn LastNameColumn
        {
            get { return Schema.Columns[17]; }
        }
        
        
        
        public static TableSchema.TableColumn Address1Column
        {
            get { return Schema.Columns[18]; }
        }
        
        
        
        public static TableSchema.TableColumn Address2Column
        {
            get { return Schema.Columns[19]; }
        }
        
        
        
        public static TableSchema.TableColumn CityColumn
        {
            get { return Schema.Columns[20]; }
        }
        
        
        
        public static TableSchema.TableColumn StateProvinceColumn
        {
            get { return Schema.Columns[21]; }
        }
        
        
        
        public static TableSchema.TableColumn PostalCodeColumn
        {
            get { return Schema.Columns[22]; }
        }
        
        
        
        public static TableSchema.TableColumn CountryColumn
        {
            get { return Schema.Columns[23]; }
        }
        
        
        
        public static TableSchema.TableColumn PhoneColumn
        {
            get { return Schema.Columns[24]; }
        }
        
        
        
        public static TableSchema.TableColumn ShipMessageColumn
        {
            get { return Schema.Columns[25]; }
        }
        
        
        
        public static TableSchema.TableColumn DtShippedColumn
        {
            get { return Schema.Columns[26]; }
        }
        
        
        
        public static TableSchema.TableColumn TrackingInformationColumn
        {
            get { return Schema.Columns[27]; }
        }
        
        
        
        public static TableSchema.TableColumn ReferenceNumberColumn
        {
            get { return Schema.Columns[28]; }
        }
        
        
        
        public static TableSchema.TableColumn MActualShippingColumn
        {
            get { return Schema.Columns[29]; }
        }
        
        
        
        public static TableSchema.TableColumn MHandlingComputedColumn
        {
            get { return Schema.Columns[30]; }
        }
        
        
        
        public static TableSchema.TableColumn DtStampColumn
        {
            get { return Schema.Columns[31]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Id = @"Id";
			 public static string TInvoiceId = @"tInvoiceId";
			 public static string UserId = @"UserId";
			 public static string CustomerId = @"CustomerId";
			 public static string BlCompany = @"blCompany";
			 public static string BlFirstName = @"blFirstName";
			 public static string BlLastName = @"blLastName";
			 public static string BlAddress1 = @"blAddress1";
			 public static string BlAddress2 = @"blAddress2";
			 public static string BlCity = @"blCity";
			 public static string BlStateProvince = @"blStateProvince";
			 public static string BlPostalCode = @"blPostalCode";
			 public static string BlCountry = @"blCountry";
			 public static string BlPhone = @"blPhone";
			 public static string BSameAsBilling = @"bSameAsBilling";
			 public static string CompanyName = @"CompanyName";
			 public static string FirstName = @"FirstName";
			 public static string LastName = @"LastName";
			 public static string Address1 = @"Address1";
			 public static string Address2 = @"Address2";
			 public static string City = @"City";
			 public static string StateProvince = @"StateProvince";
			 public static string PostalCode = @"PostalCode";
			 public static string Country = @"Country";
			 public static string Phone = @"Phone";
			 public static string ShipMessage = @"ShipMessage";
			 public static string DtShipped = @"dtShipped";
			 public static string TrackingInformation = @"TrackingInformation";
			 public static string ReferenceNumber = @"ReferenceNumber";
			 public static string MActualShipping = @"mActualShipping";
			 public static string MHandlingComputed = @"mHandlingComputed";
			 public static string DtStamp = @"dtStamp";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
