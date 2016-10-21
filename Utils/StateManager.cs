using System;
using System.Web;
using System.Web.UI;
using System.Web.Caching;

namespace Utils
{
    public enum ContextState
    {
        Application, Session, Cache, ViewState
    }

    public sealed class StateManager
    {
        #region Singleton pattern

        private static readonly StateManager obj = new StateManager();

        private System.Web.UI.StateBag ViewState = new StateBag();

        private StateManager() 
        { 
        }

        static StateManager() 
        {
        }

        public static StateManager Instance
        {
            get
            {
                return obj;
            }
        }

        public System.Web.HttpContext CurrentContext
        {
            get { return HttpContext.Current; }
        }

        public System.Web.Caching.Cache Cache
        {
            get { return HttpContext.Current.Cache; }
        }

        public void Add(string key, object data, ContextState state)
        {
            switch (state)
            {
                case ContextState.Application:
                    HttpContext.Current.Application.Add(key, data);
                    break;
                case ContextState.Session:
                    HttpContext.Current.Session.Add(key, data);
                    break;
                case ContextState.Cache:
                    HttpContext.Current.Cache.Insert(key, data);
                    break;
                case ContextState.ViewState: ViewState.Add(key, data);
                    break;
            }
        }

        public void Remove(string key, ContextState state)
        {

            if (HttpContext.Current == null)
            {
                return;
            }

            switch (state)
            {
                case ContextState.Application:
                    if (HttpContext.Current.Application[key] != null)
                        HttpContext.Current.Application.Remove(key);
                    break;
                case ContextState.Session:
                    if (HttpContext.Current.Session[key] != null)
                        HttpContext.Current.Session.Remove(key);
                    break;
                case ContextState.Cache:
                    if (HttpContext.Current.Cache[key] != null)
                        HttpContext.Current.Cache.Remove(key);
                    break;
                case ContextState.ViewState:
                    if (ViewState[key] != null)
                        ViewState.Remove(key);
                    break;
            }
        }

        public object Get(string key, ContextState state)
        {
            switch (state)
            {
                case ContextState.Application:
                    if (HttpContext.Current.Application[key] != null)
                        return HttpContext.Current.Application[key];
                    break;
                case ContextState.Session:
                    if (HttpContext.Current.Session[key] != null)
                        return HttpContext.Current.Session[key];
                    break;
                case ContextState.Cache:
                    //if (HttpContext.Current == null)
                    //{
                    //    if (HttpRuntime.Cache[key] != null)
                    //        return HttpRuntime.Cache[key];
                    //}
                    //else 
                    if (HttpContext.Current.Cache[key] != null)
                        return HttpContext.Current.Cache[key];
                    break;
                case ContextState.ViewState:
                    if (ViewState[key] != null)
                        return ViewState[key];
                    break;
                default: return null;
            }
            return null;
        }

        public int Count(ContextState state)
        {
            switch (state)
            {
                case ContextState.Application:
                    if (HttpContext.Current.Application != null)
                        return HttpContext.Current.Application.Count;
                    break;
                case ContextState.Session:
                    if (HttpContext.Current.Session != null)
                        return HttpContext.Current.Session.Count;
                    break;
                case ContextState.Cache:
                    if (HttpContext.Current.Cache != null)
                        return HttpContext.Current.Cache.Count;
                    break;
                case ContextState.ViewState:
                    if (ViewState != null)
                        return ViewState.Count;
                    break;
                default: return 0;
            }

            return 0;
        }

        #endregion

        #region Cache Objects

        private static System.Web.Caching.CacheItemRemovedCallback CacheItemRemoved = new CacheItemRemovedCallback(OnCacheItemRemoved);

        /// <summary>
        /// Note we only call the lookup reset on the show dates - as the reset website cache calls both sale shows reset and merch reset
        /// TODO: make this work for mutually exclusive resets
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="reason"></param>
        public static void OnCacheItemRemoved(string key, object value, CacheItemRemovedReason reason)
        {
            if (key.Equals("Sale_ShowDates_All"))//now we only remove tickets and merch
            {
                ResetSaleShowsCache();

                ResetLookupCache();
            }
            else if (key.Equals("Sale_Merch"))
            {
                ResetSaleMerchCache();
            }
        }        

        public static void ResetWebsiteCache()
        {
            //this will also cause the lookups to be refreshed as they are chained with sale shows
            StateManager.ResetWebsiteCache(null);
        }
        public static void ResetWebsiteCache(string merchOrShowsOrLookups)
        {
            if (merchOrShowsOrLookups != null)
                merchOrShowsOrLookups = merchOrShowsOrLookups.ToLower();

            if (merchOrShowsOrLookups == null || merchOrShowsOrLookups == "merch")
                StateManager.ResetSaleMerchCache();

            if (merchOrShowsOrLookups == null || merchOrShowsOrLookups == "shows")
                StateManager.ResetSaleShowsCache();

            if (merchOrShowsOrLookups != null && merchOrShowsOrLookups == "lookups")
                StateManager.ResetLookupCache();

            //this is done on removal of Sale_ShowDates_All
            //StateManager.ResetLookupCache();
        }
        private static void ResetSaleShowsCache()
        {
            Utils.StateManager.Instance.Remove("Sale_ShowDates_All", Utils.ContextState.Cache);
            Utils.StateManager.Instance.Remove("Sale_ShowDates_ActiveOnly", Utils.ContextState.Cache);
            Utils.StateManager.Instance.Remove("Sale_Tickets", Utils.ContextState.Cache);
            Utils.StateManager.Instance.Remove("OrderDisplayable_Dates_NoDupe", Utils.ContextState.Cache);            
        }
        private static void ResetSaleMerchCache()
        {
            Utils.StateManager.Instance.Remove("Sale_Merch", Utils.ContextState.Cache);
            //Utils.StateManager.Instance.Remove("Merch_Featured", Utils.ContextState.Cache);
            Utils.StateManager.Instance.Remove("FeaturedMerchItems", Utils.ContextState.Cache);
            Utils.StateManager.Instance.Remove("Merch_GoingFast", Utils.ContextState.Cache);
            Utils.StateManager.Instance.Remove("Merch_BestSellers", Utils.ContextState.Cache);
            Utils.StateManager.Instance.Remove("Merch_Parents", Utils.ContextState.Cache);
        }

        //MAKE PLURAL!!!!!
        //Keep in sync with Wcss._LookupCollection table names and enums
        public static string[] _LookupTableNames = {"Ages", "CharityListings", //"CartDeals", 
            "Employees", "FaqCategories", "FaqItems", //"Genres", 
            "HeaderImages",
            "HintQuestions", "InvoiceFees", "MerchBundles", "MerchCategories", "MerchColors", "MerchDivisions", "MerchSizes", 
            "ProductAccessors",
            "SaleRules", 
            "ServiceCharges", "ShowStatii", "SiteConfigs", "MerchImages", "Vendors", "Subscriptions", "SalePromotions" };

        private static void ResetLookupCache()
        {
            foreach (string s in _LookupTableNames)
                Utils.StateManager.Instance.Remove(string.Format("Lookup_{0}", s), Utils.ContextState.Cache);
        }

        #endregion
    }
}
