<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Customer_ChangePass.ascx.cs" Inherits="WillCallWeb.Controls.Customer_ChangePass" %>
<div id="changepass">
    <h4>Change your password</h4>
    <asp:ValidationSummary runat="server" ID="valChangePasswordSummary" ValidationGroup="ChangePassword1" ShowMessageBox="true" ShowSummary="false" />    
    <asp:ChangePassword ID="ChangePassword1" runat="server" OnChangingPassword="ChangePassword1_ChangingPassword" 
        OnSendingMail="ChangePassword1_SendingMail" OnSendMailError="ChangePassword1_SendMailError" OnChangedPassword="ChangePassword1_ChangedPassword">
        <ChangePasswordTemplate>         
            <table border="0" cellpadding="0" cellspacing="0" class="profile-table">
                <tr>
                    <th>Current password:<asp:RequiredFieldValidator ID="valRequireCurrentPassword" runat="server" ControlToValidate="CurrentPassword" SetFocusOnError="true" Display="Static"
                        ErrorMessage="Password is required." ToolTip="Password is required." ValidationGroup="ChangePassword1">*</asp:RequiredFieldValidator></th>
                    <td><asp:TextBox ID="CurrentPassword" TabIndex="1" TextMode="Password" runat="server" Width="250px"></asp:TextBox></td>          
                </tr>
                <tr>
                    <th>Security Question:
                    <asp:CompareValidator ID="reqReqQuestion" runat="server" ControlToValidate="Question" Operator="GreaterThan" 
                        ValidationGroup="ChangePassword1" ValueToCompare="0" ErrorMessage="Please select a security question." >*</asp:CompareValidator>
                    </th>            
                    <td>
                        <asp:DropDownList ID="Question" runat="server"  TabIndex="2" AppendDataBoundItems="true" Width="256px" ValidationGroup="ChangePassword1" >
                            <asp:ListItem Text="-- Select Security Question --" />
                        </asp:DropDownList>
                    </td>                     
                </tr>
                <tr>
                    <th>Security Answer:
                        <asp:RequiredFieldValidator ID="valRequireAnswer" CssClass="validator" runat="server" ControlToValidate="Answer" 
                            SetFocusOnError="true" Display="Static" ErrorMessage="Security answer is required." ToolTip="Security answer is required."
                            ValidationGroup="ChangePassword1">*</asp:RequiredFieldValidator>
                    </th>            
                    <td class="input"><asp:TextBox TabIndex="3" runat="server" ID="Answer" MaxLength="200" Width="250px" 
                        ValidationGroup="ChangePassword1" /></td>
                </tr>            
                <tr>
                    <th>New password:
                        <asp:RequiredFieldValidator ID="valRequireNewPassword" runat="server" ControlToValidate="NewPassword" SetFocusOnError="true" Display="Static"
                            ErrorMessage="New Password is required." ToolTip="New Password is required." ValidationGroup="ChangePassword1">*</asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="valPasswordLength" runat="server" ControlToValidate="NewPassword" SetFocusOnError="true" Display="Dynamic"
                            ValidationExpression="[\@\*\-_\w!]{5,20}" 
                            ErrorMessage="* Valid password chars are letters (A-Z, a-z), numbers (0-9), exclamation point (!), underscore(_), at symbol(@) and asterick (*). Passwords must be at least 5 chars and cannot exceed 20 chars." 
                            ToolTip="* Valid password chars are letters (A-Z, a-z), numbers (0-9), exclamation point (!), underscore(_), at symbol(@) and asterick (*). Passwords must be at least 5 chars and cannot exceed 20 chars."
                            ValidationGroup="ChangePassword1">*</asp:RegularExpressionValidator>
                    </th>
                    <td><asp:TextBox ID="NewPassword" TabIndex="4" TextMode="Password" runat="server" Width="250px"></asp:TextBox></td>            
                </tr>
                <tr>
                    <th>Confirm password:
                        <asp:RequiredFieldValidator ID="valRequireConfirmNewPassword" runat="server" ControlToValidate="ConfirmNewPassword" SetFocusOnError="true" Display="Static"
                            ErrorMessage="Confirm Password is required." ToolTip="Confirm Password is required."
                            ValidationGroup="ChangePassword1">*</asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="valComparePasswords" runat="server" ControlToCompare="NewPassword"
                            ControlToValidate="ConfirmNewPassword" SetFocusOnError="true" Display="Dynamic" ErrorMessage="The Confirm Password must match the New Password entry."
                            ValidationGroup="ChangePassword1">*</asp:CompareValidator>
                    </th>
                    <td><asp:TextBox ID="ConfirmNewPassword" TabIndex="5" TextMode="Password" runat="server" Width="250px"></asp:TextBox></td>     
                </tr>
            </table>         
            <span class="update">
                <asp:LinkButton ID="ChangePasswordPushButton" CssClass="btntribe" runat="server" CommandName="ChangePassword"
                    TabIndex="6" ValidationGroup="ChangePassword1" >Change Password</asp:LinkButton>
                <asp:Label ID="FailureText" runat="server" SkinID="FeedbackKO" EnableViewState="False" /> 
            </span>
        </ChangePasswordTemplate>
        <SuccessTemplate>
            <asp:Label runat="server" ID="lblSuccess" SkinID="FeedbackOK" Text="Your password has been changed successfully." />
        </SuccessTemplate>
        <MailDefinition BodyFileName=""  />
    </asp:ChangePassword>
</div>

