<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Mailer_ListGenerator.ascx.cs" 
    Inherits="WillCallWeb.Admin.AdminControls.Mailer_ListGenerator" %>
<%@ Register src="~/Components/Util/CalendarClock.ascx" tagname="CalendarClock" tagprefix="uc1" %>
<div id="srceditor">
    <div id="mailereditor">
        <div class="jqhead rounded">
            <h3>CUSTOMER LISTING <span class="intr">This will target all sales - duplicate email entries are possible</span></h3>
        </div>
        <div class="jqpnl rounded">
            <table border="0" cellpadding="0" cellspacing="0" width="100%" class="edittabl">
                <tr>
                    <th>Show</th>
                    <td>
                        <asp:DropDownList ID="ddlShowList" runat="server" Width="100%" AppendDataBoundItems="false" 
                            AutoPostBack="true" DataSourceID="SqlShowList" DataTextField="ShowName" DataValueField="ShowId" 
                            EnableViewState="true" >
                        </asp:DropDownList>
                    </td>
                    <th>Start</th>
                    <td style="padding:0;">
                        <uc1:CalendarClock ID="clockStart" runat="server" UseReset="false" UseTime="false" OnInit="clock_Init" 
                            OnSelectedDateChanged="clock_SelectedDateChanged" />       
                    </td>
                    <th>End</th>
                    <td style="padding:0;">
                        <uc1:CalendarClock ID="clockEnd" runat="server" UseTime="false" UseReset="false" OnInit="clock_Init" 
                            OnSelectedDateChanged="clock_SelectedDateChanged" />
                    </td>
                </tr>
                <tr>
                    <th style="vertical-align:top;">Dates</th>
                    <td>
                        <asp:CheckBoxList ID="chkShowDates" runat="server" AppendDataBoundItems="false" AutoPostBack="true" EnableViewState="false"
                            CellPadding="0" CellSpacing="0" DataSourceID="SqlDateList" DataTextField="DateName" DataValueField="DateId"
                            RepeatDirection="Vertical" RepeatLayout="Flow" TextAlign="Right" OnSelectedIndexChanged="chkShowDates_SelectedIndexChanged">
                        </asp:CheckBoxList>
                    </td>
                    <td rowspan="2" colspan="4" style="vertical-align:top !important;white-space:nowrap;text-align:center;">
                        <asp:RadioButtonList ID="rdoPurchase" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" BorderStyle="None">
                            <asp:ListItem Selected="True" Text="Purchases" Value="purchases" />
                            <asp:ListItem Text="Refunds" Value="refunds" />
                        </asp:RadioButtonList>
                        <hr />
                        <asp:RadioButtonList ID="rblShipContext" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" 
                            OnDataBinding="rblShipContext_DataBinding" OnDataBound="rblShipContext_DataBound" >
                        </asp:RadioButtonList>
                        
                        <br /><br />
                        <asp:Button ID="btnLoadDate" runat="server" Text="List From Selected Date(s)" cssclass="btnmed" Width="150px"
                            OnClientClick="return confirm('This will create a list from the selected date(s)');" 
                            onclick="btnLoad_Click" CausesValidation="false" EnableViewState="false" />
                        <br /><br />
                        <asp:Button ID="btnLoadTicket" runat="server" Text="List From Selected Ticket(s)" cssclass="btnmed" Width="150px"
                            OnClientClick="return confirm('This will create a list from the selected tickets');" 
                            onclick="btnLoad_Click" CausesValidation="false" EnableViewState="false" />
                        
                        
                    </td>
                </tr>
                <tr>
                    <th style="vertical-align:top;">Tickets</th>
                    <td>
                        <asp:CheckBoxList ID="chkShowTickets" runat="server" AppendDataBoundItems="false" AutoPostBack="false" EnableViewState="false"
                            CellPadding="0" CellSpacing="0" DataSourceID="SqlTicketList" DataTextField="TicketName" DataValueField="TicketId"
                            RepeatDirection="Vertical" RepeatLayout="Flow" TextAlign="Right" >                            
                        </asp:CheckBoxList>
                    </td>
                </tr>
            </table>
        </div>
        <div class="jqpnl rounded" style="margin-top:4px;">
            <table border="0" cellpadding="0" cellspacing="0" width="100%" class="edittabl">
                <tr>
                    <th>Merch</th>
                    <td colspan="2" style="width:100%;">
                        <asp:DropDownList ID="ddlParentList" runat="server" Width="100%" AppendDataBoundItems="false" 
                            AutoPostBack="true" DataSourceID="SqlParentList" DataTextField="ParentName" DataValueField="ParentId" 
                            EnableViewState="false">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th style="vertical-align:top;">Inventory</th>
                    <td>
                        <asp:CheckBoxList ID="chkChildren" runat="server" AppendDataBoundItems="false" AutoPostBack="false" EnableViewState="false"
                            CellPadding="0" CellSpacing="0" DataSourceID="SqlChildrenList" DataTextField="ChildName" DataValueField="ChildId"
                            RepeatDirection="Vertical" RepeatLayout="Flow" TextAlign="Right">
                        </asp:CheckBoxList>
                    </td>
                    <td style="vertical-align:top !important;">
                        <asp:Button ID="btnLoadMerch" runat="server" Text="List From Selected Children" cssclass="btnmed" Width="150px"
                            OnClientClick="return confirm('This will create a list from the selected merchandise items');" 
                            onclick="btnLoad_Click" CausesValidation="false" EnableViewState="false" />
                    </td>
                </tr>
            </table>
        </div>
        <asp:ValidationSummary ID="ValidationSummary1" CssClass="validationsummary" HeaderText="Please correct the following errors:" 
            ValidationGroup="sendemail" runat="server" />
        <div class="jqpnl rounded" style="margin-top:4px;">
            <table border="0" cellpadding="0" cellspacing="0" width="100%" class="edittabl">
                <tr>
                    <th>
                        <asp:CustomValidator ID="CustomSendTest" runat="server" ValidationGroup="sendemail" CssClass="validator" 
                            Display="Static" ErrorMessage="CustomValidator">*</asp:CustomValidator>Test Email
                    </th>
                    <td>
                        <asp:TextBox ID="txtTestEmail" runat="server" MaxLength="256" Width="300px" EnableViewState="false" />
                    </td>
                    <td>
                        <asp:Button ID="btnSendTest" runat="server" Text="Send Test Email" cssclass="btnmed" Width="100px" EnableViewState="false"
                            onclick="btnSendTest_Click" CausesValidation="false" ToolTip="Separate email addresses with comma" />
                    </td>
                    <td style="white-space:nowrap;font-size:10px;padding-right:42px;">
                        (separate emails with commas)
                    </td>
                    <td style="width:100%;">
                        <asp:Button ID="btnSend" runat="server" Text="Send To List" cssclass="btnmed" Width="100px"
                            OnClientClick="return confirm('This will send the email to all emails in the list');" 
                            onclick="btnSend_Click" CausesValidation="false" EnableViewState="false" />
                        <asp:CustomValidator ID="CustomSend" runat="server" ValidationGroup="sendemail" CssClass="validator" 
                            Display="Static" ErrorMessage="CustomValidator">*</asp:CustomValidator>
                    </td>
                </tr>
                <tr>
                    <td colspan="7">
                        <asp:Label ForeColor="Green" Font-Bold="true" ID="lblStatus" runat="server" EnableViewState="false" />
                    </td>
                </tr>
                <tr>
                    <td colspan="7" class="jqinstruction rounded">
                        Removing duplicate entries will cause any INVOICEID or PURCHASENAME PARAMS to be blank!
                    </td>
                </tr>
                <tr>
                    <th>Customer List &#9660;</th>
                    <td>
                        <asp:Button ID="btnDisplayList" runat="server" Text="Display List" cssclass="btnmed" Width="80px"
                            onclick="btnDisplayList_Click" CausesValidation="false" EnableViewState="false" />
                        <asp:Button ID="btnClearList" runat="server" Text="Clear List" cssclass="btnmed" Width="80px"
                            onclick="btnClearList_Click" CausesValidation="false" EnableViewState="false" />
                        <asp:Button ID="btnDupes" runat="server" Text="No Dupes" cssclass="btnmed" Width="80px"
                            onclick="btnDupes_Click" CausesValidation="false" EnableViewState="false" />
                    </td>
                    <th style="text-align:left;padding-left:12px;">Email Preview &#9660;</th>
                    <td colspan="2" style="width:100%;">
                        <asp:DropDownList ID="ddlEmailLetter" runat="server" Width="100%" AppendDataBoundItems="false" 
                            AutoPostBack="true" DataTextField="Name" DataValueField="Id" 
                            EnableViewState="true" 
                            OnDataBinding="ddlEmailLetter_DataBinding"
                            OnDataBound="ddlEmailLetter_DataBound" 
                            OnSelectedIndexChanged="ddlEmailLetter_SelectedIndexChanged" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:ListBox ID="lstEmails" runat="server" Width="100%" Height="400px" EnableViewState="true" 
                            SelectionMode="Multiple"></asp:ListBox>
                    </td>
                    <td colspan="3" style="background-color:#ffffff;">
                        <asp:Literal ID="litPreview" runat="server" EnableViewState="false" OnDataBinding="litPreview_DataBinding" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
</div>

<asp:SqlDataSource ID="SqlParentList" runat="server" ConnectionString="<%$ ConnectionStrings:WillCallConnectionString %>"
    SelectCommand="
        IF EXISTS (SELECT * FROM [Merch] WHERE [tParentListing] IS NULL) BEGIN
        SELECT m.[Name] as 'ParentName', m.[Id] as 'ParentId' 
        FROM Merch m 
        WHERE m.[ApplicationId] = @appId AND m.[tParentListing] IS NULL
        ORDER BY m.[Name] ASC END " OnSelecting="SqlParentList_Selecting">
     <SelectParameters>
        <asp:Parameter Name="appId" DbType="Guid" />
    </SelectParameters>   
</asp:SqlDataSource>


<asp:SqlDataSource ID="SqlShowList" runat="server" ConnectionString="<%$ ConnectionStrings:WillCallConnectionString %>"
    SelectCommand="
        CREATE TABLE #tmpShowIds(ShowId int); 
        INSERT #tmpShowIds(ShowId) 
        SELECT DISTINCT(s.[Id]) AS 'ShowId' 
        FROM [ShowDate] sd, [Show] s
        WHERE sd.[dtDateOfShow] BETWEEN @startDate AND @endDate AND sd.[tShowId] = s.[Id] AND s.[ApplicationId] = @appId 
        
        IF EXISTS (SELECT * FROM [#tmpShowIds]) BEGIN 
        SELECT ' [..Select Show..]' as ShowName, 0 as 'ShowId' UNION  
        SELECT s.[Name] + ' - ' + 
        ISNULL(v.[City],'') + ' ' + ISNULL(v.[State],'') as ShowName, s.[Id] as 'ShowId' 
        FROM #tmpShowIds ids, Show s LEFT OUTER JOIN [Venue] v ON s.[tVenueId] = v.[Id] 
        WHERE ids.[ShowId] = s.[Id] AND s.[ApplicationId] = @appId 
        ORDER BY ShowName ASC END" 
    onselecting="SqlShowList_Selecting">
    <SelectParameters>
        <asp:Parameter Name="appId" DbType="Guid" />
        <asp:ControlParameter ControlID="clockStart" Name="startDate" PropertyName="SelectedDate"
            Type="DateTime" />
        <asp:ControlParameter ControlID="clockEnd" Name="endDate" PropertyName="SelectedDate"
            Type="DateTime" />
    </SelectParameters>    
</asp:SqlDataSource>

<asp:SqlDataSource ID="SqlDateList" runat="server" ConnectionString="<%$ ConnectionStrings:WillCallConnectionString %>"
    SelectCommand="
        DECLARE @count int 
        SET @count = 0
        SELECT @count = COUNT([Id]) FROM [ShowDate] WHERE [tShowId] = @showId 
        IF (@count > 0) BEGIN 
            IF (@count > 1) BEGIN 
                SELECT ' [..All Dates..]' as DateName, 0 as DateId 
                UNION 
                SELECT CONVERT(varchar,sd.[dtDateOfShow],101) + ' ' + SUBSTRING(CONVERT(varchar,sd.[dtDateOfShow],100), 12, 8) as DateName, sd.[Id] as DateId 
                FROM ShowDate sd 
                WHERE sd.[tShowId] = @showId
                ORDER BY DateName ASC 
            END ELSE BEGIN 
                SELECT CONVERT(varchar,sd.[dtDateOfShow],101) + ' ' + SUBSTRING(CONVERT(varchar,sd.[dtDateOfShow],100), 12, 8) as DateName, sd.[Id] as DateId 
                FROM ShowDate sd 
                WHERE sd.[tShowId] = @showId
                ORDER BY sd.[dtDateOfShow] ASC 
            END 
        END " >
    <SelectParameters>
        <asp:ControlParameter Name="showId" Type="String" ControlID="ddlShowList" PropertyName="SelectedValue" />
    </SelectParameters>    
</asp:SqlDataSource>


<asp:SqlDataSource ID="SqlTicketList" runat="server" ConnectionString="<%$ ConnectionStrings:WillCallConnectionString %>"
    SelectCommand="SELECT 0" 
    onselecting="SqlTicketList_Selecting">
    <SelectParameters>
        <asp:ControlParameter Name="showId" Type="Int32" ControlID="ddlShowList" PropertyName="SelectedValue" DefaultValue="0" />
    </SelectParameters>    
</asp:SqlDataSource>

<asp:SqlDataSource ID="SqlChildrenList" runat="server" ConnectionString="<%$ ConnectionStrings:WillCallConnectionString %>"
    SelectCommand="
        DECLARE @count int 
        SET @count = 0
        SELECT @count = COUNT([Id]) FROM [Merch] WHERE [tParentListing] = @parentId 
        IF (@count > 0) BEGIN 
            IF (@count > 1) BEGIN 
                SELECT ' [..All Inventory..]' as ChildName, 0 as ChildId 
                UNION 
                SELECT '(' + CAST(m.iSold as varchar) + ') ' + 
                    CASE WHEN m.[Style] IS NOT NULL AND LEN(m.[Style]) > 0 THEN m.[Style] + ' ' ELSE '' END + 
                    CASE WHEN m.[Color] IS NOT NULL AND LEN(m.[Color]) > 0 THEN m.[Color] + ' ' ELSE '' END + 
                    CASE WHEN m.[Size] IS NOT NULL AND LEN(m.[Size]) > 0 THEN m.[Size] + ' ' ELSE '' END + 
                    CAST(m.[mPrice] as varchar) as ChildName, m.[Id] as ChildId 
                FROM Merch m 
                WHERE m.[tParentListing] = @parentId
                ORDER BY ChildName ASC 
            END ELSE BEGIN 
                SELECT '(' + CAST(m.iSold as varchar) + ') ' + 
                    CASE WHEN m.[Style] IS NOT NULL AND LEN(m.[Style]) > 0 THEN m.[Style] + ' ' ELSE '' END + 
                    CASE WHEN m.[Color] IS NOT NULL AND LEN(m.[Color]) > 0 THEN m.[Color] + ' ' ELSE '' END + 
                    CASE WHEN m.[Size] IS NOT NULL AND LEN(m.[Size]) > 0 THEN m.[Size] + ' ' ELSE '' END + 
                    CAST(m.[mPrice] as varchar) as ChildName, m.[Id] as ChildId 
                FROM Merch m 
                WHERE m.[tParentListing] = @parentId 
                ORDER BY ChildName ASC 
            END 
        END " >
    <SelectParameters>
        <asp:ControlParameter Name="parentId" Type="Int32" ControlID="ddlParentList" PropertyName="SelectedValue" DefaultValue="0" />
    </SelectParameters>    
</asp:SqlDataSource>