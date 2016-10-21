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
	/// Strongly-typed collection for the Venue class.
	/// </summary>
    [Serializable]
	public partial class VenueCollection : ActiveList<Venue, VenueCollection>
	{	   
		public VenueCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>VenueCollection</returns>
		public VenueCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                Venue o = this[i];
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
	/// This is an ActiveRecord class which wraps the Venue table.
	/// </summary>
	[Serializable]
	public partial class Venue : ActiveRecord<Venue>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public Venue()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public Venue(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public Venue(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public Venue(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("Venue", TableType.Table, DataService.GetInstance("WillCall"));
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
				
				TableSchema.TableColumn colvarICapacity = new TableSchema.TableColumn(schema);
				colvarICapacity.ColumnName = "iCapacity";
				colvarICapacity.DataType = DbType.Int32;
				colvarICapacity.MaxLength = 0;
				colvarICapacity.AutoIncrement = false;
				colvarICapacity.IsNullable = true;
				colvarICapacity.IsPrimaryKey = false;
				colvarICapacity.IsForeignKey = false;
				colvarICapacity.IsReadOnly = false;
				colvarICapacity.DefaultSetting = @"";
				colvarICapacity.ForeignKeyTableName = "";
				schema.Columns.Add(colvarICapacity);
				
				TableSchema.TableColumn colvarPictureUrl = new TableSchema.TableColumn(schema);
				colvarPictureUrl.ColumnName = "PictureUrl";
				colvarPictureUrl.DataType = DbType.AnsiString;
				colvarPictureUrl.MaxLength = 300;
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
				
				TableSchema.TableColumn colvarWebsiteUrl = new TableSchema.TableColumn(schema);
				colvarWebsiteUrl.ColumnName = "WebsiteUrl";
				colvarWebsiteUrl.DataType = DbType.AnsiString;
				colvarWebsiteUrl.MaxLength = 300;
				colvarWebsiteUrl.AutoIncrement = false;
				colvarWebsiteUrl.IsNullable = true;
				colvarWebsiteUrl.IsPrimaryKey = false;
				colvarWebsiteUrl.IsForeignKey = false;
				colvarWebsiteUrl.IsReadOnly = false;
				colvarWebsiteUrl.DefaultSetting = @"";
				colvarWebsiteUrl.ForeignKeyTableName = "";
				schema.Columns.Add(colvarWebsiteUrl);
				
				TableSchema.TableColumn colvarShortAddress = new TableSchema.TableColumn(schema);
				colvarShortAddress.ColumnName = "ShortAddress";
				colvarShortAddress.DataType = DbType.AnsiString;
				colvarShortAddress.MaxLength = 500;
				colvarShortAddress.AutoIncrement = false;
				colvarShortAddress.IsNullable = true;
				colvarShortAddress.IsPrimaryKey = false;
				colvarShortAddress.IsForeignKey = false;
				colvarShortAddress.IsReadOnly = false;
				colvarShortAddress.DefaultSetting = @"";
				colvarShortAddress.ForeignKeyTableName = "";
				schema.Columns.Add(colvarShortAddress);
				
				TableSchema.TableColumn colvarAddress = new TableSchema.TableColumn(schema);
				colvarAddress.ColumnName = "Address";
				colvarAddress.DataType = DbType.AnsiString;
				colvarAddress.MaxLength = 150;
				colvarAddress.AutoIncrement = false;
				colvarAddress.IsNullable = true;
				colvarAddress.IsPrimaryKey = false;
				colvarAddress.IsForeignKey = false;
				colvarAddress.IsReadOnly = false;
				colvarAddress.DefaultSetting = @"";
				colvarAddress.ForeignKeyTableName = "";
				schema.Columns.Add(colvarAddress);
				
				TableSchema.TableColumn colvarCity = new TableSchema.TableColumn(schema);
				colvarCity.ColumnName = "City";
				colvarCity.DataType = DbType.AnsiString;
				colvarCity.MaxLength = 100;
				colvarCity.AutoIncrement = false;
				colvarCity.IsNullable = true;
				colvarCity.IsPrimaryKey = false;
				colvarCity.IsForeignKey = false;
				colvarCity.IsReadOnly = false;
				colvarCity.DefaultSetting = @"";
				colvarCity.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCity);
				
				TableSchema.TableColumn colvarState = new TableSchema.TableColumn(schema);
				colvarState.ColumnName = "State";
				colvarState.DataType = DbType.AnsiString;
				colvarState.MaxLength = 50;
				colvarState.AutoIncrement = false;
				colvarState.IsNullable = true;
				colvarState.IsPrimaryKey = false;
				colvarState.IsForeignKey = false;
				colvarState.IsReadOnly = false;
				colvarState.DefaultSetting = @"";
				colvarState.ForeignKeyTableName = "";
				schema.Columns.Add(colvarState);
				
				TableSchema.TableColumn colvarZipCode = new TableSchema.TableColumn(schema);
				colvarZipCode.ColumnName = "ZipCode";
				colvarZipCode.DataType = DbType.AnsiString;
				colvarZipCode.MaxLength = 10;
				colvarZipCode.AutoIncrement = false;
				colvarZipCode.IsNullable = true;
				colvarZipCode.IsPrimaryKey = false;
				colvarZipCode.IsForeignKey = false;
				colvarZipCode.IsReadOnly = false;
				colvarZipCode.DefaultSetting = @"";
				colvarZipCode.ForeignKeyTableName = "";
				schema.Columns.Add(colvarZipCode);
				
				TableSchema.TableColumn colvarCountry = new TableSchema.TableColumn(schema);
				colvarCountry.ColumnName = "Country";
				colvarCountry.DataType = DbType.AnsiString;
				colvarCountry.MaxLength = 256;
				colvarCountry.AutoIncrement = false;
				colvarCountry.IsNullable = true;
				colvarCountry.IsPrimaryKey = false;
				colvarCountry.IsForeignKey = false;
				colvarCountry.IsReadOnly = false;
				colvarCountry.DefaultSetting = @"";
				colvarCountry.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCountry);
				
				TableSchema.TableColumn colvarLatitude = new TableSchema.TableColumn(schema);
				colvarLatitude.ColumnName = "Latitude";
				colvarLatitude.DataType = DbType.AnsiString;
				colvarLatitude.MaxLength = 50;
				colvarLatitude.AutoIncrement = false;
				colvarLatitude.IsNullable = true;
				colvarLatitude.IsPrimaryKey = false;
				colvarLatitude.IsForeignKey = false;
				colvarLatitude.IsReadOnly = false;
				colvarLatitude.DefaultSetting = @"";
				colvarLatitude.ForeignKeyTableName = "";
				schema.Columns.Add(colvarLatitude);
				
				TableSchema.TableColumn colvarLongitude = new TableSchema.TableColumn(schema);
				colvarLongitude.ColumnName = "Longitude";
				colvarLongitude.DataType = DbType.AnsiString;
				colvarLongitude.MaxLength = 50;
				colvarLongitude.AutoIncrement = false;
				colvarLongitude.IsNullable = true;
				colvarLongitude.IsPrimaryKey = false;
				colvarLongitude.IsForeignKey = false;
				colvarLongitude.IsReadOnly = false;
				colvarLongitude.DefaultSetting = @"";
				colvarLongitude.ForeignKeyTableName = "";
				schema.Columns.Add(colvarLongitude);
				
				TableSchema.TableColumn colvarBoxOfficePhone = new TableSchema.TableColumn(schema);
				colvarBoxOfficePhone.ColumnName = "BoxOfficePhone";
				colvarBoxOfficePhone.DataType = DbType.AnsiString;
				colvarBoxOfficePhone.MaxLength = 100;
				colvarBoxOfficePhone.AutoIncrement = false;
				colvarBoxOfficePhone.IsNullable = true;
				colvarBoxOfficePhone.IsPrimaryKey = false;
				colvarBoxOfficePhone.IsForeignKey = false;
				colvarBoxOfficePhone.IsReadOnly = false;
				colvarBoxOfficePhone.DefaultSetting = @"";
				colvarBoxOfficePhone.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBoxOfficePhone);
				
				TableSchema.TableColumn colvarBoxOfficePhoneExt = new TableSchema.TableColumn(schema);
				colvarBoxOfficePhoneExt.ColumnName = "BoxOfficePhoneExt";
				colvarBoxOfficePhoneExt.DataType = DbType.AnsiString;
				colvarBoxOfficePhoneExt.MaxLength = 100;
				colvarBoxOfficePhoneExt.AutoIncrement = false;
				colvarBoxOfficePhoneExt.IsNullable = true;
				colvarBoxOfficePhoneExt.IsPrimaryKey = false;
				colvarBoxOfficePhoneExt.IsForeignKey = false;
				colvarBoxOfficePhoneExt.IsReadOnly = false;
				colvarBoxOfficePhoneExt.DefaultSetting = @"";
				colvarBoxOfficePhoneExt.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBoxOfficePhoneExt);
				
				TableSchema.TableColumn colvarBoxOfficeNotes = new TableSchema.TableColumn(schema);
				colvarBoxOfficeNotes.ColumnName = "BoxOfficeNotes";
				colvarBoxOfficeNotes.DataType = DbType.AnsiString;
				colvarBoxOfficeNotes.MaxLength = 500;
				colvarBoxOfficeNotes.AutoIncrement = false;
				colvarBoxOfficeNotes.IsNullable = true;
				colvarBoxOfficeNotes.IsPrimaryKey = false;
				colvarBoxOfficeNotes.IsForeignKey = false;
				colvarBoxOfficeNotes.IsReadOnly = false;
				colvarBoxOfficeNotes.DefaultSetting = @"";
				colvarBoxOfficeNotes.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBoxOfficeNotes);
				
				TableSchema.TableColumn colvarMainPhone = new TableSchema.TableColumn(schema);
				colvarMainPhone.ColumnName = "MainPhone";
				colvarMainPhone.DataType = DbType.AnsiString;
				colvarMainPhone.MaxLength = 100;
				colvarMainPhone.AutoIncrement = false;
				colvarMainPhone.IsNullable = true;
				colvarMainPhone.IsPrimaryKey = false;
				colvarMainPhone.IsForeignKey = false;
				colvarMainPhone.IsReadOnly = false;
				colvarMainPhone.DefaultSetting = @"";
				colvarMainPhone.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMainPhone);
				
				TableSchema.TableColumn colvarMainPhoneExt = new TableSchema.TableColumn(schema);
				colvarMainPhoneExt.ColumnName = "MainPhoneExt";
				colvarMainPhoneExt.DataType = DbType.AnsiString;
				colvarMainPhoneExt.MaxLength = 100;
				colvarMainPhoneExt.AutoIncrement = false;
				colvarMainPhoneExt.IsNullable = true;
				colvarMainPhoneExt.IsPrimaryKey = false;
				colvarMainPhoneExt.IsForeignKey = false;
				colvarMainPhoneExt.IsReadOnly = false;
				colvarMainPhoneExt.DefaultSetting = @"";
				colvarMainPhoneExt.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMainPhoneExt);
				
				TableSchema.TableColumn colvarNotes = new TableSchema.TableColumn(schema);
				colvarNotes.ColumnName = "Notes";
				colvarNotes.DataType = DbType.AnsiString;
				colvarNotes.MaxLength = 500;
				colvarNotes.AutoIncrement = false;
				colvarNotes.IsNullable = true;
				colvarNotes.IsPrimaryKey = false;
				colvarNotes.IsForeignKey = false;
				colvarNotes.IsReadOnly = false;
				colvarNotes.DefaultSetting = @"";
				colvarNotes.ForeignKeyTableName = "";
				schema.Columns.Add(colvarNotes);
				
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
				DataService.Providers["WillCall"].AddSchema("Venue",schema);
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
		  
		[XmlAttribute("ICapacity")]
		[Bindable(true)]
		public int? ICapacity 
		{
			get { return GetColumnValue<int?>(Columns.ICapacity); }
			set { SetColumnValue(Columns.ICapacity, value); }
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
		  
		[XmlAttribute("WebsiteUrl")]
		[Bindable(true)]
		public string WebsiteUrl 
		{
			get { return GetColumnValue<string>(Columns.WebsiteUrl); }
			set { SetColumnValue(Columns.WebsiteUrl, value); }
		}
		  
		[XmlAttribute("ShortAddress")]
		[Bindable(true)]
		public string ShortAddress 
		{
			get { return GetColumnValue<string>(Columns.ShortAddress); }
			set { SetColumnValue(Columns.ShortAddress, value); }
		}
		  
		[XmlAttribute("Address")]
		[Bindable(true)]
		public string Address 
		{
			get { return GetColumnValue<string>(Columns.Address); }
			set { SetColumnValue(Columns.Address, value); }
		}
		  
		[XmlAttribute("City")]
		[Bindable(true)]
		public string City 
		{
			get { return GetColumnValue<string>(Columns.City); }
			set { SetColumnValue(Columns.City, value); }
		}
		  
		[XmlAttribute("State")]
		[Bindable(true)]
		public string State 
		{
			get { return GetColumnValue<string>(Columns.State); }
			set { SetColumnValue(Columns.State, value); }
		}
		  
		[XmlAttribute("ZipCode")]
		[Bindable(true)]
		public string ZipCode 
		{
			get { return GetColumnValue<string>(Columns.ZipCode); }
			set { SetColumnValue(Columns.ZipCode, value); }
		}
		  
		[XmlAttribute("Country")]
		[Bindable(true)]
		public string Country 
		{
			get { return GetColumnValue<string>(Columns.Country); }
			set { SetColumnValue(Columns.Country, value); }
		}
		  
		[XmlAttribute("Latitude")]
		[Bindable(true)]
		public string Latitude 
		{
			get { return GetColumnValue<string>(Columns.Latitude); }
			set { SetColumnValue(Columns.Latitude, value); }
		}
		  
		[XmlAttribute("Longitude")]
		[Bindable(true)]
		public string Longitude 
		{
			get { return GetColumnValue<string>(Columns.Longitude); }
			set { SetColumnValue(Columns.Longitude, value); }
		}
		  
		[XmlAttribute("BoxOfficePhone")]
		[Bindable(true)]
		public string BoxOfficePhone 
		{
			get { return GetColumnValue<string>(Columns.BoxOfficePhone); }
			set { SetColumnValue(Columns.BoxOfficePhone, value); }
		}
		  
		[XmlAttribute("BoxOfficePhoneExt")]
		[Bindable(true)]
		public string BoxOfficePhoneExt 
		{
			get { return GetColumnValue<string>(Columns.BoxOfficePhoneExt); }
			set { SetColumnValue(Columns.BoxOfficePhoneExt, value); }
		}
		  
		[XmlAttribute("BoxOfficeNotes")]
		[Bindable(true)]
		public string BoxOfficeNotes 
		{
			get { return GetColumnValue<string>(Columns.BoxOfficeNotes); }
			set { SetColumnValue(Columns.BoxOfficeNotes, value); }
		}
		  
		[XmlAttribute("MainPhone")]
		[Bindable(true)]
		public string MainPhone 
		{
			get { return GetColumnValue<string>(Columns.MainPhone); }
			set { SetColumnValue(Columns.MainPhone, value); }
		}
		  
		[XmlAttribute("MainPhoneExt")]
		[Bindable(true)]
		public string MainPhoneExt 
		{
			get { return GetColumnValue<string>(Columns.MainPhoneExt); }
			set { SetColumnValue(Columns.MainPhoneExt, value); }
		}
		  
		[XmlAttribute("Notes")]
		[Bindable(true)]
		public string Notes 
		{
			get { return GetColumnValue<string>(Columns.Notes); }
			set { SetColumnValue(Columns.Notes, value); }
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
        
		
		private Wcss.ShowCollection colShowRecords;
		public Wcss.ShowCollection ShowRecords()
		{
			if(colShowRecords == null)
			{
				colShowRecords = new Wcss.ShowCollection().Where(Show.Columns.TVenueId, Id).Load();
				colShowRecords.ListChanged += new ListChangedEventHandler(colShowRecords_ListChanged);
			}
			return colShowRecords;
		}
				
		void colShowRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colShowRecords[e.NewIndex].TVenueId = Id;
				colShowRecords.ListChanged += new ListChangedEventHandler(colShowRecords_ListChanged);
            }
		}
		#endregion
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a AspnetApplication ActiveRecord object related to this Venue
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
		public static void Insert(string varName,string varNameRoot,string varDisplayName,int? varICapacity,string varPictureUrl,int varIPicWidth,int varIPicHeight,string varWebsiteUrl,string varShortAddress,string varAddress,string varCity,string varState,string varZipCode,string varCountry,string varLatitude,string varLongitude,string varBoxOfficePhone,string varBoxOfficePhoneExt,string varBoxOfficeNotes,string varMainPhone,string varMainPhoneExt,string varNotes,DateTime varDtStamp,Guid varApplicationId)
		{
			Venue item = new Venue();
			
			item.Name = varName;
			
			item.NameRoot = varNameRoot;
			
			item.DisplayName = varDisplayName;
			
			item.ICapacity = varICapacity;
			
			item.PictureUrl = varPictureUrl;
			
			item.IPicWidth = varIPicWidth;
			
			item.IPicHeight = varIPicHeight;
			
			item.WebsiteUrl = varWebsiteUrl;
			
			item.ShortAddress = varShortAddress;
			
			item.Address = varAddress;
			
			item.City = varCity;
			
			item.State = varState;
			
			item.ZipCode = varZipCode;
			
			item.Country = varCountry;
			
			item.Latitude = varLatitude;
			
			item.Longitude = varLongitude;
			
			item.BoxOfficePhone = varBoxOfficePhone;
			
			item.BoxOfficePhoneExt = varBoxOfficePhoneExt;
			
			item.BoxOfficeNotes = varBoxOfficeNotes;
			
			item.MainPhone = varMainPhone;
			
			item.MainPhoneExt = varMainPhoneExt;
			
			item.Notes = varNotes;
			
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
		public static void Update(int varId,string varName,string varNameRoot,string varDisplayName,int? varICapacity,string varPictureUrl,int varIPicWidth,int varIPicHeight,string varWebsiteUrl,string varShortAddress,string varAddress,string varCity,string varState,string varZipCode,string varCountry,string varLatitude,string varLongitude,string varBoxOfficePhone,string varBoxOfficePhoneExt,string varBoxOfficeNotes,string varMainPhone,string varMainPhoneExt,string varNotes,DateTime varDtStamp,Guid varApplicationId)
		{
			Venue item = new Venue();
			
				item.Id = varId;
			
				item.Name = varName;
			
				item.NameRoot = varNameRoot;
			
				item.DisplayName = varDisplayName;
			
				item.ICapacity = varICapacity;
			
				item.PictureUrl = varPictureUrl;
			
				item.IPicWidth = varIPicWidth;
			
				item.IPicHeight = varIPicHeight;
			
				item.WebsiteUrl = varWebsiteUrl;
			
				item.ShortAddress = varShortAddress;
			
				item.Address = varAddress;
			
				item.City = varCity;
			
				item.State = varState;
			
				item.ZipCode = varZipCode;
			
				item.Country = varCountry;
			
				item.Latitude = varLatitude;
			
				item.Longitude = varLongitude;
			
				item.BoxOfficePhone = varBoxOfficePhone;
			
				item.BoxOfficePhoneExt = varBoxOfficePhoneExt;
			
				item.BoxOfficeNotes = varBoxOfficeNotes;
			
				item.MainPhone = varMainPhone;
			
				item.MainPhoneExt = varMainPhoneExt;
			
				item.Notes = varNotes;
			
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
        
        
        
        public static TableSchema.TableColumn ICapacityColumn
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
        
        
        
        public static TableSchema.TableColumn WebsiteUrlColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn ShortAddressColumn
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        public static TableSchema.TableColumn AddressColumn
        {
            get { return Schema.Columns[10]; }
        }
        
        
        
        public static TableSchema.TableColumn CityColumn
        {
            get { return Schema.Columns[11]; }
        }
        
        
        
        public static TableSchema.TableColumn StateColumn
        {
            get { return Schema.Columns[12]; }
        }
        
        
        
        public static TableSchema.TableColumn ZipCodeColumn
        {
            get { return Schema.Columns[13]; }
        }
        
        
        
        public static TableSchema.TableColumn CountryColumn
        {
            get { return Schema.Columns[14]; }
        }
        
        
        
        public static TableSchema.TableColumn LatitudeColumn
        {
            get { return Schema.Columns[15]; }
        }
        
        
        
        public static TableSchema.TableColumn LongitudeColumn
        {
            get { return Schema.Columns[16]; }
        }
        
        
        
        public static TableSchema.TableColumn BoxOfficePhoneColumn
        {
            get { return Schema.Columns[17]; }
        }
        
        
        
        public static TableSchema.TableColumn BoxOfficePhoneExtColumn
        {
            get { return Schema.Columns[18]; }
        }
        
        
        
        public static TableSchema.TableColumn BoxOfficeNotesColumn
        {
            get { return Schema.Columns[19]; }
        }
        
        
        
        public static TableSchema.TableColumn MainPhoneColumn
        {
            get { return Schema.Columns[20]; }
        }
        
        
        
        public static TableSchema.TableColumn MainPhoneExtColumn
        {
            get { return Schema.Columns[21]; }
        }
        
        
        
        public static TableSchema.TableColumn NotesColumn
        {
            get { return Schema.Columns[22]; }
        }
        
        
        
        public static TableSchema.TableColumn DtStampColumn
        {
            get { return Schema.Columns[23]; }
        }
        
        
        
        public static TableSchema.TableColumn ApplicationIdColumn
        {
            get { return Schema.Columns[24]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Id = @"Id";
			 public static string Name = @"Name";
			 public static string NameRoot = @"NameRoot";
			 public static string DisplayName = @"DisplayName";
			 public static string ICapacity = @"iCapacity";
			 public static string PictureUrl = @"PictureUrl";
			 public static string IPicWidth = @"iPicWidth";
			 public static string IPicHeight = @"iPicHeight";
			 public static string WebsiteUrl = @"WebsiteUrl";
			 public static string ShortAddress = @"ShortAddress";
			 public static string Address = @"Address";
			 public static string City = @"City";
			 public static string State = @"State";
			 public static string ZipCode = @"ZipCode";
			 public static string Country = @"Country";
			 public static string Latitude = @"Latitude";
			 public static string Longitude = @"Longitude";
			 public static string BoxOfficePhone = @"BoxOfficePhone";
			 public static string BoxOfficePhoneExt = @"BoxOfficePhoneExt";
			 public static string BoxOfficeNotes = @"BoxOfficeNotes";
			 public static string MainPhone = @"MainPhone";
			 public static string MainPhoneExt = @"MainPhoneExt";
			 public static string Notes = @"Notes";
			 public static string DtStamp = @"dtStamp";
			 public static string ApplicationId = @"ApplicationId";
						
		}
		#endregion
		
		#region Update PK Collections
		
        public void SetPKValues()
        {
                if (colShowRecords != null)
                {
                    foreach (Wcss.Show item in colShowRecords)
                    {
                        if (item.TVenueId != Id)
                        {
                            item.TVenueId = Id;
                        }
                    }
               }
		}
        #endregion
    
        #region Deep Save
		
        public void DeepSave()
        {
            Save();
            
                if (colShowRecords != null)
                {
                    colShowRecords.SaveAll();
               }
		}
        #endregion
	}
}
