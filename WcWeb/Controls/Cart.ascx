<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Cart.ascx.cs" Inherits="WillCallWeb.Controls.Cart" %>
<%@ Register src="~/Components/Cart/ShippingOptions.ascx" tagname="ShippingOptions" tagprefix="uc2" %>
<%@ Register src="~/Components/Cart/Cart_Totals.ascx" tagname="Cart_Totals" tagprefix="uc1" %>

<script src="/Includes/CountTimer.js" type="text/javascript"></script>
<div id="maincart">
    
    <div class="cart-messages">
        <%if (Ctx.Cart != null && !Ctx.Cart.HasItems)
          {%><div class="emptycart">Your Cart Is Empty!</div><%} %>
        <asp:ValidationSummary ID="ValidationSummary1" CssClass="validationsummary" HeaderText="" ValidationGroup="cartcontrol" runat="server" />
        <asp:CustomValidator ID="valCustom" runat="server" Display="Dynamic" CssClass="invisible" ErrorMessage="" ValidationGroup="cartcontrol" >*</asp:CustomValidator>
    </div>

    <div id="cartTable" runat="server" class="cart-division">
        <div id="cartlist" class="cart-sub-division">            
            <div class="title">&laquo; Your Cart &raquo;</div>
            <div class="panel-wrapper">
                <div class="item-container">                    
                    <asp:Repeater ID="rptItems" runat="server" EnableViewState="False" OnDataBinding="rptItems_DataBinding" OnItemDataBound="rptItems_ItemDataBound">
                        <HeaderTemplate>
                            <table border="0" cellpadding="0" cellspacing="0" class="crtlst" >
                                <tr>
                                    <th>Line Total</th>
                                    <th>Qty</th>
                                    <th>Each</th>                        
                                    <th class="descript">
                                        Description
                                        <a href="javascript: location.href='/Store/Cart_Edit.aspx'; " class="editbtn">Change Quantities or Delete</a>
                                    </th>
                                </tr>             
                        </HeaderTemplate>
                        <ItemTemplate>
                                <tr>
                                    <td><%# Eval("LineTotal", "{0:c}") %></td>
                                    <td><%# Eval("Quantity") %></td>
                                    <td><%# Eval("PriceEach") %></td>
                                    <td class="descript">
                                        <asp:Literal ID="litDescription" runat="server" EnableViewState="false" />
                                        <asp:literal ID="litTimer" runat="server" EnableViewState="false" />
                                    </td>
                                </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </div>
            </div>
        </div>
    </div>

    <div id="tktPanel" runat="server" class="cart-division">
        <div id="tickets" class="cart-sub-division">
            <div class="title">&laquo; <%=Wcss._Config._CartTitle_Tickets%> &raquo;</div>
            <div class="panel-wrapper panel-float">
                <asp:Repeater ID="rptTickets" runat="server" OnItemCommand="ProcessCommand" OnItemDataBound="ProcessBind">
                    <ItemTemplate>
                        <div class="item-container">
                            <div class="cartaction">
                                <%if (this.Page.ToString().ToLower() != "asp.store_shipping_aspx" && this.Page.ToString().ToLower() != "asp.store_checkout_aspx")
                                    { %>
                                    <div class="actionable">
                                        <asp:DropDownList ID="ddlQty" runat="server"></asp:DropDownList>
                                        <asp:CustomValidator ID="RowValidator" Display="Static" runat="server" Text="*" CssClass="validator" ValidationGroup="cartcontrol"></asp:CustomValidator>
                                        <asp:LinkButton ID="btnAdd" CssClass="btntribe" CommandName="updtkt" runat="server" ToolTip="Update Quantity" CausesValidation="False"
                                            CommandArgument='<%# Eval("tShowTicketId") %>'>Update Qty</asp:LinkButton>
                                        <div class="removal">select 0 to remove</div>
                                    </div>
                                <%}
                                    else
                                    { %>
                                    <div>
                                        <span class="itemqty">Qty: <%#Eval("Quantity")%></span>
                                        <a href="/Store/Cart_Edit.aspx" title="update quantity" class="btntribe">update qty</a>
                                    </div>                                
                                <%} %>                               
                                <div class="itemtotal">total<span class="money"><%# Eval("LineTotal", "{0:c}")%></span></div>
                            </div>
                            <div class="iteminfo">
                                <asp:Literal ID="LiteralPackage" runat="server" EnableViewState="false" />
                                <asp:Literal ID="LiteralDescription" runat="server" EnableViewState="false" />
                                <asp:Literal ID="LiteralStatus" runat="server" EnableViewState="false" />
                                <div class="shownames">
                                    <asp:Repeater ID="rptShowNames" runat="server" OnItemDataBound="ProcessShowNames" EnableViewState="false">
                                        <ItemTemplate>
                                            <div class="eventdate">
                                                <div class="datelist"><%# Eval("DateOfShow", "{0:ddd MMM dd yyyy}")%></div>
                                                <asp:Literal ID="LiteralTime" runat="server" />
                                            </div>
                                            <asp:Literal ID="litVenue" runat="server" EnableViewState="false" />
                                            <asp:Literal ID="LiteralEventInfo" runat="server" EnableViewState="false" />
                                        </ItemTemplate>
                                        <SeparatorTemplate><div class="item-separator">&nbsp;</div></SeparatorTemplate>
                                    </asp:Repeater>
                                    <asp:Literal ID="litCamping" runat="server" />
                                </div>                        
                                <div class="itemoptions">
                                    <asp:Literal ID="LiteralPickupOptions" runat="server" EnableViewState="false" />
                                    <asp:literal ID="litPostPurchaseText" runat="server" EnableViewState="false" />
                                    <asp:literal ID="LiteralPickup" runat="server" EnableViewState="false" />
                                    <asp:literal ID="LiteralTimer" runat="server" EnableViewState="false" />                                    
                                </div>
                                <div class="pricepanel">
                                    <span class="label">price each</span>
                                    <span class="money"><%# Eval("PerItemPrice", "{0:c}")%></span>
                                    <span class="labelsmall">(<%# Eval("Price", "{0:c}")%> + <%# Eval("ServiceFee", "{0:c}")%> service fee)</span>                                    
                                </div>
                            </div>
                            <asp:Panel ID="pnlBundle" runat="server" class="bundle-panel-wrapper" />
                        </div>
                    </ItemTemplate>
                    <SeparatorTemplate><div class="item-separator sepdark">&nbsp;</div></SeparatorTemplate>
                </asp:Repeater>    
            </div>
            <div class="clear"></div>
        </div>
       </div>
        <div id="ticketshipping" class="shippanel shiptickets cart-sub-division" runat="server">
            <div class="title">
                &laquo; Ticket Shipping Options <%if (Ctx.Cart.HasMerchandiseItems_Shippable) { %><span class="ship-doesnotapply">(applies to tickets only)</span> <%} %> &raquo; 
            </div>
            <div class="panel-wrapper">
                <div class="item-container">
                    <%if (Wcss._Config._Shipping_Tickets_ShipExplanation != null && Wcss._Config._Shipping_Tickets_ShipExplanation.Trim().Length > 0)
                        {%><div class="shipnotes cart-info"><%=Wcss._Config._Shipping_Tickets_ShipExplanation%></div><%} %>
                    <div class="shiprates">
                        <asp:Literal ID="litTicketShipping" runat="server" EnableViewState="false" />
                        <asp:RadioButtonList ID="rdoTixShipRates" runat="Server" OnDataBinding="rdoTixShipRates_DataBinding" AutoPostBack="True" 
                            OnDataBound="rdoTixShipRates_DataBound" OnSelectedIndexChanged="rdoTixShipRates_SelectedIndexChanged" />
                    </div>
                </div>
            </div>
        </div>
    
        
    <div class="cart-division">
        <div id="mrcPanel" runat="server" class="merch cart-sub-division">
            <div class="title">&laquo; <%=Wcss._Config._CartTitle_Merch%> &raquo;</div>
            <div class="panel-wrapper panel-float">
                <asp:Repeater ID="rptMerch" runat="server" OnItemCommand="ProcessCommand" OnItemDataBound="ProcessBind" >                
                    <ItemTemplate>
                        <div class="item-container">
                            <div class="cartaction">
                                <%if (this.Page.ToString().ToLower() != "asp.store_shipping_aspx" && this.Page.ToString().ToLower() != "asp.store_checkout_aspx")
                                    { %>
                                    <div class="actionable">
                                        <asp:DropDownList ID="ddlQty" runat="server"></asp:DropDownList>
                                        <asp:CustomValidator ID="RowValidator" Display="Static" runat="server" Text="*" CssClass="validator"></asp:CustomValidator>
                                        <asp:LinkButton ID="btnAdd" CssClass="btntribe" CommandName="updmrc" runat="server" ToolTip="Update Quantity" CausesValidation="False"
                                            CommandArgument='<%# Eval("tMerchId") %>' >Update Qty</asp:LinkButton>
                                        <div class="removal">select 0 to remove</div>
                                    </div>                                        
                                <%}
                                    else
                                    { %>
                                    <div>
                                        <span class="itemqty">Qty: <%#Eval("Quantity")%></span>
                                        <a href="/Store/Cart_Edit.aspx" title="update quantity" class="btntribe">update qty</a>
                                    </div>
                                <%} %>                                    
                                <div class="itemtotal">total<span class="money"><%# Eval("LineTotal", "{0:c}")%></span></div>
                            </div>
                            <div class="iteminfo">
                                <div class="productname"><%# Eval("MerchItem.DisplayName")%></div>
                                <div class="product-selection"><%# Eval("MerchItem.AttribChoice")%></div>
                                <asp:literal ID="LiteralTimer" runat="server" EnableViewState="false" />
                                <div class="pricepanel">
                                    <span class="label">price each: </span>
                                    <span class="money"><%# Eval("Price", "{0:c}")%></span>
                                </div>
                                <asp:Literal ID="litSpecialInstructions" runat="server" />                                
                            </div>
                            <asp:Panel ID="pnlBundle" runat="server" class="bundle-panel-wrapper" />
                        </div>                        
                    </ItemTemplate>
                    <SeparatorTemplate><div class="item-separator sepdark">&nbsp;</div></SeparatorTemplate>
                </asp:Repeater>
            </div>
            <div class="clear"></div>
        </div>

        <div id="shipping" class="shippanel shipmerch cart-sub-division" runat="server">
            <div class="title">
                &laquo; Merchandise Shipping Options <%if(Ctx.Cart.HasTicketItems){ %><span class="ship-doesnotapply">(does not apply to tickets)</span> <%} %> &raquo; 
                <asp:CustomValidator ID="CustomShipMerch" CssClass="validator" Display="static" runat="server" ValidationGroup="cartcontrol">*</asp:CustomValidator>
            </div>
            <div class="panel-wrapper">
                <div id="divMultiple" runat="server" class="multiple cart-info">
                    <asp:CheckBox ID="chkShipMultiple" runat="server" Checked='<%#Ctx.Cart.IsShipMultiple_Merch %>' AutoPostBack="true" OnCheckedChanged="chkShipMultiple_CheckChanged" />
                    <span>Ship Items Separately <span class="caveat">** items shipped separately will increase shipping costs **</span></span>
                </div>
                
                <%if (Ctx.Cart.HasMerchandiseItems_Shippable && (Wcss._Config._MerchShipNotes != null && Wcss._Config._MerchShipNotes.Trim().Length > 0))
                    {%><div class="shipnotes cart-info"><div class="merchnotes"><%=Wcss._Config._MerchShipNotes%></div></div><%} %>  
                <div class="itemlist-container">        
                    <uc2:ShippingOptions ID="ShippingOptions_Merch" ItemContext="merch" runat="server" OnShipRateChanged="ShippingOptions_Merch_ShipRateChanged" />                    
                </div>
            </div>
        </div>        
    </div>    

    <div class="cart-division">    
       <uc1:Cart_Totals ID="Cart_Totals1" runat="server" />
    </div>

</div>

<div class="apple_overlay" id="overlay-bundle" style="display:none;">
	<div class="contentWrap"></div>
</div>

<script type="text/javascript">
    var servertimestring = '<%= DateTime.Now %>';   
    displayItemCountdowns(servertimestring);
</script>
