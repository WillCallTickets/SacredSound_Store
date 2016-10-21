<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MailerTemplatingPreview.aspx.cs" 
    Inherits="WillCallWeb.Admin.AdminControls.MailerTemplating.MailerTemplatingPreview" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <table border="0" cellpadding="0" cellspacing="0" width="800px" class="edittabl">
        <tr><th align="left">&nbsp;HTML Preview<br /><br /></th></tr>
        <tr><td><asp:Literal ID="litHtmlVersion" runat="server" /></td>
        <tr><th align="left">&nbsp;Text Preview<br /><br /></th></tr>
        <tr><td><asp:Literal ID="litTextVersion" runat="server" /></td></tr>
    </table>
</body>
</html>
