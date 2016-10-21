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
	/// Strongly-typed collection for the JShowAct class.
	/// </summary>
    [Serializable]
	public partial class JShowActCollection : ActiveList<JShowAct, JShowActCollection>
	{	   
		public JShowActCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>JShowActCollection</returns>
		public JShowActCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                JShowAct o = this[i];
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
	/// This is an ActiveRecord class which wraps the JShowAct table.
	/// </summary>
	[Serializable]
	public partial class JShowAct : ActiveRecord<JShowAct>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public JShowAct()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public JShowAct(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public JShowAct(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public JShowAct(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("JShowAct", TableType.Table, DataService.GetInstance("WillCall"));
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
				
				TableSchema.TableColumn colvarTActId = new TableSchema.TableColumn(schema);
				colvarTActId.ColumnName = "TActId";
				colvarTActId.DataType = DbType.Int32;
				colvarTActId.MaxLength = 0;
				colvarTActId.AutoIncrement = false;
				colvarTActId.IsNullable = false;
				colvarTActId.IsPrimaryKey = false;
				colvarTActId.IsForeignKey = true;
				colvarTActId.IsReadOnly = false;
				colvarTActId.DefaultSetting = @"";
				
					colvarTActId.ForeignKeyTableName = "Act";
				schema.Columns.Add(colvarTActId);
				
				TableSchema.TableColumn colvarTShowDateId = new TableSchema.TableColumn(schema);
				colvarTShowDateId.ColumnName = "TShowDateId";
				colvarTShowDateId.DataType = DbType.Int32;
				colvarTShowDateId.MaxLength = 0;
				colvarTShowDateId.AutoIncrement = false;
				colvarTShowDateId.IsNullable = false;
				colvarTShowDateId.IsPrimaryKey = false;
				colvarTShowDateId.IsForeignKey = true;
				colvarTShowDateId.IsReadOnly = false;
				colvarTShowDateId.DefaultSetting = @"";
				
					colvarTShowDateId.ForeignKeyTableName = "ShowDate";
				schema.Columns.Add(colvarTShowDateId);
				
				TableSchema.TableColumn colvarPreText = new TableSchema.TableColumn(schema);
				colvarPreText.ColumnName = "PreText";
				colvarPreText.DataType = DbType.AnsiString;
				colvarPreText.MaxLength = 500;
				colvarPreText.AutoIncrement = false;
				colvarPreText.IsNullable = true;
				colvarPreText.IsPrimaryKey = false;
				colvarPreText.IsForeignKey = false;
				colvarPreText.IsReadOnly = false;
				colvarPreText.DefaultSetting = @"";
				colvarPreText.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPreText);
				
				TableSchema.TableColumn colvarActText = new TableSchema.TableColumn(schema);
				colvarActText.ColumnName = "ActText";
				colvarActText.DataType = DbType.AnsiString;
				colvarActText.MaxLength = 300;
				colvarActText.AutoIncrement = false;
				colvarActText.IsNullable = true;
				colvarActText.IsPrimaryKey = false;
				colvarActText.IsForeignKey = false;
				colvarActText.IsReadOnly = false;
				colvarActText.DefaultSetting = @"";
				colvarActText.ForeignKeyTableName = "";
				schema.Columns.Add(colvarActText);
				
				TableSchema.TableColumn colvarFeaturing = new TableSchema.TableColumn(schema);
				colvarFeaturing.ColumnName = "Featuring";
				colvarFeaturing.DataType = DbType.AnsiString;
				colvarFeaturing.MaxLength = 1000;
				colvarFeaturing.AutoIncrement = false;
				colvarFeaturing.IsNullable = true;
				colvarFeaturing.IsPrimaryKey = false;
				colvarFeaturing.IsForeignKey = false;
				colvarFeaturing.IsReadOnly = false;
				colvarFeaturing.DefaultSetting = @"";
				colvarFeaturing.ForeignKeyTableName = "";
				schema.Columns.Add(colvarFeaturing);
				
				TableSchema.TableColumn colvarPostText = new TableSchema.TableColumn(schema);
				colvarPostText.ColumnName = "PostText";
				colvarPostText.DataType = DbType.AnsiString;
				colvarPostText.MaxLength = 2000;
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
				
				TableSchema.TableColumn colvarBTopBilling = new TableSchema.TableColumn(schema);
				colvarBTopBilling.ColumnName = "bTopBilling";
				colvarBTopBilling.DataType = DbType.Boolean;
				colvarBTopBilling.MaxLength = 0;
				colvarBTopBilling.AutoIncrement = false;
				colvarBTopBilling.IsNullable = true;
				colvarBTopBilling.IsPrimaryKey = false;
				colvarBTopBilling.IsForeignKey = false;
				colvarBTopBilling.IsReadOnly = false;
				
						colvarBTopBilling.DefaultSetting = @"((0))";
				colvarBTopBilling.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBTopBilling);
				
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
				DataService.Providers["WillCall"].AddSchema("JShowAct",schema);
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
		  
		[XmlAttribute("TActId")]
		[Bindable(true)]
		public int TActId 
		{
			get { return GetColumnValue<int>(Columns.TActId); }
			set { SetColumnValue(Columns.TActId, value); }
		}
		  
		[XmlAttribute("TShowDateId")]
		[Bindable(true)]
		public int TShowDateId 
		{
			get { return GetColumnValue<int>(Columns.TShowDateId); }
			set { SetColumnValue(Columns.TShowDateId, value); }
		}
		  
		[XmlAttribute("PreText")]
		[Bindable(true)]
		public string PreText 
		{
			get { return GetColumnValue<string>(Columns.PreText); }
			set { SetColumnValue(Columns.PreText, value); }
		}
		  
		[XmlAttribute("ActText")]
		[Bindable(true)]
		public string ActText 
		{
			get { return GetColumnValue<string>(Columns.ActText); }
			set { SetColumnValue(Columns.ActText, value); }
		}
		  
		[XmlAttribute("Featuring")]
		[Bindable(true)]
		public string Featuring 
		{
			get { return GetColumnValue<string>(Columns.Featuring); }
			set { SetColumnValue(Columns.Featuring, value); }
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
		  
		[XmlAttribute("BTopBilling")]
		[Bindable(true)]
		public bool? BTopBilling 
		{
			get { return GetColumnValue<bool?>(Columns.BTopBilling); }
			set { SetColumnValue(Columns.BTopBilling, value); }
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
		/// Returns a Act ActiveRecord object related to this JShowAct
		/// 
		/// </summary>
		private Wcss.Act Act
		{
			get { return Wcss.Act.FetchByID(this.TActId); }
			set { SetColumnValue("TActId", value.Id); }
		}
        //set up an alternate mechanism to avoid a database call
		private Wcss.Act _actrecord = null;
		
		public Wcss.Act ActRecord
		{
		    get
            {
                if (_actrecord == null)
                {
                    _actrecord = new Wcss.Act();
                    _actrecord.CopyFrom(this.Act);
                }
                return _actrecord;
            }
            set
            {
                if(value != null && _actrecord == null)
			        _actrecord = new Wcss.Act();
                
                SetColumnValue("TActId", value.Id);
                _actrecord.CopyFrom(value);                
            }
		}
		
		
		/// <summary>
		/// Returns a ShowDate ActiveRecord object related to this JShowAct
		/// 
		/// </summary>
		private Wcss.ShowDate ShowDate
		{
			get { return Wcss.ShowDate.FetchByID(this.TShowDateId); }
			set { SetColumnValue("TShowDateId", value.Id); }
		}
        //set up an alternate mechanism to avoid a database call
		private Wcss.ShowDate _showdaterecord = null;
		
		public Wcss.ShowDate ShowDateRecord
		{
		    get
            {
                if (_showdaterecord == null)
                {
                    _showdaterecord = new Wcss.ShowDate();
                    _showdaterecord.CopyFrom(this.ShowDate);
                }
                return _showdaterecord;
            }
            set
            {
                if(value != null && _showdaterecord == null)
			        _showdaterecord = new Wcss.ShowDate();
                
                SetColumnValue("TShowDateId", value.Id);
                _showdaterecord.CopyFrom(value);                
            }
		}
		
		
		#endregion
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(int varTActId,int varTShowDateId,string varPreText,string varActText,string varFeaturing,string varPostText,int varIDisplayOrder,bool? varBTopBilling,DateTime varDtStamp)
		{
			JShowAct item = new JShowAct();
			
			item.TActId = varTActId;
			
			item.TShowDateId = varTShowDateId;
			
			item.PreText = varPreText;
			
			item.ActText = varActText;
			
			item.Featuring = varFeaturing;
			
			item.PostText = varPostText;
			
			item.IDisplayOrder = varIDisplayOrder;
			
			item.BTopBilling = varBTopBilling;
			
			item.DtStamp = varDtStamp;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varId,int varTActId,int varTShowDateId,string varPreText,string varActText,string varFeaturing,string varPostText,int varIDisplayOrder,bool? varBTopBilling,DateTime varDtStamp)
		{
			JShowAct item = new JShowAct();
			
				item.Id = varId;
			
				item.TActId = varTActId;
			
				item.TShowDateId = varTShowDateId;
			
				item.PreText = varPreText;
			
				item.ActText = varActText;
			
				item.Featuring = varFeaturing;
			
				item.PostText = varPostText;
			
				item.IDisplayOrder = varIDisplayOrder;
			
				item.BTopBilling = varBTopBilling;
			
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
        
        
        
        public static TableSchema.TableColumn TActIdColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn TShowDateIdColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn PreTextColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn ActTextColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn FeaturingColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn PostTextColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn IDisplayOrderColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn BTopBillingColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn DtStampColumn
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Id = @"Id";
			 public static string TActId = @"TActId";
			 public static string TShowDateId = @"TShowDateId";
			 public static string PreText = @"PreText";
			 public static string ActText = @"ActText";
			 public static string Featuring = @"Featuring";
			 public static string PostText = @"PostText";
			 public static string IDisplayOrder = @"iDisplayOrder";
			 public static string BTopBilling = @"bTopBilling";
			 public static string DtStamp = @"dtStamp";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
