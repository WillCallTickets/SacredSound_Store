using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace Wcss
{
	/// <summary>
	/// Wraps the app.config file by exposing it's key/value pairs
	/// as static properties.
	/// </summary>
	/// <example>
	/// For example, instead of using
	/// <code>DataManager dm = new DataManager(ConfigurationManager.AppSettings["dsn"]);</code>
	/// the wrapped static method can be used:
	/// <code>DataManager dm = new DataManager(Config.Dsn);</code>
	/// </example>

	public partial class _Config
    {
        private static bool _useNewCart = false;
        /// <summary>
        /// Only use on dev server for now
        /// </summary>
        public static bool UseNewCart
        {
            get
            {
                return (_useNewCart && _Config._DomainName.IndexOf("local") != -1);
            }
        }


        public readonly static string _Merch_ListOfRequirement_Over18_Key = "merch_listof_requirement_over18";
        public static SiteConfig _Merch_Requires_18Over_Ack_List
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Admin.ToString().ToLower() &&
                            match.Name.ToLower() == _Merch_ListOfRequirement_Over18_Key && match.ValueX != null);
                    });
                if (config != null && config.Id > 0)
                {
                    return config;//.ValueX.Split(',').Select(int.Parse).ToList();
                }
                else
                {
                    config = _Config.AddNewConfig(_Enums.SiteConfigContext.Admin, _Enums.ConfigDataTypes._string, 2000,
                        _Merch_ListOfRequirement_Over18_Key,
                        @"Stores a comma separated list that holds the ids of merch items that need an 18+ verification. The list is managed in the admin page.",
                        "");

                    return config;
                }
            }
        }

        /// <summary>
        /// This does not refresh collections
        /// </summary>
        /// <param name="context"></param>
        /// <param name="datatype"></param>
        /// <param name="maxlength"></param>
        /// <param name="configname"></param>
        /// <param name="description"></param>
        /// <param name="defaultvalue"></param>
        /// <returns></returns>
        public static SiteConfig AddNewConfig(_Enums.SiteConfigContext context, _Enums.ConfigDataTypes datatype, int maxlength,
            string configname, string description, string defaultvalue)
        {
            SiteConfig config = new SiteConfig();
            config.ApplicationId = _Config.APPLICATION_ID;
            config.Context = context.ToString();
            config.DataType = datatype.ToString().TrimStart('_');
            config.DtStamp = DateTime.Now;
            config.MaxLength = maxlength;
            config.Name = configname;
            config.Description = description;
            config.ValueX = defaultvalue;
            config.Save();
            _Lookits.SiteConfigs.Add(config);
            return config;
        }

        public static readonly string SPACERIMAGEPATH = "/Images/spacer.gif";

        public static bool SqlServerIsAvailable()
        {
            try
            {
                //cmd.CommandTimeout = 1;
                string[] sps = SubSonic.DataService.Provider.GetTableNameList();

                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }

        //ErrorLogging
        public static bool _ErrorLogToDB { get { return bool.Parse(ConfigurationManager.AppSettings["erlg_ErrorLogToDB"]); } }
        public static bool _ErrorLogToEventViewer { get { return bool.Parse(ConfigurationManager.AppSettings["erlg_ErrorLogToEventViewer"]); } }

        protected _Config(){ }
        
        public static System.Collections.Generic.List<Wcss._Enums.PrintTicketItemType> _PrintTicketTypeList
        {
            get
            {
                System.Collections.Generic.List<Wcss._Enums.PrintTicketItemType> list = 
                    new System.Collections.Generic.List<_Enums.PrintTicketItemType>();

                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Default.ToString().ToLower() &&
                            match.Name.ToLower() == "printticket_typelist" && match.ValueX != null && match.ValueX.Trim().Length > 0);
                    });
                if (config != null && config.Id > 0)
                {
                    string[] types = config.ValueX.Trim().Split(',');
                    if (types.Length > 0)
                    {
                        for (int i = 0; i < types.Length; i++)
                        {
                            try
                            {
                                list.Add((_Enums.PrintTicketItemType)Enum.Parse(typeof(_Enums.PrintTicketItemType), types[i].Trim(), true));
                            }
                            catch (Exception ex)
                            {
                                _Error.LogException(ex);
                            }
                        }
                    }
                }

                return list;
            }
        }

        public static DateTime SHOWOFFSETDATE
        {
            get
            {
                return SHOWOFFSET_SET(DateTime.Now);//.AddHours(-_Config.DayTurnoverTime).Date;
            }
        }
        public static DateTime SHOWOFFSET_SET(DateTime date)
        {
            return date.AddHours(-_Config.DayTurnoverTime).Date;
        }

        public static bool ConfigTest()
        {
            bool result = true;

            TestFlow();
            TestImages();
            TestPageMsg();
            TestShip();
            TestDefault();
            TestEmail();
            TestService();
            TestAdmin();
            TestFacebook();

            return result;
        }

        /// <summary>
        /// the hour at which everything will turn over to the next day - add this many hours
        /// </summary>
        public const int DayTurnoverTime = 3;

        #region Alt display properties

        //this is now tied to site mode
        public static bool altDisplay
        {
            get
            {
                return (_Config._Site_Entity_Name.ToLower() != "sts9");
            }
        }
        public static string _GoogleAnalyticsId { get { return ConfigurationManager.AppSettings["stp_GoogleAnalyticsId"]; } }
        public static string _GoogleAPI_DeveloperKey { get { return ConfigurationManager.AppSettings["stp_GoogleAPI_DeveloperKey"]; } }

        #endregion

        #region XML

        private static string _shipTerms_Tickets = string.Empty;
        public static string _ShippingTerms_Tickets
        {
            get
            {
                if (_shipTerms_Tickets == null || _shipTerms_Tickets.Trim().Length == 0)
                {
                    try
                    {
                        string mappedPath = System.Web.HttpContext.Current.Server.MapPath(string.Format("/{0}/Xml/ShippingTerms_Tickets.xml", _Config._VirtualResourceDir));

                        if (!System.IO.File.Exists(mappedPath))
                            return string.Empty;

                        System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                        doc.XmlResolver = null;
                        doc.Load(mappedPath);

                        System.Xml.XmlElement xml = doc.DocumentElement;

                        if (xml != null)
                        {
                            _shipTerms_Tickets = xml.InnerXml.Replace("{0}", _Config._DomainName).Trim();
                        }
                    }
                    catch (Exception ex)
                    {
                        _Error.LogException(ex);
                        return string.Empty;
                    }
                }

                return _shipTerms_Tickets;
            }
        }
        private static string _giftTerms = string.Empty;
        /// <summary>
        /// format this for email
        /// </summary>
        public static string _GiftTerms
        {
            get
            {
                if (_giftTerms == null || _giftTerms.Trim().Length == 0)
                {
                    try
                    {
                        string mappedPath = System.Web.HttpContext.Current.Server.MapPath(string.Format("/{0}/Xml/Gift_Terms.xml", _Config._VirtualResourceDir));

                        if (!System.IO.File.Exists(mappedPath))
                            return string.Empty;

                        System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                        doc.XmlResolver = null;
                        doc.Load(mappedPath);

                        System.Xml.XmlElement xml = doc.DocumentElement;

                        if (xml != null)
                        {
                            _giftTerms = xml.InnerXml.Replace("{0}", _Config._DomainName).Trim();
                        }
                    }
                    catch (Exception ex)
                    {
                        _Error.LogException(ex);
                        return string.Empty;
                    }
                }

                return _giftTerms;
            }
        }
        private static string _giftRedemptionInstructions = string.Empty;
        /// <summary>
        /// format this for email
        /// </summary>
        public static string _GiftRedemptionInstructions
        {
            get
            {
                if (_giftRedemptionInstructions == null || _giftRedemptionInstructions.Trim().Length == 0)
                {
                    try
                    {
                        string mappedPath = System.Web.HttpContext.Current.Server.MapPath(string.Format("/{0}/Xml/Gift_Redeem.xml", _Config._VirtualResourceDir));

                        if (!System.IO.File.Exists(mappedPath))
                            return string.Empty;

                        System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                        doc.XmlResolver = null;
                        doc.Load(mappedPath);

                        System.Xml.XmlElement xml = doc.DocumentElement;

                        if (xml != null)
                        {
                            _giftRedemptionInstructions = xml.InnerXml.Replace("{0}", _Config._DomainName).Trim();
                        }
                    }
                    catch (Exception ex)
                    {
                        _Error.LogException(ex);
                        return string.Empty;
                    }
                }

                return _giftRedemptionInstructions;
            }
        }
        private static string _giftDelivery = string.Empty;
        /// <summary>
        /// format this for email
        /// </summary>
        public static string _GiftDelivery
        {
            get
            {
                if (_giftDelivery == null || _giftDelivery.Trim().Length == 0)
                {
                    try
                    {
                        string mappedPath = System.Web.HttpContext.Current.Server.MapPath(string.Format("/{0}/Xml/Gift_Delivery.xml", _Config._VirtualResourceDir));

                        if (!System.IO.File.Exists(mappedPath))
                            return string.Empty;

                        System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                        doc.XmlResolver = null;
                        doc.Load(mappedPath);

                        System.Xml.XmlElement xml = doc.DocumentElement;

                        if (xml != null)
                        {
                            _giftDelivery = xml.InnerXml.Replace("{0}", _Config._DomainName).Trim();
                        }
                    }
                    catch (Exception ex)
                    {
                        _Error.LogException(ex);
                        return string.Empty;
                    }
                }

                return _giftDelivery;
            }
        }

        private static string _downloadInstructions_1320 = string.Empty;
        /// <summary>
        /// format this for email
        /// </summary>
        public static string _DownloadInstructions_1320
        {
            get
            {
                if (_downloadInstructions_1320 == null || _downloadInstructions_1320.Trim().Length == 0)
                {
                    try
                    {
                        string mappedPath = System.Web.HttpContext.Current.Server.MapPath(string.Format("/{0}/Xml/Download_Instructions_1320.xml", _Config._VirtualResourceDir));

                        if (!System.IO.File.Exists(mappedPath))
                            return string.Empty;

                        System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                        doc.XmlResolver = null;
                        doc.Load(mappedPath);

                        System.Xml.XmlElement xml = doc.DocumentElement;

                        if (xml != null)
                        {
                            _downloadInstructions_1320 = xml.InnerXml.Replace("{0}", _Config._DomainName).Trim();
                        }
                    }
                    catch (Exception ex)
                    {
                        _Error.LogException(ex);
                        return string.Empty;
                    }
                }

                return _downloadInstructions_1320;
            }
        }

        #endregion

        #region CONSTANTS

        #region Service Exclusive

        public static bool svc_ServiceTestMode { get { return bool.Parse(ConfigurationManager.AppSettings["svc_ServiceTestMode"]); } }
        public static string svc_ServiceTestEmail { get { return ConfigurationManager.AppSettings["svc_ServiceTestEmail"]; } }
        public static string svc_ServiceTestFromName { get { return ConfigurationManager.AppSettings["svc_ServiceTestFromName"]; } }
        public static int svc_MaxThreads { get { return int.Parse(ConfigurationManager.AppSettings["svc_MaxThreads"]); } }
        public static bool svc_UseSqlDebug { get { return bool.Parse(ConfigurationManager.AppSettings["svc_UseSqlDebug"]); } }

        public static string svc_ServiceEmail { get { return ConfigurationManager.AppSettings["svc_ServiceEmail"]; } }
        public static string svc_ServiceFromName { get { return ConfigurationManager.AppSettings["svc_ServiceFromName"]; } }

        public static string svc_SitePhysicalAddress { get { return ConfigurationManager.AppSettings["svc_SitePhysicalAddress"]; } }
        public static string svc_WebmasterEmail { get { return ConfigurationManager.AppSettings["svc_WebmasterEmail"]; } }
        public static string svc_AbsoluteBadmailPath { get { return ConfigurationManager.AppSettings["_AbsoluteBadmailPath"]; } }
        public static string svc_MappedVirtualResourceDirectory { get { return ConfigurationManager.AppSettings["MappedVirtualResourceDirectory"]; } }

        /// <summary>
        /// Interval between rows PROCESSED
        /// </summary>
        public static int svc_JobIntervalMilliSeconds { get { return int.Parse(ConfigurationManager.AppSettings["svc_JobIntervalMilliSeconds"]); } }
        /// <summary>
        /// Interval between rows GATHERED
        /// </summary>
        public static int svc_PauseBetweenBatches { get { return int.Parse(ConfigurationManager.AppSettings["svc_PauseBetweenBatches"]); } }
        /// <summary>
        /// Number Of Rows to gather at a time
        /// </summary>
        public static int svc_BatchRetrievalSize { get { return int.Parse(ConfigurationManager.AppSettings["svc_BatchRetrievalSize"]); } }
        /// <summary>
        /// Number Of Days to leave processed mails in queue
        /// </summary>
        public static int svc_ArchiveAfterDays { get { return int.Parse(ConfigurationManager.AppSettings["svc_ArchiveAfterDays"]); } }

        //TODO: remove this key after testing
        public static bool _SubscriptionsActive { get { return bool.Parse(ConfigurationManager.AppSettings["stp_SubscriptionsActive"]); } }

        #endregion

        private static Guid _appId = Guid.Empty;
        public static Guid APPLICATION_ID
        {
            get
            {
                if (_appId == Guid.Empty && _Config.APPLICATION_NAME != null)
                {
                    AspnetApplication app = new AspnetApplication();
                    app.LoadAndCloseReader(AspnetApplication.FetchByParameter("ApplicationName", _Config.APPLICATION_NAME));

                    if (app != null)
                        _appId = app.ApplicationId;
                }

                return _appId;
            }
        }

        public static string DSN { get { return System.Configuration.ConfigurationManager.ConnectionStrings["WillCallConnectionString"].ToString(); } }
        public static readonly int _MaxYearsCardExpiry = 15;

        /// <summary>
        /// create an alias that is easier to find in intellisense
        /// </summary>
        public int _Default_NOTSELECTED_Value = _NoSelectionIdValue;
        public static readonly int  _NoSelectionIdValue = 99;

        public static readonly int _Coupon_MaxNumAllowed = 5;
        public static readonly bool _Coupon_IgnoreCase = true;
        public const Wcss._Enums.DeliveryType DeliveryTypeDefault = Wcss._Enums.DeliveryType.parcel;
       
        #endregion

        #region Flow

        private static void TestFlow()
        {
            string val;
            int idx;
            bool bbb;

            idx = _Config._MaxMerchPurchaseQuantity;
            idx = _Config._MaxTicketPurchaseQuantity;
            TimeSpan t = _Config._BoxOffice_TicketSales_Start;
            bbb = _Config._Allow_3rdPartyPurchase;
            bbb = _Config._Display_CountdownTimer;
            bbb = _Config._Display_Venue;
            //bbb = _Config._Display_AllShowsInMenu;
            idx = _Config._Display_Next_N_Events;
            val = _Config._Display_Next_N_Events_Title;
            bbb = _Config._Display_CalendarView;
            val = _Config._Display_CalendarView_Title;
            bbb = _Config._Sales_Merch_Active;
            bbb = _Config._Sales_Tickets_Active;
            bbb = _Config._DisplayDatesAsRange;
            bbb = _Config._Display_DefaultToAllShowsInReports;
            val = _Config._AuthMessage;
            //val = _Config._TicketShipNotes;
            val = _Config._MerchShipNotes;
            val = _Config._LandingPageUrl;
            idx = _Config._Items_BestSellersToDisplay;
            idx = _Config._Items_FeaturedToDisplay;
            idx = _Config._Items_GoingFastToDisplay;
            val = _Config._CartTitle_ShipLinkedTicket;
            val = _Config._CartTitle;
            val = _Config._CartTitle_Merch;
            val = _Config._CartTitle_Tickets;
            val = _Config._CartTitle_Promotion;
            val = _Config._SiteTitle;
            val = _Config._MerchMenu_DownloadsLink;
            val = _Config._Promotion_Text;
            val = _Config._ShowLinks_Header;
            bbb = _Config._MerchMenu_HideDivisionHeader;
            bbb = _Config._MerchMenu_HideCategorieHeader;
            bbb = _Config._MerchMenu_DisplayAllCategoryProducts;
            bbb = _Config._MerchMenu_DisplayProductThumbnail;
            bbb = _Config._MerchMenu_DisplayOnlyItemInCategorieAsCategorie;

        }

        
        public static TimeSpan _DosTicket_SalesCutoff
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Default.ToString().ToLower() &&
                            match.Name.ToLower() == "dosticket_salescutoff" && match.ValueX != null &&
                            Utils.Validation.IsDecimal(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return TimeSpan.FromHours(double.Parse(config.ValueX.Trim()));

                return TimeSpan.FromHours(17);
            }
        }

        /// <summary>
        /// returns a list of amounts
        /// </summary>
        public static System.Collections.Generic.List<decimal> _CharityAmounts
        {
            get
            {
                System.Collections.Generic.List<decimal> _donAmounts = new System.Collections.Generic.List<decimal>();

                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Flow.ToString().ToLower() &&
                            match.Name.ToLower() == "donations_amountlist" && match.ValueX != null);
                    });
                if (config != null)
                {
                    if (config.ValueX != null && config.ValueX.Trim().Length > 0)
                    {
                        string[] amounts = config.ValueX.Split(',');
                        foreach (string s in amounts)
                        {
                            if (Utils.Validation.IsDecimal(s.Trim()))
                                _donAmounts.Add(decimal.Parse(s.Trim()));
                        }
                    }
                }

                //at least have a $1 choice
                if (_donAmounts.Count == 0)
                    _donAmounts.Add(1.0M);

                return _donAmounts;
            }
        }
        public static int _Inventory_Tickets_GoingFast_Threshhold
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Flow.ToString().ToLower() &&
                            match.Name.ToLower() == "inventory_ticketsgoingfast_threshhold" && match.ValueX != null && match.ValueX.Trim().Length > 0);
                    });
                if (config != null && config.Id > 0)
                    return int.Parse(config.ValueX.Trim());

                return 0;
            }
        }
        public static string _Inventory_Tickets_GoingFast_Text
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Flow.ToString().ToLower() &&
                            match.Name.ToLower() == "inventory_ticketsgoingfast_text" && match.ValueX != null && match.ValueX.Trim().Length > 0);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX.Trim();

                return string.Empty;
            }
        }
        public static int _MaxMerchPurchaseQuantity
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Flow.ToString().ToLower() &&
                            match.Name.ToLower() == "maxmerchpurchasequantity" && match.ValueX != null &&
                            Utils.Validation.IsInteger(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return int.Parse(config.ValueX.Trim());

                return 0;
            }
        }
        public static int _MaxTicketPurchaseQuantity
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Flow.ToString().ToLower() &&
                            match.Name.ToLower() == "maxticketpurchasequantity" && match.ValueX != null &&
                            Utils.Validation.IsInteger(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return int.Parse(config.ValueX.Trim());

                return 0;
            }
        }
        public static TimeSpan _BoxOffice_TicketSales_Start
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Flow.ToString().ToLower() &&
                            match.Name.ToLower() == "boxoffice_ticketsales_start" && match.ValueX != null &&
                            Utils.Validation.IsDecimal(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return TimeSpan.FromHours(double.Parse(config.ValueX.Trim()));

                return TimeSpan.FromHours(8);
            }
        }
        public static bool _Allow_3rdPartyPurchase
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Flow.ToString().ToLower() &&
                            match.Name.ToLower() == "allow_3rdpartypurchase" && match.ValueX != null &&
                            Utils.Validation.IsBoolean(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return bool.Parse(config.ValueX);

                return false;
            }
        }
        public static bool _DisplayDatesAsRange
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Flow.ToString().ToLower() &&
                            match.Name.ToLower() == "displaydatesasrange" && match.ValueX != null &&
                            Utils.Validation.IsBoolean(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return bool.Parse(config.ValueX);

                return false;
            }
        }
        /// <summary>
        /// used for situations where there is not a full calendar of shows
        /// </summary>
        public static bool _Display_DefaultToAllShowsInReports
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Flow.ToString().ToLower() &&
                            match.Name.ToLower() == "display_defaulttoallshowsinreports" && match.ValueX != null &&
                            Utils.Validation.IsBoolean(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return bool.Parse(config.ValueX);

                return false;
            }
        }
        public static bool _Display_CountdownTimer
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Flow.ToString().ToLower() &&
                            match.Name.ToLower() == "display_countdowntimer" && match.ValueX != null &&
                            Utils.Validation.IsBoolean(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return bool.Parse(config.ValueX);

                return false;
            }
        }
        public static bool _Display_Venue
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Flow.ToString().ToLower() &&
                            match.Name.ToLower() == "display_venue" && match.ValueX != null &&
                            Utils.Validation.IsBoolean(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return bool.Parse(config.ValueX);

                return false;
            }
        }
        /// <summary>
        /// note the reverse logic here due to the wording of WITHOUT
        /// </summary>
        public static bool _DisplayMenu_MonthsWithoutShows
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Flow.ToString().ToLower() &&
                            match.Name.ToLower() == "display_menu_monthswithoutshows" && match.ValueX != null &&
                            Utils.Validation.IsBoolean(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return bool.Parse(config.ValueX);

                return false;
            }
        }
        public static int _Display_Next_N_Events
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Flow.ToString().ToLower() &&
                            match.Name.ToLower() == "display_next_n_events" && match.ValueX != null &&
                            Utils.Validation.IsInteger(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return int.Parse(config.ValueX);

                return 0;
            }
        }
        public static string _Display_Next_N_Events_Title
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Flow.ToString().ToLower() &&
                            match.Name.ToLower() == "display_next_n_events_title" && match.ValueX != null);
                    });
                if (config != null)
                    return config.ValueX;

                return null;
            }
        }
        public static string _GiftLogo
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Default.ToString().ToLower() &&
                            match.Name.ToLower() == "giftlogo" && match.ValueX != null);
                    });
                if (config != null)
                    return config.ValueX;

                return string.Empty;
            }
        }
        public static string _SiteLogo
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Default.ToString().ToLower() &&
                            match.Name.ToLower() == "sitelogo" && match.ValueX != null);
                    });
                if (config != null)
                    return config.ValueX;

                return string.Empty;
            }
        }
        public static string _TicketLogo
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Default.ToString().ToLower() &&
                            match.Name.ToLower() == "ticketlogo" && match.ValueX != null);
                    });
                if (config != null)
                    return config.ValueX;
                else
                {
                    config = _Config.AddNewConfig(_Enums.SiteConfigContext.Default, _Enums.ConfigDataTypes._string, 256,
                        "TicketLogo",
                        @"A small image for use on tickets. Target dimensions are roughly 55x15.",
                        _Config._SiteLogo);

                    return config.ValueX;
                }
            }
        }
        public static string _LandingPageUrl
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Default.ToString().ToLower() &&
                            match.Name.ToLower() == "landingpageurl" && match.ValueX != null);
                    });
                if (config != null)
                    return config.ValueX;
                else
                {
                    config = _Config.AddNewConfig(_Enums.SiteConfigContext.Default, _Enums.ConfigDataTypes._string, 256,
                        "LandingPageUrl",
                        @"Used to specify a landing page url other than the ticket page. To reset to the ticketing page, set to blank.",
                        string.Empty);

                    return config.ValueX;
                }
            }
        }
        public static bool _Display_CalendarView
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Flow.ToString().ToLower() &&
                            match.Name.ToLower() == "display_calendarview" && match.ValueX != null &&
                            Utils.Validation.IsBoolean(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return bool.Parse(config.ValueX);

                return false;
            }
        }
        public static string _Display_CalendarView_Title
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Flow.ToString().ToLower() &&
                            match.Name.ToLower() == "display_calendarviewtitle" && match.ValueX != null);
                    });
                if (config != null)
                    return config.ValueX;

                return null;
            }
        }
        public static bool _Sales_Merch_Active
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Flow.ToString().ToLower() &&
                            match.Name.ToLower() == "sales_merch_active" && match.ValueX != null &&
                            Utils.Validation.IsBoolean(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return bool.Parse(config.ValueX);

                return false;
            }
        }
        public static bool _Sales_Tickets_Active
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Flow.ToString().ToLower() &&
                            match.Name.ToLower() == "sales_tickets_active" && match.ValueX != null &&
                            Utils.Validation.IsBoolean(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return bool.Parse(config.ValueX);

                return false;
            }
        }
        public static bool _ShowServiceFeesOnInfoPages
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Flow.ToString().ToLower() &&
                            match.Name.ToLower() == "showservicefeesoninfopages" && match.ValueX != null &&
                            Utils.Validation.IsBoolean(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return bool.Parse(config.ValueX);

                return true;
            }
        }
        public static string _AuthMessage
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Flow.ToString().ToLower() &&
                            match.Name.ToLower() == "auth_message" && match.ValueX != null && match.ValueX.Trim().Length > 0);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX.Trim();

                return string.Empty;
            }
        }
        public static string _MerchShipNotes
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Flow.ToString().ToLower() &&
                            match.Name.ToLower() == "merch_shipnotes" && match.ValueX != null && match.ValueX.Trim().Length > 0);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX.Trim();

                return string.Empty;
            }
        }
        public static int _Items_FeaturedToDisplay
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Flow.ToString().ToLower() &&
                            match.Name.ToLower() == "items_featuredtodisplay" && match.ValueX != null &&
                            Utils.Validation.IsInteger(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return int.Parse(config.ValueX.Trim());

                return 0;
            }
        }
        public static int _Items_BestSellersToDisplay
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Flow.ToString().ToLower() &&
                            match.Name.ToLower() == "items_bestsellerstodisplay" && match.ValueX != null &&
                            Utils.Validation.IsInteger(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return int.Parse(config.ValueX.Trim());

                return 0;
            }
        }
        public static int _Items_GoingFastToDisplay
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Flow.ToString().ToLower() &&
                            match.Name.ToLower() == "items_goingfasttodisplay" && match.ValueX != null &&
                            Utils.Validation.IsInteger(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return int.Parse(config.ValueX.Trim());

                return 0;
            }
        }
        public static string _CartTitle_ShipLinkedTicket
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Flow.ToString().ToLower() &&
                            match.Name.ToLower() == "carttitle_shiplinkedticket" && match.ValueX != null && match.ValueX.Trim().Length > 0);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX.Trim();

                return string.Empty;
            }
        }
        public static string _CartTitle_Tickets
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Flow.ToString().ToLower() &&
                            match.Name.ToLower() == "carttitle_tickets" && match.ValueX != null && match.ValueX.Trim().Length > 0);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX.Trim();

                return string.Empty;
            }
        }
        public static string _CartTitle_Merch
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Flow.ToString().ToLower() &&
                            match.Name.ToLower() == "carttitle_merch" && match.ValueX != null && match.ValueX.Trim().Length > 0);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX.Trim();

                return string.Empty;
            }
        }
        public static string _CartTitle_Promotion
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Flow.ToString().ToLower() &&
                            match.Name.ToLower() == "carttitle_promotion" && match.ValueX != null && match.ValueX.Trim().Length > 0);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX.Trim();

                return string.Empty;
            }
        }
        public static string _SiteTitle
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Flow.ToString().ToLower() &&
                            match.Name.ToLower() == "sitetitle" && match.ValueX != null && match.ValueX.Trim().Length > 0);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX.Trim();

                return string.Empty;
            }
        }
        public static string _CartTitle
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Flow.ToString().ToLower() &&
                            match.Name.ToLower() == "carttitle" && match.ValueX != null);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX.Trim();

                return string.Empty;
            }
        }

        public static string _MerchMenu_DownloadsLink
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Flow.ToString().ToLower() &&
                            match.Name.ToLower() == "merchmenu_downloadslink" && match.ValueX != null);
                    });
                if (config != null && config.Id > 0 && config.ValueX.Trim().Length > 0)
                    return config.ValueX.Trim();

                return null;
            }
        }
        public static string _MerchMenu_DownloadsDivision
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Flow.ToString().ToLower() &&
                            match.Name.ToLower() == "merchmenu_downloadsdivision" && match.ValueX != null);
                    });
                if (config != null && config.Id > 0 && config.ValueX.Trim().Length > 0)
                    return config.ValueX.Trim();

                return null;
            }
        }
        public static string _MerchMenu_DownloadsLinkText
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Flow.ToString().ToLower() &&
                            match.Name.ToLower() == "merchmenu_downloadslinktext" && match.ValueX != null);
                    });
                if (config != null && config.Id > 0 && config.ValueX.Trim().Length > 0)
                    return config.ValueX.Trim();

                return "Downloads";
            }
        }
        public static bool _MerchMenu_HideDivisionHeader
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Flow.ToString().ToLower() &&
                            match.Name.ToLower() == "merchmenu_hidedivisionheader" && match.ValueX != null &&
                            Utils.Validation.IsBoolean(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return bool.Parse(config.ValueX);
                else
                {
                    config = _Config.AddNewConfig(_Enums.SiteConfigContext.Flow, _Enums.ConfigDataTypes._boolean, 5,
                        "MerchMenu_HideDivisionHeader",
                        @"Turns off the display of the division header in the product menu.",
                        "false");

                    return bool.Parse(config.ValueX);
                }
            }
        }
        public static bool _MerchMenu_DisplayOnlyItemInCategorieAsCategorie
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Flow.ToString().ToLower() &&
                            match.Name.ToLower() == "merchmenu_displayonlyitemincategorieascategorie" && match.ValueX != null &&
                            Utils.Validation.IsBoolean(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return bool.Parse(config.ValueX);
                else
                {
                    config = _Config.AddNewConfig(_Enums.SiteConfigContext.Flow, _Enums.ConfigDataTypes._boolean, 5,
                        "MerchMenu_DisplayOnlyItemInCategorieAsCategorie",
                        @"If there is only one item in a categorie, display just the categorie name. If false, then display the product name instead of the categorie name.",
                        "false");

                    return bool.Parse(config.ValueX);
                }
            }
        }
        public static bool _MerchMenu_HideCategorieHeader
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Flow.ToString().ToLower() &&
                            match.Name.ToLower() == "merchmenu_hidecategorieheader" && match.ValueX != null &&
                            Utils.Validation.IsBoolean(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return bool.Parse(config.ValueX);
                else
                {
                    config = _Config.AddNewConfig(_Enums.SiteConfigContext.Flow, _Enums.ConfigDataTypes._boolean, 5,
                        "MerchMenu_HideCategorieHeader",
                        @"Turns off the display of the categorie header in the product menu.",
                        "false");

                    return bool.Parse(config.ValueX);
                }
            }
        }
        public static bool _MerchMenu_DisplayAllCategoryProducts
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Flow.ToString().ToLower() &&
                            match.Name.ToLower() == "merchmenu_displayallcategoryproducts" && match.ValueX != null &&
                            Utils.Validation.IsBoolean(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return bool.Parse(config.ValueX);
                else
                {
                    config = _Config.AddNewConfig(_Enums.SiteConfigContext.Flow, _Enums.ConfigDataTypes._boolean, 5,
                        "MerchMenu_DisplayAllCategoryProducts",
                        @"Displays all of the products within a category. Appropriate for small lists of offerings.",
                        "false");

                    return bool.Parse(config.ValueX);
                }
            }
        }
        public static bool _MerchMenu_DisplayProductThumbnail
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Flow.ToString().ToLower() &&
                            match.Name.ToLower() == "merchmenu_displayproductthumbnail" && match.ValueX != null &&
                            Utils.Validation.IsBoolean(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return bool.Parse(config.ValueX);
                else
                {
                    config = _Config.AddNewConfig(_Enums.SiteConfigContext.Flow, _Enums.ConfigDataTypes._boolean, 5,
                        "MerchMenu_DisplayProductThumbnail",
                        @"Display a product's thumbnail within the menu item's listing.",
                        "false");

                    return bool.Parse(config.ValueX);
                }
            }
        }
        
        public static string _Promotion_Text
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Flow.ToString().ToLower() &&
                            match.Name.ToLower() == "promotion_text" && match.ValueX != null);
                    });
                if (config != null && config.Id > 0 && config.ValueX.Trim().Length > 0)
                    return config.ValueX.Trim();

                return null;
            }
        }
        public static string _ShowLinks_Header
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Flow.ToString().ToLower() &&
                            match.Name.ToLower() == "containsshowlinks_header" && match.ValueX != null);
                    });
                if (config != null && config.Id > 0 && config.ValueX.Trim().Length > 0)
                    return config.ValueX.Trim();

                return string.Empty;
            }
        }

        #endregion

        #region Images

        private static void TestImages()
        {
            //string val;
            int idx;
            bool bbb;
            string val;

            val = _Config._BannerDimensionText;
            val = _Config._SiteImageUrl;
            idx = _Config._MerchThumbSizeSm;
            idx = _Config._MerchThumbSizeLg;
            idx = _Config._MerchThumbSizeMax;
            idx = _Config._ActThumbSizeSm;
            idx = _Config._ActThumbSizeLg;
            idx = _Config._ActThumbSizeMax;
            idx = _Config._VenueThumbSizeSm;
            idx = _Config._VenueThumbSizeLg;
            idx = _Config._VenueThumbSizeMax;
            idx = _Config._ShowThumbSizeSm;
            idx = _Config._ShowThumbSizeLg;
            idx = _Config._ShowThumbSizeMax;
            idx = _Config._CharityThumbSizeSm;
            idx = _Config._CharityThumbSizeLg;
            idx = _Config._CharityThumbSizeMax;
            idx = _Config._PromoterThumbSizeSm;
            idx = _Config._PromoterThumbSizeLg;
            idx = _Config._PromoterThumbSizeMax; 
            idx = _Config._MP3_Player_DisplayWidth;
            idx = _Config._MP3_Player_DisplayHeight;
            bbb = _Config._AlwaysUseLargeThumbsForDetail;
            bbb = _Config._HeaderImages_IgnoreOrder;
        }

        public static bool _HeaderImages_IgnoreOrder
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Images.ToString().ToLower() &&
                            match.Name.ToLower() == "headerimages_ignoreorder" && match.ValueX != null &&
                            Utils.Validation.IsBoolean(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return bool.Parse(config.ValueX);
                else
                {
                    config = _Config.AddNewConfig(_Enums.SiteConfigContext.Images, _Enums.ConfigDataTypes._boolean, 5,
                        "HeaderImages_IgnoreOrder",
                        "If true, display will ignore the display order and will order randomly.",
                        "false");

                    return bool.Parse(config.ValueX);
                }
            }
        }


        public static string _HeaderImageDimensionText
        {
            get
            {
                return string.Format("Header images should be {0} px wide and up to {1} px in height. Images less than the max width will be centered. Images less than the max height will be vertically aligned to the top of the image container.",
                    _HeaderImage_Width_MaxPixels.ToString(), _HeaderImage_Height_MaxPixels.ToString());
            }
        }
        public static int _HeaderImage_Width_MaxPixels
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Images.ToString().ToLower() &&
                            match.Name.ToLower() == "headerimage_width_maxpixels" && match.ValueX != null);
                    });
                if (config != null && config.Id > 0 && config.ValueX.Trim().Length > 0)
                    return int.Parse(config.ValueX.Trim());
                else
                {
                    config = _Config.AddNewConfig(_Enums.SiteConfigContext.Images, _Enums.ConfigDataTypes._int, 6,
                        "HeaderImage_Width_MaxPixels",
                        @"The max width, in pixels, to be used for header images. Images less than the max width will be centered. This setting needs to be synced with css values.",
                        "1000");

                    return int.Parse(config.ValueX);
                }
            }
        }
        public static int _HeaderImage_Height_MaxPixels
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Images.ToString().ToLower() &&
                            match.Name.ToLower() == "headerimage_height_maxpixels" && match.ValueX != null);
                    });
                if (config != null && config.Id > 0 && config.ValueX.Trim().Length > 0)
                    return int.Parse(config.ValueX.Trim());
                else
                {
                    config = _Config.AddNewConfig(_Enums.SiteConfigContext.Images, _Enums.ConfigDataTypes._int, 6,
                        "HeaderImage_Height_MaxPixels",
                        @"The max height, in pixels, to be used for header images. Images less than the max height will be vertically aligned to the top of the image container. This setting needs to be synced with css values.",
                        "180");

                    return int.Parse(config.ValueX);
                }
            }
        }
        public static int _HeaderImage_Default_TimeoutMsecs
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Images.ToString().ToLower() &&
                            match.Name.ToLower() == "headerimage_default_timeoutmsecs" && match.ValueX != null);
                    });
                if (config != null && config.Id > 0 && config.ValueX.Trim().Length > 0)
                    return int.Parse(config.ValueX.Trim());
                else
                {
                    config = _Config.AddNewConfig(_Enums.SiteConfigContext.Images, _Enums.ConfigDataTypes._int, 6,
                        "HeaderImage_Default_TimeoutMsecs",
                        @"The default time in milliseconds to display an image. Only valiud if there are multiple images in the display set.",
                        "2400");

                    return int.Parse(config.ValueX);
                }
            }
        }
        public static string _BannerDimensionText
        {
            get
            {
                return string.Format("images should be {0} px wide and up to {1} px in height. Images less than the max width will be centered. Images less than the max height will be vertically aligned to the top of the image container.", 
                    _Banner_Width_MaxPixels.ToString(), _Banner_Height_MaxPixels.ToString());
            }
        }
        
        public static int _Banner_Width_MaxPixels
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Images.ToString().ToLower() &&
                            match.Name.ToLower() == "banner_width_maxpixels" && match.ValueX != null);
                    });
                if (config != null && config.Id > 0 && config.ValueX.Trim().Length > 0)
                    return int.Parse(config.ValueX.Trim());
                else
                {
                    config = _Config.AddNewConfig(_Enums.SiteConfigContext.Images, _Enums.ConfigDataTypes._int, 6,
                        "Banner_Width_MaxPixels",
                        @"The max width, in pixels, to be used for banner images. Images less than the max width will be centered. This setting needs to be synced with css values.",
                        "700");

                    return int.Parse(config.ValueX);
                }
            }
        }

        public static int _Banner_Height_MaxPixels
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Images.ToString().ToLower() &&
                            match.Name.ToLower() == "banner_height_maxpixels" && match.ValueX != null);
                    });
                if (config != null && config.Id > 0 && config.ValueX.Trim().Length > 0)
                    return int.Parse(config.ValueX.Trim());
                else
                {
                    config = _Config.AddNewConfig(_Enums.SiteConfigContext.Images, _Enums.ConfigDataTypes._int, 6,
                        "Banner_Height_MaxPixels",
                        @"The max height, in pixels, to be used for banner images. Images less than the max height will be vertically aligned to the top of the image container. This setting needs to be synced with css values.",
                        "150");

                    return int.Parse(config.ValueX);
                }
            }
        }
        /// <summary>
        /// returns the full path to the site url
        /// </summary>
        public static string _SiteImageUrl
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Images.ToString().ToLower() &&
                            match.Name.ToLower() == "siteimageurl" && match.ValueX != null);
                    });
                if (config != null && config.Id > 0 && config.ValueX.Trim().Length > 0)
                    return config.ValueX.Trim();
                else
                {
                    config = _Config.AddNewConfig(_Enums.SiteConfigContext.Images, _Enums.ConfigDataTypes._string, 256,
                        "SiteImageUrl",
                        @"A url to the image to be used for the site. Please use the full path - http://domain.com/path/to/image.jpg",
                        string.Empty
                        //.Format("http://{0}/{1}/Images/UI/{2}", _Config._DomainName, _Config._VirtualResourceDir, "sts9_logo_sq.jpg"
                        );

                    return config.ValueX;
                }
            }
        }

        public static int _MerchThumbSizeSm
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Images.ToString().ToLower() &&
                            match.Name.ToLower() == "merch_thumbnail_size_small" && match.ValueX != null &&
                            Utils.Validation.IsInteger(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return int.Parse(config.ValueX);

                return 75;
            }
        }
        public static int _MerchThumbSizeLg
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Images.ToString().ToLower() &&
                            match.Name.ToLower() == "merch_thumbnail_size_large" && match.ValueX != null &&
                            Utils.Validation.IsInteger(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return int.Parse(config.ValueX);

                return 125;
            }
        }
        public static int _MerchThumbSizeMax
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Images.ToString().ToLower() &&
                            match.Name.ToLower() == "merch_thumbnail_size_max" && match.ValueX != null &&
                            Utils.Validation.IsInteger(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return int.Parse(config.ValueX);

                return 175;
            }
        }
        public static bool _AlwaysUseLargeThumbsForDetail
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Images.ToString().ToLower() &&
                            match.Name.ToLower() == "alwaysuselargethumbsfordetail" && match.ValueX != null &&
                            Utils.Validation.IsBoolean(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return bool.Parse(config.ValueX);

                return false;
            }
        }
        public static int _ActThumbSizeSm
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Images.ToString().ToLower() &&
                            match.Name.ToLower() == "act_thumbnail_size_small" && match.ValueX != null &&
                            Utils.Validation.IsInteger(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return int.Parse(config.ValueX);

                return 75;
            }
        }
        public static int _ActThumbSizeLg
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Images.ToString().ToLower() &&
                            match.Name.ToLower() == "act_thumbnail_size_large" && match.ValueX != null &&
                            Utils.Validation.IsInteger(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return int.Parse(config.ValueX);

                return 125;
            }
        }
        public static int _ActThumbSizeMax
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Images.ToString().ToLower() &&
                            match.Name.ToLower() == "act_thumbnail_size_max" && match.ValueX != null &&
                            Utils.Validation.IsInteger(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return int.Parse(config.ValueX);

                return 175;
            }
        }
        public static int _ShowThumbSizeSm
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Images.ToString().ToLower() &&
                            match.Name.ToLower() == "show_thumbnail_size_small" && match.ValueX != null &&
                            Utils.Validation.IsInteger(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return int.Parse(config.ValueX);

                return 75;
            }
        }
        public static int _ShowThumbSizeLg
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Images.ToString().ToLower() &&
                            match.Name.ToLower() == "show_thumbnail_size_large" && match.ValueX != null &&
                            Utils.Validation.IsInteger(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return int.Parse(config.ValueX);

                return 125;
            }
        }
        public static int _ShowThumbSizeMax
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Images.ToString().ToLower() &&
                            match.Name.ToLower() == "show_thumbnail_size_max" && match.ValueX != null &&
                            Utils.Validation.IsInteger(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return int.Parse(config.ValueX);

                return 300;
            }
        }
        public static int _CharityThumbSizeSm
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Images.ToString().ToLower() &&
                            match.Name.ToLower() == "charity_thumbnail_size_small" && match.ValueX != null &&
                            Utils.Validation.IsInteger(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return int.Parse(config.ValueX);

                return 75;
            }
        }
        public static int _CharityThumbSizeLg
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Images.ToString().ToLower() &&
                            match.Name.ToLower() == "charity_thumbnail_size_large" && match.ValueX != null &&
                            Utils.Validation.IsInteger(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return int.Parse(config.ValueX);

                return 125;
            }
        }
        public static int _CharityThumbSizeMax
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Images.ToString().ToLower() &&
                            match.Name.ToLower() == "charity_thumbnail_size_max" && match.ValueX != null &&
                            Utils.Validation.IsInteger(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return int.Parse(config.ValueX);

                return 300;
            }
        }
        public static int _PromoterThumbSizeSm
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Images.ToString().ToLower() &&
                            match.Name.ToLower() == "promoter_thumbnail_size_small" && match.ValueX != null &&
                            Utils.Validation.IsInteger(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return int.Parse(config.ValueX);

                return 75;
            }
        }
        public static int _PromoterThumbSizeLg
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Images.ToString().ToLower() &&
                            match.Name.ToLower() == "promoter_thumbnail_size_large" && match.ValueX != null &&
                            Utils.Validation.IsInteger(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return int.Parse(config.ValueX);

                return 125;
            }
        }
        public static int _PromoterThumbSizeMax
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Images.ToString().ToLower() &&
                            match.Name.ToLower() == "promoter_thumbnail_size_max" && match.ValueX != null &&
                            Utils.Validation.IsInteger(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return int.Parse(config.ValueX);

                return 300;
            }
        }
        public static int _VenueThumbSizeSm
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Images.ToString().ToLower() &&
                            match.Name.ToLower() == "venue_thumbnail_size_small" && match.ValueX != null &&
                            Utils.Validation.IsInteger(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return int.Parse(config.ValueX);

                return 75;
            }
        }
        public static int _VenueThumbSizeLg
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Images.ToString().ToLower() &&
                            match.Name.ToLower() == "venue_thumbnail_size_large" && match.ValueX != null &&
                            Utils.Validation.IsInteger(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return int.Parse(config.ValueX);

                return 125;
            }
        }
        public static int _VenueThumbSizeMax
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Images.ToString().ToLower() &&
                            match.Name.ToLower() == "venue_thumbnail_size_max" && match.ValueX != null &&
                            Utils.Validation.IsInteger(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return int.Parse(config.ValueX);

                return 175;
            }
        }
        public static int _MP3_Player_DisplayWidth
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Images.ToString().ToLower() &&
                            match.Name.ToLower() == "mp3_player_displaywidth" && match.ValueX != null &&
                            Utils.Validation.IsInteger(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return int.Parse(config.ValueX);

                return 175;
            }
        }
        public static int _MP3_Player_DisplayHeight
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Images.ToString().ToLower() &&
                            match.Name.ToLower() == "mp3_player_displayheight" && match.ValueX != null &&
                            Utils.Validation.IsInteger(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return int.Parse(config.ValueX);

                return 175;
            }
        }

        #endregion

        #region PageMsg

        private static void TestPageMsg()
        {
            string val;
            //int idx;
            //bool bbb;

            val = _Config._Message_ShippingPage;
            val = _Config._Message_CheckoutPage;
            val = _Config._Message_ContactPage;
            val = _Config._Message_Contact_ReTicketShipping;
            val = _Config._Message_CreateNewAccount;
            val = _Config._Message_ChooseTicketShippingPage;
            val = _Config._Message_MerchComingSoon;
            val = _Config._Mailer_ControlTitle;
            val = _Config._Mailer_ControlGreeting;
            val = _Config._PageTitle_Header;
            val = _Config._Message_Goodwill;
            val = _Config._Message_ChooseTicket_ChoiceCaution;
            val = _Config._Message_CartEdit_TicketTerms;
            val = _Config._TicketPurchaseInstructions;
            val = _Config._Message_MerchBundleInstruction;
        }
        public static string _Message_MerchBundleInstruction
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                   delegate(SiteConfig match)
                   {
                       return (match.Context.ToLower() == _Enums.SiteConfigContext.PageMsg.ToString().ToLower() &&
                           match.Name.ToLower() == "message_merchbundleinstructions" && match.ValueX != null && match.ValueX.Trim().Length > 0);
                   });
                if (config != null && config.Id > 0)
                    return config.ValueX.Trim();
                else
                {
                    config = _Config.AddNewConfig(_Enums.SiteConfigContext.PageMsg, _Enums.ConfigDataTypes._string, 1024,
                        "Message_MerchBundleInstructions",
                        string.Empty,
                        "Bundled items will be available for selection during checkout.");

                    return config.ValueX;
                }
            }
        }
        public static string _TicketPurchaseInstructions
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.PageMsg.ToString().ToLower() &&
                            match.Name.ToLower() == "ticketpurchaseinstructions" && match.ValueX != null && match.ValueX.Trim().Length > 0);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX.Trim();

                return null;
            }
        }
        public static string _PageTitle_Header
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.PageMsg.ToString().ToLower() &&
                            match.Name.ToLower() == "pagetitle_header" && match.ValueX != null && match.ValueX.Trim().Length > 0);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX.Trim();

                return null;
            }
        }
        public static string _Message_ShippingPage
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.PageMsg.ToString().ToLower() &&
                            match.Name.ToLower() == "message_shippingpage" && match.ValueX != null && match.ValueX.Trim().Length > 0);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX.Trim();

                return null;
            }
        }
        public static string _Message_Goodwill
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.PageMsg.ToString().ToLower() &&
                            match.Name.ToLower() == "message_goodwill" && match.ValueX != null && match.ValueX.Trim().Length > 0);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX.Trim();

                return null;
            }
        }
        public static string _Message_CheckoutPage
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.PageMsg.ToString().ToLower() &&
                            match.Name.ToLower() == "message_checkoutpage" && match.ValueX != null && match.ValueX.Trim().Length > 0);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX.Trim();

                return null;
            }
        }
        public static string _Message_ContactPage
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.PageMsg.ToString().ToLower() &&
                            match.Name.ToLower() == "message_contactpage" && match.ValueX != null && match.ValueX.Trim().Length > 0);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX.Trim();

                return string.Empty;
            }
        }
        public static string _Message_Contact_ReTicketShipping
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.PageMsg.ToString().ToLower() &&
                            match.Name.ToLower() == "message_contact_reticketshipping" && match.ValueX != null && match.ValueX.Trim().Length > 0);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX.Trim();

                return string.Empty;
            }
        }
        public static string _Message_CreateNewAccount
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.PageMsg.ToString().ToLower() &&
                            match.Name.ToLower() == "message_createnewaccount" && match.ValueX != null && match.ValueX.Trim().Length > 0);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX.Trim();

                return string.Empty;
            }
        }
        public static string _Message_ChooseTicketShippingPage
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.PageMsg.ToString().ToLower() &&
                            match.Name.ToLower() == "message_chooseticketshippingpage" && match.ValueX != null && match.ValueX.Trim().Length > 0);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX.Trim();

                return string.Empty;
            }
        }
        public static string _Message_MerchComingSoon
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.PageMsg.ToString().ToLower() &&
                            match.Name.ToLower() == "message_merchcomingsoon" && match.ValueX != null && match.ValueX.Trim().Length > 0);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX.Trim();

                return string.Empty;
            }
        }
        public static string _Mailer_ControlTitle
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.PageMsg.ToString().ToLower() &&
                            match.Name.ToLower() == "mailer_controltitle" && match.ValueX != null && match.ValueX.Trim().Length > 0);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX.Trim();

                return string.Empty;
            }
        }
        public static string _Mailer_ControlGreeting
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.PageMsg.ToString().ToLower() &&
                            match.Name.ToLower() == "mailer_controlgreeting" && match.ValueX != null && match.ValueX.Trim().Length > 0);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX.Trim();

                return string.Empty;
            }
        }
        public static string _Message_ChooseTicket_ChoiceCaution
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.PageMsg.ToString().ToLower() &&
                            match.Name.ToLower() == "message_chooseticket_choicecaution" && match.ValueX != null && match.ValueX.Trim().Length > 0);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX.Trim();

                return string.Empty;
            }
        }
        public static string _Message_CartEdit_TicketTerms
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.PageMsg.ToString().ToLower() &&
                            match.Name.ToLower() == "message_cartedit_ticketterms" && match.ValueX != null && match.ValueX.Trim().Length > 0);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX.Trim();

                return string.Empty;
            }
        }

        #endregion

        #region Ship

        private static void TestShip()
        {
            string val;
            int idx;
            decimal dec;
            bool bbb;

            dec = _Config._HandlingFee_Min;
            dec = _Config._HandlingFee_Max;
            dec = _Config._HandlingFee_Pct;
            dec = _Config._Shipping_Tickets_FixedAmount;
            bbb = _Config._Shipping_Tickets_UseFixed;
            bbb = _Config._Shipping_Tickets_Active;
            bbb = _Config._Shipping_Merch_Active;
            val = _Config._Shipping_Tickets_ShipExplanation;
            val = _Config._Shipping_Tickets_DefaultMethod;
            bbb = _Config._Shipping_Tickets_USA_Only;
            bbb = _Config._DisplayEstimatedShipDates;
            bbb = _Config._Shipping_Tickets_UseDefaultMethodOnly;
            idx = _Config._Shipping_Ticket_CutoffDays;
            bbb = _Config._Shipping_EstimatedDays_UseLimitedDays;
            bbb = _Config._Shipping_EstimatedDays_TTh;
            bbb = _Config._Shipping_AllowTicketsToPoBox;
            bbb = _Config._Shipping_LowCostMethod_IsActive;
            val = _Config._Shipping_LowCostMethod_Name;
            dec = _Config._Shipping_LowCostMethod_Rate;
            idx = _Config._Shipping_LowCostMethod_MaxItems;
            bbb = _Config._Shipping_UPSGround_Merch_UseFlatRate;
            dec = _Config._Shipping_UPSGround_Merch_FlatRate;
        }

        public static bool _Shipping_AllowTicketsToPoBox
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Ship.ToString().ToLower() &&
                            match.Name.ToLower() == "shipping_allowticketstopobox" && match.ValueX != null && match.ValueX.Trim().Length > 0);
                    });
                if (config != null && config.Id > 0)
                    return bool.Parse(config.ValueX.Trim());
                else
                {
                    config = _Config.AddNewConfig(_Enums.SiteConfigContext.Ship, _Enums.ConfigDataTypes._boolean, 5,
                        "Shipping_AllowTicketsToPoBox",
                        "Allows shipping to PO boxes. Not recommended to use as true. Shipping to PO boxes cannot be verified.",
                        "false");

                    return bool.Parse(config.ValueX.Trim());
                }
            }
        }

        public static bool _Shipping_LowCostMethod_IsActive
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Ship.ToString().ToLower() &&
                            match.Name.ToLower() == "shipping_lowcostmethod_isactive" && match.ValueX != null && match.ValueX.Trim().Length > 0);
                    });
                if (config != null && config.Id > 0)
                    return bool.Parse(config.ValueX.Trim());
                else
                {
                    config = _Config.AddNewConfig(_Enums.SiteConfigContext.Ship, _Enums.ConfigDataTypes._boolean, 5,
                        "Shipping_LowCostMethod_IsActive",
                        "Toggles the low cost shipping option on or off.",
                        "true");

                    return bool.Parse(config.ValueX.Trim());
                }
            }
        }
        public static string _Shipping_LowCostMethod_Name
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Ship.ToString().ToLower() &&
                            match.Name.ToLower() == "shipping_lowcostmethod_name" && match.ValueX != null && match.ValueX.Trim().Length > 0);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX.Trim();
                else
                {
                    config = _Config.AddNewConfig(_Enums.SiteConfigContext.Ship, _Enums.ConfigDataTypes._string, 256,
                        "Shipping_LowCostMethod_Name",
                        "The name of the method to use for shipping in a flat rate box - or low cost shipping option.",
                        "Store Media Rate");

                    return config.ValueX;
                }
            }
        }
        public static decimal _Shipping_LowCostMethod_Rate
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Ship.ToString().ToLower() &&
                            match.Name.ToLower() == "shipping_lowcostmethod_rate" && match.ValueX != null && match.ValueX.Trim().Length > 0);
                    });
                if (config != null && config.Id > 0)
                    return decimal.Parse(config.ValueX);
                else
                {
                    config = _Config.AddNewConfig(_Enums.SiteConfigContext.Ship, _Enums.ConfigDataTypes._decimal, 15,
                        "Shipping_LowCostMethod_Rate",
                        "The rate of the low cost method.",
                        "5.00");

                    return decimal.Parse(config.ValueX);
                }
            }
        }
        public static int _Shipping_LowCostMethod_MaxItems
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Ship.ToString().ToLower() &&
                            match.Name.ToLower() == "shipping_lowcostmethod_maxitems" && match.ValueX != null && match.ValueX.Trim().Length > 0);
                    });
                if (config != null && config.Id > 0)
                    return int.Parse(config.ValueX);
                else
                {
                    config = _Config.AddNewConfig(_Enums.SiteConfigContext.Ship, _Enums.ConfigDataTypes._int, 5,
                        "Shipping_LowCostMethod_MaxItems",
                        "The maximum number of items that will be allowed in the low cost rate.",
                        "15");

                    return int.Parse(config.ValueX);
                }
            }
        }

        public static bool _Shipping_UPSGround_Merch_UseFlatRate
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Ship.ToString().ToLower() &&
                            match.Name.ToLower() == "shipping_upsground_merch_useflatrate" && match.ValueX != null && match.ValueX.Trim().Length > 0);
                    });
                if (config != null && config.Id > 0)
                    return bool.Parse(config.ValueX.Trim());
                else
                {
                    config = _Config.AddNewConfig(_Enums.SiteConfigContext.Ship, _Enums.ConfigDataTypes._boolean, 5,
                        "Shipping_UPSGround_Merch_UseFlatRate",
                        "Toggles ship rate calculations for UPS ground method. Be sure to specify a rate.",
                        "true");

                    return bool.Parse(config.ValueX.Trim());
                }
            }
        }
        public static decimal _Shipping_UPSGround_Merch_FlatRate
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Ship.ToString().ToLower() &&
                            match.Name.ToLower() == "shipping_upsground_merch_flatrate" && match.ValueX != null && match.ValueX.Trim().Length > 0);
                    });
                if (config != null && config.Id > 0)
                    return decimal.Parse(config.ValueX);
                else
                {
                    config = _Config.AddNewConfig(_Enums.SiteConfigContext.Ship, _Enums.ConfigDataTypes._decimal, 15,
                        "Shipping_UPSGround_Merch_FlatRate",
                        "The rate of the flat ups ground shipping.",
                        "9.99");

                    return decimal.Parse(config.ValueX);
                }
            }
        }


        public static decimal _HandlingFee_Min
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Ship.ToString().ToLower() &&
                            match.Name.ToLower() == "handlingfee_min" && match.ValueX != null &&
                            Utils.Validation.IsDecimal(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return decimal.Parse(config.ValueX);

                return 0;
            }
        }
        public static decimal _HandlingFee_Max
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Ship.ToString().ToLower() &&
                            match.Name.ToLower() == "handlingfee_max" && match.ValueX != null &&
                            Utils.Validation.IsDecimal(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return decimal.Parse(config.ValueX);

                return 50;
            }
        }
        public static decimal _HandlingFee_Pct
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Ship.ToString().ToLower() &&
                            match.Name.ToLower() == "handlingfee_pct" && match.ValueX != null &&
                            Utils.Validation.IsDecimal(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return decimal.Parse(config.ValueX);

                return 10;
            }
        }
        public static decimal _Shipping_Tickets_FixedAmount
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Ship.ToString().ToLower() &&
                            match.Name.ToLower() == "shipping_tickets_fixedamount" && match.ValueX != null &&
                            Utils.Validation.IsDecimal(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return decimal.Parse(config.ValueX);

                return 0;
            }
        }
        /// <summary>
        /// This will indicate if estimated shipping days are limited to certain days of the week or if we should use the TTH or MWF days
        /// </summary>
        public static bool _Shipping_EstimatedDays_UseLimitedDays
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Ship.ToString().ToLower() &&
                            match.Name.ToLower() == "shipping_estimateddays_uselimiteddays" && match.ValueX != null &&
                            Utils.Validation.IsBoolean(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return bool.Parse(config.ValueX);

                return false;
            }
        }
        /// <summary>
        /// TRUE will set estimated ship days to Tuesday and Thursday. FALSE will set estimnated ship days to be MWF
        /// </summary>
        public static bool _Shipping_EstimatedDays_TTh
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Ship.ToString().ToLower() &&
                            match.Name.ToLower() == "shipping_estimateddays_tth" && match.ValueX != null &&
                            Utils.Validation.IsBoolean(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return bool.Parse(config.ValueX);

                return false;
            }
        }
        public static bool _Shipping_Tickets_UseFixed
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Ship.ToString().ToLower() &&
                            match.Name.ToLower() == "shipping_tickets_usefixed" && match.ValueX != null &&
                            Utils.Validation.IsBoolean(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return bool.Parse(config.ValueX);

                return false;
            }
        }
        public static bool _DisplayEstimatedShipDates
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Ship.ToString().ToLower() &&
                            match.Name.ToLower() == "displayestimatedshipdates" && match.ValueX != null &&
                            Utils.Validation.IsBoolean(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return bool.Parse(config.ValueX);

                return true;
            }
        }
        public static bool _Shipping_Tickets_Active
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Ship.ToString().ToLower() &&
                            match.Name.ToLower() == "shipping_tickets_active" && match.ValueX != null &&
                            Utils.Validation.IsBoolean(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return bool.Parse(config.ValueX);

                return false;
            }
        }
        public static bool _Shipping_Merch_Active
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Ship.ToString().ToLower() &&
                            match.Name.ToLower() == "shipping_merch_active" && match.ValueX != null &&
                            Utils.Validation.IsBoolean(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return bool.Parse(config.ValueX);

                return false;
            }
        }
        public static string _Shipping_Merch_DefaultMethod
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Ship.ToString().ToLower() &&
                            match.Name.ToLower() == "shipping_merch_defaultmethod" && match.ValueX != null && match.ValueX.Trim().Length > 0);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX.Trim();
                else
                {
                    config = _Config.AddNewConfig(_Enums.SiteConfigContext.Ship, _Enums.ConfigDataTypes._string, 256,
                        "Shipping_Merch_DefaultMethod",
                        "The default method to use for merch shipping. As of NOV 2010, this is only for administration display purposes. This simply highlights methods other than the default for easier recognition by order fillers. Be sure to match case",
                        "UPS Ground");

                    return config.ValueX;
                }
            }
        }
        public static string _Shipping_Tickets_ShipExplanation
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Ship.ToString().ToLower() &&
                            match.Name.ToLower() == "shipping_tickets_shipexplanation" && match.ValueX != null && match.ValueX.Trim().Length > 0);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX.Trim();

                return string.Empty;
            }
        }
        public static string _Shipping_Tickets_DefaultMethod
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Ship.ToString().ToLower() &&
                            match.Name.ToLower() == "shipping_tickets_defaultmethod" && match.ValueX != null && match.ValueX.Trim().Length > 0);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX.Trim();

                return null;
            }
        }
        public static bool _Shipping_Tickets_USA_Only
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Ship.ToString().ToLower() &&
                            match.Name.ToLower() == "shipping_tickets_usa_only" && match.ValueX != null &&
                            Utils.Validation.IsBoolean(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return bool.Parse(config.ValueX);

                return true;
            }
        }
        /// <summary>
        /// If Shipping Tickets USA only - further exclude Hawaii (hi), Alaska (ak) and Puerto Rico (pr)
        /// </summary>
        public static bool _Shipping_Tickets_USA_Continental_Only
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Ship.ToString().ToLower() &&
                            match.Name.ToLower() == "shipping_tickets_usa_continental_only" && match.ValueX != null &&
                            Utils.Validation.IsBoolean(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return bool.Parse(config.ValueX);

                return false;
            }
        }
        public static bool _Shipping_Tickets_UseDefaultMethodOnly
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Ship.ToString().ToLower() &&
                            match.Name.ToLower() == "shipping_tickets_usedefaultmethodonly" && match.ValueX != null &&
                            Utils.Validation.IsBoolean(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return bool.Parse(config.ValueX);

                return true;
            }
        }
        public static int _Shipping_Ticket_CutoffDays
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Ship.ToString().ToLower() &&
                            match.Name.ToLower() == "shipping_ticket_cutoffdays" && match.ValueX != null &&
                            Utils.Validation.IsInteger(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return int.Parse(config.ValueX);

                return 10;
            }
        }

        #endregion

        #region Download
        /// <summary>
        /// Describes the download directory. This should be different from the virtual resource directory and in
        /// fact should be not be a virtual directory at all for security reasons.
        /// This path should be mapped
        /// </summary>
        public static string _DownloadDirectory
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Download.ToString().ToLower() &&
                            match.Name.ToLower() == "downloaddirectory" && match.ValueX != null && match.ValueX.Trim().Length > 0);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX;

                return null;
            }
        }

        /// <summary>
        /// Toggles downloads on and off
        /// </summary>
        public static bool _DownloadsActive
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Download.ToString().ToLower() &&
                            match.Name.ToLower() == "downloadsactive" && match.ValueX != null && match.ValueX.Trim().Length > 0);
                    });
                if (config != null && config.Id > 0)
                    return bool.Parse(config.ValueX);

                return false;
            }
        }

        /// <summary>
        /// Default is one
        /// </summary>
        public static int _DownloadMax
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Download.ToString().ToLower() &&
                            match.Name.ToLower() == "downloadmax" && match.ValueX != null && match.ValueX.Trim().Length > 0);
                    });
                if (config != null && config.Id > 0)
                    return int.Parse(config.ValueX);

                return 1;
            }
        }

        #endregion

        #region Default

        private static void TestDefault()
        {
            string val;
            int idx;
            decimal dec;
            //bool bbb;

            Age a = _Config._Default_Age;
            val = _Config._Default_PreVenueText;
            val = _Config._Default_VenueName;
            val = _Config._Default_ActName;
            val = _Config._Default_CountryCode;
            val = _Config._BoxOffice_Phone;
            val = _Config._MainOffice_Phone;
            val = _Config._BoxOffice_Address;
            val = _Config._BoxOffice_Description;
            idx = _Config._Default_DownloadInventoryAllotment;
            idx = _Config._BannerDisplayTime;
            dec = _Config._AuthorizeNetLimit;
        }

        public static decimal _AuthorizeNetLimit
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Default.ToString().ToLower() &&
                            match.Name.ToLower() == "authorizenetlimit" && match.ValueX != null && match.ValueX.Trim().Length > 0);
                    });
                if (config != null && config.Id > 0)
                    return decimal.Parse(config.ValueX);
                else
                {
                    config = _Config.AddNewConfig(_Enums.SiteConfigContext.Default, _Enums.ConfigDataTypes._decimal, 10,
                        "AuthorizeNetLimit",
                        "The maximum amount allowed per single transaction.",
                        "1050.00");

                    return int.Parse(config.ValueX);
                }
            }
        }

        public static int _Default_DownloadInventoryAllotment
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Default.ToString().ToLower() &&
                            match.Name.ToLower() == "default_downloadinventoryallotment" && match.ValueX != null && match.ValueX.Trim().Length > 0);
                    });
                if (config != null && config.Id > 0)
                    return int.Parse(config.ValueX);
                else
                {
                    config = _Config.AddNewConfig(_Enums.SiteConfigContext.Default, _Enums.ConfigDataTypes._int, 6,
                        "Default_DownloadInventoryAllotment",
                        "The number of items to allot to a newly created download. 6 chars max.",
                        "10000");

                    return int.Parse(config.ValueX);
                }                
            }
        }
        public static Age _Default_Age
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Default.ToString().ToLower() &&
                            match.Name.ToLower() == "default_age" && match.ValueX != null && match.ValueX.Trim().Length > 0);
                    });
                if (config != null && config.Id > 0)
                    return (Age)_Lookits.Ages.GetList().Find(delegate(Age match) { return (match.Name.ToLower() == config.ValueX.Trim().ToLower()); });

                Age defaultAge = (Age)_Lookits.Ages.GetList().Find(delegate(Age match) { return (match.Name.ToLower() == "all ages"); });
                if (defaultAge == null)
                    throw new Exception("Default Age could not be found");

                return defaultAge;
            }
        }
        public static string _Default_PreVenueText
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Default.ToString().ToLower() &&
                            match.Name.ToLower() == "default_prevenuetext" && match.ValueX != null);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX.Trim();

                return string.Empty;
            }
        }
        public static string _MailerTemplate_ShowEvent_DateFormat
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Default.ToString().ToLower() &&
                            match.Name.ToLower() == "mlrtplt_showevent_dateformat" && match.ValueX != null);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX.Trim();

                return string.Empty;
            }
        }
        public static string _MailerTemplate_ShowLinearEvent_DateFormat
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Default.ToString().ToLower() &&
                            match.Name.ToLower() == "mlrtplt_showlinearevent_dateformat" && match.ValueX != null);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX.Trim();

                return string.Empty;
            }
        }
        /// <summary>
        /// Defaults To empty string
        /// </summary>
        public static string _Default_VenueName
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Default.ToString().ToLower() &&
                            match.Name.ToLower() == "default_venuename" && match.ValueX != null);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX.Trim();

                return string.Empty;
            }
        }
        public static string _Default_ActName
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Default.ToString().ToLower() &&
                            match.Name.ToLower() == "default_actname" && match.ValueX != null && match.ValueX.Trim().Length > 0);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX.Trim();

                return string.Empty;
            }
        }
        public static string _Default_CountryCode
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Default.ToString().ToLower() &&
                            match.Name.ToLower() == "default_countrycode" && match.ValueX != null && match.ValueX.Trim().Length > 0);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX.Trim();

                return string.Empty;
            }
        }
        public static string _BoxOffice_Phone
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Default.ToString().ToLower() &&
                            match.Name.ToLower() == "boxoffice_phone" && match.ValueX != null && match.ValueX.Trim().Length > 0);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX.Trim();

                return string.Empty;
            }
        }
        public static string _BoxOffice_Address
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Default.ToString().ToLower() &&
                            match.Name.ToLower() == "boxoffice_address" && match.ValueX != null && match.ValueX.Trim().Length > 0);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX.Trim();

                return string.Empty;
            }
        }
        public static string _BoxOffice_Description
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Default.ToString().ToLower() &&
                            match.Name.ToLower() == "boxoffice_address" && match.Description != null && match.Description.Trim().Length > 0);
                    });
                if (config != null && config.Id > 0)
                    return config.Description.Trim();

                return string.Empty;
            }
        }
        public static string _MainOffice_Phone
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Default.ToString().ToLower() &&
                            match.Name.ToLower() == "mainoffice_phone" && match.ValueX != null && match.ValueX.Trim().Length > 0);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX.Trim();

                return string.Empty;
            }
        }
        public static bool _BannerOrder_Random
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Default.ToString().ToLower() &&
                            match.Name.ToLower() == "bannerorder_random" && match.ValueX != null && match.ValueX.Trim().Length > 0);
                    });
                if (config != null && config.Id > 0)
                    return bool.Parse(config.ValueX);

                return false;
            }
        }
        public static int _BannerDisplayTime
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Default.ToString().ToLower() &&
                            match.Name.ToLower() == "bannerdisplaytime" && match.ValueX != null && match.ValueX.Trim().Length > 0);
                    });
                if (config != null && config.Id > 0)
                    return int.Parse(config.ValueX);

                return 3200;
            }
        }

        #endregion

        #region Email

        private static void TestEmail()
        {
            string val;
            int idx;
            //decimal dec;
            //bool bbb;

            val = _Config._Confirmation_CCSales;
            val = _Config._Confirmation_FromName;
            val = _Config._Confirmation_Email;
            val = _Config._CustomerService_Email;
            val = _Config._CustomerService_FromName;
            val = _Config._MassMailService_Email;
            val = _Config._MassMailService_FromName;
            idx = _Config._Inventory_LowMerch_Threshold;
            idx = _Config._Inventory_LowTickets_Threshold;
            val = _Config._Inventory_Notification_Email;
            val = _Config._MailerCorrespondence_Email;
            val = _Config._MailerCorrespondence_FromName;
        }

        public static string _Confirmation_CCSales
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Email.ToString().ToLower() &&
                            match.Name.ToLower() == "confirmation_ccsales" && match.ValueX != null);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX.Trim();

                return string.Empty;
            }
        }
        public static string _Confirmation_FromName
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Email.ToString().ToLower() &&
                            match.Name.ToLower() == "confirmation_fromname" && match.ValueX != null && match.ValueX.Trim().Length > 0);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX.Trim();

                return string.Empty;
            }
        }
        public static string _Confirmation_Email
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Email.ToString().ToLower() &&
                            match.Name.ToLower() == "confirmation_email" && match.ValueX != null && match.ValueX.Trim().Length > 0);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX.Trim();

                return string.Empty;
            }
        }
        public static string _CustomerService_Email
        {
            get
            {
                try
                {
                    SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                        delegate(SiteConfig match)
                        {
                            return (match.Context.ToLower() == _Enums.SiteConfigContext.Email.ToString().ToLower() &&
                                match.Name.ToLower() == "customerservice_email" && match.ValueX != null && match.ValueX.Trim().Length > 0);
                        });
                    if (config != null && config.Id > 0)
                        return config.ValueX.Trim();
                }
                catch(Exception)
                {
                    string appConfigVal = _Config.svc_ServiceEmail ?? string.Empty;
                    return appConfigVal;
                }

                return string.Empty;
            }
        }
        public static string _CustomerService_FromName
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Email.ToString().ToLower() &&
                            match.Name.ToLower() == "customerservice_fromname" && match.ValueX != null && match.ValueX.Trim().Length > 0);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX.Trim();

                return string.Empty;
            }
        }                    
        public static string _MailerCorrespondence_Email
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Email.ToString().ToLower() &&
                            match.Name.ToLower() == "mailercorrespondence_email" && match.ValueX != null && match.ValueX.Trim().Length > 0);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX.Trim();

                return string.Empty;
            }
        }
        public static string _MailerCorrespondence_FromName
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Email.ToString().ToLower() &&
                            match.Name.ToLower() == "mailercorrespondence_fromname" && match.ValueX != null && match.ValueX.Trim().Length > 0);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX.Trim();

                return string.Empty;
            }
        }
        public static string _MassMailService_Email
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Email.ToString().ToLower() &&
                            match.Name.ToLower() == "massmailservice_email" && match.ValueX != null && match.ValueX.Trim().Length > 0);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX.Trim();

                return string.Empty;
            }
        }
        public static string _MassMailService_FromName
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Email.ToString().ToLower() &&
                            match.Name.ToLower() == "massmailservice_fromname" && match.ValueX != null && match.ValueX.Trim().Length > 0);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX.Trim();

                return string.Empty;
            }
        }
        public static int _Inventory_LowMerch_Threshold
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Email.ToString().ToLower() &&
                            match.Name.ToLower() == "inventory_lowmerchthreshold" && match.ValueX != null && match.ValueX.Trim().Length > 0);
                    });
                if (config != null && config.Id > 0)
                    return int.Parse(config.ValueX.Trim());

                return 0;
            }
        }
        public static int _Inventory_LowTickets_Threshold
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Email.ToString().ToLower() &&
                            match.Name.ToLower() == "inventory_lowticketsthreshold" && match.ValueX != null && match.ValueX.Trim().Length > 0);
                    });
                if (config != null && config.Id > 0)
                    return int.Parse(config.ValueX.Trim());

                return 0;
            }
        }
        public static string _Inventory_Notification_Email
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Email.ToString().ToLower() &&
                            match.Name.ToLower() == "inventory_notification_email" && match.ValueX != null && match.ValueX.Trim().Length > 0);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX.Trim();

                return string.Empty;
            }
        }

        #endregion

        #region Service Charges

        private static void TestService()
        {
            //string val;
            //int idx;
            decimal dec;
            bool bbb;

            bbb = _Config._Service_ApplyPercentageToTierFee;
            dec = _Config._Service_Percentage_Roundup;
        }

        /// <summary>
        /// if true, we figure tier value - then add pct based on ticket price + svc
        /// </summary>
        public static bool _Service_ApplyPercentageToTierFee
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Service.ToString().ToLower() &&
                            match.Name.ToLower() == "service_applypercentagetotierfee" && match.ValueX != null &&
                            Utils.Validation.IsBoolean(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return bool.Parse(config.ValueX);

                return false;
            }
        }
        public static decimal _Service_Percentage_Roundup
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Service.ToString().ToLower() &&
                            match.Name.ToLower() == "service_percentage_roundup" && match.ValueX != null &&
                            Utils.Validation.IsDecimal(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return decimal.Parse(config.ValueX);

                return 0;
            }
        }

        #endregion

        #region Admin

        private static void TestAdmin()
        {
            string val;
            int idx;
            //decimal dec;
            bool bbb;

            bbb = _Config._FAQ_Page_On;
            bbb = _Config._WaiveProcFeeOnGCs;
            bbb = _Config._TwitterWidget_On;
            bbb = _Config._TwitterModule_On;
            bbb = _Config._CharityListing_On;
            bbb = _Config._AllSales_OnlineOnly;
            bbb = _Config._Allow_PurchaseShippingPostSale;
            bbb = _Config._AllowCustomerInitiatedNameChanges;
            bbb = _Config._MaintenanceMode_On;
            bbb = _Config._Blog_External_On;
            val = _Config._CC_DeveloperEmail;
            val = _Report_DailySales_Recipients;
            _Enums.SiteEntityMode md = _Config._Site_Entity_Mode;
            val = _Config._Site_Entity_Name;
            val = _Config._Site_Entity_HomePage;
            val = _Config._Site_Entity_PhysicalAddress;
            val = _Config._Site_Entity_WebmasterEmail;
            val = _Config._Blog_External_Url;
            idx = _Config._TTL_Secs_CartItems;
            idx = _Config._TTL_Secs_Extend;
            val = _Config._Admin_EmailAddress;
            val = _Config._FeaturedItem_Order;
            val = _Config._EventMenu_ListingType;
            //val = _Config._Navigation_AdditionalLinks;
            val = _Config._Merchant_ChargeStatement_Descriptor;
            idx = _Config._InventoryRefreshIntervalSecs;
        }

        public static List<System.Web.UI.Triplet> _Navigation_AdditionalLinks
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Admin.ToString().ToLower() &&
                            match.Name.ToLower() == "navigation_additionallinks" && match.ValueX != null);
                    });
                if (config != null && config.Id > 0)
                {
                    return ConstructTripletList(config.ValueX);
                }

                else
                {
                    config = _Config.AddNewConfig(_Enums.SiteConfigContext.Admin, _Enums.ConfigDataTypes._string, 500,
                        "Navigation_AdditionalLinks",
                        @"enter in the format => link text,tooltip,link~ commas and tilde",
                        "");

                    return ConstructTripletList(config.ValueX);
                }
            }
        }

        public static List<System.Web.UI.Triplet> ConstructTripletList(string s)
        {
            List<System.Web.UI.Triplet> list = new List<System.Web.UI.Triplet>();

            if (s != null)
            {
                string[] pieces = s.Split(new string[] { "~" }, StringSplitOptions.RemoveEmptyEntries);

                if (pieces.Length > 0)
                {
                    foreach (string str in pieces)
                    {
                        string[] parts = str.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                        if (parts.Length == 3)
                            list.Add(new System.Web.UI.Triplet(parts[0], parts[1], parts[2]));
                    }
                }
            }

            return list;
        }

        public static string _EventMenu_ListingType
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Admin.ToString().ToLower() &&
                            match.Name.ToLower() == "eventmenulistingtype" && match.ValueX != null);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX;

                else
                {
                    config = _Config.AddNewConfig(_Enums.SiteConfigContext.Admin, _Enums.ConfigDataTypes._string, 2000,
                        "EventMenuListingType",
                        @"3 options...showdate, show or month. Show will list as shows with date range. Showdate will list each event individually, Month will list the month as the heading.",
                        "month");

                    return config.ValueX;
                }
            }
        }

        public readonly static string _FeaturedItem_Order_Key = "featureditem_order";
        public static string _FeaturedItem_Order
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Admin.ToString().ToLower() &&
                            match.Name.ToLower() == _FeaturedItem_Order_Key && match.ValueX != null);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX;

                else
                {
                    config = _Config.AddNewConfig(_Enums.SiteConfigContext.Admin, _Enums.ConfigDataTypes._string, 2000,
                        _FeaturedItem_Order_Key,
                        @"Stores a comma separated list that holds the order of featured items. The list is managed in the admin page.",
                        "");

                    return String.Empty;
                }
            }
            set
            {

            }            
        }
        public static bool _Allow_HideShipMethod
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Admin.ToString().ToLower() &&
                            match.Name.ToLower() == "allow_hideshipmethod" && match.ValueX != null &&
                            Utils.Validation.IsBoolean(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return bool.Parse(config.ValueX);

                return false;
            }
        }
        public static bool _WaiveProcFeeOnGCs
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Admin.ToString().ToLower() &&
                            match.Name.ToLower() == "waiveprocfeeongcs" && match.ValueX != null &&
                            Utils.Validation.IsBoolean(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return bool.Parse(config.ValueX);

                return false;
            }
        }
        public static bool _Allow_PurchaseShippingPostSale
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Admin.ToString().ToLower() &&
                            match.Name.ToLower() == "allow_purchaseshippingpostsale" && match.ValueX != null &&
                            Utils.Validation.IsBoolean(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return bool.Parse(config.ValueX);

                return false;
            }
        }
        public static bool _AllSales_OnlineOnly
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Admin.ToString().ToLower() &&
                            match.Name.ToLower() == "allsales_onlineonly" && match.ValueX != null &&
                            Utils.Validation.IsBoolean(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return bool.Parse(config.ValueX);

                return false;
            }
        }
        public static bool _AllowCustomerInitiatedNameChanges
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Admin.ToString().ToLower() &&
                            match.Name.ToLower() == "allowcustomerinitiatednamechanges" && match.ValueX != null &&
                            Utils.Validation.IsBoolean(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return bool.Parse(config.ValueX);

                return false;
            }
        }
        public static bool _MaintenanceMode_On
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Admin.ToString().ToLower() &&
                            match.Name.ToLower() == "maintenancemode_on" && match.ValueX != null &&
                            Utils.Validation.IsBoolean(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return bool.Parse(config.ValueX);

                return true;//if it aint found - dont use 
            }
        }
        public static bool _FAQ_Page_On
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Admin.ToString().ToLower() &&
                            match.Name.ToLower() == "faq_page_on" && match.ValueX != null &&
                            Utils.Validation.IsBoolean(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return bool.Parse(config.ValueX);

                return false;
            }
        }
        public static bool _TwitterWidget_On
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Admin.ToString().ToLower() &&
                            match.Name.ToLower() == "twitterwidget_on" && match.ValueX != null &&
                            Utils.Validation.IsBoolean(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return bool.Parse(config.ValueX);

                return false;
            }
        }
        public static bool _TwitterModule_On
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Admin.ToString().ToLower() &&
                            match.Name.ToLower() == "twittermodule_on" && match.ValueX != null &&
                            Utils.Validation.IsBoolean(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return bool.Parse(config.ValueX);

                return false;
            }
        }
        public static bool _CharityListing_On
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Admin.ToString().ToLower() &&
                            match.Name.ToLower() == "charitylisting_on" && match.ValueX != null &&
                            Utils.Validation.IsBoolean(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return bool.Parse(config.ValueX);

                return false;
            }
        }
        public static bool _Blog_External_On
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Admin.ToString().ToLower() &&
                            match.Name.ToLower() == "blog_external_on" && match.ValueX != null &&
                            Utils.Validation.IsBoolean(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return bool.Parse(config.ValueX);

                return false;
            }
        }
        public static bool _StoreCredit_Active
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Admin.ToString().ToLower() &&
                            match.Name.ToLower() == "storecredit_active" && match.ValueX != null &&
                            Utils.Validation.IsBoolean(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return bool.Parse(config.ValueX);

                return false;
            }
        }
        public static bool _StoreCredit_AllowAdjustment
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Admin.ToString().ToLower() &&
                            match.Name.ToLower() == "storecredit_allowadjustment" && match.ValueX != null &&
                            Utils.Validation.IsBoolean(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return bool.Parse(config.ValueX);

                return false;
            }
        }
        public static string _Blog_External_Url
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Admin.ToString().ToLower() &&
                            match.Name.ToLower() == "blog_external_url" && match.ValueX != null);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX;

                return String.Empty;
            }
        }
        public static string _CC_DeveloperEmail
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Admin.ToString().ToLower() &&
                            match.Name.ToLower() == "cc_developer_email" && match.ValueX != null);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX;

                return String.Empty;
            }
        }
        public static string _Report_DailySales_Recipients
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Admin.ToString().ToLower() &&
                            match.Name.ToLower() == "report_dailysales_recipients" && match.ValueX != null);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX;

                return String.Empty;
            }
        }
        public static _Enums.SiteEntityMode _Site_Entity_Mode
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Admin.ToString().ToLower() &&
                            match.Name.ToLower() == "site_entity_mode" && match.ValueX != null);
                    });
                if (config != null && config.Id > 0)
                    return (_Enums.SiteEntityMode)Enum.Parse(typeof(_Enums.SiteEntityMode), config.ValueX, true);

                throw new Exception("mode not found");
            }
        }
        public static string _Site_Entity_Name
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Admin.ToString().ToLower() &&
                            match.Name.ToLower() == "site_entity_name" && match.ValueX != null);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX;

                return String.Empty;
            }
        }
        public static string _Site_Entity_HomePage
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Admin.ToString().ToLower() &&
                            match.Name.ToLower() == "site_entity_homepage" && match.ValueX != null);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX;

                return String.Empty;
            }
        }
        public static string _Site_Entity_PhysicalAddress
        {
            get
            {
                try
                {
                    SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                        delegate(SiteConfig match)
                        {
                            return (match.Context.ToLower() == _Enums.SiteConfigContext.Admin.ToString().ToLower() &&
                                match.Name.ToLower() == "site_entity_physicaladdress" && match.ValueX != null);
                        });
                    if (config != null && config.Id > 0)
                        return config.ValueX;
                }
                catch(Exception)
                {
                    string appConfigVal = _Config.svc_SitePhysicalAddress ?? string.Empty;
                    return appConfigVal;
                }

                return String.Empty;
            }
        }
        public static string _Site_Entity_WebmasterEmail
        {
            get
            {
               try
                {
                    SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                        delegate(SiteConfig match)
                        {
                            return (match.Context.ToLower() == _Enums.SiteConfigContext.Admin.ToString().ToLower() &&
                                match.Name.ToLower() == "site_entity_webmasteremail" && match.ValueX != null);
                        });
                    if (config != null && config.Id > 0)
                        return config.ValueX;
                }
                catch(Exception)
                {
                    string appConfigVal = _Config.svc_WebmasterEmail ?? string.Empty;
                    return appConfigVal;
                }

                return String.Empty;
            }
        }
        public static int _TTL_Secs_CartItems
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Admin.ToString().ToLower() &&
                            match.Name.ToLower() == "ttl_secs_cartitems" && match.ValueX != null &&
                            Utils.Validation.IsInteger(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return int.Parse(config.ValueX);

                return 6000;
            }
        }
        public static int _TTL_Secs_Extend
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Admin.ToString().ToLower() &&
                            match.Name.ToLower() == "ttl_secs_extend" && match.ValueX != null &&
                            Utils.Validation.IsInteger(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return int.Parse(config.ValueX);

                return 3000;
            }
        }
        public static string _Admin_EmailAddress
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Admin.ToString().ToLower() &&
                            match.Name.ToLower() == "admin_emailaddress" && match.ValueX != null);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX;

                return String.Empty;
            }
        }
        public static string _Merchant_ChargeStatement_Descriptor
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Admin.ToString().ToLower() &&
                            match.Name.ToLower() == "merchant_chargestatement_descriptor" && match.ValueX != null);
                    });
                if (config != null && config.Id > 0)
                    return config.ValueX;

                return String.Empty;
            }
        }
        public static int _InventoryRefreshIntervalSecs
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Admin.ToString().ToLower() &&
                            match.Name.ToLower() == "inventoryrefreshintervalsecs" && match.ValueX != null &&
                            Utils.Validation.IsInteger(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return int.Parse(config.ValueX);

                return 60000;
            }
        }

        #endregion

        #region SITE AND DEVELOPMENT

        //public static bool      _DisplayTestBanner { get { return bool.Parse(ConfigurationManager.AppSettings["stp_DisplayTestBanner"]); } }
        public static DateTime  _ApplicationStartDate { get { return DateTime.Parse(ConfigurationManager.AppSettings["stp_ApplicationStartDate"]); } }
        
        public static string    APPLICATION_NAME { get { return ConfigurationManager.AppSettings["stp_ApplicationName"]; } }
        public static string    _applicationName { get { return _Config.APPLICATION_NAME; } }//keep for legacy purposes

        public static string    _DomainName { get { return ConfigurationManager.AppSettings["stp_DomainName"]; } }
        public static int       _DataExpiryMins { get { return int.Parse(ConfigurationManager.AppSettings["stp_DataExpiryMins"]); } }
        //public static bool      _AjaxTestMode { get { return bool.Parse(ConfigurationManager.AppSettings["stp_AjaxTestMode"]); } }
        public static bool      _TicketTestMode { get { return bool.Parse(ConfigurationManager.AppSettings["stp_TicketTestMode"]); } }
        public static bool      _AutoLogin { get { return bool.Parse(ConfigurationManager.AppSettings["stp_AutoLogin"]); } }
        public static string    _AutoLoginName { get { return ConfigurationManager.AppSettings["stp_AutoLoginName"]; } }
        public static string    _AutoLoginPass { get { return ConfigurationManager.AppSettings["stp_AutoLoginPass"]; } }
        public static bool      _RefundTestMode { get { return bool.Parse(ConfigurationManager.AppSettings["stp_RefundTestMode"]); } }
        public static string    _SpaceImageFilePath { get { return "/Images/spacer.gif"; } }
        public static string    _ErrorLogPath { get { return ConfigurationManager.AppSettings["stp_ErrorLogPath"]; } }
        public static string    _ErrorLogTitle { get { return ConfigurationManager.AppSettings["stp_ErrorLogTitle"]; } }
        public static bool      _ErrorsToDebugger { get { return _Config._DomainName.ToLower().IndexOf("local.") != -1 || _Config._DomainName.ToLower().IndexOf("localhost") != -1; } }
        public static bool      _LogRenderError { get { return bool.Parse(ConfigurationManager.AppSettings["stp_LogRenderError"]); } }		
        public static string    _MappedRootDirectory { get { return ConfigurationManager.AppSettings["stp_MappedRootDirectory"]; } }
        public static string    _VirtualResourceDir { get { return ConfigurationManager.AppSettings["stp_VirtualResourceDir"]; } }
        public static string    _ShowDependencyFile { get { 

            string dep = System.Web.HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["stp_ShowDependencyFile"]); 
            return dep; } }
        public static string _ApiDependencyFile
        {
            get
            {

                string dep = System.Web.HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["stp_ApiDependencyFile"]);
                return dep;
            }
        }
       
        public static bool      _Coupons_Active
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Flow.ToString().ToLower() &&
                            match.Name.ToLower() == "coupons_active" && match.ValueX != null &&
                            Utils.Validation.IsBoolean(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return bool.Parse(config.ValueX);

                return false;
            }
        }
        public static bool      _Donations_Active
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                    delegate(SiteConfig match)
                    {
                        return (match.Context.ToLower() == _Enums.SiteConfigContext.Flow.ToString().ToLower() &&
                            match.Name.ToLower() == "donations_active" && match.ValueX != null &&
                            Utils.Validation.IsBoolean(match.ValueX));
                    });
                if (config != null && config.Id > 0)
                    return bool.Parse(config.ValueX);

                return false;
            }
        }
        #endregion

        #region EMAIL
        
        public static string _CustomerInquiryTemplate { get { return ConfigurationManager.AppSettings["stp_CustomerInquiryTemplate"]; } }
        public static string _ForgotPasswordTemplate { get { return ConfigurationManager.AppSettings["stp_ForgotPasswordTemplate"]; } }

        #endregion

        #region UPS & USPS

        public static string    _UPS_AccessKey { get { return ConfigurationManager.AppSettings["stp_UPS_AccessKey"]; } }
        public static string    _UPS_ServiceRates_Url { get { return ConfigurationManager.AppSettings["stp_UPS_ServiceRates_Url"]; } }
        public static string    _UPS_Tracking_Url { get { return ConfigurationManager.AppSettings["stp_UPS_Tracking_Url"]; } }
        public static string    _UPS_AccountNum { get { return ConfigurationManager.AppSettings["stp_UPS_AccountNum"]; } }
        public static string    _UPS_UserId { get { return ConfigurationManager.AppSettings["stp_UPS_UserId"]; } }
        public static string    _UPS_UserPass { get { return ConfigurationManager.AppSettings["stp_UPS_UserPass"]; } }
        public static string    _UPS_OriginZip { get { return ConfigurationManager.AppSettings["stp_UPS_OriginZip"]; } }
        public static string    _UPS_OriginCity { get { return ConfigurationManager.AppSettings["stp_UPS_OriginCity"]; } }
        public static string    _UPS_OriginState { get { return ConfigurationManager.AppSettings["stp_UPS_OriginState"]; } }
        public static string    _UPS_OriginCountryCode { get { return ConfigurationManager.AppSettings["stp_UPS_OriginCountryCode"]; } }

        public static bool      _USPS_Enabled 
        { 
            get 
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                   delegate(SiteConfig match)
                   {
                       return (match.Context.ToLower() == _Enums.SiteConfigContext.Ship.ToString().ToLower() &&
                           match.Name.ToLower() == "usps_enable" && match.ValueX != null &&
                           Utils.Validation.IsBoolean(match.ValueX));
                   });
                if (config != null && config.Id > 0)
                    return bool.Parse(config.ValueX);

                return false;
            } 
        }
        public static string    _USPS_UserId { get { return ConfigurationManager.AppSettings["stp_USPS_UserId"]; } }
        public static string    _USPS_Password { get { return ConfigurationManager.AppSettings["stp_USPS_Password"]; } }
        public static string    _USPS_WebUrl { get { return ConfigurationManager.AppSettings["stp_USPS_WebUrl"]; } }

        public static bool _UPS_Enabled
        {
            get
            {
                SiteConfig config = _Lookits.SiteConfigs.GetList().Find(
                   delegate(SiteConfig match)
                   {
                       return (match.Context.ToLower() == _Enums.SiteConfigContext.Ship.ToString().ToLower() &&
                           match.Name.ToLower() == "ups_enable" && match.ValueX != null &&
                           Utils.Validation.IsBoolean(match.ValueX));
                   });
                if (config != null && config.Id > 0)
                    return bool.Parse(config.ValueX);
                else
                {
                    config = _Config.AddNewConfig(_Enums.SiteConfigContext.Ship, _Enums.ConfigDataTypes._boolean, 5,
                        "UPS_Enable",
                        @"Enable UPS for shipping.",
                        "true");
                }

                return bool.Parse(config.ValueX);
            }
        }


        #endregion

        #region SALES AND ORDERFLOW
        
        public static readonly string    _BarCode39Path = "/Store/Fonts/FREE3OF9.TTF";

        #endregion

        #region AUTHORIZE_NET

        public static bool      _ShuntAuthNet
        {
            get
            {
                string setting = ConfigurationManager.AppSettings["stp_ShuntAuthNet"];
                if (setting != null)
                {
                    try
                    {
                        return bool.Parse(setting);

                    }
                    catch (Exception) { }
                }

                return false;
            }
        }

        public static string	_AuthorizeNetPaymentUrl	{ get { return ConfigurationManager.AppSettings["stp_AuthorizeNetPaymentUrl"]; } }
        public static string	_AuthorizeNetLogin		{ get { return ConfigurationManager.AppSettings["stp_AuthorizeNetLogin"]; } }
        public static string	_AuthorizeNetPassword	{ get { return ConfigurationManager.AppSettings["stp_AuthorizeNetPassword"]; } }
        public static string	_AuthorizeNetTxKey		{ get { return ConfigurationManager.AppSettings["stp_AuthorizeNetTxKey"]; } }

        public static string	_AuthorizeNetTestMode	{ get { return ConfigurationManager.AppSettings["stp_AuthorizeNetTestMode"]; } }
        public static bool      _AuthNetTestResult { get { return bool.Parse(ConfigurationManager.AppSettings["stp_AuthNetTestResult"]); } }
        public static string    _AuthorizeNetMD5HashValue { get { return ConfigurationManager.AppSettings["stp_AuthorizeNetMD5HashValue"]; } }
        public static int       _AuthorizeNetDuplicateSeconds { get { return int.Parse(ConfigurationManager.AppSettings["stp_AuthorizeNetDuplicateSeconds"]); } }
        public static string	_AuthorizeNetLogPath		{ get { return ConfigurationManager.AppSettings["stp_AuthorizeNetLogPath"]; } }

        #endregion

        #region MESSAGES

        public static string _ErrorPageMessage { get { return ConfigurationManager.AppSettings["stp_ErrorPageMessage"]; } }
        
        #endregion

        #region DISPLAY
        
        public static string _ThemeFolder { get { return ConfigurationManager.AppSettings["stp_ThemeFolder"]; } }

        private static string zImageUIPath = null;
        private static string Image_UI_Path { get { if (zImageUIPath == null) zImageUIPath = string.Format("/{0}/Images/UI/", _Config._VirtualResourceDir); return zImageUIPath; } }

        #endregion

        #region IMAGES & MP3s

        public static bool      _UseRemoteImages { get { return bool.Parse(ConfigurationManager.AppSettings["stp_UseRemoteImages"]); } }        

        #endregion
    }
}
