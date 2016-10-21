<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Updater.aspx.cs" Inherits="Testing_Mine_Updater" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" EnablePartialRendering="true" runat="server">
        </asp:ScriptManager>
    
    </div>
    <br />
        <div>
            <asp:Label ID="Label1" runat="server" Text="Label" Width="249px"></asp:Label><br />
            <br />
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    &nbsp;
            <hr />        
            <asp:Label ID="Label2" runat="server" Text="Label" Width="249px"></asp:Label>
            <hr />
            </ContentTemplate>
            <Triggers>
            <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
            </Triggers>
            </asp:UpdatePanel>
            <br />
            <asp:Label ID="Label3" runat="server" Text="Label" Width="249px"></asp:Label><br />
            <br />
            &nbsp;<asp:Button ID="Button1" runat="server" Text="Button" Width="233px" OnClick="Button1_Click" /></div>
    </form>

</body>
</html>
