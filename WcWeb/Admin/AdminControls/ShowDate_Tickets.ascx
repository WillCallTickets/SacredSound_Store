<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ShowDate_Tickets.ascx.cs" Inherits="WillCallWeb.Admin.AdminControls.ShowDate_Tickets" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register src="~/Components/Util/CalendarClock.ascx" tagname="CalendarClock" tagprefix="uc1" %>
<div id="srceditor">
    <div id="showticket">
        <div class="jqhead rounded">
            <h3 class="entry-title">Tickets - <asp:Literal ID="litShowTitle" runat="server" /></h3>
            
            <asp:GridView ID="GridView1" Width="100%" DataSourceID="SqlShowDates" runat="server" AutoGenerateColumns="False" DataKeyNames="Id" 
                ShowFooter="False" CssClass="lsttbl" 
                OnDataBound="GridView1_DataBound" 
                OnRowCommand="GridView1_RowCommand" 
                OnSelectedIndexChanged="GridView1_SelectedIndexChanged" >
                <SelectedRowStyle CssClass="selected" />
                <Columns>
                    <asp:TemplateField ItemStyle-Wrap="false" HeaderText="DATES">
                        <ItemTemplate>
                            <asp:LinkButton Width="20px" Id="btnSelect" CausesValidation="false" ToolTip="Select" CssClass="btnselect" runat="server" CommandName="Select" 
                                CommandArgument='<%#Eval("Id") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Center" HeaderText="Date Of Show">
                        <ItemTemplate>
                            <%#Eval("DateOfShow", "{0:MM/dd/yyyy hh:mmtt}") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="ShowTime" HeaderText="Show Time" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="AgeName" HeaderText="Ages" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="StatusName" HeaderText="Status" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="StatusText" HeaderText="StatusText" ItemStyle-Width="100%" HeaderStyle-HorizontalAlign="left" />
                 </Columns>
            </asp:GridView>
                    
            <asp:GridView ID="GridViewEntity" Width="100%" DataSourceID="SqlEntityCollection" runat="server" AutoGenerateColumns="False" 
                DataKeyNames="Id,iSold" ShowFooter="False" CssClass="lsttbl" 
                OnDataBound="GridViewEntity_DataBound" 
                OnRowCommand="GridViewEntity_RowCommand" 
                OnRowDataBound="GridViewEntity_RowDataBound" 
                OnRowDeleting="GridViewEntity_RowDeleting" >
                <HeaderStyle HorizontalAlign="left" />
                <SelectedRowStyle CssClass="selected" />
                <EmptyDataTemplate>
                    <div class="lstempty">No Tickets For Selected Date</div>
                </EmptyDataTemplate>
                <Columns>
                    <asp:TemplateField ItemStyle-Wrap="false" HeaderText="TKTS">
                        <ItemTemplate>
                            <div style="text-align:center;"><%#Eval("Id") %></div>
                            <asp:LinkButton Width="20px" Id="btnSelect" CausesValidation="false" ToolTip="Select" CssClass="btnselect" runat="server" CommandName="Select" 
                                CommandArgument='<%#Eval("Id") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Description" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="100%" >
                        <ItemTemplate>                            
                            <asp:Literal Visible='<%#Eval("bDosTicket") %>' ID="litDosIndicator" runat="server" 
                                Text="<span style='color:red;font-weight:bold;'>DOS</span>" />
                            <asp:Literal ID="litDesc" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:CheckBoxField DataField="bActive" HeaderText="Act" ReadOnly="true" HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="Center" />
                    <asp:TemplateField HeaderText="Pkg" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:CheckBox ID="chkPackage" runat="server" Enabled="false" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Vendor" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate><asp:Literal ID="litVendor" runat="server" /></ItemTemplate>
                    </asp:TemplateField>
                    <asp:CheckBoxField DataField="bUnlockActive" HeaderText="Code" ReadOnly="true" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="iAllotment" HeaderText="Allot" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="pendingStock" HeaderText="Pend" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="iSold" HeaderText="Sold" ItemStyle-HorizontalAlign="Center" />
                    <asp:TemplateField HeaderText="Avail" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate><asp:Literal ID="litAvailable" runat="server" /></ItemTemplate>
                    </asp:TemplateField>
                    
                    <asp:CheckBoxField DataField="bSoldOut" HeaderText="SO" ReadOnly="true" ItemStyle-HorizontalAlign="Center" />
                    <asp:CheckBoxField DataField="bAllowShipping" HeaderText="Ship" ReadOnly="true" ItemStyle-HorizontalAlign="Center" />
                    <asp:CheckBoxField DataField="bAllowWillCall" HeaderText="WCall" ReadOnly="true" ItemStyle-HorizontalAlign="Center" />
                    <asp:CheckBoxField DataField="bHideShipMethod" HeaderText="Hide WC" ReadOnly="true" ItemStyle-HorizontalAlign="Center" />
                    <asp:CheckBoxField DataField="bOverrideSellout" HeaderText="Over SO" ReadOnly="true" ItemStyle-HorizontalAlign="Center" />
                    <asp:TemplateField ItemStyle-Wrap="false" >
                       <ItemTemplate>
                           <asp:LinkButton Width="20px" Id="btnUp" ToolTip="Move Up" CssClass="btnup" runat="server" CommandName="Up" 
                                CommandArgument='<%#Eval("Id") %>' CausesValidation="false" />
                           <asp:LinkButton Width="20px" Id="btnDown" ToolTip="Move Down" CssClass="btndown" runat="server" CommandName="Down" 
                                CommandArgument='<%#Eval("Id") %>' CausesValidation="false" />
                           <asp:CustomValidator ID="CustomValidation" runat="server" CssClass="invisible" 
                                Display="Static" ErrorMessage="bad mojo" ValidationGroup="showticket">*</asp:CustomValidator>
                           <asp:LinkButton Width="20px" Id="btnDelete" CssClass="btndelete lastinrow" runat="server" CommandName="Delete" 
                               CommandArgument='<%#Eval("Id") %>' ToolTip="Delete" CausesValidation="false"
                               OnClientClick='return confirm("Are you sure you want to delete this row?")' />
                           
                       </ItemTemplate>
                   </asp:TemplateField>
                 </Columns>
            </asp:GridView>
        </div>
        
        
        <asp:ValidationSummary ID="ValidationSummary2" CssClass="validationsummary" HeaderText="" ValidationGroup="showticket" runat="server" />    
        <asp:FormView Width="100%" ID="FormView1" runat="server" DefaultMode="Edit" 
            DataSourceID="SqlEntity" DataKeyNames="Id" 
            OnItemInserting="FormView1_ItemInserting" 
            OnItemUpdated="FormView1_ItemUpdated" 
            OnDataBound="FormView1_DataBound" 
            OnItemCommand="FormView1_ItemCommand" 
            OnItemUpdating="FormView1_ItemUpdating" >
            <EmptyDataTemplate>
                <div class="jqpnl rounded eit">
                    <div class="cmdsection">
                        <asp:Button Id="btnNew" CausesValidation="false" runat="server" CommandName="New" 
                            Text="CREATE TICKET" CssClass="btnmed" />
                    </div>
                </div>
            </EmptyDataTemplate>
            <EditItemTemplate>
                <div class="jqpnl rounded eit">
                    <div class="cmdsection">
                        <asp:Button ID="btnSave" CausesValidation="true" ValidationGroup="showticket" runat="server" CommandName="Update" 
                            Text="Save" CssClass="btnmed" />
                        <asp:Button ID="btnCancel" runat="server" CommandName="Cancel" CausesValidation="false" 
                            Text="Cancel" CssClass="btnmed" />
                        <asp:Button ID="btnNew" runat="server" CommandName="New" CausesValidation="false" 
                            Text="New" CssClass="btnmed" />
                        <asp:CustomValidator ID="CustomValidation" runat="server" ValidationGroup="showticket" CssClass="validator" 
                                Display="Static" >*</asp:CustomValidator>
                        <asp:Button ID="btnSales" runat="server" CommandName="viewsales" CommandArgument='<%#Eval("Id") %>' CausesValidation="false" 
                            Text="View Sales" CssClass="btnmed" />
                        <asp:Button ID="btnDos" runat="server" onclick="btnDos_Click" CommandName="Create" CausesValidation="false" 
                            CommandArgument='<%#Eval("Id") %>' Text="Create Dos" CssClass="btnmed" 
                            ToolTip="This will create a DOS ticket that corresponds with this ticket. The inventory from this ticket will be pulled over on the DOS. If not enabled, the DOS ticket already exists." />
                        <asp:Button ID="btnLottery" runat="server" Visible="false" CommandName="lottery" CausesValidation="false" 
                            Text="Convert Lottery"  CssClass="btnmed" />
                        <asp:Button ID="btnBundle" runat="server" CommandName="bundle" CommandArgument='<%#Eval("Id") %>' CausesValidation="false" 
                            Text="View Bundles" CssClass="btnmed" />
                        <asp:Button ID="btnDisplay" runat="server" CausesValidation="false" CommandName="view" Text="Display Show" 
                            CssClass="btnmed" OnClientClick='doPagePopup("/Admin/ShowDisplayer.aspx", "false"); ' />
                    </div>
                        <table border="0" cellspacing="0" cellpadding="0" width="100%" class="edittabl">
                            <tr>
                                <td colspan="5" class="listing-row" style="width:100%">
                                    <span class="intr"><%#Eval("Id") %></span>
                                    <asp:Literal Visible='<%#Eval("bDosTicket") %>' ID="litDosIndicator" runat="server" 
                                        Text="<span style='color:red;font-weight:bold;font-size:16px;'>DOS</span>" />
                                    Active
                                    <asp:CheckBox id="chkActive" runat="server" checked='<%#Bind("bActive")%>' />
                                    SoldOut
                                    <asp:CheckBox id="chkSoldOut" runat="server" checked='<%#Bind("bSoldOut")%>' />
                                    Shipping
                                    <asp:CheckBox id="chkAllowShip" runat="server" checked='<%#Bind("bAllowShipping") %>' />
                                    WillCall
                                    <asp:CheckBox id="chkAllowWillCall" runat="server" checked='<%#Bind("bAllowWillCall") %>' />
                                    <a href="javascript: alert('This will hide the will call method within the order flow. WillCall must be the only selected option. eg: Shipping cannot be selected');" class="infomark">?</a>
                                    HideWillCall
                                    <asp:CheckBox id="chkHideShipMethod" runat="server" checked='<%#Bind("bHideShipMethod") %>' /> 
                                    <asp:CustomValidator ID="CustomShipOptions" runat="server" ValidationGroup="showticket" CssClass="validator" 
                                        Display="Static" ErrorMessage="Either shipping or will call must be allowed."
                                        OnServerValidate="CustomShipOptions_ServerValidate">*</asp:CustomValidator>
                                </td>
                            </tr>
                            <tr>
                                <th>Incl Shipping</th>
                                <td>
                                    <asp:CheckBox ID="chkSeparate" runat="server" Checked='<%#Bind("bShipSeparate") %>' />
                                </td>
                                <th>Incl Method</th>
                                <td colspan="2" style="width:100%;" width="100%">
                                    <asp:TextBox ID="txtFlatMethod" runat="server" Text='<%#Bind("vcFlatMethod") %>' />
                                </td>
                            </tr>
                            <tr>
                                <th>Price</th>
                                <td>
                                    <asp:TextBox ID="txtPrice" MaxLength="10" Width="65px" runat="server" Text='<%#Bind("mPrice", "{0:n}") %>' />
                                </td>
                                <th>Service Fee</th>
                                <td>
                                    <asp:TextBox ID="txtService" MaxLength="10" Width="65px" runat="server" Text='<%#Bind("mServiceCharge", "{0:n}") %>' />
                                </td>
                                <td rowspan="2">
                                    <span class="intr">
                                        <b>Please note that the max $$ per order is set to <%= Wcss._Config._AuthorizeNetLimit.ToString("c") %>
                                        <br />
                                        Notify Rob if changes are necessary</b>
                                    </span>
                                </td>
                            </tr>
                            <tr>
                                <th><a href="javascript: alert('The amount of tickets available for sale. If this is a package ticket, it will set the inventory for all of the tickets in the package.')" class="infomark">?</a>
                                    Allotment</th>
                                <td>
                                    <asp:TextBox ID="txtAllotment" MaxLength="10" Width="65px" runat="server" Text='<%#Bind("iAllotment") %>' />
                                </td>
                                <th>Max Per Order</th>
                                <td>
                                    <asp:TextBox ID="txtMaxPer" MaxLength="10" Width="65px" runat="server" Text='<%#Bind("iMaxQtyPerOrder") %>' />
                                </td>
                            </tr>
                            <tr>
                                <th>Vendor</th>
                                <td><asp:DropDownList ID="ddlVendor" runat="server" /></td>
                                <th>Ages</th>
                                <td colspan="2"><asp:DropDownList ID="ddlAges" Width="350px" runat="server" /></td>
                            </tr>
                            <tr>
                                <th><a href="javascript: alert('This will show a status message above the ticket selection.')" class="infomark">?</a>
                                    Status</th>
                                <td colspan="4" style="width:100%;"><asp:TextBox ID="txtPre" MaxLength="500" Width="650px" runat="server" Text='<%#Bind("Status") %>' /></td>
                            </tr>
                            <tr>
                                <td colspan="5">
                                    <div class="jqinstruction rounded">
                                        Camping Passes entered as a ticket can ship with tickets - they cannot be packages.<br />
                                        Take note *** They will also show up in the manifests<br />
                                        To designate a camping pass, include the two-word phrase "Camping Pass" (case-inSENsitive) in either description1<br />
                                        Specify the Valid dates, times and any other pertinent info in either description<br />
                                        Camping passes will display their name in the format [Description1][Description2][Ages][ShowName]<br />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <th><a href="javascript: alert('Misc info regarding the ticket. eg: &#34;portion donated to charity.&#34;')" class="infomark">?</a>
                                    Description 1</th>
                                <td colspan="4"><asp:TextBox ID="txtDescription" MaxLength="300" Width="650px" runat="server" Text='<%#Bind("SalesDescription") %>' /></td>
                            </tr>
                            <tr>
                                <th><a href="javascript: alert('Misc info regarding the ticket. eg: &#34;This things starts super late.&#34;')" class="infomark">?</a>
                                    Description 2</th>
                                <td colspan="4"><asp:TextBox ID="txtCriteria" MaxLength="300" Width="650px" runat="server" Text='<%#Bind("CriteriaText") %>' /></td>
                            </tr>
                            <tr>
                                <th style="padding-top:10px;"><a href="javascript: alert('The date when tickets are available to public. Overrides the unlock code, if any.')" class="infomark">?</a>
                                    Public Onsale</th>
                                <td colspan="4">      
                                    <uc1:CalendarClock ID="clockOnsale" runat="server" UseTime="true" SelectedValue='<%#Bind("dtPublicOnsale") %>' 
                                        UseReset="true" ValidationGroup="showticket" />
                                </td>
                            </tr>
                            <tr>
                                <th style="padding-top:10px;"><a href="javascript: alert('The date when tickets go offsale to public.')" class="infomark">?</a>
                                    Offsale Date</th>
                                <td colspan="4">
                                    <uc1:CalendarClock ID="clockOffsale" runat="server" UseTime="true" SelectedValue='<%#Bind("dtEndDate") %>' 
                                        DefaultValue='<%#DateTime.MaxValue %>' UseReset="true" />
                                </td>
                            </tr>
                            <tr>
                                <th style="padding-top:10px;"><a href="javascript: alert('The date when tickets can no longer be shipped. Set to &#34;Shipping_Ticket_CutoffDays&#34; in Settings&raquo;Shipping. ex: If set to the 15th, shipping can be purchased up to the 14th at 11:59PM.')" class="infomark">?</a>
                                    Ship Cutoff</th>
                                <td colspan="4">
                                    <div style="float:left;">
                                        <span style="float:left;width:15%;"><uc1:CalendarClock ID="clockCutoff" runat="server" UseTime="false" SelectedValue='<%#Bind("dtShipCutoff") %>'
                                            UseReset="false" /></span>
                                        <span style="float:right;width:83%;" class="intr">
                                            {12AM is cutoff time. It may be easier to think of this as 12:01AM of the specified date} Tickets must be purchased prior to this date to qualify for shipping. If the ticket does not have a willcall option, then this ticket will be listed as unavailable.
                                        </span>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5">
                                    <div class="jqinstruction rounded">
                                        <strong>Linked Tickets...</strong><asp:Literal ID="litLink" runat="server" />
                                    </div>
                                    <div class="jqinstruction rounded">
                                        <ul style="width:100%;"><strong>Private Access tickets</strong>
                                            <li>Set the public onsale date of the ticket to a date greater than the show date, it might even be a good idea to put it a few days past.</li>
                                            <li>The ticket will, therefore, not show up in the normal flow and only members with access will be able to purchase.</li>
                                            <li><a style="font-size:14px;" href="/Admin/ProductAccess.aspx?p=campaign">Edit product accessors</a></li>
                                            <asp:Literal ID="litProductAccess" runat="server" />
                                        </ul>
                                    </div>
                                    <div class="jqinstruction rounded">
                                        <ul><strong>Overriding Sellout tickets</strong>
                                            <li>Only applies to sold out shows and not show dates. Use a non-overriden coded ticket for restricted access to individual dates</li>
                                            <li>To use without a code...</li>
                                            <li>---set the public onsale date (above) to your desired start date</li>
                                            <li>&nbsp;</li>
                                            <li>To use with a resticted access code</li>
                                            <li>---set the public onsale date (above) to a date greater than the show date</li>
                                            <li>---set the start date of the unlock code (below) to your desired start date and time</li>
                                        </ul>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <th>Override Sellout</th>
                                <td colspan="4">
                                    <asp:CheckBox ID="chkOverSell" runat="server" Checked='<%#Bind("bOverrideSellout") %>' Text="" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5">
                                    <div class="jqinstruction rounded">
                                        <ul><strong>Unlock Code</strong>
                                            <li>The public onsale date must be specified.</li>
                                            <li>If this ticket will never be available for general public, set the date greater than the date of show.</li>
                                            <li>The link must be copied exactly and without any trailing spaces.</li>
                                            <li>To end the code sales, set the end date to a date prior to today.</li>
                                        </ul>
                                    </div>
                                </td>
                            </tr>
                        
                            <tr>
                                <th>Unlock Code</th>
                                <td colspan="4" class="listing-row">
                                    Required
                                    <asp:CheckBox ID="chkCode" runat="server" Checked='<%#Bind("bUnlockActive") %>' />
                                    <asp:TextBox ID="txtCode" MaxLength="25" Width="125px" runat="server" Text='<%#Bind("UnlockCode") %>' />
                                    <asp:Button ID="btnGenerate" runat="server" CommandName="generatecode" Text="Generate Code" Width="120px" 
                                        CssClass="btnmed" CommandArgument='<%#Eval("Id") %>' />
                                    <span style="margin-left: 20px;"><asp:Literal ID="litUnlockLink" runat="server" /></span>
                                </td>
                            </tr>
                            <tr>
                                <th style="padding-top:10px;"><a href="javascript: alert('If no date is specified, these tickets will be available immediately via the link shown.')" class="infomark">?</a>
                                    Start Date</th>
                                <td colspan="4">
                                    <uc1:CalendarClock ID="clockUnlockStart" runat="server" UseTime="true" SelectedValue='<%#Bind("dtUnlockDate") %>' 
                                        UseReset="true" ValidationGroup="showticket" />
                                </td>
                            </tr>
                            <tr>
                                <th style="padding-top:10px;"><a href="javascript: alert('If no date is specified, these tickets will be available until the show date.')" class="infomark">?</a>
                                    End Date</th>
                                <td colspan="4">
                                    <uc1:CalendarClock ID="clockUnlockEnd" runat="server" UseTime="true" DefaultValue='<%#DateTime.MaxValue %>' 
                                        SelectedValue='<%#Bind("dtUnlockEndDate") %>' UseReset="true" ValidationGroup="showticket" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5">
                                    <div class="jqinstruction rounded">
                                        <ul><strong>PARENT of Dos TICKET NOTES</strong>
                                            <li>Any changes made to the parent ticket to the following fields will propagate to the DOS ticket:</li>
                                            <li>Vendor, Description Line 1, Description Line 2, Ages, Active, SoldOut, Status, UnlockActive, UnlockCode, UnlockDate, 
		                                        UnlockEndDate, MaxQtyPerOrder</li>
                                        </ul>
                                        <br />
                                        <ul><strong>DOS TICKET NOTES</strong>
                                            <li>The following fields cannot be altered for DOS tickets: Vendor, AllowShipping, AllowWillCall, ShipCutoff, UnlockCode</li>
                                            <li>Active - works independent of the parent</li>
                                            <li>Allotment - this is set at midnight of day of show. Inventory is transferred from the parent ticket. If you need to alter, you can change 
                                            the allotment as usual on the DOS ONLY!!! If you change allotment prior to the day of show - it will be written over at midnight DOS from the parent tickets&#39; available inventory.</li>
                                            <li>Onsale date is set to midnight + turnover time  day of show.</li>
                                            <li>Offsale date is set to 3 hours before doors</li>
                                        </ul>
                                    </div>
                                    <div class="jqinstruction rounded">
                                        <ul><strong>PAST PURCHASE REQUIREMENTS</strong>
                                            <li>Specify past ticket ids required to unlock this ticket. Separate by commas.</li>
                                            <li>The list is for showing past ticket ids and will only display the past 150 tickets.</li>
                                            <li>You will need to manually search to get other ticket ids.</li>
                                            <li>Qty Limit will limit qty to purchase based on past purchase qty. Will apply to all requirements.</li>
                                            <li>Display visibility will make the ticket displayable only if requirements met (except qty) will apply to all requirements.</li>
                                        </ul>
                                        <br />
                                    </div>
                                </td>
                            </tr>
                        </table>    
                        <div class="cmdsection">
                            <asp:Button ID="Button1" CausesValidation="true" ValidationGroup="showticket" runat="server" CommandName="Update" 
                                Text="Save" CssClass="btnmed" />
                            <asp:Button ID="Button2" runat="server" CommandName="Cancel" CausesValidation="false" 
                                Text="Cancel" CssClass="btnmed" />
                            <asp:Button ID="Button3" runat="server" CommandName="New" CausesValidation="false" 
                                Text="New" CssClass="btnmed" />
                        </div>
                    </div>
            </EditItemTemplate>
            <InsertItemTemplate>
                <div class="jqpnl rounded iit">
                    <h3 class="entry-title">Adding A New Ticket...</h3>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" class="edittabl">
                        <tr>
                            <th><a href="javascript: alert('If this show has other dates, this will allow you to copy tickets from other dates.')" class="infomark">?</a>
                                <asp:Button ID="btnCopy" CausesValidation="false" CssClass="btnmed" width="100px"
                                    runat="server" CommandName="Copy" Text="Copy Selected"
                                    OnClientClick="javascript: return confirm('Are you certain you would like to copy the selection?');" />
                            </th>
                            <td style="width:100%;">
                                <asp:DropDownList ID="ddlCopyTicket" runat="server" Width="100%" OnDataBinding="ddlCopy_DataBinding" OnDataBound="ddlCopy_DataBound"
                                        AppendDataBoundItems="true">
                                    <asp:ListItem Text="<-- Select a ticket to copy (dos tickets and packages cannot be copied)-->" Selected="True" />
                                </asp:DropDownList>
                                <span class="intr">Only tickets from other dates within the same show are eligible for copying.</span>
                            </td>
                        </tr>
                        <tr>
                            <th>Vendor</th>
                            <td colspan="3"><asp:DropDownList ID="ddlVendor" runat="server" /></td>
                        </tr>
                        <tr>
                            <th>Price</th>
                            <td>
                                <asp:TextBox ID="txtPrice" runat="server" MaxLength="6" Width="65px" TabIndex="2" />
                                <span class="intr">Service fee will be auto-calculated against price.</span>
                            </td>
                        </tr>
                        <tr>
                            <th>Ages</th>
                            <td colspan="3"><asp:DropDownList ID="ddlAges" Width="350px" runat="server" TabIndex="3" /></td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <div class="jqinstruction rounded">
                                    Camping Passes entered as a ticket can ship with tickets.<br />
                                    Take note *** They will also show up in the manifests<br />
                                    To designate a camping pass, include the two-word phrase "Camping Pass" (case-inSENsitive) in either description<br />
                                    Specify the Valid dates, times and any other pertinent info in either description<br />
                                    Camping passes will display their name in the format [Description1][Description2][Ages][ShowName]<br />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <th><a href="javascript: alert('Misc info regarding the ticket. eg: &#34;portion donated to charity&#34;')" class="infomark">?</a>
                                Description 1</th>
                            <td colspan="3">
                                <asp:TextBox ID="txtDescription" runat="server" MaxLength="300" Width="100%" TabIndex="5" />
                            </td>
                        </tr>
                        <tr>
                            <th><a href="javascript: alert('Misc info regarding the ticket. eg: &#34;this is going to be late&#34;')" class="infomark">?</a>
                                Description 2</th>
                            <td colspan="3">
                                <asp:TextBox ID="txtCriteria" runat="server" MaxLength="300" Width="100%" TabIndex="4" />
                            </td>
                        </tr>
                        <tr><td colspan="2">
                                <div class="jqinstruction rounded">
                                    <ul><strong>DOS Tickets</strong>
                                        <li>A DOS ticket has to be connected to a normal ticket (cannot be used on ticket packages).</li>
                                        <li>If you choose not to create one now, you can always create a DOS ticket later.</li>
                                        <li>DOS ticket will automatically go on sale at midnight + the offset time (generally 2 hours)</li>
                                        <li>DOS allotment will AUTOMATICALLY be taken from the leftover of the base ticket (above)</li>
                                        <li>DOS ticket sales are set to cut off 3 hours before doors</li>
                                        <li>DOS Price is required when creating a DOS ticket here.</li>
                                    </ul>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                <a href="javascript: alert('Checking this box will also create a ticket to be available Day Of Show if any allotment from the normal ticket is left over on the DOS. DOS Price will be rquired when checked.')" class="infomark">?</a>
                                Create DOS?
                            </th>
                            <td><asp:CheckBox id="chkDosTicket" runat="server" /></td>
                        </tr>
                        <tr>
                            <th class="headerlabel">Dos Price:</th>
                            <td><asp:TextBox ID="txtDosPrice" runat="server" MaxLength="6" Width="65px" TabIndex="2" /></td>                                    
                        </tr>
                        <tr>
                            <td colspan="2">
                                <div class="jqinstruction rounded">
                                    <ul><strong>Ticket Packages</strong>
                                        <li>notes on packages TBA</li>
                                    </ul>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                <asp:Button ID="btnAddPackage" runat="server" CommandName="AddPackage" 
                                    Text="Add Selected" CssClass="btnmed" Width="100px" />
                            </th>
                            <td><asp:DropDownList ID="ddlDateList" runat="server" AutoPostBack="false" Width="100%">
                                    <asp:ListItem Text="-- Select a ShowDate --" Value="0" />
                                    </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <th style="vertical-align:top;">
                                <div style="margin-bottom:16px;margin-right:12px;">Package Listing</div>
                                <asp:Button ID="btnRemovePackage" CssClass="btnmed" runat="server" CommandName="RemovePackage" 
                                    Text="Remove Selected" Width="100px" />
                            </th>
                            <td>
                                <asp:ListBox ID="listAddedPackages" Width="100%" Height="75px" runat="server"></asp:ListBox>
                            </td>
                        </tr>
                    </table>
                    <div class="cmdsection">
                        <asp:Button ID="btnSave" CausesValidation="false" runat="server" 
                            CommandName="Insert" Text="Add Ticket" CssClass="btnmed" />
                        <asp:Button ID="btnCancel" runat="server" CommandName="Cancel" CausesValidation="false" 
                            Text="Cancel" CssClass="btnmed" />
                        <asp:CustomValidator ID="CustomValidation" runat="server" ValidationGroup="showticket" CssClass="validator" 
                            Display="Static" >*</asp:CustomValidator>
                    </div>
                </div>
            </InsertItemTemplate>
        </asp:FormView>

        <asp:ValidationSummary ID="ValidationSummary11" CssClass="validationsummary" HeaderText="" ValidationGroup="lottery" runat="server" />
        <asp:CustomValidator ID="CustomValidator1" runat="server" ValidationGroup="lottery" CssClass="validator" Display="Static" >*</asp:CustomValidator>

        <asp:FormView Width="100%" ID="FormLottery" runat="server" DefaultMode="Edit" DataSourceID="SqlLottery" DataKeyNames="Id" 
            OnItemUpdating="FormLottery_ItemUpdating" >
            <EmptyDataTemplate>
                <div class="jqpnl rounded eit">
                    <div class="lstempty" style="display:none;">to add a lottery - use the convert to lottery button above</div>
                </div>
            </EmptyDataTemplate>
            <EditItemTemplate>
                <div class="jqpnl rounded eit">
                    <div class="cmdsection">
                        <asp:Button ID="btnSave" CausesValidation="true" ValidationGroup="lottery" runat="server" CommandName="Update" 
                            Text="Save" CssClass="btnmed" />
                        <asp:Button ID="btnCancel" runat="server" CommandName="Cancel" CausesValidation="false" 
                            Text="Cancel" CssClass="btnmed" />
                        <asp:CustomValidator ID="CustomValidation" runat="server" ValidationGroup="lottery" CssClass="validator" 
                            Display="Static" >*</asp:CustomValidator>
                    </div>
                    <table border="0" cellspacing="0" cellpadding="0" width="100%" class="edittabl">
                        <tr>
                            <th>Id</th>
                            <td style="width:100%;"><%#Eval("Id") %></td>
                        </tr>
                        <tr>
                            <th><a href="javascript: alert('Specify the signup activity to start and end.')" class="infomark">?</a>
                                Signup Active</th>
                            <td><asp:Checkbox ID="chkSignup" runat="server" Checked='<%#Bind("bActiveSignup") %>' /></td>
                        </tr>
                        <tr>
                            <th>Start Signup</th>
                            <td><uc1:CalendarClock ID="clockSignStart" runat="server" UseTime="false" SelectedValue='<%#Bind("dtSignupStart") %>'
                                    UseReset="false" ValidationGroup="lottery" /></td>
                        </tr>
                        <tr>
                            <th>End Signup</th>
                            <td><uc1:CalendarClock ID="clockSignEnd" runat="server" UseTime="false" SelectedValue='<%#Bind("dtSignupEnd") %>'
                                    UseReset="false" ValidationGroup="lottery" /></td>
                        </tr>
                        <tr>
                            <th><a href="javascript: alert('Specify the fulfillment period to start and end.')" class="infomark">?</a>
                                Fulfillment Active</th>
                            <td><asp:Checkbox ID="chkFulfill" runat="server" Checked='<%#Bind("bActiveFulFillment") %>' /></td>
                        </tr>
                        <tr>
                             <th>Start Fulfillment</th>
                             <td><uc1:CalendarClock ID="clockFillStart" runat="server" UseTime="false" SelectedValue='<%#Bind("dtFulfillStart") %>'
                                    UseReset="false" ValidationGroup="lottery" /></td>
                        </tr>
                        <tr>
                            <th>End Fulfillment</th>
                            <td><uc1:CalendarClock ID="clockFillEnd" runat="server" UseTime="false" SelectedValue='<%#Bind("dtFulfillEnd") %>'
                                    UseReset="false" ValidationGroup="lottery" /></td>
                        </tr>
                        <tr>
                            <th><a href="javascript: alert('A friendly name for the lottery &#34;Motet_081102&#34;.')" class="infomark">?</a>
                                Name</th>
                            <td>
                                <asp:TextBox ID="txtName" runat="server" MaxLength="50" Width="300px" Text='<%#Bind("Name") %>' />
                            </td>
                        </tr>
                        <tr>
                            <th><a href="javascript: alert('This will override the name to display.')" class="infomark">?</a>
                                Display Text</th>
                            <td><asp:TextBox ID="txtDisplay" runat="server" MaxLength="256" Width="300px" Text='<%#Bind("DisplayText") %>' /></td>
                        </tr>
                        <tr>
                            <th><a href="javascript: alert('A short description. Shows up on the ? page.')" class="infomark">?</a>
                                Description</th>
                            <td><asp:TextBox ID="txtDescription" TextMode="MultiLine" runat="server" MaxLength="500"
                                    Height="40px" Width="300px" Text='<%#Bind("Description") %>' /></td>
                        </tr>
                        <tr>
                            <th><a href="javascript: alert('A long descriptions. Shows on the ? page.')" class="infomark">?</a>
                                Writeup</th>
                            <td><asp:TextBox ID="txtWriteup" TextMode="MultiLine" runat="server" MaxLength="4000"
                                    Height="60px" Width="700px" Text='<%#Bind("Writeup") %>' /></td>
                        </tr>
                        <tr>
                            <th><a href="javascript: alert('A value here forces the buyer to buy a certain qty of tickets.')" class="infomark">?</a>
                                Force Qty</th>
                            <td><asp:TextBox ID="txtQty" runat="server" MaxLength="4" Width="100px" /></td>
                        </tr>
                    </table>
                </div>
            </EditItemTemplate>
        </asp:FormView>
    </div>
</div>
<asp:SqlDataSource ID="SqlShowDates" runat="server" ConnectionString="<%$ ConnectionStrings:WillCallConnectionString %>" 
    SelectCommand="SELECT sd.[Id], sd.[dtDateOfShow] as DateOfShow, sd.[ShowTime], a.[Name] as AgeName, ss.[Name] as StatusName, sd.[StatusText] 
        FROM [ShowDate] sd, [Age] a, [ShowStatus] ss 
        WHERE sd.[tShowId] = @ShowId AND sd.[bActive] = 1 AND sd.[tAgeId] = a.[Id] AND sd.[tStatusId] = ss.[Id] 
        ORDER BY CASE WHEN (sd.bLateNightShow IS NOT NULL AND sd.bLateNightShow = 1) THEN DATEADD(hh, 24, sd.[dtDateOfShow]) ELSE sd.[dtDateOfShow] END ">
    <SelectParameters>
        <asp:SessionParameter Name="ShowId" SessionField="Admin_CurrentShowId" />
    </SelectParameters>
</asp:SqlDataSource>
<asp:SqlDataSource ID="SqlEntityCollection" runat="server" ConnectionString="<%$ ConnectionStrings:WillCallConnectionString %>" 
    SelectCommand="
        SELECT ent.[Id], ent.[TVendorId], ent.[PriceText], ISNULL(ent.[mPrice],0.0) as mPrice , ent.[DosText], ent.[mDosPrice], ent.[mServiceCharge], a.[Name] as AgeName, 
        ent.[SalesDescription], ent.[CriteriaText], 
        ent.[bActive], ent.[bSoldOut], ent.[bDosTicket], ent.[bAllowShipping], ent.[bAllowWillCall], ent.[bHideShipMethod], ent.[iAllotment], 
        ISNULL(pending.[iQty], 0) as pendingStock, ent.[iSold],
        ent.[bOverrideSellout], ent.[bUnlockActive], ent.[iDisplayOrder] as DisplayOrder, CASE WHEN COUNT(link.[Id]) IS NULL THEN 0 ELSE COUNT(link.[Id]) END as LinkCount 
        FROM [ShowTicket] ent 
        LEFT OUTER JOIN fn_PendingStock('ticket') pending ON pending.[idx] = ent.[Id] 
        LEFT OUTER JOIN [ShowTicketPackageLink] link ON ent.[Id] = link.[ParentShowTicketId], [Age] a         
        WHERE ent.[TShowDateId] = @ShowDateId AND ent.[TAgeId] = a.[Id] 
        GROUP BY ent.[Id], ent.[TVendorId], ent.[PriceText], ent.[mPrice], ent.[DosText], ent.[mDosPrice], ent.[mServiceCharge], a.[Name], ent.[SalesDescription], ent.[CriteriaText], 
        ent.[bActive], ent.[bSoldOut], ent.[bDosTicket], ent.[bAllowShipping], ent.[bAllowWillCall], ent.[bHideShipMethod], ent.[iAllotment], pending.[iQty], ent.[iSold], 
        ent.[bOverrideSellout], ent.[bUnlockActive], ent.[iDisplayOrder] 
        ORDER BY ent.[iDisplayOrder]" 
        DeleteCommand="SELECT 0 ">
    <SelectParameters>
        <asp:ControlParameter ControlID="GridView1" Name="ShowDateId" PropertyName="SelectedValue" Type="Int32" />
    </SelectParameters>
</asp:SqlDataSource>
<asp:SqlDataSource ID="SqlEntity" runat="server" ConnectionString="<%$ ConnectionStrings:WillCallConnectionString %>" 
    SelectCommand="SELECT ent.[Id], ISNULL(d.[Id], 0) as DosId, 
	    CASE WHEN COUNT(link.[Id]) IS NULL THEN 0 ELSE COUNT(link.[Id]) END as LinkCount, 
	    ent.[TVendorId], ent.[CriteriaText], ent.[SalesDescription], ent.[TAgeId], a.[Name] as AgeName, 
	    ent.[bActive], ent.[bSoldOut], ent.[bDosTicket], ent.[Status], ent.[PriceText], ent.[mPrice],  ent.[DosText], 
	    ent.[mDosPrice], ent.[mServiceCharge], ent.[bAllowShipping], ent.[bAllowWillCall], ent.[bHideShipMethod], ent.[dtShipCutoff], 
        ent.[bOverrideSellout], ent.[bUnlockActive], ent.[UnlockCode], ent.[dtUnlockDate], ent.[dtUnlockEndDate], ent.[dtPublicOnsale], 
	    ent.[dtEndDate], ent.[iMaxQtyPerOrder], ent.[iAllotment], ISNULL(ent.[mFlatShip],0) as 'mFlatShip', ent.[vcFlatMethod], 
	    ISNULL(ent.[bShipSeparate],0) as bShipSeparate, ent.[dtBackorder] 
    FROM [ShowTicket] ent 
        LEFT OUTER JOIN [ShowTicketDosTicket] d ON d.[ParentId] = ent.[Id] 
	    LEFT OUTER JOIN [ShowTicketPackageLink] link ON ent.[Id] = link.[ParentShowTicketId], [Age] a 
    WHERE ent.[Id] = @Idx AND ent.[TAgeId] = a.[Id] 
    GROUP BY ent.[Id], d.[Id], ent.[TVendorId], ent.[CriteriaText], ent.[SalesDescription], ent.[TAgeId], a.[Name], 
	    ent.[bActive], ent.[bSoldOut], ent.[bDosTicket], ent.[Status], ent.[PriceText], ent.[mPrice],  ent.[DosText], 
	    ent.[mDosPrice], ent.[mServiceCharge], ent.[bAllowShipping], ent.[bAllowWillCall], ent.[bHideShipMethod], ent.[dtShipCutoff], 
        ent.[bOverrideSellout], ent.[bUnlockActive], ent.[UnlockCode], ent.[dtUnlockDate], ent.[dtUnlockEndDate], ent.[dtPublicOnsale], 
	    ent.[dtEndDate], ent.[iMaxQtyPerOrder], ent.[iAllotment], ent.[mFlatShip] , ent.[vcFlatMethod], 
	    ent.[bShipSeparate], ent.[dtBackorder], ent.[iDisplayOrder]
    ORDER BY ent.[iDisplayOrder]"
    UpdateCommand="CREATE TABLE #Tix(idx int); INSERT #Tix (idx) SELECT @Id as 'idx'; 
        INSERT #Tix (idx) SELECT [LinkedShowTicketId] as 'idx' FROM ShowTicketPackageLink WHERE [ParentShowTicketId] = @Id; 
        UPDATE [ShowTicket] SET [TVendorId] = @TVendorId, [CriteriaText] = @CriteriaText, [SalesDescription] = @SalesDescription, [TAgeId] = @TAgeId, [bActive] = @bActive, 
        [bSoldOut] = @bSoldOut, [Status] = @Status, [PriceText] = @PriceText, [mPrice] = @mPrice, 
        [DosText] = @DosText, [mDosPrice] = @mDosPrice, [mServiceCharge] = @mServiceCharge, [bAllowShipping] = @bAllowShipping,  [bAllowWillCall] = @bAllowWillCall,
        [bHideShipMethod] = @bHideShipMethod, 
        [dtShipCutoff] = @dtShipCutoff, [bOverrideSellout] = @bOverrideSellout, [bUnlockActive] = @bUnlockActive, [UnlockCode] = @UnlockCode, [dtUnlockDate] = @dtUnlockDate, 
        [dtUnlockEndDate] = @dtUnlockEndDate, [dtPublicOnsale] = @dtPublicOnsale, [dtEndDate] = @dtEndDate, [iMaxQtyPerOrder] = @iMaxQtyPerOrder, 
        [iAllotment] = @iAllotment, [mFlatShip] = @mFlatShip, [vcFlatMethod] = @vcFlatMethod, [bShipSeparate] = @bShipSeparate, [dtBackorder] = @dtBackorder 
        FROM ShowTicket st WHERE st.[Id] IN (SELECT [idx] FROM #Tix) 
        IF EXISTS(SELECT * FROM ShowTicketDosTicket WHERE [ParentId] = @Id) BEGIN 
	    UPDATE ShowTicket
	    SET [TVendorId] = @TVendorId, [CriteriaText] = @CriteriaText, [SalesDescription] = @SalesDescription, [TAgeId] = @TAgeId, [bActive] = @bActive, 
		    [bSoldOut] = @bSoldOut, [Status] = @Status, [bOverrideSellout] = @bOverrideSellout, [bUnlockActive] = @bUnlockActive, [UnlockCode] = @UnlockCode, [dtUnlockDate] = @dtUnlockDate, 
		    [dtUnlockEndDate] = @dtUnlockEndDate, [iMaxQtyPerOrder] = @iMaxQtyPerOrder, [dtBackorder] = @dtBackorder
	    FROM [ShowTicket] st, [ShowTicketDosTicket] dos
	    WHERE dos.[ParentId] = @Id AND dos.[DosId] = st.[Id]
        END"
    InsertCommand="SELECT 0 " 
    onupdating="SqlEntity_Updating"
    onupdated="SqlEntity_Updated"
     >
    <UpdateParameters>
        <asp:Parameter Name="CriteriaText" Type="String" ConvertEmptyStringToNull="false" />
        <asp:Parameter Name="SalesDescription" Type="String" ConvertEmptyStringToNull="false" />
        <asp:Parameter Name="TAgeId" Type="Int32" />
        <asp:Parameter Name="TVendorId" Type="Int32" />
        <asp:Parameter Name="bActive" Type="boolean" />
        <asp:Parameter Name="bSoldOut" Type="boolean" />
        <asp:Parameter Name="Status" Type="String" ConvertEmptyStringToNull="false" />
        <asp:Parameter Name="PriceText" Type="String" ConvertEmptyStringToNull="false" />
        <asp:Parameter Name="mPrice" Type="Decimal" />
        <asp:Parameter Name="DosText" Type="String" ConvertEmptyStringToNull="false" />
        <asp:Parameter Name="mDosPrice" Type="Decimal" DefaultValue="0" />
        <asp:Parameter Name="mServiceCharge" Type="Decimal" />
        <asp:Parameter Name="bAllowShipping" Type="boolean" />
        <asp:Parameter Name="bAllowWillCall" Type="boolean" />
        <asp:Parameter Name="bHideShipMethod" Type="boolean" />
        <asp:Parameter Name="dtShipCutoff" Type="dateTime" />
        <asp:Parameter Name="bOverrideSellout" Type="boolean" />
        <asp:Parameter Name="bUnlockActive" Type="boolean" />
        <asp:Parameter Name="UnlockCode" Type="String" />
        
        <asp:Parameter Name="dtUnlockDate" Type="string" ConvertEmptyStringToNull="true" />
        <asp:Parameter Name="dtUnlockEndDate" Type="string" ConvertEmptyStringToNull="true" />
        <asp:Parameter Name="dtPublicOnsale" Type="string" ConvertEmptyStringToNull="true" />
        <asp:Parameter Name="dtEndDate" Type="string" ConvertEmptyStringToNull="true" />
        
        <asp:Parameter Name="iMaxQtyPerOrder" Type="Int32" />
        <asp:Parameter Name="iAllotment" Type="Int32" />
        <asp:Parameter Name="mFlatShip" Type="Decimal" />
        <asp:Parameter Name="vcFlatMethod" Type="String" />
        <asp:Parameter Name="bShipSeparate" Type="boolean" />
        <asp:Parameter Name="dtBackorder" Type="dateTime" ConvertEmptyStringToNull="true" />
        <asp:Parameter Name="Id" Type="Int32" />
    </UpdateParameters>
    <SelectParameters>
        <asp:ControlParameter ControlID="GridViewEntity" Name="Idx" PropertyName="SelectedValue" Type="Int32" />
    </SelectParameters>
</asp:SqlDataSource>
<asp:SqlDataSource ID="SqlLottery" runat="server" ConnectionString="<%$ ConnectionStrings:WillCallConnectionString %>" 
    SelectCommand="
        SELECT ent.[Id], ent.[TShowTicketId], ent.[bActiveSignup], ent.[dtSignupStart], ent.[dtSignupEnd], ent.[Name], ent.[Description], ent.[Writeup], 
        ent.[DisplayText], ent.[bActiveFulfillment], ent.[dtFulfillStart],  ent.[dtFulfillEnd], ent.[iEstablishQty] , COUNT(lr.Id) as 'ChildCount'
        FROM [Lottery] ent LEFT OUTER JOIN [LotteryRequest] lr ON lr.[TLotteryId] = ent.[Id] WHERE ent.[TShowTicketId] = @Idx 
        GROUP BY ent.[Id], ent.[TShowTicketId], ent.[bActiveSignup], ent.[dtSignupStart], ent.[dtSignupEnd], ent.[Name], ent.[Description], ent.[Writeup], 
        ent.[DisplayText], ent.[bActiveFulfillment], ent.[dtFulfillStart],  ent.[dtFulfillEnd], ent.[iEstablishQty]"
    UpdateCommand=" 
        UPDATE [Lottery] SET [bActiveSignup] = @bActiveSignup, [dtSignupStart] = @dtSignupStart, [dtSignupEnd] = @dtSignupEnd, [Name] = @Name, 
        [Description] = @Description, [Writeup] = @Writeup, [DisplayText] = @DisplayText, [bActiveFulfillment] = @bActiveFulfillment, 
        [dtFulfillStart] = @dtFulfillStart, [dtFulfillStart] = @dtFulfillStart, [dtFulfillEnd] = @dtFulfillEnd, [iEstablishQty] = @iEstablishQty 
        FROM Lottery l WHERE l.[TShowTicketId] = @Idx"
     >
    <UpdateParameters>
        <asp:Parameter Name="bActiveSignup" Type="String" ConvertEmptyStringToNull="false" />
        <asp:Parameter Name="dtSignupStart" Type="String" ConvertEmptyStringToNull="false" />
        <asp:Parameter Name="dtSignupEnd" Type="Int32" />
        <asp:Parameter Name="Name" Type="Int32" />
        <asp:Parameter Name="Description" Type="boolean" />
        <asp:Parameter Name="Writeup" Type="boolean" />
        <asp:Parameter Name="DisplayText" Type="boolean" />
        <asp:Parameter Name="bActiveFulfillment" Type="String" ConvertEmptyStringToNull="false" />
        <asp:Parameter Name="dtFulfillStart" Type="String" ConvertEmptyStringToNull="false" />
        <asp:Parameter Name="dtFulfillStart" Type="Decimal" />
        <asp:Parameter Name="dtFulfillEnd" Type="String" ConvertEmptyStringToNull="false" />
        <asp:Parameter Name="iEstablishQty" Type="Decimal" DefaultValue="0" />        
        <asp:ControlParameter ControlID="FormView1" Name="Idx" PropertyName="SelectedValue" Type="Int32" />
    </UpdateParameters>
    <SelectParameters>
        <asp:ControlParameter ControlID="FormView1" Name="Idx" PropertyName="SelectedValue" Type="Int32" />
    </SelectParameters>
</asp:SqlDataSource>