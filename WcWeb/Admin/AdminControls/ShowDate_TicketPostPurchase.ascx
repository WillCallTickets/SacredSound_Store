<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ShowDate_TicketPostPurchase.ascx.cs" Inherits="WillCallWeb.Admin.AdminControls.ShowDate_TicketPostPurchase" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register src="/Components/Util/CalendarClock.ascx" tagname="CalendarClock" tagprefix="uc1" %>

<div id="srceditor">
    <div id="showticket">
        <div class="jqhead rounded">
            <h3 class="entry-title">Tix Post Purch - <asp:Literal ID="litShowTitle" runat="server" /></h3>
            <asp:GridView ID="GridDates" Width="100%" DataSourceID="SqlDates" runat="server" AutoGenerateColumns="False" DataKeyNames="Id" 
                ShowFooter="False" CssClass="lsttbl" 
                OnDataBound="GridDates_DataBound" 
                OnRowCommand="GridDates_RowCommand" 
                OnSelectedIndexChanged="GridDates_SelectedIndexChanged" >
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
            <asp:GridView ID="GridTickets" Width="100%" DataSourceID="SqlTickets" runat="server" AutoGenerateColumns="False" 
                DataKeyNames="Id" ShowFooter="False" CssClass="lsttbl" 
                OnDataBound="GridTickets_DataBound" 
                OnRowDataBound="GridTickets_RowDataBound" OnSelectedIndexChanged="GridTickets_SelectedIndexChanged" >
                <HeaderStyle HorizontalAlign="left" />
                <SelectedRowStyle CssClass="selected" />
                <EmptyDataTemplate>
                    <div class="lstempty">No Tickets For Selected Date</div>
                </EmptyDataTemplate>
                <Columns>
                    <asp:TemplateField ItemStyle-Wrap="false" HeaderText="TKTS">
                        <ItemTemplate>
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
                 </Columns>
            </asp:GridView>
            <asp:GridView ID="GridPost" Width="100%" DataSourceID="SqlPostPurchaseCollection" runat="server" AutoGenerateColumns="False" 
                DataKeyNames="tPostPurchaseId" ShowFooter="False" CssClass="lsttbl" 
                OnRowDataBound="GridPost_RowDataBound"
                OnRowCommand="GridPost_RowCommand" OnRowDeleting="GridPost_RowDeleting" OnRowDeleted="GridPost_RowDeleted"
                OnDataBound="GridPost_DataBound" 
                OnSelectedIndexChanged="GridPost_SelectedIndexChanged" >
                <HeaderStyle HorizontalAlign="left" />
                <SelectedRowStyle CssClass="selected" />
                <EmptyDataTemplate>
                    <div class="lstempty">No PostPurchaseTexts For Selected Ticket</div>
                </EmptyDataTemplate>
                <Columns>
                    <asp:TemplateField ItemStyle-Wrap="false" HeaderText="REQS">
                        <ItemTemplate>
                            <asp:LinkButton Width="20px" Id="btnSelect" CausesValidation="false" ToolTip="Select" CssClass="btnselect" runat="server" CommandName="Select" 
                                CommandArgument='<%#Eval("tPostPurchaseId") %>' />
                            <asp:CustomValidator ID="CustomValidation" runat="server" ValidationGroup="showticket" CssClass="validator" 
                                Display="Static" >*</asp:CustomValidator> 
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:CheckBoxField DataField="bActive" HeaderText="Act" ItemStyle-HorizontalAlign="Center" />
                    <asp:TemplateField HeaderText="Description" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="100%">
                        <ItemTemplate>
                            <asp:Literal ID="litDesc" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-Wrap="false" >
                       <ItemTemplate>
                           <asp:LinkButton Width="20px" Id="btnUp" ToolTip="Move Up" CssClass="btnup" runat="server" CommandName="Up" 
                                CommandArgument='<%#Eval("tPostPurchaseId") %>' CausesValidation="false" />
                           <asp:LinkButton Width="20px" Id="btnDown" ToolTip="Move Down" CssClass="btndown" runat="server" CommandName="Down" 
                                CommandArgument='<%#Eval("tPostPurchaseId") %>' CausesValidation="false" />
                           <asp:LinkButton Width="20px" Id="btnDelete" CssClass="btndelete lastinrow" runat="server" CommandName="Delete" 
                               CommandArgument='<%#Eval("tPostPurchaseId") %>' ToolTip="Delete" CausesValidation="false"
                               OnClientClick='return confirm("Are you sure you want to delete this row?")' />
                       </ItemTemplate>
                   </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <div class="jqinstruction rounded">
                <strong>POST PURCHASE NOTES</strong>
                <ul>
                    <li>In Process Text is shown to the customer within the order flow - ticket page - and is not required</li>
                    <li>Post Text is given to the buyer after purchase - confirmation, email and print - allowable parameters are ITEMID, INVOICEID and USERNAME</li>
                    <li>Links will be displayed longhand on emails and printouts (href="LONGHAND").</li>
                    <li>Links should be targeted to blank. (target="_blank")</li>
                </ul>
            </div>
            <asp:ValidationSummary ID="ValidationSummary2" CssClass="validationsummary" HeaderText="" ValidationGroup="showticket" runat="server" />
        </div>
        <asp:FormView Width="100%" ID="FormPost" runat="server" DefaultMode="Edit" 
            DataSourceID="SqlPostPurchase" DataKeyNames="tPostPurchaseId" 
            OnDataBound="FormPost_DataBound"
            OnItemUpdated="FormPost_ItemUpdated" 
            OnItemCommand="FormPost_ItemCommand" >
            <EmptyDataTemplate>
                <div class="jqpnl rounded iit">
                    <div class="cmdsection">
                        <asp:Button Id="btnNew" CausesValidation="false" runat="server" CommandName="New" 
                            Text="CREATE POST TEXT" CssClass="btnmed" Width="120px" />
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
                    </div>
                    <br />
                    <table border="0" cellspacing="0" cellpadding="0" width="100%" class="edittabl">
                        <tr>
                            <th><span class="intr"><%#Eval("tPostPurchaseId") %></span> Active</th>
                            <td style="width:100%;" class="listing-row">
                                <asp:CheckBox ID="chkActive" runat="server" Checked='<%#Bind("bActive") %>' />
                            </td>
                        </tr>
                        <tr>
                            <th><a href="javascript: alert('This text will be shown to the customer within the order flow and is not required.')" class="infomark">?</a>In Process</th>
                            <td>
                                <asp:TextBox ID="txtInProcess" MaxLength="1500" Width="650px" runat="server" Text='<%#Bind("InProcessDescription") %>' />
                            </td>
                        </tr>
                        <tr>
                            <th valign="top">
                                <a href="javascript: alert('This text will be shown to the customer on their confirmation, email and print page and is required.')" class="infomark">?</a>Post Text &nbsp;
                                <br /><br />
                                <asp:Button ID="btnWys" CausesValidation="false" runat="server" CommandName="Wys" Text="Edit" Width="100px" 
                                    CssClass="btnmed ov-trigger" Tooltip="/Admin/AdminControls/Wysiwyg/Wysiwyg.aspx?context=ppt&ctrl=" rel="#overlay-wysiwyg" />
                            </th>
                            <td>
                                <asp:Literal ID="litDesc" runat="server" EnableViewState="false" />
                            </td>
                        </tr>
                    </table>
                </div>
            </EditItemTemplate>
        </asp:FormView>
    </div>
</div>
<div class="admin_overlay" id="overlay-wysiwyg" style="display:none;">
	<div class="contentWrap"></div>
</div>
<script type="text/javascript" src="/JQueryUI/admin-overlay.js"></script>


<asp:SqlDataSource ID="SqlDates" runat="server" ConnectionString="<%$ ConnectionStrings:WillCallConnectionString %>" 
    SelectCommand="SELECT sd.[Id], sd.[dtDateOfShow] as DateOfShow, sd.[ShowTime], a.[Name] as AgeName, ss.[Name] as StatusName, sd.[StatusText] 
        FROM [ShowDate] sd, [Age] a, [ShowStatus] ss 
        WHERE sd.[tShowId] = @ShowId AND sd.[bActive] = 1 AND sd.[tAgeId] = a.[Id] AND sd.[tStatusId] = ss.[Id] 
        ORDER BY CASE WHEN (sd.bLateNightShow IS NOT NULL AND sd.bLateNightShow = 1) THEN DATEADD(hh, 24, sd.[dtDateOfShow]) ELSE sd.[dtDateOfShow] END ">
    <SelectParameters>
        <asp:SessionParameter Name="ShowId" SessionField="Admin_CurrentShowId" />
    </SelectParameters>
</asp:SqlDataSource>

<asp:SqlDataSource ID="SqlTickets" runat="server" ConnectionString="<%$ ConnectionStrings:WillCallConnectionString %>" 
    SelectCommand="
        SELECT ent.[Id], ent.[TVendorId], ent.[PriceText], ISNULL(ent.[mPrice],0.0) as mPrice, ent.[DosText], ent.[mDosPrice], ent.[mServiceCharge], a.[Name] as AgeName, 
        ent.[SalesDescription], ent.[CriteriaText], 
        ent.[bActive], ent.[bSoldOut], ent.[bDosTicket], ent.[bAllowShipping], ent.[bAllowWillCall], ent.[iAllotment], 
        ISNULL(pending.[iQty], 0) as 'pendingStock', ent.[iSold],
        ent.[bUnlockActive], ent.[iDisplayOrder] as DisplayOrder, CASE WHEN COUNT(link.[Id]) IS NULL THEN 0 ELSE COUNT(link.[Id]) END as LinkCount 
        FROM [ShowTicket] ent 
        LEFT OUTER JOIN fn_PendingStock('ticket') pending ON pending.[idx] = ent.[Id] 
        LEFT OUTER JOIN [ShowTicketPackageLink] link ON ent.[Id] = link.[ParentShowTicketId], [Age] a         
        WHERE ent.[TShowDateId] = @ShowDateId AND ent.[TAgeId] = a.[Id] 
        GROUP BY ent.[Id], ent.[TVendorId], ent.[PriceText], ent.[mPrice], ent.[DosText], ent.[mDosPrice], ent.[mServiceCharge], a.[Name], ent.[SalesDescription], ent.[CriteriaText], 
        ent.[bActive], ent.[bSoldOut], ent.[bDosTicket], ent.[bAllowShipping], ent.[bAllowWillCall], ent.[iAllotment], pending.[iQty], ent.[iSold], 
        ent.[bUnlockActive], ent.[iDisplayOrder] 
        ORDER BY ent.[iDisplayOrder] " 
        DeleteCommand="SELECT 0 ">
    <SelectParameters>
        <asp:ControlParameter ControlID="GridDates" Name="ShowDateId" PropertyName="SelectedValue" Type="Int32" />
    </SelectParameters>
</asp:SqlDataSource>

<asp:SqlDataSource ID="SqlPostPurchaseCollection" runat="server" ConnectionString="<%$ ConnectionStrings:WillCallConnectionString %>" 
    SelectCommand="SELECT pp.[Id] as tPostPurchaseId, pp.[dtStamp], pp.[tMerchId], pp.[tShowTicketId], pp.[bActive], 
        pp.[iDisplayOrder], pp.[InProcessDescription], pp.[PostText]
        FROM [PostPurchaseText] pp
        WHERE pp.[TShowTicketId] = @tShowTicketId
        ORDER BY pp.[iDisplayOrder]"
    DeleteCommand="SELECT 0 ">
    <DeleteParameters>
        <asp:ControlParameter ControlID="GridPost" Name="tPostPurchaseId" PropertyName="SelectedValue" Type="Int32" />
    </DeleteParameters>
    <SelectParameters>
        <asp:ControlParameter ControlID="GridTickets" Name="tShowTicketId" PropertyName="SelectedValue" Type="Int32" />
    </SelectParameters>
</asp:SqlDataSource>

<asp:SqlDataSource ID="SqlPostPurchase" runat="server" ConnectionString="<%$ ConnectionStrings:WillCallConnectionString %>" 
    SelectCommand="SELECT pp.[Id] as 'tPostPurchaseId', pp.[dtStamp], pp.[tMerchId], pp.[tShowTicketId], pp.[bActive], 
        pp.[iDisplayOrder], pp.[InProcessDescription], pp.[PostText]
        FROM [PostPurchaseText] pp
        WHERE pp.[Id] = @tPostPurchaseId "
    UpdateCommand="UPDATE [PostPurchaseText] SET [bActive] = @bActive, [InProcessDescription] = @InProcessDescription WHERE [Id] = @tPostPurchaseId "    
    onupdated="SqlEntity_Updated" >
    <SelectParameters>
        <asp:ControlParameter ControlID="GridPost" Name="tPostPurchaseId" PropertyName="SelectedValue" Type="Int32" />
    </SelectParameters>
    <UpdateParameters>
        <asp:ControlParameter ControlID="GridTickets" Name="tShowTicketId" PropertyName="SelectedValue" Type="Int32" />
        <asp:ControlParameter ControlID="GridPost" Name="tPostPurchaseId" PropertyName="SelectedValue" Type="Int32" />
    </UpdateParameters>
</asp:SqlDataSource>
