<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Editor_Age.ascx.cs" Inherits="WillCallWeb.Admin.AdminControls.Editor_Age" %>
<div id="ageeditor">
     <div class="jqhead rounded">
        <div class="sectitle">AGE EDITOR <asp:CustomValidator ID="CustomValidation" runat="server" ValidationGroup="editor" CssClass="invisible" 
            Display="Static" ErrorMessage="CustomValidator">*</asp:CustomValidator></div>
        <asp:GridView Width="100%" ID="GridView1" runat="server" DataKeyNames="Id" AutoGenerateColumns="False" 
            OnDataBound="GridView1_DataBound" OnDataBinding="GridView1_DataBinding" CssClass="lsttbl" 
            OnSelectedIndexChanged="GridView1_SelectedIndexChanged" OnRowDataBound="GridView1_RowDataBound" OnRowDeleting="GridView1_RowDeleting">
            <SelectedRowStyle CssClass="selected" />
            <Columns>
                <asp:TemplateField ItemStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:LinkButton Width="20px" Id="btnSelect" CssClass="btnselect" ToolTip="Select" runat="server" CommandName="Select" 
                            Text="Select" CommandArgument='<%#Eval("Id") %>' CausesValidation="false" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Name" HeaderText="Name" HeaderStyle-HorizontalAlign="left" HeaderStyle-Width="100%" />
                <asp:TemplateField ItemStyle-Wrap="false" >
                   <ItemTemplate>
                       <asp:LinkButton Width="20px" Id="btnDelete" CssClass="btndelete lastinrow" runat="server" CommandName="Delete" ToolTip="Delete" 
                            Text="Delete" CommandArgument='<%#Eval("Id") %>' CausesValidation="false"
                           OnClientClick='return confirm("Are you sure you want to delete this row?")' />
                   </ItemTemplate>
               </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    <asp:ValidationSummary ID="ValidationSummary1" CssClass="validationsummary" HeaderText="" ValidationGroup="editor" runat="server" />
    <asp:FormView ID="FormView1" Width="100%" DefaultMode="Edit" runat="server" OnItemUpdating="FormView1_ItemUpdating" 
        OnDataBinding="FormView1_DataBinding" OnModeChanging="FormView1_ModeChanging" OnItemInserting="FormView1_ItemInserting">
        <EmptyDataTemplate>
            <div class="jqhead rounded">
                <table border="0" cellpadding="0" cellspacing="0" width="100%" class="edittabl">
                    <tr>
                        <th>Add A New Age...</th>
                        <td><asp:Button Id="btnNew" CssClass="btnmed" CausesValidation="false" runat="server" CommandName="New" Text="new" /></td>
                    </tr>
                </table>
            </div>
        </EmptyDataTemplate>
        <EditItemTemplate>
            <div class="jqhead rounded">
                 <h3 class="entry-title"><%#Eval("Name") %></h3>
                 <table border="0" cellpadding="0" cellspacing="3" width="100%" class="edittabl">
                    <tr>
                        <th>Name</th>
                        <td style="width:100%;"><asp:TextBox Width="350px" ID="txtName" ReadOnly="true" runat="server" Text='<%#Bind("Name") %>' MaxLength="256" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" CssClass="validator" ValidationGroup="editor" 
                                ErrorMessage="Name is required" Display="Static" ControlToValidate="txtName">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                </table>
                <div class="cmdsection">
                    <asp:Button Id="btnSave" CssClass="btnmed" CausesValidation="true" ValidationGroup="editor" 
                        CommandArgument='<%#Eval("Id") %>' runat="server" CommandName="Update" Text="save" />
                    <asp:Button Id="btnCancel" CssClass="btnmed" CausesValidation="false" runat="server" CommandName="Cancel" Text="cancel" />
                    <asp:Button Id="btnNew" CssClass="btnmed" CausesValidation="false" runat="server" CommandName="New" Text="new" />
                </div>
            </div>
        </EditItemTemplate>
        <InsertItemTemplate>
            <div class="jqhead rounded">
                <h3 class="entry-title">Adding A New Age...</h3>
                <table border="0" cellpadding="0" cellspacing="3" width="100%" class="edittabl">
                    <tr>
                        <th>Name</th>
                        <td style="width:100%;"><asp:TextBox Width="350px" ID="txtName" runat="server" Text='<%#Bind("Name") %>' MaxLength="256" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" CssClass="validator" ValidationGroup="editor" 
                                ErrorMessage="Name is required" Display="Static" ControlToValidate="txtName">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>                                
                </table>
                <div class="cmdsection">
                    <asp:Button Id="btnSave" CssClass="btnmed" CausesValidation="true" ValidationGroup="editor" 
                        runat="server" CommandName="Insert" Text="save" />
                    <asp:Button Id="btnCancel" CssClass="btnmed" CausesValidation="false" runat="server" CommandName="Cancel" Text="cancel" />
                </div>
            </div>
        </InsertItemTemplate>
    </asp:FormView>
</div>


