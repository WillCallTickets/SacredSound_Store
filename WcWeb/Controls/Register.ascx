<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Register.ascx.cs" Inherits="WillCallWeb.Controls.Register" %>
<%@ Register Src="UserProfile.ascx" TagName="UserProfile" TagPrefix="wc" %>

<div id="register">

    <%if (CreateUserWizard1.ActiveStep.ID == "createuser"){%> 
    <asp:Panel id="existing" runat="server" class="accountpanel">

        <div class="legend">Existing Accounts</div>
        
        <asp:Login ID="Login1" runat="server" VisibleWhenLoggedIn="false" onloggedin="Login1_LoggedIn" 
            OnLoggingIn="Login1_LoggingIn" onPreRender="Login1_PreRender" onloginerror="Login1_LoginError">
            <LayoutTemplate>
                <table border="0" cellpadding="0" cellspacing="0" style="width: 450px;" class="profile-table">
                    <tr>
                        <th>Email Address:
                            <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" 
                                ControlToValidate="UserName" ErrorMessage="Email is required." 
                                ToolTip="Email is required." ValidationGroup="Login1">*</asp:RequiredFieldValidator>
                        </th>
                        <td>
                            <asp:TextBox ID="UserName" runat="server" TabIndex="1" MaxLength="256" Width="250px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>Password:
                            <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" 
                                ControlToValidate="Password" ErrorMessage="Password is required." 
                                ToolTip="Password is required." ValidationGroup="Login1">*</asp:RequiredFieldValidator>
                        </th>
                        <td>
                            <asp:TextBox ID="Password" runat="server" TabIndex="2" TextMode="Password" Width="250px"></asp:TextBox>
                                
                        </td>
                    </tr>
                    <tr>
                        <th align="center" colspan="2" style="color:Red;">
                            <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                        </th>
                    </tr>
                    <tr>
                        <th id="loginbtn">
                            <asp:LinkButton ID="Submit" runat="server" TabIndex="3" CommandName="Login" CssClass="btntribe" 
                                meta:resourcekey="SubmitResource1" >Login</asp:LinkButton>
                        </th>
                        <td style="text-align: right; padding-right: 100px; vertical-align: middle;">
                            <div style="margin-bottom: .5em;">
                                <a href="/PasswordRecovery.aspx">forgot password?</a>
                            </div>
                            <asp:HyperLink ID="linkManageEmail" runat="server" NavigateUrl="/MailerManage.aspx" Text="manage email?" />
                            <div style="margin-top: .5em;"><asp:LinkButton ID="linkLogout" runat="server" Text="logout"  CausesValidation="false" onclick="linkLogout_Click" /></div>
                        </td>
                    </tr>
                </table>
            </LayoutTemplate>
        </asp:Login>
    </asp:Panel>
<br /><br />
    
    <%} %>
<asp:Literal ID="updateProfile" runat="server" EnableViewState="False"></asp:Literal>
<%if (Wcss._Config._Message_CreateNewAccount != null && Wcss._Config._Message_CreateNewAccount.Trim().Length > 0)
    {%>
<div class="pagemessage"><%=Wcss._Config._Message_CreateNewAccount%></div>
<%} %>
        
    <div class="accountpanel" >
        <div class="legend">New Account Registration</div>
        <span class="update">
            <span class="accountlogin-error"><asp:Label ID="ErrorMessage" SkinID="FeedbackKO" runat="server" EnableViewState="False"></asp:Label></span>
        </span>
       <asp:CreateUserWizard runat="server" ID="CreateUserWizard1" AutoGeneratePassword="False" 
          ContinueDestinationPageUrl="~/Default.aspx" FinishDestinationPageUrl="~/Default.aspx" 
          OnFinishButtonClick="CreateUserWizard1_FinishButtonClick" 
          OnCreatingUser="CreateUserWizard1_CreatingUser"
          OnCreatedUser="CreateUserWizard1_CreatedUser" 
          DuplicateUserNameErrorMessage="The e-mail address that you entered is already in use. If you have an existing account, please login at the top of the page." 
            
          CreateUserButtonType="Link" CreateUserButtonStyle-CssClass="btntribe" 
          OnSendMailError="CreateUserWizard1_SendMailError" OnSendingMail="CreateUserWizard1_SendingMail"
          FinishCompleteButtonType="Link" FinishCompleteButtonStyle-CssClass="btntribe"
           >
          <WizardSteps>
             <asp:CreateUserWizardStep ID="createuser" runat="server">
                <ContentTemplate>
                <span class="accountlogin-error"><asp:Literal ID="ErrorMessage" runat="server" EnableViewState="False" SkinID="FeedbackKO"></asp:Literal></span>
                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;" class="profile-table">
                   <tr>
                      <th>Email Address:
                      <asp:RequiredFieldValidator ID="valRequireUserName" CssClass="validator" runat="server" ControlToValidate="UserName"
                            SetFocusOnError="true" Display="Static" ErrorMessage="E-mail is required." ToolTip="E-mail is required." 
                            ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
                         <asp:RegularExpressionValidator runat="server" ID="valEmailPattern" CssClass="validator" Display="Dynamic" 
                            SetFocusOnError="true" ValidationGroup="CreateUserWizard1" ControlToValidate="UserName" 
                            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                            ErrorMessage="The e-mail address you specified is invalid.">*</asp:RegularExpressionValidator>
                      </th>
                      <td><asp:TextBox TabIndex="4" runat="server" ID="UserName" Width="250px" /></td> 
                      <td rowspan="99" valign="top">
                          <div class="cookiewarning">
                              <div class="title">
                                  Problems managing your email?</div>
                              <div class="warning">
                                  Please be sure to enter your email address in the box provided before choosing to
                                  unsubscribe or subscribe. Also be sure that javascript and cookies enabled on your
                                  browser.
                                  <br />
                                  <br />
                                  Sometimes the email you receive is forwarded from another account. Be sure you are
                                  using the correct email address.
                                  <br />
                                  <br />
                                  If you continue to have problems please <a href="Contact.aspx">contact us</a>
                              </div>
                          </div>
                          </div>
                      </td>       
                   </tr>    
                    <tr>
                      <th>Confirm Email:
                         <asp:RequiredFieldValidator ID="valRequireEmail" CssClass="validator" runat="server" ControlToValidate="Email" SetFocusOnError="true" Display="Static"
                            ErrorMessage="E-mail is required." ToolTip="E-mail is required." ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
                         <asp:CompareValidator ID="CompareEmail" CssClass="validator" runat="server" ControlToCompare="UserName" ControlToValidate="Email" 
                            SetFocusOnError="true" Display="dynamic" ErrorMessage="Confirm E-mail does not match E-mail" 
                            ToolTip="Confirm E-mail does not match E-mail" ValidationGroup="CreateUserWizard1" >*</asp:CompareValidator>
                      </th>            
                      <td><asp:TextBox TabIndex="5" runat="server" ID="Email" Width="250px" Text='<%# Email %>' /></td>
                   </tr>          
                   <tr>
                      <th>Password:
                         <asp:RequiredFieldValidator ID="valRequirePassword" CssClass="validator" runat="server" ControlToValidate="Password" SetFocusOnError="true" 
                            Display="Static" ErrorMessage="Password is required." ToolTip="Password is required." ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
                         <asp:RegularExpressionValidator ID="valPasswordLength" CssClass="validator" runat="server" ControlToValidate="Password" 
                            SetFocusOnError="true" Display="Static" 
                            ValidationExpression="[\@\*\-_\w!]{5,20}" 
                             ErrorMessage="* Valid password chars are letters (A-Z, a-z), numbers (0-9), exclamation point (!), underscore(_), at symbol(@) and asterick (*). Passwords must be at least 5 chars and cannot exceed 20 chars." 
                             ToolTip="* Valid password chars are letters (A-Z, a-z), numbers (0-9), exclamation point (!), underscore(_), at symbol(@) and asterick (*). Passwords must be at least 5 chars and cannot exceed 20 chars."
                            ValidationGroup="CreateUserWizard1">*</asp:RegularExpressionValidator>
                      </th>   
                      <td><asp:TextBox runat="server" TabIndex="6" ID="Password" TextMode="Password" Width="250px" /></td>         
                   </tr>
                   <tr>
                      <th>Confirm Password:
                         <asp:RequiredFieldValidator ID="valRequireConfirmPassword" CssClass="validator" runat="server" ControlToValidate="ConfirmPassword" 
                            SetFocusOnError="true" Display="Static" ErrorMessage="Confirm Password is required." ToolTip="Confirm Password is required."
                            ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
                         <asp:CompareValidator ID="valComparePasswords" CssClass="validator" runat="server" ControlToCompare="Password" SetFocusOnError="true"
                            ControlToValidate="ConfirmPassword" Display="Static" ErrorMessage="The Password and Confirmation Password must match."
                            ValidationGroup="CreateUserWizard1">*</asp:CompareValidator>
                      </th>            
                      <td><asp:TextBox TabIndex="7" runat="server" ID="ConfirmPassword" TextMode="Password" Width="250px" /></td>
                   </tr>
              
                   <tr>
                      <th>Security Question:
                         <asp:CompareValidator ID="reqReqQuestion" runat="server" ControlToValidate="Question" Operator="GreaterThan" 
                            ValidationGroup="CreateUserWizard1" ValueToCompare="0" ErrorMessage="Please select a security question." >*</asp:CompareValidator>
                      </th>            
                      <td>
                        <asp:DropDownList ID="Question" runat="server"  TabIndex="8" AppendDataBoundItems="true" Width="256px" >
                            <asp:ListItem Text="-- Select Security Question --" />
                        </asp:DropDownList>
                      </td>
                     
                   </tr>
                   <tr>
                      <th style="vertical-align:top;">Security Answer:
                         <asp:RequiredFieldValidator ID="valRequireAnswer" CssClass="validator" runat="server" ControlToValidate="Answer" 
                            SetFocusOnError="true" Display="Static" ErrorMessage="Security answer is required." ToolTip="Security answer is required."
                            ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
                      </th>            
                      <td style="vertical-align:top;" class="input"><asp:TextBox TabIndex="9" runat="server" ID="Answer" MaxLength="200" Width="250px" /></td>
                   </tr>
                </table>
                
                <asp:ValidationSummary ValidationGroup="CreateUserWizard1" ID="ValidationSummary1" CssClass="validationsummary" runat="server" 
                     ShowMessageBox="True" ShowSummary="true" />
                </ContentTemplate>
             </asp:CreateUserWizardStep>
         
         
             <asp:WizardStep ID="profile" runat="server" Title="Set preferences">
                <div class="title">Set-up your profile</div>
                <wc:UserProfile ID="UserProfile1" runat="server" />
             </asp:WizardStep>
         
         
         <asp:CompleteWizardStep runat="server"></asp:CompleteWizardStep>
        
      </WizardSteps>
      <MailDefinition BodyFileName="" />
   </asp:CreateUserWizard>   
   </div>
</div>