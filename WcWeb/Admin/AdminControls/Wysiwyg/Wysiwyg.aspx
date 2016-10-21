<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Wysiwyg.aspx.cs" Inherits="WillCallWeb.Admin.AdminControls.Wysiwyg.Wysiwyg" Title="Description Editor" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd"> 
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="head1" runat="server">    
    <meta http-equiv="Content-Type" content="text/html; charset=windows-1252" />
    <title></title>
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.8.2/jquery.min.js"></script>
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jqueryui/1.8.14/jquery-ui.min.js"></script>
    <link href="/Styles/admin.css" type="text/css" rel="StyleSheet" />
</head>
<body style="font: 70% Arial, Helvetica, Geneva, sans-serif;color: #ffffff;background-color:transparent;border:0;" class="overlay-body">
<form id="Main" runat="server">
    
    <div id="overlay-container">

        <div id="wysiwyg-container">
            <div id="wysiwyg-header">
                <h3><asp:Literal ID="litTitle" runat="server" OnDataBinding="litTitle_DataBinding" /></h3>
                <span class="wsyiwyg-command"><asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btnmed" CommandName="save" OnClick="btn_Click" />
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btnmed" CommandName="cancel" OnClick="btn_Click" />
                    </span>
            </div>
            <div id="wysiwyg-editor">
                <CKeditor:CKEditorControl ID="Ck_Edit" Toolbar="WctFull"  OnDataBinding="CKEditor_DataBinding"
                    runat="server" Width="98%" Height="400px" ></CKeditor:CKEditorControl>
            </div>
        </div>

    </div>
    <script type="text/javascript" src="/JQueryUI/wysiwyg-admin.js"></script>

</form>
</body>
</html>

