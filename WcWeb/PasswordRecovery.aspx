<%@ Page Language="C#" MasterPageFile="~/Template.master" AutoEventWireup="true" CodeFile="PasswordRecovery.aspx.cs" Inherits="WillCallWeb.PasswordRecovery" 
Title="Password Recovery" %>

<%@ Register Src="Controls/Menu_Item_Vertical.ascx" TagName="Menu_Item_Vertical" TagPrefix="uc2" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" Runat="Server">
    <asp:Panel ID="panelContent" runat="server" />
</asp:Content>
<asp:content ID="Content3" ContentPlaceHolderID="SideContent" runat="server">
    <uc2:Menu_Item_Vertical id="Menu_Item_Vertical1" runat="server"></uc2:Menu_Item_Vertical>    
</asp:content>