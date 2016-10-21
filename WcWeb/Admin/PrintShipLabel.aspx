<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PrintShipLabel.aspx.cs" MasterPageFile="~/TemplatePrint.master" 
Inherits="WillCallWeb.Admin.PrintShipLabel" Title="Admin - Packing List" %>


<asp:Content ID="MainContent" runat="server" ContentPlaceHolderID="MainContent">

     <ul>
        <li>allow choice for label type</li>
        <li>allow to remove label spots</li>
        <li>popup another window that auto prints</li>
        <li>allow mark as printed</li>
     </ul>
     
    <fieldset>
        <legend class="controlheader">
            <span class="title">Configure Label Page</span>
            <span class="range">
                Order:
                <asp:DropDownList ID="ddlOrder" runat="server" AutoPostBack="True"
                    OnSelectedIndexChanged="ddlOrder_SelectedIndexChanged" >
                    <asp:ListItem Text="Oldest to newest" Value="ASC" Selected="true" />
                    <asp:ListItem Text="Newest to oldest" Value="DESC" />
                </asp:DropDownList>
                Rows:
                <asp:DropDownList ID="ddlRows" runat="server" AutoPostBack="True"
                    OnDataBinding="ddlRows_DataBinding" OnSelectedIndexChanged="ddlRows_SelectedIndexChanged" />
                Cols:
                <asp:DropDownList ID="ddlCols" runat="server" AutoPostBack="True"
                    OnDataBinding="ddlCols_DataBinding" OnSelectedIndexChanged="ddlCols_SelectedIndexChanged" />
                Margins: text boxes?
            </span>
        </legend>
        
        <asp:Repeater ID="rptLabels" runat="server" >
            <ItemTemplate>
                
            </ItemTemplate>
        </asp:Repeater>
        
        <asp:DataList ID="dlLabel" runat="server" OnDataBinding="dlLabel_DataBinding" OnItemCommand="dlLabel_ItemCommand" 
            OnItemDataBound="dlLabel_ItemDataBound">
            <ItemTemplate>
                <div>
                    <div class=""><asp:CheckBox ID="chkActive" runat="server" /></div>
                    <div><%#Eval("CompanyName") %></div>
                    <div><%#Eval("Name") %></div>
                    <div><%#Eval("Address1") %></div>
                    <div><%#Eval("Address2") %></div>
                    <div><%#Eval("City") %>, <%#Eval("State") %></div>
                    <div><%#Eval("Zip") %>, <%#Eval("Country") %></div>
                </div>
            </ItemTemplate>
        </asp:DataList>
        
        
        </fieldset>
     
     
</asp:Content>