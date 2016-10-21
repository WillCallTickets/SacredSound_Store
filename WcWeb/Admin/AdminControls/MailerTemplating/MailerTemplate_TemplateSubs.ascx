<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MailerTemplate_TemplateSubs.ascx.cs" Inherits="WillCallWeb.Admin.AdminControls.MailerTemplating.MailerTemplate_TemplateSubs" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ Register src="MailerTemplate_Menu.ascx" tagname="MailerTemplate_Menu" tagprefix="uc1" %>
<div id="srceditor">
    <uc1:MailerTemplate_Menu ID="MailerTemplate_Menu1" runat="server" Title="Template Substitutions" />
    <div id="mailertemplating">
        <asp:ValidationSummary ID="ValidationSummary2" CssClass="validationsummary" HeaderText="Please correct the following errors:" 
            ValidationGroup="mailer" runat="server" />
        <div class="jqhead rounded">
            <h3 class="entry-title"><asp:CustomValidator ID="CustomValidation" runat="server" ValidationGroup="mailer" CssClass="validator" Display="Static" 
                ErrorMessage="CustomValidator">*</asp:CustomValidator><%=Atx.CurrentMailerTemplate.Name %></h3>
        </div>
        <div class="jqpnl rounded">
            <asp:DataList ID="listTemplate" runat="server" DataSourceID="SqlMailerTemplateContentList" DataKeyField="Id" 
                OnSelectedIndexChanged="listTemplate_SelectionChanged"
                RepeatDirection="Horizontal" RepeatLayout="Table" CssClass="edittabl" 
                OnItemDataBound="listTemplate_ItemDataBound">
                <SelectedItemStyle CssClass="selected" />
                <ItemTemplate>
                    <asp:Button Id="btnSelect" CssClass="btnmed" runat="server" CommandName="Select" 
                        CommandArgument='<%#Eval("Id") %>' Text='<%#Eval("Name") %>' />
                </ItemTemplate>
            </asp:DataList>
            <br />
            <asp:GridView ID="GridSubs" Width="100%" EnableViewState="true" runat="server" AllowPaging="False" CssClass="lsttbl" 
                AutoGenerateColumns="false" gridlines="Both" ShowHeader="true" DataKeyNames="Id" CellPadding="0" CellSpacing="0"
                OnDataBinding="GridSubs_DataBinding" 
                OnRowDataBound="GridSubs_RowDataBound"
                OnRowCommand="GridSubs_RowCommand" 
                OnRowDeleting="GridSubs_Deleting">
                <Columns>
                    <asp:TemplateField ItemStyle-Width="50px">
                        <ItemTemplate>
                            <asp:Button ID="btnAddNew" CssClass="btntny" runat="server" CommandName="AddNew" 
                                CommandArgument='<%#Eval("Id") %>' Text="Add" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Name" HeaderStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:TextBox ID="txtName" runat="server" Text='<%#Bind("TagName") %>' Font-Size="9px" Width="150px" MaxLength="256" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                        <HeaderTemplate>Value (***leave blank to take out of template. use {0} for replacement***)</HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="txtValue" runat="server" Text='<%#Bind("TagValue") %>' Font-Size="9px" Width="100%" MaxLength="2000" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton Width="20px" Id="btnDelete" CssClass="btndelete lastinrow" runat="server" CommandName="Delete" 
                                CommandArgument='<%#Eval("Id") %>' ToolTip="Delete" CausesValidation="false"
                                OnClientClick='return confirm("Are you sure you want to delete this row?")' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <div class="cmdsection">
                <asp:Button ID="btnSave" CssClass="btnmed" runat="server" Text="Save All" OnClick="btnSave_Click"
                    CausesValidation="false" />
                <asp:Button ID="btnCancel" CssClass="btnmed" runat="server" Text="Cancel" OnClick="btnCancel_Click"
                    CausesValidation="false" />
            </div>
            <asp:TextBox ID="txtTemplate" runat="server" TextMode="MultiLine" Height="500px" Width="100%" OnDataBinding="txtTemplate_DataBinding" ReadOnly="true" />
        </div>
    </div>
</div>
<asp:SqlDataSource ID="SqlMailerTemplateContentList" runat="server" EnableCaching="false" ConnectionString="<%$ ConnectionStrings:WillCallConnectionString %>"
    SelectCommand="SELECT mtc.[Id], mtc.[Name], mtc.[Title] FROM [MailerTemplateContent] mtc WHERE mtc.[tMailerTemplateId] = @templateId ORDER BY mtc.[iDisplayOrder]; " 
    OnSelecting="SqlMailerTemplateContentList_Selecting" OnSelected="SqlMailerTemplateContentList_Selected">
    <SelectParameters>
        <asp:Parameter Name="templateId" Type="Int32" />
    </SelectParameters>
</asp:SqlDataSource>