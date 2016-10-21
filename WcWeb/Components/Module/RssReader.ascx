<%@ Control Language="C#" AutoEventWireup="true" CodeFile="RssReader.ascx.cs" Inherits="WillCallWeb.Components.Module.RssReader" %>
<div class="sectiontitle">
    <asp:Literal runat="server" ID="litTitle" />
    <asp:HyperLink ID="lnkRss" runat="server" ToolTip="Get the RSS for this content">
        <asp:Image runat="server" ID="imgRss" alternatetext="Get RSS feed" ImageUrl="~/Images/rss.gif" width="30px" height="14px" />
    </asp:HyperLink>
</div>
<asp:DataList ID="lstRss" runat="server" EnableViewState="false">
    <ItemTemplate><%#Eval("title") %></ItemTemplate>
</asp:DataList>
    
