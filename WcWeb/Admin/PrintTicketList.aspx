<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PrintTicketList.aspx.cs" MasterPageFile="~/TemplatePrint.master" 
Inherits="WillCallWeb.Admin.PrintTicketList" Title="Admin - Ticket Sales Listing" %>
<asp:Content ID="MainContent" runat="server" ContentPlaceHolderID="MainContent">

    <div id="printtickets" class="tickets">
        <h4 style="margin-bottom:8px;">TICKET ORDERS - <%=ShowDateRecord.lst_EventName %></h4>
        <div style="margin-bottom:12px;">
            <input type="button" onclick="window.print();" value="Print" style="margin-right:12px;" />
            <asp:Button ID="btnExport" runat="server" Text = "Export" onclick="btnExport_Click" />
            <span class="checkbox" style="margin-left:12px;border:solid #666 1px;padding:4px;vertical-align:middle;">
                <label>
                    <input type="checkbox" id="chkPhone" runat="server" />Display Phone #'s
                </label>
            </span>
        </div>
        <asp:ValidationSummary EnableViewState="false" HeaderText="The following errors ocurred:" ID="ValidationSummary1" runat="server" 
            ValidationGroup="Order" CssClass="validationsummary" Width="100%" />
        <asp:CustomValidator ID="CustomValidation" runat="server" EnableViewState="false" ValidationGroup="Order" CssClass="invisible" Display="Static" ErrorMessage="CustomValidator">*</asp:CustomValidator>

        <asp:Literal ID="LiteralInventory" runat="server" EnableViewState="false"/>
        <br />
        <asp:Literal ID="LiteralSales" runat="server" EnableViewState="false" />
        </div>
    
    
   <script type="text/javascript">
   <!--
    window.print();
   //-->
   </script>
</asp:Content>