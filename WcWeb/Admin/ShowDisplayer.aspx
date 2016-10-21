<%@ Page Language="C#" Title="Admin - Display Show Panels" MasterPageFile="~/TemplateBlank.master" AutoEventWireup="true" CodeFile="ShowDisplayer.aspx.cs" Inherits="WillCallWeb.Admin.ShowDisplayer" %>
<asp:Content ID="BlankContent" runat="server" ContentPlaceHolderID="BlankContent">
    <div id="showdisplaypage">
        <div class="displayment">
            <asp:LinkButton ID="LinkButton1" runat="server" Text="refresh" onclick="LinkButton1_Click"></asp:LinkButton>
        </div>
        <asp:Panel ID="panelDisplay" runat="server" />
    </div>
</asp:Content>

                    
                    




                    
                    

