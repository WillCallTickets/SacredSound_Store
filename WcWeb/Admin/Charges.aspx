<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Charges.aspx.cs" Inherits="WillCallWeb.Admin.Charges" Title="Admin - Reports" 
 MasterPageFile="~/TemplateAdmin.master" %>
<asp:Content ID="MainContent" runat="server" ContentPlaceHolderID="MainContent">
    <div id="report">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Panel ID="Content" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>