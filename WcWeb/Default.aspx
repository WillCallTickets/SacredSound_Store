<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="WillCallWeb._Default" Title="Home" MasterPageFile="~/Template.master" %>

<%@ Register Src="Controls/Menu_Item_Vertical.ascx" TagName="Menu_Item_Vertical"
    TagPrefix="uc2" %>

<%@ MasterType VirtualPath="~/Template.master" %>


<asp:content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Panel ID="Content" runat="server">
        check all the cool stuff we have...<p></p>
        be sure to get your tickets for so and so...<p></p>
        shows just added...<p></p>
        merch just added...<p></p>
        other news...<p></p>    
    </asp:Panel>
</asp:content>


<asp:content ID="Content3" ContentPlaceHolderID="SideContent" runat="server">
    &nbsp;<uc2:Menu_Item_Vertical id="Menu_Item_Vertical1" runat="server"></uc2:Menu_Item_Vertical>
    
</asp:content>