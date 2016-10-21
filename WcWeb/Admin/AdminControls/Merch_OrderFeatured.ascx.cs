using System;
using System.Data;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Web.Services;

using Wcss;

namespace WillCallWeb.Admin.AdminControls
{ 
    public partial class Merch_OrderFeatured : BaseControl
    {
        private System.Text.StringBuilder sb = new System.Text.StringBuilder();
        protected string MaxItems = string.Empty;
        private List<int> _list = null; 
        private SiteConfig _cfg = null;
        private MerchCollection _basecoll = null;

        protected SiteConfig cfg
        {
            get
            {
                if (_cfg == null)
                {
                    _cfg = _ContextBase.GetFeatureOrderConfig();

                    //init
                    if (_cfg == null)
                    {
                        string val = _Config._FeaturedItem_Order;
                        _cfg = _ContextBase.GetFeatureOrderConfig();
                        _Lookits.RefreshLookup(_Enums.LookupTableNames.SiteConfigs.ToString());
                    }
                }

                return _cfg;
            }
        }
        
        protected MerchCollection BaseColl
        {
            get
            {
                if (_basecoll == null)
                {
                    string sql = "SELECT * FROM [Merch] m WHERE m.[ApplicationId] = @appId AND m.[tParentListing] IS NULL AND m.[bActive] = 1 AND m.[bFeaturedItem] = 1 AND m.[bInternalOnly] = 0 ORDER BY m.[Name] ";

                    SubSonic.QueryCommand cmd = new SubSonic.QueryCommand(sql, SubSonic.DataService.Provider.Name);
                    cmd.Parameters.Add("@appId", _Config.APPLICATION_ID, DbType.Guid);

                    _basecoll = new MerchCollection();
                    _basecoll.LoadAndCloseReader(SubSonic.DataService.GetReader(cmd));
                }

                return _basecoll;
            }
        }

        public List<int> OrderList
        {
            get
            {
                if (_list == null)
                {
                    _list = new List<int>();

                    //erase any settings if there are no featured items
                    if(BaseColl.Count == 0 && cfg.ValueX.Trim().Length > 0)
                    {   
                        cfg.ValueX = string.Empty;
                        cfg.Save();
                    }
                    else if (cfg.ValueX.Trim().Length == 0)
                    {
                        //this will handle both cases of no featured and a list of featured

                        //init the list and add items by alpha
                        //if there are no items - make no changes
                        //note that base coll is ordered by name
                        foreach (Merch m in BaseColl)
                            _list.Add(m.Id);

                        cfg.ValueX = System.Text.RegularExpressions.Regex.Replace(Utils.ParseHelper.ExtractListValueString(_list, ','), @"\s+", "");
                        cfg.Save();
                    }
                    else if (cfg.ValueX.Trim().Length > 0 && BaseColl.Count > 0)
                    {
                        //track changes
                        string originalOrder = cfg.ValueX.Trim();

                        //we need to ensure - here at init time - that the basecoll items are contained within the string
                        //and that the string does not contain items that are not featured any longer
                        _list.AddRange(Utils.ParseHelper.StringToList_Int(cfg.ValueX.Trim(), ','));

                        List<int> itemsToRemove = new List<int>();

                        foreach (int i in _list)
                        {
                            //get the matching merch from the basecoll
                            Merch m = BaseColl.GetList().Find(delegate(Merch match) { return (match.Id == i); });
                            //if it is not found
                            //if it is not featured
                            //add its id to the remove list
                            if (m == null || m.Id < 10000 || (!m.IsFeaturedItem))
                                itemsToRemove.Add(i);
                        }

                        //remove the unfound/unfeatured items
                        if (itemsToRemove.Count > 0)
                            foreach (int i in itemsToRemove)
                                _list.Remove(i);

                        foreach (Merch m in BaseColl)
                        {
                            //if the merch id is not in the list
                            //add it to the list
                            //we can do this in this order - add new items by alpha - default sort order
                            if (!_list.Contains(m.Id))
                                _list.Add(m.Id);
                        }


                        //stringify the list
                        //if it does not match - then save to config
                        string updatedOrder = System.Text.RegularExpressions.Regex.Replace(Utils.ParseHelper.ExtractListValueString(_list, ','), @"\s+", "");

                        if (originalOrder != updatedOrder)
                        {
                            cfg.ValueX = updatedOrder;
                            cfg.Save();
                        }

                    }
                }

                //list is now prepared for display
                return _list;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                litDisplay.DataBind();
            }
        }
        
        protected void txtMax_DataBind(object sender, EventArgs e)
        {
            TextBox txt = (TextBox)sender;
            txt.Text = _Config._Items_FeaturedToDisplay.ToString();
            MaxItems = txt.Text;
        }

        protected void litDisplay_DataBinding(object sender, EventArgs e)
        {
            txtMax.DataBind();

            Literal lit = (Literal)sender;
            sb.Length = 0;
            int row = 1;
            int max = _Config._Items_FeaturedToDisplay;

            if (OrderList.Count > 0)
            {
                sb.AppendLine("<div id=\"fetord-wrapper\">");
                sb.AppendLine("<ul>");

                foreach (int i in OrderList)
                {
                    Merch m = BaseColl.GetList().Find(delegate(Merch match) { return (match.Id == i); });

                    sb.AppendFormat("<li id=\"fet_{0}\"{1}><span class=\"fetrow\">{2}</span><span class=\"fetid\">{0}</span> {3}</li>", 
                        i.ToString(),
                        (max != 0 && row > max ) ? string.Format(" class=\"{0}\"", "over-quota") : string.Empty, 
                        row.ToString(), 
                        m.Name);
                    sb.AppendLine();

                    row++;
                }

                sb.AppendLine("</ul>");
                sb.AppendLine("</div>");
            }

            lit.Text = sb.ToString();
        }
        
        /// <summary>
        /// resets order to alpha
        /// </summary>
        protected void btnReset_Click(object sender, EventArgs e)
        {
            cfg.ValueX = string.Empty;
            cfg.Save();
            _cfg = null;
            _list = null;

            //rebind
            litDisplay.DataBind();
        }

        protected void btnMax_Click(object sender, EventArgs e)
        {
            string txt = txtMax.Text.Trim();
            int val = 0;

            //validate
            string errors = string.Empty;
            if (txt.Length == 0)
                errors = string.Format("<li>Limit is required.</li>");
            else if(!Utils.Validation.IsInteger(txt))
                errors = string.Format("<li>Limit must be numeric.</li>");
            else if (Utils.Validation.IsInteger(txt))
            {
                val = int.Parse(txt);

                if(val < 0 || val > 1000)
                    errors = string.Format("<li>Limit must be greater than zero and less than 1000.</li>");
            }

            if (errors.Trim().Length > 0)
            {
                ValidationSummary1.ShowSummary = true;
                ValidationSummary1.HeaderText = errors;
                return;
            }

            SiteConfig cfg = _Lookits.SiteConfigs.GetList().Find(delegate(SiteConfig match)
            {
                return (match.ApplicationId.ToString() == _Config.APPLICATION_ID.ToString() &&
                match.Context == _Enums.SiteConfigContext.Flow.ToString() && match.Name.ToLower() == "items_featuredtodisplay");
            });
            cfg.ValueX = val.ToString();
            cfg.Save();

            _Lookits.RefreshLookup(_Enums.SiteConfigContext.Admin.ToString());

            //rebind
            litDisplay.DataBind();
        }
}
}