<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Editor_MerchColor.ascx.cs" Inherits="WillCallWeb.Admin.AdminControls.Editor_MerchColor" %>
<div id="merchcoloreditor">
     <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
             <div class="jqhead rounded">
                <table border="0" cellpadding="0" cellspacing="0" width="100%" class="edittabl">
                    <tr>
                        <th>MERCH COLOR EDITOR</th>
                        <td><asp:CustomValidator ID="CustomValidation" runat="server" ValidationGroup="editor" CssClass="invisible" 
                                Display="Static" ErrorMessage="CustomValidator">*</asp:CustomValidator></td>
                        <td style="width:100%;">
                            <asp:Button Id="btnMerch" CssClass="btnmed" CausesValidation="false" runat="server" Width="100px" 
                                CommandName="Merch" Text="" OnClick="btnMerch_Click" />
                        </td>
                    </tr>
                </table>
                <asp:GridView ID="GridView1" ShowHeader="true" runat="server" AutoGenerateColumns="False" Width="100%" CssClass="lsttbl" 
                    OnDataBinding="GridView1_DataBinding" OnDataBound="GridView1_DataBound" OnRowDataBound="GridView1_RowDataBound" 
                    OnSelectedIndexChanged="GridView1_SelectedIndexChanged" OnRowDeleting="GridView1_RowDeleting" OnRowCommand="GridView1_RowCommand">
                    <SelectedRowStyle CssClass="selected" />
                    <Columns>
                        <asp:TemplateField ItemStyle-Wrap="false">
                            <ItemTemplate>
                                <asp:LinkButton Width="20px" Id="btnSelect" ToolTip="Select" CssClass="btnselect" runat="server" CommandName="Select" 
                                    Text="Select" CommandArgument='<%#Eval("Id") %>' CausesValidation="false" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Name" HeaderText="Name" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="100%" />
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
            <asp:ValidationSummary ID="ValidationSummary2" CssClass="validationsummary" HeaderText="" ValidationGroup="editor" runat="server" />           
            <asp:FormView ID="FormView1" runat="server" DefaultMode="Edit" Width="100%" OnDataBinding="FormView1_DataBinding" 
                OnDataBound="FormView1_DataBound" OnItemCommand="FormView1_ItemCommand" OnItemInserting="FormView1_ItemInserting" 
                OnItemUpdating="FormView1_ItemUpdating" OnModeChanging="FormView1_ModeChanging">
                <EmptyDataTemplate>
                    <div class="jqpnl rounded">
                        <h3 class="entry-title">Add A New Merch Color...</h3>
                        <asp:Button Id="btnNew" CssClass="btnmed" CausesValidation="false" runat="server" CommandName="New" Text="new" />
                    </div>
                </EmptyDataTemplate>
                <EditItemTemplate>
                    <div class="jqpnl rounded">
                        <h3 class="entry-title"><%#Eval("Name") %></h3>
                         <table border="0" cellpadding="0" cellspacing="3" width="100%" class="edittabl">
                            <tr>
                                <th><asp:RequiredFieldValidator ValidationGroup="editor" CssClass="validator" ID="RequiredFieldValidator1" 
                                    runat="server" Display="Static" ControlToValidate="txtName" ErrorMessage="Name is required.">*</asp:RequiredFieldValidator>
                                    Name</th>
                                <td style="width:100%;"><asp:TextBox ID="txtName" runat="server" ReadOnly="true" Width="250px" Text='<%#Eval("Name") %>' MaxLength="256" /></td>
                            </tr>
                        </table>
                        <div class="cmdsection">
                            <asp:Button Id="btnSave" CssClass="btnmed" CausesValidation="true" ValidationGroup="editor" 
                                CommandArgument='<%#Eval("Id") %>' runat="server" CommandName="Update" Text="save" />
                            <asp:CustomValidator ID="CustomValidation" runat="server" ValidationGroup="editor" CssClass="invisible" 
                                Display="Static" ErrorMessage="CustomValidator">*</asp:CustomValidator>
                            <asp:Button Id="btnCancel" CssClass="btnmed" CausesValidation="false" runat="server" 
                                CommandName="Cancel" Text="cancel" />
                            <asp:Button Id="btnNew" CssClass="btnmed" CausesValidation="false" runat="server" 
                                CommandName="New" Text="new" />
                        </div>
                    </div>
                </EditItemTemplate>
                <InsertItemTemplate>
                     <div class="jqpnl rounded">
                        <h3 class="entry-title">Adding A New Merch Color...</h3>
                         <table border="0" cellpadding="0" cellspacing="3" width="100%" class="edittabl">
                            <tr>
                                <th><asp:RequiredFieldValidator ValidationGroup="editor" CssClass="validator" ID="RequiredFieldValidator1" 
                                        runat="server" ControlToValidate="txtName" Display="Static" ErrorMessage="Name is required.">*</asp:RequiredFieldValidator>Name</th>
                                <td style="width:100%;"><asp:TextBox ID="txtName" runat="server" Width="200px" MaxLength="256" /></td>
                            </tr>
                        </table>
                        <div class="cmdsection">
                            <asp:Button Id="btnSave" CssClass="btnmed" CausesValidation="true" ValidationGroup="editor" 
                                CommandArgument='<%#Eval("Id") %>' runat="server" CommandName="Insert" Text="save" />
                            <asp:CustomValidator ID="CustomValidation" runat="server" ValidationGroup="editor" CssClass="invisible" 
                                Display="Static" ErrorMessage="CustomValidator">*</asp:CustomValidator>
                            <asp:Button Id="btnCancel" CssClass="btnmed" CausesValidation="false" runat="server" 
                                CommandName="Cancel" Text="cancel" />
                        </div>
                    </div>
                </InsertItemTemplate>
            </asp:FormView>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>


