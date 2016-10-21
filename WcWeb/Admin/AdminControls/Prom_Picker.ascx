<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Prom_Picker.ascx.cs" Inherits="WillCallWeb.Admin.AdminControls.Prom_Picker" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register src="~/Components/Util/CalendarClock.ascx" tagname="CalendarClock" tagprefix="uc1" %>
<%@ Register src="~/Components/Navigation/gglPager.ascx" tagname="gglPager" tagprefix="uc2" %>
<%@ Register src="~/Components/Util/MerchSelector.ascx" tagname="MerchSelector" tagprefix="uc1" %>
<div id="srceditor">
    <div id="promotion">
        <uc2:gglPager ID="GooglePager1" runat="server" PageButtonClass="btnmed" PagerTitle="Promotions" >
            <Template>
                <div class="cmdsection">
                    <asp:Button ID="btnBanner" runat="server" CssClass="btnmed" Text="Banner Editor" OnClick="btnBanner_Click" 
                        CausesValidation="false" />
                    <asp:CustomValidator ID="CustomValidation" runat="server" CssClass="invisible" Display="Static" ValidationGroup="promo"  
                        ErrorMessage="CustomValidator">*</asp:CustomValidator>
                </div>
            </Template>
        </uc2:gglPager>
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="validationsummary" 
             ValidationGroup="promo" HeaderText="" />
        <asp:GridView ID="GridView1" runat="server" DataSourceID="ObjBanners" Width="100%" CssClass="lsttbl" DataKeyNames="Id" 
            AllowPaging="True" AutoGenerateColumns="False" GridLines="Both"
            OnInit="GridView_Init"
            OnDataBinding="GridView_DataBinding" 
            OnRowDataBound="GridView_RowDataBound" 
            OnDataBound="GridView_DataBound">           
           <PagerSettings Visible="false" />
           <SelectedRowStyle CssClass="selected" />
           <EmptyDataTemplate>
                <div class="lstempty">No Promotions</div>
            </EmptyDataTemplate>
           <Columns>
               <asp:TemplateField HeaderText="#" ItemStyle-HorizontalAlign="center" ItemStyle-CssClass="rowcounter">
                    <ItemTemplate><asp:Literal ID="LiteralRowCounter" runat="server" /></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ItemStyle-Wrap="false" HeaderText="Select">
                    <ItemTemplate> 
                        <asp:LinkButton Width="20px" Id="btnSelect" ToolTip="Select" CssClass="btnselect" runat="server" CommandName="Select" 
                            CommandArgument='<%#Eval("Id") %>' CausesValidation="false" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:CheckBoxField ReadOnly="true" DataField="IsActive" HeaderText="Active" ItemStyle-HorizontalAlign="Center" />
                <asp:TemplateField HeaderText="Valid" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:CheckBox ID="chkValid" Enabled="false" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Context_Award" HeaderText="Context" ItemStyle-HorizontalAlign="Center" />
                <asp:TemplateField HeaderText="Description" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="100%">
                    <ItemTemplate>
                        <asp:Literal ID="litNaming" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Banner" HeaderStyle-HorizontalAlign="Left">
                    <ItemTemplate>
                        <asp:Literal ID="litImage" runat="server" />
                        <img src="/Images/spacer.gif" alt="" height="1px" width="190px" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Dates" HeaderStyle-HorizontalAlign="Left" ItemStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Literal ID="litDates" runat="server" />
                        <img src="/Images/spacer.gif" alt="" height="1px" width="100px" />
                    </ItemTemplate>
                </asp:TemplateField>
           </Columns>
       </asp:GridView>
        <asp:ValidationSummary ID="ValidationSummary2" CssClass="validationsummary" HeaderText="Please correct the following errors:" 
            ValidationGroup="promo"  runat="server" />
        <div class="jqpnl rounded">
            <div class="intr" style="color:#ffcc55;padding:8px;">
                <asp:Literal ID="litValid" runat="server" />
            </div>       
            <asp:Wizard ID="Wizard1" runat="server" CellPadding="0" CellSpacing="0" SkipLinkText="" Width="100%"
                OnSideBarButtonClick="OnNextStep" 
                OnDataBinding="wizard_DataBinding"
                OnActiveStepChanged="wizard_ActiveStepChanged" >
                <FinishNavigationTemplate>
                </FinishNavigationTemplate>
                <StartNavigationTemplate>
                </StartNavigationTemplate>
                <StepNavigationTemplate>
                </StepNavigationTemplate>
                <StepStyle HorizontalAlign="Left" Width="100%" />
                <SideBarStyle VerticalAlign="Top" />
                <SideBarTemplate>
                    <asp:DataList id="SideBarList" Runat="Server" RepeatLayout="Table" ItemStyle-HorizontalAlign="Center" 
                         OnItemDataBound="SideBarList_ItemDataBound" CssClass="sidbar" >
                        <SeparatorTemplate></SeparatorTemplate>
                        <SelectedItemStyle CssClass="contextselect" />
                        <ItemStyle HorizontalAlign="Center" CssClass="matchcontextdims" />
                        <ItemTemplate>
                            <asp:Button id="SideBarButton" CommandName="MoveTo" CausesValidation="false" CssClass="btnmed" Runat="Server"/>
                        </ItemTemplate>
                    </asp:DataList>
                </SideBarTemplate>
                <WizardSteps>
                    <asp:WizardStep ID="stepNaming" runat="server" title="Naming">
                        <asp:FormView ID="FormView_Naming" runat="server" DefaultMode="Edit" DataKeyNames="Id" Width="100%"
                            DataSourceID="SqlNaming" 
                            OnItemCreated="FormView_Naming_ItemCreated"
                            OnItemInserting="FormView_Naming_ItemInserting" 
                            OnItemInserted="FormView_Naming_ItemInserted"
                            OnItemUpdating="FormView_Naming_ItemUpdating"
                            OnItemUpdated="FormView_ItemUpdated"
                            OnItemCommand="FormView_Naming_ItemCommand" 
                            OnDataBound="FormView_Naming_DataBound"
                            OnModeChanging="FormView_Naming_ModeChanging">
                            <EmptyDataTemplate>
                                <div class="cmdsection">
                                    <asp:Button ID="btnNew" runat="server" CssClass="btnmed" Text="Add New Promo"  
                                        CommandName="New" CausesValidation="false" />
                                </div>
                            </EmptyDataTemplate>
                            <EditItemTemplate>
                                <table border="0" cellspacing="3" cellpadding="0" width="100%" class="edittabl">
                                    <tr>
                                        <th>Name</th>
                                        <td><asp:TextBox ID="txtName" runat="server" Text='<%#Bind("Name") %>' Width="350px" MaxLength="256" /></td>
                                    </tr>
                                    <tr>
                                        <th>Display Text</th>
                                        <td><asp:TextBox ID="txtDisplay" runat="server" Text='<%#Bind("DisplayText") %>' Width="100%" 
                                            MaxLength="1000" /></td>
                                    </tr>
                                    <tr><th style="vertical-align:top;">Additional Text</th>
                                        <td><asp:TextBox ID="txtAdditional" runat="server" Text='<%#Bind("AdditionalText") %>' 
                                                Width="100%" MaxLength="500" />
                                           <div class="intr">eg: Only one promotion per order</div>
                                        </td>
                                    </tr>
                                     <tr><th style="vertical-align:top;">Display At</th>
                                         <td style="padding:6px;">
                                            <asp:CheckBox ID="chkBannerTicket" runat="server" Checked='<%# Bind("bBannerTicket") %>' Text=" Ticketing/Home" TextAlign="Right" />
                                            <asp:CheckBox ID="chkDisplayParent" runat="server" Checked='<%# Bind("bDisplayAtParent") %>' Text=" Required Item" TextAlign="Right" />
                                            <asp:CheckBox ID="chkBannerMerch" runat="server" Checked='<%# Bind("bBannerMerch") %>' Text=" Merch" TextAlign="Right" />
                                            <br />
                                            <asp:CheckBox ID="chkDisplayCart" runat="server" Checked='<%# Bind("bBannerCartEdit") %>' Text=" CartEdit" TextAlign="Right" />
                                            <asp:CheckBox ID="chkDisplayCheckout" runat="server" Checked='<%# Bind("bBannerCheckout") %>' Text=" Checkout" TextAlign="Right" />
                                            <asp:CheckBox ID="chkDisplayShipping" runat="server" Checked='<%# Bind("bBannerShipping") %>' Text=" Shipping" TextAlign="Right" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <th><a href="javascript: alert('You may specify a url to link to when the banner is clicked.')" class="infomark">?</a>Url</th>
                                        <td><asp:TextBox ID="txtBannerClickUrl" runat="server" Text='<%#Bind("BannerClickUrl") %>' Width="350px" MaxLength="256" 
                                            OnTextChanged="txtBannerClick_TextChanged" />
                                            &nbsp;
                                            <asp:Literal ID="litUrlTest" runat="server" OnDataBinding="litUrlTest_DataBinding" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>Active &amp; Expiry</th>
                                        <td style="padding:6px;">
                                            <asp:CheckBox ID="chkActive" runat="server" Checked='<%# Bind("bActive") %>' Text=" Active" TextAlign="Right" />
                                            <asp:CheckBox ID="chkDeactivate" runat="server" Checked='<%# Bind("bDeactivateOnNoInventory") %>' 
                                                Text=" Auto Expire" TextAlign="Right" />
                                            <div class="intr">auto expire deactivates when promotional items are out of inventory</div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>Unlock Code</th>
                                        <td><asp:TextBox ID="txtUnlockCode" runat="server" Text='<%#Bind("UnlockCode") %>' Width="125px" MaxLength="256" />
                                            <asp:Button ID="btnGenerate" runat="server" CommandName="generatecode" Text="Create Code" 
                                                CssClass="btnmed" Width="80px" CommandArgument='<%#Eval("Id") %>' />
                                            <asp:Button ID="btnClearCode" runat="server" CommandName="clearcode" Text="Clear Code" 
                                                CssClass="btnmed" Width="80px" CommandArgument='<%#Eval("Id") %>' />
                                            <div class="intr" style="color: Red;">**note that this is for use in the querystring of a link - not for coupon-style entry</div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>Start Date</th>
                                        <td style="padding:0;">
                                            <uc1:CalendarClock ID="CalendarClockStart" SelectedDate='<%#Eval("dtStartDate") %>' IsRequired="false" 
                                                ValidationGroup="promo" UseTime="true" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>End Date</th>
                                        <td style="padding:0;">
                                            <uc1:CalendarClock ID="CalendarClockEnd" SelectedDate='<%#Eval("dtEndDate") %>' IsRequired="false"
                                                ValidationGroup="promo" UseTime="true" runat="server" />
                                        </td>
                                    </tr>   
                                    <tr>
                                        <th>Current Image</th>
                                        <td><%=Atx.CurrentSalePromotion.BannerUrl%>
                                            <asp:Button ID="btnDeleteBanner" runat="server" cssClass="btnmed" 
                                                CommandName="deletebanner" Text="Delete Image"
                                                OnClientClick="return confirm('Are you sure you want to delete this banner?') " />
                                            <div class="intr">
                                                <%=Wcss._Config._BannerDimensionText %>
                                                <asp:ValidationSummary ID="ValidationSummary2" CssClass="validationsummary" HeaderText="Please correct the following errors:" 
                                                    ValidationGroup="promoupload" runat="server" />
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>Image Upload</th>
                                        <td><asp:FileUpload ID="FileUpload1" runat="server" Width="350px" CssClass="btnmed" /><asp:Button ID="btnUpload" runat="server" CssClass="btnmed btnupload" Text="Upload" 
                                                CommandName="upload" CausesValidation="false" /></td>
                                    </tr>        
                                </table>
                                <div class="cmdsection">
                                    <asp:Button ID="btnSave" runat="server" CssClass="btnmed" Text="Save" 
                                        CommandName="Update" CausesValidation="false"  />
                                    <asp:Button ID="btnCancel" runat="server" CssClass="btnmed" Text="Cancel" 
                                        CommandName="Cancel" CausesValidation="false" />
                                    <asp:Button ID="btnNew" runat="server" CssClass="btnmed" Text="New Promo" 
                                        CommandName="New" CausesValidation="false" />
                                    <asp:CustomValidator ID="CustomFormValidation" runat="server" 
                                        CssClass="invisible" Display="Static" ValidationGroup="promoupload"  
                                         ErrorMessage="CustomValidator">*</asp:CustomValidator>
                                </div>
                            </EditItemTemplate>
                            <InsertItemTemplate>
                                <h3 class="entry-title" style="padding:8px;">Adding A New Promotion...</h3>
                                <table border="0" cellspacing="0" cellpadding="0" width="100%" class="edittabl">
                                    <tr>
                                        <th>Name</th>
                                        <td><asp:TextBox ID="txtName" runat="server" Text='<%#Bind("Name") %>' Width="350px" MaxLength="256" /></td>
                                    </tr>
                                    <tr>
                                        <th><a href="javascript: alert('This text will show throughout the order process as well as on the confirmation page and email.')" class="infomark">?</a>Display Text</th>
                                        <td><asp:TextBox ID="txtDisplay" runat="server" Text='<%#Bind("DisplayText") %>' Width="100%" MaxLength="1000" /></td>
                                    </tr>
                                    <tr><th><a href="javascript: alert('This text will show throughout the order process only and will NOT be displayed on the confirmation page or email.')" class="infomark">?</a>Additional Text</th>
                                        <td><asp:TextBox ID="txtAdditional" runat="server" Text='<%#Bind("AdditionalText") %>' Width="100%" MaxLength="500" /></td>
                                    </tr>
                                </table>
                                <div class="cmdsection">
                                    <asp:Button ID="btnSave" runat="server" CssClass="btnmed" Text="Save" 
                                        CommandName="Insert" CausesValidation="false"  />
                                    <asp:Button ID="btnCancel" runat="server" CssClass="btnmed" Text="Cancel" 
                                        CommandName="Cancel" CausesValidation="false" />
                                    <asp:CustomValidator ID="CustomFormValidation" runat="server" 
                                        CssClass="invisible" Display="Static" ValidationGroup="promo"  
                                         ErrorMessage="CustomValidator">*</asp:CustomValidator>
                                </div>
                            </InsertItemTemplate>
                        </asp:FormView>
                    </asp:WizardStep>
                    <asp:WizardStep ID="stepAward" runat="server" title="Awards">
                        <asp:FormView ID="FormView_Award" runat="server" DefaultMode="Edit" DataKeyNames="Id" Width="100%" 
                            DataSourceID="SqlAward" 
                            OnItemCommand="FormView_Award_ItemCommand"
                            OnItemUpdating="FormView_Award_ItemUpdating" 
                            OnDataBound="FormView_Award_DataBound" 
                            OnItemUpdated="FormView_ItemUpdated"
                            OnModeChanging="FormView_ModeChanging">
                            <EditItemTemplate>
                                <table border="0" cellspacing="3" cellpadding="0" width="100%" class="edittabl">
                                    <tr>
                                        <th><asp:Button ID="btnAddAward" CssClass="btnmed" runat="server" 
                                                CommandName="addaward" Text="Add Selected" Width="100px" /></th>
                                        <td colspan="2"><asp:DropDownList ID="ddlAwardsMerch" Width="100%" runat="server" DataSourceID="SqlAwardsAvailable" 
                                            DataTextField= "Name" DataValueField= "Id" AppendDataBoundItems="true">
                                                <asp:ListItem Selected="True" Text=" <-- Select an award --> " Value="0" />
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <asp:Literal ID="litMerchTR" runat="server" Text="<tr>" />
                                        <th style="vertical-align:top;">
                                            <div style="margin-bottom:16px;margin-right:12px;">Chosen Awards</div>
                                            <asp:Button ID="btnRemove" CssClass="btnmed" runat="server" Width="100px" CommandName="removeaward" 
                                                Text="Remove Selected" OnClientClick="return confirm('Are you sure you want to delete the selected award?');" />
                                        </th>
                                        <td colspan="2">
                                            <asp:ListBox ID="listAwards" Width="100%" Height="100" runat="server" DataSourceID="SqlChosenAwards" 
                                                DataTextField= "Name" DataValueField= "Id" />
                                        </td>
                                    </tr>
                                    <asp:Literal ID="litMerchTR2" runat="server" Text="<tr>" />
                                        <th style="vertical-align:top;">
                                            Allow Mult Selection
                                        </th>
                                        <td colspan="2">
                                            <asp:CheckBox ID="chkMultiple" runat="server" Checked='<%# Bind("bAllowMultSelections") %>' />
                                            <span class="intr">
                                                Changes the selection from one per order to multiple selection. Also makes price charged for each item.
                                            </span>
                                        </td>
                                    </tr>
                                    <tr><td colspan="3">&nbsp;</td></tr>
                                    <asp:Literal ID="litTicketTR" runat="server" Text="<tr>" />
                                        <th>Ticket</th>
                                        <td colspan="2">
                                            <asp:DropDownList ID="ddlTicket" Enabled="false" runat="server" Width="375px" />
                                            <cc1:CascadingDropDown ID="CascadingDropDown1" Enabled="false" runat="server" TargetControlID="ddlTicket" UseContextKey="true" 
                                                ContextKey='<%#Eval("tShowTicketId")%>'
                                                Category="Ticket"  PromptText="-- select a ticket item --"  LoadingText="[Loading tickets...]" 
                                                ServicePath="~/Services/Admin/PromotionListing_Svc.asmx" ServiceMethod="GetTicketListing"/>
                                            <asp:HiddenField ID="hdnTicket" runat="server" />
                                        </td>
                                    </tr>
                                    <tr><th>Weight</th>
                                        <td colspan="2"><asp:TextBox ID="txtWeight" runat="server" Text='<%#Bind("mWeight","{0:n2}") %>' Width="125px" MaxLength="10" /> <span class="intr">(merch only)</span></td>
                                    </tr>
                                    <tr><th>Price</th>
                                        <td colspan="2"><asp:TextBox ID="txtPrice" Enabled="true" runat="server" Text='<%#Bind("mPrice","{0:n2}") %>' Width="125px" MaxLength="10" /></td>
                                    </tr>
                                    <tr><th>Max/Order</th>
                                        <td colspan="2">
                                            <asp:DropDownList ID="ddlMaxPerOrder" Enabled="false" runat="server" Width="35px" OnDataBinding="ddlNumList_DataBinding" OnDataBound="ddlMaxPerOrder_DataBound" />
                                        </td>
                                    </tr>
                                    <asp:Literal ID="litDiscountTR" runat="server" Text="<tr>" />
                                        <th><a href="javascript: alert('Discount amount will override a discount percent')" class="infomark">?</a>
                                            Discount Amount
                                        </th>
                                        <td colspan="2"><asp:TextBox ID="txtDiscountAmount" runat="server" Text='<%#Bind("mDiscountAmount","{0:n2}") %>' Width="125px" MaxLength="10" /> </td>
                                    </tr>
                                    <asp:Literal ID="litPercentTR" runat="server" Text="<tr>" />
                                        <th>Discount Percent</th>
                                        <td colspan="2"><asp:TextBox ID="txtDiscountPercent" runat="server" Text='<%#Bind("iDiscountPercent") %>' Width="125px" MaxLength="10" /></td>
                                    </tr>
                                    <asp:Literal ID="litDiscountAppTR" runat="server" Text="<tr>" />
                                        <th style="vertical-align:top;">Discount Applies</th>
                                        <td style="white-space:nowrap;">
                                            <asp:CheckBoxList ID="chkDiscApp" runat="server" OnDataBinding="chkDiscApp_DataBinding" />
                                        </td>
                                        <td style="width:100%;">
                                            <div class="jqinstruction rounded" style="display:inline-block;" >
                                                <ul><strong>You may only select one award type:</strong>
                                                    <li>Merchandise</li>
                                                    <li>Ticket</li>
                                                    <li>Shipping(May have discount or percent amount)</li>
                                                    <li>**For free shipping, enter 100 in discount percent</li>
                                                    <li>TriggerItemOnly will discount the item selected for the trigger</li>
                                                </ul>
                                            </div>
                                        </td>
                                    </tr>
                                    <asp:Literal ID="litShipTR" runat="server" Text="<tr>" />
                                        <th>Ship Method</th>
                                        <td colspan="2"><asp:DropDownList ID="ddlShipping" runat="server" Width="375px" />
                                            <cc1:CascadingDropDown ID="CascadingDropDown3" runat="server" TargetControlID="ddlShipping" UseContextKey="true" ContextKey='<%#Eval("ShipOfferMethod")%>'
                                                Category="Shipping" PromptText="-- select a shipping method --"  LoadingText="[Loading methods...]" 
                                                ServicePath="~/Services/Admin/PromotionListing_Svc.asmx" ServiceMethod="GetShipMethodListing"/>
                                            <asp:HiddenField ID="hdnShipping" runat="server" />
                                    </tr>
                                    <asp:Literal ID="litMaxTR" runat="server" Text="<tr>" />
                                        <th>Max Value</th>
                                        <td colspan="2"><asp:TextBox ID="txtMaxValue" runat="server" Text='<%#Bind("mMaxValue","{0:n2}") %>' Width="125px" MaxLength="10" /></td>
                                    </tr>
                                </table>
                                <div class="cmdsection">
                                    <asp:Button ID="btnSave" runat="server" CssClass="btnmed" Text="Save" 
                                        CommandName="Update" CausesValidation="false" />
                                    <asp:Button ID="btnCancel" runat="server" CssClass="btnmed" Text="Cancel" 
                                        CommandName="Cancel" CausesValidation="false" />
                                    <asp:CustomValidator ID="CustomFormValidation" runat="server" 
                                        CssClass="invisible" Display="Static" ValidationGroup="promo"  
                                        ErrorMessage="CustomValidator">*</asp:CustomValidator>
                                </div>
                            </EditItemTemplate>
                        </asp:FormView>
                     </asp:WizardStep>
                    <asp:WizardStep ID="stepTrigger" runat="server" title="Triggers">
                        <asp:FormView ID="FormView_Trigger" runat="server" DefaultMode="Edit" DataKeyNames="Id" Width="100%" 
                            DataSourceID="SqlTrigger" 
                            OnItemUpdating="FormView_Trigger_ItemUpdating" 
                            OnDataBound="FormView_Trigger_DataBound" 
                            OnItemUpdated="FormView_ItemUpdated" 
                            OnItemCommand="FormView_Naming_ItemCommand"
                            OnModeChanging="FormView_ModeChanging">
                            <EditItemTemplate>
                                <table border="0" cellspacing="0" cellpadding="0" Width="100%" class="edittabl">
                                    <asp:Literal ID="litCodeTR" runat="server" Text="<tr>" />
                                        <th>Coupon Code</th>
                                        <td style="width:100%;white-space:nowrap;">
                                            <asp:TextBox ID="txtRequiredPromotionCode" runat="server" Text='<%#Bind("RequiredPromotionCode") %>' Width="190px" MaxLength="50" />
                                            <asp:Button ID="btnGenerate" runat="server" CommandName="generatecode" Text="Generate" Width="80px" 
                                                CssClass="btnmed" CommandArgument='<%#Eval("Id") %>' />
                                            <asp:Button ID="btnClearCode" runat="server" CommandName="clearcode" Text="Clear Code" width="80px"
                                                CssClass="btnmed" CommandArgument='<%#Eval("Id") %>' />
                                             <div class="intr">coupon code is applied during check out process you will need to enable Settings>Order Flow>Coupons_Active in the admin to enable the ability to enter a coupon code in the checkout process</div>
                                        </td>
                                    </tr>
                                    <asp:Literal ID="litUseTR" runat="server" Text="<tr>" />
                                        <th><a href="javascript: alert('Max Uses is the maximum number of times a particular customer may use a coupon code.')" class="infomark">?</a>
                                            Max Uses Per User
                                        </th>
                                        <td><asp:DropDownList id="ddlUses" runat="server" width="80px" OnDataBinding="ddlUses_DataBinding" />&nbsp;<span class="intr">(0=unlimited)</span></td>
                                    </tr>

                                    <asp:Literal ID="litMerchTR2" runat="server" Text="<tr>" />
                                        <th>
                                            <asp:Button ID="btnMerchSelect" Enabled="true" width="100px" runat="server" Text="Add Selection" CssClass="btnmed"
                                                OnClick="btnMerchSelect_Click" />
                                        </th>
                                        <td>
                                            <uc1:MerchSelector ID="MerchSelector1" runat="server" ButtonId='<%= this._btnMerchSelectId %>' ShowInventory="false" />
                                        </td>
                                    </tr>
                                    <asp:Literal ID="litMerchTR" runat="server" Text="<tr>" />
                                        <th>Required Merch
                                            <div style="font-weight:normal;font-size:12px;color:#ccc;">double-click to remove</div>
                                        </th>
                                        <td>
                                            <asp:ListBox ID="listRequiredMerch" SelectionMode="Single" runat="server" Font-Size="10px" Width="100%" CssClass="req-merch-list" 
                                                OnDataBound="listRequiredMerch_DataBound" OnDataBinding="listRequiredMerch_DataBinding" />
                                        </td>
                                    </tr>
                                    

                                    <asp:Literal ID="litShowDateTR" runat="server" Text="<tr>" />
                                        <th>Required ShowDate</th>
                                        <td>
                                            <asp:DropDownList ID="ddlReqShowDate" runat="server" Font-Size="10px" Width="100%" OnDataBinding="ddlReqShowDate_DataBinding" />                                            
                                        </td>
                                    </tr>
                                    <asp:Literal ID="litTicketTR" runat="server" Text="<tr>" />
                                        <th>Required Ticket</th>
                                        <td>
                                            <asp:DropDownList ID="ddlReqTicket" runat="server" Font-Size="10px" Width="100%" OnDataBinding="ddlReqTicket_DataBinding" />
                                        </td>
                                    </tr>
                                    <asp:Literal ID="litReqQtyTR" runat="server" Text="<tr>" />
                                        <th>Required Qty</th>
                                        <td>
                                            <asp:DropDownList ID="ddlRequiredQty" runat="server" Width="80px" OnDataBinding="ddlNumList_DataBinding" OnDataBound="ddlRequiredQty_DataBound" />
                                            <span class="intr">(applies to tickets and merchandise)</span>
                                        </td>
                                    </tr>                                            
                                    <asp:Literal ID="litMinMerchTR" runat="server" Text="<tr>" />
                                        <th><a href="javascript: alert('Purchase value must be greater OR equal to this anount')" class="infomark">?</a>
                                            Min Merch Purchase
                                         </th>
                                         <td><asp:TextBox ID="txtMinMerch" runat="server" Text='<%#Bind("mMinMerch","{0:n2}") %>' Width="80px" MaxLength="10" /></td>
                                     </tr>
                                    <asp:Literal ID="litMinTicketTR" runat="server" Text="<tr>" />
                                        <th><a href="javascript: alert('Purchase value must be greater OR equal to this anount')" class="infomark">?</a>
                                            Min Ticket Purchase
                                        </th>
                                        <td><asp:TextBox ID="txtMinTicket" runat="server" Text='<%#Bind("mMinTicket","{0:n2}") %>' Width="80px" MaxLength="10" /></td>
                                    </tr>       
                                    <asp:Literal ID="litMinTotalTR" runat="server" Text="<tr>" />
                                        <th><a href="javascript: alert('Purchase value must be greater OR equal to this anount')" class="infomark">?</a>
                                            Min Total Purchase
                                        </th>
                                        <td><asp:TextBox ID="txtMinTotal" runat="server" Text='<%#Bind("mMinTotal","{0:n2}") %>' Width="80px" MaxLength="10" />
                                            <span class="intr">Use a negative number here to trigger without having cart items</span></td>
                                    </tr>
                                    <tr>
                                        <th>Include Promo amount</th>
                                        <td>
                                            <asp:CheckBox ID="chkAllowPromo" runat="server" Checked='<%# Bind("bAllowPromoTotalInMinimum") %>' />
                                            <span class="intr">Checking this will allow any money spent on promotions to be included in the minimum totals.</span>
                                        </td>
                                    </tr>
                                    <tr id="TR_Tiers" runat="server">
                                        <th>Tiered GC Amounts
                                        </th>
                                        <td>
                                            <asp:GridView ID="gridTiers" runat="server" AutoGenerateColumns="false" ShowFooter="true" DataKeyNames="MinAmount" 
                                                OnDataBinding="gridTiers_DataBinding"
                                                OnDataBound="gridTiers_DataBound" 
                                                OnRowDataBound="gridTiers_RowDataBound" 
                                                OnRowCommand="gridTiers_RowCommand" 
                                                 
                                                OnRowEditing="gridTiers_RowEditing"
                                                OnRowCancelingEdit="gridTiers_RowCancelingEdit"                                                
                                                
                                                OnRowDeleting="gridTiers_RowDeleting"
                                                
                                                OnRowUpdating="gridTiers_RowUpdating"
                                            >
                                                <EmptyDataTemplate>
                                                    <table border="0">
                                                        <tr>
                                                            <th>MinAmount</th>
                                                            <th colspan="2">Reward</th>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtMin" runat="server" MaxLength="10" Text='<%#Bind("MinAmount") %>' />
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlTierReward" runat="server" OnDataBinding="ddlTierReward_DataBinding" />
                                                            </td>
                                                            <td>
                                                                <asp:Button UseSubmitBehavior="false" id="lnkCommand" runat="server" CommandName="Insert" Text="Add" CssClass="btnmed" /> 
                                                                <asp:Button UseSubmitBehavior="false" id="lnkCancel" runat="server" CommandName="Cancel" Text="Cancel" CssClass="btnmed" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </EmptyDataTemplate>
                                                <Columns>
                                                    <asp:TemplateField HeaderText="MinAmount" >
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtMin" runat="server" MaxLength="10" Text='<%#Bind("MinAmount") %>' />
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <%#Eval("MinAmount") %>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                        <br />
                                                            <asp:TextBox ID="txtMin" runat="server" MaxLength="10" Text='<%#Bind("MinAmount") %>' />
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Reward" >
                                                        <EditItemTemplate>
                                                            <asp:DropDownList ID="ddlTierReward" runat="server" OnDataBinding="ddlTierReward_DataBinding" />
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <%#Eval("RewardAmount") %>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                        <br />
                                                            <asp:DropDownList ID="ddlTierReward" runat="server" OnDataBinding="ddlTierReward_DataBinding" />
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="">
                                                        <EditItemTemplate>
                                                            <asp:Button UseSubmitBehavior="false" id="lnkSave" runat="server" CommandName="Update" Text="Update" CssClass="btnmed" />
                                                            <asp:Button UseSubmitBehavior="false" id="lnkCancel" runat="server" CommandName="Cancel" Text="Cancel" CssClass="btnmed" />                                                           
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Button UseSubmitBehavior="false" id="lnkEdit" runat="server" CommandName="Edit" Text="Edit" CssClass="btnmed" />
                                                            <asp:Button id="lnkDelete" runat="server" CommandName="Delete" CommandArgument='<%#Eval("MinAmount") %>' Text="Delete" CssClass="btnmed"
                                                                OnClientClick="return confirm('Are you sure you want to delete this tier?');" />
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                        <br />
                                                            <asp:Button UseSubmitBehavior="false" id="lnkCommand" runat="server" CommandName="Insert" Text="Add" CssClass="btnmed" /> 
                                                            <asp:Button UseSubmitBehavior="false" id="lnkCancel" runat="server" CommandName="Cancel" Text="Cancel" CssClass="btnmed" />                                                           
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                </Columns>                                                
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </table>
                                <div class="cmdsection">
                                    <asp:Button ID="btnSave" runat="server" CssClass="btnmed" Text="Save" 
                                        CommandName="Update" CausesValidation="false" />
                                    <asp:Button ID="btnCancel" runat="server" CssClass="btnmed" Text="Cancel" 
                                        CommandName="Cancel" CausesValidation="false" />
                                    <asp:CustomValidator ID="CustomFormValidation" runat="server" 
                                        CssClass="invisible" Display="Static" ValidationGroup="promo"  
                                        ErrorMessage="CustomValidator">*</asp:CustomValidator>
                                </div>
                            </EditItemTemplate>
                        </asp:FormView>
                     </asp:WizardStep>
                </WizardSteps>
            </asp:Wizard>
            <%if (Atx.CurrentSalePromotion != null && Atx.CurrentSalePromotion.BannerUrl != null && Atx.CurrentSalePromotion.BannerUrl.Trim().Length > 0)
              {%>
            <img src='<%=Atx.CurrentSalePromotion.Banner_VirtualFilePath %>' alt="" />
            <%} %>
        </div>
    <div style="visibility:hidden;"><asp:FileUpload ID="FileUploadRegisterControl" runat="server" Width="350px" CssClass="btnmed" /></div>
    </div>
</div>

<input type="hidden" id="hidSalePromotionId" value='<%=this.SalePromotionId %>' />
<input type="hidden" id="hidClientControlRemoverId" value='<%=this._uniqueId %>' />

<asp:ObjectDataSource ID="ObjBanners" runat="server" EnablePaging="true" 
    SelectMethod="GetSalePromotions" EnableCaching="false" 
    TypeName="Wcss.SalePromotion" SelectCountMethod="GetSalePromotionsCount"  
    OnSelecting="objData_Selecting" OnSelected="objData_Selected" >
    <SelectParameters>
        <asp:Parameter Name="bannerContext" Type="String" DefaultValue="All" />
    </SelectParameters>
</asp:ObjectDataSource>
<asp:SqlDataSource ID="SqlNaming" runat="server" ConnectionString="<%$ ConnectionStrings:WillCallConnectionString %>" 
    InsertCommand="INSERT INTO [SalePromotion] ([ApplicationId], [bActive], [iBannerTimeoutMsecs], [Name], [DisplayText], [AdditionalText], [iDisplayOrder], [dtStamp]) 
        VALUES (@ApplicationId, @Active, @iBannerTimeoutMsecs, @Name, @DisplayText, @AdditionalText, -1, (getDate()));         
        UPDATE [SalePromotion] SET [iDisplayOrder] += 1 " 
        OnInserting="SqlNaming_Inserting"
    UpdateCommand="UPDATE [SalePromotion] SET [bActive] = @bActive, [bDeactivateOnNoInventory] = @bDeactivateOnNoInventory, 
        [bDisplayAtParent] = @bDisplayAtParent, [bBannerMerch] = @bBannerMerch, [bBannerTicket] = @bBannerTicket, 
        [bBannerCartEdit] = @bBannerCartEdit, [bBannerCheckout] = @bBannerCheckout, [bBannerShipping] = @bBannerShipping, 
        [Name] = @Name, [DisplayText] = @DisplayText, [AdditionalText] = @AdditionalText, [BannerClickUrl] = @BannerClickUrl,
        [UnlockCode] = @UnlockCode, [dtStartDate] = @dtStartDate, [dtEndDate] = @dtEndDate 
        FROM [SalePromotion] sp WHERE sp.[Id] = @Id"  
    OnUpdating="SqlNaming_Updating"  
    SelectCommand="SELECT [Id], [bActive], [bDeactivateOnNoInventory], [bDisplayAtParent], [bBannerMerch], [bBannerTicket], 
        [bBannerCartEdit], [bBannerCheckout], [bBannerShipping], [Name], [BannerClickUrl], [DisplayText], [AdditionalText], [UnlockCode], 
        ISNULL([dtStartDate], '1/1/1753') as 'dtStartDate', ISNULL([dtEndDate], '1/1/1753') as 'dtEndDate'
        FROM [SalePromotion] sp WHERE sp.[Id] = @Id" 
     >
    <InsertParameters>
        <asp:Parameter Name="ApplicationId" Type="String" />
        <asp:Parameter Name="Active" Type="Boolean" DefaultValue="false" />
        <asp:Parameter Name="iBannerTimeoutMsecs" Type="Int32" />
        <asp:Parameter Name="Name" Type="String" />
        <asp:Parameter Name="DisplayText" Type="String" />
        <asp:Parameter Name="AdditionalText" Type="String" />
    </InsertParameters>
    <UpdateParameters>
        <asp:Parameter Name="Id" Type="Int32" />
        <asp:Parameter Name="bActive" Type="Boolean" />
        <asp:Parameter Name="bDeactivateOnNoInventory" Type="Boolean" />
        <asp:Parameter Name="bDisplayAtParent" Type="Boolean" />
        <asp:Parameter Name="bBannerMerch" Type="Boolean" />
        <asp:Parameter Name="bBannerTicket" Type="Boolean" />
        <asp:Parameter Name="bBannerCartEdit" Type="Boolean" />
        <asp:Parameter Name="bBannerCheckout" Type="Boolean" />
        <asp:Parameter Name="bBannerShipping" Type="Boolean" />
        <asp:Parameter Name="Name" Type="String" />
        <asp:Parameter Name="BannerClickUrl" Type="String" />
        <asp:Parameter Name="DisplayText" Type="String" />
        <asp:Parameter Name="AdditionalText" Type="String" ConvertEmptyStringToNull="true" />      
        <asp:Parameter Name="UnlockCode" Type="String" ConvertEmptyStringToNull="true" />                                  
        <asp:Parameter Name="dtStartDate" DbType="DateTime" ConvertEmptyStringToNull="true" />
        <asp:Parameter Name="dtEndDate" DbType="DateTime" ConvertEmptyStringToNull="true" />
    </UpdateParameters>
    <SelectParameters>
        <asp:ControlParameter ControlID="GridView1" Name="Id" PropertyName="SelectedValue" Type="Int32" />
    </SelectParameters>
</asp:SqlDataSource>
<asp:SqlDataSource ID="SqlChosenAwards" runat="server" ConnectionString="<%$ ConnectionStrings:WillCallConnectionString %>"
    SelectCommand="SELECT m.[Name], ent.[Id] FROM [Merch] m, [SalePromotionAward] ent 
        WHERE ent.[TSalePromotionId] = @promoId AND ent.[TParentMerchId] = m.[Id] 
        ORDER BY m.[Name] " >
    <SelectParameters>
        <asp:ControlParameter ControlID="GridView1" Name="promoId" PropertyName="SelectedValue" Type="Int32" />
    </SelectParameters>
</asp:SqlDataSource>
<asp:SqlDataSource ID="SqlAwardsAvailable" runat="server" ConnectionString="<%$ ConnectionStrings:WillCallConnectionString %>"
    SelectCommand="SELECT m.[Name], m.[Id] FROM [Merch] m WHERE m.[tParentListing] IS NULL AND m.[bActive] = 1 AND 
        m.[Id] NOT IN (SELECT mp.[Id] FROM [Merch] mp WHERE mp.[Name] LIKE '%Gift Certificate') AND 
        m.[Id] NOT IN (SELECT TParentMerchId FROM [SalePromotionAward] spa WHERE spa.TSalePromotionId = @promoId)
        ORDER BY m.[Name] " >
    <SelectParameters>
        <asp:ControlParameter ControlID="GridView1" Name="promoId" PropertyName="SelectedValue" Type="Int32" />
    </SelectParameters>
</asp:SqlDataSource>
<asp:SqlDataSource ID="SqlAward" runat="server" ConnectionString="<%$ ConnectionStrings:WillCallConnectionString %>"
    OnSelecting="SqlAward_Selecting"
    UpdateCommand="UPDATE [SalePromotion] SET [tShowTicketId] = @tShowTicketId, [mPrice] = @mPrice, 
        [bAllowMultSelections] = @bAllowMultSelections,
        [mDiscountAmount] = @mDiscountAmount, [iDiscountPercent] = @iDiscountPercent, 
        [vcDiscountContext] = CASE WHEN (@mDiscountAmount > 0 )THEN null ELSE @vcDiscountContext END, 
        [ShipOfferMethod] = CASE WHEN (@mDiscountAmount > 0 ) THEN null ELSE @ShipOfferMethod END, [iMaxPerOrder] = @iMaxPerOrder, [mWeight] = @mWeight, [mMaxValue] = @mMaxValue 
        FROM [SalePromotion] sp 
        WHERE sp.[Id] = @Id 
        "    
    SelectCommand="SELECT sp.[Id], sp.[tShowTicketId], (CAST(s.[Name] as varchar(256))) as 'TicketName', sp.[mPrice], 
        ISNULL(sp.[bAllowMultSelections],0) as [bAllowMultSelections], sp.[mDiscountAmount], sp.[iDiscountPercent], sp.[vcDiscountContext], sp.[ShipOfferMethod], sp.[iMaxPerOrder], 
        sp.[mWeight], sp.[mMaxValue] 
        FROM [SalePromotion] sp LEFT OUTER JOIN [ShowTicket] st ON sp.[tShowTicketId] = st.[Id] 
            LEFT OUTER JOIN [Show] s ON st.[tShowId] = s.[Id] 
        WHERE sp.[Id] = @Id AND sp.[ApplicationId] = @appId " >
    <UpdateParameters>
        <asp:Parameter Name="Id" Type="Int32" />
        <asp:Parameter Name="tShowTicketId" Type="Int32" ConvertEmptyStringToNull="true" />
        <asp:Parameter Name="mPrice" Type="Decimal" DefaultValue="0" />
        <asp:Parameter Name="bAllowMultSelections" Type="Boolean" DefaultValue="false" />
        <asp:Parameter Name="mDiscountAmount" Type="Decimal" DefaultValue="0" />
        <asp:Parameter Name="iDiscountPercent" Type="Int32" DefaultValue="0" />
        <asp:Parameter Name="vcDiscountContext" Type="String" ConvertEmptyStringToNull="true" />
        <asp:Parameter Name="ShipOfferMethod" Type="String" ConvertEmptyStringToNull="true" />
        <asp:Parameter Name="iMaxPerOrder" Type="Int32" DefaultValue="1" />
        <asp:Parameter Name="mWeight" Type="Decimal" DefaultValue="0" />
        <asp:Parameter Name="mMaxValue" Type="Decimal" DefaultValue="0" />
    </UpdateParameters>
    <SelectParameters>
        <asp:Parameter Name="appId" DbType="Guid" />
        <asp:ControlParameter ControlID="GridView1" Name="Id" PropertyName="SelectedValue" Type="Int32" />
    </SelectParameters>
</asp:SqlDataSource>
<asp:SqlDataSource ID="SqlTrigger" runat="server" ConnectionString="<%$ ConnectionStrings:WillCallConnectionString %>"
    OnSelecting="SqlTrigger_Selecting"
    UpdateCommand="
        UPDATE [SalePromotion] SET [RequiredPromotionCode] = @RequiredPromotionCode, 
        [tRequiredParentShowDateId] = @tRequiredParentShowDateId,
        [tRequiredParentShowTicketId] = @tRequiredParentShowTicketId, [iRequiredParentQty] = @iRequiredParentQty, 
        [mMinMerch] = @mMinMerch, [bAllowPromoTotalInMinimum] = @bAllowPromoTotalInMinimum, [mMinTicket] = @mMinTicket, 
        [mMinTotal] = @mMinTotal, [iMaxUsesPerUser] = @iMaxUsesPerUser 
        FROM [SalePromotion] sp 
        WHERE sp.[Id] = @Id "
    SelectCommand="SELECT sp.[Id], sp.[RequiredPromotionCode], sp.[vcTriggerList_Merch], sp.[tRequiredParentShowTicketId], sp.[tRequiredParentShowDateId], 
        (CAST(s.[Name] as varchar(256))) as 'TicketName', 
        (CONVERT(varchar, ISNULL(sd.[dtDateOfShow],''), 100) + SUBSTRING(s.[Name], 20, LEN(s.Name))) as 'DateName', 
        sp.[iRequiredParentQty], 
        sp.[mMinMerch], ISNULL(sp.[bAllowPromoTotalInMinimum], 0) as [bAllowPromoTotalInMinimum], sp.[mMinTicket], sp.[mMinTotal], sp.[iMaxUsesPerUser],
        sp.[jsonMeta] 
        FROM [SalePromotion] sp 
            LEFT OUTER JOIN [ShowDate] sd ON sp.[tRequiredParentShowDateId] = sd.[Id]
            LEFT OUTER JOIN [ShowTicket] st ON sp.[tRequiredParentShowTicketId] = st.[Id] 
            LEFT OUTER JOIN [Show] s ON sd.[tShowId] = s.[Id] OR st.[tShowId] = s.[Id]
        WHERE sp.[Id] = @Id AND sp.[ApplicationId] = @appId " 
    >
    <UpdateParameters>
        <asp:Parameter Name="Id" Type="Int32" />
        <asp:Parameter Name="RequiredPromotionCode" Type="String" ConvertEmptyStringToNull="true" />
        <asp:Parameter Name="tRequiredParentShowTicketId" Type="Int32" ConvertEmptyStringToNull="true" />
        <asp:Parameter Name="tRequiredParentShowDateId" Type="Int32" ConvertEmptyStringToNull="true" />
        <asp:Parameter Name="iRequiredParentQty" Type="Int32" DefaultValue="1" />
        <asp:Parameter Name="iMaxUsesPerUser" Type="Int32" />
        <asp:Parameter Name="mMinMerch" Type="Decimal" DefaultValue="0" />
        <asp:Parameter Name="bAllowPromoTotalInMinimum" Type="Boolean" DefaultValue="false" />
        <asp:Parameter Name="mMinTicket" Type="Decimal" DefaultValue="0" />
        <asp:Parameter Name="mMinTotal" Type="Decimal" DefaultValue="0" />
    </UpdateParameters>
    <SelectParameters>
        <asp:Parameter Name="appId" DbType="Guid" />
        <asp:ControlParameter ControlID="GridView1" Name="Id" PropertyName="SelectedValue" Type="Int32" />
    </SelectParameters>
</asp:SqlDataSource>
<script language="javascript" type="text/javascript">
	
    var award = '<%= this.FormView_Award.UniqueID %>';
    var trigger = '<%= this.FormView_Trigger.UniqueID %>';
   
    function pageLoad() {

        var ticket = getDOMElement(award, "ddlTicket");
        var shipping = getDOMElement(award, "ddlShipping");

        var reqTicket = getDOMElement(trigger, "ddlReqTicket");
        var reqShowDate = getDOMElement(trigger, "ddlReqShowDate");
        
       // Hook-up the change event of the dropdownlist.
       if(ticket) {
           $addHandlers(ticket, {change:onSelectionChosen}, this); 
       }
       if(shipping) {
           $addHandlers(shipping, {change:onSelectionChosen}, this); 
       }
       if(reqTicket) {
           $addHandlers(reqTicket, {change:onSelectionChosen}, this);
       }
       if (reqShowDate) {
           $addHandlers(reqShowDate, { change: onSelectionChosen }, this);
       }   
    }
    
    function onSelectionChosen(evt) {

        var ddl = evt.target;
        
        var hiddenControl;
        var controlValue = "";        
        
        if(ddl.id.indexOf("_ddlTicket") > -1) {
            hiddenControl = getDOMElement(award, "hdnTicket");
            controlValue = String.format("{0}", ddl.options[ddl.selectedIndex].value);
        }
        else if(ddl.id.indexOf("_ddlShipping") > -1) {
            hiddenControl = getDOMElement(award, "hdnShipping");
            controlValue = String.format("{0}", ddl.options[ddl.selectedIndex].value);
        }
        else if(ddl.id.indexOf("_ddlReqTicket") > -1) {
            hiddenControl = getDOMElement(trigger, "hdnReqTicket");            
            controlValue = String.format("{0}", ddl.options[ddl.selectedIndex].value);
        }
        else if (ddl.id.indexOf("_ddlReqShowDate") > -1) {
            hiddenControl = getDOMElement(trigger, "hdnReqShowDate");
            //alert(ddl.options[ddl.selectedIndex].value);
            controlValue = String.format("{0}", ddl.options[ddl.selectedIndex].value);
        }
        if(hiddenControl != undefined)
            hiddenControl.value = controlValue;
    }
</script>
