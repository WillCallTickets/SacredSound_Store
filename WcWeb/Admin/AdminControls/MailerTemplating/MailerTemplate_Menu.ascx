<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MailerTemplate_Menu.ascx.cs" EnableViewState="false"
    Inherits="WillCallWeb.Admin.AdminControls.MailerTemplating.MailerTemplate_Menu" %>
<div id="mailertemplate">    
    <div class="jqhead rounded">
        <div class="cmdsection">
            <asp:Button ID="btnSelect" runat="server" CommandName="select" OnClick="btnNav_Click" CausesValidation="false" CssClass="btnmed" Width="80px"
                Text="Select" />
            <asp:Button ID="btnContent" runat="server" CommandName="content" OnClick="btnNav_Click" CausesValidation="false" CssClass="btnmed" Width="80px"
                Text="Content" />
            <asp:Button ID="btnGenerate" runat="server" CommandName="generate" OnClick="btnNav_Click" CausesValidation="false" CssClass="btnmed" Width="80px"
                Text="Generate" />
            <asp:Button ID="btnUpload" runat="server" CommandName="upload" OnClick="btnNav_Click" CausesValidation="false" CssClass="btnmed" Width="80px"
                Text="Uploader" />
            <%if (this.Page.User.IsInRole("Super")) { %>
                <asp:Button ID="btnTptEdit" runat="server" CommandName="tptedit" OnClick="btnNav_Click" CausesValidation="false" CssClass="btnmed" Width="80px"
                    Text="Template" />
                <asp:Button ID="btnTptContainer" runat="server" CommandName="tptcontainer" OnClick="btnNav_Click" CausesValidation="false" CssClass="btnmed" Width="80px"
                    Text="Containers" />
                <asp:Button ID="btnTptSubstitution" runat="server" CommandName="tptsubstitution" OnClick="btnNav_Click" CausesValidation="false" CssClass="btnmed" Width="80px"
                    Text="Substitutions" />
            <%} %>
        </div>
        <div class="sectitle"><%=Title %></div>
    </div>
</div>