<%@ Page Language="C#" MasterPageFile="~/Template.master" AutoEventWireup="true" 
CodeFile="Confirmation.aspx.cs" Inherits="Store_Confirmation" Title="Confirmation" %>

<%@ Register Src="../Controls/Menu_Item_Vertical.ascx" TagName="Menu_Item_Vertical"
    TagPrefix="uc1" %>
    
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <div id="confirmation">
        <asp:Label ID="lblConfirmationError" runat="server" Visible="false" CssClass="validationsummary" />
        <asp:Panel ID="panelDetails" runat="server" />
        <asp:Panel ID="panelCart" runat="server" />
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="SideContent" Runat="Server">
    <uc1:Menu_Item_Vertical id="Menu_Item_Vertical1" runat="server">
    </uc1:Menu_Item_Vertical>
</asp:Content>

