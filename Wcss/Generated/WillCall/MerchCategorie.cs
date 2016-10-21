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
	/// Strongly-typed collection for the MerchCategorie class.
	/// </summary>
    [Serializable]
	public partial class MerchCategorieCollection : ActiveList<MerchCategorie, MerchCategorieCollection>
	{	   
		public MerchCategorieCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>MerchCategorieCollection</returns>
		public MerchCategorieCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                MerchCategorie o = this[i];
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
	/// This is an ActiveRecord class which wraps the MerchCategorie table.
	/// </summary>
	[Serializable]
	public partial class MerchCategorie : ActiveRecord<MerchCategorie>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public MerchCategorie()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public MerchCategorie(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public MerchCategorie(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public MerchCategorie(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("MerchCategorie", TableType.Table, DataService.GetInstance("WillCall"));
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
				
				TableSchema.TableColumn colvarTMerchDivisionId = new TableSchema.TableColumn(schema);
				colvarTMerchDivisionId.ColumnName = "tMerchDivisionId";
				colvarTMerchDivisionId.DataType = DbType.Int32;
				colvarTMerchDivisionId.MaxLength = 0;
				colvarTMerchDivisionId.AutoIncrement = false;
				colvarTMerchDivisionId.IsNullable = false;
				colvarTMerchDivisionId.IsPrimaryKey = false;
				colvarTMerchDivisionId.IsForeignKey = true;
				colvarTMerchDivisionId.IsReadOnly = false;
				colvarTMerchDivisionId.DefaultSetting = @"";
				
					colvarTMerchDivisionId.ForeignKeyTableName = "MerchDivision";
				schema.Columns.Add(colvarTMerchDivisionId);
				
				TableSchema.TableColumn colvarBInternalOnly = new TableSchema.TableColumn(schema);
				colvarBInternalOnly.ColumnName = "bInternalOnly";
				colvarBInternalOnly.DataType = DbType.Boolean;
				colvarBInternalOnly.MaxLength = 0;
				colvarBInternalOnly.AutoIncrement = false;
				colvarBInternalOnly.IsNullable = false;
				colvarBInternalOnly.IsPrimaryKey = false;
				colvarBInternalOnly.IsForeignKey = false;
				colvarBInternalOnly.IsReadOnly = false;
				
						colvarBInternalOnly.DefaultSetting = @"((0))";
				colvarBInternalOnly.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBInternalOnly);
				
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
				
				TableSchema.TableColumn colvarDescription = new TableSchema.TableColumn(schema);
				colvarDescription.ColumnName = "Description";
				colvarDescription.DataType = DbType.AnsiString;
				colvarDescription.MaxLength = 2000;
				colvarDescription.AutoIncrement = false;
				colvarDescription.IsNullable = true;
				colvarDescription.IsPrimaryKey = false;
				colvarDescription.IsForeignKey = false;
				colvarDescription.IsReadOnly = false;
				colvarDescription.DefaultSetting = @"";
				colvarDescription.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDescription);
				
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
				DataService.Providers["WillCall"].AddSchema("MerchCategorie",schema);
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
		  
		[XmlAttribute("Name")]
		[Bindable(true)]
		public string Name 
		{
			get { return GetColumnValue<string>(Columns.Name); }
			set { SetColumnValue(Columns.Name, value); }
		}
		  
		[XmlAttribute("TMerchDivisionId")]
		[Bindable(true)]
		public int TMerchDivisionId 
		{
			get { return GetColumnValue<int>(Columns.TMerchDivisionId); }
			set { SetColumnValue(Columns.TMerchDivisionId, value); }
		}
		  
		[XmlAttribute("BInternalOnly")]
		[Bindable(true)]
		public bool BInternalOnly 
		{
			get { return GetColumnValue<bool>(Columns.BInternalOnly); }
			set { SetColumnValue(Columns.BInternalOnly, value); }
		}
		  
		[XmlAttribute("IDisplayOrder")]
		[Bindable(true)]
		public int IDisplayOrder 
		{
			get { return GetColumnValue<int>(Columns.IDisplayOrder); }
			set { SetColumnValue(Columns.IDisplayOrder, value); }
		}
		  
		[XmlAttribute("Description")]
		[Bindable(true)]
		public string Description 
		{
			get { return GetColumnValue<string>(Columns.Description); }
			set { SetColumnValue(Columns.Description, value); }
		}
		  
		[XmlAttribute("DtStamp")]
		[Bindable(true)]
		public DateTime DtStamp 
		{
			get { return GetColumnValue<DateTime>(Columns.DtStamp); }
			set { SetColumnValue(Columns.DtStamp, value); }
		}
		
		#endregion
		
		
		#region PrimaryKey Methods		
		
        protected override void SetPrimaryKey(object oValue)
        {
            base.SetPrimaryKey(oValue);
            
            SetPKValues();
        }
        
		
		private Wcss.MerchJoinCatCollection colMerchJoinCatRecords;
		public Wcss.MerchJoinCatCollection MerchJoinCatRecords()
		{
			if(colMerchJoinCatRecords == null)
			{
				colMerchJoinCatRecords = new Wcss.MerchJoinCatCollection().Where(MerchJoinCat.Columns.TMerchCategorieId, Id).Load();
				colMerchJoinCatRecords.ListChanged += new ListChangedEventHandler(colMerchJoinCatRecords_ListChanged);
			}
			return colMerchJoinCatRecords;
		}
				
		void colMerchJoinCatRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colMerchJoinCatRecords[e.NewIndex].TMerchCategorieId = Id;
				colMerchJoinCatRecords.ListChanged += new ListChangedEventHandler(colMerchJoinCatRecords_ListChanged);
            }
		}
		#endregion
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a MerchDivision ActiveRecord object related to this MerchCategorie
		/// 
		/// </summary>
		private Wcss.MerchDivision MerchDivision
		{
			get { return Wcss.MerchDivision.FetchByID(this.TMerchDivisionId); }
			set { SetColumnValue("tMerchDivisionId", value.Id); }
		}
        //set up an alternate mechanism to avoid a database call
		private Wcss.MerchDivision _merchdivisionrecord = null;
		
		public Wcss.MerchDivision MerchDivisionRecord
		{
		    get
            {
                if (_merchdivisionrecord == null)
                {
                    _merchdivisionrecord = new Wcss.MerchDivision();
                    _merchdivisionrecord.CopyFrom(this.MerchDivision);
                }
                return _merchdivisionrecord;
            }
            set
            {
                if(value != null && _merchdivisionrecord == null)
			        _merchdivisionrecord = new Wcss.MerchDivision();
                
                SetColumnValue("tMerchDivisionId", value.Id);
                _merchdivisionrecord.CopyFrom(value);                
            }
		}
		
		
		#endregion
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(string varName,int varTMerchDivisionId,bool varBInternalOnly,int varIDisplayOrder,string varDescription,DateTime varDtStamp)
		{
			MerchCategorie item = new MerchCategorie();
			
			item.Name = varName;
			
			item.TMerchDivisionId = varTMerchDivisionId;
			
			item.BInternalOnly = varBInternalOnly;
			
			item.IDisplayOrder = varIDisplayOrder;
			
			item.Description = varDescription;
			
			item.DtStamp = varDtStamp;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varId,string varName,int varTMerchDivisionId,bool varBInternalOnly,int varIDisplayOrder,string varDescription,DateTime varDtStamp)
		{
			MerchCategorie item = new MerchCategorie();
			
				item.Id = varId;
			
				item.Name = varName;
			
				item.TMerchDivisionId = varTMerchDivisionId;
			
				item.BInternalOnly = varBInternalOnly;
			
				item.IDisplayOrder = varIDisplayOrder;
			
				item.Description = varDescription;
			
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
        
        
        
        public static TableSchema.TableColumn NameColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn TMerchDivisionIdColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn BInternalOnlyColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn IDisplayOrderColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn DescriptionColumn
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
			 public static string Name = @"Name";
			 public static string TMerchDivisionId = @"tMerchDivisionId";
			 public static string BInternalOnly = @"bInternalOnly";
			 public static string IDisplayOrder = @"iDisplayOrder";
			 public static string Description = @"Description";
			 public static string DtStamp = @"dtStamp";
						
		}
		#endregion
		
		#region Update PK Collections
		
        public void SetPKValues()
        {
                if (colMerchJoinCatRecords != null)
                {
                    foreach (Wcss.MerchJoinCat item in colMerchJoinCatRecords)
                    {
                        if (item.TMerchCategorieId != Id)
                        {
                            item.TMerchCategorieId = Id;
                        }
                    }
               }
		}
        #endregion
    
        #region Deep Save
		
        public void DeepSave()
        {
            Save();
            
                if (colMerchJoinCatRecords != null)
                {
                    colMerchJoinCatRecords.SaveAll();
               }
		}
        #endregion
	}
}
