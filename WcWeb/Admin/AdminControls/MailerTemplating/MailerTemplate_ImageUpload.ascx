<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MailerTemplate_ImageUpload.ascx.cs" Inherits="WillCallWeb.Admin.AdminControls.MailerTemplating.MailerTemplate_ImageUpload" %>
<%@ Register src="MailerTemplate_Menu.ascx" tagname="MailerTemplate_Menu" tagprefix="uc1" %>
<div id="srceditor">
    <uc1:MailerTemplate_Menu ID="MailerTemplate_Menu1" runat="server" Title="Image Upload" />
    <div id="mailertemplating">
        <asp:ValidationSummary ID="ValidationSummary2" CssClass="validationsummary" HeaderText="Please correct the following errors:" ValidationGroup="mailer" runat="server" />
        <div class="jqhead rounded">
            <h3 class="entry-title">
                <%if (Atx.CurrentMailer != null)
                  {%><%=Atx.CurrentMailer.Name %><%} %>
            </h3>
        </div>
        <div class="jqpnl rounded">
            <table border="0" cellpadding="0" cellspacing="0" width="100%" class="edittabl">
                <tr>
                    <td colspan="4">
                        <div class="jqinstruction rounded">
                            <ul>
                                <li style="color:#990000;">Uploads will overwite existing images with the same file name!</li>
                                <li>images will be uploaded to &lt;img src=&#39;http://<%= Wcss._Config._DomainName %>/<%=Wcss.SubscriptionEmail.Path_PostedImages %>{imageName.ext}&#39; /&gt;
                                </li>
                                <li>Images will not be resized - size the image appropriately prior to uploading
                                </li>
                            </ul>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td><asp:CustomValidator ID="CustomValidation" runat="server" ValidationGroup="mailer" CssClass="validator" 
                        Display="Static" ErrorMessage="CustomValidator">*</asp:CustomValidator></td>
                    <th>Upload</th>
                    <td><asp:FileUpload ID="uplImage" runat="server" Width="350px" CssClass="btnmed" /></td>
                    <td style="width:100%;">
                        <asp:Button ID="btnUpload" runat="server" CssClass="btnmed btnupload" Width="80px" Text="Upload Image" 
                            onclick="btnUpload_Click" /></td>
                </tr>
            </table>
            <table border="0" cellpadding="0" cellspacing="0" width="100%" class="edittabl">
                <tr>
                    <td>
                        <asp:ListBox ID="lstImages" runat="server" Width="315px" Height="600px" AutoPostBack="true" 
                            OnSelectedIndexChanged="lstImages_SelectedIndexChanged" 
                            SelectionMode="Single" OnDataBound="lstImages_DataBound" OnDataBinding="lstImages_DataBinding" />
                    </td>
                    <td style="vertical-align:top !important;"><asp:Literal ID="litImage" runat="server" OnDataBinding="litImage_DataBind" /></td>
                </tr>
            </table>
        </div>
        <div style="visibility:hidden;"><asp:FileUpload ID="FileUploadRegisterControl" runat="server" Width="350px" CssClass="btnmed" /></div>
    </div>
</div>
<asp:SqlDataSource ID="SqlBlankSelection" runat="server" EnableCaching="false" ConnectionString="<%$ ConnectionStrings:WillCallConnectionString %>"     
    SelectCommand="SELECT 0 as 'Id' " UpdateCommand="Select 0 " >
</asp:SqlDataSource>

