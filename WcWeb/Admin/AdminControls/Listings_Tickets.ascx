<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Listings_Tickets.ascx.cs" Inherits="WillCallWeb.Admin.AdminControls.Listings_Tickets" %>
<%@ Register src="~/Components/Util/CalendarClock.ascx" tagname="CalendarClock" tagprefix="uc1" %>
<%@ Register src="~/Components/Navigation/gglPager.ascx" tagname="gglPager" tagprefix="uc2" %>
<div id="listingtickets">
	<div class="jqhead rounded">
		<table border="0" cellpadding="0" cellspacing="0" width="100%" class="edittabl">
			<tr>
				<th>MANIFESTS</th>
				<td style="padding-left:6px;width:100%;">
					<asp:DropDownList ID="ddlShowDates" EnableViewState="true" runat="server" AppendDataBoundItems="true" AutoPostBack="True" 
						OnDataBound="ddlShowDates_DataBound" DataTextField="EventName" DataValueField="Id" Font-Bold="true" Font-Size="10px"
						OnSelectedIndexChanged="ddlShowDates_SelectedIndexChanged" DataSourceID="SqlEventList" Width="100%" CssClass="fxddl">
						<asp:ListItem Text="...Select An Event..." Value="0" Selected="True"></asp:ListItem>
					</asp:DropDownList>
				</td>
				<th>Start</th>
				<td style="padding:0;">
					<uc1:CalendarClock ID="clockContext" runat="server" UseTime="false" UseReset="false" 
						 ValidationGroup="entity" OnSelectedDateChanged="clock_DateChange" />
				</td>
				<td>
					<%if (this.Page.User.IsInRole("Administrator")){%>
					<asp:Button ID="btnEdit" CssClass="btnmed" runat="server" OnClick="btnEdit_Click" Text="Edit Show" />
						<%} %>
				</td>
			</tr>
		</table>
	</div>
	<div class="jqedt rounded">
		<asp:GridView ID="GridTickets" EnableViewState="false" Width="100%" runat="server" AutoGenerateColumns="False" 
			ShowHeader="true" ShowFooter="true" DataSourceID="SqlTickets" DataKeyNames="Id" HeaderStyle-Font-Size="8px" 
			Font-Size="10px" ForeColor="#333333" BackColor="#ffffff" CssClass="opttbl" 
			OnRowDataBound="GridTickets_RowDataBound" 
			OnRowCommand="GridTickets_RowCommand" 
			OnDataBound="GridTickets_DataBound"
			OnDataBinding="GridTickets_DataBinding">
			<HeaderStyle HorizontalAlign="Center" Wrap="false" />
			<RowStyle HorizontalAlign="Center" />
			<SelectedRowStyle CssClass="selected" />
			<FooterStyle HorizontalAlign="Center" BackColor="#f1f1f1" />
			<Columns>
				<asp:BoundField DataField="Id" HeaderText="Tkt Id" ItemStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="8px"  />
				<asp:TemplateField>
					<HeaderTemplate>
						<asp:Button ID="btnSelectAll" CssClass="btntny" Font-Bold="true" runat="server" CommandName="SelectAll" CommandArgument="0" Text="Select All" />
					</HeaderTemplate>
					<ItemTemplate>
						<asp:Button ID="btnSelect" CssClass="btntny" runat="server" CommandName="Select" CommandArgument='<%#Eval("Id") %>' 
							Text='<%#Eval("Each", "{0:c}") %>' />
					</ItemTemplate>
					<FooterTemplate>TOTALS:</FooterTemplate>
				</asp:TemplateField>
				<asp:BoundField DataField="mPrice" DataFormatString="{0:n2}" HtmlEncode="False" HeaderText="Price" />
				<asp:BoundField DataField="mServiceCharge" DataFormatString="{0:n2}" HtmlEncode="False" HeaderText="Svc" />
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
					<FooterTemplate><%=_allot %></FooterTemplate>
				</asp:TemplateField>
				<asp:TemplateField HeaderText="Pend">
					<ItemTemplate><%#Eval("pendingStock") %></ItemTemplate>
					<FooterTemplate><%=_pend %></FooterTemplate>
				</asp:TemplateField>
				
				<asp:TemplateField HeaderText="Sold"><ItemTemplate><%#Eval("iSold") %></ItemTemplate><FooterTemplate><%=_sold %></FooterTemplate></asp:TemplateField>
				<asp:TemplateField HeaderText="WCall"><ItemTemplate><%#Eval("WillCall") %>(<%#Eval("WillOrders") %>)</ItemTemplate>
					<FooterTemplate><%=_willCall %>(<%=_willOrders %>)</FooterTemplate></asp:TemplateField>
				<asp:TemplateField HeaderText="Shipping"><ItemTemplate><%#Eval("Shipped") %>(<%#Eval("ShipOrders") %>)</ItemTemplate>
					<FooterTemplate><%=_shipped %>(<%=_shipOrders %>)</FooterTemplate></asp:TemplateField>
				<asp:TemplateField HeaderText="Avail">
				<ItemTemplate><%#Eval("iAvailable") %></ItemTemplate>
				<FooterTemplate><%=_avail %></FooterTemplate></asp:TemplateField>
				<asp:TemplateField HeaderText="Orders"><ItemTemplate><%#(int)Eval("WillOrders") + (int)Eval("ShipOrders") %></ItemTemplate>
					<FooterTemplate><%=_allOrders %></FooterTemplate></asp:TemplateField>
					
				<asp:TemplateField HeaderText="Ref">
					<ItemTemplate><%#Eval("iRefunded") %></ItemTemplate>
					<FooterTemplate><%=_ref %></FooterTemplate>
				</asp:TemplateField>
				<asp:TemplateField HeaderText="Base" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
					<ItemTemplate><%#Eval("Base", "{0:n2}") %></ItemTemplate>
					<FooterTemplate><%=_base.ToString("n2") %></FooterTemplate>
				</asp:TemplateField>
				<asp:TemplateField HeaderText="Fees" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
					<ItemTemplate><%#Eval("Fees", "{0:n2}") %></ItemTemplate>
					<FooterTemplate><%=_fees.ToString("n2") %></FooterTemplate>
				</asp:TemplateField>
				<asp:TemplateField HeaderText="Sales" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
					<ItemTemplate><%#Eval("Sales", "{0:n2}") %></ItemTemplate>
					<FooterTemplate><%=_sales.ToString("n2") %></FooterTemplate>
				</asp:TemplateField>
				<asp:TemplateField HeaderText="Desc/Crit" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="100%" >
					<ItemTemplate>
						<asp:Literal ID="litDescCrit" runat="server" />
					</ItemTemplate>
				</asp:TemplateField>
			</Columns>
		</asp:GridView>
		<table border="0" cellpadding="0" cellspacing="0" width="100%" class="edittabl">
			<tr>
				<th style="vertical-align:top;padding-top:3px;">Filter <a href="javascript: alert('Filters the results.')" class="infomark">?</a></th>
				<td style="white-space:nowrap;">
					<div>&nbsp;
					</div>
					<asp:RadioButtonList ID="rblShipContext" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" 
						OnDataBinding="rblShipContext_DataBinding" OnDataBound="rblShipContext_DataBound" 
						AutoPostBack="true" OnSelectedIndexChanged="rblShipContext_SelectedIndexChanged">
					</asp:RadioButtonList>
					<div style="padding-top:6px;">
						<asp:RadioButtonList ID="rdoPurchase" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" 
							AutoPostBack="true" OnSelectedIndexChanged="rblShipContext_SelectedIndexChanged" >
							<asp:ListItem Selected="True" Text="Purchases" Value="purchases" />
							<asp:ListItem Text="Refunds" Value="refunds" />
						</asp:RadioButtonList></div>
				</td>
				<td colspan="2" style="width:100%;"><div class="window"><asp:Literal ID="litSelection" runat="server" EnableViewState="false" OnDataBinding="litSelection_DataBinding" /></div></td>
			</tr>
			<tr>
				<th style="background-color:Transparent;padding-right:12px;">Criteria <a href="javascript: alert('Use purchase date for first-come, first-served listings.')" class="infomark">?</a></th>
				<td style="white-space:nowrap;">
					<asp:RadioButtonList ID="lstSortContext" runat="server" AutoPostBack="true" RepeatDirection="Horizontal"
						OnDataBinding="lstSortContext_DataBinding" RepeatLayout="Flow"
						onselectedindexchanged="lstSortContext_SelectedIndexChanged" 
						ondatabound="lstSortContext_DataBound" />
				</td>
				<td style="font-weight:bold;margin-left:32px;white-space:nowrap;vertical-align:middle;">
					These settings affect printout!</td>
				<td style="width:100%;">
                    <div class="commando">
					    <asp:Button ID="btnPrint" CssClass="btnmed" runat="server" Text="Print List" OnDataBinding="btnPrint_DataBinding" />
					    <asp:Button ID="btnEmailList" runat="server" CausesValidation="False" CommandName="emaillist" 
						    Text="Get Emails" Width="80px" CssClass="btnmed" OnClick="btnGetEmails_Click" />
					    <asp:Button ID="btnCSV" CssClass="btnmed" runat="server" Text="CSV (shipping)" Width="80px" OnDataBinding="btnCSV_DataBinding" />
					    <asp:Button ID="btnBatch" CssClass="btnmed" runat="server" Text="Batch Shipping" Width="80px" CommandName="emaillist" OnClick="btnBatch_Click" />
                        <span class="filter-cell checkbox">
                            <label>
                                <input type="checkbox" id="chkPhone" runat="server" />Display Phone #'s
                            </label>
                        </span>
                    </div>
				</td>
			</tr>
		</table>
	</div>
	<uc2:gglPager ID="GooglePager1" runat="server" PageButtonClass="btntny"  />
	<asp:ValidationSummary HeaderText="The following errors ocurred:" ID="ValidationSummary1" runat="server" 
		ValidationGroup="entity" CssClass="validationsummary" Width="100%" />
	<asp:GridView ID="GridSales" EnableViewState="false" Width="100%" runat="server" AutoGenerateColumns="False" 
		cssClass="lsttbl" 
		DataSourceID="ObjectSales" DataKeyNames="ItemId" ShowFooter="true" AllowPaging="True" OnDataBinding="GridSales_DataBinding" 
		OnDataBound="GridSales_DataBound" OnRowDataBound="GridSales_RowDataBound" 
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
                    <div class="phoner"><label>(ship)</label><%#Eval("PhoneShipping") %>
                </ItemTemplate></asp:TemplateField>
			<asp:TemplateField HeaderText="PurchaseName" HeaderStyle-HorizontalAlign="Left" >
				<ItemTemplate><%#Eval("PurchaseName") %>
                    <div class="phoner"><label>(bill)</label><%#Eval("PhoneBilling") %></div>
            </ItemTemplate></asp:TemplateField>
			<asp:TemplateField HeaderText="NameOnCard" HeaderStyle-HorizontalAlign="Left" >
				<ItemTemplate><%#Eval("NameOnCard") %></ItemTemplate></asp:TemplateField>
			<asp:TemplateField HeaderText="Last4" ItemStyle-HorizontalAlign="Center" >
				<ItemTemplate><%#Eval("LastFour") %></ItemTemplate></asp:TemplateField>
			<asp:BoundField DataField="UniqueInvoiceId" HeaderText="InvoiceID" ItemStyle-HorizontalAlign="Center" />
			<asp:TemplateField HeaderText="Email" HeaderStyle-HorizontalAlign="Left" >
				<ItemTemplate>
                    <asp:Literal ID="litOrderLink" runat="server" EnableViewState="false" /><br />
                    <div class="phoner"><label>(prof)</label><%#Eval("PhoneProfile") %></div>
                </ItemTemplate>
            </asp:TemplateField>
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
ORDER BY st.[iDisplayOrder] ASC " >
	<SelectParameters>
		<asp:ControlParameter ControlID="ddlShowDates" DefaultValue="0" Name="dateId" PropertyName="SelectedValue" />
	</SelectParameters>
</asp:SqlDataSource>


<asp:ObjectDataSource ID="ObjectSales" runat="server" EnableCaching="false" SelectMethod="GetTicketIdSales"
	TypeName="Wcss.QueryRow.TicketSalesRow" EnablePaging="True" SelectCountMethod="GetTicketIdSalesCount" 
	OnSelecting="objData_Selecting" OnSelected="objData_Selected">
	<SelectParameters>
		<asp:ControlParameter ControlID="ddlShowDates" Name="showDateId" PropertyName="SelectedValue" Type="Int32" />
		<asp:ControlParameter ControlID="GridTickets" Name="showTicketIds" PropertyName="SelectedValue" DefaultValue="0" Type="String" />
		<asp:ControlParameter ControlID="rblShipContext" Name="shipContext" PropertyName="SelectedValue" DefaultValue="all" Type="String" />
		<asp:ControlParameter ControlID="rdoPurchase" Name="purchaseContext" PropertyName="SelectedValue" DefaultValue="purchases" Type="String" />
		<asp:ControlParameter ControlID="lstSortContext" Name="sortContext" PropertyName="SelectedValue" DefaultValue="alphabetical" Type="String" />
	</SelectParameters>
</asp:ObjectDataSource>