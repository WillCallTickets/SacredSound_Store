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
	/// Strongly-typed collection for the Mailer class.
	/// </summary>
    [Serializable]
	public partial class MailerCollection : ActiveList<Mailer, MailerCollection>
	{	   
		public MailerCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>MailerCollection</returns>
		public MailerCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                Mailer o = this[i];
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
	/// This is an ActiveRecord class which wraps the Mailer table.
	/// </summary>
	[Serializable]
	public partial class Mailer : ActiveRecord<Mailer>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public Mailer()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public Mailer(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public Mailer(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public Mailer(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("Mailer", TableType.Table, DataService.GetInstance("WillCall"));
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
				
				TableSchema.TableColumn colvarSubject = new TableSchema.TableColumn(schema);
				colvarSubject.ColumnName = "Subject";
				colvarSubject.DataType = DbType.AnsiString;
				colvarSubject.MaxLength = 256;
				colvarSubject.AutoIncrement = false;
				colvarSubject.IsNullable = true;
				colvarSubject.IsPrimaryKey = false;
				colvarSubject.IsForeignKey = false;
				colvarSubject.IsReadOnly = false;
				colvarSubject.DefaultSetting = @"";
				colvarSubject.ForeignKeyTableName = "";
				schema.Columns.Add(colvarSubject);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["WillCall"].AddSchema("Mailer",schema);
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
		  
		[XmlAttribute("Name")]
		[Bindable(true)]
		public string Name 
		{
			get { return GetColumnValue<string>(Columns.Name); }
			set { SetColumnValue(Columns.Name, value); }
		}
		  
		[XmlAttribute("Subject")]
		[Bindable(true)]
		public string Subject 
		{
			get { return GetColumnValue<string>(Columns.Subject); }
			set { SetColumnValue(Columns.Subject, value); }
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
				colMailerContentRecords = new Wcss.MailerContentCollection().Where(MailerContent.Columns.TMailerId, Id).Load();
				colMailerContentRecords.ListChanged += new ListChangedEventHandler(colMailerContentRecords_ListChanged);
			}
			return colMailerContentRecords;
		}
				
		void colMailerContentRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colMailerContentRecords[e.NewIndex].TMailerId = Id;
				colMailerContentRecords.ListChanged += new ListChangedEventHandler(colMailerContentRecords_ListChanged);
            }
		}
		#endregion
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a MailerTemplate ActiveRecord object related to this Mailer
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
		public static void Insert(DateTime varDtStamp,int varTMailerTemplateId,string varName,string varSubject)
		{
			Mailer item = new Mailer();
			
			item.DtStamp = varDtStamp;
			
			item.TMailerTemplateId = varTMailerTemplateId;
			
			item.Name = varName;
			
			item.Subject = varSubject;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varId,DateTime varDtStamp,int varTMailerTemplateId,string varName,string varSubject)
		{
			Mailer item = new Mailer();
			
				item.Id = varId;
			
				item.DtStamp = varDtStamp;
			
				item.TMailerTemplateId = varTMailerTemplateId;
			
				item.Name = varName;
			
				item.Subject = varSubject;
			
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
        
        
        
        public static TableSchema.TableColumn NameColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn SubjectColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Id = @"Id";
			 public static string DtStamp = @"dtStamp";
			 public static string TMailerTemplateId = @"tMailerTemplateId";
			 public static string Name = @"Name";
			 public static string Subject = @"Subject";
						
		}
		#endregion
		
		#region Update PK Collections
		
        public void SetPKValues()
        {
                if (colMailerContentRecords != null)
                {
                    foreach (Wcss.MailerContent item in colMailerContentRecords)
                    {
                        if (item.TMailerId != Id)
                        {
                            item.TMailerId = Id;
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
		}
        #endregion
	}
}
