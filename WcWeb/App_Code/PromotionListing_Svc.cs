using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.Services;
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
    public class PromotionListing_Svc : System.Web.Services.WebService
    {
        private WillCallWeb.AdminContext _atx;
        protected WillCallWeb.AdminContext Atx { get { return _atx; } }

        public PromotionListing_Svc()
        {
            //Uncomment the following line if using designed components 
            //InitializeComponent(); 
            _atx = new WillCallWeb.AdminContext();
        }

        [WebMethod]
        public CascadingDropDownNameValue[] GetShipMethodListing(string contextKey)
        {
            List<CascadingDropDownNameValue> list = new List<CascadingDropDownNameValue>();
            bool chosenMethodFound = false;

            list.Add(new CascadingDropDownNameValue("all", "all"));

            foreach (string method in _Enums.ShippingMethods)
            {
                CascadingDropDownNameValue val = new CascadingDropDownNameValue(method, method);

                if (contextKey != null && contextKey == method)
                {
                    val.isDefaultValue = true;
                    chosenMethodFound = true;
                }

                list.Add(val);
            }

            if (contextKey != null && contextKey.Trim().Length > 0 && (!chosenMethodFound))
            {
                CascadingDropDownNameValue notCurrent = new CascadingDropDownNameValue(contextKey, contextKey);
                notCurrent.isDefaultValue = true;
                list.Insert(0, notCurrent);
            }

            return list.ToArray();
        }

        
        /// <summary>
        /// This is for admin only - not for public consumption
        /// </summary>
        /// <param name="contextKey"></param>
        /// <returns></returns>
        [WebMethod]
        public CascadingDropDownNameValue[] GetTicketListing(string contextKey)
        {
            List<CascadingDropDownNameValue> list = new List<CascadingDropDownNameValue>();

            ShowTicket chosenTicket = null;
            bool chosenTicketFound = true;//indicates the selected ticket - is not true if we have no selection yet 

            if (contextKey.Trim().Length > 0)
            {
                chosenTicket = ShowTicket.FetchByID(int.Parse(contextKey));
                chosenTicketFound = false;
            }

            ShowTicketCollection coll = Atx.SaleTickets;
            if(coll.Count > 0)
                coll.Sort("DtDateOfShow", true);

            foreach (ShowTicket st in coll)
            {
                CascadingDropDownNameValue val = new CascadingDropDownNameValue(
                    Utils.ParseHelper.StripHtmlTags(st.DisplayNameWithAttribsAndDescription), st.Id.ToString());

                if (chosenTicket != null && chosenTicket.Id == st.Id)
                {
                    val.isDefaultValue = true;
                    chosenTicketFound = true;
                }

                list.Add(val);
            }

            //add the ticket if it was not in the list
            if (chosenTicket != null && (!chosenTicketFound))
            {
                CascadingDropDownNameValue notCurrent = new CascadingDropDownNameValue(
                    Utils.ParseHelper.StripHtmlTags(chosenTicket.DisplayNameWithAttribsAndDescription), chosenTicket.Id.ToString());
                notCurrent.isDefaultValue = true;
                list.Insert(0, notCurrent);
            }

            return list.ToArray();
        }

        /*
        /// <summary>
        /// This is for admin only - not for public consumption
        /// </summary>
        /// <param name="contextKey"></param>
        /// <returns></returns>
        [WebMethod]
        public CascadingDropDownNameValue[] GetDateListing(string contextKey)
        {
            List<CascadingDropDownNameValue> list = new List<CascadingDropDownNameValue>();

            ShowDate chosenDate = null;
            bool chosenDateFound = true;//indicates the selected ticket - is not true if we have no selection yet 

            if (contextKey.Trim().Length > 0)
            {
                chosenDate = ShowDate.FetchByID(int.Parse(contextKey));
                chosenDateFound = false;
            }

            ShowDateCollection coll = new ShowDateCollection();
            coll.AddRange(Atx.OrderedDisplayable_ShowDates);

            foreach (ShowDate sd in coll)
            {
                string title = string.Format("{0} {1}", sd.Display.Date_NoMarkup_StatusNotFirstNoMarkup_NoTime,
                    sd.ShowRecord.ShowNamePart);

                CascadingDropDownNameValue val = new CascadingDropDownNameValue(title, sd.Id.ToString());

                if (chosenDate != null && chosenDate.Id == sd.Id)
                {
                    val.isDefaultValue = true;
                    chosenDateFound = true;
                }

                list.Add(val);
            }

            //add the ticket if it was not in the list
            if (chosenDate != null && (!chosenDateFound))
            {
                CascadingDropDownNameValue notCurrent = new CascadingDropDownNameValue(chosenDate.Display.Billing, chosenDate.Id.ToString());
                notCurrent.isDefaultValue = true;
                list.Insert(0, notCurrent);
            }

            return list.ToArray();
        }
        */

        /// <summary>
        /// it is worthwhile to note that all children have matching attribs
        /// </summary>
        [WebMethod]
        public CascadingDropDownNameValue[] GetParentListing(string contextKey)
        {
            List<CascadingDropDownNameValue> list = new List<CascadingDropDownNameValue>();

            Merch chosenMerch = null;
            bool chosenMerchFound = true;

            if (contextKey.Trim().Length > 0)
            {
                chosenMerch = Merch.FetchByID(int.Parse(contextKey));
                chosenMerchFound = false;
            }

            foreach (Merch m in Atx.MerchParents)
            {
                CascadingDropDownNameValue val = new CascadingDropDownNameValue(Utils.ParseHelper.StripHtmlTags(m.DisplayNameWithAttribs), m.Id.ToString());
                
                if(chosenMerch != null)
                {
                    int parentSelection = (chosenMerch.IsChild) ? chosenMerch.TParentListing.Value : chosenMerch.Id;
                    if (parentSelection == m.Id)
                    {
                        val.isDefaultValue = true;
                        chosenMerchFound = true;
                    }
                }
                
                list.Add(val);
            }

            if (chosenMerch != null && (!chosenMerchFound))
            {
                CascadingDropDownNameValue notCurrent = new CascadingDropDownNameValue(Utils.ParseHelper.StripHtmlTags(chosenMerch.DisplayNameWithAttribs), chosenMerch.Id.ToString());
                notCurrent.isDefaultValue = true;
                list.Insert(0, notCurrent);
            }
           
            return list.ToArray();
        }

        [WebMethod]
        public CascadingDropDownNameValue[] GetChildListing(string knownCategoryValues, string category, string contextKey)
        {
            List<CascadingDropDownNameValue> list = new List<CascadingDropDownNameValue>();
            StringDictionary knownCategoryValuesDictionary = AjaxControlToolkit.CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues);
            string parentMerchValue = knownCategoryValuesDictionary["parent"];

            try
            {
                if (parentMerchValue != null)
                {
                    int parentIdx = int.Parse(parentMerchValue);
                    if (parentIdx > 0)
                    {
                        Merch selectedParent = Merch.FetchByID(parentIdx);

                        if (selectedParent != null)
                        {
                            //here we ensure that if there is only one choice - we choose all inventory
                            if (selectedParent.ChildMerchRecords_Active.Count > 0)
                            {
                                CascadingDropDownNameValue all = new CascadingDropDownNameValue("Select All Inventory", parentMerchValue);
                                    //(selectedParent.ChildMerchRecords_Active.Count > 1) ? parentMerchValue : selectedParent.ChildMerchRecords_Active[0].Id.ToString());

                                if ((selectedParent.ChildMerchRecords_Active.Count > 1 && contextKey == parentMerchValue) ||
                                    (selectedParent.ChildMerchRecords_Active.Count == 1 && contextKey == selectedParent.ChildMerchRecords_Active[0].Id.ToString()))
                                    all.isDefaultValue = true;

                                list.Add(all);
                            }

                            if (selectedParent.ChildMerchRecords_Active.Count > 1)
                            {
                                foreach (Merch m in selectedParent.ChildMerchRecords())
                                {
                                    CascadingDropDownNameValue val = new CascadingDropDownNameValue(Utils.ParseHelper.StripHtmlTags(m.DisplayNameWithAttribs), m.Id.ToString());

                                    if (contextKey == m.Id.ToString())
                                        val.isDefaultValue = true;

                                    list.Add(val);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _Error.LogException(ex);
            }

            return list.ToArray();
        }
    }
}

