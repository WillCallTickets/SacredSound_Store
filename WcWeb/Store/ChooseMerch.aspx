<%@ Page Language="C#" EnableEventValidation="false" MasterPageFile="~/Template.master" AutoEventWireup="true" CodeFile="ChooseMerch.aspx.cs" Inherits="Store_ChooseMerch" Title="Choose Merchandise" %>

<%@ Register Src="~/Controls/Listing_Merch.ascx" TagName="Listing_Merch" TagPrefix="uc4" %>
<%@ Register Src="~/Controls/MerchItemTemplate.ascx" TagName="MerchItemTemplate" TagPrefix="uc4" %>
<%@ Register Src="~/Controls/Menu_Item_Vertical.ascx" TagName="Menu_Item_Vertical" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <div id="choosemerch">
        <asp:Panel ID="Content" runat="server"></asp:Panel>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="SideContent" Runat="Server">
    <uc1:Menu_Item_Vertical id="Menu_Item_Vertical1" runat="server">
    </uc1:Menu_Item_Vertical>
</asp:Content>