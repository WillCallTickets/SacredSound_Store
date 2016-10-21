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
	/// Strongly-typed collection for the Cashew class.
	/// </summary>
    [Serializable]
	public partial class CashewCollection : ActiveList<Cashew, CashewCollection>
	{	   
		public CashewCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>CashewCollection</returns>
		public CashewCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                Cashew o = this[i];
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
	/// This is an ActiveRecord class which wraps the Cashew table.
	/// </summary>
	[Serializable]
	public partial class Cashew : ActiveRecord<Cashew>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public Cashew()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public Cashew(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public Cashew(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public Cashew(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("Cashew", TableType.Table, DataService.GetInstance("WillCall"));
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
				
				TableSchema.TableColumn colvarENumber = new TableSchema.TableColumn(schema);
				colvarENumber.ColumnName = "eNumber";
				colvarENumber.DataType = DbType.AnsiString;
				colvarENumber.MaxLength = 75;
				colvarENumber.AutoIncrement = false;
				colvarENumber.IsNullable = false;
				colvarENumber.IsPrimaryKey = false;
				colvarENumber.IsForeignKey = false;
				colvarENumber.IsReadOnly = false;
				colvarENumber.DefaultSetting = @"";
				colvarENumber.ForeignKeyTableName = "";
				schema.Columns.Add(colvarENumber);
				
				TableSchema.TableColumn colvarEMonth = new TableSchema.TableColumn(schema);
				colvarEMonth.ColumnName = "eMonth";
				colvarEMonth.DataType = DbType.AnsiString;
				colvarEMonth.MaxLength = 75;
				colvarEMonth.AutoIncrement = false;
				colvarEMonth.IsNullable = false;
				colvarEMonth.IsPrimaryKey = false;
				colvarEMonth.IsForeignKey = false;
				colvarEMonth.IsReadOnly = false;
				colvarEMonth.DefaultSetting = @"";
				colvarEMonth.ForeignKeyTableName = "";
				schema.Columns.Add(colvarEMonth);
				
				TableSchema.TableColumn colvarEYear = new TableSchema.TableColumn(schema);
				colvarEYear.ColumnName = "eYear";
				colvarEYear.DataType = DbType.AnsiString;
				colvarEYear.MaxLength = 75;
				colvarEYear.AutoIncrement = false;
				colvarEYear.IsNullable = false;
				colvarEYear.IsPrimaryKey = false;
				colvarEYear.IsForeignKey = false;
				colvarEYear.IsReadOnly = false;
				colvarEYear.DefaultSetting = @"";
				colvarEYear.ForeignKeyTableName = "";
				schema.Columns.Add(colvarEYear);
				
				TableSchema.TableColumn colvarEName = new TableSchema.TableColumn(schema);
				colvarEName.ColumnName = "eName";
				colvarEName.DataType = DbType.AnsiString;
				colvarEName.MaxLength = 75;
				colvarEName.AutoIncrement = false;
				colvarEName.IsNullable = false;
				colvarEName.IsPrimaryKey = false;
				colvarEName.IsForeignKey = false;
				colvarEName.IsReadOnly = false;
				colvarEName.DefaultSetting = @"";
				colvarEName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarEName);
				
				TableSchema.TableColumn colvarUserId = new TableSchema.TableColumn(schema);
				colvarUserId.ColumnName = "UserId";
				colvarUserId.DataType = DbType.Guid;
				colvarUserId.MaxLength = 0;
				colvarUserId.AutoIncrement = false;
				colvarUserId.IsNullable = false;
				colvarUserId.IsPrimaryKey = false;
				colvarUserId.IsForeignKey = true;
				colvarUserId.IsReadOnly = false;
				colvarUserId.DefaultSetting = @"";
				
					colvarUserId.ForeignKeyTableName = "aspnet_Users";
				schema.Columns.Add(colvarUserId);
				
				TableSchema.TableColumn colvarCustomerId = new TableSchema.TableColumn(schema);
				colvarCustomerId.ColumnName = "CustomerId";
				colvarCustomerId.DataType = DbType.Int32;
				colvarCustomerId.MaxLength = 0;
				colvarCustomerId.AutoIncrement = false;
				colvarCustomerId.IsNullable = false;
				colvarCustomerId.IsPrimaryKey = false;
				colvarCustomerId.IsForeignKey = false;
				colvarCustomerId.IsReadOnly = false;
				colvarCustomerId.DefaultSetting = @"";
				colvarCustomerId.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCustomerId);
				
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
				DataService.Providers["WillCall"].AddSchema("Cashew",schema);
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
		  
		[XmlAttribute("ENumber")]
		[Bindable(true)]
		public string ENumber 
		{
			get { return GetColumnValue<string>(Columns.ENumber); }
			set { SetColumnValue(Columns.ENumber, value); }
		}
		  
		[XmlAttribute("EMonth")]
		[Bindable(true)]
		public string EMonth 
		{
			get { return GetColumnValue<string>(Columns.EMonth); }
			set { SetColumnValue(Columns.EMonth, value); }
		}
		  
		[XmlAttribute("EYear")]
		[Bindable(true)]
		public string EYear 
		{
			get { return GetColumnValue<string>(Columns.EYear); }
			set { SetColumnValue(Columns.EYear, value); }
		}
		  
		[XmlAttribute("EName")]
		[Bindable(true)]
		public string EName 
		{
			get { return GetColumnValue<string>(Columns.EName); }
			set { SetColumnValue(Columns.EName, value); }
		}
		  
		[XmlAttribute("UserId")]
		[Bindable(true)]
		public Guid UserId 
		{
			get { return GetColumnValue<Guid>(Columns.UserId); }
			set { SetColumnValue(Columns.UserId, value); }
		}
		  
		[XmlAttribute("CustomerId")]
		[Bindable(true)]
		public int CustomerId 
		{
			get { return GetColumnValue<int>(Columns.CustomerId); }
			set { SetColumnValue(Columns.CustomerId, value); }
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
        
		
		private Wcss.InvoiceCollection colInvoiceRecords;
		public Wcss.InvoiceCollection InvoiceRecords()
		{
			if(colInvoiceRecords == null)
			{
				colInvoiceRecords = new Wcss.InvoiceCollection().Where(Invoice.Columns.TCashewId, Id).Load();
				colInvoiceRecords.ListChanged += new ListChangedEventHandler(colInvoiceRecords_ListChanged);
			}
			return colInvoiceRecords;
		}
				
		void colInvoiceRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colInvoiceRecords[e.NewIndex].TCashewId = Id;
				colInvoiceRecords.ListChanged += new ListChangedEventHandler(colInvoiceRecords_ListChanged);
            }
		}
		#endregion
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a AspnetUser ActiveRecord object related to this Cashew
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
		
		
		#endregion
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(string varENumber,string varEMonth,string varEYear,string varEName,Guid varUserId,int varCustomerId,DateTime varDtStamp)
		{
			Cashew item = new Cashew();
			
			item.ENumber = varENumber;
			
			item.EMonth = varEMonth;
			
			item.EYear = varEYear;
			
			item.EName = varEName;
			
			item.UserId = varUserId;
			
			item.CustomerId = varCustomerId;
			
			item.DtStamp = varDtStamp;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varId,string varENumber,string varEMonth,string varEYear,string varEName,Guid varUserId,int varCustomerId,DateTime varDtStamp)
		{
			Cashew item = new Cashew();
			
				item.Id = varId;
			
				item.ENumber = varENumber;
			
				item.EMonth = varEMonth;
			
				item.EYear = varEYear;
			
				item.EName = varEName;
			
				item.UserId = varUserId;
			
				item.CustomerId = varCustomerId;
			
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
        
        
        
        public static TableSchema.TableColumn ENumberColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn EMonthColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn EYearColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn ENameColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn UserIdColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn CustomerIdColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn DtStampColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Id = @"Id";
			 public static string ENumber = @"eNumber";
			 public static string EMonth = @"eMonth";
			 public static string EYear = @"eYear";
			 public static string EName = @"eName";
			 public static string UserId = @"UserId";
			 public static string CustomerId = @"CustomerId";
			 public static string DtStamp = @"dtStamp";
						
		}
		#endregion
		
		#region Update PK Collections
		
        public void SetPKValues()
        {
                if (colInvoiceRecords != null)
                {
                    foreach (Wcss.Invoice item in colInvoiceRecords)
                    {
                        if (item.TCashewId != Id)
                        {
                            item.TCashewId = Id;
                        }
                    }
               }
		}
        #endregion
    
        #region Deep Save
		
        public void DeepSave()
        {
            Save();
            
                if (colInvoiceRecords != null)
                {
                    colInvoiceRecords.SaveAll();
               }
		}
        #endregion
	}
}
