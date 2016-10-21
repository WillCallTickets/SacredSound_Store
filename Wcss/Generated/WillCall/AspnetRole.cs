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
	/// Strongly-typed collection for the AspnetRole class.
	/// </summary>
    [Serializable]
	public partial class AspnetRoleCollection : ActiveList<AspnetRole, AspnetRoleCollection>
	{	   
		public AspnetRoleCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>AspnetRoleCollection</returns>
		public AspnetRoleCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                AspnetRole o = this[i];
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
	/// This is an ActiveRecord class which wraps the aspnet_Roles table.
	/// </summary>
	[Serializable]
	public partial class AspnetRole : ActiveRecord<AspnetRole>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public AspnetRole()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public AspnetRole(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public AspnetRole(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public AspnetRole(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("aspnet_Roles", TableType.Table, DataService.GetInstance("WillCall"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
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
				
				TableSchema.TableColumn colvarRoleId = new TableSchema.TableColumn(schema);
				colvarRoleId.ColumnName = "RoleId";
				colvarRoleId.DataType = DbType.Guid;
				colvarRoleId.MaxLength = 0;
				colvarRoleId.AutoIncrement = false;
				colvarRoleId.IsNullable = false;
				colvarRoleId.IsPrimaryKey = true;
				colvarRoleId.IsForeignKey = false;
				colvarRoleId.IsReadOnly = false;
				
						colvarRoleId.DefaultSetting = @"(newid())";
				colvarRoleId.ForeignKeyTableName = "";
				schema.Columns.Add(colvarRoleId);
				
				TableSchema.TableColumn colvarRoleName = new TableSchema.TableColumn(schema);
				colvarRoleName.ColumnName = "RoleName";
				colvarRoleName.DataType = DbType.String;
				colvarRoleName.MaxLength = 256;
				colvarRoleName.AutoIncrement = false;
				colvarRoleName.IsNullable = false;
				colvarRoleName.IsPrimaryKey = false;
				colvarRoleName.IsForeignKey = false;
				colvarRoleName.IsReadOnly = false;
				colvarRoleName.DefaultSetting = @"";
				colvarRoleName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarRoleName);
				
				TableSchema.TableColumn colvarLoweredRoleName = new TableSchema.TableColumn(schema);
				colvarLoweredRoleName.ColumnName = "LoweredRoleName";
				colvarLoweredRoleName.DataType = DbType.String;
				colvarLoweredRoleName.MaxLength = 256;
				colvarLoweredRoleName.AutoIncrement = false;
				colvarLoweredRoleName.IsNullable = false;
				colvarLoweredRoleName.IsPrimaryKey = false;
				colvarLoweredRoleName.IsForeignKey = false;
				colvarLoweredRoleName.IsReadOnly = false;
				colvarLoweredRoleName.DefaultSetting = @"";
				colvarLoweredRoleName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarLoweredRoleName);
				
				TableSchema.TableColumn colvarDescription = new TableSchema.TableColumn(schema);
				colvarDescription.ColumnName = "Description";
				colvarDescription.DataType = DbType.String;
				colvarDescription.MaxLength = 256;
				colvarDescription.AutoIncrement = false;
				colvarDescription.IsNullable = true;
				colvarDescription.IsPrimaryKey = false;
				colvarDescription.IsForeignKey = false;
				colvarDescription.IsReadOnly = false;
				colvarDescription.DefaultSetting = @"";
				colvarDescription.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDescription);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["WillCall"].AddSchema("aspnet_Roles",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("ApplicationId")]
		[Bindable(true)]
		public Guid ApplicationId 
		{
			get { return GetColumnValue<Guid>(Columns.ApplicationId); }
			set { SetColumnValue(Columns.ApplicationId, value); }
		}
		  
		[XmlAttribute("RoleId")]
		[Bindable(true)]
		public Guid RoleId 
		{
			get { return GetColumnValue<Guid>(Columns.RoleId); }
			set { SetColumnValue(Columns.RoleId, value); }
		}
		  
		[XmlAttribute("RoleName")]
		[Bindable(true)]
		public string RoleName 
		{
			get { return GetColumnValue<string>(Columns.RoleName); }
			set { SetColumnValue(Columns.RoleName, value); }
		}
		  
		[XmlAttribute("LoweredRoleName")]
		[Bindable(true)]
		public string LoweredRoleName 
		{
			get { return GetColumnValue<string>(Columns.LoweredRoleName); }
			set { SetColumnValue(Columns.LoweredRoleName, value); }
		}
		  
		[XmlAttribute("Description")]
		[Bindable(true)]
		public string Description 
		{
			get { return GetColumnValue<string>(Columns.Description); }
			set { SetColumnValue(Columns.Description, value); }
		}
		
		#endregion
		
		
		#region PrimaryKey Methods		
		
        protected override void SetPrimaryKey(object oValue)
        {
            base.SetPrimaryKey(oValue);
            
            SetPKValues();
        }
        
		
		private Wcss.AspnetUsersInRoleCollection colAspnetUsersInRoles;
		public Wcss.AspnetUsersInRoleCollection AspnetUsersInRoles()
		{
			if(colAspnetUsersInRoles == null)
			{
				colAspnetUsersInRoles = new Wcss.AspnetUsersInRoleCollection().Where(AspnetUsersInRole.Columns.RoleId, RoleId).Load();
				colAspnetUsersInRoles.ListChanged += new ListChangedEventHandler(colAspnetUsersInRoles_ListChanged);
			}
			return colAspnetUsersInRoles;
		}
				
		void colAspnetUsersInRoles_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colAspnetUsersInRoles[e.NewIndex].RoleId = RoleId;
				colAspnetUsersInRoles.ListChanged += new ListChangedEventHandler(colAspnetUsersInRoles_ListChanged);
            }
		}
		private Wcss.SubscriptionCollection colSubscriptionRecords;
		public Wcss.SubscriptionCollection SubscriptionRecords()
		{
			if(colSubscriptionRecords == null)
			{
				colSubscriptionRecords = new Wcss.SubscriptionCollection().Where(Subscription.Columns.RoleId, RoleId).Load();
				colSubscriptionRecords.ListChanged += new ListChangedEventHandler(colSubscriptionRecords_ListChanged);
			}
			return colSubscriptionRecords;
		}
				
		void colSubscriptionRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colSubscriptionRecords[e.NewIndex].RoleId = RoleId;
				colSubscriptionRecords.ListChanged += new ListChangedEventHandler(colSubscriptionRecords_ListChanged);
            }
		}
		#endregion
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a AspnetApplication ActiveRecord object related to this AspnetRole
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
		
		
		#endregion
		
		
		
		#region Many To Many Helpers
		
		 
		public Wcss.AspnetUserCollection GetAspnetUserCollection() { return AspnetRole.GetAspnetUserCollection(this.RoleId); }
		public static Wcss.AspnetUserCollection GetAspnetUserCollection(Guid varRoleId)
		{
		    SubSonic.QueryCommand cmd = new SubSonic.QueryCommand("SELECT * FROM [dbo].[aspnet_Users] INNER JOIN [aspnet_UsersInRoles] ON [aspnet_Users].[UserId] = [aspnet_UsersInRoles].[UserId] WHERE [aspnet_UsersInRoles].[RoleId] = @RoleId", AspnetRole.Schema.Provider.Name);
			cmd.AddParameter("@RoleId", varRoleId, DbType.Guid);
			IDataReader rdr = SubSonic.DataService.GetReader(cmd);
			AspnetUserCollection coll = new AspnetUserCollection();
			coll.LoadAndCloseReader(rdr);
			return coll;
		}
		
		public static void SaveAspnetUserMap(Guid varRoleId, AspnetUserCollection items)
		{
			QueryCommandCollection coll = new SubSonic.QueryCommandCollection();
			//delete out the existing
			QueryCommand cmdDel = new QueryCommand("DELETE FROM [aspnet_UsersInRoles] WHERE [aspnet_UsersInRoles].[RoleId] = @RoleId", AspnetRole.Schema.Provider.Name);
			cmdDel.AddParameter("@RoleId", varRoleId, DbType.Guid);
			coll.Add(cmdDel);
			DataService.ExecuteTransaction(coll);
			foreach (AspnetUser item in items)
			{
				AspnetUsersInRole varAspnetUsersInRole = new AspnetUsersInRole();
				varAspnetUsersInRole.SetColumnValue("RoleId", varRoleId);
				varAspnetUsersInRole.SetColumnValue("UserId", item.GetPrimaryKeyValue());
				varAspnetUsersInRole.Save();
			}
		}
		public static void SaveAspnetUserMap(Guid varRoleId, System.Web.UI.WebControls.ListItemCollection itemList) 
		{
			QueryCommandCollection coll = new SubSonic.QueryCommandCollection();
			//delete out the existing
			 QueryCommand cmdDel = new QueryCommand("DELETE FROM [aspnet_UsersInRoles] WHERE [aspnet_UsersInRoles].[RoleId] = @RoleId", AspnetRole.Schema.Provider.Name);
			cmdDel.AddParameter("@RoleId", varRoleId, DbType.Guid);
			coll.Add(cmdDel);
			DataService.ExecuteTransaction(coll);
			foreach (System.Web.UI.WebControls.ListItem l in itemList) 
			{
				if (l.Selected) 
				{
					AspnetUsersInRole varAspnetUsersInRole = new AspnetUsersInRole();
					varAspnetUsersInRole.SetColumnValue("RoleId", varRoleId);
					varAspnetUsersInRole.SetColumnValue("UserId", l.Value);
					varAspnetUsersInRole.Save();
				}
			}
		}
		public static void SaveAspnetUserMap(Guid varRoleId , Guid[] itemList) 
		{
			QueryCommandCollection coll = new SubSonic.QueryCommandCollection();
			//delete out the existing
			 QueryCommand cmdDel = new QueryCommand("DELETE FROM [aspnet_UsersInRoles] WHERE [aspnet_UsersInRoles].[RoleId] = @RoleId", AspnetRole.Schema.Provider.Name);
			cmdDel.AddParameter("@RoleId", varRoleId, DbType.Guid);
			coll.Add(cmdDel);
			DataService.ExecuteTransaction(coll);
			foreach (Guid item in itemList) 
			{
				AspnetUsersInRole varAspnetUsersInRole = new AspnetUsersInRole();
				varAspnetUsersInRole.SetColumnValue("RoleId", varRoleId);
				varAspnetUsersInRole.SetColumnValue("UserId", item);
				varAspnetUsersInRole.Save();
			}
		}
		
		public static void DeleteAspnetUserMap(Guid varRoleId) 
		{
			QueryCommand cmdDel = new QueryCommand("DELETE FROM [aspnet_UsersInRoles] WHERE [aspnet_UsersInRoles].[RoleId] = @RoleId", AspnetRole.Schema.Provider.Name);
			cmdDel.AddParameter("@RoleId", varRoleId, DbType.Guid);
			DataService.ExecuteQuery(cmdDel);
		}
		
		#endregion
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(Guid varApplicationId,Guid varRoleId,string varRoleName,string varLoweredRoleName,string varDescription)
		{
			AspnetRole item = new AspnetRole();
			
			item.ApplicationId = varApplicationId;
			
			item.RoleId = varRoleId;
			
			item.RoleName = varRoleName;
			
			item.LoweredRoleName = varLoweredRoleName;
			
			item.Description = varDescription;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(Guid varApplicationId,Guid varRoleId,string varRoleName,string varLoweredRoleName,string varDescription)
		{
			AspnetRole item = new AspnetRole();
			
				item.ApplicationId = varApplicationId;
			
				item.RoleId = varRoleId;
			
				item.RoleName = varRoleName;
			
				item.LoweredRoleName = varLoweredRoleName;
			
				item.Description = varDescription;
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn ApplicationIdColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn RoleIdColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn RoleNameColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn LoweredRoleNameColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn DescriptionColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string ApplicationId = @"ApplicationId";
			 public static string RoleId = @"RoleId";
			 public static string RoleName = @"RoleName";
			 public static string LoweredRoleName = @"LoweredRoleName";
			 public static string Description = @"Description";
						
		}
		#endregion
		
		#region Update PK Collections
		
        public void SetPKValues()
        {
                if (colAspnetUsersInRoles != null)
                {
                    foreach (Wcss.AspnetUsersInRole item in colAspnetUsersInRoles)
                    {
                        if (item.RoleId != RoleId)
                        {
                            item.RoleId = RoleId;
                        }
                    }
               }
		
                if (colSubscriptionRecords != null)
                {
                    foreach (Wcss.Subscription item in colSubscriptionRecords)
                    {
                        if (item.RoleId != RoleId)
                        {
                            item.RoleId = RoleId;
                        }
                    }
               }
		}
        #endregion
    
        #region Deep Save
		
        public void DeepSave()
        {
            Save();
            
                if (colAspnetUsersInRoles != null)
                {
                    colAspnetUsersInRoles.SaveAll();
               }
		
                if (colSubscriptionRecords != null)
                {
                    colSubscriptionRecords.SaveAll();
               }
		}
        #endregion
	}
}
