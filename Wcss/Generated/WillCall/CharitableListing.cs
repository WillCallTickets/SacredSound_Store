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
	/// Strongly-typed collection for the CharitableListing class.
	/// </summary>
    [Serializable]
	public partial class CharitableListingCollection : ActiveList<CharitableListing, CharitableListingCollection>
	{	   
		public CharitableListingCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>CharitableListingCollection</returns>
		public CharitableListingCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                CharitableListing o = this[i];
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
	/// This is an ActiveRecord class which wraps the CharitableListing table.
	/// </summary>
	[Serializable]
	public partial class CharitableListing : ActiveRecord<CharitableListing>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public CharitableListing()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public CharitableListing(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public CharitableListing(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public CharitableListing(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("CharitableListing", TableType.Table, DataService.GetInstance("WillCall"));
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
				
				TableSchema.TableColumn colvarTCharitableOrgId = new TableSchema.TableColumn(schema);
				colvarTCharitableOrgId.ColumnName = "tCharitableOrgId";
				colvarTCharitableOrgId.DataType = DbType.Int32;
				colvarTCharitableOrgId.MaxLength = 0;
				colvarTCharitableOrgId.AutoIncrement = false;
				colvarTCharitableOrgId.IsNullable = false;
				colvarTCharitableOrgId.IsPrimaryKey = false;
				colvarTCharitableOrgId.IsForeignKey = true;
				colvarTCharitableOrgId.IsReadOnly = false;
				colvarTCharitableOrgId.DefaultSetting = @"";
				
					colvarTCharitableOrgId.ForeignKeyTableName = "CharitableOrg";
				schema.Columns.Add(colvarTCharitableOrgId);
				
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
				
				TableSchema.TableColumn colvarBAvailableForContribution = new TableSchema.TableColumn(schema);
				colvarBAvailableForContribution.ColumnName = "bAvailableForContribution";
				colvarBAvailableForContribution.DataType = DbType.Boolean;
				colvarBAvailableForContribution.MaxLength = 0;
				colvarBAvailableForContribution.AutoIncrement = false;
				colvarBAvailableForContribution.IsNullable = false;
				colvarBAvailableForContribution.IsPrimaryKey = false;
				colvarBAvailableForContribution.IsForeignKey = false;
				colvarBAvailableForContribution.IsReadOnly = false;
				
						colvarBAvailableForContribution.DefaultSetting = @"((1))";
				colvarBAvailableForContribution.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBAvailableForContribution);
				
				TableSchema.TableColumn colvarBTopBilling = new TableSchema.TableColumn(schema);
				colvarBTopBilling.ColumnName = "bTopBilling";
				colvarBTopBilling.DataType = DbType.Boolean;
				colvarBTopBilling.MaxLength = 0;
				colvarBTopBilling.AutoIncrement = false;
				colvarBTopBilling.IsNullable = false;
				colvarBTopBilling.IsPrimaryKey = false;
				colvarBTopBilling.IsForeignKey = false;
				colvarBTopBilling.IsReadOnly = false;
				colvarBTopBilling.DefaultSetting = @"";
				colvarBTopBilling.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBTopBilling);
				
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
				DataService.Providers["WillCall"].AddSchema("CharitableListing",schema);
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
		  
		[XmlAttribute("TCharitableOrgId")]
		[Bindable(true)]
		public int TCharitableOrgId 
		{
			get { return GetColumnValue<int>(Columns.TCharitableOrgId); }
			set { SetColumnValue(Columns.TCharitableOrgId, value); }
		}
		  
		[XmlAttribute("IDisplayOrder")]
		[Bindable(true)]
		public int IDisplayOrder 
		{
			get { return GetColumnValue<int>(Columns.IDisplayOrder); }
			set { SetColumnValue(Columns.IDisplayOrder, value); }
		}
		  
		[XmlAttribute("BAvailableForContribution")]
		[Bindable(true)]
		public bool BAvailableForContribution 
		{
			get { return GetColumnValue<bool>(Columns.BAvailableForContribution); }
			set { SetColumnValue(Columns.BAvailableForContribution, value); }
		}
		  
		[XmlAttribute("BTopBilling")]
		[Bindable(true)]
		public bool BTopBilling 
		{
			get { return GetColumnValue<bool>(Columns.BTopBilling); }
			set { SetColumnValue(Columns.BTopBilling, value); }
		}
		  
		[XmlAttribute("ApplicationId")]
		[Bindable(true)]
		public Guid ApplicationId 
		{
			get { return GetColumnValue<Guid>(Columns.ApplicationId); }
			set { SetColumnValue(Columns.ApplicationId, value); }
		}
		
		#endregion
		
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a AspnetApplication ActiveRecord object related to this CharitableListing
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
		
		
		/// <summary>
		/// Returns a CharitableOrg ActiveRecord object related to this CharitableListing
		/// 
		/// </summary>
		private Wcss.CharitableOrg CharitableOrg
		{
			get { return Wcss.CharitableOrg.FetchByID(this.TCharitableOrgId); }
			set { SetColumnValue("tCharitableOrgId", value.Id); }
		}
        //set up an alternate mechanism to avoid a database call
		private Wcss.CharitableOrg _charitableorgrecord = null;
		
		public Wcss.CharitableOrg CharitableOrgRecord
		{
		    get
            {
                if (_charitableorgrecord == null)
                {
                    _charitableorgrecord = new Wcss.CharitableOrg();
                    _charitableorgrecord.CopyFrom(this.CharitableOrg);
                }
                return _charitableorgrecord;
            }
            set
            {
                if(value != null && _charitableorgrecord == null)
			        _charitableorgrecord = new Wcss.CharitableOrg();
                
                SetColumnValue("tCharitableOrgId", value.Id);
                _charitableorgrecord.CopyFrom(value);                
            }
		}
		
		
		#endregion
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(DateTime varDtStamp,int varTCharitableOrgId,int varIDisplayOrder,bool varBAvailableForContribution,bool varBTopBilling,Guid varApplicationId)
		{
			CharitableListing item = new CharitableListing();
			
			item.DtStamp = varDtStamp;
			
			item.TCharitableOrgId = varTCharitableOrgId;
			
			item.IDisplayOrder = varIDisplayOrder;
			
			item.BAvailableForContribution = varBAvailableForContribution;
			
			item.BTopBilling = varBTopBilling;
			
			item.ApplicationId = varApplicationId;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varId,DateTime varDtStamp,int varTCharitableOrgId,int varIDisplayOrder,bool varBAvailableForContribution,bool varBTopBilling,Guid varApplicationId)
		{
			CharitableListing item = new CharitableListing();
			
				item.Id = varId;
			
				item.DtStamp = varDtStamp;
			
				item.TCharitableOrgId = varTCharitableOrgId;
			
				item.IDisplayOrder = varIDisplayOrder;
			
				item.BAvailableForContribution = varBAvailableForContribution;
			
				item.BTopBilling = varBTopBilling;
			
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
        
        
        
        public static TableSchema.TableColumn DtStampColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn TCharitableOrgIdColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn IDisplayOrderColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn BAvailableForContributionColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn BTopBillingColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn ApplicationIdColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Id = @"Id";
			 public static string DtStamp = @"dtStamp";
			 public static string TCharitableOrgId = @"tCharitableOrgId";
			 public static string IDisplayOrder = @"iDisplayOrder";
			 public static string BAvailableForContribution = @"bAvailableForContribution";
			 public static string BTopBilling = @"bTopBilling";
			 public static string ApplicationId = @"ApplicationId";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
