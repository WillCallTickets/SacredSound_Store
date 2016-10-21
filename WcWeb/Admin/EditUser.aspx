<%@ Page Language="C#" MasterPageFile="~/TemplateAdmin.master" AutoEventWireup="true" CodeFile="EditUser.aspx.cs" 
Inherits="WillCallWeb.Admin.EditUser" Title="Admin - Edit User" %>
<%@ Register Src="~/Controls/UserProfile.ascx" TagName="UserProfile" TagPrefix="uc1" %>
<%@ Register src="~/Controls/UserSubscriptions.ascx" tagname="UserSubscriptions" tagprefix="uc2" %>
<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" Runat="Server">
    <div id="edituser">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="jqhead rounded">
                    
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" class="edittabl" style="margin:0;">
                        <tr>
                            <th style="text-align:left;"><h3 class="entry-title"><%=userName %></h3></th>
                            <td>
                                <asp:CustomValidator ID="CustomValidation" runat="server" ValidationGroup="editor" CssClass="invisible" 
                                    Display="Static" ErrorMessage="CustomValidator">*</asp:CustomValidator>
                            </td>
                            <td style="width:100%;"><asp:Button ID="btnSales" CausesValidation="false" CssClass="btnmed" Width="100px" OnClick="btnSales_Click" runat="server" 
                                    CommandName="Sales" Text="Sales History" />
                            </td>
                        </tr>
                    </table>
                </div>

                <asp:ValidationSummary ID="ValidationSummary1" CssClass="validationsummary" HeaderText="" ValidationGroup="editor" runat="server" />
                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td style="width:30%;padding-right:6px;">
                            <div class="jqedt rounded">
                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                    <tr>
                                        <th class="subtitle" style="text-align:left;width:100%;">General Info</th>
                                        <td style="text-align:right;white-space:nowrap;">
                                            <%if (this.Page.User.IsInRole("Administrator")){%>
                                            <asp:Button ID="btnReset" runat="server" ToolTip="This will reset the password and email the customer with the new password." CausesValidation="false" 
                                                Text="Reset Pass" CssClass="btnmed" Width="100px" OnClick="btnReset_Click" OnClientClick="return confirm('Are you sure you want to reset this password?')" />
                                            <%} %>&nbsp;
                                        </td>
                                    </tr>
                                </table>
                                <table border="0" cellpadding="0" cellspacing="0" width="100%" class="edttbl">
                                    <tr>
                                       <th>User ID</th>
                                       <td style="width:100%;"><asp:Literal runat="server" ID="lblUserID" /></td>
                                    </tr>
                                    <tr>
                                        <th>User Name</th>
                                        <td><asp:Literal runat="server" ID="lblUserName" /></td>
                                    </tr>
                                    <tr>
                                        <th>E-mail</th>
                                        <td><asp:HyperLink runat="server" ID="lnkEmail" /></td>
                                    </tr>
                                    <%if (this.Page.User.IsInRole("Super")) {%>
                                    <tr>
                                        <th>Password</th>
                                        <td><asp:Literal ID="litPassword" runat="server" OnDataBinding="litPassword_DataBinding" /></td>
                                    </tr>                          
                                    <%} %>
                                    <tr>
                                        <th>Registered</th>
                                        <td><asp:Literal runat="server" ID="lblRegistered" /></td>
                                    </tr>
                                    <tr>
                                        <th>Last Login</th>
                                        <td><asp:Literal runat="server" ID="lblLastLogin" /></td>
                                    </tr>
                                    <tr>
                                        <th>Last Activity</th>
                                        <td><asp:Literal runat="server" ID="lblLastActivity" /></td>
                                    </tr>
                                    <tr>
                                        <th>Online Now</th>
                                        <td><asp:CheckBox runat="server" ID="chkOnlineNow" Enabled="false" /></td>
                                    </tr>
                                    <tr>
                                        <th>Active</th>
                                        <td><asp:CheckBox runat="server" ID="chkApproved" AutoPostBack="true" OnCheckedChanged="chkApproved_CheckedChanged" /></td>
                                    </tr>
                                    <tr>
                                        <th>Locked Out</th>
                                        <td><asp:CheckBox runat="server" ID="chkLockedOut" AutoPostBack="true" OnCheckedChanged="chkLockedOut_CheckedChanged" /></td>
                                    </tr>
                                </table>
                            </div>
                            <div class="jqedt rounded">
                                <table border="0" cellpadding="0" cellspacing="0" class="edttbl">
                                    <tr>
                                        <td>
                                            <uc2:UserSubscriptions ID="UserSubscriptions1" OnUpdated="UserSubscriptions_Updated" AllowAdminSubscriptions="true" runat="server" />
                                            <asp:Literal ID="litUserIsEmail" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                        <td style="width:30%;padding-right:6px;">
                            <div class="jqedt rounded">
                                <div class="subtitle" style="padding:4px">Previous Usernames</div>
                                <table border="0" cellpadding="0" cellspacing="0" width="100%" class="edttbl">
                                    <tr>
                                        <td>
                                            <asp:Repeater ID="rptPreviousUsername" runat="server" OnDataBinding="rptPreviousUsername_DataBinding">
                                                <ItemTemplate>
                                                    <%#Eval("Text") %>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <asp:Panel ID="panelRoles" runat="server">
                                <div class="jqedt rounded">
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                        <tr>
                                            <th class="subtitle" style="text-align:left;width:100%;">Edit User Roles</th>
                                            <td style="text-align:right">
                                                <asp:Button runat="server" ID="btnUpdateRoles" CssClass="btnmed" Text="Update Roles" width="100px"
                                                    OnClick="btnUpdateRoles_Click" CausesValidation="false" />
                                            </td>
                                        </tr>
                                    </table>
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%" class="edttbl">
                                        <tr>
                                            <td colspan="2">
                                                <asp:Label runat="server" ID="lblRolesFeedbackOK" SkinID="FeedbackOK" Text="Roles updated successfully" Visible="false" />
                                                <asp:CheckBoxList runat="server" RepeatLayout="table" ID="chkRoles" RepeatColumns="2" CellSpacing="2" 
                                                    ondatabinding="chkRoles_DataBinding" ondatabound="chkRoles_DataBound" />
                                            </td>
                                        </tr>
                                        <%if (this.Page.User.IsInRole("Super")){%>
                                        <tr>
                                            <th>
                                                <asp:RequiredFieldValidator ID="valRequireNewRole" runat="server" ControlToValidate="txtNewRole" SetFocusOnError="true"
                                                    ErrorMessage="Role name is required." ToolTip="Role name is required." ValidationGroup="CreateRole">*</asp:RequiredFieldValidator>
                                                    New Role
                                            </th>
                                            <td style="width:100%;white-space:nowrap;"><asp:TextBox runat="server" ID="txtNewRole" />&nbsp;
                                                <asp:Button runat="server" CssClass="btnmed" Width="60px" ID="btnCreateRole" Text="Create" CausesValidation="false"
                                                    OnClick="btnCreateRole_Click" />
                                            </td>
                                        </tr>
                                        <%} %>
                                    </table>
                                </div>
                            </asp:Panel>
                            <div class="jqedt rounded">
                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                    <tr>
                                        <th class="subtitle" style="text-align:left;width:100%;">StoreCredit</th>
                                        <td style="text-align:right">
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                                <table border="0" cellpadding="0" cellspacing="0" width="100%" class="edttbl">
                                    <tr><td colspan="2"><asp:Label id="lblResetCredit" ForeColor="Red" runat="server" /></td></tr>
                                    <tr>
                                        <td><asp:TextBox ID="txtCredit" ReadOnly="true" runat="server" OnDataBinding="txtCredit_DataBinding" /></td>
                                        <td style="width:100%;">
                                            <asp:Button Enabled="true" ID="btnSyncCredit" CssClass="btnmed" width="100px" runat="server" 
                                                OnClientClick="return confirm('This will update the users profile to reflect the store credit in the database. Helpful for when things get out of sync');" 
                                                OnClick="btnSyncCredit_Click" Text="Sync Credit" CausesValidation="false" />
                                            </td><td style="vertical-align:middle;"><a href="javascript: alert('This will update the users profile to reflect the store credit in the database. Helpful for when things get out of sync.');" class="infomark">?</a>    
                                        </td>
                                    </tr>
                                    <tr>
                                        <td><asp:TextBox ID="txtCreditAdjustment" runat="server" /></td>
                                        <td>
                                            <asp:Button ID="btnResetCredit" CssClass="btnmed" Width="100px" runat="server" 
                                                OnClientClick="return confirm('This will allow an arbitrary amount to be added or subtracted from the users store credit');" 
                                                OnClick="btnResetCredit_Click" Text="Adjust Credit" CausesValidation="false" />
                                                </td><td style="vertical-align:middle;"><a href="javascript: alert('This will allow an arbitrary amount to be added or subtracted from the users store credit.');" class="infomark">?</a>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                        <td style="width:30%;padding-right:6px;">
                            <div class="jqedt rounded">
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <th class="subtitle" style="text-align:left;">User Settings</th>
                                        <td style="text-align:right;white-space:nowrap;">&nbsp;
                                            <%if (this.Page.User.IsInRole("OrderFiller")){%>
                                            <asp:Button runat="server" ID="btnUpdateProfile" CssClass="btnmed" width="100px"
                                                causesvalidation="false"
                                                Text="Update Settings" OnClick="btnUpdateProfile_Click" /></span>
                                            <%} %>
                                        </td>
                                    </tr>
                                </table>
                                <table border="0" cellpadding="0" cellspacing="0" class="edttbl">
                                    <tr>
                                        <td>
                                            <asp:Label runat="server" ID="lblProfileFeedbackOK" SkinID="FeedbackOK" Text="Profile updated successfully" Visible="false" />&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div id="adminprofile">
                                                <uc1:UserProfile ID="UserProfile1" runat="server" />
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <%if (this.Page.User.IsInRole("Super")){%>
                            <div class="jqedt rounded">
                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                    <tr>
                                        <th class="subtitle" style="text-align:left;">Change Email</th>
                                        <td style="text-align:right">
                                            <asp:Button runat="server" ID="btnChangeEmail" CssClass="btnmed" Width="100px"
                                                Text="Change Email" onclick="btnChangeEmail_Click" ValidationGroup="editor" CausesValidation="true" 
                                                OnClientClick="return confirm('Are you sure you want to change this email address/account name?')" />
                                        </td>
                                    </tr>
                                </table>
                                <table border="0" cellpadding="0" cellspacing="0" width="100%" class="edttbl">
                                    <tr>
                                        <th>
                                            <asp:RegularExpressionValidator runat="server" ID="valEmailPattern" ValidationGroup="editor" 
                                                 CssClass="validator" Display="Static" 
                                                SetFocusOnError="true" ControlToValidate="txtNewEmail" 
                                                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                                                ErrorMessage="Please enter a valid e-mail address." 
                                                 onload="valEmailPattern_Load">*</asp:RegularExpressionValidator>
                                            New Email</th>
                                        <td style="width:100%;"><asp:TextBox ID="txtNewEmail" runat="server" MaxLength="256" Width="190px" /></td>
                                    </tr>
                                </table>
                            </div>
                            <%} %>
                        </td>
                    </tr>
                </table>
                <div style="padding-right:8px;">
                    <asp:GridView ID="GridView1" runat="server" DataSourceID="SqlDataSource1" 
                        AllowPaging="true" PageSize="50" PagerSettings-Mode="Numeric" PagerSettings-Position="TopAndBottom"
                        CssClass="lsttbl" EnableViewState="false" Width="100%">
                        <AlternatingRowStyle BackColor="#f1f1f1" />
                        <EmptyDataTemplate>
                            <div class="lstempty">No User Events</div>
                        </EmptyDataTemplate>
                    </asp:GridView>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>


    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:WillCallConnectionString %>"
        OnSelecting="SqlDataSource1_Selecting"
        SelectCommand="SELECT eq.[DateToProcess], eq.[DateProcessed], eq.[Status], eq.[CreatorName], 
            eq.[UserName], eq.[Verb], eq.[OldValue], eq.[NewValue], eq.[Description], eq.[Context], eq.[Ip] 
            FROM [EventQArchive] eq, [Aspnet_Users] u 
            WHERE u.[ApplicationId] = @appId AND u.[UserName] = @userName AND u.[UserId] = eq.[UserId]
            UNION 
            SELECT eq.[DateToProcess], eq.[DateProcessed], eq.[Status], eq.[CreatorName], 
            eq.[UserName], eq.[Verb], eq.[OldValue], eq.[NewValue], eq.[Description], eq.[Context], eq.[Ip] 
            FROM [EventQ] eq, [Aspnet_Users] u 
            WHERE u.[ApplicationId] = @appId AND u.[UserName] = @userName AND u.[UserId] = eq.[UserId] 
            ORDER BY [DateToProcess] Desc">
        <SelectParameters>
            <asp:Parameter Name="appId" DbType="Guid" />
            <asp:QueryStringParameter Name="userName" QueryStringField="UserName" Type="string" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>