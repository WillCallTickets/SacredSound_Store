<%@ Control Language="C#" AutoEventWireup="true" EnableViewState="true" CodeFile="ShippingOptions.ascx.cs" Inherits="WillCallWeb.Components.Cart.ShippingOptions" %>
<%@ Register src="~/Components/Util/ShipRateListing.ascx" tagname="ShipRateListing" tagprefix="uc1" %>
<div class="shippingoptions">
    <div class="panel-wrapper">
        <div class="item-container">
            <uc1:ShipRateListing ID="ShipRateListing_Normal" ItemGuid="" runat="server" OnSelectedIndexChanged="ShipRateChange" />
        </div>
        <asp:Repeater ID="rptBackorders" runat="server" OnDataBinding="rptShipping_DataBinding">
            <ItemTemplate>
                <div class="item-container">
                    <uc1:ShipRateListing ID="ShipRateListing_Backorder" ItemGuid='<%#Eval("Guid") %>' runat="server" OnSelectedIndexChanged="ShipRateChange" />
                </div>
            </ItemTemplate>
        </asp:Repeater>
        <asp:Repeater ID="rptSeparate" runat="server" OnDataBinding="rptShipping_DataBinding">
            <ItemTemplate>
                <div class="item-container">
                    <uc1:ShipRateListing ID="ShipRateListing_Separate" ItemGuid='<%#Eval("Guid") %>' runat="server" OnSelectedIndexChanged="ShipRateChange" />
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div> 
</div>