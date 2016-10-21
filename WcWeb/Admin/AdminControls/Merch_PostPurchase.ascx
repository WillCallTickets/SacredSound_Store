<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Merch_PostPurchase.ascx.cs" Inherits="WillCallWeb.Admin.AdminControls.Merch_PostPurchase" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register src="/Components/Util/CalendarClock.ascx" tagname="CalendarClock" tagprefix="uc1" %>

<div id="srceditor">
    <div id="merchitem">
        <div class="jqhead rounded">
            <h3 class="entry-title">
                <%if (Atx.CurrentMerchRecord != null) {%>
                    <%= Atx.CurrentMerchRecord.Id.ToString()%> - <%=Atx.CurrentMerchRecord.DisplayNameWithAttribs%>
                <%}%>
            </h3>
            <asp:GridView ID="GridPost" Width="100%" DataSourceID="SqlPostPurchaseCollection" runat="server" AutoGenerateColumns="False" 
                DataKeyNames="tPostPurchaseId" ShowFooter="False" CssClass="lsttbl" 
                OnRowDataBound="GridPost_RowDataBound"
                OnRowCommand="GridPost_RowCommand" OnRowDeleting="GridPost_RowDeleting" OnRowDeleted="GridPost_RowDeleted"
                OnDataBound="GridPost_DataBound" 
                OnSelectedIndexChanged="GridPost_SelectedIndexChanged" >
                <HeaderStyle HorizontalAlign="left" />
                <SelectedRowStyle CssClass="selected" />
                <EmptyDataTemplate>
                    <div class="lstempty">No PostPurchaseTexts For Selected Merch Item</div>
                </EmptyDataTemplate>
                <Columns>
                    <asp:TemplateField ItemStyle-Wrap="false" HeaderText="REQS">
                        <ItemTemplate>
                            <asp:LinkButton Width="20px" Id="btnSelect" CausesValidation="false" ToolTip="Select" CssClass="btnselect" runat="server" CommandName="Select" 
                                CommandArgument='<%#Eval("tPostPurchaseId") %>' />
                            <asp:CustomValidator ID="CustomValidation" runat="server" ValidationGroup="merch" CssClass="validator" 
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
            <asp:ValidationSummary ID="ValidationSummary2" CssClass="validationsummary" HeaderText="" ValidationGroup="merch" runat="server" />   
            <div class="jqinstruction rounded">
                <ul><strong>POST PURCHASE NOTES</strong>
                    <li>In Process Text is shown to the customer within the order flow - merch description page - and is not required</li>
                    <li>Post Text is given to the buyer after purchase - confirmation, email and print - allowable parameters are ITEMID, INVOICEID and USERNAME</li>
                    <li>Links will be displayed longhand on emails and printouts (href="LONGHAND").</li>
                    <li>Links should be targeted to blank. (target="_blank")</li>
                </ul>
                <br />
            </div>
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
                            Text="CREATE POST TEXT" CssClass="btnmed" />
                    </div>
                </div>
            </EmptyDataTemplate>    
            <EditItemTemplate>
                <div class="jqpnl rounded eit">
                    <div class="cmdsection">
                        <asp:Button ID="btnSave" CausesValidation="true" ValidationGroup="merch" runat="server" CommandName="Update" 
                            Text="Save" CssClass="btnmed" />
                        <asp:Button ID="btnCancel" runat="server" CommandName="Cancel" CausesValidation="false" 
                            Text="Cancel" CssClass="btnmed" />
                        <asp:Button ID="btnNew" runat="server" CommandName="New" CausesValidation="false" 
                            Text="New" CssClass="btnmed" />
                        <asp:CustomValidator ID="CustomValidation" runat="server" ValidationGroup="merch" CssClass="validator" 
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
                                    CssClass="btnmed ov-trigger" Tooltip="/Admin/AdminControls/Wysiwyg/Wysiwyg.aspx?context=ppm&ctrl=" rel="#overlay-wysiwyg" />
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



<asp:SqlDataSource ID="SqlPostPurchaseCollection" runat="server" ConnectionString="<%$ ConnectionStrings:WillCallConnectionString %>" 
    SelectCommand="SELECT pp.[Id] as 'tPostPurchaseId', pp.[dtStamp], pp.[tMerchId], pp.[tShowTicketId], pp.[bActive], 
        pp.[iDisplayOrder], pp.[InProcessDescription], pp.[PostText]
        FROM [PostPurchaseText] pp
        WHERE pp.[TMerchId] = @parentId
        ORDER BY pp.[iDisplayOrder]"
    DeleteCommand="SELECT 0 " OnSelecting="SqlPostPurchaseCollection_Selecting">
    <DeleteParameters>
        <asp:ControlParameter ControlID="GridPost" Name="tPostPurchaseId" PropertyName="SelectedValue" Type="Int32" />
    </DeleteParameters>
    <SelectParameters>
        <asp:Parameter Name="parentId" Type="Int32" />
    </SelectParameters>
</asp:SqlDataSource>

<asp:SqlDataSource ID="SqlPostPurchase" runat="server" ConnectionString="<%$ ConnectionStrings:WillCallConnectionString %>" 
    SelectCommand="SELECT pp.[Id] as 'tPostPurchaseId', pp.[dtStamp], pp.[tMerchId], pp.[tShowTicketId], pp.[bActive], 
        pp.[iDisplayOrder], pp.[InProcessDescription], pp.[PostText]
        FROM [PostPurchaseText] pp
        WHERE pp.[Id] = @tPostPurchaseId "
    UpdateCommand="UPDATE [PostPurchaseText] SET [bActive] = @bActive, [InProcessDescription] = @InProcessDescription WHERE [Id] = @tPostPurchaseId "
    InsertCommand="SELECT 0 "     
    onupdated="SqlEntity_Updated" >
    <SelectParameters>
        <asp:ControlParameter ControlID="GridPost" Name="tPostPurchaseId" PropertyName="SelectedValue" Type="Int32" />
    </SelectParameters>    
    <UpdateParameters>
        <asp:ControlParameter ControlID="FormPost" Name="tPostPurchaseId" PropertyName="SelectedValue" Type="Int32" />
    </UpdateParameters>
</asp:SqlDataSource>
