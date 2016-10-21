<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Customer_SalesHistory.ascx.cs" Inherits="WillCallWeb.Controls.Customer_SalesHistory" %>
<div id="customerhistory">
    <h4>Sales History</h4>
    <asp:ValidationSummary HeaderText="The following errors ocurred:" ID="ValidationSummary1" runat="server" 
        ValidationGroup="Picker" CssClass="validationsummary" Width="100%" />
    <asp:CustomValidator ID="CustomValidation" runat="server" ValidationGroup="Picker" CssClass="invisible" 
        Display="dynamic" ErrorMessage="CustomValidator">*</asp:CustomValidator>
    <asp:GridView ID="GridView1" EnableViewState="false" CellPadding="3" Width="100%" runat="server" AutoGenerateColumns="False" OnDataBound="GridView1_DataBound" 
        AllowPaging="True" GridLines="Both" DataKeyNames="InvoiceId" OnDataBinding="GridView1_DataBinding" CssClass="crewtable" PageSize="10"  
        DataSourceID="ObjectDataSource1" OnRowCommand="GridView1_RowCommand" OnRowDataBound="GridView1_RowDataBound">
        <EmptyDataTemplate>
            No purchases.
        </EmptyDataTemplate>
        <Columns>
            <asp:TemplateField ItemStyle-HorizontalAlign="center" ItemStyle-CssClass="rowcounter">
                <ItemTemplate><asp:Literal ID="LiteralRowCounter" runat="server" /></ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="InvoiceDate" HtmlEncode="false" DataFormatString="{0:MM/dd/yyyy hh:mmtt}" HeaderText="Date" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="120px" />
            <asp:TemplateField HeaderText="Description" HeaderStyle-HorizontalAlign="Left">
                <ItemTemplate>
                    <asp:Literal ID="LiteralDescription" runat="Server" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="TotalPaid" HtmlEncode="false" DataFormatString="{0:c}" HeaderText="Paid" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"/>
            <asp:BoundField DataField="TotalRefunds" HtmlEncode="false" DataFormatString="{0:c}" HeaderText="Refunds" ItemStyle-HorizontalAlign="Center"/>
            <asp:BoundField DataField="NetPaid" HtmlEncode="false" DataFormatString="{0:c}" HeaderText="Net" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"/>        
            <asp:TemplateField ItemStyle-Wrap="false" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center">
                <ItemTemplate>
                    <asp:LinkButton ID="linkViewConfirm" runat="server" CommandName="linkInvoice" CommandArgument='<%#Eval("InvoiceId") %>' 
                        ToolTip="View Confirmation Page" />&nbsp;
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <PagerSettings Position="Top" />
        <PagerStyle Font-Bold="True" Font-Size="Small" HorizontalAlign="Left" />
        <SelectedRowStyle CssClass="gridselectedrow" Font-Bold="true" />
        <RowStyle />
        <HeaderStyle CssClass="crewheader" />
    </asp:GridView>
 </div>

<asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetCustomerSalesHistory"
    TypeName="Wcss.CustomerInvoiceRow" SelectCountMethod="GetCustomerSalesHistoryCount" EnablePaging="True">
    <SelectParameters>
        <asp:ProfileParameter Name="userName" PropertyName="UserName" Type="String" />
    </SelectParameters>
</asp:ObjectDataSource>