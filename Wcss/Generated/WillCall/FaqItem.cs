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
	/// Strongly-typed collection for the FaqItem class.
	/// </summary>
    [Serializable]
	public partial class FaqItemCollection : ActiveList<FaqItem, FaqItemCollection>
	{	   
		public FaqItemCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>FaqItemCollection</returns>
		public FaqItemCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                FaqItem o = this[i];
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
	/// This is an ActiveRecord class which wraps the FaqItem table.
	/// </summary>
	[Serializable]
	public partial class FaqItem : ActiveRecord<FaqItem>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public FaqItem()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public FaqItem(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public FaqItem(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public FaqItem(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("FaqItem", TableType.Table, DataService.GetInstance("WillCall"));
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
				
				TableSchema.TableColumn colvarQuestion = new TableSchema.TableColumn(schema);
				colvarQuestion.ColumnName = "Question";
				colvarQuestion.DataType = DbType.AnsiString;
				colvarQuestion.MaxLength = 896;
				colvarQuestion.AutoIncrement = false;
				colvarQuestion.IsNullable = false;
				colvarQuestion.IsPrimaryKey = false;
				colvarQuestion.IsForeignKey = false;
				colvarQuestion.IsReadOnly = false;
				colvarQuestion.DefaultSetting = @"";
				colvarQuestion.ForeignKeyTableName = "";
				schema.Columns.Add(colvarQuestion);
				
				TableSchema.TableColumn colvarAnswer = new TableSchema.TableColumn(schema);
				colvarAnswer.ColumnName = "Answer";
				colvarAnswer.DataType = DbType.AnsiString;
				colvarAnswer.MaxLength = -1;
				colvarAnswer.AutoIncrement = false;
				colvarAnswer.IsNullable = true;
				colvarAnswer.IsPrimaryKey = false;
				colvarAnswer.IsForeignKey = false;
				colvarAnswer.IsReadOnly = false;
				colvarAnswer.DefaultSetting = @"";
				colvarAnswer.ForeignKeyTableName = "";
				schema.Columns.Add(colvarAnswer);
				
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
				
				TableSchema.TableColumn colvarTFaqCategorieId = new TableSchema.TableColumn(schema);
				colvarTFaqCategorieId.ColumnName = "tFaqCategorieId";
				colvarTFaqCategorieId.DataType = DbType.Int32;
				colvarTFaqCategorieId.MaxLength = 0;
				colvarTFaqCategorieId.AutoIncrement = false;
				colvarTFaqCategorieId.IsNullable = false;
				colvarTFaqCategorieId.IsPrimaryKey = false;
				colvarTFaqCategorieId.IsForeignKey = true;
				colvarTFaqCategorieId.IsReadOnly = false;
				colvarTFaqCategorieId.DefaultSetting = @"";
				
					colvarTFaqCategorieId.ForeignKeyTableName = "FaqCategorie";
				schema.Columns.Add(colvarTFaqCategorieId);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["WillCall"].AddSchema("FaqItem",schema);
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
		  
		[XmlAttribute("BActive")]
		[Bindable(true)]
		public bool BActive 
		{
			get { return GetColumnValue<bool>(Columns.BActive); }
			set { SetColumnValue(Columns.BActive, value); }
		}
		  
		[XmlAttribute("Question")]
		[Bindable(true)]
		public string Question 
		{
			get { return GetColumnValue<string>(Columns.Question); }
			set { SetColumnValue(Columns.Question, value); }
		}
		  
		[XmlAttribute("Answer")]
		[Bindable(true)]
		public string Answer 
		{
			get { return GetColumnValue<string>(Columns.Answer); }
			set { SetColumnValue(Columns.Answer, value); }
		}
		  
		[XmlAttribute("IDisplayOrder")]
		[Bindable(true)]
		public int IDisplayOrder 
		{
			get { return GetColumnValue<int>(Columns.IDisplayOrder); }
			set { SetColumnValue(Columns.IDisplayOrder, value); }
		}
		  
		[XmlAttribute("TFaqCategorieId")]
		[Bindable(true)]
		public int TFaqCategorieId 
		{
			get { return GetColumnValue<int>(Columns.TFaqCategorieId); }
			set { SetColumnValue(Columns.TFaqCategorieId, value); }
		}
		
		#endregion
		
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a FaqCategorie ActiveRecord object related to this FaqItem
		/// 
		/// </summary>
		private Wcss.FaqCategorie FaqCategorie
		{
			get { return Wcss.FaqCategorie.FetchByID(this.TFaqCategorieId); }
			set { SetColumnValue("tFaqCategorieId", value.Id); }
		}
        //set up an alternate mechanism to avoid a database call
		private Wcss.FaqCategorie _faqcategorierecord = null;
		
		public Wcss.FaqCategorie FaqCategorieRecord
		{
		    get
            {
                if (_faqcategorierecord == null)
                {
                    _faqcategorierecord = new Wcss.FaqCategorie();
                    _faqcategorierecord.CopyFrom(this.FaqCategorie);
                }
                return _faqcategorierecord;
            }
            set
            {
                if(value != null && _faqcategorierecord == null)
			        _faqcategorierecord = new Wcss.FaqCategorie();
                
                SetColumnValue("tFaqCategorieId", value.Id);
                _faqcategorierecord.CopyFrom(value);                
            }
		}
		
		
		#endregion
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(DateTime varDtStamp,bool varBActive,string varQuestion,string varAnswer,int varIDisplayOrder,int varTFaqCategorieId)
		{
			FaqItem item = new FaqItem();
			
			item.DtStamp = varDtStamp;
			
			item.BActive = varBActive;
			
			item.Question = varQuestion;
			
			item.Answer = varAnswer;
			
			item.IDisplayOrder = varIDisplayOrder;
			
			item.TFaqCategorieId = varTFaqCategorieId;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varId,DateTime varDtStamp,bool varBActive,string varQuestion,string varAnswer,int varIDisplayOrder,int varTFaqCategorieId)
		{
			FaqItem item = new FaqItem();
			
				item.Id = varId;
			
				item.DtStamp = varDtStamp;
			
				item.BActive = varBActive;
			
				item.Question = varQuestion;
			
				item.Answer = varAnswer;
			
				item.IDisplayOrder = varIDisplayOrder;
			
				item.TFaqCategorieId = varTFaqCategorieId;
			
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
        
        
        
        public static TableSchema.TableColumn BActiveColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn QuestionColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn AnswerColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn IDisplayOrderColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn TFaqCategorieIdColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Id = @"Id";
			 public static string DtStamp = @"dtStamp";
			 public static string BActive = @"bActive";
			 public static string Question = @"Question";
			 public static string Answer = @"Answer";
			 public static string IDisplayOrder = @"iDisplayOrder";
			 public static string TFaqCategorieId = @"tFaqCategorieId";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
