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
	/// Strongly-typed collection for the PostPurchaseText class.
	/// </summary>
    [Serializable]
	public partial class PostPurchaseTextCollection : ActiveList<PostPurchaseText, PostPurchaseTextCollection>
	{	   
		public PostPurchaseTextCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>PostPurchaseTextCollection</returns>
		public PostPurchaseTextCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                PostPurchaseText o = this[i];
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
	/// This is an ActiveRecord class which wraps the PostPurchaseText table.
	/// </summary>
	[Serializable]
	public partial class PostPurchaseText : ActiveRecord<PostPurchaseText>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public PostPurchaseText()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public PostPurchaseText(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public PostPurchaseText(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public PostPurchaseText(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("PostPurchaseText", TableType.Table, DataService.GetInstance("WillCall"));
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
				
				TableSchema.TableColumn colvarTShowTicketId = new TableSchema.TableColumn(schema);
				colvarTShowTicketId.ColumnName = "tShowTicketId";
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
				
				TableSchema.TableColumn colvarInProcessDescription = new TableSchema.TableColumn(schema);
				colvarInProcessDescription.ColumnName = "InProcessDescription";
				colvarInProcessDescription.DataType = DbType.AnsiString;
				colvarInProcessDescription.MaxLength = 1500;
				colvarInProcessDescription.AutoIncrement = false;
				colvarInProcessDescription.IsNullable = true;
				colvarInProcessDescription.IsPrimaryKey = false;
				colvarInProcessDescription.IsForeignKey = false;
				colvarInProcessDescription.IsReadOnly = false;
				colvarInProcessDescription.DefaultSetting = @"";
				colvarInProcessDescription.ForeignKeyTableName = "";
				schema.Columns.Add(colvarInProcessDescription);
				
				TableSchema.TableColumn colvarPostText = new TableSchema.TableColumn(schema);
				colvarPostText.ColumnName = "PostText";
				colvarPostText.DataType = DbType.AnsiString;
				colvarPostText.MaxLength = -1;
				colvarPostText.AutoIncrement = false;
				colvarPostText.IsNullable = false;
				colvarPostText.IsPrimaryKey = false;
				colvarPostText.IsForeignKey = false;
				colvarPostText.IsReadOnly = false;
				colvarPostText.DefaultSetting = @"";
				colvarPostText.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPostText);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["WillCall"].AddSchema("PostPurchaseText",schema);
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
		  
		[XmlAttribute("TMerchId")]
		[Bindable(true)]
		public int? TMerchId 
		{
			get { return GetColumnValue<int?>(Columns.TMerchId); }
			set { SetColumnValue(Columns.TMerchId, value); }
		}
		  
		[XmlAttribute("TShowTicketId")]
		[Bindable(true)]
		public int? TShowTicketId 
		{
			get { return GetColumnValue<int?>(Columns.TShowTicketId); }
			set { SetColumnValue(Columns.TShowTicketId, value); }
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
		  
		[XmlAttribute("InProcessDescription")]
		[Bindable(true)]
		public string InProcessDescription 
		{
			get { return GetColumnValue<string>(Columns.InProcessDescription); }
			set { SetColumnValue(Columns.InProcessDescription, value); }
		}
		  
		[XmlAttribute("PostText")]
		[Bindable(true)]
		public string PostText 
		{
			get { return GetColumnValue<string>(Columns.PostText); }
			set { SetColumnValue(Columns.PostText, value); }
		}
		
		#endregion
		
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a Merch ActiveRecord object related to this PostPurchaseText
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
		/// Returns a ShowTicket ActiveRecord object related to this PostPurchaseText
		/// 
		/// </summary>
		private Wcss.ShowTicket ShowTicket
		{
			get { return Wcss.ShowTicket.FetchByID(this.TShowTicketId); }
			set { SetColumnValue("tShowTicketId", value.Id); }
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
                
                SetColumnValue("tShowTicketId", value.Id);
                _showticketrecord.CopyFrom(value);                
            }
		}
		
		
		#endregion
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(DateTime varDtStamp,int? varTMerchId,int? varTShowTicketId,bool varBActive,int varIDisplayOrder,string varInProcessDescription,string varPostText)
		{
			PostPurchaseText item = new PostPurchaseText();
			
			item.DtStamp = varDtStamp;
			
			item.TMerchId = varTMerchId;
			
			item.TShowTicketId = varTShowTicketId;
			
			item.BActive = varBActive;
			
			item.IDisplayOrder = varIDisplayOrder;
			
			item.InProcessDescription = varInProcessDescription;
			
			item.PostText = varPostText;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varId,DateTime varDtStamp,int? varTMerchId,int? varTShowTicketId,bool varBActive,int varIDisplayOrder,string varInProcessDescription,string varPostText)
		{
			PostPurchaseText item = new PostPurchaseText();
			
				item.Id = varId;
			
				item.DtStamp = varDtStamp;
			
				item.TMerchId = varTMerchId;
			
				item.TShowTicketId = varTShowTicketId;
			
				item.BActive = varBActive;
			
				item.IDisplayOrder = varIDisplayOrder;
			
				item.InProcessDescription = varInProcessDescription;
			
				item.PostText = varPostText;
			
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
        
        
        
        public static TableSchema.TableColumn TMerchIdColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn TShowTicketIdColumn
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
        
        
        
        public static TableSchema.TableColumn InProcessDescriptionColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn PostTextColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Id = @"Id";
			 public static string DtStamp = @"dtStamp";
			 public static string TMerchId = @"tMerchId";
			 public static string TShowTicketId = @"tShowTicketId";
			 public static string BActive = @"bActive";
			 public static string IDisplayOrder = @"iDisplayOrder";
			 public static string InProcessDescription = @"InProcessDescription";
			 public static string PostText = @"PostText";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
