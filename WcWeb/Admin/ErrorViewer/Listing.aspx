<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Listing.aspx.cs" EnableViewState="false" Inherits="WillCallWeb.Admin.ErrorViewer.Listing" Title="Admin - Error Viewer Reports" 
 MasterPageFile="~/TemplateAdmin.master" %>
<asp:Content ID="MainContent" runat="server" ContentPlaceHolderID="MainContent">
    <div id="errorlisting">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="jqhead rounded">
                    <div class="sectitle">Error Listing
                        <asp:Button ID="btnArchive" CssClass="btntny" runat="server" Text="Archive Events" OnClick="btnArchive_Click" CausesValidation="false" 
                            OnClientClick='return confirm("Are you sure you want to archive all events? This will archive ALL events - not just the ones being viewed.")' />
                    </div>
                </div>
                <div style="border:none red 2px;position:relative;float:left; width:1280px;margin-top:10px;">
		                    <div style="border:none blue 2px;position:relative;float:left; width:600px;">
		                        
		                        <asp:FormView ID="FormView1" runat="server" DefaultMode="ReadOnly" DataKeyNames="Id" BackColor="#ffffff" 
		                            DataSourceID="SqlDataSource2" Width="100%" Font-Size="10px">
		                            <ItemTemplate>
		                                    <table border="1" cellspacing="0" cellpadding="3" >
		                                        <tr>
		                                            <th>ID:</th>
		                                            <td><%# Eval("Id") %></td>
		                                            <th>Date:</th>
		                                            <td style="width: 100%;"><%# Eval("Date") %></td>
		                                        </tr>
		                                        <tr>
		                                            <th>UserInfo:</th>
		                                            <td><%# Eval("Email") %></td>
		                                            <th>IP:</th>
		                                            <td><%# Eval("IpAddress") %></td>
		                                        </tr>
		                                        <tr><th valign="top">Source:</th><td colspan="3"><%# System.Web.HttpUtility.HtmlEncode(Eval("Source").ToString()) %></td></tr>
		                                        <tr><th valign="top">TargetSite:</th><td colspan="3"><%# System.Web.HttpUtility.HtmlEncode(Eval("TargetSite").ToString()) %></td></tr>
		                                        <tr>
		                                            <th valign="top">Referrer:</th><td><%# System.Web.HttpUtility.HtmlEncode(Eval("Referrer").ToString()) %></td>
		                                            <th valign="top">QueryString:</th><td><%# System.Web.HttpUtility.HtmlEncode(Eval("QueryString").ToString()) %></td>
		                                        </tr>
		                                        <tr><th>Form:</th><td colspan="3"><%# System.Web.HttpUtility.HtmlEncode(Eval("Form").ToString())%>&nbsp;</td></tr>
							                                        <tr><th valign="top">Message:</th><td colspan="3" style="overflow:auto;word-wrap:break-word;max-width:500px;"><%# System.Web.HttpUtility.HtmlEncode(Eval("Message").ToString()) %></td></tr>
							                                        <tr><td colspan="4" ><div style="word-wrap:break-word;overflow-x:scroll;min-height:120px;max-width:600px;"><%# System.Web.HttpUtility.HtmlEncode(Eval("StackTrace").ToString())%></div></td></tr>
                                    </table>
		                            </ItemTemplate>
		                        </asp:FormView>
		                        &nbsp;
		                    </div>
		                    <div style="border:none green 2px;position:relative;float:left; width:600px;margin-left:20px;">
		                        <asp:GridView ID="GridView1" runat="server" AllowPaging="True" Width="600px" AutoGenerateColumns="False" Font-Size="10px" 
		                            DataKeyNames="Id" DataSourceID="SqlDataSource1" PageSize="25" PagerSettings-Position="Top" BackColor="#ffffff" 
		                            OnDataBound="GridView1_DataBound" >
		                            <PagerStyle CssClass="pgr" Font-Size="18px"  />
		                            <SelectedRowStyle CssClass="selected" />
		                            <Columns>
		                                <asp:TemplateField ItemStyle-ForeColor="Black">
		                                    <ItemTemplate>
		                                        <div>
		                                            <span style="width:80px;padding:0 8px;">
		                                                <asp:Button ID="btnSelect" runat="server" Text='<%#Eval("Id") %>' CommandName="Select" 
		                                                    ToolTip="Select" cssClass="btntny" />
		                                            </span>
		                                            <span style="width:100%;padding:0 8px;"><%#Eval("Date", "{0:MM/dd/yyyy hh:mm:ss tt}") %></span>
		                                            <span style="float:right;padding:0 8px;text-align:right;"><%#Eval("ApplicationName") %></span>
		                                        </div>
		                                        <div style="clear:both;">
		                                            <span style="width:80px;padding:0 8px;">MSG:</span>
		                                            <div style="padding:0 8px;word-wrap:break-word;overflow-x:scroll;max-width:600px;"><%#System.Web.HttpUtility.HtmlEncode(Eval("Message").ToString()) %></div>
		                                        </div>                                            
		                                    </ItemTemplate>
		                                </asp:TemplateField>
		                            </Columns>
		                        </asp:GridView>
		                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server"
        ConnectionString="<%$ ConnectionStrings:ErrorLogConnectionString %>" 
        SelectCommand="SELECT DISTINCT [Id], [Date], [Message], [ApplicationName] FROM [Log] WHERE [ApplicationName] = @ApplicationName ORDER BY [Date] DESC"
         OnSelecting="SqlDataSource1_Selecting">
        <SelectParameters>
            <asp:Parameter Name="ApplicationName" DbType="String" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSource2" runat="server" 
        ConnectionString="<%$ ConnectionStrings:ErrorLogConnectionString %>" 
        SelectCommand="SELECT [Id], [Date], [Source], [Message], [Referrer], [StackTrace], [TargetSite], [Querystring], [Form], 
        [IpAddress], [Email], [ApplicationName] FROM [Log] WHERE ([Id] = @Id)">
        <SelectParameters>
            <asp:ControlParameter ControlID="GridView1" Name="Id" PropertyName="SelectedValue" Type="Int32" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>
