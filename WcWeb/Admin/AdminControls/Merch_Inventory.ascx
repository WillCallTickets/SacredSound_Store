<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Merch_Inventory.ascx.cs" Inherits="WillCallWeb.Admin.AdminControls.Merch_Inventory" %>
       
    <asp:ListView ID="lstInventory" runat="server" DataKeyNames="Id" ItemPlaceholderID="ListViewContent" InsertItemPosition="None" 
        ondatabinding="lstInventory_DataBinding" 
        OnItemDataBound="lstInventory_ItemDataBound"
        OnItemEditing="lstInventory_ItemEditing"
        OnItemCanceling="lstInventory_ItemCancelling" 
        OnItemUpdating="lstInventory_ItemUpdating"
        OnItemDeleting="lstInventory_ItemDeleting"
        OnItemInserting="lstInventory_ItemInserting" 
        OnItemCommand="lstInventory_ItemCommand"
        OnItemCreated="lstInventory_ItemCreated"
            >
        <EmptyDataTemplate>
            <div class="lstempty">
                No Inventory Items...
                <asp:Button ID="btnEditColor" CssClass="btntny" CausesValidation="false" runat="server" 
                    Text="Add New Item" CommandName="New" />
            </div>
        </EmptyDataTemplate>
        <LayoutTemplate>
            <div class="cmdsection">
                INVENTORY 
                <span class="intr">
	    	        If you would like to use multiple pricing levels - those must be keyed off of style
                </span>
                <asp:Label ID="lblAttribsInSync" runat="server" />
            </div>
            <table border="0" cellpadding="0" cellspacing="0" width="100%" class="lsttbl invtbl">
                <tr class="hdr">
                    <th>&nbsp;</th>
                    <th style="width:85%;text-align:left;">Style/Color</th>
                    <th style="width:15%;text-align:left;">Size</th>
                    <th>Active</th>
                    <th>SO</th>
                    <th>Allot</th>
                    <th>Dmg</th>
                    <th>Pend</th>
                    <th>Sold</th>
                    <th>Avail</th>
                    <th>Ref</th>
                    <th>Price</th>
                    <th>&nbsp;</th>
                </tr>
                <tbody runat="server" id="ListViewContent" class="invedt" />
                <tr class="invhed">
                    <td colspan="13" style="text-align:left;">
                        <div class="cmdsection">
                            <asp:Button ID="btnNewItem" CssClass="btntny" CausesValidation="false" runat="server" 
                                Text="Add New Item" CommandName="New" />
                        </div> 
                    </td>
                </tr>
            </table>
        </LayoutTemplate>
        <ItemTemplate>
            <tr>
                <td>
                    <asp:LinkButton Width="20px" Id="btnEdit" ToolTip="Edit" CssClass="btnselect" runat="server" CommandName="edit"
                        CommandArgument='<%#Eval("Id") %>' CausesValidation="false" />
                </td>
                <td style="text-align:left;"><%#Eval("Style") %> / <%#Eval("Color") %></td>
                <td style="text-align:left;white-space:nowrap;"><%#Eval("Size") %></td>
                <td><asp:checkbox ID="chkActive" runat="server" Enabled="false" Checked='<%#Eval("IsActive") %>' /></td>
                <td><asp:checkbox ID="chkSoldOut" runat="server" Enabled="false" Checked='<%#Eval("IsSoldOut") %>' /></td>
                <td><%#Eval("Allotment") %></td>
                <td><%#Eval("Damaged") %></td>
                <td><%#Eval("Pending") %></td>
                <td><%#Eval("Sold") %></td>
                <td><%#Eval("Available") %></td>
                <td><%#Eval("Refunded") %></td>
                <td><%#Eval("Price", "{0:c}") %></td>
                <td style="white-space:nowrap;">
                    <asp:LinkButton Width="20px" Id="btnDelete" CssClass="btndelete" runat="server" CommandName="Delete"
                        CommandArgument='<%#Eval("Id") %>' ToolTip="Delete" CausesValidation="false"
                        OnClientClick='return confirm("Are you sure you want to delete this row?")' />
                    <asp:CustomValidator ID="CustomValidation" runat="server" ValidationGroup="merch" 
                        CssClass="validator" Display="Static" ErrorMessage="CustomValidator">*</asp:CustomValidator>
                </td>
            </tr>
        </ItemTemplate>
        <EditItemTemplate>
            <tr class="invhed">
                <td colspan="13" style="text-align:left;"><%#Eval("Id") %> - Editing... <%#Eval("Style") %> / <%#Eval("Color") %> / <%#Eval("Size") %></td>
            </tr>
            
            <tr class="invport">
                <td colspan="13">
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" class="edttbl">
                        <tr>
                            <th><a href="javascript: alert('Only use this pricing if the inventoried product specifically states its price.')" class="infomark">?</a>
                                Price</th>
                            <td>
                                <asp:TextBox ID="txtPrice" Text='<%#Bind("mPrice", "{0:n2}") %>' runat="server" MaxLength="10" Width="75px" />
                            </td>
                            <td style="white-space:nowrap;padding-left:22px;">
                                <asp:checkbox ID="chkActive" runat="server" Checked='<%#Bind("IsActive") %>' Text=" Active" />
                            </td>
                            <td style="white-space:nowrap;">
                                <asp:checkbox ID="chkSoldOut" runat="server" Checked='<%#Bind("IsSoldOut") %>' Text=" Sold Out" />
                            </td>
                            <th>Allotment</th>
                            <td><%#Eval("Allotment") %></td>
                            <th>Damaged</th>
                            <td><%#Eval("Damaged") %></td>
                            <th>Sold</th>
                            <td><%#Eval("Sold") %></td>
                            <th>Available</th>
                            <td style="white-space:nowrap;"><%#Eval("Available") %></td>
                            <th>Refunded</th>
                            <td><%#Eval("Refunded") %></td>
                            <td style="width:50%;">&nbsp;</td>
                            <td colspan="1">&nbsp;</td>
                        </tr>
                        <tr>
                            <th>Style</th>
                            <td colspan="14" style="width:100%;">
                                <asp:DropDownList ID="ddlStyle" runat="server" Width="100%">
                                    <asp:ListItem Text="[-- Style --]" Value="0" /></asp:DropDownList>
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <th>Create Style &raquo;</th>
                            <td colspan="14" style="width:100%;">
                                <asp:TextBox ID="txtStyle" runat="server" MaxLength="256" Width="99%" /></td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <th>Color</th>
                            <td colspan="14" style="width:100%;white-space:nowrap;">
                                <asp:DropDownList ID="ddlColor" runat="server" Width="100%">
                                    <asp:ListItem Text="[-- Color --]" Value="0" /></asp:DropDownList>
                            </td>
                            <td>
                                <asp:Button ID="btnEditColor" CssClass="btntny" CausesValidation="false" runat="server" 
                                    Text="Color Editor" OnClick="btnEditor_Click" />
                            </td>
                        </tr>
                        <tr>
                            <th>Size</th>
                            <td colspan="14">
                                <asp:DropDownList ID="ddlSize" runat="server" Width="100%" >
                                    <asp:ListItem Text="[-- Size --]" Value="0" /></asp:DropDownList>
                            </td>
                            <td>
                                <asp:Button ID="btnEditSize" CssClass="btntny" CausesValidation="false" runat="server" 
                                    Text="Size Editor" OnClick="btnEditor_Click" />
                            </td>
                        </tr>
                        <tr>
                            <th>Damage</th>
                            <td colspan="2">
                                <asp:TextBox ID="txtDmg" runat="server" MaxLength="5" Width="200px" />
                            </td>
                            <td colspan="12">
                                <asp:Button ID="btnDmg" CssClass="btntny" CausesValidation="false" runat="server" 
                                    Text="Report Damage" CommandName="reportdamage" CommandArgument='<%#Eval("Id") %>' />
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <th style="vertical-align:top" valign="top">Allotment</th>
                            <td colspan="2">
                                <asp:TextBox ID="txtAllot" Text='<%#Bind("Allotment") %>' runat="server" MaxLength="5" Width="200px" />
                                <asp:Label ID="lblAllot" Text='<%#Bind("Allotment") %>' runat="server" />
                            </td>
                            <td colspan="13">&nbsp;</td>
                        </tr>
                        <tr>
                            <th style="vertical-align:top;"><%if (Atx.CurrentMerchRecord.IsActivationCodeDelivery)
                                                              { %>Enter codes<%} %></th>
                            <td colspan="14">
                                <div style="float:left;">                                    
                                    <asp:TextBox ID="lstAllot" runat="server" TextMode="MultiLine" Width="200px" Height="300px" />
                                </div>
                                <div style="float:left;">
                                    <asp:Button cssclass="btntny" ID="btnRemoveUnused" runat="server" 
                                        CausesValidation="false" CommandName="removeunusedcodes" CommandArgument='<%#Eval("Id") %>' Text="Remove Unused Codes" />
                                    <br /><br />
                                    <%if(Atx.CurrentMerchRecord.IsActivationCodeDelivery){ %>
                                        <div class="jqinstruction rounded">
                                            Inventory will be maintained by the number of unique codes entered for this item.                                        
                                        </div>
                                    <%} %>    
                                </div>
                            </td>
                            <td>&nbsp;</td>
                        </tr>

                        <tr>
                            <td colspan="16" style="white-space:nowrap;" class="cmdsection">
                                <asp:Button cssclass="btntny" ID="btnInsert" runat="server" 
                                    CausesValidation="false" CommandName="Update" Text="Save" />
                                <asp:Button ID="btnCancel" cssclass="btntny" runat="server"
                                        CausesValidation="false" CommandName="Cancel" Text="Cancel/Close" />
                                <asp:CustomValidator ID="CustomValidation" runat="server" ValidationGroup="merch" 
                                    CssClass="validator" Display="Static" ErrorMessage="CustomValidator">*</asp:CustomValidator>
                                <span style="font-size:10px;">Style, Color and Size cannot be changed after items have been sold</span>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr class="invhed"><td colspan="13" style="line-height:3px !important;">&nbsp;</td></tr>
        </EditItemTemplate>
                
        <InsertItemTemplate>
            <tr class="invhed"><td colspan="13" style="text-align:left;">Creating a New Inventory Item...</td></tr>
            <tr class="invport">
                <td colspan="13">
                    <table border="0" cellspacing="0" cellpadding="0" width="100%" class="edttbl">
                        <tr>
                            <th>Style</th>
                            <td style="width:100%;">
                                <asp:DropDownList ID="ddlStyle" runat="server" Width="100%">
                                    <asp:ListItem Text="[-- Style --]" Value="0" /></asp:DropDownList>
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <th>Create Style &raquo;</th>
                            <td style="width:100%;">
                                <asp:TextBox ID="txtStyle" runat="server" MaxLength="256" Width="100%" /></td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <th>Color</th>
                            <td style="width:100%;white-space:nowrap;">
                                <asp:DropDownList ID="ddlColor" runat="server" Width="100%">
                                    <asp:ListItem Text="[-- Color --]" Value="0" /></asp:DropDownList>
                            </td>
                            <td>
                                <asp:Button ID="btnEditColor" CssClass="btntny" CausesValidation="false" runat="server" 
                                    Text="Color Editor" OnClick="btnEditor_Click" />
                            </td>
                        </tr>
                        <tr>
                            <th>Size</th>
                            <td>
                                <asp:DropDownList ID="ddlSize" runat="server" Width="100%" >
                                    <asp:ListItem Text="[-- Size --]" Value="0" /></asp:DropDownList>
                            </td>
                            <td>
                                <asp:Button ID="btnEditSize" CssClass="btntny" CausesValidation="false" runat="server" 
                                    Text="Size Editor" OnClick="btnEditor_Click" />
                            </td>
                        </tr>
                        <tr>
                            <th><a href="javascript: alert('Only enter a price here if different from the parent item.')" class="infomark">?</a>
                                Price</th>
                            <td><asp:TextBox ID="txtInvPrice" runat="server" MaxLength="10" Width="50px" />
                                <span>Only enter a price here if different from the parent item</span>
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <th style="vertical-align:top">Allotment</th>
                            <td>
                                <asp:TextBox ID="txtAllot" runat="server" MaxLength="5" Width="200px" />
                                <span style="float:left;">
                                    <asp:TextBox ID="lstAllot" runat="server" TextMode="MultiLine" Width="200px" Height="300px" />
                                </span>
                                <%if(Atx.CurrentMerchRecord.IsActivationCodeDelivery){ %>
                                <span style="font-size:14px;line-height:normal;float:left;margin:24px 0 0 24px;">
                                    Enter codes here for inventory.<br />
                                    Inventory will be maintained by the number of unique codes entered for this item.<br />
                                    You may wait and enter codes after creating the inventory item.
                                </span>
                                <%} %>
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr class="invport">
                <td colspan="13" style="text-align:left;">
                    <div class="cmdsection">
                        <asp:Button ID="btnSave" cssclass="btntny" CausesValidation="false" runat="server" 
                            Text="Save New Item" CommandName="Insert" />
                        <asp:Button ID="btnCancel" cssclass="btntny" CausesValidation="false" runat="server" 
                            Text="Cancel/Close" CommandName="Cancel" />
                        <asp:CustomValidator ID="CustomValidation" runat="server" ValidationGroup="merch" 
                            CssClass="validator" Display="Static" ErrorMessage="CustomValidator">*</asp:CustomValidator>
                        <span style="font-size:10px;">Be sure to match sizes, colors and styles! Mismatched items will not select properly in the cart</span>
                    </div> 
                </td>
            </tr>   
        </InsertItemTemplate>
                
        <ItemSeparatorTemplate>
            <tr class="sprt"><td colspan="13" style="line-height:3px;">&nbsp;</td></tr>
        </ItemSeparatorTemplate>

    </asp:ListView> 
   