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
	/// Strongly-typed collection for the MerchBundle class.
	/// </summary>
    [Serializable]
	public partial class MerchBundleCollection : ActiveList<MerchBundle, MerchBundleCollection>
	{	   
		public MerchBundleCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>MerchBundleCollection</returns>
		public MerchBundleCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                MerchBundle o = this[i];
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
	/// This is an ActiveRecord class which wraps the MerchBundle table.
	/// </summary>
	[Serializable]
	public partial class MerchBundle : ActiveRecord<MerchBundle>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public MerchBundle()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public MerchBundle(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public MerchBundle(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public MerchBundle(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("MerchBundle", TableType.Table, DataService.GetInstance("WillCall"));
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
				
				TableSchema.TableColumn colvarTMerchId = new TableSchema.TableColumn(schema);
				colvarTMerchId.ColumnName = "TMerchId";
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
				
				TableSchema.TableColumn colvarTitle = new TableSchema.TableColumn(schema);
				colvarTitle.ColumnName = "Title";
				colvarTitle.DataType = DbType.AnsiString;
				colvarTitle.MaxLength = 256;
				colvarTitle.AutoIncrement = false;
				colvarTitle.IsNullable = false;
				colvarTitle.IsPrimaryKey = false;
				colvarTitle.IsForeignKey = false;
				colvarTitle.IsReadOnly = false;
				colvarTitle.DefaultSetting = @"";
				colvarTitle.ForeignKeyTableName = "";
				schema.Columns.Add(colvarTitle);
				
				TableSchema.TableColumn colvarComment = new TableSchema.TableColumn(schema);
				colvarComment.ColumnName = "Comment";
				colvarComment.DataType = DbType.AnsiString;
				colvarComment.MaxLength = 500;
				colvarComment.AutoIncrement = false;
				colvarComment.IsNullable = true;
				colvarComment.IsPrimaryKey = false;
				colvarComment.IsForeignKey = false;
				colvarComment.IsReadOnly = false;
				colvarComment.DefaultSetting = @"";
				colvarComment.ForeignKeyTableName = "";
				schema.Columns.Add(colvarComment);
				
				TableSchema.TableColumn colvarIRequiredParentQty = new TableSchema.TableColumn(schema);
				colvarIRequiredParentQty.ColumnName = "iRequiredParentQty";
				colvarIRequiredParentQty.DataType = DbType.Int32;
				colvarIRequiredParentQty.MaxLength = 0;
				colvarIRequiredParentQty.AutoIncrement = false;
				colvarIRequiredParentQty.IsNullable = false;
				colvarIRequiredParentQty.IsPrimaryKey = false;
				colvarIRequiredParentQty.IsForeignKey = false;
				colvarIRequiredParentQty.IsReadOnly = false;
				
						colvarIRequiredParentQty.DefaultSetting = @"((1))";
				colvarIRequiredParentQty.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIRequiredParentQty);
				
				TableSchema.TableColumn colvarIMaxSelections = new TableSchema.TableColumn(schema);
				colvarIMaxSelections.ColumnName = "iMaxSelections";
				colvarIMaxSelections.DataType = DbType.Int32;
				colvarIMaxSelections.MaxLength = 0;
				colvarIMaxSelections.AutoIncrement = false;
				colvarIMaxSelections.IsNullable = false;
				colvarIMaxSelections.IsPrimaryKey = false;
				colvarIMaxSelections.IsForeignKey = false;
				colvarIMaxSelections.IsReadOnly = false;
				
						colvarIMaxSelections.DefaultSetting = @"((1))";
				colvarIMaxSelections.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIMaxSelections);
				
				TableSchema.TableColumn colvarMPrice = new TableSchema.TableColumn(schema);
				colvarMPrice.ColumnName = "mPrice";
				colvarMPrice.DataType = DbType.Currency;
				colvarMPrice.MaxLength = 0;
				colvarMPrice.AutoIncrement = false;
				colvarMPrice.IsNullable = false;
				colvarMPrice.IsPrimaryKey = false;
				colvarMPrice.IsForeignKey = false;
				colvarMPrice.IsReadOnly = false;
				
						colvarMPrice.DefaultSetting = @"((0.0))";
				colvarMPrice.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMPrice);
				
				TableSchema.TableColumn colvarBPricedPerSelection = new TableSchema.TableColumn(schema);
				colvarBPricedPerSelection.ColumnName = "bPricedPerSelection";
				colvarBPricedPerSelection.DataType = DbType.Boolean;
				colvarBPricedPerSelection.MaxLength = 0;
				colvarBPricedPerSelection.AutoIncrement = false;
				colvarBPricedPerSelection.IsNullable = true;
				colvarBPricedPerSelection.IsPrimaryKey = false;
				colvarBPricedPerSelection.IsForeignKey = false;
				colvarBPricedPerSelection.IsReadOnly = false;
				colvarBPricedPerSelection.DefaultSetting = @"";
				colvarBPricedPerSelection.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBPricedPerSelection);
				
				TableSchema.TableColumn colvarBIncludeWeight = new TableSchema.TableColumn(schema);
				colvarBIncludeWeight.ColumnName = "bIncludeWeight";
				colvarBIncludeWeight.DataType = DbType.Boolean;
				colvarBIncludeWeight.MaxLength = 0;
				colvarBIncludeWeight.AutoIncrement = false;
				colvarBIncludeWeight.IsNullable = false;
				colvarBIncludeWeight.IsPrimaryKey = false;
				colvarBIncludeWeight.IsForeignKey = false;
				colvarBIncludeWeight.IsReadOnly = false;
				
						colvarBIncludeWeight.DefaultSetting = @"((0))";
				colvarBIncludeWeight.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBIncludeWeight);
				
				TableSchema.TableColumn colvarTShowTicketId = new TableSchema.TableColumn(schema);
				colvarTShowTicketId.ColumnName = "TShowTicketId";
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
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["WillCall"].AddSchema("MerchBundle",schema);
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
		  
		[XmlAttribute("IDisplayOrder")]
		[Bindable(true)]
		public int IDisplayOrder 
		{
			get { return GetColumnValue<int>(Columns.IDisplayOrder); }
			set { SetColumnValue(Columns.IDisplayOrder, value); }
		}
		  
		[XmlAttribute("TMerchId")]
		[Bindable(true)]
		public int? TMerchId 
		{
			get { return GetColumnValue<int?>(Columns.TMerchId); }
			set { SetColumnValue(Columns.TMerchId, value); }
		}
		  
		[XmlAttribute("Title")]
		[Bindable(true)]
		public string Title 
		{
			get { return GetColumnValue<string>(Columns.Title); }
			set { SetColumnValue(Columns.Title, value); }
		}
		  
		[XmlAttribute("Comment")]
		[Bindable(true)]
		public string Comment 
		{
			get { return GetColumnValue<string>(Columns.Comment); }
			set { SetColumnValue(Columns.Comment, value); }
		}
		  
		[XmlAttribute("IRequiredParentQty")]
		[Bindable(true)]
		public int IRequiredParentQty 
		{
			get { return GetColumnValue<int>(Columns.IRequiredParentQty); }
			set { SetColumnValue(Columns.IRequiredParentQty, value); }
		}
		  
		[XmlAttribute("IMaxSelections")]
		[Bindable(true)]
		public int IMaxSelections 
		{
			get { return GetColumnValue<int>(Columns.IMaxSelections); }
			set { SetColumnValue(Columns.IMaxSelections, value); }
		}
		  
		[XmlAttribute("MPrice")]
		[Bindable(true)]
		public decimal MPrice 
		{
			get { return GetColumnValue<decimal>(Columns.MPrice); }
			set { SetColumnValue(Columns.MPrice, value); }
		}
		  
		[XmlAttribute("BPricedPerSelection")]
		[Bindable(true)]
		public bool? BPricedPerSelection 
		{
			get { return GetColumnValue<bool?>(Columns.BPricedPerSelection); }
			set { SetColumnValue(Columns.BPricedPerSelection, value); }
		}
		  
		[XmlAttribute("BIncludeWeight")]
		[Bindable(true)]
		public bool BIncludeWeight 
		{
			get { return GetColumnValue<bool>(Columns.BIncludeWeight); }
			set { SetColumnValue(Columns.BIncludeWeight, value); }
		}
		  
		[XmlAttribute("TShowTicketId")]
		[Bindable(true)]
		public int? TShowTicketId 
		{
			get { return GetColumnValue<int?>(Columns.TShowTicketId); }
			set { SetColumnValue(Columns.TShowTicketId, value); }
		}
		
		#endregion
		
		
		#region PrimaryKey Methods		
		
        protected override void SetPrimaryKey(object oValue)
        {
            base.SetPrimaryKey(oValue);
            
            SetPKValues();
        }
        
		
		private Wcss.MerchBundleItemCollection colMerchBundleItemRecords;
		public Wcss.MerchBundleItemCollection MerchBundleItemRecords()
		{
			if(colMerchBundleItemRecords == null)
			{
				colMerchBundleItemRecords = new Wcss.MerchBundleItemCollection().Where(MerchBundleItem.Columns.TMerchBundleId, Id).Load();
				colMerchBundleItemRecords.ListChanged += new ListChangedEventHandler(colMerchBundleItemRecords_ListChanged);
			}
			return colMerchBundleItemRecords;
		}
				
		void colMerchBundleItemRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colMerchBundleItemRecords[e.NewIndex].TMerchBundleId = Id;
				colMerchBundleItemRecords.ListChanged += new ListChangedEventHandler(colMerchBundleItemRecords_ListChanged);
            }
		}
		#endregion
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a Merch ActiveRecord object related to this MerchBundle
		/// 
		/// </summary>
		private Wcss.Merch Merch
		{
			get { return Wcss.Merch.FetchByID(this.TMerchId); }
			set { SetColumnValue("TMerchId", value.Id); }
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
                
                SetColumnValue("TMerchId", value.Id);
                _merchrecord.CopyFrom(value);                
            }
		}
		
		
		/// <summary>
		/// Returns a ShowTicket ActiveRecord object related to this MerchBundle
		/// 
		/// </summary>
		private Wcss.ShowTicket ShowTicket
		{
			get { return Wcss.ShowTicket.FetchByID(this.TShowTicketId); }
			set { SetColumnValue("TShowTicketId", value.Id); }
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
                
                SetColumnValue("TShowTicketId", value.Id);
                _showticketrecord.CopyFrom(value);                
            }
		}
		
		
		#endregion
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(DateTime varDtStamp,bool varBActive,int varIDisplayOrder,int? varTMerchId,string varTitle,string varComment,int varIRequiredParentQty,int varIMaxSelections,decimal varMPrice,bool? varBPricedPerSelection,bool varBIncludeWeight,int? varTShowTicketId)
		{
			MerchBundle item = new MerchBundle();
			
			item.DtStamp = varDtStamp;
			
			item.BActive = varBActive;
			
			item.IDisplayOrder = varIDisplayOrder;
			
			item.TMerchId = varTMerchId;
			
			item.Title = varTitle;
			
			item.Comment = varComment;
			
			item.IRequiredParentQty = varIRequiredParentQty;
			
			item.IMaxSelections = varIMaxSelections;
			
			item.MPrice = varMPrice;
			
			item.BPricedPerSelection = varBPricedPerSelection;
			
			item.BIncludeWeight = varBIncludeWeight;
			
			item.TShowTicketId = varTShowTicketId;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varId,DateTime varDtStamp,bool varBActive,int varIDisplayOrder,int? varTMerchId,string varTitle,string varComment,int varIRequiredParentQty,int varIMaxSelections,decimal varMPrice,bool? varBPricedPerSelection,bool varBIncludeWeight,int? varTShowTicketId)
		{
			MerchBundle item = new MerchBundle();
			
				item.Id = varId;
			
				item.DtStamp = varDtStamp;
			
				item.BActive = varBActive;
			
				item.IDisplayOrder = varIDisplayOrder;
			
				item.TMerchId = varTMerchId;
			
				item.Title = varTitle;
			
				item.Comment = varComment;
			
				item.IRequiredParentQty = varIRequiredParentQty;
			
				item.IMaxSelections = varIMaxSelections;
			
				item.MPrice = varMPrice;
			
				item.BPricedPerSelection = varBPricedPerSelection;
			
				item.BIncludeWeight = varBIncludeWeight;
			
				item.TShowTicketId = varTShowTicketId;
			
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
        
        
        
        public static TableSchema.TableColumn IDisplayOrderColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn TMerchIdColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn TitleColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn CommentColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn IRequiredParentQtyColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn IMaxSelectionsColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn MPriceColumn
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        public static TableSchema.TableColumn BPricedPerSelectionColumn
        {
            get { return Schema.Columns[10]; }
        }
        
        
        
        public static TableSchema.TableColumn BIncludeWeightColumn
        {
            get { return Schema.Columns[11]; }
        }
        
        
        
        public static TableSchema.TableColumn TShowTicketIdColumn
        {
            get { return Schema.Columns[12]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Id = @"Id";
			 public static string DtStamp = @"dtStamp";
			 public static string BActive = @"bActive";
			 public static string IDisplayOrder = @"iDisplayOrder";
			 public static string TMerchId = @"TMerchId";
			 public static string Title = @"Title";
			 public static string Comment = @"Comment";
			 public static string IRequiredParentQty = @"iRequiredParentQty";
			 public static string IMaxSelections = @"iMaxSelections";
			 public static string MPrice = @"mPrice";
			 public static string BPricedPerSelection = @"bPricedPerSelection";
			 public static string BIncludeWeight = @"bIncludeWeight";
			 public static string TShowTicketId = @"TShowTicketId";
						
		}
		#endregion
		
		#region Update PK Collections
		
        public void SetPKValues()
        {
                if (colMerchBundleItemRecords != null)
                {
                    foreach (Wcss.MerchBundleItem item in colMerchBundleItemRecords)
                    {
                        if (item.TMerchBundleId != Id)
                        {
                            item.TMerchBundleId = Id;
                        }
                    }
               }
		}
        #endregion
    
        #region Deep Save
		
        public void DeepSave()
        {
            Save();
            
                if (colMerchBundleItemRecords != null)
                {
                    colMerchBundleItemRecords.SaveAll();
               }
		}
        #endregion
	}
}
