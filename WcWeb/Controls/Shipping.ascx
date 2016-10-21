<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Shipping.ascx.cs" Inherits="WillCallWeb.Controls.Shipping" %>
<%@ Register Src="Cart.ascx" TagName="Cart" TagPrefix="uc1" %>
<%@ Register src="UserSubscriptions.ascx" tagname="UserSubscriptions" tagprefix="uc2" %>
<%@ Register src="~/Components/Cart/Cart_Totals.ascx" tagname="Cart_Totals" tagprefix="uc1" %>
<div id="checkout">

    <%if (Wcss._Config._Message_ShippingPage != null && Wcss._Config._Message_ShippingPage.Trim().Length > 0)
      {%>
        <div class="pagemessage"><%= Wcss._Config._Message_ShippingPage.Trim()%></div>
    <%} %>
    
    <uc1:Cart ID="Cart1" runat="server" />
    
    <div id="billship">
        <div class="payinfo">
            <div class="title">Bill To: <a class="editaddress" href="/Store/Checkout.aspx">edit billing address</a></div>
            <table border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td><%=Ctx.SessionInvoice.InvoiceBillShip.BlLastName %>, <%=Ctx.SessionInvoice.InvoiceBillShip.BlFirstName %></td>                
                </tr>
                <tr>
                    <td><%=Ctx.SessionInvoice.InvoiceBillShip.BlAddress1 %></td>
                </tr>
                <%if(Ctx.SessionInvoice.InvoiceBillShip.BlAddress2.Trim().Length > 0){ %>
                <tr>
                    <td><%=Ctx.SessionInvoice.InvoiceBillShip.BlAddress2 %></td>
                </tr>
                <%} %>
                <tr>
                    <td><%=Ctx.SessionInvoice.InvoiceBillShip.BlCity %>, <%=Ctx.SessionInvoice.InvoiceBillShip.BlStateProvince %>, <%=Ctx.SessionInvoice.InvoiceBillShip.BlPostalCode %>, <%=Ctx.SessionInvoice.InvoiceBillShip.BlCountry %></td>
                </tr>
                <tr>
                    <td><%=Ctx.SessionInvoice.InvoiceBillShip.BlPhone %></td>
                </tr>
            </table>
        </div>
    
        <div id="shipinfo" runat="Server" class="shipinfo">
	        <div class="title">Ship To: <a class="editaddress" href="/Store/Checkout.aspx" >edit shipping address</a></div>
            <table border="0" cellpadding="0" cellspacing="0">
                <%if (Ctx.SessionInvoice.InvoiceBillShip.SameAsBilling){ %>    
                <tr>
                    <td><strong>same as billing address</strong></td>
                </tr>
                <%}else{ %>
                <tr>
                    <td><%=Ctx.SessionInvoice.InvoiceBillShip.FirstName %> <%=Ctx.SessionInvoice.InvoiceBillShip.LastName %></td>
                </tr>
                <tr>
                    <td><%=Ctx.SessionInvoice.InvoiceBillShip.Address1 %></td>
                </tr>
                <%if(Ctx.SessionInvoice.InvoiceBillShip.Address2.Trim().Length > 0){ %>
                <tr>
                    <td><%=Ctx.SessionInvoice.InvoiceBillShip.Address2 %></td>
                </tr>
                <%} %>
                <tr>
                    <td><%=Ctx.SessionInvoice.InvoiceBillShip.City %>, <%=Ctx.SessionInvoice.InvoiceBillShip.StateProvince %>, <%=Ctx.SessionInvoice.InvoiceBillShip.PostalCode %>, <%=Ctx.SessionInvoice.InvoiceBillShip.Country %></td>
                </tr>
                <tr>
                    <td><%=Ctx.SessionInvoice.InvoiceBillShip.Phone %></td>
                </tr>
                <tr>
                    <td class="giftmessage">
                        <div>Would you like to send a gift message?</div>
                        <div>Gift messages will appear on packing slips</div>
                        <div>(messages are limited to 250 characters)</div>
                        <asp:TextBox ID="txtMessage" runat="server" MaxLength="250" TextMode="MultiLine" Height="72px" Width="95%" CssClass="cart-giftmessage" Text='<%# Ctx.SessionInvoice.InvoiceBillShip.ShipMessage %>' />
                    </td>
                </tr>
                <%} %>
            </table>
        </div>
        <span class="clear"></span>        
    </div>
    <div class="clear"></div>    

    <%if (Wcss._Config._SubscriptionsActive)  {%>
    <div class="flow-plan">
        <uc2:UserSubscriptions ID="UserSubscriptions1" runat="server" />
    </div>
    <%} %>
    
    <asp:ValidationSummary ID="ValidationSummaryCheckout" HeaderText="Please correct the following errors:" CssClass="validationsummary" 
        ValidationGroup="Checkout" runat="server" />
    <asp:CustomValidator ID="CustomValidation" runat="server" Display="dynamic"
        ValidationGroup="Checkout">*</asp:CustomValidator>
    <div id="authorization" runat="server" class="auth">        
	    <div class="title">Authorization</div>
	    <div id="authmessage"><%=Wcss._Config._AuthMessage %></div>
	    <table cellspacing="0" cellpadding="0" border="0">            
		    <tr>
			    <td valign="top" style="padding-top:12px;">
			        <img id="ImgCaptcha" name="ImgCaptcha" alt="" class="captcha" src="~/Controls/JpegImage.aspx" runat="server" />
			    </td>
			    <td valign="top">Can't read the code?<br />
		            <asp:LinkButton id="btnCaptchaRefresh" CssClass="btntribe" CausesValidation="false" runat="server" Text="refresh" 
		                ValidationGroup="Checkout" onclick="ButtonRefreshCaptcha_Click"></asp:LinkButton>  
			    </td>
			  </tr>
			  <tr>
			      <td colspan="2">  
				        Enter the code shown above:<br />
				        <asp:textbox id="TextCaptcha" runat="server" MaxLength="25"></asp:textbox>
				        <asp:requiredfieldvalidator id="RequiredCaptcha" runat="server" ValidationGroup="Checkout" CssClass="validator" Display="Dynamic"
					        ControlToValidate="TextCaptcha" ErrorMessage="Please enter the code displayed.">*</asp:requiredfieldvalidator>
				        <asp:customvalidator id="CustomCaptcha" runat="server" CssClass="validator" ValidationGroup="Checkout" ErrorMessage="Your entry did not match the code."
					        OnServerValidate="ValidateCaptcha">*</asp:customvalidator>
					    <asp:customvalidator id="CustomAuth" runat="server" CssClass="validator">*</asp:customvalidator>
			      </td>
		      </tr>		
              <tr>
			    <td colspan="2">
			        <div id="terms">
			            <asp:checkbox id="CheckTerms" runat="server" />
			            Check this box to acknowledge that you agree to our <a class="termsofsale" href="javascript:doPagePopup(&#39;/<%=Wcss._Config._VirtualResourceDir %>/Html/TermsOfSale.html&#39;, 'false')" >terms of sale.</a>
			            <asp:customvalidator id="CustomTerms" runat="server" CssClass="validator" Display="Dynamic" ErrorMessage="Please acknowledge that you accept our terms and conditions."
					        OnServerValidate="ValidateTerms" ValidationGroup="Checkout" ClientValidationFunction="ValidateRequiredCheckBox">*</asp:customvalidator>
					</div>
			    </td>
		    </tr>      
		      <tr>
			      <td colspan="2" class="auth">
			        <asp:LinkButton id="ButtonAuth" CssClass="btntribe auth" runat="server" ValidationGroup="Checkout" 
			            onclick="ButtonAuth_Click" OnClientClick="return confirm('Have you reviewed your order?');">Purchase</asp:LinkButton>
                    <div id="checkout-continue" class="checkout-continue" >...proceeding...</div>
			    </td>
		    </tr>
	    </table>	    
    </div>
</div>            