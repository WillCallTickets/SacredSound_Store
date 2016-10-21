<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Scaffold_Editor.ascx.cs" Inherits="WillCallWeb.Admin.AdminControls.Scaffold_Editor" %>
<%@ Register Assembly="SubSonic" Namespace="SubSonic" TagPrefix="subsonic" %>

<div id="setting"><asp:ValidationSummary ID="ValidationSummary1" CssClass="validationsummary" HeaderText="" ValidationGroup="editor" runat="server" />
    <div class="jqhead rounded">
        <div class="sectitle">Settings <asp:CustomValidator ID="CustomValidation" runat="server" ValidationGroup="editor" CssClass="invisible" 
            Display="Static" ErrorMessage="CustomValidator">*</asp:CustomValidator>

           

        </div>
    </div>
</div>

