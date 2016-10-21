<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserSubscriptions.ascx.cs" Inherits="WillCallWeb.Controls.UserSubscriptions" %>
<div class="usersubscription">
    <div class="controlheader">
        <span class="title">Mail Subscriptions</span>
            <span class="links">
            <asp:LinkButton ID="btnUpdate" runat="server" CssClass="btntribe" Text="Update" onclick="btnUpdate_Click" />
            </span>
    </div>
    <ul>
        <li>Check to subscribe. Uncheck to unsubscribe.</li>
    </ul>
    <div class="feedback">
        <asp:Label runat="server" ID="lblFeedbackOK" SkinID="FeedbackOK" Text="Subscriptions updated successfully" Visible="false" />
    </div>
    <asp:CheckBoxList runat="server" RepeatLayout="table" ID="chkSubs" RepeatColumns="1" CellSpacing="4" 
        ondatabinding="chkSubs_DataBinding" ondatabound="chkSubs_DataBound" EnableViewState="true" />
</div>