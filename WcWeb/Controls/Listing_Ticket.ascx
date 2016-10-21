<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Listing_Ticket.ascx.cs" Inherits="WillCallWeb.Controls.Listing_Ticket" %>

<%if(_show != null) {%>

<div id="chooseticket">    
    <asp:CustomValidator ID="CustomVal" runat="server" CssClass="invisible" Display="dynamic"></asp:CustomValidator>

    <div id="showset" class="showlisting" runat="server">
        <div class="show <%# Eval("ShowRecord.SeoName" )%>">
            <div class="info">
                <asp:Literal ID="LiteralShowTitle" runat="server" EnableViewState="false" />
                
                <asp:Literal ID="LiteralVenue" runat="server" EnableViewState="false" />
                
                <div class="eventd">                    
                    <asp:Literal ID="LiteralShowStatus" runat="server" EnableViewState="false" />
                    <asp:Literal ID="LiteralDescription" runat="server" EnableViewState="false" />
                    <asp:Literal ID="LiteralShowTimes" runat="server" EnableViewState="false" />
                    <asp:Literal ID="litDisplayNotes" runat="server" EnableViewState="false" />
                    <asp:Literal ID="litSocial_FB" runat="server" EnableViewState="false" OnDataBinding="litSocial_DataBinding" />
                    <asp:Literal ID="LiteralNoneAvailable" runat="server" EnableViewState="false" />
                </div>
            </div>
            <asp:Literal ID="litShowImage" runat="server" />
        </div>
    </div>
        
    <div class="ticketlist">
        <div id="TicketSet" class="ticketlisting" runat="server">
            <div class="title">Tickets available online</div> 
            <asp:Literal ID="litTicketHeader" runat="server" EnableViewState="false" />
            <div class="ticketlist-wrapper">
            <asp:Repeater ID="rptTickets" runat="server" OnItemCommand="rptTickets_ItemCommand" OnItemDataBound="rptTickets_ItemDataBound" EnableViewState="true">
                <ItemTemplate>
                    <div id="divAlternate" runat="server">
                    <table class="saleitem" border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td colspan="2" class="cartaction">
                                <div class="contain">
                                    <asp:Literal ID="LiteralNotAvailable" runat="server" EnableViewState="false" />
                                    <div id="AllowPurchase" runat="server">
                                        <span class="quant">
                                            <asp:DropDownList ID="ddlQty" runat="server" EnableViewState="true"></asp:DropDownList>
                                            <asp:LinkButton ID="btnAdd" CssClass="btntribe" CommandName="updtkt" runat="server" ToolTip="Update Quantity" CausesValidation="False"
                                                CommandArgument='<%# Eval("Id") %>'><span>Add To Cart</span></asp:LinkButton>
                                        </span>
                                    </div>
                                    <div class="pricedescription">
                                            <div>
                                                <span class="label">price</span>
                                                <span class="pricing"><%# Eval("Price") %></span>
                                            </div>
                                            <%if (Wcss._Config._ShowServiceFeesOnInfoPages)
                                              {%>
                                            <div>
                                                <span class="label">service fee</span>
                                                <span class="pricing"><%# Eval("ServiceCharge") %></span>
                                            </div>
                                            <div class="each">
                                                <span class="label">each</span>
                                                <span class="pricing"><%# Eval("PerItemPrice", "{0:c}") %></span>
                                            </div>
                                            <%} %>
                                            <div class="clear" />
                                    </div>
                                </div>
                            </td>
                            <td class="iteminfo" rowspan="99">
                                <div class="eventdes">
                                    <asp:Literal ID="LiteralStatus" runat="server" EnableViewState="false" />
                                    <asp:Literal ID="LiteralPackage" runat="server" EnableViewState="false" />
                                    <asp:Literal ID="LiteralDescription" runat="server" EnableViewState="false" />
                                    <asp:Repeater ID="rptShowNames" runat="server" OnItemDataBound="ProcessDates">
                                        <ItemTemplate>
                                            <span class="eventdate">
                                                <span class="datelist">
                                                    <%# Eval("DateOfShow", "{0:ddd MMM dd yyyy}") %>
                                                </span>
                                                <span class="agestimes">
                                                    <%#Eval("AgeRecord.Name") %>
                                                    &nbsp;<span class="nowrap"><asp:Literal ID="LiteralTime" runat="server" EnableViewState="false" /></span>
                                                </span>
                                            </span>
                                            <asp:Literal ID="LiteralDateTitle" runat="server" EnableViewState="false" />
                                            <asp:Literal ID="litVenue" runat="server" EnableViewState="false" />
                                            <asp:Literal ID="LiteralDateStatus" runat="server" EnableViewState="false" />
                                            <asp:Literal ID="LiteralEventInfo" runat="server" EnableViewState="false" />
                                        </ItemTemplate>
                                        <SeparatorTemplate><hr class="separatepkg" /></SeparatorTemplate>
                                    </asp:Repeater>
                                    <asp:Literal ID="litCamping" runat="server" />
                                    <asp:Literal ID="LiteralPickupAndPost" runat="server" EnableViewState="false" />
                                </div>
                            </td>
                        </tr>                        
                    </table>
                    <asp:Literal ID="litBundle" EnableViewState="false" runat="server" />
                    </div>
                </ItemTemplate>                               
            </asp:Repeater>
            </div>
        </div>
    </div>
</div>
<%} %>