<%@ Page Language="C#" MasterPageFile="~/Template.master" AutoEventWireup="true" CodeFile="Checkout.aspx.cs" Inherits="Store_Checkout" Title="Checkout" %>

<%@ Register Src="../Controls/Menu_Item_Vertical.ascx" TagName="Menu_Item_Vertical"
    TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <asp:Panel ID="panelCheckout" runat="server" />
    <br /><br /><br />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="SideContent" Runat="Server">
    <uc1:Menu_Item_Vertical id="Menu_Item_Vertical1" runat="server">
    </uc1:Menu_Item_Vertical>
</asp:Content>

