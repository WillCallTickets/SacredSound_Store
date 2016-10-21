<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Orders_Recent.ascx.cs" Inherits="WillCallWeb.Admin.AdminControls.Orders_Recent" %>
<%@ Register src="~/Components/Util/CalendarClock.ascx" tagname="CalendarClock" tagprefix="uc1" %>
<%@ Register src="~/Components/Navigation/gglPager.ascx" tagname="gglPager" tagprefix="uc2" %>
<div id="ordersrecent">
    <div class="jqhead rounded">
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="edittabl" style="margin:0;">
            <tr>
                <th style="width:25%;text-align:left;">RECENT</th>
                <th style="padding:0 0 0 12px;">
                    <asp:DataList ID="DataListContext" DataKeyField="Value" 
                        RepeatLayout="Flow" runat="server" RepeatDirection="Horizontal" OnDataBinding="DataListContext_DataBinding" 
                        OnSelectedIndexChanged="DataListContext_SelectedIndexChanged" ItemStyle-HorizontalAlign="Center">
                        <SelectedItemStyle CssClass="contextselect" />
                        <ItemTemplate>
                            <asp:Button CssClass="btnmed" Width="50px" ID="btnSelect" runat="server" Text='<%#Eval("Text") %>' CommandName="Select" 
                                CommandArgument='<%#Eval("Value") %>' CausesValidation="false" />
                        </ItemTemplate>
                    </asp:DataList>
                </th>
                <th>Start</th>
                <td style="padding:0;">
                   <uc1:CalendarClock ID="clockStart" runat="server" UseTime="false" UseReset="false" OnInit="clock_Init" 
                        OnSelectedDateChanged="clock_SelectedDateChanged" />
                </td>
                <th>End</th>
                <td style="padding:0;">
                    <uc1:CalendarClock ID="clockEnd" runat="server" UseReset="false" UseTime="false" OnInit="clock_Init" 
                        OnSelectedDateChanged="clock_SelectedDateChanged" />
                </td>
                <td>
                    &nbsp;
                </td>
                <td align="right">
                    <asp:Button ID="btnRefresh" CausesValidation="false" CssClass="btnmed" Width="75px" runat="server" CommandName="Refresh" Text="refresh" />
                </td>
            </tr>
        </table>
    </div>
    <uc2:gglPager ID="GooglePager1" runat="server" PageButtonClass="btntny"  />
    <asp:GridView ID="GridView1" Width="100%" EnableViewState="false" 
        runat="server" CssClass="lsttbl" AutoGenerateColumns="false"
        DataSourceID="ObjectDataSource1" DataKeyNames="InvoiceId"  
        AllowPaging="true" 
        OnDataBinding="GridView1_DataBinding" 
        OnDataBound="GridView1_DataBound"
        OnRowDataBound="GridView1_RowDataBound" 
        OnInit="GridView1_Init" >
        <RowStyle BackColor="#ffffff" />
        <PagerSettings Visible="false" />
        <EmptyDataTemplate>
            <div class="lstempty">No Data For Selected Date Range &amp; Criteria</div>
        </EmptyDataTemplate>
        <Columns>
            <asp:TemplateField ItemStyle-HorizontalAlign="center" ItemStyle-CssClass="rowcounter">
                <ItemTemplate><asp:Literal ID="LiteralRowCounter" runat="server" /></ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Ship Status" ItemStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" ItemStyle-Wrap="true" >
                <ItemTemplate>
                    <asp:Literal ID="litShipping" runat="server" />
                </ItemTemplate>
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

            <asp:TemplateField HeaderText="Description" HeaderStyle-HorizontalAlign="Left" ItemStyle-Wrap="true" >
                <ItemTemplate>
                    <asp:Literal ID="literalDescription" runat="server" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="FreightAmount" HtmlEncode="False" DataFormatString="{0:n2}" HeaderText="Shipping" 
                ItemStyle-HorizontalAlign="center" SortExpression="FreightAmount" />   
            <asp:BoundField DataField="TotalPaid" DataFormatString="{0:n2}" HtmlEncode="False" HeaderText="Total" 
                SortExpression="TotalPaid" ItemStyle-HorizontalAlign="center" />    
            <asp:BoundField DataField="InvoiceStatus" HeaderText="Status" SortExpression="Status" ItemStyle-HorizontalAlign="Center" />
        </Columns>
    </asp:GridView>
</div>
<asp:ObjectDataSource ID="ObjectDataSource1" runat="server" EnablePaging="true" 
    SelectMethod="GetOrdersInRange" EnableCaching="false" 
    TypeName="Wcss.CustomerInvoiceRow" SelectCountMethod="GetOrdersInRangeCount"  
    OnSelecting="objData_Selecting" OnSelected="objData_Selected" >
    <SelectParameters>
        <asp:ControlParameter ControlID="DataListContext" Name="context" PropertyName="SelectedValue"
            Type="Object" />
        <asp:ControlParameter ControlID="clockStart" Name="startDate" PropertyName="SelectedDate"
            Type="DateTime" />
        <asp:ControlParameter ControlID="clockEnd" Name="endDate" PropertyName="SelectedDate"
            Type="DateTime" />
    </SelectParameters>
</asp:ObjectDataSource>

