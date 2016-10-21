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
	/// Strongly-typed collection for the HeaderImage class.
	/// </summary>
    [Serializable]
	public partial class HeaderImageCollection : ActiveList<HeaderImage, HeaderImageCollection>
	{	   
		public HeaderImageCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>HeaderImageCollection</returns>
		public HeaderImageCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                HeaderImage o = this[i];
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
	/// This is an ActiveRecord class which wraps the HeaderImage table.
	/// </summary>
	[Serializable]
	public partial class HeaderImage : ActiveRecord<HeaderImage>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public HeaderImage()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public HeaderImage(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public HeaderImage(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public HeaderImage(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("HeaderImage", TableType.Table, DataService.GetInstance("WillCall"));
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
				
				TableSchema.TableColumn colvarIDisplayOrder = new TableSchema.TableColumn(schema);
				colvarIDisplayOrder.ColumnName = "iDisplayOrder";
				colvarIDisplayOrder.DataType = DbType.Int32;
				colvarIDisplayOrder.MaxLength = 0;
				colvarIDisplayOrder.AutoIncrement = false;
				colvarIDisplayOrder.IsNullable = false;
				colvarIDisplayOrder.IsPrimaryKey = false;
				colvarIDisplayOrder.IsForeignKey = false;
				colvarIDisplayOrder.IsReadOnly = false;
				
						colvarIDisplayOrder.DefaultSetting = @"((-1))";
				colvarIDisplayOrder.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIDisplayOrder);
				
				TableSchema.TableColumn colvarBDisplayPriority = new TableSchema.TableColumn(schema);
				colvarBDisplayPriority.ColumnName = "bDisplayPriority";
				colvarBDisplayPriority.DataType = DbType.Boolean;
				colvarBDisplayPriority.MaxLength = 0;
				colvarBDisplayPriority.AutoIncrement = false;
				colvarBDisplayPriority.IsNullable = false;
				colvarBDisplayPriority.IsPrimaryKey = false;
				colvarBDisplayPriority.IsForeignKey = false;
				colvarBDisplayPriority.IsReadOnly = false;
				
						colvarBDisplayPriority.DefaultSetting = @"((0))";
				colvarBDisplayPriority.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBDisplayPriority);
				
				TableSchema.TableColumn colvarBExclusive = new TableSchema.TableColumn(schema);
				colvarBExclusive.ColumnName = "bExclusive";
				colvarBExclusive.DataType = DbType.Boolean;
				colvarBExclusive.MaxLength = 0;
				colvarBExclusive.AutoIncrement = false;
				colvarBExclusive.IsNullable = false;
				colvarBExclusive.IsPrimaryKey = false;
				colvarBExclusive.IsForeignKey = false;
				colvarBExclusive.IsReadOnly = false;
				
						colvarBExclusive.DefaultSetting = @"((0))";
				colvarBExclusive.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBExclusive);
				
				TableSchema.TableColumn colvarITimeoutMsec = new TableSchema.TableColumn(schema);
				colvarITimeoutMsec.ColumnName = "iTimeoutMsec";
				colvarITimeoutMsec.DataType = DbType.Int32;
				colvarITimeoutMsec.MaxLength = 0;
				colvarITimeoutMsec.AutoIncrement = false;
				colvarITimeoutMsec.IsNullable = false;
				colvarITimeoutMsec.IsPrimaryKey = false;
				colvarITimeoutMsec.IsForeignKey = false;
				colvarITimeoutMsec.IsReadOnly = false;
				
						colvarITimeoutMsec.DefaultSetting = @"((2400))";
				colvarITimeoutMsec.ForeignKeyTableName = "";
				schema.Columns.Add(colvarITimeoutMsec);
				
				TableSchema.TableColumn colvarFileName = new TableSchema.TableColumn(schema);
				colvarFileName.ColumnName = "FileName";
				colvarFileName.DataType = DbType.AnsiString;
				colvarFileName.MaxLength = 256;
				colvarFileName.AutoIncrement = false;
				colvarFileName.IsNullable = false;
				colvarFileName.IsPrimaryKey = false;
				colvarFileName.IsForeignKey = false;
				colvarFileName.IsReadOnly = false;
				colvarFileName.DefaultSetting = @"";
				colvarFileName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarFileName);
				
				TableSchema.TableColumn colvarDisplayText = new TableSchema.TableColumn(schema);
				colvarDisplayText.ColumnName = "DisplayText";
				colvarDisplayText.DataType = DbType.AnsiString;
				colvarDisplayText.MaxLength = 1000;
				colvarDisplayText.AutoIncrement = false;
				colvarDisplayText.IsNullable = true;
				colvarDisplayText.IsPrimaryKey = false;
				colvarDisplayText.IsForeignKey = false;
				colvarDisplayText.IsReadOnly = false;
				colvarDisplayText.DefaultSetting = @"";
				colvarDisplayText.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDisplayText);
				
				TableSchema.TableColumn colvarNavigateUrl = new TableSchema.TableColumn(schema);
				colvarNavigateUrl.ColumnName = "NavigateUrl";
				colvarNavigateUrl.DataType = DbType.AnsiString;
				colvarNavigateUrl.MaxLength = 256;
				colvarNavigateUrl.AutoIncrement = false;
				colvarNavigateUrl.IsNullable = true;
				colvarNavigateUrl.IsPrimaryKey = false;
				colvarNavigateUrl.IsForeignKey = false;
				colvarNavigateUrl.IsReadOnly = false;
				colvarNavigateUrl.DefaultSetting = @"";
				colvarNavigateUrl.ForeignKeyTableName = "";
				schema.Columns.Add(colvarNavigateUrl);
				
				TableSchema.TableColumn colvarTShowId = new TableSchema.TableColumn(schema);
				colvarTShowId.ColumnName = "tShowId";
				colvarTShowId.DataType = DbType.Int32;
				colvarTShowId.MaxLength = 0;
				colvarTShowId.AutoIncrement = false;
				colvarTShowId.IsNullable = true;
				colvarTShowId.IsPrimaryKey = false;
				colvarTShowId.IsForeignKey = true;
				colvarTShowId.IsReadOnly = false;
				colvarTShowId.DefaultSetting = @"";
				
					colvarTShowId.ForeignKeyTableName = "Show";
				schema.Columns.Add(colvarTShowId);
				
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
				
				TableSchema.TableColumn colvarVcContext = new TableSchema.TableColumn(schema);
				colvarVcContext.ColumnName = "vcContext";
				colvarVcContext.DataType = DbType.AnsiString;
				colvarVcContext.MaxLength = 500;
				colvarVcContext.AutoIncrement = false;
				colvarVcContext.IsNullable = true;
				colvarVcContext.IsPrimaryKey = false;
				colvarVcContext.IsForeignKey = false;
				colvarVcContext.IsReadOnly = false;
				colvarVcContext.DefaultSetting = @"";
				colvarVcContext.ForeignKeyTableName = "";
				schema.Columns.Add(colvarVcContext);
				
				TableSchema.TableColumn colvarUnlockCode = new TableSchema.TableColumn(schema);
				colvarUnlockCode.ColumnName = "UnlockCode";
				colvarUnlockCode.DataType = DbType.AnsiString;
				colvarUnlockCode.MaxLength = 256;
				colvarUnlockCode.AutoIncrement = false;
				colvarUnlockCode.IsNullable = true;
				colvarUnlockCode.IsPrimaryKey = false;
				colvarUnlockCode.IsForeignKey = false;
				colvarUnlockCode.IsReadOnly = false;
				colvarUnlockCode.DefaultSetting = @"";
				colvarUnlockCode.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUnlockCode);
				
				TableSchema.TableColumn colvarDtStart = new TableSchema.TableColumn(schema);
				colvarDtStart.ColumnName = "dtStart";
				colvarDtStart.DataType = DbType.DateTime;
				colvarDtStart.MaxLength = 0;
				colvarDtStart.AutoIncrement = false;
				colvarDtStart.IsNullable = true;
				colvarDtStart.IsPrimaryKey = false;
				colvarDtStart.IsForeignKey = false;
				colvarDtStart.IsReadOnly = false;
				colvarDtStart.DefaultSetting = @"";
				colvarDtStart.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDtStart);
				
				TableSchema.TableColumn colvarDtEnd = new TableSchema.TableColumn(schema);
				colvarDtEnd.ColumnName = "dtEnd";
				colvarDtEnd.DataType = DbType.DateTime;
				colvarDtEnd.MaxLength = 0;
				colvarDtEnd.AutoIncrement = false;
				colvarDtEnd.IsNullable = true;
				colvarDtEnd.IsPrimaryKey = false;
				colvarDtEnd.IsForeignKey = false;
				colvarDtEnd.IsReadOnly = false;
				colvarDtEnd.DefaultSetting = @"";
				colvarDtEnd.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDtEnd);
				
				TableSchema.TableColumn colvarApplicationId = new TableSchema.TableColumn(schema);
				colvarApplicationId.ColumnName = "ApplicationId";
				colvarApplicationId.DataType = DbType.Guid;
				colvarApplicationId.MaxLength = 0;
				colvarApplicationId.AutoIncrement = false;
				colvarApplicationId.IsNullable = false;
				colvarApplicationId.IsPrimaryKey = false;
				colvarApplicationId.IsForeignKey = false;
				colvarApplicationId.IsReadOnly = false;
				colvarApplicationId.DefaultSetting = @"";
				colvarApplicationId.ForeignKeyTableName = "";
				schema.Columns.Add(colvarApplicationId);
				
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
				
				TableSchema.TableColumn colvarDtModified = new TableSchema.TableColumn(schema);
				colvarDtModified.ColumnName = "dtModified";
				colvarDtModified.DataType = DbType.DateTime;
				colvarDtModified.MaxLength = 0;
				colvarDtModified.AutoIncrement = false;
				colvarDtModified.IsNullable = false;
				colvarDtModified.IsPrimaryKey = false;
				colvarDtModified.IsForeignKey = false;
				colvarDtModified.IsReadOnly = false;
				
						colvarDtModified.DefaultSetting = @"(getdate())";
				colvarDtModified.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDtModified);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["WillCall"].AddSchema("HeaderImage",schema);
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
		  
		[XmlAttribute("BActive")]
		[Bindable(true)]
		public bool BActive 
		{
			get { return GetColumnValue<bool>(Columns.BActive); }
			set { SetColumnValue(Columns.BActive, value); }
		}
		  
		[XmlAttribute("IDisplayOrder")]
		[Bindable(true)]
		public int IDisplayOrder 
		{
			get { return GetColumnValue<int>(Columns.IDisplayOrder); }
			set { SetColumnValue(Columns.IDisplayOrder, value); }
		}
		  
		[XmlAttribute("BDisplayPriority")]
		[Bindable(true)]
		public bool BDisplayPriority 
		{
			get { return GetColumnValue<bool>(Columns.BDisplayPriority); }
			set { SetColumnValue(Columns.BDisplayPriority, value); }
		}
		  
		[XmlAttribute("BExclusive")]
		[Bindable(true)]
		public bool BExclusive 
		{
			get { return GetColumnValue<bool>(Columns.BExclusive); }
			set { SetColumnValue(Columns.BExclusive, value); }
		}
		  
		[XmlAttribute("ITimeoutMsec")]
		[Bindable(true)]
		public int ITimeoutMsec 
		{
			get { return GetColumnValue<int>(Columns.ITimeoutMsec); }
			set { SetColumnValue(Columns.ITimeoutMsec, value); }
		}
		  
		[XmlAttribute("FileName")]
		[Bindable(true)]
		public string FileName 
		{
			get { return GetColumnValue<string>(Columns.FileName); }
			set { SetColumnValue(Columns.FileName, value); }
		}
		  
		[XmlAttribute("DisplayText")]
		[Bindable(true)]
		public string DisplayText 
		{
			get { return GetColumnValue<string>(Columns.DisplayText); }
			set { SetColumnValue(Columns.DisplayText, value); }
		}
		  
		[XmlAttribute("NavigateUrl")]
		[Bindable(true)]
		public string NavigateUrl 
		{
			get { return GetColumnValue<string>(Columns.NavigateUrl); }
			set { SetColumnValue(Columns.NavigateUrl, value); }
		}
		  
		[XmlAttribute("TShowId")]
		[Bindable(true)]
		public int? TShowId 
		{
			get { return GetColumnValue<int?>(Columns.TShowId); }
			set { SetColumnValue(Columns.TShowId, value); }
		}
		  
		[XmlAttribute("TMerchId")]
		[Bindable(true)]
		public int? TMerchId 
		{
			get { return GetColumnValue<int?>(Columns.TMerchId); }
			set { SetColumnValue(Columns.TMerchId, value); }
		}
		  
		[XmlAttribute("VcContext")]
		[Bindable(true)]
		public string VcContext 
		{
			get { return GetColumnValue<string>(Columns.VcContext); }
			set { SetColumnValue(Columns.VcContext, value); }
		}
		  
		[XmlAttribute("UnlockCode")]
		[Bindable(true)]
		public string UnlockCode 
		{
			get { return GetColumnValue<string>(Columns.UnlockCode); }
			set { SetColumnValue(Columns.UnlockCode, value); }
		}
		  
		[XmlAttribute("DtStart")]
		[Bindable(true)]
		public DateTime? DtStart 
		{
			get { return GetColumnValue<DateTime?>(Columns.DtStart); }
			set { SetColumnValue(Columns.DtStart, value); }
		}
		  
		[XmlAttribute("DtEnd")]
		[Bindable(true)]
		public DateTime? DtEnd 
		{
			get { return GetColumnValue<DateTime?>(Columns.DtEnd); }
			set { SetColumnValue(Columns.DtEnd, value); }
		}
		  
		[XmlAttribute("ApplicationId")]
		[Bindable(true)]
		public Guid ApplicationId 
		{
			get { return GetColumnValue<Guid>(Columns.ApplicationId); }
			set { SetColumnValue(Columns.ApplicationId, value); }
		}
		  
		[XmlAttribute("DtStamp")]
		[Bindable(true)]
		public DateTime DtStamp 
		{
			get { return GetColumnValue<DateTime>(Columns.DtStamp); }
			set { SetColumnValue(Columns.DtStamp, value); }
		}
		  
		[XmlAttribute("DtModified")]
		[Bindable(true)]
		public DateTime DtModified 
		{
			get { return GetColumnValue<DateTime>(Columns.DtModified); }
			set { SetColumnValue(Columns.DtModified, value); }
		}
		
		#endregion
		
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a Merch ActiveRecord object related to this HeaderImage
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
		/// Returns a Show ActiveRecord object related to this HeaderImage
		/// 
		/// </summary>
		private Wcss.Show Show
		{
			get { return Wcss.Show.FetchByID(this.TShowId); }
			set { SetColumnValue("tShowId", value.Id); }
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
                
                SetColumnValue("tShowId", value.Id);
                _showrecord.CopyFrom(value);                
            }
		}
		
		
		#endregion
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(bool varBActive,int varIDisplayOrder,bool varBDisplayPriority,bool varBExclusive,int varITimeoutMsec,string varFileName,string varDisplayText,string varNavigateUrl,int? varTShowId,int? varTMerchId,string varVcContext,string varUnlockCode,DateTime? varDtStart,DateTime? varDtEnd,Guid varApplicationId,DateTime varDtStamp,DateTime varDtModified)
		{
			HeaderImage item = new HeaderImage();
			
			item.BActive = varBActive;
			
			item.IDisplayOrder = varIDisplayOrder;
			
			item.BDisplayPriority = varBDisplayPriority;
			
			item.BExclusive = varBExclusive;
			
			item.ITimeoutMsec = varITimeoutMsec;
			
			item.FileName = varFileName;
			
			item.DisplayText = varDisplayText;
			
			item.NavigateUrl = varNavigateUrl;
			
			item.TShowId = varTShowId;
			
			item.TMerchId = varTMerchId;
			
			item.VcContext = varVcContext;
			
			item.UnlockCode = varUnlockCode;
			
			item.DtStart = varDtStart;
			
			item.DtEnd = varDtEnd;
			
			item.ApplicationId = varApplicationId;
			
			item.DtStamp = varDtStamp;
			
			item.DtModified = varDtModified;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varId,bool varBActive,int varIDisplayOrder,bool varBDisplayPriority,bool varBExclusive,int varITimeoutMsec,string varFileName,string varDisplayText,string varNavigateUrl,int? varTShowId,int? varTMerchId,string varVcContext,string varUnlockCode,DateTime? varDtStart,DateTime? varDtEnd,Guid varApplicationId,DateTime varDtStamp,DateTime varDtModified)
		{
			HeaderImage item = new HeaderImage();
			
				item.Id = varId;
			
				item.BActive = varBActive;
			
				item.IDisplayOrder = varIDisplayOrder;
			
				item.BDisplayPriority = varBDisplayPriority;
			
				item.BExclusive = varBExclusive;
			
				item.ITimeoutMsec = varITimeoutMsec;
			
				item.FileName = varFileName;
			
				item.DisplayText = varDisplayText;
			
				item.NavigateUrl = varNavigateUrl;
			
				item.TShowId = varTShowId;
			
				item.TMerchId = varTMerchId;
			
				item.VcContext = varVcContext;
			
				item.UnlockCode = varUnlockCode;
			
				item.DtStart = varDtStart;
			
				item.DtEnd = varDtEnd;
			
				item.ApplicationId = varApplicationId;
			
				item.DtStamp = varDtStamp;
			
				item.DtModified = varDtModified;
			
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
        
        
        
        public static TableSchema.TableColumn BActiveColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn IDisplayOrderColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn BDisplayPriorityColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn BExclusiveColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn ITimeoutMsecColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn FileNameColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn DisplayTextColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn NavigateUrlColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn TShowIdColumn
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        public static TableSchema.TableColumn TMerchIdColumn
        {
            get { return Schema.Columns[10]; }
        }
        
        
        
        public static TableSchema.TableColumn VcContextColumn
        {
            get { return Schema.Columns[11]; }
        }
        
        
        
        public static TableSchema.TableColumn UnlockCodeColumn
        {
            get { return Schema.Columns[12]; }
        }
        
        
        
        public static TableSchema.TableColumn DtStartColumn
        {
            get { return Schema.Columns[13]; }
        }
        
        
        
        public static TableSchema.TableColumn DtEndColumn
        {
            get { return Schema.Columns[14]; }
        }
        
        
        
        public static TableSchema.TableColumn ApplicationIdColumn
        {
            get { return Schema.Columns[15]; }
        }
        
        
        
        public static TableSchema.TableColumn DtStampColumn
        {
            get { return Schema.Columns[16]; }
        }
        
        
        
        public static TableSchema.TableColumn DtModifiedColumn
        {
            get { return Schema.Columns[17]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Id = @"Id";
			 public static string BActive = @"bActive";
			 public static string IDisplayOrder = @"iDisplayOrder";
			 public static string BDisplayPriority = @"bDisplayPriority";
			 public static string BExclusive = @"bExclusive";
			 public static string ITimeoutMsec = @"iTimeoutMsec";
			 public static string FileName = @"FileName";
			 public static string DisplayText = @"DisplayText";
			 public static string NavigateUrl = @"NavigateUrl";
			 public static string TShowId = @"tShowId";
			 public static string TMerchId = @"tMerchId";
			 public static string VcContext = @"vcContext";
			 public static string UnlockCode = @"UnlockCode";
			 public static string DtStart = @"dtStart";
			 public static string DtEnd = @"dtEnd";
			 public static string ApplicationId = @"ApplicationId";
			 public static string DtStamp = @"dtStamp";
			 public static string DtModified = @"dtModified";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
