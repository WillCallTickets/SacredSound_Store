<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Editor_InvoiceFee.ascx.cs" Inherits="WillCallWeb.Admin.AdminControls.Editor_InvoiceFee" %>
<%@ Register src="~/Components/Util/CalendarClock.ascx" tagname="CalendarClock" tagprefix="uc1" %>
<div id="invoicefeeeditor">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
             <div class="jqhead rounded">
                <div class="sectitle">INVOICE FEE EDITOR<asp:CustomValidator ID="CustomValidation" runat="server" ValidationGroup="editor" CssClass="invisible" 
                    Display="Static" ErrorMessage="CustomValidator">*</asp:CustomValidator></div>
                <div class="jqinstruction rounded">
                    <ul>
                        <li>Be sure that the site is off before activating and deactivating processing fees - You don't want to be changing the fees on a customer in mid order</li>
                        <li>At least one non-overridden invoice fee must be active at all times</li>
                        <li>Only one fee that is not overridden can be active, and only one override fee can be active at a time. You may have one of each.</li>
                        <li>Activations will not be fully activated until they are published</li>
                    </ul>
                </div>
                <asp:GridView Width="100%" ID="GridView1" runat="server" DataKeyNames="Id" AutoGenerateColumns="False" CssClass="lsttbl" 
                    OnDataBound="GridView1_DataBound" OnDataBinding="GridView1_DataBinding" OnRowCommand="GridView1_RowCommand"
                    OnSelectedIndexChanged="GridView1_SelectedIndexChanged" OnRowDataBound="GridView1_RowDataBound" OnRowDeleting="GridView1_RowDeleting">
                    <SelectedRowStyle CssClass="selected" />
                    <Columns>
                        <asp:TemplateField ItemStyle-Wrap="false">
                            <ItemTemplate>
                                <asp:LinkButton Width="20px" Id="btnSelect" CssClass="btnselect" ToolTip="Select" runat="server" CommandName="Select" 
                                    CommandArgument='<%#Eval("Id") %>' CausesValidation="false" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:CheckBoxField DataField="IsActive" HeaderText="Active" ReadOnly="true" ItemStyle-HorizontalAlign="Center" />
                        <asp:CheckBoxField DataField="IsOverride" HeaderText="Override" ReadOnly="true" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField ItemStyle-HorizontalAlign="center" DataField="Price" DataFormatString="{0:c}" HtmlEncode="false" HeaderText="Price" />
                        <asp:BoundField DataField="Name" HeaderText="Name" HeaderStyle-HorizontalAlign="left" />
                        <asp:TemplateField HeaderText="Description - This is displayed in the cart during the order process." HeaderStyle-HorizontalAlign="left" ItemStyle-Width="100%">
                            <ItemTemplate>
                                <asp:Literal ID="LiteralDescription" runat="server"></asp:Literal>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-Wrap="false" >
                           <ItemTemplate>
                               <asp:Button Id="btnActivate" CssClass="btntny" runat="server" CommandName="Activate" 
                                    CausesValidation="false" CommandArgument='<%#Eval("Id") %>' Text="Activate" 
                                    OnClientClick='return confirm("This will activate the selected invoice fee. This may deactivate other invoice fees. See listed rules.")' />
                               <asp:LinkButton Width="20px" Id="btnDelete" CssClass="btndelete lastinrow" runat="server" CommandName="Delete" ToolTip="Delete" 
                                    CommandArgument='<%#Eval("Id") %>' causesValidation="false"
                                   OnClientClick='return confirm("Are you sure you want to delete this row?")' />
                           </ItemTemplate>
                       </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <asp:ValidationSummary ID="ValidationSummary1" CssClass="validationsummary" HeaderText="" ValidationGroup="editor" runat="server" />
             <asp:FormView ID="FormView1" Width="100%" DefaultMode="Edit" runat="server" OnItemUpdating="FormView1_ItemUpdating" OnDataBound="FormView1_DataBound" 
                OnDataBinding="FormView1_DataBinding" OnModeChanging="FormView1_ModeChanging" OnItemInserting="FormView1_ItemInserting" >
                <EmptyDataTemplate>
                    <div class="jqedt rounded">
                        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="edittabl">
                            <tr>
                                <th>Add A New Invoice Fee...</th>
                                <td><asp:Button Id="btnNew" CssClass="btnmed" CausesValidation="false" runat="server" CommandName="New" Text="new" /></td>
                            </tr>
                        </table>
                    </div>
                </EmptyDataTemplate>
                <EditItemTemplate>
                    <div class="jqpnl rounded">
                        <h3 class="entry-title"><%#Eval("Name") %></h3>
                        <table border="0" cellpadding="0" cellspacing="3" width="100%" class="edittabl">
                            <tr>
                                <th>&nbsp;</th>
                                <td class="listing-row">
                                    Active
                                    <asp:CheckBox ID="chkActive" runat="server" Enabled="false" Checked='<%#Eval("IsActive") %>' />
                                    Is Override
                                    <asp:CheckBox ID="chkOverride" runat="server" Enabled="false" Checked='<%#Eval("IsOverride") %>' />
                                </td>
                                <td style="width:100%;">&nbsp;</td>
                            </tr>
                            <tr>
                                <th><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" CssClass="validator" ValidationGroup="editor" 
                                    ErrorMessage="Name is required" Display="Static" ControlToValidate="txtName">*</asp:RequiredFieldValidator>
                                    Name</th>
                                <td colspan="2"><asp:TextBox Width="350px" ID="txtName" runat="server" Text='<%#Bind("Name") %>' MaxLength="256" />
                            </td>
                            </tr>
                            <tr>
                                <th><asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" CssClass="validator" ValidationGroup="editor" 
                                    ErrorMessage="Price is required" Display="Static" ControlToValidate="txtPrice">*</asp:RequiredFieldValidator>
                                    <asp:CompareValidator ID="CompareValidator1" CssClass="validator" ValidationGroup="editor" Display="Dynamic" 
                                    ControlToValidate="txtPrice" Operator="DataTypeCheck" Type="Double" runat="server" ErrorMessage="Please enter a valid price."></asp:CompareValidator>
                                    Price</th>
                                <td colspan="2"><asp:TextBox Width="350px" ID="txtPrice" runat="server" Text='<%#Bind("Price") %>' MaxLength="6" /></td>
                            </tr>
                            <tr>
                                <th><asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" CssClass="validator" ValidationGroup="editor" 
                                    ErrorMessage="Description is required" Display="Static" ControlToValidate="txtDescription">*</asp:RequiredFieldValidator>
                                    Description</th>
                                <td><asp:TextBox ID="txtDescription" runat="server" Width="350px" Text='<%#Bind("Description") %>' MaxLength="300" /></td>
                                <td class="intr">*Required* This is shown to the customer</td>
                            </tr>
                        </table>
                        <div class="cmdsection">
                            <asp:Button Id="btnSave" CssClass="btnmed" CausesValidation="true" ValidationGroup="editor" 
                                CommandArgument='<%#Eval("Id") %>' runat="server" CommandName="Update" Text="save" />
                            <asp:Button Id="btnCancel" CssClass="btnmed" CausesValidation="false" runat="server" 
                                CommandName="Cancel" Text="cancel" />
                            <asp:Button Id="btnNew" CssClass="btnmed" CausesValidation="false" runat="server" 
                                CommandName="New" Text="new" />
                        </div>
                    </div>
                </EditItemTemplate>
                <InsertItemTemplate>
                    <div class="jqpnl rounded">
                         <h3 class="entry-title">Adding A New Invoice Fee...</h3>
                         <table border="0" cellpadding="0" cellspacing="3" width="100%" class="edittabl">
                            <tr>
                                <th>IsOverride</th>
                                <td><asp:CheckBox ID="chkOverride" runat="server" /> 
                                <span class="intr">Check this to create an invoice fee that will have requirements</span></td>
                            </tr>
                            <tr>
                                <th><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" CssClass="validator" ValidationGroup="editor" 
                                    ErrorMessage="Name is required" Display="Static" ControlToValidate="txtName">*</asp:RequiredFieldValidator>
                                    Name</th>
                                <td><asp:TextBox Width="350px" ID="txtName" runat="server" Text='<%#Bind("Name") %>' MaxLength="256" />
                            </tr>
                            <tr>
                                <th><asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" CssClass="validator" ValidationGroup="editor" 
                                    ErrorMessage="Price is required" Display="Static" ControlToValidate="txtPrice">*</asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="CompareValidator1" CssClass="validator" ValidationGroup="editor" Display="Dynamic" 
                                    ControlToValidate="txtPrice" Operator="DataTypeCheck" Type="Double" runat="server" ErrorMessage="Please enter a valid price."></asp:CompareValidator>
                                    Price</th>
                                <td><asp:TextBox Width="350px" ID="txtPrice" runat="server" Text='<%#Bind("Price") %>' MaxLength="6" /></td>
                            </tr>
                            <tr>
                                <th><asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" CssClass="validator" ValidationGroup="editor" 
                                        ErrorMessage="Description is required" Display="Static" ControlToValidate="txtDescription">*</asp:RequiredFieldValidator>
                                        Description</th>
                                <td><asp:TextBox ID="txtDescription" runat="server" Width="350px" Text='<%#Bind("Description") %>' MaxLength="300" />
                                    <span class="intr">*Required* This is shown to the customer</span>    
                                </td>
                                
                            </tr>
                        </table>
                        <div class="cmdsection">
                            <asp:Button Id="btnSave" CssClass="btnmed" CausesValidation="true" ValidationGroup="editor" 
                                CommandArgument='<%#Eval("Id") %>' runat="server" CommandName="Insert" Text="save" />
                            <asp:Button Id="btnCancel" CssClass="btnmed" CausesValidation="false" runat="server" 
                                CommandName="Cancel" Text="cancel" />
                        </div>
                    </div>
                </InsertItemTemplate>
            </asp:FormView>
            <asp:GridView Width="100%" ID="GridReq" runat="server" DataKeyNames="Id" AutoGenerateColumns="False" 
                OnDataBound="GridReq_DataBound" OnDataBinding="GridReq_DataBinding" CssClass="lsttbl" 
                OnSelectedIndexChanged="GridReq_SelectedIndexChanged" OnRowDataBound="GridReq_RowDataBound" OnRowDeleting="GridReq_RowDeleting">
                <SelectedRowStyle CssClass="selected" />
                <Columns>
                    <asp:TemplateField ItemStyle-Wrap="false">
                         <ItemTemplate>
                            <asp:LinkButton Width="20px" Id="btnSelect" CssClass="btnselect" ToolTip="Select" CausesValidation="false" runat="server" 
                                CommandName="Select" Text="Select" CommandArgument='<%#Eval("Id") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Description" HeaderText="Fee Override Requirements" HeaderStyle-HorizontalAlign="left" ItemStyle-Width="100%" />
                    <asp:CheckBoxField DataField="IsActive" ReadOnly="true" HeaderText="IsActive" ItemStyle-HorizontalAlign="Center" />
                    <asp:TemplateField ItemStyle-Wrap="false" HeaderText="Start" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Literal ID="litStart" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-Wrap="false" HeaderText="End" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Literal ID="litEnd" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="RequiredContext" HeaderText="Context" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="IdxListing" HeaderText="Req Idx" HeaderStyle-Wrap="false" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="RequiredQty" HeaderText="Req Qty" HeaderStyle-Wrap="false" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="MinimumAmount" HeaderText="Req Amt" HeaderStyle-Wrap="false" DataFormatString="{0:n2}" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" />
                    <asp:TemplateField ItemStyle-Wrap="false" >
                       <ItemTemplate>
                           <asp:LinkButton Width="20px" Id="btnDelete" CssClass="btndelete lastinrow" runat="server" CommandName="Delete" Text="Delete" CommandArgument='<%#Eval("Id") %>' 
                               CausesValidation="false" ToolTip="Delete"
                               OnClientClick='return confirm("Are you sure you want to delete this row?")' />
                       </ItemTemplate>
                   </asp:TemplateField>
                </Columns>
            </asp:GridView>
             <asp:FormView ID="FormReq" Width="100%" DefaultMode="Edit" runat="server" OnItemUpdating="FormReq_ItemUpdating"
                OnDataBinding="FormReq_DataBinding" OnDataBound="FormReq_DataBound" OnModeChanging="FormReq_ModeChanging" OnItemInserting="FormReq_ItemInserting">
                <EmptyDataTemplate>
                    <div class="jqhead rounded">
                        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="edittabl">
                            <tr>
                                <th>Add A New Item...</th>
                                <td><asp:Button Id="btnNew" CssClass="btnmed" CausesValidation="false" runat="server" CommandName="New" Text="new" /></td>
                            </tr>
                        </table>
                    </div>
                </EmptyDataTemplate>
                <EditItemTemplate>
                     <div class="jqpnl rounded">
                         <table border="0" cellpadding="0" cellspacing="3" width="100%" class="edittabl">
                            <tr>
                                <th><span class="intr"><%#Eval("Id") %></span> Active</th>
                                <td colspan="2">
                                    <asp:Checkbox ID="chkActive" runat="server" Font-Bold="true" Checked='<%#Bind("IsActive") %>' />
                                </td>
                            </tr>
                            <tr>
                                <th>Is Exclusive</th>
                                <td colspan="2">
                                    <asp:Checkbox ID="chkExclusive" runat="server" Font-Bold="true" Checked='<%#Bind("IsExclusive") %>' />
                                    <span class="intr">Will the fee apply if there are other items in the order? Ignored for shipping contexts.</span>
                                </td>
                                
                            </tr>
                            <tr>
                                <th>Description</th>
                                <td colspan="2">
                                    <asp:Textbox ID="txtDescription" runat="server" Text='<%#Bind("Description") %>' MaxLength="500" Width="325px" />
                                    <div class="intr">What is required? - shown to the user on non-qualification. ie: <b>Buy this item and get a discount.</b></div>
                                </td>
                                
                            </tr>
                            <tr>
                                <th>Start Date</th>
                                <td style="padding:0;">
                                    <uc1:CalendarClock ID="clockStart" runat="server" UseTime="true" SelectedValue='<%#Bind("DateStart") %>' UseReset="true" 
                                        ValidationGroup="editor" />
                                </td>
                                <td rowspan="2">
                                    <div class="jqinstruction rounded">
                                        <div>OVERRIDE FEE REQUIREMENTS...</div>
                                        <ul>
                                            <li>All requirements are OR based - meaning only ONE is required to fulfill the promotion</li>
                                            <li>For ticket packages - use the base ticket id (the one with the earliest date)</li>
                                        </ul>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <th>End Date</th>
                                <td style="padding:0;">
                                    <uc1:CalendarClock ID="clockEnd" runat="server" UseTime="true" SelectedValue='<%#Bind("DateEnd") %>' UseReset="true" 
                                        ValidationGroup="editor" />
                                </td>
                            </tr>
                                <tr>
                                    <th>Context&nbsp;<b><%#Eval("RequiredContext") %></b></th>
                                    <td colspan="2"><asp:DropDownList ID="ddlIdx" runat="server" Width="650px" /> (provided for ID reference only)</td>
                                </tr>
                                <tr>
                                    <th>Required Ids</th>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtRequiredIds" runat="server" Width="350px" Text='<%#Eval("IdxListing") %>' MaxLength="100" />
                                        <div class="intr">
                                            A comma separated list of ids - or in the case of a shipping or code requirement - the text of the method or code</div>
                                    </td>
                                </tr>
                                <tr>
                                    <th>Required Qty</th>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtQty" runat="server" Width="100px" Text='<%#Eval("RequiredQty") %>' MaxLength="12" />
                                        <span class="intr">Minimum quantity that must be purchased of the item with the id in question. TODO: make this an up down control</span>
                                    </td>
                                </tr>
                                <tr>
                                    <th>Required Amount</th>
                                    <td colspan="2"><asp:TextBox ID="txtAmount" runat="server" Width="100px" Text='<%#Eval("MinimumAmount","{0:n2}") %>' MaxLength="12" />
                                        <span class="intr">Minimum amount that must be purchased</span>
                                    </td>
                                </tr>
                            </table>
                            <div class="cmdsection">
                                <asp:Button Id="btnSave" CssClass="btnmed" CausesValidation="true" ValidationGroup="editor" 
                                    CommandArgument='<%#Eval("Id") %>' runat="server" CommandName="Update" Text="save" />
                                <asp:Button Id="btnCancel" CssClass="btnmed" CausesValidation="false" runat="server" 
                                    CommandName="Cancel" Text="cancel" />
                                <asp:Button Id="btnNew" CssClass="btnmed" CausesValidation="false" runat="server" 
                                    CommandName="New" Text="new" />
                            </div>
                        </div>
                    </EditItemTemplate>
                    <InsertItemTemplate>
                        <div class="jqpnl rounded">
                        <h3 class="entry-title">Adding A New Requirement...</h3>
                         <table border="0" cellpadding="0" cellspacing="3" width="100%" class="edittabl">
                            <tr>
                                <th>Context</th>
                                <td colspan="2" style="width:100%;"><asp:DropDownList Width="350px" ID="ddlContext" runat="server" datasource='<%#Enum.GetNames(typeof(Wcss._Enums.RequirementContext)) %>' /></td>
                            </tr>
                            <tr>
                                <th>ShipMethod Name</th>
                                <td><asp:TextBox ID="txtRequiredIds" runat="server" Width="350px" Text="" MaxLength="50" /></td>
                                <td class="intr">ONLY IF CONTEXT IS A SHIPPING METHOD! Ship method name, etc</td>
                            </tr>
                            <tr>
                                <th>&nbsp;</th>
                                <td colspan="2">
                                    <div class="jqinstruction rounded">
                                        <div>OVERRIDE FEE REQUIREMENTS...</div>
                                        <ul>
                                            <li>YOU CAN CHOOSE REQUIREMENTS IN THE NEXT STEP</li>
                                            <li>All requirements are OR based - meaning only ONE is required to fulfill the promotion</li>
                                            <li>For ticket packages - use the base ticket id (the one with the earliest date)</li>
                                        </ul>
                                    </div>
                                </td>
                            </tr>
                        </table>
                        <div class="cmdsection">
                            <asp:Button Id="btnSave" CssClass="btnmed" CausesValidation="true" ValidationGroup="editor" 
                                CommandArgument='<%#Eval("Id") %>' runat="server" CommandName="Insert" Text="save" />
                            <asp:Button Id="btnCancel" CssClass="btnmed" CausesValidation="false" runat="server" 
                                CommandName="Cancel" Text="cancel" />
                        </div>
                    </div>
                </InsertItemTemplate>
            </asp:FormView>
        </ContentTemplate>
     </asp:UpdatePanel>
</div>