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
	/// Strongly-typed collection for the MailerTemplate class.
	/// </summary>
    [Serializable]
	public partial class MailerTemplateCollection : ActiveList<MailerTemplate, MailerTemplateCollection>
	{	   
		public MailerTemplateCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>MailerTemplateCollection</returns>
		public MailerTemplateCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                MailerTemplate o = this[i];
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
	/// This is an ActiveRecord class which wraps the MailerTemplate table.
	/// </summary>
	[Serializable]
	public partial class MailerTemplate : ActiveRecord<MailerTemplate>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public MailerTemplate()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public MailerTemplate(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public MailerTemplate(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public MailerTemplate(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("MailerTemplate", TableType.Table, DataService.GetInstance("WillCall"));
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
				colvarApplicationId.IsForeignKey = false;
				colvarApplicationId.IsReadOnly = false;
				colvarApplicationId.DefaultSetting = @"";
				colvarApplicationId.ForeignKeyTableName = "";
				schema.Columns.Add(colvarApplicationId);
				
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
				
				TableSchema.TableColumn colvarStyle = new TableSchema.TableColumn(schema);
				colvarStyle.ColumnName = "Style";
				colvarStyle.DataType = DbType.AnsiString;
				colvarStyle.MaxLength = 500;
				colvarStyle.AutoIncrement = false;
				colvarStyle.IsNullable = true;
				colvarStyle.IsPrimaryKey = false;
				colvarStyle.IsForeignKey = false;
				colvarStyle.IsReadOnly = false;
				colvarStyle.DefaultSetting = @"";
				colvarStyle.ForeignKeyTableName = "";
				schema.Columns.Add(colvarStyle);
				
				TableSchema.TableColumn colvarHeader = new TableSchema.TableColumn(schema);
				colvarHeader.ColumnName = "Header";
				colvarHeader.DataType = DbType.AnsiString;
				colvarHeader.MaxLength = 3250;
				colvarHeader.AutoIncrement = false;
				colvarHeader.IsNullable = false;
				colvarHeader.IsPrimaryKey = false;
				colvarHeader.IsForeignKey = false;
				colvarHeader.IsReadOnly = false;
				colvarHeader.DefaultSetting = @"";
				colvarHeader.ForeignKeyTableName = "";
				schema.Columns.Add(colvarHeader);
				
				TableSchema.TableColumn colvarFooter = new TableSchema.TableColumn(schema);
				colvarFooter.ColumnName = "Footer";
				colvarFooter.DataType = DbType.AnsiString;
				colvarFooter.MaxLength = 3250;
				colvarFooter.AutoIncrement = false;
				colvarFooter.IsNullable = false;
				colvarFooter.IsPrimaryKey = false;
				colvarFooter.IsForeignKey = false;
				colvarFooter.IsReadOnly = false;
				colvarFooter.DefaultSetting = @"";
				colvarFooter.ForeignKeyTableName = "";
				schema.Columns.Add(colvarFooter);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["WillCall"].AddSchema("MailerTemplate",schema);
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
		  
		[XmlAttribute("Style")]
		[Bindable(true)]
		public string Style 
		{
			get { return GetColumnValue<string>(Columns.Style); }
			set { SetColumnValue(Columns.Style, value); }
		}
		  
		[XmlAttribute("Header")]
		[Bindable(true)]
		public string Header 
		{
			get { return GetColumnValue<string>(Columns.Header); }
			set { SetColumnValue(Columns.Header, value); }
		}
		  
		[XmlAttribute("Footer")]
		[Bindable(true)]
		public string Footer 
		{
			get { return GetColumnValue<string>(Columns.Footer); }
			set { SetColumnValue(Columns.Footer, value); }
		}
		
		#endregion
		
		
		#region PrimaryKey Methods		
		
        protected override void SetPrimaryKey(object oValue)
        {
            base.SetPrimaryKey(oValue);
            
            SetPKValues();
        }
        
		
		private Wcss.MailerCollection colMailerRecords;
		public Wcss.MailerCollection MailerRecords()
		{
			if(colMailerRecords == null)
			{
				colMailerRecords = new Wcss.MailerCollection().Where(Mailer.Columns.TMailerTemplateId, Id).Load();
				colMailerRecords.ListChanged += new ListChangedEventHandler(colMailerRecords_ListChanged);
			}
			return colMailerRecords;
		}
				
		void colMailerRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colMailerRecords[e.NewIndex].TMailerTemplateId = Id;
				colMailerRecords.ListChanged += new ListChangedEventHandler(colMailerRecords_ListChanged);
            }
		}
		private Wcss.MailerTemplateContentCollection colMailerTemplateContentRecords;
		public Wcss.MailerTemplateContentCollection MailerTemplateContentRecords()
		{
			if(colMailerTemplateContentRecords == null)
			{
				colMailerTemplateContentRecords = new Wcss.MailerTemplateContentCollection().Where(MailerTemplateContent.Columns.TMailerTemplateId, Id).Load();
				colMailerTemplateContentRecords.ListChanged += new ListChangedEventHandler(colMailerTemplateContentRecords_ListChanged);
			}
			return colMailerTemplateContentRecords;
		}
				
		void colMailerTemplateContentRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colMailerTemplateContentRecords[e.NewIndex].TMailerTemplateId = Id;
				colMailerTemplateContentRecords.ListChanged += new ListChangedEventHandler(colMailerTemplateContentRecords_ListChanged);
            }
		}
		#endregion
		
			
		
		//no foreign key tables defined (0)
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(DateTime varDtStamp,Guid varApplicationId,string varName,string varDescription,string varStyle,string varHeader,string varFooter)
		{
			MailerTemplate item = new MailerTemplate();
			
			item.DtStamp = varDtStamp;
			
			item.ApplicationId = varApplicationId;
			
			item.Name = varName;
			
			item.Description = varDescription;
			
			item.Style = varStyle;
			
			item.Header = varHeader;
			
			item.Footer = varFooter;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varId,DateTime varDtStamp,Guid varApplicationId,string varName,string varDescription,string varStyle,string varHeader,string varFooter)
		{
			MailerTemplate item = new MailerTemplate();
			
				item.Id = varId;
			
				item.DtStamp = varDtStamp;
			
				item.ApplicationId = varApplicationId;
			
				item.Name = varName;
			
				item.Description = varDescription;
			
				item.Style = varStyle;
			
				item.Header = varHeader;
			
				item.Footer = varFooter;
			
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
        
        
        
        public static TableSchema.TableColumn NameColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn DescriptionColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn StyleColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn HeaderColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn FooterColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Id = @"Id";
			 public static string DtStamp = @"dtStamp";
			 public static string ApplicationId = @"ApplicationId";
			 public static string Name = @"Name";
			 public static string Description = @"Description";
			 public static string Style = @"Style";
			 public static string Header = @"Header";
			 public static string Footer = @"Footer";
						
		}
		#endregion
		
		#region Update PK Collections
		
        public void SetPKValues()
        {
                if (colMailerRecords != null)
                {
                    foreach (Wcss.Mailer item in colMailerRecords)
                    {
                        if (item.TMailerTemplateId != Id)
                        {
                            item.TMailerTemplateId = Id;
                        }
                    }
               }
		
                if (colMailerTemplateContentRecords != null)
                {
                    foreach (Wcss.MailerTemplateContent item in colMailerTemplateContentRecords)
                    {
                        if (item.TMailerTemplateId != Id)
                        {
                            item.TMailerTemplateId = Id;
                        }
                    }
               }
		}
        #endregion
    
        #region Deep Save
		
        public void DeepSave()
        {
            Save();
            
                if (colMailerRecords != null)
                {
                    colMailerRecords.SaveAll();
               }
		
                if (colMailerTemplateContentRecords != null)
                {
                    colMailerTemplateContentRecords.SaveAll();
               }
		}
        #endregion
	}
}
