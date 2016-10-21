<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Cart_Purchase.ascx.cs" Inherits="WillCallWeb.Controls.Cart_Purchase" %>

<div id="maincart" class="purchase">

    <div id="donationPanel" runat="server" class="donation cart-division" enableviewstate="false">
    	<div class="cart-sub-division">
	        <div class="title">&laquo; Donations &raquo;</div>
	        <div class="panel-wrapper">
	            <asp:Repeater ID="rptDonation" runat="server" OnItemDataBound="rptDonation_ItemDataBound" enableviewstate="false">
		            <ItemTemplate>
		                <div class="item-container">
			                <asp:Literal ID="LiteralItemStatus" runat="server" enableviewstate="false" />
			                <div class="productname"><%#Eval("LineItemTotal","{0:c}") %> donation to <%# Eval("MainActName")%></div>
			                <div class="thankyou">
			                    Thank you for your support.
			                </div>
		                </div>
		            </ItemTemplate>
		            <SeparatorTemplate><div class="item-separator">&nbsp;</div></SeparatorTemplate>
	            </asp:Repeater>
	        </div>
        </div>
    </div>
	    
    <div id="tktPanel" runat="server" class="tickets cart-division" enableviewstate="false">
        <div id="tickets" class="cart-sub-division">
            <div class="title">&laquo; <%=System.Web.HttpUtility.HtmlEncode(Wcss._Config._CartTitle_Tickets)%> &raquo;</div>
            <div class="panel-wrapper">
                <asp:Repeater ID="rptTicketItems" runat="server" EnableViewState="False" OnItemDataBound="rptItem_Bind">                    
                    <ItemTemplate>
                        <div id="divPurchaseDesc" class="item-container" runat="server" enableviewstate="false">
                            <asp:Literal ID="litItemStatus" runat="server" EnableViewState="false" />
                            <asp:Literal ID="litItemDetail" runat="server" EnableViewState="false" />
                            <div class="shownames">
                                <asp:Literal ID="litShowNames" runat="server" EnableViewState="false" />
                            </div>
                            <asp:Literal ID="litPickupPost" runat="server" EnableViewState="false" />
                            <div ID="ItemPriceInfo" class="pricepanel" runat="server" enableviewstate="false" />
                            <div ID="ItemBundles" runat="server" class="bundle-container" enableviewstate="false" />
                        </div>
                    </ItemTemplate>
                    <SeparatorTemplate><div class="item-separator">&nbsp;</div></SeparatorTemplate>                    
                </asp:Repeater>
            </div>
        </div>                  
        
        <%if (_invoice.HasTicketShipmentItemsOtherThanWillCall)
          {%>        
        <div class="shippanel shiptickets cart-sub-division">
            <div class="title">&laquo; Ticket Shipping &raquo;</div>
            <div class="panel-wrapper">
                <asp:Repeater ID="rptTicketShipments" runat="server" OnItemDataBound="Shipment_ItemDataBound" EnableViewState="false">
                    <ItemTemplate>
                        <div id="divPurchaseDesc" runat="server" class="item-container" enableviewstate="false">
                            <asp:Literal ID="LiteralItemStatus" runat="server" EnableViewState="false" />                        
                            <div class="productname"><%# Eval("MainActName")%></div>
                            <div class="shipmentcontents">
                                <div class="shipcontentsheader">Items in this shipment:</div>
                                <asp:Repeater ID="rptContents" runat="server" enableviewstate="false">
                                    <ItemTemplate>
                                        <div class="shipcontentitem"><%# Eval("Quantity")%> @ <%# Eval("DtDateOfShow")%> <%# Eval("MainActName")%> <%# Eval("Description")%> <%# Eval("Criteria")%></div>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                            <div id="ItemPriceInfo" runat="server" class="pricepanel" enableviewstate="false">
                                <span class="itemtotal">ship cost<span class="money"><%# Eval("LineItemTotal", "{0:c}")%></span></span>                            
                            </div>
                        </div>
                    </ItemTemplate>
                    <SeparatorTemplate><div class="item-separator">&nbsp;</div></SeparatorTemplate>
                </asp:Repeater>
                <%if (Wcss._Config._Shipping_Tickets_ShipExplanation != null && Wcss._Config._Shipping_Tickets_ShipExplanation.Trim().Length > 0){%>
                <div class="explanation cart-info"><%= Wcss._Config._Shipping_Tickets_ShipExplanation.Trim()%></div>
                <%} %>
                <%if (Wcss._Config._ShippingTerms_Tickets.Trim().Length > 0) {%>
                <div class="shipterms cart-info">
                    <%=Wcss._Config._ShippingTerms_Tickets%>
                </div>
                <%} %>
            </div>
        </div>
        <%}%>
    </div>
      
    <div id="mrcPanel" runat="server" class="merch cart-division" enableviewstate="false">
        <div class="cart-sub-division">
            <div class="title">&laquo; <%=System.Web.HttpUtility.HtmlEncode(Wcss._Config._CartTitle_Merch)%> &raquo;</div>
            <div class="panel-wrapper">
                <asp:Repeater ID="rptMerchItems" runat="server" EnableViewState="False" OnItemDataBound="rptItem_Bind">
                    <ItemTemplate>
                        <div id="divPurchaseDesc" class="item-container" runat="server" enableviewstate="false">
                            <asp:Literal ID="litItemStatus" runat="server" EnableViewState="false" />
                            <div class="productname"><%# Eval("Quantity")%> @ <%# Eval("MainActName")%></div>
                            <asp:Literal ID="litItemDetail" runat="server" EnableViewState="false" />
                            <div id="ItemPriceInfo" runat="server" class="pricepanel" enableviewstate="false" />
                            <div ID="ItemBundles" runat="server" class="bundle-container" enableviewstate="false" />
                        </div>
                    </ItemTemplate>
                    <SeparatorTemplate><div class="item-separator">&nbsp;</div></SeparatorTemplate>
                </asp:Repeater>

                <%if (_hasGiftCertToRedeem || _hasCreditItem)
                  {%>
                <div class="giftterms cart-info"><%=Wcss._Config._GiftTerms%></div>
                
                <%if (_hasGiftCertToRedeem)
                  {%>
                <div class="giftterms cart-info"><%=Wcss._Config._GiftDelivery%></div>
                <%} %>
                <%if (_hasCreditItem)
                  {%>
                <div class="giftterms cart-info"><%=Wcss._Config._GiftRedemptionInstructions%></div>
                <%} %>

                <%} %>
            </div>
        </div>
     
        <%if (_invoice.HasMerchandiseShipmentItems){%>
        <div class="shippanel shipmerch cart-sub-division">            
            <div class="title">&laquo; Merchandise Shipping &raquo;</div>
            <div class="panel-wrapper">
                <asp:Repeater ID="rptMerchShipments" runat="server" OnItemDataBound="Shipment_ItemDataBound" enableviewstate="false">
                    <ItemTemplate>
                        <div id="divPurchaseDesc" runat="server" class="item-container" enableviewstate="false">
                            <asp:Literal ID="LiteralItemStatus" runat="server" EnableViewState="false" />
                            <div class="productname"><%# Eval("MainActName")%> <asp:Literal ID="litShipDate" runat="server" EnableViewState="false" /></div>
                            <div class="shipmentcontents">
                                <div class="shipcontentsheader">Items in this shipment:</div>
                                <asp:Repeater ID="rptContents" runat="server" enableviewstate="false" OnItemDataBound="rptContents_ItemDataBound">
                                    <ItemTemplate>
                                        <div class="shipcontentitem"><asp:Literal ID="litBundleItem" runat="server" EnableViewState="false" /><%# Eval("Quantity")%> @ <%# Eval("MainActName")%></div>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>                            
                            <div id="ItemPriceInfo" runat="server" class="pricepanel" enableviewstate="false">
                                <span class="itemtotal">ship cost<span class="money"><%# Eval("LineItemTotal", "{0:c}")%></span></span>                            
                            </div>
                        </div>
                    </ItemTemplate>
                    <SeparatorTemplate><div class="item-separator">&nbsp;</div></SeparatorTemplate>
                </asp:Repeater>
            </div>
        </div>
        <%} %>                
    </div>
   
    <div id="promoPanel" runat="server" class="promotion cart-division" enableviewstate="false">
        <div class="title">&laquo; <%=System.Web.HttpUtility.HtmlEncode(Wcss._Config._CartTitle_Promotion)%> &raquo;</div>
        <div class="panel-wrapper">
            <asp:Repeater ID="rptPromo" runat="server" OnItemDataBound="rptPromo_ItemDataBound" EnableViewState="false">
                <ItemTemplate>
                    <div class="item-container">
                        <div class="productname"><%# Eval("MainActName")%></div>
                        <asp:Literal ID="litPromoDisplay" runat="server" EnableViewState="false" />
                        <asp:Panel ID="pnlPricing" runat="server" CssClass="pricepanel" EnableViewState="false">
                            <span class="label">item total: </span>
                            <span class="money"><Asp:Literal ID="litPrice" runat="server" EnableViewState="false"/></span>
                        </asp:Panel>
                    </div>
                </ItemTemplate>
                <SeparatorTemplate><div class="item-separator">&nbsp;</div></SeparatorTemplate>
            </asp:Repeater>
        </div>
    </div>
          
    <div class="cart-division">
        <div class="carttotalcontainer cart-division">
            <table border="0" cellpadding="0" cellspacing="0" class="cart-totals">
                <%if (_invoice.OriginalCartTotal > 0) {%>
                <tr>
                    <th>Cart total:</th>
                    <td><%=_invoice.OriginalCartTotal.ToString("n2")%></td>
                </tr>             
                <%} %>
                <%if (_invoice.OriginalProcessingFee > 0)
                  {%>
                <tr>
                    <th>Processing:</th>
                    <td><%=_invoice.OriginalProcessingFee.ToString()%></td>
                </tr>
                <%} %>
                <%if (_invoice.ShippingAndHandling > 0) {%>
                <tr class="shipping">
                    <th>Shipping:</th>
                    <td><%= _invoice.ShippingAndHandling.ToString()%></td>
                </tr>
                <%} %>        
                <%if (_invoice.PromotionalDiscountTotal > 0) {%>
                <tr class="discounts">
                    <th>Discounts:</th>
                    <td>(<%=_invoice.PromotionalDiscountTotal.ToString("n")%>)</td>
                </tr>
                <%} %>            
                <tr>
                    <td colspan="2" class="totaldivider"><div class="item-separator">&nbsp;</div></td>
                </tr>
                <%if (_invoice.StoreCreditTotal > 0)
                  {%>
                <tr class="total">
                    <th>Order Total:</th>
                    <td><%=_invoice.TotalPaid.ToString("c")%></td>
                </tr>
                <tr class="credit">
                    <th>Store Credit:</th>
                    <td>(<%=_invoice.StoreCreditTotal.ToString("n2")%>)</td>
                </tr>
                <tr>
                    <td colspan="2" class="totaldivider"><div class="item-separator">&nbsp;</div></td>
                </tr>
                <%} %>
                <tr class="paid">
                    <th>Total Paid:</th>
                    <td><%=_invoice.TotalPaidAfterCredit.ToString("c")%></td>
                </tr>
            </table>
        </div>
    </div>

    <%if (_invoice.HasTicketItems && Wcss._Config._TicketPurchaseInstructions != null && Wcss._Config._TicketPurchaseInstructions.Trim().Length > 0)
      { %>
    <div class="tixpurchaseinstruct cart-division">
        <div class="title">Ticket Instructions</div>
        <div class="cart-sub-division">
            <%=Wcss._Config._TicketPurchaseInstructions.ToString().Trim()%>
        </div>
    </div>
    <%} %>

    <%if (_invoice.HasDownloadDeliveryItems)
        {%>
        <div class="tixpurchaseinstruct cart-division">
            <div class="cart-sub-division">
                <div class="giftterms cart-info"><%=Wcss._Config._DownloadInstructions_1320%></div>
            </div>
        </div>
    <%} %>

    <%if (_invoice.InvoiceBillShipRecords().Count > 0 && _invoice.InvoiceBillShipRecords()[0].ShipMessage_Working.Trim().Length > 0)
      { %>
    <div class="invoicegiftmessage cart-division">
        <div class="title">Gift Message</div>
        <div class="cart-sub-division">
            <%=_invoice.InvoiceBillShipRecords()[0].ShipMessage_Working.Trim()%>
        </div>
    </div>
    <%} %>
    
    <div id="RefundInfo" class="refundinfo cart-division" runat="server" EnableViewState="false">
        <div class="cart-sub-division">
            <div class="title">Refund Information</div>
            <div class="item-wrapper">
                <div class="item-container">
                    <asp:GridView ID="GridRefunds" Width="100%" runat="server" DataKeyNames="Id" AutoGenerateColumns="False" EnableViewState="false" 
                        OnDataBinding="GridRefunds_DataBinding" OnRowDataBound="GridRefunds_RowDataBound" CssClass="crewtable"  >
                        <HeaderStyle CssClass="selected" />
                        <Columns>
                            <asp:TemplateField HeaderText="Description" HeaderStyle-HorizontalAlign="left">
                                <ItemTemplate>
                                    <asp:Literal ID="litDescription" runat="server" EnableViewState="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Price" HeaderText="Base" HtmlEncode="false" DataFormatString="{0:n}"
                                ItemStyle-HorizontalAlign="center" />
                            <asp:BoundField DataField="ServiceCharge" HeaderText="Svc" HtmlEncode="false" DataFormatString="{0:n}"
                                ItemStyle-HorizontalAlign="center" />    
                            <asp:BoundField DataField="PricePerItem" HeaderText="Each" HtmlEncode="false" DataFormatString="{0:n}"
                                ItemStyle-HorizontalAlign="center" />
                            <asp:BoundField DataField="Quantity" HeaderText="Qty" HtmlEncode="false" ItemStyle-HorizontalAlign="center" />
                            <asp:BoundField DataField="LineItemTotal" HeaderText="Total" HtmlEncode="false" DataFormatString="{0:c}"
                                ItemStyle-HorizontalAlign="center" />
                        </Columns>
                    </asp:GridView>            
            
                    <div class="pricepanel">
                        <%if (_invoice.TotalRefunds > 0)
                          { %>
                          <p><span class="itemtotal"><span class="label">Refunds:</span><span class="money"><%=_invoice.TotalRefunds.ToString("n")%></span></span></p><%} %>
              
                        <p><span class="itemtotal"><span class="label">NetPaid:</span><span class="money"><%=_invoice.NetPaid.ToString("c")%></span></span></p>
                    </div>
                </div>
            </div>
        </div>
    </div>


   <%if (_hasGiftCertToRedeem) { %>


    <script type="text/javascript" src="/JQueryUI/jquery.tools-110511.all.min.js"></script>
    <script type="text/javascript" src="/JQueryUI/appleoverlay.js"></script>

    <div class="apple_overlay" id="overlay-bundle" style="display:none;">
        <div class="contentWrap"></div>
    </div>

  <%} %>
</div> 
<!-- 110525 - 330 -->