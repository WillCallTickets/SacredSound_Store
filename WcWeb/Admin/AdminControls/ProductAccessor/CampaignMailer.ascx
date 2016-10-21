<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CampaignMailer.ascx.cs" Inherits="WillCallWeb.Admin.AdminControls.ProductAccessor.CampaignMailer" %>
<%@ Register src="../Creators/CustomerMailer.ascx" tagname="CustomerMailer" tagprefix="uc1" %>
<div id="srceditor">
    <div id="campaignlist">
        <div class="jqhead rounded">

            <h3 class="entry-title">Campaign Mailer 
                <span style="margin:0 64px;"><asp:Button ID="btnCampaignList" runat="server" Text="Edit Campaigns" OnClick="btnCampaignList_Click" CssClass="btnmed" Width="100px" /></span>
                <span style="margin:0 64px;"><asp:Button ID="btnCampaignUser" runat="server" Text="Edit Users" OnClick="btnCampaignUser_Click" CssClass="btnmed" Width="100px" /></span>
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
        <div class="jqhead rounded">            
            <h3>User List for: <%=Atx.CurrentAccessCampaign.CampaignName %></h3>
            <asp:GridView Width="100%" ID="GridUsers" runat="server" GridLines="None" 
                AutoGenerateColumns="False" CssClass="lsttbl" BorderStyle="none"
                DataSourceId="SqlProductAccessUser"
                DataKeyNames="Id,UserName,iQuantityAllowed" EnableViewState="true" 
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
<div class="jqhead rounded">
    <div class="cmdsection">
        <asp:ValidationSummary HeaderText="The following errors ocurred:" ID="ValidationSummary3" runat="server" 
            ValidationGroup="gridcmd" CssClass="validationsummary" Width="95%" />
        <asp:Label ForeColor="Green" Font-Bold="true" ID="lblStatus" runat="server" EnableViewState="false" />
    </div>
    <h3><asp:Button ID="btnSendToSelected" runat="server" Text="Send Mail To Selected Users" OnClick="btnSend_Click" CssClass="btnmed" Width="150px"
            OnClientClick="return confirm('Have you tested and saved this email?');" ValidationGroup="gridcmd" /> 
        <asp:CustomValidator ID="Custom" runat="server" Display="static" ValidationGroup="gridcmd">*</asp:CustomValidator>
        <span class="intr">Be sure to test and save this email before sending to the list</span>
    </h3>
    <div class="jqinstruction">
        <ul>
            <li>Start date and end date parameters will only be inserted into access campaigns with a start date and end date specified. Otherwise, just hard code into the mailer.</li>
        </ul>
    </div>
    <uc1:CustomerMailer ID="CustomerMailer1" runat="server" MailerTypeTitle="Access Campaign" 
        MailTemplateSubDirectory="AccessCampaignMail" StarterTemplate="CustomerAccessTemplate.html"
        ParameterNames="EmailAddress,Quota,StartDate,EndDate" />
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
    var btnSend = 'input[id*="btnSendToSelected"]';

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
      $(btnSend).attr('disabled', noCheckboxesAreChecked);
    } 
</script> 