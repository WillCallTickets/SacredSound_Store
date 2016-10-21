<%@ Control Language="C#" AutoEventWireup="true" EnableViewState="false" CodeFile="CalendarClock.ascx.cs" Inherits="WillCallWeb.Components.Util.CalendarClock" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<div class="calendarclock" >
    <table width='<%=Width %>' border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td class="datepart">
                <asp:TextBox ID="txtDate" class="txtbox" MaxLength="25" Width="75px" runat="server" tooltip="Click calendar button to set date" AutoPostBack="true" 
                    OnTextChanged="SelectedDateChange" EnableViewState="false" />
            </td><td><asp:Button ID="ImageButton2" CssClass="calbtn" runat="server" ToolTip="Click to see calendar" Text="Calendar" EnableViewState="false" /></td>
            <td class="timepart">
                <span>
                    <asp:DropDownList ID="ddlHour" runat="server" AutoPostBack="true" OnSelectedIndexChanged="SelectedDateChange" EnableViewState="true" >
                        <asp:ListItem Text="12" /><asp:ListItem Text="1" /><asp:ListItem Text="2" /><asp:ListItem Text="3" /><asp:ListItem Text="4" />
                        <asp:ListItem Text="5" /><asp:ListItem Text="6" /><asp:ListItem Text="7" /><asp:ListItem Text="8" Selected="True" />
                        <asp:ListItem Text="9" /><asp:ListItem Text="10" /><asp:ListItem Text="11" />
                    </asp:DropDownList>
                    <asp:DropDownList ID="ddlMinute" runat="server" AutoPostBack="true" OnSelectedIndexChanged="SelectedDateChange" EnableViewState="true" >
                        <asp:ListItem Text="00" Selected="True" /><asp:ListItem Text="05" /><asp:ListItem Text="10" /><asp:ListItem Text="15" />
                        <asp:ListItem Text="20" /><asp:ListItem Text="25" /><asp:ListItem Text="30" /><asp:ListItem Text="35" />
                        <asp:ListItem Text="40" /><asp:ListItem Text="45" /><asp:ListItem Text="50" /><asp:ListItem Text="55" />
                        <asp:ListItem Text="59" /></asp:DropDownList>
                    <asp:DropDownList ID="ddlAmpm" runat="server" AutoPostBack="true" OnSelectedIndexChanged="SelectedDateChange" >
                        <asp:ListItem Text="PM" Selected="True" /><asp:ListItem Text="AM" /></asp:DropDownList>
                </span>
            </td>
            <td style="width:100%;">
                <asp:LinkButton ID="btnReset" runat="server" CausesValidation="false" ToolTip="Reset the current date selection." Text="reset" 
                    onclick="btnReset_Click" EnableViewState="false" />
               <cc1:MaskedEditExtender ID="MaskedEditExtender1" runat="server" AcceptAMPM="false" Mask="99/99/9999" MaskType="Date" 
                    MessageValidatorTip="true" TargetControlID="txtDate" OnFocusCssClass="maskededitfocus" OnInvalidCssClass="maskedediterror" />
                <cc1:MaskedEditValidator ID="MaskedEditValidator1" EnableViewState="true" runat="server" ControlExtender="MaskedEditExtender1" ControlToValidate="txtDate" 
                     InvalidValueMessage="Date entry is invalid" ValidationGroup='<%#ValidationGroup %>' ErrorMessage="Date entry is invalid"
                     ToolTip="Please enter a date in the format MM/DD/YYYY" Display="dynamic" Text="Date is required in the format MM/DD/YYYY" CssClass="validator" />
                <cc1:CalendarExtender ID="CalendarExtender1" Format="MM/dd/yyyy" runat="server" PopupButtonID="ImageButton2" PopupPosition="TopLeft" 
                    TargetControlID="txtDate" EnableViewState="true" /></td>
        </tr>
    </table>
</div>