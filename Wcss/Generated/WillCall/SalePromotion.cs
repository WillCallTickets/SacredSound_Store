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
	/// Strongly-typed collection for the SalePromotion class.
	/// </summary>
    [Serializable]
	public partial class SalePromotionCollection : ActiveList<SalePromotion, SalePromotionCollection>
	{	   
		public SalePromotionCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>SalePromotionCollection</returns>
		public SalePromotionCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                SalePromotion o = this[i];
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
	/// This is an ActiveRecord class which wraps the SalePromotion table.
	/// </summary>
	[Serializable]
	public partial class SalePromotion : ActiveRecord<SalePromotion>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public SalePromotion()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public SalePromotion(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public SalePromotion(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public SalePromotion(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("SalePromotion", TableType.Table, DataService.GetInstance("WillCall"));
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
				colvarDtStamp.IsNullable = true;
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
				
				TableSchema.TableColumn colvarIDisplayOrder = new TableSchema.TableColumn(schema);
				colvarIDisplayOrder.ColumnName = "iDisplayOrder";
				colvarIDisplayOrder.DataType = DbType.Int32;
				colvarIDisplayOrder.MaxLength = 0;
				colvarIDisplayOrder.AutoIncrement = false;
				colvarIDisplayOrder.IsNullable = false;
				colvarIDisplayOrder.IsPrimaryKey = false;
				colvarIDisplayOrder.IsForeignKey = false;
				colvarIDisplayOrder.IsReadOnly = false;
				
						colvarIDisplayOrder.DefaultSetting = @"((-1))";
				colvarIDisplayOrder.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIDisplayOrder);
				
				TableSchema.TableColumn colvarIBannerTimeoutMsecs = new TableSchema.TableColumn(schema);
				colvarIBannerTimeoutMsecs.ColumnName = "iBannerTimeoutMsecs";
				colvarIBannerTimeoutMsecs.DataType = DbType.Int32;
				colvarIBannerTimeoutMsecs.MaxLength = 0;
				colvarIBannerTimeoutMsecs.AutoIncrement = false;
				colvarIBannerTimeoutMsecs.IsNullable = false;
				colvarIBannerTimeoutMsecs.IsPrimaryKey = false;
				colvarIBannerTimeoutMsecs.IsForeignKey = false;
				colvarIBannerTimeoutMsecs.IsReadOnly = false;
				
						colvarIBannerTimeoutMsecs.DefaultSetting = @"((2400))";
				colvarIBannerTimeoutMsecs.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIBannerTimeoutMsecs);
				
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
				
				TableSchema.TableColumn colvarDisplayText = new TableSchema.TableColumn(schema);
				colvarDisplayText.ColumnName = "DisplayText";
				colvarDisplayText.DataType = DbType.AnsiString;
				colvarDisplayText.MaxLength = 1000;
				colvarDisplayText.AutoIncrement = false;
				colvarDisplayText.IsNullable = true;
				colvarDisplayText.IsPrimaryKey = false;
				colvarDisplayText.IsForeignKey = false;
				colvarDisplayText.IsReadOnly = false;
				colvarDisplayText.DefaultSetting = @"";
				colvarDisplayText.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDisplayText);
				
				TableSchema.TableColumn colvarAdditionalText = new TableSchema.TableColumn(schema);
				colvarAdditionalText.ColumnName = "AdditionalText";
				colvarAdditionalText.DataType = DbType.AnsiString;
				colvarAdditionalText.MaxLength = 500;
				colvarAdditionalText.AutoIncrement = false;
				colvarAdditionalText.IsNullable = true;
				colvarAdditionalText.IsPrimaryKey = false;
				colvarAdditionalText.IsForeignKey = false;
				colvarAdditionalText.IsReadOnly = false;
				colvarAdditionalText.DefaultSetting = @"";
				colvarAdditionalText.ForeignKeyTableName = "";
				schema.Columns.Add(colvarAdditionalText);
				
				TableSchema.TableColumn colvarTShowTicketId = new TableSchema.TableColumn(schema);
				colvarTShowTicketId.ColumnName = "tShowTicketId";
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
				
				TableSchema.TableColumn colvarRequiredPromotionCode = new TableSchema.TableColumn(schema);
				colvarRequiredPromotionCode.ColumnName = "RequiredPromotionCode";
				colvarRequiredPromotionCode.DataType = DbType.AnsiString;
				colvarRequiredPromotionCode.MaxLength = 50;
				colvarRequiredPromotionCode.AutoIncrement = false;
				colvarRequiredPromotionCode.IsNullable = true;
				colvarRequiredPromotionCode.IsPrimaryKey = false;
				colvarRequiredPromotionCode.IsForeignKey = false;
				colvarRequiredPromotionCode.IsReadOnly = false;
				colvarRequiredPromotionCode.DefaultSetting = @"";
				colvarRequiredPromotionCode.ForeignKeyTableName = "";
				schema.Columns.Add(colvarRequiredPromotionCode);
				
				TableSchema.TableColumn colvarTRequiredParentShowTicketId = new TableSchema.TableColumn(schema);
				colvarTRequiredParentShowTicketId.ColumnName = "tRequiredParentShowTicketId";
				colvarTRequiredParentShowTicketId.DataType = DbType.Int32;
				colvarTRequiredParentShowTicketId.MaxLength = 0;
				colvarTRequiredParentShowTicketId.AutoIncrement = false;
				colvarTRequiredParentShowTicketId.IsNullable = true;
				colvarTRequiredParentShowTicketId.IsPrimaryKey = false;
				colvarTRequiredParentShowTicketId.IsForeignKey = true;
				colvarTRequiredParentShowTicketId.IsReadOnly = false;
				colvarTRequiredParentShowTicketId.DefaultSetting = @"";
				
					colvarTRequiredParentShowTicketId.ForeignKeyTableName = "ShowTicket";
				schema.Columns.Add(colvarTRequiredParentShowTicketId);
				
				TableSchema.TableColumn colvarTRequiredParentShowDateId = new TableSchema.TableColumn(schema);
				colvarTRequiredParentShowDateId.ColumnName = "tRequiredParentShowDateId";
				colvarTRequiredParentShowDateId.DataType = DbType.Int32;
				colvarTRequiredParentShowDateId.MaxLength = 0;
				colvarTRequiredParentShowDateId.AutoIncrement = false;
				colvarTRequiredParentShowDateId.IsNullable = true;
				colvarTRequiredParentShowDateId.IsPrimaryKey = false;
				colvarTRequiredParentShowDateId.IsForeignKey = true;
				colvarTRequiredParentShowDateId.IsReadOnly = false;
				colvarTRequiredParentShowDateId.DefaultSetting = @"";
				
					colvarTRequiredParentShowDateId.ForeignKeyTableName = "ShowDate";
				schema.Columns.Add(colvarTRequiredParentShowDateId);
				
				TableSchema.TableColumn colvarIRequiredParentQty = new TableSchema.TableColumn(schema);
				colvarIRequiredParentQty.ColumnName = "iRequiredParentQty";
				colvarIRequiredParentQty.DataType = DbType.Int32;
				colvarIRequiredParentQty.MaxLength = 0;
				colvarIRequiredParentQty.AutoIncrement = false;
				colvarIRequiredParentQty.IsNullable = false;
				colvarIRequiredParentQty.IsPrimaryKey = false;
				colvarIRequiredParentQty.IsForeignKey = false;
				colvarIRequiredParentQty.IsReadOnly = false;
				
						colvarIRequiredParentQty.DefaultSetting = @"((1))";
				colvarIRequiredParentQty.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIRequiredParentQty);
				
				TableSchema.TableColumn colvarMPrice = new TableSchema.TableColumn(schema);
				colvarMPrice.ColumnName = "mPrice";
				colvarMPrice.DataType = DbType.Currency;
				colvarMPrice.MaxLength = 0;
				colvarMPrice.AutoIncrement = false;
				colvarMPrice.IsNullable = false;
				colvarMPrice.IsPrimaryKey = false;
				colvarMPrice.IsForeignKey = false;
				colvarMPrice.IsReadOnly = false;
				
						colvarMPrice.DefaultSetting = @"((0))";
				colvarMPrice.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMPrice);
				
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
				
				TableSchema.TableColumn colvarIDiscountPercent = new TableSchema.TableColumn(schema);
				colvarIDiscountPercent.ColumnName = "iDiscountPercent";
				colvarIDiscountPercent.DataType = DbType.Int32;
				colvarIDiscountPercent.MaxLength = 0;
				colvarIDiscountPercent.AutoIncrement = false;
				colvarIDiscountPercent.IsNullable = false;
				colvarIDiscountPercent.IsPrimaryKey = false;
				colvarIDiscountPercent.IsForeignKey = false;
				colvarIDiscountPercent.IsReadOnly = false;
				
						colvarIDiscountPercent.DefaultSetting = @"((0))";
				colvarIDiscountPercent.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIDiscountPercent);
				
				TableSchema.TableColumn colvarVcDiscountContext = new TableSchema.TableColumn(schema);
				colvarVcDiscountContext.ColumnName = "vcDiscountContext";
				colvarVcDiscountContext.DataType = DbType.AnsiString;
				colvarVcDiscountContext.MaxLength = 256;
				colvarVcDiscountContext.AutoIncrement = false;
				colvarVcDiscountContext.IsNullable = true;
				colvarVcDiscountContext.IsPrimaryKey = false;
				colvarVcDiscountContext.IsForeignKey = false;
				colvarVcDiscountContext.IsReadOnly = false;
				colvarVcDiscountContext.DefaultSetting = @"";
				colvarVcDiscountContext.ForeignKeyTableName = "";
				schema.Columns.Add(colvarVcDiscountContext);
				
				TableSchema.TableColumn colvarMMinMerch = new TableSchema.TableColumn(schema);
				colvarMMinMerch.ColumnName = "mMinMerch";
				colvarMMinMerch.DataType = DbType.Currency;
				colvarMMinMerch.MaxLength = 0;
				colvarMMinMerch.AutoIncrement = false;
				colvarMMinMerch.IsNullable = false;
				colvarMMinMerch.IsPrimaryKey = false;
				colvarMMinMerch.IsForeignKey = false;
				colvarMMinMerch.IsReadOnly = false;
				
						colvarMMinMerch.DefaultSetting = @"((0))";
				colvarMMinMerch.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMMinMerch);
				
				TableSchema.TableColumn colvarMMinTicket = new TableSchema.TableColumn(schema);
				colvarMMinTicket.ColumnName = "mMinTicket";
				colvarMMinTicket.DataType = DbType.Currency;
				colvarMMinTicket.MaxLength = 0;
				colvarMMinTicket.AutoIncrement = false;
				colvarMMinTicket.IsNullable = false;
				colvarMMinTicket.IsPrimaryKey = false;
				colvarMMinTicket.IsForeignKey = false;
				colvarMMinTicket.IsReadOnly = false;
				
						colvarMMinTicket.DefaultSetting = @"((0))";
				colvarMMinTicket.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMMinTicket);
				
				TableSchema.TableColumn colvarMMinTotal = new TableSchema.TableColumn(schema);
				colvarMMinTotal.ColumnName = "mMinTotal";
				colvarMMinTotal.DataType = DbType.Currency;
				colvarMMinTotal.MaxLength = 0;
				colvarMMinTotal.AutoIncrement = false;
				colvarMMinTotal.IsNullable = false;
				colvarMMinTotal.IsPrimaryKey = false;
				colvarMMinTotal.IsForeignKey = false;
				colvarMMinTotal.IsReadOnly = false;
				
						colvarMMinTotal.DefaultSetting = @"((0))";
				colvarMMinTotal.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMMinTotal);
				
				TableSchema.TableColumn colvarBannerUrl = new TableSchema.TableColumn(schema);
				colvarBannerUrl.ColumnName = "BannerUrl";
				colvarBannerUrl.DataType = DbType.AnsiString;
				colvarBannerUrl.MaxLength = 256;
				colvarBannerUrl.AutoIncrement = false;
				colvarBannerUrl.IsNullable = true;
				colvarBannerUrl.IsPrimaryKey = false;
				colvarBannerUrl.IsForeignKey = false;
				colvarBannerUrl.IsReadOnly = false;
				colvarBannerUrl.DefaultSetting = @"";
				colvarBannerUrl.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBannerUrl);
				
				TableSchema.TableColumn colvarBannerClickUrl = new TableSchema.TableColumn(schema);
				colvarBannerClickUrl.ColumnName = "BannerClickUrl";
				colvarBannerClickUrl.DataType = DbType.AnsiString;
				colvarBannerClickUrl.MaxLength = 256;
				colvarBannerClickUrl.AutoIncrement = false;
				colvarBannerClickUrl.IsNullable = true;
				colvarBannerClickUrl.IsPrimaryKey = false;
				colvarBannerClickUrl.IsForeignKey = false;
				colvarBannerClickUrl.IsReadOnly = false;
				colvarBannerClickUrl.DefaultSetting = @"";
				colvarBannerClickUrl.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBannerClickUrl);
				
				TableSchema.TableColumn colvarBDisplayAtParent = new TableSchema.TableColumn(schema);
				colvarBDisplayAtParent.ColumnName = "bDisplayAtParent";
				colvarBDisplayAtParent.DataType = DbType.Boolean;
				colvarBDisplayAtParent.MaxLength = 0;
				colvarBDisplayAtParent.AutoIncrement = false;
				colvarBDisplayAtParent.IsNullable = false;
				colvarBDisplayAtParent.IsPrimaryKey = false;
				colvarBDisplayAtParent.IsForeignKey = false;
				colvarBDisplayAtParent.IsReadOnly = false;
				
						colvarBDisplayAtParent.DefaultSetting = @"((0))";
				colvarBDisplayAtParent.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBDisplayAtParent);
				
				TableSchema.TableColumn colvarBBannerMerch = new TableSchema.TableColumn(schema);
				colvarBBannerMerch.ColumnName = "bBannerMerch";
				colvarBBannerMerch.DataType = DbType.Boolean;
				colvarBBannerMerch.MaxLength = 0;
				colvarBBannerMerch.AutoIncrement = false;
				colvarBBannerMerch.IsNullable = false;
				colvarBBannerMerch.IsPrimaryKey = false;
				colvarBBannerMerch.IsForeignKey = false;
				colvarBBannerMerch.IsReadOnly = false;
				
						colvarBBannerMerch.DefaultSetting = @"((0))";
				colvarBBannerMerch.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBBannerMerch);
				
				TableSchema.TableColumn colvarBBannerTicket = new TableSchema.TableColumn(schema);
				colvarBBannerTicket.ColumnName = "bBannerTicket";
				colvarBBannerTicket.DataType = DbType.Boolean;
				colvarBBannerTicket.MaxLength = 0;
				colvarBBannerTicket.AutoIncrement = false;
				colvarBBannerTicket.IsNullable = false;
				colvarBBannerTicket.IsPrimaryKey = false;
				colvarBBannerTicket.IsForeignKey = false;
				colvarBBannerTicket.IsReadOnly = false;
				
						colvarBBannerTicket.DefaultSetting = @"((0))";
				colvarBBannerTicket.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBBannerTicket);
				
				TableSchema.TableColumn colvarBBannerCartEdit = new TableSchema.TableColumn(schema);
				colvarBBannerCartEdit.ColumnName = "bBannerCartEdit";
				colvarBBannerCartEdit.DataType = DbType.Boolean;
				colvarBBannerCartEdit.MaxLength = 0;
				colvarBBannerCartEdit.AutoIncrement = false;
				colvarBBannerCartEdit.IsNullable = false;
				colvarBBannerCartEdit.IsPrimaryKey = false;
				colvarBBannerCartEdit.IsForeignKey = false;
				colvarBBannerCartEdit.IsReadOnly = false;
				
						colvarBBannerCartEdit.DefaultSetting = @"((0))";
				colvarBBannerCartEdit.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBBannerCartEdit);
				
				TableSchema.TableColumn colvarBBannerCheckout = new TableSchema.TableColumn(schema);
				colvarBBannerCheckout.ColumnName = "bBannerCheckout";
				colvarBBannerCheckout.DataType = DbType.Boolean;
				colvarBBannerCheckout.MaxLength = 0;
				colvarBBannerCheckout.AutoIncrement = false;
				colvarBBannerCheckout.IsNullable = false;
				colvarBBannerCheckout.IsPrimaryKey = false;
				colvarBBannerCheckout.IsForeignKey = false;
				colvarBBannerCheckout.IsReadOnly = false;
				
						colvarBBannerCheckout.DefaultSetting = @"((0))";
				colvarBBannerCheckout.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBBannerCheckout);
				
				TableSchema.TableColumn colvarBBannerShipping = new TableSchema.TableColumn(schema);
				colvarBBannerShipping.ColumnName = "bBannerShipping";
				colvarBBannerShipping.DataType = DbType.Boolean;
				colvarBBannerShipping.MaxLength = 0;
				colvarBBannerShipping.AutoIncrement = false;
				colvarBBannerShipping.IsNullable = false;
				colvarBBannerShipping.IsPrimaryKey = false;
				colvarBBannerShipping.IsForeignKey = false;
				colvarBBannerShipping.IsReadOnly = false;
				
						colvarBBannerShipping.DefaultSetting = @"((0))";
				colvarBBannerShipping.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBBannerShipping);
				
				TableSchema.TableColumn colvarShipOfferMethod = new TableSchema.TableColumn(schema);
				colvarShipOfferMethod.ColumnName = "ShipOfferMethod";
				colvarShipOfferMethod.DataType = DbType.AnsiString;
				colvarShipOfferMethod.MaxLength = 256;
				colvarShipOfferMethod.AutoIncrement = false;
				colvarShipOfferMethod.IsNullable = true;
				colvarShipOfferMethod.IsPrimaryKey = false;
				colvarShipOfferMethod.IsForeignKey = false;
				colvarShipOfferMethod.IsReadOnly = false;
				colvarShipOfferMethod.DefaultSetting = @"";
				colvarShipOfferMethod.ForeignKeyTableName = "";
				schema.Columns.Add(colvarShipOfferMethod);
				
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
				
				TableSchema.TableColumn colvarIMaxPerOrder = new TableSchema.TableColumn(schema);
				colvarIMaxPerOrder.ColumnName = "iMaxPerOrder";
				colvarIMaxPerOrder.DataType = DbType.Int32;
				colvarIMaxPerOrder.MaxLength = 0;
				colvarIMaxPerOrder.AutoIncrement = false;
				colvarIMaxPerOrder.IsNullable = false;
				colvarIMaxPerOrder.IsPrimaryKey = false;
				colvarIMaxPerOrder.IsForeignKey = false;
				colvarIMaxPerOrder.IsReadOnly = false;
				
						colvarIMaxPerOrder.DefaultSetting = @"((1))";
				colvarIMaxPerOrder.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIMaxPerOrder);
				
				TableSchema.TableColumn colvarMMaxValue = new TableSchema.TableColumn(schema);
				colvarMMaxValue.ColumnName = "mMaxValue";
				colvarMMaxValue.DataType = DbType.Currency;
				colvarMMaxValue.MaxLength = 0;
				colvarMMaxValue.AutoIncrement = false;
				colvarMMaxValue.IsNullable = true;
				colvarMMaxValue.IsPrimaryKey = false;
				colvarMMaxValue.IsForeignKey = false;
				colvarMMaxValue.IsReadOnly = false;
				colvarMMaxValue.DefaultSetting = @"";
				colvarMMaxValue.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMMaxValue);
				
				TableSchema.TableColumn colvarMWeight = new TableSchema.TableColumn(schema);
				colvarMWeight.ColumnName = "mWeight";
				colvarMWeight.DataType = DbType.Currency;
				colvarMWeight.MaxLength = 0;
				colvarMWeight.AutoIncrement = false;
				colvarMWeight.IsNullable = false;
				colvarMWeight.IsPrimaryKey = false;
				colvarMWeight.IsForeignKey = false;
				colvarMWeight.IsReadOnly = false;
				
						colvarMWeight.DefaultSetting = @"((0))";
				colvarMWeight.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMWeight);
				
				TableSchema.TableColumn colvarBDeactivateOnNoInventory = new TableSchema.TableColumn(schema);
				colvarBDeactivateOnNoInventory.ColumnName = "bDeactivateOnNoInventory";
				colvarBDeactivateOnNoInventory.DataType = DbType.Boolean;
				colvarBDeactivateOnNoInventory.MaxLength = 0;
				colvarBDeactivateOnNoInventory.AutoIncrement = false;
				colvarBDeactivateOnNoInventory.IsNullable = false;
				colvarBDeactivateOnNoInventory.IsPrimaryKey = false;
				colvarBDeactivateOnNoInventory.IsForeignKey = false;
				colvarBDeactivateOnNoInventory.IsReadOnly = false;
				
						colvarBDeactivateOnNoInventory.DefaultSetting = @"((1))";
				colvarBDeactivateOnNoInventory.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBDeactivateOnNoInventory);
				
				TableSchema.TableColumn colvarIMaxUsesPerUser = new TableSchema.TableColumn(schema);
				colvarIMaxUsesPerUser.ColumnName = "iMaxUsesPerUser";
				colvarIMaxUsesPerUser.DataType = DbType.Int32;
				colvarIMaxUsesPerUser.MaxLength = 0;
				colvarIMaxUsesPerUser.AutoIncrement = false;
				colvarIMaxUsesPerUser.IsNullable = false;
				colvarIMaxUsesPerUser.IsPrimaryKey = false;
				colvarIMaxUsesPerUser.IsForeignKey = false;
				colvarIMaxUsesPerUser.IsReadOnly = false;
				
						colvarIMaxUsesPerUser.DefaultSetting = @"((0))";
				colvarIMaxUsesPerUser.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIMaxUsesPerUser);
				
				TableSchema.TableColumn colvarVcTriggerListMerch = new TableSchema.TableColumn(schema);
				colvarVcTriggerListMerch.ColumnName = "vcTriggerList_Merch";
				colvarVcTriggerListMerch.DataType = DbType.AnsiString;
				colvarVcTriggerListMerch.MaxLength = 500;
				colvarVcTriggerListMerch.AutoIncrement = false;
				colvarVcTriggerListMerch.IsNullable = true;
				colvarVcTriggerListMerch.IsPrimaryKey = false;
				colvarVcTriggerListMerch.IsForeignKey = false;
				colvarVcTriggerListMerch.IsReadOnly = false;
				colvarVcTriggerListMerch.DefaultSetting = @"";
				colvarVcTriggerListMerch.ForeignKeyTableName = "";
				schema.Columns.Add(colvarVcTriggerListMerch);
				
				TableSchema.TableColumn colvarBAllowMultSelections = new TableSchema.TableColumn(schema);
				colvarBAllowMultSelections.ColumnName = "bAllowMultSelections";
				colvarBAllowMultSelections.DataType = DbType.Boolean;
				colvarBAllowMultSelections.MaxLength = 0;
				colvarBAllowMultSelections.AutoIncrement = false;
				colvarBAllowMultSelections.IsNullable = true;
				colvarBAllowMultSelections.IsPrimaryKey = false;
				colvarBAllowMultSelections.IsForeignKey = false;
				colvarBAllowMultSelections.IsReadOnly = false;
				colvarBAllowMultSelections.DefaultSetting = @"";
				colvarBAllowMultSelections.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBAllowMultSelections);
				
				TableSchema.TableColumn colvarBAllowPromoTotalInMinimum = new TableSchema.TableColumn(schema);
				colvarBAllowPromoTotalInMinimum.ColumnName = "bAllowPromoTotalInMinimum";
				colvarBAllowPromoTotalInMinimum.DataType = DbType.Boolean;
				colvarBAllowPromoTotalInMinimum.MaxLength = 0;
				colvarBAllowPromoTotalInMinimum.AutoIncrement = false;
				colvarBAllowPromoTotalInMinimum.IsNullable = true;
				colvarBAllowPromoTotalInMinimum.IsPrimaryKey = false;
				colvarBAllowPromoTotalInMinimum.IsForeignKey = false;
				colvarBAllowPromoTotalInMinimum.IsReadOnly = false;
				colvarBAllowPromoTotalInMinimum.DefaultSetting = @"";
				colvarBAllowPromoTotalInMinimum.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBAllowPromoTotalInMinimum);
				
				TableSchema.TableColumn colvarJsonMeta = new TableSchema.TableColumn(schema);
				colvarJsonMeta.ColumnName = "jsonMeta";
				colvarJsonMeta.DataType = DbType.String;
				colvarJsonMeta.MaxLength = 1024;
				colvarJsonMeta.AutoIncrement = false;
				colvarJsonMeta.IsNullable = true;
				colvarJsonMeta.IsPrimaryKey = false;
				colvarJsonMeta.IsForeignKey = false;
				colvarJsonMeta.IsReadOnly = false;
				colvarJsonMeta.DefaultSetting = @"";
				colvarJsonMeta.ForeignKeyTableName = "";
				schema.Columns.Add(colvarJsonMeta);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["WillCall"].AddSchema("SalePromotion",schema);
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
		public DateTime? DtStamp 
		{
			get { return GetColumnValue<DateTime?>(Columns.DtStamp); }
			set { SetColumnValue(Columns.DtStamp, value); }
		}
		  
		[XmlAttribute("ApplicationId")]
		[Bindable(true)]
		public Guid ApplicationId 
		{
			get { return GetColumnValue<Guid>(Columns.ApplicationId); }
			set { SetColumnValue(Columns.ApplicationId, value); }
		}
		  
		[XmlAttribute("BActive")]
		[Bindable(true)]
		public bool BActive 
		{
			get { return GetColumnValue<bool>(Columns.BActive); }
			set { SetColumnValue(Columns.BActive, value); }
		}
		  
		[XmlAttribute("IDisplayOrder")]
		[Bindable(true)]
		public int IDisplayOrder 
		{
			get { return GetColumnValue<int>(Columns.IDisplayOrder); }
			set { SetColumnValue(Columns.IDisplayOrder, value); }
		}
		  
		[XmlAttribute("IBannerTimeoutMsecs")]
		[Bindable(true)]
		public int IBannerTimeoutMsecs 
		{
			get { return GetColumnValue<int>(Columns.IBannerTimeoutMsecs); }
			set { SetColumnValue(Columns.IBannerTimeoutMsecs, value); }
		}
		  
		[XmlAttribute("Name")]
		[Bindable(true)]
		public string Name 
		{
			get { return GetColumnValue<string>(Columns.Name); }
			set { SetColumnValue(Columns.Name, value); }
		}
		  
		[XmlAttribute("DisplayText")]
		[Bindable(true)]
		public string DisplayText 
		{
			get { return GetColumnValue<string>(Columns.DisplayText); }
			set { SetColumnValue(Columns.DisplayText, value); }
		}
		  
		[XmlAttribute("AdditionalText")]
		[Bindable(true)]
		public string AdditionalText 
		{
			get { return GetColumnValue<string>(Columns.AdditionalText); }
			set { SetColumnValue(Columns.AdditionalText, value); }
		}
		  
		[XmlAttribute("TShowTicketId")]
		[Bindable(true)]
		public int? TShowTicketId 
		{
			get { return GetColumnValue<int?>(Columns.TShowTicketId); }
			set { SetColumnValue(Columns.TShowTicketId, value); }
		}
		  
		[XmlAttribute("RequiredPromotionCode")]
		[Bindable(true)]
		public string RequiredPromotionCode 
		{
			get { return GetColumnValue<string>(Columns.RequiredPromotionCode); }
			set { SetColumnValue(Columns.RequiredPromotionCode, value); }
		}
		  
		[XmlAttribute("TRequiredParentShowTicketId")]
		[Bindable(true)]
		public int? TRequiredParentShowTicketId 
		{
			get { return GetColumnValue<int?>(Columns.TRequiredParentShowTicketId); }
			set { SetColumnValue(Columns.TRequiredParentShowTicketId, value); }
		}
		  
		[XmlAttribute("TRequiredParentShowDateId")]
		[Bindable(true)]
		public int? TRequiredParentShowDateId 
		{
			get { return GetColumnValue<int?>(Columns.TRequiredParentShowDateId); }
			set { SetColumnValue(Columns.TRequiredParentShowDateId, value); }
		}
		  
		[XmlAttribute("IRequiredParentQty")]
		[Bindable(true)]
		public int IRequiredParentQty 
		{
			get { return GetColumnValue<int>(Columns.IRequiredParentQty); }
			set { SetColumnValue(Columns.IRequiredParentQty, value); }
		}
		  
		[XmlAttribute("MPrice")]
		[Bindable(true)]
		public decimal MPrice 
		{
			get { return GetColumnValue<decimal>(Columns.MPrice); }
			set { SetColumnValue(Columns.MPrice, value); }
		}
		  
		[XmlAttribute("MDiscountAmount")]
		[Bindable(true)]
		public decimal MDiscountAmount 
		{
			get { return GetColumnValue<decimal>(Columns.MDiscountAmount); }
			set { SetColumnValue(Columns.MDiscountAmount, value); }
		}
		  
		[XmlAttribute("IDiscountPercent")]
		[Bindable(true)]
		public int IDiscountPercent 
		{
			get { return GetColumnValue<int>(Columns.IDiscountPercent); }
			set { SetColumnValue(Columns.IDiscountPercent, value); }
		}
		  
		[XmlAttribute("VcDiscountContext")]
		[Bindable(true)]
		public string VcDiscountContext 
		{
			get { return GetColumnValue<string>(Columns.VcDiscountContext); }
			set { SetColumnValue(Columns.VcDiscountContext, value); }
		}
		  
		[XmlAttribute("MMinMerch")]
		[Bindable(true)]
		public decimal MMinMerch 
		{
			get { return GetColumnValue<decimal>(Columns.MMinMerch); }
			set { SetColumnValue(Columns.MMinMerch, value); }
		}
		  
		[XmlAttribute("MMinTicket")]
		[Bindable(true)]
		public decimal MMinTicket 
		{
			get { return GetColumnValue<decimal>(Columns.MMinTicket); }
			set { SetColumnValue(Columns.MMinTicket, value); }
		}
		  
		[XmlAttribute("MMinTotal")]
		[Bindable(true)]
		public decimal MMinTotal 
		{
			get { return GetColumnValue<decimal>(Columns.MMinTotal); }
			set { SetColumnValue(Columns.MMinTotal, value); }
		}
		  
		[XmlAttribute("BannerUrl")]
		[Bindable(true)]
		public string BannerUrl 
		{
			get { return GetColumnValue<string>(Columns.BannerUrl); }
			set { SetColumnValue(Columns.BannerUrl, value); }
		}
		  
		[XmlAttribute("BannerClickUrl")]
		[Bindable(true)]
		public string BannerClickUrl 
		{
			get { return GetColumnValue<string>(Columns.BannerClickUrl); }
			set { SetColumnValue(Columns.BannerClickUrl, value); }
		}
		  
		[XmlAttribute("BDisplayAtParent")]
		[Bindable(true)]
		public bool BDisplayAtParent 
		{
			get { return GetColumnValue<bool>(Columns.BDisplayAtParent); }
			set { SetColumnValue(Columns.BDisplayAtParent, value); }
		}
		  
		[XmlAttribute("BBannerMerch")]
		[Bindable(true)]
		public bool BBannerMerch 
		{
			get { return GetColumnValue<bool>(Columns.BBannerMerch); }
			set { SetColumnValue(Columns.BBannerMerch, value); }
		}
		  
		[XmlAttribute("BBannerTicket")]
		[Bindable(true)]
		public bool BBannerTicket 
		{
			get { return GetColumnValue<bool>(Columns.BBannerTicket); }
			set { SetColumnValue(Columns.BBannerTicket, value); }
		}
		  
		[XmlAttribute("BBannerCartEdit")]
		[Bindable(true)]
		public bool BBannerCartEdit 
		{
			get { return GetColumnValue<bool>(Columns.BBannerCartEdit); }
			set { SetColumnValue(Columns.BBannerCartEdit, value); }
		}
		  
		[XmlAttribute("BBannerCheckout")]
		[Bindable(true)]
		public bool BBannerCheckout 
		{
			get { return GetColumnValue<bool>(Columns.BBannerCheckout); }
			set { SetColumnValue(Columns.BBannerCheckout, value); }
		}
		  
		[XmlAttribute("BBannerShipping")]
		[Bindable(true)]
		public bool BBannerShipping 
		{
			get { return GetColumnValue<bool>(Columns.BBannerShipping); }
			set { SetColumnValue(Columns.BBannerShipping, value); }
		}
		  
		[XmlAttribute("ShipOfferMethod")]
		[Bindable(true)]
		public string ShipOfferMethod 
		{
			get { return GetColumnValue<string>(Columns.ShipOfferMethod); }
			set { SetColumnValue(Columns.ShipOfferMethod, value); }
		}
		  
		[XmlAttribute("UnlockCode")]
		[Bindable(true)]
		public string UnlockCode 
		{
			get { return GetColumnValue<string>(Columns.UnlockCode); }
			set { SetColumnValue(Columns.UnlockCode, value); }
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
		  
		[XmlAttribute("IMaxPerOrder")]
		[Bindable(true)]
		public int IMaxPerOrder 
		{
			get { return GetColumnValue<int>(Columns.IMaxPerOrder); }
			set { SetColumnValue(Columns.IMaxPerOrder, value); }
		}
		  
		[XmlAttribute("MMaxValue")]
		[Bindable(true)]
		public decimal? MMaxValue 
		{
			get { return GetColumnValue<decimal?>(Columns.MMaxValue); }
			set { SetColumnValue(Columns.MMaxValue, value); }
		}
		  
		[XmlAttribute("MWeight")]
		[Bindable(true)]
		public decimal MWeight 
		{
			get { return GetColumnValue<decimal>(Columns.MWeight); }
			set { SetColumnValue(Columns.MWeight, value); }
		}
		  
		[XmlAttribute("BDeactivateOnNoInventory")]
		[Bindable(true)]
		public bool BDeactivateOnNoInventory 
		{
			get { return GetColumnValue<bool>(Columns.BDeactivateOnNoInventory); }
			set { SetColumnValue(Columns.BDeactivateOnNoInventory, value); }
		}
		  
		[XmlAttribute("IMaxUsesPerUser")]
		[Bindable(true)]
		public int IMaxUsesPerUser 
		{
			get { return GetColumnValue<int>(Columns.IMaxUsesPerUser); }
			set { SetColumnValue(Columns.IMaxUsesPerUser, value); }
		}
		  
		[XmlAttribute("VcTriggerListMerch")]
		[Bindable(true)]
		public string VcTriggerListMerch 
		{
			get { return GetColumnValue<string>(Columns.VcTriggerListMerch); }
			set { SetColumnValue(Columns.VcTriggerListMerch, value); }
		}
		  
		[XmlAttribute("BAllowMultSelections")]
		[Bindable(true)]
		public bool? BAllowMultSelections 
		{
			get { return GetColumnValue<bool?>(Columns.BAllowMultSelections); }
			set { SetColumnValue(Columns.BAllowMultSelections, value); }
		}
		  
		[XmlAttribute("BAllowPromoTotalInMinimum")]
		[Bindable(true)]
		public bool? BAllowPromoTotalInMinimum 
		{
			get { return GetColumnValue<bool?>(Columns.BAllowPromoTotalInMinimum); }
			set { SetColumnValue(Columns.BAllowPromoTotalInMinimum, value); }
		}
		  
		[XmlAttribute("JsonMeta")]
		[Bindable(true)]
		public string JsonMeta 
		{
			get { return GetColumnValue<string>(Columns.JsonMeta); }
			set { SetColumnValue(Columns.JsonMeta, value); }
		}
		
		#endregion
		
		
		#region PrimaryKey Methods		
		
        protected override void SetPrimaryKey(object oValue)
        {
            base.SetPrimaryKey(oValue);
            
            SetPKValues();
        }
        
		
		private Wcss.InvoiceItemCollection colInvoiceItemRecords;
		public Wcss.InvoiceItemCollection InvoiceItemRecords()
		{
			if(colInvoiceItemRecords == null)
			{
				colInvoiceItemRecords = new Wcss.InvoiceItemCollection().Where(InvoiceItem.Columns.TSalePromotionId, Id).Load();
				colInvoiceItemRecords.ListChanged += new ListChangedEventHandler(colInvoiceItemRecords_ListChanged);
			}
			return colInvoiceItemRecords;
		}
				
		void colInvoiceItemRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colInvoiceItemRecords[e.NewIndex].TSalePromotionId = Id;
				colInvoiceItemRecords.ListChanged += new ListChangedEventHandler(colInvoiceItemRecords_ListChanged);
            }
		}
		private Wcss.SalePromotionAwardCollection colSalePromotionAwardRecords;
		public Wcss.SalePromotionAwardCollection SalePromotionAwardRecords()
		{
			if(colSalePromotionAwardRecords == null)
			{
				colSalePromotionAwardRecords = new Wcss.SalePromotionAwardCollection().Where(SalePromotionAward.Columns.TSalePromotionId, Id).Load();
				colSalePromotionAwardRecords.ListChanged += new ListChangedEventHandler(colSalePromotionAwardRecords_ListChanged);
			}
			return colSalePromotionAwardRecords;
		}
				
		void colSalePromotionAwardRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colSalePromotionAwardRecords[e.NewIndex].TSalePromotionId = Id;
				colSalePromotionAwardRecords.ListChanged += new ListChangedEventHandler(colSalePromotionAwardRecords_ListChanged);
            }
		}
		private Wcss.UserCouponRedemptionCollection colUserCouponRedemptionRecords;
		public Wcss.UserCouponRedemptionCollection UserCouponRedemptionRecords()
		{
			if(colUserCouponRedemptionRecords == null)
			{
				colUserCouponRedemptionRecords = new Wcss.UserCouponRedemptionCollection().Where(UserCouponRedemption.Columns.TSalePromotionId, Id).Load();
				colUserCouponRedemptionRecords.ListChanged += new ListChangedEventHandler(colUserCouponRedemptionRecords_ListChanged);
			}
			return colUserCouponRedemptionRecords;
		}
				
		void colUserCouponRedemptionRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colUserCouponRedemptionRecords[e.NewIndex].TSalePromotionId = Id;
				colUserCouponRedemptionRecords.ListChanged += new ListChangedEventHandler(colUserCouponRedemptionRecords_ListChanged);
            }
		}
		#endregion
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a AspnetApplication ActiveRecord object related to this SalePromotion
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
		/// Returns a ShowDate ActiveRecord object related to this SalePromotion
		/// 
		/// </summary>
		private Wcss.ShowDate ShowDate
		{
			get { return Wcss.ShowDate.FetchByID(this.TRequiredParentShowDateId); }
			set { SetColumnValue("tRequiredParentShowDateId", value.Id); }
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
                
                SetColumnValue("tRequiredParentShowDateId", value.Id);
                _showdaterecord.CopyFrom(value);                
            }
		}
		
		
		/// <summary>
		/// Returns a ShowTicket ActiveRecord object related to this SalePromotion
		/// 
		/// </summary>
		private Wcss.ShowTicket ShowTicket
		{
			get { return Wcss.ShowTicket.FetchByID(this.TShowTicketId); }
			set { SetColumnValue("tShowTicketId", value.Id); }
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
                
                SetColumnValue("tShowTicketId", value.Id);
                _showticketrecord.CopyFrom(value);                
            }
		}
		
		
		/// <summary>
		/// Returns a ShowTicket ActiveRecord object related to this SalePromotion
		/// 
		/// </summary>
		private Wcss.ShowTicket ShowTicketToTRequiredParentShowTicketId
		{
			get { return Wcss.ShowTicket.FetchByID(this.TRequiredParentShowTicketId); }
			set { SetColumnValue("tRequiredParentShowTicketId", value.Id); }
		}
        //set up an alternate mechanism to avoid a database call
		private Wcss.ShowTicket _showtickettotrequiredparentshowticketidrecord = null;
		
		public Wcss.ShowTicket ShowTicketToTRequiredParentShowTicketIdRecord
		{
		    get
            {
                if (_showtickettotrequiredparentshowticketidrecord == null)
                {
                    _showtickettotrequiredparentshowticketidrecord = new Wcss.ShowTicket();
                    _showtickettotrequiredparentshowticketidrecord.CopyFrom(this.ShowTicketToTRequiredParentShowTicketId);
                }
                return _showtickettotrequiredparentshowticketidrecord;
            }
            set
            {
                if(value != null && _showtickettotrequiredparentshowticketidrecord == null)
			        _showtickettotrequiredparentshowticketidrecord = new Wcss.ShowTicket();
                
                SetColumnValue("tRequiredParentShowTicketId", value.Id);
                _showtickettotrequiredparentshowticketidrecord.CopyFrom(value);                
            }
		}
		
		
		#endregion
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(DateTime? varDtStamp,Guid varApplicationId,bool varBActive,int varIDisplayOrder,int varIBannerTimeoutMsecs,string varName,string varDisplayText,string varAdditionalText,int? varTShowTicketId,string varRequiredPromotionCode,int? varTRequiredParentShowTicketId,int? varTRequiredParentShowDateId,int varIRequiredParentQty,decimal varMPrice,decimal varMDiscountAmount,int varIDiscountPercent,string varVcDiscountContext,decimal varMMinMerch,decimal varMMinTicket,decimal varMMinTotal,string varBannerUrl,string varBannerClickUrl,bool varBDisplayAtParent,bool varBBannerMerch,bool varBBannerTicket,bool varBBannerCartEdit,bool varBBannerCheckout,bool varBBannerShipping,string varShipOfferMethod,string varUnlockCode,DateTime? varDtStartDate,DateTime? varDtEndDate,int varIMaxPerOrder,decimal? varMMaxValue,decimal varMWeight,bool varBDeactivateOnNoInventory,int varIMaxUsesPerUser,string varVcTriggerListMerch,bool? varBAllowMultSelections,bool? varBAllowPromoTotalInMinimum,string varJsonMeta)
		{
			SalePromotion item = new SalePromotion();
			
			item.DtStamp = varDtStamp;
			
			item.ApplicationId = varApplicationId;
			
			item.BActive = varBActive;
			
			item.IDisplayOrder = varIDisplayOrder;
			
			item.IBannerTimeoutMsecs = varIBannerTimeoutMsecs;
			
			item.Name = varName;
			
			item.DisplayText = varDisplayText;
			
			item.AdditionalText = varAdditionalText;
			
			item.TShowTicketId = varTShowTicketId;
			
			item.RequiredPromotionCode = varRequiredPromotionCode;
			
			item.TRequiredParentShowTicketId = varTRequiredParentShowTicketId;
			
			item.TRequiredParentShowDateId = varTRequiredParentShowDateId;
			
			item.IRequiredParentQty = varIRequiredParentQty;
			
			item.MPrice = varMPrice;
			
			item.MDiscountAmount = varMDiscountAmount;
			
			item.IDiscountPercent = varIDiscountPercent;
			
			item.VcDiscountContext = varVcDiscountContext;
			
			item.MMinMerch = varMMinMerch;
			
			item.MMinTicket = varMMinTicket;
			
			item.MMinTotal = varMMinTotal;
			
			item.BannerUrl = varBannerUrl;
			
			item.BannerClickUrl = varBannerClickUrl;
			
			item.BDisplayAtParent = varBDisplayAtParent;
			
			item.BBannerMerch = varBBannerMerch;
			
			item.BBannerTicket = varBBannerTicket;
			
			item.BBannerCartEdit = varBBannerCartEdit;
			
			item.BBannerCheckout = varBBannerCheckout;
			
			item.BBannerShipping = varBBannerShipping;
			
			item.ShipOfferMethod = varShipOfferMethod;
			
			item.UnlockCode = varUnlockCode;
			
			item.DtStartDate = varDtStartDate;
			
			item.DtEndDate = varDtEndDate;
			
			item.IMaxPerOrder = varIMaxPerOrder;
			
			item.MMaxValue = varMMaxValue;
			
			item.MWeight = varMWeight;
			
			item.BDeactivateOnNoInventory = varBDeactivateOnNoInventory;
			
			item.IMaxUsesPerUser = varIMaxUsesPerUser;
			
			item.VcTriggerListMerch = varVcTriggerListMerch;
			
			item.BAllowMultSelections = varBAllowMultSelections;
			
			item.BAllowPromoTotalInMinimum = varBAllowPromoTotalInMinimum;
			
			item.JsonMeta = varJsonMeta;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varId,DateTime? varDtStamp,Guid varApplicationId,bool varBActive,int varIDisplayOrder,int varIBannerTimeoutMsecs,string varName,string varDisplayText,string varAdditionalText,int? varTShowTicketId,string varRequiredPromotionCode,int? varTRequiredParentShowTicketId,int? varTRequiredParentShowDateId,int varIRequiredParentQty,decimal varMPrice,decimal varMDiscountAmount,int varIDiscountPercent,string varVcDiscountContext,decimal varMMinMerch,decimal varMMinTicket,decimal varMMinTotal,string varBannerUrl,string varBannerClickUrl,bool varBDisplayAtParent,bool varBBannerMerch,bool varBBannerTicket,bool varBBannerCartEdit,bool varBBannerCheckout,bool varBBannerShipping,string varShipOfferMethod,string varUnlockCode,DateTime? varDtStartDate,DateTime? varDtEndDate,int varIMaxPerOrder,decimal? varMMaxValue,decimal varMWeight,bool varBDeactivateOnNoInventory,int varIMaxUsesPerUser,string varVcTriggerListMerch,bool? varBAllowMultSelections,bool? varBAllowPromoTotalInMinimum,string varJsonMeta)
		{
			SalePromotion item = new SalePromotion();
			
				item.Id = varId;
			
				item.DtStamp = varDtStamp;
			
				item.ApplicationId = varApplicationId;
			
				item.BActive = varBActive;
			
				item.IDisplayOrder = varIDisplayOrder;
			
				item.IBannerTimeoutMsecs = varIBannerTimeoutMsecs;
			
				item.Name = varName;
			
				item.DisplayText = varDisplayText;
			
				item.AdditionalText = varAdditionalText;
			
				item.TShowTicketId = varTShowTicketId;
			
				item.RequiredPromotionCode = varRequiredPromotionCode;
			
				item.TRequiredParentShowTicketId = varTRequiredParentShowTicketId;
			
				item.TRequiredParentShowDateId = varTRequiredParentShowDateId;
			
				item.IRequiredParentQty = varIRequiredParentQty;
			
				item.MPrice = varMPrice;
			
				item.MDiscountAmount = varMDiscountAmount;
			
				item.IDiscountPercent = varIDiscountPercent;
			
				item.VcDiscountContext = varVcDiscountContext;
			
				item.MMinMerch = varMMinMerch;
			
				item.MMinTicket = varMMinTicket;
			
				item.MMinTotal = varMMinTotal;
			
				item.BannerUrl = varBannerUrl;
			
				item.BannerClickUrl = varBannerClickUrl;
			
				item.BDisplayAtParent = varBDisplayAtParent;
			
				item.BBannerMerch = varBBannerMerch;
			
				item.BBannerTicket = varBBannerTicket;
			
				item.BBannerCartEdit = varBBannerCartEdit;
			
				item.BBannerCheckout = varBBannerCheckout;
			
				item.BBannerShipping = varBBannerShipping;
			
				item.ShipOfferMethod = varShipOfferMethod;
			
				item.UnlockCode = varUnlockCode;
			
				item.DtStartDate = varDtStartDate;
			
				item.DtEndDate = varDtEndDate;
			
				item.IMaxPerOrder = varIMaxPerOrder;
			
				item.MMaxValue = varMMaxValue;
			
				item.MWeight = varMWeight;
			
				item.BDeactivateOnNoInventory = varBDeactivateOnNoInventory;
			
				item.IMaxUsesPerUser = varIMaxUsesPerUser;
			
				item.VcTriggerListMerch = varVcTriggerListMerch;
			
				item.BAllowMultSelections = varBAllowMultSelections;
			
				item.BAllowPromoTotalInMinimum = varBAllowPromoTotalInMinimum;
			
				item.JsonMeta = varJsonMeta;
			
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
        
        
        
        public static TableSchema.TableColumn ApplicationIdColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn BActiveColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn IDisplayOrderColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn IBannerTimeoutMsecsColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn NameColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn DisplayTextColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn AdditionalTextColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn TShowTicketIdColumn
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        public static TableSchema.TableColumn RequiredPromotionCodeColumn
        {
            get { return Schema.Columns[10]; }
        }
        
        
        
        public static TableSchema.TableColumn TRequiredParentShowTicketIdColumn
        {
            get { return Schema.Columns[11]; }
        }
        
        
        
        public static TableSchema.TableColumn TRequiredParentShowDateIdColumn
        {
            get { return Schema.Columns[12]; }
        }
        
        
        
        public static TableSchema.TableColumn IRequiredParentQtyColumn
        {
            get { return Schema.Columns[13]; }
        }
        
        
        
        public static TableSchema.TableColumn MPriceColumn
        {
            get { return Schema.Columns[14]; }
        }
        
        
        
        public static TableSchema.TableColumn MDiscountAmountColumn
        {
            get { return Schema.Columns[15]; }
        }
        
        
        
        public static TableSchema.TableColumn IDiscountPercentColumn
        {
            get { return Schema.Columns[16]; }
        }
        
        
        
        public static TableSchema.TableColumn VcDiscountContextColumn
        {
            get { return Schema.Columns[17]; }
        }
        
        
        
        public static TableSchema.TableColumn MMinMerchColumn
        {
            get { return Schema.Columns[18]; }
        }
        
        
        
        public static TableSchema.TableColumn MMinTicketColumn
        {
            get { return Schema.Columns[19]; }
        }
        
        
        
        public static TableSchema.TableColumn MMinTotalColumn
        {
            get { return Schema.Columns[20]; }
        }
        
        
        
        public static TableSchema.TableColumn BannerUrlColumn
        {
            get { return Schema.Columns[21]; }
        }
        
        
        
        public static TableSchema.TableColumn BannerClickUrlColumn
        {
            get { return Schema.Columns[22]; }
        }
        
        
        
        public static TableSchema.TableColumn BDisplayAtParentColumn
        {
            get { return Schema.Columns[23]; }
        }
        
        
        
        public static TableSchema.TableColumn BBannerMerchColumn
        {
            get { return Schema.Columns[24]; }
        }
        
        
        
        public static TableSchema.TableColumn BBannerTicketColumn
        {
            get { return Schema.Columns[25]; }
        }
        
        
        
        public static TableSchema.TableColumn BBannerCartEditColumn
        {
            get { return Schema.Columns[26]; }
        }
        
        
        
        public static TableSchema.TableColumn BBannerCheckoutColumn
        {
            get { return Schema.Columns[27]; }
        }
        
        
        
        public static TableSchema.TableColumn BBannerShippingColumn
        {
            get { return Schema.Columns[28]; }
        }
        
        
        
        public static TableSchema.TableColumn ShipOfferMethodColumn
        {
            get { return Schema.Columns[29]; }
        }
        
        
        
        public static TableSchema.TableColumn UnlockCodeColumn
        {
            get { return Schema.Columns[30]; }
        }
        
        
        
        public static TableSchema.TableColumn DtStartDateColumn
        {
            get { return Schema.Columns[31]; }
        }
        
        
        
        public static TableSchema.TableColumn DtEndDateColumn
        {
            get { return Schema.Columns[32]; }
        }
        
        
        
        public static TableSchema.TableColumn IMaxPerOrderColumn
        {
            get { return Schema.Columns[33]; }
        }
        
        
        
        public static TableSchema.TableColumn MMaxValueColumn
        {
            get { return Schema.Columns[34]; }
        }
        
        
        
        public static TableSchema.TableColumn MWeightColumn
        {
            get { return Schema.Columns[35]; }
        }
        
        
        
        public static TableSchema.TableColumn BDeactivateOnNoInventoryColumn
        {
            get { return Schema.Columns[36]; }
        }
        
        
        
        public static TableSchema.TableColumn IMaxUsesPerUserColumn
        {
            get { return Schema.Columns[37]; }
        }
        
        
        
        public static TableSchema.TableColumn VcTriggerListMerchColumn
        {
            get { return Schema.Columns[38]; }
        }
        
        
        
        public static TableSchema.TableColumn BAllowMultSelectionsColumn
        {
            get { return Schema.Columns[39]; }
        }
        
        
        
        public static TableSchema.TableColumn BAllowPromoTotalInMinimumColumn
        {
            get { return Schema.Columns[40]; }
        }
        
        
        
        public static TableSchema.TableColumn JsonMetaColumn
        {
            get { return Schema.Columns[41]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Id = @"Id";
			 public static string DtStamp = @"dtStamp";
			 public static string ApplicationId = @"ApplicationId";
			 public static string BActive = @"bActive";
			 public static string IDisplayOrder = @"iDisplayOrder";
			 public static string IBannerTimeoutMsecs = @"iBannerTimeoutMsecs";
			 public static string Name = @"Name";
			 public static string DisplayText = @"DisplayText";
			 public static string AdditionalText = @"AdditionalText";
			 public static string TShowTicketId = @"tShowTicketId";
			 public static string RequiredPromotionCode = @"RequiredPromotionCode";
			 public static string TRequiredParentShowTicketId = @"tRequiredParentShowTicketId";
			 public static string TRequiredParentShowDateId = @"tRequiredParentShowDateId";
			 public static string IRequiredParentQty = @"iRequiredParentQty";
			 public static string MPrice = @"mPrice";
			 public static string MDiscountAmount = @"mDiscountAmount";
			 public static string IDiscountPercent = @"iDiscountPercent";
			 public static string VcDiscountContext = @"vcDiscountContext";
			 public static string MMinMerch = @"mMinMerch";
			 public static string MMinTicket = @"mMinTicket";
			 public static string MMinTotal = @"mMinTotal";
			 public static string BannerUrl = @"BannerUrl";
			 public static string BannerClickUrl = @"BannerClickUrl";
			 public static string BDisplayAtParent = @"bDisplayAtParent";
			 public static string BBannerMerch = @"bBannerMerch";
			 public static string BBannerTicket = @"bBannerTicket";
			 public static string BBannerCartEdit = @"bBannerCartEdit";
			 public static string BBannerCheckout = @"bBannerCheckout";
			 public static string BBannerShipping = @"bBannerShipping";
			 public static string ShipOfferMethod = @"ShipOfferMethod";
			 public static string UnlockCode = @"UnlockCode";
			 public static string DtStartDate = @"dtStartDate";
			 public static string DtEndDate = @"dtEndDate";
			 public static string IMaxPerOrder = @"iMaxPerOrder";
			 public static string MMaxValue = @"mMaxValue";
			 public static string MWeight = @"mWeight";
			 public static string BDeactivateOnNoInventory = @"bDeactivateOnNoInventory";
			 public static string IMaxUsesPerUser = @"iMaxUsesPerUser";
			 public static string VcTriggerListMerch = @"vcTriggerList_Merch";
			 public static string BAllowMultSelections = @"bAllowMultSelections";
			 public static string BAllowPromoTotalInMinimum = @"bAllowPromoTotalInMinimum";
			 public static string JsonMeta = @"jsonMeta";
						
		}
		#endregion
		
		#region Update PK Collections
		
        public void SetPKValues()
        {
                if (colInvoiceItemRecords != null)
                {
                    foreach (Wcss.InvoiceItem item in colInvoiceItemRecords)
                    {
                        if (item.TSalePromotionId != Id)
                        {
                            item.TSalePromotionId = Id;
                        }
                    }
               }
		
                if (colSalePromotionAwardRecords != null)
                {
                    foreach (Wcss.SalePromotionAward item in colSalePromotionAwardRecords)
                    {
                        if (item.TSalePromotionId != Id)
                        {
                            item.TSalePromotionId = Id;
                        }
                    }
               }
		
                if (colUserCouponRedemptionRecords != null)
                {
                    foreach (Wcss.UserCouponRedemption item in colUserCouponRedemptionRecords)
                    {
                        if (item.TSalePromotionId != Id)
                        {
                            item.TSalePromotionId = Id;
                        }
                    }
               }
		}
        #endregion
    
        #region Deep Save
		
        public void DeepSave()
        {
            Save();
            
                if (colInvoiceItemRecords != null)
                {
                    colInvoiceItemRecords.SaveAll();
               }
		
                if (colSalePromotionAwardRecords != null)
                {
                    colSalePromotionAwardRecords.SaveAll();
               }
		
                if (colUserCouponRedemptionRecords != null)
                {
                    colUserCouponRedemptionRecords.SaveAll();
               }
		}
        #endregion
	}
}
