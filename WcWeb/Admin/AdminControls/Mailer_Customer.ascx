<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Mailer_Customer.ascx.cs" Inherits="WillCallWeb.Admin.AdminControls.Mailer_Customer" %>
<div id="srceditor">
    <div id="mailereditor">
        <div class="jqhead rounded">
            <table border="0" cellpadding="0" cellspacing="0" width="100%" class="hedtbl">
                <tr>
                    <th><%= MailerTypeTitle.ToUpper() %> MAILER</th>
                    <td style="width:100%;">
                        Params in current template: <%= string.Join(", ", ParamNames) %> - format is &lt;PARAM&gt;PARAMNAME&lt;PARAM&gt;
                    </td>
                </tr>
            </table>
        </div>
        <asp:ValidationSummary ID="ValidationSummary1" CssClass="validationsummary" HeaderText="Please correct the following errors:" 
            ValidationGroup="custemail" runat="server" />
        <div class="jqpnl rounded">
            <table border="0" cellpadding="0" cellspacing="0" width="100%" class="edittabl">
                <tr>
                    <th colspan="2" style="width:50%;text-align:center;">Create Template
                        <asp:CustomValidator ID="CustomValidation" runat="server" ValidationGroup="custemail" CssClass="validator" 
                            Display="Dynamic" ErrorMessage="CustomValidator">*</asp:CustomValidator>
                    </th>
                    <td>&nbsp;</td>
                    <th style="width:50%;text-align:center;">
                        <asp:Button ID="btnPreview" runat="server" CssClass="btnmed" 
                            Text="Preview" onclick="btnPreview_Click" />
                        View Template - Sample Email
                    </th>
                </tr>
                <tr>
                    <th><asp:Button ID="btnLoadTemplate" runat="server" CssClass="btnmed"
                            Text="Load" onclick="btnLoadTemplate_Click" /></th>
                    <td>
                        <asp:DropDownList ID="ddlTemplates" runat="server" OnDataBound="ddlTemplates_DataBound" 
                            OnDataBinding="ddlTemplates_DataBinding" Width="100%">
                        </asp:DropDownList>
                    </td>
                    <td rowspan="10"><img src="/Images/spacer.gif" alt="" border="0" height="1px" width="20px" /></td>
                    <td rowspan="10" style="vertical-align:top !important;">
                        <div class="jqinstruction rounded" >
                            <div><asp:Literal ID="litSubject" runat="server" OnDataBinding="litSubject_DataBinding" /></div>
                            <div><asp:Literal ID="litSample" runat="server" EnableViewState="false" OnDataBinding="litSample_DataBinding" /></div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <th><asp:RequiredFieldValidator ID="Required1" runat="server" ValidationGroup="custemail" CssClass="validator" 
                            Display="Static" ErrorMessage="Please enter a subject."  
                            ControlToValidate="txtSubject">*</asp:RequiredFieldValidator>
                        Subject</th>
                    <td>
                        <asp:TextBox ID="txtSubject" runat="server" MaxLength="300" Width="100%" />                    
                    </td>
                </tr>
                <tr>
                    <th><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="custemail" CssClass="validator" 
                            Display="Static" ErrorMessage="Please enter a header."  
                            ControlToValidate="txtHeader">*</asp:RequiredFieldValidator>
                        Header</th>
                    <td>
                        <asp:TextBox ID="txtHeader" runat="server" MaxLength="300" Width="100%" />
                    </td>
                </tr>
                <tr>
                    <th style="vertical-align:top;">Body</th>
                    <td>
                        <asp:TextBox ID="txtBody" runat="server" MaxLength="2000" Width="100%" Height="350px" 
                            TextMode="multiline" />
                    </td>
                </tr>
                <tr>
                    <th><asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ValidationGroup="custemail" 
                            CssClass="validator" Display="Static" ErrorMessage="Please enter a complimentary closing."  
                            ControlToValidate="txtClosing">*</asp:RequiredFieldValidator>
                         Closing
                    </th>
                    <td><asp:TextBox ID="txtClosing" runat="server" MaxLength="300" Width="100%" /></td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Label ForeColor="Green" Font-Bold="true" ID="lblStatus" runat="server" /></td></tr>
                <tr>
                    <th>Save As...</th>
                    <td>
                        <asp:TextBox ID="txtTemplateName" runat="server" MaxLength="256" Width="100%" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <div class="jqinstruction rounded" style="text-align:center;">
                            <asp:Button ID="btnSaveTemplate" CausesValidation="false" runat="server" CssClass="btnmed" width="100px"
                                Text="Save Template" OnClick="btnSaveTemplate_Click" CommandName="save" />
                            <asp:CustomValidator ID="CustomSave" runat="server" ValidationGroup="custemail" CssClass="validator" 
                                Display="Static" ErrorMessage="CustomValidator">*</asp:CustomValidator>
                            <asp:Button ID="btnOverwriteTemplate" CausesValidation="false" runat="server" Enabled="false" CssClass="btnmed" Width="100px" 
                                Text="Overwrite ?" OnClick="btnSaveTemplate_Click" CommandName="overwrite" />
                            <asp:Button ID="btnCancelSave" CausesValidation="false" runat="server" CssClass="btnmed" Width="100px"
                                Text="Cancel" OnClick="btnSaveTemplate_Click" CommandName="cancel" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <th><asp:Button ID="btnTest" CausesValidation="false" runat="server" CssClass="btnmed" Width="100px"
                            Text="Send Test Email" OnClick="btnTest_Click" /></th>
                    <td valign="top"><asp:TextBox ID="txtTestEmail" runat="server" MaxLength="256" Width="100%" /></td>
                </tr>
            </table>
        </div>
    </div>
</div>