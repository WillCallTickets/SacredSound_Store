<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ProcessingShipping.aspx.cs" Inherits="Store_ProcessingShipping" Title="Order Processing" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Processing Your Shipping Options</title>
    <meta http-equiv="refresh" content="1; URL=/Store/Shipping.aspx" />
</head>
<body id="processing">
    <div class="legend">Shipment Processing<br /><img alt="" src="../Images/ajax-loader.gif" /></div>
    <%if (Ctx.Cart.IsShipMultiple_Merch)
        {%><h1>Separating Shipments...</h1>
    <%}
        else
        { %><h1>Combining Shipments...</h1>
    <%} %>      
    <h1>This process should not take longer than 60 secs</h1>
</body>
</html>
