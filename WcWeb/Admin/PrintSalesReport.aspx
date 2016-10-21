<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PrintSalesReport.aspx.cs" MasterPageFile="~/TemplatePrint.master" 
Inherits="WillCallWeb.Admin.PrintSalesReport" Title="Admin - Sales Report" %>
<asp:Content ID="MainContent" runat="server" ContentPlaceHolderID="MainContent">

   <asp:Panel id="pnlSales" runat="server" EnableViewState="false" cssclass="sales" />
    
   <script type="text/javascript">
   <!--
    window.print();
   //-->
   </script>
</asp:Content>