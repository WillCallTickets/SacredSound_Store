using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.Services;
using System.Web.UI.WebControls;
using AjaxControlToolkit;

using Wcss;

namespace WillCallWeb
{

    /// <summary>
    /// Summary description for MerchAttribute_Service
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.Web.Script.Services.ScriptService]
    public class MerchAttribute_Svc : System.Web.Services.WebService
    {
        private WillCallWeb.WebContext _ctx;
        protected WillCallWeb.WebContext Ctx { get { return _ctx; } }

        public MerchAttribute_Svc()
        {
            //Uncomment the following line if using designed components 
            //InitializeComponent(); 
            _ctx = new WillCallWeb.WebContext();
        }

        [WebMethod]
        public void IndicateAjaxSelectionsComplete(string chosenValues)
        {
            
        }

        [WebMethod]
        public bool CheckUnattributableInventory(int idx)
        {
            return true;
        }

        /// <summary>
        /// it is worthwhile to note that all children have matching attribs
        /// </summary>
        [WebMethod]
        public CascadingDropDownNameValue[] GetStyles(string knownCategoryValues, string category, string contextKey)
        {
            List<CascadingDropDownNameValue> list = new List<CascadingDropDownNameValue>();
            int idx = (Utils.Validation.IsInteger(contextKey)) ? int.Parse(contextKey) : 0;

            if (idx > 0)
            {
                Merch entity = (Merch)Ctx.SaleMerch.Find(idx);

                //diff prices are keyed off of style

                if (entity != null && entity.Id > 0 && entity.HasChildStyles)
                {
                    foreach (string s in entity.ChildStyleList)
                    {
                        //be sure there is inventory
                        MerchCollection coll = new MerchCollection();
                        coll.AddRange(entity.ChildMerchRecords().GetList().FindAll(delegate(Merch match)
                         { return (match.IsActive && match.Style != null && match.Style.Trim().Length > 0 && match.Style.ToLower() == s.ToLower() && match.Available > 0); }));

                        //this next line is not necessary as style is a top level object
                        //StringDictionary kv = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues);

                        if (coll.Count > 0)
                        {
                            string name = s;

                            //change the name if there are price levels
                            if ((!entity.IsGiftCertificateDelivery) && entity.PriceHasMultipleLevelsInInventory)
                                name = string.Format("{0} ({1})", name, coll[0].PriceListing);

                            list.Add(new CascadingDropDownNameValue(name, s));
                        }
                    }
                }
            }

            return list.ToArray();
        }

        /// <summary>
        /// if no style is specified, then it is treated as a top level list. 
        /// it is worthwhile to note that all children have matching attribs
        /// </summary>
        [WebMethod]
        public CascadingDropDownNameValue[] GetColors(string knownCategoryValues, string category, string contextKey)
        {
            List<CascadingDropDownNameValue> list = new List<CascadingDropDownNameValue>();
            int idx = (Utils.Validation.IsInteger(contextKey)) ? int.Parse(contextKey) : 0;

            if (idx > 0)
            {
                Merch entity = (Merch)Ctx.SaleMerch.Find(idx);

                if (entity != null && entity.Id > 0 && entity.HasChildColors)
                {
                    //establish base collection
                    MerchCollection contextMerch = new MerchCollection();
                    contextMerch.AddRange(entity.ChildMerchRecords().GetList().FindAll(
                            delegate(Merch match) { return (match.IsActive && match.Color != null && match.Color.Trim().Length > 0); }));

                    StringDictionary kv = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues);

                    //remove non-matching styles
                    string style = (kv.ContainsKey("Style")) ? kv["Style"] : string.Empty;
                    if (style.Trim().Length > 0)
                        contextMerch.GetList().RemoveAll(delegate(Merch match)
                        {
                            return (match.Style == null ||
                                (match.Style.Trim().ToLower() != style.Trim().ToLower()));
                        });

                    //use a list to get sorting and contain method to work correctly
                    List<string> coll = new List<string>();

                    //check inventory here
                    foreach (Merch m in contextMerch)
                        if (!coll.Contains(m.Color) && m.Available > 0)
                            coll.Add(m.Color);

                    if (coll.Count > 1)
                        coll.Sort();

                    foreach (string s in coll)
                        list.Add(new CascadingDropDownNameValue(s, s));
                }
            }

            return list.ToArray();
        }

        /// <summary>
        /// if no style is specified, then it is treated as a top level list.
        /// it is worthwhile to note that all children have matching attribs
        /// </summary>
        [WebMethod]
        public CascadingDropDownNameValue[] GetSizes(string knownCategoryValues, string category, string contextKey)
        {
            List<CascadingDropDownNameValue> list = new List<CascadingDropDownNameValue>();
            int idx = (Utils.Validation.IsInteger(contextKey)) ? int.Parse(contextKey) : 0;

            if (idx > 0)
            {
                Merch entity = (Merch)Ctx.SaleMerch.Find(idx);

                //NOTE: heierarchy!!!!
                if (entity != null && entity.Id > 0 && entity.HasChildSizes)
                {
                    //establish base collection
                    MerchCollection contextMerch = new MerchCollection();
                    contextMerch.AddRange(entity.ChildMerchRecords().GetList().FindAll(
                        delegate(Merch match) { return (match.IsActive && match.Size != null && match.Size.Trim().Length > 0); }));

                    StringDictionary kv = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues);

                    //remove non-matching styles
                    string style = (kv.ContainsKey("Style")) ? kv["Style"] : string.Empty;
                    if (style.Trim().Length > 0)
                        contextMerch.GetList().RemoveAll(delegate(Merch match)
                        {
                            return (match.Style == null ||
                                (match.Style.Trim().ToLower() != style.Trim().ToLower()));
                        });

                    //remove non-matching colors
                    string color = (kv.ContainsKey("Color")) ? kv["Color"] : string.Empty;
                    if (color.Trim().Length > 0)
                        contextMerch.GetList().RemoveAll(delegate(Merch match)
                        {
                            return (match.Color == null ||
                                (match.Color.Trim().ToLower() != color.Trim().ToLower()));
                        });

                    //use a list to get sorting and contain method to work correctly
                    List<string> coll = new List<string>();

                    //check inventory here
                    foreach (Merch m in contextMerch)
                        if (!coll.Contains(m.Size) && m.Available > 0)
                            coll.Add(m.Size);

                    List<ListItem> sortedList = new List<ListItem>();
                    foreach (string s in coll)
                    {
                        MerchSize mSize = (MerchSize)_Lookits.MerchSizes.GetList().Find(
                            delegate(MerchSize match)
                            {
                                return (match.Name.Trim().ToLower() == s.ToLower() ||
                                    match.Code.Trim().ToLower() == s.ToLower());
                            });

                        int ordinal = (mSize != null) ? mSize.DisplayOrder : 0;

                        sortedList.Add(new ListItem(s, ordinal.ToString()));
                    }

                    if (sortedList.Count > 1)
                        sortedList.Sort(new Utils.Reflector.CompareEntities<ListItem>(Utils.Reflector.Direction.Ascending, "Value"));

                    foreach (ListItem li in sortedList)
                        list.Add(new CascadingDropDownNameValue(li.Text, li.Text));

                }
            }

            return list.ToArray();
        }
    }
}

