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
	/// Strongly-typed collection for the Act class.
	/// </summary>
    [Serializable]
	public partial class ActCollection : ActiveList<Act, ActCollection>
	{	   
		public ActCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>ActCollection</returns>
		public ActCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                Act o = this[i];
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
	/// This is an ActiveRecord class which wraps the Act table.
	/// </summary>
	[Serializable]
	public partial class Act : ActiveRecord<Act>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public Act()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public Act(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public Act(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public Act(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("Act", TableType.Table, DataService.GetInstance("WillCall"));
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
				
				TableSchema.TableColumn colvarWebsite = new TableSchema.TableColumn(schema);
				colvarWebsite.ColumnName = "Website";
				colvarWebsite.DataType = DbType.AnsiString;
				colvarWebsite.MaxLength = 256;
				colvarWebsite.AutoIncrement = false;
				colvarWebsite.IsNullable = true;
				colvarWebsite.IsPrimaryKey = false;
				colvarWebsite.IsForeignKey = false;
				colvarWebsite.IsReadOnly = false;
				colvarWebsite.DefaultSetting = @"";
				colvarWebsite.ForeignKeyTableName = "";
				schema.Columns.Add(colvarWebsite);
				
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
				
				TableSchema.TableColumn colvarIPicWidth = new TableSchema.TableColumn(schema);
				colvarIPicWidth.ColumnName = "iPicWidth";
				colvarIPicWidth.DataType = DbType.Int32;
				colvarIPicWidth.MaxLength = 0;
				colvarIPicWidth.AutoIncrement = false;
				colvarIPicWidth.IsNullable = false;
				colvarIPicWidth.IsPrimaryKey = false;
				colvarIPicWidth.IsForeignKey = false;
				colvarIPicWidth.IsReadOnly = false;
				
						colvarIPicWidth.DefaultSetting = @"((0))";
				colvarIPicWidth.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIPicWidth);
				
				TableSchema.TableColumn colvarIPicHeight = new TableSchema.TableColumn(schema);
				colvarIPicHeight.ColumnName = "iPicHeight";
				colvarIPicHeight.DataType = DbType.Int32;
				colvarIPicHeight.MaxLength = 0;
				colvarIPicHeight.AutoIncrement = false;
				colvarIPicHeight.IsNullable = false;
				colvarIPicHeight.IsPrimaryKey = false;
				colvarIPicHeight.IsForeignKey = false;
				colvarIPicHeight.IsReadOnly = false;
				
						colvarIPicHeight.DefaultSetting = @"((0))";
				colvarIPicHeight.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIPicHeight);
				
				TableSchema.TableColumn colvarBListInDirectory = new TableSchema.TableColumn(schema);
				colvarBListInDirectory.ColumnName = "bListInDirectory";
				colvarBListInDirectory.DataType = DbType.Boolean;
				colvarBListInDirectory.MaxLength = 0;
				colvarBListInDirectory.AutoIncrement = false;
				colvarBListInDirectory.IsNullable = false;
				colvarBListInDirectory.IsPrimaryKey = false;
				colvarBListInDirectory.IsForeignKey = false;
				colvarBListInDirectory.IsReadOnly = false;
				
						colvarBListInDirectory.DefaultSetting = @"((1))";
				colvarBListInDirectory.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBListInDirectory);
				
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
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["WillCall"].AddSchema("Act",schema);
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
		  
		[XmlAttribute("Website")]
		[Bindable(true)]
		public string Website 
		{
			get { return GetColumnValue<string>(Columns.Website); }
			set { SetColumnValue(Columns.Website, value); }
		}
		  
		[XmlAttribute("PictureUrl")]
		[Bindable(true)]
		public string PictureUrl 
		{
			get { return GetColumnValue<string>(Columns.PictureUrl); }
			set { SetColumnValue(Columns.PictureUrl, value); }
		}
		  
		[XmlAttribute("IPicWidth")]
		[Bindable(true)]
		public int IPicWidth 
		{
			get { return GetColumnValue<int>(Columns.IPicWidth); }
			set { SetColumnValue(Columns.IPicWidth, value); }
		}
		  
		[XmlAttribute("IPicHeight")]
		[Bindable(true)]
		public int IPicHeight 
		{
			get { return GetColumnValue<int>(Columns.IPicHeight); }
			set { SetColumnValue(Columns.IPicHeight, value); }
		}
		  
		[XmlAttribute("BListInDirectory")]
		[Bindable(true)]
		public bool BListInDirectory 
		{
			get { return GetColumnValue<bool>(Columns.BListInDirectory); }
			set { SetColumnValue(Columns.BListInDirectory, value); }
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
		
		#endregion
		
		
		#region PrimaryKey Methods		
		
        protected override void SetPrimaryKey(object oValue)
        {
            base.SetPrimaryKey(oValue);
            
            SetPKValues();
        }
        
		
		private Wcss.DownloadCollection colDownloadRecords;
		public Wcss.DownloadCollection DownloadRecords()
		{
			if(colDownloadRecords == null)
			{
				colDownloadRecords = new Wcss.DownloadCollection().Where(Download.Columns.TActId, Id).Load();
				colDownloadRecords.ListChanged += new ListChangedEventHandler(colDownloadRecords_ListChanged);
			}
			return colDownloadRecords;
		}
				
		void colDownloadRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colDownloadRecords[e.NewIndex].TActId = Id;
				colDownloadRecords.ListChanged += new ListChangedEventHandler(colDownloadRecords_ListChanged);
            }
		}
		private Wcss.JShowActCollection colJShowActRecords;
		public Wcss.JShowActCollection JShowActRecords()
		{
			if(colJShowActRecords == null)
			{
				colJShowActRecords = new Wcss.JShowActCollection().Where(JShowAct.Columns.TActId, Id).Load();
				colJShowActRecords.ListChanged += new ListChangedEventHandler(colJShowActRecords_ListChanged);
			}
			return colJShowActRecords;
		}
				
		void colJShowActRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colJShowActRecords[e.NewIndex].TActId = Id;
				colJShowActRecords.ListChanged += new ListChangedEventHandler(colJShowActRecords_ListChanged);
            }
		}
		#endregion
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a AspnetApplication ActiveRecord object related to this Act
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
		public static void Insert(string varName,string varNameRoot,string varDisplayName,string varWebsite,string varPictureUrl,int varIPicWidth,int varIPicHeight,bool varBListInDirectory,DateTime varDtStamp,Guid varApplicationId)
		{
			Act item = new Act();
			
			item.Name = varName;
			
			item.NameRoot = varNameRoot;
			
			item.DisplayName = varDisplayName;
			
			item.Website = varWebsite;
			
			item.PictureUrl = varPictureUrl;
			
			item.IPicWidth = varIPicWidth;
			
			item.IPicHeight = varIPicHeight;
			
			item.BListInDirectory = varBListInDirectory;
			
			item.DtStamp = varDtStamp;
			
			item.ApplicationId = varApplicationId;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varId,string varName,string varNameRoot,string varDisplayName,string varWebsite,string varPictureUrl,int varIPicWidth,int varIPicHeight,bool varBListInDirectory,DateTime varDtStamp,Guid varApplicationId)
		{
			Act item = new Act();
			
				item.Id = varId;
			
				item.Name = varName;
			
				item.NameRoot = varNameRoot;
			
				item.DisplayName = varDisplayName;
			
				item.Website = varWebsite;
			
				item.PictureUrl = varPictureUrl;
			
				item.IPicWidth = varIPicWidth;
			
				item.IPicHeight = varIPicHeight;
			
				item.BListInDirectory = varBListInDirectory;
			
				item.DtStamp = varDtStamp;
			
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
        
        
        
        public static TableSchema.TableColumn NameColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn NameRootColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn DisplayNameColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn WebsiteColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn PictureUrlColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn IPicWidthColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn IPicHeightColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn BListInDirectoryColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn DtStampColumn
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
			 public static string Name = @"Name";
			 public static string NameRoot = @"NameRoot";
			 public static string DisplayName = @"DisplayName";
			 public static string Website = @"Website";
			 public static string PictureUrl = @"PictureUrl";
			 public static string IPicWidth = @"iPicWidth";
			 public static string IPicHeight = @"iPicHeight";
			 public static string BListInDirectory = @"bListInDirectory";
			 public static string DtStamp = @"dtStamp";
			 public static string ApplicationId = @"ApplicationId";
						
		}
		#endregion
		
		#region Update PK Collections
		
        public void SetPKValues()
        {
                if (colDownloadRecords != null)
                {
                    foreach (Wcss.Download item in colDownloadRecords)
                    {
                        if (item.TActId != Id)
                        {
                            item.TActId = Id;
                        }
                    }
               }
		
                if (colJShowActRecords != null)
                {
                    foreach (Wcss.JShowAct item in colJShowActRecords)
                    {
                        if (item.TActId != Id)
                        {
                            item.TActId = Id;
                        }
                    }
               }
		}
        #endregion
    
        #region Deep Save
		
        public void DeepSave()
        {
            Save();
            
                if (colDownloadRecords != null)
                {
                    colDownloadRecords.SaveAll();
               }
		
                if (colJShowActRecords != null)
                {
                    colJShowActRecords.SaveAll();
               }
		}
        #endregion
	}
}
