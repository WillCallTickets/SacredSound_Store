<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ShowDate_Details.ascx.cs" Inherits="WillCallWeb.Admin.AdminControls.ShowDate_Details" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register src="~/Components/Util/CalendarClock.ascx" tagname="CalendarClock" tagprefix="uc1" %>
<div id="srceditor">
    <div id="showdatedetails">
        <div class="jqhead rounded">
            <h3 class="entry-title">Date Details - <asp:Literal ID="litShowTitle" runat="server" /></h3>
            <asp:GridView ID="GridView1" Width="100%" runat="server" AutoGenerateColumns="False" DataKeyNames="Id" ShowFooter="False"
                cssclass="lsttbl" 
                OnDataBinding="GridView1_DataBinding" 
                OnDataBound="GridView1_DataBound" 
                OnRowDataBound="GridView1_RowDataBound"
                OnRowCommand="GridView1_RowCommand" 
                OnSelectedIndexChanged="GridView1_SelectedIndexChanged" 
                OnRowDeleting="GridView1_RowDeleting" >
                <SelectedRowStyle CssClass="selected" />
               <Columns>
                    <asp:TemplateField ItemStyle-Wrap="false" HeaderText="DATES">
                        <ItemTemplate>
                            <asp:CustomValidator ID="CustomValidation" runat="server" CssClass="invisible" 
                                Display="Static" ErrorMessage="bad mojo" ValidationGroup="showdate">*</asp:CustomValidator>
                            <asp:LinkButton Width="20px" Id="btnSelect" CausesValidation="false" ToolTip="Select" CssClass="btnselect" runat="server" CommandName="Select" 
                                CommandArgument='<%#Eval("Id") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Center" HeaderText="Date Of Show - Doors">
                        <ItemTemplate>
                            <%#Eval("DateOfShow", "{0:MM/dd/yyyy hh:mmtt}") %>                              
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:CheckBoxField DataField="IsActive" ReadOnly="true" HeaderText="Active" ItemStyle-HorizontalAlign="Center" />
                    <asp:CheckBoxField DataField="IsLateNightShow" ReadOnly="true" HeaderText="Late" ItemStyle-HorizontalAlign="Center" />
                    <asp:CheckBoxField DataField="IsPrivateShow" ReadOnly="true" HeaderText="Private" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="ShowTime" HeaderText="Show Time" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="AgesString" HeaderText="Ages" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="StatusName" HeaderText="Status" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="PricingText" HeaderText="Pricing" ItemStyle-Width="50%" HeaderStyle-HorizontalAlign="left" />
                    <asp:BoundField DataField="StatusText" HeaderText="Status" ItemStyle-Width="50%" HeaderStyle-HorizontalAlign="left" />
                    <asp:TemplateField ItemStyle-Wrap="false" >
                       <ItemTemplate>
                           <asp:LinkButton Width="20px" Id="btnDelete" CssClass="btndelete lastinrow" runat="server" CommandName="Delete" 
                                CommandArgument='<%#Eval("Id") %>' ToolTip="Delete" CausesValidation="false"
                               OnClientClick='return confirm("Are you sure you want to delete this row?")' />
                       </ItemTemplate>
                   </asp:TemplateField>
                 </Columns>
            </asp:GridView>
            <asp:ValidationSummary ID="ValidationSummary2" CssClass="validationsummary" ValidationGroup="showdate" runat="server" />

            <div class="jqpnl rounded eit">
                <asp:FormView Width="100%" ID="FormView1" runat="server" DefaultMode="Edit" 
                    OnDataBinding="FormView1_DataBinding" 
                    OnDataBound="FormView1_DataBound" 
                    OnItemCommand="FormView1_ItemCommand" 
                    OnItemInserting="FormView1_ItemInserting" 
                    OnItemUpdating="FormView1_ItemUpdating"
                    OnModeChanging="FormView1_ModeChanging" >    
                    <EditItemTemplate>
                    <div class="cmdsection">
                        <asp:Button ID="btnSave" CausesValidation="false" runat="server" CommandName="Update" 
                            Text="Save" CssClass="btnmed" />
                        <asp:Button ID="btnCancel" runat="server" CommandName="Cancel" Text="Cancel" CssClass="btnmed" />
                        <asp:Button ID="btnNew" runat="server" CommandName="New" Text="New" CssClass="btnmed" />
                        <asp:CustomValidator ID="CustomValidation" runat="server" CssClass="invisible" 
                            Display="Static" ErrorMessage="bad mojo" ValidationGroup="showdate">*</asp:CustomValidator>
                        <asp:Button ID="btnSales" runat="server" CommandName="viewsales" CommandArgument='<%#Eval("Id") %>' CausesValidation="false" 
                            Text="View Sales" CssClass="btnmed" />
                        <asp:Button ID="btnChangeShowName" runat="server" Text="Sync Show Name" CssClass="btnmed" 
                            OnClick="btnChangeShowName_Click"
                            OnClientClick="return confirm('This will update the show name to reflect the current information. Are you sure you want to continue?');" />
                        <asp:Button ID="btnDisplay" runat="server" CausesValidation="false" CommandName="view" Text="Display Show" 
                            CssClass="btnmed" OnClientClick='doPagePopup("/Admin/ShowDisplayer.aspx", "false"); ' />
                    </div>
                    <table border="0" cellspacing="0" cellpadding="0" width="100%" class="edittabl">
                        <tr>
                            <th><span class="intr">{ShowId <%#Eval("TShowId") %>} <%#Eval("Id") %></span> Date</th>
                            <td><%#Eval("DateOfShow", "{0:MM/dd/yyyy hh:mmtt}") %></td>
                        </tr>
                        <tr>
                            <th>&nbsp;</th>
                            <td class="listing-row" style="width:100%;">
                                <a style="vertical-align:4px;" href="javascript: alert('Setting to not active will remove this DATE from the user listings.')" class="infomark">?</a>
                                Active
                                <asp:CheckBox id="chkActive" runat="server" checked='<%#Eval("IsActive")%>' />
                                <a href="javascript: alert('Essentially adds 24 hours to the show time to make shows at midnight or later order correctly. This is a matter of preference as some people like to have the date of midnight be from the previous day.')" class="infomark">?</a>
                                Late Show
                                <asp:CheckBox id="chkLate" runat="server" checked='<%#Eval("IsLateNightShow")%>' />
                                <a style="vertical-align:4px;" href="javascript: alert('Remove from the user listings but allow access - just removes from side listing and main listing. Anyone can access - just not blatantly advertised. Use codes for tickets to restrict access to sales..')" class="infomark">?</a>
                                Private 
                                <asp:CheckBox ID="chkPrivate" runat="server" Checked='<%#Eval("IsPrivateShow") %>' />
                                CoHead
                                <asp:checkbox ID="chkCoHead" Enabled="false" runat="server" Checked='<%#Eval("CoHeadline") %>' />
                            </td>
                        </tr>
                        <tr>
                            <th>Title</th>
                            <td><asp:TextBox ID="txtTitle" MaxLength="500" Width="650px" runat="server" Text='<%#Eval("ShowDateTitle") %>' /></td>
                        </tr>
                        <tr>
                            <th>Door Time</th>
                            <td>
                                <asp:TextBox ID="txtDoorTime" Width="200px" runat="server"></asp:TextBox>
                                <cc1:MaskedEditExtender ID="MaskedEditExtender1" runat="server" TargetControlID="txtDoorTime" Mask="99:99" MaskType="Time" 
                                    AcceptAMPM="true" MessageValidatorTip="true" OnFocusCssClass="maskededitfocus" OnInvalidCssClass="maskedediterror" />
                                <cc1:MaskedEditValidator ID="MaskedEditValidator1" runat="server" ControlToValidate="txtDoorTime" ControlExtender="MaskedEditExtender1" 
                                    display="Dynamic" Text="*" ToolTip="Please enter a door time" InvalidValueMessage="This door time is invalid." ValidationGroup="showdate" />
                            </td>
                        </tr>
                        <tr>
                            <th>Show Time</th>
                            <td>
                                <asp:TextBox ID="txtShowTime" Width="200px" MaxLength="50" runat="server" Text='<%#Bind("ShowTime") %>'></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>Ages</th>
                            <td><asp:DropDownList ID="ddlAges" Width="350px" runat="server" /></td>
                        </tr>
                        <tr>
                            <th>Status</th>
                            <td><asp:DropDownList ID="ddlStatus" Width="350px" runat="server" /></td>
                        </tr>
                        <tr>
                            <th><a href="javascript: alert('Enter a custom value here if you need to override the auto-generated value for menu listings. It will not change the display of the show info panel on the ChooseTicket page.')" class="infomark">?</a>
                                Billing</th>
                            <td>
                                <asp:TextBox ID="txtBilling" MaxLength="300" Width="650px" runat="server" Text='<%#Eval("Billing") %>' />
                            </td>
                        </tr>
                        <tr>
                            <th>Pricing</th>
                            <td><asp:TextBox ID="txtPricing" MaxLength="500" Width="650px" runat="server" Text='<%#Eval("PricingText") %>' /></td>
                        </tr>
                        <tr>
                            <th>Status</th>
                            <td><asp:TextBox ID="txtStatus" MaxLength="500" Width="650px" runat="server" Text='<%#Eval("StatusText") %>' /></td>
                        </tr>
                        <tr>
                            <th style="vertical-align:top;">Display Notes</th>
                            <td>
                                <asp:TextBox ID="txtNotes" MaxLength="1000" Width="650px" runat="server" Text='<%#Eval("DisplayNotes") %>' /></td>
                        </tr>
                        <tr>
                            <th>
                                <a href="javascript: alert('Settings for displaying a Facebook RSVP link. Leave off the http:// portion')" class="infomark">?</a>
                                Use FB Rsvp
                            </th>
                            <td>
                                <span class="intr">
                                    <ul>
                                        <li>Do not include http:// or https://</li>
                                        <li>Do not show for heavy traffic ticket onsales!</li>
                                    </ul>
                                </span>
                                <asp:CheckBox id="chkRsvp" runat="server" checked='<%#Eval("UseFbRsvp")%>' />
                                <asp:TextBox ID="txtRsvp" MaxLength="256" Width="320px" runat="server" Text='<%#Eval("FbRsvpUrl") %>' />
                                <asp:HyperLink ID="lnkRsvp" runat="server" Target="_blank" Text="test" />
                            </td>
                        </tr>
                    </table>
                    <div class="cmdsection">
                        <asp:Button ID="Button1" CausesValidation="false" runat="server" CommandName="Update" 
                            Text="Save" CssClass="btnmed" />
                        <asp:Button ID="Button2" runat="server" CommandName="Cancel" Text="Cancel" CssClass="btnmed" />
                        <asp:Button ID="Button3" runat="server" CommandName="New" Text="New" CssClass="btnmed" />
                    </div>
                </EditItemTemplate>
                <InsertItemTemplate>
                    <div class="vit">
                    <h3 class="entry-title">Adding A Show Date...</h3>
                    
                    <table border="0" cellspacing="0" cellpadding="0" width="100%" class="edittabl">
                        <tr>
                            <th>
                                <asp:RequiredFieldValidator ID="ctmFirst" runat="server" Display="Static" CssClass="validator" ValidationGroup="showdate" 
                                     ControlToValidate="clockStartDate" ErrorMessage="Please select the start date.">*</asp:RequiredFieldValidator>
                            </th>
                            <th style="vertical-align:middle;">Date &amp; Doors</th>
                            <td style="width:100%;">
                                <uc1:CalendarClock ID="clockStartDate" runat="server" UseTime="true" UseReset="false" ValidationGroup="showdate" />
                            </td>
                        </tr>
                        <tr>
                            <th colspan="2" style="vertical-align:top;">Copy?</th>
                            <td>
                                <div><asp:CheckBox ID="chkCopyFirst" runat="server" TextAlign="Right" /></div>
                                <asp:CheckBox ID="chkCopyPrevious" runat="server" TextAlign="Right" />
                                <div class="intr">Package tickets cannot be copied</div>
                            </td>
                        </tr>
                        <tr><td colspan="4"><br /><hr /><br /></td></tr>
                        <tr>
                            <th colspan="2">Show Time</th>
                            <td>
                                <asp:TextBox ID="txtShowTime" Width="60px" runat="server"></asp:TextBox>
                                <asp:DropDownList ID="ddlShowTimeAmPm" runat="server">
                                    <asp:ListItem Selected="True" Text="PM" Value="PM" />
                                    <asp:ListItem Text="AM" Value="AM" />
                                </asp:DropDownList>                                
                            </td>
                        </tr>
                        <tr>
                            <th colspan="2"><a href="javascript: alert('Essentially adds 24 hours to the show time to make shows at midnight or later order correctly. This is a matter of preference as some people like to have the date of midnight be from the previous day.')" class="infomark">?</a>
                                Late Show</th>
                            <td>
                                <asp:CheckBox id="chkLate" Font-Bold="true" runat="server" /> <span class="intr">Late Show will not copy</span>
                            </td>
                            
                        </tr>
                        <tr>
                            <th colspan="2">Ages</th>
                            <td><asp:DropDownList ID="ddlAges" runat="server" Width="350px" /></td>
                        </tr>
                        <tr>
                            <th colspan="2">Status</th>
                            <td><asp:DropDownList ID="ddlStatus" runat="server" Width="350px" /> <span class="intr">Status will not copy</span></td>
                        </tr>
                    </table>
                    <div class="cmdsection">
                        <asp:Button ID="btnInsert" CausesValidation="true" ValidationGroup="showdate" runat="server" CommandName="Insert" 
                            Text="Save" CssClass="btnmed" />
                        <asp:Button ID="btnCancel" runat="server" CommandName="Cancel" Text="Cancel" CausesValidation="false" 
                            CssClass="btnmed" />
                    </div>
                    </div>
                </InsertItemTemplate>
                </asp:FormView>
            </div>
        </div>
    </div>
</div>