<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Charitable_Listings.ascx.cs" Inherits="WillCallWeb.Admin.AdminControls.Charitable_Listings" EnableViewState="false" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="Editor_CharitableOrg.ascx" TagName="Editor_CharitableOrg" TagPrefix="uc1" %>
<div id="srceditor">
    <div id="charitablegglisting">
        <asp:ValidationSummary ID="ValidationSummary2" CssClass="validationsummary" HeaderText="" ValidationGroup="srceditor" runat="server" />
        <div class="jqhead rounded"><h3 class="entry-title">CHARITABLE LISTINGS EDITOR</h3></div>
        <div class="jqhead rounded">
            <asp:GridView ID="GridViewEntity" Width="100%" DataSourceID="SqlCharitableListings" runat="server" AutoGenerateColumns="False" 
                DataKeyNames="Id" ShowFooter="False" CssClass="lsttbl"  
                OnRowDataBound="GridViewEntity_RowDataBound" 
                OnDataBound="GridViewEntity_DataBound" 
                OnRowCommand="GridViewEntity_RowCommand" 
                OnRowDeleting="GridViewEntity_RowDeleting"  >
                <SelectedRowStyle CssClass="selected" />
                <Columns>
                    <asp:TemplateField ItemStyle-Wrap="false">
                        <ItemTemplate>
                            <asp:LinkButton Width="20px" Id="btnSelect" ToolTip="Select" CssClass="btnselect" runat="server" CommandName="Select" 
                                CommandArgument='<%#Eval("Id") %>' CausesValidation="false" />
                        </ItemTemplate>
                    </asp:TemplateField> 
                    <asp:BoundField DataField="Name" HeaderText="Name" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="100%" />
                    <asp:CheckBoxField DataField="bActive" HeaderText="Active" ReadOnly="true" HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="Center" />
                    <asp:CheckBoxField DataField="bAvailableForContribution" HeaderText="Allow $$" ReadOnly="true" HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="Center" />
                    <asp:CheckBoxField DataField="bTopBilling" HeaderText="Featured" ReadOnly="true" HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="Center" />
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
           
            <asp:FormView Width="100%" ID="FormView1" runat="server" DefaultMode="Edit" DataSourceID="SqlEnt" DataKeyNames="Id,tCharitableOrgId" 
                OnModeChanged="FormView1_ModeChanged"
                OnDataBinding="FormView1_DataBinding"
                OnDataBound="FormView1_DataBound" 
                OnItemInserting="FormView1_ItemInserting" 
                OnItemCommand="FormView1_ItemCommand" >
                <EmptyDataTemplate>
                    <div class="cmdsection">
                        <asp:Button Id="btnNew" CausesValidation="false" CssClass="btntny" runat="server" 
                            CommandName="New" Text="Add Listing" />
                    </div>
                </EmptyDataTemplate>
                <EditItemTemplate>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" class="edittabl" >
                        <tr>
                            <td class="listing-row">
                                Active
                                <asp:CheckBox ID="chkActive" runat="server" Checked='<%#Bind("bActive") %>' />
                                Accept Contributions
                                <asp:CheckBox ID="chkContribute" runat="server" Checked='<%#Bind("bAvailableForContribution") %>' />
                                Top Billing
                                <asp:CheckBox ID="chkTopBilling" runat="server" Checked='<%#Bind("bTopBilling") %>' />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <uc1:Editor_CharitableOrg ID="Editor_CharitableOrg1" DisplayTitle="false" AbbreviatedDisplay="true" AllowSelect="false" 
                                    TitleText="SELECTED ORG" runat="server" />        
                            </td>
                        </tr>
                    </table>
                    <div class="cmdsection">
                        <asp:Button Id="btnSave" CssClass="btnmed" CausesValidation="false" runat="server" CommandName="Update" Text="Save Listing" />
                        <asp:Button Id="btnCancel" CssClass="btnmed" CausesValidation="false" runat="server" CommandName="Cancel" Text="Cancel" />
                        <asp:Button Id="btnNew" CssClass="btnmed" CausesValidation="false" runat="server" CommandName="New" Text="Add New Listing" />
                    </div>
                </EditItemTemplate>
                <InsertItemTemplate>
                    <uc1:Editor_CharitableOrg ID="Editor_CharitableOrg1" DisplayTitle="true" AbbreviatedDisplay="false" AllowSelect="true"
                        TitleText="ADDING A CHARITABLE ORG..." runat="server" /> 
                    <div class="cmdsection">            
                        <asp:Button Id="btnInsert" CssClass="btnmed" CausesValidation="false" runat="server" CommandName="Insert" Text="Add Listing" />
                        <asp:Button Id="btnCancel" CssClass="btnmed" CausesValidation="false" runat="server" CommandName="Cancel" Text="Cancel" />
                        <asp:CustomValidator ID="CustomValidation" runat="server" ValidationGroup="srceditor" CssClass="invisible" 
                            Display="Static" ErrorMessage="CustomValidator">*</asp:CustomValidator>
                    </div>
                </InsertItemTemplate>
            </asp:FormView>
        </div>
    </div>
</div>


<asp:SqlDataSource ID="SqlCharitableListings" runat="server" ConnectionString="<%$ ConnectionStrings:WillCallConnectionString %>" 
    SelectCommand="SELECT c.*, o.[Name], o.[DisplayName], o.[bActive] FROM [CharitableListing] c, [CharitableOrg] o 
        WHERE c.[ApplicationId] = @appId AND c.[tCharitableOrgId] = o.[Id] ORDER BY c.[iDisplayOrder] "
        DeleteCommand="SELECT 0 " 
        OnSelecting="Source_Selecting" >
    <SelectParameters>
        <asp:Parameter Name="appId" DbType="Guid" />        
    </SelectParameters>
    <UpdateParameters>
        <asp:Parameter Name="bAvailableForContribution" DbType="Int32" />        
    </UpdateParameters>
</asp:SqlDataSource>
<asp:SqlDataSource ID="SqlEnt" runat="server" ConnectionString="<%$ ConnectionStrings:WillCallConnectionString %>" 
    SelectCommand="SELECT c.[Id], c.[tCharitableOrgId], c.[bAvailableForContribution], c.[bTopBilling], o.[bActive] 
        FROM [CharitableListing] c, [CharitableOrg] o 
        WHERE c.[Id] = @Idx AND c.[tCharitableOrgId] = o.[Id] "
    InsertCommand="SELECT 0 " 
    UpdateCommand="UPDATE [CharitableListing] SET [bAvailableForContribution] = @bAvailableForContribution, [bTopBilling] = @bTopBilling WHERE [Id] = @Idx; 
        UPDATE [CharitableOrg] SET [bActive] = @bActive WHERE [Id] = (SELECT [tCharitableOrgId] FROM [CharitableListing] WHERE [Id] = @Idx) ">
    <SelectParameters>
        <asp:ControlParameter ControlID="GridViewEntity" DbType="Int32" Name="Idx" PropertyName="SelectedValue" />
    </SelectParameters>
    <UpdateParameters>
        <asp:ControlParameter ControlID="GridViewEntity" DbType="Int32" Name="Idx" PropertyName="SelectedValue" />
    </UpdateParameters>
</asp:SqlDataSource>