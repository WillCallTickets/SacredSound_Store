<%@ Page Language="C#" MasterPageFile="~/Template.master" AutoEventWireup="true" CodeFile="CharitableOrgs.aspx.cs" Inherits="WillCallWeb.CharitableOrgs" Title="Charitable Organizations" %>

<%@ Register Src="Controls/Menu_Item_Vertical.ascx" TagName="Menu_Item_Vertical" TagPrefix="uc2" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" Runat="Server">    
    <div class="charitableorg">
    <asp:Repeater ID="rptEnt" runat="server" OnItemDataBound="rptEnt_ItemDataBound" 
        ondatabinding="rptEnt_DataBinding" >
        <HeaderTemplate>
            <asp:Literal ID="litHeader" runat="server" />
        </HeaderTemplate>
        <ItemTemplate>
            <asp:Literal ID="litStart" runat="server" />
            <div>
                <asp:Literal ID="litName" runat="server" />
                <asp:Literal ID="litStartContainer" runat="server" />
                <asp:Literal ID="litShort" runat="server" />
                <asp:Literal ID="litWriteup" runat="server" />
                <asp:Literal ID="litEndContainer" runat="server" />
            </div>
            <asp:Literal ID="litEnd" runat="server" />
        </ItemTemplate>
    </asp:Repeater>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="SideContent" Runat="Server">
    <uc2:Menu_Item_Vertical id="Menu_Item_Vertical1" runat="server" EnableViewState="false">
    </uc2:Menu_Item_Vertical>
</asp:Content>