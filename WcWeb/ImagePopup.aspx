<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ImagePopup.aspx.cs" Inherits="ImagePopup" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title><%=_item.DisplayName %></title>
</head>
<body id="image-popup-page">
    <form id="Main" runat="server">
        <div id="detailedimage">
            <div class="legend"><%=_item.DisplayName %></div>
            <asp:FormView ID="FormView1" runat="server" AllowPaging="True" OnDataBinding="FormView1_DataBinding" 
                OnDataBound="FormView1_DataBound" OnPageIndexChanged="FormView1_PageIndexChanged" 
                OnPageIndexChanging="FormView1_PageIndexChanging">
                <PagerSettings Position="TopAndBottom" Mode="NumericFirstLast" />
                <ItemTemplate>
                    <asp:Literal ID="literalImage" runat="server" />
                </ItemTemplate>
                <PagerStyle HorizontalAlign="Left" Font-Bold="True" Font-Size="X-Large" CssClass="pagelink" />
            </asp:FormView>
        </div>
    </form>
</body>
</html>
