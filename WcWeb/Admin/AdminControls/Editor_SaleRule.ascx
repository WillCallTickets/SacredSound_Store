<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Editor_SaleRule.ascx.cs" Inherits="WillCallWeb.Admin.AdminControls.Editor_SaleRule" %>
<div id="saleruleeditor">
     <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="jqhead rounded">
                <h3 class="entry-title">SALE RULE EDITOR
                    <asp:CustomValidator ID="CustomValidation" runat="server" ValidationGroup="editor" CssClass="invisible" 
                        Display="Static" ErrorMessage="CustomValidator">*</asp:CustomValidator>
                </h3>
                <asp:GridView Width="100%" ID="GridView1" runat="server" DataKeyNames="Name" AutoGenerateColumns="False" 
                    OnDataBound="GridView1_DataBound" OnDataBinding="GridView1_DataBinding" CssClass="lsttbl"
                    OnSelectedIndexChanged="GridView1_SelectedIndexChanged" OnRowDataBound="GridView1_RowDataBound" 
                    OnRowCommand="GridView1_RowCommand" OnRowDeleting="GridView1_RowDeleting">
                    <SelectedRowStyle CssClass="selected" />
                    <Columns>
                        <asp:TemplateField ItemStyle-Wrap="false">
                            <ItemTemplate>
                                <asp:LinkButton Width="20px" Id="btnSelect" ToolTip="Select" CssClass="btnselect" runat="server" CommandName="Select" 
                                    CommandArgument='<%#Eval("Id") %>' CausesValidation="false" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:CheckBoxField DataField="IsActive" HeaderText="Active" ItemStyle-HorizontalAlign="center" />
                        <asp:BoundField DataField="Context" HeaderText="Context" HeaderStyle-HorizontalAlign="left" />
                        <asp:BoundField DataField="Name" HeaderText="Name" HeaderStyle-HorizontalAlign="left" />
                        <asp:TemplateField HeaderText="Display Text" HeaderStyle-HorizontalAlign="left">
                            <ItemTemplate>
                                <asp:Literal ID="LiteralText" runat="server"></asp:Literal>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-Wrap="false" >
                           <ItemTemplate>
                                <asp:LinkButton Width="20px" Id="btnUp" ToolTip="Move Up" CssClass="btnup" runat="server" CommandName="Up" 
                                    CommandArgument='<%#Eval("Id") %>' CausesValidation="false" />
                                <asp:LinkButton Width="20px" Id="btnDown" ToolTip="Move Down" CssClass="btndown" runat="server" CommandName="Down" 
                                    CommandArgument='<%#Eval("Id") %>' CausesValidation="true" />
                               <asp:LinkButton Width="20px" Id="btnDelete" CssClass="btndelete lastinrow" runat="server" CommandName="Delete" 
                                    CommandArgument='<%#Eval("Id") %>' ToolTip="Delete" CausesValidation="false"
                                   OnClientClick='return confirm("Are you sure you want to delete this row?")' />
                           </ItemTemplate>
                       </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <asp:ValidationSummary ID="ValidationSummary1" CssClass="validationsummary" HeaderText="" ValidationGroup="editor" runat="server" />
            <asp:FormView ID="FormView1" Width="100%" DefaultMode="Edit" runat="server" 
                OnItemUpdating="FormView1_ItemUpdating" OnDataBinding="FormView1_DataBinding" 
                OnModeChanging="FormView1_ModeChanging" 
                OnItemInserting="FormView1_ItemInserting" OnDataBound="FormView1_DataBound">
                <EmptyDataTemplate>
                    <div class="jqpnl rounded">
                        <h3 class="entry-title">>Add A New Sale Rule...</h3>
                        <asp:Button Id="btnNew" CssClass="btnmed" CausesValidation="false" runat="server" CommandName="New" Text="new" />
                    </div>
                </EmptyDataTemplate>
                <EditItemTemplate>
                    <div class="jqpnl rounded">
                        <h3 class="entry-title"><asp:Literal ID="litDescription" runat="server" /></h3>
                         <table border="0" cellpadding="0" cellspacing="3" width="100%" class="edittabl">
                            <tr>
                                <th>Is Active</th>
                                <td style="width:100%;"><asp:CheckBox ID="chkActive" runat="server" Checked='<%#Eval("IsActive") %>' /></td>
                            </tr>
                            <tr>
                                <th>Context</th>
                                <td>
                                    <asp:DropDownList ID="ddlContext" Width="350px" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <th>Name</th>
                                <td>
                                    <asp:TextBox Width="350px" ID="txtName" runat="server" Text='<%#Bind("Name") %>' MaxLength="50" />
                                </td>
                            </tr>
                            <tr>
                                <th style="vertical-align:top;"><asp:RequiredFieldValidator ID="RequiredFieldValidator1" 
                                    runat="server" CssClass="validator" ValidationGroup="editor" 
                                        ErrorMessage="Text is required" Display="Static" ControlToValidate="txtText">*</asp:RequiredFieldValidator>
                                    Display Text</th>
                                <td>
                                    <asp:TextBox ID="txtText" runat="server" TextMode="multiline" Height="200px" Width="600px" 
                                        Text='<%#Bind("DisplayText") %>' MaxLength="2000" />
                                </td>
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
                        <h3 class="entry-title">Adding A New Sale Rule...</h3>
                         <table border="0" cellpadding="0" cellspacing="3" width="100%" class="edittabl">
                            <tr>
                                <th>Context</th>
                                <td style="width:100%;"><asp:DropDownList ID="ddlContext" runat="server" Width="350px" /></td>
                            </tr>
                            <tr>
                                <th>Name</th>
                                <td><asp:TextBox Width="350px" ID="txtName" runat="server" Text='<%#Bind("Name") %>' MaxLength="50" /></td>
                            </tr>
                            <tr>
                                <th style="vertical-align: top;">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" CssClass="validator" ValidationGroup="editor" 
                                        ErrorMessage="Text is required" Display="Static" ControlToValidate="txtText">*</asp:RequiredFieldValidator>
                                    Display Text</th>
                                <td colspan="2">
                                    <asp:TextBox ID="txtText" runat="server" TextMode="multiline" Height="200px" Width="600px" 
                                        Text='<%#Bind("DisplayText") %>' MaxLength="2000" />
                                </td>
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