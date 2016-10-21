<%@ Page Language="C#" MasterPageFile="~/Template.master" AutoEventWireup="true" CodeFile="ContactSuccess.aspx.cs" Inherits="WillCallWeb.Contact" Title="Contact Us" %>

<%@ Register Src="Controls/Menu_Item_Vertical.ascx" TagName="Menu_Item_Vertical"
    TagPrefix="uc1" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" Runat="Server">
<div id="contact">
    <div class="legend">Contact Us</div>
    <span class="success">Your message has been sent!</span>
</div>
 
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="SideContent" Runat="Server">
    <uc1:Menu_Item_Vertical ID="Menu_Item_Vertical1" runat="server" />

</asp:Content>