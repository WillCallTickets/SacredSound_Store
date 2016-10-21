<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Charge_Statement.ascx.cs" Inherits="WillCallWeb.Admin.AdminControls.Charge_Statement" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<div id="monthlycharges">
    <div class="jqhead rounded">
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="hedtbl">
            <tr>
                <th>MONTHLY CHARGES REPORT</th>
                <th style="width:100%;"><asp:DropDownList ID="ddlYear" runat="server" OnDataBinding="ddlYear_DataBinding" OnDataBound="ddlYear_DataBound" 
                        AutoPostBack="true" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged" CssClass="fxddl" /></th>
                <td >shipping calculations need to be reimplemented in SP - 080726 - rk<br />
                    some issues with adding/deleting charges - does not allow nulls
                </td>
            </tr>
        </table>
    </div>
    <asp:ValidationSummary ID="ValidationSummary2" CssClass="validationsummary" HeaderText="" 
        ValidationGroup="charge" runat="server" />
    <asp:GridView ID="GridView1" Width="100%" EnableViewState="false" CssClass="lsttbl" 
        runat="server" AllowPaging="False" ShowFooter="true" 
        AutoGenerateColumns="False" DataKeyNames="Id,iMonth" DataSourceID="SqlMonths" 
        PageSize="12"
        ondatabinding="GridView1_DataBinding" ondatabound="GridView1_DataBound" 
        onrowdatabound="GridView1_RowDataBound" 
        onselectedindexchanged="GridView1_SelectedIndexChanged">
        <RowStyle HorizontalAlign="center" />
        <FooterStyle HorizontalAlign="center" />
        <SelectedRowStyle CssClass="selected" />
        <EmptyDataTemplate>
            <div class="lstempty">No Data Available</div>
        </EmptyDataTemplate>
        <Columns>
            <asp:TemplateField HeaderText="Date" ItemStyle-Wrap="false" 
                HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="left" 
                FooterStyle-HorizontalAlign="Left" FooterText="Totals" FooterStyle-Font-Bold="true" >
                <ItemTemplate>
                    <asp:LinkButton ID="btnSelect" runat="server" CssClass="fxbtn" CommandName="Select" ToolTip="Select" CausesValidation="false"
                        CommandArgument='<%#Eval("Id") %>' Text='<%#Eval("MonthYear") %>' />
                </ItemTemplate>
                <FooterTemplate>Totals</FooterTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="SalesQty" FooterText='<%this._salesQty %>' HeaderText="Sales" />
            <asp:BoundField DataField="SalesQtyPct" HeaderText="Per" DataFormatString="{0:n4}" HtmlEncode="false" />
            <asp:BoundField DataField="SalesQtyPortion" HeaderText="$Sale" DataFormatString="{0:n2}" HtmlEncode="false" />
            <asp:BoundField DataField="RefundQty" HeaderText="Refs" />
            <asp:BoundField DataField="RefundQtyPct" HeaderText="Per" DataFormatString="{0:n4}" HtmlEncode="false" />
            <asp:BoundField DataField="RefundQtyPortion" HeaderText="$Ref" DataFormatString="{0:n2}" HtmlEncode="false" />
            <asp:BoundField DataField="Gross" HeaderText="Gross" DataFormatString="{0:n2}" HtmlEncode="false" />
            <asp:BoundField DataField="GrossPct" HeaderText="%" DataFormatString="{0:n4}" HtmlEncode="false" />
            <asp:BoundField DataField="GrossPortion" HeaderText="$Gross" DataFormatString="{0:n2}" HtmlEncode="false" />
            <asp:BoundField DataField="TicketPortion" HeaderText="$Tkt" DataFormatString="{0:n2}" HtmlEncode="false" />
            <asp:BoundField DataField="MerchPortion" HeaderText="$Merch" DataFormatString="{0:n2}" HtmlEncode="false" />
            <asp:BoundField DataField="ShipPortion" HeaderText="$Ship" DataFormatString="{0:n2}" HtmlEncode="false" />
            <asp:BoundField DataField="MailerPortion" HeaderText="$Mlr" DataFormatString="{0:n2}" HtmlEncode="false" />
            <asp:BoundField DataField="HourlyPortion" HeaderText="Hourly" DataFormatString="{0:n2}" HtmlEncode="false" />
            <asp:BoundField DataField="Discount" HeaderText="Disc" DataFormatString="{0:n2}" HtmlEncode="false" />
            <asp:BoundField DataField="LineTotal" HeaderText="Total" DataFormatString="{0:n2}" HtmlEncode="false" />
            <asp:BoundField DataField="AmountPaid" HeaderText="Paid" DataFormatString="{0:n2}" HtmlEncode="false" />
        </Columns>
     </asp:GridView>
    <asp:FormView Width="100%" ID="FormView1" runat="server" DefaultMode="Edit" 
        DataSourceID="SqlEntity" DataKeyNames="Id" 
        ondatabound="FormView1_DataBound" onitemcommand="FormView1_ItemCommand" >
        <EmptyDataTemplate>
            <div class="jqpanel1 rounded">
                <span class="lstempty">No Data For Selected Month</span>
                <asp:Button ID="btnCreate" CssClass="btntny" CausesValidation="false" runat="server" CommandName="Create" Text="Create Data" />
            </div>
        </EmptyDataTemplate>
        <EditItemTemplate>
            <div class="jqhead rounded">
                <div class="sectitle"><asp:Literal ID="litTitle" runat="server" />
                    <asp:Button ID="btnSave" CssClass="btntny" CausesValidation="true" ValidationGroup="charge" runat="server" CommandName="Update" Text="Save" />
                    <asp:Button ID="btnDelete" CssClass="btntny" CausesValidation="false" runat="server" CommandName="Delete" Text="Delete" OnClientClick='return confirm("Are you sure you want to delete this statement?")' />
                    <asp:Button ID="btnCancel" CssClass="btntny" CausesValidation="false" runat="server" CommandName="Cancel" Text="Cancel" />
                </div>
            </div>
            <table border="1" cellspacing="3" cellpadding="0" width="100%" class="edittabl">
                <tr>
                    <th>Sales Qty</th><td><%#Eval("SalesQty")%></td>
                    <th>Per</th>
                    <td style="white-space: nowrap;">
                        <asp:TextBox ID="txtSalesQtyPct" runat="server" MaxLength="6" Width="40px" 
                            Text='<%#Bind("SalesQtyPct", "{0:n4}") %>' />
                        (<%=Wcss.ChargeStatement._Rate_PerSale.ToString("n4") %>)
                    </td>
                    
                    <td colspan="8">&nbsp;</td>
                    <th style="text-align: right;">Portion&nbsp;</th>
                    <td><%#Eval("SalesQtyPortion", "{0:n2}") %></td>
                </tr>
                <tr>
                    <th>Refund Qty</th><td><%#Eval("RefundQty")%></td>
                    <th>Per</th>
                    <td style="white-space: nowrap;">
                        <asp:TextBox ID="txtRefundQtyPct" runat="server" MaxLength="6" Width="40px" 
                            Text='<%#Bind("RefundQtyPct", "{0:n4}") %>' />
                        (<%=Wcss.ChargeStatement._Rate_PerRefund.ToString("n4") %>)
                    </td>
                    <td colspan="8">&nbsp;</td>
                    <th style="text-align: right;">Portion&nbsp;</th>
                    <td><%#Eval("RefundQtyPortion", "{0:n2}") %></td>
                </tr>
                <tr>
                    <th>Gross</th><td><%#Eval("Gross", "{0:n2}")%></td>
                    <th>Pct</th>
                    <td style="white-space: nowrap;">
                        <asp:TextBox ID="txtGrossPct" runat="server" MaxLength="6" Width="40px" 
                            Text='<%#Bind("GrossPct", "{0:n4}") %>' />
                        (<%=Wcss.ChargeStatement._Rate_PctGrossSales.ToString("n4") %>)
                    </td>
                    <th>Threshhold</th>
                    <td colspan="7"><asp:TextBox ID="txtGrossThreshhold" runat="server" MaxLength="10" 
                        Width="70px" Text='<%#Bind("GrossThreshhold", "{0:n2}") %>' />
                        (<%=Wcss.ChargeStatement._Rate_PctGrossSalesThreshhold.ToString("n2") %>)
                    </td>
                    <th style="text-align: right;">Portion&nbsp;</th>
                    <td><%#Eval("GrossPortion", "{0:n2}")%></td>
                </tr>                        
                <tr>
                    <th>Ticket Invoice Qty</th><td><%#Eval("TicketInvoiceQty")%></td>
                    <th>Per</th>
                    <td style="white-space: nowrap;">
                        <asp:TextBox ID="txtTicketInvoicePct" runat="server" MaxLength="6" Width="40px" 
                            Text='<%#Bind("TicketInvoicePct", "{0:n4}") %>' />
                        (<%=Wcss.ChargeStatement._Rate_PerTicketInvoice.ToString("n4") %>)
                    </td>
                    <th>Ticket Units</th><td><%#Eval("TicketUnitQty")%></td>
                    <th>Per</th>
                    <td style="white-space: nowrap;">
                        <asp:TextBox ID="txtTicketUnitPct" runat="server" MaxLength="6" Width="40px" 
                            Text='<%#Bind("TicketUnitPct", "{0:n4}") %>' />
                        (<%=Wcss.ChargeStatement._Rate_PerTicketUnit.ToString("n4") %>)
                    </td>
                    <th>Ticket Sales</th><td><%#Eval("TicketSales", "{0:n2}")%></td>
                    <th>Pct</th>
                    <td style="white-space: nowrap;">
                        <asp:TextBox ID="txtTicketSalesPct" runat="server" MaxLength="6" Width="40px" 
                            Text='<%#Bind("TicketSalesPct", "{0:n4}") %>' />
                        (<%=Wcss.ChargeStatement._Rate_PctTicketSales.ToString("n4") %>)
                    </td>
                    <th style="text-align: right;">Portion&nbsp;</th>
                    <td><%#Eval("TicketPortion", "{0:n2}")%></td>
                </tr>
                <tr>
                    <th>Merch Invoice Qty</th><td><%#Eval("MerchInvoiceQty")%></td>
                    <th>Per</th>
                    <td style="white-space: nowrap;">
                        <asp:TextBox ID="txtMerchInvoicePct" runat="server" MaxLength="6" Width="40px" 
                            Text='<%#Bind("MerchInvoicePct", "{0:n4}") %>' />
                        (<%=Wcss.ChargeStatement._Rate_PerMerchInvoice.ToString("n4") %>)
                    </td>
                    <th>Merch Units</th><td><%#Eval("MerchUnitQty")%></td>
                    <th>Per</th>
                    <td style="white-space: nowrap;">
                        <asp:TextBox ID="txtMerchUnitPct" runat="server" MaxLength="6" Width="40px" 
                            Text='<%#Bind("MerchUnitPct", "{0:n4}") %>' />
                        (<%=Wcss.ChargeStatement._Rate_PerMerchUnit.ToString("n4") %>)
                    </td>
                    <th>Merch Sales</th><td><%#Eval("MerchSales", "{0:n2}")%></td>
                    <th>Pct</th>
                    <td style="white-space: nowrap;">
                        <asp:TextBox ID="txtMerchSalesPct" runat="server" MaxLength="6" Width="40px" 
                            Text='<%#Bind("MerchSalesPct", "{0:n4}") %>' />
                        (<%=Wcss.ChargeStatement._Rate_PctMerchSales.ToString("n4") %>)
                    </td>
                    <th style="text-align: right;">Portion&nbsp;</th>
                    <td><%#Eval("MerchPortion", "{0:n2}")%></td>
                </tr>
                <tr>
                    <th>Ship Units</th><td><%#Eval("ShipUnitQty")%></td>
                    <th>Pct</th>
                    <td style="white-space: nowrap;">
                        <asp:TextBox ID="txtShipUnitPct" runat="server" MaxLength="6" Width="40px" 
                            Text='<%#Bind("ShipUnitPct", "{0:n4}") %>' />
                        (<%=Wcss.ChargeStatement._Rate_PerTktShip.ToString("n4") %>)
                    </td>
                    <th>Ship Sales</th><td><%#Eval("ShipSales", "{0:n2}")%></td>
                    <th>Pct</th>
                    <td style="white-space: nowrap;">
                        <asp:TextBox ID="txtShipSalesPct" runat="server" MaxLength="6" Width="40px" 
                            Text='<%#Bind("ShipSalesPct", "{0:n4}") %>' />
                        (<%=Wcss.ChargeStatement._Rate_PctTktShipSales.ToString("n4") %>)
                    </td>
                    <td colspan="4">&nbsp;</td>
                    <th style="text-align: right;">Portion&nbsp;</th>
                    <td><%#Eval("ShipPortion", "{0:n2}")%></td>
                </tr>
                <tr>
                    <th>Subscriptions</th><td><%#Eval("SubscriptionsSent")%></td>
                    <th>Per</th>
                    <td style="white-space: nowrap;">
                        <asp:TextBox ID="txtPerSubscription" runat="server" MaxLength="6" Width="40px" 
                            Text='<%#Bind("PerSubscription", "{0:n4}") %>' />
                        (<%=Wcss.ChargeStatement._Rate_PerSubscription.ToString("n4") %>)
                    </td>
                    <th>Mails Sent</th><td><%#Eval("MailSent")%></td>
                    <th>Per</th>
                    <td style="white-space: nowrap;">
                        <asp:TextBox ID="txtPerMailSent" runat="server" MaxLength="6" Width="40px" 
                            Text='<%#Bind("PerMailSent", "{0:n8}") %>' />
                        (<%=Wcss.ChargeStatement._Rate_PerMailSent.ToString("n8") %>)
                    </td>
                    <td colspan="4">&nbsp;</td>
                    <th style="text-align: right;">Portion&nbsp;</th>
                    <td><%#Eval("MailerPortion", "{0:n2}")%></td>
                </tr>
                <tr>
                    <td colspan="13">
                        <asp:GridView ID="GridHours" Width="100%" EnableViewState="false" runat="server" AllowPaging="False" 
                            ShowFooter="false" AutoGenerateColumns="False" DataKeyNames="Id" DataSourceID="SqlHours" 
                            PageSize="25" ondatabound="GridHours_DataBound" CssClass="lsttbl" >
                            <RowStyle HorizontalAlign="Center" />
                            <FooterStyle HorizontalAlign="center" />
                            <SelectedRowStyle CssClass="selected" />
                            <Columns>
                                <asp:ButtonField CommandName="Select" ControlStyle-CssClass="btntny" ItemStyle-HorizontalAlign="left" 
                                    DataTextField="dtPerformed" DataTextFormatString="{0:MM/dd/yyyy hh:mmtt}" HeaderText="Misc Charges" 
                                    HeaderStyle-HorizontalAlign="Left" CausesValidation="false" />
                                <asp:BoundField DataField="ServiceDescription" ControlStyle-Width="100%" HeaderText="Description" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="Hours" HeaderText="Hours" />
                                <asp:BoundField DataField="Rate" DataFormatString="{0:n4}" HtmlEncode="false" HeaderText="Rate" />
                                <asp:BoundField DataField="FlatRate" DataFormatString="{0:n2}" HtmlEncode="false" HeaderText="Flate Rate" />
                                <asp:BoundField DataField="LineTotal" DataFormatString="{0:n2}" HtmlEncode="false" HeaderText="Line Total" />
                            </Columns>
                         </asp:GridView>
                         <asp:FormView Width="100%" ID="FormHours" runat="server" 
                            DefaultMode="Edit" DataSourceID="SqlHourlyUnit" DataKeyNames="Id"
                            ondatabound="FormHours_DataBound" oniteminserting="FormHours_ItemInserting"
                             onitemdeleting="FormHours_ItemDeleting">
                            <EmptyDataTemplate>
                                <div class="jqhead rounded">
                                    <div class="sectitle">Misc Charges
                                    <asp:Button ID="btnNew" CssClass="btntny" CausesValidation="false" runat="server" 
                                        CommandName="New" Text="Add New Charge" />
                                    </div>
                                </div>
                            </EmptyDataTemplate>
                            <InsertItemTemplate>
                                <div class="jqhead rounded">
                                    <div class="sectitle">Add A New Charge
                                        <asp:Button ID="btnInsert" CssClass="btntny" CausesValidation="true" ValidationGroup="charge" runat="server" CommandName="Insert" Text="Save" />
                                        <asp:Button ID="btnCancel" CssClass="btntny" CausesValidation="false" runat="server" CommandName="Cancel" Text="Cancel" />
                                    </div>
                                </div>
                                <table border="0" cellspacing="3" cellpadding="0" width="100%" class="lsttbl">
                                    <tr>
                                        <th>Date</th>
                                        <th>Service Description</th>
                                        <th>Hours</th>
                                        <th>Rate</th>
                                        <th>Flat Rate</th>
                                        <th>Total</th>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtDatePerformed" MaxLength="10" Width="70px" runat="server" Text='<%#Bind("dtPerformed") %>' />
                                            <cc1:MaskedEditExtender ID="MaskedEditExtender1" runat="server" TargetControlID="txtDatePerformed" Mask="99/99/9999" 
                                                MaskType="Date" AcceptAMPM="false" MessageValidatorTip="true" OnFocusCssClass="maskededitfocus" 
                                                OnInvalidCssClass="maskedediterror" />
                                            <cc1:MaskedEditValidator ID="DatePaidValidator" runat="server" ControlToValidate="txtDatePerformed" 
                                                ControlExtender="MaskedEditExtender1" display="Dynamic" Text="*" ToolTip="Please enter a date" 
                                                InvalidValueMessage="date/time is invalid." ValidationGroup="charge" />
                                            <asp:CustomValidator ID="rowValidator" runat="server" CssClass="validator" Display="Static" 
                                                ValidationGroup="charge" >*</asp:CustomValidator>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDescription" MaxLength="2000" Width="400px" runat="server" Text='<%#Bind("ServiceDescription") %>' />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtHours" MaxLength="8" Width="40px" runat="server" Text='<%#Bind("Hours") %>' />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtRate" MaxLength="8" Width="40px" runat="server" Text='<%#Bind("Rate") %>' />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtFlatRate" MaxLength="8" Width="40px" runat="server" Text='<%#Bind("FlatRate") %>' />
                                        </td>
                                        <td>
                                            <asp:Literal ID="litTotal" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </InsertItemTemplate>
                            <EditItemTemplate>
                                <div class="jqhead rounded">
                                    <div class="sectitle"><%#Eval("dtPerformed", "{0:MM/dd/yyyy hh:mmtt}") %>
                                        <asp:Button ID="btnUpdate" CssClass="btntny" CausesValidation="true" ValidationGroup="charge" runat="server" CommandName="Update" Text="Save" />
                                        <asp:Button ID="btnDelete" CssClass="btntny" CausesValidation="false" runat="server" CommandName="Delete" Text="Delete" OnClientClick='return confirm("Are you sure you want to delete this charge?")' />
                                        <asp:Button ID="btnCancel" CssClass="btntny" CausesValidation="false" runat="server" CommandName="Cancel" Text="Cancel" />
                                    </div>
                                </div>
                                <table border="0" cellspacing="3" cellpadding="0" width="100%" class="lsttbl">
                                    <tr>
                                        <th>Date</th>
                                        <th>Service Description</th>
                                        <th>Hours</th>
                                        <th>Rate</th>
                                        <th>Flat Rate</th>
                                        <th>Total</th>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtDatePerformed" MaxLength="10" Width="70px" runat="server" Text='<%#Bind("dtPerformed") %>' />
                                            <cc1:MaskedEditExtender ID="MaskedEditExtender1" runat="server" TargetControlID="txtDatePerformed" Mask="99/99/9999" 
                                                MaskType="Date" AcceptAMPM="false" MessageValidatorTip="true" OnFocusCssClass="maskededitfocus" 
                                                OnInvalidCssClass="maskedediterror" />
                                            <cc1:MaskedEditValidator ID="DatePaidValidator" runat="server" ControlToValidate="txtDatePerformed" 
                                                ControlExtender="MaskedEditExtender1" display="Dynamic" Text="*" ToolTip="Please enter a date" 
                                                InvalidValueMessage="date/time is invalid." ValidationGroup="charge" />
                                            <asp:CustomValidator ID="rowValidator" runat="server" CssClass="validator" Display="Static" 
                                                ValidationGroup="charge" >*</asp:CustomValidator>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDescription" MaxLength="2000" Width="400px" runat="server" Text='<%#Bind("ServiceDescription") %>' />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtHours" MaxLength="8" Width="40px" runat="server" Text='<%#Bind("Hours") %>' />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtRate" MaxLength="8" Width="40px" runat="server" Text='<%#Bind("Rate") %>' />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtFlatRate" MaxLength="8" Width="40px" runat="server" Text='<%#Bind("FlatRate") %>' />
                                        </td>
                                        <td>
                                            <%#Eval("LineTotal", "{0:n2}") %>
                                        </td>
                                    </tr>
                                </table>
                            </EditItemTemplate>
                         </asp:FormView>
                    </td>
                    <td><%#Eval("HourlyPortion", "{0:n2}")%></td>
                </tr>
                <tr>
                    <th colspan="13" style="text-align: right;">Subtotal&nbsp;</th>
                    <td>
                        <asp:Literal ID="litSubtotal" runat="server" />
                    </td>
                </tr>
                <tr>
                    <th colspan="13" style="text-align: right;">Discounts&nbsp;</th>
                    <td>
                        <asp:TextBox ID="txtDiscount" runat="server" MaxLength="10" Width="70px" Text='<%#Bind("Discount", "{0:n2}") %>' />
                    </td>
                </tr>
                <tr>
                    <th colspan="13" style="text-align: right;">Total&nbsp;</th>
                    <td><%#Eval("LineTotal", "{0:n2}")%></td>
                </tr>
                <tr>
                    <th>Date Paid</th>
                    <td colspan="3">
                        <asp:TextBox ID="txtDatePaid" MaxLength="10" Width="70px" runat="server" Text='<%#Bind("dtPaid") %>' />
                        <cc1:MaskedEditExtender ID="MaskedEditExtender1" runat="server" TargetControlID="txtDatePaid" Mask="99/99/9999" 
                            MaskType="Date" AcceptAMPM="false" MessageValidatorTip="true" OnFocusCssClass="maskededitfocus" 
                            OnInvalidCssClass="maskedediterror" />
                        <cc1:MaskedEditValidator ID="DatePaidValidator" runat="server" ControlToValidate="txtDatePaid" 
                            ControlExtender="MaskedEditExtender1" display="Dynamic" Text="*" ToolTip="Please enter a date" 
                            InvalidValueMessage="date/time is invalid." ValidationGroup="charge" />
                    </td>
                    <th>Check #</th>
                    <td colspan="3"><asp:TextBox ID="txtCheckNumber" runat="server" MaxLength="50" Width="70px" Text='<%#Bind("CheckNumber") %>' /></td>
                    <th colspan="5" style="text-align: right;">Amount Paid&nbsp;</th>
                    <td>
                        <asp:TextBox ID="txtAmountPaid" runat="server" MaxLength="10" Width="70px" Text='<%#Bind("AmountPaid", "{0:n2}") %>' />
                    </td>
                </tr>
                <tr>
                    <th>Notes</th>
                    <td colspan="99">
                        <asp:TextBox ID="txtPayNotes" runat="server" MaxLength="2000" Text='<%#Bind("PayNotes", "{0:n2}") %>' 
                            TextMode="MultiLine" Width="500px" Height="50px" />
                    </td>
                </tr>            
            </table>
        </EditItemTemplate>
     </asp:FormView>
</div>


<asp:SqlDataSource ID="SqlMonths" runat="server" ConnectionString="<%$ ConnectionStrings:WillCallConnectionString %>"
    SELECTCOMMAND="EXEC [dbo].[tx_ChargeStatement_View] @appName, @year " onselecting="SqlMonths_Selecting" >
    <SelectParameters>
        <asp:Parameter Name="appName" Type="String" />
        <asp:ControlParameter ControlID="ddlYear" PropertyName="SelectedValue" Name="year" Type="Int32" />
    </SelectParameters>
</asp:SqlDataSource>

<asp:SqlDataSource ID="SqlEntity" runat="server" ConnectionString="<%$ ConnectionStrings:WillCallConnectionString %>"
    SELECTCOMMAND="SELECT cs.* FROM [Charge_Statement] cs WHERE cs.[Id] = @idx" 
    UPDATECOMMAND="UPDATE [Charge_Statement] SET [SalesQtyPct] = @SalesQtyPct, [RefundQtyPct] = @RefundQtyPct, 
    [GrossPct] = @GrossPct, [GrossThreshhold] = @GrossThreshhold, [TicketInvoicePct] = @TicketInvoicePct, 
    [TicketUnitPct] = @TicketUnitPct, [TicketSalesPct] = @TicketSalesPct, [MerchInvoicePct] = @MerchInvoicePct, 
    [MerchUnitPct] = @MerchUnitPct, [MerchSalesPct] = @MerchSalesPct, [ShipUnitPct] = @ShipUnitPct, 
    [ShipSalesPct] = @ShipSalesPct, [PerSubscription] = @PerSubscription, [PerMailSent] = @PerMailSent, 
    [Discount] = @Discount, [AmountPaid] = @AmountPaid, 
    [dtPaid] = @dtPaid, [CheckNumber] = @CheckNumber, [PayNotes] = @PayNotes WHERE ID = @idx "
    DELETECOMMAND="DELETE FROM [Charge_Statement] WHERE [Id] = @Id"
    >
    <SelectParameters>
        <asp:ControlParameter ControlID="GridView1" PropertyName="SelectedValue" Name="idx" Type="Int32" />
    </SelectParameters>
    <UpdateParameters>
        <asp:ControlParameter ControlID="FormView1" PropertyName="SelectedValue" Name="idx" Type="Int32" />
        <asp:Parameter Name="SalesQtyPct" Type="decimal" DefaultValue="0" ConvertEmptyStringToNull="false" />
        <asp:Parameter Name="RefundQtyPct" Type="decimal" DefaultValue="0" ConvertEmptyStringToNull="false" />
        <asp:Parameter Name="GrossPct" Type="decimal" DefaultValue="0" ConvertEmptyStringToNull="false" />
        <asp:Parameter Name="GrossThreshhold" Type="decimal" DefaultValue="0" ConvertEmptyStringToNull="false" />
        <asp:Parameter Name="TicketInvoicePct" Type="decimal" DefaultValue="0" ConvertEmptyStringToNull="false" />
        <asp:Parameter Name="TicketUnitPct" Type="decimal" DefaultValue="0" ConvertEmptyStringToNull="false" />
        <asp:Parameter Name="TicketSalesPct" Type="decimal" DefaultValue="0" ConvertEmptyStringToNull="false" />
        <asp:Parameter Name="MerchInvoicePct" Type="decimal" DefaultValue="0" ConvertEmptyStringToNull="false" />
        <asp:Parameter Name="MerchUnitPct" Type="decimal" DefaultValue="0" ConvertEmptyStringToNull="false" />
        <asp:Parameter Name="MerchSalesPct" Type="decimal" DefaultValue="0" ConvertEmptyStringToNull="false" />
        <asp:Parameter Name="ShipUnitPct" Type="decimal" DefaultValue="0" ConvertEmptyStringToNull="false" />
        <asp:Parameter Name="ShipSalesPct" Type="decimal" DefaultValue="0" ConvertEmptyStringToNull="false" />
        <asp:Parameter Name="PerSubscription" Type="decimal" DefaultValue="0" ConvertEmptyStringToNull="false" />
        <asp:Parameter Name="PerMailSent" Type="decimal" DefaultValue="0" ConvertEmptyStringToNull="false" />
        <asp:Parameter Name="Discount" Type="decimal" DefaultValue="0" ConvertEmptyStringToNull="false" />
        <asp:Parameter Name="AmountPaid" Type="decimal" DefaultValue="0" ConvertEmptyStringToNull="false" />
        <asp:Parameter Name="dtPaid" Type="DateTime" />
        <asp:Parameter Name="CheckNumber" Type="string" />
        <asp:Parameter Name="PayNotes" Type="string" />
    </UpdateParameters>
    <DeleteParameters>
        <asp:Parameter Name="Id" Type="Int32" />
    </DeleteParameters>
</asp:SqlDataSource>


<asp:SqlDataSource ID="SqlHours" runat="server" ConnectionString="<%$ ConnectionStrings:WillCallConnectionString %>"
    SELECTCOMMAND="SELECT ch.* FROM [Charge_Hourly] ch WHERE ch.[TChargeStatementId] = @StatementIdx ORDER BY ch.[dtStamp] " 
    >
    <SelectParameters>
        <asp:ControlParameter ControlID="FormView1" PropertyName="SelectedValue" Name="StatementIdx" Type="Int32" />
    </SelectParameters>
</asp:SqlDataSource>


<asp:SqlDataSource ID="SqlHourlyUnit" runat="server" ConnectionString="<%$ ConnectionStrings:WillCallConnectionString %>"
    SELECTCOMMAND="SELECT ch.* FROM [Charge_Hourly] ch WHERE ch.[Id] = @HourlyIdx " 
    UPDATECOMMAND="UPDATE [Charge_Hourly] SET [dtPerformed] = @dtPerformed, [ServiceDescription] = @ServiceDescription, [Hours] = @Hours, [Rate] = @Rate, [FlatRate] = @FlatRate WHERE [Id] = @Id; DECLARE @hourlySum money; SELECT @hourlySum = SUM(ch.[LineTotal]) FROM [Charge_Hourly] ch WHERE ch.[TChargeStatementId] = @TChargeStatementId; UPDATE [Charge_Statement] SET [HourlyPortion] = @hourlySum WHERE [Id] = @TChargeStatementId "
    INSERTCOMMAND="INSERT INTO Charge_Hourly ([TChargeStatementId], [dtPerformed], [ServiceDescription], [Hours], [Rate], [FlatRate]) VALUES (@TChargeStatementId, @dtPerformed, @ServiceDescription, @Hours, @Rate, @FlatRate); SELECT @NewId = @@IDENTITY; DECLARE @hourlySum money; SELECT @hourlySum = SUM(ch.[LineTotal]) FROM [Charge_Hourly] ch WHERE ch.[TChargeStatementId] = @TChargeStatementId; UPDATE [Charge_Statement] SET [HourlyPortion] = @hourlySum WHERE [Id] = @TChargeStatementId "
    DELETECOMMAND="DELETE FROM Charge_Hourly WHERE [Id] = @Id; DECLARE @hourlySum money; SELECT @hourlySum = SUM(ch.[LineTotal]) FROM [Charge_Hourly] ch WHERE ch.[TChargeStatementId] = @TChargeStatementId; UPDATE [Charge_Statement] SET [HourlyPortion] = @hourlySum WHERE [Id] = @TChargeStatementId " 
    onselecting="SqlHourlyUnit_Selecting" oninserted="SqlHourlyUnit_Inserted" 
    oninserting="SqlHourlyUnit_Inserting" OnDeleting="SqlHourlyUnit_Deleting" 
    ondeleted="SqlHourlyUnit_Deleted" onupdating="SqlHourlyUnit_Updating" onupdated="SqlHourlyUnit_Updated" >
    <SelectParameters>
        <asp:Parameter DefaultValue="0" Name="HourlyIdx" Type="Int32" />
    </SelectParameters>
    <UpdateParameters>
        <asp:Parameter Name="Id" Type="Int32" />
        <asp:Parameter Name="TChargeStatementId" Type="Int32" DefaultValue="0" />
        <asp:Parameter Name="dtPerformed" Type="DateTime" />
        <asp:Parameter Name="ServiceDescription" Type="String" />
        <asp:Parameter Name="Hours" Type="Decimal" />
        <asp:Parameter Name="Rate" Type="Decimal" />
        <asp:Parameter Name="FlatRate" Type="Decimal" />
    </UpdateParameters>
    <InsertParameters>
        <asp:Parameter Name="NewId" Direction="output" DefaultValue="567" Type="Int32" />
        <asp:Parameter Name="TChargeStatementId" Type="Int32" DefaultValue="0" />
        <asp:Parameter Name="dtPerformed" Type="DateTime" />
        <asp:Parameter Name="ServiceDescription" Type="String" />
        <asp:Parameter Name="Hours" Type="Decimal"  DefaultValue="0" />
        <asp:Parameter Name="Rate" Type="Decimal" DefaultValue="50" />
        <asp:Parameter Name="FlatRate" Type="Decimal" DefaultValue="0" />
    </InsertParameters>
    <DeleteParameters>
        <asp:Parameter Name="Id" Type="Int32" />
        <asp:Parameter Name="TChargeStatementId" Type="Int32" DefaultValue="0" />
    </DeleteParameters>
</asp:SqlDataSource>