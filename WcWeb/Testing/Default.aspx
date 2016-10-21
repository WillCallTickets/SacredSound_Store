<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Testing_Default" %>


<%@ Register src="~/Components/Util/CheckSlider.ascx" tagname="CheckSlider" tagprefix="uc1" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="range.css"/>

    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.4.2/jquery.min.js"></script>
    <script type="text/javascript" src="/JQueryUI/jquery.tools-110511.all.min.js"></script>

</head>
<body>
    <form id="form1" runat="server">
        <div>

            <uc1:CheckSlider ID="CheckSlider1" runat="server" TitleText="Wow that's good" MinQty="0" MaxQty="10" />
            <uc1:CheckSlider ID="CheckSlider2" runat="server" TitleText="better" MinQty="1" MaxQty="25" />
            <uc1:CheckSlider ID="CheckSlider3" runat="server" TitleText="best" MinQty="100" MaxQty="1000" />

        </div>

    </form>
</body>
</html>
