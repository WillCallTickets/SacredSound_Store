<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Report_TixSales.ascx.cs" EnableViewState="true" 
    Inherits="WillCallWeb.Admin.AdminControls.Report_TixSales" %>
<%@ Register src="~/Components/Util/CalendarClock.ascx" tagname="CalendarClock" tagprefix="uc1" %>
<%@ Register src="~/Components/Navigation/gglPager.ascx" tagname="gglPager" tagprefix="uc2" %>

<asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
            <ContentTemplate>

<div id="showsales">
    <div class="jqhead rounded">
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="edittabl">
            <tr>
                <th style="text-align:left;">DATE BREAKDOWN</th>
                <td width="100%" style="text-align:center;padding-right:22px;">
                    <asp:DropDownList ID="ddlVenue" runat="server" AppendDataBoundItems="true" DataSourceID="sql1"
                        OnDataBound="ddlVenue_DataBound" CssClass="fxddl"
                        DataTextField="Name" DataValueField="Id" Width="100%" AutoPostBack="true" EnableViewState="false" 
                        OnSelectedIndexChanged="ddlVenue_SelectedIndexChanged" Font-Bold="true" Font-Size="10px" >
                        <asp:ListItem Selected="True" Value="0" Text="<-- Search All Venues -->" />
                    </asp:DropDownList>
                </td>
                <th>Start</th>
                <td style="padding:0;">
                    <uc1:CalendarClock ID="clockStart" runat="server" UseTime="false" UseReset="false" 
                        OnInit="clock_Init" OnSelectedDateChanged="clock_SelectedDateChanged" />
                </td>
                <th>End</th>
                <td style="padding:0;">
                    <uc1:CalendarClock ID="clockEnd" runat="server" UseReset="false" UseTime="false" 
                        OnInit="clock_Init" OnSelectedDateChanged="clock_SelectedDateChanged" />
                </td>
                <td align="right">
                    <asp:Button ID="btnRefresh" CausesValidation="false" CssClass="btnmed" 
                        OnClick="btnRefresh_Click" runat="server" CommandName="Refresh" Text="Refresh" />
                </td>
            </tr>
        </table>
    </div>
        <uc2:gglPager ID="GooglePager1" runat="server" PageButtonClass="btntny"  />
        
        <div class="jqpanelresult">
            <asp:ListView ID="Listing" runat="server" DataKeyNames="ShowDateId" ItemPlaceholderID="ListViewContent" 
                EnableViewState="false"
                oninit="Listing_Init"
                ondatabinding="Listing_DataBinding" 
                OnItemDataBound="Listing_ItemDataBound"  
                OnDataBound="Listing_DataBound"                  
                DataMember="SimpleShowDateRecords">
                <EmptyDataTemplate>
                    <div class="lstempty">No Data For Selected Criteria</div>
                </EmptyDataTemplate>
                <LayoutTemplate>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" class="lsttbl">
                        <tbody runat="server" id="ListViewContent" />
                    </table>
                </LayoutTemplate>
                <ItemTemplate>                    
                    <asp:Literal ID="litRowStart" runat="server" EnableViewState="false" />
                        <asp:Literal ID="ShowDescriptionCell" runat="server" />
                        <th>TktId</th>
                        <th>DOS</th>
                        <th>Ages</th>
                        <th>Base</th>
                        <th>Svc</th>
                        <th>Per</th>
                        <th>Avail</th>
                        <th>Pend</th>
                        <th>Sold</th>
                        <th>Pkg</th>
                        <th>Act</th>
                        <th>SO</th>
                        <th>Ticket</th>
                        <th>Service</th>
                        <th>Total</th>
                        <th class="roundtopright">Refunds</th>
                    </tr>
                    <asp:Repeater ID="rptTix" EnableViewState="false" runat="server" OnItemDataBound="rptTix_ItemDataBound">
                        <ItemTemplate>
                            <asp:Literal ID="litRowStart" runat="server" EnableViewState="false" />
                                <td><%#Eval("Id") %></td>
                                <td align="center" nowrap="true"><asp:Literal Visible='<%#Eval("bDosTicket") %>' ID="litDosIndicator" runat="server" 
                                        Text="<span style='color:red;'>DOS</span>" /></td>
                                <td align="center" nowrap="true"><asp:Literal ID="litAges" runat="server" /></td>
                                <td align="center" nowrap="true"><asp:Literal ID="litPrice" runat="server" /></td>
                                <td align="center" nowrap="true"><asp:Literal ID="litService" runat="server" /></td>
                                <td align="center" nowrap="true"><asp:Literal ID="litPer" runat="server" /></td>
                                <td align="center" nowrap="true"><asp:Literal ID="litAvail" runat="server" /></td>
                                <td align="center" nowrap="true"><asp:Literal ID="litPend" runat="server" /></td>
                                <td align="center" nowrap="true"><asp:Literal ID="litSold" runat="server" /></td>
                                <td align="center"><asp:Checkbox ID="chkPackage" runat="server" Checked='<%#Eval("IsPackage") %>' Enabled="false" /></td>
                                <td align="center"><asp:Checkbox ID="chkActive" runat="server" Checked='<%#Eval("IsActive") %>' Width="20" Enabled="false" />
                                <td align="center"><asp:Checkbox ID="chkSoldOut" runat="server" Checked='<%#Eval("IsSoldOut") %>' Width="20" Enabled="false" /></td>
                                <td align="center"><asp:Literal ID="litItemBase" runat="server" /></td>
                                <td align="center"><asp:Literal ID="litItemSvc" runat="server" /></td>
                                <td align="center"><asp:Literal ID="litItemTotal" runat="server" /></td>
                                <td align="center" nowrap="true"><asp:Literal ID="litRefund" runat="server" /></td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Literal ID="litRowStart" runat="server" EnableViewState="false" />
                                <td colspan="6">&nbsp;</td>
                                <td><asp:Literal ID="litAvail" runat="server" /></td>
                                <td><asp:Literal ID="litPend" runat="server" /></td>
                                <td><asp:Literal ID="litSold" runat="server" /></td>
                                <td colspan="3">&nbsp;</td>
                                <td><asp:Literal ID="litTotBase" runat="server" /></td>
                                <td><asp:Literal ID="litTotSvc" runat="server" /></td>
                                <td><asp:Literal ID="litTot" runat="server" /></td>
                                <td><asp:Literal ID="litRef" runat="server" /></td>
                            </tr>
                            <asp:Literal ID="litRowStartClose" runat="server" EnableViewState="false" />                                
                                <th colspan="17" class="sec-closer roundbot" >
                                    <div style="line-height:2px;" >&nbsp;</div>
                                </th>
                             </tr>
                        </FooterTemplate>
                     </asp:Repeater>
                 </ItemTemplate>

                 <ItemSeparatorTemplate><tr><td colspan="17" style="background-color:Transparent;line-height:6px;">&nbsp;</td></tr></ItemSeparatorTemplate>
            </asp:ListView>
    </div>
</div>

</ContentTemplate>
</asp:UpdatePanel>


<asp:SqlDataSource ID="sql1" runat="server" ConnectionString="<%$ ConnectionStrings:WillCallConnectionString %>" 
    SelectCommand="SELECT (v.[Name] + ' - ' + CASE WHEN v.[City] IS NOT NULL AND LEN(LTRIM(RTRIM(v.[City]))) > 0 
    THEN LTRIM(RTRIM(v.[City])) ELSE '' END + CASE WHEN v.[City] IS NOT NULL AND LEN(LTRIM(RTRIM(v.[City]))) > 0 AND v.[State] IS NOT NULL AND LEN(LTRIM(RTRIM(v.[State]))) > 0 
    THEN ', ' ELSE '' END + CASE WHEN v.[State] IS NOT NULL AND LEN(LTRIM(RTRIM(v.[State]))) > 0 THEN UPPER(LTRIM(RTRIM(v.[State]))) ELSE '' END) as 'Name', v.[Id] 
    FROM [Venue] v WHERE v.[ApplicationId] = @appId ORDER BY v.[NameRoot] ASC "
    OnSelecting="sql1_Selecting">
    <SelectParameters>
        <asp:Parameter Name="appId" DbType="String" />
    </SelectParameters>
</asp:SqlDataSource>


