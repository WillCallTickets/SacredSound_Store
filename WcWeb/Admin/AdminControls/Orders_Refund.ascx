<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Orders_Refund.ascx.cs" Inherits="WillCallWeb.Admin.AdminControls.Orders_Refund" %>
<div id="orderview">
    <div id="refund">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="jqhead rounded">
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" class="edittabl" style="margin:0;">
                        <tr>
                            <th>Refund</th>
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
                <div class="jqpanel1 rounded">
                    <div class="jqinstruction rounded">
                        <ul>
                            <li>Refunded ticket shipments will not automatically change tickets to "Will Call". To do this you must use the view order or shipments page.</li>
                            <li>Orders equal to or greater than 120 days old &amp; and expired credit cards cannot be processed. The customer will have to be contacted and/or a check will have to be issued.</li>
                            <li>Orders less than 24 hours old may only be voided for the full amount of the entire invoice. After 24 hours, partial refunds are allowed.</li>
                        </ul>
                    </div>
                    <asp:GridView Width="100%" ID="GridInvoice" runat="server" DataKeyNames="Id" AutoGenerateColumns="False" 
                        EnableViewState="false" CssClass="lsttbl" Font-Size="10px"
                        OnDataBinding="GridInvoice_DataBinding" OnDataBound="GridInvoice_DataBound" OnRowDataBound="GridInvoice_RowDataBound" >
                        <Columns>
                            <asp:BoundField DataField="InvoiceDate" HeaderText="Invoice Details" HtmlEncode="false" DataFormatString="{0:MM/dd/yyyy hh:mm tt}" 
                                HeaderStyle-HorizontalAlign="left" />
                            <asp:TemplateField HeaderText="Billed To" HeaderStyle-HorizontalAlign="Left">
                                <ItemTemplate><div><%#Eval("PurchaseEmail") %></div><%#Eval("InvoiceBillShip.Addressee_Billing")%></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Shipments" HeaderStyle-HorizontalAlign="Left">
                                <ItemTemplate><asp:Literal ID="litShipments" runat="server" /></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Last 4 - Expiry" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%if (this.Page.User.IsInRole("Administrator"))
                                    {%><%#Eval("CashewRecord.LastFour") %><%}
                                    else
                                    { %>xxxx<%} %>
                                    <asp:Literal ID="litExpiry" runat="server" />
                                </ItemTemplate>
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
                        cssclass="lsttbl" Font-Size="10px" EnableViewState="false"
                        OnDataBinding="GridItems_DataBinding" 
                        OnRowDataBound="GridItems_RowDataBound" >
                        <AlternatingRowStyle BackColor="#f1f1f1" />
                        <Columns>
                            <asp:TemplateField HeaderText="Item Details" HeaderStyle-HorizontalAlign="left" HeaderStyle-Width="300px" ItemStyle-Width="300px" ItemStyle-Wrap="true" >
                                <ItemTemplate><asp:Literal ID="litDescription" runat="server" /></ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="TShipItemId" HeaderText="Ship Id" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="iQuantity" HeaderText="Qty" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="mPrice" DataFormatString="{0:n}" HtmlEncode="false" HeaderText="Price" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="mServiceCharge" DataFormatString="{0:n}" HtmlEncode="false" HeaderText="Svc" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="mLineItemTotal" DataFormatString="{0:n}" HtmlEncode="false" HeaderText="Total" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="PurchaseAction" HeaderText="Status" ItemStyle-HorizontalAlign="Center" />
                            <asp:CheckBoxField DataField="bRTS" HeaderText="RTS" ItemStyle-HorizontalAlign="center" />
                            <asp:TemplateField HeaderText="Shipped On" ItemStyle-HorizontalAlign="Center" >
                                <ItemTemplate><%#Eval("dtShipped","{0:MM/dd/yyyy hh:mmtt}") %></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Ship Method" ItemStyle-HorizontalAlign="Center" >
                                <ItemTemplate><%#Eval("ShippingMethod") %></ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>   
                </div>
                <div class="jqinstruction rounded">
                    <ul>
                        <li>Because refunds have many components (expired CCs, inventory, etc, etc), please be sure that you are aware of all the implications before attempting a refund</li>
                        <li>Discount amount should rarely be used and is for very special circumstances. For instance, it will not keep track of inventory and it may invalidate ship methods.</li>
                        <li>Note that discount amounts will override any selections in the item listing.</li>
                        <li>Transactions paid with store credit, will refund to store credit before applying any remainder to the payment processor.</li>
                        <li>Generally, to apply a refund
                            <ol style="margin-left:24px;">
                                <li>Select the payment processor to refund to (Store credit then Authnet, Authnet, Store credit, company check)</li>
                                <li>Note that company check refunds require a check number and that discount amount refunds require a description as well as an amount</li>
                                <li>Select the items from the list to be refunded (this will keep inventory in check)</li>
                                <li>Hit the &quot;do refund&quot; button</li>
                                <li>If refunding to store credit - you may need to sync the store credit to the user's account. This can be done on the users account page.</li>
                                <li>When refunding BUNDLES, be sure to include (or not) all items within the bundle. Be aware of what items are linked.</li>
                            </ol>
                        </li>
                    </ul>
                </div>
                <asp:ValidationSummary HeaderText="The following errors ocurred:" ID="ValidationSummary2" runat="server" 
                    ValidationGroup="order" CssClass="validationsummary" Width="100%" />
                <asp:Label ID="LabelSuccess" runat="server" Visible="False" Text="The invoice has been successfully refunded." Font-Bold="True" ForeColor="Red" />
                <div class="jqpanel1 rounded">
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" class="edittabl">
                        <tr>
                            <th style="text-align:left;">Select Method</th>
                            <td rowspan="3">&nbsp;</td>
                            <th>Discount Amount</th>
                            <td><asp:TextBox ID="txtDiscount" Width="60px" runat="server" MaxLength="8" /></td>
                            <td style="width:100%;" class="intr">
                                <b class="color:red">Refunds to store credit only!!!</b>
                                Issue this type of refund when, eg: a customer orders 3 day shipping and they get the item 10 days later. 
                                You feel that they deserve partial credit for this situation.
                            </td>
                        </tr>
                        <tr>
                            <td rowspan="2" style="white-space:nowrap;">
                                <asp:RadioButtonList ID="rdoProcessor" runat="server" RepeatDirection="Vertical" OnDataBinding="rdoProcessor_DataBinding" 
                                    RepeatLayout="Flow" CellPadding="0" CellSpacing="0" OnDataBound="rdoProcessor_DataBound">
                                </asp:RadioButtonList>
                            </td>
                            <th>Description</th>
                            <td colspan="2" style="width:100%;"><asp:TextBox ID="txtDescription" Width="250px" runat="server" MaxLength="300" />
                                <span class="intr">Required for discount amount refunds.</span>
                            </td>                            
                        </tr>
                        <tr>
                            <th>Check #</th>
                            <td colspan="2"><asp:TextBox ID="txtCheckNum" Width="60px" runat="server" MaxLength="20" />
                                <span class="intr">Required for check method refunds.</span>
                            </td>
                        </tr>
                    </table>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" class="edittabl">
                        <tr>
                            <td style="white-space:nowrap;">
                                <asp:Button CssClass="btnmed" Width="80px" CausesValidation="false" ID="btnSelect" runat="server" 
                                    Text="Select All" OnClick="btnSelect_Click" />
                                <asp:Button CssClass="btnmed" Width="80px" CausesValidation="false" ID="btnDeselect" runat="server" 
                                    Text="Deselect All" OnClick="btnDeselect_Click"  />
                            </td>
                            <th style="white-space:nowrap;">Purchased (<%=Atx.CurrentInvoiceRecord.NetPaid.ToString("c") %>)</th>
                            <td style="width:100%;">
                                <asp:Button CssClass="btnmed" Width="80px" ID="btnDoRefund" CausesValidation="false" runat="server" 
                                    Text="Do Refund" OnClick="btnDoRefund_Click"
                                    OnClientClick="return confirm('ARE YOU ABOLUTELY SURE that you have REVIEWED THIS refund and have SELECTED the CORRECT ITEMS for refund? ARE YOU SURE? Are you 100% SURE that your QUANTITIES are CORRECT?')" />
                                <asp:CustomValidator ID="CustomValidation" runat="server" ValidationGroup="order" CssClass="invisible" Display="Static" 
                                    ErrorMessage="CustomValidator">*</asp:CustomValidator>   
                            </td>
                        </tr>
                    </table>
                    <asp:GridView ID="GridView1" runat="server" Width="100%" AutoGenerateColumns="false" CssClass="lsttbl"
                        DataKeyNames="ItemIdentifier, ItemId, Quantity, BasePrice, Service, LineTotal, Context, Description, SalePromotionId, IsPackageTicket"
                        OnDataBinding="GridView1_DataBinding" OnRowDataBound="GridView1_RowDataBound" OnDataBound="GridView1_DataBound">
                        <EmptyDataTemplate>
                            <div class="lstempty">There are no items available for refund</div>
                        </EmptyDataTemplate>
                        <Columns>
                            <asp:TemplateField HeaderText="Select">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkSelect" runat="server" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="BasePrice" HeaderText="Base" HtmlEncode="False" DataFormatString="{0:n}" >
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Service">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkService" runat="server" />
                                    <asp:Literal ID="litService" runat="server" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" Wrap="False" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="Each" HeaderText="Each" HtmlEncode="False" DataFormatString="{0:n}" >
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Qty">
                                <HeaderTemplate>
                                    <asp:Label ID="lblQty" runat="server" ToolTip="Select qty to refund" Text="Qty" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlQty" width="60px" runat="server" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="LineTotal" HeaderText="LineTotal" HtmlEncode="False" DataFormatString="{0:c}" >
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Context" HeaderText="Context" >
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Description" HeaderStyle-HorizontalAlign="left" ItemStyle-Width="65%">
                                <ItemTemplate>
                                    <asp:Literal ID="litDescription" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <br />
                <div class="jqpanel1 rounded">
                    <asp:GridView Width="100%" ID="GridRefunds" runat="server" DataKeyNames="Id" AutoGenerateColumns="False" CssClass="lsttbl" 
                        OnDataBinding="GridRefunds_DataBinding" OnRowDataBound="GridRefunds_RowDataBound" EnableViewState="false" >
                        <AlternatingRowStyle BackColor="#f1f1f1" />
                        <EmptyDataTemplate>
                            <div class="lstempty">No Items have been refunded</div>
                        </EmptyDataTemplate>
                        <Columns>
                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-Wrap="false">
                                <HeaderTemplate>Refunded Items (<%=Atx.CurrentInvoiceRecord.TotalRefunds.ToString("c") %>)</HeaderTemplate>
                                <ItemTemplate><%#Eval("dtStamp","{0:MM/dd/yyyy hhmmtt}") %> </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Context" HeaderText="Context" ItemStyle-Font-Bold="true" HeaderStyle-HorizontalAlign="Left" />
                            <asp:TemplateField HeaderText="Description" HeaderStyle-HorizontalAlign="left">
                                <ItemTemplate>
                                    <asp:Literal ID="litDescription" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Price" HeaderText="Base" HtmlEncode="false" DataFormatString="{0:n}"
                                ItemStyle-HorizontalAlign="center" />
                            <asp:BoundField DataField="ServiceCharge" HeaderText="Svc" HtmlEncode="false" DataFormatString="{0:n}"
                                ItemStyle-HorizontalAlign="center" />    
                            <asp:BoundField DataField="PricePerItem" HeaderText="Each" HtmlEncode="false" DataFormatString="{0:n}"
                                ItemStyle-HorizontalAlign="center" />
                            <asp:BoundField DataField="Quantity" HeaderText="Qty" HtmlEncode="false" ItemStyle-HorizontalAlign="center" />
                            <asp:BoundField DataField="LineItemTotal" HeaderText="Total" HtmlEncode="false" DataFormatString="{0:c}"
                                ItemStyle-HorizontalAlign="center" />
                        </Columns>
                    </asp:GridView>
                    <asp:GridView ID="GridAuths" Width="100%" runat="server" DataSourceID="SqlAuth" AutoGenerateColumns="False" CssClass="lsttbl" 
                        DataKeyNames="Id" EnableViewState="false">
                        <AlternatingRowStyle BackColor="#f1f1f1" />
                        <Columns>
                            <asp:BoundField DataField="dtStamp" HeaderText="Auth Net Transactions" HeaderStyle-HorizontalAlign="Left" />
                            <asp:BoundField DataField="TInvoiceId" HeaderText="Id" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="InvoiceNumber" HeaderText="InvoiceNumber" ItemStyle-HorizontalAlign="Center" />
                            <asp:CheckBoxField DataField="bAuthorized" HeaderText="Auth" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="ProcessorId" HeaderText="ProcId" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="Method" HeaderText="Meth" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="TransactionType" HeaderText="Type" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="mTaxAmount" DataFormatString="{0:n2}" HtmlEncode="False" HeaderText="Tax" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="mFreightAmount" DataFormatString="{0:n2}" HtmlEncode="False" HeaderText="S&amp;H" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="mAmount" DataFormatString="{0:c}" HtmlEncode="False" HeaderText="Amount" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="Email" HeaderText="Email" HeaderStyle-HorizontalAlign="Left" />
                            <asp:BoundField DataField="NameOnCard" HeaderText="NameOnCard" HeaderStyle-HorizontalAlign="Left" />
                            <asp:BoundField DataField="IpAddress" HeaderText="IpAddress" ItemStyle-HorizontalAlign="Center" />
                        </Columns>
                    </asp:GridView>
                    <asp:GridView ID="GridTrans" Width="100%" runat="server" DataSourceID="SqlTransactions" AutoGenerateColumns="False" CssClass="lsttbl" 
                        DataKeyNames="Id" EnableViewState="false">
                        <AlternatingRowStyle BackColor="#f1f1f1" />
                        <Columns>
                            <asp:BoundField DataField="ProcessorId" HeaderText="Invoice Transactions" HeaderStyle-HorizontalAlign="left" />
                            <asp:BoundField DataField="PerformedBy" HeaderText="Performed By" HeaderStyle-HorizontalAlign="left" />
                            <asp:BoundField DataField="TransType" HeaderText="Type" HeaderStyle-HorizontalAlign="left" />
                            <asp:BoundField DataField="FundsType" HeaderText="Funds" HeaderStyle-HorizontalAlign="left" />
                            <asp:BoundField DataField="FundsProcessor" HeaderText="Processor" HeaderStyle-HorizontalAlign="left" />
                            <asp:BoundField DataField="mAmount" HtmlEncode="false" DataFormatString="{0:c}" HeaderText="Amount" ItemStyle-HorizontalAlign="center" />
                            <asp:BoundField DataField="NameOnCard" HeaderText="Card Name" HeaderStyle-HorizontalAlign="left" />
                            <asp:BoundField DataField="LastFourDigits" HeaderText="Last 4" ItemStyle-HorizontalAlign="center" />
                            <asp:BoundField DataField="UserIp" HeaderText="User Ip" ItemStyle-HorizontalAlign="center" />
                        </Columns>
                    </asp:GridView>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</div>

<asp:SqlDataSource ID="SqlAuth" runat="server" CacheDuration="300" ConnectionString="<%$ ConnectionStrings:WillCallConnectionString %>"
    EnableCaching="False"     
    SelectCommand="SELECT [dtStamp], [Id], [InvoiceNumber], [bAuthorized], [TInvoiceId], [UserId], [CustomerId], [ProcessorId], [Method], 
    [TransactionType], [mAmount], [mTaxAmount], [mFreightAmount], [Description], [Email], [NameOnCard], [IpAddress] 
    FROM [AuthorizeNet] WHERE (([TInvoiceId] = @TInvoiceId) AND ([ApplicationId] = @appId) AND ([bAuthorized] = @bAuthorized)) ORDER BY [Id] " 
    onselecting="Sql_Selecting" >
    <SelectParameters>
        <asp:Parameter Name="appId" DbType="Guid" />
        <asp:QueryStringParameter DefaultValue="0" Name="TInvoiceId" QueryStringField="Inv" Type="Int32" />
        <asp:Parameter DefaultValue="true" Name="bAuthorized" Type="Boolean" />
    </SelectParameters>
</asp:SqlDataSource>

<asp:SqlDataSource ID="SqlTransactions" runat="server" CacheDuration="300" ConnectionString="<%$ ConnectionStrings:WillCallConnectionString %>"
    EnableCaching="False" 
    SelectCommand="SELECT it.* 
        FROM [InvoiceTransaction] it, [Invoice] i WHERE i.[Id] = @TinvoiceId AND i.[ApplicationId] = @appId AND i.[Id] = it.[TInvoiceId] 
        ORDER BY [Id]" 
    onselecting="Sql_Selecting" >
    <SelectParameters>
        <asp:Parameter Name="appId" DbType="Guid" />
        <asp:QueryStringParameter DefaultValue="0" Name="TInvoiceId" QueryStringField="Inv" Type="Int32" />
    </SelectParameters>
</asp:SqlDataSource>

