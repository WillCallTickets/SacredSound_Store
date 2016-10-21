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
	/// Strongly-typed collection for the MailerContent class.
	/// </summary>
    [Serializable]
	public partial class MailerContentCollection : ActiveList<MailerContent, MailerContentCollection>
	{	   
		public MailerContentCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>MailerContentCollection</returns>
		public MailerContentCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                MailerContent o = this[i];
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
	/// This is an ActiveRecord class which wraps the MailerContent table.
	/// </summary>
	[Serializable]
	public partial class MailerContent : ActiveRecord<MailerContent>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public MailerContent()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public MailerContent(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public MailerContent(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public MailerContent(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("MailerContent", TableType.Table, DataService.GetInstance("WillCall"));
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
				
				TableSchema.TableColumn colvarTMailerId = new TableSchema.TableColumn(schema);
				colvarTMailerId.ColumnName = "tMailerId";
				colvarTMailerId.DataType = DbType.Int32;
				colvarTMailerId.MaxLength = 0;
				colvarTMailerId.AutoIncrement = false;
				colvarTMailerId.IsNullable = false;
				colvarTMailerId.IsPrimaryKey = false;
				colvarTMailerId.IsForeignKey = true;
				colvarTMailerId.IsReadOnly = false;
				colvarTMailerId.DefaultSetting = @"";
				
					colvarTMailerId.ForeignKeyTableName = "Mailer";
				schema.Columns.Add(colvarTMailerId);
				
				TableSchema.TableColumn colvarTMailerTemplateContentId = new TableSchema.TableColumn(schema);
				colvarTMailerTemplateContentId.ColumnName = "tMailerTemplateContentId";
				colvarTMailerTemplateContentId.DataType = DbType.Int32;
				colvarTMailerTemplateContentId.MaxLength = 0;
				colvarTMailerTemplateContentId.AutoIncrement = false;
				colvarTMailerTemplateContentId.IsNullable = false;
				colvarTMailerTemplateContentId.IsPrimaryKey = false;
				colvarTMailerTemplateContentId.IsForeignKey = true;
				colvarTMailerTemplateContentId.IsReadOnly = false;
				colvarTMailerTemplateContentId.DefaultSetting = @"";
				
					colvarTMailerTemplateContentId.ForeignKeyTableName = "MailerTemplateContent";
				schema.Columns.Add(colvarTMailerTemplateContentId);
				
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
				
				TableSchema.TableColumn colvarVcTitle = new TableSchema.TableColumn(schema);
				colvarVcTitle.ColumnName = "vcTitle";
				colvarVcTitle.DataType = DbType.AnsiString;
				colvarVcTitle.MaxLength = 500;
				colvarVcTitle.AutoIncrement = false;
				colvarVcTitle.IsNullable = true;
				colvarVcTitle.IsPrimaryKey = false;
				colvarVcTitle.IsForeignKey = false;
				colvarVcTitle.IsReadOnly = false;
				colvarVcTitle.DefaultSetting = @"";
				colvarVcTitle.ForeignKeyTableName = "";
				schema.Columns.Add(colvarVcTitle);
				
				TableSchema.TableColumn colvarVcContent = new TableSchema.TableColumn(schema);
				colvarVcContent.ColumnName = "vcContent";
				colvarVcContent.DataType = DbType.AnsiString;
				colvarVcContent.MaxLength = 4000;
				colvarVcContent.AutoIncrement = false;
				colvarVcContent.IsNullable = true;
				colvarVcContent.IsPrimaryKey = false;
				colvarVcContent.IsForeignKey = false;
				colvarVcContent.IsReadOnly = false;
				colvarVcContent.DefaultSetting = @"";
				colvarVcContent.ForeignKeyTableName = "";
				schema.Columns.Add(colvarVcContent);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["WillCall"].AddSchema("MailerContent",schema);
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
		  
		[XmlAttribute("TMailerId")]
		[Bindable(true)]
		public int TMailerId 
		{
			get { return GetColumnValue<int>(Columns.TMailerId); }
			set { SetColumnValue(Columns.TMailerId, value); }
		}
		  
		[XmlAttribute("TMailerTemplateContentId")]
		[Bindable(true)]
		public int TMailerTemplateContentId 
		{
			get { return GetColumnValue<int>(Columns.TMailerTemplateContentId); }
			set { SetColumnValue(Columns.TMailerTemplateContentId, value); }
		}
		  
		[XmlAttribute("BActive")]
		[Bindable(true)]
		public bool BActive 
		{
			get { return GetColumnValue<bool>(Columns.BActive); }
			set { SetColumnValue(Columns.BActive, value); }
		}
		  
		[XmlAttribute("VcTitle")]
		[Bindable(true)]
		public string VcTitle 
		{
			get { return GetColumnValue<string>(Columns.VcTitle); }
			set { SetColumnValue(Columns.VcTitle, value); }
		}
		  
		[XmlAttribute("VcContent")]
		[Bindable(true)]
		public string VcContent 
		{
			get { return GetColumnValue<string>(Columns.VcContent); }
			set { SetColumnValue(Columns.VcContent, value); }
		}
		
		#endregion
		
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a Mailer ActiveRecord object related to this MailerContent
		/// 
		/// </summary>
		private Wcss.Mailer Mailer
		{
			get { return Wcss.Mailer.FetchByID(this.TMailerId); }
			set { SetColumnValue("tMailerId", value.Id); }
		}
        //set up an alternate mechanism to avoid a database call
		private Wcss.Mailer _mailerrecord = null;
		
		public Wcss.Mailer MailerRecord
		{
		    get
            {
                if (_mailerrecord == null)
                {
                    _mailerrecord = new Wcss.Mailer();
                    _mailerrecord.CopyFrom(this.Mailer);
                }
                return _mailerrecord;
            }
            set
            {
                if(value != null && _mailerrecord == null)
			        _mailerrecord = new Wcss.Mailer();
                
                SetColumnValue("tMailerId", value.Id);
                _mailerrecord.CopyFrom(value);                
            }
		}
		
		
		/// <summary>
		/// Returns a MailerTemplateContent ActiveRecord object related to this MailerContent
		/// 
		/// </summary>
		private Wcss.MailerTemplateContent MailerTemplateContent
		{
			get { return Wcss.MailerTemplateContent.FetchByID(this.TMailerTemplateContentId); }
			set { SetColumnValue("tMailerTemplateContentId", value.Id); }
		}
        //set up an alternate mechanism to avoid a database call
		private Wcss.MailerTemplateContent _mailertemplatecontentrecord = null;
		
		public Wcss.MailerTemplateContent MailerTemplateContentRecord
		{
		    get
            {
                if (_mailertemplatecontentrecord == null)
                {
                    _mailertemplatecontentrecord = new Wcss.MailerTemplateContent();
                    _mailertemplatecontentrecord.CopyFrom(this.MailerTemplateContent);
                }
                return _mailertemplatecontentrecord;
            }
            set
            {
                if(value != null && _mailertemplatecontentrecord == null)
			        _mailertemplatecontentrecord = new Wcss.MailerTemplateContent();
                
                SetColumnValue("tMailerTemplateContentId", value.Id);
                _mailertemplatecontentrecord.CopyFrom(value);                
            }
		}
		
		
		#endregion
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(DateTime varDtStamp,int varTMailerId,int varTMailerTemplateContentId,bool varBActive,string varVcTitle,string varVcContent)
		{
			MailerContent item = new MailerContent();
			
			item.DtStamp = varDtStamp;
			
			item.TMailerId = varTMailerId;
			
			item.TMailerTemplateContentId = varTMailerTemplateContentId;
			
			item.BActive = varBActive;
			
			item.VcTitle = varVcTitle;
			
			item.VcContent = varVcContent;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varId,DateTime varDtStamp,int varTMailerId,int varTMailerTemplateContentId,bool varBActive,string varVcTitle,string varVcContent)
		{
			MailerContent item = new MailerContent();
			
				item.Id = varId;
			
				item.DtStamp = varDtStamp;
			
				item.TMailerId = varTMailerId;
			
				item.TMailerTemplateContentId = varTMailerTemplateContentId;
			
				item.BActive = varBActive;
			
				item.VcTitle = varVcTitle;
			
				item.VcContent = varVcContent;
			
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
        
        
        
        public static TableSchema.TableColumn TMailerIdColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn TMailerTemplateContentIdColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn BActiveColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn VcTitleColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn VcContentColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Id = @"Id";
			 public static string DtStamp = @"dtStamp";
			 public static string TMailerId = @"tMailerId";
			 public static string TMailerTemplateContentId = @"tMailerTemplateContentId";
			 public static string BActive = @"bActive";
			 public static string VcTitle = @"vcTitle";
			 public static string VcContent = @"vcContent";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
