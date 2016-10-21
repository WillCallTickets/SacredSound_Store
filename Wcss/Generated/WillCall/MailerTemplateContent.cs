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
	/// Strongly-typed collection for the MailerTemplateContent class.
	/// </summary>
    [Serializable]
	public partial class MailerTemplateContentCollection : ActiveList<MailerTemplateContent, MailerTemplateContentCollection>
	{	   
		public MailerTemplateContentCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>MailerTemplateContentCollection</returns>
		public MailerTemplateContentCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                MailerTemplateContent o = this[i];
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
	/// This is an ActiveRecord class which wraps the MailerTemplateContent table.
	/// </summary>
	[Serializable]
	public partial class MailerTemplateContent : ActiveRecord<MailerTemplateContent>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public MailerTemplateContent()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public MailerTemplateContent(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public MailerTemplateContent(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public MailerTemplateContent(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("MailerTemplateContent", TableType.Table, DataService.GetInstance("WillCall"));
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
				
				TableSchema.TableColumn colvarTMailerTemplateId = new TableSchema.TableColumn(schema);
				colvarTMailerTemplateId.ColumnName = "tMailerTemplateId";
				colvarTMailerTemplateId.DataType = DbType.Int32;
				colvarTMailerTemplateId.MaxLength = 0;
				colvarTMailerTemplateId.AutoIncrement = false;
				colvarTMailerTemplateId.IsNullable = false;
				colvarTMailerTemplateId.IsPrimaryKey = false;
				colvarTMailerTemplateId.IsForeignKey = true;
				colvarTMailerTemplateId.IsReadOnly = false;
				colvarTMailerTemplateId.DefaultSetting = @"";
				
					colvarTMailerTemplateId.ForeignKeyTableName = "MailerTemplate";
				schema.Columns.Add(colvarTMailerTemplateId);
				
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
				
				TableSchema.TableColumn colvarVcTemplateAsset = new TableSchema.TableColumn(schema);
				colvarVcTemplateAsset.ColumnName = "vcTemplateAsset";
				colvarVcTemplateAsset.DataType = DbType.AnsiString;
				colvarVcTemplateAsset.MaxLength = 256;
				colvarVcTemplateAsset.AutoIncrement = false;
				colvarVcTemplateAsset.IsNullable = false;
				colvarVcTemplateAsset.IsPrimaryKey = false;
				colvarVcTemplateAsset.IsForeignKey = false;
				colvarVcTemplateAsset.IsReadOnly = false;
				colvarVcTemplateAsset.DefaultSetting = @"";
				colvarVcTemplateAsset.ForeignKeyTableName = "";
				schema.Columns.Add(colvarVcTemplateAsset);
				
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
				
				TableSchema.TableColumn colvarTitle = new TableSchema.TableColumn(schema);
				colvarTitle.ColumnName = "Title";
				colvarTitle.DataType = DbType.AnsiString;
				colvarTitle.MaxLength = 256;
				colvarTitle.AutoIncrement = false;
				colvarTitle.IsNullable = true;
				colvarTitle.IsPrimaryKey = false;
				colvarTitle.IsForeignKey = false;
				colvarTitle.IsReadOnly = false;
				colvarTitle.DefaultSetting = @"";
				colvarTitle.ForeignKeyTableName = "";
				schema.Columns.Add(colvarTitle);
				
				TableSchema.TableColumn colvarTemplate = new TableSchema.TableColumn(schema);
				colvarTemplate.ColumnName = "Template";
				colvarTemplate.DataType = DbType.AnsiString;
				colvarTemplate.MaxLength = 3250;
				colvarTemplate.AutoIncrement = false;
				colvarTemplate.IsNullable = true;
				colvarTemplate.IsPrimaryKey = false;
				colvarTemplate.IsForeignKey = false;
				colvarTemplate.IsReadOnly = false;
				colvarTemplate.DefaultSetting = @"";
				colvarTemplate.ForeignKeyTableName = "";
				schema.Columns.Add(colvarTemplate);
				
				TableSchema.TableColumn colvarSeparatorTemplate = new TableSchema.TableColumn(schema);
				colvarSeparatorTemplate.ColumnName = "SeparatorTemplate";
				colvarSeparatorTemplate.DataType = DbType.AnsiString;
				colvarSeparatorTemplate.MaxLength = 500;
				colvarSeparatorTemplate.AutoIncrement = false;
				colvarSeparatorTemplate.IsNullable = true;
				colvarSeparatorTemplate.IsPrimaryKey = false;
				colvarSeparatorTemplate.IsForeignKey = false;
				colvarSeparatorTemplate.IsReadOnly = false;
				colvarSeparatorTemplate.DefaultSetting = @"";
				colvarSeparatorTemplate.ForeignKeyTableName = "";
				schema.Columns.Add(colvarSeparatorTemplate);
				
				TableSchema.TableColumn colvarIMaxListItems = new TableSchema.TableColumn(schema);
				colvarIMaxListItems.ColumnName = "iMaxListItems";
				colvarIMaxListItems.DataType = DbType.Int32;
				colvarIMaxListItems.MaxLength = 0;
				colvarIMaxListItems.AutoIncrement = false;
				colvarIMaxListItems.IsNullable = false;
				colvarIMaxListItems.IsPrimaryKey = false;
				colvarIMaxListItems.IsForeignKey = false;
				colvarIMaxListItems.IsReadOnly = false;
				
						colvarIMaxListItems.DefaultSetting = @"((0))";
				colvarIMaxListItems.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIMaxListItems);
				
				TableSchema.TableColumn colvarIMaxSelections = new TableSchema.TableColumn(schema);
				colvarIMaxSelections.ColumnName = "iMaxSelections";
				colvarIMaxSelections.DataType = DbType.Int32;
				colvarIMaxSelections.MaxLength = 0;
				colvarIMaxSelections.AutoIncrement = false;
				colvarIMaxSelections.IsNullable = false;
				colvarIMaxSelections.IsPrimaryKey = false;
				colvarIMaxSelections.IsForeignKey = false;
				colvarIMaxSelections.IsReadOnly = false;
				
						colvarIMaxSelections.DefaultSetting = @"((0))";
				colvarIMaxSelections.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIMaxSelections);
				
				TableSchema.TableColumn colvarVcFlags = new TableSchema.TableColumn(schema);
				colvarVcFlags.ColumnName = "vcFlags";
				colvarVcFlags.DataType = DbType.AnsiString;
				colvarVcFlags.MaxLength = 500;
				colvarVcFlags.AutoIncrement = false;
				colvarVcFlags.IsNullable = true;
				colvarVcFlags.IsPrimaryKey = false;
				colvarVcFlags.IsForeignKey = false;
				colvarVcFlags.IsReadOnly = false;
				colvarVcFlags.DefaultSetting = @"";
				colvarVcFlags.ForeignKeyTableName = "";
				schema.Columns.Add(colvarVcFlags);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["WillCall"].AddSchema("MailerTemplateContent",schema);
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
		  
		[XmlAttribute("TMailerTemplateId")]
		[Bindable(true)]
		public int TMailerTemplateId 
		{
			get { return GetColumnValue<int>(Columns.TMailerTemplateId); }
			set { SetColumnValue(Columns.TMailerTemplateId, value); }
		}
		  
		[XmlAttribute("IDisplayOrder")]
		[Bindable(true)]
		public int IDisplayOrder 
		{
			get { return GetColumnValue<int>(Columns.IDisplayOrder); }
			set { SetColumnValue(Columns.IDisplayOrder, value); }
		}
		  
		[XmlAttribute("VcTemplateAsset")]
		[Bindable(true)]
		public string VcTemplateAsset 
		{
			get { return GetColumnValue<string>(Columns.VcTemplateAsset); }
			set { SetColumnValue(Columns.VcTemplateAsset, value); }
		}
		  
		[XmlAttribute("Name")]
		[Bindable(true)]
		public string Name 
		{
			get { return GetColumnValue<string>(Columns.Name); }
			set { SetColumnValue(Columns.Name, value); }
		}
		  
		[XmlAttribute("Title")]
		[Bindable(true)]
		public string Title 
		{
			get { return GetColumnValue<string>(Columns.Title); }
			set { SetColumnValue(Columns.Title, value); }
		}
		  
		[XmlAttribute("Template")]
		[Bindable(true)]
		public string Template 
		{
			get { return GetColumnValue<string>(Columns.Template); }
			set { SetColumnValue(Columns.Template, value); }
		}
		  
		[XmlAttribute("SeparatorTemplate")]
		[Bindable(true)]
		public string SeparatorTemplate 
		{
			get { return GetColumnValue<string>(Columns.SeparatorTemplate); }
			set { SetColumnValue(Columns.SeparatorTemplate, value); }
		}
		  
		[XmlAttribute("IMaxListItems")]
		[Bindable(true)]
		public int IMaxListItems 
		{
			get { return GetColumnValue<int>(Columns.IMaxListItems); }
			set { SetColumnValue(Columns.IMaxListItems, value); }
		}
		  
		[XmlAttribute("IMaxSelections")]
		[Bindable(true)]
		public int IMaxSelections 
		{
			get { return GetColumnValue<int>(Columns.IMaxSelections); }
			set { SetColumnValue(Columns.IMaxSelections, value); }
		}
		  
		[XmlAttribute("VcFlags")]
		[Bindable(true)]
		public string VcFlags 
		{
			get { return GetColumnValue<string>(Columns.VcFlags); }
			set { SetColumnValue(Columns.VcFlags, value); }
		}
		
		#endregion
		
		
		#region PrimaryKey Methods		
		
        protected override void SetPrimaryKey(object oValue)
        {
            base.SetPrimaryKey(oValue);
            
            SetPKValues();
        }
        
		
		private Wcss.MailerContentCollection colMailerContentRecords;
		public Wcss.MailerContentCollection MailerContentRecords()
		{
			if(colMailerContentRecords == null)
			{
				colMailerContentRecords = new Wcss.MailerContentCollection().Where(MailerContent.Columns.TMailerTemplateContentId, Id).Load();
				colMailerContentRecords.ListChanged += new ListChangedEventHandler(colMailerContentRecords_ListChanged);
			}
			return colMailerContentRecords;
		}
				
		void colMailerContentRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colMailerContentRecords[e.NewIndex].TMailerTemplateContentId = Id;
				colMailerContentRecords.ListChanged += new ListChangedEventHandler(colMailerContentRecords_ListChanged);
            }
		}
		private Wcss.MailerTemplateSubstitutionCollection colMailerTemplateSubstitutionRecords;
		public Wcss.MailerTemplateSubstitutionCollection MailerTemplateSubstitutionRecords()
		{
			if(colMailerTemplateSubstitutionRecords == null)
			{
				colMailerTemplateSubstitutionRecords = new Wcss.MailerTemplateSubstitutionCollection().Where(MailerTemplateSubstitution.Columns.TMailerTemplateContentId, Id).Load();
				colMailerTemplateSubstitutionRecords.ListChanged += new ListChangedEventHandler(colMailerTemplateSubstitutionRecords_ListChanged);
			}
			return colMailerTemplateSubstitutionRecords;
		}
				
		void colMailerTemplateSubstitutionRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colMailerTemplateSubstitutionRecords[e.NewIndex].TMailerTemplateContentId = Id;
				colMailerTemplateSubstitutionRecords.ListChanged += new ListChangedEventHandler(colMailerTemplateSubstitutionRecords_ListChanged);
            }
		}
		#endregion
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a MailerTemplate ActiveRecord object related to this MailerTemplateContent
		/// 
		/// </summary>
		private Wcss.MailerTemplate MailerTemplate
		{
			get { return Wcss.MailerTemplate.FetchByID(this.TMailerTemplateId); }
			set { SetColumnValue("tMailerTemplateId", value.Id); }
		}
        //set up an alternate mechanism to avoid a database call
		private Wcss.MailerTemplate _mailertemplaterecord = null;
		
		public Wcss.MailerTemplate MailerTemplateRecord
		{
		    get
            {
                if (_mailertemplaterecord == null)
                {
                    _mailertemplaterecord = new Wcss.MailerTemplate();
                    _mailertemplaterecord.CopyFrom(this.MailerTemplate);
                }
                return _mailertemplaterecord;
            }
            set
            {
                if(value != null && _mailertemplaterecord == null)
			        _mailertemplaterecord = new Wcss.MailerTemplate();
                
                SetColumnValue("tMailerTemplateId", value.Id);
                _mailertemplaterecord.CopyFrom(value);                
            }
		}
		
		
		#endregion
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(DateTime varDtStamp,int varTMailerTemplateId,int varIDisplayOrder,string varVcTemplateAsset,string varName,string varTitle,string varTemplate,string varSeparatorTemplate,int varIMaxListItems,int varIMaxSelections,string varVcFlags)
		{
			MailerTemplateContent item = new MailerTemplateContent();
			
			item.DtStamp = varDtStamp;
			
			item.TMailerTemplateId = varTMailerTemplateId;
			
			item.IDisplayOrder = varIDisplayOrder;
			
			item.VcTemplateAsset = varVcTemplateAsset;
			
			item.Name = varName;
			
			item.Title = varTitle;
			
			item.Template = varTemplate;
			
			item.SeparatorTemplate = varSeparatorTemplate;
			
			item.IMaxListItems = varIMaxListItems;
			
			item.IMaxSelections = varIMaxSelections;
			
			item.VcFlags = varVcFlags;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varId,DateTime varDtStamp,int varTMailerTemplateId,int varIDisplayOrder,string varVcTemplateAsset,string varName,string varTitle,string varTemplate,string varSeparatorTemplate,int varIMaxListItems,int varIMaxSelections,string varVcFlags)
		{
			MailerTemplateContent item = new MailerTemplateContent();
			
				item.Id = varId;
			
				item.DtStamp = varDtStamp;
			
				item.TMailerTemplateId = varTMailerTemplateId;
			
				item.IDisplayOrder = varIDisplayOrder;
			
				item.VcTemplateAsset = varVcTemplateAsset;
			
				item.Name = varName;
			
				item.Title = varTitle;
			
				item.Template = varTemplate;
			
				item.SeparatorTemplate = varSeparatorTemplate;
			
				item.IMaxListItems = varIMaxListItems;
			
				item.IMaxSelections = varIMaxSelections;
			
				item.VcFlags = varVcFlags;
			
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
        
        
        
        public static TableSchema.TableColumn TMailerTemplateIdColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn IDisplayOrderColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn VcTemplateAssetColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn NameColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn TitleColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn TemplateColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn SeparatorTemplateColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn IMaxListItemsColumn
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        public static TableSchema.TableColumn IMaxSelectionsColumn
        {
            get { return Schema.Columns[10]; }
        }
        
        
        
        public static TableSchema.TableColumn VcFlagsColumn
        {
            get { return Schema.Columns[11]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Id = @"Id";
			 public static string DtStamp = @"dtStamp";
			 public static string TMailerTemplateId = @"tMailerTemplateId";
			 public static string IDisplayOrder = @"iDisplayOrder";
			 public static string VcTemplateAsset = @"vcTemplateAsset";
			 public static string Name = @"Name";
			 public static string Title = @"Title";
			 public static string Template = @"Template";
			 public static string SeparatorTemplate = @"SeparatorTemplate";
			 public static string IMaxListItems = @"iMaxListItems";
			 public static string IMaxSelections = @"iMaxSelections";
			 public static string VcFlags = @"vcFlags";
						
		}
		#endregion
		
		#region Update PK Collections
		
        public void SetPKValues()
        {
                if (colMailerContentRecords != null)
                {
                    foreach (Wcss.MailerContent item in colMailerContentRecords)
                    {
                        if (item.TMailerTemplateContentId != Id)
                        {
                            item.TMailerTemplateContentId = Id;
                        }
                    }
               }
		
                if (colMailerTemplateSubstitutionRecords != null)
                {
                    foreach (Wcss.MailerTemplateSubstitution item in colMailerTemplateSubstitutionRecords)
                    {
                        if (item.TMailerTemplateContentId != Id)
                        {
                            item.TMailerTemplateContentId = Id;
                        }
                    }
               }
		}
        #endregion
    
        #region Deep Save
		
        public void DeepSave()
        {
            Save();
            
                if (colMailerContentRecords != null)
                {
                    colMailerContentRecords.SaveAll();
               }
		
                if (colMailerTemplateSubstitutionRecords != null)
                {
                    colMailerTemplateSubstitutionRecords.SaveAll();
               }
		}
        #endregion
	}
}
