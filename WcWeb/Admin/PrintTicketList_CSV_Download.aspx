<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PrintTicketList_CSV_Download.aspx.cs" MasterPageFile="~/TemplatePrint.master" 
Inherits="WillCallWeb.Admin.PrintTicketList_CSV_Download" Title="Admin - Ticket Sales Listing" %>
<asp:Content ID="MainContent" runat="server" ContentPlaceHolderID="MainContent">

    <div id="printtickets" class="tickets">
        <br /><br />
        <asp:Button ID="btnExport" runat="server" Text = "Export" onclick="btnExport_Click" />
        <br />
        <br />
        <h4>TICKET ORDERS - </h4>
        
        <asp:ValidationSummary EnableViewState="false" HeaderText="The following errors ocurred:" ID="ValidationSummary1" runat="server" 
            ValidationGroup="Order" CssClass="validationsummary" Width="100%" />
        <asp:CustomValidator ID="CustomValidation" runat="server" EnableViewState="false" ValidationGroup="Order" CssClass="invisible" Display="Static" ErrorMessage="CustomValidator">*</asp:CustomValidator>

        <asp:GridView ID="GridView1" runat="server" OnDataBinding="GridView1_DataBinding" AutoGenerateColumns="false">
            <Columns>
                <asp:BoundField DataField="UniqueId" HeaderText="InvoiceId" />
                <asp:BoundField DataField="InvoiceDate" HeaderText="InvoiceDate" HtmlEncode="false" DataFormatString="{0:MM/dd/yyyy hh:mmtt}" />
                <asp:BoundField DataField="ShipItemId" HeaderText="ShipItemId" />
                <asp:BoundField DataField="Name" HeaderText="Name" />
                <asp:BoundField DataField="Address1" HeaderText="Address1" />
                <asp:BoundField DataField="Address2" HeaderText="Address2" />
                <asp:BoundField DataField="Zip" HeaderText="Zip" />
                <asp:BoundField DataField="City" HeaderText="City" />
                <asp:BoundField DataField="State" HeaderText="State" />
                <asp:BoundField DataField="Country" HeaderText="Country" />
                <asp:BoundField DataField="Phone" HeaderText="Phone" />
                <asp:BoundField DataField="BillingName" HeaderText="BillingName" />
                <asp:BoundField DataField="PurchaseEmail" HeaderText="PurchaseEmail" />
                <asp:BoundField DataField="PackingListIds" HeaderText="PackingListIds" />
                <asp:BoundField DataField="PackingListDescription" HeaderText="PackingListDescription" />
            </Columns>
        </asp:GridView>
        </div>
</asp:Content>