<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserProfileMaster.ascx.cs" Inherits="WillCallWeb.Controls.UserProfileMaster"  %>
<%@ Register Src="UserProfile.ascx" TagName="UserProfile" TagPrefix="uc1" %>

<div id="useraccount">
    <div class="legend"><%=this.Page.User.Identity.Name %></div>
    <span class="userlinks">
        <asp:HyperLink ID="linkEditProfile" runat="server" NavigateUrl="" Text="edit profile" />
            <%if (Wcss._Config._AllowCustomerInitiatedNameChanges)
            {%>
            <a href="/WebUser/Default.aspx?p=changename">Change Email</a>
        <%} %>
        <%if (Wcss._Config._StoreCredit_Active)
            {%>
            <asp:HyperLink ID="linkCredit" runat="server" NavigateUrl="/WebUser/Default.aspx?p=credit" Text="store credit" />
        <%} %> 
        <asp:HyperLink ID="linkHistory" runat="server" NavigateUrl="/WebUser/Default.aspx" Text="sales history" />
        <asp:HyperLink ID="linkChangePass" runat="server" NavigateUrl="/WebUser/Default.aspx?p=change" Text="change password" />
    </span>
    
    <table border="0" cellspacing="3" cellpadding="0" width="100%">
        <tr>
            <td>
                <asp:ValidationSummary runat="server" ID="valChangePasswordSummary" ValidationGroup="results" />
                <asp:CustomValidator ID="CustomResult" runat="server" CssClass="invisible" ErrorMessage="CustomValidator" ValidationGroup="results" 
                    Display="Dynamic">&nbsp;</asp:CustomValidator>
            </td>
        </tr>
        <tr>
            <td><uc1:UserProfile ID="UserProfile1" runat="server" /></td>
        </tr>
    </table>
        
    <span class="update">
        <asp:LinkButton runat="server" ID="btnUpdate" CssClass="btntribe" ValidationGroup="editprofile" OnClick="btnUpdate_Click" >Update Profile</asp:LinkButton>
        <asp:Label runat="server" ID="lblFeedbackOK" forecolor="green" CssClass="success" Text="Profile updated successfully" Visible="false" />
    </span>
       
</div>