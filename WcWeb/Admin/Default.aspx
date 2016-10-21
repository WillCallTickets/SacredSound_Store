<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" MasterPageFile="~/TemplateAdmin.master" 
Inherits="WillCallWeb.Admin._Default" Title="Admin" %>
<asp:Content ID="MainContent" runat="server" ContentPlaceHolderID="MainContent">
<div class="sectiontitle">Administration</div>
<p></p>
<%=this.Page.User.IsInRole("Administrator")%>
<p></p>
<a href="ShowEditor.aspx">Add/EditShows</a>
</asp:Content>