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
	/// Strongly-typed collection for the ChargeHourly class.
	/// </summary>
    [Serializable]
	public partial class ChargeHourlyCollection : ActiveList<ChargeHourly, ChargeHourlyCollection>
	{	   
		public ChargeHourlyCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>ChargeHourlyCollection</returns>
		public ChargeHourlyCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                ChargeHourly o = this[i];
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
	/// This is an ActiveRecord class which wraps the Charge_Hourly table.
	/// </summary>
	[Serializable]
	public partial class ChargeHourly : ActiveRecord<ChargeHourly>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public ChargeHourly()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public ChargeHourly(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public ChargeHourly(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public ChargeHourly(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("Charge_Hourly", TableType.Table, DataService.GetInstance("WillCall"));
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
				
				TableSchema.TableColumn colvarTChargeStatementId = new TableSchema.TableColumn(schema);
				colvarTChargeStatementId.ColumnName = "TChargeStatementId";
				colvarTChargeStatementId.DataType = DbType.Int32;
				colvarTChargeStatementId.MaxLength = 0;
				colvarTChargeStatementId.AutoIncrement = false;
				colvarTChargeStatementId.IsNullable = false;
				colvarTChargeStatementId.IsPrimaryKey = false;
				colvarTChargeStatementId.IsForeignKey = true;
				colvarTChargeStatementId.IsReadOnly = false;
				colvarTChargeStatementId.DefaultSetting = @"";
				
					colvarTChargeStatementId.ForeignKeyTableName = "Charge_Statement";
				schema.Columns.Add(colvarTChargeStatementId);
				
				TableSchema.TableColumn colvarDtPerformed = new TableSchema.TableColumn(schema);
				colvarDtPerformed.ColumnName = "dtPerformed";
				colvarDtPerformed.DataType = DbType.DateTime;
				colvarDtPerformed.MaxLength = 0;
				colvarDtPerformed.AutoIncrement = false;
				colvarDtPerformed.IsNullable = false;
				colvarDtPerformed.IsPrimaryKey = false;
				colvarDtPerformed.IsForeignKey = false;
				colvarDtPerformed.IsReadOnly = false;
				colvarDtPerformed.DefaultSetting = @"";
				colvarDtPerformed.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDtPerformed);
				
				TableSchema.TableColumn colvarServiceDescription = new TableSchema.TableColumn(schema);
				colvarServiceDescription.ColumnName = "ServiceDescription";
				colvarServiceDescription.DataType = DbType.AnsiString;
				colvarServiceDescription.MaxLength = 2000;
				colvarServiceDescription.AutoIncrement = false;
				colvarServiceDescription.IsNullable = false;
				colvarServiceDescription.IsPrimaryKey = false;
				colvarServiceDescription.IsForeignKey = false;
				colvarServiceDescription.IsReadOnly = false;
				colvarServiceDescription.DefaultSetting = @"";
				colvarServiceDescription.ForeignKeyTableName = "";
				schema.Columns.Add(colvarServiceDescription);
				
				TableSchema.TableColumn colvarHours = new TableSchema.TableColumn(schema);
				colvarHours.ColumnName = "Hours";
				colvarHours.DataType = DbType.Int32;
				colvarHours.MaxLength = 0;
				colvarHours.AutoIncrement = false;
				colvarHours.IsNullable = false;
				colvarHours.IsPrimaryKey = false;
				colvarHours.IsForeignKey = false;
				colvarHours.IsReadOnly = false;
				colvarHours.DefaultSetting = @"";
				colvarHours.ForeignKeyTableName = "";
				schema.Columns.Add(colvarHours);
				
				TableSchema.TableColumn colvarRate = new TableSchema.TableColumn(schema);
				colvarRate.ColumnName = "Rate";
				colvarRate.DataType = DbType.Currency;
				colvarRate.MaxLength = 0;
				colvarRate.AutoIncrement = false;
				colvarRate.IsNullable = false;
				colvarRate.IsPrimaryKey = false;
				colvarRate.IsForeignKey = false;
				colvarRate.IsReadOnly = false;
				
						colvarRate.DefaultSetting = @"((0))";
				colvarRate.ForeignKeyTableName = "";
				schema.Columns.Add(colvarRate);
				
				TableSchema.TableColumn colvarFlatRate = new TableSchema.TableColumn(schema);
				colvarFlatRate.ColumnName = "FlatRate";
				colvarFlatRate.DataType = DbType.Currency;
				colvarFlatRate.MaxLength = 0;
				colvarFlatRate.AutoIncrement = false;
				colvarFlatRate.IsNullable = false;
				colvarFlatRate.IsPrimaryKey = false;
				colvarFlatRate.IsForeignKey = false;
				colvarFlatRate.IsReadOnly = false;
				
						colvarFlatRate.DefaultSetting = @"((0))";
				colvarFlatRate.ForeignKeyTableName = "";
				schema.Columns.Add(colvarFlatRate);
				
				TableSchema.TableColumn colvarLineTotal = new TableSchema.TableColumn(schema);
				colvarLineTotal.ColumnName = "LineTotal";
				colvarLineTotal.DataType = DbType.Currency;
				colvarLineTotal.MaxLength = 0;
				colvarLineTotal.AutoIncrement = false;
				colvarLineTotal.IsNullable = true;
				colvarLineTotal.IsPrimaryKey = false;
				colvarLineTotal.IsForeignKey = false;
				colvarLineTotal.IsReadOnly = true;
				colvarLineTotal.DefaultSetting = @"";
				colvarLineTotal.ForeignKeyTableName = "";
				schema.Columns.Add(colvarLineTotal);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["WillCall"].AddSchema("Charge_Hourly",schema);
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
		  
		[XmlAttribute("TChargeStatementId")]
		[Bindable(true)]
		public int TChargeStatementId 
		{
			get { return GetColumnValue<int>(Columns.TChargeStatementId); }
			set { SetColumnValue(Columns.TChargeStatementId, value); }
		}
		  
		[XmlAttribute("DtPerformed")]
		[Bindable(true)]
		public DateTime DtPerformed 
		{
			get { return GetColumnValue<DateTime>(Columns.DtPerformed); }
			set { SetColumnValue(Columns.DtPerformed, value); }
		}
		  
		[XmlAttribute("ServiceDescription")]
		[Bindable(true)]
		public string ServiceDescription 
		{
			get { return GetColumnValue<string>(Columns.ServiceDescription); }
			set { SetColumnValue(Columns.ServiceDescription, value); }
		}
		  
		[XmlAttribute("Hours")]
		[Bindable(true)]
		public int Hours 
		{
			get { return GetColumnValue<int>(Columns.Hours); }
			set { SetColumnValue(Columns.Hours, value); }
		}
		  
		[XmlAttribute("Rate")]
		[Bindable(true)]
		public decimal Rate 
		{
			get { return GetColumnValue<decimal>(Columns.Rate); }
			set { SetColumnValue(Columns.Rate, value); }
		}
		  
		[XmlAttribute("FlatRate")]
		[Bindable(true)]
		public decimal FlatRate 
		{
			get { return GetColumnValue<decimal>(Columns.FlatRate); }
			set { SetColumnValue(Columns.FlatRate, value); }
		}
		  
		[XmlAttribute("LineTotal")]
		[Bindable(true)]
		public decimal? LineTotal 
		{
			get { return GetColumnValue<decimal?>(Columns.LineTotal); }
			set { SetColumnValue(Columns.LineTotal, value); }
		}
		
		#endregion
		
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a ChargeStatement ActiveRecord object related to this ChargeHourly
		/// 
		/// </summary>
		private Wcss.ChargeStatement ChargeStatement
		{
			get { return Wcss.ChargeStatement.FetchByID(this.TChargeStatementId); }
			set { SetColumnValue("TChargeStatementId", value.Id); }
		}
        //set up an alternate mechanism to avoid a database call
		private Wcss.ChargeStatement _chargestatementrecord = null;
		
		public Wcss.ChargeStatement ChargeStatementRecord
		{
		    get
            {
                if (_chargestatementrecord == null)
                {
                    _chargestatementrecord = new Wcss.ChargeStatement();
                    _chargestatementrecord.CopyFrom(this.ChargeStatement);
                }
                return _chargestatementrecord;
            }
            set
            {
                if(value != null && _chargestatementrecord == null)
			        _chargestatementrecord = new Wcss.ChargeStatement();
                
                SetColumnValue("TChargeStatementId", value.Id);
                _chargestatementrecord.CopyFrom(value);                
            }
		}
		
		
		#endregion
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(DateTime varDtStamp,int varTChargeStatementId,DateTime varDtPerformed,string varServiceDescription,int varHours,decimal varRate,decimal varFlatRate,decimal? varLineTotal)
		{
			ChargeHourly item = new ChargeHourly();
			
			item.DtStamp = varDtStamp;
			
			item.TChargeStatementId = varTChargeStatementId;
			
			item.DtPerformed = varDtPerformed;
			
			item.ServiceDescription = varServiceDescription;
			
			item.Hours = varHours;
			
			item.Rate = varRate;
			
			item.FlatRate = varFlatRate;
			
			item.LineTotal = varLineTotal;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varId,DateTime varDtStamp,int varTChargeStatementId,DateTime varDtPerformed,string varServiceDescription,int varHours,decimal varRate,decimal varFlatRate,decimal? varLineTotal)
		{
			ChargeHourly item = new ChargeHourly();
			
				item.Id = varId;
			
				item.DtStamp = varDtStamp;
			
				item.TChargeStatementId = varTChargeStatementId;
			
				item.DtPerformed = varDtPerformed;
			
				item.ServiceDescription = varServiceDescription;
			
				item.Hours = varHours;
			
				item.Rate = varRate;
			
				item.FlatRate = varFlatRate;
			
				item.LineTotal = varLineTotal;
			
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
        
        
        
        public static TableSchema.TableColumn TChargeStatementIdColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn DtPerformedColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn ServiceDescriptionColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn HoursColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn RateColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn FlatRateColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn LineTotalColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Id = @"Id";
			 public static string DtStamp = @"dtStamp";
			 public static string TChargeStatementId = @"TChargeStatementId";
			 public static string DtPerformed = @"dtPerformed";
			 public static string ServiceDescription = @"ServiceDescription";
			 public static string Hours = @"Hours";
			 public static string Rate = @"Rate";
			 public static string FlatRate = @"FlatRate";
			 public static string LineTotal = @"LineTotal";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
