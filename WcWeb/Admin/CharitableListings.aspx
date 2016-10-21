<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CharitableListings.aspx.cs" MasterPageFile="~/TemplateAdmin.master" 
    Inherits="WillCallWeb.Admin.CharitableListings" Title="Admin - CharitableListings" %>
<asp:Content ID="MainContent" runat="server" ContentPlaceHolderID="MainContent">
    <div id="editor">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Panel ID="Content" runat="server">
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>