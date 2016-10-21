<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CampaignList.ascx.cs" Inherits="WillCallWeb.Admin.AdminControls.ProductAccessor.CampaignList" EnableViewState="true" %>
<%@ Register src="~/Components/Util/CalendarClock.ascx" tagname="CalendarClock" tagprefix="uc1" %>
<%@ Register src="~/Admin/AdminControls/Choosers/Chooser_Merch.ascx" tagname="Chooser_Merch" tagprefix="uc3" %>
<%@ Register src="~/Admin/AdminControls/Choosers/Chooser_Ticket.ascx" tagname="Chooser_Ticket" tagprefix="uc4" %>
<div id="srceditor">
    <div id="campaignlist">
        <div class="jqhead rounded">

            <h3 class="entry-title">Campaign Access Listing 
                <span style="margin:0 64px;"><asp:Button ID="btnUserList" runat="server" Text="Edit Users" OnClick="btnUserList_Click" CssClass="btnmed" Width="100px" /></span>
                <span style="margin:0 64px;"><asp:Button ID="btnCampaignMailer" runat="server" Text="Edit Mailers" OnClick="btnCampaignMailer_Click" CssClass="btnmed" Width="100px" /></span>
                <span><asp:Button ID="btnPublish" runat="server" Text="Publish ProductAccess" OnClick="btnPublish_Click" CssClass="btnpub"
                    OnClientCLick="return confirm('This will ONLY publish the ProductAccess objects. It will not affect Shows, Merchandise, etc.');" /></span>
            </h3>

            <asp:GridView Width="100%" ID="GridListing" runat="server" GridLines="None" 
                AutoGenerateColumns="False" CssClass="lsttbl" BorderStyle="none"
                DataKeyNames="Id"
                OnInit="GridListing_Init"
                OnDataBinding="GridListing_DataBinding"
                OnRowDataBound="GridListing_RowDataBound"
                OnDataBound="GridListing_DataBound"
                
                OnSelectedIndexChanged="GridListing_SelectedIndexChanged"
                OnRowCommand="GridListing_RowCommand"
                OnRowDeleting="GridListing_RowDeleting"
                OnRowDeleted="GridListing_RowDeleted"

                >                            
                <SelectedRowStyle cssclass="selected" />
                <Columns>
                    <asp:ButtonField ButtonType="Button" DataTextField="Id" ControlStyle-CssClass="btntny" ItemStyle-HorizontalAlign="Center" CommandName="Select" />
                    <asp:CheckBoxField DataField="bActive" ReadOnly="true" HeaderText="Active" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="CampaignName" ReadOnly="true" HeaderText="Name" ItemStyle-Width="50%" HeaderStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="CampaignCode" ReadOnly="true" HeaderText="CampaignCode" HeaderStyle-HorizontalAlign="Left" />
                    <asp:TemplateField HeaderText="PublicStart" ItemStyle-HorizontalAlign="Center" >
                        <ItemTemplate>
                            <asp:Literal ID="litPublicStart" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="PublicEnd" ItemStyle-HorizontalAlign="Center" >
                        <ItemTemplate>
                            <asp:Literal ID="litPublicEnd" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-Wrap="false">
                        <ItemTemplate>
                            <asp:LinkButton Width="20px" Id="btnUp" ToolTip="Move Up" Cssclass="btn-up" runat="server" CommandName="Up" 
                                CommandArgument='<%#Eval("Id") %>' CausesValidation="false" />
                            <asp:LinkButton Width="20px" Id="btnDown" ToolTip="Move Down" Cssclass="btn-down" runat="server" CommandName="Down" 
                                CommandArgument='<%#Eval("Id") %>' CausesValidation="false" />
                            <asp:LinkButton Width="20px" Id="btnDelete" Cssclass="btn-delete" runat="server" CommandName="Delete" 
                                CommandArgument='<%#Eval("Id") %>' ToolTip="Delete" CausesValidation="false"
                                OnClientClick='return confirm("Are you sure you want to delete this row?")' />                      
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
           </div>
            <asp:ValidationSummary ID="ValidationSummary2" CssClass="validationsummary" Width="70%" HeaderText="" ValidationGroup="entity" runat="server" />
            
            <asp:FormView Width="100%" ID="FormEditor" runat="server" DefaultMode="Edit" EnableViewState="true" 
                DataKeyNames="Id" 
                OnDataBinding="FormEditor_DataBinding"
                OnDataBound="FormEditor_DataBound"
                OnItemUpdating="FormEditor_ItemUpdating"   
                OnItemCommand="FormEditor_ItemCommand"
                OnItemInserting="FormEditor_ItemInserting"
                OnModeChanging="FormEditor_ModeChanging"
                >
                <EmptyDataTemplate>
                    <div class="jqpnl rounded iit">
                        <div class="cmdsection">
                            There are no currently no campaigns <span style="padding-left:24px;"><asp:Button Id="btnNew" CausesValidation="false" runat="server" CommandName="New" 
                                Text="CREATE NEW CAMPAIGN" CssClass="btnmed" Width="150px" /></span>
                        </div>
                    </div>
                </EmptyDataTemplate>  
                <InsertItemTemplate>
                    <div class="jqpnl rounded eit">
                        <h3 class="entry-title">Adding a new Access Campaign...</h3><br />                        
                        <table border="0" cellspacing="0" cellpadding="0" width="100%" class="edittabl">
                            <tr>
                                <th><a href="javascript: alert('A friendly name for the campaign.')" class="infomark">?</a>Campaign Name</th>
                                <td style="width:100%;">
                                    <asp:TextBox ID="txtName" MaxLength="512" Width="350px" runat="server" Text='<%#Bind("CampaignName") %>' />
                                </td>
                            </tr>
                        </table>
                        <div class="cmdsection">
                            <asp:Button ID="btnSave" CausesValidation="true" ValidationGroup="entity" runat="server" CommandName="Insert" 
                                Text="Save" CssClass="btnmed" />
                            <asp:Button ID="btnCancel" runat="server" CommandName="Cancel" CausesValidation="false" 
                                Text="Cancel" CssClass="btnmed" />
                            <asp:CustomValidator ID="CustomValidation" runat="server" ValidationGroup="entity" CssClass="validator" 
                                 Display="Static" >*</asp:CustomValidator>
                        </div>
                    </div>
                </InsertItemTemplate>  
                <EditItemTemplate>
                    <div class="jqpnl rounded eit">
                        <div class="cmdsection" style="margin-bottom:12px;">
                            <asp:Button ID="btnSave" CausesValidation="true" ValidationGroup="entity" runat="server" CommandName="Update" 
                                Text="Save" CssClass="btnmed" />
                            <asp:Button ID="btnCancel" runat="server" CommandName="Cancel" CausesValidation="false" 
                                Text="Cancel" CssClass="btnmed" />
                            <asp:Button ID="btnNew" runat="server" CommandName="New" CausesValidation="false" 
                                Text="New" CssClass="btnmed" />
                            <asp:CustomValidator ID="CustomValidation" runat="server" ValidationGroup="entity" CssClass="validator" 
                                 Display="Static" >*</asp:CustomValidator>
                        </div>
                        <table border="0" cellspacing="0" cellpadding="0" width="100%" class="edittabl">
                            <tr>
                                <th><span class="intr"><%#Eval("Id") %></span> Active</th>
                                <td style="width:100%;" class="listing-row">
                                    <asp:CheckBox ID="chkActive" runat="server" Checked='<%#Bind("bActive") %>' />
                                </td>
                            </tr>
                            <tr>
                                <th><a href="javascript: alert('A friendly name for the campaign.')" class="infomark">?</a>Campaign Name</th>
                                <td>
                                    <asp:TextBox ID="txtName" MaxLength="512" Width="350px" runat="server" Text='<%#Bind("CampaignName") %>' />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtName" Display="Static"
                                        SetFocusOnError="true" ErrorMessage="The NAME field is required" ValidationGroup="entity" CssClass="val-indicator">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <th valign="top">
                                    <a href="javascript: alert('A code for the campaign.')" class="infomark">?</a>Campaign Code</th>
                                <td>
                                    <asp:TextBox ID="txtCode" MaxLength="50" Width="350px" runat="server" ReadOnly="true" EnableViewState="false" Text='<%#Bind("CampaignCode") %>' />
                                    <asp:Button ID="btnGenerateCampaignCode" runat="server" CommandName="resetcampaigncode" CssClass="btnmed" Text="Reset Code" Width="100px" />
                                </td>
                            </tr>                        
                        </table>

                        <hr />

                        <asp:GridView Width="100%" ID="GridSelections" runat="server" 
                            DataKeyNames="Id"
                            AutoGenerateColumns="False" CssClass="lsttbl" BorderStyle="none" GridLines="None" 
                            OnDataBinding="GridSelections_DataBinding" 
                            OnRowDataBound="GridSelections_RowDataBound"
                            OnRowDeleting="GridSelections_RowDeleting"
                            >                            
                            <SelectedRowStyle cssclass="selected" />
                            <EmptyDataTemplate>
                                <h4>No products have been selected</h4>
                            </EmptyDataTemplate>
                            <Columns>
                                <asp:BoundField DataField="Id" ReadOnly="true" HeaderText="Id" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="vcContext" ReadOnly="true" HeaderText="Context" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="TParentId" ReadOnly="true" HeaderText="Product Id" HeaderStyle-HorizontalAlign="Left" />
                                <asp:TemplateField ItemStyle-Wrap="true" HeaderText="Description" HeaderStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Literal ID="litDescription" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Wrap="false">
                                    <ItemTemplate>                                    
                                        <asp:LinkButton Width="20px" Id="btnDelete" Cssclass="btn-delete" runat="server" CommandName="Delete" 
                                            Text="Delete" CommandArgument='<%#Eval("Id") %>' ToolTip="Delete" CausesValidation="false"
                                            OnClientClick='return confirm("Are you sure you want to delete this row?")' />                      
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>

                        <table border="0" cellspacing="0" cellpadding="0" width="612px" class="edittabl">
                            <tr>
                                <td style="text-align:center;">
                                    <uc4:Chooser_Ticket ID="Chooser_Ticket1" runat="server" />
                                    <asp:Button ID="btnAddTicket" runat="server" CssClass="btnmed" Width="150px" CommandName="AddTicket" Text="Add Selected Ticket" />
                                </td>
                                <td style="text-align:center;">
                                    <uc3:Chooser_Merch ID="Chooser_Merch1" runat="server" />
                                    <asp:Button ID="btnAddMerch" runat="server" CssClass="btnmed" Width="150px" CommandName="AddMerch" Text="Add Selected Merchandise" />
                                </td>
                            </tr>
                        </table>
                        <hr />
                        
                        <div class="jqhead rounded eit" style="margin:12px;">
                            <asp:Panel ID="pnlAddActivationWindow" runat="server" >
                                <div class="cmdsection" style="margin-top:12px;padding-left:32px;padding-bottom:12px;">
                                    <asp:Button ID="btnAddActivation" runat="server" CommandName="addactivation" CssClass="btnmed" Text="Add Activation Window" Width="150px" />
                                </div>
                            </asp:Panel>
                            <asp:Panel ID="pnlEditActivationWindow" runat="server" >
                                <table border="0" cellspacing="0" cellpadding="0" width="100%" class="edittabl">
                                    <tr>
                                        <td colspan="2" style="padding-left:32px;padding-bottom:12px;">
                                            <asp:Button ID="btnDeleteActivation" runat="server" CommandName="deleteactivation" CssClass="btnmed" Text="Delete Activation Window" Width="150px" 
                                                OnClientClick='return confirm("Are you sure you want to delete this activation window?")' />
                                            <span class="intr"><asp:Literal ID="litId" runat="server" EnableViewState="false" /></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th style="padding-top:10px;"><a href="javascript: alert('The date when the item becomes active.')" class="infomark">?</a>Public Start</th>
                                        <td style="width:100%;">
                                             <uc1:CalendarClock ID="clockPublicStart" runat="server" UseTime="true" OnInit="clock_Init" DefaultValue='<%#Utils.Constants._MinDate %>' UseReset="true" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <th style="padding-top:10px;"><a href="javascript: alert('The date when the item expires.')" class="infomark">?</a>Public End</th>
                                        <td>
                                             <uc1:CalendarClock ID="clockPublicEnd" runat="server" UseTime="true" OnInit="clock_Init" DefaultValue='<%#DateTime.MaxValue %>' UseReset="true" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </div>
                    </div>
                </EditItemTemplate>
            </asp:FormView>
    </div>
</div>