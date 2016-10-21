<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Settings.ascx.cs" Inherits="WillCallWeb.Admin.AdminControls.Settings" %>
<div id="setting"><asp:ValidationSummary ID="ValidationSummary1" CssClass="validationsummary" HeaderText="" ValidationGroup="editor" runat="server" />
    <div class="jqhead rounded">
        <h3 class="entry-title"><%=_context %> Settings <asp:CustomValidator ID="CustomValidation" runat="server" ValidationGroup="editor" CssClass="invisible" 
            Display="Static" ErrorMessage="CustomValidator">*</asp:CustomValidator></h3>
        <asp:GridView ID="GridView1" runat="server" Width="100%" AutoGenerateColumns="False" OnDataBinding="GridView1_DataBinding" CssClass="lsttbl" 
            OnRowCancelingEdit="GridView1_RowCancelingEdit" OnRowDataBound="GridView1_RowDataBound" OnRowEditing="GridView1_RowEditing" 
            OnRowUpdating="GridView1_RowUpdating" PageSize="10000">
            <SelectedRowStyle CssClass="selected" />
            <EmptyDataTemplate>
                <div class="lstempty">No Data For Selected Criteria</div>
            </EmptyDataTemplate>
            <Columns>
                <asp:TemplateField HeaderText="Setting Name" HeaderStyle-HorizontalAlign="Left">
                    <ItemTemplate><%#Eval("Name") %></ItemTemplate>
                    <EditItemTemplate><%#Eval("Name") %></EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Value" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="50%" ItemStyle-CssClass="setting-max-width">
                    <ItemTemplate><%#Eval("ValueX") %></ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtInput" runat="server" Width="90%" />
                        <asp:CheckBox ID="chkInput" runat="server" />
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Description" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="50%">
                    <ItemTemplate><%#Eval("Description") %></ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtDescription" runat="server" TextMode="multiline" Width="300px" Height="100px" Text='<%#Eval("Description") %>' />
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="right">
                    <ItemTemplate>
                        <asp:Button Id="btnEdit" CssClass="btntny" CausesValidation="false" CommandArgument='<%#Eval("Id") %>' runat="server" 
                            CommandName="Edit" Text="edit" OnDataBinding="btnEdit_DataBind" />
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:Button Id="btnSave" CssClass="btntny" CausesValidation="true" ValidationGroup="Editor" CommandArgument='<%#Eval("Id") %>' 
                            runat="server" CommandName="Update" Text="save" />
                        <asp:Button Id="btnCancel" CssClass="btntny" CausesValidation="false" runat="server" CommandName="Cancel" Text="cancel" />
                    </EditItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    <asp:Panel ID="pnlAdd" runat="server" Visible="false">
        <div class="jqhead rounded">
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="edittabl">
            <tr>
                <th>&nbsp;</th>
                <td><asp:Label ID="lblResult" runat="server" /></td>
            </tr>
            <tr>
                <th>Data Type</th>
                <td style="width:100%;">
                    <asp:DropDownList ID="ddlDataType" runat="server" OnDataBinding="ddlDataType_DataBinding" />
                </td>
            </tr>
            <tr>
                <th>Max Length</th>
                <td>
                    <asp:TextBox ID="txtLength" runat="server" MaxLength="6" Text="" />
                    //required
                    //must be int
                </td>
            </tr>
            <tr>
                <th>Context</th>
                <td><asp:DropDownList ID="ddlContext" runat="server" DataSourceID="Sql_Context" DataTextField="Name" DataValueField="Name" /></td>
            </tr>
            <tr>
                <th>Description</th>
                <td><asp:TextBox ID="txtDescription" runat="server" MaxLength="2000" Width="400px" Text="" TextMode="MultiLine" Height="30px" /></td>
            </tr>
            <tr>
                <th>Name</th>
                <td><asp:TextBox ID="txtName" runat="server" MaxLength="500" Width="400px" Text="" /></td>
            </tr>
            <tr>
                <th>Value</th>
                <td><asp:TextBox ID="txtValue" runat="server" MaxLength="2000" Width="400px" Text="" TextMode="MultiLine" Height="30px" /></td>
            </tr>
            <tr>
                <th>&nbsp;</th>
                <td><br />
                    <asp:Button ID="btnAddNew" runat="server" OnClick="btnAddNew_Click" Text="Add New Setting" class="btntny" />
                </td>
            </tr>
        </table>
        </div>
    </asp:Panel>
</div>

<asp:SqlDataSource ID="Sql_Context" runat="server" ConnectionString="<%$ ConnectionStrings:WillCallConnectionString %>"
    SelectCommand="SELECT DISTINCT [Context] as 'Name' FROM [SiteConfig]"></asp:SqlDataSource>
