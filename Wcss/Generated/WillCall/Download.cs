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
	/// Strongly-typed collection for the Download class.
	/// </summary>
    [Serializable]
	public partial class DownloadCollection : ActiveList<Download, DownloadCollection>
	{	   
		public DownloadCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>DownloadCollection</returns>
		public DownloadCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                Download o = this[i];
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
	/// This is an ActiveRecord class which wraps the Download table.
	/// </summary>
	[Serializable]
	public partial class Download : ActiveRecord<Download>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public Download()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public Download(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public Download(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public Download(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("Download", TableType.Table, DataService.GetInstance("WillCall"));
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
				
				TableSchema.TableColumn colvarTrackNumber = new TableSchema.TableColumn(schema);
				colvarTrackNumber.ColumnName = "TrackNumber";
				colvarTrackNumber.DataType = DbType.AnsiString;
				colvarTrackNumber.MaxLength = 10;
				colvarTrackNumber.AutoIncrement = false;
				colvarTrackNumber.IsNullable = true;
				colvarTrackNumber.IsPrimaryKey = false;
				colvarTrackNumber.IsForeignKey = false;
				colvarTrackNumber.IsReadOnly = false;
				colvarTrackNumber.DefaultSetting = @"";
				colvarTrackNumber.ForeignKeyTableName = "";
				schema.Columns.Add(colvarTrackNumber);
				
				TableSchema.TableColumn colvarTitle = new TableSchema.TableColumn(schema);
				colvarTitle.ColumnName = "Title";
				colvarTitle.DataType = DbType.AnsiString;
				colvarTitle.MaxLength = 500;
				colvarTitle.AutoIncrement = false;
				colvarTitle.IsNullable = false;
				colvarTitle.IsPrimaryKey = false;
				colvarTitle.IsForeignKey = false;
				colvarTitle.IsReadOnly = false;
				colvarTitle.DefaultSetting = @"";
				colvarTitle.ForeignKeyTableName = "";
				schema.Columns.Add(colvarTitle);
				
				TableSchema.TableColumn colvarVcFileContext = new TableSchema.TableColumn(schema);
				colvarVcFileContext.ColumnName = "vcFileContext";
				colvarVcFileContext.DataType = DbType.AnsiString;
				colvarVcFileContext.MaxLength = 50;
				colvarVcFileContext.AutoIncrement = false;
				colvarVcFileContext.IsNullable = true;
				colvarVcFileContext.IsPrimaryKey = false;
				colvarVcFileContext.IsForeignKey = false;
				colvarVcFileContext.IsReadOnly = false;
				colvarVcFileContext.DefaultSetting = @"";
				colvarVcFileContext.ForeignKeyTableName = "";
				schema.Columns.Add(colvarVcFileContext);
				
				TableSchema.TableColumn colvarVcTrackContext = new TableSchema.TableColumn(schema);
				colvarVcTrackContext.ColumnName = "vcTrackContext";
				colvarVcTrackContext.DataType = DbType.AnsiString;
				colvarVcTrackContext.MaxLength = 50;
				colvarVcTrackContext.AutoIncrement = false;
				colvarVcTrackContext.IsNullable = true;
				colvarVcTrackContext.IsPrimaryKey = false;
				colvarVcTrackContext.IsForeignKey = false;
				colvarVcTrackContext.IsReadOnly = false;
				colvarVcTrackContext.DefaultSetting = @"";
				colvarVcTrackContext.ForeignKeyTableName = "";
				schema.Columns.Add(colvarVcTrackContext);
				
				TableSchema.TableColumn colvarVcGenre = new TableSchema.TableColumn(schema);
				colvarVcGenre.ColumnName = "vcGenre";
				colvarVcGenre.DataType = DbType.AnsiString;
				colvarVcGenre.MaxLength = 50;
				colvarVcGenre.AutoIncrement = false;
				colvarVcGenre.IsNullable = true;
				colvarVcGenre.IsPrimaryKey = false;
				colvarVcGenre.IsForeignKey = false;
				colvarVcGenre.IsReadOnly = false;
				colvarVcGenre.DefaultSetting = @"";
				colvarVcGenre.ForeignKeyTableName = "";
				schema.Columns.Add(colvarVcGenre);
				
				TableSchema.TableColumn colvarVcKeywords = new TableSchema.TableColumn(schema);
				colvarVcKeywords.ColumnName = "vcKeywords";
				colvarVcKeywords.DataType = DbType.AnsiString;
				colvarVcKeywords.MaxLength = 500;
				colvarVcKeywords.AutoIncrement = false;
				colvarVcKeywords.IsNullable = true;
				colvarVcKeywords.IsPrimaryKey = false;
				colvarVcKeywords.IsForeignKey = false;
				colvarVcKeywords.IsReadOnly = false;
				colvarVcKeywords.DefaultSetting = @"";
				colvarVcKeywords.ForeignKeyTableName = "";
				schema.Columns.Add(colvarVcKeywords);
				
				TableSchema.TableColumn colvarTActId = new TableSchema.TableColumn(schema);
				colvarTActId.ColumnName = "TActId";
				colvarTActId.DataType = DbType.Int32;
				colvarTActId.MaxLength = 0;
				colvarTActId.AutoIncrement = false;
				colvarTActId.IsNullable = true;
				colvarTActId.IsPrimaryKey = false;
				colvarTActId.IsForeignKey = true;
				colvarTActId.IsReadOnly = false;
				colvarTActId.DefaultSetting = @"";
				
					colvarTActId.ForeignKeyTableName = "Act";
				schema.Columns.Add(colvarTActId);
				
				TableSchema.TableColumn colvarBaseStoragePath = new TableSchema.TableColumn(schema);
				colvarBaseStoragePath.ColumnName = "BaseStoragePath";
				colvarBaseStoragePath.DataType = DbType.AnsiString;
				colvarBaseStoragePath.MaxLength = 500;
				colvarBaseStoragePath.AutoIncrement = false;
				colvarBaseStoragePath.IsNullable = true;
				colvarBaseStoragePath.IsPrimaryKey = false;
				colvarBaseStoragePath.IsForeignKey = false;
				colvarBaseStoragePath.IsReadOnly = false;
				colvarBaseStoragePath.DefaultSetting = @"";
				colvarBaseStoragePath.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBaseStoragePath);
				
				TableSchema.TableColumn colvarApplicationName = new TableSchema.TableColumn(schema);
				colvarApplicationName.ColumnName = "ApplicationName";
				colvarApplicationName.DataType = DbType.AnsiString;
				colvarApplicationName.MaxLength = 256;
				colvarApplicationName.AutoIncrement = false;
				colvarApplicationName.IsNullable = false;
				colvarApplicationName.IsPrimaryKey = false;
				colvarApplicationName.IsForeignKey = false;
				colvarApplicationName.IsReadOnly = false;
				colvarApplicationName.DefaultSetting = @"";
				colvarApplicationName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarApplicationName);
				
				TableSchema.TableColumn colvarCompilation = new TableSchema.TableColumn(schema);
				colvarCompilation.ColumnName = "Compilation";
				colvarCompilation.DataType = DbType.AnsiString;
				colvarCompilation.MaxLength = 500;
				colvarCompilation.AutoIncrement = false;
				colvarCompilation.IsNullable = true;
				colvarCompilation.IsPrimaryKey = false;
				colvarCompilation.IsForeignKey = false;
				colvarCompilation.IsReadOnly = false;
				colvarCompilation.DefaultSetting = @"";
				colvarCompilation.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCompilation);
				
				TableSchema.TableColumn colvarArtist = new TableSchema.TableColumn(schema);
				colvarArtist.ColumnName = "Artist";
				colvarArtist.DataType = DbType.AnsiString;
				colvarArtist.MaxLength = 500;
				colvarArtist.AutoIncrement = false;
				colvarArtist.IsNullable = true;
				colvarArtist.IsPrimaryKey = false;
				colvarArtist.IsForeignKey = false;
				colvarArtist.IsReadOnly = false;
				colvarArtist.DefaultSetting = @"";
				colvarArtist.ForeignKeyTableName = "";
				schema.Columns.Add(colvarArtist);
				
				TableSchema.TableColumn colvarAlbum = new TableSchema.TableColumn(schema);
				colvarAlbum.ColumnName = "Album";
				colvarAlbum.DataType = DbType.AnsiString;
				colvarAlbum.MaxLength = 500;
				colvarAlbum.AutoIncrement = false;
				colvarAlbum.IsNullable = true;
				colvarAlbum.IsPrimaryKey = false;
				colvarAlbum.IsForeignKey = false;
				colvarAlbum.IsReadOnly = false;
				colvarAlbum.DefaultSetting = @"";
				colvarAlbum.ForeignKeyTableName = "";
				schema.Columns.Add(colvarAlbum);
				
				TableSchema.TableColumn colvarFileName = new TableSchema.TableColumn(schema);
				colvarFileName.ColumnName = "FileName";
				colvarFileName.DataType = DbType.AnsiString;
				colvarFileName.MaxLength = 256;
				colvarFileName.AutoIncrement = false;
				colvarFileName.IsNullable = true;
				colvarFileName.IsPrimaryKey = false;
				colvarFileName.IsForeignKey = false;
				colvarFileName.IsReadOnly = false;
				colvarFileName.DefaultSetting = @"";
				colvarFileName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarFileName);
				
				TableSchema.TableColumn colvarVcFormat = new TableSchema.TableColumn(schema);
				colvarVcFormat.ColumnName = "vcFormat";
				colvarVcFormat.DataType = DbType.AnsiString;
				colvarVcFormat.MaxLength = 50;
				colvarVcFormat.AutoIncrement = false;
				colvarVcFormat.IsNullable = true;
				colvarVcFormat.IsPrimaryKey = false;
				colvarVcFormat.IsForeignKey = false;
				colvarVcFormat.IsReadOnly = false;
				colvarVcFormat.DefaultSetting = @"";
				colvarVcFormat.ForeignKeyTableName = "";
				schema.Columns.Add(colvarVcFormat);
				
				TableSchema.TableColumn colvarFileBinary = new TableSchema.TableColumn(schema);
				colvarFileBinary.ColumnName = "FileBinary";
				colvarFileBinary.DataType = DbType.Binary;
				colvarFileBinary.MaxLength = 2147483647;
				colvarFileBinary.AutoIncrement = false;
				colvarFileBinary.IsNullable = true;
				colvarFileBinary.IsPrimaryKey = false;
				colvarFileBinary.IsForeignKey = false;
				colvarFileBinary.IsReadOnly = false;
				colvarFileBinary.DefaultSetting = @"";
				colvarFileBinary.ForeignKeyTableName = "";
				schema.Columns.Add(colvarFileBinary);
				
				TableSchema.TableColumn colvarFileTime = new TableSchema.TableColumn(schema);
				colvarFileTime.ColumnName = "FileTime";
				colvarFileTime.DataType = DbType.AnsiString;
				colvarFileTime.MaxLength = 50;
				colvarFileTime.AutoIncrement = false;
				colvarFileTime.IsNullable = true;
				colvarFileTime.IsPrimaryKey = false;
				colvarFileTime.IsForeignKey = false;
				colvarFileTime.IsReadOnly = false;
				colvarFileTime.DefaultSetting = @"";
				colvarFileTime.ForeignKeyTableName = "";
				schema.Columns.Add(colvarFileTime);
				
				TableSchema.TableColumn colvarIFileBytes = new TableSchema.TableColumn(schema);
				colvarIFileBytes.ColumnName = "iFileBytes";
				colvarIFileBytes.DataType = DbType.Int32;
				colvarIFileBytes.MaxLength = 0;
				colvarIFileBytes.AutoIncrement = false;
				colvarIFileBytes.IsNullable = false;
				colvarIFileBytes.IsPrimaryKey = false;
				colvarIFileBytes.IsForeignKey = false;
				colvarIFileBytes.IsReadOnly = false;
				
						colvarIFileBytes.DefaultSetting = @"((-1))";
				colvarIFileBytes.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIFileBytes);
				
				TableSchema.TableColumn colvarSampleFile = new TableSchema.TableColumn(schema);
				colvarSampleFile.ColumnName = "SampleFile";
				colvarSampleFile.DataType = DbType.AnsiString;
				colvarSampleFile.MaxLength = 500;
				colvarSampleFile.AutoIncrement = false;
				colvarSampleFile.IsNullable = true;
				colvarSampleFile.IsPrimaryKey = false;
				colvarSampleFile.IsForeignKey = false;
				colvarSampleFile.IsReadOnly = false;
				colvarSampleFile.DefaultSetting = @"";
				colvarSampleFile.ForeignKeyTableName = "";
				schema.Columns.Add(colvarSampleFile);
				
				TableSchema.TableColumn colvarSampleBinary = new TableSchema.TableColumn(schema);
				colvarSampleBinary.ColumnName = "SampleBinary";
				colvarSampleBinary.DataType = DbType.AnsiString;
				colvarSampleBinary.MaxLength = 500;
				colvarSampleBinary.AutoIncrement = false;
				colvarSampleBinary.IsNullable = true;
				colvarSampleBinary.IsPrimaryKey = false;
				colvarSampleBinary.IsForeignKey = false;
				colvarSampleBinary.IsReadOnly = false;
				colvarSampleBinary.DefaultSetting = @"";
				colvarSampleBinary.ForeignKeyTableName = "";
				schema.Columns.Add(colvarSampleBinary);
				
				TableSchema.TableColumn colvarISampleClick = new TableSchema.TableColumn(schema);
				colvarISampleClick.ColumnName = "iSampleClick";
				colvarISampleClick.DataType = DbType.Int32;
				colvarISampleClick.MaxLength = 0;
				colvarISampleClick.AutoIncrement = false;
				colvarISampleClick.IsNullable = false;
				colvarISampleClick.IsPrimaryKey = false;
				colvarISampleClick.IsForeignKey = false;
				colvarISampleClick.IsReadOnly = false;
				
						colvarISampleClick.DefaultSetting = @"((0))";
				colvarISampleClick.ForeignKeyTableName = "";
				schema.Columns.Add(colvarISampleClick);
				
				TableSchema.TableColumn colvarIAttempted = new TableSchema.TableColumn(schema);
				colvarIAttempted.ColumnName = "iAttempted";
				colvarIAttempted.DataType = DbType.Int32;
				colvarIAttempted.MaxLength = 0;
				colvarIAttempted.AutoIncrement = false;
				colvarIAttempted.IsNullable = false;
				colvarIAttempted.IsPrimaryKey = false;
				colvarIAttempted.IsForeignKey = false;
				colvarIAttempted.IsReadOnly = false;
				
						colvarIAttempted.DefaultSetting = @"((0))";
				colvarIAttempted.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIAttempted);
				
				TableSchema.TableColumn colvarISuccessful = new TableSchema.TableColumn(schema);
				colvarISuccessful.ColumnName = "iSuccessful";
				colvarISuccessful.DataType = DbType.Int32;
				colvarISuccessful.MaxLength = 0;
				colvarISuccessful.AutoIncrement = false;
				colvarISuccessful.IsNullable = false;
				colvarISuccessful.IsPrimaryKey = false;
				colvarISuccessful.IsForeignKey = false;
				colvarISuccessful.IsReadOnly = false;
				
						colvarISuccessful.DefaultSetting = @"((0))";
				colvarISuccessful.ForeignKeyTableName = "";
				schema.Columns.Add(colvarISuccessful);
				
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
				
				TableSchema.TableColumn colvarDtLastValidated = new TableSchema.TableColumn(schema);
				colvarDtLastValidated.ColumnName = "dtLastValidated";
				colvarDtLastValidated.DataType = DbType.DateTime;
				colvarDtLastValidated.MaxLength = 0;
				colvarDtLastValidated.AutoIncrement = false;
				colvarDtLastValidated.IsNullable = true;
				colvarDtLastValidated.IsPrimaryKey = false;
				colvarDtLastValidated.IsForeignKey = false;
				colvarDtLastValidated.IsReadOnly = false;
				colvarDtLastValidated.DefaultSetting = @"";
				colvarDtLastValidated.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDtLastValidated);
				
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
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["WillCall"].AddSchema("Download",schema);
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
		  
		[XmlAttribute("TrackNumber")]
		[Bindable(true)]
		public string TrackNumber 
		{
			get { return GetColumnValue<string>(Columns.TrackNumber); }
			set { SetColumnValue(Columns.TrackNumber, value); }
		}
		  
		[XmlAttribute("Title")]
		[Bindable(true)]
		public string Title 
		{
			get { return GetColumnValue<string>(Columns.Title); }
			set { SetColumnValue(Columns.Title, value); }
		}
		  
		[XmlAttribute("VcFileContext")]
		[Bindable(true)]
		public string VcFileContext 
		{
			get { return GetColumnValue<string>(Columns.VcFileContext); }
			set { SetColumnValue(Columns.VcFileContext, value); }
		}
		  
		[XmlAttribute("VcTrackContext")]
		[Bindable(true)]
		public string VcTrackContext 
		{
			get { return GetColumnValue<string>(Columns.VcTrackContext); }
			set { SetColumnValue(Columns.VcTrackContext, value); }
		}
		  
		[XmlAttribute("VcGenre")]
		[Bindable(true)]
		public string VcGenre 
		{
			get { return GetColumnValue<string>(Columns.VcGenre); }
			set { SetColumnValue(Columns.VcGenre, value); }
		}
		  
		[XmlAttribute("VcKeywords")]
		[Bindable(true)]
		public string VcKeywords 
		{
			get { return GetColumnValue<string>(Columns.VcKeywords); }
			set { SetColumnValue(Columns.VcKeywords, value); }
		}
		  
		[XmlAttribute("TActId")]
		[Bindable(true)]
		public int? TActId 
		{
			get { return GetColumnValue<int?>(Columns.TActId); }
			set { SetColumnValue(Columns.TActId, value); }
		}
		  
		[XmlAttribute("BaseStoragePath")]
		[Bindable(true)]
		public string BaseStoragePath 
		{
			get { return GetColumnValue<string>(Columns.BaseStoragePath); }
			set { SetColumnValue(Columns.BaseStoragePath, value); }
		}
		  
		[XmlAttribute("ApplicationName")]
		[Bindable(true)]
		public string ApplicationName 
		{
			get { return GetColumnValue<string>(Columns.ApplicationName); }
			set { SetColumnValue(Columns.ApplicationName, value); }
		}
		  
		[XmlAttribute("Compilation")]
		[Bindable(true)]
		public string Compilation 
		{
			get { return GetColumnValue<string>(Columns.Compilation); }
			set { SetColumnValue(Columns.Compilation, value); }
		}
		  
		[XmlAttribute("Artist")]
		[Bindable(true)]
		public string Artist 
		{
			get { return GetColumnValue<string>(Columns.Artist); }
			set { SetColumnValue(Columns.Artist, value); }
		}
		  
		[XmlAttribute("Album")]
		[Bindable(true)]
		public string Album 
		{
			get { return GetColumnValue<string>(Columns.Album); }
			set { SetColumnValue(Columns.Album, value); }
		}
		  
		[XmlAttribute("FileName")]
		[Bindable(true)]
		public string FileName 
		{
			get { return GetColumnValue<string>(Columns.FileName); }
			set { SetColumnValue(Columns.FileName, value); }
		}
		  
		[XmlAttribute("VcFormat")]
		[Bindable(true)]
		public string VcFormat 
		{
			get { return GetColumnValue<string>(Columns.VcFormat); }
			set { SetColumnValue(Columns.VcFormat, value); }
		}
		  
		[XmlAttribute("FileBinary")]
		[Bindable(true)]
		public byte[] FileBinary 
		{
			get { return GetColumnValue<byte[]>(Columns.FileBinary); }
			set { SetColumnValue(Columns.FileBinary, value); }
		}
		  
		[XmlAttribute("FileTime")]
		[Bindable(true)]
		public string FileTime 
		{
			get { return GetColumnValue<string>(Columns.FileTime); }
			set { SetColumnValue(Columns.FileTime, value); }
		}
		  
		[XmlAttribute("IFileBytes")]
		[Bindable(true)]
		public int IFileBytes 
		{
			get { return GetColumnValue<int>(Columns.IFileBytes); }
			set { SetColumnValue(Columns.IFileBytes, value); }
		}
		  
		[XmlAttribute("SampleFile")]
		[Bindable(true)]
		public string SampleFile 
		{
			get { return GetColumnValue<string>(Columns.SampleFile); }
			set { SetColumnValue(Columns.SampleFile, value); }
		}
		  
		[XmlAttribute("SampleBinary")]
		[Bindable(true)]
		public string SampleBinary 
		{
			get { return GetColumnValue<string>(Columns.SampleBinary); }
			set { SetColumnValue(Columns.SampleBinary, value); }
		}
		  
		[XmlAttribute("ISampleClick")]
		[Bindable(true)]
		public int ISampleClick 
		{
			get { return GetColumnValue<int>(Columns.ISampleClick); }
			set { SetColumnValue(Columns.ISampleClick, value); }
		}
		  
		[XmlAttribute("IAttempted")]
		[Bindable(true)]
		public int IAttempted 
		{
			get { return GetColumnValue<int>(Columns.IAttempted); }
			set { SetColumnValue(Columns.IAttempted, value); }
		}
		  
		[XmlAttribute("ISuccessful")]
		[Bindable(true)]
		public int ISuccessful 
		{
			get { return GetColumnValue<int>(Columns.ISuccessful); }
			set { SetColumnValue(Columns.ISuccessful, value); }
		}
		  
		[XmlAttribute("DtStamp")]
		[Bindable(true)]
		public DateTime DtStamp 
		{
			get { return GetColumnValue<DateTime>(Columns.DtStamp); }
			set { SetColumnValue(Columns.DtStamp, value); }
		}
		  
		[XmlAttribute("DtLastValidated")]
		[Bindable(true)]
		public DateTime? DtLastValidated 
		{
			get { return GetColumnValue<DateTime?>(Columns.DtLastValidated); }
			set { SetColumnValue(Columns.DtLastValidated, value); }
		}
		  
		[XmlAttribute("ApplicationId")]
		[Bindable(true)]
		public Guid ApplicationId 
		{
			get { return GetColumnValue<Guid>(Columns.ApplicationId); }
			set { SetColumnValue(Columns.ApplicationId, value); }
		}
		
		#endregion
		
		
		#region PrimaryKey Methods		
		
        protected override void SetPrimaryKey(object oValue)
        {
            base.SetPrimaryKey(oValue);
            
            SetPKValues();
        }
        
		
		private Wcss.MerchDownloadCollection colMerchDownloadRecords;
		public Wcss.MerchDownloadCollection MerchDownloadRecords()
		{
			if(colMerchDownloadRecords == null)
			{
				colMerchDownloadRecords = new Wcss.MerchDownloadCollection().Where(MerchDownload.Columns.TDownloadId, Id).Load();
				colMerchDownloadRecords.ListChanged += new ListChangedEventHandler(colMerchDownloadRecords_ListChanged);
			}
			return colMerchDownloadRecords;
		}
				
		void colMerchDownloadRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colMerchDownloadRecords[e.NewIndex].TDownloadId = Id;
				colMerchDownloadRecords.ListChanged += new ListChangedEventHandler(colMerchDownloadRecords_ListChanged);
            }
		}
		#endregion
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a Act ActiveRecord object related to this Download
		/// 
		/// </summary>
		private Wcss.Act Act
		{
			get { return Wcss.Act.FetchByID(this.TActId); }
			set { SetColumnValue("TActId", value.Id); }
		}
        //set up an alternate mechanism to avoid a database call
		private Wcss.Act _actrecord = null;
		
		public Wcss.Act ActRecord
		{
		    get
            {
                if (_actrecord == null)
                {
                    _actrecord = new Wcss.Act();
                    _actrecord.CopyFrom(this.Act);
                }
                return _actrecord;
            }
            set
            {
                if(value != null && _actrecord == null)
			        _actrecord = new Wcss.Act();
                
                SetColumnValue("TActId", value.Id);
                _actrecord.CopyFrom(value);                
            }
		}
		
		
		/// <summary>
		/// Returns a AspnetApplication ActiveRecord object related to this Download
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
		public static void Insert(string varTrackNumber,string varTitle,string varVcFileContext,string varVcTrackContext,string varVcGenre,string varVcKeywords,int? varTActId,string varBaseStoragePath,string varApplicationName,string varCompilation,string varArtist,string varAlbum,string varFileName,string varVcFormat,byte[] varFileBinary,string varFileTime,int varIFileBytes,string varSampleFile,string varSampleBinary,int varISampleClick,int varIAttempted,int varISuccessful,DateTime varDtStamp,DateTime? varDtLastValidated,Guid varApplicationId)
		{
			Download item = new Download();
			
			item.TrackNumber = varTrackNumber;
			
			item.Title = varTitle;
			
			item.VcFileContext = varVcFileContext;
			
			item.VcTrackContext = varVcTrackContext;
			
			item.VcGenre = varVcGenre;
			
			item.VcKeywords = varVcKeywords;
			
			item.TActId = varTActId;
			
			item.BaseStoragePath = varBaseStoragePath;
			
			item.ApplicationName = varApplicationName;
			
			item.Compilation = varCompilation;
			
			item.Artist = varArtist;
			
			item.Album = varAlbum;
			
			item.FileName = varFileName;
			
			item.VcFormat = varVcFormat;
			
			item.FileBinary = varFileBinary;
			
			item.FileTime = varFileTime;
			
			item.IFileBytes = varIFileBytes;
			
			item.SampleFile = varSampleFile;
			
			item.SampleBinary = varSampleBinary;
			
			item.ISampleClick = varISampleClick;
			
			item.IAttempted = varIAttempted;
			
			item.ISuccessful = varISuccessful;
			
			item.DtStamp = varDtStamp;
			
			item.DtLastValidated = varDtLastValidated;
			
			item.ApplicationId = varApplicationId;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varId,string varTrackNumber,string varTitle,string varVcFileContext,string varVcTrackContext,string varVcGenre,string varVcKeywords,int? varTActId,string varBaseStoragePath,string varApplicationName,string varCompilation,string varArtist,string varAlbum,string varFileName,string varVcFormat,byte[] varFileBinary,string varFileTime,int varIFileBytes,string varSampleFile,string varSampleBinary,int varISampleClick,int varIAttempted,int varISuccessful,DateTime varDtStamp,DateTime? varDtLastValidated,Guid varApplicationId)
		{
			Download item = new Download();
			
				item.Id = varId;
			
				item.TrackNumber = varTrackNumber;
			
				item.Title = varTitle;
			
				item.VcFileContext = varVcFileContext;
			
				item.VcTrackContext = varVcTrackContext;
			
				item.VcGenre = varVcGenre;
			
				item.VcKeywords = varVcKeywords;
			
				item.TActId = varTActId;
			
				item.BaseStoragePath = varBaseStoragePath;
			
				item.ApplicationName = varApplicationName;
			
				item.Compilation = varCompilation;
			
				item.Artist = varArtist;
			
				item.Album = varAlbum;
			
				item.FileName = varFileName;
			
				item.VcFormat = varVcFormat;
			
				item.FileBinary = varFileBinary;
			
				item.FileTime = varFileTime;
			
				item.IFileBytes = varIFileBytes;
			
				item.SampleFile = varSampleFile;
			
				item.SampleBinary = varSampleBinary;
			
				item.ISampleClick = varISampleClick;
			
				item.IAttempted = varIAttempted;
			
				item.ISuccessful = varISuccessful;
			
				item.DtStamp = varDtStamp;
			
				item.DtLastValidated = varDtLastValidated;
			
				item.ApplicationId = varApplicationId;
			
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
        
        
        
        public static TableSchema.TableColumn TrackNumberColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn TitleColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn VcFileContextColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn VcTrackContextColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn VcGenreColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn VcKeywordsColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn TActIdColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn BaseStoragePathColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn ApplicationNameColumn
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        public static TableSchema.TableColumn CompilationColumn
        {
            get { return Schema.Columns[10]; }
        }
        
        
        
        public static TableSchema.TableColumn ArtistColumn
        {
            get { return Schema.Columns[11]; }
        }
        
        
        
        public static TableSchema.TableColumn AlbumColumn
        {
            get { return Schema.Columns[12]; }
        }
        
        
        
        public static TableSchema.TableColumn FileNameColumn
        {
            get { return Schema.Columns[13]; }
        }
        
        
        
        public static TableSchema.TableColumn VcFormatColumn
        {
            get { return Schema.Columns[14]; }
        }
        
        
        
        public static TableSchema.TableColumn FileBinaryColumn
        {
            get { return Schema.Columns[15]; }
        }
        
        
        
        public static TableSchema.TableColumn FileTimeColumn
        {
            get { return Schema.Columns[16]; }
        }
        
        
        
        public static TableSchema.TableColumn IFileBytesColumn
        {
            get { return Schema.Columns[17]; }
        }
        
        
        
        public static TableSchema.TableColumn SampleFileColumn
        {
            get { return Schema.Columns[18]; }
        }
        
        
        
        public static TableSchema.TableColumn SampleBinaryColumn
        {
            get { return Schema.Columns[19]; }
        }
        
        
        
        public static TableSchema.TableColumn ISampleClickColumn
        {
            get { return Schema.Columns[20]; }
        }
        
        
        
        public static TableSchema.TableColumn IAttemptedColumn
        {
            get { return Schema.Columns[21]; }
        }
        
        
        
        public static TableSchema.TableColumn ISuccessfulColumn
        {
            get { return Schema.Columns[22]; }
        }
        
        
        
        public static TableSchema.TableColumn DtStampColumn
        {
            get { return Schema.Columns[23]; }
        }
        
        
        
        public static TableSchema.TableColumn DtLastValidatedColumn
        {
            get { return Schema.Columns[24]; }
        }
        
        
        
        public static TableSchema.TableColumn ApplicationIdColumn
        {
            get { return Schema.Columns[25]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Id = @"Id";
			 public static string TrackNumber = @"TrackNumber";
			 public static string Title = @"Title";
			 public static string VcFileContext = @"vcFileContext";
			 public static string VcTrackContext = @"vcTrackContext";
			 public static string VcGenre = @"vcGenre";
			 public static string VcKeywords = @"vcKeywords";
			 public static string TActId = @"TActId";
			 public static string BaseStoragePath = @"BaseStoragePath";
			 public static string ApplicationName = @"ApplicationName";
			 public static string Compilation = @"Compilation";
			 public static string Artist = @"Artist";
			 public static string Album = @"Album";
			 public static string FileName = @"FileName";
			 public static string VcFormat = @"vcFormat";
			 public static string FileBinary = @"FileBinary";
			 public static string FileTime = @"FileTime";
			 public static string IFileBytes = @"iFileBytes";
			 public static string SampleFile = @"SampleFile";
			 public static string SampleBinary = @"SampleBinary";
			 public static string ISampleClick = @"iSampleClick";
			 public static string IAttempted = @"iAttempted";
			 public static string ISuccessful = @"iSuccessful";
			 public static string DtStamp = @"dtStamp";
			 public static string DtLastValidated = @"dtLastValidated";
			 public static string ApplicationId = @"ApplicationId";
						
		}
		#endregion
		
		#region Update PK Collections
		
        public void SetPKValues()
        {
                if (colMerchDownloadRecords != null)
                {
                    foreach (Wcss.MerchDownload item in colMerchDownloadRecords)
                    {
                        if (item.TDownloadId != Id)
                        {
                            item.TDownloadId = Id;
                        }
                    }
               }
		}
        #endregion
    
        #region Deep Save
		
        public void DeepSave()
        {
            Save();
            
                if (colMerchDownloadRecords != null)
                {
                    colMerchDownloadRecords.SaveAll();
               }
		}
        #endregion
	}
}
