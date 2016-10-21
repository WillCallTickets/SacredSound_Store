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
	/// Strongly-typed collection for the ItemImage class.
	/// </summary>
    [Serializable]
	public partial class ItemImageCollection : ActiveList<ItemImage, ItemImageCollection>
	{	   
		public ItemImageCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>ItemImageCollection</returns>
		public ItemImageCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                ItemImage o = this[i];
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
	/// This is an ActiveRecord class which wraps the ItemImage table.
	/// </summary>
	[Serializable]
	public partial class ItemImage : ActiveRecord<ItemImage>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public ItemImage()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public ItemImage(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public ItemImage(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public ItemImage(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("ItemImage", TableType.Table, DataService.GetInstance("WillCall"));
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
				
				TableSchema.TableColumn colvarTMerchId = new TableSchema.TableColumn(schema);
				colvarTMerchId.ColumnName = "TMerchId";
				colvarTMerchId.DataType = DbType.Int32;
				colvarTMerchId.MaxLength = 0;
				colvarTMerchId.AutoIncrement = false;
				colvarTMerchId.IsNullable = true;
				colvarTMerchId.IsPrimaryKey = false;
				colvarTMerchId.IsForeignKey = true;
				colvarTMerchId.IsReadOnly = false;
				colvarTMerchId.DefaultSetting = @"";
				
					colvarTMerchId.ForeignKeyTableName = "Merch";
				schema.Columns.Add(colvarTMerchId);
				
				TableSchema.TableColumn colvarTFutureId = new TableSchema.TableColumn(schema);
				colvarTFutureId.ColumnName = "TFutureId";
				colvarTFutureId.DataType = DbType.Int32;
				colvarTFutureId.MaxLength = 0;
				colvarTFutureId.AutoIncrement = false;
				colvarTFutureId.IsNullable = true;
				colvarTFutureId.IsPrimaryKey = false;
				colvarTFutureId.IsForeignKey = false;
				colvarTFutureId.IsReadOnly = false;
				colvarTFutureId.DefaultSetting = @"";
				colvarTFutureId.ForeignKeyTableName = "";
				schema.Columns.Add(colvarTFutureId);
				
				TableSchema.TableColumn colvarBItemImage = new TableSchema.TableColumn(schema);
				colvarBItemImage.ColumnName = "bItemImage";
				colvarBItemImage.DataType = DbType.Boolean;
				colvarBItemImage.MaxLength = 0;
				colvarBItemImage.AutoIncrement = false;
				colvarBItemImage.IsNullable = true;
				colvarBItemImage.IsPrimaryKey = false;
				colvarBItemImage.IsForeignKey = false;
				colvarBItemImage.IsReadOnly = false;
				
						colvarBItemImage.DefaultSetting = @"((1))";
				colvarBItemImage.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBItemImage);
				
				TableSchema.TableColumn colvarBDetailImage = new TableSchema.TableColumn(schema);
				colvarBDetailImage.ColumnName = "bDetailImage";
				colvarBDetailImage.DataType = DbType.Boolean;
				colvarBDetailImage.MaxLength = 0;
				colvarBDetailImage.AutoIncrement = false;
				colvarBDetailImage.IsNullable = true;
				colvarBDetailImage.IsPrimaryKey = false;
				colvarBDetailImage.IsForeignKey = false;
				colvarBDetailImage.IsReadOnly = false;
				
						colvarBDetailImage.DefaultSetting = @"((0))";
				colvarBDetailImage.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBDetailImage);
				
				TableSchema.TableColumn colvarBOverrideThumbnail = new TableSchema.TableColumn(schema);
				colvarBOverrideThumbnail.ColumnName = "bOverrideThumbnail";
				colvarBOverrideThumbnail.DataType = DbType.Boolean;
				colvarBOverrideThumbnail.MaxLength = 0;
				colvarBOverrideThumbnail.AutoIncrement = false;
				colvarBOverrideThumbnail.IsNullable = true;
				colvarBOverrideThumbnail.IsPrimaryKey = false;
				colvarBOverrideThumbnail.IsForeignKey = false;
				colvarBOverrideThumbnail.IsReadOnly = false;
				
						colvarBOverrideThumbnail.DefaultSetting = @"((0))";
				colvarBOverrideThumbnail.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBOverrideThumbnail);
				
				TableSchema.TableColumn colvarDetailDescription = new TableSchema.TableColumn(schema);
				colvarDetailDescription.ColumnName = "DetailDescription";
				colvarDetailDescription.DataType = DbType.AnsiString;
				colvarDetailDescription.MaxLength = 2000;
				colvarDetailDescription.AutoIncrement = false;
				colvarDetailDescription.IsNullable = true;
				colvarDetailDescription.IsPrimaryKey = false;
				colvarDetailDescription.IsForeignKey = false;
				colvarDetailDescription.IsReadOnly = false;
				colvarDetailDescription.DefaultSetting = @"";
				colvarDetailDescription.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDetailDescription);
				
				TableSchema.TableColumn colvarStorageRemote = new TableSchema.TableColumn(schema);
				colvarStorageRemote.ColumnName = "StorageRemote";
				colvarStorageRemote.DataType = DbType.AnsiString;
				colvarStorageRemote.MaxLength = 256;
				colvarStorageRemote.AutoIncrement = false;
				colvarStorageRemote.IsNullable = true;
				colvarStorageRemote.IsPrimaryKey = false;
				colvarStorageRemote.IsForeignKey = false;
				colvarStorageRemote.IsReadOnly = false;
				colvarStorageRemote.DefaultSetting = @"";
				colvarStorageRemote.ForeignKeyTableName = "";
				schema.Columns.Add(colvarStorageRemote);
				
				TableSchema.TableColumn colvarPath = new TableSchema.TableColumn(schema);
				colvarPath.ColumnName = "Path";
				colvarPath.DataType = DbType.AnsiString;
				colvarPath.MaxLength = 256;
				colvarPath.AutoIncrement = false;
				colvarPath.IsNullable = false;
				colvarPath.IsPrimaryKey = false;
				colvarPath.IsForeignKey = false;
				colvarPath.IsReadOnly = false;
				colvarPath.DefaultSetting = @"";
				colvarPath.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPath);
				
				TableSchema.TableColumn colvarImageName = new TableSchema.TableColumn(schema);
				colvarImageName.ColumnName = "ImageName";
				colvarImageName.DataType = DbType.AnsiString;
				colvarImageName.MaxLength = 256;
				colvarImageName.AutoIncrement = false;
				colvarImageName.IsNullable = false;
				colvarImageName.IsPrimaryKey = false;
				colvarImageName.IsForeignKey = false;
				colvarImageName.IsReadOnly = false;
				colvarImageName.DefaultSetting = @"";
				colvarImageName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarImageName);
				
				TableSchema.TableColumn colvarImageHeight = new TableSchema.TableColumn(schema);
				colvarImageHeight.ColumnName = "ImageHeight";
				colvarImageHeight.DataType = DbType.Int32;
				colvarImageHeight.MaxLength = 0;
				colvarImageHeight.AutoIncrement = false;
				colvarImageHeight.IsNullable = false;
				colvarImageHeight.IsPrimaryKey = false;
				colvarImageHeight.IsForeignKey = false;
				colvarImageHeight.IsReadOnly = false;
				colvarImageHeight.DefaultSetting = @"";
				colvarImageHeight.ForeignKeyTableName = "";
				schema.Columns.Add(colvarImageHeight);
				
				TableSchema.TableColumn colvarImageWidth = new TableSchema.TableColumn(schema);
				colvarImageWidth.ColumnName = "ImageWidth";
				colvarImageWidth.DataType = DbType.Int32;
				colvarImageWidth.MaxLength = 0;
				colvarImageWidth.AutoIncrement = false;
				colvarImageWidth.IsNullable = false;
				colvarImageWidth.IsPrimaryKey = false;
				colvarImageWidth.IsForeignKey = false;
				colvarImageWidth.IsReadOnly = false;
				colvarImageWidth.DefaultSetting = @"";
				colvarImageWidth.ForeignKeyTableName = "";
				schema.Columns.Add(colvarImageWidth);
				
				TableSchema.TableColumn colvarThumbClass = new TableSchema.TableColumn(schema);
				colvarThumbClass.ColumnName = "ThumbClass";
				colvarThumbClass.DataType = DbType.AnsiString;
				colvarThumbClass.MaxLength = 256;
				colvarThumbClass.AutoIncrement = false;
				colvarThumbClass.IsNullable = false;
				colvarThumbClass.IsPrimaryKey = false;
				colvarThumbClass.IsForeignKey = false;
				colvarThumbClass.IsReadOnly = false;
				
						colvarThumbClass.DefaultSetting = @"('')";
				colvarThumbClass.ForeignKeyTableName = "";
				schema.Columns.Add(colvarThumbClass);
				
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
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["WillCall"].AddSchema("ItemImage",schema);
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
		  
		[XmlAttribute("TMerchId")]
		[Bindable(true)]
		public int? TMerchId 
		{
			get { return GetColumnValue<int?>(Columns.TMerchId); }
			set { SetColumnValue(Columns.TMerchId, value); }
		}
		  
		[XmlAttribute("TFutureId")]
		[Bindable(true)]
		public int? TFutureId 
		{
			get { return GetColumnValue<int?>(Columns.TFutureId); }
			set { SetColumnValue(Columns.TFutureId, value); }
		}
		  
		[XmlAttribute("BItemImage")]
		[Bindable(true)]
		public bool? BItemImage 
		{
			get { return GetColumnValue<bool?>(Columns.BItemImage); }
			set { SetColumnValue(Columns.BItemImage, value); }
		}
		  
		[XmlAttribute("BDetailImage")]
		[Bindable(true)]
		public bool? BDetailImage 
		{
			get { return GetColumnValue<bool?>(Columns.BDetailImage); }
			set { SetColumnValue(Columns.BDetailImage, value); }
		}
		  
		[XmlAttribute("BOverrideThumbnail")]
		[Bindable(true)]
		public bool? BOverrideThumbnail 
		{
			get { return GetColumnValue<bool?>(Columns.BOverrideThumbnail); }
			set { SetColumnValue(Columns.BOverrideThumbnail, value); }
		}
		  
		[XmlAttribute("DetailDescription")]
		[Bindable(true)]
		public string DetailDescription 
		{
			get { return GetColumnValue<string>(Columns.DetailDescription); }
			set { SetColumnValue(Columns.DetailDescription, value); }
		}
		  
		[XmlAttribute("StorageRemote")]
		[Bindable(true)]
		public string StorageRemote 
		{
			get { return GetColumnValue<string>(Columns.StorageRemote); }
			set { SetColumnValue(Columns.StorageRemote, value); }
		}
		  
		[XmlAttribute("Path")]
		[Bindable(true)]
		public string Path 
		{
			get { return GetColumnValue<string>(Columns.Path); }
			set { SetColumnValue(Columns.Path, value); }
		}
		  
		[XmlAttribute("ImageName")]
		[Bindable(true)]
		public string ImageName 
		{
			get { return GetColumnValue<string>(Columns.ImageName); }
			set { SetColumnValue(Columns.ImageName, value); }
		}
		  
		[XmlAttribute("ImageHeight")]
		[Bindable(true)]
		public int ImageHeight 
		{
			get { return GetColumnValue<int>(Columns.ImageHeight); }
			set { SetColumnValue(Columns.ImageHeight, value); }
		}
		  
		[XmlAttribute("ImageWidth")]
		[Bindable(true)]
		public int ImageWidth 
		{
			get { return GetColumnValue<int>(Columns.ImageWidth); }
			set { SetColumnValue(Columns.ImageWidth, value); }
		}
		  
		[XmlAttribute("ThumbClass")]
		[Bindable(true)]
		public string ThumbClass 
		{
			get { return GetColumnValue<string>(Columns.ThumbClass); }
			set { SetColumnValue(Columns.ThumbClass, value); }
		}
		  
		[XmlAttribute("IDisplayOrder")]
		[Bindable(true)]
		public int IDisplayOrder 
		{
			get { return GetColumnValue<int>(Columns.IDisplayOrder); }
			set { SetColumnValue(Columns.IDisplayOrder, value); }
		}
		  
		[XmlAttribute("DtStamp")]
		[Bindable(true)]
		public DateTime DtStamp 
		{
			get { return GetColumnValue<DateTime>(Columns.DtStamp); }
			set { SetColumnValue(Columns.DtStamp, value); }
		}
		
		#endregion
		
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a Merch ActiveRecord object related to this ItemImage
		/// 
		/// </summary>
		private Wcss.Merch Merch
		{
			get { return Wcss.Merch.FetchByID(this.TMerchId); }
			set { SetColumnValue("TMerchId", value.Id); }
		}
        //set up an alternate mechanism to avoid a database call
		private Wcss.Merch _merchrecord = null;
		
		public Wcss.Merch MerchRecord
		{
		    get
            {
                if (_merchrecord == null)
                {
                    _merchrecord = new Wcss.Merch();
                    _merchrecord.CopyFrom(this.Merch);
                }
                return _merchrecord;
            }
            set
            {
                if(value != null && _merchrecord == null)
			        _merchrecord = new Wcss.Merch();
                
                SetColumnValue("TMerchId", value.Id);
                _merchrecord.CopyFrom(value);                
            }
		}
		
		
		#endregion
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(int? varTMerchId,int? varTFutureId,bool? varBItemImage,bool? varBDetailImage,bool? varBOverrideThumbnail,string varDetailDescription,string varStorageRemote,string varPath,string varImageName,int varImageHeight,int varImageWidth,string varThumbClass,int varIDisplayOrder,DateTime varDtStamp)
		{
			ItemImage item = new ItemImage();
			
			item.TMerchId = varTMerchId;
			
			item.TFutureId = varTFutureId;
			
			item.BItemImage = varBItemImage;
			
			item.BDetailImage = varBDetailImage;
			
			item.BOverrideThumbnail = varBOverrideThumbnail;
			
			item.DetailDescription = varDetailDescription;
			
			item.StorageRemote = varStorageRemote;
			
			item.Path = varPath;
			
			item.ImageName = varImageName;
			
			item.ImageHeight = varImageHeight;
			
			item.ImageWidth = varImageWidth;
			
			item.ThumbClass = varThumbClass;
			
			item.IDisplayOrder = varIDisplayOrder;
			
			item.DtStamp = varDtStamp;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varId,int? varTMerchId,int? varTFutureId,bool? varBItemImage,bool? varBDetailImage,bool? varBOverrideThumbnail,string varDetailDescription,string varStorageRemote,string varPath,string varImageName,int varImageHeight,int varImageWidth,string varThumbClass,int varIDisplayOrder,DateTime varDtStamp)
		{
			ItemImage item = new ItemImage();
			
				item.Id = varId;
			
				item.TMerchId = varTMerchId;
			
				item.TFutureId = varTFutureId;
			
				item.BItemImage = varBItemImage;
			
				item.BDetailImage = varBDetailImage;
			
				item.BOverrideThumbnail = varBOverrideThumbnail;
			
				item.DetailDescription = varDetailDescription;
			
				item.StorageRemote = varStorageRemote;
			
				item.Path = varPath;
			
				item.ImageName = varImageName;
			
				item.ImageHeight = varImageHeight;
			
				item.ImageWidth = varImageWidth;
			
				item.ThumbClass = varThumbClass;
			
				item.IDisplayOrder = varIDisplayOrder;
			
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
        
        
        
        public static TableSchema.TableColumn TMerchIdColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn TFutureIdColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn BItemImageColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn BDetailImageColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn BOverrideThumbnailColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn DetailDescriptionColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn StorageRemoteColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn PathColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn ImageNameColumn
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        public static TableSchema.TableColumn ImageHeightColumn
        {
            get { return Schema.Columns[10]; }
        }
        
        
        
        public static TableSchema.TableColumn ImageWidthColumn
        {
            get { return Schema.Columns[11]; }
        }
        
        
        
        public static TableSchema.TableColumn ThumbClassColumn
        {
            get { return Schema.Columns[12]; }
        }
        
        
        
        public static TableSchema.TableColumn IDisplayOrderColumn
        {
            get { return Schema.Columns[13]; }
        }
        
        
        
        public static TableSchema.TableColumn DtStampColumn
        {
            get { return Schema.Columns[14]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Id = @"Id";
			 public static string TMerchId = @"TMerchId";
			 public static string TFutureId = @"TFutureId";
			 public static string BItemImage = @"bItemImage";
			 public static string BDetailImage = @"bDetailImage";
			 public static string BOverrideThumbnail = @"bOverrideThumbnail";
			 public static string DetailDescription = @"DetailDescription";
			 public static string StorageRemote = @"StorageRemote";
			 public static string Path = @"Path";
			 public static string ImageName = @"ImageName";
			 public static string ImageHeight = @"ImageHeight";
			 public static string ImageWidth = @"ImageWidth";
			 public static string ThumbClass = @"ThumbClass";
			 public static string IDisplayOrder = @"iDisplayOrder";
			 public static string DtStamp = @"dtStamp";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
