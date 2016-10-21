<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TopLevelPage.aspx.cs" Inherits="Testing_Mine_TopLevelPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    
    <form id="form1" runat="server"> 
    <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
    <div>
    
    
    <div id="choosemerch">  
        <fieldset id="merch" runat="server">
            <legend>No Update</legend>
            <div>
                Non-Update: <%= DateTime.Now.ToLongTimeString() %>
            </div>
        </fieldset>    
    </div>
    

    <asp:UpdatePanel ID="UpdatePanelSelection" runat="server">
        <ContentTemplate>
            <fieldset id="update1">
                <legend>Update One - Always</legend>
                <div>
                    <div>
                        Last Update: <%= DateTime.Now.ToLongTimeString() %>
                    </div>
                    <span id="currentSelection">current</span>
                    <span id="addToCartOption" style="display:none;">
                        <a href="javascript:addSelectionToCart();">Add To Cart</a>
                    </span>
                </div>
                <div>
                    <asp:Button ID="Button1" runat="server" Text="Button" />
                </div>
            </fieldset>
            
        </ContentTemplate>
    </asp:UpdatePanel>
    
    <asp:UpdatePanel ID="UpdateMerchCart" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <fieldset id="update2">
                <legend>Update Two - Conditional</legend>
                <div>
                    <div>
                        Last Update: <%= DateTime.Now.ToLongTimeString() %>
                    </div>
                    <span id="Span1">current</span>
                    <span id="Span2" style="display:none;">
                        <a href="javascript:addSelectionToCart();">Add To Cart</a>
                    </span>
                </div>
                <div>
                    <asp:Button ID="Button2" runat="server" Text="Button" />
                </div>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>

<script type="text/javascript">
<!--
    
    function pageLoad() {
    }
    
//-->
</script>
    
    
    
    
    
    </div>
    </form>
</body>
</html>
