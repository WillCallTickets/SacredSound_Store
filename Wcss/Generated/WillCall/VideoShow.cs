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
	/// Strongly-typed collection for the VideoShow class.
	/// </summary>
    [Serializable]
	public partial class VideoShowCollection : ActiveList<VideoShow, VideoShowCollection>
	{	   
		public VideoShowCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>VideoShowCollection</returns>
		public VideoShowCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                VideoShow o = this[i];
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
	/// This is an ActiveRecord class which wraps the VideoShow table.
	/// </summary>
	[Serializable]
	public partial class VideoShow : ActiveRecord<VideoShow>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public VideoShow()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public VideoShow(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public VideoShow(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public VideoShow(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("VideoShow", TableType.Table, DataService.GetInstance("WillCall"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarId = new TableSchema.TableColumn(schema);
				colvarId.ColumnName = "Id";
				colvarId.DataType = DbType.Int32;
				colvarId.MaxLength = 0;
				colvarId.AutoIncrement = false;
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
				
				TableSchema.TableColumn colvarTShowId = new TableSchema.TableColumn(schema);
				colvarTShowId.ColumnName = "tShowId";
				colvarTShowId.DataType = DbType.Int32;
				colvarTShowId.MaxLength = 0;
				colvarTShowId.AutoIncrement = false;
				colvarTShowId.IsNullable = false;
				colvarTShowId.IsPrimaryKey = false;
				colvarTShowId.IsForeignKey = true;
				colvarTShowId.IsReadOnly = false;
				colvarTShowId.DefaultSetting = @"";
				
					colvarTShowId.ForeignKeyTableName = "Show";
				schema.Columns.Add(colvarTShowId);
				
				TableSchema.TableColumn colvarTVideoId = new TableSchema.TableColumn(schema);
				colvarTVideoId.ColumnName = "tVideoId";
				colvarTVideoId.DataType = DbType.Int32;
				colvarTVideoId.MaxLength = 0;
				colvarTVideoId.AutoIncrement = false;
				colvarTVideoId.IsNullable = false;
				colvarTVideoId.IsPrimaryKey = false;
				colvarTVideoId.IsForeignKey = true;
				colvarTVideoId.IsReadOnly = false;
				colvarTVideoId.DefaultSetting = @"";
				
					colvarTVideoId.ForeignKeyTableName = "Video";
				schema.Columns.Add(colvarTVideoId);
				
				TableSchema.TableColumn colvarIDisplayOrder = new TableSchema.TableColumn(schema);
				colvarIDisplayOrder.ColumnName = "iDisplayOrder";
				colvarIDisplayOrder.DataType = DbType.Int32;
				colvarIDisplayOrder.MaxLength = 0;
				colvarIDisplayOrder.AutoIncrement = false;
				colvarIDisplayOrder.IsNullable = false;
				colvarIDisplayOrder.IsPrimaryKey = false;
				colvarIDisplayOrder.IsForeignKey = false;
				colvarIDisplayOrder.IsReadOnly = false;
				
						colvarIDisplayOrder.DefaultSetting = @"((-1))";
				colvarIDisplayOrder.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIDisplayOrder);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["WillCall"].AddSchema("VideoShow",schema);
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
		  
		[XmlAttribute("TShowId")]
		[Bindable(true)]
		public int TShowId 
		{
			get { return GetColumnValue<int>(Columns.TShowId); }
			set { SetColumnValue(Columns.TShowId, value); }
		}
		  
		[XmlAttribute("TVideoId")]
		[Bindable(true)]
		public int TVideoId 
		{
			get { return GetColumnValue<int>(Columns.TVideoId); }
			set { SetColumnValue(Columns.TVideoId, value); }
		}
		  
		[XmlAttribute("IDisplayOrder")]
		[Bindable(true)]
		public int IDisplayOrder 
		{
			get { return GetColumnValue<int>(Columns.IDisplayOrder); }
			set { SetColumnValue(Columns.IDisplayOrder, value); }
		}
		
		#endregion
		
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a Show ActiveRecord object related to this VideoShow
		/// 
		/// </summary>
		private Wcss.Show Show
		{
			get { return Wcss.Show.FetchByID(this.TShowId); }
			set { SetColumnValue("tShowId", value.Id); }
		}
        //set up an alternate mechanism to avoid a database call
		private Wcss.Show _showrecord = null;
		
		public Wcss.Show ShowRecord
		{
		    get
            {
                if (_showrecord == null)
                {
                    _showrecord = new Wcss.Show();
                    _showrecord.CopyFrom(this.Show);
                }
                return _showrecord;
            }
            set
            {
                if(value != null && _showrecord == null)
			        _showrecord = new Wcss.Show();
                
                SetColumnValue("tShowId", value.Id);
                _showrecord.CopyFrom(value);                
            }
		}
		
		
		/// <summary>
		/// Returns a Video ActiveRecord object related to this VideoShow
		/// 
		/// </summary>
		private Wcss.Video Video
		{
			get { return Wcss.Video.FetchByID(this.TVideoId); }
			set { SetColumnValue("tVideoId", value.Id); }
		}
        //set up an alternate mechanism to avoid a database call
		private Wcss.Video _videorecord = null;
		
		public Wcss.Video VideoRecord
		{
		    get
            {
                if (_videorecord == null)
                {
                    _videorecord = new Wcss.Video();
                    _videorecord.CopyFrom(this.Video);
                }
                return _videorecord;
            }
            set
            {
                if(value != null && _videorecord == null)
			        _videorecord = new Wcss.Video();
                
                SetColumnValue("tVideoId", value.Id);
                _videorecord.CopyFrom(value);                
            }
		}
		
		
		#endregion
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(int varId,DateTime varDtStamp,int varTShowId,int varTVideoId,int varIDisplayOrder)
		{
			VideoShow item = new VideoShow();
			
			item.Id = varId;
			
			item.DtStamp = varDtStamp;
			
			item.TShowId = varTShowId;
			
			item.TVideoId = varTVideoId;
			
			item.IDisplayOrder = varIDisplayOrder;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varId,DateTime varDtStamp,int varTShowId,int varTVideoId,int varIDisplayOrder)
		{
			VideoShow item = new VideoShow();
			
				item.Id = varId;
			
				item.DtStamp = varDtStamp;
			
				item.TShowId = varTShowId;
			
				item.TVideoId = varTVideoId;
			
				item.IDisplayOrder = varIDisplayOrder;
			
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
        
        
        
        public static TableSchema.TableColumn TShowIdColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn TVideoIdColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn IDisplayOrderColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Id = @"Id";
			 public static string DtStamp = @"dtStamp";
			 public static string TShowId = @"tShowId";
			 public static string TVideoId = @"tVideoId";
			 public static string IDisplayOrder = @"iDisplayOrder";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
