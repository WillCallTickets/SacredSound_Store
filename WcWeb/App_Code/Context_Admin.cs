using System;
using System.Collections.Generic;

using Wcss;

namespace WillCallWeb
{	
	/// <summary>
	/// Summary description for SessionContext.
	/// </summary>
	public class AdminContext : WebContext
	{
		#region BLOCKING

		private List<string> _blockUIProcessingMessages = null;
		private List<string> BlockUI_ProcessingMessages
		{
			get
			{
				if (_blockUIProcessingMessages == null)
				{
					_blockUIProcessingMessages = new List<string>();
					_blockUIProcessingMessages.Add("...Processing Request...");
					_blockUIProcessingMessages.Add("...Granting Request...");
					_blockUIProcessingMessages.Add("...Brewing...");
					_blockUIProcessingMessages.Add("...Eating The Donuts...");
					_blockUIProcessingMessages.Add("...Making The Donuts...");
					_blockUIProcessingMessages.Add("...Hard Hat Area...");
					_blockUIProcessingMessages.Add("...Exhausting Resources...");
					_blockUIProcessingMessages.Add("...Clearing Cobwebs...");
					_blockUIProcessingMessages.Add("...Making My Way Back To You Babe...");
					_blockUIProcessingMessages.Add("...Taking Time Out...");
					_blockUIProcessingMessages.Add("...Shaking That Money Maker...");
					_blockUIProcessingMessages.Add("...Please Wait...");
					_blockUIProcessingMessages.Add("...Calculating PI...");
					_blockUIProcessingMessages.Add("...Slowing Global Warming...");
					_blockUIProcessingMessages.Add("...Pasteurizing...");
					_blockUIProcessingMessages.Add("...Distilling...");
                    _blockUIProcessingMessages.Add("...Virtualizing...");
                    _blockUIProcessingMessages.Add("...Minding Ps and Qs...");
                    _blockUIProcessingMessages.Add("...Distilling...");
					_blockUIProcessingMessages.Add("...Putting A Bead On...");
					_blockUIProcessingMessages.Add("...Creating Matrices...");
					_blockUIProcessingMessages.Add("...Opening Vortex...");
					_blockUIProcessingMessages.Add("...Placing A Hold On...");
					_blockUIProcessingMessages.Add("...Engaging Warp Engines...");
					_blockUIProcessingMessages.Add("...Grooving With A Pict...");
					_blockUIProcessingMessages.Add("...Eating more pork...");
					_blockUIProcessingMessages.Add("...Shaking off the heebie jeebies...");
					_blockUIProcessingMessages.Add("...Looking for Mulva...");
					_blockUIProcessingMessages.Add("...Biting off more than I can chew...");
					_blockUIProcessingMessages.Add("...Four score and seven...");
					_blockUIProcessingMessages.Add("...The humanity...");
                    _blockUIProcessingMessages.Add("...Gunter glieben glauchen globen...");
                    _blockUIProcessingMessages.Add("...For those about to rock....We Salute You...");
                    _blockUIProcessingMessages.Add("...I, Claudius...");
                    _blockUIProcessingMessages.Add("...Al...Al, Al, Al...Al...");
				}

				return _blockUIProcessingMessages;
			}
		}
		private string GetRandomProcessingMessage
		{
			get
			{
				return BlockUI_ProcessingMessages[
					Utils.ParseHelper.GenerateRandomNumber(0, BlockUI_ProcessingMessages.Count - 1)];//make it apply to zero index
			}
		}
		
		private readonly string BLOCK_UI_DEFAULTS = "message:'<h1><MESSAGE><img src=\"/Images/loaderCirc.gif\" alt=\"\" /></h1>',css:{top:'120px','white-space':'nowrap',width:'auto',padding:'12px 24px'},centerY:false,cursor:'default' ";
		public void RegisterJQueryScript(System.Web.UI.Control control, string jQueryScript, bool includeCorners)
		{
			//return;
			RegisterJQueryScript_BlockUI_AjaxMethod(control, null, includeCorners, jQueryScript);
		}
		public void RegisterJQueryScript_BlockUI_AjaxMethod(System.Web.UI.Control control, string blockedUiElement, bool includeCorners)
		{
			//return;
			RegisterJQueryScript_BlockUI_AjaxMethod(control, blockedUiElement, includeCorners, null);
		}
		public void RegisterJQueryScript_BlockUI_AjaxMethod(System.Web.UI.Control control, string blockedUiElement, bool includeCorners, string additionalScript)
		{
			//return;
			string script = string.Empty;

			if(blockedUiElement != null && blockedUiElement.Trim().Length > 0)
				script = string.Format("$().ajaxStart($('{0}').block({{ {1} }})).ajaxStop($('{0}').unblock()); ", 
					blockedUiElement, BLOCK_UI_DEFAULTS.Replace("<MESSAGE>",GetRandomProcessingMessage));

			//make sure that our edit tables have a div surrounding them
            //if (includeCorners)
            //    script += string.Format(" $('TABLE.edttbl').wrap('<div class=\"rndedttbl rounded\"></div>'); ");

			if (additionalScript != null && additionalScript.Trim().Length > 0)
				script += additionalScript;

			if (script.Trim().Length > 0)
				System.Web.UI.ScriptManager.RegisterStartupScript(control, control.GetType(),
					Guid.NewGuid().ToString(), " ;" + script, true);
		}
		/// <summary>
		/// customBlock should include parentheses so that mult objects can be selected. ie: ('.someDivs').add('.someotherclass')
		/// </summary>
		/// <param name="control"></param>
		/// <param name="blockedUiElement"></param>
		/// <param name="customBlock"></param>
		public void RegisterJQueryScript_BlockUIEvents(System.Web.UI.Control control, string blockedUiElement,
			string clickBlockElements, string changeBlockElements)
		{
			//return;
			//blks += " $('#calnav .prev').click(function() { $('#calendarcontainer').block({message: '<h1>...Loading shows...</h1>' }); }); ";
			//string blks = "$('#calnav SELECT').change(function() { $('#calendarcontainer').block({message: '<h1>...Loading shows...</h1>' }); }); ";

			string script = string.Empty;

			if (clickBlockElements != null && clickBlockElements.Trim().Length > 0)
				script += string.Format(" ${0}.click(function() {{ $('{1}').block( {{ {2} }}); }}); ", clickBlockElements, 
					blockedUiElement, BLOCK_UI_DEFAULTS.Replace("<MESSAGE>", GetRandomProcessingMessage));

			if (changeBlockElements != null && changeBlockElements.Trim().Length > 0)
				script += string.Format(" ${0}.change(function() {{ $('{1}').block( {{ {2} }}); }}); ", changeBlockElements,
					blockedUiElement, BLOCK_UI_DEFAULTS.Replace("<MESSAGE>", GetRandomProcessingMessage));

			if (script.Trim().Length > 0)
				System.Web.UI.ScriptManager.RegisterStartupScript(control, control.GetType(),
					Guid.NewGuid().ToString(), " ;" + script, true);
		}
		public void UnblockUI(System.Web.UI.Control control, string blockedUiElement)
		{
			//return;
			System.Web.UI.ScriptManager.RegisterStartupScript(control, control.GetType(), 
				Guid.NewGuid().ToString(),
				string.Format(" ; $('{0}').unblock(); ", blockedUiElement),
				true);
		}

		#endregion

		public void RefreshMailer()
		{
			int idx = (this.CurrentMailer != null) ? this.CurrentMailer.Id : 0;
			int templateId = (this.CurrentMailer != null) ? this.CurrentMailer.TMailerTemplateId : (this.CurrentMailerTemplate != null) ? this.CurrentMailerTemplate.Id: 0;

			this.CurrentMailer = null;
			this.CurrentMailerTemplate = null;

			if (idx > 0)
			{
				this.CurrentMailer = Mailer.FetchByID(idx);
				if(this.CurrentMailer != null)
					this.CurrentMailerTemplate = MailerTemplate.FetchByID(this.CurrentMailer.TMailerTemplateId);
			}
			else if (templateId > 0)
				this.CurrentMailerTemplate = MailerTemplate.FetchByID(templateId);
		}

		/// <summary>
		/// Guarantees that we have or haven't attempted authorization at some point. When null - we have not processed.
		/// Values are too sensitive for cookie storage
		/// </summary>
		public string TransactionProcessingVariables { get { return (string)Session["TransactionProcessingVariables"]; } set { Session["TransactionProcessingVariables"] = value; } }
		/// <summary>
		/// Sets the order processing variables - follow the parameter list
		/// </summary>
		public void SetTransactionVariables(int invoiceId, decimal amount, DateTime transactionTime)
		{
			this.TransactionProcessingVariables = string.Format("{0}~{1}~{2}", invoiceId.ToString(), amount.ToString(), 
				transactionTime.ToString("MM/dd/yyyy hh:mmtt"));
		}

		public AdminContext() : base() 
		{
			//string g = ActImageFolder;
		}

		public void PublishSite()
		{
            //write to dependency file to update api
            _Error.LogPublishEvent(DateTime.Now, _Enums.PublishEvent.Publish, "somebody did it");

			base.PublishFromAdmin();
		}

		#region Cookies

        public int OrdinalTabCookie
        {
            get
            {
                return this.Scookie._ordinalTabCookie;
            }
            set
            {
                Scookie._ordinalTabCookie = value;
            }
        }
		public int CurrentDonationYearContext
		{
			get
			{
				return this.Scookie._dyid;
			}
			set
			{
				Scookie._dyid = value;
			}
		}
		public int CurrentSubscriptionEmailId
		{
			get
			{
				return this.Scookie._seid;
			}
			set
			{
				Scookie._seid = value;
			}
		}
		public int AdminMerchListingContext
		{
			get
			{
				return this.Scookie._admMlc;
			}
			set
			{
				Scookie._admMlc = value;
			}
		}
		////default page size for gridviews, etc
		public int adminPageSize
		{
			get
			{
				return (System.Web.HttpContext.Current.Profile as ProfileCommon).Preferences.PageSize;
			}
			set
			{
				(System.Web.HttpContext.Current.Profile as ProfileCommon).Preferences.PageSize = value;
			}
		}
		public int vwMrcId
		{
			get
			{
				return this.Scookie._vwMrc;
			}
			set
			{
				Scookie._vwMrc = value;
			}
		}
		public int vwDwnId
		{
			get
			{
				return this.Scookie._vwDwn;
			}
			set
			{
				Scookie._vwDwn = value;
			}
		}
		public int vwTicketId
		{
			get
			{
				return this.Scookie._vwTik;
			}
			set
			{
				Scookie._vwTik = value;
			}
		}
		public int vwDateId
		{
			get
			{
				return this.Scookie._vwDte;
			}
			set
			{
				Scookie._vwDte = value;
			}
		}
		public int CurrentActId
		{
			get
			{
				return this.Scookie._acid;
			}
			set
			{
				Scookie._acid = value;
			}
		}
		public int CurrentPromoterId
		{
			get
			{
				return this.Scookie._prid;
			}
			set
			{
				Scookie._prid = value;
			}
		}
		public int CurrentCharitableOrgId
		{
			get
			{
				return this.Scookie._chid;
			}
			set
			{
				Scookie._chid = value;
			}
		}
		public int CurrentCharitableListingId
		{
			get
			{
				return this.Scookie._clid;
			}
			set
			{
				Scookie._clid = value;
			}
		}
		public int CurrentShowLinkId
		{
			get
			{
				return this.Scookie._slid;
			}
			set
			{
				Scookie._slid = value;
			}
		}
		public int CurrentTuneId
		{
			get
			{
				return this.Scookie._tnid;
			}
			set
			{
				Scookie._tnid = value;
			}
		}
		public int CurrentVenueId
		{
			get
			{
				return this.Scookie._vnid;
			}
			set
			{
				Scookie._vnid = value;
			}
		}
		#endregion

		#region events
        public delegate void CurrentShowRecordChosenEvent(object sender, EventArgs e);
        public static event CurrentShowRecordChosenEvent CurrentShowRecordChosen;
        public static void OnCurrentShowRecordChosen(object sender)
        {
            if (CurrentShowRecordChosen != null)
            {
                CurrentShowRecordChosen(sender, EventArgs.Empty);
            }
        }
        public void ResetCurrentShowRecord()
        {
            if (CurrentShowRecord != null)
            {
                SetCurrentShowRecord(CurrentShowRecord.Id);
            }
        }

        //public class OrdinalListChangedEventArgs : EventArgs
        //{
        //    protected Wcss._Enums.OrdinalContext _ordinalContext;

        //    //Alt Constructor
        //    public OrdinalListChangedEventArgs(string context)
        //    {
        //        _ordinalContext = (Wcss._Enums.OrdinalContext)Enum.Parse(typeof(Wcss._Enums.OrdinalContext), context, true);
        //    }
        //    public OrdinalListChangedEventArgs(Wcss._Enums.OrdinalContext ordinalContext)
        //    {
        //        _ordinalContext = ordinalContext;
        //    }

        //    public Wcss._Enums.OrdinalContext OrdinalContext { get { return _ordinalContext; } }
        //}
        //public delegate void OrdinalListChangedEvent(object sender, OrdinalListChangedEventArgs e);
        //public static event OrdinalListChangedEvent OrdinalListChanged;
        //public static void OnOrdinalListChanged(object sender, string context)
        //{
        //    if (OrdinalListChanged != null) { OrdinalListChanged(sender, new OrdinalListChangedEventArgs(context)); }
        //}
        //public static void OnOrdinalListChanged(object sender, Wcss._Enums.OrdinalContext context)
        //{
        //    if (OrdinalListChanged != null) { OrdinalListChanged(sender, new OrdinalListChangedEventArgs(context)); }
        //}
		#endregion

		#region Session Objects

        //public _Enums.OrdinalContext CurrentOrdinalContext
        //{
        //    get
        //    {
        //        return _Enums.OrdinalContext.merchdivision;

        //        //if (Session["Admin_CurrentOrdinalContext"] == null)
        //        //    Session.Add("Admin_CurrentOrdinalContext", _Enums.OrdinalContext.merchdivision.ToString());

        //        //return (_Enums.OrdinalContext)Enum.Parse(typeof(_Enums.OrdinalContext), Session["Admin_CurrentOrdinalContext"].ToString(), true);
        //    }
        //    set
        //    {
        //        // Session.Remove("Admin_CurrentOrdinalContext");

        //        //Session["Admin_CurrentDisplayList"] = value.ToString();

        //        OnOrdinalListChanged(this, value);
        //    }
        //}

		public List<string> CurrentDisplayList
		{
			get
			{
				if (Session["Admin_CurrentDisplayList"] != null)
					return (List<string>)Session["Admin_CurrentDisplayList"];

				return null;
			}
			set
			{
				Session.Remove("Admin_CurrentDisplayList");

				if (value != null)
					Session.Add("Admin_CurrentDisplayList", value);
			}
		}

        public ProductAccessCollection ProductAccessCampaigns
        {
            get
            {
                if (Cache["Admin_ProductAccessCampaigns"] == null)
                {
                    ProductAccessCollection coll = new ProductAccessCollection();                    
                    coll.Where(ProductAccess.Columns.ApplicationId, _Config.APPLICATION_ID).OrderByAsc("IDisplayOrder").Load();

                    Cache.Insert("Admin_ProductAccessCampaigns", coll, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(20));
                }

                return (ProductAccessCollection)Cache["Admin_ProductAccessCampaigns"];
            }
        }

        public void SetCurrentAccessCampaign(int idx)
        {
            if (idx == 0)
                Session.Remove("Admin_CurrentAccessCampaign");
            else
            {
                ProductAccess pa = ProductAccessCampaigns.GetList().Find(delegate (ProductAccess match) { return (match.Id == idx); } );
                if(pa != null && pa.ApplicationId == Wcss._Config.APPLICATION_ID)
                    this.CurrentAccessCampaign = pa;
            }
        }
 
        public ProductAccess CurrentAccessCampaign
        {
            get
            {
                return (ProductAccess)Session["Admin_CurrentAccessCampaign"];
            }
            set
            {
                Session.Remove("Admin_CurrentAccessCampaign");

                if (value != null)
                    Session.Add("Admin_CurrentAccessCampaign", value);
            }
        }
		public Wcss.QueryRow.ShippingFulfillment CurrentShippingFulfillment
		{
			get
			{
				return (Wcss.QueryRow.ShippingFulfillment)Session["Admin_CurrentShippingFulfillment"];
			}
			set
			{
				Session.Remove("Admin_CurrentShippingFulfillment");
				
				if(value != null)
					Session.Add("Admin_CurrentShippingFulfillment", value);
			}
		}
		public ShipmentBatch CurrentShipmentBatch
		{
			get
			{
				return (ShipmentBatch)Session["Admin_CurrentShipmentBatch"];
			}
			set
			{
				Session.Remove("Admin_CurrentShipmentBatch");
				
				if(value != null)
					Session.Add("Admin_CurrentShipmentBatch", value);
			}
		}
		public Wcss.QueryRow.Report_DatesTickets CurrentReport_DatesTickets
		{
			get
			{
				return (Wcss.QueryRow.Report_DatesTickets)Session["Admin_CurrentReport_DatesTickets"];
			}
			set
			{
				Session.Remove("Admin_CurrentReport_DatesTickets");

				if (value != null)
					Session.Add("Admin_CurrentReport_DatesTickets", value);
			}
		}
		public Admin.MailerTemplateCreation CurrentMailerTemplateCreation
		{
			get
			{
				return (Admin.MailerTemplateCreation)Session["Admin_CurrentMailerTemplateCreation"];
			}
			set
			{
				Session.Remove("Admin_CurrentMailerTemplateCreation");

				if (value != null)
					Session.Add("Admin_CurrentMailerTemplateCreation", value);
			}
		}
		public EmailLetter CurrentEmailLetter
		{
			get
			{
				return (EmailLetter)Session["Admin_CurrentEmailLetter"];
			}
			set
			{
				Session.Remove("Admin_CurrentEmailLetter");

				if (value != null)
					Session.Add("Admin_CurrentEmailLetter", value);
			}
		}
		public Mailer CurrentMailer
		{
			get
			{
				return (Mailer)Session["Admin_CurrentMailer"];
			}
			set
			{
				Session.Remove("Admin_CurrentMailer");
				
				if(value != null)
					Session.Add("Admin_CurrentMailer", value);
			}
		}
		public MailerTemplate CurrentMailerTemplate
		{
			get
			{
				return (MailerTemplate)Session["Admin_CurrentMailerTemplate"];
			}
			set
			{
				Session.Remove("Admin_CurrentMailerTemplate");
				
				if(value != null)
					Session.Add("Admin_CurrentMailerTemplate", value);
			}
		}

		/// <summary>
		/// This will update the session showId and will also fire an event indicating that the show record has changed
		/// </summary>
		public Show SetCurrentShowRecord(int idx)
		{
			idx = SetCurrentShowId(idx);

			//this is not redundant - enforces a refresh
			if (Session["Admin_CurrentShow"] != null)
				Session.Remove("Admin_CurrentShow");

			//reset linked objects
			this.CurrentShowLinkId = 0;
			this.CurrentTuneId = 0;

			if (idx == 0)
				return null;
				
			Show s = new Show(idx);

			if(s != null && s.ApplicationId == _Config.APPLICATION_ID)
				Session.Add("Admin_CurrentShow", s);

			return (Show)Session["Admin_CurrentShow"];
		}
		public Show CurrentShowRecord
		{
			get
			{
				return (Show)Session["Admin_CurrentShow"];
			}
		}
		/// <summary>
		/// This will update the session showId only - DOES NOT fire an event to indicate the show record has changed
		/// </summary>
		private int SetCurrentShowId(int idx)
		{
			if (idx == 0)// && Session["Admin_CurrentShowId"] != null)
			{
				Session.Remove("Admin_CurrentShowId");
				return idx;
			}
			//..if the idx has, in fact, changed
			else if (Session["Admin_CurrentShowId"] != null && (int)Session["Admin_CurrentShowId"] != idx)
			{
				Session.Remove("Admin_CurrentShowId");
				Session.Add("Admin_CurrentShowId", idx);
			}
			else if (Session["Admin_CurrentShowId"] == null && idx > 0)
				Session.Add("Admin_CurrentShowId", idx);

			return (int)Session["Admin_CurrentShowId"];

		}
		public DateTime ShowChooserStartDate
		{
			get
			{
				if (Session["Admin_ChooserStartDate"] == null)
					Session["Admin_ChooserStartDate"] = DateTime.Parse(DateTime.Now.ToString("MM/1/yyyy"));

				return (DateTime)Session["Admin_ChooserStartDate"];
			}
			set
			{
				Session["Admin_ChooserStartDate"] = value;
			}
		}
        public void RefreshCurrentShowRecord()
        {
            if (this.CurrentShowRecord != null)
            {
                int idx = this.CurrentShowRecord.Id;
                SetCurrentShowRecord(idx);
            }
        }

		#region Current Charity Listing

		public CharitableListing SetCurrentCharitableListingRecord(int idx)
		{
			idx = SetCurrentCharitableListingId(idx);

			//this is not redundant - enforces a refresh
			if (Session["Admin_CurrentCharitableListing"] != null)
				Session.Remove("Admin_CurrentCharitableListing");

			if (idx == 0)
				return null;

			CharitableListing s = new CharitableListing(idx);

			if (s != null && s.ApplicationId == _Config.APPLICATION_ID)
				Session.Add("Admin_CurrentCharitableListing", s);

			return (CharitableListing)Session["Admin_CurrentCharitableListing"];
		}
		public CharitableListing CurrentCharitableListingRecord
		{
			get
			{
				return (CharitableListing)Session["Admin_CurrentCharitableListing"];
			}
		}

		private int SetCurrentCharitableListingId(int idx)
		{
			if (idx == 0)// && Session["Admin_CurrentShowId"] != null)
			{
				Session.Remove("Admin_CurrentCharitableListingId");
				return idx;
			}
			//..if the idx has, in fact, changed
			else if (Session["Admin_CurrentCharitableListingId"] != null && (int)Session["Admin_CurrentCharitableListingId"] != idx)
			{
				Session.Remove("Admin_CurrentCharitableListingId");
				Session.Add("Admin_CurrentCharitableListingId", idx);
			}
			else if (Session["Admin_CurrentCharitableListingId"] == null && idx > 0)
				Session.Add("Admin_CurrentCharitableListingId", idx);

			return (int)Session["Admin_CurrentCharitableListingId"];
		}

		#endregion

		//try to get from cached collection - however it could ref a object not in current collection (not active, in past, etc)
		public Merch SetCurrentMerchRecord(int idx)
		{
			//reset admin var
			this.vwMrcId = idx;

			//try to find the show in the current list
			//if not found then do a new show - retrieve from db
			Merch _merch = null;
			Session.Remove("Admin_CurrentMerch");

			if (idx > 0)
			{
				_merch = (Merch)CurrentMerchListing.Find(idx);

				//TODO: load from archives??? - what to do with non-exxistent item
				if (_merch == null)
					_merch = Merch.FetchByID(idx);

				if(_merch != null && _merch.ApplicationId == _Config.APPLICATION_ID)
					Session.Add("Admin_CurrentMerch", _merch);
			}

			return _merch;
		}
		public Merch CurrentMerchRecord
		{
			get
			{
				return (Merch)Session["Admin_CurrentMerch"];
			}
		}
		public void RefreshCurrentMerchRecord()
		{
			if (this.CurrentMerchRecord != null)
			{
				int idx = this.CurrentMerchRecord.Id;
                //Session.Remove("Admin_CurrentMerch");
				SetCurrentMerchRecord(idx);
			}
		}

        public Download CurrentDownloadRecord
		{
			get
			{
				return (Download)Session["Admin_CurrentDownload"];
			}
		}
		public void RefreshCurrentDownloadRecord()
		{
			if (this.CurrentDownloadRecord != null)
			{
				int idx = this.CurrentDownloadRecord.Id;
				SetCurrentDownloadRecord(idx);
			}
		}
		//try to get from cached collection - however it could ref a object not in current collection (not active, in past, etc)
		public Download SetCurrentDownloadRecord(int idx)
		{
			//reset admin var
			this.vwDwnId = idx;

			//try to find the show in the current list
			//if not found then do a new show - retrieve from db
			Download _download = null;
			Session.Remove("Admin_CurrentDownload");

			if (idx > 0)
			{
				_download = Download.FetchByID(idx);

				if (_download != null && _download.ApplicationId == _Config.APPLICATION_ID)
					Session.Add("Admin_CurrentDownload", _download);
			}

			return _download;
		}
		public void Clear_CurrentDownloadListing()
		{
			Cache.Remove("Admin_CurrentDownload");
		}

		public Invoice SetCurrentInvoiceRecord(int idx)
		{
			Invoice _invoice = null;
			Session.Remove("Admin_CurrentInvoice");

			if (idx > 0)
			{
				_invoice = Invoice.FetchByID(idx);

				if(_invoice != null && _invoice.ApplicationId == _Config.APPLICATION_ID)
					Session.Add("Admin_CurrentInvoice", _invoice);
			}

			return _invoice;
		}
		public Invoice CurrentInvoiceRecord
		{
			get
			{
				return (Invoice)Session["Admin_CurrentInvoice"];
			}
		}

		public MerchCollection CurrentMerchListing
		{
			get
			{
				if (Cache["Admin_Merch"] == null)
				{
					MerchCollection merch = new MerchCollection();
					merch.Where(Merch.Columns.ApplicationId, _Config.APPLICATION_ID).Load();

					Cache.Insert("Admin_Merch", merch, new System.Web.Caching.CacheDependency(System.Web.HttpContext.Current.Server.MapPath(string.Format("/{0}/DependencyFiles/MerchDepend.txt", _Config._VirtualResourceDir))),
						System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(20));

					Session.Remove("Admin_CurrentMerch");//make this reload as well
				}

				return (MerchCollection)Cache["Admin_Merch"];
			}
		}
		public void Clear_CurrentMerchListing()
		{
			Cache.Remove("Admin_Merch");
		}

		//always get entire list?
		public VenueCollection Venues
		{
			get
			{
				if (Cache["Admin_Venues"] == null)
				{
					VenueCollection coll = new VenueCollection();
					coll.Where(Venue.Columns.ApplicationId, _Config.APPLICATION_ID).OrderByAsc("Name").Load();

					Cache.Insert("Admin_Venues", coll, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(20));
				}

				return (VenueCollection)Cache["Admin_Venues"];
			}
		}
		public void Clear_Venues()
		{
			Cache.Remove("Admin_Venues");
		}

		public PromoterCollection Promoters
		{
			get
			{
				if (Cache["Admin_Promoters"] == null)
				{
					PromoterCollection coll = new PromoterCollection();
					coll.Where(Promoter.Columns.ApplicationId, _Config.APPLICATION_ID).OrderByAsc("NameRoot").Load();

					Cache.Insert("Admin_Promoters", coll, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(20));
				}

				return (PromoterCollection)Cache["Admin_Promoters"];
			}
		}
		public void Clear_Promoters()
		{
			Cache.Remove("Admin_Promoters");
		}
		public CharitableOrgCollection CharitableOrgs
		{
			get
			{
				if (Cache["Admin_CharitableOrgs"] == null)
				{
					CharitableOrgCollection coll = new CharitableOrgCollection();
					coll.Where(CharitableOrg.Columns.ApplicationId, _Config.APPLICATION_ID).OrderByAsc("NameRoot").Load();

					Cache.Insert("Admin_CharitableOrgs", coll, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(20));
				}

				return (CharitableOrgCollection)Cache["Admin_CharitableOrgs"];
			}
		}
		public void Clear_CharitableOrgs()
		{
			Cache.Remove("Admin_CharitableOrgs");
		}
		public CharitableListingCollection CharitableListings
		{
			get
			{
				if (Cache["Admin_CharitableListings"] == null)
				{
					CharitableListingCollection coll = new CharitableListingCollection();
					coll.Where(CharitableListing.Columns.ApplicationId, _Config.APPLICATION_ID).OrderByAsc("IDisplayOrder").Load();

					Cache.Insert("Admin_CharitableListings", coll, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(20));
				}

				return (CharitableListingCollection)Cache["Admin_CharitableListings"];
			}
		}
		public void Clear_CharitableListings()
		{
			Cache.Remove("Admin_CharitableListings");
		}

		#endregion
	}
}
