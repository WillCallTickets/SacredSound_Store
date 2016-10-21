<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ShowChooser.ascx.cs" Inherits="WillCallWeb.Admin.ShowChooser" %>
<%@ Register src="~/Components/Util/CalendarClock.ascx" tagname="CalendarClock" tagprefix="uc1" %>
<div id="showchooser" style="margin-bottom:4px;">
    <div class="jqhead rounded">
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="hedtbl">
            <tr>
                <th>CHOOSE SHOW</th>
                <td style="width:100%;padding:0px 14px 0 14px;border:solid white 0px;">
                        <asp:DropDownList ID="ddlShow" runat="server" width="100%" AutoPostBack="True" 
                            DataSourceID="SqlShowList" DataTextField="ShowName" DataValueField="Id" 
                            OnSelectedIndexChanged="ddlShow_SelectedIndexChanged"
                            OnDataBound="ddlShow_DataBound" />
                </td>
                <th>Start</th>
                <td>
                    <uc1:CalendarClock ID="clockContext" runat="server" UseTime="false" UseReset="false" OnInit="clock_Init" 
                         ValidationGroup="entity" OnSelectedDateChanged="clock_DateChange" />
                </td>
            </tr>
        </table>
    </div>
</div>
<asp:SqlDataSource ID="SqlShowList" runat="server" ConnectionString="<%$ ConnectionStrings:WillCallConnectionString %>"    
    SelectCommand="SELECT TOP 200 s.[Id], (s.[Name] + ' ' + ISNULL(v.[City],'') + 
    CASE WHEN v.[City] IS NOT NULL AND LEN(LTRIM(RTRIM(v.[City]))) > 0 AND v.[State] IS NOT NULL AND LEN(LTRIM(RTRIM(v.[State]))) > 0 
    THEN ', ' ELSE '' END + ISNULL(v.[State],'')) as ShowName, MAX(sd.[dtDateOfShow]) as MaxDate FROM [Show] s FULL OUTER JOIN [ShowDate] sd ON sd.[tShowId] = s.[Id], 
    Venue v WHERE s.[ApplicationId] = @appId AND s.[TVenueId] = v.[Id] GROUP BY s.[Id], s.[Name], v.[City], v.[State] HAVING MAX(sd.[dtDateOfShow]) >= @date ORDER BY s.[Name] " 
        EnableViewState="True" onselecting="SqlShowList_Selecting">
    <SelectParameters>
        <asp:Parameter Name="appId" DbType="Guid" />
        <asp:ControlParameter Name="date" DbType="String" ControlID="clockContext" PropertyName="SelectedValue" />
    </SelectParameters>
</asp:SqlDataSource>