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
namespace Wcss{
    /// <summary>
    /// Strongly-typed collection for the VwShowTicketWithPending class.
    /// </summary>
    [Serializable]
    public partial class VwShowTicketWithPendingCollection : ReadOnlyList<VwShowTicketWithPending, VwShowTicketWithPendingCollection>
    {        
        public VwShowTicketWithPendingCollection() {}
    }
    /// <summary>
    /// This is  Read-only wrapper class for the vw_ShowTicketWithPending view.
    /// </summary>
    [Serializable]
    public partial class VwShowTicketWithPending : ReadOnlyRecord<VwShowTicketWithPending>, IReadOnlyRecord
    {
    
	    #region Default Settings
	    protected static void SetSQLProps() 
	    {
		    GetTableSchema();
	    }
	    #endregion
        #region Schema Accessor
	    public static TableSchema.Table Schema
        {
            get
            {
                if (BaseSchema == null)
                {
                    SetSQLProps();
                }
                return BaseSchema;
            }
        }
    	
        private static void GetTableSchema() 
        {
            if(!IsSchemaInitialized)
            {
                //Schema declaration
                TableSchema.Table schema = new TableSchema.Table("vw_ShowTicketWithPending", TableType.View, DataService.GetInstance("WillCall"));
                schema.Columns = new TableSchema.TableColumnCollection();
                schema.SchemaName = @"dbo";
                //columns
                
                TableSchema.TableColumn colvarId = new TableSchema.TableColumn(schema);
                colvarId.ColumnName = "Id";
                colvarId.DataType = DbType.Int32;
                colvarId.MaxLength = 0;
                colvarId.AutoIncrement = false;
                colvarId.IsNullable = false;
                colvarId.IsPrimaryKey = false;
                colvarId.IsForeignKey = false;
                colvarId.IsReadOnly = false;
                
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
                
                schema.Columns.Add(colvarSalesDescription);
                
                TableSchema.TableColumn colvarTShowDateId = new TableSchema.TableColumn(schema);
                colvarTShowDateId.ColumnName = "TShowDateId";
                colvarTShowDateId.DataType = DbType.Int32;
                colvarTShowDateId.MaxLength = 0;
                colvarTShowDateId.AutoIncrement = false;
                colvarTShowDateId.IsNullable = false;
                colvarTShowDateId.IsPrimaryKey = false;
                colvarTShowDateId.IsForeignKey = false;
                colvarTShowDateId.IsReadOnly = false;
                
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
                
                schema.Columns.Add(colvarBAllowWillCall);
                
                TableSchema.TableColumn colvarDtShipCutoff = new TableSchema.TableColumn(schema);
                colvarDtShipCutoff.ColumnName = "dtShipCutoff";
                colvarDtShipCutoff.DataType = DbType.DateTime;
                colvarDtShipCutoff.MaxLength = 0;
                colvarDtShipCutoff.AutoIncrement = false;
                colvarDtShipCutoff.IsNullable = false;
                colvarDtShipCutoff.IsPrimaryKey = false;
                colvarDtShipCutoff.IsForeignKey = false;
                colvarDtShipCutoff.IsReadOnly = false;
                
                schema.Columns.Add(colvarDtShipCutoff);
                
                TableSchema.TableColumn colvarBUnlockActive = new TableSchema.TableColumn(schema);
                colvarBUnlockActive.ColumnName = "bUnlockActive";
                colvarBUnlockActive.DataType = DbType.Boolean;
                colvarBUnlockActive.MaxLength = 0;
                colvarBUnlockActive.AutoIncrement = false;
                colvarBUnlockActive.IsNullable = false;
                colvarBUnlockActive.IsPrimaryKey = false;
                colvarBUnlockActive.IsForeignKey = false;
                colvarBUnlockActive.IsReadOnly = false;
                
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
                
                schema.Columns.Add(colvarISold);
                
                TableSchema.TableColumn colvarIAvailable = new TableSchema.TableColumn(schema);
                colvarIAvailable.ColumnName = "iAvailable";
                colvarIAvailable.DataType = DbType.Int32;
                colvarIAvailable.MaxLength = 0;
                colvarIAvailable.AutoIncrement = false;
                colvarIAvailable.IsNullable = true;
                colvarIAvailable.IsPrimaryKey = false;
                colvarIAvailable.IsForeignKey = false;
                colvarIAvailable.IsReadOnly = false;
                
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
                
                schema.Columns.Add(colvarDtStamp);
                
                
                BaseSchema = schema;
                //add this schema to the provider
                //so we can query it later
                DataService.Providers["WillCall"].AddSchema("vw_ShowTicketWithPending",schema);
            }
        }
        #endregion
        
        #region Query Accessor
	    public static Query CreateQuery()
	    {
		    return new Query(Schema);
	    }
	    #endregion
	    
	    #region .ctors
	    public VwShowTicketWithPending()
	    {
            SetSQLProps();
            SetDefaults();
            MarkNew();
        }
        public VwShowTicketWithPending(bool useDatabaseDefaults)
	    {
		    SetSQLProps();
		    if(useDatabaseDefaults)
		    {
				ForceDefaults();
			}
			MarkNew();
	    }
	    
	    public VwShowTicketWithPending(object keyID)
	    {
		    SetSQLProps();
		    LoadByKey(keyID);
	    }
    	 
	    public VwShowTicketWithPending(string columnName, object columnValue)
        {
            SetSQLProps();
            LoadByParam(columnName,columnValue);
        }
        
	    #endregion
	    
	    #region Props
	    
          
        [XmlAttribute("Id")]
        [Bindable(true)]
        public int Id 
	    {
		    get
		    {
			    return GetColumnValue<int>("Id");
		    }
            set 
		    {
			    SetColumnValue("Id", value);
            }
        }
	      
        [XmlAttribute("TVendorId")]
        [Bindable(true)]
        public int TVendorId 
	    {
		    get
		    {
			    return GetColumnValue<int>("TVendorId");
		    }
            set 
		    {
			    SetColumnValue("TVendorId", value);
            }
        }
	      
        [XmlAttribute("DtDateOfShow")]
        [Bindable(true)]
        public DateTime DtDateOfShow 
	    {
		    get
		    {
			    return GetColumnValue<DateTime>("dtDateOfShow");
		    }
            set 
		    {
			    SetColumnValue("dtDateOfShow", value);
            }
        }
	      
        [XmlAttribute("CriteriaText")]
        [Bindable(true)]
        public string CriteriaText 
	    {
		    get
		    {
			    return GetColumnValue<string>("CriteriaText");
		    }
            set 
		    {
			    SetColumnValue("CriteriaText", value);
            }
        }
	      
        [XmlAttribute("SalesDescription")]
        [Bindable(true)]
        public string SalesDescription 
	    {
		    get
		    {
			    return GetColumnValue<string>("SalesDescription");
		    }
            set 
		    {
			    SetColumnValue("SalesDescription", value);
            }
        }
	      
        [XmlAttribute("TShowDateId")]
        [Bindable(true)]
        public int TShowDateId 
	    {
		    get
		    {
			    return GetColumnValue<int>("TShowDateId");
		    }
            set 
		    {
			    SetColumnValue("TShowDateId", value);
            }
        }
	      
        [XmlAttribute("TShowId")]
        [Bindable(true)]
        public int TShowId 
	    {
		    get
		    {
			    return GetColumnValue<int>("TShowId");
		    }
            set 
		    {
			    SetColumnValue("TShowId", value);
            }
        }
	      
        [XmlAttribute("TAgeId")]
        [Bindable(true)]
        public int TAgeId 
	    {
		    get
		    {
			    return GetColumnValue<int>("TAgeId");
		    }
            set 
		    {
			    SetColumnValue("TAgeId", value);
            }
        }
	      
        [XmlAttribute("BActive")]
        [Bindable(true)]
        public bool BActive 
	    {
		    get
		    {
			    return GetColumnValue<bool>("bActive");
		    }
            set 
		    {
			    SetColumnValue("bActive", value);
            }
        }
	      
        [XmlAttribute("BSoldOut")]
        [Bindable(true)]
        public bool BSoldOut 
	    {
		    get
		    {
			    return GetColumnValue<bool>("bSoldOut");
		    }
            set 
		    {
			    SetColumnValue("bSoldOut", value);
            }
        }
	      
        [XmlAttribute("Status")]
        [Bindable(true)]
        public string Status 
	    {
		    get
		    {
			    return GetColumnValue<string>("Status");
		    }
            set 
		    {
			    SetColumnValue("Status", value);
            }
        }
	      
        [XmlAttribute("BDosTicket")]
        [Bindable(true)]
        public bool BDosTicket 
	    {
		    get
		    {
			    return GetColumnValue<bool>("bDosTicket");
		    }
            set 
		    {
			    SetColumnValue("bDosTicket", value);
            }
        }
	      
        [XmlAttribute("IDisplayOrder")]
        [Bindable(true)]
        public int IDisplayOrder 
	    {
		    get
		    {
			    return GetColumnValue<int>("iDisplayOrder");
		    }
            set 
		    {
			    SetColumnValue("iDisplayOrder", value);
            }
        }
	      
        [XmlAttribute("PriceText")]
        [Bindable(true)]
        public string PriceText 
	    {
		    get
		    {
			    return GetColumnValue<string>("PriceText");
		    }
            set 
		    {
			    SetColumnValue("PriceText", value);
            }
        }
	      
        [XmlAttribute("MPrice")]
        [Bindable(true)]
        public decimal? MPrice 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("mPrice");
		    }
            set 
		    {
			    SetColumnValue("mPrice", value);
            }
        }
	      
        [XmlAttribute("DosText")]
        [Bindable(true)]
        public string DosText 
	    {
		    get
		    {
			    return GetColumnValue<string>("DosText");
		    }
            set 
		    {
			    SetColumnValue("DosText", value);
            }
        }
	      
        [XmlAttribute("MDosPrice")]
        [Bindable(true)]
        public decimal? MDosPrice 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("mDosPrice");
		    }
            set 
		    {
			    SetColumnValue("mDosPrice", value);
            }
        }
	      
        [XmlAttribute("MServiceCharge")]
        [Bindable(true)]
        public decimal? MServiceCharge 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("mServiceCharge");
		    }
            set 
		    {
			    SetColumnValue("mServiceCharge", value);
            }
        }
	      
        [XmlAttribute("BAllowShipping")]
        [Bindable(true)]
        public bool BAllowShipping 
	    {
		    get
		    {
			    return GetColumnValue<bool>("bAllowShipping");
		    }
            set 
		    {
			    SetColumnValue("bAllowShipping", value);
            }
        }
	      
        [XmlAttribute("BAllowWillCall")]
        [Bindable(true)]
        public bool BAllowWillCall 
	    {
		    get
		    {
			    return GetColumnValue<bool>("bAllowWillCall");
		    }
            set 
		    {
			    SetColumnValue("bAllowWillCall", value);
            }
        }
	      
        [XmlAttribute("DtShipCutoff")]
        [Bindable(true)]
        public DateTime DtShipCutoff 
	    {
		    get
		    {
			    return GetColumnValue<DateTime>("dtShipCutoff");
		    }
            set 
		    {
			    SetColumnValue("dtShipCutoff", value);
            }
        }
	      
        [XmlAttribute("BUnlockActive")]
        [Bindable(true)]
        public bool BUnlockActive 
	    {
		    get
		    {
			    return GetColumnValue<bool>("bUnlockActive");
		    }
            set 
		    {
			    SetColumnValue("bUnlockActive", value);
            }
        }
	      
        [XmlAttribute("UnlockCode")]
        [Bindable(true)]
        public string UnlockCode 
	    {
		    get
		    {
			    return GetColumnValue<string>("UnlockCode");
		    }
            set 
		    {
			    SetColumnValue("UnlockCode", value);
            }
        }
	      
        [XmlAttribute("DtUnlockDate")]
        [Bindable(true)]
        public DateTime? DtUnlockDate 
	    {
		    get
		    {
			    return GetColumnValue<DateTime?>("dtUnlockDate");
		    }
            set 
		    {
			    SetColumnValue("dtUnlockDate", value);
            }
        }
	      
        [XmlAttribute("DtUnlockEndDate")]
        [Bindable(true)]
        public DateTime? DtUnlockEndDate 
	    {
		    get
		    {
			    return GetColumnValue<DateTime?>("dtUnlockEndDate");
		    }
            set 
		    {
			    SetColumnValue("dtUnlockEndDate", value);
            }
        }
	      
        [XmlAttribute("DtPublicOnsale")]
        [Bindable(true)]
        public DateTime? DtPublicOnsale 
	    {
		    get
		    {
			    return GetColumnValue<DateTime?>("dtPublicOnsale");
		    }
            set 
		    {
			    SetColumnValue("dtPublicOnsale", value);
            }
        }
	      
        [XmlAttribute("DtEndDate")]
        [Bindable(true)]
        public DateTime? DtEndDate 
	    {
		    get
		    {
			    return GetColumnValue<DateTime?>("dtEndDate");
		    }
            set 
		    {
			    SetColumnValue("dtEndDate", value);
            }
        }
	      
        [XmlAttribute("IMaxQtyPerOrder")]
        [Bindable(true)]
        public int? IMaxQtyPerOrder 
	    {
		    get
		    {
			    return GetColumnValue<int?>("iMaxQtyPerOrder");
		    }
            set 
		    {
			    SetColumnValue("iMaxQtyPerOrder", value);
            }
        }
	      
        [XmlAttribute("IAllotment")]
        [Bindable(true)]
        public int IAllotment 
	    {
		    get
		    {
			    return GetColumnValue<int>("iAllotment");
		    }
            set 
		    {
			    SetColumnValue("iAllotment", value);
            }
        }
	      
        [XmlAttribute("IPending")]
        [Bindable(true)]
        public int IPending 
	    {
		    get
		    {
			    return GetColumnValue<int>("iPending");
		    }
            set 
		    {
			    SetColumnValue("iPending", value);
            }
        }
	      
        [XmlAttribute("ISold")]
        [Bindable(true)]
        public int ISold 
	    {
		    get
		    {
			    return GetColumnValue<int>("iSold");
		    }
            set 
		    {
			    SetColumnValue("iSold", value);
            }
        }
	      
        [XmlAttribute("IAvailable")]
        [Bindable(true)]
        public int? IAvailable 
	    {
		    get
		    {
			    return GetColumnValue<int?>("iAvailable");
		    }
            set 
		    {
			    SetColumnValue("iAvailable", value);
            }
        }
	      
        [XmlAttribute("IRefunded")]
        [Bindable(true)]
        public int IRefunded 
	    {
		    get
		    {
			    return GetColumnValue<int>("iRefunded");
		    }
            set 
		    {
			    SetColumnValue("iRefunded", value);
            }
        }
	      
        [XmlAttribute("MFlatShip")]
        [Bindable(true)]
        public decimal? MFlatShip 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("mFlatShip");
		    }
            set 
		    {
			    SetColumnValue("mFlatShip", value);
            }
        }
	      
        [XmlAttribute("VcFlatMethod")]
        [Bindable(true)]
        public string VcFlatMethod 
	    {
		    get
		    {
			    return GetColumnValue<string>("vcFlatMethod");
		    }
            set 
		    {
			    SetColumnValue("vcFlatMethod", value);
            }
        }
	      
        [XmlAttribute("DtBackorder")]
        [Bindable(true)]
        public DateTime? DtBackorder 
	    {
		    get
		    {
			    return GetColumnValue<DateTime?>("dtBackorder");
		    }
            set 
		    {
			    SetColumnValue("dtBackorder", value);
            }
        }
	      
        [XmlAttribute("BShipSeparate")]
        [Bindable(true)]
        public bool? BShipSeparate 
	    {
		    get
		    {
			    return GetColumnValue<bool?>("bShipSeparate");
		    }
            set 
		    {
			    SetColumnValue("bShipSeparate", value);
            }
        }
	      
        [XmlAttribute("DtStamp")]
        [Bindable(true)]
        public DateTime DtStamp 
	    {
		    get
		    {
			    return GetColumnValue<DateTime>("dtStamp");
		    }
            set 
		    {
			    SetColumnValue("dtStamp", value);
            }
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
            
            public static string DtShipCutoff = @"dtShipCutoff";
            
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
	    
	    
	    #region IAbstractRecord Members
        public new CT GetColumnValue<CT>(string columnName) {
            return base.GetColumnValue<CT>(columnName);
        }
        public object GetColumnValue(string columnName) {
            return base.GetColumnValue<object>(columnName);
        }
        #endregion
	    
    }
}
