<%@ Page Language="C#" MasterPageFile="~/Template.master" AutoEventWireup="true" CodeFile="About.aspx.cs" Inherits="Store_About" Title="About" EnableViewState="false" %>


<%@ Register Src="../Controls/Menu_Item_Vertical.ascx" TagName="Menu_Item_Vertical"
    TagPrefix="uc1" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" Runat="Server">
   <asp:Panel ID="Content" runat="server" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="SideContent" Runat="Server">
    <uc1:Menu_Item_Vertical id="Menu_Item_Vertical1" runat="server">
    </uc1:Menu_Item_Vertical>
</asp:Content>