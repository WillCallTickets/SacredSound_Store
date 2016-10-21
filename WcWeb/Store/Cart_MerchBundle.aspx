<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Cart_MerchBundle.aspx.cs" Inherits="Store_Cart_MerchBundle" Title="Merch Bundle Editor" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd"> 
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="head1" runat="server">    
    <meta http-equiv="Content-Type" content="text/html; charset=windows-1252" />
    <title></title>
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.6.1/jquery.min.js"></script>
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jqueryui/1.8.14/jquery-ui.min.js"></script>
    <link href="/Styles/overlaybundlegc.css" type="text/css" rel="StyleSheet" />
</head>
<body style="font: 70% Arial, Helvetica, Geneva, sans-serif;color: #000000;background-color:transparent;" class="bun-body">
<form id="Main" runat="server">
    
    <div id="bun-container">        
        <a class="bnd-back" href="/store/cart_edit.aspx">Back To Cart</a>
        <h3><%= MerchBundleRecord.Title %></h3>
        <div class="priceline"><asp:Literal ID="litPrice" runat="server" EnableViewState="false" OnDataBinding="litPrice_DataBinding" /></div>
        
        <div id="bun-selector">
            <div class="bun-header">
                <div class="bnr">Select from the following items</div>
            <%if (MerchBundleRecord.Comment.Trim().Length > 0)
              {%>
                <div class="bun-comment"><%= MerchBundleRecord.Comment %></div>
            <%} %>
            </div>
            <div class="ul-container">
                <asp:Literal ID="litChoices" runat="server" EnableViewState="false" />
            </div>
            <a class="bnd-back" href="/store/cart_edit.aspx">Back To Cart</a>
        </div>
        
        <div id="bun-collector">
            <div class="bun-header">
                <div class="bnr">Drag selections to the area below</div>
                <div class="title-select"><asp:Literal ID="litSelected" runat="server" EnableViewState="false" /></div>
            </div>
            <div class="ul-container" id="lstCollector">
                <asp:Literal ID="litSelections" runat="server" EnableViewState="false" OnDataBinding="litSelections_DataBinding" />                
            <div class="clearfix"></div>    
            </div>
            

            <div class="message"></div>
            <a href="#" id="btnclear">Clear All</a>
            
        </div>

    </div>

    <input type="hidden" id="hidContext" value='<%=this.SaleItemContext %>' />
    <input type="hidden" id="hidSaleItem_ItemId" value='<%=this.SaleItem_ItemId.ToString() %>' />
    <input type="hidden" id="hidBundleId" value='<%=this.MerchBundleRecord.Id.ToString() %>' />   
    <script type="text/javascript" src="/JQueryUI/bundleeditor.js"></script>

</form>
</body>
</html>

