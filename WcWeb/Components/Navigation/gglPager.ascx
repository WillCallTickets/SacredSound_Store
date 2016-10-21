<%@ Control Language="C#" AutoEventWireup="true" CodeFile="gglPager.ascx.cs" EnableViewState="true" Inherits="WillCallWeb.Components.Navigation.gglPager" %>
<div class="googlepagercontainer rounded">
    <div class="pager-title"><%= PagerTitle %></div>
    <div class="ggl-template"><asp:PlaceHolder ID="placeValidation" runat="server" /></div>
    <div class="googlepager">
        <asp:Button CssClass="" ID="btnFirst" CausesValidation="false" runat="server" Tooltip="first" CommandName="firstpage" Text="|&#9666;&#9666;" OnClick="nav_Click" />
        <asp:Button CssClass="" ID="btnPrev" CausesValidation="false" runat="server" Tooltip="prev" CommandName="prevpage" Text="&#9668;" OnClick="nav_Click" />
        <span class="ggllinks">
        <asp:Repeater ID="rptPageLink" runat="server" OnDataBinding="rptPageLink_DataBinding" OnItemDataBound="rptPageLink_ItemDataBound"
            EnableViewState="true">
            <ItemTemplate>
                <asp:Button CssClass="" ID="btnPage" CausesValidation="false" runat="server" Tooltip="go to specified page" CommandName="page" Text='<%#Eval("Text") %>' 
                    CommandArgument='<%#Eval("Value") %>' OnClick="nav_Click" EnableViewState="true" />
            </ItemTemplate>
        </asp:Repeater>
        </span>
        <asp:Button CssClass="" ID="btnNext" CausesValidation="false" runat="server" ToolTip="next" CommandName="nextpage" Text="&#9658;" OnClick="nav_Click" />
        <asp:Button CssClass="" ID="btnLast" CausesValidation="false" runat="server" Tooltip="last" CommandName="lastpage" Text="&#9656;&#9656;|" OnClick="nav_Click" />
        <strong>Viewing</strong> <asp:Literal ID="litViewing" runat="server" EnableViewState="false" />
        <strong>Page Size</strong>
        <asp:DropDownList ID="ddlPageSize" CssClass="fxddl" runat="Server" AutoPostBack="True" OnSelectedIndexChanged="ddlPageSize_SelectedIndexChanged" 
            OnDataBinding="ddlPageSize_DataBinding" />
    </div>
</div>