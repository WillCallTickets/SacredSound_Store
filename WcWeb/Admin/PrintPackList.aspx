<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PrintPackList.aspx.cs" MasterPageFile="~/TemplatePrint.master" 
Inherits="WillCallWeb.Admin.PrintPackList" Title="Admin - Packing List" %>


<asp:Content ID="MainContent" runat="server" ContentPlaceHolderID="MainContent">

    <asp:ValidationSummary EnableViewState="false" HeaderText="The following errors ocurred:" ID="ValidationSummary1" runat="server" 
        ValidationGroup="Order" CssClass="validationsummary" Width="100%" />
        
    <div id="packing-list" class="controlmaincontent" style="margin:12px;">
        <h2><%=Wcss._Config._SiteTitle %></h2>

        <span class="title">
            PACKING LIST FOR INVOICE - <%=_invoice.InvoiceDate.ToString("ddd MM/dd/yyyy hh:mmtt") %> - <%=_invoice.UniqueId %>
            <asp:CustomValidator ID="CustomValidation" runat="server" EnableViewState="false" ValidationGroup="Order" CssClass="invisible" Display="Static" ErrorMessage="CustomValidator">*</asp:CustomValidator>
        </span>
            
            <fieldset>
                <legend class="controlheader"><span class="title">Ship Method</span></legend>
                <div>
                    ITEMS SHIPPED VIA - <%= _shipment.ShipMethod%>
                    <%if(_shipment.DateShipped <= DateTime.Now.AddMonths(1)) {%>
                    <br />
                    SHIP DATE - <%= _shipment.DateShipped.ToString("MM/dd/yyyy hh:mmtt") %>
                    <%} %>
                </div>
            </fieldset>
            
            <fieldset>
                <legend class="controlheader"><span class="title">Bill To</span></legend>
                <div class="addressinfo"><%=_invoice.PurchaseEmail %></div>
                <div class="addressinfo"><%=_invoice.InvoiceBillShip.BlLastName %>, <%=_invoice.InvoiceBillShip.BlFirstName%></div>
                <div class="addressinfo"><%=_invoice.InvoiceBillShip.BlAddress1%></div>
                <%if (_invoice.InvoiceBillShip.BlAddress2.Trim().Length > 0)
                  { %>
                    <div class="addressinfo"><%=_invoice.InvoiceBillShip.BlAddress2%></div>
                <%} %>
                <div class="addressinfo"><%=_invoice.InvoiceBillShip.BlCity%>, <%=_invoice.InvoiceBillShip.BlStateProvince %>, <%=_invoice.InvoiceBillShip.BlPostalCode%>, <%=_invoice.InvoiceBillShip.BlCountry%></div>
                <div class="addressinfo"><%=_invoice.InvoiceBillShip.BlPhone%></div>
            </fieldset>
            
            <fieldset>
                <legend class="controlheader"><span class="title">Ship To</span></legend>
                <div class="addressinfo"><%=_shipment.FirstName%> <%=_shipment.LastName%></div>
                <div class="addressinfo"><%=_shipment.Address1%></div>
                <%if (_shipment.Address2 != null && _shipment.Address2.Trim().Length > 0)
                  { %>
                    <div class="addressinfo"><%=_shipment.Address2%></div>
                <%} %>
                <div class="addressinfo"><%=_shipment.City%>, <%=_shipment.StateProvince%>, <%=_shipment.PostalCode%>, <%=_shipment.Country%></div>
                <div class="addressinfo"><%=_shipment.Phone%></div>
            </fieldset>
            <fieldset>
                <legend class="controlheader"><span class="title">Gift Message</span></legend>
                <asp:Literal ID="litMessage" runat="server" />
            </fieldset>
            <fieldset>
                <legend class="controlheader"><span class="title">Tracking #&#39;s</span></legend>
                <asp:Literal ID="litTracking" runat="server" />
            </fieldset>
            <fieldset>
                <legend class="controlheader"><span class="title">Items In This Shipment</span></legend>
                <asp:Literal ID="LiteralPackingList" runat="server" />
            </fieldset>
            <asp:Literal ID="litDonations" runat="server" />

    </div>
        
    
   <script type="text/javascript" language="javascript">
   <!--
    window.print();
   //-->
   </script>
</asp:Content>