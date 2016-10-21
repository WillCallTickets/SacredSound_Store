<%@ Page Language="C#" MasterPageFile="~/Template.master" AutoEventWireup="true" CodeFile="MailerManage.aspx.cs" Inherits="WillCallWeb.MailerManage" Title="Manage Mail Subscriptions" %>
<%@ Register Src="Controls/Menu_Item_Vertical.ascx" TagName="Menu_Item_Vertical" TagPrefix="uc2" %>
<%@ Register src="Controls/Mailer_Signup.ascx" tagname="Mailer_Signup" tagprefix="uc1" %>
<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" Runat="Server">
    <uc1:Mailer_Signup ID="Mailer_Signup1" runat="server" />
</asp:Content>
<asp:content ID="Content3" ContentPlaceHolderID="SideContent" runat="server">
    <uc2:Menu_Item_Vertical id="Menu_Item_Vertical1" runat="server"></uc2:Menu_Item_Vertical>
</asp:content>