<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Shipping_FulfillmentCreate.ascx.cs" Inherits="WillCallWeb.Admin.AdminControls.BulkShipping.Shipping_FulfillmentCreate" %>
<%@ Register src="~/Components/Util/CalendarClock.ascx" tagname="CalendarClock" tagprefix="uc1" %>
<div id="srceditor">
    <div id="batchcreate">
        <div class="jqhead rounded">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" class="edittabl">
                        <tr>
                            <th>CREATE BATCH SHIPMENT</th>
                            <td style="width:100%;">
                                <asp:DropDownList ID="ddlDates" runat="server" DataSourceID="SqlDates" AppendDataBoundItems="true" AutoPostBack="true" 
                                    OnSelectedIndexChanged="ddlDates_SelectedIndexChanged" EnableViewState="true" OnDataBound="ddlDates_DataBound"
                                    Width="100%" DataTextField="EventName" DataValueField="Id" >
                                    <asp:ListItem Text="<-- SELECT AN EVENT -->" Value="0" />
                                </asp:DropDownList>
                            </td>
                        </tr>
                     </table>
                     <table border="0" cellpadding="0" cellspacing="0" width="100%" class="edittabl">
                        <tr>
                            <th colspan="2" style="text-align:center;">Tickets For Selected Date (orders/#tickets)</th>
                            <th colspan="2" style="text-align:center;">Other Tickets Within These Orders (orders/#tickets)</th>
                        </tr>
                        <tr>
                            <td colspan="2" style="width:50%;vertical-align:top !important;">
                                <div class="jqinstruction rounded">
                                    <asp:CheckBoxList ID="chkTickets" runat="server" DataSourceID="SqlTicketsForDate" EnableViewState="true" 
                                        DataTextField="Id" DataValueField="Id" RepeatLayout="Flow" Width="100%"
                                        CellPadding="0" CellSpacing="0" TextAlign="Right" CssClass="rdoinp" 
                                        AutoPostBack="true" OnSelectedIndexChanged="chkTickets_SelectedIndexChanged"
                                        OnDataBound="chkTickets_DataBound" />
                                </div>
                            </td>
                            <td colspan="2" style="width:50%;vertical-align:top !important;">
                                <div class="jqinstruction rounded">
                                    <asp:CheckBoxList ID="chkOtherTickets" runat="server" EnableViewState="true" DataTextField="Id" DataValueField="Id"
                                        CellPadding="0" CellSpacing="0" TextAlign="Right" AutoPostBack="true" RepeatLayout="Flow" Width="100%"
                                        OnSelectedIndexChanged="chkOtherTickets_SelectedIndexChanged" CssClass="rdoinp"
                                        OnDataBinding="chkOtherTickets_DataBinding" OnDataBound="chkOtherTickets_DataBound" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <th>Sort By</th>
                            <td colspan="3">
                                <asp:RadioButtonList ID="rblSort" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Filter_SelectedIndexChanged" CellPadding="4" CellSpacing="4" 
                                    RepeatDirection="Horizontal" RepeatLayout="Flow" TextAlign="Right" CssClass="rdoinp" ToolTip="Choose to sort purchases by purchase name or by date purchased." >
                                    <asp:ListItem Text="Last Name" Value="lastnamefirst" Selected="True" />
                                    <asp:ListItem Text="Email" Value="purchaseemail" />
                                    <asp:ListItem Text="Date" Value="invoicedate" />
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <th>Shipping Filter</th>
                            <td><asp:RadioButtonList ID="rblFilter" runat="server" AutoPostBack="true" CssClass="rdoinp" OnSelectedIndexChanged="Filter_SelectedIndexChanged" CellPadding="4" CellSpacing="4" 
                                    RepeatDirection="Horizontal" RepeatLayout="Flow" TextAlign="Right" >                        
                                    <asp:ListItem Text="Not Yet Shipped" Value="notshippedonly" Selected="True" />
                                    <asp:ListItem Text="All" Value="all" />
                                </asp:RadioButtonList>
                            </td>
                            <th>Est. Ship Date</th>
                            <td style="padding:0;">
                                <uc1:CalendarClock ID="clockEstimate" runat="server" UseReset="false" UseTime="false" OnInit="clock_Init" />
                            </td>
                        </tr>
                    </table>
                    <asp:ValidationSummary HeaderText="The following errors ocurred:" ID="ValidationSummary1" runat="server" 
                        ValidationGroup="grid" CssClass="validationsummary" Width="100%" />
                    <asp:CustomValidator ID="CustomValidator" runat="server" ValidationGroup="grid" CssClass="validator" display="Dynamic" >*</asp:CustomValidator>        
                    <asp:ListView ID="Listing" runat="server" OnItemCommand="Listing_ItemCommand" 
                        DataKeyNames="Id,tTicketShipItemId" ItemPlaceholderID="ListViewContent" ondatabinding="Listing_DataBinding" OnItemDataBound="Listing_ItemDataBound" >
                        <LayoutTemplate>  
                            <div class="cmdsection">
                                <asp:Button ID="btnSelectAll" runat="server" CommandName="SelectAll" Text="Select All" CssClass="btnmed" />
                                <asp:Button ID="btnSelectNone" runat="server" CommandName="SelectNone" Text="Select None" CssClass="btnmed" />
                                <asp:Button ID="btnCreateShipment" runat="server" CommandName="Batch" Text="Create Batch" CssClass="btnmed"
                                    OnClientClick="return confirm('Have you verified the estimated ship date and selected all of the invoices you wish to create shipments for?');"  />
                                    <span class="tny">(this will only create shipments for items *selected above* in invoices *selected below* that have not yet shipped)</span>
                            </div>              
                            <table border="0" cellpadding="0" cellspacing="0" width="100%" class="lsttbl invtbl">
                                <tr class="hdr">
                                    <th>&nbsp;</th>
                                    <th>Ship To Name</th>
                                    <th>Date Of Invoice</th>
                                    <th>Purchaser Email</th>
                                    <th>InvoiceId</th>
                                    <th>ShipId</th>
                                    <th>Method</th>
                                </tr>
                                <tbody runat="server" id="ListViewContent" class="invedt" />
                                <tr><td colspan="7">&nbsp;</td></tr>
                            </table>
                        </LayoutTemplate>
            
                        <ItemTemplate>
                            <tr class="invport">
                                <td><asp:CheckBox ID="chkSelect" runat="server" Enabled="false" /></td>
                                <td><%#Eval("LastNameFirst") %></td>
                                <td><%#Eval("InvoiceDate", "{0:MM/dd/yyyy hh:mmtt}") %></td>
                                <td><%#Eval("PurchaseEmail") %></td>
                                <td><%#Eval("Id") %></td>
                                <td><%#Eval("tTicketShipItemId") %></td>
                                <td><%#Eval("TicketShipMethod") %></td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                                <td colspan="6" align="left">
                                    <asp:DataList ID="dlItem" runat="server" Width="100%" OnItemDataBound="dlItem_DataBound" DataKeyField="Id" ItemStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <div id="rowDiv" runat="server" style="text-align:left;position:relative;border-bottom:solid #e1e1e1 1px;">
                                                <div style="white-space:nowrap;float:left;border:solid blue 0px;width:10%;">
                                                    <span><asp:CheckBox ID="chkSlated" runat="server" Enabled="false" /><%#Eval("tShowTicketId") %> - <%#Eval("Quantity") %> @ </span>
                                                </div>
                                                <div style="float:left;border:solid red 0px;width:88%;">
                                                    <span><asp:Literal ID="litInfo" runat="server" /></span>
                                                    <span><asp:Literal ID="litShipping" runat="server" /></span>
                                                </div>
                                            </div>
                                        </ItemTemplate>
                                    </asp:DataList>
                                </td>
                            </tr>
                        </ItemTemplate>

                        <ItemSeparatorTemplate>
                            <tr class="sprt"><td colspan="7" style="line-height:3px;">&nbsp;</td></tr>
                        </ItemSeparatorTemplate>

                    </asp:ListView>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</div>
<!-- Get a list of dates in context-->
<asp:SqlDataSource ID="SqlDates" runat="server" ConnectionString="<%$ ConnectionStrings:WillCallConnectionString %>" 
    SelectCommand="SELECT sd.[Id], CONVERT(varchar, sd.[dtDateOfShow], 101) + ' ' + 
        SUBSTRING(CAST(sd.[dtDateOfShow] as varchar), LEN(CAST(sd.[dtDateOfShow] as varchar))-7, 1000) + ' ' + 
        SUBSTRING(s.[Name], 22, LEN(s.[Name])) as 'EventName'
        FROM [ShowDate] sd JOIN [Show] s ON sd.[tShowId] = s.[Id] WHERE sd.[dtDateOfShow] > @DateBaseline ORDER BY sd.[dtDateOfShow]; "
    OnSelecting="SqlDates_Selecting" >
    <SelectParameters>
        <asp:Parameter Name="DateBaseline" DbType="DateTime" />
    </SelectParameters>
</asp:SqlDataSource>
<!-- Show tickets for the selected date -->
<asp:SqlDataSource ID="SqlTicketsForDate" runat="server" ConnectionString="<%$ ConnectionStrings:WillCallConnectionString %>" 
    SelectCommand="SELECT st.[Id], ('ID: ' + CAST(st.[Id] as varchar) + ' ' +  CONVERT(varchar, sd.[dtDateOfShow], 101) + 

    ' ' + 
    
        SUBSTRING(CAST(sd.[dtDateOfShow] as varchar), LEN(CAST(sd.[dtDateOfShow] as varchar))-7, 1000) + 
    ' ' + SUBSTRING(s.[Name], 22, LEN(s.[Name])) + ' $' + CAST(st.[mPrice] as varchar) + ' + ' + CAST(st.[mServiceCharge] as varchar) + ' ' + ISNULL(st.[SalesDescription],'') + ' ' + ISNULL(st.[CriteriaText],'')) as 'TicketInfo' 
    FROM [Vendor] v, [ShowTicket] st LEFT OUTER JOIN [ShowDate] sd ON sd.[Id] = st.[tShowDateId] LEFT OUTER JOIN [Show] s ON s.[Id] = st.[tShowId] 
    WHERE st.[tShowDateId] = @ShowDateId AND st.[tVendorId] = v.[Id] AND v.[Name] = 'online' ORDER BY st.[iDisplayOrder]; " >
    <SelectParameters>
        <asp:ControlParameter ControlID="ddlDates" PropertyName="SelectedValue" DbType="Int32" Name="ShowDateId" />
    </SelectParameters>
</asp:SqlDataSource>