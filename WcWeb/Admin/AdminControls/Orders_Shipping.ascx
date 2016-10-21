<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Orders_Shipping.ascx.cs" Inherits="WillCallWeb.Admin.AdminControls.Orders_Shipping" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register src="~/Components/Util/CalendarClock.ascx" tagname="CalendarClock" tagprefix="uc1" %>
<div id="orderview">
    <div id="shpng">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="jqhead rounded">
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" class="edittabl" style="margin:0;">
                        <tr>
                            <th>Shipping</th>
                            <th>&nbsp;</th>
                            <td style="width:100%;">
                                <asp:Button ID="btnShip" runat="server" CssClass="btnmed" Width="80px" Text="Shipping" 
                                    CommandName="shipping" OnClick="btnLink_Click" CausesValidation="false" />
                                <asp:Button ID="btnInvoice" runat="server" CssClass="btnmed" Width="80px" Text="Invoice" 
                                    CommandName="invoice" OnClick="btnLink_Click" CausesValidation="false" />
                                <asp:Button ID="btnCustSales" runat="server" CssClass="btnmed" Width="80px" Text="Cust Sales" 
                                    CommandName="custsales" OnClick="btnLink_Click" CausesValidation="false" />
                                <%if (this.Page.User.IsInRole("Administrator")){%>
                                <asp:Button ID="btnRefund" runat="server" CssClass="btnmed" Width="80px" Text="Refund" 
                                    CommandName="refund" OnClick="btnLink_Click" CausesValidation="false" />
                                <asp:Button ID="btnExchange" runat="server" CssClass="btnmed" Width="80px" Text="Exchange" 
                                    CommandName="exchange" OnClick="btnLink_Click" CausesValidation="false" />
                                <%} %>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="jqpnl rounded" style="margin-bottom:2px;">
                    <h3 class="entry-title"><asp:Literal ID="litUserEditor" runat="server" /></h3>
                </div>
                <asp:ValidationSummary HeaderText="The following errors ocurred:" ID="ValidationSummary1" runat="server" 
                    ValidationGroup="order" CssClass="validationsummary" Width="100%" />
                <div class="jqedt rounded">
                    <asp:GridView Width="100%" ID="GridInvoice" runat="server" DataKeyNames="Id" AutoGenerateColumns="False" 
                        OnDataBinding="GridInvoice_DataBinding" CssClass="lsttbl" EnableViewState="false" >
                        <Columns>
                            <asp:BoundField DataField="InvoiceDate" HeaderText="Invoice Details" HtmlEncode="false" DataFormatString="{0:MM/dd/yyyy hh:mm tt}" 
                                HeaderStyle-HorizontalAlign="left" />
                            <asp:BoundField DataField="PurchaseEmail" HeaderText="Email" HeaderStyle-HorizontalAlign="Left" />
                            <asp:TemplateField HeaderText="Ship To Name" HeaderStyle-HorizontalAlign="Left">
                                <ItemTemplate><%#Eval("InvoiceBillShip.FullName_Working") %></ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="InvoiceStatus" HeaderText="Status" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="TotalPaid" HeaderText="Paid" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="ShippingAndHandling" HeaderText="Shipping" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="TotalRefunds" HeaderText="Refunds" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="NetPaid" DataFormatString="{0:c}" HeaderText="Net Paid" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="MarketingKeys" HeaderText="Marketing Keys" HeaderStyle-HorizontalAlign="Left" />
                        </Columns>
                    </asp:GridView>
                    <asp:GridView Width="100%" ID="GridItems" runat="server" DataKeyNames="Id" AutoGenerateColumns="False" 
                        OnDataBinding="GridItems_DataBinding" CssClass="lsttbl"
                        OnRowDataBound="GridItems_RowDataBound" onrowcommand="GridItems_RowCommand" >
                        <Columns>
                            <asp:TemplateField HeaderText="Items" HeaderStyle-HorizontalAlign="left" ItemStyle-Width="50%" >
                                <ItemTemplate>
                                    <asp:Literal ID="litDescription" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="TShipItemId" HeaderText="Ship Id" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="iQuantity" HeaderText="Qty" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="mPrice" DataFormatString="{0:n}" HtmlEncode="false" HeaderText="Price" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="mServiceCharge" DataFormatString="{0:n}" HtmlEncode="false" HeaderText="Svc" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="mLineItemTotal" DataFormatString="{0:n}" HtmlEncode="false" HeaderText="Total" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="PurchaseAction" HeaderText="Status" ItemStyle-HorizontalAlign="Center" />
                            <asp:CheckBoxField DataField="bRTS" HeaderText="RTS" ItemStyle-HorizontalAlign="center" />
                            <asp:TemplateField HeaderText="Shipped On" ItemStyle-HorizontalAlign="Center" ItemStyle-Wrap="false" >
                                <ItemTemplate>
                                    <asp:Literal ID="litShipped" runat="server" />&nbsp;
                                    <asp:Button ID="btnClearDate" runat="server" CommandName="cleardate" CommandArgument='<%#Eval("Id") %>' Text="rst" 
                                        OnClientClick="return confirm('This will clear the shipping date for this item. Are you sure you want to proceed?');" 
                                        ToolTip="Clear the shipping date for this item." CssClass="btnmed" CausesValidation="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Ship Method" ItemStyle-HorizontalAlign="Center" ItemStyle-Wrap="false" >
                                <ItemTemplate>
                                    <%#Eval("ShippingMethod") %>&nbsp;
                                    <asp:Button ID="btnClearMethod" runat="server" CommandName="clearmethod" CommandArgument='<%#Eval("Id") %>' Text="rst" 
                                        OnClientClick="return confirm('This will reset tickets only, to Will Call. It will also reset the ship date. Are you sure you want to proceed? Please note that to change the pickup name, you must use the save button on the VIEW ORDER page.');" 
                                        ToolTip="This will reset tickets only, to Will Call." CssClass="btnmed" CausesValidation="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:GridView Width="100%" ID="GridShip" runat="server" DataKeyNames="Id" AutoGenerateColumns="False" CssClass="lsttbl" 
                        OnDataBinding="GridShip_DataBinding" OnRowCreated="GridShip_RowCreated" 
                        OnRowDataBound="GridShip_RowDataBound" onrowcommand="GridShip_RowCommand" >
                        <SelectedRowStyle CssClass="selected" />
                        <EmptyDataTemplate>
                            <div class="lstempty">No Shipment Requests In This Order</div>
                        </EmptyDataTemplate>
                        <Columns>
                            <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Button ID="btnFulfill" runat="server" CommandName="fulfill" Text="Fulfill" 
                                        CssClass="btnmed" CausesValidation="false" />
                                    <asp:CustomValidator ID="CustomValidation" runat="server" ValidationGroup="order" CssClass="validator" 
                                        Display="Static" ErrorMessage="CustomValidator">*</asp:CustomValidator>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="50%">
                                <HeaderTemplate>Shipment Requests</HeaderTemplate>
                                <ItemTemplate>
                                    <div style="font-size:14px;line-height:24px;"><%#Eval("Id") %> - <asp:Literal ID="litShipping" runat="server" /></div>
                                    <asp:Literal ID="litReturned" runat="server" />
                                    <div style="margin-left: 1em; background-color: #f1f1f1; border: silver solid 1px;">
                                        <asp:Literal ID="litItemList" runat="server" />
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="dtDateOfShow" HtmlEncode="false" DataFormatString="{0:MM/dd/yyyy}" HeaderText="Est Ship" ItemStyle-HorizontalAlign="Center" />
                            <asp:TemplateField HeaderText="Shipped" ItemStyle-HorizontalAlign="Center" ItemStyle-Wrap="false" >
                                <ItemTemplate>
                                    <asp:Literal ID="litShipped" runat="server" />&nbsp;
                                    <asp:Button ID="btnClearDate" runat="server" Visible="false" CommandName="cleardate" CommandArgument='<%#Eval("Id") %>' Text="rst" 
                                        OnClientClick="return confirm('This will clear the shipping date for this shipment and will mark all items within this shipment as not shipped. Are you sure you want to proceed?');" 
                                        ToolTip="This will clear the shipping date for this shipment and will mark all items within this shipment as not shipped." 
                                        CssClass="btnmed" CausesValidation="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="PurchaseAction" HeaderText="Status" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="LineItemTotal" HtmlEncode="false" DataFormatString="{0:c}" HeaderText="Paid" ItemStyle-HorizontalAlign="Center" />
                            <asp:CheckBoxField DataField="bRTS" HeaderText="RTS" ItemStyle-HorizontalAlign="center" />
                        </Columns>
                    </asp:GridView>
                </div>                
                <div class="jqedt rounded" style="margin-top:4px;">
                    <asp:GridView Width="100%" ID="GridShipments" runat="server" DataKeyNames="Id" AutoGenerateColumns="False" CssClass="lsttbl" 
                        OnDataBinding="GridShipments_DataBinding" 
                        OnDataBound="GridShipments_DataBound" 
                        OnRowDeleting="GridShipments_RowDeleting" 
                        OnSelectedIndexChanged="GridShipments_SelectedIndexChanged" 
                        OnRowDataBound="GridShipments_RowDataBound" 
                        OnRowCommand="GridShipments_RowCommand" >
                        <SelectedRowStyle CssClass="selected" />
                        <EmptyDataTemplate>
                            <div class="lstempty">No Shipments In This Order</div>
                        </EmptyDataTemplate>
                        <Columns>
                            <asp:TemplateField ItemStyle-Wrap="false">
                                <ItemTemplate>
                                    <asp:LinkButton Width="20px" Id="btnEdit" CssClass="btnselect" ToolTip="Select" runat="server" CommandName="Select" 
                                        CommandArgument='<%#Eval("Id") %>' CausesValidation="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Created" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <div>
                                        <%#Eval("DateCreated", "{0:MM/dd/yy}")%>
                                    </div>
                                    <div style="padding-bottom:12px;">
                                        <%#Eval("Context")%>
                                    </div>
                                    <div>
                                        <asp:Literal ID="litBatch" runat="server" EnableViewState="false" />
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Ship Address" HeaderStyle-HorizontalAlign="left" ItemStyle-Width="50%" >
                                <ItemTemplate>
                                    <asp:Literal ID="litAddress" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="PackingList" HeaderStyle-HorizontalAlign="left" ItemStyle-Width="50%" >
                                <ItemTemplate>
                                    <asp:Literal ID="litPacking" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Actual Shipments" HeaderStyle-HorizontalAlign="left" ItemStyle-Wrap="false" >
                                <ItemTemplate>
                                    <asp:Literal ID="litShipped" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:CheckBoxField DataField="IsLabelPrinted" HeaderText="Printed" ItemStyle-HorizontalAlign="center" />
                            <asp:TemplateField ItemStyle-Wrap="false" ItemStyle-VerticalAlign="top" >
                               <ItemTemplate>
                                   <asp:CustomValidator ID="RowValidator" runat="Server" ValidationGroup="order" CssClass="validator"
                                      Display="Static" >*</asp:CustomValidator>
                                    <asp:Button CssClass="btntny" ToolTip="Processes a returned ticket. Changes ship method to will call" 
                                        ID="btnReturn" runat="server" CausesValidation="false" CommandName="RTS" Text="Return To Sender" 
                                        OnClientClick="return confirm('This processes a returned shipment and changes ticket items to WillCall. Are you sure you want to proceed?');"/>
                                   <asp:LinkButton Width="20px" Id="btnDelete" CssClass="btndelete lastinrow" runat="server" CommandName="Delete" ToolTip="Delete" 
                                        CommandArgument='<%#Eval("Id") %>' causesValidation="false"
                                       OnClientClick='return confirm("Are you sure you want to delete this row?")' />
                               </ItemTemplate>
                           </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:FormView ID="FormDetails" CssClass="noborder" runat="server" DefaultMode="Edit" Width="100%" DataKeyNames="Id" 
                        OnDataBinding="FormDetails_DataBinding" OnDataBound="FormDetails_DataBound" OnModeChanging="FormDetails_ModeChanging" 
                        OnModeChanged="FormDetails_ModeChanged" GridLines="None" OnItemCommand="FormDetails_ItemCommand" 
                        OnItemInserting="FormDetails_ItemInserting" OnItemUpdating="FormDetails_ItemUpdating" >
                        <EmptyDataTemplate>
                            <table border="0" cellspacing="0" cellpadding="0" width="100%" style="margin:0 0 6px 0;" class="edittabl">
                                <tr>
                                    <th style="text-align:left;">No Shipments In This Order</th>
                                    <td style="padding-right:22px;">
                                        <asp:Button Id="btnNew" CssClass="btnmed" Width="140px" CausesValidation="false" runat="server" 
                                            CommandName="New" Text="Add A Generic Shipment"
                                            OnClientClick="return confirm('This will add a shipment that is not linked to a shipment request - therefore it will not mark a shipment request as shipped. Do not combine this with the fullfill buttons above. You may want to use this in the case of an exchange or an item missed in an original shipment. Are you sure you want to continue?');" 
                                            ToolTip="This will add a shipment that is not linked to a shipment request - therefore not marking a shipment request as shipped. Be sure you should not be using the fullfill buttons above. You may want to use this in the case of an exchange or an item missed in an original shipment." />
                                    </td>
                                    <td class="jqinstruction rounded">
                                        <ul>
                                            <li>Generic adds a shipment that is not linked to a shipment request - therefore not marking a shipment request as shipped.</li> 
                                            <li>Be sure you should not be using the fullfill buttons above.</li>
                                            <li>You may want to use this in the case of an exchange or an item missed in an original shipment.</li>
                                        </ul>
                                    </td>
                                </tr>
                            </table>
                            <br />
                        </EmptyDataTemplate>
                        <InsertItemTemplate>
                            <table border="0" cellspacing="0" cellpadding="0" width="100%" class="edttbl">
                                <tr>
                                    <th>Contents</th>
                                    <td rowspan="2" style="white-space:nowrap;">
                                        <asp:RadioButtonList ID="rdoShipContext" RepeatLayout="flow" AutoPostBack="true" runat="server" 
                                            OnDataBinding="rdoShipContext_DataBinding" 
                                            OnDataBound="rdoShipContext_DataBound" 
                                            OnSelectedIndexChanged="rdoShipContext_SelectedIndexChanged" />
                                    </td>
                                    <th>* Current Shipping</th>
                                    <td><asp:TextBox ID="txtGenericMethod" runat="server" Width="200" /></td>
                                    <th style="text-align:left;width: 100%">
                                        <span>Specify Method (handling fees NOT added)</span>
                                        <asp:Button ID="btnCheckRates" runat="server" CssClass="btntny" width="100px"
                                            Text="Update Ship Rates" CommandName="checkrates" CausesValidation="false" />
                                    </th>
                                </tr>
                                <tr>
                                    <td style="padding-top:4px;">
                                        <asp:Button Id="btnInsert" CssClass="btntny" Width="100px" ValidationGroup="order" 
                                            CausesValidation="false" runat="server" CommandName="Insert" Text="Record Shipment"
                                            onClientClick="return confirm('Have you reviewed this shipment? Have you specified a shipping method?');" />
                                        <asp:CustomValidator ID="CustomValidation" runat="server" ValidationGroup="order" CssClass="invisible" 
                                            Display="Static" ErrorMessage="CustomValidator">*</asp:CustomValidator>
                                        <asp:Button Id="btnCancel" CssClass="btntny" Width="100px" CausesValidation="false" runat="server" 
                                            CommandName="Cancel" Text="Cancel" />
                                    </td>
                                    <td style="vertical-align:top;padding-top:4px;" colspan="2">
                                        ***If doing a no charge shipment or combining shipments,<br />&nbsp; leave a note here<br />
                                    </td>
                                    <td style="padding-bottom:4px;">
                                        <asp:RadioButtonList ID="rdoMethods" Enabled="false" RepeatDirection="Vertical" RepeatLayout="Flow" runat="server"                                         
                                            OnDataBinding="rdoMethods_DataBinding"
                                            OnDataBound="rdoMethods_DataBound" />
                                    </td>
                                </tr>
                                <tr><th>* Packing List</th>
                                    <td colspan="4" style="width: 100%; background-color: #f1f1e1;">
                                        <asp:CheckBoxList RepeatLayout="Flow" ID="chkItems" runat="server" OnDataBinding="chkItems_DataBinding" /></td>
                                </tr>
                                <tr><th>Additional Items</th>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtAdditional" Width="373px" Height="25px" TextMode="MultiLine" runat="server" 
                                            Text='<%#Eval("PackingAdditional") %>' />
                                    </td>
                                    <td>Enter items in here that may be tacked onto an order...ie: autographed cds, give-aways with an order, etc. that are not selected/listed in the packing list above.</td>
                                </tr>
                                <tr>
                                    <th>Email</th>
                                    <td style="vertical-align:middle;"><%= Atx.CurrentInvoiceRecord.PurchaseEmail %></td>
                                    <th>Company</th>
                                    <td><asp:TextBox ID="txtCompany" runat="server" Text='<%#Eval("Company") %>' MaxLength="100" /></td>
                                    <td style="width: 100%"><asp:Literal ID="litDisplayEmail" runat="server" /></td>
                                </tr>
                                <tr>
                                    <th>* First Name</th>
                                    <td><asp:TextBox ID="txtFirst" runat="server" Text='<%#Eval("FirstName") %>' MaxLength="50" /></td>
                                    <th>* Last Name</th>
                                    <td><asp:TextBox ID="txtLast" runat="server" Text='<%#Eval("LastName") %>' MaxLength="50" /></td>
                                    <td><asp:Literal ID="litFullName" runat="server" /></td>
                                </tr>
                                <tr>
                                    <th>* Address1</th>
                                    <td><asp:TextBox ID="txtAddress1" runat="server" Text='<%#Eval("Address1") %>' MaxLength="60" /></td>
                                    <th>Address2</th>
                                    <td colspan="2"><asp:TextBox ID="txtAddress2" runat="server" Text='<%#Eval("Address2") %>' MaxLength="60" /></td>
                                </tr>
                                <tr>
                                    <th>* City</th>
                                    <td><asp:TextBox ID="txtCity" runat="server" Text='<%#Bind("city") %>' MaxLength="40" /></td>
                                    <th>* State/Province</th>
                                    <td colspan="2"><asp:TextBox ID="txtState" runat="server" Text='<%#Bind("state") %>' MaxLength="40" /></td>
                                </tr>
                                <tr>
                                    <th>* Postal Code</th>
                                    <td><asp:TextBox ID="txtZip" runat="server" Text='<%#Eval("PostalCode") %>' MaxLength="20" /></td>
                                    <th>* Country</th>
                                    <td><asp:TextBox ID="txtCountry" runat="server" Text='<%#Eval("Country") %>' MaxLength="2" /></td>
                                    <td>(use 2 character code for country)</td>
                                </tr>
                                <tr>
                                    <th>* Phone</th>
                                    <td colspan="4">
                                        <asp:TextBox ID="txtPhone" runat="server" Text='<%#Eval("Phone") %>' MaxLength="25" />
                                    </td>
                                </tr>
                                <tr>
                                    <th>Ship Message:</th>
                                    <td colspan="4">
                                        <asp:TextBox ID="txtMessage" TextMode="multiLine" Height="25" Width="85%" runat="server" 
                                            Text='<%#Eval("ShipMessage") %>' MaxLength="1000" />
                                    </td>
                                </tr>
                                <tr><th>Tracking:</th>
                                    <td colspan="4">
                                        <asp:TextBox ID="txtTracking" TextMode="multiLine" Height="25" Width="85%" runat="server" 
                                            Text='<%#Eval("TrackingInformation") %>' MaxLength="500" />
                                    </td>
                                </tr>
                                <tr>
                                    <th>Amount Charged:</th>
                                    <td><asp:TextBox ID="txtCharged" runat="server" MaxLength="8" /></td>
                                    <th>Actual Shipping:</th>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtShip" runat="server" Text='<%#Eval("ShippingActual", "{0:n}") %>' MaxLength="8" />
                                    </td>
                                </tr>
                                <tr><th>Actual Weight:</th>
                                    <td colspan="5">
                                        <asp:TextBox ID="txtActualWeight" runat="server" Text='<%#Eval("WeightActual", "{0:n}") %>' MaxLength="8" />
                                    </td>
                                </tr>
                            </table>
                        </InsertItemTemplate>
                        <EditItemTemplate>
                            <table border="0" cellspacing="0" cellpadding="0" width="100%" style="margin-bottom:12px;">
                                <tr>
                                    <td style="white-space:nowrap;padding-left:12px;">
                                        <asp:Button Id="btnPrint" CssClass="btnmed lastinrow" Width="100px" runat="server" 
                                            Text="Print Pack List" CausesValidation="false" />
                                        <asp:Button Id="btnSave" CssClass="btnmed" Width="100px" ValidationGroup="order"  CausesValidation="false" 
                                            runat="server" CommandName="Update" Text="Save Changes" />
                                        <asp:Button Id="btnCancel" CssClass="btnmed" Width="100px" CausesValidation="false" runat="server" 
                                            CommandName="Cancel" Text="Cancel" />
                                        <asp:Button Id="btnShip" CssClass="btnmed" Width="100px" CausesValidation="false" runat="server" 
                                            CommandName="Ship" Text="Set Date To Now" 
                                            OnClientClick="return confirm('This will reset the ship date to the current date and time and will mark all items in this shipment as shipped at that time. Are you sure you want to proceed?');" 
                                            ToolTip="Sets the ship date to the current date and time and will mark all items in this shipment as shipped at that time." />
                                        <asp:Button Id="btnNew" CssClass="btnmed" Width="140px" CausesValidation="false" runat="server" 
                                            CommandName="New" Text="Add A Generic Shipment"
                                            OnClientClick="return confirm('This will add a shipment that is not linked to a shipment request - therefore it will not mark a shipment request as shipped. Do not combine this with the fullfill buttons above. You may want to use this in the case of an exchange or an item missed in an original shipment. Are you sure you want to continue?');" 
                                            ToolTip="This will add a shipment that is not linked to a shipment request - therefore not marking a shipment request as shipped. Are you sure you should not be using the fullfill buttons above? Use this in the case of an exchange or an item missed in an original shipment." />
                                    
                                    </td>
                                    <th style="width:100%;text-align:left;padding:0 22px;white-space:nowrap;">
                                        <asp:CustomValidator 
                                            ID="CustomValidation" runat="server" ValidationGroup="order" CssClass="invisible" 
                                            Display="Static" ErrorMessage="CustomValidator">*</asp:CustomValidator>
                                        <%#Eval("ReferenceNumber") %> - <%#Eval("context") %>
                                    </th>
                                </tr>
                            </table>
                            <table border="0" cellspacing="0" cellpadding="0" width="100%" class="edttbl">
                                <tr>
                                    <th>Status</th>
                                    <td colspan="99"><%#Eval("Status") %></td>
                                </tr>
                                <tr>
                                    <th>BatchId</th>
                                    <td><asp:Literal ID="litBatchId" runat="server" EnableViewState="false" /></td>
                                    <th>Batch Name</th>
                                    <td colspan="3"><asp:Literal ID="litBatchName" runat="server" EnableViewState="false" /></td>
                                </tr>
                                <tr>
                                    <th>Contents</th>
                                    <td><%#Eval("vcContext") %></td>
                                    <th>Ship Method</th>
                                    <td><%#Eval("ShipMethod") %></td>
                                    <td style="width: 100%">
                                        <asp:CheckBox ID="chkPrinted" runat="server" Checked='<%#Eval("IsLabelPrinted") %>' Font-Bold="true" Text="Is Label Printed: " TextAlign="left" />
                                    </td>
                                </tr>
                                <tr>
                                    <th style="vertical-align:text-top;padding-top:4px;">* Packing List</th>
                                    <td colspan="4" style="width: 100%;">
                                        <div style="border:solid #000 1px;padding:3px;background-color:#f1f1f1;">
                                            <asp:Literal ID="litPackList" runat="server" />
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <th style="vertical-align:text-top;padding-top:4px;">* Packing Notes</th>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtAdditional" Width="373px" Height="25px" TextMode="MultiLine" runat="server" 
                                            Text='<%#Eval("PackingAdditional") %>' />
                                    </td>
                                    <td>Enter items in here that may be tacked onto an order...ie: autographed cds, give-aways with an order, etc. that are not selected/listed in the packing list above.</td>
                                </tr>
                                <tr>
                                    <th>Email</th>
                                    <td><%= Atx.CurrentInvoiceRecord.PurchaseEmail %></td>
                                    <th>Company</th>
                                    <td><asp:TextBox ID="txtCompany" runat="server" Text='<%#Eval("CompanyName") %>' MaxLength="100" /></td>
                                    <td><%= Atx.CurrentInvoiceRecord.PurchaseEmail %></td>
                                </tr>
                                <tr>
                                    <th>* First Name</th>
                                    <td><asp:TextBox ID="txtFirst" runat="server" Text='<%#Eval("FirstName") %>' MaxLength="50" /></td>
                                    <th>* Last Name</th>
                                    <td><asp:TextBox ID="txtLast" runat="server" Text='<%#Eval("LastName") %>' MaxLength="50" /></td>
                                    <td><%#Eval("FirstName") %> <%#Eval("LastName") %></td>
                                </tr>
                                <tr>
                                    <th>* Address1</th>
                                    <td><asp:TextBox ID="txtAddress1" runat="server" Text='<%#Eval("Address1") %>' MaxLength="60" /></td>
                                    <th>Address2:</th>
                                    <td colspan="2"><asp:TextBox ID="txtAddress2" runat="server" Text='<%#Eval("Address2") %>' MaxLength="60" /></td>
                                </tr>
                                <tr><th>* City</th>
                                    <td><asp:TextBox ID="txtCity" runat="server" Text='<%#Eval("City") %>' MaxLength="40" /></td>
                                    <th>* State/Province</th>
                                    <td colspan="2"><asp:TextBox ID="txtState" runat="server" Text='<%#Eval("StateProvince") %>' MaxLength="40" /></td>
                                </tr>
                                <tr>
                                    <th>* Postal Code</th>
                                    <td><asp:TextBox ID="txtZip" runat="server" Text='<%#Eval("PostalCode") %>' MaxLength="20" /></td>
                                    <th>* Country</th>
                                    <td><asp:TextBox ID="txtCountry" runat="server" Text='<%#Eval("Country") %>' MaxLength="2" /></td>
                                    <td>(use 2 character code for country)</td></tr>
                                <tr>
                                    <th>* Phone</th>
                                    <td colspan="4"><asp:TextBox ID="txtPhone" runat="server" Text='<%#Eval("Phone") %>' MaxLength="25" /></td>
                                </tr>
                                <tr>
                                    <th style="vertical-align:text-top;padding-top:4px;">Ship Message</th>
                                    <td colspan="4"><asp:TextBox ID="txtMessage" TextMode="multiLine" Height="50" Width="85%" 
                                        runat="server" Text='<%#Eval("ShipMessage") %>' MaxLength="1000" /></td>
                                </tr>
                                <tr><th>Tracking</th>
                                    <td colspan="4">
                                        <asp:TextBox ID="txtTracking" Width="85%" runat="server" 
                                            Text='<%#Eval("TrackingInformation") %>' MaxLength="500" /></td>
                                </tr>
                                <tr><th>Amount Charged</th>
                                    <td>
                                        <asp:TextBox ID="txtCharged" runat="server" Text='<%#Eval("ShippingCharged") %>' MaxLength="8" />
                                    </td>
                                    <th>Actual Charges</th>
                                    <td colspan="2"><asp:TextBox ID="txtShip" runat="server" Text='<%#Eval("ShippingActual", "{0:n}") %>' MaxLength="8" /></td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <th>Calc'd Weight</th>
                                    <td>
                                        <asp:TextBox ID="txtChargedWeight" ReadOnly="true" runat="server" Text='<%#Eval("WeightCalculated", "{0:n}") %>' 
                                            MaxLength="8" /></td>
                                    <th>Actual Weight</th>
                                    <td><asp:TextBox ID="txtActualWeight" runat="server" Text='<%#Eval("WeightActual", "{0:n}") %>' MaxLength="8" /></td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <th>Date Shipped</th>
                                    <td colspan="2">
                                        <uc1:CalendarClock ID="clockShip" runat="server" UseTime="true" SelectedDate='<%#Eval("DateShipped") %>' 
                                            UseReset="false" DefaultValue='<%#DateTime.MaxValue %>' />
                                    </td>
                                    <td colspan="2">
                                        <asp:Button ID="btnClearDate" runat="server" CssClass="btnmed" Text="Clear Date" 
                                            CommandName="cleardate" CommandArgument='<%#Eval("Id") %>' CausesValidation="false" 
                                            OnClientClick="return confirm('This will clear the ship date for this shipment and will mark all items in this shipment as not shipped. Are you sure you want to proceed?');" 
                                            ToolTip="Clear the shipping date for this shipment and its package contents." />
                                    </td>
                                </tr>
                            </table>
                        </EditItemTemplate>
                    </asp:FormView>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</div>