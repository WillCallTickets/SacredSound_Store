<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Mailers.aspx.cs" MasterPageFile="~/TemplateAdmin.master" 
    Inherits="WillCallWeb.Admin.Mailers" Title="Admin - Edit Mailers" %>
<asp:Content ID="MainContent" runat="server" ContentPlaceHolderID="MainContent">
    <div id="mailers">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Panel ID="Content" runat="server">
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>