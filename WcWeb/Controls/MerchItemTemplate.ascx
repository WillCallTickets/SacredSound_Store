<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MerchItemTemplate.ascx.cs" Inherits="WillCallWeb.Controls.MerchItemTemplate" %>
<div id="listing" class="category">
    <div class="legend"><%=SectionTitle %></div>
    <asp:Literal ID="litNoItems" runat="server" EnableViewState="false" />
    <asp:Repeater ID="rptItems" runat="server" OnItemDataBound="rptItems_ItemDataBound" OnDataBinding="rptItems_DataBinding" EnableViewState="false" >
        <ItemTemplate>
            <div class="merch-container <%#Eval("SeoName") %>">
                <span class="merchheader">
                    <span class="name">
                        <a href="/Store/ChooseMerch.aspx?mite=<%# Eval("Id") %>" onmouseover="window.status='Buy Merchandise'; return true" 
                            onmouseout="window.status=' '; return true" title="Buy merchandise">
                            <%# Eval("Name" )%>
                        </a>
                    </span>
                    <asp:Literal ID="LiteralHiRes" runat="server" EnableViewState="false" />
                    <asp:Literal ID="litSaleItem" runat="server" EnableViewState="false" />
                    <span class="shorttext"><%# Eval("ShortText") %></span>
                </span>
                <div class="picsection"><asp:Literal ID="LiteralPicture" runat="server" EnableViewState="false" /></div>    
            </div>
            <asp:Literal ID="LiteralSeparator" runat="server" EnableViewState="false" />
        </ItemTemplate>
    </asp:Repeater>
</div>