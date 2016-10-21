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
	/// Strongly-typed collection for the Show class.
	/// </summary>
    [Serializable]
	public partial class ShowCollection : ActiveList<Show, ShowCollection>
	{	   
		public ShowCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>ShowCollection</returns>
		public ShowCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                Show o = this[i];
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
	/// This is an ActiveRecord class which wraps the Show table.
	/// </summary>
	[Serializable]
	public partial class Show : ActiveRecord<Show>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public Show()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public Show(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public Show(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public Show(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("Show", TableType.Table, DataService.GetInstance("WillCall"));
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
				
				TableSchema.TableColumn colvarName = new TableSchema.TableColumn(schema);
				colvarName.ColumnName = "Name";
				colvarName.DataType = DbType.AnsiString;
				colvarName.MaxLength = 300;
				colvarName.AutoIncrement = false;
				colvarName.IsNullable = false;
				colvarName.IsPrimaryKey = false;
				colvarName.IsForeignKey = false;
				colvarName.IsReadOnly = false;
				colvarName.DefaultSetting = @"";
				colvarName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarName);
				
				TableSchema.TableColumn colvarDtAnnounceDate = new TableSchema.TableColumn(schema);
				colvarDtAnnounceDate.ColumnName = "dtAnnounceDate";
				colvarDtAnnounceDate.DataType = DbType.DateTime;
				colvarDtAnnounceDate.MaxLength = 0;
				colvarDtAnnounceDate.AutoIncrement = false;
				colvarDtAnnounceDate.IsNullable = true;
				colvarDtAnnounceDate.IsPrimaryKey = false;
				colvarDtAnnounceDate.IsForeignKey = false;
				colvarDtAnnounceDate.IsReadOnly = false;
				colvarDtAnnounceDate.DefaultSetting = @"";
				colvarDtAnnounceDate.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDtAnnounceDate);
				
				TableSchema.TableColumn colvarDtDateOnSale = new TableSchema.TableColumn(schema);
				colvarDtDateOnSale.ColumnName = "dtDateOnSale";
				colvarDtDateOnSale.DataType = DbType.DateTime;
				colvarDtDateOnSale.MaxLength = 0;
				colvarDtDateOnSale.AutoIncrement = false;
				colvarDtDateOnSale.IsNullable = true;
				colvarDtDateOnSale.IsPrimaryKey = false;
				colvarDtDateOnSale.IsForeignKey = false;
				colvarDtDateOnSale.IsReadOnly = false;
				colvarDtDateOnSale.DefaultSetting = @"";
				colvarDtDateOnSale.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDtDateOnSale);
				
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
				
				TableSchema.TableColumn colvarBSoldOut = new TableSchema.TableColumn(schema);
				colvarBSoldOut.ColumnName = "bSoldOut";
				colvarBSoldOut.DataType = DbType.Boolean;
				colvarBSoldOut.MaxLength = 0;
				colvarBSoldOut.AutoIncrement = false;
				colvarBSoldOut.IsNullable = false;
				colvarBSoldOut.IsPrimaryKey = false;
				colvarBSoldOut.IsForeignKey = false;
				colvarBSoldOut.IsReadOnly = false;
				
						colvarBSoldOut.DefaultSetting = @"((0))";
				colvarBSoldOut.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBSoldOut);
				
				TableSchema.TableColumn colvarStatusText = new TableSchema.TableColumn(schema);
				colvarStatusText.ColumnName = "StatusText";
				colvarStatusText.DataType = DbType.AnsiString;
				colvarStatusText.MaxLength = 500;
				colvarStatusText.AutoIncrement = false;
				colvarStatusText.IsNullable = true;
				colvarStatusText.IsPrimaryKey = false;
				colvarStatusText.IsForeignKey = false;
				colvarStatusText.IsReadOnly = false;
				colvarStatusText.DefaultSetting = @"";
				colvarStatusText.ForeignKeyTableName = "";
				schema.Columns.Add(colvarStatusText);
				
				TableSchema.TableColumn colvarVenuePreText = new TableSchema.TableColumn(schema);
				colvarVenuePreText.ColumnName = "VenuePreText";
				colvarVenuePreText.DataType = DbType.AnsiString;
				colvarVenuePreText.MaxLength = 256;
				colvarVenuePreText.AutoIncrement = false;
				colvarVenuePreText.IsNullable = true;
				colvarVenuePreText.IsPrimaryKey = false;
				colvarVenuePreText.IsForeignKey = false;
				colvarVenuePreText.IsReadOnly = false;
				colvarVenuePreText.DefaultSetting = @"";
				colvarVenuePreText.ForeignKeyTableName = "";
				schema.Columns.Add(colvarVenuePreText);
				
				TableSchema.TableColumn colvarTVenueId = new TableSchema.TableColumn(schema);
				colvarTVenueId.ColumnName = "TVenueId";
				colvarTVenueId.DataType = DbType.Int32;
				colvarTVenueId.MaxLength = 0;
				colvarTVenueId.AutoIncrement = false;
				colvarTVenueId.IsNullable = false;
				colvarTVenueId.IsPrimaryKey = false;
				colvarTVenueId.IsForeignKey = true;
				colvarTVenueId.IsReadOnly = false;
				
						colvarTVenueId.DefaultSetting = @"((10000))";
				
					colvarTVenueId.ForeignKeyTableName = "Venue";
				schema.Columns.Add(colvarTVenueId);
				
				TableSchema.TableColumn colvarVenuePostText = new TableSchema.TableColumn(schema);
				colvarVenuePostText.ColumnName = "VenuePostText";
				colvarVenuePostText.DataType = DbType.AnsiString;
				colvarVenuePostText.MaxLength = 256;
				colvarVenuePostText.AutoIncrement = false;
				colvarVenuePostText.IsNullable = true;
				colvarVenuePostText.IsPrimaryKey = false;
				colvarVenuePostText.IsForeignKey = false;
				colvarVenuePostText.IsReadOnly = false;
				colvarVenuePostText.DefaultSetting = @"";
				colvarVenuePostText.ForeignKeyTableName = "";
				schema.Columns.Add(colvarVenuePostText);
				
				TableSchema.TableColumn colvarDisplayNotes = new TableSchema.TableColumn(schema);
				colvarDisplayNotes.ColumnName = "DisplayNotes";
				colvarDisplayNotes.DataType = DbType.AnsiString;
				colvarDisplayNotes.MaxLength = 1000;
				colvarDisplayNotes.AutoIncrement = false;
				colvarDisplayNotes.IsNullable = true;
				colvarDisplayNotes.IsPrimaryKey = false;
				colvarDisplayNotes.IsForeignKey = false;
				colvarDisplayNotes.IsReadOnly = false;
				colvarDisplayNotes.DefaultSetting = @"";
				colvarDisplayNotes.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDisplayNotes);
				
				TableSchema.TableColumn colvarInternalNotes = new TableSchema.TableColumn(schema);
				colvarInternalNotes.ColumnName = "InternalNotes";
				colvarInternalNotes.DataType = DbType.AnsiString;
				colvarInternalNotes.MaxLength = 500;
				colvarInternalNotes.AutoIncrement = false;
				colvarInternalNotes.IsNullable = true;
				colvarInternalNotes.IsPrimaryKey = false;
				colvarInternalNotes.IsForeignKey = false;
				colvarInternalNotes.IsReadOnly = false;
				colvarInternalNotes.DefaultSetting = @"";
				colvarInternalNotes.ForeignKeyTableName = "";
				schema.Columns.Add(colvarInternalNotes);
				
				TableSchema.TableColumn colvarShowTitle = new TableSchema.TableColumn(schema);
				colvarShowTitle.ColumnName = "ShowTitle";
				colvarShowTitle.DataType = DbType.AnsiString;
				colvarShowTitle.MaxLength = 300;
				colvarShowTitle.AutoIncrement = false;
				colvarShowTitle.IsNullable = true;
				colvarShowTitle.IsPrimaryKey = false;
				colvarShowTitle.IsForeignKey = false;
				colvarShowTitle.IsReadOnly = false;
				colvarShowTitle.DefaultSetting = @"";
				colvarShowTitle.ForeignKeyTableName = "";
				schema.Columns.Add(colvarShowTitle);
				
				TableSchema.TableColumn colvarDisplayUrl = new TableSchema.TableColumn(schema);
				colvarDisplayUrl.ColumnName = "DisplayUrl";
				colvarDisplayUrl.DataType = DbType.AnsiString;
				colvarDisplayUrl.MaxLength = 300;
				colvarDisplayUrl.AutoIncrement = false;
				colvarDisplayUrl.IsNullable = true;
				colvarDisplayUrl.IsPrimaryKey = false;
				colvarDisplayUrl.IsForeignKey = false;
				colvarDisplayUrl.IsReadOnly = false;
				colvarDisplayUrl.DefaultSetting = @"";
				colvarDisplayUrl.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDisplayUrl);
				
				TableSchema.TableColumn colvarIPicWidth = new TableSchema.TableColumn(schema);
				colvarIPicWidth.ColumnName = "iPicWidth";
				colvarIPicWidth.DataType = DbType.Int32;
				colvarIPicWidth.MaxLength = 0;
				colvarIPicWidth.AutoIncrement = false;
				colvarIPicWidth.IsNullable = false;
				colvarIPicWidth.IsPrimaryKey = false;
				colvarIPicWidth.IsForeignKey = false;
				colvarIPicWidth.IsReadOnly = false;
				
						colvarIPicWidth.DefaultSetting = @"((0))";
				colvarIPicWidth.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIPicWidth);
				
				TableSchema.TableColumn colvarIPicHeight = new TableSchema.TableColumn(schema);
				colvarIPicHeight.ColumnName = "iPicHeight";
				colvarIPicHeight.DataType = DbType.Int32;
				colvarIPicHeight.MaxLength = 0;
				colvarIPicHeight.AutoIncrement = false;
				colvarIPicHeight.IsNullable = false;
				colvarIPicHeight.IsPrimaryKey = false;
				colvarIPicHeight.IsForeignKey = false;
				colvarIPicHeight.IsReadOnly = false;
				
						colvarIPicHeight.DefaultSetting = @"((0))";
				colvarIPicHeight.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIPicHeight);
				
				TableSchema.TableColumn colvarTopText = new TableSchema.TableColumn(schema);
				colvarTopText.ColumnName = "TopText";
				colvarTopText.DataType = DbType.AnsiString;
				colvarTopText.MaxLength = 300;
				colvarTopText.AutoIncrement = false;
				colvarTopText.IsNullable = true;
				colvarTopText.IsPrimaryKey = false;
				colvarTopText.IsForeignKey = false;
				colvarTopText.IsReadOnly = false;
				colvarTopText.DefaultSetting = @"";
				colvarTopText.ForeignKeyTableName = "";
				schema.Columns.Add(colvarTopText);
				
				TableSchema.TableColumn colvarMidText = new TableSchema.TableColumn(schema);
				colvarMidText.ColumnName = "MidText";
				colvarMidText.DataType = DbType.AnsiString;
				colvarMidText.MaxLength = 300;
				colvarMidText.AutoIncrement = false;
				colvarMidText.IsNullable = true;
				colvarMidText.IsPrimaryKey = false;
				colvarMidText.IsForeignKey = false;
				colvarMidText.IsReadOnly = false;
				colvarMidText.DefaultSetting = @"";
				colvarMidText.ForeignKeyTableName = "";
				schema.Columns.Add(colvarMidText);
				
				TableSchema.TableColumn colvarBDisplayRichText = new TableSchema.TableColumn(schema);
				colvarBDisplayRichText.ColumnName = "bDisplayRichText";
				colvarBDisplayRichText.DataType = DbType.Boolean;
				colvarBDisplayRichText.MaxLength = 0;
				colvarBDisplayRichText.AutoIncrement = false;
				colvarBDisplayRichText.IsNullable = false;
				colvarBDisplayRichText.IsPrimaryKey = false;
				colvarBDisplayRichText.IsForeignKey = false;
				colvarBDisplayRichText.IsReadOnly = false;
				
						colvarBDisplayRichText.DefaultSetting = @"((0))";
				colvarBDisplayRichText.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBDisplayRichText);
				
				TableSchema.TableColumn colvarBHideAutoGenerated = new TableSchema.TableColumn(schema);
				colvarBHideAutoGenerated.ColumnName = "bHideAutoGenerated";
				colvarBHideAutoGenerated.DataType = DbType.Boolean;
				colvarBHideAutoGenerated.MaxLength = 0;
				colvarBHideAutoGenerated.AutoIncrement = false;
				colvarBHideAutoGenerated.IsNullable = false;
				colvarBHideAutoGenerated.IsPrimaryKey = false;
				colvarBHideAutoGenerated.IsForeignKey = false;
				colvarBHideAutoGenerated.IsReadOnly = false;
				
						colvarBHideAutoGenerated.DefaultSetting = @"((0))";
				colvarBHideAutoGenerated.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBHideAutoGenerated);
				
				TableSchema.TableColumn colvarBotText = new TableSchema.TableColumn(schema);
				colvarBotText.ColumnName = "BotText";
				colvarBotText.DataType = DbType.AnsiString;
				colvarBotText.MaxLength = -1;
				colvarBotText.AutoIncrement = false;
				colvarBotText.IsNullable = true;
				colvarBotText.IsPrimaryKey = false;
				colvarBotText.IsForeignKey = false;
				colvarBotText.IsReadOnly = false;
				colvarBotText.DefaultSetting = @"";
				colvarBotText.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBotText);
				
				TableSchema.TableColumn colvarBOverrideActBilling = new TableSchema.TableColumn(schema);
				colvarBOverrideActBilling.ColumnName = "bOverrideActBilling";
				colvarBOverrideActBilling.DataType = DbType.Boolean;
				colvarBOverrideActBilling.MaxLength = 0;
				colvarBOverrideActBilling.AutoIncrement = false;
				colvarBOverrideActBilling.IsNullable = false;
				colvarBOverrideActBilling.IsPrimaryKey = false;
				colvarBOverrideActBilling.IsForeignKey = false;
				colvarBOverrideActBilling.IsReadOnly = false;
				
						colvarBOverrideActBilling.DefaultSetting = @"((0))";
				colvarBOverrideActBilling.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBOverrideActBilling);
				
				TableSchema.TableColumn colvarActBilling = new TableSchema.TableColumn(schema);
				colvarActBilling.ColumnName = "ActBilling";
				colvarActBilling.DataType = DbType.AnsiString;
				colvarActBilling.MaxLength = -1;
				colvarActBilling.AutoIncrement = false;
				colvarActBilling.IsNullable = true;
				colvarActBilling.IsPrimaryKey = false;
				colvarActBilling.IsForeignKey = false;
				colvarActBilling.IsReadOnly = false;
				colvarActBilling.DefaultSetting = @"";
				colvarActBilling.ForeignKeyTableName = "";
				schema.Columns.Add(colvarActBilling);
				
				TableSchema.TableColumn colvarBAllowFacebookLike = new TableSchema.TableColumn(schema);
				colvarBAllowFacebookLike.ColumnName = "bAllowFacebookLike";
				colvarBAllowFacebookLike.DataType = DbType.Boolean;
				colvarBAllowFacebookLike.MaxLength = 0;
				colvarBAllowFacebookLike.AutoIncrement = false;
				colvarBAllowFacebookLike.IsNullable = false;
				colvarBAllowFacebookLike.IsPrimaryKey = false;
				colvarBAllowFacebookLike.IsForeignKey = false;
				colvarBAllowFacebookLike.IsReadOnly = false;
				
						colvarBAllowFacebookLike.DefaultSetting = @"((1))";
				colvarBAllowFacebookLike.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBAllowFacebookLike);
				
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
				
				TableSchema.TableColumn colvarExternalTixUrl = new TableSchema.TableColumn(schema);
				colvarExternalTixUrl.ColumnName = "ExternalTixUrl";
				colvarExternalTixUrl.DataType = DbType.AnsiString;
				colvarExternalTixUrl.MaxLength = 500;
				colvarExternalTixUrl.AutoIncrement = false;
				colvarExternalTixUrl.IsNullable = true;
				colvarExternalTixUrl.IsPrimaryKey = false;
				colvarExternalTixUrl.IsForeignKey = false;
				colvarExternalTixUrl.IsReadOnly = false;
				colvarExternalTixUrl.DefaultSetting = @"";
				colvarExternalTixUrl.ForeignKeyTableName = "";
				schema.Columns.Add(colvarExternalTixUrl);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["WillCall"].AddSchema("Show",schema);
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
		  
		[XmlAttribute("Name")]
		[Bindable(true)]
		public string Name 
		{
			get { return GetColumnValue<string>(Columns.Name); }
			set { SetColumnValue(Columns.Name, value); }
		}
		  
		[XmlAttribute("DtAnnounceDate")]
		[Bindable(true)]
		public DateTime? DtAnnounceDate 
		{
			get { return GetColumnValue<DateTime?>(Columns.DtAnnounceDate); }
			set { SetColumnValue(Columns.DtAnnounceDate, value); }
		}
		  
		[XmlAttribute("DtDateOnSale")]
		[Bindable(true)]
		public DateTime? DtDateOnSale 
		{
			get { return GetColumnValue<DateTime?>(Columns.DtDateOnSale); }
			set { SetColumnValue(Columns.DtDateOnSale, value); }
		}
		  
		[XmlAttribute("BActive")]
		[Bindable(true)]
		public bool BActive 
		{
			get { return GetColumnValue<bool>(Columns.BActive); }
			set { SetColumnValue(Columns.BActive, value); }
		}
		  
		[XmlAttribute("BSoldOut")]
		[Bindable(true)]
		public bool BSoldOut 
		{
			get { return GetColumnValue<bool>(Columns.BSoldOut); }
			set { SetColumnValue(Columns.BSoldOut, value); }
		}
		  
		[XmlAttribute("StatusText")]
		[Bindable(true)]
		public string StatusText 
		{
			get { return GetColumnValue<string>(Columns.StatusText); }
			set { SetColumnValue(Columns.StatusText, value); }
		}
		  
		[XmlAttribute("VenuePreText")]
		[Bindable(true)]
		public string VenuePreText 
		{
			get { return GetColumnValue<string>(Columns.VenuePreText); }
			set { SetColumnValue(Columns.VenuePreText, value); }
		}
		  
		[XmlAttribute("TVenueId")]
		[Bindable(true)]
		public int TVenueId 
		{
			get { return GetColumnValue<int>(Columns.TVenueId); }
			set { SetColumnValue(Columns.TVenueId, value); }
		}
		  
		[XmlAttribute("VenuePostText")]
		[Bindable(true)]
		public string VenuePostText 
		{
			get { return GetColumnValue<string>(Columns.VenuePostText); }
			set { SetColumnValue(Columns.VenuePostText, value); }
		}
		  
		[XmlAttribute("DisplayNotes")]
		[Bindable(true)]
		public string DisplayNotes 
		{
			get { return GetColumnValue<string>(Columns.DisplayNotes); }
			set { SetColumnValue(Columns.DisplayNotes, value); }
		}
		  
		[XmlAttribute("InternalNotes")]
		[Bindable(true)]
		public string InternalNotes 
		{
			get { return GetColumnValue<string>(Columns.InternalNotes); }
			set { SetColumnValue(Columns.InternalNotes, value); }
		}
		  
		[XmlAttribute("ShowTitle")]
		[Bindable(true)]
		public string ShowTitle 
		{
			get { return GetColumnValue<string>(Columns.ShowTitle); }
			set { SetColumnValue(Columns.ShowTitle, value); }
		}
		  
		[XmlAttribute("DisplayUrl")]
		[Bindable(true)]
		public string DisplayUrl 
		{
			get { return GetColumnValue<string>(Columns.DisplayUrl); }
			set { SetColumnValue(Columns.DisplayUrl, value); }
		}
		  
		[XmlAttribute("IPicWidth")]
		[Bindable(true)]
		public int IPicWidth 
		{
			get { return GetColumnValue<int>(Columns.IPicWidth); }
			set { SetColumnValue(Columns.IPicWidth, value); }
		}
		  
		[XmlAttribute("IPicHeight")]
		[Bindable(true)]
		public int IPicHeight 
		{
			get { return GetColumnValue<int>(Columns.IPicHeight); }
			set { SetColumnValue(Columns.IPicHeight, value); }
		}
		  
		[XmlAttribute("TopText")]
		[Bindable(true)]
		public string TopText 
		{
			get { return GetColumnValue<string>(Columns.TopText); }
			set { SetColumnValue(Columns.TopText, value); }
		}
		  
		[XmlAttribute("MidText")]
		[Bindable(true)]
		public string MidText 
		{
			get { return GetColumnValue<string>(Columns.MidText); }
			set { SetColumnValue(Columns.MidText, value); }
		}
		  
		[XmlAttribute("BDisplayRichText")]
		[Bindable(true)]
		public bool BDisplayRichText 
		{
			get { return GetColumnValue<bool>(Columns.BDisplayRichText); }
			set { SetColumnValue(Columns.BDisplayRichText, value); }
		}
		  
		[XmlAttribute("BHideAutoGenerated")]
		[Bindable(true)]
		public bool BHideAutoGenerated 
		{
			get { return GetColumnValue<bool>(Columns.BHideAutoGenerated); }
			set { SetColumnValue(Columns.BHideAutoGenerated, value); }
		}
		  
		[XmlAttribute("BotText")]
		[Bindable(true)]
		public string BotText 
		{
			get { return GetColumnValue<string>(Columns.BotText); }
			set { SetColumnValue(Columns.BotText, value); }
		}
		  
		[XmlAttribute("BOverrideActBilling")]
		[Bindable(true)]
		public bool BOverrideActBilling 
		{
			get { return GetColumnValue<bool>(Columns.BOverrideActBilling); }
			set { SetColumnValue(Columns.BOverrideActBilling, value); }
		}
		  
		[XmlAttribute("ActBilling")]
		[Bindable(true)]
		public string ActBilling 
		{
			get { return GetColumnValue<string>(Columns.ActBilling); }
			set { SetColumnValue(Columns.ActBilling, value); }
		}
		  
		[XmlAttribute("BAllowFacebookLike")]
		[Bindable(true)]
		public bool BAllowFacebookLike 
		{
			get { return GetColumnValue<bool>(Columns.BAllowFacebookLike); }
			set { SetColumnValue(Columns.BAllowFacebookLike, value); }
		}
		  
		[XmlAttribute("DtStamp")]
		[Bindable(true)]
		public DateTime DtStamp 
		{
			get { return GetColumnValue<DateTime>(Columns.DtStamp); }
			set { SetColumnValue(Columns.DtStamp, value); }
		}
		  
		[XmlAttribute("ApplicationId")]
		[Bindable(true)]
		public Guid ApplicationId 
		{
			get { return GetColumnValue<Guid>(Columns.ApplicationId); }
			set { SetColumnValue(Columns.ApplicationId, value); }
		}
		  
		[XmlAttribute("ExternalTixUrl")]
		[Bindable(true)]
		public string ExternalTixUrl 
		{
			get { return GetColumnValue<string>(Columns.ExternalTixUrl); }
			set { SetColumnValue(Columns.ExternalTixUrl, value); }
		}
		
		#endregion
		
		
		#region PrimaryKey Methods		
		
        protected override void SetPrimaryKey(object oValue)
        {
            base.SetPrimaryKey(oValue);
            
            SetPKValues();
        }
        
		
		private Wcss.HeaderImageCollection colHeaderImageRecords;
		public Wcss.HeaderImageCollection HeaderImageRecords()
		{
			if(colHeaderImageRecords == null)
			{
				colHeaderImageRecords = new Wcss.HeaderImageCollection().Where(HeaderImage.Columns.TShowId, Id).Load();
				colHeaderImageRecords.ListChanged += new ListChangedEventHandler(colHeaderImageRecords_ListChanged);
			}
			return colHeaderImageRecords;
		}
				
		void colHeaderImageRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colHeaderImageRecords[e.NewIndex].TShowId = Id;
				colHeaderImageRecords.ListChanged += new ListChangedEventHandler(colHeaderImageRecords_ListChanged);
            }
		}
		private Wcss.JShowPromoterCollection colJShowPromoterRecords;
		public Wcss.JShowPromoterCollection JShowPromoterRecords()
		{
			if(colJShowPromoterRecords == null)
			{
				colJShowPromoterRecords = new Wcss.JShowPromoterCollection().Where(JShowPromoter.Columns.TShowId, Id).Load();
				colJShowPromoterRecords.ListChanged += new ListChangedEventHandler(colJShowPromoterRecords_ListChanged);
			}
			return colJShowPromoterRecords;
		}
				
		void colJShowPromoterRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colJShowPromoterRecords[e.NewIndex].TShowId = Id;
				colJShowPromoterRecords.ListChanged += new ListChangedEventHandler(colJShowPromoterRecords_ListChanged);
            }
		}
		private Wcss.ShowDateCollection colShowDateRecords;
		public Wcss.ShowDateCollection ShowDateRecords()
		{
			if(colShowDateRecords == null)
			{
				colShowDateRecords = new Wcss.ShowDateCollection().Where(ShowDate.Columns.TShowId, Id).Load();
				colShowDateRecords.ListChanged += new ListChangedEventHandler(colShowDateRecords_ListChanged);
			}
			return colShowDateRecords;
		}
				
		void colShowDateRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colShowDateRecords[e.NewIndex].TShowId = Id;
				colShowDateRecords.ListChanged += new ListChangedEventHandler(colShowDateRecords_ListChanged);
            }
		}
		private Wcss.ShowLinkCollection colShowLinkRecords;
		public Wcss.ShowLinkCollection ShowLinkRecords()
		{
			if(colShowLinkRecords == null)
			{
				colShowLinkRecords = new Wcss.ShowLinkCollection().Where(ShowLink.Columns.TShowId, Id).Load();
				colShowLinkRecords.ListChanged += new ListChangedEventHandler(colShowLinkRecords_ListChanged);
			}
			return colShowLinkRecords;
		}
				
		void colShowLinkRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colShowLinkRecords[e.NewIndex].TShowId = Id;
				colShowLinkRecords.ListChanged += new ListChangedEventHandler(colShowLinkRecords_ListChanged);
            }
		}
		#endregion
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a AspnetApplication ActiveRecord object related to this Show
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
		
		
		/// <summary>
		/// Returns a Venue ActiveRecord object related to this Show
		/// 
		/// </summary>
		private Wcss.Venue Venue
		{
			get { return Wcss.Venue.FetchByID(this.TVenueId); }
			set { SetColumnValue("TVenueId", value.Id); }
		}
        //set up an alternate mechanism to avoid a database call
		private Wcss.Venue _venuerecord = null;
		
		public Wcss.Venue VenueRecord
		{
		    get
            {
                if (_venuerecord == null)
                {
                    _venuerecord = new Wcss.Venue();
                    _venuerecord.CopyFrom(this.Venue);
                }
                return _venuerecord;
            }
            set
            {
                if(value != null && _venuerecord == null)
			        _venuerecord = new Wcss.Venue();
                
                SetColumnValue("TVenueId", value.Id);
                _venuerecord.CopyFrom(value);                
            }
		}
		
		
		#endregion
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(string varName,DateTime? varDtAnnounceDate,DateTime? varDtDateOnSale,bool varBActive,bool varBSoldOut,string varStatusText,string varVenuePreText,int varTVenueId,string varVenuePostText,string varDisplayNotes,string varInternalNotes,string varShowTitle,string varDisplayUrl,int varIPicWidth,int varIPicHeight,string varTopText,string varMidText,bool varBDisplayRichText,bool varBHideAutoGenerated,string varBotText,bool varBOverrideActBilling,string varActBilling,bool varBAllowFacebookLike,DateTime varDtStamp,Guid varApplicationId,string varExternalTixUrl)
		{
			Show item = new Show();
			
			item.Name = varName;
			
			item.DtAnnounceDate = varDtAnnounceDate;
			
			item.DtDateOnSale = varDtDateOnSale;
			
			item.BActive = varBActive;
			
			item.BSoldOut = varBSoldOut;
			
			item.StatusText = varStatusText;
			
			item.VenuePreText = varVenuePreText;
			
			item.TVenueId = varTVenueId;
			
			item.VenuePostText = varVenuePostText;
			
			item.DisplayNotes = varDisplayNotes;
			
			item.InternalNotes = varInternalNotes;
			
			item.ShowTitle = varShowTitle;
			
			item.DisplayUrl = varDisplayUrl;
			
			item.IPicWidth = varIPicWidth;
			
			item.IPicHeight = varIPicHeight;
			
			item.TopText = varTopText;
			
			item.MidText = varMidText;
			
			item.BDisplayRichText = varBDisplayRichText;
			
			item.BHideAutoGenerated = varBHideAutoGenerated;
			
			item.BotText = varBotText;
			
			item.BOverrideActBilling = varBOverrideActBilling;
			
			item.ActBilling = varActBilling;
			
			item.BAllowFacebookLike = varBAllowFacebookLike;
			
			item.DtStamp = varDtStamp;
			
			item.ApplicationId = varApplicationId;
			
			item.ExternalTixUrl = varExternalTixUrl;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varId,string varName,DateTime? varDtAnnounceDate,DateTime? varDtDateOnSale,bool varBActive,bool varBSoldOut,string varStatusText,string varVenuePreText,int varTVenueId,string varVenuePostText,string varDisplayNotes,string varInternalNotes,string varShowTitle,string varDisplayUrl,int varIPicWidth,int varIPicHeight,string varTopText,string varMidText,bool varBDisplayRichText,bool varBHideAutoGenerated,string varBotText,bool varBOverrideActBilling,string varActBilling,bool varBAllowFacebookLike,DateTime varDtStamp,Guid varApplicationId,string varExternalTixUrl)
		{
			Show item = new Show();
			
				item.Id = varId;
			
				item.Name = varName;
			
				item.DtAnnounceDate = varDtAnnounceDate;
			
				item.DtDateOnSale = varDtDateOnSale;
			
				item.BActive = varBActive;
			
				item.BSoldOut = varBSoldOut;
			
				item.StatusText = varStatusText;
			
				item.VenuePreText = varVenuePreText;
			
				item.TVenueId = varTVenueId;
			
				item.VenuePostText = varVenuePostText;
			
				item.DisplayNotes = varDisplayNotes;
			
				item.InternalNotes = varInternalNotes;
			
				item.ShowTitle = varShowTitle;
			
				item.DisplayUrl = varDisplayUrl;
			
				item.IPicWidth = varIPicWidth;
			
				item.IPicHeight = varIPicHeight;
			
				item.TopText = varTopText;
			
				item.MidText = varMidText;
			
				item.BDisplayRichText = varBDisplayRichText;
			
				item.BHideAutoGenerated = varBHideAutoGenerated;
			
				item.BotText = varBotText;
			
				item.BOverrideActBilling = varBOverrideActBilling;
			
				item.ActBilling = varActBilling;
			
				item.BAllowFacebookLike = varBAllowFacebookLike;
			
				item.DtStamp = varDtStamp;
			
				item.ApplicationId = varApplicationId;
			
				item.ExternalTixUrl = varExternalTixUrl;
			
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
        
        
        
        public static TableSchema.TableColumn NameColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn DtAnnounceDateColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn DtDateOnSaleColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn BActiveColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn BSoldOutColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn StatusTextColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn VenuePreTextColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn TVenueIdColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn VenuePostTextColumn
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        public static TableSchema.TableColumn DisplayNotesColumn
        {
            get { return Schema.Columns[10]; }
        }
        
        
        
        public static TableSchema.TableColumn InternalNotesColumn
        {
            get { return Schema.Columns[11]; }
        }
        
        
        
        public static TableSchema.TableColumn ShowTitleColumn
        {
            get { return Schema.Columns[12]; }
        }
        
        
        
        public static TableSchema.TableColumn DisplayUrlColumn
        {
            get { return Schema.Columns[13]; }
        }
        
        
        
        public static TableSchema.TableColumn IPicWidthColumn
        {
            get { return Schema.Columns[14]; }
        }
        
        
        
        public static TableSchema.TableColumn IPicHeightColumn
        {
            get { return Schema.Columns[15]; }
        }
        
        
        
        public static TableSchema.TableColumn TopTextColumn
        {
            get { return Schema.Columns[16]; }
        }
        
        
        
        public static TableSchema.TableColumn MidTextColumn
        {
            get { return Schema.Columns[17]; }
        }
        
        
        
        public static TableSchema.TableColumn BDisplayRichTextColumn
        {
            get { return Schema.Columns[18]; }
        }
        
        
        
        public static TableSchema.TableColumn BHideAutoGeneratedColumn
        {
            get { return Schema.Columns[19]; }
        }
        
        
        
        public static TableSchema.TableColumn BotTextColumn
        {
            get { return Schema.Columns[20]; }
        }
        
        
        
        public static TableSchema.TableColumn BOverrideActBillingColumn
        {
            get { return Schema.Columns[21]; }
        }
        
        
        
        public static TableSchema.TableColumn ActBillingColumn
        {
            get { return Schema.Columns[22]; }
        }
        
        
        
        public static TableSchema.TableColumn BAllowFacebookLikeColumn
        {
            get { return Schema.Columns[23]; }
        }
        
        
        
        public static TableSchema.TableColumn DtStampColumn
        {
            get { return Schema.Columns[24]; }
        }
        
        
        
        public static TableSchema.TableColumn ApplicationIdColumn
        {
            get { return Schema.Columns[25]; }
        }
        
        
        
        public static TableSchema.TableColumn ExternalTixUrlColumn
        {
            get { return Schema.Columns[26]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Id = @"Id";
			 public static string Name = @"Name";
			 public static string DtAnnounceDate = @"dtAnnounceDate";
			 public static string DtDateOnSale = @"dtDateOnSale";
			 public static string BActive = @"bActive";
			 public static string BSoldOut = @"bSoldOut";
			 public static string StatusText = @"StatusText";
			 public static string VenuePreText = @"VenuePreText";
			 public static string TVenueId = @"TVenueId";
			 public static string VenuePostText = @"VenuePostText";
			 public static string DisplayNotes = @"DisplayNotes";
			 public static string InternalNotes = @"InternalNotes";
			 public static string ShowTitle = @"ShowTitle";
			 public static string DisplayUrl = @"DisplayUrl";
			 public static string IPicWidth = @"iPicWidth";
			 public static string IPicHeight = @"iPicHeight";
			 public static string TopText = @"TopText";
			 public static string MidText = @"MidText";
			 public static string BDisplayRichText = @"bDisplayRichText";
			 public static string BHideAutoGenerated = @"bHideAutoGenerated";
			 public static string BotText = @"BotText";
			 public static string BOverrideActBilling = @"bOverrideActBilling";
			 public static string ActBilling = @"ActBilling";
			 public static string BAllowFacebookLike = @"bAllowFacebookLike";
			 public static string DtStamp = @"dtStamp";
			 public static string ApplicationId = @"ApplicationId";
			 public static string ExternalTixUrl = @"ExternalTixUrl";
						
		}
		#endregion
		
		#region Update PK Collections
		
        public void SetPKValues()
        {
                if (colHeaderImageRecords != null)
                {
                    foreach (Wcss.HeaderImage item in colHeaderImageRecords)
                    {
                        if (item.TShowId != Id)
                        {
                            item.TShowId = Id;
                        }
                    }
               }
		
                if (colJShowPromoterRecords != null)
                {
                    foreach (Wcss.JShowPromoter item in colJShowPromoterRecords)
                    {
                        if (item.TShowId != Id)
                        {
                            item.TShowId = Id;
                        }
                    }
               }
		
                if (colShowDateRecords != null)
                {
                    foreach (Wcss.ShowDate item in colShowDateRecords)
                    {
                        if (item.TShowId != Id)
                        {
                            item.TShowId = Id;
                        }
                    }
               }
		
                if (colShowLinkRecords != null)
                {
                    foreach (Wcss.ShowLink item in colShowLinkRecords)
                    {
                        if (item.TShowId != Id)
                        {
                            item.TShowId = Id;
                        }
                    }
               }
		}
        #endregion
    
        #region Deep Save
		
        public void DeepSave()
        {
            Save();
            
                if (colHeaderImageRecords != null)
                {
                    colHeaderImageRecords.SaveAll();
               }
		
                if (colJShowPromoterRecords != null)
                {
                    colJShowPromoterRecords.SaveAll();
               }
		
                if (colShowDateRecords != null)
                {
                    colShowDateRecords.SaveAll();
               }
		
                if (colShowLinkRecords != null)
                {
                    colShowLinkRecords.SaveAll();
               }
		}
        #endregion
	}
}
