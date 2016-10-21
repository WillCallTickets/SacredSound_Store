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
	/// Strongly-typed collection for the ChargeStatement class.
	/// </summary>
    [Serializable]
	public partial class ChargeStatementCollection : ActiveList<ChargeStatement, ChargeStatementCollection>
	{	   
		public ChargeStatementCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>ChargeStatementCollection</returns>
		public ChargeStatementCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                ChargeStatement o = this[i];
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
	/// This is an ActiveRecord class which wraps the Charge_Statement table.
	/// </summary>
	[Serializable]
	public partial class ChargeStatement : ActiveRecord<ChargeStatement>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public ChargeStatement()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public ChargeStatement(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public ChargeStatement(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public ChargeStatement(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("Charge_Statement", TableType.Table, DataService.GetInstance("WillCall"));
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
				
				TableSchema.TableColumn colvarChargeStatementId = new TableSchema.TableColumn(schema);
				colvarChargeStatementId.ColumnName = "ChargeStatementId";
				colvarChargeStatementId.DataType = DbType.Guid;
				colvarChargeStatementId.MaxLength = 0;
				colvarChargeStatementId.AutoIncrement = false;
				colvarChargeStatementId.IsNullable = false;
				colvarChargeStatementId.IsPrimaryKey = false;
				colvarChargeStatementId.IsForeignKey = false;
				colvarChargeStatementId.IsReadOnly = false;
				
						colvarChargeStatementId.DefaultSetting = @"(newid())";
				colvarChargeStatementId.ForeignKeyTableName = "";
				schema.Columns.Add(colvarChargeStatementId);
				
				TableSchema.TableColumn colvarIMonth = new TableSchema.TableColumn(schema);
				colvarIMonth.ColumnName = "iMonth";
				colvarIMonth.DataType = DbType.Int32;
				colvarIMonth.MaxLength = 0;
				colvarIMonth.AutoIncrement = false;
				colvarIMonth.IsNullable = false;
				colvarIMonth.IsPrimaryKey = false;
				colvarIMonth.IsForeignKey = false;
				colvarIMonth.IsReadOnly = false;
				colvarIMonth.DefaultSetting = @"";
				colvarIMonth.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIMonth);
				
				TableSchema.TableColumn colvarIYear = new TableSchema.TableColumn(schema);
				colvarIYear.ColumnName = "iYear";
				colvarIYear.DataType = DbType.Int32;
				colvarIYear.MaxLength = 0;
				colvarIYear.AutoIncrement = false;
				colvarIYear.IsNullable = false;
				colvarIYear.IsPrimaryKey = false;
				colvarIYear.IsForeignKey = false;
				colvarIYear.IsReadOnly = false;
				colvarIYear.DefaultSetting = @"";
				colvarIYear.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIYear);
				
				TableSchema.TableColumn colvarMonthYear = new TableSchema.TableColumn(schema);
				colvarMonthYear.ColumnName = "MonthYear";
				colvarMonthYear.DataType = DbType.AnsiString;
				colvarMonthYear.MaxLength = 7;
				colvarMonthYear.AutoIncrement = false;
				colvarMonthYear.IsNullable = true;
				colvarMonthYear.IsPrimaryKey = false;
				colvarMonthYear.IsForeignKey = false;
				colvarMonthYear.IsReadOnly = true;
				colvarMonthYear.DefaultSetting = @"";
				colvarMonthYear.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMonthYear);
				
				TableSchema.TableColumn colvarSalesQty = new TableSchema.TableColumn(schema);
				colvarSalesQty.ColumnName = "SalesQty";
				colvarSalesQty.DataType = DbType.Int32;
				colvarSalesQty.MaxLength = 0;
				colvarSalesQty.AutoIncrement = false;
				colvarSalesQty.IsNullable = false;
				colvarSalesQty.IsPrimaryKey = false;
				colvarSalesQty.IsForeignKey = false;
				colvarSalesQty.IsReadOnly = false;
				
						colvarSalesQty.DefaultSetting = @"((0))";
				colvarSalesQty.ForeignKeyTableName = "";
				schema.Columns.Add(colvarSalesQty);
				
				TableSchema.TableColumn colvarSalesQtyPct = new TableSchema.TableColumn(schema);
				colvarSalesQtyPct.ColumnName = "SalesQtyPct";
				colvarSalesQtyPct.DataType = DbType.Currency;
				colvarSalesQtyPct.MaxLength = 0;
				colvarSalesQtyPct.AutoIncrement = false;
				colvarSalesQtyPct.IsNullable = false;
				colvarSalesQtyPct.IsPrimaryKey = false;
				colvarSalesQtyPct.IsForeignKey = false;
				colvarSalesQtyPct.IsReadOnly = false;
				
						colvarSalesQtyPct.DefaultSetting = @"((0))";
				colvarSalesQtyPct.ForeignKeyTableName = "";
				schema.Columns.Add(colvarSalesQtyPct);
				
				TableSchema.TableColumn colvarSalesQtyPortion = new TableSchema.TableColumn(schema);
				colvarSalesQtyPortion.ColumnName = "SalesQtyPortion";
				colvarSalesQtyPortion.DataType = DbType.Currency;
				colvarSalesQtyPortion.MaxLength = 0;
				colvarSalesQtyPortion.AutoIncrement = false;
				colvarSalesQtyPortion.IsNullable = true;
				colvarSalesQtyPortion.IsPrimaryKey = false;
				colvarSalesQtyPortion.IsForeignKey = false;
				colvarSalesQtyPortion.IsReadOnly = true;
				colvarSalesQtyPortion.DefaultSetting = @"";
				colvarSalesQtyPortion.ForeignKeyTableName = "";
				schema.Columns.Add(colvarSalesQtyPortion);
				
				TableSchema.TableColumn colvarRefundQty = new TableSchema.TableColumn(schema);
				colvarRefundQty.ColumnName = "RefundQty";
				colvarRefundQty.DataType = DbType.Int32;
				colvarRefundQty.MaxLength = 0;
				colvarRefundQty.AutoIncrement = false;
				colvarRefundQty.IsNullable = false;
				colvarRefundQty.IsPrimaryKey = false;
				colvarRefundQty.IsForeignKey = false;
				colvarRefundQty.IsReadOnly = false;
				
						colvarRefundQty.DefaultSetting = @"((0))";
				colvarRefundQty.ForeignKeyTableName = "";
				schema.Columns.Add(colvarRefundQty);
				
				TableSchema.TableColumn colvarRefundQtyPct = new TableSchema.TableColumn(schema);
				colvarRefundQtyPct.ColumnName = "RefundQtyPct";
				colvarRefundQtyPct.DataType = DbType.Currency;
				colvarRefundQtyPct.MaxLength = 0;
				colvarRefundQtyPct.AutoIncrement = false;
				colvarRefundQtyPct.IsNullable = false;
				colvarRefundQtyPct.IsPrimaryKey = false;
				colvarRefundQtyPct.IsForeignKey = false;
				colvarRefundQtyPct.IsReadOnly = false;
				
						colvarRefundQtyPct.DefaultSetting = @"((0))";
				colvarRefundQtyPct.ForeignKeyTableName = "";
				schema.Columns.Add(colvarRefundQtyPct);
				
				TableSchema.TableColumn colvarRefundQtyPortion = new TableSchema.TableColumn(schema);
				colvarRefundQtyPortion.ColumnName = "RefundQtyPortion";
				colvarRefundQtyPortion.DataType = DbType.Currency;
				colvarRefundQtyPortion.MaxLength = 0;
				colvarRefundQtyPortion.AutoIncrement = false;
				colvarRefundQtyPortion.IsNullable = true;
				colvarRefundQtyPortion.IsPrimaryKey = false;
				colvarRefundQtyPortion.IsForeignKey = false;
				colvarRefundQtyPortion.IsReadOnly = true;
				colvarRefundQtyPortion.DefaultSetting = @"";
				colvarRefundQtyPortion.ForeignKeyTableName = "";
				schema.Columns.Add(colvarRefundQtyPortion);
				
				TableSchema.TableColumn colvarGross = new TableSchema.TableColumn(schema);
				colvarGross.ColumnName = "Gross";
				colvarGross.DataType = DbType.Currency;
				colvarGross.MaxLength = 0;
				colvarGross.AutoIncrement = false;
				colvarGross.IsNullable = false;
				colvarGross.IsPrimaryKey = false;
				colvarGross.IsForeignKey = false;
				colvarGross.IsReadOnly = false;
				
						colvarGross.DefaultSetting = @"((0))";
				colvarGross.ForeignKeyTableName = "";
				schema.Columns.Add(colvarGross);
				
				TableSchema.TableColumn colvarGrossPct = new TableSchema.TableColumn(schema);
				colvarGrossPct.ColumnName = "GrossPct";
				colvarGrossPct.DataType = DbType.Currency;
				colvarGrossPct.MaxLength = 0;
				colvarGrossPct.AutoIncrement = false;
				colvarGrossPct.IsNullable = false;
				colvarGrossPct.IsPrimaryKey = false;
				colvarGrossPct.IsForeignKey = false;
				colvarGrossPct.IsReadOnly = false;
				
						colvarGrossPct.DefaultSetting = @"((0))";
				colvarGrossPct.ForeignKeyTableName = "";
				schema.Columns.Add(colvarGrossPct);
				
				TableSchema.TableColumn colvarGrossThreshhold = new TableSchema.TableColumn(schema);
				colvarGrossThreshhold.ColumnName = "GrossThreshhold";
				colvarGrossThreshhold.DataType = DbType.Currency;
				colvarGrossThreshhold.MaxLength = 0;
				colvarGrossThreshhold.AutoIncrement = false;
				colvarGrossThreshhold.IsNullable = false;
				colvarGrossThreshhold.IsPrimaryKey = false;
				colvarGrossThreshhold.IsForeignKey = false;
				colvarGrossThreshhold.IsReadOnly = false;
				
						colvarGrossThreshhold.DefaultSetting = @"((0))";
				colvarGrossThreshhold.ForeignKeyTableName = "";
				schema.Columns.Add(colvarGrossThreshhold);
				
				TableSchema.TableColumn colvarGrossPortion = new TableSchema.TableColumn(schema);
				colvarGrossPortion.ColumnName = "GrossPortion";
				colvarGrossPortion.DataType = DbType.Currency;
				colvarGrossPortion.MaxLength = 0;
				colvarGrossPortion.AutoIncrement = false;
				colvarGrossPortion.IsNullable = true;
				colvarGrossPortion.IsPrimaryKey = false;
				colvarGrossPortion.IsForeignKey = false;
				colvarGrossPortion.IsReadOnly = true;
				colvarGrossPortion.DefaultSetting = @"";
				colvarGrossPortion.ForeignKeyTableName = "";
				schema.Columns.Add(colvarGrossPortion);
				
				TableSchema.TableColumn colvarTicketInvoiceQty = new TableSchema.TableColumn(schema);
				colvarTicketInvoiceQty.ColumnName = "TicketInvoiceQty";
				colvarTicketInvoiceQty.DataType = DbType.Int32;
				colvarTicketInvoiceQty.MaxLength = 0;
				colvarTicketInvoiceQty.AutoIncrement = false;
				colvarTicketInvoiceQty.IsNullable = false;
				colvarTicketInvoiceQty.IsPrimaryKey = false;
				colvarTicketInvoiceQty.IsForeignKey = false;
				colvarTicketInvoiceQty.IsReadOnly = false;
				
						colvarTicketInvoiceQty.DefaultSetting = @"((0))";
				colvarTicketInvoiceQty.ForeignKeyTableName = "";
				schema.Columns.Add(colvarTicketInvoiceQty);
				
				TableSchema.TableColumn colvarTicketInvoicePct = new TableSchema.TableColumn(schema);
				colvarTicketInvoicePct.ColumnName = "TicketInvoicePct";
				colvarTicketInvoicePct.DataType = DbType.Currency;
				colvarTicketInvoicePct.MaxLength = 0;
				colvarTicketInvoicePct.AutoIncrement = false;
				colvarTicketInvoicePct.IsNullable = false;
				colvarTicketInvoicePct.IsPrimaryKey = false;
				colvarTicketInvoicePct.IsForeignKey = false;
				colvarTicketInvoicePct.IsReadOnly = false;
				
						colvarTicketInvoicePct.DefaultSetting = @"((0))";
				colvarTicketInvoicePct.ForeignKeyTableName = "";
				schema.Columns.Add(colvarTicketInvoicePct);
				
				TableSchema.TableColumn colvarTicketUnitQty = new TableSchema.TableColumn(schema);
				colvarTicketUnitQty.ColumnName = "TicketUnitQty";
				colvarTicketUnitQty.DataType = DbType.Int32;
				colvarTicketUnitQty.MaxLength = 0;
				colvarTicketUnitQty.AutoIncrement = false;
				colvarTicketUnitQty.IsNullable = false;
				colvarTicketUnitQty.IsPrimaryKey = false;
				colvarTicketUnitQty.IsForeignKey = false;
				colvarTicketUnitQty.IsReadOnly = false;
				
						colvarTicketUnitQty.DefaultSetting = @"((0))";
				colvarTicketUnitQty.ForeignKeyTableName = "";
				schema.Columns.Add(colvarTicketUnitQty);
				
				TableSchema.TableColumn colvarTicketUnitPct = new TableSchema.TableColumn(schema);
				colvarTicketUnitPct.ColumnName = "TicketUnitPct";
				colvarTicketUnitPct.DataType = DbType.Currency;
				colvarTicketUnitPct.MaxLength = 0;
				colvarTicketUnitPct.AutoIncrement = false;
				colvarTicketUnitPct.IsNullable = false;
				colvarTicketUnitPct.IsPrimaryKey = false;
				colvarTicketUnitPct.IsForeignKey = false;
				colvarTicketUnitPct.IsReadOnly = false;
				
						colvarTicketUnitPct.DefaultSetting = @"((0))";
				colvarTicketUnitPct.ForeignKeyTableName = "";
				schema.Columns.Add(colvarTicketUnitPct);
				
				TableSchema.TableColumn colvarTicketSales = new TableSchema.TableColumn(schema);
				colvarTicketSales.ColumnName = "TicketSales";
				colvarTicketSales.DataType = DbType.Currency;
				colvarTicketSales.MaxLength = 0;
				colvarTicketSales.AutoIncrement = false;
				colvarTicketSales.IsNullable = false;
				colvarTicketSales.IsPrimaryKey = false;
				colvarTicketSales.IsForeignKey = false;
				colvarTicketSales.IsReadOnly = false;
				
						colvarTicketSales.DefaultSetting = @"((0))";
				colvarTicketSales.ForeignKeyTableName = "";
				schema.Columns.Add(colvarTicketSales);
				
				TableSchema.TableColumn colvarTicketSalesPct = new TableSchema.TableColumn(schema);
				colvarTicketSalesPct.ColumnName = "TicketSalesPct";
				colvarTicketSalesPct.DataType = DbType.Currency;
				colvarTicketSalesPct.MaxLength = 0;
				colvarTicketSalesPct.AutoIncrement = false;
				colvarTicketSalesPct.IsNullable = false;
				colvarTicketSalesPct.IsPrimaryKey = false;
				colvarTicketSalesPct.IsForeignKey = false;
				colvarTicketSalesPct.IsReadOnly = false;
				
						colvarTicketSalesPct.DefaultSetting = @"((0))";
				colvarTicketSalesPct.ForeignKeyTableName = "";
				schema.Columns.Add(colvarTicketSalesPct);
				
				TableSchema.TableColumn colvarTicketPortion = new TableSchema.TableColumn(schema);
				colvarTicketPortion.ColumnName = "TicketPortion";
				colvarTicketPortion.DataType = DbType.Currency;
				colvarTicketPortion.MaxLength = 0;
				colvarTicketPortion.AutoIncrement = false;
				colvarTicketPortion.IsNullable = true;
				colvarTicketPortion.IsPrimaryKey = false;
				colvarTicketPortion.IsForeignKey = false;
				colvarTicketPortion.IsReadOnly = true;
				colvarTicketPortion.DefaultSetting = @"";
				colvarTicketPortion.ForeignKeyTableName = "";
				schema.Columns.Add(colvarTicketPortion);
				
				TableSchema.TableColumn colvarMerchInvoiceQty = new TableSchema.TableColumn(schema);
				colvarMerchInvoiceQty.ColumnName = "MerchInvoiceQty";
				colvarMerchInvoiceQty.DataType = DbType.Int32;
				colvarMerchInvoiceQty.MaxLength = 0;
				colvarMerchInvoiceQty.AutoIncrement = false;
				colvarMerchInvoiceQty.IsNullable = false;
				colvarMerchInvoiceQty.IsPrimaryKey = false;
				colvarMerchInvoiceQty.IsForeignKey = false;
				colvarMerchInvoiceQty.IsReadOnly = false;
				
						colvarMerchInvoiceQty.DefaultSetting = @"((0))";
				colvarMerchInvoiceQty.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMerchInvoiceQty);
				
				TableSchema.TableColumn colvarMerchInvoicePct = new TableSchema.TableColumn(schema);
				colvarMerchInvoicePct.ColumnName = "MerchInvoicePct";
				colvarMerchInvoicePct.DataType = DbType.Currency;
				colvarMerchInvoicePct.MaxLength = 0;
				colvarMerchInvoicePct.AutoIncrement = false;
				colvarMerchInvoicePct.IsNullable = false;
				colvarMerchInvoicePct.IsPrimaryKey = false;
				colvarMerchInvoicePct.IsForeignKey = false;
				colvarMerchInvoicePct.IsReadOnly = false;
				
						colvarMerchInvoicePct.DefaultSetting = @"((0))";
				colvarMerchInvoicePct.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMerchInvoicePct);
				
				TableSchema.TableColumn colvarMerchUnitQty = new TableSchema.TableColumn(schema);
				colvarMerchUnitQty.ColumnName = "MerchUnitQty";
				colvarMerchUnitQty.DataType = DbType.Int32;
				colvarMerchUnitQty.MaxLength = 0;
				colvarMerchUnitQty.AutoIncrement = false;
				colvarMerchUnitQty.IsNullable = false;
				colvarMerchUnitQty.IsPrimaryKey = false;
				colvarMerchUnitQty.IsForeignKey = false;
				colvarMerchUnitQty.IsReadOnly = false;
				
						colvarMerchUnitQty.DefaultSetting = @"((0))";
				colvarMerchUnitQty.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMerchUnitQty);
				
				TableSchema.TableColumn colvarMerchUnitPct = new TableSchema.TableColumn(schema);
				colvarMerchUnitPct.ColumnName = "MerchUnitPct";
				colvarMerchUnitPct.DataType = DbType.Currency;
				colvarMerchUnitPct.MaxLength = 0;
				colvarMerchUnitPct.AutoIncrement = false;
				colvarMerchUnitPct.IsNullable = false;
				colvarMerchUnitPct.IsPrimaryKey = false;
				colvarMerchUnitPct.IsForeignKey = false;
				colvarMerchUnitPct.IsReadOnly = false;
				
						colvarMerchUnitPct.DefaultSetting = @"((0))";
				colvarMerchUnitPct.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMerchUnitPct);
				
				TableSchema.TableColumn colvarMerchSales = new TableSchema.TableColumn(schema);
				colvarMerchSales.ColumnName = "MerchSales";
				colvarMerchSales.DataType = DbType.Currency;
				colvarMerchSales.MaxLength = 0;
				colvarMerchSales.AutoIncrement = false;
				colvarMerchSales.IsNullable = false;
				colvarMerchSales.IsPrimaryKey = false;
				colvarMerchSales.IsForeignKey = false;
				colvarMerchSales.IsReadOnly = false;
				
						colvarMerchSales.DefaultSetting = @"((0))";
				colvarMerchSales.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMerchSales);
				
				TableSchema.TableColumn colvarMerchSalesPct = new TableSchema.TableColumn(schema);
				colvarMerchSalesPct.ColumnName = "MerchSalesPct";
				colvarMerchSalesPct.DataType = DbType.Currency;
				colvarMerchSalesPct.MaxLength = 0;
				colvarMerchSalesPct.AutoIncrement = false;
				colvarMerchSalesPct.IsNullable = false;
				colvarMerchSalesPct.IsPrimaryKey = false;
				colvarMerchSalesPct.IsForeignKey = false;
				colvarMerchSalesPct.IsReadOnly = false;
				
						colvarMerchSalesPct.DefaultSetting = @"((0))";
				colvarMerchSalesPct.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMerchSalesPct);
				
				TableSchema.TableColumn colvarMerchPortion = new TableSchema.TableColumn(schema);
				colvarMerchPortion.ColumnName = "MerchPortion";
				colvarMerchPortion.DataType = DbType.Currency;
				colvarMerchPortion.MaxLength = 0;
				colvarMerchPortion.AutoIncrement = false;
				colvarMerchPortion.IsNullable = true;
				colvarMerchPortion.IsPrimaryKey = false;
				colvarMerchPortion.IsForeignKey = false;
				colvarMerchPortion.IsReadOnly = true;
				colvarMerchPortion.DefaultSetting = @"";
				colvarMerchPortion.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMerchPortion);
				
				TableSchema.TableColumn colvarShipUnitQty = new TableSchema.TableColumn(schema);
				colvarShipUnitQty.ColumnName = "ShipUnitQty";
				colvarShipUnitQty.DataType = DbType.Int32;
				colvarShipUnitQty.MaxLength = 0;
				colvarShipUnitQty.AutoIncrement = false;
				colvarShipUnitQty.IsNullable = false;
				colvarShipUnitQty.IsPrimaryKey = false;
				colvarShipUnitQty.IsForeignKey = false;
				colvarShipUnitQty.IsReadOnly = false;
				
						colvarShipUnitQty.DefaultSetting = @"((0))";
				colvarShipUnitQty.ForeignKeyTableName = "";
				schema.Columns.Add(colvarShipUnitQty);
				
				TableSchema.TableColumn colvarShipUnitPct = new TableSchema.TableColumn(schema);
				colvarShipUnitPct.ColumnName = "ShipUnitPct";
				colvarShipUnitPct.DataType = DbType.Currency;
				colvarShipUnitPct.MaxLength = 0;
				colvarShipUnitPct.AutoIncrement = false;
				colvarShipUnitPct.IsNullable = false;
				colvarShipUnitPct.IsPrimaryKey = false;
				colvarShipUnitPct.IsForeignKey = false;
				colvarShipUnitPct.IsReadOnly = false;
				
						colvarShipUnitPct.DefaultSetting = @"((0))";
				colvarShipUnitPct.ForeignKeyTableName = "";
				schema.Columns.Add(colvarShipUnitPct);
				
				TableSchema.TableColumn colvarShipSales = new TableSchema.TableColumn(schema);
				colvarShipSales.ColumnName = "ShipSales";
				colvarShipSales.DataType = DbType.Currency;
				colvarShipSales.MaxLength = 0;
				colvarShipSales.AutoIncrement = false;
				colvarShipSales.IsNullable = false;
				colvarShipSales.IsPrimaryKey = false;
				colvarShipSales.IsForeignKey = false;
				colvarShipSales.IsReadOnly = false;
				
						colvarShipSales.DefaultSetting = @"((0))";
				colvarShipSales.ForeignKeyTableName = "";
				schema.Columns.Add(colvarShipSales);
				
				TableSchema.TableColumn colvarShipSalesPct = new TableSchema.TableColumn(schema);
				colvarShipSalesPct.ColumnName = "ShipSalesPct";
				colvarShipSalesPct.DataType = DbType.Currency;
				colvarShipSalesPct.MaxLength = 0;
				colvarShipSalesPct.AutoIncrement = false;
				colvarShipSalesPct.IsNullable = false;
				colvarShipSalesPct.IsPrimaryKey = false;
				colvarShipSalesPct.IsForeignKey = false;
				colvarShipSalesPct.IsReadOnly = false;
				
						colvarShipSalesPct.DefaultSetting = @"((0))";
				colvarShipSalesPct.ForeignKeyTableName = "";
				schema.Columns.Add(colvarShipSalesPct);
				
				TableSchema.TableColumn colvarShipPortion = new TableSchema.TableColumn(schema);
				colvarShipPortion.ColumnName = "ShipPortion";
				colvarShipPortion.DataType = DbType.Currency;
				colvarShipPortion.MaxLength = 0;
				colvarShipPortion.AutoIncrement = false;
				colvarShipPortion.IsNullable = true;
				colvarShipPortion.IsPrimaryKey = false;
				colvarShipPortion.IsForeignKey = false;
				colvarShipPortion.IsReadOnly = true;
				colvarShipPortion.DefaultSetting = @"";
				colvarShipPortion.ForeignKeyTableName = "";
				schema.Columns.Add(colvarShipPortion);
				
				TableSchema.TableColumn colvarSubscriptionsSent = new TableSchema.TableColumn(schema);
				colvarSubscriptionsSent.ColumnName = "SubscriptionsSent";
				colvarSubscriptionsSent.DataType = DbType.Int32;
				colvarSubscriptionsSent.MaxLength = 0;
				colvarSubscriptionsSent.AutoIncrement = false;
				colvarSubscriptionsSent.IsNullable = false;
				colvarSubscriptionsSent.IsPrimaryKey = false;
				colvarSubscriptionsSent.IsForeignKey = false;
				colvarSubscriptionsSent.IsReadOnly = false;
				
						colvarSubscriptionsSent.DefaultSetting = @"((0))";
				colvarSubscriptionsSent.ForeignKeyTableName = "";
				schema.Columns.Add(colvarSubscriptionsSent);
				
				TableSchema.TableColumn colvarPerSubscription = new TableSchema.TableColumn(schema);
				colvarPerSubscription.ColumnName = "PerSubscription";
				colvarPerSubscription.DataType = DbType.Currency;
				colvarPerSubscription.MaxLength = 0;
				colvarPerSubscription.AutoIncrement = false;
				colvarPerSubscription.IsNullable = false;
				colvarPerSubscription.IsPrimaryKey = false;
				colvarPerSubscription.IsForeignKey = false;
				colvarPerSubscription.IsReadOnly = false;
				
						colvarPerSubscription.DefaultSetting = @"((0))";
				colvarPerSubscription.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPerSubscription);
				
				TableSchema.TableColumn colvarMailSent = new TableSchema.TableColumn(schema);
				colvarMailSent.ColumnName = "MailSent";
				colvarMailSent.DataType = DbType.Int32;
				colvarMailSent.MaxLength = 0;
				colvarMailSent.AutoIncrement = false;
				colvarMailSent.IsNullable = false;
				colvarMailSent.IsPrimaryKey = false;
				colvarMailSent.IsForeignKey = false;
				colvarMailSent.IsReadOnly = false;
				
						colvarMailSent.DefaultSetting = @"((0))";
				colvarMailSent.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMailSent);
				
				TableSchema.TableColumn colvarPerMailSent = new TableSchema.TableColumn(schema);
				colvarPerMailSent.ColumnName = "PerMailSent";
				colvarPerMailSent.DataType = DbType.Decimal;
				colvarPerMailSent.MaxLength = 0;
				colvarPerMailSent.AutoIncrement = false;
				colvarPerMailSent.IsNullable = false;
				colvarPerMailSent.IsPrimaryKey = false;
				colvarPerMailSent.IsForeignKey = false;
				colvarPerMailSent.IsReadOnly = false;
				
						colvarPerMailSent.DefaultSetting = @"((0))";
				colvarPerMailSent.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPerMailSent);
				
				TableSchema.TableColumn colvarMailerPortion = new TableSchema.TableColumn(schema);
				colvarMailerPortion.ColumnName = "MailerPortion";
				colvarMailerPortion.DataType = DbType.Decimal;
				colvarMailerPortion.MaxLength = 0;
				colvarMailerPortion.AutoIncrement = false;
				colvarMailerPortion.IsNullable = true;
				colvarMailerPortion.IsPrimaryKey = false;
				colvarMailerPortion.IsForeignKey = false;
				colvarMailerPortion.IsReadOnly = true;
				colvarMailerPortion.DefaultSetting = @"";
				colvarMailerPortion.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMailerPortion);
				
				TableSchema.TableColumn colvarHourlyPortion = new TableSchema.TableColumn(schema);
				colvarHourlyPortion.ColumnName = "HourlyPortion";
				colvarHourlyPortion.DataType = DbType.Currency;
				colvarHourlyPortion.MaxLength = 0;
				colvarHourlyPortion.AutoIncrement = false;
				colvarHourlyPortion.IsNullable = false;
				colvarHourlyPortion.IsPrimaryKey = false;
				colvarHourlyPortion.IsForeignKey = false;
				colvarHourlyPortion.IsReadOnly = false;
				
						colvarHourlyPortion.DefaultSetting = @"((0))";
				colvarHourlyPortion.ForeignKeyTableName = "";
				schema.Columns.Add(colvarHourlyPortion);
				
				TableSchema.TableColumn colvarDiscount = new TableSchema.TableColumn(schema);
				colvarDiscount.ColumnName = "Discount";
				colvarDiscount.DataType = DbType.Currency;
				colvarDiscount.MaxLength = 0;
				colvarDiscount.AutoIncrement = false;
				colvarDiscount.IsNullable = false;
				colvarDiscount.IsPrimaryKey = false;
				colvarDiscount.IsForeignKey = false;
				colvarDiscount.IsReadOnly = false;
				
						colvarDiscount.DefaultSetting = @"((0))";
				colvarDiscount.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDiscount);
				
				TableSchema.TableColumn colvarLineTotal = new TableSchema.TableColumn(schema);
				colvarLineTotal.ColumnName = "LineTotal";
				colvarLineTotal.DataType = DbType.Decimal;
				colvarLineTotal.MaxLength = 0;
				colvarLineTotal.AutoIncrement = false;
				colvarLineTotal.IsNullable = true;
				colvarLineTotal.IsPrimaryKey = false;
				colvarLineTotal.IsForeignKey = false;
				colvarLineTotal.IsReadOnly = true;
				colvarLineTotal.DefaultSetting = @"";
				colvarLineTotal.ForeignKeyTableName = "";
				schema.Columns.Add(colvarLineTotal);
				
				TableSchema.TableColumn colvarAmountPaid = new TableSchema.TableColumn(schema);
				colvarAmountPaid.ColumnName = "AmountPaid";
				colvarAmountPaid.DataType = DbType.Currency;
				colvarAmountPaid.MaxLength = 0;
				colvarAmountPaid.AutoIncrement = false;
				colvarAmountPaid.IsNullable = false;
				colvarAmountPaid.IsPrimaryKey = false;
				colvarAmountPaid.IsForeignKey = false;
				colvarAmountPaid.IsReadOnly = false;
				
						colvarAmountPaid.DefaultSetting = @"((0))";
				colvarAmountPaid.ForeignKeyTableName = "";
				schema.Columns.Add(colvarAmountPaid);
				
				TableSchema.TableColumn colvarDtPaid = new TableSchema.TableColumn(schema);
				colvarDtPaid.ColumnName = "dtPaid";
				colvarDtPaid.DataType = DbType.DateTime;
				colvarDtPaid.MaxLength = 0;
				colvarDtPaid.AutoIncrement = false;
				colvarDtPaid.IsNullable = true;
				colvarDtPaid.IsPrimaryKey = false;
				colvarDtPaid.IsForeignKey = false;
				colvarDtPaid.IsReadOnly = false;
				colvarDtPaid.DefaultSetting = @"";
				colvarDtPaid.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDtPaid);
				
				TableSchema.TableColumn colvarCheckNumber = new TableSchema.TableColumn(schema);
				colvarCheckNumber.ColumnName = "CheckNumber";
				colvarCheckNumber.DataType = DbType.AnsiString;
				colvarCheckNumber.MaxLength = 50;
				colvarCheckNumber.AutoIncrement = false;
				colvarCheckNumber.IsNullable = true;
				colvarCheckNumber.IsPrimaryKey = false;
				colvarCheckNumber.IsForeignKey = false;
				colvarCheckNumber.IsReadOnly = false;
				colvarCheckNumber.DefaultSetting = @"";
				colvarCheckNumber.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCheckNumber);
				
				TableSchema.TableColumn colvarPayNotes = new TableSchema.TableColumn(schema);
				colvarPayNotes.ColumnName = "PayNotes";
				colvarPayNotes.DataType = DbType.AnsiString;
				colvarPayNotes.MaxLength = 2000;
				colvarPayNotes.AutoIncrement = false;
				colvarPayNotes.IsNullable = true;
				colvarPayNotes.IsPrimaryKey = false;
				colvarPayNotes.IsForeignKey = false;
				colvarPayNotes.IsReadOnly = false;
				colvarPayNotes.DefaultSetting = @"";
				colvarPayNotes.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPayNotes);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["WillCall"].AddSchema("Charge_Statement",schema);
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
		  
		[XmlAttribute("ApplicationId")]
		[Bindable(true)]
		public Guid ApplicationId 
		{
			get { return GetColumnValue<Guid>(Columns.ApplicationId); }
			set { SetColumnValue(Columns.ApplicationId, value); }
		}
		  
		[XmlAttribute("ChargeStatementId")]
		[Bindable(true)]
		public Guid ChargeStatementId 
		{
			get { return GetColumnValue<Guid>(Columns.ChargeStatementId); }
			set { SetColumnValue(Columns.ChargeStatementId, value); }
		}
		  
		[XmlAttribute("IMonth")]
		[Bindable(true)]
		public int IMonth 
		{
			get { return GetColumnValue<int>(Columns.IMonth); }
			set { SetColumnValue(Columns.IMonth, value); }
		}
		  
		[XmlAttribute("IYear")]
		[Bindable(true)]
		public int IYear 
		{
			get { return GetColumnValue<int>(Columns.IYear); }
			set { SetColumnValue(Columns.IYear, value); }
		}
		  
		[XmlAttribute("MonthYear")]
		[Bindable(true)]
		public string MonthYear 
		{
			get { return GetColumnValue<string>(Columns.MonthYear); }
			set { SetColumnValue(Columns.MonthYear, value); }
		}
		  
		[XmlAttribute("SalesQty")]
		[Bindable(true)]
		public int SalesQty 
		{
			get { return GetColumnValue<int>(Columns.SalesQty); }
			set { SetColumnValue(Columns.SalesQty, value); }
		}
		  
		[XmlAttribute("SalesQtyPct")]
		[Bindable(true)]
		public decimal SalesQtyPct 
		{
			get { return GetColumnValue<decimal>(Columns.SalesQtyPct); }
			set { SetColumnValue(Columns.SalesQtyPct, value); }
		}
		  
		[XmlAttribute("SalesQtyPortion")]
		[Bindable(true)]
		public decimal? SalesQtyPortion 
		{
			get { return GetColumnValue<decimal?>(Columns.SalesQtyPortion); }
			set { SetColumnValue(Columns.SalesQtyPortion, value); }
		}
		  
		[XmlAttribute("RefundQty")]
		[Bindable(true)]
		public int RefundQty 
		{
			get { return GetColumnValue<int>(Columns.RefundQty); }
			set { SetColumnValue(Columns.RefundQty, value); }
		}
		  
		[XmlAttribute("RefundQtyPct")]
		[Bindable(true)]
		public decimal RefundQtyPct 
		{
			get { return GetColumnValue<decimal>(Columns.RefundQtyPct); }
			set { SetColumnValue(Columns.RefundQtyPct, value); }
		}
		  
		[XmlAttribute("RefundQtyPortion")]
		[Bindable(true)]
		public decimal? RefundQtyPortion 
		{
			get { return GetColumnValue<decimal?>(Columns.RefundQtyPortion); }
			set { SetColumnValue(Columns.RefundQtyPortion, value); }
		}
		  
		[XmlAttribute("Gross")]
		[Bindable(true)]
		public decimal Gross 
		{
			get { return GetColumnValue<decimal>(Columns.Gross); }
			set { SetColumnValue(Columns.Gross, value); }
		}
		  
		[XmlAttribute("GrossPct")]
		[Bindable(true)]
		public decimal GrossPct 
		{
			get { return GetColumnValue<decimal>(Columns.GrossPct); }
			set { SetColumnValue(Columns.GrossPct, value); }
		}
		  
		[XmlAttribute("GrossThreshhold")]
		[Bindable(true)]
		public decimal GrossThreshhold 
		{
			get { return GetColumnValue<decimal>(Columns.GrossThreshhold); }
			set { SetColumnValue(Columns.GrossThreshhold, value); }
		}
		  
		[XmlAttribute("GrossPortion")]
		[Bindable(true)]
		public decimal? GrossPortion 
		{
			get { return GetColumnValue<decimal?>(Columns.GrossPortion); }
			set { SetColumnValue(Columns.GrossPortion, value); }
		}
		  
		[XmlAttribute("TicketInvoiceQty")]
		[Bindable(true)]
		public int TicketInvoiceQty 
		{
			get { return GetColumnValue<int>(Columns.TicketInvoiceQty); }
			set { SetColumnValue(Columns.TicketInvoiceQty, value); }
		}
		  
		[XmlAttribute("TicketInvoicePct")]
		[Bindable(true)]
		public decimal TicketInvoicePct 
		{
			get { return GetColumnValue<decimal>(Columns.TicketInvoicePct); }
			set { SetColumnValue(Columns.TicketInvoicePct, value); }
		}
		  
		[XmlAttribute("TicketUnitQty")]
		[Bindable(true)]
		public int TicketUnitQty 
		{
			get { return GetColumnValue<int>(Columns.TicketUnitQty); }
			set { SetColumnValue(Columns.TicketUnitQty, value); }
		}
		  
		[XmlAttribute("TicketUnitPct")]
		[Bindable(true)]
		public decimal TicketUnitPct 
		{
			get { return GetColumnValue<decimal>(Columns.TicketUnitPct); }
			set { SetColumnValue(Columns.TicketUnitPct, value); }
		}
		  
		[XmlAttribute("TicketSales")]
		[Bindable(true)]
		public decimal TicketSales 
		{
			get { return GetColumnValue<decimal>(Columns.TicketSales); }
			set { SetColumnValue(Columns.TicketSales, value); }
		}
		  
		[XmlAttribute("TicketSalesPct")]
		[Bindable(true)]
		public decimal TicketSalesPct 
		{
			get { return GetColumnValue<decimal>(Columns.TicketSalesPct); }
			set { SetColumnValue(Columns.TicketSalesPct, value); }
		}
		  
		[XmlAttribute("TicketPortion")]
		[Bindable(true)]
		public decimal? TicketPortion 
		{
			get { return GetColumnValue<decimal?>(Columns.TicketPortion); }
			set { SetColumnValue(Columns.TicketPortion, value); }
		}
		  
		[XmlAttribute("MerchInvoiceQty")]
		[Bindable(true)]
		public int MerchInvoiceQty 
		{
			get { return GetColumnValue<int>(Columns.MerchInvoiceQty); }
			set { SetColumnValue(Columns.MerchInvoiceQty, value); }
		}
		  
		[XmlAttribute("MerchInvoicePct")]
		[Bindable(true)]
		public decimal MerchInvoicePct 
		{
			get { return GetColumnValue<decimal>(Columns.MerchInvoicePct); }
			set { SetColumnValue(Columns.MerchInvoicePct, value); }
		}
		  
		[XmlAttribute("MerchUnitQty")]
		[Bindable(true)]
		public int MerchUnitQty 
		{
			get { return GetColumnValue<int>(Columns.MerchUnitQty); }
			set { SetColumnValue(Columns.MerchUnitQty, value); }
		}
		  
		[XmlAttribute("MerchUnitPct")]
		[Bindable(true)]
		public decimal MerchUnitPct 
		{
			get { return GetColumnValue<decimal>(Columns.MerchUnitPct); }
			set { SetColumnValue(Columns.MerchUnitPct, value); }
		}
		  
		[XmlAttribute("MerchSales")]
		[Bindable(true)]
		public decimal MerchSales 
		{
			get { return GetColumnValue<decimal>(Columns.MerchSales); }
			set { SetColumnValue(Columns.MerchSales, value); }
		}
		  
		[XmlAttribute("MerchSalesPct")]
		[Bindable(true)]
		public decimal MerchSalesPct 
		{
			get { return GetColumnValue<decimal>(Columns.MerchSalesPct); }
			set { SetColumnValue(Columns.MerchSalesPct, value); }
		}
		  
		[XmlAttribute("MerchPortion")]
		[Bindable(true)]
		public decimal? MerchPortion 
		{
			get { return GetColumnValue<decimal?>(Columns.MerchPortion); }
			set { SetColumnValue(Columns.MerchPortion, value); }
		}
		  
		[XmlAttribute("ShipUnitQty")]
		[Bindable(true)]
		public int ShipUnitQty 
		{
			get { return GetColumnValue<int>(Columns.ShipUnitQty); }
			set { SetColumnValue(Columns.ShipUnitQty, value); }
		}
		  
		[XmlAttribute("ShipUnitPct")]
		[Bindable(true)]
		public decimal ShipUnitPct 
		{
			get { return GetColumnValue<decimal>(Columns.ShipUnitPct); }
			set { SetColumnValue(Columns.ShipUnitPct, value); }
		}
		  
		[XmlAttribute("ShipSales")]
		[Bindable(true)]
		public decimal ShipSales 
		{
			get { return GetColumnValue<decimal>(Columns.ShipSales); }
			set { SetColumnValue(Columns.ShipSales, value); }
		}
		  
		[XmlAttribute("ShipSalesPct")]
		[Bindable(true)]
		public decimal ShipSalesPct 
		{
			get { return GetColumnValue<decimal>(Columns.ShipSalesPct); }
			set { SetColumnValue(Columns.ShipSalesPct, value); }
		}
		  
		[XmlAttribute("ShipPortion")]
		[Bindable(true)]
		public decimal? ShipPortion 
		{
			get { return GetColumnValue<decimal?>(Columns.ShipPortion); }
			set { SetColumnValue(Columns.ShipPortion, value); }
		}
		  
		[XmlAttribute("SubscriptionsSent")]
		[Bindable(true)]
		public int SubscriptionsSent 
		{
			get { return GetColumnValue<int>(Columns.SubscriptionsSent); }
			set { SetColumnValue(Columns.SubscriptionsSent, value); }
		}
		  
		[XmlAttribute("PerSubscription")]
		[Bindable(true)]
		public decimal PerSubscription 
		{
			get { return GetColumnValue<decimal>(Columns.PerSubscription); }
			set { SetColumnValue(Columns.PerSubscription, value); }
		}
		  
		[XmlAttribute("MailSent")]
		[Bindable(true)]
		public int MailSent 
		{
			get { return GetColumnValue<int>(Columns.MailSent); }
			set { SetColumnValue(Columns.MailSent, value); }
		}
		  
		[XmlAttribute("PerMailSent")]
		[Bindable(true)]
		public decimal PerMailSent 
		{
			get { return GetColumnValue<decimal>(Columns.PerMailSent); }
			set { SetColumnValue(Columns.PerMailSent, value); }
		}
		  
		[XmlAttribute("MailerPortion")]
		[Bindable(true)]
		public decimal? MailerPortion 
		{
			get { return GetColumnValue<decimal?>(Columns.MailerPortion); }
			set { SetColumnValue(Columns.MailerPortion, value); }
		}
		  
		[XmlAttribute("HourlyPortion")]
		[Bindable(true)]
		public decimal HourlyPortion 
		{
			get { return GetColumnValue<decimal>(Columns.HourlyPortion); }
			set { SetColumnValue(Columns.HourlyPortion, value); }
		}
		  
		[XmlAttribute("Discount")]
		[Bindable(true)]
		public decimal Discount 
		{
			get { return GetColumnValue<decimal>(Columns.Discount); }
			set { SetColumnValue(Columns.Discount, value); }
		}
		  
		[XmlAttribute("LineTotal")]
		[Bindable(true)]
		public decimal? LineTotal 
		{
			get { return GetColumnValue<decimal?>(Columns.LineTotal); }
			set { SetColumnValue(Columns.LineTotal, value); }
		}
		  
		[XmlAttribute("AmountPaid")]
		[Bindable(true)]
		public decimal AmountPaid 
		{
			get { return GetColumnValue<decimal>(Columns.AmountPaid); }
			set { SetColumnValue(Columns.AmountPaid, value); }
		}
		  
		[XmlAttribute("DtPaid")]
		[Bindable(true)]
		public DateTime? DtPaid 
		{
			get { return GetColumnValue<DateTime?>(Columns.DtPaid); }
			set { SetColumnValue(Columns.DtPaid, value); }
		}
		  
		[XmlAttribute("CheckNumber")]
		[Bindable(true)]
		public string CheckNumber 
		{
			get { return GetColumnValue<string>(Columns.CheckNumber); }
			set { SetColumnValue(Columns.CheckNumber, value); }
		}
		  
		[XmlAttribute("PayNotes")]
		[Bindable(true)]
		public string PayNotes 
		{
			get { return GetColumnValue<string>(Columns.PayNotes); }
			set { SetColumnValue(Columns.PayNotes, value); }
		}
		
		#endregion
		
		
		#region PrimaryKey Methods		
		
        protected override void SetPrimaryKey(object oValue)
        {
            base.SetPrimaryKey(oValue);
            
            SetPKValues();
        }
        
		
		private Wcss.ChargeHourlyCollection colChargeHourlyRecords;
		public Wcss.ChargeHourlyCollection ChargeHourlyRecords()
		{
			if(colChargeHourlyRecords == null)
			{
				colChargeHourlyRecords = new Wcss.ChargeHourlyCollection().Where(ChargeHourly.Columns.TChargeStatementId, Id).Load();
				colChargeHourlyRecords.ListChanged += new ListChangedEventHandler(colChargeHourlyRecords_ListChanged);
			}
			return colChargeHourlyRecords;
		}
				
		void colChargeHourlyRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colChargeHourlyRecords[e.NewIndex].TChargeStatementId = Id;
				colChargeHourlyRecords.ListChanged += new ListChangedEventHandler(colChargeHourlyRecords_ListChanged);
            }
		}
		#endregion
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a AspnetApplication ActiveRecord object related to this ChargeStatement
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
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(DateTime varDtStamp,Guid varApplicationId,Guid varChargeStatementId,int varIMonth,int varIYear,string varMonthYear,int varSalesQty,decimal varSalesQtyPct,decimal? varSalesQtyPortion,int varRefundQty,decimal varRefundQtyPct,decimal? varRefundQtyPortion,decimal varGross,decimal varGrossPct,decimal varGrossThreshhold,decimal? varGrossPortion,int varTicketInvoiceQty,decimal varTicketInvoicePct,int varTicketUnitQty,decimal varTicketUnitPct,decimal varTicketSales,decimal varTicketSalesPct,decimal? varTicketPortion,int varMerchInvoiceQty,decimal varMerchInvoicePct,int varMerchUnitQty,decimal varMerchUnitPct,decimal varMerchSales,decimal varMerchSalesPct,decimal? varMerchPortion,int varShipUnitQty,decimal varShipUnitPct,decimal varShipSales,decimal varShipSalesPct,decimal? varShipPortion,int varSubscriptionsSent,decimal varPerSubscription,int varMailSent,decimal varPerMailSent,decimal? varMailerPortion,decimal varHourlyPortion,decimal varDiscount,decimal? varLineTotal,decimal varAmountPaid,DateTime? varDtPaid,string varCheckNumber,string varPayNotes)
		{
			ChargeStatement item = new ChargeStatement();
			
			item.DtStamp = varDtStamp;
			
			item.ApplicationId = varApplicationId;
			
			item.ChargeStatementId = varChargeStatementId;
			
			item.IMonth = varIMonth;
			
			item.IYear = varIYear;
			
			item.MonthYear = varMonthYear;
			
			item.SalesQty = varSalesQty;
			
			item.SalesQtyPct = varSalesQtyPct;
			
			item.SalesQtyPortion = varSalesQtyPortion;
			
			item.RefundQty = varRefundQty;
			
			item.RefundQtyPct = varRefundQtyPct;
			
			item.RefundQtyPortion = varRefundQtyPortion;
			
			item.Gross = varGross;
			
			item.GrossPct = varGrossPct;
			
			item.GrossThreshhold = varGrossThreshhold;
			
			item.GrossPortion = varGrossPortion;
			
			item.TicketInvoiceQty = varTicketInvoiceQty;
			
			item.TicketInvoicePct = varTicketInvoicePct;
			
			item.TicketUnitQty = varTicketUnitQty;
			
			item.TicketUnitPct = varTicketUnitPct;
			
			item.TicketSales = varTicketSales;
			
			item.TicketSalesPct = varTicketSalesPct;
			
			item.TicketPortion = varTicketPortion;
			
			item.MerchInvoiceQty = varMerchInvoiceQty;
			
			item.MerchInvoicePct = varMerchInvoicePct;
			
			item.MerchUnitQty = varMerchUnitQty;
			
			item.MerchUnitPct = varMerchUnitPct;
			
			item.MerchSales = varMerchSales;
			
			item.MerchSalesPct = varMerchSalesPct;
			
			item.MerchPortion = varMerchPortion;
			
			item.ShipUnitQty = varShipUnitQty;
			
			item.ShipUnitPct = varShipUnitPct;
			
			item.ShipSales = varShipSales;
			
			item.ShipSalesPct = varShipSalesPct;
			
			item.ShipPortion = varShipPortion;
			
			item.SubscriptionsSent = varSubscriptionsSent;
			
			item.PerSubscription = varPerSubscription;
			
			item.MailSent = varMailSent;
			
			item.PerMailSent = varPerMailSent;
			
			item.MailerPortion = varMailerPortion;
			
			item.HourlyPortion = varHourlyPortion;
			
			item.Discount = varDiscount;
			
			item.LineTotal = varLineTotal;
			
			item.AmountPaid = varAmountPaid;
			
			item.DtPaid = varDtPaid;
			
			item.CheckNumber = varCheckNumber;
			
			item.PayNotes = varPayNotes;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varId,DateTime varDtStamp,Guid varApplicationId,Guid varChargeStatementId,int varIMonth,int varIYear,string varMonthYear,int varSalesQty,decimal varSalesQtyPct,decimal? varSalesQtyPortion,int varRefundQty,decimal varRefundQtyPct,decimal? varRefundQtyPortion,decimal varGross,decimal varGrossPct,decimal varGrossThreshhold,decimal? varGrossPortion,int varTicketInvoiceQty,decimal varTicketInvoicePct,int varTicketUnitQty,decimal varTicketUnitPct,decimal varTicketSales,decimal varTicketSalesPct,decimal? varTicketPortion,int varMerchInvoiceQty,decimal varMerchInvoicePct,int varMerchUnitQty,decimal varMerchUnitPct,decimal varMerchSales,decimal varMerchSalesPct,decimal? varMerchPortion,int varShipUnitQty,decimal varShipUnitPct,decimal varShipSales,decimal varShipSalesPct,decimal? varShipPortion,int varSubscriptionsSent,decimal varPerSubscription,int varMailSent,decimal varPerMailSent,decimal? varMailerPortion,decimal varHourlyPortion,decimal varDiscount,decimal? varLineTotal,decimal varAmountPaid,DateTime? varDtPaid,string varCheckNumber,string varPayNotes)
		{
			ChargeStatement item = new ChargeStatement();
			
				item.Id = varId;
			
				item.DtStamp = varDtStamp;
			
				item.ApplicationId = varApplicationId;
			
				item.ChargeStatementId = varChargeStatementId;
			
				item.IMonth = varIMonth;
			
				item.IYear = varIYear;
			
				item.MonthYear = varMonthYear;
			
				item.SalesQty = varSalesQty;
			
				item.SalesQtyPct = varSalesQtyPct;
			
				item.SalesQtyPortion = varSalesQtyPortion;
			
				item.RefundQty = varRefundQty;
			
				item.RefundQtyPct = varRefundQtyPct;
			
				item.RefundQtyPortion = varRefundQtyPortion;
			
				item.Gross = varGross;
			
				item.GrossPct = varGrossPct;
			
				item.GrossThreshhold = varGrossThreshhold;
			
				item.GrossPortion = varGrossPortion;
			
				item.TicketInvoiceQty = varTicketInvoiceQty;
			
				item.TicketInvoicePct = varTicketInvoicePct;
			
				item.TicketUnitQty = varTicketUnitQty;
			
				item.TicketUnitPct = varTicketUnitPct;
			
				item.TicketSales = varTicketSales;
			
				item.TicketSalesPct = varTicketSalesPct;
			
				item.TicketPortion = varTicketPortion;
			
				item.MerchInvoiceQty = varMerchInvoiceQty;
			
				item.MerchInvoicePct = varMerchInvoicePct;
			
				item.MerchUnitQty = varMerchUnitQty;
			
				item.MerchUnitPct = varMerchUnitPct;
			
				item.MerchSales = varMerchSales;
			
				item.MerchSalesPct = varMerchSalesPct;
			
				item.MerchPortion = varMerchPortion;
			
				item.ShipUnitQty = varShipUnitQty;
			
				item.ShipUnitPct = varShipUnitPct;
			
				item.ShipSales = varShipSales;
			
				item.ShipSalesPct = varShipSalesPct;
			
				item.ShipPortion = varShipPortion;
			
				item.SubscriptionsSent = varSubscriptionsSent;
			
				item.PerSubscription = varPerSubscription;
			
				item.MailSent = varMailSent;
			
				item.PerMailSent = varPerMailSent;
			
				item.MailerPortion = varMailerPortion;
			
				item.HourlyPortion = varHourlyPortion;
			
				item.Discount = varDiscount;
			
				item.LineTotal = varLineTotal;
			
				item.AmountPaid = varAmountPaid;
			
				item.DtPaid = varDtPaid;
			
				item.CheckNumber = varCheckNumber;
			
				item.PayNotes = varPayNotes;
			
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
        
        
        
        public static TableSchema.TableColumn ChargeStatementIdColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn IMonthColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn IYearColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn MonthYearColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn SalesQtyColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn SalesQtyPctColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn SalesQtyPortionColumn
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        public static TableSchema.TableColumn RefundQtyColumn
        {
            get { return Schema.Columns[10]; }
        }
        
        
        
        public static TableSchema.TableColumn RefundQtyPctColumn
        {
            get { return Schema.Columns[11]; }
        }
        
        
        
        public static TableSchema.TableColumn RefundQtyPortionColumn
        {
            get { return Schema.Columns[12]; }
        }
        
        
        
        public static TableSchema.TableColumn GrossColumn
        {
            get { return Schema.Columns[13]; }
        }
        
        
        
        public static TableSchema.TableColumn GrossPctColumn
        {
            get { return Schema.Columns[14]; }
        }
        
        
        
        public static TableSchema.TableColumn GrossThreshholdColumn
        {
            get { return Schema.Columns[15]; }
        }
        
        
        
        public static TableSchema.TableColumn GrossPortionColumn
        {
            get { return Schema.Columns[16]; }
        }
        
        
        
        public static TableSchema.TableColumn TicketInvoiceQtyColumn
        {
            get { return Schema.Columns[17]; }
        }
        
        
        
        public static TableSchema.TableColumn TicketInvoicePctColumn
        {
            get { return Schema.Columns[18]; }
        }
        
        
        
        public static TableSchema.TableColumn TicketUnitQtyColumn
        {
            get { return Schema.Columns[19]; }
        }
        
        
        
        public static TableSchema.TableColumn TicketUnitPctColumn
        {
            get { return Schema.Columns[20]; }
        }
        
        
        
        public static TableSchema.TableColumn TicketSalesColumn
        {
            get { return Schema.Columns[21]; }
        }
        
        
        
        public static TableSchema.TableColumn TicketSalesPctColumn
        {
            get { return Schema.Columns[22]; }
        }
        
        
        
        public static TableSchema.TableColumn TicketPortionColumn
        {
            get { return Schema.Columns[23]; }
        }
        
        
        
        public static TableSchema.TableColumn MerchInvoiceQtyColumn
        {
            get { return Schema.Columns[24]; }
        }
        
        
        
        public static TableSchema.TableColumn MerchInvoicePctColumn
        {
            get { return Schema.Columns[25]; }
        }
        
        
        
        public static TableSchema.TableColumn MerchUnitQtyColumn
        {
            get { return Schema.Columns[26]; }
        }
        
        
        
        public static TableSchema.TableColumn MerchUnitPctColumn
        {
            get { return Schema.Columns[27]; }
        }
        
        
        
        public static TableSchema.TableColumn MerchSalesColumn
        {
            get { return Schema.Columns[28]; }
        }
        
        
        
        public static TableSchema.TableColumn MerchSalesPctColumn
        {
            get { return Schema.Columns[29]; }
        }
        
        
        
        public static TableSchema.TableColumn MerchPortionColumn
        {
            get { return Schema.Columns[30]; }
        }
        
        
        
        public static TableSchema.TableColumn ShipUnitQtyColumn
        {
            get { return Schema.Columns[31]; }
        }
        
        
        
        public static TableSchema.TableColumn ShipUnitPctColumn
        {
            get { return Schema.Columns[32]; }
        }
        
        
        
        public static TableSchema.TableColumn ShipSalesColumn
        {
            get { return Schema.Columns[33]; }
        }
        
        
        
        public static TableSchema.TableColumn ShipSalesPctColumn
        {
            get { return Schema.Columns[34]; }
        }
        
        
        
        public static TableSchema.TableColumn ShipPortionColumn
        {
            get { return Schema.Columns[35]; }
        }
        
        
        
        public static TableSchema.TableColumn SubscriptionsSentColumn
        {
            get { return Schema.Columns[36]; }
        }
        
        
        
        public static TableSchema.TableColumn PerSubscriptionColumn
        {
            get { return Schema.Columns[37]; }
        }
        
        
        
        public static TableSchema.TableColumn MailSentColumn
        {
            get { return Schema.Columns[38]; }
        }
        
        
        
        public static TableSchema.TableColumn PerMailSentColumn
        {
            get { return Schema.Columns[39]; }
        }
        
        
        
        public static TableSchema.TableColumn MailerPortionColumn
        {
            get { return Schema.Columns[40]; }
        }
        
        
        
        public static TableSchema.TableColumn HourlyPortionColumn
        {
            get { return Schema.Columns[41]; }
        }
        
        
        
        public static TableSchema.TableColumn DiscountColumn
        {
            get { return Schema.Columns[42]; }
        }
        
        
        
        public static TableSchema.TableColumn LineTotalColumn
        {
            get { return Schema.Columns[43]; }
        }
        
        
        
        public static TableSchema.TableColumn AmountPaidColumn
        {
            get { return Schema.Columns[44]; }
        }
        
        
        
        public static TableSchema.TableColumn DtPaidColumn
        {
            get { return Schema.Columns[45]; }
        }
        
        
        
        public static TableSchema.TableColumn CheckNumberColumn
        {
            get { return Schema.Columns[46]; }
        }
        
        
        
        public static TableSchema.TableColumn PayNotesColumn
        {
            get { return Schema.Columns[47]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Id = @"Id";
			 public static string DtStamp = @"dtStamp";
			 public static string ApplicationId = @"ApplicationId";
			 public static string ChargeStatementId = @"ChargeStatementId";
			 public static string IMonth = @"iMonth";
			 public static string IYear = @"iYear";
			 public static string MonthYear = @"MonthYear";
			 public static string SalesQty = @"SalesQty";
			 public static string SalesQtyPct = @"SalesQtyPct";
			 public static string SalesQtyPortion = @"SalesQtyPortion";
			 public static string RefundQty = @"RefundQty";
			 public static string RefundQtyPct = @"RefundQtyPct";
			 public static string RefundQtyPortion = @"RefundQtyPortion";
			 public static string Gross = @"Gross";
			 public static string GrossPct = @"GrossPct";
			 public static string GrossThreshhold = @"GrossThreshhold";
			 public static string GrossPortion = @"GrossPortion";
			 public static string TicketInvoiceQty = @"TicketInvoiceQty";
			 public static string TicketInvoicePct = @"TicketInvoicePct";
			 public static string TicketUnitQty = @"TicketUnitQty";
			 public static string TicketUnitPct = @"TicketUnitPct";
			 public static string TicketSales = @"TicketSales";
			 public static string TicketSalesPct = @"TicketSalesPct";
			 public static string TicketPortion = @"TicketPortion";
			 public static string MerchInvoiceQty = @"MerchInvoiceQty";
			 public static string MerchInvoicePct = @"MerchInvoicePct";
			 public static string MerchUnitQty = @"MerchUnitQty";
			 public static string MerchUnitPct = @"MerchUnitPct";
			 public static string MerchSales = @"MerchSales";
			 public static string MerchSalesPct = @"MerchSalesPct";
			 public static string MerchPortion = @"MerchPortion";
			 public static string ShipUnitQty = @"ShipUnitQty";
			 public static string ShipUnitPct = @"ShipUnitPct";
			 public static string ShipSales = @"ShipSales";
			 public static string ShipSalesPct = @"ShipSalesPct";
			 public static string ShipPortion = @"ShipPortion";
			 public static string SubscriptionsSent = @"SubscriptionsSent";
			 public static string PerSubscription = @"PerSubscription";
			 public static string MailSent = @"MailSent";
			 public static string PerMailSent = @"PerMailSent";
			 public static string MailerPortion = @"MailerPortion";
			 public static string HourlyPortion = @"HourlyPortion";
			 public static string Discount = @"Discount";
			 public static string LineTotal = @"LineTotal";
			 public static string AmountPaid = @"AmountPaid";
			 public static string DtPaid = @"dtPaid";
			 public static string CheckNumber = @"CheckNumber";
			 public static string PayNotes = @"PayNotes";
						
		}
		#endregion
		
		#region Update PK Collections
		
        public void SetPKValues()
        {
                if (colChargeHourlyRecords != null)
                {
                    foreach (Wcss.ChargeHourly item in colChargeHourlyRecords)
                    {
                        if (item.TChargeStatementId != Id)
                        {
                            item.TChargeStatementId = Id;
                        }
                    }
               }
		}
        #endregion
    
        #region Deep Save
		
        public void DeepSave()
        {
            Save();
            
                if (colChargeHourlyRecords != null)
                {
                    colChargeHourlyRecords.SaveAll();
               }
		}
        #endregion
	}
}
