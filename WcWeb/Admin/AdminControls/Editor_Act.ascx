<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Editor_Act.ascx.cs" Inherits="WillCallWeb.Admin.AdminControls.Editor_Act" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<div id="srceditor">
    <div id="acteditor">
        <div class="jqhead rounded">
        <%if (DisplayTitle)
          {%>
        <div class="sectitle"><%=TitleText %></div>
        <%} %>
            <asp:ValidationSummary ID="ValidationSummary2" CssClass="validationsummary" HeaderText="Please fix the following errors:"
                ValidationGroup="srceditor" runat="server" />
            <asp:Wizard ID="wizEdit" runat="server" CellPadding="0" CellSpacing="0" SkipLinkText="" Width="100%"
                OnSideBarButtonClick="OnNextStep" 
                OnActiveStepChanged="wizEdit_ActiveStepChanged" >
                <FinishNavigationTemplate>
                </FinishNavigationTemplate>
                <StartNavigationTemplate>
                </StartNavigationTemplate>
                <StepNavigationTemplate>
                </StepNavigationTemplate>
                <StepStyle HorizontalAlign="Left" Width="100%" />
                <SideBarStyle VerticalAlign="Top" Width="1%" />
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
                    <asp:WizardStep ID="Editor" runat="server" Title="Details">
                        <asp:Panel ID="pnlSelect" runat="server">
                            <table border="0" cellpadding="0" cellspacing="0" width="100%" class="edittabl">
                                <tr>
                                    <th>
                                        <asp:CustomValidator ID="CustomValidation" runat="server" ValidationGroup="srceditor" CssClass="validator" Display="Static"
                                            ErrorMessage="CustomValidator">*</asp:CustomValidator>
                                    </th>
                                    <th class="select-txt"><%=SelectText %></th>
                                    <td>
                                        <asp:TextBox ID="txtSelection" runat="server" Width="300px" MaxLength="256" />
                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" TargetControlID="txtSelection" ServicePath="~/Services/Admin/SuggestionService.asmx"
                                            ServiceMethod="Name_Suggestions" MinimumPrefixLength="1" CompletionSetCount="25" CompletionInterval="1000"
                                            FirstRowSelected="true" OnClientItemSelected="ActItemSelected" UseContextKey="true" ContextKey="Act">
                                        </cc1:AutoCompleteExtender>
                                    </td>
                                    <td style="vertical-align:middle;white-space:nowrap;width:100%;">
                                        <asp:RadioButtonList ID="rdoSearch" runat="server" CellPadding="0" CellSpacing="0" AutoPostBack="true" CausesValidation="false"
                                            RepeatDirection="Horizontal" RepeatLayout="Flow" OnSelectedIndexChanged="rdoSearch_SelectedIndexChanged" OnLoad="rdoSearch_Load">
                                            <asp:ListItem Text="Like" Value="true" />
                                            <asp:ListItem Text="Alpha" Value="false" />
                                        </asp:RadioButtonList>    
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:FormView ID="FormView1" runat="server" DataKeyNames="Id" DataSourceID="SqlDetails" Width="100%" OnItemInserted="FormView1_ItemInserted"
                            OnItemInserting="FormView1_ItemInserting" 
                            OnItemUpdating="FormView1_ItemUpdating" 
                            OnItemDeleted="FormView1_ItemDeleted"
                            OnItemUpdated="FormView1_ItemUpdated" 
                            OnDataBound="FormView1_DataBound"
                            OnItemDeleting="FormView1_ItemDeleting">
                            <EmptyDataTemplate>
                                <div class="cmdsection">
                                    <asp:Button ID="btnNew" CssClass="btnmed" CausesValidation="false" runat="server" 
                                        CommandName="New" Text="CREATE NEW ACT" />
                                </div>
                            </EmptyDataTemplate>
                            <EditItemTemplate>
                                <div class="edittabl-wrapper">
                                    <table border="0" cellspacing="0" cellpadding="0" width="100%" class="edittabl">
                                        <tr>
                                            <th><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Display="static" 
                                                    CssClass="validator" ControlToValidate="NameTextBox"
                                                    ValidationGroup="srceditor" ErrorMessage="Name is required">*</asp:RequiredFieldValidator>
                                            </th>
                                            <th><span class="intr"><%# Eval("Id") %></span> Name</th>
                                            <td style="white-space:nowrap;"><asp:TextBox ID="NameTextBox" MaxLength="256" Width="350px" runat="server" Text='<%# Bind("Name") %>' />
                                            </td>
                                            <td rowspan="4" style="width:100%;vertical-align:top;padding-left:48px;">                                            
                                                <asp:Literal ID="litImgThumb" runat="server" OnDataBinding="litImageThumb_DataBinding" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <th colspan="2">Display Name</th>
                                            <td>
                                                <asp:TextBox ID="DisplayNameTextBox" MaxLength="256" Width="350px" runat="server" Text='<%# Bind("DisplayName") %>' />
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>
                                                <asp:RegularExpressionValidator ID="regexWebsite" ValidationGroup="srceditor" Display="Static" 
                                                    CssClass="validator" runat="server" ControlToValidate="WebsiteTextBox" 
                                                    ErrorMessage="Please enter a valid url">*</asp:RegularExpressionValidator>
                                            </th>
                                            <th>Website</th>
                                            <td style="white-space:nowrap;">
                                                <asp:TextBox ID="WebsiteTextBox" MaxLength="256" Width="350px" runat="server" 
                                                    Text='<%# Bind("Website") %>' OnTextChanged="WebsiteTextBox_TextChanged" />
                                                <asp:HyperLink ID="linkTestWebsite" runat="server" Target="_blank" CssClass="btntst" 
                                                    NavigateUrl="" OnDataBinding="linkTestWebsite_DataBinding">test</asp:HyperLink>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th colspan="2">Image Url</th>
                                            <td><input type="text" readonly="readonly" style="width:350px;" value='<%#Eval("PictureUrl") %>' /></td>
                                        </tr>
                                    </table>
                                </div>
                                <div class="cmdsection">
                                    <asp:Button ID="UpdateButton" runat="server" CausesValidation="True" ValidationGroup="srceditor" 
                                        CommandName="Update" Text="Update" CssClass="btnmed" />
                                    <asp:Button ID="UpdateCancelButton" runat="server" CausesValidation="False" CommandName="Cancel" 
                                        Text="Cancel" CssClass="btnmed" />
                                </div>
                            </EditItemTemplate>
                            <InsertItemTemplate>
                                <div class="edittabl-wrapper">
                                    <table border="0" cellspacing="0" cellpadding="0" width="100%" class="edittabl">
                                        <tr>
                                            <th><asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="srceditor" runat="server" Display="static"
                                                    CssClass="validator" ControlToValidate="NameTextBox" ErrorMessage="Name is required">*</asp:RequiredFieldValidator>
                                            </th>
                                            <th style="white-space:nowrap;vertical-align:middle;">Name</th>
                                            <td style="width:100%;">
                                                <asp:TextBox ID="NameTextBox" MaxLength="256" Width="350px" runat="server" Text='<%# Bind("Name") %>' />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div class="cmdsection">
                                    <asp:Button ID="InsertButton" runat="server" ValidationGroup="srceditor" CausesValidation="True" 
                                        CommandName="Insert" Text="CREATE NEW ACT" CssClass="btnmed" />
                                    <asp:Button ID="InsertCancelButton" runat="server" CausesValidation="False" CommandName="Cancel" 
                                        Text="Cancel" CssClass="btnmed" />
                                </div>
                            </InsertItemTemplate>        
                            <ItemTemplate>
                                <div class="edittabl-wrapper">
                                    <table border="0" cellspacing="0" cellpadding="0" width="100%" class="edittabl">
                                        <tr>
                                            <th>&nbsp;</th>
                                            <th><span style="padding-left:12px;" class="intr"><%# Eval("Id")%></span> Name</th>
                                            <td colspan="2">
                                                <asp:TextBox ID="NameTextBox" ReadOnly="true" MaxLength="256" Width="350px" runat="server" Text='<%# Eval("Name") %>' />
                                            </td>
                                        </tr>
                                        <tr>
                                            <th colspan="2">Display Name</th>
                                            <td>
                                                <asp:TextBox ID="txtDisplayName" ReadOnly="true" MaxLength="256" Width="350px" runat="server" Text='<%# Eval("DisplayName") %>' />    
                                            </td>
                                            <td rowspan="3" style="width:100%;vertical-align:top;padding-left:48px;">                                            
                                                <asp:Literal ID="litImgThumb" runat="server" OnDataBinding="litImageThumb_DataBinding" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <th colspan="2">Website</th>
                                            <td>
                                                <asp:HyperLink ID="btnWebsiteUrl" runat="server" Target="_blank" CssClass="btntst" Text='<%#Eval("Website") %>' />&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <th colspan="2">Image Url</th>
                                            <td><input type="text" readonly="readonly" style="width:350px;" value='<%#Eval("PictureUrl") %>' /></td>
                                        </tr>
                                    </table>
                                </div>
                                <div class="cmdsection">
                                    <asp:Button Id="EditButton" ToolTip="Edit" CssClass="btnmed" runat="server" CommandName="Edit" 
                                        Text="Edit" CommandArgument='<%#Eval("Id") %>' CausesValidation="false" />
                                    <asp:Button Id="DeleteButton" CssClass="btnmed" runat="server" CommandName="Delete" 
                                       Text="Delete" CommandArgument='<%#Eval("Id") %>' ToolTip="Delete" CausesValidation="false"
                                       OnClientClick='return confirm("Are you sure you want to delete this ACT?")' />
                                    <asp:Button Id="NewButton" ToolTip="New" CssClass="btnmed" runat="server" CommandName="New" 
                                        Text="New" CommandArgument='<%#Eval("Id") %>' CausesValidation="false" />
                                </div>
                                <asp:Panel ID="pnlShowList" runat="server" class="show-list" OnDataBinding="pnlShowList_DataBinding">
                                    <h4>Show Listing:</h4>                                
                                    <asp:Repeater ID="rptShowList" runat="server" DataSourceID="SqlShowList" OnItemCommand="rptShowList_ItemCommand" >
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkShow" runat="server" CommandName="viewshow" CommandArgument='<%# Eval("ShowId") %>' Text='<%# Eval("ShowName") %>' />
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </asp:Panel>
                            </ItemTemplate>
                        </asp:FormView>
                        <asp:Panel ID="PanelHidden" runat="server">
                            <input type="hidden" id="hidSelectedValue" name="hidSelectedValue" runat="server" />
                            <asp:Button ID="btnLoad" runat="server" Text="Load/Create" CssClass="btnhid" OnClick="btnLoad_Click" />
                        </asp:Panel>
                    </asp:WizardStep>
                    <asp:WizardStep ID="Images" runat="server" Title="Set Image">
                        <div class="edittabl-wrapper">
                            <table border="0" cellspacing="0" cellpadding="0" width="100%" class="edittabl">                            
                                <tr>
                                    <th>Name</th>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtImageName" ReadOnly="true" MaxLength="256" Width="350px" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <th>Image Url</th>
                                    <td colspan="2"><input type="text" readonly="readonly" style="width:350px;" value='<%= Entity.PictureUrl %>' /></td>
                                </tr>
                                <tr>
                                    <th style="vertical-align:top;padding-top:24px;">Current Image</th>
                                    <td><asp:Literal ID="litImage" runat="server" OnDataBinding="litImage_DataBinding" /><br /></td>
                                    <td style="vertical-align:top;padding-top:18px;">
                                        <asp:Panel ID="pnlInstruction" runat="server" cssclass="jqinstruction rounded">
                                            <ul>
                                                <li>Allows you to create a SQUARE version of an image.</li>
                                                <li>Saving a cropped version of the image will delete the original.</li>
                                                <li style="padding:6px 0 12px 0;">
                                                    <asp:Button ID="btnSaveCrop" runat="server" Text="Save Cropped" CssClass="btnmed" width="100px"
                                                        OnClick="btnCrop_Click" CommandName="savecrop" CausesValidation="false" 
                                                        OnClientClick="return confirm('This will create a new image frmo the original but will also DELETE the original. Would you like to proceed?');" />
                                                </li>
                                                <li><asp:Button ID="btnCancelCrop" runat="server" Text="Cancel" CssClass="btnmed" width="100px"
                                                        OnClick="btnCrop_Click" CommandName="cancelcrop" CausesValidation="false" />
                                                </li>
                                            </ul>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <th style="padding-top:6px;">Image Upload</th>
                                    <td colspan="2">
                                        <asp:FileUpload ID="uplPicture" runat="server" Width="350px" CssClass="btnmed" />
                                        <asp:CustomValidator ID="CustomUpload" runat="server" CssClass="validator" 
                                            ValidationGroup="srceditor" Display="Static">*</asp:CustomValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <th>
                                        <br />
                                        <asp:Button ID="btnThumbnails" runat="server" Text="Repair Thumbnails" CssClass="btnmed" Width="100px" OnClick="btnThumbnails_Click" 
                                            CommandName="thumbs" CausesValidation="false" />
                                    </th>
                                    <td colspan="2"><asp:Label ID="lblAlert" runat="server" ForeColor="Green" /></td>
                                </tr>
                            </table>
                        </div>
                        <div class="cmdsection" >
                            <asp:Button ID="btnUpload" runat="server" Text="Upload" CssClass="btnmed btnupload" OnClick="btnUpload_Click" 
                                CommandName="upload" CausesValidation="false" />
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btnmed" OnClick="btnCancel_Click" 
                                CommandName="cancelupload" CausesValidation="false" />
                            <asp:Button ID="btnClear" runat="server" CssClass="btnmed" Text="Clear Image" OnClick="btnClear_Click"
                                OnClientClick='return confirm("Are you sure you want to delete this image?")' CausesValidation="false" />
                        </div>
                        <input type="hidden" id="x1" name="x1" />
                        <input type="hidden" id="y1" name="y1" />
                        <input type="hidden" id="x2" name="x2" />
                        <input type="hidden" id="y2" name="y2" />
                        <input type="hidden" id="w1" name="w1" />
                        <input type="hidden" id="h1" name="h1" />
                    </asp:WizardStep>
                </WizardSteps>
            </asp:Wizard>
      </div>
      <div style="visibility:hidden;"><asp:FileUpload ID="FileUploadRegisterControl" runat="server" Width="350px" CssClass="btnmed" /></div>
    </div>
</div>
<asp:SqlDataSource ID="SqlDetails" runat="server" ConnectionString="<%$ ConnectionStrings:WillCallConnectionString %>" EnableCaching="false"
    DeleteCommand="DELETE FROM [Act] WHERE [Id] = @Id; " InsertCommand="INSERT INTO [Act] ([ApplicationId], [Name]) VALUES (@appId, @Name); SELECT @NewId = @@IDENTITY; "
    SelectCommand="SELECT [Id], [Name], [DisplayName], [Website], [PictureUrl] FROM [Act] WHERE ([ApplicationId] = @appId AND [Id] = @Id); "
    UpdateCommand="UPDATE [Act] SET [Name] = @Name, [DisplayName] = @DisplayName, [Website] = @Website WHERE [Id] = @Id "
    OnInserted="SqlDetails_Inserted" OnInserting="SqlDetails_Inserting" OnSelecting="SqlDetails_Selecting" >
    <DeleteParameters>
        <asp:Parameter Name="Id" Type="Int32" />
    </DeleteParameters>
    <UpdateParameters>
        <asp:Parameter Name="Name" Type="String" />
        <asp:Parameter Name="DisplayName" Type="String" />
        <asp:Parameter Name="Website" Type="String" />
        <asp:Parameter Name="Id" Type="Int32" />
    </UpdateParameters>
    <SelectParameters>
        <asp:Parameter Name="appId" DbType="Guid" />
        <asp:CookieParameter ConvertEmptyStringToNull="false" CookieName="acid" DefaultValue="0" Name="Id" Type="int32" />
    </SelectParameters>
    <InsertParameters>
        <asp:Parameter Name="appId" DbType="Guid" />
        <asp:Parameter Name="NewId" Direction="output" DefaultValue="567" Type="Int32" />
        <asp:Parameter Name="Name" Type="String" />
    </InsertParameters>
</asp:SqlDataSource>
<asp:SqlDataSource ID="SqlShowList" runat="server" ConnectionString="<%$ ConnectionStrings:WillCallConnectionString %>"
    SelectCommand="SELECT DISTINCT s.[Id] as [ShowId], s.[Name] as [ShowName] FROM [JShowAct] jsa LEFT OUTER JOIN [ShowDate] sd ON sd.[Id] = jsa.[tShowDateId] LEFT OUTER JOIN [Show] s ON s.[Id] = sd.[tShowId] WHERE jsa.[tActId] = @Idx ORDER BY s.[Name] DESC "
    >    
    <SelectParameters>
        <asp:CookieParameter ConvertEmptyStringToNull="false" CookieName="acid" DefaultValue="0" Name="Idx" Type="int32" />
    </SelectParameters>
</asp:SqlDataSource>