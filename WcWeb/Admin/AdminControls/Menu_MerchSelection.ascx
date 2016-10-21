<%@ Control Language="C#" AutoEventWireup="true" EnableViewState="false" CodeFile="Menu_MerchSelection.ascx.cs" 
    Inherits="WillCallWeb.Admin.AdminControls.Menu_MerchSelection" %>
<div class="jqhead rounded" style="border:none red 1px;">
    <asp:Literal ID="litMenu" runat="server" OnDataBinding="litMenu_DataBinding" />
    <span style="border:none yellow 1px;position:relative;left:25px;top:-5px;">
        <asp:Button ID="btnCsv_Full" runat="server" CssClass="btnmed" Width="150px" Text="Shopify CSV Full" CommandName="csv" OnClick="CSV_Click" 
            OnClientClick="return confirm('This will create a CSV file for download. Includes all merch parents, inventory and images. Would you like to proceed?');" />
        
    </span>
</div>