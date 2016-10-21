<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PurchaseDetails.ascx.cs" Inherits="WillCallWeb.Controls.PurchaseDetails" %>
<%@ Import Namespace="Utils.ExtensionMethods" %>

<div id="purchasedetails">

    <div class="legend">Order Confirmation <asp:Literal ID="LiteralPrint" runat="server" EnableViewState="false" /></div>
        
    <asp:Literal ID="LiteralSendEmail" runat="server" EnableViewState="false" />
        
    <div id="billship">
        <div class="payinfo">
            <div class="title">Payment Information</div>
            <table border="0" cellpadding="0" cellspacing="0">
                <asp:Literal ID="LiteralInvoiceId" runat="server" EnableViewState="false" />
                <tr><th>Sold To:</th><td><%=this._billName %></td></tr>
                <tr><th>Email:</th><td><%=this._auth.Email %></td></tr>
                <tr><th style="vertical-align:top;">Address:</th><td><%=this._billAddress %></td></tr>
                <tr><th>Phone:</th><td><%=_auth.Phone %></td></tr>
            </table>
            <%if (Wcss._Config._Merchant_ChargeStatement_Descriptor.HasValueLength())
                {%>
                    <div class="merchantdescriptor">
                    *Note: Your statement will show a charge from <span><%= Wcss._Config._Merchant_ChargeStatement_Descriptor%></span>
                    </div>
                <%} %>
        </div>
        <div id="shipinfo" runat="server" class="shipinfo" EnableViewState="false">
            <div class="title">Shipping Information:</div>
            <table border="0" cellpadding="0" cellspacing="0">
                <tr><th>Name:</th><td><%=this._shipName %></td></tr>
                <tr><th>Address:</th><td><%=this._shipAddress %></td></tr>
            </table>
        </div>
    </div>
    <div class="clear"></div>
</div>