<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DisplayList.aspx.cs" MasterPageFile="~/TemplateBlank.master" 
    Inherits="WillCallWeb.Admin.DisplayList" Title="Admin - DisplayList" %>
<asp:Content ID="BlankContent" runat="server" ContentPlaceHolderID="BlankContent">
    <div id="displaylist">
        <asp:Literal ID="litList" runat="server" EnableViewState="false" OnDataBinding="litList_DataBinding" />
    </div>
</asp:Content>