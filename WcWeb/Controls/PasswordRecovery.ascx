<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PasswordRecovery.ascx.cs" Inherits="WillCallWeb.Controls.PasswordRecovery" %>

<div id="recoverpass" >
    <div class="legend">Recover your password</div>
        
    <%if (Wcss._Config._Message_CreateNewAccount != null && Wcss._Config._Message_CreateNewAccount.Trim().Length > 0)
        {%>
    <span class="createnewaccountmsg"><%=Wcss._Config._Message_CreateNewAccount%></span>
    <%} %>
        
    <div class="validationsection">
        <asp:ValidationSummary runat="server" ID="valSummary" ValidationGroup="PasswordRecovery1" HeaderText="Please correct the following errors:" ShowSummary="true" />
    </div>
        
    <asp:PasswordRecovery ID="PasswordRecovery1" runat="server" 
        OnSendingMail="PasswordRecovery1_SendingMail" OnSendMailError="PasswordRecovery1_SendMailError"
        OnVerifyingUser="PasswordRecovery1_VerifyUser" Width="100%" 
        onanswerlookuperror="PasswordRecovery1_AnswerLookupError" >
        <SuccessTextStyle Font-Bold="true" ForeColor="Green" />
        <UserNameTemplate>
            <div class="title">Step 1: enter your email address</div>
                <table border="0" cellspacing="0" cellpadding="0" class="profile-table">
                <tr>
                    <th>Email address<asp:RequiredFieldValidator ID="valRequireUserName" runat="server" ControlToValidate="UserName" SetFocusOnError="true" Display="Static"
                            ErrorMessage="Email address is required." ToolTip="Email address is required." ValidationGroup="PasswordRecovery1">*</asp:RequiredFieldValidator></th>
                    <td style="width: 300px;"><asp:TextBox ID="UserName" runat="server" Width="100%"></asp:TextBox></td>        
                </tr>
            </table>
            <span class="update">
                <br />
                <asp:LinkButton ID="SubmitButton" CssClass="btntribe" runat="server" CommandName="Submit" ValidationGroup="PasswordRecovery1">Submit</asp:LinkButton>
                <asp:Label ID="FailureText" runat="server" SkinID="FeedbackKO" EnableViewState="False" /> 
            </span>      
        </UserNameTemplate>
        <QuestionTemplate>
            <div class="title">Step 2: answer the following question</div>
            <table border="0" cellspacing="0" cellpadding="0" class="profile-table">
            <tr>
                <th>Email address</th>
                <td style="width: 300px;"><asp:Literal ID="UserName" runat="server"></asp:Literal></td>
                <td></td>            
            </tr>
            <tr>
                <th>Question:</th>
                <td><asp:Literal ID="Question" runat="server"></asp:Literal></td>
            </tr>
            <tr>
                <th>Answer:
                <asp:RequiredFieldValidator ID="valRequireAnswer" runat="server" ControlToValidate="Answer" SetFocusOnError="true" Display="Dynamic"
                        ErrorMessage="Answer is required." ToolTip="Answer is required." ValidationGroup="PasswordRecovery1">*</asp:RequiredFieldValidator></th>
                <td><asp:TextBox ID="Answer" runat="server" Width="100%"></asp:TextBox></td>
            </tr>
        </table>
        <div class="update">
            <br />
            <asp:LinkButton ID="SubmitButton" CssClass="btntribe" runat="server" CommandName="Submit" ValidationGroup="PasswordRecovery1" >Submit</asp:LinkButton>
            <asp:Label ID="FailureText" runat="server" SkinID="FeedbackKO" EnableViewState="False" /> 
        </div>
        </QuestionTemplate> 
        <MailDefinition BodyFileName="" />
    </asp:PasswordRecovery>

    <div id="blockerinstruct">
        Please be sure to add <span style="font-size:16px;text-decoration:underline;"><%=Wcss._Config._CustomerService_Email %></span> to your list of allowed senders.
    </div>
   
</div>