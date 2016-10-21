<%@ Control Language="C#" AutoEventWireup="true" EnableViewState="false" CodeFile="MerchHeierarchy.ascx.cs" Inherits="WillCallWeb.Components.Util.MerchHeierarchy" %>
<div id="<%=this.ContainerId %>" class="merch-hire-container">
    <asp:Panel ID="pnlDivision" runat="server" CssClass="hire-select">
        <span class="pnl-title">Division</span>
        <asp:DropDownList ID="ddlDivision" runat="server" DataSourceID="SqlDivision" DataTextField="Name" DataValueField="Id" 
            OnDataBound="ddlDivision_DataBound" Width="100%" AutoPostBack="true" /> 
    </asp:Panel>
    <asp:Panel ID="pnlCategorie" runat="server" CssClass="hire-select">
        <span class="pnl-title">Categorie</span>
        <asp:DropDownList ID="ddlCategorie" runat="server" DataSourceID="SqlCategorie" DataTextField="Name" DataValueField="Id"
            OnDataBound="ddlCategorie_DataBound" Width="100%" AutoPostBack="true" >
        </asp:DropDownList>
    </asp:Panel>
    <asp:ValidationSummary ID="ValidationSummary1" CssClass="validationsummary" HeaderText="Please fix the following errors:"
        ValidationGroup="ordinal" runat="server" />
    <asp:CustomValidator ID="ControlValidation" runat="server" ValidationGroup="ordinal" CssClass="validator" Display="None" 
        ErrorMessage="CustomValidator"></asp:CustomValidator>
    <asp:Repeater ID="rptList" runat="server" DataSourceID="SqlList" OnDataBinding="rptList_DataBinding" OnItemDataBound="rptList_ItemDataBound" >
        <HeaderTemplate>
        <div class="ordinal-wrapper">                            
            <ul>
                <li id='ordinalheader-<%=this.OrdinalContext.ToString() %>' class="ordinal-header-row">                                    
                    <span class="ordinal-row">#</span><span class="ordinal-info">&nbsp;</span>
                    <span class="cmdsection">                                     
                        <asp:Button ID="btnAddNew" runat="server" Text="Add New" CssClass="btnmed ov-formtrigger" 
                            tooltip="Create New Thing" 
                            CommandName="AddNew" CommandArgument="0" />
                    </span>
                </li>
        </HeaderTemplate>
        <ItemTemplate>
                <asp:Literal ID="litLIStart" runat="server" />                
                    <span class="ordinal-row"><asp:literal ID="litRowNum" runat="server" /></span>                    
                    <asp:CustomValidator ID="CustomValidation" runat="server" ValidationGroup="ordinal" CssClass="validator" Display="Static" 
                         ErrorMessage="CustomValidator">*</asp:CustomValidator>
                    <%if (this.OrdinalContext != Wcss._Enums.OrdinalContext.merchjoincat)
                      {%>
                    <span class="ordinal-description"><asp:literal ID="litDescription" runat="server" /></span> 
                    <%} %>
                    <span class="ordinal-name"><%#Eval("Name") %></span>
                    <span class="ordinal-info"><asp:literal ID="litInfo" runat="server" /></span> 
                    <span class="ordinal-cmd">
                        <%if (this.OrdinalContext != Wcss._Enums.OrdinalContext.merchjoincat)
                            {%>
                        <a href="" id="lnkEdit_<%#Eval("Id") %>" title="Edit" rel="<%#Eval("Id") %>" class="ordinal-edit-link ov-formtrigger-link">Edit</a>
                        <%} %>
                        <span class="ordinal-cmd-delete">
                            <asp:literal ID="litDelete" runat="server" />
                        </span>
                    </span>
                </li>
        </ItemTemplate>
        <FooterTemplate>
            </ul>
        </div>
        </FooterTemplate>
    </asp:Repeater>
   
<input type="hidden" id="hidClientId-<%=this.ContainerId %>" class="client-id" value='<%=this.UniqueID %>' />
</div>

<asp:SqlDataSource ID="SqlDivision" runat="server" EnableCaching="false" ConnectionString="<%$ ConnectionStrings:WillCallConnectionString %>"     
    SelectCommand="SELECT * FROM [MerchDivision] div WHERE div.[ApplicationId] = @appId ORDER BY [iDisplayOrder] ASC" 
    OnSelecting="SqlDivision_Selecting">
    <SelectParameters>
        <asp:Parameter Name="appId" DbType="Guid" />
    </SelectParameters>
</asp:SqlDataSource>
<asp:SqlDataSource ID="SqlCategorie" runat="server" EnableCaching="false" ConnectionString="<%$ ConnectionStrings:WillCallConnectionString %>"     
    SelectCommand="SELECT * FROM [MerchCategorie] cat WHERE cat.[tMerchDivisionId] = @divId ORDER BY [iDisplayOrder] ASC" >
    <SelectParameters>
        <asp:ControlParameter ControlID="ddlDivision" PropertyName="SelectedValue" Name="divId" DbType="Int32" />
    </SelectParameters>
</asp:SqlDataSource>
<asp:SqlDataSource ID="SqlList" runat="server" EnableCaching="false" ConnectionString="<%$ ConnectionStrings:WillCallConnectionString %>"     
    SelectCommand="SELECT 0" 
    OnSelecting="SqlList_Selecting" OnInit="SqlList_Init">
</asp:SqlDataSource>