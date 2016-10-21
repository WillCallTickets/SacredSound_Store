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
	/// Strongly-typed collection for the ShowLink class.
	/// </summary>
    [Serializable]
	public partial class ShowLinkCollection : ActiveList<ShowLink, ShowLinkCollection>
	{	   
		public ShowLinkCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>ShowLinkCollection</returns>
		public ShowLinkCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                ShowLink o = this[i];
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
	/// This is an ActiveRecord class which wraps the ShowLink table.
	/// </summary>
	[Serializable]
	public partial class ShowLink : ActiveRecord<ShowLink>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public ShowLink()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public ShowLink(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public ShowLink(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public ShowLink(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("ShowLink", TableType.Table, DataService.GetInstance("WillCall"));
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
				
				TableSchema.TableColumn colvarLinkUrl = new TableSchema.TableColumn(schema);
				colvarLinkUrl.ColumnName = "LinkUrl";
				colvarLinkUrl.DataType = DbType.AnsiString;
				colvarLinkUrl.MaxLength = 300;
				colvarLinkUrl.AutoIncrement = false;
				colvarLinkUrl.IsNullable = false;
				colvarLinkUrl.IsPrimaryKey = false;
				colvarLinkUrl.IsForeignKey = false;
				colvarLinkUrl.IsReadOnly = false;
				colvarLinkUrl.DefaultSetting = @"";
				colvarLinkUrl.ForeignKeyTableName = "";
				schema.Columns.Add(colvarLinkUrl);
				
				TableSchema.TableColumn colvarDisplayText = new TableSchema.TableColumn(schema);
				colvarDisplayText.ColumnName = "DisplayText";
				colvarDisplayText.DataType = DbType.AnsiString;
				colvarDisplayText.MaxLength = 200;
				colvarDisplayText.AutoIncrement = false;
				colvarDisplayText.IsNullable = false;
				colvarDisplayText.IsPrimaryKey = false;
				colvarDisplayText.IsForeignKey = false;
				colvarDisplayText.IsReadOnly = false;
				colvarDisplayText.DefaultSetting = @"";
				colvarDisplayText.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDisplayText);
				
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
				
						colvarIDisplayOrder.DefaultSetting = @"((0))";
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
				DataService.Providers["WillCall"].AddSchema("ShowLink",schema);
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
		  
		[XmlAttribute("LinkUrl")]
		[Bindable(true)]
		public string LinkUrl 
		{
			get { return GetColumnValue<string>(Columns.LinkUrl); }
			set { SetColumnValue(Columns.LinkUrl, value); }
		}
		  
		[XmlAttribute("DisplayText")]
		[Bindable(true)]
		public string DisplayText 
		{
			get { return GetColumnValue<string>(Columns.DisplayText); }
			set { SetColumnValue(Columns.DisplayText, value); }
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
		/// Returns a Show ActiveRecord object related to this ShowLink
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
		public static void Insert(string varLinkUrl,string varDisplayText,int varTShowId,bool varBActive,int varIDisplayOrder,DateTime varDtStamp)
		{
			ShowLink item = new ShowLink();
			
			item.LinkUrl = varLinkUrl;
			
			item.DisplayText = varDisplayText;
			
			item.TShowId = varTShowId;
			
			item.BActive = varBActive;
			
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
		public static void Update(int varId,string varLinkUrl,string varDisplayText,int varTShowId,bool varBActive,int varIDisplayOrder,DateTime varDtStamp)
		{
			ShowLink item = new ShowLink();
			
				item.Id = varId;
			
				item.LinkUrl = varLinkUrl;
			
				item.DisplayText = varDisplayText;
			
				item.TShowId = varTShowId;
			
				item.BActive = varBActive;
			
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
        
        
        
        public static TableSchema.TableColumn LinkUrlColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn DisplayTextColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn TShowIdColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn BActiveColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn IDisplayOrderColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn DtStampColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Id = @"Id";
			 public static string LinkUrl = @"LinkUrl";
			 public static string DisplayText = @"DisplayText";
			 public static string TShowId = @"TShowId";
			 public static string BActive = @"bActive";
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
