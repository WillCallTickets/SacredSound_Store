<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Listing_Show.ascx.cs" Inherits="WillCallWeb.Controls.Listing_Show" %>

<div id="chooseshow">
    <div id="month" class="showlisting" runat="server">
        <div class="legend"><%= this._monthName %></div>   
        <div class="showlist-wrapper">
            <asp:Label ID="lblEmptyData" runat="server" />
            <asp:Repeater EnableViewState="false" ID="rptShows" runat="server" OnItemDataBound="rptShows_ItemDataBound" >
                <ItemTemplate>
                    <div class="show <%# Eval("ShowRecord.SeoName" )%>">
                        <table cellpadding="0" cellspacing="0" style="width: 100%;">
                            <tr>
                                <td>
                                    <div class="info">
                                        <asp:Literal ID="LiteralShowTitle" runat="server" />
                                        <span class="datelist"><%# Eval("ShowRecord.ShowDateList" )%></span>
                                        <asp:Literal ID="litVenue" runat="server" />
                                        <span class="eventdes">
                                            <asp:Literal ID="LiteralDescription" runat="server" />
                                        </span>                                        
                                    </div>
                                </td>
                                <td class="link"><asp:Literal ID="LiteralBuyLink" runat="server" /></td>
                            </tr>
                        </table>
                    </div>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <div class="show alternate <%# Eval("ShowRecord.SeoName" )%>"">
                        <table cellpadding="0" cellspacing="0" style="width: 100%;">
                            <tr>
                                <td>
                                    <div class="info">
                                        <asp:Literal ID="LiteralShowTitle" runat="server" />
                                        <span class="datelist"><%# Eval("ShowRecord.ShowDateList" )%></span>
                                        <asp:Literal ID="litVenue" runat="server" />
                                        <span class="eventdes">
                                            <asp:Literal ID="LiteralDescription" runat="server" />
                                        </span>                                        
                                    </div>
                                </td>
                                <td class="link"><asp:Literal ID="LiteralBuyLink" runat="server" /></td>
                            </tr>
                        </table>
                    </div>
                </AlternatingItemTemplate>
            </asp:Repeater>
        </div>  
    </div>
</div>
