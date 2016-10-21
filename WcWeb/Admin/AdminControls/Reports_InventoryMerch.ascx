<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Reports_InventoryMerch.ascx.cs" Inherits="WillCallWeb.Admin.AdminControls.Reports_InventoryMerch" %>
<%@ Register src="~/Components/Navigation/gglPager.ascx" tagname="gglPager" tagprefix="uc2" %>
<div id="inventorymerch">
     <div class="jqhead rounded">
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="edittabl">
            <tr>
                <th style="text-align:left;">MERCHANDISE INVENTORY</th>
                <td>
                    <asp:Button ID="btnCsvAll" runat="server" CssClass="btnmed" Text="Get CSV" CommandName="csvall" OnClick="CSV_Click" 
                        OnClientClick="return confirm('This will make a download available for items in the ENTIRE BATCH. Would you like to proceed?');" />
                </td>
                <td style="width:100%;padding-left:22px;text-align:right;">
                    <asp:Button ID="btnRefresh" CausesValidation="false" CssClass="btnmed" 
                        OnClick="btnRefresh_Click" runat="server" CommandName="Refresh" Text="Refresh" />
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="edittabl">
            <tr>
                <th>Division</th>
                <td><asp:DropDownList ID="ddlDivision" Width="250px" runat="server" OnDataBinding="ddlDivision_DataBinding" 
                        OnDataBound="ddlDivision_DataBound" cssclass="fxddl"
                        OnSelectedIndexChanged="ddlDivision_SelectedIndexChanged" AutoPostBack="true" />
                </td>
                <th style="padding-left:32px;">Category</th>
                <td style="width:100%"><asp:DropDownList ID="ddlCategory" Width="250px" runat="server" OnDataBinding="ddlCategory_DataBinding" 
                        OnDataBound="ddlCategory_DataBound"  cssclass="fxddl"
                        OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged" AutoPostBack="true" />
                </td>
            </tr>
            <tr>
                <th>Delivery Type</th>
                <td><asp:DropDownList ID="ddlDeliveryType" Width="250px" runat="server" AutoPostBack="true" 
                        OnSelectedIndexChanged="ddlDeliveryType_SelectedIndexChanged" cssclass="fxddl"
                        OnDataBound="ddlDeliveryType_DataBound" OnDataBinding="ddlDeliveryType_DataBinding"/>
                </td>
                <th>Status</th>
                <td><asp:RadioButtonList ID="rdoStatus" runat="server" AutoPostBack="True" CellPadding="4" CellSpacing="4" Font-Bold="true" 
                        OnSelectedIndexChanged="rdoStatus_SelectedIndexChanged" RepeatDirection="Horizontal" RepeatLayout="Flow" >
                        <asp:ListItem Selected="True" Value="all">All &nbsp;</asp:ListItem>
                        <asp:ListItem Value="true">Active &nbsp;</asp:ListItem>
                        <asp:ListItem Value="false">InActive &nbsp;</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
        </table>
    </div> 
     <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
        <ContentTemplate>

    <div class="jqpanel1 rounded">
        <uc2:gglPager ID="GooglePager1" runat="server" PageButtonClass="btntny" />
        <asp:GridView ID="GridView1" AutoGenerateColumns="False" Width="100%" runat="server" AllowPaging="True" 
            DataSourceID="ObjectDataSource1" DataKeyNames="MerchId" OnRowDataBound="GridView1_RowDataBound" OnDataBinding="GridView1_DataBinding" 
            OnDataBound="GridView1_DataBound" OnRowCommand="GridView1_RowCommand"
            ShowFooter="false" EnableViewState="false" OnInit="GridView1_Init" CssClass="lsttbl">
            <PagerSettings Visible="false" />
            <AlternatingRowStyle BackColor="#e1e1e1" />
            <EmptyDataTemplate>
                <div class="lstempty">No Data For Selected Criteria</div>
            </EmptyDataTemplate>
            <Columns>
                <asp:TemplateField ItemStyle-HorizontalAlign="center" ItemStyle-CssClass="center" >
                    <ItemTemplate>
                        <asp:Button id="btnSales" runat="server" CommandName="getsales" CommandArgument='<%#Eval("MerchId") %>' CssClass="btnmed"
                            Text="Sales" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Edit" HeaderStyle-HorizontalAlign="left">
                    <ItemTemplate>
                        <asp:HyperLink ID="linkEdit" runat="server" Text='<%#Eval("MerchName") %>' NavigateUrl='<%# "/Admin/MerchEditor.aspx?p=itemedit&merchitem=" + Eval("MerchId") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Division" HeaderStyle-HorizontalAlign="Left">
                    <ItemTemplate><asp:Literal ID="litDivision" runat="server" /></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Category" HeaderStyle-HorizontalAlign="Left">
                    <ItemTemplate><asp:Literal ID="litCategory" runat="server" /></ItemTemplate>
                </asp:TemplateField>
                <asp:CheckBoxField DataField="IsActive" HeaderText="Active" ItemStyle-HorizontalAlign="center" />
                <asp:CheckBoxField DataField="IsSoldOut" HeaderText="SoldOut" ItemStyle-HorizontalAlign="center" />
                <asp:CheckBoxField DataField="IsFeatured" HeaderText="Featured" ItemStyle-HorizontalAlign="center" />
                <asp:BoundField DataField="Allot" HeaderText="Allot" ItemStyle-HorizontalAlign="center" />
                <asp:BoundField DataField="Dmg" HeaderText="Dmg" ItemStyle-HorizontalAlign="center" />
                <asp:BoundField DataField="Pend" HeaderText="Pend" ItemStyle-HorizontalAlign="center" />
                <asp:BoundField DataField="Sold" HeaderText="Sold" ItemStyle-HorizontalAlign="center" />
                <asp:BoundField DataField="Avail" HeaderText="Avail" ItemStyle-HorizontalAlign="center" />
                <asp:BoundField DataField="Weight" DataFormatString="{0:n}" HtmlEncode="false" HeaderText="Weight" ItemStyle-HorizontalAlign="center" />
                <asp:BoundField DataField="Price" DataFormatString="{0:n}" HtmlEncode="false" HeaderText="Each" ItemStyle-HorizontalAlign="center" />
            </Columns>
        </asp:GridView>
    </div>

        </ContentTemplate>
    </asp:UpdatePanel>
</div>
 <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" EnablePaging="True"
    SelectCountMethod="GetMerchParents_ByDivCat_Count" SelectMethod="GetMerchParents_ByDivCat" TypeName="Wcss.QueryRow.InventoryMerchItemRow"
    OnSelecting="objData_Selecting" OnSelected="objData_Selected" >
    <SelectParameters>
        <asp:ControlParameter ControlID="ddlDivision" DefaultValue="0" Name="divId" PropertyName="SelectedValue" Type="Int32" />
        <asp:ControlParameter ControlID="ddlCategory" DefaultValue="0" Name="catId" PropertyName="SelectedValue" Type="Int32" />
        <asp:ControlParameter ControlID="ddlDeliveryType" DefaultValue="" Name="delivery" PropertyName="SelectedValue" Type="string" />
        <asp:ControlParameter ControlID="rdoStatus" DefaultValue="All" Name="activeStatus" PropertyName="SelectedValue" Type="string" />
    </SelectParameters>
</asp:ObjectDataSource>
                
    


