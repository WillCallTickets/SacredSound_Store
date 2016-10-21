<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MerchEditor.aspx.cs" MasterPageFile="~/TemplateAdmin.master" ValidateRequest="false" 
Inherits="WillCallWeb.Admin.MerchEditor" Title="Admin - Edit Merch" %>
<%@ Register src="AdminControls/Menu_MerchSelection.ascx" tagname="Menu_MerchSelection" tagprefix="uc1" %>
<asp:Content ID="MainContent" runat="server" ContentPlaceHolderID="MainContent">
    <div id="mercheditor">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
               <uc1:Menu_MerchSelection ID="Menu_MerchSelection1" runat="server" />
                <asp:Panel ID="Content" runat="server">
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>