<%@ Control Language="C#" AutoEventWireup="true" EnableViewState="true" CodeFile="ShipRateListing.ascx.cs" Inherits="WillCallWeb.Components.Util.ShipRateListing" %>
<div class="shiprate">
    <asp:Literal ID="litTitle" runat="server" />
    <asp:Literal ID="litItems" runat="server" />
    <asp:DropDownList ID="listControl" runat="server" OnDataBinding="listControl_DataBinding"
        OnDataBound="listControl_DataBound" AutoPostBack="true" OnSelectedIndexChanged="listControl_SelectedIndexChanged" />
</div>
