<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Cart_Function.ascx.cs" Inherits="WillCallWeb.Components.Cart.Cart_Function" %>
<span class="cartfunction">
    <span class="cartlinks">
        <asp:LinkButton ID="linkClear" runat="server" CssClass="btntribe" OnClick="linkClear_Click" OnClientClick="return confirm('Are you sure you want to clear your cart?');">clear cart</asp:LinkButton>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" OnLoad="PanelLoad" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Panel ID="pnlMessage" runat="server" EnableViewState="false" Visible="false" />
                <asp:HyperLink ID="linkEdit" runat="server" CssClass="btntribe" NavigateUrl="/Store/Cart_Edit.aspx">edit cart</asp:HyperLink>
                <%if (this._useDotSeparator)
                  { %>
                <span id="MidDot" class="middot" runat="server">&middot;</span>
                <%}%>
                <asp:HyperLink ID="linkCheckout" runat="server" CssClass="btntribe" NavigateUrl="/Store/Checkout.aspx" >checkout</asp:HyperLink>                
                <asp:Literal ID="litRedeem" runat="server" EnableViewState="false" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </span>
</span>