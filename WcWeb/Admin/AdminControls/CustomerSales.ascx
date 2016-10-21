<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CustomerSales.ascx.cs" Inherits="WillCallWeb.Admin.AdminControls.CustomerSales" %>
<%@ Register src="~/Components/Navigation/gglPager.ascx" tagname="gglPager" tagprefix="uc2" %>
<div id="customersales">
     <div class="jqhead rounded">
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="edittabl" style="margin:0;">
            <tr>
                <th style="text-align:left;" class="entry-title">SALES HISTORY</th>
                <td style="text-align:right;width:100%;">
                    <asp:Button ID="btnRefresh" CausesValidation="false" runat="server" CommandName="Refresh" cssclass="btnmed"
                        OnClick="btnRefresh_Click" Text="Refresh" />
                </td>
            </tr>
        </table>
    </div>
    <div class="jqpnl rounded" style="margin-bottom:2px;">
        <h3 class="entry-title"><asp:Literal ID="LiteralUserName" runat="server" /></h3>
    </div>
    <uc2:gglPager ID="GooglePager1" runat="server" PageButtonClass="btntny"  />
    <div class="jqpnl rounded">
    <asp:GridView ID="GridView1" width="100%" runat="server" AutoGenerateColumns="False" EnableViewState="false"
        AllowPaging="True" DataKeyNames="InvoiceId" DataSourceID="ObjectCustomerInvoices" CssClass="lsttbl"
        OnInit="GridView1_Init" 
        OnDataBinding="GridView1_DataBinding" 
        OnRowDataBound="GridView1_RowDataBound" 
        OnDataBound="GridView1_DataBound" >
        <PagerSettings Visible="false" />
        <EmptyDataTemplate>
            <div class="lstempty">Customer has no purchases</div>
        </EmptyDataTemplate>
        <Columns>
            <asp:TemplateField ItemStyle-HorizontalAlign="center" ItemStyle-CssClass="rowcounter">
                <ItemTemplate><asp:Literal ID="LiteralRowCounter" runat="server" /></ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Select Invoice" HeaderStyle-HorizontalAlign="left" ItemStyle-Wrap="false">
                <ItemTemplate>
                    <asp:Literal ID="LiteralSelect" runat="server" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="TransactionType" HeaderText="Type" ItemStyle-HorizontalAlign="Center" />
            <asp:BoundField DataField="InvoiceStatus" HeaderText="Status" ItemStyle-HorizontalAlign="center" />
             <asp:TemplateField HeaderText="Description" HeaderStyle-HorizontalAlign="left" ItemStyle-Width="30%">
                <ItemTemplate>
                    <asp:Literal ID="LiteralDescription" runat="server" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="PurchaserName" HeaderText="Name" HeaderStyle-HorizontalAlign="left" />
            <asp:BoundField DataField="TaxAmount" DataFormatString="{0:n2}" HtmlEncode="False" HeaderText="Tax" ItemStyle-HorizontalAlign="center" />
            <asp:BoundField DataField="FreightAmount" DataFormatString="{0:n2}" HtmlEncode="False" HeaderText="Ship" ItemStyle-HorizontalAlign="center" />
            <asp:BoundField DataField="TotalRefunds" DataFormatString="{0:n2}" HtmlEncode="False" HeaderText="Refund" ItemStyle-HorizontalAlign="center" />
            <asp:BoundField DataField="TotalPaid" DataFormatString="{0:c}" HtmlEncode="False" HeaderText="Paid" ItemStyle-HorizontalAlign="right" />
            <asp:BoundField DataField="NetPaid" DataFormatString="{0:c}" HtmlEncode="False" HeaderText="Net Paid" ItemStyle-HorizontalAlign="right" />
        </Columns>
    </asp:GridView>
    </div>
</div>
<asp:ObjectDataSource ID="ObjectCustomerInvoices" runat="server" SelectMethod="GetCustomerSalesHistory" SelectCountMethod="GetCustomerSalesHistoryCount"
    TypeName="Wcss.CustomerInvoiceRow" EnablePaging="True" OnSelected="objData_Selected" OnSelecting="objData_Selecting">
    <SelectParameters>
        <asp:QueryStringParameter Name="userName" QueryStringField="UserName" Type="String" />
    </SelectParameters>
</asp:ObjectDataSource>