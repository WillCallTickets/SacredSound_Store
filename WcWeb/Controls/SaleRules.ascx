<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SaleRules.ascx.cs" Inherits="WillCallWeb.Controls.SaleRules" %>
<div id="salerules">
    <asp:Repeater ID="rptFeatured" runat="server">
        <ItemTemplate>
            <div class="salerule" runat="server">
                <div class="title"><%# Eval("Name") %></div>
                <div class="text"><%# Eval("DisplayText") %></div>
            </div>
        </ItemTemplate>
    </asp:Repeater>
</div>