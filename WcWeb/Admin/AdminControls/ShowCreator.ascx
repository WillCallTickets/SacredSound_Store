<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ShowCreator.ascx.cs" Inherits="WillCallWeb.Admin.ShowCreator" EnableViewState="false" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="Editor_Venue.ascx" TagName="Editor_Venue" TagPrefix="uc1" %>
<%@ Register Src="Editor_Act.ascx" TagName="Editor_Act" TagPrefix="uc2" %>
<%@ Register src="~/Components/Util/CalendarClock.ascx" tagname="CalendarClock" tagprefix="uc1" %>
<div id="srceditor">
    <div id="showcreator">
        <asp:ValidationSummary ID="ValidationSummary2" CssClass="validationsummary" HeaderText="" ValidationGroup="create" runat="server" />
        <div class="jqhead rounded" style="margin-bottom:8px;margin-top:4px;">
            <h3 style="margin-bottom:0 !important;border:solid red 0px;">OR...Create A New Show</h3> 
            <asp:CustomValidator ID="CustomValidation" runat="server" CssClass="invisible" 
                Display="Static" ErrorMessage="CustomValidator" ValidationGroup="create">*</asp:CustomValidator>
            <div class="jqpnl rounded crt" style="margin-bottom:24px;">
                <table border="0" cellspacing="0" cellpadding="0" width="100%" class="edittabl">
                    <tr>
                        <th>Date / Doors</th>
                        <td colspan="2">
                              <uc1:CalendarClock ID="clockFirstDate" runat="server" UseTime="true" UseReset="false" 
                                ValidationGroup="create" Width="200px" />
                        </td>
                        <td class="intr">
                            Enter times as hh:mm - type A or P to toggle AM and PM
                        </td>
                    </tr>
                    <tr class="showtime">
                        <th>Show Time</th>
                        <td class="listing-row" style="white-space:nowrap;">
                            TBA <asp:CheckBox ID="chkTba" runat="server" /></td>
                        <td style="padding-left:12px;">
                            <uc1:CalendarClock ID="clockShowTime" runat="server" UseDate="false" UseTime="true" UseReset="false" 
                                ValidationGroup="create" Width="120px" />
                        </td>                        
                        <td style="width:100%;padding-left:12px;" colspan="2">&nbsp;</td>
                    </tr>
                    <tr>
                        <th>Ages</th>
                        <td style="padding-top:3px;" colspan="2">
                            <asp:DropDownList ID="ddlAges" CssClass="ages" runat="server" Width="300px" OnDataBinding="ddlAges_DataBinding" 
                                OnDataBound="ddlAges_DataBound" /></td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <th>Announce</th>
                        <td colspan="2"><uc1:CalendarClock ID="clockAnnounce" runat="server" UseTime="true" UseReset="true" ValidationGroup="create" /></td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <th>On Sale</th>
                        <td colspan="2">
                            <uc1:CalendarClock ID="clockOnsale" runat="server" UseTime="true" UseReset="true" ValidationGroup="create" />
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                </table>
            </div>

            <uc1:Editor_Venue ID="Editor_Venue1" AbbreviatedDisplay="true" runat="server" TitleText="" SelectText="VENUE" MaxImageDimension="100" />
            <uc2:Editor_Act ID="Editor_Act1" runat="server" TitleText="" SelectText="MAIN ACT" MaxImageDimension="100" />

            <div class="cmdsection">
                <asp:Button ID="btnAdd" runat="server" CausesValidation="true" CssClass="btnmed" Width="100px" 
                    Text="CREATE SHOW" OnDataBinding="btnAdd_DataBinding" 
                    OnClick="btnAdd_Click" ValidationGroup="create" />
                <span class="subtitle" style="width:100%;">Date &amp; Doors, Venue and Act are required.</span>
                <br /><br />
            </div>
        </div>
    </div>
</div>