<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Shipping_ShipmentBatchView.ascx.cs" Inherits="WillCallWeb.Admin.AdminControls.BulkShipping.Shipping_ShipmentBatchView" %>
<%@ Register src="~/Components/Util/CalendarClock.ascx" tagname="CalendarClock" tagprefix="uc1" %>
<%@ Register src="~/Components/Navigation/gglPager.ascx" tagname="gglPager" tagprefix="uc2" %>
<div id="srceditor">
    <div id="batchview">
        <div class="jqhead rounded">
            <table border="0" cellpadding="0" cellspacing="0" width="100%" class="edittabl">
                <tr>
                    <th>VIEW BATCH SHIPMENT</th>
                    <td style="width:100%;">
                        <asp:Button ID="btnCsvPage" runat="server" cssclass="btnmed" Text="CSV Page" CommandName="csvpage" OnClick="CSV_Click" 
                            OnClientClick="return confirm('This will make a download available for items on this PAGE ONLY. Would you like to proceed?');" />
                        <asp:Button ID="btnCsvAll" runat="server" CssClass="btnmed" Text="CSV ALL" CommandName="csvall" OnClick="CSV_Click" 
                            OnClientClick="return confirm('This will make a download available for items in the ENTIRE BATCH. Would you like to proceed?');" />
                        <!--CSV functions need to be out of update panel-->              
                              
                    </td>
                </tr>

            </table>
            <div class="intr">
                Please note that the filenames created by the CSV downloads may be too long for shipping programs.<br />If you are having issues mapping columns - try renaming the file to a shorter filename for the upload.
            </div>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" class="edittabl">
                        <tr>
                            <th><%if (Page.User.IsInRole("_Master"))
                                { %>
                                <asp:Button ID="btnUndoBatch" runat="server" CssClass="btnmed" Text="Undo Batch" width="100px" CommandName="undobatch" OnClick="btnUndoBatch_Click" 
                                    OnClientClick="return confirm('This will undo the current BATCH. Would you like to proceed?');" />  
                                <%} %> Batch Id</th>
                            <td colspan="5">
                                <asp:DropDownList ID="ddlPreviousBatch" runat="server" AutoPostBack="true"
                                    OnDataBound="ddlPreviousBatch_DataBound"
                                    OnSelectedIndexChanged="ddlPreviousBatch_SelectedIndexChanged" 
                                    DataSourceId="SqlPreviousBatches" AppendDataBoundItems="false" Width="90%" 
                                    DataTextField="Name" DataValueField="Id">
                                </asp:DropDownList>                                
                            </td>
                        </tr>
                        
                        <tr>
                            <th style="vertical-align: top;padding-top:6px;">Tickets Incl (ord/tix)</th>
                            <td colspan="5">
                                <div class="jqinstruction rounded">
                                    <ul>
                                        <asp:Repeater ID="rptTickets" runat="server" DataSourceID="SqlTicketListing" >
                                            <ItemTemplate>
                                                <li style="margin-left:18px;list-style-type:disc !important;">
                                                    <div id="rowDiv" runat="server">
                                                        <span><%#Eval("Text") %></span>
                                                    </div>
                                                </li>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </ul>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <th>Shipping Method</th>
                            <td colspan="5">
                                <asp:RadioButtonList ID="rblMethod" runat="server" AutoPostBack="true" 
                                    OnSelectedIndexChanged="Filter_SelectedIndexChanged" CellPadding="4" CellSpacing="4" 
                                    RepeatDirection="Horizontal" RepeatLayout="Flow" TextAlign="Right">
                                    <asp:ListItem Text="UPS Ground" Value="upsground" Selected="True" />
                                    <asp:ListItem Text="Ground Shipping" Value="groundshipping" />
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <th>Actual Shipping</th>
                            <td><asp:TextBox ID="txtActualShip" runat="server" Width="75px" Text="" /> 
                                <asp:CompareValidator Display="dynamic" CssClass="validation"  ValidationGroup="grid" 
                                    ID="CompareValidator6" runat="server" ErrorMessage="Please enter a numeric quantity."
                                    ControlToValidate="txtActualShip" Operator="DataTypeCheck" Type="Double">*</asp:CompareValidator>
                            </td>
                            <td style="white-space:nowrap;">
                                <asp:Button ID="btnChangeActualPage" runat="server" CssClass="btnmed" Text="Actual To Page" Width="80px" OnClick="ChangeBatchPage"
                                    OnClientClick="return confirm('This will apply the actual amount to the shipments listed in this PAGE ONLY. Would you like to proceed?');" />
                                <asp:Button ID="btnChangeActualAll" runat="server" CssClass="btnmed" Text="Actual To All" Width="80px" OnClick="ChangeBatchAll" 
                                    OnClientClick="return confirm('This will apply the actual amount to the ALL shipments in this BATCH. Would you like to proceed?');" />
                            </td>
                            <th>Est Ship</th>
                            <td style="padding:0;"><uc1:CalendarClock ID="clockEstimate" runat="server" UseReset="false" UseTime="false" OnInit="clock_Init" /></td>
                            <td style="white-space:nowrap;">
                                <asp:Button ID="btnChangeDatePage" runat="server" CssClass="btnmed" Text="Date To Page" Width="80px" OnClick="ChangeBatchPage" 
                                    OnClientClick="return confirm('This will apply the estimated ship date to the shipments listed in this PAGE ONLY. Would you like to proceed?');" />
                                <asp:Button ID="btnChangeDateAll" runat="server" CssClass="btnmed" Text="Date To All" Width="80px" OnClick="ChangeBatchAll" 
                                    OnClientClick="return confirm('This will apply the estimated ship date to the ALL shipments in this BATCH. Would you like to proceed?');" />
                            </td>
                        </tr>
                    </table>
                    <asp:ValidationSummary HeaderText="The following errors ocurred:" ID="ValidationSummary1" runat="server" 
                        ValidationGroup="grid" CssClass="validationsummary" Width="100%" />
                    <asp:CustomValidator ID="CustomValidator" runat="server" ValidationGroup="grid" CssClass="validator" display="Dynamic" >*</asp:CustomValidator>
                    <uc2:gglPager ID="GooglePager1" runat="server" PageButtonClass="btntny"  />
                    
                    <asp:ListView ID="Listing" runat="server" DataKeyNames="Id" ItemPlaceholderID="ListViewContent" 
                        OnInit="Listing_Init"
                        OnDataBinding="Listing_DataBinding" 
                        OnItemDataBound="Listing_ItemDataBound"
                        OnItemCommand="Listing_ItemCommand">
                        <LayoutTemplate>                
                            <table border="1" cellpadding="0" cellspacing="3" width="100%" class="lsttbl">                        
                                <tbody runat="server" id="ListViewContent" class="invedt" />
                                <tr><td colspan="6">&nbsp;</td></tr>
                            </table>
                        </LayoutTemplate>
                        
                        <ItemTemplate>
                            <tr>
                                <td rowspan="3" align="center">
                                    <asp:Literal ID="litRowNum" runat="server" EnableViewState="false" />
                                    <asp:Button ID="btnRowSave" runat="server" Text="Save" CssClass="btntny" CommandName="saverow" 
                                        OnClientClick="return confirm('This will save any changes for this row. Would you like to proceed?');" />
                                    <asp:Button ID="btnPrintRow" runat="server" Text="Print" CssClass="btntny" CommandName="printrow" />
                                </td>
                                <td rowspan="3" style="text-align:left;width:50%;"><%#Eval("AddressBlockFormatted_LastNameFirst") %></td>
                                <th>Est Ship</th>
                                <td style="letter-spacing:-1px;white-space:nowrap;"><%#Eval("DateShipped", "{0:MM/dd/yyyy hh:mmtt}") %></td>
                                <th>Email</th>
                                <td style="width:50%;"><%#Eval("PurchaseEmail") %></td>
                            </tr>
                            <tr>
                                <th>InvoiceId</th>
                                <td><a style="text-decoration:underline;" title="Goto the shipment page" href='/Admin/Orders.aspx?p=shipping&Inv=<%#Eval("InvoiceId") %>'><%#Eval("UniqueId") %></a></td>
                                <th>Tracking</th>
                                <td><asp:TextBox ID="txtTracking" runat="server" Text='<%#Eval("Tracking") %>' MaxLength="500" Width="100%" /></td>
                            </tr>
                            <tr>
                                <th>ShipItemId</th>
                                <td><%#Eval("ShipItemId") %></td>
                                <th>Ship Actual</th>
                                <td><asp:TextBox ID="txtActual" runat="server" Text='<%#Eval("ShippingActual","{0:n2}") %>' MaxLength="8" Width="100px" Height="12px" /></td>
                            </tr>
                            <tr>
                                <td colspan="6" style="width:100%;">
                                    <asp:DataList ID="dlItem" runat="server" OnItemDataBound="dlItem_ItemDataBound" DataKeyField="Id" Width="100%">
                                        <ItemTemplate>
                                            <div>
                                                <span style="display:inline-block;width:100%;"><%#Eval("Quantity") %> @ <asp:Literal ID="litInfo" runat="server" /></span>
                                                <span style="white-space:nowrap;text-transform:none;">
                                                    <span class="fklbl" style="padding:2px;margin-right:6px;">Tix #s </span> 
                                                    <asp:TextBox ID="txtTicketNumbers" runat="server" Text='' Width="90%" MaxLength="2000" Height="12px" /></span>
                                            </div>
                                        </ItemTemplate>
                                    </asp:DataList>
                                </td>
                            </tr>                    
                        </ItemTemplate>
                        <ItemSeparatorTemplate><tr><td colspan="6" class="fklbl">&nbsp;</td></tr></ItemSeparatorTemplate>
                    </asp:ListView>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</div>
<asp:SqlDataSource ID="SqlPreviousBatches" runat="server" ConnectionString="<%$ ConnectionStrings:WillCallConnectionString %>" 
    SelectCommand="CREATE TABLE #tmpBatch (Id int, Name varchar(256)); INSERT [#tmpBatch] ([Id],[Name]) VALUES (0, '<-- PREVIOUS BATCHES -- >'); INSERT [#tmpBatch] ([Id],[Name]) SELECT TOP 100 b.[Id], b.[Name] FROM [ShipmentBatch] b ORDER BY [Id] DESC; IF @currentBatchId > 0 BEGIN IF NOT EXISTS (SELECT * FROM [#tmpBatch] WHERE [Id] = @currentBatchId) BEGIN INSERT [#tmpBatch] ([Id],[Name]) SELECT [Id], [Name] FROM [ShipmentBatch] WHERE [Id] = @currentBatchId END END SELECT [Id], [Name] FROM [#tmpBatch]; DROP TABLE #tmpBatch"
    OnSelecting="SqlPreviousBatches_Selecting"
    >
    <SelectParameters>
        <asp:Parameter Name="currentBatchId" DbType="Int32" DefaultValue="0" />
    </SelectParameters>
</asp:SqlDataSource>


<asp:SqlDataSource ID="SqlTicketListing" runat="server" ConnectionString="<%$ ConnectionStrings:WillCallConnectionString %>" 
    SelectCommand="
    IF (@batchId > 0) BEGIN 

    CREATE TABLE #tmpTixIds (Id int, TicketId int)
	CREATE TABLE #tmpTicketAggs (Id int, OrderQty int, ItemQty int)
	CREATE TABLE #tmpDistinctTicketList (TicketId int)

	DECLARE @eventTix varchar(1000)
	SELECT	@eventTix = [csvEventTix] FROM [ShipmentBatch] WHERE [Id] = @batchId 

	IF (@eventTix IS NOT NULL) BEGIN

        INSERT	#tmpTixIds ([Id], [TicketId])
		SELECT	ti.[Id], ti.[ListItem] as 'TicketId'
		FROM	fn_ListToTable( @eventTix ) ti

	END

	DECLARE @otherTix varchar(1000)
	SELECT	@otherTix = [csvOtherTix] FROM [ShipmentBatch] WHERE [Id] = @batchId 

	IF (@otherTix IS NOT NULL) BEGIN

		INSERT	#tmpTixIds ([Id], [TicketId])
		SELECT	ti.[Id], ti.[ListItem] as 'TicketId'
		FROM	fn_ListToTable( @otherTix ) ti

	END

	
	INSERT	[#tmpDistinctTicketList](TicketId)
	SELECT	DISTINCT([TicketId]) 
	FROM [#tmpTixIds]
	
	DROP TABLE [#tmpTixIds]
	
    --UPDATE AGGREGATES FOR DISTINCT SHOWTICKET LIST
    INSERT	[#tmpTicketAggs](Id, OrderQty, ItemQty)
	SELECT	st.[Id], COUNT(DISTINCT(ii.tInvoiceId)) as 'OrderQty', SUM(ii.[iQuantity]) as 'ItemQty'
	FROM	[#tmpDistinctTicketList] t, [ShowTicket] st, [InvoiceItem] ii, [Invoice] i, 
			[ShipmentBatch_InvoiceShipment] sbis, [InvoiceShipment] invs
	WHERE	t.[TicketId] = st.[Id] AND 
			ii.[tShowTicketId] = st.[Id] AND 
			ii.[PurchaseAction] = 'Purchased' AND 
			sbis.[tShipmentBatchId] = @batchId AND 
			sbis.[tInvoiceShipmentId] = invs.[Id] AND 
			invs.[tInvoiceId] = i.[Id] AND 
			i.[InvoiceStatus] <> 'NotPaid' AND 			
			ii.[tInvoiceId] = i.[Id] 
	GROUP BY st.[Id]

	SELECT	st.[Id] as 'Id', 
			('(' + CAST([OrderQty] as varchar) + '/' + CAST([ItemQty] as varchar) + ') ' + 
			'ID: ' + CAST(st.[Id] as varchar) + ' - ' + 

            CASE WHEN CHARINDEX('camping pass', ISNULL(st.[SalesDescription],'') + ISNULL(st.[CriteriaText],'')) > 0 THEN 'CAMPING' ELSE CAST(st.[dtDateOfShow] as varchar(30)) END + ' ' + 
			            
            SUBSTRING(s.[Name], 22, LEN(s.[Name])) + ' ' + 
			'$' + CAST(st.[mPrice] as varchar) + ' + ' + CAST(st.[mServiceCharge] as varchar) + ' ' + 
			a.[Name] + ' ' + 
			st.[SalesDescription] + ' ' + 
			st.[CriteriaText]) as 'Text' 
	FROM	[#tmpTicketAggs] t, [ShowTicket] st 
				LEFT OUTER JOIN [Show] s ON s.[Id] = st.[tShowId]
				LEFT OUTER JOIN [Age] a ON a.[Id] = st.[tAgeId]
	WHERE	t.[Id] = st.[Id]
	ORDER BY st.[dtDateOfShow]

    DROP TABLE [#tmpDistinctTicketList]
    DROP TABLE [#tmpTicketAggs]
    
END "  >
    <SelectParameters>
        <asp:ControlParameter ControlID="ddlPreviousBatch" DbType="Int32" DefaultValue="0" Name="batchId" PropertyName="SelectedValue" />
    </SelectParameters>
</asp:SqlDataSource>






	
