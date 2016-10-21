<%@ Page Language="C#" MasterPageFile="~/Template.master" AutoEventWireup="true" CodeFile="Faq.aspx.cs" Inherits="WillCallWeb.Faq" Title="FAQ" %>
<%@ Register src="Controls/Faq.ascx" tagname="Faq" tagprefix="uc1" %>
<%@ Register Src="Controls/Menu_Item_Vertical.ascx" TagName="Menu_Item_Vertical" TagPrefix="uc2" %>
<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" Runat="Server">
    <uc1:Faq ID="Faq1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="SideContent" Runat="Server">
    <uc2:Menu_Item_Vertical id="Menu_Item_Vertical1" runat="server" EnableViewState="false">
    </uc2:Menu_Item_Vertical>
</asp:Content>