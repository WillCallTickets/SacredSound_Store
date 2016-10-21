<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" MasterPageFile="~/Template.master" 
Inherits="WillCallWeb.WebUser._Default" Title="Account Profile" %>

<%@ Register Src="../Controls/Menu_Item_Vertical.ascx" TagName="Menu_Item_Vertical"
    TagPrefix="uc1" %>
    
<asp:Content ID="MainContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:Panel ID="PanelContent" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="SideContent" Runat="Server">
    <uc1:Menu_Item_Vertical id="Menu_Item_Vertical1" runat="server">
    </uc1:Menu_Item_Vertical>
</asp:Content>