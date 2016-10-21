<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Editor_HeaderImage.ascx.cs" Inherits="WillCallWeb.Admin.AdminControls.Editor_HeaderImage" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register src="~/Components/Util/CalendarClock.ascx" tagname="CalendarClock" tagprefix="uc1" %>
<%@ Register src="~/Components/Navigation/gglPager.ascx" tagname="gglPager" tagprefix="uc2" %>
<div id="srceditor">
    <div id="headerimage">

        <uc2:gglPager ID="GooglePager1" runat="server" PageButtonClass="btnmed" PagerTitle="Header Images" >
            <Template>
                <asp:CustomValidator ID="CustomValidator1" runat="server" CssClass="invisible" Display="Static" ValidationGroup="headimage" 
                    ErrorMessage="CustomValidator">*</asp:CustomValidator>
            </Template>
        </uc2:gglPager>

        <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="validationsummary" 
             ValidationGroup="headimage" HeaderText="" />
             
        <table border="0" cellspacing="0" cellpadding="0" >
            <tr>
                <td style="width:550px;padding-left:0;">
                    <asp:FormView ID="FormView1" runat="server" DefaultMode="Edit" DataKeyNames="Id,FileName" Width="100%" 
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
                            <div class="jqedt rounded">
                                <div class="cmdsection">
                                    <asp:Button ID="btnNew" runat="server" CssClass="btnmed" Text="Add New Header Image" CommandName="New" CausesValidation="false" />
                                </div>
                            </div>
                        </EmptyDataTemplate>
                        <InsertItemTemplate>
                            <div class="jqpnl rounded">
                                <h3 class="entry-title">Adding a new Header Image...</h3>
                                <table border="0" cellspacing="0" cellpadding="0" width="100%" class="edittabl">
                                    <tr>
                                        <td colspan="3" class="intr"><%=Wcss._Config._HeaderImageDimensionText%></td>
                                    </tr>
                                    <tr>
                                        <th>&nbsp;
                                        </th>
                                        <th>FileName</th>
                                        <td style="width:100%;">
                                            <asp:FileUpload ID="FileUpload1" runat="server" Width="350px" CssClass="btnmed" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <th></th>
                                        <th style="vertical-align:top;"><a href="javascript: alert('Displayed as a title in the order flow when images are not available. Not required - but highly recommended.')" class="infomark">?</a>
                                            Display Text</th>
                                        <td><asp:TextBox ID="txtDisplay" runat="server" Text='<%#Bind("DisplayText") %>' TextMode="MultiLine" Width="100%" Height="40px" MaxLength="1000"/></td>
                                    </tr>
                                </table>
                                <div class="cmdsection">
                                    <asp:Button ID="btnSaveInsert" runat="server" CssClass="btnmed" Text="Save" CommandName="Insert" 
                                        CausesValidation="true" ValidationGroup="headimage"  />
                                    <asp:Button ID="btnCancel" runat="server" CssClass="btnmed" Text="Cancel" CommandName="Cancel" CausesValidation="false" />
                                </div>
                            </div>
                        </InsertItemTemplate>
                        <EditItemTemplate>
                            <div class="jqpnl rounded">
                                <h3 class="entry-title"><%#Eval("Id") %> - <%#Eval("FileName") %></h3>
                                <div style="padding:6px;">
                                    <asp:Literal ID="litBigImage" runat="server" />
                                </div>
                                <div class="cmdsection">
                                    <asp:Button ID="btnSave22" runat="server" CssClass="btnmed" Text="Save" CommandName="Update" 
                                        CausesValidation="true" ValidationGroup="headimage"  />
                                    <asp:Button ID="btnCancel22" runat="server" CssClass="btnmed" Text="Cancel" CommandName="Cancel" CausesValidation="false" />
                                    <asp:Button ID="btnNew22" runat="server" CssClass="btnmed" Text=" New " CommandName="New" CausesValidation="false" />
                                </div>
                                <table border="0" cellspacing="0" cellpadding="0" width="100%" class="edittabl" >
                                    <tr>
                                        <td colspan="3" class="jqinstruction rounded">
                                        <%=Wcss._Config._HeaderImageDimensionText%> 
                                        </td>
                                    </tr>
                                    <tr>
                                        <th colspan="2" style="vertical-align:top;">
                                            <a href="javascript: alert('Valid file names contain letters, numbers, underscores - please do not use parentheses or non-alphanumeric chars in the file name. You may need to rename the original file. Also note that images must be saved in RGB mode (CMYK WILL NOT WORK)')" class="infomark">?</a>
                                            Change Image</th>
                                        <td>
                                            <div style="white-space:nowrap;vertical-align:middle;">
                                                <asp:FileUpload ID="FileUpload1" runat="server" Width="350px" CssClass="btnmed" /> 
                                                <span style="vertical-align:top"><asp:Button ID="btnUpload" runat="server" CssClass="btnmed btnupload" Text="Upload" 
                                                    onclick="btnUpload_Click" CausesValidation="false" /></span>
                                            </div>
                                            
                                        </td>
                                    </tr>
                                    <tr>
                                        <th></th>
                                        <th valign="top">
                                            <a href="javascript: alert('This is used to describe the promotion to the user. Displayed as a title in the order flow when images cannot be displayed')" class="infomark">?</a>
                                            Display Text</th>
                                        <td>
                                            <asp:TextBox ID="txtDisplay" runat="server" Text='<%#Bind("DisplayText") %>' TextMode="MultiLine" Width="500px" Height="24px" MaxLength="1000"/>
                                        </td>
                                    </tr>
                                    <tr><td colspan="3"><hr /></td></tr>
                                    <tr>
                                        <td colspan="3">
                                            <div class="jqinstruction rounded" style="margin-top:3px;">
                                                <ul>
                                                    <li><b>External links:</b> http://www.whatever.com</li>
                                                    <li><b>Internal links:</b>Use create link buttons for appropriate internal navigation. Internal nav does not require domain name.</li>
                                                </ul>
                                            </div>
                                         </td>
                                    </tr>
                                    <tr>
                                        <th colspan="2"><a href="javascript: alert('You may specify a url to link to when the HeaderImage is clicked.')" class="infomark">?</a>
                                            Navigate Url</th>
                                        <td style="white-space:nowrap;"><asp:TextBox ID="txtClickUrl" runat="server" Text='<%#Bind("NavigateUrl") %>' Width="250px" MaxLength="256" 
                                            OnTextChanged="txtClickUrl_TextChanged" />
                                            &nbsp;
                                            <asp:Literal ID="litUrlTest" runat="server" OnDataBinding="litUrlTest_DataBinding" />
                                            &nbsp;&nbsp;&nbsp;<asp:LinkButton ID="btnClearUrl" runat="server" CommandName="ClearUrl" Text="clear" CausesValidation="false"
                                                OnClientClick="return confirm('Are you sure you want to clear the url?');" />
                                        </td>
                                    <tr>
                                    <tr>
                                        <th colspan="2">
                                            <a href="javascript: alert('This will setup a link to a selected show. You will still need to save to complete the operation.')" class="infomark">?</a>
                                            <asp:Button ID="btnCreateShowLink" runat="server" CommandName="LoadShow" CssClass="btnmed" Width="80px" Text="Create Link" CausesValidation="false"
                                                OnClientClick="return confirm('Are you sure you want to link to the selected show?');" />
                                        </th>
                                        <td>
                                            <asp:DropDownList ID="ddlCreateLinkShowList" runat="server" DataSourceID="SqlShowList" 
                                                DataTextField="ShowName" DataValueField="ShowId" Font-Size="9px" Width="500px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>   
                                    <tr><td colspan="3"><hr /></td></tr>                                    
                                    <tr>
                                        <th colspan="2" valign="top">Display Context</th>
                                        <td style="padding:0 6px;">
                                            <asp:CheckBoxList ID="chkContext" runat="server" AutoPostBack="false" datasource='<%#Enum.GetNames(typeof(Wcss._Enums.HeaderImageContext)) %>'
                                                RepeatLayout="Table" RepeatColumns="3" DataTextFormatString=" {0}" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <th colspan="2">
                                            <a href="javascript: alert('This will ensure that this banner shows on the selected show&#39;s page. You will still need to save to complete the operation.')" class="infomark">?</a>
                                            Show Context
                                        </th>
                                        <td>
                                            <asp:DropDownList ID="ddlContextShowList" runat="server" DataSourceID="SqlShowList"
                                                DataTextField="ShowName" DataValueField="ShowId" Font-Size="9px" Width="500px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th colspan="2">
                                            <a href="javascript: alert('This will setup a link to a selected merch item. You will need to save to completed operation.')" class="infomark">?</a>
                                            Merch Context
                                        </th>
                                        <td>
                                            <asp:DropDownList ID="ddlContextMerchList" runat="server" DataSourceID="SqlMerchList"
                                                DataTextField="ItemName" DataValueField="ItemId" Font-Size="9px" Width="500px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th colspan="2">
                                            <a href="javascript: alert('Moves the display order to a higher priority within its selected contexts.')" class="infomark">?</a>
                                            Has Priority
                                        </th>
                                        <td>
                                            <asp:CheckBox ID="chkPriority" runat="server" Checked='<%#Bind("bDisplayPriority") %>' />
                                        </td>
                                    </tr>
                                    <tr>
                                        <th colspan="2">
                                            <a href="javascript: alert('Specifies if this image shuold override any other images displayed within a context.')" class="infomark">?</a>
                                            Is Exclusive
                                        </th>
                                        <td>
                                            <asp:CheckBox ID="chkIsExclusive" runat="server" Checked='<%#Bind("bExclusive") %>' />
                                        </td>
                                    </tr>
                                    <tr><td colspan="3"><hr /></td></tr>
                                    <tr>
                                        <th colspan="2">Start Date</th>
                                        <td style="padding:0;"><uc1:CalendarClock ID="CalendarClockStart" SelectedDate='<%#Eval("dtStart") %>' IsRequired="false" 
                                                ValidationGroup="headimage" UseTime="true" runat="server" Width="352" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <th colspan="2">End Date</th>
                                        <td style="padding:0;"><uc1:CalendarClock ID="CalendarClockEnd" SelectedDate='<%#Eval("dtEnd") %>' IsRequired="false"
                                                ValidationGroup="headimage" UseTime="true" runat="server" Width="352" />
                                        </td>
                                    </tr>
                                    <tr><td colspan="3"><hr /></td></tr>
                                    <tr>
                                        <th colspan="2"><a href="javascript: alert('The amount of time to display the HeaderImage - IN MILLISECONDS')" class="infomark">?</a>
                                            Display mSecs</th>
                                        <td><asp:TextBox ID="txtTimeout" runat="server" Text='<%#Bind("iTimeoutMsec") %>' Width="250px" MaxLength="6" /></td>
                                    </tr>
                                </table>
                                <div class="cmdsection">
                                    <asp:Button ID="btnSave" runat="server" CssClass="btnmed" Text="Save" CommandName="Update" 
                                        CausesValidation="true" ValidationGroup="headimage"  />
                                    <asp:Button ID="btnCancel" runat="server" CssClass="btnmed" Text="Cancel" CommandName="Cancel" CausesValidation="false" />
                                    <asp:Button ID="btnNew" runat="server" CssClass="btnmed" Text=" New " CommandName="New" CausesValidation="false" />
                                    <asp:Button Id="btnDelete" Cssclass="btnmed" runat="server" CommandName="Delete" 
                                        Text="Delete" ToolTip="Delete" CausesValidation="false"
                                        OnClientClick='return confirm("Are you sure you want to delete this image?")' />                      
                                </div>                                
                            </div>
                        </EditItemTemplate>
                    </asp:FormView>
                </td>
                <td style="width:360px;padding-left:16px;">
                    <div class="jqinstruction rounded" style="margin:0 0 8px 0;">
                        <b style="margin-right:12px;vertical-align:2px;">Randomize Display Order</b>
                        <asp:CheckBox ID="chkRandom" runat="server" Checked="<%#Wcss._Config._HeaderImages_IgnoreOrder %>" AutoPostBack="true" OnCheckedChanged="chkRandom_CheckChanged" />
                    </div>
                    <asp:GridView ID="GridView1" runat="server" DataSourceID="ObjDatum" Width="100%" CssClass="lsttbl" DataKeyNames="Id,FileName" 
                        AllowPaging="True" AutoGenerateColumns="False" GridLines="Both"
                        OnInit="GridView_Init"
                        OnDataBinding="GridView_DataBinding" 
                        OnRowDataBound="GridView_RowDataBound" 
                        onrowcommand="GridView_RowCommand"                        
                        OnDataBound="GridView_DataBound">           
                       <PagerSettings Visible="false" />
                       <SelectedRowStyle CssClass="selected" />
                       <EmptyDataTemplate>
                            <div class="lstempty">No Header Images</div>
                        </EmptyDataTemplate>
                       <Columns>
                           <asp:TemplateField HeaderText="#" ItemStyle-HorizontalAlign="center" ItemStyle-CssClass="rowcounter">
                                <ItemTemplate><asp:Literal ID="LiteralRowCounter" runat="server" /></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Wrap="false" HeaderText="Select" >
                                <ItemTemplate>
                                    <asp:LinkButton Width="20px" Id="btnSelect" ToolTip="Select" CssClass="btn-select" runat="server" CommandName="Select" 
                                        CommandArgument='<%#Eval("Id") %>' CausesValidation="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Image" HeaderStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Literal ID="litNaming" runat="server" />
                                    <asp:Literal ID="litDates" runat="server" />
                                    <asp:Literal ID="litImage" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Wrap="false">
                                <ItemTemplate>
                                    <asp:LinkButton Width="20px" Id="btnUp" ToolTip="Move Up" Cssclass="btn-up" runat="server" CommandName="Up" 
                                        CommandArgument='<%#Eval("Id") %>' CausesValidation="false" />
                                    <asp:LinkButton Width="20px" Id="btnDown" ToolTip="Move Down" Cssclass="btn-down" runat="server" CommandName="Down" 
                                        CommandArgument='<%#Eval("Id") %>' CausesValidation="false" />
                                    <asp:CustomValidator ID="CustomValidation" runat="server" ValidationGroup="headimage" Display="Static" CssClass="validator">*</asp:CustomValidator>
                                </ItemTemplate>
                            </asp:TemplateField>
                       </Columns>
                   </asp:GridView>
                </td>
            </tr>
        </table>
        
        <div style="visibility:hidden;">
            <asp:FileUpload ID="FileUploadRegisterControl" runat="server" Width="350px" CssClass="btnmed" />
            <!-- this control resides here simply to force the page to create the proper mimetype for the fileuploads which may be hidden within a form. -->
        </div>
    </div>
</div>
<asp:SqlDataSource ID="SqlShowList" runat="server" ConnectionString="<%$ ConnectionStrings:WillCallConnectionString %>" EnableCaching="true" CacheDuration="20"
    SelectCommand="SELECT 0"
    onselecting="SqlShowList_Selecting">
    <SelectParameters>
        <asp:Parameter Name="appId" DbType="Guid" />
        <asp:Parameter Name="startDate" Type="DateTime" />
    </SelectParameters>    
</asp:SqlDataSource>
<asp:SqlDataSource ID="SqlMerchList" runat="server" ConnectionString="<%$ ConnectionStrings:WillCallConnectionString %>" EnableCaching="false" CacheDuration="20"
    SelectCommand="SELECT 0"
    onselecting="SqlMerchList_Selecting">
    <SelectParameters>
        <asp:Parameter Name="appId" DbType="Guid" />
    </SelectParameters>    
</asp:SqlDataSource>
<asp:ObjectDataSource ID="ObjDatum" runat="server" EnablePaging="true" 
    SelectMethod="GetHeaderImages" EnableCaching="false" 
    TypeName="Wcss.HeaderImage" SelectCountMethod="GetHeaderImagesCount" 
    OnSelecting="objData_Selecting" OnSelected="objData_Selected" >
    <SelectParameters>
        <asp:Parameter Name="activeStatus" DbType="String" />
    </SelectParameters>    
</asp:ObjectDataSource>
<asp:SqlDataSource ID="SqlNaming" runat="server" EnableCaching="false" ConnectionString="<%$ ConnectionStrings:WillCallConnectionString %>"     
    SelectCommand="SELECT [Id], [bActive], [bDisplayPriority], [bExclusive], [iTimeoutMsec], [FileName], [DisplayText], [NavigateUrl], [tShowId], [tMerchId], [vcContext], 
    ISNULL([dtStart], '1/1/1753') as 'dtStart', ISNULL([dtEnd], '1/1/1753') as 'dtEnd'
    FROM [HeaderImage] sp WHERE sp.[Id] = @Id ORDER BY [iDisplayOrder] ASC " 
    InsertCommand="SELECT 0" 
    DeleteCommand="SELECT 0"
    UpdateCommand="UPDATE [HeaderImage] SET [bActive] = @bActive, [bDisplayPriority] = @bDisplayPriority, [bExclusive] = @bExclusive, [iTimeoutMsec] = @iTimeoutMsec, [DisplayText] = @DisplayText, [NavigateUrl] = @NavigateUrl, 
    [vcContext] = @vcContext, [tShowId] = @tShowId, [tMerchId] = @tMerchId, [dtStart] = @dtStart, [dtEnd] = dtEnd, [dtModified] = @dtModified WHERE [Id] = @Id"
    OnUpdating="SqlNaming_Updating" OnDeleting="SqlNaming_Deleting" OnDeleted="SqlNaming_Deleted" >
    <DeleteParameters>
        <asp:ControlParameter ControlID="FormView1" Name="Idx" PropertyName="SelectedValue" Type="Int32" />        
        <asp:Parameter Name="FileName" Type="String" ConvertEmptyStringToNull="true" />
    </DeleteParameters>
    <SelectParameters>
        <asp:ControlParameter ControlID="GridView1" Name="Id" PropertyName="SelectedValue" Type="Int32" />
    </SelectParameters>
    <UpdateParameters>
        <asp:ControlParameter ControlID="GridView1" Name="Id" PropertyName="SelectedValue" Type="Int32" />
        <asp:Parameter Name="bActive" Type="Boolean" DefaultValue="true" />
        <asp:Parameter Name="bDisplayPriority" Type="Boolean" DefaultValue="false" />
        <asp:Parameter Name="bExclusive" Type="Boolean" DefaultValue="false" />
        <asp:Parameter Name="iTimeoutMsec" Type="Int32" />
        <asp:Parameter Name="tShowId" Type="Int32" ConvertEmptyStringToNull="true" />
        <asp:Parameter Name="tMerchId" Type="Int32" ConvertEmptyStringToNull="true" />
        <asp:Parameter Name="DisplayText" Type="String" ConvertEmptyStringToNull="true" />
        <asp:Parameter Name="NavigateUrl" Type="String" ConvertEmptyStringToNull="true" />
        <asp:Parameter Name="vcContext" Type="String" ConvertEmptyStringToNull="true" />
        <asp:Parameter Name="dtStart" DbType="DateTime" ConvertEmptyStringToNull="true" />
        <asp:Parameter Name="dtEnd" DbType="DateTime" ConvertEmptyStringToNull="true" />
        <asp:Parameter Name="dtModified" DbType="DateTime" />
    </UpdateParameters>
</asp:SqlDataSource>