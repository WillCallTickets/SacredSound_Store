<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StaticMethods.aspx.cs" MasterPageFile="~/TemplateAdmin.master" 
Inherits="WillCallWeb.Admin.StaticMethods" Title="Admin - Static Methods" %>


<%@ Register src="../Components/Util/MerchSelector.ascx" tagname="MerchSelector" tagprefix="uc1" %>


<asp:Content ID="MainContent" runat="server" ContentPlaceHolderID="MainContent">
   
   

   <asp:HiddenField id="hidEarly" runat="server" value="Select top 1 * from invoiceitem where tshowticketid = 10104 order by dtstamp " />
   <asp:HiddenField id="hidEarlyDescriptor" runat="server" value="Masq_080802_EarlyShow" />
   <asp:HiddenField id="hidLate" runat="server" value="Select top 1 * from invoiceitem where tshowticketid = 10119 and purchaseaction = 'purchased' and shippingmethod = 'will call' order by dtstamp " />
    <asp:HiddenField id="hidLateDescriptor" runat="server" value="Masq_080802_LateShow" />
   

   <asp:HiddenField id="hidWinterTopNumberForQuery" runat="server" value="1" />
   <asp:HiddenField id="hidWinterMaxInvoiceId" runat="server" value="86334" />
   <asp:HiddenField id="hidWinterTixDateStart" runat="server" value="1/15/2014" />
   <asp:HiddenField id="hidWinterTixDateEnd" runat="server" value="6/1/2014" />
    <asp:HiddenField id="hidWinterDescriptor" runat="server" value="WinterTour_2014" />
   <asp:HiddenField id="hidWinterLink" runat="server" value="For questions regarding refunds please visit <br\/> <a href=&#34;http://sts9.com/2014/01/sts9-winter-tour-refund-information/&#34; >http://sts9.com/2014/01/sts9-winter-tour-refund-information/<\/a> <br\/> for more information " />
     
    <asp:HiddenField id="hidRefundServiceFees" runat="server" value="true" />
    
    <asp:Button ID="btnTest" Enabled="true" Height="24px" runat="server" visible="false"
                    Text="Test" OnClick="btnTest_Click" />
    

    <asp:UpdatePanel ID="updStatic" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table border="1" cellspacing="3" cellpadding="3">
                <tr>
                    <th>Test Merch Selector</th>
                    <td>
            
                        <uc1:MerchSelector ID="MerchSelector1" runat="server" ShowInventory="false" />
            
                    </td>      
                    <td>
                        <asp:LinkButton ID="btnMerchSelect" Enabled="true" Height="24px" runat="server" Text="Get Selection" CssClass="btnadmin"
                            OnClick="btnMerchSelect_Click" />
                        <asp:Label ID="lblSelect" runat="server" />
                    </td>  
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>

    <table border="1" cellspacing="3" cellpadding="3">
        <tr>
            <th>
                Send test email
            </th>
            <td>
                <asp:Button ID="Button3" Enabled="true" Height="24px" runat="server" Text="Send Test Mail"
                    OnClick="btnSendTestMail_Click" />
            </td>
            <td>
                <asp:Literal ID="Literal4" runat="server" />
            </td>
        </tr>
        <tr>
            <th>
                Init any new config values
            </th>
            <td>
                <asp:Button ID="btnConfig" Enabled="true" Height="24px" runat="server" Text="Init Config"
                    OnClick="btnConfig_Click" OnClientClick="return confirm('Are you sure?');" />
            </td>
            <td>
                <asp:Literal ID="Literal2" runat="server" />
            </td>
        </tr>
         <tr>
            <th>
                Process the invoice id for bundles
            </th>
            <td>
                <asp:Button ID="Button2" Enabled="true" Height="24px" runat="server" Text="Process the invoice id for bundles"
                    OnClick="btnBundler_Click" OnClientClick="return confirm('Are you sure?');" />
                <asp:TextBox ID="txtInvoiceId" runat="server" Width="100px"></asp:TextBox>
            </td>
            <td>
                <asp:Literal ID="Literal3" runat="server" />
            </td>
        </tr>
        <tr>
            <th>
                Record the images to the past shows
            </th>
            <td>
                <asp:Button ID="btnPast" Enabled="False" Height="24px" runat="server" Text="Record the images to the past shows"
                    OnClick="btnPast_Click" OnClientClick="return confirm('Are you sure?');" />
            </td>
            <td>
                <asp:Literal ID="Literal1" runat="server" />
            </td>
        </tr>
        <tr>
            <th>
                Reset all Passwords, hint questions and hint answers
            </th>
            <td>
                <asp:Button ID="btnPwds" Enabled="False" Height="24px" runat="server" Text="Reset all Passwords, hint questions and hint answers"
                    OnClick="btnPwds_Click" OnClientClick="return confirm('Are you sure?');" />
            </td>
            <td>
                <asp:Literal ID="litPwds" runat="server" />
            </td>
        </tr>
        <tr>
            <th>Clean Up an act</th>
            <td>
                <asp:TextBox ID="txtAct" runat="server" />
                <asp:Button ID="btnCleanAct" Enabled="false" Height="24px" runat="server" 
                    Text="Clean Act Image" OnClick="btnCleanAct_Click" 
                    OnClientClick="return confirm('Are you sure?');" />
            </td>
            <td><asp:Literal ID="litCleanAct" runat="server" /></td>
        </tr>
        <tr>
            <th>Clean Tune Urls</th>
            <td>
                <asp:Button ID="btnCleanTune" Enabled="false" Height="24px" runat="server" Text="Clean Tune Urls" OnClick="btnCleanTune_Click" OnClientClick="return confirm('Are you sure?');" />
            </td>
            <td><asp:Literal ID="cleantune" runat="server" /></td>
        </tr>
        <tr>
            <th>Check Pass</th>
            <td>
                <asp:Button ID="Button1" Enabled="false" Height="24px" runat="server" Text="Check Pass" OnClick="btnPass_Click" OnClientClick="return confirm('Are you sure?');" />
            </td>
            <td><asp:Literal ID="pass" runat="server" /></td>
        </tr>
        <tr>
            <th>Set Invoice Products</th>
            <td>
                <asp:Button ID="btnProduct" Enabled="false" Height="24px" runat="server" Text="Set vcProducts" OnClick="btnProduct_Click" OnClientClick="return confirm('Are you sure?');" />
            </td>
            <td>drop the reproduct table afterwards</td>
        </tr>
        <tr>
            <th>Rename Shows</th>
            <td>
                <asp:Button ID="btnShowName" Enabled="false" Height="24px" runat="server" Text="Redo Show Name" OnClick="btnRename_Click" OnClientClick="return confirm('Are you sure?');" />
            </td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <th>Re-encrypt Cashew</th>
            <td>
                <asp:Button ID="btnRecrypt" Enabled="false" Height="24px" runat="server" Text="Re-Encrypt" OnClick="btnRecrypt_Click" OnClientClick="return confirm('Are you sure?');" />
            </td>
            <td>Be sure to back up original cashew table in database.<br />drop the recash table afterwards</td>
        </tr>
        <tr>
            <th>Merch Shipping</th>
            <td>
                <asp:Button ID="btnMerchShipping" Enabled="false" Height="24px" runat="server" Text="Merch Ship" OnClick="btnMerchShipping_Click" OnClientClick="return confirm('Are you sure?');" />
            </td>
            <td>Be sure to back up original in database.<br /></td>
        </tr>
        <tr>
            <th>Masquerade Early Show</th>
            <td>
                <asp:Button ID="btnMasqEarly" Enabled="false" Height="24px" runat="server" Text="Masq Early Show" OnClick="btnMaskEarly_Click" OnClientClick="return confirm('Are you sure?');" />
            </td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <th>Masquerade Late Show</th>
            <td>
                <asp:Button ID="btnMaskLate" Enabled="false" Height="24px" runat="server" Text="Masq Late Show" OnClick="btnMaskLate_Click" OnClientClick="return confirm('Are you sure?');" />
            </td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <th>Winter Tour</th>
            <td>
                <asp:Button ID="btnWinter" Enabled="true" Height="24px" runat="server" 
                    Text="Winter Tour Refunds" OnClick="btnWinter_Click" 
                    OnClientClick="return confirm('Are you sure?');" />
            </td>
            <td>&nbsp;</td>
        </tr>
    </table>
     <asp:GridView ID="GridView1" runat="server" Width="100%" AutoGenerateColumns="false" Visible="true" 
        DataKeyNames="ItemIdentifier, ItemId, Quantity, BasePrice, Service, LineTotal, Context, Description, SalePromotionId, IsPackageTicket"
        OnRowDataBound="GridView1_RowDataBound" CellPadding="4" >
        <HeaderStyle CssClass="selectedalt" />
        <EmptyDataTemplate>
            There are no items available for refund.
        </EmptyDataTemplate>
        <Columns>
            <asp:TemplateField HeaderText="Select">
                <ItemTemplate>
                    <asp:CheckBox ID="chkSelect" runat="server" />
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:BoundField DataField="BasePrice" HeaderText="Base" HtmlEncode="False" DataFormatString="{0:n}" >
                <ItemStyle HorizontalAlign="Center" />
            </asp:BoundField>
            <asp:TemplateField HeaderText="Service">
                <ItemTemplate>
                    <asp:CheckBox ID="chkService" runat="server" />
                    <asp:Literal ID="litService" runat="server" />
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" Wrap="False" />
            </asp:TemplateField>
            <asp:BoundField DataField="Each" HeaderText="Each" HtmlEncode="False" DataFormatString="{0:n}" >
                <ItemStyle HorizontalAlign="Center" />
            </asp:BoundField>
            <asp:TemplateField HeaderText="Quantity">
                <ItemTemplate>
                    <asp:DropDownList ID="ddlQty" width="60px" runat="server" />
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:BoundField DataField="LineTotal" HeaderText="LineTotal" HtmlEncode="False" DataFormatString="{0:c}" >
                <ItemStyle HorizontalAlign="Center" />
            </asp:BoundField>
            <asp:BoundField DataField="Context" HeaderText="Context" >
                <ItemStyle HorizontalAlign="Center" />
            </asp:BoundField>
            <asp:TemplateField HeaderText="Description" HeaderStyle-HorizontalAlign="left" ItemStyle-Width="65%">
                <ItemTemplate>
                    <asp:Literal ID="litDescription" runat="server" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>    
</asp:Content>