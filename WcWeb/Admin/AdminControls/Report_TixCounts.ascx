<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Report_TixCounts.ascx.cs" Inherits="WillCallWeb.Admin.AdminControls.Report_TixCounts" %>
<%@ Register src="~/Components/Util/CalendarClock.ascx" tagname="CalendarClock" tagprefix="uc1" %>
<%@ Register src="~/Components/Navigation/gglPager.ascx" tagname="gglPager" tagprefix="uc2" %>

<asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
            <ContentTemplate>
<div id="ticketcounts">
    <div class="jqhead rounded">
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="edittabl">
            <tr>
                <th style="width:100%;text-align:left;">DATE OVERVIEW</th>
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
                <td align="right">
                    <asp:Button ID="btnRefresh" CausesValidation="false" CssClass="btnmed" 
                        OnClick="btnRefresh_Click" runat="server" CommandName="Refresh" Text="Refresh" />
                </td>
            </tr>
        </table>
    </div>
    <div class="jqpanel1 rounded">
        <uc2:gglPager ID="GooglePager1" runat="server" PageButtonClass="btntny"  />
         <asp:GridView ID="GridView1" runat="server"  Width="100%" AllowPaging="True" EnableViewState="false" 
        AutoGenerateColumns="False" DataKeyNames="ShowDateId" DataSourceID="ObjectDataSource1" 
         cssClass="lsttbl"
         ShowFooter="true" 
         OnInit="GridView1_Init"
         ondatabinding="GridView1_DataBinding" 
         onrowdatabound="GridView1_RowDataBound"
         onDataBound="GridView1_DataBound">
         <PagerSettings Visible="false" />
         <RowStyle HorizontalAlign="Center" />
         <FooterStyle HorizontalAlign="Center" />
         <AlternatingRowStyle CssClass="altgridrow" />
         <EmptyDataTemplate>
            <div class="lstempty">No Data For Selected Date Range</div>
        </EmptyDataTemplate>
        <Columns>
            <asp:HyperLinkField DataTextField="ShowDate" DataTextFormatString="{0:MM/dd/yy hh:mmtt}" HeaderText="Date" ControlStyle-Font-Underline="true" 
                DataNavigateUrlFormatString="/Admin/Listings.aspx?p=tickets&shodateid={0}" DataNavigateUrlFields="ShowDateId" ItemStyle-Wrap="false" />
            <asp:TemplateField HeaderText="Show Name" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="left" >
                <ItemTemplate><asp:Literal id="litName" runat="server" /></ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Sold">
                <ItemTemplate><%#Eval("Sold") %></ItemTemplate><FooterTemplate><%=_tSold %></FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Available">
                <ItemTemplate><%#Eval("Available") %></ItemTemplate><FooterTemplate><%=_tAvail %></FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Today">
                <ItemTemplate><%#Eval("ToDay")%></ItemTemplate><FooterTemplate><%=_today %></FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="1 Day">
                <ItemTemplate><%#Eval("OneDay")%></ItemTemplate><FooterTemplate><%=_1day %></FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="2 Day">
                <ItemTemplate><%#Eval("TwoDay")%></ItemTemplate><FooterTemplate><%=_2day %></FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="3 Day">
                <ItemTemplate><%#Eval("ThreeDay")%></ItemTemplate><FooterTemplate><%=_3day %></FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="4 Day">
                <ItemTemplate><%#Eval("FourDay")%></ItemTemplate><FooterTemplate><%=_4day %></FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="5 Day">
                <ItemTemplate><%#Eval("FiveDay")%></ItemTemplate><FooterTemplate><%=_5day %></FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Alloted">
                <ItemTemplate><%#Eval("Allotment")%></ItemTemplate><FooterTemplate><%=_tAllot %></FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Pending">
                <ItemTemplate><%#Eval("Pending") %></ItemTemplate><FooterTemplate><%=_tPend %></FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Refund">
                <ItemTemplate><%#Eval("Refunded")%></ItemTemplate><FooterTemplate><%=_tRefund %></FooterTemplate>
            </asp:TemplateField>
        </Columns>
     </asp:GridView>
     </div>
</div>

</ContentTemplate>
</asp:UpdatePanel>

<asp:ObjectDataSource ID="ObjectDataSource1" runat="server" EnablePaging="True" SelectMethod="GetTicketCounts"
    TypeName="Wcss.QueryRow.TicketCountRow" SelectCountMethod="GetTicketCountsCount"
    OnSelecting="objData_Selecting" OnSelected="objData_Selected">
    <SelectParameters>
        <asp:ControlParameter ControlID="clockStart" Name="startDate" PropertyName="SelectedDate"
            Type="DateTime" />
        <asp:ControlParameter ControlID="clockEnd" Name="endDate" PropertyName="SelectedDate"
            Type="DateTime" />
    </SelectParameters>
</asp:ObjectDataSource>








