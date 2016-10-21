<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Cust_StoreCredit.ascx.cs" Inherits="WillCallWeb.Components.Customer.Cust_StoreCredit" %>
<%@ Register src="~/Components/Cart/Cart_Function.ascx" tagname="Cart_Function" tagprefix="uc1" %>
<div class="giftredemption">
    <h4>Your current store credit balance is: <%=this.Profile.StoreCredit.ToString("c") %></h4>
    <div class="section">
       <div class="redeem">
            <h4>Redeem a gift certificate or store credit</h4><br />
            <table cellpadding="0" cellspacing="3" class="custtable">
                <tr>
                    <th>Enter code:</th>
                    <td><asp:TextBox ID="txtCode" runat="server" MaxLength="50" Width="300px" /></td>
                </tr>
                <tr><td>&nbsp;</td><td>ex: DCCDFEC9-A73D-4F39-BFA5-452D2B9CB7D9</td></tr>
                <tr><td colspan="2" class="spacer">
                    <asp:Label ID="lblStatus" runat="server" />&nbsp;</td>
                </tr>
                <tr>
                    <td><asp:LinkButton ID="btnSubmit" runat="server" class="btntribe" Text="Redeem Code" OnClick="btnSubmit_Click"><span>Redeem Code</span></asp:LinkButton></td>
                    <td><uc1:Cart_Function ID="Cart_Function1" runat="server" /></td>
                </tr>
            </table>
        </div>
   </div>
   <asp:Panel ID="pnlRedemptions" runat="server" CssClass="section">
       <h4>Store credit history</h4>
       <asp:GridView ID="GridCredits" runat="server" Width="100%" AllowPaging="True" CssClass="crewtable" AutoGenerateColumns="False"
           EmptyDataText="No credits" OnDataBinding="GridCredits_DataBinding" OnRowDataBound="GridCredits_RowDataBound" CellSpacing="0" CellPadding="0"
           BorderStyle="None" BorderWidth="0px" Font-Size="Smaller" PageSize="25" DataSourceID="SqlCredits">
           <RowStyle BackColor="#ffffff" VerticalAlign="Middle" />
           <AlternatingRowStyle BackColor="#f1f1f1" VerticalAlign="Middle" />
           <PagerSettings Position="Top" /> 
           <PagerStyle BackColor="#cccccc" CssClass="pager" />
           <Columns>
               <asp:TemplateField HeaderText="&nbsp; #" ItemStyle-HorizontalAlign="center" ItemStyle-CssClass="rowcounter" HeaderStyle-HorizontalAlign="Center">
                   <ItemTemplate>
                       <asp:Literal ID="LiteralRowCounter" runat="server" /></ItemTemplate>
               </asp:TemplateField>
               <asp:BoundField DataField="dtStamp" HeaderText="Date" DataFormatString="{0:MM/dd/yyyy hh:mmtt}" ItemStyle-Wrap="false" />
               <asp:BoundField DataField="mAmount" HeaderText="Amount" DataFormatString="{0:n2}" ItemStyle-Wrap="false" />
               <asp:TemplateField HeaderText="Comments">
                   <ItemTemplate>
                       <asp:Literal ID="litComment" runat="server" />
                   </ItemTemplate>
               </asp:TemplateField>
           </Columns>
       </asp:GridView>
   </asp:Panel>
</div>
<asp:SqlDataSource ID="SqlCredits" runat="server" ConnectionString="<%$ ConnectionStrings:WillCallConnectionString %>"
   SelectCommand="SELECT cred.* FROM StoreCredit cred INNER JOIN aspnet_Users u ON u.UserName = @userName AND cred.UserId = u.UserId ORDER BY cred.dtStamp DESC"  
   onselecting="Sql_Selecting" >
<SelectParameters>
    <asp:Parameter Name="appId" DbType="Guid" />
    <asp:ProfileParameter Name="userName" PropertyName="UserName" Type="String" />
   </SelectParameters>
</asp:SqlDataSource>     



