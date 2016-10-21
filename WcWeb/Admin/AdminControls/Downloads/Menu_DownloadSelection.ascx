<%@ Control Language="C#" AutoEventWireup="true" EnableViewState="false" CodeFile="Menu_DownloadSelection.ascx.cs" 
    Inherits="WillCallWeb.Admin.AdminControls.Downloads.Menu_DownloadSelection" %>
<div class="jqhead rounded">
    <table border="0" cellpadding="0" cellspacing="0" width="100%" class="hedtbl">
        <tr>
            <td style="width:100%;white-space:nowrap;">
                tree view file dir
            </td>
        </tr>
        <asp:TreeView ID="tvDirectory" runat="server" Enabled="True" ExpandImageUrl="/Images/close.gif"

CollapseImageUrl="/Images/folder.gif" OnSelectedNodeChanged="tvDirectory_SelectedNodeChanged"
>
        </asp:TreeView>
    </table>
</div>