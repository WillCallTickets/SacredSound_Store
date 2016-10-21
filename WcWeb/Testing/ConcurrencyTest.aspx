<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ConcurrencyTest.aspx.cs" Inherits="WillCallWeb.Testing_ConcurrencyTest" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <h1>ConcurrencyTesting</h1>
        <br />
        <hr />
        <asp:Button ID="btnPublish" runat="server" Text="Publish" 
            onclick="btnPublish_Click" />
        <br />
        <hr />
        <div style="background-color:White;padding:8px;">
            <asp:Label ID="lblConditions" runat="server" />
        </div>
        
    </div>
    </form>
</body>
</html>
