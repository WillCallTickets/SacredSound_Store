<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DownloadEditor.aspx.cs" MasterPageFile="~/TemplateAdmin.master" 
Inherits="WillCallWeb.Admin.DownloadEditor" Title="Admin - Edit Downloads" %>
<%@ Register src="AdminControls/Downloads/Menu_DownloadSelection.ascx" tagname="Menu_DownloadSelection" tagprefix="uc1" %>
<asp:Content ID="MainContent" runat="server" ContentPlaceHolderID="MainContent">
    <div id="downloadeditor">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc1:Menu_DownloadSelection ID="Menu_DownloadSelection1" runat="server" />
                <asp:Panel ID="Content" runat="server">
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>