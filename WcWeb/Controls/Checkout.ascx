<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Checkout.ascx.cs" Inherits="WillCallWeb.Controls.Checkout" %>
<%@ Register Src="Cart.ascx" TagName="Cart" TagPrefix="uc1" %>
<%@ Register src="UserSubscriptions.ascx" tagname="UserSubscriptions" tagprefix="uc2" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<div id="checkout">
    
    <%if (Wcss._Config._Message_CheckoutPage != null && Wcss._Config._Message_CheckoutPage.Trim().Length > 0)
      {%>
        <div class="pagemessage">
            <div class="title">Please note**</div>
            <div><%= Wcss._Config._Message_CheckoutPage.Trim()%></div>
        </div>
    <%} %>
    
    <asp:ValidationSummary ID="ValidationSummaryCheckout" CssClass="validationsummary" ValidationGroup="Checkout" runat="server" />
    <asp:CustomValidator ID="CustomValidation" runat="server" Display="dynamic" ValidationGroup="Checkout">*</asp:CustomValidator>
    
    <uc1:Cart ID="Cart1" runat="server" />

    <div id="billship">
        <div class="payinfo">   
            <div class="title">Payment information</div>
            <table cellpadding="0" cellspacing="0" border="0" class="payment">
                <%if(Ctx.Cart.HasTicketItems) {%>
                <tr>
                    <td colspan="3" style="font-size: 13px; font-weight: bold; color: Red;">
                        <%if (!Ctx.Cart.HasTicketItems_AllHiddenShipMethod)
                          {%>
                        <%if (Wcss._Config._Shipping_Tickets_Active && Ctx.Cart.HasTicketItems_CurrentlyShippable)
                          {%>
                            Will call tickets will be listed under the first and last name provided here:
                            <%}
                          else
                          { %>
                        All tickets will be held at will call and will be listed under the first and last name provided here:
                        <%} %>
                        <div style="font-size: 11px; font-weight: normal; color: Black;">Will call name changes can be amended by contacting customer service after purchase and <b>up to 3 days</b> prior to the event.</div>
                        <%} %>
                        <br />
                    </td>
                </tr>
                <%} %>
                <tr>
                    <td>First Name:</td>
                    <td colspan="2">
                        <asp:textbox id="TextFirst" runat="server" MaxLength="50"></asp:textbox>
                    </td>
                </tr>
                <tr>
                    <td>Last Name:</td>
                    <td colspan="2"><asp:textbox id="TextLast" runat="server" MaxLength="50"></asp:textbox></td>
                </tr>
			    <tr>
                    <td>Address:</td>
                    <td colspan="2"><asp:textbox id="TextAddress" runat="server" MaxLength="60"></asp:textbox></td>
                </tr>
			    <tr>
                    <td>Address2:</td>
                    <td colspan="2"><asp:textbox id="TextAddress2" runat="server" MaxLength="60"></asp:textbox></td>
                </tr>
                <tr>
                    <td>City:</td>
                    <td colspan="2"><asp:textbox id="TextCity" runat="server" MaxLength="40"></asp:textbox></td>
                </tr>
			    <tr>
                    <td>State/Province:</td>
                    <td><asp:textbox id="TextState" runat="server" MaxLength="2" Width="25px"></asp:textbox></td>
				    <td class="instructions">(2 characters only please)</td>
			    </tr>
			    <tr>
                    <td>Postal Code:</td>
                    <td colspan="2"><asp:textbox id="TextZip" runat="server" MaxLength="20"></asp:textbox></td>
                </tr>
			    <tr>
                    <td>Country:</td>
                    <td colspan="2">
			            <asp:DropDownList ID="ddlCountry" runat="server" CssClass="countrylist" OnDataBinding="ddlCountry_DataBinding" OnDataBound="ddlCountry_DataBound" />
				    </td>
			    </tr>
			    <tr>
                    <td>Phone</td>
                    <td colspan="2"><asp:textbox id="TextPhone" runat="server" MaxLength="25"></asp:textbox></td>
                </tr>
            </table>
			
			<asp:UpdatePanel ID="UpdatePanelCC" runat="server" UpdateMode="Always">
                <ContentTemplate>
                    <%if(Ctx.Cart.ChargeTotal == 0 && _CreditCardRequired) { %>
                    <div class="whycreditcard">
                        <div class="title">Why do you still need my credit card number?</div>
                        <div>Because you have items that may require shipping, we need to ask for a payment method. If your final total is $0.00, we certainly will not charge your credit card.</div>
                    </div>
                    <%}%>
                    
                    <%if(_CreditCardRequired) { %>
		            <table cellpadding="0" cellspacing="0" border="0" class="payment">		             
			            <tr>
                            <th>Card Number:
			                    <asp:RegularExpressionValidator ID="regexCC" ControlToValidate="TextNumber" runat="server" CssClass="validator"
			                        ValidationGroup="Checkout" Display="Static" ErrorMessage="Please enter a valid credit card number.">*</asp:RegularExpressionValidator>
			                </th>
			                <td><asp:textbox id="TextNumber" runat="server" MaxLength="25"></asp:textbox></td>
                        </tr>
			            <tr>
                            <th>Expiry:</th>
				            <td class="expiry">Month:
					            <asp:dropdownlist id="ddlMonth" runat="server" cssclass="month"></asp:dropdownlist>
					            &nbsp;Year:
					            <asp:dropdownlist id="ddlYear" runat="server" cssclass="year"></asp:dropdownlist>
				            </td>
			            </tr>
			            <tr>
                            <th>Name On Card:</th>
                            <td><asp:textbox id="TextCardName" runat="server" MaxLength="50"></asp:textbox></td>
                        </tr>
			            <tr>
                            <th>Security Code:</th>
                            <td><asp:textbox id="TextCode" runat="server" MaxLength="5"></asp:textbox></td>
                        </tr>		            
		            </table>
		            <%} %>		            
		        </ContentTemplate>
		    </asp:UpdatePanel>
        </div>

	    <div id="shipinfo" runat="Server" class="shipinfo">
		    <div class="title">Shipping Information</div>
		    <table cellspacing="0" cellpadding="0" border="0">
		    
		        <tr>
                    <td colspan="99" style="font-size: 13px; font-weight: bold; color: Red;">
		            <% if ((Ctx.Cart.HasTicketItems_CurrentlyShippable && (!Wcss._Config._Shipping_AllowTicketsToPoBox))
                        || 
                        (Ctx.Cart.HasMerchandiseItems_Shippable))
                    {%>
                        <div>Items cannot be shipped to PO Boxes.</div>  
                    <%} %>
                                 
		            <% if ((Ctx.Cart.HasTicketItems_CurrentlyShippable) && Wcss._Config._Shipping_Tickets_USA_Only) { %>
                        <div>Tickets may only be shipped in the mainland USA.</div>
		            <%} %>

                    </td>
                </tr>		    
			    <tr>
                    <td class="sameasbilling" colspan="3"><asp:checkbox id="CheckUseBilling" runat="server" Text="Same as billing information" Checked="True"></asp:checkbox>
				        <br /><br />
                    </td>
                </tr>
			    <tr><td>First Name:</td><td colspan="2"><asp:textbox id="TextShipFirst" runat="server" MaxLength="50"></asp:textbox></td></tr>
			    <tr><td>Last Name:</td><td colspan="2"><asp:textbox id="TextShipLast" runat="server" MaxLength="50"></asp:textbox></td></tr>
			    <tr><td>Address:</td><td colspan="2"><asp:textbox id="TextShipAddress" runat="server" MaxLength="60"></asp:textbox></td></tr>
			    <tr><td>Address2:</td><td colspan="2"><asp:textbox id="TextShipAddress2" runat="server" MaxLength="60"></asp:textbox></tr>
			    <tr><td>City:</td><td colspan="2"><asp:textbox id="TextShipCity" runat="server" MaxLength="40"></asp:textbox></td></tr>
			    <tr><td>State/Province:</td><td><asp:textbox id="TextShipState" runat="server" MaxLength="2" Width="25px"></asp:textbox></td>
				    <td class="instructions">(2 characters only please)</td>
			    </tr>
			    <tr><td>Postal Code:</td><td colspan="2"><asp:textbox id="TextShipZip" runat="server" MaxLength="20"></asp:textbox></td></tr>
			    <tr><td>Country:</td><td colspan="2"><asp:DropDownList ID="ddlShipCountry" CssClass="countrylist" runat="server" OnDataBinding="ddlCountry_DataBinding" OnDataBound="ddlCountry_DataBound" />
				    </td></tr>
			    <tr><td>Phone</td><td colspan="2"><asp:textbox id="TextShipPhone" runat="server" MaxLength="25"></asp:textbox></td></tr>
		    </table>
	    </div>
    </div>
    <div class="clear"></div>

    <%if ((!_displayShippingSection) && Wcss._Config._SubscriptionsActive)
      {%>
    <div class="flow-plan">
        <uc2:UserSubscriptions ID="UserSubscriptions1" runat="server" />
    </div>
    <%} %>
    
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
					        <asp:customvalidator id="CustomCaptcha" runat="server" CssClass="validator" ValidationGroup="Checkout" ErrorMessage="Your entry did not match the authorization code."
						        OnServerValidate="ValidateCaptcha">*</asp:customvalidator>
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
				      <td class="auth" colspan="2">
				        <asp:LinkButton id="ButtonAuth" CssClass="btntribe auth" 
                            OnClientClick="return confirm('Have you reviewed your order?');" 
                            runat="server" ValidationGroup="Checkout" onclick="ButtonAuth_Click" >Purchase</asp:LinkButton>
                        <div id="checkout-continue1"  class="checkout-continue">...proceeding...</div>
				    </td>
			    </tr>
		    </table>
	    </div>
    <div id="continueshipping" runat="server" class="continueship">
            <asp:LinkButton id="ButtonShipping" CssClass="btntribe" runat="server"
                ValidationGroup="Checkout" onclick="ButtonAuth_Click">Continue With Order</asp:LinkButton>
            <div id="checkout-continue2" class="checkout-continue" >...proceeding...</div>
    </div>
</div>