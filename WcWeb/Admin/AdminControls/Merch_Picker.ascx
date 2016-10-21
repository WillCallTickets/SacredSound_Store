<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Merch_Picker.ascx.cs" Inherits="WillCallWeb.Admin.AdminControls.Merch_Picker" %>
<%@ Register Src="Menu_MerchListing.ascx" TagName="Menu_MerchListing" TagPrefix="uc1" %>
<div id="merchselection">
    <div class="jqhead rounded">
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="edittabl">
            <tr>
                <th style="text-align:left;">Merchandise Picker</th>
                <td>
                    <asp:Button ID="btnAddNew" CausesValidation="false" runat="server" CommandName="AddNew" 
                        Text="Add Item" OnClick="btnAddNew_Click" CssClass="btnmed" />
                    <asp:Button ID="btnRefresh" CausesValidation="false" runat="server" CommandName="Refresh" 
                        Text="Refresh" OnClick="btnRefresh_Click" CssClass="btnmed" />
                </td>
                <td>
                     <asp:RadioButtonList ID="rdoListContext" runat="server" AutoPostBack="true" 
                        onselectedindexchanged="rdoListContext_SelectedIndexChanged" CellPadding="4" CellSpacing="4"
                        RepeatDirection="Horizontal" RepeatLayout="Flow">
                            <asp:ListItem Text="All &nbsp;" Value="0" Selected="True" />
                            <asp:ListItem Text="Active Only" Value="1" />
                        </asp:RadioButtonList>
                </td>
            </tr>
        </table>
    </div>
    <div id="merchpicker">
        <uc1:Menu_MerchListing ID="Menu_MerchListing1" runat="server" />
    </div>
</div>