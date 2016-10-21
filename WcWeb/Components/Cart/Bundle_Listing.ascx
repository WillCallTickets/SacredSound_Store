<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Bundle_Listing.ascx.cs" Inherits="WillCallWeb.Components.Cart.Bundle_Listing" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<div id="divbundle" class="bundle-offered" runat="server">
    <asp:Literal ID="litSimply" runat="server" EnableViewState="false" />    
    <div class="panel-wrapper">
        <asp:UpdatePanel ID="updateMerchAttributes" runat="server" UpdateMode="Conditional" OnLoad="updLoad" >
            <ContentTemplate>              
                <asp:Repeater ID="Repeater1" runat="server" OnItemDataBound="Repeater1_ItemDataBound">
                    <ItemTemplate>
                        <div class="item-container">                            
                            <asp:Literal ID="litTitle" runat="server" EnableViewState="false" />                            
                            <asp:Literal ID="litEdit" runat="server" EnableViewState="false" />
                            <asp:Literal ID="litBundleSelected" runat="server" EnableViewState="false" />
                            <asp:Literal ID="litBundleItems" runat="server" EnableViewState="false" />
                            <asp:Literal ID="litPricing" runat="server" EnableViewState="false" />
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </ContentTemplate>
         </asp:UpdatePanel>
    </div>
</div>
          


