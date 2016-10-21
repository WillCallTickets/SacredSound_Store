<%@ Control Language="C#" AutoEventWireup="true" EnableViewState="false" CodeFile="CheckSlider.ascx.cs" Inherits="WillCallWeb.Components.Util.CheckSlider" %>
<div class="check-slider" >
    <div class="title-row">
        <asp:CheckBox ID="chkSelect" runat="server" AutoPostBack="true" OnCheckedChanged="chkSelect_CheckChanged" />
        <asp:Label ID="lblTitle" runat="server" CssClass="title-text" />
    </div>
    <div class="slider-row" runat="server">
        <asp:Literal ID="litRange" runat="server" OnDataBinding="litRange_DataBinding" />
    </div>
</div>