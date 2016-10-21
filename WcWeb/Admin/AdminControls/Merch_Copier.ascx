<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Merch_Copier.ascx.cs" 
    Inherits="WillCallWeb.Admin.AdminControls.Merch_Copier" %>
<div id="srceditor">
    <div id="merchcopy">
        <div class="jqhead rounded">
            <h3 class="entry-title">Create A Copy Of: <%=Atx.CurrentMerchRecord.DisplayNameWithAttribs%></h3>
            <div class="jqinstruction rounded">
                <ul>
                    <li>All categories will be copied.</li>
                    <li>Inventory amounts will not be copied to new item.</li>
                    <li>Active, InternalOnly, Taxable, UseSalePrice and Featured will automatically be set to false.</li>
                    <li>Unlock codes and start and end dates will be set to off.</li>
                    <li>Images will not be copied.</li>
                  </ul>
            </div>
            <br />
            <asp:ValidationSummary ID="ValidationSummary2" CssClass="validationsummary" HeaderText="" ValidationGroup="entity" runat="server" />
            <table border="0" cellspacing="3" cellpadding="0" width="100%" class="edittabl">
                <tr>
                    <th>
                        <asp:RequiredFieldValidator Display="dynamic" CssClass="validation" ValidationGroup="entity" 
                            ID="RequiredName" runat="server" ErrorMessage="Please enter a name." ControlToValidate="txtName">*</asp:RequiredFieldValidator>
                    </th>
                    <th>New Name</th>
                    <td colspan="2" style="width:100%;"><asp:TextBox ID="txtName" runat="server" MaxLength="256" Width="350px" /></td>
                </tr>
                <tr>
                    <th colspan="2">Copy Images</th>
                    <td><asp:CheckBox ID="chkImages" runat="server" Checked="false" /></td>
                    <td style="width: 100%;" class="intr">Check this box to copy all images</td>
                </tr>
                <tr>
                    <th colspan="2">Copy Child Items</th>
                    <td><asp:CheckBox ID="chkCopyChildren" runat="server" Checked="false" /></td>
                    <td class="intr">Check this box to copy all style, color and size options</td>
                </tr>
                <tr>
                    <th colspan="2">Copy Bundled Items</th>
                    <td><asp:CheckBox ID="chkBundled" runat="server" Checked="false" /></td>
                    <td class="intr">Check this box to copy all merch bundles and items attached to the parent item</td>
                </tr>
            </table>
            <div class="cmdsection">
                <asp:Button ID="btnCreateCopy" CssClass="btnmed" ValidationGroup="entity" CausesValidation="true" 
                    runat="server" CommandName="Copy" Text="Create Copy" 
                    OnClick="btnCreateCopy_Click" />
                <asp:Button ID="btnCancel" CssClass="btnmed" runat="server" CommandName="Cancel" 
                    Text="Cancel" OnClick="btnCancel_Click" />
                <asp:CustomValidator ID="CustomValidation" runat="server" CssClass="invisible" 
                    Display="Static" ErrorMessage="bad mojo" ValidationGroup="entity">*</asp:CustomValidator>
            </div>
        </div>
    </div>
</div>