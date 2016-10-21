<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Report_SalesAll.ascx.cs" Inherits="WillCallWeb.Admin.AdminControls.Report_SalesAll" %>
<%@ Register src="~/Components/Util/CalendarClock.ascx" tagname="CalendarClock" tagprefix="uc1" %>
<%@ Register src="~/Components/Navigation/gglPager.ascx" tagname="gglPager" tagprefix="uc2" %>

<asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
    <Triggers>
        <asp:PostBackTrigger ControlID="btnCsv" />
    </Triggers>
    <ContentTemplate>

<div id="allsales">
    <div class="jqhead rounded">
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="edittabl">
            <tr>
                <th>ALL SALES</th>
                <td style="width:100%;text-align:center;">
                    <%if (_report != null)
                      {%>
                    <input type="button" id="btnPrint" onclick="doPagePopup('/Admin/PrintSalesReport.aspx?start=<%=_report.StartDate.Ticks.ToString() %>&end=<%=_report.EndDate.Ticks.ToString() %>' ,'false')" class="btnmed" style="width:80px" value="Print Report" />
                    <asp:Button ID="btnCsv" runat="server" OnClick="btnCsv_Click" CommandName="csvreport" CssClass="btnmed" Width="80px" Text="Get Csv" />
                    <%}
                      else
                      { %>
                    &nbsp;
                    <%} %>
                </td>
                <th>Start</th>
                <td style="padding:0;">
                    <uc1:CalendarClock ID="clockStart" runat="server" UseTime="false" UseReset="false" 
                        OnInit="clock_Init" OnSelectedDateChanged="clock_SelectedDateChanged" />
                </td>
                <td style="text-indent:-18px;">12AM</td>
                <th>End</th>
                <td style="padding:0;">
                    <uc1:CalendarClock ID="clockEnd" runat="server" UseReset="false" UseTime="false" 
                        OnInit="clock_Init" OnSelectedDateChanged="clock_SelectedDateChanged" />
                </td>
                <td style="text-indent:-18px;">11:59PM</td>
                <td align="right">
                    <asp:Button ID="btnRefresh" CausesValidation="false" CssClass="btnmed"  
                        OnClick="btnRefresh_Click" runat="server" CommandName="Refresh" Text="Refresh" />
                </td>
            </tr>
        </table>
    </div>
    <div class="rounded" style="margin-top:12px;padding-bottom:48px;">
        <asp:Panel ID="pnlReport" runat="server" EnableViewState="false" OnDataBinding="pnlReport_DataBinding" />
    </div>
</div>

</ContentTemplate>
</asp:UpdatePanel>
