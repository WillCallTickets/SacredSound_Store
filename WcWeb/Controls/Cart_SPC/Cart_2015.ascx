<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Cart_2015.ascx.cs" Inherits="WillCallWeb.Controls.Cart_SPC.Cart_2015" %>
<%@ Register src="~/Components/Cart/ShippingOptions.ascx" tagname="ShippingOptions" tagprefix="uc2" %>
<%@ Register src="~/Components/Cart/Cart_Totals.ascx" tagname="Cart_Totals" tagprefix="uc1" %>

<script src="/Includes/CountTimer.js" type="text/javascript"></script>

<div id="maincart">

    <h1 style="background-color:#ff6a00;font-size:18px;padding:5px;margin-bottom:10px;">Cart 2015</h1>
    
    <div class="cart-messages">
        <%if (Ctx.Cart != null && !Ctx.Cart.HasItems)
          {%><div class="emptycart">Your Cart Is Empty!</div><%} %>
        <asp:ValidationSummary ID="ValidationSummary1" CssClass="validationsummary" HeaderText="" ValidationGroup="cartcontrol" runat="server" />
        <asp:CustomValidator ID="valCustom" runat="server" Display="Dynamic" CssClass="invisible" ErrorMessage="" ValidationGroup="cartcontrol" >*</asp:CustomValidator>
    </div>
    
    <div id="tktPanel" runat="server" class="cart-division">
        tickets &amp; shipping
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
