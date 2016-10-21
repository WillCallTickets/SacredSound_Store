<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MailerTemplate_TemplateEdit.ascx.cs" Inherits="WillCallWeb.Admin.AdminControls.MailerTemplating.MailerTemplate_TemplateEdit" %>
<%@ Register src="MailerTemplate_Menu.ascx" tagname="MailerTemplate_Menu" tagprefix="uc1" %>
<div id="srceditor">
    <uc1:MailerTemplate_Menu ID="MailerTemplate_Menu1" runat="server" Title="Template Editor" />
    <div id="mailertemplating">
        <asp:ValidationSummary ID="ValidationSummary2" CssClass="validationsummary" HeaderText="Please correct the following errors:" 
            ValidationGroup="mailer" runat="server" />
        <div class="jqhead rounded">
            <h3 class="entry-title"><asp:CustomValidator ID="CustomValidation" runat="server" ValidationGroup="mailer" CssClass="validator" Display="Static" 
                ErrorMessage="CustomValidator">*</asp:CustomValidator><%=Atx.CurrentMailerTemplate.Name %></h3>
        </div>
        <div class="jqpnl rounded">
            <div class="cmdsection">
                <asp:Button ID="btnSave" runat="server" CssClass="btnmed" CausesValidation="true" OnClick="btnSave_Click"
                    Text="Save" ValidationGroup="mailer" />
                <asp:Button ID="btnCancel" runat="server" CssClass="btnmed" CausesValidation="false" OnClick="btnCancel_Click"
                    Text="Cancel" />
            </div>
            <asp:FormView ID="FormTemplate" runat="server" DataKeyNames="Id" Width="100%" DefaultMode="Edit" DataSourceID="SqlTemplate" 
                OnModeChanging="FormTemplate_ModeChanging" 
                OnItemUpdated="FormTemplate_ItemUpdated">
                <EmptyDataTemplate>
                    <div class="lstempty">No Template Selected</div>
                </EmptyDataTemplate>
                <EditItemTemplate>
                    <table border="0" cellspacing="0" cellpadding="0" width="100%" class="edittabl">
                        <tr>
                            <td>
                                <asp:RequiredFieldValidator ID="RequiredName" runat="server" ValidationGroup="mailer" ErrorMessage="Name is required"
                                    Display="static" CssClass="validator" ControlToValidate="txtName">*</asp:RequiredFieldValidator>
                            </td>
                            <th>Name</th>
                            <td style="width:100%;">
                                <asp:TextBox ID="txtName" runat="server" Text='<%#Bind("Name") %>' MaxLength="256" Width="100%" />
                            </td>
                        </tr>
                        <tr>
                            <th colspan="2" style="vertical-align:top !important;">Description</th>
                            <td><asp:TextBox ID="txtDescription" runat="server" Text='<%#Bind("Description") %>' MaxLength="500" Width="100%" /></td>
                        </tr>
                        <tr>
                            <th colspan="2" style="vertical-align:top !important;">Style</th>
                            <td><asp:TextBox ID="txtStyle" runat="server" Font-Size="11px" Text='<%#Bind("Style") %>' TextMode="MultiLine" 
                                    MaxLength="500" Width="100%" Height="50px" /></td>
                        </tr>
                        <tr>
                            <th colspan="2" style="vertical-align:top !important;">Header</th>
                            <td><asp:TextBox ID="txtHeader" runat="server" Font-Size="11px" Text='<%#Bind("Header") %>' TextMode="MultiLine" 
                                    MaxLength="3250" Width="100%" Height="220px" /></td>
                        </tr>
                        <tr>
                            <th colspan="2" style="vertical-align:top !important;">Footer</th>
                            <td><asp:TextBox ID="txtFooter" runat="server" Font-Size="11px" Text='<%#Bind("Footer") %>' TextMode="MultiLine" 
                                    MaxLength="3250" Width="100%" Height="220px" /></td>
                        </tr>
                    </table>
                </EditItemTemplate>
            </asp:FormView>
        </div>
        <br /><br />
    </div>
</div>
<asp:SqlDataSource ID="SqlTemplate" runat="server" EnableCaching="false" ConnectionString="<%$ ConnectionStrings:WillCallConnectionString %>"
    SelectCommand="SELECT t.* FROM [MailerTemplate] t WHERE t.[Id] = @idx; "
    UpdateCommand="UPDATE [MailerTemplate] SET Name = @Name, Description = @Description, Style = @Style, Header = @Header, Footer = @Footer WHERE [Id] = @idx; " 
    OnSelecting="SqlTemplate_SelectId"
    OnUpdating="SqlTemplate_UpdateId" >
    <SelectParameters>
        <asp:Parameter Name="idx" Type="Int32" DefaultValue="0" />
    </SelectParameters>
    <UpdateParameters>
        <asp:Parameter Name="idx" Type="Int32" DefaultValue="0" />
    </UpdateParameters>    
</asp:SqlDataSource>
