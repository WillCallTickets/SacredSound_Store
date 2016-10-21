<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Customer_Navigation.ascx.cs" Inherits="WillCallWeb.Controls.Customer_Navigation" %>
<div id="useraccount">
    <div class="legend"><%=this.Page.User.Identity.Name %></div>        
    <span class="userlinks">
        <asp:HyperLink ID="linkEditProfile" runat="server" NavigateUrl="/EditProfile.aspx" Text="edit profile" />
        <%if (Wcss._Config._AllowCustomerInitiatedNameChanges)
            {%>
                <asp:Hyperlink ID="lnkChangeName" runat="server" NavigateUrl="/WebUser/Default.aspx?p=changename" Text="Change Email" />
        <%} %>
        <%if (Wcss._Config._StoreCredit_Active)
            {%>
            <asp:HyperLink ID="linkCredit" runat="server" NavigateUrl="/WebUser/Default.aspx?p=credit" Text="store credit" />
        <%} %>
        <asp:HyperLink ID="linkHistory" runat="server" NavigateUrl="/WebUser/Default.aspx" Text="sales history" />
        <asp:HyperLink ID="linkChangePass" runat="server" NavigateUrl="/WebUser/Default.aspx?p=change" Text="change password" />
    </span>
    <asp:Panel ID="PanelContent" runat="server" />        
</div>