<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Orders_View.ascx.cs" Inherits="WillCallWeb.Admin.AdminControls.Orders_View" %>
<div id="orderview">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="jqhead rounded">
                <table border="0" cellpadding="0" cellspacing="0" width="100%" class="edittabl" style="margin:0;">
                    <tr>
                        <th>View Order</th>
                        <td><asp:CustomValidator ID="CustomValidation" runat="server" ValidationGroup="order" CssClass="validator" 
                                Display="Static" ErrorMessage="CustomValidator">*</asp:CustomValidator></td>
                        <td style="width:100%;">                            
                            <asp:Button ID="btnShip" runat="server" CssClass="btnmed" Width="80px" Text="Shipping" 
                                CommandName="shipping" OnClick="btnLink_Click" CausesValidation="false" />
                            <asp:Button ID="btnConfirm" runat="server" CssClass="btnmed" Width="80px" Text="Confirmation" 
                                CommandName="confirmation" OnClick="btnLink_Click" CausesValidation="false" />
                            <asp:Button ID="btnCustSales" runat="server" CssClass="btnmed" Width="80px" Text="Cust Sales" 
                                CommandName="custsales" OnClick="btnLink_Click" CausesValidation="false" />
                            <%if (this.Page.User.IsInRole("Administrator")){%>
                            <asp:Button ID="btnRefund" runat="server" CssClass="btnmed" Width="80px" Text="Refund" 
                                CommandName="refund" OnClick="btnLink_Click" CausesValidation="false" />
                            <asp:Button ID="btnExchange" runat="server" CssClass="btnmed" Width="80px" Text="Exchange" 
                                CommandName="exchange" OnClick="btnLink_Click" CausesValidation="false" />
                            <%} %>
                        </td>
                    </tr>
                </table>            
            </div>
            <div class="jqpnl rounded" style="margin-bottom:2px;">
                <h3 class="entry-title"><asp:Literal ID="litUserEditor" runat="server" /></h3>
            </div>
            <asp:ValidationSummary HeaderText="The following errors ocurred:" ID="ValidationSummary1" runat="server" 
                ValidationGroup="order" CssClass="validationsummary" Width="100%" />
            <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                <tr>
                    <td style="padding-right:6px;">
                        <div class="jqedt rounded">
                            <table border="0" cellpadding="0" cellspacing="0" width="100%" class="hedtbl">
                                <tr>
                                    <th>Invoice Details</th>
                                </tr>
                            </table>
                            <asp:FormView ID="FormInvoice" runat="server" DataKeyNames="Id,Email" DataSourceID="SqlInvoice" GridLines="None" 
                                OnDataBound="FormInvoice_DataBound">
                                <ItemTemplate>
                                    <table border="0" cellspacing="0" cellpadding="0" class="edttbl">
                                        <tr><th>Id</th><td colspan="3"><%# Eval("Id") %></td></tr>
                                        <tr><th>Date</th><td colspan="3"><%# Eval("dtInvoiceDate", "{0:MM/dd/yyyy hh:mmtt}") %></td></tr>
                                        <tr><th>Type</th><td colspan="3"><%# Eval("OrderType")%></td></tr>
                                        <tr><th>Status</th><td><asp:Literal ID="litStatus" runat="server" /></td>
                                            <th>Last Four</th><td><asp:Literal ID="litLastFour" runat="server" /></td>
                                        </tr>
                                        <tr><th>Unique Id</th><td colspan="3"><%# Eval("UniqueId") %></td></tr>
                                        <tr><th>Creator</th><td colspan="3"><%# Eval("Creator")%></td></tr>
                                        <tr><th>Email</th><td colspan="3"><%#Eval("Email") %></td></tr>
                                        <tr><th>User Id</th><td colspan="3" style="width:100%;"><%# Eval("UserId") %></td></tr>
                                        <tr><th>S&amp;H Calc</th><td style="width:50%;"><%# Eval("mHandlingComputed", "{0:c}")%></td>
                                            <th>Ship Actual</th><td style="width:50%;"><%# Eval("mActualShipping", "{0:c}")%></td></tr>
                                            
                                        <tr><th>Paid</th><td><%# Eval("mTotalPaid", "{0:c}")%></td>
                                            <th>Refunds</th><td><%# Eval("mTotalRefunds", "{0:c}")%></td></tr>
                                        <tr><th>Net Paid</th><td><%# Eval("mNetPaid", "{0:c}")%></td>
                                            <th>Due</th><td><%# Eval("mBalanceDue", "{0:c}")%></td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </asp:FormView>
                        </div>
                    </td>
                    <td>
                        <asp:FormView ID="FormInvoiceBillShip" runat="server" DataKeyNames="Id,UserName" DataSourceID="SqlInvoiceBillShip" 
                            OnItemUpdating="FormInvoiceBillShip_ItemUpdating" OnItemUpdated="FormInvoiceBillShip_ItemUpdated" OnDataBound="FormInvoiceBillShip_DataBound" 
                            OnItemCommand="FormInvoiceBillShip_ItemCommand">
                            <EditItemTemplate>
                                <div class="jqedt rounded">
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%" class="hedtbl">
                                        <tr>
                                            <th>Billing and Shipping</th>     
                                            <td style="text-align:right;width:100%;">
                                                <asp:CustomValidator ID="CustomValidation" runat="server" ValidationGroup="order" CssClass="invisible" Display="Static" ErrorMessage="CustomValidator">*</asp:CustomValidator>
                                                <asp:Button ID="UpdateButton" CssClass="btnmed" runat="server" ValidationGroup="order" 
                                                    CausesValidation="True" CommandName="Update" Text="Update" />
                                                <asp:Button ID="UpdateCancelButton" CssClass="btnmed" runat="server"
                                                    CausesValidation="False" CommandName="Cancel" Text="Cancel" />
                                            </td>
                                        </tr>
                                    </table>
                                     <table border="0" cellspacing="0" cellpadding="0" class="edttbl shpng">
                                        <tr><th>Id</th><td style="width:50%;"><%# Eval("Id") %></td>
                                            <th>Same As Billing</th>
                                            <td style="width:50%;"><asp:CheckBox ID="bSameAsBillingCheckBox" runat="server" Checked='<%# Bind("bSameAsBilling") %>' /></td>
                                        </tr>
                                        <tr><th>Company</th><td><%# Eval("blCompany") %></td>
                                            <th>Ship Company</th><td style="width: 150px;"><asp:TextBox ID="CompanyNameTextBox" MaxLength="100" runat="server" Text='<%# Bind("CompanyName") %>' /></td>
                                        </tr>
                                        <tr><th>First Name</th><td><%# Eval("blFirstName")%></td>
                                            <th>Ship To First Name</th><td><asp:TextBox ID="FirstNameTextBox" MaxLength="50" runat="server" Text='<%# Bind("FirstName") %>' />
                                                <asp:customvalidator id="CustomShipFirst" runat="server" CssClass="validator" Display="Static" ValidationGroup="order"
				                                    ControlToValidate="FirstNameTextBox" ErrorMessage="Please enter the shipping first name">*</asp:customvalidator></td></tr>
                                        <tr><th>Last Name</th><td><%# Eval("blLastName")%></td>
                                            <th>Ship To Last Name</th><td><asp:TextBox ID="LastNameTextBox" MaxLength="50" runat="server" Text='<%# Bind("LastName") %>' />
                                                <asp:customvalidator id="CustomShipLast" runat="server" CssClass="validator" Display="Static" ValidationGroup="order"
				                                    ControlToValidate="LastNameTextBox" ErrorMessage="Please enter the shipping last name">*</asp:customvalidator></td></tr>
                                        <tr><th>Address1</th><td><%# Eval("blAddress1")%></td>
                                            <th>Ship To Address1</th><td><asp:TextBox ID="Address1TextBox" MaxLength="60" runat="server" Text='<%# Bind("Address1") %>' />
                                                <asp:customvalidator id="CustomShipAddress" runat="server" CssClass="validator" Display="Static" ValidationGroup="order"
				                                    ControlToValidate="Address1TextBox" ErrorMessage="Please enter the shipping address">*</asp:customvalidator></td></tr>
                                        <tr><th>Address2</th><td><%# Eval("blAddress2")%></td>
                                            <th>Ship To Address2</th><td><asp:TextBox ID="Address2TextBox" MaxLength="60" runat="server" Text='<%# Bind("Address2") %>' /></td></tr>
                                        <tr><th>City</th><td><%# Eval("blCity")%></td>
                                            <th>Ship To City</th><td><asp:TextBox ID="CityTextBox" MaxLength="40" runat="server" Text='<%# Bind("City") %>' />
                                                <asp:customvalidator id="CustomShipCity" runat="server" CssClass="validator" Display="Static" ValidationGroup="order"
				                                    ControlToValidate="CityTextBox" ErrorMessage="Please enter the shipping city">*</asp:customvalidator></td></tr>
                                        <tr><th>State/Province</th><td><%# Eval("blStateProvince")%></td>
                                            <th>Ship To State</th><td><asp:TextBox ID="StateProvinceTextBox" MaxLength="40" runat="server" Text='<%# Bind("StateProvince") %>' />
                                                <asp:customvalidator id="CustomShipState" runat="server" CssClass="validator" Display="Static" ValidationGroup="order"
				                                    ControlToValidate="StateProvinceTextBox" ErrorMessage="Please enter the shipping state/province">*</asp:customvalidator></td></tr>
                                        <tr><th>Postal Code</th><td><%# Eval("blPostalCode")%></td>
                                            <th>Ship To Zip</th><td><asp:TextBox ID="PostalCodeTextBox" MaxLength="20" runat="server" Text='<%# Bind("PostalCode") %>' />
                                                <asp:customvalidator id="CustomShipZip" runat="server" CssClass="validator" Display="Static" ValidationGroup="order"
				                                    ControlToValidate="PostalCodeTextBox" ErrorMessage="Please enter the shipping postal code">*</asp:customvalidator></td></tr>
                                        <tr><th>Country</th><td><%# Eval("blCountry")%></td>
                                            <th>Ship To Country</th><td><asp:TextBox ID="CountryTextBox" MaxLength="60" runat="server" Text='<%# Bind("Country") %>' />
                                                <asp:customvalidator id="CustomShipCountry" runat="server" CssClass="validator" Display="Static" ValidationGroup="order"
				                                    ControlToValidate="CountryTextBox" ErrorMessage="Please enter the shipping country">*</asp:customvalidator></td></tr>
                                        <tr><th>Phone</th><td><%# Eval("blPhone")%></td>
                                            <th>Ship To Phone</th><td><asp:TextBox ID="PhoneTextBox" MaxLength="25" runat="server" Text='<%# Bind("Phone") %>' />
                                                <asp:customvalidator id="CustomShipPhone" runat="server" CssClass="validator" Display="Static" ValidationGroup="order"
				                                    ControlToValidate="PhoneTextBox" ErrorMessage="Please enter the shipping phone">*</asp:customvalidator></td></tr>
                                        <tr><th valign="top">Gift Message</th>
                                            <td colspan="3"><%# Eval("ShipMessage") %></td>
                                        </tr>
                                    </table> 
                                </div>                           
                            </EditItemTemplate>
                            <ItemTemplate>
                                <div class="jqedt rounded">
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%" class="hedtbl">
                                        <tr>
                                            <th>Billing and Shipping</th>
                                            <td style="white-space:nowrap;">
                                                <asp:CustomValidator ID="CustomValidation" runat="server" ValidationGroup="order" CssClass="invisible" Display="Static" ErrorMessage="CustomValidator">*</asp:CustomValidator>
                                                <asp:Button ID="EditButton" runat="server" CssClass="btnmed" CausesValidation="False" CommandName="Edit" Text="Edit" />
                                            </td>
                                            <td style="white-space:nowrap;width:100%;text-align:right;color:Red;" class="intr">
                                                <ul>
                                                    <li>Changing address here will not change EXISTING shipments!</li>
                                                    <li>It will only update the address for CREATING FUTURE shipments</li>
                                                </ul>
                                            </td>
                                        </tr>
                                    </table>
                                    <table border="0" cellspacing="0" cellpadding="0" class="edttbl">
                                        <tr>
                                            <th>Id</th>
                                            <td style="width:50%;"><%# Eval("Id") %></td>
                                            <th>Same As Billing</th>
                                            <td style="width:50%;">
                                                <asp:CheckBox ID="bSameAsBillingCheckBox" Enabled="False" runat="server" Checked='<%# Bind("bSameAsBilling") %>' /></th>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>Company</th><td><%# Eval("blCompany") %></td>
                                            <th>Ship To</th><td><%# Eval("CompanyName") %></td></tr>
                                        <tr><th>First Name</th><td><%# Eval("blFirstName")%></td><th>Ship To First Name</th><td><%# Eval("FirstName") %></td></tr>
                                        <tr><th>Last Name</th><td><%# Eval("blLastName")%></td><th>Ship To Last Name</th><td><%# Eval("LastName") %></td></tr>
                                        <tr><th>Address1</th><td><%# Eval("blAddress1")%></td><th>Ship To Address1</th><td><%# Eval("Address1") %></td></tr>
                                        <tr><th>Address2</th><td><%# Eval("blAddress2")%></td><th>Ship To Address2</th><td><%# Eval("Address2") %></td></tr>
                                        <tr><th>City</th><td><%# Eval("blCity")%></td><th>Ship To City</th><td><%# Eval("City") %></td></tr>
                                        <tr><th>State/Prov</th><td><%# Eval("blStateProvince")%></td><th>Ship To State</th><td><%# Eval("StateProvince") %></td></tr>
                                        <tr><th>Postal Code</th><td><%# Eval("blPostalCode")%></td><th>Ship To Zip</th><td><%# Eval("PostalCode") %></td></tr>
                                        <tr><th>Country</th><td><%# Eval("blCountry")%></td><th>Ship To Country</th><td><%# Eval("Country") %></td></tr>
                                        <tr><th>Phone</th><td style="white-space:nowrap;"><%# Eval("blPhone")%></td><th>Ship To Phone</th><td><%# Eval("Phone") %></td></tr>
                                        <tr><th>Gift Message</th><td colspan="3"><%# Eval("ShipMessage") %></td></tr>
                                     </table>
                                 </div>
                            </ItemTemplate>
                        </asp:FormView>
                    </td>
                </tr>
            </table>
            <br />
            <asp:ListView ID="lstProduct" runat="server" DataKeyNames="Id" ItemPlaceholderID="ListViewContent" 
                OnDataBinding="lstProduct_DataBinding" 
                OnItemDataBound="lstProduct_ItemDataBound" 
                OnDataBound="lstProduct_DataBound"      
                onitemediting="lstProduct_ItemEditing"  
                OnItemUpdating="lstProduct_Updating" 
                 OnItemCanceling="lstProduct_ItemCanceling"
                OnItemCommand="lstProduct_Command">
                <LayoutTemplate> 
                    <div class="jqedt rounded">
                        <h3 class="entry-title">Item Listing</h3>
                        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="orders">                            
                            <div runat="server" id="ListViewContent" />
                        </table>
                    </div>
                </LayoutTemplate>
                <ItemTemplate>
                    <asp:Literal ID="litTable" runat="server" EnableViewState="false" />
                    <tr id="trCommand" runat="server">                        
                        <td colspan="99">
                            <asp:Button ID="btnEdit" CssClass="btntny" runat="server" CausesValidation="false" CommandName="Edit" 
                                CommandArgument='<%#Eval("Id") %>' Text="edit" />
                            <asp:Button ID="btnLinkShip" CssClass="btntny" runat="server" CausesValidation="false" CommandName="LinkShip" 
                                CommandArgument='<%#Eval("TInvoiceId") %>' Text="view linked shipment" />
                        </td>
                    </tr>
                </ItemTemplate>
                <EditItemTemplate>
                    <tr class="editee"><th class="contxt"><%#Eval("vcContext") %></span></th>
                        <td colspan="99" style="width:100%;">
                            <asp:Literal ID="litProductName" runat="server" />&nbsp;
                        </td>
                    </tr>
                    <tr class="editee"><th>Pricing</th>
                        <td colspan="99"><%#Eval("mPrice", "{0:n2}") %> + <%#Eval("mServiceCharge", "{0:n2}")%>svc + <%#Eval("mAdjustment", "{0:n2}")%>adj = <%#Eval("mLineItemTotal", "{0:c}") %>
                    </td></tr>
                    <tr class="editee"><th style="white-space: nowrap;">Purch Name</th><td colspan="99"><%#Eval("PurchaseName") %>&nbsp;</td></tr>
                    <tr class="editee"><th>Id / Guid / Delivery Code</th><td colspan="99"><%#Eval("Id") %> / <%#Eval("Guid") %> / <%#Eval("DeliveryCode") %></td></tr>
                    <tr class="editee"><th>Shp Method</th><td colspan="99"><%#Eval("ShippingMethod") %></td></tr>
                    <tr class="editee">
                        <th>Shp Date</th>
                        <td colspan="99" style="white-space: nowrap;"><%#Eval("dtShipped", "{0:d}")%></td>
                    </tr>
                    <tr class="editee">
                        <th>Notes</th><td colspan="99"><asp:TextBox MaxLength="500" Width="800px" Height="20px" ID="txtNotes" runat="server" Text='<%#Bind("Notes")%>' /></td>
                    </tr>
                    <tr class="editee">
                        <td colspan="99" style="padding:12px 12px 12px 12px;">
                            <div class="jqedt rounded">
                                <table border="0" cellpadding="6" cellspacing="4" width="100%">
                                    <tr>
                                        <th style="background-color:#333;">Pickup Name</th>
                                        <td style="width:100%;padding-left:12px;background-color:#333;font-weight:bold;">
                                            <ul>
                                                <li>To change the pickup name - the tickets must be marked for Will Call.</li>
                                                <li style="font-weight:bold;color:red;font-size:12px;text-transform:uppercase;">
                                                    Changing the pickup name will not carry to the other tickets in the package! Pickup names must be specified separately for each ticket.
                                                </li>
                                            </ul>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th style="color:Red;font-weight:bold;text-transform:uppercase;background-color:#333;">Last Name</th>
                                        <td style="width:100%";>
                                            <asp:TextBox MaxLength="256" ID="txtPickupLast" Width="300px" runat="server" />
                                            <asp:Button ID="btnWillCall" 
                                                OnClientClick="javascript: return confirm('Are you sure you want to change the method to will call? This will also cause the ship date to be cleared. This will not save the pickup name if it has been edited - to change the pickup name, you must use the save button.');" 
                                                runat="server" CommandName="willcall" CommandArgument='<%#Eval("Id") %>' 
                                                ValidationGroup="order" Text="Change Pickup To Will Call" CssClass="btntny" />
                                            <asp:CustomValidator ID="customWillCall" runat="server" ErrorMessage="" CssClass="validator" 
                                                Display="Static" ValidationGroup="order" >*</asp:CustomValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th style="color:Red;font-weight:bold;text-transform:uppercase;background-color:#333;">First Name</th>
                                        <td>
                                            <asp:TextBox MaxLength="256" ID="txtPickupFirst" Width="300px" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <asp:HiddenField ID="hiddenPickupName" runat="server" Value='<%#Bind("PickupName") %>' />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="99" style="padding:12px;">
                            <asp:Button CssClass="btntny" ID="btnSave" runat="server" CausesValidation="true" ValidationGroup="order" 
                                CommandName="Update" CommandArgument='<%#Eval("Id") %>' Text="save" />
                            <asp:Button CssClass="btntny" ID="btnCancel" runat="server" CausesValidation="false" 
                                CommandName="Cancel" Text="cancel" />
                            &nbsp;You may change shipping options on the shipments page.
                        </td>
                    </tr>
                </EditItemTemplate>
                <ItemSeparatorTemplate><tr><td colspan="99" class="seppy">&nbsp;</td></tr></ItemSeparatorTemplate>
            </asp:ListView>
            <br />
            <%if (this.Page.User.IsInRole("_Master"))
              {%>
            <asp:Button ID="btnPrintTickets" CssClass="btntny" runat="server" CausesValidation="false" onclick="btnPrintTickets_Click"
                Text="print tickets in this order" />
            <br />
            <%} %>
            <div class="jqedt rounded">
                <asp:GridView Width="100%" ID="GridShipments" runat="server" DataKeyNames="Id" AutoGenerateColumns="False" 
                    OnDataBinding="GridShipments_DataBinding" OnDataBound="GridShipments_DataBound"  
                    OnRowDataBound="GridShipments_RowDataBound" CssClass="lsttbl" >
                    <AlternatingRowStyle BackColor="#f1f1f1" />
                    <EmptyDataTemplate>
                        <div class="lstempty">No Shipments</div>
                    </EmptyDataTemplate>
                    <Columns>
                        <asp:TemplateField HeaderText="Actual Shipments" HeaderStyle-HorizontalAlign="left" ItemStyle-Wrap="false" >
                            <ItemTemplate>
                                <asp:Literal ID="litShipped" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:CheckBoxField DataField="IsLabelPrinted" HeaderText="Printed" ItemStyle-HorizontalAlign="center" />
                        <asp:BoundField DataField="Context" HeaderText="Context" ItemStyle-HorizontalAlign="center" />
                        <asp:BoundField DataField="DateCreated" DataFormatString="{0:MM/dd/yy}" HtmlEncode="false" HeaderText="Created" ItemStyle-HorizontalAlign="center" />
                        <asp:TemplateField HeaderText="Ship Address" HeaderStyle-HorizontalAlign="left" >
                            <ItemTemplate>
                                <asp:Literal ID="litAddress" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="PackingList" HeaderStyle-HorizontalAlign="left" >
                            <ItemTemplate>
                                <asp:Literal ID="litPacking" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <br />
            <div class="jqedt rounded">
                <asp:GridView ID="GridAuths" Width="100%" runat="server" DataSourceID="SqlAuth" AutoGenerateColumns="False" DataKeyNames="Id" 
                    OnRowCommand="GridAuths_RowCommand" OnRowDataBound="GridAuths_RowDataBound" onrowcreated="GridAuths_RowCreated" CssClass="lsttbl">
                    <AlternatingRowStyle BackColor="#f1f1f1" />
                    <RowStyle HorizontalAlign="Center" />
                    <HeaderStyle HorizontalAlign="Center" />
                    <Columns>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" 
                            HeaderText="Payments and Refunds" ItemStyle-Wrap="false">
                            <ItemTemplate>
                                <asp:Button ID="btnResend" CssClass="btntny" runat="server" CausesValidation="false" CommandName="Resend" Text="send cnfrm"
                                    onclientclick="return confirm('This will send a copy of the confirmation email to the specifed address. Are you sure you want to continue?');" />
                                <asp:TextBox ID="txtResendToAddress" runat="server" MaxLength="256" Font-Size="9px" Width="175px" />
                                <asp:Literal ID="litCreditTitle" runat="server" EnableViewState="false" Visible="false" Text="<span style='font-weight: bold; font-size: 13px;'>Store Credit</span>" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Id" HeaderText="Id" />                        
                        <asp:BoundField DataField="LastFour" HeaderText="Lst4" />
                        <asp:CheckBoxField DataField="bAuthorized" HeaderText="Auth" />
                        <asp:BoundField DataField="ProcessorId" HeaderText="ProcId" />
                        <asp:BoundField DataField="Method" HeaderText="Meth" />
                        <asp:BoundField DataField="TransactionType" HeaderText="Type" />
                        <asp:BoundField DataField="mTaxAmount" DataFormatString="{0:n2}" HtmlEncode="False" HeaderText="Tax" />
                        <asp:BoundField DataField="mFreightAmount" DataFormatString="{0:n2}" HtmlEncode="False" HeaderText="S&amp;H" />
                        <asp:BoundField DataField="mAmount" DataFormatString="{0:c}" HtmlEncode="False" HeaderText="Amount" />
                        <asp:BoundField DataField="Email" HeaderText="Email" headerStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="NameOnCard" HeaderText="NameOnCard" headerStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="IpAddress" HeaderText="IpAddress" />
                    </Columns>
                </asp:GridView>
            </div>
            <br />  
            <div class="jqedt rounded">
                <asp:GridView ID="GridEvents" GridLines="Vertical" runat="server" DataSourceID="SqlEvent" Width="100%" AutoGenerateColumns="False" CssClass="lsttbl">
                    <EmptyDataTemplate>
                        <div class="lstempty">No Events...</div>
                    </EmptyDataTemplate>
                    <AlternatingRowStyle BackColor="#f1f1f1" />
                    <Columns>
                        <asp:BoundField DataField="DateToProcess" DataFormatString="{0:MM/dd/yyyy hh:mmtt}" HeaderText="To Process" HeaderStyle-HorizontalAlign="Left" >
                            <ItemStyle Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField DataField="DateProcessed" DataFormatString="{0:MM/dd/yyyy hh:mmtt}" HeaderText="Processed" >
                            <ItemStyle Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Status" HeaderText="Status" />
                        <asp:BoundField DataField="CreatorName" HeaderText="Creator" />
                        <asp:BoundField DataField="Verb" HeaderText="Verb" />
                        <asp:BoundField DataField="OldValue" HeaderText="OldVal" >
                            <ItemStyle Wrap="True" />
                        </asp:BoundField>
                        <asp:BoundField DataField="NewValue" HeaderText="NewVal" >
                            <ItemStyle Wrap="True" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Description" HeaderText="Description" >
                            <ItemStyle Wrap="True" />
                        </asp:BoundField>
                    </Columns>
                    <AlternatingRowStyle BackColor="#FEE6C6" />
                </asp:GridView>
            </div>
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
<div class="{transparent}"></div>
<asp:SqlDataSource ID="SqlInvoice" runat="server" CacheDuration="300" ConnectionString="<%$ ConnectionStrings:WillCallConnectionString %>"
    EnableCaching="False" OnSelecting="Querystring_Selecting"
    SelectCommand="SELECT i.[Id], i.[UniqueId], i.[UserId], i.[CustomerId], u.[Username] as 'Email', i.[dtInvoiceDate], i.[mTotalPaid], i.[mBalanceDue], 
        i.[OrderType], i.[Creator], 
        CASE WHEN i.[TCashewId] IS NOT NULL THEN i.[TCashewId] ELSE 0 END as 'TCashewId', 
        i.[InvoiceStatus], i.[mNetPaid], i.[mTotalRefunds], 
        CASE WHEN ibs.[mActualShipping] IS NOT NULL THEN ibs.[mActualShipping] ELSE 0 END as 'mActualShipping', 
        CASE WHEN ibs.[mHandlingComputed] IS NOT NULL THEN ibs.[mHandlingComputed] ELSE 0 END as 'mHandlingComputed',
        ISNULL(it.[LastFourDigits],'0000') as 'LastFour'
        FROM [aspnet_users] u, [Invoice] i LEFT OUTER JOIN [InvoiceBillShip] ibs ON i.[Id] = ibs.[tInvoiceId] 
            LEFT OUTER JOIN [InvoiceTransaction] it ON it.[tInvoiceId] = i.[Id] AND it.[TransType] = 'Payment' AND 
                it.[FundsType] = 'CreditCard'
        WHERE i.[Id] = @Id AND i.[ApplicationId] = @appId and i.[UserId] = u.[UserId] ">
    <SelectParameters>
        <asp:Parameter Name="appId" DbType="Guid" />
        <asp:QueryStringParameter DefaultValue="0" Name="Id" QueryStringField="Inv" Type="Int32" />
    </SelectParameters>    
</asp:SqlDataSource>
<asp:SqlDataSource ID="SqlInvoiceBillShip" runat="server" CacheDuration="300" ConnectionString="<%$ ConnectionStrings:WillCallConnectionString %>"
    EnableCaching="True" OnSelecting="Querystring_Selecting"
    SelectCommand="SELECT i.[Id], i.[tInvoiceId], i.[UserId], u.[UserName], i.[CustomerId], i.[blCompany], i.[blFirstName], i.[blLastName], i.[blAddress1], i.[blAddress2], 
        i.[blCity], i.[blStateProvince], i.[blPostalCode], i.[blCountry], i.[blPhone], i.[bSameAsBilling], i.[CompanyName], i.[FirstName], i.[LastName], 
        i.[Address1], i.[Address2], i.[City], i.[StateProvince], i.[PostalCode], i.[Country], i.[Phone], i.[ShipMessage] 
        FROM [InvoiceBillShip] i, [Aspnet_Users] u 
        WHERE i.[tInvoiceId] = @tInvoiceId AND u.[ApplicationId] = @appId AND u.[UserId] = i.[UserId]"
    UpdateCommand="UPDATE [InvoiceBillShip] SET [bSameAsBilling] = @bSameAsBilling, [CompanyName] = @CompanyName, 
        [FirstName] = @FirstName, [LastName] = @LastName, [Address1] = @Address1, [Address2] = @Address2, [City] = @City, 
        [StateProvince] = @StateProvince, [PostalCode] = @PostalCode, [Country] = @Country, [Phone] = @Phone, 
        [ShipMessage] = @ShipMessage 
        WHERE [Id] = @Id">
    <UpdateParameters>
        <asp:Parameter Name="bSameAsBilling" Type="Boolean" />
        <asp:Parameter Name="CompanyName" Type="String" />
        <asp:Parameter Name="FirstName" Type="String" />
        <asp:Parameter Name="LastName" Type="String" />
        <asp:Parameter Name="Address1" Type="String" />
        <asp:Parameter Name="Address2" Type="String" />
        <asp:Parameter Name="City" Type="String" />
        <asp:Parameter Name="StateProvince" Type="String" />
        <asp:Parameter Name="PostalCode" Type="String" />
        <asp:Parameter Name="Country" Type="String" />
        <asp:Parameter Name="Phone" Type="String" />
        <asp:Parameter Name="ShipMessage" Type="String" />
        <asp:Parameter Name="Id" Type="Int32" />
    </UpdateParameters>
    <SelectParameters>
        <asp:Parameter Name="appId" DbType="Guid" />
        <asp:QueryStringParameter DefaultValue="0" Name="tInvoiceId" QueryStringField="Inv" Type="Int32" />
    </SelectParameters>
</asp:SqlDataSource>
<asp:SqlDataSource ID="SqlItems" runat="server" CacheDuration="300" ConnectionString="<%$ ConnectionStrings:WillCallConnectionString %>"
    EnableCaching="False" OnSelecting="Querystring_Selecting"
    UpdateCommand="SELECT 0"
    >
    <UpdateParameters>
        <asp:Parameter Name="Notes" Type="String" />
        <asp:Parameter Name="PickupName" Type="String" />
        <asp:Parameter Name="Id" Type="Int32" />
    </UpdateParameters>
</asp:SqlDataSource>
<asp:SqlDataSource ID="SqlAuth" runat="server" CacheDuration="300" ConnectionString="<%$ ConnectionStrings:WillCallConnectionString %>"
    EnableCaching="False" OnSelecting="Querystring_Selecting"
    SelectCommand="SELECT a.[Id], a.[InvoiceNumber], a.[bAuthorized], a.[TInvoiceId], a.[UserId], a.[CustomerId], a.[ProcessorId], a.[Method], a.[TransactionType], 
        a.[mAmount], a.[mTaxAmount], a.[mFreightAmount], a.[Description], a.[Email], a.[NameOnCard], a.[IpAddress], ISNULL(it.[LastFourDigits],'0000') as 'LastFour' 
        FROM [AuthorizeNet] a 
            LEFT OUTER JOIN [InvoiceTransaction] it ON it.[TInvoiceId] = a.[TInvoiceId] AND it.[ProcessorId] = a.[ProcessorId] 
                AND it.[TransType] = 'Payment' AND it.[FundsType] = 'CreditCard', [Invoice] i 
        WHERE i.[Id] = @TInvoiceId AND i.[ApplicationId] = @appId AND i.[Id] = a.[TInvoiceId] AND [bAuthorized] = @bAuthorized 
        UNION 
        SELECT it.[Id], inv.[UniqueId] as 'InvoiceNumber', '1' as 'bAuthorized', @TInvoiceId as 'TInvoiceId', it.[UserId], it.[CustomerId], it.[ProcessorId],
            it.[FundsType] as 'Method', it.[TransType] as 'TransactionType', it.[mAmount], 0, 0, 'store credit' as 'Description', '' as 'Email',
            '' as 'NameOnCard', it.[UserIp] as 'IpAddress', ISNULL(it.[LastFourDigits],'0000') as 'LastFour' 
        FROM [InvoiceTransaction] it, [Invoice] inv 
        WHERE inv.[Id] = @TInvoiceId AND it.[TInvoiceId] = inv.[Id] AND it.[FundsType] = 'StoreCredit' 
        "     
    >
    <SelectParameters>
        <asp:Parameter Name="appId" DbType="Guid" />
        <asp:QueryStringParameter DefaultValue="0" Name="TInvoiceId" QueryStringField="Inv" Type="Int32" />
        <asp:Parameter DefaultValue="true" Name="bAuthorized" Type="Boolean" />
    </SelectParameters>
</asp:SqlDataSource>
<asp:SqlDataSource ID="SqlEvent" runat="server" ConnectionString="<%$ ConnectionStrings:WillCallConnectionString %>" OnSelecting="Querystring_Selecting"
    SelectCommand="SELECT eq.[Id], eq.[DateToProcess], eq.[DateProcessed], eq.[Status], eq.[CreatorId], eq.[CreatorName], eq.[UserName], eq.[Context], eq.[Verb], 
        eq.[OldValue], eq.[NewValue], eq.[Description], eq.[UserId], eq.[Ip], eq.[dtStamp] 
        FROM [EventQArchive] eq, [InvoiceEvent] evt 
        WHERE evt.[TInvoiceId] = @TInvoiceId AND evt.[TEventQId] = eq.[Id] AND eq.[ApplicationId] = @appId  
        UNION SELECT eq.[Id], eq.[DateToProcess], eq.[DateProcessed], eq.[Status], eq.[CreatorId], eq.[CreatorName], eq.[UserName], eq.[Context], eq.[Verb], 
        eq.[OldValue], eq.[NewValue], eq.[Description], eq.[UserId], eq.[Ip], eq.[dtStamp] FROM [EventQ] eq, [InvoiceEvent] evt 
        WHERE evt.[TInvoiceId] = @TInvoiceId AND evt.[TEventQId] = eq.[Id] AND eq.[ApplicationId] = @appId ORDER BY [DateProcessed] DESC">
    <SelectParameters>
        <asp:Parameter Name="appId" DbType="Guid" />
        <asp:QueryStringParameter DefaultValue="0" Name="TInvoiceId" QueryStringField="Inv" Type="Int32" />
    </SelectParameters>
</asp:SqlDataSource>