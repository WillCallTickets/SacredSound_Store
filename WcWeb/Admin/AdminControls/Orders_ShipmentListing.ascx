<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Orders_ShipmentListing.ascx.cs" Inherits="WillCallWeb.Admin.AdminControls.Orders_ShipmentListing" %>
<%@ Register src="~/Components/Util/CalendarClock.ascx" tagname="CalendarClock" tagprefix="uc1" %>
<%@ Register src="~/Components/Navigation/gglPager.ascx" tagname="gglPager" tagprefix="uc2" %>
<div id="ordersshipmentlisting">
    <div class="jqhead rounded">
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="edittabl">
            <tr>
                <th style="width:100%;text-align:left;">SHIPMENTS</th>
                <th>
                    <asp:Button CssClass="btnmed" ID="btnActual" runat="server" Text="Save Actual" Width="80px" 
                        ToolTip="Record actual shipments" OnClick="btnActual_Click" CausesValidation="false" />
                </th>
                <th style="padding:0 0 0 12px;">
                    <asp:DataList ID="DataListContext" DataKeyField="Value" 
                        RepeatLayout="Flow" runat="server" RepeatDirection="Horizontal" OnDataBinding="DataListContext_DataBinding" 
                        OnSelectedIndexChanged="DataListContext_SelectedIndexChanged">
                        <SelectedItemStyle CssClass="contextselect" />
                        <ItemTemplate>
                            <asp:Button CssClass="btnmed" ID="btnSelect" runat="server" Text='<%#Eval("Text") %>' 
                                CommandName="Select" CommandArgument='<%#Eval("Value") %>' CausesValidation="false" />
                        </ItemTemplate>
                    </asp:DataList>
                </th>
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
                    <asp:Button ID="btnRefresh" CausesValidation="false" CssClass="btnmed" runat="server" CommandName="Refresh" Text="refresh" />
                </td>
            </tr>
        </table>
    </div>
    <uc2:gglPager ID="GooglePager1" runat="server" PageButtonClass="btntny"  />
    <asp:Label ID="lblError" runat="server" EnableViewState="false" Visible="false" CssClass="validationsummary" />
    <asp:GridView ID="GridView1" Width="100%" EnableViewState="false" 
        runat="server" CssClass="lsttbl" AutoGenerateColumns="false"
        DataSourceID="ObjectDataSource1" DataKeyNames="Id"  
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
            <asp:TemplateField HeaderText="Created" ItemStyle-HorizontalAlign="center" ItemStyle-Wrap="false" >
                <ItemTemplate>
                    <asp:HyperLink ID="lnkCreated" runat="server" Text='<%#Eval("dtCreated","{0:MM/dd/yy hh:mmtt}") %>' EnableViewState="false" 
                        NavigateUrl='<%# "/Admin/Orders.aspx?p=shipping&amp;Inv=" + Eval("TInvoiceId")%>' ></asp:HyperLink>
                    <asp:Literal ID="litShipped" runat="server" EnableViewState="false" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:CheckBoxField DataField="IsLabelPrinted" ItemStyle-HorizontalAlign="center" HeaderText="Printed" />
            <asp:TemplateField HeaderText="Actual" ItemStyle-HorizontalAlign="center">
                <ItemTemplate>
                    <asp:TextBox ID="txtActual" runat="server" Width="50px" EnableViewState="false" MaxLength="8" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Ship Address" HeaderStyle-HorizontalAlign="left" >
                <ItemTemplate>
                    <asp:Literal ID="litAddress" runat="server" EnableViewState="false" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="PackingList" HeaderStyle-HorizontalAlign="left" >
                <ItemTemplate>
                    <asp:Literal ID="litPacking" runat="server" EnableViewState="false" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="ShipMethod" HeaderText="Method" ItemStyle-HorizontalAlign="center" />
        </Columns>
    </asp:GridView>
</div>
<asp:ObjectDataSource ID="ObjectDataSource1" runat="server" EnablePaging="true" EnableCaching="false" 
    TypeName="Wcss.InvoiceShipment" SelectMethod="GetInvoiceShipmentsInRange" SelectCountMethod="GetInvoiceShipmentsInRangeCount"     
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

