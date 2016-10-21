<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ShowLinks.ascx.cs" Inherits="WillCallWeb.Admin.AdminControls.ShowLinks" %>
<div id="srceditor">
    <div id="showlinks">
        <div class="jqhead rounded">
            <h3 class="entry-title">Show Links - <asp:Literal ID="litShowTitle" runat="server" /></h3>        
            <asp:GridView ID="GridView1" Width="100%" runat="server" AutoGenerateColumns="False" DataKeyNames="Id" 
                ShowFooter="False" CssClass="lsttbl"
                OnDataBinding="GridView1_DataBinding" 
                OnDataBound="GridView1_DataBound" 
                OnRowDataBound="GridView1_RowDataBound"
                OnRowCommand="GridView1_RowCommand" 
                OnRowDeleting="GridView1_RowDeleting" 
                OnSelectedIndexChanged="GridView1_SelectedIndexChanged" >
                <SelectedRowStyle CssClass="selected" />
                <EmptyDataTemplate>
                    <div class="lstempty">There Are No Show Links</div>
                </EmptyDataTemplate>
                <Columns>
                    <asp:TemplateField ItemStyle-Wrap="false" HeaderText="LINKS">
                        <ItemTemplate>
                            <asp:LinkButton Width="20px" Id="btnSelect" ToolTip="Select" CssClass="btnselect" runat="server" CommandName="Select" 
                                CommandArgument='<%#Eval("Id") %>' CausesValidation="false" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:CheckBoxField DataField="IsActive" HeaderText="Active" ItemStyle-HorizontalAlign="Center" />
                    <asp:TemplateField HeaderText="Display Text" ItemStyle-Width="100%" HeaderStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Literal ID="litText" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Link" HeaderStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Literal ID="litLink" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
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
            <asp:ValidationSummary ID="ValidationSummary2" CssClass="validationsummary" HeaderText="" ValidationGroup="links" runat="server" />
        </div>

        <asp:FormView Width="100%" ID="FormView1" runat="server" DefaultMode="Edit" 
            OnDataBinding="FormView1_DataBinding" 
            OnDataBound="FormView1_DataBound" 
            OnItemCommand="FormView1_ItemCommand" 
            OnItemInserting="FormView1_ItemInserting" 
            OnItemUpdating="FormView1_ItemUpdating"
            OnModeChanging="FormView1_ModeChanging">
            <EmptyDataTemplate>
                <div class="jqpnl rounded iit">
                    <div class="cmdsection">
                        <h3 class="entry-title" style="display:inline;">Adding A New Show Link...</h3>
                        <asp:Button Id="btnNew" CausesValidation="false" runat="server" CommandName="New" Text="New"
                            cssclass="btnmed" />
                        <asp:Button ID="btnDisplay" runat="server" CausesValidation="false" CommandName="view" Text="Display Show" 
                            CssClass="btnmed" OnClientClick='doPagePopup("/Admin/ShowDisplayer.aspx", "false"); ' />
                    </div>
                </div>
            </EmptyDataTemplate>
            <EditItemTemplate>
                <div class="jqpnl rounded iit">
                    <div class="cmdsection">
                        <asp:Button ID="btnSave" CausesValidation="false" runat="server" 
                            CommandName="Update" Text="Save" CssClass="btnmed" />
                        <asp:Button ID="btnCancel" runat="server" CommandName="Cancel" Text="Cancel" CausesValidation="false" 
                             CssClass="btnmed" />
                        <asp:Button ID="btnNew" runat="server" CommandName="New" Text="New" CausesValidation="false"
                             CssClass="btnmed" />
                        <asp:CustomValidator ID="CustomValidation" runat="server" 
                            ValidationGroup="links" CssClass="invisible" Display="Static" ErrorMessage="CustomValidator">*</asp:CustomValidator>
                        <asp:Button ID="btnDisplay" runat="server" CausesValidation="false" CommandName="view" Text="Display Show" 
                            CssClass="btnmed" OnClientClick='doPagePopup("/Admin/ShowDisplayer.aspx", "false"); ' />
                    </div>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" class="edittabl" >
                        <tr>
                            <th>Active</th>
                            <td style="width:100%;"><asp:CheckBox ID="chkActive" runat="server" Checked='<%#Bind("IsActive") %>' /></td>
                        </tr>
                        <tr>
                            <th>Display Text</th>
                            <td><asp:TextBox ID="txtDisplayText" MaxLength="300" Width="650px" runat="server" Text='<%#Bind("DisplayText") %>' /></td>
                        </tr>
                        <tr>
                            <th>Link Url</th>
                            <td><asp:TextBox ID="txtLinkUrl" MaxLength="300" Width="650px" runat="server" Text='<%#Bind("LinkUrl") %>' /></td>
                        </tr>
                        <tr>
                            <th>Show Link</th>
                            <td><asp:DropDownList ID="ddlShowLinks" runat="server" Width="650px" OnDataBinding="ddlShowLinks_DataBinding" OnDataBound="ddlShowLinks_DataBound" /></td>
                        </tr>
                    </table>                    
                </div>
            </EditItemTemplate>
            <InsertItemTemplate>
                <div class="jqpnl rounded iit">
                    <h3 class="entry-title">Add A New Show Link...</h3>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" class="edittabl">
                        <tr>
                            <th>Display Text</th>
                            <td>
                                <asp:TextBox ID="txtDisplayText" MaxLength="300" Width="650px" runat="server" Text='<%#Bind("DisplayText") %>' />
                                <div class="intr">Required for remote link types only</div>
                            </td>
                        </tr>                        
                        <tr>
                            <th>Link Url</th>
                            <td><asp:TextBox ID="txtLinkUrl" MaxLength="300" Width="650px" runat="server" Text='<%#Bind("LinkUrl") %>' /></td>
                        </tr>
                        <tr><td colspan="2"><hr /></td></tr>
                        <tr>
                            <th>Show Link</th>
                            <td><asp:DropDownList ID="ddlShowLinks" runat="server" Width="650px" OnDataBinding="ddlShowLinks_DataBinding" /></td>
                        </tr>
                    </table>
                    <div class="cmdsection">
                        <asp:Button ID="btnInsert" CausesValidation="false" runat="server" 
                            CommandName="Insert" Text="Add Link" CssClass="btnmed" />
                        <asp:Button ID="btnCancel" runat="server" CommandName="Cancel" Text="Cancel" CausesValidation="false" 
                            CssClass="btnmed" />
                        <asp:CustomValidator ID="CustomValidation" runat="server" 
                            ValidationGroup="links" CssClass="invisible" Display="Static" ErrorMessage="CustomValidator">*</asp:CustomValidator>
                    </div>
                </div>
            </InsertItemTemplate>
        </asp:FormView>
    </div>
</div>  

            




  

            




