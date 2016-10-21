<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CampaignUser.ascx.cs" Inherits="WillCallWeb.Admin.AdminControls.ProductAccessor.CampaignUser" %>
<div id="srceditor">
    <div id="campaignlist">
        <div class="jqhead rounded">

            <h3 class="entry-title">Campaign User Access 
                <span style="margin:0 64px;"><asp:Button ID="btnCampaignList" runat="server" Text="Edit Campaigns" OnClick="btnCampaignList_Click" CssClass="btnmed" Width="100px" /></span>
                <span style="margin:0 64px;"><asp:Button ID="btnCampaignMailer" runat="server" Text="Edit Mailers" OnClick="btnCampaignMailer_Click" CssClass="btnmed" Width="100px" /></span>
            </h3>

            <asp:GridView Width="100%" ID="GridListing" runat="server" GridLines="None" 
                AutoGenerateColumns="False" CssClass="lsttbl" BorderStyle="none"
                DataKeyNames="Id"
                OnInit="GridListing_Init"
                OnDataBinding="GridListing_DataBinding"
                OnRowDataBound="GridListing_RowDataBound"
                OnDataBound="GridListing_DataBound"
                
                OnSelectedIndexChanged="GridListing_SelectedIndexChanged"
                OnRowCommand="GridListing_RowCommand"

                >                            
                <SelectedRowStyle cssclass="selected" />
                <Columns>
                    <asp:ButtonField ButtonType="Button" DataTextField="Id" ControlStyle-CssClass="btntny" ItemStyle-HorizontalAlign="Center" CommandName="Select" />
                    <asp:CheckBoxField DataField="bActive" ReadOnly="true" HeaderText="Active" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="CampaignName" ReadOnly="true" HeaderText="Name" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="50%" />
                    <asp:BoundField DataField="CampaignCode" ReadOnly="true" HeaderText="Code" HeaderStyle-HorizontalAlign="Left" />
                    <asp:TemplateField HeaderText="PublicStart" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Literal ID="litPublicStart" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="PublicEnd" ItemStyle-HorizontalAlign="Center" >
                        <ItemTemplate>
                            <asp:Literal ID="litPublicEnd" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
 <ul>
                <li>handle duplicates?</li>
                <li>dupe user names not allowed - warn of dupe user id</li>
            </ul>
        <div class="jqhead rounded">
           
            <div class="cmdsection">
                
                <asp:ValidationSummary HeaderText="The following errors ocurred:" ID="ValidationSummary3" runat="server" 
                    ValidationGroup="gridcmd" CssClass="validationsummary" Width="95%" />
                <div style="border:solid red 0px;float:left;">
                    
                    
                    <div style="white-space:nowrap;">
                        <a href="javascript: alert('May refer to an employee who granted access. Friend of someone, etc')" class="infomark">?</a>
                        <span class="sectitle">
                            Referral</span>
                        <asp:TextBox ID="txtReferral" runat="server" Width="375px" MaxLength="512" />                    
                        <asp:Button ID="btnUpdateReferral" runat="server" Text="Referral Selected" CssClass="btnmed" Width="100px" OnClick="GridCmd_Click"
                            OnClientClick="return confirm('This will apply the REFERRAL information to the SELECTED rows in the list below. Continue?');" />
                        <asp:CustomValidator ID="CustomReferral" runat="server" ValidationGroup="gridcmd" CssClass="validator"
                            display="Static" >*</asp:CustomValidator>
                    </div>
                    <div style="white-space:nowrap;">
                        <a href="javascript: alert('May describe any other notes for the Access. Backstage passes, VIP passes, etc')" class="infomark">?</a>
                        <span class="sectitle">
                            Instructions</span>
                        <asp:TextBox ID="txtInstructions" runat="server" Width="375px" MaxLength="512" />                    
                        <asp:Button ID="btnUpdateInstructions" runat="server" Text="Instruct Selected" CssClass="btnmed" Width="100px" OnClick="GridCmd_Click" 
                            OnClientClick="return confirm('This will apply the INSTRUCTION information to the SELECTED rows in the list below. Continue?');" />
                        <asp:CustomValidator ID="CustomInstructions" runat="server" ValidationGroup="gridcmd" CssClass="validator"
                            display="Static" >*</asp:CustomValidator>
                    </div>
                    <div style="white-space:nowrap;">
                        <a href="javascript: alert('Quantity of items allowed for purchase.')" class="infomark">?</a>
                        <span class="sectitle">Qty</span>
                        <span style="display:inline-block;width:378px;">
                            <asp:TextBox ID="txtQty" runat="server" Width="50px" MaxLength="4" />                    
                        </span>
                        <asp:Button ID="btnUpdateQty" runat="server" Text="Qty To Selected" CssClass="btnmed" Width="100px" OnClick="GridCmd_Click" 
                            OnClientClick="return confirm('This will apply the QUANTITY information to the SELECTED rows in the list below. Continue?');" />
                        <asp:CustomValidator ID="CustomQty" runat="server" ValidationGroup="gridcmd" CssClass="validator"
                            display="Static">*</asp:CustomValidator>
                    </div>
                    
                </div>
                
                <div style="border:solid red 0px;float:left;white-space:nowrap;">
                    <span style="vertical-align:top;display:inline-block;">
                        <asp:Button ID="btnAddUsers" runat="server" Text="Add Users --&raquo;" CssClass="btnmed" Width="80px" OnClick="GridCmd_Click" 
                            OnClientClick="return confirm('This will add the email addresses in the list (to the right) as well as applying the REFERRAL, INSTRUCTION and QUANTITY information. Continue?');" />
                        <br />
                        <asp:CustomValidator ID="CustomAddUsers" runat="server" ValidationGroup="gridcmd" CssClass="validator"
                            display="Static" >*</asp:CustomValidator>
                    </span>
                    <asp:TextBox ID="txtAddUsers" runat="server" TextMode="MultiLine" Width="439px" Height="80px" />
                </div>

                
            </div>
            <div class="clear" />
            <br />
            <h3>User List for: <%=Atx.CurrentAccessCampaign.CampaignName %></h3>
            <asp:GridView Width="100%" ID="GridUsers" runat="server" GridLines="None" 
                AutoGenerateColumns="False" CssClass="lsttbl" BorderStyle="none"
                DataSourceId="SqlProductAccessUser"
                DataKeyNames="Id" EnableViewState="true" 
                OnRowDataBound="GridUsers_RowDataBound"
                >
                <SelectedRowStyle cssclass="selected" />                
                <EmptyDataTemplate>
                    <h4>No users for this campaign</h4>
                </EmptyDataTemplate>
                
                <Columns>
                    <asp:TemplateField ItemStyle-Width="40px" ItemStyle-HorizontalAlign="Center">
                        <HeaderTemplate>
                            <asp:CheckBox runat="server" ID="chkMaster" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox runat="server" ID="chkSelect" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-Width="40px" HeaderStyle-HorizontalAlign="Center">
                        <HeaderTemplate>
                            <asp:Button ID="btnDeleteSelected" runat="server" Text="Delete" ToolTip="This will delete any selected rows" CssClass="btnmed" Width="40px" OnClick="GridCmd_Click" />
                        </HeaderTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Id" ReadOnly="true" HeaderText="Id" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="60px" />
                    <asp:TemplateField ItemStyle-Width="60px" HeaderText="Registered" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:CheckBox runat="server" ID="chkRegistered" Enabled="false" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="iQuantityAllowed" ReadOnly="true" HeaderText="Qty" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="60px" />
                    <asp:BoundField DataField="UserName" ReadOnly="true" HeaderText="Name" HeaderStyle-HorizontalAlign="Left" ItemStyle-Wrap="false" />
                    <asp:BoundField DataField="Referral" ReadOnly="true" HeaderText="Referral" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="150px" />
                    <asp:BoundField DataField="Instructions" ReadOnly="true" HeaderText="Instructions" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="150px" />
                </Columns>    
            </asp:GridView>
        </div>
    </div>
</div>
<asp:SqlDataSource ID="SqlProductAccessUser" runat="server" ConnectionString="<%$ ConnectionStrings:WillCallConnectionString %>" 
    SelectCommand="SELECT e.[Id], e.[dtStamp], e.[TProductAccessId], e.[UserName], ISNULL(au.[UserId],null) as [UserId], e.[iQuantityAllowed], e.[Referral], e.[Instructions] 
        FROM [ProductAccessUser] e LEFT OUTER JOIN [aspnet_Users] au ON e.[UserId] = au.[UserId] WHERE e.[TProductAccessId] = @productAccessId
        ORDER BY e.[UserName] "
        >
    <SelectParameters>
        <asp:ControlParameter ControlID="GridListing" Name="productAccessId" PropertyName="SelectedValue" DbType="Int32" />
    </SelectParameters>
</asp:SqlDataSource>

<script type="text/javascript">

    var checkMaster = '#<%=GridUsers.ClientID%> input[id*="chkMaster"]:checkbox';
    var checkBoxSelector = '#<%=GridUsers.ClientID%> input[id*="chkSelect"]:checkbox';
    var deleteCheckedSelector = '#<%=GridUsers.ClientID%> input[id*="btnDeleteSelected"]';

    var btnReferral = '#<%=GridUsers.Parent.ClientID%>_btnUpdateReferral';
    var btnInstructions = '#<%=GridUsers.Parent.ClientID%>_btnUpdateInstructions';
    var btnQty = '#<%=GridUsers.Parent.ClientID%>_btnUpdateQty';

    $(document).ready(function () {

        $(checkMaster).bind('click', function () {
            $(checkBoxSelector).attr('checked', $(this).is(':checked'));

            ToggleCheckUncheckAllOptionAsNeeded();
        });

        $(checkBoxSelector).bind('click', ToggleCheckUncheckAllOptionAsNeeded);

        ToggleCheckUncheckAllOptionAsNeeded();
    });

    function ToggleCheckUncheckAllOptionAsNeeded() {
      var totalCheckboxes = $(checkBoxSelector),
      checkedCheckboxes = totalCheckboxes.filter(":checked"),
      noCheckboxesAreChecked = (checkedCheckboxes.length === 0),
      allCheckboxesAreChecked = (totalCheckboxes.length === checkedCheckboxes.length);

      $(checkMaster).attr('checked', allCheckboxesAreChecked);
      $(deleteCheckedSelector).attr('disabled', noCheckboxesAreChecked);
      $(btnReferral).attr('disabled', noCheckboxesAreChecked);
      $(btnInstructions).attr('disabled', noCheckboxesAreChecked);
      $(btnQty).attr('disabled', noCheckboxesAreChecked);
    } 
</script> 