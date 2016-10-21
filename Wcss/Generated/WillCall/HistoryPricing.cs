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
	/// Strongly-typed collection for the HistoryPricing class.
	/// </summary>
    [Serializable]
	public partial class HistoryPricingCollection : ActiveList<HistoryPricing, HistoryPricingCollection>
	{	   
		public HistoryPricingCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>HistoryPricingCollection</returns>
		public HistoryPricingCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                HistoryPricing o = this[i];
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
	/// This is an ActiveRecord class which wraps the HistoryPricing table.
	/// </summary>
	[Serializable]
	public partial class HistoryPricing : ActiveRecord<HistoryPricing>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public HistoryPricing()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public HistoryPricing(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public HistoryPricing(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public HistoryPricing(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("HistoryPricing", TableType.Table, DataService.GetInstance("WillCall"));
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
				
				TableSchema.TableColumn colvarUserId = new TableSchema.TableColumn(schema);
				colvarUserId.ColumnName = "UserId";
				colvarUserId.DataType = DbType.Guid;
				colvarUserId.MaxLength = 0;
				colvarUserId.AutoIncrement = false;
				colvarUserId.IsNullable = true;
				colvarUserId.IsPrimaryKey = false;
				colvarUserId.IsForeignKey = true;
				colvarUserId.IsReadOnly = false;
				colvarUserId.DefaultSetting = @"";
				
					colvarUserId.ForeignKeyTableName = "aspnet_Users";
				schema.Columns.Add(colvarUserId);
				
				TableSchema.TableColumn colvarTMerchId = new TableSchema.TableColumn(schema);
				colvarTMerchId.ColumnName = "tMerchId";
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
				
				TableSchema.TableColumn colvarDtAdjusted = new TableSchema.TableColumn(schema);
				colvarDtAdjusted.ColumnName = "dtAdjusted";
				colvarDtAdjusted.DataType = DbType.DateTime;
				colvarDtAdjusted.MaxLength = 0;
				colvarDtAdjusted.AutoIncrement = false;
				colvarDtAdjusted.IsNullable = false;
				colvarDtAdjusted.IsPrimaryKey = false;
				colvarDtAdjusted.IsForeignKey = false;
				colvarDtAdjusted.IsReadOnly = false;
				colvarDtAdjusted.DefaultSetting = @"";
				colvarDtAdjusted.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDtAdjusted);
				
				TableSchema.TableColumn colvarMOldPrice = new TableSchema.TableColumn(schema);
				colvarMOldPrice.ColumnName = "mOldPrice";
				colvarMOldPrice.DataType = DbType.Currency;
				colvarMOldPrice.MaxLength = 0;
				colvarMOldPrice.AutoIncrement = false;
				colvarMOldPrice.IsNullable = false;
				colvarMOldPrice.IsPrimaryKey = false;
				colvarMOldPrice.IsForeignKey = false;
				colvarMOldPrice.IsReadOnly = false;
				
						colvarMOldPrice.DefaultSetting = @"((0))";
				colvarMOldPrice.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMOldPrice);
				
				TableSchema.TableColumn colvarMNewPrice = new TableSchema.TableColumn(schema);
				colvarMNewPrice.ColumnName = "mNewPrice";
				colvarMNewPrice.DataType = DbType.Currency;
				colvarMNewPrice.MaxLength = 0;
				colvarMNewPrice.AutoIncrement = false;
				colvarMNewPrice.IsNullable = false;
				colvarMNewPrice.IsPrimaryKey = false;
				colvarMNewPrice.IsForeignKey = false;
				colvarMNewPrice.IsReadOnly = false;
				
						colvarMNewPrice.DefaultSetting = @"((0))";
				colvarMNewPrice.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMNewPrice);
				
				TableSchema.TableColumn colvarVcContext = new TableSchema.TableColumn(schema);
				colvarVcContext.ColumnName = "vcContext";
				colvarVcContext.DataType = DbType.AnsiString;
				colvarVcContext.MaxLength = 50;
				colvarVcContext.AutoIncrement = false;
				colvarVcContext.IsNullable = false;
				colvarVcContext.IsPrimaryKey = false;
				colvarVcContext.IsForeignKey = false;
				colvarVcContext.IsReadOnly = false;
				colvarVcContext.DefaultSetting = @"";
				colvarVcContext.ForeignKeyTableName = "";
				schema.Columns.Add(colvarVcContext);
				
				TableSchema.TableColumn colvarDescription = new TableSchema.TableColumn(schema);
				colvarDescription.ColumnName = "Description";
				colvarDescription.DataType = DbType.AnsiString;
				colvarDescription.MaxLength = 500;
				colvarDescription.AutoIncrement = false;
				colvarDescription.IsNullable = true;
				colvarDescription.IsPrimaryKey = false;
				colvarDescription.IsForeignKey = false;
				colvarDescription.IsReadOnly = false;
				colvarDescription.DefaultSetting = @"";
				colvarDescription.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDescription);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["WillCall"].AddSchema("HistoryPricing",schema);
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
		  
		[XmlAttribute("UserId")]
		[Bindable(true)]
		public Guid? UserId 
		{
			get { return GetColumnValue<Guid?>(Columns.UserId); }
			set { SetColumnValue(Columns.UserId, value); }
		}
		  
		[XmlAttribute("TMerchId")]
		[Bindable(true)]
		public int? TMerchId 
		{
			get { return GetColumnValue<int?>(Columns.TMerchId); }
			set { SetColumnValue(Columns.TMerchId, value); }
		}
		  
		[XmlAttribute("TShowTicketId")]
		[Bindable(true)]
		public int? TShowTicketId 
		{
			get { return GetColumnValue<int?>(Columns.TShowTicketId); }
			set { SetColumnValue(Columns.TShowTicketId, value); }
		}
		  
		[XmlAttribute("DtAdjusted")]
		[Bindable(true)]
		public DateTime DtAdjusted 
		{
			get { return GetColumnValue<DateTime>(Columns.DtAdjusted); }
			set { SetColumnValue(Columns.DtAdjusted, value); }
		}
		  
		[XmlAttribute("MOldPrice")]
		[Bindable(true)]
		public decimal MOldPrice 
		{
			get { return GetColumnValue<decimal>(Columns.MOldPrice); }
			set { SetColumnValue(Columns.MOldPrice, value); }
		}
		  
		[XmlAttribute("MNewPrice")]
		[Bindable(true)]
		public decimal MNewPrice 
		{
			get { return GetColumnValue<decimal>(Columns.MNewPrice); }
			set { SetColumnValue(Columns.MNewPrice, value); }
		}
		  
		[XmlAttribute("VcContext")]
		[Bindable(true)]
		public string VcContext 
		{
			get { return GetColumnValue<string>(Columns.VcContext); }
			set { SetColumnValue(Columns.VcContext, value); }
		}
		  
		[XmlAttribute("Description")]
		[Bindable(true)]
		public string Description 
		{
			get { return GetColumnValue<string>(Columns.Description); }
			set { SetColumnValue(Columns.Description, value); }
		}
		
		#endregion
		
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a AspnetUser ActiveRecord object related to this HistoryPricing
		/// 
		/// </summary>
		private Wcss.AspnetUser AspnetUser
		{
			get { return Wcss.AspnetUser.FetchByID(this.UserId); }
			set { SetColumnValue("UserId", value.UserId); }
		}
        //set up an alternate mechanism to avoid a database call
		private Wcss.AspnetUser _aspnetuserrecord = null;
		
		public Wcss.AspnetUser AspnetUserRecord
		{
		    get
            {
                if (_aspnetuserrecord == null)
                {
                    _aspnetuserrecord = new Wcss.AspnetUser();
                    _aspnetuserrecord.CopyFrom(this.AspnetUser);
                }
                return _aspnetuserrecord;
            }
            set
            {
                if(value != null && _aspnetuserrecord == null)
			        _aspnetuserrecord = new Wcss.AspnetUser();
                
                SetColumnValue("UserId", value.UserId);
                _aspnetuserrecord.CopyFrom(value);                
            }
		}
		
		
		/// <summary>
		/// Returns a Merch ActiveRecord object related to this HistoryPricing
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
		/// Returns a ShowTicket ActiveRecord object related to this HistoryPricing
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
		
		
		#endregion
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(DateTime varDtStamp,Guid? varUserId,int? varTMerchId,int? varTShowTicketId,DateTime varDtAdjusted,decimal varMOldPrice,decimal varMNewPrice,string varVcContext,string varDescription)
		{
			HistoryPricing item = new HistoryPricing();
			
			item.DtStamp = varDtStamp;
			
			item.UserId = varUserId;
			
			item.TMerchId = varTMerchId;
			
			item.TShowTicketId = varTShowTicketId;
			
			item.DtAdjusted = varDtAdjusted;
			
			item.MOldPrice = varMOldPrice;
			
			item.MNewPrice = varMNewPrice;
			
			item.VcContext = varVcContext;
			
			item.Description = varDescription;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varId,DateTime varDtStamp,Guid? varUserId,int? varTMerchId,int? varTShowTicketId,DateTime varDtAdjusted,decimal varMOldPrice,decimal varMNewPrice,string varVcContext,string varDescription)
		{
			HistoryPricing item = new HistoryPricing();
			
				item.Id = varId;
			
				item.DtStamp = varDtStamp;
			
				item.UserId = varUserId;
			
				item.TMerchId = varTMerchId;
			
				item.TShowTicketId = varTShowTicketId;
			
				item.DtAdjusted = varDtAdjusted;
			
				item.MOldPrice = varMOldPrice;
			
				item.MNewPrice = varMNewPrice;
			
				item.VcContext = varVcContext;
			
				item.Description = varDescription;
			
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
        
        
        
        public static TableSchema.TableColumn UserIdColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn TMerchIdColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn TShowTicketIdColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn DtAdjustedColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn MOldPriceColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn MNewPriceColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn VcContextColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn DescriptionColumn
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Id = @"Id";
			 public static string DtStamp = @"dtStamp";
			 public static string UserId = @"UserId";
			 public static string TMerchId = @"tMerchId";
			 public static string TShowTicketId = @"tShowTicketId";
			 public static string DtAdjusted = @"dtAdjusted";
			 public static string MOldPrice = @"mOldPrice";
			 public static string MNewPrice = @"mNewPrice";
			 public static string VcContext = @"vcContext";
			 public static string Description = @"Description";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
