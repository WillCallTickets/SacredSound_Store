using System;
using System.Text;
using System.Collections.Generic;

namespace Wcss
{
    public partial class _Lookits
    {
        private static System.Web.Caching.Cache _cache
        {
            get
            {
                return Utils.StateManager.Instance.Cache;
            }
        }

        

        #region Collection definitions

        public static Wcss.AgeCollection Ages
        {
            get
            {
                
                if (_cache["Lookup_Ages"] == null)
                {
                    Wcss.AgeCollection _ages = new Wcss.AgeCollection()
                        .Where(Age.Columns.ApplicationId, _Config.APPLICATION_ID)
                        .OrderByAsc(Wcss.Age.Columns.Name.ToString()).Load();

                    _cache.Add("Lookup_Ages", _ages, null, DateTime.MaxValue, TimeSpan.FromDays(2), System.Web.Caching.CacheItemPriority.Normal, null);
                }

                return (Wcss.AgeCollection)_cache["Lookup_Ages"];
            }
        }
        public static Wcss.CharitableListingCollection CharityListings
        {
            get
            {

                if (_cache["Lookup_CharityListings"] == null)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("SELECT listing.* FROM [CharitableListing] listing, [CharitableOrg] org ");
                    sb.Append("WHERE listing.[ApplicationId] = @appId AND listing.[tCharitableOrgId] = org.[Id] AND org.[bActive] = 1 ");
                    sb.Append("ORDER BY listing.[iDisplayOrder] ");

                    SubSonic.QueryCommand cmd = new SubSonic.QueryCommand(sb.ToString(), SubSonic.DataService.Provider.Name);
                    cmd.Parameters.Add("@appId", _Config.APPLICATION_ID, System.Data.DbType.Guid);

                    Wcss.CharitableListingCollection _chars = new Wcss.CharitableListingCollection();
                    _chars.LoadAndCloseReader(SubSonic.DataService.GetReader(cmd));

                    _cache.Add("Lookup_CharityListings", _chars, null, DateTime.MaxValue, TimeSpan.FromDays(2), System.Web.Caching.CacheItemPriority.Normal, null);
                }

                return (Wcss.CharitableListingCollection)_cache["Lookup_CharityListings"];
            }
        }
        public static Wcss.FaqCategorieCollection FaqCategories
        {
            get
            {

                if (_cache["Lookup_FaqCategories"] == null)
                {
                    Wcss.FaqCategorieCollection _faqcats = new Wcss.FaqCategorieCollection()
                        .Where(FaqCategorie.Columns.ApplicationId, _Config.APPLICATION_ID)
                        .OrderByAsc(Wcss.FaqCategorie.Columns.IDisplayOrder.ToString()).Load();

                    _cache.Add("Lookup_FaqCategories", _faqcats, null, DateTime.MaxValue, TimeSpan.FromDays(2), System.Web.Caching.CacheItemPriority.Normal, null);
                }

                return (Wcss.FaqCategorieCollection)_cache["Lookup_FaqCategories"];
            }
        }
        public static Wcss.HeaderImageCollection HeaderImages
        {
            get
            {
                if (_cache["Lookup_HeaderImages"] == null)
                {
                    Wcss.HeaderImageCollection _coll = new Wcss.HeaderImageCollection()
                        .Where(HeaderImage.Columns.ApplicationId, _Config.APPLICATION_ID)
                        .Where(HeaderImage.Columns.BActive, true)
                        .OrderByAsc(Wcss.HeaderImage.Columns.IDisplayOrder.ToString()).Load();

                    _cache.Add("Lookup_HeaderImages", _coll, null, DateTime.MaxValue, TimeSpan.FromDays(2), System.Web.Caching.CacheItemPriority.Normal, null);
                }

                return (Wcss.HeaderImageCollection)_cache["Lookup_HeaderImages"];
            }
        }
        public static Wcss.HintQuestionCollection HintQuestions
        {
            get
            {
                if (_cache["Lookup_HintQuestions"] == null)
                {
                    Wcss.HintQuestionCollection _hintQuestions = new Wcss.HintQuestionCollection()
                        .Where(HintQuestion.Columns.ApplicationId, _Config.APPLICATION_ID)
                        .OrderByAsc(Wcss.HintQuestion.Columns.IDisplayOrder.ToString()).Load();

                    _cache.Add("Lookup_HintQuestions", _hintQuestions, null, DateTime.MaxValue, TimeSpan.FromDays(2), System.Web.Caching.CacheItemPriority.Normal, null);
                }

                return (Wcss.HintQuestionCollection)_cache["Lookup_HintQuestions"];
            }
        }
        public static Wcss.InvoiceFeeCollection InvoiceFees
        {
            get
            {
                if (_cache["Lookup_InvoiceFees"] == null)
                {
                    Wcss.InvoiceFeeCollection _invoiceFees = new Wcss.InvoiceFeeCollection()
                        .Where(InvoiceFee.Columns.ApplicationId, _Config.APPLICATION_ID)
                        .OrderByDesc(Wcss.InvoiceFee.Columns.Id).Load();

                    _cache.Add("Lookup_InvoiceFees", _invoiceFees, null, DateTime.MaxValue, TimeSpan.FromDays(2), System.Web.Caching.CacheItemPriority.Normal, null);
                }

                return (Wcss.InvoiceFeeCollection)_cache["Lookup_InvoiceFees"];
            }
        }
        public static Wcss.MerchBundleCollection MerchBundles
        {
            get
            {
                if (_cache["Lookup_MerchBundles"] == null)
                {
                    Wcss.MerchBundleCollection _coll = new Wcss.MerchBundleCollection();

                    StringBuilder sb = new StringBuilder();

                    sb.Append("CREATE TABLE #tmpBundle ( bunId int ) ");

                    sb.Append("INSERT #tmpBundle(bunId) ");
                    sb.Append("SELECT mb.Id ");
                    sb.Append("FROM [MerchBundle] mb LEFT OUTER JOIN [Merch] m ON m.[Id] = mb.[TMerchId] AND m.[ApplicationId] = @appId ");
                    sb.Append("WHERE mb.[TMerchId] IS NOT NULL AND  ");
                    sb.Append("(m.[bActive] = 1 OR (m.[bActive] = 0 AND m.[dtStamp] > DATEADD(mm, -2, getDate()))) ");
                    sb.Append("UNION ");
                    sb.Append("SELECT mb.Id ");
                    sb.Append("FROM [MerchBundle] mb LEFT OUTER JOIN [ShowTicket] st ON st.[Id] = mb.[TShowTicketId] ");
                    sb.Append("LEFT OUTER JOIN [Show] s ON s.[Id] = st.[TShowId] AND s.[ApplicationId] = @appId ");
                    sb.Append("WHERE mb.[TShowTicketId] IS NOT NULL AND  ");
                    //active and date of show within 2 months
                    sb.Append("DATEADD(mm, 2, dtDateOfShow) > getDate() ");

                    sb.Append("SELECT mb.* FROM [MerchBundle] mb WHERE [ID] IN (SELECT bunId FROM [#tmpBundle]) ");

                    sb.Append("DROP TABLE #tmpBundle ");

                    SubSonic.QueryCommand cmd = new SubSonic.QueryCommand(sb.ToString(), SubSonic.DataService.Provider.Name);
                    cmd.Parameters.Add("@appId", _Config.APPLICATION_ID, System.Data.DbType.Guid);
                    _coll.LoadAndCloseReader(SubSonic.DataService.GetReader(cmd));

                    //initialize bundle items
                    foreach (MerchBundle bundle in _coll)
                        bundle.MerchBundleItemRecords();

                    _cache.Insert("Lookup_MerchBundles", _coll, null, DateTime.MaxValue, TimeSpan.FromDays(2), System.Web.Caching.CacheItemPriority.Normal, null);
                }

                return (Wcss.MerchBundleCollection)_cache["Lookup_MerchBundles"];
            }
        }
        public static Wcss.MerchCategorieCollection MerchCategories
        {
            get
            {
                if (_cache["Lookup_MerchCategories"] == null)
                {
                    Wcss.MerchCategorieCollection _merchCategories = new Wcss.MerchCategorieCollection();

                    foreach (MerchDivision div in MerchDivisions)
                        foreach (MerchCategorie cat in div.MerchCategorieRecords())
                            _merchCategories.Add(cat);

                    if (_merchCategories.Count > 1)
                        _merchCategories.Sort("IDisplayOrder", true);

                    _cache.Insert("Lookup_MerchCategories", _merchCategories, 
                        new System.Web.Caching.CacheDependency(System.Web.HttpContext.Current.Server.MapPath(string.Format("/{0}/DependencyFiles/MerchDepend.txt", _Config._VirtualResourceDir))),
                            DateTime.MaxValue, TimeSpan.FromDays(2), System.Web.Caching.CacheItemPriority.Normal, null);
                }

                return (Wcss.MerchCategorieCollection)_cache["Lookup_MerchCategories"];
            }
        }
        public static Wcss.MerchColorCollection MerchColors
        {
            get
            {
                if (_cache["Lookup_MerchColors"] == null)
                {
                    Wcss.MerchColorCollection _MerchColors = new Wcss.MerchColorCollection()
                        .Where(MerchColor.Columns.ApplicationId, _Config.APPLICATION_ID)
                        .OrderByAsc(Wcss.MerchColor.Columns.IDisplayOrder.ToString()).Load();

                    _cache.Add("Lookup_MerchColors", _MerchColors, null, DateTime.MaxValue, TimeSpan.FromDays(2), System.Web.Caching.CacheItemPriority.Normal, null);
                }

                return (Wcss.MerchColorCollection)_cache["Lookup_MerchColors"];
            }
        }
        public static Wcss.MerchDivisionCollection MerchDivisions
        {
            get
            {
                if (_cache["Lookup_MerchDivisions"] == null)
                {
                    Wcss.MerchDivisionCollection _merchDivisions = new Wcss.MerchDivisionCollection()
                        .Where(MerchDivision.Columns.ApplicationId, _Config.APPLICATION_ID)
                        .OrderByAsc(Wcss.MerchDivision.Columns.IDisplayOrder.ToString()).Load();

                    _cache.Insert("Lookup_MerchDivisions", _merchDivisions, 
                        new System.Web.Caching.CacheDependency(System.Web.HttpContext.Current.Server.MapPath(string.Format("/{0}/DependencyFiles/MerchDepend.txt", _Config._VirtualResourceDir))),
                            DateTime.MaxValue, TimeSpan.FromDays(2), System.Web.Caching.CacheItemPriority.Normal, null);
                }

                return (Wcss.MerchDivisionCollection)_cache["Lookup_MerchDivisions"];
            }
        }
        public static Wcss.MerchSizeCollection MerchSizes
        {
            get
            {
                if (_cache["Lookup_MerchSizes"] == null)
                {
                    Wcss.MerchSizeCollection _merchSizes = new Wcss.MerchSizeCollection()
                        .Where(MerchSize.Columns.ApplicationId, _Config.APPLICATION_ID)
                        .OrderByAsc(Wcss.MerchSize.Columns.IDisplayOrder.ToString()).Load();

                    _cache.Add("Lookup_MerchSizes", _merchSizes, null, DateTime.MaxValue, TimeSpan.FromDays(2), System.Web.Caching.CacheItemPriority.Normal, null);
                }

                return (Wcss.MerchSizeCollection)_cache["Lookup_MerchSizes"];
            }
        }
        public static Wcss.ProductAccessCollection ProductAccessors
        {
            get
            {
                if (_cache["Lookup_ProductAccessors"] == null)
                {
                    Wcss.ProductAccessCollection coll = ProductAccess.Populate_ProductAccess_Lookup(_Config.APPLICATION_ID, 168);

                    _cache.Add("Lookup_ProductAccessors", coll, null, DateTime.MaxValue, TimeSpan.FromDays(7), System.Web.Caching.CacheItemPriority.Normal, null);
                }

                return (Wcss.ProductAccessCollection)_cache["Lookup_ProductAccessors"];
            }
        }
        public static Wcss.SaleRuleCollection SaleRules
        {
            get
            {
                if (_cache["Lookup_SaleRules"] == null)
                {
                    Wcss.SaleRuleCollection _saleRules = new Wcss.SaleRuleCollection()
                        .Where(SaleRule.Columns.ApplicationId, _Config.APPLICATION_ID)
                        .OrderByAsc(Wcss.SaleRule.Columns.IDisplayOrder).Load();

                    _cache.Add("Lookup_SaleRules", _saleRules, null, DateTime.MaxValue, TimeSpan.FromDays(2), System.Web.Caching.CacheItemPriority.Normal, null);
                }

                return (Wcss.SaleRuleCollection)_cache["Lookup_SaleRules"];
            }
        }
        public static Wcss.ServiceChargeCollection ServiceCharges
        {
            get
            {
                if (_cache["Lookup_ServiceCharges"] == null)
                {
                    Wcss.ServiceChargeCollection _coll = new Wcss.ServiceChargeCollection()
                        .Where(ServiceCharge.Columns.ApplicationId, _Config.APPLICATION_ID)
                        .OrderByAsc(Wcss.ServiceCharge.Columns.MMaxValue).Load();

                    _cache.Add("Lookup_ServiceCharges", _coll, null, DateTime.MaxValue, TimeSpan.FromDays(2), System.Web.Caching.CacheItemPriority.Normal, null);
                }

                return (Wcss.ServiceChargeCollection)_cache["Lookup_ServiceCharges"];
            }
        }
        public static Wcss.ShowStatusCollection ShowStatii
        {
            get
            {
                if (_cache["Lookup_ShowStatii"] == null)
                {
                    Wcss.ShowStatusCollection _showStatii = new Wcss.ShowStatusCollection().OrderByAsc(Wcss.ShowStatus.Columns.Name.ToString()).Load();

                    _cache.Add("Lookup_ShowStatii", _showStatii, null, DateTime.MaxValue, TimeSpan.FromDays(2), System.Web.Caching.CacheItemPriority.Normal, null);
                }

                return (Wcss.ShowStatusCollection)_cache["Lookup_ShowStatii"];
            }
        }
        public static Wcss.SiteConfigCollection SiteConfigs
        {
            get
            {
                if (_cache["Lookup_SiteConfigs"] == null)
                {
                    //could be unnecessary - but.... (081102)
                    _cache.Remove("Lookup_SiteConfigs");
                    Wcss.SiteConfigCollection _siteConfigs = new Wcss.SiteConfigCollection()
                        .Where(SiteConfig.Columns.ApplicationId, _Config.APPLICATION_ID)
                        .OrderByAsc(Wcss.SiteConfig.Columns.Context).OrderByAsc(Wcss.SiteConfig.Columns.Name).Load();

                    _cache.Add("Lookup_SiteConfigs", _siteConfigs, null, DateTime.MaxValue, TimeSpan.FromDays(2), System.Web.Caching.CacheItemPriority.Normal, null);
                }

                return (Wcss.SiteConfigCollection)_cache["Lookup_SiteConfigs"];
            }
        }
        public static Wcss.ItemImageCollection MerchImages
        {
            get
            {
                if (_cache["Lookup_MerchImages"] == null)
                {
                    Wcss.ItemImageCollection _images = new Wcss.ItemImageCollection();
                    StringBuilder sb = new StringBuilder();

                    sb.Append("SELECT itm.* FROM [ItemImage] itm, [Merch] merch ");
                    sb.Append("WHERE itm.[tMerchId] = merch.[Id] AND merch.[ApplicationId] = @appId ");
                    sb.Append("ORDER BY itm.[iDisplayOrder]");

                    SubSonic.QueryCommand cmd = new SubSonic.QueryCommand(sb.ToString(), SubSonic.DataService.Provider.Name);
                    cmd.Parameters.Add("@appId", _Config.APPLICATION_ID, System.Data.DbType.Guid);
                    _images.LoadAndCloseReader(SubSonic.DataService.GetReader(cmd));

                    _cache.Add("Lookup_MerchImages", _images, null, DateTime.MaxValue, TimeSpan.FromDays(2), System.Web.Caching.CacheItemPriority.Normal, null);
                }

                return (Wcss.ItemImageCollection)_cache["Lookup_MerchImages"];
            }
        }
        public static Wcss.VendorCollection Vendors
        {
            get
            {
                if (_cache["Lookup_Vendors"] == null)
                {
                    Wcss.VendorCollection _vendors = new Wcss.VendorCollection().Where(Vendor.Columns.ApplicationId, _Config.APPLICATION_ID).Load();
                    if (_vendors.Count > 1)
                        _vendors.Sort("Id", true);
                    _cache.Add("Lookup_Vendors", _vendors, null, DateTime.MaxValue, TimeSpan.FromDays(2), System.Web.Caching.CacheItemPriority.Normal, null);
                }

                return (Wcss.VendorCollection)_cache["Lookup_Vendors"];
            }
        }
        public static Wcss.SubscriptionCollection Subscriptions
        {
            get
            {
                if (_cache["Lookup_Subscriptions"] == null)
                {
                    Wcss.SubscriptionCollection _subs = new Wcss.SubscriptionCollection()
                        .Where(Subscription.Columns.ApplicationId, _Config.APPLICATION_ID)
                        .OrderByDesc(Wcss.Subscription.Columns.DtStamp).Load();

                    _cache.Add("Lookup_Subscriptions", _subs, null, DateTime.MaxValue, TimeSpan.FromDays(2), System.Web.Caching.CacheItemPriority.Normal, null);
                }

                return (Wcss.SubscriptionCollection)_cache["Lookup_Subscriptions"];
            }
        }
        public static Wcss.SalePromotionCollection SalePromotions
        {
            get
            {
                if (_cache["Lookup_SalePromotions"] == null)
                {
                    TimeSpan refreshInterval = TimeSpan.FromDays(2);
                    DateTime dateToRefresh = DateTime.Now.Add(refreshInterval).AddMinutes(-20);

                    SalePromotionCollection promos = new SalePromotionCollection();
                    StringBuilder sb = new StringBuilder();
                    sb.Append("SELECT * FROM [SalePromotion] sp ");
                    sb.Append("WHERE sp.[ApplicationId] = @appId AND (sp.[bActive] IS NULL OR sp.[bActive] = 1) AND ");
                    //allow promotions to start in context. Keep-alive in context
                    sb.Append("(sp.[dtStartDate] IS NULL OR sp.[dtStartDate] < @refresh) AND ");
                    //don't bother with promotions that have ended
                    sb.Append("(sp.[dtEndDate] IS NULL OR sp.[dtEndDate] > @overlap) ");

                    //TODO: add display order to table

                    SubSonic.QueryCommand cmd = new SubSonic.QueryCommand(sb.ToString(), SubSonic.DataService.Provider.Name);
                    cmd.Parameters.Add("@appId", _Config.APPLICATION_ID, System.Data.DbType.Guid);
                    cmd.Parameters.Add("@overlap", DateTime.Now.AddHours(-48), System.Data.DbType.DateTime);
                    cmd.Parameters.Add("@refresh", dateToRefresh, System.Data.DbType.DateTime);
                    promos.LoadAndCloseReader(SubSonic.DataService.GetReader(cmd));

                    _cache.Add("Lookup_SalePromotions", promos, null, DateTime.MaxValue, refreshInterval, System.Web.Caching.CacheItemPriority.Normal, null);
                }

                return (Wcss.SalePromotionCollection)_cache["Lookup_SalePromotions"];
            }
        }
        #endregion

        static _Lookits() { }

        //MAKE PLURAL!!!!!
        //Keep in sync with StateManager and enums
        //public const string[] _LookupTableNames = {"Ages", "CharityListings", //"CartDeals", 
        //    "Employees", "FaqCategories", "FaqItems", //"Genres", 
        //    "HeaderImages",
        //    "HintQuestions", "InvoiceFees", "MerchBundles", "MerchCategories", 
        //    "MerchColors", "MerchDivisions", "MerchSizes", 
        //    "ProductAccessors", 
        //    "SaleRules", "ServiceCharges", "ShowStatii", 
        //    "SiteConfigs", "MerchImages", "Vendors", "Subscriptions", "SalePromotions" };


        #region Methods

        
        
        /// <summary>
        /// Removes all cached keys that are prefixed with the word "Lookups_"
        /// </summary>
        public static void RefreshLookup(string collectionName)
        {
            //only if we have approved the key!
            foreach (string s in Enum.GetNames(typeof(_Enums.LookupTableNames)))
            {
                if (s.ToLower().Equals(collectionName.ToLower()))
                {
                    string key = string.Format("Lookup_{0}", s);
                    object obj = _cache[key];
                    if (obj != null)
                        _cache.Remove(key);
                }
            }
        }

        /// <summary>
        /// This method should be used for all singular refreshes and is the preferred modern method. Do not use if the object has dependencies
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="UserName"></param>
        public static string RefreshLookup(_Enums.LookupTableNames tableName, string userName)
        {
            return RefreshLookup(tableName, userName, true);
        }
        public static string RefreshLookup(_Enums.LookupTableNames tableName, string userName, bool logEvent)
        {
            //only if we have approved the key!
            string status = string.Empty;
            string key = string.Format("Lookup_{0}", tableName.ToString());
            object obj = _cache[key];
            if (obj != null)
            {
                _cache.Remove(key);

                status = " [object removed from cache]";
            }
            else
                status = " [object not in cache]";

            if(logEvent)
                _Error.LogPublishEvent(DateTime.Now, _Enums.PublishEvent.Publish, string.Format("{0} {1}", key, status), userName);

            return status;
        }
        /// <summary>
        /// This method should be used for lookups with dependent collections
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="UserName"></param>
        public static void RefreshLookup(_Enums.LookupTableNames tableName, _Enums.LookupTableNames[] dependentTables, string userName)
        {
            List<string> statii = new List<string>();

            statii.Add(string.Format("Lookup_{0} {1}", tableName, RefreshLookup(tableName, userName, false)));
            statii.Add("[dependencies]");

            foreach(_Enums.LookupTableNames ltm in dependentTables)
                statii.Add(string.Format("Lookup_{0} {1}", tableName, RefreshLookup(ltm, userName, false)));

            _Error.LogPublishEvent(DateTime.Now, _Enums.PublishEvent.Publish, 
                string.Join(Environment.NewLine, statii.ToArray()), 
                userName);
        }

        /// <summary>
        /// Removes all cached keys that are prefixed with the word "Lookups_"
        /// </summary>
        public static void RefreshAll()
        {
            foreach (string s in Enum.GetNames(typeof(_Enums.LookupTableNames)))
            {
                string key = string.Format("Lookup_{0}", s);
                object obj = _cache[key];
                if (obj != null)
                    _cache.Remove(key);
            }
        }
        #endregion
    }
}
