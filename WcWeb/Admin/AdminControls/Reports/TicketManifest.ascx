<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TicketManifest.ascx.cs" Inherits="WillCallWeb.Admin.AdminControls.Reports.TicketManifest" %>
<%@ Register src="~/Components/Util/CalendarClock.ascx" tagname="CalendarClock" tagprefix="uc1" %>
<%@ Register src="~/Components/Navigation/gglPager.ascx" tagname="gglPager" tagprefix="uc2" %>

<div id="twelve">
    <div class="reporte">
        <div id="cainer">
    
            <div id="header" class="rounded">
                <table border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <th>TICKET MANIFEST</th>
                        <td style="width:100%">
                            <asp:DropDownList ID="ddlShowDates" EnableViewState="true" runat="server" 
                                AppendDataBoundItems="true" AutoPostBack="True" 
                                OnDataBound="ddlShowDates_DataBound" DataTextField="EventName" 
                                DataValueField="Id" Width="100%" 
                                OnSelectedIndexChanged="ddlShowDates_SelectedIndexChanged" 
                                DataSourceID="SqlEventList">
                                <asp:ListItem Text="...Select An Event..." Value="0" Selected="True"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <uc1:CalendarClock ID="clockContext" runat="server" UseTime="false" UseReset="false" 
                                ValidationGroup="entity" OnSelectedDateChanged="clock_DateChange" />    
                        </td>
                        <td>
                            <%if (this.Page.User.IsInRole("Administrator"))
                                {%>
                            <asp:Button ID="btnEdit" CssClass="butn" runat="server" OnClick="btnEdit_Click" Text="Edit Show" />
                            <%}
                                else
                                {%>
                            &nbsp;
                            <%} %>
                        </td>
                    </tr>                
                </table>        
            </div>

            <div id="ticket-select">
                <asp:GridView ID="GridTickets" EnableViewState="true" Width="100%" 
                    runat="server" AutoGenerateColumns="False" 
                    ShowHeader="true" ShowFooter="true" DataSourceID="SqlTickets" DataKeyNames="Id"                     
                    CssClass="info-table" 
                    OnRowDataBound="GridTickets_RowDataBound" 
                    
                    OnDataBound="GridTickets_DataBound" > 
                    <HeaderStyle CssClass="info-header" />                   
                    <RowStyle HorizontalAlign="Center" />
                    <SelectedRowStyle CssClass="selected" />
                    <FooterStyle HorizontalAlign="Center" BackColor="#f1f1f1" />
                    <Columns>
                        <asp:TemplateField ControlStyle-CssClass="info-check">
                            <HeaderTemplate>
                                <asp:Button ID="btnSelectAll" CssClass="butn" runat="server" CommandName="SelectAll" CommandArgument="0" Text="Select All" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkOne" runat="server" AutoPostBack="true" OnCheckedChanged="TicketCheckChanged" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Id" HeaderText="Tkt Id" ItemStyle-HorizontalAlign="Center" />
                        <asp:TemplateField ItemStyle-Wrap="false" HeaderText="Pricing" >
                            <ItemTemplate>
                                <div><%#Eval("Each", "{0:c}") %></div><div class="price-comp"><%#Eval("mPrice", "{0:n2}") %> + <%#Eval("mServiceCharge", "{0:n2}") %>s</div>
                            </ItemTemplate>
                            <FooterTemplate><div class="selct">SELECTED:</div>TOTALS:</FooterTemplate>                         
                        </asp:TemplateField>

                        <asp:BoundField DataField="Ages" HeaderText="Age" ItemStyle-Wrap="false" />
                        <asp:TemplateField ItemStyle-Wrap="false" HeaderText="Act" >
                            <ItemTemplate>
                                <asp:CheckBox ID="chkActive" Enabled="false" runat="server" Checked='<%#Eval("bActive") %>' Text="" Width="20" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:CheckBoxField DataField="bSoldOut" HeaderText="SO" />
                        <asp:CheckBoxField DataField="bOverrideSellout" HeaderText="OvrSO" />
                        <asp:TemplateField HeaderText="DOS">
                            <ItemTemplate>
                                <asp:Literal Visible='<%#Eval("bDosTicket") %>' ID="litDosIndicator" runat="server" 
                                    Text="<span style='color:red;'>DOS</span>" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Offsale" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:Literal ID="litOffsale" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Allot">
                            <ItemTemplate><%#Eval("iAllotment") %></ItemTemplate>
                            <FooterTemplate><div class="selct"><%=sel_allot %></div><%=_allot %></FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Pend">
                            <ItemTemplate><%#Eval("pendingStock") %></ItemTemplate>
                            <FooterTemplate><div class="selct"><%=sel_pend %></div><%=_pend %></FooterTemplate>
                        </asp:TemplateField>
                
                        <asp:TemplateField HeaderText="Sold"><ItemTemplate><%#Eval("iSold") %></ItemTemplate><FooterTemplate><div class="selct"><%=sel_sold %></div><%=_sold %></FooterTemplate></asp:TemplateField>
                        <asp:TemplateField HeaderText="WCall" FooterStyle-Wrap="false"><ItemTemplate><%#Eval("WillCall") %>(<%#Eval("WillOrders") %>)</ItemTemplate>
                            <FooterTemplate><div class="selct"><%=sel_willCall %>(<%=sel_willOrders %>)</div><%=_willCall %>(<%=_willOrders %>)</FooterTemplate></asp:TemplateField>
                        <asp:TemplateField HeaderText="Shipping"><ItemTemplate><%#Eval("Shipped") %>(<%#Eval("ShipOrders") %>)</ItemTemplate>
                            <FooterTemplate><div class="selct"><%=sel_shipped %>(<%=sel_shipOrders %>)</div><%=_shipped %>(<%=_shipOrders %>)</FooterTemplate></asp:TemplateField>
                        <asp:TemplateField HeaderText="Avail">
                        <ItemTemplate><%#Eval("iAvailable") %></ItemTemplate>
                        <FooterTemplate><div class="selct"><%=sel_avail %></div><%=_avail %></FooterTemplate></asp:TemplateField>
                        <asp:TemplateField HeaderText="Orders"><ItemTemplate><%#(int)Eval("WillOrders") + (int)Eval("ShipOrders") %></ItemTemplate>
                            <FooterTemplate><div class="selct"><%=sel_allOrders %></div><%=_allOrders %></FooterTemplate></asp:TemplateField>
                        <asp:TemplateField HeaderText="Ref">
                            <ItemTemplate><%#Eval("iRefunded") %></ItemTemplate>
                            <FooterTemplate><div class="selct"><%=sel_ref %></div><%=_ref %></FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Base" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                            <ItemTemplate><%#Eval("Base", "{0:n2}") %></ItemTemplate>
                            <FooterTemplate><div class="selct"><%=sel_base.ToString("n2") %></div><%=_base.ToString("n2") %></FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Fees" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                            <ItemTemplate><%#Eval("Fees", "{0:n2}") %></ItemTemplate>
                            <FooterTemplate><div class="selct"><%=sel_fees.ToString("n2") %></div><%=_fees.ToString("n2") %></FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Sales" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                            <ItemTemplate><%#Eval("Sales", "{0:n2}") %></ItemTemplate>
                            <FooterTemplate><div class="selct"><%=sel_sales.ToString("n2") %></div><%=_sales.ToString("n2") %></FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Criteria-Description" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="100%" >
                            <ItemTemplate>
                                <asp:Literal ID="litDescCrit" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>

            <div id="filters" class="rounded">
                <table border="0" cellpadding="0" cellspacing="0" class="filter-section">
                    <tr>
                        <td>
                            <div class="filter-cell rounded" style="height:100%;">
                                <h4>Ship Context</h4>
                                <asp:RadioButtonList ID="rblShipContext" runat="server" RepeatDirection="Vertical" RepeatLayout="Flow" 
                                    OnDataBinding="rblShipContext_DataBinding" OnDataBound="rblShipContext_DataBound" 
                                    AutoPostBack="true" OnSelectedIndexChanged="rblShipContext_SelectedIndexChanged">
                                </asp:RadioButtonList>
                            </div>
                        </td>
                        <td>
                            <div class="filter-cell rounded">
                                <h4>Purchases</h4>
                                <asp:RadioButtonList ID="rdoPurchase" runat="server" RepeatDirection="Vertical" RepeatLayout="Flow" 
                                    AutoPostBack="true" OnSelectedIndexChanged="rblShipContext_SelectedIndexChanged" >
                                    <asp:ListItem Selected="True" Text="Purchases" Value="purchases" />
                                    <asp:ListItem Text="Refunds" Value="refunds" />
                                </asp:RadioButtonList>
                            </div>
                        </td>
                        <td>
                            <div class="filter-cell rounded">
                                <h4>Sort Order</h4>
                                <asp:RadioButtonList ID="lstSortContext" runat="server" AutoPostBack="true" RepeatDirection="Vertical"
                                    OnDataBinding="lstSortContext_DataBinding" RepeatLayout="Flow"
                                    onselectedindexchanged="lstSortContext_SelectedIndexChanged" 
                                    ondatabound="lstSortContext_DataBound" />
                            </div>
                        </td>
                        <td rowspan="2" style="width:65%;">
                            <div class="filter-cell rounded">
                                <h4 style="text-align:left;margin-left:12px;">Current Selections</h4>
                                <asp:Literal ID="litSelection" runat="server" EnableViewState="false" OnDataBinding="litSelection_DataBinding" />
                            </div>
                        </td>
                    </tr>
                </table>
                <uc2:gglPager ID="GooglePager1" runat="server" PageButtonClass="btntny">
                    <Template>
                        <div class="commando">
                            <asp:Button ID="btnPrint" Enabled="false" CssClass="butn" runat="server" Text="Print List" OnDataBinding="btnPrint_DataBinding" />
                            <asp:Button ID="btnEmailList" Enabled="false" runat="server" CausesValidation="False" CommandName="emaillist" 
                                Text="Emails" CssClass="butn" OnClick="btnGetEmails_Click" />
                            <asp:Button ID="btnCSV" Enabled="false" CssClass="butn" runat="server" Text="CSV worldship" OnDataBinding="btnCSV_DataBinding" />
                            <asp:Button ID="btnBatch" Enabled="false" CssClass="butn" runat="server" Text="Batch Ship" CommandName="emaillist" OnClick="btnBatch_Click" />
                            <span class="filter-cell checkbox">
                                <label>
                                    <input type="checkbox" id="chkPhone" runat="server" />Display Phone #'s
                                </label>
                            </span>
                        </div>
                    </Template>
                </uc2:gglPager>

            </div>
            
            <div id="valida">
                <asp:ValidationSummary HeaderText="The following errors ocurred:" ID="ValidationSummary1" runat="server" 
                    ValidationGroup="entity" CssClass="validationsummary" Width="100%" />
            </div>

            <div id="listing">
                 <asp:GridView ID="GridSales" EnableViewState="false" Width="100%" runat="server" AutoGenerateColumns="False" 
                    cssClass="lsttbl" 
                    DataSourceID="ObjectSales" DataKeyNames="ItemId" ShowFooter="true" AllowPaging="True" 
                    OnDataBinding="GridSales_DataBinding" 
                    OnDataBound="GridSales_DataBound" 
                    OnRowDataBound="GridSales_RowDataBound" 
                    OnInit="GridSales_Init">
                    <PagerSettings Visible="false" />
                    <RowStyle BackColor="#ffffff" />
                    <FooterStyle HorizontalAlign="center" />
                    <EmptyDataTemplate>
                        <div class="lstempty">There are no sales for the criteria selected</div>
                    </EmptyDataTemplate>
                    <Columns>
                        <asp:TemplateField ItemStyle-HorizontalAlign="center" ItemStyle-CssClass="rowcounter">
                            <ItemTemplate><asp:Literal ID="LiteralRowCounter" runat="server" /></ItemTemplate></asp:TemplateField>
                        <asp:BoundField DataField="ShowTicketId" HeaderText="Tkt Id" ItemStyle-HorizontalAlign="Center" />
                        <asp:TemplateField HeaderText="PickupName" HeaderStyle-HorizontalAlign="Left" >
                            <ItemTemplate><%#Eval("PickupName") %>
                                <div class="phoner"><label>(ship)</label><%#Eval("PhoneShipping") %></div>
                            </ItemTemplate></asp:TemplateField>
                        <asp:TemplateField HeaderText="PurchaseName" HeaderStyle-HorizontalAlign="Left" >
                            <ItemTemplate>
                                <%#Eval("PurchaseName") %>
                                <div class="phoner"><label>(bill)</label><%#Eval("PhoneBilling") %></div>
                            </ItemTemplate>
                            </asp:TemplateField>
                        <asp:TemplateField HeaderText="NameOnCard" HeaderStyle-HorizontalAlign="Left" >
                            <ItemTemplate><%#Eval("NameOnCard") %>                                
                            </ItemTemplate></asp:TemplateField>
                        <asp:TemplateField HeaderText="Last4" ItemStyle-HorizontalAlign="Center" >
                            <ItemTemplate><%#Eval("LastFour") %></ItemTemplate></asp:TemplateField>
                        <asp:BoundField DataField="UniqueInvoiceId" HeaderText="InvoiceID" ItemStyle-HorizontalAlign="Center" /> 
                        <asp:TemplateField HeaderText="Email" HeaderStyle-HorizontalAlign="Left" >
                            <ItemTemplate><asp:HyperLink ID="linkEditUser" runat="server" 
                                NavigateUrl='<%#"/Admin/Orders.aspx?p=view&Inv=" + Eval("ParentInvoiceId") %>' 
                                ToolTip="Go to customer's invoice for this purchase" Text='<%#Eval("Email") %>' />
                                <div class="phoner"><label>(prof)</label><%#Eval("PhoneProfile") %></div>
                            </ItemTemplate></asp:TemplateField>
                        <asp:TemplateField HeaderText="Qty" ItemStyle-HorizontalAlign="center" ><ItemTemplate><%#Eval("Qty") %></ItemTemplate>
                            <FooterTemplate><%=_qty %></FooterTemplate></asp:TemplateField>
                        <asp:TemplateField HeaderText="Ages" ItemStyle-HorizontalAlign="center" ><ItemTemplate><%#Eval("Age") %></ItemTemplate></asp:TemplateField>
                        <asp:TemplateField HeaderText="Notes" HeaderStyle-HorizontalAlign="Left" ><ItemTemplate><%#Eval("Notes") %></ItemTemplate></asp:TemplateField>
                        <asp:CheckBoxField DataField="IsReturned" ItemStyle-HorizontalAlign="center" HeaderText="RTS" />
                        <asp:TemplateField HeaderText="ShippedOn" ItemStyle-HorizontalAlign="center" ><ItemTemplate><%#Eval("DateShipped") %></ItemTemplate></asp:TemplateField>
                        <asp:TemplateField HeaderText="Method" ItemStyle-HorizontalAlign="center" ><ItemTemplate><%#Eval("ShippingMethod") %></ItemTemplate></asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>

        </div>
    </div>
</div>

<asp:SqlDataSource ID="SqlEventList" runat="server" ConnectionString="<%$ ConnectionStrings:WillCallConnectionString %>"
    SelectCommand="SELECT DISTINCT TOP 1000 sd.[Id], sd.[dtDateOfShow], CONVERT(varchar(25), sd.[dtDateOfShow], 111) + ' ' + 
        LTRIM(SUBSTRING(CONVERT(varchar(25), sd.[dtDateOfShow]), LEN(CONVERT(varchar(25), sd.[dtDateOfShow])) -6, 7)) + ' - ' + 
        SUBSTRING(s.[Name], (CHARINDEX(' - ', s.[Name]) + 3), LEN(s.[Name])) as 'EventName' FROM [ShowDate] sd, [Show] s 
        WHERE sd.[tShowId] = s.[Id] AND s.[ApplicationId] = @appId AND sd.[dtDateOfShow] >= @date ORDER BY sd.[dtDateOfShow] " 
    onselecting="SqlEventList_Selecting">
    <SelectParameters>
        <asp:Parameter Name="appId" DbType="Guid" />
        <asp:Parameter Name="date" DbType="string" />
    </SelectParameters>
</asp:SqlDataSource>
<asp:SqlDataSource ID="SqlTickets" runat="server" ConnectionString="<%$ ConnectionStrings:WillCallConnectionString %>" 
     SelectCommand="SELECT st.[Id], st.[bActive], st.[dtEndDate], st.[bSoldOut], st.[bOverrideSellout], st.[bDosTicket], st.[SalesDescription], st.[CriteriaText], a.[Name] as 'Ages', 
	(st.[mPrice] + st.[mServiceCharge]) as 'Each', st.[PriceText], st.[mPrice], st.[DosText], st.[mDosPrice], st.[mServiceCharge], 
	st.[iAllotment], 	
	ISNULL(PendingStock.[iQty], 0) as 'PendingStock',
    st.[iSold], 
	st.[iAllotment] - ISNULL(PendingStock.[iQty], 0) - st.[iSold] as 'iAvailable',  	
	st.[iRefunded], 	
	SUM(CASE WHEN ii.[ShippingMethod] IS NULL OR ( LEN(LTRIM(RTRIM(ii.[ShippingMethod]))) = 0 OR ii.[ShippingMethod] = 'Will Call') 
		THEN ISNULL(ii.[iQuantity],0) ELSE 0 END) as 'WillCall', 
	SUM(CASE WHEN ii.[Id] IS NOT NULL AND (ii.[ShippingMethod] IS NULL OR ( LEN(LTRIM(RTRIM(ii.[ShippingMethod]))) = 0 OR ii.[ShippingMethod] = 'Will Call')) 
		THEN 1 ELSE 0 END) as 'WillOrders',  
	SUM(CASE WHEN ii.[ShippingMethod] IS NOT NULL AND LEN(LTRIM(RTRIM(ii.[ShippingMethod]))) > 0 AND ii.[ShippingMethod] <> 'Will Call' 
		THEN ISNULL(ii.[iQuantity],0) ELSE 0 END) as 'Shipped', 
	SUM(CASE WHEN ii.[ShippingMethod] IS NOT NULL AND LEN(LTRIM(RTRIM(ii.[ShippingMethod]))) > 0 AND ii.[ShippingMethod] <> 'Will Call' 
		THEN 1 ELSE 0 END) as 'ShipOrders', 
	(ISNULL(st.[mPrice],0) * st.[iSold]) as 'Base', (ISNULL(st.[mServiceCharge],0) * st.[iSold]) as 'Fees', ((ISNULL(st.[mPrice],0) + ISNULL(st.[mServiceCharge], 0)) * st.[iSold]) as 'Sales' 
FROM [Age] a, [ShowTicket] st 
LEFT OUTER JOIN fn_PendingStock('ticket') PendingStock ON PendingStock.[idx] = st.[Id] 
    LEFT OUTER JOIN [InvoiceItem] ii 
	ON ii.[vcContext] = 'ticket' AND ii.[tShowTicketId] = st.[Id] AND ii.[PurchaseAction] = 'Purchased' 	
WHERE st.[TShowDateId] = @dateId AND st.[TAgeId] = a.[Id] 
GROUP BY st.[Id], st.[bActive], st.[dtEndDate], st.[bSoldOut], st.[bOverrideSellout], st.[bDosTicket], st.[SalesDescription], st.[CriteriaText], a.[Name], st.[PriceText], 
	st.[mPrice], st.[DosText], st.[mDosPrice], st.[mServiceCharge], st.[iAllotment], st.[iSold], PendingStock.[iQty], 
	st.[iAvailable], st.[iRefunded], st.[iDisplayOrder] 
ORDER BY st.[iDisplayOrder] ASC "  >
    <SelectParameters>
        <asp:ControlParameter ControlID="ddlShowDates" DefaultValue="0" Name="dateId" PropertyName="SelectedValue" />
    </SelectParameters>
</asp:SqlDataSource>
<asp:ObjectDataSource ID="ObjectSales" runat="server" EnableCaching="false" SelectMethod="GetTicketIdSales"
    TypeName="Wcss.QueryRow.TicketSalesRow" EnablePaging="True" SelectCountMethod="GetTicketIdSalesCount" 
    OnSelecting="objData_Selecting" OnSelected="objData_Selected">
    <SelectParameters>
        <asp:ControlParameter ControlID="ddlShowDates" Name="showDateId" PropertyName="SelectedValue" Type="Int32" />        
        <asp:Parameter Name="showTicketIds" DefaultValue="" Type="String" />
        <asp:ControlParameter ControlID="rblShipContext" Name="shipContext" PropertyName="SelectedValue" DefaultValue="all" Type="String" />
        <asp:ControlParameter ControlID="rdoPurchase" Name="purchaseContext" PropertyName="SelectedValue" DefaultValue="purchases" Type="String" />
        <asp:ControlParameter ControlID="lstSortContext" Name="sortContext" PropertyName="SelectedValue" DefaultValue="alphabetical" Type="String" />        
    </SelectParameters>
</asp:ObjectDataSource>