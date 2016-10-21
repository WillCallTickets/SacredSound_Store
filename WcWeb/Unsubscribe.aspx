<%@ Page Language="C#" MasterPageFile="~/Template.master" AutoEventWireup="true" CodeFile="Unsubscribe.aspx.cs" Inherits="WillCallWeb.Unsubscribe" Title="Unsubscribe Request" %>
<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" Runat="Server">   
    <div id="un-subscribe">
        <div class="legend">Unsubscribe</div>
        <asp:ValidationSummary runat="server" ID="valSummary" ValidationGroup="unsubscribe" HeaderText="Please correct the following errors:" 
            cssclass="validationsummary" />
    
    <table border="0" cellpadding="0" cellspacing="0" class="profile-table" width="500">
        <tr>
            <td colspan="2" style="font-weight:bold;padding-bottom:24px;" >Please submit your email address below to be 
                removed from our mailing list. Please note that it may take up to 72 hours for 
                you to be fully removed from our mailings.</th>
        </tr>
        <tr>
            <th>Email Address:
                <asp:RequiredFieldValidator ID="reqEmail" runat="server" ControlToValidate="txtEmail" SetFocusOnError="true" Display="Static" 
                    ErrorMessage="Email Address is required." ToolTip="Email Address is required." ValidationGroup="unsubscribe">*</asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator runat="server" ID="valEmailPattern" CssClass="validator" Display="Dynamic" 
                    SetFocusOnError="true" ControlToValidate="txtEmail" ErrorMessage="Please enter a valid e-mail address." 
                    ValidationGroup="unsubscribe" onload="valEmailPattern_Load">*</asp:RegularExpressionValidator>
            </th>
            <td style="width: 400px;"><asp:TextBox ID="txtEmail" runat="server" Width="250px" MaxLength="256"></asp:TextBox></td>
        </tr>
        <tr>
            <td colspan="2">
                <br /><br />
                <asp:LinkButton ID="btnUnsub" runat="server" CssClass="btntribe" ValidationGroup="unsubscribe" 
                    onclick="btnUnsub_Click" >Unsubscribe</asp:LinkButton>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Label runat="server" ID="lblFeedbackOK" SkinID="FeedbackOK" Text="Your email has been successfully removed from our mailings." Visible="false" />
                <asp:Label runat="server" ID="lblFeedbackKO" SkinID="FeedbackKO" Text="Your email is not listed in our records. Please contact customer support." Visible="false" />
            </td>
        </tr>
    </table>
   
   
   </div>
   
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="SideContent" Runat="Server">
    
</asp:Content>