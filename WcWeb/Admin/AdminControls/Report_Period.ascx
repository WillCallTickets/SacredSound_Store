<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Report_Period.ascx.cs" Inherits="WillCallWeb.Admin.AdminControls.Report_Period" %>
<%@ Register src="~/Components/Util/CalendarClock.ascx" tagname="CalendarClock" tagprefix="uc1" %>

<asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
    <ContentTemplate>

<div id="ticketcounts">
    <div class="jqhead rounded">
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="edittabl">
            <tr>
                <th style="width:100%;text-align:left;">Service Fee Breakdown
                    <div class="intr">Tix bought in this period for shows within this period:
                        <asp:Literal ID="litTixInPeriod" runat="server" EnableViewState="false" OnDataBinding="litTixInPeriod_DataBinding" />
                    </div>
                </th>
                <th>Start</th>
                <td style="padding:0;">
                    <uc1:CalendarClock ID="clockStart" runat="server" UseReset="false" UseTime="false" OnInit="clock_Init" 
                        OnSelectedDateChanged="clock_SelectedDateChanged" /> </td>
                <td style="text-indent:-18px;">12AM </td>
                <th>End</th>
                <td style="padding:0;">
                    <uc1:CalendarClock ID="clockEnd" runat="server" UseTime="false" UseReset="false" OnInit="clock_Init" 
                        OnSelectedDateChanged="clock_SelectedDateChanged" /> 
                </td>
                <td style="text-indent:-18px;">11:59PM</td>
                <td align="right">
                    <asp:Button ID="btnRefresh" CausesValidation="false" CssClass="btnmed"  
                        OnClick="btnRefresh_Click" runat="server" CommandName="Refresh" Text="Refresh" />
                </td>
            </tr>
        </table>
    </div>
    <div class="jqpanel1 rounded">
         <asp:GridView ID="GridView1" runat="server"  Width="100%" AllowPaging="False" EnableViewState="false" 
            AutoGenerateColumns="False" DataKeyNames="ServiceCharge" DataSourceID="ObjectDataSource1" 
             cssClass="lsttbl"
             PageSize="1050" 
             ShowFooter="true" 
             ondatabinding="GridView1_DataBinding" 
             onrowdatabound="GridView1_RowDataBound"
             OnDataBound="GridView1_DataBound">
         <PagerSettings Visible="false" />
         <RowStyle HorizontalAlign="Center" />
         <FooterStyle HorizontalAlign="Center" />
         <EmptyDataTemplate>
            <div class="lstempty">No Data For Selected Date Range</div>
        </EmptyDataTemplate>
        <Columns>
            <asp:BoundField DataField="ServiceCharge" DataFormatString="{0:c}" HeaderText="ServiceCharge" />
            <asp:TemplateField HeaderText="Num Items Sold" >
                <ItemTemplate><%#Eval("NumItems") %></ItemTemplate>
                <FooterTemplate><%=_numItems %></FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="BasePriceTotal" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" >
                <ItemTemplate><%#Eval("BasePriceTotal", "{0:n2}")%></ItemTemplate>
                <FooterTemplate><%=_basePriceTotal.ToString("c")%></FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="ServiceChargeTotal" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" >
                <ItemTemplate><%#Eval("ServiceChargeTotal", "{0:n2}")%></ItemTemplate>
                <FooterTemplate><%=_serviceChargeTotal.ToString("c")%></FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="LineItemTotal" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" >
                <ItemTemplate><%#Eval("LineItemTotal", "{0:n2}")%></ItemTemplate>
                <FooterTemplate><%=_lineItemTotal.ToString("c")%></FooterTemplate>
            </asp:TemplateField>
        </Columns>
     </asp:GridView>
     </div>
</div>
</ContentTemplate>
</asp:UpdatePanel>

<asp:ObjectDataSource ID="ObjectDataSource1" runat="server" EnablePaging="False" SelectMethod="GetServiceFeeBreakdownInPeriod"
    TypeName="Wcss.QueryRow.ServiceFeeBreakdownRow" >
    <SelectParameters>
        <asp:ControlParameter ControlID="clockStart" Name="startDate" PropertyName="SelectedDate"
            Type="DateTime" />
        <asp:ControlParameter ControlID="clockEnd" Name="endDate" PropertyName="SelectedDate"
            Type="DateTime" />
    </SelectParameters>
</asp:ObjectDataSource>








