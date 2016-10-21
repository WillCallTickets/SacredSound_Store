using System;
using System.Web;
using System.Web.Caching;
using System.Data;
using System.Collections.Generic;
using System.Threading;
using System.Linq;

namespace Wcss
{
    public class _ContextBase
    {
        protected HttpRequest Request;

        /// <summary>
        /// 100329 - Is this correct? Or should this be a static declaration assigned to the StateManager.Instance?
        /// I realize that it is assigned in the constructor - but is this proper? Not sure why the commented out portion is there
        /// </summary>
        protected System.Web.Caching.Cache Cache;// = Utils.StateManager.Instance.Cache;

        public _ContextBase()
        {
            Request = Utils.StateManager.Instance.CurrentContext.Request;
            Cache = Utils.StateManager.Instance.Cache;                     
        }

        #region Api Specific
        
        public CacheDependency apiDependency;
        public System.Web.Caching.CacheItemRemovedCallback ApiCacheFileChanged = null;
        
        /// <summary>
        /// When called, triggers an update to the file dependency which in turn will start the cache refresh/update
        /// </summary>
        public void ResetApiDependency(string userName)
        {
            Utils.FileLoader.SaveToFile(_Config._ApiDependencyFile,
                string.Format("{0}{1} - {2}", Environment.NewLine, DateTime.Now.ToLongTimeString(), userName));
        }
        
        /// <summary>
        /// Essentially a dummy variable that handles the dependency file and subsequent cache refresh
        /// </summary>
        public long Api_PublishDate
        {
            get
            {
                if (Cache["Api_PublishDate"] == null)
                {
                    apiDependency = new System.Web.Caching.CacheDependency(_Config._ApiDependencyFile);
                    ApiCacheFileChanged = new System.Web.Caching.CacheItemRemovedCallback(Api_PublishCallback);

                    Cache.Insert("Api_PublishDate", 
                        DateTime.Now.Ticks,
                        apiDependency, 
                        System.Web.Caching.Cache.NoAbsoluteExpiration,
                        System.Web.Caching.Cache.NoSlidingExpiration,
                        System.Web.Caching.CacheItemPriority.Default,
                        ApiCacheFileChanged);
                }

                return (long)Cache["Api_PublishDate"];
            }
            set
            {
                Cache["Api_PublishDate"] = value;
            }
        }

        /// <summary>
        /// This will reset the cache for this (API) process
        /// Currently removes all base cache objects
        /// TODO: force updateof newly updated objects only
        /// </summary>
        public void Api_PublishCallback(String k, Object v, CacheItemRemovedReason r)
        {
            API_RemoveCacheObjects();
            RemoveAllAPICacheObjects(null);
        }

        public virtual void API_RemoveCacheObjects()
        {
            string s = "l";
        }

        /// <summary>
        /// previously publish
        /// </summary>
        public void RemoveAllAPICacheObjects(string userName)
        {
            Utils.StateManager.ResetWebsiteCache();
            _Error.LogPublishEvent(DateTime.Now, _Enums.PublishEvent.Publish, (userName == null) ? "Api Reset" : userName);
        }
        

        #endregion

        #region CacheControl
        
        public void PublishFromAdmin()
        {
            ResetCache();
        }

        /// <summary>
        /// Do not call this from the API
        /// </summary>
        private void ResetCache()
        {
            //log it - this is instrumental in troubleshooting the cause of site errors/issues
            string userName = HttpContext.Current.User.Identity.Name;
            _Error.LogPublishEvent(DateTime.Now, _Enums.PublishEvent.Publish, userName);

            Utils.StateManager.ResetWebsiteCache();            

            Cache.Remove("CacheDependency_JsonUpcoming");
            Cache.Remove("FirstShowEver");//TODO: move mgmt of this object to application start

            //Todo: does this need to be reset via the api call?
            Cache.Remove("Merch_Requires_18Over_Acknowledge_List");

            //dont call in api - would cause infinite loop
            ResetApiDependency(userName);
        }

        #endregion

        /// <summary>
        /// gives us a property to track how far back in history our calendars should go.
        /// TODO: set this to init in application OnStart
        /// </summary>
        public Show FirstShowEver
        {
            get
            {
                if (Cache["FirstShowEver"] == null)
                {
                    ShowCollection _showCollection = new ShowCollection();
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();

                    sb.Append("SELECT TOP 1 * FROM [Show] s WHERE s.[ApplicationId] = @appId AND s.[bActive] = 1 ORDER BY s.[Name] ASC ");

                    SubSonic.QueryCommand cmd = new SubSonic.QueryCommand(sb.ToString(), SubSonic.DataService.Provider.Name);
                    cmd.Parameters.Add("@appId", _Config.APPLICATION_ID, DbType.Guid);

                    _showCollection.LoadAndCloseReader(SubSonic.DataService.GetReader(cmd));

                    if (_showCollection.Count > 0)
                        Cache.Insert("FirstShowEver", _showCollection[0]);
                }

                return (Show)Cache["FirstShowEver"];
            }
        }

        
        /// <summary>
        ///This collection should not be used in admin
        /// </summary>
        public List<int> Merch_Requires_18Over_Acknowledge_List
        {
            get
            {
                if (Cache["Merch_Requires_18Over_Acknowledge_List"] == null)
                {
                    List<int> list = new List<int>();
                    string configVal = _Config._Merch_Requires_18Over_Ack_List.ValueX;

                    if (configVal != null && configVal.Trim().Length > 0)
                        list.AddRange(_Config._Merch_Requires_18Over_Ack_List.ValueX.Split(',').Select<string, int>(int.Parse).ToList());

                    Cache.Insert("Merch_Requires_18Over_Acknowledge_List", list, null,
                        DateTime.Now.AddMinutes(1), System.Web.Caching.Cache.NoSlidingExpiration);
                }

                return (List<int>)Cache["Merch_Requires_18Over_Acknowledge_List"];
            }
        }

        /// <summary>
        /// You will probably need to filter this by to get only currently active shows
        /// Note that filtered objects are in a diff namespace due to their reliance on Linq
        /// </summary>
        public ShowDateCollection SaleShowDates_All
        {
            get
            {
                if (Cache["Sale_ShowDates_All"] == null)
                {
                    //allow some time for the shows to be displayed
                    //gets shows with a date greater than now(specified) that are active
                    //add 8 hours so show will display after doors and for rest of evening
                    //this returns active shows
                    using (IDataReader rdr = SPs.TxGetSaleShowDates(_Config.APPLICATION_ID, _Config.SHOWOFFSETDATE.ToString("yyyy/MM/dd hh:mmtt")).GetReader())
                    {
                        ShowDateCollection showDates = new ShowDateCollection();
                        showDates.LoadAndCloseReader(rdr);

                        if (showDates.Count > 0)
                        {
                            showDates.Sort("DtDateOfShow", true);

                            Cache.Insert("Sale_ShowDates_All", showDates, 
                                new CacheDependency(_Config._ShowDependencyFile), 
                                DateTime.Now.AddMinutes(_Config._DataExpiryMins),
                                System.Web.Caching.Cache.NoSlidingExpiration,
                                CacheItemPriority.Normal, 
                                Utils.StateManager.OnCacheItemRemoved);

                            //reset active list                            
                            Cache.Remove("Sale_Tickets");
                            Cache.Remove("ShowDatesAtBeginningOfMonth");//do this here and not at publish to keep in sync with Sale_ShowDates_All
                        }
                    }
                }

                return (ShowDateCollection)Cache["Sale_ShowDates_All"];
            }
        }

        /// <summary>
        /// Returns displayable tickets gathered from OnSaleShows. Not to be used for calculations of ticket inventory due to it being cached.
        /// Also, not to be used for anything in the order flow - only for display of show information
        /// </summary>
        public ShowTicketCollection SaleTickets
        {
            get
            {
                if (Utils.StateManager.Instance.Get("Sale_Tickets", Utils.ContextState.Cache) == null)
                {
                    ShowTicketCollection tickets = new ShowTicketCollection();

                    if (SaleShowDates_All != null)
                    {
                        foreach (ShowDate sd in SaleShowDates_All)
                        {
                            foreach (ShowTicket st in sd.GetDisplayableTickets(_Enums.VendorTypes.online, null, true, false, tickets))
                                tickets.Add(st);
                        }

                        if (tickets.Count > 1)
                            tickets.Sort("Id", true);

                    }

                    Utils.StateManager.Instance.Cache.Insert("Sale_Tickets", tickets, null, DateTime.MaxValue, System.Web.Caching.Cache.NoSlidingExpiration);
                }

                return (ShowTicketCollection)Utils.StateManager.Instance.Get("Sale_Tickets", Utils.ContextState.Cache);
            }
        }
        
        /// <summary>
        /// A list of all the merch items that are active, sorted by name.
        /// </summary>
        public MerchCollection SaleMerch
        {
            get
            {
                if (Cache["Sale_Merch"] == null)
                {
                    //get parents that are active
                    //add to that collection - the child items of active parents
                    string sql = "CREATE TABLE #tmpMerchParents (Idx int); ";
                    sql += "INSERT #tmpMerchParents(Idx) SELECT m.[Id] as 'Idx' FROM [Merch] m WHERE m.[ApplicationId] = @appId AND m.[tParentListing] IS NULL AND m.[bActive] = 1 ";
                    sql += "SELECT m.* FROM [#tmpMerchParents] tp, [Merch] m WHERE m.[Id] = tp.[Idx] UNION ";
                    //get the matching children
                    sql += "SELECT * FROM [Merch] m WHERE  m.[ApplicationId] = @appId AND m.[tParentListing] IS NOT NULL AND m.[bActive] = 1 ";
                    sql += "AND m.[tParentListing] IN (SELECT [Idx] FROM #tmpMerchParents) ORDER BY NAME; ";

                    SubSonic.QueryCommand cmd = new SubSonic.QueryCommand(sql, SubSonic.DataService.Provider.Name);
                    cmd.Parameters.Add("@appId", _Config.APPLICATION_ID, DbType.Guid);

                    MerchCollection merch = new MerchCollection();
                    merch.LoadAndCloseReader(SubSonic.DataService.GetReader(cmd));

                    Cache.Insert("Sale_Merch", merch, null, DateTime.Now.AddMinutes(75), 
                        System.Web.Caching.Cache.NoSlidingExpiration);
                }

                return (MerchCollection)Cache["Sale_Merch"];
            }
        }
        
        public static SiteConfig GetFeatureOrderConfig()
        {
            return _Lookits.SiteConfigs.GetList().Find(delegate(SiteConfig match)
            {
                return (match.ApplicationId == _Config.APPLICATION_ID &&
                  match.Context == _Enums.SiteConfigContext.Admin.ToString() &&
                  match.Name.ToLower() == _Config._FeaturedItem_Order_Key.ToLower());
            });
        }
        
        public MerchCollection FeaturedMerchListing
        {
            get
            {
                if (Utils.StateManager.Instance.Cache["FeaturedMerchItems"] == null)
                {
                    MerchCollection returnColl = new MerchCollection();

                    SiteConfig _cfg = _ContextBase.GetFeatureOrderConfig();
                    if (_cfg != null && _cfg.ValueX.TrimEnd().Length > 0)
                    {
                        //make a list from config
                        System.Collections.Generic.List<int> _list = new System.Collections.Generic.List<int>();
                        _list.AddRange(Utils.ParseHelper.StringToList_Int(_cfg.ValueX.Trim(), ','));
                        
                        //loop thru and construct featured
                        foreach (int i in _list)
                        {
                            Merch m = this.SaleMerch.GetList().Find(delegate(Merch match) {
                                return (match.Id == i && match.IsParent && match.IsFeaturedItem && match.IsActive && (!match.IsInternalOnly));
                            });
                            if(m != null)
                                returnColl.Add(m);
                        }
                    }
                    else
                    {
                        returnColl.AddRange(this.SaleMerch.GetList().FindAll(delegate(Merch match) { 
                            return (match.IsParent && match.IsFeaturedItem && match.IsActive && (!match.IsInternalOnly)); }));
                        if (returnColl.Count > 1)
                            returnColl.Sort("Name", true);
                    }

                    //****ALLOW CLIENT TO LIMIT COLLECTION SIZE!!!!
                    //limit collection to number of featured items specified
                    //int maxListings = _Config._Items_FeaturedToDisplay;
                    //if (maxListings != 0)
                    //    returnColl.Take(maxListings);

                    Utils.StateManager.Instance.Cache.Insert("FeaturedMerchItems", returnColl);
                }

                return (MerchCollection)Utils.StateManager.Instance.Cache["FeaturedMerchItems"];
            }
        }
        
        public MerchCollection Merch_GoingFast
        {
            get
            {
                if (Cache["Merch_GoingFast"] == null)
                {
                    MerchCollection coll = new MerchCollection();
                    coll.AddRange(SaleMerch.GetList().FindAll(delegate(Merch match) { return (match.IsParent && match.IsDisplayable && match.AvailableChildren > 0); }));
                    if (coll.Count > 1)
                        coll.GetList().Sort(delegate(Merch x, Merch y) { return (x.AvailableChildren.CompareTo(y.AvailableChildren)); });//going fast have least amount left

                    int max = _Config._Items_GoingFastToDisplay;

                    if (max >= 0 && coll.Count > 0)
                        if (coll.Count > max)
                            coll.GetList().RemoveRange(max, coll.Count - max);

                    Cache.Insert("Merch_GoingFast", coll, null, DateTime.Now.AddMinutes(75),
                        System.Web.Caching.Cache.NoSlidingExpiration);
                }

                return (MerchCollection)Cache["Merch_GoingFast"];
            }
        }
        
        public MerchCollection Merch_BestSellers
        {
            get
            {
                if (Cache["Merch_BestSellers"] == null)
                {
                    MerchCollection coll = new MerchCollection();
                    coll.AddRange(SaleMerch.GetList().FindAll(delegate(Merch match) { return (match.IsParent && match.IsDisplayable && match.AvailableChildren > 0 && match.SoldChildren > 0); }));
                    if (coll.Count > 1)
                        coll.GetList().Sort(delegate(Merch y, Merch x) { return (x.SoldChildren.CompareTo(y.SoldChildren)); });//reverse order

                    int max = _Config._Items_BestSellersToDisplay;

                    if (max >= 0 && coll.Count > 0)
                        if (coll.Count > max)
                            coll.GetList().RemoveRange(max, coll.Count - max);

                    Cache.Insert("Merch_BestSellers", coll, null, DateTime.Now.AddMinutes(75),
                        System.Web.Caching.Cache.NoSlidingExpiration);
                }

                return (MerchCollection)Cache["Merch_BestSellers"];
            }
        }

        /// <summary>
        /// A list of ALL the merch items sorted by name. Includes non-active items.
        /// </summary>
        public MerchCollection MerchParents
        {
            get
            {
                if (Cache["Merch_Parents"] == null)
                {
                    MerchCollection merch = new MerchCollection();
                    merch.Where(Merch.Columns.ApplicationId, _Config.APPLICATION_ID);
                    merch.Where("tParentListing", null);
                    merch.Load();
                    if (merch.Count > 0)
                    {
                        if (merch.Count > 1)
                            merch.Sort("Name", true);

                        Cache.Insert("Merch_Parents", merch, null, DateTime.Now.AddMinutes(75),
                            System.Web.Caching.Cache.NoSlidingExpiration);
                    }
                }

                return (MerchCollection)Cache["Merch_Parents"];
            }
            set
            {
                Cache.Remove("Merch_Parents");
            }
        }

        /// <summary>
        /// This method is used for testing conditions that may have changed within an orders lifespan
        /// It is also an attempt (albeit meager) at trying to debug and troubleshoot some concurrency issues
        /// Leave commented out for non-testing scenario
        /// </summary>
        public void MonkeyWrench()
        {
            //string sql = "update showticket set bactive = 0 where id in (10273,10274) ";
            //SubSonic.QueryCommand cmd = new SubSonic.QueryCommand(sql, SubSonic.DataService.Provider.Name);

            //SubSonic.DataService.ExecuteQuery(cmd);

            //Publish();
        }
    }
}
