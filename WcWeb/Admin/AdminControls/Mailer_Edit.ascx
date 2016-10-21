<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Mailer_Edit.ascx.cs" Inherits="WillCallWeb.Admin.AdminControls.Mailer_Edit" %>
<div id="srceditor">
    <div id="mailereditor">
        <asp:FormView ID="FormView1" runat="server" DefaultMode="Edit" Width="100%" DataKeyNames="Id,TEmailLetterId"
            OnDataBinding="FormView1_DataBinding" 
            OnModeChanging="FormView1_ModeChanging" 
            OnDataBound="FormView1_DataBound" 
            OnItemDeleting="FormView1_ItemDeleting" 
            OnItemInserting="FormView1_ItemInserting" 
            OnItemUpdating="FormView1_ItemUpdating" 
            OnItemCommand="FormView1_ItemCommand" 
            onitemcreated="FormView1_ItemCreated" >
            <EmptyDataTemplate>
                <div class="jqhead rounded">
                    <h3 class="entry-title">
                        Add a New Mailer....
                        <asp:Button Id="btnNew" CausesValidation="false" runat="server" CommandName="New" Text="New" CssClass="btnmed" />
                    </h3>
                </div>
            </EmptyDataTemplate>
            <EditItemTemplate>
                <div class="jqhead rounded">
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" class="edittabl">
                        <tr>
                            <th>EDIT MAILER</th>
                            <td style="width:100%;">
                                <asp:DropDownList ID="ddlMailers" runat="server" OnDataBinding="ddlMailers_DataBinding" 
                                    OnDataBound="ddlMailers_DataBound" Width="100%"
                                    AutoPostBack="true" OnSelectedIndexChanged="ddlMailers_SelectedIndexChanged" />
                            </td>
                        </tr>
                    </table>
                    <table border="0" cellspacing="0" cellpadding="0" width="100%" class="edittabl">
                        <tr>
                            <th><asp:RequiredFieldValidator ValidationGroup="mailer" CssClass="validator" ID="RequiredName" 
                                    runat="server" Display="Static" ControlToValidate="txtName" ErrorMessage="Name is required.">*</asp:RequiredFieldValidator>
                                Name
                            </th>
                            <td><asp:TextBox ID="txtName" runat="server" Width="280px" MaxLength="256" /></td>
                            <td rowspan="3">
                                 <asp:FileUpload ID="uplMailerImage" runat="server" Width="350px" CssClass="btnmed" />
                                 <div class="jqinstruction rounded">
                                    <ul>
                                        <li style="color:#990000;">Uploads will overwite existing images with the same file name!</li>
                                        <li>To add an image, upload the file and add img tag</li>
                                        <li>&lt;img src=&#39;http://domain/<%=Wcss.SubscriptionEmail.Path_PostedImages %>/{imageName.ext}&#39; /&gt;</li>
                                    </ul>
                                    <asp:Button ID="btnUploadImage" runat="server" CssClass="btnmed btnupload" Width="80px" Text="Upload Image" CommandName="uploadimage" />
                                </div>
                               <asp:DropDownList ID="ddlMailerImage" Width="350px" runat="server" OnDataBinding="ddlFile_DataBinding" >
                                    <asp:ListItem Text="--- Image Directory Files ---" Value="" />
                                </asp:DropDownList><br />
                            </td>
                        </tr>
                        <tr>
                            <th>Subscription</th>
                            <td>
                                <asp:DropDownList ID="ddlSubscription" Width="280px" runat="server" OnDataBinding="ddlSubscription_DataBinding" OnDataBound="ddlSubscription_DataBound" />
                            </td>
                        </tr>
                        <tr>
                            <th><asp:RequiredFieldValidator ValidationGroup="mailer" CssClass="validator" ID="RequiredSubject" 
                                    runat="server" Display="Static" ControlToValidate="txtSubject" ErrorMessage="Subject is required.">*</asp:RequiredFieldValidator>
                                Subject
                            </th>
                            <td><asp:TextBox ID="txtSubject" runat="server" Width="280px" MaxLength="256" Text='<%#Eval("EmailLetterRecord.Subject") %>' /></td>
                        </tr>
                    </table>
                    <div class="cmdsection">
                        <asp:Button ID="btnUpdate" ValidationGroup="mailer" CausesValidation="true" CssClass="btnmed" 
                            runat="server" CommandName="Update" Text="Save Changes" />
                        <asp:Button ID="btnCancel" CssClass="btnmed" runat="server" CommandName="Cancel" Text="Cancel" />
                        <asp:Button ID="btnDelete" CssClass="btnmed" runat="server" CommandName="Delete" Text="Delete" />
                        <asp:Button ID="btnNew" CssClass="btnmed" runat="server" CommandName="New" Text="New" />
                        <asp:CustomValidator ID="CustomValidation" runat="server" ValidationGroup="mailer" CssClass="validator" 
                            Display="Static" ErrorMessage="CustomValidator">*</asp:CustomValidator> 
                        <asp:Button ID="btnPreview" runat="server" CssClass="btnmed" CommandName="Preview" Text="Preview"
                            OnClientClick="javascript:doPagePopup('/Admin/MailerViewer.aspx','true'); " />
                    </div>
                </div>
                <asp:ValidationSummary ID="ValidationSummary2" CssClass="validationsummary" HeaderText="" 
                    ValidationGroup="mailer" runat="server" />
                <div class="jqpnl rounded">
                    <div>
                        <div class="subtitle">Style</div>
                        <asp:TextBox ID="txtStyle" runat="server" TextMode="MultiLine" Height="60px" Width="100%" Text='<%#Eval("EmailLetterRecord.StyleContent") %>' />
                        <div class="subtitle">Html View (preview button is magnifying glass on document icon)</div>
                        <asp:TextBox ID="txtHtml" runat="server" TextMode="MultiLine" Height="600px" Width="100%" Text='<%#Eval("EmailLetterRecord.HtmlVersion") %>' />
                        <div class="subtitle">Text View <span class="sml">(all html email should also have a text version)
                            <asp:RequiredFieldValidator ValidationGroup="mailer" CssClass="validator" ID="RequiredFieldValidator1" 
                                runat="server" Display="Static" ControlToValidate="txtBody" ErrorMessage="Text version is required.">*</asp:RequiredFieldValidator>
                            <span style="color:Red;">Text links must start with http:// or https://</span></span>
                        </div>
                        <asp:TextBox ID="txtBody" runat="server" Text='<%#Eval("EmailLetterRecord.TextVersion") %>' Width="100%" Height="300px" TextMode="MultiLine" />
                    </div>  
                </div>  
            </EditItemTemplate>
            <InsertItemTemplate>
                <div class="jqhead rounded">
                    <h3 class="entry-title">Adding A New Email Letter...</h3>
                </div>
                <asp:ValidationSummary ID="ValidationSummary2" CssClass="validationsummary" HeaderText="" 
                    ValidationGroup="mailer" runat="server" />
                <div class="jqpnl rounded">
                    <table border="0" cellspacing="0" cellpadding="0" width="100%" class="edittabl">
                        <tr>
                            <th>Copy Previous</th>
                            <td colspan="2">
                                <asp:DropDownList ID="ddlMailers" runat="server" OnDataBinding="ddlMailers_DataBinding" Width="500px" >
                                    <asp:ListItem Text="<-- Select an email to copy -->" Value="0" />
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <th><asp:Button ID="btnPreview" runat="server" CssClass="btnmed" CommandName="Preview" Text="View Selection"
                                    OnClientClick="javascript:showEmailLetterSelection('ddlMailers'); " />
                            </th>
                            <td><asp:CheckBox ID="chkCopy" runat="server" Text="Copy selected email?" /></td>
                        </tr>
                        <tr><td colspan="3">&nbsp;</td></tr>
                        <tr>
                            <th><asp:RequiredFieldValidator ValidationGroup="mailer" CssClass="validator" ID="RequiredName" 
                                    runat="server" Display="Static" ControlToValidate="txtName" ErrorMessage="Name is required.">*</asp:RequiredFieldValidator>
                                Name
                            </th>
                            <td><asp:TextBox ID="txtName" runat="server" Width="280px" MaxLength="256" />
                            </td>
                            <td style="width:100%;">(A date prefix will be included in the filename automatically.)</td>
                        </tr>
                        <tr>
                            <th>Subscriptions</th>
                            <td colspan="2">
                                <asp:DropDownList ID="ddlSubscription" Width="280px" runat="server" OnDataBinding="ddlSubscription_DataBinding" OnDataBound="ddlSubscription_DataBound" />
                            </td>
                        </tr>
                        <tr>
                            <th>
                                <asp:RequiredFieldValidator ValidationGroup="mailer" CssClass="validator" ID="RequiredSubject" 
                                    runat="server" Display="Static" ControlToValidate="txtSubject" ErrorMessage="Subject is required.">*</asp:RequiredFieldValidator>
                                Email Subject
                            </th>
                            <td colspan="2"><asp:TextBox ID="txtSubject" runat="server" Width="280px" MaxLength="256" /></td>
                        </tr>
                        <tr><td colspan="3">&nbsp;</td></tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td colspan="2">You will be able to edit the email following creation.</td>
                        </tr>
                    </table>
                    <div class="cmdsection">
                        <asp:Button ID="btnInsert" CausesValidation="true" ValidationGroup="mailer" CssClass="btnmed" 
                            runat="server" CommandName="Insert" Text="insert" />
                        <asp:Button ID="btnCancel" CssClass="btnmed" runat="server" CommandName="Cancel" Text="cancel" />
                    </div>
                </div>
            </InsertItemTemplate>
        </asp:FormView>
    </div>        
    <div style="visibility:hidden;"><asp:FileUpload ID="FileUploadRegisterControl" runat="server" Width="350px" CssClass="btnmed" /></div>
</div>