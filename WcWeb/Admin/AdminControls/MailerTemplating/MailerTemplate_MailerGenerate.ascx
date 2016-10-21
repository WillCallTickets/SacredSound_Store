<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MailerTemplate_MailerGenerate.ascx.cs" Inherits="WillCallWeb.Admin.AdminControls.MailerTemplating.MailerTemplate_MailerGenerate" %>
<%@ Register src="MailerTemplate_Menu.ascx" tagname="MailerTemplate_Menu" tagprefix="uc1" %>
<div id="srceditor">
    <uc1:MailerTemplate_Menu ID="MailerTemplate_Menu1" runat="server" Title="Mailer Generation" />
    <div id="mailertemplating">
        <asp:ValidationSummary ID="ValidationSummary2" CssClass="validationsummary" HeaderText="Please correct the following errors:" 
            ValidationGroup="mailer" runat="server" />
        <div class="jqhead rounded">
            <h3 class="entry-title">
                <asp:CustomValidator ID="CustomValidation" runat="server" ValidationGroup="mailer" CssClass="validator" Display="Static" 
                    ErrorMessage="CustomValidator">*</asp:CustomValidator><%=Atx.CurrentMailer.Name %></h3>
        </div>
        <div class="jqpnl rounded">
            <table border="0" cellpadding="0" cellspacing="0" width="100%" class="edittabl">
                <tr>
                    <th>
                        <asp:Button ID="btnPreview" runat="server" Text="Create Preview" CommandName="preview" CausesValidation="false" 
                            OnClick="btnPreview_Click" CssClass="btnmed" Width="100px" />
                    </th>
                    <td style="width:50%;" class="intr">Preview the mailer below</td>
                    <th>
                        <asp:Button ID="btnGenerate" runat="server" Text="Generate Mailer" CommandName="genmailer" CausesValidation="false" 
                            OnClick="btnGenerate_Click" CssClass="btnmed" Width="100px" />
                    </th>
                    <td style="width:50%;" class="intr">Generate the final version of the mailer</td>
                </tr>
            </table>
        </div>
        <iframe src="/Admin/AdminControls/MailerTemplating/MailerTemplatingPreview.aspx" width="100%" height="1500" align="left"></iframe>
    </div>    
</div>