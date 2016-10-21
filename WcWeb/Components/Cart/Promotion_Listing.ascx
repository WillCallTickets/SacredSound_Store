<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Promotion_Listing.ascx.cs" Inherits="WillCallWeb.Components.Cart.Promotion_Listing" %>
<div id="promoavailable" runat="server" class="promotion cart-division">
    <div class="cart-sub-division">
        <div class="title">&laquo; <%=System.Web.HttpUtility.HtmlEncode(Wcss._Config._CartTitle_Promotion)%> &raquo;</div>
        <div class="panel-wrapper">
            <asp:Repeater ID="rptPromo" runat="server" OnDataBinding="rptPromo_DataBinding" OnItemDataBound="rptPromo_ItemDataBound">
                <ItemTemplate>
                    <div class="item-container">
                        <asp:UpdatePanel ID="updateMerchAttributes" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="promodisplaytext">
                                    <%# Eval("SalePromo.DisplayText")%>
                                    <asp:Literal ID="litAdditional" runat="server" />
                                    <asp:Literal ID="litCaveat" runat="server" />
                                </div>
                                <asp:Literal ID="litSelection" runat="server" />
                                <div class="promoselection">
                                    <asp:RadioButtonList ID="ddlAwards" Visible="false" runat="server" AutoPostBack="true" 
                                        OnSelectedIndexChanged="lstAwards_SelectedIndexChanged" RepeatLayout="Flow" />
                                    <div>
                                        <asp:CheckBoxList ID="chkAwardSelections" Visible="false" runat="server" AutoPostBack="true" 
                                            OnSelectedIndexChanged="lstAwards_SelectedIndexChanged" RepeatLayout="Flow" />
                                    </div>
                                </div>
                                <div id="promoQty" class="promo-qty" runat="server" visible="false">                                    
                                    <asp:DropDownList ID="ddlAwardQty" runat="server" AutoPostBack="true" 
                                        OnSelectedIndexChanged="ddlAwardQty_SelectedIndexChanged" /> 
                                    <asp:Literal ID="litAllowable" runat="server" EnableViewState="false" />
                                </div>
                                <asp:Literal ID="litExtra" runat="server" />
                                <asp:Panel ID="pnlPricing" runat="server">
                                    <div class="pricepanel">
                                        <span class="label">total: </span>
                                        <span class="money"><asp:Literal ID="litPrice" runat="server" EnableViewState="false" /></span>
                                    </div>
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </ItemTemplate>
                <SeparatorTemplate><div class="item-separator">&nbsp;</div></SeparatorTemplate>
            </asp:Repeater>
        </div>
    </div>
</div>