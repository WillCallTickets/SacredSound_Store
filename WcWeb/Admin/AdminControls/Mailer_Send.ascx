<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Mailer_Send.ascx.cs" Inherits="WillCallWeb.Admin.AdminControls.Mailer_Send" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register src="~/Components/Util/CalendarClock.ascx" tagname="CalendarClock" tagprefix="uc1" %>
<div id="srceditor">
    <div id="mailersender">
        <div class="jqhead rounded">
            <table border="0" cellpadding="0" cellspacing="0" width="100%" class="edittabl">
                <tr>
                    <th>SEND MAILER</th>
                    <td style="width:100%;">
                        <asp:DropDownList ID="ddlMailers" runat="server" OnDataBinding="ddlMailers_DataBinding" OnDataBound="ddlMailers_DataBound" Width="100%"
                            AutoPostBack="true" OnSelectedIndexChanged="ddlMailers_SelectedIndexChanged" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Button ID="btnCancel" CssClass="btnmed" runat="server" CommandName="Cancel" Text="Cancel" 
                            CausesValidation="false" />
                        <asp:Button Id="btnEditor" runat="server" OnClick="btnEditor_Click" Text="Edit" 
                            CssClass="btnmed" CausesValidation="false" />
                        <asp:Button ID="btnPreview" runat="server" CssClass="btnmed" Text="Preview"
                            OnClientClick="javascript:doPagePopup('/Admin/MailerViewer.aspx','true'); " CausesValidation="false" />
                    </td>
                </tr>
            </table>
        </div>
        <asp:ValidationSummary ID="ValidationSummary2" CssClass="validationsummary" HeaderText="Please correct the following errors:" 
            ValidationGroup="mailer" runat="server" />
        <asp:Label runat="server" ID="lblFeedback" ForeColor="Green" Font-Bold="true" Text="" Visible="false" />
        <asp:FormView ID="FormView1" runat="server" DefaultMode="Edit" Width="100%" DataKeyNames="Id,TEmailLetterId"
            OnDataBinding="FormView1_DataBinding" OnModeChanging="FormView1_ModeChanging" 
            OnDataBound="FormView1_DataBound" OnItemCommand="FormView1_ItemCommand" >
            <EmptyDataTemplate>
                <table border="0" cellpadding="0" cellspacing="0" width="100%" class="edittabl">
                    <tr>
                        <th>Add A New Mailer...</th>
                        <td>
                            <asp:Button Id="btnEditor" CausesValidation="false" runat="server" CommandName="GotoEditPage" Text="New" 
                                CssClass="btnmed" />
                        </td>
                    </tr>
                </table>
            </EmptyDataTemplate>
            <EditItemTemplate>
                <div class="jqpnl rounded">
                    <table border="0" cellspacing="0" cellpadding="0" width="100%" class="edittabl" style="margin:0;">
                        <tr>
                            <asp:Literal ID="litInQueue" runat="server" />
                            <td style="width: 100%;">
                                <asp:Button ID="btnRemoveQ" CssClass="btnmed" Width="100px" runat="server" CommandName="qremove" 
                                   CommandArgument='<%#Eval("TEmailLetterId") %>' Text="Remove From Q" CausesValidation="false" />
                                <asp:Button ID="btnPauseQ" CssClass="btnmed" Width="100px" runat="server" CommandName="qpause" 
                                   CommandArgument='<%#Eval("TEmailLetterId") %>' Text="Pause Q" CausesValidation="false" />
                                <asp:Button ID="btnRestartQ" CssClass="btnmed" Width="100px" runat="server" CommandName="qrestart" 
                                   CommandArgument='<%#Eval("TEmailLetterId") %>' Text="Restart Q" CausesValidation="false" />
                                <asp:Button ID="btnRefresh" CssClass="btnmed" Width="100px" runat="server" CommandName="qrefresh" 
                                   Text="Refresh" CausesValidation="false" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="jqpnl rounded" style="margin-top:2px;">
                    <table border="0" cellspacing="0" cellpadding="0" width="100%" class="edittabl">
                        <tr>
                            <th>
                                <asp:Button ID="btnTest" CssClass="btnmed" Width="120px" runat="server" CommandName="sendtest" 
                                   CommandArgument='<%#Eval("TEmailLetterId") %>' Text="Send Test Emails" 
                                   OnClientClick="return confirm('Have you verified the recipient emails?')" />
                            </th>
                            <td><asp:TextBox ID="txtTestList" runat="server" TextMode="MultiLine" Width="300px" Height="50px" /></td>
                            <td class="intr">one address per line or comma-separated list
                                    <asp:CustomValidator ID="CustomTest" runat="server" ValidationGroup="mailer" CssClass="invisible" 
                                        Display="Static" ErrorMessage="CustomValidator">*</asp:CustomValidator>
                            </td>
                        </tr>
                        <tr><td colspan="3"><hr /></td></tr>
                        <tr>
                            <th>Date To Send</th>
                            <td style="padding:0;"><uc1:CalendarClock ID="clockSend" runat="server" UseTime="true" UseReset="false" 
                                    ValidationGroup="mailer" />
                            </td>
                            <td class="intr">It is highly recommended to send the email with a future date/time. Give yourself time to review
                            </td>
                        </tr>
                        <tr><td colspan="3"><hr /></td></tr>
                        <tr>
                            <th><asp:Button ID="btnSubscribers" CssClass="btnmed" Width="120px" runat="server" CommandName="sendsubscription" 
                                   CommandArgument='<%#Eval("TEmailLetterId") %>' Text="Send To Subscribers" CausesValidation="true" 
                                   ValidationGroup="mailer" /></th>
                            <th style="text-align:left;"><asp:Literal ID="litSubscribed" runat="server" /></th>
                            <td class="intr">date to send must be specified
                                    <asp:CustomValidator ID="CustomSubscription" runat="server" ValidationGroup="mailer" CssClass="invisible" 
                                        Display="Static" ErrorMessage="CustomValidator">*</asp:CustomValidator>
                            </td>
                        </tr>
                        <tr><td colspan="3"><hr /></td></tr>
                        <tr>
                            <th>
                                <asp:Button ID="btnShort" CssClass="btnmed" Width="120px" runat="server" CommandName="sendshortlist" 
                                   CommandArgument='<%#Eval("TEmailLetterId") %>' Text="Send To Custom List" 
                                   OnClientClick="return confirm('Have you verified the recipient emails and the time to send them?')" />
                            </th>
                            <td><asp:TextBox ID="txtShortList" runat="server" TextMode="MultiLine" Width="300px" Height="250px" /></td>
                            <td class="intr">
                                <ul>
                                    <li>one address per line</li>
                                    <li>date to send must be specified</li>
                                    <li>subscription opt out will not be included</li>
                                </ul>
                                <asp:CustomValidator ID="CustomShort" runat="server" ValidationGroup="mailer" CssClass="invisible" 
                                    Display="Static" ErrorMessage="CustomValidator">*</asp:CustomValidator>
                            </td>
                        </tr>
                    </table>
                </div>
            </EditItemTemplate>
        </asp:FormView>
    </div>
</div>            