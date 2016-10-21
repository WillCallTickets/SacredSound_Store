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
	/// Strongly-typed collection for the Merch class.
	/// </summary>
    [Serializable]
	public partial class MerchCollection : ActiveList<Merch, MerchCollection>
	{	   
		public MerchCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>MerchCollection</returns>
		public MerchCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                Merch o = this[i];
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
	/// This is an ActiveRecord class which wraps the Merch table.
	/// </summary>
	[Serializable]
	public partial class Merch : ActiveRecord<Merch>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public Merch()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public Merch(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public Merch(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public Merch(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("Merch", TableType.Table, DataService.GetInstance("WillCall"));
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
				
				TableSchema.TableColumn colvarName = new TableSchema.TableColumn(schema);
				colvarName.ColumnName = "Name";
				colvarName.DataType = DbType.AnsiString;
				colvarName.MaxLength = 256;
				colvarName.AutoIncrement = false;
				colvarName.IsNullable = false;
				colvarName.IsPrimaryKey = false;
				colvarName.IsForeignKey = false;
				colvarName.IsReadOnly = false;
				colvarName.DefaultSetting = @"";
				colvarName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarName);
				
				TableSchema.TableColumn colvarStyle = new TableSchema.TableColumn(schema);
				colvarStyle.ColumnName = "Style";
				colvarStyle.DataType = DbType.AnsiString;
				colvarStyle.MaxLength = 256;
				colvarStyle.AutoIncrement = false;
				colvarStyle.IsNullable = true;
				colvarStyle.IsPrimaryKey = false;
				colvarStyle.IsForeignKey = false;
				colvarStyle.IsReadOnly = false;
				colvarStyle.DefaultSetting = @"";
				colvarStyle.ForeignKeyTableName = "";
				schema.Columns.Add(colvarStyle);
				
				TableSchema.TableColumn colvarColor = new TableSchema.TableColumn(schema);
				colvarColor.ColumnName = "Color";
				colvarColor.DataType = DbType.AnsiString;
				colvarColor.MaxLength = 256;
				colvarColor.AutoIncrement = false;
				colvarColor.IsNullable = true;
				colvarColor.IsPrimaryKey = false;
				colvarColor.IsForeignKey = false;
				colvarColor.IsReadOnly = false;
				colvarColor.DefaultSetting = @"";
				colvarColor.ForeignKeyTableName = "";
				schema.Columns.Add(colvarColor);
				
				TableSchema.TableColumn colvarSize = new TableSchema.TableColumn(schema);
				colvarSize.ColumnName = "Size";
				colvarSize.DataType = DbType.AnsiString;
				colvarSize.MaxLength = 256;
				colvarSize.AutoIncrement = false;
				colvarSize.IsNullable = true;
				colvarSize.IsPrimaryKey = false;
				colvarSize.IsForeignKey = false;
				colvarSize.IsReadOnly = false;
				colvarSize.DefaultSetting = @"";
				colvarSize.ForeignKeyTableName = "";
				schema.Columns.Add(colvarSize);
				
				TableSchema.TableColumn colvarTParentListing = new TableSchema.TableColumn(schema);
				colvarTParentListing.ColumnName = "tParentListing";
				colvarTParentListing.DataType = DbType.Int32;
				colvarTParentListing.MaxLength = 0;
				colvarTParentListing.AutoIncrement = false;
				colvarTParentListing.IsNullable = true;
				colvarTParentListing.IsPrimaryKey = false;
				colvarTParentListing.IsForeignKey = true;
				colvarTParentListing.IsReadOnly = false;
				colvarTParentListing.DefaultSetting = @"";
				
					colvarTParentListing.ForeignKeyTableName = "Merch";
				schema.Columns.Add(colvarTParentListing);
				
				TableSchema.TableColumn colvarBActive = new TableSchema.TableColumn(schema);
				colvarBActive.ColumnName = "bActive";
				colvarBActive.DataType = DbType.Boolean;
				colvarBActive.MaxLength = 0;
				colvarBActive.AutoIncrement = false;
				colvarBActive.IsNullable = false;
				colvarBActive.IsPrimaryKey = false;
				colvarBActive.IsForeignKey = false;
				colvarBActive.IsReadOnly = false;
				
						colvarBActive.DefaultSetting = @"((1))";
				colvarBActive.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBActive);
				
				TableSchema.TableColumn colvarBInternalOnly = new TableSchema.TableColumn(schema);
				colvarBInternalOnly.ColumnName = "bInternalOnly";
				colvarBInternalOnly.DataType = DbType.Boolean;
				colvarBInternalOnly.MaxLength = 0;
				colvarBInternalOnly.AutoIncrement = false;
				colvarBInternalOnly.IsNullable = false;
				colvarBInternalOnly.IsPrimaryKey = false;
				colvarBInternalOnly.IsForeignKey = false;
				colvarBInternalOnly.IsReadOnly = false;
				
						colvarBInternalOnly.DefaultSetting = @"((0))";
				colvarBInternalOnly.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBInternalOnly);
				
				TableSchema.TableColumn colvarBTaxable = new TableSchema.TableColumn(schema);
				colvarBTaxable.ColumnName = "bTaxable";
				colvarBTaxable.DataType = DbType.Boolean;
				colvarBTaxable.MaxLength = 0;
				colvarBTaxable.AutoIncrement = false;
				colvarBTaxable.IsNullable = false;
				colvarBTaxable.IsPrimaryKey = false;
				colvarBTaxable.IsForeignKey = false;
				colvarBTaxable.IsReadOnly = false;
				
						colvarBTaxable.DefaultSetting = @"((0))";
				colvarBTaxable.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBTaxable);
				
				TableSchema.TableColumn colvarBFeaturedItem = new TableSchema.TableColumn(schema);
				colvarBFeaturedItem.ColumnName = "bFeaturedItem";
				colvarBFeaturedItem.DataType = DbType.Boolean;
				colvarBFeaturedItem.MaxLength = 0;
				colvarBFeaturedItem.AutoIncrement = false;
				colvarBFeaturedItem.IsNullable = false;
				colvarBFeaturedItem.IsPrimaryKey = false;
				colvarBFeaturedItem.IsForeignKey = false;
				colvarBFeaturedItem.IsReadOnly = false;
				
						colvarBFeaturedItem.DefaultSetting = @"((0))";
				colvarBFeaturedItem.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBFeaturedItem);
				
				TableSchema.TableColumn colvarShortText = new TableSchema.TableColumn(schema);
				colvarShortText.ColumnName = "ShortText";
				colvarShortText.DataType = DbType.AnsiString;
				colvarShortText.MaxLength = 300;
				colvarShortText.AutoIncrement = false;
				colvarShortText.IsNullable = true;
				colvarShortText.IsPrimaryKey = false;
				colvarShortText.IsForeignKey = false;
				colvarShortText.IsReadOnly = false;
				colvarShortText.DefaultSetting = @"";
				colvarShortText.ForeignKeyTableName = "";
				schema.Columns.Add(colvarShortText);
				
				TableSchema.TableColumn colvarVcDisplayTemplate = new TableSchema.TableColumn(schema);
				colvarVcDisplayTemplate.ColumnName = "vcDisplayTemplate";
				colvarVcDisplayTemplate.DataType = DbType.AnsiString;
				colvarVcDisplayTemplate.MaxLength = 50;
				colvarVcDisplayTemplate.AutoIncrement = false;
				colvarVcDisplayTemplate.IsNullable = true;
				colvarVcDisplayTemplate.IsPrimaryKey = false;
				colvarVcDisplayTemplate.IsForeignKey = false;
				colvarVcDisplayTemplate.IsReadOnly = false;
				colvarVcDisplayTemplate.DefaultSetting = @"";
				colvarVcDisplayTemplate.ForeignKeyTableName = "";
				schema.Columns.Add(colvarVcDisplayTemplate);
				
				TableSchema.TableColumn colvarDescription = new TableSchema.TableColumn(schema);
				colvarDescription.ColumnName = "Description";
				colvarDescription.DataType = DbType.AnsiString;
				colvarDescription.MaxLength = -1;
				colvarDescription.AutoIncrement = false;
				colvarDescription.IsNullable = true;
				colvarDescription.IsPrimaryKey = false;
				colvarDescription.IsForeignKey = false;
				colvarDescription.IsReadOnly = false;
				colvarDescription.DefaultSetting = @"";
				colvarDescription.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDescription);
				
				TableSchema.TableColumn colvarBUnlockActive = new TableSchema.TableColumn(schema);
				colvarBUnlockActive.ColumnName = "bUnlockActive";
				colvarBUnlockActive.DataType = DbType.Boolean;
				colvarBUnlockActive.MaxLength = 0;
				colvarBUnlockActive.AutoIncrement = false;
				colvarBUnlockActive.IsNullable = false;
				colvarBUnlockActive.IsPrimaryKey = false;
				colvarBUnlockActive.IsForeignKey = false;
				colvarBUnlockActive.IsReadOnly = false;
				
						colvarBUnlockActive.DefaultSetting = @"((0))";
				colvarBUnlockActive.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBUnlockActive);
				
				TableSchema.TableColumn colvarUnlockCode = new TableSchema.TableColumn(schema);
				colvarUnlockCode.ColumnName = "UnlockCode";
				colvarUnlockCode.DataType = DbType.AnsiString;
				colvarUnlockCode.MaxLength = 256;
				colvarUnlockCode.AutoIncrement = false;
				colvarUnlockCode.IsNullable = true;
				colvarUnlockCode.IsPrimaryKey = false;
				colvarUnlockCode.IsForeignKey = false;
				colvarUnlockCode.IsReadOnly = false;
				colvarUnlockCode.DefaultSetting = @"";
				colvarUnlockCode.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUnlockCode);
				
				TableSchema.TableColumn colvarDtUnlockDate = new TableSchema.TableColumn(schema);
				colvarDtUnlockDate.ColumnName = "dtUnlockDate";
				colvarDtUnlockDate.DataType = DbType.DateTime;
				colvarDtUnlockDate.MaxLength = 0;
				colvarDtUnlockDate.AutoIncrement = false;
				colvarDtUnlockDate.IsNullable = true;
				colvarDtUnlockDate.IsPrimaryKey = false;
				colvarDtUnlockDate.IsForeignKey = false;
				colvarDtUnlockDate.IsReadOnly = false;
				colvarDtUnlockDate.DefaultSetting = @"";
				colvarDtUnlockDate.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDtUnlockDate);
				
				TableSchema.TableColumn colvarDtUnlockEndDate = new TableSchema.TableColumn(schema);
				colvarDtUnlockEndDate.ColumnName = "dtUnlockEndDate";
				colvarDtUnlockEndDate.DataType = DbType.DateTime;
				colvarDtUnlockEndDate.MaxLength = 0;
				colvarDtUnlockEndDate.AutoIncrement = false;
				colvarDtUnlockEndDate.IsNullable = true;
				colvarDtUnlockEndDate.IsPrimaryKey = false;
				colvarDtUnlockEndDate.IsForeignKey = false;
				colvarDtUnlockEndDate.IsReadOnly = false;
				colvarDtUnlockEndDate.DefaultSetting = @"";
				colvarDtUnlockEndDate.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDtUnlockEndDate);
				
				TableSchema.TableColumn colvarDtStartDate = new TableSchema.TableColumn(schema);
				colvarDtStartDate.ColumnName = "dtStartDate";
				colvarDtStartDate.DataType = DbType.DateTime;
				colvarDtStartDate.MaxLength = 0;
				colvarDtStartDate.AutoIncrement = false;
				colvarDtStartDate.IsNullable = true;
				colvarDtStartDate.IsPrimaryKey = false;
				colvarDtStartDate.IsForeignKey = false;
				colvarDtStartDate.IsReadOnly = false;
				colvarDtStartDate.DefaultSetting = @"";
				colvarDtStartDate.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDtStartDate);
				
				TableSchema.TableColumn colvarDtEndDate = new TableSchema.TableColumn(schema);
				colvarDtEndDate.ColumnName = "dtEndDate";
				colvarDtEndDate.DataType = DbType.DateTime;
				colvarDtEndDate.MaxLength = 0;
				colvarDtEndDate.AutoIncrement = false;
				colvarDtEndDate.IsNullable = true;
				colvarDtEndDate.IsPrimaryKey = false;
				colvarDtEndDate.IsForeignKey = false;
				colvarDtEndDate.IsReadOnly = false;
				colvarDtEndDate.DefaultSetting = @"";
				colvarDtEndDate.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDtEndDate);
				
				TableSchema.TableColumn colvarMPrice = new TableSchema.TableColumn(schema);
				colvarMPrice.ColumnName = "mPrice";
				colvarMPrice.DataType = DbType.Currency;
				colvarMPrice.MaxLength = 0;
				colvarMPrice.AutoIncrement = false;
				colvarMPrice.IsNullable = true;
				colvarMPrice.IsPrimaryKey = false;
				colvarMPrice.IsForeignKey = false;
				colvarMPrice.IsReadOnly = false;
				colvarMPrice.DefaultSetting = @"";
				colvarMPrice.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMPrice);
				
				TableSchema.TableColumn colvarBUseSalePrice = new TableSchema.TableColumn(schema);
				colvarBUseSalePrice.ColumnName = "bUseSalePrice";
				colvarBUseSalePrice.DataType = DbType.Boolean;
				colvarBUseSalePrice.MaxLength = 0;
				colvarBUseSalePrice.AutoIncrement = false;
				colvarBUseSalePrice.IsNullable = true;
				colvarBUseSalePrice.IsPrimaryKey = false;
				colvarBUseSalePrice.IsForeignKey = false;
				colvarBUseSalePrice.IsReadOnly = false;
				
						colvarBUseSalePrice.DefaultSetting = @"((0))";
				colvarBUseSalePrice.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBUseSalePrice);
				
				TableSchema.TableColumn colvarMSalePrice = new TableSchema.TableColumn(schema);
				colvarMSalePrice.ColumnName = "mSalePrice";
				colvarMSalePrice.DataType = DbType.Currency;
				colvarMSalePrice.MaxLength = 0;
				colvarMSalePrice.AutoIncrement = false;
				colvarMSalePrice.IsNullable = true;
				colvarMSalePrice.IsPrimaryKey = false;
				colvarMSalePrice.IsForeignKey = false;
				colvarMSalePrice.IsReadOnly = false;
				
						colvarMSalePrice.DefaultSetting = @"((0))";
				colvarMSalePrice.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMSalePrice);
				
				TableSchema.TableColumn colvarVcDeliveryType = new TableSchema.TableColumn(schema);
				colvarVcDeliveryType.ColumnName = "vcDeliveryType";
				colvarVcDeliveryType.DataType = DbType.AnsiString;
				colvarVcDeliveryType.MaxLength = 50;
				colvarVcDeliveryType.AutoIncrement = false;
				colvarVcDeliveryType.IsNullable = true;
				colvarVcDeliveryType.IsPrimaryKey = false;
				colvarVcDeliveryType.IsForeignKey = false;
				colvarVcDeliveryType.IsReadOnly = false;
				colvarVcDeliveryType.DefaultSetting = @"";
				colvarVcDeliveryType.ForeignKeyTableName = "";
				schema.Columns.Add(colvarVcDeliveryType);
				
				TableSchema.TableColumn colvarBLowRateQualified = new TableSchema.TableColumn(schema);
				colvarBLowRateQualified.ColumnName = "bLowRateQualified";
				colvarBLowRateQualified.DataType = DbType.Boolean;
				colvarBLowRateQualified.MaxLength = 0;
				colvarBLowRateQualified.AutoIncrement = false;
				colvarBLowRateQualified.IsNullable = true;
				colvarBLowRateQualified.IsPrimaryKey = false;
				colvarBLowRateQualified.IsForeignKey = false;
				colvarBLowRateQualified.IsReadOnly = false;
				colvarBLowRateQualified.DefaultSetting = @"";
				colvarBLowRateQualified.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBLowRateQualified);
				
				TableSchema.TableColumn colvarMWeight = new TableSchema.TableColumn(schema);
				colvarMWeight.ColumnName = "mWeight";
				colvarMWeight.DataType = DbType.Currency;
				colvarMWeight.MaxLength = 0;
				colvarMWeight.AutoIncrement = false;
				colvarMWeight.IsNullable = true;
				colvarMWeight.IsPrimaryKey = false;
				colvarMWeight.IsForeignKey = false;
				colvarMWeight.IsReadOnly = false;
				colvarMWeight.DefaultSetting = @"";
				colvarMWeight.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMWeight);
				
				TableSchema.TableColumn colvarMFlatShip = new TableSchema.TableColumn(schema);
				colvarMFlatShip.ColumnName = "mFlatShip";
				colvarMFlatShip.DataType = DbType.Currency;
				colvarMFlatShip.MaxLength = 0;
				colvarMFlatShip.AutoIncrement = false;
				colvarMFlatShip.IsNullable = true;
				colvarMFlatShip.IsPrimaryKey = false;
				colvarMFlatShip.IsForeignKey = false;
				colvarMFlatShip.IsReadOnly = false;
				colvarMFlatShip.DefaultSetting = @"";
				colvarMFlatShip.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMFlatShip);
				
				TableSchema.TableColumn colvarVcFlatMethod = new TableSchema.TableColumn(schema);
				colvarVcFlatMethod.ColumnName = "vcFlatMethod";
				colvarVcFlatMethod.DataType = DbType.AnsiString;
				colvarVcFlatMethod.MaxLength = 256;
				colvarVcFlatMethod.AutoIncrement = false;
				colvarVcFlatMethod.IsNullable = true;
				colvarVcFlatMethod.IsPrimaryKey = false;
				colvarVcFlatMethod.IsForeignKey = false;
				colvarVcFlatMethod.IsReadOnly = false;
				colvarVcFlatMethod.DefaultSetting = @"";
				colvarVcFlatMethod.ForeignKeyTableName = "";
				schema.Columns.Add(colvarVcFlatMethod);
				
				TableSchema.TableColumn colvarDtBackorder = new TableSchema.TableColumn(schema);
				colvarDtBackorder.ColumnName = "dtBackorder";
				colvarDtBackorder.DataType = DbType.DateTime;
				colvarDtBackorder.MaxLength = 0;
				colvarDtBackorder.AutoIncrement = false;
				colvarDtBackorder.IsNullable = true;
				colvarDtBackorder.IsPrimaryKey = false;
				colvarDtBackorder.IsForeignKey = false;
				colvarDtBackorder.IsReadOnly = false;
				colvarDtBackorder.DefaultSetting = @"";
				colvarDtBackorder.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDtBackorder);
				
				TableSchema.TableColumn colvarBShipSeparate = new TableSchema.TableColumn(schema);
				colvarBShipSeparate.ColumnName = "bShipSeparate";
				colvarBShipSeparate.DataType = DbType.Boolean;
				colvarBShipSeparate.MaxLength = 0;
				colvarBShipSeparate.AutoIncrement = false;
				colvarBShipSeparate.IsNullable = true;
				colvarBShipSeparate.IsPrimaryKey = false;
				colvarBShipSeparate.IsForeignKey = false;
				colvarBShipSeparate.IsReadOnly = false;
				
						colvarBShipSeparate.DefaultSetting = @"((0))";
				colvarBShipSeparate.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBShipSeparate);
				
				TableSchema.TableColumn colvarBSoldOut = new TableSchema.TableColumn(schema);
				colvarBSoldOut.ColumnName = "bSoldOut";
				colvarBSoldOut.DataType = DbType.Boolean;
				colvarBSoldOut.MaxLength = 0;
				colvarBSoldOut.AutoIncrement = false;
				colvarBSoldOut.IsNullable = true;
				colvarBSoldOut.IsPrimaryKey = false;
				colvarBSoldOut.IsForeignKey = false;
				colvarBSoldOut.IsReadOnly = false;
				
						colvarBSoldOut.DefaultSetting = @"((0))";
				colvarBSoldOut.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBSoldOut);
				
				TableSchema.TableColumn colvarIMaxQtyPerOrder = new TableSchema.TableColumn(schema);
				colvarIMaxQtyPerOrder.ColumnName = "iMaxQtyPerOrder";
				colvarIMaxQtyPerOrder.DataType = DbType.Int32;
				colvarIMaxQtyPerOrder.MaxLength = 0;
				colvarIMaxQtyPerOrder.AutoIncrement = false;
				colvarIMaxQtyPerOrder.IsNullable = false;
				colvarIMaxQtyPerOrder.IsPrimaryKey = false;
				colvarIMaxQtyPerOrder.IsForeignKey = false;
				colvarIMaxQtyPerOrder.IsReadOnly = false;
				
						colvarIMaxQtyPerOrder.DefaultSetting = @"((8))";
				colvarIMaxQtyPerOrder.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIMaxQtyPerOrder);
				
				TableSchema.TableColumn colvarIAllotment = new TableSchema.TableColumn(schema);
				colvarIAllotment.ColumnName = "iAllotment";
				colvarIAllotment.DataType = DbType.Int32;
				colvarIAllotment.MaxLength = 0;
				colvarIAllotment.AutoIncrement = false;
				colvarIAllotment.IsNullable = false;
				colvarIAllotment.IsPrimaryKey = false;
				colvarIAllotment.IsForeignKey = false;
				colvarIAllotment.IsReadOnly = false;
				
						colvarIAllotment.DefaultSetting = @"((0))";
				colvarIAllotment.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIAllotment);
				
				TableSchema.TableColumn colvarIDamaged = new TableSchema.TableColumn(schema);
				colvarIDamaged.ColumnName = "iDamaged";
				colvarIDamaged.DataType = DbType.Int32;
				colvarIDamaged.MaxLength = 0;
				colvarIDamaged.AutoIncrement = false;
				colvarIDamaged.IsNullable = false;
				colvarIDamaged.IsPrimaryKey = false;
				colvarIDamaged.IsForeignKey = false;
				colvarIDamaged.IsReadOnly = false;
				
						colvarIDamaged.DefaultSetting = @"((0))";
				colvarIDamaged.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIDamaged);
				
				TableSchema.TableColumn colvarIPending = new TableSchema.TableColumn(schema);
				colvarIPending.ColumnName = "iPending";
				colvarIPending.DataType = DbType.Int32;
				colvarIPending.MaxLength = 0;
				colvarIPending.AutoIncrement = false;
				colvarIPending.IsNullable = false;
				colvarIPending.IsPrimaryKey = false;
				colvarIPending.IsForeignKey = false;
				colvarIPending.IsReadOnly = false;
				
						colvarIPending.DefaultSetting = @"((0))";
				colvarIPending.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIPending);
				
				TableSchema.TableColumn colvarISold = new TableSchema.TableColumn(schema);
				colvarISold.ColumnName = "iSold";
				colvarISold.DataType = DbType.Int32;
				colvarISold.MaxLength = 0;
				colvarISold.AutoIncrement = false;
				colvarISold.IsNullable = false;
				colvarISold.IsPrimaryKey = false;
				colvarISold.IsForeignKey = false;
				colvarISold.IsReadOnly = false;
				
						colvarISold.DefaultSetting = @"((0))";
				colvarISold.ForeignKeyTableName = "";
				schema.Columns.Add(colvarISold);
				
				TableSchema.TableColumn colvarIAvailable = new TableSchema.TableColumn(schema);
				colvarIAvailable.ColumnName = "iAvailable";
				colvarIAvailable.DataType = DbType.Int32;
				colvarIAvailable.MaxLength = 0;
				colvarIAvailable.AutoIncrement = false;
				colvarIAvailable.IsNullable = true;
				colvarIAvailable.IsPrimaryKey = false;
				colvarIAvailable.IsForeignKey = false;
				colvarIAvailable.IsReadOnly = true;
				colvarIAvailable.DefaultSetting = @"";
				colvarIAvailable.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIAvailable);
				
				TableSchema.TableColumn colvarIRefunded = new TableSchema.TableColumn(schema);
				colvarIRefunded.ColumnName = "iRefunded";
				colvarIRefunded.DataType = DbType.Int32;
				colvarIRefunded.MaxLength = 0;
				colvarIRefunded.AutoIncrement = false;
				colvarIRefunded.IsNullable = false;
				colvarIRefunded.IsPrimaryKey = false;
				colvarIRefunded.IsForeignKey = false;
				colvarIRefunded.IsReadOnly = false;
				
						colvarIRefunded.DefaultSetting = @"((0))";
				colvarIRefunded.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIRefunded);
				
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
				DataService.Providers["WillCall"].AddSchema("Merch",schema);
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
		  
		[XmlAttribute("Name")]
		[Bindable(true)]
		public string Name 
		{
			get { return GetColumnValue<string>(Columns.Name); }
			set { SetColumnValue(Columns.Name, value); }
		}
		  
		[XmlAttribute("Style")]
		[Bindable(true)]
		public string Style 
		{
			get { return GetColumnValue<string>(Columns.Style); }
			set { SetColumnValue(Columns.Style, value); }
		}
		  
		[XmlAttribute("Color")]
		[Bindable(true)]
		public string Color 
		{
			get { return GetColumnValue<string>(Columns.Color); }
			set { SetColumnValue(Columns.Color, value); }
		}
		  
		[XmlAttribute("Size")]
		[Bindable(true)]
		public string Size 
		{
			get { return GetColumnValue<string>(Columns.Size); }
			set { SetColumnValue(Columns.Size, value); }
		}
		  
		[XmlAttribute("TParentListing")]
		[Bindable(true)]
		public int? TParentListing 
		{
			get { return GetColumnValue<int?>(Columns.TParentListing); }
			set { SetColumnValue(Columns.TParentListing, value); }
		}
		  
		[XmlAttribute("BActive")]
		[Bindable(true)]
		public bool BActive 
		{
			get { return GetColumnValue<bool>(Columns.BActive); }
			set { SetColumnValue(Columns.BActive, value); }
		}
		  
		[XmlAttribute("BInternalOnly")]
		[Bindable(true)]
		public bool BInternalOnly 
		{
			get { return GetColumnValue<bool>(Columns.BInternalOnly); }
			set { SetColumnValue(Columns.BInternalOnly, value); }
		}
		  
		[XmlAttribute("BTaxable")]
		[Bindable(true)]
		public bool BTaxable 
		{
			get { return GetColumnValue<bool>(Columns.BTaxable); }
			set { SetColumnValue(Columns.BTaxable, value); }
		}
		  
		[XmlAttribute("BFeaturedItem")]
		[Bindable(true)]
		public bool BFeaturedItem 
		{
			get { return GetColumnValue<bool>(Columns.BFeaturedItem); }
			set { SetColumnValue(Columns.BFeaturedItem, value); }
		}
		  
		[XmlAttribute("ShortText")]
		[Bindable(true)]
		public string ShortText 
		{
			get { return GetColumnValue<string>(Columns.ShortText); }
			set { SetColumnValue(Columns.ShortText, value); }
		}
		  
		[XmlAttribute("VcDisplayTemplate")]
		[Bindable(true)]
		public string VcDisplayTemplate 
		{
			get { return GetColumnValue<string>(Columns.VcDisplayTemplate); }
			set { SetColumnValue(Columns.VcDisplayTemplate, value); }
		}
		  
		[XmlAttribute("Description")]
		[Bindable(true)]
		public string Description 
		{
			get { return GetColumnValue<string>(Columns.Description); }
			set { SetColumnValue(Columns.Description, value); }
		}
		  
		[XmlAttribute("BUnlockActive")]
		[Bindable(true)]
		public bool BUnlockActive 
		{
			get { return GetColumnValue<bool>(Columns.BUnlockActive); }
			set { SetColumnValue(Columns.BUnlockActive, value); }
		}
		  
		[XmlAttribute("UnlockCode")]
		[Bindable(true)]
		public string UnlockCode 
		{
			get { return GetColumnValue<string>(Columns.UnlockCode); }
			set { SetColumnValue(Columns.UnlockCode, value); }
		}
		  
		[XmlAttribute("DtUnlockDate")]
		[Bindable(true)]
		public DateTime? DtUnlockDate 
		{
			get { return GetColumnValue<DateTime?>(Columns.DtUnlockDate); }
			set { SetColumnValue(Columns.DtUnlockDate, value); }
		}
		  
		[XmlAttribute("DtUnlockEndDate")]
		[Bindable(true)]
		public DateTime? DtUnlockEndDate 
		{
			get { return GetColumnValue<DateTime?>(Columns.DtUnlockEndDate); }
			set { SetColumnValue(Columns.DtUnlockEndDate, value); }
		}
		  
		[XmlAttribute("DtStartDate")]
		[Bindable(true)]
		public DateTime? DtStartDate 
		{
			get { return GetColumnValue<DateTime?>(Columns.DtStartDate); }
			set { SetColumnValue(Columns.DtStartDate, value); }
		}
		  
		[XmlAttribute("DtEndDate")]
		[Bindable(true)]
		public DateTime? DtEndDate 
		{
			get { return GetColumnValue<DateTime?>(Columns.DtEndDate); }
			set { SetColumnValue(Columns.DtEndDate, value); }
		}
		  
		[XmlAttribute("MPrice")]
		[Bindable(true)]
		public decimal? MPrice 
		{
			get { return GetColumnValue<decimal?>(Columns.MPrice); }
			set { SetColumnValue(Columns.MPrice, value); }
		}
		  
		[XmlAttribute("BUseSalePrice")]
		[Bindable(true)]
		public bool? BUseSalePrice 
		{
			get { return GetColumnValue<bool?>(Columns.BUseSalePrice); }
			set { SetColumnValue(Columns.BUseSalePrice, value); }
		}
		  
		[XmlAttribute("MSalePrice")]
		[Bindable(true)]
		public decimal? MSalePrice 
		{
			get { return GetColumnValue<decimal?>(Columns.MSalePrice); }
			set { SetColumnValue(Columns.MSalePrice, value); }
		}
		  
		[XmlAttribute("VcDeliveryType")]
		[Bindable(true)]
		public string VcDeliveryType 
		{
			get { return GetColumnValue<string>(Columns.VcDeliveryType); }
			set { SetColumnValue(Columns.VcDeliveryType, value); }
		}
		  
		[XmlAttribute("BLowRateQualified")]
		[Bindable(true)]
		public bool? BLowRateQualified 
		{
			get { return GetColumnValue<bool?>(Columns.BLowRateQualified); }
			set { SetColumnValue(Columns.BLowRateQualified, value); }
		}
		  
		[XmlAttribute("MWeight")]
		[Bindable(true)]
		public decimal? MWeight 
		{
			get { return GetColumnValue<decimal?>(Columns.MWeight); }
			set { SetColumnValue(Columns.MWeight, value); }
		}
		  
		[XmlAttribute("MFlatShip")]
		[Bindable(true)]
		public decimal? MFlatShip 
		{
			get { return GetColumnValue<decimal?>(Columns.MFlatShip); }
			set { SetColumnValue(Columns.MFlatShip, value); }
		}
		  
		[XmlAttribute("VcFlatMethod")]
		[Bindable(true)]
		public string VcFlatMethod 
		{
			get { return GetColumnValue<string>(Columns.VcFlatMethod); }
			set { SetColumnValue(Columns.VcFlatMethod, value); }
		}
		  
		[XmlAttribute("DtBackorder")]
		[Bindable(true)]
		public DateTime? DtBackorder 
		{
			get { return GetColumnValue<DateTime?>(Columns.DtBackorder); }
			set { SetColumnValue(Columns.DtBackorder, value); }
		}
		  
		[XmlAttribute("BShipSeparate")]
		[Bindable(true)]
		public bool? BShipSeparate 
		{
			get { return GetColumnValue<bool?>(Columns.BShipSeparate); }
			set { SetColumnValue(Columns.BShipSeparate, value); }
		}
		  
		[XmlAttribute("BSoldOut")]
		[Bindable(true)]
		public bool? BSoldOut 
		{
			get { return GetColumnValue<bool?>(Columns.BSoldOut); }
			set { SetColumnValue(Columns.BSoldOut, value); }
		}
		  
		[XmlAttribute("IMaxQtyPerOrder")]
		[Bindable(true)]
		public int IMaxQtyPerOrder 
		{
			get { return GetColumnValue<int>(Columns.IMaxQtyPerOrder); }
			set { SetColumnValue(Columns.IMaxQtyPerOrder, value); }
		}
		  
		[XmlAttribute("IAllotment")]
		[Bindable(true)]
		public int IAllotment 
		{
			get { return GetColumnValue<int>(Columns.IAllotment); }
			set { SetColumnValue(Columns.IAllotment, value); }
		}
		  
		[XmlAttribute("IDamaged")]
		[Bindable(true)]
		public int IDamaged 
		{
			get { return GetColumnValue<int>(Columns.IDamaged); }
			set { SetColumnValue(Columns.IDamaged, value); }
		}
		  
		[XmlAttribute("IPending")]
		[Bindable(true)]
		public int IPending 
		{
			get { return GetColumnValue<int>(Columns.IPending); }
			set { SetColumnValue(Columns.IPending, value); }
		}
		  
		[XmlAttribute("ISold")]
		[Bindable(true)]
		public int ISold 
		{
			get { return GetColumnValue<int>(Columns.ISold); }
			set { SetColumnValue(Columns.ISold, value); }
		}
		  
		[XmlAttribute("IAvailable")]
		[Bindable(true)]
		public int? IAvailable 
		{
			get { return GetColumnValue<int?>(Columns.IAvailable); }
			set { SetColumnValue(Columns.IAvailable, value); }
		}
		  
		[XmlAttribute("IRefunded")]
		[Bindable(true)]
		public int IRefunded 
		{
			get { return GetColumnValue<int>(Columns.IRefunded); }
			set { SetColumnValue(Columns.IRefunded, value); }
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
		
		
		#region PrimaryKey Methods		
		
        protected override void SetPrimaryKey(object oValue)
        {
            base.SetPrimaryKey(oValue);
            
            SetPKValues();
        }
        
		
		private Wcss.HeaderImageCollection colHeaderImageRecords;
		public Wcss.HeaderImageCollection HeaderImageRecords()
		{
			if(colHeaderImageRecords == null)
			{
				colHeaderImageRecords = new Wcss.HeaderImageCollection().Where(HeaderImage.Columns.TMerchId, Id).Load();
				colHeaderImageRecords.ListChanged += new ListChangedEventHandler(colHeaderImageRecords_ListChanged);
			}
			return colHeaderImageRecords;
		}
				
		void colHeaderImageRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colHeaderImageRecords[e.NewIndex].TMerchId = Id;
				colHeaderImageRecords.ListChanged += new ListChangedEventHandler(colHeaderImageRecords_ListChanged);
            }
		}
		private Wcss.HistoryInventoryCollection colHistoryInventoryRecords;
		public Wcss.HistoryInventoryCollection HistoryInventoryRecords()
		{
			if(colHistoryInventoryRecords == null)
			{
				colHistoryInventoryRecords = new Wcss.HistoryInventoryCollection().Where(HistoryInventory.Columns.TMerchId, Id).Load();
				colHistoryInventoryRecords.ListChanged += new ListChangedEventHandler(colHistoryInventoryRecords_ListChanged);
			}
			return colHistoryInventoryRecords;
		}
				
		void colHistoryInventoryRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colHistoryInventoryRecords[e.NewIndex].TMerchId = Id;
				colHistoryInventoryRecords.ListChanged += new ListChangedEventHandler(colHistoryInventoryRecords_ListChanged);
            }
		}
		private Wcss.HistoryPricingCollection colHistoryPricingRecords;
		public Wcss.HistoryPricingCollection HistoryPricingRecords()
		{
			if(colHistoryPricingRecords == null)
			{
				colHistoryPricingRecords = new Wcss.HistoryPricingCollection().Where(HistoryPricing.Columns.TMerchId, Id).Load();
				colHistoryPricingRecords.ListChanged += new ListChangedEventHandler(colHistoryPricingRecords_ListChanged);
			}
			return colHistoryPricingRecords;
		}
				
		void colHistoryPricingRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colHistoryPricingRecords[e.NewIndex].TMerchId = Id;
				colHistoryPricingRecords.ListChanged += new ListChangedEventHandler(colHistoryPricingRecords_ListChanged);
            }
		}
		private Wcss.InvoiceItemCollection colInvoiceItemRecords;
		public Wcss.InvoiceItemCollection InvoiceItemRecords()
		{
			if(colInvoiceItemRecords == null)
			{
				colInvoiceItemRecords = new Wcss.InvoiceItemCollection().Where(InvoiceItem.Columns.TMerchId, Id).Load();
				colInvoiceItemRecords.ListChanged += new ListChangedEventHandler(colInvoiceItemRecords_ListChanged);
			}
			return colInvoiceItemRecords;
		}
				
		void colInvoiceItemRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colInvoiceItemRecords[e.NewIndex].TMerchId = Id;
				colInvoiceItemRecords.ListChanged += new ListChangedEventHandler(colInvoiceItemRecords_ListChanged);
            }
		}
		private Wcss.ItemImageCollection colItemImageRecords;
		public Wcss.ItemImageCollection ItemImageRecords()
		{
			if(colItemImageRecords == null)
			{
				colItemImageRecords = new Wcss.ItemImageCollection().Where(ItemImage.Columns.TMerchId, Id).Load();
				colItemImageRecords.ListChanged += new ListChangedEventHandler(colItemImageRecords_ListChanged);
			}
			return colItemImageRecords;
		}
				
		void colItemImageRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colItemImageRecords[e.NewIndex].TMerchId = Id;
				colItemImageRecords.ListChanged += new ListChangedEventHandler(colItemImageRecords_ListChanged);
            }
		}
		private Wcss.MerchBundleCollection colMerchBundleRecords;
		public Wcss.MerchBundleCollection MerchBundleRecords()
		{
			if(colMerchBundleRecords == null)
			{
				colMerchBundleRecords = new Wcss.MerchBundleCollection().Where(MerchBundle.Columns.TMerchId, Id).Load();
				colMerchBundleRecords.ListChanged += new ListChangedEventHandler(colMerchBundleRecords_ListChanged);
			}
			return colMerchBundleRecords;
		}
				
		void colMerchBundleRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colMerchBundleRecords[e.NewIndex].TMerchId = Id;
				colMerchBundleRecords.ListChanged += new ListChangedEventHandler(colMerchBundleRecords_ListChanged);
            }
		}
		private Wcss.MerchBundleItemCollection colMerchBundleItemRecords;
		public Wcss.MerchBundleItemCollection MerchBundleItemRecords()
		{
			if(colMerchBundleItemRecords == null)
			{
				colMerchBundleItemRecords = new Wcss.MerchBundleItemCollection().Where(MerchBundleItem.Columns.TMerchId, Id).Load();
				colMerchBundleItemRecords.ListChanged += new ListChangedEventHandler(colMerchBundleItemRecords_ListChanged);
			}
			return colMerchBundleItemRecords;
		}
				
		void colMerchBundleItemRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colMerchBundleItemRecords[e.NewIndex].TMerchId = Id;
				colMerchBundleItemRecords.ListChanged += new ListChangedEventHandler(colMerchBundleItemRecords_ListChanged);
            }
		}
		private Wcss.MerchDownloadCollection colMerchDownloadRecords;
		public Wcss.MerchDownloadCollection MerchDownloadRecords()
		{
			if(colMerchDownloadRecords == null)
			{
				colMerchDownloadRecords = new Wcss.MerchDownloadCollection().Where(MerchDownload.Columns.TMerchId, Id).Load();
				colMerchDownloadRecords.ListChanged += new ListChangedEventHandler(colMerchDownloadRecords_ListChanged);
			}
			return colMerchDownloadRecords;
		}
				
		void colMerchDownloadRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colMerchDownloadRecords[e.NewIndex].TMerchId = Id;
				colMerchDownloadRecords.ListChanged += new ListChangedEventHandler(colMerchDownloadRecords_ListChanged);
            }
		}
		private Wcss.MerchJoinCatCollection colMerchJoinCatRecords;
		public Wcss.MerchJoinCatCollection MerchJoinCatRecords()
		{
			if(colMerchJoinCatRecords == null)
			{
				colMerchJoinCatRecords = new Wcss.MerchJoinCatCollection().Where(MerchJoinCat.Columns.TMerchId, Id).Load();
				colMerchJoinCatRecords.ListChanged += new ListChangedEventHandler(colMerchJoinCatRecords_ListChanged);
			}
			return colMerchJoinCatRecords;
		}
				
		void colMerchJoinCatRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colMerchJoinCatRecords[e.NewIndex].TMerchId = Id;
				colMerchJoinCatRecords.ListChanged += new ListChangedEventHandler(colMerchJoinCatRecords_ListChanged);
            }
		}
		private Wcss.MerchCollection colChildMerchRecords;
		public Wcss.MerchCollection ChildMerchRecords()
		{
			if(colChildMerchRecords == null)
			{
				colChildMerchRecords = new Wcss.MerchCollection().Where(Merch.Columns.TParentListing, Id).Load();
				colChildMerchRecords.ListChanged += new ListChangedEventHandler(colChildMerchRecords_ListChanged);
			}
			return colChildMerchRecords;
		}
				
		void colChildMerchRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colChildMerchRecords[e.NewIndex].TParentListing = Id;
				colChildMerchRecords.ListChanged += new ListChangedEventHandler(colChildMerchRecords_ListChanged);
            }
		}
		private Wcss.PostPurchaseTextCollection colPostPurchaseTextRecords;
		public Wcss.PostPurchaseTextCollection PostPurchaseTextRecords()
		{
			if(colPostPurchaseTextRecords == null)
			{
				colPostPurchaseTextRecords = new Wcss.PostPurchaseTextCollection().Where(PostPurchaseText.Columns.TMerchId, Id).Load();
				colPostPurchaseTextRecords.ListChanged += new ListChangedEventHandler(colPostPurchaseTextRecords_ListChanged);
			}
			return colPostPurchaseTextRecords;
		}
				
		void colPostPurchaseTextRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colPostPurchaseTextRecords[e.NewIndex].TMerchId = Id;
				colPostPurchaseTextRecords.ListChanged += new ListChangedEventHandler(colPostPurchaseTextRecords_ListChanged);
            }
		}
		private Wcss.RequiredMerchCollection colRequiredMerchRecords;
		public Wcss.RequiredMerchCollection RequiredMerchRecords()
		{
			if(colRequiredMerchRecords == null)
			{
				colRequiredMerchRecords = new Wcss.RequiredMerchCollection().Where(RequiredMerch.Columns.TMerchId, Id).Load();
				colRequiredMerchRecords.ListChanged += new ListChangedEventHandler(colRequiredMerchRecords_ListChanged);
			}
			return colRequiredMerchRecords;
		}
				
		void colRequiredMerchRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colRequiredMerchRecords[e.NewIndex].TMerchId = Id;
				colRequiredMerchRecords.ListChanged += new ListChangedEventHandler(colRequiredMerchRecords_ListChanged);
            }
		}
		private Wcss.SalePromotionAwardCollection colSalePromotionAwardRecords;
		public Wcss.SalePromotionAwardCollection SalePromotionAwardRecords()
		{
			if(colSalePromotionAwardRecords == null)
			{
				colSalePromotionAwardRecords = new Wcss.SalePromotionAwardCollection().Where(SalePromotionAward.Columns.TParentMerchId, Id).Load();
				colSalePromotionAwardRecords.ListChanged += new ListChangedEventHandler(colSalePromotionAwardRecords_ListChanged);
			}
			return colSalePromotionAwardRecords;
		}
				
		void colSalePromotionAwardRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colSalePromotionAwardRecords[e.NewIndex].TParentMerchId = Id;
				colSalePromotionAwardRecords.ListChanged += new ListChangedEventHandler(colSalePromotionAwardRecords_ListChanged);
            }
		}
		#endregion
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a AspnetApplication ActiveRecord object related to this Merch
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
		/// Returns a Merch ActiveRecord object related to this Merch
		/// 
		/// </summary>
		private Wcss.Merch ParentMerch
		{
			get { return Wcss.Merch.FetchByID(this.TParentListing); }
			set { SetColumnValue("tParentListing", value.Id); }
		}
        //set up an alternate mechanism to avoid a database call
		private Wcss.Merch _parentmerchrecord = null;
		
		public Wcss.Merch ParentMerchRecord
		{
		    get
            {
                if (_parentmerchrecord == null)
                {
                    _parentmerchrecord = new Wcss.Merch();
                    _parentmerchrecord.CopyFrom(this.ParentMerch);
                }
                return _parentmerchrecord;
            }
            set
            {
                if(value != null && _parentmerchrecord == null)
			        _parentmerchrecord = new Wcss.Merch();
                
                SetColumnValue("tParentListing", value.Id);
                _parentmerchrecord.CopyFrom(value);                
            }
		}
		
		
		#endregion
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(string varName,string varStyle,string varColor,string varSize,int? varTParentListing,bool varBActive,bool varBInternalOnly,bool varBTaxable,bool varBFeaturedItem,string varShortText,string varVcDisplayTemplate,string varDescription,bool varBUnlockActive,string varUnlockCode,DateTime? varDtUnlockDate,DateTime? varDtUnlockEndDate,DateTime? varDtStartDate,DateTime? varDtEndDate,decimal? varMPrice,bool? varBUseSalePrice,decimal? varMSalePrice,string varVcDeliveryType,bool? varBLowRateQualified,decimal? varMWeight,decimal? varMFlatShip,string varVcFlatMethod,DateTime? varDtBackorder,bool? varBShipSeparate,bool? varBSoldOut,int varIMaxQtyPerOrder,int varIAllotment,int varIDamaged,int varIPending,int varISold,int? varIAvailable,int varIRefunded,DateTime varDtStamp,Guid varApplicationId)
		{
			Merch item = new Merch();
			
			item.Name = varName;
			
			item.Style = varStyle;
			
			item.Color = varColor;
			
			item.Size = varSize;
			
			item.TParentListing = varTParentListing;
			
			item.BActive = varBActive;
			
			item.BInternalOnly = varBInternalOnly;
			
			item.BTaxable = varBTaxable;
			
			item.BFeaturedItem = varBFeaturedItem;
			
			item.ShortText = varShortText;
			
			item.VcDisplayTemplate = varVcDisplayTemplate;
			
			item.Description = varDescription;
			
			item.BUnlockActive = varBUnlockActive;
			
			item.UnlockCode = varUnlockCode;
			
			item.DtUnlockDate = varDtUnlockDate;
			
			item.DtUnlockEndDate = varDtUnlockEndDate;
			
			item.DtStartDate = varDtStartDate;
			
			item.DtEndDate = varDtEndDate;
			
			item.MPrice = varMPrice;
			
			item.BUseSalePrice = varBUseSalePrice;
			
			item.MSalePrice = varMSalePrice;
			
			item.VcDeliveryType = varVcDeliveryType;
			
			item.BLowRateQualified = varBLowRateQualified;
			
			item.MWeight = varMWeight;
			
			item.MFlatShip = varMFlatShip;
			
			item.VcFlatMethod = varVcFlatMethod;
			
			item.DtBackorder = varDtBackorder;
			
			item.BShipSeparate = varBShipSeparate;
			
			item.BSoldOut = varBSoldOut;
			
			item.IMaxQtyPerOrder = varIMaxQtyPerOrder;
			
			item.IAllotment = varIAllotment;
			
			item.IDamaged = varIDamaged;
			
			item.IPending = varIPending;
			
			item.ISold = varISold;
			
			item.IAvailable = varIAvailable;
			
			item.IRefunded = varIRefunded;
			
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
		public static void Update(int varId,string varName,string varStyle,string varColor,string varSize,int? varTParentListing,bool varBActive,bool varBInternalOnly,bool varBTaxable,bool varBFeaturedItem,string varShortText,string varVcDisplayTemplate,string varDescription,bool varBUnlockActive,string varUnlockCode,DateTime? varDtUnlockDate,DateTime? varDtUnlockEndDate,DateTime? varDtStartDate,DateTime? varDtEndDate,decimal? varMPrice,bool? varBUseSalePrice,decimal? varMSalePrice,string varVcDeliveryType,bool? varBLowRateQualified,decimal? varMWeight,decimal? varMFlatShip,string varVcFlatMethod,DateTime? varDtBackorder,bool? varBShipSeparate,bool? varBSoldOut,int varIMaxQtyPerOrder,int varIAllotment,int varIDamaged,int varIPending,int varISold,int? varIAvailable,int varIRefunded,DateTime varDtStamp,Guid varApplicationId)
		{
			Merch item = new Merch();
			
				item.Id = varId;
			
				item.Name = varName;
			
				item.Style = varStyle;
			
				item.Color = varColor;
			
				item.Size = varSize;
			
				item.TParentListing = varTParentListing;
			
				item.BActive = varBActive;
			
				item.BInternalOnly = varBInternalOnly;
			
				item.BTaxable = varBTaxable;
			
				item.BFeaturedItem = varBFeaturedItem;
			
				item.ShortText = varShortText;
			
				item.VcDisplayTemplate = varVcDisplayTemplate;
			
				item.Description = varDescription;
			
				item.BUnlockActive = varBUnlockActive;
			
				item.UnlockCode = varUnlockCode;
			
				item.DtUnlockDate = varDtUnlockDate;
			
				item.DtUnlockEndDate = varDtUnlockEndDate;
			
				item.DtStartDate = varDtStartDate;
			
				item.DtEndDate = varDtEndDate;
			
				item.MPrice = varMPrice;
			
				item.BUseSalePrice = varBUseSalePrice;
			
				item.MSalePrice = varMSalePrice;
			
				item.VcDeliveryType = varVcDeliveryType;
			
				item.BLowRateQualified = varBLowRateQualified;
			
				item.MWeight = varMWeight;
			
				item.MFlatShip = varMFlatShip;
			
				item.VcFlatMethod = varVcFlatMethod;
			
				item.DtBackorder = varDtBackorder;
			
				item.BShipSeparate = varBShipSeparate;
			
				item.BSoldOut = varBSoldOut;
			
				item.IMaxQtyPerOrder = varIMaxQtyPerOrder;
			
				item.IAllotment = varIAllotment;
			
				item.IDamaged = varIDamaged;
			
				item.IPending = varIPending;
			
				item.ISold = varISold;
			
				item.IAvailable = varIAvailable;
			
				item.IRefunded = varIRefunded;
			
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
        
        
        
        public static TableSchema.TableColumn NameColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn StyleColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn ColorColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn SizeColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn TParentListingColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn BActiveColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn BInternalOnlyColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn BTaxableColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn BFeaturedItemColumn
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        public static TableSchema.TableColumn ShortTextColumn
        {
            get { return Schema.Columns[10]; }
        }
        
        
        
        public static TableSchema.TableColumn VcDisplayTemplateColumn
        {
            get { return Schema.Columns[11]; }
        }
        
        
        
        public static TableSchema.TableColumn DescriptionColumn
        {
            get { return Schema.Columns[12]; }
        }
        
        
        
        public static TableSchema.TableColumn BUnlockActiveColumn
        {
            get { return Schema.Columns[13]; }
        }
        
        
        
        public static TableSchema.TableColumn UnlockCodeColumn
        {
            get { return Schema.Columns[14]; }
        }
        
        
        
        public static TableSchema.TableColumn DtUnlockDateColumn
        {
            get { return Schema.Columns[15]; }
        }
        
        
        
        public static TableSchema.TableColumn DtUnlockEndDateColumn
        {
            get { return Schema.Columns[16]; }
        }
        
        
        
        public static TableSchema.TableColumn DtStartDateColumn
        {
            get { return Schema.Columns[17]; }
        }
        
        
        
        public static TableSchema.TableColumn DtEndDateColumn
        {
            get { return Schema.Columns[18]; }
        }
        
        
        
        public static TableSchema.TableColumn MPriceColumn
        {
            get { return Schema.Columns[19]; }
        }
        
        
        
        public static TableSchema.TableColumn BUseSalePriceColumn
        {
            get { return Schema.Columns[20]; }
        }
        
        
        
        public static TableSchema.TableColumn MSalePriceColumn
        {
            get { return Schema.Columns[21]; }
        }
        
        
        
        public static TableSchema.TableColumn VcDeliveryTypeColumn
        {
            get { return Schema.Columns[22]; }
        }
        
        
        
        public static TableSchema.TableColumn BLowRateQualifiedColumn
        {
            get { return Schema.Columns[23]; }
        }
        
        
        
        public static TableSchema.TableColumn MWeightColumn
        {
            get { return Schema.Columns[24]; }
        }
        
        
        
        public static TableSchema.TableColumn MFlatShipColumn
        {
            get { return Schema.Columns[25]; }
        }
        
        
        
        public static TableSchema.TableColumn VcFlatMethodColumn
        {
            get { return Schema.Columns[26]; }
        }
        
        
        
        public static TableSchema.TableColumn DtBackorderColumn
        {
            get { return Schema.Columns[27]; }
        }
        
        
        
        public static TableSchema.TableColumn BShipSeparateColumn
        {
            get { return Schema.Columns[28]; }
        }
        
        
        
        public static TableSchema.TableColumn BSoldOutColumn
        {
            get { return Schema.Columns[29]; }
        }
        
        
        
        public static TableSchema.TableColumn IMaxQtyPerOrderColumn
        {
            get { return Schema.Columns[30]; }
        }
        
        
        
        public static TableSchema.TableColumn IAllotmentColumn
        {
            get { return Schema.Columns[31]; }
        }
        
        
        
        public static TableSchema.TableColumn IDamagedColumn
        {
            get { return Schema.Columns[32]; }
        }
        
        
        
        public static TableSchema.TableColumn IPendingColumn
        {
            get { return Schema.Columns[33]; }
        }
        
        
        
        public static TableSchema.TableColumn ISoldColumn
        {
            get { return Schema.Columns[34]; }
        }
        
        
        
        public static TableSchema.TableColumn IAvailableColumn
        {
            get { return Schema.Columns[35]; }
        }
        
        
        
        public static TableSchema.TableColumn IRefundedColumn
        {
            get { return Schema.Columns[36]; }
        }
        
        
        
        public static TableSchema.TableColumn DtStampColumn
        {
            get { return Schema.Columns[37]; }
        }
        
        
        
        public static TableSchema.TableColumn ApplicationIdColumn
        {
            get { return Schema.Columns[38]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Id = @"Id";
			 public static string Name = @"Name";
			 public static string Style = @"Style";
			 public static string Color = @"Color";
			 public static string Size = @"Size";
			 public static string TParentListing = @"tParentListing";
			 public static string BActive = @"bActive";
			 public static string BInternalOnly = @"bInternalOnly";
			 public static string BTaxable = @"bTaxable";
			 public static string BFeaturedItem = @"bFeaturedItem";
			 public static string ShortText = @"ShortText";
			 public static string VcDisplayTemplate = @"vcDisplayTemplate";
			 public static string Description = @"Description";
			 public static string BUnlockActive = @"bUnlockActive";
			 public static string UnlockCode = @"UnlockCode";
			 public static string DtUnlockDate = @"dtUnlockDate";
			 public static string DtUnlockEndDate = @"dtUnlockEndDate";
			 public static string DtStartDate = @"dtStartDate";
			 public static string DtEndDate = @"dtEndDate";
			 public static string MPrice = @"mPrice";
			 public static string BUseSalePrice = @"bUseSalePrice";
			 public static string MSalePrice = @"mSalePrice";
			 public static string VcDeliveryType = @"vcDeliveryType";
			 public static string BLowRateQualified = @"bLowRateQualified";
			 public static string MWeight = @"mWeight";
			 public static string MFlatShip = @"mFlatShip";
			 public static string VcFlatMethod = @"vcFlatMethod";
			 public static string DtBackorder = @"dtBackorder";
			 public static string BShipSeparate = @"bShipSeparate";
			 public static string BSoldOut = @"bSoldOut";
			 public static string IMaxQtyPerOrder = @"iMaxQtyPerOrder";
			 public static string IAllotment = @"iAllotment";
			 public static string IDamaged = @"iDamaged";
			 public static string IPending = @"iPending";
			 public static string ISold = @"iSold";
			 public static string IAvailable = @"iAvailable";
			 public static string IRefunded = @"iRefunded";
			 public static string DtStamp = @"dtStamp";
			 public static string ApplicationId = @"ApplicationId";
						
		}
		#endregion
		
		#region Update PK Collections
		
        public void SetPKValues()
        {
                if (colHeaderImageRecords != null)
                {
                    foreach (Wcss.HeaderImage item in colHeaderImageRecords)
                    {
                        if (item.TMerchId != Id)
                        {
                            item.TMerchId = Id;
                        }
                    }
               }
		
                if (colHistoryInventoryRecords != null)
                {
                    foreach (Wcss.HistoryInventory item in colHistoryInventoryRecords)
                    {
                        if (item.TMerchId != Id)
                        {
                            item.TMerchId = Id;
                        }
                    }
               }
		
                if (colHistoryPricingRecords != null)
                {
                    foreach (Wcss.HistoryPricing item in colHistoryPricingRecords)
                    {
                        if (item.TMerchId != Id)
                        {
                            item.TMerchId = Id;
                        }
                    }
               }
		
                if (colInvoiceItemRecords != null)
                {
                    foreach (Wcss.InvoiceItem item in colInvoiceItemRecords)
                    {
                        if (item.TMerchId != Id)
                        {
                            item.TMerchId = Id;
                        }
                    }
               }
		
                if (colItemImageRecords != null)
                {
                    foreach (Wcss.ItemImage item in colItemImageRecords)
                    {
                        if (item.TMerchId != Id)
                        {
                            item.TMerchId = Id;
                        }
                    }
               }
		
                if (colMerchBundleRecords != null)
                {
                    foreach (Wcss.MerchBundle item in colMerchBundleRecords)
                    {
                        if (item.TMerchId != Id)
                        {
                            item.TMerchId = Id;
                        }
                    }
               }
		
                if (colMerchBundleItemRecords != null)
                {
                    foreach (Wcss.MerchBundleItem item in colMerchBundleItemRecords)
                    {
                        if (item.TMerchId != Id)
                        {
                            item.TMerchId = Id;
                        }
                    }
               }
		
                if (colMerchDownloadRecords != null)
                {
                    foreach (Wcss.MerchDownload item in colMerchDownloadRecords)
                    {
                        if (item.TMerchId != Id)
                        {
                            item.TMerchId = Id;
                        }
                    }
               }
		
                if (colMerchJoinCatRecords != null)
                {
                    foreach (Wcss.MerchJoinCat item in colMerchJoinCatRecords)
                    {
                        if (item.TMerchId != Id)
                        {
                            item.TMerchId = Id;
                        }
                    }
               }
		
                if (colChildMerchRecords != null)
                {
                    foreach (Wcss.Merch item in colChildMerchRecords)
                    {
                        if (item.TParentListing != Id)
                        {
                            item.TParentListing = Id;
                        }
                    }
               }
		
                if (colPostPurchaseTextRecords != null)
                {
                    foreach (Wcss.PostPurchaseText item in colPostPurchaseTextRecords)
                    {
                        if (item.TMerchId != Id)
                        {
                            item.TMerchId = Id;
                        }
                    }
               }
		
                if (colRequiredMerchRecords != null)
                {
                    foreach (Wcss.RequiredMerch item in colRequiredMerchRecords)
                    {
                        if (item.TMerchId != Id)
                        {
                            item.TMerchId = Id;
                        }
                    }
               }
		
                if (colSalePromotionAwardRecords != null)
                {
                    foreach (Wcss.SalePromotionAward item in colSalePromotionAwardRecords)
                    {
                        if (item.TParentMerchId != Id)
                        {
                            item.TParentMerchId = Id;
                        }
                    }
               }
		}
        #endregion
    
        #region Deep Save
		
        public void DeepSave()
        {
            Save();
            
                if (colHeaderImageRecords != null)
                {
                    colHeaderImageRecords.SaveAll();
               }
		
                if (colHistoryInventoryRecords != null)
                {
                    colHistoryInventoryRecords.SaveAll();
               }
		
                if (colHistoryPricingRecords != null)
                {
                    colHistoryPricingRecords.SaveAll();
               }
		
                if (colInvoiceItemRecords != null)
                {
                    colInvoiceItemRecords.SaveAll();
               }
		
                if (colItemImageRecords != null)
                {
                    colItemImageRecords.SaveAll();
               }
		
                if (colMerchBundleRecords != null)
                {
                    colMerchBundleRecords.SaveAll();
               }
		
                if (colMerchBundleItemRecords != null)
                {
                    colMerchBundleItemRecords.SaveAll();
               }
		
                if (colMerchDownloadRecords != null)
                {
                    colMerchDownloadRecords.SaveAll();
               }
		
                if (colMerchJoinCatRecords != null)
                {
                    colMerchJoinCatRecords.SaveAll();
               }
		
                if (colChildMerchRecords != null)
                {
                    colChildMerchRecords.SaveAll();
               }
		
                if (colPostPurchaseTextRecords != null)
                {
                    colPostPurchaseTextRecords.SaveAll();
               }
		
                if (colRequiredMerchRecords != null)
                {
                    colRequiredMerchRecords.SaveAll();
               }
		
                if (colSalePromotionAwardRecords != null)
                {
                    colSalePromotionAwardRecords.SaveAll();
               }
		}
        #endregion
	}
}
