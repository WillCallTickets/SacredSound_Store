<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Orders_Exchange.ascx.cs" 
    Inherits="WillCallWeb.Admin.AdminControls.Orders_Exchange" %>
<div id="orderview">
    <div id="exchange">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="jqhead rounded">
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" class="edittabl" style="margin:0;">
                        <tr>
                            <th>Exchange</th>
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
                            <li>Exchanges will replace an item with the same value paid for the original item.</li>
                            <li>Packaged tickets cannot be exchanged (yet...)</li>
                            <li>It is up to you to decide if a price difference is OK.</li>
                            <li>Items marked as damaged affect total allotment. Exchanged items, for other reasons, go back into inventory.</li>
                            <li style="text-transform:uppercase;">Customers will see your reasons!</li>
                        </ul>
                    </div>
                    <asp:GridView Width="100%" ID="GridInvoice" runat="server" DataKeyNames="Id" AutoGenerateColumns="False"
                        EnableViewState="false" CssClass="lsttbl" Font-Size="10px" 
                        OnDataBinding="GridInvoice_DataBinding" OnDataBound="GridInvoice_DataBound" >
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
                <asp:ValidationSummary HeaderText="The following errors ocurred:" ID="ValidationSummary1" runat="server" 
                    ValidationGroup="order" CssClass="validationsummary" Width="100%" />
                <asp:Label ID="LabelSuccess" runat="server" Visible="False" Text="The invoice has been successfully refunded." Font-Bold="True" ForeColor="Red" />
                <div class="jqpnl rounded">
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" class="edittabl">
                        <tr>
                            <th>Exchangeable Items</th>
                            <td>
                                <asp:Button CssClass="btnmed" Width="80px" ID="btnDoExchange" runat="server" Text="Do Exchange" 
                                    OnClick="btnDoExchange_Click" CausesValidation="false"
                                    OnClientClick="return confirm('Are you sure you want to exchange the selected items? \r\nDOUBLE CHECK! \r\nNote that items marked as damaged will decrease availability. \r\nMake sure you use the correct reason.')" />
                            </td>
                            <td>
                                <asp:CustomValidator ID="CustomValidation" runat="server" ValidationGroup="order" CssClass="invisible" Display="Static" 
                                    ErrorMessage="CustomValidator">*</asp:CustomValidator>
                            </td>
                            <td style="width:100%;padding-left:22px;vertical-align:middle;">
                                <asp:CheckBox ID="chkIssueCredit" runat="server" Text="Assign any overage to store credit" TextAlign="Right" />
                            </td>
                        </tr>
                    </table>
                    <asp:GridView ID="GridView1" runat="server" Width="100%" AutoGenerateColumns="false" CssClass="lsttbl"
                        DataKeyNames="ItemIdentifier, ItemId, Quantity, BasePrice, Service, LineTotal, Context, Description, IsPackageTicket"
                        OnDataBinding="GridView1_DataBinding" OnRowDataBound="GridView1_RowDataBound" >
                        <AlternatingRowStyle BackColor="#f1f1f1" />
                        <EmptyDataTemplate>
                            <div class="lstempty">There are no items available for exchange</div>
                        </EmptyDataTemplate>
                        <Columns>
                            <asp:TemplateField HeaderText="Select" ItemStyle-HorizontalAlign="Center" >
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkSelect" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="BasePrice" HeaderText="Base" HtmlEncode="False" DataFormatString="{0:n}" 
                                ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="Service" HeaderText="Svc" DataFormatString="{0:n}" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="Each" HeaderText="Each" HtmlEncode="False" DataFormatString="{0:n}" 
                                ItemStyle-HorizontalAlign="Center" />
                            <asp:TemplateField HeaderText="Quantity" ItemStyle-HorizontalAlign="Center" >
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlQty" width="60px" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="LineTotal" HeaderText="LineTotal" HtmlEncode="False" DataFormatString="{0:c}" 
                                ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="Context" HeaderText="Context" ItemStyle-HorizontalAlign="Center" />
                            <asp:TemplateField HeaderText="Description" ItemStyle-Width="65%" HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="left">
                                <ItemTemplate>
                                    <div style="text-align: left;"><asp:Literal ID="litDescription" runat="server" /></div>
                                    <div style="text-align: left; background-color: #e1e1e1;">
                                        <div style="text-align: left;padding-bottom: 1em;">
                                            <span>
                                                <asp:RadioButtonList ID="rdoReason" runat="server" RepeatLayout="Flow" RepeatDirection="vertical" />
                                                <asp:TextBox ID="txtOther" Width="300px" runat="server" MaxLength="300" ></asp:TextBox>
                                            </span>
                                        </div>
                                        <div style="text-align: left;padding-bottom: 1em;">
                                            <asp:DropDownList Width="100%" ID="ddlExchange"
                                                OnDataBound="ddlExchange_DataBound" runat="server" />
                                        </div>
                                    </div>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <br />
                <div class="jqpnl rounded">
                    <asp:GridView Width="100%" ID="GridExchanges" runat="server" DataKeyNames="Id" AutoGenerateColumns="False" 
                        DataSourceID="SqlInvoiceEvents" CssClass="lsttbl" >
                        <EmptyDataTemplate>
                            <div class="lstempty">No Items have been exchanged</div>
                        </EmptyDataTemplate>
                        <Columns>
                            <asp:BoundField DataField="DateProcessed" DataFormatString="{0:MM/dd/yyyy hh:mmtt}" HeaderText="Date Of Exchange" HtmlEncode="false" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="OldValue" HeaderText="Original Item" HeaderStyle-HorizontalAlign="Left" />
                            <asp:BoundField DataField="Description" HeaderText="Description" HeaderStyle-HorizontalAlign="Left" />
                        </Columns>
                    </asp:GridView>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</div>
<asp:SqlDataSource ID="SqlInvoiceEvents" runat="server" ConnectionString="<%$ ConnectionStrings:WillCallConnectionString %>"
    SELECTCOMMAND="SELECT eq.* FROM [InvoiceEvent] ie, [EventQ] eq 
        WHERE ie.[TInvoiceId] = @InvoiceId AND ie.[TEventQId] = eq.[Id] AND eq.[Verb] = 'Exchange' AND eq.[ApplicationId] = @appId 
        ORDER BY eq.[DateProcessed] " 
    onselecting="SqlInvoiceEvents_Selecting" >
    <SelectParameters>
        <asp:Parameter Name="appId" DbType="Guid" />
        <asp:ControlParameter ControlID="GridInvoice" DefaultValue="0" Name="InvoiceId" PropertyName="SelectedValue" />
    </SelectParameters>
</asp:SqlDataSource>