<%@ Page Language="C#" Title="Admin - Display Show Panels" MasterPageFile="~/TemplateBlank.master" AutoEventWireup="true" CodeFile="Display_Merch.aspx.cs" Inherits="WillCallWeb.Admin.Display_Merch" %>
<asp:Content ID="BlankContent" runat="server" ContentPlaceHolderID="BlankContent">
    <div id="showdisplaypage">
        <div class="displayment">
            <asp:LinkButton ID="LinkButton1" runat="server" Text="refresh" onclick="LinkButton1_Click"></asp:LinkButton>
        </div>
        <div id="choosemerch">
            <asp:Panel ID="panelDisplay" runat="server" />
        </div>
    </div>
</asp:Content>

                    
                    




                    
                    

