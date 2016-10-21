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
	/// Strongly-typed collection for the ShowTicketPackageLink class.
	/// </summary>
    [Serializable]
	public partial class ShowTicketPackageLinkCollection : ActiveList<ShowTicketPackageLink, ShowTicketPackageLinkCollection>
	{	   
		public ShowTicketPackageLinkCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>ShowTicketPackageLinkCollection</returns>
		public ShowTicketPackageLinkCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                ShowTicketPackageLink o = this[i];
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
	/// This is an ActiveRecord class which wraps the ShowTicketPackageLink table.
	/// </summary>
	[Serializable]
	public partial class ShowTicketPackageLink : ActiveRecord<ShowTicketPackageLink>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public ShowTicketPackageLink()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public ShowTicketPackageLink(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public ShowTicketPackageLink(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public ShowTicketPackageLink(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("ShowTicketPackageLink", TableType.Table, DataService.GetInstance("WillCall"));
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
				
				TableSchema.TableColumn colvarGroupIdentifier = new TableSchema.TableColumn(schema);
				colvarGroupIdentifier.ColumnName = "GroupIdentifier";
				colvarGroupIdentifier.DataType = DbType.Guid;
				colvarGroupIdentifier.MaxLength = 0;
				colvarGroupIdentifier.AutoIncrement = false;
				colvarGroupIdentifier.IsNullable = true;
				colvarGroupIdentifier.IsPrimaryKey = false;
				colvarGroupIdentifier.IsForeignKey = false;
				colvarGroupIdentifier.IsReadOnly = false;
				colvarGroupIdentifier.DefaultSetting = @"";
				colvarGroupIdentifier.ForeignKeyTableName = "";
				schema.Columns.Add(colvarGroupIdentifier);
				
				TableSchema.TableColumn colvarParentShowTicketId = new TableSchema.TableColumn(schema);
				colvarParentShowTicketId.ColumnName = "ParentShowTicketId";
				colvarParentShowTicketId.DataType = DbType.Int32;
				colvarParentShowTicketId.MaxLength = 0;
				colvarParentShowTicketId.AutoIncrement = false;
				colvarParentShowTicketId.IsNullable = false;
				colvarParentShowTicketId.IsPrimaryKey = false;
				colvarParentShowTicketId.IsForeignKey = true;
				colvarParentShowTicketId.IsReadOnly = false;
				colvarParentShowTicketId.DefaultSetting = @"";
				
					colvarParentShowTicketId.ForeignKeyTableName = "ShowTicket";
				schema.Columns.Add(colvarParentShowTicketId);
				
				TableSchema.TableColumn colvarLinkedShowTicketId = new TableSchema.TableColumn(schema);
				colvarLinkedShowTicketId.ColumnName = "LinkedShowTicketId";
				colvarLinkedShowTicketId.DataType = DbType.Int32;
				colvarLinkedShowTicketId.MaxLength = 0;
				colvarLinkedShowTicketId.AutoIncrement = false;
				colvarLinkedShowTicketId.IsNullable = false;
				colvarLinkedShowTicketId.IsPrimaryKey = false;
				colvarLinkedShowTicketId.IsForeignKey = true;
				colvarLinkedShowTicketId.IsReadOnly = false;
				colvarLinkedShowTicketId.DefaultSetting = @"";
				
					colvarLinkedShowTicketId.ForeignKeyTableName = "ShowTicket";
				schema.Columns.Add(colvarLinkedShowTicketId);
				
				TableSchema.TableColumn colvarDtStamp = new TableSchema.TableColumn(schema);
				colvarDtStamp.ColumnName = "dtStamp";
				colvarDtStamp.DataType = DbType.DateTime;
				colvarDtStamp.MaxLength = 0;
				colvarDtStamp.AutoIncrement = false;
				colvarDtStamp.IsNullable = true;
				colvarDtStamp.IsPrimaryKey = false;
				colvarDtStamp.IsForeignKey = false;
				colvarDtStamp.IsReadOnly = false;
				
						colvarDtStamp.DefaultSetting = @"(getdate())";
				colvarDtStamp.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDtStamp);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["WillCall"].AddSchema("ShowTicketPackageLink",schema);
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
		  
		[XmlAttribute("GroupIdentifier")]
		[Bindable(true)]
		public Guid? GroupIdentifier 
		{
			get { return GetColumnValue<Guid?>(Columns.GroupIdentifier); }
			set { SetColumnValue(Columns.GroupIdentifier, value); }
		}
		  
		[XmlAttribute("ParentShowTicketId")]
		[Bindable(true)]
		public int ParentShowTicketId 
		{
			get { return GetColumnValue<int>(Columns.ParentShowTicketId); }
			set { SetColumnValue(Columns.ParentShowTicketId, value); }
		}
		  
		[XmlAttribute("LinkedShowTicketId")]
		[Bindable(true)]
		public int LinkedShowTicketId 
		{
			get { return GetColumnValue<int>(Columns.LinkedShowTicketId); }
			set { SetColumnValue(Columns.LinkedShowTicketId, value); }
		}
		  
		[XmlAttribute("DtStamp")]
		[Bindable(true)]
		public DateTime? DtStamp 
		{
			get { return GetColumnValue<DateTime?>(Columns.DtStamp); }
			set { SetColumnValue(Columns.DtStamp, value); }
		}
		
		#endregion
		
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a ShowTicket ActiveRecord object related to this ShowTicketPackageLink
		/// 
		/// </summary>
		private Wcss.ShowTicket ShowTicket
		{
			get { return Wcss.ShowTicket.FetchByID(this.ParentShowTicketId); }
			set { SetColumnValue("ParentShowTicketId", value.Id); }
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
                
                SetColumnValue("ParentShowTicketId", value.Id);
                _showticketrecord.CopyFrom(value);                
            }
		}
		
		
		/// <summary>
		/// Returns a ShowTicket ActiveRecord object related to this ShowTicketPackageLink
		/// 
		/// </summary>
		private Wcss.ShowTicket ShowTicketToLinkedShowTicketId
		{
			get { return Wcss.ShowTicket.FetchByID(this.LinkedShowTicketId); }
			set { SetColumnValue("LinkedShowTicketId", value.Id); }
		}
        //set up an alternate mechanism to avoid a database call
		private Wcss.ShowTicket _showtickettolinkedshowticketidrecord = null;
		
		public Wcss.ShowTicket ShowTicketToLinkedShowTicketIdRecord
		{
		    get
            {
                if (_showtickettolinkedshowticketidrecord == null)
                {
                    _showtickettolinkedshowticketidrecord = new Wcss.ShowTicket();
                    _showtickettolinkedshowticketidrecord.CopyFrom(this.ShowTicketToLinkedShowTicketId);
                }
                return _showtickettolinkedshowticketidrecord;
            }
            set
            {
                if(value != null && _showtickettolinkedshowticketidrecord == null)
			        _showtickettolinkedshowticketidrecord = new Wcss.ShowTicket();
                
                SetColumnValue("LinkedShowTicketId", value.Id);
                _showtickettolinkedshowticketidrecord.CopyFrom(value);                
            }
		}
		
		
		#endregion
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(Guid? varGroupIdentifier,int varParentShowTicketId,int varLinkedShowTicketId,DateTime? varDtStamp)
		{
			ShowTicketPackageLink item = new ShowTicketPackageLink();
			
			item.GroupIdentifier = varGroupIdentifier;
			
			item.ParentShowTicketId = varParentShowTicketId;
			
			item.LinkedShowTicketId = varLinkedShowTicketId;
			
			item.DtStamp = varDtStamp;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varId,Guid? varGroupIdentifier,int varParentShowTicketId,int varLinkedShowTicketId,DateTime? varDtStamp)
		{
			ShowTicketPackageLink item = new ShowTicketPackageLink();
			
				item.Id = varId;
			
				item.GroupIdentifier = varGroupIdentifier;
			
				item.ParentShowTicketId = varParentShowTicketId;
			
				item.LinkedShowTicketId = varLinkedShowTicketId;
			
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
        
        
        
        public static TableSchema.TableColumn GroupIdentifierColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn ParentShowTicketIdColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn LinkedShowTicketIdColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn DtStampColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Id = @"Id";
			 public static string GroupIdentifier = @"GroupIdentifier";
			 public static string ParentShowTicketId = @"ParentShowTicketId";
			 public static string LinkedShowTicketId = @"LinkedShowTicketId";
			 public static string DtStamp = @"dtStamp";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
