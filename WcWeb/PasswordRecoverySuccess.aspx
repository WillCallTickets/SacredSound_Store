<%@ Page Language="C#" MasterPageFile="~/Template.master" AutoEventWireup="true" CodeFile="PasswordRecoverySuccess.aspx.cs" Inherits="WillCallWeb.PasswordRecoverySuccess" 
Title="Password Recovery" %>

<%@ Register Src="Controls/Menu_Item_Vertical.ascx" TagName="Menu_Item_Vertical" TagPrefix="uc2" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" Runat="Server">
    <div class="passwordrecoverysuccess">
        Your password has been emailed to you.
    </div>
</asp:Content>
<asp:content ID="Content3" ContentPlaceHolderID="SideContent" runat="server">
    <uc2:Menu_Item_Vertical id="Menu_Item_Vertical1" runat="server"></uc2:Menu_Item_Vertical>    
</asp:content>