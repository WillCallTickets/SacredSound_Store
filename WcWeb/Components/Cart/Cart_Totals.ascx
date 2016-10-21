<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Cart_Totals.ascx.cs" Inherits="WillCallWeb.Components.Cart.Cart_Totals" %>
<%@ Register Src="~/Components/Cart/Cart_Function.ascx" TagName="Cart_Function" TagPrefix="uc3" %>
<%@ Register src="~/Components/Cart/Charity.ascx" tagname="Charity" tagprefix="uc2" %>
<%@ Register src="~/Components/Cart/Promotion_Listing.ascx" tagname="Promotion_Listing" tagprefix="uc4" %>
<div id="carttotals">
    <asp:CustomValidator ID="valCustom" runat="server" Display="Dynamic" CssClass="invisible" ErrorMessage="" ValidationGroup="cartcontrol">*</asp:CustomValidator>
    <asp:UpdatePanel ID="UpdatePanelCartTotals" runat="server" OnLoad="PanelTotals_Load" UpdateMode="Conditional">
        <ContentTemplate>
            
            <uc4:Promotion_Listing ID="Promotion_Listing1" runat="server" />

            <div class="carterror">
                <asp:Literal ID="litError" runat="server" /></div>
            
            <uc2:Charity ID="Charity1" runat="server" />

            <div class="carttotalcontainer cart-sub-division">                
                <table border="0" cellpadding="0" cellspacing="0" class="cart-totals">
                    <tr>
                        <th style="width:200px;">cart total:</th>
                        <td ><%=Ctx.Cart.PreFeeTotal.ToString("n")%></td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <th>processing:</th>
                        <td><%=Ctx.Cart.ProcessingCharge.ToString("n")%></td>
                        <td class="message"><%= (Ctx.Cart.ProcessingFee.IsOverride) ? Ctx.Cart.ProcessingFee.Description : "&nbsp;"%></td>
                    </tr>
                    <%if (Ctx.Cart.HasTicketItems_CurrentlyShippable || (Ctx.Cart.HasMerchandiseItems_Shippable && Wcss._Config._Shipping_Merch_Active))
                        {%>
                    <tr class="shipping">
                        <th>shipping:</th>
                        <td><%=Ctx.Cart.ShippingAndHandling.ToString("n")%></td>
                        <td class="message">
                            <%if (this.Page.ToString().ToLower() != "asp.store_shipping_aspx")
                                {%>
                            <%if (this.Page.ToString().ToLower() == "asp.store_checkout_aspx")
                                { %>*shipping charges will be added on next page<%}
                                else
                                { %>*shipping options available at checkout<%} %>
                            <%}
                                else
                                {%>&nbsp;<%} %>
                        </td>
                    </tr>
                    <%} %>
                    <%if (Wcss._Config._Coupons_Active && (this.Page.ToString().ToLower() == "asp.store_shipping_aspx" || this.Page.ToString().ToLower() == "asp.store_checkout_aspx"))
                        {%>
                    <tr class="coupon">
                        <th>coupon:</th>
                        <td><asp:TextBox ID="txtCoupon" runat="server" MaxLength="50" /></td>
                        <td class="message"><asp:LinkButton ID="btnCoupon" runat="server" CssClass="btntribe" OnClick="btnCoupon_Click">Apply Coupon</asp:LinkButton></td>
                    </tr>
                    <%} %>
                    <asp:Repeater ID="rptPromotion" runat="server" OnDataBinding="rptPromotion_DataBinding" OnItemDataBound="rptPromotion_ItemDataBound"
                        OnItemCommand="rptPromotion_ItemCommand">
                        <ItemTemplate>
                            <tr class="promotion">
                                <th><asp:Literal ID="litDesc" runat="server" /></th>
                                <td><asp:Literal ID="litAmount" runat="server" /></td>
                                <td class="message">
                                    <asp:Literal ID="litEligible" runat="server" />
                                    <asp:Literal ID="litCode" runat="server" />
                                    <span class="remover">
                                        <asp:LinkButton Width="20px" ID="btnDelete" BorderStyle="none" CssClass="removepromo-btn"
                                            runat="server" CommandName="Delete" CommandArgument='<%#Eval("tSalePromotionId") %>' CausesValidation="false"
                                            ToolTip="Remove Coupon" >
                                            <img src="/Images/Close.gif" border="0" alt="Remove Coupon" />
                                        </asp:LinkButton></span>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                    <%if (Ctx.Cart.CharityAmount > 0)
                        {%>
                    <tr class="donation">
                        <th>donation:</th>
                        <td><%=Ctx.Cart.CharityAmount.ToString("n2")%></td>
                        <td>&nbsp;</td>
                    </tr>
                    <%} %>
                    <tr>
                        <td colspan="3" class="totaldivider"><div class="item-separator">&nbsp;</div></td>
                    </tr>
                    <%if (Ctx.Cart.SubTotal > 0 && Wcss._Config._StoreCredit_Active && this.Profile.StoreCredit > 0)
                        { %>
                    <tr class="subtotal">
                        <th>subtotal:</th>
                        <td><%=Ctx.Cart.SubTotal.ToString("c")%></td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr class="credit">
                        <%if (Ctx.Cart.StoreCreditToApply == 0)
                            {%>
                        <th colspan="2" class="youhavecredit">
                            you have store credit!
                            <div class="creditdoesnotapply">Store credit cannot be applied to gift certificates</div>
                        </th>
                        <td style="vertical-align:top;" class="message"><asp:LinkButton ID="btnCreditApplyInitial" runat="server" CausesValidation="false" cssclass="youhavecredit"
                            OnClick="btnCreditApplyInitial_Click">Apply Credit</asp:LinkButton></td>
                        <%}
                            else //if (Ctx.Cart.StoreCreditToApply > 0)
                            {%>
                        <th>store credit:</th>
                        <td>(<%=Ctx.Cart.StoreCreditToApply.ToString("n2")%>)</td>
                        <td class="message">
                            <%if (Wcss._Config._StoreCredit_AllowAdjustment)
                                {%>
                            <asp:TextBox ID="txtCreditAmount" runat="server" Width="50px" OnDataBinding="txtCreditAmount_DataBind" MaxLength="10" />
                            <asp:CompareValidator Display="dynamic" CssClass="validation" ValidationGroup="cartcontrol" ID="CompareValidator6"
                                runat="server" ErrorMessage="Please enter a numeric quantity." ControlToValidate="txtCreditAmount" Operator="DataTypeCheck"
                                Type="Currency">*</asp:CompareValidator>
                            <asp:LinkButton ID="btnCreditApplyPost" runat="server" CssClass="btntribe" CausesValidation="true" OnClick="btnCreditApplyPost_Click">Apply Credit</asp:LinkButton>
                            &#42;
                            <%} %>
                            <asp:LinkButton ID="btnCreditRemove" runat="server" CausesValidation="false" OnClientClick="return confirm('Are you sure you want to remove the store credit from this order?')"
                                cssclass="youhavecredit" OnClick="btnCreditRemove_Click">Remove Credit</asp:LinkButton>
                        </td>
                        <%} %>
                    </tr>
                    <%} %>
                    <tr class="total">
                        <th>order total:</th>
                        <td><%=Ctx.Cart.ChargeTotal.ToString("c")%></td>
                        <td class="message">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <div class="carteditor">
                                <uc3:Cart_Function ID="Cart_Function2" runat="server" />
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
