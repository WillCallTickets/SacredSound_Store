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
	/// Strongly-typed collection for the Subscription class.
	/// </summary>
    [Serializable]
	public partial class SubscriptionCollection : ActiveList<Subscription, SubscriptionCollection>
	{	   
		public SubscriptionCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>SubscriptionCollection</returns>
		public SubscriptionCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                Subscription o = this[i];
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
	/// This is an ActiveRecord class which wraps the Subscription table.
	/// </summary>
	[Serializable]
	public partial class Subscription : ActiveRecord<Subscription>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public Subscription()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public Subscription(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public Subscription(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public Subscription(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("Subscription", TableType.Table, DataService.GetInstance("WillCall"));
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
				
				TableSchema.TableColumn colvarRoleId = new TableSchema.TableColumn(schema);
				colvarRoleId.ColumnName = "RoleId";
				colvarRoleId.DataType = DbType.Guid;
				colvarRoleId.MaxLength = 0;
				colvarRoleId.AutoIncrement = false;
				colvarRoleId.IsNullable = false;
				colvarRoleId.IsPrimaryKey = false;
				colvarRoleId.IsForeignKey = true;
				colvarRoleId.IsReadOnly = false;
				colvarRoleId.DefaultSetting = @"";
				
					colvarRoleId.ForeignKeyTableName = "aspnet_Roles";
				schema.Columns.Add(colvarRoleId);
				
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
				
				TableSchema.TableColumn colvarBDefault = new TableSchema.TableColumn(schema);
				colvarBDefault.ColumnName = "bDefault";
				colvarBDefault.DataType = DbType.Boolean;
				colvarBDefault.MaxLength = 0;
				colvarBDefault.AutoIncrement = false;
				colvarBDefault.IsNullable = false;
				colvarBDefault.IsPrimaryKey = false;
				colvarBDefault.IsForeignKey = false;
				colvarBDefault.IsReadOnly = false;
				
						colvarBDefault.DefaultSetting = @"((0))";
				colvarBDefault.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBDefault);
				
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
				
				TableSchema.TableColumn colvarDescription = new TableSchema.TableColumn(schema);
				colvarDescription.ColumnName = "Description";
				colvarDescription.DataType = DbType.AnsiString;
				colvarDescription.MaxLength = 500;
				colvarDescription.AutoIncrement = false;
				colvarDescription.IsNullable = true;
				colvarDescription.IsPrimaryKey = false;
				colvarDescription.IsForeignKey = false;
				colvarDescription.IsReadOnly = false;
				colvarDescription.DefaultSetting = @"";
				colvarDescription.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDescription);
				
				TableSchema.TableColumn colvarInternalDescription = new TableSchema.TableColumn(schema);
				colvarInternalDescription.ColumnName = "InternalDescription";
				colvarInternalDescription.DataType = DbType.AnsiString;
				colvarInternalDescription.MaxLength = 2000;
				colvarInternalDescription.AutoIncrement = false;
				colvarInternalDescription.IsNullable = true;
				colvarInternalDescription.IsPrimaryKey = false;
				colvarInternalDescription.IsForeignKey = false;
				colvarInternalDescription.IsReadOnly = false;
				colvarInternalDescription.DefaultSetting = @"";
				colvarInternalDescription.ForeignKeyTableName = "";
				schema.Columns.Add(colvarInternalDescription);
				
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
				DataService.Providers["WillCall"].AddSchema("Subscription",schema);
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
		  
		[XmlAttribute("RoleId")]
		[Bindable(true)]
		public Guid RoleId 
		{
			get { return GetColumnValue<Guid>(Columns.RoleId); }
			set { SetColumnValue(Columns.RoleId, value); }
		}
		  
		[XmlAttribute("BActive")]
		[Bindable(true)]
		public bool BActive 
		{
			get { return GetColumnValue<bool>(Columns.BActive); }
			set { SetColumnValue(Columns.BActive, value); }
		}
		  
		[XmlAttribute("BDefault")]
		[Bindable(true)]
		public bool BDefault 
		{
			get { return GetColumnValue<bool>(Columns.BDefault); }
			set { SetColumnValue(Columns.BDefault, value); }
		}
		  
		[XmlAttribute("Name")]
		[Bindable(true)]
		public string Name 
		{
			get { return GetColumnValue<string>(Columns.Name); }
			set { SetColumnValue(Columns.Name, value); }
		}
		  
		[XmlAttribute("Description")]
		[Bindable(true)]
		public string Description 
		{
			get { return GetColumnValue<string>(Columns.Description); }
			set { SetColumnValue(Columns.Description, value); }
		}
		  
		[XmlAttribute("InternalDescription")]
		[Bindable(true)]
		public string InternalDescription 
		{
			get { return GetColumnValue<string>(Columns.InternalDescription); }
			set { SetColumnValue(Columns.InternalDescription, value); }
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
        
		
		private Wcss.SubscriptionUserCollection colSubscriptionUserRecords;
		public Wcss.SubscriptionUserCollection SubscriptionUserRecords()
		{
			if(colSubscriptionUserRecords == null)
			{
				colSubscriptionUserRecords = new Wcss.SubscriptionUserCollection().Where(SubscriptionUser.Columns.TSubscriptionId, Id).Load();
				colSubscriptionUserRecords.ListChanged += new ListChangedEventHandler(colSubscriptionUserRecords_ListChanged);
			}
			return colSubscriptionUserRecords;
		}
				
		void colSubscriptionUserRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colSubscriptionUserRecords[e.NewIndex].TSubscriptionId = Id;
				colSubscriptionUserRecords.ListChanged += new ListChangedEventHandler(colSubscriptionUserRecords_ListChanged);
            }
		}
		private Wcss.SubscriptionEmailCollection colSubscriptionEmailRecords;
		public Wcss.SubscriptionEmailCollection SubscriptionEmailRecords()
		{
			if(colSubscriptionEmailRecords == null)
			{
				colSubscriptionEmailRecords = new Wcss.SubscriptionEmailCollection().Where(SubscriptionEmail.Columns.TSubscriptionId, Id).Load();
				colSubscriptionEmailRecords.ListChanged += new ListChangedEventHandler(colSubscriptionEmailRecords_ListChanged);
			}
			return colSubscriptionEmailRecords;
		}
				
		void colSubscriptionEmailRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colSubscriptionEmailRecords[e.NewIndex].TSubscriptionId = Id;
				colSubscriptionEmailRecords.ListChanged += new ListChangedEventHandler(colSubscriptionEmailRecords_ListChanged);
            }
		}
		#endregion
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a AspnetApplication ActiveRecord object related to this Subscription
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
		/// Returns a AspnetRole ActiveRecord object related to this Subscription
		/// 
		/// </summary>
		private Wcss.AspnetRole AspnetRole
		{
			get { return Wcss.AspnetRole.FetchByID(this.RoleId); }
			set { SetColumnValue("RoleId", value.RoleId); }
		}
        //set up an alternate mechanism to avoid a database call
		private Wcss.AspnetRole _aspnetrolerecord = null;
		
		public Wcss.AspnetRole AspnetRoleRecord
		{
		    get
            {
                if (_aspnetrolerecord == null)
                {
                    _aspnetrolerecord = new Wcss.AspnetRole();
                    _aspnetrolerecord.CopyFrom(this.AspnetRole);
                }
                return _aspnetrolerecord;
            }
            set
            {
                if(value != null && _aspnetrolerecord == null)
			        _aspnetrolerecord = new Wcss.AspnetRole();
                
                SetColumnValue("RoleId", value.RoleId);
                _aspnetrolerecord.CopyFrom(value);                
            }
		}
		
		
		#endregion
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(Guid varRoleId,bool varBActive,bool varBDefault,string varName,string varDescription,string varInternalDescription,DateTime varDtStamp,Guid varApplicationId)
		{
			Subscription item = new Subscription();
			
			item.RoleId = varRoleId;
			
			item.BActive = varBActive;
			
			item.BDefault = varBDefault;
			
			item.Name = varName;
			
			item.Description = varDescription;
			
			item.InternalDescription = varInternalDescription;
			
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
		public static void Update(int varId,Guid varRoleId,bool varBActive,bool varBDefault,string varName,string varDescription,string varInternalDescription,DateTime varDtStamp,Guid varApplicationId)
		{
			Subscription item = new Subscription();
			
				item.Id = varId;
			
				item.RoleId = varRoleId;
			
				item.BActive = varBActive;
			
				item.BDefault = varBDefault;
			
				item.Name = varName;
			
				item.Description = varDescription;
			
				item.InternalDescription = varInternalDescription;
			
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
        
        
        
        public static TableSchema.TableColumn RoleIdColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn BActiveColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn BDefaultColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn NameColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn DescriptionColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn InternalDescriptionColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn DtStampColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn ApplicationIdColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Id = @"Id";
			 public static string RoleId = @"RoleId";
			 public static string BActive = @"bActive";
			 public static string BDefault = @"bDefault";
			 public static string Name = @"Name";
			 public static string Description = @"Description";
			 public static string InternalDescription = @"InternalDescription";
			 public static string DtStamp = @"dtStamp";
			 public static string ApplicationId = @"ApplicationId";
						
		}
		#endregion
		
		#region Update PK Collections
		
        public void SetPKValues()
        {
                if (colSubscriptionUserRecords != null)
                {
                    foreach (Wcss.SubscriptionUser item in colSubscriptionUserRecords)
                    {
                        if (item.TSubscriptionId != Id)
                        {
                            item.TSubscriptionId = Id;
                        }
                    }
               }
		
                if (colSubscriptionEmailRecords != null)
                {
                    foreach (Wcss.SubscriptionEmail item in colSubscriptionEmailRecords)
                    {
                        if (item.TSubscriptionId != Id)
                        {
                            item.TSubscriptionId = Id;
                        }
                    }
               }
		}
        #endregion
    
        #region Deep Save
		
        public void DeepSave()
        {
            Save();
            
                if (colSubscriptionUserRecords != null)
                {
                    colSubscriptionUserRecords.SaveAll();
               }
		
                if (colSubscriptionEmailRecords != null)
                {
                    colSubscriptionEmailRecords.SaveAll();
               }
		}
        #endregion
	}
}
