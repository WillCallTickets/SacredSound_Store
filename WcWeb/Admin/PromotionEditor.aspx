<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PromotionEditor.aspx.cs" MasterPageFile="~/TemplateAdmin.master" EnableEventValidation="false"
Inherits="WillCallWeb.Admin.PromotionEditor" Title="Admin - Edit Promotions" %>
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