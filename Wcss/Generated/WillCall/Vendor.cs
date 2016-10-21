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
	/// Strongly-typed collection for the Vendor class.
	/// </summary>
    [Serializable]
	public partial class VendorCollection : ActiveList<Vendor, VendorCollection>
	{	   
		public VendorCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>VendorCollection</returns>
		public VendorCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                Vendor o = this[i];
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
	/// This is an ActiveRecord class which wraps the Vendor table.
	/// </summary>
	[Serializable]
	public partial class Vendor : ActiveRecord<Vendor>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public Vendor()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public Vendor(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public Vendor(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public Vendor(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("Vendor", TableType.Table, DataService.GetInstance("WillCall"));
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
				
				TableSchema.TableColumn colvarDisplayName = new TableSchema.TableColumn(schema);
				colvarDisplayName.ColumnName = "DisplayName";
				colvarDisplayName.DataType = DbType.AnsiString;
				colvarDisplayName.MaxLength = 256;
				colvarDisplayName.AutoIncrement = false;
				colvarDisplayName.IsNullable = true;
				colvarDisplayName.IsPrimaryKey = false;
				colvarDisplayName.IsForeignKey = false;
				colvarDisplayName.IsReadOnly = false;
				colvarDisplayName.DefaultSetting = @"";
				colvarDisplayName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDisplayName);
				
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
				DataService.Providers["WillCall"].AddSchema("Vendor",schema);
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
		  
		[XmlAttribute("Name")]
		[Bindable(true)]
		public string Name 
		{
			get { return GetColumnValue<string>(Columns.Name); }
			set { SetColumnValue(Columns.Name, value); }
		}
		  
		[XmlAttribute("DisplayName")]
		[Bindable(true)]
		public string DisplayName 
		{
			get { return GetColumnValue<string>(Columns.DisplayName); }
			set { SetColumnValue(Columns.DisplayName, value); }
		}
		  
		[XmlAttribute("Description")]
		[Bindable(true)]
		public string Description 
		{
			get { return GetColumnValue<string>(Columns.Description); }
			set { SetColumnValue(Columns.Description, value); }
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
        
		
		private Wcss.InvoiceCollection colInvoiceRecords;
		public Wcss.InvoiceCollection InvoiceRecords()
		{
			if(colInvoiceRecords == null)
			{
				colInvoiceRecords = new Wcss.InvoiceCollection().Where(Invoice.Columns.TVendorId, Id).Load();
				colInvoiceRecords.ListChanged += new ListChangedEventHandler(colInvoiceRecords_ListChanged);
			}
			return colInvoiceRecords;
		}
				
		void colInvoiceRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colInvoiceRecords[e.NewIndex].TVendorId = Id;
				colInvoiceRecords.ListChanged += new ListChangedEventHandler(colInvoiceRecords_ListChanged);
            }
		}
		#endregion
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a AspnetApplication ActiveRecord object related to this Vendor
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
		public static void Insert(DateTime varDtStamp,string varName,string varDisplayName,string varDescription,Guid varApplicationId)
		{
			Vendor item = new Vendor();
			
			item.DtStamp = varDtStamp;
			
			item.Name = varName;
			
			item.DisplayName = varDisplayName;
			
			item.Description = varDescription;
			
			item.ApplicationId = varApplicationId;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varId,DateTime varDtStamp,string varName,string varDisplayName,string varDescription,Guid varApplicationId)
		{
			Vendor item = new Vendor();
			
				item.Id = varId;
			
				item.DtStamp = varDtStamp;
			
				item.Name = varName;
			
				item.DisplayName = varDisplayName;
			
				item.Description = varDescription;
			
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
        
        
        
        public static TableSchema.TableColumn DtStampColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn NameColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn DisplayNameColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn DescriptionColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn ApplicationIdColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Id = @"Id";
			 public static string DtStamp = @"dtStamp";
			 public static string Name = @"Name";
			 public static string DisplayName = @"DisplayName";
			 public static string Description = @"Description";
			 public static string ApplicationId = @"ApplicationId";
						
		}
		#endregion
		
		#region Update PK Collections
		
        public void SetPKValues()
        {
                if (colInvoiceRecords != null)
                {
                    foreach (Wcss.Invoice item in colInvoiceRecords)
                    {
                        if (item.TVendorId != Id)
                        {
                            item.TVendorId = Id;
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
