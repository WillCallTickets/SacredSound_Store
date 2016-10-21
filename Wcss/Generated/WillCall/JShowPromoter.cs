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
	/// Strongly-typed collection for the JShowPromoter class.
	/// </summary>
    [Serializable]
	public partial class JShowPromoterCollection : ActiveList<JShowPromoter, JShowPromoterCollection>
	{	   
		public JShowPromoterCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>JShowPromoterCollection</returns>
		public JShowPromoterCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                JShowPromoter o = this[i];
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
	/// This is an ActiveRecord class which wraps the JShowPromoter table.
	/// </summary>
	[Serializable]
	public partial class JShowPromoter : ActiveRecord<JShowPromoter>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public JShowPromoter()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public JShowPromoter(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public JShowPromoter(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public JShowPromoter(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("JShowPromoter", TableType.Table, DataService.GetInstance("WillCall"));
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
				
				TableSchema.TableColumn colvarTPromoterId = new TableSchema.TableColumn(schema);
				colvarTPromoterId.ColumnName = "TPromoterId";
				colvarTPromoterId.DataType = DbType.Int32;
				colvarTPromoterId.MaxLength = 0;
				colvarTPromoterId.AutoIncrement = false;
				colvarTPromoterId.IsNullable = false;
				colvarTPromoterId.IsPrimaryKey = false;
				colvarTPromoterId.IsForeignKey = true;
				colvarTPromoterId.IsReadOnly = false;
				colvarTPromoterId.DefaultSetting = @"";
				
					colvarTPromoterId.ForeignKeyTableName = "Promoter";
				schema.Columns.Add(colvarTPromoterId);
				
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
				
				TableSchema.TableColumn colvarPreText = new TableSchema.TableColumn(schema);
				colvarPreText.ColumnName = "PreText";
				colvarPreText.DataType = DbType.AnsiString;
				colvarPreText.MaxLength = 300;
				colvarPreText.AutoIncrement = false;
				colvarPreText.IsNullable = true;
				colvarPreText.IsPrimaryKey = false;
				colvarPreText.IsForeignKey = false;
				colvarPreText.IsReadOnly = false;
				colvarPreText.DefaultSetting = @"";
				colvarPreText.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPreText);
				
				TableSchema.TableColumn colvarPromoterText = new TableSchema.TableColumn(schema);
				colvarPromoterText.ColumnName = "PromoterText";
				colvarPromoterText.DataType = DbType.AnsiString;
				colvarPromoterText.MaxLength = 300;
				colvarPromoterText.AutoIncrement = false;
				colvarPromoterText.IsNullable = true;
				colvarPromoterText.IsPrimaryKey = false;
				colvarPromoterText.IsForeignKey = false;
				colvarPromoterText.IsReadOnly = false;
				colvarPromoterText.DefaultSetting = @"";
				colvarPromoterText.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPromoterText);
				
				TableSchema.TableColumn colvarPostText = new TableSchema.TableColumn(schema);
				colvarPostText.ColumnName = "PostText";
				colvarPostText.DataType = DbType.AnsiString;
				colvarPostText.MaxLength = 300;
				colvarPostText.AutoIncrement = false;
				colvarPostText.IsNullable = true;
				colvarPostText.IsPrimaryKey = false;
				colvarPostText.IsForeignKey = false;
				colvarPostText.IsReadOnly = false;
				colvarPostText.DefaultSetting = @"";
				colvarPostText.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPostText);
				
				TableSchema.TableColumn colvarIDisplayOrder = new TableSchema.TableColumn(schema);
				colvarIDisplayOrder.ColumnName = "iDisplayOrder";
				colvarIDisplayOrder.DataType = DbType.Int32;
				colvarIDisplayOrder.MaxLength = 0;
				colvarIDisplayOrder.AutoIncrement = false;
				colvarIDisplayOrder.IsNullable = false;
				colvarIDisplayOrder.IsPrimaryKey = false;
				colvarIDisplayOrder.IsForeignKey = false;
				colvarIDisplayOrder.IsReadOnly = false;
				colvarIDisplayOrder.DefaultSetting = @"";
				colvarIDisplayOrder.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIDisplayOrder);
				
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
				DataService.Providers["WillCall"].AddSchema("JShowPromoter",schema);
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
		  
		[XmlAttribute("TPromoterId")]
		[Bindable(true)]
		public int TPromoterId 
		{
			get { return GetColumnValue<int>(Columns.TPromoterId); }
			set { SetColumnValue(Columns.TPromoterId, value); }
		}
		  
		[XmlAttribute("TShowId")]
		[Bindable(true)]
		public int TShowId 
		{
			get { return GetColumnValue<int>(Columns.TShowId); }
			set { SetColumnValue(Columns.TShowId, value); }
		}
		  
		[XmlAttribute("PreText")]
		[Bindable(true)]
		public string PreText 
		{
			get { return GetColumnValue<string>(Columns.PreText); }
			set { SetColumnValue(Columns.PreText, value); }
		}
		  
		[XmlAttribute("PromoterText")]
		[Bindable(true)]
		public string PromoterText 
		{
			get { return GetColumnValue<string>(Columns.PromoterText); }
			set { SetColumnValue(Columns.PromoterText, value); }
		}
		  
		[XmlAttribute("PostText")]
		[Bindable(true)]
		public string PostText 
		{
			get { return GetColumnValue<string>(Columns.PostText); }
			set { SetColumnValue(Columns.PostText, value); }
		}
		  
		[XmlAttribute("IDisplayOrder")]
		[Bindable(true)]
		public int IDisplayOrder 
		{
			get { return GetColumnValue<int>(Columns.IDisplayOrder); }
			set { SetColumnValue(Columns.IDisplayOrder, value); }
		}
		  
		[XmlAttribute("DtStamp")]
		[Bindable(true)]
		public DateTime DtStamp 
		{
			get { return GetColumnValue<DateTime>(Columns.DtStamp); }
			set { SetColumnValue(Columns.DtStamp, value); }
		}
		
		#endregion
		
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a Promoter ActiveRecord object related to this JShowPromoter
		/// 
		/// </summary>
		private Wcss.Promoter Promoter
		{
			get { return Wcss.Promoter.FetchByID(this.TPromoterId); }
			set { SetColumnValue("TPromoterId", value.Id); }
		}
        //set up an alternate mechanism to avoid a database call
		private Wcss.Promoter _promoterrecord = null;
		
		public Wcss.Promoter PromoterRecord
		{
		    get
            {
                if (_promoterrecord == null)
                {
                    _promoterrecord = new Wcss.Promoter();
                    _promoterrecord.CopyFrom(this.Promoter);
                }
                return _promoterrecord;
            }
            set
            {
                if(value != null && _promoterrecord == null)
			        _promoterrecord = new Wcss.Promoter();
                
                SetColumnValue("TPromoterId", value.Id);
                _promoterrecord.CopyFrom(value);                
            }
		}
		
		
		/// <summary>
		/// Returns a Show ActiveRecord object related to this JShowPromoter
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
		
		
		#endregion
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(int varTPromoterId,int varTShowId,string varPreText,string varPromoterText,string varPostText,int varIDisplayOrder,DateTime varDtStamp)
		{
			JShowPromoter item = new JShowPromoter();
			
			item.TPromoterId = varTPromoterId;
			
			item.TShowId = varTShowId;
			
			item.PreText = varPreText;
			
			item.PromoterText = varPromoterText;
			
			item.PostText = varPostText;
			
			item.IDisplayOrder = varIDisplayOrder;
			
			item.DtStamp = varDtStamp;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varId,int varTPromoterId,int varTShowId,string varPreText,string varPromoterText,string varPostText,int varIDisplayOrder,DateTime varDtStamp)
		{
			JShowPromoter item = new JShowPromoter();
			
				item.Id = varId;
			
				item.TPromoterId = varTPromoterId;
			
				item.TShowId = varTShowId;
			
				item.PreText = varPreText;
			
				item.PromoterText = varPromoterText;
			
				item.PostText = varPostText;
			
				item.IDisplayOrder = varIDisplayOrder;
			
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
        
        
        
        public static TableSchema.TableColumn TPromoterIdColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn TShowIdColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn PreTextColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn PromoterTextColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn PostTextColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn IDisplayOrderColumn
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
			 public static string TPromoterId = @"TPromoterId";
			 public static string TShowId = @"TShowId";
			 public static string PreText = @"PreText";
			 public static string PromoterText = @"PromoterText";
			 public static string PostText = @"PostText";
			 public static string IDisplayOrder = @"iDisplayOrder";
			 public static string DtStamp = @"dtStamp";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
