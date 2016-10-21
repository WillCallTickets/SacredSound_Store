<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AgeVerification18.aspx.cs" Inherits="Store_AgeVerification18" Title="Age Verification" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd"> 
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="head1" runat="server">    
    <meta http-equiv="Content-Type" content="text/html; charset=windows-1252" />
    <title></title>
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.6.1/jquery.min.js"></script>
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jqueryui/1.8.14/jquery-ui.min.js"></script>
    <link href="/Styles/overlayage.css" type="text/css" rel="StyleSheet" />
</head>
<body style="font: 70% Arial, Helvetica, Geneva, sans-serif;width:100%;overflow:hidden;" class="verification age18-body">
<form id="Main" runat="server">
    
    <div id="verify18_container">
        <h2>Age Verification</h2>
        <div id="message"></div>
        <h4>You must be 18 years of age or older to view this page.</h4>  
        <h4>Please enter your date of birth:</h4>  
        <div>
            <asp:DropDownList ID="ddlMonth" runat="server" OnDataBinding="ddl_DataBinding" AppendDataBoundItems="true" EnableViewState="true">
                <asp:ListItem Selected="True" Text="Month" Value="0"></asp:ListItem>
            </asp:DropDownList>
            <asp:DropDownList ID="ddlDay" runat="server" OnDataBinding="ddl_DataBinding" AppendDataBoundItems="true" EnableViewState="true">
                <asp:ListItem Selected="True" Text="Day" Value="0"></asp:ListItem>
            </asp:DropDownList>            
            <asp:DropDownList ID="ddlYear" runat="server" OnDataBinding="ddl_DataBinding" AppendDataBoundItems="true" EnableViewState="true">
                <asp:ListItem Selected="True" Text="Year" Value="0"></asp:ListItem>
            </asp:DropDownList>
            <a href="#" id="btnSubmit" class="btnmed submission">Submit</a>
        </div>
    </div>
    <asp:HiddenField ID="hidUserName" runat="server" />
    <asp:HiddenField ID="hidProfileDob" runat="server" />
    <script type="text/javascript" src="/JQueryUI/age18verify.js"></script>

</form>
</body>
</html>

