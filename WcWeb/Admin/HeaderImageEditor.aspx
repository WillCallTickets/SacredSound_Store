<%@ Page Language="C#" AutoEventWireup="true" CodeFile="HeaderImageEditor.aspx.cs" MasterPageFile="~/TemplateAdmin.master" EnableEventValidation="false"
Inherits="WillCallWeb.Admin.HeaderImageEditor" Title="Admin - Edit HeaderImage" %>
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