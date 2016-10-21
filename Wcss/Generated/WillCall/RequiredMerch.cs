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
	/// Strongly-typed collection for the RequiredMerch class.
	/// </summary>
    [Serializable]
	public partial class RequiredMerchCollection : ActiveList<RequiredMerch, RequiredMerchCollection>
	{	   
		public RequiredMerchCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>RequiredMerchCollection</returns>
		public RequiredMerchCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                RequiredMerch o = this[i];
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
	/// This is an ActiveRecord class which wraps the Required_Merch table.
	/// </summary>
	[Serializable]
	public partial class RequiredMerch : ActiveRecord<RequiredMerch>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public RequiredMerch()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public RequiredMerch(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public RequiredMerch(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public RequiredMerch(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("Required_Merch", TableType.Table, DataService.GetInstance("WillCall"));
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
				
				TableSchema.TableColumn colvarTMerchId = new TableSchema.TableColumn(schema);
				colvarTMerchId.ColumnName = "tMerchId";
				colvarTMerchId.DataType = DbType.Int32;
				colvarTMerchId.MaxLength = 0;
				colvarTMerchId.AutoIncrement = false;
				colvarTMerchId.IsNullable = false;
				colvarTMerchId.IsPrimaryKey = false;
				colvarTMerchId.IsForeignKey = true;
				colvarTMerchId.IsReadOnly = false;
				colvarTMerchId.DefaultSetting = @"";
				
					colvarTMerchId.ForeignKeyTableName = "Merch";
				schema.Columns.Add(colvarTMerchId);
				
				TableSchema.TableColumn colvarTRequiredId = new TableSchema.TableColumn(schema);
				colvarTRequiredId.ColumnName = "tRequiredId";
				colvarTRequiredId.DataType = DbType.Int32;
				colvarTRequiredId.MaxLength = 0;
				colvarTRequiredId.AutoIncrement = false;
				colvarTRequiredId.IsNullable = false;
				colvarTRequiredId.IsPrimaryKey = false;
				colvarTRequiredId.IsForeignKey = true;
				colvarTRequiredId.IsReadOnly = false;
				colvarTRequiredId.DefaultSetting = @"";
				
					colvarTRequiredId.ForeignKeyTableName = "Required";
				schema.Columns.Add(colvarTRequiredId);
				
				TableSchema.TableColumn colvarBLimitQtyToPastQty = new TableSchema.TableColumn(schema);
				colvarBLimitQtyToPastQty.ColumnName = "bLimitQtyToPastQty";
				colvarBLimitQtyToPastQty.DataType = DbType.Boolean;
				colvarBLimitQtyToPastQty.MaxLength = 0;
				colvarBLimitQtyToPastQty.AutoIncrement = false;
				colvarBLimitQtyToPastQty.IsNullable = false;
				colvarBLimitQtyToPastQty.IsPrimaryKey = false;
				colvarBLimitQtyToPastQty.IsForeignKey = false;
				colvarBLimitQtyToPastQty.IsReadOnly = false;
				
						colvarBLimitQtyToPastQty.DefaultSetting = @"((0))";
				colvarBLimitQtyToPastQty.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBLimitQtyToPastQty);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["WillCall"].AddSchema("Required_Merch",schema);
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
		  
		[XmlAttribute("TMerchId")]
		[Bindable(true)]
		public int TMerchId 
		{
			get { return GetColumnValue<int>(Columns.TMerchId); }
			set { SetColumnValue(Columns.TMerchId, value); }
		}
		  
		[XmlAttribute("TRequiredId")]
		[Bindable(true)]
		public int TRequiredId 
		{
			get { return GetColumnValue<int>(Columns.TRequiredId); }
			set { SetColumnValue(Columns.TRequiredId, value); }
		}
		  
		[XmlAttribute("BLimitQtyToPastQty")]
		[Bindable(true)]
		public bool BLimitQtyToPastQty 
		{
			get { return GetColumnValue<bool>(Columns.BLimitQtyToPastQty); }
			set { SetColumnValue(Columns.BLimitQtyToPastQty, value); }
		}
		
		#endregion
		
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a Merch ActiveRecord object related to this RequiredMerch
		/// 
		/// </summary>
		private Wcss.Merch Merch
		{
			get { return Wcss.Merch.FetchByID(this.TMerchId); }
			set { SetColumnValue("tMerchId", value.Id); }
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
                
                SetColumnValue("tMerchId", value.Id);
                _merchrecord.CopyFrom(value);                
            }
		}
		
		
		/// <summary>
		/// Returns a Required ActiveRecord object related to this RequiredMerch
		/// 
		/// </summary>
		private Wcss.Required Required
		{
			get { return Wcss.Required.FetchByID(this.TRequiredId); }
			set { SetColumnValue("tRequiredId", value.Id); }
		}
        //set up an alternate mechanism to avoid a database call
		private Wcss.Required _requiredrecord = null;
		
		public Wcss.Required RequiredRecord
		{
		    get
            {
                if (_requiredrecord == null)
                {
                    _requiredrecord = new Wcss.Required();
                    _requiredrecord.CopyFrom(this.Required);
                }
                return _requiredrecord;
            }
            set
            {
                if(value != null && _requiredrecord == null)
			        _requiredrecord = new Wcss.Required();
                
                SetColumnValue("tRequiredId", value.Id);
                _requiredrecord.CopyFrom(value);                
            }
		}
		
		
		#endregion
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(DateTime varDtStamp,int varTMerchId,int varTRequiredId,bool varBLimitQtyToPastQty)
		{
			RequiredMerch item = new RequiredMerch();
			
			item.DtStamp = varDtStamp;
			
			item.TMerchId = varTMerchId;
			
			item.TRequiredId = varTRequiredId;
			
			item.BLimitQtyToPastQty = varBLimitQtyToPastQty;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varId,DateTime varDtStamp,int varTMerchId,int varTRequiredId,bool varBLimitQtyToPastQty)
		{
			RequiredMerch item = new RequiredMerch();
			
				item.Id = varId;
			
				item.DtStamp = varDtStamp;
			
				item.TMerchId = varTMerchId;
			
				item.TRequiredId = varTRequiredId;
			
				item.BLimitQtyToPastQty = varBLimitQtyToPastQty;
			
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
        
        
        
        public static TableSchema.TableColumn TMerchIdColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn TRequiredIdColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn BLimitQtyToPastQtyColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Id = @"Id";
			 public static string DtStamp = @"dtStamp";
			 public static string TMerchId = @"tMerchId";
			 public static string TRequiredId = @"tRequiredId";
			 public static string BLimitQtyToPastQty = @"bLimitQtyToPastQty";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
