<%@ Page Language="C#" MasterPageFile="~/Template.master" AutoEventWireup="true" CodeFile="Shipping.aspx.cs" Inherits="Store_Shipping" Title="Shipping" %>

<%@ Register Src="../Controls/Menu_Item_Vertical.ascx" TagName="Menu_Item_Vertical"
    TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <asp:Panel ID="panelContent" runat="server" />
    <br /><br /><br />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="SideContent" Runat="Server">
   <uc1:Menu_Item_Vertical id="Menu_Item_Vertical1" runat="server">
    </uc1:Menu_Item_Vertical>
</asp:Content>

