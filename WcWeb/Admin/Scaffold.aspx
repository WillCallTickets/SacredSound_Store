<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Scaffold.aspx.cs" MasterPageFile="~/TemplateAdmin.master" 
    Inherits="WillCallWeb.Admin.Scaffold" Title="Admin - Scaffold Editor" %>
<asp:Content ID="MainContent" runat="server" ContentPlaceHolderID="MainContent">
    <div id="scaffoldeditor">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Panel ID="Content" runat="server">
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>