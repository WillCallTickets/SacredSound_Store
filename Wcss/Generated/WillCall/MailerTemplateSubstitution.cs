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
	/// Strongly-typed collection for the MailerTemplateSubstitution class.
	/// </summary>
    [Serializable]
	public partial class MailerTemplateSubstitutionCollection : ActiveList<MailerTemplateSubstitution, MailerTemplateSubstitutionCollection>
	{	   
		public MailerTemplateSubstitutionCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>MailerTemplateSubstitutionCollection</returns>
		public MailerTemplateSubstitutionCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                MailerTemplateSubstitution o = this[i];
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
	/// This is an ActiveRecord class which wraps the MailerTemplateSubstitution table.
	/// </summary>
	[Serializable]
	public partial class MailerTemplateSubstitution : ActiveRecord<MailerTemplateSubstitution>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public MailerTemplateSubstitution()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public MailerTemplateSubstitution(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public MailerTemplateSubstitution(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public MailerTemplateSubstitution(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("MailerTemplateSubstitution", TableType.Table, DataService.GetInstance("WillCall"));
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
				
				TableSchema.TableColumn colvarTagName = new TableSchema.TableColumn(schema);
				colvarTagName.ColumnName = "TagName";
				colvarTagName.DataType = DbType.AnsiString;
				colvarTagName.MaxLength = 256;
				colvarTagName.AutoIncrement = false;
				colvarTagName.IsNullable = false;
				colvarTagName.IsPrimaryKey = false;
				colvarTagName.IsForeignKey = false;
				colvarTagName.IsReadOnly = false;
				colvarTagName.DefaultSetting = @"";
				colvarTagName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarTagName);
				
				TableSchema.TableColumn colvarTagValue = new TableSchema.TableColumn(schema);
				colvarTagValue.ColumnName = "TagValue";
				colvarTagValue.DataType = DbType.AnsiString;
				colvarTagValue.MaxLength = 2000;
				colvarTagValue.AutoIncrement = false;
				colvarTagValue.IsNullable = false;
				colvarTagValue.IsPrimaryKey = false;
				colvarTagValue.IsForeignKey = false;
				colvarTagValue.IsReadOnly = false;
				colvarTagValue.DefaultSetting = @"";
				colvarTagValue.ForeignKeyTableName = "";
				schema.Columns.Add(colvarTagValue);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["WillCall"].AddSchema("MailerTemplateSubstitution",schema);
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
		  
		[XmlAttribute("TMailerTemplateContentId")]
		[Bindable(true)]
		public int TMailerTemplateContentId 
		{
			get { return GetColumnValue<int>(Columns.TMailerTemplateContentId); }
			set { SetColumnValue(Columns.TMailerTemplateContentId, value); }
		}
		  
		[XmlAttribute("TagName")]
		[Bindable(true)]
		public string TagName 
		{
			get { return GetColumnValue<string>(Columns.TagName); }
			set { SetColumnValue(Columns.TagName, value); }
		}
		  
		[XmlAttribute("TagValue")]
		[Bindable(true)]
		public string TagValue 
		{
			get { return GetColumnValue<string>(Columns.TagValue); }
			set { SetColumnValue(Columns.TagValue, value); }
		}
		
		#endregion
		
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a MailerTemplateContent ActiveRecord object related to this MailerTemplateSubstitution
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
		public static void Insert(DateTime varDtStamp,int varTMailerTemplateContentId,string varTagName,string varTagValue)
		{
			MailerTemplateSubstitution item = new MailerTemplateSubstitution();
			
			item.DtStamp = varDtStamp;
			
			item.TMailerTemplateContentId = varTMailerTemplateContentId;
			
			item.TagName = varTagName;
			
			item.TagValue = varTagValue;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varId,DateTime varDtStamp,int varTMailerTemplateContentId,string varTagName,string varTagValue)
		{
			MailerTemplateSubstitution item = new MailerTemplateSubstitution();
			
				item.Id = varId;
			
				item.DtStamp = varDtStamp;
			
				item.TMailerTemplateContentId = varTMailerTemplateContentId;
			
				item.TagName = varTagName;
			
				item.TagValue = varTagValue;
			
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
        
        
        
        public static TableSchema.TableColumn TMailerTemplateContentIdColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn TagNameColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn TagValueColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Id = @"Id";
			 public static string DtStamp = @"dtStamp";
			 public static string TMailerTemplateContentId = @"tMailerTemplateContentId";
			 public static string TagName = @"TagName";
			 public static string TagValue = @"TagValue";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
