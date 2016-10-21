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
	/// Strongly-typed collection for the Search class.
	/// </summary>
    [Serializable]
	public partial class SearchCollection : ActiveList<Search, SearchCollection>
	{	   
		public SearchCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>SearchCollection</returns>
		public SearchCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                Search o = this[i];
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
	/// This is an ActiveRecord class which wraps the Search table.
	/// </summary>
	[Serializable]
	public partial class Search : ActiveRecord<Search>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public Search()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public Search(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public Search(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public Search(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("Search", TableType.Table, DataService.GetInstance("WillCall"));
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
				colvarApplicationId.IsForeignKey = true;
				colvarApplicationId.IsReadOnly = false;
				colvarApplicationId.DefaultSetting = @"";
				
					colvarApplicationId.ForeignKeyTableName = "aspnet_Applications";
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
				
				TableSchema.TableColumn colvarVcContext = new TableSchema.TableColumn(schema);
				colvarVcContext.ColumnName = "vcContext";
				colvarVcContext.DataType = DbType.AnsiString;
				colvarVcContext.MaxLength = 50;
				colvarVcContext.AutoIncrement = false;
				colvarVcContext.IsNullable = false;
				colvarVcContext.IsPrimaryKey = false;
				colvarVcContext.IsForeignKey = false;
				colvarVcContext.IsReadOnly = false;
				colvarVcContext.DefaultSetting = @"";
				colvarVcContext.ForeignKeyTableName = "";
				schema.Columns.Add(colvarVcContext);
				
				TableSchema.TableColumn colvarTerms = new TableSchema.TableColumn(schema);
				colvarTerms.ColumnName = "Terms";
				colvarTerms.DataType = DbType.String;
				colvarTerms.MaxLength = 256;
				colvarTerms.AutoIncrement = false;
				colvarTerms.IsNullable = false;
				colvarTerms.IsPrimaryKey = false;
				colvarTerms.IsForeignKey = false;
				colvarTerms.IsReadOnly = false;
				colvarTerms.DefaultSetting = @"";
				colvarTerms.ForeignKeyTableName = "";
				schema.Columns.Add(colvarTerms);
				
				TableSchema.TableColumn colvarIResults = new TableSchema.TableColumn(schema);
				colvarIResults.ColumnName = "iResults";
				colvarIResults.DataType = DbType.Int32;
				colvarIResults.MaxLength = 0;
				colvarIResults.AutoIncrement = false;
				colvarIResults.IsNullable = false;
				colvarIResults.IsPrimaryKey = false;
				colvarIResults.IsForeignKey = false;
				colvarIResults.IsReadOnly = false;
				
						colvarIResults.DefaultSetting = @"((0))";
				colvarIResults.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIResults);
				
				TableSchema.TableColumn colvarEmailAddress = new TableSchema.TableColumn(schema);
				colvarEmailAddress.ColumnName = "EmailAddress";
				colvarEmailAddress.DataType = DbType.AnsiString;
				colvarEmailAddress.MaxLength = 256;
				colvarEmailAddress.AutoIncrement = false;
				colvarEmailAddress.IsNullable = false;
				colvarEmailAddress.IsPrimaryKey = false;
				colvarEmailAddress.IsForeignKey = false;
				colvarEmailAddress.IsReadOnly = false;
				colvarEmailAddress.DefaultSetting = @"";
				colvarEmailAddress.ForeignKeyTableName = "";
				schema.Columns.Add(colvarEmailAddress);
				
				TableSchema.TableColumn colvarIpAddress = new TableSchema.TableColumn(schema);
				colvarIpAddress.ColumnName = "IpAddress";
				colvarIpAddress.DataType = DbType.AnsiString;
				colvarIpAddress.MaxLength = 25;
				colvarIpAddress.AutoIncrement = false;
				colvarIpAddress.IsNullable = false;
				colvarIpAddress.IsPrimaryKey = false;
				colvarIpAddress.IsForeignKey = false;
				colvarIpAddress.IsReadOnly = false;
				colvarIpAddress.DefaultSetting = @"";
				colvarIpAddress.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIpAddress);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["WillCall"].AddSchema("Search",schema);
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
		  
		[XmlAttribute("VcContext")]
		[Bindable(true)]
		public string VcContext 
		{
			get { return GetColumnValue<string>(Columns.VcContext); }
			set { SetColumnValue(Columns.VcContext, value); }
		}
		  
		[XmlAttribute("Terms")]
		[Bindable(true)]
		public string Terms 
		{
			get { return GetColumnValue<string>(Columns.Terms); }
			set { SetColumnValue(Columns.Terms, value); }
		}
		  
		[XmlAttribute("IResults")]
		[Bindable(true)]
		public int IResults 
		{
			get { return GetColumnValue<int>(Columns.IResults); }
			set { SetColumnValue(Columns.IResults, value); }
		}
		  
		[XmlAttribute("EmailAddress")]
		[Bindable(true)]
		public string EmailAddress 
		{
			get { return GetColumnValue<string>(Columns.EmailAddress); }
			set { SetColumnValue(Columns.EmailAddress, value); }
		}
		  
		[XmlAttribute("IpAddress")]
		[Bindable(true)]
		public string IpAddress 
		{
			get { return GetColumnValue<string>(Columns.IpAddress); }
			set { SetColumnValue(Columns.IpAddress, value); }
		}
		
		#endregion
		
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a AspnetApplication ActiveRecord object related to this Search
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
		public static void Insert(Guid varApplicationId,DateTime varDtStamp,string varVcContext,string varTerms,int varIResults,string varEmailAddress,string varIpAddress)
		{
			Search item = new Search();
			
			item.ApplicationId = varApplicationId;
			
			item.DtStamp = varDtStamp;
			
			item.VcContext = varVcContext;
			
			item.Terms = varTerms;
			
			item.IResults = varIResults;
			
			item.EmailAddress = varEmailAddress;
			
			item.IpAddress = varIpAddress;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varId,Guid varApplicationId,DateTime varDtStamp,string varVcContext,string varTerms,int varIResults,string varEmailAddress,string varIpAddress)
		{
			Search item = new Search();
			
				item.Id = varId;
			
				item.ApplicationId = varApplicationId;
			
				item.DtStamp = varDtStamp;
			
				item.VcContext = varVcContext;
			
				item.Terms = varTerms;
			
				item.IResults = varIResults;
			
				item.EmailAddress = varEmailAddress;
			
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
        
        
        
        public static TableSchema.TableColumn VcContextColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn TermsColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn IResultsColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn EmailAddressColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn IpAddressColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Id = @"Id";
			 public static string ApplicationId = @"ApplicationId";
			 public static string DtStamp = @"dtStamp";
			 public static string VcContext = @"vcContext";
			 public static string Terms = @"Terms";
			 public static string IResults = @"iResults";
			 public static string EmailAddress = @"EmailAddress";
			 public static string IpAddress = @"IpAddress";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
