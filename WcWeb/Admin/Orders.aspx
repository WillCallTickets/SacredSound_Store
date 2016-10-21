<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Orders.aspx.cs" MasterPageFile="~/TemplateAdmin.master" 
Inherits="WillCallWeb.Admin.Orders" Title="Admin - Orders" %>
<asp:Content ID="MainContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
    </asp:ScriptManagerProxy>
    <div id="orders">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Panel ID="Content" runat="server">
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>