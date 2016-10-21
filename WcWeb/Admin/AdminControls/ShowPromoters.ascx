<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ShowPromoters.ascx.cs" Inherits="WillCallWeb.Admin.AdminControls.ShowPromoters" %>
<%@ Register Src="Editor_Promoter.ascx" TagName="Editor_Promoter" TagPrefix="uc1" %>
<div id="srceditor">
    <div id="showpromoters">
        <div class="jqhead rounded">
            <h3 class="entry-title">Promoters - <asp:Literal ID="litShowTitle" runat="server" /></h3>        
            <asp:GridView ID="GridView1" Width="100%" runat="server" AutoGenerateColumns="False" DataKeyNames="Id" ShowFooter="False"
                CssClass="lsttbl" 
                OnRowCommand="GridView1_RowCommand" 
                OnSelectedIndexChanged="GridView1_SelectedIndexChanged" 
                OnRowDeleting="GridView1_RowDeleting" 
                OnDataBinding="GridView1_DataBinding" 
                OnDataBound="GridView1_DataBound" 
                OnRowDataBound="GridView1_RowDataBound" >
                <SelectedRowStyle CssClass="selected" />
                <EmptyDataTemplate>
                    <div class="lstempty">There Are No Promoters For This Show</div>
                </EmptyDataTemplate>
                <Columns>
                    <asp:TemplateField ItemStyle-Wrap="false" HeaderText="LINKS">
                        <ItemTemplate>
                            <asp:LinkButton Width="20px" Id="btnSelect" ToolTip="Select" CssClass="btnselect" runat="server" CommandName="Select" 
                                CommandArgument='<%#Eval("Id") %>' CausesValidation="false" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Description" ItemStyle-Width="100%" HeaderStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Literal ID="litText" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-Wrap="false" >
                       <ItemTemplate>
                            <asp:LinkButton Width="20px" Id="btnUp" ToolTip="Move Up" CssClass="btnup" runat="server" CommandName="Up" 
                                CommandArgument='<%#Eval("Id") %>' CausesValidation="false" />
                            <asp:LinkButton Width="20px" Id="btnDown" ToolTip="Move Down" CssClass="btndown" runat="server" CommandName="Down" 
                                CommandArgument='<%#Eval("Id") %>' CausesValidation="false" />
                           <asp:LinkButton Width="20px" Id="btnDelete" CssClass="btndelete lastinrow" runat="server" CommandName="Delete" 
                                CommandArgument='<%#Eval("Id") %>' ToolTip="Delete" CausesValidation="false"
                               OnClientClick='return confirm("Are you sure you want to delete this row?")' />
                       </ItemTemplate>
                   </asp:TemplateField>
                </Columns>
            </asp:GridView>            
            <asp:ValidationSummary ID="ValidationSummary2" CssClass="validationsummary" HeaderText="" ValidationGroup="promo" runat="server" />
        </div>

        <asp:FormView Width="100%" ID="FormView1" runat="server" DefaultMode="Edit" 
            OnDataBinding="FormView1_DataBinding" 
            OnItemCommand="FormView1_ItemCommand" 
            OnItemInserting="FormView1_ItemInserting" 
            OnDataBound="FormView1_DataBound"
            OnModeChanging="FormView1_ModeChanging" 
            OnItemUpdating="FormView1_ItemUpdating">
            <EmptyDataTemplate>
                <div class="jqpnl rounded iit">
                    <div class="cmdsection">
                        <h3 class="entry-title" style="display:inline-block;">Add A New Promoter...</h3>
                        <asp:Button Id="btnNew" CausesValidation="false" runat="server" CommandName="New" Text="New"
                            cssclass="btnmed" />  
                        <asp:Button ID="btnDisplay" runat="server" CausesValidation="false" CommandName="view" Text="Display Show" 
                            CssClass="btnmed" OnClientClick='doPagePopup("/Admin/ShowDisplayer.aspx", "false"); ' />
                    </div>
                </div>
            </EmptyDataTemplate>
            <EditItemTemplate>
                <div class="jqpnl rounded iit">
                    <div class="cmdsection">
                        <asp:Button ID="btnSave" CausesValidation="false" runat="server" 
                            CommandName="Update" Text="Save" CssClass="btnmed" />
                        <asp:Button ID="btnCancel" runat="server" CommandName="Cancel" Text="Cancel" CausesValidation="false" 
                             CssClass="btnmed" />
                        <asp:Button ID="btnNew" runat="server" CommandName="New" Text="New" CausesValidation="false"
                             CssClass="btnmed" />
                        <asp:CustomValidator ID="CustomValidation" runat="server" 
                            ValidationGroup="promo" CssClass="invisible" Display="Static" ErrorMessage="CustomValidator">*</asp:CustomValidator>
                        <asp:Button ID="btnDisplay" runat="server" CausesValidation="false" CommandName="view" Text="Display Show" 
                            CssClass="btnmed" OnClientClick='doPagePopup("/Admin/ShowDisplayer.aspx", "false"); ' />
                    </div>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" class="edittabl" >
                        <tr>
                            <th>Pre</th>
                            <td style="width:100%;"><asp:TextBox ID="txtPreText" MaxLength="300" Width="650px" runat="server" Text='<%#Bind("PreText") %>' /></td>
                        </tr>
                        <tr>
                            <th>Promo Text</th>
                            <td><asp:TextBox ID="txtPromoterText" MaxLength="300" Width="650px" runat="server" Text='<%#Bind("PromoterText") %>' /></td>
                        </tr>
                        <tr>
                            <th>Post</th>
                            <td><asp:TextBox ID="txtPostText" MaxLength="300" Width="650px" runat="server" Text='<%#Bind("PostText") %>' /></td>
                        </tr>   
                    </table>
                    <uc1:Editor_Promoter ID="Editor_Promoter1" runat="server" SelectedIdx='<%#Eval("tPromoterId") %>' AllowSelect="false" 
                        TitleText="" DisplayTitle="false" MaxImageDimension="100" />
                </div>
            </EditItemTemplate>
            <InsertItemTemplate>
                <div class="jqpnl rounded iit">
                    <uc1:Editor_Promoter ID="Editor_Promoter1" runat="server" AllowSelect="true" 
                        TitleText="" DisplayTitle="false" MaxImageDimension="100" />                    
                    <div class="cmdsection">
                        <asp:Button ID="btnInsert" CausesValidation="false" runat="server" 
                            CommandName="Insert" Text="Add Promoter" CssClass="btnmed" />
                        <asp:Button ID="btnCancel" runat="server" CommandName="Cancel" Text="Cancel"
                             CssClass="btnmed" />
                        <asp:CustomValidator ID="CustomValidation" runat="server" 
                            ValidationGroup="promo" CssClass="invisible" Display="Static" ErrorMessage="CustomValidator">*</asp:CustomValidator>
                    </div>
                </div>
            </InsertItemTemplate>
        </asp:FormView>
    </div>
</div>