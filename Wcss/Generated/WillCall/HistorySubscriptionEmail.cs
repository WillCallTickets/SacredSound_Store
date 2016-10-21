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
	/// Strongly-typed collection for the HistorySubscriptionEmail class.
	/// </summary>
    [Serializable]
	public partial class HistorySubscriptionEmailCollection : ActiveList<HistorySubscriptionEmail, HistorySubscriptionEmailCollection>
	{	   
		public HistorySubscriptionEmailCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>HistorySubscriptionEmailCollection</returns>
		public HistorySubscriptionEmailCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                HistorySubscriptionEmail o = this[i];
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
	/// This is an ActiveRecord class which wraps the HistorySubscriptionEmail table.
	/// </summary>
	[Serializable]
	public partial class HistorySubscriptionEmail : ActiveRecord<HistorySubscriptionEmail>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public HistorySubscriptionEmail()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public HistorySubscriptionEmail(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public HistorySubscriptionEmail(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public HistorySubscriptionEmail(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("HistorySubscriptionEmail", TableType.Table, DataService.GetInstance("WillCall"));
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
				
				TableSchema.TableColumn colvarTSubscriptionEmailId = new TableSchema.TableColumn(schema);
				colvarTSubscriptionEmailId.ColumnName = "TSubscriptionEmailId";
				colvarTSubscriptionEmailId.DataType = DbType.Int32;
				colvarTSubscriptionEmailId.MaxLength = 0;
				colvarTSubscriptionEmailId.AutoIncrement = false;
				colvarTSubscriptionEmailId.IsNullable = false;
				colvarTSubscriptionEmailId.IsPrimaryKey = false;
				colvarTSubscriptionEmailId.IsForeignKey = true;
				colvarTSubscriptionEmailId.IsReadOnly = false;
				colvarTSubscriptionEmailId.DefaultSetting = @"";
				
					colvarTSubscriptionEmailId.ForeignKeyTableName = "SubscriptionEmail";
				schema.Columns.Add(colvarTSubscriptionEmailId);
				
				TableSchema.TableColumn colvarDtSent = new TableSchema.TableColumn(schema);
				colvarDtSent.ColumnName = "dtSent";
				colvarDtSent.DataType = DbType.DateTime;
				colvarDtSent.MaxLength = 0;
				colvarDtSent.AutoIncrement = false;
				colvarDtSent.IsNullable = false;
				colvarDtSent.IsPrimaryKey = false;
				colvarDtSent.IsForeignKey = false;
				colvarDtSent.IsReadOnly = false;
				colvarDtSent.DefaultSetting = @"";
				colvarDtSent.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDtSent);
				
				TableSchema.TableColumn colvarIRecipients = new TableSchema.TableColumn(schema);
				colvarIRecipients.ColumnName = "iRecipients";
				colvarIRecipients.DataType = DbType.Int32;
				colvarIRecipients.MaxLength = 0;
				colvarIRecipients.AutoIncrement = false;
				colvarIRecipients.IsNullable = false;
				colvarIRecipients.IsPrimaryKey = false;
				colvarIRecipients.IsForeignKey = false;
				colvarIRecipients.IsReadOnly = false;
				colvarIRecipients.DefaultSetting = @"";
				colvarIRecipients.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIRecipients);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["WillCall"].AddSchema("HistorySubscriptionEmail",schema);
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
		  
		[XmlAttribute("TSubscriptionEmailId")]
		[Bindable(true)]
		public int TSubscriptionEmailId 
		{
			get { return GetColumnValue<int>(Columns.TSubscriptionEmailId); }
			set { SetColumnValue(Columns.TSubscriptionEmailId, value); }
		}
		  
		[XmlAttribute("DtSent")]
		[Bindable(true)]
		public DateTime DtSent 
		{
			get { return GetColumnValue<DateTime>(Columns.DtSent); }
			set { SetColumnValue(Columns.DtSent, value); }
		}
		  
		[XmlAttribute("IRecipients")]
		[Bindable(true)]
		public int IRecipients 
		{
			get { return GetColumnValue<int>(Columns.IRecipients); }
			set { SetColumnValue(Columns.IRecipients, value); }
		}
		
		#endregion
		
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a SubscriptionEmail ActiveRecord object related to this HistorySubscriptionEmail
		/// 
		/// </summary>
		private Wcss.SubscriptionEmail SubscriptionEmail
		{
			get { return Wcss.SubscriptionEmail.FetchByID(this.TSubscriptionEmailId); }
			set { SetColumnValue("TSubscriptionEmailId", value.Id); }
		}
        //set up an alternate mechanism to avoid a database call
		private Wcss.SubscriptionEmail _subscriptionemailrecord = null;
		
		public Wcss.SubscriptionEmail SubscriptionEmailRecord
		{
		    get
            {
                if (_subscriptionemailrecord == null)
                {
                    _subscriptionemailrecord = new Wcss.SubscriptionEmail();
                    _subscriptionemailrecord.CopyFrom(this.SubscriptionEmail);
                }
                return _subscriptionemailrecord;
            }
            set
            {
                if(value != null && _subscriptionemailrecord == null)
			        _subscriptionemailrecord = new Wcss.SubscriptionEmail();
                
                SetColumnValue("TSubscriptionEmailId", value.Id);
                _subscriptionemailrecord.CopyFrom(value);                
            }
		}
		
		
		#endregion
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(DateTime varDtStamp,int varTSubscriptionEmailId,DateTime varDtSent,int varIRecipients)
		{
			HistorySubscriptionEmail item = new HistorySubscriptionEmail();
			
			item.DtStamp = varDtStamp;
			
			item.TSubscriptionEmailId = varTSubscriptionEmailId;
			
			item.DtSent = varDtSent;
			
			item.IRecipients = varIRecipients;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varId,DateTime varDtStamp,int varTSubscriptionEmailId,DateTime varDtSent,int varIRecipients)
		{
			HistorySubscriptionEmail item = new HistorySubscriptionEmail();
			
				item.Id = varId;
			
				item.DtStamp = varDtStamp;
			
				item.TSubscriptionEmailId = varTSubscriptionEmailId;
			
				item.DtSent = varDtSent;
			
				item.IRecipients = varIRecipients;
			
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
        
        
        
        public static TableSchema.TableColumn TSubscriptionEmailIdColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn DtSentColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn IRecipientsColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Id = @"Id";
			 public static string DtStamp = @"dtStamp";
			 public static string TSubscriptionEmailId = @"TSubscriptionEmailId";
			 public static string DtSent = @"dtSent";
			 public static string IRecipients = @"iRecipients";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
