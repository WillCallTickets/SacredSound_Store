<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ShowEditor.aspx.cs" MasterPageFile="~/TemplateAdmin.master" EnableEventValidation="false" 
    Inherits="WillCallWeb.Admin.ShowEditor" Title="Admin - Edit Shows" %>
<%@ Register src="AdminControls/ShowChooser.ascx" tagname="ShowChooser" tagprefix="uc1" %>
<asp:Content ID="MainContent" runat="server" ContentPlaceHolderID="MainContent">
    <div id="editor">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc1:ShowChooser ID="ShowChooser1" runat="server" />
                <asp:Panel ID="Content" runat="server"></asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>