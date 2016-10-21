<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Charity.ascx.cs" Inherits="WillCallWeb.Components.Cart.Charity" %>
<div id="charitydonation">
    <div class="cart-sub-division">
        <div class="panel-wrapper">
            <div class="title"><asp:Literal ID="litCharitablelisting" runat="server" /></div>
            <div class="item-container">
                <div id="divListing" runat="server" class="charitablelisting">
                    <asp:Literal ID="litSelect" runat="server" Text="<div class='charityselect'>Select a charity:</div>" />
                    <asp:RadioButtonList ID="rdoListings" runat="server" RepeatLayout="Flow" AutoPostBack="true" OnSelectedIndexChanged="rdoListings_SelectedIndexChanged" 
                        OnDataBinding="rdoListings_DataBinding" ondatabound="rdoListings_DataBound" />
                </div>
                <div class="donationchoice">
                    <asp:CheckBox ID="chkDonate" runat="server" AutoPostBack="true" OnCheckedChanged="chkDonate_CheckedChanged" OnDataBinding="chkDonate_DataBinding" />
                    Add a $<asp:DropDownList ID="ddlAmounts" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlAmounts_SelectedIndexChanged" 
                        OnDataBinding="ddlAmounts_DataBinding" OnDataBound="ddlAmounts_DataBound" />
                    <asp:Literal ID="litAmount" runat="server" />
                    USD donation to my order total. (check to apply)
                </div>
            </div>
        </div>
    </div>
</div>