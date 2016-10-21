<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MerchEditor_NoUpdate.aspx.cs" MasterPageFile="~/TemplateAdmin.master" 
Inherits="WillCallWeb.Admin.MerchEditor_NoUpdate" Title="Admin - Edit Merch" %>
<%@ Register src="AdminControls/Menu_MerchSelection.ascx" tagname="Menu_MerchSelection" tagprefix="uc1" %>
<asp:Content ID="MainContent" runat="server" ContentPlaceHolderID="MainContent">
    <div id="mercheditor">
        <uc1:Menu_MerchSelection ID="Menu_MerchSelection1" runat="server" />
        <asp:Panel ID="Content" runat="server">
        </asp:Panel>
    </div>
</asp:Content>