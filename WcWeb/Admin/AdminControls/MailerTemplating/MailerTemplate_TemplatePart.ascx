<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MailerTemplate_TemplatePart.ascx.cs" Inherits="WillCallWeb.Admin.AdminControls.MailerTemplating.MailerTemplate_TemplatePart" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ Register src="MailerTemplate_Menu.ascx" tagname="MailerTemplate_Menu" tagprefix="uc1" %>
<div id="srceditor">
    <uc1:MailerTemplate_Menu ID="MailerTemplate_Menu1" runat="server" Title="Template Containers" />
    <div id="mailertemplating">
        <asp:ValidationSummary ID="ValidationSummary2" CssClass="validationsummary" HeaderText="Please correct the following errors:" 
            ValidationGroup="mailer" runat="server" />
        <div class="jqhead rounded">
            <h3 class="entry-title"><asp:CustomValidator ID="CustomValidation" runat="server" ValidationGroup="mailer" CssClass="validator" Display="Static" 
                ErrorMessage="CustomValidator">*</asp:CustomValidator><%=Atx.CurrentMailerTemplate.Name %></h3>
        </div>
        <div class="jqpnl rounded">
            <asp:GridView ID="GridList" Width="100%" EnableViewState="false" runat="server" AllowPaging="False" AutoGenerateColumns="False"
                DataKeyNames="Id" DataSourceID="SqlMailerTemplateContentList" CssClass="lsttbl" 
                OnRowCommand="GridList_RowCommand" 
                OnRowDeleting="GridList_RowDeleting"
                OnDataBound="GridList_DataBound" >
                <RowStyle HorizontalAlign="center" />
                <SelectedRowStyle CssClass="selected" />
                <EmptyDataTemplate>
                    <div class="lstempty">No Content Specified</div>
                </EmptyDataTemplate>
                <Columns>
                    <asp:TemplateField ItemStyle-Wrap="false">
                        <ItemTemplate>
                            <asp:LinkButton Width="20px" Id="btnSelect" ToolTip="Select" CssClass="btnselect" runat="server" CommandName="Select" 
                                CommandArgument='<%#Eval("Id") %>' CausesValidation="false" ImageUrl="/Images/spacer.gif" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Name" HeaderText="Name" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="vcTemplateAsset" HeaderText="Asset" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="Title" HeaderText="Title" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"
                        ItemStyle-Width="100%" />
                    <asp:TemplateField ItemStyle-Wrap="false" >
                        <ItemTemplate>
                            <asp:LinkButton Width="20px" Id="btnUp" ToolTip="Move Up" CssClass="btnup" runat="server" CommandName="Up" 
                                CommandArgument='<%#Eval("Id") %>' CausesValidation="false" />
                            <asp:LinkButton Width="20px" Id="btnDown" ToolTip="Move Down" CssClass="btndown" runat="server" CommandName="Down" 
                                CommandArgument='<%#Eval("Id") %>' CausesValidation="false" />
                            <asp:LinkButton Width="20px" Id="btnDelete" CssClass="btndelete lastinrow" runat="server" CommandName="Delete" 
                                CommandArgument='<%#Eval("Id") %>' ToolTip="Delete" CausesValidation="false"
                                OnClientClick='return confirm("Are you sure you want to delete this row?")' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        <div class="jqpnl rounded">
            <asp:FormView ID="FormEntity1" runat="server" DataKeyNames="Id" Width="100%" DefaultMode="Edit" DataSourceID="SqlEntity1" 
                OnModeChanging="FormEntity1_ModeChanging" 
                OnItemUpdating="FormEntity1_ItemUpdating" 
                OnItemUpdated="FormEntity1_ItemUpdated" 
                OnItemInserting="FormEntity1_Inserting" 
                OnItemInserted="FormEntity1_Inserted">
                <EmptyDataTemplate>
                    <div class="cmdsection">
                        <asp:Button ID="btnNew" runat="server" CssClass="btnmed" CommandName="New" CausesValidation="false" Text="New" />
                    </div>
                </EmptyDataTemplate>
                <EditItemTemplate>
                    <div class="cmdsection">
                        <asp:Button ID="btnSave" runat="server" CssClass="btnmed" CommandName="Update" CausesValidation="true" Text="Save" />
                        <asp:Button ID="btnCancel" runat="server" CssClass="btnmed" CommandName="Cancel" CausesValidation="false" Text="Cancel" />
                        <asp:Button ID="btnNew" runat="server" CssClass="btnmed" CommandName="New" CausesValidation="false" Text="New" />
                    </div>
                    <table border="0" cellspacing="0" cellpadding="0" width="100%" class="edittabl">
                        <tr>
                            <td>
                                <asp:RequiredFieldValidator ID="RequiredName" runat="server" ValidationGroup="mailer" ErrorMessage="Name is required"
                                    Display="static" CssClass="validator" ControlToValidate="txtName">*</asp:RequiredFieldValidator>
                            </td>
                            <th>Name</th>
                            <td style="width:50%;">
                                <asp:TextBox ID="txtName" runat="server" Text='<%#Bind("Name") %>' MaxLength="256" Width="50%" />
                            </td>
                            <th>Assets</th>
                            <td colspan="2" style="width:50%;">
                                <asp:DropDownList ID="ddlAsset" runat="server" Width="50%" ondatabinding="ddlAsset_DataBinding" 
                                    OnDataBound="ddlAsset_DataBound" />
                            </td>
                        </tr>
                        <tr>
                            <th colspan="2">Title</th>
                            <td><asp:TextBox ID="txtTitle" runat="server" Text='<%#Bind("Title") %>' MaxLength="256" Width="325px" /></td>
                            <th>MaxListings</th>
                            <td><asp:Label ID="lblMaxItems" runat="server" /></td>
                            <td><asp:TextBox ID="txtMaxItems" runat="server" Text='<%#Bind("iMaxListItems") %>' />                                        
                                <cc1:SliderExtender ID="SliderExtender2" runat="server"
                                    TargetControlID="txtMaxItems" BoundControlID="lblMaxItems"
                                    Orientation="Horizontal" Minimum="0" Maximum="200" RailCssClass="ajax__slider_h_rail" HandleCssClass="ajax__slider_h_handle"
                                    TooltipText="The max number of items to choose from is {0}"/>
                             </td>
                        </tr>
                        <tr>
                            <th colspan="2">Flags</th>
                            <td><asp:TextBox ID="txtFlags" runat="server" Text='<%#Bind("vcFlags") %>' MaxLength="500" Width="325px" /></td>
                            <th>MaxSelections</th>
                            <td><asp:Label ID="lblMaxSelections" runat="server" /></td>
                            <td><asp:TextBox ID="txtMaxSelections" runat="server" Text='<%#Bind("iMaxSelections") %>' />                                        
                                <cc1:SliderExtender ID="SliderExtender1" runat="server"
                                    TargetControlID="txtMaxSelections" BoundControlID="lblMaxSelections"
                                    Orientation="Horizontal" Minimum="0" Maximum="200" RailCssClass="ajax__slider_h_rail" HandleCssClass="ajax__slider_h_handle"
                                    TooltipText="The max number of selections is {0}" />
                            </td>
                        </tr>
                        <tr>
                            <th colspan="2" style="vertical-align:top !important;">Template</th>
                            <td colspan="4">
                                <asp:TextBox ID="txtTemplate" runat="server" Font-Size="11px" Text='<%#Bind("Template") %>' TextMode="MultiLine" 
                                    MaxLength="3250" Width="100%" Height="350px" /></td>
                        </tr>
                        <tr>
                            <th colspan="2" style="vertical-align:top !important;">Separator</th>
                            <td colspan="4">
                                <asp:TextBox ID="txtSeparator" runat="server" Font-Size="11px" Text='<%#Bind("SeparatorTemplate") %>' TextMode="MultiLine" 
                                    MaxLength="500" Width="100%" Height="50px" /></td>
                        </tr>
                    </table>
                </EditItemTemplate>
                <InsertItemTemplate>
                    <div class="cmdsection">
                        <asp:Button ID="btnSave" runat="server" CssClass="btnmed" CommandName="Insert" CausesValidation="true" Text="Save" />
                        <asp:Button ID="btnCancel" runat="server" CssClass="btnmed" CommandName="Cancel" CausesValidation="false" Text="Cancel" />
                    </div>
                    <table border="0" cellspacing="4" cellpadding="6" width="100%" class="edittabl">
                        <tr>
                            <td>
                                <asp:RequiredFieldValidator ID="RequiredName" runat="server" ValidationGroup="mailer" ErrorMessage="Name is required"
                                    Display="static" CssClass="validator" ControlToValidate="txtName">*</asp:RequiredFieldValidator>
                            </td>
                            <th>Name</th>
                            <td style="width:100%;"><asp:TextBox ID="txtName" runat="server" Text='<%#Bind("Name") %>' MaxLength="256" Width="100%" /></td>
                        </tr>
                        <tr>
                            <th colspan="2">Title</th>
                            <td><asp:TextBox ID="txtTitle" runat="server" Text='<%#Bind("Title") %>' MaxLength="256" Width="100%" /></td>
                        </tr>
                        <tr>
                            <th colspan="2">Assets</th>
                            <td><asp:DropDownList ID="ddlAsset" runat="server" Width="100%" ondatabinding="ddlAsset_DataBinding" 
                                    OnDataBound="ddlAsset_DataBound" /></td>
                        </tr>
                    </table>
                </InsertItemTemplate>
            </asp:FormView>
        </div>
    </div>
</div>
<asp:SqlDataSource ID="SqlEntity1" runat="server" EnableCaching="false" ConnectionString="<%$ ConnectionStrings:WillCallConnectionString %>"
    SelectCommand="SELECT * FROM [MailerTemplateContent] WHERE [Id] = @Id; "
    UpdateCommand="UPDATE [MailerTemplateContent] SET [vcTemplateAsset] = @TemplateAsset, [Name] = @Name, [Title] = @Title, [Template] = @Template, [SeparatorTemplate] = @SeparatorTemplate, 
        [iMaxListItems] = @iMaxListItems, [iMaxSelections] = @iMaxSelections, [vcFlags] = @vcFlags  WHERE [Id] = @Id"
    InsertCommand="INSERT [MailerTemplateContent] ([tMailerTemplateId], [iDisplayOrder], [vcTemplateAsset], [Name], [Title]) VALUES (@tMailerTemplateId, @iDisplayOrder, @TemplateAsset, @Name, @Title); "
    >
    <SelectParameters>
        <asp:ControlParameter ControlID="GridList" Name="Id" PropertyName="SelectedValue" Type="Int32" />
    </SelectParameters>
    <UpdateParameters>
        <asp:ControlParameter ControlID="GridList" Name="Id" PropertyName="SelectedValue" Type="Int32" />
        <asp:Parameter Name="TemplateAsset" Type="String" ConvertEmptyStringToNull="false" />
    </UpdateParameters>
    <InsertParameters>
        <asp:Parameter Name="tMailerTemplateIdId" Type="Int32" DefaultValue="0" />
        <asp:Parameter Name="iDisplayOrder" Type="Int32" DefaultValue="0" />
        <asp:Parameter Name="TemplateAsset" Type="String" ConvertEmptyStringToNull="false" />
    </InsertParameters>
</asp:SqlDataSource>
<asp:SqlDataSource ID="SqlMailerTemplateContentList" runat="server" EnableCaching="false" ConnectionString="<%$ ConnectionStrings:WillCallConnectionString %>"
    DeleteCommand="DELETE FROM [MailerTemplateContent] WHERE [Id] = @Id "
    SelectCommand="SELECT mtc.[Id], mtc.[vcTemplateAsset], mtc.[Name], mtc.[Title] FROM [MailerTemplateContent] mtc WHERE mtc.[tMailerTemplateId] = @templateId ORDER BY mtc.[iDisplayOrder]; " 
    OnSelecting="SqlMailerTemplateContentList_Selecting">
    <DeleteParameters>
    </DeleteParameters>
    <SelectParameters>
        <asp:Parameter Name="templateId" Type="Int32" />
    </SelectParameters>
</asp:SqlDataSource>