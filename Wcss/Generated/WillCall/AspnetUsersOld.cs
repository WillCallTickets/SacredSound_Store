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
	/// Strongly-typed collection for the AspnetUsersOld class.
	/// </summary>
    [Serializable]
	public partial class AspnetUsersOldCollection : ActiveList<AspnetUsersOld, AspnetUsersOldCollection>
	{	   
		public AspnetUsersOldCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>AspnetUsersOldCollection</returns>
		public AspnetUsersOldCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                AspnetUsersOld o = this[i];
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
	/// This is an ActiveRecord class which wraps the aspnet_Users_Old table.
	/// </summary>
	[Serializable]
	public partial class AspnetUsersOld : ActiveRecord<AspnetUsersOld>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public AspnetUsersOld()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public AspnetUsersOld(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public AspnetUsersOld(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public AspnetUsersOld(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("aspnet_Users_Old", TableType.Table, DataService.GetInstance("WillCall"));
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
				
				TableSchema.TableColumn colvarTCustomerId = new TableSchema.TableColumn(schema);
				colvarTCustomerId.ColumnName = "TCustomerId";
				colvarTCustomerId.DataType = DbType.Int32;
				colvarTCustomerId.MaxLength = 0;
				colvarTCustomerId.AutoIncrement = false;
				colvarTCustomerId.IsNullable = false;
				colvarTCustomerId.IsPrimaryKey = false;
				colvarTCustomerId.IsForeignKey = false;
				colvarTCustomerId.IsReadOnly = false;
				colvarTCustomerId.DefaultSetting = @"";
				colvarTCustomerId.ForeignKeyTableName = "";
				schema.Columns.Add(colvarTCustomerId);
				
				TableSchema.TableColumn colvarUserId = new TableSchema.TableColumn(schema);
				colvarUserId.ColumnName = "UserId";
				colvarUserId.DataType = DbType.Guid;
				colvarUserId.MaxLength = 0;
				colvarUserId.AutoIncrement = false;
				colvarUserId.IsNullable = false;
				colvarUserId.IsPrimaryKey = false;
				colvarUserId.IsForeignKey = false;
				colvarUserId.IsReadOnly = false;
				colvarUserId.DefaultSetting = @"";
				colvarUserId.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserId);
				
				TableSchema.TableColumn colvarUserName = new TableSchema.TableColumn(schema);
				colvarUserName.ColumnName = "UserName";
				colvarUserName.DataType = DbType.AnsiString;
				colvarUserName.MaxLength = 300;
				colvarUserName.AutoIncrement = false;
				colvarUserName.IsNullable = false;
				colvarUserName.IsPrimaryKey = false;
				colvarUserName.IsForeignKey = false;
				colvarUserName.IsReadOnly = false;
				colvarUserName.DefaultSetting = @"";
				colvarUserName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserName);
				
				TableSchema.TableColumn colvarOldPass = new TableSchema.TableColumn(schema);
				colvarOldPass.ColumnName = "oldPass";
				colvarOldPass.DataType = DbType.AnsiString;
				colvarOldPass.MaxLength = 256;
				colvarOldPass.AutoIncrement = false;
				colvarOldPass.IsNullable = false;
				colvarOldPass.IsPrimaryKey = false;
				colvarOldPass.IsForeignKey = false;
				colvarOldPass.IsReadOnly = false;
				colvarOldPass.DefaultSetting = @"";
				colvarOldPass.ForeignKeyTableName = "";
				schema.Columns.Add(colvarOldPass);
				
				TableSchema.TableColumn colvarDtUpdated = new TableSchema.TableColumn(schema);
				colvarDtUpdated.ColumnName = "dtUpdated";
				colvarDtUpdated.DataType = DbType.DateTime;
				colvarDtUpdated.MaxLength = 0;
				colvarDtUpdated.AutoIncrement = false;
				colvarDtUpdated.IsNullable = true;
				colvarDtUpdated.IsPrimaryKey = false;
				colvarDtUpdated.IsForeignKey = false;
				colvarDtUpdated.IsReadOnly = false;
				colvarDtUpdated.DefaultSetting = @"";
				colvarDtUpdated.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDtUpdated);
				
				TableSchema.TableColumn colvarIpAddress = new TableSchema.TableColumn(schema);
				colvarIpAddress.ColumnName = "IpAddress";
				colvarIpAddress.DataType = DbType.AnsiString;
				colvarIpAddress.MaxLength = 25;
				colvarIpAddress.AutoIncrement = false;
				colvarIpAddress.IsNullable = true;
				colvarIpAddress.IsPrimaryKey = false;
				colvarIpAddress.IsForeignKey = false;
				colvarIpAddress.IsReadOnly = false;
				colvarIpAddress.DefaultSetting = @"";
				colvarIpAddress.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIpAddress);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["WillCall"].AddSchema("aspnet_Users_Old",schema);
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
		  
		[XmlAttribute("TCustomerId")]
		[Bindable(true)]
		public int TCustomerId 
		{
			get { return GetColumnValue<int>(Columns.TCustomerId); }
			set { SetColumnValue(Columns.TCustomerId, value); }
		}
		  
		[XmlAttribute("UserId")]
		[Bindable(true)]
		public Guid UserId 
		{
			get { return GetColumnValue<Guid>(Columns.UserId); }
			set { SetColumnValue(Columns.UserId, value); }
		}
		  
		[XmlAttribute("UserName")]
		[Bindable(true)]
		public string UserName 
		{
			get { return GetColumnValue<string>(Columns.UserName); }
			set { SetColumnValue(Columns.UserName, value); }
		}
		  
		[XmlAttribute("OldPass")]
		[Bindable(true)]
		public string OldPass 
		{
			get { return GetColumnValue<string>(Columns.OldPass); }
			set { SetColumnValue(Columns.OldPass, value); }
		}
		  
		[XmlAttribute("DtUpdated")]
		[Bindable(true)]
		public DateTime? DtUpdated 
		{
			get { return GetColumnValue<DateTime?>(Columns.DtUpdated); }
			set { SetColumnValue(Columns.DtUpdated, value); }
		}
		  
		[XmlAttribute("IpAddress")]
		[Bindable(true)]
		public string IpAddress 
		{
			get { return GetColumnValue<string>(Columns.IpAddress); }
			set { SetColumnValue(Columns.IpAddress, value); }
		}
		
		#endregion
		
		
			
		
		//no foreign key tables defined (0)
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(Guid varApplicationId,DateTime varDtStamp,int varTCustomerId,Guid varUserId,string varUserName,string varOldPass,DateTime? varDtUpdated,string varIpAddress)
		{
			AspnetUsersOld item = new AspnetUsersOld();
			
			item.ApplicationId = varApplicationId;
			
			item.DtStamp = varDtStamp;
			
			item.TCustomerId = varTCustomerId;
			
			item.UserId = varUserId;
			
			item.UserName = varUserName;
			
			item.OldPass = varOldPass;
			
			item.DtUpdated = varDtUpdated;
			
			item.IpAddress = varIpAddress;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varId,Guid varApplicationId,DateTime varDtStamp,int varTCustomerId,Guid varUserId,string varUserName,string varOldPass,DateTime? varDtUpdated,string varIpAddress)
		{
			AspnetUsersOld item = new AspnetUsersOld();
			
				item.Id = varId;
			
				item.ApplicationId = varApplicationId;
			
				item.DtStamp = varDtStamp;
			
				item.TCustomerId = varTCustomerId;
			
				item.UserId = varUserId;
			
				item.UserName = varUserName;
			
				item.OldPass = varOldPass;
			
				item.DtUpdated = varDtUpdated;
			
				item.IpAddress = varIpAddress;
			
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
        
        
        
        public static TableSchema.TableColumn ApplicationIdColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn DtStampColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn TCustomerIdColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn UserIdColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn UserNameColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn OldPassColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn DtUpdatedColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn IpAddressColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Id = @"Id";
			 public static string ApplicationId = @"ApplicationId";
			 public static string DtStamp = @"dtStamp";
			 public static string TCustomerId = @"TCustomerId";
			 public static string UserId = @"UserId";
			 public static string UserName = @"UserName";
			 public static string OldPass = @"oldPass";
			 public static string DtUpdated = @"dtUpdated";
			 public static string IpAddress = @"IpAddress";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
