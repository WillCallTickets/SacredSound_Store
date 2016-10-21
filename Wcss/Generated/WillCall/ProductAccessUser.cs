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
	/// Strongly-typed collection for the ProductAccessUser class.
	/// </summary>
    [Serializable]
	public partial class ProductAccessUserCollection : ActiveList<ProductAccessUser, ProductAccessUserCollection>
	{	   
		public ProductAccessUserCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>ProductAccessUserCollection</returns>
		public ProductAccessUserCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                ProductAccessUser o = this[i];
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
	/// This is an ActiveRecord class which wraps the ProductAccessUser table.
	/// </summary>
	[Serializable]
	public partial class ProductAccessUser : ActiveRecord<ProductAccessUser>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public ProductAccessUser()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public ProductAccessUser(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public ProductAccessUser(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public ProductAccessUser(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("ProductAccessUser", TableType.Table, DataService.GetInstance("WillCall"));
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
				
				TableSchema.TableColumn colvarTProductAccessId = new TableSchema.TableColumn(schema);
				colvarTProductAccessId.ColumnName = "TProductAccessId";
				colvarTProductAccessId.DataType = DbType.Int32;
				colvarTProductAccessId.MaxLength = 0;
				colvarTProductAccessId.AutoIncrement = false;
				colvarTProductAccessId.IsNullable = false;
				colvarTProductAccessId.IsPrimaryKey = false;
				colvarTProductAccessId.IsForeignKey = true;
				colvarTProductAccessId.IsReadOnly = false;
				colvarTProductAccessId.DefaultSetting = @"";
				
					colvarTProductAccessId.ForeignKeyTableName = "ProductAccess";
				schema.Columns.Add(colvarTProductAccessId);
				
				TableSchema.TableColumn colvarUserName = new TableSchema.TableColumn(schema);
				colvarUserName.ColumnName = "UserName";
				colvarUserName.DataType = DbType.AnsiString;
				colvarUserName.MaxLength = 256;
				colvarUserName.AutoIncrement = false;
				colvarUserName.IsNullable = false;
				colvarUserName.IsPrimaryKey = false;
				colvarUserName.IsForeignKey = false;
				colvarUserName.IsReadOnly = false;
				colvarUserName.DefaultSetting = @"";
				colvarUserName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserName);
				
				TableSchema.TableColumn colvarUserId = new TableSchema.TableColumn(schema);
				colvarUserId.ColumnName = "UserId";
				colvarUserId.DataType = DbType.Guid;
				colvarUserId.MaxLength = 0;
				colvarUserId.AutoIncrement = false;
				colvarUserId.IsNullable = true;
				colvarUserId.IsPrimaryKey = false;
				colvarUserId.IsForeignKey = true;
				colvarUserId.IsReadOnly = false;
				colvarUserId.DefaultSetting = @"";
				
					colvarUserId.ForeignKeyTableName = "aspnet_Users";
				schema.Columns.Add(colvarUserId);
				
				TableSchema.TableColumn colvarIQuantityAllowed = new TableSchema.TableColumn(schema);
				colvarIQuantityAllowed.ColumnName = "iQuantityAllowed";
				colvarIQuantityAllowed.DataType = DbType.Int32;
				colvarIQuantityAllowed.MaxLength = 0;
				colvarIQuantityAllowed.AutoIncrement = false;
				colvarIQuantityAllowed.IsNullable = false;
				colvarIQuantityAllowed.IsPrimaryKey = false;
				colvarIQuantityAllowed.IsForeignKey = false;
				colvarIQuantityAllowed.IsReadOnly = false;
				
						colvarIQuantityAllowed.DefaultSetting = @"((0))";
				colvarIQuantityAllowed.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIQuantityAllowed);
				
				TableSchema.TableColumn colvarReferral = new TableSchema.TableColumn(schema);
				colvarReferral.ColumnName = "Referral";
				colvarReferral.DataType = DbType.AnsiString;
				colvarReferral.MaxLength = 512;
				colvarReferral.AutoIncrement = false;
				colvarReferral.IsNullable = true;
				colvarReferral.IsPrimaryKey = false;
				colvarReferral.IsForeignKey = false;
				colvarReferral.IsReadOnly = false;
				colvarReferral.DefaultSetting = @"";
				colvarReferral.ForeignKeyTableName = "";
				schema.Columns.Add(colvarReferral);
				
				TableSchema.TableColumn colvarInstructions = new TableSchema.TableColumn(schema);
				colvarInstructions.ColumnName = "Instructions";
				colvarInstructions.DataType = DbType.AnsiString;
				colvarInstructions.MaxLength = 512;
				colvarInstructions.AutoIncrement = false;
				colvarInstructions.IsNullable = true;
				colvarInstructions.IsPrimaryKey = false;
				colvarInstructions.IsForeignKey = false;
				colvarInstructions.IsReadOnly = false;
				colvarInstructions.DefaultSetting = @"";
				colvarInstructions.ForeignKeyTableName = "";
				schema.Columns.Add(colvarInstructions);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["WillCall"].AddSchema("ProductAccessUser",schema);
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
		  
		[XmlAttribute("TProductAccessId")]
		[Bindable(true)]
		public int TProductAccessId 
		{
			get { return GetColumnValue<int>(Columns.TProductAccessId); }
			set { SetColumnValue(Columns.TProductAccessId, value); }
		}
		  
		[XmlAttribute("UserName")]
		[Bindable(true)]
		public string UserName 
		{
			get { return GetColumnValue<string>(Columns.UserName); }
			set { SetColumnValue(Columns.UserName, value); }
		}
		  
		[XmlAttribute("UserId")]
		[Bindable(true)]
		public Guid? UserId 
		{
			get { return GetColumnValue<Guid?>(Columns.UserId); }
			set { SetColumnValue(Columns.UserId, value); }
		}
		  
		[XmlAttribute("IQuantityAllowed")]
		[Bindable(true)]
		public int IQuantityAllowed 
		{
			get { return GetColumnValue<int>(Columns.IQuantityAllowed); }
			set { SetColumnValue(Columns.IQuantityAllowed, value); }
		}
		  
		[XmlAttribute("Referral")]
		[Bindable(true)]
		public string Referral 
		{
			get { return GetColumnValue<string>(Columns.Referral); }
			set { SetColumnValue(Columns.Referral, value); }
		}
		  
		[XmlAttribute("Instructions")]
		[Bindable(true)]
		public string Instructions 
		{
			get { return GetColumnValue<string>(Columns.Instructions); }
			set { SetColumnValue(Columns.Instructions, value); }
		}
		
		#endregion
		
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a AspnetUser ActiveRecord object related to this ProductAccessUser
		/// 
		/// </summary>
		private Wcss.AspnetUser AspnetUser
		{
			get { return Wcss.AspnetUser.FetchByID(this.UserId); }
			set { SetColumnValue("UserId", value.UserId); }
		}
        //set up an alternate mechanism to avoid a database call
		private Wcss.AspnetUser _aspnetuserrecord = null;
		
		public Wcss.AspnetUser AspnetUserRecord
		{
		    get
            {
                if (_aspnetuserrecord == null)
                {
                    _aspnetuserrecord = new Wcss.AspnetUser();
                    _aspnetuserrecord.CopyFrom(this.AspnetUser);
                }
                return _aspnetuserrecord;
            }
            set
            {
                if(value != null && _aspnetuserrecord == null)
			        _aspnetuserrecord = new Wcss.AspnetUser();
                
                SetColumnValue("UserId", value.UserId);
                _aspnetuserrecord.CopyFrom(value);                
            }
		}
		
		
		/// <summary>
		/// Returns a ProductAccess ActiveRecord object related to this ProductAccessUser
		/// 
		/// </summary>
		private Wcss.ProductAccess ProductAccess
		{
			get { return Wcss.ProductAccess.FetchByID(this.TProductAccessId); }
			set { SetColumnValue("TProductAccessId", value.Id); }
		}
        //set up an alternate mechanism to avoid a database call
		private Wcss.ProductAccess _productaccessrecord = null;
		
		public Wcss.ProductAccess ProductAccessRecord
		{
		    get
            {
                if (_productaccessrecord == null)
                {
                    _productaccessrecord = new Wcss.ProductAccess();
                    _productaccessrecord.CopyFrom(this.ProductAccess);
                }
                return _productaccessrecord;
            }
            set
            {
                if(value != null && _productaccessrecord == null)
			        _productaccessrecord = new Wcss.ProductAccess();
                
                SetColumnValue("TProductAccessId", value.Id);
                _productaccessrecord.CopyFrom(value);                
            }
		}
		
		
		#endregion
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(DateTime varDtStamp,int varTProductAccessId,string varUserName,Guid? varUserId,int varIQuantityAllowed,string varReferral,string varInstructions)
		{
			ProductAccessUser item = new ProductAccessUser();
			
			item.DtStamp = varDtStamp;
			
			item.TProductAccessId = varTProductAccessId;
			
			item.UserName = varUserName;
			
			item.UserId = varUserId;
			
			item.IQuantityAllowed = varIQuantityAllowed;
			
			item.Referral = varReferral;
			
			item.Instructions = varInstructions;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varId,DateTime varDtStamp,int varTProductAccessId,string varUserName,Guid? varUserId,int varIQuantityAllowed,string varReferral,string varInstructions)
		{
			ProductAccessUser item = new ProductAccessUser();
			
				item.Id = varId;
			
				item.DtStamp = varDtStamp;
			
				item.TProductAccessId = varTProductAccessId;
			
				item.UserName = varUserName;
			
				item.UserId = varUserId;
			
				item.IQuantityAllowed = varIQuantityAllowed;
			
				item.Referral = varReferral;
			
				item.Instructions = varInstructions;
			
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
        
        
        
        public static TableSchema.TableColumn TProductAccessIdColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn UserNameColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn UserIdColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn IQuantityAllowedColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn ReferralColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn InstructionsColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Id = @"Id";
			 public static string DtStamp = @"dtStamp";
			 public static string TProductAccessId = @"TProductAccessId";
			 public static string UserName = @"UserName";
			 public static string UserId = @"UserId";
			 public static string IQuantityAllowed = @"iQuantityAllowed";
			 public static string Referral = @"Referral";
			 public static string Instructions = @"Instructions";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
