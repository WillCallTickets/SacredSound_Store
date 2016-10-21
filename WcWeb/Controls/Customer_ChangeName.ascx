<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Customer_ChangeName.ascx.cs" Inherits="WillCallWeb.Controls.Customer_ChangeName" %>

<div id="changename">
    <h4>Change your email address</h4>
    <asp:ValidationSummary runat="server" ID="valChangePasswordSummary" ValidationGroup="ChangeUsername" ShowSummary="true" />
        
        <table border="0" cellspacing="0" cellpadding="0" class="profile-table">
            <tr>
                <th>Current password:<asp:RequiredFieldValidator ID="valRequireCurrentPassword" runat="server" ControlToValidate="CurrentPassword" SetFocusOnError="true" Display="Static"
                    ErrorMessage="Password is required." ToolTip="Password is required." ValidationGroup="ChangeUsername">*</asp:RequiredFieldValidator></th>
                <td><asp:TextBox ID="CurrentPassword" TabIndex="1" TextMode="Password" runat="server" Width="250px"></asp:TextBox></td>          
            </tr>            
            <tr>
                <th>New Email address:
                    <asp:RequiredFieldValidator ID="valRequireNewUsername" runat="server" ControlToValidate="NewUsername" SetFocusOnError="true" Display="Static"
                        ErrorMessage="New Email address is required." ToolTip="New Email address is required." ValidationGroup="ChangeUsername">*</asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="valUsername" runat="server" ControlToValidate="NewUsername" SetFocusOnError="true" Display="Dynamic"
                        ErrorMessage="Please enter a valid emailaddress for your new username." 
                        ToolTip="Please enter a valid emailaddress for your new username." 
                        ValidationGroup="ChangeUsername">*</asp:RegularExpressionValidator>
                </th>
                <td><asp:TextBox ID="NewUsername" TabIndex="2" runat="server" Width="250px"></asp:TextBox></td>
            </tr>
            <tr><td colspan="2"><br /></td></tr>
            <tr>
                <th>Security Question:
                    <asp:CompareValidator ID="reqReqQuestion" runat="server" ControlToValidate="Question" Operator="GreaterThan" 
                        ValidationGroup="ChangeUsername" ValueToCompare="0" ErrorMessage="Please select a security question." >*</asp:CompareValidator>
                </th>            
                <td>
                    <asp:DropDownList ID="Question" runat="server"  TabIndex="3" AppendDataBoundItems="true" Width="256px" ValidationGroup="ChangeUsername" >
                        <asp:ListItem Text="-- Select Security Question --" />
                    </asp:DropDownList>
                </td>                     
            </tr>
            <tr>
                <th>Security Answer:
                    <asp:RequiredFieldValidator ID="valRequireAnswer" CssClass="validator" runat="server" ControlToValidate="Answer" 
                        SetFocusOnError="true" Display="Static" ErrorMessage="Security answer is required." ToolTip="Security answer is required."
                        ValidationGroup="ChangeUsername">*</asp:RequiredFieldValidator>
                </th>            
                <td class="input"><asp:TextBox TabIndex="4" runat="server" ID="Answer" MaxLength="200" Width="250px" 
                    ValidationGroup="ChangeUsername" /></td>
            </tr>
        </table>
        <span class="update">
        <asp:LinkButton ID="btnChangeName" CssClass="btntribe" runat="server" 
            CommandName="ChangeName" TabIndex="5" 
            ValidationGroup="ChangeUsername" onclick="btnChangeName_Click" ><span>Change Email</span></asp:LinkButton>
    </span>
</div>