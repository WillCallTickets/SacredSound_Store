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
	/// Strongly-typed collection for the Lottery class.
	/// </summary>
    [Serializable]
	public partial class LotteryCollection : ActiveList<Lottery, LotteryCollection>
	{	   
		public LotteryCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>LotteryCollection</returns>
		public LotteryCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                Lottery o = this[i];
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
	/// This is an ActiveRecord class which wraps the Lottery table.
	/// </summary>
	[Serializable]
	public partial class Lottery : ActiveRecord<Lottery>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public Lottery()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public Lottery(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public Lottery(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public Lottery(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("Lottery", TableType.Table, DataService.GetInstance("WillCall"));
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
				colvarDtStamp.IsNullable = true;
				colvarDtStamp.IsPrimaryKey = false;
				colvarDtStamp.IsForeignKey = false;
				colvarDtStamp.IsReadOnly = false;
				
						colvarDtStamp.DefaultSetting = @"(getdate())";
				colvarDtStamp.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDtStamp);
				
				TableSchema.TableColumn colvarTShowTicketId = new TableSchema.TableColumn(schema);
				colvarTShowTicketId.ColumnName = "TShowTicketId";
				colvarTShowTicketId.DataType = DbType.Int32;
				colvarTShowTicketId.MaxLength = 0;
				colvarTShowTicketId.AutoIncrement = false;
				colvarTShowTicketId.IsNullable = false;
				colvarTShowTicketId.IsPrimaryKey = false;
				colvarTShowTicketId.IsForeignKey = true;
				colvarTShowTicketId.IsReadOnly = false;
				colvarTShowTicketId.DefaultSetting = @"";
				
					colvarTShowTicketId.ForeignKeyTableName = "ShowTicket";
				schema.Columns.Add(colvarTShowTicketId);
				
				TableSchema.TableColumn colvarTShowDateId = new TableSchema.TableColumn(schema);
				colvarTShowDateId.ColumnName = "TShowDateId";
				colvarTShowDateId.DataType = DbType.Int32;
				colvarTShowDateId.MaxLength = 0;
				colvarTShowDateId.AutoIncrement = false;
				colvarTShowDateId.IsNullable = false;
				colvarTShowDateId.IsPrimaryKey = false;
				colvarTShowDateId.IsForeignKey = false;
				colvarTShowDateId.IsReadOnly = false;
				colvarTShowDateId.DefaultSetting = @"";
				colvarTShowDateId.ForeignKeyTableName = "";
				schema.Columns.Add(colvarTShowDateId);
				
				TableSchema.TableColumn colvarTShowId = new TableSchema.TableColumn(schema);
				colvarTShowId.ColumnName = "TShowId";
				colvarTShowId.DataType = DbType.Int32;
				colvarTShowId.MaxLength = 0;
				colvarTShowId.AutoIncrement = false;
				colvarTShowId.IsNullable = false;
				colvarTShowId.IsPrimaryKey = false;
				colvarTShowId.IsForeignKey = false;
				colvarTShowId.IsReadOnly = false;
				colvarTShowId.DefaultSetting = @"";
				colvarTShowId.ForeignKeyTableName = "";
				schema.Columns.Add(colvarTShowId);
				
				TableSchema.TableColumn colvarBActiveSignup = new TableSchema.TableColumn(schema);
				colvarBActiveSignup.ColumnName = "bActiveSignup";
				colvarBActiveSignup.DataType = DbType.Boolean;
				colvarBActiveSignup.MaxLength = 0;
				colvarBActiveSignup.AutoIncrement = false;
				colvarBActiveSignup.IsNullable = false;
				colvarBActiveSignup.IsPrimaryKey = false;
				colvarBActiveSignup.IsForeignKey = false;
				colvarBActiveSignup.IsReadOnly = false;
				
						colvarBActiveSignup.DefaultSetting = @"((0))";
				colvarBActiveSignup.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBActiveSignup);
				
				TableSchema.TableColumn colvarDtSignupStart = new TableSchema.TableColumn(schema);
				colvarDtSignupStart.ColumnName = "dtSignupStart";
				colvarDtSignupStart.DataType = DbType.DateTime;
				colvarDtSignupStart.MaxLength = 0;
				colvarDtSignupStart.AutoIncrement = false;
				colvarDtSignupStart.IsNullable = true;
				colvarDtSignupStart.IsPrimaryKey = false;
				colvarDtSignupStart.IsForeignKey = false;
				colvarDtSignupStart.IsReadOnly = false;
				colvarDtSignupStart.DefaultSetting = @"";
				colvarDtSignupStart.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDtSignupStart);
				
				TableSchema.TableColumn colvarDtSignupEnd = new TableSchema.TableColumn(schema);
				colvarDtSignupEnd.ColumnName = "dtSignupEnd";
				colvarDtSignupEnd.DataType = DbType.DateTime;
				colvarDtSignupEnd.MaxLength = 0;
				colvarDtSignupEnd.AutoIncrement = false;
				colvarDtSignupEnd.IsNullable = true;
				colvarDtSignupEnd.IsPrimaryKey = false;
				colvarDtSignupEnd.IsForeignKey = false;
				colvarDtSignupEnd.IsReadOnly = false;
				colvarDtSignupEnd.DefaultSetting = @"";
				colvarDtSignupEnd.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDtSignupEnd);
				
				TableSchema.TableColumn colvarName = new TableSchema.TableColumn(schema);
				colvarName.ColumnName = "Name";
				colvarName.DataType = DbType.AnsiString;
				colvarName.MaxLength = 50;
				colvarName.AutoIncrement = false;
				colvarName.IsNullable = true;
				colvarName.IsPrimaryKey = false;
				colvarName.IsForeignKey = false;
				colvarName.IsReadOnly = false;
				colvarName.DefaultSetting = @"";
				colvarName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarName);
				
				TableSchema.TableColumn colvarDescription = new TableSchema.TableColumn(schema);
				colvarDescription.ColumnName = "Description";
				colvarDescription.DataType = DbType.AnsiString;
				colvarDescription.MaxLength = 500;
				colvarDescription.AutoIncrement = false;
				colvarDescription.IsNullable = true;
				colvarDescription.IsPrimaryKey = false;
				colvarDescription.IsForeignKey = false;
				colvarDescription.IsReadOnly = false;
				colvarDescription.DefaultSetting = @"";
				colvarDescription.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDescription);
				
				TableSchema.TableColumn colvarDisplayText = new TableSchema.TableColumn(schema);
				colvarDisplayText.ColumnName = "DisplayText";
				colvarDisplayText.DataType = DbType.AnsiString;
				colvarDisplayText.MaxLength = 256;
				colvarDisplayText.AutoIncrement = false;
				colvarDisplayText.IsNullable = true;
				colvarDisplayText.IsPrimaryKey = false;
				colvarDisplayText.IsForeignKey = false;
				colvarDisplayText.IsReadOnly = false;
				colvarDisplayText.DefaultSetting = @"";
				colvarDisplayText.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDisplayText);
				
				TableSchema.TableColumn colvarWriteup = new TableSchema.TableColumn(schema);
				colvarWriteup.ColumnName = "Writeup";
				colvarWriteup.DataType = DbType.AnsiString;
				colvarWriteup.MaxLength = -1;
				colvarWriteup.AutoIncrement = false;
				colvarWriteup.IsNullable = true;
				colvarWriteup.IsPrimaryKey = false;
				colvarWriteup.IsForeignKey = false;
				colvarWriteup.IsReadOnly = false;
				colvarWriteup.DefaultSetting = @"";
				colvarWriteup.ForeignKeyTableName = "";
				schema.Columns.Add(colvarWriteup);
				
				TableSchema.TableColumn colvarBActiveFulfillment = new TableSchema.TableColumn(schema);
				colvarBActiveFulfillment.ColumnName = "bActiveFulfillment";
				colvarBActiveFulfillment.DataType = DbType.Boolean;
				colvarBActiveFulfillment.MaxLength = 0;
				colvarBActiveFulfillment.AutoIncrement = false;
				colvarBActiveFulfillment.IsNullable = false;
				colvarBActiveFulfillment.IsPrimaryKey = false;
				colvarBActiveFulfillment.IsForeignKey = false;
				colvarBActiveFulfillment.IsReadOnly = false;
				
						colvarBActiveFulfillment.DefaultSetting = @"((0))";
				colvarBActiveFulfillment.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBActiveFulfillment);
				
				TableSchema.TableColumn colvarDtFulfillStart = new TableSchema.TableColumn(schema);
				colvarDtFulfillStart.ColumnName = "dtFulfillStart";
				colvarDtFulfillStart.DataType = DbType.DateTime;
				colvarDtFulfillStart.MaxLength = 0;
				colvarDtFulfillStart.AutoIncrement = false;
				colvarDtFulfillStart.IsNullable = true;
				colvarDtFulfillStart.IsPrimaryKey = false;
				colvarDtFulfillStart.IsForeignKey = false;
				colvarDtFulfillStart.IsReadOnly = false;
				colvarDtFulfillStart.DefaultSetting = @"";
				colvarDtFulfillStart.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDtFulfillStart);
				
				TableSchema.TableColumn colvarDtFulfillEnd = new TableSchema.TableColumn(schema);
				colvarDtFulfillEnd.ColumnName = "dtFulfillEnd";
				colvarDtFulfillEnd.DataType = DbType.DateTime;
				colvarDtFulfillEnd.MaxLength = 0;
				colvarDtFulfillEnd.AutoIncrement = false;
				colvarDtFulfillEnd.IsNullable = true;
				colvarDtFulfillEnd.IsPrimaryKey = false;
				colvarDtFulfillEnd.IsForeignKey = false;
				colvarDtFulfillEnd.IsReadOnly = false;
				colvarDtFulfillEnd.DefaultSetting = @"";
				colvarDtFulfillEnd.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDtFulfillEnd);
				
				TableSchema.TableColumn colvarIEstablishQty = new TableSchema.TableColumn(schema);
				colvarIEstablishQty.ColumnName = "iEstablishQty";
				colvarIEstablishQty.DataType = DbType.Int32;
				colvarIEstablishQty.MaxLength = 0;
				colvarIEstablishQty.AutoIncrement = false;
				colvarIEstablishQty.IsNullable = false;
				colvarIEstablishQty.IsPrimaryKey = false;
				colvarIEstablishQty.IsForeignKey = false;
				colvarIEstablishQty.IsReadOnly = false;
				
						colvarIEstablishQty.DefaultSetting = @"((0))";
				colvarIEstablishQty.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIEstablishQty);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["WillCall"].AddSchema("Lottery",schema);
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
		public DateTime? DtStamp 
		{
			get { return GetColumnValue<DateTime?>(Columns.DtStamp); }
			set { SetColumnValue(Columns.DtStamp, value); }
		}
		  
		[XmlAttribute("TShowTicketId")]
		[Bindable(true)]
		public int TShowTicketId 
		{
			get { return GetColumnValue<int>(Columns.TShowTicketId); }
			set { SetColumnValue(Columns.TShowTicketId, value); }
		}
		  
		[XmlAttribute("TShowDateId")]
		[Bindable(true)]
		public int TShowDateId 
		{
			get { return GetColumnValue<int>(Columns.TShowDateId); }
			set { SetColumnValue(Columns.TShowDateId, value); }
		}
		  
		[XmlAttribute("TShowId")]
		[Bindable(true)]
		public int TShowId 
		{
			get { return GetColumnValue<int>(Columns.TShowId); }
			set { SetColumnValue(Columns.TShowId, value); }
		}
		  
		[XmlAttribute("BActiveSignup")]
		[Bindable(true)]
		public bool BActiveSignup 
		{
			get { return GetColumnValue<bool>(Columns.BActiveSignup); }
			set { SetColumnValue(Columns.BActiveSignup, value); }
		}
		  
		[XmlAttribute("DtSignupStart")]
		[Bindable(true)]
		public DateTime? DtSignupStart 
		{
			get { return GetColumnValue<DateTime?>(Columns.DtSignupStart); }
			set { SetColumnValue(Columns.DtSignupStart, value); }
		}
		  
		[XmlAttribute("DtSignupEnd")]
		[Bindable(true)]
		public DateTime? DtSignupEnd 
		{
			get { return GetColumnValue<DateTime?>(Columns.DtSignupEnd); }
			set { SetColumnValue(Columns.DtSignupEnd, value); }
		}
		  
		[XmlAttribute("Name")]
		[Bindable(true)]
		public string Name 
		{
			get { return GetColumnValue<string>(Columns.Name); }
			set { SetColumnValue(Columns.Name, value); }
		}
		  
		[XmlAttribute("Description")]
		[Bindable(true)]
		public string Description 
		{
			get { return GetColumnValue<string>(Columns.Description); }
			set { SetColumnValue(Columns.Description, value); }
		}
		  
		[XmlAttribute("DisplayText")]
		[Bindable(true)]
		public string DisplayText 
		{
			get { return GetColumnValue<string>(Columns.DisplayText); }
			set { SetColumnValue(Columns.DisplayText, value); }
		}
		  
		[XmlAttribute("Writeup")]
		[Bindable(true)]
		public string Writeup 
		{
			get { return GetColumnValue<string>(Columns.Writeup); }
			set { SetColumnValue(Columns.Writeup, value); }
		}
		  
		[XmlAttribute("BActiveFulfillment")]
		[Bindable(true)]
		public bool BActiveFulfillment 
		{
			get { return GetColumnValue<bool>(Columns.BActiveFulfillment); }
			set { SetColumnValue(Columns.BActiveFulfillment, value); }
		}
		  
		[XmlAttribute("DtFulfillStart")]
		[Bindable(true)]
		public DateTime? DtFulfillStart 
		{
			get { return GetColumnValue<DateTime?>(Columns.DtFulfillStart); }
			set { SetColumnValue(Columns.DtFulfillStart, value); }
		}
		  
		[XmlAttribute("DtFulfillEnd")]
		[Bindable(true)]
		public DateTime? DtFulfillEnd 
		{
			get { return GetColumnValue<DateTime?>(Columns.DtFulfillEnd); }
			set { SetColumnValue(Columns.DtFulfillEnd, value); }
		}
		  
		[XmlAttribute("IEstablishQty")]
		[Bindable(true)]
		public int IEstablishQty 
		{
			get { return GetColumnValue<int>(Columns.IEstablishQty); }
			set { SetColumnValue(Columns.IEstablishQty, value); }
		}
		
		#endregion
		
		
		#region PrimaryKey Methods		
		
        protected override void SetPrimaryKey(object oValue)
        {
            base.SetPrimaryKey(oValue);
            
            SetPKValues();
        }
        
		
		private Wcss.LotteryRequestCollection colLotteryRequestRecords;
		public Wcss.LotteryRequestCollection LotteryRequestRecords()
		{
			if(colLotteryRequestRecords == null)
			{
				colLotteryRequestRecords = new Wcss.LotteryRequestCollection().Where(LotteryRequest.Columns.TLotteryId, Id).Load();
				colLotteryRequestRecords.ListChanged += new ListChangedEventHandler(colLotteryRequestRecords_ListChanged);
			}
			return colLotteryRequestRecords;
		}
				
		void colLotteryRequestRecords_ListChanged(object sender, ListChangedEventArgs e)
		{
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
		        // Set foreign key value
		        colLotteryRequestRecords[e.NewIndex].TLotteryId = Id;
				colLotteryRequestRecords.ListChanged += new ListChangedEventHandler(colLotteryRequestRecords_ListChanged);
            }
		}
		#endregion
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a ShowTicket ActiveRecord object related to this Lottery
		/// 
		/// </summary>
		private Wcss.ShowTicket ShowTicket
		{
			get { return Wcss.ShowTicket.FetchByID(this.TShowTicketId); }
			set { SetColumnValue("TShowTicketId", value.Id); }
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
                
                SetColumnValue("TShowTicketId", value.Id);
                _showticketrecord.CopyFrom(value);                
            }
		}
		
		
		#endregion
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(DateTime? varDtStamp,int varTShowTicketId,int varTShowDateId,int varTShowId,bool varBActiveSignup,DateTime? varDtSignupStart,DateTime? varDtSignupEnd,string varName,string varDescription,string varDisplayText,string varWriteup,bool varBActiveFulfillment,DateTime? varDtFulfillStart,DateTime? varDtFulfillEnd,int varIEstablishQty)
		{
			Lottery item = new Lottery();
			
			item.DtStamp = varDtStamp;
			
			item.TShowTicketId = varTShowTicketId;
			
			item.TShowDateId = varTShowDateId;
			
			item.TShowId = varTShowId;
			
			item.BActiveSignup = varBActiveSignup;
			
			item.DtSignupStart = varDtSignupStart;
			
			item.DtSignupEnd = varDtSignupEnd;
			
			item.Name = varName;
			
			item.Description = varDescription;
			
			item.DisplayText = varDisplayText;
			
			item.Writeup = varWriteup;
			
			item.BActiveFulfillment = varBActiveFulfillment;
			
			item.DtFulfillStart = varDtFulfillStart;
			
			item.DtFulfillEnd = varDtFulfillEnd;
			
			item.IEstablishQty = varIEstablishQty;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(int varId,DateTime? varDtStamp,int varTShowTicketId,int varTShowDateId,int varTShowId,bool varBActiveSignup,DateTime? varDtSignupStart,DateTime? varDtSignupEnd,string varName,string varDescription,string varDisplayText,string varWriteup,bool varBActiveFulfillment,DateTime? varDtFulfillStart,DateTime? varDtFulfillEnd,int varIEstablishQty)
		{
			Lottery item = new Lottery();
			
				item.Id = varId;
			
				item.DtStamp = varDtStamp;
			
				item.TShowTicketId = varTShowTicketId;
			
				item.TShowDateId = varTShowDateId;
			
				item.TShowId = varTShowId;
			
				item.BActiveSignup = varBActiveSignup;
			
				item.DtSignupStart = varDtSignupStart;
			
				item.DtSignupEnd = varDtSignupEnd;
			
				item.Name = varName;
			
				item.Description = varDescription;
			
				item.DisplayText = varDisplayText;
			
				item.Writeup = varWriteup;
			
				item.BActiveFulfillment = varBActiveFulfillment;
			
				item.DtFulfillStart = varDtFulfillStart;
			
				item.DtFulfillEnd = varDtFulfillEnd;
			
				item.IEstablishQty = varIEstablishQty;
			
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
        
        
        
        public static TableSchema.TableColumn TShowTicketIdColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn TShowDateIdColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn TShowIdColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn BActiveSignupColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn DtSignupStartColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn DtSignupEndColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn NameColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn DescriptionColumn
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        public static TableSchema.TableColumn DisplayTextColumn
        {
            get { return Schema.Columns[10]; }
        }
        
        
        
        public static TableSchema.TableColumn WriteupColumn
        {
            get { return Schema.Columns[11]; }
        }
        
        
        
        public static TableSchema.TableColumn BActiveFulfillmentColumn
        {
            get { return Schema.Columns[12]; }
        }
        
        
        
        public static TableSchema.TableColumn DtFulfillStartColumn
        {
            get { return Schema.Columns[13]; }
        }
        
        
        
        public static TableSchema.TableColumn DtFulfillEndColumn
        {
            get { return Schema.Columns[14]; }
        }
        
        
        
        public static TableSchema.TableColumn IEstablishQtyColumn
        {
            get { return Schema.Columns[15]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Id = @"Id";
			 public static string DtStamp = @"dtStamp";
			 public static string TShowTicketId = @"TShowTicketId";
			 public static string TShowDateId = @"TShowDateId";
			 public static string TShowId = @"TShowId";
			 public static string BActiveSignup = @"bActiveSignup";
			 public static string DtSignupStart = @"dtSignupStart";
			 public static string DtSignupEnd = @"dtSignupEnd";
			 public static string Name = @"Name";
			 public static string Description = @"Description";
			 public static string DisplayText = @"DisplayText";
			 public static string Writeup = @"Writeup";
			 public static string BActiveFulfillment = @"bActiveFulfillment";
			 public static string DtFulfillStart = @"dtFulfillStart";
			 public static string DtFulfillEnd = @"dtFulfillEnd";
			 public static string IEstablishQty = @"iEstablishQty";
						
		}
		#endregion
		
		#region Update PK Collections
		
        public void SetPKValues()
        {
                if (colLotteryRequestRecords != null)
                {
                    foreach (Wcss.LotteryRequest item in colLotteryRequestRecords)
                    {
                        if (item.TLotteryId != Id)
                        {
                            item.TLotteryId = Id;
                        }
                    }
               }
		}
        #endregion
    
        #region Deep Save
		
        public void DeepSave()
        {
            Save();
            
                if (colLotteryRequestRecords != null)
                {
                    colLotteryRequestRecords.SaveAll();
               }
		}
        #endregion
	}
}
