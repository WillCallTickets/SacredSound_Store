<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Merch_Item.ascx.cs" Inherits="WillCallWeb.Admin.AdminControls.Merch_Item" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register src="/Components/Util/CalendarClock.ascx" tagname="CalendarClock" tagprefix="uc1" %>

<%@ Register src="Merch_Inventory.ascx" tagname="Merch_Inventory" tagprefix="uc2" %>

<div id="srceditor">
    <div id="merchitem">        
        <div class="jqhead rounded">
            <%if (Atx.CurrentMerchRecord != null && FormView1.CurrentMode != FormViewMode.Insert)
                  {%>
            <div class="cmdsection">
                <asp:Button ID="btnSave" ValidationGroup="merch" CausesValidation="false" runat="server" CommandName="Update" 
                    Text="Save" CssClass="btnmed tpcmd" OnClick="btnSave_Click" />
                <asp:Button ID="btnCancel" CausesValidation="false" runat="server" CommandName="Cancel" 
                    Text="Cancel" CssClass="btnmed tpcmd" OnClick="btnCancel_Click" />
                <asp:Button ID="btnDelete" CausesValidation="false" runat="server" CommandName="Delete" 
                    Text="Delete" CssClass="btnmed tpcmd" OnClick="btnDelete_Click" />
                <asp:Button ID="btnNew" CausesValidation="false" runat="server" CommandName="New" 
                    Text="New" CssClass="btnmed tpcmd" OnClick="btnNew_Click" />
                <asp:Button ID="btnSync" CausesValidation="false" runat="server" CommandName="sync" 
                    Text="Sync Inventory" Width="100px" CssClass="btnmed tpcmd" OnClick="btnSync_Click" />                    
                <asp:Button ID="btnDisplay" runat="server" CausesValidation="false" CommandName="view" Text="Display Item" Width="100px"
                    CssClass="btnmed tpcmd" OnClientClick='doPagePopup("/Admin/Display_Merch.aspx", "false"); ' />                            
                <asp:CustomValidator ID="CustomValidation" Display="static" runat="server" ValidationGroup="merch" CssClass="validator">*</asp:CustomValidator>                
            </div>
            <%} %>
            
            <asp:FormView ID="FormView1" Width="100%" runat="server" DataKeyNames="Id" DefaultMode="Edit" 
                OnDataBinding="FormView1_DataBinding" 
                OnDataBound="FormView1_DataBound" 
                OnItemCommand="FormView1_ItemCommand" 
                OnItemDeleting="FormView1_ItemDeleting" 
                OnItemInserting="FormView1_ItemInserting" 
                OnItemUpdating="FormView1_ItemUpdating" 
                OnItemCreated="FormView1_ItemCreated"
                OnModeChanging="FormView1_ModeChanging">
                <EditItemTemplate>
                    <div class="jqpnl rounded eit">
                    <h3 class="entry-title">
                <%if (Atx.CurrentMerchRecord != null && FormView1.CurrentMode != FormViewMode.Insert){%>
                    <%=Atx.CurrentMerchRecord.DisplayNameWithAttribs%>
                    <%if (Atx.CurrentMerchRecord.Allotment == 0)
                      {%>
                      <span style="color:red;font-size:16px;font-weight:bold;">There is no inventory allotment for this product</span>
                    <%} %>
                    <%if (Atx.CurrentMerchRecord.PriceHasMultipleLevelsInInventory)
                      {%>
                      <div style="color:red;font-size:16px;font-weight:bold;">
                        There are multiple price levels for this product. Changing the master price will affect all inventory pricing. Setting a sale price will override all inventory pricing.
                      </div>
                      <%} %>
                <%} else {%>Add A New Item<%}%></h3>
                    <table border="0" cellspacing="0" cellpadding="0" width="100%" class="edittabl">
                        <tr>
                            <th>Direct Link</th>
                            <td colspan="5">
                                <input type="text" readonly="readonly" size="80" value='<%# Eval("SalesUrl") %>' />
                                <%if (Atx.CurrentMerchRecord.IsInternalOnly)
                                  {%>
                                  <span style="color:red;font-size:16px;font-weight:bold;">This item is for internal display only!</span>
                                <%} %>
                            </td>
                        </tr>
                        <tr>
                            <th><asp:Button ID="linkAddCat" CssClass="btnmed" Width="100px" runat="server" Text="Add Selected" CommandName="addcat" CausesValidation="false" /></th>
                            <td colspan="5">
                                <asp:DropDownList ID="ddlCategories" runat="server" Width="300px">
                                    <asp:ListItem Text="<-- Merchandise Categories -->" Value="0" />
                                </asp:DropDownList>
                                <asp:Button ID="linkNewCat" CssClass="btnmed" runat="server" Text="Create New Category" Width="200px" CommandName="newcat" CausesValidation="false" />
                            </td>
                        </tr>
                        <tr>
                            <th>
                                <asp:Button ID="linkDeleteCat" CssClass="btnmed" Width="100px" runat="server" Text="Delete Selected" CommandName="deletecat" />
                            </th>
                            <td colspan="5"><asp:ListBox ID="listCategories" runat="server" Rows="5" Width="508px" Height="60px" /></td>
                        </tr>
                        <tr>
                            <th><span class="intr" style="padding-right:16px;"><%= Atx.CurrentMerchRecord.Id.ToString()%></span>Name</th>
                            <td colspan="5"><asp:TextBox ID="txtName" runat="server" Width="500px" MaxLength="256" Text='<%#Bind("Name") %>'></asp:TextBox></td>
                        </tr>    
                        <tr>
                            <th>Short Desc</th>
                            <td colspan="5">
                                <asp:TextBox ID="txtShortDescription" runat="server" Width="500px" 
                                    MaxLength="300" Text='<%#Bind("ShortText") %>'></asp:TextBox> <span class="intr">Only shows on item display page</span></td>
                        </tr>                    
                        <tr>
                            <th>Display Template</th>
                            <td colspan="5">
                                <asp:DropDownList ID="ddlTemplate" runat="server" Width="250px" OnDataBound="ddlTemplate_DataBound" 
                                    datasource='<%#Enum.GetNames(typeof(Wcss._Enums.MerchDisplayTemplate)) %>'/> 
                                <span class="intr">Legacy and ThreeColumn are the only methods that will auto-display pictures</span>
                            </td>
                        </tr>
                        
                        <tr>
                            <th style="vertical-align:top;">
                                Description 
                                <div style="margin-top:8px;">
                                    <asp:Button ID="btnWys" CausesValidation="false" runat="server" CommandName="Wys" Text="Edit" Width="100px" 
                                        CssClass="btnmed ov-trigger" Tooltip="/Admin/AdminControls/Wysiwyg/Wysiwyg.aspx?context=m&ctrl=" rel="#overlay-wysiwyg" />
                                    <br /><br />
                                    <asp:Button ID="btnDisplay" runat="server" CausesValidation="false" CommandName="view" Text="Display Item" Width="100px"
                                        CssClass="btnmed" OnClientClick='doPagePopup("/Admin/Display_Merch.aspx", "false"); ' />
                                </div>
                            </th>
                            <td colspan="5" style="width:500px;">
                                <asp:Literal ID="litDesc" runat="server"  />
                            </td>
                        </tr>
                        <tr><td colspan="6"><hr /></tr>
                        <!-- do all spacing in this row as it has all the cells -->
                        <tr>
                            <th style="width:150px;">Price $</th>
                            <td style="width:150px;">
                                <asp:TextBox ID="txtPrice" runat="server" Text='<%#Bind("Price") %>' MaxLength="8" Width="60px" />
                                <span class="intr">affects inventory $</span>
                            </td>
                            <th style="width:150px;">Sale Price $</th>
                            <td style="width:100px;"><asp:TextBox ID="txtSalePrice" runat="server" Text='<%#Bind("SalePrice") %>' 
                                MaxLength="8" Width="60px" OnDataBinding="txtSalePrice_DataBinding" /></td>
                            <th style="width:150px;">Use Sale Price</th>
                            <td style="width:100%;" class="listing-row"><asp:CheckBox ID="chkUseSalePrice" runat="server" Checked='<%#Bind("UseSalePrice") %>' /></td>
                        </tr>
                        <tr>
                            <th>Delivery Type</th>
                            <td><input type="text" value='<%#Eval("DeliveryType") %>' readonly="readonly" /></td>
                            <th>Weight</th>
                            <td><asp:TextBox ID="txtWeight" runat="server" Text='<%#Bind("Weight") %>' MaxLength="5" Width="60px" /></td>
                            <th>Max/Order</th>
                            <td>
                                <asp:TextBox ID="txtMax" runat="server" Text='<%#Bind("MaxQuantityPerOrder") %>' MaxLength="3" Width="70px" />
                            </td>
                        </tr>
                        <tr>
                            <th>Flat Shipping $</th>
                            <td><asp:TextBox ID="txtFlatShip" runat="server" Text='<%#Bind("FlatShip", "{0:n2}") %>' MaxLength="8" Width="60px" /></td>
                            <th>Flat Method</th>
                            <td colspan="3" style="width:100%;"><asp:TextBox ID="txtFlatMethod" runat="server" Text='<%#Bind("FlatMethod") %>' />
                                <asp:Button id="btnShipNotes" runat="server" CausesValidation="false" Text="Shipping Notes"
                                    cssclass="btnmed" Width="100px"
                                    OnClientClick="javascript: doPagePopup('/Admin/Inst_MerchShipping.html', 'false');" />
                            </td>
                        </tr>
                        <tr><td colspan="6"><hr /></tr>
                        <tr>
                            <td colspan="6" class="listing-row">
                                Active
                                <asp:CheckBox ID="chkActive" runat="server" Checked='<%#Eval("bActive") %>' />
                                SoldOut
                                <asp:CheckBox ID="chkSoldOut" runat="server" Checked='<%#Eval("bSoldOut") %>' />
                                Internal
                                <asp:CheckBox ID="chkInternal" runat="server" Checked='<%#Eval("bInternalOnly") %>' />
                                Featured
                                <asp:CheckBox ID="chkFeatured" runat="server" Checked='<%#Eval("bFeaturedItem") %>' />
                                Ship Separate
                                <asp:CheckBox ID="chkSeparate" runat="server" Checked='<%#Bind("IsShipSeparate") %>' />
                                Low Rate Qualified                            
                                <asp:CheckBox ID="chkLowRate" runat="server" Checked='<%#Eval("IsLowRateQualified") %>' />
                            </td>
                        </tr>
                        <tr><td colspan="6"><hr /></tr>
                        <tr>
                            <td colspan="6" class="listing-row">
                                Allotment
                                <span class="nbr"><%#Eval("Allotment")%></span>
                                Damaged
                                <span class="nbr"><%#Eval("Damaged")%></span>
                                Pending
                                <span class="nbr"><%#Eval("Pending")%></span>
                                Sold
                                <span class="nbr"><%#Eval("Sold")%></span>
                                Available
                                <span class="nbr"><%#Eval("Available")%></span>
                                Refunds
                                <span class="nbr"><%#Eval("Refunded")%></span>
                            </td>
                        </tr>
                    </table>
                    </div>
                </EditItemTemplate>
                <InsertItemTemplate>
                    <div class="jqpnl rounded iit">
                    <table border="0" cellspacing="0" cellpadding="0" width="800px" class="edittabl">
                        <tr>
                            <th>Category</th>
                            <td style="width:100%;">
                                <asp:DropDownList ID="ddlCategories" runat="server" AutoPostBack="false" Width="250px">
                                    <asp:ListItem Text="<-- Select Merchandise Category -->" Value="0" />
                                </asp:DropDownList>
                                <asp:Button ID="linkNewCat" CssClass="btnmed" Width="200px" runat="server" 
                                    Text="Create New Category" CommandName="newcat" CausesValidation="false" />
                            </td>
                        </tr>
                        <tr>
                            <th>Name</th>
                            <td><asp:TextBox ID="txtName" runat="server" Width="450px" MaxLength="256" Text='<%#Bind("Name") %>'></asp:TextBox></td>
                        </tr>
                         <tr>
                            <th>Delivery Type</th>
                            <td>
                                <asp:DropDownList ID="ddlDeliveryType" Enabled="true" runat="server" Width="250px" 
                                    OnDataBound="ddlDeliveryType_DataBound" datasource='<%#Enum.GetNames(typeof(Wcss._Enums.DeliveryType)) %>'/>
                            <div class="intr">
                                If you can send it in the mail - needs to be parcel.<br />
                                Gift Certificates should be gift certificate<br />
                                Downloads available on this site, or where the codes are generated by this site should be downloads<br />                                
                                Use activation code for 3rd party code activation or when the activation code is provided outside this site. EVEN if the product is a download
                            </div>
                            </td>
                        </tr>
                        <tr>
                            <th>Low Rate Qualified</th>
                            <td>
                                <asp:CheckBox ID="chkLowRate" runat="server" Checked='<%#Bind("IsLowRateQualified") %>' Text="" />
                                <div class="intr">Only parcels can be qualified for a low rate.</div>
                            </td>
                        </tr>
                        <tr>
                            <th>Weight</th>
                            <td><asp:TextBox ID="txtWeight" runat="server" Width="250px" MaxLength="5" Text='<%#Bind("mWeight") %>'></asp:TextBox>
                            <div class="intr">Required for parcels</div>
                            </td>
                        </tr>
                    </table>
                    </div>
                </InsertItemTemplate>
            </asp:FormView>

            <asp:ValidationSummary ID="ValidationSummary1" CssClass="validationsummary" HeaderText="" ValidationGroup="merch" runat="server" />

           <%if (FormView1.CurrentMode != FormViewMode.Insert)
             {%>
            <div class="jqpanel1 rounded"> 
                <uc2:Merch_Inventory ID="Merch_Inventory1" runat="server" />
            </div>
           <%} %>
            <%else
              {%>
            <div class="cmdsection" style="margin-top:12px;">
                <asp:Button ID="Button1" ValidationGroup="merch" CausesValidation="false" runat="server" CommandName="Update" 
                    Text="Save" CssClass="btnmed" OnClick="btnSave_Click" />
                <asp:Button ID="Button2" CausesValidation="false" runat="server" CommandName="Cancel" 
                    Text="Cancel" CssClass="btnmed" OnClick="btnCancel_Click" />
                <asp:CustomValidator ID="CustomValidator1" Display="static" runat="server" ValidationGroup="merch" CssClass="validator">*</asp:CustomValidator>                
            </div>
            <%} %>
        </div>
    </div>
</div>

<div class="admin_overlay" id="overlay-wysiwyg" style="display:none;">
	<div class="contentWrap"></div>
</div>

<script type="text/javascript" src="/JQueryUI/admin-overlay.js"></script>