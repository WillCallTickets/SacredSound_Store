<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MailerTemplate_MailerEdit.ascx.cs" Inherits="WillCallWeb.Admin.AdminControls.MailerTemplating.MailerTemplate_MailerEdit" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit.HTMLEditor" tagprefix="cc1" %>
<%@ Register src="MailerTemplate_Menu.ascx" tagname="MailerTemplate_Menu" tagprefix="uc1" %>
<div id="srceditor">
    <uc1:MailerTemplate_Menu ID="MailerTemplate_Menu1" runat="server" Title="Mailer Edit" />
    <div id="mailertemplating">
        <asp:ValidationSummary ID="ValidationSummary2" CssClass="validationsummary" HeaderText="Please correct the following errors:" 
            ValidationGroup="mailer" runat="server" />
        <div class="jqhead rounded">
            <h3 class="entry-title"><asp:CustomValidator ID="CustomValidation" runat="server" ValidationGroup="mailer" CssClass="validator" Display="Static" 
                ErrorMessage="CustomValidator">*</asp:CustomValidator><%=Atx.CurrentMailer.Name %></h3>
        </div>
        <div class="jqpnl rounded">
        <asp:GridView ID="GridList" Width="100%" EnableViewState="false" runat="server" AllowPaging="False" AutoGenerateColumns="False"
            DataKeyNames="Id,vcTemplateAsset,Name,Title,iMaxListItems,iMaxSelections" DataSourceID="SqlMailerTemplateContentList" 
            CssClass="lsttbl" 
            OnDataBound="GridList_DataBound">
            <SelectedRowStyle CssClass="selected" />
            <EmptyDataTemplate>
                <div class="lstempty">No Content Specified</div>
            </EmptyDataTemplate>
            <Columns>
                <asp:TemplateField ItemStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:LinkButton Width="20px" Id="btnSelect" ToolTip="Select" CssClass="btnselect" runat="server" CommandName="Select" 
                            CommandArgument='<%#Eval("Id") %>' CausesValidation="false" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:CheckBoxField DataField="bContentActive" HeaderText="Active" ReadOnly="true" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="iMaxSelections" HeaderText="Max" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="vcTemplateAsset" HeaderText="Assets" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="Name" HeaderText="Name" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="Title" HeaderText="Title" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="100%" />
            </Columns>
        </asp:GridView>
        </div>
        <div class="jqpnl rounded" style="margin-top:2px;">
            <asp:FormView ID="FormEntity1" runat="server" DataKeyNames="Id" Width="100%" DefaultMode="Edit" DataSourceID="SqlEntity1" 
                OnItemCommand="FormEntity1_ItemCommand"
                OnModeChanging="FormEntity1_ModeChanging" 
                OnItemUpdated="FormEntity1_ItemUpdated" 
                OnDataBound="FormEntity1_DataBound" 
                OnItemUpdating="FormEntity1_Updating" >
                <EmptyDataTemplate>
                    <div class="lstempty">No Mailer Content</div>
                </EmptyDataTemplate>
                <EditItemTemplate>
                    <div class="cmdsection">
                        <asp:Button ID="btnSave1" runat="server" CssClass="btnmed" CommandName="Update" 
                            CommandArgument='<%#Eval("Id")%>' CausesValidation="true" Text="Save All" />
                        <asp:Button ID="btnCancel1" runat="server" CssClass="btnmed" CommandName="Cancel" 
                            CausesValidation="false" Text="Cancel" />
                    </div>
                    <table border="0" cellspacing="0" cellpadding="0" width="900px" class="edittabl">
                        <tr>
                            <th>IsActive</th>
                            <td><asp:CheckBox ID="chkActive" runat="server" Checked='<%#Bind("bActive") %>' Text="" /></td>
                            <th>ID</th>
                            <td style="width:100%;"><asp:Label ID="lblId" runat=server Text='<%#Eval("Id")%>' /></td>
                        </tr>
                        <tr>
                            <th><a href="javascript: alert('If left blank, the section will not display any section title. HTML tags are ok. 500 chars max.')" class="infomark">?</a>
                                Title</th>
                            <th colspan="3">
                                <asp:TextBox ID="txtTitle" runat="server" MaxLength="500" Width="100%" Text='<%#Bind("vcTitle") %>' /></th>
                        </tr>
                        <tr>
                            <th>Tags Used</th>
                            <td colspan="3"><asp:Literal ID="litTagsUsed" runat="server" />&nbsp;</td>
                        </tr>
                    </table>
                    <table border="0" cellspacing="0" cellpadding="0" width="100%" class="hedtbl">
                        <tr>
                            <td>
                                <asp:Panel ID="pnlShowList" runat="server" >
                                    <div class="cmdsection">
                                        <asp:Button ID="btnAllOn" CssClass="btnmed" runat="server" Text="Select All" CommandName="AllOn" 
                                            CausesValidation="false" />
                                        <asp:Button ID="btnAllOff" CssClass="btnmed" runat="server" Text="Select None" CommandName="AllOff" 
                                            CausesValidation="false" />
                                        <asp:Button ID="btnRefresh" runat="server" CssClass="btnmed" CommandName="Refresh" 
                                            CausesValidation="false" Text="Refresh Events" 
                                            OnClientClick='return confirm("Are you sure you want to delete all of the events for this mailer section?")' />
                                    </div>
                                    <asp:GridView ID="GridEvent" Width="100%" EnableViewState="true" runat="server" AllowPaging="False" CssClass="lsttbl"
                                        AutoGenerateColumns="false" gridlines="Both" ShowHeader="true" DataKeyNames="Id" CellPadding="0" CellSpacing="0"
                                        OnDataBinding="GridEvent_DataBinding" 
                                        OnRowDataBound="GridEvent_RowDataBound"
                                        OnRowCommand="GridEvent_RowCommand">
                                          <Columns>
                                            <asp:TemplateField ItemStyle-Wrap="false" >
                                                <ItemTemplate>
                                                    <asp:LinkButton Width="20px" Id="btnUp" ToolTip="Move Up" CssClass="btnup" runat="server" CommandName="Up" 
                                                        CommandArgument='<%#Eval("Id") %>' CausesValidation="false" />
                                                    <asp:LinkButton Width="20px" Id="btnDown" ToolTip="Move Down" CssClass="btndown" runat="server" CommandName="Down" 
                                                        CommandArgument='<%#Eval("Id") %>' CausesValidation="false" />
                                                    <asp:LinkButton Width="20px" Id="btnDelete" CssClass="btndelete lastinrow" runat="server" CommandName="Delete" 
                                                        CommandArgument='<%#Eval("Id") %>' ToolTip="Delete" CausesValidation="false"
                                                        OnClientClick='return confirm("Are you sure you want to delete this row?")' />
                                                    <asp:Button ID="btnAddEvent" CssClass="btnmed" runat="server" CommandName="AddEvent" 
                                                        CommandArgument='<%#Eval("Id") %>' Text="Add" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <HeaderTemplate>Use</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkActive" runat="server" Checked='<%#Bind("IsActive") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="Date">
                                                <HeaderTemplate>Date</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtDate" runat="server" Text='<%#Bind("DateString") %>' Font-Size="9px" Width="150px" MaxLength="500" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="Status">
                                                <HeaderTemplate>Status</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtStatus" runat="server" Text='<%#Bind("Status") %>' Font-Size="9px" Width="100px" MaxLength="500" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="ShowTitle">
                                                <HeaderTemplate>ShowTitle</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtShowTitle" runat="server" Text='<%#Bind("ShowTitle") %>' Font-Size="9px" Width="100px" MaxLength="500" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="Promoter">
                                                <HeaderTemplate>Promoter</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtPromoter" runat="server" Text='<%#Bind("Promoter") %>' Font-Size="9px" Width="100px" MaxLength="500" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="Header">
                                                <HeaderTemplate>Header</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtHeader" runat="server" Text='<%#Bind("Header") %>' Font-Size="9px" Width="100px" MaxLength="256" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="Headliner">
                                                <HeaderTemplate>Headliner</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtHeadliner" runat="server" Text='<%#Bind("Headliner") %>' Font-Size="9px" Width="250px" MaxLength="2000" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="Opener">
                                                <HeaderTemplate>Opener</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtOpener" runat="server" Text='<%#Bind("Opener") %>' Font-Size="9px" Width="250px" MaxLength="1000" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="Venue">
                                                <HeaderTemplate>Venue</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtVenue" runat="server" Text='<%#Bind("Venue") %>' Font-Size="9px" Width="120px" MaxLength="500" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="Times">
                                                <HeaderTemplate>Times</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtTimes" runat="server" Text='<%#Bind("Times") %>' Font-Size="9px" Width="120px" MaxLength="500" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="Ages">
                                                <HeaderTemplate>Ages</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtAges" runat="server" Text='<%#Bind("Ages") %>' Font-Size="9px" Width="120px" MaxLength="500" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="Pricing">
                                                <HeaderTemplate>Pricing</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtPricing" runat="server" Text='<%#Bind("Pricing") %>' Font-Size="9px" Width="120px" MaxLength="500" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="Url">
                                                <HeaderTemplate>Url</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtUrl" runat="server" Text='<%#Bind("Url") %>' Font-Size="9px" Width="50px" MaxLength="256" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField AccessibleHeaderText="ShowDisplayUrl">
                                                <HeaderTemplate>ShowDisplayUrl</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtImage" runat="server" Text='<%#Bind("ImageUrl") %>' Font-Size="9px" Width="250px" MaxLength="256" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                          </Columns>
                                    </asp:GridView>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:GridView ID="GridShow" Width="100%" EnableViewState="true" runat="server" AllowPaging="False" 
                                    AutoGenerateColumns="false" gridlines="Both" ShowHeader="true" CssClass="lsttbl"
                                    OnDataBinding="GridShow_DataBinding" 
                                    OnRowDataBound="GridShow_RowDataBound" 
                                    OnDataBound="GridShow_DataBound"
                                    OnRowUpdating="GridShow_RowUpdating" 
                                    OnRowCommand="GridShow_RowCommand" 
                                    OnRowDeleting="GridShow_RowDeleting"
                                    OnRowEditing="GridShow_RowEditing" 
                                    OnRowCancelingEdit="GridShow_RowCancelingEdit"  >
                                    <SelectedRowStyle CssClass="selected" />
                                    <EmptyDataTemplate>
                                        <div class="cmdsection">
                                            Show Editor&nbsp;
                                            <asp:Button ID="btnNew" CssClass="btnmed" runat="server" CommandName="New" Text="Add New" />
                                        </div>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField ItemStyle-Width="20%">                                       
                                            <HeaderTemplate>
                                                Show Editor&nbsp;
                                                <asp:Button ID="btnNew" CssClass="btnmed" runat="server" CommandName="New" Text="Add New" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:LinkButton Width="20px" Id="btnSelect" ToolTip="Select" CssClass="btnselect" runat="server" CommandName="Select" 
                                                    CommandArgument='<%#Eval("IDX") %>' CausesValidation="false" />
                                                <asp:LinkButton Width="20px" Id="btnUp" ToolTip="Move Up" CssClass="btnup" runat="server" CommandName="Up" 
                                                    CommandArgument='<%#Eval("IDX") %>' CausesValidation="false" />
                                                <asp:LinkButton Width="20px" Id="btnDown" ToolTip="Move Down" CssClass="btndown" runat="server" CommandName="Down" 
                                                    CommandArgument='<%#Eval("IDX") %>' CausesValidation="false" />
                                                <asp:LinkButton Width="20px" Id="btnDelete" CssClass="btndelete lastinrow" runat="server" CommandName="Delete" 
                                                    CommandArgument='<%#Eval("IDX") %>' ToolTip="Delete" CausesValidation="false"
                                                    OnClientClick='return confirm("Are you sure you want to delete this row?")' />
                                            </ItemTemplate>                                        
                                            <EditItemTemplate>
                                                <div class="cmdsection">
                                                    <asp:Button ID="btnSave" CssClass="btnmed" runat="server" CommandName="Update" 
                                                        CommandArgument='<%#Eval("IDX") %>' Text="Save" />
                                                    <asp:Button ID="btnCancel" CssClass="btnmed" runat="server" CommandName="Cancel" 
                                                        CommandArgument='<%#Eval("IDX") %>' Text="Cancel" />
                                                </div>
                                                <table width="100%" border="0" cellpadding="0" cellspacing="0" class="lsttbl" >
                                                    <tr>
                                                        <th>Date</th>
                                                        <td style="width:100%;">
                                                            <asp:TextBox ID="txtDate" runat="server" Text='<%#Bind("DATE")%>' Width="100%" MaxLength="256" /></td>
                                                    </tr>
                                                    <tr>
                                                        <th>Status</th>
                                                        <td><asp:TextBox ID="txtStatus" runat="server" Text='<%#Bind("STATUS")%>' Width="100%" MaxLength="256" /></td>
                                                    </tr>
                                                    <tr>
                                                        <th>Headliner</th>
                                                        <td><asp:TextBox ID="txtHeadliner" runat="server" Text='<%#Bind("HEADLINER")%>' Width="100%" MaxLength="500" /></td>
                                                    </tr>
                                                    <tr>
                                                        <th>Opener</th>
                                                        <td><asp:TextBox ID="txtOpener" runat="server" Text='<%#Bind("OPENER")%>' Width="100%" MaxLength="500" /></td>
                                                    </tr>
                                                    <tr>
                                                        <th>Venue</th>
                                                        <td><asp:TextBox ID="txtVenue" runat="server" Text='<%#Bind("VENUE")%>' Width="100%" MaxLength="256" /></td>
                                                    </tr>
                                                </table>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Date" ControlStyle-Width="15%" AccessibleHeaderText="Date">
                                            <ItemTemplate><%#Eval("DATE")%></ItemTemplate>
                                            <EditItemTemplate></EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Status" ControlStyle-Width="15%" AccessibleHeaderText="Status">
                                            <ItemTemplate><%#Eval("STATUS")%></ItemTemplate>
                                            <EditItemTemplate></EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Headliner" ControlStyle-Width="15%" AccessibleHeaderText="Headliner">
                                            <ItemTemplate><%#Eval("HEADLINER")%></ItemTemplate>
                                            <EditItemTemplate></EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Opener" ControlStyle-Width="15%" AccessibleHeaderText="Opener">
                                            <ItemTemplate><%#Eval("OPENER")%></ItemTemplate>
                                            <EditItemTemplate></EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Venue" ControlStyle-Width="15%" AccessibleHeaderText="Venue">
                                            <ItemTemplate><%#Eval("VENUE")%></ItemTemplate>
                                            <EditItemTemplate></EditItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td style="font-weight:normal;">
                                <div class="subtitle">
                                    <div class="cmdsection" style="margin-left:0;">
                                        <asp:Button ID="btnWys" CausesValidation="false" runat="server" CommandName="Wys" Text="Edit Content" Width="120px" 
                                            CssClass="btnmed ov-trigger" Tooltip="/Admin/AdminControls/Wysiwyg/Wysiwyg.aspx?context=med&ctrl=" rel="#overlay-wysiwyg" />
                                        <asp:Label ID="lblContent" runat="server" AssociatedControlID="litDesc" Text="Content" />
                                    </div>
                                </div>
                                <asp:Literal ID="litDesc" runat="server" OnDataBinding="litDesc_DataBinding" />
                                <br /><br /><br />
                            </td>
                        </tr>
                    </table>
                </EditItemTemplate>
            </asp:FormView>
        </div>
    </div>
</div>

<div class="admin_overlay" id="overlay-wysiwyg" style="display:none;">
	<div class="contentWrap"></div>
</div>

<script type="text/javascript" src="/JQueryUI/admin-overlay.js"></script>



<asp:SqlDataSource ID="SqlEntity1" runat="server" EnableCaching="false" ConnectionString="<%$ ConnectionStrings:WillCallConnectionString %>"
    SelectCommand="IF NOT EXISTS (SELECT * FROM [MailerContent] WHERE [tMailerId] = @tMailerId AND [tMailerTemplateContentId] = @tMailerTemplateContentId) BEGIN 
            INSERT [MailerContent] ([tMailerId], [tMailerTemplateContentId], [bActive], [vcTitle]) VALUES (@tMailerId, @tMailerTemplateContentId, @active, @title); 
        END 
        SELECT * FROM [MailerContent] WHERE [tMailerId] = @tMailerId AND [tMailerTemplateContentId] = @tMailerTemplateContentId; "
    OnSelecting="SqlEntity1_Selecting"
    UpdateCommand="UPDATE [MailerContent] SET [bActive] = @bActive, [vcTitle] = @vcTitle WHERE [Id] = @Id "
    >
    <SelectParameters>
        <asp:Parameter Name="tMailerId" Type="Int32" />
        <asp:ControlParameter ControlID="GridList" Name="tMailerTemplateContentId" PropertyName="SelectedValue" Type="Int32" />
        <asp:Parameter Name="active" Type="Boolean" DefaultValue="false" />
        <asp:Parameter Name="title" Type="String" ConvertEmptyStringToNull="true" />
    </SelectParameters>
</asp:SqlDataSource>
<asp:SqlDataSource ID="SqlMailerTemplateContentList" runat="server" EnableCaching="false" ConnectionString="<%$ ConnectionStrings:WillCallConnectionString %>"
    SelectCommand="SELECT mtc.[Id], ISNULL(mc.[bActive], 0) as 'bContentActive', mtc.[vcTemplateAsset], mtc.[Name], mtc.[Title], mtc.[vcFlags], 
        ISNULL(mtc.[iMaxListItems], 0) as 'iMaxListItems', ISNULL(mtc.[iMaxSelections],0) as 'iMaxSelections' 
        FROM [MailerTemplateContent] mtc LEFT OUTER JOIN [MailerContent] mc ON mc.[tMailerId] = @mailerId AND mc.[tMailerTemplateContentId] = mtc.[Id] 
        WHERE mtc.[tMailerTemplateId] = @templateId ORDER BY mtc.[iDisplayOrder]; " 
    OnSelecting="SqlMailerTemplateContentList_Selecting">
    <SelectParameters>
        <asp:Parameter Name="templateId" Type="Int32" />
        <asp:Parameter Name="mailerId" Type="Int32" />
    </SelectParameters>
</asp:SqlDataSource>