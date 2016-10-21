<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Merch_OrderFeatured.ascx.cs" Inherits="WillCallWeb.Admin.AdminControls.Merch_OrderFeatured" %>
<%@ Register Src="Menu_MerchListing.ascx" TagName="Menu_MerchListing" TagPrefix="uc1" %>

<div class="jqhead rounded">
<div id="fetord-items">
    <div id="lmt-display">        
        <table border="0" cellpadding="0" cellspacing="0">
            <tr class="hdf">
                <td colspan="2">
                    <h3>Featured Item Ordering</h3>
                </td>
                <td><asp:Button ID="btnReset" runat="server" Text="Reset Order To Alphabetical" OnClick="btnReset_Click" CssClass="" /></td>
            </tr>
            <tr><td colspan="3"><asp:ValidationSummary ID="ValidationSummary1" CssClass="validationsummary" HeaderText="" ValidationGroup="feat" runat="server" /></td></tr>
            <tr>
                <th style="font-size:14px;text-align:left;white-space:nowrap;">Max Items To Display&nbsp;</th>
                <td style="font-size:10px;">
                    <asp:TextBox ID="txtMax" runat="server" Width="80px" OnDataBinding="txtMax_DataBind" />
                    <asp:RequiredFieldValidator Display="static" CssClass="validation" ValidationGroup="feat" 
                        ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please enter a limit of items to display." 
                        ControlToValidate="txtMax">*</asp:RequiredFieldValidator>
                    <asp:CompareValidator Display="dynamic" CssClass="validation"  ValidationGroup="feat" 
                        ID="CompareValidator6" runat="server" ErrorMessage="Please enter a numeric quantity."
                        ControlToValidate="txtMax" Operator="DataTypeCheck" Type="Integer">*</asp:CompareValidator>
                    <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="txtMax"
                        CssClass="validation" ErrorMessage="Please enter a positive value for the limit." Display="dynamic"
                        MaximumValue="1000" MinimumValue="0" Type="Integer" ValidationGroup="feat">*</asp:RangeValidator> 
                </td>
                <td style="width:95%;">
                    <asp:Button ID="btnMax" runat="server" Text="Update Max Items" OnClick="btnMax_Click" CssClass="btntny" ValidationGroup="feat" />
                </td>
            </tr>
            <tr>
                <td colspan="3" style="font-size:11px;">
                    ***This will limit the number of featured items to display in the menus and on the listing page. Use 0 for no limit.
                    <br />
                    ***Items that are over the limit are listed below the main list.
                    <br />
                    ***If you don't see your item note that this list will not show internal items. 
                </td>
            </tr>
        </table>    
        <asp:Literal ID="litDisplay" runat="server" OnDataBinding="litDisplay_DataBinding" />
    </div>
</div>
</div>
<input type="hidden" id="hidMax" value='<%=this.MaxItems %>' />

<script type="text/javascript" src="/JQueryUI/orderer.js"></script>
