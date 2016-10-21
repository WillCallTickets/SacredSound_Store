<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Bulk_SalePrice_Merch.ascx.cs" Inherits="WillCallWeb.Admin.AdminControls.Bulk_SalePrice_Merch" %>
<%@ Register src="~/Components/Util/CalendarClock.ascx" tagname="CalendarClock" tagprefix="uc1" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<div class="salepricemerch" style="width: 95%;">
    <ul>        
        <li>Enter items that will go on sale. All events must be set to future dates.</li>
        <li>Insert publish events to publish changes oneminute after they are processed. There must be non-publish events to insert a publish event.</li>
    </ul>
    <div style="margin-bottom: 1em;">
        <asp:ValidationSummary HeaderText="The following errors ocurred:" ID="ValidationSummary1" runat="server" 
            ValidationGroup="pdte" CssClass="validationsummary" Width="95%" />
        <asp:ValidationSummary HeaderText="The following errors ocurred:" ID="ValidationSummary3" runat="server" 
            ValidationGroup="processprice" CssClass="validationsummary" Width="95%" />
        <table border="0" cellspacing="3" cellpadding="0" width="100%" style="background-color: #f1f1f1;">
            <tr><th colspan="7" class="headerlabel">Bulk Edit:&nbsp;</th></tr>
            <tr>
                <th class="headerlabel">Date:&nbsp;</th>
                <td style="white-space: nowrap;">
                    <asp:LinkButton ID="btnApplyDate" runat="server" ValidationGroup="pdte" CssClass="btnadmin" Text="Apply To Selections" onclick="btnApplyDate_Click" />
                    <asp:CustomValidator ID="CustomProcess" runat="server" ValidationGroup="pdte" CssClass="validator" ValidateEmptyText="true"
                        display="Static" 
                        ControlToValidate="cclProcess" OnServerValidate="ValidateDateGreaterThanNow" ErrorMessage="Please enter a date in the future (required).">*</asp:CustomValidator>  
                </td>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <uc1:CalendarClock ID="cclProcess" ValidationGroup="pdte" UseTime="true" UseReset="false" runat="server" DateQualifierText="Process date" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <th class="headerlabel">Price:&nbsp;</th>
                <td style="white-space: nowrap;">
                     <asp:LinkButton ID="btnApplyPrice" runat="server" ValidationGroup="processprice" 
                        CssClass="btnadmin" Text="Apply To Selections" onclick="btnApplyPrice_Click" />
                    <asp:RequiredFieldValidator Display="dynamic" CssClass="validation" ValidationGroup="processprice" 
                        ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please enter a price." 
                        ControlToValidate="txtPrice">*</asp:RequiredFieldValidator>
                    <asp:CompareValidator Display="dynamic" CssClass="validation"  ValidationGroup="processprice" 
                        ID="CompareValidator6" runat="server" ErrorMessage="Please enter a numeric quantity."
                        ControlToValidate="txtPrice" Operator="DataTypeCheck" Type="Double">*</asp:CompareValidator>
                    <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="txtPrice"
                        CssClass="validation" ErrorMessage="Please enter a positive value for price." Display="dynamic"
                        MaximumValue="1000" MinimumValue="0" Type="Double" ValidationGroup="processprice">*</asp:RangeValidator> 
                </td>
                <td>
                    <asp:TextBox ID="txtPrice" runat="server" MaxLength="10" Width="125px" />
                   </td>
                <td style="width: 100%;">&nbsp;</td>
            </tr>
        </table>
    </div>        
    <fieldset style="width: 95%;">
        <legend style="font-weight: bold;font-size: 1.4em;color: #9e550c;display: inline;">Merchandise Sale Price Update</legend>
        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
            <ContentTemplate>
                <div style="margin-bottom: 1em;">
                    <asp:ValidationSummary HeaderText="The following errors ocurred:" ID="ValidationSummary2" runat="server" 
                        ValidationGroup="form" CssClass="validationsummary" Width="95%" />
                    <asp:FormView ID="FormView1" runat="server" DataKeyNames="Id" Width="100%" 
                        DataSourceID="SqlEvent" OnItemInserting="FormView1_ItemInserting" 
                        OnItemUpdating="FormView1_ItemUpdating" 
                        OnItemUpdated="FormView1_ItemUpdated" DefaultMode="Edit" OnItemCommand="FormView1_ItemCommand" 
                        ondatabound="FormView1_DataBound" onmodechanging="FormView1_ModeChanging" >
                        <EmptyDataTemplate>
                            <asp:LinkButton ID="btnInsert" CssClass="btnadmin" runat="server" CommandName="New" Text="New Item" CausesValidation="false" />
                        </EmptyDataTemplate>
                        <EditItemTemplate>
                            <div style="white-space: nowrap;">
                                <asp:LinkButton ID="btnCancel" CssClass="btnadmin" runat="server" CommandName="Cancel" Text="Cancel" CausesValidation="false" />
                                <asp:LinkButton ID="btnUpdate" CssClass="btnadmin" runat="server" CommandName="Update" ValidationGroup="form" Text="Save Changes" />
                                <asp:LinkButton ID="btnInsert" CssClass="btnadmin" runat="server" CommandName="New" Text="Add New Item" CausesValidation="false" />
                                <asp:LinkButton ID="btnPublish" CssClass="btnadmin" runat="server" CommandName="Publish" CausesValidation="false" Text=" Add Publish" />
                            </div>
                            <table border="0" cellspacing="3" cellpadding="0" width="100%" style="border: solid #333 1px;background-color: #FEE6c6;">
                                <tr>
                                    <th class="headerlabel" style="vertical-align: text-top;">Start Date:</th>
                                    <td style="white-space: nowrap;">
                                        <uc1:CalendarClock ID="cclStartDate" SelectedDate='<%#Bind("DateToProcess") %>' 
                                            ValidationGroup="form" UseTime="true" UseReset="false" runat="server" DateQualifierText="Process date" />
                                        <asp:CustomValidator ID="CustomProcess" runat="server" ValidationGroup="form" CssClass="validator" ValidateEmptyText="true"
                                            display="Static" ControlToValidate="cclStartDate" OnServerValidate="ValidateDateGreaterThanNow" 
                                            ErrorMessage="Please enter a date in the future (required).">*</asp:CustomValidator>  
                                    </td>
                                    <th class="headerlabel" style="vertical-align: text-top; padding-left: 2em;">Sale Price: $</th>
                                    <td style="white-space: nowrap;">
                                        <asp:TextBox ID="txtPrice" runat="server" Text='<%#Bind("NewValue") %>' MaxLength="10" Width="65px" />
                                        <asp:RequiredFieldValidator Display="dynamic" CssClass="validation" ValidationGroup="form" 
                                            ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please enter a price." 
                                            ControlToValidate="txtPrice">*</asp:RequiredFieldValidator>
                                        <asp:CompareValidator Display="dynamic" CssClass="validation"  ValidationGroup="form" 
                                            ID="CompareValidator6" runat="server" ErrorMessage="Please enter a numeric quantity."
                                            ControlToValidate="txtPrice" Operator="DataTypeCheck" Type="Double">*</asp:CompareValidator>
                                         <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="txtPrice"
                                            CssClass="validation" ErrorMessage="Please enter a positive value for price." Display="dynamic"
                                            MaximumValue="1000" MinimumValue="0" Type="Double" ValidationGroup="form">*</asp:RangeValidator>  
                                    </td>
                                    <th class="headerlabel" style="vertical-align: text-top; padding-left: 2em;">Desc:</th>
                                    <td style="width: 80%;"><%#Eval("Description") %></td>
                                </tr>
                            </table>
                        </EditItemTemplate>
                        <InsertItemTemplate>
                            <div style="white-space: nowrap;">
                                <asp:LinkButton ID="btnCancel" CssClass="btnadmin" runat="server" CommandName="Cancel" Text="Cancel" CausesValidation="false" />
                                <asp:LinkButton ID="btnInsert" CssClass="btnadmin" runat="server" CommandName="Insert" ValidationGroup="form" Text="Add Item" />
                                <a href="javascript: alert('This will add an item using the form below.')" class="infomark">?</a>
                                <asp:LinkButton ID="btnPublish" CssClass="btnadmin" runat="server" CommandName="Publish" CausesValidation="false" Text=" Add Publish" />
                                <a href="javascript: alert('This will add a publish item. It will automatically set itself to publish 1 minute after the last item in the list.')" class="infomark">?</a>
                            </div>
                            <table border="1" cellspacing="3" cellpadding="0" width="100%">
                                <tr>
                                    <th class="headerlabel" style="white-space: nowrap;">Start Date:
                                        <asp:CustomValidator ID="CustomProcess" runat="server" ValidationGroup="form" CssClass="validator" ValidateEmptyText="true"
                                            display="Static" ControlToValidate="cclProcessor" OnServerValidate="ValidateDateGreaterThanNow" 
                                            ErrorMessage="Please enter a date in the future (required).">*</asp:CustomValidator>  
                                    </th>
                                    <td style="white-space: nowrap;">
                                        <uc1:CalendarClock ID="cclProcessor" SelectedDate='<%#Bind("DateToProcess") %>' 
                                            ValidationGroup="form" UseTime="true" UseReset="false" runat="server" DateQualifierText="Start date" />
                                    </td>
                                    <th class="headerlabel">Merch:</th>
                                    <td>
                                        <asp:DropDownList ID="ddlMerchParents" runat="server" OnDataBinding="ddlMerchParents_DataBinding" OnDataBound="ddlMerchParents_DataBound"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddlMerchParents_SelectedIndexChanged" />
                                        <asp:RequiredFieldValidator ID="reqDdl" runat="server" Display="Static" ValidationGroup="form" ControlToValidate="ddlMerchParents" 
                                            InitialValue="0" SetFocusOnError="true" ErrorMessage="Please select a merchandise item.">*</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <th class="headerlabel">Current:</th>
                                    <td style="width: 30px;"><asp:Label ID="lblCurrentPrice" runat="server" /></td>
                                    <th class="headerlabel">Sale Price:</th>
                                    <td>
                                        <asp:TextBox ID="txtPrice" runat="server" MaxLength="10" Width="100px" Text='<%#Bind("NewValue")%>'></asp:TextBox>
                                        <asp:RequiredFieldValidator Display="dynamic" CssClass="validation" ValidationGroup="form" 
                                            ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please enter a price." 
                                            ControlToValidate="txtPrice">*</asp:RequiredFieldValidator>
                                        <asp:CompareValidator Display="dynamic" CssClass="validation"  ValidationGroup="form" 
                                            ID="CompareValidator6" runat="server" ErrorMessage="Please enter a numeric quantity."
                                            ControlToValidate="txtPrice" Operator="DataTypeCheck" Type="Double">*</asp:CompareValidator>
                                         <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="txtPrice"
                                            CssClass="validation" ErrorMessage="Please enter a positive value for price." Display="dynamic"
                                            MaximumValue="1000" MinimumValue="0" Type="Double" ValidationGroup="form">*</asp:RangeValidator>  
                                    </td>
                                </tr>
                            </table>
                        </InsertItemTemplate>
                    </asp:FormView>
                </div>
                <asp:GridView ID="GridView1" EnableViewState="false" Width="100%" runat="server" AutoGenerateColumns="False" 
                    ShowHeader="true" ShowFooter="true" DataSourceID="Sql_Events" 
                            OnRowDataBound="GridView1_RowDataBound" OnRowCommand="GridView1_RowCommand"
                    DataKeyNames="Id, DateToProcess" 
                            OnSelectedIndexChanged="GridView1_SelectedIndexChanged" 
                            ondatabound="GridView1_DataBound" >
                    <HeaderStyle cssclass="center" HorizontalAlign="Left" />
                    <SelectedRowStyle BackColor="#FEE6C6" />
                    <EmptyDataTemplate>
                        <div class="lstempty">No Scheduled Events</div>
                    </EmptyDataTemplate>
                    <Columns>
                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderStyle-Wrap="false" ItemStyle-HorizontalAlign="Center">
                            <HeaderTemplate>
                                <asp:LinkButton ID="btnSelectAll" CssClass="btnadmin" runat="server" OnClick="btnSelectAll_Click" Text="Select All" />
                                <asp:LinkButton ID="btnClear" runat="server" CssClass="btnadmin" OnClick="btnClear_Click" Text="Clear All" />
                            </HeaderTemplate>
                            <FooterTemplate>
                            </FooterTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkSelect" runat="server" AutoPostBack="true" OnCheckedChanged="chkSelect_CheckChanged" />
                                &nbsp;<%#Eval("Id") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="DateToProcess" HeaderText="DateToProcess" ItemStyle-Wrap="false" ItemStyle-Width="15%" />
                        <asp:BoundField DataField="Verb" HeaderText="Context" ItemStyle-Width="15%" />
                        <asp:BoundField DataField="OldValue" HeaderText="OldValue" ItemStyle-Width="7%" />
                        <asp:BoundField DataField="NewValue" HeaderText="NewValue" ItemStyle-Width="7%" />
                        <asp:BoundField DataField="Description" HeaderText="Description" ItemStyle-Width="40%" />
                        <asp:TemplateField ItemStyle-Wrap="false">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnDelete" runat="server" CssClass="btnadmin" CausesValidation="false" CommandName="Delete" Text="Delete" />
                                <asp:LinkButton ID="btnUp" CssClass="btnadmin" runat="server" CommandName="Up" CommandArgument='<%#Eval("Id") %>' Text="&#9650;"></asp:LinkButton>
                                <asp:LinkButton ID="btnDown" CssClass="btnadmin" runat="server" CommandName="Down" CommandArgument='<%#Eval("Id") %>' Text="&#9660;"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
           </ContentTemplate>
      </asp:UpdatePanel>
   </fieldset>
</div>
<asp:SqlDataSource ID="SqlEvent" runat="server" ConnectionString="<%$ ConnectionStrings:WillCallConnectionString %>"
    SelectCommand="SELECT eq.* FROM [EventQ] eq WHERE eq.[ApplicationId] = @appId AND eq.[Id] = @Id"
    InsertCommand="DECLARE @userId uniqueidentifier; SELECT @userId = u.[UserId] FROM [Aspnet_Users] u 
        WHERE u.[ApplicationId] = @appId AND u.[UserName] = @creatorName; 
        INSERT [EventQ]([ApplicationId], [DateToProcess], [AttemptsRemaining], [iPriority], [CreatorId], [CreatorName], [Context], [Verb], 
            [OldValue], [NewValue], [Description], [IP]) 
        VALUES (@appId, @dateToProcess, 3, 0, @userId, @creatorName, @context, @verb, @oldValue, @newValue, @description, @ip) 
        SELECT @newId = SCOPE_IDENTITY() "
    UpdateCommand="UPDATE [EventQ] SET [DateToProcess] = @DateToProcess, [NewValue] = @NewValue WHERE [Id] = @Id; "
    OnInserted="SqlEvent_Inserted" oninserting="SqlEvent_Inserting" onSelecting="SqlEvent_Selecting" 
>
    <SelectParameters>
        <asp:Parameter Name="appId" DbType="Guid" />
        <asp:ControlParameter ControlID="GridView1" DefaultValue="0" Name="Id" PropertyName="SelectedValue" Type="Int32" />
    </SelectParameters>
    <InsertParameters>
        <asp:Parameter Name="appId" DbType="Guid" />
        <asp:Parameter DefaultValue="" Name="DateToProcess" Type="DateTime" />
        <asp:ProfileParameter Name="creatorName" PropertyName="UserName" Type="String" />
        <asp:Parameter DefaultValue="" Name="context" Type="String" />
        <asp:Parameter DefaultValue="" Name="verb" Type="String" />
        <asp:Parameter DefaultValue="" Name="oldValue" Type="String" />
        <asp:Parameter DefaultValue="" Name="NewValue" Type="String" />
        <asp:Parameter DefaultValue="" Name="description" Type="String" />
        <asp:Parameter DefaultValue="" Name="ip" Type="String" />
        <asp:Parameter Name="newId" Type="Int32" DefaultValue="345" Direction="InputOutput" />
    </InsertParameters>
    <UpdateParameters>
        <asp:Parameter DefaultValue="" Name="DateToProcess" Type="DateTime" />
        <asp:Parameter DefaultValue="" Name="NewValue" Type="String" />
        <asp:ControlParameter ControlID="FormView1" DefaultValue="0" Name="Id" PropertyName="SelectedValue" Type="Int32" />
    </UpdateParameters>
</asp:SqlDataSource>

<asp:SqlDataSource ID="Sql_Events" runat="server" ConnectionString="<%$ ConnectionStrings:WillCallConnectionString %>"
    OnSelecting="Sql_Events_Selecting"
    DeleteCommand="DELETE FROM [EventQ] WHERE [Id] = @Id"
    SelectCommand="SELECT DISTINCT TOP 1000 eq.* FROM [EventQ] eq 
        WHERE eq.[ApplicationId] = @appId AND eq.[Threadlock] IS NULL AND eq.[DateProcessed] IS NULL AND 
            ISNULL(eq.[AttemptsRemaining],1) > 0 AND eq.[DateToProcess] > ((GETDATE())) AND eq.[Context] = @context AND (eq.[Verb] = @verb OR eq.[Verb] = @publish) ORDER BY eq.[DateToProcess]; ">
    <SelectParameters>
        <asp:Parameter Name="appId" DbType="Guid" />
        <asp:Parameter DefaultValue="" Name="context" Type="String" />
        <asp:Parameter DefaultValue="" Name="verb" Type="String" />
        <asp:Parameter DefaultValue="Publish" Name="publish" Type="String" />
    </SelectParameters>
    <DeleteParameters>
        <asp:Parameter Name="Id" Type="Int32" />
    </DeleteParameters>
</asp:SqlDataSource>