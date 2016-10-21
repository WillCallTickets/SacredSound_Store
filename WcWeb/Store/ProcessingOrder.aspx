<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ProcessingOrder.aspx.cs" Inherits="Store_ProcessingOrder" Title="Order Processing" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Processing Your Order</title>    
    <meta http-equiv="refresh" content="1; URL=/Store/Confirmation.aspx" />
</head>
<body id="processing">
    
    <%if (!Ctx.Cart.HasItems || Ctx.OrderProcessingVariables == null) { Response.Redirect("Default.aspx"); }%>
    
    <div class="legend">Order Processing<br /><img alt="" src="../Images/ajax-loader.gif" /></div>
    <h1>Processing Your Order...</h1>
    <h1>Do Not Press Back or Refresh!</h1>
    <h1>This process should not take longer than 60 secs</h1>
</body>
</html>
