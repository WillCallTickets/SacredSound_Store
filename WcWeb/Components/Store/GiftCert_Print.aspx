<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GiftCert_Print.aspx.cs" MasterPageFile="~/TemplatePrint.master" Inherits="WillCallWeb.Components.Store.GiftCert_Print" Title="Print Gift Certificate" %>

<asp:Content ID="MainContent" runat="server" ContentPlaceHolderID="MainContent">
<div class="print">
    <div class="giftcert">
       <table border="0" cellpadding="0" cellspacing="6" class="gift">
            <tr><td rowspan="99">&nbsp;&nbsp;</td><td colspan="2">&nbsp;</td></tr>
            <tr>
                <td><img src="/WillCallResources/Images/UI/<%=Wcss._Config._GiftLogo%>" /></td>
                <td><h1>Gift Certificate</h1></td>
            </tr>
            <tr><td colspan="2">&nbsp;</td></tr>
            <tr>
                <th>Amount</th>
                <td><%=Amount %></td>
            </tr>
            <tr>
                <th>To</th>
                <td><%=To %></td>
            </tr>
            <tr>
                <th>From</th>
                <td><%=From %></td>
            </tr>
            <tr>
                <th>Code</th>
                <td><%=GiftCode %></td>
            </tr>
        </table>
    
        <br />
        <hr />
        <div class="instructions"><%=Wcss._Config._GiftRedemptionInstructions%></div>
        <div class="instructions"><%=Wcss._Config._GiftTerms%></div>
        <br /><br />
    </div>
</div>

<script type="text/javascript">
   <!--
    window.print();
   //-->
</script>
   
</asp:Content>

