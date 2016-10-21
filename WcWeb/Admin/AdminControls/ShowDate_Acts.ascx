<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ShowDate_Acts.ascx.cs" Inherits="WillCallWeb.Admin.AdminControls.ShowDate_Acts" %>
<%@ Register Src="Editor_Act.ascx" TagName="Editor_Act" TagPrefix="uc2" %>
<div id="srceditor">
    <div id="showact">
        <div class="jqhead rounded">
            <h3 class="entry-title">Acts - <asp:Literal ID="litShowTitle" runat="server" /></h3>
            <asp:GridView ID="GridView1" Width="100%" DataSourceID="SqlShowDates" runat="server" AutoGenerateColumns="False" 
                DataKeyNames="Id,ShowId,bAutoBilling" ShowFooter="False" CssClass="lsttbl"
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
            <div class="cmdsection">
                <a href="javascript: alert('This will toggle how the acts and promoters are displayed. Auto inserts WITH and ampersands. Legacy allows you to control text displayed by pre, featuring and post text. Custom is - custom')" class="infomark">?</a>
                Billing Method...
                <asp:RadioButtonList ID="rdoBilling" runat="server" AutoPostBack="true" RepeatDirection="Horizontal" RepeatLayout="Flow" CellSpacing="8" CellPadding="8"
                    OnDataBound="rdoBilling_DataBound" OnSelectedIndexChanged="rdoBilling_SelectedIndexChanged">
                    <asp:ListItem Text=" Auto&nbsp;&nbsp;&nbsp;&nbsp;" Value="Auto" />
                    <asp:ListItem Text=" Legacy&nbsp;&nbsp;&nbsp;&nbsp;" Value="Legacy" />
                    <asp:ListItem Text=" Custom&nbsp;&nbsp;&nbsp;&nbsp;" Value="Custom" />
                </asp:RadioButtonList>
            </div>
            <asp:GridView ID="GridViewEntity" Width="100%" DataSourceID="SqlShowActs" runat="server" AutoGenerateColumns="False" 
                DataKeyNames="Id" ShowFooter="False" CssClass="lsttbl" 
                OnDataBound="GridViewEntity_DataBound" 
                OnRowCommand="GridViewEntity_RowCommand" 
                OnRowDataBound="GridViewEntity_RowDataBound" 
                OnRowDeleting="GridViewEntity_RowDeleting" >
                <HeaderStyle HorizontalAlign="left" />
                <SelectedRowStyle CssClass="selected" />
                <Columns>
                    <asp:TemplateField ItemStyle-Wrap="false" HeaderText="ACTS">
                        <ItemTemplate>
                            <asp:LinkButton Width="20px" Id="btnSelect" CausesValidation="false" ToolTip="Select" CssClass="btnselect" runat="server" CommandName="Select" 
                                CommandArgument='<%#Eval("Id") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:CheckBoxField DataField="TopBilling" HeaderText="Head" ReadOnly="true" HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="Center" />
                    <asp:TemplateField ItemStyle-Wrap="false" HeaderText="Act" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="100%" >
                        <ItemTemplate>
                            <asp:Literal ID="litDesc" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-Wrap="false" >
                       <ItemTemplate>
                            <asp:LinkButton Width="20px" Id="btnUp" ToolTip="Move Up" CssClass="btnup" runat="server" CommandName="Up" 
                                CommandArgument='<%#Eval("Id") %>' CausesValidation="false" />
                            <asp:LinkButton Width="20px" Id="btnDown" ToolTip="Move Down" CssClass="btndown" runat="server" CommandName="Down" 
                                CommandArgument='<%#Eval("Id") %>' CausesValidation="false" />
                            <asp:CustomValidator ID="CustomValidation" runat="server" CssClass="invisible" 
                                Display="Static" ErrorMessage="bad mojo" ValidationGroup="showact">*</asp:CustomValidator>
                           <asp:LinkButton Width="20px" Id="btnDelete" CssClass="btndelete lastinrow" runat="server" CommandName="Delete" 
                                CommandArgument='<%#Eval("Id") %>' ToolTip="Delete" CausesValidation="false"
                               OnClientClick='return confirm("Are you sure you want to delete this row?")' />
                       </ItemTemplate>
                   </asp:TemplateField>
                 </Columns>
            </asp:GridView>
            <asp:ValidationSummary ID="ValidationSummary1" CssClass="validationsummary" ValidationGroup="showact" runat="server" />

            <div class="jqpnl rounded eit">

             <asp:FormView Width="100%" CssClass="noborder" ID="FormView1" runat="server" DefaultMode="Edit" OnItemInserting="FormView1_ItemInserting" 
                DataSourceID="SqlAct" DataKeyNames="Id" OnItemUpdated="FormView1_ItemUpdated" OnItemCommand="FormView1_ItemCommand" OnDataBound="FormView1_DataBound"
                 >
                <EditItemTemplate>
                    <div class="cmdsection">
                        <asp:Button ID="btnSave" CausesValidation="false" runat="server" CommandName="Update" 
                            Text="Save" CssClass="btnmed" />
                        <asp:Button ID="btnCancel" runat="server" CommandName="Cancel" Text="Cancel" CssClass="btnmed" />
                        <asp:Button ID="btnNew" runat="server" CommandName="New" Text="New" CssClass="btnmed" />
                        <asp:CustomValidator ID="CustomValidation" runat="server" CssClass="invisible" 
                            Display="Static" ErrorMessage="bad mojo" ValidationGroup="showact">*</asp:CustomValidator>
                        <asp:Button ID="btnSales" runat="server" CommandName="viewsales" CausesValidation="false" 
                            Text="View Sales" CssClass="btnmed" />
                        <asp:Button ID="btnChangeShowName" runat="server" Text="Sync Show Name" CssClass="btnmed" 
                            OnClick="btnChangeShowName_Click"
                            OnClientClick="return confirm('This will update the show name to reflect the current information. Are you sure you want to continue?');" />
                        <asp:Button ID="btnDisplay" runat="server" CausesValidation="false" CommandName="view" Text="Display Show" 
                            CssClass="btnmed" OnClientClick='doPagePopup("/Admin/ShowDisplayer.aspx", "false"); ' />
                    </div>
                    <table border="0" cellspacing="0" cellpadding="0" width="100%" class="edittabl">
                        <tr>
                            <th class="intr">{ShowActId <%#Eval("Id") %>}</th>
                            <td style="width:100%;" class="listing-row">
                                <a href="javascript: alert('Setting this to true indicates that this act should receive top billing. All headliners and co-headliners should have this checked.')" class="infomark">?</a>
                                Headliner
                                <asp:CheckBox ID="chkHeadline" runat="server" Checked='<%#Bind("TopBilling") %>' />
                            </td>
                        </tr>
                        
                        <%if (rdoBilling.SelectedValue.ToLower() != "custom")
                          {%>
                        <tr>
                            <th><a href="javascript: alert('Text to be shown prior to the act.')" class="infomark">?</a>
                                Pre Text</th>
                            <td><asp:TextBox ID="txtPre" MaxLength="500" Width="100%" runat="server" Text='<%#Bind("PreText") %>' /></td>
                        </tr>
                        <tr>
                            <th><a href="javascript: alert('Text to be shown after the act but included as if were a normal part of the act name.')" class="infomark">?</a>
                                Act Text</th>
                            <td><asp:TextBox ID="txtAct" MaxLength="300" Width="100%" runat="server" Text='<%#Bind("ActText") %>' /></td>
                        </tr>
                        <tr>
                            <th><a href="javascript: alert('Common to many shows...will show the text as &#34;featuring&#34; so and so. Note that this will not show in the ticket listing - for use in show info listing only.')" class="infomark">?</a>
                                Featuring</th>
                            <td><asp:TextBox ID="txtFeaturing" MaxLength="1000" Width="100%" runat="server" Text='<%#Bind("Featuring") %>' /></td>
                        </tr>
                        <tr>
                            <th><a href="javascript: alert('Text to be shown at the end of the act listing.')" class="infomark">?</a>
                                Post Text</th>
                            <td><asp:TextBox ID="txtPost" MaxLength="2000" Width="100%" runat="server" Text='<%#Bind("PostText") %>' /></td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <div class="jqinstruction rounded">
                                    <ul>
                                        <li>act text will always display at the same size as the act - featuring will be smaller</li>
                                        <li>act text will start right after the act name - featuring should go to a new line</li>
                                        <li>act text will show in the ticketing options - featuring will not</li>
                                        <li>still figuring what to do with pre and post text</li>
                                    </ul>
                                </div>
                            </td>
                        </tr>
                        <%} %>
                    </table>
                    
                    <uc2:Editor_Act ID="Editor_Act2" runat="server" SelectedIdx='<%#Eval("tActId") %>' AllowSelect="false" 
                        TitleText="" DisplayTitle="false" MaxImageDimension="100" />                    
                </EditItemTemplate>
                <InsertItemTemplate>
                    <asp:Panel ID="pnlCopy" runat="server">
                        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="edittabl">
                            <tr>
                                <th>Copy Act</th>
                                <td style="width: 100%;"><asp:DropDownList ID="ddlCopyAct" runat="server" /></td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Button ID="btnCopy" CausesValidation="false" runat="server" CommandName="Copy" 
                                        Text="Copy Selection" CssClass="btnmed"
                                        OnClientClick="javascript: return confirm('Are you certain you would like to copy the selection?');" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <div class="jqpanel1 altpanelbg rounded" style="padding:1px;">
                        <uc2:Editor_Act ID="Editor_Act1" runat="server" AllowSelect="true" 
                            TitleText="" DisplayTitle="false" MaxImageDimension="100" />
                    </div>
                    <div class="cmdsection">
                        <asp:Button ID="btnSave" CausesValidation="false" runat="server" CommandName="Insert" 
                            Text="Add Act" CssClass="btnmed" />
                        <asp:Button ID="btnCancel" runat="server" CommandName="Cancel" CausesValidation="false"
                            Text="Cancel" CssClass="btnmed" />
                        <asp:CustomValidator ID="CustomValidation" runat="server" 
                            ValidationGroup="showact" CssClass="invisible" Display="Static" ErrorMessage="CustomValidator">*</asp:CustomValidator>
                    </div>
                </InsertItemTemplate>
            </asp:FormView>
            <%if (rdoBilling.SelectedValue.ToLower() == "custom")
              {%>
            <div class="cmdsection">Custom Billing...
                <asp:Button ID="btnWys" CausesValidation="false" runat="server" CommandName="Wys" Text="Edit" Width="100px" 
                    CssClass="btnmed ov-trigger" Tooltip="" rel="#overlay-wysiwyg" />
                
                <asp:Button ID="btnDisplay" runat="server" CausesValidation="false" CommandName="view" Text="Display Item" Width="100px"
                    CssClass="btnmed" OnClientClick='doPagePopup("/Admin/ShowDisplayer.aspx", "false"); ' />
              </div> 
            <asp:Literal ID="litDesc" runat="server" OnDataBinding="litDesc_DataBinding" />
            <%} %>
            </div>
        </div>
    </div>
</div>

<div class="admin_overlay" id="overlay-wysiwyg" style="display:none;">
	<div class="contentWrap"></div>
</div>

<script type="text/javascript" src="/JQueryUI/admin-overlay.js"></script>


<asp:SqlDataSource ID="SqlShowDates" runat="server" ConnectionString="<%$ ConnectionStrings:WillCallConnectionString %>" 
    SelectCommand="SELECT sd.[Id], sd.[dtDateOfShow] as DateOfShow, sd.[ShowTime], a.[Name] as AgeName, ss.[Name] as StatusName, 
        sd.[StatusText], sd.[TShowId] as ShowId, sd.[bAutoBilling] as bAutoBilling
        FROM [ShowDate] sd, [Age] a, [ShowStatus] ss
        WHERE sd.[tShowId] = @ShowId AND sd.[bActive] = 1 AND sd.[tAgeId] = a.[Id] AND sd.[tStatusId] = ss.[Id] 
        ORDER BY CASE WHEN (sd.bLateNightShow IS NOT NULL AND sd.bLateNightShow = 1) THEN DATEADD(hh, 24, sd.[dtDateOfShow]) ELSE sd.[dtDateOfShow] END ">
    <SelectParameters>
        <asp:SessionParameter Name="ShowId" SessionField="Admin_CurrentShowId" />
    </SelectParameters>
</asp:SqlDataSource>


<asp:SqlDataSource ID="SqlShowActs" runat="server" ConnectionString="<%$ ConnectionStrings:WillCallConnectionString %>" 
    SelectCommand="SELECT j.[Id], a.[Name] as ActName, j.[PreText], j.[ActText], j.[Featuring], j.[PostText], j.[iDisplayOrder] as DisplayOrder, 
        j.[bTopBilling] as TopBilling 
        FROM [JShowAct] j, [Act] a 
        WHERE j.[TShowDateId] = @ShowDateId AND j.[TActId] = a.[Id] 
        ORDER BY j.[iDisplayOrder]" 
        DeleteCommand="SELECT 0 ">
    <SelectParameters>
        <asp:ControlParameter ControlID="GridView1" Name="ShowDateId" PropertyName="SelectedValue" Type="Int32" />
    </SelectParameters>
</asp:SqlDataSource>

<asp:SqlDataSource ID="SqlAct" runat="server" ConnectionString="<%$ ConnectionStrings:WillCallConnectionString %>" 
    SelectCommand="SELECT j.[Id], a.[Name] as 'ActName', j.[TActId], j.[PreText], j.[ActText], j.[Featuring], j.[PostText], ISNULL(j.[bTopBilling], 0) as 'TopBilling' 
        FROM [JShowAct] j, [Act] a 
        WHERE j.[Id] = @Idx AND j.[TActId] = a.[Id] "
    UpdateCommand="UPDATE [JShowAct] SET [bTopBilling] = @TopBilling, [PreText] = @PreText, [ActText] = @ActText, [Featuring] = @Featuring, [PostText] = @PostText WHERE [Id] = @Id; "
    InsertCommand="SELECT 0 " >
    <UpdateParameters>
        <asp:Parameter Name="TopBilling" Type="boolean" />
        <asp:Parameter Name="PreText" Type="String" />
        <asp:Parameter Name="ActText" Type="String" />
        <asp:Parameter Name="Featuring" Type="String" />
        <asp:Parameter Name="PostText" Type="String" />
        <asp:Parameter Name="Id" Type="Int32" />
        <asp:SessionParameter Name="showId" SessionField="Admin_CurrentShowId" />
    </UpdateParameters>
    <SelectParameters>
        <asp:ControlParameter ControlID="GridViewEntity" Name="Idx" PropertyName="SelectedValue" Type="Int32" />
    </SelectParameters>
</asp:SqlDataSource>