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
	/// Strongly-typed collection for the ProductAccess class.
	/// </summary>
    [Serializable]
	public partial class ProductAccessCollection : ActiveList<ProductAccess, ProductAccessCollection>
	{	   
		public ProductAccessCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>ProductAccessCollection</returns>
		public ProductAccessCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                ProductAccess o = this[i];
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
	/// This is an ActiveRecord class which wraps the ProductAccess table.
	/// </summary>
	[Serializable]
	public partial class ProductAccess : ActiveRecord<ProductAccess>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public ProductAccess()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public ProductAccess(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public ProductAccess(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public ProductAccess(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("ProductAccess", TableType.Table, DataService.GetInstance("WillCall"));
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
				
				TableSchema.TableColumn colvarCampaignName = new TableSchema.TableColumn(schema);
				colvarCampaignName.ColumnName = "CampaignName";
				colvarCampaignName.DataType = DbType.AnsiString;
				colvarCampaignName.MaxLength = 512;
				colvarCampaignName.AutoIncrement = false;
				colvarCampaignName.IsNullable = false;
				colvarCampaignName.IsPrimaryKey = false;
				colvarCampaignName.IsForeignKey = false;
				colvarCampaignName.IsReadOnly = false;
				colvarCampaignName.DefaultSetting = @"";
				colvarCampaignName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCampaignName);
				
				TableSchema.TableColumn colvarCampaignCode = new TableSchema.TableColumn(schema);
				colvarCampaignCode.ColumnName = "CampaignCode";
				colvarCampaignCode.DataType = DbType.AnsiString;
				colvarCampaignCode.MaxLength = 50;
				colvarCampaignCode.AutoIncrement = false;
				colvarCampaignCode.IsNullable = false;
				colvarCampaignCode.IsPrimaryKey = false;
				colvarCampaignCode.IsForeignKey = false;
				colvarCampaignCode.IsReadOnly = false;
				colvarCampaignCode.DefaultSetting = @"";
				colvarCampaignCode.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCampaignCode);
				
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
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["WillCall"].AddSchema("ProductAccess",schema);
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
		  
		[XmlAttribute("ApplicationId")]
		[Bindable(true)]
		public Guid ApplicationId 
		{
			get { return GetColumnValue<Guid>(Columns.ApplicationId); }
			set { SetColumnValue(Columns.ApplicationId, value); }
		}
		  
		[XmlAttribute("BActive")]
		[Bindable(true)]
		public bool BActive 
		{
			get { return GetColumnValue<bool>(Columns.BActive); }
			set { SetColumnValue(Columns.BActive, value); }
		}
		  
		[XmlAttribute("CampaignName")]
		[Bindable(true)]
		public string CampaignName 
		{
			get { return GetColumnValue<string>(Columns.CampaignName); }
			set { SetColumnValue(Columns.CampaignName, value); }
		}
		  
		[XmlAttribute("CampaignCode")]
		[Bindable(true)]
		public string CampaignCode 
		{
			get { return GetColumnValue<string>(Columns.CampaignCode); }
			set { SetColumnValue(Columns.CampaignCode, value); }
		}
		  
		[XmlAttribute("IDisplayOrder")]
		[Bindable(true)]
		public int IDisplayOrder 
		{
			get { return GetColumnValue<int>(Columns.IDisplayOrder); }
			set { SetColumnValue(Columns.IDisplayOrder, value); }
		}
		
		#endregion
		
		
		#region PrimaryKey Methods		
		
        protected override void SetPrimaryKey(object oValue)
        {
            base.SetPrimaryKey(oValue);
            
            SetPKValues();
        }
        
		
		private Wcss.ProductAccessProductCollection colProductAccessProductRecords;
		public Wcss.ProductAccessProductCollection ProductAccessProductRecords()
		{
			if(colProductAccessProductRecords == null)
			{
				colProductAccessProductRecords = new Wcss.ProductAccessProductCollection().Where(ProductAccessProduct.Columns.TProductAccessId, Id).Load();
				colProductAccessProductRecords.ListChanged += new ListChangedEventHandler(colProductAccessProductRecords_ListChanged);
			}
			return colProductAccessProductRecords;
		}
				
		void colProductAccessProductRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colProductAccessProductRecords[e.NewIndex].TProductAccessId = Id;
				colProductAccessProductRecords.ListChanged += new ListChangedEventHandler(colProductAccessProductRecords_ListChanged);
            }
		}
		private Wcss.ProductAccessUserCollection colProductAccessUserRecords;
		public Wcss.ProductAccessUserCollection ProductAccessUserRecords()
		{
			if(colProductAccessUserRecords == null)
			{
				colProductAccessUserRecords = new Wcss.ProductAccessUserCollection().Where(ProductAccessUser.Columns.TProductAccessId, Id).Load();
				colProductAccessUserRecords.ListChanged += new ListChangedEventHandler(colProductAccessUserRecords_ListChanged);
			}
			return colProductAccessUserRecords;
		}
				
		void colProductAccessUserRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colProductAccessUserRecords[e.NewIndex].TProductAccessId = Id;
				colProductAccessUserRecords.ListChanged += new ListChangedEventHandler(colProductAccessUserRecords_ListChanged);
            }
		}
		#endregion
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a AspnetApplication ActiveRecord object related to this ProductAccess
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
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(DateTime varDtStamp,Guid varApplicationId,bool varBActive,string varCampaignName,string varCampaignCode,int varIDisplayOrder)
		{
			ProductAccess item = new ProductAccess();
			
			item.DtStamp = varDtStamp;
			
			item.ApplicationId = varApplicationId;
			
			item.BActive = varBActive;
			
			item.CampaignName = varCampaignName;
			
			item.CampaignCode = varCampaignCode;
			
			item.IDisplayOrder = varIDisplayOrder;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varId,DateTime varDtStamp,Guid varApplicationId,bool varBActive,string varCampaignName,string varCampaignCode,int varIDisplayOrder)
		{
			ProductAccess item = new ProductAccess();
			
				item.Id = varId;
			
				item.DtStamp = varDtStamp;
			
				item.ApplicationId = varApplicationId;
			
				item.BActive = varBActive;
			
				item.CampaignName = varCampaignName;
			
				item.CampaignCode = varCampaignCode;
			
				item.IDisplayOrder = varIDisplayOrder;
			
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
        
        
        
        public static TableSchema.TableColumn ApplicationIdColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn BActiveColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn CampaignNameColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn CampaignCodeColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn IDisplayOrderColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Id = @"Id";
			 public static string DtStamp = @"dtStamp";
			 public static string ApplicationId = @"ApplicationId";
			 public static string BActive = @"bActive";
			 public static string CampaignName = @"CampaignName";
			 public static string CampaignCode = @"CampaignCode";
			 public static string IDisplayOrder = @"iDisplayOrder";
						
		}
		#endregion
		
		#region Update PK Collections
		
        public void SetPKValues()
        {
                if (colProductAccessProductRecords != null)
                {
                    foreach (Wcss.ProductAccessProduct item in colProductAccessProductRecords)
                    {
                        if (item.TProductAccessId != Id)
                        {
                            item.TProductAccessId = Id;
                        }
                    }
               }
		
                if (colProductAccessUserRecords != null)
                {
                    foreach (Wcss.ProductAccessUser item in colProductAccessUserRecords)
                    {
                        if (item.TProductAccessId != Id)
                        {
                            item.TProductAccessId = Id;
                        }
                    }
               }
		}
        #endregion
    
        #region Deep Save
		
        public void DeepSave()
        {
            Save();
            
                if (colProductAccessProductRecords != null)
                {
                    colProductAccessProductRecords.SaveAll();
               }
		
                if (colProductAccessUserRecords != null)
                {
                    colProductAccessUserRecords.SaveAll();
               }
		}
        #endregion
	}
}
