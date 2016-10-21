<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Editor_Faq.ascx.cs" Inherits="WillCallWeb.Admin.AdminControls.Editor_Faq" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<div id="srceditor">
    <div id="faqeditor">
        <div class="jqhead rounded">
            <h3 class="entry-title">FAQ EDITOR</h3>
        </div>
        <asp:ValidationSummary ID="ValidationSummary1" CssClass="validationsummary" HeaderText="Please fix the following errors:"
            ValidationGroup="srceditor" runat="server" />
        <div class="jqpnl rounded">
            <table border="0" cellpadding="0" cellspacing="0" width="100%" class="edittabl">
                <tr>
                    <td rowspan="99" style="text-align:center;">
                        <asp:DataList ID="listCategories" runat="server" DataKeyField="Id" GridLines="None" HorizontalAlign="Center" 
                            RepeatDirection="Vertical" RepeatLayout="Table" CssClass="sidbar" 
                            ShowFooter="false" ShowHeader="true" 
                            OnDataBinding="listCategories_DataBinding" 
                            OnItemDataBound="listCategories_ItemDataBound" 
                            OnSelectedIndexChanged="listCategories_SelectedIndexChanged"
                            OnItemCommand="listCategories_ItemCommand">
                            <SeparatorTemplate></SeparatorTemplate>
                            <SelectedItemStyle CssClass="contextselect" />
                            <ItemStyle HorizontalAlign="Center" CssClass="matchcontextdims" Wrap="false" />
                            <HeaderTemplate>
                                <div class="subtitle" style="text-align:center;">Categorie Listing</div>
                                <div style="text-align:center;white-space:nowrap;margin:8px 0;">
                                    <span style="display:inline-block;vertical-align:top;padding:4px 0 0 0;font-weight:bold;">
                                        Faq is <%if (Wcss._Config._FAQ_Page_On){%>ON<%}else{ %>OFF<%} %>
                                    </span>
                                    <asp:Button ID="btnToggle" runat="server" Text="Toggle" cssclass="btnmed" 
                                        OnClientClick="return confirm('This will toggle the FAQ on or off. Are you sure you want to continue?');" 
                                        onclick="btnToggle_Click" CausesValidation="false" />
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Button ID="btnCategory" CssClass="btnmed" Width="100px" CommandName="Select" runat="server" 
                                    ToolTip='<%#Eval("Name") %>' CausesValidation="false" />
                                <span style="display:inline-block;vertical-align:middle;margin:0;">
                                    <asp:LinkButton Width="20px" Id="btnUp" ToolTip="Move Up" CssClass="btnup" runat="server" CommandName="Up" 
                                        CommandArgument='<%#Eval("Id") %>' CausesValidation="false" />
                                    <asp:LinkButton Width="20px" Id="btnDown" ToolTip="Move Down" CssClass="btndown" runat="server" CommandName="Down" 
                                        CommandArgument='<%#Eval("Id") %>' CausesValidation="false" />
                                </span>
                            </ItemTemplate>
                         </asp:DataList>
                    </td>
                    <td style="width:100%" valign="top" >
                        <div class="jqhead rounded">
                            <asp:FormView ID="FormView1" runat="server" DefaultMode="Edit" DataKeyNames="Id" Width="100%"  
                            OnModeChanging="FormView1_ModeChanging" 
                            OnDataBinding="FormView1_DataBinding" 
                            OnDataBound="FormView1_DataBound"
                            OnItemInserting="FormView1_ItemInserting" 
                            OnItemUpdating="FormView1_ItemUpdating" 
                            OnItemDeleting="FormView1_ItemDeleting">
                            <EmptyDataTemplate>
                                <div class="cmdsection">
                                    <h3 class="entry-title">Add New Categorie
                                    <asp:Button ID="btnNew" CssClass="btnmed" runat="server" CommandName="New" Text="New Categorie"
                                        causesValidation="false" /></h3>
                                </div>
                            </EmptyDataTemplate>
                            <EditItemTemplate>
                                <div class="cmdsection">
                                    <h3 class="entry-title"><%#Eval("Name") %>
                                        <asp:Button ID="btnSave" CssClass="btnmed" runat="server" 
                                            CommandName="Update" Text="Save" CausesValidation="true" ValidationGroup="srceditor" />
                                        <asp:Button ID="btnCancel" CssClass="btnmed" runat="server" CommandName="Cancel" 
                                            Text="Cancel" CausesValidation="false" />
                                        <asp:Button Id="btnDelete" CssClass="btnmed" runat="server" CommandName="Delete" 
                                           Text="Delete" CommandArgument='<%#Eval("Id") %>' ToolTip="Delete" CausesValidation="false"
                                           OnClientClick='return confirm("Are you sure you want to delete this CATEGORIE?")' />
                                        <asp:Button ID="btnNew" CssClass="btnmed" runat="server" CommandName="New" Text="New Categorie"
                                            causesValidation="false" />
                                    </h3>
                                </div>
                                <table border="0" cellpadding="0" cellspacing="0" class="edittabl">
                                    <tr>
                                        <th><asp:CustomValidator ID="cusCategory" runat="server" ValidationGroup="srceditor" 
                                                CssClass="validator" Display="Static">*</asp:CustomValidator>
                                        </th>
                                        <th>Category Name</th>
                                        <td><asp:TextBox ID="txtName" runat="server" ReadOnly="true" MaxLength="256" 
                                                Width="300px" Text='<%#Bind("Name") %>' /></td>
                                    </tr>
                                    <tr>
                                        <th colspan="2">Display Name</th>
                                        <td><asp:TextBox ID="txtDisplay" runat="server" MaxLength="500" Width="300px" Text='<%#Bind("DisplayText") %>' /></td>
                                    </tr>
                                    <tr>
                                        <th colspan="2">Is Active</th>
                                        <td><asp:CheckBox ID="chkActive" runat="server" Checked='<%#Bind("IsActive") %>' /></td>
                                    </tr>
                                    <tr>
                                        <th colspan="2">Desc</th>
                                        <td><asp:TextBox ID="txtDescription" runat="server" MaxLength="500" 
                                                Width="300px" Text='<%#Bind("Description") %>' /></td>
                                    </tr>   
                                </table>
                                <div class="jqpnl rounded">
                                    <asp:GridView ID="GridItems" Width="100%" runat="server" AutoGenerateColumns="False" DataKeyNames="Id" 
                                        ShowFooter="False" CssClass="lsttbl" GridLines="Both"
                                        OnDataBinding="GridItems_DataBinding" 
                                        OnRowDataBound="GridItems_RowDataBound" 
                                        OnDataBound="GridItems_DataBound" 
                                        OnRowCommand="GridItems_RowCommand" 
                                        OnRowDeleting="GridItems_RowDeleting" 
                                        OnSelectedIndexChanged="GridItems_SelectedIndexChanged" >                          
                                        <SelectedRowStyle CssClass="selected" />
                                        <EmptyDataTemplate>
                                            <div class="lstempty">No Faq Items</div>
                                        </EmptyDataTemplate>
                                        <Columns>
                                            <asp:TemplateField ItemStyle-Wrap="false">
                                                <ItemTemplate>
                                                    <asp:LinkButton Width="20px" Id="btnSelect" ToolTip="Select" CssClass="btnselect" runat="server" CommandName="Select" 
                                                        CommandArgument='<%#Eval("Id") %>' CausesValidation="false" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:CheckBoxField DataField="IsActive" HeaderText="Active" HeaderStyle-HorizontalAlign="Center" 
                                                ItemStyle-HorizontalAlign="Center" />
                                            <asp:TemplateField ItemStyle-Width="100%" >
                                                <ItemTemplate>
                                                    <div><asp:Literal ID="litQuestion" runat="server" /></div>
                                                    <div style="height:65px;overflow:auto;"><asp:Literal ID="litAnswer" runat="server" /></div>
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
                                                   <asp:CustomValidator ID="CustomDelete" runat="server" ValidationGroup="srceditor" CssClass="validator" 
                                                        Display="Static" ErrorMessage="">*</asp:CustomValidator>
                                               </ItemTemplate>
                                           </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                                <div class="jqpnl rounded">
                                    <asp:FormView ID="FormItem" runat="server" Width="100%" DefaultMode="Edit" DataKeyNames="Id" 
                                    OnDataBinding="FormItem_DataBinding" 
                                    OnDataBound="FormItem_DataBound"
                                    OnItemInserting="FormItem_ItemInserting" 
                                    OnItemUpdating="FormItem_ItemUpdating" 
                                    OnItemCommand="FormItem_ItemCommand"
                                    OnModeChanging="FormItem_ModeChanging" >
                                    <EmptyDataTemplate>
                                        <div class="cmdsection">
                                            <h3 class="entry-title">Add New Faq Item
                                                <asp:Button ID="btnNew" CssClass="btnmed" runat="server" CommandName="New" Text="New Item"
                                                    CausesValidation="false" />
                                            </h3>
                                        </legend>
                                        </fieldset>
                                    </EmptyDataTemplate>
                                    <EditItemTemplate>
                                        <div class="cmdsection">
                                            <h3 class="entry-title">Selected Item
                                                <asp:Button ID="btnActivate" CssClass="btnmed" CausesValidation="false" 
                                                    runat="server" CommandName="activate" CommandArgument='<%#Eval("Id") %>' Text="Activate" />
                                                <asp:Button ID="btnSave" CssClass="btnmed" CausesValidation="true" 
                                                    ValidationGroup="srceditor" runat="server" CommandName="Update" Text="Save" />
                                                <asp:Button ID="btnCancel" CssClass="btnmed" runat="server" 
                                                    CommandName="Cancel" Text="Cancel" CausesValidation="false" />
                                                <asp:Button ID="btnNew" CssClass="btnmed" runat="server" 
                                                    CommandName="New" Text="New" CausesValidation="false" />
                                            </h3>
                                        </div>
                                        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="edittabl">
                                            <tr>
                                                <th style="vertical-align:top;">Re-categorize</th>
                                                <td>
                                                    <asp:DropDownList ID="ddlCategories" runat="server" OnDataBinding="ddlCategories_DataBinding" 
                                                        OnDataBound="ddlCategories_DataBound" AutoPostBack="true" Width="250px" 
                                                        OnSelectedIndexChanged="ddlCategories_SelectedIndexChanged" />
                                                    <div style="margin-top:6px;">*** note that this will cause the current item to be marked as inactive ***</div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <th>
                                                    <asp:RequiredFieldValidator ID="Required1" runat="server" ValidationGroup="srceditor" CssClass="validator" 
                                                        Display="Static" ErrorMessage="Question is required" ControlToValidate="txtQuestion">*</asp:RequiredFieldValidator>
                                                    Question</th>
                                                <td>
                                                    <asp:TextBox ID="txtQuestion" runat="server" MaxLength="1000" TextMode="MultiLine" Height="50px" 
                                                        Width="100%" Text='<%#Bind("Question") %>' /></td>
                                            </tr>
                                            <tr>
                                                <th>
                                                    Answer                                                
                                                    <div style="margin-top:8px;">
                                                        <asp:Button ID="btnWys" CausesValidation="false" runat="server" CommandName="Wys" Text="Edit" Width="100px" 
                                                            CssClass="btnmed ov-trigger" Tooltip="/Admin/AdminControls/Wysiwyg/Wysiwyg.aspx?context=faq&ctrl=" rel="#overlay-wysiwyg" />
                                                    </div>
                                                </th>
                                                <td style="width:100%;">
                                                    <asp:Literal ID="litDesc" runat="server" />
                                                </td>
                                            </tr>  
                                        </table>
                                    </EditItemTemplate>
                                    <InsertItemTemplate> 
                                        <div class="cmdsection">
                                            <h3 class="entry-title">Add New Faq Item
                                                <asp:Button ID="btnSave" CausesValidation="true" ValidationGroup="srceditor" runat="server" 
                                                    CommandName="Insert" Text="Save" CssClass="btnmed" />
                                                <asp:Button ID="btnCancel" CausesValidation="false" runat="server" CommandName="Cancel" 
                                                    Text="Cancel" CssClass="btnmed" />
                                            </h3>
                                        </div>
                                        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="edittabl">
                                            <tr>
                                                <th>
                                                    <asp:RequiredFieldValidator ID="Required1" runat="server" ValidationGroup="srceditor" CssClass="validator" 
                                                        Display="Static" ErrorMessage="Question is required" ControlToValidate="txtQuestion">*</asp:RequiredFieldValidator>
                                                </th>
                                                <th style="vertical-align:top;">Question &#9658;</th>
                                                <td style="width:100%;">
                                                    <asp:TextBox ID="txtQuestion" runat="server" MaxLength="1000" TextMode="MultiLine" Height="50px" 
                                                        Width="100%" Text='<%#Bind("Question") %>' />
                                                    <div class="intr">**include question mark if appropriate**</div>
                                                </td>
                                            </tr>
                                            <tr><th colspan="2" style="vertical-align: bottom;text-align:center;">Answer &#9660;</th>
                                                <th>&nbsp;</th></tr>
                                            <tr>
                                                <td colspan="3" style="width:100%;">
                                                    <asp:TextBox ID="txtAnswer" runat="server" Width="100%" Height="125px" MaxLength="2000" TextMode="MultiLine" />
                                                </td>
                                            </tr>
                                        </table>
                                    </InsertItemTemplate>
                                </asp:FormView>
                                </div>
                            </EditItemTemplate>
                            <InsertItemTemplate>
                                <div class="cmdsection">
                                    <h3 class="entry-title">Add New Faq Categorie
                                        <asp:Button ID="btnSave" CausesValidation="true" ValidationGroup="srceditor" runat="server" 
                                            CommandName="Insert" Text="Save" CssClass="btnmed" />
                                        <asp:Button ID="btnCancel" runat="server" CommandName="Cancel" Text="Cancel" CausesValidation="false" 
                                            CssClass="btnmed" />
                                    </h3>
                                </div>
                                <table border="0" cellpadding="0" cellspacing="0" width="100%" class="edittabl">
                                    <tr>
                                        <th>
                                            <asp:CustomValidator ID="cusCategory" runat="server" ValidationGroup="srceditor" 
                                                CssClass="validator" Display="Static">*</asp:CustomValidator>
                                        </th>
                                        <th>Category Name</th>
                                        <td><asp:TextBox ID="txtName" runat="server" MaxLength="256" Width="300px" Text='<%#Bind("Name") %>' /></td>
                                        <td class="intr">must be unique</td>
                                    </tr>
                                    <tr>
                                        <th colspan="2">Display Name</th>
                                        <td colspan="2"><asp:TextBox ID="txtDisplay" runat="server" MaxLength="500" Width="300px" Text='<%#Bind("DisplayText") %>' /></td>
                                    </tr>
                                </table>
                            </InsertItemTemplate>
                        </asp:FormView>
                        </div>
                    </td>
                </tr>
            </table>
         </div>
     </div>
</div>

<div class="admin_overlay" id="overlay-wysiwyg" style="display:none;">
	<div class="contentWrap"></div>
</div>

<script type="text/javascript" src="/JQueryUI/admin-overlay.js"></script>