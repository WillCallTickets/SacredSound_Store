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
	/// Strongly-typed collection for the ShowEvent class.
	/// </summary>
    [Serializable]
	public partial class ShowEventCollection : ActiveList<ShowEvent, ShowEventCollection>
	{	   
		public ShowEventCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>ShowEventCollection</returns>
		public ShowEventCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                ShowEvent o = this[i];
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
	/// This is an ActiveRecord class which wraps the ShowEvent table.
	/// </summary>
	[Serializable]
	public partial class ShowEvent : ActiveRecord<ShowEvent>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public ShowEvent()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public ShowEvent(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public ShowEvent(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public ShowEvent(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("ShowEvent", TableType.Table, DataService.GetInstance("WillCall"));
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
				
				TableSchema.TableColumn colvarTOwnerId = new TableSchema.TableColumn(schema);
				colvarTOwnerId.ColumnName = "tOwnerId";
				colvarTOwnerId.DataType = DbType.Int32;
				colvarTOwnerId.MaxLength = 0;
				colvarTOwnerId.AutoIncrement = false;
				colvarTOwnerId.IsNullable = false;
				colvarTOwnerId.IsPrimaryKey = false;
				colvarTOwnerId.IsForeignKey = false;
				colvarTOwnerId.IsReadOnly = false;
				colvarTOwnerId.DefaultSetting = @"";
				colvarTOwnerId.ForeignKeyTableName = "";
				schema.Columns.Add(colvarTOwnerId);
				
				TableSchema.TableColumn colvarVcOwnerType = new TableSchema.TableColumn(schema);
				colvarVcOwnerType.ColumnName = "vcOwnerType";
				colvarVcOwnerType.DataType = DbType.AnsiString;
				colvarVcOwnerType.MaxLength = 256;
				colvarVcOwnerType.AutoIncrement = false;
				colvarVcOwnerType.IsNullable = false;
				colvarVcOwnerType.IsPrimaryKey = false;
				colvarVcOwnerType.IsForeignKey = false;
				colvarVcOwnerType.IsReadOnly = false;
				colvarVcOwnerType.DefaultSetting = @"";
				colvarVcOwnerType.ForeignKeyTableName = "";
				schema.Columns.Add(colvarVcOwnerType);
				
				TableSchema.TableColumn colvarTParentId = new TableSchema.TableColumn(schema);
				colvarTParentId.ColumnName = "tParentId";
				colvarTParentId.DataType = DbType.Int32;
				colvarTParentId.MaxLength = 0;
				colvarTParentId.AutoIncrement = false;
				colvarTParentId.IsNullable = false;
				colvarTParentId.IsPrimaryKey = false;
				colvarTParentId.IsForeignKey = false;
				colvarTParentId.IsReadOnly = false;
				
						colvarTParentId.DefaultSetting = @"((0))";
				colvarTParentId.ForeignKeyTableName = "";
				schema.Columns.Add(colvarTParentId);
				
				TableSchema.TableColumn colvarVcParentType = new TableSchema.TableColumn(schema);
				colvarVcParentType.ColumnName = "vcParentType";
				colvarVcParentType.DataType = DbType.AnsiString;
				colvarVcParentType.MaxLength = 256;
				colvarVcParentType.AutoIncrement = false;
				colvarVcParentType.IsNullable = true;
				colvarVcParentType.IsPrimaryKey = false;
				colvarVcParentType.IsForeignKey = false;
				colvarVcParentType.IsReadOnly = false;
				colvarVcParentType.DefaultSetting = @"";
				colvarVcParentType.ForeignKeyTableName = "";
				schema.Columns.Add(colvarVcParentType);
				
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
				
				TableSchema.TableColumn colvarIOrdinal = new TableSchema.TableColumn(schema);
				colvarIOrdinal.ColumnName = "iOrdinal";
				colvarIOrdinal.DataType = DbType.Int32;
				colvarIOrdinal.MaxLength = 0;
				colvarIOrdinal.AutoIncrement = false;
				colvarIOrdinal.IsNullable = false;
				colvarIOrdinal.IsPrimaryKey = false;
				colvarIOrdinal.IsForeignKey = false;
				colvarIOrdinal.IsReadOnly = false;
				
						colvarIOrdinal.DefaultSetting = @"((-1))";
				colvarIOrdinal.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIOrdinal);
				
				TableSchema.TableColumn colvarDateString = new TableSchema.TableColumn(schema);
				colvarDateString.ColumnName = "DateString";
				colvarDateString.DataType = DbType.AnsiString;
				colvarDateString.MaxLength = 500;
				colvarDateString.AutoIncrement = false;
				colvarDateString.IsNullable = true;
				colvarDateString.IsPrimaryKey = false;
				colvarDateString.IsForeignKey = false;
				colvarDateString.IsReadOnly = false;
				colvarDateString.DefaultSetting = @"";
				colvarDateString.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDateString);
				
				TableSchema.TableColumn colvarStatus = new TableSchema.TableColumn(schema);
				colvarStatus.ColumnName = "Status";
				colvarStatus.DataType = DbType.AnsiString;
				colvarStatus.MaxLength = 500;
				colvarStatus.AutoIncrement = false;
				colvarStatus.IsNullable = true;
				colvarStatus.IsPrimaryKey = false;
				colvarStatus.IsForeignKey = false;
				colvarStatus.IsReadOnly = false;
				colvarStatus.DefaultSetting = @"";
				colvarStatus.ForeignKeyTableName = "";
				schema.Columns.Add(colvarStatus);
				
				TableSchema.TableColumn colvarShowTitle = new TableSchema.TableColumn(schema);
				colvarShowTitle.ColumnName = "ShowTitle";
				colvarShowTitle.DataType = DbType.AnsiString;
				colvarShowTitle.MaxLength = 500;
				colvarShowTitle.AutoIncrement = false;
				colvarShowTitle.IsNullable = true;
				colvarShowTitle.IsPrimaryKey = false;
				colvarShowTitle.IsForeignKey = false;
				colvarShowTitle.IsReadOnly = false;
				colvarShowTitle.DefaultSetting = @"";
				colvarShowTitle.ForeignKeyTableName = "";
				schema.Columns.Add(colvarShowTitle);
				
				TableSchema.TableColumn colvarPromoter = new TableSchema.TableColumn(schema);
				colvarPromoter.ColumnName = "Promoter";
				colvarPromoter.DataType = DbType.AnsiString;
				colvarPromoter.MaxLength = 500;
				colvarPromoter.AutoIncrement = false;
				colvarPromoter.IsNullable = true;
				colvarPromoter.IsPrimaryKey = false;
				colvarPromoter.IsForeignKey = false;
				colvarPromoter.IsReadOnly = false;
				colvarPromoter.DefaultSetting = @"";
				colvarPromoter.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPromoter);
				
				TableSchema.TableColumn colvarHeader = new TableSchema.TableColumn(schema);
				colvarHeader.ColumnName = "Header";
				colvarHeader.DataType = DbType.AnsiString;
				colvarHeader.MaxLength = 500;
				colvarHeader.AutoIncrement = false;
				colvarHeader.IsNullable = true;
				colvarHeader.IsPrimaryKey = false;
				colvarHeader.IsForeignKey = false;
				colvarHeader.IsReadOnly = false;
				colvarHeader.DefaultSetting = @"";
				colvarHeader.ForeignKeyTableName = "";
				schema.Columns.Add(colvarHeader);
				
				TableSchema.TableColumn colvarHeadliner = new TableSchema.TableColumn(schema);
				colvarHeadliner.ColumnName = "Headliner";
				colvarHeadliner.DataType = DbType.AnsiString;
				colvarHeadliner.MaxLength = 2000;
				colvarHeadliner.AutoIncrement = false;
				colvarHeadliner.IsNullable = true;
				colvarHeadliner.IsPrimaryKey = false;
				colvarHeadliner.IsForeignKey = false;
				colvarHeadliner.IsReadOnly = false;
				colvarHeadliner.DefaultSetting = @"";
				colvarHeadliner.ForeignKeyTableName = "";
				schema.Columns.Add(colvarHeadliner);
				
				TableSchema.TableColumn colvarOpener = new TableSchema.TableColumn(schema);
				colvarOpener.ColumnName = "Opener";
				colvarOpener.DataType = DbType.AnsiString;
				colvarOpener.MaxLength = 1000;
				colvarOpener.AutoIncrement = false;
				colvarOpener.IsNullable = true;
				colvarOpener.IsPrimaryKey = false;
				colvarOpener.IsForeignKey = false;
				colvarOpener.IsReadOnly = false;
				colvarOpener.DefaultSetting = @"";
				colvarOpener.ForeignKeyTableName = "";
				schema.Columns.Add(colvarOpener);
				
				TableSchema.TableColumn colvarVenue = new TableSchema.TableColumn(schema);
				colvarVenue.ColumnName = "Venue";
				colvarVenue.DataType = DbType.AnsiString;
				colvarVenue.MaxLength = 500;
				colvarVenue.AutoIncrement = false;
				colvarVenue.IsNullable = true;
				colvarVenue.IsPrimaryKey = false;
				colvarVenue.IsForeignKey = false;
				colvarVenue.IsReadOnly = false;
				colvarVenue.DefaultSetting = @"";
				colvarVenue.ForeignKeyTableName = "";
				schema.Columns.Add(colvarVenue);
				
				TableSchema.TableColumn colvarTimes = new TableSchema.TableColumn(schema);
				colvarTimes.ColumnName = "Times";
				colvarTimes.DataType = DbType.AnsiString;
				colvarTimes.MaxLength = 500;
				colvarTimes.AutoIncrement = false;
				colvarTimes.IsNullable = true;
				colvarTimes.IsPrimaryKey = false;
				colvarTimes.IsForeignKey = false;
				colvarTimes.IsReadOnly = false;
				colvarTimes.DefaultSetting = @"";
				colvarTimes.ForeignKeyTableName = "";
				schema.Columns.Add(colvarTimes);
				
				TableSchema.TableColumn colvarAges = new TableSchema.TableColumn(schema);
				colvarAges.ColumnName = "Ages";
				colvarAges.DataType = DbType.AnsiString;
				colvarAges.MaxLength = 500;
				colvarAges.AutoIncrement = false;
				colvarAges.IsNullable = true;
				colvarAges.IsPrimaryKey = false;
				colvarAges.IsForeignKey = false;
				colvarAges.IsReadOnly = false;
				colvarAges.DefaultSetting = @"";
				colvarAges.ForeignKeyTableName = "";
				schema.Columns.Add(colvarAges);
				
				TableSchema.TableColumn colvarPricing = new TableSchema.TableColumn(schema);
				colvarPricing.ColumnName = "Pricing";
				colvarPricing.DataType = DbType.AnsiString;
				colvarPricing.MaxLength = 256;
				colvarPricing.AutoIncrement = false;
				colvarPricing.IsNullable = true;
				colvarPricing.IsPrimaryKey = false;
				colvarPricing.IsForeignKey = false;
				colvarPricing.IsReadOnly = false;
				colvarPricing.DefaultSetting = @"";
				colvarPricing.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPricing);
				
				TableSchema.TableColumn colvarUrl = new TableSchema.TableColumn(schema);
				colvarUrl.ColumnName = "Url";
				colvarUrl.DataType = DbType.AnsiString;
				colvarUrl.MaxLength = 256;
				colvarUrl.AutoIncrement = false;
				colvarUrl.IsNullable = true;
				colvarUrl.IsPrimaryKey = false;
				colvarUrl.IsForeignKey = false;
				colvarUrl.IsReadOnly = false;
				colvarUrl.DefaultSetting = @"";
				colvarUrl.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUrl);
				
				TableSchema.TableColumn colvarImageUrl = new TableSchema.TableColumn(schema);
				colvarImageUrl.ColumnName = "ImageUrl";
				colvarImageUrl.DataType = DbType.AnsiString;
				colvarImageUrl.MaxLength = 256;
				colvarImageUrl.AutoIncrement = false;
				colvarImageUrl.IsNullable = true;
				colvarImageUrl.IsPrimaryKey = false;
				colvarImageUrl.IsForeignKey = false;
				colvarImageUrl.IsReadOnly = false;
				colvarImageUrl.DefaultSetting = @"";
				colvarImageUrl.ForeignKeyTableName = "";
				schema.Columns.Add(colvarImageUrl);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["WillCall"].AddSchema("ShowEvent",schema);
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
		  
		[XmlAttribute("TOwnerId")]
		[Bindable(true)]
		public int TOwnerId 
		{
			get { return GetColumnValue<int>(Columns.TOwnerId); }
			set { SetColumnValue(Columns.TOwnerId, value); }
		}
		  
		[XmlAttribute("VcOwnerType")]
		[Bindable(true)]
		public string VcOwnerType 
		{
			get { return GetColumnValue<string>(Columns.VcOwnerType); }
			set { SetColumnValue(Columns.VcOwnerType, value); }
		}
		  
		[XmlAttribute("TParentId")]
		[Bindable(true)]
		public int TParentId 
		{
			get { return GetColumnValue<int>(Columns.TParentId); }
			set { SetColumnValue(Columns.TParentId, value); }
		}
		  
		[XmlAttribute("VcParentType")]
		[Bindable(true)]
		public string VcParentType 
		{
			get { return GetColumnValue<string>(Columns.VcParentType); }
			set { SetColumnValue(Columns.VcParentType, value); }
		}
		  
		[XmlAttribute("BActive")]
		[Bindable(true)]
		public bool BActive 
		{
			get { return GetColumnValue<bool>(Columns.BActive); }
			set { SetColumnValue(Columns.BActive, value); }
		}
		  
		[XmlAttribute("IOrdinal")]
		[Bindable(true)]
		public int IOrdinal 
		{
			get { return GetColumnValue<int>(Columns.IOrdinal); }
			set { SetColumnValue(Columns.IOrdinal, value); }
		}
		  
		[XmlAttribute("DateString")]
		[Bindable(true)]
		public string DateString 
		{
			get { return GetColumnValue<string>(Columns.DateString); }
			set { SetColumnValue(Columns.DateString, value); }
		}
		  
		[XmlAttribute("Status")]
		[Bindable(true)]
		public string Status 
		{
			get { return GetColumnValue<string>(Columns.Status); }
			set { SetColumnValue(Columns.Status, value); }
		}
		  
		[XmlAttribute("ShowTitle")]
		[Bindable(true)]
		public string ShowTitle 
		{
			get { return GetColumnValue<string>(Columns.ShowTitle); }
			set { SetColumnValue(Columns.ShowTitle, value); }
		}
		  
		[XmlAttribute("Promoter")]
		[Bindable(true)]
		public string Promoter 
		{
			get { return GetColumnValue<string>(Columns.Promoter); }
			set { SetColumnValue(Columns.Promoter, value); }
		}
		  
		[XmlAttribute("Header")]
		[Bindable(true)]
		public string Header 
		{
			get { return GetColumnValue<string>(Columns.Header); }
			set { SetColumnValue(Columns.Header, value); }
		}
		  
		[XmlAttribute("Headliner")]
		[Bindable(true)]
		public string Headliner 
		{
			get { return GetColumnValue<string>(Columns.Headliner); }
			set { SetColumnValue(Columns.Headliner, value); }
		}
		  
		[XmlAttribute("Opener")]
		[Bindable(true)]
		public string Opener 
		{
			get { return GetColumnValue<string>(Columns.Opener); }
			set { SetColumnValue(Columns.Opener, value); }
		}
		  
		[XmlAttribute("Venue")]
		[Bindable(true)]
		public string Venue 
		{
			get { return GetColumnValue<string>(Columns.Venue); }
			set { SetColumnValue(Columns.Venue, value); }
		}
		  
		[XmlAttribute("Times")]
		[Bindable(true)]
		public string Times 
		{
			get { return GetColumnValue<string>(Columns.Times); }
			set { SetColumnValue(Columns.Times, value); }
		}
		  
		[XmlAttribute("Ages")]
		[Bindable(true)]
		public string Ages 
		{
			get { return GetColumnValue<string>(Columns.Ages); }
			set { SetColumnValue(Columns.Ages, value); }
		}
		  
		[XmlAttribute("Pricing")]
		[Bindable(true)]
		public string Pricing 
		{
			get { return GetColumnValue<string>(Columns.Pricing); }
			set { SetColumnValue(Columns.Pricing, value); }
		}
		  
		[XmlAttribute("Url")]
		[Bindable(true)]
		public string Url 
		{
			get { return GetColumnValue<string>(Columns.Url); }
			set { SetColumnValue(Columns.Url, value); }
		}
		  
		[XmlAttribute("ImageUrl")]
		[Bindable(true)]
		public string ImageUrl 
		{
			get { return GetColumnValue<string>(Columns.ImageUrl); }
			set { SetColumnValue(Columns.ImageUrl, value); }
		}
		
		#endregion
		
		
			
		
		//no foreign key tables defined (0)
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(DateTime varDtStamp,int varTOwnerId,string varVcOwnerType,int varTParentId,string varVcParentType,bool varBActive,int varIOrdinal,string varDateString,string varStatus,string varShowTitle,string varPromoter,string varHeader,string varHeadliner,string varOpener,string varVenue,string varTimes,string varAges,string varPricing,string varUrl,string varImageUrl)
		{
			ShowEvent item = new ShowEvent();
			
			item.DtStamp = varDtStamp;
			
			item.TOwnerId = varTOwnerId;
			
			item.VcOwnerType = varVcOwnerType;
			
			item.TParentId = varTParentId;
			
			item.VcParentType = varVcParentType;
			
			item.BActive = varBActive;
			
			item.IOrdinal = varIOrdinal;
			
			item.DateString = varDateString;
			
			item.Status = varStatus;
			
			item.ShowTitle = varShowTitle;
			
			item.Promoter = varPromoter;
			
			item.Header = varHeader;
			
			item.Headliner = varHeadliner;
			
			item.Opener = varOpener;
			
			item.Venue = varVenue;
			
			item.Times = varTimes;
			
			item.Ages = varAges;
			
			item.Pricing = varPricing;
			
			item.Url = varUrl;
			
			item.ImageUrl = varImageUrl;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varId,DateTime varDtStamp,int varTOwnerId,string varVcOwnerType,int varTParentId,string varVcParentType,bool varBActive,int varIOrdinal,string varDateString,string varStatus,string varShowTitle,string varPromoter,string varHeader,string varHeadliner,string varOpener,string varVenue,string varTimes,string varAges,string varPricing,string varUrl,string varImageUrl)
		{
			ShowEvent item = new ShowEvent();
			
				item.Id = varId;
			
				item.DtStamp = varDtStamp;
			
				item.TOwnerId = varTOwnerId;
			
				item.VcOwnerType = varVcOwnerType;
			
				item.TParentId = varTParentId;
			
				item.VcParentType = varVcParentType;
			
				item.BActive = varBActive;
			
				item.IOrdinal = varIOrdinal;
			
				item.DateString = varDateString;
			
				item.Status = varStatus;
			
				item.ShowTitle = varShowTitle;
			
				item.Promoter = varPromoter;
			
				item.Header = varHeader;
			
				item.Headliner = varHeadliner;
			
				item.Opener = varOpener;
			
				item.Venue = varVenue;
			
				item.Times = varTimes;
			
				item.Ages = varAges;
			
				item.Pricing = varPricing;
			
				item.Url = varUrl;
			
				item.ImageUrl = varImageUrl;
			
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
        
        
        
        public static TableSchema.TableColumn TOwnerIdColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn VcOwnerTypeColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn TParentIdColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn VcParentTypeColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn BActiveColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn IOrdinalColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn DateStringColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn StatusColumn
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        public static TableSchema.TableColumn ShowTitleColumn
        {
            get { return Schema.Columns[10]; }
        }
        
        
        
        public static TableSchema.TableColumn PromoterColumn
        {
            get { return Schema.Columns[11]; }
        }
        
        
        
        public static TableSchema.TableColumn HeaderColumn
        {
            get { return Schema.Columns[12]; }
        }
        
        
        
        public static TableSchema.TableColumn HeadlinerColumn
        {
            get { return Schema.Columns[13]; }
        }
        
        
        
        public static TableSchema.TableColumn OpenerColumn
        {
            get { return Schema.Columns[14]; }
        }
        
        
        
        public static TableSchema.TableColumn VenueColumn
        {
            get { return Schema.Columns[15]; }
        }
        
        
        
        public static TableSchema.TableColumn TimesColumn
        {
            get { return Schema.Columns[16]; }
        }
        
        
        
        public static TableSchema.TableColumn AgesColumn
        {
            get { return Schema.Columns[17]; }
        }
        
        
        
        public static TableSchema.TableColumn PricingColumn
        {
            get { return Schema.Columns[18]; }
        }
        
        
        
        public static TableSchema.TableColumn UrlColumn
        {
            get { return Schema.Columns[19]; }
        }
        
        
        
        public static TableSchema.TableColumn ImageUrlColumn
        {
            get { return Schema.Columns[20]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Id = @"Id";
			 public static string DtStamp = @"dtStamp";
			 public static string TOwnerId = @"tOwnerId";
			 public static string VcOwnerType = @"vcOwnerType";
			 public static string TParentId = @"tParentId";
			 public static string VcParentType = @"vcParentType";
			 public static string BActive = @"bActive";
			 public static string IOrdinal = @"iOrdinal";
			 public static string DateString = @"DateString";
			 public static string Status = @"Status";
			 public static string ShowTitle = @"ShowTitle";
			 public static string Promoter = @"Promoter";
			 public static string Header = @"Header";
			 public static string Headliner = @"Headliner";
			 public static string Opener = @"Opener";
			 public static string Venue = @"Venue";
			 public static string Times = @"Times";
			 public static string Ages = @"Ages";
			 public static string Pricing = @"Pricing";
			 public static string Url = @"Url";
			 public static string ImageUrl = @"ImageUrl";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
