<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Reports_InventoryBundles.ascx.cs" Inherits="WillCallWeb.Admin.AdminControls.Reports_InventoryBundles" %>
<%@ Register src="~/Components/Navigation/gglPager.ascx" tagname="gglPager" tagprefix="uc2" %>
<%@ Register src="~/Components/Util/CalendarClock.ascx" tagname="CalendarClock" tagprefix="uc1" %>
<div id="inventorybundles">
     <div class="jqhead rounded">
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="edittabl">
            <tr>
                <th style="text-align:left;">BUNDLE INVENTORY</th>
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
                <td style="width:100%;padding-left:22px;text-align:right;">
                    <asp:Button ID="btnCsvAll" runat="server" CssClass="btnmed" Text="Get CSV" CommandName="csvall" OnClick="CSV_Click" 
                        OnClientClick="return confirm('This will make a download available for items in the ENTIRE BATCH. Would you like to proceed?');" />
                    &nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btnRefresh" CausesValidation="false" CssClass="btnmed" 
                        OnClick="btnRefresh_Click" runat="server" CommandName="Refresh" Text="Refresh" />
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="edittabl">
            <tr>
                <th>Category</th>
                <td><asp:DropDownList ID="ddlCategory" Width="250px" runat="server" 
                    cssclass="fxddl" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged" AutoPostBack="true" >
                        <asp:ListItem Selected="True" Value="all">All</asp:ListItem>
                        <asp:ListItem Value="merch">Merch</asp:ListItem>
                        <asp:ListItem Value="ticket">Tickets</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <th>Active</th>
                <td><asp:RadioButtonList ID="rdoStatus" runat="server" AutoPostBack="True" CellPadding="4" CellSpacing="4" Font-Bold="true" 
                        OnSelectedIndexChanged="rdoStatus_SelectedIndexChanged" RepeatDirection="Horizontal" RepeatLayout="Flow" >
                        <asp:ListItem Selected="True" Value="true">Active &nbsp;</asp:ListItem>
                        <asp:ListItem Value="false">InActive</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
        </table>
    </div> 
     <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="jqpanel1 rounded">
                <uc2:gglPager ID="GooglePager1" runat="server" PageButtonClass="btntny" />
                <asp:GridView ID="GridView1" AutoGenerateColumns="False" Width="100%" runat="server" AllowPaging="True" 
                    DataSourceID="ObjectDataSource1" DataKeyNames="BundleId" OnRowDataBound="GridView1_RowDataBound" OnDataBinding="GridView1_DataBinding" 
                    OnDataBound="GridView1_DataBound" 
                    ShowFooter="false" EnableViewState="false" OnInit="GridView1_Init" CssClass="lsttbl">
                    <PagerSettings Visible="false" />
                    <AlternatingRowStyle BackColor="#e1e1e1" />
                    <EmptyDataTemplate>
                        <div class="lstempty">No Data For Selected Criteria</div>
                    </EmptyDataTemplate>
                    <Columns>
                        <asp:TemplateField HeaderText="Edit" HeaderStyle-HorizontalAlign="left">
                            <ItemTemplate>
                                <asp:HyperLink ID="linkEdit" runat="server" Text='<%#Eval("ParentDescription") %>' NavigateUrl='<%# "/Admin/MerchEditor.aspx?p=Bundle&merchitem=" + Eval("TMerchId") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Description" HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="left" >
                            <ItemTemplate>
                                <asp:Literal ID="litDescription" runat="server" EnableViewState="false" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:CheckBoxField DataField="IsActive" HeaderText="Active" ItemStyle-HorizontalAlign="center" />
                        <asp:BoundField DataField="RequiredParentQty" HeaderText="Req" ItemStyle-HorizontalAlign="center" />
                        <asp:BoundField DataField="MaxSelections" HeaderText="Max" ItemStyle-HorizontalAlign="center" />
                        <asp:BoundField DataField="Price" HeaderText="Price" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="center" />
                        <asp:CheckBoxField DataField="IsIncludeWeight" HeaderText="Wgt" ItemStyle-HorizontalAlign="center" />
                        <asp:BoundField DataField="NumBundlesSold" HeaderText="Sold" ItemStyle-HorizontalAlign="center" />
                        <asp:BoundField DataField="BundleSales" HeaderText="Sales" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="center" />
                        <asp:BoundField DataField="NumBundlesRefunded" HeaderText="B-Ref" ItemStyle-HorizontalAlign="center" />
                        <asp:BoundField DataField="BundleRefunds" HeaderText="B-Refs" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="center" />
                        <asp:BoundField DataField="NumItemsSold" HeaderText="I-Sold" ItemStyle-HorizontalAlign="center" />
                        <asp:BoundField DataField="NumItemsRefunded" HeaderText="I-Ref" ItemStyle-HorizontalAlign="center" />
                    </Columns>
                </asp:GridView>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
 <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" EnablePaging="True"
    SelectCountMethod="GetBundleReportRows_Count" SelectMethod="GetBundleReportRows" TypeName="Wcss.QueryRow.SalesReportBundleRow"
    OnSelecting="objData_Selecting" OnSelected="objData_Selected" >
    <SelectParameters>
        <asp:ControlParameter ControlID="ddlCategory" DefaultValue="all" Name="category" PropertyName="SelectedValue" Type="string" />
        <asp:ControlParameter ControlID="rdoStatus" DefaultValue="true" Name="activeStatus" PropertyName="SelectedValue" Type="string" />
        <asp:ControlParameter ControlID="clockStart" Name="startDate" PropertyName="SelectedDate"
            Type="DateTime" DefaultValue="1/1/2000" />
        <asp:ControlParameter ControlID="clockEnd" Name="endDate" PropertyName="SelectedDate"
            Type="DateTime" DefaultValue="1/1/2050" />
    </SelectParameters>
</asp:ObjectDataSource>
                
    


