<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ShowTicket_Bundle.ascx.cs" 
    Inherits="WillCallWeb.Admin.AdminControls.ShowTicket_Bundle" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<div id="srceditor">
    <div id="MerchBundle">
        <asp:UpdatePanel ID="updatePanel" runat="server" UpdateMode="Conditional">
            <ContentTemplate> 
                <div class="jqhead rounded">
                    <h3 class="entry-title">Bundle Editor - <asp:Literal ID="litTitle" runat="server" OnDataBinding="litTitle_DataBinding" EnableViewState="false" /></h3>            

                    <div class="entity-listing">
                        <asp:GridView Width="100%" ID="GridView1" runat="server" GridLines="None" 
                        AutoGenerateColumns="False" CssClass="lsttbl" BorderStyle="none"
                        OnDataBinding="GridView1_DataBinding" 
                        OnDataBound="GridView1_DataBound" 
                        OnRowCommand="GridView1_RowCommand" 
                        OnRowDataBound="GridView1_RowDataBound" 
                        OnRowDeleting="GridView1_RowDeleting" 
                        OnRowEditing="GridView1_RowEditing"
                        OnRowCancelingEdit="GridView1_RowCancelingEdit" 
                        OnRowUpdating="GridView1_RowUpdating"
                        OnSelectedIndexChanged="GridView1_SelectedIndexChanged" 
                        >                            
                        <SelectedRowStyle cssclass="selected" />
                        <EmptyDataTemplate>
                            <div class="cmdsection" style="padding:12px;">
                                <asp:Button ID="btnNew" runat="server" CausesValidation="false" ToolTip="Add New" 
                                    Cssclass="btnmed" CommandName="New" Text="Add New" ForeColor="Black" />  
                                <span class="intr">There are no bundles for this item.</span>
                            </div>
                        </EmptyDataTemplate>            
                        <Columns>
                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Center" ItemStyle-Wrap="false" ItemStyle-Width="100px">
                                <HeaderTemplate>
                                    <div class="cmdsection">
                                        <asp:Button ID="btnNew" runat="server" CausesValidation="false" ToolTip="Add New" 
                                            Cssclass="btnmed" CommandName="New" Text="Add New" ForeColor="Black" />
                                    </div>
                                </HeaderTemplate>                    
                                <ItemTemplate>
                                    <asp:LinkButton Width="20px" Id="btnSelect" ToolTip="Select" Cssclass="btn-select" runat="server" CommandName="select"
                                        CommandArgument='<%#Eval("Id") %>' CausesValidation="false" />
                                    <asp:LinkButton Width="20px" Id="btnEdit" ToolTip="Edit" Cssclass="btn-edit" runat="server" CommandName="edit"
                                        CommandArgument='<%#Eval("Id") %>' CausesValidation="false" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:LinkButton Width="20px" ID="btnSave" runat="server" Cssclass="btn-save" CausesValidation="false" ToolTip="Save" 
                                        CommandName="Update" Text="Save" />                                        
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Active" ItemStyle-HorizontalAlign="center">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkActive" runat="server" Enabled="false" Checked='<%#Eval("IsActive") %>' />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:CheckBox ID="chkActive" runat="server" Checked='<%#Bind("IsActive") %>' />
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Title" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="50%" >
                                <ItemTemplate>
                                    <%#Eval("Title") %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtTitle" runat="server" Text='<%#Bind("Title") %>' MaxLength="256" />
                                </EditItemTemplate>                    
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Comment" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="50%" >
                                <ItemTemplate>
                                    <%#Eval("Comment") %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtComment" runat="server" Text='<%#Bind("Comment") %>' MaxLength="500" />
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Weight" ItemStyle-HorizontalAlign="center">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkWeight" runat="server" Enabled="false" Checked='<%#Eval("IncludeWeight") %>' />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:CheckBox ID="chkWeight" runat="server" Checked='<%#Bind("IncludeWeight") %>' />
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ReqParentQty" ItemStyle-HorizontalAlign="center" >
                                <ItemTemplate>
                                    <%#Eval("RequiredParentQty") %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlParentQty" runat="server" CssClass="ddl" OnDataBinding="ddlQty_DataBinding" />
                                </EditItemTemplate>                
                            </asp:TemplateField>                                
                            <asp:TemplateField HeaderText="Select" ItemStyle-HorizontalAlign="center" >
                                <ItemTemplate>
                                    <%#Eval("MaxSelections") %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlMaxSelection" runat="server" CssClass="ddl" OnDataBinding="ddlQty_DataBinding" />
                                </EditItemTemplate>                
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Price" ItemStyle-HorizontalAlign="Center" >
                                <ItemTemplate>
                                    <%#Eval("Price", "{0:c}") %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtPrice" runat="server" Text='<%#Bind("Price", "{0:n2}") %>' MaxLength="10" />                        
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="PricedPerSelection" ItemStyle-HorizontalAlign="center">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkPriced" runat="server" Enabled="false" Checked='<%#Eval("PricedPerSelection") %>' />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:CheckBox ID="chkPriced" runat="server" Checked='<%#Bind("PricedPerSelection") %>' />
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Wrap="false">
                                <ItemTemplate>
                                    <asp:LinkButton Width="20px" Id="btnUp" ToolTip="Move Up" Cssclass="btn-up" runat="server" CommandName="Up" 
                                        CommandArgument='<%#Eval("Id") %>' CausesValidation="false" />
                                    <asp:LinkButton Width="20px" Id="btnDown" ToolTip="Move Down" Cssclass="btn-down" runat="server" CommandName="Down" 
                                        CommandArgument='<%#Eval("Id") %>' CausesValidation="false" />
                                    <asp:CustomValidator ID="CustomValidation" runat="server" ValidationGroup="entity" Display="Static" CssClass="validator">*</asp:CustomValidator>  
                                    <asp:LinkButton Width="20px" Id="btnDelete" Cssclass="btn-delete" runat="server" CommandName="Delete" 
                                        CommandArgument='<%#Eval("Id") %>' ToolTip="Delete" CausesValidation="false"
                                        OnClientClick='return confirm("Are you sure you want to delete this row?")' />                      
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:CustomValidator ID="CustomValidation" runat="server" ValidationGroup="entity" Display="Static" CssClass="validator">*</asp:CustomValidator>
                                    <asp:LinkButton Width="20px" ID="btnCancel" runat="server" Cssclass="btn-cancel" CausesValidation="false" ToolTip="Cancel" 
                                        CommandName="Cancel" Text="Cancel" />
                                </EditItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    </div>

                    <asp:ValidationSummary ID="ValidationSummary2" CssClass="validationsummary" Width="70%" HeaderText="" ValidationGroup="entity" runat="server" />
                </div>

                <asp:ListView ID="ListView1" runat="server" DataKeyNames="Id" ItemPlaceholderID="ListViewContent" 
                    OnItemCommand="ListView1_ItemCommand" 
                    ondatabinding="ListView1_DataBinding" OnItemInserting="ListView1_ItemInserting" 
                    OnItemDataBound="ListView1_ItemDataBound" ondatabound="ListView1_DataBound" 
                        onitemdeleting="ListView1_ItemDeleting" >
                    <EmptyDataTemplate>
                    </EmptyDataTemplate>

                    <LayoutTemplate>  
                        <div class="jqpnl rounded">      
                            <table border="0" cellpadding="0" cellspacing="0" width="100%" class="edittabl" >
                                <thead>
                                    <tr class="headr"><th style="text-align:center;width:36px;">Active</th><th style="width:100%;text-align:left;">Merchandise / Inventory</th><th>&nbsp;</th></tr>
                                </thead>
                                <tbody runat="server" id="ListViewContent" />
                            </table>
                        </div>
                    </LayoutTemplate>

                    <ItemTemplate>
                        <tr class="main-item">
                            <td style="text-align:center;padding: 0 12px;width:36px;">
                                <asp:CheckBox ID="chkActive" runat="server" AutoPostBack="true" OnCheckedChanged="chkActive_CheckChanged" Checked='<%#Eval("IsActive") %>' />
                            </td>
                            <td>(bundle id: <%#Eval("Id") %>) <%#Eval("MerchRecord.DisplayNameWithAttribs") %></td>
                            <td style="white-space:nowrap;">
                                <asp:LinkButton Width="20px" Id="btnUp" ToolTip="Move Up" Cssclass="btn-up" runat="server" CommandName="Up" 
                                    CommandArgument='<%#Eval("Id") %>' CausesValidation="false" />
                                <asp:LinkButton Width="20px" Id="btnDown" ToolTip="Move Down" Cssclass="btn-down" runat="server" CommandName="Down" 
                                    CommandArgument='<%#Eval("Id") %>' CausesValidation="false" />
                                <asp:LinkButton Width="20px" Id="btnDelete" Cssclass="btn-delete lastinrow" runat="server" CommandName="Delete" 
                                    CommandArgument='<%#Eval("Id") %>' ToolTip="Delete" CausesValidation="false"
                                    OnClientClick='return confirm("Are you sure you want to delete this row?")' />
                            </td>
                        </tr>
                        <asp:Repeater ID="rptInventory" runat="server" OnItemDataBound="rptInventory_ItemDataBound" EnableViewState="false">
                            <ItemTemplate>
                                <tr>
                                    <td colspan="3" class="inv-item">
                                        <span class="nbr"><%#Eval("Available") %></span> (merch id: <%#Eval("Id") %>) <asp:Literal ID="litProductName" runat="server" /></td>
                                </tr>
                            </ItemTemplate>                     
                        </asp:Repeater>
                    </ItemTemplate>

                    <InsertItemTemplate>
                        <tr>
                            <td colspan="3">
                                <table border="0" cellpadding="0" cellspacing="0" width="100%" class="edittabl">
                                    <tr>
                                        <td style="white-space:nowrap;">
                                            <asp:LinkButton ID="btnNew" runat="server" CausesValidation="false" 
                                                ToolTip="Add New" Cssclass="link-addnew" CommandName="Insert" Text="Add Selected Item &raquo;" />
                                            <asp:CustomValidator ID="CustomValidation" runat="server" ValidationGroup="entity"
                                                Display="Static" CssClass="validator">*</asp:CustomValidator>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlMerch" CssClass="ddl" runat="server" DataSourceID="SqlMerch"
                                                DataTextField="Name" DataValueField="Id" AppendDataBoundItems="true" EnableViewState="false">
                                                <asp:ListItem Selected="True" Text=" <-- Select an item --> " Value="0" />
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </InsertItemTemplate>
            
                    <ItemSeparatorTemplate>
                        <tr class="sprt"><td colspan="3" style="line-height:8px;">&nbsp;</td></tr>
                    </ItemSeparatorTemplate>

                </asp:ListView>
                   
                <asp:SqlDataSource ID="SqlMerch" runat="server" ConnectionString="<%$ ConnectionStrings:WillCallConnectionString %>"
                    SelectCommand="
                        SELECT m.[Id],
                            CASE WHEN	m.[tParentListing] IS NULL THEN m.[Name] + ' (All Active Inventory)'
                                ELSE	(parent.[Name] + ' - ' + ISNULL(m.[Style],'') + ' ' + ISNULL(m.[Color],'') + ' ' + ISNULL(m.[Size],'')) END as 'Name'	
                        FROM [Merch] m 
                            --we need this to get the name
                            LEFT OUTER JOIN [Merch] parent ON parent.[tParentListing] IS NULL AND m.[tParentListing] = parent.[Id] 
                            --we need to get number of other children
                            --when merch is a child - get other children with same parent
                            LEFT OUTER JOIN [Merch] otherChildren ON m.[tParentListing] IS NOT NULL AND m.[tParentListing] = otherChildren.[tParentListing] AND otherChildren.[bActive] = 1
    
                        WHERE m.[bActive] = 1 and (m.[tParentListing] IS NULL OR m.[iAvailable] > 0) AND 
                            CASE WHEN m.[tParentListing] IS NULL AND (m.[vcDeliveryType] = 'download' OR m.[vcDeliveryType] = 'activationcode') THEN 1 ELSE  
                            CASE WHEN (parent.[vcDeliveryType] = 'download' OR parent.[vcDeliveryType] = 'activationcode') THEN 1 ELSE 0 END 
                            END = 1 
                        GROUP BY m.[Id], CASE WHEN	m.[tParentListing] IS NULL THEN m.[Name] + ' (All Active Inventory)'
                                ELSE	(parent.[Name] + ' - ' + ISNULL(m.[Style],'') + ' ' + ISNULL(m.[Color],'') + ' ' + ISNULL(m.[Size],'')) END
                        HAVING COUNT(otherChildren.[Id]) <> 1         
                        ORDER BY 'Name' " >            
                </asp:SqlDataSource>

            </ContentTemplate>
        </asp:UpdatePanel>
    </div>  

    <asp:Panel ID="pnlPopup" runat="server" class="jqhead rounded iit" style="display:none;">

        <h3 class="entry-title">Add A New Bundle...</h3>
        <asp:UpdatePanel ID="updPnlCustomerDetail" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Button id="btnShowPopup" runat="server" style="display:none" />
                <cc1:ModalPopupExtender ID="mdlPopup" runat="server" 
                    TargetControlID="btnShowPopup" PopupControlID="pnlPopup" 
                    CancelControlID="btnCancelForm" BackgroundCssClass="modalBackground" 
                    />
                <asp:ValidationSummary ID="ValidationSummary1" CssClass="validationsummary" Width="70%" HeaderText="" ValidationGroup="package" runat="server" />            

                <asp:FormView ID="FormView1" Width="100%" runat="server" DataKeyNames="Id" Visible="false" DefaultMode="Insert" 
                    OnDataBinding="FormView1_DataBinding" 
                    OnDataBound="FormView1_DataBound" 
                    OnItemCommand="FormView1_ItemCommand" 
                    OnItemInserting="FormView1_ItemInserting" 
                    OnModeChanging="FormView1_ModeChanging">
                    <InsertItemTemplate>
                            
                        <table border="0" cellspacing="0" cellpadding="0" width="100%" class="edittabl" >                        
                            <tr>
                                <th>Name</th>
                                <td><asp:TextBox ID="txtTitle" runat="server" MaxLength="256" Width="400px" Text='<%#Bind("Title") %>' />
                                    <div class="intr">A name for the bundle. This could be "free t-shirt with purchase" "receive a poster with purchase of merch item", 
                                        "add a shirt for $5 when you buy merch", "buy one get one free", etc</div>
                                </td>
                            </tr>                                    
                            <tr>
                                <th>Description</th>
                                <td><asp:TextBox ID="txtComment" runat="server" MaxLength="500" Width="400px" Text='<%#Bind("Comment") %>' />
                                    <div class="intr">Extra instructions for the offer. "may delay your shipment", "will be sent separately"</div>
                                </td>
                            </tr>
                            <tr>
                                <th>Req Parent Qty</th>
                                <td><asp:DropDownList ID="ddlParentQty" runat="server" OnDataBinding="ddlQty_DataBinding" />
                                    <div class="intr">The number of parent items needed to be in the cart to enable the child items.</div>
                                </td>
                            </tr>       
                            <tr>
                                <th>Price</th>
                                <td><asp:TextBox ID="txtPrice" runat="server" MaxLength="10" Width="60px" Text='<%#Bind("Price") %>' />
                                    <div class="intr">The cost of this particular selection.</div>
                                </td>
                            </tr>
                            <tr>
                                <th>Priced Per Selection</th>
                                <td><asp:CheckBox ID="chkPriced" runat="server" Checked='<%#Bind("PricedPerSelection") %>' />
                                    <div class="intr">Checking this box will cause the bundle to be priced per selection as opposed to a one-time cost per bundle.</div>
                                </td>
                            </tr>
                            <tr>
                                <th>Max Selections</th>
                                <td><asp:DropDownList ID="ddlMaxSelection" runat="server" OnDataBinding="ddlQty_DataBinding" />
                                    <div class="intr">The number of selections allowed within the bundle. 1 = dropdown, more = checkbox list.</div>
                                </td>
                            </tr>
                        </table>
                    </InsertItemTemplate>
                </asp:FormView>
                <span class="cmdsection modal-section">
                    <asp:LinkButton Width="20px" ID="btnSaveForm" runat="server" Cssclass="btn-save" ToolTip="Save" CausesValidation="false"
                            CommandName="Insert" Text="Save" OnClick="btnFormView1Save_Click" />
                    <asp:LinkButton Width="20px" ID="btnCancelForm" runat="server" Cssclass="btn-cancel" ToolTip="Cancel" CausesValidation="false"
                        CommandName="Cancel" Text="Cancel" />
                    <asp:CustomValidator ID="CustomValidation" runat="server" ValidationGroup="package" 
                        Display="Static" CssClass="validator">*</asp:CustomValidator>
                </span>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
</div>