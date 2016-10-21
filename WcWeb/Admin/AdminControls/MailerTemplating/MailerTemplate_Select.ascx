<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MailerTemplate_Select.ascx.cs" Inherits="WillCallWeb.Admin.AdminControls.MailerTemplating.MailerTemplate_Select" %>
<%@ Register src="MailerTemplate_Menu.ascx" tagname="MailerTemplate_Menu" tagprefix="uc1" %>
<div id="srceditor">
    <uc1:MailerTemplate_Menu ID="MailerTemplate_Menu1" runat="server" Title="Select / Create" />
    <div id="mailertemplating">
        <asp:ValidationSummary ID="ValidationSummary2" CssClass="validationsummary" HeaderText="Please correct the following errors:" 
            ValidationGroup="mailer" runat="server" /> 
        <div class="jqpnl rounded">
            <asp:FormView ID="FormMailer" runat="server" DataKeyNames="Id" Width="100%" DefaultMode="Edit" DataSourceID="SqlInsertMailer" 
                OnItemInserting="FormMailer_Inserting" 
                OnItemInserted="FormMailer_Inserted" 
                OnModeChanging="FormMailer_ModeChanging">
                <EmptyDataTemplate>
                    <table border="0" cellspacing="0" cellpadding="0" width="100%" class="edittabl">
                        <tr>
                            <th>Select Mailer</th>
                            <td style="width:100%;">
                                <asp:DropDownList ID="ddlMailerList" runat="server" AppendDataBoundItems="true" DataSourceID="SqlMailerList" Width="600px"
                                    OnDataBound="ddlMailerList_DataBound" OnSelectedIndexChanged="ddlMailerList_SelectedIndexChanged" AutoPostBack="true"
                                    DataTextField="Name" DataValueField="Id">
                                    <asp:ListItem Text="<-- SELECT A MAILER -->" Value="0" />
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <div class="cmdsection">
                        <asp:Button Id="btnNew" CausesValidation="false" runat="server" CommandName="New" 
                            Text="New Mailer" CssClass="btnmed" />
                    </div>  
                </EmptyDataTemplate>
                <InsertItemTemplate>
                    <table border="0" cellspacing="0" cellpadding="0" width="100%" class="edittabl">
                        <tr>
                            <td><asp:CustomValidator ID="rowValidator" runat="server" ValidationGroup="mailer" CssClass="validator" Display="Static"
                                    ErrorMessage="CustomValidator">*</asp:CustomValidator>
                            </td>
                            <th><a href="javascript: alert('REQUIRED. If you do not wish to use a template, use the create mailer page')" class="infomark">?</a>
                                 Select Template
                            </th>
                            <td style="width:100%;">
                                <asp:DropDownList ID="ddlTemplateList" runat="server" AppendDataBoundItems="true" DataSourceID="SqlTemplateList"
                                    Width="100%" DataTextField="Name" DataValueField="Id">
                                    <asp:ListItem Text="<-- SELECT A TEMPLATE -->" Value="0" />
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td><asp:RequiredFieldValidator ID="RequiredName" runat="server" ValidationGroup="mailer" ErrorMessage="Name is required"
                                    Display="static" CssClass="validator" ControlToValidate="txtName">*</asp:RequiredFieldValidator>
                            </td>
                            <th><a href="javascript: alert('REQUIRED. This current date will be inserted into the name as well upon creation. The date is for clarification only and does not need to be changed after created')"
                                    class="infomark">?</a>
                                Name
                            </th>
                            <td><asp:TextBox ID="txtName" runat="server" MaxLength="200" Width="100%" Text='<%#Bind("Name") %>' /></td>
                        </tr>
                        <tr>
                            <td style="vertical-align:top !important;"><asp:RequiredFieldValidator ID="RequiredSubject" runat="server" ValidationGroup="mailer" ErrorMessage="Subject is required"
                                    Display="static" CssClass="validator" ControlToValidate="txtSubject">*</asp:RequiredFieldValidator>
                            </td>
                            <th style="vertical-align:top !important;">
                                <a href="javascript: alert('REQUIRED. This will be the subject of the email')" class="infomark">?</a>
                                Subject
                            </th>
                            <td>
                                <asp:DropDownList ID="ddlSubject" runat="server" AppendDataBoundItems="true" AutoPostBack="true" DataTextField="Subject"
                                    OnSelectedIndexChanged="ddlSubject_SelectedIndexChanged" DataSourceID="SqlSubjectList" Width="300px">
                                    <asp:ListItem Text="<-- Select a Subject -->" />
                                </asp:DropDownList>
                                <br />
                                <br />
                                Or create a new subject<br />
                                <br />
                                <asp:TextBox ID="txtSubject" runat="server" MaxLength="256" Width="325px" Text='<%#Bind("Subject") %>' />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <br />
                                <br />
                                <asp:Button ID="btnInsert" runat="server" CssClass="btnmed" CommandName="Insert" 
                                    CausesValidation="true" Text="Save" />
                                <asp:Button ID="btnCancel" runat="server" CssClass="btnmed" CommandName="Cancel" 
                                    CausesValidation="false" Text="Cancel" />
                            </td>
                        </tr>
                    </table>
                </InsertItemTemplate>
            </asp:FormView>
            <br /><br /><br />
            <asp:FormView ID="FormTemplate" runat="server" DataKeyNames="Id" Width="100%" DefaultMode="Edit" DataSourceID="SqlInsertTemplate" 
                OnItemInserting="FormTemplate_Inserting" 
                OnItemInserted="FormTemplate_Inserted" 
                OnModeChanging="FormTemplate_ModeChanging">
                <EmptyDataTemplate>
                    <table border="0" cellspacing="0" cellpadding="0" width="100%" class="edittabl">
                        <tr>
                            <td>&nbsp;</td>
                            <th>Select Template</th>
                            <td style="width:100%;">
                                <asp:DropDownList ID="ddlTemplateList" runat="server" AppendDataBoundItems="true" DataSourceID="SqlTemplateList" Width="600px"
                                    OnDataBound="ddlTemplateList_DataBound" OnSelectedIndexChanged="ddlTemplateList_SelectedIndexChanged" AutoPostBack="true"
                                    DataTextField="Name" DataValueField="Id">
                                    <asp:ListItem Text="<-- SELECT A TEMPLATE -->" Value="0" />
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <div class="cmdsection">
                        <asp:Button Id="btnNew" CausesValidation="false" runat="server" CommandName="New" 
                            Text="New Template" CssClass="btnmed" />
                    </div>
                </EmptyDataTemplate>
                <InsertItemTemplate>
                    <table border="0" cellspacing="0" cellpadding="0" width="100%" class="edittabl">
                        <tr>
                            <th colspan="2">Existing</th>
                            <td style="width:100%;">
                                <asp:DropDownList ID="ddlCopyTemplate" runat="server" AppendDataBoundItems="true" DataSourceID="SqlTemplateList" 
                                    Width="100%" AutoPostBack="false" DataTextField="Name" DataValueField="Id">
                                    <asp:ListItem Text="<-- SELECT TEMPLATE -->" Value="0" />
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CustomValidator ID="CustomCopy" runat="server" ValidationGroup="mailer" ErrorMessage="Please select a template to copy."
                                    Display="static" CssClass="validator" >*</asp:CustomValidator>
                            </td>
                            <td><br /><br /></td>
                            <td><asp:CheckBox ID="chkCopyTemplate" runat="server" Text="Copy Selected Template?" TextAlign="Right" /></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:RequiredFieldValidator ID="RequiredName" runat="server" ValidationGroup="mailer" ErrorMessage="Name is required"
                                    Display="static" CssClass="validator" ControlToValidate="txtName">*</asp:RequiredFieldValidator>
                            </td>
                            <th>Name</th>
                            <td style="width:700px;"><asp:TextBox ID="txtName" runat="server" Text='<%#Bind("Name") %>' MaxLength="256" Width="100%" />
                            </td>
                        </tr>
                        <tr>
                            <th colspan="2">Description</th>
                            <td><asp:TextBox ID="txtDescription" runat="server" Text='<%#Bind("Description") %>' MaxLength="500" Width="100%" /></td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <br />
                                <br />
                                <asp:Button ID="btnInsert" runat="server" CssClass="btnmed" CommandName="Insert" 
                                    CausesValidation="true" Text="Save" />
                                <asp:Button ID="btnCancel" runat="server" CssClass="btnmed" CommandName="Cancel" 
                                    CausesValidation="false" Text="Cancel" />
                            </td>
                        </tr>
                    </table>
                </InsertItemTemplate>
            </asp:FormView>
        </div>            
    </div>
</div>
<asp:SqlDataSource ID="SqlMailerList" runat="server" EnableCaching="false" ConnectionString="<%$ ConnectionStrings:WillCallConnectionString %>"
    SelectCommand="SELECT ent.[Id], ent.[Name] FROM [Mailer] ent ORDER BY ent.[Id] DESC "></asp:SqlDataSource>
<asp:SqlDataSource ID="SqlTemplateList" runat="server" EnableCaching="false" ConnectionString="<%$ ConnectionStrings:WillCallConnectionString %>"
    SelectCommand="SELECT ent.[Id], ent.[Name] FROM [MailerTemplate] ent ORDER BY ent.[Id] DESC "></asp:SqlDataSource>
<asp:SqlDataSource ID="SqlSubjectList" runat="server" EnableCaching="true" CacheDuration="20" ConnectionString="<%$ ConnectionStrings:WillCallConnectionString %>"
    SelectCommand="SELECT DISTINCT(ent.Subject) as Subject FROM [Mailer] ent ORDER BY ent.Subject "></asp:SqlDataSource>
<asp:SqlDataSource ID="SqlInsertMailer" runat="server" EnableCaching="false" ConnectionString="<%$ ConnectionStrings:WillCallConnectionString %>"
    InsertCommand="INSERT INTO Mailer([tMailerTemplateId], [Name], [Subject]) VALUES (@tMailerTemplateId, @Name, @Subject); IF(@@ROWCOUNT > 0) SELECT @insId = SCOPE_IDENTITY() ELSE SELECT @insId = 0;"
    OnInserted="SqlInsertMailer_Inserted" >
    <InsertParameters>
        <asp:Parameter Name="tMailerTemplateId" Type="Int32" DefaultValue="0" ConvertEmptyStringToNull="false" />
        <asp:Parameter Name="insId" Type="Int32" DefaultValue="0" Direction="Output" />
    </InsertParameters>    
</asp:SqlDataSource>
<asp:SqlDataSource ID="SqlInsertTemplate" runat="server" EnableCaching="false" ConnectionString="<%$ ConnectionStrings:WillCallConnectionString %>"
    InsertCommand="INSERT INTO MailerTemplate([ApplicationId], [Name], [Description], [Header], [Footer]) VALUES (@appId, @Name, @Description, '', ''); IF(@@ROWCOUNT > 0) SELECT @insId = SCOPE_IDENTITY() ELSE SELECT @insId = 0;"
    OnInserting="SqlInsertTemplate_Inserting"
    OnInserted="SqlInsertTemplate_Inserted" >
    <InsertParameters>
        <asp:Parameter Name="appId" DbType="Guid" />
        <asp:Parameter Name="insId" Type="Int32" DefaultValue="0" Direction="Output" />
    </InsertParameters>    
</asp:SqlDataSource>