<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Download_Item.ascx.cs" Inherits="WillCallWeb.Admin.AdminControls.Downloads.Download_Item" %>

<div id="srceditor">
    <div id="merchitem">
        <div class="lngtitle">
            <%if (Atx.CurrentDownloadRecord != null && FormView1.CurrentMode != FormViewMode.Insert){%>
                <%= Atx.CurrentDownloadRecord.Id.ToString()%> - <%=Atx.CurrentDownloadRecord.FileName%>
            <%} else {%>Add A New Item<%}%>
        </div>
        <div class="jqhead rounded">
            <asp:ValidationSummary ID="ValidationSummary1" CssClass="validationsummary" HeaderText="" ValidationGroup="merch" runat="server" />
            <div class="cmdsection">
                <asp:Button ID="btnSave" ValidationGroup="merch" CausesValidation="false" runat="server" CommandName="Update" 
                    Text="Save" CssClass="btntny" OnClick="btnSave_Click" />
                <asp:Button ID="btnCancel" CausesValidation="false" runat="server" CommandName="Cancel" 
                    Text="Cancel" CssClass="btntny" OnClick="btnCancel_Click" />
                <asp:Button ID="btnDelete" CausesValidation="false" runat="server" CommandName="Delete" 
                    Text="Delete" CssClass="btntny" OnClick="btnDelete_Click" />
                <asp:Button ID="btnNew" CausesValidation="false" runat="server" CommandName="New" 
                    Text="New" CssClass="btntny" OnClick="btnNew_Click" />
                <asp:CustomValidator ID="CustomValidation" Display="static" runat="server" ValidationGroup="merch" CssClass="validator">*</asp:CustomValidator>
            </div>
            <asp:FormView ID="FormView1" Width="100%" runat="server" DataKeyNames="Id" DefaultMode="Edit" 
                OnDataBinding="FormView1_DataBinding" 
                OnDataBound="FormView1_DataBound" 
                OnItemCommand="FormView1_ItemCommand" 
                OnItemDeleting="FormView1_ItemDeleting" 
                OnItemInserting="FormView1_ItemInserting" 
                OnItemUpdating="FormView1_ItemUpdating" 
                OnModeChanging="FormView1_ModeChanging">
                <EditItemTemplate>
                    <table border="0" cellspacing="0" cellpadding="0" width="100%" class="edittabl">
                        <tr>
                            <th>Name</th>
                            <td colspan="5"><asp:TextBox ID="txtName" runat="server" Width="100%" MaxLength="256" Text='<%#Bind("FileName") %>'></asp:TextBox></td>
                        </tr>
                        <tr>
                            <th>Short Desc</th>
                            <td colspan="5"><asp:TextBox ID="txtShortDescription" runat="server" Width="100%" MaxLength="300" Text='<%#Bind("TrackContext") %>'></asp:TextBox></td>
                        </tr>
                        <tr>
                            <th>Inventory</th>
                            <td colspan="5">
                                <table border="0" cellspacing="0" cellpadding="0" >
                                    <tr>
                                        <th>Seconds</th>
                                        <td style="padding-right:16px;"><%#Eval("FileSeconds")%></td>
                                        <th>Bytes</th>
                                        <td style="padding-right:16px;"><%#Eval("FileBytes")%></td>
                                        <th>Sample Clicks</th>
                                        <td style="padding-right:16px;"><%#Eval("SampleClick")%></td>
                                        <th>Attempted</th>
                                        <td style="padding-right:16px;"><%#Eval("Attempted")%></td>
                                        <th>Successful</th>
                                        <td style="padding-right:16px;"><%#Eval("Successful")%></td>   
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </EditItemTemplate>
                <InsertItemTemplate>
                    <table border="0" cellspacing="0" cellpadding="0" width="100%" class="edittabl">
                        <tr>
                            <th>Name</th>
                            <td colspan="5"><asp:TextBox ID="txtName" runat="server" Width="100%" MaxLength="256" Text='<%#Bind("FileName") %>'></asp:TextBox></td>
                        </tr>
                        <tr>
                            <th>Short Desc</th>
                            <td colspan="5"><asp:TextBox ID="txtShortDescription" runat="server" Width="100%" MaxLength="300" Text='<%#Bind("TrackContext") %>'></asp:TextBox></td>
                        </tr>
                        <tr>
                            <th>Inventory</th>
                            <td colspan="5">
                                <table border="0" cellspacing="0" cellpadding="0" >
                                    <tr>
                                        <th>Seconds</th>
                                        <td style="padding-right:16px;"><%#Eval("FileSeconds")%></td>
                                        <th>Bytes</th>
                                        <td style="padding-right:16px;"><%#Eval("FileBytes")%></td>
                                        <th>Sample Clicks</th>
                                        <td style="padding-right:16px;"><%#Eval("SampleClick")%></td>
                                        <th>Attempted</th>
                                        <td style="padding-right:16px;"><%#Eval("Attempted")%></td>
                                        <th>Successful</th>
                                        <td style="padding-right:16px;"><%#Eval("Successful")%></td>   
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </InsertItemTemplate>
            </asp:FormView>
        </div>
    </div>
</div>