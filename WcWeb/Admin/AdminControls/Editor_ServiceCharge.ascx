<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Editor_ServiceCharge.ascx.cs" Inherits="WillCallWeb.Admin.AdminControls.Editor_ServiceCharge" %>
<div id="servicechargeeditor">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="jqhead rounded">
                <div class="sectitle">SERVICE CHARGE EDITOR
                    <asp:CustomValidator ID="CustomValidation" runat="server" ValidationGroup="editor" CssClass="invisible" 
                        Display="Static" ErrorMessage="CustomValidator">*</asp:CustomValidator>
                </div>
                 <asp:GridView Width="100%" ID="GridView1" runat="server" DataKeyNames="Id" AutoGenerateColumns="False" CssClass="lsttbl"
                    OnDataBound="GridView1_DataBound" OnDataBinding="GridView1_DataBinding" 
                    OnSelectedIndexChanged="GridView1_SelectedIndexChanged" OnRowDataBound="GridView1_RowDataBound" OnRowDeleting="GridView1_RowDeleting">
                    <SelectedRowStyle CssClass="selected" />
                    <Columns>
                        <asp:TemplateField ItemStyle-Wrap="false">
                            <ItemTemplate>
                                <asp:LinkButton Width="20px" Id="btnSelect" ToolTip="Select" CssClass="btnselect" runat="server" CommandName="Select" 
                                    Text="Select" CommandArgument='<%#Eval("Id") %>' CausesValidation="false" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="MaxValue" HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="right" 
                            DataFormatString="{0:c}" HtmlEncode="false" HeaderText="Max Value" />
                        <asp:BoundField DataField="Charge" HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="right"  
                            DataFormatString="{0:c}" HtmlEncode="false" HeaderText="Charge" />
                        <asp:BoundField HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="right" DataField="Percentage" 
                            DataFormatString="{0:n4}" HtmlEncode="false" HeaderText="Percentage" />
                        <asp:TemplateField ItemStyle-Wrap="false" ItemStyle-Width="100%" ><ItemTemplate>&nbsp;</ItemTemplate></asp:TemplateField>
                        <asp:TemplateField ItemStyle-Wrap="false" >
                           <ItemTemplate>
                               <asp:LinkButton Width="20px" Id="btnDelete" CssClass="btndelete lastinrow" runat="server" CommandName="Delete" 
                                    Text="Delete" CommandArgument='<%#Eval("Id") %>' ToolTip="Delete" CausesValidation="false"
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
                    <div class="jqpnl rounded">
                        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="edittabl">
                            <tr>
                                <th>Add A New Service Charge...</th>
                                <td><asp:Button Id="btnNew" CssClass="btnmed" CausesValidation="false" runat="server" CommandName="New" Text="new" /></td>
                            </tr>
                        </table>
                    </div>
                </EmptyDataTemplate>
                <EditItemTemplate>
                    <div class="jqpnl rounded">
                        <h3 class="entry-title">MaxValue: <%#Eval("MaxValue", "{0:n2}")%> Charge: <%#Eval("Charge", "{0:n2}")%> Pct: <%#Eval("Percentage","{0:n2}") %></h3>
                         <table border="0" cellpadding="0" cellspacing="3" width="100%" class="edittabl">
                            <tr>
                                <th>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" CssClass="validator" ValidationGroup="editor" 
                                        ErrorMessage="MaxValue is required" Display="Static" ControlToValidate="txtMax">*</asp:RequiredFieldValidator>
                                    <asp:CompareValidator ID="CompareValidator1" CssClass="validator" ValidationGroup="editor" Display="Dynamic" 
                                        ControlToValidate="txtMax" Operator="DataTypeCheck" Type="Double" runat="server" ErrorMessage="Please enter a valid max value."></asp:CompareValidator>
                                    <a href="javascript: alert('Max Value is the maximum value that the charge will be applied to.')" class="infomark">?</a>Max Value</th>
                                <td style="width:100%;">
                                    <asp:TextBox Width="350px" ID="txtMax" runat="server" Text='<%#Bind("MaxValue","{0:n2}") %>' MaxLength="6" />
                               </td>
                            </tr>
                            <tr>
                                <th>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" CssClass="validator" ValidationGroup="editor" 
                                        ErrorMessage="Charge is required" Display="Static" ControlToValidate="txtCharge">*</asp:RequiredFieldValidator>
                                    <asp:CompareValidator ID="CompareValidator2" CssClass="validator" ValidationGroup="editor" Display="Dynamic" 
                                        ControlToValidate="txtCharge" Operator="DataTypeCheck" Type="Double" runat="server" ErrorMessage="Please enter a valid charge."></asp:CompareValidator>
                                    <a href="javascript: alert('Charge is the amount of service fee to add when under the max value.')" class="infomark">?</a>Charge</th>
                                <td>
                                    <asp:TextBox Width="350px" ID="txtCharge" runat="server" Text='<%#Bind("Charge","{0:n2}") %>' MaxLength="6" />
                               </td>
                            </tr>
                            <tr>
                                <th>
                                    <asp:CompareValidator ID="CompareValidator3" CssClass="validator" ValidationGroup="editor" Display="Dynamic" 
                                        ControlToValidate="txtPct" Operator="DataTypeCheck" Type="Double" runat="server" 
                                        ErrorMessage="Please enter a valid percentage."></asp:CompareValidator>
                                    <asp:RangeValidator ID="RangeValidator3" CssClass="validator" ValidationGroup="editor" Display="Dynamic"
                                        ControlToValidate="txtPct" MinimumValue=".0" MaximumValue=".99" Type="Double" runat="server" 
                                        ErrorMessage="Please enter a valid percentage. (between .00 and .99)" /> 
                                    <a href="javascript: alert('Percentage is the additional amount applied to the service fee.')" class="infomark">?</a>Percentage</th>
                                <td>
                                    <asp:TextBox Width="350px" ID="txtPct" runat="server" Text='<%#Bind("Percentage","{0:n2}") %>' MaxLength="8" /> (Between .00 and .99)
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
                        <h3 class="entry-title">Adding A New Service Charge...</h3>
                         <table border="0" cellpadding="0" cellspacing="0" width="100%" class="edittabl">
                            <tr>
                                <th>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" CssClass="validator" ValidationGroup="editor" 
                                        ErrorMessage="MaxValue is required" Display="Static" ControlToValidate="txtMax">*</asp:RequiredFieldValidator>
                                    <asp:CompareValidator ID="CompareValidator1" CssClass="validator" ValidationGroup="editor" Display="Dynamic" 
                                        ControlToValidate="txtMax" Operator="DataTypeCheck" Type="Double" runat="server" ErrorMessage="Please enter a valid max value."></asp:CompareValidator>
                                    <a href="javascript: alert('Max Value is the maximum value that the charge will be applied to.')" class="infomark">?</a>Max Value</th>
                                <td style="width:100%;"><asp:TextBox Width="350px" ID="txtMax" runat="server" Text='<%#Bind("MaxValue","{0:n2}") %>' MaxLength="6" />
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" CssClass="validator" ValidationGroup="editor" 
                                        ErrorMessage="Charge is required" Display="Static" ControlToValidate="txtCharge">*</asp:RequiredFieldValidator>
                                    <asp:CompareValidator ID="CompareValidator2" CssClass="validator" ValidationGroup="editor" Display="Dynamic" 
                                        ControlToValidate="txtCharge" Operator="DataTypeCheck" Type="Double" runat="server" ErrorMessage="Please enter a valid charge."></asp:CompareValidator>
                                    <a href="javascript: alert('Charge is the amount of service fee to add when under the max value.')" class="infomark">?</a>Charge</th>
                                <td>
                                    <asp:TextBox Width="350px" ID="txtCharge" runat="server" Text='<%#Bind("Charge","{0:n2}") %>' MaxLength="6" />
                               </td>
                            </tr>
                            <tr>
                                <th>
                                    <asp:CompareValidator ID="CompareValidator3" CssClass="validator" ValidationGroup="editor" Display="Dynamic" 
                                        ControlToValidate="txtPct" Operator="DataTypeCheck" Type="Double" runat="server" ErrorMessage="Please enter a valid percentage."></asp:CompareValidator>
                                    <asp:RangeValidator ID="RangeValidator3" CssClass="validator" ValidationGroup="editor" Display="Dynamic"
                                         ControlToValidate="txtPct" MinimumValue=".0" MaximumValue=".99" Type="Double" runat="server" ErrorMessage="Please enter a valid percentage. (between .00 and .99)" /> 
                                    <a href="javascript: alert('Percentage is the additional amount applied to the service fee.')" class="infomark">?</a>Percentage</th>
                                <td><asp:TextBox Width="350px" ID="txtPct" runat="server" Text='<%#Bind("Percentage","{0:n2}") %>' MaxLength="4" /> (Between .00 and .99)
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

