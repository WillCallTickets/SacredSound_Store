<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CustomerPicker.ascx.cs" Inherits="WillCallWeb.Admin.AdminControls.CustomerPicker" %>
<div id="customerpicker">
    <div class="jqhead rounded" >
        <div class="cmdsection"><span class="sectitle">Customer Picker</span>
            <asp:Button ID="btnSearch" CssClass="btnmed" CommandName="Search" runat="server" Text="Search"
            OnClick="btnSearch_Click" CausesValidation="false" />                                
        <%if(Page.User.IsInRole("Super") || Page.User.IsInRole("Administrator")){ %>
        <asp:Button ID="btnAdmins" CssClass="btnmed" runat="server" Text="Admin List" OnClick="btnAdmins_Click" CausesValidation="false" />
        <%} %>
        </div>
        <br />
        <div class="jqpnl rounded">
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="edittabl">
            <tr>
                <th>Email</th>
                <td style="width:100%;"><asp:TextBox ID="txtEmail" TabIndex="1" MaxLength="256" Width="300px" runat="server"></asp:TextBox> *partial matches ok</td>
            </tr>
            <tr>
                <th>Last Name</th>
                <td><asp:TextBox ID="txtLastName" TabIndex="2" MaxLength="256" Width="300px" runat="server"></asp:TextBox> *exact matches only</td>
            </tr>
            <tr>
                <th>Invoice Id</th>
                <td><asp:TextBox ID="txtInvoice" TabIndex="3" MaxLength="256" Width="300px" runat="server"></asp:TextBox> *partial matches ok</td>
            </tr>
            <tr>
                <th>Last Four of CC</th>
                <td><asp:TextBox ID="txtLastFour" TabIndex="3" MaxLength="6" Width="300px" runat="server"></asp:TextBox> *exact matches only</td>
            </tr>
            <tr>
                <th>Customer Id</th>
                <td><asp:TextBox ID="txtCustId" TabIndex="4" MaxLength="256" Width="300px" runat="server"></asp:TextBox> *partial matches ok</td>
            </tr>
            <tr>
                <th>Birthday Month</th>
                <td>
                    <asp:DropDownList ID="ddlBdMonth" TabIndex="5" Width="300px" runat="server">
                        <asp:ListItem Selected="True" Text="-- Please Select a Month --" Value="0"></asp:ListItem>
                        <asp:ListItem Text="January" Value="1" />
                        <asp:ListItem Text="February" Value="2" />
                        <asp:ListItem Text="March" Value="3" />
                        <asp:ListItem Text="April" Value="4" />
                        <asp:ListItem Text="May" Value="5" />
                        <asp:ListItem Text="June" Value="6" />
                        <asp:ListItem Text="July" Value="7" />
                        <asp:ListItem Text="August" Value="8" />
                        <asp:ListItem Text="September" Value="9" />
                        <asp:ListItem Text="October" Value="10" />
                        <asp:ListItem Text="November" Value="11" />
                        <asp:ListItem Text="December" Value="12" />
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
        </div>
        <br />
        <div style="padding:14px;width:750px;background-color:#fff;border:solid #333 1px;font-size:12px;">
            <asp:Label ID="lblCriteria" CssClass="criteria" runat="server"></asp:Label>
            <asp:Repeater ID="rptResults" runat="server" OnItemDataBound="rptResults_ItemDataBound" >
                <ItemTemplate>
                    <asp:Literal ID="litItem" runat="server" />
                    <br />
                </ItemTemplate>
            </asp:Repeater>
        </div>
        <br />
    </div>
</div>
