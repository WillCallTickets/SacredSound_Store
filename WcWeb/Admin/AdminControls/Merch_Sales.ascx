<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Merch_Sales.ascx.cs" Inherits="WillCallWeb.Admin.AdminControls.Merch_Sales" %>
<%@ Register src="~/Components/Util/CalendarClock.ascx" tagname="CalendarClock" tagprefix="uc1" %>
<%@ Register src="~/Components/Navigation/gglPager.ascx" tagname="gglPager" tagprefix="uc2" %>
<%@ Register src="~/Admin/AdminControls/Menu_MerchSelection.ascx" tagname="Menu_MerchSelection" tagprefix="uc1" %>
<div id="merchsales">
    <uc1:Menu_MerchSelection ID="Menu_MerchSelection1" runat="server" LinkToPage="Sales" Title="Sales" />
    <div class="lngtitle"><asp:Literal ID="litLifetime" runat="server" /></div>
    <div class="jqhead rounded">
        <table border="0" cellspacing="0" cellpadding="0" width="100%" class="edttbl">
            <tr>
                <th>Style</th>
                <td colspan="4" style="width:100%;">
                    <asp:DropDownList ID="ddlStyle" Width="100%" runat="server" OnDataBinding="ddlStyle_DataBinding" 
                        OnSelectedIndexChanged="Select_SelectedIndexChanged" AutoPostBack="True" />
                </td>
            </tr>
            <tr>
                <th>Color</th>
                <td colspan="4">
                    <asp:DropDownList ID="ddlColor" Width="100%" runat="server" OnDataBinding="ddlColor_DataBinding" 
                        OnSelectedIndexChanged="Select_SelectedIndexChanged" AutoPostBack="True" />
                </td>
            </tr>
            <tr>
                <th>Size</th>
                <td>
                    <asp:DropDownList ID="ddlSize" runat="server" OnDataBinding="ddlSize_DataBinding" 
                        OnSelectedIndexChanged="Select_SelectedIndexChanged" AutoPostBack="True" />
                </td>
                <th>Status</th>
                <td style="width:100%;">
                    <asp:RadioButtonList ID="rdoStatus" runat="server" AutoPostBack="True" CellPadding="0" CellSpacing="0" 
                        OnSelectedIndexChanged="Select_SelectedIndexChanged" RepeatDirection="Horizontal" RepeatLayout="Flow" >
                        <asp:ListItem Selected="True" Value="all">All</asp:ListItem>
                        <asp:ListItem Value="true">Active</asp:ListItem>
                        <asp:ListItem Value="false">InActive</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
                <td>
                    <asp:Button ID="btnCsv" runat="server" OnClick="btnCsvClick" CssClass="btntny" Width="80px" Text="Exclusive Csv" />
                </td>
            </tr>
        </table>
        <uc2:gglPager ID="GooglePager1" runat="server" PageButtonClass="btntny" />
        <asp:GridView ID="GridView1" ShowFooter="false" AutoGenerateColumns="False" Width="100%" runat="server" AllowPaging="True" 
            DataSourceID="ObjectDataSource1" EnableViewState="false" cssclass="lsttbl"
            OnRowDataBound="GridView1_RowDataBound" 
            OnDataBinding="GridView1_DataBinding" 
            OnDataBound="GridView1_DataBound" 
            OnRowCommand="GridView1_RowCommand" 
            OnSelectedIndexChanged="GridView1_SelectedIndexChanged" 
            OnRowCreated="GridView1_RowCreated">
            <PagerSettings Visible="false" />
            <SelectedRowStyle CssClass="selected" />
            <EmptyDataTemplate>
                <div class="lstempty">No Items match the criteria selected</div>
            </EmptyDataTemplate>
            <Columns>
                <asp:TemplateField ItemStyle-HorizontalAlign="center" ItemStyle-CssClass="rowcounter">
                    <HeaderTemplate><asp:LinkButton ID="linkDeSelect" runat="server" CssClass="btnadmin" Text="All" CommandName="deselect" /></HeaderTemplate>
                    <ItemTemplate><asp:Panel ID="PanelRowCounter" runat="server" /></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ItemStyle-HorizontalAlign="center" >
                    <ItemTemplate>
                        <asp:Button ID="btnCsv" runat="server" CommandName="csvreport" CommandArgument='<%#Eval("MerchId") %>' CssClass="btntny" Width="80px" Text="Exclusive Csv" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Style" HeaderText="Style" HeaderStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="Color" HeaderText="Color" HeaderStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="Size" HeaderText="Size" HeaderStyle-HorizontalAlign="Left" />
                <asp:CheckBoxField DataField="IsActive" HeaderText="Act" ItemStyle-HorizontalAlign="Center" />
                <asp:CheckBoxField DataField="IsSoldOut" HeaderText="SO" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="Price" DataFormatString="{0:n2}" HtmlEncode="false" HeaderText="Price" ItemStyle-HorizontalAlign="Center" />
                <asp:CheckBoxField DataField="UseSalePrice" HeaderText="On Sale" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="SalePrice" DataFormatString="{0:n2}" HtmlEncode="false" HeaderText="Sale$" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="Weight" DataFormatString="{0:n}" HtmlEncode="false" HeaderText="Weight" ItemStyle-HorizontalAlign="Center" />
                <asp:TemplateField ItemStyle-HorizontalAlign="center" >
                    <HeaderTemplate>Allot <asp:Literal ID="litAllot" runat="server" /></HeaderTemplate>
                    <ItemTemplate><%#Eval("Allot") %></ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Dmg" HeaderText="Dmg" />
                <asp:TemplateField HeaderText="Pend" ItemStyle-HorizontalAlign="center" >
                    <HeaderTemplate>Pend <asp:Literal ID="litPend" runat="server" /></HeaderTemplate>
                    <ItemTemplate><%#Eval("Pend") %></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Sold" ItemStyle-HorizontalAlign="center" >
                     <HeaderTemplate>Sold <asp:Literal ID="litSold" runat="server" /></HeaderTemplate>
                    <ItemTemplate><%#Eval("Sold") %></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Avail" ItemStyle-HorizontalAlign="center" >
                     <HeaderTemplate>Avail <asp:Literal ID="litAvail" runat="server" /></HeaderTemplate>
                    <ItemTemplate><%#Eval("Avail") %></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Refund" ItemStyle-HorizontalAlign="center" >
                     <HeaderTemplate>Refund <asp:Literal ID="litRefund" runat="server" /></HeaderTemplate>
                    <ItemTemplate><%#Eval("Refund") %></ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    <div class="jqpanel1 rounded" style="margin-top:6px;">
        <table border="0" cellspacing="0" cellpadding="0" class="hedtbl" style="width:100%;">
            <tr>
                <th>Select Date Range</th>
                <th>Start</th>
                <td>
                    <uc1:CalendarClock ID="clockStart" runat="server" UseReset="false" UseTime="false" OnInit="clock_Init" 
                        OnSelectedDateChanged="clock_SelectedDateChanged" /></td>
                <th>End</th>
                <td>
                    <uc1:CalendarClock ID="clockEnd" runat="server" UseReset="false" UseTime="false" OnInit="clock_Init" 
                        OnSelectedDateChanged="clock_SelectedDateChanged" />
                </td>
                <td style="width:100%;text-align:right;">
                    <asp:Button ID="btnEmailList" runat="server" CausesValidation="False" CommandName="emaillist" 
                        Text="Get Emails" CssClass="btntny" OnClick="btnGetEmails_Click" />
                    <asp:Button ID="btnEmailAndId" runat="server" CausesValidation="False" CommandName="emailandid" 
                        Text="Get Emails And Ids" CssClass="btntny" OnClick="btnGetEmailAndIds_Click" />
                    <asp:Button ID="btnCodes" runat="server" CausesValidation="False" CommandName="codes" 
                        Text="Get Codes" CssClass="btntny" OnClick="btnCodes_Click" />
                    <asp:Button ID="btnCodeOnly" runat="server" CausesValidation="False" CommandName="codeonly" 
                        Text="Code Only" CssClass="btntny" OnClick="btnCodes_Click" />
                </td>
            </tr>
        </table>
        <asp:GridView ID="SqlItemReport" AutoGenerateColumns="false" ShowFooter="false" Width="100%" runat="server" AllowPaging="False"
            DataSourceID="SqlItemReporter" EnableViewState="False" CssClass="lsttbl" 
            OnRowDataBound="SqlItemReport_RowDataBound" >
            <RowStyle HorizontalAlign="Center" />
            <EmptyDataTemplate><div class="lstempty">No Aggregate Data For This Item In Selected Range</div></EmptyDataTemplate>
            <Columns>
                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                    <HeaderTemplate><asp:Literal ID="litRangedTitle" runat="server" /></HeaderTemplate>
                    <ItemTemplate><asp:Literal ID="litRangedAttribs" runat="server" /></ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="NumLineItems" HeaderText="Sale Entries" />
                <asp:BoundField DataField="UnitsSold" HeaderText="Qtyy Sold" />
                <asp:BoundField DataField="SaleTotal" DataFormatString="{0:c}" HtmlEncode="false" HeaderText="Sales Total"  />
            </Columns>
        </asp:GridView>
        <uc2:gglPager ID="GooglePagerSales" runat="server" PageButtonClass="btntny" />
        <asp:GridView ID="SalesGrid" AutoGenerateColumns="False" Width="100%" runat="server" AllowPaging="True" ShowFooter="True" 
            DataSourceID="ObjectDataSource2" EnableViewState="False" CssClass="lsttbl" 
            OnRowDataBound="SalesGrid_RowDataBound" 
            OnDataBinding="SalesGrid_DataBinding" 
            OnDataBound="SalesGrid_DataBound" 
            OnInit="SalesGrid_Init">
            <PagerSettings Visible="false" />
            <EmptyDataTemplate><div class="lstempty">No Sales For This Item In Selected Range</div></EmptyDataTemplate>
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate><asp:Literal ID="LiteralRowCounter" runat="server" /></ItemTemplate>
                    <ItemStyle CssClass="rowcounter" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Invoice Details" ItemStyle-HorizontalAlign="left" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" >
                <ItemTemplate>
                    <a href='/Admin/Orders.aspx?p=view&amp;Inv=<%#Eval("InvoiceId") %>'><%#Eval("InvoiceDate") %></a>
                    <div class="list-inv-id"><%#Eval("UniqueId") %></div>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Purchaser Details" ItemStyle-HorizontalAlign="left" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" >
                <ItemTemplate>
                    <a href='/Admin/CustomerEditor.aspx?p=sales&amp;UserName=<%#Eval("PurchaserEmail") %>'><%#Eval("PurchaserName")%></a>
                    <div class="list-inv-id"><%#Eval("PurchaserEmail")%></div>
                </ItemTemplate>
            </asp:TemplateField>
                <asp:TemplateField HeaderText="Description" ItemStyle-Width="100%" HeaderStyle-HorizontalAlign="Left" >
                    <ItemTemplate><asp:Literal ID="literalDescription" runat="server" /></ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="InvoiceStatus" HeaderText="Status" SortExpression="Status" ItemStyle-HorizontalAlign="Center" />
                <asp:TemplateField HeaderText="Shipping" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center">
                    <ItemTemplate><asp:Literal ID="litFreight" runat="server" /></ItemTemplate>
                    <FooterTemplate><%=_gridFreight.ToString("c") %></FooterTemplate>
                </asp:TemplateField> 
                <asp:TemplateField HeaderText="Total" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" >
                    <ItemTemplate><asp:Literal ID="litTotal" runat="server" /></ItemTemplate>
                    <FooterTemplate><%=_gridTotal.ToString("c") %></FooterTemplate>
                </asp:TemplateField> 
            </Columns>
        </asp:GridView>
    </div>
</div>
<asp:ObjectDataSource ID="ObjectDataSource1" runat="server" EnablePaging="True" SelectMethod="GetMerchInventoryInRange"
    TypeName="Wcss.InventoryMerchInRange" SelectCountMethod="GetMerchInventoryInRange_Count" 
    OnSelecting="objData_Selecting" OnSelected="objData_Selected" >
    <SelectParameters>
        <asp:QueryStringParameter Name="parentId" QueryStringField="merchitem" Type="Int32" />
        <asp:ControlParameter ControlID="ddlStyle" Name="style" PropertyName="SelectedValue" Type="string" />
        <asp:ControlParameter ControlID="ddlColor" Name="color" PropertyName="SelectedValue" Type="string" />
        <asp:ControlParameter ControlID="ddlSize" Name="size" PropertyName="SelectedValue" Type="string" />
        <asp:ControlParameter ControlID="rdoStatus" Name="activeStatus" PropertyName="SelectedValue" Type="string" />
    </SelectParameters>
</asp:ObjectDataSource>
<asp:SqlDataSource ID="SqlItemReporter" runat="server" EnableCaching="false" ConnectionString="<%$ ConnectionStrings:WillCallConnectionString %>"
    SelectCommand="SET @style = ISNULL(@style,''); SET @color = ISNULL(@color,''); SET @size = ISNULL(@size,''); SET @activeStatus = LOWER(@activeStatus); 
        SET @startDate = CAST(CONVERT(varchar(256), @startdate, 101) as DateTime); SET @endDate = DATEADD(mi, -1, CAST(CONVERT(varchar(256), DATEADD(dd,1,@endDate), 101) as DateTime)); 
        CREATE TABLE #tmpMerch ( merchId int ); 
        IF (@gridId > 0) SET @parentId = @gridId; IF EXISTS (SELECT * FROM Merch m WHERE m.[Id] = @parentId AND tParentListing IS NULL) 
        BEGIN INSERT INTO #tmpMerch(merchId) SELECT	m.[Id] as 'merchId' 
        FROM Merch m WHERE m.[tParentListing] = @parentId AND CASE @Style WHEN '' THEN 1 ELSE CASE WHEN m.[Style] = @Style THEN 1 ELSE 0 END END = 1 AND 
        CASE @Color WHEN '' THEN 1 ELSE CASE WHEN m.[Color] = @Color THEN 1 ELSE 0 END END = 1 AND CASE @Size WHEN '' THEN 1 ELSE CASE WHEN m.[Size] = @Size THEN 1 ELSE 0 END END = 1 AND 
        CASE @ActiveStatus WHEN 'true' THEN CASE WHEN (m.[bActive] IS NULL OR (m.[bActive] IS NOT NULL AND m.[bActive] = 1)) THEN 1 ELSE 0 END 
        WHEN 'false' THEN CASE WHEN (m.[bActive] IS NOT NULL AND m.[bActive] = 0) THEN 1 ELSE 0 END ELSE 1 END = 1 END ELSE BEGIN INSERT	INTO #tmpMerch(merchId) 
        SELECT	m.[Id] as 'merchId' FROM Merch m WHERE m.[Id] = @parentId END SELECT COUNT(ii.[Id]) as 'NumLineItems', SUM(ii.[iQuantity]) as 'UnitsSold', SUM(ii.[mLineItemTotal]) as 'SaleTotal' 
        FROM	[InvoiceItem] ii, [Invoice] i, [#tmpMerch] m WHERE	ii.[tMerchId] = m.[merchId] AND  ii.[PurchaseAction] = 'Purchased' AND ii.[tInvoiceId] = i.[Id] AND i.[dtInvoiceDate] 
        BETWEEN @startDate AND @endDate "  
    onselecting="SqlItemReporter_Selecting">
    <SelectParameters>
        <asp:QueryStringParameter Name="parentId" QueryStringField="merchitem" Type="Int32" />
        <asp:ControlParameter ControlID="GridView1" Name="gridId" DefaultValue="0" PropertyName="SelectedValue" Type="int32" />
        <asp:ControlParameter ControlID="rdoStatus" Name="activeStatus" PropertyName="SelectedValue" Type="string" />
        <asp:ControlParameter ControlID="clockStart" Name="startDate" PropertyName="SelectedDate" Type="DateTime" />
        <asp:ControlParameter ControlID="clockEnd" Name="endDate" PropertyName="SelectedDate" Type="DateTime" />
        <asp:Parameter Name="style" Type="string" DefaultValue="" ConvertEmptyStringToNull="false" />
        <asp:Parameter Name="color" Type="string" DefaultValue="" ConvertEmptyStringToNull="false" />
        <asp:Parameter Name="size" Type="string" DefaultValue="" ConvertEmptyStringToNull="false" />
    </SelectParameters>
</asp:SqlDataSource>
<asp:ObjectDataSource ID="ObjectDataSource2" runat="server" EnablePaging="True" SelectMethod="GetMerchSalesInRange"
    TypeName="Wcss.CustomerInvoiceRow" SelectCountMethod="GetMerchSalesInRangeCount"
    OnSelecting="objData_Selecting" OnSelected="objData_Selected" >
    <SelectParameters>
        <asp:QueryStringParameter Name="parentId" QueryStringField="merchitem" Type="Int32" />
        <asp:ControlParameter ControlID="GridView1" Name="gridId" DefaultValue="0" PropertyName="SelectedValue" Type="int32" />
        <asp:ControlParameter ControlID="ddlStyle" Name="style" PropertyName="SelectedValue" Type="string" />
        <asp:ControlParameter ControlID="ddlColor" Name="color" PropertyName="SelectedValue" Type="string" />
        <asp:ControlParameter ControlID="ddlSize" Name="size" PropertyName="SelectedValue" Type="string" />
        <asp:ControlParameter ControlID="rdoStatus" Name="activeStatus" PropertyName="SelectedValue" Type="string" />
        <asp:ControlParameter ControlID="clockStart" Name="startDate" PropertyName="SelectedDate" Type="DateTime" />
        <asp:ControlParameter ControlID="clockEnd" Name="endDate" PropertyName="SelectedDate" Type="DateTime" />
    </SelectParameters>
</asp:ObjectDataSource>