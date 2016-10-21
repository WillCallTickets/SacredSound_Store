<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Report_MerchDetail.ascx.cs" Inherits="WillCallWeb.Admin.AdminControls.Report_MerchDetail" %>
<%@ Register src="~/Components/Util/CalendarClock.ascx" tagname="CalendarClock" tagprefix="uc1" %>

<asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
    <ContentTemplate>

<div id="ticketcounts">
    <div class="jqhead rounded">
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="edittabl">
            <tr>
                <th style="width:100%;text-align:left;">Merchandise Breakdown
                    <div class="intr">Please note that merch items can belong to more than one category</div>
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
            AutoGenerateColumns="False" DataSourceID="ObjectDataSource1" 
             cssClass="lsttbl"
             PageSize="1050" 
             ShowFooter="true" 
             ondatabinding="GridView1_DataBinding" 
             onrowdatabound="GridView1_RowDataBound">
         <PagerSettings Visible="false" />         
         <RowStyle HorizontalAlign="Left" />
         <EmptyDataTemplate>
            <div class="lstempty">No Data For Selected Date Range</div>
        </EmptyDataTemplate>        
        <Columns>
            <asp:TemplateField HeaderText="Division" HeaderStyle-HorizontalAlign="Left" ItemStyle-CssClass="division-header">
                <ItemTemplate>
                    <asp:Literal ID="litDivision" runat="server" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="CatName" HeaderText="Category" HeaderStyle-HorizontalAlign="Left" ItemStyle-Font-Bold="true" />
            <asp:BoundField DataField="NumItemsSold" HeaderText="# Sold" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
            <asp:BoundField DataField="TotalSales" DataFormatString="{0:n2}" HeaderText="$$$ In Period" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />            
        </Columns>        
     </asp:GridView>
     </div>
</div>
</ContentTemplate>
</asp:UpdatePanel>

<asp:ObjectDataSource ID="ObjectDataSource1" runat="server" EnablePaging="False" SelectMethod="GetMerchSalesDetailInPeriod"
    TypeName="Wcss.QueryRow.MerchSalesDetailRow" >
    <SelectParameters>
        <asp:ControlParameter ControlID="clockStart" Name="startDate" PropertyName="SelectedDate"
            Type="DateTime" />
        <asp:ControlParameter ControlID="clockEnd" Name="endDate" PropertyName="SelectedDate"
            Type="DateTime" />
    </SelectParameters>
</asp:ObjectDataSource>








