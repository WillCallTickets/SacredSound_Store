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
	/// Strongly-typed collection for the CharitableOrg class.
	/// </summary>
    [Serializable]
	public partial class CharitableOrgCollection : ActiveList<CharitableOrg, CharitableOrgCollection>
	{	   
		public CharitableOrgCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>CharitableOrgCollection</returns>
		public CharitableOrgCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                CharitableOrg o = this[i];
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
	/// This is an ActiveRecord class which wraps the CharitableOrg table.
	/// </summary>
	[Serializable]
	public partial class CharitableOrg : ActiveRecord<CharitableOrg>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public CharitableOrg()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public CharitableOrg(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public CharitableOrg(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public CharitableOrg(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("CharitableOrg", TableType.Table, DataService.GetInstance("WillCall"));
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
				
						colvarBActive.DefaultSetting = @"((0))";
				colvarBActive.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBActive);
				
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
				
				TableSchema.TableColumn colvarNameRoot = new TableSchema.TableColumn(schema);
				colvarNameRoot.ColumnName = "NameRoot";
				colvarNameRoot.DataType = DbType.AnsiString;
				colvarNameRoot.MaxLength = 256;
				colvarNameRoot.AutoIncrement = false;
				colvarNameRoot.IsNullable = true;
				colvarNameRoot.IsPrimaryKey = false;
				colvarNameRoot.IsForeignKey = false;
				colvarNameRoot.IsReadOnly = true;
				colvarNameRoot.DefaultSetting = @"";
				colvarNameRoot.ForeignKeyTableName = "";
				schema.Columns.Add(colvarNameRoot);
				
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
				
				TableSchema.TableColumn colvarWebsiteUrl = new TableSchema.TableColumn(schema);
				colvarWebsiteUrl.ColumnName = "WebsiteUrl";
				colvarWebsiteUrl.DataType = DbType.AnsiString;
				colvarWebsiteUrl.MaxLength = 256;
				colvarWebsiteUrl.AutoIncrement = false;
				colvarWebsiteUrl.IsNullable = true;
				colvarWebsiteUrl.IsPrimaryKey = false;
				colvarWebsiteUrl.IsForeignKey = false;
				colvarWebsiteUrl.IsReadOnly = false;
				colvarWebsiteUrl.DefaultSetting = @"";
				colvarWebsiteUrl.ForeignKeyTableName = "";
				schema.Columns.Add(colvarWebsiteUrl);
				
				TableSchema.TableColumn colvarPictureUrl = new TableSchema.TableColumn(schema);
				colvarPictureUrl.ColumnName = "PictureUrl";
				colvarPictureUrl.DataType = DbType.AnsiString;
				colvarPictureUrl.MaxLength = 256;
				colvarPictureUrl.AutoIncrement = false;
				colvarPictureUrl.IsNullable = true;
				colvarPictureUrl.IsPrimaryKey = false;
				colvarPictureUrl.IsForeignKey = false;
				colvarPictureUrl.IsReadOnly = false;
				colvarPictureUrl.DefaultSetting = @"";
				colvarPictureUrl.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPictureUrl);
				
				TableSchema.TableColumn colvarShortDescription = new TableSchema.TableColumn(schema);
				colvarShortDescription.ColumnName = "ShortDescription";
				colvarShortDescription.DataType = DbType.AnsiString;
				colvarShortDescription.MaxLength = 500;
				colvarShortDescription.AutoIncrement = false;
				colvarShortDescription.IsNullable = true;
				colvarShortDescription.IsPrimaryKey = false;
				colvarShortDescription.IsForeignKey = false;
				colvarShortDescription.IsReadOnly = false;
				colvarShortDescription.DefaultSetting = @"";
				colvarShortDescription.ForeignKeyTableName = "";
				schema.Columns.Add(colvarShortDescription);
				
				TableSchema.TableColumn colvarDescription = new TableSchema.TableColumn(schema);
				colvarDescription.ColumnName = "Description";
				colvarDescription.DataType = DbType.AnsiString;
				colvarDescription.MaxLength = -1;
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
				DataService.Providers["WillCall"].AddSchema("CharitableOrg",schema);
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
		  
		[XmlAttribute("Name")]
		[Bindable(true)]
		public string Name 
		{
			get { return GetColumnValue<string>(Columns.Name); }
			set { SetColumnValue(Columns.Name, value); }
		}
		  
		[XmlAttribute("NameRoot")]
		[Bindable(true)]
		public string NameRoot 
		{
			get { return GetColumnValue<string>(Columns.NameRoot); }
			set { SetColumnValue(Columns.NameRoot, value); }
		}
		  
		[XmlAttribute("DisplayName")]
		[Bindable(true)]
		public string DisplayName 
		{
			get { return GetColumnValue<string>(Columns.DisplayName); }
			set { SetColumnValue(Columns.DisplayName, value); }
		}
		  
		[XmlAttribute("WebsiteUrl")]
		[Bindable(true)]
		public string WebsiteUrl 
		{
			get { return GetColumnValue<string>(Columns.WebsiteUrl); }
			set { SetColumnValue(Columns.WebsiteUrl, value); }
		}
		  
		[XmlAttribute("PictureUrl")]
		[Bindable(true)]
		public string PictureUrl 
		{
			get { return GetColumnValue<string>(Columns.PictureUrl); }
			set { SetColumnValue(Columns.PictureUrl, value); }
		}
		  
		[XmlAttribute("ShortDescription")]
		[Bindable(true)]
		public string ShortDescription 
		{
			get { return GetColumnValue<string>(Columns.ShortDescription); }
			set { SetColumnValue(Columns.ShortDescription, value); }
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
        
		
		private Wcss.CharitableListingCollection colCharitableListingRecords;
		public Wcss.CharitableListingCollection CharitableListingRecords()
		{
			if(colCharitableListingRecords == null)
			{
				colCharitableListingRecords = new Wcss.CharitableListingCollection().Where(CharitableListing.Columns.TCharitableOrgId, Id).Load();
				colCharitableListingRecords.ListChanged += new ListChangedEventHandler(colCharitableListingRecords_ListChanged);
			}
			return colCharitableListingRecords;
		}
				
		void colCharitableListingRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colCharitableListingRecords[e.NewIndex].TCharitableOrgId = Id;
				colCharitableListingRecords.ListChanged += new ListChangedEventHandler(colCharitableListingRecords_ListChanged);
            }
		}
		#endregion
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a AspnetApplication ActiveRecord object related to this CharitableOrg
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
		public static void Insert(DateTime varDtStamp,bool varBActive,string varName,string varNameRoot,string varDisplayName,string varWebsiteUrl,string varPictureUrl,string varShortDescription,string varDescription,Guid varApplicationId)
		{
			CharitableOrg item = new CharitableOrg();
			
			item.DtStamp = varDtStamp;
			
			item.BActive = varBActive;
			
			item.Name = varName;
			
			item.NameRoot = varNameRoot;
			
			item.DisplayName = varDisplayName;
			
			item.WebsiteUrl = varWebsiteUrl;
			
			item.PictureUrl = varPictureUrl;
			
			item.ShortDescription = varShortDescription;
			
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
		public static void Update(int varId,DateTime varDtStamp,bool varBActive,string varName,string varNameRoot,string varDisplayName,string varWebsiteUrl,string varPictureUrl,string varShortDescription,string varDescription,Guid varApplicationId)
		{
			CharitableOrg item = new CharitableOrg();
			
				item.Id = varId;
			
				item.DtStamp = varDtStamp;
			
				item.BActive = varBActive;
			
				item.Name = varName;
			
				item.NameRoot = varNameRoot;
			
				item.DisplayName = varDisplayName;
			
				item.WebsiteUrl = varWebsiteUrl;
			
				item.PictureUrl = varPictureUrl;
			
				item.ShortDescription = varShortDescription;
			
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
        
        
        
        public static TableSchema.TableColumn BActiveColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn NameColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn NameRootColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn DisplayNameColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn WebsiteUrlColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn PictureUrlColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn ShortDescriptionColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn DescriptionColumn
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        public static TableSchema.TableColumn ApplicationIdColumn
        {
            get { return Schema.Columns[10]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Id = @"Id";
			 public static string DtStamp = @"dtStamp";
			 public static string BActive = @"bActive";
			 public static string Name = @"Name";
			 public static string NameRoot = @"NameRoot";
			 public static string DisplayName = @"DisplayName";
			 public static string WebsiteUrl = @"WebsiteUrl";
			 public static string PictureUrl = @"PictureUrl";
			 public static string ShortDescription = @"ShortDescription";
			 public static string Description = @"Description";
			 public static string ApplicationId = @"ApplicationId";
						
		}
		#endregion
		
		#region Update PK Collections
		
        public void SetPKValues()
        {
                if (colCharitableListingRecords != null)
                {
                    foreach (Wcss.CharitableListing item in colCharitableListingRecords)
                    {
                        if (item.TCharitableOrgId != Id)
                        {
                            item.TCharitableOrgId = Id;
                        }
                    }
               }
		}
        #endregion
    
        #region Deep Save
		
        public void DeepSave()
        {
            Save();
            
                if (colCharitableListingRecords != null)
                {
                    colCharitableListingRecords.SaveAll();
               }
		}
        #endregion
	}
}
