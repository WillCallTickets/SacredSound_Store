<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="shipping_demo.aspx.cs" Inherits="shipping_demo"  
MaintainScrollPositionOnPostback ="true" %>

<%-- 
'************************************************************************
' ECOMMERCEMAX SOLUTIONS http://www.ecommercemax.com
' Contact: info@ecommercemax.com
' December 2005
'
'*************************************************************************************
' IMPORTANT: YOU MAY NOT PUBLISH THIS SOURCE CODE IN PUBLICLY ACCESSIBLE WEBSITES LIKE, 
' BUT NOT LIMITED TO FORUMS, NEWSLETTERS, NEWSGROUPS ETC.

' *** YOU MAY NOT RESELL THIS SCRIPT. ***

'*************************************************************************************

' *** WARRANTY DISCLAIMER. THE CODES ON THIS SCRIPT PACKAGE ARE PROVIDED    ***
' *** "AS IS" WITHOUT WARRANTIES OF ANY KIND EITHER EXPRESS OR IMPLIED. TO  ***
' *** THE FULLEST EXTENT POSSIBLE PURSUANT TO THE APPLICABLE LAW,           ***
' *** ECOMMERCEMAX SOLUTIONS DISCLAIMS ALL WARRANTIES, EXPRESSED OR         ***
' *** IMPLIED, INCLUDING, BUT NOT LIMITED TO, IMPLIED WARRANTIES OF         ***
' *** MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE, NON-INFRINGEMENT   ***
' *** OR OTHER VIOLATION OF RIGHTS. ECOMMERCEMAX SOLUTIONS DOES NOT         ***
' *** WARRANT OR MAKE ANY REPRESENTATIONS REGARDING THE USE, VALIDITY,      ***
' *** ACCURACY, OR RELIABILITY OF, OR THE RESULTS OF THE USE OF, OR         ***
' *** OTHERWISE RESPECTING, THE CODES ON THIS SCRIPT PACKAGE OR ANY         ***
' *** RESOURCES USED ON THIS SCRIPT PACKAGE.                                ***

' *** Limitation of Liability.                                                     ***
' *** IN NO EVENT WILL ECOMMERCEMAX SOLUTIONS, OR OTHER THIRD PARTIES              ***
' *** MENTIONED AT THIS SITE BE LIABLE FOR ANY DAMAGES WHATSOEVER (INCLUDING,      ***
' *** WITHOUT LIMITATION, THOSE RESULTING FROM LOST PROFITS, LOST DATA OR          ***
' *** BUSINESS INTERRUPTION) ARISING OUT OF THE USE, INABILITY TO USE, OR THE      ***
' *** RESULTS OF USE OF THIS SCRIPTS PACKAGE, ANY WEB SITES LINKED TO THIS TOOL,   ***
' *** OR THE MATERIALS OR INFORMATION CONTAINED HERE, WHETHER BASED ON WARRANTY,   ***
' *** CONTRACT, TORT OR ANY OTHER LEGAL THEORY AND WHETHER OR NOT ADVISED OF THE   ***
' *** POSSIBILITY OF SUCH DAMAGES. IF YOUR USE OF THE MATERIALS OR INFORMATION     ***
' *** FROM THIS TOOL RESULTS IN THE NEED FOR SERVICING, REPAIR OR CORRECTION OF    ***
' *** EQUIPMENT OR DATA, YOU ASSUME ALL COSTS THEREOF.                             ***
'*************************************************************************************

--%> 

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Ecommercemax |  USPS Domestic Shipping Realtime Rates - Demo</title>
</head>
<body style="font-size: 8pt; font-family: Arial; text-align: left;">
    <form id="form1" runat="server">
    <div style="font-size: 8pt; font-family: Arial">
        <asp:HyperLink ID="HyperLink1" runat="server" Font-Bold="True" Font-Names="Arial"
            Font-Size="Large" Font-Underline="True" NavigateUrl="http://www.ecommercemax.com">ECOMMERCEMAX SOLUTIONS</asp:HyperLink><br />
        <br />
        <strong><span style="font-size: 14pt">USPS, Fedex, and UPS Realtime Rates calculator<br />
        </span></strong>
        Demo page - This example form is written on ASP.NET 2. However,
        the core module or class that it uses to perform the actual live shipping calculation
        can be used in ASP.NET 1.1 forms as well.<br />
        <br />
        Typically, you would initialize the SHIPPER information within your code. They could
        be hardcoded or coming from a database. In this example, we initialized the shipper
        information within the code. View "Source" to view/edit those settings. The SHIPTO
        information is initialized with default values using the form below just for testing and demo
        purposes. In a real-life ecommerce application, we would normally initialize these values within the application codes too.<br />
        <br />
        SHIP TO INFORMATION<br />
        <table style="width: 808px" border="1"  cellpadding="5" cellspacing="0">
            <tr>
                <td bgcolor="deepskyblue" style="width: 128px; height: 26px">
                    Weight In LBS</td>
                <td bgcolor="deepskyblue" style="width: 322px; height: 26px">
                    <asp:TextBox ID="tb_wt" runat="server">10</asp:TextBox></td>
            </tr>
            <tr>
                <td bgcolor="deepskyblue" style="width: 128px; height: 26px">
                    Ship to postal code</td>
                <td bgcolor="deepskyblue" style="width: 322px; height: 26px">
                    <asp:TextBox ID="tb_zip" runat="server">90210</asp:TextBox></td>
            </tr>
            <tr>
                <td bgcolor="deepskyblue" style="width: 128px; height: 26px">
                    Ship to State</td>
                <td bgcolor="deepskyblue" style="width: 322px; height: 26px">
                    <asp:TextBox ID="tb_state" runat="server">CA</asp:TextBox></td>
            </tr>
            <tr>
                <td bgcolor="deepskyblue" style="width: 128px; height: 26px">
                    Ship to country(USPS use only)</td>
                <td bgcolor="deepskyblue" style="width: 322px; height: 26px">
                    <asp:TextBox ID="tb_country" runat="server"></asp:TextBox>
                    Ex. CANADA<br />
                    For use only by USPS INTERNATIONAL. USPS uses full country name instead of country-codes.<br />
                    To find USPS' official list of supported countries go to http://pe.usps.gov/text/Imm/Immctry.html</td>
            </tr>
            <tr>
                <td bgcolor="deepskyblue" style="width: 128px; height: 26px">
                    Ship to Country Code</td>
                <td bgcolor="deepskyblue" style="width: 322px; height: 26px">
                <asp:TextBox ID="tb_country_code" runat="server">US</asp:TextBox>&nbsp;<br />
                    For use only by UPS and FEDEX. UPS and FEDEX uses country-codes unlike USPS.</td>
            </tr>
            <tr>
                <td bgcolor="deepskyblue" style="width: 128px; height: 26px">
                </td>
                <td bgcolor="deepskyblue" style="width: 322px; height: 26px">
                    <asp:CheckBox ID="cb_USPS" runat="server" Checked="True" Text="USPS" />&nbsp;
                    <asp:CheckBox ID="cb_fedex" runat="server" Text="FEDEX" Checked="True" />&nbsp;
                    <asp:CheckBox ID="cb_UPS" runat="server" Text="UPS" Checked="True" /></td>
            </tr>
            <tr>
                <td bgcolor="#00bfff" colspan="2" style="text-align: center">
                    <br />
                    <asp:Button ID="Button1" runat="server" Text="Submit" OnClick="Button1_Click" /><br />
                </td>
            </tr>
        </table>
        <br />
        After you hit Submit, this page will instantiate the SHIPPING class, initialize properties,
        then run Execute method. In this example, the results were binded to a Dropdown
        and a Radiobutton control. It's up to you how to utilize or present those results.</div>
        <br />
            <hr />
        <strong style="font-size: 8pt; font-family: Arial"> 
            RESULTS:<br />
        </strong>
        <br />
        USPS Result Code:
        <asp:Label ID="usps_result_code" runat="server" Font-Bold="True"></asp:Label><br />
        FEDEX Result Code:
        <asp:Label ID="fedex_result_code" runat="server" Font-Bold="True"></asp:Label><br />
        UPS Result Code:
        <asp:Label ID="ups_result_code" runat="server" Font-Bold="True"></asp:Label><br />
        <br />
        USPS Error (if any):
        <asp:Label ID="usps_error" runat="server" Font-Bold="True"></asp:Label><br />
        FEDEX Error (if any):
        <asp:Label ID="fedex_error" runat="server" Font-Bold="True"></asp:Label><br />
        UPS Error (if any):
        <asp:Label ID="ups_error" runat="server" Font-Bold="True"></asp:Label><br />
        <br /><br />
        Number of Rate Options returned:
        <asp:Label ID="Label_rates_count" runat="server" Font-Bold="True"></asp:Label><br />
        <br />
        Example 1: Binded to a Dropdown List<br />
        <asp:DropDownList ID="DropDownList1" runat="server">
        </asp:DropDownList><br />
        <br />
        Example 2: Binded to a Radiobutton List<br />
        <asp:RadioButtonList ID="RadioButtonList1" runat="server">
        </asp:RadioButtonList><br />
        <br />
        <strong>Quick Notes:</strong><br />
        USPS:<br />
        &nbsp;+ &nbsp;You need to sign-up for a USPS UserID and password. At the time of
        writing this, USPS has made the password optional. Create a USPS account here:
        <br />
        <asp:HyperLink ID="HyperLink5" runat="server" NavigateUrl="http://www.uspswebtools.com/registration">http://www.uspswebtools.com/registration</asp:HyperLink><br />
        &nbsp;+&nbsp;
        For more information about USPS shipping, visit the official documentation
        on
        <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="http://www.usps.com/webtools/htm/Rates-Calculatorsv1-0.htm">http://www.usps.com/webtools/htm/Rates-Calculatorsv1-0.htm</asp:HyperLink><br />
        &nbsp;+
        For conducting a USPS "canned" test go to &nbsp;<asp:HyperLink ID="HyperLink3" runat="server"
            NavigateUrl="http://www.ecommercemax.com/usps/usps_canned_test.asp">http://www.ecommercemax.com/usps/usps_canned_test.asp</asp:HyperLink><br />
        <br />
        FEDEX requires only an Account Number so you need to go to Fedex.com to sign up
        for an account if you do not have one yet.
        <br />
        For getting a Fedex Meter Number (Meter# is optional &amp; can be set to just "0")
        &nbsp;<asp:HyperLink ID="HyperLink4" runat="server" NavigateUrl="http://www.ecommercemax.com/fedex_shipping/get_fedex_meter.asp">http://www.ecommercemax.com/fedex_shipping/get_fedex_meter.asp</asp:HyperLink><br />
        <br />
        UPS requires you to have&nbsp; an Access License Number, a User ID and an Account
        Password. Remember that a Developer Key is different from the Access License Number.
        But you have to get a Developer Key to get an Access License Number.&nbsp;<br />
        You may create your account here:
        <asp:HyperLink ID="HyperLink6" runat="server" NavigateUrl="http://www.ups.com/e_comm_access/gettools_index?loc=en_US">http://www.ups.com/e_comm_access/gettools_index?loc=en_US</asp:HyperLink><br /><br />
        <br />
    </form>
</body>
</html>
