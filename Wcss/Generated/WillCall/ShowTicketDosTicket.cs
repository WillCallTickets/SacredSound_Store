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
	/// Strongly-typed collection for the ShowTicketDosTicket class.
	/// </summary>
    [Serializable]
	public partial class ShowTicketDosTicketCollection : ActiveList<ShowTicketDosTicket, ShowTicketDosTicketCollection>
	{	   
		public ShowTicketDosTicketCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>ShowTicketDosTicketCollection</returns>
		public ShowTicketDosTicketCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                ShowTicketDosTicket o = this[i];
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
	/// This is an ActiveRecord class which wraps the ShowTicketDosTicket table.
	/// </summary>
	[Serializable]
	public partial class ShowTicketDosTicket : ActiveRecord<ShowTicketDosTicket>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public ShowTicketDosTicket()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public ShowTicketDosTicket(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public ShowTicketDosTicket(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public ShowTicketDosTicket(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("ShowTicketDosTicket", TableType.Table, DataService.GetInstance("WillCall"));
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
				
				TableSchema.TableColumn colvarParentId = new TableSchema.TableColumn(schema);
				colvarParentId.ColumnName = "ParentId";
				colvarParentId.DataType = DbType.Int32;
				colvarParentId.MaxLength = 0;
				colvarParentId.AutoIncrement = false;
				colvarParentId.IsNullable = false;
				colvarParentId.IsPrimaryKey = false;
				colvarParentId.IsForeignKey = true;
				colvarParentId.IsReadOnly = false;
				colvarParentId.DefaultSetting = @"";
				
					colvarParentId.ForeignKeyTableName = "ShowTicket";
				schema.Columns.Add(colvarParentId);
				
				TableSchema.TableColumn colvarDosId = new TableSchema.TableColumn(schema);
				colvarDosId.ColumnName = "DosId";
				colvarDosId.DataType = DbType.Int32;
				colvarDosId.MaxLength = 0;
				colvarDosId.AutoIncrement = false;
				colvarDosId.IsNullable = false;
				colvarDosId.IsPrimaryKey = false;
				colvarDosId.IsForeignKey = true;
				colvarDosId.IsReadOnly = false;
				colvarDosId.DefaultSetting = @"";
				
					colvarDosId.ForeignKeyTableName = "ShowTicket";
				schema.Columns.Add(colvarDosId);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["WillCall"].AddSchema("ShowTicketDosTicket",schema);
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
		  
		[XmlAttribute("ParentId")]
		[Bindable(true)]
		public int ParentId 
		{
			get { return GetColumnValue<int>(Columns.ParentId); }
			set { SetColumnValue(Columns.ParentId, value); }
		}
		  
		[XmlAttribute("DosId")]
		[Bindable(true)]
		public int DosId 
		{
			get { return GetColumnValue<int>(Columns.DosId); }
			set { SetColumnValue(Columns.DosId, value); }
		}
		
		#endregion
		
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a ShowTicket ActiveRecord object related to this ShowTicketDosTicket
		/// 
		/// </summary>
		private Wcss.ShowTicket ShowTicket
		{
			get { return Wcss.ShowTicket.FetchByID(this.ParentId); }
			set { SetColumnValue("ParentId", value.Id); }
		}
        //set up an alternate mechanism to avoid a database call
		private Wcss.ShowTicket _showticketrecord = null;
		
		public Wcss.ShowTicket ShowTicketRecord
		{
		    get
            {
                if (_showticketrecord == null)
                {
                    _showticketrecord = new Wcss.ShowTicket();
                    _showticketrecord.CopyFrom(this.ShowTicket);
                }
                return _showticketrecord;
            }
            set
            {
                if(value != null && _showticketrecord == null)
			        _showticketrecord = new Wcss.ShowTicket();
                
                SetColumnValue("ParentId", value.Id);
                _showticketrecord.CopyFrom(value);                
            }
		}
		
		
		/// <summary>
		/// Returns a ShowTicket ActiveRecord object related to this ShowTicketDosTicket
		/// 
		/// </summary>
		private Wcss.ShowTicket ShowTicketToDosId
		{
			get { return Wcss.ShowTicket.FetchByID(this.DosId); }
			set { SetColumnValue("DosId", value.Id); }
		}
        //set up an alternate mechanism to avoid a database call
		private Wcss.ShowTicket _showtickettodosidrecord = null;
		
		public Wcss.ShowTicket ShowTicketToDosIdRecord
		{
		    get
            {
                if (_showtickettodosidrecord == null)
                {
                    _showtickettodosidrecord = new Wcss.ShowTicket();
                    _showtickettodosidrecord.CopyFrom(this.ShowTicketToDosId);
                }
                return _showtickettodosidrecord;
            }
            set
            {
                if(value != null && _showtickettodosidrecord == null)
			        _showtickettodosidrecord = new Wcss.ShowTicket();
                
                SetColumnValue("DosId", value.Id);
                _showtickettodosidrecord.CopyFrom(value);                
            }
		}
		
		
		#endregion
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(DateTime varDtStamp,int varParentId,int varDosId)
		{
			ShowTicketDosTicket item = new ShowTicketDosTicket();
			
			item.DtStamp = varDtStamp;
			
			item.ParentId = varParentId;
			
			item.DosId = varDosId;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varId,DateTime varDtStamp,int varParentId,int varDosId)
		{
			ShowTicketDosTicket item = new ShowTicketDosTicket();
			
				item.Id = varId;
			
				item.DtStamp = varDtStamp;
			
				item.ParentId = varParentId;
			
				item.DosId = varDosId;
			
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
        
        
        
        public static TableSchema.TableColumn ParentIdColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn DosIdColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Id = @"Id";
			 public static string DtStamp = @"dtStamp";
			 public static string ParentId = @"ParentId";
			 public static string DosId = @"DosId";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
