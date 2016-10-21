<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Merch_Over18.ascx.cs" Inherits="WillCallWeb.Admin.AdminControls.Merch_Over18" %>

<div class="jqhead rounded">
<div id="fetord-items">
    <div id="lmt-display">        
        <table border="0" cellpadding="0" cellspacing="0">
            <tr class="hdf">
                <td colspan="2">
                    <h3>Merch Items That Require 18+ Acknowledgement</h3>
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr><td colspan="3"><asp:ValidationSummary ID="ValidationSummary1" CssClass="validationsummary" HeaderText="" ValidationGroup="over18" runat="server" /></td></tr>
            <tr>
                <td colspan="3">
                    <asp:DropDownList ID="ddlParentList" runat="server" Width="100%" AppendDataBoundItems="false" OnDataBound="ddlParentList_DataBound" 
                        AutoPostBack="false" DataSourceID="SqlParentList" DataTextField="ParentName" DataValueField="ParentId" 
                        EnableViewState="false">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:Button ID="btnAddSelection" runat="server" Text="Add Selection" cssclass="btnmed" Width="80px"
                        onclick="btnAddSelection_Click" CausesValidation="false" EnableViewState="false" />
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:GridView Width="100%" ID="GridView1" runat="server" DataKeyNames="Id" AutoGenerateColumns="False" CssClass="lsttbl" 
                        OnDataBinding="GridView1_DataBinding" 
                        OnRowDataBound="GridView1_RowDataBound" 
                        OnRowDeleting="GridView1_RowDeleting">
                        <SelectedRowStyle CssClass="selected" />
                        <Columns>                            
                            <asp:TemplateField ItemStyle-Wrap="false" >
                               <ItemTemplate>
                                   <asp:LinkButton Width="20px" Id="btnDelete" CssClass="btndelete" runat="server" CommandName="Delete" ToolTip="Delete" 
                                       Text="Delete" CommandArgument='<%#Eval("Id") %>' CausesValidation="false"
                                       OnClientClick='return confirm("Are you sure you want to delete this row?")' />
                                <asp:CustomValidator ID="RowValidator" Display="static" runat="server" ValidationGroup="over18" CssClass="validator">*</asp:CustomValidator>
                               </ItemTemplate>
                           </asp:TemplateField>
                           <asp:BoundField DataField="Name" HeaderText="Name" HeaderStyle-HorizontalAlign="left" HeaderStyle-Width="100%" />
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </div>
</div>
</div>

<asp:SqlDataSource ID="SqlParentList" runat="server" ConnectionString="<%$ ConnectionStrings:WillCallConnectionString %>"
    SelectCommand="
        IF EXISTS (SELECT * FROM [Merch] WHERE [tParentListing] IS NULL) BEGIN
        SELECT m.[Name] as 'ParentName', m.[Id] as 'ParentId' 
        FROM Merch m 
        WHERE m.[ApplicationId] = @appId AND m.[tParentListing] IS NULL
        ORDER BY m.[Name] ASC END " OnSelecting="SqlParentList_Selecting">
     <SelectParameters>
        <asp:Parameter Name="appId" DbType="Guid" />
    </SelectParameters>   
</asp:SqlDataSource>
