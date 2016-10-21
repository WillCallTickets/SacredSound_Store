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
	/// Strongly-typed collection for the ShowTicket class.
	/// </summary>
    [Serializable]
	public partial class ShowTicketCollection : ActiveList<ShowTicket, ShowTicketCollection>
	{	   
		public ShowTicketCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>ShowTicketCollection</returns>
		public ShowTicketCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                ShowTicket o = this[i];
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
	/// This is an ActiveRecord class which wraps the ShowTicket table.
	/// </summary>
	[Serializable]
	public partial class ShowTicket : ActiveRecord<ShowTicket>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public ShowTicket()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public ShowTicket(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public ShowTicket(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public ShowTicket(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("ShowTicket", TableType.Table, DataService.GetInstance("WillCall"));
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
				
				TableSchema.TableColumn colvarTVendorId = new TableSchema.TableColumn(schema);
				colvarTVendorId.ColumnName = "TVendorId";
				colvarTVendorId.DataType = DbType.Int32;
				colvarTVendorId.MaxLength = 0;
				colvarTVendorId.AutoIncrement = false;
				colvarTVendorId.IsNullable = false;
				colvarTVendorId.IsPrimaryKey = false;
				colvarTVendorId.IsForeignKey = false;
				colvarTVendorId.IsReadOnly = false;
				colvarTVendorId.DefaultSetting = @"";
				colvarTVendorId.ForeignKeyTableName = "";
				schema.Columns.Add(colvarTVendorId);
				
				TableSchema.TableColumn colvarDtDateOfShow = new TableSchema.TableColumn(schema);
				colvarDtDateOfShow.ColumnName = "dtDateOfShow";
				colvarDtDateOfShow.DataType = DbType.DateTime;
				colvarDtDateOfShow.MaxLength = 0;
				colvarDtDateOfShow.AutoIncrement = false;
				colvarDtDateOfShow.IsNullable = false;
				colvarDtDateOfShow.IsPrimaryKey = false;
				colvarDtDateOfShow.IsForeignKey = false;
				colvarDtDateOfShow.IsReadOnly = false;
				colvarDtDateOfShow.DefaultSetting = @"";
				colvarDtDateOfShow.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDtDateOfShow);
				
				TableSchema.TableColumn colvarCriteriaText = new TableSchema.TableColumn(schema);
				colvarCriteriaText.ColumnName = "CriteriaText";
				colvarCriteriaText.DataType = DbType.AnsiString;
				colvarCriteriaText.MaxLength = 300;
				colvarCriteriaText.AutoIncrement = false;
				colvarCriteriaText.IsNullable = true;
				colvarCriteriaText.IsPrimaryKey = false;
				colvarCriteriaText.IsForeignKey = false;
				colvarCriteriaText.IsReadOnly = false;
				colvarCriteriaText.DefaultSetting = @"";
				colvarCriteriaText.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCriteriaText);
				
				TableSchema.TableColumn colvarSalesDescription = new TableSchema.TableColumn(schema);
				colvarSalesDescription.ColumnName = "SalesDescription";
				colvarSalesDescription.DataType = DbType.AnsiString;
				colvarSalesDescription.MaxLength = 300;
				colvarSalesDescription.AutoIncrement = false;
				colvarSalesDescription.IsNullable = true;
				colvarSalesDescription.IsPrimaryKey = false;
				colvarSalesDescription.IsForeignKey = false;
				colvarSalesDescription.IsReadOnly = false;
				colvarSalesDescription.DefaultSetting = @"";
				colvarSalesDescription.ForeignKeyTableName = "";
				schema.Columns.Add(colvarSalesDescription);
				
				TableSchema.TableColumn colvarTShowDateId = new TableSchema.TableColumn(schema);
				colvarTShowDateId.ColumnName = "TShowDateId";
				colvarTShowDateId.DataType = DbType.Int32;
				colvarTShowDateId.MaxLength = 0;
				colvarTShowDateId.AutoIncrement = false;
				colvarTShowDateId.IsNullable = false;
				colvarTShowDateId.IsPrimaryKey = false;
				colvarTShowDateId.IsForeignKey = true;
				colvarTShowDateId.IsReadOnly = false;
				colvarTShowDateId.DefaultSetting = @"";
				
					colvarTShowDateId.ForeignKeyTableName = "ShowDate";
				schema.Columns.Add(colvarTShowDateId);
				
				TableSchema.TableColumn colvarTShowId = new TableSchema.TableColumn(schema);
				colvarTShowId.ColumnName = "TShowId";
				colvarTShowId.DataType = DbType.Int32;
				colvarTShowId.MaxLength = 0;
				colvarTShowId.AutoIncrement = false;
				colvarTShowId.IsNullable = false;
				colvarTShowId.IsPrimaryKey = false;
				colvarTShowId.IsForeignKey = false;
				colvarTShowId.IsReadOnly = false;
				colvarTShowId.DefaultSetting = @"";
				colvarTShowId.ForeignKeyTableName = "";
				schema.Columns.Add(colvarTShowId);
				
				TableSchema.TableColumn colvarTAgeId = new TableSchema.TableColumn(schema);
				colvarTAgeId.ColumnName = "TAgeId";
				colvarTAgeId.DataType = DbType.Int32;
				colvarTAgeId.MaxLength = 0;
				colvarTAgeId.AutoIncrement = false;
				colvarTAgeId.IsNullable = false;
				colvarTAgeId.IsPrimaryKey = false;
				colvarTAgeId.IsForeignKey = false;
				colvarTAgeId.IsReadOnly = false;
				colvarTAgeId.DefaultSetting = @"";
				colvarTAgeId.ForeignKeyTableName = "";
				schema.Columns.Add(colvarTAgeId);
				
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
				
				TableSchema.TableColumn colvarBSoldOut = new TableSchema.TableColumn(schema);
				colvarBSoldOut.ColumnName = "bSoldOut";
				colvarBSoldOut.DataType = DbType.Boolean;
				colvarBSoldOut.MaxLength = 0;
				colvarBSoldOut.AutoIncrement = false;
				colvarBSoldOut.IsNullable = false;
				colvarBSoldOut.IsPrimaryKey = false;
				colvarBSoldOut.IsForeignKey = false;
				colvarBSoldOut.IsReadOnly = false;
				
						colvarBSoldOut.DefaultSetting = @"((0))";
				colvarBSoldOut.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBSoldOut);
				
				TableSchema.TableColumn colvarStatus = new TableSchema.TableColumn(schema);
				colvarStatus.ColumnName = "Status";
				colvarStatus.DataType = DbType.AnsiString;
				colvarStatus.MaxLength = 500;
				colvarStatus.AutoIncrement = false;
				colvarStatus.IsNullable = true;
				colvarStatus.IsPrimaryKey = false;
				colvarStatus.IsForeignKey = false;
				colvarStatus.IsReadOnly = false;
				colvarStatus.DefaultSetting = @"";
				colvarStatus.ForeignKeyTableName = "";
				schema.Columns.Add(colvarStatus);
				
				TableSchema.TableColumn colvarBDosTicket = new TableSchema.TableColumn(schema);
				colvarBDosTicket.ColumnName = "bDosTicket";
				colvarBDosTicket.DataType = DbType.Boolean;
				colvarBDosTicket.MaxLength = 0;
				colvarBDosTicket.AutoIncrement = false;
				colvarBDosTicket.IsNullable = false;
				colvarBDosTicket.IsPrimaryKey = false;
				colvarBDosTicket.IsForeignKey = false;
				colvarBDosTicket.IsReadOnly = false;
				
						colvarBDosTicket.DefaultSetting = @"((0))";
				colvarBDosTicket.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBDosTicket);
				
				TableSchema.TableColumn colvarIDisplayOrder = new TableSchema.TableColumn(schema);
				colvarIDisplayOrder.ColumnName = "iDisplayOrder";
				colvarIDisplayOrder.DataType = DbType.Int32;
				colvarIDisplayOrder.MaxLength = 0;
				colvarIDisplayOrder.AutoIncrement = false;
				colvarIDisplayOrder.IsNullable = false;
				colvarIDisplayOrder.IsPrimaryKey = false;
				colvarIDisplayOrder.IsForeignKey = false;
				colvarIDisplayOrder.IsReadOnly = false;
				colvarIDisplayOrder.DefaultSetting = @"";
				colvarIDisplayOrder.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIDisplayOrder);
				
				TableSchema.TableColumn colvarPriceText = new TableSchema.TableColumn(schema);
				colvarPriceText.ColumnName = "PriceText";
				colvarPriceText.DataType = DbType.AnsiString;
				colvarPriceText.MaxLength = 300;
				colvarPriceText.AutoIncrement = false;
				colvarPriceText.IsNullable = true;
				colvarPriceText.IsPrimaryKey = false;
				colvarPriceText.IsForeignKey = false;
				colvarPriceText.IsReadOnly = false;
				colvarPriceText.DefaultSetting = @"";
				colvarPriceText.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPriceText);
				
				TableSchema.TableColumn colvarMPrice = new TableSchema.TableColumn(schema);
				colvarMPrice.ColumnName = "mPrice";
				colvarMPrice.DataType = DbType.Currency;
				colvarMPrice.MaxLength = 0;
				colvarMPrice.AutoIncrement = false;
				colvarMPrice.IsNullable = true;
				colvarMPrice.IsPrimaryKey = false;
				colvarMPrice.IsForeignKey = false;
				colvarMPrice.IsReadOnly = false;
				
						colvarMPrice.DefaultSetting = @"((0))";
				colvarMPrice.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMPrice);
				
				TableSchema.TableColumn colvarDosText = new TableSchema.TableColumn(schema);
				colvarDosText.ColumnName = "DosText";
				colvarDosText.DataType = DbType.AnsiString;
				colvarDosText.MaxLength = 300;
				colvarDosText.AutoIncrement = false;
				colvarDosText.IsNullable = true;
				colvarDosText.IsPrimaryKey = false;
				colvarDosText.IsForeignKey = false;
				colvarDosText.IsReadOnly = false;
				colvarDosText.DefaultSetting = @"";
				colvarDosText.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDosText);
				
				TableSchema.TableColumn colvarMDosPrice = new TableSchema.TableColumn(schema);
				colvarMDosPrice.ColumnName = "mDosPrice";
				colvarMDosPrice.DataType = DbType.Currency;
				colvarMDosPrice.MaxLength = 0;
				colvarMDosPrice.AutoIncrement = false;
				colvarMDosPrice.IsNullable = true;
				colvarMDosPrice.IsPrimaryKey = false;
				colvarMDosPrice.IsForeignKey = false;
				colvarMDosPrice.IsReadOnly = false;
				colvarMDosPrice.DefaultSetting = @"";
				colvarMDosPrice.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMDosPrice);
				
				TableSchema.TableColumn colvarMServiceCharge = new TableSchema.TableColumn(schema);
				colvarMServiceCharge.ColumnName = "mServiceCharge";
				colvarMServiceCharge.DataType = DbType.Currency;
				colvarMServiceCharge.MaxLength = 0;
				colvarMServiceCharge.AutoIncrement = false;
				colvarMServiceCharge.IsNullable = true;
				colvarMServiceCharge.IsPrimaryKey = false;
				colvarMServiceCharge.IsForeignKey = false;
				colvarMServiceCharge.IsReadOnly = false;
				colvarMServiceCharge.DefaultSetting = @"";
				colvarMServiceCharge.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMServiceCharge);
				
				TableSchema.TableColumn colvarBAllowShipping = new TableSchema.TableColumn(schema);
				colvarBAllowShipping.ColumnName = "bAllowShipping";
				colvarBAllowShipping.DataType = DbType.Boolean;
				colvarBAllowShipping.MaxLength = 0;
				colvarBAllowShipping.AutoIncrement = false;
				colvarBAllowShipping.IsNullable = false;
				colvarBAllowShipping.IsPrimaryKey = false;
				colvarBAllowShipping.IsForeignKey = false;
				colvarBAllowShipping.IsReadOnly = false;
				
						colvarBAllowShipping.DefaultSetting = @"((1))";
				colvarBAllowShipping.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBAllowShipping);
				
				TableSchema.TableColumn colvarBAllowWillCall = new TableSchema.TableColumn(schema);
				colvarBAllowWillCall.ColumnName = "bAllowWillCall";
				colvarBAllowWillCall.DataType = DbType.Boolean;
				colvarBAllowWillCall.MaxLength = 0;
				colvarBAllowWillCall.AutoIncrement = false;
				colvarBAllowWillCall.IsNullable = false;
				colvarBAllowWillCall.IsPrimaryKey = false;
				colvarBAllowWillCall.IsForeignKey = false;
				colvarBAllowWillCall.IsReadOnly = false;
				
						colvarBAllowWillCall.DefaultSetting = @"((1))";
				colvarBAllowWillCall.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBAllowWillCall);
				
				TableSchema.TableColumn colvarBHideShipMethod = new TableSchema.TableColumn(schema);
				colvarBHideShipMethod.ColumnName = "bHideShipMethod";
				colvarBHideShipMethod.DataType = DbType.Boolean;
				colvarBHideShipMethod.MaxLength = 0;
				colvarBHideShipMethod.AutoIncrement = false;
				colvarBHideShipMethod.IsNullable = false;
				colvarBHideShipMethod.IsPrimaryKey = false;
				colvarBHideShipMethod.IsForeignKey = false;
				colvarBHideShipMethod.IsReadOnly = false;
				
						colvarBHideShipMethod.DefaultSetting = @"((0))";
				colvarBHideShipMethod.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBHideShipMethod);
				
				TableSchema.TableColumn colvarDtShipCutoff = new TableSchema.TableColumn(schema);
				colvarDtShipCutoff.ColumnName = "dtShipCutoff";
				colvarDtShipCutoff.DataType = DbType.DateTime;
				colvarDtShipCutoff.MaxLength = 0;
				colvarDtShipCutoff.AutoIncrement = false;
				colvarDtShipCutoff.IsNullable = false;
				colvarDtShipCutoff.IsPrimaryKey = false;
				colvarDtShipCutoff.IsForeignKey = false;
				colvarDtShipCutoff.IsReadOnly = false;
				colvarDtShipCutoff.DefaultSetting = @"";
				colvarDtShipCutoff.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDtShipCutoff);
				
				TableSchema.TableColumn colvarBOverrideSellout = new TableSchema.TableColumn(schema);
				colvarBOverrideSellout.ColumnName = "bOverrideSellout";
				colvarBOverrideSellout.DataType = DbType.Boolean;
				colvarBOverrideSellout.MaxLength = 0;
				colvarBOverrideSellout.AutoIncrement = false;
				colvarBOverrideSellout.IsNullable = false;
				colvarBOverrideSellout.IsPrimaryKey = false;
				colvarBOverrideSellout.IsForeignKey = false;
				colvarBOverrideSellout.IsReadOnly = false;
				
						colvarBOverrideSellout.DefaultSetting = @"((0))";
				colvarBOverrideSellout.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBOverrideSellout);
				
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
				
				TableSchema.TableColumn colvarDtPublicOnsale = new TableSchema.TableColumn(schema);
				colvarDtPublicOnsale.ColumnName = "dtPublicOnsale";
				colvarDtPublicOnsale.DataType = DbType.DateTime;
				colvarDtPublicOnsale.MaxLength = 0;
				colvarDtPublicOnsale.AutoIncrement = false;
				colvarDtPublicOnsale.IsNullable = true;
				colvarDtPublicOnsale.IsPrimaryKey = false;
				colvarDtPublicOnsale.IsForeignKey = false;
				colvarDtPublicOnsale.IsReadOnly = false;
				colvarDtPublicOnsale.DefaultSetting = @"";
				colvarDtPublicOnsale.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDtPublicOnsale);
				
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
				
				TableSchema.TableColumn colvarIMaxQtyPerOrder = new TableSchema.TableColumn(schema);
				colvarIMaxQtyPerOrder.ColumnName = "iMaxQtyPerOrder";
				colvarIMaxQtyPerOrder.DataType = DbType.Int32;
				colvarIMaxQtyPerOrder.MaxLength = 0;
				colvarIMaxQtyPerOrder.AutoIncrement = false;
				colvarIMaxQtyPerOrder.IsNullable = true;
				colvarIMaxQtyPerOrder.IsPrimaryKey = false;
				colvarIMaxQtyPerOrder.IsForeignKey = false;
				colvarIMaxQtyPerOrder.IsReadOnly = false;
				colvarIMaxQtyPerOrder.DefaultSetting = @"";
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
				DataService.Providers["WillCall"].AddSchema("ShowTicket",schema);
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
		  
		[XmlAttribute("TVendorId")]
		[Bindable(true)]
		public int TVendorId 
		{
			get { return GetColumnValue<int>(Columns.TVendorId); }
			set { SetColumnValue(Columns.TVendorId, value); }
		}
		  
		[XmlAttribute("DtDateOfShow")]
		[Bindable(true)]
		public DateTime DtDateOfShow 
		{
			get { return GetColumnValue<DateTime>(Columns.DtDateOfShow); }
			set { SetColumnValue(Columns.DtDateOfShow, value); }
		}
		  
		[XmlAttribute("CriteriaText")]
		[Bindable(true)]
		public string CriteriaText 
		{
			get { return GetColumnValue<string>(Columns.CriteriaText); }
			set { SetColumnValue(Columns.CriteriaText, value); }
		}
		  
		[XmlAttribute("SalesDescription")]
		[Bindable(true)]
		public string SalesDescription 
		{
			get { return GetColumnValue<string>(Columns.SalesDescription); }
			set { SetColumnValue(Columns.SalesDescription, value); }
		}
		  
		[XmlAttribute("TShowDateId")]
		[Bindable(true)]
		public int TShowDateId 
		{
			get { return GetColumnValue<int>(Columns.TShowDateId); }
			set { SetColumnValue(Columns.TShowDateId, value); }
		}
		  
		[XmlAttribute("TShowId")]
		[Bindable(true)]
		public int TShowId 
		{
			get { return GetColumnValue<int>(Columns.TShowId); }
			set { SetColumnValue(Columns.TShowId, value); }
		}
		  
		[XmlAttribute("TAgeId")]
		[Bindable(true)]
		public int TAgeId 
		{
			get { return GetColumnValue<int>(Columns.TAgeId); }
			set { SetColumnValue(Columns.TAgeId, value); }
		}
		  
		[XmlAttribute("BActive")]
		[Bindable(true)]
		public bool BActive 
		{
			get { return GetColumnValue<bool>(Columns.BActive); }
			set { SetColumnValue(Columns.BActive, value); }
		}
		  
		[XmlAttribute("BSoldOut")]
		[Bindable(true)]
		public bool BSoldOut 
		{
			get { return GetColumnValue<bool>(Columns.BSoldOut); }
			set { SetColumnValue(Columns.BSoldOut, value); }
		}
		  
		[XmlAttribute("Status")]
		[Bindable(true)]
		public string Status 
		{
			get { return GetColumnValue<string>(Columns.Status); }
			set { SetColumnValue(Columns.Status, value); }
		}
		  
		[XmlAttribute("BDosTicket")]
		[Bindable(true)]
		public bool BDosTicket 
		{
			get { return GetColumnValue<bool>(Columns.BDosTicket); }
			set { SetColumnValue(Columns.BDosTicket, value); }
		}
		  
		[XmlAttribute("IDisplayOrder")]
		[Bindable(true)]
		public int IDisplayOrder 
		{
			get { return GetColumnValue<int>(Columns.IDisplayOrder); }
			set { SetColumnValue(Columns.IDisplayOrder, value); }
		}
		  
		[XmlAttribute("PriceText")]
		[Bindable(true)]
		public string PriceText 
		{
			get { return GetColumnValue<string>(Columns.PriceText); }
			set { SetColumnValue(Columns.PriceText, value); }
		}
		  
		[XmlAttribute("MPrice")]
		[Bindable(true)]
		public decimal? MPrice 
		{
			get { return GetColumnValue<decimal?>(Columns.MPrice); }
			set { SetColumnValue(Columns.MPrice, value); }
		}
		  
		[XmlAttribute("DosText")]
		[Bindable(true)]
		public string DosText 
		{
			get { return GetColumnValue<string>(Columns.DosText); }
			set { SetColumnValue(Columns.DosText, value); }
		}
		  
		[XmlAttribute("MDosPrice")]
		[Bindable(true)]
		public decimal? MDosPrice 
		{
			get { return GetColumnValue<decimal?>(Columns.MDosPrice); }
			set { SetColumnValue(Columns.MDosPrice, value); }
		}
		  
		[XmlAttribute("MServiceCharge")]
		[Bindable(true)]
		public decimal? MServiceCharge 
		{
			get { return GetColumnValue<decimal?>(Columns.MServiceCharge); }
			set { SetColumnValue(Columns.MServiceCharge, value); }
		}
		  
		[XmlAttribute("BAllowShipping")]
		[Bindable(true)]
		public bool BAllowShipping 
		{
			get { return GetColumnValue<bool>(Columns.BAllowShipping); }
			set { SetColumnValue(Columns.BAllowShipping, value); }
		}
		  
		[XmlAttribute("BAllowWillCall")]
		[Bindable(true)]
		public bool BAllowWillCall 
		{
			get { return GetColumnValue<bool>(Columns.BAllowWillCall); }
			set { SetColumnValue(Columns.BAllowWillCall, value); }
		}
		  
		[XmlAttribute("BHideShipMethod")]
		[Bindable(true)]
		public bool BHideShipMethod 
		{
			get { return GetColumnValue<bool>(Columns.BHideShipMethod); }
			set { SetColumnValue(Columns.BHideShipMethod, value); }
		}
		  
		[XmlAttribute("DtShipCutoff")]
		[Bindable(true)]
		public DateTime DtShipCutoff 
		{
			get { return GetColumnValue<DateTime>(Columns.DtShipCutoff); }
			set { SetColumnValue(Columns.DtShipCutoff, value); }
		}
		  
		[XmlAttribute("BOverrideSellout")]
		[Bindable(true)]
		public bool BOverrideSellout 
		{
			get { return GetColumnValue<bool>(Columns.BOverrideSellout); }
			set { SetColumnValue(Columns.BOverrideSellout, value); }
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
		  
		[XmlAttribute("DtPublicOnsale")]
		[Bindable(true)]
		public DateTime? DtPublicOnsale 
		{
			get { return GetColumnValue<DateTime?>(Columns.DtPublicOnsale); }
			set { SetColumnValue(Columns.DtPublicOnsale, value); }
		}
		  
		[XmlAttribute("DtEndDate")]
		[Bindable(true)]
		public DateTime? DtEndDate 
		{
			get { return GetColumnValue<DateTime?>(Columns.DtEndDate); }
			set { SetColumnValue(Columns.DtEndDate, value); }
		}
		  
		[XmlAttribute("IMaxQtyPerOrder")]
		[Bindable(true)]
		public int? IMaxQtyPerOrder 
		{
			get { return GetColumnValue<int?>(Columns.IMaxQtyPerOrder); }
			set { SetColumnValue(Columns.IMaxQtyPerOrder, value); }
		}
		  
		[XmlAttribute("IAllotment")]
		[Bindable(true)]
		public int IAllotment 
		{
			get { return GetColumnValue<int>(Columns.IAllotment); }
			set { SetColumnValue(Columns.IAllotment, value); }
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
        
		
		private Wcss.HistoryInventoryCollection colHistoryInventoryRecords;
		public Wcss.HistoryInventoryCollection HistoryInventoryRecords()
		{
			if(colHistoryInventoryRecords == null)
			{
				colHistoryInventoryRecords = new Wcss.HistoryInventoryCollection().Where(HistoryInventory.Columns.TShowTicketId, Id).Load();
				colHistoryInventoryRecords.ListChanged += new ListChangedEventHandler(colHistoryInventoryRecords_ListChanged);
			}
			return colHistoryInventoryRecords;
		}
				
		void colHistoryInventoryRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colHistoryInventoryRecords[e.NewIndex].TShowTicketId = Id;
				colHistoryInventoryRecords.ListChanged += new ListChangedEventHandler(colHistoryInventoryRecords_ListChanged);
            }
		}
		private Wcss.HistoryPricingCollection colHistoryPricingRecords;
		public Wcss.HistoryPricingCollection HistoryPricingRecords()
		{
			if(colHistoryPricingRecords == null)
			{
				colHistoryPricingRecords = new Wcss.HistoryPricingCollection().Where(HistoryPricing.Columns.TShowTicketId, Id).Load();
				colHistoryPricingRecords.ListChanged += new ListChangedEventHandler(colHistoryPricingRecords_ListChanged);
			}
			return colHistoryPricingRecords;
		}
				
		void colHistoryPricingRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colHistoryPricingRecords[e.NewIndex].TShowTicketId = Id;
				colHistoryPricingRecords.ListChanged += new ListChangedEventHandler(colHistoryPricingRecords_ListChanged);
            }
		}
		private Wcss.InvoiceItemCollection colInvoiceItemRecords;
		public Wcss.InvoiceItemCollection InvoiceItemRecords()
		{
			if(colInvoiceItemRecords == null)
			{
				colInvoiceItemRecords = new Wcss.InvoiceItemCollection().Where(InvoiceItem.Columns.TShowTicketId, Id).Load();
				colInvoiceItemRecords.ListChanged += new ListChangedEventHandler(colInvoiceItemRecords_ListChanged);
			}
			return colInvoiceItemRecords;
		}
				
		void colInvoiceItemRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colInvoiceItemRecords[e.NewIndex].TShowTicketId = Id;
				colInvoiceItemRecords.ListChanged += new ListChangedEventHandler(colInvoiceItemRecords_ListChanged);
            }
		}
		private Wcss.LotteryCollection colLotteryRecords;
		public Wcss.LotteryCollection LotteryRecords()
		{
			if(colLotteryRecords == null)
			{
				colLotteryRecords = new Wcss.LotteryCollection().Where(Lottery.Columns.TShowTicketId, Id).Load();
				colLotteryRecords.ListChanged += new ListChangedEventHandler(colLotteryRecords_ListChanged);
			}
			return colLotteryRecords;
		}
				
		void colLotteryRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colLotteryRecords[e.NewIndex].TShowTicketId = Id;
				colLotteryRecords.ListChanged += new ListChangedEventHandler(colLotteryRecords_ListChanged);
            }
		}
		private Wcss.MerchBundleCollection colMerchBundleRecords;
		public Wcss.MerchBundleCollection MerchBundleRecords()
		{
			if(colMerchBundleRecords == null)
			{
				colMerchBundleRecords = new Wcss.MerchBundleCollection().Where(MerchBundle.Columns.TShowTicketId, Id).Load();
				colMerchBundleRecords.ListChanged += new ListChangedEventHandler(colMerchBundleRecords_ListChanged);
			}
			return colMerchBundleRecords;
		}
				
		void colMerchBundleRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colMerchBundleRecords[e.NewIndex].TShowTicketId = Id;
				colMerchBundleRecords.ListChanged += new ListChangedEventHandler(colMerchBundleRecords_ListChanged);
            }
		}
		private Wcss.PostPurchaseTextCollection colPostPurchaseTextRecords;
		public Wcss.PostPurchaseTextCollection PostPurchaseTextRecords()
		{
			if(colPostPurchaseTextRecords == null)
			{
				colPostPurchaseTextRecords = new Wcss.PostPurchaseTextCollection().Where(PostPurchaseText.Columns.TShowTicketId, Id).Load();
				colPostPurchaseTextRecords.ListChanged += new ListChangedEventHandler(colPostPurchaseTextRecords_ListChanged);
			}
			return colPostPurchaseTextRecords;
		}
				
		void colPostPurchaseTextRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colPostPurchaseTextRecords[e.NewIndex].TShowTicketId = Id;
				colPostPurchaseTextRecords.ListChanged += new ListChangedEventHandler(colPostPurchaseTextRecords_ListChanged);
            }
		}
		private Wcss.RequiredShowTicketPastPurchaseCollection colRequiredShowTicketPastPurchaseRecords;
		public Wcss.RequiredShowTicketPastPurchaseCollection RequiredShowTicketPastPurchaseRecords()
		{
			if(colRequiredShowTicketPastPurchaseRecords == null)
			{
				colRequiredShowTicketPastPurchaseRecords = new Wcss.RequiredShowTicketPastPurchaseCollection().Where(RequiredShowTicketPastPurchase.Columns.TShowTicketId, Id).Load();
				colRequiredShowTicketPastPurchaseRecords.ListChanged += new ListChangedEventHandler(colRequiredShowTicketPastPurchaseRecords_ListChanged);
			}
			return colRequiredShowTicketPastPurchaseRecords;
		}
				
		void colRequiredShowTicketPastPurchaseRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colRequiredShowTicketPastPurchaseRecords[e.NewIndex].TShowTicketId = Id;
				colRequiredShowTicketPastPurchaseRecords.ListChanged += new ListChangedEventHandler(colRequiredShowTicketPastPurchaseRecords_ListChanged);
            }
		}
		private Wcss.SalePromotionCollection colSalePromotionRecords;
		public Wcss.SalePromotionCollection SalePromotionRecords()
		{
			if(colSalePromotionRecords == null)
			{
				colSalePromotionRecords = new Wcss.SalePromotionCollection().Where(SalePromotion.Columns.TShowTicketId, Id).Load();
				colSalePromotionRecords.ListChanged += new ListChangedEventHandler(colSalePromotionRecords_ListChanged);
			}
			return colSalePromotionRecords;
		}
				
		void colSalePromotionRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colSalePromotionRecords[e.NewIndex].TShowTicketId = Id;
				colSalePromotionRecords.ListChanged += new ListChangedEventHandler(colSalePromotionRecords_ListChanged);
            }
		}
		private Wcss.SalePromotionCollection colSalePromotionRecordsFromShowTicket;
		public Wcss.SalePromotionCollection SalePromotionRecordsFromShowTicket()
		{
			if(colSalePromotionRecordsFromShowTicket == null)
			{
				colSalePromotionRecordsFromShowTicket = new Wcss.SalePromotionCollection().Where(SalePromotion.Columns.TRequiredParentShowTicketId, Id).Load();
				colSalePromotionRecordsFromShowTicket.ListChanged += new ListChangedEventHandler(colSalePromotionRecordsFromShowTicket_ListChanged);
			}
			return colSalePromotionRecordsFromShowTicket;
		}
				
		void colSalePromotionRecordsFromShowTicket_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colSalePromotionRecordsFromShowTicket[e.NewIndex].TRequiredParentShowTicketId = Id;
				colSalePromotionRecordsFromShowTicket.ListChanged += new ListChangedEventHandler(colSalePromotionRecordsFromShowTicket_ListChanged);
            }
		}
		private Wcss.ShowTicketDosTicketCollection colShowTicketDosTicketRecords;
		public Wcss.ShowTicketDosTicketCollection ShowTicketDosTicketRecords()
		{
			if(colShowTicketDosTicketRecords == null)
			{
				colShowTicketDosTicketRecords = new Wcss.ShowTicketDosTicketCollection().Where(ShowTicketDosTicket.Columns.ParentId, Id).Load();
				colShowTicketDosTicketRecords.ListChanged += new ListChangedEventHandler(colShowTicketDosTicketRecords_ListChanged);
			}
			return colShowTicketDosTicketRecords;
		}
				
		void colShowTicketDosTicketRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colShowTicketDosTicketRecords[e.NewIndex].ParentId = Id;
				colShowTicketDosTicketRecords.ListChanged += new ListChangedEventHandler(colShowTicketDosTicketRecords_ListChanged);
            }
		}
		private Wcss.ShowTicketDosTicketCollection colShowTicketDosTicketRecordsFromShowTicket;
		public Wcss.ShowTicketDosTicketCollection ShowTicketDosTicketRecordsFromShowTicket()
		{
			if(colShowTicketDosTicketRecordsFromShowTicket == null)
			{
				colShowTicketDosTicketRecordsFromShowTicket = new Wcss.ShowTicketDosTicketCollection().Where(ShowTicketDosTicket.Columns.DosId, Id).Load();
				colShowTicketDosTicketRecordsFromShowTicket.ListChanged += new ListChangedEventHandler(colShowTicketDosTicketRecordsFromShowTicket_ListChanged);
			}
			return colShowTicketDosTicketRecordsFromShowTicket;
		}
				
		void colShowTicketDosTicketRecordsFromShowTicket_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colShowTicketDosTicketRecordsFromShowTicket[e.NewIndex].DosId = Id;
				colShowTicketDosTicketRecordsFromShowTicket.ListChanged += new ListChangedEventHandler(colShowTicketDosTicketRecordsFromShowTicket_ListChanged);
            }
		}
		private Wcss.ShowTicketPackageLinkCollection colShowTicketPackageLinkRecords;
		public Wcss.ShowTicketPackageLinkCollection ShowTicketPackageLinkRecords()
		{
			if(colShowTicketPackageLinkRecords == null)
			{
				colShowTicketPackageLinkRecords = new Wcss.ShowTicketPackageLinkCollection().Where(ShowTicketPackageLink.Columns.ParentShowTicketId, Id).Load();
				colShowTicketPackageLinkRecords.ListChanged += new ListChangedEventHandler(colShowTicketPackageLinkRecords_ListChanged);
			}
			return colShowTicketPackageLinkRecords;
		}
				
		void colShowTicketPackageLinkRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colShowTicketPackageLinkRecords[e.NewIndex].ParentShowTicketId = Id;
				colShowTicketPackageLinkRecords.ListChanged += new ListChangedEventHandler(colShowTicketPackageLinkRecords_ListChanged);
            }
		}
		private Wcss.ShowTicketPackageLinkCollection colShowTicketPackageLinkRecordsFromShowTicket;
		public Wcss.ShowTicketPackageLinkCollection ShowTicketPackageLinkRecordsFromShowTicket()
		{
			if(colShowTicketPackageLinkRecordsFromShowTicket == null)
			{
				colShowTicketPackageLinkRecordsFromShowTicket = new Wcss.ShowTicketPackageLinkCollection().Where(ShowTicketPackageLink.Columns.LinkedShowTicketId, Id).Load();
				colShowTicketPackageLinkRecordsFromShowTicket.ListChanged += new ListChangedEventHandler(colShowTicketPackageLinkRecordsFromShowTicket_ListChanged);
			}
			return colShowTicketPackageLinkRecordsFromShowTicket;
		}
				
		void colShowTicketPackageLinkRecordsFromShowTicket_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colShowTicketPackageLinkRecordsFromShowTicket[e.NewIndex].LinkedShowTicketId = Id;
				colShowTicketPackageLinkRecordsFromShowTicket.ListChanged += new ListChangedEventHandler(colShowTicketPackageLinkRecordsFromShowTicket_ListChanged);
            }
		}
		#endregion
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a ShowDate ActiveRecord object related to this ShowTicket
		/// 
		/// </summary>
		private Wcss.ShowDate ShowDate
		{
			get { return Wcss.ShowDate.FetchByID(this.TShowDateId); }
			set { SetColumnValue("TShowDateId", value.Id); }
		}
        //set up an alternate mechanism to avoid a database call
		private Wcss.ShowDate _showdaterecord = null;
		
		public Wcss.ShowDate ShowDateRecord
		{
		    get
            {
                if (_showdaterecord == null)
                {
                    _showdaterecord = new Wcss.ShowDate();
                    _showdaterecord.CopyFrom(this.ShowDate);
                }
                return _showdaterecord;
            }
            set
            {
                if(value != null && _showdaterecord == null)
			        _showdaterecord = new Wcss.ShowDate();
                
                SetColumnValue("TShowDateId", value.Id);
                _showdaterecord.CopyFrom(value);                
            }
		}
		
		
		#endregion
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(int varTVendorId,DateTime varDtDateOfShow,string varCriteriaText,string varSalesDescription,int varTShowDateId,int varTShowId,int varTAgeId,bool varBActive,bool varBSoldOut,string varStatus,bool varBDosTicket,int varIDisplayOrder,string varPriceText,decimal? varMPrice,string varDosText,decimal? varMDosPrice,decimal? varMServiceCharge,bool varBAllowShipping,bool varBAllowWillCall,bool varBHideShipMethod,DateTime varDtShipCutoff,bool varBOverrideSellout,bool varBUnlockActive,string varUnlockCode,DateTime? varDtUnlockDate,DateTime? varDtUnlockEndDate,DateTime? varDtPublicOnsale,DateTime? varDtEndDate,int? varIMaxQtyPerOrder,int varIAllotment,int varIPending,int varISold,int? varIAvailable,int varIRefunded,decimal? varMFlatShip,string varVcFlatMethod,DateTime? varDtBackorder,bool? varBShipSeparate,DateTime varDtStamp)
		{
			ShowTicket item = new ShowTicket();
			
			item.TVendorId = varTVendorId;
			
			item.DtDateOfShow = varDtDateOfShow;
			
			item.CriteriaText = varCriteriaText;
			
			item.SalesDescription = varSalesDescription;
			
			item.TShowDateId = varTShowDateId;
			
			item.TShowId = varTShowId;
			
			item.TAgeId = varTAgeId;
			
			item.BActive = varBActive;
			
			item.BSoldOut = varBSoldOut;
			
			item.Status = varStatus;
			
			item.BDosTicket = varBDosTicket;
			
			item.IDisplayOrder = varIDisplayOrder;
			
			item.PriceText = varPriceText;
			
			item.MPrice = varMPrice;
			
			item.DosText = varDosText;
			
			item.MDosPrice = varMDosPrice;
			
			item.MServiceCharge = varMServiceCharge;
			
			item.BAllowShipping = varBAllowShipping;
			
			item.BAllowWillCall = varBAllowWillCall;
			
			item.BHideShipMethod = varBHideShipMethod;
			
			item.DtShipCutoff = varDtShipCutoff;
			
			item.BOverrideSellout = varBOverrideSellout;
			
			item.BUnlockActive = varBUnlockActive;
			
			item.UnlockCode = varUnlockCode;
			
			item.DtUnlockDate = varDtUnlockDate;
			
			item.DtUnlockEndDate = varDtUnlockEndDate;
			
			item.DtPublicOnsale = varDtPublicOnsale;
			
			item.DtEndDate = varDtEndDate;
			
			item.IMaxQtyPerOrder = varIMaxQtyPerOrder;
			
			item.IAllotment = varIAllotment;
			
			item.IPending = varIPending;
			
			item.ISold = varISold;
			
			item.IAvailable = varIAvailable;
			
			item.IRefunded = varIRefunded;
			
			item.MFlatShip = varMFlatShip;
			
			item.VcFlatMethod = varVcFlatMethod;
			
			item.DtBackorder = varDtBackorder;
			
			item.BShipSeparate = varBShipSeparate;
			
			item.DtStamp = varDtStamp;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varId,int varTVendorId,DateTime varDtDateOfShow,string varCriteriaText,string varSalesDescription,int varTShowDateId,int varTShowId,int varTAgeId,bool varBActive,bool varBSoldOut,string varStatus,bool varBDosTicket,int varIDisplayOrder,string varPriceText,decimal? varMPrice,string varDosText,decimal? varMDosPrice,decimal? varMServiceCharge,bool varBAllowShipping,bool varBAllowWillCall,bool varBHideShipMethod,DateTime varDtShipCutoff,bool varBOverrideSellout,bool varBUnlockActive,string varUnlockCode,DateTime? varDtUnlockDate,DateTime? varDtUnlockEndDate,DateTime? varDtPublicOnsale,DateTime? varDtEndDate,int? varIMaxQtyPerOrder,int varIAllotment,int varIPending,int varISold,int? varIAvailable,int varIRefunded,decimal? varMFlatShip,string varVcFlatMethod,DateTime? varDtBackorder,bool? varBShipSeparate,DateTime varDtStamp)
		{
			ShowTicket item = new ShowTicket();
			
				item.Id = varId;
			
				item.TVendorId = varTVendorId;
			
				item.DtDateOfShow = varDtDateOfShow;
			
				item.CriteriaText = varCriteriaText;
			
				item.SalesDescription = varSalesDescription;
			
				item.TShowDateId = varTShowDateId;
			
				item.TShowId = varTShowId;
			
				item.TAgeId = varTAgeId;
			
				item.BActive = varBActive;
			
				item.BSoldOut = varBSoldOut;
			
				item.Status = varStatus;
			
				item.BDosTicket = varBDosTicket;
			
				item.IDisplayOrder = varIDisplayOrder;
			
				item.PriceText = varPriceText;
			
				item.MPrice = varMPrice;
			
				item.DosText = varDosText;
			
				item.MDosPrice = varMDosPrice;
			
				item.MServiceCharge = varMServiceCharge;
			
				item.BAllowShipping = varBAllowShipping;
			
				item.BAllowWillCall = varBAllowWillCall;
			
				item.BHideShipMethod = varBHideShipMethod;
			
				item.DtShipCutoff = varDtShipCutoff;
			
				item.BOverrideSellout = varBOverrideSellout;
			
				item.BUnlockActive = varBUnlockActive;
			
				item.UnlockCode = varUnlockCode;
			
				item.DtUnlockDate = varDtUnlockDate;
			
				item.DtUnlockEndDate = varDtUnlockEndDate;
			
				item.DtPublicOnsale = varDtPublicOnsale;
			
				item.DtEndDate = varDtEndDate;
			
				item.IMaxQtyPerOrder = varIMaxQtyPerOrder;
			
				item.IAllotment = varIAllotment;
			
				item.IPending = varIPending;
			
				item.ISold = varISold;
			
				item.IAvailable = varIAvailable;
			
				item.IRefunded = varIRefunded;
			
				item.MFlatShip = varMFlatShip;
			
				item.VcFlatMethod = varVcFlatMethod;
			
				item.DtBackorder = varDtBackorder;
			
				item.BShipSeparate = varBShipSeparate;
			
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
        
        
        
        public static TableSchema.TableColumn TVendorIdColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn DtDateOfShowColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn CriteriaTextColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn SalesDescriptionColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn TShowDateIdColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn TShowIdColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn TAgeIdColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn BActiveColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn BSoldOutColumn
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        public static TableSchema.TableColumn StatusColumn
        {
            get { return Schema.Columns[10]; }
        }
        
        
        
        public static TableSchema.TableColumn BDosTicketColumn
        {
            get { return Schema.Columns[11]; }
        }
        
        
        
        public static TableSchema.TableColumn IDisplayOrderColumn
        {
            get { return Schema.Columns[12]; }
        }
        
        
        
        public static TableSchema.TableColumn PriceTextColumn
        {
            get { return Schema.Columns[13]; }
        }
        
        
        
        public static TableSchema.TableColumn MPriceColumn
        {
            get { return Schema.Columns[14]; }
        }
        
        
        
        public static TableSchema.TableColumn DosTextColumn
        {
            get { return Schema.Columns[15]; }
        }
        
        
        
        public static TableSchema.TableColumn MDosPriceColumn
        {
            get { return Schema.Columns[16]; }
        }
        
        
        
        public static TableSchema.TableColumn MServiceChargeColumn
        {
            get { return Schema.Columns[17]; }
        }
        
        
        
        public static TableSchema.TableColumn BAllowShippingColumn
        {
            get { return Schema.Columns[18]; }
        }
        
        
        
        public static TableSchema.TableColumn BAllowWillCallColumn
        {
            get { return Schema.Columns[19]; }
        }
        
        
        
        public static TableSchema.TableColumn BHideShipMethodColumn
        {
            get { return Schema.Columns[20]; }
        }
        
        
        
        public static TableSchema.TableColumn DtShipCutoffColumn
        {
            get { return Schema.Columns[21]; }
        }
        
        
        
        public static TableSchema.TableColumn BOverrideSelloutColumn
        {
            get { return Schema.Columns[22]; }
        }
        
        
        
        public static TableSchema.TableColumn BUnlockActiveColumn
        {
            get { return Schema.Columns[23]; }
        }
        
        
        
        public static TableSchema.TableColumn UnlockCodeColumn
        {
            get { return Schema.Columns[24]; }
        }
        
        
        
        public static TableSchema.TableColumn DtUnlockDateColumn
        {
            get { return Schema.Columns[25]; }
        }
        
        
        
        public static TableSchema.TableColumn DtUnlockEndDateColumn
        {
            get { return Schema.Columns[26]; }
        }
        
        
        
        public static TableSchema.TableColumn DtPublicOnsaleColumn
        {
            get { return Schema.Columns[27]; }
        }
        
        
        
        public static TableSchema.TableColumn DtEndDateColumn
        {
            get { return Schema.Columns[28]; }
        }
        
        
        
        public static TableSchema.TableColumn IMaxQtyPerOrderColumn
        {
            get { return Schema.Columns[29]; }
        }
        
        
        
        public static TableSchema.TableColumn IAllotmentColumn
        {
            get { return Schema.Columns[30]; }
        }
        
        
        
        public static TableSchema.TableColumn IPendingColumn
        {
            get { return Schema.Columns[31]; }
        }
        
        
        
        public static TableSchema.TableColumn ISoldColumn
        {
            get { return Schema.Columns[32]; }
        }
        
        
        
        public static TableSchema.TableColumn IAvailableColumn
        {
            get { return Schema.Columns[33]; }
        }
        
        
        
        public static TableSchema.TableColumn IRefundedColumn
        {
            get { return Schema.Columns[34]; }
        }
        
        
        
        public static TableSchema.TableColumn MFlatShipColumn
        {
            get { return Schema.Columns[35]; }
        }
        
        
        
        public static TableSchema.TableColumn VcFlatMethodColumn
        {
            get { return Schema.Columns[36]; }
        }
        
        
        
        public static TableSchema.TableColumn DtBackorderColumn
        {
            get { return Schema.Columns[37]; }
        }
        
        
        
        public static TableSchema.TableColumn BShipSeparateColumn
        {
            get { return Schema.Columns[38]; }
        }
        
        
        
        public static TableSchema.TableColumn DtStampColumn
        {
            get { return Schema.Columns[39]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Id = @"Id";
			 public static string TVendorId = @"TVendorId";
			 public static string DtDateOfShow = @"dtDateOfShow";
			 public static string CriteriaText = @"CriteriaText";
			 public static string SalesDescription = @"SalesDescription";
			 public static string TShowDateId = @"TShowDateId";
			 public static string TShowId = @"TShowId";
			 public static string TAgeId = @"TAgeId";
			 public static string BActive = @"bActive";
			 public static string BSoldOut = @"bSoldOut";
			 public static string Status = @"Status";
			 public static string BDosTicket = @"bDosTicket";
			 public static string IDisplayOrder = @"iDisplayOrder";
			 public static string PriceText = @"PriceText";
			 public static string MPrice = @"mPrice";
			 public static string DosText = @"DosText";
			 public static string MDosPrice = @"mDosPrice";
			 public static string MServiceCharge = @"mServiceCharge";
			 public static string BAllowShipping = @"bAllowShipping";
			 public static string BAllowWillCall = @"bAllowWillCall";
			 public static string BHideShipMethod = @"bHideShipMethod";
			 public static string DtShipCutoff = @"dtShipCutoff";
			 public static string BOverrideSellout = @"bOverrideSellout";
			 public static string BUnlockActive = @"bUnlockActive";
			 public static string UnlockCode = @"UnlockCode";
			 public static string DtUnlockDate = @"dtUnlockDate";
			 public static string DtUnlockEndDate = @"dtUnlockEndDate";
			 public static string DtPublicOnsale = @"dtPublicOnsale";
			 public static string DtEndDate = @"dtEndDate";
			 public static string IMaxQtyPerOrder = @"iMaxQtyPerOrder";
			 public static string IAllotment = @"iAllotment";
			 public static string IPending = @"iPending";
			 public static string ISold = @"iSold";
			 public static string IAvailable = @"iAvailable";
			 public static string IRefunded = @"iRefunded";
			 public static string MFlatShip = @"mFlatShip";
			 public static string VcFlatMethod = @"vcFlatMethod";
			 public static string DtBackorder = @"dtBackorder";
			 public static string BShipSeparate = @"bShipSeparate";
			 public static string DtStamp = @"dtStamp";
						
		}
		#endregion
		
		#region Update PK Collections
		
        public void SetPKValues()
        {
                if (colHistoryInventoryRecords != null)
                {
                    foreach (Wcss.HistoryInventory item in colHistoryInventoryRecords)
                    {
                        if (item.TShowTicketId != Id)
                        {
                            item.TShowTicketId = Id;
                        }
                    }
               }
		
                if (colHistoryPricingRecords != null)
                {
                    foreach (Wcss.HistoryPricing item in colHistoryPricingRecords)
                    {
                        if (item.TShowTicketId != Id)
                        {
                            item.TShowTicketId = Id;
                        }
                    }
               }
		
                if (colInvoiceItemRecords != null)
                {
                    foreach (Wcss.InvoiceItem item in colInvoiceItemRecords)
                    {
                        if (item.TShowTicketId != Id)
                        {
                            item.TShowTicketId = Id;
                        }
                    }
               }
		
                if (colLotteryRecords != null)
                {
                    foreach (Wcss.Lottery item in colLotteryRecords)
                    {
                        if (item.TShowTicketId != Id)
                        {
                            item.TShowTicketId = Id;
                        }
                    }
               }
		
                if (colMerchBundleRecords != null)
                {
                    foreach (Wcss.MerchBundle item in colMerchBundleRecords)
                    {
                        if (item.TShowTicketId != Id)
                        {
                            item.TShowTicketId = Id;
                        }
                    }
               }
		
                if (colPostPurchaseTextRecords != null)
                {
                    foreach (Wcss.PostPurchaseText item in colPostPurchaseTextRecords)
                    {
                        if (item.TShowTicketId != Id)
                        {
                            item.TShowTicketId = Id;
                        }
                    }
               }
		
                if (colRequiredShowTicketPastPurchaseRecords != null)
                {
                    foreach (Wcss.RequiredShowTicketPastPurchase item in colRequiredShowTicketPastPurchaseRecords)
                    {
                        if (item.TShowTicketId != Id)
                        {
                            item.TShowTicketId = Id;
                        }
                    }
               }
		
                if (colSalePromotionRecords != null)
                {
                    foreach (Wcss.SalePromotion item in colSalePromotionRecords)
                    {
                        if (item.TShowTicketId != Id)
                        {
                            item.TShowTicketId = Id;
                        }
                    }
               }
		
                if (colSalePromotionRecordsFromShowTicket != null)
                {
                    foreach (Wcss.SalePromotion item in colSalePromotionRecordsFromShowTicket)
                    {
                        if (item.TRequiredParentShowTicketId != Id)
                        {
                            item.TRequiredParentShowTicketId = Id;
                        }
                    }
               }
		
                if (colShowTicketDosTicketRecords != null)
                {
                    foreach (Wcss.ShowTicketDosTicket item in colShowTicketDosTicketRecords)
                    {
                        if (item.ParentId != Id)
                        {
                            item.ParentId = Id;
                        }
                    }
               }
		
                if (colShowTicketDosTicketRecordsFromShowTicket != null)
                {
                    foreach (Wcss.ShowTicketDosTicket item in colShowTicketDosTicketRecordsFromShowTicket)
                    {
                        if (item.DosId != Id)
                        {
                            item.DosId = Id;
                        }
                    }
               }
		
                if (colShowTicketPackageLinkRecords != null)
                {
                    foreach (Wcss.ShowTicketPackageLink item in colShowTicketPackageLinkRecords)
                    {
                        if (item.ParentShowTicketId != Id)
                        {
                            item.ParentShowTicketId = Id;
                        }
                    }
               }
		
                if (colShowTicketPackageLinkRecordsFromShowTicket != null)
                {
                    foreach (Wcss.ShowTicketPackageLink item in colShowTicketPackageLinkRecordsFromShowTicket)
                    {
                        if (item.LinkedShowTicketId != Id)
                        {
                            item.LinkedShowTicketId = Id;
                        }
                    }
               }
		}
        #endregion
    
        #region Deep Save
		
        public void DeepSave()
        {
            Save();
            
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
		
                if (colLotteryRecords != null)
                {
                    colLotteryRecords.SaveAll();
               }
		
                if (colMerchBundleRecords != null)
                {
                    colMerchBundleRecords.SaveAll();
               }
		
                if (colPostPurchaseTextRecords != null)
                {
                    colPostPurchaseTextRecords.SaveAll();
               }
		
                if (colRequiredShowTicketPastPurchaseRecords != null)
                {
                    colRequiredShowTicketPastPurchaseRecords.SaveAll();
               }
		
                if (colSalePromotionRecords != null)
                {
                    colSalePromotionRecords.SaveAll();
               }
		
                if (colSalePromotionRecordsFromShowTicket != null)
                {
                    colSalePromotionRecordsFromShowTicket.SaveAll();
               }
		
                if (colShowTicketDosTicketRecords != null)
                {
                    colShowTicketDosTicketRecords.SaveAll();
               }
		
                if (colShowTicketDosTicketRecordsFromShowTicket != null)
                {
                    colShowTicketDosTicketRecordsFromShowTicket.SaveAll();
               }
		
                if (colShowTicketPackageLinkRecords != null)
                {
                    colShowTicketPackageLinkRecords.SaveAll();
               }
		
                if (colShowTicketPackageLinkRecordsFromShowTicket != null)
                {
                    colShowTicketPackageLinkRecordsFromShowTicket.SaveAll();
               }
		}
        #endregion
	}
}
