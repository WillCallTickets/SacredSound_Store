<%@ Control Language="C#" AutoEventWireup="true" EnableViewState="false" CodeFile="HeaderBar.ascx.cs" Inherits="WillCallWeb.Controls.HeaderBar" %>
<%@ Register Src="Cart_Small.ascx" TagName="Cart_Small" TagPrefix="uc2" %>
<%@ Register Src="/Controls/Login_Mini.ascx" TagName="Login_Mini" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<div id="headerbar">
    <uc2:Cart_Small ID="Cart_Small1" runat="server" />
    <uc1:Login_Mini ID="Login_Mini1" runat="server" />
</div>

   
   
   