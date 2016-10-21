<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PrintConfirm.aspx.cs" MasterPageFile="~/TemplatePrint.master" Inherits="Store_PrintConfirm" Title="Order Confirmation" %>
<asp:Content ID="MainContent" runat="server" ContentPlaceHolderID="MainContent">

    <div id="printconfirmation">
        <asp:Panel ID="pnlDetails" runat="server" />
        <asp:Panel ID="pnlCart" runat="server" />
    </div>
    
   <script language="javascript" type="text/javascript">
   <!--
    window.print();
   //-->
   </script>
    
</asp:Content>