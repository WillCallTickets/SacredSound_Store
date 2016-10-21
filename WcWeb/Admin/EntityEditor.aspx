<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EntityEditor.aspx.cs" MasterPageFile="~/TemplateAdmin.master" EnableEventValidation="false" 
Inherits="WillCallWeb.Admin.EntityEditor" Title="Admin - Edit Entities" Async="true" %>
<asp:Content ID="MainContent" runat="server" ContentPlaceHolderID="MainContent">
    <div id="srceditor">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Panel ID="Content" runat="server">
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>