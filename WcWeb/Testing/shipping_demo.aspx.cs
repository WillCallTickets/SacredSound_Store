/*
 *************************************************************************************
 ECOMMERCEMAX SOLUTIONS http://www.ecommercemax.com
 Contact: info@ecommercemax.com
 December 2005

 *************************************************************************************
 IMPORTANT: YOU MAY NOT PUBLISH THIS SOURCE CODE IN PUBLICLY ACCESSIBLE WEBSITES LIKE, 
 BUT NOT LIMITED TO FORUMS, NEWSLETTERS, NEWSGROUPS ETC.

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
using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Wcss;

public partial class shipping_demo : System.Web.UI.Page
{
    

    protected void Button1_Click(object sender, EventArgs e)
    {
        ecommercemax_shipping srates = new ecommercemax_shipping();

        const string const_shipperpostalcode = "91306";
        const string const_shippercity = "WINNETKA";
        const string const_shipperstateprovincecode = "CA";
        const string const_shippercountrycode = "US";

        srates.shippercity = const_shippercity;
        srates.shipperpostalcode = const_shipperpostalcode;
        srates.shipperstateprovincecode = const_shipperstateprovincecode;
        srates.shippercountrycode = const_shippercountrycode;

        srates.usps_will_compute = cb_USPS.Checked;
        srates.fedex_will_compute = cb_fedex.Checked;
        srates.ups_will_compute = cb_UPS.Checked;

        srates.receivercountrycode = tb_country_code.Text;
        srates.receiverpostalcode = tb_zip.Text;
        srates.receiverstateprovincecode = tb_state.Text;
        srates.shipmentweight = Convert.ToDecimal(tb_wt.Text);
        //srates.pkg_height = 50;
        //srates.pkg_length  = 50;
        //srates.pkg_width  = 50;
        //srates.pkg_declaredvalue = 1000;
      

        /* ---------------------------------------------------------------------------------------
         * START- USPS INITIALIZATION
         * Note that before you can use the USPS live production server, you have to sign up
         * for your own USPS user id and perform a 'canned' test for that user id.
         * 
         * Here's the utility page for doing a 'canned' test:
         *     http://www.ecommercemax.com/usps/usps_canned_test.asp    
         * -----------------------------------------------------------------------------------------
         */

        if (srates.usps_will_compute )
        {
            //ASSIGN YOUR OWN SETTINGS BY GETTING VALUES FROM YOUR WEB.CONFIG FILE
            srates.usps_userid = System.Configuration.ConfigurationManager.AppSettings["usps_userid"];
            srates.usps_password = System.Configuration.ConfigurationManager.AppSettings["usps_password"];
            
            //In ASP.NET 1.1, you would do this as:
            //srates.usps_userid = ConfigurationSettings.AppSettings["usps_userid"];
            //srates.usps_password = ConfigurationSettings.AppSettings["usps_password"];

            //IF YOU DON'T WANT TO STORE YOUR CREDENTIALS IN WEB.CONFIG, YOU CAN ALSO 
            //DO A DIRECT ASSIGNMENT, LIKE:
            //srates.usps_userid = "MY_USPS_ID"
            //srates.usps_password = "MY PASSWORD"

            //THE COMPONENT AUTOMATICALLY ASSIGNS THIS VALUE, BUT IN CASE USPS CHANGES THEIR 
            //URL PAGE (HIGHLY UNLIKELY) THEN YOU CAN STILL OVERRIDE IT.
            //srates.usps_weburl = "http://production.shippingapis.com/shippingapi.dll";

            //INITIALIZE USPS DEFAULT DOMESTIC options          
            srates.usps_service = ecommercemax_shipping.usps_service_options._all;
            srates.usps_packagesize = ecommercemax_shipping.usps_packagesize_options._regular;
            srates.usps_machinable = ecommercemax_shipping.TrueorFalse_options._false;
            srates.receiverpostalcode = tb_zip.Text;

            //INITIALIZE USPS DEFAULT INTERNATIONAL options  
            srates.usps_mailtype = ecommercemax_shipping.usps_mailtype_options._intl_package;
            srates.usps_receivercountry = tb_country.Text;
            
        }

        /* ---------------------------------------------------------------------------------------
        * START- FEDEX INITIALIZATION
        * Note that the use of a FEDEX meter number is optional.
        * 
        * Here's the free utility page for generating your own Fedex Meter Number:
        *     http://www.ecommercemax.com/fedex_shipping/get_fedex_meter.asp    
        * -----------------------------------------------------------------------------------------
        */
        if (srates.fedex_will_compute )
        {
            //ASSIGN YOUR OWN SETTINGS BY GETTING VALUES FROM YOUR WEB.CONFIG FILE
            srates.fedex_accountnumber = System.Configuration.ConfigurationManager.AppSettings["fedex_accountnumber"];
            srates.fedex_meter_no = System.Configuration.ConfigurationManager.AppSettings["fedex_meternumber"];
            
            //In ASP.NET 1.1, you would do this as:
            //srates.fedex_accountnumber = ConfigurationSettings.AppSettings["fedex_accountnumber"];
            //srates.fedex_meter_no = ConfigurationSettings.AppSettings["fedex_meter_no"];

            //IF YOU DON'T WANT TO STORE YOUR CREDENTIALS IN WEB.CONFIG, YOU CAN ALSO 
            //DO A DIRECT ASSIGNMENT, LIKE:
            //srates.fedex_accountnumber = "MY_fedex_ID"
            //srates.fedex_meternumber = "MY METER NO."

            //THE COMPONENT AUTOMATICALLY ASSIGNS THIS VALUE, BUT IN CASE fedex CHANGES THEIR 
            //URL PAGE (HIGHLY UNLIKELY) THEN YOU CAN STILL OVERRIDE IT.
            //srates.fedex_weburl = "https://gateway.fedex.com/GatewayDC";

            //INITIALIZE FEDEX options          
            srates.fedex_DropoffType = ecommercemax_shipping.fedex_DropoffType_options.REGULARPICKUP;
            srates.fedex_Packaging = ecommercemax_shipping.fedex_Packaging_options.YOURPACKAGING;
            srates.residentialaddressindicator = ecommercemax_shipping.TrueorFalse_options._false;
            srates.shipmentweight = Convert.ToDecimal (tb_wt.Text);
        }

        /* ---------------------------------------------------------------------------------------
         * 
         * START- UPS INITIALIZATION
         * 
         * -----------------------------------------------------------------------------------------
         */
        if (srates.ups_will_compute)
        {
            //ASSIGN YOUR OWN SETTINGS BY GETTING VALUES FROM YOUR WEB.CONFIG FILE
            srates.ups_accesslicensenumber = _Config._UPS_AccessKey;// System.Configuration.ConfigurationManager.AppSettings["ups_accesslicensenumber"];
            srates.ups_userid = _Config._UPS_UserId;// System.Configuration.ConfigurationManager.AppSettings["ups_userid"];
            srates.ups_password = _Config._UPS_UserPass;// System.Configuration.ConfigurationManager.AppSettings["ups_password"];
            srates.ups_shippernumber = _Config._UPS_AccountNum;// System.Configuration.ConfigurationManager.AppSettings["ups_shippernumber"];

            //In ASP.NET 1.1, you would do this as:
            //srates.ups_accesslicensenumber = ConfigurationSettings.AppSettings["ups_accesslicensenumber"];
            //etc...

            //IF YOU DON'T WANT TO STORE YOUR CREDENTIALS IN WEB.CONFIG, YOU CAN ALSO 
            //DO A DIRECT ASSIGNMENT, LIKE:
            //srates.ups_accesslicensenumber = "YOUR ACCESS LIC #"
            //etc..

            //THE COMPONENT AUTOMATICALLY ASSIGNS THIS VALUE, BUT IN CASE UPS CHANGES THEIR 
            //URL PAGE (HIGHLY UNLIKELY) THEN YOU CAN STILL OVERRIDE IT.
            //srates.ups_weburl = "https://www.ups.com/ups.app/xml/Rate?";

            //INITIALIZE UPS options          
            srates.ups_pickuptypecode = ecommercemax_shipping.ups_pickuptypecode_options._01DailyPickup;
            srates.ups_packagetype = ecommercemax_shipping.ups_packagetype_options._02Package;
            srates.shipperpostalcode = const_shipperpostalcode;
            srates.shipperstateprovincecode = const_shipperstateprovincecode;
            srates.shippercountrycode = const_shippercountrycode;
        }

        //EXECUTE THE LIVE CALCULATOR
        srates.execute();

        //ACCESS THE RESULT CODE; 0=SUCCESS, 1+= ERROR
        usps_result_code.Text = srates.usps_result_code.ToString();
        fedex_result_code.Text = srates.fedex_result_code.ToString();
        ups_result_code.Text = srates.ups_result_code.ToString();

        //ACCESS THE ERROR DESCRIPTION IF THERE IS ONE
        usps_error.Text = srates.usps_error_description;
        fedex_error.Text = srates.fedex_error_description;
        ups_error.Text = srates.ups_error_description;

        //INITIALIZE CONTROLS
        Label_rates_count.Text = "";
        DropDownList1.Items.Clear();
        RadioButtonList1.Items.Clear();

        if (srates.shiprates_total_count > 0 )
        {
            //DISPLAY TOTAL NUMBER OF RATES RETURNED
            Label_rates_count.Text = srates.shiprates_total_count.ToString();
            
            ArrayList values = new ArrayList();
            
            string temp_option = "";
            for (int i = 0; i<=(srates.shiprates_total_count - 1);i++)
            {
                //ACCESS THE RETURNED CARRIER CODE
                temp_option = srates.shiprates[i].carrier_code;

                //CONCATENATE THE SHIPPING COST OF A RATE ITEM
                temp_option = temp_option + "--> " + String.Format("{0:c}", srates.shiprates[i].shipping_cost);

                //CONCATENATE THE SHIPPING DESCRIPTION OF A RATE ITEM
                temp_option = temp_option + " - " + srates.shiprates[i].description;

                //CONCATENATE THE SHIPPING DATE ESTIMATE OF A RATE ITEM
                temp_option = temp_option + ", " + srates.shiprates[i].speed;

                //ADD THE CONSTRUCTED STRING
                values.Add(temp_option);
            }

            //POPULATE THE DROPDOWNLIST CONTROL WITH THE CONSTRUCTED VALUES
            DropDownList1.DataSource = values;
            DropDownList1.DataBind();

            //POPULATE THE RADIOBUTTONLIST CONTROL WITH THE CONSTRUCTED VALUES
            RadioButtonList1.DataSource = values;
            RadioButtonList1.DataBind();
        }
    }
}
