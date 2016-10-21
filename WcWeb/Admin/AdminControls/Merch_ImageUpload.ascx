<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Merch_ImageUpload.ascx.cs" Inherits="WillCallWeb.Admin.AdminControls.Merch_ImageUpload" %>
<div id="srceditor">
    <div id="imageupload">
        <div class="jqhead rounded">
            <h3 class="entry-title">Images for &raquo; <%=Atx.CurrentMerchRecord.DisplayNameWithAttribs %></h3>
            <asp:GridView Width="100%" ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="lsttbl" 
                OnDataBinding="GridView1_DataBinding" 
                OnDataBound="GridView1_DataBound" 
                OnRowCommand="GridView1_RowCommand" 
                OnRowDataBound="GridView1_RowDataBound" 
                OnRowDeleting="GridView1_RowDeleting" 
                OnSelectedIndexChanged="GridView1_SelectedIndexChanged">
                <SelectedRowStyle cssclass="selected" />
                <Columns>
                    <asp:TemplateField HeaderText="Select Image" ItemStyle-CssClass="imageselectcell">
                        <ItemTemplate>
                            <asp:LinkButton Id="btnSelect" Height="50" runat="server" CommandName="Select" CommandArgument='<%#Eval("Id") %>'
                                 CausesValidation="false" >
                                <asp:Image ID="imgSelect" runat="server" BorderStyle="None" CssClass='<%#Eval("ThumbClass") %>' />
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="ImageName" HeaderText="Name" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="100%" />
                    <asp:BoundField DataField="Path" HeaderText="Path" />
                    <asp:CheckBoxField DataField="IsItemImage" HeaderText="Itm Img" ItemStyle-HorizontalAlign="center" />
                    <asp:CheckBoxField DataField="IsDetailImage" HeaderText="Dtl" ItemStyle-HorizontalAlign="center" />
                    <asp:CheckBoxField DataField="OverrideThumbnail" HeaderText="NoThm" ItemStyle-HorizontalAlign="center" />
                    <asp:TemplateField HeaderText="Original Dimensions/Description" ItemStyle-Wrap="false">
                        <ItemTemplate>
                            <asp:Literal ID="LiteralDimension" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="ThumbClass" HeaderText="Class" HeaderStyle-Width="5%" />
                    <asp:TemplateField ItemStyle-Wrap="false">
                        <ItemTemplate>
                            <asp:LinkButton Width="20px" Id="btnUp" ToolTip="Move Up" CssClass="btnup" runat="server" CommandName="Up" 
                                CommandArgument='<%#Eval("Id") %>' CausesValidation="false" />
                            <asp:LinkButton Width="20px" Id="btnDown" ToolTip="Move Down" CssClass="btndown" runat="server" CommandName="Down" 
                                CommandArgument='<%#Eval("Id") %>' CausesValidation="false" />
                            <asp:CustomValidator ID="CustomValidation" runat="server" ValidationGroup="entity" Display="Static" CssClass="validator">*</asp:CustomValidator>
                            <asp:LinkButton Width="20px" Id="btnDelete" CssClass="btndelete lastinrow" runat="server" CommandName="Delete" 
                               CommandArgument='<%#Eval("Id") %>' ToolTip="Delete" CausesValidation="false"
                               OnClientClick='return confirm("Are you sure you want to delete this row?")' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <asp:ValidationSummary ID="ValidationSummary1" CssClass="validationsummary" HeaderText="" ValidationGroup="entity" runat="server" />
        </div>

        <asp:FormView ID="FormView1" Width="100%" runat="server" DefaultMode="Edit" 
            OnDataBinding="FormView1_DataBinding" 
            OnDataBound="FormView1_DataBound" 
            OnItemInserting="FormView1_ItemInserting" 
            OnItemUpdating="FormView1_ItemUpdating" 
            OnItemCreated="FormView1_ItemCreated" 
            OnModeChanging="FormView1_ModeChanging" 
            >
            <EditItemTemplate>
                <div class="jqpanel1 rounded eit">
                    <h3 class="entry-title"><%#Eval("ImageName") %></h3>
                    <div class="cmdsection">
                        <asp:Button ID="btnSave" runat="server" CssClass="btnmed" CausesValidation="false" 
                            CommandName="Update" Text="Save" />
                        <asp:Button ID="btnCancel" runat="server" CssClass="btnmed" CausesValidation="false" 
                            CommandName="Cancel" Text="Cancel" />
                        <asp:CustomValidator ID="CustomValidation" runat="server" ValidationGroup="entity" 
                            Display="Static" CssClass="validator">*</asp:CustomValidator>
                        <asp:Button ID="btnNew" runat="server" CommandName="New" Text="New" CssClass="btnmed" CausesValidation="false" />
                    </div>
                    <br />
                    <table border="0" cellspacing="0" cellpadding="0" class="edittabl">
                        <tr>
                            <th>
                                <span class="intr"><%#Eval("Id") %></span>
                                <a href="javascript: alert('Image is used for thumbnails. The standard file presentation type')" class="infomark">?</a>
                                Item Image
                            </th>
                            <td class="listing-row">
                                <asp:CheckBox ID="chkImage" runat="server" Checked='<%#Eval("IsItemImage") %>' />
                                <a href="javascript: alert('Image is used for detailed view in \'hi-res\' links. Shown on the image popup pages')" class="infomark">?</a>
                                Is Detail
                                <asp:CheckBox ID="chkDetail" runat="server" Checked='<%#Eval("IsDetailImage") %>' />
                                <a href="javascript: alert('Overrides the default sizing for images and uses the original image without creating a thumbnail. Not recommended unless a hi-res image is absolutely necessary for normal display. Use for pictures that do not look correct as a thumbnail - smudged/moire.')" class="infomark">?</a>
                                No Thumbs
                                <asp:CheckBox ID="chkThumb" runat="server" Checked='<%#Eval("OverrideThumbnail") %>' />
                            </td>
                        </tr>
                        <tr>
                            <th>Description</th>
                            <td>
                                <asp:TextBox ID="txtDescription" TextMode="multiLine" Width="350px" Height="50px" MaxLength="2000" 
                                    runat="server" Text='<%#Eval("DetailDescription") %>' />
                            </td>
                        </tr>
                        <tr>
                            <th>&nbsp;</th>
                            <td class="intr">Description will only appear for detail images on the detail popup page</td>
                        </tr>
                    </table>                    
                </div>
            </EditItemTemplate>
            <EmptyDataTemplate>
                <div class="jqpanel1 rounded eit">
                    <div class="cmdsection">
                    <div class="lstempty">No Image Selected
                        <asp:Button ID="btnNew" runat="server" CausesValidation="false" 
                            CommandName="New" Text="New" CssClass="btnmed" />
                    </div></div>
                    </div>
            </EmptyDataTemplate>
            <InsertItemTemplate>
                <div class="jqpanel1 rounded iit">
                    <h3 class="entry-title">Adding A New Image...</h3>
                    <table border="0" cellspacing="0" cellpadding="0" class="edittabl">
                        <tr>
                            <th>Select Image</th>
                            <td>
                                <asp:FileUpload ID="FileUpload1" runat="server" Width="350px" CssClass="btnmed" />
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td>
                                <div class="jqinstruction rounded">
                                    <ul>
                                        <li>Images must be in rgb color</li>
                                    </ul>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <div class="cmdsection">
                        <asp:Button ID="btnUpload" runat="server" CssClass="btnmed btnupload" CommandName="Insert" 
                            Text="Upload File" ValidationGroup="entity" CausesValidation="false" />
                        <asp:Button ID="btnCancel" runat="server" CssClass="btnmed" CausesValidation="false" 
                            CommandName="Cancel" Text="Cancel" />
                        <asp:CustomValidator ID="CustomFileUpload" runat="server" ValidationGroup="entity" 
                            Display="Static" CssClass="validator">*</asp:CustomValidator>
                    </div>
                </div>
            </InsertItemTemplate>
        </asp:FormView>
        <div class="jqhead rounded">
            <asp:Literal ID="litImage" runat="server" />
        </div>
        <div style="visibility:hidden;"><asp:FileUpload ID="FileUploadRegisterControl" runat="server" Width="350px" CssClass="btnmed" /></div>
    </div>
</div>
