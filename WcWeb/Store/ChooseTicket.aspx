<%@ Page Language="C#" MasterPageFile="~/Template.master" AutoEventWireup="true" CodeFile="ChooseTicket.aspx.cs" Inherits="Store_ChooseTicket" Title="Choose Tickets" %>
<%@ Register Src="~/Controls/Menu_Item_Vertical.ascx" TagName="Menu_Item_Vertical" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    
    <%if (WillCallWeb.Globals.ShowId > 0){ %>
    <asp:Panel ID="panelTicket" runat="server" />
   <%} else { %>
    <asp:Panel ID="panelShow" runat="server" />
   <%} %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="SideContent" Runat="Server">
    <uc1:Menu_Item_Vertical id="Menu_Item_Vertical1" runat="server" EnableViewState="false" />
</asp:Content>