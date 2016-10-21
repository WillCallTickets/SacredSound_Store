<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Prom_Banner.ascx.cs" Inherits="WillCallWeb.Admin.AdminControls.Prom_Banner" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register src="~/Components/Util/CalendarClock.ascx" tagname="CalendarClock" tagprefix="uc1" %>
<%@ Register src="~/Components/Navigation/gglPager.ascx" tagname="gglPager" tagprefix="uc2" %>
<div id="srceditor">
    <div id="promotion">
        <uc2:gglPager ID="GooglePager1" runat="server" PageButtonClass="btnmed" PagerTitle="Banners" >
            <Template>
                <div class="cmdsection">
                    <asp:Button ID="btnPromo" runat="server" CssClass="btnmed" Text="Promotion Editor" OnClick="btnPromo_Click" 
                        CausesValidation="false" />
                    <asp:CustomValidator ID="CustomValidator1" runat="server" CssClass="invisible" Display="Static" ValidationGroup="promo"  
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
                <div class="lstempty">No Banners</div>
            </EmptyDataTemplate>
           <Columns>
               <asp:TemplateField HeaderText="#" ItemStyle-HorizontalAlign="center" ItemStyle-CssClass="rowcounter">
                    <ItemTemplate><asp:Literal ID="LiteralRowCounter" runat="server" /></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ItemStyle-Wrap="false" HeaderText="Select" >
                    <ItemTemplate>
                        <asp:LinkButton Width="20px" Id="btnSelect" ToolTip="Select" CssClass="btnselect" runat="server" CommandName="Select" 
                            CommandArgument='<%#Eval("Id") %>' CausesValidation="false" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:CheckBoxField ReadOnly="true" DataField="IsActive" HeaderText="Active" ItemStyle-HorizontalAlign="Center" />
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
        <asp:FormView ID="FormView1" runat="server" DefaultMode="Edit" DataKeyNames="Id,BannerUrl" Width="100%" 
            DataSourceID="SqlNaming" 
            onmodechanging="FormView1_ModeChanging" 
            OnItemCreated="FormView1_ItemCreated"
            OnDataBinding="FormView1_DataBinding"
            OnDataBound="FormView1_DataBound" 
            OnItemInserting="FormView1_ItemInserting" 
            OnItemInserted="FormView1_ItemInserted"
            onitemcommand="FormView1_ItemCommand" 
            OnItemUpdated="FormView1_ItemUpdated">
            <EmptyDataTemplate>
                <div class="jqhead rounded">
                    <div class="cmdsection">
                        <asp:Button ID="btnNew" runat="server" CssClass="btnmed" Text="Add New Promo" CommandName="New" CausesValidation="false" />
                    </div>
                </div>
            </EmptyDataTemplate>
            <InsertItemTemplate>
                <div class="jqhead rounded">
                    <h3 class="entry-title">Adding a new banner...</h3>
                    <table border="0" cellspacing="0" cellpadding="0" width="100%" class="edittabl">
                        <tr>
                            <th><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="promo" 
                                ErrorMessage="Name is required" Display="static" CssClass="validator" 
                                ControlToValidate="txtName" >*</asp:RequiredFieldValidator>
                            </th>
                            <th>Name</th>
                            <td style="width:100%;"><asp:TextBox ID="txtName" runat="server" Text='<%#Bind("Name") %>' Width="100%" MaxLength="256" /></td>
                        </tr>
                        <tr><th><asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ValidationGroup="promo"  ErrorMessage="Display Text is required"
                                    Display="static" CssClass="validator" ControlToValidate="txtDisplay" >*</asp:RequiredFieldValidator>
                            </th>
                            <th><a href="javascript: alert('This is ued to describe the promotion to the user. Displayed as a title in the order flow')" class="infomark">?</a>
                                Display Text</th>
                            <td><asp:TextBox ID="txtDisplay" runat="server" Text='<%#Bind("DisplayText") %>' Width="100%" MaxLength="1000"/></td>
                        </tr>
                        <tr><th colspan="2">
                                <a href="javascript: alert('A line of additional information regarding the promotion, ie: must be 21 to purchase, offer good until. Note that some information will be automatic such as the ShipMethod and requirements')" class="infomark">?</a>
                                Additional Text</th>
                            <td><asp:TextBox ID="txtAdditional" runat="server" Text='<%#Bind("AdditionalText") %>' Width="100%" MaxLength="500" /></td>
                        </tr>
                    </table>
                    <div class="cmdsection">
                        <asp:Button ID="btnSave" runat="server" CssClass="btnmed" Text="Save" CommandName="Insert" 
                            CausesValidation="true" ValidationGroup="promo"  />
                        <asp:Button ID="btnCancel" runat="server" CssClass="btnmed" Text="Cancel" CommandName="Cancel" CausesValidation="false" />
                    </div>
                </div>
            </InsertItemTemplate>
            <EditItemTemplate>
                <div class="jqhead rounded">
                    <h3 class="entry-title"><%#Eval("Name") %></h3>
                    <table border="0" cellspacing="0" cellpadding="0" width="100%" class="edittabl">
                        <tr>
                            <th colspan="2"><a href="javascript: alert('Turns the promotion on/off. When creating a new promotion, this is off by default')" class="infomark">?</a>
                                Active</th>
                            <td><asp:CheckBox ID="chkActive" runat="server" Checked='<%# Bind("bActive") %>' /> - <%#Eval("Id") %></td>
                        </tr>
                        <tr>
                            <th colspan="2"><a href="javascript: alert('The amount of time to display the banner - IN MILLISECONDS')" class="infomark">?</a>
                                Display mSecs</th>
                            <td><asp:TextBox ID="txtTimeout" runat="server" Text='<%#Bind("iBannerTimeoutMsecs") %>' Width="350px" MaxLength="6" /></td>
                        </tr>
                        <tr>
                            <th><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="promo"  ErrorMessage="Name is required"
                                    Display="static" CssClass="validator" ControlToValidate="txtName" >*</asp:RequiredFieldValidator></th>
                            <th>
                                <a href="javascript: alert('A friendly name for the banner. This name is not displayed in the order flow.')" class="infomark">?</a>
                                Name</th>
                            <td style="width:100%;"><asp:TextBox ID="txtName" runat="server" Text='<%#Bind("Name") %>' Width="350px" MaxLength="256" /></td>
                        </tr>
                         <tr>
                            <th><asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ValidationGroup="promo"  ErrorMessage="Display Text is required"
                                    Display="static" CssClass="validator" ControlToValidate="txtDisplay" >*</asp:RequiredFieldValidator>
                            </th>
                            <th>
                                <a href="javascript: alert('This is ued to describe the promotion to the user. Displayed as a title in the order flow')" class="infomark">?</a>
                                Display Text</th>
                            <td>
                                <asp:TextBox ID="txtDisplay" runat="server" Text='<%#Bind("DisplayText") %>' Width="100%" MaxLength="1000"/>
                            </td>
                        </tr>
                        <tr>
                            <th colspan="2"><a href="javascript: alert('A line of additional information regarding the promotion, ie: must be 21 to purchase, offer good until. Note that some information will be automatic such as the ShipMethod and requirements')" class="infomark">?</a>
                                Additional Text</th>
                            <td><asp:TextBox ID="txtAdditional" runat="server" Text='<%#Bind("AdditionalText") %>' Width="100%" MaxLength="500" /></td>
                        </tr>
                        <tr>
                            <th colspan="3"><hr /></th>
                        </tr>
                        <tr>
                            <th colspan="2">
                                <a href="javascript: alert('This will setup a link to a selected show. You will need to save to complete the operation.')" class="infomark">?</a>
                                <asp:Button ID="btnLoadShow" runat="server" CommandName="LoadShow" CssClass="btnmed" Text="Load Selection" Width="100px" CausesValidation="false"
                                    OnClientClick="return confirm('Are you sure you want to link to the selected show?');" />
                            </th>
                            <td>
                                <asp:DropDownList ID="ddlShowList" runat="server" DataSourceID="SqlShowList"
                                    DataTextField="ShowName" DataValueField="ShowId">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <th colspan="2"><a href="javascript: alert('You may specify a url to link to when the banner is clicked.')" class="infomark">?</a>
                                Click Thru Url</th>
                            <td><asp:TextBox ID="txtBannerClickUrl" runat="server" Text='<%#Bind("BannerClickUrl") %>' Width="350px" MaxLength="256" 
                                OnTextChanged="txtBannerClick_TextChanged" />
                                &nbsp;
                                <asp:Literal ID="litUrlTest" runat="server" OnDataBinding="litUrlTest_DataBinding" />
                                &nbsp;&nbsp;&nbsp;<asp:LinkButton ID="btnClearUrl" runat="server" CommandName="ClearUrl" Text="clear" CausesValidation="false"
                                    OnClientClick="return confirm('Are you sure you want to clear the url?');" />
                            </td>
                        <tr>
                        <tr>
                            <th colspan="2">&nbsp;</th>
                            <td>
                                <div class="jqinstruction rounded" style="margin-top:3px;">
                                        <li><b>External links:</b> http://www.whatever.com</li>
                                        <li><b>Show links:</b> /store/chooseticket.aspx?sid=</li>
                                        <li><b>Merch links:</b> visit the page you would like to navigate to and copy the url.</li>
                                        <li>All you need to do is to chop off the http://domainname.com. The link should start with /store/choosemerch.aspx?...</li>
                                    </ul>
                                </div>
                            </td>
                        </tr>                        
                        <tr>
                            <th colspan="2">Pages To Display On</th>
                            <td style="padding:6px;">
                                <asp:CheckBox ID="chkBannerTicket" runat="server" Checked='<%# Bind("bBannerTicket") %>' Text=" Ticketing" TextAlign="Right" />
                                <asp:CheckBox ID="chkBannerMerch" runat="server" Checked='<%# Bind("bBannerMerch") %>' Text=" Merch" TextAlign="Right" />
                                <asp:CheckBox ID="chkDisplayCart" runat="server" Checked='<%# Bind("bBannerCartEdit") %>' Text=" CartEdit" TextAlign="Right" />
                                <asp:CheckBox ID="chkDisplayParent" Enabled="false" runat="server" Checked='<%# Bind("bDisplayAtParent") %>' Text=" Required" TextAlign="Right" />
                                <asp:CheckBox ID="chkDisplayCheckout" runat="server" Checked='<%# Bind("bBannerCheckout") %>' Text=" Checkout" TextAlign="Right" />
                                <asp:CheckBox ID="chkDisplayShipping" runat="server" Checked='<%# Bind("bBannerShipping") %>' Text=" Shipping" TextAlign="Right" />
                            </td>
                        </tr>
                        <tr>
                            <th colspan="2">Start Date</th>
                            <td><uc1:CalendarClock ID="CalendarClockStart" SelectedDate='<%#Eval("dtStartDate") %>' IsRequired="false" 
                                    ValidationGroup="promo" UseTime="true" runat="server" Width="352" />
                            </td>
                        </tr>
                        <tr>
                            <th colspan="2">End Date</th>
                            <td><uc1:CalendarClock ID="CalendarClockEnd" SelectedDate='<%#Eval("dtEndDate") %>' IsRequired="false"
                                    ValidationGroup="promo" UseTime="true" runat="server" Width="352" />
                            </td>
                        </tr>
                        <tr>
                            <th colspan="2">Current Image</th>
                            <td><%=Wcss.SalePromotion.Banner_VirtualDirectory %><%#Eval("BannerUrl") %></td>
                        </tr>
                        <tr>
                            <th colspan="2">&nbsp;</th>
                            <td><%=Wcss._Config._BannerDimensionText %></td>
                        </tr>
                        <tr>
                            <th colspan="2">
                                <a href="javascript: alert('Valid file names contain letters, numbers, underscores - please do not use parentheses or non-alphanumeric chars in the file name. You may need to rename the original file. Also note that images must be saved in RGB mode (CMYK WILL NOT WORK)')" class="infomark">?</a>
                                Image Upload</th>
                            <td colspan="4" style="white-space:nowrap;">
                                <asp:FileUpload ID="FileUpload1" runat="server" Width="350px" CssClass="btnmed" /></td>
                        </tr>
                        <tr>
                            <td colspan="2">&nbsp;</td>
                            <td class="jqinstruction rounded">
                                <asp:Button ID="btnUpload" runat="server" CssClass="btnmed btnupload" Text="Upload" 
                                    onclick="btnUpload_Click" CausesValidation="false" />
                                <asp:Button ID="btnDeleteBanner" runat="server" cssClass="btnmed" CommandName="deletebanner" 
                                    CausesValidation="false"
                                    OnClientClick="return confirm('Are you sure you want to delete this banner?') " Text="Delete" /> 
                            </td>
                        </tr>
                    </table>
                    <div class="cmdsection">
                        <asp:Button ID="btnSave" runat="server" CssClass="btnmed" Text="Save" CommandName="Update" 
                            CausesValidation="true" ValidationGroup="promo"  />
                        <asp:Button ID="btnCancel" runat="server" CssClass="btnmed" Text="Cancel" CommandName="Cancel" CausesValidation="false" />
                        <asp:Button ID="btnNew" runat="server" CssClass="btnmed" Text="New Banner" CommandName="New" CausesValidation="false" />
                    </div>
                    <div class="jqpanel1 rounded">
                        <asp:Literal ID="litImage" runat="server" />
                    </div>
                </div>
            </EditItemTemplate>
        </asp:FormView>
    <div style="visibility:hidden;"><asp:FileUpload ID="FileUploadRegisterControl" runat="server" Width="350px" CssClass="btnmed" /></div>
    </div>
</div>
<asp:SqlDataSource ID="SqlShowList" runat="server" ConnectionString="<%$ ConnectionStrings:WillCallConnectionString %>"
    SelectCommand="SELECT 0"
    onselecting="SqlShowList_Selecting">
    <SelectParameters>
        <asp:Parameter Name="appId" DbType="Guid" />
        <asp:Parameter Name="startDate" Type="DateTime" />
    </SelectParameters>    
</asp:SqlDataSource>
<asp:ObjectDataSource ID="ObjBanners" runat="server" EnablePaging="true" 
    SelectMethod="GetSalePromotions" EnableCaching="false" 
    TypeName="Wcss.SalePromotion" SelectCountMethod="GetSalePromotionsCount"  
    OnSelecting="objData_Selecting" OnSelected="objData_Selected" >
    <SelectParameters>
        <asp:Parameter Name="bannerContext" Type="String" DefaultValue="BannersOnly" />
    </SelectParameters>
</asp:ObjectDataSource>
<asp:SqlDataSource ID="SqlNaming" runat="server" EnableCaching="false" ConnectionString="<%$ ConnectionStrings:WillCallConnectionString %>"     
    SelectCommand="SELECT [Id], [bActive], [bDeactivateOnNoInventory], [bDisplayAtParent], [bBannerMerch], [bBannerTicket], 
    [bBannerCartEdit], [bBannerCheckout], [bBannerShipping], [iBannerTimeoutMsecs], [Name], [BannerUrl], [BannerClickUrl], [DisplayText], [AdditionalText], 
    ISNULL([dtStartDate], '1/1/1753') as 'dtStartDate', ISNULL([dtEndDate], '1/1/1753') as 'dtEndDate'
    FROM [SalePromotion] sp WHERE sp.[Id] = @Id" 
    InsertCommand="INSERT INTO [SalePromotion] ([ApplicationId], [bActive], [iBannerTimeoutMsecs], [Name], [DisplayText], [AdditionalText], [dtStamp]) 
    VALUES (@ApplicationId, @Active, @iBannerTimeoutMsecs, @Name, @DisplayText, @AdditionalText, (getDate())); 
    DECLARE @idx int; SET @idx = SCOPE_IDENTITY(); 
    UPDATE [SalePromotion] SET [iDisplayOrder] =  @idx WHERE [Id] = @idx; " 
    OnInserting="SqlNaming_Inserting"
    UpdateCommand="UPDATE [SalePromotion] SET [bActive] = @bActive, [iBannerTimeoutMsecs] = @iBannerTimeoutMsecs,
    [bDisplayAtParent] = @bDisplayAtParent, [bBannerMerch] = @bBannerMerch, [bBannerTicket] = @bBannerTicket, 
    [bBannerCartEdit] = @bBannerCartEdit, [bBannerCheckout] = @bBannerCheckout, [bBannerShipping] = @bBannerShipping, 
    [Name] = @Name, [DisplayText] = @DisplayText, [AdditionalText] = @AdditionalText, [BannerClickUrl] = @BannerClickUrl,
    [dtStartDate] = @dtStartDate, [dtEndDate] = @dtEndDate 
    FROM [SalePromotion] sp WHERE sp.[Id] = @Id"  
    OnUpdating="SqlNaming_Updating" >
    <SelectParameters>
        <asp:ControlParameter ControlID="GridView1" Name="Id" PropertyName="SelectedValue" Type="Int32" />
    </SelectParameters>
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
        <asp:Parameter Name="iBannerTimeoutMsecs" Type="Int32" />
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
        <asp:Parameter Name="dtStartDate" DbType="DateTime" ConvertEmptyStringToNull="true" />
        <asp:Parameter Name="dtEndDate" DbType="DateTime" ConvertEmptyStringToNull="true" />
    </UpdateParameters>
</asp:SqlDataSource>