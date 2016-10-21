<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Reports_InventoryTickets.ascx.cs" Inherits="WillCallWeb.Admin.AdminControls.Reports_InventoryTickets" %>
<%@ Register src="~/Components/Util/CalendarClock.ascx" tagname="CalendarClock" tagprefix="uc1" %>
<%@ Register src="~/Components/Navigation/gglPager.ascx" tagname="gglPager" tagprefix="uc2" %>
<div id="inventorytickets">
     <div class="jqhead rounded">
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="edittabl">
            <tr>
                <th style="width:100%;text-align:left;">TICKET INVENTORY</th>
                <th>Start</th>
                <td style="padding:0;">
                    <uc1:CalendarClock ID="clockStart" runat="server" UseReset="false" UseTime="false" OnInit="clock_Init" OnSelectedDateChanged="clock_SelectedDateChanged" />
                </td>
                <th>End</th>
                <td style="padding:0;">
                    <uc1:CalendarClock ID="clockEnd" runat="server" UseReset="false" UseTime="false" OnInit="clock_Init" OnSelectedDateChanged="clock_SelectedDateChanged" />
                </td>
                <td align="right">
                    <asp:Button ID="btnRefresh" CausesValidation="false" CssClass="btnmed" 
                        OnClick="btnRefresh_Click" runat="server" CommandName="Refresh" Text="refresh" />
                </td>
            </tr>
        </table>
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
            <ContentTemplate>
    <div class="jqpanel1 rounded">
        <uc2:gglPager ID="GooglePager1" runat="server" PageButtonClass="btntny" />
        <asp:GridView ID="GridView1" AutoGenerateColumns="False" Width="100%" runat="server" AllowPaging="True" 
            DataSourceID="ObjectDataSource1" OnRowDataBound="GridView1_RowDataBound" OnDataBinding="GridView1_DataBinding" 
            OnDataBound="GridView1_DataBound" OnRowCommand="GridView1_RowCommand"
            ShowFooter="true" EnableViewState="false" OnInit="GridView1_Init" CssClass="lsttbl">
            <PagerSettings Visible="false" />
            <AlternatingRowStyle CssClass="altgridrow" />
            <EmptyDataTemplate>
                <div class="lstempty">There are no tickets to display in the selected date range</div>
            </EmptyDataTemplate>
            <Columns>
                <asp:TemplateField HeaderText="Show Description" HeaderStyle-HorizontalAlign="left">
                    <ItemTemplate>
                        <div>
                            <asp:HyperLink ID="linkShowDate" ToolTip="Details for this show date" runat="server" />
                            <div>
                                <asp:HyperLink ID="linkTicket" ToolTip="Details for this ticket" runat="server" Text='<%#Eval("ShowDate", "{0:MM/dd/yyyy hh:mmtt}") %>' />&nbsp;
                                <%#Eval("AgeName") %>&nbsp;
                                <%#Eval("Price", "{0:n2}")%> + <%#Eval("ServiceCharge", "{0:n2}") %>svc = <%#Eval("TotalPrice", "{0:c}") %>
                            </div>
                            <asp:Literal ID="litDescCrit" runat="server" />
                            <asp:Literal ID="litStatus" runat="server" />
                            
                            <%if(Page.User.IsInRole("Super")){ %>
                            <div>
                                <asp:Button ID="btnSync" CssClass="btntny" runat="server" Text="Sync" CommandName="sync" CommandArgument='<%#Eval("ShowTicketId")%>' />
                            </div>
                            <%} %>
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:CheckBoxField DataField="IsActive" HeaderText="Act" ItemStyle-HorizontalAlign="center" />
                <asp:CheckBoxField DataField="IsSoldOut" HeaderText="SO" ItemStyle-HorizontalAlign="center" />
                <asp:TemplateField HeaderText="DOS">
                    <ItemTemplate>
                        <asp:Literal Visible='<%#Eval("IsDosTicket") %>' ID="litDosIndicator" runat="server" 
                            Text="<span style='color:red;'>DOS</span>" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Allot" ItemStyle-HorizontalAlign="center" >
                    <ItemTemplate>
                        <%#Eval("Allot") %>
                        <div>Actual:</div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Pend" ItemStyle-HorizontalAlign="center" >
                    <ItemTemplate>
                        <%#Eval("Pend") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Sold" ItemStyle-HorizontalAlign="center" >
                    <ItemTemplate>
                        <%#Eval("Sold") %>
                        <div><asp:Literal ID="litPurchased" runat="server" /></div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Avail" HeaderText="Avail" ItemStyle-HorizontalAlign="center" />
                <asp:TemplateField HeaderText="Refund" ItemStyle-HorizontalAlign="center" >
                    <ItemTemplate>
                        <%#Eval("Refund") %>
                        <div><asp:Literal ID="litRefund" runat="server" /></div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Tix" ItemStyle-HorizontalAlign="center" >
                    <ItemTemplate><asp:Literal ID="litTix" runat="server" /></ItemTemplate>
                    <FooterStyle HorizontalAlign="center" />
                    <FooterTemplate><%= _totalTix.ToString("n") %></FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Fees" ItemStyle-HorizontalAlign="center" >
                    <ItemTemplate><asp:Literal ID="litFee" runat="server" /></ItemTemplate>
                    <FooterStyle HorizontalAlign="center" />
                    <FooterTemplate><%= _totalFee.ToString("n") %></FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Totals" ItemStyle-HorizontalAlign="right" >
                    <ItemTemplate><asp:Literal ID="litTotal" runat="server" /></ItemTemplate>
                    <FooterStyle HorizontalAlign="right" />
                    <FooterTemplate><%= _totalAll.ToString("c") %></FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="On/Off" HeaderStyle-HorizontalAlign="Center" ItemStyle-Wrap="false" ItemStyle-Font-Size="XX-Small">
                    <ItemTemplate>
                        <asp:Literal ID="litOffsale" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    </ContentTemplate>
    </asp:UpdatePanel>
</div>
<asp:ObjectDataSource ID="ObjectDataSource1" EnablePaging="true" runat="server" SelectMethod="GetInventoryDiscrepancies_Ticket"
    TypeName="Wcss.QueryRow.InventoryDiscrep_TicketRow" SelectCountMethod="GetInventoryDiscrepancies_TicketCount"
    OnSelecting="objData_Selecting" OnSelected="objData_Selected"
    >
    <SelectParameters>
        <asp:ControlParameter ControlID="clockStart" Name="startDate" PropertyName="SelectedDate"
            Type="DateTime" />
        <asp:ControlParameter ControlID="clockEnd" Name="endDate" PropertyName="SelectedDate"
            Type="DateTime" />
    </SelectParameters>
</asp:ObjectDataSource>