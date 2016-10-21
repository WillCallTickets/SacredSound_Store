<%@ Control Language="C#" AutoEventWireup="true" EnableViewState="false" CodeFile="Cart_Small.ascx.cs" Inherits="WillCallWeb.Controls.Cart_Small" %>
<%@ Register Src="~/Components/Cart/Cart_Function.ascx" TagName="Cart_Function" TagPrefix="uc1" %>
<span class="cartsmall">
    <span class="carttitle"><%= Wcss._Config._CartTitle%></span>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" OnLoad="PanelLoad" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:HyperLink ID="lnkEdit" ToolTip="edit shopping cart" runat="server" 
                NavigateUrl="/Store/Cart_Edit.aspx"> (<%=Ctx.Cart.ItemCount %>) item<%if (Ctx.Cart.ItemCount != 1){%>s<%} %>   (<%=Ctx.Cart.ChargeTotal.ToString("c") %>)</asp:HyperLink>
            <uc1:Cart_Function ID="Cart_Function1" runat="server" UseDotSeparator="true" />
        </ContentTemplate>
    </asp:UpdatePanel>
</span>