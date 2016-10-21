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
	/// Strongly-typed collection for the ShowDate class.
	/// </summary>
    [Serializable]
	public partial class ShowDateCollection : ActiveList<ShowDate, ShowDateCollection>
	{	   
		public ShowDateCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>ShowDateCollection</returns>
		public ShowDateCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                ShowDate o = this[i];
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
	/// This is an ActiveRecord class which wraps the ShowDate table.
	/// </summary>
	[Serializable]
	public partial class ShowDate : ActiveRecord<ShowDate>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public ShowDate()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public ShowDate(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public ShowDate(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public ShowDate(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("ShowDate", TableType.Table, DataService.GetInstance("WillCall"));
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
				
				TableSchema.TableColumn colvarDtDateOfShow = new TableSchema.TableColumn(schema);
				colvarDtDateOfShow.ColumnName = "dtDateOfShow";
				colvarDtDateOfShow.DataType = DbType.DateTime;
				colvarDtDateOfShow.MaxLength = 0;
				colvarDtDateOfShow.AutoIncrement = false;
				colvarDtDateOfShow.IsNullable = false;
				colvarDtDateOfShow.IsPrimaryKey = false;
				colvarDtDateOfShow.IsForeignKey = false;
				colvarDtDateOfShow.IsReadOnly = false;
				colvarDtDateOfShow.DefaultSetting = @"";
				colvarDtDateOfShow.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDtDateOfShow);
				
				TableSchema.TableColumn colvarShowTime = new TableSchema.TableColumn(schema);
				colvarShowTime.ColumnName = "ShowTime";
				colvarShowTime.DataType = DbType.AnsiString;
				colvarShowTime.MaxLength = 50;
				colvarShowTime.AutoIncrement = false;
				colvarShowTime.IsNullable = true;
				colvarShowTime.IsPrimaryKey = false;
				colvarShowTime.IsForeignKey = false;
				colvarShowTime.IsReadOnly = false;
				colvarShowTime.DefaultSetting = @"";
				colvarShowTime.ForeignKeyTableName = "";
				schema.Columns.Add(colvarShowTime);
				
				TableSchema.TableColumn colvarBLateNightShow = new TableSchema.TableColumn(schema);
				colvarBLateNightShow.ColumnName = "bLateNightShow";
				colvarBLateNightShow.DataType = DbType.Boolean;
				colvarBLateNightShow.MaxLength = 0;
				colvarBLateNightShow.AutoIncrement = false;
				colvarBLateNightShow.IsNullable = false;
				colvarBLateNightShow.IsPrimaryKey = false;
				colvarBLateNightShow.IsForeignKey = false;
				colvarBLateNightShow.IsReadOnly = false;
				
						colvarBLateNightShow.DefaultSetting = @"((0))";
				colvarBLateNightShow.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBLateNightShow);
				
				TableSchema.TableColumn colvarTShowId = new TableSchema.TableColumn(schema);
				colvarTShowId.ColumnName = "TShowId";
				colvarTShowId.DataType = DbType.Int32;
				colvarTShowId.MaxLength = 0;
				colvarTShowId.AutoIncrement = false;
				colvarTShowId.IsNullable = false;
				colvarTShowId.IsPrimaryKey = false;
				colvarTShowId.IsForeignKey = true;
				colvarTShowId.IsReadOnly = false;
				colvarTShowId.DefaultSetting = @"";
				
					colvarTShowId.ForeignKeyTableName = "Show";
				schema.Columns.Add(colvarTShowId);
				
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
				
				TableSchema.TableColumn colvarShowDateTitle = new TableSchema.TableColumn(schema);
				colvarShowDateTitle.ColumnName = "ShowDateTitle";
				colvarShowDateTitle.DataType = DbType.AnsiString;
				colvarShowDateTitle.MaxLength = 500;
				colvarShowDateTitle.AutoIncrement = false;
				colvarShowDateTitle.IsNullable = true;
				colvarShowDateTitle.IsPrimaryKey = false;
				colvarShowDateTitle.IsForeignKey = false;
				colvarShowDateTitle.IsReadOnly = false;
				colvarShowDateTitle.DefaultSetting = @"";
				colvarShowDateTitle.ForeignKeyTableName = "";
				schema.Columns.Add(colvarShowDateTitle);
				
				TableSchema.TableColumn colvarStatusText = new TableSchema.TableColumn(schema);
				colvarStatusText.ColumnName = "StatusText";
				colvarStatusText.DataType = DbType.AnsiString;
				colvarStatusText.MaxLength = 500;
				colvarStatusText.AutoIncrement = false;
				colvarStatusText.IsNullable = true;
				colvarStatusText.IsPrimaryKey = false;
				colvarStatusText.IsForeignKey = false;
				colvarStatusText.IsReadOnly = false;
				colvarStatusText.DefaultSetting = @"";
				colvarStatusText.ForeignKeyTableName = "";
				schema.Columns.Add(colvarStatusText);
				
				TableSchema.TableColumn colvarPricingText = new TableSchema.TableColumn(schema);
				colvarPricingText.ColumnName = "PricingText";
				colvarPricingText.DataType = DbType.AnsiString;
				colvarPricingText.MaxLength = 500;
				colvarPricingText.AutoIncrement = false;
				colvarPricingText.IsNullable = true;
				colvarPricingText.IsPrimaryKey = false;
				colvarPricingText.IsForeignKey = false;
				colvarPricingText.IsReadOnly = false;
				colvarPricingText.DefaultSetting = @"";
				colvarPricingText.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPricingText);
				
				TableSchema.TableColumn colvarTicketUrl = new TableSchema.TableColumn(schema);
				colvarTicketUrl.ColumnName = "TicketUrl";
				colvarTicketUrl.DataType = DbType.AnsiString;
				colvarTicketUrl.MaxLength = 500;
				colvarTicketUrl.AutoIncrement = false;
				colvarTicketUrl.IsNullable = true;
				colvarTicketUrl.IsPrimaryKey = false;
				colvarTicketUrl.IsForeignKey = false;
				colvarTicketUrl.IsReadOnly = false;
				colvarTicketUrl.DefaultSetting = @"";
				colvarTicketUrl.ForeignKeyTableName = "";
				schema.Columns.Add(colvarTicketUrl);
				
				TableSchema.TableColumn colvarDisplayNotes = new TableSchema.TableColumn(schema);
				colvarDisplayNotes.ColumnName = "DisplayNotes";
				colvarDisplayNotes.DataType = DbType.AnsiString;
				colvarDisplayNotes.MaxLength = 1000;
				colvarDisplayNotes.AutoIncrement = false;
				colvarDisplayNotes.IsNullable = true;
				colvarDisplayNotes.IsPrimaryKey = false;
				colvarDisplayNotes.IsForeignKey = false;
				colvarDisplayNotes.IsReadOnly = false;
				colvarDisplayNotes.DefaultSetting = @"";
				colvarDisplayNotes.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDisplayNotes);
				
				TableSchema.TableColumn colvarTAgeId = new TableSchema.TableColumn(schema);
				colvarTAgeId.ColumnName = "TAgeId";
				colvarTAgeId.DataType = DbType.Int32;
				colvarTAgeId.MaxLength = 0;
				colvarTAgeId.AutoIncrement = false;
				colvarTAgeId.IsNullable = false;
				colvarTAgeId.IsPrimaryKey = false;
				colvarTAgeId.IsForeignKey = true;
				colvarTAgeId.IsReadOnly = false;
				
						colvarTAgeId.DefaultSetting = @"((10000))";
				
					colvarTAgeId.ForeignKeyTableName = "Age";
				schema.Columns.Add(colvarTAgeId);
				
				TableSchema.TableColumn colvarTStatusId = new TableSchema.TableColumn(schema);
				colvarTStatusId.ColumnName = "TStatusId";
				colvarTStatusId.DataType = DbType.Int32;
				colvarTStatusId.MaxLength = 0;
				colvarTStatusId.AutoIncrement = false;
				colvarTStatusId.IsNullable = false;
				colvarTStatusId.IsPrimaryKey = false;
				colvarTStatusId.IsForeignKey = true;
				colvarTStatusId.IsReadOnly = false;
				
						colvarTStatusId.DefaultSetting = @"((10000))";
				
					colvarTStatusId.ForeignKeyTableName = "ShowStatus";
				schema.Columns.Add(colvarTStatusId);
				
				TableSchema.TableColumn colvarBilling = new TableSchema.TableColumn(schema);
				colvarBilling.ColumnName = "Billing";
				colvarBilling.DataType = DbType.AnsiString;
				colvarBilling.MaxLength = 300;
				colvarBilling.AutoIncrement = false;
				colvarBilling.IsNullable = true;
				colvarBilling.IsPrimaryKey = false;
				colvarBilling.IsForeignKey = false;
				colvarBilling.IsReadOnly = false;
				colvarBilling.DefaultSetting = @"";
				colvarBilling.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBilling);
				
				TableSchema.TableColumn colvarBAutoBilling = new TableSchema.TableColumn(schema);
				colvarBAutoBilling.ColumnName = "bAutoBilling";
				colvarBAutoBilling.DataType = DbType.Boolean;
				colvarBAutoBilling.MaxLength = 0;
				colvarBAutoBilling.AutoIncrement = false;
				colvarBAutoBilling.IsNullable = false;
				colvarBAutoBilling.IsPrimaryKey = false;
				colvarBAutoBilling.IsForeignKey = false;
				colvarBAutoBilling.IsReadOnly = false;
				
						colvarBAutoBilling.DefaultSetting = @"((1))";
				colvarBAutoBilling.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBAutoBilling);
				
				TableSchema.TableColumn colvarBPrivateShow = new TableSchema.TableColumn(schema);
				colvarBPrivateShow.ColumnName = "bPrivateShow";
				colvarBPrivateShow.DataType = DbType.Boolean;
				colvarBPrivateShow.MaxLength = 0;
				colvarBPrivateShow.AutoIncrement = false;
				colvarBPrivateShow.IsNullable = false;
				colvarBPrivateShow.IsPrimaryKey = false;
				colvarBPrivateShow.IsForeignKey = false;
				colvarBPrivateShow.IsReadOnly = false;
				
						colvarBPrivateShow.DefaultSetting = @"((0))";
				colvarBPrivateShow.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBPrivateShow);
				
				TableSchema.TableColumn colvarBUseFbRsvp = new TableSchema.TableColumn(schema);
				colvarBUseFbRsvp.ColumnName = "bUseFbRsvp";
				colvarBUseFbRsvp.DataType = DbType.Boolean;
				colvarBUseFbRsvp.MaxLength = 0;
				colvarBUseFbRsvp.AutoIncrement = false;
				colvarBUseFbRsvp.IsNullable = false;
				colvarBUseFbRsvp.IsPrimaryKey = false;
				colvarBUseFbRsvp.IsForeignKey = false;
				colvarBUseFbRsvp.IsReadOnly = false;
				
						colvarBUseFbRsvp.DefaultSetting = @"((1))";
				colvarBUseFbRsvp.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBUseFbRsvp);
				
				TableSchema.TableColumn colvarFbRsvpUrl = new TableSchema.TableColumn(schema);
				colvarFbRsvpUrl.ColumnName = "FbRsvpUrl";
				colvarFbRsvpUrl.DataType = DbType.AnsiString;
				colvarFbRsvpUrl.MaxLength = 256;
				colvarFbRsvpUrl.AutoIncrement = false;
				colvarFbRsvpUrl.IsNullable = true;
				colvarFbRsvpUrl.IsPrimaryKey = false;
				colvarFbRsvpUrl.IsForeignKey = false;
				colvarFbRsvpUrl.IsReadOnly = false;
				colvarFbRsvpUrl.DefaultSetting = @"";
				colvarFbRsvpUrl.ForeignKeyTableName = "";
				schema.Columns.Add(colvarFbRsvpUrl);
				
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
				DataService.Providers["WillCall"].AddSchema("ShowDate",schema);
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
		  
		[XmlAttribute("DtDateOfShow")]
		[Bindable(true)]
		public DateTime DtDateOfShow 
		{
			get { return GetColumnValue<DateTime>(Columns.DtDateOfShow); }
			set { SetColumnValue(Columns.DtDateOfShow, value); }
		}
		  
		[XmlAttribute("ShowTime")]
		[Bindable(true)]
		public string ShowTime 
		{
			get { return GetColumnValue<string>(Columns.ShowTime); }
			set { SetColumnValue(Columns.ShowTime, value); }
		}
		  
		[XmlAttribute("BLateNightShow")]
		[Bindable(true)]
		public bool BLateNightShow 
		{
			get { return GetColumnValue<bool>(Columns.BLateNightShow); }
			set { SetColumnValue(Columns.BLateNightShow, value); }
		}
		  
		[XmlAttribute("TShowId")]
		[Bindable(true)]
		public int TShowId 
		{
			get { return GetColumnValue<int>(Columns.TShowId); }
			set { SetColumnValue(Columns.TShowId, value); }
		}
		  
		[XmlAttribute("BActive")]
		[Bindable(true)]
		public bool BActive 
		{
			get { return GetColumnValue<bool>(Columns.BActive); }
			set { SetColumnValue(Columns.BActive, value); }
		}
		  
		[XmlAttribute("ShowDateTitle")]
		[Bindable(true)]
		public string ShowDateTitle 
		{
			get { return GetColumnValue<string>(Columns.ShowDateTitle); }
			set { SetColumnValue(Columns.ShowDateTitle, value); }
		}
		  
		[XmlAttribute("StatusText")]
		[Bindable(true)]
		public string StatusText 
		{
			get { return GetColumnValue<string>(Columns.StatusText); }
			set { SetColumnValue(Columns.StatusText, value); }
		}
		  
		[XmlAttribute("PricingText")]
		[Bindable(true)]
		public string PricingText 
		{
			get { return GetColumnValue<string>(Columns.PricingText); }
			set { SetColumnValue(Columns.PricingText, value); }
		}
		  
		[XmlAttribute("TicketUrl")]
		[Bindable(true)]
		public string TicketUrl 
		{
			get { return GetColumnValue<string>(Columns.TicketUrl); }
			set { SetColumnValue(Columns.TicketUrl, value); }
		}
		  
		[XmlAttribute("DisplayNotes")]
		[Bindable(true)]
		public string DisplayNotes 
		{
			get { return GetColumnValue<string>(Columns.DisplayNotes); }
			set { SetColumnValue(Columns.DisplayNotes, value); }
		}
		  
		[XmlAttribute("TAgeId")]
		[Bindable(true)]
		public int TAgeId 
		{
			get { return GetColumnValue<int>(Columns.TAgeId); }
			set { SetColumnValue(Columns.TAgeId, value); }
		}
		  
		[XmlAttribute("TStatusId")]
		[Bindable(true)]
		public int TStatusId 
		{
			get { return GetColumnValue<int>(Columns.TStatusId); }
			set { SetColumnValue(Columns.TStatusId, value); }
		}
		  
		[XmlAttribute("Billing")]
		[Bindable(true)]
		public string Billing 
		{
			get { return GetColumnValue<string>(Columns.Billing); }
			set { SetColumnValue(Columns.Billing, value); }
		}
		  
		[XmlAttribute("BAutoBilling")]
		[Bindable(true)]
		public bool BAutoBilling 
		{
			get { return GetColumnValue<bool>(Columns.BAutoBilling); }
			set { SetColumnValue(Columns.BAutoBilling, value); }
		}
		  
		[XmlAttribute("BPrivateShow")]
		[Bindable(true)]
		public bool BPrivateShow 
		{
			get { return GetColumnValue<bool>(Columns.BPrivateShow); }
			set { SetColumnValue(Columns.BPrivateShow, value); }
		}
		  
		[XmlAttribute("BUseFbRsvp")]
		[Bindable(true)]
		public bool BUseFbRsvp 
		{
			get { return GetColumnValue<bool>(Columns.BUseFbRsvp); }
			set { SetColumnValue(Columns.BUseFbRsvp, value); }
		}
		  
		[XmlAttribute("FbRsvpUrl")]
		[Bindable(true)]
		public string FbRsvpUrl 
		{
			get { return GetColumnValue<string>(Columns.FbRsvpUrl); }
			set { SetColumnValue(Columns.FbRsvpUrl, value); }
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
        
		
		private Wcss.JShowActCollection colJShowActRecords;
		public Wcss.JShowActCollection JShowActRecords()
		{
			if(colJShowActRecords == null)
			{
				colJShowActRecords = new Wcss.JShowActCollection().Where(JShowAct.Columns.TShowDateId, Id).Load();
				colJShowActRecords.ListChanged += new ListChangedEventHandler(colJShowActRecords_ListChanged);
			}
			return colJShowActRecords;
		}
				
		void colJShowActRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colJShowActRecords[e.NewIndex].TShowDateId = Id;
				colJShowActRecords.ListChanged += new ListChangedEventHandler(colJShowActRecords_ListChanged);
            }
		}
		private Wcss.SalePromotionCollection colSalePromotionRecords;
		public Wcss.SalePromotionCollection SalePromotionRecords()
		{
			if(colSalePromotionRecords == null)
			{
				colSalePromotionRecords = new Wcss.SalePromotionCollection().Where(SalePromotion.Columns.TRequiredParentShowDateId, Id).Load();
				colSalePromotionRecords.ListChanged += new ListChangedEventHandler(colSalePromotionRecords_ListChanged);
			}
			return colSalePromotionRecords;
		}
				
		void colSalePromotionRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colSalePromotionRecords[e.NewIndex].TRequiredParentShowDateId = Id;
				colSalePromotionRecords.ListChanged += new ListChangedEventHandler(colSalePromotionRecords_ListChanged);
            }
		}
		private Wcss.ShowTicketCollection colShowTicketRecords;
		public Wcss.ShowTicketCollection ShowTicketRecords()
		{
			if(colShowTicketRecords == null)
			{
				colShowTicketRecords = new Wcss.ShowTicketCollection().Where(ShowTicket.Columns.TShowDateId, Id).Load();
				colShowTicketRecords.ListChanged += new ListChangedEventHandler(colShowTicketRecords_ListChanged);
			}
			return colShowTicketRecords;
		}
				
		void colShowTicketRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colShowTicketRecords[e.NewIndex].TShowDateId = Id;
				colShowTicketRecords.ListChanged += new ListChangedEventHandler(colShowTicketRecords_ListChanged);
            }
		}
		#endregion
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a Age ActiveRecord object related to this ShowDate
		/// 
		/// </summary>
		private Wcss.Age Age
		{
			get { return Wcss.Age.FetchByID(this.TAgeId); }
			set { SetColumnValue("TAgeId", value.Id); }
		}
        //set up an alternate mechanism to avoid a database call
		private Wcss.Age _agerecord = null;
		
		public Wcss.Age AgeRecord
		{
		    get
            {
                if (_agerecord == null)
                {
                    _agerecord = new Wcss.Age();
                    _agerecord.CopyFrom(this.Age);
                }
                return _agerecord;
            }
            set
            {
                if(value != null && _agerecord == null)
			        _agerecord = new Wcss.Age();
                
                SetColumnValue("TAgeId", value.Id);
                _agerecord.CopyFrom(value);                
            }
		}
		
		
		/// <summary>
		/// Returns a Show ActiveRecord object related to this ShowDate
		/// 
		/// </summary>
		private Wcss.Show Show
		{
			get { return Wcss.Show.FetchByID(this.TShowId); }
			set { SetColumnValue("TShowId", value.Id); }
		}
        //set up an alternate mechanism to avoid a database call
		private Wcss.Show _showrecord = null;
		
		public Wcss.Show ShowRecord
		{
		    get
            {
                if (_showrecord == null)
                {
                    _showrecord = new Wcss.Show();
                    _showrecord.CopyFrom(this.Show);
                }
                return _showrecord;
            }
            set
            {
                if(value != null && _showrecord == null)
			        _showrecord = new Wcss.Show();
                
                SetColumnValue("TShowId", value.Id);
                _showrecord.CopyFrom(value);                
            }
		}
		
		
		/// <summary>
		/// Returns a ShowStatus ActiveRecord object related to this ShowDate
		/// 
		/// </summary>
		private Wcss.ShowStatus ShowStatus
		{
			get { return Wcss.ShowStatus.FetchByID(this.TStatusId); }
			set { SetColumnValue("TStatusId", value.Id); }
		}
        //set up an alternate mechanism to avoid a database call
		private Wcss.ShowStatus _showstatusrecord = null;
		
		public Wcss.ShowStatus ShowStatusRecord
		{
		    get
            {
                if (_showstatusrecord == null)
                {
                    _showstatusrecord = new Wcss.ShowStatus();
                    _showstatusrecord.CopyFrom(this.ShowStatus);
                }
                return _showstatusrecord;
            }
            set
            {
                if(value != null && _showstatusrecord == null)
			        _showstatusrecord = new Wcss.ShowStatus();
                
                SetColumnValue("TStatusId", value.Id);
                _showstatusrecord.CopyFrom(value);                
            }
		}
		
		
		#endregion
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(DateTime varDtDateOfShow,string varShowTime,bool varBLateNightShow,int varTShowId,bool varBActive,string varShowDateTitle,string varStatusText,string varPricingText,string varTicketUrl,string varDisplayNotes,int varTAgeId,int varTStatusId,string varBilling,bool varBAutoBilling,bool varBPrivateShow,bool varBUseFbRsvp,string varFbRsvpUrl,DateTime varDtStamp)
		{
			ShowDate item = new ShowDate();
			
			item.DtDateOfShow = varDtDateOfShow;
			
			item.ShowTime = varShowTime;
			
			item.BLateNightShow = varBLateNightShow;
			
			item.TShowId = varTShowId;
			
			item.BActive = varBActive;
			
			item.ShowDateTitle = varShowDateTitle;
			
			item.StatusText = varStatusText;
			
			item.PricingText = varPricingText;
			
			item.TicketUrl = varTicketUrl;
			
			item.DisplayNotes = varDisplayNotes;
			
			item.TAgeId = varTAgeId;
			
			item.TStatusId = varTStatusId;
			
			item.Billing = varBilling;
			
			item.BAutoBilling = varBAutoBilling;
			
			item.BPrivateShow = varBPrivateShow;
			
			item.BUseFbRsvp = varBUseFbRsvp;
			
			item.FbRsvpUrl = varFbRsvpUrl;
			
			item.DtStamp = varDtStamp;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varId,DateTime varDtDateOfShow,string varShowTime,bool varBLateNightShow,int varTShowId,bool varBActive,string varShowDateTitle,string varStatusText,string varPricingText,string varTicketUrl,string varDisplayNotes,int varTAgeId,int varTStatusId,string varBilling,bool varBAutoBilling,bool varBPrivateShow,bool varBUseFbRsvp,string varFbRsvpUrl,DateTime varDtStamp)
		{
			ShowDate item = new ShowDate();
			
				item.Id = varId;
			
				item.DtDateOfShow = varDtDateOfShow;
			
				item.ShowTime = varShowTime;
			
				item.BLateNightShow = varBLateNightShow;
			
				item.TShowId = varTShowId;
			
				item.BActive = varBActive;
			
				item.ShowDateTitle = varShowDateTitle;
			
				item.StatusText = varStatusText;
			
				item.PricingText = varPricingText;
			
				item.TicketUrl = varTicketUrl;
			
				item.DisplayNotes = varDisplayNotes;
			
				item.TAgeId = varTAgeId;
			
				item.TStatusId = varTStatusId;
			
				item.Billing = varBilling;
			
				item.BAutoBilling = varBAutoBilling;
			
				item.BPrivateShow = varBPrivateShow;
			
				item.BUseFbRsvp = varBUseFbRsvp;
			
				item.FbRsvpUrl = varFbRsvpUrl;
			
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
        
        
        
        public static TableSchema.TableColumn DtDateOfShowColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn ShowTimeColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn BLateNightShowColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn TShowIdColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn BActiveColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn ShowDateTitleColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn StatusTextColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn PricingTextColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn TicketUrlColumn
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        public static TableSchema.TableColumn DisplayNotesColumn
        {
            get { return Schema.Columns[10]; }
        }
        
        
        
        public static TableSchema.TableColumn TAgeIdColumn
        {
            get { return Schema.Columns[11]; }
        }
        
        
        
        public static TableSchema.TableColumn TStatusIdColumn
        {
            get { return Schema.Columns[12]; }
        }
        
        
        
        public static TableSchema.TableColumn BillingColumn
        {
            get { return Schema.Columns[13]; }
        }
        
        
        
        public static TableSchema.TableColumn BAutoBillingColumn
        {
            get { return Schema.Columns[14]; }
        }
        
        
        
        public static TableSchema.TableColumn BPrivateShowColumn
        {
            get { return Schema.Columns[15]; }
        }
        
        
        
        public static TableSchema.TableColumn BUseFbRsvpColumn
        {
            get { return Schema.Columns[16]; }
        }
        
        
        
        public static TableSchema.TableColumn FbRsvpUrlColumn
        {
            get { return Schema.Columns[17]; }
        }
        
        
        
        public static TableSchema.TableColumn DtStampColumn
        {
            get { return Schema.Columns[18]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Id = @"Id";
			 public static string DtDateOfShow = @"dtDateOfShow";
			 public static string ShowTime = @"ShowTime";
			 public static string BLateNightShow = @"bLateNightShow";
			 public static string TShowId = @"TShowId";
			 public static string BActive = @"bActive";
			 public static string ShowDateTitle = @"ShowDateTitle";
			 public static string StatusText = @"StatusText";
			 public static string PricingText = @"PricingText";
			 public static string TicketUrl = @"TicketUrl";
			 public static string DisplayNotes = @"DisplayNotes";
			 public static string TAgeId = @"TAgeId";
			 public static string TStatusId = @"TStatusId";
			 public static string Billing = @"Billing";
			 public static string BAutoBilling = @"bAutoBilling";
			 public static string BPrivateShow = @"bPrivateShow";
			 public static string BUseFbRsvp = @"bUseFbRsvp";
			 public static string FbRsvpUrl = @"FbRsvpUrl";
			 public static string DtStamp = @"dtStamp";
						
		}
		#endregion
		
		#region Update PK Collections
		
        public void SetPKValues()
        {
                if (colJShowActRecords != null)
                {
                    foreach (Wcss.JShowAct item in colJShowActRecords)
                    {
                        if (item.TShowDateId != Id)
                        {
                            item.TShowDateId = Id;
                        }
                    }
               }
		
                if (colSalePromotionRecords != null)
                {
                    foreach (Wcss.SalePromotion item in colSalePromotionRecords)
                    {
                        if (item.TRequiredParentShowDateId != Id)
                        {
                            item.TRequiredParentShowDateId = Id;
                        }
                    }
               }
		
                if (colShowTicketRecords != null)
                {
                    foreach (Wcss.ShowTicket item in colShowTicketRecords)
                    {
                        if (item.TShowDateId != Id)
                        {
                            item.TShowDateId = Id;
                        }
                    }
               }
		}
        #endregion
    
        #region Deep Save
		
        public void DeepSave()
        {
            Save();
            
                if (colJShowActRecords != null)
                {
                    colJShowActRecords.SaveAll();
               }
		
                if (colSalePromotionRecords != null)
                {
                    colSalePromotionRecords.SaveAll();
               }
		
                if (colShowTicketRecords != null)
                {
                    colShowTicketRecords.SaveAll();
               }
		}
        #endregion
	}
}
