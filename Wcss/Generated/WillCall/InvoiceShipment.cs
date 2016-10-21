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
	/// Strongly-typed collection for the InvoiceShipment class.
	/// </summary>
    [Serializable]
	public partial class InvoiceShipmentCollection : ActiveList<InvoiceShipment, InvoiceShipmentCollection>
	{	   
		public InvoiceShipmentCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>InvoiceShipmentCollection</returns>
		public InvoiceShipmentCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                InvoiceShipment o = this[i];
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
	/// This is an ActiveRecord class which wraps the InvoiceShipment table.
	/// </summary>
	[Serializable]
	public partial class InvoiceShipment : ActiveRecord<InvoiceShipment>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public InvoiceShipment()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public InvoiceShipment(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public InvoiceShipment(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public InvoiceShipment(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("InvoiceShipment", TableType.Table, DataService.GetInstance("WillCall"));
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
				colvarUserId.IsForeignKey = false;
				colvarUserId.IsReadOnly = false;
				colvarUserId.DefaultSetting = @"";
				colvarUserId.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserId);
				
				TableSchema.TableColumn colvarDtCreated = new TableSchema.TableColumn(schema);
				colvarDtCreated.ColumnName = "dtCreated";
				colvarDtCreated.DataType = DbType.DateTime;
				colvarDtCreated.MaxLength = 0;
				colvarDtCreated.AutoIncrement = false;
				colvarDtCreated.IsNullable = false;
				colvarDtCreated.IsPrimaryKey = false;
				colvarDtCreated.IsForeignKey = false;
				colvarDtCreated.IsReadOnly = false;
				
						colvarDtCreated.DefaultSetting = @"(getdate())";
				colvarDtCreated.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDtCreated);
				
				TableSchema.TableColumn colvarReferenceNumber = new TableSchema.TableColumn(schema);
				colvarReferenceNumber.ColumnName = "ReferenceNumber";
				colvarReferenceNumber.DataType = DbType.Guid;
				colvarReferenceNumber.MaxLength = 0;
				colvarReferenceNumber.AutoIncrement = false;
				colvarReferenceNumber.IsNullable = false;
				colvarReferenceNumber.IsPrimaryKey = false;
				colvarReferenceNumber.IsForeignKey = false;
				colvarReferenceNumber.IsReadOnly = false;
				
						colvarReferenceNumber.DefaultSetting = @"(newid())";
				colvarReferenceNumber.ForeignKeyTableName = "";
				schema.Columns.Add(colvarReferenceNumber);
				
				TableSchema.TableColumn colvarVcContext = new TableSchema.TableColumn(schema);
				colvarVcContext.ColumnName = "vcContext";
				colvarVcContext.DataType = DbType.AnsiString;
				colvarVcContext.MaxLength = 256;
				colvarVcContext.AutoIncrement = false;
				colvarVcContext.IsNullable = false;
				colvarVcContext.IsPrimaryKey = false;
				colvarVcContext.IsForeignKey = false;
				colvarVcContext.IsReadOnly = false;
				colvarVcContext.DefaultSetting = @"";
				colvarVcContext.ForeignKeyTableName = "";
				schema.Columns.Add(colvarVcContext);
				
				TableSchema.TableColumn colvarTShipItemId = new TableSchema.TableColumn(schema);
				colvarTShipItemId.ColumnName = "TShipItemId";
				colvarTShipItemId.DataType = DbType.Int32;
				colvarTShipItemId.MaxLength = 0;
				colvarTShipItemId.AutoIncrement = false;
				colvarTShipItemId.IsNullable = true;
				colvarTShipItemId.IsPrimaryKey = false;
				colvarTShipItemId.IsForeignKey = true;
				colvarTShipItemId.IsReadOnly = false;
				colvarTShipItemId.DefaultSetting = @"";
				
					colvarTShipItemId.ForeignKeyTableName = "InvoiceItem";
				schema.Columns.Add(colvarTShipItemId);
				
				TableSchema.TableColumn colvarBLabelPrinted = new TableSchema.TableColumn(schema);
				colvarBLabelPrinted.ColumnName = "bLabelPrinted";
				colvarBLabelPrinted.DataType = DbType.Boolean;
				colvarBLabelPrinted.MaxLength = 0;
				colvarBLabelPrinted.AutoIncrement = false;
				colvarBLabelPrinted.IsNullable = false;
				colvarBLabelPrinted.IsPrimaryKey = false;
				colvarBLabelPrinted.IsForeignKey = false;
				colvarBLabelPrinted.IsReadOnly = false;
				
						colvarBLabelPrinted.DefaultSetting = @"((0))";
				colvarBLabelPrinted.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBLabelPrinted);
				
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
				
				TableSchema.TableColumn colvarBRTS = new TableSchema.TableColumn(schema);
				colvarBRTS.ColumnName = "bRTS";
				colvarBRTS.DataType = DbType.Boolean;
				colvarBRTS.MaxLength = 0;
				colvarBRTS.AutoIncrement = false;
				colvarBRTS.IsNullable = true;
				colvarBRTS.IsPrimaryKey = false;
				colvarBRTS.IsForeignKey = false;
				colvarBRTS.IsReadOnly = false;
				
						colvarBRTS.DefaultSetting = @"((0))";
				colvarBRTS.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBRTS);
				
				TableSchema.TableColumn colvarStatus = new TableSchema.TableColumn(schema);
				colvarStatus.ColumnName = "Status";
				colvarStatus.DataType = DbType.AnsiString;
				colvarStatus.MaxLength = 50;
				colvarStatus.AutoIncrement = false;
				colvarStatus.IsNullable = true;
				colvarStatus.IsPrimaryKey = false;
				colvarStatus.IsForeignKey = false;
				colvarStatus.IsReadOnly = false;
				colvarStatus.DefaultSetting = @"";
				colvarStatus.ForeignKeyTableName = "";
				schema.Columns.Add(colvarStatus);
				
				TableSchema.TableColumn colvarVcCarrier = new TableSchema.TableColumn(schema);
				colvarVcCarrier.ColumnName = "vcCarrier";
				colvarVcCarrier.DataType = DbType.AnsiString;
				colvarVcCarrier.MaxLength = 256;
				colvarVcCarrier.AutoIncrement = false;
				colvarVcCarrier.IsNullable = false;
				colvarVcCarrier.IsPrimaryKey = false;
				colvarVcCarrier.IsForeignKey = false;
				colvarVcCarrier.IsReadOnly = false;
				colvarVcCarrier.DefaultSetting = @"";
				colvarVcCarrier.ForeignKeyTableName = "";
				schema.Columns.Add(colvarVcCarrier);
				
				TableSchema.TableColumn colvarShipMethod = new TableSchema.TableColumn(schema);
				colvarShipMethod.ColumnName = "ShipMethod";
				colvarShipMethod.DataType = DbType.AnsiString;
				colvarShipMethod.MaxLength = 256;
				colvarShipMethod.AutoIncrement = false;
				colvarShipMethod.IsNullable = false;
				colvarShipMethod.IsPrimaryKey = false;
				colvarShipMethod.IsForeignKey = false;
				colvarShipMethod.IsReadOnly = false;
				colvarShipMethod.DefaultSetting = @"";
				colvarShipMethod.ForeignKeyTableName = "";
				schema.Columns.Add(colvarShipMethod);
				
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
				
				TableSchema.TableColumn colvarPackingList = new TableSchema.TableColumn(schema);
				colvarPackingList.ColumnName = "PackingList";
				colvarPackingList.DataType = DbType.AnsiString;
				colvarPackingList.MaxLength = 2000;
				colvarPackingList.AutoIncrement = false;
				colvarPackingList.IsNullable = false;
				colvarPackingList.IsPrimaryKey = false;
				colvarPackingList.IsForeignKey = false;
				colvarPackingList.IsReadOnly = false;
				
						colvarPackingList.DefaultSetting = @"('')";
				colvarPackingList.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPackingList);
				
				TableSchema.TableColumn colvarPackingAdditional = new TableSchema.TableColumn(schema);
				colvarPackingAdditional.ColumnName = "PackingAdditional";
				colvarPackingAdditional.DataType = DbType.AnsiString;
				colvarPackingAdditional.MaxLength = 500;
				colvarPackingAdditional.AutoIncrement = false;
				colvarPackingAdditional.IsNullable = true;
				colvarPackingAdditional.IsPrimaryKey = false;
				colvarPackingAdditional.IsForeignKey = false;
				colvarPackingAdditional.IsReadOnly = false;
				colvarPackingAdditional.DefaultSetting = @"";
				colvarPackingAdditional.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPackingAdditional);
				
				TableSchema.TableColumn colvarMWeightCalculated = new TableSchema.TableColumn(schema);
				colvarMWeightCalculated.ColumnName = "mWeightCalculated";
				colvarMWeightCalculated.DataType = DbType.Currency;
				colvarMWeightCalculated.MaxLength = 0;
				colvarMWeightCalculated.AutoIncrement = false;
				colvarMWeightCalculated.IsNullable = false;
				colvarMWeightCalculated.IsPrimaryKey = false;
				colvarMWeightCalculated.IsForeignKey = false;
				colvarMWeightCalculated.IsReadOnly = false;
				
						colvarMWeightCalculated.DefaultSetting = @"((0))";
				colvarMWeightCalculated.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMWeightCalculated);
				
				TableSchema.TableColumn colvarMWeightActual = new TableSchema.TableColumn(schema);
				colvarMWeightActual.ColumnName = "mWeightActual";
				colvarMWeightActual.DataType = DbType.Currency;
				colvarMWeightActual.MaxLength = 0;
				colvarMWeightActual.AutoIncrement = false;
				colvarMWeightActual.IsNullable = false;
				colvarMWeightActual.IsPrimaryKey = false;
				colvarMWeightActual.IsForeignKey = false;
				colvarMWeightActual.IsReadOnly = false;
				
						colvarMWeightActual.DefaultSetting = @"((0))";
				colvarMWeightActual.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMWeightActual);
				
				TableSchema.TableColumn colvarMHandlingCalculated = new TableSchema.TableColumn(schema);
				colvarMHandlingCalculated.ColumnName = "mHandlingCalculated";
				colvarMHandlingCalculated.DataType = DbType.Currency;
				colvarMHandlingCalculated.MaxLength = 0;
				colvarMHandlingCalculated.AutoIncrement = false;
				colvarMHandlingCalculated.IsNullable = false;
				colvarMHandlingCalculated.IsPrimaryKey = false;
				colvarMHandlingCalculated.IsForeignKey = false;
				colvarMHandlingCalculated.IsReadOnly = false;
				
						colvarMHandlingCalculated.DefaultSetting = @"((0))";
				colvarMHandlingCalculated.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMHandlingCalculated);
				
				TableSchema.TableColumn colvarMShippingCharged = new TableSchema.TableColumn(schema);
				colvarMShippingCharged.ColumnName = "mShippingCharged";
				colvarMShippingCharged.DataType = DbType.Currency;
				colvarMShippingCharged.MaxLength = 0;
				colvarMShippingCharged.AutoIncrement = false;
				colvarMShippingCharged.IsNullable = false;
				colvarMShippingCharged.IsPrimaryKey = false;
				colvarMShippingCharged.IsForeignKey = false;
				colvarMShippingCharged.IsReadOnly = false;
				
						colvarMShippingCharged.DefaultSetting = @"((0))";
				colvarMShippingCharged.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMShippingCharged);
				
				TableSchema.TableColumn colvarMShippingActual = new TableSchema.TableColumn(schema);
				colvarMShippingActual.ColumnName = "mShippingActual";
				colvarMShippingActual.DataType = DbType.Currency;
				colvarMShippingActual.MaxLength = 0;
				colvarMShippingActual.AutoIncrement = false;
				colvarMShippingActual.IsNullable = false;
				colvarMShippingActual.IsPrimaryKey = false;
				colvarMShippingActual.IsForeignKey = false;
				colvarMShippingActual.IsReadOnly = false;
				
						colvarMShippingActual.DefaultSetting = @"((0))";
				colvarMShippingActual.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMShippingActual);
				
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
				DataService.Providers["WillCall"].AddSchema("InvoiceShipment",schema);
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
		  
		[XmlAttribute("DtCreated")]
		[Bindable(true)]
		public DateTime DtCreated 
		{
			get { return GetColumnValue<DateTime>(Columns.DtCreated); }
			set { SetColumnValue(Columns.DtCreated, value); }
		}
		  
		[XmlAttribute("ReferenceNumber")]
		[Bindable(true)]
		public Guid ReferenceNumber 
		{
			get { return GetColumnValue<Guid>(Columns.ReferenceNumber); }
			set { SetColumnValue(Columns.ReferenceNumber, value); }
		}
		  
		[XmlAttribute("VcContext")]
		[Bindable(true)]
		public string VcContext 
		{
			get { return GetColumnValue<string>(Columns.VcContext); }
			set { SetColumnValue(Columns.VcContext, value); }
		}
		  
		[XmlAttribute("TShipItemId")]
		[Bindable(true)]
		public int? TShipItemId 
		{
			get { return GetColumnValue<int?>(Columns.TShipItemId); }
			set { SetColumnValue(Columns.TShipItemId, value); }
		}
		  
		[XmlAttribute("BLabelPrinted")]
		[Bindable(true)]
		public bool BLabelPrinted 
		{
			get { return GetColumnValue<bool>(Columns.BLabelPrinted); }
			set { SetColumnValue(Columns.BLabelPrinted, value); }
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
		  
		[XmlAttribute("BRTS")]
		[Bindable(true)]
		public bool? BRTS 
		{
			get { return GetColumnValue<bool?>(Columns.BRTS); }
			set { SetColumnValue(Columns.BRTS, value); }
		}
		  
		[XmlAttribute("Status")]
		[Bindable(true)]
		public string Status 
		{
			get { return GetColumnValue<string>(Columns.Status); }
			set { SetColumnValue(Columns.Status, value); }
		}
		  
		[XmlAttribute("VcCarrier")]
		[Bindable(true)]
		public string VcCarrier 
		{
			get { return GetColumnValue<string>(Columns.VcCarrier); }
			set { SetColumnValue(Columns.VcCarrier, value); }
		}
		  
		[XmlAttribute("ShipMethod")]
		[Bindable(true)]
		public string ShipMethod 
		{
			get { return GetColumnValue<string>(Columns.ShipMethod); }
			set { SetColumnValue(Columns.ShipMethod, value); }
		}
		  
		[XmlAttribute("TrackingInformation")]
		[Bindable(true)]
		public string TrackingInformation 
		{
			get { return GetColumnValue<string>(Columns.TrackingInformation); }
			set { SetColumnValue(Columns.TrackingInformation, value); }
		}
		  
		[XmlAttribute("PackingList")]
		[Bindable(true)]
		public string PackingList 
		{
			get { return GetColumnValue<string>(Columns.PackingList); }
			set { SetColumnValue(Columns.PackingList, value); }
		}
		  
		[XmlAttribute("PackingAdditional")]
		[Bindable(true)]
		public string PackingAdditional 
		{
			get { return GetColumnValue<string>(Columns.PackingAdditional); }
			set { SetColumnValue(Columns.PackingAdditional, value); }
		}
		  
		[XmlAttribute("MWeightCalculated")]
		[Bindable(true)]
		public decimal MWeightCalculated 
		{
			get { return GetColumnValue<decimal>(Columns.MWeightCalculated); }
			set { SetColumnValue(Columns.MWeightCalculated, value); }
		}
		  
		[XmlAttribute("MWeightActual")]
		[Bindable(true)]
		public decimal MWeightActual 
		{
			get { return GetColumnValue<decimal>(Columns.MWeightActual); }
			set { SetColumnValue(Columns.MWeightActual, value); }
		}
		  
		[XmlAttribute("MHandlingCalculated")]
		[Bindable(true)]
		public decimal MHandlingCalculated 
		{
			get { return GetColumnValue<decimal>(Columns.MHandlingCalculated); }
			set { SetColumnValue(Columns.MHandlingCalculated, value); }
		}
		  
		[XmlAttribute("MShippingCharged")]
		[Bindable(true)]
		public decimal MShippingCharged 
		{
			get { return GetColumnValue<decimal>(Columns.MShippingCharged); }
			set { SetColumnValue(Columns.MShippingCharged, value); }
		}
		  
		[XmlAttribute("MShippingActual")]
		[Bindable(true)]
		public decimal MShippingActual 
		{
			get { return GetColumnValue<decimal>(Columns.MShippingActual); }
			set { SetColumnValue(Columns.MShippingActual, value); }
		}
		  
		[XmlAttribute("DtStamp")]
		[Bindable(true)]
		public DateTime DtStamp 
		{
			get { return GetColumnValue<DateTime>(Columns.DtStamp); }
			set { SetColumnValue(Columns.DtStamp, value); }
		}
		
		#endregion
		
		
		#region PrimaryKey Methods		
		
        protected override void SetPrimaryKey(object oValue)
        {
            base.SetPrimaryKey(oValue);
            
            SetPKValues();
        }
        
		
		private Wcss.InvoiceShipmentItemCollection colInvoiceShipmentItemRecords;
		public Wcss.InvoiceShipmentItemCollection InvoiceShipmentItemRecords()
		{
			if(colInvoiceShipmentItemRecords == null)
			{
				colInvoiceShipmentItemRecords = new Wcss.InvoiceShipmentItemCollection().Where(InvoiceShipmentItem.Columns.TInvoiceShipmentId, Id).Load();
				colInvoiceShipmentItemRecords.ListChanged += new ListChangedEventHandler(colInvoiceShipmentItemRecords_ListChanged);
			}
			return colInvoiceShipmentItemRecords;
		}
				
		void colInvoiceShipmentItemRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colInvoiceShipmentItemRecords[e.NewIndex].TInvoiceShipmentId = Id;
				colInvoiceShipmentItemRecords.ListChanged += new ListChangedEventHandler(colInvoiceShipmentItemRecords_ListChanged);
            }
		}
		private Wcss.ShipmentBatchInvoiceShipmentCollection colShipmentBatchInvoiceShipmentRecords;
		public Wcss.ShipmentBatchInvoiceShipmentCollection ShipmentBatchInvoiceShipmentRecords()
		{
			if(colShipmentBatchInvoiceShipmentRecords == null)
			{
				colShipmentBatchInvoiceShipmentRecords = new Wcss.ShipmentBatchInvoiceShipmentCollection().Where(ShipmentBatchInvoiceShipment.Columns.TInvoiceShipmentId, Id).Load();
				colShipmentBatchInvoiceShipmentRecords.ListChanged += new ListChangedEventHandler(colShipmentBatchInvoiceShipmentRecords_ListChanged);
			}
			return colShipmentBatchInvoiceShipmentRecords;
		}
				
		void colShipmentBatchInvoiceShipmentRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colShipmentBatchInvoiceShipmentRecords[e.NewIndex].TInvoiceShipmentId = Id;
				colShipmentBatchInvoiceShipmentRecords.ListChanged += new ListChangedEventHandler(colShipmentBatchInvoiceShipmentRecords_ListChanged);
            }
		}
		#endregion
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a Invoice ActiveRecord object related to this InvoiceShipment
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
		
		
		/// <summary>
		/// Returns a InvoiceItem ActiveRecord object related to this InvoiceShipment
		/// 
		/// </summary>
		private Wcss.InvoiceItem InvoiceItem
		{
			get { return Wcss.InvoiceItem.FetchByID(this.TShipItemId); }
			set { SetColumnValue("TShipItemId", value.Id); }
		}
        //set up an alternate mechanism to avoid a database call
		private Wcss.InvoiceItem _invoiceitemrecord = null;
		
		public Wcss.InvoiceItem InvoiceItemRecord
		{
		    get
            {
                if (_invoiceitemrecord == null)
                {
                    _invoiceitemrecord = new Wcss.InvoiceItem();
                    _invoiceitemrecord.CopyFrom(this.InvoiceItem);
                }
                return _invoiceitemrecord;
            }
            set
            {
                if(value != null && _invoiceitemrecord == null)
			        _invoiceitemrecord = new Wcss.InvoiceItem();
                
                SetColumnValue("TShipItemId", value.Id);
                _invoiceitemrecord.CopyFrom(value);                
            }
		}
		
		
		#endregion
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(int varTInvoiceId,Guid? varUserId,DateTime varDtCreated,Guid varReferenceNumber,string varVcContext,int? varTShipItemId,bool varBLabelPrinted,string varCompanyName,string varFirstName,string varLastName,string varAddress1,string varAddress2,string varCity,string varStateProvince,string varPostalCode,string varCountry,string varPhone,string varShipMessage,DateTime? varDtShipped,bool? varBRTS,string varStatus,string varVcCarrier,string varShipMethod,string varTrackingInformation,string varPackingList,string varPackingAdditional,decimal varMWeightCalculated,decimal varMWeightActual,decimal varMHandlingCalculated,decimal varMShippingCharged,decimal varMShippingActual,DateTime varDtStamp)
		{
			InvoiceShipment item = new InvoiceShipment();
			
			item.TInvoiceId = varTInvoiceId;
			
			item.UserId = varUserId;
			
			item.DtCreated = varDtCreated;
			
			item.ReferenceNumber = varReferenceNumber;
			
			item.VcContext = varVcContext;
			
			item.TShipItemId = varTShipItemId;
			
			item.BLabelPrinted = varBLabelPrinted;
			
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
			
			item.BRTS = varBRTS;
			
			item.Status = varStatus;
			
			item.VcCarrier = varVcCarrier;
			
			item.ShipMethod = varShipMethod;
			
			item.TrackingInformation = varTrackingInformation;
			
			item.PackingList = varPackingList;
			
			item.PackingAdditional = varPackingAdditional;
			
			item.MWeightCalculated = varMWeightCalculated;
			
			item.MWeightActual = varMWeightActual;
			
			item.MHandlingCalculated = varMHandlingCalculated;
			
			item.MShippingCharged = varMShippingCharged;
			
			item.MShippingActual = varMShippingActual;
			
			item.DtStamp = varDtStamp;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varId,int varTInvoiceId,Guid? varUserId,DateTime varDtCreated,Guid varReferenceNumber,string varVcContext,int? varTShipItemId,bool varBLabelPrinted,string varCompanyName,string varFirstName,string varLastName,string varAddress1,string varAddress2,string varCity,string varStateProvince,string varPostalCode,string varCountry,string varPhone,string varShipMessage,DateTime? varDtShipped,bool? varBRTS,string varStatus,string varVcCarrier,string varShipMethod,string varTrackingInformation,string varPackingList,string varPackingAdditional,decimal varMWeightCalculated,decimal varMWeightActual,decimal varMHandlingCalculated,decimal varMShippingCharged,decimal varMShippingActual,DateTime varDtStamp)
		{
			InvoiceShipment item = new InvoiceShipment();
			
				item.Id = varId;
			
				item.TInvoiceId = varTInvoiceId;
			
				item.UserId = varUserId;
			
				item.DtCreated = varDtCreated;
			
				item.ReferenceNumber = varReferenceNumber;
			
				item.VcContext = varVcContext;
			
				item.TShipItemId = varTShipItemId;
			
				item.BLabelPrinted = varBLabelPrinted;
			
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
			
				item.BRTS = varBRTS;
			
				item.Status = varStatus;
			
				item.VcCarrier = varVcCarrier;
			
				item.ShipMethod = varShipMethod;
			
				item.TrackingInformation = varTrackingInformation;
			
				item.PackingList = varPackingList;
			
				item.PackingAdditional = varPackingAdditional;
			
				item.MWeightCalculated = varMWeightCalculated;
			
				item.MWeightActual = varMWeightActual;
			
				item.MHandlingCalculated = varMHandlingCalculated;
			
				item.MShippingCharged = varMShippingCharged;
			
				item.MShippingActual = varMShippingActual;
			
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
        
        
        
        public static TableSchema.TableColumn DtCreatedColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn ReferenceNumberColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn VcContextColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn TShipItemIdColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn BLabelPrintedColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn CompanyNameColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn FirstNameColumn
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        public static TableSchema.TableColumn LastNameColumn
        {
            get { return Schema.Columns[10]; }
        }
        
        
        
        public static TableSchema.TableColumn Address1Column
        {
            get { return Schema.Columns[11]; }
        }
        
        
        
        public static TableSchema.TableColumn Address2Column
        {
            get { return Schema.Columns[12]; }
        }
        
        
        
        public static TableSchema.TableColumn CityColumn
        {
            get { return Schema.Columns[13]; }
        }
        
        
        
        public static TableSchema.TableColumn StateProvinceColumn
        {
            get { return Schema.Columns[14]; }
        }
        
        
        
        public static TableSchema.TableColumn PostalCodeColumn
        {
            get { return Schema.Columns[15]; }
        }
        
        
        
        public static TableSchema.TableColumn CountryColumn
        {
            get { return Schema.Columns[16]; }
        }
        
        
        
        public static TableSchema.TableColumn PhoneColumn
        {
            get { return Schema.Columns[17]; }
        }
        
        
        
        public static TableSchema.TableColumn ShipMessageColumn
        {
            get { return Schema.Columns[18]; }
        }
        
        
        
        public static TableSchema.TableColumn DtShippedColumn
        {
            get { return Schema.Columns[19]; }
        }
        
        
        
        public static TableSchema.TableColumn BRTSColumn
        {
            get { return Schema.Columns[20]; }
        }
        
        
        
        public static TableSchema.TableColumn StatusColumn
        {
            get { return Schema.Columns[21]; }
        }
        
        
        
        public static TableSchema.TableColumn VcCarrierColumn
        {
            get { return Schema.Columns[22]; }
        }
        
        
        
        public static TableSchema.TableColumn ShipMethodColumn
        {
            get { return Schema.Columns[23]; }
        }
        
        
        
        public static TableSchema.TableColumn TrackingInformationColumn
        {
            get { return Schema.Columns[24]; }
        }
        
        
        
        public static TableSchema.TableColumn PackingListColumn
        {
            get { return Schema.Columns[25]; }
        }
        
        
        
        public static TableSchema.TableColumn PackingAdditionalColumn
        {
            get { return Schema.Columns[26]; }
        }
        
        
        
        public static TableSchema.TableColumn MWeightCalculatedColumn
        {
            get { return Schema.Columns[27]; }
        }
        
        
        
        public static TableSchema.TableColumn MWeightActualColumn
        {
            get { return Schema.Columns[28]; }
        }
        
        
        
        public static TableSchema.TableColumn MHandlingCalculatedColumn
        {
            get { return Schema.Columns[29]; }
        }
        
        
        
        public static TableSchema.TableColumn MShippingChargedColumn
        {
            get { return Schema.Columns[30]; }
        }
        
        
        
        public static TableSchema.TableColumn MShippingActualColumn
        {
            get { return Schema.Columns[31]; }
        }
        
        
        
        public static TableSchema.TableColumn DtStampColumn
        {
            get { return Schema.Columns[32]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Id = @"Id";
			 public static string TInvoiceId = @"tInvoiceId";
			 public static string UserId = @"UserId";
			 public static string DtCreated = @"dtCreated";
			 public static string ReferenceNumber = @"ReferenceNumber";
			 public static string VcContext = @"vcContext";
			 public static string TShipItemId = @"TShipItemId";
			 public static string BLabelPrinted = @"bLabelPrinted";
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
			 public static string BRTS = @"bRTS";
			 public static string Status = @"Status";
			 public static string VcCarrier = @"vcCarrier";
			 public static string ShipMethod = @"ShipMethod";
			 public static string TrackingInformation = @"TrackingInformation";
			 public static string PackingList = @"PackingList";
			 public static string PackingAdditional = @"PackingAdditional";
			 public static string MWeightCalculated = @"mWeightCalculated";
			 public static string MWeightActual = @"mWeightActual";
			 public static string MHandlingCalculated = @"mHandlingCalculated";
			 public static string MShippingCharged = @"mShippingCharged";
			 public static string MShippingActual = @"mShippingActual";
			 public static string DtStamp = @"dtStamp";
						
		}
		#endregion
		
		#region Update PK Collections
		
        public void SetPKValues()
        {
                if (colInvoiceShipmentItemRecords != null)
                {
                    foreach (Wcss.InvoiceShipmentItem item in colInvoiceShipmentItemRecords)
                    {
                        if (item.TInvoiceShipmentId != Id)
                        {
                            item.TInvoiceShipmentId = Id;
                        }
                    }
               }
		
                if (colShipmentBatchInvoiceShipmentRecords != null)
                {
                    foreach (Wcss.ShipmentBatchInvoiceShipment item in colShipmentBatchInvoiceShipmentRecords)
                    {
                        if (item.TInvoiceShipmentId != Id)
                        {
                            item.TInvoiceShipmentId = Id;
                        }
                    }
               }
		}
        #endregion
    
        #region Deep Save
		
        public void DeepSave()
        {
            Save();
            
                if (colInvoiceShipmentItemRecords != null)
                {
                    colInvoiceShipmentItemRecords.SaveAll();
               }
		
                if (colShipmentBatchInvoiceShipmentRecords != null)
                {
                    colShipmentBatchInvoiceShipmentRecords.SaveAll();
               }
		}
        #endregion
	}
}
