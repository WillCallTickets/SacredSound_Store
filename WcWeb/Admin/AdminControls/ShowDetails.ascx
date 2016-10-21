<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ShowDetails.ascx.cs" Inherits="WillCallWeb.Admin.AdminControls.ShowDetails" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register src="~/Components/Util/CalendarClock.ascx" tagname="CalendarClock" tagprefix="uc1" %>

<div id="srceditor">
    <div id="showdetails">
        <div class="jqhead rounded">
            <h3 class="entry-title"><asp:Literal ID="litShowTitle" runat="server" /></h3>
            <div class="cmdsection">
                <asp:Button ID="btnUpdate" runat="server" CausesValidation="false" CommandName="Update" Text="Save" 
                    OnClick="btnUpdate_Click" CssClass="btnmed" />
                <asp:Button ID="btnCancel" runat="server" CausesValidation="False" CssClass="btnmed" 
                    CommandName="Cancel" Text="Cancel" OnClick="btnCancel_Click" />
                <asp:Button ID="btnDelete" runat="server" CausesValidation="False" CssClass="btnmed" 
                    CommandName="Delete" Text="Delete" OnClick="btnDelete_Click" />
                <asp:CustomValidator ID="CustomValidation" runat="server" CssClass="invisible" 
                    Display="Static" ErrorMessage="bad mojo" ValidationGroup="showdate">*</asp:CustomValidator>
                <asp:Button ID="btnSales" runat="server" CommandName="viewsales" CommandArgument="0" CausesValidation="false" 
                    Text="View Sales" CssClass="btnmed" OnClick="btnSales_Click" />
                <asp:Button ID="btnChangeShowName" runat="server" Text="Sync Show Name" CssClass="btnmed" 
                    OnClick="btnChangeShowName_Click"
                    OnClientClick="return confirm('This will update the show name to reflect the current information. Are you sure you want to continue?');" />
                <asp:Button ID="btnDisplay" runat="server" CausesValidation="false" CommandName="view" Text="Display Show" 
                    CssClass="btnmed" OnClientClick='doPagePopup("/Admin/ShowDisplayer.aspx", "false"); ' />
            </div>
            <asp:ValidationSummary ID="ValidationSummary1" CssClass="validationsummary" HeaderText="" ValidationGroup="showdate" runat="server" />
            <asp:FormView ID="FormView1" Width="100%" runat="server" DataKeyNames="Id" DefaultMode="Edit"
                OnDataBinding="FormView1_DataBinding" OnItemUpdating="FormView1_ItemUpdating" 
                OnItemDeleting="FormView1_ItemDeleting"  
                OnModeChanging="FormView1_ModeChanging" OnDataBound="FormView1_DataBound" 
                onitemcreated="FormView1_ItemCreated">
                <EditItemTemplate>
                    <div class="jqpnl rounded eit">
                        <table border="0" cellspacing="0" cellpadding="0" width="100%" class="edittabl">
                            <tr>
                                <th>Direct Link</th>
                                <td colspan="2"><input type="text" readonly="readonly" size="100%" value='<%# Eval("ShowUrl") %>' /></td>
                            </tr>
                            <tr>
                                <th>
                                    <span class="intr"><%#Eval("Id") %></span> Show Details
                                </th>
                                <td class="listing-row" colspan="2">
                                    <a style="vertical-align:4px;" href="javascript: alert('Setting to not active will remove this SHOW from the user listings.')" class="infomark">?</a>
                                    Active
                                    <asp:CheckBox id="chkActive" runat="server" checked='<%#Eval("IsActive")%>' />
                                    SoldOut
                                    <asp:CheckBox id="chkSoldOut" runat="server" checked='<%#Eval("IsSoldOut")%>' />
                                    UseFacebook
                                    <asp:CheckBox ID="chkFacebookLike" runat="server" Checked='<%#Bind("IsAllowFacebookLike") %>' />
                                    <asp:Literal ID="litFB" runat="server" OnDataBinding="litFB_DataBinding" />
                                </td>
                            </tr>
                            <tr>
                                <th><a href="javascript: alert('This will display a message regarding the overall status of the show.')" class="infomark">?</a>
                                    Status</th>
                                <td style="padding-right:16px;"><asp:TextBox ID="txtStatus" MaxLength="500" Width="360px" runat="server" Text='<%#Eval("StatusText") %>' /></td>                            
                                <td rowspan="8" valign="top" style="vertical-align:top !important; width:100%;">
                                    <div class="jqinstruction rounded">
                                        <h2>Show Image</h3>
                                        <asp:Literal ID="litImage" runat="server" OnDataBinding="litImage_DataBinding" />
                                        <ul>
                                            <li>Be sure to save after editing the image</li>
                                            <li>This will automatically default to the headliner image. If none is present, it will display the Venue image. If that does not exist, it will show a blank image.</li>
                                            <li>You may also set another image here to override the act and venue images.</li>
                                            <li>Shows should rarely share an image - when they do - you will need to upload/delete each show image individually</li>
                                        </ul>
                                        <div style="margin:12px 0;">
                                            <asp:FileUpload ID="uplPicture" runat="server" Width="350px" CssClass="btnmed" />
                                        </div>
                                        <div>
                                            <asp:Button ID="btnUpload" runat="server" Text="Upload" OnClick="btnUpload_Click" 
                                                CssClass="btnmed btupload" CommandName="upload" CausesValidation="false" />
                                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancelUpload_Click" 
                                                CssClass="btnmed" CommandName="cancelupload" CausesValidation="false" />
                                            <asp:CustomValidator ID="CustomUpload" ValidationGroup="upload" runat="server" CssClass="validator" 
                                                Display="Static" >*</asp:CustomValidator>
                                            <asp:Button ID="btnClear" runat="server" CssClass="btnmed" Text="Clear Image" CausesValidation="false" 
                                                OnClientClick='return confirm("Are you sure you want to delete this image? This will only delete an image assigned to the show. It will not affect an image linked from an act.")' 
                                                OnClick="btnClear_Click" />
                                        </div>
                                        <asp:ValidationSummary ID="ValidationSummary1" CssClass="validationsummary" HeaderText="" 
                                            ValidationGroup="upload" runat="server" />
                                    </div>
                                </td>                       
                            </tr>
                            <tr>
                                <th><a href="javascript: alert('This will display a title for the show.')" class="infomark">?</a>
                                    Title</th>
                                <td><asp:TextBox ID="txtTitle" MaxLength="300" Width="360px" runat="server" Text='<%#Eval("ShowTitle") %>' /></td>                            
                            </tr>
                            <tr>
                                <th valign="top"><a href="javascript: alert('Text displayed just below the venue name.')" class="infomark">?</a> Header</th>
                                <td><asp:TextBox ID="txtTopText" MaxLength="300" Width="360px" TextMode="MultiLine" runat="server" Text='<%#Eval("TopText") %>' /></td>
                            </tr>
                            <tr>
                                <th><a href="javascript: alert('Text to be displayed prior to the venue name.')" class="infomark">?</a>
                                    Venue Pre</th>
                                <td>
                                    <asp:TextBox ID="txtVenuePreText" MaxLength="256" Width="270px" runat="server" Text='<%#Eval("VenuePreText") %>' />
                                    &nbsp;<asp:Button ID="btnVenueEditor" runat="server" Text="Venue Editor" cssclass="btntny" 
                                        causesValidation="false" OnClick="btnVenueEditor_Click" />
                                </td>
                            </tr>
                            <tr>
                                <th>Venue</th>
                                <td style="font-weight:bold;"><%#Eval("VenueRecord.Name_Displayable") %></td>                                
                            </tr>
                            <tr>
                                <th><a href="javascript: alert('Text to be displayed immediately after the venue name.')" class="infomark">?</a> Venue Post</th>
                                <td><asp:TextBox ID="txtVenuePostText" MaxLength="256" Width="360px" runat="server" Text='<%#Eval("VenuePostText") %>' /></td>                                
                            </tr>
                            <tr>
                                <th style="vertical-align:top;padding-top:10px;">
                                    <a href="javascript: alert('This show will not display until after the announce date.')" class="infomark">?</a> 
                                    Announce</th>
                                <td>
                                    <uc1:CalendarClock ID="clockAnnounce" runat="server" UseTime="true" UseReset="true"
                                        SelectedValue='<%#Eval("AnnounceDate") %>'  />
                                </td>
                            </tr>
                            <tr>
                                <th style="vertical-align:top;padding-top:10px;">
                                    <a href="javascript: alert('This show's tickets will not be available for sale until after the onsale date. Note that this cascades down to all tickets within a show group.')" class="infomark">?</a> 
                                    OnSale</th>
                                <td>                            
                                    <uc1:CalendarClock ID="clockOnsale" runat="server" UseTime="true" UseReset="true" 
                                         SelectedValue='<%#Eval("DateOnSale") %>'  />
                                    <div class="intr">
                                        Onsale changes will apply to all tickets within this show group
                                    </div>                             
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    <a href="javascript: alert('A short description to be displayed - shows on the SHOW LISTING PAGE ONLY and NOT THE MAIN INFORMATION PAGE. Think of this as a brief description for the write-up below.')" class="infomark">?</a> 
                                    Short Desc</th>
                                <td>
                                    <asp:TextBox ID="txtMidText" MaxLength="300" Width="360px" runat="server" Text='<%#Eval("MidText") %>' />
                                </td>
                                <th style="text-align:left;">                                    
                                    Internal Notes                                    
                                </th>
                            </tr>
                            <tr>
                                <th>
                                    <a href="javascript: alert('Displays extra information about the show. Use to describe ticket packages or any info pertaining to this show that is not urgent. Use status for urgent messages.')" class="infomark">?</a>
                                    Display Notes    
                                </th>
                                <td>
                                    <asp:TextBox ID="txtDisplayNotes" MaxLength="1000" Width="360px" TextMode="multiline" runat="server" Text='<%#Eval("DisplayNotes") %>' />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNotes" MaxLength="500" Width="360px" Height="100%" TextMode="MultiLine" runat="server" Text='<%#Eval("InternalNotes") %>' />
                                </td>
                            </tr>  

                            <!-- added external tix urls -->

                            <tr>
                                <th>
                                    <a href="javascript: alert('An external ticket url will override any STS9Store links and change the link text to TICKETS. That link will redirect to the url entered here. Please include and specify http or https.')" class="infomark">?</a> 
                                    External TixUrl</th>
                                <td colspan="2">
                                    <asp:TextBox ID="txtExternalTixUrl" MaxLength="500" Width="100%" runat="server" Text='<%#Eval("ExternalTixUrl") %>' />
                                    <div class="intr">
                                        <strong style="color:white;">please be sure to include the http:// or https:// with the link!!!</strong>
                                    </div>
                                </td>
                            </tr>

                                                 
                            <tr><td colspan="3"><hr /></td></tr>
                            <tr>
                                <th>Display Options</th>
                                <td colspan="2" class="listing-row">
                                    <a href="javascript: alert('Hides auto generated content for ticket listing and show links. Hides ticket and show links')" class="infomark">?</a>
                                    Hide Auto-Gen
                                    <asp:CheckBox ID="chkHideAuto" runat="server" Checked='<%#Bind("IsHideAutoGenerated") %>' />
                                    <a style="vertical-align:4px;" href="javascript: alert('Toggling this on will use the text from below -only- for the writeup. No acts will be auto shown. Ticket listings will still be displayed as well as show links.')" class="infomark">?</a> 
                                    RichText Only
                                    <asp:CheckBox ID="chkRichText" runat="server" Checked='<%#Bind("IsDisplayRichText") %>' />
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    <div style="margin-top:8px;">
                                        <asp:Button ID="btnWys" CausesValidation="false" runat="server" CommandName="Wys" Text="Edit" Width="100px" 
                                            CssClass="btnmed ov-trigger" Tooltip="/Admin/AdminControls/Wysiwyg/Wysiwyg.aspx?context=s&ctrl=" rel="#overlay-wysiwyg" />
                                        <br /><br />
                                        <asp:Button ID="btnDisplay" runat="server" CausesValidation="false" CommandName="view" Text="Display Item" Width="100px"
                                            CssClass="btnmed" OnClientClick='doPagePopup("/Admin/ShowDisplayer.aspx", "false"); ' />
                                    </div>
                                </th>
                                <td colspan="2" style="width:100%;">
                                    <asp:Literal ID="litDesc" runat="server" />
                                </td>
                            </tr>                        
                        </table>
                    </div>
                    <br />
                </EditItemTemplate>
            </asp:FormView>
        </div>
    </div>
</div>

<div class="admin_overlay" id="overlay-wysiwyg" style="display:none;">
	<div class="contentWrap"></div>
</div>

<script type="text/javascript" src="/JQueryUI/admin-overlay.js"></script>