<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Prom_BannerOrder.ascx.cs" Inherits="WillCallWeb.Admin.AdminControls.Prom_BannerOrder" %>
<div id="srceditor">
    <div id="promotion">
        <div class="jqhead rounded">
            <h3 class="entry-title">Banner Ordering</h3>
        </div>
        <asp:GridView ID="GridView1" runat="server" DataSourceID="SqlList" Width="100%" CssClass="lsttbl" DataKeyNames="Id" 
            AllowPaging="False" AutoGenerateColumns="False" GridLines="Both"
            OnRowCommand="GridView1_RowCommand"
            OnDataBinding="GridView_DataBinding" OnRowCreated="GridView1_RowCreated"
            OnRowDataBound="GridView_RowDataBound" 
            OnDataBound="GridView_DataBound">           
           <PagerSettings Visible="false" />
           <SelectedRowStyle CssClass="selected" />
           <EmptyDataTemplate>
                <div class="lstempty">No Banners</div>
            </EmptyDataTemplate>
           <Columns>
               <asp:TemplateField HeaderText="#" ItemStyle-HorizontalAlign="center" ItemStyle-CssClass="rowcounter">
                    <ItemTemplate><asp:Literal ID="LiteralRowCounter" runat="server" /></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ItemStyle-Wrap="false" HeaderText="Select" >
                    <ItemTemplate>
                        <asp:LinkButton Width="20px" Id="btnUp" ToolTip="Move Up" CssClass="btnup" runat="server" CommandName="Up" 
                            CausesValidation="false" />
                       <asp:LinkButton Width="20px" Id="btnDown" ToolTip="Move Down" CssClass="btndown" runat="server" CommandName="Down" 
                            CausesValidation="false" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:CheckBoxField ReadOnly="true" DataField="bActive" HeaderText="Active" ItemStyle-HorizontalAlign="Center" />
                <asp:TemplateField HeaderText="Description" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="100%">
                    <ItemTemplate>
                        <asp:Literal ID="litNaming" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Banner" HeaderStyle-HorizontalAlign="Left">
                    <ItemTemplate>
                        <asp:Literal ID="litImage" runat="server" />
                        <img src="/Images/spacer.gif" alt="" height="1px" width="190px" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Dates" HeaderStyle-HorizontalAlign="Left" ItemStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Literal ID="litDates" runat="server" />
                        <img src="/Images/spacer.gif" alt="" height="1px" width="100px" />
                    </ItemTemplate>
                </asp:TemplateField>
           </Columns>
       </asp:GridView>
    </div>
</div>
<asp:SqlDataSource ID="SqlList" runat="server" EnableCaching="false" ConnectionString="<%$ ConnectionStrings:WillCallConnectionString %>"     
    SelectCommand="SELECT * FROM [SalePromotion] sp WHERE sp.[ApplicationId] = @appId AND [bActive] = 1 
        AND ([dtEndDate] IS NULL OR ([dtEndDate] IS NOT NULL AND [dtEndDate] > getDate())) ORDER BY [iDisplayOrder] DESC" 
    OnSelecting="SqlList_Selecting" OnSelected="SqlList_Selected">
    <SelectParameters>
        <asp:Parameter Name="appId" DbType="Guid" />
    </SelectParameters>
</asp:SqlDataSource>