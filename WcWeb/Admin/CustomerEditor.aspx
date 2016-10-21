<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CustomerEditor.aspx.cs" MasterPageFile="~/TemplateAdmin.master" 
Inherits="WillCallWeb.Admin.CustomerEditor" Title="Admin - Customer Editor" %>
<asp:Content ID="MainContent" runat="server" ContentPlaceHolderID="MainContent">
    <div id="customer">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Panel ID="Content" runat="server">
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>