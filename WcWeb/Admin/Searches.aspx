<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Searches.aspx.cs" Inherits="WillCallWeb.Admin.Searches" Title="Admin - Search Report" 
 MasterPageFile="~/TemplateAdmin.master" %>
<asp:Content ID="MainContent" runat="server" ContentPlaceHolderID="MainContent">
    <div id="searches">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="jqhead rounded"><div class="sectitle">Searches</div></div>
                <asp:GridView ID="GridView1" runat="server" AllowPaging="True" Width="100%" AutoGenerateColumns="true" DataKeyNames="Id" DataSourceID="SqlDataSource1"
                    PageSize="25" PagerSettings-Position="Top" CssClass="lsttbl" >
                    <PagerStyle CssClass="pgr" />
                    <SelectedRowStyle CssClass="selected" />
                    <EmptyDataTemplate>
                        <div class="lstempty">No Searches</div>
                    </EmptyDataTemplate>
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server"
        ConnectionString="<%$ ConnectionStrings:WillCallConnectionString %>" 
        SelectCommand="SELECT DISTINCT * FROM [Search] WHERE [ApplicationId] = @ApplicationId ORDER BY [dtStamp] DESC"
         OnSelecting="SqlDataSource1_Selecting">
        <SelectParameters>
            <asp:Parameter Name="ApplicationId" DbType="Guid" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>