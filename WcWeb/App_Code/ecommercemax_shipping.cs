/*
 *************************************************************************************
 ECOMMERCEMAX SOLUTIONS http://www.ecommercemax.com
 Contact: info@ecommercemax.com
 January 2006

 *************************************************************************************
 IMPORTANT: YOU MAY NOT PUBLISH THIS SOURCE CODE IN PUBLICLY ACCESSIBLE WEBSITES LIKE, 
 BUT NOT LIMITED TO FORUMS, newSLETTERS, newSGROUPS ETC.

 *** YOU MAY NOT REDISTRIBUTE FOR FREE OR RESELL THIS SCRIPT. ***

 *************************************************************************************
  -------------------
  WARRANTY DISCLAIMER
  ------------------- 
  THE CODES ON THIS SCRIPT PACKAGE ARE PROVIDED    
  "AS IS" WITHOUT WARRANTIES OF ANY KIND EITHER EXPRESS OR IMPLIED. TO  
  THE FULLEST EXTENT POSSIBLE PURSUANT TO THE APPLICABLE LAW,           
  ECOMMERCEMAX SOLUTIONS DISCLAIMS ALL WARRANTIES, EXPRESSED OR         
  IMPLIED, INCLUDING, BUT NOT LIMITED TO, IMPLIED WARRANTIES OF         
  MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE, NON-INFRINGEMENT   
  OR OTHER VIOLATION OF RIGHTS. ECOMMERCEMAX SOLUTIONS DOES NOT         
  WARRANT OR MAKE ANY REPRESENTATIONS REGARDING THE USE, VALIDITY,      
  ACCURACY, OR RELIABILITY OF, OR THE RESULTS OF THE USE OF, OR         
  OTHERWISE RESPECTING, THE CODES ON THIS SCRIPT PACKAGE OR ANY         
  RESOURCES USED ON THIS SCRIPT PACKAGE.                                
  -----------------------
  LIMITATION OF LIABILITY
  -----------------------                                                    
  IN NO EVENT WILL ECOMMERCEMAX SOLUTIONS, OR OTHER THIRD PARTIES              
  MENTIONED AT THIS SITE BE LIABLE FOR ANY DAMAGES WHATSOEVER (INCLUDING,      
  WITHOUT LIMITATION, THOSE RESULTING FROM LOST PROFITS, LOST DATA OR          
  BUSINESS INTERRUPTION) ARISING OUT OF THE USE, INABILITY TO USE, OR THE      
  RESULTS OF USE OF THIS SCRIPTS PACKAGE, ANY WEB SITES LINKED TO THIS TOOL,   
  OR THE MATERIALS OR INFORMATION CONTAINED HERE, WHETHER BASED ON WARRANTY,   
  CONTRACT, TORT OR ANY OTHER LEGAL THEORY AND WHETHER OR NOT ADVISED OF THE   
  POSSIBILITY OF SUCH DAMAGES. IF YOUR USE OF THE MATERIALS OR INFORMATION     
  FROM THIS TOOL RESULTS IN THE NEED FOR SERVICING, REPAIR OR CORRECTION OF    
  EQUIPMENT OR DATA, YOU ASSUME ALL COSTS THEREOF.                             
 *************************************************************************************
 */

/* MY NOTES
 * ****************************************
 * 
 * To find USPS' official list of supported countries go to http://pe.usps.gov/text/Imm/Immctry.html
 * 
 * */

using System;
using System.Web.UI.WebControls;
using System.IO;
using System.Net;
using System.Collections.Generic;
using System.Text;
using System.Xml;

using Wcss;
using WillCallWeb.StoreObjects;

public class ecommercemax_shipping
{
    /// <summary>
    /// Determines if a shipment qualifies for the low cost shipping rate 
    /// </summary>
    /// <param name="values"></param>
    /// <param name="merchList"></param>
    private static void AddCustomRates(List<ListItem> values, List<MerchWithQuantity> merchList)
    {
        if (merchList == null || merchList.Count == 0 || (! _Config._Shipping_LowCostMethod_IsActive))
            return;

        int lowRateQualified = 0;
        int otherQty = 0;

        foreach (MerchWithQuantity itm in merchList)
        {
            //because this cart deal is for regular items...
            //ignore those items that ship separately - third party
            //also ignore those that have a flat rate
            if (itm.MerchRecord.DeliveryType == _Enums.DeliveryType.parcel && (!itm.MerchRecord.IsShipSeparate) && (!itm.MerchRecord.IsFlatShip))
            {
                if (itm.MerchRecord.IsLowRateQualified)
                    lowRateQualified += itm.Qty;
                else
                    otherQty += itm.Qty;
            }
        }

        if (otherQty == 0 && lowRateQualified > 0)
        {
            if(lowRateQualified <= _Config._Shipping_LowCostMethod_MaxItems)
            {
                string text = string.Format("{0} --> {1}", _Config._Shipping_LowCostMethod_Name.ToUpper(), _Config._Shipping_LowCostMethod_Rate.ToString("c"));

                string value = string.Format("{0}~{1}", _Config._Shipping_LowCostMethod_Name.ToString(), _Config._Shipping_LowCostMethod_Rate.ToString());

                values.Add(new ListItem(text, value));
            }
        }
    }

    public static decimal ComputeHandlingFee(WillCallWeb.StoreObjects.ShoppingCart cart, decimal itemTotalToComputeAgainst)
    {
        if (!cart.HasMerchandiseItems_Shippable)
            return 0;

        //get the approopriate ship method
        List<SaleItem_Shipping> shipments = cart.Shipments_Merch.FindAll(delegate(SaleItem_Shipping match) { return (match.IsGeneral && !match.IsFlatShip); });

        if (shipments.Count == 0)
            return 0;

        return ComputeHandlingFee(shipments[0], itemTotalToComputeAgainst);
    }

    public static decimal ComputeHandlingFee(WillCallWeb.StoreObjects.SaleItem_Shipping shipment, decimal itemTotalToComputeAgainst)
    {
        if (shipment.IsFlatShip)
            return 0;

        return ComputeHandlingFee(shipment.ShipMethod, itemTotalToComputeAgainst);
    }

    public static decimal ComputeHandlingFee(string shipMethod, decimal itemTotalToComputeAgainst)
    {
        if (itemTotalToComputeAgainst <= 0)
            return 0;

        //only add a handling charge for 2day and 3day
        shipMethod = shipMethod.ToLower();

        //if the 9.99 rate is in effect - only compute handling fee for 2day and 3day
        if (_Config._Shipping_UPSGround_Merch_UseFlatRate && shipMethod.IndexOf("ups ground") != -1)
            return 0;

        //do not add a handling charge to media rate
        if (_Config._Shipping_LowCostMethod_IsActive && shipMethod == _Config._Shipping_LowCostMethod_Name)
            return 0;


        decimal min = _Config._HandlingFee_Min;//101112 $1
        decimal max = _Config._HandlingFee_Max;//101112 $5
        decimal pct = (.01m) * _Config._HandlingFee_Pct;//101112 10%

        decimal orderCharge = itemTotalToComputeAgainst * pct;

        if (orderCharge < min)
            return min;
        else if (orderCharge > max)
            return max;

        //do rounding to nearest quarter
        decimal result = Math.Round(Math.Round((orderCharge / .25m) + 0.25m, 0) * .25m, 2);

        return result;
    }
    
    /// <summary>
    /// From Admin/Orders_Shipping - when we need to calculate additional shipping for processed orders - reshipping, etc
    /// item list will be list of MERCHANDISE invoice items
    /// </summary>
    /// <returns></returns>
    public static List<ListItem> GetShipRates(decimal handlingCharge,
        string address, string countryCode, string zipCode, string state, decimal weight, List<InvoiceItem> itemList)
    {
        //translate invoice items into a merchlist
        List<MerchWithQuantity> merchList = new List<MerchWithQuantity>();
        foreach (InvoiceItem ii in itemList)
        {
            if(ii.IsMerchandiseItem)
                merchList.Add(new MerchWithQuantity(ii.Quantity, ii.MerchRecord));
        }

        return GetShipRates(handlingCharge, address, countryCode, zipCode, state, weight, false, merchList);
    }

    /// <summary>
    /// This handles the normal order flow
    /// item list will be list of MERCHANDISE invoice items - when ticket shipping this is null
    /// </summary>
    public static List<ListItem> GetShipRates(decimal handlingCharge,
        string address, string countryCode, string zipCode, string state, 
        decimal weight, bool IsTicketShipping,
        WillCallWeb.StoreObjects.SaleItem_Shipping shipment)
    {
        List<Wcss.MerchWithQuantity> merchList = new List<MerchWithQuantity>();

        if (shipment != null && shipment.ShipContext == _Enums.InvoiceItemContext.shippingmerch)
        {
            merchList.AddRange(shipment.Items_Merch_All);

            foreach (WillCallWeb.StoreObjects.SaleItem_Promotion sim in shipment.Items_Promo)
            {
                if (sim.SalePromotion.IsMerchPromotion && sim.HasProductSelections)
                {
                    int qty = (sim.SelectedAwardsMerchCollection.Count <= 1) ? sim.Quantity : 1;

                    foreach (Merch m in sim.SelectedAwardsMerchCollection)
                    {
                        merchList.Add(new MerchWithQuantity(qty, m));
                    }
                    
                    //promotions will not have bundles themselves!
                }
            }
        }

        return GetShipRates(handlingCharge, address, countryCode, zipCode, state, weight, IsTicketShipping, merchList);
    }

    private static List<ListItem> GetShipRates(decimal handlingCharge,
        string address, string countryCode, string zipCode, string state, 
        decimal weight,
        bool IsTicketShipping, List<MerchWithQuantity> merchList)
    {        
        if (IsTicketShipping)
        {
            if (Utils.Shipping.IsPoBoxAddress(address) && (! _Config._Shipping_AllowTicketsToPoBox))
            {
                return new List<ListItem>();
                //throw new Exception("Sorry, tickets cannot be shipped to PO Boxes.");
            }

            //cleanup
            if (countryCode.ToLower().Trim() == "usa")
                countryCode = "us";

            ///enforce general rules
            ///Tickets may only be shipped within the continental United States if flag is set
            if (_Config._Shipping_Tickets_USA_Only)
            {
                if(countryCode.ToLower() != "us")
                    return new List<ListItem>();
                else if (_Config._Shipping_Tickets_USA_Continental_Only &&
                    (state.ToLower() == "hi" || state.ToLower() == "ak" || state.ToLower() == "pr"))
                    return new List<ListItem>();
            }
        }

        //ensure a min weight
        //if (weight < 1.0M)
        //    weight = 1;

        ecommercemax_shipping srates = new ecommercemax_shipping();

        srates.handlingFee = handlingCharge;

        //Use UPS Origin address for all
        srates.shippercity = _Config._UPS_OriginCity;
        srates.shipperpostalcode = _Config._UPS_OriginZip;
        srates.shipperstateprovincecode = _Config._UPS_OriginState;
        srates.shippercountrycode = _Config._UPS_OriginCountryCode;



        //determines which shipping rates to use
        bool usps = (bool)(_Config._USPS_Enabled && (!IsTicketShipping));
        bool ups = (bool)(_Config._UPS_Enabled);
        //srates.usps_will_compute = usps;

        srates.usps_will_compute = usps;
        srates.fedex_will_compute = false;
        srates.ups_will_compute = ups;

        //TODO: get code from input
        srates.receivercountrycode = countryCode;
        srates.receiverpostalcode = zipCode;
        srates.receiverstateprovincecode = state;
        srates.shipmentweight = weight;

        /* ---------------------------------------------------------------------------------------
         * 
         * START- UPS INITIALIZATION
         * 
         * -----------------------------------------------------------------------------------------
         */
        if (srates.ups_will_compute && (! Utils.Shipping.IsPoBoxAddress(address)))
        {
            //ASSIGN YOUR OWN SETTINGS BY GETTING VALUES FROM YOUR WEB.CONFIG FILE
            srates.ups_accesslicensenumber = _Config._UPS_AccessKey;// System.Configuration.ConfigurationManager.AppSettings["ups_accesslicensenumber"];
            srates.ups_userid = _Config._UPS_UserId;// System.Configuration.ConfigurationManager.AppSettings["ups_userid"];
            srates.ups_password = _Config._UPS_UserPass;// System.Configuration.ConfigurationManager.AppSettings["ups_password"];
            srates.ups_shippernumber = _Config._UPS_AccountNum;// System.Configuration.ConfigurationManager.AppSettings["ups_shippernumber"];
            srates.ups_weburl = _Config._UPS_ServiceRates_Url;//"https://www.ups.com/ups.app/xml/Rate?";

            //INITIALIZE UPS options          
            srates.ups_pickuptypecode = ecommercemax_shipping.ups_pickuptypecode_options._01DailyPickup;
            srates.ups_packagetype = ecommercemax_shipping.ups_packagetype_options._02Package;
        }
        else
            srates.ups_will_compute = false;

        if (srates.usps_will_compute)
        {
            //Matter at First-Class Mail prices cannot exceed 13 ounces. 
            //First-Class Mail weighing more than 13 ounces is Priority Mail 

            srates.usps_password = _Config._USPS_Password;
            srates.usps_userid = _Config._USPS_UserId;
            srates.usps_weburl = _Config._USPS_WebUrl;

            srates.usps_postal_zone = zipCode;
            srates.usps_receivercountry = countryCode;

            srates.usps_mailtype = usps_mailtype_options._flat;
            srates.usps_packagesize = usps_packagesize_options._regular;
            srates.usps_service = usps_service_options._all;
            
            //TODO: implement
            //srates.usps_mediaratequalified = mediaRateQualified;
        }

        //EXECUTE THE LIVE CALCULATOR
        srates.execute();

        //ACCESS THE RESULT CODE; 0=SUCCESS, 1+= ERROR
        //usps_result_code.Text = srates.usps_result_code.ToString();
        //fedex_result_code.Text = srates.fedex_result_code.ToString();
        //ups_result_code.Text = srates.ups_result_code.ToString();

        //Errors hinge on ups results
        //we dont really care about usps errors as long as ups is working


        //TODO: handle errors
        //ACCESS THE ERROR DESCRIPTION IF THERE IS ONE
        //usps_error.Text = srates.usps_error_description;
        //fedex_error.Text = srates.fedex_error_description;
        //ups_error.Text = srates.ups_error_description;
        if (srates.ups_error_description != null && srates.ups_error_description.Trim().Length > 0)
            throw new Exception(srates.ups_error_description.Trim());
        if (srates.usps_error_description != null && srates.usps_error_description.Trim().Length > 0)
            throw new Exception(srates.usps_error_description.Trim());

        List<ListItem> values = new List<ListItem>();

        //add our own special custom rates
        AddCustomRates(values, merchList);

        //note here that we do not directly ref the ups, usps or fedex rates
        foreach(ecommercemax_shipping.shiprates_detail rate in srates.shiprates)
        {
            string text = string.Empty;
            string value = string.Empty;

            //US Postal Service rates - rates are selected in execute
            if (! IsTicketShipping && rate.carrier_code.ToLower().Equals("usps"))
            {
                text = string.Format("{0} --> {1} {2}{3}", rate.carrier_code.ToUpper(), rate.shipping_cost.ToString("c"), rate.description,
                    (rate.speed.Trim().Length > 0) ? string.Format(", {0}", rate.speed.Trim().ToLower().Replace("day", "business day")) : string.Empty);

                value = string.Format("{0}~{1}", rate.description, rate.shipping_cost);
            }
            //UPS rates
            else if (rate.carrier_code.ToLower().Equals("ups"))
            {
                //UPS Options
                //"ups ground"
                //"ups 3 day select"
                //"ups 2nd day air."
                //"ups next day air saver"
                //"ups next day air."
                //"ups next day air. early a.m."

                //FOREIGN
                //CA "ups standard ups"
                //CA, JP, UK "ups worldwide expeditedsm"
                //CA, JP, UK "ups express saver"

                bool UPS_GroundOnly = IsTicketShipping && _Config._Shipping_Tickets_DefaultMethod.ToLower() == "ups ground";
                string desc = rate.description.Trim().ToLower();

                if ((!UPS_GroundOnly) || (UPS_GroundOnly && desc.IndexOf("ups ground") != -1))
                {
                    //we would like to use 03 - ground, 07 - worldwide or 12 - 3day select
                    text = string.Format("{0} --> {1} {2}{3}", rate.carrier_code.ToUpper(), rate.shipping_cost.ToString("c"), rate.description,
                        (rate.speed.Trim().Length > 0) ? string.Format(", {0}", rate.speed.Trim().ToLower().Replace("day", "business day")) : string.Empty);

                    //append caveat to standard ship
                    if (desc.IndexOf("ups standard") != -1)
                        text += string.Format(" {0}", "***Purchaser will be responsible for any fees incurred transporting order from customs to destination. Buyer beware: these fees may be excessive.");

                    //string value = string.Format("{0}~{1}", srates.shiprates[i].description, srates.shiprates[i].shipping_cost);
                    value = string.Format("{0}~{1}", rate.description, rate.shipping_cost);
                }
            }

            //ADD THE CONSTRUCTED STRING
            if(text.Length > 0)
                values.Add(new ListItem(text, value));
        }        

        return values;

    }

    public void execute()
    {
        if (usps_will_compute)
            usps_execute();

        if (fedex_will_compute)
            fedex_execute(ecommercemax_shipping.fedex_Carrier_options.ALL);

        if (ups_will_compute)
            ups_execute();

        shiprates = new List<shiprates_detail>();

        if (usps_rates != null && usps_rates.Count > 0)
        {
            foreach (usps_rate_detail rate in usps_rates)
            {
                string desc = rate.description.ToLower();

                //limit rates
                if (desc.ToLower().IndexOf("first-class mail parcel") != -1 || desc.ToLower().IndexOf("priority mail") != -1)//express mail is way expensive || desc.Equals("express mail"))
                {
                    shiprates_detail usps = new shiprates_detail();

                    usps.carrier_code = "usps";
                    usps.shipping_cost = (rate.shipping_cost) + this.handlingFee;
                    usps.description = rate.description;
                    usps.speed = rate.speed;
                    usps.usps_maxdimensions = rate.usps_maxdimensions;
                    usps.usps_maxweight = Convert.ToDecimal(rate.usps_maxweight);

                    shiprates.Add(usps);
                }
            }
        }

        if (fedex_rates != null && fedex_rates.Count > 0)
        {
            foreach (fedex_Rate_Detail rate in fedex_rates)
            {
                shiprates_detail fedex = new shiprates_detail();

                fedex.carrier_code = "fedex";
                fedex.shipping_cost = (rate.shipping_cost) + this.handlingFee;
                fedex.description = rate.description;
                fedex.speed = rate.speed;
                fedex.fedex_deliverydate = rate.fedex_deliverydate;
                fedex.fedex_deliveryday = rate.fedex_deliveryday;
                fedex.fedex_timeintransit = rate.fedex_timeintransit;

                shiprates.Add(fedex);
            }
        }

        if (ups_rates != null && ups_rates.Count > 0)
        {
            foreach (ups_rate_detail rate in ups_rates)
            {
                string desc = rate.description.ToLower();

                //use IndexOf() instead of equals() for rate filtering as punctuation is inconsistent
                if (desc.IndexOf("ups ground") != -1 || desc.IndexOf("ups 3 day select") != -1 || desc.IndexOf("ups 2nd day air") != -1 ||  //domestic
                        desc.IndexOf("ups worldwide") != -1 || desc.IndexOf("ups standard") != -1)                                          //international
                {
                    shiprates_detail ups = new shiprates_detail();

                    ups.carrier_code = "ups";

                    if (_Config._Shipping_UPSGround_Merch_UseFlatRate && desc.IndexOf("ups ground") != -1)
                        ups.shipping_cost = _Config._Shipping_UPSGround_Merch_FlatRate;
                    else
                        ups.shipping_cost = (rate.shipping_cost) + this.handlingFee;

                    ups.description = rate.description;
                    ups.speed = rate.speed;
                    ups.ups_scheduleddeliverytime = rate.ups_scheduleddeliverytime;

                    shiprates.Add(ups);
                }
            }
        }

        //http://dotnetslackers.com/community/blogs/simoneb/archive/2007/06/20/How-to-sort-a-generic-List_3C00_T_3E00_.aspx
        if (shiprates.Count > 1)
            shiprates.Sort(delegate(shiprates_detail x, shiprates_detail y) { return (x.shipping_cost.CompareTo(y.shipping_cost)); });
    }

    public ecommercemax_shipping()
    {
        proxy_address = "";
        usps_will_compute = false;
        m_usps_result_code = 0;
        m_shippercountrycode = "US";
        m_receivercountrycode = "US";
        pkg_length = 0;
        pkg_width = 0;
        pkg_height = 0;
        pkg_declaredvalue = 0;
        m_shiprates_total_count = 0;
        proxy_address = "";

        //=======================================================================
        //USPS Default Values
        //=======================================================================
        m_usps_service = "ALL";// "All";
        //m_usps_rates_count = 0;
        m_usps_error_description = "";
        m_usps_raw_xml_text = "";
        m_usps_container = "FLAT RATE BOX";
        usps_packagesize = usps_packagesize_options._regular;
        usps_weburl = "http://production.shippingapis.com/shippingapi.dll";

        //=======================================================================
        //FEDEX Default Values
        //=======================================================================
        fedex_will_compute = false;
        m_fedex_result_code = 0;
        m_fedex_error_description = "";
        m_fedex_raw_xml_text = "";
        fedex_weburl = "https://gateway.fedex.com/GatewayDC";

        //=======================================================================
        //UPS Default Values
        //======================================================================= 
        ups_will_compute = false;
        m_ups_error_description = "";
        m_ups_raw_xml_text = "";
        proxy_address = "";
        ups_pickuptypecode = ups_pickuptypecode_options._01DailyPickup;
        ups_packagetype = ups_packagetype_options._02Package;
        ups_insured_value = 0;
        ups_saturdaypickup = TrueorFalse_options._false;
        ups_saturdaydelivery = TrueorFalse_options._false;
        ups_weburl = "https://www.ups.com/ups.app/xml/Rate?";
    }

    #region Internal 

    private string m_version = "1.7";
    public string version
    {
        get
        {
            return m_version;
        }
    }

    public enum TrueorFalse_options
    {
        _true, _false
    }

    public struct shiprates_detail
    {
        public string carrier_code;
        public string description;
        public decimal shipping_cost;
        public string speed;
        public string usps_maxdimensions;
        public decimal usps_maxweight;
        public string fedex_deliveryday;
        public string fedex_deliverydate;
        public string fedex_timeintransit;
        public string ups_scheduleddeliverytime;
    }

    public List<shiprates_detail> shiprates;

    private int m_shiprates_total_count;
    public int shiprates_total_count
    {
        get
        {
            return m_shiprates_total_count;
        }
    }

    protected System.Text.StringBuilder xml = new System.Text.StringBuilder();

    //public decimal orderTotal;
    public decimal handlingFee;
    public string receiver_country_code;
    public string shippercity;
    public string receivercity;
    public string proxy_address;
    public Boolean usps_will_compute;
    private string m_shipperstateprovincecode;
    public string shipperstateprovincecode
    {
        get
        {
            return (m_shipperstateprovincecode);
        }
        set
        {
            m_shipperstateprovincecode = value.Trim().ToUpper();
        }
    }

    private string m_shippercountrycode;
    public string shippercountrycode
    {
        get
        {
            return (m_shippercountrycode);
        }
        set
        {
            m_shippercountrycode = value.Trim().ToUpper();
            if (m_shippercountrycode == "")
                m_shippercountrycode = "US";
        }
    }

    private string m_receiverstateprovincecode;
    public string receiverstateprovincecode
    {
        get
        {
            return (m_receiverstateprovincecode);
        }
        set
        {
            m_receiverstateprovincecode = value.Trim().ToUpper();
        }
    }

    private string m_receivercountrycode;
    public string receivercountrycode
    {
        get
        {
            return (m_receivercountrycode);
        }
        set
        {
            m_receivercountrycode = value.Trim().ToUpper();
            if (m_receivercountrycode == "")
                m_receivercountrycode = "US";
        }
    }

    private string m_shipperpostalcode;
    public string shipperpostalcode
    {
        set
        {
            if (value.Trim() == "")
                m_shipperpostalcode = "0";
            else
                m_shipperpostalcode = value;
            if (m_shippercountrycode == "US")
            {
                if (m_shipperpostalcode.Length > 5)
                    m_shipperpostalcode = m_shipperpostalcode.Substring(0, 5);
            }
        }
    }

    private string m_receiverpostalcode;
    public string receiverpostalcode
    {
        set
        {
            if (value.Trim() == "")
                m_receiverpostalcode = "0";
            else
                m_receiverpostalcode = value;
            if (m_receivercountrycode == "US")
            {
                if (m_receiverpostalcode.Length > 5)
                    m_receiverpostalcode = m_receiverpostalcode.Substring(0, 5);
            }
        }
    }

    public decimal pkg_length;
    public decimal pkg_width;
    public decimal pkg_height;
    public decimal shipmentweight;
    public decimal pkg_declaredvalue;

    #endregion

    #region USPS

    //'==================================================================================
    //'usps
    //'==================================================================================

    public enum usps_container_options
    {
        _flatrateenvelope,
        _frb
    }

    private string m_usps_container;
    public usps_container_options usps_container
    {
        set
        {
            switch (value)
            {
                case usps_container_options._flatrateenvelope:
                    m_usps_container = "Flat Rate Envelope";
                    break;
                case usps_container_options._frb:
                    m_usps_container = "Flat Rate Box";
                    break;
                default:
                    m_usps_container = "";
                    break;
            }
        }
    }

    public enum usps_service_options
    {
        _all,
        _express,
        _first_class,
        _priority,
        _parcel,
        _bpm,
        _library,
        _media,
        _online
    }

    private string m_usps_service;
    public usps_service_options usps_service
    {
        set
        {
            switch (value)
            {
                case usps_service_options._all:
                    m_usps_service = "All";
                    break;
                case usps_service_options._express:
                    m_usps_service = "Express";
                    break;
                case usps_service_options._first_class:
                    m_usps_service = "First Class";
                    break;
                case usps_service_options._priority:
                    m_usps_service = "Priority";
                    break;
                case usps_service_options._parcel:
                    m_usps_service = "Parcel";
                    break;
                case usps_service_options._bpm:
                    m_usps_service = "BPM";
                    break;
                case usps_service_options._library:
                    m_usps_service = "Library";
                    break;
                case usps_service_options._media:
                    m_usps_service = "Media";
                    break;
                case usps_service_options._online:
                    m_usps_service = "Online";
                    break;
                default:
                    m_usps_service = "All";
                    break;
            }
        }
    }

    public enum usps_packagesize_options
    {
        _regular,
        _large
        //,_oversize,
    }

    private string m_usps_packagesize;
    public usps_packagesize_options usps_packagesize
    {
        set
        {
            switch (value)
            {
                case usps_packagesize_options._regular:
                    m_usps_packagesize = "REGULAR";
                    break;
                case usps_packagesize_options._large:
                    m_usps_packagesize = "LARGE";
                    break;
                //case usps_packagesize_options._oversize:
                //    m_usps_packagesize = "Oversize";
                //    break;
                default:
                    m_usps_packagesize = "REGULAR";
                    break;
            }
        }
    }

    public enum usps_mailtype_options
    {
        _letter,
        _flat,
        _parcel,
        _postcard,
        _intl_package,
        _intl_envelope
        //_package,
        //_envelope,
        //_postcards_or_aerogrammes,
        //_matter_for_the_blind,
    }

    private string m_usps_mailtype;
    public usps_mailtype_options usps_mailtype
    {
        set
        {
            switch (value)
            {
                case usps_mailtype_options._letter:
                    m_usps_mailtype = "LETTER";
                    break;
                case usps_mailtype_options._flat:
                    m_usps_mailtype = "FLAT RATE BOX";
                    break;
                case usps_mailtype_options._postcard:
                    m_usps_mailtype = "POSTCARD";
                    break;
                case usps_mailtype_options._parcel:
                    m_usps_mailtype = "PARCEL";
                    break;
                case usps_mailtype_options._intl_package:
                    m_usps_mailtype = "Package";
                    break;
                case usps_mailtype_options._intl_envelope:
                    m_usps_mailtype = "Envelope";
                    break;
                default:
                    m_usps_mailtype = "FLAT RATE BOX";
                    break;
            }
        }
    }

    public struct usps_rate_detail
    {
        public string description;
        public decimal shipping_cost;
        public string speed;
        public string usps_maxdimensions;
        public decimal usps_maxweight;
    }

    public List<usps_rate_detail> usps_rates;

    private int m_usps_result_code;
    public int usps_result_code
    {
        get
        {
            return (m_usps_result_code);
        }
    }

    private string m_usps_error_description;
    public string usps_error_description
    {
        get
        {
            return (m_usps_error_description);
        }
    }

    private string m_usps_raw_xml_text;
    public string usps_raw_xml_text
    {
        get
        {
            return (m_usps_raw_xml_text);
        }
    }

    private string m_usps_receivercountry;
    public string usps_receivercountry
    {
        get
        {
            return (m_usps_receivercountry);
        }
        set
        {
            m_usps_receivercountry = value.Trim().ToLower();
        }
    }

    public string usps_postal_zone;
    public string usps_weburl;
    public string usps_userid;
    public string usps_password;
    public TrueorFalse_options usps_machinable;
    public string usps_prohibitions, usps_restrictions;
    public bool usps_mediaratequalified;

    /// <summary>
    /// domestic API V4
    /// </summary>
    /// <returns></returns>
    private int usps_execute_V4_V2()
    {
        string rateResponseVersion = "RateV4Response";

        //DO NOT convert weight to minimum of one pound for USPS
        //////////decimal upsWeight = shipmentweight;
        //////////if (upsWeight < 1.0M)
        //////////    upsWeight = 1;

        decimal weight_in_ounces = shipmentweight * 16;
        //USPS has limit of 70 pounds or 1120 ounces
        if(weight_in_ounces > 1120)
        {
            m_usps_error_description = "USPS has limit of 70 pounds or 1120 ounces";
            return 1;
        }

        //construct the xml request
        xml.Length = 0;

        if ((m_usps_receivercountry == "us") || (usps_receivercountry == "usa") || (usps_receivercountry == ""))
            FormatV4Request(xml, weight_in_ounces);
        
        //NO intl rates for usps are being used
        //else
        //{
        //    FormatV2Request(xml, weight_in_ounces);
        //    rateResponseVersion = "IntlRateV2Response";
        //}

        try
        {
            //construct the request
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(usps_weburl);
            if (proxy_address != "")
            {
                WebProxy prxy = new WebProxy(proxy_address);
                req.Proxy = prxy;
            }
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            req.ContentLength = xml.Length;

            StreamWriter stOut = new StreamWriter(req.GetRequestStream(), System.Text.Encoding.ASCII);
            stOut.Write(xml.ToString());
            stOut.Close();
            StreamReader stIn = new StreamReader(req.GetResponse().GetResponseStream());
            m_usps_raw_xml_text = stIn.ReadToEnd();
            stIn.Close();

            //reset the xml
            xml.Length = 0;

            System.Xml.XmlDocument myxmldocument = new System.Xml.XmlDocument();
            myxmldocument.LoadXml(m_usps_raw_xml_text);
            string errordesc = "";

            if (m_usps_raw_xml_text.Length > 7 && m_usps_raw_xml_text.Substring(0, 7) == "<Error>")
                errordesc = m_usps_raw_xml_text;

            if (errordesc == "")
            {
                System.Xml.XmlNodeList error_nodelist;
                error_nodelist = myxmldocument.SelectNodes(string.Format("/{0}/Package/Error", rateResponseVersion));
                if (error_nodelist.Count >= 1)
                {
                    XmlNodeList elemList = myxmldocument.GetElementsByTagName("Description");
                    for (int i=0; i < elemList.Count; i++)
                    {   
                      errordesc += elemList[i].InnerXml;
                    }  

                    //errordesc = myxmldocument.SelectSingleNode(string.Format("/{0}/Package/Error/description", rateResponseVersion)).InnerText;
                }
            }

            if (errordesc != "")
            {
                m_usps_result_code = 1;
                m_usps_error_description = errordesc;
            }
            else
            {
                System.Xml.XmlNodeList ratedshipment_nodelist;
                usps_rates = new List<usps_rate_detail>();

                if (rateResponseVersion == "RateV4Response")
                {
                    ratedshipment_nodelist = myxmldocument.SelectNodes(string.Format("/{0}/Package/Postage", rateResponseVersion));
                    //usps_postal_zone = myxmldocument.SelectSingleNode(string.Format("/{0}/Package/Zone", rateResponseVersion)).InnerText;

                    foreach (System.Xml.XmlNode node in ratedshipment_nodelist)
                    {
                        usps_rate_detail usps = new usps_rate_detail();

                        System.Xml.XmlNode nd = node.SelectSingleNode("MailService");
                        string desc = nd.InnerText;
                        usps.description = Utils.ParseHelper.StripHtmlTags(System.Web.HttpUtility.HtmlDecode(desc.ToLower())).Replace("&reg;", string.Empty);
                        
                        if (usps.description.IndexOf("priority mail") != -1)
                            usps.speed = "2-3 days";
                        else
                        {
                            if (usps.description.IndexOf("express mail") != -1)
                                usps.speed = "Next Day";
                            else
                                usps.speed = "2-9 days";
                        }

                        usps.shipping_cost = Convert.ToDecimal(node.SelectSingleNode("Rate").InnerText);


                        //if (usps.description.Length > 13 && usps.description.Substring(0, 13).ToLower() == "priority mail")
                        //    usps.speed = "2-3 days";
                        //else
                        //{
                        //    if (usps.description.Length > 12 && usps.description.Substring(0, 12).ToLower() == "express mail")
                        //        usps.speed = "Next Day";
                        //    else
                        //        usps.speed = "2-9 days";
                        //}

                        usps.usps_maxdimensions = "";
                        usps.usps_maxweight = 0;

                        usps_rates.Add(usps);
                    }

                }
                else
                {
                    ratedshipment_nodelist = myxmldocument.SelectNodes(string.Format("/{0}/Package/Service", rateResponseVersion));

                    System.Xml.XmlNodeList prohibitions_nlist;
                    prohibitions_nlist = myxmldocument.SelectNodes(string.Format("/{0}/Package/Prohibitions", rateResponseVersion));
                    if (prohibitions_nlist.Count >= 1)
                    {
                        usps_prohibitions = myxmldocument.SelectSingleNode(string.Format("/{0}/Package/Prohibitions", rateResponseVersion)).InnerText;
                    }

                    System.Xml.XmlNodeList restrictions_nlist;
                    restrictions_nlist = myxmldocument.SelectNodes(string.Format("/{0}/Package/Restrictions", rateResponseVersion));
                    if (restrictions_nlist.Count >= 1)
                    {
                        usps_restrictions = myxmldocument.SelectSingleNode(string.Format("/{0}/Package/Restrictions", rateResponseVersion)).InnerText;
                    }

                    

                    foreach (System.Xml.XmlNode node in ratedshipment_nodelist)
                    {
                        usps_rate_detail usps = new usps_rate_detail();

                        usps.shipping_cost = Convert.ToDecimal(node.SelectSingleNode("Postage").InnerText);
                        usps.description = node.SelectSingleNode("SvcDescription").InnerText;
                        usps.speed = node.SelectSingleNode("SvcCommitments").InnerText;
                        usps.usps_maxdimensions = node.SelectSingleNode("MaxDimensions").InnerText;
                        usps.usps_maxweight = Convert.ToDecimal(node.SelectSingleNode("MaxWeight").InnerText);

                        usps_rates.Add(usps);
                    }                    
                }


                m_usps_result_code = 0;
                m_usps_error_description = "";


            }
        }
        catch (Exception e)
        {
            m_usps_result_code = 2;
            m_usps_error_description = e.ToString();
        }



        return m_usps_result_code;
    }
    
    #region Format XML Request

    public void FormatV4Request(StringBuilder xml, decimal weight_in_ounces)
    {
        xml.Append      ("API=RateV4&XML=");
        xml.AppendFormat("<RateV4Request USERID='{0}'>", usps_userid);
        xml.Append      ("<Revision>2</Revision>");
        xml.Append      ("<Package ID='0'>");

        ///Service enumeration
        ///FIRST CLASS, FIRST CLASS HFP COMMERCIAL, PRIORITY, PRIORITY COMMERCIAL, PRIORITY HFP COMMERCIAL, 
        ///EXPRESS, EXPRESS COMMERCIAL, EXPRESS SH, EXPRESS SH COMMERCIAL, EXPRESS HFP, EXPRESS HFP COMMERCIAL, 
        ///PARCEL, MEDIA, LIBRARY, ALL, ONLINE
        //xml.AppendFormat("<Service>{0}</Service>", m_usps_service);
        if(usps_mediaratequalified)
            xml.AppendFormat("<Service>{0}</Service>", "MEDIA");
        else
            xml.AppendFormat("<Service>{0}</Service>", "ALL");



        ///CURRENTLY NOT IMPLEMENTED!!!
        ///FirstClassMailType enumeration
        ///LETTER, FLAT, PARCEL, POSTCARD
        //xml.AppendFormat("<FirstClassMailType>{0}</FirstClassMailType>", );
        xml.AppendFormat("<ZipOrigination>{0}</ZipOrigination>", m_shipperpostalcode);
        xml.AppendFormat("<ZipDestination>{0}</ZipDestination>", m_receiverpostalcode);
        xml.Append      ("<Pounds>0</Pounds>");
        xml.AppendFormat("<Ounces>{0}</Ounces>", weight_in_ounces.ToString());

        ///Container enumeration
        ///FLAT RATE ENVELOPE, PADDED FLAT RATE ENVELOPE, LEGAL FLAT RATE ENVELOPE, SM FLAT RATE ENVELOPE, WINDOW FLAT RATE ENVELOPE, 
        ///GIFT CARD FLAT RATE ENVELOPE, FLAT RATE BOX, SM FLAT RATE BOX, MD FLAT RATE BOX, LG FLAT RATE BOX, 
        ///REGIONALRATEBOXA, REGIONALRATEBOXB, RECTANGULAR, NONRECTANGULAR
        //if (((m_usps_service.ToLower().IndexOf("express mail") != -1) || (m_usps_service.ToLower().IndexOf("priority mail") != -1)) & (m_usps_container != ""))
        //    xml.AppendFormat("<Container>{0}</Container>", m_usps_container);
        //else
        //    xml.Append("<Container/>");

        //xml.AppendFormat("<Container>{0}</Container>", "FLAT RATE BOX");
        xml.AppendFormat("<Container>{0}</Container>", "VARIABLE");

        ///REGULAR: Package dimensions are 12’’ or less;
        ///LARGE: Any package dimension is larger than 12’’.
        xml.AppendFormat("<Size>{0}</Size>", m_usps_packagesize);

        if (m_usps_packagesize.ToLower() == "large")
        {
            xml.AppendFormat("<Width>{0}</Width>", pkg_width.ToString());
            xml.AppendFormat("<Length>{0}</Length>", pkg_length.ToString());
            xml.AppendFormat("<Height>{0}</Height>", pkg_height.ToString());
            //xml.AppendFormat("<Girth>{0}</Girth>");
        }

        //optional
        //xml.Append("<Value>{0}</Value>");

        if (usps_machinable == TrueorFalse_options._true)
            xml.Append("<Machinable>True</Machinable>");
        else
            xml.Append("<Machinable>False</Machinable>");

        xml.Append("</Package></RateV4Request>");
    }
    public void FormatV2Request(StringBuilder xml, decimal weight_in_ounces)
    {
        xml.Append      ("API=IntlRateV2&XML=");
        xml.AppendFormat("<IntlRateV2Request USERID='{0}'>", usps_userid);
        xml.Append      ("<Revision>2</Revision>");
        xml.Append      ("<Package ID='0'>");
        xml.Append      ("<Pounds>0</Pounds>");
        xml.AppendFormat("<Ounces>{0}</Ounces>", weight_in_ounces);

        if (usps_machinable == TrueorFalse_options._true)
            xml.Append("<Machinable>True</Machinable>");
        else
            xml.Append("<Machinable>False</Machinable>");

        xml.Append("<MailType>Package</MailType>");

        xml.AppendFormat("<Country>{0}</Country>", m_usps_receivercountry);

        if(m_usps_packagesize.ToLower() == "large")
            xml.Append("<Container>RECTANGULAR</Container>");
        else
            xml.Append("<Container/>");

        ///REGULAR: Package dimensions are 12’’ or less;
        ///LARGE: Any package dimension is larger than 12’’.
        xml.AppendFormat("<Size>{0}</Size>", m_usps_packagesize);
        if (m_usps_packagesize.ToLower() == "large")
        {
            xml.AppendFormat("<Width>{0}</Width>", pkg_width.ToString());
            xml.AppendFormat("<Length>{0}</Length>", pkg_length.ToString());
            xml.AppendFormat("<Height>{0}</Height>", pkg_height.ToString());
            //xml.AppendFormat("<Girth>{0}</Girth>");
        }

        xml.Append("<CommercialFlag>N</CommercialFlag>");

        xml.Append("</Package></IntlRateV2Request>");
    }

    #endregion

    public int usps_execute()
    {
        m_usps_result_code = usps_execute_V4_V2();

        return m_usps_result_code;
    }

    #endregion

    #region Fedex
    //'==================================================================================
    //'FEDEX PROPERTIES
    //'==================================================================================
    public Boolean fedex_will_compute;

    public string fedex_CustomerTransactionIdentifier;
    public string fedex_weburl;
    public string fedex_meter_no;
    public string fedex_accountnumber;

    public enum fedex_Carrier_options
    {
        FDXE,
        FDXG,
        ALL
    }

   public enum fedex_DropoffType_options
   {
        REGULARPICKUP,
        REQUESTCOURIER,
        DROPBOX,
        BUSINESSSERVICE_CENTER,
        STATION
    }
    
    private string m_fedex_DropoffType;
    public fedex_DropoffType_options fedex_DropoffType
    {
        set
        {
            switch (value)
            {
                case fedex_DropoffType_options.BUSINESSSERVICE_CENTER:
                    m_fedex_DropoffType = "BUSINESSSERVICE CENTER";
                    break;
                case fedex_DropoffType_options.DROPBOX:
                    m_fedex_DropoffType = "DROPBOX";
                    break;
                case fedex_DropoffType_options.REGULARPICKUP:
                    m_fedex_DropoffType = "REGULARPICKUP";
                    break;
                case fedex_DropoffType_options.REQUESTCOURIER:
                    m_fedex_DropoffType = "REQUESTCOURIER";
                    break;
                case fedex_DropoffType_options.STATION:
                    m_fedex_DropoffType = "STATION";
                    break;
                default:
                    m_fedex_DropoffType = "REGULARPICKUP";
                    break;
            }
        }
    }

    public enum fedex_Packaging_options
    {
        YOURPACKAGING,
        FEDEXENVELOPE,
        FEDEXPAK,
        FEDEXBOX,
        FEDEXTUBE,
        FEDEX10KGBOX,
        FEDEX25KGBOX
    }
    
    private string m_fedex_Packaging;
    public fedex_Packaging_options fedex_Packaging
    {
        set
        {
            switch (value)
            {
                case fedex_Packaging_options.FEDEX10KGBOX:
                    m_fedex_Packaging = "FEDEX10KGBOX";
                    break;
                case fedex_Packaging_options.FEDEX25KGBOX:
                    m_fedex_Packaging = "FEDEX25KGBOX";
                    break;
                case fedex_Packaging_options.FEDEXBOX:
                    m_fedex_Packaging = "FEDEXBOX";
                    break;
                case fedex_Packaging_options.FEDEXENVELOPE:
                    m_fedex_Packaging = "FEDEXENVELOPE";
                    break;
                case fedex_Packaging_options.FEDEXPAK:
                    m_fedex_Packaging = "FEDEXPAK";
                    break;
                case fedex_Packaging_options.FEDEXTUBE:
                    m_fedex_Packaging = "FEDEXTUBE";
                    break;
                case fedex_Packaging_options.YOURPACKAGING:
                    m_fedex_Packaging = "YOURPACKAGING ";
                    break;
                default:
                    m_fedex_Packaging = "YOURPACKAGING";
                    break;
            }
        }
    }

    public struct fedex_Rate_Detail
    {
        public string CarrierCode;
        public string description;
        public decimal shipping_cost;
        public string speed;
        public string fedex_deliveryday;
        public string fedex_deliverydate;
        public string fedex_timeintransit;
    }
    public List<fedex_Rate_Detail> fdxe_rates;
    public List<fedex_Rate_Detail> fdxg_rates;
    public List<fedex_Rate_Detail> fedex_rates;

    private int m_fedex_result_code;
    public int fedex_result_code
    {
        get
        {
            return m_fedex_result_code;
        }
    }

    private string m_fedex_error_description;
    public string fedex_error_description
    {
        get
        {
            return m_fedex_error_description;
        }
    }

    private string m_fedex_raw_xml_text;
    public string fedex_raw_xml_text
    {
        get
        {
            return m_fedex_raw_xml_text;
        }
    }

    public string fedex_ShipDate;
    

public void execute_fdx(fedex_Carrier_options p_Carrier)
{
    try
    {
        decimal fedexWeight = shipmentweight;
        if (fedexWeight < 1.0M)
            fedexWeight = 1;

        xml.Length = 0;
        string CarrierCode;
        if (p_Carrier == fedex_Carrier_options.FDXE) 
            CarrierCode = "FDXE";
        else
            CarrierCode = "FDXG";

        xml.Append("<?xml version='1.0' encoding='UTF-8' ?>");
        xml.Append("<FDXRateAvailableServicesRequest xmlns:api='http://www.fedex.com/fsmapi' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' ");
        xml.Append("xsi:noNamespaceSchemaLocation='FDXRateAvailableServicesRequest.xsd'>");
        xml.Append("<RequestHeader>");
        xml.AppendFormat("<CustomerTransactionIdentifier>{0}</CustomerTransactionIdentifier>", fedex_CustomerTransactionIdentifier);
        xml.AppendFormat("<AccountNumber>{0}</AccountNumber>", fedex_accountnumber);
        xml.AppendFormat("<MeterNumber>{0}</MeterNumber>", fedex_meter_no);
        xml.AppendFormat("<CarrierCode>{0}</CarrierCode>", CarrierCode);
        xml.Append("</RequestHeader>");
        xml.AppendFormat("<ShipDate>{0}</ShipDate>", fedex_ShipDate);
        xml.AppendFormat("<DropoffType>{0}</DropoffType>", m_fedex_DropoffType);
        xml.AppendFormat("<Packaging>{0}</Packaging>", m_fedex_Packaging);
        xml.Append("<WeightUnits>LBS</WeightUnits>");
        xml.AppendFormat("<Weight>{0}</Weight>", fedexWeight.ToString());

        if (
            (pkg_length > 0) || 
            (pkg_width > 0) || 
            (pkg_height > 0) )
        {
            xml.Append("<Dimensions>");
            if ((pkg_length) > 0)
            {
                xml.AppendFormat("<Length>{0}</Length>", pkg_length);
            }
            if ((pkg_width) > 0)
            {
                xml.AppendFormat("<Width>{0}</Width>", pkg_width);
            }
            if ((pkg_height) > 0)
            {
                xml.AppendFormat("<Height>{0}</Height>", pkg_height);
            }
            xml.Append("<Units>IN</Units></Dimensions>");
        }
        if (m_residentialaddressindicator != "") {
            xml.Append("<SpecialServices><ResidentialDelivery>1</ResidentialDelivery></SpecialServices>");
        }
        if ((pkg_declaredvalue) > 0) {
            xml.AppendFormat("<DeclaredValue><Value>{0}</Value></DeclaredValue>", pkg_declaredvalue);
        }

        xml.Append("<ListRate>false</ListRate><OriginAddress>");
        xml.AppendFormat("<StateOrProvinceCode>{0}</StateOrProvinceCode>", m_shipperstateprovincecode);
        xml.AppendFormat("<PostalCode>{0}</PostalCode>", m_shipperpostalcode);
        xml.AppendFormat("<CountryCode>{0}</CountryCode>", m_shippercountrycode);
        xml.Append("</OriginAddress><DestinationAddress>");

        if (m_receiverstateprovincecode != "") {
            xml.AppendFormat("<StateOrProvinceCode>{0}</StateOrProvinceCode>", m_receiverstateprovincecode);
        }
        if (m_receiverpostalcode != "") {
            xml.AppendFormat("<PostalCode>{0}</PostalCode>", m_receiverpostalcode);
        }

        xml.AppendFormat("<CountryCode>{0}</CountryCode>", m_receivercountrycode);
        xml.Append("</DestinationAddress><Payment><PayorType>SENDER</PayorType></Payment>");
        xml.Append("<PackageCount>1</PackageCount></FDXRateAvailableServicesRequest>");

        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(fedex_weburl);
        if (proxy_address != "")
        {
            WebProxy prxy = new WebProxy(proxy_address);
            req.Proxy = prxy;
        }
        req.Method = "POST";
        req.ContentType = "application/x-www-form-urlencoded";
        req.ContentLength = xml.Length;
        StreamWriter stOut = new StreamWriter(req.GetRequestStream(), System.Text.Encoding.ASCII);
        stOut.Write(xml.ToString());
        stOut.Close();

        StreamReader stIn = new StreamReader(req.GetResponse().GetResponseStream());
        m_fedex_raw_xml_text = stIn.ReadToEnd();
        stIn.Close();
        xml.Length = 0;


        //'START PARSING
        System.Xml.XmlDocument MyXMLDocument = new System.Xml.XmlDocument();
        MyXMLDocument.LoadXml(m_fedex_raw_xml_text);

        System.Xml.XmlNodeList Error_Node;
        if (p_Carrier == fedex_Carrier_options.FDXE) 
            Error_Node = MyXMLDocument.SelectNodes("/FDXRateAvailableServicesReply/Error/Code");
        else
            Error_Node = MyXMLDocument.SelectNodes("/FDXRateAvailableServicesReply/Error/Code");
        
        int Node_Count;
        Node_Count = Error_Node.Count;
        if (Node_Count > 0) 
        {
            m_fedex_result_code = 1;
            if (p_Carrier == fedex_Carrier_options.FDXE) 
            {
                if (m_fedex_error_description == "") 
                    m_fedex_error_description = string.Format("On Fedex Express: {0}", MyXMLDocument.SelectSingleNode("/FDXRateAvailableServicesReply/Error/Message").InnerText);
                else
                    m_fedex_error_description = string.Format("{0}; On Fedex Express: {1}", 
                        m_fedex_error_description, MyXMLDocument.SelectSingleNode("/FDXRateAvailableServicesReply/Error/Message").InnerText);
            }
            else
            {
                if (m_fedex_error_description == "") 
                    m_fedex_error_description = string.Format("On Fedex Ground: {0}", MyXMLDocument.SelectSingleNode("/FDXRateAvailableServicesReply/Error/Message").InnerText);
                else
                    m_fedex_error_description =  string.Format("{0}; On Fedex Ground: {1}", 
                        m_fedex_error_description, MyXMLDocument.SelectSingleNode("/FDXRateAvailableServicesReply/Error/Message").InnerText);
            }
        }
        else
        {
            System.Xml.XmlNodeList pkg_NodeList;
            pkg_NodeList = MyXMLDocument.SelectNodes("/FDXRateAvailableServicesReply/Entry");
            
            if (p_Carrier == fedex_Carrier_options.FDXE) 
            {
                int count = pkg_NodeList.Count;
                fdxe_rates = new List<fedex_Rate_Detail>(count);

                foreach(System.Xml.XmlNode node in pkg_NodeList)
                {
                    fedex_Rate_Detail rate = new fedex_Rate_Detail();

                    rate.CarrierCode = CarrierCode;

                    System.Xml.XmlNodeList check_node;
                    check_node = MyXMLDocument.SelectNodes("/FDXRateAvailableServicesReply/Entry/DeliveryDate");
                    if (check_node.Count > 0) 
                        rate.fedex_deliverydate = node.SelectSingleNode("DeliveryDate").InnerText;
                    
                    check_node = MyXMLDocument.SelectNodes("/FDXRateAvailableServicesReply/Entry/DeliveryDay");
                    if (check_node.Count > 0)
                        rate.fedex_deliveryday = node.SelectSingleNode("DeliveryDay").InnerText;

                    rate.shipping_cost = Convert.ToDecimal(node.SelectSingleNode("EstimatedCharges/DiscountedCharges/NetCharge").InnerText);
                    rate.description = node.SelectSingleNode("Service").InnerText;
                    if (rate.fedex_deliverydate != "")
                        rate.speed = rate.fedex_deliverydate;

                    if (rate.fedex_deliveryday != "")
                        rate.speed = string.Format("{0} {1}", rate.speed, rate.fedex_deliveryday);

                    if (rate.fedex_timeintransit != "") 
                    {
                        if (rate.speed != "") 
                            rate.speed = string.Format("{0} day(s), {1}", rate.fedex_timeintransit, rate.speed);
                        else
                            rate.speed = string.Format("{0} day(s)", rate.fedex_timeintransit);
                    }

                    fdxe_rates.Add(rate);
                } 
            }
            else
            {
                int count = pkg_NodeList.Count;
                fdxg_rates = new List<fedex_Rate_Detail>(count);

                string mm_TimeInTransit;
                mm_TimeInTransit = "";

                foreach (System.Xml.XmlNode node in pkg_NodeList)
                {
                    fedex_Rate_Detail rate = new fedex_Rate_Detail();

                    rate.CarrierCode = "FDXG";
                    System.Xml.XmlNodeList check_node;
                    check_node = MyXMLDocument.SelectNodes("/FDXRateAvailableServicesReply/Entry/TimeInTransit");
                    if (check_node.Count > 0) {
                        rate.fedex_timeintransit = node.SelectSingleNode("TimeInTransit").InnerText;
                        mm_TimeInTransit = string.Format("{0} days", node.SelectSingleNode("TimeInTransit").InnerText);
                    }
                    rate.shipping_cost = Convert.ToDecimal(node.SelectSingleNode("EstimatedCharges/DiscountedCharges/NetCharge").InnerText);
                    rate.description = node.SelectSingleNode("Service").InnerText;
                    if (rate.fedex_deliverydate != "") 
                        rate.speed = rate.fedex_deliverydate;
                    
                    if (rate.fedex_deliveryday != "") 
                        rate.speed = string.Format("{0} {1}", rate.speed, rate.fedex_deliveryday);
                    
                    if (rate.fedex_timeintransit != "") 
                    {
                        if (rate.speed != "") 
                            rate.speed = string.Format("{0} day(s), {1}", rate.fedex_timeintransit, rate.speed);
                        else
                            rate.speed = string.Format("{0} day(s)", rate.fedex_timeintransit);
                    }

                    fdxg_rates.Add(rate);
                }
            }
        }
    }
    catch(Exception e)
    {
        m_fedex_result_code = 2;
        m_fedex_error_description = e.Message;
    }
}

public void fedex_execute(fedex_Carrier_options CarrierType)
{
    m_fedex_error_description = "";
    switch (CarrierType)
    {
        case fedex_Carrier_options.ALL:
            execute_fdx(fedex_Carrier_options.FDXE);
            if (fedex_result_code == 0)
            {
                execute_fdx(fedex_Carrier_options.FDXG);
                if (fedex_result_code == 0)
                {
                    int count = fdxe_rates.Count + fdxg_rates.Count;
                    fedex_rates = new List<fedex_Rate_Detail>(count);

                    fedex_rates.AddRange(fdxe_rates);
                    fedex_rates.AddRange(fdxg_rates);
                }
                else //' No GROUND results so use EXPRESS only
                {
                    m_fedex_result_code = 0; //'Reset Result Code to success of previous Carrier Code

                    fedex_rates = new List<fedex_Rate_Detail>(fdxe_rates.Count);

                    fedex_rates.AddRange(fdxe_rates);
                }
            }
            else //'No EXPRESS results, so try GROUND only
            {
                execute_fdx(fedex_Carrier_options.FDXG);
                if (fedex_result_code == 0)
                {
                    fedex_rates = new List<fedex_Rate_Detail>(fdxg_rates.Count);

                    fedex_rates.AddRange(fdxg_rates);
                }
            }
            break;
        case fedex_Carrier_options.FDXE:
            execute_fdx(fedex_Carrier_options.FDXE);
            if (fedex_result_code == 0) 
            {
                fedex_rates = new List<fedex_Rate_Detail>(fdxe_rates.Count);

                fedex_rates.AddRange(fdxe_rates);
            }
            break;
        case fedex_Carrier_options.FDXG:
            execute_fdx(fedex_Carrier_options.FDXG);
            if (fedex_result_code == 0) 
            {
                fedex_rates = new List<fedex_Rate_Detail>(fdxg_rates.Count);

                fedex_rates.AddRange(fdxg_rates);
            }
            break;
        }
}
    #endregion

    #region UPS

    //=========================================================================================================
    // UPS Section
    //=========================================================================================================
    public Boolean ups_will_compute;


    public enum ups_pickuptypecode_options 
    {
        _01DailyPickup,
        _03CustomerCounter,
        _06OneTimePickup,
        _07OnCallAir,
        _11SuggestedRetailRates,
        _20AirServiceCenter,
    }

    public string ups_weburl;
    public string ups_accesslicensenumber;
    public string ups_userid;
    public string ups_password;
  
  
    private string m_ups_pickuptypecode;
    public ups_pickuptypecode_options ups_pickuptypecode
    {
        set
        {
            switch (value)
            {
                case ups_pickuptypecode_options._01DailyPickup:
                    m_ups_pickuptypecode = "01";
                    m_ups_customerclassification = "01";
                    break;
                case ups_pickuptypecode_options._03CustomerCounter:
                    m_ups_pickuptypecode = "03";
                    m_ups_customerclassification = "04";
                    break;
                case ups_pickuptypecode_options._06OneTimePickup:
                    m_ups_pickuptypecode = "06";
                    m_ups_customerclassification = "03";
                    break;
                case ups_pickuptypecode_options._07OnCallAir:
                    m_ups_pickuptypecode = "07";
                    m_ups_customerclassification = "03";
                    break;
                case ups_pickuptypecode_options._11SuggestedRetailRates:
                    m_ups_pickuptypecode = "11";
                    m_ups_customerclassification = "03";
                    break;
                case ups_pickuptypecode_options._20AirServiceCenter:
                    m_ups_pickuptypecode = "20";
                    m_ups_customerclassification = "03";
                    break;
                default:
                    m_ups_pickuptypecode = "01";
                    break;
            }
        }
    }

    public enum ups_customerclassification_options
    {
        _01WholeSale,
        _03Occasional,
        _04Retail,
    }
    
    private string m_ups_customerclassification;
    public ups_customerclassification_options ups_customerclassification
    {
        set
        {
            switch (value) 
            {
                case ups_customerclassification_options._01WholeSale:
                    m_ups_customerclassification = "01";
                    break;
                case ups_customerclassification_options._03Occasional:
                    m_ups_customerclassification = "03";
                    break;
                case ups_customerclassification_options._04Retail:
                    m_ups_customerclassification = "04";
                    break;
                default:
                    m_ups_customerclassification = "03";
                    break;
            }
        }
    }

    public enum ups_packagetype_options
    {
        _01UPS_Letter_or_Envelope,
        _02Package,
        _03UPS_Tube,
        _04UPS_Pak,
        _21UPS_Express_Box,
        _24UPS_25Kg_Box,
        _25UPS_10Kg_Box,
    }
    
    private string m_ups_packagetype;
    public ups_packagetype_options ups_packagetype
    {
        set
        {
            switch (value) 
            {
                case ups_packagetype_options._01UPS_Letter_or_Envelope:
                    m_ups_packagetype = "01";
                    break;
                case ups_packagetype_options._02Package:
                    m_ups_packagetype = "02";
                    break;
                case ups_packagetype_options._03UPS_Tube:
                    m_ups_packagetype = "03";
                    break;
                case ups_packagetype_options._04UPS_Pak:             
                    m_ups_packagetype = "04";
                    break;
                case ups_packagetype_options._21UPS_Express_Box:
                    m_ups_packagetype = "21";
                    break;
                case ups_packagetype_options._24UPS_25Kg_Box:
                    m_ups_packagetype = "24";
                    break;
                case ups_packagetype_options._25UPS_10Kg_Box:
                    m_ups_packagetype = "25";
                    break;
            }
        }
    }

    public string ups_shippernumber;
    public string ups_largepackageindicator;

    public struct ups_rate_detail
    {
        public string description;
        public string ups_code;
        public decimal shipping_cost;
        public string speed;
        public string ups_scheduleddeliverytime;
    }
    public List<ups_rate_detail> ups_rates;

    private int m_ups_result_code;
    public int ups_result_code
    {
        get
        {
            return (m_ups_result_code);
        }
    }

    private string m_ups_error_description;
    public string ups_error_description
    {
        get
        {
            return (m_ups_error_description);
        }
    }

    private string m_ups_raw_xml_text;
    public string ups_raw_xml_text
    {
        get
        {
            return (m_ups_raw_xml_text);
        }
    }

    public decimal ups_insured_value;
  
    private string m_ups_saturdaypickup;
    public TrueorFalse_options ups_saturdaypickup
    {
        set
        {
            if (value == TrueorFalse_options._true)
                m_ups_saturdaypickup = "True";
            else
                m_ups_saturdaypickup = "";
        }
    }
    
    private string m_ups_saturdaydelivery;
    public TrueorFalse_options ups_saturdaydelivery
    {
        set
        {
            if (value == TrueorFalse_options._true)
                m_ups_saturdaydelivery = "True";
            else
                m_ups_saturdaydelivery = "";
        }
    }

    private string m_residentialaddressindicator;
    public TrueorFalse_options residentialaddressindicator
    {
        set
        {
            if (value == TrueorFalse_options._true)
            {
                m_residentialaddressindicator = "True";
            }
            else
            {
                m_residentialaddressindicator = "";
            }
        }
    }

    public void ups_execute()
    {
        try
        {
            decimal upsWeight = shipmentweight;
            if (upsWeight < 1.0M)
                upsWeight = 1;

            xml.Length = 0;

            xml.AppendFormat("<?xml version='1.0'?><AccessRequest xml:lang='en-US'><AccessLicenseNumber>{0}</AccessLicenseNumber>", ups_accesslicensenumber);
            xml.AppendFormat("<UserId>{0}</UserId><Password>{1}</Password></AccessRequest><?xml version='1.0'?>", ups_userid, ups_password);
            xml.Append("<RatingServiceSelectionRequest xml:lang='en-US'>");
            xml.Append("<Request><TransactionReference><CustomerContext>Rating and Service</CustomerContext><XpciVersion>1.0001</XpciVersion></TransactionReference>");
            xml.Append("<RequestAction>Rate</RequestAction>");
            xml.Append("<RequestOption>shop</RequestOption></Request>");
            xml.AppendFormat("<PickupType><Code>{0}</Code></PickupType>", m_ups_pickuptypecode);
            xml.AppendFormat("<CustomerClassification><Code>{0}</Code></CustomerClassification>", m_ups_customerclassification);
            xml.Append("<Shipment><Shipper>");

            if (ups_shippernumber != "")
                xml.AppendFormat("<ShipperNumber>{0}</ShipperNumber>", ups_shippernumber);

            xml.AppendFormat("<Address><city>{0}</city>", shippercity);
            xml.AppendFormat("<StateProvinceCode>{0}</StateProvinceCode>", m_shipperstateprovincecode);
            xml.AppendFormat("<PostalCode>{0}</PostalCode>", m_shipperpostalcode);
            xml.AppendFormat("<CountryCode>{0}</CountryCode>", m_shippercountrycode);
            xml.Append("</Address></Shipper><ShipTo><Address>");
            xml.AppendFormat("<city>{0}</city>", receivercity);
            xml.AppendFormat("<StateProvinceCode>{0}</StateProvinceCode>", m_receiverstateprovincecode);
            xml.AppendFormat("<PostalCode>{0}</PostalCode>", m_receiverpostalcode);
            xml.AppendFormat("<CountryCode>{0}</CountryCode>", m_receivercountrycode);

            if (m_residentialaddressindicator != "")
                xml.Append("<ResidentialAddressIndicator></ResidentialAddressIndicator>");

            xml.Append("</Address></ShipTo><Service><Code>11</Code></Service><Package>");
            xml.AppendFormat("<PackagingType><Code>{0}</Code>", m_ups_packagetype);
            xml.Append("<Description>Package</Description></PackagingType><Dimensions>");
            xml.AppendFormat("<Length>{0}</Length>", pkg_length);
            xml.AppendFormat("<Width>{0}</Width>", pkg_width);
            xml.AppendFormat("<Height>{0}</Height>", pkg_height);
            xml.Append("</Dimensions><Description>Rate Shopping</Description>");
            xml.AppendFormat("<PackageWeight><Weight>{0}</Weight></PackageWeight>", upsWeight.ToString());

            if (ups_largepackageindicator != null && ups_largepackageindicator != "")
                xml.Append("<LargePackageIndicator>1</LargePackageIndicator>");

            xml.AppendFormat("<PackageServiceOptions><InsuredValue><MonetaryValue>{0}</MonetaryValue></InsuredValue></PackageServiceOptions>", ups_insured_value);
            xml.Append("</Package><ShipmentServiceOptions>");


            if (m_ups_saturdaypickup != "")
                xml.Append("<SaturdayPickupIndicator></SaturdayPickupIndicator>");

            if (m_ups_saturdaydelivery != "")
                xml.Append("<SaturdayDeliveryIndicator></SaturdayDeliveryIndicator>");

            xml.Append("</ShipmentServiceOptions></Shipment></RatingServiceSelectionRequest>");


            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(ups_weburl);
            if (proxy_address != "")
            {
                WebProxy prxy = new WebProxy(proxy_address);
                req.Proxy = prxy;
            }
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            req.ContentLength = xml.Length;
            StreamWriter stOut = new StreamWriter(req.GetRequestStream(), System.Text.Encoding.ASCII);
            stOut.Write(xml.ToString());
            stOut.Close();
            StreamReader stIn = new StreamReader(req.GetResponse().GetResponseStream());
            m_ups_raw_xml_text = stIn.ReadToEnd();
            stIn.Close();
            xml.Length = 0;

            //START PARSING

            System.Xml.XmlDocument MyXMLDocument = new System.Xml.XmlDocument();
            MyXMLDocument.LoadXml(m_ups_raw_xml_text);

            System.Xml.XmlNode Success_Node;
            Success_Node = MyXMLDocument.SelectSingleNode("/RatingServiceSelectionResponse/Response/ResponseStatusCode");

            if (Success_Node.InnerText != "1")
            {
                m_ups_result_code = 1;
                m_ups_error_description = MyXMLDocument.SelectSingleNode("/RatingServiceSelectionResponse/Response/Error/ErrorDescription").InnerText;
            }
            else
            {
                System.Xml.XmlNodeList RatedShipment_NodeList;
                RatedShipment_NodeList = MyXMLDocument.SelectNodes("/RatingServiceSelectionResponse/RatedShipment");

                int count = RatedShipment_NodeList.Count;
                ups_rates = new List<ups_rate_detail>(count);

                foreach (System.Xml.XmlNode node in RatedShipment_NodeList)
                {
                    ups_rate_detail rate = new ups_rate_detail();

                    rate.ups_code = node.SelectSingleNode("Service/Code").InnerText;
                    rate.shipping_cost = Convert.ToDecimal(node.SelectSingleNode("TotalCharges/MonetaryValue").InnerText);
                    rate.ups_scheduleddeliverytime = node.SelectSingleNode("ScheduledDeliveryTime").InnerText;
                    rate.speed = node.SelectSingleNode("GuaranteedDaysToDelivery").InnerText;

                    if (rate.speed != "")
                        rate.speed = rate.speed + " day(s)";

                    switch (rate.ups_code)
                    {
                        case "01":
                            rate.description = "UPS Next Day Air.";
                            break;
                        case "02":
                            rate.description = "UPS 2nd Day Air.";
                            break;
                        case "03":
                            rate.description = "UPS Ground";
                            break;
                        case "07":
                            if (m_shippercountrycode.ToUpper() == "US" || m_shippercountrycode.ToUpper() == "CA" || m_shippercountrycode.ToUpper() == "PR")
                                rate.description = "UPS Worldwide ExpressSM";
                            else
                                rate.description = "UPS Express";
                            break;
                        case "08":
                            if (m_shippercountrycode.ToUpper() == "US" || m_shippercountrycode.ToUpper() == "CA" || m_shippercountrycode.ToUpper() == "PR")
                                rate.description = "UPS Worldwide ExpeditedSM";
                            else
                                rate.description = "UPS Expedited";
                            break;
                        case "11":
                            rate.description = "UPS Standard UPS";
                            break;
                        case "12":
                            rate.description = "UPS 3 Day Select";
                            break;
                        case "13":
                            rate.description = "UPS Next Day Air Saver";
                            break;
                        case "14":
                            rate.description = "UPS Next Day Air. Early A.M.";
                            break;
                        case "54":
                            rate.description = "UPS Worldwide Express PlusSM";
                            break;
                        case "59":
                            rate.description = "UPS 2nd Day Air A.M.";
                            break;
                        case "65":
                            rate.description = "UPS Express Saver";
                            break;
                    }

                    ups_rates.Add(rate);
                }
                m_ups_result_code = 0;
                m_ups_error_description = "";
            }
        }
        catch (Exception e)
        {
            m_ups_result_code = 2;
            m_ups_error_description = e.Message;
        }
    }
    
    #endregion
}