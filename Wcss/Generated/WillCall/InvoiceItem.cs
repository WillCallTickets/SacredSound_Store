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
	/// Strongly-typed collection for the InvoiceItem class.
	/// </summary>
    [Serializable]
	public partial class InvoiceItemCollection : ActiveList<InvoiceItem, InvoiceItemCollection>
	{	   
		public InvoiceItemCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>InvoiceItemCollection</returns>
		public InvoiceItemCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                InvoiceItem o = this[i];
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
	/// This is an ActiveRecord class which wraps the InvoiceItem table.
	/// </summary>
	[Serializable]
	public partial class InvoiceItem : ActiveRecord<InvoiceItem>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public InvoiceItem()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public InvoiceItem(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public InvoiceItem(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public InvoiceItem(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("InvoiceItem", TableType.Table, DataService.GetInstance("WillCall"));
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
				colvarGuid.ColumnName = "Guid";
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
				
				TableSchema.TableColumn colvarTInvoiceId = new TableSchema.TableColumn(schema);
				colvarTInvoiceId.ColumnName = "TInvoiceId";
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
				
				TableSchema.TableColumn colvarTShowTicketId = new TableSchema.TableColumn(schema);
				colvarTShowTicketId.ColumnName = "TShowTicketId";
				colvarTShowTicketId.DataType = DbType.Int32;
				colvarTShowTicketId.MaxLength = 0;
				colvarTShowTicketId.AutoIncrement = false;
				colvarTShowTicketId.IsNullable = true;
				colvarTShowTicketId.IsPrimaryKey = false;
				colvarTShowTicketId.IsForeignKey = true;
				colvarTShowTicketId.IsReadOnly = false;
				colvarTShowTicketId.DefaultSetting = @"";
				
					colvarTShowTicketId.ForeignKeyTableName = "ShowTicket";
				schema.Columns.Add(colvarTShowTicketId);
				
				TableSchema.TableColumn colvarTMerchId = new TableSchema.TableColumn(schema);
				colvarTMerchId.ColumnName = "TMerchId";
				colvarTMerchId.DataType = DbType.Int32;
				colvarTMerchId.MaxLength = 0;
				colvarTMerchId.AutoIncrement = false;
				colvarTMerchId.IsNullable = true;
				colvarTMerchId.IsPrimaryKey = false;
				colvarTMerchId.IsForeignKey = true;
				colvarTMerchId.IsReadOnly = false;
				colvarTMerchId.DefaultSetting = @"";
				
					colvarTMerchId.ForeignKeyTableName = "Merch";
				schema.Columns.Add(colvarTMerchId);
				
				TableSchema.TableColumn colvarTShowId = new TableSchema.TableColumn(schema);
				colvarTShowId.ColumnName = "TShowId";
				colvarTShowId.DataType = DbType.Int32;
				colvarTShowId.MaxLength = 0;
				colvarTShowId.AutoIncrement = false;
				colvarTShowId.IsNullable = true;
				colvarTShowId.IsPrimaryKey = false;
				colvarTShowId.IsForeignKey = false;
				colvarTShowId.IsReadOnly = false;
				colvarTShowId.DefaultSetting = @"";
				colvarTShowId.ForeignKeyTableName = "";
				schema.Columns.Add(colvarTShowId);
				
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
				
				TableSchema.TableColumn colvarTSalePromotionId = new TableSchema.TableColumn(schema);
				colvarTSalePromotionId.ColumnName = "TSalePromotionId";
				colvarTSalePromotionId.DataType = DbType.Int32;
				colvarTSalePromotionId.MaxLength = 0;
				colvarTSalePromotionId.AutoIncrement = false;
				colvarTSalePromotionId.IsNullable = true;
				colvarTSalePromotionId.IsPrimaryKey = false;
				colvarTSalePromotionId.IsForeignKey = true;
				colvarTSalePromotionId.IsReadOnly = false;
				colvarTSalePromotionId.DefaultSetting = @"";
				
					colvarTSalePromotionId.ForeignKeyTableName = "SalePromotion";
				schema.Columns.Add(colvarTSalePromotionId);
				
				TableSchema.TableColumn colvarPurchaseName = new TableSchema.TableColumn(schema);
				colvarPurchaseName.ColumnName = "PurchaseName";
				colvarPurchaseName.DataType = DbType.AnsiString;
				colvarPurchaseName.MaxLength = 300;
				colvarPurchaseName.AutoIncrement = false;
				colvarPurchaseName.IsNullable = true;
				colvarPurchaseName.IsPrimaryKey = false;
				colvarPurchaseName.IsForeignKey = false;
				colvarPurchaseName.IsReadOnly = false;
				colvarPurchaseName.DefaultSetting = @"";
				colvarPurchaseName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPurchaseName);
				
				TableSchema.TableColumn colvarDtDateOfShow = new TableSchema.TableColumn(schema);
				colvarDtDateOfShow.ColumnName = "dtDateOfShow";
				colvarDtDateOfShow.DataType = DbType.DateTime;
				colvarDtDateOfShow.MaxLength = 0;
				colvarDtDateOfShow.AutoIncrement = false;
				colvarDtDateOfShow.IsNullable = true;
				colvarDtDateOfShow.IsPrimaryKey = false;
				colvarDtDateOfShow.IsForeignKey = false;
				colvarDtDateOfShow.IsReadOnly = false;
				colvarDtDateOfShow.DefaultSetting = @"";
				colvarDtDateOfShow.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDtDateOfShow);
				
				TableSchema.TableColumn colvarAgeDescription = new TableSchema.TableColumn(schema);
				colvarAgeDescription.ColumnName = "AgeDescription";
				colvarAgeDescription.DataType = DbType.AnsiString;
				colvarAgeDescription.MaxLength = 200;
				colvarAgeDescription.AutoIncrement = false;
				colvarAgeDescription.IsNullable = true;
				colvarAgeDescription.IsPrimaryKey = false;
				colvarAgeDescription.IsForeignKey = false;
				colvarAgeDescription.IsReadOnly = false;
				colvarAgeDescription.DefaultSetting = @"";
				colvarAgeDescription.ForeignKeyTableName = "";
				schema.Columns.Add(colvarAgeDescription);
				
				TableSchema.TableColumn colvarMainActName = new TableSchema.TableColumn(schema);
				colvarMainActName.ColumnName = "MainActName";
				colvarMainActName.DataType = DbType.AnsiString;
				colvarMainActName.MaxLength = 305;
				colvarMainActName.AutoIncrement = false;
				colvarMainActName.IsNullable = true;
				colvarMainActName.IsPrimaryKey = false;
				colvarMainActName.IsForeignKey = false;
				colvarMainActName.IsReadOnly = false;
				colvarMainActName.DefaultSetting = @"";
				colvarMainActName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMainActName);
				
				TableSchema.TableColumn colvarCriteria = new TableSchema.TableColumn(schema);
				colvarCriteria.ColumnName = "Criteria";
				colvarCriteria.DataType = DbType.AnsiString;
				colvarCriteria.MaxLength = 300;
				colvarCriteria.AutoIncrement = false;
				colvarCriteria.IsNullable = true;
				colvarCriteria.IsPrimaryKey = false;
				colvarCriteria.IsForeignKey = false;
				colvarCriteria.IsReadOnly = false;
				colvarCriteria.DefaultSetting = @"";
				colvarCriteria.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCriteria);
				
				TableSchema.TableColumn colvarDescription = new TableSchema.TableColumn(schema);
				colvarDescription.ColumnName = "Description";
				colvarDescription.DataType = DbType.AnsiString;
				colvarDescription.MaxLength = 300;
				colvarDescription.AutoIncrement = false;
				colvarDescription.IsNullable = true;
				colvarDescription.IsPrimaryKey = false;
				colvarDescription.IsForeignKey = false;
				colvarDescription.IsReadOnly = false;
				colvarDescription.DefaultSetting = @"";
				colvarDescription.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDescription);
				
				TableSchema.TableColumn colvarMPrice = new TableSchema.TableColumn(schema);
				colvarMPrice.ColumnName = "mPrice";
				colvarMPrice.DataType = DbType.Currency;
				colvarMPrice.MaxLength = 0;
				colvarMPrice.AutoIncrement = false;
				colvarMPrice.IsNullable = false;
				colvarMPrice.IsPrimaryKey = false;
				colvarMPrice.IsForeignKey = false;
				colvarMPrice.IsReadOnly = false;
				colvarMPrice.DefaultSetting = @"";
				colvarMPrice.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMPrice);
				
				TableSchema.TableColumn colvarMServiceCharge = new TableSchema.TableColumn(schema);
				colvarMServiceCharge.ColumnName = "mServiceCharge";
				colvarMServiceCharge.DataType = DbType.Currency;
				colvarMServiceCharge.MaxLength = 0;
				colvarMServiceCharge.AutoIncrement = false;
				colvarMServiceCharge.IsNullable = false;
				colvarMServiceCharge.IsPrimaryKey = false;
				colvarMServiceCharge.IsForeignKey = false;
				colvarMServiceCharge.IsReadOnly = false;
				
						colvarMServiceCharge.DefaultSetting = @"((0))";
				colvarMServiceCharge.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMServiceCharge);
				
				TableSchema.TableColumn colvarMAdjustment = new TableSchema.TableColumn(schema);
				colvarMAdjustment.ColumnName = "mAdjustment";
				colvarMAdjustment.DataType = DbType.Currency;
				colvarMAdjustment.MaxLength = 0;
				colvarMAdjustment.AutoIncrement = false;
				colvarMAdjustment.IsNullable = false;
				colvarMAdjustment.IsPrimaryKey = false;
				colvarMAdjustment.IsForeignKey = false;
				colvarMAdjustment.IsReadOnly = false;
				
						colvarMAdjustment.DefaultSetting = @"((0))";
				colvarMAdjustment.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMAdjustment);
				
				TableSchema.TableColumn colvarMPricePerItem = new TableSchema.TableColumn(schema);
				colvarMPricePerItem.ColumnName = "mPricePerItem";
				colvarMPricePerItem.DataType = DbType.Currency;
				colvarMPricePerItem.MaxLength = 0;
				colvarMPricePerItem.AutoIncrement = false;
				colvarMPricePerItem.IsNullable = true;
				colvarMPricePerItem.IsPrimaryKey = false;
				colvarMPricePerItem.IsForeignKey = false;
				colvarMPricePerItem.IsReadOnly = true;
				colvarMPricePerItem.DefaultSetting = @"";
				colvarMPricePerItem.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMPricePerItem);
				
				TableSchema.TableColumn colvarIQuantity = new TableSchema.TableColumn(schema);
				colvarIQuantity.ColumnName = "iQuantity";
				colvarIQuantity.DataType = DbType.Int32;
				colvarIQuantity.MaxLength = 0;
				colvarIQuantity.AutoIncrement = false;
				colvarIQuantity.IsNullable = false;
				colvarIQuantity.IsPrimaryKey = false;
				colvarIQuantity.IsForeignKey = false;
				colvarIQuantity.IsReadOnly = false;
				colvarIQuantity.DefaultSetting = @"";
				colvarIQuantity.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIQuantity);
				
				TableSchema.TableColumn colvarMLineItemTotal = new TableSchema.TableColumn(schema);
				colvarMLineItemTotal.ColumnName = "mLineItemTotal";
				colvarMLineItemTotal.DataType = DbType.Currency;
				colvarMLineItemTotal.MaxLength = 0;
				colvarMLineItemTotal.AutoIncrement = false;
				colvarMLineItemTotal.IsNullable = true;
				colvarMLineItemTotal.IsPrimaryKey = false;
				colvarMLineItemTotal.IsForeignKey = false;
				colvarMLineItemTotal.IsReadOnly = true;
				colvarMLineItemTotal.DefaultSetting = @"";
				colvarMLineItemTotal.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMLineItemTotal);
				
				TableSchema.TableColumn colvarPurchaseAction = new TableSchema.TableColumn(schema);
				colvarPurchaseAction.ColumnName = "PurchaseAction";
				colvarPurchaseAction.DataType = DbType.AnsiString;
				colvarPurchaseAction.MaxLength = 50;
				colvarPurchaseAction.AutoIncrement = false;
				colvarPurchaseAction.IsNullable = false;
				colvarPurchaseAction.IsPrimaryKey = false;
				colvarPurchaseAction.IsForeignKey = false;
				colvarPurchaseAction.IsReadOnly = false;
				colvarPurchaseAction.DefaultSetting = @"";
				colvarPurchaseAction.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPurchaseAction);
				
				TableSchema.TableColumn colvarNotes = new TableSchema.TableColumn(schema);
				colvarNotes.ColumnName = "Notes";
				colvarNotes.DataType = DbType.AnsiString;
				colvarNotes.MaxLength = 1500;
				colvarNotes.AutoIncrement = false;
				colvarNotes.IsNullable = true;
				colvarNotes.IsPrimaryKey = false;
				colvarNotes.IsForeignKey = false;
				colvarNotes.IsReadOnly = false;
				colvarNotes.DefaultSetting = @"";
				colvarNotes.ForeignKeyTableName = "";
				schema.Columns.Add(colvarNotes);
				
				TableSchema.TableColumn colvarPickupName = new TableSchema.TableColumn(schema);
				colvarPickupName.ColumnName = "PickupName";
				colvarPickupName.DataType = DbType.AnsiString;
				colvarPickupName.MaxLength = 256;
				colvarPickupName.AutoIncrement = false;
				colvarPickupName.IsNullable = true;
				colvarPickupName.IsPrimaryKey = false;
				colvarPickupName.IsForeignKey = false;
				colvarPickupName.IsReadOnly = false;
				colvarPickupName.DefaultSetting = @"";
				colvarPickupName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPickupName);
				
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
				
				TableSchema.TableColumn colvarShippingNotes = new TableSchema.TableColumn(schema);
				colvarShippingNotes.ColumnName = "ShippingNotes";
				colvarShippingNotes.DataType = DbType.AnsiString;
				colvarShippingNotes.MaxLength = 500;
				colvarShippingNotes.AutoIncrement = false;
				colvarShippingNotes.IsNullable = true;
				colvarShippingNotes.IsPrimaryKey = false;
				colvarShippingNotes.IsForeignKey = false;
				colvarShippingNotes.IsReadOnly = false;
				colvarShippingNotes.DefaultSetting = @"";
				colvarShippingNotes.ForeignKeyTableName = "";
				schema.Columns.Add(colvarShippingNotes);
				
				TableSchema.TableColumn colvarShippingMethod = new TableSchema.TableColumn(schema);
				colvarShippingMethod.ColumnName = "ShippingMethod";
				colvarShippingMethod.DataType = DbType.AnsiString;
				colvarShippingMethod.MaxLength = 256;
				colvarShippingMethod.AutoIncrement = false;
				colvarShippingMethod.IsNullable = true;
				colvarShippingMethod.IsPrimaryKey = false;
				colvarShippingMethod.IsForeignKey = false;
				colvarShippingMethod.IsReadOnly = false;
				colvarShippingMethod.DefaultSetting = @"";
				colvarShippingMethod.ForeignKeyTableName = "";
				schema.Columns.Add(colvarShippingMethod);
				
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
				DataService.Providers["WillCall"].AddSchema("InvoiceItem",schema);
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
		  
		[XmlAttribute("TInvoiceId")]
		[Bindable(true)]
		public int TInvoiceId 
		{
			get { return GetColumnValue<int>(Columns.TInvoiceId); }
			set { SetColumnValue(Columns.TInvoiceId, value); }
		}
		  
		[XmlAttribute("VcContext")]
		[Bindable(true)]
		public string VcContext 
		{
			get { return GetColumnValue<string>(Columns.VcContext); }
			set { SetColumnValue(Columns.VcContext, value); }
		}
		  
		[XmlAttribute("TShowTicketId")]
		[Bindable(true)]
		public int? TShowTicketId 
		{
			get { return GetColumnValue<int?>(Columns.TShowTicketId); }
			set { SetColumnValue(Columns.TShowTicketId, value); }
		}
		  
		[XmlAttribute("TMerchId")]
		[Bindable(true)]
		public int? TMerchId 
		{
			get { return GetColumnValue<int?>(Columns.TMerchId); }
			set { SetColumnValue(Columns.TMerchId, value); }
		}
		  
		[XmlAttribute("TShowId")]
		[Bindable(true)]
		public int? TShowId 
		{
			get { return GetColumnValue<int?>(Columns.TShowId); }
			set { SetColumnValue(Columns.TShowId, value); }
		}
		  
		[XmlAttribute("TShipItemId")]
		[Bindable(true)]
		public int? TShipItemId 
		{
			get { return GetColumnValue<int?>(Columns.TShipItemId); }
			set { SetColumnValue(Columns.TShipItemId, value); }
		}
		  
		[XmlAttribute("TSalePromotionId")]
		[Bindable(true)]
		public int? TSalePromotionId 
		{
			get { return GetColumnValue<int?>(Columns.TSalePromotionId); }
			set { SetColumnValue(Columns.TSalePromotionId, value); }
		}
		  
		[XmlAttribute("PurchaseName")]
		[Bindable(true)]
		public string PurchaseName 
		{
			get { return GetColumnValue<string>(Columns.PurchaseName); }
			set { SetColumnValue(Columns.PurchaseName, value); }
		}
		  
		[XmlAttribute("DtDateOfShow")]
		[Bindable(true)]
		public DateTime? DtDateOfShow 
		{
			get { return GetColumnValue<DateTime?>(Columns.DtDateOfShow); }
			set { SetColumnValue(Columns.DtDateOfShow, value); }
		}
		  
		[XmlAttribute("AgeDescription")]
		[Bindable(true)]
		public string AgeDescription 
		{
			get { return GetColumnValue<string>(Columns.AgeDescription); }
			set { SetColumnValue(Columns.AgeDescription, value); }
		}
		  
		[XmlAttribute("MainActName")]
		[Bindable(true)]
		public string MainActName 
		{
			get { return GetColumnValue<string>(Columns.MainActName); }
			set { SetColumnValue(Columns.MainActName, value); }
		}
		  
		[XmlAttribute("Criteria")]
		[Bindable(true)]
		public string Criteria 
		{
			get { return GetColumnValue<string>(Columns.Criteria); }
			set { SetColumnValue(Columns.Criteria, value); }
		}
		  
		[XmlAttribute("Description")]
		[Bindable(true)]
		public string Description 
		{
			get { return GetColumnValue<string>(Columns.Description); }
			set { SetColumnValue(Columns.Description, value); }
		}
		  
		[XmlAttribute("MPrice")]
		[Bindable(true)]
		public decimal MPrice 
		{
			get { return GetColumnValue<decimal>(Columns.MPrice); }
			set { SetColumnValue(Columns.MPrice, value); }
		}
		  
		[XmlAttribute("MServiceCharge")]
		[Bindable(true)]
		public decimal MServiceCharge 
		{
			get { return GetColumnValue<decimal>(Columns.MServiceCharge); }
			set { SetColumnValue(Columns.MServiceCharge, value); }
		}
		  
		[XmlAttribute("MAdjustment")]
		[Bindable(true)]
		public decimal MAdjustment 
		{
			get { return GetColumnValue<decimal>(Columns.MAdjustment); }
			set { SetColumnValue(Columns.MAdjustment, value); }
		}
		  
		[XmlAttribute("MPricePerItem")]
		[Bindable(true)]
		public decimal? MPricePerItem 
		{
			get { return GetColumnValue<decimal?>(Columns.MPricePerItem); }
			set { SetColumnValue(Columns.MPricePerItem, value); }
		}
		  
		[XmlAttribute("IQuantity")]
		[Bindable(true)]
		public int IQuantity 
		{
			get { return GetColumnValue<int>(Columns.IQuantity); }
			set { SetColumnValue(Columns.IQuantity, value); }
		}
		  
		[XmlAttribute("MLineItemTotal")]
		[Bindable(true)]
		public decimal? MLineItemTotal 
		{
			get { return GetColumnValue<decimal?>(Columns.MLineItemTotal); }
			set { SetColumnValue(Columns.MLineItemTotal, value); }
		}
		  
		[XmlAttribute("PurchaseAction")]
		[Bindable(true)]
		public string PurchaseAction 
		{
			get { return GetColumnValue<string>(Columns.PurchaseAction); }
			set { SetColumnValue(Columns.PurchaseAction, value); }
		}
		  
		[XmlAttribute("Notes")]
		[Bindable(true)]
		public string Notes 
		{
			get { return GetColumnValue<string>(Columns.Notes); }
			set { SetColumnValue(Columns.Notes, value); }
		}
		  
		[XmlAttribute("PickupName")]
		[Bindable(true)]
		public string PickupName 
		{
			get { return GetColumnValue<string>(Columns.PickupName); }
			set { SetColumnValue(Columns.PickupName, value); }
		}
		  
		[XmlAttribute("BRTS")]
		[Bindable(true)]
		public bool? BRTS 
		{
			get { return GetColumnValue<bool?>(Columns.BRTS); }
			set { SetColumnValue(Columns.BRTS, value); }
		}
		  
		[XmlAttribute("DtShipped")]
		[Bindable(true)]
		public DateTime? DtShipped 
		{
			get { return GetColumnValue<DateTime?>(Columns.DtShipped); }
			set { SetColumnValue(Columns.DtShipped, value); }
		}
		  
		[XmlAttribute("ShippingNotes")]
		[Bindable(true)]
		public string ShippingNotes 
		{
			get { return GetColumnValue<string>(Columns.ShippingNotes); }
			set { SetColumnValue(Columns.ShippingNotes, value); }
		}
		  
		[XmlAttribute("ShippingMethod")]
		[Bindable(true)]
		public string ShippingMethod 
		{
			get { return GetColumnValue<string>(Columns.ShippingMethod); }
			set { SetColumnValue(Columns.ShippingMethod, value); }
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
        
		
		private Wcss.CharitableContributionCollection colCharitableContributionRecords;
		public Wcss.CharitableContributionCollection CharitableContributionRecords()
		{
			if(colCharitableContributionRecords == null)
			{
				colCharitableContributionRecords = new Wcss.CharitableContributionCollection().Where(CharitableContribution.Columns.TInvoiceItemId, Id).Load();
				colCharitableContributionRecords.ListChanged += new ListChangedEventHandler(colCharitableContributionRecords_ListChanged);
			}
			return colCharitableContributionRecords;
		}
				
		void colCharitableContributionRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colCharitableContributionRecords[e.NewIndex].TInvoiceItemId = Id;
				colCharitableContributionRecords.ListChanged += new ListChangedEventHandler(colCharitableContributionRecords_ListChanged);
            }
		}
		private Wcss.InventoryCollection colInventoryRecords;
		public Wcss.InventoryCollection InventoryRecords()
		{
			if(colInventoryRecords == null)
			{
				colInventoryRecords = new Wcss.InventoryCollection().Where(Inventory.Columns.TInvoiceItemId, Id).Load();
				colInventoryRecords.ListChanged += new ListChangedEventHandler(colInventoryRecords_ListChanged);
			}
			return colInventoryRecords;
		}
				
		void colInventoryRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colInventoryRecords[e.NewIndex].TInvoiceItemId = Id;
				colInventoryRecords.ListChanged += new ListChangedEventHandler(colInventoryRecords_ListChanged);
            }
		}
		private Wcss.InvoiceItemCollection colChildInvoiceItemRecords;
		public Wcss.InvoiceItemCollection ChildInvoiceItemRecords()
		{
			if(colChildInvoiceItemRecords == null)
			{
				colChildInvoiceItemRecords = new Wcss.InvoiceItemCollection().Where(InvoiceItem.Columns.TShipItemId, Id).Load();
				colChildInvoiceItemRecords.ListChanged += new ListChangedEventHandler(colChildInvoiceItemRecords_ListChanged);
			}
			return colChildInvoiceItemRecords;
		}
				
		void colChildInvoiceItemRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colChildInvoiceItemRecords[e.NewIndex].TShipItemId = Id;
				colChildInvoiceItemRecords.ListChanged += new ListChangedEventHandler(colChildInvoiceItemRecords_ListChanged);
            }
		}
		private Wcss.InvoiceItemPostPurchaseTextCollection colInvoiceItemPostPurchaseTextRecords;
		public Wcss.InvoiceItemPostPurchaseTextCollection InvoiceItemPostPurchaseTextRecords()
		{
			if(colInvoiceItemPostPurchaseTextRecords == null)
			{
				colInvoiceItemPostPurchaseTextRecords = new Wcss.InvoiceItemPostPurchaseTextCollection().Where(InvoiceItemPostPurchaseText.Columns.TInvoiceItemId, Id).Load();
				colInvoiceItemPostPurchaseTextRecords.ListChanged += new ListChangedEventHandler(colInvoiceItemPostPurchaseTextRecords_ListChanged);
			}
			return colInvoiceItemPostPurchaseTextRecords;
		}
				
		void colInvoiceItemPostPurchaseTextRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colInvoiceItemPostPurchaseTextRecords[e.NewIndex].TInvoiceItemId = Id;
				colInvoiceItemPostPurchaseTextRecords.ListChanged += new ListChangedEventHandler(colInvoiceItemPostPurchaseTextRecords_ListChanged);
            }
		}
		private Wcss.InvoiceShipmentCollection colInvoiceShipmentRecords;
		public Wcss.InvoiceShipmentCollection InvoiceShipmentRecords()
		{
			if(colInvoiceShipmentRecords == null)
			{
				colInvoiceShipmentRecords = new Wcss.InvoiceShipmentCollection().Where(InvoiceShipment.Columns.TShipItemId, Id).Load();
				colInvoiceShipmentRecords.ListChanged += new ListChangedEventHandler(colInvoiceShipmentRecords_ListChanged);
			}
			return colInvoiceShipmentRecords;
		}
				
		void colInvoiceShipmentRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colInvoiceShipmentRecords[e.NewIndex].TShipItemId = Id;
				colInvoiceShipmentRecords.ListChanged += new ListChangedEventHandler(colInvoiceShipmentRecords_ListChanged);
            }
		}
		private Wcss.InvoiceShipmentItemCollection colInvoiceShipmentItemRecords;
		public Wcss.InvoiceShipmentItemCollection InvoiceShipmentItemRecords()
		{
			if(colInvoiceShipmentItemRecords == null)
			{
				colInvoiceShipmentItemRecords = new Wcss.InvoiceShipmentItemCollection().Where(InvoiceShipmentItem.Columns.TInvoiceItemId, Id).Load();
				colInvoiceShipmentItemRecords.ListChanged += new ListChangedEventHandler(colInvoiceShipmentItemRecords_ListChanged);
			}
			return colInvoiceShipmentItemRecords;
		}
				
		void colInvoiceShipmentItemRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colInvoiceShipmentItemRecords[e.NewIndex].TInvoiceItemId = Id;
				colInvoiceShipmentItemRecords.ListChanged += new ListChangedEventHandler(colInvoiceShipmentItemRecords_ListChanged);
            }
		}
		#endregion
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a Invoice ActiveRecord object related to this InvoiceItem
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
		
		
		/// <summary>
		/// Returns a Merch ActiveRecord object related to this InvoiceItem
		/// 
		/// </summary>
		private Wcss.Merch Merch
		{
			get { return Wcss.Merch.FetchByID(this.TMerchId); }
			set { SetColumnValue("TMerchId", value.Id); }
		}
        //set up an alternate mechanism to avoid a database call
		private Wcss.Merch _merchrecord = null;
		
		public Wcss.Merch MerchRecord
		{
		    get
            {
                if (_merchrecord == null)
                {
                    _merchrecord = new Wcss.Merch();
                    _merchrecord.CopyFrom(this.Merch);
                }
                return _merchrecord;
            }
            set
            {
                if(value != null && _merchrecord == null)
			        _merchrecord = new Wcss.Merch();
                
                SetColumnValue("TMerchId", value.Id);
                _merchrecord.CopyFrom(value);                
            }
		}
		
		
		/// <summary>
		/// Returns a SalePromotion ActiveRecord object related to this InvoiceItem
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
		
		
		/// <summary>
		/// Returns a InvoiceItem ActiveRecord object related to this InvoiceItem
		/// 
		/// </summary>
		private Wcss.InvoiceItem ParentInvoiceItem
		{
			get { return Wcss.InvoiceItem.FetchByID(this.TShipItemId); }
			set { SetColumnValue("TShipItemId", value.Id); }
		}
        //set up an alternate mechanism to avoid a database call
		private Wcss.InvoiceItem _parentinvoiceitemrecord = null;
		
		public Wcss.InvoiceItem ParentInvoiceItemRecord
		{
		    get
            {
                if (_parentinvoiceitemrecord == null)
                {
                    _parentinvoiceitemrecord = new Wcss.InvoiceItem();
                    _parentinvoiceitemrecord.CopyFrom(this.ParentInvoiceItem);
                }
                return _parentinvoiceitemrecord;
            }
            set
            {
                if(value != null && _parentinvoiceitemrecord == null)
			        _parentinvoiceitemrecord = new Wcss.InvoiceItem();
                
                SetColumnValue("TShipItemId", value.Id);
                _parentinvoiceitemrecord.CopyFrom(value);                
            }
		}
		
		
		/// <summary>
		/// Returns a ShowTicket ActiveRecord object related to this InvoiceItem
		/// 
		/// </summary>
		private Wcss.ShowTicket ShowTicket
		{
			get { return Wcss.ShowTicket.FetchByID(this.TShowTicketId); }
			set { SetColumnValue("TShowTicketId", value.Id); }
		}
        //set up an alternate mechanism to avoid a database call
		private Wcss.ShowTicket _showticketrecord = null;
		
		public Wcss.ShowTicket ShowTicketRecord
		{
		    get
            {
                if (_showticketrecord == null)
                {
                    _showticketrecord = new Wcss.ShowTicket();
                    _showticketrecord.CopyFrom(this.ShowTicket);
                }
                return _showticketrecord;
            }
            set
            {
                if(value != null && _showticketrecord == null)
			        _showticketrecord = new Wcss.ShowTicket();
                
                SetColumnValue("TShowTicketId", value.Id);
                _showticketrecord.CopyFrom(value);                
            }
		}
		
		
		#endregion
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(Guid varGuid,int varTInvoiceId,string varVcContext,int? varTShowTicketId,int? varTMerchId,int? varTShowId,int? varTShipItemId,int? varTSalePromotionId,string varPurchaseName,DateTime? varDtDateOfShow,string varAgeDescription,string varMainActName,string varCriteria,string varDescription,decimal varMPrice,decimal varMServiceCharge,decimal varMAdjustment,decimal? varMPricePerItem,int varIQuantity,decimal? varMLineItemTotal,string varPurchaseAction,string varNotes,string varPickupName,bool? varBRTS,DateTime? varDtShipped,string varShippingNotes,string varShippingMethod,DateTime varDtStamp)
		{
			InvoiceItem item = new InvoiceItem();
			
			item.Guid = varGuid;
			
			item.TInvoiceId = varTInvoiceId;
			
			item.VcContext = varVcContext;
			
			item.TShowTicketId = varTShowTicketId;
			
			item.TMerchId = varTMerchId;
			
			item.TShowId = varTShowId;
			
			item.TShipItemId = varTShipItemId;
			
			item.TSalePromotionId = varTSalePromotionId;
			
			item.PurchaseName = varPurchaseName;
			
			item.DtDateOfShow = varDtDateOfShow;
			
			item.AgeDescription = varAgeDescription;
			
			item.MainActName = varMainActName;
			
			item.Criteria = varCriteria;
			
			item.Description = varDescription;
			
			item.MPrice = varMPrice;
			
			item.MServiceCharge = varMServiceCharge;
			
			item.MAdjustment = varMAdjustment;
			
			item.MPricePerItem = varMPricePerItem;
			
			item.IQuantity = varIQuantity;
			
			item.MLineItemTotal = varMLineItemTotal;
			
			item.PurchaseAction = varPurchaseAction;
			
			item.Notes = varNotes;
			
			item.PickupName = varPickupName;
			
			item.BRTS = varBRTS;
			
			item.DtShipped = varDtShipped;
			
			item.ShippingNotes = varShippingNotes;
			
			item.ShippingMethod = varShippingMethod;
			
			item.DtStamp = varDtStamp;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varId,Guid varGuid,int varTInvoiceId,string varVcContext,int? varTShowTicketId,int? varTMerchId,int? varTShowId,int? varTShipItemId,int? varTSalePromotionId,string varPurchaseName,DateTime? varDtDateOfShow,string varAgeDescription,string varMainActName,string varCriteria,string varDescription,decimal varMPrice,decimal varMServiceCharge,decimal varMAdjustment,decimal? varMPricePerItem,int varIQuantity,decimal? varMLineItemTotal,string varPurchaseAction,string varNotes,string varPickupName,bool? varBRTS,DateTime? varDtShipped,string varShippingNotes,string varShippingMethod,DateTime varDtStamp)
		{
			InvoiceItem item = new InvoiceItem();
			
				item.Id = varId;
			
				item.Guid = varGuid;
			
				item.TInvoiceId = varTInvoiceId;
			
				item.VcContext = varVcContext;
			
				item.TShowTicketId = varTShowTicketId;
			
				item.TMerchId = varTMerchId;
			
				item.TShowId = varTShowId;
			
				item.TShipItemId = varTShipItemId;
			
				item.TSalePromotionId = varTSalePromotionId;
			
				item.PurchaseName = varPurchaseName;
			
				item.DtDateOfShow = varDtDateOfShow;
			
				item.AgeDescription = varAgeDescription;
			
				item.MainActName = varMainActName;
			
				item.Criteria = varCriteria;
			
				item.Description = varDescription;
			
				item.MPrice = varMPrice;
			
				item.MServiceCharge = varMServiceCharge;
			
				item.MAdjustment = varMAdjustment;
			
				item.MPricePerItem = varMPricePerItem;
			
				item.IQuantity = varIQuantity;
			
				item.MLineItemTotal = varMLineItemTotal;
			
				item.PurchaseAction = varPurchaseAction;
			
				item.Notes = varNotes;
			
				item.PickupName = varPickupName;
			
				item.BRTS = varBRTS;
			
				item.DtShipped = varDtShipped;
			
				item.ShippingNotes = varShippingNotes;
			
				item.ShippingMethod = varShippingMethod;
			
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
        
        
        
        public static TableSchema.TableColumn GuidColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn TInvoiceIdColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn VcContextColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn TShowTicketIdColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn TMerchIdColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn TShowIdColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn TShipItemIdColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn TSalePromotionIdColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn PurchaseNameColumn
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        public static TableSchema.TableColumn DtDateOfShowColumn
        {
            get { return Schema.Columns[10]; }
        }
        
        
        
        public static TableSchema.TableColumn AgeDescriptionColumn
        {
            get { return Schema.Columns[11]; }
        }
        
        
        
        public static TableSchema.TableColumn MainActNameColumn
        {
            get { return Schema.Columns[12]; }
        }
        
        
        
        public static TableSchema.TableColumn CriteriaColumn
        {
            get { return Schema.Columns[13]; }
        }
        
        
        
        public static TableSchema.TableColumn DescriptionColumn
        {
            get { return Schema.Columns[14]; }
        }
        
        
        
        public static TableSchema.TableColumn MPriceColumn
        {
            get { return Schema.Columns[15]; }
        }
        
        
        
        public static TableSchema.TableColumn MServiceChargeColumn
        {
            get { return Schema.Columns[16]; }
        }
        
        
        
        public static TableSchema.TableColumn MAdjustmentColumn
        {
            get { return Schema.Columns[17]; }
        }
        
        
        
        public static TableSchema.TableColumn MPricePerItemColumn
        {
            get { return Schema.Columns[18]; }
        }
        
        
        
        public static TableSchema.TableColumn IQuantityColumn
        {
            get { return Schema.Columns[19]; }
        }
        
        
        
        public static TableSchema.TableColumn MLineItemTotalColumn
        {
            get { return Schema.Columns[20]; }
        }
        
        
        
        public static TableSchema.TableColumn PurchaseActionColumn
        {
            get { return Schema.Columns[21]; }
        }
        
        
        
        public static TableSchema.TableColumn NotesColumn
        {
            get { return Schema.Columns[22]; }
        }
        
        
        
        public static TableSchema.TableColumn PickupNameColumn
        {
            get { return Schema.Columns[23]; }
        }
        
        
        
        public static TableSchema.TableColumn BRTSColumn
        {
            get { return Schema.Columns[24]; }
        }
        
        
        
        public static TableSchema.TableColumn DtShippedColumn
        {
            get { return Schema.Columns[25]; }
        }
        
        
        
        public static TableSchema.TableColumn ShippingNotesColumn
        {
            get { return Schema.Columns[26]; }
        }
        
        
        
        public static TableSchema.TableColumn ShippingMethodColumn
        {
            get { return Schema.Columns[27]; }
        }
        
        
        
        public static TableSchema.TableColumn DtStampColumn
        {
            get { return Schema.Columns[28]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Id = @"Id";
			 public static string Guid = @"Guid";
			 public static string TInvoiceId = @"TInvoiceId";
			 public static string VcContext = @"vcContext";
			 public static string TShowTicketId = @"TShowTicketId";
			 public static string TMerchId = @"TMerchId";
			 public static string TShowId = @"TShowId";
			 public static string TShipItemId = @"TShipItemId";
			 public static string TSalePromotionId = @"TSalePromotionId";
			 public static string PurchaseName = @"PurchaseName";
			 public static string DtDateOfShow = @"dtDateOfShow";
			 public static string AgeDescription = @"AgeDescription";
			 public static string MainActName = @"MainActName";
			 public static string Criteria = @"Criteria";
			 public static string Description = @"Description";
			 public static string MPrice = @"mPrice";
			 public static string MServiceCharge = @"mServiceCharge";
			 public static string MAdjustment = @"mAdjustment";
			 public static string MPricePerItem = @"mPricePerItem";
			 public static string IQuantity = @"iQuantity";
			 public static string MLineItemTotal = @"mLineItemTotal";
			 public static string PurchaseAction = @"PurchaseAction";
			 public static string Notes = @"Notes";
			 public static string PickupName = @"PickupName";
			 public static string BRTS = @"bRTS";
			 public static string DtShipped = @"dtShipped";
			 public static string ShippingNotes = @"ShippingNotes";
			 public static string ShippingMethod = @"ShippingMethod";
			 public static string DtStamp = @"dtStamp";
						
		}
		#endregion
		
		#region Update PK Collections
		
        public void SetPKValues()
        {
                if (colCharitableContributionRecords != null)
                {
                    foreach (Wcss.CharitableContribution item in colCharitableContributionRecords)
                    {
                        if (item.TInvoiceItemId != Id)
                        {
                            item.TInvoiceItemId = Id;
                        }
                    }
               }
		
                if (colInventoryRecords != null)
                {
                    foreach (Wcss.Inventory item in colInventoryRecords)
                    {
                        if (item.TInvoiceItemId != Id)
                        {
                            item.TInvoiceItemId = Id;
                        }
                    }
               }
		
                if (colChildInvoiceItemRecords != null)
                {
                    foreach (Wcss.InvoiceItem item in colChildInvoiceItemRecords)
                    {
                        if (item.TShipItemId != Id)
                        {
                            item.TShipItemId = Id;
                        }
                    }
               }
		
                if (colInvoiceItemPostPurchaseTextRecords != null)
                {
                    foreach (Wcss.InvoiceItemPostPurchaseText item in colInvoiceItemPostPurchaseTextRecords)
                    {
                        if (item.TInvoiceItemId != Id)
                        {
                            item.TInvoiceItemId = Id;
                        }
                    }
               }
		
                if (colInvoiceShipmentRecords != null)
                {
                    foreach (Wcss.InvoiceShipment item in colInvoiceShipmentRecords)
                    {
                        if (item.TShipItemId != Id)
                        {
                            item.TShipItemId = Id;
                        }
                    }
               }
		
                if (colInvoiceShipmentItemRecords != null)
                {
                    foreach (Wcss.InvoiceShipmentItem item in colInvoiceShipmentItemRecords)
                    {
                        if (item.TInvoiceItemId != Id)
                        {
                            item.TInvoiceItemId = Id;
                        }
                    }
               }
		}
        #endregion
    
        #region Deep Save
		
        public void DeepSave()
        {
            Save();
            
                if (colCharitableContributionRecords != null)
                {
                    colCharitableContributionRecords.SaveAll();
               }
		
                if (colInventoryRecords != null)
                {
                    colInventoryRecords.SaveAll();
               }
		
                if (colChildInvoiceItemRecords != null)
                {
                    colChildInvoiceItemRecords.SaveAll();
               }
		
                if (colInvoiceItemPostPurchaseTextRecords != null)
                {
                    colInvoiceItemPostPurchaseTextRecords.SaveAll();
               }
		
                if (colInvoiceShipmentRecords != null)
                {
                    colInvoiceShipmentRecords.SaveAll();
               }
		
                if (colInvoiceShipmentItemRecords != null)
                {
                    colInvoiceShipmentItemRecords.SaveAll();
               }
		}
        #endregion
	}
}
