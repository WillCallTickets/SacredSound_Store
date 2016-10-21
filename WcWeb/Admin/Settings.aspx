<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Settings.aspx.cs" MasterPageFile="~/TemplateAdmin.master" 
    Inherits="WillCallWeb.Admin.Settings" Title="Admin - Edit Merch" %>
<asp:Content ID="MainContent" runat="server" ContentPlaceHolderID="MainContent">
    <div id="settingeditor">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Panel ID="Content" runat="server">
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>