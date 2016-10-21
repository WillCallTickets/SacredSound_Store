<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GiftCert_Email.aspx.cs" MasterPageFile="~/TemplatePrint.master" Inherits="WillCallWeb.Components.Store.GiftCert_Email" Title="Gift Certificate Email Confirmation" %>

<asp:Content ID="MainContent" runat="server" ContentPlaceHolderID="MainContent">
    <div class="giftcert">
       <table border="0" cellpadding="0" cellspacing="6" class="gift">
            <tr><td rowspan="99">&nbsp;&nbsp;</td><td colspan="2">&nbsp;</td></tr>
            <tr>
                <td><img src="/WillCallResources/Images/UI/<%=Wcss._Config._GiftLogo%>" /></td>
                <td><h1>Gift Certificate</h1></td>
            </tr>
            <tr><td colspan="2">&nbsp;</td></tr>
            <tr>
                <td style="color: Green; font-size: 16px;" colspan="2">The gift certificate has been sent to <%=Email %></td>
            </tr>
            <tr><td colspan="2">&nbsp;</td></tr>
            <tr><td colspan="2"><a class="btntribe" href="javascript: history.back();"><span>back</span></a></td></tr>
        </table>
        <br /><br />
    </div>
</asp:Content>

