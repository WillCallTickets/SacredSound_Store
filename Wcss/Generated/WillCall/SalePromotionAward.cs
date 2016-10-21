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
	/// Strongly-typed collection for the SalePromotionAward class.
	/// </summary>
    [Serializable]
	public partial class SalePromotionAwardCollection : ActiveList<SalePromotionAward, SalePromotionAwardCollection>
	{	   
		public SalePromotionAwardCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>SalePromotionAwardCollection</returns>
		public SalePromotionAwardCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                SalePromotionAward o = this[i];
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
	/// This is an ActiveRecord class which wraps the SalePromotionAward table.
	/// </summary>
	[Serializable]
	public partial class SalePromotionAward : ActiveRecord<SalePromotionAward>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public SalePromotionAward()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public SalePromotionAward(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public SalePromotionAward(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public SalePromotionAward(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("SalePromotionAward", TableType.Table, DataService.GetInstance("WillCall"));
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
				
				TableSchema.TableColumn colvarTSalePromotionId = new TableSchema.TableColumn(schema);
				colvarTSalePromotionId.ColumnName = "TSalePromotionId";
				colvarTSalePromotionId.DataType = DbType.Int32;
				colvarTSalePromotionId.MaxLength = 0;
				colvarTSalePromotionId.AutoIncrement = false;
				colvarTSalePromotionId.IsNullable = false;
				colvarTSalePromotionId.IsPrimaryKey = false;
				colvarTSalePromotionId.IsForeignKey = true;
				colvarTSalePromotionId.IsReadOnly = false;
				colvarTSalePromotionId.DefaultSetting = @"";
				
					colvarTSalePromotionId.ForeignKeyTableName = "SalePromotion";
				schema.Columns.Add(colvarTSalePromotionId);
				
				TableSchema.TableColumn colvarTParentMerchId = new TableSchema.TableColumn(schema);
				colvarTParentMerchId.ColumnName = "TParentMerchId";
				colvarTParentMerchId.DataType = DbType.Int32;
				colvarTParentMerchId.MaxLength = 0;
				colvarTParentMerchId.AutoIncrement = false;
				colvarTParentMerchId.IsNullable = true;
				colvarTParentMerchId.IsPrimaryKey = false;
				colvarTParentMerchId.IsForeignKey = true;
				colvarTParentMerchId.IsReadOnly = false;
				colvarTParentMerchId.DefaultSetting = @"";
				
					colvarTParentMerchId.ForeignKeyTableName = "Merch";
				schema.Columns.Add(colvarTParentMerchId);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["WillCall"].AddSchema("SalePromotionAward",schema);
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
		  
		[XmlAttribute("BActive")]
		[Bindable(true)]
		public bool BActive 
		{
			get { return GetColumnValue<bool>(Columns.BActive); }
			set { SetColumnValue(Columns.BActive, value); }
		}
		  
		[XmlAttribute("TSalePromotionId")]
		[Bindable(true)]
		public int TSalePromotionId 
		{
			get { return GetColumnValue<int>(Columns.TSalePromotionId); }
			set { SetColumnValue(Columns.TSalePromotionId, value); }
		}
		  
		[XmlAttribute("TParentMerchId")]
		[Bindable(true)]
		public int? TParentMerchId 
		{
			get { return GetColumnValue<int?>(Columns.TParentMerchId); }
			set { SetColumnValue(Columns.TParentMerchId, value); }
		}
		
		#endregion
		
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a Merch ActiveRecord object related to this SalePromotionAward
		/// 
		/// </summary>
		private Wcss.Merch Merch
		{
			get { return Wcss.Merch.FetchByID(this.TParentMerchId); }
			set { SetColumnValue("TParentMerchId", value.Id); }
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
                
                SetColumnValue("TParentMerchId", value.Id);
                _merchrecord.CopyFrom(value);                
            }
		}
		
		
		/// <summary>
		/// Returns a SalePromotion ActiveRecord object related to this SalePromotionAward
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
		
		
		#endregion
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(DateTime varDtStamp,bool varBActive,int varTSalePromotionId,int? varTParentMerchId)
		{
			SalePromotionAward item = new SalePromotionAward();
			
			item.DtStamp = varDtStamp;
			
			item.BActive = varBActive;
			
			item.TSalePromotionId = varTSalePromotionId;
			
			item.TParentMerchId = varTParentMerchId;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varId,DateTime varDtStamp,bool varBActive,int varTSalePromotionId,int? varTParentMerchId)
		{
			SalePromotionAward item = new SalePromotionAward();
			
				item.Id = varId;
			
				item.DtStamp = varDtStamp;
			
				item.BActive = varBActive;
			
				item.TSalePromotionId = varTSalePromotionId;
			
				item.TParentMerchId = varTParentMerchId;
			
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
        
        
        
        public static TableSchema.TableColumn BActiveColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn TSalePromotionIdColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn TParentMerchIdColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Id = @"Id";
			 public static string DtStamp = @"dtStamp";
			 public static string BActive = @"bActive";
			 public static string TSalePromotionId = @"TSalePromotionId";
			 public static string TParentMerchId = @"TParentMerchId";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
