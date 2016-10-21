<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Mailer_Subscription.ascx.cs" 
    Inherits="WillCallWeb.Admin.AdminControls.Mailer_Subscription" %>
<div id="srceditor">
    <div id="mailersubscription">
        <div class="jqhead rounded">
            <div class="sectitle">SUBSCRIPTION MANAGER</div>
            <asp:GridView ID="GridView1" runat="server" Width="100%" AutoGenerateColumns="False" DataKeyNames="Id" CssClass="lsttbl"
                OnDataBinding="GridView1_DataBinding"
                OnRowDataBound="GridView1_RowDataBound" 
                OnDataBound="GridView1_DataBound" 
                OnSelectedIndexChanged="GridView1_SelectedIndexChanged">
                <SelectedRowStyle CssClass="selected" />
                <EmptyDataTemplate><div class="lstempty">There are currently no subscriptions</div></EmptyDataTemplate>
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton Width="20px" Id="btnSelect" CssClass="btnselect" ToolTip="Select" runat="server" CommandName="Select" 
                                Text="Select" CommandArgument='<%#Eval("Id") %>' CausesValidation="false" />        
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:CheckboxField DataField="IsDefault" HeaderText="Default" ItemStyle-HorizontalAlign="center" />
                    <asp:CheckboxField DataField="IsActive" HeaderText="Active" ItemStyle-HorizontalAlign="center" />
                    <asp:TemplateField HeaderText="Recipients" headerstyle-horizontalalign="Left">
                        <ItemTemplate>
                            <%#Eval("AspnetRoleRecord.RoleName") %>
                            <asp:Literal ID="litRole" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Name" HeaderText="Name" HeaderStyle-HorizontalAlign="left" />
                    <asp:BoundField DataField="Description" HeaderText="Description" HeaderStyle-HorizontalAlign="left" />
                </Columns>
            </asp:GridView>
        </div>
        <asp:ValidationSummary ID="ValidationSummary2" CssClass="validationsummary" HeaderText="" 
            ValidationGroup="mailer" runat="server" />
        <asp:FormView ID="FormView1" runat="server" DefaultMode="Edit" Width="100%" DataKeyNames="Id"
            OnDataBinding="FormView1_DataBinding" 
            OnDataBound="FormView1_DataBound" 
            OnItemCommand="FormView1_ItemCommand" 
            OnItemInserting="FormView1_ItemInserting" 
            OnItemUpdating="FormView1_ItemUpdating" 
            OnModeChanging="FormView1_ModeChanging" 
            OnItemDeleting="FormView1_ItemDeleting">
            <EmptyDataTemplate>
                <div class="jqhead rounded sectitle">
                    Add A New Item...
                    <asp:Button Id="btnNew" CausesValidation="false" runat="server" CommandName="New" Text="New" CssClass="btnmed" />
                </div>
            </EmptyDataTemplate>
            <EditItemTemplate>
                <div class="jqpnl rounded">
                    <h3 class="entry-title"><%#Eval("Name") %></h3>
                    <table border="0" cellspacing="0" cellpadding="0" width="100%" class="edittabl">
                        <tr>
                            <td colspan="3">
                                <div class="jqinstruction rounded">
                                    <ul>
                                        <li>To be notified of every sale (not recommended for high-volume sales!)</li>
                                        <li>Make sure the recipients are set to administrator or report viewer</li>
                                        <li>Name should use this syntax: Track_{Merch|Ticket}_{Id}</li>
                                        <li>Ex: Track_Merch_10125, Track_Ticket_11109</li>
                                        <li>Be sure to describe the item you are tracking in the description field for easier tracking.</li>
                                    </ul>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <th>Is Active</th>
                            <td>
                                <asp:CheckBox ID="chkActive" runat="server" Checked='<%#Bind("IsActive") %>' />
                                <span style="white-space: pre;">               </span>
                                <asp:Button ID="btnSetDefault" Enabled="false" CommandArgument='<%#Eval("Id") %>' CausesValidation="false" 
                                    CssClass="btnmed" Width="100px" runat="server" CommandName="default" Text="Set As Default" 
                                    OnClientClick="return confirm('Are you sure you want to make this the default subscription? This will deselect the current default subscription.')" />
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <th>Recipients</th>
                            <td style="width:50%;">
                                <asp:DropDownList ID="ddlRoles" runat="server" Width="100%" Enabled="false" 
                                    OnDataBinding="Roles_DataBinding" OnDataBound="Roles_DataBound" />
                            </td>
                            <td style="white-space:nowrap;width:50%;" class="intr">WebUser refers to the general customer</td>
                        </tr>                        
                        <tr>
                            <th><asp:RequiredFieldValidator ValidationGroup="mailer" CssClass="validator" ID="RequiredName" 
                                    runat="server" Display="Static" ControlToValidate="txtName" ErrorMessage="Name is required.">*</asp:RequiredFieldValidator>
                                Name</th>
                            <td><asp:TextBox ID="txtName" runat="server" Width="100%" MaxLength="256" Text='<%#Bind("Name") %>' /></td>
                            <td>&nbsp;</td>
                        </tr>                        
                        <tr>
                            <th style="vertical-align:top;">Description</th>
                            <td><asp:TextBox ID="txtDescription" runat="server" Width="100%" Height="60px" TextMode="MultiLine" MaxLength="500" Text='<%#Bind("Description") %>' /></td>
                            <td class="intr">This text will be shown to the customer</td>
                        </tr>
                        <tr>
                            <th style="vertical-align:top;">Notes</th>
                            <td><asp:TextBox ID="txtInternalDescription" runat="server" Width="100%" Height="40px" 
                                TextMode="MultiLine" MaxLength="2000" Text='<%#Bind("InternalDescription") %>' /></td>
                            <td>&nbsp;</td>
                        </tr>
                    </table>
                    <div class="cmdsection">
                        <asp:Button ID="btnUpdate" CausesValidation="true" CssClass="btnmed" runat="server" 
                            CommandName="Update" Text="Save" />
                        <asp:Button ID="btnCancel" CssClass="btnmed" runat="server" CommandName="Cancel" Text="cancel" />
                        <asp:Button ID="btnDelete" CssClass="btnmed" runat="server" CommandName="Delete" Text="delete" 
                            OnClientClick="return confirm('Are you sure you want to delete this subscription?')" />
                        <asp:Button ID="btnNew" CssClass="btnmed" runat="server" CommandName="New" Text="new" />
                        <asp:CustomValidator ID="CustomValidation" runat="server" ValidationGroup="mailer" CssClass="validator" 
                            Display="Static" ErrorMessage="CustomValidator">*</asp:CustomValidator>
                    </div>
                </div>
            </EditItemTemplate>
            <InsertItemTemplate>
                <div class="jqpnl rounded">
                    <h3 class="entry-title">Adding A New Subscription...</h3>
                    <table border="0" cellspacing="0" cellpadding="0" class="edittabl">
                        <tr>
                            <td colspan="3">
                                <div class="jqinstruction rounded">
                                    <ul>
                                        <li>To be notified of every sale (not recommended for high-volume sales!)</li>
                                        <li>Make sure the recipients are set to administrator or report viewer</li>
                                        <li>Name should use this syntax: Track_{Merch|Ticket}_{Id}</li>
                                        <li>Ex: Track_Merch_10125, Track_Ticket_11109</li>
                                        <li>Be sure to describe the item you are tracking in the description field for easier tracking.</li>
                                    </ul>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <th>Is Active</th>
                            <td>
                                <asp:CheckBox ID="chkActive" runat="server" Checked="false" />
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <th>Recipients</th>
                            <td style="width:50%;">
                                <asp:DropDownList ID="ddlRoles" runat="server" Width="100%" 
                                    OnDataBinding="Roles_DataBinding" OnDataBound="Roles_DataBound" />
                            </td>
                            <td style="width:50%;white-space:nowrap;" class="intr">WebUser refers to the general customer</td>
                        </tr>
                        <tr>
                            <th><asp:RequiredFieldValidator ValidationGroup="mailer" CssClass="validator" ID="RequiredName" 
                                    runat="server" Display="Static" ControlToValidate="txtName" ErrorMessage="Name is required.">*</asp:RequiredFieldValidator>
                                Name
                            </th>
                            <td><asp:TextBox ID="txtName" runat="server" Width="100%" MaxLength="256" /></td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <th style="vertical-align:top;">Description</th>
                            <td><asp:TextBox ID="txtDescription" runat="server" Width="100%" Height="60px" TextMode="MultiLine" MaxLength="500" Text='<%#Bind("Description") %>' /></td>
                            <td class="intr">This text will be shown to the customer</td>
                        </tr>
                        <tr>
                            <th style="vertical-align:top;">Notes</th>
                            <td><asp:TextBox ID="txtInternalDescription" runat="server" Width="100%" Height="60px" TextMode="MultiLine" MaxLength="2000" Text='<%#Bind("InternalDescription") %>' /></td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <th>Subscribe Recipients?</th>
                            <td colspan="2">
                                <asp:CheckBox ID="chkSubscribe" runat="server" Checked="false" />
                                <span class="intr">Checking this box will make this subscription active for the chosen recipients</span></td>
                        </tr>
                    </table>
                    <div class="cmdsection">
                        <asp:Button ID="btnInsert" CausesValidation="true" CssClass="btnmed" runat="server" 
                            CommandName="Insert" Text="Save" />
                        <asp:Button ID="btnCancel" CssClass="btnmed" runat="server" CommandName="Cancel" Text="cancel" />
                        <asp:CustomValidator ID="CustomValidation" runat="server" ValidationGroup="mailer" CssClass="validator" 
                            Display="Static" ErrorMessage="CustomValidator">*</asp:CustomValidator>
                    </div>
                </div>
            </InsertItemTemplate>
        </asp:FormView>
    </div>
</div>