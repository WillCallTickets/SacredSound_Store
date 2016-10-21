<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Chooser_Ticket.ascx.cs" Inherits="WillCallWeb.Admin.AdminControls.Choosers.Chooser_Ticket" %>
<%@ Register src="~/Components/Util/CalendarClock.ascx" tagname="CalendarClock" tagprefix="uc1" %>
<div id="srceditor">
    <div id="ChooserTicket-Container" class="Normal">
        <asp:UpdatePanel ID="UpdatePanelChooserTicket" runat="server">
            <ContentTemplate>
                <div class="selectors">
                    <div><span>List Start</span><span class="clk"><uc1:CalendarClock ID="clockBaseline" runat="server" UseReset="false" UseTime="false" OnInit="clock_Init" /></span></div>
                    <div><span>Dates</span><asp:DropDownList ID="ddlDates" runat="server" AutoPostBack="true" DataSourceID="SqlDates" DataTextField="Name" DataValueField="Idx" /></div>
                    <div><span>Tickets</span><asp:DropDownList ID="ddlTickets" runat="server" DataSourceID="SqlTicketsForDate" DataTextField="Name" DataValueField="Idx" /></div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</div>

<!-- Get a list of dates in context-->
<asp:SqlDataSource ID="SqlDates" runat="server" ConnectionString="<%$ ConnectionStrings:WillCallConnectionString %>" 
    SelectCommand="SELECT CONVERT(varchar, sd.[dtDateOfShow], 101) + ' ' + 
        SUBSTRING(CAST(sd.[dtDateOfShow] as varchar), LEN(CAST(sd.[dtDateOfShow] as varchar))-7, 1000) + ' ' + 
        SUBSTRING(s.[Name], 22, LEN(s.[Name])) as [Name], sd.[Id] as [Idx]
        FROM [ShowDate] sd JOIN [Show] s ON sd.[tShowId] = s.[Id] WHERE sd.[dtDateOfShow] > @DateBaseline ORDER BY sd.[dtDateOfShow]; "
    OnSelecting="SqlDates_Selecting" >
    <SelectParameters>
        <asp:ControlParameter ControlID="clockBaseline" PropertyName="SelectedDate" DbType="DateTime" Name="DateBaseline" />
    </SelectParameters>
</asp:SqlDataSource>
<!-- Show tickets for the selected date -->
<asp:SqlDataSource ID="SqlTicketsForDate" runat="server" ConnectionString="<%$ ConnectionStrings:WillCallConnectionString %>" 
    SelectCommand="SELECT ('ID: ' + CAST(st.[Id] as varchar) + ' ' + CONVERT(varchar, sd.[dtDateOfShow], 101) + ' ' + 
        SUBSTRING(CAST(sd.[dtDateOfShow] as varchar), LEN(CAST(sd.[dtDateOfShow] as varchar))-7, 1000) + 
    ' ' + SUBSTRING(s.[Name], 22, LEN(s.[Name])) + ' $' + CAST(st.[mPrice] as varchar) + ' + ' + CAST(st.[mServiceCharge] as varchar) + ' ' + ISNULL(st.[SalesDescription],'') + ' ' + ISNULL(st.[CriteriaText],'')) as [Name],
    st.[Id] as [Idx] 
    FROM [ShowTicket] st LEFT OUTER JOIN [ShowDate] sd ON sd.[Id] = st.[tShowDateId] LEFT OUTER JOIN [Show] s ON s.[Id] = st.[tShowId] 
    WHERE st.[tShowDateId] = @ShowDateId ORDER BY st.[iDisplayOrder]; " >
    <SelectParameters>
        <asp:ControlParameter ControlID="clockBaseline" PropertyName="SelectedDate" DbType="DateTime" Name="DateBaseline" />
        <asp:ControlParameter ControlID="ddlDates" PropertyName="SelectedValue" DbType="Int32" Name="ShowDateId" />
    </SelectParameters>
</asp:SqlDataSource>

<script language="javascript" type="text/javascript">
    // Get a reference to the PageRequestManager.
    var prm = Sys.WebForms.PageRequestManager.getInstance();
    var selectionPanelId = '<%= this.UpdatePanelChooserTicket.UniqueID %>';

    var clock = getChildElement(selectionPanelId, "clockBaseline");
    var dates = getChildElement(selectionPanelId, "ddlDates");
    var tickets = getChildElement(selectionPanelId, "ddlTickets");

    // Using that prm reference, hook _initializeRequest
    // and _endRequest, to run our code at the begin and end
    // of any async postbacks that occur.
    prm.add_initializeRequest(InitializeRequest);
    prm.add_endRequest(EndRequest);

    // Executed anytime an async postback occurs.
    function InitializeRequest(sender, args) {
        // Change the Container div's CSS class to .Progress.
        $get('ChooserTicket-Container').className = 'Progress';

        // Get a reference to the element that raised the postback,
        //   and disables it.
        $get(args._postBackElement.id).disabled = true;

        //disable drop downs
        if (clock)
            clock.disabled = true;
        if (dates)
            dates.disabled = true;
        if (tickets)
            tickets.disabled = true;
    }

    // Executed when the async postback completes.
    function EndRequest(sender, args) {
        // Change the Container div's class back to .Normal.
        $get('ChooserTicket-Container').className = 'Normal';

        // Get a reference to the element that raised the postback
        //   which is completing, and enable it.
        $get(sender._postBackSettings.sourceElement.id).disabled = false;

        //enable drop downs
        if (clock)
            clock.disabled = false;
        if (dates)
            dates.disabled = false;
        if (tickets)
            tickets.disabled = false;

    }
</script>